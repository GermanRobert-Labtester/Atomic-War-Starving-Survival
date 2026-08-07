using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class CallCaravanState
    {
        public string actionId = "action_call_caravan";
        public int powerCostWatts = 10;
        public float arrivalTimeHours = 24f;
        public bool isCaravanEnRoute = false;
        public bool isCaravanKilledEnRoute = false;
    }

    /// <summary>
    /// Prompt #409: System: Calling Caravans.
    /// Calls a Trader using the RadioSystem (costs 10 Power, takes 24 hours to arrive).
    /// Performs route safety checks: if blocked by AshDrifts or hostile Bandits, the caravan is killed en route.
    /// </summary>
    public class Action_CallCaravan
    {
        private CallCaravanState _state = new CallCaravanState();

        public event Action<CallCaravanState> OnCaravanDispatched;
        public event Action<CallCaravanState> OnCaravanKilledInAmbush;
        public event Action<CallCaravanState> OnCaravanArrivedSafely;

        public CallCaravanState State => _state;

        public bool RequestCaravan(ref float shelterPowerStorage, bool isRouteBlocked, bool isBanditHostileOnRoute)
        {
            if (shelterPowerStorage < _state.powerCostWatts || _state.isCaravanEnRoute)
                return false;

            shelterPowerStorage -= _state.powerCostWatts;
            _state.isCaravanEnRoute = true;
            _state.isCaravanKilledEnRoute = false;

            OnCaravanDispatched?.Invoke(_state);

            if (isRouteBlocked || isBanditHostileOnRoute)
            {
                _state.isCaravanKilledEnRoute = true;
                _state.isCaravanEnRoute = false;
                OnCaravanKilledInAmbush?.Invoke(_state);
                return false;
            }

            return true;
        }

        public void CompleteArrival()
        {
            if (_state.isCaravanEnRoute && !_state.isCaravanKilledEnRoute)
            {
                _state.isCaravanEnRoute = false;
                OnCaravanArrivedSafely?.Invoke(_state);
            }
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public CallCaravanState CaptureState() => _state;

        public void RestoreState(CallCaravanState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
