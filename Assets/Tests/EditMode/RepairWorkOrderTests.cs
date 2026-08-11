using System.Collections.Generic;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;
using NUnit.Framework;
using UnityEngine;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class RepairWorkOrderTests
    {
        private readonly List<Object> _toDestroy = new List<Object>();
        private AirFiltrationModuleSO _filterDefinition;

        [SetUp]
        public void SetUp()
        {
            _filterDefinition = ScriptableObject.CreateInstance<AirFiltrationModuleSO>();
            _filterDefinition.ModuleId = "air_filtration";
            _filterDefinition.DisplayName = "Air Filter";
            _toDestroy.Add(_filterDefinition);
        }

        [TearDown]
        public void TearDown()
        {
            for (int i = 0; i < _toDestroy.Count; i++)
            {
                if (_toDestroy[i] != null)
                    Object.DestroyImmediate(_toDestroy[i]);
            }
            _toDestroy.Clear();
        }

        [Test]
        public void WorkOrder_ClaimsProgressesCancelsSafelyThenCompletes()
        {
            var materials = CreateMaterials();
            var survivor = new Survivor { Id = "elena_vasquez", DisplayName = "Elena Vasquez" };
            var survivors = new List<Survivor> { survivor };
            var shelter = CreateShelter(45f);
            var network = PowerNetwork.CreateDefault();
            network.GetSource("diesel_generator").Fuel = 10f;
            network.Rebalance();

            using (var maintenance = CreateMaintenance(shelter, network, materials, survivors))
            using (var workOrders = new RepairWorkOrderSystem(maintenance, () => survivors))
            {
                Assert.That(maintenance.AssignSurvivor("elena_vasquez"), Is.True);
                Assert.That(workOrders.TryQueue(MaintenanceTargetType.Module, "air_filtration", out var queued), Is.True);
                StringAssert.Contains("QUEUED", queued.Reason);
                Assert.That(materials["mechanical_parts"], Is.EqualTo(10));

                Assert.That(workOrders.TryStartWork(survivor, out _), Is.True);
                workOrders.Tick(1f);
                Assert.That(workOrders.GetSnapshot().ProgressHours, Is.EqualTo(1f));
                Assert.That(shelter.GetModule("air_filtration").FilterHealth, Is.EqualTo(45f));

                network.GetSource("diesel_generator").Fuel = 0f;
                network.Rebalance();
                Assert.That(workOrders.HasActiveOrder, Is.False);
                Assert.That(materials["mechanical_parts"], Is.EqualTo(10));
                Assert.That(materials["electronic_scrap"], Is.EqualTo(4));
                StringAssert.Contains("CANCELLED", workOrders.GetSnapshot().LastReport);

                network.GetSource("diesel_generator").Fuel = 10f;
                network.Rebalance();
                Assert.That(workOrders.TryQueue(MaintenanceTargetType.Module, "air_filtration", out _), Is.True);
                Assert.That(workOrders.TryStartWork(survivor, out _), Is.True);
                survivor.State = SurvivorState.Resting;
                workOrders.Tick(0.25f);
                Assert.That(workOrders.HasActiveOrder, Is.False);
                Assert.That(materials["mechanical_parts"], Is.EqualTo(10));
                StringAssert.Contains("interrupted", workOrders.GetSnapshot().LastReport);

                survivor.State = SurvivorState.Idle;
                Assert.That(workOrders.TryQueue(MaintenanceTargetType.Module, "air_filtration", out _), Is.True);
                Assert.That(workOrders.TryStartWork(survivor, out _), Is.True);
                workOrders.Tick(RepairWorkOrderSystem.ModuleWorkHours);

                Assert.That(workOrders.HasActiveOrder, Is.False);
                Assert.That(shelter.GetModule("air_filtration").FilterHealth, Is.EqualTo(100f));
                Assert.That(materials["mechanical_parts"], Is.EqualTo(9));
                Assert.That(materials["electronic_scrap"], Is.EqualTo(3));
            }
        }

        [Test]
        public void WorkOrder_ActionUsesPriorityAndLoadResumesQueued()
        {
            var materials = CreateMaterials();
            var survivor = new Survivor { Id = "elena_vasquez", DisplayName = "Elena Vasquez" };
            var survivors = new List<Survivor> { survivor };
            var shelter = CreateShelter(72f);
            var network = PowerNetwork.CreateDefault();
            network.GetSource("diesel_generator").Fuel = 10f;
            network.Rebalance();
            var action = ScriptableObject.CreateInstance<RepairWorkOrderActionSO>();
            _toDestroy.Add(action);

            using (var maintenance = CreateMaintenance(shelter, network, materials, survivors))
            using (var workOrders = new RepairWorkOrderSystem(maintenance, () => survivors))
            {
                maintenance.AssignSurvivor("elena_vasquez");
                maintenance.AdjustPriority(1);
                Assert.That(workOrders.TryQueue(MaintenanceTargetType.Module, "air_filtration", out _), Is.True);
                var context = new AIContext
                {
                    Survivor = survivor,
                    RepairWorkOrderSystem = workOrders
                };
                Assert.That(action.EvaluateRaw(context), Is.EqualTo(0.9f));
                action.Execute(context);
                Assert.That(workOrders.GetSnapshot().Status, Is.EqualTo(RepairWorkOrderStatus.Working));
                workOrders.Tick(0.5f);

                var saved = workOrders.CaptureState();
                using (var restored = new RepairWorkOrderSystem(maintenance, () => survivors))
                {
                    restored.RestoreState(saved);
                    var restoredSnapshot = restored.GetSnapshot();
                    Assert.That(restoredSnapshot.Status, Is.EqualTo(RepairWorkOrderStatus.Queued));
                    Assert.That(restoredSnapshot.ProgressHours, Is.EqualTo(0.5f));
                    Assert.That(materials["mechanical_parts"], Is.EqualTo(10));
                }
            }
        }

        private BunkerMaintenanceSystem CreateMaintenance(
            Shelter shelter,
            PowerNetwork network,
            Dictionary<string, int> materials,
            List<Survivor> survivors)
        {
            return new BunkerMaintenanceSystem(
                shelter,
                network,
                id => materials.TryGetValue(id, out var amount) ? amount : 0,
                (id, amount) =>
                {
                    if (!materials.TryGetValue(id, out var onHand) || onHand < amount) return false;
                    materials[id] = onHand - amount;
                    return true;
                },
                () => survivors);
        }

        private Shelter CreateShelter(float filterHealth)
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance(_filterDefinition, 1)
            {
                FilterHealth = filterHealth,
                IsEnabled = true
            });
            return shelter;
        }

        private static Dictionary<string, int> CreateMaterials()
        {
            return new Dictionary<string, int>
            {
                { "mechanical_parts", 10 },
                { "electronic_scrap", 4 }
            };
        }
    }
}
