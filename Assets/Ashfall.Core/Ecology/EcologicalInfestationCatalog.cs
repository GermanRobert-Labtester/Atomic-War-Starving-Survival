using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Ecology
{
    /// <summary>
    /// Plan 28 Phase 4 — loader for <c>ecological_infestations.json</c> (the
    /// infestation data authority). Engine-agnostic: IFileIO + IJsonSerializer
    /// ports; parse failures route through CatalogDiagnostics (H4 pattern),
    /// missing file stays silent-null by design.
    /// </summary>
    public static class EcologicalInfestationCatalogLoader
    {
        public const string DefaultFileName = "ecological_infestations.json";

        public static List<EcologicalInfestationDefinition>? Load(
            string dataDir, IFileIO fileIO, IJsonSerializer json, ILog? log = null)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir)) return null;
            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path)) return null;
            try
            {
                string raw = fileIO.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var file = json.Deserialize<EcologicalInfestationFileRaw>(raw);
                return file?.infestations;
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(path, "EcologicalInfestationFileRaw", ex);
                return null;
            }
        }

        public const string FileName = "ecological_infestations.json";

        [Serializable]
        public sealed class EcologicalInfestationFileRaw
        {
            public int schema_version = 1;
            public string collection_id = string.Empty;
            public string description = string.Empty;
            public List<EcologicalInfestationDefinition> infestations = new List<EcologicalInfestationDefinition>();
        }
    }
}
