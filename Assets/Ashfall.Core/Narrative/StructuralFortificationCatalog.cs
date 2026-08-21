using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class BlastGateAuditEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("gate_designation")]
        public string GateDesignation { get; set; } = string.Empty;

        [JsonPropertyName("mechanical_subsystem")]
        public string MechanicalSubsystem { get; set; } = string.Empty;

        [JsonPropertyName("failure_mode")]
        public string FailureMode { get; set; } = string.Empty;

        [JsonPropertyName("structural_severity")]
        public string StructuralSeverity { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class SumpSiltReportEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("sump_basin_id")]
        public string SumpBasinId { get; set; } = string.Empty;

        [JsonPropertyName("pump_model")]
        public string PumpModel { get; set; } = string.Empty;

        [JsonPropertyName("silt_density_grams_per_liter")]
        public float SiltDensityGramsPerLiter { get; set; }

        [JsonPropertyName("operational_status")]
        public string OperationalStatus { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class LeadWallDegradationEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("wall_sector_id")]
        public string WallSectorId { get; set; } = string.Empty;

        [JsonPropertyName("lead_thickness_mm")]
        public float LeadThicknessMm { get; set; }

        [JsonPropertyName("structural_degradation_mode")]
        public string StructuralDegradationMode { get; set; } = string.Empty;

        [JsonPropertyName("shielding_integrity_pct")]
        public int ShieldingIntegrityPct { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class IntakeFilterClogEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("filter_bank_id")]
        public string FilterBankId { get; set; } = string.Empty;

        [JsonPropertyName("filter_stage")]
        public string FilterStage { get; set; } = string.Empty;

        [JsonPropertyName("differential_pressure_pascals")]
        public float DifferentialPressurePascals { get; set; }

        [JsonPropertyName("airflow_loss_pct")]
        public int AirflowLossPct { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class StructuralFortificationCatalog
    {
        private readonly List<BlastGateAuditEntry> _gateEntries = new List<BlastGateAuditEntry>();
        private readonly List<SumpSiltReportEntry> _siltEntries = new List<SumpSiltReportEntry>();
        private readonly List<LeadWallDegradationEntry> _leadWallEntries = new List<LeadWallDegradationEntry>();
        private readonly List<IntakeFilterClogEntry> _filterEntries = new List<IntakeFilterClogEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<BlastGateAuditEntry> GateEntries => _gateEntries;
        public IReadOnlyList<SumpSiltReportEntry> SiltEntries => _siltEntries;
        public IReadOnlyList<LeadWallDegradationEntry> LeadWallEntries => _leadWallEntries;
        public IReadOnlyList<IntakeFilterClogEntry> FilterEntries => _filterEntries;

        public int TotalCount => _gateEntries.Count + _siltEntries.Count + _leadWallEntries.Count + _filterEntries.Count;

        public static StructuralFortificationCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new StructuralFortificationCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Blast Gate Mechanical Audits
            string gatePath = Path.Combine(directoryPath, "blast_gate_mechanical_audits.json");
            if (File.Exists(gatePath))
            {
                var list = JsonSerializer.Deserialize<List<BlastGateAuditEntry>>(File.ReadAllText(gatePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._gateEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Sump Drainage Silt Reports
            string siltPath = Path.Combine(directoryPath, "sump_drainage_silt_reports.json");
            if (File.Exists(siltPath))
            {
                var list = JsonSerializer.Deserialize<List<SumpSiltReportEntry>>(File.ReadAllText(siltPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._siltEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Lead Wall Degradation Logs
            string leadPath = Path.Combine(directoryPath, "lead_wall_degradation_logs.json");
            if (File.Exists(leadPath))
            {
                var list = JsonSerializer.Deserialize<List<LeadWallDegradationEntry>>(File.ReadAllText(leadPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._leadWallEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Intake Filter Clogging Logs
            string filterPath = Path.Combine(directoryPath, "intake_filter_clogging_logs.json");
            if (File.Exists(filterPath))
            {
                var list = JsonSerializer.Deserialize<List<IntakeFilterClogEntry>>(File.ReadAllText(filterPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._filterEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public BlastGateAuditEntry? GetGateAudit(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is BlastGateAuditEntry e ? e : null;
        }

        public SumpSiltReportEntry? GetSiltReport(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is SumpSiltReportEntry e ? e : null;
        }

        public LeadWallDegradationEntry? GetLeadWall(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is LeadWallDegradationEntry e ? e : null;
        }

        public IntakeFilterClogEntry? GetFilterClog(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is IntakeFilterClogEntry e ? e : null;
        }
    }
}
