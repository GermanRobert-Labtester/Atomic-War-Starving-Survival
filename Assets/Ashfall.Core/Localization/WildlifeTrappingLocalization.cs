using System;

namespace Ashfall.Core.Localization
{
    /// <summary>
    /// Stable localization-key conventions for wildlife trapping content.
    /// Catalogs remain backward-compatible raw-text catalogs: callers pass the
    /// catalog text as the resolver fallback, while saves continue to contain
    /// only stable content IDs.
    /// </summary>
    public static class WildlifeTrappingLocalization
    {
        public const string FirstSnareTutorialId = "wildlife.trapping.first_snare";
        public const string WearOutTutorialId = "wildlife.trapping.wear_out";
        public const string BycatchTutorialId = "wildlife.trapping.bycatch";

        public static string TrapNameKey(string trapId) => Key("trap", trapId, "name");
        public static string TrapDescriptionKey(string trapId) => Key("trap", trapId, "description");
        public static string PreyNameKey(string speciesId) => Key("prey", speciesId, "name");
        public static string PreyDescriptionKey(string speciesId) => Key("prey", speciesId, "description");
        public static string BaitNameKey(string baitId) => Key("bait", baitId, "name");
        public static string TutorialTitleKey(string tutorialId) => $"{tutorialId}.title";
        public static string TutorialBodyKey(string tutorialId) => $"{tutorialId}.body";

        private static string Key(string category, string id, string field)
        {
            string safeCategory = string.IsNullOrWhiteSpace(category) ? "content" : category.Trim();
            string safeId = string.IsNullOrWhiteSpace(id) ? "unknown" : id.Trim();
            return $"wildlife.{safeCategory}.{safeId}.{field}";
        }
    }
}
