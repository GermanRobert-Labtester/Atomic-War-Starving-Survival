using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Faction loot pools, raid armor ratings, and combat helpers that wire
    /// <see cref="Item_AmmoTypes.ResolveHit"/> into hatch / expedition / skirmish paths.
    /// </summary>
    public partial class Item_AmmoTypes
    {
        /// <summary>Baseline armor rating for hostile hatch raids by faction id.</summary>
        public const float ArmorUnarmored = 0f;
        public const float ArmorLightRaider = 8f;
        public const float ArmorMilitary = 28f;
        public const float ArmorBlackOps = 40f;
        public const float ArmorMutant = 12f;

        private static List<string> _civilianLootCache;
        private static List<string> _militaryExclusiveCache;
        private static List<string> _battleRifleCache;
        private static List<string> _apApiCache;

        /// <summary>
        /// Map economy / encounter faction snake_case ids onto ammo source buckets.
        /// </summary>
        public static AmmoFactionSource MapFactionId(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return AmmoFactionSource.CivilianCraft;
            string f = factionId.ToLowerInvariant();

            if (f.Contains("black_ops") || f.Contains("blackops"))
                return AmmoFactionSource.BlackOpsMilitary;
            if (f.Contains("spec_ops") || f.Contains("specops"))
                return AmmoFactionSource.SpecOpsRebel;
            if (f.Contains("mercenary"))
                return f.Contains("rebel") ? AmmoFactionSource.MercenaryRebel : AmmoFactionSource.MercenaryMilitary;
            if (f.Contains("military") || f.Contains("mil_") || f == "mil" || f.Contains("army")
                || f.Contains("national_guard") || f.Contains("deserter"))
                return AmmoFactionSource.MilitaryForces;
            if (f.Contains("rebel") || f.Contains("insurgent") || f.Contains("partisan"))
                return AmmoFactionSource.RebelForces;
            return AmmoFactionSource.CivilianCraft;
        }

        /// <summary>Armor value used by ResolveHit for a raid / encounter faction.</summary>
        public static float GetFactionArmor(string factionId)
        {
            var src = MapFactionId(factionId);
            switch (src)
            {
                case AmmoFactionSource.BlackOpsMilitary:
                case AmmoFactionSource.SpecOpsRebel:
                    return ArmorBlackOps;
                case AmmoFactionSource.MilitaryForces:
                case AmmoFactionSource.MercenaryMilitary:
                case AmmoFactionSource.MercenaryRebel:
                case AmmoFactionSource.RebelForces:
                    return ArmorMilitary;
                default:
                    return ArmorLightRaider;
            }
        }

        /// <summary>Armor inferred from expedition encounter ids (mutant/animal/warlord/human).</summary>
        public static float InferEncounterArmor(string encounterId)
        {
            if (string.IsNullOrEmpty(encounterId)) return ArmorLightRaider;
            string id = encounterId.ToLowerInvariant();
            if (id.Contains("warlord") || id.Contains("juggernaut") || id.Contains("mil"))
                return ArmorMilitary;
            if (id.Contains("mutant") || id.Contains("ghoul"))
                return ArmorMutant;
            if (id.Contains("dog") || id.Contains("animal") || id.Contains("feral"))
                return ArmorUnarmored;
            if (id.Contains("bandit") || id.Contains("looter") || id.Contains("raider")
                || id.Contains("deserter") || id.Contains("scavenger"))
                return ArmorLightRaider;
            return ArmorLightRaider;
        }

        public static bool IsMilitaryOrRebelSource(AmmoFactionSource source)
        {
            return source == AmmoFactionSource.MilitaryForces
                || source == AmmoFactionSource.RebelForces
                || source == AmmoFactionSource.MercenaryMilitary
                || source == AmmoFactionSource.MercenaryRebel
                || source == AmmoFactionSource.BlackOpsMilitary
                || source == AmmoFactionSource.SpecOpsRebel;
        }

        /// <summary>
        /// Location loot table ids that should inject military/rebel exclusive ammo.
        /// </summary>
        public static bool IsMilitaryLootTable(string lootTableId)
        {
            if (string.IsNullOrEmpty(lootTableId)) return false;
            string id = lootTableId.ToLowerInvariant();
            return id.Contains("military")
                || id.Contains("ground_zero")
                || id.Contains("armory")
                || id.Contains("mil_");
        }

        public static AmmoFactionSource SourceForLootTable(string lootTableId)
        {
            if (!IsMilitaryLootTable(lootTableId)) return AmmoFactionSource.CivilianCraft;
            string id = (lootTableId ?? string.Empty).ToLowerInvariant();
            if (id.Contains("rebel")) return AmmoFactionSource.RebelForces;
            return AmmoFactionSource.MilitaryForces;
        }

        public static IReadOnlyList<string> GetCivilianLootIds()
        {
            EnsureCatalog();
            if (_civilianLootCache != null) return _civilianLootCache;
            var list = new List<string>();
            foreach (var kv in Catalog)
            {
                if (kv.Value.Craftable) list.Add(kv.Key);
            }
            _civilianLootCache = list;
            return list;
        }

        public static IReadOnlyList<string> GetMilitaryExclusiveLootIds()
        {
            EnsureCatalog();
            if (_militaryExclusiveCache != null) return _militaryExclusiveCache;
            var list = new List<string>();
            foreach (var kv in Catalog)
            {
                if (!kv.Value.Craftable && IsExclusiveToFactions(kv.Key))
                    list.Add(kv.Key);
            }
            _militaryExclusiveCache = list;
            return list;
        }

        public static IReadOnlyList<string> GetBattleRifleExclusiveIds()
        {
            EnsureCatalog();
            if (_battleRifleCache != null) return _battleRifleCache;
            var list = new List<string>();
            foreach (var kv in Catalog)
            {
                if (kv.Value.WeaponClass == WeaponAmmoClass.BattleRifle)
                    list.Add(kv.Key);
            }
            _battleRifleCache = list;
            return list;
        }

        public static IReadOnlyList<string> GetApApiLootIds()
        {
            EnsureCatalog();
            if (_apApiCache != null) return _apApiCache;
            var list = new List<string>();
            foreach (var kv in Catalog)
            {
                if (kv.Value.Modification == BulletModification.Ap
                    || kv.Value.Modification == BulletModification.Api
                    || kv.Value.Modification == BulletModification.M855A1
                    || kv.Value.Modification == BulletModification.JhpAp
                    || kv.Value.Modification == BulletModification.ExplosiveIncendiary)
                    list.Add(kv.Key);
            }
            _apApiCache = list;
            return list;
        }

        /// <summary>
        /// Weighted roll of ammo item ids for a faction source.
        /// Military/rebel sources only return non-craftable exclusives (AP/API/battle/sniper/AV).
        /// Civilian returns craftable loads only.
        /// </summary>
        public static List<string> RollFactionAmmoLoot(
            AmmoFactionSource source,
            System.Random rng,
            int count = 1,
            bool preferApApi = false)
        {
            EnsureCatalog();
            rng ??= AtomicWar._Game.Utilities.SeededRandom.CreateFixed("item_ammotypes_combatloot");
            count = Mathf.Clamp(count, 0, 8);
            var results = new List<string>(count);
            if (count == 0) return results;

            IReadOnlyList<string> pool;
            if (IsMilitaryOrRebelSource(source))
            {
                if (preferApApi)
                {
                    var ap = GetApApiLootIds();
                    var br = GetBattleRifleExclusiveIds();
                    var merged = new List<string>(ap.Count + br.Count);
                    for (int i = 0; i < ap.Count; i++) merged.Add(ap[i]);
                    for (int i = 0; i < br.Count; i++)
                        if (!merged.Contains(br[i])) merged.Add(br[i]);
                    // Fall back to all exclusives if merge empty.
                    pool = merged.Count > 0 ? merged : GetMilitaryExclusiveLootIds();
                }
                else
                {
                    pool = GetMilitaryExclusiveLootIds();
                }
            }
            else
            {
                pool = GetCivilianLootIds();
            }

            if (pool == null || pool.Count == 0) return results;

            for (int i = 0; i < count; i++)
            {
                // Weight rarer military loads slightly higher when exclusive.
                string pick = WeightedPick(pool, rng, weightByRarity: IsMilitaryOrRebelSource(source));
                if (!string.IsNullOrEmpty(pick))
                    results.Add(pick);
            }
            return results;
        }

        /// <summary>Default military intervention rewards (skirmish mil-vs-rebel).</summary>
        public static List<string> DefaultMilitaryInterventionRewards()
        {
            return new List<string>
            {
                "ammo_556x45_ap",
                "ammo_556x45_m855a1",
                "ammo_762x51_ap"
            };
        }

        /// <summary>Default rebel intervention rewards.</summary>
        public static List<string> DefaultRebelInterventionRewards()
        {
            return new List<string>
            {
                "ammo_762x39_ap",
                "ammo_545x39_ap",
                "ammo_300blk_ap"
            };
        }

        /// <summary>
        /// Hatch stockpile defense contribution using ResolveHit vs raid armor.
        /// Soft-cap so huge stacks do not trivialise raids.
        /// </summary>
        public float GetAmmoStockpileDefensePower(string ammoItemId, int amount, float targetArmor)
        {
            if (amount <= 0) return 0f;
            if (!TryGetLoad(ammoItemId, out var load))
            {
                // Legacy unknown ammo: keep prior soft scale.
                return Mathf.Min(20f, amount * 0.4f);
            }

            var hit = ResolveHit(load.Id, load.BaseDamage, targetArmor);
            float perRound = Mathf.Max(0.05f, hit.FinalDamage * 0.08f);
            // AP / exclusive rounds punch above their count.
            if (!load.Craftable) perRound *= 1.35f;
            if (load.Modification == BulletModification.Ap
                || load.Modification == BulletModification.Api
                || load.Modification == BulletModification.M855A1)
                perRound *= 1.2f;

            return Mathf.Min(28f, amount * perRound);
        }

        /// <summary>
        /// Prefer burning civilian craftable ammo first when repelling raids
        /// (save military exclusives). Returns true if id is preferred to spend.
        /// </summary>
        public static int AmmoSpendPriority(string ammoItemId)
        {
            // Lower = spend first.
            if (!TryGetLoad(ammoItemId, out var load)) return 50;
            if (load.Craftable) return 10;
            if (load.Modification == BulletModification.Fmj
                || load.Modification == BulletModification.SoftLead)
                return 30;
            if (load.Modification == BulletModification.Jhp) return 20;
            if (load.Modification == BulletModification.Ap) return 80;
            if (load.Modification == BulletModification.Api) return 90;
            if (load.Modification == BulletModification.M855A1) return 85;
            if (load.Modification == BulletModification.BoatTail) return 88;
            return 60;
        }

        /// <summary>
        /// Whether a craft recipe result is allowed on the workbench (civilian only).
        /// Unknown ids are allowed (non-ammo recipes).
        /// </summary>
        public static bool IsWorkbenchCraftAllowed(string resultItemId)
        {
            if (string.IsNullOrEmpty(resultItemId)) return true;
            if (!IsAmmoItemId(resultItemId)) return true;
            if (!TryGetLoad(resultItemId, out var load)) return true;
            return load.Craftable;
        }

        private static string WeightedPick(IReadOnlyList<string> pool, System.Random rng, bool weightByRarity)
        {
            if (pool == null || pool.Count == 0) return null;
            if (!weightByRarity)
                return pool[rng.Next(pool.Count)];

            float total = 0f;
            for (int i = 0; i < pool.Count; i++)
            {
                TryGetLoad(pool[i], out var load);
                total += RarityWeight(load != null ? load.Rarity : AmmoRarity.Common);
            }
            if (total <= 0f) return pool[rng.Next(pool.Count)];

            double roll = rng.NextDouble() * total;
            for (int i = 0; i < pool.Count; i++)
            {
                TryGetLoad(pool[i], out var load);
                roll -= RarityWeight(load != null ? load.Rarity : AmmoRarity.Common);
                if (roll <= 0) return pool[i];
            }
            return pool[pool.Count - 1];
        }

        private static float RarityWeight(AmmoRarity rarity)
        {
            // Slight bias toward mid exclusives so mythic/legendary are rare drops.
            switch (rarity)
            {
                case AmmoRarity.Common: return 4f;
                case AmmoRarity.Uncommon: return 3.5f;
                case AmmoRarity.Rare: return 3f;
                case AmmoRarity.VeryRare: return 2.2f;
                case AmmoRarity.MythicRare: return 1.0f;
                case AmmoRarity.LegendaryVeryRare: return 0.45f;
                default: return 1f;
            }
        }
    }
}
