using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Simulation; // CompostSystem, SterilizationSystem, ChelationSystem, etc. (audit C-3 split)
using Random = System.Random;

namespace AtomicWar._Game.AI
{
    /// <summary>
    /// Evaluation context passed to Utility AI actions containing survivor needs,
    /// shelter state, inventory contents, and environmental world state.
    /// </summary>
    public class AIContext
    {
        public Survivor Survivor;
        public Shelter.Shelter Shelter;
        public Inventory.Inventory Inventory;
        public bool IsFalloutStorm;
        public float AmbientRadRate;
        public bool IsRadiationRising;
        /// <summary>True when the survivor currently has the Listless status (light deprivation).</summary>
        public bool IsListless;
        /// <summary>True when the shelter's grow-light module is running; relevant to morale-seeking actions.</summary>
        public bool GrowLightActive;

        /// <summary>0..1 current uncertainty in the survivor's picture of danger (e.g.
        /// 1 - RadiationKnowledgeMap map-tile confidence).</summary>
        public float MapUncertainty;

        /// <summary>Injected by GameBootstrap: computes belief-adjusted action multipliers.</summary>
        public BeliefSystem BeliefSystem;

        /// <summary>Mirrors Survivor.HasRadiationAnxietyStatus.</summary>
        public bool IsAnxious;

        /// <summary>Mirrors Survivor.IsNumb.</summary>
        public bool IsNumb;

        /// <summary>Medical triage pipeline (afflictions / treatments).</summary>
        public MedicalSystem MedicalSystem;

        /// <summary>Mental-break system (Prompt #29). Used by MentalBreakComfortAction
        /// to find broken survivors and by TreatPatientAction to trigger the
        /// medical-bed cure path. Null-safe: not wired in scenes that don't
        /// care about breaks.</summary>
        public MentalBreakSystem MentalBreak;

        /// <summary>Shelter electricity grid (generation, load-shed, pedaling).</summary>
        public PowerNetwork PowerNetwork;

        /// <summary>Hatch defense / raid resolution (Prompt #33).</summary>
        public HatchDefenseSystem HatchDefense;

        /// <summary>0..1 perceived raid threat (faction hostility / noise).</summary>
        public float RaidThreatLevel;

        /// <summary>Current campaign day (for post-Day 30 guard priority).</summary>
        public int CurrentDay;

        /// <summary>Indoor °C for sleep quality (wired from TemperatureSystem).</summary>
        public float IndoorTemperatureC = 15f;

        /// <summary>Preferred sleep room id (bed RoomId overrides when claimed).</summary>
        public string SleepRoomId = "quarters";

        /// <summary>Optional adjacency query for diesel noise vs sleep room.</summary>
        public Func<string, string, bool> AreRoomsAdjacent;

        /// <summary>
        /// When set, SleepActionSO uses these conditions instead of building from
        /// shelter/power (tests / scripted sleep cycles).
        /// </summary>
        public SleepConditions? SleepConditionsOverride;

        /// <summary>Bunker water cisterns (clean/dirty/irradiated), fed by the catchment + purifier.</summary>
        public WaterStorage WaterStorage;

        /// <summary>Prompt #51 — vermin infestation (HuntRats action).</summary>
        public VerminSystem VerminSystem;

        /// <summary>Prompt #50 — waste/hygiene system (latrine, disposal).</summary>
        public WasteSystem WasteSystem;

        /// <summary>Prompt #52 — jury-rigged modules (repair decisions).</summary>
        public JuryRigSystem JuryRigSystem;

        /// <summary>Prompt #49 — structural integrity (shoring priority).</summary>
        public StructuralIntegritySystem StructuralIntegrity;

        /// <summary>
        /// True when ElectronicScrap is short for a critical workbench repair
        /// (water purifier / hard-broken geiger). Scavenge actions prioritize junk.
        /// </summary>
        public bool NeedsElectronicScrapForCriticalRepair;

        /// <summary>0..1 urgency for junk scavenging when scrap is needed (from deficit).</summary>
        public float JunkScavengeUrgency;

        /// <summary>Applies radiation dose exposure (e.g. from drinking irradiated water).</summary>
        public RadiationSystem RadiationSystem;

        // --- C-3: bindings for newly wired systems (audit C-1) ---

        /// <summary>Prompt #119 — room rubble clearing (ExcavationSystem).</summary>
        public ExcavationSystem ExcavationSystem;

        /// <summary>Prompt #120 — pre-Day-30 room flooding (RoomFloodingSystem).</summary>
        public RoomFloodingSystem FloodingSystem;

