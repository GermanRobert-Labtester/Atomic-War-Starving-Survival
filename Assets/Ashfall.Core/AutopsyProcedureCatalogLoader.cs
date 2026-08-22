using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>Container shape for autopsy_procedures.json (the authority).</summary>
    [Serializable]
    public sealed class AutopsyProcedureCatalogContainer
    {
        public List<AutopsyProcedure> procedures = new List<AutopsyProcedure>();
    }

    /// <summary>
    /// Loads autopsy procedure definitions from JSON.
    /// Engine-agnostic: uses IFileIO and IJsonSerializer ports.
    /// </summary>
    public static class AutopsyProcedureCatalogLoader
    {
        public const string DefaultFileName = "autopsy_procedures.json";

        public static List<AutopsyProcedure> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<AutopsyProcedure>();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new List<AutopsyProcedure>();

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return new List<AutopsyProcedure>();

            var container = json.Deserialize<AutopsyProcedureCatalogContainer>(rawText);
            return container?.procedures ?? new List<AutopsyProcedure>();
        }

        public static int LoadAndRegister(
            AutopsySystem system,
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
