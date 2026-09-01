using System.Collections.Generic;
#pragma warning disable CS0649
#pragma warning disable CS8618

namespace Ashfall.Core.Muster
{
    /// <summary>One authored faction-culture codex entry (muster_faction_culture.json).
    /// Plan 25 · 25E: everyday customs that make the factions societies instead of
    /// banners — claim marks, water accounting, the code of what the Toll refuses,
    /// the neutral-ground meal. Codex/codex-adjacent surfaces render these; events
    /// should not restate them.</summary>
    public class FactionCultureEntry
    {
        public string id = string.Empty;
        public string factionId = string.Empty;
        public string title = string.Empty;
        public string body = string.Empty;
    }

    /// <summary>
    /// Engine-agnostic loader for muster_faction_culture.json. Missing file →
    /// empty list; future schema → empty list (never partially parsed).
    /// </summary>
    public static class FactionCultureCatalogLoader
    {
        public const string FileName = "muster_faction_culture.json";
        public const int CurrentSchemaVersion = 1;

        public static List<FactionCultureEntry> LoadEntries(
            string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new List<FactionCultureEntry>();
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
                var root = json.Deserialize<CultureRoot>(raw);
                if (root == null) return result;
                if (root.schema_version > CurrentSchemaVersion)
                    return result;
                var entries = root.entries;
                if (entries == null) return result;
                for (int i = 0; i < entries.Count; i++)
                {
                    var e = entries[i];
                    if (e == null || string.IsNullOrEmpty(e.id) || string.IsNullOrEmpty(e.body))
                        continue;
                    result.Add(new FactionCultureEntry
                    {
                        id = e.id,
                        factionId = e.faction_id ?? string.Empty,
                        title = e.title ?? string.Empty,
                        body = e.body
                    });
                }
            }
            catch (System.Exception ex_CATDIAG)
            {
                Ashfall.Core.IO.CatalogDiagnostics.Warn(path, "FactionCultureRoot", ex_CATDIAG);
                return result;
            }
            return result;
        }

        private class CultureRoot
        {
            public int schema_version = 1;
            public List<Entry> entries = new List<Entry>();
        }

        private class Entry
        {
            public string id;
            public string faction_id;
            public string title;
            public string body;
        }
    }
}
