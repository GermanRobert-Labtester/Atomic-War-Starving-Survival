using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// The Martian / Rocket Silo Ending (#763).
    /// Clear the MissileSilo, locate an orbital escape pod, and fuel it with massive
    /// reserves of Fuel and Electronics. Up to 3 survivors launch into orbit —
    /// leaving everything behind.
    /// </summary>
    [Serializable]
    public class TheMartianState
    {
        public float fuelRequired = 100f;
        public int electronicsRequired = 50;
        public int survivorsLaunched;
        public string victoryId = "victory_the_martian";
        public bool siloCleared;
        public bool triggered;
    }

    public class Victory_TheMartian
    {
        public event Action<int> OnEndingTriggered;   // survivorsLaunched
        public event Action OnLaunchSequenceStarted;

        public TheMartianState State { get; private set; }

        public Victory_TheMartian()
        {
            State = new TheMartianState();
        }

        public Victory_TheMartian(TheMartianState state)
        {
            State = state ?? new TheMartianState();
        }

        /// <summary>
        /// Checks whether the Martian victory condition is met.
        /// Requires a cleared silo, 100 units of Fuel, 50 Electronics, and at least 3 survivors.
        /// </summary>
        /// <param name="siloCleared">Whether the MissileSilo has been cleared of debris.</param>
        /// <param name="fuel">Available fuel units.</param>
        /// <param name="electronics">Available electronics units.</param>
        /// <param name="availableSurvivors">Number of living survivors.</param>
        /// <returns>True if the ending is triggered.</returns>
        public bool CheckVictory(bool siloCleared, float fuel, int electronics, int availableSurvivors)
        {
            if (State.triggered) return true;

            State.siloCleared = siloCleared;

            if (siloCleared &&
                fuel >= State.fuelRequired &&
                electronics >= State.electronicsRequired &&
                availableSurvivors >= 3)
            {
                State.triggered = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Initiates the launch sequence and returns the number of survivors who board the pod.
        /// Launches at most 3 survivors (or fewer if not enough are available).
        /// </summary>
        /// <param name="fuel">Fuel consumed for the launch.</param>
        /// <param name="electronics">Electronics consumed for the launch.</param>
        /// <param name="survivorCount">Number of survivors available to launch.</param>
        /// <returns>Number of survivors who launched (min of 3, survivorCount).</returns>
        public int Launch(float fuel, int electronics, int survivorCount)
        {
            OnLaunchSequenceStarted?.Invoke();

            int launched = Mathf.Min(3, survivorCount);
            State.survivorsLaunched = launched;
            State.triggered = true;

            OnEndingTriggered?.Invoke(launched);
            return launched;
        }

        /// <summary>
        /// Returns the orbital-escape epilogue text for The Martian ending.
        /// </summary>
        /// <param name="launched">Number of survivors who made it into orbit.</param>
        public string GetEndingText(int launched)
        {
            string survivorLine = launched == 1
                ? "One soul, strapped into a seat built for three."
                : $"{launched} souls, strapped into seats that were never meant for civilians.";

            return
                "The silo doors ground open — rust and ice falling like confetti.\n\n" +
                "The escape pod sat in the centre of the launch cradle, " +
                "its hull scarred by decades of neglect. " +
                "But the engines turned over. The electronics held.\n\n" +
                survivorLine + "\n\n" +
                "The countdown was silent. No mission control. No cheering crowd. " +
                "Just the hiss of hydraulics and the rumble of igniting fuel.\n\n" +
                "Three minutes later, the Earth was a grey marble below them — " +
                "swirled with ash clouds where continents used to be visible.\n\n" +
                "The orbital station was dark when they docked. " +
                "But the life-support indicators blinked green.\n\n" +
                "They were alive. They were above it all. " +
                "And for now, that was enough.\n\n" +
                "— ENDING: THE MARTIAN —";
        }
    }
}
