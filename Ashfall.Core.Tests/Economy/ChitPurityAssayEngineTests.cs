// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public class ChitPurityAssayEngineTests
    {
        [Fact]
        public void EvaluateAssay_MintPristine_AlwaysAcceptedWithZeroRisk()
        {
            var result = ChitPurityAssayEngine.EvaluateAssay(
                amount: 100,
                tier: PurityTier.MintPristine,
                merchantPerceptionPermille: 1000,
                deterministicSeed: 42
            );

            Assert.True(result.IsAccepted);
            Assert.False(result.IsCounterfeitDetected);
            Assert.Equal(100, result.AcceptedAmount);
            Assert.Equal(0, result.ConfiscatedAmount);
            Assert.Equal(0, result.PenaltyTrustPermille);
            Assert.Equal(0, result.GeneratedHeatPermille);
            Assert.Contains("Mint-pristine", result.StatusNotice);
        }

        [Fact]
        public void EvaluateAssay_CounterfeitDetected_ConfiscatesFundsAndIncrementsHeat()
        {
            // Seed 3 produces hash roll 89, well below effectiveDetectionOdds (approx 760)
            uint seed = 3;
            var result = ChitPurityAssayEngine.EvaluateAssay(
                amount: 50,
                tier: PurityTier.CounterfeitLead,
                merchantPerceptionPermille: 900,
                deterministicSeed: seed,
                transactionNonce: 1
            );

            Assert.False(result.IsAccepted);
            Assert.True(result.IsCounterfeitDetected);
            Assert.Equal(0, result.AcceptedAmount);
            Assert.Equal(50, result.ConfiscatedAmount);
            Assert.True(result.PenaltyTrustPermille > 0);
            Assert.True(result.GeneratedHeatPermille >= 50);
            Assert.Contains("Counterfeit lead currency detected", result.StatusNotice);
        }

        [Fact]
        public void EvaluateAssay_DilutedScrapDetected_DiscountsPayoutAndPenalizesTrust()
        {
            // Seed 1 produces hash roll 172, below diluted scrap detection odds (approx 360)
            uint seed = 1;
            var result = ChitPurityAssayEngine.EvaluateAssay(
                amount: 100,
                tier: PurityTier.DilutedScrap,
                merchantPerceptionPermille: 800,
                deterministicSeed: seed,
                transactionNonce: 3
            );

            Assert.True(result.IsAccepted);
            Assert.True(result.IsCounterfeitDetected);
            Assert.Equal(70, result.AcceptedAmount);
            Assert.Equal(30, result.ConfiscatedAmount);
            Assert.True(result.PenaltyTrustPermille > 0);
            Assert.Equal(15, result.GeneratedHeatPermille);
        }

        [Fact]
        public void EvaluateAssay_DeterministicReplay_SameSeedProducesIdenticalOutcome()
        {
            uint seed = 54321;
            var result1 = ChitPurityAssayEngine.EvaluateAssay(
                amount: 75,
                tier: PurityTier.CounterfeitLead,
                merchantPerceptionPermille: 500,
                deterministicSeed: seed,
                transactionNonce: 7
            );

            var result2 = ChitPurityAssayEngine.EvaluateAssay(
                amount: 75,
                tier: PurityTier.CounterfeitLead,
                merchantPerceptionPermille: 500,
                deterministicSeed: seed,
                transactionNonce: 7
            );

            Assert.Equal(result1.IsAccepted, result2.IsAccepted);
            Assert.Equal(result1.IsCounterfeitDetected, result2.IsCounterfeitDetected);
            Assert.Equal(result1.AcceptedAmount, result2.AcceptedAmount);
            Assert.Equal(result1.ConfiscatedAmount, result2.ConfiscatedAmount);
            Assert.Equal(result1.PenaltyTrustPermille, result2.PenaltyTrustPermille);
            Assert.Equal(result1.GeneratedHeatPermille, result2.GeneratedHeatPermille);
        }

        [Fact]
        public void CertifyDilutedScrap_ValidAmount_ProducesCorrectStandardAlloyYieldAndFee()
        {
            // 950 diluted chits (700 permille) -> (950 * 700) / 950 = 700 standard chits
            // Fee = 700 * 5% = 35 chits
            // Net = 700 - 35 = 665 chits
            var cert = ChitPurityAssayEngine.CertifyDilutedScrap(950);

            Assert.True(cert.Success);
            Assert.Equal(665, cert.CertifiedStandardAlloyChits);
            Assert.Equal(35, cert.CertificationFeeChits);
            Assert.Contains("certified 950 diluted scrap chits into 665 standard alloy", cert.Summary);
        }
    }
}