        /// <summary>Prompt #121 — hidden storage stashes (HiddenStorageSystem).</summary>
        public HiddenStorageSystem HiddenStorageSystem;

        /// <summary>Prompt #122 — ceiling collapse tracker (CeilingCollapseSystem).</summary>
        public CeilingCollapseSystem CeilingCollapseSystem;

        /// <summary>Prompt #123 — perimeter traps (PerimeterTrapSystem).</summary>
        public PerimeterTrapSystem PerimeterTrapSystem;

        /// <summary>Prompt #124 — tunneling (TunnelingSystem).</summary>
        public TunnelingSystem TunnelingSystem;

        /// <summary>Prompt #125 — material shielding upgrades (MaterialShieldingSystem).</summary>
        public MaterialShieldingSystem MaterialShieldingSystem;

        /// <summary>Prompt #126 — escape hatch excavation (EscapeHatchSystem).</summary>
        public EscapeHatchSystem EscapeHatchSystem;

        /// <summary>Prompt #128 — airlock contamination gating (AirlockSystem).</summary>
        public AirlockSystem AirlockSystem;

        /// <summary>Prompt #167 — compost bin (CompostSystem).</summary>
        public CompostSystem CompostSystem;

        /// <summary>Prompt #169 — surgical sterilization (SterilizationSystem).</summary>
        public SterilizationSystem SterilizationSystem;

        /// <summary>Prompt #170 — chelation therapy (ChelationSystem).</summary>
        public ChelationSystem ChelationSystem;

        /// <summary>Prompt #171 — overworld wind turbine (WindTurbineSystem).</summary>
        public WindTurbineSystem WindTurbineSystem;

        /// <summary>Prompt #173 — internal hauling (InternalHaulingSystem).</summary>
        public InternalHaulingSystem HaulingSystem;

        /// <summary>Prompt #168 — scrap pipe guns (misfire risk).</summary>
        public ScrapWeaponSystem ScrapWeaponSystem;

        /// <summary>Prompt #174 — weapon oiling / durability.</summary>
        public WeaponMaintenanceSystem WeaponMaintenanceSystem;

        /// <summary>Prompts #182–#188 — combat milestone perks.</summary>
        public CombatPerkSystem CombatPerks;

        /// <summary>Prompts #189–#194 — survival / wasteland-digestion perks.</summary>
        public SurvivalPerkSystem SurvivalPerks;

        /// <summary>Prompts #195–#200 — shelter-engineering perks.</summary>
        public ShelterPerkSystem ShelterPerks;

        /// <summary>Prompts #201–#205 — medical milestone perks.</summary>
        public MedicalPerkSystem MedicalPerks;

        /// <summary>Prompt #177 — triage board medication permissions.</summary>
        public TriageBoardSystem TriageSystem;

        /// <summary>Prompt #166 — trauma resilience buffer.</summary>
        public ResilienceSystem ResilienceSystem;

        /// <summary>Prompt #172 — antibiotic resistance tracking.</summary>
        public AntibioticResistanceSystem AntibioticResistSystem;

        /// <summary>Prompt #67 — cartography table (Chart Map action).</summary>
        public CartographySystem CartographySystem;

        /// <summary>Prompt #64 — internal door locks (sleepwalk blocking).</summary>
        public InternalLockSystem InternalLockSystem;

        /// <summary>Prompt #66 — mentorship / teach skill sessions.</summary>
        public MentorshipSystem MentorshipSystem;

        /// <summary>Affinity matrix for mentorship scoring (from MentalBreakSystem).</summary>
        public InterpersonalAffinity Affinity;

        /// <summary>
        /// Optional: pick an uncharted intel node id for ChartMap. Injected by
        /// GameBootstrap so AI actions stay free of Core map types.
        /// </summary>
        public Func<string> GetUnchartedNodeId;

        /// <summary>All living survivors (for Treat Patient target selection).</summary>
        public Func<IReadOnlyList<Survivor>> GetSurvivors;

        public Random Random;

        /// <summary>
        /// Optional hook: start a radiation survey for this survivor. Returns true if
        /// a mission began. Injected by GameBootstrap so AI actions stay free of Core refs.
        /// </summary>
        public Func<Survivor, bool> OnRequestSurvey;

        public AIContext() { }

        public AIContext(Survivor survivor, Shelter.Shelter shelter = null, Inventory.Inventory inventory = null, Random random = null)
        {
            Survivor = survivor;
            Shelter = shelter;
            Inventory = inventory;
            Random = random;
        }
    }
}
