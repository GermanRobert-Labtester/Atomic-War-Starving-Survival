using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.PlayMode
{
    /// <summary>
    /// PlayMode integration tests for the inventory + atomic item model: equipping a
    /// protective suit reduces radiation exposure, consuming iodine grants a timed
    /// rad-resistance status, inventory survives a save/load round-trip, and the
    /// weight limit + transfer behave correctly.
    /// </summary>
    [TestFixture]
    public class InventoryPlayModeTests
    {
        private const float Eps = 1e-4f;

        /// <summary>Parameter object for NewItem — reduces the 10-param signature (audit smell fix).</summary>
        private struct ItemCfg
        {
            public float RadProtection;
            public float Durability;
            public bool Equipable;
            public EquipSlot Slot;
            public int StackMax;
            public float Weight;
            public float RadCleanse;
            public float Contamination;

            public static readonly ItemCfg Default = new ItemCfg
            {
                StackMax = 10,
                Weight = 1f
            };
        }

        private static ItemDefinition NewItem(string id, ItemType type, ItemCfg cfg = default)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = type;
            item.radProtection = cfg.RadProtection;
            item.durability = cfg.Durability;
            item.isEquipable = cfg.Equipable;
            item.equipSlot = cfg.Slot;
            item.stackMax = cfg.StackMax > 0 ? cfg.StackMax : ItemCfg.Default.StackMax;
            item.weight = cfg.Weight > 0f ? cfg.Weight : ItemCfg.Default.Weight;
            item.radCleanse = cfg.RadCleanse;
            item.contamination = cfg.Contamination;
            return item;
        }

        private static NeedsSystem NewNeedsSystem()
        {
            return new NeedsSystem(ScriptableObject.CreateInstance<NeedsProfile>());
        }

        [UnityTest]
        public IEnumerator EquipSuit_ReducesRadiationExposure()
        {
            var suit = NewItem("hazmat_suit", ItemType.Protective, new ItemCfg
            {
                RadProtection = 80f, Durability = 100f, Equipable = true,
                Slot = EquipSlot.Body, StackMax = 1
            });
            var inventory = new Inventory { Capacity = 10, MaxWeight = 1000f };
            inventory.Add(suit, 1);

            Assert.That(inventory.Equip(suit), Is.True);
            Assert.That(inventory.GetEquippedProtection(), Is.EqualTo(80f).Within(Eps));

            var needs = NewNeedsSystem();

            // Survivor wearing the suit: exposure context built from equipped gear.
            var suited = new Survivor { Id = "suited" };
            var radSuited = new RadiationSystem(needs, s =>
            {
                var ctx = new ExposureContext { ZoneRadLevel = 100f };
                ctx.WornGear.AddRange(inventory.BuildWornGear());
                return ctx;
            });
            radSuited.Register(suited);
            radSuited.Tick(1f);

            // Survivor with no protection.
            var exposed = new Survivor { Id = "exposed" };
            var radExposed = new RadiationSystem(needs, s => new ExposureContext { ZoneRadLevel = 100f });
            radExposed.Register(exposed);
            radExposed.Tick(1f);

            Assert.That(exposed.LifetimeRadiationExposure, Is.EqualTo(100f).Within(Eps),
                "Unprotected survivor absorbs the full 100 rad/h for one hour");
            Assert.That(suited.LifetimeRadiationExposure, Is.EqualTo(20f).Within(Eps),
                "Suited survivor absorbs 100 - 80 protection");
            Assert.That(suited.LifetimeRadiationExposure, Is.LessThan(exposed.LifetimeRadiationExposure),
                "Wearing the suit must reduce absorbed exposure");

            yield return null;
        }

        [UnityTest]
        public IEnumerator ConsumeIodine_GrantsTimedRadResistance()
        {
            var iodine = NewItem("iodine_pills", ItemType.Iodine, new ItemCfg { StackMax = 5 });
            var inventory = new Inventory { Capacity = 10 };
            inventory.Add(iodine, 2);

            var needs = NewNeedsSystem();
            var survivor = new Survivor { Id = "s1" };

            // Constant zone dose of 20/hr so the resistance halving is observable.
            var rad = new RadiationSystem(needs, s => new ExposureContext { ZoneRadLevel = 20f });
            rad.Register(survivor);

            Assert.That(inventory.Consume(iodine, survivor, rad, needs), Is.True);
            Assert.That(inventory.Count(iodine), Is.EqualTo(1));
            Assert.That(survivor.HasStatus(SurvivorStatus.RadResistance), Is.True);
            Assert.That(survivor.RadResistanceHoursRemaining, Is.EqualTo(RadiationSystem.IodineResistanceHours).Within(Eps));

            // While resistant the dose rate is halved: 20 * 0.5 = 10 for one hour.
            rad.Tick(1f);
            Assert.That(survivor.RadiationDose, Is.EqualTo(10f).Within(Eps));

            // Exhaust the remaining timer; the status expires.
            rad.Tick(survivor.RadResistanceHoursRemaining);
            Assert.That(survivor.HasStatus(SurvivorStatus.RadResistance), Is.False);
            Assert.That(survivor.RadResistanceHoursRemaining, Is.EqualTo(0f).Within(Eps));

            yield return null;
        }

        [UnityTest]
        public IEnumerator SaveLoad_InventoryStaysIntact()
        {
            var food = NewItem("canned_food", ItemType.Food, new ItemCfg { StackMax = 10, Weight = 0.5f });
            var suit = NewItem("hazmat_suit", ItemType.Protective, new ItemCfg
            {
                RadProtection = 80f, Durability = 100f, Equipable = true,
                Slot = EquipSlot.Body, StackMax = 1, Weight = 5f
            });
            var lookup = new Dictionary<string, ItemDefinition> { { food.id, food }, { suit.id, suit } };

            var inventory = new Inventory { Capacity = 20, MaxWeight = 100f };
            inventory.Add(food, 3);
            inventory.Add(suit, 1);
            Assert.That(inventory.Equip(suit), Is.True);

            var state = inventory.CaptureState();
            string json = JsonUtility.ToJson(state);
            var restored = JsonUtility.FromJson<InventorySaveState>(json);

            var rebuilt = new Inventory();
            rebuilt.RestoreState(restored, id => lookup.TryGetValue(id, out var item) ? item : null);

            Assert.That(rebuilt.Capacity, Is.EqualTo(20));
            Assert.That(rebuilt.Count(food), Is.EqualTo(3));

            var equipped = rebuilt.GetEquipped(EquipSlot.Body);
            Assert.That(equipped, Is.Not.Null);
            Assert.That(equipped.Item.id, Is.EqualTo("hazmat_suit"));
            Assert.That(equipped.CurrentDurability, Is.EqualTo(100f).Within(Eps));

            yield return null;
        }

        [UnityTest]
        public IEnumerator WeightLimit_BlocksAdd_And_TransferMovesStock()
        {
            var food = NewItem("canned_food", ItemType.Food, new ItemCfg { StackMax = 10, Weight = 1f });
            var inventory = new Inventory { Capacity = 20, MaxWeight = 5f };

            Assert.That(inventory.Add(food, 5), Is.True);  // 5 * 1 == MaxWeight
            Assert.That(inventory.Add(food, 1), Is.False); // would exceed the weight limit

            var other = new Inventory { Capacity = 20, MaxWeight = 100f };
            Assert.That(inventory.Transfer(food, 3, other), Is.True);
            Assert.That(inventory.Count(food), Is.EqualTo(2));
            Assert.That(other.Count(food), Is.EqualTo(3));

            yield return null;
        }
    }
}
