using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class WarlordSuccessionState
    {
        public string eventId = "event_warlord_succession";
        public string originalFactionId = "";
        public string subFaction1Id = "";
        public string subFaction2Id = "";
        public bool isFractured = false;
        public bool areAtWar = false;
    }

    /// <summary>
    /// Prompt #660: Event: Warlord Succession.
    /// Assassinate Faction Leader → Faction fractures into 2 weaker warring sub-factions.
    /// Play them against each other.
    /// </summary>
    public class Event_WarlordSuccession
    {
        private WarlordSuccessionState _state = new WarlordSuccessionState();

        public event Action<WarlordSuccessionState, string> OnLeaderAssassinated;
        public event Action<WarlordSuccessionState> OnFactionFractured;
        public event Action<WarlordSuccessionState> OnSubFactionsAtWar;
        public event Action<WarlordSuccessionState, string> OnFactionsPlayedOff;

        public WarlordSuccessionState State => _state;

        public bool AssassinateLeader(string factionId, System.Random rng)
        {
            if (_state.isFractured || string.IsNullOrEmpty(factionId))
                return false;

            _state.originalFactionId = factionId;
            _state.subFaction1Id = $"{factionId}_splinter_a";
            _state.subFaction2Id = $"{factionId}_splinter_b";
            _state.isFractured = true;
            _state.areAtWar = true;

            OnLeaderAssassinated?.Invoke(_state, factionId);
            OnFactionFractured?.Invoke(_state);
            OnSubFactionsAtWar?.Invoke(_state);

            return true;
        }

        public bool PlayFactionsOffEachOther(string targetSubFaction)
        {
            if (!_state.isFractured || !_state.areAtWar)
                return false;

            if (targetSubFaction != _state.subFaction1Id && targetSubFaction != _state.subFaction2Id)
                return false;

            OnFactionsPlayedOff?.Invoke(_state, targetSubFaction);
            return true;
        }
    }
}
