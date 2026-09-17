// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Endgame
{
    /// <summary>
    /// Immutable input record gathering campaign facts for epilogue evaluation (Plan 19 / INV-19.1).
    /// Pure Core DTO with zero engine dependencies.
    /// </summary>
    public sealed record EpilogueContextInputs(
        int Days,
        int LivingDwellers,
        int DeathsRecorded,
        bool GrandTreatySigned,
        bool TempestDecommissioned,
        bool DebtLedgersBurned,
        bool ChildrenSurvived,
        bool VelSecretExposed,
        IReadOnlyList<string>? SourceIds = null);

    /// <summary>
    /// Factory for creating EpilogueEvaluationContext and CampaignOutcomeSnapshot
    /// from live campaign authorities (Plan 19 / FX-01).
    /// </summary>
    public static class EpilogueContextFactory
    {
        public static EpilogueEvaluationContext Build(EpilogueContextInputs inputs)
        {
            if (inputs == null) throw new ArgumentNullException(nameof(inputs));
            if (inputs.Days < 0) throw new ArgumentOutOfRangeException(nameof(inputs), "Days must be non-negative.");
            if (inputs.LivingDwellers < 0) throw new ArgumentOutOfRangeException(nameof(inputs), "LivingDwellers must be non-negative.");
            if (inputs.DeathsRecorded < 0) throw new ArgumentOutOfRangeException(nameof(inputs), "DeathsRecorded must be non-negative.");

            return new EpilogueEvaluationContext
            {
                totalDaysSurvived = inputs.Days,
                livingDwellerCount = inputs.LivingDwellers,
                totalDeathsRecorded = inputs.DeathsRecorded,
                grandTreatySigned = inputs.GrandTreatySigned,
                tempestDecommissioned = inputs.TempestDecommissioned,
                debtLedgersBurned = inputs.DebtLedgersBurned,
                childrenSurvived = inputs.ChildrenSurvived,
                velSecretExposed = inputs.VelSecretExposed
            };
        }

        public static CampaignOutcomeSnapshot CreateSnapshot(CampaignOutcomeEvaluationInput input)
            => CampaignOutcomeEvaluator.Evaluate(input);

        public static EpilogueEvaluationContext CreateContext(CampaignOutcomeEvaluationInput input)
            => CampaignOutcomeEvaluator.Evaluate(input).ToContext();

        public static EpilogueEvaluationContext CreateContext(CampaignOutcomeSnapshot snapshot)
        {
            if (snapshot == null) throw new ArgumentNullException(nameof(snapshot));
            return snapshot.ToContext();
        }
    }
}
