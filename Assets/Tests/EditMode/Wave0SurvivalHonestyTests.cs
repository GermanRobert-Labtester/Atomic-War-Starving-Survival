using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Core;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Wave 0 — first-hour survival loop matches domain promise.
    ///
    /// Each test pins a single audit item that was OPEN as of the Part 4 second-pass
    /// re-verification. The fixes are small (sign flip, route through a method, all-or-
    /// nothing preflight) but the domain effects are large: food actually feeds, a
    /// starving survivor actually dies, anti-rad actually goes through tolerance, a
    /// full bag no longer eats equipped gear, and a real endgame condition actually
    /// freezes the run.
    /// </summary>
    [TestFixture]
    public class Wave0SurvivalHonestyTests
    {
        private const float Eps = 1e-3f;

        private static ItemDefinition MakeItem(
            string id,
            float hungerRestore = 0f,
            float thirstRestore = 0f,
            float healthEffect = 0f,
            float radCleanse = 0f,
            ItemType type = ItemType.Food,
            int stackMax = 20)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = type;
            item.stackMax = stackMax;
            item.weight = 0.1f;
            item.hungerRestore = hungerRestore;
            item.thirstRestore = thirstRestore;
            item.healthEffect = healthEffect;
            item.radCleanse = radCleanse;
            return item;
        }

        private static Survivor MakeSurvivor(string id = "sv1")
        {
            return new Survivor { Id = id, DisplayName = id };
        }

        // -----------------------------------------------------------------
        // DEEP3-INV-001 — Inventory.Consume hunger/thirst sign
        // -----------------------------------------------------------------

        [Test]
        public void Consume_HungrySurvivor_EatingFood_ReducesHunger()
        {
            var survivor = MakeSurvivor();
            survivor.Needs.Hunger = 100f; // critical

            var food = MakeItem("canned_beans", hungerRestore: 40f);
            var inv = new Inventory { Capacity = 10, MaxWeight = 50f };
            inv.Add(food, 1);

            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile);
            var rad = new RadiationSystem(needs);

            bool ok = inv.Consume(food, survivor, rad, needs, 1f);
            Assert.IsTrue(ok);
            // Hunger 100 - 40 = 60; previously this was 100 + 40 (clamped to 100, no effect).
            Assert.That(survivor.Needs.Hunger, Is.EqualTo(60f).Within(Eps),
                "Consume must subtract hungerRestore from Hunger (Hunger high = hungry).");

            Object.DestroyImmediate(food);
            Object.DestroyImmediate(profile);
        }

        [Test]
        public void Consume_ThirstySurvivor_DrinkingWater_ReducesThirst()
        {
            var survivor = MakeSurvivor();
            survivor.Needs.Thirst = 100f;

            var water = MakeItem("clean_water", thirstRestore: 50f, type: ItemType.Food);
            var inv = new Inventory { Capacity = 10, MaxWeight = 50f };
            inv.Add(water, 1);

            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile);
            var rad = new RadiationSystem(needs);

            bool ok = inv.Consume(water, survivor, rad, needs, 1f);
            Assert.IsTrue(ok);
            Assert.That(survivor.Needs.Thirst, Is.EqualTo(50f).Within(Eps));

            Object.DestroyImmediate(water);
            Object.DestroyImmediate(profile);
        }

        [Test]
        public void Consume_DecrementsStack_OnSuccess()
        {
            var survivor = MakeSurvivor();
            var food = MakeItem("canned_beans", hungerRestore: 40f);
            var inv = new Inventory { Capacity = 10, MaxWeight = 50f };
            inv.Add(food, 3);

            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile);
            var rad = new RadiationSystem(needs);

            inv.Consume(food, survivor, rad, needs, 1f);
            Assert.AreEqual(2, inv.Count(food), "Consume must remove exactly one from the stack.");

            Object.DestroyImmediate(food);
            Object.DestroyImmediate(profile);
        }

        // -----------------------------------------------------------------
        // INV-001 — Inventory.Add is all-or-nothing
        // -----------------------------------------------------------------

        [Test]
        public void Add_OverCapacity_ReturnsFalse_AndFiresNoEvents()
        {
            var food = MakeItem("canned_beans", stackMax: 5);
            var inv = new Inventory { Capacity = 1, MaxWeight = 50f };
            inv.Add(food, 1);

            int addEvents = 0;
            int changeEvents = 0;
            inv.OnItemAdded += (_, __) => addEvents++;
            inv.OnInventoryChanged += () => changeEvents++;

            // 6 items in one stack: stackMax=5 means we'd need a 2nd slot, but Capacity=1.
            bool ok = inv.Add(food, 6);
            Assert.IsFalse(ok, "Add must reject when not all-or-nothing fits.");
            Assert.AreEqual(0, addEvents, "OnItemAdded must not fire on a rejected Add.");
            Assert.AreEqual(0, changeEvents, "OnInventoryChanged must not fire on a rejected Add.");
            Assert.AreEqual(1, inv.Count(food), "Existing stack must be untouched on rejection.");

            Object.DestroyImmediate(food);
        }

        [Test]
        public void Add_PartialFill_ThenRejection_LeavesStateIntact()
        {
            // Repro of the original bug: pre-fix, the loop filled an existing stack
            // (firing OnItemAdded), then bailed when the new slot wouldn't fit. State
            // was: 4 added + 0 added + 6 dropped on the floor.
            var food = MakeItem("canned_beans", stackMax: 5);
            var inv = new Inventory { Capacity = 1, MaxWeight = 50f };
            inv.Add(food, 4); // existing 4 of 5

            bool ok = inv.Add(food, 6); // needs 2 slots, only 1 capacity
            Assert.IsFalse(ok);
            Assert.AreEqual(4, inv.Count(food), "Existing stack must be untouched on rejection.");

            Object.DestroyImmediate(food);
        }

        // -----------------------------------------------------------------
        // INV-002 — Unequip rolls back on full bag
        // -----------------------------------------------------------------

        [Test]
        public void Unequip_FullBag_KeepsEquippedItem()
        {
            // Capacity 1, full of a DIFFERENT item. Equipping any item requires
            // removing it from the bag; we use reflection to set _equipped so the
            // bag stays full of the foreign item. Unequip must roll back because
            // the bag cannot accept the hazmat (no matching stack, no free slot).
            var hazmat = MakeItem("hazmat", type: ItemType.Protective);
            hazmat.equipSlot = EquipSlot.Body;
            hazmat.isEquipable = true;
            var junk = MakeItem("scrap_metal");
            junk.isEquipable = true;
            junk.equipSlot = EquipSlot.None; // defensive: make sure junk is not equipable

            var inv = new Inventory { Capacity = 1, MaxWeight = 50f };
            Assert.IsTrue(inv.Add(junk, 1), "precondition: bag starts full of junk");
            // Reflectively inject the hazmat as already-equipped so we test the
            // Unequip path in isolation, not Equip.
            typeof(Inventory).GetField("_equipped",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(inv, new List<EquippedItem>
                {
                    new EquippedItem { Item = hazmat, CurrentDurability = hazmat.durability }
                });
            Assert.IsFalse(inv.CanAdd(hazmat, 1), "precondition: bag full of foreign item, cannot accept hazmat");

            var result = inv.Unequip(EquipSlot.Body);
            Assert.IsNull(result, "Unequip must return null when storage cannot accept the item.");
            Assert.IsNotNull(inv.GetEquipped(EquipSlot.Body),
                "Equipped item must stay equipped when Unequip is rejected.");

            Object.DestroyImmediate(hazmat);
            Object.DestroyImmediate(junk);
        }

        [Test]
        public void Unequip_EmptySlot_ReturnsNull()
        {
            var inv = new Inventory { Capacity = 5, MaxWeight = 50f };
            var result = inv.Unequip(EquipSlot.Body);
            Assert.IsNull(result);
        }

        // -----------------------------------------------------------------
        // DEEP3-INV-003 — DrinkActionSO removes from inventory
        // -----------------------------------------------------------------

        [Test]
        public void DrinkAction_Execute_RemovesFromInventory_AndReducesThirst()
        {
            var survivor = MakeSurvivor();
            survivor.Needs.Thirst = 100f;

            var water = MakeItem("clean_water", thirstRestore: 50f, type: ItemType.Food);
            var inv = new Inventory { Capacity = 5, MaxWeight = 50f };
            inv.Add(water, 2);

            var action = ScriptableObject.CreateInstance<DrinkActionSO>();
            var ctx = new AIContext
            {
                Survivor = survivor,
                Inventory = inv,
                Random = new System.Random(1)
            };

            action.Execute(ctx);

            Assert.AreEqual(1, inv.Count(water), "Drink must remove one water from inventory.");
            Assert.That(survivor.Needs.Thirst, Is.EqualTo(50f).Within(Eps),
                "Drink must reduce Thirst by the item's thirstRestore (Thirst high = thirsty).");

            Object.DestroyImmediate(water);
            Object.DestroyImmediate(action);
        }

        [Test]
        public void DrinkAction_Execute_NoWater_DoesNothing()
        {
            var survivor = MakeSurvivor();
            survivor.Needs.Thirst = 100f;

            var inv = new Inventory { Capacity = 5, MaxWeight = 50f };
            var action = ScriptableObject.CreateInstance<DrinkActionSO>();
            var ctx = new AIContext
            {
                Survivor = survivor,
                Inventory = inv,
                Random = new System.Random(1)
            };

            action.Execute(ctx);

            Assert.That(survivor.Needs.Thirst, Is.EqualTo(100f).Within(Eps),
                "Drink must NOT mutate Thirst when there is no water to consume.");

            Object.DestroyImmediate(action);
        }

        // -----------------------------------------------------------------
        // DEEP3-INV-004 — TakeIodineActionSO calls RadiationSystem.AdministerIodine
        // -----------------------------------------------------------------

        [Test]
        public void TakeIodineAction_Execute_CallsAdministerIodine_AndSetsResistance()
        {
            var survivor = MakeSurvivor();
            var iodine = MakeItem("iodine_pills", type: ItemType.Medical);
            iodine.equipSlot = EquipSlot.None; // not equipable; inventory item only

            var inv = new Inventory { Capacity = 5, MaxWeight = 50f };
            inv.Add(iodine, 1);

            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile);
            var rad = new RadiationSystem(needs);

            Assert.IsFalse(survivor.HasRadResistance, "precondition");

            var action = ScriptableObject.CreateInstance<TakeIodineActionSO>();
            var ctx = new AIContext
            {
                Survivor = survivor,
                Inventory = inv,
                RadiationSystem = rad,
                Random = new System.Random(1)
            };
            action.Execute(ctx);

            Assert.IsTrue(survivor.HasRadResistance, "AdministerIodine must set HasRadResistance.");
            Assert.Greater(survivor.RadResistanceHoursRemaining, 0f,
                "AdministerIodine must grant a positive resistance window.");
            Assert.AreEqual(0, inv.Count(iodine), "Iodine pill must be removed from inventory.");

            Object.DestroyImmediate(iodine);
            Object.DestroyImmediate(action);
            Object.DestroyImmediate(profile);
        }

        // -----------------------------------------------------------------
        // DEEP3-INV-005 — UseAntiRadActionSO calls RadiationSystem.AdministerAntiRad
        // -----------------------------------------------------------------

        [Test]
        public void UseAntiRadAction_Execute_CallsAdministerAntiRad_AndReducesDose()
        {
            var survivor = MakeSurvivor();
            survivor.RadiationDose = 80f;

            var antiRad = MakeItem("anti_rad", radCleanse: 30f, type: ItemType.Medical);
            var inv = new Inventory { Capacity = 5, MaxWeight = 50f };
            inv.Add(antiRad, 1);

            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile);
            var rad = new RadiationSystem(needs);

            var action = ScriptableObject.CreateInstance<UseAntiRadActionSO>();
            action.GetChemEffectiveness = (_, __) => 1f;
            action.GetChemDurationHours = (_, __) => 0f;
            var ctx = new AIContext
            {
                Survivor = survivor,
                Inventory = inv,
                RadiationSystem = rad,
                Random = new System.Random(1)
            };
            action.Execute(ctx);

            Assert.That(survivor.RadiationDose, Is.EqualTo(50f).Within(Eps),
                "AdministerAntiRad must reduce RadiationDose by BaseRadReduction*effectiveness.");
            Assert.AreEqual(0, inv.Count(antiRad), "Anti-rad must be removed from inventory.");

            Object.DestroyImmediate(antiRad);
            Object.DestroyImmediate(action);
            Object.DestroyImmediate(profile);
        }

        // -----------------------------------------------------------------
        // DEEP-001 — critical need drains Health (starvation kills)
        // -----------------------------------------------------------------

        [Test]
        public void NeedsSystem_CriticalHunger_DrainsHealth()
        {
            var survivor = MakeSurvivor();
            // Hunger is clamped to [0,100] by Modify, so the only value we can hold is
            // the cap — which is also the default critical threshold. The drain rate
            // scales with Hunger/critical, so the cap is the worst case.
            survivor.Needs.Hunger = 100f;
            survivor.Needs.Health = 100f;

            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile);
            needs.Tick(survivor, 1f);

            Assert.Less(survivor.Needs.Health, 100f,
                "A survivor at critical hunger must lose Health — without this, "
                + "starvation can softlock a run on full rations but no deaths.");

            Object.DestroyImmediate(profile);
        }

        [Test]
        public void NeedsSystem_AtExactCriticalThreshold_DrainsHealthAtFullRate()
        {
            // Hunger is clamped to [0,100] and hungerCritical=100, so 'at critical' is
            // indistinguishable from 'at the cap'. The drain fraction is Hunger/critical,
            // which is 1.0 here — the full healthLossFromHunger per hour. A survivor
            // sitting at 100 hunger loses 3 HP/hour (default profile) until they eat
            // or die. This is the entire fix for DEEP-001: without it, starvation
            // could not kill.
            var survivor = MakeSurvivor();
            survivor.Needs.Hunger = 100f;
            survivor.Needs.Health = 100f;

            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile);
            needs.Tick(survivor, 1f);

            Assert.Less(survivor.Needs.Health, 100f,
                "A survivor at exact critical hunger (the cap) must lose Health at the full rate.");
            Assert.Less(survivor.Needs.Morale, 100f,
                "Morale must still drain at exact critical.");

            Object.DestroyImmediate(profile);
        }

        [Test]
        public void NeedsSystem_CriticalCold_DrainsHealth()
        {
            var survivor = MakeSurvivor();
            survivor.Needs.Warmth = 0f; // below warmthCritical=10
            survivor.Needs.Health = 100f;

            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile,
                isNearHeatSource: _ => false); // cold
            needs.Tick(survivor, 1f);

            Assert.Less(survivor.Needs.Health, 100f,
                "A freezing survivor must lose Health.");

            Object.DestroyImmediate(profile);
        }

        // -----------------------------------------------------------------
        // DEATH-001/004 — SurvivorNeedWrite hardening
        // -----------------------------------------------------------------

        [Test]
        public void SurvivorNeedWrite_SetHealth_OnDeadSurvivor_DoesNotCreateZombie()
        {
            // DEATH-004 regression: pre-fix, SetHealth(deadSv, 50f) set Health=50
            // while leaving State=Dead, creating a zombie. Post-fix, the call
            // returns early when !IsAlive and forceRevive is false.
            var sv = MakeSurvivor();
            sv.Needs.Health = 100f;
            sv.State = SurvivorState.Dead;
            SurvivorNeedWrite.SetHealth(sv, 50f);
            Assert.AreEqual(100f, sv.Needs.Health,
                "SetHealth on a dead survivor must not write positive HP (no zombie).");
            Assert.AreEqual(SurvivorState.Dead, sv.State);
        }

        [Test]
        public void SurvivorNeedWrite_SetHealth_WithForceRevive_OnDeadSurvivor_WritesHP()
        {
            // The legitimate resurrect path (adrenaline revive) must opt in.
            var sv = MakeSurvivor();
            sv.Needs.Health = 100f;
            sv.State = SurvivorState.Dead;
            SurvivorNeedWrite.SetHealth(sv, 1f, -1f, null, forceRevive: true);
            Assert.AreEqual(1f, sv.Needs.Health,
                "forceRevive: true must allow SetHealth on a dead survivor.");
        }

        [Test]
        public void SurvivorNeedWrite_SetHealth_DropsToZero_FiresOnKilled()
        {
            // DEATH-001 regression: pre-fix, SetHealth(sv, 0f) set State=Dead
            // silently — no OnDied, no personal quest hooks. Post-fix, the
            // optional onKilled callback fires when Health crosses to 0.
            var sv = MakeSurvivor();
            sv.Needs.Health = 100f;
            sv.State = SurvivorState.Idle;
            int killCount = 0;
            SurvivorNeedWrite.SetHealth(sv, 0f, -1f, _ => killCount++);
            Assert.AreEqual(0f, sv.Needs.Health);
            Assert.AreEqual(SurvivorState.Dead, sv.State);
            Assert.AreEqual(1, killCount, "onKilled must fire exactly once when Health drops to 0.");
        }

        [Test]
        public void SurvivorNeedWrite_SetHealth_AlreadyDead_DoesNotDoubleFire()
        {
            // Health was already 0 (previous kill). SetHealth(sv, 0f) must
            // not fire onKilled a second time — death is a transition, not a state.
            var sv = MakeSurvivor();
            sv.Needs.Health = 0f;
            sv.State = SurvivorState.Dead;
            int killCount = 0;
            SurvivorNeedWrite.SetHealth(sv, 0f, -1f, _ => killCount++);
            Assert.AreEqual(0, killCount,
                "SetHealth on an already-dead survivor must not fire onKilled.");
        }

    }
}
