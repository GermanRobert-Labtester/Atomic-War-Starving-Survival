using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Action-driven progression (Prompts #179–#181). Survivors earn hidden XP
    /// by doing work; perks unlock as muscle memory. Expert tracks are limited
    /// to one predetermined discipline per survivor. Perks go Dormant after 14
    /// unused days; desperate stress can trigger an Epiphany of instant mastery.
    /// Plain C#, save/load safe, EditMode-testable.
    /// </summary>
    public class SkillProgressionSystem
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

        private readonly Dictionary<string, ProgressionState> _bySurvivor =
            new Dictionary<string, ProgressionState>();

        private readonly List<PerkSO> _perkCatalog = new List<PerkSO>();
        private readonly Dictionary<string, PerkSO> _perkById =
            new Dictionary<string, PerkSO>();

        private PersonalQuestSystem _personalQuests;

        /// <summary>Prompt #235 — Polymath 3x action perk XP.</summary>
        public void BindPersonalQuests(PersonalQuestSystem personalQuests) =>
            _personalQuests = personalQuests;

        public event Action<Survivor, string, float> OnXpGained;          // sv, discipline, newXp
        public event Action<Survivor, PerkSO> OnPerkEarned;
        public event Action<Survivor, PerkSO> OnPerkDormant;
        public event Action<Survivor, PerkSO> OnPerkReactivated;
        public event Action<Survivor, PerkSO> OnEpiphany;

        public int CatalogCount => _perkCatalog.Count;

        // -----------------------------------------------------------------
        // Catalog
        // -----------------------------------------------------------------

        public void RegisterPerk(PerkSO perk)
        {
            if (perk == null || string.IsNullOrEmpty(perk.id)) return;
            if (_perkById.ContainsKey(perk.id))
            {
                // Replace (idempotent by id)
                for (int i = 0; i < _perkCatalog.Count; i++)
                {
                    if (_perkCatalog[i] != null && _perkCatalog[i].id == perk.id)
                    {
                        _perkCatalog[i] = perk;
                        break;
                    }
                }
            }
            else
            {
                _perkCatalog.Add(perk);
            }
            _perkById[perk.id] = perk;
        }

        public void RegisterDefaultPerks()
        {
            // Runtime-created defaults so EditMode / bootstrap need no assets.
            RegisterPerk(MakeRuntimePerk("perk_field_dressing", "Field Dressing", "medical", 50f, 0.10f, false));
            RegisterPerk(MakeRuntimePerk("perk_steady_hands", "Steady Hands", "medical", 120f, 0.20f, true));
            RegisterPerk(MakeRuntimePerk("perk_rough_repairs", "Rough Repairs", "crafting", 50f, 0.10f, false));
            RegisterPerk(MakeRuntimePerk("perk_workshop_sense", "Workshop Sense", "crafting", 120f, 0.20f, true));
            RegisterPerk(MakeRuntimePerk("perk_signal_ear", "Signal Ear", "science", 50f, 0.10f, false));
            RegisterPerk(MakeRuntimePerk("perk_cold_analysis", "Cold Analysis", "science", 120f, 0.20f, true));
            RegisterPerk(MakeRuntimePerk("perk_watchful", "Watchful", "combat", 50f, 0.10f, false));
            RegisterPerk(MakeRuntimePerk("perk_trail_memory", "Trail Memory", "scavenging", 50f, 0.10f, false));
            RegisterPerk(MakeRuntimePerk("perk_hard_living", "Hard Living", "survival", 50f, 0.10f, false));
            RegisterCombatPerks();
            RegisterSurvivalPerks();
            RegisterShelterPerks();
            RegisterMedicalPerks();
            RegisterExpeditionPerks();
            RegisterSocialPerks();
            RegisterLatentExpertTraits();
        }

        /// <summary>
        /// Milestone combat perks (Prompts #182–#188). xpThreshold is unreachable via
        /// action XP — granted only through <see cref="TryGrantPerk"/>.
        /// </summary>
        public void RegisterCombatPerks()
        {
            const float milestoneOnly = 999999f;
            RegisterPerk(MakeRuntimePerk(CombatPerkSystem.TapRackBangId, "Tap-Rack-Bang", "combat", milestoneOnly, 0.05f, false));
            RegisterPerk(MakeRuntimePerk(CombatPerkSystem.ColdBoreId, "Cold Bore", "combat", milestoneOnly, 0.05f, false));
            RegisterPerk(MakeRuntimePerk(CombatPerkSystem.SuppressingFireId, "Suppressing Fire", "combat", milestoneOnly, 0.05f, false));
            RegisterPerk(MakeRuntimePerk(CombatPerkSystem.CloseQuartersId, "Close Quarters", "combat", milestoneOnly, 0.10f, false));
            RegisterPerk(MakeRuntimePerk(CombatPerkSystem.TrapSetterId, "Trap Setter", "combat", milestoneOnly, 0.05f, false));
            RegisterPerk(MakeRuntimePerk(CombatPerkSystem.LootersReflexId, "Looter's Reflex", "scavenging", milestoneOnly, 0.05f, false));
            RegisterPerk(MakeRuntimePerk(CombatPerkSystem.DesensitizedId, "Desensitized", "combat", milestoneOnly, 0f, false));
        }

        /// <summary>
        /// Milestone survival perks (Prompts #189–#194). xpThreshold is unreachable via
        /// action XP — granted only through <see cref="TryGrantPerk"/>.
        /// </summary>
        public void RegisterSurvivalPerks()
        {
            const float milestoneOnly = 999999f;
            RegisterPerk(MakeRuntimePerk(SurvivalPerkSystem.RationStretcherId, "Ration Stretcher", "survival", milestoneOnly, 0.05f, false));
            RegisterPerk(MakeRuntimePerk(SurvivalPerkSystem.IronStomachId, "Iron Stomach", "survival", milestoneOnly, 0.05f, false));
            RegisterPerk(MakeRuntimePerk(SurvivalPerkSystem.WastelandBrewerId, "Wasteland Brewer", "survival", milestoneOnly, 0.05f, false));
            RegisterPerk(MakeRuntimePerk(SurvivalPerkSystem.ButcherId, "The Butcher", "survival", milestoneOnly, 0.05f, false));
            RegisterPerk(MakeRuntimePerk(SurvivalPerkSystem.PharmacologistId, "Pharmacologist", "medical", milestoneOnly, 0.10f, false));
            RegisterPerk(MakeRuntimePerk(SurvivalPerkSystem.MycologyId, "Mycology", "science", milestoneOnly, 0.05f, false));
        }

        /// <summary>
        /// Milestone shelter-engineering perks (Prompts #195–#200). xpThreshold is
        /// unreachable via action XP — granted only through <see cref="TryGrantPerk"/>.
        /// </summary>
        public void RegisterShelterPerks()
        {
            const float milestoneOnly = 999999f;
            RegisterPerk(MakeRuntimePerk(ShelterPerkSystem.JuryRiggerId, "Jury-Rigger", "crafting", milestoneOnly, 0.05f, false));
            RegisterPerk(MakeRuntimePerk(ShelterPerkSystem.StructuralEngineerId, "Structural Engineer", "crafting", milestoneOnly, 0.10f, false));
            RegisterPerk(MakeRuntimePerk(ShelterPerkSystem.HvacTechId, "HVAC Technician", "crafting", milestoneOnly, 0.05f, false));
            RegisterPerk(MakeRuntimePerk(ShelterPerkSystem.ScrapperId, "Scrapper", "crafting", milestoneOnly, 0.05f, false));
            RegisterPerk(MakeRuntimePerk(ShelterPerkSystem.SandhogId, "The Sandhog", "crafting", milestoneOnly, 0.05f, false));
            RegisterPerk(MakeRuntimePerk(ShelterPerkSystem.ThermodynamicsId, "Thermodynamics", "science", milestoneOnly, 0.05f, false));
        }

        /// <summary>
        /// Milestone medical perks (Prompts #201–#205). xpThreshold is unreachable via
        /// action XP — granted only through <see cref="TryGrantPerk"/>.
        /// Replaces the old XP-threshold Steady Hands entry with the milestone version.
        /// </summary>
        public void RegisterMedicalPerks()
        {
            const float milestoneOnly = 999999f;
            // Supersedes the XP-threshold perk_steady_hands registered in RegisterDefaultPerks.
            // Milestone awards are not expert-track gated (any medic can earn via Phase-2 cures).
            RegisterPerk(MakeRuntimePerk(MedicalPerkSystem.SteadyHandsId, "Steady Hands", "medical", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(MedicalPerkSystem.TriageUnderFireId, "Triage Under Fire", "medical", milestoneOnly, 0.10f, false));
            RegisterPerk(MakeRuntimePerk(MedicalPerkSystem.RadiologistId, "Radiologist", "medical", milestoneOnly, 0.05f, false));
            RegisterPerk(MakeRuntimePerk(MedicalPerkSystem.AnatomistId, "Anatomist", "medical", milestoneOnly, 0.10f, false));
            RegisterPerk(MakeRuntimePerk(MedicalPerkSystem.ParamedicId, "Paramedic", "medical", milestoneOnly, 0.10f, false));
        }

        /// <summary>
        /// Milestone expedition / wasteland perks (Prompts #206–#210). xpThreshold is
        /// unreachable via action XP — granted only through <see cref="TryGrantPerk"/>.
        /// </summary>
        public void RegisterExpeditionPerks()
        {
            const float milestoneOnly = 999999f;
            RegisterPerk(MakeRuntimePerk(ExpeditionPerkSystem.PackMuleId, "Pack Mule", "scavenging", milestoneOnly, 0.05f, false));
            RegisterPerk(MakeRuntimePerk(ExpeditionPerkSystem.LightStepId, "Light Step", "scavenging", milestoneOnly, 0.05f, false));
            RegisterPerk(MakeRuntimePerk(ExpeditionPerkSystem.UrbanPathfinderId, "Urban Pathfinder", "scavenging", milestoneOnly, 0.05f, false));
            RegisterPerk(MakeRuntimePerk(ExpeditionPerkSystem.NightTerrorId, "Night Terror", "combat", milestoneOnly, 0.10f, false));
            RegisterPerk(MakeRuntimePerk(ExpeditionPerkSystem.ForagerId, "Forager", "survival", milestoneOnly, 0.05f, false));
        }

        /// <summary>
        /// Milestone social / leadership perks (Prompts #211–#213). xpThreshold is
        /// unreachable via action XP — granted only through <see cref="TryGrantPerk"/>.
        /// </summary>
        public void RegisterSocialPerks()
        {
            const float milestoneOnly = 999999f;
            RegisterPerk(MakeRuntimePerk(SocialPerkSystem.DeEscalatorId, "De-Escalator", "survival", milestoneOnly, 0.05f, false));
            RegisterPerk(MakeRuntimePerk(SocialPerkSystem.QuartermasterId, "Quartermaster", "scavenging", milestoneOnly, 0.05f, false));
            RegisterPerk(MakeRuntimePerk(SocialPerkSystem.TaskmasterId, "Taskmaster", "survival", milestoneOnly, 0.10f, false));
        }

        /// <summary>
        /// Latent expert traits (Prompts #214–#248). Unreachable via action XP —
        /// granted only when a personal questline completes.
        /// Marked isExpertPerk so only the predetermined expert track may hold them
        /// (PersonalQuestSystem also grants via TryGrantPerk after quest finish).
        /// </summary>
        public void RegisterLatentExpertTraits()
        {
            const float milestoneOnly = 999999f;
            // Expert-flag false: quest grant must work for any assigned survivor
            // without requiring ExpertDisciplineId match mid-quest.
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.MiracleWorkerId, "Miracle Worker", "medical", milestoneOnly, 0.25f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.AlchemistId, "Alchemist", "medical", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.ZoonoticExpertId, "Zoonotic Expert", "medical", milestoneOnly, 0.15f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.AnchorId, "Anchor", "survival", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.DeathBlindId, "Death-Blind", "survival", milestoneOnly, 0.15f, false));
            // Prompts #220–#224
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.WarlordId, "Warlord", "combat", milestoneOnly, 0.25f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.PeacekeeperId, "Peacekeeper", "combat", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.JuggernautId, "Juggernaut", "combat", milestoneOnly, 0.25f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.ApexPredatorId, "Apex Predator", "scavenging", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.SurvivalistId, "Survivalist", "survival", milestoneOnly, 0.20f, false));
            // Prompts #225–#234
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.HydraulicMasterId, "Hydraulic Master", "survival", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.GridWalkerId, "Grid Walker", "survival", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.VaultBuilderId, "Vault Builder", "survival", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.GreaseMonkeyId, "Grease Monkey", "scavenging", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.SynthesizerId, "Synthesizer", "medical", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.GaiaId, "Gaia", "survival", milestoneOnly, 0.25f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.WastelandRunnerId, "Wasteland Runner", "scavenging", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.GhostId, "Ghost", "scavenging", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.StormcallerId, "Stormcaller", "survival", milestoneOnly, 0.15f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.RadWalkerId, "Rad-Walker", "survival", milestoneOnly, 0.25f, false));
            // Prompts #235–#248
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.PolymathId, "Polymath", "social", milestoneOnly, 0.25f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.DemagogueId, "Demagogue", "social", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.ShepherdId, "Shepherd", "social", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.MuckrakerId, "Muckraker", "scavenging", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.VoiceOfTheWastesId, "Voice of the Wastes", "social", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.IronChefId, "Iron Chef", "survival", milestoneOnly, 0.25f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.TirelessId, "Tireless", "combat", milestoneOnly, 0.25f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.AsbestosId, "Asbestos", "combat", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.ArmorerId, "Armorer", "scavenging", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.TinkererId, "Tinkerer", "survival", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.LorekeeperId, "Lorekeeper", "social", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.ZealotsBaneId, "Zealot's Bane", "combat", milestoneOnly, 0.25f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.ChemResistantId, "Chem-Resistant", "medical", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.ProtectorId, "Protector", "social", milestoneOnly, 0.25f, false));
            // Prompts #249–#256
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.MatriarchId, "Matriarch", "social", milestoneOnly, 0.25f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.PillarOfAtlasId, "Pillar of Atlas", "survival", milestoneOnly, 0.25f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.WastelandScoutId, "Wasteland Scout", "scavenging", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.ChildOfTheAshId, "Child of the Ash", "combat", milestoneOnly, 0.25f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.ColdCalculusId, "Cold Calculus", "survival", milestoneOnly, 0.25f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.ButcherOfDay30Id, "Butcher of Day 30", "combat", milestoneOnly, 0.25f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.MasterManipulatorId, "Master Manipulator", "social", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.DragonsHoardId, "Dragon's Hoard", "scavenging", milestoneOnly, 0.20f, false));
            // Prompts #257–#266
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.ArtOfWarId, "The Art of War", "combat", milestoneOnly, 0.25f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.DemolitionsExpertId, "Demolitions Expert", "combat", milestoneOnly, 0.25f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.GhostShooterId, "Ghost Shooter", "combat", milestoneOnly, 0.25f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.SupplyChainMasterId, "Supply Chain Master", "scavenging", milestoneOnly, 0.20f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.ReclaimedYouthId, "Reclaimed Youth", "social", milestoneOnly, 0.25f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.SoulWeaverId, "Soul Weaver", "social", milestoneOnly, 0.25f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.LoneWolfId, "Lone Wolf", "survival", milestoneOnly, 0.25f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.GroundedOptimistId, "Grounded Optimist", "social", milestoneOnly, 0.25f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.LivingSaintId, "Living Saint", "social", milestoneOnly, 0.25f, false));
            RegisterPerk(MakeRuntimePerk(PersonalQuestSystem.HumbledHealerId, "Humbled Healer", "medical", milestoneOnly, 0.25f, false));
        }

        /// <summary>
        /// Grant a catalog perk by id without XP (milestone / narrative awards).
        /// Returns true if newly granted.
        /// </summary>
        public bool TryGrantPerk(Survivor survivor, string perkId, int currentDay = 0)
        {
            if (survivor == null || !survivor.IsAlive || string.IsNullOrEmpty(perkId)) return false;
            var perk = GetPerk(perkId);
            if (perk == null) return false;
            var state = GetOrCreate(survivor.Id);
            if (!CanEarnPerk(survivor, state, perk)) return false;
            GrantPerk(survivor, state, perk);
            if (!string.IsNullOrEmpty(perk.disciplineId))
                state.LastUsedDay[perk.disciplineId] = currentDay;
            return true;
        }

        private static PerkSO MakeRuntimePerk(
            string id, string display, string discipline, float xp, float bonus, bool expert)
        {
            var p = ScriptableObject.CreateInstance<PerkSO>();
            p.id = id;
            p.displayName = display;
            p.description = display;
            p.disciplineId = discipline;
            p.xpThreshold = xp;
            p.skillBonus = bonus;
            p.isExpertPerk = expert;
            return p;
        }

        public PerkSO GetPerk(string perkId)
        {
            if (string.IsNullOrEmpty(perkId)) return null;
            return _perkById.TryGetValue(perkId, out var p) ? p : null;
        }

        // -----------------------------------------------------------------
        // Action recording (Prompt #179)
        // -----------------------------------------------------------------

        /// <summary>
        /// Record that a survivor performed work in a discipline. Awards hidden
        /// XP, reactivates dormant perks, may grant new perks, may fire Epiphany.
        /// </summary>
        public void RecordAction(
            Survivor survivor,
            string disciplineId,
            float xpAmount,
            int currentDay,
            System.Random rng = null)
        {
            if (survivor == null || !survivor.IsAlive) return;
            if (string.IsNullOrEmpty(disciplineId) || xpAmount <= 0f) return;

            var state = GetOrCreate(survivor.Id);
            state.LastUsedDay[disciplineId] = currentDay;

            // Reactivate dormant perks for this discipline (Prompt #180).
            ReactivateDormantForDiscipline(survivor, state, disciplineId);

            // Stress Epiphany (Prompt #181) — may instantly master before XP add.
            if (TryStressEpiphany(survivor, state, disciplineId, rng))
                return;

            // Prompt #235 — Polymath learns action-driven perks 3x faster.
            float mult = _personalQuests != null
                ? _personalQuests.GetActionPerkXpMultiplier(survivor)
                : 1f;
            float awarded = xpAmount * mult;

            float prev = state.Xp.TryGetValue(disciplineId, out float existing) ? existing : 0f;
            float next = prev + awarded;
            state.Xp[disciplineId] = next;
            OnXpGained?.Invoke(survivor, disciplineId, next);

            TryAwardPerks(survivor, state, disciplineId);
            SyncSkillBonuses(survivor, state);
        }

        /// <summary>Convenience: resolve discipline/xp from action metadata fields.</summary>
        public void RecordActionFromMetadata(
            Survivor survivor,
            string disciplineId,
            float xpAmount,
            int currentDay,
            System.Random rng = null)
        {
            RecordAction(survivor, disciplineId, xpAmount, currentDay, rng);
        }

        public float GetXp(string survivorId, string disciplineId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var state)) return 0f;
            return state.Xp.TryGetValue(disciplineId, out float xp) ? xp : 0f;
        }

        public bool HasActivePerk(string survivorId, string perkId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var state)) return false;
            return state.ActivePerkIds.Contains(perkId);
        }

        public bool HasDormantPerk(string survivorId, string perkId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var state)) return false;
            return state.DormantPerkIds.Contains(perkId);
        }

        public bool HasEarnedExpertPerk(string survivorId)
        {
            return _bySurvivor.TryGetValue(survivorId, out var state) && state.ExpertPerkEarned;
        }

        public IReadOnlyList<string> GetActivePerkIds(string survivorId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var state))
                return Array.Empty<string>();
            return state.ActivePerkIds;
        }

        // -----------------------------------------------------------------
        // Decay (Prompt #180)
        // -----------------------------------------------------------------

        /// <summary>
        /// Once per game-day: any active perk whose discipline has not been
        /// practiced for <see cref="DormantAfterUnusedDays"/> becomes Dormant
        /// and loses mechanical benefit until practiced again.
        /// </summary>
        public void TickDaily(int currentDay, IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                if (!_bySurvivor.TryGetValue(sv.Id, out var state)) continue;
                ApplyDecay(sv, state, currentDay);
            }
        }

        private void ApplyDecay(Survivor sv, ProgressionState state, int currentDay)
        {
            // Snapshot active list — we may move items to dormant.
            var active = state.ActivePerkIds;
            for (int i = active.Count - 1; i >= 0; i--)
            {
                var perk = GetPerk(active[i]);
                if (perk == null || string.IsNullOrEmpty(perk.disciplineId)) continue;

                int lastUsed = 0;
                state.LastUsedDay.TryGetValue(perk.disciplineId, out lastUsed);
                if (lastUsed <= 0) lastUsed = currentDay; // never practiced since grant — start clock today

                if (currentDay - lastUsed < DormantAfterUnusedDays) continue;

                string perkId = active[i];
                active.RemoveAt(i);
                if (!state.DormantPerkIds.Contains(perkId))
                    state.DormantPerkIds.Add(perkId);
                OnPerkDormant?.Invoke(sv, perk);
            }
            SyncSkillBonuses(sv, state);
        }

        private void ReactivateDormantForDiscipline(Survivor sv, ProgressionState state, string disciplineId)
        {
            for (int i = state.DormantPerkIds.Count - 1; i >= 0; i--)
            {
                var perk = GetPerk(state.DormantPerkIds[i]);
                if (perk == null) continue;
                if (!string.Equals(perk.disciplineId, disciplineId, StringComparison.Ordinal))
                    continue;

                string perkId = state.DormantPerkIds[i];
                state.DormantPerkIds.RemoveAt(i);
                if (!state.ActivePerkIds.Contains(perkId))
                    state.ActivePerkIds.Add(perkId);
                OnPerkReactivated?.Invoke(sv, perk);
            }
        }

        // -----------------------------------------------------------------
        // Epiphany (Prompt #181)
        // -----------------------------------------------------------------

        private bool TryStressEpiphany(
            Survivor survivor, ProgressionState state, string disciplineId, System.Random rng)
        {
            if (survivor.Needs == null) return false;
            bool desperate = survivor.Needs.Morale < EpiphanyMoraleThreshold
                             || survivor.Needs.Health < EpiphanyHealthThreshold;
            if (!desperate) return false;

            rng ??= new System.Random();
            if (rng.NextDouble() >= EpiphanyChance) return false;

            // Instant mastery: max XP for discipline + grant all eligible perks.
            float maxThreshold = 0f;
            for (int i = 0; i < _perkCatalog.Count; i++)
            {
                var p = _perkCatalog[i];
                if (p == null || !string.Equals(p.disciplineId, disciplineId, StringComparison.Ordinal))
                    continue;
                if (p.xpThreshold > maxThreshold) maxThreshold = p.xpThreshold;
            }
            if (maxThreshold <= 0f) maxThreshold = 100f;

            state.Xp[disciplineId] = maxThreshold;
            TryAwardPerks(survivor, state, disciplineId);
            // Force-grant expert if this is their track and not yet earned.
            ForceGrantBestPerk(survivor, state, disciplineId);

            survivor.Needs.Morale = Mathf.Clamp(EpiphanyMoraleRestore, 0f, 100f);
            SyncSkillBonuses(survivor, state);

            // Fire with the highest-threshold active perk for this discipline, if any.
            PerkSO highlight = null;
            for (int i = 0; i < state.ActivePerkIds.Count; i++)
            {
                var p = GetPerk(state.ActivePerkIds[i]);
                if (p == null || !string.Equals(p.disciplineId, disciplineId, StringComparison.Ordinal))
                    continue;
                if (highlight == null || p.xpThreshold > highlight.xpThreshold)
                    highlight = p;
            }
            if (highlight != null)
                OnEpiphany?.Invoke(survivor, highlight);
            else
                OnEpiphany?.Invoke(survivor, null);

            return true;
        }

        private void ForceGrantBestPerk(Survivor sv, ProgressionState state, string disciplineId)
        {
            PerkSO best = null;
            for (int i = 0; i < _perkCatalog.Count; i++)
            {
                var p = _perkCatalog[i];
                if (p == null || !string.Equals(p.disciplineId, disciplineId, StringComparison.Ordinal))
                    continue;
                if (!CanEarnPerk(sv, state, p)) continue;
                if (best == null || p.xpThreshold > best.xpThreshold) best = p;
            }
            if (best == null) return;
            GrantPerk(sv, state, best);
        }

        // -----------------------------------------------------------------
        // Perk award helpers
        // -----------------------------------------------------------------

        private void TryAwardPerks(Survivor sv, ProgressionState state, string disciplineId)
        {
            float xp = state.Xp.TryGetValue(disciplineId, out float v) ? v : 0f;
            for (int i = 0; i < _perkCatalog.Count; i++)
            {
                var perk = _perkCatalog[i];
                if (perk == null) continue;
                if (!string.Equals(perk.disciplineId, disciplineId, StringComparison.Ordinal))
                    continue;
                if (xp < perk.xpThreshold) continue;
                if (!CanEarnPerk(sv, state, perk)) continue;
                GrantPerk(sv, state, perk);
            }
        }

        private bool CanEarnPerk(Survivor sv, ProgressionState state, PerkSO perk)
        {
            if (perk == null || string.IsNullOrEmpty(perk.id)) return false;
            if (state.ActivePerkIds.Contains(perk.id) || state.DormantPerkIds.Contains(perk.id))
                return false;

            if (perk.isExpertPerk)
            {
                // Only one expert perk per survivor, and only for their predetermined track.
                if (state.ExpertPerkEarned) return false;
                if (string.IsNullOrEmpty(sv.ExpertDisciplineId)) return false;
                if (!string.Equals(sv.ExpertDisciplineId, perk.disciplineId, StringComparison.Ordinal))
                    return false;
            }
            return true;
        }

        private void GrantPerk(Survivor sv, ProgressionState state, PerkSO perk)
        {
            if (perk == null) return;
            if (state.ActivePerkIds.Contains(perk.id) || state.DormantPerkIds.Contains(perk.id))
                return;

            state.ActivePerkIds.Add(perk.id);
            if (perk.isExpertPerk)
                state.ExpertPerkEarned = true;

            OnPerkEarned?.Invoke(sv, perk);
            SyncSkillBonuses(sv, state);
        }

        /// <summary>
        /// Write mechanical bonuses onto the survivor so Effective*Skill reads them.
        /// Only Active (non-dormant) perks contribute.
        /// </summary>
        public void SyncSkillBonuses(Survivor sv, ProgressionState state = null)
        {
            if (sv == null) return;
            if (state == null)
                _bySurvivor.TryGetValue(sv.Id, out state);

            float med = 0f, craft = 0f, sci = 0f, combat = 0f, scav = 0f, surv = 0f;
            if (state != null)
            {
                for (int i = 0; i < state.ActivePerkIds.Count; i++)
                {
                    var p = GetPerk(state.ActivePerkIds[i]);
                    if (p == null) continue;
                    switch (p.disciplineId)
                    {
                        case "medical": med += p.skillBonus; break;
                        case "crafting": craft += p.skillBonus; break;
                        case "science": sci += p.skillBonus; break;
                        case "combat": combat += p.skillBonus; break;
                        case "scavenging": scav += p.skillBonus; break;
                        case "survival": surv += p.skillBonus; break;
                    }
                }
            }
            sv.ProgressionMedicalBonus = med;
            sv.ProgressionCraftingBonus = craft;
            sv.ProgressionScienceBonus = sci;
            sv.ProgressionCombatBonus = combat;
            sv.ProgressionScavengingBonus = scav;
            sv.ProgressionSurvivalBonus = surv;
        }

        private ProgressionState GetOrCreate(string survivorId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var state))
            {
                state = new ProgressionState();
                _bySurvivor[survivorId] = state;
            }
            return state;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public SkillProgressionSave CaptureState()
        {
            var save = new SkillProgressionSave
            {
                Entries = new List<SurvivorProgressionSave>()
            };
            foreach (var kv in _bySurvivor)
            {
                var st = kv.Value;
                var entry = new SurvivorProgressionSave
                {
                    SurvivorId = kv.Key,
                    ExpertPerkEarned = st.ExpertPerkEarned,
                    ActivePerkIds = new List<string>(st.ActivePerkIds),
                    DormantPerkIds = new List<string>(st.DormantPerkIds),
                    DisciplineIds = new List<string>(),
                    XpValues = new List<float>(),
                    LastUsedDays = new List<int>()
                };
                foreach (var xp in st.Xp)
                {
                    entry.DisciplineIds.Add(xp.Key);
                    entry.XpValues.Add(xp.Value);
                    entry.LastUsedDays.Add(
                        st.LastUsedDay.TryGetValue(xp.Key, out int d) ? d : 0);
                }
                // Also persist last-used for disciplines with 0 XP (after grant-only paths).
                foreach (var lu in st.LastUsedDay)
                {
                    if (entry.DisciplineIds.Contains(lu.Key)) continue;
                    entry.DisciplineIds.Add(lu.Key);
                    entry.XpValues.Add(0f);
                    entry.LastUsedDays.Add(lu.Value);
                }
                save.Entries.Add(entry);
            }
            return save;
        }

        public void RestoreState(SkillProgressionSave save, IReadOnlyList<Survivor> survivors = null)
        {
            _bySurvivor.Clear();
            if (save?.Entries == null) return;

            for (int i = 0; i < save.Entries.Count; i++)
            {
                var e = save.Entries[i];
                if (e == null || string.IsNullOrEmpty(e.SurvivorId)) continue;
                var st = new ProgressionState
                {
                    ExpertPerkEarned = e.ExpertPerkEarned
                };
                if (e.ActivePerkIds != null)
                    st.ActivePerkIds.AddRange(e.ActivePerkIds);
                if (e.DormantPerkIds != null)
                    st.DormantPerkIds.AddRange(e.DormantPerkIds);
                if (e.DisciplineIds != null)
                {
                    for (int d = 0; d < e.DisciplineIds.Count; d++)
                    {
                        string disc = e.DisciplineIds[d];
                        if (string.IsNullOrEmpty(disc)) continue;
                        float xp = e.XpValues != null && d < e.XpValues.Count ? e.XpValues[d] : 0f;
                        int day = e.LastUsedDays != null && d < e.LastUsedDays.Count ? e.LastUsedDays[d] : 0;
                        st.Xp[disc] = xp;
                        st.LastUsedDay[disc] = day;
                    }
                }
                _bySurvivor[e.SurvivorId] = st;
            }

            if (survivors == null) return;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null) continue;
                if (_bySurvivor.TryGetValue(sv.Id, out var st))
                    SyncSkillBonuses(sv, st);
            }
        }

        /// <summary>Per-survivor runtime progression bookkeeping.</summary>
        public sealed class ProgressionState
        {
            public readonly Dictionary<string, float> Xp = new Dictionary<string, float>();
            public readonly Dictionary<string, int> LastUsedDay = new Dictionary<string, int>();
            public readonly List<string> ActivePerkIds = new List<string>();
            public readonly List<string> DormantPerkIds = new List<string>();
            public bool ExpertPerkEarned;
        }
    }

    [Serializable]
    public class SkillProgressionSave
    {
        public List<SurvivorProgressionSave> Entries = new List<SurvivorProgressionSave>();
    }

    [Serializable]
    public class SurvivorProgressionSave
    {
        public string SurvivorId;
        public bool ExpertPerkEarned;
        public List<string> ActivePerkIds = new List<string>();
        public List<string> DormantPerkIds = new List<string>();
        public List<string> DisciplineIds = new List<string>();
        public List<float> XpValues = new List<float>();
        public List<int> LastUsedDays = new List<int>();
    }
}
