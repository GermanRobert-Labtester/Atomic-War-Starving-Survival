// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Underground
{
    public enum TunnelHazardType
    {
        CollapseRisk = 0,
        Flooding = 1,
        ToxicGas = 2,
        Radiation = 3,
        Darkness = 4
    }

    public enum TunnelStatus
    {
        Clear = 0,
        Impassable = 1,
        Collapsed = 2
    }

    [Serializable]
    public sealed class TunnelSegment
    {
        public string SegmentId { get; set; } = string.Empty;
        public string SegmentName { get; set; } = string.Empty;
        public string ConnectsFrom { get; set; } = string.Empty;
        public string ConnectsTo { get; set; } = string.Empty;
        public float LengthHours { get; set; } = 2f;
        public int Difficulty { get; set; } = 1;
        public List<TunnelHazardType> Hazards { get; set; } = new List<TunnelHazardType>();
        public bool IsDiscovered { get; set; } = false;
        public float StructuralIntegrity { get; set; } = 100f; // 0 to 100
        public TunnelStatus Status { get; set; } = TunnelStatus.Clear;
    }

    [Serializable]
    public sealed class TunnelJunction
    {
        public string JunctionId { get; set; } = string.Empty;
        public string JunctionName { get; set; } = string.Empty;
        public List<string> ConnectedSegmentIds { get; set; } = new List<string>();
        public bool IsDiscovered { get; set; } = false;
        public bool HasResourceDeposit { get; set; } = false;
        public string ResourceDepositType { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class TunnelJunctionDef
    {
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public List<string> connected_segments { get; set; } = new List<string>();
        public bool has_resource { get; set; }
        public string resource_type { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class TunnelSegmentDef
    {
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string from { get; set; } = string.Empty;
        public string to { get; set; } = string.Empty;
        public float length_hours { get; set; } = 2f;
        public int difficulty { get; set; } = 1;
        public float integrity { get; set; } = 100f;
        public List<string> hazards { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class TunnelNetworkCatalogData
    {
        public int schema_version { get; set; } = 1;
        public List<TunnelJunctionDef> junctions { get; set; } = new List<TunnelJunctionDef>();
        public List<TunnelSegmentDef> segments { get; set; } = new List<TunnelSegmentDef>();
    }

    [Serializable]
    public sealed class TunnelNetworkState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<TunnelSegment> Segments { get; set; } = new List<TunnelSegment>();
        public List<TunnelJunction> Junctions { get; set; } = new List<TunnelJunction>();
    }

    /// <summary>
    /// Plan 167 — Underground Tunnel Network System.
    /// Manages subterranean passages between the shelter, junctions, and hidden bunkers/vaults,
    /// tracking structural integrity, hazards (collapse, flood, gas), maintenance, and fast transit.
    /// </summary>
    public sealed class TunnelNetworkSystem
    {
        private readonly TunnelNetworkState _state;

        public event Action<TunnelSegment>? OnSegmentDiscovered;
        public event Action<TunnelSegment>? OnSegmentCollapsed;
        public event Action<TunnelSegment>? OnSegmentRepaired;

        public int TotalSegmentCount => _state.Segments.Count;
        public int DiscoveredSegmentCount => _state.Segments.Count(s => s.IsDiscovered);
        public int JunctionCount => _state.Junctions.Count;

        public TunnelNetworkSystem(TunnelNetworkState? state = null)
        {
            _state = state ?? new TunnelNetworkState();
        }

        public TunnelJunction RegisterJunction(
            string junctionId,
            string name,
            bool hasResource = false,
            string resourceType = "")
        {
            if (string.IsNullOrWhiteSpace(junctionId)) throw new ArgumentNullException(nameof(junctionId));

            var junction = _state.Junctions.FirstOrDefault(j => string.Equals(j.JunctionId, junctionId, StringComparison.OrdinalIgnoreCase));
            if (junction == null)
            {
                junction = new TunnelJunction
                {
                    JunctionId = junctionId.Trim(),
                    JunctionName = string.IsNullOrWhiteSpace(name) ? junctionId : name.Trim(),
                    IsDiscovered = false,
                    HasResourceDeposit = hasResource,
                    ResourceDepositType = resourceType ?? string.Empty
                };
                _state.Junctions.Add(junction);
            }

            return junction;
        }

        public TunnelSegment RegisterSegment(
            string segmentId,
            string name,
            string connectsFrom,
            string connectsTo,
            float lengthHours = 2f,
            int difficulty = 1,
            float integrity = 100f)
        {
            if (string.IsNullOrWhiteSpace(segmentId)) throw new ArgumentNullException(nameof(segmentId));

            var segment = _state.Segments.FirstOrDefault(s => string.Equals(s.SegmentId, segmentId, StringComparison.OrdinalIgnoreCase));
            if (segment == null)
            {
                segment = new TunnelSegment
                {
                    SegmentId = segmentId.Trim(),
                    SegmentName = string.IsNullOrWhiteSpace(name) ? segmentId : name.Trim(),
                    ConnectsFrom = connectsFrom ?? string.Empty,
                    ConnectsTo = connectsTo ?? string.Empty,
                    LengthHours = Math.Max(0.5f, lengthHours),
                    Difficulty = Math.Clamp(difficulty, 1, 5),
                    StructuralIntegrity = Math.Clamp(integrity, 0f, 100f),
                    Status = integrity > 0f ? TunnelStatus.Clear : TunnelStatus.Collapsed,
                    IsDiscovered = false
                };
                _state.Segments.Add(segment);

                // Add to junction connections if applicable
                var j1 = _state.Junctions.FirstOrDefault(j => string.Equals(j.JunctionId, connectsFrom, StringComparison.OrdinalIgnoreCase));
                if (j1 != null && !j1.ConnectedSegmentIds.Contains(segment.SegmentId))
                {
                    j1.ConnectedSegmentIds.Add(segment.SegmentId);
                }

                var j2 = _state.Junctions.FirstOrDefault(j => string.Equals(j.JunctionId, connectsTo, StringComparison.OrdinalIgnoreCase));
                if (j2 != null && !j2.ConnectedSegmentIds.Contains(segment.SegmentId))
                {
                    j2.ConnectedSegmentIds.Add(segment.SegmentId);
                }
            }

            return segment;
        }

        public bool DiscoverSegment(string segmentId)
        {
            var segment = _state.Segments.FirstOrDefault(s => string.Equals(s.SegmentId, segmentId, StringComparison.OrdinalIgnoreCase));
            if (segment == null) return false;

            if (!segment.IsDiscovered)
            {
                segment.IsDiscovered = true;
                OnSegmentDiscovered?.Invoke(segment);
            }

            return true;
        }

        public (bool CanTraverse, float TravelTimeHours, string Reason) CanTraverse(string fromNode, string toNode)
        {
            var segment = _state.Segments.FirstOrDefault(s =>
                (string.Equals(s.ConnectsFrom, fromNode, StringComparison.OrdinalIgnoreCase) && string.Equals(s.ConnectsTo, toNode, StringComparison.OrdinalIgnoreCase)) ||
                (string.Equals(s.ConnectsFrom, toNode, StringComparison.OrdinalIgnoreCase) && string.Equals(s.ConnectsTo, fromNode, StringComparison.OrdinalIgnoreCase)));

            if (segment == null)
            {
                return (false, 0f, "No connecting tunnel exists.");
            }

            if (!segment.IsDiscovered)
            {
                return (false, 0f, "Tunnel has not yet been discovered.");
            }

            if (segment.Status == TunnelStatus.Collapsed)
            {
                return (false, 0f, "Tunnel has suffered a structural collapse and is blocked.");
            }

            if (segment.Status == TunnelStatus.Impassable)
            {
                return (false, 0f, "Tunnel is currently impassable due to hazards.");
            }

            return (true, segment.LengthHours, "Transit path clear.");
        }

        public bool RepairSegment(string segmentId, float repairAmount = 50f)
        {
            var segment = _state.Segments.FirstOrDefault(s => string.Equals(s.SegmentId, segmentId, StringComparison.OrdinalIgnoreCase));
            if (segment == null) return false;

            segment.StructuralIntegrity = Math.Clamp(segment.StructuralIntegrity + repairAmount, 0f, 100f);
            if (segment.StructuralIntegrity > 20f && segment.Status == TunnelStatus.Collapsed)
            {
                segment.Status = TunnelStatus.Clear;
            }

            OnSegmentRepaired?.Invoke(segment);
            return true;
        }

        public void LoadCatalog(TunnelNetworkCatalogData catalog)
        {
            if (catalog == null) return;

            if (catalog.junctions != null)
            {
                foreach (var j in catalog.junctions)
                {
                    RegisterJunction(j.id, j.name, j.has_resource, j.resource_type);
                }
            }

            if (catalog.segments != null)
            {
                foreach (var s in catalog.segments)
                {
                    var seg = RegisterSegment(s.id, s.name, s.from, s.to, s.length_hours, s.difficulty, s.integrity);
                    if (s.hazards != null)
                    {
                        foreach (var hStr in s.hazards)
                        {
                            if (Enum.TryParse<TunnelHazardType>(hStr, true, out var hType) && !seg.Hazards.Contains(hType))
                            {
                                seg.Hazards.Add(hType);
                            }
                        }
                    }
                }
            }
        }

        public TunnelSegment? FindSegment(string segmentId)
        {
            if (string.IsNullOrWhiteSpace(segmentId)) return null;
            return _state.Segments.FirstOrDefault(s => string.Equals(s.SegmentId, segmentId, StringComparison.OrdinalIgnoreCase));
        }

        public bool ReinforceSegment(string segmentId, float integrityGain = 25f)
        {
            var segment = FindSegment(segmentId);
            if (segment == null) return false;

            segment.StructuralIntegrity = Math.Clamp(segment.StructuralIntegrity + integrityGain, 0f, 100f);
            if (segment.StructuralIntegrity >= 40f)
            {
                segment.Hazards.Remove(TunnelHazardType.CollapseRisk);
            }
            if (segment.StructuralIntegrity > 20f && segment.Status == TunnelStatus.Collapsed)
            {
                segment.Status = TunnelStatus.Clear;
            }
            OnSegmentRepaired?.Invoke(segment);
            return true;
        }

        public bool ClearHazard(string segmentId, TunnelHazardType hazard)
        {
            var segment = FindSegment(segmentId);
            if (segment == null) return false;

            bool removed = segment.Hazards.Remove(hazard);
            if (segment.Hazards.Count == 0 && segment.Status == TunnelStatus.Impassable)
            {
                segment.Status = TunnelStatus.Clear;
            }
            return removed;
        }

        public (bool Found, float SavedHours, string Reason) EvaluateSurfaceBypass(string fromLocation, string toLocation)
        {
            var traversal = CanTraverse(fromLocation, toLocation);
            if (!traversal.CanTraverse)
            {
                return (false, 0f, traversal.Reason);
            }

            // Estimate surface travel time baseline as 2.0x tunnel length
            float surfaceHours = traversal.TravelTimeHours * 2.0f;
            float saved = Math.Max(0f, surfaceHours - traversal.TravelTimeHours);
            return (true, saved, $"Underground tunnel bypass available; estimated savings: {saved:0.0}h.");
        }

        public void TickDay(int currentDay)
        {
            foreach (var seg in _state.Segments)
            {
                if (seg.Status == TunnelStatus.Collapsed) continue;

                // Slow structural wear
                seg.StructuralIntegrity = Math.Clamp(seg.StructuralIntegrity - 0.75f, 0f, 100f);

                if (seg.StructuralIntegrity <= 0f)
                {
                    seg.Status = TunnelStatus.Collapsed;
                    OnSegmentCollapsed?.Invoke(seg);
                }
                else if (seg.StructuralIntegrity < 25f)
                {
                    if (!seg.Hazards.Contains(TunnelHazardType.CollapseRisk))
                    {
                        seg.Hazards.Add(TunnelHazardType.CollapseRisk);
                    }
                }
            }
        }

        public IReadOnlyList<TunnelSegment> GetDiscoveredSegments()
        {
            return _state.Segments.Where(s => s.IsDiscovered).ToList();
        }

        /// <summary>
        /// Plan 167 — clears the live network so an authored catalog can be
        /// seeded over the built-in canonical fallback. Never called on a
        /// restored campaign, so a player's discovered/repaired network is
        /// preserved.
        /// </summary>
        public void Clear()
        {
            _state.Segments.Clear();
            _state.Junctions.Clear();
            _state.NextSequence = 1;
        }

        /// <summary>
        /// Read-only summary of live tunnel state (Plan 167). Exposed for the
        /// architecture scanner, the map presentation, and the host probe.
        /// </summary>
        public TunnelNetworkCensus GetCensus()
        {
            int discovered = 0;
            int collapsed = 0;
            int impassable = 0;
            int hazardous = 0;
            float integritySum = 0f;

            for (int i = 0; i < _state.Segments.Count; i++)
            {
                var segment = _state.Segments[i];
                if (segment == null) continue;
                if (segment.IsDiscovered) discovered++;
                if (segment.Status == TunnelStatus.Collapsed) collapsed++;
                else if (segment.Status == TunnelStatus.Impassable) impassable++;
                if (segment.Hazards != null && segment.Hazards.Count > 0) hazardous++;
                integritySum += segment.StructuralIntegrity;
            }

            float average = _state.Segments.Count > 0 ? integritySum / _state.Segments.Count : 0f;
            return new TunnelNetworkCensus(
                _state.Segments.Count,
                discovered,
                _state.Junctions.Count,
                collapsed,
                impassable,
                hazardous,
                average);
        }

        public TunnelNetworkState CaptureState()
        {
            var state = new TunnelNetworkState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                Segments = new List<TunnelSegment>(_state.Segments.Count),
                Junctions = new List<TunnelJunction>(_state.Junctions.Count)
            };

            foreach (var s in _state.Segments)
            {
                state.Segments.Add(new TunnelSegment
                {
                    SegmentId = s.SegmentId,
                    SegmentName = s.SegmentName,
                    ConnectsFrom = s.ConnectsFrom,
                    ConnectsTo = s.ConnectsTo,
                    LengthHours = s.LengthHours,
                    Difficulty = s.Difficulty,
                    Hazards = new List<TunnelHazardType>(s.Hazards),
                    IsDiscovered = s.IsDiscovered,
                    StructuralIntegrity = s.StructuralIntegrity,
                    Status = s.Status
                });
            }

            foreach (var j in _state.Junctions)
            {
                state.Junctions.Add(new TunnelJunction
                {
                    JunctionId = j.JunctionId,
                    JunctionName = j.JunctionName,
                    ConnectedSegmentIds = new List<string>(j.ConnectedSegmentIds),
                    IsDiscovered = j.IsDiscovered,
                    HasResourceDeposit = j.HasResourceDeposit,
                    ResourceDepositType = j.ResourceDepositType
                });
            }

            return state;
        }

        public void RestoreState(TunnelNetworkState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            // Schema gate: a newer payload must not be silently down-cast; a
            // payload that omits the field is the original v1 shape and loads.
            if (state.SchemaVersion > 1)
                throw new InvalidOperationException(
                    $"TunnelNetworkState schema {state.SchemaVersion} is newer than supported (1).");
            if (state.SchemaVersion < 1)
                state.SchemaVersion = 1;

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.Segments.Clear();
            _state.Junctions.Clear();

            if (state.Segments != null)
            {
                foreach (var s in state.Segments)
                {
                    _state.Segments.Add(new TunnelSegment
                    {
                        SegmentId = s.SegmentId,
                        SegmentName = s.SegmentName,
                        ConnectsFrom = s.ConnectsFrom,
                        ConnectsTo = s.ConnectsTo,
                        LengthHours = s.LengthHours,
                        Difficulty = s.Difficulty,
                        Hazards = new List<TunnelHazardType>(s.Hazards ?? Enumerable.Empty<TunnelHazardType>()),
                        IsDiscovered = s.IsDiscovered,
                        StructuralIntegrity = s.StructuralIntegrity,
                        Status = s.Status
                    });
                }
            }

            if (state.Junctions != null)
            {
                foreach (var j in state.Junctions)
                {
                    _state.Junctions.Add(new TunnelJunction
                    {
                        JunctionId = j.JunctionId,
                        JunctionName = j.JunctionName,
                        ConnectedSegmentIds = new List<string>(j.ConnectedSegmentIds ?? Enumerable.Empty<string>()),
                        IsDiscovered = j.IsDiscovered,
                        HasResourceDeposit = j.HasResourceDeposit,
                        ResourceDepositType = j.ResourceDepositType
                    });
                }
            }
        }
    }

    /// <summary>
    /// Read-only census of live tunnel-network state (Plan 167). Exposed for the
    /// architecture scanner, the map panel, and the host self-test probe.
    /// </summary>
    public struct TunnelNetworkCensus
    {
        public int TotalSegments { get; }
        public int DiscoveredSegments { get; }
        public int TotalJunctions { get; }
        public int CollapsedSegments { get; }
        public int ImpassableSegments { get; }
        public int HazardousSegments { get; }
        public float AverageIntegrity { get; }

        public TunnelNetworkCensus(
            int totalSegments,
            int discoveredSegments,
            int totalJunctions,
            int collapsedSegments,
            int impassableSegments,
            int hazardousSegments,
            float averageIntegrity)
        {
            TotalSegments = totalSegments;
            DiscoveredSegments = discoveredSegments;
            TotalJunctions = totalJunctions;
            CollapsedSegments = collapsedSegments;
            ImpassableSegments = impassableSegments;
            HazardousSegments = hazardousSegments;
            AverageIntegrity = averageIntegrity;
        }
    }
}
