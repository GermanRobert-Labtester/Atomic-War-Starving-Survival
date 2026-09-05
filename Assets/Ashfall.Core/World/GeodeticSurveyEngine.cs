using System;
using System.Collections.Generic;
using System.IO;
#pragma warning disable CS8618

namespace Ashfall.Core.World
{
    // ─── Catalog types ───

    [Serializable]
    public sealed class GeodeticSurveyCatalog
    {
        public int schema_version = 1;
        public List<SurveyPointDef> survey_points = new List<SurveyPointDef>();
        public SurveyEquipmentDef survey_equipment = new SurveyEquipmentDef();
        public Dictionary<string, WeatherModifierDef> weather_modifiers = new Dictionary<string, WeatherModifierDef>(StringComparer.OrdinalIgnoreCase);
        public TriangulationParamsDef triangulation = new TriangulationParamsDef();
        public NavigationEffectsDef navigation_effects = new NavigationEffectsDef();
    }

    [Serializable]
    public sealed class SurveyPointDef
    {
        public string survey_point_id = string.Empty;
        public string world_node_id = string.Empty;
        public string display_name = string.Empty;
        public string point_type = string.Empty;
        public float elevation_m;
        public string visibility_class = string.Empty;
        public List<string> obstruction_tags = new List<string>();
        public float baseline_quality;
        public bool construction_allowed;
        public List<string> construction_required_items = new List<string>();
        public int construction_labor_ticks;
        public List<string> hidden_route_refs = new List<string>();
    }

    [Serializable]
    public sealed class SurveyEquipmentDef
    {
        public string theodolite_item_id = "item_theodolite_brass_precision";
        public string stadia_rod_item_id = "item_surveyor_stadia_rod";
        public string datum_plate_item_id = "item_datum_plate_bronze";
        public float theodolite_base_error_degrees = 0.05f;
        public float per_observation_wear = 0.005f;
    }

    [Serializable]
    public sealed class WeatherModifierDef
    {
        public float error_multiplier = 1f;
        public float visibility_multiplier = 1f;
    }

    [Serializable]
    public sealed class TriangulationParamsDef
    {
        public float min_baseline_length_m = 50f;
        public float max_baseline_length_m = 50000f;
        public float min_triangle_angle_degrees = 5f;
        public float max_triangle_angle_degrees = 175f;
        public int required_observations_per_triangle = 3;
        public float network_accuracy_max = 1f;
        public float network_accuracy_floor = 0.1f;
    }

    [Serializable]
    public sealed class NavigationEffectsDef
    {
        public float drift_reduction_per_accuracy = 0.5f;
        public float speed_bonus_per_accuracy = 0.15f;
        public float max_drift_reduction = 1f;
        public float max_speed_bonus = 0.25f;
    }

    // ─── State DTOs ───

    [Serializable]
    public sealed class GeodeticSurveyState
    {
        public string systemId = GeodeticSurveyEngine.SystemId;
        public List<SurveyMonumentState> monuments = new List<SurveyMonumentState>();
        public List<SurveyObservation> observations = new List<SurveyObservation>();
        public List<ResolvedTriangle> resolvedTriangles = new List<ResolvedTriangle>();
        public float networkAccuracy;
        public List<string> unlockedShortcutIds = new List<string>();
        public List<string> surveyedCorridorIds = new List<string>();
    }

    [Serializable]
    public sealed class SurveyMonumentState
    {
        public string monumentId = string.Empty;
        public string surveyPointId = string.Empty;
        public string worldNodeId = string.Empty;
        public float integrity = 1f;
        public int establishedDay = -1;
        public bool isActive = true;
    }

    [Serializable]
    public sealed class SurveyObservation
    {
        public string observationId = string.Empty;
        public string fromMonumentId = string.Empty;
        public string toPointId = string.Empty;
        public float horizontalAngleDegrees;
        public float verticalAngleDegrees;
        public float uncertaintyDegrees;
        public int observedDay;
        public string weatherCondition = string.Empty;
    }

    [Serializable]
    public sealed class ResolvedTriangle
    {
        public string triangleId = string.Empty;
        public string pointAId = string.Empty;
        public string pointBId = string.Empty;
        public string pointCId = string.Empty;
        public float accuracy;
        public int resolvedDay;
        public List<string> unlockedRoutes = new List<string>();
    }

    // ─── Engine ───

