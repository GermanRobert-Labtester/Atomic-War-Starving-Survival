using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Tests
{
    public class IndustrialCapabilityExpansionTests
    {
        // ─── Plan 110: Chlor-Alkali Synthesis Tests ───

        [Fact]
        public void ChlorAlkali_ProcessStart_ConsumesFeedstockAtomically()
        {
            var inv = new Inventory.Inventory();
            inv.AddById(ChlorAlkaliSynthesisEngine.ItemRockSalt, 5);
            var rng = new SeededRng(1001);
            var engine = new ChlorAlkaliSynthesisEngine(inv, rng, () => 5000f);

            var catalog = new ChlorAlkaliCatalog();
            var process = new ChlorAlkaliProcessDef
            {
                process_id = "process_chlor_alkali_brine_electrolysis",
                display_name = "Brine Electrolysis",
                power_kw = 2.5f,
                duration_ticks = 2,
                sanitation_output_units = 6,
                caustic_output_units = 3
            };
            process.feedstock_costs.Add(new ChlorAlkaliFeedstockCost { item_id = ChlorAlkaliSynthesisEngine.ItemRockSalt, amount = 2 });
            catalog.processes.Add(process);
            engine.LoadCatalog(catalog);

            var res = engine.StartProcess("process_chlor_alkali_brine_electrolysis");
            Assert.True(res.IsSuccess);
            Assert.Equal(3, inv.CountById(ChlorAlkaliSynthesisEngine.ItemRockSalt));
            Assert.Equal(ChlorAlkaliProcessStatus.Processing, engine.State.status);
        }

        [Fact]
        public void ChlorAlkali_ProcessStart_BlockedWhenPowerInsufficient()
        {
            var inv = new Inventory.Inventory();
            inv.AddById(ChlorAlkaliSynthesisEngine.ItemRockSalt, 5);
            var engine = new ChlorAlkaliSynthesisEngine(inv, new SeededRng(1002), () => 500f); // only 0.5 kW

            var catalog = new ChlorAlkaliCatalog();
            var process = new ChlorAlkaliProcessDef
            {
                process_id = "process_high_power",
                power_kw = 3.0f // requires 3.0 kW
            };
            catalog.processes.Add(process);
            engine.LoadCatalog(catalog);

            var res = engine.StartProcess("process_high_power");
            Assert.False(res.IsSuccess);
            Assert.Equal("insufficient_power", res.FailureCode);
        }

        [Fact]
        public void ChlorAlkali_ProcessTick_CompletesBatchAndProducesOutputs()
        {
            var inv = new Inventory.Inventory();
            inv.AddById(ChlorAlkaliSynthesisEngine.ItemRockSalt, 10);
            var engine = new ChlorAlkaliSynthesisEngine(inv, new SeededRng(1003), () => 10000f);

            var catalog = new ChlorAlkaliCatalog();
            var process = new ChlorAlkaliProcessDef
            {
                process_id = "test_proc",
                power_kw = 1.0f,
                duration_ticks = 1,
                process_efficiency = 1.0f,
                sanitation_output_units = 4,
                caustic_output_units = 2
            };
            catalog.processes.Add(process);
            engine.LoadCatalog(catalog);

            engine.StartProcess("test_proc");
            engine.TickDay(1);

            Assert.Equal(4, inv.CountById(ChlorAlkaliSynthesisEngine.ItemBleach));
            Assert.Equal(2, inv.CountById(ChlorAlkaliSynthesisEngine.ItemCausticSoda));
            Assert.Equal(1, engine.State.completedBatches);
        }

        [Fact]
        public void ChlorAlkali_CaptureRestore_PreservesFullState()
        {
            var inv = new Inventory.Inventory();
            var engine = new ChlorAlkaliSynthesisEngine(inv, new SeededRng(1004));
            engine.State.membraneHealth = 0.45f;
            engine.State.completedBatches = 7;
            engine.State.hazardLoad = 0.35f;

            var snapshot = engine.CaptureState();
            var restoredEngine = new ChlorAlkaliSynthesisEngine(inv, new SeededRng(1004));
            restoredEngine.RestoreState(snapshot);

            Assert.Equal(0.45f, restoredEngine.State.membraneHealth, 3);
            Assert.Equal(7, restoredEngine.State.completedBatches);
            Assert.Equal(0.35f, restoredEngine.State.hazardLoad, 3);
        }

        // ─── Plan 111: Solar Concentrator Tests ───

        [Fact]
        public void SolarConcentrator_OutputCalculatedCorrectly()
        {
            var inv = new Inventory.Inventory();
            var engine = new SolarConcentratorEngine(inv, new SeededRng(2001), () => 0.9f); // 90% solar insolation

            var catalog = new SolarConcentratorCatalog();
            catalog.concentrators.Add(new SolarConcentratorDef
            {
                concentrator_id = "solar_dish_medium",
                max_thermal_kw = 10.0f,
                stirling_output_kw = 2.0f,
                optical_efficiency = 0.8f,
                tracking_quality = 1.0f
            });
            engine.LoadCatalog(catalog);
            engine.State.mirrorReflectivity = 1.0f;
            engine.State.stirlingAttached = true;

            engine.RecalculateOutputs(0.9f);

            Assert.True(engine.State.currentThermalKw > 5.0f);
            Assert.True(engine.State.currentElectricalKw > 1.0f);
        }

        [Fact]
        public void SolarConcentrator_StormFoulsReflectivity_CleaningRestoresIt()
        {
            var inv = new Inventory.Inventory();
            var engine = new SolarConcentratorEngine(inv, new SeededRng(2002));
            engine.State.mirrorReflectivity = 1.0f;

            engine.ApplyAshStormFouling(0.15f);
            Assert.True(engine.State.mirrorReflectivity < 0.90f);

            var cleanRes = engine.CleanMirrors();
            Assert.True(cleanRes.IsSuccess);
            Assert.True(engine.State.mirrorReflectivity >= 0.95f);
        }

        [Fact]
        public void SolarConcentrator_SolarDistillation_ProducesCleanWater()
        {
            var inv = new Inventory.Inventory();
            var engine = new SolarConcentratorEngine(inv, new SeededRng(2003));
            engine.State.currentThermalKw = 4.0f; // ample thermal energy

            var waterSys = new WaterTreatmentSystem();
            waterSys.AddWater(WaterType.Brackish, 20.0f);

            var res = engine.PerformSolarDistillation(waterSys, 10.0f);
            Assert.True(res.IsSuccess);
            Assert.Equal(10.0f, waterSys.BrackishWater, 1);
            Assert.True(waterSys.CleanWater >= 7.0f);
        }

        [Fact]
        public void SolarConcentrator_CaptureRestore_PreservesState()
        {
            var inv = new Inventory.Inventory();
            var engine = new SolarConcentratorEngine(inv, new SeededRng(2005));
            engine.State.mirrorReflectivity = 0.72f;
            engine.State.alignmentQuality = 0.91f;
            engine.State.trackingMode = SolarTrackingMode.Motorized;

            var snapshot = engine.CaptureState();
            var restored = new SolarConcentratorEngine(inv, new SeededRng(2005));
            restored.RestoreState(snapshot);

            Assert.Equal(0.72f, restored.State.mirrorReflectivity, 2);
            Assert.Equal(0.91f, restored.State.alignmentQuality, 2);
            Assert.Equal(SolarTrackingMode.Motorized, restored.State.trackingMode);
        }

        // ─── Plan 112: Precision Optics Tests ───

        [Fact]
        public void PrecisionOptics_StartWorkpiece_ConsumesBlank()
        {
            var inv = new Inventory.Inventory();
            inv.AddById(PrecisionOpticsEngine.ItemGlassBlank, 2);
            var engine = new PrecisionOpticsEngine(inv, new SeededRng(3001));

            var catalog = new PrecisionOpticsCatalog();
            catalog.recipes.Add(new OpticRecipeDef
            {
                optic_recipe_id = "optic_periscope_prism",
                display_name = "Periscope Prism",
                blank_item_id = PrecisionOpticsEngine.ItemGlassBlank
            });
            engine.LoadCatalog(catalog);

            var res = engine.StartWorkpiece("optic_periscope_prism");
            Assert.True(res.IsSuccess);
            Assert.Equal(1, inv.CountById(PrecisionOpticsEngine.ItemGlassBlank));
            Assert.NotNull(engine.State.activeWorkpiece);
        }

        [Fact]
        public void PrecisionOptics_AdvanceWork_CompletesStagesAndFinalizes()
        {
            var inv = new Inventory.Inventory();
            inv.AddById(PrecisionOpticsEngine.ItemGlassBlank, 1);
            var engine = new PrecisionOpticsEngine(inv, new SeededRng(3002));

            var catalog = new PrecisionOpticsCatalog();
            var recipe = new OpticRecipeDef
            {
                optic_recipe_id = "optic_test_lens",
                display_name = "Test Lens",
                blank_item_id = PrecisionOpticsEngine.ItemGlassBlank,
                max_quality = 0.90f
            };
            recipe.stages.Add(new OpticStageDef { stage = "grind", work_units = 5.0f, quality_gain = 0.30f });
            recipe.stages.Add(new OpticStageDef { stage = "polish", work_units = 5.0f, quality_gain = 0.40f });
            catalog.recipes.Add(recipe);
            engine.LoadCatalog(catalog);

            engine.StartWorkpiece("optic_test_lens");

            // Advance stage 1
            engine.AdvanceWork(6.0f);
            Assert.Equal(1, engine.State.activeWorkpiece!.currentStageIndex);

            // Advance stage 2
            engine.AdvanceWork(6.0f);
            Assert.True(engine.State.activeWorkpiece!.isCompleted);

            // Complete and receive item
            var compRes = engine.CompleteOptic("item_test_finished_lens");
            Assert.True(compRes.IsSuccess);
            Assert.Equal(1, inv.CountById("item_test_finished_lens"));
            Assert.Null(engine.State.activeWorkpiece);
            Assert.Equal(1, engine.State.completedOpticsCount);
        }

        [Fact]
        public void PrecisionOptics_FoucaultKnifeEdgeTest_FiguringGain()
        {
            var inv = new Inventory.Inventory();
            inv.AddById(PrecisionOpticsEngine.ItemGlassBlank, 1);
            inv.AddById(PrecisionOpticsEngine.ItemFoucaultRig, 1);
            var engine = new PrecisionOpticsEngine(inv, new SeededRng(3003));

            var catalog = new PrecisionOpticsCatalog();
            catalog.recipes.Add(new OpticRecipeDef
            {
                optic_recipe_id = "optic_parabolic_mirror",
                display_name = "Parabolic Primary",
                blank_item_id = PrecisionOpticsEngine.ItemGlassBlank
            });
            engine.LoadCatalog(catalog);

            engine.StartWorkpiece("optic_parabolic_mirror");
            float initialAberration = engine.State.activeWorkpiece!.figureAberration;

            var testRes = engine.TestFigureWithFoucault();
            Assert.True(testRes.IsSuccess);
            Assert.True(engine.State.activeWorkpiece.figureAberration < initialAberration);
        }

        // ─── Plan 113: Ballistic Shield Tests ───

        [Fact]
        public void BallisticShield_EquipAndChangeStance()
        {
            var inv = new Inventory.Inventory();
            var engine = new BallisticShieldEngine(inv, new SeededRng(4001));

            var catalog = new BallisticShieldCatalog();
            catalog.shields.Add(new BallisticShieldDef
            {
                shield_id = "shield_heavy_breaching",
                display_name = "Heavy Breaching Shield",
                integrity_max = 120.0f,
                anchor_supported = true
            });
            engine.LoadCatalog(catalog);

            var equipRes = engine.EquipShield("shield_heavy_breaching");
            Assert.True(equipRes.IsSuccess);
            Assert.Equal(ShieldStance.Carried, engine.State.stance);
            Assert.Equal(120.0f, engine.State.currentIntegrity);

            var stanceRes = engine.SetStance(ShieldStance.Braced);
            Assert.True(stanceRes.IsSuccess);
            Assert.Equal(ShieldStance.Braced, engine.State.stance);
        }

        [Fact]
        public void BallisticShield_InterceptDamage_FrontalBlocked_FlankPenetrates()
        {
            var inv = new Inventory.Inventory();
            var engine = new BallisticShieldEngine(inv, new SeededRng(4002));

            var catalog = new BallisticShieldCatalog();
            catalog.shields.Add(new BallisticShieldDef
            {
                shield_id = "shield_riot_polycarbonate",
                display_name = "Riot Shield",
                coverage_arc_deg = 90.0f,
                frontal_block_rating = 0.80f,
                integrity_max = 100.0f
            });
            engine.LoadCatalog(catalog);
            engine.EquipShield("shield_riot_polycarbonate");
            engine.SetStance(ShieldStance.Raised);

            // Frontal hit (angle = 0 deg)
            var frontBlock = engine.InterceptDamage(40.0f, 0.0f);
            Assert.True(frontBlock.success);
            Assert.True(frontBlock.absorbedDamage > 25.0f);
            Assert.True(frontBlock.penetratingDamage < 15.0f);
            Assert.Equal(1, engine.State.totalShotsBlocked);

            // Flank hit (angle = 120 deg > 45 deg half-arc)
            var flankBlock = engine.InterceptDamage(40.0f, 120.0f);
            Assert.False(flankBlock.success);
            Assert.Equal(0f, flankBlock.absorbedDamage);
            Assert.Equal(40.0f, flankBlock.penetratingDamage);
        }

        [Fact]
        public void BallisticShield_MassiveDamage_ShattersShield()
        {
            var inv = new Inventory.Inventory();
            var engine = new BallisticShieldEngine(inv, new SeededRng(4003));

            var catalog = new BallisticShieldCatalog();
            catalog.shields.Add(new BallisticShieldDef
            {
                shield_id = "shield_scrap",
                display_name = "Scrap Shield",
                coverage_arc_deg = 80.0f,
                frontal_block_rating = 0.70f,
                integrity_max = 30.0f
            });
            engine.LoadCatalog(catalog);
            engine.EquipShield("shield_scrap");
            engine.SetStance(ShieldStance.Raised);

            var block = engine.InterceptDamage(100.0f, 0.0f);
            Assert.True(block.shattered);
            Assert.Equal(ShieldStance.Broken, engine.State.stance);
        }

        [Fact]
        public void BallisticShield_Anchoring_DeploysSpikes()
        {
            var inv = new Inventory.Inventory();
            var engine = new BallisticShieldEngine(inv, new SeededRng(4004));

            var catalog = new BallisticShieldCatalog();
            catalog.shields.Add(new BallisticShieldDef
            {
                shield_id = "shield_trench_mantlet",
                display_name = "Trench Mantlet",
                anchor_supported = true
            });
            engine.LoadCatalog(catalog);
            engine.EquipShield("shield_trench_mantlet");

            var anchorRes = engine.AnchorToGround();
            Assert.True(anchorRes.IsSuccess);
            Assert.Equal(ShieldStance.Anchored, engine.State.stance);
            Assert.True(engine.State.isAnchored);

            var unanchorRes = engine.Unanchor();
            Assert.True(unanchorRes.IsSuccess);
            Assert.False(engine.State.isAnchored);
        }
    }
}
