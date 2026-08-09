using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Simulation; // CompostSystem, SterilizationSystem, etc. (audit C-3 split)
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Events;

namespace AtomicWar._Game.Core
{
    // =====================================================================

    [Serializable]
    public class SaveData
    {
        public int SaveVersion = SaveSystem.CurrentSaveVersion;
        public string Checksum = "";
        public GameStateSave GameState = new GameStateSave();
        public WeatherState Weather;
        public float ElapsedHours;
        public PhotoperiodState Photoperiod;
        public List<SurvivorSave> Survivors = new List<SurvivorSave>();
        public List<ShelterModuleSave> ShelterModules = new List<ShelterModuleSave>();
        public List<string> WorldFlagKeys = new List<string>();
        public List<bool> WorldFlagValues = new List<bool>();
        public RadiationKnowledgeSave RadiationKnowledge;
        public InventorySaveState Inventory;
        public MedicalSystemSave Medical;
        public WorldPhaseSave WorldPhase;
        public DynamicEconomySave Economy;
        public PowerNetworkSave Power;
        public HatchDefenseSave HatchDefense;
        public FactionRadioInterceptSave FactionRadioIntercepts;
        public JournalSave Journal;
        public VictoryProjectSave VictoryProject;
        /// <summary>Deferred narrative chain queue (Prompt #43).</summary>
        public ScheduledEventSave ScheduledEvents;
        /// <summary>EventRunner cooldowns + active delayed consequences.</summary>
        public EventRunnerStateSave EventRunnerState;
        /// <summary>Internal mystery / Missing Rations state.</summary>
        public SuspicionTrackerSave Suspicion;
        /// <summary>Weather-driven hatch seal / DigOut / suffocation (Prompt #48).</summary>
        public HatchEntrapmentSave HatchEntrapment;
        /// <summary>Internal Horror — room atmosphere / fire / humidity.</summary>
        public ShelterAtmosphereSave Atmosphere;
        /// <summary>Internal Horror — corpse source ids.</summary>
        public CorpseManagementSave Corpses;
        /// <summary>Internal Horror — pantry rust state.</summary>
        public PantryContaminationSave Pantry;
        /// <summary>Prompt #13 — scavenging habit score + plant/detect counters.</summary>
        public SabotagedCacheSave SabotagedCaches;
        /// <summary>Prompt #14 — death-zone windstorm shift history.</summary>
        public ShiftingHotspotSave ShiftingHotspots;
        /// <summary>Prompt #17 — inter-faction raid plan wiretaps.</summary>
        public FactionRaidPlanSave FactionRaidPlans;
        /// <summary>Prompt #18 — delayed dig-out debt collector ledger.</summary>
        public DebtCollectorSave DebtCollector;
        /// <summary>Prompt #19 — post-EMP ghost station unlock + heard set.</summary>
        public GhostStationSave GhostStations;
        /// <summary>Prompt #20 — Lifeboat Transmission contact/resolve state.</summary>
        public LifeboatTransmissionSave Lifeboat;
        /// <summary>Child Dependent system state (Prompt #9).</summary>
        public ChildDependentSystem.SaveState ChildDependent;
        /// <summary>Prompt #49 — structural integrity + cave-in state.</summary>
        public StructuralIntegritySave StructuralIntegrity;
        /// <summary>Prompt #50 — waste accumulation + hygiene.</summary>
        public WasteSystemSave Waste;
        /// <summary>Prompt #51 — vermin pest level.</summary>
        public VerminSave Vermin;
        /// <summary>Prompt #52 — jury-rigged modules.</summary>
        public JuryRigSave JuryRig;
        /// <summary>Prompt #53 — frozen pipes + burst state.</summary>
        public FreezePipeSave FreezePipe;
        /// <summary>Prompt #55 — blood types + transfusion state.</summary>
        public BloodTransfusionSave BloodTransfusion;
        /// <summary>Prompt #56 — amputee tracking.</summary>
        public AmputationSave Amputation;
        /// <summary>Prompt #57 — scurvy deficiency + toothache.</summary>
        public ScurvySave Scurvy;
        /// <summary>Prompt #60 — radiation mutagenesis stages.</summary>
        public MutagenesisSave Mutagenesis;
        /// <summary>Prompt #67 — cartography charted nodes + supplies.</summary>
        public CartographySave Cartography;
        /// <summary>Prompt #69 — flooded map nodes.</summary>
        public FloodedNodeSave FloodedNodes;
        /// <summary>Prompt #71 — faction tracker entries.</summary>
        public TrackerSave Tracker;
        /// <summary>Prompt #72 — dead drop entries.</summary>
        public DeadDropSave DeadDrops;
        /// <summary>Prompt #73 — hostage situations.</summary>
        public HostageSave Hostages;
        /// <summary>Prompt #74 — propaganda broadcasts.</summary>
        public PropagandaSave Propaganda;
        /// <summary>Prompt #75 — deserter/spy entries.</summary>
        public DeserterSave Deserters;
        /// <summary>Prompt #76 — weather scapegoat state.</summary>
        public ScapegoatSave Scapegoat;
        /// <summary>Prompt #77 — labor camp slave tracking.</summary>
        public LaborCampSave LaborCamps;
        /// <summary>Prompt #78 — cult moral disgust.</summary>
        public CultMoralSave CultMoral;
        /// <summary>Prompt #79 — mutated ecosystem radiation days.</summary>
        public EcosystemSave Ecosystem;
        public WaterStorageSave Water;
        /// <summary>Accumulated bunker ambient contamination (rads/hr) from hatch dilemmas.</summary>
        public float BunkerContamination;
        public GeneratedMapSave GeneratedMap;
        public FlashpointChoreographerSave FlashpointChoreographer;
        public AffinityMatrixSave Affinity = new AffinityMatrixSave();
        public List<ExpeditionSaveState> Expeditions = new List<ExpeditionSaveState>();

