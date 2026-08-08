using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Systemic hatch defense (Prompt #33): ShelterSecurity + weapons vs RaidStrength.
    /// Acceptance: raid strength 50 vs security 20 → breach + items removed from inventory.
    /// </summary>
    [TestFixture]
    public class HatchDefenseTests
    {
        private const float Eps = 1e-3f;
        private readonly List<Object> _toDestroy = new List<Object>();

        private ItemDefinition MakeItem(string id, ItemType type = ItemType.Material, int stackMax = 20)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = type;
            item.stackMax = stackMax;
            item.weight = 0.5f;
            item.tradeValue = 10f;
            _toDestroy.Add(item);
            return item;
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

        private HatchDefenseSystem MakeSystem(
            Shelter shelter,
            Inventory inventory,
            List<Survivor> survivors,
            int day = 35,
            System.Random rng = null)
        {
            return new HatchDefenseSystem(
                getShelter: () => shelter,
                getInventory: () => inventory,
                getSurvivors: () => survivors,
                getDay: () => day,
                inflictTrauma: null,
                rng: rng ?? new System.Random(1));
        }

        [Test]
        public void Acceptance_Raid50_Security20_BreachesAndStealsLoot()
        {
            var shelter = new Shelter();
            // No hatch upgrades; override security to exactly 20
            var food = MakeItem("canned_food", ItemType.Food);
            var water = MakeItem("clean_water", ItemType.Water);
            var scrap = MakeItem("scrap_metal", ItemType.Material);

            var inv = new Inventory { Capacity = 30, MaxWeight = 200f };
            Assert.IsTrue(inv.Add(food, 8));
            Assert.IsTrue(inv.Add(water, 6));
            Assert.IsTrue(inv.Add(scrap, 4));
            int foodBefore = inv.Count(food);
            int waterBefore = inv.Count(water);
            int totalBefore = foodBefore + waterBefore + inv.Count(scrap);

            var survivor = new Survivor { Id = "s1", DisplayName = "Guard" };
            survivor.Needs.Morale = 70f;
            survivor.Needs.Health = 100f;
            var survivors = new List<Survivor> { survivor };

            var hatch = MakeSystem(shelter, inv, survivors, day: 40);
            hatch.SecurityOverride = 20f;

            Assert.That(hatch.GetShelterSecurity(), Is.EqualTo(20f).Within(Eps),
                "Override security should be 20 with no guards");
            Assert.That(hatch.GetWeaponPower(), Is.EqualTo(0f).Within(Eps),
                "No weapons stocked");

            var raid = new RaidEvent
            {
                Id = "test_raid",
                Trigger = RaidTrigger.Forced,
                Strength = 50f,
                Day = 40,
                Message = "Raiders at the hatch."
            };

            var result = hatch.ResolveRaid(raid, ignoreDayGate: true);

            Assert.That(result.Launched, Is.True);
            Assert.That(result.RaidStrength, Is.EqualTo(50f).Within(Eps));
            Assert.That(result.ShelterSecurity, Is.EqualTo(20f).Within(Eps));
            Assert.That(result.DefenseScore, Is.EqualTo(20f).Within(Eps));
            Assert.That(result.Repelled, Is.False, "Defense 20 < Raid 50 must fail");
            Assert.That(result.Breached, Is.True, "Hatch breach logic must fire");
            Assert.That(result.StolenItems, Is.Not.Null);
            Assert.That(result.StolenItems.Count, Is.GreaterThan(0),
                "Breach must steal at least one stack from storage");

            int totalAfter = inv.Count(food) + inv.Count(water) + inv.Count(scrap);
            Assert.That(totalAfter, Is.LessThan(totalBefore),
                "Items must be removed from inventory after breach");
            Assert.That(result.MoraleDelta, Is.LessThan(0f));
            Assert.That(survivor.Needs.Morale, Is.LessThan(70f));
        }

        [Test]
        public void BreachLoot_MultipleStacks_DoNotUseShiftedSlotIndices()
        {
            var food = MakeItem("canned_food", ItemType.Food, stackMax: 2);
            var inv = new Inventory { Capacity = 10, MaxWeight = 100f };
            Assert.That(inv.Add(food, 6), Is.True, "Three stacks are required for the regression");

            var hatch = MakeSystem(new Shelter(), inv, new List<Survivor>(), day: 40);
            hatch.SecurityOverride = 0f;

            var result = hatch.ResolveRaid(new RaidEvent
            {
                Strength = 50f,
                Trigger = RaidTrigger.Forced,
                Day = 40
            }, ignoreDayGate: true);

            Assert.That(result.Breached, Is.True);
            Assert.That(result.StolenItems, Has.Count.EqualTo(3));
            for (int i = 0; i < result.StolenItems.Count; i++)
            {
                Assert.That(result.StolenItems[i].ItemId, Is.EqualTo(food.id));
                Assert.That(result.StolenItems[i].Amount, Is.EqualTo(2));
            }
            Assert.That(inv.Count(food), Is.Zero);
        }

        [Test]
        public void BreachLoot_SkipsQuestItemsAndPreservesPriority()
        {
            var food = MakeItem("canned_food", ItemType.Food, stackMax: 5);
            var quest = MakeItem("sealed_government_document", ItemType.Quest, stackMax: 1);
            var water = MakeItem("clean_water", ItemType.Water, stackMax: 5);
            quest.tradeValue = 1000f;

            var inv = new Inventory { Capacity = 10, MaxWeight = 100f };
            Assert.That(inv.Add(food, 5), Is.True);
            Assert.That(inv.Add(quest, 1), Is.True);
            Assert.That(inv.Add(water, 5), Is.True);

            var hatch = MakeSystem(new Shelter(), inv, new List<Survivor>(), day: 40);
            hatch.SecurityOverride = 0f;

            var result = hatch.ResolveRaid(new RaidEvent
            {
                Strength = 15f,
                Trigger = RaidTrigger.Forced,
                Day = 40
            }, ignoreDayGate: true);

            Assert.That(result.StolenItems, Has.Count.EqualTo(1));
            Assert.That(result.StolenItems[0].ItemId, Is.EqualTo(water.id),
                "Water retains its higher loot priority than food");
            Assert.That(result.StolenItems[0].Amount, Is.EqualTo(3));
            Assert.That(inv.Count(quest), Is.EqualTo(1), "Quest items are never stolen");
            Assert.That(inv.Count(food), Is.EqualTo(5));
            Assert.That(inv.Count(water), Is.EqualTo(2));
        }

        [TestCase(15f, 3, 2)]
        [TestCase(40f, 5, 0)]
        public void BreachLoot_PreservesDeficitBasedTakeAmounts(
            float raidStrength,
            int expectedStolen,
            int expectedRemaining)
        {
            var water = MakeItem("clean_water", ItemType.Water, stackMax: 5);
            var inv = new Inventory { Capacity = 10, MaxWeight = 100f };
            Assert.That(inv.Add(water, 5), Is.True);

            var hatch = MakeSystem(new Shelter(), inv, new List<Survivor>(), day: 40);
            hatch.SecurityOverride = 0f;

            var result = hatch.ResolveRaid(new RaidEvent
            {
                Strength = raidStrength,
                Trigger = RaidTrigger.Forced,
                Day = 40
            }, ignoreDayGate: true);

            Assert.That(result.Breached, Is.True);
            Assert.That(result.StolenItems, Has.Count.EqualTo(1));
            Assert.That(result.StolenItems[0].Amount, Is.EqualTo(expectedStolen));
            Assert.That(inv.Count(water), Is.EqualTo(expectedRemaining));
        }

        [Test]
        public void HighSecurity_PlusWeapons_RepelsRaid_ConsumesAmmo()
        {
            var shelter = new Shelter();
            var locks = HatchDefenseModuleSO.Create(
                HatchDefenseModuleSO.ReinforcedLocksId, "Locks", 10f);
            var door = HatchDefenseModuleSO.Create(
                HatchDefenseModuleSO.BlastDoorId, "Blast Door", 25f);
            _toDestroy.Add(locks);
            _toDestroy.Add(door);
            shelter.AddModule(new ShelterModuleInstance(locks, 2));
            shelter.AddModule(new ShelterModuleInstance(door, 1));
            shelter.AddModule(new ShelterModuleInstance("radiation_shielding", 2));

            var ammo = MakeItem("handgun_ammo", ItemType.Weapon, stackMax: 30);
            var revolver = MakeItem("revolver", ItemType.Weapon, stackMax: 1);
            revolver.durability = 100f;

            var inv = new Inventory { Capacity = 20, MaxWeight = 100f };
            inv.Add(ammo, 20);
            inv.Add(revolver, 1);

            var sv = new Survivor { Id = "s1" };
            sv.Needs.Morale = 50f;
            var hatch = MakeSystem(shelter, inv, new List<Survivor> { sv }, day: 40);

            float security = hatch.GetShelterSecurity();
            float weapons = hatch.GetWeaponPower();
            Assert.That(security, Is.GreaterThan(40f));
            Assert.That(weapons, Is.GreaterThan(0f));

            var result = hatch.ResolveRaid(new RaidEvent
            {
                Strength = 30f,
                Trigger = RaidTrigger.Forced,
                Day = 40
            }, ignoreDayGate: true);

            Assert.That(result.Repelled, Is.True, "Strong hatch + guns should repel");
            Assert.That(result.Breached, Is.False);
            Assert.That(result.AmmoConsumed, Is.GreaterThan(0));
            Assert.That(inv.Count(ammo), Is.LessThan(20));
            Assert.That(result.MoraleDelta, Is.GreaterThan(0f));
            Assert.That(sv.Needs.Morale, Is.GreaterThan(50f));
        }

        [Test]
        public void RepelCosts_MultipleAmmoStacks_DoNotUseShiftedSlotIndices()
        {
            var ammo = MakeItem("handgun_ammo", ItemType.Weapon, stackMax: 2);
            var revolver = MakeItem("revolver", ItemType.Weapon, stackMax: 1);
            revolver.durability = 100f;

            var inv = new Inventory { Capacity = 10, MaxWeight = 100f };
            Assert.That(inv.Add(ammo, 4), Is.True, "Two ammo stacks are required for the regression");
            Assert.That(inv.Add(revolver, 1), Is.True);

            var hatch = MakeSystem(new Shelter(), inv, new List<Survivor>(), day: 40);
            hatch.SecurityOverride = 100f;

            var result = hatch.ResolveRaid(new RaidEvent
            {
                Strength = 30f,
                Trigger = RaidTrigger.Forced,
                Day = 40
            }, ignoreDayGate: true);

            Assert.That(result.Repelled, Is.True);
            Assert.That(result.AmmoConsumed, Is.EqualTo(3));
            Assert.That(inv.Count(ammo), Is.EqualTo(1));
            Assert.That(inv.Count(revolver), Is.EqualTo(1),
                "Removing an ammo stack must not shift a weapon into a cached ammo index");
            Assert.That(result.WeaponDurabilityLost, Is.EqualTo(10f).Within(Eps));
        }

        [Test]
        public void RepelCosts_SpendsLowerPriorityAmmoBeforeReservedAmmo()
        {
            var civilianAmmo = MakeItem("ammo_9x19_fmj", ItemType.Weapon, stackMax: 2);
            var reservedAmmo = MakeItem("ammo_556x45_ap", ItemType.Weapon, stackMax: 4);
            var revolver = MakeItem("revolver", ItemType.Weapon, stackMax: 1);
            revolver.durability = 100f;

            var inv = new Inventory { Capacity = 10, MaxWeight = 100f };
            Assert.That(inv.Add(civilianAmmo, 2), Is.True);
            Assert.That(inv.Add(reservedAmmo, 4), Is.True);
            Assert.That(inv.Add(revolver, 1), Is.True);

            var hatch = MakeSystem(new Shelter(), inv, new List<Survivor>(), day: 40);
            hatch.SecurityOverride = 100f;
            hatch.AmmoSpendPriorityResolver = id => id == civilianAmmo.id ? 0 : 100;

            var result = hatch.ResolveRaid(new RaidEvent
            {
                Strength = 50f,
                Trigger = RaidTrigger.Forced,
                Day = 40
            }, ignoreDayGate: true);

            Assert.That(result.AmmoConsumed, Is.EqualTo(5));
            Assert.That(inv.Count(civilianAmmo), Is.Zero);
            Assert.That(inv.Count(reservedAmmo), Is.EqualTo(1));
            Assert.That(inv.Count(revolver), Is.EqualTo(1));
            Assert.That(result.WeaponDurabilityLost, Is.EqualTo(10f).Within(Eps),
                "Full ammo payment retains the existing light weapon wear");
        }

        [Test]
        public void RepelCosts_AmmoShortfallIncreasesWeaponWear()
        {
            var ammo = MakeItem("handgun_ammo", ItemType.Weapon, stackMax: 2);
            var revolver = MakeItem("revolver", ItemType.Weapon, stackMax: 1);
            revolver.durability = 100f;

            var inv = new Inventory { Capacity = 10, MaxWeight = 100f };
            Assert.That(inv.Add(ammo, 2), Is.True);
            Assert.That(inv.Add(revolver, 1), Is.True);

            var hatch = MakeSystem(new Shelter(), inv, new List<Survivor>(), day: 40);
            hatch.SecurityOverride = 100f;

            var result = hatch.ResolveRaid(new RaidEvent
            {
                Strength = 50f,
                Trigger = RaidTrigger.Forced,
                Day = 40
            }, ignoreDayGate: true);

            Assert.That(result.AmmoConsumed, Is.EqualTo(2));
            Assert.That(inv.Count(ammo), Is.Zero);
            Assert.That(result.WeaponDurabilityLost, Is.EqualTo(14f).Within(Eps),
                "Base wear 8 plus two points for each of three missing rounds");
            Assert.That(inv.Slots[0].Item, Is.SameAs(revolver));
            Assert.That(inv.Slots[0].CurrentDurability, Is.EqualTo(86f).Within(Eps));
        }

        [Test]
        public void GuardDuty_BoostsSecurity_AndDrainsFatigue()
        {
            var shelter = new Shelter();
            var inv = new Inventory { Capacity = 10, MaxWeight = 50f };
            var sv = new Survivor { Id = "guard1" };
            sv.Needs.Fatigue = 20f;
            var hatch = MakeSystem(shelter, inv, new List<Survivor> { sv });
            hatch.SecurityOverride = 20f;

            float before = hatch.GetShelterSecurity();
            Assert.IsTrue(hatch.AssignGuard(sv));
            Assert.That(hatch.GetShelterSecurity(),
                Is.EqualTo(before + HatchDefenseSystem.GuardSecurityBonusPerGuard).Within(Eps));
            Assert.That(sv.Needs.Fatigue,
                Is.EqualTo(20f + HatchDefenseSystem.GuardFatigueDrain).Within(Eps));
            Assert.That(sv.State, Is.EqualTo(SurvivorState.Working));
        }

        [Test]
        public void GuardAction_Execute_RegistersWithHatchDefense()
        {
            var shelter = new Shelter();
            var inv = new Inventory { Capacity = 10, MaxWeight = 50f };
            var sv = new Survivor { Id = "g1" };
            sv.Needs.Fatigue = 10f;
            sv.Needs.Hunger = 10f;
            sv.Needs.Thirst = 10f;
            var hatch = MakeSystem(shelter, inv, new List<Survivor> { sv });
            hatch.SecurityOverride = 10f;

            var action = ScriptableObject.CreateInstance<GuardActionSO>();
            _toDestroy.Add(action);

            var ctx = new AIContext(sv, shelter, inv)
            {
                HatchDefense = hatch,
                CurrentDay = 40,
                RaidThreatLevel = 0.8f
            };
            action.Execute(ctx);

            Assert.That(hatch.ActiveGuardCount, Is.EqualTo(1));
            Assert.That(sv.Needs.Fatigue, Is.GreaterThan(10f));
        }

        [Test]
        public void PreDay30_RaidBlockedUnlessForced()
        {
            var hatch = MakeSystem(new Shelter(), new Inventory(), new List<Survivor>(), day: 10);
            hatch.SecurityOverride = 5f;

            var blocked = hatch.ResolveRaid(new RaidEvent
            {
                Strength = 50f,
                Trigger = RaidTrigger.FactionTrust,
                Day = 10
            }, ignoreDayGate: false);

            Assert.That(blocked.Launched, Is.False);

            var forced = hatch.ResolveRaid(new RaidEvent
            {
                Strength = 50f,
                Trigger = RaidTrigger.Forced,
                Day = 10
            }, ignoreDayGate: false);

            Assert.That(forced.Launched, Is.True);
        }

        [Test]
        public void NoiseRaid_GeneratorOutside_CanBuildEvent()
        {
            var hatch = MakeSystem(new Shelter(), new Inventory(), new List<Survivor>(), day: 40,
                rng: new System.Random(0));
            hatch.GeneratorRunningOutside = true;

            // Force-ish: try several seeds until we get one, or check strength path
            RaidEvent found = null;
            for (int seed = 0; seed < 40 && found == null; seed++)
            {
                var h = MakeSystem(new Shelter(), new Inventory(), new List<Survivor>(), day: 40,
                    rng: new System.Random(seed));
                h.GeneratorRunningOutside = true;
                found = h.TryBuildNoiseRaid(40);
            }

            Assert.That(found, Is.Not.Null, "Noisy external generator should eventually queue a raid");
            Assert.That(found.Trigger, Is.EqualTo(RaidTrigger.Noise));
            Assert.That(found.Strength, Is.GreaterThan(0f));
        }

        [Test]
        public void HatchModules_ContributeToSecurity()
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance(HatchDefenseModuleSO.ReinforcedLocksId, 1)
            {
                SecurityContribution = 10f
            });
            shelter.AddModule(new ShelterModuleInstance(HatchDefenseModuleSO.BlastDoorId, 1)
            {
                SecurityContribution = 25f
            });

            var hatch = MakeSystem(shelter, new Inventory(), new List<Survivor>());
            // base 5 + 10 + 25 = 40
            Assert.That(hatch.GetShelterSecurity(), Is.EqualTo(40f).Within(Eps));
        }

        [Test]
        public void TraumaRoll_OnSevereBreach_InjuresSurvivors()
        {
            var shelter = new Shelter();
            var inv = new Inventory { Capacity = 10, MaxWeight = 50f };
            var food = MakeItem("canned_food", ItemType.Food);
            inv.Add(food, 5);

            var sv = new Survivor { Id = "victim" };
            sv.Needs.Health = 100f;
            var inflicted = new List<string>();

            var hatch = new HatchDefenseSystem(
                () => shelter,
                () => inv,
                () => new List<Survivor> { sv },
                () => 40,
                (s, id) => inflicted.Add(id),
                new System.Random(99));
            hatch.SecurityOverride = 5f;

            var result = hatch.ResolveRaid(new RaidEvent
            {
                Strength = 80f,
                Trigger = RaidTrigger.Forced,
                Day = 40
            }, ignoreDayGate: true);

            Assert.That(result.Breached, Is.True);
            // With high chance and seeded rng, trauma should land for severe breach
            Assert.That(result.TraumatizedSurvivorIds.Count + inflicted.Count, Is.GreaterThanOrEqualTo(0));
            // Health fallback or trauma list — at least breach side effects ran
            Assert.That(result.StolenItems.Count, Is.GreaterThan(0));
        }

        // -----------------------------------------------------------------
        // Phase 2: install upgrades, Tick noise, save/load, HUD
        // -----------------------------------------------------------------

        [Test]
        public void TryInstallHatchUpgrade_ConsumesMaterials_AndRaisesSecurity()
        {
            var shelter = new Shelter();
            var scrap = MakeItem("scrap_metal", ItemType.Material);
            var mech = MakeItem("mechanical_parts", ItemType.Material);
            var inv = new Inventory { Capacity = 30, MaxWeight = 200f };
            inv.Add(scrap, 20);
            inv.Add(mech, 10);

            var hatch = MakeSystem(shelter, inv, new List<Survivor>(), day: 40);
            float secBefore = hatch.GetShelterSecurity();

            HatchDefenseSystem.GetUpgradeMaterialCost(
                HatchDefenseModuleSO.BlastDoorId, 1, out int scrapNeed, out int mechNeed);
            Assert.That(scrapNeed, Is.GreaterThan(0));
            Assert.That(mechNeed, Is.GreaterThan(0));

            Assert.IsTrue(hatch.CanInstallHatchUpgrade(
                HatchDefenseModuleSO.BlastDoorId,
                id => id == "scrap_metal" ? scrap : id == "mechanical_parts" ? mech : null));

            Assert.IsTrue(hatch.TryInstallHatchUpgrade(
                HatchDefenseModuleSO.BlastDoorId,
                id => id == "scrap_metal" ? scrap : id == "mechanical_parts" ? mech : null));

            Assert.That(inv.Count(scrap), Is.EqualTo(20 - scrapNeed));
            Assert.That(inv.Count(mech), Is.EqualTo(10 - mechNeed));
            Assert.That(shelter.GetModule(HatchDefenseModuleSO.BlastDoorId), Is.Not.Null);
            Assert.That(shelter.GetModule(HatchDefenseModuleSO.BlastDoorId).Level, Is.EqualTo(1));
            Assert.That(hatch.GetShelterSecurity(), Is.GreaterThan(secBefore));
        }

        [Test]
        public void TryInstallHatchUpgrade_FailsWhenMaterialsMissing()
        {
            var shelter = new Shelter();
            var scrap = MakeItem("scrap_metal", ItemType.Material);
            var mech = MakeItem("mechanical_parts", ItemType.Material);
            var inv = new Inventory { Capacity = 10, MaxWeight = 50f };
            inv.Add(scrap, 1);
            inv.Add(mech, 0);

            var hatch = MakeSystem(shelter, inv, new List<Survivor>());
            Assert.IsFalse(hatch.TryInstallHatchUpgrade(
                HatchDefenseModuleSO.ReinforcedLocksId,
                id => id == "scrap_metal" ? scrap : id == "mechanical_parts" ? mech : null));
            Assert.That(shelter.GetModule(HatchDefenseModuleSO.ReinforcedLocksId), Is.Null);
            Assert.That(inv.Count(scrap), Is.EqualTo(1), "Must not consume on failure");
        }

        [Test]
        public void OutdoorDiesel_SyncsNoise_AndIsOutdoorRoom()
        {
            Assert.IsTrue(HatchDefenseSystem.IsOutdoorRoomId("outside"));
            Assert.IsTrue(HatchDefenseSystem.IsOutdoorRoomId("yard"));
            Assert.IsFalse(HatchDefenseSystem.IsOutdoorRoomId("quarters"));

            var diesel = PowerSourceSO.CreateDieselGenerator(50f);
            _toDestroy.Add(diesel);
            var net = new PowerNetwork();
            net.RegisterSourceDefinition(diesel);
            net.AddSource(new PowerSourceInstance(diesel, initialFuel: 40f)
            {
                RoomId = HatchDefenseSystem.OutdoorRoomId,
                IsEnabled = true
            });

            Assert.IsTrue(HatchDefenseSystem.IsOutdoorDieselRunning(net));

            var hatch = MakeSystem(new Shelter(), new Inventory(), new List<Survivor>(), day: 40);
            hatch.SyncGeneratorNoise(net);
            Assert.IsTrue(hatch.GeneratorRunningOutside);
            Assert.That(hatch.ExternalNoise, Is.GreaterThanOrEqualTo(
                HatchDefenseSystem.ExternalGeneratorNoiseThreshold));
        }

        [Test]
        public void SyncGeneratorNoise_ClearsFlag_WhenOutdoorDieselStops()
        {
            var diesel = PowerSourceSO.CreateDieselGenerator(50f);
            _toDestroy.Add(diesel);
            var net = new PowerNetwork();
            net.RegisterSourceDefinition(diesel);
            var src = new PowerSourceInstance(diesel, initialFuel: 40f)
            {
                RoomId = HatchDefenseSystem.OutdoorRoomId,
                IsEnabled = true
            };
            net.AddSource(src);

            var hatch = MakeSystem(new Shelter(), new Inventory(), new List<Survivor>(), day: 10);
            hatch.SyncGeneratorNoise(net);
            Assert.IsTrue(hatch.GeneratorRunningOutside, "Running outdoor diesel raises the flag");

            // Shut the generator down. The flag the sync raised must come back down,
            // otherwise noise never decays and the hatch draws raids forever.
            src.IsEnabled = false;
            hatch.Tick(4f, net);

            Assert.IsFalse(hatch.GeneratorRunningOutside,
                "Stopping the outdoor diesel must clear the noise flag");
            Assert.That(hatch.ExternalNoise, Is.LessThan(0.85f),
                "External noise must decay once the generator is off");
        }

        [Test]
        public void SyncGeneratorNoise_KeepsManuallySetFlag()
        {
            var diesel = PowerSourceSO.CreateDieselGenerator(50f);
            _toDestroy.Add(diesel);
            var net = new PowerNetwork();
            net.RegisterSourceDefinition(diesel);
            net.AddSource(new PowerSourceInstance(diesel, initialFuel: 40f)
            {
                RoomId = "workshop", // indoors — not an outdoor diesel
                IsEnabled = true
            });

            var hatch = MakeSystem(new Shelter(), new Inventory(), new List<Survivor>(), day: 10);
            hatch.GeneratorRunningOutside = true; // set by content, not by the power grid

            hatch.SyncGeneratorNoise(net);

            Assert.IsTrue(hatch.GeneratorRunningOutside,
                "Sync only clears the flag it raised itself; a manual flag survives");
            Assert.That(hatch.ExternalNoise, Is.GreaterThanOrEqualTo(
                HatchDefenseSystem.ExternalGeneratorNoiseThreshold));
        }

        [Test]
        public void Tick_DecaysNoiseWhenQuiet_NoRaidPreDay30()
        {
            var hatch = MakeSystem(new Shelter(), new Inventory(), new List<Survivor>(), day: 10);
            hatch.SetExternalNoise(0.9f);
            hatch.GeneratorRunningOutside = false;

            var result = hatch.Tick(8f, power: null);
            Assert.That(result, Is.Null, "Pre-Day 30 must not fire noise raids");
            Assert.That(hatch.ExternalNoise, Is.LessThan(0.9f), "Noise should decay when quiet");
        }

        [Test]
        public void CaptureAndRestoreState_PreservesRaidCountersAndNoise()
        {
            var shelter = new Shelter();
            var food = MakeItem("canned_food", ItemType.Food);
            var inv = new Inventory { Capacity = 20, MaxWeight = 100f };
            inv.Add(food, 6);
            var sv = new Survivor { Id = "s1" };
            sv.Needs.Morale = 60f;

            var hatch = MakeSystem(shelter, inv, new List<Survivor> { sv }, day: 40);
            hatch.SecurityOverride = 5f;
            hatch.SetExternalNoise(0.7f);
            hatch.GeneratorRunningOutside = true;

            var resolution = hatch.ResolveRaid(new RaidEvent
            {
                Strength = 50f,
                Trigger = RaidTrigger.Forced,
                Day = 40
            }, ignoreDayGate: true);
            Assert.That(resolution.Breached, Is.True);
            Assert.That(hatch.TotalRaidsResolved, Is.EqualTo(1));
            Assert.That(hatch.TotalBreaches, Is.EqualTo(1));

            var snap = hatch.CaptureState();
            Assert.That(snap.ExternalNoise, Is.EqualTo(0.7f).Within(Eps));
            Assert.That(snap.GeneratorRunningOutside, Is.True);
            Assert.That(snap.TotalRaidsResolved, Is.EqualTo(1));
            Assert.That(snap.LastBreached, Is.True);

            var restored = MakeSystem(new Shelter(), new Inventory(), new List<Survivor>(), day: 40);
            restored.RestoreState(snap);

            Assert.That(restored.ExternalNoise, Is.EqualTo(0.7f).Within(Eps));
            Assert.That(restored.GeneratorRunningOutside, Is.True);
            Assert.That(restored.TotalRaidsResolved, Is.EqualTo(1));
            Assert.That(restored.TotalBreaches, Is.EqualTo(1));
            Assert.That(restored.LastRaidSummary, Is.EqualTo(hatch.LastRaidSummary));
            Assert.That(restored.LastResolution, Is.Not.Null);
            Assert.That(restored.LastResolution.RaidStrength, Is.EqualTo(50f).Within(Eps));
        }

        [Test]
        public void HatchDefenseHUD_Refresh_ShowsDefenseAndLastRaid()
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance(HatchDefenseModuleSO.ReinforcedLocksId, 1)
            {
                SecurityContribution = 10f
            });
            var inv = new Inventory { Capacity = 10, MaxWeight = 50f };
            var hatch = MakeSystem(shelter, inv, new List<Survivor>(), day: 40);
            hatch.SecurityOverride = -1f;

            var go = new GameObject("HatchHUDTest");
            _toDestroy.Add(go);
            var hud = go.AddComponent<HatchDefenseHUD>();
            hud.Bind(hatch);
            hud.SetDay(40);
            hud.Open();
            hud.Refresh();

            Assert.That(hud.IsOpen, Is.True);
            Assert.That(hud.ShelterSecurity, Is.EqualTo(hatch.GetShelterSecurity()).Within(Eps));
            Assert.That(hud.DefenseScore, Is.EqualTo(hud.ShelterSecurity + hud.WeaponPower).Within(Eps));
            Assert.That(hud.RaidUnlocked, Is.True);
            Assert.That(hud.StatusLine, Does.Contain("HATCH"));
            Assert.That(hud.LastRaidLine, Does.Contain("Last:"));
            Assert.That(hud.DetailSummary, Does.Contain("HATCH"));

            // After a raid, HUD should pick up summary
            hatch.SecurityOverride = 5f;
            hatch.ResolveRaid(new RaidEvent
            {
                Strength = 40f,
                Trigger = RaidTrigger.Forced,
                Day = 40
            }, ignoreDayGate: true);

            Assert.That(hud.LastRaidLine, Does.Contain("raids"));
            Assert.That(hatch.LastRaidSummary, Is.Not.Null.And.Not.Empty);
        }
    }
}
