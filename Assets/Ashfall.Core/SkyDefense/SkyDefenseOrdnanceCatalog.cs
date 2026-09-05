using System;
using System.Collections.Generic;
using Ashfall.Core.Catalogs;

namespace Ashfall.Core.SkyDefense
{
    /// <summary>
    /// Authored ordnance definition (sky_defense_ordnance.json).
    /// Real-world-like figures are GAMEPLAY DATA for authored probability and
    /// logistics math — never executable ballistics. The <c>item_id</c> is the
    /// single countable inventory authority for the round type.
    /// </summary>
    [Serializable]
    public sealed class SkyDefenseOrdnanceDefinition
    {
        public string ordnance_id = string.Empty;
        public string display_name = string.Empty;
        public string ammo_type = string.Empty;
        public string item_id = string.Empty;
        public int magazine_units;                 // rounds per loaded magazine (>0)
        public float tracking_modifier;            // -2..+2 added to intercept chance
        public float interception_modifier;        // -0.2..+0.4 added to intercept chance
        public int heat_per_volley;                // 0..100 barrel heat per volley
        public int recoil_load;                    // 0..10 hydraulic wear per volley
        public float burst_radius_units;           // game abstraction units
        public float interception_ceiling_units;   // game abstraction units
        public int radar_lock_units;               // game ticks of lock time
        public float fragmentation_density;        // 0..1 authored effectiveness share
        public float propellant_grain_kg;          // authored flavor/logistics figure
        public float residual_shrapnel_severity;   // 0..1 fraction of strike kept as shrapnel on success
        public List<string>? tags;
    }

    [Serializable]
    public sealed class SkyDefenseOrdnanceCatalogContainer
    {
        public List<SkyDefenseOrdnanceDefinition> ordnance = new();
    }

    /// <summary>Loads and validates sky_defense_ordnance.json (the authority).</summary>
    public static class SkyDefenseOrdnanceCatalogLoader
    {
        public const string DefaultFileName = "sky_defense_ordnance.json";
        public const int ExpectedOrdnanceCount = 6;

        public static List<SkyDefenseOrdnanceDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                throw new InstitutionCatalogException(DefaultFileName, new[] { $"catalog file not found: {path}" });

            string rawText = fileIO.ReadAllText(path);
            var container = json.Deserialize<SkyDefenseOrdnanceCatalogContainer>(rawText)
                ?? throw new InstitutionCatalogException(DefaultFileName, new[] { "catalog root failed to deserialize" });

            Validate(DefaultFileName, container.ordnance);
            return container.ordnance;
        }

        public static void Validate(string catalogName, List<SkyDefenseOrdnanceDefinition>? ordnance)
        {
            var f = new InstitutionCatalogParse.Findings();
            if (ordnance == null || ordnance.Count == 0)
            {
                f.Add("*", "ordnance", "catalog defines no ordnance");
                f.ThrowIfAny(catalogName);
                return;
            }

            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var o in ordnance)
            {
                string id = o.ordnance_id;
                f.RequireNonEmpty(id, "ordnance_id", id);
                if (!InstitutionCatalogParse.IsCanonicalSnakeCase(id))
                    f.Add(id, "ordnance_id", "must be canonical snake_case");
                if (!seen.Add(id))
                    f.Add(id, "ordnance_id", "duplicate ordnance_id within catalog");

                f.RequireNonEmpty(id, "display_name", o.display_name);
                f.RequireNonEmpty(id, "ammo_type", o.ammo_type);
                f.RequireNonEmpty(id, "item_id", o.item_id);
                f.RequirePositive(id, "magazine_units", o.magazine_units, "rounds");
                f.RequireRange(id, "tracking_modifier", o.tracking_modifier, -2f, 2f);
                f.RequireRange(id, "interception_modifier", o.interception_modifier, -0.2f, 0.4f);
                f.RequireRange(id, "heat_per_volley", o.heat_per_volley, 0, 100);
                f.RequireRange(id, "recoil_load", o.recoil_load, 0, 10);
                f.RequireAtLeastZero(id, "burst_radius_units", o.burst_radius_units);
                f.RequireAtLeastZero(id, "interception_ceiling_units", o.interception_ceiling_units);
                f.RequireRange(id, "radar_lock_units", o.radar_lock_units, 0, 10);
                f.RequireRange(id, "fragmentation_density", o.fragmentation_density, 0f, 1f);
                f.RequireAtLeastZero(id, "propellant_grain_kg", o.propellant_grain_kg);
                f.RequireRange(id, "residual_shrapnel_severity", o.residual_shrapnel_severity, 0f, 1f);
            }

            f.ThrowIfAny(catalogName);
        }
    }
}
