// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.IO;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// One charge class for the shelter cupola: abstract feedstock/fuel/flux
    /// billing plus normalized heat, wear, slag, and hazard balance values.
    /// Deliberately gameplay-abstract — no real furnace operating data.
    /// </summary>
    [Serializable]
    public sealed class CupolaChargeDefinition
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;
        public string feedstock_item_id = string.Empty;
        public int feedstock_quantity;
        public string fuel_item_id = string.Empty;
        public int fuel_quantity;
        public string flux_item_id = string.Empty;
        public int flux_quantity;
        public float required_blower_power_w;
        public string heat_band = "MeltReady"; // Cold, Heating, MeltReady, Overheated
        public int melt_ticks;
        public float refractory_wear_per_batch;
        public float slag_load;
        public List<string> allowed_mold_ids = new List<string>();
        public string base_yield_item_id = string.Empty;
        public int base_yield_quantity = 1;
        public float hazard_rating; // 0..1 normalized
        public List<string> tags = new List<string>();

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(id)) { error = "Charge id cannot be empty."; return false; }
            if (string.IsNullOrWhiteSpace(feedstock_item_id) || feedstock_quantity <= 0)
            { error = $"Charge '{id}' needs a feedstock item and positive quantity."; return false; }
            if (string.IsNullOrWhiteSpace(fuel_item_id) || fuel_quantity <= 0)
            { error = $"Charge '{id}' needs a fuel item and positive quantity."; return false; }
            if (string.IsNullOrWhiteSpace(flux_item_id) || flux_quantity <= 0)
            { error = $"Charge '{id}' needs a flux item and positive quantity."; return false; }
            if (melt_ticks <= 0) { error = $"Charge '{id}' must have melt_ticks > 0."; return false; }
            if (refractory_wear_per_batch < 0 || slag_load < 0 || required_blower_power_w < 0)
            { error = $"Charge '{id}' wear/slag/power values cannot be negative."; return false; }
            if (hazard_rating < 0f || hazard_rating > 1f)
            { error = $"Charge '{id}' hazard_rating must be within [0, 1]."; return false; }
            if (string.IsNullOrWhiteSpace(base_yield_item_id) || base_yield_quantity <= 0)
            { error = $"Charge '{id}' needs a base yield item and positive quantity."; return false; }
            if (allowed_mold_ids == null || allowed_mold_ids.Count == 0)
            { error = $"Charge '{id}' must allow at least one mold."; return false; }
            error = string.Empty;
            return true;
        }
    }

    /// <summary>A mold/pattern profile: what the melt becomes when poured. Data-driven; no real green-sand procedure.</summary>
    [Serializable]
    public sealed class FoundryMoldProfile
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;
        public string output_item_id = string.Empty;
        public int output_quantity = 1;
        public int metal_units_required;
        public float quality_target; // 0..100 normalized
        public float wear_per_cast;
        public List<string> tags = new List<string>();

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(id)) { error = "Mold id cannot be empty."; return false; }
            if (string.IsNullOrWhiteSpace(output_item_id) || output_quantity <= 0)
            { error = $"Mold '{id}' needs an output item and positive quantity."; return false; }
            if (metal_units_required <= 0) { error = $"Mold '{id}' must require metal units."; return false; }
            if (quality_target < 0f || quality_target > 100f)
            { error = $"Mold '{id}' quality_target must be within [0, 100]."; return false; }
            if (wear_per_cast < 0) { error = $"Mold '{id}' wear cannot be negative."; return false; }
            error = string.Empty;
            return true;
        }
    }

    /// <summary>Maintenance profile for the cupola: refractory reline (and optional chemical descale via item flow).</summary>
    [Serializable]
    public sealed class CupolaMaintenanceProfile
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;
        public string refractory_item_id = string.Empty;
        public int refractory_quantity = 1;
        public float labor_hours;
        public float refractory_restore = 30f;
        public float slag_reduction = 50f;
        public string descale_item_id = string.Empty;
        public int descale_quantity = 1;
        public float descale_slag_reduction;
        public List<string> tags = new List<string>();
    }

    [Serializable]
    public sealed class CupolaFoundryCatalogDto
    {
        public int schema_version = 1;
        public List<CupolaChargeDefinition> charges = new List<CupolaChargeDefinition>();
        public List<FoundryMoldProfile> molds = new List<FoundryMoldProfile>();
        public CupolaMaintenanceProfile? maintenance;
    }

    public sealed class CupolaFoundryCatalog
    {
        private readonly Dictionary<string, CupolaChargeDefinition> _charges = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, FoundryMoldProfile> _molds = new(StringComparer.OrdinalIgnoreCase);

        public CupolaFoundryCatalog(IEnumerable<CupolaChargeDefinition>? charges, IEnumerable<FoundryMoldProfile>? molds, CupolaMaintenanceProfile? maintenance)
        {
            Maintenance = maintenance;
            foreach (var c in charges ?? Enumerable.Empty<CupolaChargeDefinition>())
                if (c != null && !string.IsNullOrWhiteSpace(c.id)) _charges[c.id] = c;
            foreach (var m in molds ?? Enumerable.Empty<FoundryMoldProfile>())
                if (m != null && !string.IsNullOrWhiteSpace(m.id)) _molds[m.id] = m;
        }

        public CupolaMaintenanceProfile? Maintenance { get; }
        public IReadOnlyDictionary<string, CupolaChargeDefinition> Charges => _charges;
        public IReadOnlyDictionary<string, FoundryMoldProfile> Molds => _molds;

        public CupolaChargeDefinition? GetCharge(string chargeId) =>
            string.IsNullOrEmpty(chargeId) ? null : _charges.TryGetValue(chargeId, out var c) ? c : null;

        public FoundryMoldProfile? GetMold(string moldId) =>
            string.IsNullOrEmpty(moldId) ? null : _molds.TryGetValue(moldId, out var m) ? m : null;
    }

    public static class CupolaFoundryCatalogLoader
    {
        public const string DefaultFileName = "cupola_foundry_catalog.json";

        public static CupolaFoundryCatalog? Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null) return null;
            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path)) return null;

            string raw = fileIO.ReadAllText(path);
            CupolaFoundryCatalogDto? dto;
            try
            {
                dto = json.Deserialize<CupolaFoundryCatalogDto>(raw);
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(path, "cupola_foundry_catalog", ex);
                return null;
            }
            if (dto?.charges == null || dto.molds == null) return null;

            return new CupolaFoundryCatalog(dto.charges, dto.molds, dto.maintenance);
        }
    }
}
