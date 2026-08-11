using System.Collections.Generic;
using AtomicWar._Game.Core;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;
using NUnit.Framework;
using UnityEngine;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Repair-terminal contract: condition uses the existing shelter/grid fields,
    /// materials are verified before consumption, and a full blackout is a hard lock.
    /// </summary>
    [TestFixture]
    public class BunkerMaintenanceTests
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
        public void Snapshot_ReportsWearRecipesAssignmentAndPersistedPriority()
        {
            var materials = CreateMaterials();
            var survivor = new Survivor { Id = "elena_vasquez", DisplayName = "Elena Vasquez" };
            var survivors = new List<Survivor> { survivor };
            var shelter = CreateShelter(42f);
            var network = PowerNetwork.CreateDefault();

            using (var maintenance = CreateSystem(shelter, network, materials, survivors))
            {
                Assert.That(maintenance.AssignSurvivor("elena_vasquez"), Is.True);
                Assert.That(maintenance.AdjustPriority(1), Is.True);

                var snapshot = maintenance.GetSnapshot();
                var filter = FindTarget(snapshot, MaintenanceTargetType.Module, "air_filtration");
                var generator = FindTarget(snapshot, MaintenanceTargetType.PowerSource, "diesel_generator");
                Assert.That(snapshot.AssignedSurvivorName, Is.EqualTo("Elena Vasquez"));
                Assert.That(snapshot.RepairPriority, Is.EqualTo(MaintenanceRepairPriority.Critical));
                Assert.That(filter.Condition, Is.EqualTo(42f));
                Assert.That(filter.Materials, Has.Count.EqualTo(2));
                Assert.That(filter.Materials[0].ItemId, Is.EqualTo("mechanical_parts"));
                Assert.That(filter.Materials[1].ItemId, Is.EqualTo("electronic_scrap"));
                Assert.That(generator.Materials, Has.Count.EqualTo(1));
                Assert.That(generator.Materials[0].Amount, Is.EqualTo(2));

                var saved = maintenance.CaptureState();
                using (var restored = CreateSystem(shelter, network, materials, survivors))
                {
                    restored.RestoreState(saved);
                    Assert.That(restored.AssignedSurvivorId, Is.EqualTo("elena_vasquez"));
                    Assert.That(restored.RepairPriority, Is.EqualTo(MaintenanceRepairPriority.Critical));
                }
            }
        }

        [Test]
        public void Repair_BlocksDuringBlackoutThenConsumesMaterialsAndRestoresCondition()
        {
            var materials = CreateMaterials();
            var survivors = new List<Survivor>
            {
                new Survivor { Id = "elena_vasquez", DisplayName = "Elena Vasquez" }
            };
            var shelter = CreateShelter(25f);
            var network = PowerNetwork.CreateDefault(0f);

            using (var maintenance = CreateSystem(shelter, network, materials, survivors))
            {
                Assert.That(maintenance.AssignSurvivor("elena_vasquez"), Is.True);
                Assert.That(network.IsBlackout, Is.True);
                Assert.That(maintenance.TryRepair(MaintenanceTargetType.Module, "air_filtration", out var blocked), Is.False);
                StringAssert.Contains("active grid failure", blocked.Reason);
                Assert.That(materials["mechanical_parts"], Is.EqualTo(10));
                Assert.That(materials["electronic_scrap"], Is.EqualTo(4));

                network.GetSource("diesel_generator").Fuel = 10f;
                network.Rebalance();
                Assert.That(network.IsBlackout, Is.False);
                Assert.That(maintenance.TryRepair(MaintenanceTargetType.Module, "air_filtration", out var repaired), Is.True);
                Assert.That(repaired.Succeeded, Is.True);
                Assert.That(shelter.GetModule("air_filtration").FilterHealth, Is.EqualTo(100f));
                Assert.That(materials["mechanical_parts"], Is.EqualTo(9));
                Assert.That(materials["electronic_scrap"], Is.EqualTo(3));
            }
        }

        [Test]
        public void Terminal_QueuesInterruptibleRepairIntentAndUsesNBinding()
        {
            var materials = CreateMaterials();
            var survivors = new List<Survivor>
            {
                new Survivor { Id = "elena_vasquez", DisplayName = "Elena Vasquez" }
            };
            var shelter = CreateShelter(55f);
            var network = PowerNetwork.CreateDefault();
            using (var maintenance = CreateSystem(shelter, network, materials, survivors))
            using (var workOrders = new RepairWorkOrderSystem(maintenance, () => survivors))
            {
                var go = new GameObject("BunkerMaintenanceHudTests");
                _toDestroy.Add(go);
                var terminal = go.AddComponent<BunkerMaintenanceHUD>();
                terminal.Bind(maintenance.GetSnapshot, () => survivors, workOrders.GetSnapshot);
                terminal.OnSurvivorAssignmentRequested += id => maintenance.AssignSurvivor(id);
                terminal.OnPriorityAdjustmentRequested += direction => maintenance.AdjustPriority(direction);
                terminal.OnRepairRequested += (kind, id) =>
                {
                    workOrders.TryQueue(kind, id, out var result);
                    terminal.ReportOutcome(result.Reason);
                };
                terminal.OnRepairCancellationRequested += () =>
                {
                    workOrders.CancelActiveOrder(out var result);
                    terminal.ReportOutcome(result.Reason);
                };

                terminal.Open();
                StringAssert.Contains("BUNKER MAINTENANCE", terminal.PanelSummary);
                StringAssert.Contains("GRID INTERLOCK: clear.", terminal.PanelSummary);
                Assert.That(terminal.SelectNextSurvivor(), Is.True);
                Assert.That(maintenance.AssignedSurvivorId, Is.EqualTo("elena_vasquez"));
                Assert.That(terminal.IncreasePriority(), Is.True);
                Assert.That(maintenance.RepairPriority, Is.EqualTo(MaintenanceRepairPriority.Critical));
                Assert.That(terminal.RepairSelected(), Is.True);
                Assert.That(shelter.GetModule("air_filtration").FilterHealth, Is.EqualTo(55f));
                Assert.That(workOrders.HasActiveOrder, Is.True);
                StringAssert.Contains("WORK ORDER: QUEUED", terminal.PanelSummary);
                Assert.That(terminal.CancelRepairOrder(), Is.True);
                Assert.That(workOrders.HasActiveOrder, Is.False);
                Assert.That(materials["mechanical_parts"], Is.EqualTo(10));
                StringAssert.Contains("REPORT: CANCELLED", terminal.PanelSummary);

                var inputGo = new GameObject("BunkerMaintenanceInputTests");
                _toDestroy.Add(inputGo);
                var input = inputGo.AddComponent<PlayerInputHandler>();
                Assert.That(input.BunkerMaintenanceKey, Is.EqualTo(KeyCode.N));
            }
        }

        private BunkerMaintenanceSystem CreateSystem(
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

        private static BunkerMaintenanceTargetSnapshot FindTarget(
            BunkerMaintenanceSnapshot snapshot,
            MaintenanceTargetType type,
            string id)
        {
            for (int i = 0; i < snapshot.Targets.Count; i++)
            {
                var target = snapshot.Targets[i];
                if (target.TargetType == type && target.TargetId == id)
                    return target;
            }
            Assert.Fail("Missing maintenance target: " + id);
            return null;
        }
    }
}
