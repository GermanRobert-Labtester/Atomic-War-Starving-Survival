// SPDX-License-Identifier: MIT
// Plan 162 — Agriculture save/load: deep-copy capture, partial-growth
// save/restore continuation equivalence, nutrition state round-trip.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Farming;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class AgriculturePersistenceTests
    {
        private static CropStrainCatalogContainer TestCatalog()
        {
            var c = new CropStrainCatalogContainer();
            c.strains.Add(new CropStrainDef
            {
                id = "strain_test_tuber",
                display_name = "Test Tuber",
                seed_item_id = "item_seed_tuber",
                mutation_threshold = 1f,
                pest_susceptibility = 0f
            });
            c.compost_recipes.Add(new CompostRecipeDef
            {
                id = "recipe_compost_test",
                input_item_id = "tainted_food",
                input_count = 2,
                output_item_id = "item_compost_humus",
                output_count = 1,
                duration_days = 3
            });
            return c;
        }

        private static AgricultureEnvironmentSnapshot Env() => new AgricultureEnvironmentSnapshot
        {
            TemperaturePenaltyC = 0f,
            OutdoorRadModifier = 100f,
            LightingAvailabilityPermille = 1000f,
            AshContaminationRate = 0.04f,
            SeasonWindowId = "any"
        };

        private static void TickOneDay(AgricultureSystem ag, int day, bool water = true)
        {
            if (water) ag.Water(0, AgriWaterBand.Clean);
            ag.TickDay(day, Env(), new SeededRng(900 + day), new SeededRng(950 + day));
        }

        private static AgricultureSystem NewPlanted(out GreenhouseSystem gh)
        {
            gh = new GreenhouseSystem(seed: 77);
            gh.EnsurePlots(1);
            var ag = new AgricultureSystem(gh);
            ag.LoadCatalog(TestCatalog());
            ag.PlantWithStrain(0, "strain_test_tuber", 1);
            return ag;
        }

        private static string Snapshot(AgricultureSystem ag, GreenhouseSystem gh)
        {
            var p = ag.State.plots[0];
            var g = gh.Plots[0];
            return $"{g.stage}|{g.growth:F4}|{g.water:F2}|{g.soilContamination:F2}|{g.blight:F3}|"
                   + $"{p.strain_id}|{p.medium_quality:F3}|{p.toxicity_permille}|{p.pest_severity:F2}|{p.mutation_outcome}|"
                   + $"{ag.State.compost_batches.Count}|{gh.TotalHarvests}";
        }

        [Fact]
        public void CaptureState_IsADeepCopyNotAnAlias()
        {
            var ag = NewPlanted(out var gh);
            TickOneDay(ag, 1);
            var snapshot = ag.CaptureState();

            TickOneDay(ag, 2);
            TickOneDay(ag, 3);

            Assert.NotEqual(
                snapshot.plots[0].medium_quality,
                ag.State.plots[0].medium_quality);
        }

        [Fact]
        public void SaveRestore_ContinuationMatchesUninterruptedRun()
        {
            // Run A: 6 uninterrupted days.
            var agA = NewPlanted(out var ghA);
            agA.TryStartCompostBatch("recipe_compost_test", 1);
            for (int d = 1; d <= 6; d++) TickOneDay(agA, d);

            // Run B: 3 days -> capture -> fresh composition -> restore -> 3 days.
            var agB1 = NewPlanted(out var ghB1);
            agB1.TryStartCompostBatch("recipe_compost_test", 1);
            for (int d = 1; d <= 3; d++) TickOneDay(agB1, d);

            var agriSave = agB1.CaptureState();
            var ghSave = ghB1.CaptureState();

            var ghB2 = new GreenhouseSystem(seed: 77);
            var agB2 = new AgricultureSystem(ghB2);
            agB2.LoadCatalog(TestCatalog());
            ghB2.RestoreState(ghSave);
            agB2.RestoreState(agriSave);
            for (int d = 4; d <= 6; d++) TickOneDay(agB2, d);

            Assert.Equal(Snapshot(agA, ghA), Snapshot(agB2, ghB2));
        }

        [Fact]
        public void RestoreState_OldSaveDefaultsSurvive()
        {
            var gh = new GreenhouseSystem(seed: 78);
            gh.EnsurePlots(1);
            var ag = new AgricultureSystem(gh);
            ag.LoadCatalog(TestCatalog());

            // Pre-feature (bare) save: null-tolerant restore, empty defaults.
            ag.RestoreState(null);
            Assert.Empty(ag.State.plots);
            Assert.False(ag.State.first_harvest_narrative_fired);

            var bare = new AgricultureState(); // old-format section with defaults
            bare.plots = null;
            ag.RestoreState(bare);
            Assert.NotNull(ag.State.plots);
        }

        [Fact]
        public void UnlockedStrainsAndNarrativeFlags_PersistAcrossRoundTrip()
        {
            var ag = NewPlanted(out _);
            ag.UnlockStrain("strain_test_tuber");
            ag.NotifyBlightOutbreak(0);

            var restored = new AgricultureSystem(new GreenhouseSystem(seed: 1));
            restored.LoadCatalog(TestCatalog());
            restored.RestoreState(ag.CaptureState());

            Assert.True(restored.IsStrainUnlocked("strain_test_tuber"));
            Assert.Equal(new List<int> { 0 }, restored.State.blight_narrative_plots);
        }

        [Fact]
        public void NutritionState_RoundTripsAndDrivesContinuation()
        {
            var a = new NutritionDiversitySystem();
            a.RecordMeal("s1", "crop_tuber", 1);
            a.RecordMeal("s1", "crop_leafy_green", 1);
            a.TickDay(1);

            var b = new NutritionDiversitySystem();
            b.RestoreState(a.CaptureState());

            Assert.Equal(a.Deficiencies("s1"), b.Deficiencies("s1"));
            Assert.Equal(a.DiversityRatio("s1"), b.DiversityRatio("s1"));

            // Continuation equivalence: same meals on the restored copy.
            a.RecordMeal("s1", "crop_oilseed", 2);
            b.RecordMeal("s1", "crop_oilseed", 2);
            a.TickDay(2);
            b.TickDay(2);
            Assert.Equal(a.Deficiencies("s1"), b.Deficiencies("s1"));
        }
    }
}
