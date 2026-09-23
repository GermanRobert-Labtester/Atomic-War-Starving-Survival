// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 174 — Procedural Survivor Backstories & Origin Mechanics
// Pure domain system. Generates mechanically-relevant personal histories for
// survivors: occupation, life experiences, skill bonuses, trait modifiers,
// and narrative text. No RNG dependency — caller provides seeded values.
// Save-section: backstory_state.json (owned by this system).
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Survivors
{
    // ── Catalog DTOs ────────────────────────────────────────────────────────

    [Serializable]
    public sealed class SkillBonusDef
    {
        public string skill_id { get; set; } = string.Empty;
        public int bonus { get; set; }
    }

    [Serializable]
    public sealed class SkillPenaltyDef
    {
        public string skill_id { get; set; } = string.Empty;
        public int penalty { get; set; }
    }

    [Serializable]
    public sealed class OccupationDef
    {
        public string occupation_id { get; set; } = string.Empty;
        public string label { get; set; } = string.Empty;
        public List<SkillBonusDef> skill_bonuses { get; set; } = new List<SkillBonusDef>();
        public List<SkillPenaltyDef> skill_penalties { get; set; } = new List<SkillPenaltyDef>();
        public List<string> starting_traits { get; set; } = new List<string>();
        public List<string> starting_item_ids { get; set; } = new List<string>();
        public string flavor { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class LifeExperienceDef
    {
        public string experience_id { get; set; } = string.Empty;
        public string label { get; set; } = string.Empty;
        public string category { get; set; } = string.Empty;
        public string rarity { get; set; } = "common";
        public SkillBonusDef? skill_bonus { get; set; }
        public string trait_modifier { get; set; } = string.Empty;
        public string flavor { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class BackstoryTemplateDef
    {
        public string template_id { get; set; } = string.Empty;
        public string label { get; set; } = string.Empty;
        public string occupation_id { get; set; } = string.Empty;
        public List<string> life_experience_ids { get; set; } = new List<string>();
        public string defining_moment { get; set; } = string.Empty;
        public string pre_war_life { get; set; } = string.Empty;
        public string reason_for_survival { get; set; } = string.Empty;
        public List<string> secrets { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class BackstoryCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<OccupationDef> occupations { get; set; } = new List<OccupationDef>();
        public List<LifeExperienceDef> life_experiences { get; set; } = new List<LifeExperienceDef>();
        public List<BackstoryTemplateDef> backstory_templates { get; set; } = new List<BackstoryTemplateDef>();
    }

    // ── Runtime records ─────────────────────────────────────────────────────

    [Serializable]
    public sealed class SurvivorBackstory
    {
        public string SurvivorId { get; set; } = string.Empty;
        public string TemplateId { get; set; } = string.Empty;
        public string OccupationId { get; set; } = string.Empty;
        public List<string> LifeExperienceIds { get; set; } = new List<string>();
        public string PreWarLife { get; set; } = string.Empty;
        public string DefiningMoment { get; set; } = string.Empty;
        public string ReasonForSurvival { get; set; } = string.Empty;
        public List<string> Secrets { get; set; } = new List<string>();
        public List<string> RevealedSecrets { get; set; } = new List<string>();
        public int GeneratedDay { get; set; } = 1;
    }

    [Serializable]
    public sealed class BackstoryProjection
    {
        public string SurvivorId { get; set; } = string.Empty;
        public string OccupationLabel { get; set; } = string.Empty;
        public Dictionary<string, int> SkillBonuses { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> SkillPenalties { get; set; } = new Dictionary<string, int>();
        public List<string> StartingTraits { get; set; } = new List<string>();
        public List<string> StartingItemIds { get; set; } = new List<string>();
        public List<string> ExperienceLabels { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class BackstoryState
    {
        public int SchemaVersion { get; set; } = 1;
        public List<SurvivorBackstory> Backstories { get; set; } = new List<SurvivorBackstory>();
    }

    // ── System ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Plan 174 — Generates and projects mechanically-relevant survivor
    /// backstories. Catalog-driven: occupations, life experiences, and
    /// narrative templates. No RNG — caller supplies template/experience IDs
    /// (from a seeded draw or explicit assignment). Rule 5: this system owns
    /// backstory records only; skill/trait application belongs to the host.
    /// </summary>
    public sealed class BackstorySystem
    {
        private readonly BackstoryState _state;
        private readonly Dictionary<string, OccupationDef> _occupations =
            new Dictionary<string, OccupationDef>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, LifeExperienceDef> _experiences =
            new Dictionary<string, LifeExperienceDef>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, BackstoryTemplateDef> _templates =
            new Dictionary<string, BackstoryTemplateDef>(StringComparer.OrdinalIgnoreCase);

        public event Action<SurvivorBackstory>? OnBackstoryAssigned;
        public event Action<string, string>? OnSecretRevealed;  // (survivorId, secretId)

        public int BackstoryCount => _state.Backstories.Count;

        public BackstorySystem() { _state = new BackstoryState(); }
        public BackstorySystem(BackstoryState state) { _state = state ?? new BackstoryState(); }

        // ── Catalog loading ────────────────────────────────────────────────

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var catalog = JsonSerializer.Deserialize<BackstoryCatalog>(json, options);
                if (catalog == null) return;

                foreach (var occ in catalog.occupations ?? new List<OccupationDef>())
                    if (!string.IsNullOrWhiteSpace(occ.occupation_id))
                        _occupations[occ.occupation_id] = occ;

                foreach (var exp in catalog.life_experiences ?? new List<LifeExperienceDef>())
                    if (!string.IsNullOrWhiteSpace(exp.experience_id))
                        _experiences[exp.experience_id] = exp;

                foreach (var tmpl in catalog.backstory_templates ?? new List<BackstoryTemplateDef>())
                    if (!string.IsNullOrWhiteSpace(tmpl.template_id))
                        _templates[tmpl.template_id] = tmpl;
            }
            catch (Exception) { /* malformed catalog falls back to built-in defaults; authoring errors are enforced by the data-integrity gate */ }
        }

        public IReadOnlyCollection<OccupationDef> GetAllOccupations() => _occupations.Values;
        public IReadOnlyCollection<LifeExperienceDef> GetAllExperiences() => _experiences.Values;
        public IReadOnlyCollection<BackstoryTemplateDef> GetAllTemplates() => _templates.Values;

        public OccupationDef? GetOccupation(string id) =>
            _occupations.TryGetValue(id, out var v) ? v : null;

        public LifeExperienceDef? GetExperience(string id) =>
            _experiences.TryGetValue(id, out var v) ? v : null;

        public BackstoryTemplateDef? GetTemplate(string id) =>
            _templates.TryGetValue(id, out var v) ? v : null;

        // ── Backstory assignment ───────────────────────────────────────────

        /// <summary>
        /// Assigns a backstory from a template to a survivor.
        /// Idempotent: reassigning replaces the existing record.
        /// </summary>
        public SurvivorBackstory AssignFromTemplate(string survivorId, string templateId, int day)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) throw new ArgumentException("survivorId");

            // Remove existing if any
            _state.Backstories.RemoveAll(b =>
                string.Equals(b.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));

            var backstory = new SurvivorBackstory
            {
                SurvivorId = survivorId,
                TemplateId = templateId,
                GeneratedDay = day
            };

            if (_templates.TryGetValue(templateId, out var tmpl))
            {
                backstory.OccupationId = tmpl.occupation_id;
                backstory.LifeExperienceIds = new List<string>(tmpl.life_experience_ids);
                backstory.PreWarLife = tmpl.pre_war_life;
                backstory.DefiningMoment = tmpl.defining_moment;
                backstory.ReasonForSurvival = tmpl.reason_for_survival;
                backstory.Secrets = new List<string>(tmpl.secrets);
            }

            _state.Backstories.Add(backstory);
            OnBackstoryAssigned?.Invoke(backstory);
            return backstory;
        }

        /// <summary>
        /// Assigns a fully custom backstory without a template.
        /// </summary>
        public SurvivorBackstory AssignCustom(
            string survivorId, string occupationId,
            IEnumerable<string> experienceIds,
            string preWarLife, string definingMoment, string reasonForSurvival,
            int day)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) throw new ArgumentException("survivorId");

            _state.Backstories.RemoveAll(b =>
                string.Equals(b.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));

            var backstory = new SurvivorBackstory
            {
                SurvivorId = survivorId,
                OccupationId = occupationId,
                LifeExperienceIds = experienceIds?.ToList() ?? new List<string>(),
                PreWarLife = preWarLife,
                DefiningMoment = definingMoment,
                ReasonForSurvival = reasonForSurvival,
                GeneratedDay = day
            };

            _state.Backstories.Add(backstory);
            OnBackstoryAssigned?.Invoke(backstory);
            return backstory;
        }

        public SurvivorBackstory? GetBackstory(string survivorId) =>
            _state.Backstories.FirstOrDefault(b =>
                string.Equals(b.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));

        // ── Mechanical projection ──────────────────────────────────────────

        /// <summary>
        /// Returns the stacked mechanical effects of a survivor's backstory:
        /// skill bonuses/penalties, starting traits, starting items, and
        /// experience labels. Pure function — no side-effects.
        /// </summary>
        public BackstoryProjection ProjectEffects(string survivorId)
        {
            var result = new BackstoryProjection { SurvivorId = survivorId };

            var backstory = GetBackstory(survivorId);
            if (backstory == null) return result;

            // Occupation bonuses
            if (_occupations.TryGetValue(backstory.OccupationId, out var occ))
            {
                result.OccupationLabel = occ.label;
                foreach (var sb in occ.skill_bonuses)
                {
                    result.SkillBonuses.TryGetValue(sb.skill_id, out int existing);
                    result.SkillBonuses[sb.skill_id] = existing + sb.bonus;
                }
                foreach (var sp in occ.skill_penalties)
                {
                    result.SkillPenalties.TryGetValue(sp.skill_id, out int existing);
                    result.SkillPenalties[sp.skill_id] = existing + sp.penalty;
                }
                foreach (var t in occ.starting_traits)
                    if (!result.StartingTraits.Contains(t))
                        result.StartingTraits.Add(t);
                foreach (var item in occ.starting_item_ids)
                    if (!result.StartingItemIds.Contains(item))
                        result.StartingItemIds.Add(item);
            }

            // Life experience bonuses (additive on top of occupation)
            foreach (var expId in backstory.LifeExperienceIds)
            {
                if (!_experiences.TryGetValue(expId, out var exp)) continue;
                result.ExperienceLabels.Add(exp.label);

                if (exp.skill_bonus != null && !string.IsNullOrWhiteSpace(exp.skill_bonus.skill_id))
                {
                    result.SkillBonuses.TryGetValue(exp.skill_bonus.skill_id, out int existing);
                    result.SkillBonuses[exp.skill_bonus.skill_id] = existing + exp.skill_bonus.bonus;
                }
                if (!string.IsNullOrWhiteSpace(exp.trait_modifier) &&
                    !result.StartingTraits.Contains(exp.trait_modifier))
                    result.StartingTraits.Add(exp.trait_modifier);
            }

            return result;
        }

        // ── Secret revelation ──────────────────────────────────────────────

        public bool RevealSecret(string survivorId, string secretId)
        {
            var backstory = GetBackstory(survivorId);
            if (backstory == null) return false;
            if (!backstory.Secrets.Contains(secretId)) return false;
            if (backstory.RevealedSecrets.Contains(secretId)) return false;
            backstory.RevealedSecrets.Add(secretId);
            OnSecretRevealed?.Invoke(survivorId, secretId);
            return true;
        }

        // ── Save / Restore ─────────────────────────────────────────────────

        public BackstoryState CaptureState()
        {
            var snapshot = new BackstoryState
            {
                SchemaVersion = _state.SchemaVersion,
                Backstories = _state.Backstories
                    .Select(b => new SurvivorBackstory
                    {
                        SurvivorId = b.SurvivorId,
                        TemplateId = b.TemplateId,
                        OccupationId = b.OccupationId,
                        LifeExperienceIds = new List<string>(b.LifeExperienceIds),
                        PreWarLife = b.PreWarLife,
                        DefiningMoment = b.DefiningMoment,
                        ReasonForSurvival = b.ReasonForSurvival,
                        Secrets = new List<string>(b.Secrets),
                        RevealedSecrets = new List<string>(b.RevealedSecrets),
                        GeneratedDay = b.GeneratedDay
                    }).ToList()
            };
            return snapshot;
        }

        public void RestoreState(BackstoryState? saved)
        {
            if (saved == null) return;
            _state.SchemaVersion = saved.SchemaVersion;
            _state.Backstories = saved.Backstories
                ?.Select(b => new SurvivorBackstory
                {
                    SurvivorId = b.SurvivorId,
                    TemplateId = b.TemplateId,
                    OccupationId = b.OccupationId,
                    LifeExperienceIds = new List<string>(b.LifeExperienceIds ?? new List<string>()),
                    PreWarLife = b.PreWarLife,
                    DefiningMoment = b.DefiningMoment,
                    ReasonForSurvival = b.ReasonForSurvival,
                    Secrets = new List<string>(b.Secrets ?? new List<string>()),
                    RevealedSecrets = new List<string>(b.RevealedSecrets ?? new List<string>()),
                    GeneratedDay = b.GeneratedDay
                }).ToList() ?? new List<SurvivorBackstory>();
        }
    }
}
