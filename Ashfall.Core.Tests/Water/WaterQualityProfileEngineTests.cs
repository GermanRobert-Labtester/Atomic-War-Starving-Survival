// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Water;
using Xunit;

namespace Ashfall.Core.Tests.Water
{
    public sealed class WaterQualityProfileEngineTests
    {
        [Fact]
        public void EvaluateSourceContamination_DistilledPotable_HasZeroContaminants()
        {
            var profile = WaterQualityProfileEngine.EvaluateSourceContamination(WaterSourcePurityTier.DistilledPotable);

            Assert.Equal(0, profile.ParticulatePpm);
            Assert.Equal(0, profile.HeavyMetalsPpm);
            Assert.Equal(0, profile.RadIsotopesBqL);
            Assert.Equal(0, profile.BioPathogensCfu);

            int risk = WaterQualityProfileEngine.CalculateHealthRiskPermille(profile);
            Assert.Equal(0, risk);
        }

        [Fact]
        public void EvaluateSourceContamination_RawSurfaceRunoff_WithFlood_IncreasesContaminants()
        {
            var normal = WaterQualityProfileEngine.EvaluateSourceContamination(
                WaterSourcePurityTier.RawSurfaceRunoff,
                floodContaminationPermille: 0);

            var flooded = WaterQualityProfileEngine.EvaluateSourceContamination(
                WaterSourcePurityTier.RawSurfaceRunoff,
                floodContaminationPermille: 500); // 50% flood increase

            Assert.True(flooded.ParticulatePpm > normal.ParticulatePpm);
            Assert.True(flooded.BioPathogensCfu > normal.BioPathogensCfu);
        }

        [Fact]
        public void CalculateTreatmentYield_ReverseOsmosis_HighFilterIntegrity_LowRisk()
        {
            var result = WaterQualityProfileEngine.CalculateTreatmentYield(
                WaterSourcePurityTier.SumpBrine,
                TreatmentMode.ReverseOsmosis,
                filterIntegrityPermille: 900);

            Assert.Equal(800, result.YieldFractionPermille);
            Assert.Equal(40, result.FilterWearPermille);
            Assert.Equal(WaterSourcePurityTier.ReverseOsmosis, result.ResultingPurityTier);
            Assert.Equal(10, result.PathogenRiskPermille);
            Assert.Equal(15, result.HeavyMetalRiskPermille);
        }

        [Fact]
        public void CalculateTreatmentYield_Distillation_ProducesPotableWithoutPathogens()
        {
            var result = WaterQualityProfileEngine.CalculateTreatmentYield(
                WaterSourcePurityTier.RawSurfaceRunoff,
                TreatmentMode.Distillation);

            Assert.Equal(700, result.YieldFractionPermille);
            Assert.Equal(WaterSourcePurityTier.DistilledPotable, result.ResultingPurityTier);
            Assert.Equal(0, result.PathogenRiskPermille);
        }
    }
}
