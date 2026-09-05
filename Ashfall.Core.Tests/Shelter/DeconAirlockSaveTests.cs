using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radiation;
using Ashfall.Core.StartingLevel;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 78 — decon airlock save/restore parity and event-suppression tests.
    /// Pins: save mid-cycle → restore → identical result; restore is silent;
    /// reagent consumed exactly once; effluent/disposal ledger round-trips.
    /// </summary>
    public class DeconAirlockSaveTests
    {
        private static DeconProtocolCatalog MakeCatalog()
        {
            var catalog = new DeconProtocolCatalog
            {
                protocols = new List<DeconProtocolDef>
                {
                    new DeconProtocolDef
                    {
                        protocol_id = "decon_save_test",
                        display_name = "Save Test Protocol",
                        stages = new List<DeconStageDef>
                        {
                            new DeconStageDef { stage_id = "stage_s1", stage_order = 0, duration_ticks = 2, water_liters = 10f, external_contamination_multiplier = 0.2f, effluent_contamination_contribution = 0.1f, requires_operator = true, operator_skill_factor = 0f },
                            new DeconStageDef { stage_id = "stage_s2", stage_order = 1, duration_ticks = 2, water_liters = 8f, external_contamination_multiplier = 0.3f, effluent_contamination_contribution = 0.2f, requires_operator = true, operator_skill_factor = 0f },
                            new DeconStageDef { stage_id = "stage_s3", stage_order = 2, duration_ticks = 1, water_liters = 0, external_contamination_multiplier = 0f, effluent_contamination_contribution = 0f, requires_operator = true, operator_skill_factor = 0f }
                        },
                        total_chelator_units = 0,
                        interlock_threshold_mSv_per_h = 0.5f
                    }
                },
                effluent_treatment = new DeconEffluentTreatmentDef { default_tank_capacity_liters = 200f },
                gear_disposal = new DeconGearDisposalDef { disposal_threshold = 0.85f }
            };

            catalog.protocols.Add(new DeconProtocolDef
            {
                protocol_id = "decon_chelated",
                stages = new List<DeconStageDef>
                {
                    new DeconStageDef { stage_id = "stage_c1", stage_order = 0, duration_ticks = 1, water_liters = 5f, external_contamination_multiplier = 0.4f, effluent_contamination_contribution = 0.3f, requires_operator = true, operator_skill_factor = 0f }
                },
                total_chelator_units = 2,
                interlock_threshold_mSv_per_h = 0.5f
            });

            return catalog;
        }

        private static DecontaminationSystem Create(out Inventory.Inventory inv, DeconProtocolCatalog? catalog = null)
        {
            inv = new Inventory.Inventory();
            var rad = new RadiationSystem(seed: 42);
            var airlock = new AirlockSecuritySystem(new SeededRng(42));
            var sl = new StartingLevelSystem();
            return new DecontaminationSystem(new SeededRng(999), rad, inv, airlock, sl, catalog ?? MakeCatalog());
        }

        private static void Seed(Inventory.Inventory inv)
        {
            inv.AddById("water_clean", 100);
            inv.AddById("soap", 100);
        }

        private static string RunToCompletion(DecontaminationSystem d)
        {
            string outcome = string.Empty;
            DeconStageResult r;
            do
            {
                r = d.TickActiveStage();
                if (r.cycleComplete) outcome = r.outcome;
            }
            while (!r.cycleComplete && string.IsNullOrEmpty(r.error));
            return outcome;
        }

        private static string StartAndRunToCompletion(DecontaminationSystem d, string survivor, float contamination, out float finalSurface)
        {
            float observed = float.NaN;
            string outcome = string.Empty;

            void Handler(DeconCase c)
            {
                observed = c.surfaceContamination;
            }

            d.OnCaseCompleted += Handler;
            try
            {
                d.StartProtocolCycle("decon_save_test", survivor, "gear_a", contamination);
                outcome = RunToCompletion(d);
            }
            finally
            {
                d.OnCaseCompleted -= Handler;
            }
            finalSurface = observed;
            return outcome;
        }

        [Fact]
        public void SaveMidCycle_Restore_ContinuesToIdenticalResult()
        {
            // Control: uninterrupted run.
            var control = Create(out var invC);
            Seed(invC);
            float controlSurface;
            string controlOutcome = StartAndRunToCompletion(control, "s1", 0.8f, out controlSurface);

            // Saved run: interrupt mid stage 1, capture, restore into a fresh system, finish.
            var saved = Create(out var invS);
            Seed(invS);
            saved.StartProtocolCycle("decon_save_test", "s1", "gear_a", 0.8f);
            saved.TickActiveStage();
            saved.TickActiveStage(); // stage_s1 has 2 ticks — exactly at the stage boundary
            var captured = saved.CaptureState();

            var restored = Create(out var invR);
            Seed(invR);
            restored.RestoreState(captured);

            float restoredSurface;
            string restoredOutcome = StartAndRunToCompletion(restored, "s1_resume", 0.8f, out restoredSurface);

            Assert.Equal(controlOutcome, restoredOutcome);
            Assert.Equal(controlSurface, restoredSurface, 6);
        }

        [Fact]
        public void SaveMidStage_Restore_ResumesExactStageProgress()
        {
            var saved = Create(out var invS);
            Seed(invS);
            saved.StartProtocolCycle("decon_save_test", "s1", "gear_a", 0.8f);
            saved.TickActiveStage(); // one of two ticks in stage_s1
            var captured = saved.CaptureState();

            var restored = Create(out var invR);
            Seed(invR);
            restored.RestoreState(captured);

            Assert.NotNull(restored.State.activeCase);
            Assert.Equal("stage_s1", restored.State.activeCase!.currentStageId);
            Assert.Equal(1, restored.State.activeCase.stageTicksRemaining);
            Assert.Equal(0, restored.State.activeCase.currentStageIndex);
            // Cycle must still complete with the identical outcome.
            Assert.Equal("rewash_required", RunToCompletion(restored));
        }

        [Fact]
        public void Restore_DoesNotFireCompletionEvent()
        {
            var d = Create(out var inv);
            Seed(inv);
            d.StartProtocolCycle("decon_save_test", "s1", "gear_a", 0.8f);
            d.TickActiveStage();
            var captured = d.CaptureState();

            int completionEvents = 0;
            int changedEvents = 0;
            var d2 = Create(out var inv2);
            Seed(inv2);
            d2.OnCaseCompleted += _ => completionEvents++;
            d2.OnDeconChanged += () => changedEvents++;
            d2.RestoreState(captured);

            Assert.Equal(0, completionEvents);
            Assert.Equal(0, changedEvents);
            Assert.NotNull(d2.State.activeCase);
            Assert.Equal(DeconStatus.InProgress, d2.State.activeCase!.status);
        }

        [Fact]
        public void Restore_PreservesEffluentAndDisposalLedger()
        {
            var d = Create(out var inv);
            Seed(inv);
            inv.AddById("item_sealed_waste_bin", 1);
            inv.AddById("gear_x", 1);
            d.DisposeContaminatedGear("gear_x");
            d.StartProtocolCycle("decon_save_test", "s1", "gear_a", 0.8f);
            d.TickActiveStage();
            d.TickActiveStage();

            var captured = d.CaptureState();
            var d2 = Create(out var inv2);
            Seed(inv2);
            d2.RestoreState(captured);

            Assert.Single(d2.State.disposedGearIds);
            Assert.Equal(d.State.effluentTankVolume, d2.State.effluentTankVolume, 6);
            Assert.Equal(d.State.effluentTankContamination, d2.State.effluentTankContamination, 6);
            Assert.NotNull(d2.State.activeCase);
            Assert.Equal(d.State.activeCase!.currentStageIndex, d2.State.activeCase!.currentStageIndex);
            Assert.Equal(d.State.activeCase.stageTicksRemaining, d2.State.activeCase.stageTicksRemaining);
        }

        [Fact]
        public void ReagentConsumed_ExactlyOnce_PerCycle()
        {
            var d = Create(out var inv);
            Seed(inv);
            inv.AddById("item_decon_chelator_concentrate", 4);

            Assert.Equal(ActionResult.StatusKind.Success, d.StartProtocolCycle("decon_chelated", "s1", "gear_a", 0.8f).Status);
            Assert.Equal(2, inv.CountById("item_decon_chelator_concentrate")); // exactly 2 consumed, not 4
        }

        [Fact]
        public void Recapture_AfterRestore_IsNormalized()
        {
            var d = Create(out var inv);
            Seed(inv);
            d.StartProtocolCycle("decon_save_test", "s1", "gear_a", 0.8f);
            d.TickActiveStage();
            var first = d.CaptureState();

            var d2 = Create(out var inv2);
            Seed(inv2);
            d2.RestoreState(first);
            var second = d2.CaptureState();

            Assert.Equal(
                new SystemTextJsonSerializer().Serialize(first),
                new SystemTextJsonSerializer().Serialize(second));
        }
    }
}
