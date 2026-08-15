using System;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_GrainExchangeState
    {
        public string id = "faction_grain_exchange";
        public string displayName = "The Grain Exchange";
        public bool isActive;
        /// <summary>True after the player takes the seat of setting the board.</summary>
        public bool playerControlsBoard;
        /// <summary>Attendees per season; starts at the four Powers.</summary>
        public int attendees = 4;
        /// <summary>Seasons of decline while the player keeps the board.</summary>
        public int decliningSeasons;
        public bool exchangeQuieted;
        public string badgeAssetId = "faction_badge_free_traders";
        /// <summary>Lore bible interlocks — the Exchange has no enforcement; the Tally
        /// sells exactly that. True once the player arranges Tally enforcement.</summary>
        public bool tallyEnforcementArranged;
        public int tallyContractsArranged;
    }

    /// <summary>
    /// Lore bible 05_FACTIONS §4 — The Grain Exchange (peaceful, fragile Current).
    /// Not believers: a clearing house. No guards, no charter, no enforcement.
    /// It works for exactly one reason: everybody attending is hungry.
    /// If the player takes the board, it works — and then it quietly stops.
    /// </summary>
    public class NPC_GrainExchange
    {
        private NPC_GrainExchangeState _state = new NPC_GrainExchangeState();

        /// <summary>Raised once, roughly ninety days after the board stopped being repainted.</summary>
        public event Action<NPC_GrainExchangeState> OnExchangeQuieted;

        public NPC_GrainExchangeState State => _state;

        public void Initialise(string displayName, string badgeAssetId)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            if (!string.IsNullOrEmpty(badgeAssetId)) _state.badgeAssetId = badgeAssetId;
            _state.isActive = true;
        }

        /// <summary>
        /// The Year Somebody Wasn't Hungry: the player sets the board.
        /// A straightforwardly good economic outcome, presented without irony.
        /// </summary>
        public void PlayerSetsBoard()
        {
            _state.playerControlsBoard = true;
        }

        /// <summary>
        /// Lore bible 05_FACTIONS interlocks — the Exchange has no guards, no
        /// charter, and no enforcement mechanism whatsoever. The Tally sells
        /// exactly that. Arranging it writes a real contract through the Tally
        /// and marks the arrangement on the Exchange's state.
        /// </summary>
        public TallyContract ArrangeTallyEnforcement(
            NPC_Tally tally,
            string debtorId,
            string debt,
            string term,
            string rate,
            string forfeit,
            int dueDay)
        {
            if (tally == null) return null;
            var contract = tally.WriteContract(debtorId, debt, term, rate, forfeit, dueDay);
            _state.tallyEnforcementArranged = true;
            _state.tallyContractsArranged++;
            return contract;
        }

        /// <summary>
        /// Seasonal tick. While the player controls the board the Exchange does
        /// not collapse dramatically — it simply has fewer attendees each
        /// season, and the board stops being repainted.
        /// </summary>
        public void TickSeasonalDecline()
        {
            if (!_state.playerControlsBoard || _state.exchangeQuieted) return;

            _state.decliningSeasons++;
            _state.attendees = Mathf.Max(0, _state.attendees - 1);
            if (_state.attendees <= 0)
            {
                _state.exchangeQuieted = true;
                OnExchangeQuieted?.Invoke(_state);
            }
        }

        public NPC_GrainExchangeState CaptureState() => _state;
        public void RestoreState(NPC_GrainExchangeState saved) { _state = saved ?? new NPC_GrainExchangeState(); }
    }
}
