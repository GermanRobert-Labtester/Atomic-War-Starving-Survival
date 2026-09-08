// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// Centralized adapter that maps runtime affliction, disease, vital deficit,
    /// and psychology identifiers to authored condition text records in <see cref="MedicalTextCatalog"/>.
    /// Guaranteed deterministic; UI panels must never maintain independent mapping dictionaries.
    /// </summary>
    public static class MedicalConditionResolver
    {
        private static readonly Dictionary<string, string> s_runtimeToCatalogId =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                // Core treatable afflictions
                [MedicalTreatmentCatalog.RespiratoryDegenerationId] = "medical_asthma",
                [MedicalTreatmentCatalog.RadiationSicknessId]       = "medical_radiation_exposure",
                [MedicalTreatmentCatalog.ChemicalDependencyId]      = "medical_addiction",
                [MedicalTreatmentCatalog.HealthDeficitId]           = "medical_laceration",

                // Core observe-only psychology projections
                [MedicalTreatmentCatalog.CombatTraumaId]            = "medical_ptsd",
                [MedicalTreatmentCatalog.SomaticFlashbackId]        = "medical_panic_attack",
                [MedicalTreatmentCatalog.GuiltInsomniaId]           = "medical_insomnia",

                // Disease catalog mappings
                ["disease_acute_radiation_syndrome"]                = "medical_radiation_exposure",
                ["disease_septic_rust_wound_fever"]                 = "medical_wound_infection",
                ["disease_spore_wound_dermatitis"]                  = "medical_infection",
                ["disease_cholera"]                                 = "medical_dehydration_severe",
                ["disease_dysentery"]                               = "medical_dehydration_severe",
                ["disease_zoonotic_flu"]                            = "medical_infection",
                ["disease_fungal_respiratory"]                      = "medical_asthma",
                ["disease_condemned_air_cough"]                     = "medical_asthma",
                ["disease_silo_lung"]                               = "medical_asthma",
                ["disease_deep_excavation_mold_lung"]               = "medical_asthma",

                // Survival vital deficit mappings
                ["deficit_dehydration"]                             = "medical_dehydration",
                ["deficit_severe_dehydration"]                      = "medical_dehydration_severe",
                ["deficit_starvation"]                              = "medical_starvation",
                ["deficit_hypothermia"]                             = "medical_hypothermia",
                ["deficit_heatstroke"]                              = "medical_heatstroke",
                ["deficit_chronic_radiation"]                       = "medical_chronic_radiation",
                ["chronic_radiation"]                               = "medical_chronic_radiation",
                ["permanent_lung_damage"]                           = "medical_asthma",
            };

        /// <summary>
        /// Resolves a runtime identifier to an authored medical condition ID.
        /// Returns null if there is no safe or authentic mapping.
        /// </summary>
        public static string? ResolveToMedicalTextId(string? runtimeId)
        {
            if (string.IsNullOrWhiteSpace(runtimeId)) return null;

            if (s_runtimeToCatalogId.TryGetValue(runtimeId, out var mappedId))
                return mappedId;

            // Direct pass-through if already in medical_ snake_case format
            if (runtimeId.StartsWith("medical_", StringComparison.OrdinalIgnoreCase))
                return runtimeId.ToLowerInvariant();

            return null;
        }

        /// <summary>
        /// Retrieves clinical prose for a patient's condition deterministically.
        /// </summary>
        public static ClinicalProseSnapshot? GetClinicalProse(
            MedicalTextCatalog? catalog,
            string? runtimeConditionId,
            string? survivorId = null)
        {
            if (catalog == null || string.IsNullOrWhiteSpace(runtimeConditionId))
                return null;

            string? catalogId = ResolveToMedicalTextId(runtimeConditionId);
            if (catalogId == null) return null;

            var entry = catalog.TryGetConditionText(catalogId);
            if (entry == null) return null;

            int seed = 0;
            if (!string.IsNullOrEmpty(survivorId))
            {
                // Stable deterministic pseudo-hash using StableHash (djb2/x33) without System.Random or GetHashCode
                seed = Math.Abs((StableHash.Of(survivorId) * 397) ^ StableHash.Of(runtimeConditionId));
            }

            return new ClinicalProseSnapshot
            {
                CatalogId = entry.id,
                DisplayName = entry.display_name,
                DiagnosisSummary = entry.diagnosis_text,
                SymptomLine = catalog.GetSymptomProse(entry.id, seed),
                ComplicationWarning = catalog.GetComplicationWarning(entry.id),
                RecoveryLine = catalog.GetRecoveryProse(entry.id)
            };
        }
    }

    /// <summary>
    /// Pure projection snapshot of clinical prose for UI display.
    /// </summary>
    public sealed class ClinicalProseSnapshot
    {
        public string CatalogId = string.Empty;
        public string DisplayName = string.Empty;
        public string DiagnosisSummary = string.Empty;
        public string SymptomLine = string.Empty;
        public string ComplicationWarning = string.Empty;
        public string RecoveryLine = string.Empty;
    }
}
