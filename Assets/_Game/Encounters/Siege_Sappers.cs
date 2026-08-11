using System;

namespace AtomicWar._Game.Encounters
{
    [Serializable]
    public class SiegeSappersState
    {
        public string siegeId = "siege_sappers";
        public int turnsDigging;
        public bool tunnelComplete;
        public string breachLocation = "airlock_floor";
        public bool surfaceDefensesBypassed;
    }

    /// <summary>
    /// Prompt #820: Sapping Tunnels. Raiders dig under PerimeterTraps and
    /// Turrets, breaching the Airlock through the floor. Bypasses all
    /// surface defenses entirely. Player needs underground sensors to
    /// detect the digging early.
    /// Plain C#. Save/load safe.
    /// </summary>
    public class Siege_Sappers
    {
        private SiegeSappersState _state = new SiegeSappersState();

        private const int TurnsToDig = 5;

        // -- Events --
        public event Action OnDiggingStarted;
        public event Action OnTunnelComplete;
        public event Action<string> OnBreachTriggered;  // location

        public SiegeSappersState State => _state;

        /// <summary>Raiders begin digging beneath the bunker.</summary>
        public void StartDigging()
        {
            _state.turnsDigging = 0;
            _state.tunnelComplete = false;
            _state.surfaceDefensesBypassed = false;
            _state.breachLocation = "airlock_floor";

            OnDiggingStarted?.Invoke();
        }

        /// <summary>
        /// Advance one turn of digging. After 5 turns the tunnel is complete
        /// and raiders can breach.
        /// </summary>
        /// <returns>True if the tunnel just completed this turn.</returns>
        public bool TickTurn()
        {
            if (_state.tunnelComplete) return false;

            _state.turnsDigging++;

            if (_state.turnsDigging >= TurnsToDig)
            {
                _state.tunnelComplete = true;
                _state.surfaceDefensesBypassed = true;
                OnTunnelComplete?.Invoke();
                return true;
            }

            return false;
        }

        /// <summary>True when the tunnel is complete and raiders can breach.</summary>
        public bool IsTunnelComplete()
        {
            return _state.tunnelComplete;
        }

        /// <summary>
        /// Raiders breach through the floor into the airlock, bypassing
        /// all surface defenses.
        /// </summary>
        public void TriggerBreach()
        {
            if (!_state.tunnelComplete) return;

            _state.surfaceDefensesBypassed = true;
            OnBreachTriggered?.Invoke(_state.breachLocation);
        }

        /// <summary>
        /// Returns the breach location identifier.
        /// Always "airlock_floor" — sappers come up from below.
        /// </summary>
        public string GetBreachLocation()
        {
            return _state.breachLocation;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public SiegeSappersState CaptureState()
        {
            return new SiegeSappersState
            {
                siegeId = _state.siegeId,
                turnsDigging = _state.turnsDigging,
                tunnelComplete = _state.tunnelComplete,
                breachLocation = _state.breachLocation,
                surfaceDefensesBypassed = _state.surfaceDefensesBypassed
            };
        }

        public void RestoreState(SiegeSappersState saved)
        {
            _state = saved ?? new SiegeSappersState();
        }
    }
}
