// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.IO;

namespace Ashfall.Core.Shelter
{
    [Serializable]
    public sealed class NuclearCoreDefinition
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public string powerClass = "RTG";
        public float baseElectricalOutput = 100.0f;
        public string thermalClass = "Passive";
        public string radiationClass = "Sealed";
        public float coolingDemand = 0.0f;
        public float shieldingRequirement = 20.0f;
        public float wearRate = 0.05f;
        public string decayClass = "DecadeLong";
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

        public static NuclearCoreCatalog? Load(string dataDir, IFileIO fileIO, IJsonSerializer jsonSerializer)
        {
            string path = Path.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path)) return null;

            string json = fileIO.ReadAllText(path);
            var dto = jsonSerializer.Deserialize<NuclearCoreCatalogDto>(json);
            if (dto?.profiles == null) return null;

            return new NuclearCoreCatalog(dto.profiles);
        }
    }
}
