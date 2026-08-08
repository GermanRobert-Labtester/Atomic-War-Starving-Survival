using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Result structure after resolving a moral dilemma.
    /// </summary>
    public class MoralDilemmaResult
    {
        public MoralDilemmaEvent Event;
        public DesperateChoiceKind Choice;
        public int FoodRestored;
        public int MoralePenaltiesApplied;
        public int TraumasInflicted;
    }

    /// <summary>
    /// Pure C# system handling Cannibalism & Moral Dilemmas under extreme starvation (Prompt #38).
    /// Raised when Hunger >= 90 and shelter storage has zero food.
    /// </summary>
    
    [Serializable]
    public class MoralDilemmaSystemSave
    {
        public string systemId = "moral_dilemma_system";
    }
public class MoralDilemmaSystem
    {
        public const float CriticalHungerThreshold = 90f;
        public const float BaseMoralePenaltyForButchering = -40f;
        public const float ReducedMoralePenaltyForButchering = -20f;
        public const string CannibalismTraumaId = "cannibalism_trauma";

        public MoralDilemmaEvent ActiveDilemma { get; private set; }

        private NeedsSystem _needsSystem;
        public void SetNeedsSystem(NeedsSystem ns) => _needsSystem = ns;

        public event Action<MoralDilemmaEvent> OnDilemmaRaised;
        public event Action<MoralDilemmaEvent> OnDilemmaResolved;

        /// <summary>
        /// Check if starvation conditions trigger a MoralDilemmaEvent.
        /// Requires at least one living survivor with Hunger >= 90 and zero food in shelter storage.
        /// </summary>
        public bool CheckForDilemmaTrigger(IReadOnlyList<Survivor> survivors, Inventory.Inventory shelterInventory, int day = 1)
        {
            if (survivors == null || survivors.Count == 0) return false;
            if (ActiveDilemma != null && !ActiveDilemma.IsResolved) return false;

            float highestHunger = 0f;
            int livingCount = 0;
            int deadCount = 0;
            bool criticalStarvation = false;

            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s == null) continue;

                if (!s.IsAlive)
                {
                    deadCount++;
                }
                else
                {
                    livingCount++;
                    if (s.Needs != null)
                    {
                        highestHunger = Mathf.Max(highestHunger, s.Needs.Hunger);
                        if (s.Needs.Hunger >= CriticalHungerThreshold)
                        {
                            criticalStarvation = true;
                        }
                    }
                }
            }

            if (!criticalStarvation || livingCount == 0) return false;

            // Check if shelter inventory has 0 food items
            int foodCount = GetStoredFoodCount(shelterInventory);
            if (foodCount > 0) return false;

            var dilemma = new MoralDilemmaEvent
            {
                Id = "moral_dilemma_day_" + day,
                Day = day,
                CriticalHunger = highestHunger,
                LivingSurvivorCount = livingCount,
                DeadSurvivorCount = deadCount,
                IsResolved = false,
                Message = "Food stores are empty. Critical starvation setting in."
            };

            ActiveDilemma = dilemma;
            OnDilemmaRaised?.Invoke(dilemma);
            EventBus.Raise(dilemma);
            return true;
        }

        /// <summary>
        /// Resolve an active moral dilemma with a chosen DesperateChoiceKind.
        /// </summary>
        public MoralDilemmaResult ResolveChoice(DesperateChoiceKind choice, IReadOnlyList<Survivor> survivors, Inventory.Inventory shelterInventory)
        {
            if (ActiveDilemma == null || ActiveDilemma.IsResolved) return null;

            ActiveDilemma.IsResolved = true;
            ActiveDilemma.ChosenResolution = choice;

            var result = new MoralDilemmaResult
            {
                Event = ActiveDilemma,
                Choice = choice
            };

            if (survivors == null) return result;

            switch (choice)
            {
                case DesperateChoiceKind.Butchering:
                    // 1. Restore food supply (e.g. 2 canned_food / emergency ration units)
                    int restored = AddEmergencyFood(shelterInventory, 2);
                    result.FoodRestored = restored;

                    // 2. Apply morale penalty and trauma to living survivors
                    for (int i = 0; i < survivors.Count; i++)
                    {
                        var s = survivors[i];
                        if (s == null || !s.IsAlive || s.Needs == null) continue;

                        bool isPsychopath = s.HasTrait("Psychopath");
                        bool isSurvivalist = s.HasTrait("Survivalist");

                        float moraleDelta = (isPsychopath || isSurvivalist)
                            ? ReducedMoralePenaltyForButchering
                            : BaseMoralePenaltyForButchering;

                        if (_needsSystem != null)
                            _needsSystem.Modify(s, NeedKind.Morale, moraleDelta);
                        else
                            s.Needs.Morale = Mathf.Clamp(s.Needs.Morale + moraleDelta, 0f, 100f);
                        result.MoralePenaltiesApplied++;

                        if (!isPsychopath)
                        {
                            if (!s.HasTrauma(CannibalismTraumaId))
                            {
                                s.Traumas.Add(CannibalismTraumaId);
                                result.TraumasInflicted++;
                            }
                        }
                    }
                    break;

                case DesperateChoiceKind.Starve:
                    for (int i = 0; i < survivors.Count; i++)
                    {
                        var s = survivors[i];
                        if (s?.Needs == null || !s.IsAlive) continue;
                        if (_needsSystem != null)
                            _needsSystem.Modify(s, NeedKind.Morale, -10f);
                        else
                            s.Needs.Morale = Mathf.Clamp(s.Needs.Morale - 10f, 0f, 100f);
                        result.MoralePenaltiesApplied++;
                    }
                    break;

                case DesperateChoiceKind.AbandonWeak:
                    for (int i = 0; i < survivors.Count; i++)
                    {
                        var s = survivors[i];
                        if (s?.Needs == null || !s.IsAlive) continue;
                        if (_needsSystem != null)
                            _needsSystem.Modify(s, NeedKind.Morale, -20f);
                        else
                            s.Needs.Morale = Mathf.Clamp(s.Needs.Morale - 20f, 0f, 100f);
                        result.MoralePenaltiesApplied++;
                    }
                    break;
            }

            OnDilemmaResolved?.Invoke(ActiveDilemma);
            EventBus.Raise(ActiveDilemma);
            return result;
        }

        public static int GetStoredFoodCount(Inventory.Inventory inventory)
        {
            if (inventory?.Slots == null) return 0;
            int total = 0;
            for (int i = 0; i < inventory.Slots.Count; i++)
            {
                var slot = inventory.Slots[i];
                if (slot?.Item != null && slot.Amount > 0 && slot.Item.type == ItemType.Food)
                {
                    total += slot.Amount;
                }
            }
            return total;
        }

        private static int AddEmergencyFood(Inventory.Inventory inventory, int amount)
        {
            if (inventory == null || amount <= 0) return 0;

            var food = ScriptableObject.CreateInstance<ItemDefinition>();
            food.id = "canned_food";
            food.displayName = "Emergency Rations";
            food.type = ItemType.Food;
            food.stackMax = 10;
            food.weight = 0.5f;

            return inventory.Add(food, amount) ? amount : 0;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public MoralDilemmaSystemSave CaptureState() => new MoralDilemmaSystemSave();

        public void RestoreState(MoralDilemmaSystemSave saved) { _ = saved; }

}
}
