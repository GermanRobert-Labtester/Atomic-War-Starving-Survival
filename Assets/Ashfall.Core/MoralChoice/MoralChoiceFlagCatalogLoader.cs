using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.MoralChoice
{
    // ── JSON wire records ──

    [Serializable]
    public sealed class MoralChoiceFlagCatalogContainer
    {
        public int schema_version = 1;
        public string description = string.Empty;
        public List<MoralFlagRecord> flags = new List<MoralFlagRecord>();
    }

    [Serializable]
    public sealed class MoralFlagRecord
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
    }

    /// <summary>
    /// Loads moral_choice_flags.json — flag definitions for branch locking
    /// and quest gating. Engine-agnostic.
    /// </summary>
    public static class MoralChoiceFlagCatalogLoader
    {
        public const string DefaultFileName = "moral_choice_flags.json";

        public static MoralChoiceFlagDefinitions Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new MoralChoiceFlagDefinitions();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new MoralChoiceFlagDefinitions();

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new MoralChoiceFlagDefinitions();

            var container = json.Deserialize<MoralChoiceFlagCatalogContainer>(raw);
            if (container?.flags == null)
                return new MoralChoiceFlagDefinitions();

            var data = new MoralChoiceFlagDefinitions();
            data.Flags = container.flags
                .Where(f => f != null)
                .Select(f => new MoralFlagDefinition { Id = f.id, DisplayName = f.display_name })
                .ToList();
            return data;
        }
    }
}
