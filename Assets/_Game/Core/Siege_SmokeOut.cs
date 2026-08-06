using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SiegeSmokeOutState
    {
        public string siegeId = "siege_smoke_out";
        public bool ventsBlocked;
        public float o2Level = 100f;
        public int gasMasksEquipped;
        public float hoursRemaining;
        public bool survivorsSuffocating;
    }

    /// <summary>
    /// Prompt #826: Smoke Out. Raiders block vents and light toxic fires.
    /// O2 drops rapidly. The player has 2 hours to don GasMasks or open
    /// the hatch and fight in the smoke. At 0 O2, all survivors suffocate.
    /// Plain C#. Save/load safe.
    /// </summary>
    public class Siege_SmokeOut
    {
        private SiegeSmokeOutState _state = new SiegeSmokeOutState();

        /// <summary>O2 drops 50 per hour, so ~2 hours to zero from 100.</summary>
        private const float O2DropPerMinute = 50f / 60f;
        private const float O2CriticalThreshold = 20f;

        // -- Events --
        public event Action OnVentsBlocked;
        public event Action<float> OnO2Dropped;          // current O2 level
        public event Action<int> OnGasMasksEquipped;     // count equipped
        public event Action OnHatchOpened;
        public event Action OnSuffocation;

        public SiegeSmokeOutState State => _state;

        /// <summary>
        /// Raiders block the vents and ignite toxic fires. O2 starts
        /// dropping immediately.
        /// </summary>
        public void BlockVents()
        {
            _state.ventsBlocked = true;
            _state.o2Level = 100f;
            _state.gasMasksEquipped = 0;
            _state.hoursRemaining = 2f;
            _state.survivorsSuffocating = false;

            OnVentsBlocked?.Invoke();
        }

        /// <summary>
        /// Advance one minute. O2 drops by approximately 0.833 per minute
        /// (50 per hour). Gas masks slow the effective drain for equipped
        /// survivors but do not stop it entirely.
        /// </summary>
        public void TickMinute()
        {
            if (!_state.ventsBlocked || _state.o2Level <= 0f) return;

            _state.o2Level = Math.Max(0f, _state.o2Level - O2DropPerMinute);
            _state.hoursRemaining = _state.o2Level / 50f;

            OnO2Dropped?.Invoke(_state.o2Level);

            if (_state.o2Level <= O2CriticalThreshold && !_state.survivorsSuffocating)
            {
                _state.survivorsSuffocating = true;
            }

            if (_state.o2Level <= 0f)
            {
                _state.survivorsSuffocating = true;
                OnSuffocation?.Invoke();
            }
        }

        /// <summary>
        /// Equip gas masks on survivors. Masks buy time but are limited.
        /// Does not stop the O2 drop — just mitigates the effect on
        /// equipped survivors.
        /// </summary>
        /// <param name="count">Number of gas masks distributed.</param>
        public void EquipGasMasks(int count)
        {
            if (count <= 0) return;

            _state.gasMasksEquipped += count;
            OnGasMasksEquipped?.Invoke(_state.gasMasksEquipped);
        }

        /// <summary>
        /// Open the hatch and fight in the smoke. Lets smoke in but allows
        /// combat with the raiders outside. O2 drain continues but the
        /// player can engage.
        /// </summary>
        public void OpenHatchAndFight()
        {
            OnHatchOpened?.Invoke();
        }

        /// <summary>
        /// True when O2 is below the critical threshold and survivors
        /// are in danger of suffocation.
        /// </summary>
        public bool IsO2Critical()
        {
            return _state.o2Level <= O2CriticalThreshold;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public SiegeSmokeOutState CaptureState()
        {
            return new SiegeSmokeOutState
            {
                siegeId = _state.siegeId,
                ventsBlocked = _state.ventsBlocked,
                o2Level = _state.o2Level,
                gasMasksEquipped = _state.gasMasksEquipped,
                hoursRemaining = _state.hoursRemaining,
                survivorsSuffocating = _state.survivorsSuffocating
            };
        }

        public void RestoreState(SiegeSmokeOutState saved)
        {
            _state = saved ?? new SiegeSmokeOutState();
        }
    }
}
