using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
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

        /// <summary>
        /// True when ElectronicScrap is short for a critical workbench repair
        /// (water purifier / hard-broken geiger). Scavenge actions prioritize junk.
        /// </summary>
        public bool NeedsElectronicScrapForCriticalRepair;

        /// <summary>0..1 urgency for junk scavenging when scrap is needed (from deficit).</summary>
        public float JunkScavengeUrgency;

        /// <summary>Applies radiation dose exposure (e.g. from drinking irradiated water).</summary>
        public RadiationSystem RadiationSystem;

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
