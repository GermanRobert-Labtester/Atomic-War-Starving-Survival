using System.Collections.Generic;
using AtomicWar._Game.Core;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;
using NUnit.Framework;
using UnityEngine;
using Ashfall.Core;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class SurvivorWorkShiftTests
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
        public void WorkShifts_ReserveTickInterruptAndRestoreSafely()
        {
            var elena = new Survivor { Id = "elena_vasquez", DisplayName = "Elena Vasquez" };
            var survivors = new List<Survivor> { elena };
            var shifts = new SurvivorWorkShiftSystem(() => survivors, _ => true);
            using (var board = new SurvivorTaskBoardSystem(null, () => survivors, shifts))
            {
                Assert.That(board.TryAssignShift(WorkShiftDuty.AirFiltration, elena.Id, out var assigned), Is.True);
                StringAssert.Contains("ASSIGNED", assigned.Reason);
                Assert.That(elena.State, Is.EqualTo(SurvivorState.Working));

                shifts.Tick(1.5f);
                Assert.That(FindSlot(shifts.GetSnapshot(), WorkShiftDuty.AirFiltration).HoursWorked, Is.EqualTo(1.5f));
                Assert.That(board.GetSnapshot().SurvivorAssignments[0].AssignmentLabel,
                    Is.EqualTo("air filtration [reserved]"));
                Assert.That(shifts.TryAssign(WorkShiftDuty.WaterPurification, elena.Id, out var duplicate), Is.False);
                StringAssert.Contains("assigned elsewhere", duplicate.Reason);
                StringAssert.Contains("air filtration", board.GetAssignmentConflictReason(elena));

                elena.State = SurvivorState.Resting;
                shifts.Tick(0.25f);
                Assert.That(FindSlot(shifts.GetSnapshot(), WorkShiftDuty.AirFiltration).AssignedSurvivorId, Is.Null);

                elena.State = SurvivorState.Idle;
                Assert.That(shifts.TryAssign(WorkShiftDuty.RationPreparation, elena.Id, out _), Is.True);
                shifts.Tick(2f);
                var save = shifts.CaptureState();
                Assert.That(shifts.CancelShift(WorkShiftDuty.RationPreparation, out _), Is.True);
                Assert.That(elena.State, Is.EqualTo(SurvivorState.Idle));

                var restored = new SurvivorWorkShiftSystem(() => survivors, _ => true);
                {
                    restored.RestoreState(save);
                    var rationShift = FindSlot(restored.GetSnapshot(), WorkShiftDuty.RationPreparation);
                    Assert.That(rationShift.AssignedSurvivorId, Is.EqualTo(elena.Id));
                    Assert.That(rationShift.HoursWorked, Is.EqualTo(2f));
                    Assert.That(elena.State, Is.EqualTo(SurvivorState.Working));
                }
            }
        }

        [Test]
        public void WorkShifts_RejectRepairReservedWorker()
        {
            var materials = new Dictionary<string, int>
            {
                { "mechanical_parts", 10 },
                { "electronic_scrap", 4 }
            };
            var elena = new Survivor { Id = "elena_vasquez", DisplayName = "Elena Vasquez" };
            var survivors = new List<Survivor> { elena };
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance(_filterDefinition, 1)
            {
                FilterHealth = 45f,
                IsEnabled = true
            });
            var power = PowerNetwork.CreateDefault();
            power.GetSource("diesel_generator").Fuel = 10f;
            power.Rebalance();

            using (var maintenance = new BunkerMaintenanceSystem(
                shelter,
                power,
                id => materials.TryGetValue(id, out var amount) ? amount : 0,
                (id, amount) =>
                {
                    if (!materials.TryGetValue(id, out var onHand) || onHand < amount) return false;
                    materials[id] = onHand - amount;
                    return true;
                },
                () => survivors))
            using (var repair = new RepairWorkOrderSystem(maintenance, () => survivors))
            {
                var shifts = new SurvivorWorkShiftSystem(() => survivors, _ => true, repair);
                Assert.That(maintenance.AssignSurvivor(elena.Id), Is.True);
                Assert.That(repair.TryQueue(MaintenanceTargetType.Module, "air_filtration", out _), Is.True);
                Assert.That(shifts.TryAssign(WorkShiftDuty.AirFiltration, elena.Id, out var result), Is.False);
                StringAssert.Contains("committed to bunker repair", result.Reason);
                Assert.That(elena.State, Is.EqualTo(SurvivorState.Idle));
            }
        }

        [Test]
        public void WorkShifts_RotateDrainFatigueHandoverAndRestoreSafely()
        {
            var elena = new Survivor { Id = "elena_vasquez", DisplayName = "Elena Vasquez" };
            var marcus = new Survivor { Id = "marcus_reed", DisplayName = "Marcus Reed" };
            var survivors = new List<Survivor> { elena, marcus };
            var shifts = new SurvivorWorkShiftSystem(() => survivors, _ => true);
            int handovers = 0;
            int emergencyHandovers = 0;
            shifts.OnShiftHandedOver += result =>
            {
                handovers++;
                if (result.WasEmergencyHandover) emergencyHandovers++;
            };

            using (var board = new SurvivorTaskBoardSystem(null, () => survivors, shifts))
            {
                Assert.That(board.TryAssignShift(WorkShiftDuty.AirFiltration, elena.Id, out _), Is.True);
                Assert.That(board.TryAssignShift(WorkShiftDuty.AirFiltration, marcus.Id, out var relief), Is.True);
                StringAssert.Contains("RELIEF ASSIGNED", relief.Reason);
                Assert.That(board.GetSnapshot().SurvivorAssignments[1].AssignmentLabel,
                    Is.EqualTo("air filtration relief [reserved]"));
                StringAssert.Contains("on relief for", board.GetAssignmentConflictReason(marcus));

                shifts.Tick(1.25f);
                var saved = shifts.CaptureState();
                Assert.That(saved.shifts[0].reliefSurvivorId, Is.EqualTo(marcus.Id));
                Assert.That(saved.shifts[0].hoursWorked, Is.EqualTo(1.25f));
                Assert.That(saved.shifts[0].hoursSinceHandover, Is.EqualTo(1.25f));
                Assert.That(shifts.CancelShift(WorkShiftDuty.AirFiltration, out _), Is.True);
                Assert.That(elena.State, Is.EqualTo(SurvivorState.Idle));
                Assert.That(marcus.State, Is.EqualTo(SurvivorState.Idle));

                var restored = new SurvivorWorkShiftSystem(() => survivors, _ => true);
                restored.OnShiftHandedOver += result =>
                {
                    handovers++;
                    if (result.WasEmergencyHandover) emergencyHandovers++;
                };
                restored.RestoreState(saved);
                var restoredSlot = FindSlot(restored.GetSnapshot(), WorkShiftDuty.AirFiltration);
                Assert.That(restoredSlot.AssignedSurvivorId, Is.EqualTo(elena.Id));
                Assert.That(restoredSlot.ReliefSurvivorId, Is.EqualTo(marcus.Id));
                Assert.That(restoredSlot.HoursSinceHandover, Is.EqualTo(1.25f));

                restored.Tick(SurvivorWorkShiftSystem.RotationHours - 1.25f);
                var rotatedSlot = FindSlot(restored.GetSnapshot(), WorkShiftDuty.AirFiltration);
                Assert.That(rotatedSlot.AssignedSurvivorId, Is.EqualTo(marcus.Id));
                Assert.That(rotatedSlot.ReliefSurvivorId, Is.Null);
                Assert.That(rotatedSlot.HandoverCount, Is.EqualTo(1));
                Assert.That(rotatedSlot.HoursSinceHandover, Is.EqualTo(0f));
                Assert.That(elena.Needs.Fatigue,
                    Is.EqualTo(SurvivorWorkShiftSystem.FatiguePerStaffedHour
                        * SurvivorWorkShiftSystem.RotationHours));
                Assert.That(marcus.Needs.Fatigue, Is.EqualTo(0f));
                Assert.That(elena.State, Is.EqualTo(SurvivorState.Idle));
                Assert.That(marcus.State, Is.EqualTo(SurvivorState.Working));

                Assert.That(restored.TryAssign(WorkShiftDuty.AirFiltration, elena.Id, out _), Is.True);
                marcus.Needs.Fatigue = SurvivorWorkShiftSystem.EmergencyFatigueThreshold - 1f;
                restored.Tick(0.5f);
                var fatigueHandover = FindSlot(restored.GetSnapshot(), WorkShiftDuty.AirFiltration);
                Assert.That(fatigueHandover.AssignedSurvivorId, Is.EqualTo(elena.Id));
                Assert.That(fatigueHandover.HandoverCount, Is.EqualTo(2));
                Assert.That(marcus.State, Is.EqualTo(SurvivorState.Idle));

                marcus.Needs.Fatigue = 0f;
                Assert.That(restored.TryAssign(WorkShiftDuty.AirFiltration, marcus.Id, out _), Is.True);
                elena.State = SurvivorState.Resting;
                restored.Tick(0.25f);
                var interruptionHandover = FindSlot(restored.GetSnapshot(), WorkShiftDuty.AirFiltration);
                Assert.That(interruptionHandover.AssignedSurvivorId, Is.EqualTo(marcus.Id));
                Assert.That(interruptionHandover.HandoverCount, Is.EqualTo(3));
                Assert.That(elena.State, Is.EqualTo(SurvivorState.Resting));
                Assert.That(marcus.State, Is.EqualTo(SurvivorState.Working));
                Assert.That(handovers, Is.EqualTo(3));
                Assert.That(emergencyHandovers, Is.EqualTo(2));

                Assert.That(restored.CancelShift(WorkShiftDuty.AirFiltration, out _), Is.True);
                Assert.That(restored.TryAssign(WorkShiftDuty.AirFiltration, marcus.Id, out _), Is.True);
                marcus.Needs.Fatigue = SurvivorWorkShiftSystem.EmergencyFatigueThreshold - 1f;
                restored.Tick(0.25f);
                Assert.That(FindSlot(restored.GetSnapshot(), WorkShiftDuty.AirFiltration).AssignedSurvivorId, Is.Null);
                Assert.That(marcus.State, Is.EqualTo(SurvivorState.Idle));
            }
        }

        [Test]
        public void WorkShifts_RecommendHazardsPersistAndRequirePlayerApproval()
        {
            var elena = CreateSurvivor("elena_vasquez", "Elena Vasquez");
            var marcus = CreateSurvivor("marcus_reed", "Marcus Reed");
            var nora = CreateSurvivor("nora_kim", "Nora Kim");
            var dani = CreateSurvivor("dani_ortiz", "Dani Ortiz");
            elena.Needs.Fatigue = SurvivorWorkShiftSystem.RecommendationMaxFatigue + 1f;
            nora.Needs.Fatigue = 20f;
            dani.Needs.Fatigue = 20f;
            var survivors = new List<Survivor> { elena, marcus, nora, dani };
            var context = new WorkShiftRecommendationContext
            {
                FilterOperational = false,
                AirQuality = SurvivorWorkShiftSystem.CriticalAirQuality - 1f,
                FilterHealth = SurvivorWorkShiftSystem.LowFilterHealth - 1f,
                FilterBurnPerHour = 1f,
                FilterRuntimeHours = 8f
            };
            var shifts = new SurvivorWorkShiftSystem(
                () => survivors,
                _ => true,
                null,
                null,
                () => context);
            int changed = 0;
            shifts.OnChanged += () => changed++;

            Assert.That(shifts.RefreshRecommendations(), Is.False,
                "An offline station must not produce an unusable assignment suggestion.");
            Assert.That(shifts.GetSnapshot().Recommendations, Is.Empty);

            context.FilterOperational = true;
            Assert.That(shifts.RefreshRecommendations(), Is.True);
            var recommendation = shifts.GetSnapshot().Recommendations[0];
            Assert.That(recommendation.Duty, Is.EqualTo(WorkShiftDuty.AirFiltration));
            Assert.That(recommendation.Priority, Is.EqualTo(WorkShiftRecommendationPriority.Critical));
            Assert.That(recommendation.SuggestedSurvivorId, Is.EqualTo(marcus.Id),
                "The rested idle worker should be preferred over an exhausted worker.");
            StringAssert.Contains("Safe for 8.0h", recommendation.Reason,
                "Recommendation reasoning should include the current duty forecast.");
            Assert.That(FindSlot(shifts.GetSnapshot(), WorkShiftDuty.AirFiltration).AssignedSurvivorId, Is.Null,
                "Refreshing recommendations must never reserve or auto-assign a survivor.");
            Assert.That(changed, Is.EqualTo(1));

            context.FilterRuntimeHours = 2f;
            Assert.That(shifts.RefreshRecommendations(), Is.True,
                "A changed availability forecast must refresh the displayed recommendation reason.");
            StringAssert.Contains("Only 2.0h", shifts.GetSnapshot().Recommendations[0].Reason);

            var save = shifts.CaptureState();
            Assert.That(save.recommendations, Has.Count.EqualTo(1));
            var restored = new SurvivorWorkShiftSystem(
                () => survivors,
                _ => true,
                null,
                null,
                () => context);
            restored.RestoreState(save);
            Assert.That(restored.GetSnapshot().Recommendations[0].SuggestedSurvivorId, Is.EqualTo(marcus.Id));

            context.FilterOperational = false;
            Assert.That(restored.TryApproveRecommendation(WorkShiftDuty.AirFiltration, out var cleared), Is.False);
            StringAssert.Contains("no pending", cleared.Reason);
            Assert.That(restored.GetSnapshot().Recommendations, Is.Empty);

            context.FilterOperational = true;
            Assert.That(restored.RefreshRecommendations(), Is.True);
            Assert.That(restored.TryApproveRecommendation(WorkShiftDuty.AirFiltration, out var approved), Is.True);
            Assert.That(approved.WasRecommendationApproved, Is.True);
            StringAssert.Contains("APPROVED", approved.Reason);
            Assert.That(FindSlot(restored.GetSnapshot(), WorkShiftDuty.AirFiltration).AssignedSurvivorId,
                Is.EqualTo(marcus.Id));
            Assert.That(restored.GetSnapshot().Recommendations, Is.Empty);

            context.FilterOperational = false;
            context.ProjectedFoodCoverage = 1f;
            nora.Needs.Hunger = SurvivorWorkShiftSystem.CriticalCrewNeed - 1f;
            Assert.That(restored.RefreshRecommendations(), Is.True);
            var rationRecommendation = restored.GetSnapshot().Recommendations[0];
            Assert.That(rationRecommendation.Duty, Is.EqualTo(WorkShiftDuty.RationPreparation));
            Assert.That(rationRecommendation.SuggestedSurvivorId, Is.EqualTo(dani.Id),
                "A hungry survivor should trigger ration preparation without being selected to staff it.");
        }

        [Test]
        public void WorkShifts_ForecastStationAvailabilityAndRestoreFromLiveTelemetry()
        {
            var elena = CreateSurvivor("elena_vasquez", "Elena Vasquez");
            var marcus = CreateSurvivor("marcus_reed", "Marcus Reed");
            var nora = CreateSurvivor("nora_kim", "Nora Kim");
            var dani = CreateSurvivor("dani_ortiz", "Dani Ortiz");
            var survivors = new List<Survivor> { elena, marcus, nora, dani };
            var context = new WorkShiftRecommendationContext
            {
                FilterOperational = true,
                FilterBurnPerHour = 2f,
                FilterRuntimeHours = 16f,
                HeaterOperational = true,
                HeaterBurnPerHour = 1f,
                HeaterRuntimeHours = 3f,
                PurifierOperational = true,
                PurifierFilterBurnPerHour = 1f,
                PurifierRuntimeHours = 5f,
                PurifierUnitsQueued = 3,
                RationOperational = true,
                FoodDaysRemaining = 4f,
                WaterDaysRemaining = 0.5f,
                FoodUnitsPerDay = 4,
                WaterUnitsPerDay = 4
            };
            var shifts = new SurvivorWorkShiftSystem(() => survivors, _ => true, null, null, () => context);

            Assert.That(shifts.TryAssign(WorkShiftDuty.AirFiltration, elena.Id, out _), Is.True);
            Assert.That(shifts.TryAssign(WorkShiftDuty.HeaterFuel, marcus.Id, out _), Is.True);
            Assert.That(shifts.TryAssign(WorkShiftDuty.WaterPurification, nora.Id, out _), Is.True);
            Assert.That(shifts.TryAssign(WorkShiftDuty.RationPreparation, dani.Id, out _), Is.True);

            var snapshot = shifts.GetSnapshot();
            var filter = FindSlot(snapshot, WorkShiftDuty.AirFiltration).Availability;
            var heater = FindSlot(snapshot, WorkShiftDuty.HeaterFuel).Availability;
            var purifier = FindSlot(snapshot, WorkShiftDuty.WaterPurification).Availability;
            var rations = FindSlot(snapshot, WorkShiftDuty.RationPreparation).Availability;
            Assert.That(filter.Status, Is.EqualTo(WorkShiftAvailabilityStatus.Stable));
            StringAssert.Contains("16.0h", filter.Summary);
            Assert.That(heater.Status, Is.EqualTo(WorkShiftAvailabilityStatus.Critical));
            StringAssert.Contains("Only 3.0h", heater.Summary);
            Assert.That(purifier.Status, Is.EqualTo(WorkShiftAvailabilityStatus.Low));
            StringAssert.Contains("Safe for 5.0h", purifier.Summary);
            Assert.That(rations.Status, Is.EqualTo(WorkShiftAvailabilityStatus.Critical));
            StringAssert.Contains("Only 0.5d", rations.Summary);

            var saved = shifts.CaptureState();
            Assert.That(shifts.CancelShift(WorkShiftDuty.AirFiltration, out _), Is.True);
            Assert.That(shifts.CancelShift(WorkShiftDuty.HeaterFuel, out _), Is.True);
            Assert.That(shifts.CancelShift(WorkShiftDuty.WaterPurification, out _), Is.True);
            Assert.That(shifts.CancelShift(WorkShiftDuty.RationPreparation, out _), Is.True);

            var restored = new SurvivorWorkShiftSystem(() => survivors, _ => true, null, null, () => context);
            restored.RestoreState(saved);
            var restoredFilter = FindSlot(restored.GetSnapshot(), WorkShiftDuty.AirFiltration).Availability;
            Assert.That(restoredFilter.RemainingHours, Is.EqualTo(16f));
            Assert.That(restoredFilter.CurrentBurnPerHour, Is.EqualTo(2f));
            Assert.That(restoredFilter.Status, Is.EqualTo(WorkShiftAvailabilityStatus.Stable));
        }

        [Test]
        public void WorkShifts_ApplyScopedEffects_AndRestoreThemFromStaffing()
        {
            var survivors = new List<Survivor>
            {
                CreateSurvivor("elena_vasquez", "Elena Vasquez"),
                CreateSurvivor("marcus_reed", "Marcus Reed"),
                CreateSurvivor("nora_kim", "Nora Kim"),
                CreateSurvivor("dani_ortiz", "Dani Ortiz")
            };
            var shifts = new SurvivorWorkShiftSystem(() => survivors, _ => true);
            Assert.That(shifts.TryAssign(WorkShiftDuty.AirFiltration, "elena_vasquez", out _), Is.True);
            Assert.That(shifts.TryAssign(WorkShiftDuty.HeaterFuel, "marcus_reed", out _), Is.True);
            Assert.That(shifts.TryAssign(WorkShiftDuty.WaterPurification, "nora_kim", out _), Is.True);
            Assert.That(shifts.TryAssign(WorkShiftDuty.RationPreparation, "dani_ortiz", out _), Is.True);

            var shelter = new Shelter();
            var filter = new ShelterModuleInstance("air_filtration", 1) { FilterHealth = 100f };
            var heater = new ShelterModuleInstance("heater", 1) { Fuel = 20f };
            var purifier = new ShelterModuleInstance("water_purifier", 1) { FilterHealth = 100f };
            shelter.AddModule(filter);
            shelter.AddModule(heater);
            shelter.AddModule(purifier);
            shelter.SetModuleConsumptionMultiplierProvider(shifts.GetModuleResourceConsumptionMultiplier);
            shelter.Tick(1f);

            Assert.That(filter.FilterHealth, Is.EqualTo(98.5f).Within(0.001f),
                "A staffed filter should wear 25% more slowly.");
            Assert.That(heater.Fuel, Is.EqualTo(19.2f).Within(0.001f),
                "A tended heater should burn 20% less fuel.");

            var water = new WaterEconomySystem();
            water.SetPurifierHoursPerUnitMultiplierProvider(
                () => shifts.GetEffectsSnapshot().PurifierHoursPerUnitMultiplier);
            var storage = new WaterStorage { DirtyWater = 2f };
            water.Tick(1.5f, WeatherKind.Clear, 1, shelter, storage);
            Assert.That(storage.CleanWater, Is.EqualTo(1f).Within(0.001f),
                "A staffed purifier should complete its 2-hour conversion in 1.5 hours.");

            int food = 10;
            int waterUnits = 10;
            var rationing = new BunkerRationingSystem(
                resource => resource == RationResource.Food ? food : waterUnits,
                (resource, requested) =>
                {
                    if (resource == RationResource.Food)
                    {
                        int issued = Mathf.Min(food, requested);
                        food -= issued;
                        return issued;
                    }
                    int issuedWater = Mathf.Min(waterUnits, requested);
                    waterUnits -= issuedWater;
                    return issuedWater;
                });
            rationing.SetRationRestoreMultiplierProvider(
                () => shifts.GetEffectsSnapshot().RationRestoreMultiplier);
            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            _toDestroy.Add(profile);
            var needs = new NeedsSystem(profile);
            Assert.That(rationing.ApplyDailyRations(1, survivors, needs), Is.True);
            Assert.That(survivors[0].Needs.Hunger, Is.EqualTo(56f).Within(0.001f));
            Assert.That(survivors[0].Needs.Thirst, Is.EqualTo(45f).Within(0.001f));

            var saved = shifts.CaptureState();
            Assert.That(shifts.CancelShift(WorkShiftDuty.AirFiltration, out _), Is.True);
            Assert.That(shifts.CancelShift(WorkShiftDuty.HeaterFuel, out _), Is.True);
            Assert.That(shifts.CancelShift(WorkShiftDuty.WaterPurification, out _), Is.True);
            Assert.That(shifts.CancelShift(WorkShiftDuty.RationPreparation, out _), Is.True);
            var restored = new SurvivorWorkShiftSystem(() => survivors, _ => true);
            restored.RestoreState(saved);
            var restoredEffects = restored.GetEffectsSnapshot();
            Assert.That(restoredEffects.FilterWearMultiplier,
                Is.EqualTo(SurvivorWorkShiftSystem.SupervisedFilterWearMultiplier));
            Assert.That(restoredEffects.HeaterFuelBurnMultiplier,
                Is.EqualTo(SurvivorWorkShiftSystem.TendedHeaterFuelBurnMultiplier));
            Assert.That(restoredEffects.PurifierHoursPerUnitMultiplier,
                Is.EqualTo(SurvivorWorkShiftSystem.SupervisedPurifierHoursPerUnitMultiplier));
            Assert.That(restoredEffects.RationRestoreMultiplier,
                Is.EqualTo(SurvivorWorkShiftSystem.PreparedRationRestoreMultiplier));
        }

        private static Survivor CreateSurvivor(string id, string displayName)
        {
            var survivor = new Survivor { Id = id, DisplayName = displayName };
            survivor.Needs.Hunger = 100f;
            survivor.Needs.Thirst = 100f;
            return survivor;
        }

        private static SurvivorWorkShiftSlotSnapshot FindSlot(
            SurvivorWorkShiftSnapshot snapshot,
            WorkShiftDuty duty)
        {
            for (int i = 0; i < snapshot.Slots.Count; i++)
                if (snapshot.Slots[i].Duty == duty) return snapshot.Slots[i];
            Assert.Fail("Missing shift slot: " + duty + ".");
            return null;
        }
    }
}
