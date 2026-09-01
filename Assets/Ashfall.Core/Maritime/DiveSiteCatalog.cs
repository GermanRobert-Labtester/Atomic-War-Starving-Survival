using System;
using System.Collections.Generic;
using System.Linq;

using Ashfall.Core.IO;
namespace Ashfall.Core.Maritime
{
    /// <summary>
    /// ASHFALL: THE BLACK FLOTILLA (Expansion 09) — data model for dive sites.
    /// Reads dive_sites.json (schema_version 2) via the engine-agnostic
    /// IFileIO/IJsonSerializer ports. The shipwreck sites themselves are the
    /// immersive stage for <see cref="StealthDiveInstance"/>; the instance keeps
    /// its four-room shape and the catalog supplies per-site oxygen budget,
    /// noise floor, keeper questline thread, and room hazard profiles.
    /// Plan 23 adds optional site-scoped mechanics: coastal location anchor,
    /// gear gate, contamination key, data-driven safes (SafeCrackingSystem),
    /// procedural loot tables, and a discovery path. All new fields default
    /// safe for pre-Plan-23 catalogs and saves.
    /// </summary>
    [Serializable]
    public class DiveSiteRoom
    {
        public string room_type = string.Empty;
        public int hazard_level;
        public float search_difficulty;
    }

    [Serializable]
    public class DiveSiteDefinition
    {
        public string site_id = string.Empty;
        public string name = string.Empty;
        public int oxygen_budget_ticks;
        public float base_noise_floor;
        public string keeper_thread_id = string.Empty;
        public List<DiveSiteRoom> rooms = new List<DiveSiteRoom>();

        // ── Plan 23 (optional enrichment — old saves/catalogs default safe) ──

        /// <summary>Coastal overworld anchor (existing loc_*/location_* id). Empty = chart-only site.</summary>
        public string location_id = string.Empty;

        /// <summary>Gear gate: item id required to start this dive (empty = no gate).</summary>
        public string required_item_id = string.Empty;

        /// <summary>How many of the required item are consumed/needed (default 1).</summary>
        public int required_item_count = 1;

        /// <summary>PsychologicalContaminationSystem key applied on contaminated outcomes (site-scoped).</summary>
        public string contamination_key = string.Empty;

        /// <summary>Data-driven safes opened through the real SafeCrackingSystem.</summary>
        public List<SafeDefinition> safes = new List<SafeDefinition>();

        /// <summary>Procedural scavenge table for repeatable site salvage (VariableLootNode grammar).</summary>
        public List<VariableLootNode> loot_table = new List<VariableLootNode>();

        /// <summary>How this site becomes known to the player (lore/discovery path note).</summary>
        public string discovery = string.Empty;
    }

    [Serializable]
    public class DiveSiteContainer
    {
        public int schema_version;
        public List<DiveSiteDefinition> dive_sites = new List<DiveSiteDefinition>();
    }

    public static class DiveSiteCatalogLoader
    {
        public const string FileName = "dive_sites.json";

        public static DiveSiteContainer Load(
            string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var container = new DiveSiteContainer();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir)) return container;

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path)) return container;

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw)) return container;

            try
            {
                var parsed = json.Deserialize<DiveSiteContainer>(raw);
                if (parsed != null)
                {
                    container.schema_version = parsed.schema_version;
                    if (parsed.dive_sites != null)
                        container.dive_sites.AddRange(parsed.dive_sites);
                }
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(path, "DiveSiteContainer", ex_CATDIAG);
                // Missing or malformed catalog degrades to an empty container;
                // the host selftest surfaces it as a data-integrity failure.
            }
            return container;
        }

        public static DiveSiteDefinition? FindById(DiveSiteContainer container, string siteId)
        {
            if (container?.dive_sites == null || string.IsNullOrEmpty(siteId)) return null;
            for (int i = 0; i < container.dive_sites.Count; i++)
                if (container.dive_sites[i] != null && container.dive_sites[i].site_id == siteId)
                    return container.dive_sites[i];
            return null;
        }
    }
}
