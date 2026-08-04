using NUnit.Framework;
using AtomicWar._Game.Survivors;
using System.Collections.Generic;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class SurvivorDiariesTests
    {
        private SurvivorDiariesSystem _diariesSystem;
        private Survivor _survivor;
        private MentalBreakSystem _mentalBreakSystem;

        [SetUp]
        public void SetUp()
        {
            _diariesSystem = new SurvivorDiariesSystem();
            _survivor = new Survivor
            {
                Id = "test_survivor",
                DisplayName = "Test Subject",
                RiskBias = RiskBiasTrait.Paranoid,
                PerceivedRadRisk = 0.8f
            };
            _mentalBreakSystem = new MentalBreakSystem();
        }

        [Test]
        public void PassiveEntryGeneration_AddsEntryToDiary()
        {
            var rng = new System.Random(42);
            _diariesSystem.GeneratePassiveEntry(_survivor, 1, rng);

            var entries = _diariesSystem.GetDiaryEntries("test_survivor");
            Assert.IsNotNull(entries);
            Assert.AreEqual(1, entries.Count);
            Assert.AreEqual(1, entries[0].DayCreated);
        }

        [Test]
        public void ReadDiary_WhenCaught_LowersTrustAndHidesItems()
        {
            var rng = new System.Random(42);
            var intel = _diariesSystem.ReadDiary(_survivor, sv => new List<string> { "radiation_burns" }, _mentalBreakSystem, rng, 0.0f);

            Assert.IsTrue(intel.WasCaught);
            Assert.IsTrue(_survivor.HasHiddenStash);
            Assert.IsNotNull(_survivor.HiddenItemIds);
            Assert.Greater(_survivor.HiddenItemIds.Count, 0);
            Assert.Less(_survivor.Needs.Morale, 50f);
            Assert.Contains("radiation_burns", intel.ActiveAfflictionNames);
        }

        [Test]
        public void ReadDiary_WhenNotCaught_RevealsIntelWithoutPenalty()
        {
            var rng = new System.Random(42);
            var intel = _diariesSystem.ReadDiary(_survivor, null, _mentalBreakSystem, rng, 0.99f);

            Assert.IsFalse(intel.WasCaught);
            Assert.IsFalse(_survivor.HasHiddenStash);
            Assert.AreEqual(RiskBiasTrait.Paranoid, intel.RiskBias);
        }
    }
}
