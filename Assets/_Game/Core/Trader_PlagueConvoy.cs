using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class PlagueConvoyState
    {
        public string traderId = "trader_plague_convoy";
        public string displayName = "The Plague Convoy";
        public bool isCoughingVisible = true;
        public float discountPercentage = 0.80f; // 80% discount
        public string introducedPlagueAffliction = "phase_1_plague";
    }

    /// <summary>
    /// Prompt #417: System: The Plague Convoy.
    /// Caravan offering high-tier pre-war loot at massive discounts, but the traders are coughing.
    /// Trading with them automatically introduces a highly contagious Phase 1 Plague into the airlock.
    /// </summary>
    public class Trader_PlagueConvoy
    {
        private PlagueConvoyState _state = new PlagueConvoyState();

        public event Action<PlagueConvoyState, string> OnPlagueIntroducedToAirlock;

        public PlagueConvoyState State => _state;

        public List<string> ExecuteTrade(int playerScrap, out string plagueAffliction)
        {
            plagueAffliction = _state.introducedPlagueAffliction;
            OnPlagueIntroducedToAirlock?.Invoke(_state, plagueAffliction);

            return new List<string> { "prewar_trauma_kit", "military_grade_batteries", "canned_beef" };
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public PlagueConvoyState CaptureState() => _state;

        public void RestoreState(PlagueConvoyState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
