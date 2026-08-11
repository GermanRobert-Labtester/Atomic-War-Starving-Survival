using System;

namespace AtomicWar._Game.Narrative
{
    [Serializable]
    public class GreatFamineState
    {
        public string eventId = "world_event_great_famine";
        public int triggerDay = 80;
        public bool isActive = false;
        public bool foodLootTableZeroed = false;
        public bool hydroponicsViable = false;
        public bool cannibalismAvailable = false;
    }

    /// <summary>
    /// Prompt #567: Crisis — The Great Famine.
    /// On Day 80 a blight destroys all remaining pre-war food on the map.
    /// Loot tables for food are globally set to 0. The only survival paths are
    /// internal Hydroponics or Cannibalism. Save/load safe. Plain C#.
    /// </summary>
    public class WorldEvent_GreatFamine
    {
        private GreatFamineState _state = new GreatFamineState();

        // -- Events --
        public event Action<GreatFamineState> OnFamineTriggered;
        public event Action<GreatFamineState> OnFoodLootZeroed;
        public event Action<GreatFamineState> OnHydroponicsSurvival;
        public event Action<GreatFamineState> OnCannibalismOption;

        public GreatFamineState State => _state;

        /// <summary>
        /// Checks whether the Great Famine should activate. Activates on the
        /// configured trigger day if not already active.
        /// </summary>
        public void CheckActivation(int currentDay)
        {
            if (_state.isActive) return;

            if (currentDay >= _state.triggerDay)
            {
                _state.isActive = true;
                _state.foodLootTableZeroed = true;

                OnFamineTriggered?.Invoke(_state);
                OnFoodLootZeroed?.Invoke(_state);
            }
        }

        /// <summary>
        /// Returns the food loot multiplier. 0f when the famine is active
        /// (all food loot tables zeroed), 1f otherwise.
        /// </summary>
        public float GetFoodLootMultiplier()
        {
            return _state.isActive && _state.foodLootTableZeroed ? 0f : 1f;
        }

        /// <summary>
        /// Determines whether the player can survive via Hydroponics.
        /// Requires an installed Hydroponics module, water, and seeds.
        /// </summary>
        public bool CanSurviveViaHydroponics(
            bool hasHydroponicsModule,
            bool hasWater,
            bool hasSeeds)
        {
            bool viable = hasHydroponicsModule && hasWater && hasSeeds;
            _state.hydroponicsViable = viable;

            if (viable)
                OnHydroponicsSurvival?.Invoke(_state);

            return viable;
        }

        /// <summary>
        /// Returns true if corpses are available for cannibalism.
        /// Updates the state flag and fires the event when the option becomes available.
        /// </summary>
        public bool IsCannibalismAvailable(int corpseCount)
        {
            bool available = corpseCount > 0;
            bool wasAvailable = _state.cannibalismAvailable;
            _state.cannibalismAvailable = available;

            if (available && !wasAvailable)
                OnCannibalismOption?.Invoke(_state);

            return available;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public GreatFamineState GetState() => _state;

        // ── Save / Load ────────────────────────────────────────────────


        public GreatFamineState CaptureState() => _state;



        public void RestoreState(GreatFamineState state)
        {
            _state = state ?? new GreatFamineState();
        }

}
}
