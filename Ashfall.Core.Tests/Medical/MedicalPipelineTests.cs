// SPDX-License-Identifier: MIT
// Task #133 — Unified medical pipeline: identity, knowledge, transactions, determinism.
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Medical;
using Ashfall.Core.PlayerCommand;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public class AfflictionIdTests
    {
        [Fact]
        public void EqualValues_AreEqual_Ordinal()
        {
            var a = AfflictionId.Parse("affliction_respiratory_degeneration");
            var b = AfflictionId.Parse("affliction_respiratory_degeneration");
            Assert.Equal(a, b);
            Assert.True(a == b);
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Affliction_Upper")]
        [InlineData("has-dash")]
        [InlineData("_leading")]
        [InlineData("trailing_")]
        [InlineData("double__underscore")]
        [InlineData("way-too-long-because-it-exceeds-the-sixty-four-character-limit-for-ids-x")]
        public void InvalidValues_AreRejected(string? value)
        {
            Assert.False(AfflictionId.IsValid(value, out _));
            Assert.Throws<ArgumentException>(() => AfflictionId.Parse(value!));
        }

        [Fact]
        public void JsonRoundTrip_PreservesIdentity()
        {
            var serializer = new Ashfall.Core.SystemTextJsonSerializer();
            var holder = new IdHolder { Id = AfflictionId.Parse("affliction_radiation_sickness") };
            string json = serializer.Serialize(holder);
            var restored = serializer.Deserialize<IdHolder>(json);
            Assert.Equal(holder.Id, restored!.Id);
        }

        private sealed class IdHolder
        {
            public AfflictionId Id { get; set; }
        }
    }

    public class AfflictionEpisodeIdTests
    {
        [Fact]
        public void Create_BuildsDeterministicComposite()
        {
            var survivor = Ashfall.Core.Survivors.SurvivorId.Parse("survivor_mara");
            var def = AfflictionId.Parse("affliction_respiratory_degeneration");
            var e1 = AfflictionEpisodeId.Create(survivor, def);
            var e2 = AfflictionEpisodeId.Create(survivor, def);
            Assert.Equal(e1, e2);
            Assert.Equal("survivor_mara:affliction_respiratory_degeneration:0", e1.Value);
        }

        [Fact]
        public void Create_SecondWound_GetsDistinctEpisode()
        {
            var survivor = Ashfall.Core.Survivors.SurvivorId.Parse("survivor_mara");
            var def = AfflictionId.Parse("affliction_wound");
            var first = AfflictionEpisodeId.Create(survivor, def, 0);
            var second = AfflictionEpisodeId.Create(survivor, def, 1);
            Assert.NotEqual(first, second);
        }

        [Fact]
        public void Parse_ExtractsSegments()
        {
            var parsed = AfflictionEpisodeId.TryParse("the_mare:affliction_radiation_sickness:2", out var id, out _);
            Assert.True(parsed);
            Assert.Equal("the_mare", id.Survivor.Value);
            Assert.Equal("affliction_radiation_sickness", id.Definition.Value);
            Assert.Equal(2, id.Ordinal);
        }

        [Theory]
        [InlineData("")]
        [InlineData("no-separators")]
        [InlineData("survivor_x:affliction_y")]
        [InlineData("survivor_x:affliction_y:notanumber")]
        [InlineData("survivor_x:affliction_y:-1")]
        [InlineData(":affliction_y:0")]
        public void InvalidComposites_AreRejected(string value)
        {
            Assert.False(AfflictionEpisodeId.IsValid(value, out _));
        }
    }

    public class DiagnosisKnowledgeStoreTests
    {
        [Fact]
        public void UnderlyingCondition_CanExist_WithoutDiagnosis()
        {
            var store = new DiagnosisKnowledgeStore();
            var episode = AfflictionEpisodeId.Create(
                Ashfall.Core.Survivors.SurvivorId.Parse("survivor_mara"),
                AfflictionId.Parse("affliction_respiratory_degeneration"));
            Assert.Equal(DiagnosisStatus.Unknown, store.GetStatus(episode));
        }

        [Fact]
        public void Promote_GoesUnknownToSuspectedToConfirmed_NeverLowering()
        {
            var store = new DiagnosisKnowledgeStore();
            var episode = AfflictionEpisodeId.Create(
                Ashfall.Core.Survivors.SurvivorId.Parse("survivor_mara"),
                AfflictionId.Parse("affliction_respiratory_degeneration"));

            Assert.Equal(DiagnosisStatus.Suspected, store.Promote(episode, day: 3));
            Assert.Equal(DiagnosisStatus.Confirmed, store.Promote(episode, day: 5));
            Assert.Equal(DiagnosisStatus.Confirmed, store.Promote(episode, day: 9));
            Assert.Equal(DiagnosisStatus.Confirmed, store.GetStatus(episode));
        }

        [Fact]
        public void RoundTrip_PreservesRecords()
        {
            var store = new DiagnosisKnowledgeStore();
            var episode = AfflictionEpisodeId.Create(
                Ashfall.Core.Survivors.SurvivorId.Parse("survivor_mara"),
                AfflictionId.Parse("affliction_radiation_sickness"));
            store.Confirm(episode, 7);
            string before = Ashfall.Core.SaveChecksum.Compute(store.CaptureState());

            var restored = new DiagnosisKnowledgeStore();
            restored.RestoreState(store.CaptureState());
            Assert.Equal(before, Ashfall.Core.SaveChecksum.Compute(restored.CaptureState()));
            Assert.Equal(DiagnosisStatus.Confirmed, restored.GetStatus(episode));
        }

        [Fact]
        public void Restore_DropsInvalidEpisodes()
        {
            var saved = new DiagnosisKnowledgeSaveState();
            saved.records.Add(new DiagnosisRecord { episodeId = "not-valid", status = "confirmed" });
            var store = new DiagnosisKnowledgeStore();
            store.RestoreState(saved);
            Assert.Empty(store.Records);
        }
    }

    public class MedicalReservationLedgerTests
    {
        [Fact]
        public void Reserve_IncreasesClaim_ReleaseFreesIt()
        {
            var ledger = new MedicalReservationLedger();
            var sv = Ashfall.Core.Survivors.SurvivorId.Parse("survivor_mara");
            int id = ledger.Reserve(sv, MedicalReservationKind.Medicine, "inhaler", 1, "treatment_inhaler");
            Assert.True(id > 0);
            Assert.Equal(1, ledger.ReservedQuantity("inhaler"));
            Assert.True(ledger.Release(id));
            Assert.Equal(0, ledger.ReservedQuantity("inhaler"));
        }

        [Fact]
        public void TwoReservations_SumClaims()
        {
            var ledger = new MedicalReservationLedger();
            ledger.Reserve(Ashfall.Core.Survivors.SurvivorId.Parse("survivor_a"), MedicalReservationKind.Medicine, "rad_away", 1, "treatment_anti_rad");
            ledger.Reserve(Ashfall.Core.Survivors.SurvivorId.Parse("survivor_b"), MedicalReservationKind.Medicine, "rad_away", 2, "treatment_anti_rad");
            Assert.Equal(3, ledger.ReservedQuantity("rad_away"));
        }

        [Fact]
        public void RoundTrip_PreservesReservations_AndCounter()
        {
            var ledger = new MedicalReservationLedger();
            var sv = Ashfall.Core.Survivors.SurvivorId.Parse("survivor_mara");
            int id = ledger.Reserve(sv, MedicalReservationKind.Medicine, "inhaler", 1, "treatment_inhaler");
            var restored = new MedicalReservationLedger();
            restored.RestoreState(ledger.CaptureState());
            Assert.True(restored.TryGet(id, out var row));
            Assert.Equal("inhaler", row.targetId);
            int next = restored.Reserve(sv, MedicalReservationKind.Medicine, "inhaler", 1, "treatment_inhaler");
            Assert.True(next > id, "restored counter must never re-issue ids");
        }
    }

    public class MedicalProcedureScheduleTests
    {
        [Fact]
        public void Scheduled_Advances_OnlyViaAdvance()
        {
            var schedule = new MedicalProcedureSchedule();
            var sv = Ashfall.Core.Survivors.SurvivorId.Parse("survivor_mara");
            int id = schedule.Schedule(sv, "treatment_detox", AfflictionEpisodeId.Create(sv, AfflictionId.Parse("affliction_chemical_dependency")), 1, 48f, Array.Empty<int>());
            Assert.True(schedule.HasActiveProcedure(sv, "treatment_detox"));

            var none = schedule.Advance(10f, 1);
            Assert.Empty(none);
            Assert.True(schedule.TryGetActive(id, out var row));
            Assert.Equal(38f, row.remainingHours, 3);

            var completions = schedule.Advance(38f, 3);
            Assert.Single(completions);
            Assert.Equal(id, completions[0].ProcedureId);
            Assert.False(schedule.HasActiveProcedure(sv, "treatment_detox"));
        }

        [Fact]
        public void Cancel_ReleasesRow_ToHistory()
        {
            var schedule = new MedicalProcedureSchedule();
            var sv = Ashfall.Core.Survivors.SurvivorId.Parse("survivor_mara");
            int id = schedule.Schedule(sv, "treatment_x", AfflictionEpisodeId.Create(sv, AfflictionId.Parse("affliction_a")), 1, 10f, Array.Empty<int>());
            var cancelled = schedule.Cancel(id, 2);
            Assert.NotNull(cancelled);
            Assert.Equal("cancelled", cancelled!.status);
            Assert.False(schedule.HasActiveProcedure(sv, "treatment_x"));
        }

        [Fact]
        public void MidProcedureRoundTrip_PreservesRemainingHours()
        {
            var schedule = new MedicalProcedureSchedule();
            var sv = Ashfall.Core.Survivors.SurvivorId.Parse("survivor_mara");
            int id = schedule.Schedule(sv, "treatment_x", AfflictionEpisodeId.Create(sv, AfflictionId.Parse("affliction_a")), 1, 72f, new List<int> { 5 });

            schedule.Advance(30f, 2);
            var saved = schedule.CaptureState();

            var restored = new MedicalProcedureSchedule();
            restored.RestoreState(saved);
            Assert.True(restored.TryGetActive(id, out var row));
            Assert.Equal(42f, row.remainingHours, 3);
            Assert.Contains(5, row.reservationIds);

            // Continuing produces the identical completion.
            var completions = restored.Advance(42f, 4);
            Assert.Single(completions);
        }
    }

    /// <summary>
    /// Coordinator transaction tests. The respiratory handler runs against the
    /// real <see cref="RespiratoryDegenerationSystem"/>; the availability
    /// callback is a stub standing in for the canonical survivor lifecycle.
    /// </summary>
    public class MedicalPipelineCoordinatorTests
    {
        private const string SvId = "survivor_test_patient";

        private sealed class Fixture
        {
            public Ashfall.Core.Inventory.Inventory Inventory { get; } = new Ashfall.Core.Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            public DiagnosisKnowledgeStore Diagnosis { get; } = new DiagnosisKnowledgeStore();
            public MedicalReservationLedger Reservations { get; } = new MedicalReservationLedger();
            public MedicalProcedureSchedule Schedule { get; } = new MedicalProcedureSchedule();
            public RespiratoryDegenerationSystem Respiratory { get; } = new RespiratoryDegenerationSystem();
            public MedicalPipelineCoordinator Pipeline { get; }
            public int Day = 1;
            public bool PatientAvailable = true;
            public string AvailabilityReason = "ok";

            public Fixture()
            {
                var respiratoryHandler = new RespiratoryAfflictionHandler(Respiratory);
                Pipeline = new MedicalPipelineCoordinator(
                    Inventory, Diagnosis, Reservations, Schedule,
                    _ => PatientAvailable ? PatientAvailability.Ok() : PatientAvailability.Blocked(AvailabilityReason),
                    () => Day);
                Pipeline.RegisterHandler(respiratoryHandler);
                Inventory.TryProduce("inhaler", 5);
                Inventory.TryProduce("herbal_tea", 5);
            }

            public Ashfall.Core.Survivors.SurvivorId Sv => Ashfall.Core.Survivors.SurvivorId.Parse(SvId);
        }

        [Fact]
        public void PreviewTreatment_IsSideEffectFree()
        {
            var fx = new Fixture();
            fx.Respiratory.TickHours(SvId, 4f); // ash-free indoor rate is 0; force exposure
            fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 20f;
            string before = Ashfall.Core.SaveChecksum.Compute(fx.Respiratory.CaptureState())
                + "|" + fx.Inventory.CountById("inhaler")
                + "|" + fx.Reservations.ReservedQuantity("inhaler");

            var preview = fx.Pipeline.PreviewTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler);

            Assert.True(preview.IsAvailable);
            string after = Ashfall.Core.SaveChecksum.Compute(fx.Respiratory.CaptureState())
                + "|" + fx.Inventory.CountById("inhaler")
                + "|" + fx.Reservations.ReservedQuantity("inhaler");
            Assert.Equal(before, after);
        }

        [Fact]
        public void ExecuteTreatment_ReservesOnce_ConsumesOnce_Applies()
        {
            var fx = new Fixture();
            fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 20f;

            var result = fx.Pipeline.ExecuteTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler);

            Assert.True(result.Success, result.ReasonCode);
            Assert.Equal(4, fx.Inventory.CountById("inhaler"));
            Assert.Equal(0, fx.Reservations.ReservedQuantity("inhaler"));
            Assert.Equal(10f, fx.Respiratory.RespiratoryDegradation(SvId), 3); // 20 − 10
        }

        [Fact]
        public void MissingMedicine_BlocksTreatment()
        {
            var fx = new Fixture();
            fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 20f;
            fx.Inventory.TryConsume("inhaler", 5); // none left

            var preview = fx.Pipeline.PreviewTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler);
            Assert.False(preview.IsAvailable);
            Assert.Equal("missing_medicine", preview.FailureCode);

            var result = fx.Pipeline.ExecuteTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler);
            Assert.False(result.Success);
            Assert.Equal("missing_medicine", result.ReasonCode);
            // Nothing changed: no reservations leaked.
            Assert.Equal(0, fx.Reservations.ReservedQuantity("inhaler"));
        }

        [Fact]
        public void Contradiction_ZeroDegradation_BlocksInhaler()
        {
            var fx = new Fixture();
            var preview = fx.Pipeline.PreviewTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler);
            Assert.False(preview.IsAvailable);
            Assert.Equal("no_respiratory_damage", preview.FailureCode);
        }

        [Fact]
        public void PatientUnavailable_BlocksTreatment_WithLifecycleReason()
        {
            var fx = new Fixture { PatientAvailable = false, AvailabilityReason = "patient_away" };
            fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 20f;

            var preview = fx.Pipeline.PreviewTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler);
            Assert.False(preview.IsAvailable);
            Assert.Equal("patient_away", preview.FailureCode);

            var result = fx.Pipeline.ExecuteTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler);
            Assert.False(result.Success);
            Assert.Equal(5, fx.Inventory.CountById("inhaler")); // no consumption
        }

        [Fact]
        public void StalePreview_IsRejected()
        {
            var fx = new Fixture();
            fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 20f;
            // Advance past version 0 so a captured version is genuinely stale.
            fx.Pipeline.ExecuteDiagnose(fx.Sv, AfflictionId.Parse(MedicalTreatmentCatalog.RespiratoryDegenerationId));
            long staleVersion = fx.Pipeline.StateVersion;
            fx.Pipeline.ExecuteTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentHerbalTea);
            Assert.NotEqual(staleVersion, fx.Pipeline.StateVersion);

            var preview = fx.Pipeline.PreviewTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler, expectedVersion: staleVersion);
            Assert.False(preview.IsAvailable);
            Assert.Equal("stale_preview", preview.FailureCode);
        }

        [Fact]
        public void Diagnose_MutatesOnlyKnowledge()
        {
            var fx = new Fixture();
            fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 20f;

            var result = fx.Pipeline.ExecuteDiagnose(fx.Sv, AfflictionId.Parse(MedicalTreatmentCatalog.RespiratoryDegenerationId));

            Assert.True(result.Success, result.ReasonCode);
            var episode = AfflictionEpisodeId.Create(fx.Sv, AfflictionId.Parse(MedicalTreatmentCatalog.RespiratoryDegenerationId));
            Assert.Equal(DiagnosisStatus.Confirmed, fx.Pipeline.Diagnosis.GetStatus(episode));
            Assert.Equal(5, fx.Inventory.CountById("inhaler")); // no cost
            Assert.Equal(20f, fx.Respiratory.RespiratoryDegradation(SvId), 3); // no clinical change
        }

        [Fact]
        public void Diagnose_HealthySurvivor_IsRefused()
        {
            var fx = new Fixture();
            var result = fx.Pipeline.ExecuteDiagnose(fx.Sv, AfflictionId.Parse(MedicalTreatmentCatalog.RespiratoryDegenerationId));
            Assert.False(result.Success);
            Assert.Equal("no_plausible_condition", result.ReasonCode);
        }

        [Fact]
        public void SuspectFromEvidence_NeverConfirms_AndIsIdempotent()
        {
            var fx = new Fixture();
            var def = AfflictionId.Parse(MedicalTreatmentCatalog.RespiratoryDegenerationId);
            fx.Pipeline.SuspectFromEvidence(fx.Sv, def, 2, "severe_cough_threshold");
            fx.Pipeline.SuspectFromEvidence(fx.Sv, def, 4, "severe_cough_threshold");

            var episode = AfflictionEpisodeId.Create(fx.Sv, def);
            Assert.Equal(DiagnosisStatus.Suspected, fx.Pipeline.Diagnosis.GetStatus(episode));
        }

        [Fact]
        public void LegacyMigration_ConfirmsOnce()
        {
            var fx = new Fixture();
            var def = AfflictionId.Parse(MedicalTreatmentCatalog.RespiratoryDegenerationId);
            fx.Pipeline.ConfirmForLegacySave(fx.Sv, def, 1);
            fx.Pipeline.ConfirmForLegacySave(fx.Sv, def, 2);
            var episode = AfflictionEpisodeId.Create(fx.Sv, def);
            Assert.Equal(DiagnosisStatus.Confirmed, fx.Pipeline.Diagnosis.GetStatus(episode));
            Assert.Equal(1, fx.Pipeline.Diagnosis.Records.Count);
        }

        [Fact]
        public void PipelineRoundTrip_PreservesKnowledgeAndVersion()
        {
            var fx = new Fixture();
            fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 20f;
            fx.Pipeline.ExecuteDiagnose(fx.Sv, AfflictionId.Parse(MedicalTreatmentCatalog.RespiratoryDegenerationId));
            long version = fx.Pipeline.StateVersion;
            var saved = fx.Pipeline.CaptureState();

            var restored = new MedicalPipelineCoordinator(
                fx.Inventory, new DiagnosisKnowledgeStore(), new MedicalReservationLedger(),
                new MedicalProcedureSchedule(),
                _ => PatientAvailability.Ok(), () => 1);
            restored.RestoreState(saved);

            var episode = AfflictionEpisodeId.Create(fx.Sv, AfflictionId.Parse(MedicalTreatmentCatalog.RespiratoryDegenerationId));
            Assert.Equal(DiagnosisStatus.Confirmed, restored.Diagnosis.GetStatus(episode));
            Assert.Equal(version, restored.StateVersion);
        }

        [Fact]
        public void SameStartingState_SameCommands_ProduceSameOutcome()
        {
            RunDeterminismPair(out float a1, out float a2, out long v1, out long v2);
            Assert.Equal(a1, a2);
            Assert.Equal(v1, v2);
        }

        private static void RunDeterminismPair(out float finalA, out float finalB, out long versionA, out long versionB)
        {
            float[] finals = new float[2];
            long[] versions = new long[2];
            for (int run = 0; run < 2; run++)
            {
                var fx = new Fixture();
                fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 60f;
                var def = AfflictionId.Parse(MedicalTreatmentCatalog.RespiratoryDegenerationId);
                fx.Pipeline.SuspectFromEvidence(fx.Sv, def, 1, "severe_cough_threshold");
                fx.Pipeline.ExecuteDiagnose(fx.Sv, def);
                fx.Pipeline.ExecuteTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentHerbalTea);
                fx.Pipeline.ExecuteTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler);
                finals[run] = fx.Respiratory.RespiratoryDegradation(SvId);
                versions[run] = fx.Pipeline.StateVersion;
            }
            finalA = finals[0]; finalB = finals[1];
            versionA = versions[0]; versionB = versions[1];
        }

        [Fact]
        public void ReconcilePatientDeath_CancelsProcedures_AndReleasesClaims()
        {
            var fx = new Fixture();
            fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 90f;
            int reservation = fx.Reservations.Reserve(fx.Sv, MedicalReservationKind.Medicine, "inhaler", 1, "treatment_inhaler");

            fx.Pipeline.ReconcilePatientDeath(fx.Sv, 5);

            Assert.Equal(0, fx.Reservations.ReservedQuantity("inhaler"));
            Assert.False(fx.Reservations.TryGet(reservation, out _));
        }

        [Fact]
        public void TreatmentRefused_FiresOnlyOnBlockedExecution()
        {
            var fx = new Fixture();
            var refusals = new List<string>();
            fx.Pipeline.OnTreatmentRefused += (t, s, reason) => refusals.Add(reason);

            fx.Pipeline.ExecuteTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler); // blocked: no damage
            Assert.Single(refusals);
            Assert.Equal("no_respiratory_damage", refusals[0]);
        }

        [Fact]
        public void PatientRecovered_FiresWhenSeverityReachesZero()
        {
            var fx = new Fixture();
            fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 3f;
            var recovered = new List<string>();
            fx.Pipeline.OnPatientRecovered += (affliction, s) => recovered.Add(affliction);

            var result = fx.Pipeline.ExecuteTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentHerbalTea);

            Assert.True(result.Success, result.ReasonCode);
            Assert.Single(recovered); // 3 − 3 = 0 → resolved
        }
    }

    public class PatientRecordProjectorTests
    {
        private const string SvId = "survivor_record_patient";

        private sealed class Fixture
        {
            public Ashfall.Core.Inventory.Inventory Inventory { get; } = new Ashfall.Core.Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            public RespiratoryDegenerationSystem Respiratory { get; } = new RespiratoryDegenerationSystem();
            public MedicalPipelineCoordinator Pipeline { get; }
            public PatientRecordProjector Projector { get; }

            public Fixture()
            {
                Pipeline = new MedicalPipelineCoordinator(
                    Inventory, new DiagnosisKnowledgeStore(), new MedicalReservationLedger(),
                    new MedicalProcedureSchedule(),
                    _ => PatientAvailability.Ok(), () => 1);
                Pipeline.RegisterHandler(new RespiratoryAfflictionHandler(Respiratory));
                Inventory.TryProduce("inhaler", 3);
                Inventory.TryProduce("herbal_tea", 3);
                Projector = new PatientRecordProjector(Pipeline);
            }

            public Ashfall.Core.Survivors.SurvivorId Sv => Ashfall.Core.Survivors.SurvivorId.Parse(SvId);
        }

        [Fact]
        public void HealthySurvivor_HasNoAfflictions()
        {
            var fx = new Fixture();
            var record = fx.Projector.Project(fx.Sv);
            Assert.Empty(record.Afflictions);
            Assert.Empty(record.Symptoms);
            Assert.Equal("stable", record.Prognosis);
        }

        [Fact]
        public void UnconfirmedCondition_HidesSeverity_ShowsSymptom()
        {
            var fx = new Fixture();
            fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 55f;
            // Severe-cough onset auto-suspects (host wires the domain event);
            // here we drive the same pipeline entry point directly.
            fx.Pipeline.SuspectFromEvidence(fx.Sv,
                AfflictionId.Parse(MedicalTreatmentCatalog.RespiratoryDegenerationId), 1, "severe_cough_threshold");

            var record = fx.Projector.Project(fx.Sv);

            Assert.Single(record.Afflictions);
            Assert.Equal("suspected", record.Afflictions[0].DiagnosisStatus);
            Assert.False(record.Afflictions[0].SeverityDisclosed);
            Assert.Equal(0f, record.Afflictions[0].SeverityValue);
            Assert.Contains(record.Symptoms, s => s.SymptomId == RespiratoryAfflictionHandler.SymptomSevereCough);
        }

        [Fact]
        public void ConfirmedCondition_DisclosesSeverity()
        {
            var fx = new Fixture();
            fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 55f;
            fx.Pipeline.ExecuteDiagnose(fx.Sv, AfflictionId.Parse(MedicalTreatmentCatalog.RespiratoryDegenerationId));

            var record = fx.Projector.Project(fx.Sv);

            Assert.Equal("confirmed", record.Afflictions[0].DiagnosisStatus);
            Assert.True(record.Afflictions[0].SeverityDisclosed);
            Assert.Equal(55f, record.Afflictions[0].SeverityValue, 3);
        }

        [Fact]
        public void TreatmentAvailability_ReportsBlockedReason()
        {
            var fx = new Fixture();
            fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 55f;

            var record = fx.Projector.Project(fx.Sv);

            var inhaler = record.Treatments.Single(t => t.TreatmentId == MedicalTreatmentCatalog.TreatmentInhaler);
            Assert.True(inhaler.Available, inhaler.ReasonCode);

            fx.Inventory.TryConsume("inhaler", 3);
            var after = fx.Projector.Project(fx.Sv);
            var blocked = after.Treatments.Single(t => t.TreatmentId == MedicalTreatmentCatalog.TreatmentInhaler);
            Assert.False(blocked.Available);
            Assert.Equal("missing_medicine", blocked.ReasonCode);
        }
    }

    public class PatientRecordIntegrityValidatorTests
    {
        [Fact]
        public void ValidState_ProducesNoFindings()
        {
            var state = new MedicalPipelineSaveState();
            state.diagnosis.records.Add(new DiagnosisRecord
            {
                episodeId = "survivor_mara:affliction_respiratory_degeneration:0",
                status = "confirmed"
            });
            var findings = PatientRecordIntegrityValidator.Validate(state, new PatientRecordIntegrityValidator.Context());
            Assert.Empty(findings);
        }

        [Fact]
        public void UnknownSurvivor_IsFatal()
        {
            var state = new MedicalPipelineSaveState();
            state.reservations.reservations.Add(new MedicalReservation
            {
                reservationId = 1,
                survivorId = "ghost_survivor",
                kind = "medicine",
                targetId = "inhaler",
                quantity = 1,
                treatmentId = "treatment_inhaler"
            });
            var context = new PatientRecordIntegrityValidator.Context
            {
                IsKnownSurvivor = id => id == "survivor_mara"
            };
            var findings = PatientRecordIntegrityValidator.Validate(state, context);
            Assert.Contains(findings, f => f.code == "reservation_unknown_survivor" && f.fatal);
        }

        [Fact]
        public void UnknownTreatment_IsFatal()
        {
            var state = new MedicalPipelineSaveState();
            state.procedures.procedures.Add(new MedicalProcedureRow
            {
                procedureId = 1,
                survivorId = "survivor_mara",
                treatmentId = "treatment_nonexistent",
                status = "active",
                remainingHours = 10f,
                totalHours = 10f
            });
            var findings = PatientRecordIntegrityValidator.Validate(state, new PatientRecordIntegrityValidator.Context());
            Assert.Contains(findings, f => f.code == "procedure_unknown_treatment" && f.fatal);
        }
    }
}
