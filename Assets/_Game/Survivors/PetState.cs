using System;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    [Flags]
    public enum PetTraits
    {
        None = 0,
        RatCatcher = 1 << 0, // Prevents food spoilage events
        Barker = 1 << 1      // Raises ShelterSecurity, lowers SleepQuality
    }

    /// <summary>
    /// AI entity simpler than a survivor: has Needs (Hunger, Thirst, Radiation).
    /// Acts as a high-risk morale battery and a mobile contamination source when fur is dirty.
    /// Save/load safe.
    /// </summary>
    [Serializable]
    public class PetState
    {
        public string Id;
        public string DisplayName;
        public PetTraits Traits = PetTraits.None;

        public float Hunger;    // 0..100 (100 = starved to death)
        public float Thirst;    // 0..100 (100 = died of thirst)
        public float Radiation; // 0..100 (accumulated radiation dose)

        /// <summary>Rads/hr contamination carried on fur after outdoor exposure.</summary>
        public float FurContamination;

        /// <summary>ID of the room currently occupied in the shelter.</summary>
        public string CurrentRoomId;

        public bool IsAlive = true;

        public bool HasTrait(PetTraits trait)
        {
            return (Traits & trait) == trait;
        }
    }
}
