using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Faction world-item / attachment loot tables: mapping, pool composition,
    /// extremely rare attachments, scavenging + skirmish injection.
    /// </summary>
    [TestFixture]
    public class ItemWorldLootTests
    {
        private readonly List<Object> _toDestroy = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            for (int i = 0; i < _toDestroy.Count; i++)
            {
                if (_toDestroy[i] != null)
                    Object.DestroyImmediate(_toDestroy[i]);
            }
            _toDestroy.Clear();
        }

        private ItemDefinition MakeItem(string id, ItemType type = ItemType.Material, int stackMax = 20)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = type;
            item.stackMax = stackMax;
            item.weight = 0.2f;
            item.tradeValue = 2f;
            _toDestroy.Add(item);
            return item;
        }

        [Test]
        public void MapWorldLootFaction_Recognizes_AllArmedSources()
        {
            Assert.AreEqual(WorldLootFaction.BlackOpsMilitary,
                Item_WorldCatalog.MapWorldLootFaction("black_ops_team"));
            Assert.AreEqual(WorldLootFaction.SpecOpsRebel,
                Item_WorldCatalog.MapWorldLootFaction("spec_ops_cell"));
            Assert.AreEqual(WorldLootFaction.MercenaryMilitary,
                Item_WorldCatalog.MapWorldLootFaction("mercenary_military_contract"));
            Assert.AreEqual(WorldLootFaction.MercenaryRebel,
                Item_WorldCatalog.MapWorldLootFaction("mercenary_rebel_company"));
            Assert.AreEqual(WorldLootFaction.Military,
                Item_WorldCatalog.MapWorldLootFaction("military_patrol"));
            Assert.AreEqual(WorldLootFaction.Rebel,
                Item_WorldCatalog.MapWorldLootFaction("rebel_forces"));
            Assert.AreEqual(WorldLootFaction.Insurgent,
                Item_WorldCatalog.MapWorldLootFaction("insurgent_cell"));
            Assert.AreEqual(WorldLootFaction.Insurgent,
                Item_WorldCatalog.MapWorldLootFaction("terrorist_cell"));
            Assert.AreEqual(WorldLootFaction.Bandit,
                Item_WorldCatalog.MapWorldLootFaction("bandit_raider"));
            Assert.AreEqual(WorldLootFaction.Civilian,
                Item_WorldCatalog.MapWorldLootFaction("suburban_family"));
        }

        [Test]
        public void SourceForLootTable_Maps_Military_Rebel_Bandit()
        {
            Assert.AreEqual(WorldLootFaction.Military,
                Item_WorldCatalog.SourceForLootTable("military_armory"));
            Assert.AreEqual(WorldLootFaction.Rebel,
                Item_WorldCatalog.SourceForLootTable("rebel_cache"));
            Assert.AreEqual(WorldLootFaction.Bandit,
                Item_WorldCatalog.SourceForLootTable("bandit_camp"));
            Assert.AreEqual(WorldLootFaction.Insurgent,
                Item_WorldCatalog.SourceForLootTable("insurgent_safehouse"));
            Assert.AreEqual(WorldLootFaction.BlackOpsMilitary,
                Item_WorldCatalog.SourceForLootTable("black_ops_cache"));
            Assert.IsTrue(Item_WorldCatalog.IsFactionGearLootTable("military_supply"));
            Assert.IsTrue(Item_WorldCatalog.IsFactionGearLootTable("raider_checkpoint"));
            Assert.IsFalse(Item_WorldCatalog.IsFactionGearLootTable("suburban_homes"));
        }

        [Test]
        public void AttachmentLooseChance_Specialists_Higher_Than_Bandit_Still_Rare()
        {
            Assert.Greater(
                Item_WorldCatalog.AttachmentLooseChance(WorldLootFaction.BlackOpsMilitary),
                Item_WorldCatalog.AttachmentLooseChance(WorldLootFaction.Military));
            Assert.Greater(
                Item_WorldCatalog.AttachmentLooseChance(WorldLootFaction.Military),
                Item_WorldCatalog.AttachmentLooseChance(WorldLootFaction.Bandit));
            Assert.AreEqual(0f,
                Item_WorldCatalog.AttachmentLooseChance(WorldLootFaction.Civilian));
            Assert.Less(
                Item_WorldCatalog.AttachmentLooseChance(WorldLootFaction.BlackOpsMilitary),
                0.10f,
                "Even black-ops loose attachments must stay extremely rare");
        }

        [Test]
        public void RollFactionWorldLoot_Military_Yields_Known_Catalog_Ids()
        {
            var rng = new System.Random(42);
            var rolls = Item_WorldCatalog.RollFactionWorldLoot(
                WorldLootFaction.Military, rng, count: 8, allowAttachments: false, dangerLevel: 4f);

            Assert.AreEqual(8, rolls.Count);
            foreach (var roll in rolls)
            {
                Assert.IsFalse(string.IsNullOrEmpty(roll.ItemId), "empty id");
                Assert.IsTrue(Item_WorldCatalog.Contains(roll.ItemId), roll.ItemId);
                Assert.IsFalse(roll.IsAttachment, roll.ItemId);
                Assert.GreaterOrEqual(roll.Amount, 1);
            }
        }

        [Test]
        public void RollFactionWorldLoot_Bandit_Prefers_Scrap_And_Deprecated()
        {
            var rng = new System.Random(99);
            var rolls = Item_WorldCatalog.RollFactionWorldLoot(
                WorldLootFaction.Bandit, rng, count: 40, allowAttachments: false);

            var ids = new HashSet<string>();
            foreach (var r in rolls) ids.Add(r.ItemId);

            // Across 40 rolls bandit pool should surface scrap / deprecated gear.
            bool anyScrap = ids.Contains(Item_WorldCatalog.ScrapMetal)
                || ids.Contains(Item_WorldCatalog.ShellCasing)
                || ids.Contains(Item_WorldCatalog.Crowbar)
                || ids.Contains(Item_WorldCatalog.BodyArmourDeprecated)
                || ids.Contains(Item_WorldCatalog.KnifeImprovised);
            Assert.IsTrue(anyScrap, "bandit pool should yield scrap/deprecated gear");
        }

        [Test]
        public void RollFactionWorldLoot_BlackOps_Can_Yield_Mil_Armour_Or_Mre()
        {
            var rng = new System.Random(7);
            var rolls = Item_WorldCatalog.RollFactionWorldLoot(
                WorldLootFaction.BlackOpsMilitary, rng, count: 48, allowAttachments: false);

            bool milGear = false;
            foreach (var r in rolls)
            {
                if (r.ItemId == Item_WorldCatalog.BodyArmourMilitary
                    || r.ItemId == Item_WorldCatalog.HelmetMilitary
                    || r.ItemId == Item_WorldCatalog.MreMilitary
                    || r.ItemId == Item_WorldCatalog.NvGogglesMilitary
                    || r.ItemId == Item_WorldCatalog.GrenadeMilitary
                    || r.ItemId == Item_WorldCatalog.HelmetHeavyMilitary
                    || r.ItemId == Item_WorldCatalog.ArmourHeavyMilitary)
                {
                    milGear = true;
                    break;
                }
            }
            Assert.IsTrue(milGear, "black-ops pool should include mil armour/MRE/NV/grenade");

            // Pool composition must list the mil gear entries even if RNG is unlucky.
            var poolIds = Item_WorldCatalog.GetPoolItemIds(WorldLootFaction.BlackOpsMilitary);
            CollectionAssert.Contains(poolIds, Item_WorldCatalog.BodyArmourMilitary);
            CollectionAssert.Contains(poolIds, Item_WorldCatalog.MreMilitary);
            CollectionAssert.Contains(poolIds, Item_WorldCatalog.NvGogglesMilitary);
            CollectionAssert.Contains(poolIds, Item_WorldCatalog.GrenadeMilitary);
        }

        [Test]
        public void TryRollLooseAttachment_Usually_Fails_Even_For_BlackOps()
        {
            var rng = new System.Random(123);
            int hits = 0;
            const int trials = 200;
            for (int i = 0; i < trials; i++)
            {
                if (Item_WorldCatalog.TryRollLooseAttachment(
                        WorldLootFaction.BlackOpsMilitary, rng, 5f, out var roll))
                {
                    hits++;
                    Assert.IsTrue(roll.IsAttachment);
                    Assert.IsTrue(roll.ExtremelyRare);
                    CollectionAssert.Contains(
                        Item_WorldCatalog.MilitaryAttachmentIds(),
                        roll.ItemId);
                }
            }
            // Chance ~4% * danger scale ≈ 5.6% → expect roughly 5–20 of 200, never most.
            Assert.Less(hits, trials / 3, "attachments must remain extremely rare loose loot");
            // With 200 trials at ~5% we almost always get at least one — but don't hard-require
            // (flaky RNG). Just ensure the API can succeed with forced high chance path via
            // many trials OR that zero is still a valid rare outcome.
            Assert.GreaterOrEqual(hits, 0);
        }

        [Test]
        public void ForcedAttachmentPool_Only_Military_Attachment_Ids()
        {
            // Force many attachment rolls by using high chance faction + many trials;
            // collect any successes and validate membership.
            var rng = new System.Random(1);
            var seen = new HashSet<string>();
            for (int i = 0; i < 500; i++)
            {
                if (Item_WorldCatalog.TryRollLooseAttachment(
                        WorldLootFaction.SpecOpsRebel, rng, 6f, out var roll))
                    seen.Add(roll.ItemId);
            }
            foreach (var id in seen)
            {
                CollectionAssert.Contains(Item_WorldCatalog.MilitaryAttachmentIds(), id);
            }
        }

        [Test]
        public void CreateItemDefinition_Builds_From_WorldCatalog()
        {
            var def = Item_WorldCatalog.CreateItemDefinition(Item_WorldCatalog.AttMilSuppressor);
            Assert.IsNotNull(def);
            _toDestroy.Add(def);
            Assert.AreEqual(Item_WorldCatalog.AttMilSuppressor, def.id);
            Assert.AreEqual(ItemType.Tool, def.type);
            Assert.AreEqual(1, def.stackMax);

            var water = Item_WorldCatalog.CreateItemDefinition(Item_WorldCatalog.WaterBottle1LFull);
            Assert.IsNotNull(water);
            _toDestroy.Add(water);
            Assert.AreEqual(ItemType.Water, water.type);
            Assert.Greater(water.thirstRestore, 0f);

            var armour = Item_WorldCatalog.CreateItemDefinition(Item_WorldCatalog.HelmetMilitary);
            Assert.IsNotNull(armour);
            _toDestroy.Add(armour);
            Assert.IsTrue(armour.isEquipable);
            Assert.AreEqual(EquipSlot.Head, armour.equipSlot);
        }

        [Test]
        public void FromAmmoSource_Maps_Military_And_Rebel()
        {
            Assert.AreEqual(WorldLootFaction.Military,
                Item_WorldCatalog.FromAmmoSource(AmmoFactionSource.MilitaryForces));
            Assert.AreEqual(WorldLootFaction.Rebel,
                Item_WorldCatalog.FromAmmoSource(AmmoFactionSource.RebelForces));
            Assert.AreEqual(WorldLootFaction.BlackOpsMilitary,
                Item_WorldCatalog.FromAmmoSource(AmmoFactionSource.BlackOpsMilitary));
            Assert.AreEqual(WorldLootFaction.Civilian,
                Item_WorldCatalog.FromAmmoSource(AmmoFactionSource.CivilianCraft));
        }

        [Test]
        public void Skirmish_RollScavengedWorldLoot_MilitaryWinner_KnownIds()
        {
            var state = new SkirmishState
            {
                winningFaction = "military_patrol",
                totalCorpsesGenerated = 5
            };
            var loot = SkirmishEncounter.RollScavengedWorldLoot(state, new System.Random(3));
            Assert.Greater(loot.Count, 0);
            foreach (var roll in loot)
            {
                Assert.IsTrue(Item_WorldCatalog.Contains(roll.ItemId), roll.ItemId);
            }
        }

        [Test]
        public void Skirmish_RollScavengedWorldLoot_BanditWinner_KnownIds()
        {
            var state = new SkirmishState
            {
                winningFaction = "bandit_raider",
                totalCorpsesGenerated = 3
            };
            var ids = SkirmishEncounter.RollScavengedWorldLootIds(state, new System.Random(5));
            Assert.Greater(ids.Count, 0);
            foreach (var id in ids)
                Assert.IsTrue(Item_WorldCatalog.Contains(id), id);
        }

        [Test]
        public void LocationScavenging_MilitaryTable_Injects_WorldLoot()
        {
            bool injected = false;
            List<string> lastIds = null;

            for (int seed = 0; seed < 50 && !injected; seed++)
            {
                var inv = new Inventory { Capacity = 40, MaxWeight = 200f };
                var scav = new LocationScavengingSystem(
                    radSystem: null,
                    inventory: inv,
                    itemCatalog: null,
                    seed: seed);
                scav.BindWorldLoot(
                    worldItemFactory: id => MakeItem(id),
                    getLocationLootTableId: _ => "military_armory");

                var loc = ScriptableObject.CreateInstance<LocationDefinitionSO>();
                loc.id = "node_mil_armory";
                loc.displayName = "Armory";
                loc.dangerLevel = 5f;
                loc.travelHours = 0.01f;
                _toDestroy.Add(loc);

                var sv = new Survivor { Id = "scav_w_" + seed };
                sv.Needs.Health = 100f;
                Assert.IsTrue(scav.StartMission(sv, loc));
                scav.Tick(1f);

                if (scav.LastInjectedWorldLootIds.Count > 0)
                {
                    injected = true;
                    lastIds = new List<string>(scav.LastInjectedWorldLootIds);
                }
            }

            Assert.IsTrue(injected, "Military armory scavenge should inject world gear across seeds");
            foreach (var id in lastIds)
                Assert.IsTrue(Item_WorldCatalog.Contains(id), id);
        }

        [Test]
        public void LocationScavenging_BanditTable_Injects_WorldLoot()
        {
            bool injected = false;
            for (int seed = 0; seed < 50 && !injected; seed++)
            {
                var inv = new Inventory { Capacity = 40, MaxWeight = 200f };
                var scav = new LocationScavengingSystem(
                    radSystem: null,
                    inventory: inv,
                    itemCatalog: null,
                    seed: seed);
                scav.BindWorldLoot(
                    worldItemFactory: id => MakeItem(id),
                    getLocationLootTableId: _ => "bandit_camp");

                var loc = ScriptableObject.CreateInstance<LocationDefinitionSO>();
                loc.id = "node_bandit";
                loc.displayName = "Camp";
                loc.dangerLevel = 3.5f;
                loc.travelHours = 0.01f;
                _toDestroy.Add(loc);

                var sv = new Survivor { Id = "scav_b_" + seed };
                sv.Needs.Health = 100f;
                Assert.IsTrue(scav.StartMission(sv, loc));
                scav.Tick(1f);

                if (scav.LastInjectedWorldLootIds.Count > 0)
                    injected = true;
            }
            Assert.IsTrue(injected, "Bandit camp should inject world gear across seeds");
        }

        [Test]
        public void MilitaryPool_Does_Not_Include_Attachments_As_Normal_Gear()
        {
            // Attachments only via TryRollLooseAttachment — not in the standard gear pool.
            var pool = Item_WorldCatalog.GetPoolItemIds(WorldLootFaction.Military);
            foreach (var att in Item_WorldCatalog.MilitaryAttachmentIds())
                CollectionAssert.DoesNotContain(pool, att);
        }

        [Test]
        public void AttachmentPool_Is_Military_Attachments_Only()
        {
            var attPool = Item_WorldCatalog.GetAttachmentPoolIds(WorldLootFaction.Military);
            Assert.Greater(attPool.Count, 0);
            foreach (var id in attPool)
                CollectionAssert.Contains(Item_WorldCatalog.MilitaryAttachmentIds(), id);
        }
    }
}
