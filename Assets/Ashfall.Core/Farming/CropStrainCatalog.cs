// SPDX-License-Identifier: MIT
// ============================================================================
// Catalog    : CropStrainCatalog
// Data       : Assets/StreamingAssets/Data/crop_strains.json
// System     : AgricultureSystem (Plan 162)
// Purpose    : Advanced crop strains, pest infestations, and compost recipes
//              layered above the canonical GreenhouseSystem CropCatalog.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.IO;

namespace Ashfall.Core.Farming
{
    /// <summary>Nutrition category weights (0..1) for a food item or strain harvest.</summary>
    [Serializable]
    public sealed class NutritionProfileDef
    {
        public float calories { get; set; } = 1f;
        public float protein { get; set; }
        public float vitamin_c { get; set; }
        public float micronutrients { get; set; }
        public float fats { get; set; }
        public float fiber { get; set; }

        public static readonly string[] Categories =
        {
            "calories", "protein", "vitamin_c", "micronutrients", "fats", "fiber"
        };

        public float Get(string category) => category switch
        {
            "calories" => calories,
            "protein" => protein,
            "vitamin_c" => vitamin_c,
            "micronutrients" => micronutrients,
            "fats" => fats,
            "fiber" => fiber,
            _ => 0f
        };
    }

    /// <summary>One authored mutation outcome and its draw weight.</summary>
    [Serializable]
    public sealed class MutationOutcomeDef
    {
        public string outcome { get; set; } = "no_mutation";
        public string result_strain_id { get; set; } = "";
        public float weight { get; set; } = 1f;
    }

