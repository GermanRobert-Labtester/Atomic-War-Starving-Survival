// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    /// <summary>
    /// Plan 54 proof over the real pipeline, schedule ledger, inventory
    /// reservation path, and respiratory domain. The 30-day loop is scripted
    /// so a failure prints a reproducible day/action rather than relying on a
    /// probabilistic workload.
    /// </summary>
    public sealed class MedicalPipelinePhase2Tests
    {
        private const string SurvivorId = "phase2_medical_patient";

        private sealed class Fixture
        {
            public Ashfall.Core.Inventory.Inventory Inventory { get; }
                = new Ashfall.Core.Inventory.Inventory { Capacity = 64, MaxWeight = 200f };
            public DiagnosisKnowledgeStore Diagnosis { get; } = new DiagnosisKnowledgeStore();
            public MedicalReservationLedger Reservations { get; } = new MedicalReservationLedger();
            public MedicalProcedureSchedule Schedule { get; } = new MedicalProcedureSchedule();
            public RespiratoryDegenerationSystem Respiratory { get; } = new RespiratoryDegenerationSystem();
            public MedicalPipelineCoordinator Pipeline { get; }
            public int Day = 1;

            public Fixture(bool researched = true)
            {
                Inventory.AddById(MedicalTreatmentCatalog.ItemOxygenSupply, 30);
                Pipeline = new MedicalPipelineCoordinator(
                    Inventory,
                    Diagnosis,
                    Reservations,
                    Schedule,
                    _ => PatientAvailability.Ok(),
                    () => Day,
                    capabilityCheck: id => researched && id == "knowledge_chelation_therapy");
                Pipeline.RegisterHandler(new RespiratoryAfflictionHandler(Respiratory));
                Respiratory.IsInFalloutStorm = () => true;
                Respiratory.GetFilterHealth = () => 100f;
                Respiratory.GetOrCreate(SurvivorId).respiratoryDegradation = 20f;
            }

            public Ashfall.Core.Survivors.SurvivorId Survivor =>
                Ashfall.Core.Survivors.SurvivorId.Parse(SurvivorId);
        }

        [Fact]
        public void AntiRadTreatment_RequiresSharedResearch_WhenCapabilityProviderIsBound()
        {
            var locked = new Fixture(researched: false);
            locked.Inventory.AddById(MedicalTreatmentCatalog.ItemAntiRad, 1);
            var dose = new System.Collections.Generic.Dictionary<string, float> { [SurvivorId] = 100f };
            var handler = new RadiationSicknessAfflictionHandler(
                id => dose.TryGetValue(id, out var value) ? value : 0f,
                _ => "ManifestIllness",
                _ => true,
                _ => true,
                (id, amount) => { dose[id] = Math.Max(0f, dose[id] - amount); return true; });
            locked.Pipeline.RegisterHandler(handler);

            var preview = locked.Pipeline.PreviewTreatment(
                locked.Survivor, MedicalTreatmentCatalog.TreatmentAntiRad);
            Assert.False(preview.IsAvailable);
            Assert.Equal("research_required", preview.FailureCode);
            Assert.Equal(1, locked.Inventory.CountById(MedicalTreatmentCatalog.ItemAntiRad));
        }

        [Fact]
        public void ThirtyDayScheduledTreatmentWorkload_IsDeterministicAndConservesOxygen()
        {
            var first = RunThirtyDayWorkload();
            var second = RunThirtyDayWorkload();

            Assert.Equal(30, first.completed);
            Assert.Equal(0, first.activeAtEnd);
            Assert.Equal(0, first.oxygenRemaining);
            Assert.Equal(first.completed, second.completed);
            Assert.Equal(first.oxygenRemaining, second.oxygenRemaining);
            Assert.Equal(first.finalDegradation, second.finalDegradation, 3);
            Assert.Equal(first.pipelineChecksum, second.pipelineChecksum);
        }

        [Fact]
        public void ActiveScheduledProcedure_RoundTripsAndResumesWithoutDuplicateConsumption()
        {
            var uninterrupted = new Fixture();
            var scheduled = uninterrupted.Pipeline.ExecuteTreatment(
                uninterrupted.Survivor, MedicalTreatmentCatalog.TreatmentOxygenSupport);
            Assert.True(scheduled.Success, scheduled.ReasonCode);
            Assert.Single(uninterrupted.Pipeline.Schedule.Active);
            Assert.Equal(1, uninterrupted.Reservations.ReservedQuantity(
                MedicalTreatmentCatalog.ItemOxygenSupply));

            var pipelineState = uninterrupted.Pipeline.CaptureState();
            var respiratoryState = uninterrupted.Respiratory.CaptureState();

            var restored = new Fixture();
            restored.Respiratory.RestoreState(respiratoryState);
            restored.Pipeline.RestoreState(pipelineState);
            restored.Day = 2;
            uninterrupted.Day = 2;

            Assert.Single(restored.Pipeline.Schedule.Active);
            Assert.Equal(1, restored.Reservations.ReservedQuantity(
                MedicalTreatmentCatalog.ItemOxygenSupply));

            var firstCompletion = uninterrupted.Pipeline.AdvanceScheduled(24f, 2);
            var restoredCompletion = restored.Pipeline.AdvanceScheduled(24f, 2);

            Assert.Single(firstCompletion);
            Assert.Single(restoredCompletion);
            Assert.Empty(uninterrupted.Pipeline.Schedule.Active);
            Assert.Empty(restored.Pipeline.Schedule.Active);
            Assert.Equal(29, uninterrupted.Inventory.CountById(
                MedicalTreatmentCatalog.ItemOxygenSupply));
            Assert.Equal(29, restored.Inventory.CountById(
                MedicalTreatmentCatalog.ItemOxygenSupply));
            Assert.Equal(uninterrupted.Respiratory.RespiratoryDegradation(SurvivorId),
                restored.Respiratory.RespiratoryDegradation(SurvivorId), 3);
            Assert.Equal(0, uninterrupted.Reservations.ReservedQuantity(
                MedicalTreatmentCatalog.ItemOxygenSupply));
            Assert.Equal(0, restored.Reservations.ReservedQuantity(
                MedicalTreatmentCatalog.ItemOxygenSupply));
        }

        private static (int completed, int activeAtEnd, int oxygenRemaining, float finalDegradation, string pipelineChecksum)
            RunThirtyDayWorkload()
        {
            var fx = new Fixture();
            int completed = 0;
            for (int day = 1; day <= 30; day++)
            {
                fx.Day = day;
                fx.Respiratory.TickHours(SurvivorId, 24f);
                if (fx.Pipeline.Schedule.Active.Count == 0 &&
                    fx.Respiratory.RespiratoryDegradation(SurvivorId) > 0f)
                {
                    var scheduled = fx.Pipeline.ExecuteTreatment(
                        fx.Survivor, MedicalTreatmentCatalog.TreatmentOxygenSupport);
                    Assert.True(scheduled.Success, $"day {day}: {scheduled.ReasonCode}");
                }

                var results = fx.Pipeline.AdvanceScheduled(24f, day);
                completed += results.Count;
                Assert.Equal(0, fx.Reservations.ReservedQuantity(MedicalTreatmentCatalog.ItemOxygenSupply));
            }

            return (
                completed,
                fx.Pipeline.Schedule.Active.Count,
                fx.Inventory.CountById(MedicalTreatmentCatalog.ItemOxygenSupply),
                fx.Respiratory.RespiratoryDegradation(SurvivorId),
                Ashfall.Core.SaveChecksum.Compute(fx.Pipeline.CaptureState()));
        }
    }
}
