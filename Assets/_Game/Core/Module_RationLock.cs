using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class RationLockState
    {
        public string moduleId = "module_ration_lock";
        public int dailyCap = 1;
        public float resentmentPerDay = 0.05f;
    }

    /// <summary>
    /// Prompt #807: Smart Rationing Locks.
    /// Player sets cap (e.g., "1 Meal Per Day"). Pantry locks when cap is reached.
    /// Prevents BingeEater/Theft but causes Resentment (Affinity drop) daily.
    /// </summary>
    public class Module_RationLock
    {
        public event Action<int> OnCapSet;                    // dailyCap
        public event Action<string> OnRationDenied;           // survivorId — tried to eat over cap
        public event Action<string> OnResentmentApplied;      // survivorId

        private RationLockState _state;

        public Module_RationLock(RationLockState state = null)
        {
            _state = state ?? new RationLockState();
        }

        public string ModuleId => _state.moduleId;

        /// <summary>
        /// Set the daily meal cap. Pantry locks once survivors hit this limit.
        /// </summary>
        public void SetCap(int mealsPerDay)
        {
            if (mealsPerDay < 0)
            {
                Debug.LogWarning("[Module_RationLock] SetCap called with negative value.");
                return;
            }

            _state.dailyCap = mealsPerDay;
            OnCapSet?.Invoke(mealsPerDay);
        }

        public int GetCap() => _state.dailyCap;

        /// <summary>
        /// Attempt to consume a meal. Returns false if survivor is over the daily cap.
        /// </summary>
        public bool TryConsume(string survivorId, int mealsConsumedToday)
        {
            if (string.IsNullOrEmpty(survivorId))
            {
                Debug.LogWarning("[Module_RationLock] TryConsume called with null/empty survivorId.");
                return false;
            }

            if (mealsConsumedToday >= _state.dailyCap)
            {
                OnRationDenied?.Invoke(survivorId);
                return false;
            }

            return true;
        }

        /// <summary>
        /// End-of-day tick: applies resentment to all survivors (Affinity drop from rationing).
        /// </summary>
        public void TickDay(List<string> survivorIds)
        {
            if (survivorIds == null)
            {
                Debug.LogWarning("[Module_RationLock] TickDay called with null survivorIds.");
                return;
            }

            foreach (string survivorId in survivorIds)
            {
                if (!string.IsNullOrEmpty(survivorId))
                {
                    OnResentmentApplied?.Invoke(survivorId);
                }
            }
        }

        public float GetResentmentPerDay() => _state.resentmentPerDay;

        public RationLockState CaptureState()
        {
            return new RationLockState
            {
                moduleId = _state.moduleId,
                dailyCap = _state.dailyCap,
                resentmentPerDay = _state.resentmentPerDay
            };
        }

        public void RestoreState(RationLockState state)
        {
            _state = state ?? new RationLockState();
        }
    }
}
