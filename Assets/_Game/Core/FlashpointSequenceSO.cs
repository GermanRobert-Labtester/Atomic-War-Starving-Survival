using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Economy;

namespace AtomicWar._Game.Flashpoint
{
    // -------------------------------------------------------------------
    // Designer-facing data for the Day-30 Flashpoint Choreography.
    //
    // Three concerns live here, in three ordered lists:
    //
    // 1) Buildup days (25-29, plus the 30-morning false calm): the
    //    per-day side effects that signal the end. Audio cues swap,
    //    the economy spikes, world flags get set so save/load knows
    //    which days have already applied.
    //
    // 2) Choreography steps: the second-by-second timeline of the
    //    moment itself (white flash -> EMP -> shockwave -> sirens ->
    //    weather shift -> radiation HUD unlock). Each step is a delay
    //    from the previous one plus an action. The mechanic-side actions
    //    (EMP, weather, morale) are wired by the choreographer; the
    //    narrative-side actions are typed EventBus events.
    //
    // 3) Accessibility overrides: when the player has the
    //    photosensitivity-safe option enabled, the flash is shorter and
    //    desaturated, and the camera shake amplitude is reduced.
    //
    // The asset is loaded once at GameBootstrap init. All tuning happens
    // in the inspector, never in code.
    // -------------------------------------------------------------------

    /// <summary>
    /// One per-day side effect for the 25-29 buildup. <see cref="Day"/>
    /// is the campaign day the side effects fire on. All side effects are
    /// idempotent: the choreographer skips a day if the world flag is
    /// already set, so save/load doesn't double-apply.
    /// </summary>
    [Serializable]
    public class FlashpointBuildupDay
    {
        [Tooltip("Campaign day the side effects fire on (25-29).")]
        public int day;

        [Tooltip("Stable id for the audio mix the ambient layer should switch to on this day " +
                 "(e.g. 'audio_cue_civil_war_silence'). Consumers look this up by id.")]
        public string audioCueId;

        [Tooltip("Id of an EconomyModifier entry below; null = no economy change.")]
        public string economyModifierId;

        [Tooltip("World flag set after the side effects apply. Used for save/load idempotency.")]
        public string worldFlagKey;

        [TextArea(2, 4)]
        [Tooltip("Designer-facing description of what the player should notice on this day " +
                 "(used by tests and by the IntelBible to keep narrative design and code in sync).")]
        public string narrativeNote;
    }

    /// <summary>
    /// Demand-spike recipe applied via DynamicEconomySystem.AdjustDemand
    /// when the matching buildup day fires. Items that don't exist in
    /// the catalog are silently ignored by AdjustDemand.
    /// </summary>
    [Serializable]
    public class FlashpointEconomyModifier
    {
        [Tooltip("Stable id referenced by FlashpointBuildupDay.economyModifierId.")]
        public string id;

        [Tooltip("Set DynamicEconomySystem into barter-only mode. Refuses offers " +
                 "consisting of items not in acceptedItemIds.")]
        public bool enableBarterOnlyMode;

        [Tooltip("Item ids accepted as offers while barter-only is on. " +
                 "Must match ids in items.json (e.g. 'iodine_pills', 'clean_water', 'fuel').")]
        public List<string> acceptedItemIds = new List<string>();

        [Tooltip("Demand multiplier added to each item id below on the day this modifier fires.")]
        public List<FlashpointDemandSpike> demandSpikes = new List<FlashpointDemandSpike>();
    }

    [Serializable]
    public class FlashpointDemandSpike
    {
        [Tooltip("Item id from items.json. The spike is silently dropped if the id isn't in the catalog.")]
        public string itemId;

        [Tooltip("Positive makes the item more scarce / valuable. 0..3 is the useful range.")]
        public float multiplierDelta = 1f;
    }

    /// <summary>
    /// Single second-by-second step of the choreography. The choreographer
    /// is a state machine over these. <see cref="DelayFromPreviousSeconds"/>
    /// is the wait since the previous step fired; <see cref="ActionId"/>
    /// picks the branch the choreographer runs.
    /// </summary>
    [Serializable]
    public class FlashpointChoreographyStep
    {
        [Tooltip("Stable id for the action ('flash', 'emp', 'shockwave', 'sirens', " +
                 "'weather_shift', 'radiation_hud_unlock', 'complete').")]
        public string actionId;

