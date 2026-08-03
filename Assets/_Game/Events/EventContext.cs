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
        public const float MedicalSkillTraitThreshold = 0.5f;

        public int CurrentDay = 1;
        public float CurrentHour = 12f;
        public bool IsFalloutStorm;
        public Survivor PrimarySurvivor;
        public Shelter.Shelter Shelter;
        public Inventory.Inventory Inventory;
        public Dictionary<string, bool> WorldFlags = new Dictionary<string, bool>();
        public Random Random;

        /// <summary>Diesel CO ppm (Prompt #20) for high_co2 journal discovery.</summary>
        public float CarbonMonoxidePpm;
        /// <summary>Indoor °C for freezing_shelter journal discovery.</summary>
        public float IndoorTemperatureC = 15f;

        // Optional — used by EventEffect with SurvivorBId / FactionId for cross-survivor
        // lookups (Prompt #29: interpersonal affinity matrix). The runner is
        // null-safe if these are not wired.
        public List<Survivor> AllSurvivors;
        public MentalBreakSystem MentalBreak;

        /// <summary>
        /// Faction trust lookup (factionId → -100..100). Wired from DynamicEconomySystem
        /// so Events assembly stays free of an Economy reference.
        /// </summary>
        public Func<string, float> GetFactionTrust;

        /// <summary>
        /// Optional push when an eventFlag is set (SaveSystem.SetWorldFlag).
        /// </summary>
        public Action<string, bool> OnEventFlagChanged;

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
            OnEventFlagChanged?.Invoke(flagId, value);
        }

        /// <summary>Alias for narrative eventFlags (same storage as world flags).</summary>
        public bool HasEventFlag(string flagId) => GetFlag(flagId);

        public void SetEventFlag(string flagId, bool value = true) => SetFlag(flagId, value);

        /// <summary>Active event flag ids (true entries only).</summary>
        public List<string> GetEventFlags()
        {
            var list = new List<string>();
            if (WorldFlags == null) return list;
            foreach (var kv in WorldFlags)
            {
                if (kv.Value && !string.IsNullOrEmpty(kv.Key))
                    list.Add(kv.Key);
            }
            return list;
        }

        public float ResolveFactionTrust(string factionId)
        {
            if (string.IsNullOrEmpty(factionId) || GetFactionTrust == null) return 0f;
            return GetFactionTrust(factionId);
        }

        /// <summary>
        /// True if any living bunker survivor satisfies a trait gate string.
        /// RiskBias names (Paranoid, Reckless, …) or "Medical" (MedicalSkill threshold).
        /// </summary>
        public bool HasTraitInBunker(string requiredTrait)
        {
            if (string.IsNullOrEmpty(requiredTrait)) return true;

            // Prefer full crew; fall back to primary.
            if (AllSurvivors != null)
            {
                for (int i = 0; i < AllSurvivors.Count; i++)
                {
                    if (SurvivorMatchesTrait(AllSurvivors[i], requiredTrait)) return true;
                }
            }
            return SurvivorMatchesTrait(PrimarySurvivor, requiredTrait);
        }

        public static bool SurvivorMatchesTrait(Survivor survivor, string requiredTrait)
        {
            if (survivor == null || !survivor.IsAlive || string.IsNullOrEmpty(requiredTrait))
                return false;

            string t = requiredTrait.Trim();
            if (string.Equals(t, "Medical", StringComparison.OrdinalIgnoreCase)
                || string.Equals(t, "medic", StringComparison.OrdinalIgnoreCase))
            {
                return survivor.MedicalSkill >= MedicalSkillTraitThreshold;
            }

            if (Enum.TryParse(t, ignoreCase: true, out RiskBiasTrait bias))
                return survivor.RiskBias == bias;

            return false;
        }

        /// <summary>Copy true flags from a save-system dictionary into this context.</summary>
        public void ImportFlags(IReadOnlyDictionary<string, bool> flags)
        {
            if (WorldFlags == null) WorldFlags = new Dictionary<string, bool>();
            WorldFlags.Clear();
            if (flags == null) return;
            foreach (var kv in flags)
            {
                if (!string.IsNullOrEmpty(kv.Key))
                    WorldFlags[kv.Key] = kv.Value;
            }
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
