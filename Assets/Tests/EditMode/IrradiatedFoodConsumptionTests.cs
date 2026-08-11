using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Eating irradiated/contaminated food ingests dose via Inventory.Consume →
    /// RadiationSystem.Expose(item.contamination * ContaminationDosePerUnit).
    /// </summary>
    [TestFixture]
    public class IrradiatedFoodConsumptionTests
    {
        private const float Eps = 1e-3f;

        private static NeedsSystem NewNeedsSystem()
        {
            return new NeedsSystem(ScriptableObject.CreateInstance<NeedsProfile>());
        }

        private static ItemDefinition NewFood(string id, float contamination, float hungerRestore = 20f)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = ItemType.ContaminatedFood;
            item.hungerRestore = hungerRestore;
            item.contamination = contamination;
            item.stackMax = 10;
            item.weight = 0.1f;
            return item;
        }

        [Test]
        public void Consume_ContaminatedFood_IncreasesRadiationDose()
        {
            var inventory = new Inventory { Capacity = 10, MaxWeight = 100f };
            var food = NewFood("glowing_meat", contamination: 0.2f);
            inventory.Add(food, 1);

            var survivor = new Survivor { Id = "sv1", State = SurvivorState.Idle };
            var needs = NewNeedsSystem();
            var radiation = new RadiationSystem(needs, s => new ExposureContext { ZoneRadLevel = 0f });
            radiation.Register(survivor);

            bool consumed = inventory.Consume(food, survivor, radiation, needs);

            Assert.IsTrue(consumed);
            Assert.That(survivor.RadiationDose, Is.EqualTo(0.2f * 50f).Within(Eps),
                "Eating contaminated food must ingest dose = contamination * ContaminationDosePerUnit.");
        }

        [Test]
        public void Consume_CleanFood_DoesNotIncreaseRadiationDose()
        {
            var inventory = new Inventory { Capacity = 10, MaxWeight = 100f };
            var food = NewFood("canned_food", contamination: 0f);
            inventory.Add(food, 1);

            var survivor = new Survivor { Id = "sv1", State = SurvivorState.Idle };
            var needs = NewNeedsSystem();
            var radiation = new RadiationSystem(needs, s => new ExposureContext { ZoneRadLevel = 0f });
            radiation.Register(survivor);

            inventory.Consume(food, survivor, radiation, needs);

            Assert.That(survivor.RadiationDose, Is.EqualTo(0f).Within(Eps));
        }

        [Test]
        public void Consume_ContaminatedFood_AlsoRestoresHunger()
        {
            var inventory = new Inventory { Capacity = 10, MaxWeight = 100f };
            var food = NewFood("glowing_meat", contamination: 0.2f, hungerRestore: 30f);
            inventory.Add(food, 1);

            var survivor = new Survivor { Id = "sv1", State = SurvivorState.Idle };
            survivor.Needs.Hunger = 80f;
            var needs = NewNeedsSystem();
            var radiation = new RadiationSystem(needs, s => new ExposureContext { ZoneRadLevel = 0f });
            radiation.Register(survivor);

            inventory.Consume(food, survivor, radiation, needs);

            Assert.That(survivor.Needs.Hunger, Is.EqualTo(50f).Within(Eps),
                "Contaminated food still restores hunger even while ingesting dose.");
        }

        [Test]
        public void Consume_ContaminatedFood_WithoutRadiationSystem_StillConsumesSafely()
        {
            var inventory = new Inventory { Capacity = 10, MaxWeight = 100f };
            var food = NewFood("glowing_meat", contamination: 0.2f);
            inventory.Add(food, 1);

            var survivor = new Survivor { Id = "sv1", State = SurvivorState.Idle };
            var needs = NewNeedsSystem();

            bool consumed = false;
            Assert.DoesNotThrow(() => consumed = inventory.Consume(food, survivor, null, needs));
            Assert.IsTrue(consumed);
            Assert.That(survivor.RadiationDose, Is.EqualTo(0f).Within(Eps),
                "No RadiationSystem injected — dose ingestion must be skipped, not defaulted elsewhere.");
        }

        [Test]
        public void Consume_MultipleContaminatedUnits_AccumulatesDoseAcrossMeals()
        {
            var inventory = new Inventory { Capacity = 10, MaxWeight = 100f };
            var food = NewFood("glowing_meat", contamination: 0.1f);
            inventory.Add(food, 3);

            var survivor = new Survivor { Id = "sv1", State = SurvivorState.Idle };
            var needs = NewNeedsSystem();
            var radiation = new RadiationSystem(needs, s => new ExposureContext { ZoneRadLevel = 0f });
            radiation.Register(survivor);

            inventory.Consume(food, survivor, radiation, needs);
            inventory.Consume(food, survivor, radiation, needs);
            inventory.Consume(food, survivor, radiation, needs);

            Assert.That(survivor.RadiationDose, Is.EqualTo(3 * 0.1f * 50f).Within(Eps));
        }
    }
}
