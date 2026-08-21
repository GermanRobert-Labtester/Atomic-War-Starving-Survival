using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class RadiationTopoSheetEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("quadrangle_name")]
        public string QuadrangleName { get; set; } = string.Empty;

        [JsonPropertyName("grid_scale_ratio")]
        public string GridScaleRatio { get; set; } = string.Empty;

        [JsonPropertyName("peak_gamma_field_r_hr")]
        public float PeakGammaFieldRHr { get; set; }

        [JsonPropertyName("dominant_terrain_feature")]
        public string DominantTerrainFeature { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class ScavengerRouteNoteEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("route_identifier")]
        public string RouteIdentifier { get; set; } = string.Empty;

        [JsonPropertyName("lead_scout_name")]
        public string LeadScoutName { get; set; } = string.Empty;

        [JsonPropertyName("hazard_level")]
        public string HazardLevel { get; set; } = string.Empty;

        [JsonPropertyName("distance_kilometers")]
        public float DistanceKilometers { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class CanyonMudflowReportEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("canyon_location_id")]
        public string CanyonLocationId { get; set; } = string.Empty;

        [JsonPropertyName("estimated_slurry_volume_m3")]
        public float EstimatedSlurryVolumeM3 { get; set; }

        [JsonPropertyName("flow_velocity_kmh")]
        public float FlowVelocityKmh { get; set; }

        [JsonPropertyName("structural_impact_severity")]
        public string StructuralImpactSeverity { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class CraterLakeLimnologyEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("crater_lake_name")]
        public string CraterLakeName { get; set; } = string.Empty;

        [JsonPropertyName("maximum_depth_meters")]
        public float MaximumDepthMeters { get; set; }

        [JsonPropertyName("bottom_layer_dissolved_h2s_ppm")]
        public float BottomLayerDissolvedH2sPpm { get; set; }

        [JsonPropertyName("stratification_type")]
        public string StratificationType { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class WastelandCartographyCatalog
    {
        private readonly List<RadiationTopoSheetEntry> _topoEntries = new List<RadiationTopoSheetEntry>();
        private readonly List<ScavengerRouteNoteEntry> _routeEntries = new List<ScavengerRouteNoteEntry>();
        private readonly List<CanyonMudflowReportEntry> _mudflowEntries = new List<CanyonMudflowReportEntry>();
        private readonly List<CraterLakeLimnologyEntry> _limnologyEntries = new List<CraterLakeLimnologyEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<RadiationTopoSheetEntry> TopoEntries => _topoEntries;
        public IReadOnlyList<ScavengerRouteNoteEntry> RouteEntries => _routeEntries;
        public IReadOnlyList<CanyonMudflowReportEntry> MudflowEntries => _mudflowEntries;
        public IReadOnlyList<CraterLakeLimnologyEntry> LimnologyEntries => _limnologyEntries;

        public int TotalCount => _topoEntries.Count + _routeEntries.Count + _mudflowEntries.Count + _limnologyEntries.Count;

        public static WastelandCartographyCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new WastelandCartographyCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Surface Radiation Survey Topo Sheets
            string topoPath = Path.Combine(directoryPath, "surface_radiation_topo_sheets.json");
            if (File.Exists(topoPath))
            {
                var list = JsonSerializer.Deserialize<List<RadiationTopoSheetEntry>>(File.ReadAllText(topoPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._topoEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Scavenger Expedition Route Field Notes
            string routePath = Path.Combine(directoryPath, "scavenger_expedition_route_notes.json");
            if (File.Exists(routePath))
            {
                var list = JsonSerializer.Deserialize<List<ScavengerRouteNoteEntry>>(File.ReadAllText(routePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._routeEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Blind Canyon Flash-Flood Mudflow Reports
            string mudPath = Path.Combine(directoryPath, "canyon_mudflow_hazard_reports.json");
            if (File.Exists(mudPath))
            {
                var list = JsonSerializer.Deserialize<List<CanyonMudflowReportEntry>>(File.ReadAllText(mudPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._mudflowEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Crater Lake Limnology & Heavy Metal Stratification
            string limPath = Path.Combine(directoryPath, "crater_lake_limnology_records.json");
            if (File.Exists(limPath))
            {
                var list = JsonSerializer.Deserialize<List<CraterLakeLimnologyEntry>>(File.ReadAllText(limPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._limnologyEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public RadiationTopoSheetEntry? GetTopoSheet(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is RadiationTopoSheetEntry e ? e : null;
        }

        public ScavengerRouteNoteEntry? GetRouteNote(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is ScavengerRouteNoteEntry e ? e : null;
        }

        public CanyonMudflowReportEntry? GetMudflow(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is CanyonMudflowReportEntry e ? e : null;
        }

        public CraterLakeLimnologyEntry? GetLimnology(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is CraterLakeLimnologyEntry e ? e : null;
        }
    }
}
