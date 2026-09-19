// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : RelationshipDecayHostSession
// Core System  : Ashfall.Core.Survivors.RelationshipDecaySystem
// Host Caller  : Main.RelationshipDecay
// Purpose      : Plan 182 — Survivor pair bond decay, interaction tracking, and social drift
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public sealed class RelationshipDecayHostSession
    {
        private readonly RelationshipDecaySystem _system;

        public RelationshipDecaySystem System => _system;

        public event Action? StateChanged;

        public int TrackedPairCount => _system.TrackedPairCount;
        public IReadOnlyList<PairBondState> Pairs => _system.Pairs;
        public IReadOnlyList<SocialDriftEvent> DriftHistory => _system.DriftHistory;

        public RelationshipDecayHostSession(RelationshipDecaySystem system)
        {
            _system = system ?? throw new ArgumentNullException(nameof(system));

            _system.OnBondDrifted += _ => StateChanged?.Invoke();
            _system.OnBondBroken += _ => StateChanged?.Invoke();
        }

        public PairBondState RegisterOrUpdatePair(
            string survivorA,
            string survivorB,
            SurvivorBondType bond,
            float initialAffinity = 25f,
            float initialTrust = 30f,
            int currentDay = 1)
        {
            var p = _system.RegisterOrUpdatePair(survivorA, survivorB, bond, initialAffinity, initialTrust, currentDay);
            StateChanged?.Invoke();
            return p;
        }

        public PairBondState? GetPair(string survivorA, string survivorB)
        {
            return _system.GetPair(survivorA, survivorB);
        }

        public bool RecordInteraction(string survivorA, string survivorB, string interactionType, float affinityBonus, int currentDay)
        {
            bool res = _system.RecordInteraction(survivorA, survivorB, interactionType, affinityBonus, currentDay);
            if (res)
                StateChanged?.Invoke();
            return res;
        }

        public void TickDay(int currentDay)
        {
            _system.TickDay(currentDay);
            StateChanged?.Invoke();
        }

        public RelationshipDecayState CaptureState()
        {
            return _system.CaptureState();
        }

        public void RestoreState(RelationshipDecayState state)
        {
            _system.RestoreState(state);
            StateChanged?.Invoke();
        }
    }
}
