using System;

namespace Ashfall.Core
{
    /// <summary>Status lifecycle of a debt bounty obligation.</summary>
    public enum DebtBountyStatus
    {
        Pending,
        Resolved,
        Cancelled
    }

    /// <summary>
    /// Thin persisted record representing enforcement raid pressure requested by a
    /// defaulted debt consequence (Section F5 Path B).
    /// </summary>
    [Serializable]
    public class DebtBountyRecord
    {
        public string id = string.Empty;
        public string factionId = string.Empty;
        public string severity = string.Empty; // "moderate", "severe", "low"
        public string sourceDebtId = string.Empty;
        public int issuedDay;
        public DebtBountyStatus status = DebtBountyStatus.Pending;
        public int resolvedDay = -1;
        public int cancelledDay = -1;

        public DebtBountyRecord Clone() => new DebtBountyRecord
        {
            id = id,
            factionId = factionId,
            severity = severity,
            sourceDebtId = sourceDebtId,
            issuedDay = issuedDay,
            status = status,
            resolvedDay = resolvedDay,
            cancelledDay = cancelledDay
        };
    }

    /// <summary>
    /// Central mapping from debt consequence severity to raid pressure and priority (F5.1 / F5.3).
    /// </summary>
    public static class DebtBountySeverity
    {
        public const float LowBountyBoost = 0.05f;
        public const float ModerateBountyBoost = 0.15f;
        public const float SevereRaidBoost = 0.30f;
        public const float MaxAggregateBountyBoost = 0.50f;

        public static float GetRaidChanceBoost(string severity)
        {
            if (string.Equals(severity, "severe", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(severity, "raid", StringComparison.OrdinalIgnoreCase))
                return SevereRaidBoost;
            if (string.Equals(severity, "moderate", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(severity, "bounty", StringComparison.OrdinalIgnoreCase))
                return ModerateBountyBoost;
            if (string.Equals(severity, "low", StringComparison.OrdinalIgnoreCase))
                return LowBountyBoost;
            return ModerateBountyBoost;
        }

        public static int GetPriority(string severity)
        {
            if (string.Equals(severity, "severe", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(severity, "raid", StringComparison.OrdinalIgnoreCase))
                return 3;
            if (string.Equals(severity, "moderate", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(severity, "bounty", StringComparison.OrdinalIgnoreCase))
                return 2;
            return 1;
        }
    }
}
