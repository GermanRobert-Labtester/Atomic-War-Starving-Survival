// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.IO;

namespace Ashfall.Core.Expeditions
{
    /// <summary>
    /// One rig/station class for vertical route logistics: normalized capability,
    /// cargo class, power mode, wear, and safety values. Gameplay-abstract —
    /// no real rigging, anchor-load, or cable specification data.
    /// </summary>
    [Serializable]
    public sealed class AscentRigProfile
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;
        public string tool_class = string.Empty;
        public List<string> route_capability_tags = new List<string>();
        public int max_cargo_class;
        public string power_mode = "hand"; // hand | motorized
        public string fuel_item_id = string.Empty;
        public int fuel_per_use;
        public int setup_ticks;
        public float travel_reduction_factor; // 0..1, catalog balance data
        public float wear_per_use;
        public float safety_rating; // 0..1 normalized
        public List<string> install_item_ids = new List<string>();
        public List<string> repair_item_ids = new List<string>();
        public List<string> tags = new List<string>();

        public bool IsMotorized => string.Equals(power_mode, "motorized", StringComparison.OrdinalIgnoreCase);

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(id)) { error = "Rig id cannot be empty."; return false; }
            if (route_capability_tags == null || route_capability_tags.Count == 0)
            { error = $"Rig '{id}' must declare at least one route capability tag."; return false; }
            if (max_cargo_class < 0) { error = $"Rig '{id}' cargo class cannot be negative."; return false; }
            if (!IsMotorized && !string.Equals(power_mode, "hand", StringComparison.OrdinalIgnoreCase))
            { error = $"Rig '{id}' power_mode must be 'hand' or 'motorized'."; return false; }
            if (IsMotorized && (string.IsNullOrWhiteSpace(fuel_item_id) || fuel_per_use <= 0))
            { error = $"Motorized rig '{id}' needs a fuel item and positive fuel_per_use."; return false; }
            if (setup_ticks <= 0) { error = $"Rig '{id}' must have setup_ticks > 0."; return false; }
            if (travel_reduction_factor < 0f || travel_reduction_factor > 1f)
            { error = $"Rig '{id}' travel_reduction_factor must be within [0, 1]."; return false; }
            if (wear_per_use < 0) { error = $"Rig '{id}' wear cannot be negative."; return false; }
            if (safety_rating < 0f || safety_rating > 1f)
            { error = $"Rig '{id}' safety_rating must be within [0, 1]."; return false; }
            if (install_item_ids == null || install_item_ids.Count == 0)
            { error = $"Rig '{id}' must bill at least one install item."; return false; }
            error = string.Empty;
            return true;
        }
    }

    [Serializable]
    public sealed class VerticalAscentCatalogDto
    {
        public int schema_version = 1;
        public List<AscentRigProfile> rigs = new List<AscentRigProfile>();
    }

    public sealed class VerticalAscentCatalog
    {
        private readonly Dictionary<string, AscentRigProfile> _rigs = new(StringComparer.OrdinalIgnoreCase);

        public VerticalAscentCatalog(IEnumerable<AscentRigProfile>? rigs)
        {
            foreach (var r in rigs ?? Enumerable.Empty<AscentRigProfile>())
                if (r != null && !string.IsNullOrWhiteSpace(r.id)) _rigs[r.id] = r;
        }

        public IReadOnlyDictionary<string, AscentRigProfile> Rigs => _rigs;

        public AscentRigProfile? GetRig(string rigId) =>
            string.IsNullOrEmpty(rigId) ? null : _rigs.TryGetValue(rigId, out var r) ? r : null;
    }

    public static class VerticalAscentCatalogLoader
    {
        public const string DefaultFileName = "climbing_winch_catalog.json";

        public static VerticalAscentCatalog? Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null) return null;
            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path)) return null;

            string raw = fileIO.ReadAllText(path);
            VerticalAscentCatalogDto? dto;
            try
            {
                dto = json.Deserialize<VerticalAscentCatalogDto>(raw);
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(path, "climbing_winch_catalog", ex);
                return null;
            }
            if (dto?.rigs == null) return null;

            return new VerticalAscentCatalog(dto.rigs);
        }
    }
}
