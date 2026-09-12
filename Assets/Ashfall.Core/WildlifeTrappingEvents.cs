// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core
{
    /// <summary>Stable, presentation-neutral lifecycle event for one trap site.</summary>
    public sealed class TrapLifecycleEvent
    {
        public string siteId = string.Empty;
        public string trapId = string.Empty;
        public string trapType = string.Empty;
        public bool isBroken;
        public int day;
    }

    /// <summary>
    /// Exactly-once butchery identity. The primary species is explicit so a
    /// bycatch entry can never accidentally become a second morale action.
    /// </summary>
    public sealed class ButcheryCompletedEvent
    {
        public string actionId = string.Empty;
        public string siteId = string.Empty;
        public string butcherId = string.Empty;
        public string primarySpeciesId = string.Empty;
        public string bycatchSpeciesId = string.Empty;
        public float primaryYield;
        public float bycatchYield;
        public float totalYield;
        public bool isToxic;
        public bool bycatchToxic;
        public int setDay;
    }

    /// <summary>
    /// Complete, presentation-neutral result for one resolved secondary catch.
    /// The legacy six-argument event remains available for existing consumers;
    /// this payload guarantees subscribers observe the committed yield and
    /// toxicity state as well as stable domain IDs.
    /// </summary>
    public sealed class BycatchOccurredEvent
    {
        public string siteId = string.Empty;
        public string trapId = string.Empty;
        public string primarySpeciesId = string.Empty;
        public string bycatchSpeciesId = string.Empty;
        public float bycatchYield;
        public bool bycatchToxic;
        public int day;
        public string hunterId = string.Empty;
    }

    // ── Flagship Integration Plan IV — shared event-delivery contract ──

    /// <summary>Delivery lifecycle for externally consumed trapping facts.</summary>
    public static class WildlifeTrappingEventStatus
    {
        public const string Pending = "pending";
        public const string Delivered = "delivered";
    }

    /// <summary>
    /// Kinds of unresolved external facts the trapping system can enqueue.
    /// Trapping emits domain facts; the destination authority owns the actual
    /// consequence (moral dilemma, encounter content, radio presentation).
    /// </summary>
    public static class WildlifeTrappingEventKinds
    {
        /// <summary>Moral dilemma for a morally weighted primary-catch butchery.</summary>
        public const string MoralConsequence = "moral_consequence";

        /// <summary>Human-interference encounter surfaced by an eligible failed check.</summary>
        public const string TrapEncounter = "trap_encounter";

        /// <summary>Noteworthy trapping outcome for the radio information layer.</summary>
        public const string TrappingBroadcast = "trapping_broadcast";

        /// <summary>Atmospheric failed-check event resolved by the narrative authority.</summary>
        public const string NarrativeIncident = "narrative_incident";
    }

    /// <summary>
    /// Semantic content IDs for the three trap-interference encounter
    /// families. Authored as real encounter definitions in
    /// narrative_encounters.json; these constants are the canonical spellings.
    /// </summary>
    public static class TrapEncounterIds
    {
        public const string BaitStolen = "enc_trap_bait_stolen";
        public const string Tampered = "enc_trap_tampered";
        public const string StrangerDiscovery = "enc_trap_stranger_discovery";
    }

    /// <summary>
    /// Stable authored IDs for miss-only trapping incidents. Ordering is
    /// explicit so incident selection never depends on dictionary iteration.
    /// </summary>
    public static class TrapNarrativeIncidentIds
    {
        public const string SprungBloodTrail = "trap_sprung_blood_trail";
        public const string BaitStolen = "trap_bait_stolen";
        public const string HumanBootprints = "trap_human_bootprints";

        public static readonly string[] Ordered =
        {
            SprungBloodTrail,
            BaitStolen,
            HumanBootprints
        };
    }

    /// <summary>Semantic content IDs for trapping radio broadcasts.</summary>
    public static class TrappingBroadcastIds
    {
        public const string FirstCatch = "radio_wildlife_first_catch";
        public const string TrapBroken = "radio_wildlife_trap_broken";
        public const string RareBycatch = "radio_wildlife_rare_bycatch";
    }

    /// <summary>
    /// Moral-severity tier mapping for trapping prey. Severity (0..1) maps to
    /// a tiered authored moral quest; the moral-choice system owns the actual
    /// dilemma and resolution. Thresholds are inclusive lower bounds.
    /// </summary>
    public static class TrappingMoralTier
    {
        public const string HighQuestId = "quest_moral_trap_prey_high";
        public const string MediumQuestId = "quest_moral_trap_prey_medium";
        public const string LowQuestId = "quest_moral_trap_prey_low";

        public const float HighThreshold = 0.75f;
        public const float MediumThreshold = 0.35f;

        public static string ResolveQuestId(float moralWeight)
        {
            if (moralWeight >= HighThreshold) return HighQuestId;
            if (moralWeight >= MediumThreshold) return MediumQuestId;
            return LowQuestId;
        }
    }

    /// <summary>
    /// One unresolved external trapping fact. Persisted in
    /// <see cref="WildlifeTrappingState"/> with a stable identity so a save
    /// taken on any delivery boundary restores without loss or duplication.
    /// Delivery lifecycle: Created → Pending → (destination accepts) →
    /// Delivered (pruned from the pending surface).
    /// </summary>
    [Serializable]
    public sealed class WildlifeTrappingPendingEvent
    {
        public string eventId = string.Empty;
        public int sequence;
        public string kind = string.Empty; // WildlifeTrappingEventKinds
        public string sourceTrapSiteId = string.Empty;
        public string survivorId = string.Empty;
        public string speciesId = string.Empty;
        public string payloadId = string.Empty; // quest/encounter/broadcast content ID
        public float weight;
        public int day;
        public string status = WildlifeTrappingEventStatus.Pending;
    }
}
