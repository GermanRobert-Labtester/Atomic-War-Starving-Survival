using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>One drainage stratum definition from sump_drainage_catalog.json (the authority).</summary>
    public sealed class SumpStratumDef
    {
        public string stratum_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        /// <summary>Multiplier applied to ingress from live groundwater pressure.</summary>
        public float water_table_pressure { get; set; } = 1f;
        /// <summary>Basin level rise (cm/day) at the reference groundwater level, before pressure/mitigations.</summary>
        public float base_ingress_cm_per_day { get; set; } = 2f;
        /// <summary>Volume fraction of ingress that enters as suspended silt (mass-conserving).</summary>
        public float silt_fraction { get; set; } = 0.05f;
        /// <summary>0..3 — drives tiered contamination gain when the basin floods.</summary>
        public int toxicity_tier { get; set; } = 0;
        /// <summary>Free-form composition tag; future metallurgy seam consumer.</summary>
        public string metal_content_profile { get; set; } = string.Empty;
        /// <summary>Free-form gas hazard tag; future ventilation emission seam consumer.</summary>
        public string gas_risk_profile { get; set; } = string.Empty;
        public float temperature_modifier { get; set; } = 1f;
        /// <summary>Multiplier on solids-driven pump wear while pumping this stratum's water.</summary>
        public float pump_load_modifier { get; set; } = 1f;
        public List<string> tags { get; set; } = new List<string>();
    }

    /// <summary>Container shape for sump_drainage_catalog.json (the authority).</summary>
    public sealed class SumpDrainageCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public List<SumpStratumDef> strata { get; set; } = new List<SumpStratumDef>();
    }

    /// <summary>
    /// Loads sump drainage strata definitions from JSON.
    /// Engine-agnostic: uses IFileIO and IJsonSerializer ports.
    /// The sump system remains the sole drainage authority — this catalog only
    /// parameterizes ingress, silt, toxicity, and pump load per stratum.
    /// </summary>
    public static class SumpDrainageCatalogLoader
    {
        public const string DefaultFileName = "sump_drainage_catalog.json";

        public static List<SumpStratumDef> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<SumpStratumDef>();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new List<SumpStratumDef>();

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return new List<SumpStratumDef>();

            var container = json.Deserialize<SumpDrainageCatalogContainer>(rawText);
            return container?.strata ?? new List<SumpStratumDef>();
        }
    }
}
