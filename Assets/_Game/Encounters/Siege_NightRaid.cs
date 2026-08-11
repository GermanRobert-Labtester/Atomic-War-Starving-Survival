using System;

namespace AtomicWar._Game.Encounters
{
    [Serializable]
    public class SiegeNightRaidState
    {
        public string siegeId = "siege_night_raid";
        public bool powerCut;
        public int turnsInDark;
        public bool turretsDisabled;
        public float flashlightBatteries;
    }

    /// <summary>
    /// Prompt #825: Night Raid. Raiders cut exterior power lines. The bunker
    /// goes pitch black. Turrets are disabled. Combat is restricted to
    /// Flashlight cones and Melee. Claustrophobic terror.
    /// Plain C#. Save/load safe.
    /// </summary>
    public class Siege_NightRaid
    {
        private SiegeNightRaidState _state = new SiegeNightRaidState();

        private const float BatteryDrainPerTurn = 10f;
        private const int TurnsToRestorePower = 2;

        // -- Events --
        public event Action OnPowerCut;
        public event Action<int> OnTurnInDark;          // turn number
        public event Action OnTurretsDisabled;
        public event Action OnFlashlightCombat;
        public event Action OnMeleeOnly;
        public event Action OnPowerRestored;

        public SiegeNightRaidState State => _state;

        /// <summary>
        /// Raiders cut the exterior power lines. All powered modules go
        /// offline. Turrets can't fire.
        /// </summary>
        public void CutPower()
        {
            _state.powerCut = true;
            _state.turnsInDark = 0;
            _state.turretsDisabled = true;

            OnPowerCut?.Invoke();
            OnTurretsDisabled?.Invoke();
        }

        /// <summary>
        /// Advance one turn in the dark. Drains flashlight batteries.
        /// </summary>
        public void TickTurn()
        {
            if (!_state.powerCut) return;

            _state.turnsInDark++;

            // Drain flashlight batteries
            _state.flashlightBatteries = Math.Max(0f, _state.flashlightBatteries - BatteryDrainPerTurn);

            OnTurnInDark?.Invoke(_state.turnsInDark);
        }

        /// <summary>
        /// Check whether a flashlight has enough charge to aim a shot.
        /// </summary>
        /// <param name="batteryLevel">Battery level of the flashlight.</param>
        /// <returns>True if the flashlight can be used for aimed combat.</returns>
        public bool HasFlashlight(float batteryLevel)
        {
            return batteryLevel > 0f && _state.powerCut;
        }

        /// <summary>
        /// Resolve combat during the blackout. Flashlight-aimed shots are
        /// possible if batteries remain; otherwise only melee works.
        /// </summary>
        /// <param name="hasFlashlight">Whether the defender has a working flashlight.</param>
        /// <param name="usesMelee">Whether the defender resorts to melee.</param>
        /// <returns>
        /// "flashlight" if ranged combat was possible, "melee" if only melee
        /// was available, or "none" if the siege is not active.
        /// </returns>
        public string ResolveCombat(bool hasFlashlight, bool usesMelee)
        {
            if (!_state.powerCut) return "none";

            if (hasFlashlight && _state.flashlightBatteries > 0f)
            {
                OnFlashlightCombat?.Invoke();
                return "flashlight";
            }

            if (usesMelee)
            {
                OnMeleeOnly?.Invoke();
                return "melee";
            }

            // No flashlight, no melee — desperate situation
            OnMeleeOnly?.Invoke();
            return "melee";
        }

        /// <summary>
        /// Restore power by sending someone to the generator room.
        /// Takes 2 turns (caller must invoke TickTurn twice after this
        /// before calling RestorePower to finalize).
        /// Alternatively, call directly when the 2 turns have elapsed.
        /// </summary>
        public void RestorePower()
        {
            if (!_state.powerCut) return;

            _state.powerCut = false;
            _state.turretsDisabled = false;

            OnPowerRestored?.Invoke();
        }

        /// <summary>
        /// Set the current flashlight battery level (for save/load or
        /// when batteries are swapped).
        /// </summary>
        public void SetFlashlightBattery(float level)
        {
            _state.flashlightBatteries = Math.Max(0f, level);
        }

        /// <summary>Number of turns required to restore power via generator room.</summary>
        public int TurnsToRestore => TurnsToRestorePower;

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public SiegeNightRaidState CaptureState()
        {
            return new SiegeNightRaidState
            {
                siegeId = _state.siegeId,
                powerCut = _state.powerCut,
                turnsInDark = _state.turnsInDark,
                turretsDisabled = _state.turretsDisabled,
                flashlightBatteries = _state.flashlightBatteries
            };
        }

        public void RestoreState(SiegeNightRaidState saved)
        {
            _state = saved ?? new SiegeNightRaidState();
        }
    }
}
