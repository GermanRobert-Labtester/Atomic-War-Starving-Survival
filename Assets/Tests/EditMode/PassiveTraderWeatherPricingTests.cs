using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// REPROMOTE-001 — PassiveTrader weather exchange rates must affect barter unit
    /// values when the host wires SetWeatherItemPriceMultiplier (as GameBootstrap does).
    /// </summary>
    [TestFixture]
    public class PassiveTraderWeatherPricingTests
    {
        private const float Eps = 1e-3f;
        private ItemDefinition _water;

        [SetUp]
        public void SetUp()
        {
            _water = ScriptableObject.CreateInstance<ItemDefinition>();
            _water.id = "clean_water";
            _water.displayName = "Clean Water";
            _water.tradeValue = 10f;
            _water.type = ItemType.Material;
        }

        [TearDown]
        public void TearDown()
        {
            if (_water != null) Object.DestroyImmediate(_water);
            _water = null;
        }

        [Test]
        public void PassiveTrader_FalloutStorm_MultipliesWaterFiveTimes()
        {
            var trader = new NPC_PassiveTrader();
            float clear = trader.GetPriceMultiplierForItem("clean_water", "Clear");
            float storm = trader.GetPriceMultiplierForItem("clean_water", "FalloutStorm");
            Assert.AreEqual(1f, clear, Eps);
            Assert.AreEqual(5f, storm, Eps);
        }

        [Test]
        public void Economy_WeatherProvider_AppliesPassiveTraderMultToBarter()
        {
            var trader = new NPC_PassiveTrader();
            string weather = "Clear";
            var eco = new DynamicEconomySystem(() => WorldPhase.NuclearWinter, null, new System.Random(1));
            var factions = DynamicEconomySystem.CreateDefaultFactions();
            try
            {
                for (int i = 0; i < factions.Count; i++)
                    eco.RegisterFaction(factions[i]);

                eco.SetWeatherItemPriceMultiplier(itemId =>
                    trader.GetPriceMultiplierForItem(itemId, weather));

                string factionId = FactionSO.Ids.ScavengerCamp;
                Assert.IsTrue(eco.Factions.ContainsKey(factionId));

                float clearBuy = eco.GetBarterUnitValue(_water, factionId, playerSelling: false);
                weather = "FalloutStorm";
                float stormBuy = eco.GetBarterUnitValue(_water, factionId, playerSelling: false);

                Assert.Greater(clearBuy, 0f);
                Assert.AreEqual(clearBuy * 5f, stormBuy, Eps,
                    "Fallout storm must apply PassiveTrader 5× water mult on barter unit value");
            }
            finally
            {
                for (int i = 0; i < factions.Count; i++)
                {
                    if (factions[i] != null) Object.DestroyImmediate(factions[i]);
                }
            }
        }
    }
}
