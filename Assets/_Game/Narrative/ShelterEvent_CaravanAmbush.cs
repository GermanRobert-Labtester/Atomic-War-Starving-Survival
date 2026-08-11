using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Narrative
{
    [Serializable]
    public class CaravanAmbushState
    {
        public string eventId = "shelter_event_caravan_ambush";
        public string traderFactionId = "wasteland_caravans";
        public int ammoNeededToDefend = 20;
        public float discountGainedRatio = 0.50f; // 50% discount
        public bool isFactionBlacklisted = false;
    }

    /// <summary>
    /// Prompt #410: Event: Caravan Ambush (Defense).
    /// A Trader arrives at the hatch pursued by Raiders.
    /// Helping (burning ammo) grants massive Discount and Trust. Locking the door results in the trader's death,
    /// allows corpse looting, but blacklists the player from that faction forever.
    /// </summary>
    public class ShelterEvent_CaravanAmbush
    {
        private CaravanAmbushState _state = new CaravanAmbushState();

        public event Action<CaravanAmbushState, float> OnCaravanRescuedWithDiscount;
        public event Action<CaravanAmbushState> OnCaravanAbandonedBlacklisted;

        public CaravanAmbushState State => _state;

        public bool ProvideCoveringFire(ref int totalAmmunition, out float discountRatio)
        {
            discountRatio = 0f;
            if (totalAmmunition >= _state.ammoNeededToDefend)
            {
                totalAmmunition -= _state.ammoNeededToDefend;
                discountRatio = _state.discountGainedRatio;

                OnCaravanRescuedWithDiscount?.Invoke(_state, discountRatio);
                return true;
            }
            return false;
        }

        public List<string> LockDoorAndAbandonTrader()
        {
            _state.isFactionBlacklisted = true;
            OnCaravanAbandonedBlacklisted?.Invoke(_state);
            return new List<string> { "trader_corpse_scrap", "rare_prewar_goods" };
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public CaravanAmbushState CaptureState() => _state;

        public void RestoreState(CaravanAmbushState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
