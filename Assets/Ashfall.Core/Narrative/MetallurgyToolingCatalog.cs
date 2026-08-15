using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class CupolaSlagLeachingEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("furnace_unit_id")]
        public string FurnaceUnitId { get; set; } = string.Empty;

        [JsonPropertyName("melt_temperature_celsius")]
        public float MeltTemperatureCelsius { get; set; }

        [JsonPropertyName("slag_basicity_ratio")]
        public float SlagBasicityRatio { get; set; }

        [JsonPropertyName("metallurgical_defect")]
        public string MetallurgicalDefect { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class CarbideToolWearEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("tool_identifier")]
        public string ToolIdentifier { get; set; } = string.Empty;

        [JsonPropertyName("carbide_grade")]
        public string CarbideGrade { get; set; } = string.Empty;

        [JsonPropertyName("cutting_speed_m_min")]
        public float CuttingSpeedMMin { get; set; }

        [JsonPropertyName("wear_mechanism")]
        public string WearMechanism { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class GearQuenchingFaultEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("gear_component_id")]
        public string GearComponentId { get; set; } = string.Empty;

        [JsonPropertyName("steel_alloy_grade")]
        public string SteelAlloyGrade { get; set; } = string.Empty;

        [JsonPropertyName("case_hardness_hrc")]
        public float CaseHardnessHrc { get; set; }

        [JsonPropertyName("quenching_medium")]
        public string QuenchingMedium { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class BulletAlloyAssayEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("alloy_batch_code")]
        public string AlloyBatchCode { get; set; } = string.Empty;

        [JsonPropertyName("lead_percentage")]
        public float LeadPercentage { get; set; }

        [JsonPropertyName("antimony_percentage")]
        public float AntimonyPercentage { get; set; }

        [JsonPropertyName("brinell_hardness_bhn")]
        public float BrinellHardnessBhn { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class MetallurgyToolingCatalog
    {
        private readonly List<CupolaSlagLeachingEntry> _slagEntries = new List<CupolaSlagLeachingEntry>();
        private readonly List<CarbideToolWearEntry> _toolEntries = new List<CarbideToolWearEntry>();
        private readonly List<GearQuenchingFaultEntry> _gearEntries = new List<GearQuenchingFaultEntry>();
        private readonly List<BulletAlloyAssayEntry> _bulletEntries = new List<BulletAlloyAssayEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<CupolaSlagLeachingEntry> SlagEntries => _slagEntries;
        public IReadOnlyList<CarbideToolWearEntry> ToolEntries => _toolEntries;
        public IReadOnlyList<GearQuenchingFaultEntry> GearEntries => _gearEntries;
        public IReadOnlyList<BulletAlloyAssayEntry> BulletEntries => _bulletEntries;

        public int TotalCount => _slagEntries.Count + _toolEntries.Count + _gearEntries.Count + _bulletEntries.Count;

        public static MetallurgyToolingCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new MetallurgyToolingCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Cupola Furnace Slag Leaching Records
            string slagPath = Path.Combine(directoryPath, "cupola_slag_leaching_records.json");
            if (File.Exists(slagPath))
            {
                var list = JsonSerializer.Deserialize<List<CupolaSlagLeachingEntry>>(File.ReadAllText(slagPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._slagEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Tungsten-Carbide Tool Bit Wear Audits
            string toolPath = Path.Combine(directoryPath, "carbide_tool_wear_audits.json");
            if (File.Exists(toolPath))
            {
                var list = JsonSerializer.Deserialize<List<CarbideToolWearEntry>>(File.ReadAllText(toolPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._toolEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Case-Hardened Gear Quenching Faults
            string gearPath = Path.Combine(directoryPath, "gear_quenching_fault_logs.json");
            if (File.Exists(gearPath))
            {
                var list = JsonSerializer.Deserialize<List<GearQuenchingFaultEntry>>(File.ReadAllText(gearPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._gearEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Lead-Antimony Cast Bullet Alloy Assays
            string bulletPath = Path.Combine(directoryPath, "bullet_alloy_assay_reports.json");
            if (File.Exists(bulletPath))
            {
                var list = JsonSerializer.Deserialize<List<BulletAlloyAssayEntry>>(File.ReadAllText(bulletPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._bulletEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public CupolaSlagLeachingEntry GetSlag(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is CupolaSlagLeachingEntry e ? e : null;
        }

        public CarbideToolWearEntry GetTool(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is CarbideToolWearEntry e ? e : null;
        }

        public GearQuenchingFaultEntry GetGear(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is GearQuenchingFaultEntry e ? e : null;
        }

        public BulletAlloyAssayEntry GetBullet(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is BulletAlloyAssayEntry e ? e : null;
        }
    }
}
