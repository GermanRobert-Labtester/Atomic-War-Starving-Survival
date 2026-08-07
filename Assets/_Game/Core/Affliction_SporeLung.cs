using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SporeLungState
    {
        public string survivorId;
        public int daysSinceInfection;
        public bool isContagious;
        public bool coughingUpFungi;
    }

    
    [Serializable]
    public class SporeLungSystemSave
    {
        public string systemId = "affliction_spore_lung";
    }
public class SporeLungSystem
    {
        private readonly Dictionary<string, SporeLungState> _infected = new Dictionary<string, SporeLungState>();

        public IReadOnlyDictionary<string, SporeLungState> Infected => _infected;

        public event Action<string, string> OnSporeLungContracted;   // survivorId, source
        public event Action<string, string> OnSporeLungSpread;       // survivorId, fromSurvivorId
        public event Action<string> OnSporeLungCured;                // survivorId

        public void ContractSporeLung(string survivorId, string source)
        {
            if (_infected.ContainsKey(survivorId))
                return;

            var state = new SporeLungState
            {
                survivorId = survivorId,
                daysSinceInfection = 0,
                isContagious = false,
                coughingUpFungi = true
            };
            _infected[survivorId] = state;
            OnSporeLungContracted?.Invoke(survivorId, source);
        }

        public void TickDay(string survivorId, string currentRoomId, Func<IReadOnlyList<string>> getRoomOccupantIds)
        {
            if (!_infected.TryGetValue(survivorId, out var state))
                return;

            state.daysSinceInfection++;

            if (state.daysSinceInfection >= 2 && !state.isContagious)
            {
                state.isContagious = true;
            }

            if (state.isContagious && currentRoomId != null)
            {
                var occupants = getRoomOccupantIds();
                if (occupants != null)
                {
                    for (int i = 0; i < occupants.Count; i++)
                    {
                        var occupantId = occupants[i];
                        if (occupantId != survivorId && !_infected.ContainsKey(occupantId))
                        {
                            ContractSporeLung(occupantId, survivorId);
                            OnSporeLungSpread?.Invoke(occupantId, survivorId);
                        }
                    }
                }
            }
        }

        public bool TryCure(string survivorId, bool hasFungicides)
        {
            if (!hasFungicides)
                return false;

            if (!_infected.Remove(survivorId))
                return false;

            OnSporeLungCured?.Invoke(survivorId);
            return true;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public SporeLungSystemSave CaptureState() => new SporeLungSystemSave();

        public void RestoreState(SporeLungSystemSave saved) { _ = saved; }

}
}
