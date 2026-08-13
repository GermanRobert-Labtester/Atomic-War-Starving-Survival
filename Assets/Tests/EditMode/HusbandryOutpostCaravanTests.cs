using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Expeditions;
using AtomicWar._Game.Economy;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class HusbandryOutpostCaravanTests
    {
        [Test]
        public void AnimalPen_Building_SpawnsStarterHensAndYieldsEggs()
        {
            var pen = new ShelterModule_AnimalPen();
            pen.BuildPen();

            Assert.IsTrue(pen.IsBuilt);
            Assert.AreEqual(2, pen.AnimalCount);

            pen.AddFeed(10f);
            pen.AddWater(10f);

            int eggYield = 0;
            pen.OnYieldGathered += (species, amount) =>
            {
                if (species == LivestockSpecies.ScrapHen) eggYield += (int)amount;
            };

            pen.DailyTick();
            Assert.AreEqual(2, eggYield);
            Assert.AreEqual(2, pen.State.totalEggsGathered);
        }

        [Test]
        public void AnimalPen_Slaughter_YieldsMeatAndRemovesAnimal()
        {
            var pen = new ShelterModule_AnimalPen();
            pen.BuildPen();
            pen.AddAnimal(LivestockSpecies.AshGoat, "Goat-Alpha");

            var goat = pen.State.animals.Find(a => a.species == LivestockSpecies.AshGoat);
            Assert.IsNotNull(goat);

            int meat = pen.SlaughterAnimal(goat.animalId);
            Assert.AreEqual(18, meat);
            Assert.AreEqual(2, pen.AnimalCount);
            Assert.AreEqual(18, pen.State.totalMeatYieldedKg);
        }

        [Test]
        public void ForwardOutpost_EstablishAndFatigueReduction()
        {
            var outpostSys = new ForwardOutpostSystem();
            bool established = outpostSys.TryEstablishOutpost("node_hydro_plant", "Hydro Staging Camp", 2.0f);
            Assert.IsTrue(established);
            Assert.AreEqual(1, outpostSys.OutpostCount);

            float fatigueNormal = outpostSys.CalculateTravelFatigueMultiplier("node_ruined_market", 20f);
            float fatigueOutpost = outpostSys.CalculateTravelFatigueMultiplier("node_hydro_plant", 20f);

            Assert.AreEqual(20f, fatigueNormal);
            Assert.AreEqual(12f, fatigueOutpost); // 40% reduction
        }

        [Test]
        public void TravelingCaravan_MovementAndTrade()
        {
            var caravanSys = new TravelingCaravanSystem();
            var route = new List<string> { "node_crossing", "node_depot", "node_hydro" };
            caravanSys.SpawnCaravan("caravan_scalehouse", "Scalehouse Grain Train", "faction_the_scale", route);

            Assert.AreEqual(1, caravanSys.CaravanCount);
            Assert.IsNotNull(caravanSys.GetCaravanAtNode("node_crossing"));

            // Advance time
            caravanSys.DailyTick();
            caravanSys.DailyTick(); // reached stay duration (2 days) -> moves to node_depot

            Assert.IsNull(caravanSys.GetCaravanAtNode("node_crossing"));
            Assert.IsNotNull(caravanSys.GetCaravanAtNode("node_depot"));

            int playerRations = 10;
            bool bought = caravanSys.TryBuyItem("caravan_scalehouse", "item_clean_water", 2, ref playerRations);
            Assert.IsTrue(bought);
            Assert.AreEqual(8, playerRations);
            Assert.AreEqual(1, caravanSys.State.completedTradesCount);
        }
    }
}
