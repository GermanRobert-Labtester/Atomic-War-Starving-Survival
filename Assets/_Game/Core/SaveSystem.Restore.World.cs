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
        /// <summary>Restore when system is non-null; collapses repeated if-null chains.</summary>
        private static void RestIf<T>(T system, Action<T> restore) where T : class
        {
            if (system != null) restore(system);
        }

        private void RestoreWorldAndFactionSystems(SaveData data)
        {
            RestoreCoreWorldSystems(data);
            RestoreFactionSideSystems(data);
            RestoreNarrativeSideSystems(data);
        }

        private void RestoreCoreWorldSystems(SaveData data)
        {
            RestIf(_worldPhaseSystem, s => { if (data.WorldPhase != null) s.RestoreState(data.WorldPhase); });
            RestIf(_economySystem, s => { if (data.Economy != null) s.RestoreState(data.Economy); });
            RestIf(_powerNetwork, s =>
            {
                if (data.Power == null) return;
                s.RestoreState(data.Power);
                s.ApplyToShelter(_shelter);
            });
            RestIf(_hatchDefense, s => { if (data.HatchDefense != null) s.RestoreState(data.HatchDefense); });
            RestIf(_factionRadioIntercepts, s => s.RestoreState(data.FactionRadioIntercepts));
            RestIf(_journalSystem, s => s.RestoreState(data.Journal));
            RestIf(_victoryProject, s => s.RestoreState(data.VictoryProject));
            RestIf(_eventRunner, s => s.RestoreScheduledState(data.ScheduledEvents));
            RestIf(_suspicionTracker, s => s.RestoreState(data.Suspicion));
            RestIf(_hatchEntrapment, s => s.RestoreState(data.HatchEntrapment));
            RestIf(_atmosphereSystem, s => { if (data.Atmosphere != null) s.RestoreState(data.Atmosphere); });
            RestIf(_pantrySystem, s => { if (data.Pantry != null) s.RestoreState(data.Pantry); });
            RestIf(_sabotagedCaches, s => s.RestoreState(data.SabotagedCaches));
            if (_generatedMap != null && data.GeneratedMap != null)
                RestoreGeneratedMap(data.GeneratedMap);
        }

        private void RestoreFactionSideSystems(SaveData data)
        {
            RestIf(_shiftingHotspots, s =>
            {
                s.Bind(_generatedMap, _knowledgeMap);
                s.RestoreState(data.ShiftingHotspots);
            });
            RestIf(_factionRaidPlans, s =>
            {
                s.SetMap(_generatedMap);
                s.RestoreState(data.FactionRaidPlans);
            });
            RestIf(_debtCollector, s => s.RestoreState(data.DebtCollector));
            RestIf(_ghostStations, s => s.RestoreState(data.GhostStations));
            RestIf(_lifeboat, s => s.RestoreState(data.Lifeboat));
            RestIf(_trackerSystem, s => s.RestoreState(data.Tracker));
            RestIf(_deadDropSystem, s => s.RestoreState(data.DeadDrops));
            RestIf(_hostageSystem, s => s.RestoreState(data.Hostages));
            RestIf(_propagandaSystem, s => s.RestoreState(data.Propaganda));
            RestIf(_deserterSystem, s => s.RestoreState(data.Deserters));
            RestIf(_scapegoatSystem, s => s.RestoreState(data.Scapegoat));
            RestIf(_laborCampSystem, s => s.RestoreState(data.LaborCamps));
            RestIf(_cultMoralSystem, s => s.RestoreState(data.CultMoral));
        }

        private void RestoreNarrativeSideSystems(SaveData data)
        {
            RestIf(_cartographySystem, s => s.RestoreState(data.Cartography));
            RestIf(_floodedNodeSystem, s => s.RestoreState(data.FloodedNodes));
            RestIf(_ecosystemSystem, s => s.RestoreState(data.Ecosystem));
            RestIf(_houseToBunkerSystem, s => s.RestoreState(data.HouseToBunker));
            RestIf(_locationQuestSystem, s => s.RestoreState(data.LocationQuests));
        }

        private void RestoreGeneratedMap(GeneratedMapSave mapSave)
        {
            if (_generatedMap.Seed != mapSave.Seed)
            {
                var rebuilt = MapGenerator.Generate(mapSave.Seed);
                _generatedMap.Seed = rebuilt.Seed;
                _generatedMap.Nodes = rebuilt.Nodes;
                _generatedMap.Paths = rebuilt.Paths;
            }
            _generatedMap.RestoreRevealState(mapSave);
        }

        private void RestoreShelterTacticalSystems(SaveData data)
        {
            RestIf(_structuralIntegrity, s => s.RestoreState(data.StructuralIntegrity));
            RestIf(_wasteSystem, s => s.RestoreState(data.Waste));
            RestIf(_verminSystem, s => s.RestoreState(data.Vermin));
            RestIf(_juryRigSystem, s => s.RestoreState(data.JuryRig));
            RestIf(_freezePipeSystem, s => s.RestoreState(data.FreezePipe));
            RestIf(_excavationSystem, s => s.RestoreState(data.Excavation));
            RestIf(_floodingSystem, s => s.RestoreState(data.Flooding));
            RestIf(_hiddenStorageSystem, s => s.RestoreState(data.HiddenStorage));
            RestIf(_ceilingCollapseSystem, s => s.RestoreState(data.CeilingCollapse));
            RestIf(_perimeterTrapSystem, s => s.RestoreState(data.PerimeterTraps));
            RestIf(_tunnelingSystem, s => s.RestoreState(data.Tunneling));
            RestIf(_hatchVisibilitySystem, s => s.RestoreState(data.HatchVisibility));
            RestIf(_escapeHatchSystem, s => s.RestoreState(data.EscapeHatch));
            RestIf(_materialShieldingSystem, s => s.RestoreState(data.MaterialShielding));
            RestIf(_airlockSystem, s => s.RestoreState(data.Airlock));
            RestIf(_noiseSystem, s => s.RestoreState(data.Noise));
        }

        private void RestoreSimulationExtras(SaveData data)
        {
            RestIf(_resilienceSystem, s => s.RestoreState(data.Resilience));
            RestIf(_compostSystem, s => s.RestoreState(data.Compost));
            RestIf(_windTurbineSystem, s => s.RestoreState(data.WindTurbine));
            RestIf(_haulingSystem, s => s.RestoreState(data.Hauling));
            RestIf(_weaponMaintenanceSystem, s => s.RestoreState(data.WeaponMaint));
            RestIf(_aestheticsSystem, s => s.RestoreState(data.Aesthetics));
            RestIf(_hamRadioSystem, s => s.RestoreState(data.HamRadio));
            if (_skillProgression != null && data.SkillProgression != null)
                _skillProgression.RestoreState(data.SkillProgression, _getSurvivors?.Invoke());
            RestIf(_combatPerkSystem, s => s.RestoreState(data.CombatPerks));
            RestIf(_survivalPerkSystem, s => s.RestoreState(data.SurvivalPerks));
            RestIf(_shelterPerkSystem, s => s.RestoreState(data.ShelterPerks));
            RestIf(_medicalPerkSystem, s => s.RestoreState(data.MedicalPerks));
            RestIf(_expeditionPerkSystem, s => s.RestoreState(data.ExpeditionPerks));
        }

    }
}
