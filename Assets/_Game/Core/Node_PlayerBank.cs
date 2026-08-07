using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class PlayerBankState
    {
        public string nodeId = "";
        public bool isEstablished = false;
        public List<string> storedItemIds = new List<string>();
        public bool isGuarded = false;
        public float banditRaidChance = 0.15f;
    }

    /// <summary>
    /// Prompt #666: Node: Player Bank.
    /// Clear map node → off-site stash. Infinite items. Susceptible to BanditRaids when unguarded.
    /// </summary>
    public class Node_PlayerBank
    {
        private PlayerBankState _state = new PlayerBankState();

        public event Action<PlayerBankState> OnBankEstablished;
        public event Action<PlayerBankState, string> OnItemDeposited;
        public event Action<PlayerBankState, string> OnItemWithdrawn;
        public event Action<PlayerBankState> OnBankRaided;

        public PlayerBankState State => _state;

        public bool Establish(string nodeId)
        {
            if (_state.isEstablished || string.IsNullOrEmpty(nodeId))
                return false;

            _state.nodeId = nodeId;
            _state.isEstablished = true;

            OnBankEstablished?.Invoke(_state);
            return true;
        }

        public bool DepositItem(string itemId)
        {
            if (!_state.isEstablished || string.IsNullOrEmpty(itemId))
                return false;

            _state.storedItemIds.Add(itemId);
            OnItemDeposited?.Invoke(_state, itemId);
            return true;
        }

        public bool WithdrawItem(string itemId)
        {
            if (!_state.isEstablished || string.IsNullOrEmpty(itemId))
                return false;

            if (!_state.storedItemIds.Contains(itemId))
                return false;

            _state.storedItemIds.Remove(itemId);
            OnItemWithdrawn?.Invoke(_state, itemId);
            return true;
        }

        public bool CheckRaid(System.Random rng)
        {
            if (!_state.isEstablished || _state.isGuarded)
                return false;

            bool raided = rng != null && (float)rng.NextDouble() < _state.banditRaidChance;

            if (raided)
            {
                _state.storedItemIds.Clear();
                OnBankRaided?.Invoke(_state);
            }

            return raided;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public PlayerBankState CaptureState() => _state;

        public void RestoreState(PlayerBankState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
