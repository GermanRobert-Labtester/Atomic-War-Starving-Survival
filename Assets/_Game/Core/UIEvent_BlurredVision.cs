using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BlurredVisionState
    {
        public string eventId = "ui_event_blurred_vision";
        public float toxicityThreshold = 0.7f;
        public float feverThreshold = 38.5f;
        public float blurIntensity = 0.8f;
    }

    /// <summary>
    /// Prompt #752: Blurred Vision.
    /// High Toxicity/Fever: camera DoF blurs aggressively.
    /// Reading small text becomes difficult.
    /// </summary>
    public class UIEvent_BlurredVision
    {
        private BlurredVisionState _state = new BlurredVisionState();
        private bool _isBlurred = false;

        public event Action<float> OnBlurApplied;
        public event Action OnBlurRemoved;

        public BlurredVisionState State => _state;

        public void CheckVision(float toxicity, float bodyTempCelsius)
        {
            float intensity = GetBlurIntensity(toxicity, bodyTempCelsius);

            if (intensity > 0f && !_isBlurred)
            {
                _isBlurred = true;
                OnBlurApplied?.Invoke(intensity);
            }
            else if (intensity > 0f && _isBlurred)
            {
                // Intensity changed — re-fire with updated value
                OnBlurApplied?.Invoke(intensity);
            }
            else if (intensity <= 0f && _isBlurred)
            {
                _isBlurred = false;
                OnBlurRemoved?.Invoke();
            }
        }

        public float GetBlurIntensity(float toxicity, float bodyTempCelsius)
        {
            bool toxicBlur = toxicity >= _state.toxicityThreshold;
            bool feverBlur = bodyTempCelsius >= _state.feverThreshold;

            if (!toxicBlur && !feverBlur)
                return 0f;

            // Combine severity: average the contribution from each source
            float toxContrib = toxicBlur
                ? Mathf.Clamp01((toxicity - _state.toxicityThreshold) / (1f - _state.toxicityThreshold))
                : 0f;

            float feverContrib = feverBlur
                ? Mathf.Clamp01((bodyTempCelsius - _state.feverThreshold) / (41.5f - _state.feverThreshold))
                : 0f;

            float combined = Mathf.Max(toxContrib, feverContrib);
            return Mathf.Lerp(0.3f, _state.blurIntensity, combined);
        }

        public bool IsBlurred() => _isBlurred;
    }
}
