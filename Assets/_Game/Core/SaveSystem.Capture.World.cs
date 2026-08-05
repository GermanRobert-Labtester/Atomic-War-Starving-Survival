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
using AtomicWar._Game.Simulation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Events;

namespace AtomicWar._Game.Core
{
    public partial class SaveSystem
    {
        /// <summary>Capture when system is non-null; collapses repeated if-null chains.</summary>
        private static void CapIf<T>(T system, Action<T> capture) where T : class
        {
            if (system != null) capture(system);
        }

        private void CaptureWorldAndFactionSystems(SaveData data)
        {
            // Core world / economy / narrative bookkeeping
            CapIf(_worldPhaseSystem, s => data.WorldPhase = s.CaptureState());
            CapIf(_economySystem, s => data.Economy = s.CaptureState());
            CapIf(_powerNetwork, s => data.Power = s.CaptureState());
            CapIf(_hatchDefense, s => data.HatchDefense = s.CaptureState());
            CapIf(_factionRadioIntercepts, s => data.FactionRadioIntercepts = s.CaptureState());
            CapIf(_journalSystem, s => data.Journal = s.CaptureState());
            CapIf(_victoryProject, s => data.VictoryProject = s.CaptureState());
            CapIf(_eventRunner, s => data.ScheduledEvents = s.CaptureScheduledState());
            CapIf(_suspicionTracker, s => data.Suspicion = s.CaptureState());
            CapIf(_hatchEntrapment, s => data.HatchEntrapment = s.CaptureState());
            CapIf(_atmosphereSystem, s => data.Atmosphere = s.CaptureState());
            CapIf(_pantrySystem, s => data.Pantry = s.CaptureState());
            CapIf(_sabotagedCaches, s => data.SabotagedCaches = s.CaptureState());

            // Faction side systems (raids, debt, ghosts, camps)
            CapIf(_shiftingHotspots, s => data.ShiftingHotspots = s.CaptureState());
            CapIf(_factionRaidPlans, s => data.FactionRaidPlans = s.CaptureState());
            CapIf(_debtCollector, s => data.DebtCollector = s.CaptureState());
            CapIf(_ghostStations, s => data.GhostStations = s.CaptureState());
            CapIf(_lifeboat, s => data.Lifeboat = s.CaptureState());
            CapIf(_trackerSystem, s => data.Tracker = s.CaptureState());
            CapIf(_deadDropSystem, s => data.DeadDrops = s.CaptureState());
            CapIf(_hostageSystem, s => data.Hostages = s.CaptureState());
            CapIf(_propagandaSystem, s => data.Propaganda = s.CaptureState());
            CapIf(_deserterSystem, s => data.Deserters = s.CaptureState());
            CapIf(_scapegoatSystem, s => data.Scapegoat = s.CaptureState());
            CapIf(_laborCampSystem, s => data.LaborCamps = s.CaptureState());
            CapIf(_cultMoralSystem, s => data.CultMoral = s.CaptureState());

            // Narrative side systems
            CapIf(_cartographySystem, s => data.Cartography = s.CaptureState());
            CapIf(_floodedNodeSystem, s => data.FloodedNodes = s.CaptureState());
            CapIf(_ecosystemSystem, s => data.Ecosystem = s.CaptureState());
            CapIf(_houseToBunkerSystem, s => data.HouseToBunker = s.CaptureState());
            CapIf(_locationQuestSystem, s => data.LocationQuests = s.CaptureState());
        }

        private void CaptureShelterTacticalSystems(SaveData data)
        {
            CapIf(_structuralIntegrity, s => data.StructuralIntegrity = s.CaptureState());
            CapIf(_wasteSystem, s => data.Waste = s.CaptureState());
            CapIf(_verminSystem, s => data.Vermin = s.CaptureState());
            CapIf(_juryRigSystem, s => data.JuryRig = s.CaptureState());
            CapIf(_freezePipeSystem, s => data.FreezePipe = s.CaptureState());
            CapIf(_excavationSystem, s => data.Excavation = s.CaptureState());
            CapIf(_floodingSystem, s => data.Flooding = s.CaptureState());
            CapIf(_hiddenStorageSystem, s => data.HiddenStorage = s.CaptureState());
            CapIf(_ceilingCollapseSystem, s => data.CeilingCollapse = s.CaptureState());
            CapIf(_perimeterTrapSystem, s => data.PerimeterTraps = s.CaptureState());
            CapIf(_tunnelingSystem, s => data.Tunneling = s.CaptureState());
            CapIf(_hatchVisibilitySystem, s => data.HatchVisibility = s.CaptureState());
            CapIf(_escapeHatchSystem, s => data.EscapeHatch = s.CaptureState());
            CapIf(_materialShieldingSystem, s => data.MaterialShielding = s.CaptureState());
            CapIf(_airlockSystem, s => data.Airlock = s.CaptureState());
            CapIf(_noiseSystem, s => data.Noise = s.CaptureState());
        }

        private void CaptureSimulationExtras(SaveData data)
        {
            CapIf(_resilienceSystem, s => data.Resilience = s.CaptureState());
            CapIf(_compostSystem, s => data.Compost = s.CaptureState());
            CapIf(_windTurbineSystem, s => data.WindTurbine = s.CaptureState());
            CapIf(_haulingSystem, s => data.Hauling = s.CaptureState());
            CapIf(_weaponMaintenanceSystem, s => data.WeaponMaint = s.CaptureState());
            CapIf(_aestheticsSystem, s => data.Aesthetics = s.CaptureState());
            CapIf(_hamRadioSystem, s => data.HamRadio = s.CaptureState());
            CapIf(_skillProgression, s => data.SkillProgression = s.CaptureState());
            CapIf(_combatPerkSystem, s => data.CombatPerks = s.CaptureState());
        }
    }
}
