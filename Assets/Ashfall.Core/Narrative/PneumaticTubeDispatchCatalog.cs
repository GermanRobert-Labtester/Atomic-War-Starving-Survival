using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class PneumaticCarrierCapsuleEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("capsule_serial_number")]
        public string CapsuleSerialNumber { get; set; } = string.Empty;

        [JsonPropertyName("carrier_diameter_mm")]
        public float CarrierDiameterMm { get; set; }

        [JsonPropertyName("transit_velocity_m_s")]
        public float TransitVelocityMS { get; set; }

        [JsonPropertyName("felt_wear_thickness_mm")]
        public float FeltWearThicknessMm { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class PneumaticTubeDiverterEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("diverter_junction_id")]
        public string DiverterJunctionId { get; set; } = string.Empty;

        [JsonPropertyName("diverter_mechanism_type")]
        public string DiverterMechanismType { get; set; } = string.Empty;

        [JsonPropertyName("switching_time_milliseconds")]
        public float SwitchingTimeMilliseconds { get; set; }

        [JsonPropertyName("tube_internal_diameter_mm")]
        public float TubeInternalDiameterMm { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class RootesBlowerVacuumEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("blower_station_id")]
        public string BlowerStationId { get; set; } = string.Empty;

        [JsonPropertyName("rotor_lobe_configuration")]
        public string RotorLobeConfiguration { get; set; } = string.Empty;

        [JsonPropertyName("vacuum_differential_bar")]
        public float VacuumDifferentialBar { get; set; }

        [JsonPropertyName("volumetric_flow_m3_min")]
        public float VolumetricFlowM3Min { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class PneumaticCylinderLeatherEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("actuator_cylinder_id")]
        public string ActuatorCylinderId { get; set; } = string.Empty;

        [JsonPropertyName("packing_leather_type")]
        public string PackingLeatherType { get; set; } = string.Empty;

        [JsonPropertyName("operating_pressure_bar")]
        public float OperatingPressureBar { get; set; }

        [JsonPropertyName("bore_diameter_mm")]
        public float BoreDiameterMm { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class PneumaticTubeDispatchCatalog
    {
        private readonly List<PneumaticCarrierCapsuleEntry> _carrierEntries = new List<PneumaticCarrierCapsuleEntry>();
        private readonly List<PneumaticTubeDiverterEntry> _diverterEntries = new List<PneumaticTubeDiverterEntry>();
        private readonly List<RootesBlowerVacuumEntry> _blowerEntries = new List<RootesBlowerVacuumEntry>();
        private readonly List<PneumaticCylinderLeatherEntry> _cylinderEntries = new List<PneumaticCylinderLeatherEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<PneumaticCarrierCapsuleEntry> CarrierEntries => _carrierEntries;
        public IReadOnlyList<PneumaticTubeDiverterEntry> DiverterEntries => _diverterEntries;
        public IReadOnlyList<RootesBlowerVacuumEntry> BlowerEntries => _blowerEntries;
        public IReadOnlyList<PneumaticCylinderLeatherEntry> CylinderEntries => _cylinderEntries;

        public int TotalCount => _carrierEntries.Count + _diverterEntries.Count + _blowerEntries.Count + _cylinderEntries.Count;

        public static PneumaticTubeDispatchCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new PneumaticTubeDispatchCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Leather Carrier Capsule Wear & Felt Ring Logs
            string carPath = Path.Combine(directoryPath, "pneumatic_carrier_capsule_logs.json");
            if (File.Exists(carPath))
            {
                var list = JsonSerializer.Deserialize<List<PneumaticCarrierCapsuleEntry>>(File.ReadAllText(carPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._carrierEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Brass Tube Junction Switching Diverter Audits
            string divPath = Path.Combine(directoryPath, "pneumatic_tube_diverter_audits.json");
            if (File.Exists(divPath))
            {
                var list = JsonSerializer.Deserialize<List<PneumaticTubeDiverterEntry>>(File.ReadAllText(divPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._diverterEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Rootes Blower Vacuum Differential Pressure Reports
            string blowPath = Path.Combine(directoryPath, "rootes_blower_vacuum_reports.json");
            if (File.Exists(blowPath))
            {
                var list = JsonSerializer.Deserialize<List<RootesBlowerVacuumEntry>>(File.ReadAllText(blowPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._blowerEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Pneumatic Air Cylinder Piston Cup Leather Assays
            string cylPath = Path.Combine(directoryPath, "pneumatic_cylinder_leather_assays.json");
            if (File.Exists(cylPath))
            {
                var list = JsonSerializer.Deserialize<List<PneumaticCylinderLeatherEntry>>(File.ReadAllText(cylPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._cylinderEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public PneumaticCarrierCapsuleEntry GetCarrier(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is PneumaticCarrierCapsuleEntry e ? e : null;
        }

        public PneumaticTubeDiverterEntry GetDiverter(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is PneumaticTubeDiverterEntry e ? e : null;
        }

        public RootesBlowerVacuumEntry GetBlower(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is RootesBlowerVacuumEntry e ? e : null;
        }

        public PneumaticCylinderLeatherEntry GetCylinder(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is PneumaticCylinderLeatherEntry e ? e : null;
        }
    }
}
