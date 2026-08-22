using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>Container shape for archive_inks.json (the authority).</summary>
    [Serializable]
    public sealed class ArchiveInkCatalogContainer
    {
        public List<InkMaterialDefinition> inks = new List<InkMaterialDefinition>();
    }

    /// <summary>
    /// Loads archive ink material definitions from JSON.
    /// Engine-agnostic: uses IFileIO and IJsonSerializer ports.
    /// </summary>
    public static class ArchiveInkCatalogLoader
    {
        public const string DefaultFileName = "archive_inks.json";

        public static List<InkMaterialDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<InkMaterialDefinition>();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new List<InkMaterialDefinition>();

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return new List<InkMaterialDefinition>();

            var container = json.Deserialize<ArchiveInkCatalogContainer>(rawText);
            return container?.inks ?? new List<InkMaterialDefinition>();
        }

        public static int LoadAndRegister(
            ArchiveDeskSystem system,
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer json)
        {
            if (system == null) return 0;
            var defs = Load(dataDir, fileIO, json);
            system.LoadInkCatalog(defs);
            return defs.Count;
        }
    }
}
