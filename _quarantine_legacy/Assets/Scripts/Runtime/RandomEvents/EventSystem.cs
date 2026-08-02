using System.Collections.Generic;
using AtomicWar.Core.Events;
using AtomicWar.Data;
using AtomicWar.Runtime.Inventory;
using AtomicWar.Runtime.Survivors;
using UnityEngine;

namespace AtomicWar.Runtime.RandomEvents
{
    public struct EventTriggeredEvent
    {
        public EventData Event;
    }

    public struct ChoiceResolvedEvent
    {
        public EventData Event;
        public EventChoice ChosenOption;
    }

    /// <summary>
    /// Pure C# system handling random story events, moral dilemmas, choices, and consequences.
    /// </summary>
    public class EventSystem
    {
        private readonly InventorySystem _inventorySystem;
        private readonly SurvivorSystem _survivorSystem;

        public EventData ActiveEvent { get; private set; }

        public EventSystem(InventorySystem inventorySystem, SurvivorSystem survivorSystem)
        {
            _inventorySystem = inventorySystem;
            _survivorSystem = survivorSystem;
        }

        public void TriggerEvent(EventData eventData)
        {
            ActiveEvent = eventData;
            Debug.Log($"[EventSystem] Triggered Event: {eventData.Title}");
            EventBus.Raise(new EventTriggeredEvent { Event = eventData });
        }

        public bool SelectChoice(int choiceIndex)
        {
            if (ActiveEvent == null || choiceIndex < 0 || choiceIndex >= ActiveEvent.Choices.Count)
                return false;

            var choice = ActiveEvent.Choices[choiceIndex];

            // Verify costs
            foreach (var cost in choice.ItemCosts)
            {
                if (!_inventorySystem.HasItemAmount(cost.Item.Id, cost.Amount))
                {
                    Debug.LogWarning($"[EventSystem] Cannot afford choice cost: {cost.Item.DisplayName}");
                    return false;
                }
            }

            // Deduct costs
            foreach (var cost in choice.ItemCosts)
            {
                _inventorySystem.RemoveItem(cost.Item, cost.Amount);
            }

            // Award rewards
            foreach (var reward in choice.ItemRewards)
            {
                _inventorySystem.AddItem(reward.Item, reward.Amount);
            }

            // Apply moral and health changes to living survivors
            foreach (var survivor in _survivorSystem.GetLivingSurvivors())
            {
                if (choice.MoraleChange != 0f)
                {
                    float moraleDelta = choice.MoraleChange * survivor.Data.MoralSensitivity;
                    survivor.Morale = Mathf.Clamp(survivor.Morale + moraleDelta, 0f, 100f);
                }

                if (choice.HealthChange != 0f)
                {
                    survivor.Health = Mathf.Clamp(survivor.Health + choice.HealthChange, 0f, survivor.Data.MaxHealth);
                }
            }

            EventBus.Raise(new ChoiceResolvedEvent
            {
                Event = ActiveEvent,
                ChosenOption = choice
            });

            ActiveEvent = null;
            return true;
        }
    }
}
