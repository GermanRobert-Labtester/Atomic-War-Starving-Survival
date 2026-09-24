// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 25 — The Iron Road
// Subsystem    : Rail Track Maintenance Ledger (stateful owner over the
//                signed pure RailTrackMaintenanceEngine, DEC-80)
// ============================================================================
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Rail
{
    /// <summary>
    /// Persisted maintenance ledger for the rail corridor, keyed by the canonical
    /// segment id owned by <c>RailwaySystem</c>. This is the gauge/alignment/wear
    /// layer only; topology and dispatch remain with the canonical rail owner.
    /// </summary>
    [Serializable]
    public sealed class RailMaintenanceState
    {
        public int SchemaVersion { get; set; } = 1;
        public Dictionary<string, TrackSegmentState> Segments { get; set; } =
            new Dictionary<string, TrackSegmentState>(StringComparer.Ordinal);

        public RailMaintenanceState Clone() => new RailMaintenanceState
        {
            SchemaVersion = SchemaVersion,
            Segments = new Dictionary<string, TrackSegmentState>(Segments, StringComparer.Ordinal)
        };
    }

    /// <summary>Bounded read model of the rail maintenance ledger.</summary>
    public struct RailMaintenanceCensus
    {
        public int SegmentCount { get; }
        public int BlockedSegments { get; }
        public int DegradedSegments { get; }
        public int AverageWearPermille { get; }
        public int AverageBridgeIntegrityPermille { get; }

        public RailMaintenanceCensus(
            int segmentCount, int blockedSegments, int degradedSegments,
            int averageWearPermille, int averageBridgeIntegrityPermille)
        {
            SegmentCount = segmentCount;
            BlockedSegments = blockedSegments;
            DegradedSegments = degradedSegments;
            AverageWearPermille = averageWearPermille;
            AverageBridgeIntegrityPermille = averageBridgeIntegrityPermille;
        }
    }

    /// <summary>
    /// Owns the mutable per-segment maintenance state and delegates all physical
    /// arithmetic to the signed pure <see cref="RailTrackMaintenanceEngine"/>.
    /// It never owns topology, dispatch, or pathfinding.
    /// </summary>
    public sealed class RailTrackMaintenanceLedger
    {
        private readonly RailMaintenanceState _state;

        public RailTrackMaintenanceLedger(RailMaintenanceState? state = null)
        {
            _state = state ?? new RailMaintenanceState();
        }

        public IReadOnlyDictionary<string, TrackSegmentState> Segments => _state.Segments;
        public int SegmentCount => _state.Segments.Count;

        public RailMaintenanceState CaptureState() => _state.Clone();

        public void RestoreState(RailMaintenanceState? saved)
        {
            if (saved == null) return;
            if (saved.SchemaVersion > _state.SchemaVersion)
                throw new InvalidOperationException(
                    $"rail maintenance schema {saved.SchemaVersion} is newer than supported {_state.SchemaVersion}.");

            _state.SchemaVersion = saved.SchemaVersion <= 0 ? _state.SchemaVersion : saved.SchemaVersion;
            _state.Segments = saved.Segments != null
                ? new Dictionary<string, TrackSegmentState>(saved.Segments, StringComparer.Ordinal)
                : new Dictionary<string, TrackSegmentState>(StringComparer.Ordinal);
        }

        /// <summary>Returns the segment's maintenance state, creating a pristine record if absent.</summary>
        public TrackSegmentState EnsureSegment(string segmentId)
        {
            if (string.IsNullOrWhiteSpace(segmentId)) throw new ArgumentException("segment id required", nameof(segmentId));
            if (!_state.Segments.TryGetValue(segmentId, out var seg))
            {
                seg = new TrackSegmentState { SegmentId = segmentId };
                _state.Segments[segmentId] = seg;
            }
            return seg;
        }

        public bool TryGetSegment(string segmentId, out TrackSegmentState segment) =>
            _state.Segments.TryGetValue(segmentId ?? string.Empty, out segment!);

        /// <summary>
        /// Seeds a pristine maintenance record for every canonical segment that does
        /// not yet have one, carrying the authored bridge load / max-train-mass facts.
        /// Existing records are left untouched so player maintenance is never reset.
        /// </summary>
        public int SeedFromTopology(IEnumerable<(string SegmentId, bool BridgeRequired, float MaxTrainMass)> segments)
        {
            if (segments == null) return 0;
            int seeded = 0;
            foreach (var (id, bridgeRequired, maxTrainMass) in segments)
            {
                if (string.IsNullOrWhiteSpace(id) || _state.Segments.ContainsKey(id)) continue;
                int load = maxTrainMass > 0 ? (int)Math.Round(maxTrainMass) : 150;
                _state.Segments[id] = new TrackSegmentState
                {
                    SegmentId = id,
                    TrackWearPermille = 100,
                    BridgeIntegrityPermille = bridgeRequired ? 1000 : 1000,
                    MaxBridgeLoadTons = load,
                    GaugeStability = TrackGaugeStability.PristineStandard,
                    IsBlockedByDebris = false
                };
                seeded++;
            }
            return seeded;
        }

        /// <summary>Read-only feasibility advisory for dispatch. Never moves a train.</summary>
        public RailTransitEvaluationResult Evaluate(string segmentId, LocomotiveClass loco, int cargoTonnage)
        {
            var segment = EnsureSegment(segmentId);
            return RailTrackMaintenanceEngine.EvaluateTrackFeasibility(segment, loco, cargoTonnage);
        }

        /// <summary>Applies the wear a completed run puts on a segment. Returns the resulting evaluation.</summary>
        public RailTransitEvaluationResult ApplyRun(string segmentId, LocomotiveClass loco, int cargoTonnage)
        {
            var segment = EnsureSegment(segmentId);
            RailTrackMaintenanceEngine.ApplyTrainWear(segment, loco, cargoTonnage);
            return RailTrackMaintenanceEngine.EvaluateTrackFeasibility(segment, loco, cargoTonnage);
        }

        /// <summary>Performs a workgang repair pass on a segment.</summary>
        public bool Maintain(string segmentId, int repairMaterialPermille, int workgangLaborHours)
        {
            if (!_state.Segments.TryGetValue(segmentId ?? string.Empty, out var segment)) return false;
            RailTrackMaintenanceEngine.PerformMaintenance(segment, repairMaterialPermille, workgangLaborHours);
            return true;
        }

        /// <summary>Sets/clears the debris-blocked flag (obstruction clearing is a separate act).</summary>
        public bool SetBlocked(string segmentId, bool blocked)
        {
            if (!_state.Segments.TryGetValue(segmentId ?? string.Empty, out var segment)) return false;
            segment.IsBlockedByDebris = blocked;
            return true;
        }

        public RailMaintenanceCensus GetCensus()
        {
            if (_state.Segments.Count == 0) return default;

            int blocked = 0, degraded = 0, wearSum = 0, bridgeSum = 0;
            foreach (var seg in _state.Segments.Values)
            {
                if (seg.IsBlockedByDebris) blocked++;
                if (seg.GaugeStability != TrackGaugeStability.PristineStandard) degraded++;
                wearSum += seg.TrackWearPermille;
                bridgeSum += seg.BridgeIntegrityPermille;
            }
            int n = _state.Segments.Count;
            return new RailMaintenanceCensus(n, blocked, degraded, wearSum / n, bridgeSum / n);
        }

        public void Clear() => _state.Segments.Clear();
    }
}
