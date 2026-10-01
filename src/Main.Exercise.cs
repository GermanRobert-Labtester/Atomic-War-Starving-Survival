// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 216 — Survivor Exercise & Physical Training host wiring.
// The pure domain ExerciseSystem is the authority for athletic conditioning,
// workout routines, fatigue load, injury risk, and deconditioning decay.
// ============================================================================

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Survivors;

using Ashfall.Core;
using Ashfall.Core.Random;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ExerciseHostSession? _exercise;
        private bool _exerciseDirty;

        public ExerciseHostSession? Exercise => _exercise;

        public void SetupExercise()
        {
            if (_exercise != null) return;

            var saved = ExerciseSaveStore.TryLoad();
            _exercise = ExerciseHostSession.Create(saved);
            _exercise.StateChanged += () => _exerciseDirty = true;

            // Load authored exercise routines catalog
            string catalogPath = CatalogPath.ResolveCatalog("exercise_routines.json");
            var catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (catalogIo.FileExists(catalogPath))
            {
                _exercise.LoadCatalog(catalogIo.ReadAllText(catalogPath));
            }
        }

        public FitnessProfile GetOrCreateSurvivorFitnessProfile(string survivorId, float initialBase = 30f)
        {
            SetupExercise();
            return _exercise!.GetOrCreateProfile(survivorId, initialBase);
        }

        public WorkoutResult? ExecuteSurvivorExerciseRoutine(string survivorId, string routineId, int currentDay, float intensityMultiplier = 1.0f)
        {
            SetupExercise();
            return _exercise!.ExecuteRoutine(survivorId, routineId, currentDay, intensityMultiplier);
        }

        public WorkoutResult ExecuteSurvivorWorkout(string survivorId, WorkoutRoutineType routine, int currentDay, float intensity = 1.0f)
        {
            SetupExercise();
            return _exercise!.ExecuteWorkout(survivorId, routine, currentDay, intensity);
        }

        public ExerciseCensus GetExerciseCensus() =>
            _exercise?.Census ?? default;

        public void TickExercise(int day)
        {
            SetupExercise();
            _exercise!.TickDay(day);
        }

        public void SaveExercise()
        {
            if (_exercise == null) return;
            var state = _exercise.System.CaptureState();
            if (CaptureSection(
                    ExerciseSaveStore.SectionName,
                    ExerciseSaveStore.TryCapturePersisted(state)))
            {
                _exerciseDirty = false;
            }
        }

        public void ResetExercise()
        {
            _exercise = null;
            _exerciseDirty = false;
        }
        /// <summary>
        /// Execute a workout through the survivor-social coordinator. Fitness is
        /// checked from the existing duty-fitness model; fatigue is attributed
        /// by the coordinator to the canonical Needs owner and the result is
        /// persisted with the existing survivor_social section.
        /// </summary>
        public WorkoutResult? ExecuteSurvivorWorkout(
            string survivorId,
            WorkoutRoutineType routine,
            float intensity = 1f)
        {
            SetupSurvivorSocial();
            var fitness = EvaluateSurvivorFitness(survivorId);
            if (fitness.IsHardBlocked) return null;

            var rng = _campaignDay.Rng.Fork(CampaignStreamIds.Social, _simDay, (int)routine);
            var result = _survivorSocial.ExecuteWorkout(
                survivorId, routine, _simDay, intensity, rng);
            if (result != null)
            {
                _survivorSocialDirty = true;
                RefreshSurvivorSocialReadModel();
            }
            return result;
        }

        /// <summary>Read-only access to the persisted conditioning profile.</summary>
        public FitnessProfile? GetSurvivorFitnessProfile(string survivorId)
        {
            SetupSurvivorSocial();
            if (_survivorSocial == null || string.IsNullOrEmpty(survivorId)) return null;
            return _survivorSocial.Exercise.TryGetProfile(survivorId);
        }

    }
}
