using System.Collections.Generic;
#pragma warning disable CS0649
#pragma warning disable CS8618

namespace Ashfall.Core.Muster
{
    /// <summary>One Section XII epilogue-matrix outcome (muster_epilogues.json).</summary>
    public class EndingDefinition
    {
        public string endingKey = string.Empty;
        public string title = string.Empty;
        public string prose = string.Empty;
    }

    /// <summary>
    /// Engine-agnostic loader for muster_epilogues.json — the eight Day-360
    /// outcomes (Section XII). MusterSystem resolves ending keys at approach
    /// time; this catalog supplies the prose those keys name.
    /// </summary>
    public static class EpilogueMatrixLoader
    {
        public const string FileName = "muster_epilogues.json";
        public const int CurrentSchemaVersion = 1;

        public static List<EndingDefinition> LoadEpilogues(
            string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new List<EndingDefinition>();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return result;

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
                return result;

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return result;

            try
            {
                var root = json.Deserialize<EpilogueCatalogRoot>(raw);
                if (root == null) return result;
                if (root.schema_version > CurrentSchemaVersion)
                    return result;
                var entries = root.epilogues;
                if (entries == null) return result;
                for (int i = 0; i < entries.Count; i++)
                {
                    var e = entries[i];
                    if (e == null || string.IsNullOrEmpty(e.ending_key)) continue;
                    result.Add(new EndingDefinition
                    {
                        endingKey = e.ending_key,
                        title = e.title ?? string.Empty,
                        prose = e.prose ?? string.Empty
                    });
                }
            }
            catch (System.Exception ex_CATDIAG)
            {
                Ashfall.Core.IO.CatalogDiagnostics.Warn(path, "EpilogueCatalogRoot", ex_CATDIAG);
                return result;
            }
            return result;
        }

        /// <summary>Schema-envelope root for muster_epilogues.json.</summary>
        private class EpilogueCatalogRoot
        {
            public int schema_version = 1;
            public List<EndingEntry> epilogues = new List<EndingEntry>();
        }

        private class EndingEntry
        {
            public string ending_key;
            public string title;
            public string prose;
        }
    }
}
