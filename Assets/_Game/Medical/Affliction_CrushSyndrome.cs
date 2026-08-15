using System;
using System.Collections.Generic;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Medical
{
    [Serializable]
    public class CrushSyndromeState
    {
        public string survivorId;
        public int daysSinceInjury;
        public float myoglobinToxicity; // 0..100%
        public bool renalRiskTriggered;
    }

    [Serializable]
    public class CrushSyndromeSystemSave
    {
        public string systemId = "affliction_crush_syndrome";
        public List<CrushSyndromeState> afflicted = new List<CrushSyndromeState>();
    }

    /// <summary>
    /// Expansion V / Spec §3.2: Crush Syndrome (Reperfusion Injury).
    /// Delayed-onset systemic trauma resulting from entrapment under collapsed masonry or heavy rubble.
    /// Untreated myoglobin release threatens acute renal failure unless treated with IV saline and rest.
    /// </summary>
    public class CrushSyndromeSystem
    {
        private readonly Dictionary<string, CrushSyndromeState> _afflicted = new Dictionary<string, CrushSyndromeState>();

        public IReadOnlyDictionary<string, CrushSyndromeState> Afflicted => _afflicted;

        public event Action<string, string> OnCrushSyndromeContracted;
        public event Action<string> OnRenalCrisisTriggered;
        public event Action<string> OnCrushSyndromeCured;

        public void ContractCrushSyndrome(string survivorId, string cause)
        {
            if (_afflicted.ContainsKey(survivorId)) return;

            var state = new CrushSyndromeState
            {
                survivorId = survivorId,
                daysSinceInjury = 0,
                myoglobinToxicity = 40f,
                renalRiskTriggered = false
            };
            _afflicted[survivorId] = state;
            OnCrushSyndromeContracted?.Invoke(survivorId, cause);
        }

        public void TickDay(string survivorId)
        {
            if (!_afflicted.TryGetValue(survivorId, out var state)) return;

            state.daysSinceInjury++;
            state.myoglobinToxicity += 15f;

            if (state.myoglobinToxicity >= 75f && !state.renalRiskTriggered)
            {
                state.renalRiskTriggered = true;
                OnRenalCrisisTriggered?.Invoke(survivorId);
            }
        }

        public bool TryTreat(string survivorId, bool hasSalineOrDialysis)
        {
            if (!hasSalineOrDialysis) return false;

            if (!_afflicted.Remove(survivorId)) return false;

            OnCrushSyndromeCured?.Invoke(survivorId);
            return true;
        }

        public CrushSyndromeSystemSave CaptureState() => new CrushSyndromeSystemSave
        {
            afflicted = SaveMap.Capture(_afflicted),
        };

        public void RestoreState(CrushSyndromeSystemSave saved) =>
            SaveMap.Restore(_afflicted, saved?.afflicted, e => e.survivorId);
    }
}
