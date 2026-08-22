using System;
using System.Collections.Generic;
using Ashfall.Core.StartingLevel;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class VentilationSource
    {
        public string sourceId = string.Empty;
        public float smokeOutputPerDay;
        public float coOutputPerDay;
        public bool requiresExhaust;
    }

    [Serializable]
    public sealed class VentilationState
    {
        public List<VentilationSource> sources = new List<VentilationSource>();
        public float airflowEfficiency = 1f;
    }

    /// <summary>
    /// Engine-agnostic ventilation registry. It provides a bounded environmental
    /// multiplier and records contamination/exhaust sources without host physics.
    /// </summary>
    public sealed class VentilationSystem
    {
        private VentilationState _state = new VentilationState();
        private readonly StartingLevelSystem _startingLevel;

        public VentilationState State => _state;
        public event Action OnVentilationChanged;

        public VentilationSystem(StartingLevelSystem startingLevel)
        {
            _startingLevel = startingLevel ?? throw new ArgumentNullException(nameof(startingLevel));
        }

        public void RegisterSource(VentilationSource source)
        {
            if (source == null || string.IsNullOrEmpty(source.sourceId)) return;
            int index = _state.sources.FindIndex(s => s.sourceId == source.sourceId);
            var copy = CloneSource(source);
            if (index >= 0) _state.sources[index] = copy;
            else _state.sources.Add(copy);
            OnVentilationChanged?.Invoke();
        }

        public bool RemoveSource(string sourceId)
        {
            int removed = _state.sources.RemoveAll(s => s.sourceId == sourceId);
            if (removed == 0) return false;
            OnVentilationChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Environmental disease/airborne-risk multiplier. 1 means no reduction;
        /// efficient airflow lowers risk, while registered exhaust-requiring sources
        /// partially consume that benefit. Result remains bounded and deterministic.
        /// </summary>
        public float GetDiseaseMultiplier()
        {
            float efficiency = Math.Clamp(_state.airflowEfficiency, 0f, 1f);
            float sourcePenalty = 0f;
            foreach (var source in _state.sources)
            {
                if (!source.requiresExhaust) continue;
                sourcePenalty += Math.Max(0f, source.smokeOutputPerDay) * 0.02f;
                sourcePenalty += Math.Max(0f, source.coOutputPerDay) * 0.02f;
                if (source.smokeOutputPerDay == 0f && source.coOutputPerDay == 0f)
                    sourcePenalty += 0.05f;
            }
            return Math.Clamp(1f - (efficiency * 0.5f) + sourcePenalty, 0.25f, 1.5f);
        }

        public VentilationState CaptureState()
        {
            var clone = new VentilationState { airflowEfficiency = _state.airflowEfficiency };
            foreach (var source in _state.sources) clone.sources.Add(CloneSource(source));
            return clone;
        }

        public void RestoreState(VentilationState saved)
        {
            if (saved == null) return;
            _state = new VentilationState { airflowEfficiency = saved.airflowEfficiency };
            if (saved.sources != null)
                foreach (var source in saved.sources)
                    if (source != null) _state.sources.Add(CloneSource(source));
            OnVentilationChanged?.Invoke();
        }

        private static VentilationSource CloneSource(VentilationSource source) => new VentilationSource
        {
            sourceId = source.sourceId ?? string.Empty,
            smokeOutputPerDay = source.smokeOutputPerDay,
            coOutputPerDay = source.coOutputPerDay,
            requiresExhaust = source.requiresExhaust
        };
    }
}
