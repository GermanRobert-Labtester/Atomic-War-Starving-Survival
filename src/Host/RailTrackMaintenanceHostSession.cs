// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : RailTrackMaintenanceSaveStore
// Core State : Ashfall.Core.Rail.RailMaintenanceState
// Host Caller: Main.RailTrackMaintenance
// Purpose    : Expansion 25 — per-segment rail gauge/wear/bridge maintenance.
//              Topology and dispatch remain owned by RailwaySystem; this section
//              is the physical maintenance ledger only.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Rail;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class RailTrackMaintenanceSaveStore
    {
        public const string FileName = "rail_track_maintenance_save.json";
        public const string SectionName = "rail_track_maintenance";

        private static readonly SaveStore<RailMaintenanceState> s_store =
            SaveStoreHub.Checksummed<RailMaintenanceState>(FileName, nameof(RailTrackMaintenanceSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(RailMaintenanceState state) => s_store.CaptureBare(state);
        public static RailMaintenanceState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(RailMaintenanceState state) => s_store.TrySave(state);
        public static RailMaintenanceState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Expansion 25 host session. Wraps the stateful
    /// <see cref="RailTrackMaintenanceLedger"/> over the signed pure
    /// <see cref="RailTrackMaintenanceEngine"/>. It seeds segment ids from the
    /// canonical <c>RailwaySystem</c> topology and only ever reports feasibility —
    /// it never moves a train or owns a route.
    /// </summary>
    public sealed class RailTrackMaintenanceHostSession : HostSessionBase
    {
        private readonly RailTrackMaintenanceLedger _ledger;

        public RailTrackMaintenanceLedger Ledger => _ledger;
        public RailMaintenanceCensus Census => _ledger.GetCensus();
        public int SegmentCount => _ledger.SegmentCount;
        public string LastEvent { get; private set; } = string.Empty;

        public RailTrackMaintenanceHostSession(RailMaintenanceState? state = null)
        {
            _ledger = new RailTrackMaintenanceLedger(state);
        }

        public static RailTrackMaintenanceHostSession Create(RailMaintenanceState? state = null) =>
            new RailTrackMaintenanceHostSession(state);

        /// <summary>Seeds pristine records for canonical segments that have none.</summary>
        public int SeedFromTopology(IEnumerable<(string SegmentId, bool BridgeRequired, float MaxTrainMass)> segments)
        {
            int seeded = _ledger.SeedFromTopology(segments);
            if (seeded > 0)
            {
                LastEvent = $"Seeded {seeded} rail maintenance segment(s) from topology.";
                RaiseStateChanged();
            }
            return seeded;
        }

        public RailTransitEvaluationResult Evaluate(string segmentId, LocomotiveClass loco, int cargoTonnage) =>
            _ledger.Evaluate(segmentId, loco, cargoTonnage);

        /// <summary>Applied when a train departs a segment: wears track and bridge.</summary>
        public RailTransitEvaluationResult RecordRun(string segmentId, LocomotiveClass loco, int cargoTonnage)
        {
            var result = _ledger.ApplyRun(segmentId, loco, cargoTonnage);
            LastEvent = $"Rail run on '{segmentId}': wear updated (derailment risk {result.DerailmentRiskPermille}\u2030).";
            RaiseStateChanged();
            return result;
        }

        public bool Maintain(string segmentId, int repairMaterialPermille, int workgangLaborHours)
        {
            bool ok = _ledger.Maintain(segmentId, repairMaterialPermille, workgangLaborHours);
            if (ok)
            {
                LastEvent = $"Maintenance gang worked segment '{segmentId}'.";
                RaiseStateChanged();
            }
            return ok;
        }

        public bool SetBlocked(string segmentId, bool blocked)
        {
            bool ok = _ledger.SetBlocked(segmentId, blocked);
            if (ok)
            {
                LastEvent = blocked ? $"Segment '{segmentId}' declared blocked." : $"Segment '{segmentId}' cleared of debris.";
                RaiseStateChanged();
            }
            return ok;
        }

        public bool TryGetSegment(string segmentId, out TrackSegmentState segment) =>
            _ledger.TryGetSegment(segmentId, out segment);

        public RailMaintenanceState CaptureState() => _ledger.CaptureState();
        public void RestoreState(RailMaintenanceState? state) => _ledger.RestoreState(state);
        public void Clear() => _ledger.Clear();
    }
}
