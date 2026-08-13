using System;
using System.Collections.Generic;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using Random = System.Random;
using Ashfall.Core;
using Ashfall.Core.Journal;

namespace AtomicWar._Game.Events
{
    /// <summary>
    /// Evaluation context passed to EventRunner containing time of day, weather,
    /// survivor state, shelter status, inventory items, world flags, and RNG.
    /// </summary>
    public class EventContext
    {
        public const float MedicalSkillTraitThreshold = 0.5f;
        /// <summary>Science/technical threshold for the "Science" trait gate
        /// (mirrors <see cref="MedicalSkillTraitThreshold"/>). Used by radio-triggered
        /// events where the bunker needs a tech at the dial to scrutinize a broadcast.</summary>
        public const float ScienceSkillTraitThreshold = 0.5f;

        /// <summary>
        /// Reliability rating for the intel currently driving an event. The radio
        /// airwaves are full of desperate liars: an Unverified broadcast is just
        /// what was said; Verified has been corroborated (multiple frequencies,
        /// post-broadcast confirmation, or a survivor actually walked the
        /// route); a Trap is a broadcast engineered to lure a response (a
        /// pre-positioned ambush, a recorded loop, a poisoned cache).
        /// GameEvents driven by radio intel inherit the reliability of the
        /// broadcast that triggered them; choices that send an expedition or
        /// trust the broadcast should branch on this value.
        /// </summary>
        public IntelReliability ActiveIntelReliability = IntelReliability.Unverified;

        /// <summary>
        /// True while a survivor is actively listening to or tuning the radio.
        /// Gates radio-triggered events (Prompt #46): the player must be at the
        /// radio for the broadcast to reach them as an interactive choice.
        /// Set by the ListenToRadio AI action / player UI; cleared when the
        /// survivor detunes or leaves the radio module.
        /// </summary>
        public bool IsOnRadio;

        public int CurrentDay = 1;
        public float CurrentHour = 12f;
        public bool IsFalloutStorm;
        /// <summary>Live weather kind for EventConditions.RequireBlizzard / RequireExtremeWeather.</summary>
        public WeatherKind CurrentWeather = WeatherKind.Clear;
        /// <summary>True when CurrentWeather is Blizzard (Prompt #48).</summary>
        public bool IsBlizzard => CurrentWeather == WeatherKind.Blizzard;
        /// <summary>Blizzard or FalloutStorm — hatch-entrapment weather gate.</summary>
        public bool IsExtremeWeather =>
            CurrentWeather == WeatherKind.Blizzard
            || CurrentWeather == WeatherKind.FalloutStorm
            || IsFalloutStorm;
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

        /// <summary>Optional — when set, health/dose deltas use death-safe / event-raising paths.</summary>
        public NeedsSystem NeedsSystem;
        /// <summary>Optional — when set, radiation deltas use <see cref="RadiationSystem"/>.</summary>
        public RadiationSystem RadiationSystem;

        /// <summary>
        /// Faction trust lookup (factionId → -100..100). Wired from DynamicEconomySystem
        /// so Events assembly stays free of an Economy reference.
        /// </summary>
        public Func<string, float> GetFactionTrust;

        /// <summary>
        /// Optional push when an eventFlag is set (SaveSystem.SetWorldFlag).
        /// </summary>
        public Action<string, bool> OnEventFlagChanged;

        /// <summary>
        /// True when food or water stock is below 10% of inventory capacity.
        /// Set by SuspicionTracker / GameBootstrap before event evaluation.
        /// </summary>
        public bool IsResourceStarved;

        /// <summary>Optional POV / player survivor id — excluded from mystery suspect pool.</summary>
        public string PlayerSurvivorId;

        /// <summary>Live suspicion / internal-mystery state (optional).</summary>
        public SuspicionTracker Suspicion;

        /// <summary>
        /// Optional located-knowledge base (JournalSystem). Enables knowledge-gated
        /// events via <see cref="EventConditions.RequiredKnowledgeKey"/>.
        /// </summary>
        public KnowledgeBase Knowledge;

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
            if (string.Equals(t, "Science", StringComparison.OrdinalIgnoreCase)
                || string.Equals(t, "Tech", StringComparison.OrdinalIgnoreCase)
                || string.Equals(t, "Scientist", StringComparison.OrdinalIgnoreCase))
            {
                return survivor.ScienceSkill >= ScienceSkillTraitThreshold;
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
