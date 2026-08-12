using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Leadership System — designating an informal bunker leader grants morale
    /// bonuses during crises but accumulates high personal stress when deaths
    /// or severe injuries occur. At 100 stress: 3x mental break risk.
    ///
    /// Owns: Survivor.IsDesignatedLeader, Survivor.LeaderStressAccumulation,
    /// Survivor.LeaderDeathsWitnessed.
    /// </summary>
    public class LeadershipSystem
    {
        public const float LeaderCrisisMoraleAura = 10f;
        public const float LeaderStressPerDeath = 25f;
        public const float LeaderStressPerInjury = 10f;
        public const float LeaderStressDecayPerDay = 2f;
        public const float LeaderStressMax = 100f;
        public const float LeaderBreakRiskMultiplier = 3f;
        public const float StepDownCooldownDays = 14f;

        public event Action<Survivor> OnLeaderDesignated;
        public event Action<Survivor> OnLeaderSteppedDown;
        public event Action<Survivor, float> OnLeaderStressIncreased;
        public event Action<Survivor> OnLeaderBreakRisk;

        // Host hooks
        public Action<Survivor, float> ApplyMoraleDelta;
        public Action<float> ApplyShelterMoraleDelta;
        public Func<IReadOnlyList<Survivor>> GetSurvivors;

        private string _currentLeaderId;
        private float _stepDownCooldown;

        public string CurrentLeaderId => _currentLeaderId;

        public bool DesignateLeader(Survivor sv)
        {
            if (sv == null || !sv.IsAlive) return false;
            if (_stepDownCooldown > 0f) return false;

            // Clear previous leader
            if (!string.IsNullOrEmpty(_currentLeaderId))
            {
                var prevLeader = FindSurvivor(_currentLeaderId);
                if (prevLeader != null)
                {
                    prevLeader.IsDesignatedLeader = false;
                    prevLeader.LeaderStressAccumulation = 0f;
                }
            }

            sv.IsDesignatedLeader = true;
            _currentLeaderId = sv.Id;
            OnLeaderDesignated?.Invoke(sv);
            return true;
        }

        public bool StepDown(Survivor sv)
        {
            if (sv == null || sv.Id != _currentLeaderId) return false;
            sv.IsDesignatedLeader = false;
            _currentLeaderId = null;
            _stepDownCooldown = StepDownCooldownDays;
            OnLeaderSteppedDown?.Invoke(sv);
            return true;
        }

        public void OnSurvivorDied(Survivor dead)
        {
            if (dead == null || string.IsNullOrEmpty(_currentLeaderId)) return;
            var leader = FindSurvivor(_currentLeaderId);
            if (leader == null || !leader.IsAlive) return;

            leader.LeaderStressAccumulation =
                Math.Min(LeaderStressMax,
                leader.LeaderStressAccumulation + LeaderStressPerDeath);
            leader.LeaderDeathsWitnessed++;
            OnLeaderStressIncreased?.Invoke(leader, leader.LeaderStressAccumulation);

            if (leader.LeaderStressAccumulation >= LeaderStressMax)
                OnLeaderBreakRisk?.Invoke(leader);
        }

        public void OnCrisisEvent()
        {
            if (string.IsNullOrEmpty(_currentLeaderId)) return;
            var leader = FindSurvivor(_currentLeaderId);
            if (leader == null || !leader.IsAlive) return;

            // Apply morale aura to all survivors during crisis
            ApplyShelterMoraleDelta?.Invoke(LeaderCrisisMoraleAura);
        }

        public void Tick(float gameHours)
        {
            if (_stepDownCooldown > 0f)
                _stepDownCooldown -= gameHours / 24f;

            if (string.IsNullOrEmpty(_currentLeaderId)) return;
            var leader = FindSurvivor(_currentLeaderId);
            if (leader == null || !leader.IsAlive) return;

            leader.LeaderStressAccumulation = Math.Max(0f,
                leader.LeaderStressAccumulation -
                LeaderStressDecayPerDay * (gameHours / 24f));
        }

        private Survivor FindSurvivor(string id)
        {
            var survivors = GetSurvivors?.Invoke();
            if (survivors == null) return null;
            for (int i = 0; i < survivors.Count; i++)
            {
                if (survivors[i] != null && survivors[i].Id == id)
                    return survivors[i];
            }
            return null;
        }
    }
}
