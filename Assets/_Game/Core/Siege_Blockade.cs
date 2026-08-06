using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SiegeBlockadeState
    {
        public string siegeId = "siege_blockade";
        public int daysActive;
        public bool expeditionsLocked;
        public float raiderMorale;
        public float raiderSupplies;
        public float bunkerSupplies;
        public bool raidersLeft;
    }

    /// <summary>
    /// Prompt #824: Starvation Blockade. A massive army camps on the hatch.
    /// Expeditions are hard-locked. The player must survive on internal
    /// stores (Hydroponics, WaterWell) until the raiders starve first.
    /// Raiders carry 40 days of supplies. Each day both sides consume.
    /// If the bunker runs out first — game over.
    /// Plain C#. Save/load safe.
    /// </summary>
    public class Siege_Blockade
    {
        private SiegeBlockadeState _state = new SiegeBlockadeState();

        private const float RaiderStartingSupplies = 40f;
        private const float RaiderDailyConsumption = 1f;

        // -- Events --
        public event Action OnBlockadeStarted;
        public event Action<int, float> OnDayPassed;        // (day, remaining bunker stores)
        public event Action OnRaidersStarved;
        public event Action OnExpeditionsUnlocked;

        public SiegeBlockadeState State => _state;

        /// <summary>
        /// The army arrives and sets up camp on the hatch. Expeditions
        /// are locked immediately.
        /// </summary>
        public void StartBlockade()
        {
            _state.daysActive = 0;
            _state.expeditionsLocked = true;
            _state.raiderMorale = 100f;
            _state.raiderSupplies = RaiderStartingSupplies;
            _state.bunkerSupplies = 0f;
            _state.raidersLeft = false;

            OnBlockadeStarted?.Invoke();
        }

        /// <summary>
        /// Advance one day. Both sides consume supplies.
        /// </summary>
        /// <param name="bunkerFoodPerDay">Bunker food consumed per day.</param>
        /// <param name="bunkerWaterPerDay">Bunker water consumed per day.</param>
        /// <param name="bunkerStores">Current total bunker stores (food + water).</param>
        /// <returns>
        /// 0 = siege ongoing. 1 = raiders starved (they leave). -1 = bunker
        /// ran out first (game over).
        /// </returns>
        public int TickDay(float bunkerFoodPerDay, float bunkerWaterPerDay, float bunkerStores)
        {
            if (_state.raidersLeft) return 1;

            _state.daysActive++;
            _state.bunkerSupplies = bunkerStores;

            // Raider consumption
            _state.raiderSupplies -= RaiderDailyConsumption;
            _state.raiderMorale = Math.Max(0f, _state.raiderMorale - 2.5f);

            // Bunker consumption (external system deducts; we just track)
            _state.bunkerSupplies -= (bunkerFoodPerDay + bunkerWaterPerDay);

            float remaining = Math.Max(0f, _state.bunkerSupplies);
            OnDayPassed?.Invoke(_state.daysActive, remaining);

            // Check raider starvation
            if (_state.raiderSupplies <= 0f)
            {
                _state.raidersLeft = true;
                _state.expeditionsLocked = false;
                OnRaidersStarved?.Invoke();
                OnExpeditionsUnlocked?.Invoke();
                return 1;
            }

            // Check bunker starvation
            if (_state.bunkerSupplies <= 0f)
            {
                return -1;
            }

            return 0;
        }

        /// <summary>True while the blockade is active and no one can leave.</summary>
        public bool IsExpeditionLocked()
        {
            return _state.expeditionsLocked && !_state.raidersLeft;
        }

        /// <summary>True when raiders have run out of supplies and left.</summary>
        public bool HaveRaidersLeft()
        {
            return _state.raidersLeft;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public SiegeBlockadeState CaptureState()
        {
            return new SiegeBlockadeState
            {
                siegeId = _state.siegeId,
                daysActive = _state.daysActive,
                expeditionsLocked = _state.expeditionsLocked,
                raiderMorale = _state.raiderMorale,
                raiderSupplies = _state.raiderSupplies,
                bunkerSupplies = _state.bunkerSupplies,
                raidersLeft = _state.raidersLeft
            };
        }

        public void RestoreState(SiegeBlockadeState saved)
        {
            _state = saved ?? new SiegeBlockadeState();
        }
    }
}
