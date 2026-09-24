// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Plan 188 — Individual Survivor Daily Routines System Host Wiring.
// SurvivorRoutineSystem is the sole authority for daily routine assignments,
// hourly activity schedules, chronotype preferences, satisfaction evaluation,
// and interpersonal schedule conflicts.
// ============================================================================

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private SurvivorRoutineHostSession? _survivorRoutines;
        private bool _survivorRoutinesDirty;

        public SurvivorRoutineHostSession? SurvivorRoutines => _survivorRoutines;

        public void SetupSurvivorRoutines()
        {
            if (_survivorRoutines != null) return;

            _survivorRoutines = SurvivorRoutineHostSession.Create(_dataDir);

            var saved = SurvivorRoutineSaveStore.TryLoad();
            if (saved != null)
            {
                _survivorRoutines.RestoreState(saved);
            }

            _survivorRoutines.StateChanged += () => _survivorRoutinesDirty = true;

            // Wire routine provider to SurvivorDetailPanel if available
            if (_survivorDetailPanel != null)
            {
                _survivorDetailPanel.RoutineProvider = id => _survivorRoutines.GetRoutine(id);
            }
        }

        public void TickSurvivorRoutines(int day)
        {
            if (_survivorRoutines == null) SetupSurvivorRoutines();
            if (_survivorRoutines == null) return;

            // Default daily satisfaction estimate from roster state.
            // hoursWorked / hoursSlept / mealsHad / socialHours are approximated from needs state.
            if (_survivors?.RosterState != null)
            {
                for (int i = 0; i < _survivors.RosterState.Count; i++)
                {
                    var s = _survivors.RosterState[i];
                    if (s == null || string.IsNullOrEmpty(s.Id) || !s.IsAlive) continue;

                    // Approximate daily inputs from needs state (values are thresholds 0–100; higher = more depleted).
                    int hoursWorked = 8;
                    int hoursSlept = s.Fatigue > 50f ? 10 : 8;
                    int mealsHad = s.Hunger > 60f ? 1 : 3;
                    int socialHours = 2;

                    _survivorRoutines.EvaluateDailySatisfaction(
                        s.Id, day, hoursWorked, hoursSlept, mealsHad, socialHours);
                }
            }

            // Detect schedule conflicts from shelter assignments.
            var roomAssignments = BuildRoomAssignments();
            var workAssignments = BuildWorkspaceAssignments();
            if (roomAssignments.Count > 0 || workAssignments.Count > 0)
            {
                _survivorRoutines.DetectConflicts(day, roomAssignments, workAssignments);
            }
        }

        public void SaveSurvivorRoutines()
        {
            if (_survivorRoutines == null) return;
            var state = _survivorRoutines.CaptureState();
            SurvivorRoutineSaveStore.TrySave(state);
            if (CaptureSection(SurvivorRoutineSaveStore.SectionName,
                SurvivorRoutineSaveStore.TryCapturePersisted(state)))
            {
                _survivorRoutinesDirty = false;
            }
        }

        public void FlushSurvivorRoutinesIfDirty()
        {
            if (_survivorRoutinesDirty)
                SaveSurvivorRoutines();
        }

        public SurvivorRoutineCensus GetSurvivorRoutinesCensus() =>
            _survivorRoutines?.Census ?? default;

        public void ResetSurvivorRoutines()
        {
            _survivorRoutines = null;
            _survivorRoutinesDirty = false;
        }

        /// <summary>
        /// Returns survivor→room assignments from current shelter state for conflict detection.
        /// Returns empty when holdfast runtime is not yet wired or doesn't expose assignments.
        /// </summary>
        private Dictionary<string, string> BuildRoomAssignments()
        {
            // TODO: wire to _holdfastRuntime.RoomAssignments when HoldfastRuntimeSession exposes them.
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns survivor→workspace assignments from current shelter state for conflict detection.
        /// Returns empty when holdfast runtime is not yet wired or doesn't expose assignments.
        /// </summary>
        private Dictionary<string, string> BuildWorkspaceAssignments()
        {
            // TODO: wire to _holdfastRuntime.WorkspaceAssignments when HoldfastRuntimeSession exposes them.
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
    }
}
