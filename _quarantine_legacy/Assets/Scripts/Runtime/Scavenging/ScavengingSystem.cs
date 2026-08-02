using System.Collections.Generic;
using AtomicWar.Core.Events;
using AtomicWar.Data;
using AtomicWar.Runtime.Inventory;
using AtomicWar.Runtime.Survivors;
using UnityEngine;

namespace AtomicWar.Runtime.Scavenging
{
    public class ScavengeRunData
    {
        public SurvivorModel Scavenger { get; set; }
        public ScavengeLocationData Location { get; set; }
        public List<InventorySlot> LootCollected { get; } = new List<InventorySlot>();
    }

    public struct ScavengeCompletedEvent
    {
        public SurvivorModel Scavenger;
        public ScavengeLocationData Location;
        public bool Success;
        public string SummaryReport;
    }

    /// <summary>
    /// Pure C# system processing night scavenging trips, risk factors, combat risk, and loot return.
    /// </summary>
    public class ScavengingSystem
    {
        private readonly InventorySystem _shelterInventory;
        private ScavengeRunData _currentRun;

        public ScavengingSystem(InventorySystem shelterInventory)
        {
            _shelterInventory = shelterInventory;
        }

        public bool PrepareScavengeRun(SurvivorModel scavenger, ScavengeLocationData location)
        {
            if (scavenger == null || location == null || !scavenger.IsAlive) return false;

            _currentRun = new ScavengeRunData
            {
                Scavenger = scavenger,
                Location = location
            };

            scavenger.CurrentState = SurvivorState.Scavenging;
            Debug.Log($"[ScavengingSystem] Prepared run: {scavenger.Data.CharacterName} -> {location.LocationName}");
            return true;
        }

        public void ResolveNightRun()
        {
            if (_currentRun == null || _currentRun.Scavenger == null) return;

            var scavenger = _currentRun.Scavenger;
            var location = _currentRun.Location;

            float currentWeight = 0f;
            float maxWeight = scavenger.Data.CarryCapacityWeight;

            // Generate loot based on location loot pool
            foreach (var itemIngredient in location.PossibleLootPool)
            {
                int rolledAmount = Random.Range(1, itemIngredient.Amount + 1);
                float itemWeight = itemIngredient.Item.Weight * rolledAmount;

                if (currentWeight + itemWeight <= maxWeight)
                {
                    _shelterInventory.AddItem(itemIngredient.Item, rolledAmount);
                    currentWeight += itemWeight;
                }
            }

            // Calculate danger outcomes
            string report = $"{scavenger.Data.CharacterName} returned from {location.LocationName}. ";
            bool survived = true;

            if (location.Danger != DangerLevel.Safe)
            {
                float dangerChance = (int)location.Danger * 0.25f;
                if (Random.value < dangerChance)
                {
                    float damage = Random.Range(15f, 40f) / scavenger.Data.CombatEfficiency;
                    scavenger.Health -= damage;
                    report += $"Encountered hostiles and took {damage:F0} damage!";

                    if (scavenger.Health <= 0)
                    {
                        survived = false;
                        report = $"{scavenger.Data.CharacterName} failed to return from {location.LocationName}!";
                    }
                }
            }

            if (survived)
            {
                scavenger.CurrentState = SurvivorState.Idle;
                scavenger.Fatigue = Mathf.Clamp(scavenger.Fatigue + 40f, 0f, 100f);
            }

            EventBus.Raise(new ScavengeCompletedEvent
            {
                Scavenger = scavenger,
                Location = location,
                Success = survived,
                SummaryReport = report
            });

            _currentRun = null;
        }
    }
}
