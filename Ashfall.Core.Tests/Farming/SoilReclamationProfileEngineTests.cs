using Ashfall.Core.Farming;
using Xunit;

namespace Ashfall.Core.Tests.Farming
{
    public class SoilReclamationProfileEngineTests
    {
        [Fact]
        public void Evaluate_BarrenAsh_ProducesLowGerminationAndHighMutation()
        {
            var result = SoilReclamationProfileEngine.Evaluate(
                initialSalinityPermille: 300,
                initialRadionuclideLoadPermille: 800,
                initialOrganicMatterPermille: 50,
                initialPhTenths: 45, // Acidic ash
                amendment: SoilAmendment.None
            );

            Assert.Equal(SoilQualityTier.BarrenAsh, result.QualityTier);
            Assert.False(result.IsCultivable);
            Assert.True(result.CropMutationRiskPermille > 650);
            Assert.True(result.GerminationViabilityPermille < 250);
        }

        [Fact]
        public void Evaluate_BiocharAdsorption_ReducesRadionuclideLoadAndMutation()
        {
            var untreated = SoilReclamationProfileEngine.Evaluate(
                initialSalinityPermille: 200,
                initialRadionuclideLoadPermille: 500,
                initialOrganicMatterPermille: 300,
                initialPhTenths: 60,
                amendment: SoilAmendment.None
            );

            var treated = SoilReclamationProfileEngine.Evaluate(
                initialSalinityPermille: 200,
                initialRadionuclideLoadPermille: 500,
                initialOrganicMatterPermille: 300,
                initialPhTenths: 60,
                amendment: SoilAmendment.BiocharAdsorbent,
                amendmentQuantityUnits: 4 // 4 * 80 = 320 reduction
            );

            Assert.Equal(500, untreated.NetRadionuclideLoadPermille);
            Assert.Equal(180, treated.NetRadionuclideLoadPermille); // 500 - 320
            Assert.True(treated.CropMutationRiskPermille < untreated.CropMutationRiskPermille);
        }

        [Fact]
        public void Evaluate_CompostMulch_BuffersPhAndElevatesYield()
        {
            var result = SoilReclamationProfileEngine.Evaluate(
                initialSalinityPermille: 100,
                initialRadionuclideLoadPermille: 100,
                initialOrganicMatterPermille: 400,
                initialPhTenths: 50, // Acidic
                amendment: SoilAmendment.CompostMulch,
                amendmentQuantityUnits: 5 // 5 * 100 organic, 5 * 4 = +20 pH tenths
            );

            Assert.Equal(68, result.NetPhTenths); // buffered and clamped to 68 (6.8 pH neutral)
            Assert.Equal(900, result.NetOrganicMatterPermille); // 400 + 500
            Assert.Equal(SoilQualityTier.EnrichedTopsoil, result.QualityTier);
            Assert.True(result.YieldMultiplierPermille >= 1200);
            Assert.True(result.IsCultivable);
        }

        [Fact]
        public void Evaluate_LeachingFlush_FlushesSolubleSalts()
        {
            var result = SoilReclamationProfileEngine.Evaluate(
                initialSalinityPermille: 600,
                initialRadionuclideLoadPermille: 100,
                initialOrganicMatterPermille: 350,
                initialPhTenths: 68,
                amendment: SoilAmendment.LeachingFlush,
                amendmentQuantityUnits: 4 // 4 * 120 = 480 reduction
            );

            Assert.Equal(120, result.NetSalinityPermille); // 600 - 480
            Assert.True(result.GerminationViabilityPermille > 700);
        }

        [Fact]
        public void Evaluate_FallowRotation_ProvidesGradualStabilization()
        {
            var result = SoilReclamationProfileEngine.Evaluate(
                initialSalinityPermille: 200,
                initialRadionuclideLoadPermille: 150,
                initialOrganicMatterPermille: 300,
                initialPhTenths: 68,
                amendment: SoilAmendment.FallowRotation
            );

            Assert.Equal(330, result.NetOrganicMatterPermille);
            Assert.Equal(130, result.NetRadionuclideLoadPermille);
            Assert.Equal(185, result.NetSalinityPermille);
        }
    }
}
