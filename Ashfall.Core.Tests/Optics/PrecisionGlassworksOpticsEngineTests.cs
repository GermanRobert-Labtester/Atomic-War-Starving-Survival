// SPDX-License-Identifier: MIT
// Expansion 29 — The Glass : PrecisionGlassworksOpticsEngine focused tests
using Xunit;
using Ashfall.Core.Optics;

namespace Ashfall.Core.Tests.Optics
{
    public sealed class PrecisionGlassworksOpticsEngineTests
    {
        // ── 1. Poor kiln temperature causes thermal shock and batch cracks ──
        [Fact]
        public void AdvanceAnnealingStage_CracksBatch_WhenKilnTemperatureTooLow()
        {
            var batch = new GlassBatchState
            {
                BatchId              = "batch-001",
                SilicaPurityPermille = 900,
                AnnealingStage       = 0
            };

            // Kiln temperature of 0 → maximum deviation → shock accumulates fast
            for (int i = 0; i < 3; i++)
                PrecisionGlassworksOpticsEngine.AdvanceAnnealingStage(batch, 0);

            Assert.True(batch.IsCracked,
                "Extreme temperature deviation should crack the batch");
        }

        // ── 2. Full annealing at optimal temperature yields PrecisionOptic tier ──
        [Fact]
        public void AdvanceAnnealingStage_YieldsPrecisionOptic_WhenOptimalAndHighPurity()
        {
            var batch = new GlassBatchState
            {
                BatchId              = "batch-002",
                SilicaPurityPermille = 980, // >950 → PrecisionOptic
                AnnealingStage       = 0
            };

            // Optimal kiln temperature 900 permille — three stages
            for (int i = 0; i < PrecisionGlassworksOpticsEngine.MaxAnnealingStages; i++)
                PrecisionGlassworksOpticsEngine.AdvanceAnnealingStage(batch, 900);

            Assert.False(batch.IsCracked, "Optimal anneal should not crack the batch");
            Assert.Equal(GlassPurityTier.PrecisionOptic, batch.ResultingTier);
        }

        // ── 3. Crude glass cannot satisfy a prescription tolerance ──
        [Fact]
        public void GrindCorrectionLens_FailsTolerance_WithCrudeGlass()
        {
            var result = PrecisionGlassworksOpticsEngine.GrindCorrectionLens(
                GlassPurityTier.Crude,
                VisionCorrectionBand.MildMyopia,
                grinderSkillPermille: 1000,
                abrasiveGritAvailablePermille: 1000,
                grindSeed: 77);

            Assert.False(result.MeetsPrescriptionTolerance,
                "Crude glass should never meet prescription tolerance");
        }

        // ── 4. PrecisionOptic glass + skilled grinder meets severe myopia tolerance ──
        [Fact]
        public void GrindCorrectionLens_MeetsTolerance_WithPrecisionGlassAndHighSkill()
        {
            var result = PrecisionGlassworksOpticsEngine.GrindCorrectionLens(
                GlassPurityTier.PrecisionOptic,
                VisionCorrectionBand.SevereMyopia,
                grinderSkillPermille: 900,
                abrasiveGritAvailablePermille: 1000,
                grindSeed: 42);

            Assert.False(result.LensCracked, "High-skill grind on PrecisionOptic should not crack");
            Assert.True(result.MeetsPrescriptionTolerance,
                "PrecisionOptic + expert grinder should meet severe myopia tolerance");
        }

        // ── 5. Theodolite calibration is field-ready only with OpticalGrade+ lens ──
        [Fact]
        public void CalibrateTheodolite_NotFieldReady_WithCommonWindowGlass()
        {
            var commonResult = PrecisionGlassworksOpticsEngine.CalibrateTheodolite(
                GlassPurityTier.CommonWindow, calibratorSkillPermille: 1000);

            var precisionResult = PrecisionGlassworksOpticsEngine.CalibrateTheodolite(
                GlassPurityTier.PrecisionOptic, calibratorSkillPermille: 800);

            Assert.False(commonResult.IsFieldReady,
                "CommonWindow glass should not achieve field-ready theodolite calibration");
            Assert.True(precisionResult.IsFieldReady,
                "PrecisionOptic glass with skilled calibrator should be field-ready");
            Assert.True(precisionResult.SurveyRangeMetres > commonResult.SurveyRangeMetres,
                "Higher purity should yield greater survey range");
        }
    }
}
