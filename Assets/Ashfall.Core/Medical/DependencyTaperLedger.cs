// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 35 — The Habit
// Subsystem    : Chemical Dependency Taper, Withdrawal Management & Care Policy Ledger
// Authority    : docs/expansions/wave5/expansion_35_the_habit_plan.md
//                DEC-88, DEC-330
// ============================================================================
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// Persisted state for chemical dependency taper programs, withdrawal management,
    /// and shelter care policy posture.
    /// </summary>
    [Serializable]
    public sealed class DependencyTaperState
    {
        public int SchemaVersion { get; set; } = 1;
        public CarePolicyPosture CurrentPosture { get; set; } = CarePolicyPosture.Monitored;
        public int SubstituteMedicineStockPermille { get; set; } = 1000;
        public Dictionary<string, TaperProgramState> ActivePrograms { get; set; } =
            new Dictionary<string, TaperProgramState>(StringComparer.Ordinal);
        public List<string> CompletedSurvivorIds { get; set; } = new List<string>();

        public DependencyTaperState Clone()
        {
            var clone = new DependencyTaperState
            {
                SchemaVersion = SchemaVersion,
                CurrentPosture = CurrentPosture,
                SubstituteMedicineStockPermille = SubstituteMedicineStockPermille,
                ActivePrograms = new Dictionary<string, TaperProgramState>(ActivePrograms.Count, StringComparer.Ordinal),
                CompletedSurvivorIds = new List<string>(CompletedSurvivorIds)
            };

            foreach (var kvp in ActivePrograms)
            {
                clone.ActivePrograms[kvp.Key] = kvp.Value.Clone();
            }

            return clone;
        }
    }

    /// <summary>
    /// Bounded read model for the dependency taper and withdrawal management subsystem.
    /// </summary>
    public struct DependencyTaperCensus
    {
        public int ActiveProgramsCount { get; }
        public int CompletedProgramsCount { get; }
        public int MedicallySupervisedCount { get; }
        public int CriticalBurdenCount { get; }
        public CarePolicyPosture CurrentPosture { get; }
        public int SubstituteMedicineStockPermille { get; }

        public DependencyTaperCensus(
            int activeProgramsCount,
            int completedProgramsCount,
            int medicallySupervisedCount,
            int criticalBurdenCount,
            CarePolicyPosture currentPosture,
            int substituteMedicineStockPermille)
        {
            ActiveProgramsCount = activeProgramsCount;
            CompletedProgramsCount = completedProgramsCount;
            MedicallySupervisedCount = medicallySupervisedCount;
            CriticalBurdenCount = criticalBurdenCount;
            CurrentPosture = currentPosture;
            SubstituteMedicineStockPermille = substituteMedicineStockPermille;
        }
    }

    /// <summary>
    /// Owns the mutable state of active taper programs and shelter care policy posture,
    /// delegating all calculation and tier classification to the signed pure
    /// <see cref="DependencyTaperWithdrawalEngine"/>.
    /// ChemicalDependencySystem remains the underlying dependency ledger authority;
    /// this ledger manages the treatment and tapering overlay.
    /// </summary>
    public sealed class DependencyTaperLedger
    {
        private readonly DependencyTaperState _state;

        public DependencyTaperLedger(DependencyTaperState? state = null)
        {
            _state = state ?? new DependencyTaperState();
        }

        public IReadOnlyDictionary<string, TaperProgramState> ActivePrograms => _state.ActivePrograms;
        public IReadOnlyList<string> CompletedSurvivorIds => _state.CompletedSurvivorIds;
        public CarePolicyPosture CurrentPosture => _state.CurrentPosture;
        public int SubstituteMedicineStockPermille => _state.SubstituteMedicineStockPermille;

        public DependencyTaperState CaptureState() => _state.Clone();

        public void RestoreState(DependencyTaperState? saved)
        {
            if (saved == null) return;
            if (saved.SchemaVersion > _state.SchemaVersion)
            {
                throw new InvalidOperationException(
                    $"dependency taper schema {saved.SchemaVersion} is newer than supported {_state.SchemaVersion}.");
            }

            _state.SchemaVersion = saved.SchemaVersion <= 0 ? _state.SchemaVersion : saved.SchemaVersion;
            _state.CurrentPosture = saved.CurrentPosture;
            _state.SubstituteMedicineStockPermille = Math.Clamp(saved.SubstituteMedicineStockPermille, 0, 1000);

            _state.ActivePrograms.Clear();
            if (saved.ActivePrograms != null)
            {
                foreach (var kvp in saved.ActivePrograms)
                {
                    if (kvp.Value != null)
                    {
                        _state.ActivePrograms[kvp.Key] = kvp.Value.Clone();
                    }
                }
            }

            _state.CompletedSurvivorIds.Clear();
            if (saved.CompletedSurvivorIds != null)
            {
                _state.CompletedSurvivorIds.AddRange(saved.CompletedSurvivorIds);
            }
        }

        public void SetPolicyPosture(CarePolicyPosture posture)
        {
            _state.CurrentPosture = posture;
        }

        public void SetSubstituteStock(int stockPermille)
        {
            _state.SubstituteMedicineStockPermille = Math.Clamp(stockPermille, 0, 1000);
        }

        public bool TryGetProgram(string survivorId, out TaperProgramState? program)
        {
            if (string.IsNullOrWhiteSpace(survivorId))
            {
                program = null;
                return false;
            }
            return _state.ActivePrograms.TryGetValue(survivorId, out program);
        }

        public TaperProgramState EnrollProgram(
            string survivorId,
            int dependencyPermille,
            bool isMedicallySupervised,
            int? dailyStepDown = null)
        {
            if (string.IsNullOrWhiteSpace(survivorId))
                throw new ArgumentException("Survivor ID cannot be null or empty.", nameof(survivorId));

            int stepDown = dailyStepDown ?? DependencyTaperWithdrawalEngine.ComputeRecommendedStepDown(
                dependencyPermille,
                isMedicallySupervised,
                _state.SubstituteMedicineStockPermille);

            var program = new TaperProgramState
            {
                SurvivorId = survivorId,
                DependencyPermille = Math.Clamp(dependencyPermille, 0, 1000),
                DailyStepDownPermille = Math.Clamp(stepDown, 10, 500),
                CurrentSubstituteDosePermille = 1000,
                PeerSupportSessionsCompleted = 0,
                IsMedicallySupervised = isMedicallySupervised,
                TaperDaysElapsed = 0
            };

            _state.ActivePrograms[survivorId] = program;
            return program;
        }

        public TaperDayResult AdvanceProgramDay(string survivorId, bool peerSupportRunToday)
        {
            if (string.IsNullOrWhiteSpace(survivorId))
                throw new ArgumentException("Survivor ID cannot be null or empty.", nameof(survivorId));

            if (!_state.ActivePrograms.TryGetValue(survivorId, out var program))
                throw new InvalidOperationException($"No active taper program for survivor '{survivorId}'.");

            var result = DependencyTaperWithdrawalEngine.AdvanceTaperDay(program, peerSupportRunToday);

            if (result.TaperComplete)
            {
                _state.ActivePrograms.Remove(survivorId);
                if (!_state.CompletedSurvivorIds.Contains(survivorId))
                {
                    _state.CompletedSurvivorIds.Add(survivorId);
                }
            }

            return result;
        }

        public List<(string SurvivorId, TaperDayResult Result)> AdvanceAll(bool peerSupportRunToday)
        {
            var results = new List<(string SurvivorId, TaperDayResult Result)>(_state.ActivePrograms.Count);
            var survivorIds = new List<string>(_state.ActivePrograms.Keys);

            foreach (var id in survivorIds)
            {
                if (_state.ActivePrograms.ContainsKey(id))
                {
                    var res = AdvanceProgramDay(id, peerSupportRunToday);
                    results.Add((id, res));
                }
            }

            return results;
        }

        public CarePolicyPosture EvaluateCarePolicy(int totalShelterPopulation)
        {
            int criticalCases = 0;
            foreach (var program in _state.ActivePrograms.Values)
            {
                if (DependencyTaperWithdrawalEngine.ClassifyDependencySeverity(program.DependencyPermille) == DependencySeverityTier.Critical)
                {
                    criticalCases++;
                }
            }

            return DependencyTaperWithdrawalEngine.RecommendCarePolicy(
                criticalCases,
                totalShelterPopulation,
                _state.CurrentPosture);
        }

        public DependencyTaperCensus GetCensus()
        {
            int supervised = 0;
            int critical = 0;

            foreach (var program in _state.ActivePrograms.Values)
            {
                if (program.IsMedicallySupervised) supervised++;
                if (DependencyTaperWithdrawalEngine.ClassifyDependencySeverity(program.DependencyPermille) == DependencySeverityTier.Critical)
                {
                    critical++;
                }
            }

            return new DependencyTaperCensus(
                activeProgramsCount: _state.ActivePrograms.Count,
                completedProgramsCount: _state.CompletedSurvivorIds.Count,
                medicallySupervisedCount: supervised,
                criticalBurdenCount: critical,
                currentPosture: _state.CurrentPosture,
                substituteMedicineStockPermille: _state.SubstituteMedicineStockPermille);
        }

        public void Clear()
        {
            _state.ActivePrograms.Clear();
            _state.CompletedSurvivorIds.Clear();
            _state.CurrentPosture = CarePolicyPosture.Monitored;
            _state.SubstituteMedicineStockPermille = 1000;
        }
    }
}
