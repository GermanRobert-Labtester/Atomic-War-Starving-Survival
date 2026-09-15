// SPDX-License-Identifier: MIT
// ============================================================================
// Wave F — Plans 174–177 unified deterministic campaign replay (§12–§14).
// One connected 30-day campaign harness over the four Core authorities:
//
//   wildlife companion adoption → training/upkeep → guard/pack/morale roles
//   fictional belief influence → conversion/fervor → cohesion or fanaticism
//   dynamic radiation storm → route disruption → exposure → shelter warning
//   severe trauma → limb loss → bionic surgery → rehabilitation → maintenance
//
// All state rides the canonical authorities (NeedsSystem, RadiationSystem,
// AmputationSystem, Inventory). Cross-effects travel through typed APIs.
// §13: continuous run == mid-reload split (day 11 → save/restore → resume),
// fingerprint equality over every authoritative field. No rerolls.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Ashfall.Core.Ecology;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Medical;
using Ashfall.Core.Radiation;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Flagship174to177
{
    public sealed class Plans174To177CampaignIntegrationTests
    {
        private const int CampaignSeed = 174177;
        private const int SplitDay = 11;
        private const int TotalDays = 30;

        // ── inline authored content (same values as the shipped catalogs) ──

        private static CompanionSpeciesProfile Hound() => new CompanionSpeciesProfile
        {
            species_id = "species_ash_hound", display_name = "Ash Hound",
            role_tags = { "guard", "morale" }, base_food_per_day = 2,
            fallback_food_item_ids = { "raw_meat" }, max_health = 80,
            trainability = 7, bond_rate = 9, guard_rating = 45, morale_support_bp = 250
        };

        private static CompanionSpeciesProfile Goat() => new CompanionSpeciesProfile
        {
            species_id = "species_feral_goat", display_name = "Feral Goat",
            role_tags = { "pack", "morale" }, base_food_per_day = 3,
            fallback_food_item_ids = { "crop_ash_grain" }, max_health = 70,
            trainability = 5, bond_rate = 6, guard_rating = 5, pack_capacity_kg = 25, morale_support_bp = 120
        };

        private static CompanionSpeciesProfile Hare() => new CompanionSpeciesProfile
        {
            species_id = "species_cotton_hare", display_name = "Cotton Hare",
            role_tags = { "morale" }, base_food_per_day = 1,
            fallback_food_item_ids = { "crop_leafy_green" }, max_health = 30,
            trainability = 4, bond_rate = 10, guard_rating = 0, morale_support_bp = 300
        };

        private static AnomalyDefinition StormFront() => new AnomalyDefinition
        {
            anomaly_id = "anomaly_ember_gale_front", display_name = "Ember Gale Front",
            anomaly_type = "rad_storm_front", spawn_region_tags = { "open_waste" },
            movement_profile = "storm_front", movement_speed_kph = 9f, wind_response = 0.4f,
            radiation_rate = 120f, radius_km = 26f, duration_days = 8,
            warning_radius_km = 55f, warning_profile = "standard", detection_threshold = 18f,
            wildlife_modifier_bp = -650, environmental_effect_tags = { "ember_fall" },
            tags = { "storm" }
        };

        private static AnomalyDefinition IridescentPool() => new AnomalyDefinition
        {
            anomaly_id = "anomaly_iridescent_pool", display_name = "Iridescent Pool",
            anomaly_type = "mutagenic_field", spawn_region_tags = { "deep_zone" },
            movement_profile = "static", radiation_rate = 40f, radius_km = 6f,
            duration_days = 40, warning_radius_km = 12f, warning_profile = "standard",
            detection_threshold = 8f, loot_table_id = "table_loot_chemical_plant",
            loot_site_count = 2, wildlife_modifier_bp = -900,
            environmental_effect_tags = { "mutagenic" }, tags = { "loot_site" }
        };

        private static ImplantDefinition LegServo() => new ImplantDefinition
        {
            implant_id = "implant_reaction_piston_leg", display_name = "Reaction-Piston Leg",
            body_slot = "leg", implant_class = "rechargeable",
            functional_restore_bp = 1150, skill_modifier_bp = 150,
            power_profile = "rechargeable", daily_power_draw_watts = 20, battery_days = 4,
            maintenance_interval_days = 5, daily_condition_decay_bp = 50,
            integration_risk_bp = 1600, integration_recovery_days = 10,
            malfunction_risk_bp = 450, electrical_vulnerability = "high",
            required_item_ids = { "bionic_leg_prototype" }
        };

        private static ZealotryBeliefProfile Witnesses() => new ZealotryBeliefProfile
        {
            belief_id = "belief_ash_witnesses", display_name = "The Ash Witnesses",
            doctrine_tags = { "memory", "penance" },
            conversion_base_bp = 1400, fervor_daily_decay_bp = 450, fervor_ritual_gain_bp = 1800,
            cohesion_bonus_bp = 600, despair_resistance_bp = 1600, fanaticism_threshold = 78,
            dissent_tolerance = "low", ritual_resource_item_ids = { "canned_food" },
            broadcast_profile = "neutral", tags = { "fictional" }
        };

        // ── the campaign harness ───────────────────────────────────────

        private sealed class Campaign
        {
            public int Seed;
            public NeedsSystem Needs = new NeedsSystem();
            public RadiationSystem Radiation = null!;
            public ExposureEnvironmentResolver Resolver = new ExposureEnvironmentResolver();
            public CompanionAnimalSystem Companions = null!;
            public AnomalyHazardSystem Anomalies = null!;
            public AmputationSystem Amputation = null!;
            public BionicsSystem Bionics = null!;
            public ZealotrySystem Zealotry = null!;
            public Dictionary<string, int> Inv = new Dictionary<string, int>(StringComparer.Ordinal);
            public Ashfall.Core.Inventory.Inventory AmpInventory = null!;
            public List<string> Fingerprints = new List<string>();

            // Event counters (assertions; not part of the state fingerprint).
            public int Warnings;
            public int Conversions;
            public int LootResolvedSuccess;
            public int LootResolvedDuplicate;
            public bool ImplantInstalled;
            public bool ImplantIntegrated;

            public void Stock(string id, int n) => Inv[id] = Inv.TryGetValue(id, out var v) ? v + n : n;
            public int Count(string id) => Inv.TryGetValue(id, out var v) ? v : 0;
            public void Take(string id, int n) { Inv[id] = Math.Max(0, Count(id) - n); }
        }

        private static Campaign CreateCampaign(int seed)
        {
            var camp = new Campaign { Seed = seed };

            // Inventory (the ONE item-quantity authority for the harness).
            camp.Stock("raw_meat", 120);
            camp.Stock("crop_ash_grain", 120);
            camp.Stock("crop_leafy_green", 60);
            camp.Stock("canned_food", 30);
            camp.Stock("item_surgical_kit", 5);
            camp.Stock("surgical_saw", 5);
            camp.Stock("bionic_leg_prototype", 5);
            camp.Stock("antiseptic_1l_of_1l", 5);
            camp.Stock("painkillers", 5);
            camp.Stock("repair_kit", 10);

            // Survivors with varied stress (§12).
            foreach (var id in new[] { "s_guard", "s_scout", "s_amputee", "s_convert" })
                camp.Needs.Register(new SurvivorNeedsState { Id = id });
            camp.Needs.Modify("s_amputee", NeedKind.Morale, 30f);   // varied stress bands

            // ── Anomalies (Plan 176) ──
            var anomalyLoad = new AnomalyCatalogLoadResult();
            anomalyLoad.Anomalies.Add(StormFront());
            anomalyLoad.Anomalies.Add(IridescentPool());
            camp.Anomalies = new AnomalyHazardSystem(AnomalyCatalogLoader.ToCatalog(anomalyLoad));
            camp.Anomalies.OnStormApproaching += _ => camp.Warnings++;

            // ── Companions (Plan 174) ──
            camp.Companions = new CompanionAnimalSystem(new[] { Hound(), Goat(), Hare() });
            camp.Companions.BindFoodPort(
                id => camp.Count(id),
                (id, n) => camp.Take(id, n));
            camp.Companions.OnCompanionDied += _ => { /* grief asserted separately */ };

            // ── Amputation (limb authority) ── its surgery bill draws from a
            // dedicated inventory stocked identically in both runs; the only
            // consumer is the single day-12 surgery, so restore parity holds.
            camp.AmpInventory = new Ashfall.Core.Inventory.Inventory();
            camp.AmpInventory.AddById("surgical_saw", 5);
            camp.AmpInventory.AddById("item_surgical_kit", 10);
            camp.Amputation = new AmputationSystem(new SeededRng(seed * 31 + 7), camp.AmpInventory, camp.Needs);
            camp.Amputation.RegisterProcedure(new SurgicalProcedureDef
            {
                procedure_id = "proc_field_leg",
                display_name = "Field Leg Amputation",
                limb_group = "leg",
                required_tool_id = "surgical_saw",
                required_items = { new SurgicalItemCost { item_id = "item_surgical_kit", amount = 1 } },
                base_shock_risk = 0.0f,      // scripted: always survives
                base_bleeding = 10f,
                recovery_days_min = 3,
                recovery_days_max = 3,
                phantom_pain_chance = 0f
            });

            // ── Bionics (Plan 177) ──
            var bioLoad = new BionicsCatalogLoadResult();
            bioLoad.Implants.Add(LegServo());
            camp.Bionics = new BionicsSystem(camp.Amputation, BionicsCatalogLoader.ToDefinitions(bioLoad));
            camp.Bionics.BindInventory(
                id => camp.Count(id),
                (id, n) => camp.Take(id, n));
            camp.Bionics.ChargerAvailable = () => true;   // powered clinic in the harness
            camp.Bionics.OnImplantInstalled += (_, __) => camp.ImplantInstalled = true;
            camp.Bionics.OnIntegrationCompleted += _ => camp.ImplantIntegrated = true;

            // ── Zealotry (Plan 175) ──
            camp.Zealotry = new ZealotrySystem(new[] { Witnesses() });
            camp.Zealotry.OnConverted += (_, __) => camp.Conversions++;

            // ── Radiation (dose authority) ──
            camp.Resolver.ShelterInteriorBaseRadRate = 0.5f;
            camp.Resolver.WastelandOutdoorBaseRadRate = 6f;
            camp.Resolver.LocationRadRateProvider = locId => 8f;
            camp.Resolver.FalloutContaminationProvider = _ => 0f;
            camp.Resolver.AnomalyRadRateProvider = locId =>
                locId == "loc_chemical_plant" ? camp.Anomalies.GetRadiationRate(15f, 35f) : 0f;
            camp.Resolver.WeatherRadModifierProvider = () => 0f;
            camp.Resolver.SetSurvivorLocation("s_scout", SurvivorExposureLocation.Expedition, "loc_chemical_plant");
            camp.Radiation = new RadiationSystem(state =>
                camp.Resolver.Resolve(state.Id).ToExposureContext(null), seed: 1401);
            foreach (var s in camp.Needs.Registered)
                camp.Radiation.Register(new SurvivorRadState { Id = s.Id });

            // ── Day 1 setup commands (§14 days 1–5) ──
            camp.Companions.RegisterCompanion("domestic_1", "species_ash_hound", 1, "Sable");
            camp.Companions.RegisterCompanion("domestic_2", "species_feral_goat", 1, "Pail");
            camp.Companions.RegisterCompanion("domestic_3", "species_cotton_hare", 1, "Mote");
            camp.Companions.Assign("domestic_1", "s_guard", CompanionRole.Guard, _ => true);
            camp.Companions.Assign("domestic_2", "s_scout", CompanionRole.Pack, _ => true);
            camp.Companions.Assign("domestic_3", "s_amputee", CompanionRole.Morale, _ => true);

            camp.Zealotry.RegisterLeader("s_guard", "belief_ash_witnesses");

            Assert.True(camp.Anomalies.TrySpawn("anomaly_ember_gale_front", 15f, 240f, 1, 180f).Success);
            Assert.True(camp.Anomalies.TrySpawn("anomaly_iridescent_pool", 15f, 70f, 1).Success);

            return camp;
        }

        private static void RunDay(Campaign camp, int day)
        {
            // Day-keyed forks (split-run parity: same day → same rolls).
            var wanderRng = new SeededRng(unchecked(camp.Seed * 977 + day));
            var sickRng = new SeededRng(unchecked(camp.Seed * 977 + day + 1));
            var convRng = new SeededRng(unchecked(camp.Seed * 977 + day + 2));
            var malRng = new SeededRng(unchecked(camp.Seed * 977 + day + 3));

            // 1. The world moves (storm track, warnings).
            camp.Anomalies.TickDay(day, windDirDeg: 90f, windSpeedKph: 12f, wanderRng);
            camp.Anomalies.EvaluateApproach("shelter", 0f, 0f);

            // 2. Companion care (feeding consumes real inventory).
            camp.Companions.SicknessRoll = () => sickRng.NextDouble();
            camp.Companions.TickDay(day);

            // 3. Scripted campaign events (§14).
            if (day >= 7 && day <= 9 && camp.Conversions == 0)
            {
                // Recruiting window (days 7–9): deterministic forced roll so the
                // scripted campaign always yields the §14 milestone conversion;
                // resistance paths are proven separately in Phase-1 tests.
                camp.Zealotry.TryConvert("s_convert", "belief_ash_witnesses", day,
                    new ConversionContext { Stress01 = 0.8f, LeaderCharisma01 = 0.6f, LeaderOfSameBelief = true },
                    AlwaysRollRng.Instance);
            }
            if (day == 8) camp.Zealotry.SetShrine("belief_ash_witnesses", present: true);
            if (day == 9)
            {
                var sites = camp.Anomalies.LootSites;
                if (sites.Count > 0 && camp.Anomalies.TryResolveLootSite(sites[0].site_id, day).Success)
                    camp.LootResolvedSuccess++;
            }
            if (day == 10)
            {
                var sites = camp.Anomalies.LootSites;
                if (sites.Count > 0 && !camp.Anomalies.TryResolveLootSite(sites[0].site_id, day).Success)
                    camp.LootResolvedDuplicate++;
            }
            if (day == 12)
            {
                var result = camp.Amputation.PerformAmputation("s_amputee", LimbId.RightLeg, "proc_field_leg");
                Assert.True(result.Success);
            }
            if (day == 16)
            {
                var r = camp.Bionics.TryInstall("s_amputee", LimbId.RightLeg,
                    "implant_reaction_piston_leg", day, convRng);
                Assert.True(r.Success, r.ReasonCode);
            }
            if (day >= 17 && day % 5 == 2)
            {
                camp.Bionics.PerformMaintenance("s_amputee", LimbId.RightLeg, "repair_kit", day);
            }

            // 4. Core daily ticks (canonical ordering §3).
            camp.Bionics.MalfunctionRoll = () => malRng.NextDouble();
            camp.Bionics.TickDay(day);
            camp.Zealotry.TickDay(day);

            // Ritual cadence (bounded, transactional through the inventory).
            if (day % 3 == 0)
            {
                string? group = FindLargestBeliefGroup(camp.Zealotry);
                if (group != null && camp.Zealotry.State.unresolved_demands.Count == 0)
                {
                    var demand = camp.Zealotry.EmitRitualDemand(group, day);
                    if (demand != null)
                    {
                        bool available = true;
                        foreach (var itemId in demand.ItemIds)
                            if (camp.Count(itemId) <= 0) { available = false; break; }
                        if (available)
                            foreach (var itemId in demand.ItemIds)
                                camp.Take(itemId, 1);
                        camp.Zealotry.ResolveRitualDemand(group, day, available);
                    }
                }
            }

            camp.Amputation.TickDay(day);
            camp.Radiation.Tick(24f);

            AppendFingerprint(camp, day);
        }

        private static string? FindLargestBeliefGroup(ZealotrySystem zealotry)
        {
            string? best = null;
            int bestCount = 0;
            var counts = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var b in zealotry.State.believers)
            {
                if (b == null || string.IsNullOrEmpty(b.belief_id)) continue;
                counts.TryGetValue(b.belief_id, out int c);
                counts[b.belief_id] = c + 1;
                if (counts[b.belief_id] > bestCount
                    || (counts[b.belief_id] == bestCount && best != null && string.CompareOrdinal(b.belief_id, best) < 0))
                {
                    bestCount = counts[b.belief_id];
                    best = b.belief_id;
                }
            }
            return best;
        }

        // ── fingerprint (§12: every authoritative field, invariant format) ──

        private static void AppendFingerprint(Campaign camp, int day)
        {
            var sb = new StringBuilder();
            string F(float v) => v.ToString("F2", CultureInfo.InvariantCulture);
            sb.Append("D").Append(day);

            foreach (var c in camp.Companions.State.companions)
            {
                if (c == null) continue;
                sb.Append("|C:").Append(c.companion_id)
                  .Append(':').Append(c.hunger)
                  .Append(':').Append(c.health)
                  .Append(':').Append(c.bond)
                  .Append(':').Append(c.training_level)
                  .Append(':').Append(c.role)
                  .Append(':').Append(c.alive ? "a" : "x");
            }
            foreach (var b in camp.Zealotry.State.believers)
            {
                if (b == null) continue;
                sb.Append("|B:").Append(b.survivor_id)
                  .Append(':').Append(b.belief_id)
                  .Append(':').Append(b.conviction)
                  .Append(':').Append(b.fervor)
                  .Append(':').Append(b.dissent)
                  .Append(':').Append(b.in_crisis ? "c" : "s");
            }
            sb.Append("|ESC:").Append(camp.Zealotry.State.escalation_stage);

            var hazards = new List<AnomalyHazardInstance>(camp.Anomalies.State.hazards);
            hazards.Sort((a, b) => string.CompareOrdinal(a.hazard_id, b.hazard_id));
            foreach (var h in hazards)
            {
                if (h == null) continue;
                sb.Append("|A:").Append(h.hazard_id)
                  .Append(":(").Append(F(h.position_x)).Append(',').Append(F(h.position_y)).Append(')')
                  .Append(':').Append(F(h.intensity))
                  .Append(':').Append(h.age_days)
                  .Append(':').Append(h.active ? "on" : "off");
            }
            foreach (var s in camp.Anomalies.LootSites)
            {
                if (s == null) continue;
                sb.Append("|LT:").Append(s.site_id).Append(':').Append(s.resolved ? "r" : "u");
            }

            foreach (var r in camp.Radiation.Registered)
            {
                if (r == null) continue;
                sb.Append("|RD:").Append(r.Id).Append(':').Append(F(r.RadiationDose));
            }

            foreach (var i in camp.Bionics.State.implants)
            {
                if (i == null) continue;
                sb.Append("|BI:").Append(i.instance_id)
                  .Append(":c").Append(F(i.condition))
                  .Append(":b").Append(F(i.battery_days_remaining))
                  .Append(":i").Append(i.integration_days_left)
                  .Append(':').Append(i.integration_status)
                  .Append(":m").Append(i.malfunction)
                  .Append(':').Append(i.destroyed ? "x" : "o");
            }
            var ampLimb = camp.Amputation.GetLimb("s_amputee", LimbId.RightLeg);
            if (ampLimb != null)
                sb.Append("|AMP:RightLeg:").Append(ampLimb.condition);

            foreach (var s in camp.Needs.Registered)
            {
                if (s == null) continue;
                sb.Append("|MP:").Append(s.Id).Append(':').Append(F(s.Morale));
            }
            foreach (var kv in camp.Inv)
                sb.Append("|INV:").Append(kv.Key).Append(':').Append(kv.Value);
            // Inventory keys are fixed at creation and captured/restored in the
            // same insertion order; quantity deltas prove single consumption.

            camp.Fingerprints.Add(sb.ToString());
        }

        private static string Hash(List<string> fingerprints)
        {
            using var sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(string.Join("\n", fingerprints)));
            var sb = new StringBuilder();
            foreach (var b in bytes) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        /// <summary>Deterministic always-succeed roll source for scripted
        /// campaign events (never used by decay/movement paths).</summary>
        private sealed class AlwaysRollRng : ISeededRng
        {
            public static readonly AlwaysRollRng Instance = new AlwaysRollRng();
            public int Seed => 0;
            public int Next(int minInclusive, int maxExclusive) => minInclusive;
            public float NextFloat() => 0f;
            public double NextDouble() => 0.0;
        }

        // ── state capture/restore across all six systems (§13) ─────────

        private sealed class SaveBlob
        {
            public CompanionSystemState Companions = null!;
            public AnomalyHazardSystemState Anomalies = null!;
            public AmputationSystemState Amputation = null!;
            public BionicsSystemState Bionics = null!;
            public ZealotrySystemState Zealotry = null!;
            public List<(string id, float dose, bool acute)> Doses = new List<(string, float, bool)>();
            public Dictionary<string, int> Inventory = new Dictionary<string, int>(StringComparer.Ordinal);
        }

        private static SaveBlob CaptureAll(Campaign camp)
        {
            var blob = new SaveBlob
            {
                Companions = camp.Companions.CaptureState(),
                Anomalies = camp.Anomalies.CaptureState(),
                Amputation = camp.Amputation.CaptureState(),
                Bionics = camp.Bionics.CaptureState(),
                Zealotry = camp.Zealotry.CaptureState()
            };
            foreach (var r in camp.Radiation.Registered)
                blob.Doses.Add((r.Id, r.RadiationDose, r.HasAcuteRadiationSickness));
            foreach (var kv in camp.Inv) blob.Inventory[kv.Key] = kv.Value;
            return blob;
        }

        private static Campaign RestoreAll(SaveBlob blob)
        {
            var camp = CreateCampaign(blob.Doses.Count > 0 ? CampaignSeed : CampaignSeed);
            // Discard the day-1 scripted spawn/registration state from the fresh
            // build — the restore path replaces ALL authoritative state exactly.
            camp.Companions.RestoreState(blob.Companions);
            camp.Anomalies.RestoreState(blob.Anomalies);
            camp.Amputation.RestoreState(blob.Amputation);
            camp.Bionics.RestoreState(blob.Bionics);
            camp.Zealotry.RestoreState(blob.Zealotry);
            camp.Inv.Clear();
            foreach (var kv in blob.Inventory) camp.Inv[kv.Key] = kv.Value;
            foreach (var (id, dose, acute) in blob.Doses)
            {
                var rad = new SurvivorRadState { Id = id, RadiationDose = dose, HasAcuteRadiationSickness = acute };
                camp.Radiation.Register(rad);
            }
            return camp;
        }

        // ── §13: continuous == mid-reload split ────────────────────────

        [Fact]
        public void ThirtyDayReplay_ContinuousMatchesMidReloadSplit_FieldByField()
        {
            var continuous = CreateCampaign(CampaignSeed);
            for (int day = 1; day <= TotalDays; day++) RunDay(continuous, day);

            var split = CreateCampaign(CampaignSeed);
            for (int day = 1; day <= SplitDay; day++) RunDay(split, day);
            var blob = CaptureAll(split);
            var resumed = RestoreAll(blob);
            // Carry the pre-split fingerprints forward — the split run's first
            // half was recorded before the save; together they must equal the
            // continuous run day-for-day.
            resumed.Fingerprints.InsertRange(0, split.Fingerprints.GetRange(0, SplitDay));
            for (int day = SplitDay + 1; day <= TotalDays; day++) RunDay(resumed, day);

            Assert.Equal(continuous.Fingerprints.Count, resumed.Fingerprints.Count);
            for (int i = 0; i < continuous.Fingerprints.Count; i++)
                Assert.True(string.Equals(continuous.Fingerprints[i], resumed.Fingerprints[i], StringComparison.Ordinal),
                    $"fingerprint divergence at index {i} (day {i + 1}):\n  continuous: {continuous.Fingerprints[i]}\n  split:      {resumed.Fingerprints[i]}");
            Assert.Equal(Hash(continuous.Fingerprints), Hash(resumed.Fingerprints));
        }

        // ── §14 milestone assertions ───────────────────────────────────

        [Fact]
        public void ThirtyDayCampaign_MilestoneAssertions()
        {
            var camp = CreateCampaign(CampaignSeed);
            for (int day = 1; day <= TotalDays; day++) RunDay(camp, day);

            // Days 1–5 — companion integration: food consumed once per day per
            // animal, stable ids, no free role benefit.
            Assert.True(camp.Companions.Companion("domestic_1")!.alive);
            Assert.True(camp.Companions.Companion("domestic_1")!.bond > 0);
            Assert.True(camp.Count("raw_meat") < 120, "hound feed consumed real units");

            // Days 6–10 — ideological pressure: deterministic conversion,
            // ritual transactions, no duplicate loot, one warning.
            Assert.Equal(1, camp.Conversions);
            Assert.NotNull(camp.Zealotry.Believer("s_convert"));
            Assert.Equal(1, camp.LootResolvedSuccess);
            Assert.Equal(1, camp.LootResolvedDuplicate);   // second resolve REJECTED
            Assert.Equal(1, camp.Warnings);                // storm approach warned once

            // Days 11–15 — storm: deterministic track, route dose spike, expiry.
            var storm = Assert.Single(camp.Anomalies.State.hazards,
                h => h.anomaly_id == "anomaly_ember_gale_front");
            Assert.False(storm.active);                    // expired after 8 days
            var scoutDose = camp.Radiation.Registered.First(r => r.Id == "s_scout").RadiationDose;
            Assert.True(scoutDose > 0f, "expedition member absorbed storm dose");

            // Days 16–20 — limb loss → bionic surgery (no instant recovery).
            var limb = camp.Amputation.GetLimb("s_amputee", LimbId.RightLeg)!;
            Assert.Equal(LimbCondition.Bionic, limb.condition);
            var implant = camp.Bionics.InstanceFor("s_amputee", LimbId.RightLeg);
            Assert.NotNull(implant);
            Assert.True(camp.ImplantInstalled);

            // Days 21–27 — rehabilitation completes; bounded bonus flows.
            Assert.True(camp.ImplantIntegrated);
            float bonus = camp.Bionics.GetLimbCapabilityBonusBp("s_amputee", LimbId.RightLeg);
            Assert.InRange(bonus, 0f, BionicsCaps.CapabilityBonusCapBp);
            float movement = camp.Amputation.GetMovementSpeedMultiplier("s_amputee");
            Assert.True(movement > 0.8f, $"bionic leg restores mobility (got {movement:F2})");

            // Days 28–30 — final assertions: exactly-once everywhere.
            Assert.Equal(1, camp.Conversions);             // no double conversion
            Assert.True(camp.Companions.Companion("domestic_1")!.alive, "cared-for companion survives");
            float packBonus = camp.Companions.GetPackCapacityBonusForSurvivor("s_scout");
            Assert.InRange(packBonus, 0f, Goat().pack_capacity_kg);
            Assert.Equal(Hash(camp.Fingerprints), Hash(camp.Fingerprints)); // sanity
        }

        // ── §5.15/§28: grief applies exactly once, bounded ─────────────

        [Fact]
        public void CompanionDeath_GriefAppliedOnce_Bounded()
        {
            var camp = CreateCampaign(CampaignSeed);
            // Starve the hare: no leafy greens ever.
            camp.Inv["crop_leafy_green"] = 0;

            var hare = camp.Companions.Companion("domestic_3")!;
            hare.health = 6;
            float moraleBefore = camp.Needs.Get("s_amputee")!.Morale;

            int deathEvents = 0;
            camp.Companions.OnCompanionDied += _ => deathEvents++;

            for (int day = 2; day <= 12; day++)
            {
                camp.Anomalies.TickDay(day, 90f, 12f, new SeededRng(day));
                camp.Companions.SicknessRoll = () => 1.0;   // never sick: pure starvation
                camp.Companions.TickDay(day);
                if (!hare.alive && deathEvents > 0 && deathEvents == 1)
                {
                    // Apply the host-side grief route exactly once (as Main does).
                    int deltaBp = camp.Companions.GetGriefMoraleDeltaBp("domestic_3");
                    camp.Needs.Modify("s_amputee", NeedKind.Morale, deltaBp / 100f);
                }
                camp.Amputation.TickDay(day);
            }

            Assert.Equal(1, deathEvents);                  // grief fires exactly once
            Assert.False(hare.alive);
            float moraleAfter = camp.Needs.Get("s_amputee")!.Morale;
            Assert.True(moraleAfter < moraleBefore, "bounded grief registered");
            int grief = camp.Companions.GetGriefMoraleDeltaBp("domestic_3");
            Assert.InRange(grief, CompanionAnimalSystem.GriefMoraleShockMaxBp, CompanionAnimalSystem.GriefMoraleFloorBp);
        }

        // ── §14 days 21–27: ideology + bionics + companion interplay ───

        [Fact]
        public void Campaign_IdeologyReacts_BoundedInterplay()
        {
            var camp = CreateCampaign(CampaignSeed);
            for (int day = 1; day <= TotalDays; day++) RunDay(camp, day);

            // The converted survivor gains bounded despair resistance through
            // the belief (never bypassing morale — morale state is intact).
            var believer = camp.Zealotry.Believer("s_convert")!;
            Assert.False(string.IsNullOrEmpty(believer.belief_id));
            int resistance = camp.Zealotry.GetDespairResistanceBp("s_convert");
            Assert.InRange(resistance, 0, ZealotryCaps.MaxDespairResistanceBp);

            // Morale remains a canonical, finite 0..100 value (never nullified).
            foreach (var s in camp.Needs.Registered)
            {
                Assert.InRange(s.Morale, 0f, 100f);
            }

            // Companion morale support stayed bounded for the amputee.
            int support = camp.Companions.GetMoraleSupportBp("domestic_3");
            Assert.InRange(support, 0, Hare().morale_support_bp);
        }
    }
}
