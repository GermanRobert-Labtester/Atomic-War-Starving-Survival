// SPDX-License-Identifier: MIT
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
}
