// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using Ashfall.Core.IO;

namespace Ashfall.Core.Shelter
{
    [Serializable]
    public sealed class NuclearCoreDefinition
    {
        public string id = string.Empty;

        // Plan 47 wave 1: canonical snake_case wire keys; legacy camelCase keys
        // are accepted through CatalogKeyNormalizer (docs/data/SNAKE_CASE_MIGRATION.md).
        [JsonPropertyName("display_name")]
        public string displayName = string.Empty;
        [JsonPropertyName("power_class")]
        public string powerClass = "RTG";
        [JsonPropertyName("base_electrical_output")]
        public float baseElectricalOutput = 100.0f;
        [JsonPropertyName("thermal_class")]
        public string thermalClass = "Passive";
        [JsonPropertyName("radiation_class")]
        public string radiationClass = "Sealed";
        [JsonPropertyName("cooling_demand")]
        public float coolingDemand = 0.0f;
        [JsonPropertyName("shielding_requirement")]
        public float shieldingRequirement = 20.0f;
        [JsonPropertyName("wear_rate")]
        public float wearRate = 0.05f;
        [JsonPropertyName("decay_class")]
        public string decayClass = "DecadeLong";
        [JsonPropertyName("emergency_shutdown_item_id")]
        public string emergencyShutdownItemId = string.Empty;
        public List<string> tags = new List<string>();

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                error = "Nuclear core ID cannot be empty.";
                return false;
            }
            if (baseElectricalOutput < 0)
            {
                error = $"Core '{id}' cannot have negative electrical output.";
                return false;
            }
            if (coolingDemand < 0 || shieldingRequirement < 0 || wearRate < 0)
            {
                error = $"Core '{id}' cannot have negative cooling, shielding, or wear rates.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(emergencyShutdownItemId))
            {
                error = $"Core '{id}' must specify an emergencyShutdownItemId.";
                return false;
            }
            if (thermalClass != "Passive" && thermalClass != "LowThermal" && thermalClass != "HighThermal")
            {
                error = $"Core '{id}' has invalid thermalClass '{thermalClass}'.";
                return false;
            }
            if (radiationClass != "Sealed" && radiationClass != "Moderate" && radiationClass != "HighPenetration")
            {
                error = $"Core '{id}' has invalid radiationClass '{radiationClass}'.";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }

    [Serializable]
    public sealed class NuclearCoreCatalogDto
    {
        public int schema_version { get; set; } = 1;
        public List<NuclearCoreDefinition> profiles { get; set; } = new List<NuclearCoreDefinition>();
    }

    public sealed class NuclearCoreCatalog
    {
        private readonly Dictionary<string, NuclearCoreDefinition> _profiles = new(StringComparer.OrdinalIgnoreCase);

        public NuclearCoreCatalog(IEnumerable<NuclearCoreDefinition>? profiles)
        {
            if (profiles == null) return;
            foreach (var p in profiles)
            {
                if (p != null && !string.IsNullOrWhiteSpace(p.id))
                    _profiles[p.id] = p;
            }
        }

        public IReadOnlyDictionary<string, NuclearCoreDefinition> Profiles => _profiles;

        public NuclearCoreDefinition? GetProfile(string profileId)
        {
            if (string.IsNullOrEmpty(profileId)) return null;
            return _profiles.TryGetValue(profileId, out var def) ? def : null;
        }
    }

    public static class NuclearCoreCatalogLoader
    {
        public const string DefaultFileName = "nuclear_core_profiles.json";

        /// <summary>Plan 47 wave 1: legacy camelCase → canonical snake_case (spelling-only).</summary>
        public static readonly IReadOnlyDictionary<string, string> KeyAliases = new Dictionary<string, string>
        {
            ["displayName"] = "display_name",
            ["powerClass"] = "power_class",
            ["baseElectricalOutput"] = "base_electrical_output",
            ["thermalClass"] = "thermal_class",
            ["radiationClass"] = "radiation_class",
            ["coolingDemand"] = "cooling_demand",
            ["shieldingRequirement"] = "shielding_requirement",
            ["wearRate"] = "wear_rate",
            ["decayClass"] = "decay_class",
            ["emergencyShutdownItemId"] = "emergency_shutdown_item_id",
        };

        public static NuclearCoreCatalog? Load(string dataDir, IFileIO fileIO, IJsonSerializer jsonSerializer)
        {
            string path = Path.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path)) return null;

            string json = fileIO.ReadAllText(path);
            // Dual-read: legacy camelCase accepted, conflicts fail loudly.
            string normalized = CatalogKeyNormalizer.Normalize(json, KeyAliases, DefaultFileName);
            var dto = jsonSerializer.Deserialize<NuclearCoreCatalogDto>(normalized);
            if (dto?.profiles == null) return null;

            return new NuclearCoreCatalog(dto.profiles);
        }
    }
}
