using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class NumbersStationCipherEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("station_nickname")]
        public string StationNickname { get; set; } = string.Empty;

        [JsonPropertyName("transmission_frequency_khz")]
        public float TransmissionFrequencyKhz { get; set; }

        [JsonPropertyName("modulation_mode")]
        public string ModulationMode { get; set; } = string.Empty;

        [JsonPropertyName("chime_interval_seconds")]
        public int ChimeIntervalSeconds { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class SeismicFaultAlarmEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("station_id")]
        public string StationId { get; set; } = string.Empty;

        [JsonPropertyName("richter_magnitude")]
        public float RichterMagnitude { get; set; }

        [JsonPropertyName("depth_km")]
        public float DepthKm { get; set; }

        [JsonPropertyName("alert_tier")]
        public string AlertTier { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class EmpSnifferLogEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("detector_id")]
        public string DetectorId { get; set; } = string.Empty;

        [JsonPropertyName("e1_field_strength_volts_per_meter")]
        public float E1FieldStrengthVoltsPerMeter { get; set; }

        [JsonPropertyName("rise_time_nanoseconds")]
        public float RiseTimeNanoseconds { get; set; }

        [JsonPropertyName("pulse_classification")]
        public string PulseClassification { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class BunkerWiretapEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("intercept_channel")]
        public string InterceptChannel { get; set; } = string.Empty;

        [JsonPropertyName("target_faction")]
        public string TargetFaction { get; set; } = string.Empty;

        [JsonPropertyName("audio_clarity_score")]
        public float AudioClarityScore { get; set; }

        [JsonPropertyName("speaker_identities")]
        public string SpeakerIdentities { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class SignalIntelligenceCatalog
    {
        private readonly List<NumbersStationCipherEntry> _cipherEntries = new List<NumbersStationCipherEntry>();
        private readonly List<SeismicFaultAlarmEntry> _seismicEntries = new List<SeismicFaultAlarmEntry>();
        private readonly List<EmpSnifferLogEntry> _empEntries = new List<EmpSnifferLogEntry>();
        private readonly List<BunkerWiretapEntry> _wiretapEntries = new List<BunkerWiretapEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<NumbersStationCipherEntry> CipherEntries => _cipherEntries;
        public IReadOnlyList<SeismicFaultAlarmEntry> SeismicEntries => _seismicEntries;
        public IReadOnlyList<EmpSnifferLogEntry> EmpEntries => _empEntries;
        public IReadOnlyList<BunkerWiretapEntry> WiretapEntries => _wiretapEntries;

        public int TotalCount => _cipherEntries.Count + _seismicEntries.Count + _empEntries.Count + _wiretapEntries.Count;

        public static SignalIntelligenceCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new SignalIntelligenceCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Numbers Station Ciphers
            string cipherPath = Path.Combine(directoryPath, "numbers_station_ciphers.json");
            if (File.Exists(cipherPath))
            {
                var list = CatalogLocator.LoadWrappedList<NumbersStationCipherEntry>(File.ReadAllText(cipherPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._cipherEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Seismic Fault Alarms
            string seismicPath = Path.Combine(directoryPath, "seismic_array_fault_alarms.json");
            if (File.Exists(seismicPath))
            {
                var list = CatalogLocator.LoadWrappedList<SeismicFaultAlarmEntry>(File.ReadAllText(seismicPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._seismicEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. EMP Sniffer Logs
            string empPath = Path.Combine(directoryPath, "emp_atmospheric_sniffer_logs.json");
            if (File.Exists(empPath))
            {
                var list = CatalogLocator.LoadWrappedList<EmpSnifferLogEntry>(File.ReadAllText(empPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._empEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Bunker Wiretap Transcripts
            string wiretapPath = Path.Combine(directoryPath, "bunker_wiretap_transcripts.json");
            if (File.Exists(wiretapPath))
            {
                var list = CatalogLocator.LoadWrappedList<BunkerWiretapEntry>(File.ReadAllText(wiretapPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._wiretapEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public NumbersStationCipherEntry? GetCipher(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is NumbersStationCipherEntry e ? e : null;
        }

        public SeismicFaultAlarmEntry? GetSeismic(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is SeismicFaultAlarmEntry e ? e : null;
        }

        public EmpSnifferLogEntry? GetEmp(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is EmpSnifferLogEntry e ? e : null;
        }

        public BunkerWiretapEntry? GetWiretap(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is BunkerWiretapEntry e ? e : null;
        }
    }
}
