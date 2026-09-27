// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : EmergencyMusterReadinessSaveStore
// Core Engine: Ashfall.Core.Shelter.EmergencyMusterReadinessEngine
// Host Caller: Main.EmergencyMusterReadiness
// Purpose    : Expansion 23 The Alarm — emergency muster readiness. The Core
//              engine is the sole authority over the composite readiness score,
//              evacuation timing, missing-survivor risk, cascade intervention
//              margins, and drill compliance fatigue; the host owns only the
//              drill ledger and its persistence.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persisted emergency readiness state: the drill ledger plus shelter facts.
    /// </summary>
    [Serializable]
    public sealed class EmergencyMusterReadinessSaveState
    {
        public int schema_version = 1;
        public int currentDay;
        public int populationCount;
        public int activeWardenCount;
        public int lastDrillDay = -1;
        public EmergencyDrillType lastDrillType = EmergencyDrillType.None;
        public List<int> drillDays = new List<int>();
        public List<DrillRecord> drills = new List<DrillRecord>();
    }

    /// <summary>One conducted drill and its Core-evaluated readiness outcome.</summary>
    [Serializable]
    public sealed class DrillRecord
    {
        public int day;
        public EmergencyDrillType drillType;
        public int compositeReadinessScorePermille;
        public int estimatedEvacuationMinutes;
        public int missingSurvivorRiskPermille;
        public int cascadeInterventionMarginMinutes;
        public int complianceFatiguePermille;
        public bool isReadinessCertified;
    }

    public static class EmergencyMusterReadinessSaveStore
    {
        public const string FileName = "emergency_muster_readiness_save.json";
        public const string SectionName = "emergency_muster_readiness";

        private static readonly SaveStore<EmergencyMusterReadinessSaveState> s_store =
            SaveStoreHub.Checksummed<EmergencyMusterReadinessSaveState>(FileName, nameof(EmergencyMusterReadinessSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(EmergencyMusterReadinessSaveState state) => s_store.CaptureBare(state);
        public static EmergencyMusterReadinessSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(EmergencyMusterReadinessSaveState state) => s_store.TrySave(state);
        public static EmergencyMusterReadinessSaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>Host session composing the pure Core emergency muster engine.</summary>
    public sealed class EmergencyMusterReadinessHostSession : HostSessionBase
    {
        private readonly EmergencyMusterReadinessSaveState _state;

        public EmergencyMusterReadinessHostSession(EmergencyMusterReadinessSaveState? state = null)
        {
            _state = state ?? new EmergencyMusterReadinessSaveState();
            _state.drillDays ??= new List<int>();
            _state.drills ??= new List<DrillRecord>();
        }

        public static EmergencyMusterReadinessHostSession Create(EmergencyMusterReadinessSaveState? state = null) =>
            new EmergencyMusterReadinessHostSession(state);

        public string LastEvent { get; private set; } = string.Empty;

        public int CurrentDay => _state.currentDay;
        public int PopulationCount { get => _state.populationCount; }
        public int ActiveWardenCount { get => _state.activeWardenCount; }
        public int DrillCount => _state.drills.Count;
        public IReadOnlyList<DrillRecord> Drills => _state.drills;
        public EmergencyDrillType LastDrillType => _state.lastDrillType;

        public bool SetShelterCensus(int populationCount, int activeWardenCount)
        {
            int pop = Math.Max(0, populationCount);
            int wardens = Math.Max(0, activeWardenCount);
            if (pop == _state.populationCount && wardens == _state.activeWardenCount) return false;
            _state.populationCount = pop;
            _state.activeWardenCount = wardens;
            LastEvent = $"Muster census updated: {pop} survivors, {wardens} wardens.";
            RaiseStateChanged();
            return true;
        }

        /// <summary>Days elapsed since the most recent drill; 0 when none is on record.</summary>
        public int DaysSinceLastDrill(int day)
        {
            int best = -1;
            foreach (var d in _state.drillDays)
                if (d <= day && d > best) best = d;
            if (best < 0)
            {
                return _state.lastDrillDay < 0 ? 0 : Math.Max(0, day - _state.lastDrillDay);
            }
            return Math.Max(0, day - best);
        }

        public int RecentDrillCountIn14Days(int day) =>
            _state.drillDays.Count(d => d <= day && day - d < 14);

        /// <summary>
        /// Conducts one drill and evaluates readiness. Every score, timing, risk,
        /// margin, and fatigue outcome is decided by the Core engine; the host
        /// only supplies the census and supplies the corridor/chamber facts.
        /// </summary>
        public MusterReadinessEvaluationResult ConductDrill(
            int day,
            EmergencyDrillType drillType,
            int routeClearancePermille,
            int refugeChamberIntegrityPermille)
        {
            _state.currentDay = day;
            int route = Math.Max(0, Math.Min(1000, routeClearancePermille));
            int chamber = Math.Max(0, Math.Min(1000, refugeChamberIntegrityPermille));

            var result = EmergencyMusterReadinessEngine.Evaluate(
                Math.Max(1, _state.populationCount),
                _state.activeWardenCount,
                DaysSinceLastDrill(day),
                _state.lastDrillType,
                route,
                chamber,
                RecentDrillCountIn14Days(day));

            _state.drillDays.Add(day);
            _state.lastDrillDay = day;
            _state.lastDrillType = drillType;
            _state.drills.Add(new DrillRecord
            {
                day = day,
                drillType = drillType,
                compositeReadinessScorePermille = result.CompositeReadinessScorePermille,
                estimatedEvacuationMinutes = result.EstimatedEvacuationMinutes,
                missingSurvivorRiskPermille = result.MissingSurvivorRiskPermille,
                cascadeInterventionMarginMinutes = result.CascadeInterventionMarginMinutes,
                complianceFatiguePermille = result.ComplianceFatiguePermille,
                isReadinessCertified = result.IsReadinessCertified
            });

            LastEvent = $"Day {day} {drillType} drill: readiness {result.CompositeReadinessScorePermille} permille, evacuation {result.EstimatedEvacuationMinutes} min.";
            RaiseStateChanged();
            return result;
        }

        /// <summary>Advances the readiness clock one day without conducting a drill.</summary>
        public int AdvanceDay(int day)
        {
            _state.currentDay = day;
            LastEvent = $"Muster readiness advanced to day {day}; last drill day {_state.lastDrillDay}.";
            RaiseStateChanged();
            return _state.lastDrillDay;
        }

        public int CertedDrillCount => _state.drills.Count(d => d.isReadinessCertified);

        public EmergencyMusterReadinessSaveState CaptureState()
        {
            var copy = new EmergencyMusterReadinessSaveState
            {
                schema_version = _state.schema_version,
                currentDay = _state.currentDay,
                populationCount = _state.populationCount,
                activeWardenCount = _state.activeWardenCount,
                lastDrillDay = _state.lastDrillDay,
                lastDrillType = _state.lastDrillType
            };
            copy.drillDays.AddRange(_state.drillDays);
            foreach (var d in _state.drills)
            {
                copy.drills.Add(new DrillRecord
                {
                    day = d.day,
                    drillType = d.drillType,
                    compositeReadinessScorePermille = d.compositeReadinessScorePermille,
                    estimatedEvacuationMinutes = d.estimatedEvacuationMinutes,
                    missingSurvivorRiskPermille = d.missingSurvivorRiskPermille,
                    cascadeInterventionMarginMinutes = d.cascadeInterventionMarginMinutes,
                    complianceFatiguePermille = d.complianceFatiguePermille,
                    isReadinessCertified = d.isReadinessCertified
                });
            }
            return copy;
        }

        public void RestoreState(EmergencyMusterReadinessSaveState? state)
        {
            _state.currentDay = state?.currentDay ?? 0;
            _state.populationCount = state?.populationCount ?? 0;
            _state.activeWardenCount = state?.activeWardenCount ?? 0;
            _state.lastDrillDay = state?.lastDrillDay ?? -1;
            _state.lastDrillType = state?.lastDrillType ?? EmergencyDrillType.None;
            _state.drillDays.Clear();
            _state.drills.Clear();
            if (state == null) return;
            _state.schema_version = state.schema_version;
            if (state.drillDays != null) _state.drillDays.AddRange(state.drillDays);
            foreach (var d in state.drills ?? new List<DrillRecord>())
                if (d != null) _state.drills.Add(d);
            LastEvent = "Restored emergency muster readiness state.";
            RaiseStateChanged();
        }

        public bool TrySave() => EmergencyMusterReadinessSaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = EmergencyMusterReadinessSaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
