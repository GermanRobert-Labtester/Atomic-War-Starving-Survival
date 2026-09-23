// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Economy
{
    public enum PurityTier
    {
        MintPristine = 1,
        StandardAlloy = 2,
        DilutedScrap = 3,
        CounterfeitLead = 4
    }

    public sealed class ChitAssayResult
    {
        public bool IsAccepted { get; }
        public bool IsCounterfeitDetected { get; }
        public int AcceptedAmount { get; }
        public int ConfiscatedAmount { get; }
        public int PenaltyTrustPermille { get; }
        public int GeneratedHeatPermille { get; }
        public string StatusNotice { get; }

        public ChitAssayResult(
            bool isAccepted,
            bool isCounterfeitDetected,
            int acceptedAmount,
            int confiscatedAmount,
            int penaltyTrustPermille,
            int generatedHeatPermille,
            string statusNotice)
        {
            IsAccepted = isAccepted;
            IsCounterfeitDetected = isCounterfeitDetected;
            AcceptedAmount = Math.Max(0, acceptedAmount);
            ConfiscatedAmount = Math.Max(0, confiscatedAmount);
            PenaltyTrustPermille = Math.Clamp(penaltyTrustPermille, 0, 1000);
            GeneratedHeatPermille = Math.Clamp(generatedHeatPermille, 0, 1000);
            StatusNotice = statusNotice ?? string.Empty;
        }
    }

    public sealed class ChitCertificationResult
    {
        public bool Success { get; }
        public int CertifiedStandardAlloyChits { get; }
        public int CertificationFeeChits { get; }
        public string Summary { get; }

        public ChitCertificationResult(
            bool success,
            int certifiedStandardAlloyChits,
            int certificationFeeChits,
            string summary)
        {
            Success = success;
            CertifiedStandardAlloyChits = Math.Max(0, certifiedStandardAlloyChits);
            CertificationFeeChits = Math.Max(0, certificationFeeChits);
            Summary = summary ?? string.Empty;
        }
    }

    /// <summary>
    /// XP-04-F5 / UNBLOCK-02 §5.5: Scrap Chit Purity Tiers & Counterfeit Detection Engine.
    /// Models currency alloy purity, merchant assay detection odds, trust penalties,
    /// heat escalation upon counterfeit discovery, and assay office certification.
    /// Engine-free, integer/permille determinism with seeded hash.
    /// </summary>
    public static class ChitPurityAssayEngine
    {
        public const int StandardAlloyNominalValuePermille = 950;
        public const int DilutedScrapNominalValuePermille = 700;
        public const int CounterfeitLeadNominalValuePermille = 150;
        public const int CertificationFeePermille = 50; // 5% fee at assay office

        public static (int nominalValuePermille, int baseDetectionOddsPermille) GetTierParameters(PurityTier tier)
        {
            return tier switch
            {
                PurityTier.MintPristine => (1000, 0),
                PurityTier.StandardAlloy => (StandardAlloyNominalValuePermille, 50),
                PurityTier.DilutedScrap => (DilutedScrapNominalValuePermille, 400),
                PurityTier.CounterfeitLead => (CounterfeitLeadNominalValuePermille, 800),
                _ => (StandardAlloyNominalValuePermille, 50)
            };
        }

        public static ChitAssayResult EvaluateAssay(
            int amount,
            PurityTier tier,
            int merchantPerceptionPermille,
            uint deterministicSeed,
            int transactionNonce = 1)
        {
            if (amount <= 0)
            {
                return new ChitAssayResult(
                    isAccepted: false,
                    isCounterfeitDetected: false,
                    acceptedAmount: 0,
                    confiscatedAmount: 0,
                    penaltyTrustPermille: 0,
                    generatedHeatPermille: 0,
                    statusNotice: "Invalid currency amount."
                );
            }

            merchantPerceptionPermille = Math.Clamp(merchantPerceptionPermille, 0, 1000);

            if (tier == PurityTier.MintPristine)
            {
                return new ChitAssayResult(
                    isAccepted: true,
                    isCounterfeitDetected: false,
                    acceptedAmount: amount,
                    confiscatedAmount: 0,
                    penaltyTrustPermille: 0,
                    generatedHeatPermille: 0,
                    statusNotice: "Mint-pristine currency accepted with zero challenge."
                );
            }

            var (nominalValue, baseDetectionOdds) = GetTierParameters(tier);

            // Effective detection probability scales with merchant perception:
            // detectionOdds = (baseDetectionOdds * (500 + merchantPerceptionPermille / 2)) / 1000
            int effectiveDetectionOdds = (baseDetectionOdds * (500 + merchantPerceptionPermille / 2)) / 1000;
            effectiveDetectionOdds = Math.Clamp(effectiveDetectionOdds, 0, 950);

            // Deterministic roll using 32-bit Murmur-style hash
            uint roll = HashSeed(deterministicSeed, (uint)amount, (uint)transactionNonce) % 1000;

            if (roll < (uint)effectiveDetectionOdds)
            {
                // Detected as impure or counterfeit!
                if (tier == PurityTier.CounterfeitLead)
                {
                    int trustPenalty = Math.Min(300, (merchantPerceptionPermille * 250) / 1000 + 50);
                    int heatIncrease = Math.Min(200, 50 + (merchantPerceptionPermille * 100) / 1000);

                    return new ChitAssayResult(
                        isAccepted: false,
                        isCounterfeitDetected: true,
                        acceptedAmount: 0,
                        confiscatedAmount: amount,
                        penaltyTrustPermille: trustPenalty,
                        generatedHeatPermille: heatIncrease,
                        statusNotice: "Counterfeit lead currency detected during assay; transaction refused, funds confiscated, and merchant alerted."
                    );
                }
                else if (tier == PurityTier.DilutedScrap)
                {
                    // Merchant discounts payment to actual metal value and levies mild trust penalty
                    int discountedAmount = (amount * nominalValue) / 1000;
                    int trustPenalty = (merchantPerceptionPermille * 80) / 1000;
                    int heatIncrease = 15;

                    return new ChitAssayResult(
                        isAccepted: true,
                        isCounterfeitDetected: true,
                        acceptedAmount: discountedAmount,
                        confiscatedAmount: amount - discountedAmount,
                        penaltyTrustPermille: trustPenalty,
                        generatedHeatPermille: heatIncrease,
                        statusNotice: $"Diluted scrap alloy detected; merchant discounted payout to actual metal content ({discountedAmount} chits)."
                    );
                }
                else
                {
                    // Standard alloy minor discount if slight impurities caught
                    return new ChitAssayResult(
                        isAccepted: true,
                        isCounterfeitDetected: false,
                        acceptedAmount: amount,
                        confiscatedAmount: 0,
                        penaltyTrustPermille: 0,
                        generatedHeatPermille: 0,
                        statusNotice: "Standard alloy verified with minor surface oxidation; accepted at face value."
                    );
                }
            }

            // Assay passed without incident
            return new ChitAssayResult(
                isAccepted: true,
                isCounterfeitDetected: false,
                acceptedAmount: amount,
                confiscatedAmount: 0,
                penaltyTrustPermille: 0,
                generatedHeatPermille: 0,
                statusNotice: "Currency accepted without challenge."
            );
        }

        public static ChitCertificationResult CertifyDilutedScrap(int dilutedAmount)
        {
            if (dilutedAmount <= 0)
            {
                return new ChitCertificationResult(false, 0, 0, "Invalid amount to certify.");
            }

            // Converts diluted scrap (700 permille) into standard alloy (950 permille) minus fee
            // netStandardValue = (dilutedAmount * 700) / 950
            int rawStandardChits = (dilutedAmount * DilutedScrapNominalValuePermille) / StandardAlloyNominalValuePermille;
            int fee = Math.Max(1, (rawStandardChits * CertificationFeePermille) / 1000);
            int netCertified = Math.Max(1, rawStandardChits - fee);

            return new ChitCertificationResult(
                success: true,
                certifiedStandardAlloyChits: netCertified,
                certificationFeeChits: fee,
                summary: $"Smelted and certified {dilutedAmount} diluted scrap chits into {netCertified} standard alloy chits (Fee: {fee} chits)."
            );
        }

        private static uint HashSeed(uint seed, uint val1, uint val2)
        {
            uint h = seed ^ (val1 * 0x85ebca6b) ^ (val2 * 0xc2b2ae35);
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;
            return h;
        }
    }
}
