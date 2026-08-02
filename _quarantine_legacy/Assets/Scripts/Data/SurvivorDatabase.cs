using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar.Data
{
    [Serializable]
    public class DetailedSurvivorProfile
    {
        public string Id;
        public string CharacterName;
        public int Age;
        public string PreWarOccupation;
        public string PersonalityTrait;
        public string UsefulSkill;
        public string Weakness;
        public string SecretFear;
        public string MoralLine;
        [TextArea(3, 6)] public string Biography;

        // Base Gameplay Stats
        public float MaxHealth = 100f;
        public float BaseHungerDecayRate = 2.0f;
        public float BaseFatigueDecayRate = 1.5f;
        public float MoralSensitivity = 1.0f;
        public float CraftingSpeedMultiplier = 1.0f;
        public float CombatEfficiency = 1.0f;
    }

    [CreateAssetMenu(fileName = "SurvivorDatabase", menuName = "AtomicWar/Data/SurvivorDatabase")]
    public class SurvivorDatabase : ScriptableObject
    {
        public List<DetailedSurvivorProfile> Survivors = new List<DetailedSurvivorProfile>();

        public DetailedSurvivorProfile GetById(string id)
        {
            return Survivors.Find(s => s.Id == id);
        }
    }
}
