using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Radiation;
using Random = System.Random;
using Ashfall.Core.Journal;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class DrinkContaminatedWaterActionTests
    {
        private const float Eps = 1e-4f;

        private DrinkContaminatedWaterActionSO _action;
        private NeedsSystem _needsSystem;
        private RadiationSystem _radiationSystem;

        [SetUp]
        public void SetUp()
        {
            _action = ScriptableObject.CreateInstance<DrinkContaminatedWaterActionSO>();
            _action.ThirstRestore = 35f;
            _action.MinThirstToConsider = 60f;
            _action.IrradiatedDoseAmount = 25f;
            _action.DirtyWaterIllnessChance = 0f;

            _needsSystem = new NeedsSystem(ScriptableObject.CreateInstance<NeedsProfile>());
            _radiationSystem = new RadiationSystem(_needsSystem);
        }

        private static Survivor MakeSurvivor(float thirst, RiskBiasTrait riskBias)
        {
            var survivor = new Survivor { Id = "s1", DisplayName = "Test Survivor", RiskBias = riskBias };
            survivor.Needs.Thirst = thirst;
            return survivor;
        }

        [Test]
        public void EvaluateRaw_NoContaminatedWaterAvailable_ScoresZero()
        {
            var survivor = MakeSurvivor(90f, RiskBiasTrait.Reckless);
            var storage = new WaterStorage();
            var context = new AIContext { Survivor = survivor, WaterStorage = storage, Random = new Random(1) };

            Assert.That(_action.EvaluateRaw(context), Is.EqualTo(0f).Within(Eps));
        }

        [Test]
        public void EvaluateRaw_CleanWaterAvailable_ScoresZero()
        {
            var survivor = MakeSurvivor(90f, RiskBiasTrait.Reckless);
            var storage = new WaterStorage();
            storage.AddClean(10f);
            storage.AddDirty(10f);
            var context = new AIContext { Survivor = survivor, WaterStorage = storage, Random = new Random(1) };

            Assert.That(_action.EvaluateRaw(context), Is.EqualTo(0f).Within(Eps));
        }

        [Test]
        public void EvaluateRaw_BelowThirstThreshold_ScoresZero()
        {
            var survivor = MakeSurvivor(30f, RiskBiasTrait.Reckless);
            var storage = new WaterStorage();
            storage.AddDirty(10f);
            var context = new AIContext { Survivor = survivor, WaterStorage = storage, Random = new Random(1) };

            Assert.That(_action.EvaluateRaw(context), Is.EqualTo(0f).Within(Eps));
        }

        [Test]
        public void EvaluateRaw_RecklessSurvivor_ScoresHigherThanParanoidSurvivor()
        {
            var storage = new WaterStorage();
            storage.AddDirty(10f);

            var reckless = MakeSurvivor(90f, RiskBiasTrait.Reckless);
            var recklessContext = new AIContext { Survivor = reckless, WaterStorage = storage, Random = new Random(1) };

            var paranoid = MakeSurvivor(90f, RiskBiasTrait.Paranoid);
            var paranoidContext = new AIContext { Survivor = paranoid, WaterStorage = storage, Random = new Random(1) };

            Assert.That(_action.EvaluateRaw(recklessContext), Is.GreaterThan(_action.EvaluateRaw(paranoidContext)));
        }

        [Test]
        public void Execute_PrefersIrradiatedOverDirty_AndAppliesRadiationDose()
        {
            var survivor = MakeSurvivor(90f, RiskBiasTrait.Reckless);
            var storage = new WaterStorage();
            storage.AddIrradiated(5f);
            storage.AddDirty(5f);
            var context = new AIContext
            {
                Survivor = survivor,
                WaterStorage = storage,
                RadiationSystem = _radiationSystem,
                Random = new Random(1)
            };

            _action.Execute(context);

            Assert.That(storage.IrradiatedWater, Is.EqualTo(4f).Within(Eps));
            Assert.That(storage.DirtyWater, Is.EqualTo(5f).Within(Eps));
            Assert.That(survivor.Needs.Thirst, Is.EqualTo(55f).Within(Eps));
            Assert.That(survivor.RadiationDose, Is.GreaterThan(0f));
        }

        [Test]
        public void Execute_FallsBackToDirtyWater_WhenNoIrradiatedAvailable()
        {
            var survivor = MakeSurvivor(90f, RiskBiasTrait.Reckless);
            var storage = new WaterStorage();
            storage.AddDirty(5f);
            var context = new AIContext
            {
                Survivor = survivor,
                WaterStorage = storage,
                RadiationSystem = _radiationSystem,
                Random = new Random(1)
            };

            _action.Execute(context);

            Assert.That(storage.DirtyWater, Is.EqualTo(4f).Within(Eps));
            Assert.That(survivor.Needs.Thirst, Is.EqualTo(55f).Within(Eps));
            Assert.That(survivor.RadiationDose, Is.EqualTo(0f).Within(Eps));
        }
    }
}
