using System;
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{

/// <summary>
/// ASHFALL — Skill Progression Engine (engine-agnostic Core port).
///
/// Replaces <c>Assets/_Game/Survivors/SkillProgressionSystem.cs</c>. Same
/// semantics, same authored constants, same save/load shape — but driven by
/// <see cref="ISeededRng"/> instead of <c>System.Random</c> + Unity's
/// <c>Mathf.Clamp</c>, and read against <see cref="SkillActor"/> instead of
/// the legacy <c>Survivor</c> runtime class. Host adapters supply the actor
/// from engine state — Core no longer reaches into Unity / Godot.
///
/// Action-driven progression (Prompt #179): survivors earn hidden XP by doing
/// work; perks unlock as muscle memory. Expert tracks are limited to one
/// predetermined discipline per survivor. Skills go Dormant after 14 unused
/// days; desperate stress can trigger an Epiphany of instant mastery.
/// </summary>
public sealed class SkillProgressionSystem
{
    public const float DefaultXpPerAction = 5f;
    public const int DormantAfterUnusedDays = 14;
    public const float EpiphanyMoraleThreshold = 10f;
    public const float EpiphanyHealthThreshold = 20f;
    public const float EpiphanyChance = 0.05f;
    public const float EpiphanyMoraleRestore = 100f;

    /// <summary>Known discipline ids (snake_case).</summary>
    public static readonly string[] Disciplines =
    {
        "medical", "crafting", "science", "combat", "scavenging", "survival"
    };

    /// <summary>Threshold for milestone-only skills (action XP can never unlock).</summary>
    public const float UnreachableXp = 999999f;

    private readonly Dictionary<string, SkillProgressionState> _bySurvivor =
        new Dictionary<string, SkillProgressionState>(StringComparer.Ordinal);

    /// <summary>Cached per-discipline bonus totals, keyed by survivorId. Used as the read source for delta-encoding.</summary>
    private readonly Dictionary<string, Dictionary<string, float>> _bonusCacheBySurvivor =
        new Dictionary<string, Dictionary<string, float>>(StringComparer.Ordinal);

    private readonly List<SkillDef> _catalog = new List<SkillDef>();
    private readonly Dictionary<string, SkillDef> _bySkillId =
        new Dictionary<string, SkillDef>(StringComparer.Ordinal);

    /// <summary>Multiplier lookup: returns action-driven XP multiplier for an actor. <c>null</c> disables multi-perk adjustment.</summary>
    public Func<string, float> ActionXpMultiplier { get; set; }

    /// <summary>Lookup: stops skill decay across the entire bunker (e.g. Archivist perk).</summary>
    public Func<bool> BunkerSkillDecayStopped { get; set; }

    /// <summary>Lookup: maximum morale cap for an actor (Traumatized etc.); applied after Epiphany restore.</summary>
    public Func<string, float> MaxMoraleCap { get; set; }

    /// <summary>Host-side mood setter; called after a successful Epiphany. <c>null</c> disables the morale restore.</summary>
    public Action<string, float> ApplyMorale { get; set; }

    /// <summary>Fired when an actor gains XP. Args: (actor, discipline, newXp).</summary>
    public event Action<SkillActor, string, float> OnXpGained;

    /// <summary>Fired when a skill is earned. Args: (actor, skillId).</summary>
    public event Action<SkillActor, string> OnSkillEarned;

    /// <summary>Fired when a skill is decayed to dormant. Args: (actor, skillId).</summary>
    public event Action<SkillActor, string> OnSkillDormant;

    /// <summary>Fired when a dormant skill is reactivated. Args: (actor, skillId).</summary>
    public event Action<SkillActor, string> OnSkillReactivated;

    /// <summary>Fired when an Epiphany fires. Args: (actor, highlightSkillId-or-null).</summary>
    public event Action<SkillActor, string> OnEpiphany;

    public int CatalogCount => _catalog.Count;

    // ─── Catalog ────────────────────────────────────────────────────

    /// <summary>
    /// Add or replace a skill definition. Replacement by id is idempotent —
    /// a second registration with the same id overrides the catalog entry.
    /// </summary>
    public void RegisterSkill(SkillDef def)
    {
        if (def == null || string.IsNullOrEmpty(def.id)) return;
        string id = def.id;
        if (_bySkillId.ContainsKey(id))
        {
            for (int i = 0; i < _catalog.Count; i++)
            {
                if (_catalog[i] != null && string.Equals(_catalog[i].id, id, StringComparison.Ordinal))
                {
                    _catalog[i] = def;
                    break;
                }
            }
        }
        else
        {
            _catalog.Add(def);
        }
        _bySkillId[id] = def;
    }

    public SkillDef GetSkill(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;
        return _bySkillId.TryGetValue(id, out var s) ? s : null;
    }

    /// <summary>
    /// Register a runtime-defined default catalog (no JSON asset required).
    /// Matches the legacy <c>RegisterDefaultPerks</c> shape with the seven-tier
    /// window made explicit; the long list of quest-granted milestone skills is
    /// preserved by id (discipline on a latent skill is set at the narrative
    /// award site, not in the catalog fallback).
    /// </summary>
    public void RegisterDefaultSkills()
    {
        // Tier-Threshold action-driven skills (each discipline has one).
        RegisterSkill(MakeSkill("skill_field_dressing", "Field Dressing", "medical", 50f, 0.10f, false));
        RegisterSkill(MakeSkill("skill_steady_hands", "Steady Hands", "medical", 120f, 0.20f, true));
        RegisterSkill(MakeSkill("skill_rough_repairs", "Rough Repairs", "crafting", 50f, 0.10f, false));
        RegisterSkill(MakeSkill("skill_workshop_sense", "Workshop Sense", "crafting", 120f, 0.20f, true));
        RegisterSkill(MakeSkill("skill_signal_ear", "Signal Ear", "science", 50f, 0.10f, false));
        RegisterSkill(MakeSkill("skill_cold_analysis", "Cold Analysis", "science", 120f, 0.20f, true));
        RegisterSkill(MakeSkill("skill_watchful", "Watchful", "combat", 50f, 0.10f, false));
        RegisterSkill(MakeSkill("skill_trail_memory", "Trail Memory", "scavenging", 50f, 0.10f, false));
        RegisterSkill(MakeSkill("skill_hard_living", "Hard Living", "survival", 50f, 0.10f, false));

        // Milestone-only skills (action XP can never unlock them — granted
        // explicitly through TryGrantSkill at narrative paths).
        RegisterCombatMilestones();
        RegisterSurvivalMilestones();
        RegisterShelterMilestones();
        RegisterMedicalMilestones();
        RegisterExpeditionMilestones();
        RegisterSocialMilestones();
        RegisterLatentExpertTraits();
    }

    private void RegisterCombatMilestones()
    {
        RegisterSkill(MakeMilestone("skill_tap_rack_bang", "Tap-Rack-Bang", "combat", 0.05f));
        RegisterSkill(MakeMilestone("skill_cold_bore", "Cold Bore", "combat", 0.05f));
        RegisterSkill(MakeMilestone("skill_suppressing_fire", "Suppressing Fire", "combat", 0.05f));
        RegisterSkill(MakeMilestone("skill_close_quarters", "Close Quarters", "combat", 0.10f));
        RegisterSkill(MakeMilestone("skill_trap_setter", "Trap Setter", "combat", 0.05f));
        RegisterSkill(MakeMilestone("skill_looters_reflex", "Looter's Reflex", "scavenging", 0.05f));
        RegisterSkill(MakeMilestone("skill_desensitized", "Desensitized", "combat", 0f));
    }

    private void RegisterSurvivalMilestones()
    {
        RegisterSkill(MakeMilestone("skill_ration_stretcher", "Ration Stretcher", "survival", 0.05f));
        RegisterSkill(MakeMilestone("skill_iron_stomach", "Iron Stomach", "survival", 0.05f));
        RegisterSkill(MakeMilestone("skill_wasteland_brewer", "Wasteland Brewer", "survival", 0.05f));
        RegisterSkill(MakeMilestone("skill_butcher", "The Butcher", "survival", 0.05f));
        RegisterSkill(MakeMilestone("skill_pharmacologist", "Pharmacologist", "medical", 0.10f));
        RegisterSkill(MakeMilestone("skill_mycology", "Mycology", "science", 0.05f));
    }

    private void RegisterShelterMilestones()
    {
        RegisterSkill(MakeMilestone("skill_jury_rigger", "Jury-Rigger", "crafting", 0.05f));
        RegisterSkill(MakeMilestone("skill_structural_engineer", "Structural Engineer", "crafting", 0.10f));
        RegisterSkill(MakeMilestone("skill_hvac_tech", "HVAC Technician", "crafting", 0.05f));
        RegisterSkill(MakeMilestone("skill_scrapper", "Scrapper", "crafting", 0.05f));
        RegisterSkill(MakeMilestone("skill_sandhog", "The Sandhog", "crafting", 0.05f));
        RegisterSkill(MakeMilestone("skill_thermodynamics", "Thermodynamics", "science", 0.05f));
    }

    private void RegisterMedicalMilestones()
    {
        RegisterSkill(MakeMilestone("skill_steady_hands_field", "Steady Hands (Field)", "medical", 0.20f));
        RegisterSkill(MakeMilestone("skill_triage_under_fire", "Triage Under Fire", "medical", 0.10f));
        RegisterSkill(MakeMilestone("skill_radiologist", "Radiologist", "medical", 0.05f));
        RegisterSkill(MakeMilestone("skill_anatomist", "Anatomist", "medical", 0.10f));
        RegisterSkill(MakeMilestone("skill_paramedic", "Paramedic", "medical", 0.10f));
    }

    private void RegisterExpeditionMilestones()
    {
        RegisterSkill(MakeMilestone("skill_pack_mule", "Pack Mule", "scavenging", 0.05f));
        RegisterSkill(MakeMilestone("skill_light_step", "Light Step", "scavenging", 0.05f));
        RegisterSkill(MakeMilestone("skill_urban_pathfinder", "Urban Pathfinder", "scavenging", 0.05f));
        RegisterSkill(MakeMilestone("skill_night_terror", "Night Terror", "combat", 0.10f));
        RegisterSkill(MakeMilestone("skill_forager", "Forager", "survival", 0.05f));
    }

    private void RegisterSocialMilestones()
    {
        RegisterSkill(MakeMilestone("skill_de_escalator", "De-Escalator", "survival", 0.05f));
        RegisterSkill(MakeMilestone("skill_quartermaster", "Quartermaster", "scavenging", 0.05f));
        RegisterSkill(MakeMilestone("skill_taskmaster", "Taskmaster", "survival", 0.10f));
    }

    private void RegisterLatentExpertTraits()
    {
        // Latent quest-granted skills; ids preserved for cross-host save
        // compatibility. Discipline is set at the narrative grant site.
        string[] latentIds =
        {
            "skill_miracle_worker", "skill_alchemist", "skill_zoonotic_expert", "skill_anchor",
            "skill_death_blind", "skill_warlord", "skill_peacekeeper", "skill_juggernaut",
            "skill_apex_predator", "skill_survivalist", "skill_hydraulic_master",
            "skill_grid_walker", "skill_vault_builder", "skill_grease_monkey",
            "skill_synthesizer", "skill_gaia", "skill_wasteland_runner", "skill_ghost",
            "skill_stormcaller", "skill_rad_walker", "skill_polymath", "skill_demagogue",
            "skill_shepherd", "skill_muckraker", "skill_voice_of_the_wastes",
            "skill_iron_chef", "skill_tireless", "skill_asbestos", "skill_armorer",
            "skill_tinkerer", "skill_lorekeeper", "skill_zealots_bane", "skill_chem_resistant",
            "skill_protector", "skill_matriarch", "skill_pillar_of_atlas",
            "skill_wasteland_scout", "skill_child_of_the_ash", "skill_cold_calculus",
            "skill_butcher_of_day_30", "skill_master_manipulator", "skill_dragons_hoard",
            "skill_art_of_war", "skill_demolitions_expert", "skill_ghost_shooter",
            "skill_supply_chain_master", "skill_reclaimed_youth", "skill_soul_weaver",
            "skill_lone_wolf", "skill_grounded_optimist", "skill_living_saint",
            "skill_humbled_healer", "skill_clean_and_sober", "skill_the_watcher",
            "skill_hyper_aware", "skill_fire_breather", "skill_sonar",
            "skill_improvised_engineering", "skill_radiotrophic", "skill_apex_scavenger",
            "skill_zen_state", "skill_master_geneticist", "skill_the_enforcer",
            "skill_legend_of_the_wastes", "skill_the_statesman", "skill_cybernetics",
            "skill_beacon_of_truth", "skill_master_pathologist", "skill_monopolist",
            "skill_deep_delver", "skill_logistics_master", "skill_forge_master",
            "skill_sanitization_expert", "skill_deforester", "skill_epidemiologist",
            "skill_celestial_navigator", "skill_archivist", "skill_auditor",
            "skill_maestro", "skill_blockade_runner", "skill_executioner", "skill_shadow",
            "skill_master_of_disguise", "skill_mechanic_prodigy", "skill_diplomat",
            "skill_wasteland_gladiator", "skill_chief_of_medicine", "skill_drone_operator",
            "skill_choir_of_one", "skill_hive_tactics", "skill_hive_healing",
            "skill_truth_seeker", "skill_wildman", "skill_second_life", "skill_iron_will",
            "skill_unseen_listener", "skill_ruthless_capitalist", "skill_prodigy",
            "skill_commander", "skill_cyber_arm", "skill_redemption", "skill_overclocked",
            "skill_wasteland_guardian", "skill_omniscience"
        };
        for (int i = 0; i < latentIds.Length; i++)
        {
            RegisterSkill(MakeMilestone(latentIds[i], ReplaceSkillUnderscores(latentIds[i]),
                string.Empty, 0.20f));
        }
    }

    private static string ReplaceSkillUnderscores(string id)
    {
        if (string.IsNullOrEmpty(id)) return string.Empty;
        string stripped = id.StartsWith("skill_", StringComparison.Ordinal) ? id.Substring(6) : id;
        return stripped.Replace('_', ' ');
    }

    private static SkillDef MakeSkill(string id, string display, string discipline,
        float xp, float bonus, bool expert) =>
        new SkillDef { id = id, displayName = display, description = display,
            disciplineId = discipline, xpThreshold = xp, skillBonus = bonus,
            isExpertSkill = expert };

    private static SkillDef MakeMilestone(string id, string display, string discipline, float bonus) =>
        MakeSkill(id, display, discipline, UnreachableXp, bonus, false);

    // ─── Awarding ────────────────────────────────────────────────────

    /// <summary>
    /// Grant a catalog skill by id without XP (milestone / narrative awards).
    /// Returns true if newly granted.
    /// </summary>
    public bool TryGrantSkill(SkillActor actor, string skillId, int currentDay = 0)
    {
        if (actor == null || !actor.IsAlive || string.IsNullOrEmpty(skillId)) return false;
        var skill = GetSkill(skillId);
        if (skill == null) return false;
        var state = GetOrCreate(actor.Id);
        if (!CanEarnSkill(actor, state, skill)) return false;
        GrantSkill(actor, state, skill);
        if (!string.IsNullOrEmpty(skill.disciplineId))
            SetLastUsedDay(state, skill.disciplineId, currentDay);
        return true;
    }

    /// <summary>
    /// Record that an actor performed work in a discipline. Awards hidden XP,
    /// reactivates dormant skills, may grant new skills, may fire Epiphany.
    /// </summary>
    public void RecordAction(SkillActor actor, string disciplineId, float xpAmount,
        int currentDay, ISeededRng rng = null!)
    {
        if (actor == null || !actor.IsAlive) return;
        if (string.IsNullOrEmpty(disciplineId) || xpAmount <= 0f) return;

        var state = GetOrCreate(actor.Id);
        SetLastUsedDay(state, disciplineId, currentDay);

        // Reactivate dormant skills for this discipline.
        ReactivateDormantForDiscipline(actor, state, disciplineId);

        // Stress Epiphany may instantly master before XP add — if so, stop.
        if (TryStressEpiphany(actor, state, disciplineId, rng))
            return;

        // Optional multiplier (e.g. Polymath-style fast learner).
        float mult = ActionXpMultiplier?.Invoke(actor.Id) ?? 1f;
        float awarded = xpAmount * mult;

        float prev = GetXp(state, disciplineId);
        float next = prev + awarded;
        SetXp(state, disciplineId, next);
        OnXpGained?.Invoke(actor, disciplineId, next);

        TryAwardSkills(actor, state, disciplineId);
        SyncSkillBonuses(actor, state);
    }

    public float GetXp(string actorId, string disciplineId)
    {
        if (string.IsNullOrEmpty(actorId) || string.IsNullOrEmpty(disciplineId)) return 0f;
        return _bySurvivor.TryGetValue(actorId, out var state)
            ? GetXp(state, disciplineId) : 0f;
    }

    public bool HasActiveSkill(string actorId, string skillId)
        => _bySurvivor.TryGetValue(actorId ?? string.Empty, out var state)
            && skillId != null && state.activeSkillIds.Contains(skillId);

    public bool HasDormantSkill(string actorId, string skillId)
        => _bySurvivor.TryGetValue(actorId ?? string.Empty, out var state)
            && skillId != null && state.dormantSkillIds.Contains(skillId);

    public bool HasEarnedExpertSkill(string actorId)
        => _bySurvivor.TryGetValue(actorId ?? string.Empty, out var state) && state.expertSkillEarned;

    public IReadOnlyList<string> GetActiveSkillIds(string actorId)
    {
        if (string.IsNullOrEmpty(actorId)) return Array.Empty<string>();
        if (!_bySurvivor.TryGetValue(actorId, out var state)) return Array.Empty<string>();
        return state.activeSkillIds;
    }

    /// <summary>Days since this discipline was last practiced; <c>-1</c> if never.</summary>
    public int DaysSinceLastPractice(string actorId, string disciplineId, int currentDay)
    {
        if (!_bySurvivor.TryGetValue(actorId ?? string.Empty, out var state)) return -1;
        for (int i = 0; i < state.disciplineIds.Count; i++)
        {
            if (!string.Equals(state.disciplineIds[i], disciplineId, StringComparison.Ordinal))
                continue;
            int d = state.lastUsedDays != null && i < state.lastUsedDays.Count ? state.lastUsedDays[i] : 0;
            if (d <= 0) return -1;
            return currentDay - d;
        }
        return -1;
    }

    // ─── Decay ───────────────────────────────────────────────────────

    /// <summary>
    /// Once per game-day: any active skill whose discipline has not been
    /// practiced for <see cref="DormantAfterUnusedDays"/> becomes Dormant
    /// and loses mechanical benefit until practiced again.
    /// </summary>
    public void TickDaily(int currentDay, IReadOnlyList<SkillActor> actors)
    {
        if (actors == null) return;
        if (BunkerSkillDecayStopped != null && BunkerSkillDecayStopped())
            return;
        for (int i = 0; i < actors.Count; i++)
        {
            var actor = actors[i];
            if (actor == null || !actor.IsAlive) continue;
            if (!_bySurvivor.TryGetValue(actor.Id, out var state)) continue;
            ApplyDecay(actor, state, currentDay);
        }
    }

    private void ApplyDecay(SkillActor actor, SkillProgressionState state, int currentDay)
    {
        var active = state.activeSkillIds;
        for (int i = active.Count - 1; i >= 0; i--)
        {
            var skill = GetSkill(active[i]);
            if (skill == null || string.IsNullOrEmpty(skill.disciplineId)) continue;

            int lastUsed = GetLastUsedDay(state, skill.disciplineId);
            if (lastUsed <= 0) lastUsed = currentDay;

            if (currentDay - lastUsed < DormantAfterUnusedDays) continue;

            string skillId = active[i];
            active.RemoveAt(i);
            if (!state.dormantSkillIds.Contains(skillId))
                state.dormantSkillIds.Add(skillId);
            OnSkillDormant?.Invoke(actor, skillId);
        }
        SyncSkillBonuses(actor, state);
    }

    private void ReactivateDormantForDiscipline(SkillActor actor, SkillProgressionState state, string disciplineId)
    {
        for (int i = state.dormantSkillIds.Count - 1; i >= 0; i--)
        {
            var skill = GetSkill(state.dormantSkillIds[i]);
            if (skill == null) continue;
            if (!string.Equals(skill.disciplineId, disciplineId, StringComparison.Ordinal))
                continue;

            string skillId = state.dormantSkillIds[i];
            state.dormantSkillIds.RemoveAt(i);
            if (!state.activeSkillIds.Contains(skillId))
                state.activeSkillIds.Add(skillId);
            OnSkillReactivated?.Invoke(actor, skillId);
        }
    }

    // ─── Epiphany ────────────────────────────────────────────────────

    private bool TryStressEpiphany(SkillActor actor, SkillProgressionState state,
        string disciplineId, ISeededRng rng)
    {
        bool desperate = actor.Morale < EpiphanyMoraleThreshold
                         || actor.Health < EpiphanyHealthThreshold;
        if (!desperate) return false;
        if (rng == null || rng.NextDouble() >= EpiphanyChance) return false;

        // Compute effective max threshold in this discipline so XP is set above
        // any action-tier perk.
        float maxThreshold = 0f;
        for (int i = 0; i < _catalog.Count; i++)
        {
            var p = _catalog[i];
            if (p == null || !string.Equals(p.disciplineId, disciplineId, StringComparison.Ordinal))
                continue;
            if (p.xpThreshold > maxThreshold && p.xpThreshold < UnreachableXp)
                maxThreshold = p.xpThreshold;
        }
        if (maxThreshold <= 0f) maxThreshold = 100f;

        SetXp(state, disciplineId, maxThreshold);
        TryAwardSkills(actor, state, disciplineId);
        ForceGrantBestSkill(actor, state, disciplineId);

        // Surge morale to the cap. MaxMoraleCap lets the host kill the surge
        // if a trait hard-caps (e.g. Traumatized -> 60). ApplyMorale is the
        // host-side setter; when null we skip the moral restore step.
        if (ApplyMorale != null)
        {
            float moraleCap = MaxMoraleCap?.Invoke(actor.Id) ?? 100f;
            float clamped = Clamp(EpiphanyMoraleRestore, 0f, Math.Max(0f, moraleCap));
            ApplyMorale(actor.Id, clamped);
        }
        SyncSkillBonuses(actor, state);

        // Highlight is the highest-threshold active skill for this discipline.
        string highlight = null;
        for (int i = 0; i < state.activeSkillIds.Count; i++)
        {
            var s = GetSkill(state.activeSkillIds[i]);
            if (s == null || !string.Equals(s.disciplineId, disciplineId, StringComparison.Ordinal))
                continue;
            if (highlight == null) highlight = s.id;
            else
            {
                var prev = GetSkill(highlight);
                if (prev != null && s.xpThreshold > prev.xpThreshold) highlight = s.id;
            }
        }
        OnEpiphany?.Invoke(actor, highlight);
        return true;
    }

    private void ForceGrantBestSkill(SkillActor actor, SkillProgressionState state, string disciplineId)
    {
        SkillDef? best = null;
        for (int i = 0; i < _catalog.Count; i++)
        {
            var p = _catalog[i];
            if (p == null || !string.Equals(p.disciplineId, disciplineId, StringComparison.Ordinal))
                continue;
            if (!CanEarnSkill(actor, state, p)) continue;
            if (best == null || (p.xpThreshold < UnreachableXp && p.xpThreshold > best.xpThreshold))
                best = p;
        }
        if (best == null) return;
        GrantSkill(actor, state, best);
    }

    // ─── Award helpers ──────────────────────────────────────────────

    private void TryAwardSkills(SkillActor actor, SkillProgressionState state, string disciplineId)
    {
        float xp = GetXp(state, disciplineId);
        for (int i = 0; i < _catalog.Count; i++)
        {
            var skill = _catalog[i];
            if (skill == null || string.IsNullOrEmpty(skill.id)) continue;
            if (!string.Equals(skill.disciplineId, disciplineId, StringComparison.Ordinal))
                continue;
            if (xp < skill.xpThreshold) continue;
            if (!CanEarnSkill(actor, state, skill)) continue;
            GrantSkill(actor, state, skill);
        }
    }

    private bool CanEarnSkill(SkillActor actor, SkillProgressionState state, SkillDef skill)
    {
        if (skill == null || string.IsNullOrEmpty(skill.id)) return false;
        if (state.activeSkillIds.Contains(skill.id) || state.dormantSkillIds.Contains(skill.id))
            return false;

        if (skill.isExpertSkill)
        {
            if (state.expertSkillEarned) return false;
            if (string.IsNullOrEmpty(actor.ExpertDisciplineId)) return false;
            if (!string.Equals(actor.ExpertDisciplineId, skill.disciplineId, StringComparison.Ordinal))
                return false;
        }
        return true;
    }

    private void GrantSkill(SkillActor actor, SkillProgressionState state, SkillDef skill)
    {
        if (skill == null) return;
        if (state.activeSkillIds.Contains(skill.id) || state.dormantSkillIds.Contains(skill.id))
            return;
        state.activeSkillIds.Add(skill.id);
        if (skill.isExpertSkill) state.expertSkillEarned = true;
        OnSkillEarned?.Invoke(actor, skill.id);
        SyncSkillBonuses(actor, state);
    }

    /// <summary>
    /// Write mechanical bonuses onto the actor so the effective-skill read
    /// reflects the active (non-dormant) catalog. Only Active contributes.
    /// The cache is the read source for delta-encoding across calls.
    /// </summary>
    public void SyncSkillBonuses(SkillActor actor, SkillProgressionState state = null!)
    {
        if (actor == null) return;
        if (state == null && !_bySurvivor.TryGetValue(actor.Id, out state))
        {
            // Zero out known disciplines if no state exists.
            for (int i = 0; i < Disciplines.Length; i++)
                actor.SetSkillBonus(Disciplines[i], 0f);
            return;
        }

        // Compute totals from active skills.
        var totals = new Dictionary<string, float>(StringComparer.Ordinal);
        for (int i = 0; i < Disciplines.Length; i++)
            totals[Disciplines[i]] = 0f;

        if (state != null)
        {
            for (int i = 0; i < state.activeSkillIds.Count; i++)
            {
                var s = GetSkill(state.activeSkillIds[i]);
                if (s == null || string.IsNullOrEmpty(s.disciplineId)) continue;
                if (!totals.ContainsKey(s.disciplineId))
                    totals[s.disciplineId] = 0f;
                totals[s.disciplineId] += s.skillBonus;
            }
        }

        // Push each discipline total to the actor and update local cache.
        for (int i = 0; i < Disciplines.Length; i++)
        {
            float value = totals[Disciplines[i]];
            actor.SetSkillBonus(Disciplines[i], value);
        }

        _bonusCacheBySurvivor[actor.Id] = totals;
    }

    /// <summary>Read-only peek at the last computed bonus total for a discipline; useful for UI snapshotting.</summary>
    public float GetCachedBonus(string actorId, string disciplineId)
    {
        if (string.IsNullOrEmpty(actorId) || string.IsNullOrEmpty(disciplineId)) return 0f;
        if (!_bonusCacheBySurvivor.TryGetValue(actorId, out var totals)) return 0f;
        return totals.TryGetValue(disciplineId, out float v) ? v : 0f;
    }

    // ─── Save / Load ─────────────────────────────────────────────────

    /// <summary>
    /// Capture the simulation state for save. Plain DTO with parallel arrays
    /// so legacy <c>JsonUtility</c> paths stay viable (the Core default
    /// (<c>SystemTextJsonSerializer</c>) handles dictionaries too).
    /// </summary>
    public SkillProgressionSaveState CaptureState()
    {
        var save = new SkillProgressionSaveState();
        foreach (var kv in _bySurvivor)
        {
            var st = kv.Value;
            save.survivorIds.Add(kv.Key);
            var entry = new SkillProgressionState
            {
                expertSkillEarned = st.expertSkillEarned,
                activeSkillIds = new List<string>(st.activeSkillIds),
                dormantSkillIds = new List<string>(st.dormantSkillIds),
                disciplineIds = new List<string>(st.disciplineIds),
                xpValues = new List<float>(st.xpValues),
                lastUsedDays = new List<int>(st.lastUsedDays),
            };
            save.entries.Add(entry);
        }
        return save;
    }

    /// <summary>
    /// Restore a prior save envelope. Sanity-checks parallel-array lengths and
    /// deduplicates by survivor id (last write wins, matches the legacy semantics).
    /// </summary>
    public void RestoreState(SkillProgressionSaveState save,
        IReadOnlyList<SkillActor> actors = null!)
    {
        _bySurvivor.Clear();
        _bonusCacheBySurvivor.Clear();
        if (save == null || save.survivorIds == null || save.entries == null) return;

        for (int i = 0; i < save.survivorIds.Count; i++)
        {
            string actorId = save.survivorIds[i];
            if (string.IsNullOrEmpty(actorId)) continue;
            if (i >= save.entries.Count) break;
            var e = save.entries[i];
            _bySurvivor[actorId] = new SkillProgressionState
            {
                expertSkillEarned = e.expertSkillEarned,
                activeSkillIds = e.activeSkillIds != null ? new List<string>(e.activeSkillIds) : new List<string>(),
                dormantSkillIds = e.dormantSkillIds != null ? new List<string>(e.dormantSkillIds) : new List<string>(),
                disciplineIds = e.disciplineIds != null ? new List<string>(e.disciplineIds) : new List<string>(),
                xpValues = e.xpValues != null ? new List<float>(e.xpValues) : new List<float>(),
                lastUsedDays = e.lastUsedDays != null ? new List<int>(e.lastUsedDays) : new List<int>(),
            };
        }

        if (actors == null) return;
        for (int i = 0; i < actors.Count; i++)
        {
            var actor = actors[i];
            if (actor == null) continue;
            if (_bySurvivor.TryGetValue(actor.Id, out var st))
                SyncSkillBonuses(actor, st);
        }
    }

    // ─── Dictionary shims ───────────────────────────────────────────

    private SkillProgressionState GetOrCreate(string actorId)
    {
        if (string.IsNullOrEmpty(actorId))
            return new SkillProgressionState();
        if (!_bySurvivor.TryGetValue(actorId, out var state))
        {
            state = new SkillProgressionState();
            _bySurvivor[actorId] = state;
        }
        return state;
    }

    private static float GetXp(SkillProgressionState state, string disciplineId)
    {
        if (state == null || state.disciplineIds == null || state.xpValues == null)
            return 0f;
        for (int i = 0; i < state.disciplineIds.Count; i++)
        {
            if (string.Equals(state.disciplineIds[i], disciplineId, StringComparison.Ordinal))
            {
                if (i < state.xpValues.Count) return state.xpValues[i];
                return 0f;
            }
        }
        return 0f;
    }

    private static void SetXp(SkillProgressionState state, string disciplineId, float value)
    {
        if (state == null || string.IsNullOrEmpty(disciplineId)) return;
        if (state.disciplineIds == null) state.disciplineIds = new List<string>();
        if (state.xpValues == null) state.xpValues = new List<float>();
        for (int i = 0; i < state.disciplineIds.Count; i++)
        {
            if (!string.Equals(state.disciplineIds[i], disciplineId, StringComparison.Ordinal))
                continue;
            if (i < state.xpValues.Count) state.xpValues[i] = value; else state.xpValues.Add(value);
            return;
        }
        state.disciplineIds.Add(disciplineId);
        state.xpValues.Add(value);
    }

    private static int GetLastUsedDay(SkillProgressionState state, string disciplineId)
    {
        if (state == null || state.disciplineIds == null || state.lastUsedDays == null)
            return 0;
        for (int i = 0; i < state.disciplineIds.Count; i++)
        {
            if (string.Equals(state.disciplineIds[i], disciplineId, StringComparison.Ordinal))
            {
                if (i < state.lastUsedDays.Count) return state.lastUsedDays[i];
                return 0;
            }
        }
        return 0;
    }

    private static void SetLastUsedDay(SkillProgressionState state, string disciplineId, int day)
    {
        if (state == null || string.IsNullOrEmpty(disciplineId)) return;
        if (state.disciplineIds == null) state.disciplineIds = new List<string>();
        if (state.lastUsedDays == null) state.lastUsedDays = new List<int>();
        for (int i = 0; i < state.disciplineIds.Count; i++)
        {
            if (!string.Equals(state.disciplineIds[i], disciplineId, StringComparison.Ordinal))
                continue;
            if (i < state.lastUsedDays.Count) state.lastUsedDays[i] = day; else state.lastUsedDays.Add(day);
            return;
        }
        state.disciplineIds.Add(disciplineId);
        state.lastUsedDays.Add(day);
    }

    private static float Clamp(float value, float min, float max)
        => value < min ? min : (value > max ? max : value);
}
}
