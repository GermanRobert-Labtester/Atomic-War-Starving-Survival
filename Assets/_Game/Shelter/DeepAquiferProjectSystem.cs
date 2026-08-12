using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Deep Aquifer Project System (#75) — a massive endgame engineering
    /// goal requiring heavy resource investment to drill down to a clean
    /// subterranean water aquifer, securing permanent water purity.
    ///
    /// Plain C#, save-safe. Ties into WaterStorage + WaterEconomySystem.
    /// </summary>
    public class DeepAquiferProjectSystem
    {
        public const float TotalDrillHoursRequired = 200f;
        public const int SteelBeamsRequired = 10;
        public const int ConcreteRequired = 15;
        public const int PipeSegmentsRequired = 20;
        public const float PermanentWaterPerDay = 50f;
        public const float PurityLevel = 100f;

        public event Action<float> OnDrillProgressChanged;
        // progress 0..1
        public event Action OnAquiferReached;
        public event Action OnPermanentWaterActive;

        private float _drillHoursCompleted;
        private bool _isActive;
        private bool _isComplete;

        public float DrillProgress => TotalDrillHoursRequired > 0f
            ? _drillHoursCompleted / TotalDrillHoursRequired : 0f;
        public bool IsComplete => _isComplete;
        public bool IsActive => _isActive;

        public bool CanStartProject(int steelBeams, int concrete, int pipeSegments)
        {
            return !_isActive && !_isComplete &&
                steelBeams >= SteelBeamsRequired &&
                concrete >= ConcreteRequired &&
                pipeSegments >= PipeSegmentsRequired;
        }

        public bool StartProject()
        {
            if (_isActive || _isComplete) return false;
            _isActive = true;
            return true;
        }

        public float ContributeDrillHours(float hours)
        {
            if (!_isActive || _isComplete) return 0f;

            float remaining = TotalDrillHoursRequired - _drillHoursCompleted;
            float contributed = Math.Min(remaining, hours);
            _drillHoursCompleted += contributed;

            OnDrillProgressChanged?.Invoke(DrillProgress);

            if (_drillHoursCompleted >= TotalDrillHoursRequired)
            {
                _isComplete = true;
                _isActive = false;
                OnAquiferReached?.Invoke();
                OnPermanentWaterActive?.Invoke();
            }

            return contributed;
        }

        public float GetDailyWaterOutput()
        {
            return _isComplete ? PermanentWaterPerDay : 0f;
        }
    }
}
