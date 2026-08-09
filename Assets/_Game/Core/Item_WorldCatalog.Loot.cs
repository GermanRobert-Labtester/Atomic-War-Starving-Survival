using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Faction source for world-item / attachment loot tables.
    /// Attachments are extremely rare as loose loot — usually already fitted on
    /// mil/rebel/black-ops/spec-ops/merc/bandit/insurgent weapons.
    /// </summary>
    public enum WorldLootFaction
    {
        Civilian = 0,
        Bandit = 1,
        Insurgent = 2,
        Rebel = 3,
        MercenaryRebel = 4,
        MercenaryMilitary = 5,
        Military = 6,
        SpecOpsRebel = 7,
        BlackOpsMilitary = 8
    }

    /// <summary>One rolled world-item stack (id + amount).</summary>
    public struct WorldLootRoll
    {
        public string ItemId;
        public int Amount;
        public bool IsAttachment;
        public bool ExtremelyRare;
    }

    /// <summary>
    /// Weighted faction loot tables for attachments, scrap, armour, food, fuel, tools.
    /// Pure data + RNG — scav/expedition/skirmish hosts inject results into inventory.
    /// </summary>
    public static partial class Item_WorldCatalog
    {
        /// <summary>Loose-attachment base chance (before danger scaling). Extremely low.</summary>
        public const float AttachmentChanceBlackOps = 0.040f;
        public const float AttachmentChanceSpecOps = 0.035f;
        public const float AttachmentChanceMerc = 0.020f;
        public const float AttachmentChanceMilitary = 0.015f;
        public const float AttachmentChanceRebel = 0.012f;
        public const float AttachmentChanceInsurgent = 0.005f;
        public const float AttachmentChanceBandit = 0.003f;
        public const float AttachmentChanceCivilian = 0f;

        private static readonly Dictionary<WorldLootFaction, WorldLootEntry[]> Pools =
            new Dictionary<WorldLootFaction, WorldLootEntry[]>();
        private static WorldLootEntry[] _attachmentPool;
        private static WorldLootEntry[] _attachmentPoolSpecialist;
        private static WorldLootEntry[] _attachmentPoolRaider;
        private static bool _lootBuilt;

        private struct WorldLootEntry
        {
            public string Id;
            public float Weight;
            public int MinAmount;
            public int MaxAmount;

            public WorldLootEntry(string id, float weight, int min = 1, int max = 1)
            {
                Id = id;
                Weight = weight;
                MinAmount = min;
                MaxAmount = max;
            }
        }

        /// <summary>
        /// Map economy / encounter / NPC faction snake_case ids onto world-loot buckets.
        /// </summary>
        public static WorldLootFaction MapWorldLootFaction(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return WorldLootFaction.Civilian;
            string f = factionId.ToLowerInvariant();

            if (f.Contains("black_ops") || f.Contains("blackops"))
                return WorldLootFaction.BlackOpsMilitary;
            if (f.Contains("spec_ops") || f.Contains("specops"))
                return WorldLootFaction.SpecOpsRebel;
            if (f.Contains("mercenary") && f.Contains("rebel"))
                return WorldLootFaction.MercenaryRebel;
            if (f.Contains("mercenary") && (f.Contains("mil") || f.Contains("army")))
                return WorldLootFaction.MercenaryMilitary;
            if (f.Contains("mercenary") || f.Contains("merc_"))
                return WorldLootFaction.MercenaryMilitary;
            if (f.Contains("military") || f.Contains("mil_") || f == "mil" || f.Contains("army")
                || f.Contains("national_guard") || f.Contains("deserter"))
                return WorldLootFaction.Military;
            if (f.Contains("rebel") || f.Contains("partisan"))
                return WorldLootFaction.Rebel;
            if (f.Contains("insurgent") || f.Contains("terror"))
                return WorldLootFaction.Insurgent;
            if (f.Contains("bandit") || f.Contains("raider") || f.Contains("looter")
                || f.Contains("scavenger"))
                return WorldLootFaction.Bandit;
            return WorldLootFaction.Civilian;
        }

        /// <summary>From AmmoFactionSource (shared with ammo loot hosts).</summary>
        public static WorldLootFaction FromAmmoSource(AmmoFactionSource source)
        {
            switch (source)
            {
                case AmmoFactionSource.BlackOpsMilitary: return WorldLootFaction.BlackOpsMilitary;
                case AmmoFactionSource.SpecOpsRebel: return WorldLootFaction.SpecOpsRebel;
                case AmmoFactionSource.MercenaryMilitary: return WorldLootFaction.MercenaryMilitary;
                case AmmoFactionSource.MercenaryRebel: return WorldLootFaction.MercenaryRebel;
                case AmmoFactionSource.MilitaryForces: return WorldLootFaction.Military;
                case AmmoFactionSource.RebelForces: return WorldLootFaction.Rebel;
                default: return WorldLootFaction.Civilian;
            }
        }

        public static bool IsArmedFaction(WorldLootFaction faction)
        {
            return faction != WorldLootFaction.Civilian;
        }

        public static bool IsSpecialistFaction(WorldLootFaction faction)
        {
            return faction == WorldLootFaction.BlackOpsMilitary
                || faction == WorldLootFaction.SpecOpsRebel;
        }

        /// <summary>
        /// Location loot table ids that should inject faction world gear
        /// (attachments still gated by extremely-rare chance).
        /// </summary>
        public static bool IsFactionGearLootTable(string lootTableId)
        {
            if (string.IsNullOrEmpty(lootTableId)) return false;
            string id = lootTableId.ToLowerInvariant();
            return id.Contains("military")
                || id.Contains("armory")
                || id.Contains("armoury")
                || id.Contains("ground_zero")
                || id.Contains("mil_")
                || id.Contains("rebel")
                || id.Contains("bandit")
                || id.Contains("insurgent")
                || id.Contains("raider")
                || id.Contains("cache")
                || id.Contains("checkpoint");
        }

        public static WorldLootFaction SourceForLootTable(string lootTableId)
        {
            if (string.IsNullOrEmpty(lootTableId)) return WorldLootFaction.Civilian;
            string id = lootTableId.ToLowerInvariant();
            if (id.Contains("black_ops") || id.Contains("blackops"))
                return WorldLootFaction.BlackOpsMilitary;
            if (id.Contains("spec_ops") || id.Contains("specops"))
                return WorldLootFaction.SpecOpsRebel;
            if (id.Contains("mercenary") && id.Contains("rebel"))
                return WorldLootFaction.MercenaryRebel;
            if (id.Contains("mercenary"))
                return WorldLootFaction.MercenaryMilitary;
            if (id.Contains("rebel"))
                return WorldLootFaction.Rebel;
            if (id.Contains("insurgent") || id.Contains("terror"))
                return WorldLootFaction.Insurgent;
            if (id.Contains("bandit") || id.Contains("raider") || id.Contains("looter"))
                return WorldLootFaction.Bandit;
            if (id.Contains("military") || id.Contains("armory") || id.Contains("armoury")
                || id.Contains("ground_zero") || id.Contains("mil_"))
                return WorldLootFaction.Military;
            return WorldLootFaction.Civilian;
        }

        /// <summary>Base probability a corpse/cache yields a loose military attachment.</summary>
        public static float AttachmentLooseChance(WorldLootFaction faction)
        {
            switch (faction)
            {
                case WorldLootFaction.BlackOpsMilitary: return AttachmentChanceBlackOps;
                case WorldLootFaction.SpecOpsRebel: return AttachmentChanceSpecOps;
                case WorldLootFaction.MercenaryMilitary:
                case WorldLootFaction.MercenaryRebel: return AttachmentChanceMerc;
                case WorldLootFaction.Military: return AttachmentChanceMilitary;
                case WorldLootFaction.Rebel: return AttachmentChanceRebel;
                case WorldLootFaction.Insurgent: return AttachmentChanceInsurgent;
                case WorldLootFaction.Bandit: return AttachmentChanceBandit;
                default: return AttachmentChanceCivilian;
            }
        }

        /// <summary>
        /// Weighted roll of world item ids for a faction source.
        /// Attachments are rolled separately at extremely low chance when
        /// <paramref name="allowAttachments"/> is true.
        /// </summary>
        public static List<WorldLootRoll> RollFactionWorldLoot(
            WorldLootFaction faction,
            System.Random rng,
            int count = 1,
            bool allowAttachments = true,
            float dangerLevel = 3f)
        {
            EnsureLoot();
            rng ??= AtomicWar._Game.Utilities.SeededRandom.Stream("item_worldcatalog_loot");
            // Soft cap keeps host rolls bounded; tests may request larger samples.
            count = Mathf.Clamp(count, 0, 64);
            var results = new List<WorldLootRoll>(count);
            if (count == 0) return results;

            // Optional single attachment attempt (not per-item — once per roll batch).
            if (allowAttachments && TryRollLooseAttachment(faction, rng, dangerLevel, out var att))
            {
                results.Add(att);
                if (results.Count >= count) return results;
            }

            var pool = GetPool(faction);
            if (pool == null || pool.Length == 0) return results;

            int need = count - results.Count;
            for (int i = 0; i < need; i++)
            {
                var entry = WeightedPick(pool, rng);
                if (entry.Id == null) continue;
                int amt = entry.MinAmount;
                if (entry.MaxAmount > entry.MinAmount)
                    amt = rng.Next(entry.MinAmount, entry.MaxAmount + 1);
                bool rare = false;
                if (TryGet(entry.Id, out var def))
                    rare = def.ExtremelyRare || def.MilitaryGrade;
                results.Add(new WorldLootRoll
                {
                    ItemId = entry.Id,
                    Amount = Mathf.Max(1, amt),
                    IsAttachment = false,
                    ExtremelyRare = rare
                });
            }
            return results;
        }

        /// <summary>
        /// Extremely rare loose attachment roll. Returns false most of the time —
        /// attachments are usually already mounted on faction weapons.
        /// </summary>
        public static bool TryRollLooseAttachment(
            WorldLootFaction faction,
            System.Random rng,
            float dangerLevel,
            out WorldLootRoll roll)
        {
            EnsureLoot();
            roll = default;
            rng ??= AtomicWar._Game.Utilities.SeededRandom.Stream("item_worldcatalog_loot");
            float chance = AttachmentLooseChance(faction);
            if (chance <= 0f) return false;
            // High danger slightly raises the floor (still tiny).
            chance *= 1f + Mathf.Clamp(dangerLevel, 0f, 6f) * 0.08f;
            chance = Mathf.Clamp(chance, 0f, 0.12f);
            if (rng.NextDouble() >= chance) return false;

            var pool = GetAttachmentPool(faction);
            if (pool == null || pool.Length == 0) return false;
            var entry = WeightedPick(pool, rng);
            if (entry.Id == null) return false;
            roll = new WorldLootRoll
            {
                ItemId = entry.Id,
                Amount = 1,
                IsAttachment = true,
                ExtremelyRare = true
            };
            return true;
        }

        /// <summary>Convenience: ids only (amount collapsed to 1 each for hosts that stack later).</summary>
        public static List<string> RollFactionWorldLootIds(
            WorldLootFaction faction,
            System.Random rng,
            int count = 1,
            bool allowAttachments = true,
            float dangerLevel = 3f)
        {
            var rolls = RollFactionWorldLoot(faction, rng, count, allowAttachments, dangerLevel);
            var ids = new List<string>(rolls.Count);
            for (int i = 0; i < rolls.Count; i++)
            {
                if (!string.IsNullOrEmpty(rolls[i].ItemId))
                    ids.Add(rolls[i].ItemId);
            }
            return ids;
        }

        /// <summary>
        /// Field-scavenge world gear from a skirmish winner (corpses / abandoned kit).
        /// Attachments remain extremely rare.
        /// </summary>
        public static List<WorldLootRoll> RollScavengedWorldLoot(
            string winningFactionId,
            System.Random rng,
            int corpseCount = 1)
        {
            rng ??= AtomicWar._Game.Utilities.SeededRandom.Stream("item_worldcatalog_loot");
            var faction = MapWorldLootFaction(winningFactionId);
            if (string.Equals(winningFactionId, "Mutual Destruction", StringComparison.Ordinal)
                || string.Equals(winningFactionId, "none", StringComparison.Ordinal)
                || string.Equals(winningFactionId, "player_involved", StringComparison.Ordinal))
            {
                // Both sides bled — mix mil + rebel casings / scrap.
                var mixed = new List<WorldLootRoll>();
                mixed.AddRange(RollFactionWorldLoot(WorldLootFaction.Military, rng, 1, true, 4f));
                mixed.AddRange(RollFactionWorldLoot(WorldLootFaction.Rebel, rng, 1, true, 4f));
                return mixed;
            }

            int count = corpseCount >= 4 ? 2 : 1;
            if (IsSpecialistFaction(faction) && corpseCount >= 2)
                count = Mathf.Min(3, count + 1);
            float danger = IsSpecialistFaction(faction) ? 5.5f
                : IsArmedFaction(faction) ? 4f : 2f;
            return RollFactionWorldLoot(faction, rng, count, allowAttachments: true, dangerLevel: danger);
        }

        /// <summary>
        /// Build a runtime <see cref="ItemDefinition"/> from a world catalog id
        /// (when the SO catalog has not imported the row yet).
        /// </summary>
        public static ItemDefinition CreateItemDefinition(string itemId)
        {
            if (!TryGet(itemId, out var def) || def == null) return null;
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = def.Id;
            item.displayName = def.DisplayName;
            item.description = def.Description;
            item.type = def.Type;
            item.stackMax = Mathf.Max(1, def.StackMax);
            item.weight = def.Weight;
            item.tradeValue = def.TradeValue;
            item.tradeTier = def.TradeTier;
            item.isEquipable = def.IsEquipable;
            item.equipSlot = ParseEquipSlot(def.EquipSlot);
            item.durability = def.Durability;
            item.radProtection = def.RadProtection;
            item.hungerRestore = def.HungerRestore;
            item.thirstRestore = def.ThirstRestore;
            item.healthEffect = def.HealthEffect;
            item.moraleEffect = def.MoraleEffect;
            return item;
        }

        private static EquipSlot ParseEquipSlot(string slot)
        {
            // Shared with the JSON import gate so the two paths cannot drift apart
            // again; an unrecognised spelling is logged rather than silently
            // downgrading the item to an unequippable EquipSlot.None.
            if (EquipSlots.TryParse(slot, out var parsed)) return parsed;

            Debug.LogWarning(
                $"[Item_WorldCatalog] Unknown equipSlot '{slot}' — item will not be equippable. " +
                $"Valid: {string.Join(", ", EquipSlots.CanonicalNames)}");
            return EquipSlot.None;
        }

        public static IReadOnlyList<string> GetPoolItemIds(WorldLootFaction faction)
        {
            EnsureLoot();
            var pool = GetPool(faction);
            var list = new List<string>(pool.Length);
            for (int i = 0; i < pool.Length; i++)
                list.Add(pool[i].Id);
            return list;
        }

        public static IReadOnlyList<string> GetAttachmentPoolIds(WorldLootFaction faction)
        {
            EnsureLoot();
            var pool = GetAttachmentPool(faction);
            var list = new List<string>(pool.Length);
            for (int i = 0; i < pool.Length; i++)
                list.Add(pool[i].Id);
            return list;
        }

        // ── Pool construction ────────────────────────────────────────────

        private static void EnsureLoot()
        {
            if (_lootBuilt) return;
            lock (Pools)
            {
                if (_lootBuilt) return;
                BuildLootPools();
                _lootBuilt = true;
            }
        }

        private static void BuildLootPools()
        {
            // Attachments — specialist-weighted; double-scope rarest.
            _attachmentPool = new[]
            {
                new WorldLootEntry(AttMilSuppressor, 1.2f),
                new WorldLootEntry(AttMilLaserdot, 1.0f),
                new WorldLootEntry(AttMilTacticalGrip, 1.4f),
                new WorldLootEntry(AttMilLongRangeScope, 0.8f),
                new WorldLootEntry(AttMilHolosight, 1.1f),
                new WorldLootEntry(AttMilDoubleScope5x10x, 0.35f)
            };

            // Faction-skewed variants. Built once alongside the default pool:
            // GetAttachmentPool is on the per-drop path and these are read-only.
            _attachmentPoolSpecialist = new[]
            {
                new WorldLootEntry(AttMilSuppressor, 1.4f),
                new WorldLootEntry(AttMilLaserdot, 1.3f),
                new WorldLootEntry(AttMilTacticalGrip, 1.0f),
                new WorldLootEntry(AttMilLongRangeScope, 1.2f),
                new WorldLootEntry(AttMilHolosight, 1.1f),
                new WorldLootEntry(AttMilDoubleScope5x10x, 0.55f)
            };

            // Salvaged scraps — no double scopes from raiders.
            _attachmentPoolRaider = new[]
            {
                new WorldLootEntry(AttMilTacticalGrip, 1.5f),
                new WorldLootEntry(AttMilHolosight, 1.0f),
                new WorldLootEntry(AttMilSuppressor, 0.6f),
                new WorldLootEntry(AttMilLaserdot, 0.5f)
            };

            // Shared scrap / components
            var scrap = new[]
            {
                new WorldLootEntry(ShellCasing, 3f, 2, 8),
                new WorldLootEntry(BulletCasing, 3f, 2, 10),
                new WorldLootEntry(ScrapMetal, 2.5f, 1, 3),
                new WorldLootEntry(Cloth, 2f, 1, 3),
                new WorldLootEntry(Gunpowder, 1.2f, 1, 2),
                new WorldLootEntry(SalvagedTechTrash, 1.5f, 1, 2)
            };

            var broken = new[]
            {
                new WorldLootEntry(CrowbarBroken, 1.2f),
                new WorldLootEntry(WireCuttersBroken, 1f),
                new WorldLootEntry(MetalPipeBroken, 1.3f),
                new WorldLootEntry(ShovelBroken, 1f),
                new WorldLootEntry(MultitoolBroken, 0.8f),
                new WorldLootEntry(KnifeBroken, 1.1f),
                new WorldLootEntry(HammerBroken, 1f),
                new WorldLootEntry(ScrewdriverBroken, 1f)
            };

            var civilianFood = new[]
            {
                new WorldLootEntry(CannedFood, 2.5f, 1, 2),
                new WorldLootEntry(CannedMeat, 1.5f),
                new WorldLootEntry(PreservedCrackers, 2f, 1, 2),
                new WorldLootEntry(VegetableCarrot, 1.5f, 1, 3),
                new WorldLootEntry(VegetablePotato, 1.5f, 1, 3),
                new WorldLootEntry(VegetableBeetroot, 1.2f, 1, 2),
                new WorldLootEntry(WaterBottleHalfOf1L, 1.8f),
                new WorldLootEntry(WaterBottle1LFull, 1.2f),
                new WorldLootEntry(WaterBottleEmpty, 2f, 1, 2)
            };

            // Deprecated scrap-only bullets (sample of common calibers)
            var deprecatedAmmo = new[]
            {
                new WorldLootEntry(DeprecatedBulletId("cal_9x19"), 1.5f, 1, 6),
                new WorldLootEntry(DeprecatedBulletId("cal_12ga"), 1.2f, 1, 4),
                new WorldLootEntry(DeprecatedBulletId("cal_556x45"), 1.0f, 1, 4),
                new WorldLootEntry(DeprecatedBulletId("cal_762x39"), 1.0f, 1, 4)
            };

            Pools[WorldLootFaction.Civilian] = Concat(
                civilianFood,
                scrap,
                broken,
                new[]
                {
                    new WorldLootEntry(Fertilizer, 1.5f, 1, 2),
                    new WorldLootEntry(Hammer, 0.8f),
                    new WorldLootEntry(Screwdriver, 0.9f),
                    new WorldLootEntry(KnifeImprovised, 1.0f),
                    new WorldLootEntry(MetalPipe, 1.1f),
                    new WorldLootEntry(FuelHalfOf1L, 0.7f),
                    new WorldLootEntry(BodyArmourDeprecated, 0.4f),
                    new WorldLootEntry(HelmetDeprecated, 0.5f)
                });

            Pools[WorldLootFaction.Bandit] = Concat(
                scrap,
                broken,
                deprecatedAmmo,
                new[]
                {
                    new WorldLootEntry(Crowbar, 1.4f),
                    new WorldLootEntry(KnifeImprovised, 1.6f),
                    new WorldLootEntry(MetalPipe, 1.5f),
                    new WorldLootEntry(Lockpick, 0.8f),
                    new WorldLootEntry(WireCutters, 0.9f),
                    new WorldLootEntry(AccelerantHalf, 1.0f),
                    new WorldLootEntry(AccelerantFull, 0.5f),
                    new WorldLootEntry(FuelHalfOf1L, 1.1f),
                    new WorldLootEntry(CannedFood, 1.5f, 1, 2),
                    new WorldLootEntry(WaterBottleHalfOf2L, 1.2f),
                    new WorldLootEntry(BodyArmourDeprecated, 1.2f),
                    new WorldLootEntry(HelmetDeprecated, 1.3f),
                    new WorldLootEntry(HelmetHeavyDeprecated, 0.7f),
                    new WorldLootEntry(BodyArmourHeavyDeprecated, 0.6f),
                    new WorldLootEntry(Shovel, 0.7f)
                });

            Pools[WorldLootFaction.Insurgent] = Concat(
                scrap,
                deprecatedAmmo,
                new[]
                {
                    new WorldLootEntry(Gunpowder, 2.0f, 1, 3),
                    new WorldLootEntry(Sulphur, 1.5f, 1, 2),
                    new WorldLootEntry(ExplosivePowderNitroglycerin, 0.35f),
                    new WorldLootEntry(GrenadeMilitary, 0.4f),
                    new WorldLootEntry(Lockpick, 1.2f),
                    new WorldLootEntry(WireCutters, 1.3f),
                    new WorldLootEntry(Crowbar, 1.1f),
                    new WorldLootEntry(KnifeSwissBattle, 0.6f),
                    new WorldLootEntry(Multitool, 0.8f),
                    new WorldLootEntry(AccelerantFull, 0.9f),
                    new WorldLootEntry(Fuel1L, 0.8f),
                    new WorldLootEntry(BodyArmourDeprecated, 1.0f),
                    new WorldLootEntry(HelmetDeprecated, 1.0f),
                    new WorldLootEntry(CannedMeat, 1.2f),
                    new WorldLootEntry(WaterBottle1LOf2L, 1.0f),
                    new WorldLootEntry(ShellCasing, 2.5f, 3, 12),
                    new WorldLootEntry(BulletCasing, 2.5f, 3, 12)
                });

            var milCommon = new[]
            {
                new WorldLootEntry(ShellCasing, 2.5f, 4, 14),
                new WorldLootEntry(BulletCasing, 2.5f, 4, 14),
                new WorldLootEntry(Gunpowder, 1.5f, 1, 3),
                new WorldLootEntry(MreMilitary, 1.8f, 1, 2),
                new WorldLootEntry(Fuel1L, 1.2f),
                new WorldLootEntry(FuelHalfOf1L, 1.0f),
                new WorldLootEntry(WaterBottle1LFull, 1.3f),
                new WorldLootEntry(WaterBottle2LFull, 0.7f),
                new WorldLootEntry(Cloth, 1.0f, 1, 2),
                new WorldLootEntry(ScrapMetal, 1.2f, 1, 2),
                new WorldLootEntry(Multitool, 0.9f),
                new WorldLootEntry(Screwdriver, 0.8f),
                new WorldLootEntry(WireCutters, 0.9f)
            };

            Pools[WorldLootFaction.Rebel] = Concat(
                milCommon,
                new[]
                {
                    new WorldLootEntry(BodyArmourMilitary, 0.7f),
                    new WorldLootEntry(HelmetMilitary, 0.8f),
                    new WorldLootEntry(GrenadeMilitary, 0.45f),
                    new WorldLootEntry(KnifeSwissBattle, 0.7f),
                    new WorldLootEntry(BayonetSwissMachete, 0.5f),
                    new WorldLootEntry(Crowbar, 1.0f),
                    new WorldLootEntry(Lockpick, 0.9f),
                    new WorldLootEntry(BodyArmourDeprecated, 0.9f),
                    new WorldLootEntry(HelmetDeprecated, 0.9f),
                    new WorldLootEntry(PreservedCrackers, 1.2f, 1, 2),
                    new WorldLootEntry(DeprecatedBulletId("cal_545x39"), 1.0f, 1, 5),
                    new WorldLootEntry(DeprecatedBulletId("cal_762x39"), 1.1f, 1, 5)
                });

            Pools[WorldLootFaction.Military] = Concat(
                milCommon,
                new[]
                {
                    new WorldLootEntry(BodyArmourMilitary, 1.0f),
                    new WorldLootEntry(HelmetMilitary, 1.1f),
                    new WorldLootEntry(HelmetHeavyMilitary, 0.45f),
                    new WorldLootEntry(ArmourHeavyMilitary, 0.3f),
                    new WorldLootEntry(GrenadeMilitary, 0.7f),
                    new WorldLootEntry(KnifeSwissBattle, 0.8f),
                    new WorldLootEntry(BayonetSwissMachete, 0.55f),
                    new WorldLootEntry(Shovel, 0.9f),
                    new WorldLootEntry(Hammer, 0.7f),
                    new WorldLootEntry(DeprecatedBulletId("cal_556x45"), 1.0f, 1, 6),
                    new WorldLootEntry(DeprecatedBulletId("cal_762x51"), 0.8f, 1, 4),
                    new WorldLootEntry(SalvagedTechTrash, 1.0f)
                });

            Pools[WorldLootFaction.MercenaryMilitary] = Concat(
                Pools[WorldLootFaction.Military],
                new[]
                {
                    new WorldLootEntry(NvGogglesMilitary, 0.35f),
                    new WorldLootEntry(Lockpick, 1.0f),
                    new WorldLootEntry(AccelerantFull, 0.8f),
                    new WorldLootEntry(HeartyMealCooked, 0.5f)
                });

            Pools[WorldLootFaction.MercenaryRebel] = Concat(
                Pools[WorldLootFaction.Rebel],
                new[]
                {
                    new WorldLootEntry(NvGogglesMilitary, 0.25f),
                    new WorldLootEntry(GrenadeMilitary, 0.6f),
                    new WorldLootEntry(ExplosivePowderNitroglycerin, 0.3f)
                });

            Pools[WorldLootFaction.SpecOpsRebel] = Concat(
                milCommon,
                new[]
                {
                    new WorldLootEntry(BodyArmourMilitary, 1.0f),
                    new WorldLootEntry(HelmetMilitary, 1.0f),
                    new WorldLootEntry(NvGogglesMilitary, 0.7f),
                    new WorldLootEntry(GrenadeMilitary, 0.9f),
                    new WorldLootEntry(KnifeSwissBattle, 1.0f),
                    new WorldLootEntry(BayonetSwissMachete, 0.8f),
                    new WorldLootEntry(Multitool, 1.1f),
                    new WorldLootEntry(Lockpick, 1.2f),
                    new WorldLootEntry(ExplosivePowderNitroglycerin, 0.5f),
                    new WorldLootEntry(MreMilitary, 1.5f, 1, 2),
                    new WorldLootEntry(HelmetHeavyMilitary, 0.5f),
                    new WorldLootEntry(ArmourHeavyMilitary, 0.35f)
                });

            Pools[WorldLootFaction.BlackOpsMilitary] = Concat(
                milCommon,
                new[]
                {
                    new WorldLootEntry(BodyArmourMilitary, 1.1f),
                    new WorldLootEntry(HelmetMilitary, 1.1f),
                    new WorldLootEntry(HelmetHeavyMilitary, 0.7f),
                    new WorldLootEntry(ArmourHeavyMilitary, 0.55f),
                    new WorldLootEntry(NvGogglesMilitary, 0.9f),
                    new WorldLootEntry(GrenadeMilitary, 1.0f),
                    new WorldLootEntry(KnifeSwissBattle, 1.0f),
                    new WorldLootEntry(BayonetSwissMachete, 0.85f),
                    new WorldLootEntry(Multitool, 1.0f),
                    new WorldLootEntry(ExplosivePowderNitroglycerin, 0.55f),
                    new WorldLootEntry(MreMilitary, 1.6f, 1, 2),
                    new WorldLootEntry(SalvagedTechTrash, 1.2f, 1, 2),
                    new WorldLootEntry(WaterBottle1_5LOf2L, 0.9f)
                });
        }

        private static WorldLootEntry[] GetPool(WorldLootFaction faction)
        {
            EnsureLoot();
            if (Pools.TryGetValue(faction, out var pool)) return pool;
            return Pools[WorldLootFaction.Civilian];
        }

        /// <summary>
        /// Specialist factions weight long-range / dual scopes higher;
        /// bandits only ever see grips/holosights if they get a drop at all.
        /// </summary>
        private static WorldLootEntry[] GetAttachmentPool(WorldLootFaction faction)
        {
            EnsureLoot();
            if (faction == WorldLootFaction.BlackOpsMilitary
                || faction == WorldLootFaction.SpecOpsRebel)
            {
                return _attachmentPoolSpecialist;
            }
            if (faction == WorldLootFaction.Bandit || faction == WorldLootFaction.Insurgent)
            {
                return _attachmentPoolRaider;
            }
            return _attachmentPool;
        }

        private static WorldLootEntry WeightedPick(WorldLootEntry[] pool, System.Random rng)
        {
            if (pool == null || pool.Length == 0) return default;
            float total = 0f;
            for (int i = 0; i < pool.Length; i++)
                total += pool[i].Weight;
            if (total <= 0f) return pool[rng.Next(pool.Length)];

            double roll = rng.NextDouble() * total;
            for (int i = 0; i < pool.Length; i++)
            {
                roll -= pool[i].Weight;
                if (roll <= 0) return pool[i];
            }
            return pool[pool.Length - 1];
        }

        private static WorldLootEntry[] Concat(params WorldLootEntry[][] parts)
        {
            int n = 0;
            for (int i = 0; i < parts.Length; i++)
                if (parts[i] != null) n += parts[i].Length;
            var result = new WorldLootEntry[n];
            int w = 0;
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i] == null) continue;
                for (int j = 0; j < parts[i].Length; j++)
                    result[w++] = parts[i][j];
            }
            return result;
        }
    }
}
