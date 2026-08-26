using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class WoodAshLyeHydrometerEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("leaching_vat_id")]
        public string LeachingVatId { get; set; } = string.Empty;

        [JsonPropertyName("feedstock_ash_source")]
        public string FeedstockAshSource { get; set; } = string.Empty;

        [JsonPropertyName("lye_specific_gravity_baume")]
        public float LyeSpecificGravityBaume { get; set; }

        [JsonPropertyName("potassium_hydroxide_concentration_pct")]
        public float PotassiumHydroxideConcentrationPct { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class TallowSaponificationKettleEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("boiling_kettle_id")]
        public string BoilingKettleId { get; set; } = string.Empty;

        [JsonPropertyName("fat_charge_tallow_kg")]
        public float FatChargeTallowKg { get; set; }

        [JsonPropertyName("saponification_temperature_celsius")]
        public float SaponificationTemperatureCelsius { get; set; }

        [JsonPropertyName("grain_curd_yield_kg")]
        public float GrainCurdYieldKg { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class ColdProcessSoapCuringEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("molding_rack_id")]
        public string MoldingRackId { get; set; } = string.Empty;

        [JsonPropertyName("cure_time_days")]
        public float CureTimeDays { get; set; }

        [JsonPropertyName("bar_moisture_content_pct")]
        public float BarMoistureContentPct { get; set; }

        [JsonPropertyName("shore_hardness_durometer")]
        public float ShoreHardnessDurometer { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class SweetWaterGlycerinEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("glycerin_still_id")]
        public string GlycerinStillId { get; set; } = string.Empty;

        [JsonPropertyName("crude_glycerin_concentration_pct")]
        public float CrudeGlycerinConcentrationPct { get; set; }

        [JsonPropertyName("distillation_vacuum_bar")]
        public float DistillationVacuumBar { get; set; }

        [JsonPropertyName("glycerol_purity_pct")]
        public float GlycerolPurityPct { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class SoapSaponificationCatalog
    {
        private readonly List<WoodAshLyeHydrometerEntry> _lyeEntries = new List<WoodAshLyeHydrometerEntry>();
        private readonly List<TallowSaponificationKettleEntry> _tallowEntries = new List<TallowSaponificationKettleEntry>();
        private readonly List<ColdProcessSoapCuringEntry> _curingEntries = new List<ColdProcessSoapCuringEntry>();
        private readonly List<SweetWaterGlycerinEntry> _glycerinEntries = new List<SweetWaterGlycerinEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<WoodAshLyeHydrometerEntry> LyeEntries => _lyeEntries;
        public IReadOnlyList<TallowSaponificationKettleEntry> TallowEntries => _tallowEntries;
        public IReadOnlyList<ColdProcessSoapCuringEntry> CuringEntries => _curingEntries;
        public IReadOnlyList<SweetWaterGlycerinEntry> GlycerinEntries => _glycerinEntries;

        public int TotalCount => _lyeEntries.Count + _tallowEntries.Count + _curingEntries.Count + _glycerinEntries.Count;

        public static SoapSaponificationCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new SoapSaponificationCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Leached Wood-Ash Potash Lye Hydrometer Logs
            string lyePath = Path.Combine(directoryPath, "wood_ash_lye_hydrometer_logs.json");
            if (File.Exists(lyePath))
            {
                var list = CatalogLocator.LoadWrappedList<WoodAshLyeHydrometerEntry>(File.ReadAllText(lyePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._lyeEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Rendered Animal Tallow & Bone Grease Saponification Audits
            string tallowPath = Path.Combine(directoryPath, "tallow_saponification_kettle_audits.json");
            if (File.Exists(tallowPath))
            {
                var list = CatalogLocator.LoadWrappedList<TallowSaponificationKettleEntry>(File.ReadAllText(tallowPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._tallowEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Bunker Cold-Process Hard Bar Soap Curing Reports
            string curingPath = Path.Combine(directoryPath, "cold_process_soap_curing_reports.json");
            if (File.Exists(curingPath))
            {
                var list = CatalogLocator.LoadWrappedList<ColdProcessSoapCuringEntry>(File.ReadAllText(curingPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._curingEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Crude Glycerin Sweet-Water Separation & Distillation Assays
            string glycerinPath = Path.Combine(directoryPath, "sweet_water_glycerin_assays.json");
            if (File.Exists(glycerinPath))
            {
                var list = CatalogLocator.LoadWrappedList<SweetWaterGlycerinEntry>(File.ReadAllText(glycerinPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._glycerinEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public WoodAshLyeHydrometerEntry? GetLye(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is WoodAshLyeHydrometerEntry e ? e : null;
        }

        public TallowSaponificationKettleEntry? GetTallow(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is TallowSaponificationKettleEntry e ? e : null;
        }

        public ColdProcessSoapCuringEntry? GetCuring(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is ColdProcessSoapCuringEntry e ? e : null;
        }

        public SweetWaterGlycerinEntry? GetGlycerin(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is SweetWaterGlycerinEntry e ? e : null;
        }
    }
}
