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
    /// Prompt #861 — adaptive warlords: strategy record, top-3 counters, gear, save slot.
    /// </summary>
    [TestFixture]
    public class AdaptiveWarlordsTests
    {
        [Test]
        public void RecordStrategy_IncrementsAndFiresEvent()
        {
            var sys = new System_AdaptiveWarlords();
            string gotId = null;
            int gotCount = 0;
            sys.OnStrategyRecorded += (id, n) =>
            {
                gotId = id;
                gotCount = n;
            };

            sys.RecordStrategy(System_AdaptiveWarlords.StrategySnipers);
            sys.RecordStrategy(System_AdaptiveWarlords.StrategySnipers);
            Assert.AreEqual(System_AdaptiveWarlords.StrategySnipers, gotId);
            Assert.AreEqual(2, gotCount);
            Assert.AreEqual(2, sys.GetStrategyUseCount(System_AdaptiveWarlords.StrategySnipers));

            sys.RecordStrategy(null);
            sys.RecordStrategy("");
            Assert.AreEqual(2, sys.GetStrategyUseCount(System_AdaptiveWarlords.StrategySnipers));
        }

        [Test]
        public void OnPlaythroughEnd_KeepsTop3_AndBuildsCounters()
        {
            var sys = new System_AdaptiveWarlords();
            // 4 strategies; traps highest, then snipers, stealth, turrets lowest.
            for (int i = 0; i < 5; i++) sys.RecordStrategy(System_AdaptiveWarlords.StrategyTraps);
            for (int i = 0; i < 4; i++) sys.RecordStrategy(System_AdaptiveWarlords.StrategySnipers);
            for (int i = 0; i < 3; i++) sys.RecordStrategy(System_AdaptiveWarlords.StrategyStealth);
            for (int i = 0; i < 1; i++) sys.RecordStrategy(System_AdaptiveWarlords.StrategyTurrets);

            var applied = new List<string>();
            sys.OnCounterApplied += (strat, counter) => applied.Add(strat + "→" + counter);
            bool ready = false;
            sys.OnPlaythroughCountersReady += () => ready = true;

            sys.OnPlaythroughEnd();

            Assert.IsTrue(ready);
            Assert.AreEqual(3, sys.TrackedStrategyCount, "Top 3 only");
            Assert.AreEqual(3, sys.ActiveCounterCount);
            Assert.AreEqual(0, sys.GetStrategyUseCount(System_AdaptiveWarlords.StrategyTurrets),
                "Lowest strategy trimmed from tracked list");
            Assert.IsTrue(sys.HasGearModifier(System_AdaptiveWarlords.CounterSappers));
            Assert.IsTrue(sys.HasGearModifier(System_AdaptiveWarlords.CounterSmokeKevlar));
            Assert.IsTrue(sys.HasGearModifier(System_AdaptiveWarlords.CounterDogs));
            Assert.IsFalse(sys.HasGearModifier(System_AdaptiveWarlords.CounterEmp));
            Assert.AreEqual(3, applied.Count);
        }

        [Test]
        public void GetCounterStrategy_KnownAndUnknown()
        {
            var sys = new System_AdaptiveWarlords();
            Assert.AreEqual(System_AdaptiveWarlords.CounterSmokeKevlar,
                sys.GetCounterStrategy(System_AdaptiveWarlords.StrategySnipers));
            Assert.AreEqual(System_AdaptiveWarlords.CounterEmp,
                sys.GetCounterStrategy(System_AdaptiveWarlords.StrategyTurrets));
            Assert.AreEqual(System_AdaptiveWarlords.CounterDogs,
                sys.GetCounterStrategy(System_AdaptiveWarlords.StrategyStealth));
            Assert.AreEqual(System_AdaptiveWarlords.CounterSappers,
                sys.GetCounterStrategy(System_AdaptiveWarlords.StrategyTraps));
            Assert.AreEqual(string.Empty, sys.GetCounterStrategy("unknown_strat"));
            Assert.AreEqual(string.Empty, sys.GetCounterStrategy(null));
        }

        [Test]
        public void GetWarlordGear_AppendsModifiers_AndFiresEvents()
        {
            var sys = new System_AdaptiveWarlords();
            sys.RecordStrategy(System_AdaptiveWarlords.StrategySnipers);
            sys.RecordStrategy(System_AdaptiveWarlords.StrategyStealth);
            sys.OnPlaythroughEnd();

            var mods = new List<string>();
            sys.OnWarlordGearModified += (baseGear, mod) => mods.Add(mod);

            string gear = sys.GetWarlordGear("rifle");
            StringAssert.StartsWith("rifle+", gear);
            StringAssert.Contains(System_AdaptiveWarlords.CounterSmokeKevlar, gear);
            StringAssert.Contains(System_AdaptiveWarlords.CounterDogs, gear);
            Assert.AreEqual(2, mods.Count);

            Assert.AreEqual("standard", new System_AdaptiveWarlords().GetWarlordGear("standard"));
        }

        [Test]
        public void CaptureRestore_DeepCopy_PreservesCounters()
        {
            var a = new System_AdaptiveWarlords();
            a.RecordStrategy(System_AdaptiveWarlords.StrategyTurrets);
            a.RecordStrategy(System_AdaptiveWarlords.StrategyTurrets);
            a.RecordStrategy(System_AdaptiveWarlords.StrategyTraps);
            a.OnPlaythroughEnd();

            var save = a.CaptureState();
            Assert.AreEqual("system_adaptive_warlords", save.system_id);
            Assert.AreEqual(2, save.previous_strategies.Count);
            Assert.Greater(save.warlord_gear_modifiers.Count, 0);

            // Mutate after capture must not touch snapshot.
            a.RecordStrategy(System_AdaptiveWarlords.StrategySnipers);
            Assert.AreEqual(2, save.previous_strategies.Count);

            var b = new System_AdaptiveWarlords();
            b.RestoreState(save);
            Assert.AreEqual(2, b.TrackedStrategyCount);
            Assert.AreEqual(2, b.GetStrategyUseCount(System_AdaptiveWarlords.StrategyTurrets));
            Assert.IsTrue(b.HasGearModifier(System_AdaptiveWarlords.CounterEmp));
            Assert.IsTrue(b.HasGearModifier(System_AdaptiveWarlords.CounterSappers));
            Assert.IsFalse(b.HasGearModifier(System_AdaptiveWarlords.CounterSmokeKevlar));
            StringAssert.Contains("emp", b.GetWarlordGear("base"));
        }

        [Test]
        public void RestoreNull_Resets()
        {
            var sys = new System_AdaptiveWarlords();
            sys.RecordStrategy(System_AdaptiveWarlords.StrategySnipers);
            sys.OnPlaythroughEnd();
            sys.RestoreState(null);
            Assert.AreEqual(0, sys.TrackedStrategyCount);
            Assert.AreEqual(0, sys.ActiveCounterCount);
            Assert.AreEqual("standard", sys.GetWarlordGear("standard"));
        }

        [Test]
        public void SaveSystemAdapter_AdaptiveWarlordsSlot_RoundTrip()
        {
            string dir = SaveSystemTestFactory.TempDir("warlords");
            try
            {
                var warA = new System_AdaptiveWarlords();
                warA.RecordStrategy(System_AdaptiveWarlords.StrategyStealth);
                warA.RecordStrategy(System_AdaptiveWarlords.StrategyStealth);
                warA.RecordStrategy(System_AdaptiveWarlords.StrategySnipers);
                warA.OnPlaythroughEnd();
                string gearA = warA.GetWarlordGear("kit");

                SaveSystem Make(System_AdaptiveWarlords war) =>
                    SaveSystemTestFactory.MakeSave(dir, ss => { ss.SetAdaptiveWarlordsSystem(war); });

                Assert.IsTrue(Make(warA).Save("warlords_slot"));

                var warB = new System_AdaptiveWarlords();
                Assert.IsTrue(Make(warB).Load("warlords_slot"));

                Assert.AreEqual(2, warB.TrackedStrategyCount);
                Assert.AreEqual(2, warB.GetStrategyUseCount(System_AdaptiveWarlords.StrategyStealth));
                Assert.IsTrue(warB.HasGearModifier(System_AdaptiveWarlords.CounterDogs));
                Assert.IsTrue(warB.HasGearModifier(System_AdaptiveWarlords.CounterSmokeKevlar));
                Assert.AreEqual(gearA, warB.GetWarlordGear("kit"));
            }
            finally
            {
                try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { /* best-effort */ }
            }
        }
    }
}
