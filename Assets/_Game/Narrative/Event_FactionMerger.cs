using System;

namespace AtomicWar._Game.Narrative
{
    [Serializable]
    public class FactionMergerState
    {
        public string eventId = "event_faction_merger";
        public string faction1Id = "";
        public string faction2Id = "";
        public string superFactionId = "";
        public bool isMerged = false;
        public float tributeMultiplier = 3f;
        public float lootMultiplier = 2f;
    }

    /// <summary>
    /// Prompt #664: Event: Faction Merger.
    /// Two allied Warlords merge → Super-Faction. Take over map. Economy booms.
    /// Tribute demands triple.
    /// </summary>
    public class Event_FactionMerger
    {
        private FactionMergerState _state = new FactionMergerState();

        public event Action<FactionMergerState, string, string> OnMergerTriggered;
        public event Action<FactionMergerState> OnSuperFactionFormed;

        public FactionMergerState State => _state;

        public bool TriggerMerger(string f1, string f2)
        {
            if (_state.isMerged)
                return false;

            if (string.IsNullOrEmpty(f1) || string.IsNullOrEmpty(f2))
                return false;

            _state.faction1Id = f1;
            _state.faction2Id = f2;
            _state.superFactionId = $"{f1}_{f2}_superfaction";
            _state.isMerged = true;

            OnMergerTriggered?.Invoke(_state, f1, f2);
            OnSuperFactionFormed?.Invoke(_state);
            return true;
        }

        public float GetTributeDemand(float baseTribute)
        {
            if (!_state.isMerged)
                return baseTribute;

            return baseTribute * _state.tributeMultiplier;
        }

        public float GetLootBonus()
        {
            if (!_state.isMerged)
                return 1f;

            return _state.lootMultiplier;
        }

        public FactionMergerState CaptureState()
        {
            return _state;
        }

        public void RestoreState(FactionMergerState saved)
        {
            _state = saved ?? new FactionMergerState();
        }
    }
}
