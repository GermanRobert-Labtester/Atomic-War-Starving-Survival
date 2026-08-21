using System.Collections.Generic;
#pragma warning disable CS0649

namespace Ashfall.Core.Muster
{
    /// <summary>One entry from currents.json (the sector's political actors).</summary>
    public class CurrentDefinition
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public string alignment = string.Empty;
        public string homeRegion = string.Empty;
        public bool isActive;
        public float trust;
        public List<string> wants = new List<string>();
        public List<string> offers = new List<string>();
        public string signatureQuote = string.Empty;
        public string accessRule = string.Empty;
        public string badgeAssetId = string.Empty;
    }

    /// <summary>
    /// Engine-agnostic loader for currents.json (Expansion 06 roster data).
    /// The file is the authority; hosts present what this loads. Uses the
    /// IFileIO/IJsonSerializer ports so both engines and headless tests read
    /// identically.
    /// </summary>
    public static class CurrentsCatalogLoader
    {
        public const string FileName = "currents.json";
        public const int CurrentSchemaVersion = 1;

        public static List<CurrentDefinition> LoadCurrents(
            string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new List<CurrentDefinition>();
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
                var root = json.Deserialize<CurrentsCatalogRoot>(raw);
                if (root == null) return result;
                if (root.schema_version > CurrentSchemaVersion)
                    return result;
                var entries = root.entries;
                if (entries == null) return result;
                for (int i = 0; i < entries.Count; i++)
                {
                    var e = entries[i];
                    if (e == null || string.IsNullOrEmpty(e.id)) continue;
                    result.Add(new CurrentDefinition
                    {
                        id = e.id,
                        displayName = e.display_name ?? string.Empty,
                        alignment = e.alignment ?? string.Empty,
                        homeRegion = e.home_region ?? string.Empty,
                        isActive = e.is_active,
                        trust = e.trust,
                        wants = ToList(e.wants),
                        offers = ToList(e.offers),
                        signatureQuote = e.signature_quote ?? string.Empty,
                        accessRule = e.access_rule ?? string.Empty,
                        badgeAssetId = e.badge_asset_id ?? string.Empty
                    });
                }
            }
            catch
            {
                return result;
            }
            return result;
        }

        private static List<string> ToList(string[] source)
        {
            var list = new List<string>();
            if (source == null) return list;
            for (int i = 0; i < source.Length; i++)
                if (!string.IsNullOrEmpty(source[i])) list.Add(source[i]);
            return list;
        }

        /// <summary>Schema-envelope root for currents.json.</summary>
        private class CurrentsCatalogRoot
        {
            public int schema_version = 1;
            public List<CurrentEntry> entries = new List<CurrentEntry>();
        }

        private class CurrentEntry
        {
            public string id;
            public string display_name;
            public string alignment;
            public string home_region;
            public bool is_active;
            public float trust;
            public string[] wants;
            public string[] offers;
            public string signature_quote;
            public string access_rule;
            public string badge_asset_id;
        }
    }
}
