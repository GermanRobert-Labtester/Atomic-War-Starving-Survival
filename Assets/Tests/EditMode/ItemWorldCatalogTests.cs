using System.Text.RegularExpressions;
using NUnit.Framework;
using AtomicWar._Game.Core;
using AtomicWar._Game.Inventory;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Master snake_case world-item registry: military attachments, scrap, deprecated
    /// caliber bullets, armour, food, fuel, tools, water bottles.
    /// </summary>
    [TestFixture]
    public class ItemWorldCatalogTests
    {
        private static readonly Regex SnakeCase = new Regex(
            @"^[a-z][a-z0-9]*(_[a-z0-9]+)*$",
            RegexOptions.Compiled);

        [Test]
        public void Catalog_Is_NonEmpty_And_AllIds_SnakeCase()
        {
            var ids = Item_WorldCatalog.AllIds();
            Assert.Greater(ids.Count, 50, "world catalog should register the full content list");

            foreach (var id in ids)
            {
                Assert.IsTrue(SnakeCase.IsMatch(id), "not snake_case: " + id);
                Assert.IsTrue(Item_WorldCatalog.Contains(id), id);
                Assert.IsTrue(Item_WorldCatalog.TryGet(id, out var def), id);
                Assert.AreEqual(id, def.Id);
                Assert.IsFalse(string.IsNullOrEmpty(def.DisplayName), id);
            }
        }

        [Test]
        public void MilitaryAttachments_Are_ExtremelyRare_ItemIds()
        {
            string[] expected =
            {
                Item_WorldCatalog.AttMilSuppressor,
                Item_WorldCatalog.AttMilLaserdot,
                Item_WorldCatalog.AttMilTacticalGrip,
                Item_WorldCatalog.AttMilLongRangeScope,
                Item_WorldCatalog.AttMilHolosight,
                Item_WorldCatalog.AttMilDoubleScope5x10x
            };

            CollectionAssert.AreEqual(expected, Item_WorldCatalog.MilitaryAttachmentIds());

            foreach (var id in expected)
            {
                Assert.IsTrue(Item_WorldCatalog.TryGet(id, out var def), id);
                Assert.IsTrue(def.MilitaryGrade, id);
                Assert.IsTrue(def.ExtremelyRare, id);
                Assert.AreEqual(ItemType.Tool, def.Type, id);
                Assert.AreEqual(1, def.StackMax, id);
            }
        }

        [Test]
        public void DeprecatedBullets_Are_ScrapOnly_PerCaliber()
        {
            string[] calibers =
            {
                "cal_9x19", "cal_545x39", "cal_762x51", "cal_338lapua", "cal_50bmg"
            };

            foreach (var cal in calibers)
            {
                string id = Item_WorldCatalog.DeprecatedBulletId(cal);
                Assert.AreEqual("ammo_deprecated_" + cal, id);
                Assert.IsTrue(Item_WorldCatalog.TryGet(id, out var def), id);
                Assert.IsTrue(def.ScrapOnly, id);
                Assert.AreEqual(ItemType.Material, def.Type, id);
                Assert.IsNotNull(def.ScrapMaterials);
                Assert.Greater(def.ScrapMaterials.Length, 0, id);
            }
        }

        [Test]
        public void ScrapComponents_Registered()
        {
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.ShellCasing));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.BulletCasing));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.Gunpowder));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.Sulphur));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.ExplosivePowderNitroglycerin));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.ScrapMetal));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.Fertilizer));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.Cloth));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.SalvagedTechTrash));

            Assert.IsTrue(Item_WorldCatalog.TryGet(
                Item_WorldCatalog.ExplosivePowderNitroglycerin, out var nitro));
            Assert.IsTrue(nitro.MilitaryGrade);
            Assert.IsTrue(nitro.ExtremelyRare);
        }

        [Test]
        public void Armour_And_Helmets_Registered()
        {
            Assert.IsTrue(Item_WorldCatalog.TryGet(Item_WorldCatalog.BodyArmourMilitary, out var ba));
            Assert.IsTrue(ba.MilitaryGrade);
            Assert.AreEqual(ItemType.Protective, ba.Type);
            Assert.IsTrue(ba.IsEquipable);

            Assert.IsTrue(Item_WorldCatalog.TryGet(Item_WorldCatalog.HelmetMilitary, out var h));
            Assert.AreEqual("Head", h.EquipSlot);

            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.NvGogglesMilitary));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.HelmetHeavyMilitary));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.ArmourHeavyMilitary));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.BodyArmourDeprecated));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.HelmetDeprecated));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.HelmetHeavyDeprecated));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.BodyArmourHeavyDeprecated));
        }

        [Test]
        public void Food_Fuel_Tools_Water_Registered()
        {
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.CannedFood));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.VegetableCarrot));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.VegetablePotato));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.VegetableBeetroot));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.CannedMeat));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.PreservedCrackers));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.MreMilitary));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.BoiledVegetableSoup));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.HeartyMealCooked));

            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.Fuel1L));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.FuelHalfOf1L));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.AccelerantFull));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.AccelerantHalf));

            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.KnifeImprovised));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.KnifeSwissBattle));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.BayonetSwissMachete));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.Hammer));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.Screwdriver));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.Multitool));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.Shovel));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.GrenadeMilitary));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.Crowbar));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.WireCutters));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.Lockpick));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.MetalPipe));

            Assert.IsTrue(Item_WorldCatalog.TryGet(Item_WorldCatalog.GrenadeMilitary, out var grenade));
            Assert.IsTrue(grenade.ExtremelyRare);
            Assert.IsTrue(grenade.MilitaryGrade);

            // Broken tools are scrap-only
            Assert.IsTrue(Item_WorldCatalog.TryGet(Item_WorldCatalog.CrowbarBroken, out var broken));
            Assert.IsTrue(broken.ScrapOnly);
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.WireCuttersBroken));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.MetalPipeBroken));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.ShovelBroken));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.MultitoolBroken));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.KnifeBroken));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.HammerBroken));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.ScrewdriverBroken));

            // Water bottles (fill / capacity)
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.WaterBottle1LFull));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.WaterBottle2LFull));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.WaterBottle1LOf2L));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.WaterBottleHalfOf1L));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.WaterBottleHalfOf2L));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.WaterBottle1_5LOf2L));
            Assert.IsTrue(Item_WorldCatalog.Contains(Item_WorldCatalog.WaterBottleEmpty));

            Assert.IsTrue(Item_WorldCatalog.TryGet(Item_WorldCatalog.WaterBottle1LFull, out var water));
            Assert.AreEqual(ItemType.Water, water.Type);
            Assert.Greater(water.ThirstRestore, 0f);
        }
    }
}
