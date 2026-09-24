// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : DependencyTaperWithdrawalSaveStore
// Core State : Ashfall.Core.Medical.DependencyTaperState
// Host Caller: Main.DependencyTaperWithdrawal
// Purpose    : Expansion 35 — Chemical Dependency Taper, Withdrawal Management
//              & Care Policy Engine host session and persistence.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Medical;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class DependencyTaperWithdrawalSaveStore
    {
        public const string FileName = "dependency_taper_withdrawal_save.json";
        public const string SectionName = "dependency_taper_withdrawal";

        private static readonly SaveStore<DependencyTaperState> s_store =
            SaveStoreHub.Checksummed<DependencyTaperState>(FileName, nameof(DependencyTaperWithdrawalSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(DependencyTaperState state) => s_store.CaptureBare(state);
        public static DependencyTaperState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(DependencyTaperState state) => s_store.TrySave(state);
        public static DependencyTaperState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Expansion 35 host session. Wraps the stateful
    /// <see cref="DependencyTaperLedger"/> over the signed pure
    /// <see cref="DependencyTaperWithdrawalEngine"/>.
    /// ChemicalDependencySystem remains the underlying dependency ledger authority;
    /// this host session manages active taper schedules and care policies.
    /// </summary>
    public sealed class DependencyTaperWithdrawalHostSession : HostSessionBase
    {
        private readonly DependencyTaperLedger _ledger;

        public DependencyTaperLedger Ledger => _ledger;
        public DependencyTaperCensus Census => _ledger.GetCensus();
        public CarePolicyPosture CurrentPosture => _ledger.CurrentPosture;
        public int SubstituteMedicineStockPermille => _ledger.SubstituteMedicineStockPermille;
        public string LastEvent { get; private set; } = string.Empty;

        public DependencyTaperWithdrawalHostSession(DependencyTaperState? state = null)
        {
            _ledger = new DependencyTaperLedger(state);
        }

        public static DependencyTaperWithdrawalHostSession Create(DependencyTaperState? state = null) =>
            new DependencyTaperWithdrawalHostSession(state);

        public TaperProgramState EnrollProgram(
            string survivorId,
            int dependencyPermille,
            bool isMedicallySupervised,
            int? dailyStepDown = null)
        {
            var prog = _ledger.EnrollProgram(survivorId, dependencyPermille, isMedicallySupervised, dailyStepDown);
            LastEvent = $"Enrolled survivor '{survivorId}' in taper program (step-down {prog.DailyStepDownPermille}\u2030/day, supervised: {isMedicallySupervised}).";
            RaiseStateChanged();
            return prog;
        }

        public TaperDayResult AdvanceProgramDay(string survivorId, bool peerSupportRunToday)
        {
            var result = _ledger.AdvanceProgramDay(survivorId, peerSupportRunToday);
            LastEvent = result.TaperComplete
                ? $"Survivor '{survivorId}' completed taper program successfully."
                : $"Advanced taper for '{survivorId}': dose {result.NewSubstituteDosePermille}\u2030, symptoms {result.Symptoms}.";
            RaiseStateChanged();
            return result;
        }

        public List<(string SurvivorId, TaperDayResult Result)> AdvanceAll(bool peerSupportRunToday)
        {
            var results = _ledger.AdvanceAll(peerSupportRunToday);
            LastEvent = $"Advanced {results.Count} active taper programs.";
            RaiseStateChanged();
            return results;
        }

        public void SetPolicyPosture(CarePolicyPosture posture)
        {
            _ledger.SetPolicyPosture(posture);
            LastEvent = $"Shelter dependency care policy posture set to {posture}.";
            RaiseStateChanged();
        }

        public void SetSubstituteStock(int stockPermille)
        {
            _ledger.SetSubstituteStock(stockPermille);
            RaiseStateChanged();
        }

        public CarePolicyPosture EvaluateCarePolicy(int totalShelterPopulation) =>
            _ledger.EvaluateCarePolicy(totalShelterPopulation);

        public DependencyTaperState CaptureState() => _ledger.CaptureState();
        public void RestoreState(DependencyTaperState? state) => _ledger.RestoreState(state);
        public void Clear()
        {
            _ledger.Clear();
            RaiseStateChanged();
        }
    }
}
