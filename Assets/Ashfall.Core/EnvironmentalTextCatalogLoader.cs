using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    /// <summary>
    /// Single entry from environmental_texts_expansion_05.json.
    /// Each entry is a location-anchored environmental text (sign, note, diary, broadcast, etc.)
    /// with optional filtering dimensions (type, tags).
    /// </summary>
    [Serializable]
    public sealed class EnvironmentalTextEntry
    {
        /// <summary>Unique snake_case id, e.g. "env_bunker_perimeter_sign".</summary>
        public string id;

        /// <summary>Location id this text is anchored to, e.g. "bunker_perimeter".</summary>
        public string location;

        /// <summary>The environmental text/prose itself.</summary>
        public string text;

        /// <summary>Entry type, e.g. "warning", "note", "diary", "broadcast".</summary>
        public string type;

        /// <summary>Free-form tags, e.g. ["danger", "bunker", "warning"].</summary>
        public string[] tags;
    }

    /// <summary>Root shape of environmental_texts_expansion_05.json.</summary>
    [Serializable]
    public sealed class EnvironmentalTextCatalogFile
    {
        public int schema_version;
        public List<EnvironmentalTextEntry> environmental_texts = new List<EnvironmentalTextEntry>();
    }

    /// <summary>
    /// Loads environmental text entries from JSON.
    /// Engine-agnostic: uses IFileIO and IJsonSerializer ports (Invariant 2).
    /// </summary>
    public static class EnvironmentalTextCatalogLoader
    {
        public const string DefaultFileName = "environmental_texts_expansion_05.json";

        /// <summary>
        /// Loads all environmental text entries from the data directory.
        /// Returns an empty list when the file is missing, empty, or malformed.
        /// </summary>
        public static List<EnvironmentalTextEntry> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<EnvironmentalTextEntry>();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new List<EnvironmentalTextEntry>();

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return new List<EnvironmentalTextEntry>();

            var container = json.Deserialize<EnvironmentalTextCatalogFile>(rawText);
            return container?.environmental_texts ?? new List<EnvironmentalTextEntry>();
        }

        /// <summary>
        /// Loads the catalog and feeds entries into an <see cref="EnvironmentalTextSystem"/>.
        /// Returns the number of entries loaded.
        /// </summary>
        public static int LoadAndRegister(
            EnvironmentalTextSystem system,
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer json)
        {
            if (system == null) return 0;
            var entries = Load(dataDir, fileIO, json);
            system.LoadCatalog(entries);
            return entries.Count;
        }
    }
}
