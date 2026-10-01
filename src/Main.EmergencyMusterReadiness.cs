// SPDX-License-Identifier: MIT
// ============================================================================
// Emergency muster readiness host composition (Expansion 23: The Alarm). The
// Core EmergencyMusterReadinessEngine is the sole authority over the composite
// readiness score, evacuation timing, missing-survivor risk, cascade
// intervention margins, and drill compliance fatigue. The host owns only the
// drill ledger and supplies the census plus corridor/chamber facts.
// ============================================================================

using System;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private EmergencyMusterReadinessHostSession? _musterReadiness;
        private bool _musterReadinessDirty;

        public EmergencyMusterReadinessHostSession? EmergencyMusterReadinessSession => _musterReadiness;

        public void SetupEmergencyMusterReadiness()
        {
            if (_musterReadiness != null) return;
            var saved = EmergencyMusterReadinessSaveStore.TryLoad();
            _musterReadiness = EmergencyMusterReadinessHostSession.Create(saved);
            _musterReadiness.StateChanged += () => _musterReadinessDirty = true;
        }

        /// <summary>Sets the shelter census used for warden-coverage evaluation.</summary>
        public bool SetMusterShelterCensus(int populationCount, int activeWardenCount)
        {
            SetupEmergencyMusterReadiness();
            bool ok = _musterReadiness!.SetShelterCensus(populationCount, activeWardenCount);
            if (ok) _musterReadinessDirty = true;
            return ok;
        }

        /// <summary>
        /// Conducts one emergency drill. Population and warden counts come from
        /// the existing survivor owner; the Core engine decides readiness,
        /// evacuation timing, risk, margins, and fatigue.
        /// </summary>
        public MusterReadinessEvaluationResult ConductEmergencyMusterDrill(
            int day,
            EmergencyDrillType drillType,
            int routeClearancePermille,
            int refugeChamberIntegrityPermille)
        {
            SetupEmergencyMusterReadiness();
            var result = _musterReadiness!.ConductDrill(day, drillType, routeClearancePermille, refugeChamberIntegrityPermille);
            _musterReadinessDirty = true;
            return result;
        }

        public int AdvanceEmergencyMusterReadinessDay(int day)
        {
            SetupEmergencyMusterReadiness();
            _musterReadiness!.AdvanceDay(day);
            _musterReadinessDirty = true;
            return _musterReadiness.DaysSinceLastDrill(day);
        }

        public (int Drills, int Certified, int DaysSinceDrill, int Fatigue) GetEmergencyMusterReadinessReadout()
        {
            SetupEmergencyMusterReadiness();
            return (_musterReadiness!.DrillCount, _musterReadiness.CertedDrillCount,
                    _musterReadiness.DaysSinceLastDrill(_musterReadiness.CurrentDay),
                    _musterReadiness.Drills.Count > 0
                        ? _musterReadiness.Drills[_musterReadiness.Drills.Count - 1].complianceFatiguePermille
                        : 0);
        }

        public void SaveEmergencyMusterReadiness()
        {
            if (_musterReadiness == null) return;
            var state = _musterReadiness.CaptureState();
            if (CaptureSection(EmergencyMusterReadinessSaveStore.SectionName, EmergencyMusterReadinessSaveStore.TryCapturePersisted(state)))
                _musterReadinessDirty = false;
        }

        public void ResetEmergencyMusterReadiness()
        {
            _musterReadiness = null;
            _musterReadinessDirty = false;
        }
    }
}
