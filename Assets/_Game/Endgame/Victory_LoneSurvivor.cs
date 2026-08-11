using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Endgame
{
    /// <summary>
    /// Lone Survivor Ending (#759).
    /// Triggered when only one survivor remains after Day 100.
    /// The survivor packs a bag, opens the hatch, and walks into the wasteland alone.
    /// </summary>
    [Serializable]
    public class LoneSurvivorState
    {
        public string victoryId = "victory_lone_survivor";
        public int minDayRequired = 100;
        public int survivorCountRequired = 1;
        public bool triggered;
    }

    public class Victory_LoneSurvivor
    {
        public event Action<string> OnEndingTriggered;

        public LoneSurvivorState State { get; private set; }

        public Victory_LoneSurvivor()
        {
            State = new LoneSurvivorState();
        }

        public Victory_LoneSurvivor(LoneSurvivorState state)
        {
            State = state ?? new LoneSurvivorState();
        }

        /// <summary>
        /// Checks whether the lone-survivor victory condition is met.
        /// </summary>
        /// <param name="currentDay">Current in-game day.</param>
        /// <param name="aliveSurvivorCount">Number of living survivors.</param>
        /// <returns>True if the ending is triggered.</returns>
        public bool CheckVictory(int currentDay, int aliveSurvivorCount)
        {
            if (State.triggered) return true;

            if (currentDay >= State.minDayRequired &&
                aliveSurvivorCount == State.survivorCountRequired)
            {
                State.triggered = true;
                // survivorId is unknown here; caller passes name via GetEndingText.
                OnEndingTriggered?.Invoke("lone_survivor");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the melancholic epilogue text for the lone survivor ending.
        /// </summary>
        /// <param name="survivorName">Display name of the surviving character.</param>
        /// <returns>Multi-line epilogue string.</returns>
        public string GetEndingText(string survivorName)
        {
            return
                $"Day {State.minDayRequired}. The bunker fell silent weeks ago.\n\n" +
                $"{survivorName} packed what little remained — a dented canteen, " +
                "three iodine pills, a photograph no one would ever see again.\n\n" +
                "The hatch groaned open for the last time. " +
                "Grey light spilled across the concrete floor like an apology.\n\n" +
                $"{survivorName} did not look back. There was nothing left to look at.\n\n" +
                "Somewhere beyond the ash, the wind still blew. " +
                "Somewhere, someone might still be alive.\n\n" +
                $"But {survivorName} would never know. " +
                "The wasteland does not answer questions.\n\n" +
                "— ENDING: LONE SURVIVOR —";
        }

        // ── Save / Load ────────────────────────────────────────────────

        public LoneSurvivorState CaptureState()
        {
            return new LoneSurvivorState
            {
                victoryId = State.victoryId,
                minDayRequired = State.minDayRequired,
                survivorCountRequired = State.survivorCountRequired,
                triggered = State.triggered,
            };
        }

        public void RestoreState(LoneSurvivorState state)
        {
            if (state == null)
            {
                State = new LoneSurvivorState();
                return;
            }
            State = new LoneSurvivorState
            {
                victoryId = state.victoryId,
                minDayRequired = state.minDayRequired,
                survivorCountRequired = state.survivorCountRequired,
                triggered = state.triggered,
            };
        }
    }
}
