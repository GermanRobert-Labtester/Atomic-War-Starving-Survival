using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class CharcoalMoundPyrolysisEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("coaling_mound_id")]
        public string CoalingMoundId { get; set; } = string.Empty;

        [JsonPropertyName("feedstock_timber_species")]
        public string FeedstockTimberSpecies { get; set; } = string.Empty;

        [JsonPropertyName("pyrolysis_peak_temperature_celsius")]
        public float PyrolysisPeakTemperatureCelsius { get; set; }

        [JsonPropertyName("charcoal_gravimetric_yield_pct")]
        public float CharcoalGravimetricYieldPct { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class RetortWoodVinegarEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("retort_vessel_id")]
        public string RetortVesselId { get; set; } = string.Empty;

        [JsonPropertyName("condensed_liquor_volume_liters")]
        public float CondensedLiquorVolumeLiters { get; set; }

        [JsonPropertyName("pyroligneous_acetic_acid_pct")]
        public float PyroligneousAceticAcidPct { get; set; }

        [JsonPropertyName("settled_wood_tar_volume_liters")]
        public float SettledWoodTarVolumeLiters { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class BiocharCationExchangeEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("soil_amendment_lot_id")]
        public string SoilAmendmentLotId { get; set; } = string.Empty;

        [JsonPropertyName("biochar_bet_surface_area_m2_per_g")]
        public float BiocharBetSurfaceAreaM2PerG { get; set; }

        [JsonPropertyName("cation_exchange_capacity_meq")]
        public float CationExchangeCapacityMeq { get; set; }

        [JsonPropertyName("soil_ph_buffered_level")]
        public float SoilPhBufferedLevel { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class ForgeCharcoalAshEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("carbon_assay_batch_id")]
        public string CarbonAssayBatchId { get; set; } = string.Empty;

        [JsonPropertyName("fixed_carbon_pct")]
        public float FixedCarbonPct { get; set; }

        [JsonPropertyName("ash_content_pct")]
        public float AshContentPct { get; set; }

        [JsonPropertyName("forge_hearth_peak_temp_celsius")]
        public float ForgeHearthPeakTempCelsius { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class CharcoalPyrolysisCatalog
    {
        private readonly List<CharcoalMoundPyrolysisEntry> _moundEntries = new List<CharcoalMoundPyrolysisEntry>();
        private readonly List<RetortWoodVinegarEntry> _retortEntries = new List<RetortWoodVinegarEntry>();
        private readonly List<BiocharCationExchangeEntry> _biocharEntries = new List<BiocharCationExchangeEntry>();
        private readonly List<ForgeCharcoalAshEntry> _forgeEntries = new List<ForgeCharcoalAshEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<CharcoalMoundPyrolysisEntry> MoundEntries => _moundEntries;
        public IReadOnlyList<RetortWoodVinegarEntry> RetortEntries => _retortEntries;
        public IReadOnlyList<BiocharCationExchangeEntry> BiocharEntries => _biocharEntries;
        public IReadOnlyList<ForgeCharcoalAshEntry> ForgeEntries => _forgeEntries;

        public int TotalCount => _moundEntries.Count + _retortEntries.Count + _biocharEntries.Count + _forgeEntries.Count;

        public static CharcoalPyrolysisCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new CharcoalPyrolysisCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Earth Mound Charcoal Pit Pyrolysis Logs
            string moundPath = Path.Combine(directoryPath, "charcoal_mound_pyrolysis_logs.json");
            if (File.Exists(moundPath))
            {
                var list = JsonSerializer.Deserialize<List<CharcoalMoundPyrolysisEntry>>(File.ReadAllText(moundPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._moundEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Sealed Steel Retort Wood Vinegar & Tar Condensation Audits
            string retortPath = Path.Combine(directoryPath, "retort_wood_vinegar_audits.json");
            if (File.Exists(retortPath))
            {
                var list = JsonSerializer.Deserialize<List<RetortWoodVinegarEntry>>(File.ReadAllText(retortPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._retortEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Subterranean Biochar Soil Inoculation & Cation Exchange Reports
            string biocharPath = Path.Combine(directoryPath, "biochar_cation_exchange_reports.json");
            if (File.Exists(biocharPath))
            {
                var list = JsonSerializer.Deserialize<List<BiocharCationExchangeEntry>>(File.ReadAllText(biocharPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._biocharEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Hardwood Forge Lump Carbon Density & Ash Content Assays
            string forgePath = Path.Combine(directoryPath, "forge_charcoal_ash_assays.json");
            if (File.Exists(forgePath))
            {
                var list = JsonSerializer.Deserialize<List<ForgeCharcoalAshEntry>>(File.ReadAllText(forgePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._forgeEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public CharcoalMoundPyrolysisEntry? GetMound(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is CharcoalMoundPyrolysisEntry e ? e : null;
        }

        public RetortWoodVinegarEntry? GetRetort(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is RetortWoodVinegarEntry e ? e : null;
        }

        public BiocharCationExchangeEntry? GetBiochar(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is BiocharCationExchangeEntry e ? e : null;
        }

        public ForgeCharcoalAshEntry? GetForge(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is ForgeCharcoalAshEntry e ? e : null;
        }
    }
}
