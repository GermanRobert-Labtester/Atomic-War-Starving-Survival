// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Ashfall.Core.IO;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// C2 / Plan 20B — data-authored shelter shielding/interior-radiation
    /// coefficients (shelter_shielding.json). One authority for every bunker
    /// protective contributor the interior model composes; no balance values
    /// live in a switch. All values are validated on load (ranges, finiteness)
    /// and surfaced through CatalogIntegrityValidator.
    /// </summary>
    public sealed class ShelterShieldingCatalog
    {
        public const string DefaultFileName = "shelter_shielding.json";

        public int schema_version { get; set; } = 1;

        /// <summary>Data-authored shelter interior baseline (mSv/h); replaces the legacy Core const.</summary>
        public float interior_baseline_rad_rate { get; set; } = 2.0f;

        public float filter_clog_max_ingress_penalty { get; set; } = 0.6f;
        public float ventilation_duct_breach_max_penalty { get; set; } = 0.4f;
        public float ventilation_saturation_max_penalty { get; set; } = 0.3f;
        public float ventilation_recirculation_penalty { get; set; } = 0.25f;
        public float airlock_breach_max_ingress_penalty { get; set; } = 0.8f;
        public float airlock_incident_ingress_penalty { get; set; } = 0.4f;

        /// <summary>Weather amplification of existing ingress defects only (plan §25).</summary>
        public float weather_ingress_scale { get; set; } = 0.01f;

        /// <summary>Indoor radon contribution (mSv/h per Bq/m³).</summary>
        public float radon_rad_per_bqm3 { get; set; } = 0.005f;
        public float flooding_contamination_scale { get; set; } = 4.0f;
        public float shelter_contamination_scale { get; set; } = 8.0f;
        public float decon_internal_reduction_fraction { get; set; } = 0.5f;

        public float max_ingress_multiplier { get; set; } = 5.0f;
        public float max_interior_rad_rate { get; set; } = 25.0f;

        [JsonIgnore]
        public List<string> Errors { get; } = new List<string>();

        /// <summary>True when every authored coefficient is present and in range.</summary>
        [JsonIgnore]
        public bool IsValid => Errors.Count == 0;

        public static ShelterShieldingCatalog LoadFromDirectory(string dataDir, IFileIO fileIO)
        {
            var catalog = new ShelterShieldingCatalog();
            if (fileIO == null || string.IsNullOrEmpty(dataDir))
            {
                catalog.Errors.Add("shelter_shielding.json: no data directory bound");
                return catalog;
            }

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
            {
                catalog.Errors.Add("shelter_shielding.json: file missing");
                return catalog;
            }

            try
            {
                var parsed = System.Text.Json.JsonSerializer.Deserialize<ShelterShieldingCatalog>(
                    fileIO.ReadAllText(path), SystemTextJsonSerializer.Options);
                if (parsed == null)
                {
                    catalog.Errors.Add("shelter_shielding.json: empty document");
                    return catalog;
                }
                Validate(parsed, catalog.Errors);
                return catalog.Errors.Count == 0 ? parsed : MergeWithDefaults(parsed, catalog.Errors);
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn("ShelterShieldingCatalog", path, ex);
                catalog.Errors.Add("shelter_shielding.json: parse failure — " + ex.Message);
                return catalog;
            }
        }

        /// <summary>Validation used by both the loader and the integrity gate.</summary>
        public static void Validate(ShelterShieldingCatalog c, List<string> errors)
        {
            if (c.schema_version != 1)
                errors.Add($"shelter_shielding.json: unsupported schema_version {c.schema_version} (expected 1)");

            RequireRange(c.interior_baseline_rad_rate, 0.01f, 100f, "interior_baseline_rad_rate", errors);
            RequireRange(c.filter_clog_max_ingress_penalty, 0f, 5f, "filter_clog_max_ingress_penalty", errors);
            RequireRange(c.ventilation_duct_breach_max_penalty, 0f, 5f, "ventilation_duct_breach_max_penalty", errors);
            RequireRange(c.ventilation_saturation_max_penalty, 0f, 5f, "ventilation_saturation_max_penalty", errors);
            RequireRange(c.ventilation_recirculation_penalty, 0f, 5f, "ventilation_recirculation_penalty", errors);
            RequireRange(c.airlock_breach_max_ingress_penalty, 0f, 5f, "airlock_breach_max_ingress_penalty", errors);
            RequireRange(c.airlock_incident_ingress_penalty, 0f, 5f, "airlock_incident_ingress_penalty", errors);
            RequireRange(c.weather_ingress_scale, 0f, 1f, "weather_ingress_scale", errors);
            RequireRange(c.radon_rad_per_bqm3, 0f, 1f, "radon_rad_per_bqm3", errors);
            RequireRange(c.flooding_contamination_scale, 0f, 100f, "flooding_contamination_scale", errors);
            RequireRange(c.shelter_contamination_scale, 0f, 100f, "shelter_contamination_scale", errors);
            RequireRange(c.decon_internal_reduction_fraction, 0f, 1f, "decon_internal_reduction_fraction", errors);
            RequireRange(c.max_ingress_multiplier, 1f, 10f, "max_ingress_multiplier", errors);
            RequireRange(c.max_interior_rad_rate, 1f, 200f, "max_interior_rad_rate", errors);
        }

        private static void RequireRange(float value, float min, float max, string field, List<string> errors)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
                errors.Add($"shelter_shielding.json: {field} must be finite");
            else if (value < min || value > max)
                errors.Add($"shelter_shielding.json: {field}={value} out of range [{min},{max}]");
        }

        private static ShelterShieldingCatalog MergeWithDefaults(
            ShelterShieldingCatalog parsed, List<string> errors)
        {
            // Invalid fields keep the safe defaults so a bad row cannot silently
            // scale dose; errors stay reported for the integrity gate.
            errors.Add("shelter_shielding.json: invalid coefficients replaced with safe defaults");
            return new ShelterShieldingCatalog();
        }
    }

    /// <summary>
    /// C2 / Plan 20B (§28) — contributor breakdown of the interior-radiation
    /// computation, produced by the same arithmetic path that drives the dose
    /// (one model, no parallel display math). <see cref="WeakestContributor"/>
    /// names the largest single mover of the interior rate so the shelter UI
    /// can answer "why is interior radiation high?".
    /// </summary>
    public sealed class ShelterShieldingBreakdown
    {
        public float StructuralBleed { get; set; }
        public float IngressPenalty { get; set; }
        public float IngressMultiplier { get; set; } = 1f;
        public float ExternalRad { get; set; }
        public float RadonRad { get; set; }
        public float FloodingRad { get; set; }
        public float ContaminationRad { get; set; }
        public float InternalRad { get; set; }
        public bool DeconReduced { get; set; }
        public float InteriorRad { get; set; }

        /// <summary>Largest single contributor label (defect or source), or
        /// "ceiling attenuation" when the structure is the limiting factor.</summary>
        public string WeakestContributor { get; set; } = string.Empty;

        /// <summary>mSv/h attributable to <see cref="WeakestContributor"/>.</summary>
        public float WeakestContribution { get; set; }
    }

    /// <summary>
    /// C2 / Plan 20B — the one shelter shielding/interior-radiation model
    /// (plan §22). Composition precedence:
    /// <code>
    /// interior baseline bleed (baseline × (1 − structural attenuation))
    ///   × ingress multiplier (filter clog + duct breach + saturation +
    ///     recirculation + airlock seal/incident; weather amplifies existing
    ///     defects only — an intact structure lets no weather in)
    ///   + internal sources (radon + flooding + shelter contamination)
    ///   × (decon reduction when a decon cycle is active)
    /// </code>
    /// Providers default to nominal (healthy) values, so an unbound model
    /// reproduces the legacy interior math exactly (plan §3.3 one model; §57.4
    /// no UI/runtime drift: every surface reads this result).
    /// </summary>
    public sealed class ShelterShieldingModel
    {
        public ShelterShieldingCatalog Config { get; }

        public Func<float>? StructuralAttenuationProvider;      // 0..1 weakest ceiling
        public Func<float>? FilterHealthPercentProvider;        // 0..100 (StartingLevel)
        public Func<float>? VentilationDuctIntegrityProvider;   // 0..100
        public Func<float>? VentilationFilterSaturationProvider;// 0..100 (higher = clogged)
        public Func<bool>? VentilationRecirculationProvider;
        public Func<float>? AirlockSealProvider;                // 1 secure … 0 breached
        public Func<bool>? AirlockIncidentProvider;
        public Func<float>? WeatherRadModifierProvider;         // outdoor rad add-on
        public Func<float>? IndoorRadonProvider;                // Bq/m³
        public Func<float>? FloodingContaminationProvider;      // 0..1
        public Func<float>? ShelterContaminationProvider;       // decon shelter level
        public Func<bool>? DeconActiveProvider;

        public ShelterShieldingModel(ShelterShieldingCatalog? config = null)
        {
            Config = config ?? new ShelterShieldingCatalog();
        }

        /// <summary>Interior ambient (mSv/h) at the given baseline zone rate.</summary>
        public float ComputeInteriorRad(float baselineZone)
            => GetBreakdown(baselineZone).InteriorRad;

        /// <summary>
        /// Full contributor breakdown — the single arithmetic path (§22.3).
        /// </summary>
        public ShelterShieldingBreakdown GetBreakdown(float baselineZone)
        {
            if (float.IsNaN(baselineZone) || baselineZone < 0f) baselineZone = 0f;
            var c = Config;

            float attenuation = Clamp01(StructuralAttenuationProvider?.Invoke() ?? 0f);
            float bleed = baselineZone * (1f - attenuation);

            float filterHealth = Clamp01((FilterHealthPercentProvider?.Invoke() ?? 100f) / 100f);
            float duct = Clamp01((VentilationDuctIntegrityProvider?.Invoke() ?? 100f) / 100f);
            float saturation = Clamp01((VentilationFilterSaturationProvider?.Invoke() ?? 0f) / 100f);
            float seal = Clamp01(AirlockSealProvider?.Invoke() ?? 1f);
            bool incident = AirlockIncidentProvider?.Invoke() ?? false;
            bool recirculating = VentilationRecirculationProvider?.Invoke() ?? false;

            float filterPenalty = c.filter_clog_max_ingress_penalty * (1f - filterHealth);
            float ductPenalty = c.ventilation_duct_breach_max_penalty * (1f - duct);
            float saturationPenalty = c.ventilation_saturation_max_penalty * saturation;
            float recirculationPenalty = recirculating ? c.ventilation_recirculation_penalty : 0f;
            float airlockPenalty = c.airlock_breach_max_ingress_penalty * (1f - seal);
            float incidentPenalty = incident ? c.airlock_incident_ingress_penalty : 0f;

            float penalty = filterPenalty + ductPenalty + saturationPenalty
                + recirculationPenalty + airlockPenalty + incidentPenalty;

            if (penalty > 0f)
            {
                float weather = MathF.Max(0f, WeatherRadModifierProvider?.Invoke() ?? 0f);
                penalty *= 1f + c.weather_ingress_scale * weather;
            }

            float ingress = MathF.Min(c.max_ingress_multiplier, MathF.Max(1f, 1f + penalty));
            float external = bleed * ingress;

            float radon = MathF.Max(0f, IndoorRadonProvider?.Invoke() ?? 0f) * c.radon_rad_per_bqm3;
            float flooding = Clamp01(FloodingContaminationProvider?.Invoke() ?? 0f)
                * c.flooding_contamination_scale;
            float contamination = MathF.Max(0f, ShelterContaminationProvider?.Invoke() ?? 0f)
                * c.shelter_contamination_scale;
            float internalBeforeDecon = radon + flooding + contamination;
            bool deconReduced = DeconActiveProvider?.Invoke() == true;
            float internalRad = deconReduced
                ? internalBeforeDecon * (1f - c.decon_internal_reduction_fraction)
                : internalBeforeDecon;

            float interior = external + internalRad;
            if (float.IsNaN(interior) || interior < 0f) interior = 0f;
            interior = MathF.Min(c.max_interior_rad_rate, interior);

            var breakdown = new ShelterShieldingBreakdown
            {
                StructuralBleed = bleed,
                IngressPenalty = penalty,
                IngressMultiplier = ingress,
                ExternalRad = external,
                RadonRad = radon,
                FloodingRad = flooding,
                ContaminationRad = contamination,
                InternalRad = internalRad,
                DeconReduced = deconReduced,
                InteriorRad = interior
            };

            // Weakest contributor = largest single mover of the interior rate.
            // Attribution: the penalty portion of the external rate
            // (external − structural bleed) is shared among defect contributors
            // proportionally to their pre-weather penalties, so weather
            // amplification and the ingress cap cannot inflate one defect's
            // attributed share beyond what it actually contributes.
            float penaltyPortion = MathF.Max(0f, external - bleed);
            float basePenaltySum = filterPenalty + ductPenalty + saturationPenalty
                + recirculationPenalty + airlockPenalty + incidentPenalty;
            float DefectShare(float basePenalty)
                => basePenaltySum > 0f ? penaltyPortion * (basePenalty / basePenaltySum) : 0f;
            var candidates = new (string Label, float Contribution)[]
            {
                ("air filter", DefectShare(filterPenalty)),
                ("ventilation duct", DefectShare(ductPenalty)),
                ("ventilation clogging", DefectShare(saturationPenalty)),
                ("recirculation", DefectShare(recirculationPenalty)),
                ("airlock seal", DefectShare(airlockPenalty)),
                ("airlock incident", DefectShare(incidentPenalty)),
                ("radon", radon),
                ("flooding", flooding),
                ("shelter contamination", contamination)
            };

            string weakest = "ceiling attenuation";
            float weakestContribution = bleed;
            for (int i = 0; i < candidates.Length; i++)
            {
                if (candidates[i].Contribution > weakestContribution)
                {
                    weakestContribution = candidates[i].Contribution;
                    weakest = candidates[i].Label;
                }
            }
            breakdown.WeakestContributor = weakest;
            breakdown.WeakestContribution = weakestContribution;
            return breakdown;
        }

        private static float Clamp01(float v)
            => float.IsNaN(v) ? 0f : MathF.Min(1f, MathF.Max(0f, v));
    }
}
