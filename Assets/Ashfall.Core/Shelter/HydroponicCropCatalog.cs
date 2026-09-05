// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.IO;

namespace Ashfall.Core.Shelter
{
    [Serializable]
    public sealed class HydroponicCropDefinition
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public string description = string.Empty;
        public int germinationTicks = 2;
        public int growthTicks = 6;
        public float waterLitresPerDay = 3.0f;
        public float nutrientUnitsPerDay = 2.0f;
        public float ledPowerWatts = 450.0f;
        public string preferredSpectrum = "Growth_Blue";
        public string baseYieldItemId = string.Empty;
        public int baseYieldQuantity = 1;
        public float coldTolerance = 0.5f;
        public float contaminationTolerance = 0.5f;
        public float mutationAffinity = 0.15f;
        public List<string> tags = new List<string>();

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                error = "Crop ID cannot be empty.";
                return false;
            }
            if (germinationTicks <= 0 || growthTicks <= 0)
            {
                error = $"Crop '{id}' must have germinationTicks and growthTicks > 0.";
                return false;
            }
            if (waterLitresPerDay < 0 || nutrientUnitsPerDay < 0 || ledPowerWatts < 0)
            {
                error = $"Crop '{id}' cannot have negative resource demands.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(baseYieldItemId) || baseYieldQuantity <= 0)
            {
                error = $"Crop '{id}' must have a valid baseYieldItemId and baseYieldQuantity > 0.";
                return false;
            }
            if (preferredSpectrum != "Growth_Blue" && preferredSpectrum != "Flowering_Red" && preferredSpectrum != "Hardening_Infrared")
            {
                error = $"Crop '{id}' has invalid preferredSpectrum '{preferredSpectrum}'.";
                return false;
            }
            if (float.IsNaN(coldTolerance) || float.IsInfinity(coldTolerance) ||
                float.IsNaN(contaminationTolerance) || float.IsInfinity(contaminationTolerance) ||
                float.IsNaN(mutationAffinity) || float.IsInfinity(mutationAffinity))
            {
                error = $"Crop '{id}' has non-finite tolerance or affinity parameters.";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }

    [Serializable]
    public sealed class HydroponicCropCatalogDto
    {
        public int schema_version { get; set; } = 1;
        public List<HydroponicCropDefinition> crops { get; set; } = new List<HydroponicCropDefinition>();
    }

    public sealed class HydroponicCropCatalog
    {
        private readonly Dictionary<string, HydroponicCropDefinition> _crops = new(StringComparer.OrdinalIgnoreCase);

        public HydroponicCropCatalog(IEnumerable<HydroponicCropDefinition>? crops)
        {
            if (crops == null) return;
            foreach (var c in crops)
            {
                if (c != null && !string.IsNullOrWhiteSpace(c.id))
                    _crops[c.id] = c;
            }
        }

        public IReadOnlyDictionary<string, HydroponicCropDefinition> Crops => _crops;

        public HydroponicCropDefinition? GetCrop(string cropId)
        {
            if (string.IsNullOrEmpty(cropId)) return null;
            return _crops.TryGetValue(cropId, out var def) ? def : null;
        }
    }

    public static class HydroponicCropCatalogLoader
    {
        public const string DefaultFileName = "hydroponic_crops.json";

        public static HydroponicCropCatalog? Load(string dataDir, IFileIO fileIO, IJsonSerializer jsonSerializer)
        {
            string path = Path.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path)) return null;

            string json = fileIO.ReadAllText(path);
            var dto = jsonSerializer.Deserialize<HydroponicCropCatalogDto>(json);
            if (dto?.crops == null) return null;

            return new HydroponicCropCatalog(dto.crops);
        }
    }
}
