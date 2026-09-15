// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Radio
{
    [Serializable]
    public sealed class UvCoronaDetectorProfile
    {
        public string detector_id = string.Empty;
        public string battery_item_id = string.Empty;
        public float base_detection_range = 1f;
        public float minimum_fault_intensity = 0.25f;
        public float sensor_noise = 0.08f;
        public int battery_use_per_scan = 1;
        public float calibration_drift_per_day = 0.001f;
        public float condition_noise_factor = 0.15f;

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(detector_id) || string.IsNullOrWhiteSpace(battery_item_id))
            { error = "UV detector id/battery is empty"; return false; }
            if (base_detection_range <= 0f || minimum_fault_intensity < 0f || minimum_fault_intensity > 1f
                || sensor_noise < 0f || sensor_noise > 1f || battery_use_per_scan <= 0
                || calibration_drift_per_day < 0f || condition_noise_factor < 0f || condition_noise_factor > 1f)
            { error = $"UV detector '{detector_id}' has invalid bounds"; return false; }
            error = string.Empty;
            return true;
        }
    }

    [Serializable]
    public sealed class UvCoronaEnvironmentProfile
    {
        public string environment_id = string.Empty;
        public float visibility = 1f;
        public float humidity = 0.2f;
        public float ash_load;
        public float fault_activity_modifier = 1f;

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(environment_id)) { error = "UV environment id is empty"; return false; }
            if (visibility < 0f || visibility > 1f || humidity < 0f || humidity > 1f
                || ash_load < 0f || ash_load > 1f || fault_activity_modifier <= 0f || fault_activity_modifier > 2f)
            { error = $"UV environment '{environment_id}' has invalid bounds"; return false; }
            error = string.Empty;
            return true;
        }
    }

    [Serializable]
    public sealed class UvCoronaCatalogDto
    {
        public int schema_version = 1;
        public List<UvCoronaDetectorProfile> detectors = new List<UvCoronaDetectorProfile>();
        public List<UvCoronaEnvironmentProfile> environments = new List<UvCoronaEnvironmentProfile>();
    }

    public sealed class UvCoronaDetectionCatalog
    {
        public int SchemaVersion { get; }
        public IReadOnlyList<UvCoronaDetectorProfile> Detectors { get; }
        public IReadOnlyList<UvCoronaEnvironmentProfile> Environments { get; }

        public UvCoronaDetectionCatalog(UvCoronaCatalogDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            UvCoronaDetectionCatalogLoader.Validate(dto);
            SchemaVersion = dto.schema_version;
            Detectors = new List<UvCoronaDetectorProfile>(dto.detectors);
            Environments = new List<UvCoronaEnvironmentProfile>(dto.environments);
        }

        public UvCoronaDetectorProfile? FindDetector(string id)
        {
            foreach (var profile in Detectors)
                if (profile.detector_id == id) return profile;
            return null;
        }

        public UvCoronaEnvironmentProfile? FindEnvironment(string id)
        {
            foreach (var profile in Environments)
                if (profile.environment_id == id) return profile;
            return null;
        }
    }

    public static class UvCoronaDetectionCatalogLoader
    {
        public const string CatalogFileName = "uv_corona_detector_catalog.json";

        public static UvCoronaDetectionCatalog Load(string dataDirectory, IFileIO? fileIO = null, IJsonSerializer? serializer = null)
        {
            fileIO ??= new FileSystemIO();
            serializer ??= new SystemTextJsonSerializer();
            string path = fileIO.Combine(dataDirectory, CatalogFileName);
            if (!fileIO.FileExists(path)) throw new InvalidOperationException($"Missing {CatalogFileName}: {path}");
            var dto = serializer.Deserialize<UvCoronaCatalogDto>(fileIO.ReadAllText(path));
            if (dto == null) throw new InvalidOperationException($"{CatalogFileName} deserialized null");
            return new UvCoronaDetectionCatalog(dto);
        }

        public static void Validate(UvCoronaCatalogDto dto)
        {
            if (dto.schema_version < 1) throw new InvalidOperationException("uv_corona_detector_catalog schema_version must be >= 1");
            if (dto.detectors == null || dto.detectors.Count == 0) throw new InvalidOperationException("UV catalog requires a detector");
            if (dto.environments == null || dto.environments.Count == 0) throw new InvalidOperationException("UV catalog requires an environment");
            var detectorIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var detector in dto.detectors)
            {
                if (detector == null) throw new InvalidOperationException("Null UV detector profile");
                if (!detector.Validate(out string error)) throw new InvalidOperationException(error);
                if (!detectorIds.Add(detector.detector_id)) throw new InvalidOperationException($"Duplicate UV detector '{detector.detector_id}'");
            }
            var environmentIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var environment in dto.environments)
            {
                if (environment == null) throw new InvalidOperationException("Null UV environment profile");
                if (!environment.Validate(out string error)) throw new InvalidOperationException(error);
                if (!environmentIds.Add(environment.environment_id)) throw new InvalidOperationException($"Duplicate UV environment '{environment.environment_id}'");
            }
        }
    }
}
