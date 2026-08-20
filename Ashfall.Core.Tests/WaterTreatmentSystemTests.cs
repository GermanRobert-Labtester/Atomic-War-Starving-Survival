using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WaterTreatmentSystemTests
    {
        private static WaterTreatmentSystem CreateSystem()
        {
            return new WaterTreatmentSystem();
        }

        // ── Water Management ────────────────────────────────────────────

        [Fact]
        public void AddWater_IncreasesTank()
        {
            var wt = CreateSystem();
            var result = wt.AddWater(WaterType.Clean, 10f);
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal(10f, wt.CleanWater);
        }

        [Fact]
        public void AddWater_Negative_Fails()
        {
            var wt = CreateSystem();
            var result = wt.AddWater(WaterType.Clean, -5f);
            Assert.Equal(ActionResult.StatusKind.Failed, result.Status);
        }

        [Fact]
        public void RemoveWater_DecreasesTank()
        {
            var wt = CreateSystem();
            wt.AddWater(WaterType.Clean, 10f);
            var result = wt.RemoveWater(WaterType.Clean, 4f);
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal(6f, wt.CleanWater);
        }

        [Fact]
        public void RemoveWater_MoreThanAvailable_Partial()
        {
            var wt = CreateSystem();
            wt.AddWater(WaterType.Clean, 3f);
            var result = wt.RemoveWater(WaterType.Clean, 10f);
            Assert.Equal(ActionResult.StatusKind.Partial, result.Status);
            Assert.Equal(0f, wt.CleanWater);
        }

        [Fact]
        public void TotalWater_SumsAllTanks()
        {
            var wt = CreateSystem();
            wt.AddWater(WaterType.Clean, 5f);
            wt.AddWater(WaterType.Raw, 3f);
            wt.AddWater(WaterType.Brackish, 2f);
            wt.AddWater(WaterType.Irradiated, 1f);
            Assert.Equal(11f, wt.TotalWater);
        }

        // ── Treatment Jobs ──────────────────────────────────────────────

        [Fact]
        public void StartTreatment_WithoutInput_Blocks()
        {
            var wt = CreateSystem();
            var result = wt.StartTreatment(TreatmentMode.CharcoalFiltration, 5f);
            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.Equal("insufficient_water", result.FailureCode);
        }

        [Fact]
        public void StartCharcoalFiltration_ConsumesRawWater()
        {
            var wt = CreateSystem();
            wt.AddWater(WaterType.Raw, 10f);
            wt.AddCharcoal(5f);

            var result = wt.StartTreatment(TreatmentMode.CharcoalFiltration, 5f);
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.True(wt.IsProcessing);
            Assert.Equal(5f, wt.RawWater); // 10 - 5 consumed
        }

        [Fact]
        public void CharcoalFiltration_CompletesWithCleanWater()
        {
            var wt = CreateSystem();
            wt.AddWater(WaterType.Raw, 10f);
            wt.AddCharcoal(5f);
            wt.StartTreatment(TreatmentMode.CharcoalFiltration, 4f);

            var result = wt.TickTreatment(1.0f); // full day progress
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal("water.treatment_complete", result.MessageKey);
            Assert.False(wt.IsProcessing);

            // 4 raw * 0.85 efficiency = 3.4 clean water
            Assert.Equal(3.4f, wt.CleanWater, 2);
        }

        [Fact]
        public void Distillation_ConsumesFuel()
        {
            var wt = CreateSystem();
            wt.AddWater(WaterType.Brackish, 10f);
            wt.AddFuel(5f);
            wt.StartTreatment(TreatmentMode.Distillation, 4f);

            wt.TickTreatment(1.0f);
            Assert.True(wt.CleanWater > 0);
            // Fuel consumed: 4 * 0.1 = 0.4
            Assert.Equal(4.6f, wt.State.distillationFuel, 2);
        }

        [Fact]
        public void Decontamination_RemovesRadiation()
        {
            var wt = CreateSystem();
            wt.AddWater(WaterType.Irradiated, 5f);
            wt.AddCharcoal(5f);
            wt.StartTreatment(TreatmentMode.Decontamination, 3f);

            float exposure = 0;
            wt.OnRadiationExposure += (dose) => exposure += dose;

            wt.TickTreatment(1.0f);
            Assert.True(wt.CleanWater > 0);
            // Input was irradiated, but decontamination handles it
            // No radiation exposure from treated water
            Assert.Equal(0f, exposure);
        }

        [Fact]
        public void CancelTreatment_LosesInput()
        {
            var wt = CreateSystem();
            wt.AddWater(WaterType.Raw, 10f);
            wt.AddCharcoal(5f);
            wt.StartTreatment(TreatmentMode.CharcoalFiltration, 5f);

            wt.CancelTreatment();
            Assert.False(wt.IsProcessing);
            // Input water was consumed and is lost
            Assert.Equal(5f, wt.RawWater);
            // Charcoal should not have been consumed yet (not completed)
            Assert.Equal(5f, wt.State.charcoalSupply);
        }

        [Fact]
        public void StartTreatment_WhenBusy_Blocks()
        {
            var wt = CreateSystem();
            wt.AddWater(WaterType.Raw, 10f);
            wt.AddCharcoal(5f);
            wt.StartTreatment(TreatmentMode.CharcoalFiltration, 3f);

            var result = wt.StartTreatment(TreatmentMode.CharcoalFiltration, 3f);
            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.Equal("already_processing", result.FailureCode);
        }

        // ── Daily Ration ────────────────────────────────────────────────

        [Fact]
        public void ConsumeRation_UsesCleanWaterFirst()
        {
            var wt = CreateSystem();
            wt.AddWater(WaterType.Clean, 10f);
            wt.AddWater(WaterType.Raw, 5f);

            var result = wt.ConsumeRation(3f);
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal(7f, wt.CleanWater);  // 10 - 3
            Assert.Equal(5f, wt.RawWater);    // untouched
        }

        [Fact]
        public void ConsumeRation_FallsBackToRaw()
        {
            var wt = CreateSystem();
            wt.AddWater(WaterType.Clean, 2f);
            wt.AddWater(WaterType.Raw, 5f);

            var result = wt.ConsumeRation(5f);
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal(0f, wt.CleanWater);  // exhausted
            Assert.Equal(2f, wt.RawWater);    // 5 - 3
        }

        [Fact]
        public void ConsumeRation_WithIrradiated_EmitsExposure()
        {
            var wt = CreateSystem();
            wt.AddWater(WaterType.Irradiated, 5f);

            float radExposure = 0;
            wt.OnRadiationExposure += (dose) => radExposure += dose;

            var result = wt.ConsumeRation(3f);
            Assert.True(radExposure > 0);
            Assert.Equal(2f, wt.State.irradiatedWater); // 5 - 3
        }

        [Fact]
        public void ConsumeRation_Shortfall_ReturnsPartial()
        {
            var wt = CreateSystem();
            wt.AddWater(WaterType.Clean, 2f);

            var result = wt.ConsumeRation(5f);
            Assert.Equal(ActionResult.StatusKind.Partial, result.Status);
            Assert.Equal(0f, wt.CleanWater);
            Assert.Equal(3f, result.Deltas["shortfall"]);
        }

        // ── Resource Management ─────────────────────────────────────────

        [Fact]
        public void ReplaceFilter_ResetsIntegrity()
        {
            var wt = CreateSystem();
            wt.State.filterIntegrity = 45f;

            wt.ReplaceFilter();
            Assert.Equal(100f, wt.FilterIntegrity);
            Assert.Equal(1, wt.State.filterReplacements);
        }

        [Fact]
        public void AddCharcoal_IncreasesSupply()
        {
            var wt = CreateSystem();
            wt.AddCharcoal(10f);
            Assert.Equal(10f, wt.State.charcoalSupply);
        }

        // ── Daily Tick ──────────────────────────────────────────────────

        [Fact]
        public void TickDay_AdvancesActiveTreatment()
        {
            var wt = CreateSystem();
            wt.AddWater(WaterType.Raw, 10f);
            wt.AddCharcoal(5f);
            wt.StartTreatment(TreatmentMode.CharcoalFiltration, 4f);

            wt.TickDay(10);
            Assert.False(wt.IsProcessing); // should complete in one tick
            Assert.True(wt.CleanWater > 0);
        }

        // ── Save / Load ─────────────────────────────────────────────────

        [Fact]
        public void CaptureRestoreState_PreservesWaterLevels()
        {
            var wt = CreateSystem();
            wt.AddWater(WaterType.Clean, 7f);
            wt.AddWater(WaterType.Raw, 3f);
            wt.AddWater(WaterType.Irradiated, 2f);

            var state = wt.CaptureState();
            Assert.Equal(7f, state.cleanWater);
            Assert.Equal(3f, state.rawWater);

            var wt2 = CreateSystem();
            wt2.RestoreState(state);
            Assert.Equal(7f, wt2.CleanWater);
            Assert.Equal(3f, wt2.RawWater);
            Assert.Equal(2f, wt2.IrradiatedWater);
        }

        [Fact]
        public void CaptureRestoreState_PreservesCompletedJobs()
        {
            var wt = CreateSystem();
            wt.AddWater(WaterType.Raw, 10f);
            wt.AddCharcoal(5f);
            wt.StartTreatment(TreatmentMode.CharcoalFiltration, 4f);
            wt.TickTreatment(1.0f);

            var state = wt.CaptureState();
            Assert.Single(state.completedJobs);

            var wt2 = CreateSystem();
            wt2.RestoreState(state);
            Assert.Single(wt2.State.completedJobs);
            Assert.Equal(3.4f, wt2.CleanWater, 2);
        }

        [Fact]
        public void RestoreNullState_DoesNotCrash()
        {
            var wt = CreateSystem();
            wt.RestoreState(null); // should not throw
            Assert.Equal(0f, wt.CleanWater);
        }

        // ── Mass Balance ────────────────────────────────────────────────

        [Fact]
        public void MassBalance_InputEqualsOutputPlusWaste()
        {
            var wt = CreateSystem();
            wt.AddWater(WaterType.Raw, 20f);
            wt.AddCharcoal(5f);

            float initialTotal = wt.TotalWater;
            wt.StartTreatment(TreatmentMode.CharcoalFiltration, 10f);
            wt.TickTreatment(1.0f);

            // Input: 10 raw consumed. Output: 8.5 clean + 1.5 waste (lost).
            // Total should be: 20 - 10 + 8.5 = 18.5
            Assert.Equal(18.5f, wt.TotalWater, 2);
        }
    }
}
