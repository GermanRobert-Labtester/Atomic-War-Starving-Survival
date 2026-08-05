using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    public enum ProstheticTier
    {
        PegLeg,        // Tier 1: Wood, restores 30% speed
        ScrapLimb,     // Tier 2: Metal, restores 60% speed, heavy
        PreWarBionics  // Tier 3: Electronics + motors, restores 100% speed + 10 Carry Weight
    }

    [Serializable]
    public class ProstheticData
    {
        public ProstheticTier tier;
        public float speedRestorationRatio;
        public float carryWeightBonusKg;
    }

    /// <summary>
    /// Prompt #390: System: Prosthetic Crafting Tiers.
    /// Expands Amputee system (#314) with 3 tiers of prosthetics:
    /// PegLeg (+30% speed), ScrapLimb (+60% speed), PreWarBionics (+100% speed, +10 Carry Weight).
    /// </summary>
    public class ProstheticCraftingSystem
    {
        public event Action<string, ProstheticTier> OnProstheticEquipped;

        public ProstheticData GetProstheticData(ProstheticTier tier)
        {
            switch (tier)
            {
                case ProstheticTier.PegLeg:
                    return new ProstheticData { tier = tier, speedRestorationRatio = 0.30f, carryWeightBonusKg = 0f };
                case ProstheticTier.ScrapLimb:
                    return new ProstheticData { tier = tier, speedRestorationRatio = 0.60f, carryWeightBonusKg = 0f };
                case ProstheticTier.PreWarBionics:
                    return new ProstheticData { tier = tier, speedRestorationRatio = 1.00f, carryWeightBonusKg = 10f };
            }
            return new ProstheticData { tier = tier, speedRestorationRatio = 0.30f, carryWeightBonusKg = 0f };
        }

        public void EquipProsthetic(string survivorId, ProstheticTier tier)
        {
            OnProstheticEquipped?.Invoke(survivorId, tier);
        }
    }
}
