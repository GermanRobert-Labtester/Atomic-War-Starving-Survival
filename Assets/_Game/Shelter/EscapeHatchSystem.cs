using System;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>Prompt #126 — Secondary escape hatch; evac protocol as alternate endgame.</summary>
    public class EscapeHatchSystem
    {
        public const string EscapeHatchModuleId = "escape_hatch";
        public const float ExcavationHoursRequired = 120f;
        public const float ConcreteRequired = 20f;

        private bool _escapeHatchBuilt;
        private float _excavationProgress;
        private bool _evacTriggered;

        public bool IsBuilt => _escapeHatchBuilt;
        public float ExcavationProgress => _excavationProgress;
        public bool EvacTriggered => _evacTriggered;

        public event Action OnEscapeHatchCompleted;
        public event Action OnEvacuationTriggered;

        public float Excavate(float hours)
        {
            if (_escapeHatchBuilt) return 1f;
            _excavationProgress += hours / ExcavationHoursRequired;
            if (_excavationProgress >= 1f) { _escapeHatchBuilt = true; OnEscapeHatchCompleted?.Invoke(); }
            return Mathf.Clamp01(_excavationProgress);
        }

        public bool TriggerEvacuation()
        {
            if (!_escapeHatchBuilt || _evacTriggered) return false;
            _evacTriggered = true;
            OnEvacuationTriggered?.Invoke();
            return true;
        }

        public EscapeHatchSave CaptureState() => new EscapeHatchSave { EscapeHatchBuilt = _escapeHatchBuilt, ExcavationProgress = _excavationProgress, EvacTriggered = _evacTriggered };
        public void RestoreState(EscapeHatchSave s) { _escapeHatchBuilt = s?.EscapeHatchBuilt ?? false; _excavationProgress = s?.ExcavationProgress ?? 0f; _evacTriggered = s?.EvacTriggered ?? false; }
    }
    [Serializable] public class EscapeHatchSave { public bool EscapeHatchBuilt, EvacTriggered; public float ExcavationProgress; }
}
