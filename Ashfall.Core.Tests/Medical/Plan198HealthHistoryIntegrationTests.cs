// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 198: Health History & Medical Records System — Integration Tests
// Verifies medical record template catalog loading, medical record logging,
// resolution/recovery events, vaccination immunity tracking and booster alerts,
// health trends tracking, and save/restore persistence.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Medical;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class Plan198HealthHistoryIntegrationTests : CatalogTestBase
    {
        [Fact]
        public void LoadCatalog_LoadsAllSevenTemplates()
        {
            var system = new HealthHistorySystem();
            string path = Path.Combine(DataDirectory, "medical_record_templates.json");
            Assert.True(File.Exists(path), $"medical_record_templates.json must exist at {path}");

            system.LoadCatalog(File.ReadAllText(path));

            var templates = system.GetAllTemplates();
            Assert.Equal(7, templates.Count);

            var illness = system.GetTemplate("template_illness");
            Assert.NotNull(illness);
            Assert.Equal("illness", illness.record_type);
            Assert.Equal("moderate", illness.severity_default);
            Assert.Equal(7, illness.typical_duration_days);

            var radiation = system.GetTemplate("template_radiation");
            Assert.NotNull(radiation);
            Assert.Equal("radiation_exposure", radiation.record_type);
            Assert.Equal("critical", radiation.severity_default);
        }

        [Fact]
        public void LogRecord_CreatesRecordAndAutoEmitsHealthEvent()
        {
            var system = new HealthHistorySystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "medical_record_templates.json")));

            MedicalRecord? addedRecord = null;
            system.OnRecordAdded += r => addedRecord = r;

            var record = system.LogRecord(
                survivorId: "surv_01",
                recordType: "illness",
                description: "Severe Ash Pneumonitis",
                severity: "severe",
                day: 4,
                medicId: "surv_doc",
                notes: "Exposed to ash storm without respirator");

            Assert.NotNull(record);
            Assert.NotNull(addedRecord);
            Assert.Equal(1, system.TotalRecordsCount);
            Assert.Equal("ongoing", record.Outcome);
            Assert.Equal("surv_doc", record.TreatingMedicId);

            var records = system.GetSurvivorRecords("surv_01");
            Assert.Single(records);
            Assert.Equal("illness", records[0].RecordType);

            var events = system.GetSurvivorEvents("surv_01");
            Assert.Single(events);
            Assert.Equal("diagnosis", events[0].EventType);
        }

        [Fact]
        public void ResolveRecord_UpdatesOutcomeAndLogsRecoveryEvent()
        {
            var system = new HealthHistorySystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "medical_record_templates.json")));

            var record = system.LogRecord(
                survivorId: "surv_02",
                recordType: "injury",
                description: "Deep shrapnel laceration",
                severity: "severe",
                day: 2);

            bool resolvedFired = false;
            system.OnRecordResolved += r => resolvedFired = true;

            bool resolved = system.ResolveRecord(record.RecordId, "resolved", day: 9, "Wound closed cleanly without infection");
            Assert.True(resolved);
            Assert.True(resolvedFired);
            Assert.Equal("resolved", record.Outcome);

            var events = system.GetSurvivorEvents("surv_02");
            Assert.Equal(2, events.Count); // diagnosis + recovery
            Assert.Contains(events, e => e.EventType == "recovery");
        }

        [Fact]
        public void AdministerVaccine_TracksImmunityAndTriggersBoosterAlertOnTick()
        {
            var system = new HealthHistorySystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "medical_record_templates.json")));

            VaccinationRecord? vacAdministered = null;
            system.OnVaccinationAdministered += v => vacAdministered = v;

            var vac = system.AdministerVaccine(
                survivorId: "surv_03",
                vaccineType: "ash_fever_toxoid",
                day: 1,
                medicId: "surv_doc",
                initialImmunity: 100.0f,
                durationDays: 30);

            Assert.NotNull(vac);
            Assert.NotNull(vacAdministered);
            Assert.Equal(31, vac.BoosterDueDay);
            Assert.Equal(100.0f, system.GetVaccinationImmunity("surv_03", "ash_fever_toxoid"));

            VaccinationRecord? boosterAlert = null;
            system.OnBoosterDueAlert += v => boosterAlert = v;

            // Tick to day 32 (past booster due day)
            system.TickDay(32);

            Assert.NotNull(boosterAlert);
            Assert.Equal("ash_fever_toxoid", boosterAlert.VaccineType);
            Assert.True(vac.ImmunityLevel < 100.0f);
        }

        [Fact]
        public void RecordDailyHealthTrend_CalculatesDirectionAndTracksMetrics()
        {
            var system = new HealthHistorySystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "medical_record_templates.json")));

            // Day 1
            system.RecordDailyHealthTrend("surv_04", day: 1, overallHealth: 70.0f, radiationDose: 15.0f, immuneStrength: 80.0f, chronicCount: 0);

            // Day 2 (overall health improved, radiation dose reduced)
            system.RecordDailyHealthTrend("surv_04", day: 2, overallHealth: 85.0f, radiationDose: 5.0f, immuneStrength: 80.0f, chronicCount: 0);

            var trends = system.GetLatestTrends("surv_04");
            Assert.Equal(4, trends.Count);

            var healthTrend = trends.First(t => t.HealthMetric == "overall_health");
            Assert.Equal(85.0f, healthTrend.Value);
            Assert.Equal("improving", healthTrend.Trend);

            var radTrend = trends.First(t => t.HealthMetric == "radiation_dose");
            Assert.Equal(5.0f, radTrend.Value);
            Assert.Equal("declining", radTrend.Trend);
        }

        [Fact]
        public void SaveRestoreState_PreservesAllMedicalRecordsAndHistory()
        {
            var system = new HealthHistorySystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "medical_record_templates.json")));

            system.LogRecord("surv_05", "checkup", "Annual vitals exam", "mild", day: 1);
            system.AdministerVaccine("surv_05", "pneumo_shield", day: 1);
            system.RecordDailyHealthTrend("surv_05", day: 1, overallHealth: 95.0f, radiationDose: 0.0f, immuneStrength: 90.0f, chronicCount: 0);

            var captured = system.CaptureState();
            Assert.NotNull(captured);
            Assert.Equal(2, captured.Records.Count); // checkup + auto-record from vaccine
            Assert.Single(captured.Vaccinations);
            Assert.Equal(4, captured.Trends.Count);

            var restoredSystem = new HealthHistorySystem();
            restoredSystem.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "medical_record_templates.json")));
            restoredSystem.RestoreState(captured);

            Assert.Equal(captured.Records.Count, restoredSystem.TotalRecordsCount);
            Assert.Equal(1, restoredSystem.TotalVaccinationsCount);
            Assert.Equal(4, restoredSystem.TotalTrendsCount);

            var records = restoredSystem.GetSurvivorRecords("surv_05");
            Assert.Equal(2, records.Count);
            Assert.Equal(100.0f, restoredSystem.GetVaccinationImmunity("surv_05", "pneumo_shield"));
        }
    }
}
