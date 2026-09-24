// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : SurvivorRoutineSaveStore
// Core State : Ashfall.Core.Survivors.SurvivorRoutineState
// Host Caller: Main.SurvivorRoutines (SetupSurvivorRoutines / SaveSurvivorRoutines)
// Purpose    : Plan 188 — Individual survivor daily routines system:
//              hourly schedules, activity time blocks, chronotype preferences,
//              routine satisfaction evaluation, and interpersonal conflicts.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class SurvivorRoutineSaveStore
    {
        public const string FileName = "survivor_routines_save.json";
        public const string SectionName = "survivor_routines";

        private static readonly SaveStore<SurvivorRoutineState> s_store =
            SaveStoreHub.Checksummed<SurvivorRoutineState>(FileName, nameof(SurvivorRoutineSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(SurvivorRoutineState state) => s_store.CaptureBare(state);
        public static SurvivorRoutineState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(SurvivorRoutineState state) => s_store.TrySave(state);
        public static SurvivorRoutineState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Host session manager for Plan 188 (Individual Survivor Daily Routines).
    /// Binds routine templates from routine_templates.json, manages survivor daily schedules,
    /// evaluates hourly activity blocks, tracks daily satisfaction, and detects roommate/workspace conflicts.
    /// </summary>
    public sealed class SurvivorRoutineHostSession : HostSessionBase
    {
        private readonly SurvivorRoutineSystem _system;
        private string _lastEvent = string.Empty;

        public SurvivorRoutineSystem System => _system;
        public string LastEvent => _lastEvent;
        public SurvivorRoutineCensus Census => _system.GetCensus();
        public int TrackedRoutineCount => _system.TrackedRoutineCount;
        public string EnforcementLevel => _system.EnforcementLevel;

        public SurvivorRoutineHostSession(
            string? dataDir = null,
            SurvivorRoutineSystem? system = null)
        {
            _system = system ?? new SurvivorRoutineSystem();

            if (!string.IsNullOrEmpty(dataDir))
            {
                LoadCatalogs(dataDir);
            }

            _system.OnRoutineAssigned += (survivorId, templateId) =>
            {
                _lastEvent = $"Survivor '{survivorId}' assigned to routine template '{templateId}'.";
                RaiseStateChanged();
            };

            _system.OnSatisfactionEvaluated += record =>
            {
                _lastEvent = $"Survivor '{record.SurvivorId}' routine satisfaction: {record.OverallSatisfaction:F1}% (Day {record.Day}).";
                RaiseStateChanged();
            };

            _system.OnConflictDetected += conflict =>
            {
                _lastEvent = $"Routine conflict detected: {conflict.ConflictType} between '{conflict.SurvivorA}' and '{conflict.SurvivorB}'.";
                RaiseStateChanged();
            };

            _system.OnConflictResolved += conflictId =>
            {
                _lastEvent = $"Routine conflict '{conflictId}' resolved.";
                RaiseStateChanged();
            };
        }

        public static SurvivorRoutineHostSession Create(
            string? dataDir = null,
            SurvivorRoutineSystem? system = null)
        {
            return new SurvivorRoutineHostSession(dataDir, system);
        }

        public void LoadCatalogs(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;

            try
            {
                var result = RoutineTemplateCatalogLoader.Load(dataDir);
                if (result.Success && result.Catalog != null)
                {
                    _system.BindValidatedCatalog(result.Catalog);
                    _lastEvent = $"Loaded {result.Catalog.templates.Count} routine templates.";
                    RaiseStateChanged();
                }
                else if (result.Errors.Count > 0)
                {
                    _lastEvent = $"Failed to load routine templates catalog: {string.Join("; ", result.Errors)}";
                }
            }
            catch (Exception ex)
            {
                _lastEvent = $"Exception loading routine templates catalog: {ex.Message}";
            }
        }

        public SurvivorRoutineRecord AssignRoutine(string survivorId, string templateId)
        {
            var record = _system.AssignRoutine(survivorId, templateId);
            RaiseStateChanged();
            return record;
        }

        public SurvivorRoutineRecord? GetRoutine(string survivorId) => _system.GetRoutine(survivorId);

        public void SetPreference(string survivorId, string chronotype, string workShift = "morning", string social = "balanced")
        {
            _system.SetPreference(survivorId, chronotype, workShift, social);
            RaiseStateChanged();
        }

        public RoutinePreferenceRecord? GetPreference(string survivorId) => _system.GetPreference(survivorId);

        public string GetActivityAtHour(string survivorId, int hour) => _system.GetActivityAtHour(survivorId, hour);

        public void SetEnforcementLevel(string level)
        {
            _system.SetEnforcementLevel(level);
            RaiseStateChanged();
        }

        public RoutineSatisfactionRecord EvaluateDailySatisfaction(
            string survivorId, int day, int hoursWorked, int hoursSlept, int mealsHad, int socialHours)
        {
            return _system.EvaluateDailySatisfaction(survivorId, day, hoursWorked, hoursSlept, mealsHad, socialHours);
        }

        public IReadOnlyList<RoutineConflictRecord> DetectConflicts(
            int day,
            Dictionary<string, string>? roomAssignments = null,
            Dictionary<string, string>? workspaceAssignments = null)
        {
            return _system.DetectConflicts(day, roomAssignments, workspaceAssignments);
        }

        public bool ResolveConflict(string conflictId)
        {
            bool ok = _system.ResolveConflict(conflictId);
            if (ok)
            {
                RaiseStateChanged();
            }
            return ok;
        }

        public IReadOnlyList<RoutineConflictRecord> GetActiveConflicts() => _system.GetActiveConflicts();

        public IReadOnlyCollection<RoutineTemplateDef> GetAllTemplates() => _system.GetAllTemplates();

        public RoutineTemplateDef? GetTemplate(string templateId) => _system.GetTemplate(templateId);

        public SurvivorRoutineState CaptureState() => _system.CaptureState();

        public void RestoreState(SurvivorRoutineState? state)
        {
            _system.RestoreState(state);
            RaiseStateChanged();
        }

        public void Reset()
        {
            _system.RestoreState(new SurvivorRoutineState());
            _lastEvent = string.Empty;
            RaiseStateChanged();
        }
    }
}
