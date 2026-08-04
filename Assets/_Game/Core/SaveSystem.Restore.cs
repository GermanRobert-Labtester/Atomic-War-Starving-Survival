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
        // Restore from snapshot
        // -----------------------------------------------------------------

        private void RestoreFromSnapshot(SaveData data)
        {
            _gameState.Day = data.GameState.Day;
            _gameState.Phase = data.GameState.Phase;
            _gameState.IsPaused = data.GameState.IsPaused;

            if (_weatherSystem != null && data.Weather != null)
            {
                _weatherSystem.RestoreState(data.Weather);
            }

            if (_temperatureSystem != null)
            {
                _temperatureSystem.SetElapsedHours(data.ElapsedHours);
            }

            // Survivors
            var existing = _getSurvivors?.Invoke();
            if (existing != null && data.Survivors != null)
            {
                for (int i = 0; i < data.Survivors.Count; i++)
                {
                    Survivor sv = i < existing.Count ? existing[i] : null;
                    if (sv == null) continue;
                    RestoreSurvivor(sv, data.Survivors[i]);
                }
            }

            // Shelter modules
            if (_shelter != null && data.ShelterModules != null)
            {
                RestoreShelterModules(data.ShelterModules);
            }

            // Dosimeters
            if (_radiationSystem != null && data.Survivors != null)
            {
                RestoreDosimeters(data.Survivors);
            }

            // World flags
            _worldFlags.Clear();
            if (data.WorldFlagKeys != null && data.WorldFlagValues != null)
            {
                int count = Mathf.Min(data.WorldFlagKeys.Count, data.WorldFlagValues.Count);
                for (int i = 0; i < count; i++)
                {
                    _worldFlags[data.WorldFlagKeys[i]] = data.WorldFlagValues[i];
                }
            }

            if (_photoPeriodSystem != null && data.Photoperiod != null)
            {
                _photoPeriodSystem.RestoreState(data.Photoperiod);
            }

            if (_knowledgeMap != null && data.RadiationKnowledge != null)
            {
                _knowledgeMap.RestoreState(data.RadiationKnowledge);
            }

            if (_inventory != null && data.Inventory != null && _itemLookup != null)
            {
                _inventory.RestoreState(data.Inventory, _itemLookup);
            }

            if (_medicalSystem != null && data.Medical != null)
            {
                _medicalSystem.RestoreState(data.Medical);
            }

            if (_bloodTransfusion != null)
                _bloodTransfusion.RestoreState(data.BloodTransfusion);

            if (_amputationSystem != null)
                _amputationSystem.RestoreState(data.Amputation);

            if (_scurvySystem != null)
                _scurvySystem.RestoreState(data.Scurvy);

            if (_mutagenesisSystem != null)
                _mutagenesisSystem.RestoreState(data.Mutagenesis);

            if (_worldPhaseSystem != null && data.WorldPhase != null)
            {
                _worldPhaseSystem.RestoreState(data.WorldPhase);
            }

            if (_economySystem != null && data.Economy != null)
            {
                _economySystem.RestoreState(data.Economy);
            }

            if (_powerNetwork != null && data.Power != null)
            {
                _powerNetwork.RestoreState(data.Power);
                _powerNetwork.ApplyToShelter(_shelter);
            }

            if (_hatchDefense != null && data.HatchDefense != null)
            {
                _hatchDefense.RestoreState(data.HatchDefense);
            }

            if (_factionRadioIntercepts != null)
            {
                // Null snapshot (pre-feature saves) clears to empty log.
                _factionRadioIntercepts.RestoreState(data.FactionRadioIntercepts);
            }

            if (_journalSystem != null)
            {
                // Null journal on legacy saves resets empty (no re-fire of OnEntryAdded).
                _journalSystem.RestoreState(data.Journal);
            }

            if (_victoryProject != null)
            {
                // Null victory on legacy saves resets to Ongoing.
                _victoryProject.RestoreState(data.VictoryProject);
            }

            if (_eventRunner != null)
            {
                // Null queue on legacy saves clears scheduled narrative chains.
                _eventRunner.RestoreScheduledState(data.ScheduledEvents);
            }

            if (_suspicionTracker != null)
                _suspicionTracker.RestoreState(data.Suspicion);

            if (_hatchEntrapment != null)
                _hatchEntrapment.RestoreState(data.HatchEntrapment);

            if (_atmosphereSystem != null && data.Atmosphere != null)
                _atmosphereSystem.RestoreState(data.Atmosphere);

            if (_corpseSystem != null && data.Corpses != null)
                _corpseSystem.RestoreState(data.Corpses);

            if (_pantrySystem != null && data.Pantry != null)
                _pantrySystem.RestoreState(data.Pantry);

            if (_sabotagedCaches != null)
                _sabotagedCaches.RestoreState(data.SabotagedCaches);

            if (_generatedMap != null && data.GeneratedMap != null)
            {
                // Layout is pure seed; regenerate if seed differs, then re-apply fog flags.
                if (_generatedMap.Seed != data.GeneratedMap.Seed)
                {
                    var rebuilt = MapGenerator.Generate(data.GeneratedMap.Seed);
                    _generatedMap.Seed = rebuilt.Seed;
                    _generatedMap.Nodes = rebuilt.Nodes;
                    _generatedMap.Paths = rebuilt.Paths;
                }
                _generatedMap.RestoreRevealState(data.GeneratedMap);
            }

            // Prompt #14 — re-apply windstorm rad migrations after seed layout is restored.
            if (_shiftingHotspots != null)
            {
                _shiftingHotspots.Bind(_generatedMap, _knowledgeMap);
                _shiftingHotspots.RestoreState(data.ShiftingHotspots);
            }

            if (_factionRaidPlans != null)
            {
                _factionRaidPlans.SetMap(_generatedMap);
                _factionRaidPlans.RestoreState(data.FactionRaidPlans);
            }

            if (_debtCollector != null)
                _debtCollector.RestoreState(data.DebtCollector);

            if (_ghostStations != null)
                _ghostStations.RestoreState(data.GhostStations);

            if (_lifeboat != null)
                _lifeboat.RestoreState(data.Lifeboat);

            if (_childSystem != null)
            {
                var survivors = _getSurvivors?.Invoke();
                _childSystem.RestoreState(data.ChildDependent, survivors);
            }

            if (_structuralIntegrity != null)
                _structuralIntegrity.RestoreState(data.StructuralIntegrity);

            if (_wasteSystem != null)
                _wasteSystem.RestoreState(data.Waste);

            if (_verminSystem != null)
                _verminSystem.RestoreState(data.Vermin);

            if (_juryRigSystem != null)
                _juryRigSystem.RestoreState(data.JuryRig);

            if (_freezePipeSystem != null)
                _freezePipeSystem.RestoreState(data.FreezePipe);

            if (_cartographySystem != null)
                _cartographySystem.RestoreState(data.Cartography);

            if (_trackerSystem != null)
                _trackerSystem.RestoreState(data.Tracker);

            if (_deadDropSystem != null)
                _deadDropSystem.RestoreState(data.DeadDrops);
            if (_hostageSystem != null)
                _hostageSystem.RestoreState(data.Hostages);
            if (_propagandaSystem != null)
                _propagandaSystem.RestoreState(data.Propaganda);
            if (_deserterSystem != null)
                _deserterSystem.RestoreState(data.Deserters);
            if (_scapegoatSystem != null)
                _scapegoatSystem.RestoreState(data.Scapegoat);
            if (_laborCampSystem != null)
                _laborCampSystem.RestoreState(data.LaborCamps);
            if (_cultMoralSystem != null)
                _cultMoralSystem.RestoreState(data.CultMoral);
            if (_ecosystemSystem != null)
                _ecosystemSystem.RestoreState(data.Ecosystem);
            if (_houseToBunkerSystem != null)
                _houseToBunkerSystem.RestoreState(data.HouseToBunker);
            if (_locationQuestSystem != null)
                _locationQuestSystem.RestoreState(data.LocationQuests);
            if (_excavationSystem != null) _excavationSystem.RestoreState(data.Excavation);
            if (_floodingSystem != null) _floodingSystem.RestoreState(data.Flooding);
            if (_hiddenStorageSystem != null) _hiddenStorageSystem.RestoreState(data.HiddenStorage);
            if (_ceilingCollapseSystem != null) _ceilingCollapseSystem.RestoreState(data.CeilingCollapse);
            if (_perimeterTrapSystem != null) _perimeterTrapSystem.RestoreState(data.PerimeterTraps);
            if (_tunnelingSystem != null) _tunnelingSystem.RestoreState(data.Tunneling);
            if (_hatchVisibilitySystem != null) _hatchVisibilitySystem.RestoreState(data.HatchVisibility);
            if (_escapeHatchSystem != null) _escapeHatchSystem.RestoreState(data.EscapeHatch);
            if (_materialShieldingSystem != null) _materialShieldingSystem.RestoreState(data.MaterialShielding);
            if (_airlockSystem != null) _airlockSystem.RestoreState(data.Airlock);
            if (_noiseSystem != null) _noiseSystem.RestoreState(data.Noise);
            if (_resilienceSystem != null) _resilienceSystem.RestoreState(data.Resilience);
            if (_compostSystem != null) _compostSystem.RestoreState(data.Compost);
            if (_sterilizationSystem != null) _sterilizationSystem.RestoreState(data.Sterilization);
            if (_chelationSystem != null) _chelationSystem.RestoreState(data.Chelation);
            if (_windTurbineSystem != null) _windTurbineSystem.RestoreState(data.WindTurbine);
            if (_antibioticResistSystem != null) _antibioticResistSystem.RestoreState(data.AntibioticResist);
            if (_haulingSystem != null) _haulingSystem.RestoreState(data.Hauling);
            if (_weaponMaintenanceSystem != null) _weaponMaintenanceSystem.RestoreState(data.WeaponMaint);
            if (_aestheticsSystem != null) _aestheticsSystem.RestoreState(data.Aesthetics);
            if (_hamRadioSystem != null) _hamRadioSystem.RestoreState(data.HamRadio);
            if (_triageSystem != null) _triageSystem.RestoreState(data.Triage);
            if (_polypharmacySystem != null) _polypharmacySystem.RestoreState(data.Polypharmacy);

            // H-4: Restore all ISaveable-registered subsystems from paired lists.
            RestoreSubsystemStates(data);

            if (_waterStorage != null && data.Water != null)
            {
                _waterStorage.RestoreState(data.Water);
            }

            // BunkerContamination is accumulated ambient rads inside the shelter.
            // It decays naturally via Shelter.TickContaminationDecay each hour tick.
            if (_shelter != null)
            {
                _shelter.SetBunkerContamination(data.BunkerContamination);
            }

            if (_mentalBreakSystem != null && data.Affinity != null)
            {
                _mentalBreakSystem.Affinity.Restore(data.Affinity.Entries);
            }

            if (_restoreChoreographer != null)
            {
                // Choreographer restore is safe even if the snapshot is null
                // (first launch) — it resets the state machine to defaults.
                _restoreChoreographer(data.FlashpointChoreographer);
            }

            // Restore phantom intruder cooldowns
            if (_phantomIntruderSystem != null && data.PhantomCooldownKeys != null)
            {
                _phantomIntruderSystem.Cooldowns.Clear();
                for (int i = 0; i < data.PhantomCooldownKeys.Count && i < data.PhantomCooldownValues.Count; i++)
                {
                    _phantomIntruderSystem.Cooldowns[data.PhantomCooldownKeys[i]] = data.PhantomCooldownValues[i];
                }
            }

            // Restore shelter room unlock + rubble state (Prompt #5)
            if (_shelter != null && data.ShelterRooms != null)
            {
                for (int i = 0; i < data.ShelterRooms.Count; i++)
                {
                    var roomSave = data.ShelterRooms[i];
                    if (roomSave == null || string.IsNullOrEmpty(roomSave.RoomId)) continue;
                    var room = _shelter.GetRoom(roomSave.RoomId);
                    if (room == null) continue;
                    room.UnlockState = (RoomUnlockState)roomSave.UnlockState;
                    room.RubbleClearHoursRemaining = roomSave.RubbleClearHoursRemaining;
                    room.RubbleClearHoursTotal = roomSave.RubbleClearHoursTotal;
                    if (roomSave.DiaryFragmentIds != null)
                        room.DiaryFragmentIds = new List<string>(roomSave.DiaryFragmentIds);
                    if (roomSave.RevealedDiaryIndices != null)
                        room.RevealedDiaryIndices = new List<int>(roomSave.RevealedDiaryIndices);
                }
            }

            if (_expeditionSystem != null && data.Expeditions != null)
            {
                RestoreExpeditions(data.Expeditions);
            }

            // If the nuclear exchange has already fired, unpause radiation and
            // allow hazardous weather. These flags are normally toggled by
            // HandleNuclearExchange() in GameBootstrap, but restore does not
            // replay that event, so we apply the post-exchange state here.
            if (_worldPhaseSystem != null && _worldPhaseSystem.HasTriggeredExchange)
            {
                if (_radiationSystem != null) _radiationSystem.IsPaused = false;
                if (_weatherSystem != null) _weatherSystem.RestrictToNonHazardWeather = false;
            }
        }

        private void RestoreExpeditions(List<ExpeditionSaveState> expeditions)
        {
            if (_expeditionSystem == null || expeditions == null) return;

            var existingExpeditions = _expeditionSystem.ActiveExpeditions as List<ExpeditionState>;
            var survivors = _getSurvivors?.Invoke();

            foreach (var saveExp in expeditions)
            {
                if (saveExp == null || string.IsNullOrEmpty(saveExp.SurvivorId)) continue;

                Survivor survivor = null;
                if (survivors != null)
                {
                    for (int i = 0; i < survivors.Count; i++)
                    {
                        if (survivors[i]?.Id == saveExp.SurvivorId)
                        {
                            survivor = survivors[i];
                            break;
                        }
                    }
                }

                var state = _expeditionSystem.GetExpeditionBySurvivor(saveExp.SurvivorId);
                if (state == null)
                {
                    state = new ExpeditionState();
                    if (existingExpeditions != null)
                    {
                        existingExpeditions.Add(state);
                    }
                }

                state.ExpeditionId = saveExp.ExpeditionId;
                state.SurvivorId = saveExp.SurvivorId;
                state.TargetLocationId = saveExp.TargetLocationId;
                state.TargetLocationName = saveExp.TargetLocationName;
                state.Stance = saveExp.Stance;
                state.Phase = saveExp.Phase;
                state.CurrentTick = saveExp.CurrentTick;
                state.TotalDistanceTicks = saveExp.TotalDistanceTicks;
                state.TravelTicksCompleted = saveExp.TravelTicksCompleted;
                state.LootingTicksCompleted = saveExp.LootingTicksCompleted;
                state.CarryingCapacity = saveExp.CarryingCapacity;
                state.CurrentWeight = saveExp.CurrentWeight;
                state.Stamina = saveExp.Stamina;
                state.SuitDegradation = saveExp.SuitDegradation;
                state.TrueRadPerHour = saveExp.TrueRadPerHour;
                state.DangerLevel = saveExp.DangerLevel;
                state.IsPushingLuck = saveExp.IsPushingLuck;
                state.IsRetreating = saveExp.IsRetreating;
                state.isCommsSevered = saveExp.IsCommsSevered;
                state.flashpointBehavior = saveExp.FlashpointBehavior;
                state.originalEtaTicks = saveExp.OriginalEtaTicks;
                state.shelterDelayTicksRemaining = saveExp.ShelterDelayTicksRemaining;
                state.returnSpeedMultiplier = saveExp.ReturnSpeedMultiplier;
                state.returnSpeedDivisor = saveExp.ReturnSpeedDivisor;
                state.LocationEncounterFired = saveExp.LocationEncounterFired;
                state.UxoDetonated = saveExp.UxoDetonated;
                state.HasBicycle = saveExp.HasBicycle;
                state.BicycleDurability = saveExp.BicycleDurability;
                state.IsWading = saveExp.IsWading;
                state.IsNightScavenge = saveExp.IsNightScavenge;
                state.HasFlashlight = saveExp.HasFlashlight;
                state.FlashlightBattery = saveExp.FlashlightBattery;
                state.Survivor = survivor;

                state.CollectedLootItemIds.Clear();
                if (saveExp.CollectedLootItemIds != null)
                {
                    state.CollectedLootItemIds.AddRange(saveExp.CollectedLootItemIds);
                }

                state.CollectedLoot.Clear();
                if (_itemLookup != null && state.CollectedLootItemIds != null)
                {
                    for (int i = 0; i < state.CollectedLootItemIds.Count; i++)
                    {
                        var itemDef = _itemLookup(state.CollectedLootItemIds[i]);
                        if (itemDef != null)
                        {
                            state.CollectedLoot.Add(itemDef);
                        }
                    }
                }

                state.RecalculateWeight();
            }
        }

    }
}
