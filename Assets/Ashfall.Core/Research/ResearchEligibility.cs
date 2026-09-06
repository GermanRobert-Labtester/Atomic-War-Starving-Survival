// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Failure/status codes for research start eligibility.
    /// </summary>
    public enum ResearchEligibilityCode
    {
        Eligible = 0,
        UnknownNode = 1,
        NotDiscovered = 2,
        MissingPrerequisites = 3,
        AlreadyActive = 4,
        AnotherResearchActive = 5,
        AlreadyCompleted = 6
    }

    /// <summary>
    /// Immutable structured evaluation of whether a knowledge node is currently eligible to be researched.
    /// </summary>
    public sealed class ResearchEligibility
    {
        public string Id { get; }
        public bool CanStart { get; }
        public bool IsDiscovered { get; }
        public bool IsCompleted { get; }
        public bool IsActive { get; }
        public IReadOnlyList<string> MissingPrerequisites { get; }
        public string? ConflictingActiveResearchId { get; }
        public ResearchEligibilityCode Code { get; }

        public ResearchEligibility(
            string id,
            bool canStart,
            bool isDiscovered,
            bool isCompleted,
            bool isActive,
            IReadOnlyList<string>? missingPrerequisites,
            string? conflictingActiveResearchId,
            ResearchEligibilityCode code)
        {
            Id = id ?? string.Empty;
            CanStart = canStart;
            IsDiscovered = isDiscovered;
            IsCompleted = isCompleted;
            IsActive = isActive;
            MissingPrerequisites = missingPrerequisites ?? Array.Empty<string>();
            ConflictingActiveResearchId = conflictingActiveResearchId;
            Code = code;
        }

        public override string ToString() =>
            $"[ResearchEligibility: {Id} CanStart={CanStart} Code={Code} Active={IsActive} Completed={IsCompleted} Discovered={IsDiscovered}]";
    }
}
