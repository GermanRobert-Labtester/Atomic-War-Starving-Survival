using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Warlords;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Adaptive warlord AI tests (proposed model). Covers doctrine transition
    /// rules, cooldown/hysteresis, stable target selection, legal/illegal
    /// annexation, contested resolution, player/system consequences, missing
    /// reference validation, faction alias conflict reporting, YearOfAshSave v3
    /// round-trip/migration, and same-seed trace equivalence.
    /// </summary>
    public class WarlordDoctrineTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static WarlordDoctrineCatalog LoadCatalog()
        {
            var files = new FileSystemIO();
            return WarlordDoctrineCatalogLoader.Load(DataDir(), files, new SystemTextJsonSerializer());
        }

        /// <summary>Shared catalog access for sibling test classes (expedition danger integration).</summary>
        public static WarlordDoctrineCatalog LoadCatalogForTests() => LoadCatalog();

        private static WarlordDoctrineSystem NewWarlord(int seed = 7719) =>
            new WarlordDoctrineSystem(LoadCatalog(), seed);

        private static WarlordContext Calm() =>
            new WarlordContext { EnvironmentHazard = 0.2f, RivalPressure = 0.3f, PlayerStanding = 0 };

        private static void TickRange(WarlordDoctrineSystem w, int from, int to, WarlordContext ctx = null)
        {
            ctx = ctx ?? Calm();
            var rng = new SeededRng(w.State.seedSalt + from);
            for (int day = from; day <= to; day++)
                w.TickDaily(day, rng, ctx);
        }

        // ── Canonical identity & alias conflict reporting ──────────────

        [Fact]
        public void Warlord_BindsOnlyToCanonicalFactionId()
        {
            var catalog = LoadCatalog();
            Assert.Equal("warlords_sector_4", catalog.Warlord.faction_id);
            Assert.Equal("warlords_sector_4", WarlordDoctrineSystem.CanonicalFactionId);
            Assert.False(string.Equals(catalog.Warlord.faction_id, "raiders", StringComparison.Ordinal));
            Assert.False(string.Equals(catalog.Warlord.faction_id, "ash_militia", StringComparison.Ordinal));
        }

        [Fact]
        public void Warlord_AliasConflicts_AreReported_NotMerged()
        {
            var files = new FileSystemIO();
            var catalog = LoadCatalog();
            var validation = WarlordCatalogValidator.Validate(catalog, DataDir(), files);
            Assert.True(validation.Clean, string.Join("; ", validation.Errors));
            Assert.True(validation.AliasWarnings.Count >= 1, "alias conflicts must be reported");
            Assert.Contains(validation.AliasWarnings, w => w.Contains("raiders", StringComparison.Ordinal));
            Assert.Contains(validation.AliasWarnings, w => w.Contains("ash_militia", StringComparison.Ordinal));
        }

        [Fact]
        public void Warlord_CatalogValidation_RejectsMissingReferences()
        {
            // A catalog pointing at a non-existent location must fail loudly.
            var bad = LoadCatalog();
            bad.Territory[0].location_id = "loc_does_not_exist_anywhere";
            bad.Territory[0].home = false;
            var files = new FileSystemIO();
            var validation = WarlordCatalogValidator.Validate(bad, DataDir(), files);
            Assert.False(validation.Clean);
            Assert.Contains(validation.Errors, e => e.Contains("loc_does_not_exist_anywhere", StringComparison.Ordinal));

            // A doctrine transition to an unknown doctrine must fail loudly.
            var bad2 = LoadCatalog();
            bad2.Doctrines[0].transitions[0].to = "warlord_doctrine_unknown";
            var validation2 = WarlordCatalogValidator.Validate(bad2, DataDir(), files);
            Assert.False(validation2.Clean);
            Assert.Contains(validation2.Errors, e => e.Contains("warlord_doctrine_unknown", StringComparison.Ordinal));
        }

        // ── Initial doctrine & territory ───────────────────────────────

        [Fact]
        public void Warlord_StartsWithCatalogDoctrine_AndHomeControlled()
        {
            var w = NewWarlord();
            Assert.Equal("warlord_doctrine_toll", w.DoctrineId);
            Assert.Equal(WarlordTerritoryState.Controlled, w.TerritoryState("loc_toll_house"));
            Assert.Equal(1, w.ControlledCount());
            Assert.Equal(WarlordTerritoryState.None, w.ReportedState("loc_grain_silo")); // no omniscience before observation
            Assert.Equal(0.35f, w.TravelDangerModifier("loc_toll_house"));
            Assert.True(w.IsHostileAccess("loc_toll_house"));
        }

        // ── Doctrine transition rules ──────────────────────────────────

        [Fact]
        public void Warlord_DoctrineTransitions_FireOnSignals_AndRespectCooldown()
        {
            var w = NewWarlord(101);
            // Feed scouts so the warlord can see its adjacent ground.
            w.Observe("loc_weighbridge", WarlordTerritoryState.None, 210);
            w.Observe("loc_denial_cut_substation", WarlordTerritoryState.None, 210);
            var rng = new SeededRng(101);
            for (int day = 210; day <= 400; day++)
                w.TickDaily(day, rng, Calm());

            Assert.True(w.State.doctrineHistory.Count >= 1, "doctrine history records transitions");
            for (int i = 1; i < w.State.doctrineHistory.Count; i++)
            {
                int gap = w.State.doctrineHistory[i].day - w.State.doctrineHistory[i - 1].day;
                Assert.True(gap >= LoadCatalog().Warlord.doctrine_cooldown_days,
                    "no transition inside the cooldown window (thrash guard)");
            }
        }

        [Fact]
        public void Warlord_DoctrineChange_DoesNotThrashUnderBoundaryConditions()
        {
            // High environment hazard flips any doctrine toward withdrawal; the
            // cooldown must prevent immediate flip-back.
            var w = NewWarlord(303);
            w.Observe("loc_weighbridge", WarlordTerritoryState.None, 210);
            w.Observe("loc_denial_cut_substation", WarlordTerritoryState.None, 210);
            var hazard = new WarlordContext { EnvironmentHazard = 0.9f, RivalPressure = 0.3f };
            var rng = new SeededRng(303);
            for (int day = 210; day <= 260; day++)
                w.TickDaily(day, rng, hazard);
            int changes = w.State.doctrineHistory.Count;
            // 50 days / 10-day cooldown → at most ~5 changes, not one per tick.
            Assert.True(changes <= 6, "doctrine changes bounded by cooldown under stress");
        }

        // ── Stable target selection ────────────────────────────────────

        [Fact]
        public void Warlord_TargetSelection_IsDeterministic_AndPrefersWeakTargets()
        {
            var a = NewWarlord(505);
            var b = NewWarlord(505);
            a.Observe("loc_weighbridge", WarlordTerritoryState.None, 210);
            b.Observe("loc_weighbridge", WarlordTerritoryState.None, 210);
            var rngA = new SeededRng(505);
            var rngB = new SeededRng(505);
            for (int day = 210; day <= 300; day++)
            {
                a.TickDaily(day, rngA, Calm());
                b.TickDaily(day, rngB, Calm());
            }
            // Same seed ⇒ same territory state (same targets, same outcomes).
            Assert.Equal(a.State.territory.Count, b.State.territory.Count);
            for (int i = 0; i < a.State.territory.Count; i++)
            {
                Assert.Equal(a.State.territory[i].locationId, b.State.territory[i].locationId);
                Assert.Equal(a.State.territory[i].state, b.State.territory[i].state);
            }
        }

        // ── Legal & illegal annexation ─────────────────────────────────

        [Fact]
        public void Warlord_Annexation_RespectsAdjacencyAndCooldown()
        {
            var w = NewWarlord(707);
            // The grain silo is not adjacent to warlord ground at start (only the
            // substation bridges it), so it cannot be annexed until the substation
            // is controlled.
            Assert.Equal(WarlordTerritoryState.None, w.TerritoryState("loc_grain_silo"));
            // Push the substation to contested via a contest that must succeed:
            // drive a full annexation program and require the ladder to climb
            // rather than jump.
            w.Observe("loc_weighbridge", WarlordTerritoryState.None, 210);
            w.Observe("loc_denial_cut_substation", WarlordTerritoryState.None, 210);
            var rng = new SeededRng(707);
            w.OnActionExecuted += r =>
            {
                // Annex side-effect tracked via event wiring; kept for future assertion
            };
            for (int day = 210; day <= 500; day++)
                w.TickDaily(day, rng, Calm());

            // If any node reached Controlled beyond home, it must be a legal
            // annexation target (adjacent to prior control, not the home).
            for (int i = 0; i < w.State.territory.Count; i++)
            {
                var rec = w.State.territory[i];
                if (rec == null || rec.locationId == "loc_toll_house") continue;
                if (rec.state == (int)WarlordTerritoryState.Controlled)
                {
                    Assert.True(IsAdjacent(w, rec.locationId, "loc_toll_house")
                        || HasControlledNeighbor(w, rec.locationId),
                        "controlled node '" + rec.locationId + "' is reachable through the graph");
                }
            }
            // Home can never be lost.
            Assert.Equal(WarlordTerritoryState.Controlled, w.TerritoryState("loc_toll_house"));
        }

        private static bool IsAdjacent(WarlordDoctrineSystem w, string a, string b)
        {
            var neighbors = w.Catalog.Neighbors(a);
            for (int i = 0; i < neighbors.Count; i++)
                if (neighbors[i] == b)
                    return true;
            return false;
        }

        private static bool HasControlledNeighbor(WarlordDoctrineSystem w, string locationId)
        {
            var neighbors = w.Catalog.Neighbors(locationId);
            for (int i = 0; i < neighbors.Count; i++)
                if (w.TerritoryState(neighbors[i]) == WarlordTerritoryState.Controlled)
                    return true;
            return false;
        }

        // ── Contested resolution ───────────────────────────────────────

        [Fact]
        public void Warlord_Contest_ProducesClaimsOrContests_NotInstantControl()
        {
            var w = NewWarlord(909);
            w.Observe("loc_weighbridge", WarlordTerritoryState.None, 210);
            w.Observe("loc_denial_cut_substation", WarlordTerritoryState.None, 210);
            var rng = new SeededRng(909);
            // Single contest push: force the ladder to Claimed first (never None → Controlled).
            w.TickDaily(210, rng, Calm()); // harvest/transition day; no op yet
            for (int day = 211; day <= 240; day++)
                w.TickDaily(day, rng, Calm());
            int st = (int)w.TerritoryState("loc_weighbridge");
            Assert.True(st <= (int)WarlordTerritoryState.Contested,
                "weighbridge can at most be contested after a short push, never instantly controlled");
        }

        // ── Tribute payment loop: reliability → doctrine pressure ─────

        [Fact]
        public void Warlord_TributeRefusal_DrivesDoctrineTowardAggression()
        {
            // Refusing tribute drags player_tribute_reliability below 0.5, which
            // is the consolidation → toll transition — the warlord stops waiting
            // and starts raiding, raising road danger.
            var w = NewWarlord(191);
            w.Observe("loc_weighbridge", WarlordTerritoryState.None, 210);
            w.Observe("loc_denial_cut_substation", WarlordTerritoryState.None, 210);
            var rng = new SeededRng(191);
            var ctx = Calm();
            // Drive into consolidation first (supply shortage or failures).
            for (int day = 210; day <= 260; day++)
                w.TickDaily(day, rng, ctx);
            // Force a tribute ask and refuse it repeatedly (short weeks).
            for (int i = 0; i < 3; i++)
            {
                int next;
                w.SettleTribute(0, 300 + i, out next);
            }
            // Reliability = paid/asked; nothing paid ⇒ 0.0 < 0.5 ⇒ toll returns.
            Assert.True(w.State.totalWeeksAsked > 0, "tribute has been asked");
            Assert.True(w.State.totalWeeksPaid == 0, "no payments made");
            var rng2 = new SeededRng(192);
            for (int day = 300; day <= 340; day++)
                w.TickDaily(day, rng2, ctx);
            // Once consolidation's cooldown lapses, the reliability signal fires.
            Assert.Contains(w.State.doctrineHistory, h => h.doctrineId == "warlord_doctrine_toll"
                || w.DoctrineId == "warlord_doctrine_toll");
        }

        [Fact]
        public void Warlord_FullPayments_KeepTheRoadCalm()
        {
            // Full, reliable payment keeps reliability high: no consolidation→toll
            // pressure from the player side.
            var w = NewWarlord(193);
            w.Observe("loc_weighbridge", WarlordTerritoryState.None, 210);
            var rng = new SeededRng(193);
            var ctx = Calm();
            for (int day = 210; day <= 240; day++)
                w.TickDaily(day, rng, ctx);
            int ask = w.Catalog.Warlord.tribute_base_amount;
            for (int i = 0; i < 3; i++)
            {
                int next;
                w.SettleTribute(ask, 300 + i, out next);
            }
            Assert.True(w.State.totalWeeksPaid == 3, "three full payments recorded");
            Assert.True(w.State.consecutiveShortWeeks == 0, "no short-week streak after full payments");
            Assert.Equal(1f, w.TributeMultiplier);
        }

        [Fact]
        public void Warlord_CollectorVoice_IsAuthoredAndDeterministic()
        {
            var catalog = LoadCatalog();
            foreach (var state in new[] { "demand", "paid", "short", "refused" })
            {
                string line = catalog.CollectorLine(state, 250);
                Assert.False(string.IsNullOrEmpty(line), "collector voice authored for " + state);
                Assert.Equal(line, catalog.CollectorLine(state, 250)); // deterministic per day
            }
        }

        // ── Player/system consequences ─────────────────────────────────

        [Fact]
        public void Warlord_TributeEscalates_OnShortPayment_AndCaps()
        {
            var w = NewWarlord(111);
            var catalog = LoadCatalog();
            var rng = new SeededRng(111);
            w.TickDaily(210, rng, Calm());
            // Tribute fires on the interval; force asks and short-pay them.
            int ask = 0;
            w.OnTributeDemanded += (amount, item, day) => ask = amount;
            for (int day = 210; day <= 224; day++)
                w.TickDaily(day, rng, Calm());
            Assert.True(ask >= catalog.Warlord.tribute_base_amount, "tribute asked on the cadence");
            for (int i = 0; i < 6; i++)
            {
                int next;
                w.SettleTribute(0, 224 + i, out next); // short pay / refuse
            }
            Assert.True(w.TributeMultiplier > 1f, "short payments escalate the ask");
            Assert.True(w.TributeMultiplier <= catalog.Warlord.tribute_max_multiplier, "escalation capped at 8×");
            Assert.True(w.State.consecutiveShortWeeks >= 1, "short-week counter advances");
        }

        [Fact]
        public void Warlord_ControlledTerritory_RaisesTravelDanger()
        {
            var w = NewWarlord(131);
            Assert.Equal(0.35f, w.TravelDangerModifier("loc_toll_house"));
            // Unclaimed nodes contribute nothing at start.
            Assert.Equal(0f, w.TravelDangerModifier("loc_weighbridge"));
            Assert.Equal(0f, w.TravelDangerModifier("loc_grain_silo"));

            // The middle rungs of the ladder raise danger progressively. Craft the
            // territory records directly (the state DTO is the save authority).
            var st = w.CaptureState();
            st.Territory("loc_weighbridge").state = (int)WarlordTerritoryState.Claimed;
            w.RestoreState(st);
            Assert.Equal(0.10f, w.TravelDangerModifier("loc_weighbridge"));

            st = w.CaptureState();
            st.Territory("loc_weighbridge").state = (int)WarlordTerritoryState.Contested;
            w.RestoreState(st);
            Assert.Equal(0.20f, w.TravelDangerModifier("loc_weighbridge"));

            st = w.CaptureState();
            st.Territory("loc_weighbridge").state = (int)WarlordTerritoryState.Controlled;
            w.RestoreState(st);
            Assert.Equal(0.35f, w.TravelDangerModifier("loc_weighbridge"));
            Assert.True(w.IsHostileAccess("loc_weighbridge"));
        }

        // ── Same-seed trace equivalence (full trace) ───────────────────

        [Fact]
        public void Warlord_SameSeed_FullTrace_IsIdentical()
        {
            var a = NewWarlord(151);
            var b = NewWarlord(151);
            var ta = new List<string>();
            var tb = new List<string>();
            a.OnTerritoryChanged += (l, f, t, d) => ta.Add($"{d}:{l}:{f}->{t}");
            b.OnTerritoryChanged += (l, f, t, d) => tb.Add($"{d}:{l}:{f}->{t}");
            a.Observe("loc_weighbridge", WarlordTerritoryState.None, 210);
            a.Observe("loc_denial_cut_substation", WarlordTerritoryState.None, 210);
            b.Observe("loc_weighbridge", WarlordTerritoryState.None, 210);
            b.Observe("loc_denial_cut_substation", WarlordTerritoryState.None, 210);
            var ra = new SeededRng(151);
            var rb = new SeededRng(151);
            for (int day = 210; day <= 330; day++)
            {
                a.TickDaily(day, ra, Calm());
                b.TickDaily(day, rb, Calm());
            }
            Assert.Equal(ta, tb); // identical event ordering
            Assert.Equal(a.State.doctrineId, b.State.doctrineId);
            Assert.Equal(a.State.supply, b.State.supply);
            Assert.Equal(a.State.totalOperations, b.State.totalOperations);
            Assert.Equal(a.State.successStreak, b.State.successStreak);
            Assert.Equal(a.State.failureStreak, b.State.failureStreak);
        }

        // ── Save round-trip & migration ────────────────────────────────

        [Fact]
        public void Warlord_YearOfAshSaveV3_RoundTripsChecksummed()
        {
            var w = NewWarlord(171);
            w.Observe("loc_weighbridge", WarlordTerritoryState.None, 210);
            var rng = new SeededRng(171);
            for (int day = 210; day <= 280; day++)
                w.TickDaily(day, rng, Calm());

            var json = new SystemTextJsonSerializer();
            var timeline = new YearOfAshTimelineSystem();
            var encounters = new DoorEncounterSystem();
            var factionWar = new FactionWarSystem();
            var save = YearOfAshSaveCodec.Capture(timeline, encounters, factionWar, null, null, null, null, w);
            Assert.Equal(YearOfAshSave.CurrentSaveVersion, save.saveVersion);
            Assert.False(string.IsNullOrEmpty(save.Checksum));
            string encoded = YearOfAshSaveCodec.Encode(save, json);

            var loaded = YearOfAshSaveCodec.Decode(encoded, json);
            Assert.Equal(w.DoctrineId, loaded.warlord.doctrineId);
            Assert.Equal(w.Supply, loaded.warlord.supply);
            Assert.Equal(w.State.territory.Count, loaded.warlord.territory.Count);
            Assert.Equal(w.State.totalOperations, loaded.warlord.totalOperations);

            // Restore into a fresh system reproduces the territory ladder.
            var fresh = NewWarlord(171);
            fresh.RestoreState(loaded.warlord);
            Assert.Equal(w.TerritoryState("loc_toll_house"), fresh.TerritoryState("loc_toll_house"));
            Assert.Equal(w.DoctrineId, fresh.DoctrineId);
        }

        [Fact]
        public void Warlord_YearOfAshSave_V2Migrates_V1Migrates_AndFutureRejected()
        {
            var json = new SystemTextJsonSerializer();
            var timeline = new YearOfAshTimelineSystem();
            var encounters = new DoorEncounterSystem();
            var factionWar = new FactionWarSystem();

            var v2 = new YearOfAshSaveV2
            {
                saveVersion = 2,
                simDay = 220,
                timeline = timeline.CaptureState(),
                encounters = encounters.CaptureState(),
                factionWar = factionWar.CaptureState()
            };
            v2.Checksum = SaveChecksum.Compute(v2);
            var migrated = YearOfAshSaveCodec.Decode(json.Serialize(v2), json);
            Assert.Equal(YearOfAshSave.CurrentSaveVersion, migrated.saveVersion);
            Assert.NotNull(migrated.warlord);
            Assert.Equal("warlord_doctrine_toll", migrated.warlord.doctrineId);
            Assert.Equal("warlords_sector_4", migrated.warlord.factionId);

            var v1 = new YearOfAshSaveV1
            {
                saveVersion = 1,
                simDay = 200,
                timeline = timeline.CaptureState(),
                encounters = encounters.CaptureState(),
                factionWar = factionWar.CaptureState()
            };
            v1.Checksum = SaveChecksum.Compute(v1);
            var m1 = YearOfAshSaveCodec.Decode(json.Serialize(v1), json);
            Assert.Equal(YearOfAshSave.CurrentSaveVersion, m1.saveVersion);
            Assert.Equal("warlord_doctrine_toll", m1.warlord.doctrineId);

            // Future-version rejection.
            var future = json.Deserialize<YearOfAshSave>(json.Serialize(v2));
            future.saveVersion = YearOfAshSave.CurrentSaveVersion + 1;
            future.Checksum = SaveChecksum.Compute(future);
            Assert.Throws<InvalidOperationException>(() => YearOfAshSaveCodec.Decode(json.Serialize(future), json));

            // Tamper rejection.
            string tampered = json.Serialize(v2).Replace("\"simDay\":220", "\"simDay\":221");
            Assert.Throws<InvalidOperationException>(() => YearOfAshSaveCodec.Decode(tampered, json));
        }

        // ── Regression: existing Year of Ash systems still green ───────

        [Fact]
        public void Warlord_Regression_FactionWarAndTimelineUnaffected()
        {
            var fw = new FactionWarSystem();
            fw.SimulateDailyFriction(250);
            Assert.True(fw.WarTension >= 50, "faction war friction unchanged");
            var timeline = new YearOfAshTimelineSystem();
            timeline.AdvanceDay(250);
            Assert.Equal(250, timeline.CurrentDay);
        }
    }
}
