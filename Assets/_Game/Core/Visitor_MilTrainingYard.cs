using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class MilTrainingYardState
    {
        public string cardId = "visitor_mil_training_yard";
        public string displayName = "Military Training Yard";
        public int militaryNpcCount = 12;
        public bool hasHighTierLoot = true;
        public bool requiresEndgameWeapons = true;
        public float bunkerRaidMultiplier = 1.0f;
        public int raidMultiplierDurationDays = 0;
    }

    /// <summary>
    /// Prompt #327: Location Visitor: Military Training Yard.
    /// Spawns massive Military NPCs with high-tier loot (Ammunition, BodyArmor).
    /// Flees after attacking -> Military Raids on player's bunker increase by +300% for 7 days.
    /// </summary>
    public class Visitor_MilTrainingYard
    {
        private MilTrainingYardState _state = new MilTrainingYardState();

        public event Action<MilTrainingYardState, float, int> OnBunkerRaidThreatEscalated;

        public MilTrainingYardState State => _state;

        public void NotifyPlayerFledAfterAttack()
        {
            _state.bunkerRaidMultiplier = 4.0f; // +300% increase (1.0 -> 4.0)
            _state.raidMultiplierDurationDays = 7; // For 1 week

            OnBunkerRaidThreatEscalated?.Invoke(_state, _state.bunkerRaidMultiplier, _state.raidMultiplierDurationDays);
        }

        public List<string> GenerateHighTierLoot()
        {
            return new List<string>
            {
                "military_ammunition_crate",
                "military_body_armor_heavy",
                "advanced_assault_rifle",
                "tactical_grenade_pack"
            };
        }
    }
}
