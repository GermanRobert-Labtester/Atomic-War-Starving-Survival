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

        // -----------------------------------------------------------------
        // Deferred narrative chain scheduling (Prompt #43 — scheduleEvent).
        // When ScheduleEventId is non-empty, EventRunner enqueues the named
        // GameEvent to fire on ScheduleOnDay. This is how choices in Part 1
        // schedule Part 2 or Part 3 of a multi-stage story arc.
        // -----------------------------------------------------------------

        /// <summary>
        /// snake_case id of the GameEvent to schedule. Leave empty for no scheduling.
        /// Matches an id in the EventRunner pool or authored events.json chain.
        /// </summary>
        public string ScheduleEventId;
        /// <summary>
        /// Absolute campaign day for ScheduleEventId. Ignored when
        /// <see cref="ScheduleDelayDays"/> is &gt; 0.
        /// </summary>
        public int ScheduleOnDay;
        /// <summary>
        /// Relative delay in days from the current context day
        /// (executeOnDay = CurrentDay + ScheduleDelayDays). Preferred for
        /// dynamic arcs like emissary aftermath. When &gt; 0, overrides ScheduleOnDay.
        /// </summary>
        public int ScheduleDelayDays;
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

        // -----------------------------------------------------------------
        // Trait / trust / eventFlag gates (dynamic choice list)
        // -----------------------------------------------------------------

        /// <summary>
        /// Trait gate: RiskBias name (Paranoid, Reckless, …) or "Medical".
        /// Empty = no trait requirement. Evaluated against anyone in the bunker.
        /// </summary>
        public string RequiredTrait;

        /// <summary>Faction id for trust gate (snake_case, e.g. scavenger_camp).</summary>
        public string RequiredTrustFactionId;

        /// <summary>
        /// If set (not NaN), trust must be ≥ this value.
        /// Example: ScavengerCamp &gt; 20 → RequiredTrustMin = 20.001 or use exclusive helpers.
        /// </summary>
        public float RequiredTrustMin = float.NaN;

        /// <summary>
        /// If set (not NaN), trust must be &lt; this value (strict).
        /// Example: threatening emissary → RequiredTrustMaxExclusive = -20.
        /// </summary>
        public float RequiredTrustMaxExclusive = float.NaN;

        /// <summary>Event flags that must already be true (prerequisites).</summary>
        public List<string> RequiredEventFlags = new List<string>();

        /// <summary>
        /// Item id (snake_case) that must be present in the bunker inventory
        /// for this choice to be available. Used by radio-triggered events
        /// where the player must own a <c>radio_transmitter</c> to broadcast
        /// a warning, or a <c>geiger_counter</c> to verify a hot-zone claim.
        /// Evaluated alongside trait/trust gates; gated with the same
        /// <see cref="HideIfGatesFail"/> semantics.
        /// </summary>
        public string RequiredItemId;

        /// <summary>Flags injected into SaveSystem / EventContext when this choice resolves.</summary>
        public List<string> SetEventFlags = new List<string>();

        /// <summary>When true (default), failed gates hide the choice. When false, gray it out.</summary>
        public bool HideIfGatesFail = true;

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

        /// <summary>Trait + trust + eventFlag gates (BeliefCheck separate).</summary>
        public bool PassesTraitAndTrustGates(EventContext context)
        {
            if (!string.IsNullOrEmpty(RequiredTrait))
            {
                if (context == null || !context.HasTraitInBunker(RequiredTrait))
                    return false;
            }

            if (!string.IsNullOrEmpty(RequiredTrustFactionId) && context != null)
            {
                float trust = context.ResolveFactionTrust(RequiredTrustFactionId);
                if (!float.IsNaN(RequiredTrustMin) && trust < RequiredTrustMin)
                    return false;
                if (!float.IsNaN(RequiredTrustMaxExclusive) && trust >= RequiredTrustMaxExclusive)
                    return false;
            }
            else if ((!float.IsNaN(RequiredTrustMin) || !float.IsNaN(RequiredTrustMaxExclusive))
                     && context == null)
            {
                return false;
            }

            if (RequiredEventFlags != null && RequiredEventFlags.Count > 0)
            {
                if (context == null) return false;
                for (int i = 0; i < RequiredEventFlags.Count; i++)
                {
                    string f = RequiredEventFlags[i];
                    if (string.IsNullOrEmpty(f)) continue;
                    if (!context.HasEventFlag(f)) return false;
                }
            }

            // Inventory item gate (Prompt #46). When RequiredItemId is set
            // the bunker must hold at least one of the item; an empty
            // context is treated as "not in inventory" and fails the gate.
            if (!string.IsNullOrEmpty(RequiredItemId))
            {
                if (context == null || context.Inventory == null) return false;
                if (context.Inventory.CountById(RequiredItemId) <= 0) return false;
            }

            return true;
        }

        /// <summary>All gates including BeliefCheck (primary survivor for belief).</summary>
        public bool PassesAllGates(EventContext context)
        {
            if (!PassesTraitAndTrustGates(context)) return false;
            if (BeliefCheck != null && BeliefCheck.HideIfFails
                && !PassesBeliefCheck(context?.PrimarySurvivor))
                return false;
            // Soft belief check without HideIfFails still passes presentation;
            // weighted pick handles reweight separately.
            return true;
        }
    }

    /// <summary>UI-facing view of a choice after gate evaluation.</summary>
    [Serializable]
    public class PresentedEventChoice
    {
        public EventChoice Choice;
        public bool IsAvailable;
        public bool IsGrayedOut;
        public bool IsHidden;
        public string GateFailReason;

        public string ChoiceId => Choice != null ? Choice.ChoiceId : string.Empty;
        public string Text => Choice != null ? Choice.Text : string.Empty;
    }

    [Serializable]
    public class EventConditions
    {
        public int MinDay = 1;
        public float MinHour = 0f;
        public float MaxHour = 24f;
        public bool RequireFalloutStorm;
        /// <summary>When true, event only fires during a Blizzard (Prompt #48).</summary>
        public bool RequireBlizzard;
        /// <summary>
        /// When true, event only fires during Blizzard OR FalloutStorm
        /// (Prompt #48 — weather-driven hatch entrapment gates).
        /// </summary>
        public bool RequireExtremeWeather;
        public float MinShelterAirQuality = -1f;
        public float MaxShelterAirQuality = -1f;
        public float MinSurvivorRad = -1f;
        public float MinSurvivorHunger = -1f;
        public string RequiredItemId;
        public string RequiredFlagId;
        /// <summary>All listed eventFlags must be set for the event to fire.</summary>
        public List<string> RequiredEventFlags = new List<string>();
        /// <summary>
        /// When true, event only fires if food or water stock is below 10% of inventory capacity
        /// (see <see cref="EventContext.IsResourceStarved"/> / SuspicionTracker).
        /// </summary>
        public bool RequireResourceStarved;
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
            if (conditions.RequireBlizzard && !context.IsBlizzard) return false;
            if (conditions.RequireExtremeWeather && !context.IsExtremeWeather) return false;

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

            if (conditions.RequiredEventFlags != null)
            {
                for (int i = 0; i < conditions.RequiredEventFlags.Count; i++)
                {
                    string f = conditions.RequiredEventFlags[i];
                    if (string.IsNullOrEmpty(f)) continue;
                    if (!context.GetFlag(f)) return false;
                }
            }

            if (!string.IsNullOrEmpty(conditions.RequiredItemId) && (context.Inventory == null || context.Inventory.Count(new Inventory.ItemDefinition { id = conditions.RequiredItemId }) <= 0))
                return false;

            if (conditions.RequireResourceStarved && !context.IsResourceStarved)
                return false;

            return true;
        }

        /// <summary>
        /// Body text that may swap under low faction trust (threatening emissary).
        /// </summary>
        [TextArea(2, 4)]
        public string threateningBodyText;

        /// <summary>Faction id used for threatening body swap.</summary>
        public string threateningFactionId;

        /// <summary>When trust is strictly below this, use threateningBodyText if set.</summary>
        public float threateningTrustBelow = -20f;

        public string ResolveBodyText(EventContext context)
        {
            if (context != null
                && !string.IsNullOrEmpty(threateningBodyText)
                && !string.IsNullOrEmpty(threateningFactionId)
                && context.ResolveFactionTrust(threateningFactionId) < threateningTrustBelow)
            {
                return threateningBodyText;
            }
            return bodyText;
        }

        public virtual void Apply()
        {
            // Default apply fallback
        }
    }
}
