using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// REPROMOTE-Weapon-001 — hatch GetWeaponPower reads HMG stock + crew.
    /// </summary>
    [TestFixture]
    public class WeaponHmgRepromoteTests
    {
        private List<Object> _destroy;

        [SetUp]
        public void SetUp() => _destroy = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            for (int i = 0; i < _destroy.Count; i++)
            {
                if (_destroy[i] != null) Object.DestroyImmediate(_destroy[i]);
            }
            _destroy = null;
        }

        [Test]
        public void Hmg_GetHatchDefensePower_RequiresMountCrewAndOil()
        {
            var hmg = new Weapon_HMG();
            Assert.AreEqual(0f, hmg.GetHatchDefensePower(2, true));
            hmg.Mount("hatch");
            Assert.AreEqual(0f, hmg.GetHatchDefensePower(1, true), "needs 2 operators");
            Assert.AreEqual(Weapon_HMG.HatchDefensePower, hmg.GetHatchDefensePower(2, true));
            Assert.AreEqual(0f, hmg.GetHatchDefensePower(2, isOiled: false));
        }

        [Test]
        public void GetWeaponPower_IncludesMountedHmgWhenStockAndCrewed()
        {
            var inv = new Inventory { Capacity = 30, MaxWeight = 100f };
            var hmgItem = ScriptableObject.CreateInstance<ItemDefinition>();
            hmgItem.id = Weapon_HMG.InventoryItemId;
            hmgItem.type = ItemType.Weapon;
            hmgItem.stackMax = 1;
            _destroy.Add(hmgItem);
            inv.Add(hmgItem, 1);

            var g1 = new Survivor { Id = "g1", DisplayName = "Gunner", State = SurvivorState.Idle };
            var g2 = new Survivor { Id = "g2", DisplayName = "Loader", State = SurvivorState.Idle };
            var survivors = new List<Survivor> { g1, g2 };

            var hmg = new Weapon_HMG();
            hmg.Mount("hatch");

            var hatch = new HatchDefenseSystem(
                getInventory: () => inv,
                getSurvivors: () => survivors,
                getDay: () => 40);
            hatch.AssignGuard(g1, bonus: 1f);
            hatch.AssignGuard(g2, bonus: 1f);
            hatch.GetMountedHmgDefensePower = stock =>
            {
                if (stock.CountById(Weapon_HMG.InventoryItemId) <= 0) return 0f;
                return hmg.GetHatchDefensePower(hatch.ActiveGuardCount, isOiled: true);
            };

            float power = hatch.GetWeaponPower();
            Assert.That(power, Is.GreaterThanOrEqualTo(Weapon_HMG.HatchDefensePower),
                "GetWeaponPower must include mounted HMG defense when stocked and crewed");
        }
    }
}
