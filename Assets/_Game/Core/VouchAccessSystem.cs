using System;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — the gate. A social, not a seasonal,
    /// threshold into the Crossing. No countdown, no calendar bar: someone
    /// stakes their name on you and that is all there is to it.
    /// Spec: docs/expansions/expansion_03_nobodys_charter_plan.md §5.2.
    /// </summary>
    [Serializable]
    public class VouchAccessSystemState
    {
        public string systemId = VouchAccessSystem.SystemId;
        /// <summary>Id of the NPC who vouched. Empty = never vouched.</summary>
        public string vouchedBy = "";
        /// <summary>True once the vouching trust was betrayed (gate re-closes).</summary>
        public bool vouchBurned;
        /// <summary>After the opening arc, the player's own name is sufficient.</summary>
        public bool accessSoftened;
        /// <summary>Mattis's last-resort vouch has been cashed this playthrough.</summary>
        public bool lastResortUsed;
    }

    /// <summary>
    /// Plain C#, event-driven, save/load safe. Model: access is a state, not
    /// a number. A vouch can be burned (gate re-closes), softened (own name
    /// suffices), or re-granted by a new vouching NPC — the pack never
    /// hard-locks the player, there is always one more vouch at real cost.
    /// </summary>
    public class VouchAccessSystem
    {
        public const string SystemId = "vouch_access_system";

        private VouchAccessSystemState _state = new VouchAccessSystemState();

        public event Action<string> OnVouchGranted;   // vouchedBy id
        public event Action OnVouchBurned;
        public event Action OnAccessSoftened;

        public VouchAccessSystemState State => _state;

        /// <summary>True when the viaduct is closed to an un-vouched visitor.</summary>
        public bool RequiresVouch => !_state.accessSoftened
            && (string.IsNullOrEmpty(_state.vouchedBy) || _state.vouchBurned);

        /// <summary>True when the player may currently pass the gate.</summary>
        public bool HasAccess => !RequiresVouch;

        public string VouchedBy => _state.vouchedBy;
        public bool VouchBurned => _state.vouchBurned;
        public bool AccessSoftened => _state.accessSoftened;
        public bool LastResortUsed => _state.lastResortUsed;

        /// <summary>
        /// Grant access on the word of <paramref name="npcId"/>. A gentle
        /// idempotence: once access is held (or softened) a second vouch is
        /// a no-op — the Crossing does not re-admit someone already inside.
        /// <paramref name="isLastResort"/> flags a vouch that cost real
        /// capital (Mattis). No-op after the access has softened.
        /// </summary>
        public bool GrantVouch(string npcId, bool isLastResort = false)
        {
            if (string.IsNullOrEmpty(npcId)) return false;
            if (_state.accessSoftened) return false;
            // Already cleanly vouched: no change; idempotent.
            if (!_state.vouchBurned && !string.IsNullOrEmpty(_state.vouchedBy)) return false;

            _state.vouchedBy = npcId;
            _state.vouchBurned = false;
            if (isLastResort) _state.lastResortUsed = true;
            OnVouchGranted?.Invoke(npcId);
            return true;
        }

        /// <summary>
        /// The vouching trust was betrayed. The gate re-closes until a
        /// *new* vouch is found; the burned NPC's relationship is damaged
        /// and the player's own name is worth less until softened again.
        /// </summary>
        public bool BurnVouch()
        {
            // Softened access cannot be burned down to a closed gate: the
            // own-name standing is informal and stays soft.
            if (_state.accessSoftened) return false;
            // Nothing to burn if the gate was never opened through a vouch.
            if (string.IsNullOrEmpty(_state.vouchedBy) && !_state.vouchBurned) return false;

            _state.vouchedBy = "";
            _state.vouchBurned = true;
            OnVouchBurned?.Invoke();
            return true;
        }

        /// <summary>
        /// After the opening arc: the player's own name is sufficient.
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
            return true;
        }

        // ── The bible's "last resort" contract ────────────────────────────
        /// <summary>
        /// The paid-for last resort is only offered after a first vouch was
        /// burned. A fresh, never-attempted gate has no "resort" — the first
        /// vouch is still available, so Mattis is not the first resort.
        /// </summary>
        public bool NeedsLastResort => RequiresVouch && _state.vouchBurned && !_state.lastResortUsed;

        public VouchAccessSystemState CaptureState() => _state;

        public void RestoreState(VouchAccessSystemState saved)
        {
            if (saved == null) return;
            _state = saved;
        }
    }
}