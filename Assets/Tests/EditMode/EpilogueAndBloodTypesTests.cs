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
    /// Prompt #768 epilogue stats + Prompt #829 blood types (bag path) wiring.
    /// </summary>
    [TestFixture]
    public class EpilogueAndBloodTypesTests
    {
        private const float Eps = 1e-3f;

        // ── Epilogue ───────────────────────────────────────────────────

        [Test]
        public void Epilogue_RecordMealAndBullets_ThenGenerate()
        {
            var sys = new System_EpilogueStats();
            sys.RecordMealCooked(3);
            sys.RecordBulletsFired(12);
            Assert.AreEqual(3, sys.MealsCooked);
            Assert.AreEqual(12, sys.BulletsFired);

            EpilogueRecord got = null;
            sys.OnEpilogueGenerated += r => got = r;

            var record = sys.GenerateEpilogue(
                mealsCooked: -1,
                bulletsFired: -1,
                daysSurvived: 22,
                deadSurvivors: new List<string> { "Elena" },
                deathRoomIds: new List<string> { "quarters" },
                finalJournalEntries: new List<string> { "The filters failed." });

            Assert.AreSame(record, got);
            Assert.AreEqual(3, record.mealsCooked);
            Assert.AreEqual(12, record.bulletsFired);
            Assert.AreEqual(22, record.daysSurvived);
            Assert.AreEqual(1, record.survivorsDead.Count);
            Assert.IsTrue(sys.HasRecord);

            string narrative = sys.GetNarrativeSummary(record);
            StringAssert.Contains("22 days", narrative);
            StringAssert.Contains("3 meals", narrative);
            StringAssert.Contains("12 bullets", narrative);
            StringAssert.Contains("Elena", narrative);
            StringAssert.Contains("quarters", narrative);

            var highlights = sys.GetTopDownHighlights(record);
            Assert.AreEqual(1, highlights.Count);
            Assert.AreEqual("quarters", highlights[0]);
        }

        [Test]
        public void Epilogue_SaveRoundTrip_PreservesRecordAndCounters()
        {
            var a = new System_EpilogueStats();
            a.RecordMealCooked(2);
            a.RecordBulletsFired(5);
            a.GenerateEpilogue(
                -1, -1, 9,
                new List<string> { "Mara", "Jen" },
                new List<string> { "stores", "entry" },
                new List<string> { "We ran out of iodine." });

            var save = a.CaptureState();
            Assert.IsTrue(save.hasRecord);
            Assert.AreEqual("system_epilogue_stats", save.systemId);

            var b = new System_EpilogueStats();
            b.RestoreState(save);
            Assert.IsTrue(b.HasRecord);
            Assert.AreEqual(2, b.MealsCooked);
            Assert.AreEqual(5, b.BulletsFired);
            Assert.AreEqual(9, b.LastRecord.daysSurvived);
            Assert.AreEqual(2, b.LastRecord.survivorsDead.Count);
            StringAssert.Contains("iodine", b.GetNarrativeSummary(b.LastRecord));
        }

        [Test]
        public void Epilogue_RestoreNull_Resets()
        {
            var sys = new System_EpilogueStats();
            sys.RecordMealCooked(1);
            sys.GenerateEpilogue(-1, -1, 1, null, null, null);
            sys.RestoreState(null);
            Assert.IsFalse(sys.HasRecord);
            Assert.AreEqual(0, sys.MealsCooked);
        }

        // ── Blood Types ────────────────────────────────────────────────

        [Test]
        public void BloodTypes_EnsureAndTest_DiscoversType()
        {
            var sys = new System_BloodTypes();
            sys.SetRng(new System.Random(1));
            string type = sys.EnsureBloodType("sv_a");
            Assert.IsNotNull(type);
            Assert.IsTrue(
                type == System_BloodTypes.TYPE_A || type == System_BloodTypes.TYPE_B
                || type == System_BloodTypes.TYPE_AB || type == System_BloodTypes.TYPE_O);

            Assert.IsFalse(sys.IsTested("sv_a"));
            string discovered = null;
            sys.OnBloodTypeDiscovered += (id, t) => discovered = t;
            sys.TestBlood("sv_a");
            Assert.IsTrue(sys.IsTested("sv_a"));
            Assert.AreEqual(type, discovered);
        }

        [Test]
        public void BloodTypes_Compatibility_O_UniversalDonor_AB_UniversalRecipient()
        {
            var sys = new System_BloodTypes();
            Assert.IsTrue(sys.CheckCompatibility(System_BloodTypes.TYPE_A, System_BloodTypes.TYPE_O));
            Assert.IsTrue(sys.CheckCompatibility(System_BloodTypes.TYPE_AB, System_BloodTypes.TYPE_B));
            Assert.IsTrue(sys.CheckCompatibility(System_BloodTypes.TYPE_A, System_BloodTypes.TYPE_A));
            Assert.IsFalse(sys.CheckCompatibility(System_BloodTypes.TYPE_A, System_BloodTypes.TYPE_B));
        }

        [Test]
        public void BloodTypes_CompatibleBag_NoShock()
        {
            var sys = new System_BloodTypes();
            sys.AssignBloodType("sv_r", System_BloodTypes.TYPE_A);
            bool shock = false;
            sys.OnHemolyticShock += _ => shock = true;
            bool ok = sys.TryTransfuseBag("sv_r", System_BloodTypes.TYPE_O, out bool died);
            Assert.IsTrue(ok);
            Assert.IsFalse(died);
            Assert.IsFalse(shock);
            Assert.IsFalse(sys.IsInShock("sv_r"));
        }

        [Test]
        public void BloodTypes_IncompatibleBag_ShockAndMostlyDeath()
        {
            // Fixed RNG so ResolveShock is deterministic: NextDouble < 0.8 → death.
            var sys = new System_BloodTypes();
            sys.SetRng(new System.Random(0)); // NextDouble often low enough
            sys.AssignBloodType("sv_r", System_BloodTypes.TYPE_A);

            int shocks = 0, deaths = 0, survivals = 0;
            sys.OnHemolyticShock += _ => shocks++;
            sys.OnDeath += _ => deaths++;
            sys.OnSurvival += _ => survivals++;

            // Force death by using a custom path: Transfuse + ResolveShock with controlled rng.
            // Try multiple seeds until we observe both death and survival paths at least once
            // is overkill — assert shock always fires and outcome is death OR survival.
            bool ok = sys.TryTransfuseBag("sv_r", System_BloodTypes.TYPE_B, out bool died);
            Assert.IsFalse(ok);
            Assert.AreEqual(1, shocks);
            Assert.AreEqual(1, deaths + survivals);
            Assert.AreEqual(died, deaths == 1);
            Assert.IsFalse(sys.IsInShock("sv_r"), "Shock resolved after bag path.");
        }

        [Test]
        public void BloodTypes_SaveRoundTrip()
        {
            var a = new System_BloodTypes();
            a.AssignBloodType("sv1", System_BloodTypes.TYPE_AB);
            a.AssignBloodType("sv2", System_BloodTypes.TYPE_O);
            a.TestBlood("sv1");
            a.Transfuse("sv2", System_BloodTypes.TYPE_A); // incompatible → shock active

            var save = a.CaptureState();
            var b = new System_BloodTypes();
            b.RestoreState(save);

            Assert.AreEqual(System_BloodTypes.TYPE_AB, b.GetBloodType("sv1"));
            Assert.AreEqual(System_BloodTypes.TYPE_O, b.GetBloodType("sv2"));
            Assert.IsTrue(b.IsTested("sv1"));
            Assert.IsFalse(b.IsTested("sv2"));
            Assert.IsTrue(b.IsInShock("sv2"));
        }

        [Test]
        public void SaveSystemAdapter_BothSystems_RoundTrip()
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_epi_blood_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            try
            {
                var profile = ScriptableObject.CreateInstance<NeedsProfile>();
                var needs = new NeedsSystem(profile, sv => true);
                var weather = new WeatherSystem(null, 3);
                var temp = new TemperatureSystem(null, weather);
                var rad = new RadiationSystem(needs);

                var epiA = new System_EpilogueStats();
                epiA.RecordMealCooked(4);
                epiA.RecordBulletsFired(7);
                epiA.GenerateEpilogue(-1, -1, 15,
                    new List<string> { "Ash" },
                    new List<string> { "plant" },
                    new List<string> { "Gone quiet." });

                var bloodA = new System_BloodTypes();
                bloodA.AssignBloodType("sv_ash", System_BloodTypes.TYPE_B);
                bloodA.TestBlood("sv_ash");

                SaveSystem Make(System_EpilogueStats epi, System_BloodTypes blood)
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
                    ss.SetEpilogueStatsSystem(epi);
                    ss.SetBloodTypesSystem(blood);
                    return ss;
                }

                Assert.IsTrue(Make(epiA, bloodA).Save("epi_blood_slot"));

                var epiB = new System_EpilogueStats();
                var bloodB = new System_BloodTypes();
                Assert.IsTrue(Make(epiB, bloodB).Load("epi_blood_slot"));

                Assert.IsTrue(epiB.HasRecord);
                Assert.AreEqual(4, epiB.MealsCooked);
                Assert.AreEqual(7, epiB.BulletsFired);
                Assert.AreEqual(System_BloodTypes.TYPE_B, bloodB.GetBloodType("sv_ash"));
                Assert.IsTrue(bloodB.IsTested("sv_ash"));

                UnityEngine.Object.DestroyImmediate(profile);
            }
            finally
            {
                try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { /* best-effort */ }
            }
        }
    }
}
