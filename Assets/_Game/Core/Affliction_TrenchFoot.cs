using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class TrenchFootState
    {
        public string survivorId;
        public int daysSinceOnset;
        public bool agilityReduced;
        public bool progressedToGangrene;
        public int gangreneThresholdDays = 5;
    }

    
    [Serializable]
    public class TrenchFootSystemSave
    {
        public string systemId = "affliction_trench_foot";
    }
public class TrenchFootSystem
    {
        private readonly Dictionary<string, TrenchFootState> _afflicted = new Dictionary<string, TrenchFootState>();

        public IReadOnlyDictionary<string, TrenchFootState> Afflicted => _afflicted;

        public event Action<string, string> OnTrenchFootContracted;    // survivorId, cause
        public event Action<string> OnGangreneProgressed;              // survivorId
        public event Action<string> OnTrenchFootCured;                 // survivorId

        public void ContractTrenchFoot(string survivorId, string cause)
        {
            if (_afflicted.ContainsKey(survivorId))
                return;

            var state = new TrenchFootState
            {
                survivorId = survivorId,
                daysSinceOnset = 0,
                agilityReduced = true,
                progressedToGangrene = false,
                gangreneThresholdDays = 5
            };
            _afflicted[survivorId] = state;
            OnTrenchFootContracted?.Invoke(survivorId, cause);
        }

        public void TickDay(string survivorId)
        {
            if (!_afflicted.TryGetValue(survivorId, out var state))
                return;

            state.daysSinceOnset++;

            if (state.daysSinceOnset >= state.gangreneThresholdDays && !state.progressedToGangrene)
            {
                state.progressedToGangrene = true;
                OnGangreneProgressed?.Invoke(survivorId);
            }
        }

        public float GetSpeedMultiplier(string survivorId)
        {
            return _afflicted.ContainsKey(survivorId) ? 0.5f : 1.0f;
        }

        public bool TryCure(string survivorId, bool hasCleanBandages, bool canRest)
        {
            if (!hasCleanBandages || !canRest)
                return false;

            if (!_afflicted.Remove(survivorId))
                return false;

            OnTrenchFootCured?.Invoke(survivorId);
            return true;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public TrenchFootSystemSave CaptureState() => new TrenchFootSystemSave();

        public void RestoreState(TrenchFootSystemSave saved) { _ = saved; }

}
}
