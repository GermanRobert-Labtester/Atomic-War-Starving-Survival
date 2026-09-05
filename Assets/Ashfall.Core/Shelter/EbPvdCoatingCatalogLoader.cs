// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// EB-PVD coating catalog loaded from
    /// <c>Assets/StreamingAssets/Data/ebpvd_coating_catalog.json</c>.
    /// Coatings, substrate classes (inventory result mapping), failure profiles,
    /// and maintenance windows are data authority — the engine registers from here.
    /// </summary>
    [Serializable]
    public sealed class EbPvdCoatingCatalog
    {
        public int schema_version = 1;
        public EbPvdCatalogMachineDef machine = new EbPvdCatalogMachineDef();
        public List<EbPvdCatalogCoatingDef> coatings = new List<EbPvdCatalogCoatingDef>();
        public List<EbPvdCatalogSubstrateDef> substrate_classes = new List<EbPvdCatalogSubstrateDef>();
        public List<EbPvdCatalogFailureDef> failure_profiles = new List<EbPvdCatalogFailureDef>();
        public EbPvdCatalogMaintenanceDef maintenance = new EbPvdCatalogMaintenanceDef();
    }

    [Serializable]
    public sealed class EbPvdCatalogMachineDef
    {
        public float min_voltage_kv = 15.0f;
        public float max_voltage_kv = 25.0f;
        public float nominal_power_kw = 10.0f;
        public float target_vacuum_mbar = 0.00001f;
    }

    [Serializable]
    public sealed class EbPvdCatalogCoatingDef
    {
        public string id = string.Empty;
        public string display_name_key = string.Empty;
        public string display_name = string.Empty;
        public string ceramic_target_item_id = "item_ebpvd_ceramic_target_ingot";
        public string bond_coat_item_id = "item_mcraly_bond_coat_powder";
        public List<string> valid_substrate_tags = new List<string>();
        public float target_thickness_um = 125.0f;
        public float base_duration_hours = 4.0f;
        public float base_power_kwh = 40.0f;
        public float thermal_resistance_bonus = 0.35f;
        public float max_temperature_bonus_c = 180.0f;
        public float durability_bonus = 0.4f;
        public float spallation_base_risk = 0.05f;
        public string required_skill = "mechanical";
    }

    [Serializable]
    public sealed class EbPvdCatalogSubstrateDef
    {
        public string tag = string.Empty;
        public string display_name = string.Empty;
        public string default_item_id = string.Empty;
        public string result_item_id = string.Empty;
    }

    [Serializable]
    public sealed class EbPvdCatalogFailureDef
    {
        public string code = string.Empty;
        public string display_name = string.Empty;
        public float base_risk;
        public float threshold_hours;
    }

    [Serializable]
    public sealed class EbPvdCatalogMaintenanceDef
    {
        public float filament_max_hours = 100.0f;
        public float vacuum_pump_max_hours = 150.0f;
        public float chamber_shield_max_hours = 200.0f;
    }

    public static class EbPvdCoatingCatalogLoader
    {
        public static EbPvdCoatingCatalog Load(string dataDir, IFileIO files, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(dataDir)) throw new ArgumentException("dataDir required", nameof(dataDir));
            if (files == null) throw new ArgumentNullException(nameof(files));
            if (json == null) throw new ArgumentNullException(nameof(json));

            string path = files.Combine(dataDir, "ebpvd_coating_catalog.json");
            if (!files.FileExists(path))
                return new EbPvdCoatingCatalog(); // Empty catalog; engine seeds stay in force

            string raw = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new EbPvdCoatingCatalog();

            var catalog = json.Deserialize<EbPvdCoatingCatalog>(raw);
            if (catalog == null)
                throw new InvalidOperationException("Failed to deserialize ebpvd_coating_catalog.json");

            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var coating in catalog.coatings)
            {
                if (string.IsNullOrEmpty(coating.id) || !seenIds.Add(coating.id))
                    throw new InvalidOperationException($"Duplicate or empty ebpvd coating id: '{coating?.id}'");
                if (coating.target_thickness_um <= 0f || coating.base_duration_hours <= 0f)
                    throw new InvalidOperationException($"ebpvd coating '{coating.id}' must have positive thickness and duration");
                if (string.IsNullOrEmpty(coating.ceramic_target_item_id) || string.IsNullOrEmpty(coating.bond_coat_item_id))
                    throw new InvalidOperationException($"ebpvd coating '{coating.id}' references an empty item id");
            }
            var seenProfiles = new HashSet<string>(StringComparer.Ordinal);
            foreach (var profile in catalog.failure_profiles)
            {
                if (string.IsNullOrEmpty(profile.code) || !seenProfiles.Add(profile.code))
                    throw new InvalidOperationException($"Duplicate or empty ebpvd failure code: '{profile?.code}'");
                if (profile.base_risk < 0f || profile.threshold_hours < 0f)
                    throw new InvalidOperationException($"ebpvd failure profile '{profile.code}' has negative values");
            }
            return catalog;
        }
    }

    /// <summary>Mapping helpers from catalog DTOs onto engine definitions.</summary>
    public static class EbPvdCoatingCatalogMapping
    {
        public static IEnumerable<EbPvdCoatingDef> ToCoatingDefs(this EbPvdCoatingCatalog catalog)
        {
            foreach (var c in catalog.coatings)
            {
                yield return new EbPvdCoatingDef
                {
                    Id = c.id,
                    DisplayName = string.IsNullOrEmpty(c.display_name) ? c.display_name_key : c.display_name,
                    CeramicTargetItemId = c.ceramic_target_item_id,
                    BondCoatItemId = c.bond_coat_item_id,
                    ValidSubstrateTags = new List<string>(c.valid_substrate_tags),
                    TargetThicknessUm = c.target_thickness_um,
                    BaseDurationHours = c.base_duration_hours,
                    BasePowerKw = c.base_power_kwh / Math.Max(1f, c.base_duration_hours),
                    ThermalResistanceBonus = c.thermal_resistance_bonus,
                    MaxTemperatureBonusC = c.max_temperature_bonus_c,
                    DurabilityBonus = c.durability_bonus,
                    SpallationBaseRisk = c.spallation_base_risk
                };
            }
        }

        public static IEnumerable<EbPvdFailureProfile> ToFailureProfiles(this EbPvdCoatingCatalog catalog)
        {
            foreach (var p in catalog.failure_profiles)
            {
                yield return new EbPvdFailureProfile
                {
                    Code = p.code,
                    BaseRisk = p.base_risk,
                    ThresholdHours = p.threshold_hours
                };
            }
        }

        /// <summary>Resolves the inventory result item for a substrate tag, if authored.</summary>
        public static bool TryGetSubstrateResultItem(this EbPvdCoatingCatalog catalog, string substrateTag, out string resultItemId)
        {
            foreach (var s in catalog.substrate_classes)
            {
                if (string.Equals(s.tag, substrateTag, StringComparison.Ordinal) &&
                    !string.IsNullOrEmpty(s.result_item_id))
                {
                    resultItemId = s.result_item_id;
                    return true;
                }
            }
            resultItemId = string.Empty;
            return false;
        }
    }
}
