using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class LimeKilnCalcinationEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("kiln_structure_id")]
        public string KilnStructureId { get; set; } = string.Empty;

        [JsonPropertyName("feedstock_stone_type")]
        public string FeedstockStoneType { get; set; } = string.Empty;

        [JsonPropertyName("calcination_temp_celsius")]
        public float CalcinationTempCelsius { get; set; }

        [JsonPropertyName("quicklime_yield_tons")]
        public float QuicklimeYieldTons { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class PozzolanMortarRecipeEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("mortar_recipe_code")]
        public string MortarRecipeCode { get; set; } = string.Empty;

        [JsonPropertyName("pozzolanic_source")]
        public string PozzolanicSource { get; set; } = string.Empty;

        [JsonPropertyName("compressive_strength_mpa")]
        public float CompressiveStrengthMpa { get; set; }

        [JsonPropertyName("curing_environment")]
        public string CuringEnvironment { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class RefractoryFirebrickSpallEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("furnace_zone_id")]
        public string FurnaceZoneId { get; set; } = string.Empty;

        [JsonPropertyName("refractory_brick_grade")]
        public string RefractoryBrickGrade { get; set; } = string.Empty;

        [JsonPropertyName("operating_temperature_celsius")]
        public float OperatingTemperatureCelsius { get; set; }

        [JsonPropertyName("failure_mechanism")]
        public string FailureMechanism { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class MudbrickWeatheringEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("adobe_batch_identifier")]
        public string AdobeBatchIdentifier { get; set; } = string.Empty;

        [JsonPropertyName("reinforcement_fiber_type")]
        public string ReinforcementFiberType { get; set; } = string.Empty;

        [JsonPropertyName("clay_to_sand_ratio")]
        public string ClayToSandRatio { get; set; } = string.Empty;

        [JsonPropertyName("wet_compressive_strength_mpa")]
        public float WetCompressiveStrengthMpa { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class MasonryBrickworksCatalog
    {
        private readonly List<LimeKilnCalcinationEntry> _kilnEntries = new List<LimeKilnCalcinationEntry>();
        private readonly List<PozzolanMortarRecipeEntry> _mortarEntries = new List<PozzolanMortarRecipeEntry>();
        private readonly List<RefractoryFirebrickSpallEntry> _refractoryEntries = new List<RefractoryFirebrickSpallEntry>();
        private readonly List<MudbrickWeatheringEntry> _adobeEntries = new List<MudbrickWeatheringEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<LimeKilnCalcinationEntry> KilnEntries => _kilnEntries;
        public IReadOnlyList<PozzolanMortarRecipeEntry> MortarEntries => _mortarEntries;
        public IReadOnlyList<RefractoryFirebrickSpallEntry> RefractoryEntries => _refractoryEntries;
        public IReadOnlyList<MudbrickWeatheringEntry> AdobeEntries => _adobeEntries;

        public int TotalCount => _kilnEntries.Count + _mortarEntries.Count + _refractoryEntries.Count + _adobeEntries.Count;

        public static MasonryBrickworksCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new MasonryBrickworksCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Vertical Shaft Lime Kiln Calcination Logs
            string kilnPath = Path.Combine(directoryPath, "lime_kiln_calcination_logs.json");
            if (File.Exists(kilnPath))
            {
                var list = CatalogLocator.LoadWrappedList<LimeKilnCalcinationEntry>(File.ReadAllText(kilnPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._kilnEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Roman Pozzolanic Ash Mortar Formulations
            string mortarPath = Path.Combine(directoryPath, "pozzolan_mortar_formulations.json");
            if (File.Exists(mortarPath))
            {
                var list = CatalogLocator.LoadWrappedList<PozzolanMortarRecipeEntry>(File.ReadAllText(mortarPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._mortarEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Refractory Firebrick Slag Spalling Records
            string refPath = Path.Combine(directoryPath, "refractory_firebrick_spalling_logs.json");
            if (File.Exists(refPath))
            {
                var list = CatalogLocator.LoadWrappedList<RefractoryFirebrickSpallEntry>(File.ReadAllText(refPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._refractoryEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Adobe Straw Mudbrick Weathering Assays
            string adobePath = Path.Combine(directoryPath, "mudbrick_weathering_assays.json");
            if (File.Exists(adobePath))
            {
                var list = CatalogLocator.LoadWrappedList<MudbrickWeatheringEntry>(File.ReadAllText(adobePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._adobeEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public LimeKilnCalcinationEntry? GetKiln(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is LimeKilnCalcinationEntry e ? e : null;
        }

        public PozzolanMortarRecipeEntry? GetMortar(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is PozzolanMortarRecipeEntry e ? e : null;
        }

        public RefractoryFirebrickSpallEntry? GetRefractory(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is RefractoryFirebrickSpallEntry e ? e : null;
        }

        public MudbrickWeatheringEntry? GetAdobe(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is MudbrickWeatheringEntry e ? e : null;
        }
    }
}
