using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core.IO;

namespace Ashfall.Core.Shelter
{
    [Serializable]
    public sealed class OrbitalEventDef
    {
        public string id = string.Empty;
        public string name = string.Empty;
        public string description = string.Empty;
        public string severity = "Minor";
        public string signal_type = "radar_anomaly";
        public bool is_false_positive = false;
        public float impact_energy_mj = 10f;
        public int lead_time_days = 3;
        public int affected_cell_spread = 1;
        public float penetration_power_mj = 8f;
        public string salvage_yield_item_id = string.Empty;
        public int salvage_yield_quantity = 1;
        public string revealed_site_id = string.Empty;
        public string radio_hook_text = string.Empty;
    }

    [Serializable]
    public sealed class OrbitalHarrowCatalogFile
    {
        public int schema_version = 1;
        public List<OrbitalEventDef> events = new List<OrbitalEventDef>();
    }

    public static class OrbitalHarrowCatalogLoader
    {
        public const string FileName = "orbital_harrow_events.json";

        public static List<OrbitalEventDef> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var list = new List<OrbitalEventDef>();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return list;

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
                return list;

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return list;

            try
            {
                var parsed = json.Deserialize<OrbitalHarrowCatalogFile>(raw);
                if (parsed?.events != null)
                {
                    list.AddRange(parsed.events);
                }
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(path, "OrbitalHarrowCatalogFile", ex);
            }

            return list;
        }
    }
}
