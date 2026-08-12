using System;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Expansion IV — Chapter 45 UI Additions.
    /// MemoryFlashVignette: Flash 0.2-second monochrome image of pre-war shadow burned into wall + high-pitched tinnitus ring
    /// when a survivor with trait_hyperthymesia experiences a stressor (low health, radiation spike).
    /// </summary>
    public class MemoryFlashVignette : MonoBehaviour
    {
        public const float FlashDurationSeconds = 0.2f;

        private float _flashTimer;
        private bool _isFlashing;

        public bool IsFlashing => _isFlashing;

        public event Action OnTinnitusTriggered;

        public void TriggerMemoryFlash(Survivor survivor)
        {
            if (survivor == null || !survivor.HasTrait("trait_hyperthymesia")) return;

            _isFlashing = true;
            _flashTimer = FlashDurationSeconds;
            OnTinnitusTriggered?.Invoke();
        }

        private void Update()
        {
            if (_isFlashing)
            {
                _flashTimer -= Time.deltaTime;
                if (_flashTimer <= 0f)
                {
                    _isFlashing = false;
                }
            }
        }
    }
}
