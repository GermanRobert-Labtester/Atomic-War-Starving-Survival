using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Environment;
using Ashfall.Core;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class SubBayAndForecastTests
    {
        [Test]
        public void SubBay_InitialState_IsFloodedAndLocked()
        {
            var subBay = new ShelterModule_SubBay();
            Assert.IsFalse(subBay.IsBuilt);
            Assert.AreEqual(4.5f, subBay.WaterDepthMeters);
            Assert.IsFalse(subBay.IsDivingUnlocked);
            Assert.IsFalse(subBay.PerformDive());
        }

        [Test]
        public void SubBay_Pumping_DrainsWaterAndUnlocksDiving()
        {
            var subBay = new ShelterModule_SubBay();
            subBay.BuildModule();
            Assert.IsTrue(subBay.IsBuilt);

            var pumps = new System_BilgePumps();
            pumps.Activate();

            bool drainedFired = false;
            subBay.OnSubBayDrained += (state) => drainedFired = true;

            // Pump down from 4.5m by 1.0m four times
            subBay.PumpWater(pumps, 1.0f);
            Assert.AreEqual(3.5f, subBay.WaterDepthMeters);
            Assert.IsFalse(subBay.IsDivingUnlocked);

            subBay.PumpWater(pumps, 1.0f);
            subBay.PumpWater(pumps, 1.0f);
            subBay.PumpWater(pumps, 1.0f); // 0.5m left <= 1.0m

            Assert.AreEqual(0.5f, subBay.WaterDepthMeters);
            Assert.IsTrue(subBay.IsDivingUnlocked);
            Assert.IsTrue(drainedFired);
            Assert.IsTrue(subBay.PerformDive());
            Assert.AreEqual(1, subBay.State.completedDivesCount);
        }

        [Test]
        public void SubBay_SaveRestore_PreservesDepthAndDivingState()
        {
            var subBayA = new ShelterModule_SubBay();
            subBayA.BuildModule();
            var pumps = new System_BilgePumps();
            pumps.Activate();
            subBayA.PumpWater(pumps, 4.0f);
            subBayA.PerformDive();

            var state = subBayA.CaptureState();

            var subBayB = new ShelterModule_SubBay();
            subBayB.RestoreState(state);

            Assert.IsTrue(subBayB.IsBuilt);
            Assert.AreEqual(0.5f, subBayB.WaterDepthMeters);
            Assert.IsTrue(subBayB.IsDivingUnlocked);
            Assert.AreEqual(1, subBayB.State.completedDivesCount);
        }

        [Test]
        public void FalloutForecast_UpgradeSensor_ExpandsHorizonAndAccuracy()
        {
            var forecastSys = new FalloutForecastSystem(null);
            Assert.AreEqual(1, forecastSys.SensorLevel);
            Assert.AreEqual(5, forecastSys.HorizonDays);

            forecastSys.UpgradeSensorArray(3);
            Assert.AreEqual(3, forecastSys.SensorLevel);
            Assert.AreEqual(6, forecastSys.HorizonDays);
            Assert.AreEqual(0.2f, forecastSys.State.sensorAccuracyBonus);
        }

        [Test]
        public void FalloutForecast_GenerateForecast_ProducesEntriesWithConfidenceDecay()
        {
            var forecastSys = new FalloutForecastSystem(null);
            var entries = forecastSys.GenerateForecast(currentDay: 10, worldSeed: 12345);

            Assert.AreEqual(5, entries.Count);
            Assert.IsTrue(entries[0].confidence > entries[4].confidence);
            Assert.IsTrue(entries[0].predictedRadLevel > 0f);
            Assert.IsNotNull(entries[0].windDirection);
        }
    }
}
