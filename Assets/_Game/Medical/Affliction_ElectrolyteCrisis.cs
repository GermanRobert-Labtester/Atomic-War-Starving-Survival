using System;
using System.Collections.Generic;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Medical
{
    [Serializable]
    public class ElectrolyteCrisisState
    {
        public string survivorId;
        public int daysSinceDepletion;
        public float sodiumDeficit; // 0..100%
        public bool muscleSpasmsActive;
    }

    [Serializable]
    public class ElectrolyteCrisisSystemSave
    {
        public string systemId = "affliction_electrolyte_crisis";
        public List<ElectrolyteCrisisState> afflicted = new List<ElectrolyteCrisisState>();
    }

    /// <summary>
    /// Expansion V / Spec §3.2: Electrolyte Crisis (Hyponatremia / Heat Exhaustion).
    /// Results from intense exertion and sweating without adequate mineral salt intake.
    /// Causes severe cramps and disorientation; cured rapidly by oral rehydration salts or saline broth.
    /// </summary>
    public class ElectrolyteCrisisSystem
    {
        private readonly Dictionary<string, ElectrolyteCrisisState> _afflicted = new Dictionary<string, ElectrolyteCrisisState>();

        public IReadOnlyDictionary<string, ElectrolyteCrisisState> Afflicted => _afflicted;

        public event Action<string, string> OnElectrolyteCrisisContracted;
        public event Action<string> OnMuscleSpasmsTriggered;
        public event Action<string> OnElectrolyteCrisisCured;

        public void ContractCrisis(string survivorId, string cause)
        {
            if (_afflicted.ContainsKey(survivorId)) return;

            var state = new ElectrolyteCrisisState
            {
                survivorId = survivorId,
                daysSinceDepletion = 0,
                sodiumDeficit = 50f,
                muscleSpasmsActive = true
            };
            _afflicted[survivorId] = state;
            OnElectrolyteCrisisContracted?.Invoke(survivorId, cause);
            OnMuscleSpasmsTriggered?.Invoke(survivorId);
        }

        public void TickDay(string survivorId)
        {
            if (!_afflicted.TryGetValue(survivorId, out var state)) return;

            state.daysSinceDepletion++;
            state.sodiumDeficit = Math.Min(100f, state.sodiumDeficit + 20f);
        }

        public bool TryAdministerSalts(string survivorId, bool hasMineralSaltsOrBroth)
        {
            if (!hasMineralSaltsOrBroth) return false;

            if (!_afflicted.Remove(survivorId)) return false;

            OnElectrolyteCrisisCured?.Invoke(survivorId);
            return true;
        }

        public ElectrolyteCrisisSystemSave CaptureState() => new ElectrolyteCrisisSystemSave
        {
            afflicted = SaveMap.Capture(_afflicted),
        };

        public void RestoreState(ElectrolyteCrisisSystemSave saved) =>
            SaveMap.Restore(_afflicted, saved?.afflicted, e => e.survivorId);
    }
}
