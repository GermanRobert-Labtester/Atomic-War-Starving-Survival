using System;
using UnityEngine;
using AtomicWar._Game.Environment;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Expansion IV — Chapter 45 UI Additions.
    /// OzoneScourgeOverlay: Blown-out pure white surface camera feeds during Weather_FalseSpring / Weather_SilentSpring.
    /// If player stares without item_welders_glass filter, triggers OPTIC NERVE DEGRADATION DETECTED warning.
    /// </summary>
    public class OzoneScourgeOverlay : MonoBehaviour
    {
        private OzoneScourgeSystem _ozoneSystem;
        private float _unshieldedStareTimer;
        private bool _isWarningActive;

        public bool IsWarningActive => _isWarningActive;
        public float UnshieldedStareTimer => _unshieldedStareTimer;

        public void BindOzoneSystem(OzoneScourgeSystem ozoneSystem)
        {
            _ozoneSystem = ozoneSystem;
        }

        public void OnCameraFeedViewed(bool hasWeldersGlassFilter, float deltaTime)
        {
            if (_ozoneSystem == null || !_ozoneSystem.IsOzoneScourgeActive())
            {
                _unshieldedStareTimer = 0f;
                _isWarningActive = false;
                return;
            }

            if (!hasWeldersGlassFilter)
            {
                _unshieldedStareTimer += deltaTime;
                if (_unshieldedStareTimer > 2.0f)
                {
                    _isWarningActive = true;
                }
            }
            else
            {
                _unshieldedStareTimer = 0f;
                _isWarningActive = false;
            }
        }
    }
}
