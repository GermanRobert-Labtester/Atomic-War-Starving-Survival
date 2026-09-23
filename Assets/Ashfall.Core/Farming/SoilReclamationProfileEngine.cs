using System;

namespace Ashfall.Core.Farming
{
    /// <summary>
    /// Classification of reclaimed wasteland soil readiness.
    /// </summary>
    public enum SoilQualityTier
    {
        BarrenAsh = 0,       // Severe radionuclide load and zero organic humus
        AcidicSaline = 1,    // High chemical salt and extreme pH, low germination
        RemediatedLoam = 2,  // Functional reclaimed soil, acceptable yields
        EnrichedTopsoil = 3  // Prime living humus, enhanced vitality and crop yields
    }

    /// <summary>
    /// Soil reclamation treatment types applicable during cultivation cycles.
    /// </summary>
    public enum SoilAmendment
    {
        None = 0,
        BiocharAdsorbent = 1, // Binds radionuclides and heavy metal toxins
        CompostMulch = 2,     // Adds organic matter and buffers extreme pH
        LeachingFlush = 3,    // Flushes soluble salts using treated water
        FallowRotation = 4    // Rest period allowing microbial restoration
    }

    /// <summary>
    /// Immutable soil evaluation result produced by <see cref="SoilReclamationProfileEngine"/>.
    /// </summary>
    public readonly struct SoilEvaluationResult
    {
        public SoilQualityTier QualityTier { get; }
        public int GerminationViabilityPermille { get; }
        public int CropMutationRiskPermille { get; }
        public int YieldMultiplierPermille { get; }
        public int NetSalinityPermille { get; }
        public int NetRadionuclideLoadPermille { get; }
        public int NetOrganicMatterPermille { get; }
        public int NetPhTenths { get; }
        public bool IsCultivable { get; }

        public SoilEvaluationResult(
            SoilQualityTier qualityTier,
            int germinationViabilityPermille,
            int cropMutationRiskPermille,
            int yieldMultiplierPermille,
            int netSalinityPermille,
            int netRadionuclideLoadPermille,
            int netOrganicMatterPermille,
            int netPhTenths,
            bool isCultivable)
        {
            QualityTier = qualityTier;
            GerminationViabilityPermille = germinationViabilityPermille;
            CropMutationRiskPermille = cropMutationRiskPermille;
            YieldMultiplierPermille = yieldMultiplierPermille;
            NetSalinityPermille = netSalinityPermille;
            NetRadionuclideLoadPermille = netRadionuclideLoadPermille;
            NetOrganicMatterPermille = netOrganicMatterPermille;
            NetPhTenths = netPhTenths;
            IsCultivable = isCultivable;
        }
    }

    /// <summary>
    /// Pure domain engine for open-ground soil reclamation, amendment chemistry,
    /// and agricultural fertility evaluation (Expansion 15: The Deep Root).
    /// Operates without engine dependencies or duplicate farming minigames.
    /// </summary>
    public static class SoilReclamationProfileEngine
    {
        public const int PermilleScale = 1000;
        public const int NeutralPhTenths = 68; // 6.8 pH neutral-optimal loam

