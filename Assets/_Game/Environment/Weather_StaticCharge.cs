using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Environment
{
    [Serializable]
    public class StaticChargeState
    {
        public string weatherId = "weather_static_charge";
        public string displayName = "Static Charge";
        public bool isActive = false;
        public float durationHours = 8f;
        public float hoursRemaining = 0f;
        public float shockDamage = 15f;
        public List<string> affectedModules = new List<string>();
    }

    /// <summary>
    /// Prompt #653: Weather — Static Charge.
    /// Air becomes heavily ionized, electrifying all metal shelter modules.
    /// Interacting with an affected module without RubberGloves deals Shock damage.
    /// </summary>
    public class Weather_StaticCharge
    {
        private StaticChargeState _state = new StaticChargeState();

        // -- Events --
        public event Action<StaticChargeState> OnStaticChargeTriggered;
        public event Action<StaticChargeState> OnStaticChargeEnded;
        public event Action<StaticChargeState, string, float> OnShockDamageDealt;

        public StaticChargeState State => _state;

        public bool IsActive => _state.hoursRemaining > 0f;

        /// <summary>
        /// Triggers the static charge event, electrifying all registered modules.
        /// </summary>
        public void Trigger()
        {
            _state.isActive = true;
            _state.hoursRemaining = _state.durationHours;
            OnStaticChargeTriggered?.Invoke(_state);
        }

        /// <summary>
        /// Registers a module as affected by the static charge. Call during setup
        /// or when new metal modules are installed.
        /// </summary>
        public void RegisterModule(string moduleId)
        {
            if (string.IsNullOrEmpty(moduleId)) return;
            if (!_state.affectedModules.Contains(moduleId))
            {
                _state.affectedModules.Add(moduleId);
            }
        }

        /// <summary>
        /// Per-hour tick. Decrements remaining time and ends the event when expired.
        /// </summary>
        public void TickHour()
        {
            if (!IsActive) return;

            _state.hoursRemaining = Mathf.Max(0f, _state.hoursRemaining - 1f);

            if (!IsActive)
            {
                _state.isActive = false;
                OnStaticChargeEnded?.Invoke(_state);
            }
        }

        /// <summary>
        /// Returns the shock damage dealt when interacting with the given module.
        /// Returns 0 if the module is not electrified or the survivor has RubberGloves.
        /// </summary>
        public float GetShockDamage(string moduleId, bool hasRubberGloves)
        {
            if (!IsActive) return 0f;
            if (!IsModuleElectrified(moduleId)) return 0f;
            if (hasRubberGloves) return 0f;

            float damage = _state.shockDamage;
            OnShockDamageDealt?.Invoke(_state, moduleId, damage);
            return damage;
        }

        /// <summary>
        /// Returns whether the given module is currently electrified.
        /// </summary>
        public bool IsModuleElectrified(string moduleId)
        {
            if (!IsActive) return false;
            if (string.IsNullOrEmpty(moduleId)) return false;
            return _state.affectedModules.Contains(moduleId);
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public StaticChargeState GetState() => _state;

        public StaticChargeState CaptureState()
        {
            return new StaticChargeState
            {
                weatherId = _state.weatherId,
                displayName = _state.displayName,
                isActive = _state.isActive,
                durationHours = _state.durationHours,
                hoursRemaining = _state.hoursRemaining,
                shockDamage = _state.shockDamage,
                affectedModules = _state.affectedModules != null
                    ? new List<string>(_state.affectedModules)
                    : new List<string>()
            };
        }

        public void RestoreState(StaticChargeState state)
        {
            _state = state ?? new StaticChargeState();
            if (_state.affectedModules == null)
                _state.affectedModules = new List<string>();
        }
    }
}
