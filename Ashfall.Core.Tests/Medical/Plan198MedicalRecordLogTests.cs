// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Plan198MedicalRecord
{
    /// <summary>
    /// DEBT-198-PIPELINE-EVENT-LOG — bounded append-only medical record.
    /// Contract: ids/day/kind only (never free text), bounded retention,
    /// survives the pipeline save round-trip, and old saves load empty.
    /// </summary>
    public sealed class Plan198MedicalRecordLogTests
    {
        [Fact]
        public void Append_records_ids_and_day_only()
        {
            var log = new MedicalRecordLog();
            log.Append(day: 12, MedicalRecordKinds.DiagnosisConfirmed, "sv_alpha", "disease_rad_fever");

            var entry = Assert.Single(log.Entries);
            Assert.Equal(12, entry.day);
            Assert.Equal("diagnosis_confirmed", entry.kind);
            Assert.Equal("sv_alpha", entry.survivorId);
            Assert.Equal("disease_rad_fever", entry.detail);
        }

        [Fact]
        public void Blank_kinds_are_rejected()
        {
            var log = new MedicalRecordLog();
            log.Append(1, string.Empty, "sv_alpha", "x");
            log.Append(1, "", "sv_beta");
            Assert.Equal(0, log.Count);
        }

        [Fact]
        public void Log_is_bounded_and_evicts_oldest_first()
        {
            var log = new MedicalRecordLog();
            for (int day = 1; day <= MedicalRecordLog.MaxEntries + 10; day++)
                log.Append(day, MedicalRecordKinds.TreatmentCompleted, "sv_alpha", $"treatment_{day}");

            Assert.Equal(MedicalRecordLog.MaxEntries, log.Count);
            // Oldest evicted: first retained entry is day 11.
            Assert.Equal(11, log.Entries[0].day);
            Assert.Equal(MedicalRecordLog.MaxEntries + 10, log.Entries[^1].day);
        }

        [Fact]
        public void ForSurvivor_filters_newest_first_and_is_bounded()
        {
            var log = new MedicalRecordLog();
            log.Append(1, MedicalRecordKinds.DiagnosisSuspected, "sv_alpha", "a");
            log.Append(2, MedicalRecordKinds.DiagnosisSuspected, "sv_bravo", "b");
            log.Append(3, MedicalRecordKinds.DiagnosisConfirmed, "sv_alpha", "c");

            var alpha = log.ForSurvivor("sv_alpha", max: 1);
            Assert.Single(alpha);
            Assert.Equal(3, alpha[0].day);

            Assert.Equal(2, log.ForSurvivor("sv_alpha", max: 5).Count);
            Assert.Empty(log.ForSurvivor("", 5));
            Assert.Empty(log.ForSurvivor("sv_alpha", 0));
        }

        [Fact]
        public void Capture_restore_round_trips_and_defends_the_bound()
        {
            var log = new MedicalRecordLog();
            log.Append(5, MedicalRecordKinds.PatientRecovered, "sv_alpha", "disease_rad_fever");
            var captured = log.CaptureState();

            var restored = new MedicalRecordLog();
            restored.RestoreState(captured);
            Assert.Single(restored.Entries);
            Assert.Equal(MedicalRecordKinds.PatientRecovered, restored.Entries[0].kind);

            // An edited/oversized save cannot exceed the bound.
            var oversized = new List<MedicalRecordEntry>();
            for (int i = 0; i < MedicalRecordLog.MaxEntries + 25; i++)
                oversized.Add(new MedicalRecordEntry { day = i, kind = MedicalRecordKinds.TreatmentCompleted, survivorId = "sv_x" });
            var bounded = new MedicalRecordLog();
            bounded.RestoreState(oversized);
            Assert.Equal(MedicalRecordLog.MaxEntries, bounded.Count);
        }

        [Fact]
        public void Old_save_without_record_field_loads_empty()
        {
            // Backward compatibility: pre-Plan-198 saves have no record list.
            var log = new MedicalRecordLog();
            log.Append(1, MedicalRecordKinds.TreatmentScheduled, "sv_alpha", "t");
            log.RestoreState(null);
            Assert.Equal(0, log.Count);

            var save = new MedicalPipelineSaveState();
            Assert.NotNull(save.record);
            Assert.Empty(save.record);
        }

        [Fact]
        public void Record_never_exposes_a_free_text_note_field()
        {
            // Retention/privacy rule: the persisted DTO stores ids and a day only.
            Assert.Null(typeof(MedicalRecordEntry).GetField("note"));
            Assert.Null(typeof(MedicalRecordEntry).GetField("notes"));
            Assert.Null(typeof(MedicalRecordEntry).GetField("text"));
            Assert.Equal(
                new[] { "day", "detail", "kind", "survivorId" },
                typeof(MedicalRecordEntry).GetFields().Select(f => f.Name).OrderBy(n => n).ToArray());
        }

        [Fact]
        public void Coordinator_records_its_own_emitted_events_and_persists_them()
        {
            // The log mirrors the events the coordinator already emits — one
            // subscription, so it cannot drift from the emit sites.
            int day = 4;
            var respiratory = new RespiratoryDegenerationSystem();
            respiratory.GetOrCreate("sv_alpha").respiratoryDegradation = 20f;
            var coordinator = CreateCoordinator(() => day, respiratory);

            var result = coordinator.ExecuteDiagnose(
                Ashfall.Core.Survivors.SurvivorId.Parse("sv_alpha"),
                AfflictionId.Parse(MedicalTreatmentCatalog.RespiratoryDegenerationId));
            Assert.True(result.Success, result.ReasonCode);

            var entry = Assert.Single(coordinator.Record.Entries);
            Assert.Equal(MedicalRecordKinds.DiagnosisConfirmed, entry.kind);
            Assert.Equal("sv_alpha", entry.survivorId);
            Assert.Equal(4, entry.day);

            // Persists with the pipeline section (no new save section).
            var saved = coordinator.CaptureState();
            Assert.Single(saved.record);

            var restored = CreateCoordinator(() => day, new RespiratoryDegenerationSystem());
            restored.RestoreState(saved);
            Assert.Single(restored.Record.Entries);
            Assert.Equal(MedicalRecordKinds.DiagnosisConfirmed, restored.Record.Entries[0].kind);
        }

        private static MedicalPipelineCoordinator CreateCoordinator(System.Func<int> day, RespiratoryDegenerationSystem respiratory)
        {
            var coordinator = new MedicalPipelineCoordinator(
                new Ashfall.Core.Inventory.Inventory { Capacity = 20, MaxWeight = 100f },
                new DiagnosisKnowledgeStore(),
                new MedicalReservationLedger(),
                new MedicalProcedureSchedule(),
                _ => PatientAvailability.Ok(),
                day);
            coordinator.RegisterHandler(new RespiratoryAfflictionHandler(respiratory));
            return coordinator;
        }
    }
}
