// SPDX-License-Identifier: MIT
// Plan 162 — AgricultureSystem behavior tests (catalog, growth composition,
// water/light/toxicity, mutation RNG isolation, pests, compost, narratives).
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Farming;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class AgricultureSystemTests
    {
        private static string FindDataDir()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
            return dataDir ?? string.Empty;
        }

        /// <summary>ISeededRng wrapper that counts every draw (RNG budget tests).</summary>
        private sealed class CountingRng : ISeededRng
        {
            private readonly SeededRng _inner;
            public int Draws;
            public CountingRng(int seed) { _inner = new SeededRng(seed); }
            public int Seed => _inner.Seed;
            public int Next(int minInclusive, int maxExclusive) { Draws++; return _inner.Next(minInclusive, maxExclusive); }
            public float NextFloat() { Draws++; return _inner.NextFloat(); }
            public double NextDouble() { Draws++; return _inner.NextDouble(); }
        }

        private static CropStrainCatalogContainer ShippedCatalog()
        {
            var dataDir = FindDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "Could not locate StreamingAssets/Data");
            return CropStrainCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
        }

        private static AgricultureSystem MakeSystem(
            GreenhouseSystem greenhouse, CropStrainCatalogContainer catalog = null)
        {
            var ag = new AgricultureSystem(greenhouse);
            if (catalog != null) ag.LoadCatalog(catalog);
            return ag;
        }

        private static AgricultureEnvironmentSnapshot Env(
            float lighting = 1000f, float rad = 100f, float ash = 0.04f) =>
            new AgricultureEnvironmentSnapshot
            {
                TemperaturePenaltyC = 0f,
                OutdoorRadModifier = rad,
                LightingAvailabilityPermille = lighting,
                AshContaminationRate = ash,
                SeasonWindowId = "any"
            };

        private static CropStrainCatalogContainer ForcedOutcomeCatalog(
            string outcome, string variantStrainId = "")
        {
            var c = new CropStrainCatalogContainer();
            c.strains.Add(new CropStrainDef
            {
                id = "strain_test_forced",
                display_name = "Forced Test Strain",
                seed_item_id = "item_seed_tuber",
                yield_modifier = 1f,
                pest_susceptibility = 0f,
                mutation_threshold = 0.1f,
                mutation_outcomes =
                {
                    new MutationOutcomeDef { outcome = outcome, result_strain_id = variantStrainId, weight = 1f }
                },
                tags = { "tuber" }
            });
            if (!string.IsNullOrEmpty(variantStrainId))
            {
                c.strains.Add(new CropStrainDef
                {
                    id = variantStrainId,
                    display_name = "Forced Variant",
                    seed_item_id = "item_seed_tuber",
                    mutation_threshold = 1f,
                    pest_susceptibility = 0f
                });
            }
            return c;
        }

        private static CropStrainCatalogContainer AlwaysPestCatalog()
        {
            var c = ForcedOutcomeCatalog("no_mutation");
            c.strains[0].pest_susceptibility = 1f;
            c.pests.Add(new PestDef
            {
                id = "infestation_test_swarm",
                display_name = "Test Swarm",
                base_chance_per_day = 1f,   // guaranteed on the first eligible draw
                severity_step = 0.2f,
                yield_damage_at_full = 0.6f,
                treats_with_item_ids = { "item_pest_treatment_dust" },
                season_tag = "any",
                target_strain_tags = { "tuber" }
            });
            return c;
        }

        /// <summary>Run one planted tuber plot to maturity (7 full-light days;
        /// 6x(100/6) accumulates to 99.999992 in float32, so maturity lands on day 7).</summary>
        private static void RunToMature(AgricultureSystem ag, int days = 7, int startDay = 1, float lighting = 1000f, float rad = 100f)
        {
            for (int d = 0; d < days; d++)
            {
                ag.Water(0, AgriWaterBand.Clean);
                ag.TickDay(startDay + d, Env(lighting, rad), new SeededRng(700 + d), new SeededRng(800 + d));
            }
        }

        // ------------------------------------------------------------------
        // Catalog (plan tests 1-3, 14)
        // ------------------------------------------------------------------

        [Fact]
        public void ShippedCatalog_ValidatesWithZeroDiagnostics()
        {
            var diags = CropStrainCatalogLoader.Validate(ShippedCatalog());
            Assert.True(diags.Count == 0,
                "Expected 0 diagnostics, got:\n" + string.Join("\n", diags.Select(d => d.ToString())));
        }

        [Fact]
        public void ShippedCatalog_SeedItemsResolveInCanonicalCropCatalog()
        {
            var catalog = ShippedCatalog();
            Assert.True(catalog.strains.Count >= 8, "expected a meaningful strain roster");
            foreach (var s in catalog.strains)
            {
                Assert.NotNull(GreenhouseExpansionCatalog.CropCatalog.Get(s.seed_item_id));
            }
        }

        [Fact]
        public void CatalogValidation_RejectsBadDefinitions()
        {
            var bad = new CropStrainCatalogContainer();
            bad.strains.Add(new CropStrainDef { id = "strain_dup", seed_item_id = "item_seed_tuber", yield_modifier = 3f });
            bad.strains.Add(new CropStrainDef { id = "strain_dup", seed_item_id = "item_seed_nope" });
            var diags = CropStrainCatalogLoader.Validate(bad);
            Assert.Contains(diags, d => d.Problem.Contains("duplicate"));
            Assert.Contains(diags, d => d.Problem.Contains("does not resolve in the canonical CropCatalog"));
            Assert.Contains(diags, d => d.Field == "yield_modifier");
        }

        [Fact]
        public void CatalogValidation_RejectsCompostValueLoop()
        {
            var loop = new CropStrainCatalogContainer();
            loop.compost_recipes.Add(new CompostRecipeDef
            {
                id = "recipe_compost_a", input_item_id = "tainted_food", output_item_id = "item_compost_humus"
            });
            loop.compost_recipes.Add(new CompostRecipeDef
            {
                id = "recipe_compost_b", input_item_id = "item_compost_humus", output_item_id = "tainted_food"
            });
            var diags = CropStrainCatalogLoader.Validate(loop);
            Assert.Contains(diags, d => d.Problem.Contains("value loop"));
        }

        // ------------------------------------------------------------------
        // Growth composition (plan tests 4-7)
        // ------------------------------------------------------------------

        [Fact]
        public void Growth_IsDeterministicForSameSeedAndEnvironment()
        {
            var run = new Func<List<string>>(() =>
            {
                var gh = new GreenhouseSystem(seed: 313);
                gh.EnsurePlots(2);
                var ag = MakeSystem(gh, ShippedCatalog());
                ag.PlantWithStrain(0, "strain_tuber_heirloom", 1);
                for (int d = 1; d <= 8; d++)
                {
                    ag.Water(0, AgriWaterBand.Clean);
                    ag.TickDay(d, Env(), new SeededRng(4400 + d), new SeededRng(4500 + d));
                }
                return ag.CaptureState().plots.Select(p => $"{p.strain_id}:{p.medium_quality:F2}:{p.toxicity_permille}:{p.pest_severity:F2}").ToList();
            });

            var a = run();
            var b = run();
            Assert.Equal(a, b);
        }

        [Fact]
        public void Growth_EndOfDayPhaseTransitionsAreExact()
        {
            var gh = new GreenhouseSystem(seed: 7);
            gh.EnsurePlots(1);
            var ag = MakeSystem(gh, ForcedOutcomeCatalog("no_mutation"));
            ag.PlantWithStrain(0, "strain_test_forced", 1);
            Assert.Equal((int)GreenhouseStage.Sprouting, gh.Plots[0].stage);

            // Tuber: 144h to mature at 6h light/day -> ~16.67 growth/day;
            // Growing at >=33 (day 2), Mature at >=100 (float32 lands on day 7).
            for (int d = 1; d <= 7; d++)
            {
                ag.Water(0, AgriWaterBand.Clean);
                ag.TickDay(d, Env(), new SeededRng(100 + d), new SeededRng(200 + d));
                var expected = d < 2 ? GreenhouseStage.Sprouting
                    : (d < 7 ? GreenhouseStage.Growing : GreenhouseStage.Mature);
                Assert.True(gh.Plots[0].stage == (int)expected,
                    $"day {d}: expected {expected}, got stage {gh.Plots[0].stage}");
            }
        }

        [Fact]
        public void Power_LightingOutageHaltsGrowthAndCutsYield()
        {
            var ghDark = new GreenhouseSystem(seed: 9);
            ghDark.EnsurePlots(1);
            var dark = MakeSystem(ghDark, ForcedOutcomeCatalog("no_mutation"));
            dark.PlantWithStrain(0, "strain_test_forced", 1);
            for (int d = 1; d <= 6; d++)
            {
                dark.Water(0, AgriWaterBand.Clean);
                dark.TickDay(d, Env(lighting: 0f), new SeededRng(10), new SeededRng(11));
            }
            Assert.Equal((int)GreenhouseStage.Sprouting, ghDark.Plots[0].stage);
            Assert.True(ghDark.Plots[0].growth < GreenhouseSystem.GrowingThreshold,
                "unpowered grow lights must halt growth (light factor 0)");

            var lit = CalculateYieldMultiplierAtLight(1000f);
            var unlit = CalculateYieldMultiplierAtLight(0f);
            Assert.True(unlit < lit, "light modifier must reduce yield exactly once, monotonically");
        }

        private static float CalculateYieldMultiplierAtLight(float lighting)
        {
            var b = AgricultureSystem.CalculateYield(new AgricultureSystem.YieldInputs
            {
                BaseYield = 4, StrainModifier = 1f, LightFactor = lighting / 1000f,
                WaterBandFactor = 1f, ToxicityPermille = 0, ToxicityTolerancePermille = 300,
                PestSeverity = 0, PestYieldDamageAtFull = 0.5f, MutationOutcome = 0,
                MediumQuality = 100f
            });
            return b.FinalMultiplier;
        }

        [Fact]
        public void Water_BandToxicityAccumulatesOncePerWatering()
        {
            var gh = new GreenhouseSystem(seed: 11);
            gh.EnsurePlots(1);
            var ag = MakeSystem(gh, ForcedOutcomeCatalog("no_mutation"));
            ag.Water(0, AgriWaterBand.Clean);
            Assert.Equal(0, ag.State.plots[0].toxicity_permille);
            ag.Water(0, AgriWaterBand.Marginal);
            Assert.Equal(100, ag.State.plots[0].toxicity_permille); // 50 units * 2‰
            ag.Water(0, AgriWaterBand.Unsafe);
            Assert.Equal(850, ag.State.plots[0].toxicity_permille); // +750, single application
        }

        // ------------------------------------------------------------------
        // Yield bounds (plan tests 8, 13-bounded)
        // ------------------------------------------------------------------

        [Fact]
        public void Toxicity_ReducesYieldWithinBounds()
        {
            var clean = BreakdownAt(tox: 0);
            var toxic = BreakdownAt(tox: 1000);
            Assert.True(toxic.FinalYield < clean.FinalYield, "toxicity must reduce yield");
            Assert.True(toxic.ToxicityModifier >= 0.5f, "toxicity modifier is bounded to at most -50%");
            Assert.True(toxic.ToxicityModifier <= clean.ToxicityModifier);
            Assert.True(toxic.FinalYield >= 0);
        }

        private static YieldBreakdown BreakdownAt(int tox)
        {
            return AgricultureSystem.CalculateYield(new AgricultureSystem.YieldInputs
            {
                BaseYield = 5, StrainModifier = 1f, LightFactor = 1f, WaterBandFactor = 1f,
                ToxicityPermille = tox, ToxicityTolerancePermille = 300,
                PestSeverity = 0f, PestYieldDamageAtFull = 0.6f, MutationOutcome = 0,
                MediumQuality = 100f
            });
        }

        [Fact]
        public void Yield_NeverNegativeNaNOrRunaway()
        {
            var worst = AgricultureSystem.CalculateYield(new AgricultureSystem.YieldInputs
            {
                BaseYield = 5, StrainModifier = 1.5f, LightFactor = 1f, WaterBandFactor = 1f,
                ToxicityPermille = 0, ToxicityTolerancePermille = 300, PestSeverity = 1f,
                PestYieldDamageAtFull = 1f, MutationOutcome = (int)AgriMutationOutcome.SterileSeed,
                MediumQuality = 100f
            });
            Assert.True(worst.FinalYield >= 0 && !float.IsNaN(worst.FinalMultiplier));

            var boosted = AgricultureSystem.CalculateYield(new AgricultureSystem.YieldInputs
            {
                BaseYield = 5, StrainModifier = 1.5f, LightFactor = 1f, WaterBandFactor = 1f,
                ToxicityPermille = 0, ToxicityTolerancePermille = 300, PestSeverity = 0f,
                PestYieldDamageAtFull = 1f, MutationOutcome = (int)AgriMutationOutcome.YieldBoost,
                MediumQuality = 100f
            });
            Assert.True(boosted.FinalYield <= 5 * 2, "final yield is capped at 2x base");
        }

        // ------------------------------------------------------------------
        // Mutation (plan tests 9-12)
        // ------------------------------------------------------------------

        private static AgriMutationOutcome RollForcedOutcomeAtMaturity(string outcome, string variant = "")
        {
            var gh = new GreenhouseSystem(seed: 21);
            gh.EnsurePlots(1);
            var ag = MakeSystem(gh, ForcedOutcomeCatalog(outcome, variant));
            ag.PlantWithStrain(0, "strain_test_forced", 1);
            RunToMature(ag, rad: 300f); // pressure 1.0 >= threshold 0.1
            return (AgriMutationOutcome)ag.State.plots[0].mutation_outcome;
        }

        [Fact]
        public void Mutation_SameSeedSameStateSameOutcome()
        {
            var first = RollForcedOutcomeAtMaturity("toxic_harvest");
            var second = RollForcedOutcomeAtMaturity("toxic_harvest");
            Assert.Equal(first, second);
            Assert.Equal(AgriMutationOutcome.ToxicHarvest, first);
        }

        [Fact]
        public void Mutation_IneligibleConditionsConsumeNoMutationRng()
        {
            var gh = new GreenhouseSystem(seed: 22);
            gh.EnsurePlots(1);
            var catalog = ForcedOutcomeCatalog("toxic_harvest");
            // Threshold 1.0 (clamped max) with pressure 0.333 stays ineligible:
            // the mutation stream must not be touched.
            catalog.strains[0].mutation_threshold = 1f;
            var ag = MakeSystem(gh, catalog);
            ag.PlantWithStrain(0, "strain_test_forced", 1);

            var mutationRng = new CountingRng(33);
            for (int d = 1; d <= 6; d++)
            {
                ag.Water(0, AgriWaterBand.Clean);
                ag.TickDay(d, Env(rad: 100f), new SeededRng(10), mutationRng);
            }
            Assert.Equal(0, mutationRng.Draws);
            Assert.Equal((int)AgriMutationOutcome.None, ag.State.plots[0].mutation_outcome);
        }

        [Fact]
        public void Mutation_ToxicHarvestMarksHarvestContaminated()
        {
            var gh = new GreenhouseSystem(seed: 23);
            gh.EnsurePlots(1);
            var ag = MakeSystem(gh, ForcedOutcomeCatalog("toxic_harvest"));
            ag.PlantWithStrain(0, "strain_test_forced", 1);
            RunToMature(ag, rad: 300f);
            var harvest = ag.Harvest(0);
            Assert.True(harvest.success);
            Assert.True(harvest.contaminated);
            Assert.Equal(GreenhouseExpansionCatalog.Items.TaintedFood, harvest.yieldItemId);
        }

        [Fact]
        public void Mutation_HardyStrainResolvesVariantAndUnlocksOnHarvest()
        {
            var gh = new GreenhouseSystem(seed: 24);
            gh.EnsurePlots(1);
            var ag = MakeSystem(gh, ForcedOutcomeCatalog("hardy_strain", "strain_test_variant"));
            ag.PlantWithStrain(0, "strain_test_forced", 1);
            RunToMature(ag, rad: 300f);
            var p = ag.State.plots[0];
            Assert.Equal(AgriMutationOutcome.HardyStrain, (AgriMutationOutcome)p.mutation_outcome);
            Assert.Equal("strain_test_variant", p.mutation_strain_id);

            var harvest = ag.Harvest(0);
            Assert.True(harvest.success);
            Assert.True(ag.IsStrainUnlocked("strain_test_variant"), "hardy variant persists as unlocked strain");
        }

        // ------------------------------------------------------------------
        // Pests (plan tests 15-16)
        // ------------------------------------------------------------------

        [Fact]
        public void Pest_ProgressionIsDeterministicAndSeverityStepsWithoutRng()
        {
            var run = new Func<List<float>>(() =>
            {
                var gh = new GreenhouseSystem(seed: 31);
                gh.EnsurePlots(1);
                var ag = MakeSystem(gh, AlwaysPestCatalog());
                ag.PlantWithStrain(0, "strain_test_forced", 1);
                var severities = new List<float>();
                for (int d = 1; d <= 4; d++)
                {
                    ag.Water(0, AgriWaterBand.Clean);
                    ag.TickDay(d, Env(), new SeededRng(500 + d), new SeededRng(600 + d));
                    severities.Add(ag.State.plots[0].pest_severity);
                }
                return severities;
            });
            var a = run();
            var b = run();
            Assert.Equal(a, b);
            Assert.True(a[0] > 0f, "guaranteed pest infests on first eligible day");
            Assert.True(a[3] > a[1], "existing infestation worsens deterministically (no rng draws)");
        }

        [Fact]
        public void Pest_TreatmentValidatesItemThenClears()
        {
            var gh = new GreenhouseSystem(seed: 32);
            gh.EnsurePlots(1);
            var ag = MakeSystem(gh, AlwaysPestCatalog());
            ag.PlantWithStrain(0, "strain_test_forced", 1);
            ag.Water(0, AgriWaterBand.Clean);
            ag.TickDay(1, Env(), new SeededRng(1), new SeededRng(2));
            Assert.True(ag.State.plots[0].pest_severity > 0f);

            Assert.False(ag.TryTreatPestInfestation(0, "item_seed_tuber"), "wrong item must be rejected");
            Assert.True(ag.CanTreatPest(0, "item_pest_treatment_dust"));
            Assert.True(ag.TryTreatPestInfestation(0, "item_pest_treatment_dust"));
            Assert.Equal(0f, ag.State.plots[0].pest_severity);
        }

        // ------------------------------------------------------------------
        // Compost (plan tests 13-14)
        // ------------------------------------------------------------------

        [Fact]
        public void Compost_LifecycleStartsWaitsAndCollectsOnce()
        {
            var gh = new GreenhouseSystem(seed: 41);
            var ag = MakeSystem(gh, ShippedCatalog());
            var recipe = ag.CompostRecipe("recipe_compost_humus");
            Assert.NotNull(recipe);

            Assert.True(ag.TryStartCompostBatch("recipe_compost_humus", 10));
            Assert.False(ag.IsCompostReady("recipe_compost_humus", 10 + recipe.duration_days - 1));
            Assert.Equal(0, ag.TryCollectCompost("recipe_compost_humus", 10 + recipe.duration_days - 1));

            Assert.True(ag.IsCompostReady("recipe_compost_humus", 10 + recipe.duration_days));
            Assert.Equal(recipe.output_count, ag.TryCollectCompost("recipe_compost_humus", 10 + recipe.duration_days));
            Assert.Equal(0, ag.TryCollectCompost("recipe_compost_humus", 10 + recipe.duration_days));
        }

        [Fact]
        public void Compost_ApplicationImprovesMediumAndReducesToxicity()
        {
            var gh = new GreenhouseSystem(seed: 42);
            gh.EnsurePlots(1);
            var ag = MakeSystem(gh, ForcedOutcomeCatalog("no_mutation"));
            ag.PlantWithStrain(0, "strain_test_forced", 1);
            ag.Water(0, AgriWaterBand.Unsafe);

            // Let the medium decay for a few days so compost has room to help.
            for (int d = 1; d <= 4; d++)
                ag.TickDay(d, Env(), new SeededRng(70 + d), new SeededRng(80 + d));

            var before = ag.CaptureState().plots[0];
            Assert.True(before.medium_quality < 100f, "medium must decay before compost can improve it");
            Assert.True(ag.TryApplyCompost(0));
            var after = ag.State.plots[0];
            Assert.True(after.medium_quality > before.medium_quality);
            Assert.True(after.toxicity_permille < before.toxicity_permille);
        }

        // ------------------------------------------------------------------
        // Narrative one-shots (plan tests 17-18)
        // ------------------------------------------------------------------

        [Fact]
        public void FirstHarvest_NarrativeFiresExactlyOnce()
        {
            var gh = new GreenhouseSystem(seed: 51);
            gh.EnsurePlots(2);
            var ag = MakeSystem(gh, ForcedOutcomeCatalog("no_mutation"));
            int fired = 0;
            ag.OnFirstHarvest += _ => fired++;

            ag.PlantWithStrain(0, "strain_test_forced", 1);
            RunToMature(ag);
            Assert.True(ag.Harvest(0).success);
            Assert.Equal(1, fired);

            ag.PlantWithStrain(1, "strain_test_forced", 7);
            for (int d = 8; d <= 14; d++)
            {
                ag.Water(1, AgriWaterBand.Clean);
                ag.TickDay(d, Env(), new SeededRng(100 + d), new SeededRng(200 + d));
            }
            Assert.True(ag.Harvest(1).success);
            Assert.Equal(1, fired);
            Assert.True(ag.State.first_harvest_narrative_fired);
        }

        [Fact]
        public void BlightNarrative_FiresOncePerEpisode()
        {
            var gh = new GreenhouseSystem(seed: 52);
            gh.EnsurePlots(1);
            var ag = MakeSystem(gh, ForcedOutcomeCatalog("no_mutation"));
            int fired = 0;
            ag.OnBlightNarrative += _ => fired++;

            ag.NotifyBlightOutbreak(0);
            ag.NotifyBlightOutbreak(0);
            Assert.Equal(1, fired);

            ag.CloseBlightNarrative(0);
            ag.NotifyBlightOutbreak(0);
            Assert.Equal(2, fired);
        }

        // ------------------------------------------------------------------
        // Idempotence
        // ------------------------------------------------------------------

        [Fact]
        public void TickDay_IsIdempotentPerDay()
        {
            var gh = new GreenhouseSystem(seed: 61);
            gh.EnsurePlots(1);
            var ag = MakeSystem(gh, ForcedOutcomeCatalog("no_mutation"));
            ag.PlantWithStrain(0, "strain_test_forced", 1);
            ag.Water(0, AgriWaterBand.Clean);
            ag.TickDay(1, Env(), new SeededRng(1), new SeededRng(2));
            float growthAfterFirst = gh.Plots[0].growth;
            float qualityAfterFirst = ag.State.plots[0].medium_quality;

            ag.Water(0, AgriWaterBand.Clean); // player action must not be eaten by the guard
            ag.TickDay(1, Env(), new SeededRng(1), new SeededRng(2));
            Assert.Equal(growthAfterFirst, gh.Plots[0].growth);
            Assert.Equal(qualityAfterFirst, ag.State.plots[0].medium_quality);
        }

        // ------------------------------------------------------------------
        // Nutrition diversity (plan §5.19-5.20)
        // ------------------------------------------------------------------

        [Fact]
        public void Nutrition_MonotonousDietBecomesDeficientAndDiversityClears()
        {
            var nutrition = new NutritionDiversitySystem();
            nutrition.LoadCatalog(NutritionProfileCatalogLoader.Load(
                FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer()));

            int started = 0, cleared = 0;
            nutrition.OnDeficiencyStarted += (_, _) => started++;
            nutrition.OnDeficiencyCleared += (_, _) => cleared++;

            // 15 days of nothing but tubers (protein/fat/fiber only).
            for (int d = 1; d <= 15; d++)
            {
                nutrition.RecordMeal("survivor_a", "crop_tuber", d);
                nutrition.TickDay(d);
            }
            var deficient = nutrition.Deficiencies("survivor_a");
            Assert.Contains("vitamin_c", deficient);
            Assert.Contains("micronutrients", deficient);
            Assert.True(nutrition.DeficiencyPressure("survivor_a") > 0f);
            Assert.True(started >= 2);

            // Balanced diet covers every category inside the window.
            for (int d = 16; d <= 30; d++)
            {
                nutrition.RecordMeal("survivor_a", "crop_tuber", d);
                nutrition.RecordMeal("survivor_a", "crop_leafy_green", d);
                nutrition.RecordMeal("survivor_a", "crop_oilseed", d);
                nutrition.RecordMeal("survivor_a", "crop_cold_legume", d);
                nutrition.RecordMeal("survivor_a", "crop_mushroom", d);
                nutrition.TickDay(d);
            }
            Assert.Empty(nutrition.Deficiencies("survivor_a"));
            Assert.True(cleared >= 2);
            Assert.Equal(0f, nutrition.DeficiencyPressure("survivor_a"));
        }

        [Fact]
        public void Nutrition_UnmappedFoodIsCaloriesOnly()
        {
            var nutrition = new NutritionDiversitySystem();
            var profile = nutrition.ProfileFor("some_unknown_food");
            Assert.Equal(1f, profile.calories);
            Assert.Equal(0f, profile.protein);
        }
    }
}
