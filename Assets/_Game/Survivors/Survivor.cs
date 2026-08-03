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

        // -------------------------------------------------------------------
        // Medical triage — skill for Treat Patient actions. Affliction instances
        // live in MedicalSystem (keyed by survivor id); this is the only skill
        // field needed on the survivor for treatment speed / resource sparing.
        // -------------------------------------------------------------------

        /// <summary>0..1 medical competence. Higher = faster treatments, fewer spare parts used.</summary>
        public float MedicalSkill = 0.3f;

        /// <summary>
        /// 0..1 scientific/technical competence. Drives audio/signal analysis, circuit
        /// diagnostics, and "is this a recording loop?" inference. Default 0.3 means
        /// the average survivor cannot reliably scrutinize a broadcast — the radio
        /// only becomes a safe narrative tool if the bunker has a medic or a tech
        /// at the dial. Used by EventContext.HasTraitInBunker("Science").
        /// </summary>
        public float ScienceSkill = 0.3f;

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
            switch (status)
            {
                case SurvivorStatus.AcuteRadiationSickness: return HasAcuteRadiationSickness;
                case SurvivorStatus.ChronicIllness: return HasChronicIllness;
                case SurvivorStatus.RadResistance: return HasRadResistance;
                case SurvivorStatus.Listless: return IsListless;
                case SurvivorStatus.AcuteRadiationSyndrome: return HasAcuteRadiationSyndrome;
                case SurvivorStatus.RadiationAnxiety: return HasRadiationAnxietyStatus;
                case SurvivorStatus.Numb: return IsNumb;
                case SurvivorStatus.Fractured: return IsFractured;
                default: return false;
            }
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

        /// <summary>Maximum dynamic health cap for this survivor (caps at 75 if ScarredLungs present).</summary>
        public float MaxHealthCap
        {
            get
            {
                if (HasDisability("scarred_lungs")) return 75f;
                return 100f;
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

        /// <summary>Maximum stamina cap (reduced to 60 by LungFibrosis).</summary>
        public float MaxStaminaCap
        {
            get
            {
                if (!ActiveChronicIllness.HasValue) return 100f;
                return ActiveChronicIllness.Value == ChronicIllnessKind.LungFibrosis ? 60f : 100f;
            }
        }
    }
}
