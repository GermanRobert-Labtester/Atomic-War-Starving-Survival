using System.Collections.Generic;

namespace Ashfall.Core.Verdict
{
    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — catalog loader for the three
    /// Verdict data files (verdict_data.json, verdict_locations.json,
    /// verdict_radio.json). Authoring authority is the design bible; the loader
    /// mirrors the WitnessCatalogLoader pattern (missing file => empty list,
    /// malformed => empty, engine-agnostic via IFileIO/IJsonSerializer).
    /// </summary>
    public static class VerdictCatalogLoader
    {
        public const string DataFile = "verdict_data.json";
        public const string LocationsFile = "verdict_locations.json";
        public const string ItemsFile = "verdict_items.json";
        public const string RadioFile = "verdict_radio.json";

        // ── Locations ───────────────────────────────────────────────────────────

        public class VerdictLocationEntry
        {
            public string id = string.Empty;
            public string displayName = string.Empty;
            public string description = string.Empty;
            public int dangerLevel = 5;
            public float travelHours = 5f;
            public float baseRadsPerHour = 30f;
        }

        public static List<VerdictLocationEntry> LoadLocations(
            string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new List<VerdictLocationEntry>();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir)) return result;
            string path = fileIO.Combine(dataDir, LocationsFile);
            if (!fileIO.FileExists(path)) return result;
            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw)) return result;
            try
            {
                var parsed = json.Deserialize<VerdictLocationEntry[]>(raw);
                if (parsed == null) return result;
                for (int i = 0; i < parsed.Length; i++)
                {
                    var e = parsed[i];
                    if (e == null || string.IsNullOrEmpty(e.id)) continue;
                    result.Add(e);
                }
            }
            catch { return result; }
            return result;
        }

        // ── Items ───────────────────────────────────────────────────────────────

        /// <summary>One Verdict story/evidence item row. Runtime-schema compatible
        /// (id/displayName/weightKg/tradeValue/category/description) with optional
        /// enrichments the bible authors (tier, mechanical_effects, etc.). Loaded
        /// only so the story content is reachable; never treated as loot.</summary>
        /// <summary>Optional effect payload of an evidence/story item (recorded for
        /// reachability; the game enrolls evidence through the EvidenceLedger, not
        /// this mirror). Mirrors the authored JSON shape.</summary>
        public class VerdictItemEffects
        {
            public int enrolled_evidence;
            public string note = string.Empty;
        }

        public class VerdictItemEntry
        {
            public string id = string.Empty;
            public string displayName = string.Empty;
            public float weightKg;
            public float tradeValue;
            public string category = "story_item";
            public string tier = string.Empty;
            public string description = string.Empty;
            public VerdictItemEffects mechanical_effects = null;
            public string downstream_quest_trigger = string.Empty;
            public string faction_affinity = string.Empty;
            public string rarity = string.Empty;
        }

        public static List<VerdictItemEntry> LoadItems(
            string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new List<VerdictItemEntry>();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir)) return result;
            string path = fileIO.Combine(dataDir, ItemsFile);
            if (!fileIO.FileExists(path)) return result;
            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw)) return result;
            try
            {
                var parsed = json.Deserialize<VerdictItemEntry[]>(raw);
                if (parsed == null) return result;
                for (int i = 0; i < parsed.Length; i++)
                {
                    var e = parsed[i];
                    if (e == null || string.IsNullOrEmpty(e.id)) continue;
                    result.Add(e);
                }
            }
            catch { return result; }
            return result;
        }

        // ── Radio ───────────────────────────────────────────────────────────────

        public class VerdictRadioEntry
        {
            public string id = string.Empty;
            public string frequency = string.Empty;
            public int dayTrigger = 180;
            public string source = string.Empty;
            public string message = string.Empty;
            public string signalStrength = string.Empty;
            public string kind = "telemetry";
        }

        public static List<VerdictRadioEntry> LoadRadio(
            string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new List<VerdictRadioEntry>();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir)) return result;
            string path = fileIO.Combine(dataDir, RadioFile);
            if (!fileIO.FileExists(path)) return result;
            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw)) return result;
            try
            {
                var parsed = json.Deserialize<VerdictRadioContainer>(raw);
                if (parsed?.broadcasts == null) return result;
                foreach (var e in parsed.broadcasts)
                {
                    if (e == null || string.IsNullOrEmpty(e.id)) continue;
                    result.Add(e);
                }
            }
            catch { return result; }
            return result;
        }

        private class VerdictDataContainer
        {
            public List<string> corruption_corpus = new List<string>();
        }

        /// <summary>Load the corruption corpus from verdict_data.json (empty if missing).</summary>
        public static List<string> LoadCorruptionCorpus(
            string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new List<string>();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir)) return result;
            string path = fileIO.Combine(dataDir, DataFile);
            if (!fileIO.FileExists(path)) return result;
            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw)) return result;
            try
            {
                var parsed = json.Deserialize<VerdictDataContainer>(raw);
                if (parsed?.corruption_corpus != null)
                    result.AddRange(parsed.corruption_corpus);
            }
            catch { }
            return result;
        }

        private class VerdictRadioContainer
        {
            public List<VerdictRadioEntry> broadcasts = new List<VerdictRadioEntry>();
        }
    }
}
