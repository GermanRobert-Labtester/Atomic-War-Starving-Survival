using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class TimberCreosoteTreatmentEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("treatment_retort_id")]
        public string TreatmentRetortId { get; set; } = string.Empty;

        [JsonPropertyName("wood_species_treated")]
        public string WoodSpeciesTreated { get; set; } = string.Empty;

        [JsonPropertyName("retention_kg_m3")]
        public float RetentionKgM3 { get; set; }

        [JsonPropertyName("penetration_depth_mm")]
        public float PenetrationDepthMm { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class SquareSetShoringEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("stope_location_id")]
        public string StopeLocationId { get; set; } = string.Empty;

        [JsonPropertyName("timber_framing_system")]
        public string TimberFramingSystem { get; set; } = string.Empty;

        [JsonPropertyName("measured_rock_pressure_mpa")]
        public float MeasuredRockPressureMpa { get; set; }

        [JsonPropertyName("set_deflection_mm")]
        public float SetDeflectionMm { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class TimberDryRotFruitingEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("infestation_site_id")]
        public string InfestationSiteId { get; set; } = string.Empty;

        [JsonPropertyName("fungal_species_identified")]
        public string FungalSpeciesIdentified { get; set; } = string.Empty;

        [JsonPropertyName("timber_moisture_content_pct")]
        public float TimberMoistureContentPct { get; set; }

        [JsonPropertyName("affected_area_sq_meters")]
        public float AffectedAreaSqMeters { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class MortiseTenonFailureEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("framing_assembly_id")]
        public string FramingAssemblyId { get; set; } = string.Empty;

        [JsonPropertyName("joint_geometry_type")]
        public string JointGeometryType { get; set; } = string.Empty;

        [JsonPropertyName("peg_material_species")]
        public string PegMaterialSpecies { get; set; } = string.Empty;

        [JsonPropertyName("joint_load_kilonewtons")]
        public float JointLoadKilonewtons { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class TimberCarpentryCatalog
    {
        private readonly List<TimberCreosoteTreatmentEntry> _treatmentEntries = new List<TimberCreosoteTreatmentEntry>();
        private readonly List<SquareSetShoringEntry> _shoringEntries = new List<SquareSetShoringEntry>();
        private readonly List<TimberDryRotFruitingEntry> _rotEntries = new List<TimberDryRotFruitingEntry>();
        private readonly List<MortiseTenonFailureEntry> _mortiseEntries = new List<MortiseTenonFailureEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<TimberCreosoteTreatmentEntry> TreatmentEntries => _treatmentEntries;
        public IReadOnlyList<SquareSetShoringEntry> ShoringEntries => _shoringEntries;
        public IReadOnlyList<TimberDryRotFruitingEntry> RotEntries => _rotEntries;
        public IReadOnlyList<MortiseTenonFailureEntry> MortiseEntries => _mortiseEntries;

        public int TotalCount => _treatmentEntries.Count + _shoringEntries.Count + _rotEntries.Count + _mortiseEntries.Count;

        public static TimberCarpentryCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new TimberCarpentryCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Creosote Pressure-Treatment Pit Logs
            string treatPath = Path.Combine(directoryPath, "timber_creosote_treatment_logs.json");
            if (File.Exists(treatPath))
            {
                var list = CatalogLocator.LoadWrappedList<TimberCreosoteTreatmentEntry>(File.ReadAllText(treatPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._treatmentEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Square-Set Mine Shoring Load Deflection Audits
            string shorePath = Path.Combine(directoryPath, "square_set_shoring_audits.json");
            if (File.Exists(shorePath))
            {
                var list = CatalogLocator.LoadWrappedList<SquareSetShoringEntry>(File.ReadAllText(shorePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._shoringEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Dry-Rot Fruiting Body Records
            string rotPath = Path.Combine(directoryPath, "timber_dry_rot_fruiting_records.json");
            if (File.Exists(rotPath))
            {
                var list = CatalogLocator.LoadWrappedList<TimberDryRotFruitingEntry>(File.ReadAllText(rotPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._rotEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Mortise & Tenon Joint Compression Failure Reports
            string mortPath = Path.Combine(directoryPath, "mortise_tenon_failure_reports.json");
            if (File.Exists(mortPath))
            {
                var list = CatalogLocator.LoadWrappedList<MortiseTenonFailureEntry>(File.ReadAllText(mortPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._mortiseEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public TimberCreosoteTreatmentEntry? GetTreatment(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is TimberCreosoteTreatmentEntry e ? e : null;
        }

        public SquareSetShoringEntry? GetShoring(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is SquareSetShoringEntry e ? e : null;
        }

        public TimberDryRotFruitingEntry? GetRot(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is TimberDryRotFruitingEntry e ? e : null;
        }

        public MortiseTenonFailureEntry? GetMortise(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is MortiseTenonFailureEntry e ? e : null;
        }
    }
}