        [Tooltip("Seconds to wait since the previous step. Steps are timed in real seconds " +
                 "(Time.deltaTime), not game hours, because the flash is a visual event.")]
        public float delayFromPreviousSeconds = 1f;

        [Tooltip("0..1 amplitude for camera shake during this step. 0 = no shake.")]
        [Range(0f, 1f)] public float cameraShakeAmplitude = 0f;

        [Tooltip("Designer note for the choreography design doc / tests.")]
        [TextArea(2, 4)] public string narrativeNote;

        [Tooltip("Prompts #319–#325 — when actionId is 'weather_event_trigger', " +
                 "this is the snake_case weather-event id routed by the " +
                 "bridge in GameBootstrap.Weather.NewContent.cs to the right " +
                 "Weather_<Name>.Trigger() call. Empty for all other actionIds. " +
                 "Allowed values: weather_ash_lightning, weather_fog_of_particulate, " +
                 "weather_thermal_inversion, weather_ice_storm, weather_silence.")]
        public string weatherEventId = string.Empty;
    }

    /// <summary>
    /// Tunable accessibility overrides applied to the white flash and
    /// camera shake when the player has the photosensitivity-safe option
    /// enabled. Default values are conservative: shorter flash, lower
    /// shake amplitude, and a desaturated overlay instead of pure white.
    /// </summary>
    [Serializable]
    public class FlashpointAccessibilityOverrides
    {
        [Tooltip("Default white-flash duration when accessibility-safe is OFF.")]
        public float defaultFlashSeconds = 4.0f;

        [Tooltip("White-flash duration when accessibility-safe is ON. Shorter to reduce " +
                 "photosensitivity risk.")]
        public float safeFlashSeconds = 1.5f;

        [Tooltip("Camera-shake amplitude multiplier when accessibility-safe is ON. " +
                 "0.5 = half-strength shake.")]
        [Range(0f, 1f)] public float safeShakeMultiplier = 0.5f;

        [Tooltip("If true, the white flash is rendered as a desaturated bright overlay " +
                 "rather than pure white. Reduces seizure risk for photosensitive players.")]
        public bool safeDesaturateFlash = true;
    }

    [CreateAssetMenu(fileName = "FlashpointSequence", menuName = "ASHFALL/Data/Flashpoint Sequence")]
    public class FlashpointSequenceSO : ScriptableObject
    {
        [Header("Identity")]
        [Tooltip("Stable id (snake_case) for this sequence. Used in save data.")]
        public string sequenceId = "default";

        [Header("Buildup Days (25-30 morning)")]
        [Tooltip("One entry per day. Days outside [1, campaignLength] are ignored.")]
        public List<FlashpointBuildupDay> buildupDays = new List<FlashpointBuildupDay>();

        [Header("Economy Modifiers")]
        [Tooltip("Named demand-spike recipes referenced from buildupDays.")]
        public List<FlashpointEconomyModifier> economyModifiers = new List<FlashpointEconomyModifier>();

        [Header("Choreography Steps (the moment itself)")]
        [Tooltip("Ordered. The first step fires immediately on OnNuclearExchange; " +
                 "each subsequent step fires delayFromPreviousSeconds after the previous one.")]
        public List<FlashpointChoreographyStep> steps = new List<FlashpointChoreographyStep>();

        [Header("Accessibility")]
        public FlashpointAccessibilityOverrides accessibility = new FlashpointAccessibilityOverrides();

        /// <summary>Lookup an economy modifier by id (null if not present).</summary>
        public FlashpointEconomyModifier FindEconomyModifier(string id)
        {
            if (string.IsNullOrEmpty(id) || economyModifiers == null) return null;
            for (int i = 0; i < economyModifiers.Count; i++)
            {
                var m = economyModifiers[i];
                if (m != null && m.id == id) return m;
            }
            return null;
        }

        /// <summary>Lookup a buildup day entry by campaign day (null if not present).</summary>
        public FlashpointBuildupDay FindBuildupDay(int day)
        {
            if (buildupDays == null) return null;
            for (int i = 0; i < buildupDays.Count; i++)
            {
                var d = buildupDays[i];
                if (d != null && d.day == day) return d;
            }
            return null;
        }
    }
}
