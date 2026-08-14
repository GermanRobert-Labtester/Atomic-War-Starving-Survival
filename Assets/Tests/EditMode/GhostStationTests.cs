using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using Ashfall.Core;
using Ashfall.Core.Journal;
using JournalSystem = AtomicWar._Game.Events.JournalSystem;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompt #19 — Ghost Stations: post-EMP only, GhostLoop intel, morale hit,
    /// optional diary chain, never plume/extraction/military unlock.
    /// </summary>
    [TestFixture]
    public class GhostStationTests
    {
        private const float Eps = 1e-3f;
        private List<Object> _toDestroy;
        private int _day;

        [SetUp]
        public void SetUp()
        {
            _day = 35;
            _toDestroy = new List<Object>();
        }

        [TearDown]
        public void TearDown()
        {
            if (_toDestroy == null) return;
            for (int i = 0; i < _toDestroy.Count; i++)
            {
                if (_toDestroy[i] != null)
                    Object.DestroyImmediate(_toDestroy[i]);
            }
            _toDestroy = null;
        }

        private static Survivor MakeSurvivor(string id = "s1", float morale = 60f)
        {
            var s = new Survivor
            {
                Id = id,
                DisplayName = id,
                State = SurvivorState.Idle
            };
            s.Needs.Morale = morale;
            s.Needs.Health = 80f;
            return s;
        }

        private (GhostStationSystem ghosts, RadioTunerSystem tuner, JournalSystem journal, List<Survivor> survivors)
            MakeStack(bool unlock = false)
        {
            var survivors = new List<Survivor> { MakeSurvivor() };
            var journal = new JournalSystem();
            var tuner = new RadioTunerSystem(new System.Random(11));
            tuner.State.AvailableFuel = 20f;
            tuner.State.EmpDamage = 0f;

            // Pre-EMP live band for contrast.
            var emergency = ScriptableObject.CreateInstance<RadioFrequencySO>();
            emergency.id = RadioFrequencySO.Ids.Emergency;
            emergency.displayName = "Emergency";
            emergency.frequencyMHz = 107f;
            emergency.type = RadioFrequencyType.Emergency;
            emergency.activeFromDay = 31;
            emergency.baseSignalStrength = 0.5f;
            emergency.broadcasts = new List<RadioBroadcastSO>();
            _toDestroy.Add(emergency);
            tuner.SetFrequencies(new[] { emergency });

            var ghosts = new GhostStationSystem();
            ghosts.Bind(
                tuner,
                journal,
                getSurvivors: () => survivors,
                getDay: () => _day);

            if (unlock)
                ghosts.NotifyEmpOccurred();

            return (ghosts, tuner, journal, survivors);
        }

        [Test]
        public void BeforeEmp_GhostsNotOnDial()
        {
            var (ghosts, tuner, _, _) = MakeStack(unlock: false);
            Assert.IsFalse(ghosts.IsUnlocked);
            Assert.IsNull(tuner.GetFrequency(RadioFrequencySO.Ids.GhostWeatherLoop));
            Assert.IsNull(tuner.GetFrequency(RadioFrequencySO.Ids.GhostDeadOperator));
            ghosts.Unbind();
            ghosts.DestroyRuntimeAssets();
        }

        [Test]
        public void NotifyEmp_UnlocksAndInjectsGhostFrequencies()
        {
            var (ghosts, tuner, _, _) = MakeStack(unlock: false);
            bool unlocked = false;
            ghosts.OnUnlocked += () => unlocked = true;

            Assert.IsTrue(ghosts.NotifyEmpOccurred());
            Assert.IsTrue(ghosts.IsUnlocked);
            Assert.IsTrue(unlocked);
            Assert.IsFalse(ghosts.NotifyEmpOccurred(), "Unlock is idempotent");

            Assert.IsNotNull(tuner.GetFrequency(RadioFrequencySO.Ids.GhostWeatherLoop));
            Assert.IsNotNull(tuner.GetFrequency(RadioFrequencySO.Ids.GhostDeadOperator));
            Assert.IsNotNull(tuner.GetFrequency(RadioFrequencySO.Ids.GhostCivilDefense));

            var weather = tuner.GetFrequency(RadioFrequencySO.Ids.GhostWeatherLoop);
            Assert.That(weather.type, Is.EqualTo(RadioFrequencyType.GhostStation));
            Assert.That(weather.baseSignalStrength, Is.LessThanOrEqualTo(0.35f));
            Assert.That(weather.ResolveInterceptChannelTag(), Is.Empty,
                "Ghost bands must not surface faction intercepts");

            ghosts.Unbind();
            ghosts.DestroyRuntimeAssets();
        }

        [Test]
        public void TuneGhost_ExtractsGhostLoop_NotPlumeOrMilitary()
        {
            var (ghosts, tuner, _, _) = MakeStack(unlock: true);
            IntelNode extracted = null;
            tuner.OnIntelExtracted += n => extracted = n;

            Assert.IsTrue(tuner.TuneToFrequency(RadioFrequencySO.Ids.GhostWeatherLoop));
            // BaseTuningHours = 2 at full signal; ghost signal is weak so give headroom.
            bool complete = tuner.Tick(12f, WeatherKind.Clear, _day);
            Assert.IsTrue(complete, "Tuning should complete with enough hours");
            Assert.IsNotNull(extracted);
            Assert.That(extracted.Type, Is.EqualTo(IntelType.GhostLoop));
            Assert.That(extracted.SourceFrequencyId, Is.EqualTo(RadioFrequencySO.Ids.GhostWeatherLoop));
            Assert.That(extracted.TargetLocationId, Is.Null.Or.Empty);
            Assert.That(extracted.Confidence, Is.LessThan(0.3f));
            Assert.IsFalse(VictoryProjectManager.IsMilitaryIntel(extracted));

            // Plume path must reject ghost intel.
            var map = new RadiationKnowledgeMap();
            Assert.IsFalse(tuner.ApplyPlumeReportToMap(extracted, map));

            ghosts.Unbind();
            ghosts.DestroyRuntimeAssets();
        }

        [Test]
        public void HearGhost_AppliesMoraleHit_Once()
        {
            var (ghosts, _, _, survivors) = MakeStack(unlock: true);
            float moraleBefore = survivors[0].Needs.Morale;

            Assert.IsTrue(ghosts.ApplyGhostHear(RadioFrequencySO.Ids.GhostWeatherLoop));
            Assert.That(survivors[0].Needs.Morale,
                Is.EqualTo(moraleBefore - GhostStationSystem.DefaultMoraleHit).Within(Eps));
            Assert.IsTrue(ghosts.HasHeard(RadioFrequencySO.Ids.GhostWeatherLoop));

            float afterFirst = survivors[0].Needs.Morale;
            Assert.IsFalse(ghosts.ApplyGhostHear(RadioFrequencySO.Ids.GhostWeatherLoop),
                "Second hear of same station is a no-op");
            Assert.That(survivors[0].Needs.Morale, Is.EqualTo(afterFirst).Within(Eps));

            ghosts.Unbind();
            ghosts.DestroyRuntimeAssets();
        }

        [Test]
        public void DeadOperatorGhost_UnlocksDiaryFragment_Once()
        {
            var (ghosts, _, journal, _) = MakeStack(unlock: true);
            JournalEntry added = null;
            journal.OnEntryAdded += e => added = e;

            Assert.IsTrue(ghosts.ApplyGhostHear(RadioFrequencySO.Ids.GhostDeadOperator));
            Assert.IsNotNull(added);
            Assert.That(added.KnowledgeKey, Is.EqualTo(GhostStationSystem.DiaryDeadOperatorKey));
            Assert.That(added.Text, Does.Contain("54.0").Or.Contain("shelter list").IgnoreCase);
            Assert.That(journal.EntryCount, Is.EqualTo(1));
            Assert.IsTrue(journal.Knowledge.Has(GhostStationSystem.DiaryDeadOperatorKey));

            Assert.IsFalse(ghosts.ApplyGhostHear(RadioFrequencySO.Ids.GhostDeadOperator));
            Assert.That(journal.EntryCount, Is.EqualTo(1), "Diary must not duplicate");

            ghosts.Unbind();
            ghosts.DestroyRuntimeAssets();
        }

        [Test]
        public void GhostIntel_DoesNotUnlockExtraction()
        {
            var victory = new VictoryProjectManager();
            var ghostIntel = IntelNode.CreateGhostLoop(
                RadioFrequencySO.Ids.GhostCivilDefense,
                _day,
                "Await further instructions.");

            Assert.IsFalse(victory.NotifyIntel(ghostIntel));
            Assert.That(victory.MilitaryIntelDecrypted, Is.EqualTo(0));
            Assert.IsFalse(victory.ExtractionUnlocked);
        }

        [Test]
        public void CreateGhostIntel_NeverPlumeOrMilitaryTypes()
        {
            var def = new GhostStationDef
            {
                Id = RadioFrequencySO.Ids.GhostCivilDefense,
                LoopText = "Remain indoors."
            };
            var intel = GhostStationSystem.CreateGhostIntel(def, _day);
            Assert.IsNotNull(intel);
            Assert.That(intel.Type, Is.EqualTo(IntelType.GhostLoop));
            Assert.That(intel.Type, Is.Not.EqualTo(IntelType.PlumeReport));
            Assert.That(intel.Type, Is.Not.EqualTo(IntelType.TroopMovement));
            Assert.That(intel.Type, Is.Not.EqualTo(IntelType.MortarWarning));
            Assert.IsFalse(VictoryProjectManager.IsMilitaryIntel(intel));
        }

        [Test]
        public void CaptureRestore_PreservesUnlockAndHeard()
        {
            var (ghosts, _, _, _) = MakeStack(unlock: true);
            ghosts.ApplyGhostHear(RadioFrequencySO.Ids.GhostWeatherLoop);
            ghosts.ApplyGhostHear(RadioFrequencySO.Ids.GhostCivilDefense);
            var save = ghosts.CaptureState();
            ghosts.Unbind();
            ghosts.DestroyRuntimeAssets();

            var (ghosts2, tuner2, _, _) = MakeStack(unlock: false);
            ghosts2.RestoreState(save);

            Assert.IsTrue(ghosts2.IsUnlocked);
            Assert.IsTrue(ghosts2.HasHeard(RadioFrequencySO.Ids.GhostWeatherLoop));
            Assert.IsTrue(ghosts2.HasHeard(RadioFrequencySO.Ids.GhostCivilDefense));
            Assert.IsNotNull(tuner2.GetFrequency(RadioFrequencySO.Ids.GhostDeadOperator),
                "Restore re-injects ghost frequencies into the bound tuner");

            ghosts2.Unbind();
            ghosts2.DestroyRuntimeAssets();
        }

        [Test]
        public void HearBeforeUnlock_DoesNothing()
        {
            var (ghosts, _, journal, survivors) = MakeStack(unlock: false);
            float morale = survivors[0].Needs.Morale;
            Assert.IsFalse(ghosts.ApplyGhostHear(RadioFrequencySO.Ids.GhostWeatherLoop));
            Assert.That(survivors[0].Needs.Morale, Is.EqualTo(morale).Within(Eps));
            Assert.That(journal.EntryCount, Is.EqualTo(0));
            ghosts.Unbind();
        }

        [Test]
        public void TunerCreateIntel_GhostStation_AlwaysGhostLoop()
        {
            // Direct path: inject a ghost freq, tune, extract.
            var (ghosts, tuner, _, _) = MakeStack(unlock: true);
            var list = new List<IntelNode>();
            tuner.OnIntelExtracted += n => list.Add(n);

            foreach (var id in new[]
                     {
                         RadioFrequencySO.Ids.GhostWeatherLoop,
                         RadioFrequencySO.Ids.GhostDeadOperator,
                         RadioFrequencySO.Ids.GhostCivilDefense
                     })
            {
                tuner.State.AvailableFuel = 50f;
                tuner.State.EmpDamage = 0f;
                Assert.IsTrue(tuner.TuneToFrequency(id));
                Assert.IsTrue(tuner.Tick(12f, WeatherKind.Clear, _day), $"Failed to tune {id}");
            }

            Assert.That(list.Count, Is.EqualTo(3));
            for (int i = 0; i < list.Count; i++)
            {
                Assert.That(list[i].Type, Is.EqualTo(IntelType.GhostLoop));
                Assert.IsFalse(VictoryProjectManager.IsMilitaryIntel(list[i]));
            }

            ghosts.Unbind();
            ghosts.DestroyRuntimeAssets();
        }
    }
}
