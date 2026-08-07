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
    /// Prompt #658 — carrion birds: outdoor corpses → vultures → hatch/map/morale, save.
    /// </summary>
    [TestFixture]
    public class CarrionBirdsTests
    {
        private const float Eps = 1e-3f;

        [Test]
        public void Spawn_AddCorpse_IncrementsAndFiresEvent()
        {
            var birds = new System_CarrionBirds();
            Assert.AreEqual(0, birds.CorpseCount);
            Assert.IsFalse(birds.VulturesPresent);

            int seen = -1;
            birds.OnCorpseAdded += (_, count) => seen = count;

            birds.AddCorpse();
            Assert.AreEqual(1, birds.CorpseCount);
            Assert.AreEqual(1, seen);

            birds.AddCorpse();
            Assert.AreEqual(2, birds.CorpseCount);
            Assert.AreEqual(2, seen);
        }

        [Test]
        public void TickDay_WithCorpses_SpawnsVulturesAndHatchVisibility()
        {
            var birds = new System_CarrionBirds();
            int arrivals = 0;
            birds.OnVulturesArrived += _ => arrivals++;

            birds.AddCorpse();
            birds.TickDay();

            Assert.IsTrue(birds.VulturesPresent);
            Assert.AreEqual(1, arrivals);
            Assert.AreEqual(1f, birds.GetHatchVisibility(), Eps);

            // Idempotent while corpses remain.
            birds.TickDay();
            Assert.AreEqual(1, arrivals);
            Assert.IsTrue(birds.VulturesPresent);
        }

        [Test]
        public void TickDay_AfterRemoveCorpses_DepartsFlock()
        {
            var birds = new System_CarrionBirds();
            birds.AddCorpse();
            birds.TickDay();
            Assert.IsTrue(birds.VulturesPresent);

            int departures = 0;
            birds.OnVulturesDeparted += _ => departures++;
            int removed = 0;
            birds.OnCorpsesRemoved += _ => removed++;

            birds.RemoveCorpses();
            Assert.AreEqual(0, birds.CorpseCount);
            Assert.AreEqual(1, removed);
            // Flock leaves on the next daily tick, not immediately.
            Assert.IsTrue(birds.VulturesPresent);

            birds.TickDay();
            Assert.IsFalse(birds.VulturesPresent);
            Assert.AreEqual(0f, birds.GetHatchVisibility(), Eps);
            Assert.AreEqual(1, departures);
        }

        [Test]
        public void CorpseBuried_HostHook_AddsOutdoorCorpse_AndTickMarksDangerAndMorale()
        {
            // Mirrors GameBootstrap.WireCarrionBirds host pattern.
            var birds = new System_CarrionBirds();
            var hatch = new HatchVisibilitySystem();
            var map = new GeneratedMap
            {
                Seed = 1,
                Nodes = new List<MapNode>
                {
                    new MapNode { NodeId = GeneratedMap.ShelterNodeId, DangerLevel = 0f }
                }
            };

            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile, sv => true);
            var sv = new Survivor { Id = "s1", DisplayName = "Ada" };
            sv.Needs.Morale = 50f;
            var roster = new List<Survivor> { sv };

            // corpse buried → outdoor carrion
            Action onBuried = () => birds.AddCorpse();
            onBuried();
            Assert.AreEqual(1, birds.CorpseCount);

            // daily tick → vultures → hatch max + map danger + morale pressure
            birds.TickDay();
            Assert.IsTrue(birds.VulturesPresent);

            float target = birds.GetHatchVisibility();
            if (hatch.Visibility < target)
                hatch.AddVisibility(target - hatch.Visibility);
            Assert.AreEqual(1f, hatch.Visibility, Eps);

            var shelter = map.ShelterNode;
            Assert.IsNotNull(shelter);
            float dangerBefore = shelter.DangerLevel;
            shelter.DangerLevel += System_CarrionBirds.MapDangerBoost;
            Assert.AreEqual(dangerBefore + System_CarrionBirds.MapDangerBoost, shelter.DangerLevel, Eps);

            needs.Modify(sv, NeedKind.Morale, -System_CarrionBirds.MoralePressurePerDay);
            Assert.AreEqual(50f - System_CarrionBirds.MoralePressurePerDay, sv.Needs.Morale, Eps);

            UnityEngine.Object.DestroyImmediate(profile);
        }

        [Test]
        public void CaptureRestore_PreservesCorpsesAndVultureState()
        {
            var a = new System_CarrionBirds();
            a.AddCorpse();
            a.AddCorpse();
            a.TickDay();
            Assert.IsTrue(a.VulturesPresent);

            var save = a.CaptureState();
            Assert.AreEqual("system_carrion_birds", save.systemId);
            Assert.AreEqual(2, save.corpseCount);
            Assert.IsTrue(save.vulturesPresent);
            Assert.AreEqual(1f, save.hatchVisibilityOverride, Eps);

            // Mutate after capture — snapshot must stay frozen.
            a.RemoveCorpses();
            a.TickDay();
            Assert.AreEqual(2, save.corpseCount);
            Assert.IsTrue(save.vulturesPresent);

            var b = new System_CarrionBirds();
            b.RestoreState(save);
            Assert.AreEqual(2, b.CorpseCount);
            Assert.IsTrue(b.VulturesPresent);
            Assert.AreEqual(1f, b.GetHatchVisibility(), Eps);

            b.RestoreState(null);
            Assert.AreEqual(0, b.CorpseCount);
            Assert.IsFalse(b.VulturesPresent);
            Assert.AreEqual(0f, b.GetHatchVisibility(), Eps);
        }

        [Test]
        public void SaveSystemAdapter_CarrionBirdsSlot_RoundTrip()
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_carrion_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            try
            {
                var profile = ScriptableObject.CreateInstance<NeedsProfile>();
                var needs = new NeedsSystem(profile, sv => true);
                var weather = new WeatherSystem(null, 3);
                var temp = new TemperatureSystem(null, weather);
                var rad = new RadiationSystem(needs);

                var birdsA = new System_CarrionBirds();
                birdsA.AddCorpse();
                birdsA.AddCorpse();
                birdsA.TickDay();
                Assert.IsTrue(birdsA.VulturesPresent);
                Assert.AreEqual(2, birdsA.CorpseCount);

                SaveSystem Make(System_CarrionBirds birds)
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
                    ss.SetCarrionBirdsSystem(birds);
                    return ss;
                }

                Assert.IsTrue(Make(birdsA).Save("carrion_slot"));

                var birdsB = new System_CarrionBirds();
                Assert.IsTrue(Make(birdsB).Load("carrion_slot"));

                Assert.AreEqual(2, birdsB.CorpseCount);
                Assert.IsTrue(birdsB.VulturesPresent);
                Assert.AreEqual(1f, birdsB.GetHatchVisibility(), Eps);
                Assert.AreEqual("system_carrion_birds", birdsB.SystemId);

                UnityEngine.Object.DestroyImmediate(profile);
            }
            finally
            {
                try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { /* best-effort */ }
            }
        }
    }
}
