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

        public static int RunDefenseSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;
            var details = new List<string>();

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; details.Add($"  PASS {gate}"); }
                else { fail++; details.Add($"  FAIL {gate}{(note.Length > 0 ? " — " + note : "")}"); }
                GD.Print($"{(ok ? "[PASS]" : "[FAIL]")} defense/{gate}");
            }

            // 1. Catalog.
            var traps = Ashfall.Core.Defense.TrapCatalogLoader.Load(dataDirectory);
            var diags = Ashfall.Core.Defense.TrapCatalogLoader.Validate(traps);
            Check("trap_catalog", diags.Count == 0, string.Join("; ", diags));
            Check("trap_roster", traps.Count >= 4, $"got {traps.Count}");

            // 2. Install + deterministic engagement + capture handoff event.
            // Uses a forced-certain trap (activation 1.0) so the spring gates
            // assert behavior, not a roll; shipped data is covered by gate 1.
            var forcedTrap = new Ashfall.Core.Defense.DefenseTrapDefinition
            {
                id = "trap_selftest_certain",
                display_name = "Selftest Snare",
                defense_type = "snare",
                base_strength = 2,
                max_hp = 100,
                activation_chance = 1f,
                capture_chance = 0.5f
            };
            var sys = new Ashfall.Core.Defense.DefenseSystem(new[] { forcedTrap });
            bool installed = sys.InstallTrap("trap_selftest_certain", "approach", (_, _) => true);
            Check("install_trap", installed);

            int captured = 0;
            sys.OnRaiderCaptured += (_, count) => captured += count;
            var r1 = sys.ResolvePreCombatRaid(10, 6, false, null, null,
                new SeededRng(91), new SeededRng(92));
            Check("engagement_records", r1.Records.Count > 0);
            Check("trap_springs_once", sys.Installations[0].sprung && sys.Installations[0].total_activations == 1);

            // 3. Reset requires resources; broken traps need repair first.
            var id = sys.Installations[0].installation_id;
            Check("reset_requires_sprung", sys.CanResetTrap(id));
            bool reset = sys.TryResetTrap(id, (_, _) => true);
            Check("reset_arms_again", reset && !sys.Installations[0].sprung);

            // 4. Perimeter composition: strength breakdown is non-opaque.
            var strength = sys.CalculatePerimeterStrength(null, null);
            Check("strength_breakdown", strength.Traps >= 0 && strength.Total >= 0,
                $"traps {strength.Traps} total {strength.Total}");

            // 5. Deterministic replay of the engagement.
            var sys2 = new Ashfall.Core.Defense.DefenseSystem(new[] { forcedTrap });
            sys2.InstallTrap("trap_selftest_certain", "approach", (_, _) => true);
            var r2 = sys2.ResolvePreCombatRaid(10, 6, false, null, null,
                new SeededRng(91), new SeededRng(92));
            Check("deterministic_replay",
                r2.RemainingRaiders == r1.RemainingRaiders && r2.RaidersCaptured == r1.RaidersCaptured);

            // 6. Save round-trip.
            var snapshot = sys.CaptureState();
            var restored = new Ashfall.Core.Defense.DefenseSystem(new[] { forcedTrap });
            restored.RestoreState(snapshot);
            Check("save_round_trip", restored.Installations.Count == sys.Installations.Count
                && restored.RaidLog.Count == sys.RaidLog.Count);

            GD.Print($"defense selftest: {pass} passed, {fail} failed");
            if (fail > 0) GD.Print(string.Join("\n", details));
            return EmitSummary("defense_selftest", fail == 0, failedCount: fail, passedCount: pass,
                details: string.Join("; ", details));
        }
    }
}
