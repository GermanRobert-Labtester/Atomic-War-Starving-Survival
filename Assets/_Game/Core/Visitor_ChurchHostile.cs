using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ChurchHostileState
    {
        public string cardId = "visitor_church_hostile";
        public string displayName = "Church Hostile Takeover";
        public bool isBellTowerSniperActive = true;
        public bool isGraveyardApproachRequired = true;
        public int noiseTrapCount = 5;
        public bool isDetectedBySniper = false;
    }

    /// <summary>
    /// Prompt #356: Visitor Event: Church - Hostile Takeover.
    /// Bandits have seized the church, operating a sniper nest from the bell tower.
    /// Player must approach through the graveyard (provides high cover, but contains noise traps).
    /// </summary>
    public class Visitor_ChurchHostile
    {
        private ChurchHostileState _state = new ChurchHostileState();

        public event Action<ChurchHostileState, bool> OnGraveyardApproachAttempted;
        public event Action<ChurchHostileState> OnSniperAlerted;

        public ChurchHostileState State => _state;

        public bool AttemptGraveyardApproach(int playerStealthSkill, System.Random rng)
        {
            // Stealth check against graveyard noise traps
            bool success = playerStealthSkill >= 10 && rng.NextDouble() > 0.40;
            OnGraveyardApproachAttempted?.Invoke(_state, success);

            if (!success)
            {
                _state.isDetectedBySniper = true;
                OnSniperAlerted?.Invoke(_state);
            }

            return success;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public ChurchHostileState CaptureState() => _state;

        public void RestoreState(ChurchHostileState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
