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

        /// <summary>Maximum dynamic health cap for this survivor (caps at 75 if ScarredLungs present).</summary>
        public float MaxHealthCap
        {
            get
            {
                float cap = BaseMaxHealth > 0f ? BaseMaxHealth : 100f;
                if (HasDisability("scarred_lungs")) return Mathf.Min(75f, cap);
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
}
