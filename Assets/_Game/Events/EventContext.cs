using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Inventory;
using Random = System.Random;

namespace AtomicWar._Game.Events
{
    /// <summary>
    /// Evaluation context passed to EventRunner containing time of day, weather,
    /// survivor state, shelter status, inventory items, world flags, and RNG.
    /// </summary>
    public class EventContext
    {
        public int CurrentDay = 1;
        public float CurrentHour = 12f;
        public bool IsFalloutStorm;
        public Survivor PrimarySurvivor;
        public Shelter.Shelter Shelter;
        public Inventory.Inventory Inventory;
        public Dictionary<string, bool> WorldFlags = new Dictionary<string, bool>();
        public Random Random;

        // Optional — used by EventEffect with SurvivorBId / FactionId for cross-survivor
        // lookups (Prompt #29: interpersonal affinity matrix). The runner is
        // null-safe if these are not wired.
        public List<Survivor> AllSurvivors;
        public MentalBreakSystem MentalBreak;

        public EventContext() { }

        public EventContext(Survivor survivor, Shelter.Shelter shelter = null, Inventory.Inventory inventory = null, Random random = null)
        {
            PrimarySurvivor = survivor;
            Shelter = shelter;
            Inventory = inventory;
            Random = random;
        }

        public bool GetFlag(string flagId)
        {
            if (string.IsNullOrEmpty(flagId) || WorldFlags == null) return false;
            return WorldFlags.TryGetValue(flagId, out bool val) && val;
        }

        public void SetFlag(string flagId, bool value)
        {
            if (string.IsNullOrEmpty(flagId)) return;
            if (WorldFlags == null) WorldFlags = new Dictionary<string, bool>();
            WorldFlags[flagId] = value;
        }

        /// <summary>Look up a survivor by id from the AllSurvivors list (null-safe).</summary>
        public Survivor FindSurvivor(string id)
        {
            if (string.IsNullOrEmpty(id) || AllSurvivors == null) return null;
            for (int i = 0; i < AllSurvivors.Count; i++)
            {
                if (AllSurvivors[i] != null && AllSurvivors[i].Id == id) return AllSurvivors[i];
            }
            return null;
        }
    }
}
