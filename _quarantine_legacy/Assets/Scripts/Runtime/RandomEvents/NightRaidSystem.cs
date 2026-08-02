using System.Collections.Generic;
using AtomicWar.Core.Events;
using AtomicWar.Data;
using AtomicWar.Runtime.GameState;
using AtomicWar.Runtime.Inventory;
using AtomicWar.Runtime.RandomEvents;
using AtomicWar.Runtime.Survivors;
using UnityEngine;

namespace AtomicWar.Runtime.RandomEvents
{
    public struct NightRaidEventTriggered
    {
        public string RaidMessage;
        public bool GuardedSuccessfully;
        public int FoodStolen;
    }

    /// <summary>
    /// Pure C# system processing random night raids on the shelter.
    /// Evaluates active guards, stolen inventory resources, and health injuries.
    /// </summary>
    public class NightRaidSystem
    {
        private readonly InventorySystem _inventorySystem;
        private readonly SurvivorSystem _survivorSystem;

        public NightRaidSystem(InventorySystem inventorySystem, SurvivorSystem survivorSystem)
        {
            _inventorySystem = inventorySystem;
            _survivorSystem = survivorSystem;
            EventBus.Subscribe<PhaseChangedEvent>(OnPhaseChanged);
        }

        public void Unsubscribe()
        {
            EventBus.Unsubscribe<PhaseChangedEvent>(OnPhaseChanged);
        }

        private void OnPhaseChanged(PhaseChangedEvent e)
        {
            // Trigger night raid chance when entering Night phase
            if (e.NewPhase == DayCyclePhase.Night)
            {
                EvaluateNightRaid();
            }
        }

        public void EvaluateNightRaid()
        {
            // 40% chance of night raid occurring
            if (Random.value > 0.40f) return;

            Debug.LogWarning("[NightRaidSystem] A BANDIT RAID IS OCCURRING TONIGHT!");

            // Check if any survivor is guarding
            bool hasGuard = false;
            float totalGuardCombatPower = 0f;

            foreach (var survivor in _survivorSystem.GetLivingSurvivors())
            {
                if (survivor.CurrentState == SurvivorState.Guard)
                {
                    hasGuard = true;
                    totalGuardCombatPower += survivor.Data != null ? survivor.Data.CombatEfficiency : 1.0f;
                }
            }

            if (hasGuard && totalGuardCombatPower >= 1.0f)
            {
                // Raid repelled by guards
                Debug.Log("[NightRaidSystem] Guards successfully repelled the night raid!");
                EventBus.Raise(new NightRaidEventTriggered
                {
                    RaidMessage = "Bandits tried to raid the shelter, but your guards repelled them!",
                    GuardedSuccessfully = true,
                    FoodStolen = 0
                });
            }
            else
            {
                // Unprotected or weak defense: lose food and take minor damage
                int foodCount = _inventorySystem.GetItemCount("item_food");
                int foodStolen = Mathf.Min(foodCount, Random.Range(1, 3));

                if (foodStolen > 0)
                {
                    // Remove stolen food
                    var foodItem = ScriptableObject.CreateInstance<ItemData>();
                    foodItem.Id = "item_food";
                    _inventorySystem.RemoveItem(foodItem, foodStolen);
                }

                // Inflict injuries on sleeping survivors
                foreach (var survivor in _survivorSystem.GetLivingSurvivors())
                {
                    survivor.Health = Mathf.Clamp(survivor.Health - Random.Range(10f, 25f), 0f, 100f);
                }

                Debug.LogWarning($"[NightRaidSystem] Raid failed defense! {foodStolen} food stolen, survivors injured.");

                EventBus.Raise(new NightRaidEventTriggered
                {
                    RaidMessage = $"Bandits raided the shelter! Stole {foodStolen} food and injured survivors.",
                    GuardedSuccessfully = false,
                    FoodStolen = foodStolen
                });
            }
        }
    }
}