        /// <summary>PhantomIntruderSystem cooldowns (Prompt #6). survivorId → remaining cooldown hours.</summary>
        public List<string> PhantomCooldownKeys = new List<string>();
        public List<float> PhantomCooldownValues = new List<float>();

        /// <summary>ShelterRoom unlock state + rubble progress (Prompt #5).</summary>
        public List<ShelterRoomSave> ShelterRooms = new List<ShelterRoomSave>();
        public AestheticsSave Aesthetics;
        public AirlockSave Airlock;
        public AntibioticResistSave AntibioticResist;
        public CeilingCollapseSave CeilingCollapse;
        public ChelationSave Chelation;
        public CompostSave Compost;
        public SkillProgressionSave SkillProgression;
        /// <summary>Prompts #182–#188 — combat milestone counters (jams, kills, flees…).</summary>
        public CombatPerkSave CombatPerks;
        /// <summary>Prompts #189–#194 — survival milestone counters (meals, crops, planter…).</summary>
        public SurvivalPerkSave SurvivalPerks;
        /// <summary>Prompts #195–#200 — shelter engineering counters (jury-rigs, struts, dig…).</summary>
        public ShelterPerkSave ShelterPerks;
        /// <summary>Prompts #201–#205 — medical milestone counters (Phase-2 cures, Death's Door…).</summary>
        public MedicalPerkSave MedicalPerks;
        /// <summary>Prompts #206–#210 — expedition milestone counters (pack mule, light step…).</summary>
        public ExpeditionPerkSave ExpeditionPerks;
        /// <summary>Prompts #211–#213 — social / leadership counters (de-escalator, haul, morale…).</summary>
        public SocialPerkSave SocialPerks;
        /// <summary>Prompts #214–#219 — personal quest progress + latent trait unlock flags.</summary>
        public PersonalQuestSave PersonalQuests;
        public EscapeHatchSave EscapeHatch;
        public ExcavationSave Excavation;
        public FloodingSave Flooding;
        public HamRadioSave HamRadio;
        public HatchVisibilitySave HatchVisibility;
        public HaulingSave Hauling;
        public HiddenStorageSave HiddenStorage;
        public HouseToBunkerSave HouseToBunker;
        public LocationQuestSave LocationQuests;
        public MaterialShieldingSave MaterialShielding;
        public NoiseSave Noise;
        public PerimeterTrapSave PerimeterTraps;
        public PolypharmSave Polypharmacy;
        public ResilienceSave Resilience;
        public SterilizationSave Sterilization;
        public List<string> SubsystemSaveIds = new List<string>();
        public List<string> SubsystemSaveJsons = new List<string>();
        public TriageSave Triage;
        public TunnelingSave Tunneling;
        public WeaponMaintSave WeaponMaint;
        public WindTurbineSave WindTurbine;


    }

