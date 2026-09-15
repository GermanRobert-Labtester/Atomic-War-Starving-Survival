// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.World
{
    [Serializable]
    public sealed class GprEquipmentProfile
    {
        public string gpr_id = string.Empty;
        public string battery_item_id = string.Empty;
        public float starting_condition = 1f;
        public float starting_calibration = 1f;

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(gpr_id) || string.IsNullOrWhiteSpace(battery_item_id)) { error = "GPR equipment id/battery is empty"; return false; }
            if (starting_condition <= 0f || starting_condition > 1f || starting_calibration <= 0f || starting_calibration > 1f)
            { error = $"GPR equipment '{gpr_id}' has invalid bounds"; return false; }
            error = string.Empty;
            return true;
        }
    }

    [Serializable]
    public sealed class GprSurveyModeProfile
    {
        public string mode_id = string.Empty;
        public float penetration_rating;
        public float resolution_rating;
        public int power_cost = 1;
        public int scan_time_ticks = 1;

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(mode_id)) { error = "GPR mode id is empty"; return false; }
            if (penetration_rating <= 0f || penetration_rating > 1f || resolution_rating <= 0f || resolution_rating > 1f
                || power_cost <= 0 || scan_time_ticks <= 0)
            { error = $"GPR mode '{mode_id}' has invalid bounds"; return false; }
            error = string.Empty;
            return true;
        }
    }

    [Serializable]
    public sealed class GprTerrainProfile
    {
        public string terrain_id = string.Empty;
        public float attenuation;
        public float moisture;

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(terrain_id)) { error = "GPR terrain id is empty"; return false; }
            if (attenuation < 0f || attenuation > 1f || moisture < 0f || moisture > 1f)
            { error = $"GPR terrain '{terrain_id}' has invalid bounds"; return false; }
            error = string.Empty;
            return true;
        }
    }

    [Serializable]
    public sealed class GprAnomalyProfile
    {
        public string anomaly_id = string.Empty;
        public string anomaly_class = "unknown_reflector";
        public float depth_band;
        public float signal_strength;

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(anomaly_id) || string.IsNullOrWhiteSpace(anomaly_class)) { error = "GPR anomaly id/class is empty"; return false; }
            if (depth_band < 0f || signal_strength < 0f || signal_strength > 1f)
            { error = $"GPR anomaly '{anomaly_id}' has invalid bounds"; return false; }
            error = string.Empty;
            return true;
        }
    }

    [Serializable]
    public sealed class GroundPenetratingRadarCatalogDto
    {
        public int schema_version = 1;
        public List<GprEquipmentProfile> equipment = new List<GprEquipmentProfile>();
        public List<GprSurveyModeProfile> modes = new List<GprSurveyModeProfile>();
        public List<GprTerrainProfile> terrains = new List<GprTerrainProfile>();
        public List<GprAnomalyProfile> anomalies = new List<GprAnomalyProfile>();
    }

    public sealed class GroundPenetratingRadarCatalog
    {
        public int SchemaVersion { get; }
        public IReadOnlyList<GprEquipmentProfile> Equipment { get; }
        public IReadOnlyList<GprSurveyModeProfile> Modes { get; }
        public IReadOnlyList<GprTerrainProfile> Terrains { get; }
        public IReadOnlyList<GprAnomalyProfile> Anomalies { get; }

        public GroundPenetratingRadarCatalog(GroundPenetratingRadarCatalogDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            GroundPenetratingRadarCatalogLoader.Validate(dto);
            SchemaVersion = dto.schema_version;
            Equipment = new List<GprEquipmentProfile>(dto.equipment);
            Modes = new List<GprSurveyModeProfile>(dto.modes);
            Terrains = new List<GprTerrainProfile>(dto.terrains);
            Anomalies = new List<GprAnomalyProfile>(dto.anomalies);
        }

        public GprEquipmentProfile? FindEquipment(string id) { foreach (var x in Equipment) if (x.gpr_id == id) return x; return null; }
        public GprSurveyModeProfile? FindMode(string id) { foreach (var x in Modes) if (x.mode_id == id) return x; return null; }
        public GprTerrainProfile? FindTerrain(string id) { foreach (var x in Terrains) if (x.terrain_id == id) return x; return null; }
        public GprAnomalyProfile? FindAnomaly(string id) { foreach (var x in Anomalies) if (x.anomaly_id == id) return x; return null; }
    }

    public static class GroundPenetratingRadarCatalogLoader
    {
        public const string CatalogFileName = "gpr_exploration_catalog.json";

        public static GroundPenetratingRadarCatalog Load(string dataDirectory, IFileIO? fileIO = null, IJsonSerializer? serializer = null)
        {
            fileIO ??= new FileSystemIO();
            serializer ??= new SystemTextJsonSerializer();
            string path = fileIO.Combine(dataDirectory, CatalogFileName);
            if (!fileIO.FileExists(path)) throw new InvalidOperationException($"Missing {CatalogFileName}: {path}");
            var dto = serializer.Deserialize<GroundPenetratingRadarCatalogDto>(fileIO.ReadAllText(path));
            if (dto == null) throw new InvalidOperationException($"{CatalogFileName} deserialized null");
            return new GroundPenetratingRadarCatalog(dto);
        }

        public static void Validate(GroundPenetratingRadarCatalogDto dto)
        {
            if (dto.schema_version < 1) throw new InvalidOperationException("gpr_exploration_catalog schema_version must be >= 1");
            if (dto.equipment == null || dto.equipment.Count == 0 || dto.modes == null || dto.modes.Count == 0
                || dto.terrains == null || dto.terrains.Count == 0 || dto.anomalies == null || dto.anomalies.Count == 0)
                throw new InvalidOperationException("GPR catalog requires equipment, modes, terrains, and anomalies");
            ValidateUnique(dto.equipment, x => x.gpr_id, "equipment", x => x.Validate(out string error) ? string.Empty : error);
            ValidateUnique(dto.modes, x => x.mode_id, "mode", x => x.Validate(out string error) ? string.Empty : error);
            ValidateUnique(dto.terrains, x => x.terrain_id, "terrain", x => x.Validate(out string error) ? string.Empty : error);
            ValidateUnique(dto.anomalies, x => x.anomaly_id, "anomaly", x => x.Validate(out string error) ? string.Empty : error);
            bool hasDeep = false, hasDetail = false;
            foreach (var mode in dto.modes)
            {
                hasDeep |= mode.penetration_rating > mode.resolution_rating;
                hasDetail |= mode.resolution_rating > mode.penetration_rating;
            }
            if (!hasDeep || !hasDetail) throw new InvalidOperationException("GPR modes must expose a penetration/resolution tradeoff");
        }

        private static void ValidateUnique<T>(IReadOnlyList<T> list, Func<T, string> id, string label, Func<T, string> validationError)
            where T : class
        {
            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var entry in list)
            {
                if (entry == null) throw new InvalidOperationException($"Null GPR {label}");
                string error = validationError(entry);
                if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error);
                if (!ids.Add(id(entry))) throw new InvalidOperationException($"Duplicate GPR {label} '{id(entry)}'");
            }
        }
    }
}
