using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Victory path wiring: 14 endgame condition trackers (all Victory_* except TrueEnding)
    /// — API smoke + Capture/Restore + save-slot round-trips.
    /// </summary>
    [TestFixture]
    public class VictoryPathsWiringTests
    {
        private const float Eps = 1e-3f;

        private static string TempDir(string tag)
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_victory_" + tag + "_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            return dir;
        }

        private static SaveSystem MakeSave(string dir, Action<SaveSystem> wire)
        {
            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile, sv => true);
            var weather = new WeatherSystem(null, 3);
            var temp = new TemperatureSystem(null, weather);
            var rad = new RadiationSystem(needs);
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
            wire(ss);
            return ss;
        }

        // ── Airlift ────────────────────────────────────────────────────

        [Test]
        public void Airlift_DefenseExtract_CaptureRestore()
        {
            var a = new Victory_Airlift();
            a.StartDefense(4);
            a.TickRealTime(30f);
            a.ResolveWaveDefense(10f);
            Assert.IsFalse(a.IsVictoryAchieved());

            var save = a.CaptureState();
            Assert.IsTrue(save.isActive);
            Assert.AreEqual(4, save.survivorsOnRoof);
            Assert.AreEqual(1, save.wavesDefeated);
            Assert.Less(save.defenseTimerSeconds, 1440f);

            a.TickRealTime(2000f); // finish extract
            Assert.IsTrue(a.IsVictoryAchieved());

            var b = new Victory_Airlift();
            b.RestoreState(save);
            Assert.IsTrue(b.State.isActive);
            Assert.AreEqual(1, b.State.wavesDefeated);
            Assert.IsFalse(b.IsVictoryAchieved());
        }

        // ── Ascendancy ─────────────────────────────────────────────────

        [Test]
        public void Ascendancy_CheckVictory_Capture()
        {
            var v = new Victory_Ascendancy();
            bool fired = false;
            v.OnEndingTriggered += () => fired = true;

            Assert.IsFalse(v.CheckVictory(new List<(string, float, bool)>
            {
                ("a", 500f, true)
            }));

            Assert.IsTrue(v.CheckVictory(new List<(string, float, bool)>
            {
                ("a", 1000f, true),
                ("b", 1200f, true)
            }));
            Assert.IsTrue(fired);

            var save = v.CaptureState();
            Assert.AreEqual(1000f, save.radThreshold, Eps);
            var v2 = new Victory_Ascendancy();
            v2.RestoreState(save);
            Assert.AreEqual(1000f, v2.State.radThreshold, Eps);
        }

        // ── Buried Alive ───────────────────────────────────────────────

        [Test]
        public void BuriedAlive_SealWithSystems_RoundTrip()
        {
            var v = new Victory_BuriedAlive();
            Assert.IsTrue(v.SealAndDetonate(true, true));
            Assert.IsTrue(v.State.triggered);
            Assert.IsTrue(v.CanSurviveIndefinitely());

            var save = v.CaptureState();
            var v2 = new Victory_BuriedAlive();
            v2.RestoreState(save);
            Assert.IsTrue(v2.State.isSealed);
            Assert.IsTrue(v2.State.triggered);
            Assert.IsTrue(v2.CanSurviveIndefinitely());
        }

        // ── Cannibal King ──────────────────────────────────────────────

        [Test]
        public void CannibalKing_Thresholds_SaveSlot()
        {
            string dir = TempDir("cannibal");
            try
            {
                var a = new Victory_CannibalKing();
                a.TrackUsage(50, 30);
                a.MarkWarlordsDefeated();
                Assert.IsTrue(a.CheckVictory());

                Assert.IsTrue(MakeSave(dir, ss => ss.SetVictoryCannibalKingSystem(a)).Save("slot"));
                var b = new Victory_CannibalKing();
                Assert.IsTrue(MakeSave(dir, ss => ss.SetVictoryCannibalKingSystem(b)).Load("slot"));
                Assert.IsTrue(b.State.triggered);
                Assert.AreEqual(50, b.State.humanMeatMealsUsed);
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }

        // ── Defection ──────────────────────────────────────────────────

        [Test]
        public void Defection_Surrender_Capture()
        {
            var v = new Victory_Defection();
            Assert.IsFalse(v.Surrender(3, true)); // siege too low
            Assert.IsTrue(v.Surrender(5, true));
            Assert.IsTrue(v.State.isGameOver);

            var save = v.CaptureState();
            var v2 = new Victory_Defection();
            v2.RestoreState(save);
            Assert.IsTrue(v2.State.isSurrendered);
            Assert.IsFalse(v2.Surrender(5, true)); // already over
        }

        // ── Icebreaker ─────────────────────────────────────────────────

        [Test]
        public void Icebreaker_ContactDeliverExtract()
        {
            var v = new Victory_Icebreaker();
            Assert.IsTrue(v.ContactSubmarine(true));
            v.DeliverExplosives(100);
            Assert.IsTrue(v.CheckExtraction(10, 100, factionArmyArrived: false));
            Assert.IsTrue(v.IsVictoryAchieved());

            var save = v.CaptureState();
            var v2 = new Victory_Icebreaker();
            v2.RestoreState(save);
            Assert.IsTrue(v2.State.isExtracted);
            Assert.AreEqual(100, v2.State.explosivesDelivered);
        }

        // ── Lone Survivor ──────────────────────────────────────────────

        [Test]
        public void LoneSurvivor_Day100_Capture()
        {
            var v = new Victory_LoneSurvivor();
            Assert.IsFalse(v.CheckVictory(99, 1));
            Assert.IsTrue(v.CheckVictory(100, 1));
            Assert.IsTrue(v.State.triggered);

            var save = v.CaptureState();
            var v2 = new Victory_LoneSurvivor();
            v2.RestoreState(save);
            Assert.IsTrue(v2.State.triggered);
            Assert.IsTrue(v2.CheckVictory(200, 5)); // already triggered stays true
        }

        // ── MAD ────────────────────────────────────────────────────────

        [Test]
        public void MAD_Fire_CaptureRestore()
        {
            var v = new Victory_MAD();
            Assert.IsFalse(v.FireAtOwnCoordinates(false, true));
            Assert.IsTrue(v.FireAtOwnCoordinates(true, true));
            Assert.IsTrue(v.IsVictoryAchieved());

            var save = v.CaptureState();
            var v2 = new Victory_MAD();
            v2.RestoreState(save);
            Assert.IsTrue(v2.State.isTriggered);
            Assert.IsFalse(v2.FireAtOwnCoordinates(true, true)); // already fired
        }

        // ── Migration ──────────────────────────────────────────────────

        [Test]
        public void Migration_Abandon_Capture()
        {
            var v = new Victory_Migration();
            Assert.IsTrue(v.CheckVictory(true, true, 3));
            Assert.IsTrue(v.IsVictoryAchieved());

            var save = v.CaptureState();
            var v2 = new Victory_Migration();
            v2.RestoreState(save);
            Assert.IsTrue(v2.State.bunkerAbandoned);
        }

        // ── The Broadcast ──────────────────────────────────────────────

        [Test]
        public void Broadcast_UploadComplete_RoundTrip()
        {
            var v = new Victory_TheBroadcast();
            Assert.IsTrue(v.StartUpload(true, true));
            for (int i = 0; i < 25; i++)
                v.TickMinute(100f, null); // no rng → no assault slowdown
            Assert.IsTrue(v.IsUploadComplete());

            var save = v.CaptureState();
            Assert.IsTrue(save.isUploadComplete);
            Assert.AreEqual(100f, save.uploadProgress, Eps);

            var v2 = new Victory_TheBroadcast();
            v2.RestoreState(save);
            Assert.IsTrue(v2.IsVictoryAchieved());
        }

        // ── The Cure ───────────────────────────────────────────────────

        [Test]
        public void TheCure_AllPrereqs_SaveSlot()
        {
            string dir = TempDir("cure");
            try
            {
                var a = new Victory_TheCure();
                Assert.IsTrue(a.CheckVictory(true, true, true));
                Assert.IsTrue(a.State.formulaBroadcast);

                Assert.IsTrue(MakeSave(dir, ss => ss.SetVictoryTheCureSystem(a)).Save("slot"));
                var b = new Victory_TheCure();
                Assert.IsTrue(MakeSave(dir, ss => ss.SetVictoryTheCureSystem(b)).Load("slot"));
                Assert.IsTrue(b.State.triggered);
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }

        // ── The Martian ────────────────────────────────────────────────

        [Test]
        public void TheMartian_Launch_Capture()
        {
            var v = new Victory_TheMartian();
            Assert.IsTrue(v.CheckVictory(true, 100f, 50, 4));
            int launched = v.Launch(100f, 50, 4);
            Assert.AreEqual(3, launched);
            Assert.AreEqual(3, v.State.survivorsLaunched);

            var save = v.CaptureState();
            var v2 = new Victory_TheMartian();
            v2.RestoreState(save);
            Assert.IsTrue(v2.State.triggered);
            Assert.AreEqual(3, v2.State.survivorsLaunched);
        }

        // ── Underground City ───────────────────────────────────────────

        [Test]
        public void UndergroundCity_ActivateSeal_Capture()
        {
            var v = new Victory_UndergroundCity();
            Assert.IsTrue(v.TryActivate(true, true, 20));
            v.SealHatch();
            Assert.IsTrue(v.IsVictoryAchieved());

            var save = v.CaptureState();
            var v2 = new Victory_UndergroundCity();
            v2.RestoreState(save);
            Assert.IsTrue(v2.State.hatchSealed);
            Assert.IsTrue(v2.IsVictoryAchieved());
        }

        // ── Unifier ────────────────────────────────────────────────────

        [Test]
        public void Unifier_AllTrust_Capture()
        {
            var v = new Victory_Unifier();
            bool fired = false;
            v.OnEndingTriggered += () => fired = true;

            Assert.IsFalse(v.CheckVictory(new Dictionary<string, float>
            {
                { "faction_traders", 0.5f },
                { "faction_terrorist", 0f }
            }));

            Assert.IsTrue(v.CheckVictory(new Dictionary<string, float>
            {
                { "faction_traders", 1f },
                { "faction_farmers", 1f },
                { "faction_terrorist", 0f }
            }));
            Assert.IsTrue(fired);

            var save = v.CaptureState();
            Assert.AreEqual(1f, save.trustRequired, Eps);
            var v2 = new Victory_Unifier();
            v2.RestoreState(save);
            Assert.AreEqual(1f, v2.State.trustRequired, Eps);
        }

        // ── Multi-slot save (several victories in one file) ─────────────

        [Test]
        public void MultiVictory_SaveSlot_RoundTrip()
        {
            string dir = TempDir("multi");
            try
            {
                var airlift = new Victory_Airlift();
                airlift.StartDefense(2);
                airlift.TickRealTime(100f);

                var mad = new Victory_MAD();
                mad.FireAtOwnCoordinates(true, true);

                var city = new Victory_UndergroundCity();
                city.CheckProgress(true, false, 10);

                void Wire(SaveSystem ss)
                {
                    ss.SetVictoryAirliftSystem(airlift);
                    ss.SetVictoryMadSystem(mad);
                    ss.SetVictoryUndergroundCitySystem(city);
                }

                Assert.IsTrue(MakeSave(dir, Wire).Save("slot"));

                var airlift2 = new Victory_Airlift();
                var mad2 = new Victory_MAD();
                var city2 = new Victory_UndergroundCity();
                Assert.IsTrue(MakeSave(dir, ss =>
                {
                    ss.SetVictoryAirliftSystem(airlift2);
                    ss.SetVictoryMadSystem(mad2);
                    ss.SetVictoryUndergroundCitySystem(city2);
                }).Load("slot"));

                Assert.IsTrue(airlift2.State.isActive);
                Assert.AreEqual(2, airlift2.State.survivorsOnRoof);
                Assert.IsTrue(mad2.State.isTriggered);
                Assert.IsTrue(city2.State.geothermalTapUpgraded);
                Assert.AreEqual(10, city2.State.roomsExcavated);
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }
    }
}
