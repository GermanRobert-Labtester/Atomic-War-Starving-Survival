using System;
using System.Collections.Generic;
using AtomicWar._Game.Events;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// The outcome when a player expedition arrives at the stranger_cache location.
    /// </summary>
    public enum StrangerCacheOutcome
    {
        Unknown,
        /// <summary>Clean water was given: the cache is real and fully stocked.</summary>
        RealCache,
        /// <summary>Irradiated water was given: the coordinates lead to a faction ambush.</summary>
        FactionAmbush
    }

    /// <summary>
    /// Pure C# system for resolving the flag-dependent Part 3 outcome of the
    /// "Silent Knock" deferred narrative chain (Prompt #43).
    ///
    /// Reads world flags written by the event choices and determines whether
    /// the stranger_cache expedition yields a real supply cache or a faction ambush.
    /// </summary>
    public static class NarrativeChainEngine
    {
        // World flags written by Silent Knock events.json choices
        public const string FlagStrangerInside            = "stranger_inside";
        public const string FlagStrangerIgnored           = "stranger_ignored";
        public const string FlagStrangerHasCoordinates    = "stranger_has_coordinates";
        public const string FlagGivenCleanWater           = "stranger_given_clean_water";
        public const string FlagGivenIrradiatedWater      = "stranger_given_irradiated_water";
        public const string FlagCacheExpeditionLaunched   = "stranger_cache_expedition_launched";
        public const string FlagCacheAbandoned            = "stranger_cache_abandoned";
        public const string FlagStrangerDiedOutside       = "stranger_died_outside";

        // Encounter ids injected into the EncounterPool for Part 3 resolution.
        public const string EncounterIdRealCache   = "stranger_cache_real";
        public const string EncounterIdAmbush      = "stranger_cache_ambush";

        /// <summary>
        /// Evaluate which Part 3 outcome should apply based on current world flags.
        /// Call when the expedition arrives at the stranger_cache location.
        /// </summary>
        public static StrangerCacheOutcome EvaluateOutcome(EventContext context)
        {
            if (context == null) return StrangerCacheOutcome.Unknown;

            // Coordinates must have been shared
            if (!context.GetFlag(FlagStrangerHasCoordinates)) return StrangerCacheOutcome.Unknown;

            if (context.GetFlag(FlagGivenIrradiatedWater)) return StrangerCacheOutcome.FactionAmbush;
            if (context.GetFlag(FlagGivenCleanWater))      return StrangerCacheOutcome.RealCache;

            // Stranger was let in but no water given yet — default to unknown (chain not yet resolved)
            return StrangerCacheOutcome.Unknown;
        }

        /// <summary>
        /// Build the encounter pool entries appropriate for the Part 3 outcome.
        /// Returns an EncounterSO (runtime ScriptableObject) for each outcome path.
        /// </summary>
        public static EncounterSO BuildOutcomeEncounter(StrangerCacheOutcome outcome)
        {
            switch (outcome)
            {
                case StrangerCacheOutcome.RealCache:
                    return BuildEncounter(
                        id: EncounterIdRealCache,
                        title: "The Cache",
                        description: "The locker is exactly where he said it would be. Pre-war medical supplies, sealed rations, two full water purifiers. He told the truth.",
                        category: EncounterCategory.Discovery,
                        baseWeight: 10f,
                        minDangerLevel: 0f,
                        lootChoiceId: "take_supplies",
                        lootChoiceText: "Load up and return.",
                        lootItemId: "canned_food",
                        lootItemAmount: 4
                    );

                case StrangerCacheOutcome.FactionAmbush:
                    return BuildEncounter(
                        id: EncounterIdAmbush,
                        title: "The Ambush",
                        description: "Three of them step out of the shadow of the building. Armed. They were waiting. The cartographer sold you out — maybe he had no choice after the irradiated water.",
                        category: EncounterCategory.Combat,
                        baseWeight: 10f,
                        minDangerLevel: 0f,
                        lootChoiceId: "flee_ambush",
                        lootChoiceText: "Drop everything and run.",
                        lootItemId: null,
                        lootItemAmount: 0
                    );

                default:
                    return null;
            }
        }

        private static EncounterSO BuildEncounter(
            string id,
            string title,
            string description,
            EncounterCategory category,
            float baseWeight,
            float minDangerLevel,
            string lootChoiceId,
            string lootChoiceText,
            string lootItemId,
            int lootItemAmount)
        {
            var encounter = UnityEngine.ScriptableObject.CreateInstance<EncounterSO>();
            encounter.id = id;
            encounter.title = title;
            encounter.description = description;
            encounter.category = category;
            encounter.baseWeight = baseWeight;
            encounter.stealthWeightMultiplier = 1f;
            encounter.speedWeightMultiplier = 1f;
            encounter.minDangerLevel = minDangerLevel;
            encounter.enableAutoResolution = false;

            var choice = new EventChoice
            {
                ChoiceId = lootChoiceId,
                Text = lootChoiceText
            };
            if (!string.IsNullOrEmpty(lootItemId))
            {
                choice.Effects.Add(new EventEffect
                {
                    ItemId = lootItemId,
                    ItemAmount = lootItemAmount
                });
            }
            encounter.choices.Add(choice);
            return encounter;
        }
    }
}
