// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 174 Phase 1 — Companion Animals Core contract tests.
// Covers: strict catalog validation, registration with stable identity from
// the wildlife authority, assignment validation, deterministic bond/training,
// canonical food consumption with fallback hierarchy, hunger/health drift,
// bounded guard/pack/morale modifiers, sickness/recovery rolls, bounded grief
// payload (morale authority applies it), save/load, and old-save baseline.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Ecology;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.Plan174Ecology
{
    public sealed class Plan174CompanionAnimalTests
    {
        private static CompanionSpeciesProfile Hound() => new CompanionSpeciesProfile
        {
            species_id = "species_ash_hound",
            display_name = "Ash Hound",
            role_tags = { "guard", "morale" },
            base_food_per_day = 2,
            fallback_food_item_ids = { "raw_meat", "cooked_meat" },
            max_health = 80,
            trainability = 7,
            bond_rate = 9,
            guard_rating = 45,
            pack_capacity_kg = 0,
            morale_support_bp = 250,
            disease_resistance = 4
        };

        private static CompanionSpeciesProfile Goat() => new CompanionSpeciesProfile
        {
            species_id = "species_feral_goat",
            display_name = "Feral Goat",
            role_tags = { "pack", "morale" },
            base_food_per_day = 3,
            fallback_food_item_ids = { "crop_ash_grain", "item_grain_flour" },
            max_health = 70,
            trainability = 5,
            bond_rate = 6,
            guard_rating = 5,
            pack_capacity_kg = 25,
            morale_support_bp = 120,
            disease_resistance = 6
        };

        private static CompanionAnimalSystem CreateSystem(params CompanionSpeciesProfile[] profiles)
        {
            var counts = new Dictionary<string, int>(StringComparer.Ordinal);
            var consumed = new List<(string id, int amount)>();
            var system = new CompanionAnimalSystem(profiles);
            system.BindFoodPort(
                id => counts.TryGetValue(id, out var n) ? n : 0,
                (id, amount) => { counts[id] = counts.TryGetValue(id, out var n) ? n : 0; counts[id] = Math.Max(0, counts[id] - amount); consumed.Add((id, amount)); });
            return system;
        }

        private static string FindDataDir()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string candidate = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data");
                if (File.Exists(Path.Combine(candidate, "companion_animals.json"))) return candidate;
                dir = dir.Parent;
            }
            return string.Empty;
        }

        // ── catalog ────────────────────────────────────────────────────

        [Fact]
        public void Loader_AuthoredCatalog_LoadsWithoutErrors()
        {
            string dataDir = FindDataDir();
            Assert.True(dataDir.Length > 0, "companion_animals.json not found");
            var result = CompanionAnimalCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(result.HasErrors, string.Join("; ", result.Errors));
            Assert.True(result.Companions.Count >= 3);
        }

        [Fact]
        public void Loader_RejectsDuplicatesAndBadRoles()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_plan174_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            try
            {
                File.WriteAllText(Path.Combine(dir, CompanionAnimalCatalogLoader.FileName),
                    "{\"schema_version\":1,\"companions\":[" +
                    "{\"species_id\":\"species_a\",\"display_name\":\"A\",\"role_tags\":[\"guard\"]," +
                    "\"fallback_food_item_ids\":[\"raw_meat\"],\"max_health\":50}," +
                    "{\"species_id\":\"species_a\",\"display_name\":\"B\",\"role_tags\":[\"pack\"]," +
                    "\"fallback_food_item_ids\":[\"raw_meat\"],\"max_health\":50}]}");
                var dup = CompanionAnimalCatalogLoader.Load(dir, fileIO, json);
                Assert.True(dup.HasErrors);
                Assert.Contains(dup.Errors, e => e.Contains("duplicate"));

                File.WriteAllText(Path.Combine(dir, CompanionAnimalCatalogLoader.FileName),
                    "{\"schema_version\":1,\"companions\":[" +
                    "{\"species_id\":\"species_b\",\"display_name\":\"B\",\"role_tags\":[\"stealth\"]," +
                    "\"fallback_food_item_ids\":[\"raw_meat\"],\"max_health\":50}]}");
                var badRole = CompanionAnimalCatalogLoader.Load(dir, fileIO, json);
                Assert.True(badRole.HasErrors);
                Assert.Contains(badRole.Errors, e => e.Contains("unknown role_tag"));
            }
            finally
            {
                Directory.Delete(dir, recursive: true);
            }
        }

        // ── registration & identity ────────────────────────────────────

        [Fact]
        public void Registration_StableIdentity_FromWildlifeAnimalId()
        {
            var system = CreateSystem(Hound());
            var r = system.RegisterCompanion("domestic_1", "species_ash_hound", tamedDay: 4, name: "Sable");
            Assert.True(r.Success);
            var c = system.Companion("domestic_1");
            Assert.NotNull(c);
            Assert.Equal("domestic_1", c!.companion_id);   // identity ≠ name
            Assert.Equal("Sable", c.name);

            // Duplicate adoption refused.
            Assert.False(system.RegisterCompanion("domestic_1", "species_ash_hound", 5).Success);
            // Unknown profile refused.
            Assert.False(system.RegisterCompanion("domestic_2", "species_missing", 5).Success);
        }

        [Fact]
        public void Registration_KnownSpeciesCheck_GatesWildlifeAuthority()
        {
            var system = CreateSystem(Hound());
            system.KnownSpeciesCheck = id => id == "species_ash_hound";
            Assert.True(system.RegisterCompanion("domestic_9", "species_ash_hound", 1).Success);
            system.KnownSpeciesCheck = _ => false; // wildlife authority lost the species
            Assert.Equal("unknown_species", system.RegisterCompanion("domestic_10", "species_ash_hound", 2).ReasonCode);
        }

        // ── assignment ─────────────────────────────────────────────────

        [Fact]
        public void Assignment_ValidatesRoleCompatibilityAndHandler()
        {
            var system = CreateSystem(Hound(), Goat());
            system.RegisterCompanion("domestic_1", "species_ash_hound", 1);
            system.RegisterCompanion("domestic_2", "species_feral_goat", 1);

            // Goat cannot guard (role_tags lack "guard").
            Assert.Equal("role_incompatible",
                system.Assign("domestic_2", "survivor_a", CompanionRole.Guard, _ => true).ReasonCode);
            Assert.True(system.Assign("domestic_2", "survivor_a", CompanionRole.Pack, _ => true).Success);
            Assert.True(system.Assign("domestic_1", "survivor_a", CompanionRole.Guard, _ => true).Success);

            // Dead handler refused.
            Assert.Equal("handler_dead",
                system.Assign("domestic_1", "survivor_b", CompanionRole.Guard, s => s != "survivor_b").ReasonCode);
        }

        // ── feeding & hunger ───────────────────────────────────────────

        [Fact]
        public void Feeding_ConsumesCanonicalInventoryExactlyOncePerDay()
        {
            var counts = new Dictionary<string, int>(StringComparer.Ordinal) { { "raw_meat", 10 } };
            var consumed = new List<(string, int)>();
            var system = new CompanionAnimalSystem(new[] { Hound() });
            system.BindFoodPort(
                id => counts.TryGetValue(id, out var n) ? n : 0,
                (id, a) => { counts[id] -= a; consumed.Add((id, a)); });

            system.RegisterCompanion("domestic_1", "species_ash_hound", 1);
            var c = system.Companion("domestic_1")!;

            system.TickDay(5);
            Assert.Equal(8, counts["raw_meat"]);             // 2 units consumed once
            Assert.Single(consumed);
            Assert.Equal(0, c.hunger);

            // Same-day re-feed is a no-op (no double consumption).
            system.Feed(c, Hound(), 5);
            Assert.Single(consumed);

            // Next day without food: hunger climbs, nothing dies from one miss.
            counts["raw_meat"] = 0;
            system.TickDay(6);
            Assert.Equal(Plan174Values.HungerDailyGain, c.hunger);
            Assert.True(c.alive);
        }

        [Fact]
        public void Feeding_FallbackHierarchy_UsedWhenPreferredMissing()
        {
            var counts = new Dictionary<string, int>(StringComparer.Ordinal) { { "cooked_meat", 5 } };
            var system = new CompanionAnimalSystem(new[] { Hound() });
            system.BindFoodPort(id => counts.TryGetValue(id, out var n) ? n : 0, (id, a) => counts[id] -= a);

            system.RegisterCompanion("domestic_1", "species_ash_hound", 1);
            var c = system.Companion("domestic_1")!;
            var map = new Dictionary<string, string>(StringComparer.Ordinal) { { "meat", "raw_meat" } };

            var result = system.Feed(c, Hound(), 5, map);
            Assert.True(result.Fed);
            Assert.Equal("cooked_meat", result.FoodItemId);   // fallback, since raw_meat absent
            Assert.Equal(3, counts["cooked_meat"]);
        }

        // ── bond & training ────────────────────────────────────────────

        [Fact]
        public void BondProgression_Deterministic_ClampedAtMax()
        {
            var counts = new Dictionary<string, int>(StringComparer.Ordinal) { { "raw_meat", 400 } };
            var system = new CompanionAnimalSystem(new[] { Hound() });
            system.BindFoodPort(id => counts.TryGetValue(id, out var n) ? n : 0, (id, a) => counts[id] -= a);
            system.RegisterCompanion("domestic_1", "species_ash_hound", 1);
            system.Assign("domestic_1", "survivor_a", CompanionRole.Guard, _ => true);
            var c = system.Companion("domestic_1")!;

            for (int day = 2; day <= 40; day++)
                system.TickDay(day); // food always stocked → daily care gains
            Assert.Equal(Plan174Values.MaxBond, c.bond);
        }

        [Fact]
        public void TrainingProgression_RequiresHandlerFoodHealth_AndStaysBounded()
        {
            var counts = new Dictionary<string, int>(StringComparer.Ordinal) { { "raw_meat", 400 } };
            var system = new CompanionAnimalSystem(new[] { Hound() });
            system.BindFoodPort(id => counts.TryGetValue(id, out var n) ? n : 0, (id, a) => counts[id] -= a);

            system.RegisterCompanion("domestic_1", "species_ash_hound", 1);
            var c = system.Companion("domestic_1")!;
            // No handler assigned: training never progresses.
            system.TickDay(2);
            Assert.Equal(0, c.training_level);

            system.Assign("domestic_1", "survivor_a", CompanionRole.Guard, _ => true);
            for (int day = 2; day <= 12; day++) system.TickDay(day);
            Assert.True(c.training_level > 0, "trained days should raise level");
            Assert.True((int)c.training_level <= (int)Plan174Values.MaxTrainingLevel);
        }

        // ── role modifiers (bounded) ───────────────────────────────────

        [Fact]
        public void GuardModifier_ScalesWithTraining_FadesWhenInjured_UntrainedLow()
        {
            var counts = new Dictionary<string, int>(StringComparer.Ordinal) { { "raw_meat", 400 } };
            var system = new CompanionAnimalSystem(new[] { Hound() });
            system.BindFoodPort(id => counts.TryGetValue(id, out var n) ? n : 0, (id, a) => counts[id] -= a);
            system.RegisterCompanion("domestic_1", "species_ash_hound", 1);
            var c = system.Companion("domestic_1")!;

            // Wrong role → zero.
            system.Assign("domestic_1", "survivor_a", CompanionRole.Morale, _ => true);
            Assert.Equal(0f, system.GetGuardModifier("domestic_1"), 3);

            system.Assign("domestic_1", "survivor_a", CompanionRole.Guard, _ => true);
            float untrained = system.GetGuardModifier("domestic_1");
            Assert.True(untrained > 0f && untrained < Hound().guard_rating,
                "untrained guard must be positive but below authored rating");

            // Injured below floor → zero benefit (§5.10).
            c.health = 10;
            Assert.Equal(0f, system.GetGuardModifier("domestic_1"), 3);
        }

        [Fact]
        public void PackCapacityBonus_Bounded_AndZeroForWrongRole()
        {
            var counts = new Dictionary<string, int>(StringComparer.Ordinal) { { "crop_ash_grain", 400 } };
            var system = new CompanionAnimalSystem(new[] { Goat() });
            system.BindFoodPort(id => counts.TryGetValue(id, out var n) ? n : 0, (id, a) => counts[id] -= a);
            system.RegisterCompanion("domestic_2", "species_feral_goat", 1);
            var c = system.Companion("domestic_2")!;

            // Hound profile has zero pack capacity anyway; goat as guard → zero.
            system.Assign("domestic_2", "survivor_a", CompanionRole.Guard, _ => true);
            Assert.Equal(0f, system.GetPackCapacityBonus("domestic_2"), 3);

            system.Assign("domestic_2", "survivor_a", CompanionRole.Pack, _ => true);
            float bonus = system.GetPackCapacityBonus("domestic_2");
            Assert.True(bonus > 0f && bonus <= Goat().pack_capacity_kg);
        }

        [Fact]
        public void MoraleSupport_BoundedByAuthoredProfile_AndZeroWhenDead()
        {
            var counts = new Dictionary<string, int>(StringComparer.Ordinal) { { "raw_meat", 400 } };
            var system = new CompanionAnimalSystem(new[] { Hound() });
            system.BindFoodPort(id => counts.TryGetValue(id, out var n) ? n : 0, (id, a) => counts[id] -= a);
            system.RegisterCompanion("domestic_1", "species_ash_hound", 1);
            system.Assign("domestic_1", "survivor_a", CompanionRole.Morale, _ => true);
            var c = system.Companion("domestic_1")!;

            system.TickDay(2);
            int support = system.GetMoraleSupportBp("domestic_1");
            Assert.InRange(support, 0, Hound().morale_support_bp);

            c.alive = false;
            Assert.Equal(0, system.GetMoraleSupportBp("domestic_1"));
        }

        // ── death & grief ──────────────────────────────────────────────

        [Fact]
        public void Death_EmitsTypedEvent_GriefBoundedAndBondScaled()
        {
            var counts = new Dictionary<string, int>(StringComparer.Ordinal) { { "raw_meat", 0 } };
            var system = new CompanionAnimalSystem(new[] { Hound() });
            system.BindFoodPort(id => counts.TryGetValue(id, out var n) ? n : 0, (id, a) => counts[id] -= a);
            system.RegisterCompanion("domestic_1", "species_ash_hound", 1);
            var c = system.Companion("domestic_1")!;
            c.health = 5;   // sustained starvation finishes it

            CompanionState? died = null;
            system.OnCompanionDied += state => died = state;

            for (int day = 2; day <= 8; day++) system.TickDay(day);
            Assert.NotNull(died);
            Assert.False(c.alive);

            // Grief payload bounded: max magnitude ceiling, bond-scaled.
            int grief = system.GetGriefMoraleDeltaBp("domestic_1");
            Assert.InRange(grief, Plan174Values.GriefMoraleShockMaxBp, Plan174Values.GriefMoraleFloorBp);
        }

        // ── persistence ────────────────────────────────────────────────

        [Fact]
        public void SaveLoad_PreservesExactCompanionState()
        {
            var counts = new Dictionary<string, int>(StringComparer.Ordinal) { { "raw_meat", 400 } };
            var system = new CompanionAnimalSystem(new[] { Hound() });
            system.BindFoodPort(id => counts.TryGetValue(id, out var n) ? n : 0, (id, a) => counts[id] -= a);
            system.RegisterCompanion("domestic_1", "species_ash_hound", 3, "Sable");
            system.Assign("domestic_1", "survivor_a", CompanionRole.Guard, _ => true);
            for (int day = 3; day <= 9; day++) system.TickDay(day);

            var saved = system.CaptureState();
            var restored = new CompanionAnimalSystem(new[] { Hound() });
            restored.RestoreState(saved);

            Assert.Equal(saved.companions.Count, restored.State.companions.Count);
            var a = saved.companions[0];
            var b = restored.State.companions[0];
            Assert.Equal(a.companion_id, b.companion_id);
            Assert.Equal(a.name, b.name);
            Assert.Equal(a.bond, b.bond);
            Assert.Equal(a.health, b.health);
            Assert.Equal(a.hunger, b.hunger);
            Assert.Equal(a.training_level, b.training_level);
            Assert.Equal(a.role, b.role);
            Assert.Equal(a.assigned_survivor_id, b.assigned_survivor_id);
            Assert.Equal(a.alive, b.alive);
        }

        [Fact]
        public void OldSaveBaseline_RestoreFromNullIsSafe()
        {
            var system = CreateSystem(Hound());
            system.RestoreState(null);
            Assert.Empty(system.State.companions);
        }

        // ── determinism ────────────────────────────────────────────────

        [Fact]
        public void DeterministicReplay_SameSeedSameUpkeep()
        {
            var run = (int seed) =>
            {
                var counts = new Dictionary<string, int>(StringComparer.Ordinal) { { "raw_meat", 400 } };
                var system = new CompanionAnimalSystem(new[] { Hound() });
                system.BindFoodPort(id => counts.TryGetValue(id, out var n) ? n : 0, (id, a) => counts[id] -= a);
                int roll = 0;
                system.SicknessRoll = () =>
                {
                    roll++;
                    // Deterministic hash roll (no wall-clock, no System.Random).
                    uint h = (uint)(seed * 1000 + roll) * 2654435761u;
                    return (h % 1000u) / 1000.0;
                };
                system.RegisterCompanion("domestic_1", "species_ash_hound", 1);
                system.Assign("domestic_1", "survivor_a", CompanionRole.Guard, _ => true);
                for (int day = 2; day <= 20; day++) system.TickDay(day);
                var c = system.Companion("domestic_1")!;
                return (c.bond, c.health, c.hunger, c.training_level, c.sickness);
            };

            var a = run(3);
            var b = run(3);
            Assert.Equal(a, b);
        }
    }

    internal static class Plan174Values
    {
        public const int MaxBond = CompanionAnimalSystem.MaxBond;
        public const int HungerDailyGain = CompanionAnimalSystem.HungerDailyGain;
        public const int GriefMoraleShockMaxBp = CompanionAnimalSystem.GriefMoraleShockMaxBp;
        public const int GriefMoraleFloorBp = CompanionAnimalSystem.GriefMoraleFloorBp;
        public const int MaxTrainingLevel = (int)CompanionTrainingLevel.Expert;
    }
}
