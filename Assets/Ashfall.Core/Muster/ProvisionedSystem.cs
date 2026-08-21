using System;
using System.Collections.Generic;

namespace Ashfall.Core.Muster
{
    /// <summary>Serialized state of the Provisioned (Section V.3) — the Grid's
    /// pre-war stock-holders behind Quenna Brix at loc_second_winter_homestead.</summary>
    public class ProvisionedState
    {
        public string systemId = ProvisionedSystem.SystemId;
        public bool isActive;
        public int respectScore;
        public bool haveMadeContact;
        public List<string> unlockedTradeIds = new List<string>();

        public const int ContactThreshold = 12;
    }

    /// <summary>
    /// Engine-agnostic state machine for faction_the_provisioned (Section V.3).
    /// respectScore rises ONLY via RecordUnprompted() from other systems — the
    /// player cannot buy their way in (Section V.3, "the test nobody announces").
    /// There is deliberately no Approach fork: the whole questline is the single,
    /// patient gesture of earning it rather than buying it.
    /// </summary>
    public class ProvisionedSystem
    {
        public const string SystemId = "provisioned_system";

        private readonly ProvisionedState _state;

        public event Action<ProvisionedState> OnStateChanged;
        public event Action OnContactMade;

        public ProvisionedSystem(ProvisionedState state = null!)
        {
            _state = state ?? new ProvisionedState();
            if (_state.systemId != SystemId) _state.systemId = SystemId;
            if (_state.unlockedTradeIds == null) _state.unlockedTradeIds = new List<string>();
        }

        public ProvisionedState State => _state;
        public int RespectScore => _state.respectScore;
        public bool HaveMadeContact => _state.haveMadeContact;

        /// <summary>The only path that raises respect. Fired BY OTHER systems when
        /// the player helps a third party with no Provisioned benefit — Grain
        /// Exchange famine relief, a free Long Walk escort, a Gun claim honored.</summary>
        public void RecordUnprompted(int amount)
        {
            if (amount <= 0) return;
            _state.respectScore += amount;
            if (!_state.haveMadeContact && _state.respectScore >= ProvisionedState.ContactThreshold)
            {
                _state.haveMadeContact = true;
                OnContactMade?.Invoke();
            }
            RaiseChanged();
        }

        /// <summary>Small, near-worthless trade offer unlocked on contact — the point
        /// was never their inventory.</summary>
        public bool UnlockCache(string tradeId)
        {
            if (!_state.haveMadeContact) return false;
            if (string.IsNullOrEmpty(tradeId)) return false;
            if (_state.unlockedTradeIds.Contains(tradeId)) return false;
            _state.unlockedTradeIds.Add(tradeId);
            RaiseChanged();
            return true;
        }

        public bool HasTrade(string tradeId) => _state.unlockedTradeIds.Contains(tradeId);

        public void Activate() { _state.isActive = true; RaiseChanged(); }

        // ── Save / Load ────────────────────────────────────────────────

        public ProvisionedState CaptureState()
        {
            var copy = new ProvisionedState
            {
                systemId = _state.systemId,
                isActive = _state.isActive,
                respectScore = _state.respectScore,
                haveMadeContact = _state.haveMadeContact
            };
            var sorted = new List<string>(_state.unlockedTradeIds);
            sorted.Sort(StringComparer.Ordinal);
            copy.unlockedTradeIds = sorted;
            return copy;
        }

        public void RestoreState(ProvisionedState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _state.isActive = saved.isActive;
            _state.respectScore = Math.Max(0, saved.respectScore);
            _state.haveMadeContact = saved.haveMadeContact;
            _state.unlockedTradeIds.Clear();
            if (saved.unlockedTradeIds != null)
                _state.unlockedTradeIds.AddRange(saved.unlockedTradeIds);
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
