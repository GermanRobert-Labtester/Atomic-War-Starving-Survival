using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class CryoSeedAmpouleEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("dewar_canister_id")]
        public string DewarCanisterId { get; set; } = string.Empty;

        [JsonPropertyName("crop_botanical_species")]
        public string CropBotanicalSpecies { get; set; } = string.Empty;

        [JsonPropertyName("storage_temperature_celsius")]
        public float StorageTemperatureCelsius { get; set; }

        [JsonPropertyName("seed_moisture_content_pct")]
        public float SeedMoistureContentPct { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class RagdollGerminationEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("germination_tray_id")]
        public string GerminationTrayId { get; set; } = string.Empty;

        [JsonPropertyName("crop_cultivar_name")]
        public string CropCultivarName { get; set; } = string.Empty;

        [JsonPropertyName("seeds_tested_count")]
        public float SeedsTestedCount { get; set; }

        [JsonPropertyName("germination_viability_pct")]
        public float GerminationViabilityPct { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class SilicaGelSeedDesiccationEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("storage_vessel_id")]
        public string StorageVesselId { get; set; } = string.Empty;

        [JsonPropertyName("desiccant_compound_type")]
        public string DesiccantCompoundType { get; set; } = string.Empty;

        [JsonPropertyName("seed_batch_moisture_pct")]
        public float SeedBatchMoisturePct { get; set; }

        [JsonPropertyName("equilibrium_rh_pct")]
        public float EquilibriumRhPct { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class HeirloomSeedViabilityEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("landrace_variety_id")]
        public string LandraceVarietyId { get; set; } = string.Empty;

        [JsonPropertyName("generation_cycle_number")]
        public float GenerationCycleNumber { get; set; }

        [JsonPropertyName("parent_population_size")]
        public float ParentPopulationSize { get; set; }

        [JsonPropertyName("phenotype_degeneration_pct")]
        public float PhenotypeDegenerationPct { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class SeedBankPreservationCatalog
    {
        private readonly List<CryoSeedAmpouleEntry> _cryoEntries = new List<CryoSeedAmpouleEntry>();
        private readonly List<RagdollGerminationEntry> _ragdollEntries = new List<RagdollGerminationEntry>();
        private readonly List<SilicaGelSeedDesiccationEntry> _silicaEntries = new List<SilicaGelSeedDesiccationEntry>();
        private readonly List<HeirloomSeedViabilityEntry> _heirloomEntries = new List<HeirloomSeedViabilityEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<CryoSeedAmpouleEntry> CryoEntries => _cryoEntries;
        public IReadOnlyList<RagdollGerminationEntry> RagdollEntries => _ragdollEntries;
        public IReadOnlyList<SilicaGelSeedDesiccationEntry> SilicaEntries => _silicaEntries;
        public IReadOnlyList<HeirloomSeedViabilityEntry> HeirloomEntries => _heirloomEntries;

        public int TotalCount => _cryoEntries.Count + _ragdollEntries.Count + _silicaEntries.Count + _heirloomEntries.Count;

        public static SeedBankPreservationCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new SeedBankPreservationCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Cryogenic Liquid Nitrogen Seed Ampoule Preservation Logs
            string cryoPath = Path.Combine(directoryPath, "cryo_seed_ampoule_logs.json");
            if (File.Exists(cryoPath))
            {
                var list = CatalogLocator.LoadWrappedList<CryoSeedAmpouleEntry>(File.ReadAllText(cryoPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._cryoEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Petri Dish Ragdoll Germination Rate Assays
            string ragdollPath = Path.Combine(directoryPath, "ragdoll_germination_assays.json");
            if (File.Exists(ragdollPath))
            {
                var list = CatalogLocator.LoadWrappedList<RagdollGerminationEntry>(File.ReadAllText(ragdollPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._ragdollEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Hermetic Glass Jar Silica Gel Desiccation Audits
            string silicaPath = Path.Combine(directoryPath, "silica_gel_seed_desiccation_audits.json");
            if (File.Exists(silicaPath))
            {
                var list = CatalogLocator.LoadWrappedList<SilicaGelSeedDesiccationEntry>(File.ReadAllText(silicaPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._silicaEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Heirloom Landrace Seed Viability & Phenotype Degeneration Reports
            string heirloomPath = Path.Combine(directoryPath, "heirloom_seed_viability_reports.json");
            if (File.Exists(heirloomPath))
            {
                var list = CatalogLocator.LoadWrappedList<HeirloomSeedViabilityEntry>(File.ReadAllText(heirloomPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._heirloomEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public CryoSeedAmpouleEntry? GetCryo(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is CryoSeedAmpouleEntry e ? e : null;
        }

        public RagdollGerminationEntry? GetRagdoll(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is RagdollGerminationEntry e ? e : null;
        }

        public SilicaGelSeedDesiccationEntry? GetSilica(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is SilicaGelSeedDesiccationEntry e ? e : null;
        }

        public HeirloomSeedViabilityEntry? GetHeirloom(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is HeirloomSeedViabilityEntry e ? e : null;
        }
    }
}
