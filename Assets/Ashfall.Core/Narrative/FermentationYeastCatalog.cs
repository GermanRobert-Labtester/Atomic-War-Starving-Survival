using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class SourdoughMotherAcidityEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("starter_crock_id")]
        public string StarterCrockId { get; set; } = string.Empty;

        [JsonPropertyName("microbial_consortium_type")]
        public string MicrobialConsortiumType { get; set; } = string.Empty;

        [JsonPropertyName("culture_ph_level")]
        public float CulturePhLevel { get; set; }

        [JsonPropertyName("lactic_to_acetic_acid_ratio")]
        public float LacticToAceticAcidRatio { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class BrewersYeastKrausenEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("fermentation_tun_id")]
        public string FermentationTunId { get; set; } = string.Empty;

        [JsonPropertyName("yeast_strain_designation")]
        public string YeastStrainDesignation { get; set; } = string.Empty;

        [JsonPropertyName("apparent_attenuation_pct")]
        public float ApparentAttenuationPct { get; set; }

        [JsonPropertyName("fermentation_temperature_celsius")]
        public float FermentationTemperatureCelsius { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class SilageLacticPitEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("silage_trench_id")]
        public string SilageTrenchId { get; set; } = string.Empty;

        [JsonPropertyName("forage_substrate_crop")]
        public string ForageSubstrateCrop { get; set; } = string.Empty;

        [JsonPropertyName("pit_fermentation_ph")]
        public float PitFermentationPh { get; set; }

        [JsonPropertyName("butyric_acid_concentration_pct")]
        public float ButyricAcidConcentrationPct { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class FermentationCrockAirlockEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("stoneware_crock_id")]
        public string StonewareCrockId { get; set; } = string.Empty;

        [JsonPropertyName("crock_volume_liters")]
        public float CrockVolumeLiters { get; set; }

        [JsonPropertyName("brine_salinity_pct")]
        public float BrineSalinityPct { get; set; }

        [JsonPropertyName("airlock_water_loss_ml_per_day")]
        public float AirlockWaterLossMlPerDay { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class FermentationYeastCatalog
    {
        private readonly List<SourdoughMotherAcidityEntry> _sourdoughEntries = new List<SourdoughMotherAcidityEntry>();
        private readonly List<BrewersYeastKrausenEntry> _brewingEntries = new List<BrewersYeastKrausenEntry>();
        private readonly List<SilageLacticPitEntry> _silageEntries = new List<SilageLacticPitEntry>();
        private readonly List<FermentationCrockAirlockEntry> _crockEntries = new List<FermentationCrockAirlockEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<SourdoughMotherAcidityEntry> SourdoughEntries => _sourdoughEntries;
        public IReadOnlyList<BrewersYeastKrausenEntry> BrewingEntries => _brewingEntries;
        public IReadOnlyList<SilageLacticPitEntry> SilageEntries => _silageEntries;
        public IReadOnlyList<FermentationCrockAirlockEntry> CrockEntries => _crockEntries;

        public int TotalCount => _sourdoughEntries.Count + _brewingEntries.Count + _silageEntries.Count + _crockEntries.Count;

        public static FermentationYeastCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new FermentationYeastCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Sourdough Wild Yeast Mother Culture Acidity Logs
            string sourdoughPath = Path.Combine(directoryPath, "sourdough_mother_acidity_logs.json");
            if (File.Exists(sourdoughPath))
            {
                var list = JsonSerializer.Deserialize<List<SourdoughMotherAcidityEntry>>(File.ReadAllText(sourdoughPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._sourdoughEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Top-Fermenting Brewer's Yeast Krausen Ring Audits
            string brewingPath = Path.Combine(directoryPath, "brewers_yeast_krausen_audits.json");
            if (File.Exists(brewingPath))
            {
                var list = JsonSerializer.Deserialize<List<BrewersYeastKrausenEntry>>(File.ReadAllText(brewingPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._brewingEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Anaerobic Silage Lactic Fermentation Pit Reports
            string silagePath = Path.Combine(directoryPath, "silage_lactic_pit_reports.json");
            if (File.Exists(silagePath))
            {
                var list = JsonSerializer.Deserialize<List<SilageLacticPitEntry>>(File.ReadAllText(silagePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._silageEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Stoneware Fermentation Crock Water-Airlock Seal Assays
            string crockPath = Path.Combine(directoryPath, "fermentation_crock_airlock_assays.json");
            if (File.Exists(crockPath))
            {
                var list = JsonSerializer.Deserialize<List<FermentationCrockAirlockEntry>>(File.ReadAllText(crockPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._crockEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public SourdoughMotherAcidityEntry? GetSourdough(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is SourdoughMotherAcidityEntry e ? e : null;
        }

        public BrewersYeastKrausenEntry? GetBrewing(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is BrewersYeastKrausenEntry e ? e : null;
        }

        public SilageLacticPitEntry? GetSilage(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is SilageLacticPitEntry e ? e : null;
        }

        public FermentationCrockAirlockEntry? GetCrock(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is FermentationCrockAirlockEntry e ? e : null;
        }
    }
}
