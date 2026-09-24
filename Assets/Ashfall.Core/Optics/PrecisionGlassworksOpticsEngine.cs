// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 29 — The Glass
// Subsystem    : Precision Vitrification, Optics Grinding & Vision Care Engine
// Authority    : docs/expansions/wave4/expansion_29_the_glass_plan.md
//                UNBLOCK-05_EXPANSION_WAVES_C3_EN_GATE.md §3.1, §5.3
// ============================================================================
using System;

namespace Ashfall.Core.Optics
{
    /// <summary>
    /// Vitrification purity tier of a produced glass batch.
    /// Higher purity enables finer optical work.
    /// </summary>
    public enum GlassPurityTier
    {
        Crude         = 0,  // bubbles, inclusions — only structural use
        CommonWindow  = 1,  // clear enough for windowpanes
        OpticalGrade  = 2,  // low distortion — suitable for magnifying lenses
        PrecisionOptic = 3  // near-flawless — theodolite lenses, corrective spectacles
    }

    /// <summary>
    /// Corrective vision prescription band for a survivor.
    /// </summary>
    public enum VisionCorrectionBand
    {
        NoCorrectionNeeded = 0,
        MildMyopia         = 1,  // -0.25 to -1.50 dioptre equivalent
        ModerateMyopia     = 2,  // -1.75 to -4.00
        SevereMyopia       = 3,  // > -4.00 (debilitating without correction)
        Hyperopia          = 4,  // far-sighted; needs convex lens
        Presbyopia         = 5   // age-related; reading lens required
    }

    /// <summary>
    /// Mutable state record for a silica glass batch in the vitrification kiln.
    /// </summary>
    public sealed class GlassBatchState
    {
        public string BatchId                  { get; set; } = string.Empty;
        /// <summary>Silica purity of raw batch (0..1000 permille; 1000 = pure quartz sand).</summary>
        public int SilicaPurityPermille        { get; set; } = 700;
        /// <summary>Current annealing stage (0 = raw melt, 3 = fully annealed).</summary>
        public int AnnealingStage              { get; set; } = 0;
        /// <summary>Thermal shock risk accumulated (0..1000 permille; ≥700 = crack risk).</summary>
        public int ThermalShockRiskPermille    { get; set; } = 0;
        /// <summary>Whether the batch cracked during annealing.</summary>
        public bool IsCracked                  { get; set; } = false;
        /// <summary>Resulting purity tier once annealing is complete.</summary>
        public GlassPurityTier ResultingTier   { get; set; } = GlassPurityTier.Crude;

        public GlassBatchState Clone() => new GlassBatchState
        {
            BatchId              = BatchId,
            SilicaPurityPermille = SilicaPurityPermille,
            AnnealingStage       = AnnealingStage,
            ThermalShockRiskPermille = ThermalShockRiskPermille,
            IsCracked            = IsCracked,
            ResultingTier        = ResultingTier
        };
    }

    /// <summary>
    /// Immutable result of a lens grinding session.
    /// </summary>
    public readonly struct LensGrindingResult
    {
        /// <summary>Optical precision achieved (0..1000 permille).</summary>
        public int OpticalPrecisionPermille    { get; }
        /// <summary>True if the lens meets the target prescription within tolerance.</summary>
        public bool MeetsPrescriptionTolerance { get; }
        /// <summary>Abrasive grit consumed permille (0..1000 of available stock).</summary>
        public int GritConsumedPermille        { get; }
        /// <summary>Whether the lens cracked under the grinding pressure.</summary>
        public bool LensCracked                { get; }

        public LensGrindingResult(
            int opticalPrecisionPermille,
            bool meetsPrescriptionTolerance,
            int gritConsumedPermille,
            bool lensCracked)
        {
            OpticalPrecisionPermille   = Math.Clamp(opticalPrecisionPermille, 0, 1000);
            MeetsPrescriptionTolerance = meetsPrescriptionTolerance;
            GritConsumedPermille       = Math.Clamp(gritConsumedPermille, 0, 1000);
            LensCracked                = lensCracked;
        }
    }

