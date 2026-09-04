using System;
using System.Collections.Generic;
using Ashfall.Core.Catalogs;

namespace Ashfall.Core.Sanatorium
{
    /// <summary>
    /// Authored psychological condition (psychological_therapies.json).
    /// Conditions map onto canonical survivor trauma surfaces
    /// (hypervigilance / flashback / guilt-insomnia) — the sanatorium never
    /// keeps a second trauma model.
    /// </summary>
    [Serializable]
    public sealed class TherapyConditionDefinition
    {
        public string condition_id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;

        /// <summary>Canonical surface: hypervigilance | flashback | guilt_insomnia | none.</summary>
        public string canonical_surface = "none";

        /// <summary>True only for explicitly authored reversible conditions.</summary>
        public bool reversible;
    }

    /// <summary>Authored therapy protocol (psychological_therapies.json).</summary>
    [Serializable]
    public sealed class PsychologicalTherapyDefinition
    {
        public string therapy_id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;
        public List<string>? eligible_conditions;      // condition_* refs
        public int duration_days;                      // > 0 campaign days
        public string staff_skill_id = string.Empty;   // skill_* ref
        public int staff_skill_threshold;              // 0..100 canonical skill value
        public List<InstitutionCatalogParse.CatalogCostEntry>? resource_costs;
        public int acute_stress_reduction_permille;    // 0..800 authored stress delta
        public int recovery_progress;                  // 0..100 treatment progress per completion
        public float relapse_modifier;                 // 0..1 reduction applied to relapse risk
        public string work_restriction = "none";       // none | light_duty | bedrest
        public List<string>? side_effects;
        public bool grants_journal_entry;              // dream transcription / testimony → archive/journal
        public List<string>? tags;

        public bool EligibleForCondition(string conditionId) =>
            eligible_conditions != null && conditionId != null && eligible_conditions.Contains(conditionId);
    }

    [Serializable]
    public sealed class PsychologicalTherapyCatalogContainer
    {
        public List<TherapyConditionDefinition> conditions = new();
        public List<PsychologicalTherapyDefinition> therapies = new();
    }

    /// <summary>Loads and validates psychological_therapies.json (the authority).</summary>
    public static class PsychologicalTherapyCatalogLoader
    {
        public const string DefaultFileName = "psychological_therapies.json";
        public const int ExpectedTherapyCount = 8;

        public static PsychologicalTherapyCatalogContainer Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                throw new InstitutionCatalogException(DefaultFileName, new[] { $"catalog file not found: {path}" });

            string rawText = fileIO.ReadAllText(path);
            var container = json.Deserialize<PsychologicalTherapyCatalogContainer>(rawText)
                ?? throw new InstitutionCatalogException(DefaultFileName, new[] { "catalog root failed to deserialize" });

            Validate(DefaultFileName, container.conditions, container.therapies);
            return container;
        }

        public static void Validate(
            string catalogName,
            List<TherapyConditionDefinition>? conditions,
            List<PsychologicalTherapyDefinition>? therapies)
        {
            var f = new InstitutionCatalogParse.Findings();

            var conditionIds = new HashSet<string>(StringComparer.Ordinal);
            if (conditions != null)
            {
                foreach (var c in conditions)
                {
                    string id = c.condition_id;
                    f.RequireNonEmpty(id, "condition_id", id);
                    if (!InstitutionCatalogParse.IsCanonicalSnakeCase(id))
                        f.Add(id, "condition_id", "must be canonical snake_case");
                    if (!conditionIds.Add(id))
                        f.Add(id, "condition_id", "duplicate condition_id within catalog");
                    f.RequireNonEmpty(id, "display_name", c.display_name);
                    if (c.canonical_surface is not ("hypervigilance" or "flashback" or "guilt_insomnia" or "none"))
                        f.Add(id, "canonical_surface", $"unknown canonical surface '{c.canonical_surface}'");
                }
            }

            if (therapies == null || therapies.Count == 0)
            {
                f.Add("*", "therapies", "catalog defines no therapies");
                f.ThrowIfAny(catalogName);
                return;
            }

            var seenTherapies = new HashSet<string>(StringComparer.Ordinal);
            foreach (var t in therapies)
            {
                string id = t.therapy_id;
                f.RequireNonEmpty(id, "therapy_id", id);
                if (!InstitutionCatalogParse.IsCanonicalSnakeCase(id))
                    f.Add(id, "therapy_id", "must be canonical snake_case");
                if (!seenTherapies.Add(id))
                    f.Add(id, "therapy_id", "duplicate therapy_id within catalog");

                f.RequireNonEmpty(id, "display_name", t.display_name);
                f.RequirePositive(id, "duration_days", t.duration_days, "days");
                f.RequireNonEmpty(id, "staff_skill_id", t.staff_skill_id);
                f.RequireRange(id, "staff_skill_threshold", t.staff_skill_threshold, 0, 100);
                f.RequireRange(id, "acute_stress_reduction_permille", t.acute_stress_reduction_permille, 0, 800);
                f.RequireRange(id, "recovery_progress", t.recovery_progress, 0, 100);
                f.RequireRange(id, "relapse_modifier", t.relapse_modifier, 0f, 1f);
                f.RequireCostItems(id, "resource_costs", t.resource_costs);

                if (t.eligible_conditions == null || t.eligible_conditions.Count == 0)
                {
                    f.Add(id, "eligible_conditions", "every therapy needs at least one eligible condition");
                }
                else
                {
                    foreach (string cond in t.eligible_conditions)
                    {
                        if (!conditionIds.Contains(cond))
                            f.Add(id, "eligible_conditions", $"unknown condition id '{cond}'");
                    }
                }

                if (t.work_restriction is not ("none" or "light_duty" or "bedrest"))
                    f.Add(id, "work_restriction", $"unknown work restriction '{t.work_restriction}'");
            }

            f.ThrowIfAny(catalogName);
        }
    }
}
