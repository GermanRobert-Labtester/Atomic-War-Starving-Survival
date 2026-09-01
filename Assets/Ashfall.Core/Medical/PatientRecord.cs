// SPDX-License-Identifier: MIT
// Task #133 — Read-only patient-record projection over domain-owned state.
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Medical
{
    /// <summary>One underlying condition row as the record shows it.</summary>
    [Serializable]
    public sealed class PatientAfflictionView
    {
        public string EpisodeId = string.Empty;
        public string AfflictionId = string.Empty;
        public string StageLabel = string.Empty;
        public bool IsActive;
        /// <summary>Diagnosis knowledge: "unknown" | "suspected" | "confirmed" | "ruled_out".</summary>
        public string DiagnosisStatus = "unknown";
        /// <summary>Severity is only disclosed once the diagnosis is confirmed (Phase 7: no duplicate UI truth, no free knowledge).</summary>
        public bool SeverityDisclosed;
        public float SeverityValue;
    }

    /// <summary>One observable symptom row.</summary>
    [Serializable]
    public sealed class PatientSymptomView
    {
        public string SymptomId = string.Empty;
        public string SourceEpisodeId = string.Empty;
        public string Presentation = string.Empty;
        public float Intensity;
    }

    /// <summary>Availability of one treatment with a stable blocked reason.</summary>
    [Serializable]
    public sealed class PatientTreatmentView
    {
        public string TreatmentId = string.Empty;
        public string DisplayName = string.Empty;
        public string AfflictionId = string.Empty;
        public bool Available;
        /// <summary>Stable snake_case reason code when blocked ("ok" when available).</summary>
        public string ReasonCode = "ok";
        public Dictionary<string, int> ItemCosts = new Dictionary<string, int>();
    }

    /// <summary>One in-flight procedure row (read-only view).</summary>
    [Serializable]
    public sealed class PatientProcedureView
    {
        public int ProcedureId;
        public string TreatmentId = string.Empty;
        public float RemainingHours;
        public float TotalHours;
        public int StartDay;
    }

    /// <summary>
    /// The coherent medical view of one survivor. This is a <b>projection</b>:
    /// every field traces to an authoritative domain store, nothing here is
    /// persisted, and nothing here is mutable gameplay state. UI consumes this
    /// record; it must not reconstruct medical truth from raw ledgers.
    /// </summary>
    [Serializable]
    public sealed class PatientRecord
    {
        public string SurvivorId = string.Empty;
        public bool Available;
        public string AvailabilityReason = "ok";

        /// <summary>Underlying conditions the domain currently reports (masked by diagnosis knowledge).</summary>
        public List<PatientAfflictionView> Afflictions = new List<PatientAfflictionView>();
        public List<PatientSymptomView> Symptoms = new List<PatientSymptomView>();

        /// <summary>Treatment availability computed from live preview — never stored.</summary>
        public List<PatientTreatmentView> Treatments = new List<PatientTreatmentView>();

        public List<PatientProcedureView> ScheduledProcedures = new List<PatientProcedureView>();

        /// <summary>Derived summary; not an independent mutable truth (Phase 40).</summary>
        public string Prognosis = string.Empty;
    }

    /// <summary>
    /// Builds <see cref="PatientRecord"/> projections from the coordinator plus
    /// domain handlers. Deterministic: handlers are iterated in registration
    /// order sorted by definition id; symptoms sorted by id.
    /// </summary>
    public sealed class PatientRecordProjector
    {
        private readonly MedicalPipelineCoordinator _pipeline;

        public PatientRecordProjector(MedicalPipelineCoordinator pipeline)
        {
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
        }

        public PatientRecord Project(Survivors.SurvivorId survivor)
        {
            var record = new PatientRecord { SurvivorId = survivor.Value };
            if (survivor.IsEmpty) return record;

            var availability = _pipeline.AvailabilityOf(survivor);
            record.Available = availability.Available;
            record.AvailabilityReason = availability.Available ? "ok" : availability.ReasonCode;

            var definitions = new List<AfflictionId>();
            foreach (var handler in _pipeline.Handlers)
                definitions.Add(handler.DefinitionId);
            definitions.Sort(static (a, b) => string.CompareOrdinal(a.Value, b.Value));

            var symptoms = new List<SymptomProjection>();
            bool anyActive = false;

            foreach (var definition in definitions)
            {
                var handler = _pipeline.GetHandler(definition);
                if (handler == null) continue;
                var episode = handler.GetEpisode(survivor);
                if (episode == null) continue;

                var status = _pipeline.Diagnosis.GetStatus(episode.EpisodeId);

                // Task #133 P1 — disease identities stay hidden until an
                // explicit identify confirms them. Unknown disease episodes
                // surface no affliction row at all (unnamed symptoms below
                // still hint at the vector); suspected ones surface a masked
                // row that names nothing — not even the episode id, which
                // would leak the disease id through its text.
                bool diseaseFamily = definition.Value.StartsWith("disease_", StringComparison.Ordinal);
                if (diseaseFamily && status != DiagnosisStatus.Confirmed)
                {
                    if (status == DiagnosisStatus.Suspected)
                    {
                        record.Afflictions.Add(new PatientAfflictionView
                        {
                            EpisodeId = string.Empty,
                            AfflictionId = MedicalTreatmentCatalog.UnidentifiedIllnessId,
                            StageLabel = "Unidentified illness",
                            IsActive = true,
                            DiagnosisStatus = DiagnosisKnowledgeStore.StatusToString(status),
                            SeverityDisclosed = false,
                            SeverityValue = 0f
                        });
                    }
                }
                else
                {
                    record.Afflictions.Add(new PatientAfflictionView
                    {
                        EpisodeId = episode.EpisodeId.Value,
                        AfflictionId = definition.Value,
                        StageLabel = episode.StageLabel,
                        IsActive = episode.IsActive,
                        DiagnosisStatus = DiagnosisKnowledgeStore.StatusToString(status),
                        SeverityDisclosed = status == DiagnosisStatus.Confirmed,
                        SeverityValue = status == DiagnosisStatus.Confirmed ? episode.SeverityValue : 0f
                    });
                }

                if (episode.IsActive) anyActive = true;
                var handlerSymptoms = handler.ProjectSymptoms(survivor);
                for (int i = 0; i < handlerSymptoms.Count; i++)
                {
                    var s = handlerSymptoms[i];
                    if (!s.Observable) continue;
                    symptoms.Add(s);
                }
            }

            symptoms.Sort(static (a, b) => string.CompareOrdinal(a.SymptomId, b.SymptomId));
            foreach (var s in symptoms)
            {
                record.Symptoms.Add(new PatientSymptomView
                {
                    SymptomId = s.SymptomId,
                    SourceEpisodeId = s.SourceEpisode.Value,
                    Presentation = s.Presentation,
                    Intensity = s.Intensity
                });
            }

            // Treatment availability: computed live through the side-effect-free preview.
            foreach (var def in MedicalTreatmentCatalog.All)
            {
                var preview = _pipeline.PreviewTreatment(survivor, def.TreatmentId);
                record.Treatments.Add(new PatientTreatmentView
                {
                    TreatmentId = def.TreatmentId,
                    DisplayName = def.DisplayName,
                    AfflictionId = def.AfflictionId,
                    Available = preview.IsAvailable,
                    ReasonCode = preview.IsAvailable ? "ok" : preview.FailureCode,
                    ItemCosts = new Dictionary<string, int>(def.ItemCosts)
                });
            }

            foreach (var row in _pipeline.Schedule.Active)
            {
                if (!string.Equals(row.survivorId, survivor.Value, StringComparison.Ordinal)) continue;
                record.ScheduledProcedures.Add(new PatientProcedureView
                {
                    ProcedureId = row.procedureId,
                    TreatmentId = row.treatmentId,
                    RemainingHours = row.remainingHours,
                    TotalHours = row.totalHours,
                    StartDay = row.startDay
                });
            }

            record.Prognosis = DerivePrognosis(record, anyActive);
            return record;
        }

        private static string DerivePrognosis(PatientRecord record, bool anyActive)
        {
            if (!record.Available)
                return record.AvailabilityReason == "patient_dead" || record.AvailabilityReason == "patient_memorialized"
                    ? "deceased"
                    : "unavailable";
            if (!anyActive) return "stable";
            foreach (var a in record.Afflictions)
            {
                if (!a.IsActive) continue;
                if (string.Equals(a.StageLabel, "TERMINAL LUNG DAMAGE", StringComparison.Ordinal))
                    return "terminal";
                if (string.Equals(a.StageLabel, "ManifestIllness", StringComparison.Ordinal))
                    return "critical";
            }
            return "guarded";
        }
    }
}
