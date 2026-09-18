// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Commitments
{
    public enum CommitmentStatus
    {
        Pending = 0,
        Warning = 1,
        Active = 2,
        Met = 3,
        Missed = 4
    }

    /// <summary>
    /// ASHFALL — Immutable read model exposing an obligation's current state (Plan 38 §38C.6).
    /// </summary>
    public sealed class CommitmentReadModel
    {
        public string Id { get; }
        public string Type { get; }
        public string Title { get; }
        public string Counterparty { get; }
        public int StartDay { get; }
        public int DueDay { get; }
        public int DaysRemaining { get; }
        public CommitmentStatus Status { get; }
        public int TargetQuantity { get; }
        public int CurrentQuantity { get; }
        public string TargetId { get; }
        public string ConditionType { get; }
        public string ConsequenceClass { get; }
        public string ConsequenceTarget { get; }
        public int ConsequenceMagnitude { get; }

        public CommitmentReadModel(
            string id,
            string type,
            string title,
            string counterparty,
            int startDay,
            int dueDay,
            int daysRemaining,
            CommitmentStatus status,
            int targetQuantity,
            int currentQuantity,
            string targetId,
            string conditionType,
            string consequenceClass,
            string consequenceTarget,
            int consequenceMagnitude)
        {
            Id = id ?? string.Empty;
            Type = type ?? string.Empty;
            Title = title ?? string.Empty;
            Counterparty = counterparty ?? string.Empty;
            StartDay = startDay;
            DueDay = dueDay;
            DaysRemaining = daysRemaining;
            Status = status;
            TargetQuantity = targetQuantity;
            CurrentQuantity = currentQuantity;
            TargetId = targetId ?? string.Empty;
            ConditionType = conditionType ?? string.Empty;
            ConsequenceClass = consequenceClass ?? string.Empty;
            ConsequenceTarget = consequenceTarget ?? string.Empty;
            ConsequenceMagnitude = consequenceMagnitude;
        }
    }
}
