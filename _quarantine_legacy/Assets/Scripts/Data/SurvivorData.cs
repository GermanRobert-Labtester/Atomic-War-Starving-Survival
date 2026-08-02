using UnityEngine;

namespace AtomicWar.Data
{
    [CreateAssetMenu(fileName = "NewSurvivor", menuName = "AtomicWar/Data/SurvivorData")]
    public class SurvivorData : ScriptableObject
    {
        [Header("Identity")]
        public string Id;
        public string CharacterName;
        public string Profession;
        [TextArea] public string Backstory;
        public Sprite Portrait;

        [Header("Base Stats")]
        public float MaxHealth = 100f;
        public float BaseHungerDecayRate = 2f; // per hour
        public float BaseFatigueDecayRate = 1.5f;
        public float CarryCapacityWeight = 25f;

        [Header("Traits & Perks")]
        public float MoralSensitivity = 1.0f; // Multiplier on morale loss
        public float CraftingSpeedMultiplier = 1.0f;
        public float CombatEfficiency = 1.0f;
    }
}
