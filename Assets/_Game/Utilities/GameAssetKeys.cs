using System;

namespace AtomicWar._Game.Utilities
{
    /// <summary>
    /// Canonical mapping from a game-data id to the path its art/audio lives at.
    ///
    /// Deliberately convention-based rather than a serialized field. Every item,
    /// module and faction already carries a stable snake_case id, so an extra
    /// "iconPath" field on ~500 definitions would be redundant data that can drift
    /// out of sync with the id. Instead the id *is* the key, and these helpers are
    /// the single place that knows how a key becomes a path.
    ///
    /// Paths are relative to a <c>Resources</c> folder and carry no file extension,
    /// which is what <c>Resources.Load</c> expects. If the project later moves to
    /// Addressables, only this class and the provider need to change.
    /// </summary>
    public static class GameAssetKeys
    {
        public const string ItemIconRoot = "Art/Items";
        public const string ShelterModuleIconRoot = "Art/ShelterModules";
        public const string FactionEmblemRoot = "Art/Factions";
        public const string SurvivorPortraitRoot = "Art/Portraits";
        public const string SfxRoot = "Audio/Sfx";
        public const string MusicRoot = "Audio/Music";
        public const string AmbienceRoot = "Audio/Ambience";

        /// <summary>Sprite for an inventory/world item, keyed by its item id.</summary>
        public static string ItemIcon(string itemId) => Combine(ItemIconRoot, itemId);

        /// <summary>Sprite for a shelter module, keyed by its module id.</summary>
        public static string ShelterModuleIcon(string moduleId) => Combine(ShelterModuleIconRoot, moduleId);

        /// <summary>Emblem sprite for a faction, keyed by faction id.</summary>
        public static string FactionEmblem(string factionId) => Combine(FactionEmblemRoot, factionId);

        /// <summary>Portrait sprite for a survivor archetype or portrait id.</summary>
        public static string SurvivorPortrait(string portraitId) => Combine(SurvivorPortraitRoot, portraitId);

        /// <summary>One-shot sound effect, keyed by cue id (e.g. "geiger_click").</summary>
        public static string Sfx(string cueId) => Combine(SfxRoot, cueId);

        /// <summary>Music track, keyed by track id.</summary>
        public static string Music(string trackId) => Combine(MusicRoot, trackId);

        /// <summary>Looping ambience bed, keyed by ambience id.</summary>
        public static string Ambience(string ambienceId) => Combine(AmbienceRoot, ambienceId);

        /// <summary>
        /// True when <paramref name="id"/> is a well-formed asset key: lowercase
        /// snake_case, no path separators or extension. Ids that fail this produce
        /// paths that silently never resolve, so callers and the editor report use
        /// this to catch the problem at authoring time instead of at runtime.
        /// </summary>
        public static bool IsValidId(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            for (int i = 0; i < id.Length; i++)
            {
                char c = id[i];
                bool ok = (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == '_';
                if (!ok) return false;
            }
            return true;
        }

        private static string Combine(string root, string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            return root + "/" + id.Trim();
        }
    }
}
