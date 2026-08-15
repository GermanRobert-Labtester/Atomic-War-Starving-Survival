using System.Collections.Generic;

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
                var parsed = json.Deserialize<WitnessEntry[]>(raw);
                if (parsed == null) return result;
                for (int i = 0; i < parsed.Length; i++)
                {
                    var e = parsed[i];
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
            catch
            {
                return result;
            }
            return result;
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
