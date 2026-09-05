// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Persistent minefield hazard state for a route segment.
    /// </summary>
    [Serializable]
    public sealed class MinefieldSegmentState
    {
        public float Density01 { get; set; } = 0.0f;
        public List<string> TypeTags { get; set; } = new List<string>();
        public bool Detected { get; set; } = false;
        public float ClearedFraction01 { get; set; } = 0.0f;
        public int LastClearedDay { get; set; } = -1;
        public float ResidualRisk01 { get; set; } = 0.0f;

        public MinefieldSegmentState Clone() => new MinefieldSegmentState
        {
            Density01 = Density01,
            TypeTags = new List<string>(TypeTags),
            Detected = Detected,
            ClearedFraction01 = ClearedFraction01,
            LastClearedDay = LastClearedDay,
            ResidualRisk01 = ResidualRisk01
        };
    }

    /// <summary>
    /// Persistent rail condition and profile quality for a rail route segment.
    /// </summary>
    [Serializable]
    public sealed class RailSegmentCondition
    {
        public float RoughnessIndex { get; set; } = 1.0f; // 0.0 = ultra-smooth, 1.0 = severely corrugated/damaged
        public float ProfileError01 { get; set; } = 0.8f;
        public float SurfaceDefect01 { get; set; } = 0.5f;
        public float RustScale01 { get; set; } = 0.5f;
        public float SafeSpeedLimitKph { get; set; } = 25.0f;
        public float GroundFraction01 { get; set; } = 0.0f;
        public int LastGroundDay { get; set; } = -1;
        // Route engineered design ceiling. 0 = no explicit design cap recorded;
        // rehabilitation may then raise speed up to the grinding head's cap only.
        public float DesignSpeedLimitKph { get; set; } = 0f;

        public RailSegmentCondition Clone() => new RailSegmentCondition
        {
            RoughnessIndex = RoughnessIndex,
            ProfileError01 = ProfileError01,
            SurfaceDefect01 = SurfaceDefect01,
            RustScale01 = RustScale01,
            SafeSpeedLimitKph = SafeSpeedLimitKph,
            GroundFraction01 = GroundFraction01,
            LastGroundDay = LastGroundDay,
            DesignSpeedLimitKph = DesignSpeedLimitKph
        };
    }

    /// <summary>
    /// State record for one route segment's physical and logistical infrastructure.
    /// </summary>
    [Serializable]
    public sealed class RouteSegmentInfrastructureRecord
    {
        public string RouteId { get; set; } = string.Empty;
        public string SegmentId { get; set; } = string.Empty;
        public string Mode { get; set; } = "road"; // road, rail, subway, service_tunnel, offroad
        public List<string> HazardFlags { get; set; } = new List<string>();
        public MinefieldSegmentState MinefieldState { get; set; } = new MinefieldSegmentState();
        public RailSegmentCondition RailCondition { get; set; } = new RailSegmentCondition();
        public string ClearanceState { get; set; } = "unbreached"; // unbreached, partial, cleared, secured
        public float SpeedLimitKph { get; set; } = 50.0f;
        public float RoughnessIndex { get; set; } = 0.5f;
        public List<string> MaintenanceFlags { get; set; } = new List<string>();
        public int LastModifiedDay { get; set; } = 1;

        public RouteSegmentInfrastructureRecord Clone() => new RouteSegmentInfrastructureRecord
        {
            RouteId = RouteId,
            SegmentId = SegmentId,
            Mode = Mode,
            HazardFlags = new List<string>(HazardFlags),
            MinefieldState = MinefieldState.Clone(),
            RailCondition = RailCondition.Clone(),
            ClearanceState = ClearanceState,
            SpeedLimitKph = SpeedLimitKph,
            RoughnessIndex = RoughnessIndex,
            MaintenanceFlags = new List<string>(MaintenanceFlags),
            LastModifiedDay = LastModifiedDay
        };
    }

    /// <summary>
    /// Persistent state DTO for the Route Infrastructure System.
    /// </summary>
    [Serializable]
    public sealed class RouteInfrastructureState
    {
        public int SchemaVersion { get; set; } = 1;
        public List<RouteSegmentInfrastructureRecord> Segments { get; set; } = new List<RouteSegmentInfrastructureRecord>();

        public RouteInfrastructureState Clone()
        {
            var copy = new RouteInfrastructureState { SchemaVersion = SchemaVersion };
            foreach (var seg in Segments)
            {
                copy.Segments.Add(seg.Clone());
            }
            return copy;
        }
    }

    /// <summary>
    /// Single authority for mutable route infrastructure, corridor maintenance, minefield breaching,
    /// and rail corridor condition. References canonical Wasteland and Railway route topology without duplicating it.
    /// </summary>
    public sealed class RouteInfrastructureSystem
    {
        private readonly Dictionary<string, RouteSegmentInfrastructureRecord> _segments =
            new Dictionary<string, RouteSegmentInfrastructureRecord>(StringComparer.Ordinal);

        public event Action<string, string>? OnRouteSegmentUpdated;
        public event Action<string, string, float>? OnMinefieldCleared;
        public event Action<string, string, float>? OnRailGrinded;

        public RouteInfrastructureSystem(RouteInfrastructureState? initialState = null)
        {
            if (initialState != null)
            {
                RestoreState(initialState);
            }
        }

        private static string Key(string routeId, string segmentId) =>
            $"{routeId}:{segmentId}";

        public IReadOnlyCollection<RouteSegmentInfrastructureRecord> GetAllSegments() =>
            _segments.Values;

        public RouteSegmentInfrastructureRecord GetOrCreateSegment(string routeId, string segmentId, string mode = "road", float defaultSpeedLimit = 50f)
        {
            string k = Key(routeId, segmentId);
            if (!_segments.TryGetValue(k, out var seg))
            {
                seg = new RouteSegmentInfrastructureRecord
                {
                    RouteId = routeId,
                    SegmentId = segmentId,
                    Mode = mode,
                    SpeedLimitKph = defaultSpeedLimit,
                    RoughnessIndex = mode == "rail" ? 0.8f : 0.5f
                };
                if (mode == "rail")
                {
                    seg.RailCondition.SafeSpeedLimitKph = defaultSpeedLimit;
                }
                _segments[k] = seg;
            }
            return seg;
        }

        public RouteSegmentInfrastructureRecord? FindSegment(string routeId, string segmentId)
        {
            _segments.TryGetValue(Key(routeId, segmentId), out var seg);
            return seg;
        }

        public float GetSpeedLimit(string routeId)
        {
            float minSpeed = float.MaxValue;
            bool found = false;
            foreach (var pair in _segments)
            {
                if (string.Equals(pair.Value.RouteId, routeId, StringComparison.Ordinal))
                {
                    found = true;
                    float effective = pair.Value.Mode == "rail"
                        ? pair.Value.RailCondition.SafeSpeedLimitKph
                        : pair.Value.SpeedLimitKph;
                    if (effective < minSpeed) minSpeed = effective;
                }
            }
            return found ? minSpeed : 50.0f;
        }

        public float GetHazardModifier(string routeId, IEnumerable<string>? vehicleCapabilities = null)
        {
            float hazardSum = 0f;
            int count = 0;
            var caps = vehicleCapabilities != null ? new HashSet<string>(vehicleCapabilities, StringComparer.OrdinalIgnoreCase) : null;

            foreach (var pair in _segments)
            {
                if (string.Equals(pair.Value.RouteId, routeId, StringComparison.Ordinal))
                {
                    count++;
                    float segmentHazard = 1.0f;
                    if (pair.Value.MinefieldState.Density01 > 0f)
                    {
                        float mineRisk = pair.Value.MinefieldState.ResidualRisk01;
                        if (caps != null && caps.Contains("mine_resistant"))
                        {
                            mineRisk *= 0.5f;
                        }
                        segmentHazard += mineRisk * 1.5f;
                    }
                    if (pair.Value.Mode == "rail")
                    {
                        segmentHazard += pair.Value.RailCondition.RoughnessIndex * 0.5f;
                    }
                    hazardSum += segmentHazard;
                }
            }
            return count > 0 ? (hazardSum / count) : 1.0f;
        }

        public float GetTravelModifier(string routeId, IEnumerable<string>? vehicleCapabilities = null)
        {
            float totalSpeed = 0f;
            int count = 0;
            foreach (var pair in _segments)
            {
                if (string.Equals(pair.Value.RouteId, routeId, StringComparison.Ordinal))
                {
                    count++;
                    float speed = pair.Value.Mode == "rail"
                        ? pair.Value.RailCondition.SafeSpeedLimitKph
                        : pair.Value.SpeedLimitKph;
                    totalSpeed += speed;
                }
            }
            if (count == 0) return 1.0f;
            float avgSpeed = totalSpeed / count;
            // Baseline 50 km/h = 1.0 modifier. Faster speed = lower travel time modifier.
            return Math.Clamp(50.0f / Math.Max(10.0f, avgSpeed), 0.25f, 3.0f);
        }

        public bool CanTraverse(string routeId, IEnumerable<string>? vehicleCapabilities = null)
        {
            var caps = vehicleCapabilities != null ? new HashSet<string>(vehicleCapabilities, StringComparer.OrdinalIgnoreCase) : null;
            foreach (var pair in _segments)
            {
                if (string.Equals(pair.Value.RouteId, routeId, StringComparison.Ordinal))
                {
                    if (pair.Value.HazardFlags.Contains("impassable"))
                        return false;
                    if (pair.Value.Mode == "rail" && (caps == null || !caps.Contains("rail_capable")))
                        return false;
                    if (pair.Value.MinefieldState.ResidualRisk01 >= 0.9f && (caps == null || !caps.Contains("armored")))
                        return false;
                }
            }
            return true;
        }

        public void RegisterMinefield(string routeId, string segmentId, float density01, List<string>? typeTags = null, int day = 1)
        {
            var seg = GetOrCreateSegment(routeId, segmentId, "road");
            seg.MinefieldState.Density01 = Math.Clamp(density01, 0f, 1f);
            seg.MinefieldState.ResidualRisk01 = seg.MinefieldState.Density01;
            seg.MinefieldState.Detected = true;
            seg.MinefieldState.ClearedFraction01 = 0f;
            seg.MinefieldState.LastClearedDay = day;
            if (typeTags != null)
            {
                seg.MinefieldState.TypeTags = new List<string>(typeTags);
            }
            seg.ClearanceState = "unbreached";
            seg.LastModifiedDay = day;
            OnRouteSegmentUpdated?.Invoke(routeId, segmentId);
        }

        public void ApplyMineClearance(string routeId, string segmentId, float progressMeters, float segmentLengthMeters, float clearanceEfficiency, int day, float minResidualRisk = 0.02f)
        {
            var seg = GetOrCreateSegment(routeId, segmentId);
            if (segmentLengthMeters <= 0f) segmentLengthMeters = 1000f;
            float fractionIncrement = progressMeters / segmentLengthMeters;
            seg.MinefieldState.ClearedFraction01 = Math.Clamp(seg.MinefieldState.ClearedFraction01 + fractionIncrement, 0f, 1f);

            float baseDensity = seg.MinefieldState.Density01;
            float cleared = seg.MinefieldState.ClearedFraction01;
            float eff = Math.Clamp(clearanceEfficiency, 0.5f, 1.0f);
            // The residual floor must never exceed the field's own density (sparse
            // fields clamp to themselves, not to the generic floor).
            float floor = Math.Clamp(Math.Min(minResidualRisk, baseDensity), 0f, 1f);
            seg.MinefieldState.ResidualRisk01 = Math.Clamp(baseDensity * (1.0f - (eff * cleared)), floor, Math.Max(floor, baseDensity));

            if (seg.MinefieldState.ClearedFraction01 >= 0.99f)
            {
                seg.ClearanceState = "cleared";
                seg.HazardFlags.Remove("minefield_active");
            }
            else
            {
                seg.ClearanceState = "partial";
            }

            seg.MinefieldState.LastClearedDay = day;
            seg.LastModifiedDay = day;
            OnRouteSegmentUpdated?.Invoke(routeId, segmentId);
            OnMinefieldCleared?.Invoke(routeId, segmentId, seg.MinefieldState.ClearedFraction01);
        }

        public void RegisterRailSegment(string routeId, string segmentId, float initialRoughness, float safeSpeedKph, int day = 1, float designSpeedKph = 0f)
        {
            var seg = GetOrCreateSegment(routeId, segmentId, "rail", safeSpeedKph);
            seg.Mode = "rail"; // explicit: a rail registration upgrades any pre-existing road-mode record
            seg.RailCondition.RoughnessIndex = Math.Clamp(initialRoughness, 0.05f, 1.0f);
            seg.RailCondition.SafeSpeedLimitKph = safeSpeedKph;
            seg.RailCondition.GroundFraction01 = 0.0f;
            seg.RailCondition.LastGroundDay = day;
            seg.RailCondition.DesignSpeedLimitKph = Math.Max(0f, designSpeedKph);
            seg.RoughnessIndex = seg.RailCondition.RoughnessIndex;
            seg.LastModifiedDay = day;
            OnRouteSegmentUpdated?.Invoke(routeId, segmentId);
        }

        public void ApplyRailGrinding(string routeId, string segmentId, float progressKm, float segmentLengthKm, float roughnessReduction, float speedBonusKph, float maxSpeedCapKph, int day)
        {
            var seg = GetOrCreateSegment(routeId, segmentId, "rail");
            if (segmentLengthKm <= 0f) segmentLengthKm = 10f;
            float passFraction = progressKm / segmentLengthKm;
            seg.RailCondition.GroundFraction01 = Math.Clamp(seg.RailCondition.GroundFraction01 + passFraction, 0f, 1f);

            // Progressive reduction in roughness
            float actualReduction = roughnessReduction * passFraction;
            seg.RailCondition.RoughnessIndex = Math.Clamp(seg.RailCondition.RoughnessIndex - actualReduction, 0.05f, 1.0f);
            seg.RoughnessIndex = seg.RailCondition.RoughnessIndex;

            // Upgrade safe speed bounded by min(head cap, route design limit). Without
            // a recorded design limit the head cap is the only ceiling.
            float speedCeiling = maxSpeedCapKph;
            if (seg.RailCondition.DesignSpeedLimitKph > 0f)
                speedCeiling = Math.Min(maxSpeedCapKph, seg.RailCondition.DesignSpeedLimitKph);
            speedCeiling = Math.Max(15f, speedCeiling);
            float speedUpgrade = speedBonusKph * passFraction;
            seg.RailCondition.SafeSpeedLimitKph = Math.Clamp(seg.RailCondition.SafeSpeedLimitKph + speedUpgrade, 15f, speedCeiling);
            seg.SpeedLimitKph = seg.RailCondition.SafeSpeedLimitKph;

            seg.RailCondition.LastGroundDay = day;
            seg.LastModifiedDay = day;
            OnRouteSegmentUpdated?.Invoke(routeId, segmentId);
            OnRailGrinded?.Invoke(routeId, segmentId, seg.RailCondition.RoughnessIndex);
        }

        public RouteInfrastructureState CaptureState()
        {
            var state = new RouteInfrastructureState { SchemaVersion = 1 };
            var keys = new List<string>(_segments.Keys);
            keys.Sort(string.CompareOrdinal);
            foreach (var k in keys)
            {
                state.Segments.Add(_segments[k].Clone());
            }
            return state;
        }

        public void RestoreState(RouteInfrastructureState? state)
        {
            _segments.Clear();
            if (state == null) return;
            foreach (var seg in state.Segments)
            {
                if (seg == null || string.IsNullOrEmpty(seg.RouteId) || string.IsNullOrEmpty(seg.SegmentId))
                    continue;
                _segments[Key(seg.RouteId, seg.SegmentId)] = seg.Clone();
            }
        }
    }
}
