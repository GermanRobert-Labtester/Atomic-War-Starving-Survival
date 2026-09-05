// SPDX-License-Identifier: MIT
// ============================================================================
// HostCli Partial : Plans 162-165 selftests
// Plan 162        : --agriculture-selftest — catalog validation, growth
//                   composition, mutation RNG isolation, compost lifecycle,
//                   and nutrition diversity over a deterministic 10-day run.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Farming;
using Godot;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunAgricultureSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;
            var details = new List<string>();

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; details.Add($"  PASS {gate}"); }
                else { fail++; details.Add($"  FAIL {gate}{(note.Length > 0 ? " — " + note : "")}"); }
                GD.Print($"{(ok ? "[PASS]" : "[FAIL]")} agriculture/{gate}");
            }

            // 1. Catalog validation.
            var io = CatalogPath.CreateFileIOForDataDir(dataDirectory);
            var json = new SystemTextJsonSerializer();
            var catalog = CropStrainCatalogLoader.Load(dataDirectory, io, json);
            var diags = CropStrainCatalogLoader.Validate(catalog);
            Check("crop_strains_catalog", diags.Count == 0,
                diags.Count > 0 ? string.Join("; ", diags) : "");
            Check("strain_roster", catalog.strains.Count >= 8, $"got {catalog.strains.Count}");

            // 2. Deterministic 10-day growth + mutation + compost scenario.
            var gh = new GreenhouseSystem(seed: 1986);
            gh.EnsurePlots(2);
            var ag = new AgricultureSystem(gh);
            ag.LoadCatalog(catalog);
            bool planted = ag.PlantWithStrain(0, "strain_tuber_heirloom", 1);
            Check("plant_with_strain", planted);

            var env = new AgricultureEnvironmentSnapshot
            {
                TemperaturePenaltyC = 0f,
                OutdoorRadModifier = 100f,
                LightingAvailabilityPermille = 1000f,
                AshContaminationRate = 0.04f,
                SeasonWindowId = "any"
            };
            for (int d = 1; d <= 8; d++)
            {
                ag.Water(0, AgriWaterBand.Clean);
                ag.TickDay(d, env, new SeededRng(3100 + d), new SeededRng(3200 + d));
            }
            Check("growth_reaches_mature",
                gh.Plots[0].stage == (int)GreenhouseStage.Mature,
                $"stage {gh.Plots[0].stage}");

            // 3. Mutation isolation: same forks → same outcome on a replay.
            var replayGh = new GreenhouseSystem(seed: 1986);
            replayGh.EnsurePlots(2);
            var replay = new AgricultureSystem(replayGh);
            replay.LoadCatalog(catalog);
            replay.PlantWithStrain(0, "strain_tuber_heirloom", 1);
            for (int d = 1; d <= 8; d++)
            {
                replay.Water(0, AgriWaterBand.Clean);
                replay.TickDay(d, env, new SeededRng(3100 + d), new SeededRng(3200 + d));
            }
            Check("deterministic_replay",
                replay.State.plots[0].mutation_outcome == ag.State.plots[0].mutation_outcome
                && replayGh.Plots[0].growth == gh.Plots[0].growth);

            // 4. Harvest is bounded and first-harvest narrative is one-shot.
            int firstFires = 0;
            ag.OnFirstHarvest += _ => firstFires++;
            var harvest = ag.Harvest(0);
            Check("harvest_bounded",
                harvest.success && harvest.finalAmount >= 0 && harvest.finalAmount <= harvest.baseAmount * 2,
                $"final {harvest.finalAmount} base {harvest.baseAmount}");
            Check("first_harvest_once", firstFires == 1, $"fired {firstFires}");

            // 5. Compost lifecycle.
            Check("compost_recipe", ag.CompostRecipe("recipe_compost_humus") != null);
            ag.TryStartCompostBatch("recipe_compost_humus", 1);
            Check("compost_collect_after_duration",
                ag.TryCollectCompost("recipe_compost_humus", 1 + ag.CompostRecipe("recipe_compost_humus")!.duration_days) > 0);

            // 6. Nutrition diversity over a monotonous diet.
            var nutrition = new NutritionDiversitySystem();
            nutrition.LoadCatalog(NutritionProfileCatalogLoader.Load(dataDirectory, io, json));
            for (int d = 1; d <= 15; d++)
            {
                nutrition.RecordMeal("s1", "crop_tuber", d);
                nutrition.TickDay(d);
            }
            Check("nutrition_deficiency_detected",
                nutrition.Deficiencies("s1").Count > 0 && nutrition.DeficiencyPressure("s1") > 0f);

            // 7. Save capture/restore round-trip.
            var snapshot = ag.CaptureState();
            var restored = new AgricultureSystem(new GreenhouseSystem(seed: 1));
            restored.LoadCatalog(catalog);
            restored.RestoreState(snapshot);
            Check("save_round_trip", restored.State.plots.Count == ag.State.plots.Count);

            GD.Print($"agriculture selftest: {pass} passed, {fail} failed");
            if (fail > 0) GD.Print(string.Join("\n", details));
            return EmitSummary("agriculture_selftest", fail == 0, failedCount: fail, passedCount: pass,
                details: string.Join("; ", details));
        }
    }
}
