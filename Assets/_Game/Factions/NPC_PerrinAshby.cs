using System;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_PerrinAshbyState
    {
        public string id = "npc_perrin_ashby";
        public string displayName = "Perrin Ashby";
        public bool isActive;
        /// <summary>Drafts written and presented for signature.</summary>
        public int draftsWritten;
        /// <summary>Signatures collected (honest or otherwise).</summary>
        public int signaturesCollected;
        /// <summary>True once Perrin has passed a clause he knows is unfair (he won't).</summary>
        public bool passedUnfairClause;
    }

    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — Perrin Ashby, Compact drafter.
    /// Earnest, precise, slightly too fond of "finally." Drafts charters,
    /// canvasses support, refuses unfair speed. If he passes a clause he
    /// knows is unfair, the draft stalls and the Annex loses its advocate.
    /// Stationary at the Petition Tent.
    /// </summary>
    public class NPC_PerrinAshby
    {
        private NPC_PerrinAshbyState _state = new NPC_PerrinAshbyState();

        public event Action<NPC_PerrinAshbyState, int> OnDraftWritten;
        public event Action<NPC_PerrinAshbyState, int> OnSignatureCollected;

        public NPC_PerrinAshbyState State => _state;

        public void Initialise(string displayName)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            _state.isActive = true;
        }

        /// <summary>Write a draft. Returns the running count.</summary>
        public int WriteDraft()
        {
            _state.draftsWritten++;
            OnDraftWritten?.Invoke(_state, _state.draftsWritten);
            return _state.draftsWritten;
        }

        /// <summary>Collect a signature on the current draft. Returns the running count.</summary>
        public int CollectSignature()
        {
            _state.signaturesCollected++;
            OnSignatureCollected?.Invoke(_state, _state.signaturesCollected);
            return _state.signaturesCollected;
        }

        /// <summary>
        /// Mark that an unfair clause was passed. This permanently damages
        /// Perrin's credibility — the draft stalls.
        /// </summary>
        public void MarkUnfairClause()
        {
            _state.passedUnfairClause = true;
        }

        public NPC_PerrinAshbyState CaptureState() => _state;
        public void RestoreState(NPC_PerrinAshbyState saved) { _state = saved ?? new NPC_PerrinAshbyState(); }
    }
}
