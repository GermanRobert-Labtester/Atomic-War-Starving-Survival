using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Runtime state of the radio module: power consumption, signal strength, EMP damage,
    /// currently tuned frequency, and tuning progress. Save/load safe. Updated each tick
    /// by RadioTunerSystem.
    /// </summary>
    [Serializable]
    public class RadioState
    {
        /// <summary>Fuel/electricity consumed per hour of operation.</summary>
        public float PowerConsumptionPerHour = 0.5f;

        /// <summary>Available fuel/electricity for the radio.</summary>
        public float AvailableFuel;

        /// <summary>Current signal strength (0..1). Affected by weather, EMP damage, frequency.</summary>
        public float SignalStrength;

        /// <summary>EMP damage level (0..100). 0 = undamaged, 100 = destroyed.</summary>
        [Range(0f, 100f)]
        public float EmpDamage;

        /// <summary>Currently tuned frequency ID (null = not tuned).</summary>
        public string CurrentFrequencyId;

        /// <summary>Tuning progress (0..1). 1 = fully tuned, ready to extract intel.</summary>
        [Range(0f, 1f)]
        public float TuningProgress;

        /// <summary>Hours spent tuning the current frequency.</summary>
        public float TuningHoursSpent;

        /// <summary>Whether the radio is currently operational (powered, not destroyed).</summary>
        public bool IsOperational => AvailableFuel > 0f && EmpDamage < 100f;

        /// <summary>Whether the radio is currently tuned and ready to extract intel.</summary>
        public bool IsTuned => TuningProgress >= 1f && IsOperational;

        /// <summary>Fired when signal strength changes significantly.</summary>
        public event Action<float> OnSignalStrengthChanged;

        /// <summary>Fired when EMP damage is applied.</summary>
        public event Action<float> OnEmpDamageApplied;

        /// <summary>Fired when tuning completes (progress reaches 1).</summary>
        public event Action<string> OnTuningComplete;

        /// <summary>
        /// Consume fuel over elapsed hours. Returns fuel consumed.
        /// </summary>
        public float ConsumeFuel(float gameHours)
        {
            if (gameHours <= 0f || !IsOperational) return 0f;
            float consumed = Mathf.Min(AvailableFuel, PowerConsumptionPerHour * gameHours);
            AvailableFuel -= consumed;
            return consumed;
        }

        /// <summary>
        /// Refill the radio's fuel supply.
        /// </summary>
        public void Refuel(float amount)
        {
            AvailableFuel = Mathf.Max(0f, AvailableFuel + amount);
        }

        /// <summary>
        /// Apply EMP damage to the radio. Returns true if the radio is now destroyed.
        /// </summary>
        public bool ApplyEmpDamage(float damage)
        {
            if (damage <= 0f) return false;
            float oldDamage = EmpDamage;
            EmpDamage = Mathf.Clamp(EmpDamage + damage, 0f, 100f);
            if (EmpDamage > oldDamage)
            {
                OnEmpDamageApplied?.Invoke(EmpDamage);
            }
            return EmpDamage >= 100f;
        }

        /// <summary>
        /// Repair EMP damage. Returns amount repaired.
        /// </summary>
        public float Repair(float amount)
        {
            if (amount <= 0f) return 0f;
            float oldDamage = EmpDamage;
            EmpDamage = Mathf.Max(0f, EmpDamage - amount);
            return oldDamage - EmpDamage;
        }

        /// <summary>
        /// Update signal strength based on frequency, weather, and EMP damage.
        /// </summary>
        public void UpdateSignalStrength(float baseSignal, float weatherModifier, float interferenceSusceptibility)
        {
            float oldSignal = SignalStrength;

            // Signal = base * (1 - emp_damage_fraction) * weather_modifier
            // EMP damage reduces signal proportionally
            float empFactor = 1f - (EmpDamage / 100f);

            // Weather interference: weatherModifier is 0..1 (1 = clear, 0 = storm)
            // Interference susceptibility determines how much weather affects this frequency
            float weatherFactor = Mathf.Lerp(1f, weatherModifier, interferenceSusceptibility);

            SignalStrength = Mathf.Clamp01(baseSignal * empFactor * weatherFactor);

            if (Mathf.Abs(SignalStrength - oldSignal) > 0.05f)
            {
                OnSignalStrengthChanged?.Invoke(SignalStrength);
            }
        }

        /// <summary>
        /// Advance tuning progress over elapsed hours. Returns true if tuning completes.
        /// </summary>
        public bool AdvanceTuning(float gameHours, float tuningRate)
        {
            if (gameHours <= 0f || !IsOperational) return false;

            float oldProgress = TuningProgress;
            TuningHoursSpent += gameHours;
            float progressGain = gameHours * tuningRate * SignalStrength;
            TuningProgress = Mathf.Clamp01(TuningProgress + progressGain);

            if (TuningProgress >= 1f && oldProgress < 1f)
            {
                OnTuningComplete?.Invoke(CurrentFrequencyId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Reset tuning to tune a different frequency.
        /// </summary>
        public void ResetTuning(string newFrequencyId)
        {
            CurrentFrequencyId = newFrequencyId;
            TuningProgress = 0f;
            TuningHoursSpent = 0f;
        }

        /// <summary>
        /// Capture state for save/load.
        /// </summary>
        public RadioStateSave CaptureState()
        {
            return new RadioStateSave
            {
                AvailableFuel = AvailableFuel,
                SignalStrength = SignalStrength,
                EmpDamage = EmpDamage,
                CurrentFrequencyId = CurrentFrequencyId,
                TuningProgress = TuningProgress,
                TuningHoursSpent = TuningHoursSpent
            };
        }

        /// <summary>
        /// Restore state from save/load.
        /// </summary>
        public void RestoreState(RadioStateSave save)
        {
            if (save == null) return;
            AvailableFuel = save.AvailableFuel;
            SignalStrength = save.SignalStrength;
            EmpDamage = save.EmpDamage;
            CurrentFrequencyId = save.CurrentFrequencyId;
            TuningProgress = save.TuningProgress;
            TuningHoursSpent = save.TuningHoursSpent;
        }
    }

    /// <summary>
    /// Save/load snapshot of radio state.
    /// </summary>
    [Serializable]
    public class RadioStateSave
    {
        public float AvailableFuel;
        public float SignalStrength;
        public float EmpDamage;
        public string CurrentFrequencyId;
        public float TuningProgress;
        public float TuningHoursSpent;
    }
}
