// SPDX-License-Identifier: MIT
// Task #133 — Respiratory vertical-slice handler.
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// Pipeline adapter over <see cref="RespiratoryDegenerationSystem"/>. The
    /// respiratory domain keeps every clinical rule (progression, thresholds,
    /// relief durations); this handler only projects its state into the shared
    /// episode/symptom/treatment contract and forwards validated treatment
    /// calls to the domain APIs.
    ///
    /// <para>Behavioral parity rule: progression is never touched here — the
    /// Phase-0 day owner keeps calling <c>Respiratory.TickHours</c> exactly as
    /// before. This handler reads and treats, it does not tick.</para>
    /// </summary>
    public sealed class RespiratoryAfflictionHandler : IAfflictionHandler
    {
        public const string SymptomOccasionalCough = "symptom_occasional_cough";
        public const string SymptomSevereCough = "symptom_severe_cough";
        public const string SymptomBreathlessness = "symptom_breathlessness";

        private readonly RespiratoryDegenerationSystem _respiratory;

        public RespiratoryAfflictionHandler(RespiratoryDegenerationSystem respiratory)
        {
            _respiratory = respiratory ?? throw new ArgumentNullException(nameof(respiratory));
        }

        public AfflictionId DefinitionId =>
            new AfflictionId(MedicalTreatmentCatalog.RespiratoryDegenerationId);

        public AfflictionEpisodeSnapshot? GetEpisode(Survivors.SurvivorId survivor)
        {
            if (survivor.IsEmpty) return null;
            float degradation = _respiratory.RespiratoryDegradation(survivor.Value);
            if (degradation <= 0f) return null;

            return new AfflictionEpisodeSnapshot
            {
                EpisodeId = AfflictionEpisodeId.Create(survivor, DefinitionId),
                DefinitionId = DefinitionId,
                Survivor = survivor,
                SeverityValue = degradation,
                StageLabel = StageLabel(degradation, _respiratory.HasPermanentLungDamage(survivor.Value)),
                IsActive = true,
                IsSymptomatic = degradation > 0f
            };
        }

        public IReadOnlyList<SymptomProjection> ProjectSymptoms(Survivors.SurvivorId survivor)
        {
            var symptoms = new List<SymptomProjection>();
            if (survivor.IsEmpty) return symptoms;
            float degradation = _respiratory.RespiratoryDegradation(survivor.Value);
            if (degradation <= 0f) return symptoms;

            var episode = AfflictionEpisodeId.Create(survivor, DefinitionId);
            if (degradation < RespiratoryDegenerationSystem.SevereCoughThreshold)
            {
                symptoms.Add(new SymptomProjection
                {
                    SymptomId = SymptomOccasionalCough,
                    SourceEpisode = episode,
                    Presentation = "Occasional cough",
                    Intensity = degradation / RespiratoryDegenerationSystem.SevereCoughThreshold
                });
            }
            else
            {
                symptoms.Add(new SymptomProjection
                {
                    SymptomId = SymptomSevereCough,
                    SourceEpisode = episode,
                    Presentation = "Severe cough",
                    Intensity = 1f
                });
                symptoms.Add(new SymptomProjection
                {
                    SymptomId = SymptomBreathlessness,
                    SourceEpisode = episode,
                    Presentation = "Breathlessness on exertion",
                    Intensity = 0.7f
                });
            }
            return symptoms;
        }

        public bool CouldHaveCondition(Survivors.SurvivorId survivor)
        {
            return !survivor.IsEmpty && _respiratory.RespiratoryDegradation(survivor.Value) > 0f;
        }

        public string? ValidateTreatment(Survivors.SurvivorId survivor, string treatmentId, string? targetItem = null)
        {
            if (treatmentId != MedicalTreatmentCatalog.TreatmentInhaler &&
                treatmentId != MedicalTreatmentCatalog.TreatmentHerbalTea &&
                treatmentId != MedicalTreatmentCatalog.TreatmentOxygenSupport)
                return "treatment_not_for_affliction";
            if (_respiratory.RespiratoryDegradation(survivor.Value) <= 0f)
                return "no_respiratory_damage";
            return null;
        }

        public bool ApplyTreatment(Survivors.SurvivorId survivor, string treatmentId, string? targetItem = null)
        {
            return treatmentId switch
            {
                MedicalTreatmentCatalog.TreatmentInhaler => _respiratory.ApplyInhaler(survivor.Value),
                MedicalTreatmentCatalog.TreatmentOxygenSupport => _respiratory.ApplyInhaler(survivor.Value),
                MedicalTreatmentCatalog.TreatmentHerbalTea => _respiratory.ApplyHerbalTea(survivor.Value),
                _ => false
            };
        }

        public bool HasResolved(Survivors.SurvivorId survivor)
        {
            return _respiratory.RespiratoryDegradation(survivor.Value) <= 0f;
        }

        /// <summary>Stage label mirroring the existing UI projection (MedicalPanel).</summary>
        public static string StageLabel(float degradation, bool permanent)
        {
            if (degradation <= 0f) return "CLEAR";
            if (degradation < RespiratoryDegenerationSystem.SevereCoughThreshold)
                return "MILD COUGH";
            if (degradation < RespiratoryDegenerationSystem.IrreversibleThreshold)
                return "SEVERE COUGH";
            if (degradation < RespiratoryDegenerationSystem.TerminalLungThreshold)
                return permanent ? "PERMANENT LUNG DAMAGE" : "CRITICAL";
            return "TERMINAL LUNG DAMAGE";
        }
    }
}
