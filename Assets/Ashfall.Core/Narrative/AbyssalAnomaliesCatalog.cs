// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class HydrophoneAcousticEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("buoy_callsign")]
        public string BuoyCallsign { get; set; } = string.Empty;

        [JsonPropertyName("acoustic_frequency_hz")]
        public float AcousticFrequencyHz { get; set; }

        [JsonPropertyName("depth_meters")]
        public int DepthMeters { get; set; }

        [JsonPropertyName("signal_classification")]
        public string SignalClassification { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("signal_amplitude_db")]
        public float SignalAmplitudeDb { get; set; }

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class GeothermalBoreholeEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("borehole_id")]
        public string BoreholeId { get; set; } = string.Empty;

        [JsonPropertyName("depth_meters")]
        public int DepthMeters { get; set; }

        [JsonPropertyName("temperature_celsius")]
        public float TemperatureCelsius { get; set; }

        [JsonPropertyName("casing_pressure_bar")]
        public float CasingPressureBar { get; set; }

        [JsonPropertyName("geological_formation")]
        public string GeologicalFormation { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class CryopodFailureEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("pod_id")]
        public string PodId { get; set; } = string.Empty;

        [JsonPropertyName("subject_designation")]
        public string SubjectDesignation { get; set; } = string.Empty;

        [JsonPropertyName("core_temperature_kelvin")]
        public float CoreTemperatureKelvin { get; set; }

        [JsonPropertyName("chamber_pressure_kpa")]
        public float ChamberPressureKpa { get; set; }

        [JsonPropertyName("system_alert")]
        public string SystemAlert { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class SaltMineInscriptionEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("mine_gallery")]
        public string MineGallery { get; set; } = string.Empty;

        [JsonPropertyName("rock_medium")]
        public string RockMedium { get; set; } = string.Empty;

        [JsonPropertyName("inscription_tool")]
        public string InscriptionTool { get; set; } = string.Empty;

        [JsonPropertyName("recorder_identity")]
        public string RecorderIdentity { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class AbyssalAnomaliesCatalog
    {
        private readonly List<HydrophoneAcousticEntry> _hydrophoneEntries = new List<HydrophoneAcousticEntry>();
        private readonly List<GeothermalBoreholeEntry> _boreholeEntries = new List<GeothermalBoreholeEntry>();
        private readonly List<CryopodFailureEntry> _cryopodEntries = new List<CryopodFailureEntry>();
        private readonly List<SaltMineInscriptionEntry> _saltMineEntries = new List<SaltMineInscriptionEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<HydrophoneAcousticEntry> HydrophoneEntries => _hydrophoneEntries;
        public IReadOnlyList<GeothermalBoreholeEntry> BoreholeEntries => _boreholeEntries;
        public IReadOnlyList<CryopodFailureEntry> CryopodEntries => _cryopodEntries;
        public IReadOnlyList<SaltMineInscriptionEntry> SaltMineEntries => _saltMineEntries;

        public int TotalCount => _hydrophoneEntries.Count + _boreholeEntries.Count + _cryopodEntries.Count + _saltMineEntries.Count;

        public IReadOnlyList<object> AllEntries
        {
            get
            {
                var list = new List<object>(TotalCount);
                list.AddRange(_hydrophoneEntries);
                list.AddRange(_boreholeEntries);
                list.AddRange(_cryopodEntries);
                list.AddRange(_saltMineEntries);
                return list;
            }
        }

        public void Clear()
        {
            _hydrophoneEntries.Clear();
            _boreholeEntries.Clear();
            _cryopodEntries.Clear();
            _saltMineEntries.Clear();
            _entriesById.Clear();
        }

        public static AbyssalAnomaliesCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new AbyssalAnomaliesCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Hydrophone Logs
            string hydroPath = Path.Combine(directoryPath, "hydrophone_acoustic_logs.json");
            if (File.Exists(hydroPath))
            {
                var list = CatalogLocator.LoadWrappedList<HydrophoneAcousticEntry>(File.ReadAllText(hydroPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._hydrophoneEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Geothermal Borehole Logs
            string boreholePath = Path.Combine(directoryPath, "geothermal_borehole_logs.json");
            if (File.Exists(boreholePath))
            {
                var list = CatalogLocator.LoadWrappedList<GeothermalBoreholeEntry>(File.ReadAllText(boreholePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._boreholeEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Cryopod Failure Logs
            string cryoPath = Path.Combine(directoryPath, "cryopod_failure_logs.json");
            if (File.Exists(cryoPath))
            {
                var list = CatalogLocator.LoadWrappedList<CryopodFailureEntry>(File.ReadAllText(cryoPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._cryopodEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Salt Mine Inscriptions
            string saltPath = Path.Combine(directoryPath, "salt_mine_inscriptions.json");
            if (File.Exists(saltPath))
            {
                var list = CatalogLocator.LoadWrappedList<SaltMineInscriptionEntry>(File.ReadAllText(saltPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._saltMineEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public static AbyssalAnomaliesCatalog LoadFromDirectory(string dataDir, IFileIO files, IJsonSerializer serializer)
        {
            var catalog = new AbyssalAnomaliesCatalog();
            string narrativeDir = files.Combine(dataDir, "narrative");
            if (!files.DirectoryExists(narrativeDir))
            {
                narrativeDir = dataDir;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Hydrophone Logs
            string hydroPath = files.Combine(narrativeDir, "hydrophone_acoustic_logs.json");
            if (files.FileExists(hydroPath))
            {
                var list = CatalogLocator.LoadWrappedList<HydrophoneAcousticEntry>(files.ReadAllText(hydroPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id) || catalog._entriesById.ContainsKey(item.Id)) continue;
                        catalog._hydrophoneEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Geothermal Borehole Logs
            string boreholePath = files.Combine(narrativeDir, "geothermal_borehole_logs.json");
            if (files.FileExists(boreholePath))
            {
                var list = CatalogLocator.LoadWrappedList<GeothermalBoreholeEntry>(files.ReadAllText(boreholePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id) || catalog._entriesById.ContainsKey(item.Id)) continue;
                        catalog._boreholeEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Cryopod Failure Logs
            string cryoPath = files.Combine(narrativeDir, "cryopod_failure_logs.json");
            if (files.FileExists(cryoPath))
            {
                var list = CatalogLocator.LoadWrappedList<CryopodFailureEntry>(files.ReadAllText(cryoPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id) || catalog._entriesById.ContainsKey(item.Id)) continue;
                        catalog._cryopodEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Salt Mine Inscriptions
            string saltPath = files.Combine(narrativeDir, "salt_mine_inscriptions.json");
            if (files.FileExists(saltPath))
            {
                var list = CatalogLocator.LoadWrappedList<SaltMineInscriptionEntry>(files.ReadAllText(saltPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id) || catalog._entriesById.ContainsKey(item.Id)) continue;
                        catalog._saltMineEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public object? GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            _entriesById.TryGetValue(id, out var obj);
            return obj;
        }

        public List<object> GetByTag(string tag)
        {
            var results = new List<object>();
            if (string.IsNullOrEmpty(tag)) return results;

            foreach (var h in _hydrophoneEntries)
                if (h.Tags.Exists(t => string.Equals(t, tag, StringComparison.OrdinalIgnoreCase))) results.Add(h);

            foreach (var b in _boreholeEntries)
                if (b.Tags.Exists(t => string.Equals(t, tag, StringComparison.OrdinalIgnoreCase))) results.Add(b);

            foreach (var c in _cryopodEntries)
                if (c.Tags.Exists(t => string.Equals(t, tag, StringComparison.OrdinalIgnoreCase))) results.Add(c);

            foreach (var s in _saltMineEntries)
                if (s.Tags.Exists(t => string.Equals(t, tag, StringComparison.OrdinalIgnoreCase))) results.Add(s);

            return results;
        }

        public List<object> GetBySearch(string term)
        {
            var results = new List<object>();
            if (string.IsNullOrEmpty(term)) return results;

            foreach (var h in _hydrophoneEntries)
            {
                if (h.Id.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    h.BuoyCallsign.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    h.SignalClassification.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    h.Prose.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    h.Tags.Exists(t => t.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    results.Add(h);
                }
            }

            foreach (var b in _boreholeEntries)
            {
                if (b.Id.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    b.BoreholeId.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    b.GeologicalFormation.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    b.Prose.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    b.Tags.Exists(t => t.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    results.Add(b);
                }
            }

            foreach (var c in _cryopodEntries)
            {
                if (c.Id.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    c.PodId.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    c.SubjectDesignation.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    c.SystemAlert.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    c.Prose.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    c.Tags.Exists(t => t.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    results.Add(c);
                }
            }

            foreach (var s in _saltMineEntries)
            {
                if (s.Id.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.MineGallery.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.RockMedium.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.RecorderIdentity.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.Prose.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.Tags.Exists(t => t.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    results.Add(s);
                }
            }

            return results;
        }

        public HydrophoneAcousticEntry? GetHydrophone(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is HydrophoneAcousticEntry e ? e : null;
        }

        public GeothermalBoreholeEntry? GetBorehole(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is GeothermalBoreholeEntry e ? e : null;
        }

        public CryopodFailureEntry? GetCryopod(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is CryopodFailureEntry e ? e : null;
        }

        public SaltMineInscriptionEntry? GetSaltMine(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is SaltMineInscriptionEntry e ? e : null;
        }
    }
}
