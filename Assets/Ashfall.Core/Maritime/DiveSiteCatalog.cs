using System;
using System.Collections.Generic;

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
                CatalogDiagnostics.Warn("<unknown>", "unknown", ex_CATDIAG);
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
