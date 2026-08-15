using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using Ashfall.Core.Journal;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Prompt #47 — expedition outcomes driven by radio intel reliability.
    /// Factories for the Safe Haven location-bound ambush (Unverified send)
    /// and empty-cache discovery (analyzed Trap). Both gate on
    /// <see cref="EventRunner.SafeHavenTargetLocationId"/>.
    /// </summary>
    public static class SafeHavenEncounters
    {
        public const string EmptyCacheEncounterId = "enc_safe_haven_empty_cache";

        /// <summary>
        /// Sniper ambush — injected when the player sends an expedition on
        /// Unverified Safe Haven intel. forceOnArrival guarantees the beat
        /// fires the first time the team reaches grid 4-7-North.
        /// </summary>
        public static EncounterSO CreateAmbush()
        {
            var ambush = ScriptableObject.CreateInstance<EncounterSO>();
            ambush.id = EventRunner.SafeHavenAmbushEncounterId;
            ambush.title = "Safe Haven Sniper Ambush";
            ambush.description =
                "The cache is not a cache. The 'bunker entrance' is a firing position. " +
                "A single high-caliber round takes the first survivor in the chest before the rest " +
                "even hear the shot. The loop is still playing. There was never anyone in the bunker.";
            ambush.category = EncounterCategory.Combat;
            ambush.baseWeight = 5f;
            ambush.stealthWeightMultiplier = 1.2f;
            ambush.speedWeightMultiplier = 1.4f;
            ambush.minDangerLevel = 0f;
            ambush.requiredLocationId = EventRunner.SafeHavenTargetLocationId;
            ambush.forceOnArrival = true;
            ambush.enableAutoResolution = false;
            ambush.autoEngageTrait = RiskBiasTrait.Reckless;
            ambush.autoFleeTrait = RiskBiasTrait.Paranoid;
            ambush.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "drag_wounded_back",
                    Text = "Drag the wounded back. Leave the cache.",
                    MoraleDelta = -20f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { TargetNeed = "health", NeedDelta = -35f }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "suppress_and_advance",
                    Text = "Pin down the shooter and push forward.",
                    MoraleDelta = -8f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { TargetNeed = "health", NeedDelta = -25f }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "abort_expedition",
                    Text = "Abort. Run.",
                    MoraleDelta = -5f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { TargetNeed = "fatigue", NeedDelta = 10f }
                    }
                }
            };
            return ambush;
        }

        /// <summary>
        /// Empty cache — injected when the player analyzed the loop first
        /// (Trap confirmed). They earn a cold discovery, not a sniper.
        /// </summary>
        public static EncounterSO CreateEmptyCache()
        {
            var cache = ScriptableObject.CreateInstance<EncounterSO>();
            cache.id = EmptyCacheEncounterId;
            cache.title = "Empty Cache";
            cache.description =
                "The grid coordinates are real. The bunker entrance is real. " +
                "Inside: dust, a broken cot, a tin cup with a dead fly. " +
                "Someone broadcast a recording of a place that has been dead for months. " +
                "You leave with less hope than you brought.";
            cache.category = EncounterCategory.Discovery;
            cache.baseWeight = 5f;
            cache.stealthWeightMultiplier = 1f;
            cache.speedWeightMultiplier = 1f;
            cache.minDangerLevel = 0f;
            cache.requiredLocationId = EventRunner.SafeHavenTargetLocationId;
            cache.forceOnArrival = true;
            cache.enableAutoResolution = false;
            cache.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "search_anyway",
                    Text = "Search the corners. Leave nothing.",
                    MoraleDelta = -8f
                },
                new EventChoice
                {
                    ChoiceId = "turn_back",
                    Text = "Turn back. The loop was a lie.",
                    MoraleDelta = -4f
                }
            };
            return cache;
        }
    }
}
