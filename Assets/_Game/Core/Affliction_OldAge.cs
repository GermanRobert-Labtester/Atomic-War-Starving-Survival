using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class OldAgeState
    {
        public string afflictionId = "affliction_old_age";
        public int dayThreshold = 200;
        public float dailyStatLoss = 1f;
        public bool isBedridden = false;
        public List<string> bedriddenSurvivorIds = new List<string>();
        public List<string> passedSurvivorIds = new List<string>();
        public List<string> trackedSurvivorIds = new List<string>();
        public List<int> bedriddenDayCounts = new List<int>();
    }

    public class Affliction_OldAge
    {
        public event Action<string, float> OnStatDecayed;
        public event Action<string> OnBedridden;
        public event Action<string> OnPeacefulPassing;

        private readonly OldAgeState _state;
        private const int BedriddenPassingDays = 10;

        public Affliction_OldAge()
        {
            _state = new OldAgeState();
        }

        public (float newStamina, float newHealth) TickDay(string survivorId, int currentDay, float currentMaxStamina, float currentMaxHealth)
        {
            if (currentDay <= _state.dayThreshold)
                return (currentMaxStamina, currentMaxHealth);

            if (_state.passedSurvivorIds.Contains(survivorId))
                return (currentMaxStamina, currentMaxHealth);

            float newStamina = Mathf.Max(0f, currentMaxStamina - _state.dailyStatLoss);
            float newHealth = Mathf.Max(0f, currentMaxHealth - _state.dailyStatLoss);

            OnStatDecayed?.Invoke(survivorId, _state.dailyStatLoss);

            if (newStamina <= 0f || newHealth <= 0f)
            {
                if (!_state.bedriddenSurvivorIds.Contains(survivorId))
                {
                    _state.bedriddenSurvivorIds.Add(survivorId);
                    _state.trackedSurvivorIds.Add(survivorId);
                    _state.bedriddenDayCounts.Add(0);
                    OnBedridden?.Invoke(survivorId);
                }
                else
                {
                    int idx = _state.bedriddenSurvivorIds.IndexOf(survivorId);
                    if (idx >= 0 && idx < _state.bedriddenDayCounts.Count)
                    {
                        _state.bedriddenDayCounts[idx]++;
                        if (_state.bedriddenDayCounts[idx] >= BedriddenPassingDays)
                        {
                            _state.passedSurvivorIds.Add(survivorId);
                            OnPeacefulPassing?.Invoke(survivorId);
                        }
                    }
                }
            }

            return (newStamina, newHealth);
        }

        public bool IsBedridden(string survivorId)
        {
            return _state.bedriddenSurvivorIds.Contains(survivorId);
        }

        public OldAgeState CaptureState() => _state;

        public void RestoreState(OldAgeState state)
        {
            _state.afflictionId = state.afflictionId;
            _state.dayThreshold = state.dayThreshold;
            _state.dailyStatLoss = state.dailyStatLoss;
            _state.isBedridden = state.isBedridden;
            _state.bedriddenSurvivorIds = new List<string>(state.bedriddenSurvivorIds);
            _state.passedSurvivorIds = new List<string>(state.passedSurvivorIds);
            _state.trackedSurvivorIds = new List<string>(state.trackedSurvivorIds);
            _state.bedriddenDayCounts = new List<int>(state.bedriddenDayCounts);
        }
    }
}
