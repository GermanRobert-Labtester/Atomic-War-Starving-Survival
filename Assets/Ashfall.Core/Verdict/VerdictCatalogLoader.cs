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