    /// <summary>Expedition runtime state snapshot.</summary>
    [Serializable]
    public class ExpeditionSaveState
    {
        public string ExpeditionId;
        public string SurvivorId;
        public string TargetLocationId;
        public string TargetLocationName;
        public ExpeditionStance Stance;
        public ExpeditionPhase Phase;
        public int CurrentTick;
        public int TotalDistanceTicks;
        public int TravelTicksCompleted;
        public int LootingTicksCompleted;
        public float CarryingCapacity;
        public float CurrentWeight;
        public float Stamina;
        public float SuitDegradation;
        public float TrueRadPerHour;
        public float DangerLevel;
        public bool IsPushingLuck;
        public bool IsRetreating;

        // Day-30 Flashpoint intercept state (Prompt #26)
        public bool IsCommsSevered;
        public FlashpointBehavior FlashpointBehavior;
        public float OriginalEtaTicks;
        public int ShelterDelayTicksRemaining;
        public float ReturnSpeedMultiplier = 1f;
        public float ReturnSpeedDivisor = 1f;

        /// <summary>Prevents double-trigger of location-bound encounters across save/load.</summary>
        public bool LocationEncounterFired;
        /// <summary>Prevents double-trigger of UXO landmine across save/load.</summary>
        public bool UxoDetonated;
        /// <summary>Prompt #210 — Forager's once-per-expedition empty-loot food grant.
        /// Same double-trigger guard as the two flags above: unpersisted, it reset to
        /// false on load, so dropping the granted food and scavenging empty again
        /// re-granted it.</summary>
        public bool ForagerLootApplied;

        // Prompt #68 — bicycle logistics
        public bool HasBicycle;
        public float BicycleDurability;

        // Prompt #69 — flooded node wading
        public bool IsWading;

        // Prompt #70 — night scavenging
        public bool IsNightScavenge;
        public bool HasFlashlight;
        public float FlashlightBattery;

        public List<string> CollectedLootItemIds = new List<string>();
    }

    // =====================================================================
    // Sub-snapshots
    // =====================================================================

    /// <summary>DTO for GameState (auto-properties are invisible to JsonUtility).</summary>
    [Serializable]
    public class GameStateSave
    {
        public GamePhase Phase;
        public int Day;
        public bool IsPaused;
    }

    /// <summary>Full survivor snapshot including needs, radiation, and dosimeter.</summary>
    [Serializable]
    public class SurvivorSave
    {
        public string Id;
        public string DisplayName;
        public SurvivorState State;

        // Needs (plain fields for JsonUtility)
        public float Hunger;
        public float Thirst;
        public float Fatigue;
        public float Warmth = 100f;
        public float Morale = 75f;
        public float Health = 100f;
        // Hygiene was a live need (ToothDecaySystem, HotShower, WastelandSoap all
        // read/write it) that was never persisted, so it silently reset to full on
        // every load. The 100f default matches Needs.Hygiene so pre-fix saves — which
        // carry no Hygiene key — deserialise to "clean" rather than to 0.
        public float Hygiene = 100f;
        public bool WasHungerCritical;
        public bool WasThirstCritical;
        public bool WasWarmthCritical;

        // Radiation
        public float RadiationDose;
        public float LifetimeRadiationExposure;
        public bool HasAcuteRadiationSickness;
        public bool HasChronicIllness;
        public bool HasRadResistance;
        public float RadResistanceHoursRemaining;
        public bool HasFullSuitEquipped;

        // Latent damage / prognosis pipeline
        public float AcuteDoseWindow;
        public PrognosisStage PrognosisStage;
        public float OnsetTimer;
        public float LatentDamage;
        public float IodineProtectionTimer;
        public bool HasAcuteRadiationSyndrome;

        // Dosimeter
        public float DosimeterRate;
        public float DosimeterRecent;

        // Light / photoperiod
        public float LightExposure = 100f;
        public float VitaminDProxy = 100f;
        public bool  IsListless;

        // Medical skill (0..1)
        public float MedicalSkill = 0.3f;

        // Prompt #10/8/7 — new skill & trait fields
        public float ScienceSkill = 0.3f;
        public float CraftingSkill = 0.3f;
        /// <summary>Predetermined expert discipline (Prompt #179). One per survivor.</summary>
        public string ExpertDisciplineId;
        public float ConsecutiveLowMoraleDays;
        public List<string> AtrophiedSkills = new List<string>();
        public List<string> Traits = new List<string>();
        public List<string> Traumas = new List<string>();
        public RiskBiasTrait RiskBias = RiskBiasTrait.Realist;

