using System.Collections.Generic;
#pragma warning disable CS0649
#pragma warning disable CS8618

namespace Ashfall.Core.Muster
{
    /// <summary>One conditional testimony variant. Selection is first authored match:
    /// requiresAnyFlags (≥1 set), requiresAllFlags (all set), forbidsFlags (none set);
    /// a variant with no conditions is the unconditional fallback.</summary>
    public class WitnessTestimony
    {
        public string variantId = string.Empty;
        public List<string> requiresAnyFlags = new List<string>();
        public List<string> requiresAllFlags = new List<string>();
        public List<string> forbidsFlags = new List<string>();
        public string body = string.Empty;
    }

    /// <summary>One Muster witness account (muster_witnesses.json, schema v1 or v2).
    /// v1 entries carry a flat body; v2 entries may carry faction/subject/priority
    /// metadata and conditional testimonies. `body` always mirrors the first
    /// testimony's text so v1-era presentation keeps working.</summary>
    public class WitnessDefinition
    {
        public string id = string.Empty;
        public string witnessName = string.Empty;
        public string locationId = string.Empty;
        public string knowledgeKey = string.Empty;
        public int dayMin;
        public string body = string.Empty;

        // ── v2 ────────────────────────────────────────────────────────
        public string factionId = string.Empty;
        public string subjectId = string.Empty;   // npc_*/survivor_* id for alive/dead gating
        public int priority;                       // ordering: priority desc, then id ordinal
        public List<WitnessTestimony> testimonies = new List<WitnessTestimony>();
    }

    /// <summary>
    /// Engine-agnostic loader for muster_witnesses.json. Schema v1 (three flat
    /// Harven succession accounts) and v2 (Plan 25 conditional testimonies) both
    /// load; a schema beyond CurrentSchemaVersion is rejected (empty list), never
    /// partially parsed. Framing is composed by the journal's JournalVoice pipeline
    /// keyed to whoever RECORDED the account, never to the witness.
    /// </summary>
    public static class WitnessCatalogLoader
    {
        public const string FileName = "muster_witnesses.json";
        public const int CurrentSchemaVersion = 2;

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
                    var def = new WitnessDefinition
                    {
                        id = e.id,
                        witnessName = e.witness_name ?? string.Empty,
                        locationId = e.location_id ?? string.Empty,
                        knowledgeKey = e.knowledge_key ?? string.Empty,
                        dayMin = e.day_min,
                        factionId = e.faction_id ?? string.Empty,
                        subjectId = e.subject_id ?? string.Empty,
                        priority = e.priority
                    };
                    if (e.testimonies != null && e.testimonies.Count > 0)
                    {
                        for (int t = 0; t < e.testimonies.Count; t++)
                        {
                            var te = e.testimonies[t];
                            if (te == null || string.IsNullOrEmpty(te.body)) continue;
                            var testimony = new WitnessTestimony
                            {
                                variantId = te.variant_id ?? string.Empty,
                                body = te.body
                            };
                            CopyFlags(te.requires_any_flags, testimony.requiresAnyFlags);
                            CopyFlags(te.requires_all_flags, testimony.requiresAllFlags);
                            CopyFlags(te.forbids_flags, testimony.forbidsFlags);
                            def.testimonies.Add(testimony);
                        }
                    }
                    if (def.testimonies.Count == 0 && !string.IsNullOrEmpty(e.body))
                    {
                        // v1 shape (or a v2 entry that kept a bare body).
                        def.testimonies.Add(new WitnessTestimony { variantId = "account", body = e.body });
                    }
                    if (def.testimonies.Count == 0) continue;
                    def.body = def.testimonies[0].body;
                    result.Add(def);
                }
            }
            catch (System.Exception ex_CATDIAG)
            {
                Ashfall.Core.IO.CatalogDiagnostics.Warn(path, "WitnessCatalogRoot", ex_CATDIAG);
                return result;
            }
            return result;
        }

        private static void CopyFlags(List<string> source, List<string> target)
        {
            if (source == null) return;
            for (int i = 0; i < source.Count; i++)
                if (!string.IsNullOrEmpty(source[i])) target.Add(source[i]);
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
            // ── v2 ──
            public string faction_id;
            public string subject_id;
            public int priority;
            public List<TestimonyEntry> testimonies;
        }

        private class TestimonyEntry
        {
            public string variant_id;
            public List<string> requires_any_flags;
            public List<string> requires_all_flags;
            public List<string> forbids_flags;
            public string body;
        }
    }
}
