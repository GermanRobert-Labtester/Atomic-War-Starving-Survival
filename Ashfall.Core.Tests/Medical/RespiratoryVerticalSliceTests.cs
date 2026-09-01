// SPDX-License-Identifier: MIT
// Task #133 — Respiratory vertical slice: parity, single-progression, longitudinal.
using System;
using System.Linq;
using Ashfall.Core.Medical;
using Ashfall.Core.PlayerCommand;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    /// <summary>
    /// Phase 26/27/62 of the Task #133 plan: the respiratory vertical slice
    /// proves the complete pipeline — cause → condition → symptom → diagnosis →
    /// preview → reservation → execution → outcome → save/load — while the
    /// respiratory domain keeps behavioral parity (identical numbers, single
    /// progression owner).
    /// </summary>
    public class RespiratoryVerticalSliceTests
    {
        private const string SvId = "survivor_slice_patient";

        private sealed class Fixture
        {
            public Ashfall.Core.Inventory.Inventory Inventory { get; }
                = new Ashfall.Core.Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            public DiagnosisKnowledgeStore Diagnosis { get; } = new DiagnosisKnowledgeStore();
            public MedicalReservationLedger Reservations { get; } = new MedicalReservationLedger();
            public MedicalProcedureSchedule Schedule { get; } = new MedicalProcedureSchedule();
            public RespiratoryDegenerationSystem Respiratory { get; } = new RespiratoryDegenerationSystem();
            public MedicalPipelineCoordinator Pipeline { get; }
            public PatientRecordProjector Projector { get; }
            public int Day = 1;

            public Fixture()
            {
                Pipeline = new MedicalPipelineCoordinator(
                    Inventory, Diagnosis, Reservations, Schedule,
                    _ => PatientAvailability.Ok(), () => Day);
                Pipeline.RegisterHandler(new RespiratoryAfflictionHandler(Respiratory));
                Projector = new PatientRecordProjector(Pipeline);
                Inventory.TryProduce("inhaler", 5);
                Inventory.TryProduce("herbal_tea", 5);
            }

            public Ashfall.Core.Survivors.SurvivorId Sv => Ashfall.Core.Survivors.SurvivorId.Parse(SvId);
        }

        private static string RespiratoryChecksum(RespiratoryDegenerationSystem system)
            => Ashfall.Core.SaveChecksum.Compute(system.CaptureState());

        // ── Parity (Phase 27): pipeline treatment == legacy direct application ──

        [Fact]
        public void PipelineInhaler_MatchesLegacyDirectApplication_Exactly()
        {
            var legacy = new RespiratoryDegenerationSystem();
            legacy.GetOrCreate(SvId).respiratoryDegradation = 55f;
            legacy.ApplyInhaler(SvId);

            var fx = new Fixture();
            fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 55f;
            var result = fx.Pipeline.ExecuteTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler);

            Assert.True(result.Success, result.ReasonCode);
            Assert.Equal(
                RespiratoryChecksum(legacy),
                RespiratoryChecksum(fx.Respiratory));
        }

        [Fact]
        public void PipelineHerbalTea_MatchesLegacyDirectApplication_Exactly()
        {
            var legacy = new RespiratoryDegenerationSystem();
            legacy.GetOrCreate(SvId).respiratoryDegradation = 12f;
            legacy.ApplyHerbalTea(SvId);

            var fx = new Fixture();
            fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 12f;
            var result = fx.Pipeline.ExecuteTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentHerbalTea);

            Assert.True(result.Success, result.ReasonCode);
            Assert.Equal(
                RespiratoryChecksum(legacy),
                RespiratoryChecksum(fx.Respiratory));
        }

        // ── Single progression owner (Phase 64): the pipeline never ticks ──

        [Fact]
        public void Treatment_DoesNotAdvanceProgression_OnlyAppliesRelief()
        {
            var fx = new Fixture();
            fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 30f;

            fx.Pipeline.ExecuteTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler);

            // Legacy ApplyInhaler semantics exactly: −10 degradation, 8h relief,
            // and zero time-based progression (the handler never ticks).
            Assert.Equal(20f, fx.Respiratory.RespiratoryDegradation(SvId), 3);
            Assert.Equal(RespiratoryDegenerationSystem.InhalerReliefDurationHours,
                fx.Respiratory.InhalerReliefHours(SvId), 3);
        }

        // ── Longitudinal scenario (Phase 66): exposure → symptom → diagnose →
        //    treatment → recovery, with real TickHours progression between ──

        [Fact]
        public void Longitudinal_ExposureToRecovery_ThroughPipelineAndRealTicks()
        {
            var fx = new Fixture();
            var respiratoryDef = AfflictionId.Parse(MedicalTreatmentCatalog.RespiratoryDegenerationId);
            var episode = AfflictionEpisodeId.Create(fx.Sv, respiratoryDef);

            // 1. Cause: fallout-storm exposure advances the domain exactly as
            //    the Phase-0 day owner does (0.5/hour).
            fx.Respiratory.IsInFalloutStorm = () => true;
            fx.Respiratory.GetFilterHealth = () => 100f;
            fx.Respiratory.TickHours(SvId, 24f); // day 1: +12 degradation
            Assert.Equal(12f, fx.Respiratory.RespiratoryDegradation(SvId), 3);
            Assert.Equal(DiagnosisStatus.Unknown, fx.Pipeline.Diagnosis.GetStatus(episode));

            // 2. Deterioration to symptom threshold raises Suspected automatically
            //    (production wiring listens to OnSevereCoughStarted).
            fx.Respiratory.TickHours(SvId, 24f * 4f); // crosses 50
            fx.Pipeline.SuspectFromEvidence(fx.Sv, respiratoryDef, 5, "severe_cough_threshold");
            Assert.Equal(DiagnosisStatus.Suspected, fx.Pipeline.Diagnosis.GetStatus(episode));

            // 3. Explicit diagnose → Confirmed.
            fx.Pipeline.ExecuteDiagnose(fx.Sv, respiratoryDef);
            Assert.Equal(DiagnosisStatus.Confirmed, fx.Pipeline.Diagnosis.GetStatus(episode));

            // 4. Treatment through the validated transaction.
            var treated = fx.Pipeline.ExecuteTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler);
            Assert.True(treated.Success, treated.ReasonCode);
            Assert.Equal(4, fx.Inventory.CountById("inhaler"));

            // 5. More exposure continues through the real domain tick — the
            //    pipeline did not take over progression.
            fx.Respiratory.TickHours(SvId, 24f);
            Assert.True(fx.Respiratory.RespiratoryDegradation(SvId) > 0f);
        }

        // ── Save/restore mid-affliction + mid-knowledge (Phase 67/68) ──

        [Fact]
        public void MidAfflictionRoundTrip_PreservesConditionAndKnowledge()
        {
            var fx = new Fixture();
            fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 60f;
            fx.Pipeline.ExecuteDiagnose(fx.Sv, AfflictionId.Parse(MedicalTreatmentCatalog.RespiratoryDegenerationId));

            var respiratorySave = fx.Respiratory.CaptureState();
            var pipelineSave = fx.Pipeline.CaptureState();

            // Restore into fresh instances (the SetupXxx load path).
            var respiratory2 = new RespiratoryDegenerationSystem();
            respiratory2.RestoreState(respiratorySave);
            var pipeline2 = new MedicalPipelineCoordinator(
                fx.Inventory, new DiagnosisKnowledgeStore(), new MedicalReservationLedger(),
                new MedicalProcedureSchedule(), _ => PatientAvailability.Ok(), () => 1);
            pipeline2.RegisterHandler(new RespiratoryAfflictionHandler(respiratory2));
            pipeline2.RestoreState(pipelineSave);

            // Identical continuation: same treatment → same result.
            var outcome1 = fx.Pipeline.ExecuteTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler);
            var outcome2 = pipeline2.ExecuteTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler);
            Assert.True(outcome1.Success && outcome2.Success);
            Assert.Equal(
                fx.Respiratory.RespiratoryDegradation(SvId),
                respiratory2.RespiratoryDegradation(SvId), 3);
        }

        // ── Diagnosis does not leak underlying truth before confirmation ──

        [Fact]
        public void MildCondition_WithoutEvidence_StaysUnknown_SymptomStillObservable()
        {
            var fx = new Fixture();
            fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 20f;

            var record = fx.Projector.Project(fx.Sv);

            Assert.Equal("unknown", record.Afflictions.Single().DiagnosisStatus);
            Assert.False(record.Afflictions.Single().SeverityDisclosed);
            var symptom = record.Symptoms.Single(s => s.SymptomId == RespiratoryAfflictionHandler.SymptomOccasionalCough);
            Assert.Equal("Occasional cough", symptom.Presentation);
        }
    }
}
