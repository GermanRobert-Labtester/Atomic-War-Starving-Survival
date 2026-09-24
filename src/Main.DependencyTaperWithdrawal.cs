// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Expansion 35 — The Habit: Dependency Taper, Withdrawal Management
// & Care Policy host wiring.
// The signed pure DependencyTaperWithdrawalEngine (DEC-88) is the taper schedule,
// withdrawal symptom band, and care policy posture calculation authority.
// ChemicalDependencySystem remains the underlying dependency ledger authority;
// this host owns the active taper program and care policy management.
// ============================================================================

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private DependencyTaperWithdrawalHostSession? _dependencyTaper;
        private bool _dependencyTaperDirty;

        public DependencyTaperWithdrawalHostSession? DependencyTaper => _dependencyTaper;

        public void SetupDependencyTaperWithdrawal()
        {
            if (_dependencyTaper != null) return;

            var saved = DependencyTaperWithdrawalSaveStore.TryLoad();
            _dependencyTaper = DependencyTaperWithdrawalHostSession.Create(saved);
            _dependencyTaper.StateChanged += () => _dependencyTaperDirty = true;
        }

        public TaperProgramState EnrollDependencyTaper(
            string survivorId,
            int dependencyPermille,
            bool isMedicallySupervised,
            int? dailyStepDown = null)
        {
            SetupDependencyTaperWithdrawal();
            return _dependencyTaper!.EnrollProgram(survivorId, dependencyPermille, isMedicallySupervised, dailyStepDown);
        }

        public TaperDayResult AdvanceDependencyTaperDay(string survivorId, bool peerSupportRunToday)
        {
            SetupDependencyTaperWithdrawal();
            return _dependencyTaper!.AdvanceProgramDay(survivorId, peerSupportRunToday);
        }

        public List<(string SurvivorId, TaperDayResult Result)> AdvanceAllDependencyTapers(bool peerSupportRunToday)
        {
            SetupDependencyTaperWithdrawal();
            return _dependencyTaper!.AdvanceAll(peerSupportRunToday);
        }

        public void SetDependencyCarePosture(CarePolicyPosture posture)
        {
            SetupDependencyTaperWithdrawal();
            _dependencyTaper!.SetPolicyPosture(posture);
        }

        public void SetDependencySubstituteStock(int stockPermille)
        {
            SetupDependencyTaperWithdrawal();
            _dependencyTaper!.SetSubstituteStock(stockPermille);
        }

        public CarePolicyPosture EvaluateDependencyCarePolicy(int totalShelterPopulation)
        {
            SetupDependencyTaperWithdrawal();
            return _dependencyTaper!.EvaluateCarePolicy(totalShelterPopulation);
        }

        public DependencyTaperCensus GetDependencyTaperCensus() =>
            _dependencyTaper?.Census ?? default;

        public void SaveDependencyTaperWithdrawal()
        {
            if (_dependencyTaper == null) return;
            var state = _dependencyTaper.CaptureState();
            DependencyTaperWithdrawalSaveStore.TrySave(state);
            if (CaptureSection(DependencyTaperWithdrawalSaveStore.SectionName, DependencyTaperWithdrawalSaveStore.TryCapturePersisted(state)))
                _dependencyTaperDirty = false;
        }

        public void FlushDependencyTaperWithdrawalIfDirty()
        {
            if (_dependencyTaperDirty)
                SaveDependencyTaperWithdrawal();
        }

        public void ResetDependencyTaperWithdrawal()
        {
            _dependencyTaper = null;
            _dependencyTaperDirty = false;
        }
    }
}
