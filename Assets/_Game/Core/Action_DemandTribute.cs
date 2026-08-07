using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class DemandTributeState
    {
        public string actionId = "action_demand_tribute";
        public float securityThreshold = 80f;
        public float armoryThreshold = 50f;
        public int tributeFoodPerDay = 10;
        public int tributeWaterPerDay = 5;
        public bool hasVassals = false;
    }

    /// <summary>
    /// Prompt #662: Action: Demand Tribute.
    /// If ShelterSecurity+Armory out-scale local Factions → demand Food/Water from them.
    /// They become vassals. You are the Warlord.
    /// </summary>
    public class Action_DemandTribute
    {
        private DemandTributeState _state = new DemandTributeState();

        public event Action<DemandTributeState> OnTributeDemanded;
        public event Action<DemandTributeState> OnVassalsAcquired;
        public event Action<DemandTributeState, int, int> OnTributeCollected;

        public DemandTributeState State => _state;

        public bool DemandTribute(float shelterSecurity, float armoryPower, float factionPower)
        {
            if (_state.hasVassals)
                return false;

            float playerPower = shelterSecurity + armoryPower;

            if (playerPower < _state.securityThreshold + _state.armoryThreshold)
                return false;

            if (playerPower <= factionPower)
                return false;

            _state.hasVassals = true;
            OnTributeDemanded?.Invoke(_state);
            OnVassalsAcquired?.Invoke(_state);
            return true;
        }

        public (int food, int water) CollectTribute()
        {
            if (!_state.hasVassals)
                return (0, 0);

            int food = _state.tributeFoodPerDay;
            int water = _state.tributeWaterPerDay;

            OnTributeCollected?.Invoke(_state, food, water);
            return (food, water);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public DemandTributeState CaptureState() => _state;

        public void RestoreState(DemandTributeState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
