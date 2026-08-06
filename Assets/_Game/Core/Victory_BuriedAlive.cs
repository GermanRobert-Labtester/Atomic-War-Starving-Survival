using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Buried Alive / Turtle Ending (#761).
    /// Seal the vault door and detonate the stairwell. Completely cut off from the surface.
    /// With Hydroponics + Geothermal power, survivors can endure indefinitely —
    /// but the world above will forget they ever existed.
    /// </summary>
    [Serializable]
    public class BuriedAliveState
    {
        public string victoryId = "victory_buried_alive";
        public bool requiresHydroponics = true;
        public bool requiresGeothermal = true;
        public bool isSealed;
        public bool stairwellDetonated;
        public bool hasHydroponics;
        public bool hasGeothermal;
        public bool triggered;
        public bool slowDeath;
    }

    public class Victory_BuriedAlive
    {
        public event Action OnEndingTriggered;
        public event Action OnStairwellDetonated;

        public BuriedAliveState State { get; private set; }

        public Victory_BuriedAlive()
        {
            State = new BuriedAliveState();
        }

        public Victory_BuriedAlive(BuriedAliveState state)
        {
            State = state ?? new BuriedAliveState();
        }

        /// <summary>
        /// Seals the vault door and detonates the stairwell, cutting off all surface access.
        /// If both Hydroponics and Geothermal are present, the bunker can survive indefinitely.
        /// Otherwise the survivors face a slow death underground.
        /// </summary>
        /// <param name="hasHydroponics">Whether the bunker has a working hydroponics bay.</param>
        /// <param name="hasGeothermal">Whether the bunker has a working geothermal generator.</param>
        /// <returns>True if the ending triggers (both systems present and seal succeeds).</returns>
        public bool SealAndDetonate(bool hasHydroponics, bool hasGeothermal)
        {
            State.hasHydroponics = hasHydroponics;
            State.hasGeothermal = hasGeothermal;
            State.isSealed = true;
            State.stairwellDetonated = true;

            OnStairwellDetonated?.Invoke();

            if (hasHydroponics && hasGeothermal)
            {
                State.triggered = true;
                State.slowDeath = false;
                OnEndingTriggered?.Invoke();
                return true;
            }

            // Without both systems the bunker is sealed but unsustainable.
            State.slowDeath = true;
            return false;
        }

        /// <summary>
        /// Whether the sealed bunker can sustain life indefinitely.
        /// </summary>
        public bool CanSurviveIndefinitely()
        {
            return State.isSealed &&
                   State.hasHydroponics &&
                   State.hasGeothermal;
        }

        /// <summary>
        /// Returns the isolation epilogue text for the Buried Alive ending.
        /// </summary>
        public string GetEndingText()
        {
            if (State.slowDeath)
            {
                return
                    "The stairwell collapsed in a cloud of dust and rubble. " +
                    "The vault door will never open again.\n\n" +
                    "Without hydroponics, the food ran out in weeks. " +
                    "Without geothermal, the cold crept in like a patient animal.\n\n" +
                    "The survivors huddled together in the dark, " +
                    "listening to the silence where the world used to be.\n\n" +
                    "No one came. No one could.\n\n" +
                    "— ENDING: BURIED ALIVE (SLOW DEATH) —";
            }

            return
                "The stairwell collapsed in a cloud of dust and rubble. " +
                "The vault door will never open again.\n\n" +
                "But deep beneath the poisoned earth, the hydroponics hum. " +
                "The geothermal turbine turns. Water cycles. Lights glow.\n\n" +
                "The survivors tend their gardens in the artificial light, " +
                "growing food no sun will ever touch.\n\n" +
                "Above them, seasons pass unmarked. " +
                "Snow falls on a world that has forgotten their names.\n\n" +
                "They are alive. They will endure. " +
                "But they will never see the sky again.\n\n" +
                "— ENDING: BURIED ALIVE —";
        }
    }
}
