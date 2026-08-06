using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompt #806 — automated bilge pumps: activate, detect flood, route water, save.
    /// </summary>
    [TestFixture]
    public class BilgePumpsTests
    {
        private const float Eps = 1e-3f;

        [Test]
        public void Activate_EnablesRouting_DeactivateStops()
        {
            var pumps = new System_BilgePumps();
            Assert.IsFalse(pumps.IsActive());

            Assert.AreEqual(0f, pumps.RouteWater(100f), Eps);

            pumps.Activate();
            Assert.IsTrue(pumps.IsActive());
            float purified = pumps.RouteWater(100f);
            Assert.AreEqual(70f, purified, Eps); // default 0.7 efficiency
            Assert.AreEqual(70f, pumps.GetTotalWaterRouted(), Eps);

            pumps.Deactivate();
            Assert.AreEqual(0f, pumps.RouteWater(100f), Eps);
            Assert.AreEqual(70f, pumps.GetTotalWaterRouted(), Eps);
        }

        [Test]
        public void DetectFlooding_FiresEvent()
        {
            var pumps = new System_BilgePumps();
            int hits = 0;
            pumps.OnFloodingDetected += () => hits++;

            Assert.IsFalse(pumps.DetectFlooding(false));
            Assert.AreEqual(0, hits);
            Assert.IsTrue(pumps.DetectFlooding(true));
            Assert.AreEqual(1, hits);
        }

        [Test]
        public void RouteWater_FiresRoutedAndPurifierBoost()
        {
            var pumps = new System_BilgePumps();
            pumps.Activate();
            pumps.SetPurificationEfficiency(0.5f);

            float routed = -1f;
            int boosts = 0;
            pumps.OnWaterRouted += L => routed = L;
            pumps.OnPurifierBoosted += () => boosts++;

            float got = pumps.RouteWater(40f);
            Assert.AreEqual(20f, got, Eps);
            Assert.AreEqual(20f, routed, Eps);
            Assert.AreEqual(1, boosts);
        }

        [Test]
        public void ProcessFloodedRooms_InactiveProducesZero_ActiveProducesScaled()
        {
            var pumps = new System_BilgePumps();
            Assert.AreEqual(0f, pumps.ProcessFloodedRooms(2), Eps);

            pumps.Activate();
            // 2 rooms × 50 L × 0.7 = 70
            float purified = pumps.ProcessFloodedRooms(2);
            Assert.AreEqual(70f, purified, Eps);
            Assert.AreEqual(70f, pumps.TotalWaterRouted, Eps);
        }

        [Test]
        public void FloodingSystem_OnRoomFlooded_WithActivePumps_AddsCleanWater()
        {
            // Mirrors GameBootstrap.WireBilgePumps host pattern.
            var pumps = new System_BilgePumps();
            pumps.Activate();
            var flood = new RoomFloodingSystem();
            var water = new WaterStorage();
            float cleanBefore = water.CleanWater;

            flood.OnRoomFlooded += _ =>
            {
                float purified = pumps.ProcessFloodedRooms(1);
                if (purified > 0f) water.AddClean(purified);
            };

            flood.ForceFlood("cellar");
            Assert.AreEqual(cleanBefore + 35f, water.CleanWater, Eps); // 50 * 0.7
            Assert.IsTrue(flood.IsFlooded("cellar"));
        }

        [Test]
        public void CaptureRestore_PreservesActiveAndTotals()
        {
            var a = new System_BilgePumps();
            a.Activate();
            a.SetPurificationEfficiency(0.8f);
            a.RouteWater(25f); // 20 purified

            var save = a.CaptureState();
            Assert.AreEqual("system_bilge_pumps", save.systemId);
            Assert.IsTrue(save.isActive);
            Assert.AreEqual(0.8f, save.purificationEfficiency, Eps);
            Assert.AreEqual(20f, save.totalWaterRouted, Eps);

            // Mutate after capture.
            a.RouteWater(10f);
            Assert.AreEqual(20f, save.totalWaterRouted, Eps);

            var b = new System_BilgePumps();
            b.RestoreState(save);
            Assert.IsTrue(b.IsActive());
            Assert.AreEqual(0.8f, b.PurificationEfficiency, Eps);
            Assert.AreEqual(20f, b.GetTotalWaterRouted(), Eps);

            b.RestoreState(null);
            Assert.IsFalse(b.IsActive());
            Assert.AreEqual(0f, b.GetTotalWaterRouted(), Eps);
        }

        [Test]
        public void SaveSystemAdapter_BilgePumpsSlot_RoundTrip()
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_bilge_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            try
            {
                var profile = ScriptableObject.CreateInstance<NeedsProfile>();
                var needs = new NeedsSystem(profile, sv => true);
                var weather = new WeatherSystem(null, 3);
                var temp = new TemperatureSystem(null, weather);
                var rad = new RadiationSystem(needs);

                var bilgeA = new System_BilgePumps();
                bilgeA.Activate();
                bilgeA.RouteWater(100f);
                Assert.AreEqual(70f, bilgeA.GetTotalWaterRouted(), Eps);

                SaveSystem Make(System_BilgePumps bilge)
                {
                    var ss = new SaveSystem(new SaveSystem.CoreDeps
                    {
                        GameState = new GameState(),
                        WeatherSystem = weather,
                        TemperatureSystem = temp,
                        NeedsSystem = needs,
                        RadiationSystem = rad,
                        Shelter = new ShelterClass(),
                        GetSurvivors = () => new List<Survivor>(),
                        ItemLookup = id => null,
                        ModuleLookup = id => null,
                        SavesDir = dir
                    });
                    ss.SetBilgePumpsSystem(bilge);
                    return ss;
                }

                Assert.IsTrue(Make(bilgeA).Save("bilge_slot"));

                var bilgeB = new System_BilgePumps();
                Assert.IsTrue(Make(bilgeB).Load("bilge_slot"));

                Assert.IsTrue(bilgeB.IsActive());
                Assert.AreEqual(70f, bilgeB.GetTotalWaterRouted(), Eps);
                Assert.AreEqual(0.7f, bilgeB.PurificationEfficiency, Eps);

                UnityEngine.Object.DestroyImmediate(profile);
            }
            finally
            {
                try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { /* best-effort */ }
            }
        }
    }
}
