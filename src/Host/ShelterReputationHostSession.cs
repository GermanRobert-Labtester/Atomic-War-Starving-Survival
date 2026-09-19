// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : ShelterReputationHostSession
// Core System  : Ashfall.Core.Reputation.ShelterReputationSystem
// Host Caller  : Main.ShelterReputation
// Purpose      : Plan 207 — Coordinates shelter reputation, notoriety, public
//                tags, and external perception evidence across the wasteland.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core.Reputation;

namespace AtomicWar.GodotApp
{
    public sealed class ShelterReputationHostSession
    {
        private readonly ShelterReputationSystem _system;

        public ShelterReputationSystem System => _system;

        public event Action? StateChanged;

        public float Notoriety => _system.Notoriety;
        public IReadOnlyList<ReputationTag> ActiveTags => _system.ActiveTags;
        public int EvidenceCount => _system.EvidenceCount;

        public ShelterReputationHostSession(ShelterReputationSystem system)
        {
            _system = system ?? throw new ArgumentNullException(nameof(system));

            _system.OnEvidenceRecorded += _ => StateChanged?.Invoke();
            _system.OnTagAcquired += _ => StateChanged?.Invoke();
            _system.OnTagLost += _ => StateChanged?.Invoke();
            _system.OnNotorietyChanged += _ => StateChanged?.Invoke();
            _system.OnStateChanged += () => StateChanged?.Invoke();
        }

        public bool RecordEvidence(
            string eventId,
            ReputationDimension dimension,
            float delta,
            InformationMedium medium,
            int currentDay,
            string description = "")
        {
            var evidence = _system.RecordEvidence(eventId, dimension, delta, medium, currentDay, description);
            if (evidence != null)
            {
                StateChanged?.Invoke();
                return true;
            }
            return false;
        }

        public void TickDay(int currentDay)
        {
            _system.TickDay(currentDay);
            StateChanged?.Invoke();
        }

        public float GetScore(ReputationDimension dimension)
        {
            return _system.GetScore(dimension);
        }

        public bool HasTag(ReputationTag tag)
        {
            return _system.HasTag(tag);
        }

        public IReadOnlyList<ReputationEvidence> GetEvidenceHistory()
        {
            return _system.State.EvidenceHistory;
        }

        public ShelterReputationState CaptureState()
        {
            return _system.CaptureState();
        }

        public void RestoreState(ShelterReputationState state)
        {
            _system.RestoreState(state);
            StateChanged?.Invoke();
        }
    }
}