    /// <summary>
    /// Immutable result of a theodolite calibration pass.
    /// </summary>
    public readonly struct TheodoliteCalibrationResult
    {
        /// <summary>Angular calibration accuracy (0..1000 permille; 1000 = arcminute precision).</summary>
        public int CalibrationAccuracyPermille { get; }
        /// <summary>Survey range improvement in wasteland-metres.</summary>
        public int SurveyRangeMetres           { get; }
        /// <summary>True if the optic grade is high enough for precision surveying.</summary>
        public bool IsFieldReady               { get; }

        public TheodoliteCalibrationResult(
            int calibrationAccuracyPermille,
            int surveyRangeMetres,
            bool isFieldReady)
        {
            CalibrationAccuracyPermille = Math.Clamp(calibrationAccuracyPermille, 0, 1000);
            SurveyRangeMetres           = Math.Max(0, surveyRangeMetres);
            IsFieldReady                = isFieldReady;
        }
    }

    /// <summary>
    /// Pure domain engine governing silica batch vitrification, thermal annealing,
    /// lens curvature grinding, corrective vision prescriptions, and theodolite calibration.
    /// Extends the PrecisionOpticsEngine seam without duplicating optical catalog authority.
    /// Zero engine references, integer permille determinism.
    /// </summary>
    public static class PrecisionGlassworksOpticsEngine
    {
        /// <summary>Thermal shock risk permille at which cracking becomes likely.</summary>
        public const int CrackRiskThreshold = 700;

        /// <summary>Annealing stages required to fully temper a batch (3 stages).</summary>
        public const int MaxAnnealingStages = 3;

        /// <summary>
        /// Advances one annealing stage for a silica glass batch.
        /// Mutates AnnealingStage, ThermalShockRiskPermille, IsCracked, and ResultingTier.
        /// </summary>
        /// <param name="batch">Batch state to mutate.</param>
        /// <param name="kilnTemperaturePermille">
        ///     Kiln temperature precision (0..1000; 1000 = optimal 560°C anneal).
        ///     Temperatures below 700 permille introduce thermal gradients.
        /// </param>
        public static void AdvanceAnnealingStage(GlassBatchState batch, int kilnTemperaturePermille)
        {
            if (batch == null) throw new ArgumentNullException(nameof(batch));
            if (batch.IsCracked) return;
            if (batch.AnnealingStage >= MaxAnnealingStages) return;

            kilnTemperaturePermille = Math.Clamp(kilnTemperaturePermille, 0, 1000);

            // Thermal shock risk increases when kiln temperature deviates from optimal
            int deviation = Math.Abs(kilnTemperaturePermille - 900);
            int shockIncrease = (deviation * 300) / 1000;
            batch.ThermalShockRiskPermille = Math.Min(1000,
                batch.ThermalShockRiskPermille + shockIncrease);

            if (batch.ThermalShockRiskPermille >= CrackRiskThreshold)
            {
                batch.IsCracked = true;
                return;
            }

            batch.AnnealingStage++;

            // Determine resulting tier once fully annealed
            if (batch.AnnealingStage >= MaxAnnealingStages)
            {
                batch.ResultingTier = batch.SilicaPurityPermille switch
                {
                    >= 950 => GlassPurityTier.PrecisionOptic,
                    >= 800 => GlassPurityTier.OpticalGrade,
                    >= 600 => GlassPurityTier.CommonWindow,
                    _      => GlassPurityTier.Crude
                };
            }
        }

