// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Expansion 25 — The Iron Road: rail track maintenance host wiring.
// The signed pure RailTrackMaintenanceEngine (DEC-80) is the physical
// authority for gauge stability, track wear, and bridge load. RailwaySystem
// remains the sole owner of topology, dispatch, and pathfinding; this host only
// seeds segment ids from that topology and records the wear a completed run
// puts on a segment. It never moves a train.
// ============================================================================

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Rail;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private RailTrackMaintenanceHostSession? _railTrackMaintenance;
        private bool _railTrackMaintenanceDirty;

        public RailTrackMaintenanceHostSession? RailTrackMaintenance => _railTrackMaintenance;

        public void SetupRailTrackMaintenance()
        {
            if (_railTrackMaintenance != null) return;

            var saved = RailTrackMaintenanceSaveStore.TryLoad();
            _railTrackMaintenance = RailTrackMaintenanceHostSession.Create(saved);
            _railTrackMaintenance.StateChanged += () => _railTrackMaintenanceDirty = true;

            SeedRailMaintenanceFromTopology();
        }

        /// <summary>
        /// Seeds pristine maintenance records for every canonical rail segment that
        /// does not yet have one. Existing records are never reset.
        /// </summary>
        private void SeedRailMaintenanceFromTopology()
        {
            if (_railTrackMaintenance == null) return;

            RailwaySystem railway;
            try
            {
                railway = EnsureRailway();
            }
            catch (Exception ex)
            {
                GD.PrintErr("[Ashfall Godot] Rail maintenance seeding skipped: " + ex.Message);
                return;
            }
            if (railway == null) return;

            var segments = new List<(string, bool, float)>();
            foreach (var kv in railway.SegmentDefs)
            {
                var def = kv.Value;
                if (def == null || string.IsNullOrWhiteSpace(def.segment_id)) continue;
                segments.Add((def.segment_id, def.bridge_required, def.max_train_mass));
            }

            int seeded = _railTrackMaintenance.SeedFromTopology(segments);
            if (seeded > 0)
                GD.Print($"[Ashfall Godot] Rail maintenance: seeded {seeded} segment(s) from the canonical topology.");
        }

        /// <summary>
        /// Records the wear a departing train puts on the segment it enters. Called
        /// from the canonical RailwaySystem dispatch event; this never dispatches.
        /// </summary>
        public void RecordRailRun(string? segmentId, LocomotiveClass loco = LocomotiveClass.LightSteamShunter, int cargoTons = 0)
        {
            if (string.IsNullOrWhiteSpace(segmentId)) return;
            SetupRailTrackMaintenance();
            _railTrackMaintenance?.RecordRun(segmentId!, loco, cargoTons);
        }

        /// <summary>
        /// Classifies a dispatched train's total mass into the engine's locomotive
        /// table and records the run's wear on the segment.
        /// </summary>
        public void RecordRailRunFromTrain(RailwaySystem railway, string trainId, string? segmentId)
        {
            if (railway == null || string.IsNullOrWhiteSpace(segmentId)) return;

            int totalTons = 0;
            var train = railway.State?.trains?.Find(t => string.Equals(t.trainId, trainId, StringComparison.Ordinal));
            if (train != null)
                totalTons = (int)Math.Round(railway.CalculateTrainMass(train));

            LocomotiveClass loco = ClassifyLocomotive(totalTons);
            int cargoTons = Math.Max(0, totalTons - RailTrackMaintenanceEngine.GetLocomotiveWeightTons(loco));
            RecordRailRun(segmentId, loco, cargoTons);
        }

        private static LocomotiveClass ClassifyLocomotive(int totalTons) => totalTons switch
        {
            <= 2 => LocomotiveClass.ManualHandcar,
            <= 30 => LocomotiveClass.LightSteamShunter,
            <= 120 => LocomotiveClass.DieselFreightRig,
            _ => LocomotiveClass.ArmoredBattleTrain
        };

        /// <summary>Feasibility advisory for dispatch. Never moves a train.</summary>
        public RailTransitEvaluationResult EvaluateRailTrack(string segmentId, LocomotiveClass loco, int cargoTons)
        {
            SetupRailTrackMaintenance();
            return _railTrackMaintenance?.Evaluate(segmentId, loco, cargoTons)
                ?? RailTrackMaintenanceEngine.EvaluateTrackFeasibility(new Ashfall.Core.Rail.TrackSegmentState { SegmentId = segmentId }, loco, cargoTons);
        }

        public bool MaintainRailSegment(string segmentId, int repairMaterialPermille, int workgangLaborHours)
        {
            SetupRailTrackMaintenance();
            return _railTrackMaintenance?.Maintain(segmentId, repairMaterialPermille, workgangLaborHours) ?? false;
        }

        public RailMaintenanceCensus GetRailTrackMaintenanceCensus() =>
            _railTrackMaintenance?.Census ?? default;

        public void SaveRailTrackMaintenance()
        {
            if (_railTrackMaintenance == null) return;
            var state = _railTrackMaintenance.CaptureState();
            RailTrackMaintenanceSaveStore.TrySave(state);
            if (CaptureSection(RailTrackMaintenanceSaveStore.SectionName, RailTrackMaintenanceSaveStore.TryCapturePersisted(state)))
                _railTrackMaintenanceDirty = false;
        }

        public void FlushRailTrackMaintenanceIfDirty()
        {
            if (_railTrackMaintenanceDirty)
                SaveRailTrackMaintenance();
        }

        public void ResetRailTrackMaintenance()
        {
            _railTrackMaintenance = null;
            _railTrackMaintenanceDirty = false;
        }
    }
}
