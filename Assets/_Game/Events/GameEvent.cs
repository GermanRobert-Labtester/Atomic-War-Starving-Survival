using System;
using System.Collections.Generic;
using UnityEngine;

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
    }

    [Serializable]
    public class DelayedConsequence
    {
        public float DelayHours = 24f;
        public string Title;
        [TextArea(2, 4)] public string Description;
        public List<EventEffect> Effects = new List<EventEffect>();
    }

    [Serializable]
    public class EventChoice
    {
        public string ChoiceId;
        public string Text;
        public float MoraleDelta;
        public float RelationshipDelta;
        public List<EventEffect> Effects = new List<EventEffect>();
        public DelayedConsequence DelayedConsequence;
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
