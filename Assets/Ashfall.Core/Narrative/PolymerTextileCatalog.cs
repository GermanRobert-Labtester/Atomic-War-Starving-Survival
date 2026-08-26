using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class NeopreneGasketDegradationEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("mask_model_designation")]
        public string MaskModelDesignation { get; set; } = string.Empty;

        [JsonPropertyName("elastomer_polymer_type")]
        public string ElastomerPolymerType { get; set; } = string.Empty;

        [JsonPropertyName("ozone_exposure_ppm")]
        public float OzoneExposurePpm { get; set; }

        [JsonPropertyName("degradation_severity")]
        public string DegradationSeverity { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class AramidFiberRotEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("armor_item_id")]
        public string ArmorItemId { get; set; } = string.Empty;

        [JsonPropertyName("aramid_yarn_type")]
        public string AramidYarnType { get; set; } = string.Empty;

        [JsonPropertyName("residual_tensile_strength_pct")]
        public float ResidualTensileStrengthPct { get; set; }

        [JsonPropertyName("failure_phenomenon")]
        public string FailurePhenomenon { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class TireRetreadCompoundEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("tire_casing_id")]
        public string TireCasingId { get; set; } = string.Empty;

        [JsonPropertyName("rubber_compound_formula")]
        public string RubberCompoundFormula { get; set; } = string.Empty;

        [JsonPropertyName("vulcanization_temp_celsius")]
        public float VulcanizationTempCelsius { get; set; }

        [JsonPropertyName("road_wear_rating")]
        public string RoadWearRating { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class CelluloidFilmDecompositionEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("film_archive_reel_id")]
        public string FilmArchiveReelId { get; set; } = string.Empty;

        [JsonPropertyName("polymer_base_chemistry")]
        public string PolymerBaseChemistry { get; set; } = string.Empty;

        [JsonPropertyName("decomposition_stage")]
        public string DecompositionStage { get; set; } = string.Empty;

        [JsonPropertyName("combustion_temperature_celsius")]
        public float CombustionTemperatureCelsius { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class PolymerTextileCatalog
    {
        private readonly List<NeopreneGasketDegradationEntry> _gasketEntries = new List<NeopreneGasketDegradationEntry>();
        private readonly List<AramidFiberRotEntry> _aramidEntries = new List<AramidFiberRotEntry>();
        private readonly List<TireRetreadCompoundEntry> _tireEntries = new List<TireRetreadCompoundEntry>();
        private readonly List<CelluloidFilmDecompositionEntry> _filmEntries = new List<CelluloidFilmDecompositionEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<NeopreneGasketDegradationEntry> GasketEntries => _gasketEntries;
        public IReadOnlyList<AramidFiberRotEntry> AramidEntries => _aramidEntries;
        public IReadOnlyList<TireRetreadCompoundEntry> TireEntries => _tireEntries;
        public IReadOnlyList<CelluloidFilmDecompositionEntry> FilmEntries => _filmEntries;

        public int TotalCount => _gasketEntries.Count + _aramidEntries.Count + _tireEntries.Count + _filmEntries.Count;

        public static PolymerTextileCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new PolymerTextileCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Neoprene Gasket Degradation Logs
            string gasketPath = Path.Combine(directoryPath, "neoprene_gasket_degradation_logs.json");
            if (File.Exists(gasketPath))
            {
                var list = CatalogLocator.LoadWrappedList<NeopreneGasketDegradationEntry>(File.ReadAllText(gasketPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._gasketEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Para-Aramid Ballistic Fiber Rot Reports
            string aramidPath = Path.Combine(directoryPath, "aramid_fiber_rot_reports.json");
            if (File.Exists(aramidPath))
            {
                var list = CatalogLocator.LoadWrappedList<AramidFiberRotEntry>(File.ReadAllText(aramidPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._aramidEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Vulcanized Tire Tread Re-Treading Logs
            string tirePath = Path.Combine(directoryPath, "tire_retreading_compound_logs.json");
            if (File.Exists(tirePath))
            {
                var list = CatalogLocator.LoadWrappedList<TireRetreadCompoundEntry>(File.ReadAllText(tirePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._tireEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Celluloid Film Base Spontaneous Decomposition
            string filmPath = Path.Combine(directoryPath, "celluloid_film_decomposition_records.json");
            if (File.Exists(filmPath))
            {
                var list = CatalogLocator.LoadWrappedList<CelluloidFilmDecompositionEntry>(File.ReadAllText(filmPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._filmEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public NeopreneGasketDegradationEntry? GetGasket(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is NeopreneGasketDegradationEntry e ? e : null;
        }

        public AramidFiberRotEntry? GetAramid(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is AramidFiberRotEntry e ? e : null;
        }

        public TireRetreadCompoundEntry? GetTire(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is TireRetreadCompoundEntry e ? e : null;
        }

        public CelluloidFilmDecompositionEntry? GetFilm(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is CelluloidFilmDecompositionEntry e ? e : null;
        }
    }
}
