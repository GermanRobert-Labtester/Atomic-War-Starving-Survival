using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radiation;
using Ashfall.Core.StartingLevel;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 78 — decon airlock deterministic, boundary, and stage-machine tests.
    /// Uses synthetic protocol catalogs for exact boundary control, plus one
    /// shipped-catalog load test proving the data authority parses and validates.
    /// </summary>
    public class DeconAirlockSystemTests
    {
        // ─── Helpers ───

        private static DeconProtocolCatalog MakeCatalog()
        {
            return new DeconProtocolCatalog
            {
                protocols = new List<DeconProtocolDef>
                {
                    new DeconProtocolDef
                    {
                        protocol_id = "decon_test_single",
                        display_name = "Single Gate Test Protocol",
                        stages = new List<DeconStageDef>
                        {
                            new DeconStageDef
                            {
                                stage_id = "stage_gate_only", stage_order = 0,
                                duration_ticks = 1, water_liters = 0,
                                external_contamination_multiplier = 0f,
                                effluent_contamination_contribution = 0f,
                                requires_operator = true, operator_skill_factor = 0f
                            }
                        },
                        total_chelator_units = 0,
                        interlock_threshold_mSv_per_h = 0.5f
                    },
                    new DeconProtocolDef
                    {
                        protocol_id = "decon_test_stages",
                        display_name = "Three Stage Test Protocol",
                        stages = new List<DeconStageDef>
                        {
                            new DeconStageDef
                            {
                                stage_id = "stage_alpha", stage_order = 0, duration_ticks = 2,
                                water_liters = 10f, external_contamination_multiplier = 0.2f,
                                effluent_contamination_contribution = 0.1f,
                                requires_operator = true, operator_skill_factor = 0f
                            },
                            new DeconStageDef
                            {
                                stage_id = "stage_beta", stage_order = 1, duration_ticks = 1,
                                water_liters = 5f, external_contamination_multiplier = 0.3f,
                                effluent_contamination_contribution = 0.2f,
                                requires_operator = false, operator_skill_factor = 0f
                            },
                            new DeconStageDef
                            {
                                stage_id = "stage_gate", stage_order = 2, duration_ticks = 1,
                                water_liters = 0, external_contamination_multiplier = 0f,
                                effluent_contamination_contribution = 0f,
                                requires_operator = true, operator_skill_factor = 0f
                            }
                        },
                        total_chelator_units = 0,
                        interlock_threshold_mSv_per_h = 0.5f
                    },
                    new DeconProtocolDef
                    {
                        protocol_id = "decon_test_chelated",
                        display_name = "Chelated Test Protocol",
                        stages = new List<DeconStageDef>
                        {
                            new DeconStageDef
                            {
                                stage_id = "stage_chem", stage_order = 0, duration_ticks = 1,
                                water_liters = 10f, external_contamination_multiplier = 0.5f,
                                effluent_contamination_contribution = 0.4f,
                                requires_operator = true, operator_skill_factor = 0f
                            }
                        },
                        total_chelator_units = 2,
                        interlock_threshold_mSv_per_h = 0.5f
                    }
                },
                effluent_treatment = new DeconEffluentTreatmentDef { default_tank_capacity_liters = 200f },
                gear_disposal = new DeconGearDisposalDef { disposal_threshold = 0.85f }
            };
        }

        private static DecontaminationSystem Create(out Inventory.Inventory inv, DeconProtocolCatalog? catalog = null)
        {
            inv = new Inventory.Inventory();
            var rad = new RadiationSystem(seed: 42);
            var airlock = new AirlockSecuritySystem(new SeededRng(42));
            var sl = new StartingLevelSystem();
            return new DecontaminationSystem(
                new SeededRng(1234), rad, inv, airlock, sl,
                catalog ?? MakeCatalog());
        }

        private static string RunStagedCycle(DecontaminationSystem d, float skill = 0.5f)
        {
            string lastOutcome = string.Empty;
            DeconStageResult r;
            do
            {
                r = d.TickActiveStage(skill);
                if (r.cycleComplete) lastOutcome = r.outcome;
            }
            while (!r.cycleComplete && string.IsNullOrEmpty(r.error));
            return lastOutcome;
        }

        // ─── Deterministic tests ───

        [Fact]
        public void SameContamination_SameProtocol_SamePostWashResult()
        {
            var d1 = Create(out var inv1);
            var d2 = Create(out var inv2);
            inv1.AddById("water_clean", 50); inv1.AddById("soap", 50);
            inv2.AddById("water_clean", 50); inv2.AddById("soap", 50);

            d1.StartProtocolCycle("decon_test_stages", "s1", "gear_a", 0.8f);
            d2.StartProtocolCycle("decon_test_stages", "s1", "gear_a", 0.8f);

            float final1 = 0, final2 = 0;
            DeconStageResult r1, r2;
            do
            {
                r1 = d1.TickActiveStage();
                r2 = d2.TickActiveStage();
                if (r1.cycleComplete) final1 = r1.surfaceContamination;
                if (r2.cycleComplete) final2 = r2.surfaceContamination;
            }
            while (!r1.cycleComplete && string.IsNullOrEmpty(r1.error));

            Assert.Equal(final1, final2, 6);
            Assert.True(final1 < 0.8f, "cycle must reduce external contamination");
        }

        [Fact]
        public void StageOrdering_FollowsProtocolSequence_ExactlyOnce()
        {
            var d = Create(out var inv);
            inv.AddById("water_clean", 50); inv.AddById("soap", 50);
            d.StartProtocolCycle("decon_test_stages", "s1", "gear_a", 0.8f);

            var seen = new List<string>();
            DeconStageResult r;
            do
            {
                r = d.TickActiveStage();
                if (r.stageComplete) seen.Add(r.stageId);
            }
            while (!r.cycleComplete && string.IsNullOrEmpty(r.error));

            Assert.Equal(new[] { "stage_alpha", "stage_beta", "stage_gate" }, seen);
        }

        [Fact]
        public void InterlockThreshold_ExactBoundary_AtThresholdPasses()
        {
            // Gate reading = surfaceContamination * 10; threshold 0.5.
            // Surface exactly 0.05 -> reading 0.5 -> NOT > threshold -> pass.
            var d = Create(out var inv);
            inv.AddById("water_clean", 50); inv.AddById("soap", 50);
            d.StartProtocolCycle("decon_test_single", "s1", "gear_a", 0.05f);
            var outcome = RunStagedCycle(d);
            Assert.Equal("decontaminated", outcome);
            Assert.True(d.CanOpenInnerDoor());
        }

        [Fact]
        public void InterlockThreshold_JustAbove_RequiresRewash()
        {
            var d = Create(out var inv);
            inv.AddById("water_clean", 50); inv.AddById("soap", 50);
            d.StartProtocolCycle("decon_test_single", "s1", "gear_a", 0.0501f);
            var outcome = RunStagedCycle(d);
            Assert.Equal("rewash_required", outcome);
            Assert.False(d.CanOpenInnerDoor());
            Assert.Equal("REWASH REQUIRED", d.InnerDoorFailureReason());
        }

        [Fact]
        public void ZeroReagent_Blocks_WithoutConsumingWater()
        {
            var d = Create(out var inv);
            inv.AddById("water_clean", 10); inv.AddById("soap", 10);
            // No chelator ampoules in inventory.
            var waterBefore = inv.CountById("water_clean");

            var r = d.StartProtocolCycle("decon_test_chelated", "s1", "gear_a", 0.8f);

            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
            Assert.Equal("no_chelator", r.FailureCode);
            Assert.Equal(waterBefore, inv.CountById("water_clean"));
            Assert.Null(d.State.activeCase);
        }

        [Fact]
        public void FullEffluentTank_NeverExceedsCapacity()
        {
            var d = Create(out var inv);
            inv.AddById("water_clean", 50); inv.AddById("soap", 50);
            d.State.effluentTankVolume = d.State.effluentTankCapacity; // exactly full

            d.StartProtocolCycle("decon_test_stages", "s1", "gear_a", 0.8f);
            RunStagedCycle(d);

            Assert.True(d.State.effluentTankVolume <= d.State.effluentTankCapacity + 0.0001f);
        }

        [Fact]
        public void EffluentAccumulation_StableAcrossIdenticalRuns()
        {
            float vol1, cont1, vol2, cont2;
            RunTwoCycles(out vol1, out cont1);
            RunTwoCycles(out vol2, out cont2);
            Assert.Equal(vol1, vol2, 5);
            Assert.Equal(cont1, cont2, 5);
            Assert.True(vol1 > 0f);
            Assert.True(cont1 > 0f);
        }

        private static void RunTwoCycles(out float vol, out float cont)
        {
            var d = Create(out var inv);
            inv.AddById("water_clean", 100); inv.AddById("soap", 100);
            for (int i = 0; i < 2; i++)
            {
                d.StartProtocolCycle("decon_test_stages", "s" + i, "gear_a", 0.8f);
                RunStagedCycle(d);
            }
            vol = d.State.effluentTankVolume;
            cont = d.State.effluentTankContamination;
        }

        [Fact]
        public void DisposalDecision_StableAtExactThreshold()
        {
            var d = Create(out var inv);
            Assert.True(d.ShouldDisposeGear(0.85f));   // exactly at threshold
            Assert.False(d.ShouldDisposeGear(0.8499f)); // just below
        }

        [Fact]
        public void ManualOverride_Logged_AndForcesClearance()
        {
            var d = Create(out var inv);
            inv.AddById("water_clean", 50); inv.AddById("soap", 50);
            d.StartProtocolCycle("decon_test_single", "s1", "gear_a", 0.9f); // will fail gate

            Assert.Equal(ActionResult.StatusKind.Success, d.EngageManualOverride().Status);
            var outcome = RunStagedCycle(d);

            Assert.Equal("decontaminated_override", outcome);
            Assert.Single(d.State.overrideLog);
            Assert.True(d.State.shelterContaminated);
        }

        [Fact]
        public void EmergencyGearDisposal_RequiresBin_AndConsumesGear()
        {
            var d = Create(out var inv);
            inv.AddById("item_sealed_waste_bin", 1);
            inv.AddById("item_heavy_neoprene_scrub_brush", 1);

            var r = d.DisposeContaminatedGear("item_heavy_neoprene_scrub_brush");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Equal(0, inv.CountById("item_heavy_neoprene_scrub_brush"));
            Assert.Single(d.State.disposedGearIds);

            // Second disposal without a bin is blocked
            inv.AddById("item_heavy_neoprene_scrub_brush", 1);
            var r2 = d.DisposeContaminatedGear("item_heavy_neoprene_scrub_brush");
            Assert.Equal(ActionResult.StatusKind.Blocked, r2.Status);
            Assert.Equal("no_waste_bin", r2.FailureCode);
        }

        [Fact]
        public void EffluentTreatment_RecoversWater_PreservesSludge()
        {
            var d = Create(out var inv);
            inv.AddById("water_clean", 100); inv.AddById("soap", 100);
            d.StartProtocolCycle("decon_test_stages", "s1", "gear_a", 0.8f);
            RunStagedCycle(d);

            float sludgeBefore = d.State.effluentSludgeVolume;
            Assert.True(d.State.effluentTankVolume > 0);

            var r = d.TreatEffluent();
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Equal(0f, d.State.effluentTankVolume);
            Assert.Equal(0f, d.State.effluentTankContamination);
            Assert.True(d.State.effluentSludgeVolume > sludgeBefore, "hazardous residue must be preserved as sludge");
            Assert.True(inv.CountById("water_clean") > 0, "recovered process water returned to inventory");
        }

        [Fact]
        public void EffluentFilter_RequiredForTreatment()
        {
            var d = Create(out var inv);
            inv.AddById("water_clean", 50); inv.AddById("soap", 50);
            d.StartProtocolCycle("decon_test_stages", "s1", "gear_a", 0.8f);
            RunStagedCycle(d);

            d.State.effluentFilterInstalled = true;
            d.State.effluentFilterRemainingLiters = 0f; // exhausted

            var r = d.TreatEffluent();
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
            Assert.Equal("filter_exhausted", r.FailureCode);
        }

        // ─── Shipped catalog (data authority) ───

        [Fact]
        public void ShippedProtocolCatalog_Loads_AndPassesValidation()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var dataDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            if (!Directory.Exists(dataDir))
                dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");

            var catalog = DeconProtocolCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.NotEmpty(catalog.protocols);
            // The four authored protocols from the decontamination_protocol_catalog.json
            Assert.Contains(catalog.protocols, p => p.protocol_id == "decon_standard_return");
            Assert.Contains(catalog.protocols, p => p.protocol_id == "decon_emergency_rapid");
            Assert.Contains(catalog.protocols, p => p.protocol_id == "decon_equipment_only");
            Assert.Contains(catalog.protocols, p => p.protocol_id == "decon_maximum_containment");

            var standard = catalog.protocols.Find(p => p.protocol_id == "decon_standard_return")!;
            Assert.Equal(4, standard.stages.Count);
            Assert.Equal("stage_coarse_dust_strip", standard.stages[0].stage_id);
            Assert.Equal("stage_radiometric_gate", standard.stages[3].stage_id);
            Assert.Equal(0.5f, standard.interlock_threshold_mSv_per_h);
        }
    }
}
