using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Events
{
    [Serializable]
    public class EventEffect
    {
        public string TargetNeed; // "hunger", "thirst", "fatigue", "warmth", "morale", "health", "radiation"
        public float NeedDelta;
        public string ItemId;
        public int ItemAmount; // positive = add, negative = remove
        public string SetWorldFlag;
        public bool WorldFlagValue = true;

        /// <summary>Optional faction id for trust matrix deltas (DynamicEconomySystem).</summary>
        public string FactionId;
        /// <summary>Trust change applied when this effect resolves (-100..100 scale).</summary>
        public float TrustDelta;

        // -----------------------------------------------------------------
        // Interpersonal affinity (Prompt #29). When set, ApplyEffect adjusts
        // the survivor-pair affinity by AffinityDelta. SurvivorAId may be
        // left empty to mean "the primary survivor" (the event context).
        // -----------------------------------------------------------------

        /// <summary>First survivor in the affinity pair. Empty = primary survivor.</summary>
        public string SurvivorAId;
        /// <summary>Second survivor in the affinity pair.</summary>
        public string SurvivorBId;
        /// <summary>Affinity change applied to the pair (-100..100 scale).</summary>
        public float AffinityDelta;
    }

    [Serializable]
    public class DelayedConsequence
    {
        public float DelayHours = 24f;
        public string Title;
        [TextArea(2, 4)] public string Description;
        public List<EventEffect> Effects = new List<EventEffect>();
    }

    /// <summary>
    /// Belief-based gate/reweight on an EventChoice, structurally separate from the
    /// objective world-state EventConditions on GameEvent. Lets a Denialist survivor
    /// never even see a "wear the suit" choice offered as costly, or a Paranoid
    /// survivor's "take iodine, just in case" choice get auto-favored.
    /// </summary>
    [Serializable]
    public class BeliefCheck
    {
        /// <summary>Empty = any trait passes.</summary>
        public List<RiskBiasTrait> RequiredTraits = new List<RiskBiasTrait>();
        public float MinPerceivedRadRisk = -1f; // -1 = ignore
        public float MaxPerceivedRadRisk = -1f; // -1 = ignore
        /// <summary>Applied to the choice's effective weight in PickWeightedChoice when the check passes.</summary>
        public float WeightMultiplier = 1f;
        /// <summary>If true and the check fails, the choice is removed from GetAvailableChoices entirely.</summary>
        public bool HideIfFails;
    }

    [Serializable]
    public class EventChoice
    {
        public string ChoiceId;
        public string Text;
        public float MoraleDelta;
        /// <summary>Legacy relationship delta; when FactionId is set, also applied as trust if TrustDelta is 0.</summary>
        public float RelationshipDelta;
        /// <summary>Faction trust matrix target (snake_case id, e.g. scavenger_camp).</summary>
        public string FactionId;
        /// <summary>Trust change applied by DynamicEconomySystem on choice resolve.</summary>
        public float TrustDelta;
        public List<EventEffect> Effects = new List<EventEffect>();
        public DelayedConsequence DelayedConsequence;
        public BeliefCheck BeliefCheck;

        /// <summary>Whether the given survivor's beliefs satisfy this choice's BeliefCheck (true if none set).</summary>
        public bool PassesBeliefCheck(Survivor survivor)
        {
            if (BeliefCheck == null) return true;
            if (survivor == null) return false;

            if (BeliefCheck.RequiredTraits != null && BeliefCheck.RequiredTraits.Count > 0
                && !BeliefCheck.RequiredTraits.Contains(survivor.RiskBias))
            {
                return false;
            }
            if (BeliefCheck.MinPerceivedRadRisk >= 0f && survivor.PerceivedRadRisk < BeliefCheck.MinPerceivedRadRisk)
            {
                return false;
            }
            if (BeliefCheck.MaxPerceivedRadRisk >= 0f && survivor.PerceivedRadRisk > BeliefCheck.MaxPerceivedRadRisk)
            {
                return false;
            }
            return true;
        }
    }

    [Serializable]
    public class EventConditions
    {
        public int MinDay = 1;
        public float MinHour = 0f;
        public float MaxHour = 24f;
        public bool RequireFalloutStorm;
        public float MinShelterAirQuality = -1f;
        public float MaxShelterAirQuality = -1f;
        public float MinSurvivorRad = -1f;
        public float MinSurvivorHunger = -1f;
        public string RequiredItemId;
        public string RequiredFlagId;
    }

    /// <summary>
    /// ScriptableObject definition of a narrative/moral event: identity, selection weight,
    /// gating conditions, choices, explicit effects, and optional delayed consequences.
    /// Data-driven from StreamingAssets/Data/events.json.
    /// </summary>
    [CreateAssetMenu(fileName = "NewGameEvent", menuName = "ASHFALL/Game Event")]
    public class GameEvent : ScriptableObject
    {
        public string id;
        public string title;
        [TextArea(3, 6)] public string bodyText;
        public float weight = 1f;
        public EventConditions conditions = new EventConditions();
        public List<EventChoice> choices = new List<EventChoice>();

        public int minDay
        {
            get => conditions != null ? conditions.MinDay : 1;
            set
            {
                if (conditions == null) conditions = new EventConditions();
                conditions.MinDay = value;
            }
        }

        public virtual bool CanTrigger(EventContext context)
        {
            if (context == null) return false;
            if (conditions == null) return true;

            if (context.CurrentDay < conditions.MinDay) return false;
            if (context.CurrentHour < conditions.MinHour || context.CurrentHour > conditions.MaxHour) return false;
            if (conditions.RequireFalloutStorm && !context.IsFalloutStorm) return false;

            if (conditions.MinShelterAirQuality >= 0f && (context.Shelter == null || context.Shelter.AirQuality < conditions.MinShelterAirQuality))
                return false;
            if (conditions.MaxShelterAirQuality >= 0f && (context.Shelter != null && context.Shelter.AirQuality > conditions.MaxShelterAirQuality))
                return false;

            if (conditions.MinSurvivorRad >= 0f && (context.PrimarySurvivor == null || context.PrimarySurvivor.RadiationDose < conditions.MinSurvivorRad))
                return false;
            if (conditions.MinSurvivorHunger >= 0f && (context.PrimarySurvivor == null || context.PrimarySurvivor.Needs.Hunger < conditions.MinSurvivorHunger))
                return false;

            if (!string.IsNullOrEmpty(conditions.RequiredFlagId) && !context.GetFlag(conditions.RequiredFlagId))
                return false;

            if (!string.IsNullOrEmpty(conditions.RequiredItemId) && (context.Inventory == null || context.Inventory.Count(new Inventory.ItemDefinition { id = conditions.RequiredItemId }) <= 0))
                return false;

            return true;
        }

        public virtual void Apply()
        {
            // Default apply fallback
        }
    }
}
