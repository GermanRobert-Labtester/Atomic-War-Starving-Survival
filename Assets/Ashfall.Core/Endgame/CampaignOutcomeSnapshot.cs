using System;
using System.Collections.Generic;

namespace Ashfall.Core.Endgame
{
    /// <summary>
    /// Evaluated endgame outcome snapshot (FX-01 / Plan 19).
    /// Holds the authoritative, derived facts of a completed or inspected campaign,
    /// the evaluated regional/demographic/moral classifications, the narrative prose,
    /// and an outcome trace detailing why each predicate was determined true or false.
    /// </summary>
    [Serializable]
    public sealed class CampaignOutcomeSnapshot
    {
        // ── Evaluated Outcome Classifications ────────────────────────
        public RegionalFate Fate { get; set; }
        public DemographicOutcome Demographics { get; set; }
        public MoralStanding MoralStanding { get; set; }
        public string NarrativeProse { get; set; } = string.Empty;

        // ── Canonical Campaign Facts ─────────────────────────────────
        public int TotalDaysSurvived { get; set; }
        public int LivingDwellerCount { get; set; }
        public int TotalDeathsRecorded { get; set; }
        public bool GrandTreatySigned { get; set; }
        public bool TempestDecommissioned { get; set; }
        public bool DebtLedgersBurned { get; set; }
        public bool ChildrenSurvived { get; set; }
        public bool VelSecretExposed { get; set; }

        // ── Extended Metrics ─────────────────────────────────────────
        public int ActiveDebtsCount { get; set; }
        public int RatifiedTreatiesCount { get; set; }
        public int ChildrenCount { get; set; }
        public string VerdictEndingKey { get; set; } = string.Empty;

        // ── Outcome Trace ────────────────────────────────────────────
        /// <summary>
        /// Forensic derivation log explaining why every ending predicate was resolved true or false.
        /// </summary>
        public List<string> OutcomeTrace { get; set; } = new List<string>();

        /// <summary>
        /// Projects this snapshot into the classic matrix evaluation context.
        /// </summary>
        public EpilogueEvaluationContext ToContext()
        {
            return new EpilogueEvaluationContext
            {
                totalDaysSurvived = TotalDaysSurvived,
                livingDwellerCount = LivingDwellerCount,
                totalDeathsRecorded = TotalDeathsRecorded,
                grandTreatySigned = GrandTreatySigned,
                tempestDecommissioned = TempestDecommissioned,
                debtLedgersBurned = DebtLedgersBurned,
                childrenSurvived = ChildrenSurvived,
                velSecretExposed = VelSecretExposed
            };
        }
    }
}
