// SPDX-License-Identifier: MIT
// Task #133 — Radiation-sickness and health-deficit pipeline handlers.
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// Pipeline adapter for radiation sickness. Dose and phase progression stay
    /// fully authoritative in <c>RadiationSystem</c> and
    /// <c>RadiationPhaseProgression</c> — the handler observes them through
    /// host-supplied delegates and applies iodine / anti-rad through the
    /// host's existing survivors-domain APIs. The pipeline never writes dose.
    ///
    /// <para>Symptoms are projections of the sickness phase and booked dose
    /// bands; booked dosimetry (DoseLedger) remains a measurement, not a
    /// diagnosis.</para>
    /// </summary>
    public sealed class RadiationSicknessAfflictionHandler : IAfflictionHandler
    {
        public const string SymptomNausea = "symptom_nausea";
        public const string SymptomWeakness = "symptom_weakness";
        public const string SymptomBleeding = "symptom_bleeding";

        private readonly Func<string, float> _getDose;
        private readonly Func<string, string> _getPhaseName;
        private readonly Func<string, bool> _hasAcuteSickness;

        /// <summary>Apply potassium-iodide protection through the radiation/survivors domain.</summary>
        private readonly Func<string, bool> _applyIodine;
        /// <summary>Apply an anti-rad chelation dose through the radiation/survivors domain.</summary>
        private readonly Func<string, float, bool> _applyAntiRad;

        public RadiationSicknessAfflictionHandler(
            Func<string, float> getDose,
            Func<string, string> getPhaseName,
            Func<string, bool> hasAcuteSickness,
            Func<string, bool> applyIodine,
            Func<string, float, bool> applyAntiRad)
        {
            _getDose = getDose ?? throw new ArgumentNullException(nameof(getDose));
            _getPhaseName = getPhaseName ?? throw new ArgumentNullException(nameof(getPhaseName));
            _hasAcuteSickness = hasAcuteSickness ?? throw new ArgumentNullException(nameof(hasAcuteSickness));
            _applyIodine = applyIodine ?? throw new ArgumentNullException(nameof(applyIodine));
            _applyAntiRad = applyAntiRad ?? throw new ArgumentNullException(nameof(applyAntiRad));
        }

        public AfflictionId DefinitionId =>
            new AfflictionId(MedicalTreatmentCatalog.RadiationSicknessId);

        public AfflictionEpisodeSnapshot? GetEpisode(Survivors.SurvivorId survivor)
        {
            if (survivor.IsEmpty) return null;
            string phase = _getPhaseName(survivor.Value);
            float dose = _getDose(survivor.Value);
            bool active = !string.Equals(phase, "Healthy", StringComparison.Ordinal) || _hasAcuteSickness(survivor.Value);
            if (!active && dose <= 0f) return null;

            return new AfflictionEpisodeSnapshot
            {
                EpisodeId = AfflictionEpisodeId.Create(survivor, DefinitionId),
                DefinitionId = DefinitionId,
                Survivor = survivor,
                SeverityValue = dose,
                StageLabel = active ? phase : "EXPOSED",
                IsActive = active,
                IsSymptomatic = active
            };
        }

        public IReadOnlyList<SymptomProjection> ProjectSymptoms(Survivors.SurvivorId survivor)
        {
            var symptoms = new List<SymptomProjection>();
            if (survivor.IsEmpty) return symptoms;
            string phase = _getPhaseName(survivor.Value);
            bool acute = _hasAcuteSickness(survivor.Value);
            if (string.Equals(phase, "Healthy", StringComparison.Ordinal) && !acute) return symptoms;

            var episode = AfflictionEpisodeId.Create(survivor, DefinitionId);
            symptoms.Add(new SymptomProjection
            {
                SymptomId = SymptomNausea,
                SourceEpisode = episode,
                Presentation = "Nausea",
                Intensity = 0.5f
            });
            symptoms.Add(new SymptomProjection
            {
                SymptomId = SymptomWeakness,
                SourceEpisode = episode,
                Presentation = "Weakness",
                Intensity = 0.6f
            });
            if (string.Equals(phase, "ManifestIllness", StringComparison.Ordinal))
            {
                symptoms.Add(new SymptomProjection
                {
                    SymptomId = SymptomBleeding,
                    SourceEpisode = episode,
                    Presentation = "Bleeding",
                    Intensity = 0.9f
                });
            }
            return symptoms;
        }

        public bool CouldHaveCondition(Survivors.SurvivorId survivor)
        {
            if (survivor.IsEmpty) return false;
            string phase = _getPhaseName(survivor.Value);
            return !string.Equals(phase, "Healthy", StringComparison.Ordinal)
                || _hasAcuteSickness(survivor.Value)
                || _getDose(survivor.Value) > 0f;
        }

        public string? ValidateTreatment(Survivors.SurvivorId survivor, string treatmentId, string? targetItem = null)
        {
            switch (treatmentId)
            {
                case MedicalTreatmentCatalog.TreatmentIodine:
                    return null; // prophylaxis — legal before exposure (existing UI rule)
                case MedicalTreatmentCatalog.TreatmentAntiRad:
                    if (_getDose(survivor.Value) <= 0f)
                        return "no_radiation_dose";
                    return null;
                default:
                    return "treatment_not_for_affliction";
            }
        }

        public bool ApplyTreatment(Survivors.SurvivorId survivor, string treatmentId, string? targetItem = null)
        {
            return treatmentId switch
            {
                MedicalTreatmentCatalog.TreatmentIodine => _applyIodine(survivor.Value),
                MedicalTreatmentCatalog.TreatmentAntiRad => _applyAntiRad(survivor.Value, 40f),
                _ => false
            };
        }

        public bool HasResolved(Survivors.SurvivorId survivor)
        {
            string phase = _getPhaseName(survivor.Value);
            return string.Equals(phase, "Healthy", StringComparison.Ordinal)
                && !_hasAcuteSickness(survivor.Value)
                && _getDose(survivor.Value) <= 0f;
        }
    }

    /// <summary>
    /// Pipeline adapter for low health (bandage-style care). The value is owned
    /// by the Needs domain; this handler reads it and applies healing through
    /// the survivors domain. It exists so the general-care treatment button
    /// follows the same validated transaction path as every other treatment.
    /// </summary>
    public sealed class HealthDeficitAfflictionHandler : IAfflictionHandler
    {
        public const string SymptomWounded = "symptom_wounded";

        private readonly Func<string, float> _getHealth;
        private readonly Func<string, float> _getMaxHealth;
        private readonly Func<string, float, bool> _applyHeal;

        public HealthDeficitAfflictionHandler(
            Func<string, float> getHealth,
            Func<string, float> getMaxHealth,
            Func<string, float, bool> applyHeal)
        {
            _getHealth = getHealth ?? throw new ArgumentNullException(nameof(getHealth));
            _getMaxHealth = getMaxHealth ?? throw new ArgumentNullException(nameof(getMaxHealth));
            _applyHeal = applyHeal ?? throw new ArgumentNullException(nameof(applyHeal));
        }

        public AfflictionId DefinitionId =>
            new AfflictionId(MedicalTreatmentCatalog.HealthDeficitId);

        public AfflictionEpisodeSnapshot? GetEpisode(Survivors.SurvivorId survivor)
        {
            if (survivor.IsEmpty) return null;
            float health = _getHealth(survivor.Value);
            float max = Math.Max(1f, _getMaxHealth(survivor.Value));
            if (health >= max) return null;
            return new AfflictionEpisodeSnapshot
            {
                EpisodeId = AfflictionEpisodeId.Create(survivor, DefinitionId),
                DefinitionId = DefinitionId,
                Survivor = survivor,
                SeverityValue = (max - health) / max * 100f,
                StageLabel = health < max * 0.3f ? "CRITICAL" : "WOUNDED",
                IsActive = true,
                IsSymptomatic = true
            };
        }

        public IReadOnlyList<SymptomProjection> ProjectSymptoms(Survivors.SurvivorId survivor)
        {
            if (survivor.IsEmpty) return Array.Empty<SymptomProjection>();
            float health = _getHealth(survivor.Value);
            float max = Math.Max(1f, _getMaxHealth(survivor.Value));
            if (health >= max) return Array.Empty<SymptomProjection>();
            return new[]
            {
                new SymptomProjection
                {
                    SymptomId = SymptomWounded,
                    SourceEpisode = AfflictionEpisodeId.Create(survivor, DefinitionId),
                    Presentation = "Wounded",
                    Intensity = (max - health) / max
                }
            };
        }

        public bool CouldHaveCondition(Survivors.SurvivorId survivor)
        {
            return !survivor.IsEmpty
                && _getHealth(survivor.Value) < Math.Max(1f, _getMaxHealth(survivor.Value));
        }

        public string? ValidateTreatment(Survivors.SurvivorId survivor, string treatmentId, string? targetItem = null)
        {
            if (treatmentId != MedicalTreatmentCatalog.TreatmentBandage)
                return "treatment_not_for_affliction";
            if (_getHealth(survivor.Value) >= Math.Max(1f, _getMaxHealth(survivor.Value)))
                return "health_full";
            return null;
        }

        public bool ApplyTreatment(Survivors.SurvivorId survivor, string treatmentId, string? targetItem = null)
        {
            return treatmentId == MedicalTreatmentCatalog.TreatmentBandage && _applyHeal(survivor.Value, 25f);
        }

        public bool HasResolved(Survivors.SurvivorId survivor)
        {
            return _getHealth(survivor.Value) >= Math.Max(1f, _getMaxHealth(survivor.Value));
        }
    }
}
