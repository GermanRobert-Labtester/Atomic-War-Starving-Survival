using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;

using AtomicWar._Game.Encounters;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// ResolveHit + faction loot wiring: hatch AP vs JHP, skirmish exclusives,
    /// military loot rolls, workbench civilian-only craft gate.
    /// </summary>
    [TestFixture]
    public class ItemAmmoTypesWiringTests
    {
        private const float Eps = 0.01f;
        private readonly List<Object> _toDestroy = new List<Object>();
        private Item_AmmoTypes _ammo;

        [SetUp]
        public void SetUp()
        {
            _ammo = new Item_AmmoTypes();
        }

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

        private ItemDefinition MakeItem(string id, ItemType type = ItemType.Weapon, int stackMax = 99)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = type;
            item.stackMax = stackMax;
            item.weight = 0.05f;
            item.tradeValue = 2f;
            _toDestroy.Add(item);
            return item;
        }

        // ── ResolveHit combat axes ──────────────────────────────────────

        [Test]
        public void ResolveHit_ApBeatsJhp_OnMilitaryArmor()
        {
            float armor = Item_AmmoTypes.ArmorMilitary;
            Assert.IsTrue(Item_AmmoTypes.TryGetLoad("ammo_556x45_ap", out var ap));
            Assert.IsTrue(Item_AmmoTypes.TryGetLoad("ammo_556x45_jhp", out var jhp)
                || Item_AmmoTypes.TryGetLoad("ammo_9x19_jhp", out jhp));

            var apHit = _ammo.ResolveHit(ap.Id, ap.BaseDamage, armor);
            var softHit = _ammo.ResolveHit(jhp.Id, jhp.BaseDamage, armor);

            Assert.Greater(apHit.FinalDamage, softHit.FinalDamage,
                "AP should out-damage hollow point against military armor");
            Assert.IsTrue(softHit.ArmorPenaltyApplied,
                "JHP should take armor penalty against military plate");
            Assert.Greater(
                apHit.FinalDamage / Mathf.Max(1f, ap.BaseDamage),
                softHit.FinalDamage / Mathf.Max(1f, jhp.BaseDamage));
        }

        [Test]
        public void HatchDefense_ApStockpile_BeatsJhp_VsMilitaryRaidArmor()
        {
            var ap = MakeItem("ammo_556x45_ap", stackMax: 50);
            var jhp = MakeItem("ammo_9x19_jhp", stackMax: 50);

            var invAp = new Inventory { Capacity = 20, MaxWeight = 100f };
            invAp.Add(ap, 20);
            var invJhp = new Inventory { Capacity = 20, MaxWeight = 100f };
            invJhp.Add(jhp, 20);

            var hatchAp = new HatchDefenseSystem(
                getShelter: () => new Shelter(),
                getInventory: () => invAp,
                getSurvivors: () => new List<Survivor>(),
                getDay: () => 40);
            var hatchJhp = new HatchDefenseSystem(
                getShelter: () => new Shelter(),
                getInventory: () => invJhp,
                getSurvivors: () => new List<Survivor>(),
                getDay: () => 40);

            hatchAp.AmmoDefensePowerResolver =
                (id, amt, armor) => _ammo.GetAmmoStockpileDefensePower(id, amt, armor);
            hatchJhp.AmmoDefensePowerResolver =
                (id, amt, armor) => _ammo.GetAmmoStockpileDefensePower(id, amt, armor);

            float milArmor = Item_AmmoTypes.GetFactionArmor("military_patrol");
            float apPower = hatchAp.GetWeaponPower(invAp, milArmor);
            float jhpPower = hatchJhp.GetWeaponPower(invJhp, milArmor);

            Assert.Greater(apPower, jhpPower,
                "AP stockpile should contribute more hatch power vs military armor than JHP");
        }

        [Test]
        public void HatchDefense_SpendsCivilianAmmo_BeforeAp()
        {
            var civilian = MakeItem("ammo_9x19_fmj", stackMax: 50);
            var ap = MakeItem("ammo_556x45_ap", stackMax: 50);
            var inv = new Inventory { Capacity = 20, MaxWeight = 100f };
            inv.Add(civilian, 15);
            inv.Add(ap, 15);

            var hatch = new HatchDefenseSystem(
                getShelter: () => new Shelter(),
                getInventory: () => inv,
                getSurvivors: () => new List<Survivor>(),
                getDay: () => 40);
            hatch.SecurityOverride = 80f;
            hatch.AmmoSpendPriorityResolver = Item_AmmoTypes.AmmoSpendPriority;
            hatch.FactionArmorResolver = Item_AmmoTypes.GetFactionArmor;
            hatch.AmmoDefensePowerResolver =
                (id, amt, armor) => _ammo.GetAmmoStockpileDefensePower(id, amt, armor);

            var result = hatch.ResolveRaid(new RaidEvent
            {
                Strength = 20f,
                Trigger = RaidTrigger.Forced,
                Day = 40,
                FactionId = "military_patrol"
            }, ignoreDayGate: true);

            Assert.IsTrue(result.Repelled, "High security should repel");
            Assert.Greater(result.AmmoConsumed, 0);
            // Civilian should be burned first — AP stack untouched until civilian gone.
            Assert.Less(inv.Count(civilian), 15, "Civilian FMJ spent first");
            Assert.AreEqual(15, inv.Count(ap), "AP exclusives preserved while civilian remains");
        }

        // ── Faction loot tables ─────────────────────────────────────────

        [Test]
        public void RollFactionAmmoLoot_Military_OnlyExclusives()
        {
            var rng = new System.Random(7);
            var rolls = Item_AmmoTypes.RollFactionAmmoLoot(
                AmmoFactionSource.MilitaryForces, rng, count: 8, preferApApi: true);

            Assert.AreEqual(8, rolls.Count);
            foreach (var id in rolls)
            {
                Assert.IsTrue(Item_AmmoTypes.TryGetLoad(id, out var load), id);
                Assert.IsFalse(load.Craftable, id + " must be non-craftable exclusive");
                Assert.IsTrue(
                    load.Modification == BulletModification.Ap
                    || load.Modification == BulletModification.Api
                    || load.Modification == BulletModification.M855A1
                    || load.WeaponClass == WeaponAmmoClass.BattleRifle
                    || load.WeaponClass == WeaponAmmoClass.Sniper
                    || load.WeaponClass == WeaponAmmoClass.AntiMateriel
                    || Item_AmmoTypes.IsExclusiveToFactions(id),
                    id + " should be military exclusive load");
            }
        }

        [Test]
        public void RollFactionAmmoLoot_Civilian_OnlyCraftable()
        {
            var rng = new System.Random(11);
            var rolls = Item_AmmoTypes.RollFactionAmmoLoot(
                AmmoFactionSource.CivilianCraft, rng, count: 8, preferApApi: false);

            Assert.AreEqual(8, rolls.Count);
            foreach (var id in rolls)
            {
                Assert.IsTrue(Item_AmmoTypes.IsCraftable(id), id);
                Assert.IsFalse(Item_AmmoTypes.IsExclusiveToFactions(id), id);
            }
        }

        [Test]
        public void IsMilitaryLootTable_RecognizesArmoryAndGroundZero()
        {
            Assert.IsTrue(Item_AmmoTypes.IsMilitaryLootTable("military_armory"));
            Assert.IsTrue(Item_AmmoTypes.IsMilitaryLootTable("loot_ground_zero"));
            Assert.IsTrue(Item_AmmoTypes.IsMilitaryLootTable("mil_cache"));
            Assert.IsFalse(Item_AmmoTypes.IsMilitaryLootTable("suburban_homes"));
            Assert.IsFalse(Item_AmmoTypes.IsMilitaryLootTable(null));
        }

        [Test]
        public void LocationScavenging_MilitaryTable_InjectsExclusiveAmmo()
        {
            bool injected = false;
            List<string> lastIds = null;

            for (int seed = 0; seed < 40 && !injected; seed++)
            {
                var inv = new Inventory { Capacity = 40, MaxWeight = 200f };
                var scav = new LocationScavengingSystem(
                    radSystem: null,
                    inventory: inv,
                    itemCatalog: null,
                    seed: seed);
                scav.BindAmmoTypes(
                    _ammo,
                    ammoItemFactory: id => MakeItem(id),
                    getLocationLootTableId: _ => "military_armory");

                var loc = ScriptableObject.CreateInstance<LocationDefinitionSO>();
                loc.id = "node_mil_armory";
                loc.displayName = "Armory";
                loc.dangerLevel = 5f;
                loc.travelHours = 0.01f;
                _toDestroy.Add(loc);

                var sv = new Survivor { Id = "scav_" + seed };
                sv.Needs.Health = 100f;
                Assert.IsTrue(scav.StartMission(sv, loc));
                scav.Tick(1f);

                if (scav.LastInjectedAmmoIds.Count > 0)
                {
                    injected = true;
                    lastIds = new List<string>(scav.LastInjectedAmmoIds);
                }
            }

            Assert.IsTrue(injected, "Military armory scavenge should inject exclusive ammo across seeds");
            foreach (var id in lastIds)
            {
                Assert.IsTrue(Item_AmmoTypes.TryGetLoad(id, out var load), id);
                Assert.IsFalse(load.Craftable, id);
            }
        }

        // ── Skirmish rewards + hit chance ───────────────────────────────

        [Test]
        public void Skirmish_MilVsRebel_RewardsAreNonCraftableExclusives()
        {
            var sk = new Skirmish_Mil_vs_Rebel("crossroads");
            var mil = sk.InterveneForMilitary(out float rebelDelta);
            Assert.Less(rebelDelta, 0f);
            Assert.Greater(mil.Count, 0);
            foreach (var id in mil)
            {
                Assert.IsFalse(Item_AmmoTypes.IsCraftable(id), id);
                Assert.IsTrue(Item_AmmoTypes.IsExclusiveToFactions(id)
                    || id.Contains("_ap") || id.Contains("m855a1"), id);
            }

            var sk2 = new Skirmish_Mil_vs_Rebel("crossroads_b");
            var rebel = sk2.InterveneForRebels(out float milDelta);
            Assert.Less(milDelta, 0f);
            foreach (var id in rebel)
            {
                Assert.IsFalse(Item_AmmoTypes.IsCraftable(id), id);
            }
        }

        [Test]
        public void Skirmish_HitChance_MilitaryAp_BeatsRaiderFmj_VsArmor()
        {
            float milOnMil = SkirmishEncounter.ComputeSkirmishHitChance(
                "military_forces", "military_forces", _ammo);
            float raiderOnMil = SkirmishEncounter.ComputeSkirmishHitChance(
                "bandit_raider", "military_forces", _ammo);

            Assert.Greater(milOnMil, raiderOnMil,
                "Military AP baseline should hit armored targets more often than raider FMJ");
        }

        [Test]
        public void Skirmish_ScavengedAmmo_MilitaryWinner_DropsExclusives()
        {
            var state = new SkirmishState
            {
                winningFaction = "military_patrol",
                totalCorpsesGenerated = 5
            };
            var loot = SkirmishEncounter.RollScavengedAmmo(state, new System.Random(3));
            Assert.Greater(loot.Count, 0);
            foreach (var id in loot)
            {
                Assert.IsTrue(Item_AmmoTypes.TryGetLoad(id, out var load), id);
                Assert.IsFalse(load.Craftable, id);
            }
        }

        // ── Workbench craft gate ────────────────────────────────────────

        [Test]
        public void WorkbenchCraftGate_BlocksMilitary_AllowsCivilian()
        {
            Assert.IsTrue(Item_AmmoTypes.IsWorkbenchCraftAllowed("ammo_9x19_fmj"));
            Assert.IsTrue(Item_AmmoTypes.IsWorkbenchCraftAllowed("ammo_12ga_buck"));
            Assert.IsTrue(Item_AmmoTypes.IsWorkbenchCraftAllowed("scrap_metal")); // non-ammo
            Assert.IsFalse(Item_AmmoTypes.IsWorkbenchCraftAllowed("ammo_556x45_ap"));
            Assert.IsFalse(Item_AmmoTypes.IsWorkbenchCraftAllowed("ammo_556x45_m855a1"));
            Assert.IsFalse(Item_AmmoTypes.IsWorkbenchCraftAllowed("ammo_762x51_fmj")); // battle rifle
            Assert.IsFalse(Item_AmmoTypes.IsWorkbenchCraftAllowed("ammo_50bmg_ap"));
        }

        [Test]
        public void CraftingSystem_Gate_RejectsApRecipe()
        {
            var inv = new Inventory { Capacity = 30, MaxWeight = 200f };
            var lead = MakeItem("lead_scrap", ItemType.Material);
            var powder = MakeItem("gunpowder", ItemType.Material);
            inv.Add(lead, 20);
            inv.Add(powder, 20);

            var craft = new CraftingSystem(inv);
            craft.AddStation(new CraftingStation
            {
                id = "workbench",
                displayName = "Workbench",
                Condition = 100f
            });
            craft.BindCraftResultGate(Item_AmmoTypes.IsWorkbenchCraftAllowed);

            var apResult = MakeItem("ammo_556x45_ap");
            var apRecipe = ScriptableObject.CreateInstance<Recipe>();
            apRecipe.id = "craft_ap_illegal";
            apRecipe.result = apResult;
            apRecipe.resultAmount = 5;
            apRecipe.requiredStationId = "workbench";
            apRecipe.ingredients = new List<Ingredient>
            {
                new Ingredient { item = lead, amount = 1 },
                new Ingredient { item = powder, amount = 1 }
            };
            _toDestroy.Add(apRecipe);

            Assert.IsFalse(craft.CanCraft(apRecipe), "AP must not craft at workbench");
            Assert.IsFalse(craft.StartCraft(apRecipe));

            var civResult = MakeItem("ammo_9x19_fmj");
            var civRecipe = ScriptableObject.CreateInstance<Recipe>();
            civRecipe.id = "craft_9mm_fmj";
            civRecipe.result = civResult;
            civRecipe.resultAmount = 5;
            civRecipe.requiredStationId = "workbench";
            civRecipe.ingredients = new List<Ingredient>
            {
                new Ingredient { item = lead, amount = 1 },
                new Ingredient { item = powder, amount = 1 }
            };
            _toDestroy.Add(civRecipe);

            Assert.IsTrue(craft.CanCraft(civRecipe), "Civilian FMJ is workbench-legal");
        }

        [Test]
        public void FactionArmor_MilitaryHigherThanRaider()
        {
            Assert.Greater(
                Item_AmmoTypes.GetFactionArmor("military_forces"),
                Item_AmmoTypes.GetFactionArmor("bandit_raider"));
            Assert.Greater(
                Item_AmmoTypes.GetFactionArmor("black_ops_team"),
                Item_AmmoTypes.GetFactionArmor("military_forces"));
            Assert.AreEqual(
                Item_AmmoTypes.ArmorMilitary,
                Item_AmmoTypes.InferEncounterArmor("warlord_checkpoint"));
        }

        [Test]
        public void AmmoSpendPriority_CivilianBeforeAp()
        {
            Assert.Less(
                Item_AmmoTypes.AmmoSpendPriority("ammo_9x19_fmj"),
                Item_AmmoTypes.AmmoSpendPriority("ammo_556x45_ap"));
            Assert.Less(
                Item_AmmoTypes.AmmoSpendPriority("ammo_9x19_jhp"),
                Item_AmmoTypes.AmmoSpendPriority("ammo_50bmg_api"));
        }
    }
}
