using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class HempFiberHacklingEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("retting_floor_id")]
        public string RettingFloorId { get; set; } = string.Empty;

        [JsonPropertyName("raw_stalk_crop_origin")]
        public string RawStalkCropOrigin { get; set; } = string.Empty;

        [JsonPropertyName("retting_duration_days")]
        public float RettingDurationDays { get; set; }

        [JsonPropertyName("fiber_tensile_tenacity_cn_tex")]
        public float FiberTensileTenacityCnTex { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class WireRopeStrandingEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("cable_spool_identifier")]
        public string CableSpoolIdentifier { get; set; } = string.Empty;

        [JsonPropertyName("wire_rope_construction")]
        public string WireRopeConstruction { get; set; } = string.Empty;

        [JsonPropertyName("nominal_diameter_mm")]
        public float NominalDiameterMm { get; set; }

        [JsonPropertyName("breaking_strength_metric_tons")]
        public float BreakingStrengthMetricTons { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class ManilaHawserBreakageEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("hawser_coil_id")]
        public string HawserCoilId { get; set; } = string.Empty;

        [JsonPropertyName("fiber_botanical_origin")]
        public string FiberBotanicalOrigin { get; set; } = string.Empty;

        [JsonPropertyName("rope_diameter_inches")]
        public float RopeDiameterInches { get; set; }

        [JsonPropertyName("tensile_break_load_kn")]
        public float TensileBreakLoadKn { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class RopeTransmissionSplicingEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("drive_line_shaft_id")]
        public string DriveLineShaftId { get; set; } = string.Empty;

        [JsonPropertyName("rope_drive_system")]
        public string RopeDriveSystem { get; set; } = string.Empty;

        [JsonPropertyName("transmitted_power_kilowatts")]
        public float TransmittedPowerKilowatts { get; set; }

        [JsonPropertyName("splice_length_diameters")]
        public float SpliceLengthDiameters { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class CordageCableCatalog
    {
        private readonly List<HempFiberHacklingEntry> _hempEntries = new List<HempFiberHacklingEntry>();
        private readonly List<WireRopeStrandingEntry> _wireEntries = new List<WireRopeStrandingEntry>();
        private readonly List<ManilaHawserBreakageEntry> _hawserEntries = new List<ManilaHawserBreakageEntry>();
        private readonly List<RopeTransmissionSplicingEntry> _spliceEntries = new List<RopeTransmissionSplicingEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<HempFiberHacklingEntry> HempEntries => _hempEntries;
        public IReadOnlyList<WireRopeStrandingEntry> WireEntries => _wireEntries;
        public IReadOnlyList<ManilaHawserBreakageEntry> HawserEntries => _hawserEntries;
        public IReadOnlyList<RopeTransmissionSplicingEntry> SpliceEntries => _spliceEntries;

        public int TotalCount => _hempEntries.Count + _wireEntries.Count + _hawserEntries.Count + _spliceEntries.Count;

        public static CordageCableCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new CordageCableCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Hemp Fiber Hackling & Dew-Retting Logs
            string hempPath = Path.Combine(directoryPath, "hemp_fiber_hackling_logs.json");
            if (File.Exists(hempPath))
            {
                var list = CatalogLocator.LoadWrappedList<HempFiberHacklingEntry>(File.ReadAllText(hempPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._hempEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Steel Wire Stranding & Core Lubrication Assays
            string wirePath = Path.Combine(directoryPath, "wire_rope_stranding_assays.json");
            if (File.Exists(wirePath))
            {
                var list = CatalogLocator.LoadWrappedList<WireRopeStrandingEntry>(File.ReadAllText(wirePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._wireEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Manila Hawser Tensile Breakage Reports
            string hawserPath = Path.Combine(directoryPath, "manila_hawser_breakage_reports.json");
            if (File.Exists(hawserPath))
            {
                var list = CatalogLocator.LoadWrappedList<ManilaHawserBreakageEntry>(File.ReadAllText(hawserPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._hawserEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Endless Transmission Rope Splicing Audits
            string splicePath = Path.Combine(directoryPath, "rope_transmission_splicing_audits.json");
            if (File.Exists(splicePath))
            {
                var list = CatalogLocator.LoadWrappedList<RopeTransmissionSplicingEntry>(File.ReadAllText(splicePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._spliceEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public HempFiberHacklingEntry? GetHemp(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is HempFiberHacklingEntry e ? e : null;
        }

        public WireRopeStrandingEntry? GetWire(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is WireRopeStrandingEntry e ? e : null;
        }

        public ManilaHawserBreakageEntry? GetHawser(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is ManilaHawserBreakageEntry e ? e : null;
        }

        public RopeTransmissionSplicingEntry? GetSplice(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is RopeTransmissionSplicingEntry e ? e : null;
        }
    }
}
