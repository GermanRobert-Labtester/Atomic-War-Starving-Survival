using NUnit.Framework;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.AI.Actions;
using UnityEngine;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class SleepwalkingTests
    {
        private InternalLockSystem _lockSystem;
        private SleepwalkActionSO _sleepwalkAction;
        private Survivor _survivor;

        [SetUp]
        public void SetUp()
        {
            _lockSystem = new InternalLockSystem();
            _sleepwalkAction = ScriptableObject.CreateInstance<SleepwalkActionSO>();
            _survivor = new Survivor
            {
                Id = "sleepwalker",
                DisplayName = "Somnambulist",
                RadiationAnxiety = 0.8f,
                State = SurvivorState.Resting,
                CurrentRoomId = "quarters"
            };
        }

        [Test]
        public void ScoreAction_HighRadiationAnxiety_ReturnsHighUtilityScore()
        {
            float score = _sleepwalkAction.ScoreAction(_survivor, _lockSystem);
            Assert.Greater(score, 0.8f);
        }

        [Test]
        public void ExecuteSleepwalk_WhenDoorLocked_IsBlocked()
        {
            _lockSystem.SetDoorLock("quarters", true);
            var rng = new System.Random(42);

            bool executed = _sleepwalkAction.ExecuteSleepwalk(_survivor, _lockSystem, rng, out var hazard);

            Assert.IsFalse(executed);
        }

        [Test]
        public void ExecuteSleepwalk_WhenGuarded_IsBlocked()
        {
            _lockSystem.AssignGuard("quarters", "guard_survivor");
            var rng = new System.Random(42);

            bool executed = _sleepwalkAction.ExecuteSleepwalk(_survivor, _lockSystem, rng, out var hazard);

            Assert.IsFalse(executed);
        }

        [Test]
        public void ExecuteSleepwalk_Unblocked_ExecutesHazardAndIncursCost()
        {
            var rng = new System.Random(42);
            float initialRad = _survivor.RadiationDose;

            bool executed = _sleepwalkAction.ExecuteSleepwalk(_survivor, _lockSystem, rng, out var hazard);

            Assert.IsTrue(executed);
        }
    }
}
