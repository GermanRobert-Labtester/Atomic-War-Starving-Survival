// SPDX-License-Identifier: MIT
namespace Ashfall.Core.Localization
{
    /// <summary>
    /// Contextual first-hour lesson identities routed through the persisted
    /// onboarding authority (<c>OnboardingJourney.RequestContextualTutorial</c>).
    /// These are event-driven nudges, not journey stages: each fires once per
    /// campaign from a genuine runtime situation and never mutates gameplay.
    /// </summary>
    public static class OnboardingLessonLocalization
    {
        /// <summary>Fired when the player opens the expedition board with an unprotected party.</summary>
        public const string ProtectionBeforeDispatchId = "expedition.protection";

        /// <summary>Fired when the player checks the weather during a severe-weather day.</summary>
        public const string SevereWeatherPrepId = "weather.storm_prep";

        /// <summary>Fired on the first realtime combat encounter of a campaign.</summary>
        public const string CombatBasicsId = "combat.basics";

        /// <summary>
        /// Fired once when a living survivor starts day 1 in acute radiation
        /// sickness — the single first-hour `UNTAUGHT_LETHAL`
        /// (`docs/onboarding/TUTORIAL_REVIEW.md`, ranked proposal #1).
        /// </summary>
        public const string AcuteRadiationId = "medical.acute_radiation";

        /// <summary>Localization key for a lesson title (falls back to the authored English copy).</summary>
        public static string TutorialTitleKey(string tutorialId) => $"{tutorialId}.title";

        /// <summary>Localization key for a lesson body (falls back to the authored English copy).</summary>
        public static string TutorialBodyKey(string tutorialId) => $"{tutorialId}.body";
    }
}
