using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// High humidity rusts canned goods: ItemType.Food quietly becomes ContaminatedFood.
    /// Eating contaminated food rolls for Botulism (respiratory paralysis).
    /// Internal Horror — Botulism &amp; The Rusting Pantry.
    /// </summary>
    public class PantryContaminationSystem
    {
        public const string ContaminatedFoodItemId = "contaminated_food";

        /// <summary>Humidity at/above which canned goods begin to rust.</summary>
        public const float HumidityRustThreshold = 0.65f;

        /// <summary>Chance per game-hour per food unit to convert when humid.</summary>
        public const float RustChancePerUnitPerHour = 0.04f;

        /// <summary>Probability of botulism when eating ContaminatedFood.</summary>
        public const float BotulismChanceOnEat = 0.35f;

        private readonly Inventory.Inventory _inventory;
        private readonly System.Random _rng;
        private ItemDefinition _contaminatedFoodDef;
        private ShelterRoom _storesRoom;

        public event Action<int> OnFoodRusted;
        public event Action<Survivor> OnBotulismContracted;
        public event Action OnPantryChanged;

        public PantryContaminationSystem(
            Inventory.Inventory inventory,
            System.Random rng = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _rng = rng ?? new System.Random(73);
        }

        public void SetContaminatedFoodDefinition(ItemDefinition def)
        {
            _contaminatedFoodDef = def;
        }

        public void SetStoresRoom(ShelterRoom room) => _storesRoom = room;

        public static ItemDefinition CreateContaminatedFoodDefinition()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = ContaminatedFoodItemId;
            item.displayName = "Rusted Can";
            item.description = "The seal bulged. It still smells like food if you don't think about it.";
            item.type = ItemType.ContaminatedFood;
            item.stackMax = 20;
            item.weight = 0.4f;
            item.hungerRestore = 25f;
            item.healthEffect = -5f;
            item.moraleEffect = -8f;
            item.contamination = 0.3f;
            item.tradeValue = 1f;
            return item;
        }

        /// <summary>
        /// Convert Food → ContaminatedFood while room humidity is high.
        /// </summary>
        public void Tick(float gameHours, ShelterRoom humiditySource = null)
        {
            if (gameHours <= 0f || _inventory == null) return;

            var room = humiditySource ?? _storesRoom;
            float humidity = room != null ? room.Humidity : 0f;
            if (humidity < HumidityRustThreshold) return;

            EnsureContaminatedDef();
            if (_contaminatedFoodDef == null) return;

            int converted = 0;
            var slots = _inventory.Slots;
            if (slots == null) return;

            // Snapshot candidates first (mutate carefully)
            for (int i = slots.Count - 1; i >= 0; i--)
            {
                var slot = slots[i];
                if (slot?.Item == null || slot.Amount <= 0) continue;
                if (slot.Item.type != ItemType.Food) continue;

                int amount = slot.Amount;
                int toConvert = 0;
                for (int u = 0; u < amount; u++)
                {
                    if (_rng.NextDouble() < RustChancePerUnitPerHour * gameHours)
                        toConvert++;
                }
                if (toConvert <= 0) continue;

                var foodDef = slot.Item;
                int removed = Mathf.Min(toConvert, slot.Amount);
                if (!_inventory.Remove(foodDef, removed)) continue;
                _inventory.Add(_contaminatedFoodDef, removed);
                converted += removed;
            }

            // Also rust food sitting in room storage slots
            if (room?.Slots != null)
            {
                for (int i = 0; i < room.Slots.Count; i++)
                {
                    var slot = room.Slots[i];
                    if (slot == null || slot.IsEmpty || slot.Item == null) continue;
                    if (slot.Item.type != ItemType.Food) continue;

                    int toConvert = 0;
                    for (int u = 0; u < slot.Amount; u++)
                    {
                        if (_rng.NextDouble() < RustChancePerUnitPerHour * gameHours)
                            toConvert++;
                    }
                    if (toConvert <= 0) continue;

                    int take = Mathf.Min(toConvert, slot.Amount);
                    slot.RemoveItem(take);
                    // Put contaminated into bunker inventory when room slot empties partially
                    _inventory.Add(_contaminatedFoodDef, take);
                    converted += take;
                }
            }

            if (converted > 0)
            {
                OnFoodRusted?.Invoke(converted);
                OnPantryChanged?.Invoke();
            }
        }

        /// <summary>
        /// After consuming ContaminatedFood, roll for Botulism affliction.
        /// Call from eat path when item.type == ContaminatedFood.
        /// </summary>
        public bool TryRollBotulism(Survivor eater, MedicalSystem medical)
        {
            if (eater == null || !eater.IsAlive || medical == null) return false;
            if (_rng.NextDouble() > BotulismChanceOnEat) return false;

            if (medical.HasAffliction(eater, AfflictionSO.Ids.Botulism)) return false;
            if (!medical.Inflict(eater, AfflictionSO.Ids.Botulism)) return false;

            OnBotulismContracted?.Invoke(eater);
            return true;
        }

        /// <summary>
        /// Consume one ContaminatedFood (or any food) and apply botulism risk when contaminated.
        /// </summary>
        public bool ConsumeFood(
            ItemDefinition item,
            Survivor eater,
            NeedsSystem needs,
            MedicalSystem medical,
            RadiationSystem radiation = null)
        {
            if (item == null || eater == null || _inventory == null) return false;
            if (!_inventory.Consume(item, eater, radiation, needs)) return false;

            if (item.type == ItemType.ContaminatedFood)
                TryRollBotulism(eater, medical);

            return true;
        }

        private void EnsureContaminatedDef()
        {
            if (_contaminatedFoodDef != null) return;
            var slot = _inventory.FindSlot(ContaminatedFoodItemId);
            if (slot?.Item != null)
            {
                _contaminatedFoodDef = slot.Item;
                return;
            }
            _contaminatedFoodDef = CreateContaminatedFoodDefinition();
        }

        public PantryContaminationSave CaptureState()
        {
            // Stateless beyond inventory contents; placeholder for future accumulators.
            return new PantryContaminationSave();
        }

        public void RestoreState(PantryContaminationSave save)
        {
            // no-op
        }
    }

    [Serializable]
    public class PantryContaminationSave
    {
        // Reserved for future rust progress accumulators per stack.
        public int Version = 1;
    }
}