    /// <summary>
    /// ASHFALL Geodetic Survey Engine (Plan 79).
    /// Owns survey baselines, monument state, triangulation math, and geodetic knowledge.
    /// Does not own canonical map coordinates, expedition state, or combat resolution.
    /// </summary>
    public sealed class GeodeticSurveyEngine
    {
        public const string SystemId = "geodetic_survey";

        private GeodeticSurveyState _state = new GeodeticSurveyState();
        private readonly GeodeticSurveyCatalog _catalog;
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private int _currentDay;

        public GeodeticSurveyState State => _state;
        public float NetworkAccuracy => _state.networkAccuracy;
        public IReadOnlyList<SurveyMonumentState> Monuments => _state.monuments;
        public IReadOnlyList<string> UnlockedShortcuts => _state.unlockedShortcutIds;

        public event Action<SurveyMonumentState>? OnMonumentEstablished;
        public event Action<ResolvedTriangle>? OnTriangleResolved;
        public event Action<string>? OnShortcutUnlocked;
        public event Action? OnSurveyChanged;

        public GeodeticSurveyEngine(GeodeticSurveyCatalog catalog, ISeededRng rng, ILog? log = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public SurveyPointDef? FindPoint(string surveyPointId)
        {
            if (string.IsNullOrEmpty(surveyPointId)) return null;
            foreach (var p in _catalog.survey_points)
                if (p.survey_point_id == surveyPointId) return p;
            return null;
        }

        public SurveyMonumentState? FindMonument(string monumentId)
        {
            if (string.IsNullOrEmpty(monumentId)) return null;
            foreach (var m in _state.monuments)
                if (m.monumentId == monumentId) return m;
            return null;
        }

        /// <summary>Finds the monument established at a survey point.</summary>
        public SurveyMonumentState? FindMonumentBySurveyPoint(string surveyPointId)
        {
            if (string.IsNullOrEmpty(surveyPointId)) return null;
            foreach (var m in _state.monuments)
                if (m.surveyPointId == surveyPointId) return m;
            return null;
        }

        /// <summary>
        /// Establish a survey monument at a survey point. Consumes required items.
        /// </summary>
        public ActionResult EstablishMonument(string surveyPointId, int day, Func<string, int, bool> consumeItems)
        {
            var point = FindPoint(surveyPointId);
            if (point == null)
                return ActionResult.Blocked("unknown_point", "survey.unknown_point");

            if (!point.construction_allowed)
                return ActionResult.Blocked("construction_forbidden", "survey.construction_forbidden");

            // Check if already established
            foreach (var m in _state.monuments)
                if (m.surveyPointId == surveyPointId && m.isActive)
                    return ActionResult.Blocked("already_established", "survey.already_established");

            // Consume required items
            foreach (var itemId in point.construction_required_items)
            {
                if (!consumeItems(itemId, 1))
                    return ActionResult.Blocked("missing_items", $"survey.missing_{itemId}");
            }

            var monument = new SurveyMonumentState
            {
                monumentId = $"monument_{surveyPointId}",
                surveyPointId = surveyPointId,
                worldNodeId = point.world_node_id,
                integrity = 1f,
                establishedDay = day,
                isActive = true
            };

            _state.monuments.Add(monument);
            _log.Info($"[Survey] monument established at {surveyPointId}");
            OnMonumentEstablished?.Invoke(monument);
            OnSurveyChanged?.Invoke();
            return ActionResult.Success("survey.monument_established");
        }

        /// <summary>
        /// Record an angle observation from one monument to a target point.
        /// </summary>
        public SurveyObservation Observe(string fromMonumentId, string targetPointId, string weatherCondition, float surveyorSkill = 0.5f)
        {
            var monument = FindMonument(fromMonumentId) ?? FindMonumentBySurveyPoint(fromMonumentId);
            if (monument == null || !monument.isActive)
                return new SurveyObservation { observationId = "invalid" };

            var target = FindPoint(targetPointId);
            if (target == null)
                return new SurveyObservation { observationId = "invalid" };

            // Base error from instrument
            float baseError = _catalog.survey_equipment.theodolite_base_error_degrees;

            // Weather modifier
            float weatherMult = 1f;
            if (_catalog.weather_modifiers.TryGetValue(weatherCondition, out var wmod))
                weatherMult = wmod.error_multiplier;

            // Skill modifier
            float skillMult = 1f + (1f - surveyorSkill) * 0.5f;

            // Deterministic pseudo-random noise (seeded)
            float noise = (float)(_rng.NextDouble() * 2.0 - 1.0) * 0.02f;

            float uncertainty = baseError * weatherMult * skillMult + noise;
            uncertainty = Math.Max(0.001f, Math.Min(uncertainty, 5f));

            // Generate deterministic angles using a stable string hash (not GetHashCode)
            float hashA = StableStringHash(monument.surveyPointId);
            float hashB = StableStringHash(targetPointId);
            float horizontalAngle = (hashA * 360f + hashB * 180f) % 360f;
            float verticalAngle = (hashA * 90f - hashB * 45f);
            verticalAngle = Math.Clamp(verticalAngle, -90f, 90f);

            var obs = new SurveyObservation
            {
                observationId = $"obs_{fromMonumentId}_{targetPointId}_{_currentDay}",
                fromMonumentId = fromMonumentId,
                toPointId = targetPointId,
                horizontalAngleDegrees = horizontalAngle,
                verticalAngleDegrees = verticalAngle,
                uncertaintyDegrees = uncertainty,
                observedDay = _currentDay,
                weatherCondition = weatherCondition
            };

            _state.observations.Add(obs);
            _log.Info($"[Survey] observation {obs.observationId}: h={horizontalAngle:F2} v={verticalAngle:F2} ±{uncertainty:F3}°");
            OnSurveyChanged?.Invoke();
            return obs;
        }

        /// <summary>
        /// Try to resolve a triangle from three surveyed points. Returns the triangle if resolvable.
        /// </summary>
        public ResolvedTriangle? TryResolveTriangle(string pointAId, string pointBId, string pointCId)
        {
            var mA = FindMonument(pointAId) ?? FindMonumentBySurveyPoint(pointAId);
            var mB = FindMonument(pointBId) ?? FindMonumentBySurveyPoint(pointBId);
            var mC = FindMonument(pointCId) ?? FindMonumentBySurveyPoint(pointCId);

            if (mA == null || !mA.isActive || mB == null || !mB.isActive || mC == null || !mC.isActive)
                return null;

            // Check if already resolved
            string triId = $"tri_{pointAId}_{pointBId}_{pointCId}";
            foreach (var t in _state.resolvedTriangles)
                if (t.triangleId == triId) return t;

            // Check triangle validity: angles must be within bounds
            var pA = FindPoint(pointAId);
            var pB = FindPoint(pointBId);
            var pC = FindPoint(pointCId);
            if (pA == null || pB == null || pC == null) return null;

            // Check baseline lengths
            float ab = Math.Abs(pA.elevation_m - pB.elevation_m) + 100f;
            float bc = Math.Abs(pB.elevation_m - pC.elevation_m) + 100f;
            float ca = Math.Abs(pC.elevation_m - pA.elevation_m) + 100f;

            if (ab < _catalog.triangulation.min_baseline_length_m ||
                bc < _catalog.triangulation.min_baseline_length_m ||
                ca < _catalog.triangulation.min_baseline_length_m)
                return null;

            // Compute accuracy from monument qualities
            float accuracy = (mA.integrity + mB.integrity + mC.integrity) / 3f;
            accuracy *= (pA.baseline_quality + pB.baseline_quality + pC.baseline_quality) / 3f;

            var triangle = new ResolvedTriangle
            {
                triangleId = triId,
                pointAId = pointAId,
                pointBId = pointBId,
                pointCId = pointCId,
                accuracy = Math.Clamp(accuracy, _catalog.triangulation.network_accuracy_floor, _catalog.triangulation.network_accuracy_max),
                resolvedDay = _currentDay
            };

            // Unlock hidden routes from all three points
            foreach (var pointDef in new[] { pA, pB, pC })
            {
                foreach (var routeId in pointDef.hidden_route_refs)
                {
                    if (!_state.unlockedShortcutIds.Contains(routeId))
                    {
                        triangle.unlockedRoutes.Add(routeId);
                        _state.unlockedShortcutIds.Add(routeId);
                        OnShortcutUnlocked?.Invoke(routeId);
                    }
                }
            }

            _state.resolvedTriangles.Add(triangle);

            // Update network accuracy
            RecalculateNetworkAccuracy();

            _log.Info($"[Survey] triangle resolved: {triId} accuracy={accuracy:F2}");
            OnTriangleResolved?.Invoke(triangle);
            OnSurveyChanged?.Invoke();
            return triangle;
        }

        /// <summary>
        /// Damage a monument (weathering, combat, faction activity).
        /// </summary>
        public void DamageMonument(string monumentId, float damage)
        {
            var m = FindMonument(monumentId) ?? FindMonumentBySurveyPoint(monumentId);
            if (m == null) return;
            m.integrity = Math.Max(0, m.integrity - damage);
            if (m.integrity <= 0)
            {
                m.isActive = false;
                _log.Warn($"[Survey] monument {monumentId} destroyed");
            }
            RecalculateNetworkAccuracy();
            OnSurveyChanged?.Invoke();
        }

        /// <summary>
        /// Get the navigation drift multiplier for a surveyed corridor.
        /// </summary>
        public float GetDriftReduction(string corridorId)
        {
            if (!_state.surveyedCorridorIds.Contains(corridorId)) return 0f;
            return Math.Min(_catalog.navigation_effects.max_drift_reduction,
                _state.networkAccuracy * _catalog.navigation_effects.drift_reduction_per_accuracy);
        }

        /// <summary>
        /// Get the travel speed bonus for a surveyed corridor.
        /// </summary>
        public float GetSpeedBonus(string corridorId)
        {
            if (!_state.surveyedCorridorIds.Contains(corridorId)) return 0f;
            return Math.Min(_catalog.navigation_effects.max_speed_bonus,
                _state.networkAccuracy * _catalog.navigation_effects.speed_bonus_per_accuracy);
        }

        /// <summary>
        /// Mark a corridor as surveyed.
        /// </summary>
        public void MarkCorridorSurveyed(string corridorId)
        {
            if (!_state.surveyedCorridorIds.Contains(corridorId))
            {
                _state.surveyedCorridorIds.Add(corridorId);
                OnSurveyChanged?.Invoke();
            }
        }

        public void TickDay(int day)
        {
            _currentDay = day;
        }

        private void RecalculateNetworkAccuracy()
        {
            if (_state.resolvedTriangles.Count == 0)
            {
                _state.networkAccuracy = _catalog.triangulation.network_accuracy_floor;
                return;
            }
            float sum = 0f;
            foreach (var t in _state.resolvedTriangles)
                sum += t.accuracy;
            _state.networkAccuracy = Math.Clamp(sum / _state.resolvedTriangles.Count,
                _catalog.triangulation.network_accuracy_floor,
                _catalog.triangulation.network_accuracy_max);
        }

        public GeodeticSurveyState CaptureState() => CloneState(_state);

        public void RestoreState(GeodeticSurveyState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static GeodeticSurveyState CloneState(GeodeticSurveyState src)
        {
            if (src == null) return new GeodeticSurveyState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<GeodeticSurveyState>(json) ?? new GeodeticSurveyState();
        }

        public GeodeticSurveyCatalog Catalog => _catalog;

        /// <summary>
        /// Deterministic string-to-float hash (0..1). Uses FNV-1a style for cross-platform stability.
        /// </summary>
        private static float StableStringHash(string s)
        {
            if (string.IsNullOrEmpty(s)) return 0f;
            uint hash = 2166136261;
            foreach (char c in s)
            {
                hash ^= c;
                hash *= 16777619;
            }
            return (float)(hash % 1000000) / 1000000f;
        }
    }

    /// <summary>
    /// Loads <c>geodetic_survey_catalog.json</c>.
    /// </summary>
    public static class GeodeticSurveyCatalogLoader
    {
        public static GeodeticSurveyCatalog Load(string dataDir, IFileIO files, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(dataDir)) throw new ArgumentException("dataDir required", nameof(dataDir));
            if (files == null) throw new ArgumentNullException(nameof(files));
            if (json == null) throw new ArgumentNullException(nameof(json));

            string path = files.Combine(dataDir, "geodetic_survey_catalog.json");
            if (!files.FileExists(path))
                return new GeodeticSurveyCatalog();

            string raw = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new GeodeticSurveyCatalog();

            var catalog = json.Deserialize<GeodeticSurveyCatalog>(raw);
            if (catalog == null)
                throw new InvalidOperationException("Failed to deserialize geodetic_survey_catalog.json");

            // Validate
            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var p in catalog.survey_points)
            {
                if (string.IsNullOrEmpty(p.survey_point_id))
                    throw new InvalidOperationException("Geodetic survey catalog: survey_point_id is required");
                if (!seenIds.Add(p.survey_point_id))
                    throw new InvalidOperationException($"Geodetic survey catalog: duplicate survey_point_id '{p.survey_point_id}'");
                if (float.IsNaN(p.elevation_m) || float.IsInfinity(p.elevation_m))
                    throw new InvalidOperationException($"Geodetic survey catalog: invalid elevation for '{p.survey_point_id}'");
            }

            return catalog;
        }
    }
}