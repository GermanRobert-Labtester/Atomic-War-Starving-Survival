using System;
using System.Collections.Generic;

namespace Ashfall.Core.Subterranean
{
    /// <summary>One authored connection edge between two subterranean zones.</summary>
    public sealed class SubterraneanConnectionRuleDef
    {
        /// <summary>Optional origin zone; omitted edges hang off the enclosing zone's own id.</summary>
        public string from { get; set; } = string.Empty;
        /// <summary>Connected zone id (strict reference).</summary>
        public string to { get; set; } = string.Empty;
        /// <summary>Traversal label (blast_door / stairwell / winze / ...), vocabulary only.</summary>
        public string kind { get; set; } = string.Empty;
    }

    /// <summary>
    /// One authored subterranean zone template (Flagship XI — Plan 156). Zones are
    /// underground nodes beneath surface anchors; the SubterraneanSystem owns the
    /// persistent node state derived from these definitions.
    /// </summary>
    public sealed class SubterraneanZoneDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        /// <summary>Cave | Metro | UtilityTunnel | Bunker | Mine | CollapsedFacility — label only.</summary>
        public string zone_type { get; set; } = string.Empty;
        /// <summary>1 = shallow ... 3 = deep. Gates entry progression.</summary>
        public int depth_tier { get; set; } = 1;
        /// <summary>Base cave-in risk contribution, 0..1.</summary>
        public float base_structural_risk { get; set; }
        /// <summary>stale | thin | foul — abstract air-class label driving oxygen drain.</summary>
        public string oxygen_class { get; set; } = string.Empty;
        /// <summary>0..1 susceptibility to surface-weather flood pressure.</summary>
        public float flood_susceptibility { get; set; }
        /// <summary>Strict reference to a scavenging table id (canonical loot authority).</summary>
        public string scavenging_table_id { get; set; } = string.Empty;
        public List<string> narrative_tags { get; set; } = new List<string>();
        /// <summary>Strict reference to the surface loc_* anchor this zone lies beneath.</summary>
        public string surface_anchor_id { get; set; } = string.Empty;
        public List<SubterraneanConnectionRuleDef> connection_rules { get; set; } = new List<SubterraneanConnectionRuleDef>();
    }

    /// <summary>Container shape for subterranean_zones.json (the authority).</summary>
    public sealed class SubterraneanZoneCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public List<SubterraneanZoneDef> subterranean_zones { get; set; } = new List<SubterraneanZoneDef>();
    }

    /// <summary>
    /// Loads subterranean zone templates from JSON. Engine-agnostic: uses IFileIO
    /// and IJsonSerializer ports. The SubterraneanSystem remains the sole
    /// underground-node authority — the catalog only parameterizes templates.
    /// </summary>
    public static class SubterraneanZoneCatalogLoader
    {
        public const string DefaultFileName = "subterranean_zones.json";

        public static SubterraneanZoneCatalogContainer Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new SubterraneanZoneCatalogContainer();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new SubterraneanZoneCatalogContainer();

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return new SubterraneanZoneCatalogContainer();

            var container = json.Deserialize<SubterraneanZoneCatalogContainer>(rawText);
            return container ?? new SubterraneanZoneCatalogContainer();
        }
    }
}
