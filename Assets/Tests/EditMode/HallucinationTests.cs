using NUnit.Framework;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using UnityEngine;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class HallucinationTests
    {
        private HallucinationSystem _hallucinationSystem;
        private PhantomActionSO _phantomAction;
        private Survivor _sickSurvivor;

        [SetUp]
        public void SetUp()
        {
            _hallucinationSystem = new HallucinationSystem();
            _phantomAction = ScriptableObject.CreateInstance<PhantomActionSO>();

            _sickSurvivor = new Survivor
            {
                Id = "sick_guy",
                DisplayName = "Hallucinating Patient",
                RadiationAnxiety = 0.8f
            };
            _sickSurvivor.Needs.Health = 20f;
            _sickSurvivor.Needs.Thirst = 50f;
        }

        [Test]
        public void Tick_LowHealthOrAnxiety_SpawnsPhantomItem()
        {
            var rng = new System.Random(42);
            var item = _hallucinationSystem.SpawnPhantomForSurvivor(_sickSurvivor, rng);

            Assert.IsNotNull(item);
            Assert.AreEqual(_sickSurvivor.Id, item.TargetSurvivorId);
            Assert.AreEqual(1, _hallucinationSystem.GetPhantomsForSurvivor("sick_guy").Count);
        }

        [Test]
        public void ConsumePhantomItem_GrantsZeroBenefits()
        {
            var rng = new System.Random(42);
            var item = _hallucinationSystem.SpawnPhantomForSurvivor(_sickSurvivor, rng);

            float initialThirst = _sickSurvivor.Needs.Thirst;
            float initialHealth = _sickSurvivor.Needs.Health;

            bool consumed = _hallucinationSystem.ConsumePhantomItem(_sickSurvivor, item.Id);

            Assert.IsTrue(consumed);
            Assert.AreEqual(initialThirst, _sickSurvivor.Needs.Thirst); // 0 hydration gain
            Assert.AreEqual(initialHealth, _sickSurvivor.Needs.Health); // 0 health gain
            Assert.AreEqual(0, _hallucinationSystem.GetPhantomsForSurvivor("sick_guy").Count);
        }

        [Test]
        public void PhantomAction_ScoresZero_WhenSurvivorHasNoActivePhantom()
        {
            // Qualifies on health/anxiety, but HallucinationSystem never rolled a spawn:
            // without the H-6a fix this scored 0.85 anyway and the AI picked a no-op action.
            float score = _phantomAction.ScoreAction(_sickSurvivor, _hallucinationSystem);

            Assert.AreEqual(0f, score);
        }

        [Test]
        public void PhantomAction_Execute_ConsumesTheSurvivorsActivePhantom()
        {
            var rng = new System.Random(42);
            var item = _hallucinationSystem.SpawnPhantomForSurvivor(_sickSurvivor, rng);

            float score = _phantomAction.ScoreAction(_sickSurvivor, _hallucinationSystem);
            Assert.Greater(score, 0f);

            string raisedId = null;
            _phantomAction.OnPhantomActionAttempted += (sv, id) => raisedId = id;

            bool executed = _phantomAction.ExecutePhantomAction(_sickSurvivor, _hallucinationSystem, out string message);

            Assert.IsTrue(executed);
            Assert.AreEqual(item.Id, raisedId);
            StringAssert.Contains(item.DisplayName, message);
            Assert.AreEqual(0, _hallucinationSystem.GetPhantomsForSurvivor("sick_guy").Count);
        }
    }
}
