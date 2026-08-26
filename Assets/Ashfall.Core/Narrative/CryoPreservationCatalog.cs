using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class CryoGermplasmViabilityEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("accession_number")]
        public string AccessionNumber { get; set; } = string.Empty;

        [JsonPropertyName("crop_species")]
        public string CropSpecies { get; set; } = string.Empty;

        [JsonPropertyName("storage_temperature_kelvin")]
        public float StorageTemperatureKelvin { get; set; }

        [JsonPropertyName("germination_viability_pct")]
        public float GerminationViabilityPct { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class LiquidNitrogenCompressorEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("cryocooler_unit_id")]
        public string CryocoolerUnitId { get; set; } = string.Empty;

        [JsonPropertyName("working_fluid")]
        public string WorkingFluid { get; set; } = string.Empty;

        [JsonPropertyName("operating_pressure_bar")]
        public float OperatingPressureBar { get; set; }

        [JsonPropertyName("failure_mode")]
        public string FailureMode { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class PermafrostMethaneEruptionEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("vault_geological_sector")]
        public string VaultGeologicalSector { get; set; } = string.Empty;

        [JsonPropertyName("estimated_methane_volume_m3")]
        public float EstimatedMethaneVolumeM3 { get; set; }

        [JsonPropertyName("structural_displacement_cm")]
        public float StructuralDisplacementCm { get; set; }

        [JsonPropertyName("eruption_trigger")]
        public string EruptionTrigger { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class CropGenomeDegradationEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("cultivar_id")]
        public string CultivarId { get; set; } = string.Empty;

        [JsonPropertyName("generation_cycle")]
        public int GenerationCycle { get; set; }

        [JsonPropertyName("mutation_rate_per_megabase")]
        public float MutationRatePerMegabase { get; set; }

        [JsonPropertyName("phenotypic_defect")]
        public string PhenotypicDefect { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class CryoPreservationCatalog
    {
        private readonly List<CryoGermplasmViabilityEntry> _germplasmEntries = new List<CryoGermplasmViabilityEntry>();
        private readonly List<LiquidNitrogenCompressorEntry> _compressorEntries = new List<LiquidNitrogenCompressorEntry>();
        private readonly List<PermafrostMethaneEruptionEntry> _permafrostEntries = new List<PermafrostMethaneEruptionEntry>();
        private readonly List<CropGenomeDegradationEntry> _genomeEntries = new List<CropGenomeDegradationEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<CryoGermplasmViabilityEntry> GermplasmEntries => _germplasmEntries;
        public IReadOnlyList<LiquidNitrogenCompressorEntry> CompressorEntries => _compressorEntries;
        public IReadOnlyList<PermafrostMethaneEruptionEntry> PermafrostEntries => _permafrostEntries;
        public IReadOnlyList<CropGenomeDegradationEntry> GenomeEntries => _genomeEntries;

        public int TotalCount => _germplasmEntries.Count + _compressorEntries.Count + _permafrostEntries.Count + _genomeEntries.Count;

        public static CryoPreservationCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new CryoPreservationCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Cryogenic Germplasm Viability Audits
            string germPath = Path.Combine(directoryPath, "cryo_germplasm_viability_audits.json");
            if (File.Exists(germPath))
            {
                var list = CatalogLocator.LoadWrappedList<CryoGermplasmViabilityEntry>(File.ReadAllText(germPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._germplasmEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Liquid Nitrogen Compressor Failures
            string compPath = Path.Combine(directoryPath, "liquid_nitrogen_compressor_failures.json");
            if (File.Exists(compPath))
            {
                var list = CatalogLocator.LoadWrappedList<LiquidNitrogenCompressorEntry>(File.ReadAllText(compPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._compressorEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Permafrost Vault Methane Eruptions
            string permPath = Path.Combine(directoryPath, "permafrost_methane_eruption_logs.json");
            if (File.Exists(permPath))
            {
                var list = CatalogLocator.LoadWrappedList<PermafrostMethaneEruptionEntry>(File.ReadAllText(permPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._permafrostEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Heirloom Crop Genome Degradation
            string genPath = Path.Combine(directoryPath, "crop_genome_degradation_reports.json");
            if (File.Exists(genPath))
            {
                var list = CatalogLocator.LoadWrappedList<CropGenomeDegradationEntry>(File.ReadAllText(genPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._genomeEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public CryoGermplasmViabilityEntry? GetGermplasm(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is CryoGermplasmViabilityEntry e ? e : null;
        }

        public LiquidNitrogenCompressorEntry? GetCompressor(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is LiquidNitrogenCompressorEntry e ? e : null;
        }

        public PermafrostMethaneEruptionEntry? GetPermafrost(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is PermafrostMethaneEruptionEntry e ? e : null;
        }

        public CropGenomeDegradationEntry? GetGenome(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is CropGenomeDegradationEntry e ? e : null;
        }
    }
}
