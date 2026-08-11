using System.Collections.Generic;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;
using NUnit.Framework;
using UnityEngine;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class SurvivorTaskBoardTests
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
                if (_toDestroy[i] != null) Object.DestroyImmediate(_toDestroy[i]);
            _toDestroy.Clear();
        }

        [Test]
        public void TaskBoard_ReservesQueuedWorker_AdjustsPriority_AndRestoresFeedback()
        {
            var materials = CreateMaterials();
            var survivor = new Survivor { Id = "elena_vasquez", DisplayName = "Elena Vasquez" };
            var survivors = new List<Survivor> { survivor };
            var shelter = CreateShelter();
            var power = PowerNetwork.CreateDefault();
            power.GetSource("diesel_generator").Fuel = 10f;
            power.Rebalance();

            using (var maintenance = CreateMaintenance(shelter, power, materials, survivors))
            using (var workOrders = new RepairWorkOrderSystem(maintenance, () => survivors))
            using (var board = new SurvivorTaskBoardSystem(workOrders, () => survivors))
            {
                Assert.That(maintenance.AssignSurvivor(survivor.Id), Is.True);
                Assert.That(workOrders.TryQueue(MaintenanceTargetType.Module, "air_filtration", out _), Is.True);

                var queued = board.GetSnapshot();
                Assert.That(queued.ActiveTasks, Has.Count.EqualTo(1));
                Assert.That(queued.ActiveTasks[0].Status, Is.EqualTo("queued"));
                Assert.That(queued.SurvivorAssignments[0].IsReserved, Is.True);
                StringAssert.Contains("committed to bunker repair", board.GetAssignmentConflictReason(survivor));

                Assert.That(board.TryAdjustActivePriority(1, out var priorityResult), Is.True);
                StringAssert.Contains("PRIORITY UPDATED", priorityResult.Reason);
                Assert.That(workOrders.GetSnapshot().Priority, Is.EqualTo(MaintenanceRepairPriority.Critical));

                var saved = board.CaptureState();
                using (var restored = new SurvivorTaskBoardSystem(workOrders, () => survivors))
                {
                    restored.RestoreState(saved);
                    StringAssert.Contains("PRIORITY UPDATED", restored.GetSnapshot().LastReport);
                }

                Assert.That(board.CancelActiveTask(out var cancelled), Is.True);
                Assert.That(cancelled.Cancelled, Is.True);
                Assert.That(board.GetAssignmentConflictReason(survivor), Is.Null);
                Assert.That(materials["mechanical_parts"], Is.EqualTo(10));
                Assert.That(materials["electronic_scrap"], Is.EqualTo(4));
            }
        }

        [Test]
        public void TaskBoardHud_ReportsActiveReservationAndRaisesCommands()
        {
            var boardObject = new GameObject("Survivor task board HUD test");
            _toDestroy.Add(boardObject);
            var hud = boardObject.AddComponent<SurvivorTaskBoardHUD>();
            int priorityDirection = 0;
            int cancellationCount = 0;
            hud.OnPriorityAdjustmentRequested += direction => priorityDirection = direction;
            hud.OnCancellationRequested += () => cancellationCount++;
            hud.Bind(() => new SurvivorTaskBoardSnapshot
            {
                LastReport = "QUEUED: Air Filter awaits Elena Vasquez.",
                ActiveTasks = new List<SurvivorTaskBoardTask>
                {
                    new SurvivorTaskBoardTask
                    {
                        DisplayName = "Bunker repair",
                        Status = "queued",
                        AssignedSurvivorName = "Elena Vasquez",
                        Priority = MaintenanceRepairPriority.Standard,
                        RequiredWorkHours = 2f
                    }
                },
                SurvivorAssignments = new List<SurvivorTaskBoardAssignment>
                {
                    new SurvivorTaskBoardAssignment
                    {
                        SurvivorName = "Elena Vasquez",
                        AssignmentLabel = "Bunker repair [reserved]",
                        IsReserved = true
                    }
                }
            });

            hud.Open();
            Assert.That(hud.IncreasePriority(), Is.True);
            Assert.That(hud.CancelActiveTask(), Is.True);
            Assert.That(priorityDirection, Is.EqualTo(1));
            Assert.That(cancellationCount, Is.EqualTo(1));
            StringAssert.Contains("ACTIVE WORK ORDERS", hud.PanelSummary);
            StringAssert.Contains("BUNKER REPAIR", hud.PanelSummary);
            StringAssert.Contains("Bunker repair [reserved]", hud.PanelSummary);
        }

        [Test]
        public void TaskBoardHud_SelectsAssignsAndCancelsDutyShift()
        {
            var boardObject = new GameObject("Survivor task-board shift HUD test");
            _toDestroy.Add(boardObject);
            var hud = boardObject.AddComponent<SurvivorTaskBoardHUD>();
            WorkShiftDuty assignedDuty = WorkShiftDuty.HeaterFuel;
            string assignedSurvivorId = null;
            WorkShiftDuty cancelledDuty = WorkShiftDuty.HeaterFuel;
            var snapshot = new SurvivorTaskBoardSnapshot
            {
                ActiveTasks = new List<SurvivorTaskBoardTask>(),
                ShiftSlots = new List<SurvivorWorkShiftSlotSnapshot>
                {
                    new SurvivorWorkShiftSlotSnapshot
                    {
                        Duty = WorkShiftDuty.AirFiltration,
                        DisplayName = "air filtration",
                        IsSupported = true,
                        EffectSummary = "25% less filter wear"
                    }
                },
                SurvivorAssignments = new List<SurvivorTaskBoardAssignment>
                {
                    new SurvivorTaskBoardAssignment
                    {
                        SurvivorId = "elena_vasquez",
                        SurvivorName = "Elena Vasquez",
                        AssignmentLabel = "available"
                    }
                }
            };
            hud.OnShiftAssignmentRequested += (duty, survivorId) =>
            {
                assignedDuty = duty;
                assignedSurvivorId = survivorId;
            };
            hud.OnShiftCancellationRequested += duty => cancelledDuty = duty;
            int recommendationApprovals = 0;
            hud.OnShiftRecommendationApprovalRequested += () => recommendationApprovals++;
            snapshot.ShiftRecommendations = new List<SurvivorWorkShiftRecommendationSnapshot>
            {
                new SurvivorWorkShiftRecommendationSnapshot
                {
                    Duty = WorkShiftDuty.AirFiltration,
                    SuggestedSurvivorName = "Elena Vasquez",
                    Priority = WorkShiftRecommendationPriority.Critical,
                    Reason = "Air quality is becoming unsafe."
                }
            };
            hud.Bind(() => snapshot);
            hud.Open();

            Assert.That(hud.AssignSelectedShift(), Is.True);
            Assert.That(hud.ApproveTopShiftRecommendation(), Is.True);
            Assert.That(assignedDuty, Is.EqualTo(WorkShiftDuty.AirFiltration));
            Assert.That(assignedSurvivorId, Is.EqualTo("elena_vasquez"));
            Assert.That(recommendationApprovals, Is.EqualTo(1));

            snapshot.ShiftSlots[0].AssignedSurvivorId = "elena_vasquez";
            snapshot.ShiftSlots[0].AssignedSurvivorName = "Elena Vasquez";
            snapshot.ShiftSlots[0].ReliefSurvivorId = "marcus_reed";
            snapshot.ShiftSlots[0].ReliefSurvivorName = "Marcus Reed";
            snapshot.ShiftSlots[0].HoursSinceHandover = 2f;
            snapshot.ShiftSlots[0].RotationHours = 4f;
            snapshot.ShiftSlots[0].HandoverCount = 1;
            snapshot.ShiftSlots[0].Availability = new WorkShiftAvailabilityForecast
            {
                Duty = WorkShiftDuty.AirFiltration,
                IsStaffed = true,
                Status = WorkShiftAvailabilityStatus.Low,
                Summary = "Safe for 8.0h at 2.0/h; reserve is low."
            };
            hud.Refresh();
            Assert.That(hud.CancelActiveTask(), Is.True);
            Assert.That(cancelledDuty, Is.EqualTo(WorkShiftDuty.AirFiltration));
            StringAssert.Contains("AIR FILTRATION", hud.PanelSummary);
            StringAssert.Contains("CREW PICK: Elena Vasquez", hud.PanelSummary);
            StringAssert.Contains("SHIFT RECOMMENDATIONS", hud.PanelSummary);
            StringAssert.Contains("CRITICAL · AIR FILTRATION", hud.PanelSummary);
            StringAssert.Contains("[R] APPROVE", hud.PanelSummary);
            StringAssert.Contains("rotate 2.0/4.0h", hud.PanelSummary);
            StringAssert.Contains("RELIEF: Marcus Reed", hud.PanelSummary);
            StringAssert.Contains("25% less filter wear", hud.PanelSummary);
            StringAssert.Contains("Safe for 8.0h at 2.0/h; reserve is low.", hud.PanelSummary);
        }

        [Test]
        public void TaskBoard_ApprovesTopDutyRecommendation()
        {
            var elena = new Survivor { Id = "elena_vasquez", DisplayName = "Elena Vasquez" };
            elena.Needs.Hunger = 100f;
            elena.Needs.Thirst = 100f;
            var survivors = new List<Survivor> { elena };
            var context = new WorkShiftRecommendationContext
            {
                FilterOperational = true,
                AirQuality = SurvivorWorkShiftSystem.CriticalAirQuality - 1f
            };
            var shifts = new SurvivorWorkShiftSystem(
                () => survivors,
                _ => true,
                null,
                null,
                () => context);
            using (var board = new SurvivorTaskBoardSystem(null, () => survivors, shifts))
            {
                Assert.That(shifts.RefreshRecommendations(), Is.True);
                var snapshot = board.GetSnapshot();
                Assert.That(snapshot.ShiftRecommendations, Has.Count.EqualTo(1));
                Assert.That(snapshot.ShiftRecommendations[0].Duty, Is.EqualTo(WorkShiftDuty.AirFiltration));
                Assert.That(board.TryApproveTopShiftRecommendation(out var result), Is.True);
                StringAssert.Contains("APPROVED", result.Reason);
                Assert.That(shifts.GetSnapshot().Slots[0].AssignedSurvivorId, Is.EqualTo(elena.Id));
            }
        }

        private BunkerMaintenanceSystem CreateMaintenance(
            Shelter shelter,
            PowerNetwork power,
            Dictionary<string, int> materials,
            List<Survivor> survivors)
        {
            return new BunkerMaintenanceSystem(
                shelter,
                power,
                id => materials.TryGetValue(id, out var amount) ? amount : 0,
                (id, amount) =>
                {
                    if (!materials.TryGetValue(id, out var onHand) || onHand < amount) return false;
                    materials[id] = onHand - amount;
                    return true;
                },
                () => survivors);
        }

        private Shelter CreateShelter()
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance(_filterDefinition, 1)
            {
                FilterHealth = 48f,
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
