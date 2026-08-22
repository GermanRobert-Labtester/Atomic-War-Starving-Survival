using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>Container shape for library_manuals.json (the authority).</summary>
    [Serializable]
    public sealed class LibraryManualCatalogContainer
    {
        public List<ManualDefinition> manuals = new List<ManualDefinition>();
    }

    /// <summary>
    /// Loads library manual definitions from JSON.
    /// Engine-agnostic: uses IFileIO and IJsonSerializer ports.
    /// </summary>
    public static class LibraryManualCatalogLoader
    {
        public const string DefaultFileName = "library_manuals.json";

        public static List<ManualDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<ManualDefinition>();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new List<ManualDefinition>();

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return new List<ManualDefinition>();

            var container = json.Deserialize<LibraryManualCatalogContainer>(rawText);
            return container?.manuals ?? new List<ManualDefinition>();
        }

        public static int LoadAndRegister(
            LibraryStudySystem system,
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer json)
        {
            if (system == null) return 0;
            var defs = Load(dataDir, fileIO, json);
            system.LoadCatalog(defs);
            return defs.Count;
        }
    }
}
