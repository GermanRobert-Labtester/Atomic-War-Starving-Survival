using System.Collections.Generic;
#pragma warning disable CS0649
#pragma warning disable CS8618

namespace Ashfall.Core.Muster
{
    /// <summary>One Section III witness account (muster_witnesses.json).</summary>
    public class WitnessDefinition
    {
        public string id = string.Empty;
        public string witnessName = string.Empty;
        public string locationId = string.Empty;
        public string knowledgeKey = string.Empty;
        public int dayMin;
        public string body = string.Empty;
    }

    /// <summary>
    /// Engine-agnostic loader for muster_witnesses.json — the three Harven
    /// succession accounts (Section III). The framing sentence is composed by
    /// the journal's JournalVoice pipeline keyed to whoever RECORDED the
    /// account (the authoring survivor's RiskBiasTrait), never to the
    /// witness; these records carry only the pre-written body.
    /// </summary>
    public static class WitnessCatalogLoader
    {
        public const string FileName = "muster_witnesses.json";
        public const int CurrentSchemaVersion = 1;

        public static List<WitnessDefinition> LoadWitnesses(
            string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new List<WitnessDefinition>();
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
                var root = json.Deserialize<WitnessCatalogRoot>(raw);
                if (root == null) return result;
                if (root.schema_version > CurrentSchemaVersion)
                    return result;
                var entries = root.witnesses;
                if (entries == null) return result;
                for (int i = 0; i < entries.Count; i++)
                {
                    var e = entries[i];
                    if (e == null || string.IsNullOrEmpty(e.id)) continue;
                    result.Add(new WitnessDefinition
                    {
                        id = e.id,
                        witnessName = e.witness_name ?? string.Empty,
                        locationId = e.location_id ?? string.Empty,
                        knowledgeKey = e.knowledge_key ?? string.Empty,
                        dayMin = e.day_min,
                        body = e.body ?? string.Empty
                    });
                }
            }
            catch (System.Exception ex_CATDIAG)
            {
                Ashfall.Core.IO.CatalogDiagnostics.Warn(path, "WitnessCatalogRoot", ex_CATDIAG);
                return result;
            }
            return result;
        }

        /// <summary>Schema-envelope root for muster_witnesses.json.</summary>
        private class WitnessCatalogRoot
        {
            public int schema_version = 1;
            public List<WitnessEntry> witnesses = new List<WitnessEntry>();
        }

        private class WitnessEntry
        {
            public string id;
            public string witness_name;
            public string location_id;
            public string knowledge_key;
            public int day_min;
            public string body;
        }
    }
}
