using System;
using System.Collections.Generic;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Medical
{
    [Serializable]
    public class LivestockFeverState
    {
        public string survivorId;
        public int daysSinceInfection;
        public float feverSeverity; // 0..100%
        public bool isBedridden;
    }

    [Serializable]
    public class LivestockFeverSystemSave
    {
        public string systemId = "affliction_livestock_fever";
        public List<LivestockFeverState> afflicted = new List<LivestockFeverState>();
    }

    /// <summary>
    /// Expansion V / Spec §3.2: Zoonotic Livestock Fever (Brucella / Coxiella Strain).
    /// Contracted from unpasteurized livestock milk or handling diseased shelter animals.
    /// Induces cyclic fevers and debility; treated with doxycycline or broad-spectrum antibiotics.
    /// </summary>
    public class LivestockFeverSystem
    {
        private readonly Dictionary<string, LivestockFeverState> _afflicted = new Dictionary<string, LivestockFeverState>();

        public IReadOnlyDictionary<string, LivestockFeverState> Afflicted => _afflicted;

        public event Action<string, string> OnFeverContracted;
        public event Action<string> OnBedriddenTriggered;
        public event Action<string> OnFeverCured;

        public void ContractFever(string survivorId, string animalSource)
        {
            if (_afflicted.ContainsKey(survivorId)) return;

            var state = new LivestockFeverState
            {
                survivorId = survivorId,
                daysSinceInfection = 0,
                feverSeverity = 30f,
                isBedridden = false
            };
            _afflicted[survivorId] = state;
            OnFeverContracted?.Invoke(survivorId, animalSource);
        }

        public void TickDay(string survivorId)
        {
            if (!_afflicted.TryGetValue(survivorId, out var state)) return;

            state.daysSinceInfection++;
            state.feverSeverity = Math.Min(100f, state.feverSeverity + 15f);

            if (state.feverSeverity >= 60f && !state.isBedridden)
            {
                state.isBedridden = true;
                OnBedriddenTriggered?.Invoke(survivorId);
            }
        }

        public bool TryTreatWithAntibiotics(string survivorId, bool hasAntibiotics)
        {
            if (!hasAntibiotics) return false;

            if (!_afflicted.Remove(survivorId)) return false;

            OnFeverCured?.Invoke(survivorId);
            return true;
        }

        public LivestockFeverSystemSave CaptureState() => new LivestockFeverSystemSave
        {
            afflicted = SaveMap.Capture(_afflicted),
        };

        public void RestoreState(LivestockFeverSystemSave saved) =>
            SaveMap.Restore(_afflicted, saved?.afflicted, e => e.survivorId);
    }
}
