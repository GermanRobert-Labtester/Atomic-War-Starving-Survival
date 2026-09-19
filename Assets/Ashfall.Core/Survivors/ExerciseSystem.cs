// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Survivors
{
    public enum WorkoutRoutineType
    {
        Calisthenics = 0,
        CardioDrill = 1,
        StrengthTraining = 2,
        FlexibilityStretching = 3,
        CombatDrill = 4
    }

    [Serializable]
    public sealed class FitnessProfile
    {
        public string SurvivorId { get; set; } = string.Empty;
        public float Cardio { get; set; } = 30f;
        public float Strength { get; set; } = 30f;
        public float Flexibility { get; set; } = 30f;
        public float Endurance { get; set; } = 30f;
        public int LastWorkoutDay { get; set; } = 0;
        public int WorkoutStreak { get; set; } = 0;
        public int TotalWorkoutsCompleted { get; set; } = 0;

        public float OverallConditioning => (Cardio + Strength + Flexibility + Endurance) * 0.25f;
    }

    [Serializable]
    public sealed class WorkoutResult
    {
        public string SurvivorId { get; set; } = string.Empty;
        public WorkoutRoutineType RoutineType { get; set; }
        public int Day { get; set; }
        public float CardioGain { get; set; }
        public float StrengthGain { get; set; }
        public float FlexibilityGain { get; set; }
        public float EnduranceGain { get; set; }
        public float FatigueIncurred { get; set; }
        public bool InjuryOccurred { get; set; }
        public string InjuryReason { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class ExerciseSystemState
    {
        public int SchemaVersion { get; set; } = 1;
        public float BaselineFitnessFloor { get; set; } = 20f;
        public float DeconditioningRatePerDay { get; set; } = 0.5f;
        public List<FitnessProfile> Profiles { get; set; } = new List<FitnessProfile>();
    }

    /// <summary>
    /// Plan 216 / C1[41] — Survivor Exercise & Physical Conditioning System.
    /// Manages progressive athletic conditioning, workout routines, fatigue load,
    /// injury risk, and deconditioning decay across shelter dwellers.
    /// </summary>
    public sealed class ExerciseSystem
    {
        private readonly ExerciseSystemState _state;

        public event Action<WorkoutResult>? OnWorkoutCompleted;
        public event Action<FitnessProfile, float>? OnDeconditioned;

        public int ProfileCount => _state.Profiles.Count;

        public ExerciseSystem(ExerciseSystemState? state = null)
        {
            _state = state ?? new ExerciseSystemState();
        }

        public FitnessProfile GetOrCreateProfile(string survivorId, float initialBase = 30f)
        {
            if (string.IsNullOrEmpty(survivorId)) throw new ArgumentNullException(nameof(survivorId));

            var profile = _state.Profiles.FirstOrDefault(p =>
                string.Equals(p.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));

            if (profile == null)
            {
                profile = new FitnessProfile
                {
                    SurvivorId = survivorId,
                    Cardio = Math.Clamp(initialBase, 10f, 100f),
                    Strength = Math.Clamp(initialBase, 10f, 100f),
                    Flexibility = Math.Clamp(initialBase, 10f, 100f),
                    Endurance = Math.Clamp(initialBase, 10f, 100f),
                    LastWorkoutDay = 0,
                    WorkoutStreak = 0
                };
                _state.Profiles.Add(profile);
            }

            return profile;
        }

        /// <summary>
        /// Non-mutating profile lookup for fitness/read-model projections.
        /// Presentation and capability evaluation must not create persistent
        /// exercise state merely by being refreshed.
        /// </summary>
        public FitnessProfile? TryGetProfile(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            return _state.Profiles.FirstOrDefault(p =>
                string.Equals(p.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));
        }

        public WorkoutResult ExecuteWorkout(
            string survivorId,
            WorkoutRoutineType routine,
            int currentDay,
            float intensity = 1.0f,
            ISeededRng? rng = null)
        {
            var profile = GetOrCreateProfile(survivorId);
            intensity = Math.Clamp(intensity, 0.5f, 2.0f);

            // Update streak
            if (profile.LastWorkoutDay == currentDay - 1)
            {
                profile.WorkoutStreak++;
            }
            else if (profile.LastWorkoutDay < currentDay - 1)
            {
                profile.WorkoutStreak = 1;
            }
            profile.LastWorkoutDay = currentDay;
            profile.TotalWorkoutsCompleted++;

            var result = new WorkoutResult
            {
                SurvivorId = survivorId,
                RoutineType = routine,
                Day = currentDay
            };

            switch (routine)
            {
                case WorkoutRoutineType.Calisthenics:
                    result.CardioGain = 1.0f * intensity;
                    result.StrengthGain = 1.0f * intensity;
                    result.FlexibilityGain = 1.0f * intensity;
                    result.EnduranceGain = 1.0f * intensity;
                    result.FatigueIncurred = 12f * intensity;
                    break;

                case WorkoutRoutineType.CardioDrill:
                    result.CardioGain = 2.5f * intensity;
                    result.EnduranceGain = 2.0f * intensity;
                    result.StrengthGain = 0.5f * intensity;
                    result.FlexibilityGain = 0.2f * intensity;
                    result.FatigueIncurred = 20f * intensity;
                    break;

                case WorkoutRoutineType.StrengthTraining:
                    result.StrengthGain = 3.0f * intensity;
                    result.EnduranceGain = 1.0f * intensity;
                    result.CardioGain = 0.5f * intensity;
                    result.FlexibilityGain = 0.0f;
                    result.FatigueIncurred = 25f * intensity;
                    break;

                case WorkoutRoutineType.FlexibilityStretching:
                    result.FlexibilityGain = 3.0f * intensity;
                    result.EnduranceGain = 0.5f * intensity;
                    result.CardioGain = 0.2f * intensity;
                    result.StrengthGain = 0.2f * intensity;
                    result.FatigueIncurred = 8f * intensity;
                    break;

                case WorkoutRoutineType.CombatDrill:
                    result.CardioGain = 1.8f * intensity;
                    result.StrengthGain = 2.0f * intensity;
                    result.EnduranceGain = 2.0f * intensity;
                    result.FlexibilityGain = 0.8f * intensity;
                    result.FatigueIncurred = 28f * intensity;
                    break;
            }

            // Diminishing returns above 80
            float ApplyGains(float current, float gain)
            {
                float efficiency = current > 80f ? 0.4f : (current > 60f ? 0.7f : 1.0f);
                return Math.Clamp(current + gain * efficiency, 0f, 100f);
            }

            profile.Cardio = ApplyGains(profile.Cardio, result.CardioGain);
            profile.Strength = ApplyGains(profile.Strength, result.StrengthGain);
            profile.Flexibility = ApplyGains(profile.Flexibility, result.FlexibilityGain);
            profile.Endurance = ApplyGains(profile.Endurance, result.EnduranceGain);

            // Injury evaluation: higher intensity & lower flexibility increases risk
            float injuryRisk = (intensity - 0.8f) * 0.05f - (profile.Flexibility * 0.0005f);
            if (injuryRisk > 0.01f && rng != null)
            {
                if (rng.NextDouble() < injuryRisk)
                {
                    result.InjuryOccurred = true;
                    result.InjuryReason = routine == WorkoutRoutineType.StrengthTraining ? "MuscleStrain" : "JointSprain";
                }
            }

            OnWorkoutCompleted?.Invoke(result);
            return result;
        }

        public void TickDay(int currentDay)
        {
            for (int i = 0; i < _state.Profiles.Count; i++)
            {
                var profile = _state.Profiles[i];
                if (profile.LastWorkoutDay <= 0) continue;

                int inactiveDays = currentDay - profile.LastWorkoutDay;
                if (inactiveDays > 2)
                {
                    profile.WorkoutStreak = 0;
                }

                // Deconditioning begins after 3 inactive days
                if (inactiveDays > 3)
                {
                    float decay = _state.DeconditioningRatePerDay;
                    float floor = _state.BaselineFitnessFloor;

                    float oldCond = profile.OverallConditioning;
                    profile.Cardio = Math.Max(floor, profile.Cardio - decay);
                    profile.Strength = Math.Max(floor, profile.Strength - decay);
                    profile.Flexibility = Math.Max(floor, profile.Flexibility - decay);
                    profile.Endurance = Math.Max(floor, profile.Endurance - decay);

                    float diff = oldCond - profile.OverallConditioning;
                    if (diff > 0f)
                    {
                        OnDeconditioned?.Invoke(profile, diff);
                    }
                }
            }
        }

        public float GetFatigueResistanceMultiplier(string survivorId)
        {
            var profile = _state.Profiles.FirstOrDefault(p =>
                string.Equals(p.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));

            if (profile == null) return 1.0f;

            // Conditioning 0 -> 1.25x fatigue accumulation, 50 -> 1.0x, 100 -> 0.75x
            float score = profile.OverallConditioning;
            return Math.Clamp(1.25f - (score * 0.005f), 0.75f, 1.25f);
        }

        public ExerciseSystemState CaptureState()
        {
            var captured = new ExerciseSystemState
            {
                SchemaVersion = _state.SchemaVersion,
                BaselineFitnessFloor = _state.BaselineFitnessFloor,
                DeconditioningRatePerDay = _state.DeconditioningRatePerDay,
                Profiles = new List<FitnessProfile>(_state.Profiles.Count)
            };

            for (int i = 0; i < _state.Profiles.Count; i++)
            {
                var p = _state.Profiles[i];
                captured.Profiles.Add(new FitnessProfile
                {
                    SurvivorId = p.SurvivorId,
                    Cardio = p.Cardio,
                    Strength = p.Strength,
                    Flexibility = p.Flexibility,
                    Endurance = p.Endurance,
                    LastWorkoutDay = p.LastWorkoutDay,
                    WorkoutStreak = p.WorkoutStreak,
                    TotalWorkoutsCompleted = p.TotalWorkoutsCompleted
                });
            }

            return captured;
        }

        public void RestoreState(ExerciseSystemState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.BaselineFitnessFloor = state.BaselineFitnessFloor;
            _state.DeconditioningRatePerDay = state.DeconditioningRatePerDay;
            _state.Profiles.Clear();

            if (state.Profiles != null)
            {
                for (int i = 0; i < state.Profiles.Count; i++)
                {
                    var p = state.Profiles[i];
                    _state.Profiles.Add(new FitnessProfile
                    {
                        SurvivorId = p.SurvivorId,
                        Cardio = p.Cardio,
                        Strength = p.Strength,
                        Flexibility = p.Flexibility,
                        Endurance = p.Endurance,
                        LastWorkoutDay = p.LastWorkoutDay,
                        WorkoutStreak = p.WorkoutStreak,
                        TotalWorkoutsCompleted = p.TotalWorkoutsCompleted
                    });
                }
            }
        }
    }
}
