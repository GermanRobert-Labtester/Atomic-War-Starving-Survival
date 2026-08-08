using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class EstablishRouteState
    {
        public string actionId = "action_establish_route";
        public bool routeEstablished = false;
        public List<string> patrolSurvivorIds = new List<string>();
        public int ammoPerDay = 5;
        public int moneyPerDay = 10;
    }

    /// <summary>
    /// Prompt #663: Action: Establish Route.
    /// Assign armed survivors to Patrol between Allied Factions. Passive Ammo+Money income.
    /// Removes survivors from bunker indefinitely.
    /// </summary>
    /// <summary>DEMOTE-Action-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_EstablishRoute
    {
        private EstablishRouteState _state = new EstablishRouteState();

        public event Action<EstablishRouteState, List<string>> OnRouteEstablished;
        public event Action<EstablishRouteState, int, int> OnIncomeCollected;

        public EstablishRouteState State => _state;

        public bool Establish(List<string> survivorIds, string factionA, string factionB)
        {
            if (_state.routeEstablished)
                return false;

            if (survivorIds == null || survivorIds.Count == 0)
                return false;

            if (string.IsNullOrEmpty(factionA) || string.IsNullOrEmpty(factionB))
                return false;

            _state.patrolSurvivorIds = new List<string>(survivorIds);
            _state.routeEstablished = true;

            OnRouteEstablished?.Invoke(_state, _state.patrolSurvivorIds);
            return true;
        }

        public (int ammo, int money) CollectIncome()
        {
            if (!_state.routeEstablished)
                return (0, 0);

            int ammo = _state.ammoPerDay;
            int money = _state.moneyPerDay;

            OnIncomeCollected?.Invoke(_state, ammo, money);
            return (ammo, money);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public EstablishRouteState CaptureState() => _state;

        public void RestoreState(EstablishRouteState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
