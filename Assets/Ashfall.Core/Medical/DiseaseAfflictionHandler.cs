// SPDX-License-Identifier: MIT
// Task #133 P1 — Disease write-path handler: quarantine/release via the pipeline.
using System;
using System.Collections.Generic;
using Ashfall.Core.Disease;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// Pipeline adapter over <see cref="DiseaseSystem"/> for one authored
    /// disease. The disease domain keeps every clinical rule (spread, incubation,
    /// lethality, outbreak, quarantine semantics); this handler only projects
    /// infection state into the shared episode/symptom contract and forwards
    /// validated quarantine/release calls.
    ///
    /// <para>Behavioral parity rule: progression is never touched here — the
    /// medical_disease day owner keeps calling <c>DiseaseSystem.TickDaily</c>
    /// exactly as before. This handler reads and isolates, it does not tick.</para>
    ///
    /// <para>Identity: the affliction definition id IS the catalog disease id
    /// (<c>disease_cholera</c>…). One infection per (survivor, disease) exists in
    /// the domain, so the episode ordinal is always 0.</para>
    /// </summary>
    public sealed class DiseaseAfflictionHandler : IAfflictionHandler
    {
        // Unnamed symptom ids — presentations never leak the disease name
        // before an explicit identify (Task #133 P1 hidden-knowledge rule).
        public const string SymptomGastrointestinalDistress = "symptom_gastrointestinal_distress";
        public const string SymptomFever = "symptom_fever";
        public const string SymptomPersistentCough = "symptom_persistent_cough";
        public const string SymptomFatigue = "symptom_fatigue";
        public const string SymptomBreathlessness = "symptom_breathlessness";

        private readonly DiseaseSystem _disease;
        private readonly DiseaseDefinition _definition;

        public DiseaseAfflictionHandler(DiseaseSystem disease, DiseaseDefinition definition)
        {
            _disease = disease ?? throw new ArgumentNullException(nameof(disease));
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            if (string.IsNullOrEmpty(_definition.id))
                throw new ArgumentException("Disease definition id must not be empty.", nameof(definition));
        }

        public AfflictionId DefinitionId => new AfflictionId(_definition.id);

        /// <summary>The catalog disease id this handler owns (equals DefinitionId.Value).</summary>
        public string DiseaseId => _definition.id;

        /// <summary>Catalog display name — domain truth; the projector masks it until Confirmed.</summary>
        public string DisplayName => _definition.display_name;

        public AfflictionEpisodeSnapshot? GetEpisode(Survivors.SurvivorId survivor)
        {
            if (survivor.IsEmpty) return null;
            if (!_disease.TryGetInfection(survivor.Value, _definition.id, out int daysSick, out bool quarantined))
                return null;

            return new AfflictionEpisodeSnapshot
            {
                EpisodeId = AfflictionEpisodeId.Create(survivor, DefinitionId),
                DefinitionId = DefinitionId,
                Survivor = survivor,
                SeverityValue = daysSick,
                StageLabel = _definition.display_name,
                IsActive = true,
                IsSymptomatic = quarantined
                    || _disease.IsContagious(survivor.Value, _definition.id)
                    || daysSick > 0
            };
        }

        public IReadOnlyList<SymptomProjection> ProjectSymptoms(Survivors.SurvivorId survivor)
        {
            var symptoms = new List<SymptomProjection>();
            if (survivor.IsEmpty) return symptoms;
            if (!_disease.TryGetInfection(survivor.Value, _definition.id, out int _, out bool _))
                return symptoms;

            var episode = AfflictionEpisodeId.Create(survivor, DefinitionId);
            switch (DiseaseVectorNames.Parse(_definition.vector))
            {
                case DiseaseVector.Water:
                    symptoms.Add(New(SymptomGastrointestinalDistress, "Watery diarrhoea", 0.9f, episode));
                    symptoms.Add(New(SymptomFever, "Fever", 0.5f, episode));
                    break;
                case DiseaseVector.Air:
                    symptoms.Add(New(SymptomPersistentCough, "Persistent cough", 0.9f, episode));
                    symptoms.Add(New(SymptomFever, "Fever", 0.6f, episode));
                    break;
                case DiseaseVector.Blood:
                    symptoms.Add(New(SymptomFever, "Fever with chills", 0.9f, episode));
                    symptoms.Add(New(SymptomFatigue, "Profound fatigue", 0.6f, episode));
                    break;
                case DiseaseVector.Spore:
                    symptoms.Add(New(SymptomPersistentCough, "Cough with dark flecks", 0.9f, episode));
                    symptoms.Add(New(SymptomBreathlessness, "Breathlessness", 0.6f, episode));
                    break;
            }
            return symptoms;
        }

        public bool CouldHaveCondition(Survivors.SurvivorId survivor)
        {
            return !survivor.IsEmpty && _disease.IsInfected(survivor.Value, _definition.id);
        }

        public string? ValidateTreatment(Survivors.SurvivorId survivor, string treatmentId, string? targetItem = null)
        {
            bool infected = _disease.IsInfected(survivor.Value, _definition.id);
            bool quarantined = _disease.IsQuarantined(survivor.Value, _definition.id);
            switch (treatmentId)
            {
                case MedicalTreatmentCatalog.TreatmentQuarantine:
                    if (!infected) return "not_infected";
                    if (quarantined) return "already_quarantined";
                    return null;
                case MedicalTreatmentCatalog.TreatmentRelease:
                    if (!quarantined) return "not_quarantined";
                    return null;
                default:
                    return "treatment_not_for_affliction";
            }
        }

        public bool ApplyTreatment(Survivors.SurvivorId survivor, string treatmentId, string? targetItem = null)
        {
            switch (treatmentId)
            {
                case MedicalTreatmentCatalog.TreatmentQuarantine:
                    _disease.Quarantine(survivor.Value, _definition.id);
                    return _disease.IsQuarantined(survivor.Value, _definition.id);
                case MedicalTreatmentCatalog.TreatmentRelease:
                    _disease.EndQuarantine(survivor.Value, _definition.id);
                    return !_disease.IsQuarantined(survivor.Value, _definition.id);
                default:
                    return false;
            }
        }

        public bool HasResolved(Survivors.SurvivorId survivor)
        {
            return !_disease.IsInfected(survivor.Value, _definition.id);
        }

        /// <summary>
        /// Register one handler per authored disease, in ordinal id order.
        /// Returns the number of handlers registered.
        /// </summary>
        public static int RegisterAll(MedicalPipelineCoordinator pipeline, DiseaseSystem disease, DiseaseCatalog catalog)
        {
            if (pipeline == null || disease == null || catalog == null) return 0;
            var defs = new List<DiseaseDefinition>(catalog.Diseases);
            defs.Sort(static (a, b) => string.CompareOrdinal(a.id, b.id));
            int count = 0;
            foreach (var d in defs)
            {
                if (d == null || string.IsNullOrEmpty(d.id)) continue;
                pipeline.RegisterHandler(new DiseaseAfflictionHandler(disease, d));
                count++;
            }
            return count;
        }

        private static SymptomProjection New(string id, string presentation, float intensity, AfflictionEpisodeId episode)
        {
            return new SymptomProjection
            {
                SymptomId = id,
                SourceEpisode = episode,
                Presentation = presentation,
                Intensity = intensity
            };
        }
    }
}
