using System;
using UnityEngine;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// weather_ash_lightning — Ash Lightning (Section X).
    /// Static discharge in the ash cloud. Surface expeditions impossible.
    /// Risk of fire if the shelter has unshielded electronics. The sky
    /// flickers orange. The survivors watch the hatch and don't speak.
    /// </summary>
    [Serializable]
    public class AshLightningState
    {
        public string weatherId = "weather_ash_lightning";
        public string displayName = "Ash Lightning";
        public bool isActive = false;
        public float surfaceExpeditionBlockFactor = 0f;   // 0 = no expeditions
        public float unshieldedElectronicsFireChancePerHour = 0.05f;
        public int durationHours = 6;
    }

    /// <summary>DEMOTE-Weather-batch — dormant ghost. Re-promote with Boot+Save+host.</summary>
    public class Weather_AshLightning
    {
        private AshLightningState _state = new AshLightningState();

        public event Action<AshLightningState, float> OnStaticDischarge;     // (state, fireChanceRolled)
        public event Action<AshLightningState> OnFlickerOrange;

        public AshLightningState State => _state;

        /// <summary>Returns true if a fire was rolled this tick.</summary>
        public bool Tick(float deltaHours, bool isVentilationActive, bool hasUnshieldedElectronics, System.Random rng)
        {
            if (!_state.isActive) return false;
            if (hasUnshieldedElectronics && rng != null)
            {
                if (rng.NextDouble() < _state.unshieldedElectronicsFireChancePerHour * deltaHours)
                {
                    OnStaticDischarge?.Invoke(_state, 1f);
                    return true;
                }
            }
            OnFlickerOrange?.Invoke(_state);
            return false;
        }

        public void SetActive(bool active) { _state.isActive = active; }

        /// <summary>Convenience: fire the event for its full configured duration (matches Weather_BloodRain.Trigger() convention).</summary>
        public void Trigger() => SetActive(true);

        public bool BlocksSurfaceExpeditions => _state.isActive;

        public AshLightningState CaptureState() => _state;
        public void RestoreState(AshLightningState s) { _state = s ?? new AshLightningState(); }
    }
}
