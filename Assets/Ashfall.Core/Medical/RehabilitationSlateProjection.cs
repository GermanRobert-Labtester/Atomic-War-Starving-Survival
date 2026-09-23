// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// EN-04 / UNBLOCK-05: Rehabilitation Medicine Slate Read Model.
    /// Pure projection over SurvivorBodyState and medical facts rendering the
    /// survivor's prosthetic status, adaptation progress, next milestone, and phantom pain state.
    /// Renders for SurvivorDetailPanel and medical ward patient slates without mutating state.
    /// </summary>
    public sealed class RehabilitationSlateProjection
    {
        public string SurvivorId { get; }
        public bool HasProsthetics { get; }
        public int FittedProstheticsCount { get; }
        public string CurrentPhase { get; }
        public int DaysInPhase { get; }
        public float QualityPercent { get; }
        public string NextMilestone { get; }
        public bool HasPhantomPain { get; }

        public RehabilitationSlateProjection(
            string survivorId,
            bool hasProsthetics,
            int fittedProstheticsCount,
            string currentPhase,
            int daysInPhase,
            float qualityPercent,
            string nextMilestone,
            bool hasPhantomPain)
        {
            SurvivorId = survivorId ?? string.Empty;
            HasProsthetics = hasProsthetics;
            FittedProstheticsCount = Math.Max(0, fittedProstheticsCount);
            CurrentPhase = currentPhase ?? "none";
            DaysInPhase = Math.Max(0, daysInPhase);
            QualityPercent = Math.Clamp(qualityPercent, 0f, 100f);
            NextMilestone = nextMilestone ?? string.Empty;
            HasPhantomPain = hasPhantomPain;
        }

        public static RehabilitationSlateProjection Project(
            string survivorId,
            SurvivorBodyState? bodyState,
            bool hasPhantomPain = false,
            int fittingDurationDays = RehabilitationProgressionEngine.DefaultFittingDurationDays,
            int adaptationDurationDays = RehabilitationProgressionEngine.DefaultAdaptationDurationDays)
        {
            if (bodyState == null)
            {
                return new RehabilitationSlateProjection(
                    survivorId,
                    hasProsthetics: false,
                    fittedProstheticsCount: 0,
                    currentPhase: "none",
                    daysInPhase: 0,
                    qualityPercent: 100f,
                    nextMilestone: "No prosthetics fitted",
                    hasPhantomPain: hasPhantomPain);
            }

            int prostheticsCount = 0;
            foreach (var limb in bodyState.Limbs.Values)
            {
                if (string.Equals(limb.Condition, "prosthetized", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(limb.Condition, "prosthetic", StringComparison.OrdinalIgnoreCase))
                {
                    prostheticsCount++;
                }
            }

            if (prostheticsCount == 0 && bodyState.Rehab == null)
            {
                return new RehabilitationSlateProjection(
                    survivorId,
                    hasProsthetics: false,
                    fittedProstheticsCount: 0,
                    currentPhase: "none",
                    daysInPhase: 0,
                    qualityPercent: 100f,
                    nextMilestone: "No prosthetics fitted",
                    hasPhantomPain: hasPhantomPain);
            }

            var rehab = bodyState.Rehab;
            string phase = rehab?.Phase?.ToLowerInvariant() ?? "none";
            int daysInPhase = rehab?.DaysInPhase ?? 0;
            float qualityPercent = (rehab?.QualityRampPermille ?? 1000) / 10f;
            string nextMilestone;

            switch (phase)
            {
                case "fitting":
                    int fittingLeft = Math.Max(1, fittingDurationDays - daysInPhase);
                    nextMilestone = $"Adaptation phase in {fittingLeft} day{(fittingLeft == 1 ? "" : "s")}";
                    break;

                case "adaptation":
                    int adaptLeft = Math.Max(1, adaptationDurationDays - daysInPhase);
                    nextMilestone = $"Full mastery in {adaptLeft} day{(adaptLeft == 1 ? "" : "s")}";
                    break;

                case "mastery":
                    nextMilestone = "Prosthetic mastery achieved";
                    break;

                default:
                    nextMilestone = "Stable";
                    break;
            }

            return new RehabilitationSlateProjection(
                survivorId,
                hasProsthetics: prostheticsCount > 0,
                fittedProstheticsCount: prostheticsCount,
                currentPhase: phase,
                daysInPhase: daysInPhase,
                qualityPercent: qualityPercent,
                nextMilestone: nextMilestone,
                hasPhantomPain: hasPhantomPain);
        }
    }
}
