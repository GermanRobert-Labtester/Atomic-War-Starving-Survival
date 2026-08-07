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
            // Positional only when present (pre-migration). New saves restore via SubsystemSaveIds.
            RestIf(_worldPhaseSystem, s => { if (data.WorldPhase != null) s.RestoreState(data.WorldPhase); });
            RestIf(_economySystem, s => { if (data.Economy != null) s.RestoreState(data.Economy); });
            RestIf(_powerNetwork, s =>
            {
                if (data.Power == null) return;
                s.RestoreState(data.Power);
                s.ApplyToShelter(_shelter);
            });
            RestIf(_hatchDefense, s => { if (data.HatchDefense != null) s.RestoreState(data.HatchDefense); });
            RestIf(_factionRadioIntercepts, s =>
            {
                if (data.FactionRadioIntercepts != null) s.RestoreState(data.FactionRadioIntercepts);
            });
            RestIf(_journalSystem, s => { if (data.Journal != null) s.RestoreState(data.Journal); });
            RestIf(_victoryProject, s => { if (data.VictoryProject != null) s.RestoreState(data.VictoryProject); });
            // EventRunner remains special-path positional restore.
            RestIf(_eventRunner, s => s.RestoreScheduledState(data.ScheduledEvents));
            RestIf(_suspicionTracker, s => { if (data.Suspicion != null) s.RestoreState(data.Suspicion); });
            RestIf(_hatchEntrapment, s => { if (data.HatchEntrapment != null) s.RestoreState(data.HatchEntrapment); });
            RestIf(_atmosphereSystem, s => { if (data.Atmosphere != null) s.RestoreState(data.Atmosphere); });
            RestIf(_pantrySystem, s => { if (data.Pantry != null) s.RestoreState(data.Pantry); });
            RestIf(_sabotagedCaches, s => { if (data.SabotagedCaches != null) s.RestoreState(data.SabotagedCaches); });
            if (_generatedMap != null && data.GeneratedMap != null)
                RestoreGeneratedMap(data.GeneratedMap);
        }

        private void RestoreFactionSideSystems(SaveData data)
        {
            // Complex specials: Bind/SetMap before restore.
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
            RestIf(_debtCollector, s => { if (data.DebtCollector != null) s.RestoreState(data.DebtCollector); });
            RestIf(_ghostStations, s => { if (data.GhostStations != null) s.RestoreState(data.GhostStations); });
            RestIf(_lifeboat, s => { if (data.Lifeboat != null) s.RestoreState(data.Lifeboat); });
            // Migrated dual-path systems — positional only when present.
            RestIf(_trackerSystem, s => { if (data.Tracker != null) s.RestoreState(data.Tracker); });
            RestIf(_deadDropSystem, s => { if (data.DeadDrops != null) s.RestoreState(data.DeadDrops); });
            RestIf(_hostageSystem, s => { if (data.Hostages != null) s.RestoreState(data.Hostages); });
            RestIf(_propagandaSystem, s => { if (data.Propaganda != null) s.RestoreState(data.Propaganda); });
            RestIf(_deserterSystem, s => { if (data.Deserters != null) s.RestoreState(data.Deserters); });
            RestIf(_scapegoatSystem, s => { if (data.Scapegoat != null) s.RestoreState(data.Scapegoat); });
            RestIf(_laborCampSystem, s => { if (data.LaborCamps != null) s.RestoreState(data.LaborCamps); });
            RestIf(_cultMoralSystem, s => { if (data.CultMoral != null) s.RestoreState(data.CultMoral); });
        }

        private void RestoreNarrativeSideSystems(SaveData data)
        {
            // Positional only when present (pre-migration saves). New saves restore
            // via SubsystemSaveIds (RegisterSystem adapters).
            RestIf(_cartographySystem, s => { if (data.Cartography != null) s.RestoreState(data.Cartography); });
            RestIf(_floodedNodeSystem, s => { if (data.FloodedNodes != null) s.RestoreState(data.FloodedNodes); });
            RestIf(_ecosystemSystem, s => { if (data.Ecosystem != null) s.RestoreState(data.Ecosystem); });
            RestIf(_houseToBunkerSystem, s => { if (data.HouseToBunker != null) s.RestoreState(data.HouseToBunker); });
            RestIf(_locationQuestSystem, s => { if (data.LocationQuests != null) s.RestoreState(data.LocationQuests); });
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
            // Positional only when present (pre-migration saves).
            RestIf(_structuralIntegrity, s => { if (data.StructuralIntegrity != null) s.RestoreState(data.StructuralIntegrity); });
            RestIf(_wasteSystem, s => { if (data.Waste != null) s.RestoreState(data.Waste); });
            RestIf(_verminSystem, s => { if (data.Vermin != null) s.RestoreState(data.Vermin); });
            RestIf(_juryRigSystem, s => { if (data.JuryRig != null) s.RestoreState(data.JuryRig); });
            RestIf(_freezePipeSystem, s => { if (data.FreezePipe != null) s.RestoreState(data.FreezePipe); });
            RestIf(_excavationSystem, s => { if (data.Excavation != null) s.RestoreState(data.Excavation); });
            RestIf(_floodingSystem, s => { if (data.Flooding != null) s.RestoreState(data.Flooding); });
            RestIf(_hiddenStorageSystem, s => { if (data.HiddenStorage != null) s.RestoreState(data.HiddenStorage); });
            RestIf(_ceilingCollapseSystem, s => { if (data.CeilingCollapse != null) s.RestoreState(data.CeilingCollapse); });
            RestIf(_perimeterTrapSystem, s => { if (data.PerimeterTraps != null) s.RestoreState(data.PerimeterTraps); });
            RestIf(_tunnelingSystem, s => { if (data.Tunneling != null) s.RestoreState(data.Tunneling); });
            RestIf(_hatchVisibilitySystem, s => { if (data.HatchVisibility != null) s.RestoreState(data.HatchVisibility); });
            RestIf(_escapeHatchSystem, s => { if (data.EscapeHatch != null) s.RestoreState(data.EscapeHatch); });
            RestIf(_materialShieldingSystem, s => { if (data.MaterialShielding != null) s.RestoreState(data.MaterialShielding); });
            RestIf(_airlockSystem, s => { if (data.Airlock != null) s.RestoreState(data.Airlock); });
            RestIf(_noiseSystem, s => { if (data.Noise != null) s.RestoreState(data.Noise); });
        }

        private void RestoreSimulationExtras(SaveData data)
        {
            // Positional only when present (pre-migration). New saves restore via SubsystemSaveIds.
            RestIf(_resilienceSystem, s => { if (data.Resilience != null) s.RestoreState(data.Resilience); });
            RestIf(_compostSystem, s => { if (data.Compost != null) s.RestoreState(data.Compost); });
            RestIf(_windTurbineSystem, s => { if (data.WindTurbine != null) s.RestoreState(data.WindTurbine); });
            RestIf(_haulingSystem, s => { if (data.Hauling != null) s.RestoreState(data.Hauling); });
            RestIf(_weaponMaintenanceSystem, s => { if (data.WeaponMaint != null) s.RestoreState(data.WeaponMaint); });
            RestIf(_aestheticsSystem, s => { if (data.Aesthetics != null) s.RestoreState(data.Aesthetics); });
            RestIf(_hamRadioSystem, s => { if (data.HamRadio != null) s.RestoreState(data.HamRadio); });
            if (_skillProgression != null && data.SkillProgression != null)
                _skillProgression.RestoreState(data.SkillProgression, _getSurvivors?.Invoke());
            RestIf(_combatPerkSystem, s => { if (data.CombatPerks != null) s.RestoreState(data.CombatPerks); });
            RestIf(_survivalPerkSystem, s => { if (data.SurvivalPerks != null) s.RestoreState(data.SurvivalPerks); });
            RestIf(_shelterPerkSystem, s => { if (data.ShelterPerks != null) s.RestoreState(data.ShelterPerks); });
            RestIf(_medicalPerkSystem, s => { if (data.MedicalPerks != null) s.RestoreState(data.MedicalPerks); });
            RestIf(_expeditionPerkSystem, s => { if (data.ExpeditionPerks != null) s.RestoreState(data.ExpeditionPerks); });
            RestIf(_socialPerkSystem, s => { if (data.SocialPerks != null) s.RestoreState(data.SocialPerks); });
            RestIf(_personalQuestSystem, s => { if (data.PersonalQuests != null) s.RestoreState(data.PersonalQuests); });
        }

    }
}
