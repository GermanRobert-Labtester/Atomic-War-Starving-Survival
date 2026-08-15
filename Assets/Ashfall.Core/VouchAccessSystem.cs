using System;

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — the gate. A social, not a seasonal,
    /// threshold into the Crossing.
    /// Spec: docs/expansions/expansion_04_nobodys_charter_plan.md §5.2.
    /// </summary>
    [Serializable]
    public class VouchAccessSystemState
    {
        public string systemId = VouchAccessSystem.SystemId;
        public string vouchedBy = "";
        public bool vouchBurned;
        public bool accessSoftened;
        public bool lastResortUsed;
    }

    public class VouchAccessSystem
    {
        public const string SystemId = "vouch_access_system";
        public const string FlagExpUnlocked = "exp_nobodys_charter_unlocked";

        private VouchAccessSystemState _state = new VouchAccessSystemState();

        public event Action<string> OnVouchGranted;
        public event Action OnVouchBurned;
        public event Action OnAccessSoftened;
        public event Action<VouchAccessSystemState> OnStateChanged;

        public VouchAccessSystemState State => _state;

        public bool RequiresVouch => !_state.accessSoftened
            && (string.IsNullOrEmpty(_state.vouchedBy) || _state.vouchBurned);

        public bool HasAccess => !RequiresVouch;

        public string VouchedBy => _state.vouchedBy;
        public bool VouchBurned => _state.vouchBurned;
        public bool AccessSoftened => _state.accessSoftened;
        public bool LastResortUsed => _state.lastResortUsed;

        public bool GrantVouch(string npcId, bool isLastResort = false)
        {
            if (string.IsNullOrEmpty(npcId)) return false;
            if (_state.accessSoftened) return false;
            if (!_state.vouchBurned && !string.IsNullOrEmpty(_state.vouchedBy)) return false;

            _state.vouchedBy = npcId;
            _state.vouchBurned = false;
            if (isLastResort) _state.lastResortUsed = true;
            OnVouchGranted?.Invoke(npcId);
            RaiseChanged();
            return true;
        }

        public bool BurnVouch()
        {
            if (_state.accessSoftened) return false;
            if (string.IsNullOrEmpty(_state.vouchedBy) && !_state.vouchBurned) return false;

            _state.vouchedBy = "";
            _state.vouchBurned = true;
            OnVouchBurned?.Invoke();
            RaiseChanged();
            return true;
        }

        /// <summary>
        /// After the opening arc the player's own name becomes sufficient.
        /// Requires a name on the ledger first (a vouch granted or burned) —
        /// you cannot soften a gate that was never opened through a name.
        /// Idempotent; returns false while the gate is untouched.
        /// </summary>
        public bool SoftenAccess()
        {
            if (_state.accessSoftened) return true;
            if (string.IsNullOrEmpty(_state.vouchedBy) && !_state.vouchBurned) return false;
            _state.accessSoftened = true;
            OnAccessSoftened?.Invoke();
            RaiseChanged();
            return true;
        }

        /// <summary>
        /// The paid-for last resort is only offered after a first vouch was
        /// burned. A fresh, never-attempted gate has no "resort" — the first
        /// vouch is still available, so Mattis is not the first resort.
        /// </summary>
        public bool NeedsLastResort => RequiresVouch && _state.vouchBurned && !_state.lastResortUsed;

        public VouchAccessSystemState CaptureState()
        {
            return new VouchAccessSystemState
            {
                systemId = _state.systemId,
                vouchedBy = _state.vouchedBy,
                vouchBurned = _state.vouchBurned,
                accessSoftened = _state.accessSoftened,
                lastResortUsed = _state.lastResortUsed
            };
        }

        public void RestoreState(VouchAccessSystemState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _state.vouchedBy = saved.vouchedBy ?? string.Empty;
            _state.vouchBurned = saved.vouchBurned;
            _state.accessSoftened = saved.accessSoftened;
            _state.lastResortUsed = saved.lastResortUsed;
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