    /// <summary>
    /// Advanced strain profile for an existing canonical crop. The seed item
    /// must already resolve through GreenhouseExpansionCatalog.CropCatalog —
    /// a strain never invents a new seed or yield; it profiles how a known
    /// crop behaves under advanced agriculture.
    /// </summary>
    [Serializable]
    public sealed class CropStrainDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string seed_item_id { get; set; } = string.Empty;
        public float yield_modifier { get; set; } = 1f;
        public float soil_tolerance { get; set; } = 0.5f;
        public int toxicity_tolerance_permille { get; set; } = 300;
        public float radiation_tolerance { get; set; } = 0.5f;
        public float pest_susceptibility { get; set; } = 0.5f;
        public float mutation_threshold { get; set; } = 1f;
        public NutritionProfileDef nutrition_profile { get; set; } = new NutritionProfileDef();
        public List<MutationOutcomeDef> mutation_outcomes { get; set; } = new List<MutationOutcomeDef>();
        public List<string> tags { get; set; } = new List<string>();
    }

    /// <summary>Pest infestation definition. Ids use the infestation_ prefix.</summary>
    [Serializable]
    public sealed class PestDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public float base_chance_per_day { get; set; } = 0.02f;
        public float severity_step { get; set; } = 0.1f;
        public float yield_damage_at_full { get; set; } = 0.6f;
        public List<string> treats_with_item_ids { get; set; } = new List<string>();
        public string season_tag { get; set; } = "any";
        public List<string> target_strain_tags { get; set; } = new List<string>();
    }

    /// <summary>
    /// Lossy compost conversion: organic input → bounded fertilizer output.
    /// The output item is never a valid compost input, so no value loop can
    /// close (structural guarantee, plan §5.18).
    /// </summary>
    [Serializable]
    public sealed class CompostRecipeDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string input_item_id { get; set; } = string.Empty;
        public int input_count { get; set; } = 2;
        public string output_item_id { get; set; } = string.Empty;
        public int output_count { get; set; } = 1;
        public int duration_days { get; set; } = 5;
    }

    [Serializable]
    public sealed class CropStrainCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public List<CropStrainDef> strains { get; set; } = new List<CropStrainDef>();
        public List<PestDef> pests { get; set; } = new List<PestDef>();
        public List<CompostRecipeDef> compost_recipes { get; set; } = new List<CompostRecipeDef>();
    }

    /// <summary>Deterministic catalog validation diagnostics (plan §4).</summary>
    public sealed class CropStrainCatalogDiagnostic
    {
        public string File { get; }
        public string EntryId { get; }
        public string Field { get; }
        public string Problem { get; }

        public CropStrainCatalogDiagnostic(string file, string entryId, string field, string problem)
        {
            File = file; EntryId = entryId; Field = field; Problem = problem;
        }

        public override string ToString() =>
            $"{File}[{EntryId}].{Field}: {Problem}";
    }

    public static class CropStrainCatalogLoader
    {
        public const string DefaultFileName = "crop_strains.json";

        public static CropStrainCatalogContainer Load(
            string dataDir, IFileIO files, IJsonSerializer json)
        {
            var path = System.IO.Path.Combine(dataDir, DefaultFileName);
            if (!files.FileExists(path))
                return new CropStrainCatalogContainer();
            try
            {
                var text = files.ReadAllText(path);
                return json.Deserialize<CropStrainCatalogContainer>(text)
                       ?? new CropStrainCatalogContainer();
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(path, "crop strain catalog", ex);
                return new CropStrainCatalogContainer();
            }
        }

        /// <summary>Aggregates all rule violations in stable (file order) sequence.</summary>
        public static List<CropStrainCatalogDiagnostic> Validate(CropStrainCatalogContainer catalog)
        {
            var diags = new List<CropStrainCatalogDiagnostic>();
            string file = DefaultFileName;
            var strainIds = new HashSet<string>(StringComparer.Ordinal);

            foreach (var s in catalog.strains)
            {
                if (s == null) continue;
                if (string.IsNullOrEmpty(s.id) || !s.id.StartsWith("strain_", StringComparison.Ordinal))
                    diags.Add(new CropStrainCatalogDiagnostic(file, s.id, "id", "must be a strain_ id"));
                else if (!strainIds.Add(s.id))
                    diags.Add(new CropStrainCatalogDiagnostic(file, s.id, "id", "duplicate strain id"));

                var crop = string.IsNullOrEmpty(s.seed_item_id)
                    ? null
                    : GreenhouseExpansionCatalog.CropCatalog.Get(s.seed_item_id);
                if (crop == null)
                    diags.Add(new CropStrainCatalogDiagnostic(
                        file, s.id, "seed_item_id",
                        $"'{s.seed_item_id}' does not resolve in the canonical CropCatalog"));

                if (s.yield_modifier < 0.5f || s.yield_modifier > 1.5f)
                    diags.Add(new CropStrainCatalogDiagnostic(
                        file, s.id, "yield_modifier", "must be within [0.5, 1.5]"));

                if (s.pest_susceptibility < 0f || s.pest_susceptibility > 1f)
                    diags.Add(new CropStrainCatalogDiagnostic(
                        file, s.id, "pest_susceptibility", "must be within [0, 1]"));

                if (s.mutation_threshold < 0f || s.mutation_threshold > 1f)
                    diags.Add(new CropStrainCatalogDiagnostic(
                        file, s.id, "mutation_threshold", "must be within [0, 1]"));

                if (s.nutrition_profile != null)
                {
                    foreach (var c in NutritionProfileDef.Categories)
                    {
                        float v = s.nutrition_profile.Get(c);
                        if (v < 0f || v > 1f)
                            diags.Add(new CropStrainCatalogDiagnostic(
                                file, s.id, $"nutrition_profile.{c}", "must be within [0, 1]"));
                    }
                }

                float total = 0f;
                foreach (var m in s.mutation_outcomes)
                {
                    if (m == null) continue;
                    if (m.weight < 0f)
                        diags.Add(new CropStrainCatalogDiagnostic(
                            file, s.id, "mutation_outcomes.weight", "must be >= 0"));
                    total += m.weight;
                    if (!IsLegalOutcome(m.outcome))
                        diags.Add(new CropStrainCatalogDiagnostic(
                            file, s.id, "mutation_outcomes.outcome", $"'{m.outcome}' is not a legal outcome"));
                    if (m.outcome == "hardy_strain" && string.IsNullOrEmpty(m.result_strain_id))
                        diags.Add(new CropStrainCatalogDiagnostic(
                            file, s.id, "mutation_outcomes.result_strain_id",
                            "hardy_strain outcomes must name a variant strain"));
                }
                if (s.mutation_outcomes.Count > 0 && total <= 0f)
                    diags.Add(new CropStrainCatalogDiagnostic(
                        file, s.id, "mutation_outcomes", "weights must sum above 0"));
            }

            // Second pass: variant references resolve.
            var known = new HashSet<string>(strainIds, StringComparer.Ordinal);
            foreach (var s in catalog.strains)
            {
                if (s == null) continue;
                foreach (var m in s.mutation_outcomes)
                {
                    if (m != null && !string.IsNullOrEmpty(m.result_strain_id)
                        && !known.Contains(m.result_strain_id))
                        diags.Add(new CropStrainCatalogDiagnostic(
                            file, s.id, "mutation_outcomes.result_strain_id",
                            $"'{m.result_strain_id}' does not resolve in this catalog"));
                }
            }

            var pestIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var p in catalog.pests)
            {
                if (p == null) continue;
                if (string.IsNullOrEmpty(p.id) || !p.id.StartsWith("infestation_", StringComparison.Ordinal))
                    diags.Add(new CropStrainCatalogDiagnostic(file, p.id, "id", "must be an infestation_ id"));
                else if (!pestIds.Add(p.id))
                    diags.Add(new CropStrainCatalogDiagnostic(file, p.id, "id", "duplicate pest id"));
                if (p.base_chance_per_day < 0f || p.base_chance_per_day > 1f)
                    diags.Add(new CropStrainCatalogDiagnostic(
                        file, p.id, "base_chance_per_day", "must be within [0, 1]"));
                if (p.severity_step <= 0f || p.severity_step > 1f)
                    diags.Add(new CropStrainCatalogDiagnostic(
                        file, p.id, "severity_step", "must be within (0, 1]"));
                if (p.yield_damage_at_full < 0f || p.yield_damage_at_full > 1f)
                    diags.Add(new CropStrainCatalogDiagnostic(
                        file, p.id, "yield_damage_at_full", "must be within [0, 1]"));
                if (p.treats_with_item_ids == null || p.treats_with_item_ids.Count == 0)
                    diags.Add(new CropStrainCatalogDiagnostic(
                        file, p.id, "treats_with_item_ids", "at least one treatment item is required"));
            }

            var recipeIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var r in catalog.compost_recipes)
            {
                if (r == null) continue;
                if (string.IsNullOrEmpty(r.id) || !r.id.StartsWith("recipe_", StringComparison.Ordinal))
                    diags.Add(new CropStrainCatalogDiagnostic(file, r.id, "id", "must be a recipe_ id"));
                else if (!recipeIds.Add(r.id))
                    diags.Add(new CropStrainCatalogDiagnostic(file, r.id, "id", "duplicate recipe id"));
                if (r.input_count < 1 || r.output_count < 1)
                    diags.Add(new CropStrainCatalogDiagnostic(
                        file, r.id, "counts", "input/output counts must be >= 1"));
                if (r.duration_days < 1)
                    diags.Add(new CropStrainCatalogDiagnostic(
                        file, r.id, "duration_days", "must be >= 1"));
                // Structural no-loop rule: the output of composting must never
                // be a valid compost input (lossy chain, plan §5.18).
                if (!string.IsNullOrEmpty(r.output_item_id) &&
                    catalog.compost_recipes.Exists(x => x != null && x.input_item_id == r.output_item_id))
                    diags.Add(new CropStrainCatalogDiagnostic(
                        file, r.id, "output_item_id",
                        "compost output must not itself be a compost input (value loop)"));
            }

            return diags;
        }

        public static bool IsLegalOutcome(string outcome) => outcome switch
        {
            "no_mutation" => true,
            "yield_boost" => true,
            "yield_penalty" => true,
            "toxic_harvest" => true,
            "hardy_strain" => true,
            "sterile_seed" => true,
            _ => false
        };
    }
}