        /// <summary>
        /// Evaluates soil fertility, germination viability, and mutation risk after applying agricultural amendments.
        /// </summary>
        /// <param name="initialSalinityPermille">Initial dissolved salt concentration (0..1000).</param>
        /// <param name="initialRadionuclideLoadPermille">Initial isotopic contamination level (0..1000).</param>
        /// <param name="initialOrganicMatterPermille">Initial biological humus concentration (0..1000).</param>
        /// <param name="initialPhTenths">Initial pH in tenths (e.g., 45 = 4.5 acidic, 85 = 8.5 alkaline).</param>
        /// <param name="amendment">Active remediation amendment applied this cycle.</param>
        /// <param name="amendmentQuantityUnits">Quantity of amendment material applied (0..10).</param>
        /// <returns>Immutable <see cref="SoilEvaluationResult"/>.</returns>
        public static SoilEvaluationResult Evaluate(
            int initialSalinityPermille,
            int initialRadionuclideLoadPermille,
            int initialOrganicMatterPermille,
            int initialPhTenths,
            SoilAmendment amendment,
            int amendmentQuantityUnits = 1)
        {
            initialSalinityPermille = Math.Max(0, Math.Min(PermilleScale, initialSalinityPermille));
            initialRadionuclideLoadPermille = Math.Max(0, Math.Min(PermilleScale, initialRadionuclideLoadPermille));
            initialOrganicMatterPermille = Math.Max(0, Math.Min(PermilleScale, initialOrganicMatterPermille));
            initialPhTenths = Math.Max(30, Math.Min(110, initialPhTenths));
            amendmentQuantityUnits = Math.Max(0, Math.Min(10, amendmentQuantityUnits));

            int salinity = initialSalinityPermille;
            int radLoad = initialRadionuclideLoadPermille;
            int organic = initialOrganicMatterPermille;
            int ph = initialPhTenths;

            // Apply amendment effects
            switch (amendment)
            {
                case SoilAmendment.BiocharAdsorbent:
                    // Biochar strongly adsorbs heavy isotopes: -80 permille per unit
                    radLoad = Math.Max(0, radLoad - (amendmentQuantityUnits * 80));
                    // Minor organic carbon increase
                    organic = Math.Min(PermilleScale, organic + (amendmentQuantityUnits * 15));
                    // Buffers towards neutral
                    if (ph < NeutralPhTenths) ph = Math.Min(NeutralPhTenths, ph + (amendmentQuantityUnits * 2));
                    break;

                case SoilAmendment.CompostMulch:
                    // Compost boosts organic humus significantly: +100 permille per unit
                    organic = Math.Min(PermilleScale, organic + (amendmentQuantityUnits * 100));
                    // Neutralizes acidity/alkalinity
                    if (ph < NeutralPhTenths) ph = Math.Min(NeutralPhTenths, ph + (amendmentQuantityUnits * 4));
                    else if (ph > NeutralPhTenths) ph = Math.Max(NeutralPhTenths, ph - (amendmentQuantityUnits * 4));
                    break;

                case SoilAmendment.LeachingFlush:
                    // Leaching flushes soluble salts: -120 permille per unit
                    salinity = Math.Max(0, salinity - (amendmentQuantityUnits * 120));
                    // Flushes slight organic matter as runoff
                    organic = Math.Max(0, organic - (amendmentQuantityUnits * 10));
                    break;

                case SoilAmendment.FallowRotation:
                    // Fallow slowly stabilizes soil over time
                    organic = Math.Min(PermilleScale, organic + 30);
                    radLoad = Math.Max(0, radLoad - 20);
                    salinity = Math.Max(0, salinity - 15);
                    break;

                case SoilAmendment.None:
                default:
                    break;
            }

            // Calculate Germination Viability Permille
            // Salt and extreme pH drastically suppress germination
            int phDeviation = Math.Abs(ph - NeutralPhTenths);
            int phPenalty = phDeviation * 25; // each 0.1 pH deviation from 6.8 loses 25 permille
            int saltPenalty = (salinity * 700) / PermilleScale; // up to 70% loss from salt

            int germination = PermilleScale - phPenalty - saltPenalty;
            // Humus provides germination resilience
            germination += (organic * 200) / PermilleScale;
            germination = Math.Max(0, Math.Min(PermilleScale, germination));

            // Calculate Crop Mutation Risk Permille
            // Radionuclide load is primary driver of genetic breakdown
            int mutationRisk = (radLoad * 850) / PermilleScale;
            // High organic humus provides minor antioxidant/soil flora buffering against uptake
            mutationRisk -= (organic * 150) / PermilleScale;
            mutationRisk = Math.Max(0, Math.Min(PermilleScale, mutationRisk));

            // Determine Quality Tier
            SoilQualityTier tier;
            if (radLoad > 600 || organic < 100 || germination < 200)
            {
                tier = SoilQualityTier.BarrenAsh;
            }
            else if (salinity > 450 || phDeviation > 15 || organic < 300)
            {
                tier = SoilQualityTier.AcidicSaline;
            }
            else if (organic < 650 || radLoad > 150 || salinity > 200)
            {
                tier = SoilQualityTier.RemediatedLoam;
            }
            else
            {
                tier = SoilQualityTier.EnrichedTopsoil;
            }

            // Yield Multiplier Permille
            int yieldMult;
            switch (tier)
            {
                case SoilQualityTier.EnrichedTopsoil:
                    yieldMult = 1200 + ((organic - 650) * 400) / 350; // 120%..160%
                    break;
                case SoilQualityTier.RemediatedLoam:
                    yieldMult = 800 + ((organic - 300) * 400) / 350;  // 80%..120%
                    break;
                case SoilQualityTier.AcidicSaline:
                    yieldMult = 300 + (germination * 300) / PermilleScale; // 30%..60%
                    break;
                case SoilQualityTier.BarrenAsh:
                default:
                    yieldMult = (germination * 200) / PermilleScale; // 0%..20%
                    break;
            }

            bool cultivable = (tier != SoilQualityTier.BarrenAsh) && (germination >= 250);

            return new SoilEvaluationResult(
                qualityTier: tier,
                germinationViabilityPermille: germination,
                cropMutationRiskPermille: mutationRisk,
                yieldMultiplierPermille: yieldMult,
                netSalinityPermille: salinity,
                netRadionuclideLoadPermille: radLoad,
                netOrganicMatterPermille: organic,
                netPhTenths: ph,
                isCultivable: cultivable
            );
        }
    }
}
