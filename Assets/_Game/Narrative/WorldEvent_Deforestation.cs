using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Narrative
{
    [Serializable]
    public class DeforestationState
    {
        public string eventId = "world_event_deforestation";
        public string displayName = "Acid Deforestation";
        public int triggerDay = 90;
        public bool isActive = false;
        public float woodLootMultiplier = 0f;
    }

    /// <summary>
    /// Prompt #654: World Event — Deforestation.
    /// Acid rain combined with Faction logging kills all remaining forests.
    /// All Wood loot drops to 0 globally. WoodStoves and ShoringStruts
    /// can no longer be crafted.
    /// </summary>
    public class WorldEvent_Deforestation
    {
        private DeforestationState _state = new DeforestationState();

        /// <summary>Recipe ids that require wood and become unavailable.</summary>
        private static readonly HashSet<string> WoodRecipes = new HashSet<string>
        {
            "wood_stove",
            "shoring_strut"
        };

        // -- Events --
        public event Action<DeforestationState> OnDeforestationTriggered;
        public event Action<DeforestationState> OnWoodLootZeroed;

        public DeforestationState State => _state;

        /// <summary>
        /// Checks whether the deforestation event should activate.
        /// Activates on the configured trigger day if not already active.
        /// </summary>
        public void CheckActivation(int currentDay)
        {
            if (_state.isActive) return;

            if (currentDay >= _state.triggerDay)
            {
                _state.isActive = true;
                _state.woodLootMultiplier = 0f;

                OnDeforestationTriggered?.Invoke(_state);
                OnWoodLootZeroed?.Invoke(_state);
            }
        }

        /// <summary>
        /// Returns the current wood loot multiplier. 0f when deforestation
        /// is active (all wood loot zeroed), 1f otherwise.
        /// </summary>
        public float GetWoodLootMultiplier()
        {
            return _state.isActive ? _state.woodLootMultiplier : 1f;
        }

        /// <summary>
        /// Returns whether a recipe requiring wood can still be crafted.
        /// Returns false for wood-dependent recipes once deforestation is active.
        /// </summary>
        public bool CanCraftWithWood(string recipeId)
        {
            if (!_state.isActive) return true;
            if (string.IsNullOrEmpty(recipeId)) return true;

            // Block known wood-dependent recipes
            if (WoodRecipes.Contains(recipeId)) return false;

            // Allow non-wood recipes
            return true;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public DeforestationState GetState() => _state;

        // ── Save / Load ────────────────────────────────────────────────


        public DeforestationState CaptureState() => _state;



        public void RestoreState(DeforestationState state)
        {
            _state = state ?? new DeforestationState();
        }

}
}
