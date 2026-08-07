using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class HitAndRunState
    {
        public string id = "encounter_hit_and_run";
        public string displayName = "Hit and Run Choice";
        public float killSuccessChance = 0.90f; // 90% chance
        public float moraleDrop = 25f;           // Massive morale penalty
        public float engineDamage = 40f;         // Severe engine block damage
    }

    /// <summary>
    /// Prompt #388: Encounter: Hit and Run.
    /// When engaged by hostile infantry while driving, player can choose "Run Them Down".
    /// 90% chance to kill enemy instantly, but causes massive Morale drop and severe engine block damage.
    /// </summary>
    public class Encounter_HitAndRun
    {
        private HitAndRunState _state = new HitAndRunState();

        public event Action<HitAndRunState, bool> OnRunThemDownExecuted;

        public HitAndRunState State => _state;

        public bool RunThemDown(ref float driverMorale, ref float vehicleEngineDurability, System.Random rng)
        {
            bool success = rng.NextDouble() < _state.killSuccessChance;
            driverMorale = Mathf.Max(0f, driverMorale - _state.moraleDrop);
            vehicleEngineDurability = Mathf.Max(0f, vehicleEngineDurability - _state.engineDamage);

            OnRunThemDownExecuted?.Invoke(_state, success);
            return success;
        }

        public HitAndRunState CaptureState() => _state;

        public void RestoreState(HitAndRunState saved)
        {
            _state = saved ?? new HitAndRunState();
        }
    }
}