        // Prompt #7 — Addiction & Withdrawal
        public List<ConsumptionRecordSave> ConsumptionHistory = new List<ConsumptionRecordSave>();
        public float HoursSinceLastDose;
        public bool IsInWithdrawal;

        // Prompt #9 — Child / Dependent
        public bool CannotScavenge;
        public bool CannotCraft;
        public bool CannotFight;
        public bool IsChild;

        // Belief / risk-perception (pre-existing gap — now saved)
        public float PerceivedRadRisk = 0.3f;
        public float TrustInInstruments = 0.7f;
        public float RadiationAnxiety;
        public float Numbness;
        public bool HasRadiationAnxietyStatus;
        public bool IsNumb;
        public string CurrentRoomId;

        // Chronic Disease (pre-existing gap — now saved)
        public string ActiveChronicIllness; // "BoneMarrowDepression", "LungFibrosis", "RadiationCataracts", or empty
        public float ChronicIllnessManagedHours;
        public List<string> DisabilityIds = new List<string>();

        // Prompt #165 — Clothing degradation
        public float ClothingDurability = 100f;
        public bool IsRagged;

        // Grief keepsakes (item ids held as memorials — was missing from survivor DTO)
        public List<string> KeepsakeItemIds = new List<string>();

        // Mental-break system (Prompt #29)
        public string CurrentMentalBreakId = string.Empty;
        public float LowMoraleHours;
        public float MentalBreakCureProgress;

        // Internal mysteries (Prompt #45) — permanent Fractured status
        public bool IsFractured;

        // Prompts #214–#219 — personal quest destiny (latent trait not Day-0 granted).
        public string ArchetypeId;
        public string LatentExpertTraitId;
        public string ActiveQuestlineId;
        public bool QuestlineActive;
        public bool LatentTraitUnlocked;
        public int QuestStage;
        public float QuestProgress;
        public int DaysAlive;
        public bool MoraleHitZero;
    }

    /// <summary>Serializable mirror of Survivors.ConsumptionRecord for save/load.</summary>
    [Serializable]
    public struct ConsumptionRecordSave
    {
        public string ItemId;
        public int DayConsumed;
    }

    /// <summary>ShelterRoom unlock + rubble state (Prompt #5).</summary>
    [Serializable]
    public class ShelterRoomSave
    {
        public string RoomId;
        public int UnlockState; // cast from RoomUnlockState enum
        public float RubbleClearHoursRemaining;
        public float RubbleClearHoursTotal;
        public List<string> DiaryFragmentIds = new List<string>();
        public List<int> RevealedDiaryIndices = new List<int>();
        // Room atmosphere (fire, mold, CO2, O2, bulkhead, contamination) is NOT
        // duplicated here on purpose: ShelterAtmosphereSystem.CaptureState owns it
        // and shares the same ShelterRoom instances. A second writer here would
        // race with it on restore order.
    }

    /// <summary>Shelter module runtime state snapshot.</summary>
    [Serializable]
    public class ShelterModuleSave
    {
        public string ModuleId;
        public int Level = 1;
        public bool IsEnabled = true;
        public float FilterHealth = 100f;
        public float Fuel;
        /// <summary>Prompt #200 Thermodynamics: burn multiplier from the last fuel
        /// loader (0.8 = 20% longer). Defaults to 1 so pre-SAVE-011 saves load
        /// as they always did.</summary>
        public float FuelBurnMultiplier = 1f;
        public float WaterConversionProgress;
        /// <summary>Shelter room the module is installed in (e.g. "quarters", "plant").</summary>
        public string RoomId;
        /// <summary>Bed modules: current sleepers this evaluation wave.</summary>
        public int Occupancy;
        /// <summary>Bed module comfort fallback (0..1).</summary>
        public float ComfortLevel;
        /// <summary>Bed module capacity fallback.</summary>
        public int Capacity;
        /// <summary>Hatch defense fallback security points per level. HatchDefenseSystem
        /// re-defaults this from the module id when it is &lt;= 0, so an unpersisted
        /// custom value silently reverted to the stock number instead of vanishing.</summary>
        public float SecurityContribution;
    }
}
