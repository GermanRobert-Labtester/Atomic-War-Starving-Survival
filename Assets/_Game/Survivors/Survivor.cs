using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Coarse activity state driving need decay rates and AI availability.
    /// </summary>
    public enum SurvivorState
    {
        Idle,
        Working,
        Resting,
        Sick,
        Incapacitated,
        Dead
    }

    // -------------------------------------------------------------------
    // Expansion enums — Phase 0 foundation for 40-system expansion.
    // -------------------------------------------------------------------

    /// <summary>
    /// Explicit radiation sickness phase tracking. Replaces the binary
    /// HasAcuteRadiationSickness with a full clinical progression model.
    /// Owned by AtomicWar._Game.Radiation.RadiationPhaseProgression.
    /// </summary>
    public enum RadiationSicknessPhase
    {
        Healthy,
        Prodromal,
        Latent,
        ManifestIllness,
        ChronicFibrosis,
        RecoveryOrDeath
    }

    /// <summary>
    /// Direction of moral branching after accumulating 5+ moral choices.
    /// Owned by AtomicWar._Game.Survivors.MoralBranchingSystem.
    /// </summary>
    public enum MoralBranchDirection
    {
        Neutral,
        NumbedResilience,
        BurdenedCompassion
    }

    /// <summary>
    /// Kinds of chemical dependency beyond the base AddictionSystem.
    /// Owned by AtomicWar._Game.Medical.ChemicalDependencySystem.
    /// </summary>
    public enum ChemicalDependencyKind
    {
        Opioid,
        Alcohol,
        Stimulant,
        Sedative
    }

    /// <summary>
    /// Philosophical stance adopted by a survivor. Dictates which emergency
    /// decisions trigger morale recovery versus severe distress.
    /// Owned by BeliefSystem (extended for Antigravity #49).
    /// </summary>
    public enum PhilosophicalStance
    {
        Undecided,
        Stoicism,
        Faith,
        MaterialRationalism
    }

    /// <summary>
    /// Runtime model for a single survivor: identity, activity state, and current
    /// need values. Save/load safe (primitives only); behaviour lives in
    /// NeedsSystem, RadiationSystem, and the Utility AI.
    /// </summary>
    [System.Serializable]
    public class Survivor
    {
        public string Id;
        public string DisplayName;
        public int Age = 30;
        public bool IsBunkerBorn;

        // Plain field, not an auto-property: JsonUtility does not serialize
        // properties, so a { get; set; } here would silently fail to save/load.
        public SurvivorState State = SurvivorState.Idle;

        public bool IsAlive => State != SurvivorState.Dead;

        public Needs Needs { get; } = new Needs();

        // Owned and written by AtomicWar._Game.Radiation.RadiationSystem.
        // RadiationDose is the current, clamped 0..100 reading; LifetimeRadiationExposure
        // is unclamped and only ever grows, driving Chronic Illness.
        public float RadiationDose;
        public float LifetimeRadiationExposure;

        public bool HasAcuteRadiationSickness;
        public bool HasChronicIllness;
        public bool HasFullSuitEquipped;

        // Temporary rad resistance (e.g. from iodine pills): timed, owned and written
        // by AtomicWar._Game.Radiation.RadiationSystem.
        public bool HasRadResistance;
        public float RadResistanceHoursRemaining;

        // -------------------------------------------------------------------
        // Latent damage / prognosis pipeline — the delayed "fallout kills you
        // later" layer on top of the instant checks above. Owned and written by
        // AtomicWar._Game.Radiation.PrognosisPipeline (invoked from RadiationSystem).
        // -------------------------------------------------------------------

        /// <summary>Rolling sum of recent dose (decays over time) — the acute trigger quantity.</summary>
        public float AcuteDoseWindow;
        public PrognosisStage PrognosisStage = PrognosisStage.Healthy;
        /// <summary>Days until the next prognosis stage transition. Hidden from the player by
        /// default; revealed only via RadiationSystem.ExaminePrognosis (a medical exam).</summary>
        public float OnsetTimer;
        /// <summary>Permanent accumulated tissue damage. Never decays; feeds both the acute
        /// pathway's severity curves and the chronic-illness threshold.</summary>
        public float LatentDamage;
        /// <summary>Hours remaining in the iodine protection window (see AdministerIodine).</summary>
        public float IodineProtectionTimer;
        public bool HasAcuteRadiationSyndrome;

        // -------------------------------------------------------------------
        // Photoperiod / light state — owned and written by PhotoperiodSystem.
        // -------------------------------------------------------------------

        /// <summary>
        /// Recent-light index (0..100). Accumulates during effective daylight,
        /// drains in darkness.  When it falls to LightProfile.listlessThreshold
        /// the survivor becomes Listless.
        /// </summary>
        public float LightExposure = 100f;

        /// <summary>
        /// Hidden status: true when LightExposure has been below the threshold long
        /// enough to trigger seasonal-affective / cabin-fever effects.  Does NOT
        /// appear as a visible need bar; manifests as morale drain + AI score penalty.
        /// </summary>
        public bool IsListless;

        /// <summary>
        /// Vitamin D proxy (0..100, hidden). Accumulates slowly in useful light;
        /// decays in prolonged darkness.  When low it silently penalises health and
        /// morale. Offset by consuming vitaminD-tagged food items (fish, eggs, etc.).
        /// </summary>
        public float VitaminDProxy = 100f;

        // -------------------------------------------------------------------
        // Belief / risk-perception — subjective danger sense, distinct from the
        // objective radiation state above. Owned and written by
        // AtomicWar._Game.Survivors.BeliefSystem. Two survivors with identical
        // RadiationDose can have wildly different PerceivedRadRisk.
        // -------------------------------------------------------------------

        /// <summary>Characteristic bias in how this survivor interprets radiation risk.</summary>
        public RiskBiasTrait RiskBias = RiskBiasTrait.Realist;

        /// <summary>Subjective sense of radiation danger (0..1). Updated by observed
        /// experience (sickness witnessed, hot trips survived) and trait, NOT by truth.</summary>
        public float PerceivedRadRisk = 0.3f;

        /// <summary>How much this survivor trusts the geiger/dosimeter vs their own gut (0..1).</summary>
        public float TrustInInstruments = 0.7f;

        /// <summary>Mental status (0..1): rises when PerceivedRadRisk and instrument
        /// uncertainty are both high. Causes refusal-to-scavenge, hoarding, sleep loss.</summary>
        public float RadiationAnxiety;

        /// <summary>Mental status (0..1): the opposite failure mode of RadiationAnxiety —
        /// stops caring, takes lethal risks.</summary>
        public float Numbness;

        /// <summary>True once RadiationAnxiety has crossed BeliefSystem.AnxietyThreshold.</summary>
        public bool HasRadiationAnxietyStatus;

        /// <summary>True once Numbness has crossed BeliefSystem.NumbnessThreshold.</summary>
        public bool IsNumb;

        /// <summary>
        /// Permanent Fractured status after a forgiven internal theft (or bunker-wide scar).
        /// See <see cref="SurvivorStatus.Fractured"/>.
        /// </summary>
        public bool IsFractured;

        // Prompt #165 — clothing degradation
        public float ClothingDurability = 100f;
        public bool IsRagged;

        /// <summary>
        /// Black Rain dread (Prompt #11). True while outdoors in BlackRain or
        /// listening to it hit the hatch. Owned by BlackRainHazardSystem.
        /// </summary>
        public bool HasDread;

        // -------------------------------------------------------------------
        // Medical triage — skill for Treat Patient actions. Affliction instances
        // live in MedicalSystem (keyed by survivor id); this is the only skill
        // field needed on the survivor for treatment speed / resource sparing.
        // -------------------------------------------------------------------

        /// <summary>
        /// 0..1 medical aptitude base. Varied per character at creation; not a final
        /// "level". Action-driven perks (SkillProgressionSystem) stack on top.
        /// </summary>
        public float MedicalSkill = 0.3f;

        /// <summary>
        /// 0..1 scientific aptitude base. Varied per character. Used by
        /// EventContext.HasTraitInBunker("Science") and cartography speed.
        /// </summary>
        public float ScienceSkill = 0.3f;

        /// <summary>
        /// 0..1 crafting aptitude base. Varied per character. Subject to Skill
        /// Atrophy (Prompt #10) and action-driven perks (Prompt #179).
        /// </summary>
        public float CraftingSkill = 0.3f;

        /// <summary>
        /// Predetermined expert discipline this survivor may master (one only).
        /// snake_case: medical / crafting / science / combat / scavenging / survival.
        /// Empty = no expert track. Owned by SkillProgressionSystem (Prompt #179).
        /// </summary>
        public string ExpertDisciplineId;

        // -------------------------------------------------------------------
        // Personal Quest Engine & Latent Expert Traits (Prompt #214).
        // Survivors do NOT start with their expert trait — they earn it via
        // activeQuestline completion. Owned by PersonalQuestSystem.
        // -------------------------------------------------------------------

        /// <summary>snake_case archetype id (e.g. the_surgeon).</summary>
        public string ArchetypeId;

        /// <summary>
        /// Predetermined latent expert trait perk id (e.g. trait_miracle_worker).
        /// Stored at creation; granted only when the personal questline finishes.
        /// </summary>
        public string LatentExpertTraitId;

        /// <summary>Active personal questline id (QuestlineSO.id), or empty.</summary>
        public string ActiveQuestlineId;

        /// <summary>True once the questline has been triggered (day 30 or morale recovery).</summary>
        public bool QuestlineActive;

        /// <summary>True once the latent expert trait has been permanently unlocked.</summary>
        public bool LatentTraitUnlocked;

        /// <summary>Quest stage index (0..max). Owned by PersonalQuestSystem.</summary>
        public int QuestStage;

        /// <summary>Generic quest progress accumulator (ops, hours, etc.).</summary>
        public float QuestProgress;

        /// <summary>Campaign days this survivor has been alive in the bunker.</summary>
        public int DaysAlive;

        /// <summary>True after morale has hit 0 (arm for 0→100 quest trigger).</summary>
        public bool MoraleHitZero;

        /// <summary>Active (non-dormant) perk bonus for medical. Written by SkillProgressionSystem.</summary>
        public float ProgressionMedicalBonus;
        /// <summary>Active perk bonus for crafting.</summary>
        public float ProgressionCraftingBonus;
        /// <summary>Active perk bonus for science.</summary>
        public float ProgressionScienceBonus;
        /// <summary>Active perk bonus for combat/guard work.</summary>
        public float ProgressionCombatBonus;
        /// <summary>Active perk bonus for scavenging.</summary>
        public float ProgressionScavengingBonus;
        /// <summary>Active perk bonus for survival chores.</summary>
        public float ProgressionSurvivalBonus;

        // -------------------------------------------------------------------
        // Skill Atrophy (Prompt #10). When Morale stays below the atrophy
        // threshold for the configured window, MedicalSkill and CraftingSkill
        // permanently degrade. Owned by SkillAtrophySystem.
        // -------------------------------------------------------------------

        /// <summary>Consecutive days with morale below the atrophy threshold.
        /// Reset when morale climbs above it. Owned by SkillAtrophySystem.</summary>
        public float ConsecutiveLowMoraleDays;

        /// <summary>Set of skill names that have already atrophied (e.g. "medical", "crafting").
        /// Prevents double-atrophy. Owned by SkillAtrophySystem.</summary>
        public System.Collections.Generic.List<string> AtrophiedSkills = new System.Collections.Generic.List<string>();

        // -------------------------------------------------------------------
        // Addiction & Withdrawal (Prompt #7). Owned by AddictionSystem.
        // -------------------------------------------------------------------

        /// <summary>Rolling 7-day consumption log of addictive items (morphine, anti-rads).
        /// Owned by AddictionSystem.</summary>
        public System.Collections.Generic.List<ConsumptionRecord> ConsumptionHistory = new System.Collections.Generic.List<ConsumptionRecord>();

        /// <summary>Game-hours since last dose of an addictive chem while Addicted.
        /// When this exceeds the withdrawal threshold, WithdrawalSickness begins.</summary>
        public float HoursSinceLastDose;

        /// <summary>True when the survivor is in active withdrawal (shaking, panicking,
        /// AI override to SearchForChems). Owned by AddictionSystem.</summary>
        public bool IsInWithdrawal;

        // -------------------------------------------------------------------
        // Dependent / Child mechanic (Prompt #9).
        // -------------------------------------------------------------------

        /// <summary>True for survivors who cannot scavenge, craft, or fight (e.g. children).
        /// The AI skips actions that require these flags.</summary>
        public bool CannotScavenge;
        public bool CannotCraft;
        public bool CannotFight;

        /// <summary>True if this survivor is a child dependent. Drives the Hope buff.</summary>
        public bool IsChild;

        /// <summary>Prompt #61: True if survivor caught player reading diary and created a hidden stash.</summary>
        public bool HasHiddenStash;
        public System.Collections.Generic.List<string> HiddenItemIds = new System.Collections.Generic.List<string>();

        /// <summary>Prompt #63: True if survivor is currently deployed on an expedition outdoors.</summary>
        public bool IsOnExpedition;

        /// <summary>Prompt #64: List of keepsake item IDs hoarded by survivor in memory of deceased friends.</summary>
        public System.Collections.Generic.List<string> KeepsakeItemIds = new System.Collections.Generic.List<string>();

        /// <summary>
        /// Snake-case id of the <see cref="AtomicWar._Game.Shelter.ShelterRoom"/>
        /// the survivor currently occupies (e.g. "entry", "stores", "quarters").
        /// Empty/null = unassigned (treated as "common area" — the passive
        /// morale drain from a broken survivor hits all other unassigned
        /// survivors in the shelter). Owned by the room-assignment system
        /// (Prompt #29 follow-up); read by MentalBreakSystem and the AI.
        /// </summary>
        public string CurrentRoomId;

        // -------------------------------------------------------------------
        // Mental-break system (Prompt #29). When morale stays below the
        // break-threshold for the configured window, the survivor rolls for
        // a MentalBreakSO and starts behaving erratically. Owned and
        // written by AtomicWar._Game.Survivors.MentalBreakSystem.
        // -------------------------------------------------------------------

        /// <summary>Id of the currently-active MentalBreakSO, or null/empty if sane.
        /// The SO is looked up at consume-time via MentalBreakSystem.GetBreak(id).</summary>
        public string currentMentalBreakId;

        /// <summary>Hours the survivor has been below the low-morale break threshold.
        /// Reset to 0 the moment their morale climbs back above the threshold.
        /// Owned by MentalBreakSystem.</summary>
        public float lowMoraleHours;

        /// <summary>Cure progress accumulated against the active break, in game-hours.
        /// When this reaches the break's <c>cureHours</c>, the break resolves.
        /// Owned by MentalBreakSystem.</summary>
        public float mentalBreakCureProgress;

        /// <summary>Whether the given status is currently active on this survivor.</summary>
        public bool HasStatus(SurvivorStatus status)
        {
            return status switch
            {
                SurvivorStatus.AcuteRadiationSickness => HasAcuteRadiationSickness,
                SurvivorStatus.ChronicIllness => HasChronicIllness,
                SurvivorStatus.RadResistance => HasRadResistance,
                SurvivorStatus.Listless => IsListless,
                SurvivorStatus.AcuteRadiationSyndrome => HasAcuteRadiationSyndrome,
                SurvivorStatus.RadiationAnxiety => HasRadiationAnxietyStatus,
                SurvivorStatus.Numb => IsNumb,
                SurvivorStatus.Fractured => IsFractured,
                SurvivorStatus.Dread => HasDread,
                _ => false
            };
        }

        /// <summary>Convenience: true if the survivor is currently broken
        /// (has a non-empty <c>currentMentalBreakId</c>).</summary>
        public bool HasMentalBreak => !string.IsNullOrEmpty(currentMentalBreakId);

        // -------------------------------------------------------------------
        // Chronic disabilities / permanent consequences (Prompt #36).
        // Earned by surviving critical affliction states > 72 hours. Permanent.
        // -------------------------------------------------------------------
        public System.Collections.Generic.List<string> DisabilityIds = new System.Collections.Generic.List<string>();

        public bool HasDisability(string id)
        {
            if (string.IsNullOrEmpty(id) || DisabilityIds == null) return false;
            for (int i = 0; i < DisabilityIds.Count; i++)
            {
                if (string.Equals(DisabilityIds[i], id, System.StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        /// <summary>
        /// Base max health before disabilities. Juggernaut (#222) doubles this permanently.
        /// </summary>
        public float BaseMaxHealth = 100f;

        /// <summary>
        /// Base max stamina before disabilities. Tireless (#241) triples this permanently.
        /// </summary>
        public float BaseMaxStamina = 100f;

        /// <summary>Health ceiling imposed by the ScarredLungs disability.</summary>
        public const float ScarredLungsHealthCap = 75f;

        /// <summary>Maximum dynamic health cap for this survivor (caps at 75 if ScarredLungs present).</summary>
        public float MaxHealthCap
        {
            get
            {
                float cap = BaseMaxHealth > 0f ? BaseMaxHealth : 100f;
                if (HasDisability(DisabilityId.ScarredLungs))
                    return Mathf.Min(ScarredLungsHealthCap, cap);
                return cap;
            }
        }

        // -------------------------------------------------------------------
        // Personality Traits & Moral Traumas (Prompt #38).
        // -------------------------------------------------------------------
        public System.Collections.Generic.List<string> Traits = new System.Collections.Generic.List<string>();
        public System.Collections.Generic.List<string> Traumas = new System.Collections.Generic.List<string>();

        public bool HasTrait(string traitId)
        {
            if (string.IsNullOrEmpty(traitId) || Traits == null) return false;
            for (int i = 0; i < Traits.Count; i++)
            {
                if (string.Equals(Traits[i], traitId, System.StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        public bool HasTrauma(string traumaId)
        {
            if (string.IsNullOrEmpty(traumaId) || Traumas == null) return false;
            for (int i = 0; i < Traumas.Count; i++)
            {
                if (string.Equals(Traumas[i], traumaId, System.StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        // -------------------------------------------------------------------
        // Aesthetic portrait tags (Prompt #192 Bloodstained, etc.).
        // Subtle UI flags — not gameplay stats. snake_case ids.
        // -------------------------------------------------------------------
        public System.Collections.Generic.List<string> AestheticTags =
            new System.Collections.Generic.List<string>();

        public bool HasAestheticTag(string tagId)
        {
            if (string.IsNullOrEmpty(tagId) || AestheticTags == null) return false;
            for (int i = 0; i < AestheticTags.Count; i++)
            {
                if (string.Equals(AestheticTags[i], tagId, System.StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>Adds a portrait aesthetic tag. Returns true if newly added.</summary>
        public bool AddAestheticTag(string tagId)
        {
            if (string.IsNullOrEmpty(tagId)) return false;
            if (AestheticTags == null) AestheticTags = new System.Collections.Generic.List<string>();
            if (HasAestheticTag(tagId)) return false;
            AestheticTags.Add(tagId);
            return true;
        }

        // -------------------------------------------------------------------
        // Chronic Disease Pipeline (Prompt #39).
        // -------------------------------------------------------------------
        public ChronicIllnessKind? ActiveChronicIllness;
        public float ChronicIllnessManagedHours;

        public bool IsChronicIllnessManaged => ChronicIllnessManagedHours > 0f;

        /// <summary>Fatigue drain multiplier from chronic illness.</summary>
        public float FatigueDrainMultiplier
        {
            get
            {
                if (IsChronicIllnessManaged || !ActiveChronicIllness.HasValue) return 1.0f;
                switch (ActiveChronicIllness.Value)
                {
                    case ChronicIllnessKind.BoneMarrowDepression: return 2.0f;
                    case ChronicIllnessKind.LungFibrosis: return 1.5f;
                    default: return 1.0f;
                }
            }
        }

        /// <summary>Scavenging and surveying yield multiplier from chronic ocular/cataract damage.</summary>
        public float ScavengingYieldMultiplier
        {
            get
            {
                if (IsChronicIllnessManaged || !ActiveChronicIllness.HasValue) return 1.0f;
                return ActiveChronicIllness.Value == ChronicIllnessKind.RadiationCataracts ? 0.5f : 1.0f;
            }
        }

        /// <summary>Maximum stamina cap (reduced to 60 by LungFibrosis; Tireless multiplies base).</summary>
        public float MaxStaminaCap
        {
            get
            {
                float baseStam = BaseMaxStamina > 0f ? BaseMaxStamina : 100f;
                if (!ActiveChronicIllness.HasValue) return baseStam;
                return ActiveChronicIllness.Value == ChronicIllnessKind.LungFibrosis
                    ? Mathf.Min(60f, baseStam) : baseStam;
            }
        }

        /// <summary>Effective MedicalSkill after atrophy + active progression perks.</summary>
        public float EffectiveMedicalSkill
        {
            get
            {
                float v = AtrophiedSkills != null && AtrophiedSkills.Contains("medical")
                    ? MedicalSkill * 0.5f : MedicalSkill;
                return Mathf.Clamp01(v + ProgressionMedicalBonus);
            }
        }

        /// <summary>Effective CraftingSkill after atrophy + active progression perks.</summary>
        public float EffectiveCraftingSkill
        {
            get
            {
                float v = AtrophiedSkills != null && AtrophiedSkills.Contains("crafting")
                    ? CraftingSkill * 0.5f : CraftingSkill;
                return Mathf.Clamp01(v + ProgressionCraftingBonus);
            }
        }

        /// <summary>Effective ScienceSkill with active progression perks.</summary>
        public float EffectiveScienceSkill =>
            Mathf.Clamp01(ScienceSkill + ProgressionScienceBonus);

        // ===================================================================
        // PHASE 0 — Massive Expansion Foundation Fields
        // ===================================================================
        // These ~30 fields support 40+ new systems across 4 expansion
        // categories. Each block is owned by a specific system (see doc).
        // All are plain serializable fields — no auto-properties — so
        // JsonUtility correctly round-trips them through save/load.
        // ===================================================================

        // ---- System 1: Radiation Phase Progression ----
        /// <summary>Explicit phase in the Prodromal→Latent→Manifest→ChronicFibrosis
        /// pipeline. Owned by RadiationPhaseProgression.</summary>
        public RadiationSicknessPhase SicknessPhase = RadiationSicknessPhase.Healthy;
        /// <summary>Hours elapsed in current SicknessPhase.</summary>
        public float PhaseHoursElapsed;
        /// <summary>0..100 lung capacity. Permanently reduced by ChronicFibrosis.</summary>
        public float LungCapacity = 100f;
        /// <summary>True when ChronicFibrosis has permanently damaged lungs.</summary>
        public bool HasPermanentLungDamage;

        // ---- System 2: Phantom Memory Triggers ----
        /// <summary>snake_case background id driving phantom trigger selection
        /// (e.g. child_refugee, former_soldier, nurse).</summary>
        public string PhantomBackgroundId;
        /// <summary>Count of phantom triggers experienced this campaign.</summary>
        public int PhantomTriggersExperienced;
        /// <summary>Hours remaining on motivation boost after a positive trigger.</summary>
        public float PhantomMotivationBoostHours;

        // ---- System 3: Guilt-Driven Insomnia ----
        /// <summary>List of guilt sources from ruthless decisions.</summary>
        public System.Collections.Generic.List<GuiltRecord> GuiltSources =
            new System.Collections.Generic.List<GuiltRecord>();
        /// <summary>0..1 severity multiplying sleep quality penalty.</summary>
        public float GuiltInsomniaSeverity;
        /// <summary>Hours remaining of sedative compensation for guilt insomnia.</summary>
        public float SedativeCompensationHours;

        // ---- System 4: Combat Trauma & Hypervigilance ----
        /// <summary>Total combat encounters survived. Drives hypervigilance.</summary>
        public int CombatEncountersSurvived;
        /// <summary>0..1 — increases defense but causes false-alarm events.</summary>
        public float HypervigilanceLevel;
        /// <summary>Hours since last combat. Hypervigilance decays if this is high.</summary>
        public float HoursSinceLastCombat;
        /// <summary>True if a false alarm already fired tonight (once per night max).</summary>
        public bool HadFalseAlarmTonight;

        // ---- System 5: Somatic Flashback Episodes ----
        /// <summary>0..1 susceptibility to audio-triggered flashbacks.</summary>
        public float FlashbackSusceptibility;
        /// <summary>True while another companion is grounding this survivor.</summary>
        public bool IsGroundedByCompanion;
        /// <summary>0..1 current work efficiency penalty from active flashback.</summary>
        public float FlashbackWorkEfficiencyPenalty;

        // ---- System 6: Desensitization vs Empathy Branching ----
        /// <summary>Total moral choices made. At 5, branching is decided.</summary>
        public int MoralChoiceCount;
        /// <summary>Which branch the survivor is on (Neutral until decided).</summary>
        public MoralBranchDirection BranchDirection = MoralBranchDirection.Neutral;
        /// <summary>0..1 — higher = more immune to death morale loss.</summary>
        public float NumbedResilienceLevel;
        /// <summary>0..1 — higher = stronger shelter buff when helping others.</summary>
        public float BurdenedCompassionLevel;

        // ---- System 7: Respiratory Degenerative Illness ----
        /// <summary>0..100 irreversible respiratory damage from fallout ash.</summary>
        public float RespiratoryDegradation;
        /// <summary>True when survivor needs an inhaler to function normally.</summary>
        public bool RequiresInhaler;
        /// <summary>Hours of relief remaining after using a medical inhaler.</summary>
        public float InhalerReliefHours;

        // ---- System 8: Survivor Trade Specialties ----
        /// <summary>snake_case pre-war profession id (electrician, nurse, machinist, teacher).</summary>
        public string PreWarProfessionId;
        /// <summary>List of craft milestone ids completed by this survivor.</summary>
        public System.Collections.Generic.List<string> CraftMilestonesCompleted =
            new System.Collections.Generic.List<string>();

        // ---- System 9: Terminal Illness & Final Wishes ----
        /// <summary>True when a terminal prognosis has been declared.</summary>
        public bool HasTerminalPrognosis;
        /// <summary>Days remaining before terminal outcome. Counts down.</summary>
        public float TerminalPrognosisDaysRemaining;
        /// <summary>True if the final wish questline was completed.</summary>
        public bool FinalWishCompleted;

        // ---- System 10: Chemical Dependency Arcs ----
        /// <summary>Expanded chemical dependencies beyond base addiction.</summary>
        public System.Collections.Generic.List<ChemicalDependency> ChemicalDependencies =
            new System.Collections.Generic.List<ChemicalDependency>();

        // ---- System 11: Personal Keepsake Slot ----
        /// <summary>snake_case item id of the survivor's single designated keepsake.
        /// Set at game start. Losing it causes severe grief.</summary>
        public string PersonalKeepsakeItemId;
        /// <summary>True if the keepsake has been permanently lost/traded.</summary>
        public bool HasLostKeepsake;
        /// <summary>0..1 grief intensity from lost keepsake. Decays slowly.</summary>
        public float KeepsakeGriefLevel;

        // ---- System 13: Bunker Wall Carvings ----
        /// <summary>Days since this survivor last carved a wall tally/drawing.</summary>
        public int DaysSinceLastWallCarving = 7;
        /// <summary>True if already carved today (once per day max).</summary>
        public bool HasCarvedTallyToday;

        // ---- System 14: Lost Correspondence ----
        /// <summary>Letter item ids recovered for this survivor's family subplot.</summary>
        public System.Collections.Generic.List<string> RecoveredLetterIds =
            new System.Collections.Generic.List<string>();
        /// <summary>Family fate subplot ids that have been resolved.</summary>
        public System.Collections.Generic.List<string> ResolvedFamilyFateIds =
            new System.Collections.Generic.List<string>();

        // ---- System 19: Memorial Wall ----
        /// <summary>True if survivor has paid respects at the memorial wall.</summary>
        public bool HasPaidRespectsAtMemorial;
        /// <summary>Hours remaining of memorial comfort morale buff.</summary>
        public float MemorialComfortHours;

        // ---- System 21: Shared Trauma Bonding ----
        /// <summary>Records of trauma bonds formed with other survivors.</summary>
        public System.Collections.Generic.List<TraumaBondRecord> TraumaBonds =
            new System.Collections.Generic.List<TraumaBondRecord>();

        // ---- System 22: Ideological Friction ----
        /// <summary>snake_case belief profile id for roommate compatibility checks.</summary>
        public string BeliefProfileId;

        // ---- System 23: Ration Allocation Conflicts ----
        /// <summary>0..1 — how fair this survivor perceives ration distribution to be.</summary>
        public float PerceivedRationFairness = 1f;
        /// <summary>Id of the survivor most resented for over-allocation (or empty).</summary>
        public string RationResentmentTargetId;
        /// <summary>0..1 — accumulated resentment toward RationResentmentTargetId.</summary>
        public float RationResentmentLevel;

        // ---- System 25: Confession & Reconciliation ----
        /// <summary>Ids of survivors this survivor holds a permanent grudge against.</summary>
        public System.Collections.Generic.List<string> GrudgeIds =
            new System.Collections.Generic.List<string>();

        // ---- System 27: Desertion Risk ----
        /// <summary>0..1 — accumulated desertion intent from low morale + faction pressure.</summary>
        public float DesertionIntent;
        /// <summary>Days since last desertion attempt (cooldown).</summary>
        public float DaysSinceDesertionAttempt = 30f;

        // ---- System 29: Leadership ----
        /// <summary>True if designated as the informal bunker leader.</summary>
        public bool IsDesignatedLeader;
        /// <summary>0..100 accumulated leader stress from deaths and crises.</summary>
        public float LeaderStressAccumulation;
        /// <summary>Count of deaths witnessed while leader.</summary>
        public int LeaderDeathsWitnessed;

        // ---- System 30: Compassionate Caregiving ----
        /// <summary>Id of the survivor currently being cared for by this survivor.</summary>
        public string CaregivingTargetId;
        /// <summary>Id of the survivor currently providing care to this survivor.</summary>
        public string CaregiverId;
        /// <summary>0..1 accumulated caregiving bond strength with CaregiverId.</summary>
        public float CaregivingBondStrength;

        // ---- System 11b: GriefKeepsake expansion ----
        /// <summary>True if this survivor has at least one keepsake from a fallen friend.</summary>
        public bool HasGriefKeepsake =>
            KeepsakeItemIds != null && KeepsakeItemIds.Count > 0;

        // ---- Convenience helpers for new fields ----

        /// <summary>True if in an active radiation sickness phase (not Healthy).</summary>
        public bool HasActiveRadiationSickness =>
            SicknessPhase != RadiationSicknessPhase.Healthy &&
            SicknessPhase != RadiationSicknessPhase.RecoveryOrDeath;

        /// <summary>True if the survivor has any chemical dependency registered.</summary>
        public bool HasChemicalDependency =>
            ChemicalDependencies != null && ChemicalDependencies.Count > 0;

        /// <summary>True if the moral branch has been decided (non-Neutral).</summary>
        public bool HasMoralBranch => BranchDirection != MoralBranchDirection.Neutral;

        /// <summary>True if this survivor has a trauma bond with the given survivor.</summary>
        public bool HasTraumaBondWith(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId) || TraumaBonds == null) return false;
            for (int i = 0; i < TraumaBonds.Count; i++)
            {
                if (string.Equals(TraumaBonds[i].BondedSurvivorId, survivorId,
                    System.StringComparison.Ordinal)) return true;
            }
            return false;
        }

        /// <summary>True if this survivor holds a grudge against the given survivor.</summary>
        public bool HasGrudgeAgainst(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId) || GrudgeIds == null) return false;
            for (int i = 0; i < GrudgeIds.Count; i++)
            {
                if (string.Equals(GrudgeIds[i], survivorId,
                    System.StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        // ===================================================================
        // ANTIGRAVITY EXPANSION — Mechanics #41–80 Fields
        // ===================================================================

        // ---- #41: Claustrophobic Tremors ----
        /// <summary>0..1 — builds from prolonged low-ceiling confinement.</summary>
        public float ClaustrophobiaLevel;
        /// <summary>Hours of outdoor/ventilated time needed to calm claustrophobia.</summary>
        public float OutdoorHoursNeeded;
        /// <summary>True when claustrophobia is causing active panic penalty.</summary>
        public bool HasClaustrophobicPanic;

        // ---- #42: Auditory Tinnitus ----
        /// <summary>True if survivor has tinnitus from blast/exposure.</summary>
        public bool HasTinnitus;
        /// <summary>Hours remaining of tinnitus episode.</summary>
        public float TinnitusHoursRemaining;
        /// <summary>True when tinnitus blocks verbal raid warnings.</summary>
        public bool IsDeafToWarnings;

        // ---- #43: Somnambulism (Sleepwalking) ----
        /// <summary>0..1 risk of sleepwalking tonight.</summary>
        public float SleepwalkingRisk;
        /// <summary>Last room id the survivor sleepwalked to.</summary>
        public string LastSleepwalkRoomId;
        /// <summary>True if sleepwalked tonight (once per night max).</summary>
        public bool DidSleepwalkTonight;

        // ---- #44: Phantom Hunger & Hoarding ----
        /// <summary>True if survivor compulsively hoards food.</summary>
        public bool HasHoardingCompulsion;
        /// <summary>Count of food items hidden in personal quarters.</summary>
        public int HiddenFoodCount;
        /// <summary>True if hoard was discovered during room inspection.</summary>
        public bool HoardWasDiscovered;

        // ---- #45: Hyper-Vigilant Scavenger's Eye ----
        /// <summary>True if this survivor developed the scavenger's eye perk.</summary>
        public bool HasScavengerEye;
        /// <summary>Count of hidden caches discovered with this perk.</summary>
        public int HiddenCachesFound;

        // ---- #46: Nerve Damage & Steady Hand Strain ----
        /// <summary>True if survivor has nerve damage from radiation/wounds.</summary>
        public bool HasNerveDamage;
        /// <summary>Hours remaining of nerve stabilizer drug effect.</summary>
        public float NerveStabilizerHours;
        /// <summary>0..1 weapon accuracy penalty from nerve damage.</summary>
        public float WeaponAccuracyModifier;

        // ---- #47: Survivor Bond Resilience (Lifeline) ----
        /// <summary>Id of the bonded partner who grants the Lifeline perk.</summary>
        public string LifelinePartnerId;
        /// <summary>True if this survivor has the Lifeline perk active.</summary>
        public bool HasLifelinePerk;

        // ---- #48: Anosmia (Loss of Smell) ----
        /// <summary>True if survivor lost sense of smell from toxic exposure.</summary>
        public bool HasAnosmia;
        /// <summary>True when anosmia makes them blind to gas leaks.</summary>
        public bool IsBlindToGasLeaks;

        // ---- #49: Survivor Faith / Philosophy ----
        /// <summary>Philosophical stance adopted by the survivor.</summary>
        public PhilosophicalStance Stance = PhilosophicalStance.Undecided;

        // ---- #50: Terminal Radiation Acceptance ----
        /// <summary>True when survivor has accepted their fate at 800+ mSv.</summary>
        public bool HasTerminalAcceptance;
        /// <summary>True when acceptance grants immunity to morale loss from volunteering.</summary>
        public bool VolunteerMoraleImmune;

        // ---- #51: Fallout Season Transitions ----
        /// <summary>Current season as perceived by survivor (affects mood).</summary>
        public string PerceivedSeasonId;

        // ---- #52: Ash Drift Room Burial ----
        /// <summary>Hours since the hatch was last cleared of ash.</summary>
        public float HoursSinceAshClearing;

        // ---- #55: Ghost Town Repopulation ----
        /// <summary>Set of location ids this survivor has seen change state.</summary>
        public System.Collections.Generic.List<string> WitnessedLocationChanges =
            new System.Collections.Generic.List<string>();

        // ---- #60: Survivor Graveyard ----
        /// <summary>Count of times this survivor has visited grave markers.</summary>
        public int GraveVisitsCount;
        /// <summary>Hours remaining of grave-visit morale buff.</summary>
        public float GraveVisitComfortHours;

        // ---- #71: Survival Codex / Bunker Manifesto ----
        /// <summary>The law code this survivor follows.</summary>
        public string ManifestoLawCodeId;
        /// <summary>0..1 how strongly this survivor adheres to the manifesto.</summary>
        public float ManifestoAdherence;

        // ---- #72: Generational Knowledge Transfer ----
        /// <summary>Count of skills successfully transferred to younger survivors.</summary>
        public int SkillsTransferredCount;

        // ---- #73: Vault of Pre-War Culture ----
        /// <summary>Count of cultural artifacts this survivor has preserved.</summary>
        public int CulturalArtifactsPreserved;
        /// <summary>0..1 permanent psychological resilience from culture.</summary>
        public float CulturalResilience;

        // ---- #75: Deep Geothermal / Aquifer Project ----
        /// <summary>Hours contributed to the deep aquifer drilling project.</summary>
        public float AquiferProjectHoursContributed;

        // ---- #78: Faction Peace Treaty ----
        /// <summary>True if this survivor witnessed the peace treaty signing.</summary>
        public bool WitnessedPeaceTreaty;

        // ---- #79: Final Fallout Cleansing Forecast ----
        /// <summary>Known days until primary fallout dissipates.</summary>
        public float KnownFalloutCleansingDays;

        // ---- #80: Campaign Legacy ----
        /// <summary>List of campaign milestone ids recorded for this survivor.</summary>
        public System.Collections.Generic.List<string> CampaignMilestoneIds =
            new System.Collections.Generic.List<string>();

        // ---- Convenience helpers for Antigravity fields ----

        /// <summary>True if claustrophobia is currently active.</summary>
        public bool HasActiveClaustrophobia => ClaustrophobiaLevel > 0.5f;

        /// <summary>True if nerve damage is currently affecting performance.</summary>
        public bool HasActiveNerveDamage =>
            HasNerveDamage && NerveStabilizerHours <= 0f;

        /// <summary>True if philosophy stance has been chosen.</summary>
        public bool HasPhilosophicalStance => Stance != PhilosophicalStance.Undecided;

        /// <summary>Effective weapon accuracy modifier accounting for nerve damage.</summary>
        public float EffectiveWeaponAccuracy =>
            HasActiveNerveDamage ? (1f - WeaponAccuracyModifier) : 1f;
    }

    /// <summary>
    /// A single consumption event of a tracked (addictive) item. Logged per survivor
    /// so AddictionSystem can count uses within a rolling 7-day window.
    /// </summary>
    [System.Serializable]
    public struct ConsumptionRecord
    {
        public string ItemId;
        public int DayConsumed;

        public ConsumptionRecord(string itemId, int dayConsumed)
        {
            ItemId = itemId;
            DayConsumed = dayConsumed;
        }
    }

    // ===================================================================
    // Phase 0 — New Serializable Structs for Expansion Fields
    // ===================================================================

    /// <summary>
    /// A guilt source recorded when a survivor makes or witnesses a ruthless
    /// decision. Owned by GuiltInsomniaSystem.
    /// </summary>
    [System.Serializable]
    public struct GuiltRecord
    {
        /// <summary>snake_case id of the decision/event that caused guilt.</summary>
        public string SourceId;
        /// <summary>Campaign day when guilt was recorded.</summary>
        public int DayRecorded;
        /// <summary>0..1 severity of this guilt source.</summary>
        public float Severity;

        public GuiltRecord(string sourceId, int dayRecorded, float severity)
        {
            SourceId = sourceId;
            DayRecorded = dayRecorded;
            Severity = severity;
        }
    }

    /// <summary>
    /// A chemical dependency on a specific addictive substance. Extends
    /// the base AddictionSystem with substance-specific tracking.
    /// Owned by ChemicalDependencySystem.
    /// </summary>
    [System.Serializable]
    public struct ChemicalDependency
    {
        /// <summary>snake_case item id of the addictive substance.</summary>
        public string ItemId;
        /// <summary>0..1 dependency level. Higher = worse withdrawal.</summary>
        public float DependencyLevel;
        /// <summary>Kind of chemical (opioid, alcohol, stimulant, sedative).</summary>
        public ChemicalDependencyKind Kind;
        /// <summary>True if currently in a managed detox program.</summary>
        public bool InManagedDetox;
        /// <summary>Hours of detox progress accumulated.</summary>
        public float DetoxProgressHours;

        public ChemicalDependency(string itemId, float dependencyLevel,
            ChemicalDependencyKind kind, bool inManagedDetox = false,
            float detoxProgressHours = 0f)
        {
            ItemId = itemId;
            DependencyLevel = dependencyLevel;
            Kind = kind;
            InManagedDetox = inManagedDetox;
            DetoxProgressHours = detoxProgressHours;
        }
    }

    /// <summary>
    /// A trauma bond formed between two survivors who endured a shared
    /// hazard together. Owned by TraumaBondSystem.
    /// </summary>
    [System.Serializable]
    public struct TraumaBondRecord
    {
        /// <summary>Id of the other survivor in this bond.</summary>
        public string BondedSurvivorId;
        /// <summary>0..1 bond strength. Decays without shared activity.</summary>
        public float BondStrength;
        /// <summary>snake_case id of the hazard that formed this bond.</summary>
        public string SharedHazardId;
        /// <summary>Campaign day when this bond was formed.</summary>
        public int DayFormed;

        public TraumaBondRecord(string bondedSurvivorId, float bondStrength,
            string sharedHazardId, int dayFormed)
        {
            BondedSurvivorId = bondedSurvivorId;
            BondStrength = bondStrength;
            SharedHazardId = sharedHazardId;
            DayFormed = dayFormed;
        }
    }

    // ===================================================================
    // Antigravity Expansion — New Serializable Structs (#41–80)
    // ===================================================================

    /// <summary>
    /// A hidden food hoard discovered during room inspection.
    /// Owned by HoardingBehaviorSystem (#44).
    /// </summary>
    [System.Serializable]
    public struct HiddenFoodHoard
    {
        public string ItemId;
        public int Count;
        public int DayHidden;
        public bool IsDiscovered;

        public HiddenFoodHoard(string itemId, int count, int dayHidden)
        {
            ItemId = itemId;
            Count = count;
            DayHidden = dayHidden;
            IsDiscovered = false;
        }
    }

    /// <summary>
    /// A sleepwalking incident recorded for a survivor.
    /// Owned by SleepwalkingSystem (#43, extends SleepDeprivationSystem).
    /// </summary>
    [System.Serializable]
    public struct SleepwalkIncident
    {
        public int DayOccurred;
        public string OriginRoomId;
        public string DestinationRoomId;
        public string ActionTaken; // "moved_food", "unlocked_hatch", "wandered_hazard"

        public SleepwalkIncident(int day, string origin, string dest, string action)
        {
            DayOccurred = day;
            OriginRoomId = origin;
            DestinationRoomId = dest;
            ActionTaken = action;
        }
    }

    /// <summary>
    /// A manifesto law entry in the Survival Codex.
    /// Owned by BunkerManifestoSystem (#71).
    /// </summary>
    [System.Serializable]
    public struct ManifestoLaw
    {
        public string LawId;
        public string Category; // rationing, medical, defense, justice
        public string DisplayText;
        public int DayEnacted;
        public float AdherenceBonus; // morale effect for followers

        public ManifestoLaw(string lawId, string category, string text,
            int dayEnacted, float adherenceBonus)
        {
            LawId = lawId;
            Category = category;
            DisplayText = text;
            DayEnacted = dayEnacted;
            AdherenceBonus = adherenceBonus;
        }
    }

    /// <summary>
    /// A single campaign milestone for the Chronicle of Ash.
    /// Owned by ChronicleLogger (#80).
    /// </summary>
    [System.Serializable]
    public struct CampaignMilestone
    {
        public string MilestoneId;
        public int DayRecorded;
        public string Category; // death, moral_choice, faction_deal, storm_survival, discovery
        public string Description;
        public string[] InvolvedSurvivorIds;

        public CampaignMilestone(string id, int day, string category,
            string description, string[] involvedIds)
        {
            MilestoneId = id;
            DayRecorded = day;
            Category = category;
            Description = description;
            InvolvedSurvivorIds = involvedIds ?? new string[0];
        }
    }
}
