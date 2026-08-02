using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Inventory;
using Random = System.Random;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class UtilityAITests
    {
        private const float Eps = 1e-4f;

        private EatActionSO _eatAction;
        private DrinkActionSO _drinkAction;
        private SleepActionSO _sleepAction;
        private WarmUpActionSO _warmUpAction;
        private TakeIodineActionSO _takeIodineAction;
        private UseAntiRadActionSO _useAntiRadAction;
        private ScavengeActionSO _scavengeAction;
        private RepairFilterActionSO _repairFilterAction;
        private RestActionSO _restAction;

        private ItemDefinition _foodItem;
        private ItemDefinition _iodineItem;
        private ItemDefinition _antiRadItem;
        private ItemDefinition _fuelItem;
        private ItemDefinition _airFilterItem;

        [SetUp]
        public void SetUp()
        {
            _foodItem = ScriptableObject.CreateInstance<ItemDefinition>();
            _foodItem.id = "canned_food";
            _foodItem.hungerRestored = 40f;

            _iodineItem = ScriptableObject.CreateInstance<ItemDefinition>();
            _iodineItem.id = "iodine_pills";

            _antiRadItem = ScriptableObject.CreateInstance<ItemDefinition>();
            _antiRadItem.id = "anti_rad";

            _fuelItem = ScriptableObject.CreateInstance<ItemDefinition>();
            _fuelItem.id = "fuel";

            _airFilterItem = ScriptableObject.CreateInstance<ItemDefinition>();
            _airFilterItem.id = "air_filter";

            _eatAction = ScriptableObject.CreateInstance<EatActionSO>();
            _drinkAction = ScriptableObject.CreateInstance<DrinkActionSO>();
            _sleepAction = ScriptableObject.CreateInstance<SleepActionSO>();
            _warmUpAction = ScriptableObject.CreateInstance<WarmUpActionSO>();
            _takeIodineAction = ScriptableObject.CreateInstance<TakeIodineActionSO>();
            _useAntiRadAction = ScriptableObject.CreateInstance<UseAntiRadActionSO>();
            _scavengeAction = ScriptableObject.CreateInstance<ScavengeActionSO>();
            _repairFilterAction = ScriptableObject.CreateInstance<RepairFilterActionSO>();
            _restAction = ScriptableObject.CreateInstance<RestActionSO>();
        }

        [Test]
        public void UtilityAI_StarvingAndIrradiatedSurvivor_RanksAntiRadAndEatAboveRest()
        {
            var survivor = new Survivor { Id = "s1", DisplayName = "Test Survivor" };
            survivor.Needs.Hunger = 90f;
            survivor.RadiationDose = 70f;

            var inventory = new Inventory { Capacity = 10 };
            inventory.Add(_foodItem, 2);
            inventory.Add(_antiRadItem, 1);
            inventory.Add(_iodineItem, 1);

            var context = new AIContext
            {
                Survivor = survivor,
                Inventory = inventory,
                Random = new Random(42)
            };

            var ai = new UtilityAI();
            var candidates = new List<SurvivorAction> { _eatAction, _useAntiRadAction, _restAction };

            float scoreEat = ai.Scorer.Score(_eatAction, context);
            float scoreAntiRad = ai.Scorer.Score(_useAntiRadAction, context);
            float scoreRest = ai.Scorer.Score(_restAction, context);

            Assert.That(scoreEat, Is.GreaterThan(scoreRest));
            Assert.That(scoreAntiRad, Is.GreaterThan(scoreRest));

            var selectedAction = ai.SelectAction(context, candidates);
            Assert.That(selectedAction, Is.Not.EqualTo(_restAction));
            Assert.That(selectedAction == _eatAction || selectedAction == _useAntiRadAction, Is.True);
        }

        [Test]
        public void UtilityAI_DeterministicSelection_WithSeededRNG()
        {
            var survivor = new Survivor { Id = "s1" };
            survivor.Needs.Hunger = 50f;

            var inventory = new Inventory { Capacity = 10 };
            inventory.Add(_foodItem, 1);

            var candidates = new List<SurvivorAction> { _eatAction, _restAction };

            var ai = new UtilityAI();
            var context1 = new AIContext(survivor, null, inventory, new Random(12345));
            var selected1 = ai.SelectAction(context1, candidates);

            var context2 = new AIContext(survivor, null, inventory, new Random(12345));
            var selected2 = ai.SelectAction(context2, candidates);

            Assert.That(selected1, Is.EqualTo(selected2));
        }

        [Test]
        public void TakeIodine_ScoreSpikes_WhenRadiationRisingAndIodineInInventory()
        {
            var survivor = new Survivor { Id = "s1" };
            var inventory = new Inventory { Capacity = 10 };

            var contextNoIodine = new AIContext
            {
                Survivor = survivor,
                Inventory = inventory,
                IsRadiationRising = true
            };
            var ai = new UtilityAI();

            Assert.That(ai.Scorer.Score(_takeIodineAction, contextNoIodine), Is.EqualTo(0f).Within(Eps));

            inventory.Add(_iodineItem, 1);
            var contextWithIodine = new AIContext
            {
                Survivor = survivor,
                Inventory = inventory,
                IsRadiationRising = true
            };

            float score = ai.Scorer.Score(_takeIodineAction, contextWithIodine);
            Assert.That(score, Is.GreaterThan(0.85f));
        }

        [Test]
        public void WarmUp_ScoreSpikes_WhenWarmthBelow30AndFuelAndHeaterExist()
        {
            var survivor = new Survivor { Id = "s1" };
            survivor.Needs.Warmth = 20f;

            var shelter = new Shelter();
            var heaterSO = ScriptableObject.CreateInstance<HeaterModuleSO>();
            heaterSO.ModuleId = "heater";

            var heaterInst = new ShelterModuleInstance(heaterSO, level: 1) { Fuel = 5f };
            shelter.AddModule(heaterInst);

            var context = new AIContext
            {
                Survivor = survivor,
                Shelter = shelter
            };
            var ai = new UtilityAI();

            float score = ai.Scorer.Score(_warmUpAction, context);
            Assert.That(score, Is.GreaterThan(0.9f));
        }

        [Test]
        public void Scavenge_SuppressedToZero_DuringFalloutStorm_UnlessFullSuitEquipped()
        {
            var survivor = new Survivor { Id = "s1", HasFullSuitEquipped = false };
            var contextInStorm = new AIContext
            {
                Survivor = survivor,
                IsFalloutStorm = true
            };
            var ai = new UtilityAI();

            Assert.That(ai.Scorer.Score(_scavengeAction, contextInStorm), Is.EqualTo(0f).Within(Eps));

            survivor.HasFullSuitEquipped = true;
            Assert.That(ai.Scorer.Score(_scavengeAction, contextInStorm), Is.GreaterThan(0f));
        }

        [Test]
        public void RepairFilter_ScoreRises_AsFilterHealthFalls()
        {
            var shelter = new Shelter();
            var airSO = ScriptableObject.CreateInstance<AirFiltrationModuleSO>();
            airSO.ModuleId = "air_filtration";

            var airInst = new ShelterModuleInstance(airSO, level: 1) { FilterHealth = 80f };
            shelter.AddModule(airInst);

            var inventory = new Inventory { Capacity = 10 };
            inventory.Add(_airFilterItem, 1);

            var contextHighHealth = new AIContext
            {
                Shelter = shelter,
                Inventory = inventory
            };
            var ai = new UtilityAI();

            float scoreHighHealth = ai.Scorer.Score(_repairFilterAction, contextHighHealth);

            airInst.FilterHealth = 10f;
            float scoreLowHealth = ai.Scorer.Score(_repairFilterAction, contextHighHealth);

            Assert.That(scoreLowHealth, Is.GreaterThan(scoreHighHealth));
        }
    }
}
