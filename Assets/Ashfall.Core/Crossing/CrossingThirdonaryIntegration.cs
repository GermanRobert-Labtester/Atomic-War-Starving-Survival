using System;
using System.Collections.Generic;

namespace Ashfall.Core.Crossing
{
    public enum CovenantStatus
    {
        Unknown,
        Eligible,
        Active,
        Breached,
        Dissolved
    }

    public enum DisputeStatus
    {
        Unknown,
        Eligible,
        Active,
        Resolved,
        Escalated
    }

    public sealed class CovenantEligibilityResult
    {
        public CovenantStatus Status;
        public string CovenantId = string.Empty;
        public string Reason = string.Empty;
    }

    public sealed class DisputeEligibilityResult
    {
        public DisputeStatus Status;
        public string DisputeId = string.Empty;
        public string Reason = string.Empty;
    }

    /// <summary>
    /// Typed eligibility layer over CrossingArbitrationSystem and CrossingQuestSystem.
    /// Engine-agnostic thin query wrapper; no direct gameplay mutation.
    /// </summary>
    public sealed class CrossingThirdonaryIntegration
    {
        private static readonly HashSet<string> RecognizedCovenants = new(StringComparer.Ordinal)
        {
            "covenant_salvaged_accord",
            "covenant_bridge_toll",
            "covenant_water_charter"
        };

        private static readonly HashSet<string> RecognizedDisputes = new(StringComparer.Ordinal)
        {
            "dispute_registry_claim",
            "dispute_ferry_passage",
            "dispute_scrapline_border"
        };

        private readonly CrossingArbitrationSystem _arbitration;
        private readonly CrossingQuestSystem _quests;

        public CrossingThirdonaryIntegration(CrossingArbitrationSystem arbitration, CrossingQuestSystem quests)
        {
            _arbitration = arbitration ?? throw new ArgumentNullException(nameof(arbitration));
            _quests = quests ?? throw new ArgumentNullException(nameof(quests));
        }

        public CrossingArbitrationSystem Arbitration => _arbitration;
        public CrossingQuestSystem Quests => _quests;

        public bool IsOpeningQuestComplete() => _quests.IsQuestCompleted(CrossingQuestSystem.OpeningQuest);

        public CovenantEligibilityResult GetCovenantEligibility(string covenantId, int currentDay)
        {
            var result = new CovenantEligibilityResult { CovenantId = covenantId };

            if (string.IsNullOrEmpty(covenantId) || !RecognizedCovenants.Contains(covenantId))
            {
                result.Status = CovenantStatus.Unknown;
                result.Reason = $"Covenant '{covenantId}' is not recognized by the Crossing charter.";
                return result;
            }

            string flagBase = covenantId.StartsWith("covenant_", StringComparison.Ordinal)
                ? $"flag_{covenantId}"
                : $"flag_covenant_{covenantId}";

            if (_quests.HasFlag($"{flagBase}_breached") || _quests.HasFlag($"flag_covenant_{covenantId}_breached"))
            {
                result.Status = CovenantStatus.Breached;
                result.Reason = "Covenant was breached and its seal broken.";
                return result;
            }

            if (_quests.HasFlag($"{flagBase}_dissolved") || _quests.HasFlag($"flag_covenant_{covenantId}_dissolved"))
            {
                result.Status = CovenantStatus.Dissolved;
                result.Reason = "Covenant has dissolved by mutual accord.";
                return result;
            }

            if (_quests.HasFlag($"{flagBase}_active") || _quests.HasFlag($"flag_covenant_{covenantId}_active"))
            {
                result.Status = CovenantStatus.Active;
                result.Reason = "Covenant is active and in force.";
                return result;
            }

            if (!IsOpeningQuestComplete())
            {
                result.Status = CovenantStatus.Unknown;
                result.Reason = "Opening vouch quest not completed; counterparty will not hear covenant petitions.";
                return result;
            }

            result.Status = CovenantStatus.Eligible;
            result.Reason = "Eligible for covenant ratification.";
            return result;
        }

        public DisputeEligibilityResult GetDisputeEligibility(string disputeId, int currentDay)
        {
            var result = new DisputeEligibilityResult { DisputeId = disputeId };

            if (string.IsNullOrEmpty(disputeId) || !RecognizedDisputes.Contains(disputeId))
            {
                result.Status = DisputeStatus.Unknown;
                result.Reason = $"Dispute '{disputeId}' is not recognized.";
                return result;
            }

            string disputeBase = disputeId.StartsWith("dispute_", StringComparison.Ordinal)
                ? $"flag_{disputeId}"
                : $"flag_dispute_{disputeId}";

            if (_quests.HasFlag($"{disputeBase}_escalated") || _quests.HasFlag($"flag_dispute_{disputeId}_escalated"))
            {
                result.Status = DisputeStatus.Escalated;
                result.Reason = "Dispute escalated to armed reprisal or asset seizure.";
                return result;
            }

            if (_quests.HasFlag($"{disputeBase}_resolved") || _quests.HasFlag($"flag_dispute_{disputeId}_resolved"))
            {
                result.Status = DisputeStatus.Resolved;
                result.Reason = "Dispute settled through arbitration ruling.";
                return result;
            }

            if (_quests.HasFlag($"{disputeBase}_active") || _quests.HasFlag($"flag_dispute_{disputeId}_active"))
            {
                result.Status = DisputeStatus.Active;
                result.Reason = "Dispute is currently undergoing hearings before backers.";
                return result;
            }

            if (!IsOpeningQuestComplete())
            {
                result.Status = DisputeStatus.Unknown;
                result.Reason = "Opening vouch quest not completed; dispute docket locked.";
                return result;
            }

            result.Status = DisputeStatus.Eligible;
            result.Reason = "Eligible for thirdonary arbitration.";
            return result;
        }
    }
}
