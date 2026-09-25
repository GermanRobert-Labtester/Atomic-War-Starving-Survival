// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Radiation
{
    // ── DTOs (snake_case JSON authority) ────────────────────────────────────

    public sealed class HotspotBandDto
    {
        [JsonPropertyName("severity")] public string severity { get; set; } = string.Empty;
        [JsonPropertyName("min_usv")] public double min_usv { get; set; }
        [JsonPropertyName("label")] public string label { get; set; } = string.Empty;
    }

    public sealed class HotspotPolicyDto
    {
        [JsonPropertyName("schema_version")] public int schema_version { get; set; } = 1;
        [JsonPropertyName("collection_id")] public string collection_id { get; set; } = string.Empty;
        [JsonPropertyName("description")] public string description { get; set; } = string.Empty;
        [JsonPropertyName("watch_threshold_usv")] public double watch_threshold_usv { get; set; }
        [JsonPropertyName("bands")] public List<HotspotBandDto> bands { get; set; } = new();
    }

    // ── Domain ──────────────────────────────────────────────────────────────

    public sealed class HotspotBand
    {
        public HotspotBand(string severity, double minUsv, string label)
        {
            Severity = severity;
            MinUsv = minUsv;
            Label = label;
        }

        public string Severity { get; }
        public double MinUsv { get; }
        public string Label { get; }
    }

    /// <summary>
    /// Authored severity policy for the dose-geography hotspot watch list.
    /// Pure data; the survey below is a read-only projection over live readings.
    /// </summary>
    public sealed class RadiationHotspotPolicy
    {
        public const string FileName = "radiation_hotspot_policy.json";

        private readonly List<HotspotBand> _bands = new();

        public double WatchThresholdUsv { get; private set; } = 25.0;
        public IReadOnlyList<HotspotBand> Bands => _bands;
        public IReadOnlyList<string> Errors { get; private set; } = Array.Empty<string>();

        /// <summary>
        /// Built-in policy mirroring the authored
        /// <c>radiation_hotspot_policy.json</c> bands. Used when no host has
        /// loaded the JSON (headless/selftest paths); production hosts may load
        /// the authored file through <see cref="Load"/> for the same values.
        /// </summary>
        public static RadiationHotspotPolicy Default
        {
            get
            {
                var policy = new RadiationHotspotPolicy { WatchThresholdUsv = 25.0 };
                policy._bands.Add(new HotspotBand("extreme", 200.0, "EXTREME — do not linger"));
                policy._bands.Add(new HotspotBand("high", 100.0, "HIGH — shielded transit only"));
                policy._bands.Add(new HotspotBand("elevated", 50.0, "ELEVATED — time the exposure"));
                policy._bands.Add(new HotspotBand("watch", 25.0, "WATCH — log the reading"));
                return policy;
            }
        }

        public static RadiationHotspotPolicy Load(string json)
        {
            var policy = new RadiationHotspotPolicy();
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(json))
            {
                policy.Errors = new[] { "hotspot policy is empty" };
                return policy;
            }

            HotspotPolicyDto? dto;
            try
            {
                dto = JsonSerializer.Deserialize<HotspotPolicyDto>(json, SystemTextJsonSerializer.Options);
            }
            catch (Exception ex)
            {
                policy.Errors = new[] { $"hotspot policy parse failed: {ex.Message}" };
                return policy;
            }

            if (dto == null || dto.bands == null || dto.bands.Count == 0)
            {
                policy.Errors = new[] { "hotspot policy defines no bands" };
                return policy;
            }

            policy.WatchThresholdUsv = dto.watch_threshold_usv > 0 ? dto.watch_threshold_usv : 25.0;
            foreach (var band in dto.bands)
            {
                if (band == null || string.IsNullOrWhiteSpace(band.severity)) continue;
                policy._bands.Add(new HotspotBand(band.severity, Math.Max(0, band.min_usv), band.label ?? string.Empty));
            }
            policy._bands.Sort((a, b) => b.MinUsv.CompareTo(a.MinUsv));
            policy.Errors = errors;
            return policy;
        }

        /// <summary>Band for a reading, or null when below the watch threshold.</summary>
        public HotspotBand? BandFor(double radiationUsv)
        {
            if (radiationUsv < WatchThresholdUsv) return null;
            foreach (var band in _bands)
            {
                if (radiationUsv >= band.MinUsv) return band;
            }
            return _bands.Count > 0 ? _bands[_bands.Count - 1] : null;
        }
    }

    public readonly struct HotspotRow
    {
        public HotspotRow(string locationId, string displayName, string sector, double radiationUsv,
            string severity, string label)
        {
            LocationId = locationId;
            DisplayName = displayName;
            Sector = sector;
            RadiationUsv = radiationUsv;
            Severity = severity;
            Label = label;
        }

        public string LocationId { get; }
        public string DisplayName { get; }
        public string Sector { get; }
        public double RadiationUsv { get; }
        public string Severity { get; }
        public string Label { get; }
    }

    /// <summary>
    /// Read-only projection: classify live dose-geography readings into a
    /// severity-banded hotspot watch list, worst first. Owns no state and
    /// writes nothing; the panel that shows the list remains a viewer.
    /// </summary>
    public static class RadiationHotspotSurvey
    {
        public static IReadOnlyList<HotspotRow> Classify(
            RadiationHotspotPolicy policy,
            IEnumerable<DoseLocationDef>? locations)
        {
            var rows = new List<HotspotRow>();
            if (policy == null || locations == null) return rows;

            foreach (var location in locations)
            {
                if (location == null) continue;
                var band = policy.BandFor(location.radiationUsv);
                if (band == null) continue;
                rows.Add(new HotspotRow(
                    location.id,
                    string.IsNullOrWhiteSpace(location.displayName) ? location.id : location.displayName,
                    location.sector,
                    location.radiationUsv,
                    band.Severity,
                    band.Label));
            }

            rows.Sort((a, b) => b.RadiationUsv.CompareTo(a.RadiationUsv));
            return rows;
        }
    }
}
