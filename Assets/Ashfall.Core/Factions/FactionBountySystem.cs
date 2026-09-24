// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Factions
{
    public enum FactionBountySeverity
    {
        None = 0,
        Moderate = 1,
        Severe = 2,
        Extreme = 3
    }

    public enum FactionBountyState
    {
        Active = 0,
        Resolved = 1,
        Forgiven = 2
    }

    [Serializable]
    public sealed class FactionBountyProvenance
    {
        public string SourceType { get; set; } = "patrol_violation";
        public string EncounterId { get; set; } = string.Empty;
        public string ChoiceId { get; set; } = string.Empty;
        public string SourceResolutionId { get; set; } = string.Empty;
        public int Day { get; set; }
    }

    [Serializable]
    public sealed class FactionBountyRecord
    {
        public string BountyId { get; set; } = string.Empty;
        public string FactionId { get; set; } = string.Empty;
        public int AuthoredStandingDelta { get; set; }
        public FactionBountySeverity Severity { get; set; } = FactionBountySeverity.Moderate;
        public FactionBountyState State { get; set; } = FactionBountyState.Active;
        public FactionBountyProvenance Provenance { get; set; } = new();
        public int IssuedDay { get; set; }
        public int? ResolvedDay { get; set; }
    }

    [Serializable]
    public sealed class FactionBountySystemState
    {
        public List<FactionBountyRecord> Bounties { get; set; } = new();
    }

    /// <summary>
    /// Engine-agnostic bounty authority for faction infractions, combat violations,
    /// and severe patrol disobedience (Flagship VII / F10).
    /// Pure C#, deterministic, zero engine dependencies.
    /// </summary>
    public sealed class FactionBountySystem
    {
        public const string SystemId = "faction_bounty_system";
        public const int PatrolBountyStandingThreshold = -10;

        private readonly FactionBountySystemState _state;

        public FactionBountySystemState State => _state;
        public IReadOnlyList<FactionBountyRecord> AllBounties => _state.Bounties;

        public event Action<FactionBountyRecord>? OnBountyIssued;
        public event Action<FactionBountyRecord>? OnBountyResolved;

        public FactionBountySystem(FactionBountySystemState? state = null)
        {
            _state = new FactionBountySystemState();
            RestoreState(state);
        }

        public static FactionBountySeverity CalculateSeverity(int authoredStandingDelta)
        {
            if (authoredStandingDelta <= -20)
                return FactionBountySeverity.Extreme;
            if (authoredStandingDelta <= -15)
                return FactionBountySeverity.Severe;
            if (authoredStandingDelta <= -10)
                return FactionBountySeverity.Moderate;
            return FactionBountySeverity.None;
        }

        public FactionBountyRecord? IssuePatrolBounty(
            string factionId,
            string encounterId,
            string choiceId,
            int authoredStandingDelta,
            int day)
        {
            if (string.IsNullOrWhiteSpace(factionId)
                || string.IsNullOrWhiteSpace(encounterId)
                || string.IsNullOrWhiteSpace(choiceId)
                || day < 0)
            {
                return null;
            }
            if (authoredStandingDelta > PatrolBountyStandingThreshold)
            {
                return null;
            }

            string canonicalFaction = FactionStandingIdResolver.ToSystemsId(factionId);
            string sourceResolutionId = $"{encounterId}:{choiceId}:{day}";

            // Deduplication by sourceResolutionId
            var existing = _state.Bounties.Find(b =>
                string.Equals(b.Provenance?.SourceResolutionId, sourceResolutionId, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                return existing;
            }

            var severity = CalculateSeverity(authoredStandingDelta);
            string bountyId = $"bounty_{canonicalFaction}_{day}_{_state.Bounties.Count + 1}";

            var record = new FactionBountyRecord
            {
                BountyId = bountyId,
                FactionId = canonicalFaction,
                AuthoredStandingDelta = authoredStandingDelta,
                Severity = severity,
                State = FactionBountyState.Active,
                IssuedDay = day,
                Provenance = new FactionBountyProvenance
                {
                    SourceType = "patrol_violation",
                    EncounterId = encounterId ?? string.Empty,
                    ChoiceId = choiceId ?? string.Empty,
                    SourceResolutionId = sourceResolutionId,
                    Day = day
                }
            };

            _state.Bounties.Add(record);
            OnBountyIssued?.Invoke(record);
            return record;
        }

        public bool ResolveBounty(string bountyId, int day)
        {
            if (string.IsNullOrWhiteSpace(bountyId)) return false;
            var bounty = _state.Bounties.Find(b => string.Equals(b.BountyId, bountyId, StringComparison.OrdinalIgnoreCase));
            if (bounty != null && bounty.State == FactionBountyState.Active)
            {
                bounty.State = FactionBountyState.Resolved;
                bounty.ResolvedDay = day;
                OnBountyResolved?.Invoke(bounty);
                return true;
            }
            return false;
        }

        public int ClearBountiesForFaction(string factionId, int day)
        {
            if (string.IsNullOrWhiteSpace(factionId)) return 0;
            string canonical = FactionStandingIdResolver.ToSystemsId(factionId);
            int count = 0;

            foreach (var b in _state.Bounties)
            {
                if (b.State == FactionBountyState.Active && string.Equals(b.FactionId, canonical, StringComparison.OrdinalIgnoreCase))
                {
                    b.State = FactionBountyState.Resolved;
                    b.ResolvedDay = day;
                    count++;
                    OnBountyResolved?.Invoke(b);
                }
            }

            return count;
        }

        public bool HasActiveBounty(string factionId)
        {
            if (string.IsNullOrWhiteSpace(factionId)) return false;
            string canonical = FactionStandingIdResolver.ToSystemsId(factionId);
            return _state.Bounties.Exists(b => b.State == FactionBountyState.Active &&
                string.Equals(b.FactionId, canonical, StringComparison.OrdinalIgnoreCase));
        }

        public IReadOnlyList<FactionBountyRecord> GetActiveBounties()
        {
            return _state.Bounties.Where(b => b.State == FactionBountyState.Active).ToList();
        }

        public IReadOnlyList<FactionBountyRecord> GetActiveBountiesForFaction(string factionId)
        {
            if (string.IsNullOrWhiteSpace(factionId)) return Array.Empty<FactionBountyRecord>();
            string canonical = FactionStandingIdResolver.ToSystemsId(factionId);
            return _state.Bounties.Where(b => b.State == FactionBountyState.Active &&
                string.Equals(b.FactionId, canonical, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public FactionBountySystemState CaptureState()
        {
            var copy = new FactionBountySystemState
            {
                Bounties = new List<FactionBountyRecord>()
            };

            if (_state.Bounties != null)
            {
                foreach (var bounty in _state.Bounties)
                {
                    var cloned = CloneRecord(bounty);
                    if (cloned != null)
                        copy.Bounties.Add(cloned);
                }
            }

            return copy;
        }

        public void RestoreState(FactionBountySystemState? state)
        {
            _state.Bounties = new List<FactionBountyRecord>();
            if (state?.Bounties == null) return;

            foreach (var bounty in state.Bounties)
            {
                var cloned = CloneRecord(bounty);
                if (cloned != null)
                    _state.Bounties.Add(cloned);
            }
        }

        private static FactionBountyRecord? CloneRecord(FactionBountyRecord? source)
        {
            if (source == null) return null;

            return new FactionBountyRecord
            {
                BountyId = source.BountyId ?? string.Empty,
                FactionId = source.FactionId ?? string.Empty,
                AuthoredStandingDelta = source.AuthoredStandingDelta,
                Severity = source.Severity,
                State = source.State,
                IssuedDay = source.IssuedDay,
                ResolvedDay = source.ResolvedDay,
                Provenance = new FactionBountyProvenance
                {
                    SourceType = source.Provenance?.SourceType ?? "patrol_violation",
                    EncounterId = source.Provenance?.EncounterId ?? string.Empty,
                    ChoiceId = source.Provenance?.ChoiceId ?? string.Empty,
                    SourceResolutionId = source.Provenance?.SourceResolutionId ?? string.Empty,
                    Day = source.Provenance?.Day ?? source.IssuedDay
                }
            };
        }
    }
}
