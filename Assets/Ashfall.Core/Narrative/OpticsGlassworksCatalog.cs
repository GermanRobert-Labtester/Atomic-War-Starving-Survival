using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class PeriscopePrismDelaminationEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("periscope_assembly_id")]
        public string PeriscopeAssemblyId { get; set; } = string.Empty;

        [JsonPropertyName("optical_glass_type")]
        public string OpticalGlassType { get; set; } = string.Empty;

        [JsonPropertyName("optical_cement_type")]
        public string OpticalCementType { get; set; } = string.Empty;

        [JsonPropertyName("transmission_loss_pct")]
        public float TransmissionLossPct { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class SightGlassThermalShockEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("boiler_system_id")]
        public string BoilerSystemId { get; set; } = string.Empty;

        [JsonPropertyName("glass_composition")]
        public string GlassComposition { get; set; } = string.Empty;

        [JsonPropertyName("operating_pressure_bar")]
        public float OperatingPressureBar { get; set; }

        [JsonPropertyName("failure_classification")]
        public string FailureClassification { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class OpticalCoatingRadBrowningEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("optic_system_id")]
        public string OpticSystemId { get; set; } = string.Empty;

        [JsonPropertyName("substrate_material")]
        public string SubstrateMaterial { get; set; } = string.Empty;

        [JsonPropertyName("accumulated_gamma_dose_rad")]
        public float AccumulatedGammaDoseRad { get; set; }

        [JsonPropertyName("solarization_spectral_band")]
        public string SolarizationSpectralBand { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class ScintillatorAgingEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("detector_unit_id")]
        public string DetectorUnitId { get; set; } = string.Empty;

        [JsonPropertyName("crystal_composition")]
        public string CrystalComposition { get; set; } = string.Empty;

        [JsonPropertyName("quantum_yield_photons_kev")]
        public float QuantumYieldPhotonsKev { get; set; }

        [JsonPropertyName("degradation_mode")]
        public string DegradationMode { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class OpticsGlassworksCatalog
    {
        private readonly List<PeriscopePrismDelaminationEntry> _prismEntries = new List<PeriscopePrismDelaminationEntry>();
        private readonly List<SightGlassThermalShockEntry> _sightGlassEntries = new List<SightGlassThermalShockEntry>();
        private readonly List<OpticalCoatingRadBrowningEntry> _radBrowningEntries = new List<OpticalCoatingRadBrowningEntry>();
        private readonly List<ScintillatorAgingEntry> _scintillatorEntries = new List<ScintillatorAgingEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<PeriscopePrismDelaminationEntry> PrismEntries => _prismEntries;
        public IReadOnlyList<SightGlassThermalShockEntry> SightGlassEntries => _sightGlassEntries;
        public IReadOnlyList<OpticalCoatingRadBrowningEntry> RadBrowningEntries => _radBrowningEntries;
        public IReadOnlyList<ScintillatorAgingEntry> ScintillatorEntries => _scintillatorEntries;

        public int TotalCount => _prismEntries.Count + _sightGlassEntries.Count + _radBrowningEntries.Count + _scintillatorEntries.Count;

        public static OpticsGlassworksCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new OpticsGlassworksCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Periscope Prism Delamination Logs
            string prismPath = Path.Combine(directoryPath, "periscope_prism_delamination_logs.json");
            if (File.Exists(prismPath))
            {
                var list = JsonSerializer.Deserialize<List<PeriscopePrismDelaminationEntry>>(File.ReadAllText(prismPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._prismEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Sight Glass Thermal Shock Logs
            string sightPath = Path.Combine(directoryPath, "borosilicate_sight_glass_thermal_shock.json");
            if (File.Exists(sightPath))
            {
                var list = JsonSerializer.Deserialize<List<SightGlassThermalShockEntry>>(File.ReadAllText(sightPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._sightGlassEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Optical Coating Radiation Browning Reports
            string radPath = Path.Combine(directoryPath, "optical_coating_rad_browning_reports.json");
            if (File.Exists(radPath))
            {
                var list = JsonSerializer.Deserialize<List<OpticalCoatingRadBrowningEntry>>(File.ReadAllText(radPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._radBrowningEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Lead Crystal Scintillator Aging Logs
            string scintPath = Path.Combine(directoryPath, "lead_crystal_scintillator_aging_logs.json");
            if (File.Exists(scintPath))
            {
                var list = JsonSerializer.Deserialize<List<ScintillatorAgingEntry>>(File.ReadAllText(scintPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._scintillatorEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public PeriscopePrismDelaminationEntry GetPrism(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is PeriscopePrismDelaminationEntry e ? e : null;
        }

        public SightGlassThermalShockEntry GetSightGlass(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is SightGlassThermalShockEntry e ? e : null;
        }

        public OpticalCoatingRadBrowningEntry GetRadBrowning(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is OpticalCoatingRadBrowningEntry e ? e : null;
        }

        public ScintillatorAgingEntry GetScintillator(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is ScintillatorAgingEntry e ? e : null;
        }
    }
}
