using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Cannibal King Ending (#760).
    /// Triggered when the player sustains the bunker on HumanMeat meals and RaiderScrap,
    /// defeats the Warlords, and becomes the new terror of the wasteland.
    /// </summary>
    [Serializable]
    public class CannibalKingState
    {
        public string victoryId = "victory_cannibal_king";
        public int humanMeatMealsThreshold = 50;
        public int raiderScrapUsageThreshold = 30;
        public int humanMeatMealsUsed;
        public int raiderScrapUsed;
        public bool warlordsDefeated;
        public bool triggered;
    }

    public class Victory_CannibalKing
    {
        public event Action OnEndingTriggered;

        public CannibalKingState State { get; private set; }

        public Victory_CannibalKing()
        {
            State = new CannibalKingState();
        }

        public Victory_CannibalKing(CannibalKingState state)
        {
            State = state ?? new CannibalKingState();
        }

        /// <summary>
        /// Tracks cumulative usage of human meat meals and raider scrap.
        /// </summary>
        /// <param name="humanMeatMeals">Total human meat meals consumed so far.</param>
        /// <param name="raiderScrapUsed">Total raider scrap repurposed so far.</param>
        public void TrackUsage(int humanMeatMeals, int raiderScrapUsed)
        {
            State.humanMeatMealsUsed = humanMeatMeals;
            State.raiderScrapUsed = raiderScrapUsed;
        }

        /// <summary>
        /// Marks the Warlords as defeated. Must be called before CheckVictory can succeed.
        /// </summary>
        public void MarkWarlordsDefeated()
        {
            State.warlordsDefeated = true;
        }

        /// <summary>
        /// Checks whether the Cannibal King victory condition is met.
        /// Both resource thresholds and warlord defeat are required.
        /// </summary>
        /// <returns>True if the ending is triggered.</returns>
        public bool CheckVictory()
        {
            if (State.triggered) return true;

            if (State.humanMeatMealsUsed >= State.humanMeatMealsThreshold &&
                State.raiderScrapUsed >= State.raiderScrapUsageThreshold &&
                State.warlordsDefeated)
            {
                State.triggered = true;
                OnEndingTriggered?.Invoke();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the dark epilogue text for the Cannibal King ending.
        /// </summary>
        public string GetEndingText()
        {
            return
                "The Warlords are dead. Their fortresses — silent. " +
                "Their armies — ash on the wind.\n\n" +
                $"You fed your people {State.humanMeatMealsUsed} meals " +
                $"from the bodies of the fallen. " +
                $"You forged {State.raiderScrapUsed} weapons from their scrap.\n\n" +
                "The bunker doors stand open now, but no one dares enter. " +
                "Traders whisper your name and cross to the other side of the road.\n\n" +
                "You didn't save the world. You became its worst nightmare.\n\n" +
                "The wasteland has a new king. " +
                "And the king is always hungry.\n\n" +
                "— ENDING: CANNIBAL KING —";
        }

        // ── Save / Load ────────────────────────────────────────────────

        public CannibalKingState CaptureState()
        {
            return new CannibalKingState
            {
                victoryId = State.victoryId,
                humanMeatMealsThreshold = State.humanMeatMealsThreshold,
                raiderScrapUsageThreshold = State.raiderScrapUsageThreshold,
                humanMeatMealsUsed = State.humanMeatMealsUsed,
                raiderScrapUsed = State.raiderScrapUsed,
                warlordsDefeated = State.warlordsDefeated,
                triggered = State.triggered,
            };
        }

        public void RestoreState(CannibalKingState state)
        {
            if (state == null)
            {
                State = new CannibalKingState();
                return;
            }
            State = new CannibalKingState
            {
                victoryId = state.victoryId,
                humanMeatMealsThreshold = state.humanMeatMealsThreshold,
                raiderScrapUsageThreshold = state.raiderScrapUsageThreshold,
                humanMeatMealsUsed = state.humanMeatMealsUsed,
                raiderScrapUsed = state.raiderScrapUsed,
                warlordsDefeated = state.warlordsDefeated,
                triggered = state.triggered,
            };
        }
    }
}
