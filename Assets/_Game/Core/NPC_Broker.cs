using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BrokerState
    {
        public string npcId = "npc_broker";
        public int appearsDay = 75;
        public bool isVisible = false;
        public List<string> availableBlueprints = new List<string>();
        public int priceInRaiders = 2;
        public int priceInGold = 50;
    }

    /// <summary>
    /// Prompt #667: NPC: Broker.
    /// Appears on Radio Day 75+. Deals in Mega-Project blueprints.
    /// Demands Captured Raiders or GoldBullion.
    /// </summary>
    public class NPC_Broker
    {
        private BrokerState _state = new BrokerState();

        public event Action<BrokerState> OnBrokerAppeared;
        public event Action<BrokerState, string> OnBlueprintPurchased;
        public event Action<BrokerState, string> OnPurchaseFailed;

        public BrokerState State => _state;

        public bool CheckAppearance(int currentDay)
        {
            if (_state.isVisible)
                return false;

            if (currentDay < _state.appearsDay)
                return false;

            _state.isVisible = true;
            OnBrokerAppeared?.Invoke(_state);
            return true;
        }

        public bool TryBuy(string blueprintId, int raidersOffered, int goldOffered)
        {
            if (!_state.isVisible || string.IsNullOrEmpty(blueprintId))
                return false;

            if (!_state.availableBlueprints.Contains(blueprintId))
            {
                OnPurchaseFailed?.Invoke(_state, blueprintId);
                return false;
            }

            bool canAffordWithRaiders = raidersOffered >= _state.priceInRaiders;
            bool canAffordWithGold = goldOffered >= _state.priceInGold;

            if (!canAffordWithRaiders && !canAffordWithGold)
            {
                OnPurchaseFailed?.Invoke(_state, blueprintId);
                return false;
            }

            _state.availableBlueprints.Remove(blueprintId);
            OnBlueprintPurchased?.Invoke(_state, blueprintId);
            return true;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public BrokerState CaptureState() => _state;

        public void RestoreState(BrokerState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
