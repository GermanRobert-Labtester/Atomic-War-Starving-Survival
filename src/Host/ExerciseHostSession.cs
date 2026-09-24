// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ExerciseSaveStore
// Core State : Ashfall.Core.Survivors.ExerciseSystemState
// Host Caller: Main.Exercise
// Purpose    : Plan 216 — Survivor Exercise & Physical Training host session & persistence.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class ExerciseSaveStore
    {
        public const string FileName = "exercise_save.json";
        public const string SectionName = "exercise";

        private static readonly SaveStore<ExerciseSystemState> s_store =
            SaveStoreHub.Checksummed<ExerciseSystemState>(FileName, nameof(ExerciseSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(ExerciseSystemState state) => s_store.CaptureBare(state);
        public static ExerciseSystemState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(ExerciseSystemState state) => s_store.TrySave(state);
        public static ExerciseSystemState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Plan 216 host session. Wraps <see cref="ExerciseSystem"/>.
    /// Exposes athletic conditioning, workouts, routine assignment, and deconditioning tracking.
    /// </summary>
    public sealed class ExerciseHostSession : HostSessionBase
    {
        private readonly ExerciseSystem _system;

        public ExerciseSystem System => _system;
        public ExerciseCensus Census => _system.GetCensus();
        public string LastEvent { get; private set; } = string.Empty;

        public ExerciseHostSession(ExerciseSystemState? state = null)
        {
            _system = new ExerciseSystem(state);
            _system.OnWorkoutCompleted += res =>
            {
                LastEvent = $"Workout completed: {res.SurvivorId} performed {res.RoutineType} (Day {res.Day}).";
                RaiseStateChanged();
            };
            _system.OnDeconditioned += (p, diff) =>
            {
                LastEvent = $"Deconditioning alert: {p.SurvivorId} lost {diff:F2} fitness.";
                RaiseStateChanged();
            };
        }

        public static ExerciseHostSession Create(ExerciseSystemState? state = null) =>
            new ExerciseHostSession(state);

        public void LoadCatalog(string json)
        {
            _system.LoadCatalog(json);
            LastEvent = $"Loaded {_system.AvailableRoutineCount} exercise routines.";
            RaiseStateChanged();
        }

        public FitnessProfile GetOrCreateProfile(string survivorId, float initialBase = 30f)
        {
            var p = _system.GetOrCreateProfile(survivorId, initialBase);
            RaiseStateChanged();
            return p;
        }

        public WorkoutResult? ExecuteRoutine(string survivorId, string routineId, int currentDay, float intensityMultiplier = 1.0f)
        {
            var res = _system.ExecuteRoutine(survivorId, routineId, currentDay, intensityMultiplier);
            if (res != null) RaiseStateChanged();
            return res;
        }

        public WorkoutResult ExecuteWorkout(string survivorId, WorkoutRoutineType routine, int currentDay, float intensity = 1.0f)
        {
            var res = _system.ExecuteWorkout(survivorId, routine, currentDay, intensity);
            RaiseStateChanged();
            return res;
        }


        public void TickDay(int currentDay)
        {
            _system.TickDay(currentDay);
            LastEvent = $"Ticked survivor exercise on day {currentDay}.";
            RaiseStateChanged();
        }

        public ExerciseSystemState CaptureState() => _system.CaptureState();
        public void RestoreState(ExerciseSystemState state)
        {
            _system.RestoreState(state);
            LastEvent = "Restored exercise state.";
            RaiseStateChanged();
        }

        public bool TrySave() => ExerciseSaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = ExerciseSaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
