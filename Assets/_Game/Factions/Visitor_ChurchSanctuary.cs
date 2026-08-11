using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class ChurchSanctuaryState
    {
        public string cardId = "visitor_church_sanctuary";
        public string displayName = "Church Sanctuary";
        public bool isSafeZone = true;
        public bool isFatigueFrozen = true;
        public bool isRestingMidExpedition = false;
        public float fatigueRestoredPerHour = 10f;
    }

    /// <summary>
    /// Prompt #355: Visitor Event: Church - The Sanctuary.
    /// Filled with passive civilians and a Priest. Safe Zone node that freezes Fatigue accumulation
    /// and allows mid-expedition rest without returning to bunker.
    /// </summary>
    /// <summary>DEMOTE-Visitor-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Visitor_ChurchSanctuary
    {
        private ChurchSanctuaryState _state = new ChurchSanctuaryState();

        public event Action<ChurchSanctuaryState, float> OnMidExpeditionRestTicked;

        public ChurchSanctuaryState State => _state;

        public float RestMidExpedition(float restHours)
        {
            _state.isRestingMidExpedition = true;
            float fatigueRecovered = restHours * _state.fatigueRestoredPerHour;

            OnMidExpeditionRestTicked?.Invoke(_state, fatigueRecovered);
            return fatigueRecovered;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public ChurchSanctuaryState CaptureState() => _state;

        public void RestoreState(ChurchSanctuaryState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
