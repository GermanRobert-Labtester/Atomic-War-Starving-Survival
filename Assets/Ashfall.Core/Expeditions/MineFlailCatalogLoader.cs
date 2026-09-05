// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Expeditions
{
    /// <summary>
    /// Mine-clearing flail module catalog loaded from
    /// <c>Assets/StreamingAssets/Data/mine_flail_catalog.json</c>.
    /// Module tuning is data authority — the engine registers from here.
    /// </summary>
    [Serializable]
    public sealed class MineFlailCatalog
    {
        public int schema_version = 1;
        public List<MineFlailCatalogModuleDef> modules = new List<MineFlailCatalogModuleDef>();
        public MineFlailCatalogMaintenanceDef maintenance = new MineFlailCatalogMaintenanceDef();
    }

    [Serializable]
    public sealed class MineFlailCatalogModuleDef
    {
        public string id = string.Empty;
        public string display_name_key = string.Empty;
        public string display_name = string.Empty;
        public List<string> compatible_vehicle_tags = new List<string>();
        public float nominal_rpm = 300.0f;
        public float rpm_min = 150.0f;
        public float rpm_max = 450.0f;
        public float cleared_width_m = 3.5f;
        public float nominal_breach_speed_kph = 8.0f;
        public int chain_link_capacity = 40;
        public float chain_wear_per_km = 2.0f;
        public float hydraulic_pressure_nominal = 180.0f;
        public float blast_shield_integrity = 100.0f;
        public float mine_clearance_efficiency = 0.94f;
        public float tripwire_clearance_efficiency = 0.98f;
        public float obstacle_stall_risk = 0.05f;
        public float fuel_multiplier = 1.8f;
        public List<string> required_items = new List<string>();
    }

    [Serializable]
    public sealed class MineFlailCatalogMaintenanceDef
    {
        public int chain_link_cost_per_replacement = 1;
        public float hydraulic_rebuild_threshold_hours = 100.0f;
    }

    public static class MineFlailCatalogLoader
    {
        public static MineFlailCatalog Load(string dataDir, IFileIO files, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(dataDir)) throw new ArgumentException("dataDir required", nameof(dataDir));
            if (files == null) throw new ArgumentNullException(nameof(files));
            if (json == null) throw new ArgumentNullException(nameof(json));

            string path = files.Combine(dataDir, "mine_flail_catalog.json");
            if (!files.FileExists(path))
                return new MineFlailCatalog(); // Empty catalog; engine seeds stay in force

            string raw = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new MineFlailCatalog();

            var catalog = json.Deserialize<MineFlailCatalog>(raw);
            if (catalog == null)
                throw new InvalidOperationException("Failed to deserialize mine_flail_catalog.json");

            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var module in catalog.modules)
            {
                if (string.IsNullOrEmpty(module.id) || !seenIds.Add(module.id))
                    throw new InvalidOperationException($"Duplicate or empty mine flail module id: '{module?.id}'");
                if (module.chain_link_capacity <= 0 || module.nominal_breach_speed_kph <= 0f)
                    throw new InvalidOperationException($"mine flail module '{module.id}' has non-positive capacity or speed");
                if (module.hydraulic_pressure_nominal <= 0f)
                    throw new InvalidOperationException($"mine flail module '{module.id}' has non-positive hydraulic nominal");
            }
            return catalog;
        }
    }

    /// <summary>Mapping helpers from catalog DTOs onto engine definitions.</summary>
    public static class MineFlailCatalogMapping
    {
        public static IEnumerable<MineClearingFlailDef> ToModuleDefs(this MineFlailCatalog catalog)
        {
            foreach (var m in catalog.modules)
            {
                yield return new MineClearingFlailDef
                {
                    Id = m.id,
                    DisplayName = string.IsNullOrEmpty(m.display_name) ? m.display_name_key : m.display_name,
                    CompatibleVehicleTags = new List<string>(m.compatible_vehicle_tags),
                    NominalRpm = m.nominal_rpm,
                    ClearedWidthM = m.cleared_width_m,
                    NominalBreachSpeedKph = m.nominal_breach_speed_kph,
                    ChainLinkCapacity = m.chain_link_capacity,
                    ChainWearPerKm = m.chain_wear_per_km,
                    HydraulicPressureNominal = m.hydraulic_pressure_nominal,
                    MineClearanceEfficiency = m.mine_clearance_efficiency,
                    ObstacleStallRisk = m.obstacle_stall_risk
                };
            }
        }
    }
}
