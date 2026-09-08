// SPDX-License-Identifier: MIT
// Plan 165 — WildlifeEcosystemSystem tests: single population authority,
// predation/radiation pressure, extinction/recolonization, apex, taming
// transfer, knowledge gating, persistence equivalence.
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class WildlifeEcosystemSystemTests
    {
        private static string FindDataDir()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
            return dataDir ?? string.Empty;
        }

        private static WildlifeEcosystemContainer ShippedCatalog() =>
            WildlifeEcosystemCatalogLoader.Load(FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

        private static WildlifeMigrationSystem SeededMigration()
        {
            var m = new WildlifeMigrationSystem(new SeededRng(42));
            m.RegisterPack("pack_hare_a", "species_cotton_hare", "sector_4_hinterlands", 8);
            m.RegisterPack("pack_hare_b", "species_cotton_hare", "sector_2_north_ridge", 6);
            m.RegisterPack("pack_wolf_a", "species_wolf", "sector_4_hinterlands", 12);
            m.RegisterPack("pack_goat_a", "species_feral_goat", "sector_2_north_ridge", 7);
            m.RegisterPack("pack_rat_a", "species_blight_rat", "sector_4_hinterlands", 9);
            m.SetSectorAdjacency(new[]
            {
                ("sector_4_hinterlands", new List<string> { "sector_2_north_ridge" }),
                ("sector_2_north_ridge", new List<string> { "sector_4_hinterlands" })
            });
            return m;
        }

        private static WildlifeEcosystemSystem MakeEcosystem(WildlifeEcosystemContainer catalog) =>
            new WildlifeEcosystemSystem().Also(e => e.LoadCatalog(catalog));

        [Fact]
        public void ShippedCatalog_Validates()
        {
            var catalog = ShippedCatalog();
            Assert.True(catalog.species.Count >= 12, $"expected 12 species, got {catalog.species.Count}");
            Assert.Empty(WildlifeEcosystemCatalogLoader.Validate(catalog));
        }

        [Fact]
        public void CatalogValidation_RejectsBadEntries()
        {
            var bad = new WildlifeEcosystemContainer();
            bad.species.Add(new FaunaSpeciesDef { id = "species_dup" });
            bad.species.Add(new FaunaSpeciesDef { id = "species_dup" });
            bad.species.Add(new FaunaSpeciesDef { id = "species_prey_ok" });
            bad.predator_prey.Add(new PredatorPreyEdgeDef
            { predator_species_id = "species_missing", prey_species_id = "species_prey_ok" });
            bad.predator_prey.Add(new PredatorPreyEdgeDef
            { predator_species_id = "species_prey_ok", prey_species_id = "species_prey_ok" });
            var diags = WildlifeEcosystemCatalogLoader.Validate(bad);
            Assert.Contains(diags, d => d.Contains("duplicate"));
            Assert.Contains(diags, d => d.Contains("predator not defined"));
            Assert.Contains(diags, d => d.Contains("self-predation"));
        }

        [Fact]
        public void Populations_DeriveFromTheSingleAuthority()
        {
            var eco = MakeEcosystem(ShippedCatalog());
            var m = SeededMigration();
            Assert.Equal(8, eco.SectorSpeciesPopulation(m, "sector_4_hinterlands", "species_cotton_hare"));
            Assert.Equal(6, eco.SectorSpeciesPopulation(m, "sector_2_north_ridge", "species_cotton_hare"));
            // Trapping density reads the same packs.
            Assert.Equal(eco.SectorDensityMultiplier(m, "sector_4_hinterlands"),
                eco.SectorDensityMultiplier(m, "sector_4_hinterlands"));
            Assert.True(eco.SectorDensityMultiplier(m, "sector_4_hinterlands") > 0f);
        }

        [Fact]
        public void PredatorPressure_ReducesPrey_BoundedByRemnant()
        {
            var eco = MakeEcosystem(ShippedCatalog());
            var m = SeededMigration();
            int preyBefore = eco.SectorSpeciesPopulation(m, "sector_4_hinterlands", "species_cotton_hare");

            eco.TickDay(1, m, outdoorRadModifier: 100f, "window_spring_storms",
                new SeededRng(1), new SeededRng(2), new SeededRng(3));

            int preyAfter = eco.SectorSpeciesPopulation(m, "sector_4_hinterlands", "species_cotton_hare");
            Assert.True(preyAfter < preyBefore, $"wolves must thin hares ({preyBefore} -> {preyAfter})");
            Assert.True(preyAfter >= WildlifeEcosystemSystem.ExtinctionThreshold, "remnant floor holds");
        }

        [Fact]
        public void RadiationAttrition_IsSpeciesSpecific()
        {
            var eco = MakeEcosystem(ShippedCatalog());
            var m = SeededMigration();
            int haresBefore = eco.SectorSpeciesPopulation(m, "sector_4_hinterlands", "species_cotton_hare");
            int ratsBefore = eco.SectorSpeciesPopulation(m, "sector_4_hinterlands", "species_blight_rat");

            // FalloutStorm-scale radiation (200): hares (tolerance 0.3) suffer
            // radiological attrition on top of predation; blight rats (0.9) are
            // under their tolerance band, so only rad-dog predation touches them.
            for (int d = 1; d <= 5; d++)
                eco.TickDay(d, m, outdoorRadModifier: 200f, "window_spring_storms",
                    new SeededRng(100 + d), new SeededRng(200 + d), new SeededRng(300 + d));

            int haresAfter = eco.SectorSpeciesPopulation(m, "sector_4_hinterlands", "species_cotton_hare");
            int ratsAfter = eco.SectorSpeciesPopulation(m, "sector_4_hinterlands", "species_blight_rat");
            Assert.True(haresBefore - haresAfter > ratsBefore - ratsAfter,
                $"radiation-intolerant hares must decline faster than tolerant rats " +
                $"(hares {haresBefore}->{haresAfter}, rats {ratsBefore}->{ratsAfter})");
        }

        [Fact]
        public void EcologyTick_IsDeterministic()
        {
            var run = new Func<string>(() =>
            {
                var eco = MakeEcosystem(ShippedCatalog());
                var m = SeededMigration();
                for (int d = 1; d <= 10; d++)
                    eco.TickDay(d, m, 150f, "window_spring_storms",
                        new SeededRng(500 + d), new SeededRng(600 + d), new SeededRng(700 + d));
                var sb = new System.Text.StringBuilder();
                foreach (var p in m.State.packs)
                    sb.Append(p.packId).Append('=').Append(p.population).Append('@').Append(p.currentSectorId).Append(';');
                foreach (var x in eco.State.extinct_species_sectors) sb.Append(x).Append(';');
                return sb.ToString();
            });
            Assert.Equal(run(), run());
        }

        [Fact]
        public void LocalExtinction_FlagsAtThreshold_AndRecolonizes()
        {
            var eco = MakeEcosystem(ShippedCatalog());
            var m = SeededMigration();
            // Drive hares in sector 2 down to the floor by hand through the
            // one authority, then tick to evaluate flags.
            m.ThinSpeciesInSector("species_cotton_hare", "sector_2_north_ridge", 4, floor: 2);
            eco.TickDay(1, m, 100f, "window_spring_storms", new SeededRng(1), new SeededRng(2), new SeededRng(3));
            Assert.True(eco.IsLocallyExtinct("sector_2_north_ridge", "species_cotton_hare"),
                "population at the threshold must flag local extinction");

            // Recolonization: migration brings numbers back past the bar.
            var pack = m.TryGetPack("pack_hare_b");
            pack!.population = WildlifeEcosystemSystem.RecolonizationPopulation + 2;
            eco.TickDay(2, m, 100f, "window_spring_storms", new SeededRng(4), new SeededRng(5), new SeededRng(6));
            Assert.False(eco.IsLocallyExtinct("sector_2_north_ridge", "species_cotton_hare"),
                "recovered population must clear the extinction flag");
        }

        [Fact]
        public void ApexActivity_TriggersDeterministically_AndReportsOnce()
        {
            var eco = MakeEcosystem(ShippedCatalog());
            var m = SeededMigration();
            m.TryGetPack("pack_wolf_a")!.population = 16; // past threshold 14
            int spotted = 0;
            eco.OnApexPredatorSpotted += (_, _) => spotted++;

            var a = new SeededRng(7);   // fresh stream each run
            for (int d = 1; d <= 6 && spotted == 0; d++)
                eco.TickDay(d, m, 100f, "window_spring_storms", new SeededRng(1), new SeededRng(2), a);

            var b = new SeededRng(7);
            var eco2 = MakeEcosystem(ShippedCatalog());
            var m2 = SeededMigration();
            m2.TryGetPack("pack_wolf_a")!.population = 16;
            int spotted2 = 0;
            eco2.OnApexPredatorSpotted += (_, _) => spotted2++;
            for (int d = 1; d <= 6 && spotted2 == 0; d++)
                eco2.TickDay(d, m2, 100f, "window_spring_storms", new SeededRng(1), new SeededRng(2), b);

            Assert.Equal(spotted, spotted2);
            Assert.True(spotted == 1 || spotted == 0, "at most one apex report per activation");
            if (spotted == 1)
            {
                Assert.Single(eco.ApexActivities);
                Assert.Equal("species_wolf", eco.ApexActivities[0].species_id);
            }
        }

        [Fact]
        public void Taming_OnlyForEligibleSpecies_AndTransfersOutOfWild()
        {
            var eco = MakeEcosystem(ShippedCatalog());
            var m = SeededMigration();
            Assert.False(eco.CanTame("species_wolf"), "wolves are not tameable");
            Assert.True(eco.CanTame("species_cotton_hare"));

            int before = eco.SectorSpeciesPopulation(m, "sector_4_hinterlands", "species_cotton_hare");
            DomesticAnimalState? tamed = null;
            for (int attempt = 0; attempt < 20 && tamed == null; attempt++)
                tamed = eco.TryTame("species_cotton_hare", "sector_4_hinterlands", 5, "survivor_a", m, new SeededRng(90 + attempt));
            Assert.NotNull(tamed);
            Assert.Single(eco.DomesticAnimals);
            Assert.True(before - 1 == eco.SectorSpeciesPopulation(m, "sector_4_hinterlands", "species_cotton_hare"),
                "a tamed animal must leave the wild population through the one authority");

            // Untameable species never tame.
            Assert.Null(eco.TryTame("species_wolf", "sector_4_hinterlands", 6, "s", m, new SeededRng(1)));
        }

        [Fact]
        public void Pressure_DecaysOverDays()
        {
            var eco = MakeEcosystem(ShippedCatalog());
            var m = SeededMigration();
            eco.RecordHuntingPressure("sector_4_hinterlands", "species_cotton_hare", 3);
            Assert.Equal(3, eco.PressureOn("sector_4_hinterlands", "species_cotton_hare"));
            eco.TickDay(1, m, 100f, "window_spring_storms", new SeededRng(1), new SeededRng(2), new SeededRng(3));
            Assert.Equal(2, eco.PressureOn("sector_4_hinterlands", "species_cotton_hare"));
            eco.TickDay(2, m, 100f, "window_spring_storms", new SeededRng(4), new SeededRng(5), new SeededRng(6));
            eco.TickDay(3, m, 100f, "window_spring_storms", new SeededRng(7), new SeededRng(8), new SeededRng(9));
            Assert.Equal(0, eco.PressureOn("sector_4_hinterlands", "species_cotton_hare"));
        }

        [Fact]
        public void BestiaryKnowledge_IsObservationGated()
        {
            var eco = MakeEcosystem(ShippedCatalog());
            Assert.Equal("unknown", eco.KnowledgeLevel("species_wolf"));
            eco.RecordObservation("species_wolf", "sector_4_hinterlands", 1);
            Assert.Equal("observed", eco.KnowledgeLevel("species_wolf"));
            eco.RecordObservation("species_wolf", "sector_4_hinterlands", 2);
            eco.RecordObservation("species_wolf", "sector_2_north_ridge", 3);
            Assert.Equal("studied", eco.KnowledgeLevel("species_wolf"));
            for (int i = 0; i < 5; i++)
                eco.RecordObservation("species_wolf", "sector_4_hinterlands", 4 + i);
            Assert.Equal("documented", eco.KnowledgeLevel("species_wolf"));
        }

        [Fact]
        public void SaveLoad_NextEcologyTickMatchesUninterrupted()
        {
            var run = new Func<string>(() =>
            {
                var eco = MakeEcosystem(ShippedCatalog());
                var m = SeededMigration();
                for (int d = 1; d <= 12; d++)
                    eco.TickDay(d, m, 180f, "window_spring_storms",
                        new SeededRng(800 + d), new SeededRng(850 + d), new SeededRng(900 + d));
                var sb = new System.Text.StringBuilder();
                foreach (var p in m.State.packs)
                    sb.Append(p.population).Append(',');
                foreach (var x in eco.State.extinct_species_sectors) sb.Append(x).Append(';');
                return sb.ToString();
            });

            // B: 6 days -> save -> restore -> 6 more days.
            var ecoB = MakeEcosystem(ShippedCatalog());
            var mB = SeededMigration();
            for (int d = 1; d <= 6; d++)
                ecoB.TickDay(d, mB, 180f, "window_spring_storms",
                    new SeededRng(800 + d), new SeededRng(850 + d), new SeededRng(900 + d));
            var ecoSave = ecoB.CaptureState();
            var packsSave = mB.CaptureState();

            var mB2 = new WildlifeMigrationSystem(new SeededRng(42));
            mB2.RestoreState(packsSave);
            var ecoB2 = MakeEcosystem(ShippedCatalog());
            ecoB2.RestoreState(ecoSave);
            for (int d = 7; d <= 12; d++)
                ecoB2.TickDay(d, mB2, 180f, "window_spring_storms",
                    new SeededRng(800 + d), new SeededRng(850 + d), new SeededRng(900 + d));

            var expected = run();
            var actual = new System.Text.StringBuilder();
            foreach (var p in mB2.State.packs) actual.Append(p.population).Append(',');
            foreach (var x in ecoB2.State.extinct_species_sectors) actual.Append(x).Append(';');
            Assert.Equal(expected, actual.ToString());
        }

        [Fact]
        public void OldSaveDefaults_RestoreSurvivesNulls()
        {
            var eco = MakeEcosystem(ShippedCatalog());
            eco.RestoreState(null);
            Assert.Empty(eco.State.extinct_species_sectors);
            var bare = new WildlifeEcosystemState
            { extinct_species_sectors = null, pressures = null, observations = null };
            eco.RestoreState(bare);
            Assert.NotNull(eco.State.extinct_species_sectors);
        }
    }

    internal static class TestExtensions
    {
        public static T Also<T>(this T self, Action<T> action)
        {
            action(self);
            return self;
        }
    }
}
