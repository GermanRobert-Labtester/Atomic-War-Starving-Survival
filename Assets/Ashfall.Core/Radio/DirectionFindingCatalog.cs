// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.IO;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// One fixed HF/DF listening array. Gameplay baseline geometry and
    /// uncertainty modifiers only — no real antenna engineering recipes.
    /// </summary>
    [Serializable]
    public sealed class DirectionFindingArrayDef
    {
        public string array_id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;
        public string baseline_class = "short"; // short | medium | long
        public string station_id = string.Empty;
        public float baseline_x_km;
        public float baseline_y_km;
        public List<string> band_ids = new List<string>();
        public float base_bearing_error_deg = 5f;
        public float skywave_uncertainty_mult = 1.4f;
        public float power_demand_w;
        public int calibration_interval_days = 7;
        public List<string> install_item_ids = new List<string>();
        public List<string> tags = new List<string>();

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(array_id)) { error = "array_id empty"; return false; }
            if (string.IsNullOrWhiteSpace(station_id)) { error = $"Array '{array_id}' missing station_id"; return false; }
            if (base_bearing_error_deg <= 0f || base_bearing_error_deg > 30f)
            { error = $"Array '{array_id}' base_bearing_error_deg out of range"; return false; }
            if (skywave_uncertainty_mult < 1f || skywave_uncertainty_mult > 5f)
            { error = $"Array '{array_id}' skywave_uncertainty_mult out of range"; return false; }
            if (power_demand_w < 0f) { error = $"Array '{array_id}' power cannot be negative"; return false; }
            if (calibration_interval_days < 1) { error = $"Array '{array_id}' calibration_interval_days < 1"; return false; }
            error = string.Empty;
            return true;
        }
    }

    [Serializable]
    public sealed class DirectionFindingBandDef
    {
        public string band_id = string.Empty;
        public string display_name = string.Empty;
        public float min_mhz;
        public float max_mhz;
    }

    [Serializable]
    public sealed class DirectionFindingSkywaveSeasonDef
    {
        public string season_id = string.Empty;
        public List<string> weather_tags = new List<string>();
        public float uncertainty_mult = 1f;
        public float confidence_mult = 1f;
        public float false_signature_chance;
    }

    /// <summary>
    /// Transmitter fingerprint: raises identity confidence and may map to a
    /// wasteland location. Identity never implies free exact coordinates.
    /// </summary>
    [Serializable]
    public sealed class DirectionFindingFingerprintDef
    {
        public string fingerprint_id = string.Empty;
        public string signal_id = string.Empty;
        public string display_name = string.Empty;
        public string mapped_location_id = string.Empty;
        public float identity_confidence = 0.5f;
        public List<string> tags = new List<string>();
    }

    [Serializable]
    public sealed class DirectionFindingCatalogDto
    {
        public int schema_version = 1;
        public List<DirectionFindingArrayDef> arrays = new List<DirectionFindingArrayDef>();
        public List<DirectionFindingBandDef> bands = new List<DirectionFindingBandDef>();
        public List<DirectionFindingSkywaveSeasonDef> skywave_seasons = new List<DirectionFindingSkywaveSeasonDef>();
        public List<DirectionFindingFingerprintDef> fingerprints = new List<DirectionFindingFingerprintDef>();
    }

    public sealed class DirectionFindingCatalog
    {
        private readonly Dictionary<string, DirectionFindingArrayDef> _arrays =
            new Dictionary<string, DirectionFindingArrayDef>(StringComparer.Ordinal);
        private readonly Dictionary<string, DirectionFindingBandDef> _bands =
            new Dictionary<string, DirectionFindingBandDef>(StringComparer.Ordinal);
        private readonly List<DirectionFindingSkywaveSeasonDef> _seasons = new List<DirectionFindingSkywaveSeasonDef>();
        private readonly Dictionary<string, DirectionFindingFingerprintDef> _fingerprintsBySignal =
            new Dictionary<string, DirectionFindingFingerprintDef>(StringComparer.Ordinal);

        public DirectionFindingCatalog(
            IEnumerable<DirectionFindingArrayDef>? arrays,
            IEnumerable<DirectionFindingBandDef>? bands,
            IEnumerable<DirectionFindingSkywaveSeasonDef>? seasons,
            IEnumerable<DirectionFindingFingerprintDef>? fingerprints)
        {
            foreach (var a in arrays ?? Enumerable.Empty<DirectionFindingArrayDef>())
            {
                if (a == null || string.IsNullOrWhiteSpace(a.array_id)) continue;
                _arrays[a.array_id] = a;
            }
            foreach (var b in bands ?? Enumerable.Empty<DirectionFindingBandDef>())
            {
                if (b == null || string.IsNullOrWhiteSpace(b.band_id)) continue;
                _bands[b.band_id] = b;
            }
            foreach (var s in seasons ?? Enumerable.Empty<DirectionFindingSkywaveSeasonDef>())
            {
                if (s == null || string.IsNullOrWhiteSpace(s.season_id)) continue;
                _seasons.Add(s);
            }
            foreach (var f in fingerprints ?? Enumerable.Empty<DirectionFindingFingerprintDef>())
            {
                if (f == null || string.IsNullOrWhiteSpace(f.signal_id)) continue;
                _fingerprintsBySignal[f.signal_id] = f;
            }
        }

        public IReadOnlyDictionary<string, DirectionFindingArrayDef> Arrays => _arrays;
        public IReadOnlyDictionary<string, DirectionFindingBandDef> Bands => _bands;
        public IReadOnlyList<DirectionFindingSkywaveSeasonDef> SkywaveSeasons => _seasons;
        public IReadOnlyDictionary<string, DirectionFindingFingerprintDef> FingerprintsBySignal => _fingerprintsBySignal;

        public DirectionFindingArrayDef? GetArray(string arrayId) =>
            string.IsNullOrEmpty(arrayId) ? null : (_arrays.TryGetValue(arrayId, out var a) ? a : null);

        public DirectionFindingArrayDef? GetArrayByStation(string stationId)
        {
            if (string.IsNullOrEmpty(stationId)) return null;
            foreach (var a in _arrays.Values)
            {
                if (string.Equals(a.station_id, stationId, StringComparison.Ordinal))
                    return a;
            }
            return null;
        }

        public DirectionFindingFingerprintDef? GetFingerprintForSignal(string signalId) =>
            string.IsNullOrEmpty(signalId)
                ? null
                : (_fingerprintsBySignal.TryGetValue(signalId, out var f) ? f : null);

        public DirectionFindingSkywaveSeasonDef? ResolveSkywave(string weatherCondition)
        {
            if (string.IsNullOrEmpty(weatherCondition)) return null;
            foreach (var season in _seasons)
            {
                if (season.weather_tags == null) continue;
                for (int i = 0; i < season.weather_tags.Count; i++)
                {
                    if (string.Equals(season.weather_tags[i], weatherCondition, StringComparison.Ordinal))
                        return season;
                }
            }
            return null;
        }
    }

    public static class DirectionFindingCatalogLoader
    {
        public const string DefaultFileName = "direction_finding_catalog.json";

        public static DirectionFindingCatalogDto Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null) throw new ArgumentNullException(nameof(fileIO));
            if (json == null) throw new ArgumentNullException(nameof(json));
            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                throw new InvalidOperationException($"Missing direction-finding catalog: {path}");

            string raw = fileIO.ReadAllText(path);
            var dto = json.Deserialize<DirectionFindingCatalogDto>(raw)
                      ?? throw new InvalidOperationException("direction_finding_catalog.json deserialized null");
            return dto;
        }

        public static DirectionFindingCatalog Build(DirectionFindingCatalogDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new DirectionFindingCatalog(dto.arrays, dto.bands, dto.skywave_seasons, dto.fingerprints);
        }

        public static void Validate(DirectionFindingCatalogDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.schema_version < 1)
                throw new InvalidOperationException("direction_finding_catalog schema_version must be >= 1");
            if (dto.arrays == null || dto.arrays.Count == 0)
                throw new InvalidOperationException("direction_finding_catalog requires at least one array");

            var seenArrays = new HashSet<string>(StringComparer.Ordinal);
            var seenStations = new HashSet<string>(StringComparer.Ordinal);
            foreach (var a in dto.arrays)
            {
                if (a == null) throw new InvalidOperationException("null array entry");
                if (!a.Validate(out var err)) throw new InvalidOperationException(err);
                if (!seenArrays.Add(a.array_id))
                    throw new InvalidOperationException($"Duplicate array_id '{a.array_id}'");
                if (!seenStations.Add(a.station_id))
                    throw new InvalidOperationException($"Duplicate station_id '{a.station_id}' across arrays");
            }

            if (dto.fingerprints != null)
            {
                var seenFp = new HashSet<string>(StringComparer.Ordinal);
                var seenSig = new HashSet<string>(StringComparer.Ordinal);
                foreach (var f in dto.fingerprints)
                {
                    if (f == null) continue;
                    if (string.IsNullOrWhiteSpace(f.fingerprint_id))
                        throw new InvalidOperationException("fingerprint_id empty");
                    if (!seenFp.Add(f.fingerprint_id))
                        throw new InvalidOperationException($"Duplicate fingerprint_id '{f.fingerprint_id}'");
                    if (string.IsNullOrWhiteSpace(f.signal_id))
                        throw new InvalidOperationException($"Fingerprint '{f.fingerprint_id}' missing signal_id");
                    if (!seenSig.Add(f.signal_id))
                        throw new InvalidOperationException($"Duplicate fingerprint signal_id '{f.signal_id}'");
                    if (f.identity_confidence < 0f || f.identity_confidence > 1f)
                        throw new InvalidOperationException($"Fingerprint '{f.fingerprint_id}' identity_confidence out of range");
                }
            }

            if (dto.skywave_seasons != null)
            {
                foreach (var s in dto.skywave_seasons)
                {
                    if (s == null) continue;
                    if (s.uncertainty_mult < 0.5f || s.uncertainty_mult > 5f)
                        throw new InvalidOperationException($"Season '{s.season_id}' uncertainty_mult out of range");
                    if (s.confidence_mult < 0.1f || s.confidence_mult > 1.5f)
                        throw new InvalidOperationException($"Season '{s.season_id}' confidence_mult out of range");
                    if (s.false_signature_chance < 0f || s.false_signature_chance > 1f)
                        throw new InvalidOperationException($"Season '{s.season_id}' false_signature_chance out of range");
                }
            }
        }
    }
}
