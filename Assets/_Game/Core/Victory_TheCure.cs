using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// The Cure / Science Victory (#762).
    /// A surviving Microbiologist synthesises a retro-virus that consumes ambient radiation.
    /// The formula is broadcast over the HamRadio, and a global alliance begins to form
    /// from the scattered remnants of civilisation.
    /// </summary>
    [Serializable]
    public class TheCureState
    {
        public string victoryId = "victory_the_cure";
        public bool requiresMicrobiologist = true;
        public bool requiresHamRadio = true;
        public bool requiresRareChemicals = true;
        public bool formulaBroadcast;
        public bool triggered;
    }

    public class Victory_TheCure
    {
        public event Action OnEndingTriggered;
        public event Action OnFormulaBroadcast;

        public TheCureState State { get; private set; }

        public Victory_TheCure()
        {
            State = new TheCureState();
        }

        public Victory_TheCure(TheCureState state)
        {
            State = state ?? new TheCureState();
        }

        /// <summary>
        /// Checks whether the Cure victory condition is met.
        /// Requires a living Microbiologist, a working HamRadio, and rare chemicals.
        /// If all three are present the formula is broadcast and the ending triggers.
        /// </summary>
        /// <param name="hasMicrobiologist">Whether a Microbiologist survivor is alive.</param>
        /// <param name="hasHamRadio">Whether a functional HamRadio is available.</param>
        /// <param name="hasRareChemicals">Whether the required rare chemicals have been gathered.</param>
        /// <returns>True if the ending is triggered.</returns>
        public bool CheckVictory(bool hasMicrobiologist, bool hasHamRadio, bool hasRareChemicals)
        {
            if (State.triggered) return true;

            if (hasMicrobiologist && hasHamRadio && hasRareChemicals)
            {
                State.formulaBroadcast = true;
                State.triggered = true;
                OnFormulaBroadcast?.Invoke();
                OnEndingTriggered?.Invoke();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the scientific-salvation epilogue text for The Cure ending.
        /// </summary>
        public string GetEndingText()
        {
            return
                "The retro-virus worked.\n\n" +
                "Dr. Vasquez held the vial up to the bunker's last working light — " +
                "a pale green liquid that shimmered like something alive.\n\n" +
                "It ate radiation. Not slowly. Not partially. Completely.\n\n" +
                "The HamRadio crackled to life at 0300. " +
                "Static, then voices — dozens of them, scattered across frequencies " +
                "no one had monitored in months.\n\n" +
                "A settlement in the Urals. A navy submarine off the coast of nowhere. " +
                "A university basement in what used to be a city.\n\n" +
                "They heard the formula. They understood. " +
                "And for the first time since the bombs fell, " +
                "they had something to do besides survive.\n\n" +
                "The alliance that formed that night had no name. " +
                "It had no flag, no anthem, no borders.\n\n" +
                "It had only a frequency — and a promise.\n\n" +
                "— ENDING: THE CURE —";
        }
    }
}