        /// <summary>
        /// Grinds a lens blank to a target curvature for a corrective prescription.
        /// </summary>
        /// <param name="glassPurity">Purity tier of the glass blank being ground.</param>
        /// <param name="targetBand">Target vision correction the lens must satisfy.</param>
        /// <param name="grinderSkillPermille">Grinder artisan skill (0..1000).</param>
        /// <param name="abrasiveGritAvailablePermille">Available abrasive stock (0..1000).</param>
        /// <param name="grindSeed">Deterministic seed for outcome variance (combine with <see cref="StableHash"/>).</param>
        public static LensGrindingResult GrindCorrectionLens(
            GlassPurityTier glassPurity,
            VisionCorrectionBand targetBand,
            int grinderSkillPermille,
            int abrasiveGritAvailablePermille,
            int grindSeed)
        {
            grinderSkillPermille          = Math.Clamp(grinderSkillPermille, 0, 1000);
            abrasiveGritAvailablePermille = Math.Clamp(abrasiveGritAvailablePermille, 0, 1000);

            // Crude glass cannot hold optical precision
            if (glassPurity == GlassPurityTier.Crude)
            {
                return new LensGrindingResult(80, false, (abrasiveGritAvailablePermille * 200) / 1000, false);
            }

            // Base optical precision from glass tier and grinder skill
            int glassTierBonus = glassPurity switch
            {
                GlassPurityTier.CommonWindow  => 300,
                GlassPurityTier.OpticalGrade  => 600,
                GlassPurityTier.PrecisionOptic => 900,
                _                              => 100
            };
            int baseQuality = (glassTierBonus + grinderSkillPermille) / 2;

            // Deterministic variance ±10%
            int hash = StableHash.Combine(grindSeed, (int)glassPurity);
            hash = StableHash.Combine(hash, (int)targetBand);
            int variance = ((hash & 0x7FFFFFFF) % 21) - 10; // -10..+10
            baseQuality = Math.Clamp(baseQuality + (baseQuality * variance) / 100, 0, 1000);

            // Prescription difficulty modifier
            int prescriptionDifficulty = targetBand switch
            {
                VisionCorrectionBand.NoCorrectionNeeded => 0,
                VisionCorrectionBand.MildMyopia          => 100,
                VisionCorrectionBand.Hyperopia            => 120,
                VisionCorrectionBand.Presbyopia           => 150,
                VisionCorrectionBand.ModerateMyopia       => 200,
                VisionCorrectionBand.SevereMyopia         => 350,
                _                                         => 150
            };

            // Grit consumed proportional to difficulty
            int gritConsumed = Math.Min(abrasiveGritAvailablePermille,
                (prescriptionDifficulty * 3 + 100));
            gritConsumed = Math.Clamp(gritConsumed, 0, 1000);

            // Insufficient grit degrades quality
            if (abrasiveGritAvailablePermille < gritConsumed)
            {
                baseQuality = (baseQuality * abrasiveGritAvailablePermille) / Math.Max(1, gritConsumed);
            }

            // Lens crack risk for severe prescriptions on lower-grade glass
            bool lensCracked = false;
            if (targetBand == VisionCorrectionBand.SevereMyopia &&
                glassPurity <= GlassPurityTier.CommonWindow &&
                grinderSkillPermille < 400)
            {
                lensCracked = true;
                return new LensGrindingResult(0, false, gritConsumed, true);
            }

            // Tolerance check: meets prescription if quality ≥ band threshold
            int toleranceThreshold = prescriptionDifficulty + 400;
            bool meetsTolerance = baseQuality >= toleranceThreshold;

            return new LensGrindingResult(baseQuality, meetsTolerance, gritConsumed, lensCracked);
        }

        /// <summary>
        /// Calibrates a theodolite optical assembly for wasteland surveying.
        /// </summary>
        /// <param name="lensPurity">Purity of the fitted objective lens.</param>
        /// <param name="calibratorSkillPermille">Calibrator skill (0..1000).</param>
        public static TheodoliteCalibrationResult CalibrateTheodolite(
            GlassPurityTier lensPurity,
            int calibratorSkillPermille)
        {
            calibratorSkillPermille = Math.Clamp(calibratorSkillPermille, 0, 1000);

            if (lensPurity < GlassPurityTier.OpticalGrade)
            {
                // Cannot achieve useful calibration with crude or common glass
                return new TheodoliteCalibrationResult(150, 500, false);
            }

            int purityBonus = lensPurity switch
            {
                GlassPurityTier.OpticalGrade   => 500,
                GlassPurityTier.PrecisionOptic  => 800,
                _                               => 200
            };

            int calibrationAccuracy = (purityBonus + calibratorSkillPermille) / 2;
            calibrationAccuracy = Math.Clamp(calibrationAccuracy, 0, 1000);

            // Survey range scales with calibration quality (max 20 km equivalent)
            int surveyRange = (calibrationAccuracy * 20000) / 1000;
            bool isFieldReady = calibrationAccuracy >= 600;

            return new TheodoliteCalibrationResult(calibrationAccuracy, surveyRange, isFieldReady);
        }

        /// <summary>
        /// Returns the minimum glass purity tier required for a given vision correction.
        /// </summary>
        public static GlassPurityTier MinimumPurityForVision(VisionCorrectionBand band) =>
            band switch
            {
                VisionCorrectionBand.NoCorrectionNeeded => GlassPurityTier.Crude,
                VisionCorrectionBand.MildMyopia          => GlassPurityTier.CommonWindow,
                VisionCorrectionBand.Hyperopia            => GlassPurityTier.CommonWindow,
                VisionCorrectionBand.Presbyopia           => GlassPurityTier.OpticalGrade,
                VisionCorrectionBand.ModerateMyopia       => GlassPurityTier.OpticalGrade,
                VisionCorrectionBand.SevereMyopia         => GlassPurityTier.PrecisionOptic,
                _                                         => GlassPurityTier.OpticalGrade
            };
    }
}
