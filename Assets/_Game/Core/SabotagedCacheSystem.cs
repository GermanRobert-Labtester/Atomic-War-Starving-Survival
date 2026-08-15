using System;
using Ashfall.Core.Economy;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Survivors;
using UnityEngine;
using Random = System.Random;
using Ashfall.Core.Journal;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Result of processing a looted medical item for sabotage (Prompt #13).
    /// </summary>
    public enum SabotagedLootOutcome
    {
        /// <summary>Item is safe — keep as rolled.</summary>
        Clean,
        /// <summary>Item replaced with poisoned iodine (undetected).</summary>
        Poisoned,
        /// <summary>Tampering spotted — item discarded, not taken home.</summary>
        DetectedAndDiscarded
    }

    /// <summary>
    /// Prompt #13 — Sabotaged Caches: hostile factions learn scavenging habits
    /// and plant poisoned medical crates. High Medical skill or Paranoid bias
    /// can spot the swap; otherwise the first consumer pays.
    /// </summary>
    public class SabotagedCacheSystem
    {
        public const string PoisonedIodineItemId = "poisoned_iodine_pills";
        public const string RealIodineItemId = "iodine_pills";

        /// <summary>Scavenge habit score before sabotage can begin.</summary>
        public const int HabitThreshold = 5;

        /// <summary>Chance a medical loot roll is swapped once habit is high + hostiles exist.</summary>
        public const float SabotageChance = 0.55f;

        /// <summary>Medical skill at/above this spots tampering.</summary>
        public const float MedicalDetectionThreshold = 0.6f;

        /// <summary>Immediate health loss when poisoned pills are swallowed.</summary>
        public const float PoisonConsumeHealthDamage = 45f;

        /// <summary>Morale hit on consume.</summary>
        public const float PoisonConsumeMoraleDamage = 15f;

        private readonly Random _rng;
        private DynamicEconomySystem _economy;
        private ItemDefinition _poisonedIodineDef;
        private int _habitScore;
        private int _cachesPlanted;
        private int _cachesDetected;
        private int _poisonsConsumed;

        public int HabitScore => _habitScore;
        public int CachesPlanted => _cachesPlanted;
        public int CachesDetected => _cachesDetected;
        public int PoisonsConsumed => _poisonsConsumed;

        public event Action<Survivor, string> OnTamperingDetected;
        public event Action<Survivor> OnPoisonPlanted;
        public event Action<Survivor> OnPoisonConsumed;
        public event Action OnStateChanged;

        public SabotagedCacheSystem(Random rng = null)
        {
            _rng = rng ?? new Random(91);
            _poisonedIodineDef = CreatePoisonedIodineDefinition();
        }

        public void BindEconomy(DynamicEconomySystem economy)
        {
            _economy = economy;
        }

        public void SetPoisonedIodineDefinition(ItemDefinition def)
        {
            if (def != null) _poisonedIodineDef = def;
        }

        public ItemDefinition PoisonedIodineDefinition => _poisonedIodineDef;

        /// <summary>Factory for the deceptive medical item (looks like iodine).</summary>
        public static ItemDefinition CreatePoisonedIodineDefinition()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = PoisonedIodineItemId;
            item.displayName = "Iodine Pills";
            item.description =
                "Pristine foil. The grit inside is wrong — too coarse, too grey. " +
                "Or it is not, if you do not look.";
            item.type = ItemType.Medical;
            item.stackMax = 10;
            item.weight = 0.05f;
            item.healthEffect = -PoisonConsumeHealthDamage;
            item.moraleEffect = -PoisonConsumeMoraleDamage;
            item.tradeValue = 8f;
            item.contamination = 0.2f;
            return item;
        }

        /// <summary>Factory for poison_ingestion affliction (ongoing drain).</summary>
        public static AfflictionSO CreatePoisonIngestionAffliction()
        {
            var a = ScriptableObject.CreateInstance<AfflictionSO>();
            a.id = AfflictionSO.Ids.PoisonIngestion;
            a.displayName = "Poison Ingestion";
            a.description = "Rat poison. The gut burns. The hands shake.";
            a.phase = AfflictionPhase.Phase1;
            a.healthDrainPerHour = 6f;
            a.baseLethality = 1.4f;
            a.progressionHours = 18f;
            a.progressesToId = AfflictionSO.Ids.Sepsis;
            a.emergencyHaltItemId = "anti_toxin";
            return a;
        }

        /// <summary>Call when a scavenger successfully takes loot from the field.</summary>
        public void RecordScavengeLoot(string nodeId = null)
        {
            _habitScore = Math.Min(100, _habitScore + 1);
            OnStateChanged?.Invoke();
        }

        /// <summary>True when any known faction is HostileRaid or Rob.</summary>
        public bool HasHostileFactionLearningHabits()
        {
            if (_economy?.Factions == null || _economy.Factions.Count == 0) return false;
            foreach (var kv in _economy.Factions)
            {
                var f = kv.Value;
                if (f == null || string.IsNullOrEmpty(f.id)) continue;
                var stance = _economy.GetStance(f.id);
                if (stance == TradeStance.HostileRaid || stance == TradeStance.Rob)
                    return true;
            }
            return false;
        }

        /// <summary>Test hook: set habit score without looping scavenges.</summary>
        public void SetHabitScoreForTests(int score)
        {
            _habitScore = Math.Max(0, Math.Min(100, score));
        }

        /// <summary>Whether sabotage rolls are eligible (habit + hostiles).</summary>
        public bool IsSabotageActive =>
            _habitScore >= HabitThreshold && HasHostileFactionLearningHabits();

        public static bool CanDetectTampering(Survivor scavenger)
        {
            if (scavenger == null) return false;
            if (scavenger.EffectiveMedicalSkill >= MedicalDetectionThreshold) return true;
            if (scavenger.RiskBias == RiskBiasTrait.Paranoid) return true;
            if (scavenger.HasTrait("medical") || scavenger.HasTrait("medic")) return true;
            return false;
        }

        public static bool IsMedicalLootCandidate(ItemDefinition item)
        {
            if (item == null) return false;
            if (item.id == PoisonedIodineItemId) return false;
            return item.type == ItemType.Medical
                || item.type == ItemType.Iodine
                || item.type == ItemType.AntiRad
                || string.Equals(item.id, RealIodineItemId, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Process a rolled loot item. May replace medical loot with poison or discard on detect.
        /// Non-medical items always return Clean with the same item.
        /// </summary>
        public SabotagedLootOutcome ProcessLootCandidate(
            Survivor scavenger,
            ItemDefinition rolledItem,
            out ItemDefinition resultItem)
        {
            resultItem = rolledItem;
            if (rolledItem == null || scavenger == null) return SabotagedLootOutcome.Clean;
            if (!IsMedicalLootCandidate(rolledItem)) return SabotagedLootOutcome.Clean;
            if (!IsSabotageActive) return SabotagedLootOutcome.Clean;
            if (_rng.NextDouble() >= SabotageChance) return SabotagedLootOutcome.Clean;

            // Hostile plant succeeds — check detection
            if (CanDetectTampering(scavenger))
            {
                _cachesDetected++;
                resultItem = null;
                OnTamperingDetected?.Invoke(scavenger,
                    "The seals are wrong. Crushed grit where iodine should be. Left it.");
                OnStateChanged?.Invoke();
                return SabotagedLootOutcome.DetectedAndDiscarded;
            }

            EnsurePoisonDef();
            resultItem = _poisonedIodineDef;
            _cachesPlanted++;
            OnPoisonPlanted?.Invoke(scavenger);
            OnStateChanged?.Invoke();
            return SabotagedLootOutcome.Poisoned;
        }

        /// <summary>
        /// After inventory remove/consume of poisoned pills: afflict + track.
        /// Call when item id is <see cref="PoisonedIodineItemId"/> (Inventory already applied healthEffect).
        /// </summary>
        public bool TryApplyPoisonOnConsume(ItemDefinition item, Survivor consumer, MedicalSystem medical)
        {
            if (item == null || consumer == null || !consumer.IsAlive) return false;
            if (!string.Equals(item.id, PoisonedIodineItemId, StringComparison.OrdinalIgnoreCase))
                return false;

            if (medical != null)
                medical.Inflict(consumer, AfflictionSO.Ids.PoisonIngestion);

            // Extra immediate damage if NeedsSystem path skipped healthEffect
            if (consumer.Needs != null && item.healthEffect >= 0f)
            {
                SurvivorNeedWrite.AdjustHealth(consumer, -PoisonConsumeHealthDamage);
            }

            _poisonsConsumed++;
            OnPoisonConsumed?.Invoke(consumer);
            OnStateChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Full consume path for tests / AI: remove one from inventory, apply needs, poison.
        /// </summary>
        public bool ConsumePoisonedFromInventory(
            Inventory.Inventory inventory,
            Survivor consumer,
            MedicalSystem medical,
            NeedsSystem needs = null)
        {
            if (inventory == null || consumer == null) return false;
            EnsurePoisonDef();
            if (inventory.Count(_poisonedIodineDef) < 1
                && inventory.CountById(PoisonedIodineItemId) < 1)
                return false;

            // Prefer def instance; fall back to id scan via Count if needed
            ItemDefinition def = _poisonedIodineDef;
            if (inventory.Count(def) < 1)
            {
                // Find matching slot by id
                def = FindPoisonInInventory(inventory);
                if (def == null) return false;
            }

            if (!inventory.Consume(def, consumer, radiation: null, needs: needs))
                return false;

            return TryApplyPoisonOnConsume(def, consumer, medical);
        }

        private static ItemDefinition FindPoisonInInventory(Inventory.Inventory inventory)
        {
            if (inventory?.Slots == null) return null;
            for (int i = 0; i < inventory.Slots.Count; i++)
            {
                var s = inventory.Slots[i];
                if (s?.Item != null
                    && string.Equals(s.Item.id, PoisonedIodineItemId, StringComparison.OrdinalIgnoreCase))
                    return s.Item;
            }
            return null;
        }

        private void EnsurePoisonDef()
        {
            if (_poisonedIodineDef == null)
                _poisonedIodineDef = CreatePoisonedIodineDefinition();
        }

        public SabotagedCacheSave CaptureState()
        {
            return new SabotagedCacheSave
            {
                HabitScore = _habitScore,
                CachesPlanted = _cachesPlanted,
                CachesDetected = _cachesDetected,
                PoisonsConsumed = _poisonsConsumed
            };
        }

        public void RestoreState(SabotagedCacheSave save)
        {
            if (save == null)
            {
                _habitScore = 0;
                _cachesPlanted = 0;
                _cachesDetected = 0;
                _poisonsConsumed = 0;
                return;
            }
            _habitScore = Math.Max(0, save.HabitScore);
            _cachesPlanted = Math.Max(0, save.CachesPlanted);
            _cachesDetected = Math.Max(0, save.CachesDetected);
            _poisonsConsumed = Math.Max(0, save.PoisonsConsumed);
        }
    }

    [Serializable]
    public class SabotagedCacheSave
    {
        public int HabitScore;
        public int CachesPlanted;
        public int CachesDetected;
        public int PoisonsConsumed;
    }
}
