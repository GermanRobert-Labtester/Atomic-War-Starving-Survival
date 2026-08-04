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
        // -----------------------------------------------------------------
        // Capture snapshot
        // -----------------------------------------------------------------

        private SaveData CaptureSnapshot()
        {
            var data = new SaveData
            {
                SaveVersion = CurrentSaveVersion,
                GameState = new GameStateSave
                {
                    Phase = _gameState.Phase,
                    Day = _gameState.Day,
                    IsPaused = _gameState.IsPaused
                }
            };

            if (_weatherSystem != null)
                data.Weather = _weatherSystem.GetState();

            if (_temperatureSystem != null)
                data.ElapsedHours = _temperatureSystem.TotalElapsedHours;

            if (_getSurvivors != null)
            {
                var survivors = _getSurvivors();
                if (survivors != null)
                {
                    foreach (var sv in survivors)
                    {
                        data.Survivors.Add(CaptureSurvivor(sv));
                    }
                }
            }

            if (_shelter != null)
            {
                foreach (var mod in _shelter.Modules)
                {
                    data.ShelterModules.Add(new ShelterModuleSave
                    {
                        ModuleId = mod.ModuleId,
                        Level = mod.Level,
                        IsEnabled = mod.IsEnabled,
                        FilterHealth = mod.FilterHealth,
                        Fuel = mod.Fuel,
                        WaterConversionProgress = mod.WaterConversionProgress,
                        RoomId = mod.RoomId,
                        Occupancy = mod.Occupancy,
                        ComfortLevel = mod.ComfortLevel,
                        Capacity = mod.Capacity
                    });
                }
            }

            if (_worldFlags.Count > 0)
            {
                foreach (var kv in _worldFlags)
                {
                    data.WorldFlagKeys.Add(kv.Key);
                    data.WorldFlagValues.Add(kv.Value);
                }
            }

            if (_photoPeriodSystem != null)
                data.Photoperiod = _photoPeriodSystem.GetState();

            if (_knowledgeMap != null)
                data.RadiationKnowledge = _knowledgeMap.CaptureState();

            if (_inventory != null)
                data.Inventory = _inventory.CaptureState();

            if (_medicalSystem != null)
                data.Medical = _medicalSystem.CaptureState();

            if (_bloodTransfusion != null)
                data.BloodTransfusion = _bloodTransfusion.CaptureState();

            if (_amputationSystem != null)
                data.Amputation = _amputationSystem.CaptureState();

            if (_scurvySystem != null)
                data.Scurvy = _scurvySystem.CaptureState();

            if (_mutagenesisSystem != null)
                data.Mutagenesis = _mutagenesisSystem.CaptureState();

            if (_worldPhaseSystem != null)
                data.WorldPhase = _worldPhaseSystem.CaptureState();

            if (_economySystem != null)
                data.Economy = _economySystem.CaptureState();

            if (_powerNetwork != null)
                data.Power = _powerNetwork.CaptureState();

            if (_hatchDefense != null)
                data.HatchDefense = _hatchDefense.CaptureState();

            if (_factionRadioIntercepts != null)
                data.FactionRadioIntercepts = _factionRadioIntercepts.CaptureState();

            if (_journalSystem != null)
                data.Journal = _journalSystem.CaptureState();

            if (_victoryProject != null)
                data.VictoryProject = _victoryProject.CaptureState();

            if (_eventRunner != null)
                data.ScheduledEvents = _eventRunner.CaptureScheduledState();

            if (_suspicionTracker != null)
                data.Suspicion = _suspicionTracker.CaptureState();

            if (_hatchEntrapment != null)
                data.HatchEntrapment = _hatchEntrapment.CaptureState();

            if (_atmosphereSystem != null)
                data.Atmosphere = _atmosphereSystem.CaptureState();

            if (_corpseSystem != null)
                data.Corpses = _corpseSystem.CaptureState();

            if (_pantrySystem != null)
                data.Pantry = _pantrySystem.CaptureState();

            if (_sabotagedCaches != null)
                data.SabotagedCaches = _sabotagedCaches.CaptureState();

            if (_shiftingHotspots != null)
                data.ShiftingHotspots = _shiftingHotspots.CaptureState();

            if (_factionRaidPlans != null)
                data.FactionRaidPlans = _factionRaidPlans.CaptureState();

            if (_debtCollector != null)
                data.DebtCollector = _debtCollector.CaptureState();

            if (_ghostStations != null)
                data.GhostStations = _ghostStations.CaptureState();

            if (_lifeboat != null)
                data.Lifeboat = _lifeboat.CaptureState();

            if (_childSystem != null)
                data.ChildDependent = _childSystem.CaptureState();

            if (_structuralIntegrity != null)
                data.StructuralIntegrity = _structuralIntegrity.CaptureState();

            if (_wasteSystem != null)
                data.Waste = _wasteSystem.CaptureState();

            if (_verminSystem != null)
                data.Vermin = _verminSystem.CaptureState();

            if (_juryRigSystem != null)
                data.JuryRig = _juryRigSystem.CaptureState();

            if (_freezePipeSystem != null)
                data.FreezePipe = _freezePipeSystem.CaptureState();

            if (_cartographySystem != null)
                data.Cartography = _cartographySystem.CaptureState();

            if (_trackerSystem != null)
                data.Tracker = _trackerSystem.CaptureState();

            if (_deadDropSystem != null)
                data.DeadDrops = _deadDropSystem.CaptureState();
            if (_hostageSystem != null)
                data.Hostages = _hostageSystem.CaptureState();
            if (_propagandaSystem != null)
                data.Propaganda = _propagandaSystem.CaptureState();
            if (_deserterSystem != null)
                data.Deserters = _deserterSystem.CaptureState();
            if (_scapegoatSystem != null)
                data.Scapegoat = _scapegoatSystem.CaptureState();
            if (_laborCampSystem != null)
                data.LaborCamps = _laborCampSystem.CaptureState();
            if (_cultMoralSystem != null)
                data.CultMoral = _cultMoralSystem.CaptureState();
            if (_ecosystemSystem != null)
                data.Ecosystem = _ecosystemSystem.CaptureState();
            if (_houseToBunkerSystem != null)
                data.HouseToBunker = _houseToBunkerSystem.CaptureState();
            if (_locationQuestSystem != null)
                data.LocationQuests = _locationQuestSystem.CaptureState();
            if (_excavationSystem != null) data.Excavation = _excavationSystem.CaptureState();
            if (_floodingSystem != null) data.Flooding = _floodingSystem.CaptureState();
            if (_hiddenStorageSystem != null) data.HiddenStorage = _hiddenStorageSystem.CaptureState();
            if (_ceilingCollapseSystem != null) data.CeilingCollapse = _ceilingCollapseSystem.CaptureState();
            if (_perimeterTrapSystem != null) data.PerimeterTraps = _perimeterTrapSystem.CaptureState();
            if (_tunnelingSystem != null) data.Tunneling = _tunnelingSystem.CaptureState();
            if (_hatchVisibilitySystem != null) data.HatchVisibility = _hatchVisibilitySystem.CaptureState();
            if (_escapeHatchSystem != null) data.EscapeHatch = _escapeHatchSystem.CaptureState();
            if (_materialShieldingSystem != null) data.MaterialShielding = _materialShieldingSystem.CaptureState();
            if (_airlockSystem != null) data.Airlock = _airlockSystem.CaptureState();
            if (_noiseSystem != null) data.Noise = _noiseSystem.CaptureState();
            if (_resilienceSystem != null) data.Resilience = _resilienceSystem.CaptureState();
            if (_compostSystem != null) data.Compost = _compostSystem.CaptureState();
            if (_sterilizationSystem != null) data.Sterilization = _sterilizationSystem.CaptureState();
            if (_chelationSystem != null) data.Chelation = _chelationSystem.CaptureState();
            if (_windTurbineSystem != null) data.WindTurbine = _windTurbineSystem.CaptureState();
            if (_antibioticResistSystem != null) data.AntibioticResist = _antibioticResistSystem.CaptureState();
            if (_haulingSystem != null) data.Hauling = _haulingSystem.CaptureState();
            if (_weaponMaintenanceSystem != null) data.WeaponMaint = _weaponMaintenanceSystem.CaptureState();
            if (_aestheticsSystem != null) data.Aesthetics = _aestheticsSystem.CaptureState();
            if (_hamRadioSystem != null) data.HamRadio = _hamRadioSystem.CaptureState();
            if (_triageSystem != null) data.Triage = _triageSystem.CaptureState();
            if (_polypharmacySystem != null) data.Polypharmacy = _polypharmacySystem.CaptureState();

            // H-4: Capture all ISaveable-registered subsystems into paired lists.
            CaptureSubsystemStates(data);

            if (_generatedMap != null)
                data.GeneratedMap = _generatedMap.CaptureState();

            if (_waterStorage != null)
                data.Water = _waterStorage.CaptureState();

            if (_shelter != null)
                data.BunkerContamination = _shelter.BunkerContamination;

            if (_mentalBreakSystem != null)
            {
                var affSave = new AffinityMatrixSave();
                affSave.Entries.AddRange(_mentalBreakSystem.Affinity.Snapshot());
                data.Affinity = affSave;
            }

            if (_captureChoreographer != null)
                data.FlashpointChoreographer = _captureChoreographer();

            if (_expeditionSystem != null && _expeditionSystem.ActiveExpeditions != null)
            {
                foreach (var exp in _expeditionSystem.ActiveExpeditions)
                {
                    if (exp == null) continue;
                    var saveExp = new ExpeditionSaveState
                    {
                        ExpeditionId = exp.ExpeditionId,
                        SurvivorId = exp.SurvivorId,
                        TargetLocationId = exp.TargetLocationId,
                        TargetLocationName = exp.TargetLocationName,
                        Stance = exp.Stance,
                        Phase = exp.Phase,
                        CurrentTick = exp.CurrentTick,
                        TotalDistanceTicks = exp.TotalDistanceTicks,
                        TravelTicksCompleted = exp.TravelTicksCompleted,
                        LootingTicksCompleted = exp.LootingTicksCompleted,
                        CarryingCapacity = exp.CarryingCapacity,
                        CurrentWeight = exp.CurrentWeight,
                        Stamina = exp.Stamina,
                        SuitDegradation = exp.SuitDegradation,
                        TrueRadPerHour = exp.TrueRadPerHour,
                        DangerLevel = exp.DangerLevel,
                        IsPushingLuck = exp.IsPushingLuck,
                        IsRetreating = exp.IsRetreating,
                        LocationEncounterFired = exp.LocationEncounterFired,
                        UxoDetonated = exp.UxoDetonated,
                        HasBicycle = exp.HasBicycle,
                        BicycleDurability = exp.BicycleDurability,
                        IsWading = exp.IsWading,
                        IsNightScavenge = exp.IsNightScavenge,
                        HasFlashlight = exp.HasFlashlight,
                        FlashlightBattery = exp.FlashlightBattery
                    };
                    if (exp.CollectedLootItemIds != null)
                    {
                        saveExp.CollectedLootItemIds.AddRange(exp.CollectedLootItemIds);
                    }
                    data.Expeditions.Add(saveExp);
                }
            }

            // H-4: ISaveable subsystem states captured above in CaptureSubsystemStates.

            // PhantomIntruder cooldowns (survivorId → remaining hours)
            if (_phantomIntruderSystem != null && _phantomIntruderSystem.Cooldowns != null)
            {
                foreach (var kv in _phantomIntruderSystem.Cooldowns)
                {
                    if (kv.Value > 0f)
                    {
                        data.PhantomCooldownKeys.Add(kv.Key);
                        data.PhantomCooldownValues.Add(kv.Value);
                    }
                }
            }

            // ShelterRoom unlock + rubble state (Prompt #5)
            if (_shelter != null && _shelter.Rooms != null)
            {
                foreach (var room in _shelter.Rooms)
                {
                    if (room == null || string.IsNullOrEmpty(room.RoomId)) continue;
                    data.ShelterRooms.Add(new ShelterRoomSave
                    {
                        RoomId = room.RoomId,
                        UnlockState = (int)room.UnlockState,
                        RubbleClearHoursRemaining = room.RubbleClearHoursRemaining,
                        RubbleClearHoursTotal = room.RubbleClearHoursTotal,
                        DiaryFragmentIds = room.DiaryFragmentIds != null
                            ? new List<string>(room.DiaryFragmentIds)
                            : new List<string>(),
                        RevealedDiaryIndices = room.RevealedDiaryIndices != null
                            ? new List<int>(room.RevealedDiaryIndices)
                            : new List<int>()
                    });
                }
            }

            return data;
        }

    }
}
