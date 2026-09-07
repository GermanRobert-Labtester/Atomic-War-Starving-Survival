// SPDX-License-Identifier: MIT
// Plan 205 — cargo airdrop engine: scheduling, deterministic drift and
// quantized landing, damage curve, beacon lifecycle, interception race,
// partial recovery with capacity, contents generated once, save safety.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public class CargoAirdropEngineTests
    {
        private static CargoAirdropCatalog CreateCatalog() => new CargoAirdropCatalog
        {
            descent_bands = 3,
            max_active_drops = 2,
            interception = new AirdropInterceptionDef { rate_per_day_bp = 1000, expired_grace_days = 2 },
            drop_profiles = new List<AirdropProfileDef>
            {
                new AirdropProfileDef
                {
                    drop_profile_id = "drop_medical",
                    trigger_signal_outcomes = new List<string> { "survivor_community" },
                    cargo_pool_id = "pool_med",
                    wind_sensitivity = 1.0f,
                    base_impact_kph = 20f,
                    wind_impact_factor_kph = 0.5f,
                    light_impact_kph = 25f,
                    heavy_impact_kph = 40f,
                    integrity_loss_light_pct = 15,
                    integrity_loss_heavy_pct = 50,
                    beacon_duration_days = 4,
                    interception_rate_per_day_bp = 1000
                }
            },
            cargo_pools = new List<AirdropCargoPoolDef>
            {
                new AirdropCargoPoolDef
                {
                    cargo_pool_id = "pool_med",
                    entries = new List<AirdropCargoEntryDef>
                    {
                        new AirdropCargoEntryDef { item_id = "antibiotics", quantity = 4, weight_kg_per_unit = 0.05f },
                        new AirdropCargoEntryDef { item_id = "medkit", quantity = 3, weight_kg_per_unit = 0.4f },
                        new AirdropCargoEntryDef { item_id = "item_surgical_kit", quantity = 1, weight_kg_per_unit = 1.5f }
                    }
                }
            }
        };

        private static CargoAirdropSystem MakeSystem(int seed, int day = 1)
        {
            var sys = new CargoAirdropSystem(new SeededRng(seed));
            sys.BindCatalog(CreateCatalog());
            sys.DayProvider = () => day;
            return sys;
        }

        // ── Scheduling validation ───────────────────────────────────────

        [Fact]
        public void Schedule_RejectsUnknownProfile_AndMissingSignal()
        {
            var sys = MakeSystem(42);
            Assert.False(sys.ScheduleDrop("drop_nonexistent", "sig_1", 0, 0).IsSuccess);
            Assert.False(sys.ScheduleDrop("drop_medical", "", 0, 0).IsSuccess);
            Assert.Empty(sys.State.drops);
        }

        [Fact]
        public void Schedule_EnforcesMaxActiveDrops()
        {
            var sys = MakeSystem(42, day: 1);
            Assert.True(sys.ScheduleDrop("drop_medical", "sig_1", 0, 0).IsSuccess);
            Assert.True(sys.ScheduleDrop("drop_medical", "sig_2", 1, 1).IsSuccess);
            var blocked = sys.ScheduleDrop("drop_medical", "sig_3", 2, 2);
            Assert.Equal(AirdropFailures.MaxActiveDrops, blocked.FailureCode);
        }

        // ── Deterministic drift & quantized landing ─────────────────────

        [Fact]
        public void Drift_IsDeterministic_Quantized_AndWindProportional()
        {
            var a = MakeSystem(42, day: 1);
            a.WindDirectionDeg = () => 0f;  // blowing toward +x
            a.WindSpeedKph = () => 10f;
            Assert.True(a.ScheduleDrop("drop_medical", "sig_a", 0, 0).IsSuccess);
            var dropA = a.State.drops[0];

            // Wind at 10 kph toward +x, sensitivity 1.0, 3 bands → drift 30 km.
            Assert.Equal(30, dropA.landing_x);
            Assert.Equal(0, dropA.landing_y);
            Assert.Equal(0f, dropA.wind_direction_deg, 1);
            Assert.Equal(10f, dropA.wind_speed_kph, 1);

            // Same seed + same wind = same landing (split-run convergence).
            var b = MakeSystem(42, day: 1);
            b.WindDirectionDeg = () => 0f;
            b.WindSpeedKph = () => 10f;
            Assert.True(b.ScheduleDrop("drop_medical", "sig_b", 0, 0).IsSuccess);
            Assert.Equal(dropA.landing_x, b.State.drops[0].landing_x);
            Assert.Equal(dropA.landing_y, b.State.drops[0].landing_y);

            // Quantization: coordinates are integers.
            Assert.IsType<int>(dropA.landing_x);
        }

        // ── Descent, landing, damage curve ──────────────────────────────

        [Fact]
        public void Descent_TakesCatalogBands_ThenLands_WithIntegrityFromCurve()
        {
            var sys = MakeSystem(42, day: 1);
            sys.WindSpeedKph = () => 2f; // light impact (20 + 1 < 25) → integrity 100
            Assert.True(sys.ScheduleDrop("drop_medical", "sig_1", 0, 0).IsSuccess);
            var drop = sys.State.drops[0];
            Assert.Equal("descending", drop.phase);
            Assert.Equal(3, drop.current_band);

            sys.TickDay(2);
            Assert.Equal("descending", drop.phase);
            sys.TickDay(3);
            sys.TickDay(4);
            Assert.Equal("landed", drop.phase);
            Assert.True(drop.beacon_active);
            Assert.Equal(8, drop.beacon_expires_day); // landing day 4 + 4-day beacon
            Assert.Equal(100, drop.cargo_integrity_pct);

            // Landing day: release 1 + 3 bands = day 4.
            Assert.Equal(4, drop.landing_day);
        }

        [Fact]
        public void HighWind_DegradesCargoIntegrity_AtLanding()
        {
            var sys = MakeSystem(42, day: 1);
            sys.WindSpeedKph = () => 40f; // impact 20 + 20 = 40 ≥ heavy → 50% loss
            Assert.True(sys.ScheduleDrop("drop_medical", "sig_1", 0, 0).IsSuccess);
            for (int d = 2; d <= 4; d++) sys.TickDay(d);

            var drop = sys.State.drops[0];
            Assert.Equal(50, drop.cargo_integrity_pct);
            // Deterministic per-entry losses: 4 antibiotics → 2 remain, 3 medkits → 1.
            Assert.Contains(drop.remaining_contents, c => c.item_id == "antibiotics" && c.quantity == 2);
            Assert.Contains(drop.remaining_contents, c => c.item_id == "medkit" && c.quantity == 2);
        }

        // ── Contents generated once (Trap H) ────────────────────────────

        [Fact]
        public void CrateContents_GeneratedOnceAndPersisted_NeverRerolled()
        {
            var sys = MakeSystem(42, day: 1);
            Assert.True(sys.ScheduleDrop("drop_medical", "sig_1", 0, 0).IsSuccess);
            var contents = sys.State.drops[0].crate_contents;

            // Serialize → restore → contents identical.
            var json = System.Text.Json.JsonSerializer.Serialize(sys.CaptureState());
            var restored = System.Text.Json.JsonSerializer.Deserialize<CargoAirdropState>(json)!;
            var sys2 = new CargoAirdropSystem(new SeededRng(999)); // different rng post-load
            sys2.BindCatalog(CreateCatalog());
            sys2.RestoreState(restored);

            Assert.Equal(contents.Count, sys2.State.drops[0].crate_contents.Count);
            for (int i = 0; i < contents.Count; i++)
            {
                Assert.Equal(contents[i].item_id, sys2.State.drops[0].crate_contents[i].item_id);
                Assert.Equal(contents[i].quantity, sys2.State.drops[0].crate_contents[i].quantity);
            }
        }

        // ── Beacon & interception race ──────────────────────────────────

        [Fact]
        public void BeaconExpires_AndInterceptionRace_Accrues()
        {
            var sys = MakeSystem(42, day: 1);
            Assert.True(sys.ScheduleDrop("drop_medical", "sig_1", 0, 0).IsSuccess);
            for (int d = 2; d <= 4; d++) sys.TickDay(d); // land day 4, beacon to day 8
            var drop = sys.State.drops[0];

            // Beacon expires day 8 → dark on tick 9. Ticks 5-8 accrue 4000bp.
            for (int d = 5; d <= 9; d++) sys.TickDay(d);
            Assert.False(drop.beacon_active);

            // Ticks 10-14 accrue the remaining 6000bp → intercepted on day 14.
            for (int d = 10; d <= 14; d++) sys.TickDay(d);
            Assert.Equal("intercepted", drop.phase);
            Assert.Equal(1, sys.State.total_intercepted);
        }

        [Fact]
        public void Recovery_StopsTheInterceptionClock()
        {
            var sys = MakeSystem(42, day: 1);
            Assert.True(sys.ScheduleDrop("drop_medical", "sig_1", 0, 0).IsSuccess);
            for (int d = 2; d <= 4; d++) sys.TickDay(d);

            // Full grant: crate empties, phase recovered before interception.
            Assert.Equal(8, sys.CollectCrate(sys.State.drops[0].event_id,
                (itemId, weight, qty) => true));
            Assert.Equal("recovered", sys.State.drops[0].phase);
            Assert.Equal(1, sys.State.total_recovered);

            // Post-recovery ticks do not intercept.
            for (int d = 5; d <= 20; d++) sys.TickDay(d);
            Assert.Equal("recovered", sys.State.drops[0].phase);
        }

        // ── Recovery & capacity ─────────────────────────────────────────

        [Fact]
        public void Collect_RespectsCapacity_PartialRecovery_KeepsRemainder()
        {
            var sys = MakeSystem(42, day: 1);
            Assert.True(sys.ScheduleDrop("drop_medical", "sig_1", 0, 0).IsSuccess);
            for (int d = 2; d <= 4; d++) sys.TickDay(d);

            // Capacity refuses everything: nothing collected, crate intact.
            int collected = sys.CollectCrate(sys.State.drops[0].event_id, (_, _, _) => false);
            Assert.Equal(0, collected);
            Assert.Equal("landed", sys.State.drops[0].phase);
            Assert.Equal(3, sys.State.drops[0].remaining_contents.Count);

            // Capacity allows only the light items: partial recovery, remainder kept.
            collected = sys.CollectCrate(sys.State.drops[0].event_id, (itemId, weight, qty) =>
                weight * qty <= 0.5f); // antibiotics (0.2) + medkits (1.2 > 0.5 → refused)
            Assert.Equal(4, collected); // antibiotics only
            Assert.Equal("landed", sys.State.drops[0].phase);
            Assert.Equal(2, sys.State.drops[0].remaining_contents.Count);

            // Retry collects the rest once capacity allows.
            collected = sys.CollectCrate(sys.State.drops[0].event_id, (_, _, _) => true);
            Assert.Equal(4, collected); // 3 medkits (jitter may drop 1) + 1 surgical kit
            Assert.Equal("recovered", sys.State.drops[0].phase);
        }

        [Fact]
        public void Collect_IsIdempotent_AfterRecovery()
        {
            var (sys, _) = SetupLanded();
            string eventId = sys.State.drops[0].event_id;
            Assert.Equal(8, sys.CollectCrate(eventId, (_, _, _) => true));
            Assert.Equal(0, sys.CollectCrate(eventId, (_, _, _) => true));
            Assert.Equal("recovered", sys.State.drops[0].phase);
        }

        private (CargoAirdropSystem sys, Inventory.Inventory inv) SetupLanded()
        {
            var sys = MakeSystem(42, day: 1);
            Assert.True(sys.ScheduleDrop("drop_medical", "sig_1", 0, 0).IsSuccess);
            for (int d = 2; d <= 4; d++) sys.TickDay(d);
            return (sys, new Inventory.Inventory());
        }

        // ── Beacon reactivation ─────────────────────────────────────────

        [Fact]
        public void ReactivateBeacon_OnlyWhenLandedAndDark()
        {
            var sys = MakeSystem(42, day: 1);
            Assert.True(sys.ScheduleDrop("drop_medical", "sig_1", 0, 0).IsSuccess);
            string eventId = sys.State.drops[0].event_id;

            // Descending: blocked.
            Assert.False(sys.ReactivateBeacon(eventId).IsSuccess);
            for (int d = 2; d <= 4; d++) sys.TickDay(d);

            // Landed with live beacon: blocked (already active).
            Assert.False(sys.ReactivateBeacon(eventId).IsSuccess);
            sys.TickDay(9);
            Assert.False(sys.State.drops[0].beacon_active);

            // Dark beacon: reactivates.
            Assert.True(sys.ReactivateBeacon(eventId).IsSuccess);
            Assert.True(sys.State.drops[0].beacon_active);
        }

        // ── Save safety ─────────────────────────────────────────────────

        [Fact]
        public void OldSaveBaseline_NoDrops_NoRewards()
        {
            var sys = new CargoAirdropSystem(new SeededRng(42));
            sys.RestoreState(null);
            Assert.Empty(sys.State.drops);
            Assert.Equal(0, sys.State.total_recovered);
            Assert.Equal(0, sys.State.total_intercepted);
        }

        [Fact]
        public void SaveRoundTrip_MidDescent_PreservesWindAndProgress()
        {
            var sys = MakeSystem(42, day: 1);
            sys.WindDirectionDeg = () => 45f;
            sys.WindSpeedKph = () => 12f;
            Assert.True(sys.ScheduleDrop("drop_medical", "sig_1", 5, 5).IsSuccess);
            sys.TickDay(2); // 1 of 3 bands

            var json = System.Text.Json.JsonSerializer.Serialize(sys.CaptureState());
            var restored = System.Text.Json.JsonSerializer.Deserialize<CargoAirdropState>(json)!;
            var sys2 = new CargoAirdropSystem(new SeededRng(1)); // rng differs post-load
            sys2.BindCatalog(CreateCatalog());
            sys2.RestoreState(restored);

            var drop = sys2.State.drops[0];
            Assert.Equal("descending", drop.phase);
            Assert.Equal(2, drop.current_band);
            Assert.Equal(45f, drop.wind_direction_deg, 1);
            Assert.Equal(12f, drop.wind_speed_kph, 1);

            // Landing coordinates were computed at schedule time and are stable.
            var origLanding = (sys.State.drops[0].landing_x, sys.State.drops[0].landing_y);
            for (int d = 3; d <= 4; d++) sys2.TickDay(d);
            Assert.Equal(origLanding.Item1, sys2.State.drops[0].landing_x);
            Assert.Equal(origLanding.Item2, sys2.State.drops[0].landing_y);
            Assert.Equal("landed", sys2.State.drops[0].phase);
        }

        // ── Deterministic replay ────────────────────────────────────────

        [Fact]
        public void DeterministicReplay_SameSeedSameWind_SameOutcome()
        {
            CargoAirdropState Run(int seed)
            {
                var sys = MakeSystem(seed, day: 1);
                sys.WindDirectionDeg = () => 120f;
                sys.WindSpeedKph = () => 15f;
                Assert.True(sys.ScheduleDrop("drop_medical", "sig_x", 0, 0).IsSuccess);
                for (int d = 2; d <= 12; d++) sys.TickDay(d);
                sys.CollectCrate(sys.State.drops[0].event_id, (_, _, _) => true);
                return sys.CaptureState();
            }

            var a = Run(88);
            var b = Run(88);
            Assert.Equal(a.drops[0].landing_x, b.drops[0].landing_x);
            Assert.Equal(a.drops[0].landing_y, b.drops[0].landing_y);
            Assert.Equal(a.drops[0].cargo_integrity_pct, b.drops[0].cargo_integrity_pct);
            Assert.Equal(a.drops[0].phase, b.drops[0].phase);
            Assert.Equal(a.total_recovered, b.total_recovered);
            Assert.Equal(a.total_intercepted, b.total_intercepted);
        }

        // ── Data authority gate ─────────────────────────────────────────

        [Fact]
        public void Catalog_PoolsReferenceCanonicalItems_AndProfilesResolve()
        {
            string start = Directory.GetCurrentDirectory();
            Assert.True(CatalogLocator.TryFindDataDirectory(start, out string dataDir));
            string path = System.IO.Path.Combine(dataDir, "cargo_airdrop_catalog.json");
            Assert.True(File.Exists(path), $"Catalog not found at {path}");

            var catalog = System.Text.Json.JsonSerializer.Deserialize<CargoAirdropCatalog>(File.ReadAllText(path));
            Assert.NotNull(catalog);
            Assert.Equal(1, catalog!.schema_version);

            using var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(System.IO.Path.Combine(dataDir, "items.json")));
            var ids = new HashSet<string>();
            foreach (var item in doc.RootElement.GetProperty("items").EnumerateArray())
                ids.Add(item.GetProperty("id").GetString()!);

            var poolIds = new HashSet<string>();
            foreach (var pool in catalog.cargo_pools)
            {
                poolIds.Add(pool.cargo_pool_id);
                foreach (var e in pool.entries)
                    Assert.True(ids.Contains(e.item_id), $"cargo item '{e.item_id}' missing from items.json");
            }
            foreach (var p in catalog.drop_profiles)
            {
                Assert.Contains(p.cargo_pool_id, poolIds);
                Assert.False(p.trigger_signal_outcomes.Count == 0, $"{p.drop_profile_id} has no trigger outcomes");
            }
        }
    }
}
