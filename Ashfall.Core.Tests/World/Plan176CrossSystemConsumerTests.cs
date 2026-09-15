// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 176 — cross-system consumer tests.
//   1. ExposureEnvironmentResolver anomaly provider (typed expedition handoff;
//      legacy null-provider path byte-identical; dose stays with RadiationSystem)
//   2. WildlifeEcosystemSystem hazard-avoidance migration (deterministic,
//      through the ONE migration authority, bounded chance, attraction no-op)
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core.Radiation;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Plan176World
{
    public sealed class Plan176CrossSystemConsumerTests
    {
        // ── exposure resolver ──────────────────────────────────────────

        [Fact]
        public void ExposureResolver_LegacyPath_UnchangedWithoutAnomalyProvider()
        {
            var resolver = new ExposureEnvironmentResolver
            {
                WeatherRadModifierProvider = () => 150f,
                FalloutContaminationProvider = locId => locId == "loc_x" ? 30f : 0f
            };

            var env = resolver.ResolveForEnvironment(SurvivorExposureLocation.Expedition, "loc_x");
            Assert.Equal(40f + 150f + 30f, env.EffectiveZoneRadLevel, 3);
            Assert.Equal(0f, env.AnomalyRadRate, 3);
        }

        [Fact]
        public void ExposureResolver_Expedition_IncludesAnomalyRate()
        {
            var resolver = new ExposureEnvironmentResolver
            {
                WeatherRadModifierProvider = () => 150f,
                FalloutContaminationProvider = locId => 30f,
                AnomalyRadRateProvider = locId => locId == "loc_x" ? 42f : 0f
            };

            var env = resolver.ResolveForEnvironment(SurvivorExposureLocation.Expedition, "loc_x");
            Assert.Equal(40f + 150f + 30f + 42f, env.EffectiveZoneRadLevel, 3);
            Assert.Equal(42f, env.AnomalyRadRate, 3);
            Assert.Contains("Anomaly", env.ExposureReason);

            // Location outside any anomaly: rate zero, base composition intact.
            var clean = resolver.ResolveForEnvironment(SurvivorExposureLocation.Expedition, "loc_y");
            Assert.Equal(40f + 150f + 30f, clean.EffectiveZoneRadLevel, 3);
            Assert.Equal(0f, clean.AnomalyRadRate, 3);
        }

        [Fact]
        public void ExposureResolver_ShelterInterior_IgnoresAnomalyRate()
        {
            // V1 contract: only the Expedition environment consumes anomaly
            // radiation. Shelter interior stays shielding-authoritative.
            var resolver = new ExposureEnvironmentResolver
            {
                AnomalyRadRateProvider = _ => 999f
            };

            var env = resolver.ResolveForEnvironment(SurvivorExposureLocation.ShelterInterior, "loc_x");
            Assert.Equal(0f, env.AnomalyRadRate, 3);
            Assert.Equal(2f, env.EffectiveZoneRadLevel, 3);
        }

        // ── wildlife avoidance ─────────────────────────────────────────

        private static WildlifeEcosystemContainer SingleSpeciesCatalog()
        {
            var catalog = new WildlifeEcosystemContainer();
            catalog.species.Add(new FaunaSpeciesDef
            {
                id = "species_test_hare", display_name = "Test Hare",
                radiation_tolerance = 0.9f, diet_type = "herbivore",
                tameable = false, tags = { "prey" }
            });
            // No predator-prey edges, no seasonal moves: avoidance is isolated.
            return catalog;
        }

        private static WildlifeMigrationSystem OnePackMigration()
        {
            var m = new WildlifeMigrationSystem(new SeededRng(42));
            m.RegisterPack("pack_hare_x", "species_test_hare", "sector_a", 10);
            m.SetSectorAdjacency(new[]
            {
                ("sector_a", new List<string> { "sector_b" }),
                ("sector_b", new List<string> { "sector_a" })
            });
            return m;
        }

        private static void TickDays(WildlifeEcosystemSystem system, WildlifeMigrationSystem migration,
            IReadOnlyDictionary<string, float>? modifiers, int days, int seed)
        {
            for (int day = 1; day <= days; day++)
            {
                system.TickDay(day, migration, 0f, "any",
                    new SeededRng(seed * 10 + day),
                    new SeededRng(seed * 10 + day),
                    new SeededRng(seed * 10 + day),
                    modifiers);
            }
        }

        [Fact]
        public void WildlifeAvoidance_NullModifiers_NeverMigrates()
        {
            var migration = OnePackMigration();
            var system = new WildlifeEcosystemSystem();
            system.LoadCatalog(SingleSpeciesCatalog());

            TickDays(system, migration, modifiers: null, days: 10, seed: 5);

            Assert.Equal("sector_a", migration.State.packs[0].currentSectorId);
        }

        [Fact]
        public void WildlifeAvoidance_FullAvoidance_MigratesDeterministically()
        {
            var modifiers = new Dictionary<string, float>(StringComparer.Ordinal)
            {
                { "sector_a", -1f }
            };

            var a = OnePackMigration();
            var sysA = new WildlifeEcosystemSystem();
            sysA.LoadCatalog(SingleSpeciesCatalog());
            TickDays(sysA, a, modifiers, days: 10, seed: 7);

            var b = OnePackMigration();
            var sysB = new WildlifeEcosystemSystem();
            sysB.LoadCatalog(SingleSpeciesCatalog());
            TickDays(sysB, b, modifiers, days: 10, seed: 7);

            // Same seed → identical track; the pack has left the hazard sector.
            Assert.Equal(a.State.packs[0].currentSectorId, b.State.packs[0].currentSectorId);
            Assert.Equal("sector_b", a.State.packs[0].currentSectorId);
        }

        [Fact]
        public void WildlifeAvoidance_Attraction_IsNoOp()
        {
            var modifiers = new Dictionary<string, float>(StringComparer.Ordinal)
            {
                { "sector_a", 0.8f }
            };
            var migration = OnePackMigration();
            var system = new WildlifeEcosystemSystem();
            system.LoadCatalog(SingleSpeciesCatalog());

            TickDays(system, migration, modifiers, days: 10, seed: 5);

            Assert.Equal("sector_a", migration.State.packs[0].currentSectorId);
        }

        [Fact]
        public void WildlifeAvoidance_EventFiresExactlyOncePerMigration()
        {
            var modifiers = new Dictionary<string, float>(StringComparer.Ordinal)
            {
                { "sector_a", -1f }
            };
            var migration = OnePackMigration();
            var system = new WildlifeEcosystemSystem();
            system.LoadCatalog(SingleSpeciesCatalog());

            var events = new List<(string species, string from, string to)>();
            system.OnHazardAvoidanceMigration += (s, f, t) => events.Add((s, f, t));

            TickDays(system, migration, modifiers, days: 10, seed: 7);

            // The pack left once and never came back (sector_b has no avoidance).
            Assert.Equal("sector_b", migration.State.packs[0].currentSectorId);
            Assert.Single(events);
            Assert.Equal(("species_test_hare", "sector_a", "sector_b"), events[0]);
        }

        [Fact]
        public void WildlifeAvoidance_SplitRun_ProducesIdenticalTrack()
        {
            var modifiers = new Dictionary<string, float>(StringComparer.Ordinal)
            {
                { "sector_a", -0.9f }
            };

            var continuous = OnePackMigration();
            var contSys = new WildlifeEcosystemSystem();
            contSys.LoadCatalog(SingleSpeciesCatalog());
            TickDays(contSys, continuous, modifiers, days: 6, seed: 11);

            // Split at day 3: identical forks per day → identical outcome.
            var split = OnePackMigration();
            var splitSys = new WildlifeEcosystemSystem();
            splitSys.LoadCatalog(SingleSpeciesCatalog());
            for (int day = 1; day <= 3; day++)
                splitSys.TickDay(day, split, 0f, "any",
                    new SeededRng(11 * 10 + day), new SeededRng(11 * 10 + day),
                    new SeededRng(11 * 10 + day), modifiers);
            var restoredMigration = new WildlifeMigrationSystem(new SeededRng(42));
            restoredMigration.RestoreState(split.CaptureState());
            for (int day = 4; day <= 6; day++)
                restoredSysTick(splitSys, restoredMigration, modifiers, day, 11);

            Assert.Equal(
                continuous.State.packs[0].currentSectorId,
                split.State.packs[0].currentSectorId);
        }

        private static void restoredSysTick(WildlifeEcosystemSystem system, WildlifeMigrationSystem migration,
            IReadOnlyDictionary<string, float> modifiers, int day, int seed)
        {
            system.TickDay(day, migration, 0f, "any",
                new SeededRng(seed * 10 + day), new SeededRng(seed * 10 + day),
                new SeededRng(seed * 10 + day), modifiers);
        }
    }
}
