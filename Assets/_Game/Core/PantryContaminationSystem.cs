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
        private Survivors.PersonalQuestSystem _personalQuests;
        private Func<IReadOnlyList<Survivor>> _getSurvivors;
        private ShelterRoom _storesRoom;
        /// <summary>Prompt #212 — Quartermaster degradation mult for the active room.</summary>
        private Func<string, float> _getDegradationMult;

        /// <summary>#256 personal-stash food spoil chance per unit per game-hour (when not Dragon's Hoard).</summary>
        public const float PersonalStashSpoilChancePerHour = 0.03f;
        public const string SpoiledMeatItemId = "spoiled_meat";

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

        /// <summary>
        /// Prompt #212 — inject room degradation multiplier (Quartermaster → 0.5).
        /// Signature: roomId → rate mult (1 = normal, 0.5 = half spoil).
        /// </summary>
        public void BindDegradationMultiplier(Func<string, float> getMult) =>
            _getDegradationMult = getMult;

        /// <summary>
        /// Prompt #224 Survivalist + #256 Dragon's Hoard personal-stash never-spoils.
        /// </summary>
        public void BindPersonalQuests(
            Survivors.PersonalQuestSystem personalQuests,
            Func<IReadOnlyList<Survivor>> getSurvivors = null)
        {
            _personalQuests = personalQuests;
            _getSurvivors = getSurvivors;
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
        /// Prompt #212 — Quartermaster in the room halves spoil chance.
        /// </summary>
        public void Tick(float gameHours, ShelterRoom humiditySource = null)
        {
            if (gameHours <= 0f) return;

            int converted = 0;

            // #256: personal hidden stashes may spoil unless Dragon's Hoard
            // (independent of bunker humidity — stash sits on the person).
            converted += TickPersonalStashes(_getSurvivors?.Invoke(), gameHours);

            if (_inventory != null)
            {
                var room = humiditySource ?? _storesRoom;
                float humidity = room != null ? room.Humidity : 0f;
                if (humidity >= HumidityRustThreshold)
                {
                    EnsureContaminatedDef();
                    if (_contaminatedFoodDef != null)
                    {
                        string roomId = room != null ? room.RoomId : null;
                        float degMult = 1f;
                        if (_getDegradationMult != null && !string.IsNullOrEmpty(roomId))
                            degMult = Mathf.Clamp(_getDegradationMult(roomId), 0f, 1f);
                        float effectiveHours = gameHours * degMult;

                        var slots = _inventory.Slots;
                        if (slots != null)
                        {
                            for (int i = slots.Count - 1; i >= 0; i--)
                            {
                                var slot = slots[i];
                                if (slot?.Item == null || slot.Amount <= 0) continue;
                                if (slot.Item.type != ItemType.Food) continue;

                                int amount = slot.Amount;
                                int toConvert = 0;
                                for (int u = 0; u < amount; u++)
                                {
                                    if (_rng.NextDouble() < RustChancePerUnitPerHour * effectiveHours)
                                        toConvert++;
                                }
                                if (toConvert <= 0) continue;

                                var foodDef = slot.Item;
                                int removed = Mathf.Min(toConvert, slot.Amount);
                                if (!_inventory.Remove(foodDef, removed)) continue;
                                _inventory.Add(_contaminatedFoodDef, removed);
                                converted += removed;
                            }
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
                                    if (_rng.NextDouble() < RustChancePerUnitPerHour * effectiveHours)
                                        toConvert++;
                                }
                                if (toConvert <= 0) continue;

                                int take = Mathf.Min(toConvert, slot.Amount);
                                slot.RemoveItem(take);
                                _inventory.Add(_contaminatedFoodDef, take);
                                converted += take;
                            }
                        }
                    }
                }
            }

            if (converted > 0)
            {
                OnFoodRusted?.Invoke(converted);
                OnPantryChanged?.Invoke();
            }
        }

        /// <summary>
        /// #256 — food ids in <see cref="Survivor.HiddenItemIds"/> slowly convert to
        /// spoiled meat unless the owner has Dragon's Hoard
        /// (<see cref="PersonalQuestSystem.ItemInPersonalStashNeverSpoils"/>).
        /// Returns number of units spoiled.
        /// </summary>
        public int TickPersonalStashes(IReadOnlyList<Survivor> survivors, float gameHours)
        {
            if (survivors == null || gameHours <= 0f) return 0;
            int spoiled = 0;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                if (sv.HiddenItemIds == null || sv.HiddenItemIds.Count == 0) continue;
                if (_personalQuests != null
                    && _personalQuests.ItemInPersonalStashNeverSpoils(sv))
                    continue;

                for (int k = 0; k < sv.HiddenItemIds.Count; k++)
                {
                    string id = sv.HiddenItemIds[k];
                    if (!IsSpoilablePersonalFoodId(id)) continue;
                    if (_rng.NextDouble() >= PersonalStashSpoilChancePerHour * gameHours)
                        continue;
                    sv.HiddenItemIds[k] = SpoiledMeatItemId;
                    spoiled++;
                }
            }
            return spoiled;
        }

        /// <summary>Heuristic: hidden ids that look like food/rations can spoil.</summary>
        public static bool IsSpoilablePersonalFoodId(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            if (string.Equals(itemId, SpoiledMeatItemId, StringComparison.OrdinalIgnoreCase))
                return false;
            if (string.Equals(itemId, ContaminatedFoodItemId, StringComparison.OrdinalIgnoreCase))
                return false;
            // Gear tags / non-food never spoil.
            if (itemId.StartsWith("butcher_loot_", StringComparison.OrdinalIgnoreCase))
                return false;
            if (itemId.StartsWith("fake_stash_", StringComparison.OrdinalIgnoreCase))
                return false;
            return itemId.IndexOf("food", StringComparison.OrdinalIgnoreCase) >= 0
                || itemId.IndexOf("ration", StringComparison.OrdinalIgnoreCase) >= 0
                || itemId.IndexOf("can", StringComparison.OrdinalIgnoreCase) >= 0
                || itemId.IndexOf("bean", StringComparison.OrdinalIgnoreCase) >= 0
                || itemId.IndexOf("meat", StringComparison.OrdinalIgnoreCase) >= 0
                || itemId.IndexOf("bread", StringComparison.OrdinalIgnoreCase) >= 0
                || itemId.IndexOf("grain", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// After consuming ContaminatedFood, roll for Botulism affliction.
        /// Call from eat path when item.type == ContaminatedFood.
        /// </summary>
        public bool TryRollBotulism(Survivor eater, MedicalSystem medical)
        {
            if (eater == null || !eater.IsAlive || medical == null) return false;
            // Prompt #224 — Survivalist eats raw ContaminatedFood without sickness.
            if (_personalQuests != null
                && _personalQuests.CanEatContaminatedWithoutSickness(eater))
                return false;
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
