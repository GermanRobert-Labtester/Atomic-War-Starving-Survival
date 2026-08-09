using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class RebelTrainingYardState
    {
        public string cardId = "visitor_rebel_training_yard";
        public string displayName = "Rebel Training Yard";
        public int trapDensity = 8;
        public int barricadeRating = 85;
        public float rebelTrustGainOnFlank = 50f;
    }

    /// <summary>
    /// Prompt #332: Location Visitor: Rebel Training Yard.
    /// High concentration of traps and barricades. Loot is mostly ScrapMetal and Explosives.
    /// If Military attacks node (Skirmish), player can flank military for massive Rebel trust (+50).
    /// </summary>
    /// <summary>DEMOTE-Visitor-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Visitor_RebelTrainingYard
    {
        private RebelTrainingYardState _state = new RebelTrainingYardState();

        public event Action<RebelTrainingYardState, float> OnMilitaryFlankedForTrust;

        public RebelTrainingYardState State => _state;

        public float PerformFlankAttackOnMilitary()
        {
            OnMilitaryFlankedForTrust?.Invoke(_state, _state.rebelTrustGainOnFlank);
            return _state.rebelTrustGainOnFlank;
        }

        public List<string> GenerateLoot()
        {
            return new List<string>
            {
                "scrap_metal_stack",
                "scrap_metal_stack",
                "improvised_explosive",
                "gunpowder_sack"
            };
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public RebelTrainingYardState CaptureState() => _state;

        public void RestoreState(RebelTrainingYardState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
