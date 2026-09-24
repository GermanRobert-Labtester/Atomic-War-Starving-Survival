// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 198 — Health History & Medical Records System
// Pure domain authority for persistent survivor medical histories, clinical
// logs, diagnostic events, vaccination tracking, and longitudinal health trends.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Medical
{
    // ── Catalog DTOs ────────────────────────────────────────────────────────

    [Serializable]
    public sealed class MedicalRecordTemplateDef
    {
        public string template_id { get; set; } = string.Empty;
        public string record_type { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string severity_default { get; set; } = "mild";
        public int typical_duration_days { get; set; } = 7;
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class MedicalRecordTemplatesCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<MedicalRecordTemplateDef> templates { get; set; } = new List<MedicalRecordTemplateDef>();
    }

    // ── State DTOs ──────────────────────────────────────────────────────────

    [Serializable]
    public sealed class MedicalRecord
    {
        public string RecordId { get; set; } = string.Empty;
        public string SurvivorId { get; set; } = string.Empty;
        public string RecordType { get; set; } = "illness"; // illness, injury, treatment, vaccination, radiation_exposure, chronic_condition, checkup
        public int RecordedDay { get; set; } = 1;
        public string Description { get; set; } = string.Empty;
        public string Severity { get; set; } = "mild";       // mild, moderate, severe, critical
        public int DurationDays { get; set; } = 0;
        public string Outcome { get; set; } = "ongoing";     // ongoing, resolved, chronic, fatal
        public List<string> TreatmentApplied { get; set; } = new List<string>();
        public string TreatingMedicId { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class HealthEvent
    {
        public string EventId { get; set; } = string.Empty;
        public string SurvivorId { get; set; } = string.Empty;
        public string EventType { get; set; } = "diagnosis"; // diagnosis, treatment, recovery, relapse, complication, vaccination, checkup
        public int EventDay { get; set; } = 1;
        public string Description { get; set; } = string.Empty;
        public string RelatedCondition { get; set; } = string.Empty;
        public string Outcome { get; set; } = "success";     // success, partial, failure
        public string Notes { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class VaccinationRecord
    {
        public string VaccinationId { get; set; } = string.Empty;
        public string SurvivorId { get; set; } = string.Empty;
        public string VaccineType { get; set; } = string.Empty;
        public int AdministeredDay { get; set; } = 1;
        public string AdministeredByMedicId { get; set; } = string.Empty;
        public float ImmunityLevel { get; set; } = 100.0f; // 0 to 100
        public int ImmunityDurationDays { get; set; } = 60;
        public int BoosterDueDay { get; set; } = 61;
        public bool BoosterAlertFired { get; set; } = false;
    }

    [Serializable]
    public sealed class HealthTrend
    {
        public string TrendId { get; set; } = string.Empty;
        public string SurvivorId { get; set; } = string.Empty;
        public string HealthMetric { get; set; } = "overall_health"; // overall_health, radiation_dose, immune_strength, chronic_condition_count
        public int MeasurementDay { get; set; } = 1;
        public float Value { get; set; } = 100.0f;
        public string Trend { get; set; } = "stable"; // improving, stable, declining
    }

    [Serializable]
    public sealed class HealthHistoryState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<MedicalRecord> Records { get; set; } = new List<MedicalRecord>();
        public List<HealthEvent> Events { get; set; } = new List<HealthEvent>();
        public List<VaccinationRecord> Vaccinations { get; set; } = new List<VaccinationRecord>();
        public List<HealthTrend> Trends { get; set; } = new List<HealthTrend>();
        public bool AutoRecordTreatments { get; set; } = true;
        public bool ShowTrends { get; set; } = true;
    }

    // ── Domain System ───────────────────────────────────────────────────────

    public sealed class HealthHistorySystem
    {
        private HealthHistoryState _state;
        private readonly Dictionary<string, MedicalRecordTemplateDef> _templateDefs =
            new Dictionary<string, MedicalRecordTemplateDef>(StringComparer.OrdinalIgnoreCase);

        public event Action<MedicalRecord>? OnRecordAdded;
        public event Action<MedicalRecord>? OnRecordResolved;
        public event Action<HealthEvent>? OnHealthEventLogged;
        public event Action<VaccinationRecord>? OnVaccinationAdministered;
        public event Action<VaccinationRecord>? OnBoosterDueAlert;
        public event Action<HealthTrend>? OnTrendRecorded;

        public int TotalRecordsCount => _state.Records.Count;
        public int TotalEventsCount => _state.Events.Count;
        public int TotalVaccinationsCount => _state.Vaccinations.Count;
        public int TotalTrendsCount => _state.Trends.Count;

        public HealthHistorySystem()
        {
            _state = new HealthHistoryState();
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("Catalog JSON cannot be null or empty", nameof(json));

            var catalog = JsonSerializer.Deserialize<MedicalRecordTemplatesCatalog>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (catalog?.templates == null) return;

            _templateDefs.Clear();
            foreach (var template in catalog.templates)
            {
                if (!string.IsNullOrEmpty(template.template_id))
                {
                    _templateDefs[template.template_id] = template;
                }
            }
        }

        public IReadOnlyList<MedicalRecordTemplateDef> GetAllTemplates() => _templateDefs.Values.ToList();

        public MedicalRecordTemplateDef? GetTemplate(string templateId)
        {
            _templateDefs.TryGetValue(templateId, out var template);
            return template;
        }

        public MedicalRecord LogRecord(
            string survivorId,
            string recordType,
            string description,
            string severity,
            int day,
            string medicId = "",
            string outcome = "ongoing",
            int durationDays = 0,
            List<string>? treatments = null,
            string notes = "")
        {
            var record = new MedicalRecord
            {
                RecordId = $"medrec_{_state.NextSequence++}",
                SurvivorId = survivorId,
                RecordType = recordType,
                RecordedDay = day,
                Description = description,
                Severity = severity,
                DurationDays = durationDays,
                Outcome = outcome,
                TreatmentApplied = treatments != null ? new List<string>(treatments) : new List<string>(),
                TreatingMedicId = medicId,
                Notes = notes
            };

            _state.Records.Add(record);
            OnRecordAdded?.Invoke(record);

            // If auto-recording events
            if (_state.AutoRecordTreatments)
            {
                LogHealthEvent(
                    survivorId: survivorId,
                    eventType: recordType == "treatment" ? "treatment" : "diagnosis",
                    description: description,
                    day: day,
                    relatedCondition: recordType,
                    outcome: "success",
                    notes: notes);
            }

            return record;
        }

        public bool ResolveRecord(string recordId, string outcome, int day, string notes = "")
        {
            var record = _state.Records.FirstOrDefault(r => string.Equals(r.RecordId, recordId, StringComparison.OrdinalIgnoreCase));
            if (record == null) return false;

            record.Outcome = outcome;
            if (!string.IsNullOrEmpty(notes))
            {
                record.Notes = string.IsNullOrEmpty(record.Notes) ? notes : $"{record.Notes}; {notes}";
            }

            OnRecordResolved?.Invoke(record);

            LogHealthEvent(
                survivorId: record.SurvivorId,
                eventType: outcome == "resolved" ? "recovery" : "complication",
                description: $"Condition '{record.Description}' marked as {outcome}.",
                day: day,
                relatedCondition: record.RecordType,
                outcome: outcome == "resolved" ? "success" : "partial",
                notes: notes);

            return true;
        }

        public HealthEvent LogHealthEvent(
            string survivorId,
            string eventType,
            string description,
            int day,
            string relatedCondition = "",
            string outcome = "success",
            string notes = "")
        {
            var evt = new HealthEvent
            {
                EventId = $"hevt_{_state.NextSequence++}",
                SurvivorId = survivorId,
                EventType = eventType,
                EventDay = day,
                Description = description,
                RelatedCondition = relatedCondition,
                Outcome = outcome,
                Notes = notes
            };

            _state.Events.Add(evt);
            OnHealthEventLogged?.Invoke(evt);
            return evt;
        }

        public VaccinationRecord AdministerVaccine(
            string survivorId,
            string vaccineType,
            int day,
            string medicId = "",
            float initialImmunity = 100.0f,
            int durationDays = 60)
        {
            var vac = new VaccinationRecord
            {
                VaccinationId = $"vac_{_state.NextSequence++}",
                SurvivorId = survivorId,
                VaccineType = vaccineType,
                AdministeredDay = day,
                AdministeredByMedicId = medicId,
                ImmunityLevel = Math.Clamp(initialImmunity, 0.0f, 100.0f),
                ImmunityDurationDays = durationDays,
                BoosterDueDay = day + durationDays,
                BoosterAlertFired = false
            };

            _state.Vaccinations.Add(vac);
            OnVaccinationAdministered?.Invoke(vac);

            LogRecord(
                survivorId: survivorId,
                recordType: "vaccination",
                description: $"Inoculated with {vaccineType}.",
                severity: "mild",
                day: day,
                medicId: medicId,
                outcome: "resolved",
                durationDays: durationDays,
                treatments: new List<string> { vaccineType },
                notes: $"Booster due on day {vac.BoosterDueDay}.");

            return vac;
        }

        public List<HealthTrend> RecordDailyHealthTrend(
            string survivorId,
            int day,
            float overallHealth,
            float radiationDose,
            float immuneStrength,
            int chronicCount)
        {
            var createdTrends = new List<HealthTrend>();

            void AddTrend(string metric, float value)
            {
                var prevTrend = _state.Trends
                    .Where(t => string.Equals(t.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase) && string.Equals(t.HealthMetric, metric, StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(t => t.MeasurementDay)
                    .FirstOrDefault();

                string trendDirection = "stable";
                if (prevTrend != null)
                {
                    if (value > prevTrend.Value + 0.5f) trendDirection = "improving";
                    else if (value < prevTrend.Value - 0.5f) trendDirection = "declining";
                }

                var trend = new HealthTrend
                {
                    TrendId = $"trend_{_state.NextSequence++}",
                    SurvivorId = survivorId,
                    HealthMetric = metric,
                    MeasurementDay = day,
                    Value = value,
                    Trend = trendDirection
                };

                _state.Trends.Add(trend);
                createdTrends.Add(trend);
                OnTrendRecorded?.Invoke(trend);
            }

            AddTrend("overall_health", overallHealth);
            AddTrend("radiation_dose", radiationDose);
            AddTrend("immune_strength", immuneStrength);
            AddTrend("chronic_condition_count", chronicCount);

            return createdTrends;
        }

        public void TickDay(int day)
        {
            // Decay active vaccinations and trigger booster alert when due
            foreach (var vac in _state.Vaccinations)
            {
                if (day >= vac.BoosterDueDay)
                {
                    vac.ImmunityLevel = Math.Max(10.0f, vac.ImmunityLevel - 2.0f);
                    if (!vac.BoosterAlertFired)
                    {
                        vac.BoosterAlertFired = true;
                        OnBoosterDueAlert?.Invoke(vac);
                    }
                }
                else
                {
                    // Linear immunity degradation towards 30% baseline at expiration
                    float elapsedDays = Math.Max(0, day - vac.AdministeredDay);
                    float fraction = Math.Clamp(elapsedDays / vac.ImmunityDurationDays, 0.0f, 1.0f);
                    vac.ImmunityLevel = Math.Max(30.0f, 100.0f - (70.0f * fraction));
                }
            }
        }

        public IReadOnlyList<MedicalRecord> GetSurvivorRecords(string survivorId) =>
            _state.Records.Where(r => string.Equals(r.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase)).ToList();

        public IReadOnlyList<HealthEvent> GetSurvivorEvents(string survivorId) =>
            _state.Events.Where(e => string.Equals(e.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase)).ToList();

        public IReadOnlyList<VaccinationRecord> GetVaccinations(string survivorId) =>
            _state.Vaccinations.Where(v => string.Equals(v.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase)).ToList();

        public float GetVaccinationImmunity(string survivorId, string vaccineType)
        {
            var vac = _state.Vaccinations
                .Where(v => string.Equals(v.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase) &&
                            string.Equals(v.VaccineType, vaccineType, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(v => v.AdministeredDay)
                .FirstOrDefault();

            return vac?.ImmunityLevel ?? 0.0f;
        }

        public IReadOnlyList<HealthTrend> GetLatestTrends(string survivorId)
        {
            var metrics = new[] { "overall_health", "radiation_dose", "immune_strength", "chronic_condition_count" };
            var list = new List<HealthTrend>();

            foreach (var m in metrics)
            {
                var latest = _state.Trends
                    .Where(t => string.Equals(t.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase) && string.Equals(t.HealthMetric, m, StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(t => t.MeasurementDay)
                    .FirstOrDefault();

                if (latest != null) list.Add(latest);
            }

            return list;
        }

        public HealthHistoryState CaptureState() => _state;

        public void RestoreState(HealthHistoryState? state)
        {
            _state = state ?? new HealthHistoryState();
        }

        public HealthHistoryCensus GetCensus()
        {
            int chronic = 0;
            for (int i = 0; i < _state.Records.Count; i++)
            {
                if (string.Equals(_state.Records[i].Outcome, "chronic", StringComparison.OrdinalIgnoreCase))
                    chronic++;
            }
            return new HealthHistoryCensus(_state.Records.Count, _state.Events.Count, _state.Vaccinations.Count, _state.Trends.Count, chronic);
        }
    }

    public struct HealthHistoryCensus
    {
        public readonly int TotalRecords;
        public readonly int TotalEvents;
        public readonly int TotalVaccinations;
        public readonly int TotalTrends;
        public readonly int ActiveChronicConditions;

        public HealthHistoryCensus(int records, int events, int vaccinations, int trends, int chronic)
        {
            TotalRecords = records;
            TotalEvents = events;
            TotalVaccinations = vaccinations;
            TotalTrends = trends;
            ActiveChronicConditions = chronic;
        }
    }
}
