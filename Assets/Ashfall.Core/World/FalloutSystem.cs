using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ashfall.Core.World
{
    [Serializable]
    public sealed class FalloutPatternDef
    {
        public string pattern_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public List<string> origin_tags { get; set; } = new List<string>();
        public float initial_radius { get; set; } = 15.0f;
        public float initial_toxicity { get; set; } = 150.0f;
        public float base_dispersal_rate { get; set; } = 0.05f;
        public float base_dissipation_hours { get; set; } = 48.0f;
        public float wind_response { get; set; } = 1.0f;
        public float ground_deposition_rate { get; set; } = 1.0f;
        public float groundwater_taint_threshold_hours { get; set; } = 12.0f;
        public float warning_radius { get; set; } = 30.0f;
        public float black_rain_chance { get; set; } = 0.1f;
    }

    [Serializable]
    public sealed class FalloutCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public List<FalloutPatternDef> patterns { get; set; } = new List<FalloutPatternDef>();
    }

    [Serializable]
    public sealed class RadCloud
    {
        public string cloudId { get; set; } = string.Empty;
        public string patternId { get; set; } = string.Empty;
        public string originZoneId { get; set; } = string.Empty;
        public float positionX { get; set; }
        public float positionY { get; set; }
        public float radius { get; set; } = 15.0f;
        public float toxicity { get; set; } = 100.0f;
        public float baseDispersalRate { get; set; } = 0.05f;
        public float remainingMass { get; set; } = 100.0f;
        public float ageHours { get; set; }
        public float dissipationHoursRemaining { get; set; } = 48.0f;
        public bool active { get; set; } = true;
        public bool warningFired { get; set; }
        public List<string> activeZoneOverlaps { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class FalloutSystemState
    {
        public string systemId = FalloutSystem.SystemId;
        public List<RadCloud> clouds = new List<RadCloud>();
        public bool isSealed;
        public float sealDurationHoursRemaining;
        public float sealEfficiency = 0.85f;
        public Dictionary<string, float> waterSourceLingerHours = new Dictionary<string, float>(StringComparer.Ordinal);
        public List<string> taintedWaterSources = new List<string>();
        public List<string> oneShotWarnings = new List<string>();
    }

    public sealed class FalloutSystem
    {
        public const string SystemId = "fallout";
        private FalloutSystemState _state = new FalloutSystemState();
        private readonly Dictionary<string, FalloutPatternDef> _patterns = new Dictionary<string, FalloutPatternDef>(StringComparer.Ordinal);
        private readonly ILog _log;
        private int _cloudCounter;

        public FalloutSystemState State => _state;
        public IReadOnlyList<RadCloud> ActiveClouds => _state.clouds.FindAll(c => c.active);
        public bool IsShelterSealed => _state.isSealed && _state.sealDurationHoursRemaining > 0f;

        public event Action<RadCloud>? OnFalloutSpawned;
        public event Action<RadCloud, string, float>? OnFalloutWarning; // cloud, zoneId, distance
        public event Action<RadCloud, string>? OnFalloutEnteredZone;
        public event Action<RadCloud, string>? OnFalloutClearedZone;
        public event Action<string>? OnGroundwaterTainted;
        public event Action<float, float>? OnShelterSealed; // duration, efficiency
        public event Action? OnShelterUnsealed;

        public FalloutSystem(ILog? log = null, string dataPath = "")
        {
            _log = log ?? NullLog.Instance;
            LoadCatalog(dataPath);
        }

        public void LoadCatalog(string dataPath)
        {
            string path = string.IsNullOrEmpty(dataPath)
                ? Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data", "fallout_patterns.json")
                : Path.Combine(dataPath, "fallout_patterns.json");

            if (!File.Exists(path))
            {
                // Fallback default patterns
                RegisterPattern(new FalloutPatternDef
                {
                    pattern_id = "fallout_pattern_strontium_plume",
                    display_name = "Strontium-90 Plume",
                    initial_radius = 15.0f,
                    initial_toxicity = 180.0f,
                    base_dispersal_rate = 0.05f,
                    base_dissipation_hours = 72.0f,
                    warning_radius = 35.0f
                });
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                var container = JsonSerializer.Deserialize<FalloutCatalogContainer>(json);
                if (container?.patterns != null)
                {
                    foreach (var p in container.patterns)
                        RegisterPattern(p);
                }
            }
            catch (Exception ex)
            {
                _log.Warn($"[FalloutSystem] Failed to load catalog from {path}: {ex.Message}");
            }
        }

        public void RegisterPattern(FalloutPatternDef pattern)
        {
            if (pattern == null || string.IsNullOrEmpty(pattern.pattern_id)) return;
            _patterns[pattern.pattern_id] = pattern;
        }

        public RadCloud SpawnCloud(string patternId, float posX, float posY, string originZoneId = "")
        {
            _patterns.TryGetValue(patternId, out var def);
            float initRadius = def?.initial_radius ?? 15.0f;
            float initToxicity = def?.initial_toxicity ?? 150.0f;
            float dissipation = def?.base_dissipation_hours ?? 48.0f;
            float dispersalRate = def?.base_dispersal_rate ?? 0.05f;

            var cloud = new RadCloud
            {
                cloudId = $"cloud_{++_cloudCounter}_{patternId}",
                patternId = patternId,
                originZoneId = originZoneId,
                positionX = posX,
                positionY = posY,
                radius = initRadius,
                toxicity = initToxicity,
                baseDispersalRate = dispersalRate,
                remainingMass = 100.0f,
                ageHours = 0f,
                dissipationHoursRemaining = dissipation,
                active = true
            };

            _state.clouds.Add(cloud);
            OnFalloutSpawned?.Invoke(cloud);
            return cloud;
        }

        public static void CalculateWindDispersal(RadCloud cloud, float windDirDeg, float windSpeedKmh, float hours)
        {
            if (cloud == null || !cloud.active || hours <= 0f) return;

            // Mathematical advection: direction 0° = North (+Y), 90° = East (+X), 180° = South (-Y), 270° = West (-X)
            double rad = (90.0 - windDirDeg) * (Math.PI / 180.0);
            float distKm = windSpeedKmh * hours;
            float dx = (float)(Math.Cos(rad) * distKm);
            float dy = (float)(Math.Sin(rad) * distKm);

            cloud.positionX += dx;
            cloud.positionY += dy;

            // Dispersion: radius increases over time proportionally to dispersal rate and wind movement
            float expansion = cloud.baseDispersalRate * (1.0f + windSpeedKmh * 0.02f) * hours * 10f;
            cloud.radius += expansion;

            // Toxicity dissipation
            cloud.ageHours += hours;
            cloud.dissipationHoursRemaining = Math.Max(0f, cloud.dissipationHoursRemaining - hours);
            float decayFactor = Math.Max(0f, 1.0f - (hours / Math.Max(1f, cloud.dissipationHoursRemaining + hours)));
            cloud.remainingMass *= decayFactor;

            // Concentration lowers as area (radius^2) expands
            float areaRatio = Math.Max(1.0f, cloud.radius / 15.0f);
            cloud.toxicity = Math.Max(0f, (cloud.remainingMass * 1.5f) / areaRatio);

            if (cloud.dissipationHoursRemaining <= 0f || cloud.toxicity < 1.0f)
            {
                cloud.active = false;
            }
        }

        public void Tick(float hours, float windDirDeg, float windSpeedKmh, IReadOnlyDictionary<string, (float x, float y)> zonePositions)
        {
            if (hours <= 0f) return;

            // 1. Tick shelter seal
            if (_state.isSealed)
            {
                _state.sealDurationHoursRemaining -= hours;
                if (_state.sealDurationHoursRemaining <= 0f)
                {
                    _state.isSealed = false;
                    _state.sealDurationHoursRemaining = 0f;
                    OnShelterUnsealed?.Invoke();
                }
            }

            // 2. Disperse clouds and evaluate zone overlaps
            for (int i = 0; i < _state.clouds.Count; i++)
            {
                var cloud = _state.clouds[i];
                if (!cloud.active) continue;

                CalculateWindDispersal(cloud, windDirDeg, windSpeedKmh, hours);
                if (!cloud.active) continue;

                _patterns.TryGetValue(cloud.patternId, out var def);
                float warnRadius = def?.warning_radius ?? (cloud.radius * 1.5f);
                float taintHoursReq = def?.groundwater_taint_threshold_hours ?? 12.0f;

                var currentlyOverlapping = new List<string>();

                foreach (var kvp in zonePositions)
                {
                    string zoneId = kvp.Key;
                    float zx = kvp.Value.x;
                    float zy = kvp.Value.y;

                    float dist = (float)Math.Sqrt(Math.Pow(cloud.positionX - zx, 2) + Math.Pow(cloud.positionY - zy, 2));

                    // Early warning radius check
                    if (dist <= warnRadius)
                    {
                        string warningKey = $"{cloud.cloudId}_{zoneId}_warn";
                        if (!_state.oneShotWarnings.Contains(warningKey))
                        {
                            _state.oneShotWarnings.Add(warningKey);
                            OnFalloutWarning?.Invoke(cloud, zoneId, dist);
                        }
                    }

                    // Direct cloud radius overlap
                    if (dist <= cloud.radius)
                    {
                        currentlyOverlapping.Add(zoneId);
                        if (!cloud.activeZoneOverlaps.Contains(zoneId))
                        {
                            cloud.activeZoneOverlaps.Add(zoneId);
                            OnFalloutEnteredZone?.Invoke(cloud, zoneId);
                        }

                        // Accumulate groundwater taint linger
                        if (!_state.taintedWaterSources.Contains(zoneId))
                        {
                            _state.waterSourceLingerHours.TryGetValue(zoneId, out float currLinger);
                            currLinger += hours;
                            _state.waterSourceLingerHours[zoneId] = currLinger;

                            if (currLinger >= taintHoursReq)
                            {
                                _state.taintedWaterSources.Add(zoneId);
                                OnGroundwaterTainted?.Invoke(zoneId);
                            }
                        }
                    }
                }

                // Check cleared zones
                for (int z = cloud.activeZoneOverlaps.Count - 1; z >= 0; z--)
                {
                    string zId = cloud.activeZoneOverlaps[z];
                    if (!currentlyOverlapping.Contains(zId))
                    {
                        cloud.activeZoneOverlaps.RemoveAt(z);
                        OnFalloutClearedZone?.Invoke(cloud, zId);
                    }
                }
            }
        }

        public float GetZoneRadiationRate(string zoneId, float zoneX, float zoneY)
        {
            float totalRate = 0f;
            for (int i = 0; i < _state.clouds.Count; i++)
            {
                var c = _state.clouds[i];
                if (!c.active) continue;

                float dist = (float)Math.Sqrt(Math.Pow(c.positionX - zoneX, 2) + Math.Pow(c.positionY - zoneY, 2));
                if (dist <= c.radius)
                {
                    float factor = 1.0f - (dist / Math.Max(1.0f, c.radius));
                    totalRate += c.toxicity * factor;
                }
            }

            // Apply shelter sealing attenuation if queried for shelter zone
            if (zoneId == "loc_holdfast" && IsShelterSealed)
            {
                totalRate *= (1.0f - _state.sealEfficiency);
            }

            return totalRate;
        }

        public bool SealShelter(float durationHours, float efficiency = 0.85f)
        {
            if (durationHours <= 0f) return false;
            _state.isSealed = true;
            _state.sealDurationHoursRemaining = Math.Max(_state.sealDurationHoursRemaining, durationHours);
            _state.sealEfficiency = Math.Clamp(efficiency, 0.1f, 0.99f);
            OnShelterSealed?.Invoke(_state.sealDurationHoursRemaining, _state.sealEfficiency);
            return true;
        }

        public void RestoreState(FalloutSystemState state)
        {
            if (state == null) return;
            _state = state;
            _cloudCounter = _state.clouds.Count;
        }
    }
}
