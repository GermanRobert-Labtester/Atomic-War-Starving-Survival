// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// F14-E / UNBLOCK-01: Deterministic rehabilitation arc progression engine.
    /// Advances survivor prosthetic rehabilitation through the three normative phases:
    /// fitting (3-5 days, 500 permille quality) ->
    /// adaptation (10-20 days, ramps 500 -> 1000 permille scaled by resilience) ->
    /// mastery (permanent, 1000 permille quality + bonus).
    /// Pure domain engine; zero RNG; integer permille math.
    /// </summary>
    public static class RehabilitationProgressionEngine
    {
        public const int DefaultFittingDurationDays = 4;
        public const int DefaultAdaptationDurationDays = 14;
        public const int FittingQualityPermille = 500;
        public const int MasteryQualityPermille = 1000;

        /// <summary>
        /// Creates an initial RehabRecord when a prosthetic is first fitted.
        /// </summary>
        public static RehabRecord StartRehabilitation(string prostheticTypeKey)
        {
            return new RehabRecord(
                prostheticTypeKey ?? "prosthetic",
                phase: "fitting",
                daysInPhase: 0,
                qualityRampPermille: FittingQualityPermille);
        }

        /// <summary>
        /// Advances the rehabilitation state by the specified number of days,
        /// scaling adaptation rate by the survivor's resilience multiplier.
        /// </summary>
        public static RehabRecord AdvanceDaily(
            RehabRecord? current,
            float resilienceMultiplier = 1.0f,
            int days = 1,
            int fittingDurationDays = DefaultFittingDurationDays,
            int adaptationDurationDays = DefaultAdaptationDurationDays)
        {
            if (current == null)
            {
                return StartRehabilitation("prosthetic");
            }

            if (days <= 0)
            {
                return current;
            }

            int fittingDays = Math.Max(1, fittingDurationDays);
            int adaptDays = Math.Max(1, adaptationDurationDays);
            float resilience = Math.Clamp(resilienceMultiplier, 0.5f, 2.0f);

            string phase = current.Phase?.ToLowerInvariant() ?? "fitting";
            int daysInPhase = current.DaysInPhase;
            int quality = Math.Clamp(current.QualityRampPermille, FittingQualityPermille, MasteryQualityPermille);

            for (int d = 0; d < days; d++)
            {
                if (string.Equals(phase, "fitting", StringComparison.OrdinalIgnoreCase))
                {
                    daysInPhase++;
                    quality = FittingQualityPermille;
                    if (daysInPhase >= fittingDays)
                    {
                        phase = "adaptation";
                        daysInPhase = 0;
                    }
                }
                else if (string.Equals(phase, "adaptation", StringComparison.OrdinalIgnoreCase))
                {
                    daysInPhase++;
                    // Base daily ramp is 500 permille spread across adaptDays
                    int baseRamp = (MasteryQualityPermille - FittingQualityPermille) / adaptDays;
                    int scaledRamp = (int)Math.Round(baseRamp * resilience);
                    if (scaledRamp <= 0) scaledRamp = 1;

                    quality = Math.Min(MasteryQualityPermille, quality + scaledRamp);

                    if (daysInPhase >= adaptDays || quality >= MasteryQualityPermille)
                    {
                        phase = "mastery";
                        daysInPhase = 0;
                        quality = MasteryQualityPermille;
                    }
                }
                else // mastery
                {
                    phase = "mastery";
                    daysInPhase++;
                    quality = MasteryQualityPermille;
                }
            }

            return new RehabRecord(current.ProstheticTypeKey, phase, daysInPhase, quality);
        }

        /// <summary>
        /// Returns the effective quality factor (0.50 to 1.00) from quality permille.
        /// </summary>
        public static float GetQualityFactor(RehabRecord? rehab)
        {
            if (rehab == null) return 1.0f; // Intact default
            return rehab.QualityRampPermille / 1000f;
        }
    }
}
