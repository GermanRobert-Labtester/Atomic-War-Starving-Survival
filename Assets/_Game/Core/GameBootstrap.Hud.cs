using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        // -----------------------------------------------------------------
        // HUD wiring
        // -----------------------------------------------------------------

        private void WireHUD()
        {
            if (_hud == null) return;

            // #255 Deceptive: NeedsBar may lie about hunger/thirst/health.
            _hud.BindPersonalQuests(PersonalQuests);
            _hud.BindEventRunner(EventRunner);
            _hud.BindEconomy(EconomySystem);
            _hud.BindRoomAssignment(Survivors, Shelter);
            _hud.BindPowerNetwork(PowerNetwork);
            _hud.BindHatchDefense(HatchDefenseSystem);
            _hud.BindScavengeDispatch(
                _locationCatalog,
                () => Survivors,
                GetScavengeDispatchBlockReason,
                GetScavengeDispatchTaskLabel,
                BuildScavengeMissionRoster,
                BuildScavengePreflightSummary,
                locationId => _hud.MapKnowledgeHUD != null
                    ? _hud.MapKnowledgeHUD.GetTileLabel(locationId)
                    : "?",
                GetScavengeLootPreview);
            WireScavengeDispatchHud();
            _hud.BindOverflowCrate(() => OverflowCrateSystem != null ? OverflowCrateSystem.GetSnapshot() : null);
            WireOverflowCrateHud();
            _hud.BindFieldGearLoadout(() => FieldGearLoadoutSystem != null ? FieldGearLoadoutSystem.GetSnapshot() : null);
            WireFieldGearLoadoutHud();
            _hud.BindBunkerRationing(() => BunkerRationingSystem != null
                ? BunkerRationingSystem.GetSnapshot(Survivors)
                : null);
            WireBunkerRationingHud();
            _hud.BindWaterPurification(
                () => WaterEconomySystem != null ? WaterEconomySystem.GetSnapshot(Shelter, WaterStorage) : null,
                () => BunkerRationingSystem != null ? BunkerRationingSystem.GetSnapshot(Survivors) : null);
            WireWaterPurificationHud();
            _hud.BindAirHeatManagement(() => AirHeatManagementSystem != null
                ? AirHeatManagementSystem.GetSnapshot()
                : null);
            WireAirHeatManagementHud();
            _hud.BindBunkerMaintenance(
                () => BunkerMaintenanceSystem != null ? BunkerMaintenanceSystem.GetSnapshot() : null,
                () => Survivors,
                () => RepairWorkOrderSystem != null ? RepairWorkOrderSystem.GetSnapshot() : null);
            WireBunkerMaintenanceHud();
            _hud.BindSurvivorTaskBoard(() => SurvivorTaskBoardSystem != null
                ? SurvivorTaskBoardSystem.GetSnapshot()
                : null);
            WireSurvivorTaskBoardHud();
            _hud.EnsureRadioInterceptHud();
            WireRadioInterceptTuner();
            SyncRadioInterceptHudFromLog();
            _hud.EnsureJournalBook();
            SyncJournalBookFromSystem();
            _hud.EnsureExpeditionEncounterLog();
            // Ammo tooltips / hatch stockpile / combat log (safe if ItemAmmoTypes not ready yet).
            WireAmmoUiBindings();
            RefreshInventoryStrip(); // initial pooled icon sync
            // UI Toolkit diegetic panels (hatch ammo/arms, encounter log, stores tooltip).
            _hud.EnsureDiegeticHud();
            WireInternalHorrorHud();
            _hud.EnsureEndgameSummary();
            if (VictoryProject != null && VictoryProject.IsTerminal && VictoryProject.LastSummary != null)
                PushEndgameSummaryToHud(VictoryProject.LastSummary);
            _hud.BindGeneratedMap(GeneratedMap, () => WeatherSystem != null ? WeatherSystem.Current : WeatherKind.Clear);
            _hud.BindWorkbench(WorkbenchSystem);

            // Map screen expedition requests → ExpeditionSystem
            if (_hud.MapScreenUI != null)
            {
                // Defensive: if WireHUD ever runs twice in a session (load-game
                // hot path, future "reset and replay", re-entrant Awake), the
                // old lambda would orphan in MapScreenUI.OnExpeditionRequested.
                if (_onMapExpeditionRequested != null)
                    _hud.MapScreenUI.OnExpeditionRequested -= _onMapExpeditionRequested;
                _onMapExpeditionRequested = (survivor, nodeId, pathReq) =>
                {
                    if (ExpeditionSystem == null || survivor == null || pathReq == null) return;
                    var node = GeneratedMap?.GetNode(nodeId);
                    if (node != null)
                        ExpeditionSystem.StartExpedition(survivor, node);
                    else
                        ExpeditionSystem.StartExpeditionFromPath(
                            survivor,
                            new ExpeditionSystem.PathRequest
                            {
                                NodeId = nodeId,
                                TravelHours = pathReq.TravelHours,
                                TrueRad = pathReq.TrueRad,
                                DangerLevel = pathReq.DangerLevel,
                                DisplayName = pathReq.NodeId
                            });
                };
                _hud.MapScreenUI.OnExpeditionRequested += _onMapExpeditionRequested;
            }

            // Wire radiation updates
            if (_onRadiationDoseChanged != null)
                RadiationSystem.OnDoseChanged -= _onRadiationDoseChanged;
            // Snapshot the primary survivor reference once so the hot-path
            // comparison does not re-index the Survivors list per event.
            // Null Survivors / empty list is a valid state before the world
            // is initialised; the lambda becomes a no-op then.
            var primarySurvivor = Survivors != null && Survivors.Count > 0 ? Survivors[0] : null;
            _onRadiationDoseChanged = (sv, dose) =>
            {
                if (sv == primarySurvivor && sv != null)
                {
                    _hud.OnRadiationUpdated(sv.LifetimeRadiationExposure, sv.RadiationDose);
                }
                // Anti-rad / scripted dose changes outside Tick still drive
                // trust-inversion raid cascades (healthy-ceiling cross).
                EconomySystem?.NotifyPartyRadiationChanged();
            };
            RadiationSystem.OnDoseChanged += _onRadiationDoseChanged;

            // Wire needs updates
            if (_onNeedChanged != null)
                NeedsSystem.OnNeedChanged -= _onNeedChanged;
            _onNeedChanged = (sv, kind, value) =>
            {
                if (sv == primarySurvivor && sv != null)
                {
                    _hud.Bind(sv);
                }
            };
            NeedsSystem.OnNeedChanged += _onNeedChanged;

            // Vitals shows the day and hour, and HUD deliberately holds no
            // TimeSystem reference -- every value it displays is pushed in.
            // The first SetClock below triggers the first RepaintVitals, which
            // reads the cached dose off the DosimeterHUD -- so the dose must
            // land on the HUD before that first paint, or the vitals panel
            // reads "0.00 Sv" until the next OnDoseChanged event fires. The
            // smoke test only catches non-empty text, so an off-by-one wiring
            // here is otherwise invisible.
            if (_onHourTickHud != null)
                TimeSystem.OnHourTick -= _onHourTickHud;
            _onHourTickHud = (day, hour) => _hud.SetClock(day, hour);
            TimeSystem.OnHourTick += _onHourTickHud;
            var primary = Survivors != null && Survivors.Count > 0 ? Survivors[0] : null;
            if (primary != null)
                _hud.OnRadiationUpdated(primary.LifetimeRadiationExposure, primary.RadiationDose);
            _hud.SetClock(TimeSystem.CurrentDay, TimeSystem.CurrentHour);

            // Wire shelter
            _hud.OnShelterUpdated(Shelter);

            // Initial fog-of-war push
            RefreshMapKnowledgeHUD();
        }

        /// <summary>
        /// Core owns the dispatch request and live mission events. UI only emits
        /// an intent and receives formatted outcome data, keeping the UI assembly
        /// independent from Core's mission types.
        /// </summary>
        private void WireScavengeDispatchHud()
        {
            var dispatch = _hud != null ? _hud.ScavengeDispatchHUD : null;
            if (dispatch == null || ScavengingSystem == null) return;

            if (_onScavengeDispatchRequested != null)
                dispatch.OnDispatchRequested -= _onScavengeDispatchRequested;
            _onScavengeDispatchRequested = HandleScavengeDispatchRequested;
            dispatch.OnDispatchRequested += _onScavengeDispatchRequested;

            if (_onScavengeMissionStartedHud != null)
                ScavengingSystem.OnMissionStarted -= _onScavengeMissionStartedHud;
            _onScavengeMissionStartedHud = mission =>
            {
                if (mission == null || mission.Kind != MissionKind.Scavenge) return;
                var board = _hud != null ? _hud.ScavengeDispatchHUD : null;
                board?.ReportMissionStarted(
                    mission.Survivor != null && !string.IsNullOrEmpty(mission.Survivor.DisplayName)
                        ? mission.Survivor.DisplayName
                        : mission.SurvivorId,
                    string.IsNullOrEmpty(mission.LocationName) ? mission.LocationId : mission.LocationName,
                    mission.TotalHours);
            };
            ScavengingSystem.OnMissionStarted += _onScavengeMissionStartedHud;

            if (_onScavengeAfterActionHud != null)
                ScavengingSystem.OnScavengeAfterActionReady -= _onScavengeAfterActionHud;
            _onScavengeAfterActionHud = report =>
            {
                var board = _hud != null ? _hud.ScavengeDispatchHUD : null;
                board?.ReportAfterAction(BuildScavengeAfterActionSummary(report));
            };
            ScavengingSystem.OnScavengeAfterActionReady += _onScavengeAfterActionHud;

            SyncScavengeAfterActionHud();
        }

        /// <summary>
        /// Replays the last save-safe scavenging report into the terminal. This is
        /// intentionally callable after a SaveSystem restore because HUD wiring
        /// happens before a pending Continue slot is loaded during Awake.
        /// </summary>
        private void SyncScavengeAfterActionHud()
        {
            var dispatch = _hud != null ? _hud.ScavengeDispatchHUD : null;
            var report = ScavengingSystem != null ? ScavengingSystem.LastAfterActionReport : null;
            if (dispatch != null && report != null)
                dispatch.ReportAfterAction(BuildScavengeAfterActionSummary(report));
        }

        /// <summary>Core receives crate transfer intent; UI never mutates either inventory directly.</summary>
        private void WireOverflowCrateHud()
        {
            var crate = _hud != null ? _hud.OverflowCrateHUD : null;
            if (crate == null || OverflowCrateSystem == null) return;

            if (_onOverflowCrateTransferRequested != null)
                crate.OnTransferRequested -= _onOverflowCrateTransferRequested;
            _onOverflowCrateTransferRequested = HandleOverflowCrateTransferRequested;
            crate.OnTransferRequested += _onOverflowCrateTransferRequested;

            OverflowCrateSystem.OnChanged -= RefreshOverflowCrateHud;
            OverflowCrateSystem.OnChanged += RefreshOverflowCrateHud;
        }

        private void RefreshOverflowCrateHud()
        {
            if (_hud == null) return;
            _hud.OverflowCrateHUD?.Refresh();
            _hud.RefreshDiegeticHud();
        }

        /// <summary>Core receives loadout intent; UI never mutates worn gear directly.</summary>
        private void WireFieldGearLoadoutHud()
        {
            var loadout = _hud != null ? _hud.FieldGearLoadoutHUD : null;
            if (loadout == null || FieldGearLoadoutSystem == null) return;

            if (_onFieldGearEquipRequested != null)
                loadout.OnEquipRequested -= _onFieldGearEquipRequested;
            if (_onFieldGearUnequipRequested != null)
                loadout.OnUnequipRequested -= _onFieldGearUnequipRequested;
            _onFieldGearEquipRequested = HandleFieldGearEquipRequested;
            _onFieldGearUnequipRequested = HandleFieldGearUnequipRequested;
            loadout.OnEquipRequested += _onFieldGearEquipRequested;
            loadout.OnUnequipRequested += _onFieldGearUnequipRequested;

            FieldGearLoadoutSystem.OnChanged -= RefreshFieldGearLoadoutHud;
            FieldGearLoadoutSystem.OnChanged += RefreshFieldGearLoadoutHud;
        }

        private void RefreshFieldGearLoadoutHud()
        {
            if (_hud == null) return;
            _hud.FieldGearLoadoutHUD?.Refresh();
            // The dispatch board reads equipped protection in its preflight text.
            // Refresh immediately after a gear change instead of waiting for a
            // later inventory frame/event.
            _hud.ScavengeDispatchHUD?.Refresh();
            _hud.RefreshDiegeticHud();
        }

        private void WireBunkerRationingHud()
        {
            var rationing = _hud != null ? _hud.BunkerRationingHUD : null;
            if (rationing == null || BunkerRationingSystem == null) return;

            if (_onRationLevelAdjustmentRequested != null)
                rationing.OnLevelAdjustmentRequested -= _onRationLevelAdjustmentRequested;
            _onRationLevelAdjustmentRequested = HandleRationLevelAdjustmentRequested;
            rationing.OnLevelAdjustmentRequested += _onRationLevelAdjustmentRequested;

            BunkerRationingSystem.OnChanged -= RefreshBunkerRationingHud;
            BunkerRationingSystem.OnChanged += RefreshBunkerRationingHud;
        }

        private void RefreshBunkerRationingHud()
        {
            if (_hud == null) return;
            _hud.BunkerRationingHUD?.Refresh();
            _hud.WaterPurificationHUD?.Refresh();
            _hud.RefreshDiegeticHud();
        }

        /// <summary>Core owns purifier queue mutations; the HUD only emits a direction.</summary>
        private void WireWaterPurificationHud()
        {
            var waterTerminal = _hud != null ? _hud.WaterPurificationHUD : null;
            if (waterTerminal == null || WaterEconomySystem == null) return;

            if (_onWaterQueueCycleRequested != null)
                waterTerminal.OnQueueCycleRequested -= _onWaterQueueCycleRequested;
            _onWaterQueueCycleRequested = HandleWaterQueueCycleRequested;
            waterTerminal.OnQueueCycleRequested += _onWaterQueueCycleRequested;

            WaterEconomySystem.OnWaterStateChanged -= RefreshWaterPurificationHud;
            WaterEconomySystem.OnWaterStateChanged += RefreshWaterPurificationHud;
        }

        private void RefreshWaterPurificationHud()
        {
            if (_hud == null) return;
            _hud.WaterPurificationHUD?.Refresh();
            // The clean cistern is a ration source, so every purifier/catchment
            // change must immediately revise the water coverage projection.
            _hud.BunkerRationingHUD?.Refresh();
            _hud.RefreshDiegeticHud();
        }

        /// <summary>Core receives climate intent while PowerNetwork remains the persisted authority.</summary>
        private void WireAirHeatManagementHud()
        {
            var terminal = _hud != null ? _hud.AirHeatManagementHUD : null;
            if (terminal == null || AirHeatManagementSystem == null) return;

            if (_onAirHeatPriorityAdjustmentRequested != null)
                terminal.OnPriorityAdjustmentRequested -= _onAirHeatPriorityAdjustmentRequested;
            if (_onAirHeatRequestToggleRequested != null)
                terminal.OnRequestToggleRequested -= _onAirHeatRequestToggleRequested;
            _onAirHeatPriorityAdjustmentRequested = HandleAirHeatPriorityAdjustmentRequested;
            _onAirHeatRequestToggleRequested = HandleAirHeatRequestToggleRequested;
            terminal.OnPriorityAdjustmentRequested += _onAirHeatPriorityAdjustmentRequested;
            terminal.OnRequestToggleRequested += _onAirHeatRequestToggleRequested;

            AirHeatManagementSystem.OnChanged -= RefreshAirHeatManagementHud;
            AirHeatManagementSystem.OnChanged += RefreshAirHeatManagementHud;
        }

        private void RefreshAirHeatManagementHud()
        {
            if (_hud == null) return;
            _hud.AirHeatManagementHUD?.Refresh();
            _hud.RefreshDiegeticHud();
        }

        /// <summary>Core owns terminal assignment; Utility AI owns work-order claiming.</summary>
        private void WireBunkerMaintenanceHud()
        {
            var terminal = _hud != null ? _hud.BunkerMaintenanceHUD : null;
            if (terminal == null || BunkerMaintenanceSystem == null) return;

            if (_onMaintenanceRepairRequested != null)
                terminal.OnRepairRequested -= _onMaintenanceRepairRequested;
            if (_onMaintenanceRepairCancellationRequested != null)
                terminal.OnRepairCancellationRequested -= _onMaintenanceRepairCancellationRequested;
            if (_onMaintenanceSurvivorAssignmentRequested != null)
                terminal.OnSurvivorAssignmentRequested -= _onMaintenanceSurvivorAssignmentRequested;
            if (_onMaintenancePriorityAdjustmentRequested != null)
                terminal.OnPriorityAdjustmentRequested -= _onMaintenancePriorityAdjustmentRequested;
            _onMaintenanceRepairRequested = HandleMaintenanceRepairRequested;
            _onMaintenanceRepairCancellationRequested = HandleMaintenanceRepairCancellationRequested;
            _onMaintenanceSurvivorAssignmentRequested = HandleMaintenanceSurvivorAssignmentRequested;
            _onMaintenancePriorityAdjustmentRequested = HandleMaintenancePriorityAdjustmentRequested;
            terminal.OnRepairRequested += _onMaintenanceRepairRequested;
            terminal.OnRepairCancellationRequested += _onMaintenanceRepairCancellationRequested;
            terminal.OnSurvivorAssignmentRequested += _onMaintenanceSurvivorAssignmentRequested;
            terminal.OnPriorityAdjustmentRequested += _onMaintenancePriorityAdjustmentRequested;

            BunkerMaintenanceSystem.OnChanged -= RefreshBunkerMaintenanceHud;
            BunkerMaintenanceSystem.OnChanged += RefreshBunkerMaintenanceHud;
            if (RepairWorkOrderSystem != null)
            {
                RepairWorkOrderSystem.OnChanged -= RefreshBunkerMaintenanceHud;
                RepairWorkOrderSystem.OnChanged += RefreshBunkerMaintenanceHud;
            }
        }

        private void RefreshBunkerMaintenanceHud()
        {
            if (_hud == null) return;
            _hud.BunkerMaintenanceHUD?.Refresh();
            _hud.RefreshDiegeticHud();
        }

        /// <summary>Task-board commands stay in Core; the HUD only raises intent events.</summary>
        private void WireSurvivorTaskBoardHud()
        {
            var board = _hud != null ? _hud.SurvivorTaskBoardHUD : null;
            if (board == null || SurvivorTaskBoardSystem == null) return;

            if (_onTaskBoardPriorityAdjustmentRequested != null)
                board.OnPriorityAdjustmentRequested -= _onTaskBoardPriorityAdjustmentRequested;
            if (_onTaskBoardCancellationRequested != null)
                board.OnCancellationRequested -= _onTaskBoardCancellationRequested;
            if (_onTaskBoardShiftAssignmentRequested != null)
                board.OnShiftAssignmentRequested -= _onTaskBoardShiftAssignmentRequested;
            if (_onTaskBoardShiftCancellationRequested != null)
                board.OnShiftCancellationRequested -= _onTaskBoardShiftCancellationRequested;
            if (_onTaskBoardRecommendationApprovalRequested != null)
                board.OnShiftRecommendationApprovalRequested -= _onTaskBoardRecommendationApprovalRequested;
            _onTaskBoardPriorityAdjustmentRequested = HandleTaskBoardPriorityAdjustmentRequested;
            _onTaskBoardCancellationRequested = HandleTaskBoardCancellationRequested;
            _onTaskBoardShiftAssignmentRequested = HandleTaskBoardShiftAssignmentRequested;
            _onTaskBoardShiftCancellationRequested = HandleTaskBoardShiftCancellationRequested;
            _onTaskBoardRecommendationApprovalRequested = HandleTaskBoardRecommendationApprovalRequested;
            board.OnPriorityAdjustmentRequested += _onTaskBoardPriorityAdjustmentRequested;
            board.OnCancellationRequested += _onTaskBoardCancellationRequested;
            board.OnShiftAssignmentRequested += _onTaskBoardShiftAssignmentRequested;
            board.OnShiftCancellationRequested += _onTaskBoardShiftCancellationRequested;
            board.OnShiftRecommendationApprovalRequested += _onTaskBoardRecommendationApprovalRequested;

            SurvivorTaskBoardSystem.OnChanged -= RefreshSurvivorTaskBoardHud;
            SurvivorTaskBoardSystem.OnChanged += RefreshSurvivorTaskBoardHud;
        }

        private void RefreshSurvivorTaskBoardHud()
        {
            if (_hud == null) return;
            _hud.SurvivorTaskBoardHUD?.Refresh();
            _hud.RefreshDiegeticHud();
        }

        // -----------------------------------------------------------------
        // Event context + narrative chain pool helpers
        // -----------------------------------------------------------------

        /// <summary>
        /// Shared EventContext for hourly event ticks and day-gated scheduleEvent chains.
        /// Imports SaveSystem world flags and wires trust + flag persistence.
        /// </summary>
        private EventContext BuildEventContext(int day, float hour = 12f, float? indoorTempC = null)
        {
            if (_eventCtxRng == null) WarmDayTickCaches();

            float indoor = indoorTempC ?? (TemperatureSystem != null
                ? TemperatureSystem.GetIndoorTemperature(Shelter)
                : 15f);

            // Reuse one EventContext each hour — ImportFlags clears WorldFlags in place.
            var ctx = _eventContextScratch;
            ctx.PrimarySurvivor = Survivors != null && Survivors.Count > 0 ? Survivors[0] : null;
            ctx.Shelter = Shelter;
            ctx.Inventory = Inventory;
            ctx.Random = _eventCtxRng;
            ctx.CurrentDay = day;
            ctx.CurrentHour = hour;
            ctx.IsFalloutStorm = WeatherSystem != null && WeatherSystem.Current == WeatherKind.FalloutStorm;
            ctx.CurrentWeather = WeatherSystem != null ? WeatherSystem.Current : WeatherKind.Clear;
            ctx.AllSurvivors = Survivors;
            ctx.MentalBreak = MentalBreakSystem;
            ctx.NeedsSystem = NeedsSystem;
            ctx.RadiationSystem = RadiationSystem;
            ctx.CarbonMonoxidePpm = PowerNetwork != null ? PowerNetwork.CarbonMonoxidePpm : 0f;
            ctx.IndoorTemperatureC = indoor;
            ctx.GetFactionTrust = _getFactionTrustStored;
            ctx.OnEventFlagChanged = _onEventFlagChangedCached;
            ctx.PlayerSurvivorId = Survivors != null && Survivors.Count > 0 && Survivors[0] != null
                ? Survivors[0].Id
                : null;
            ctx.Suspicion = SuspicionTracker;
            ctx.ActiveIntelReliability = IntelReliability.Unverified;
            ctx.IsOnRadio = false;
            ctx.IsResourceStarved = false;

            if (SaveSystem != null)
                ctx.ImportFlags(SaveSystem.WorldFlags);
            else if (ctx.WorldFlags != null)
                ctx.WorldFlags.Clear();

            if (SuspicionTracker != null)
            {
                SuspicionTracker.RefreshStarved(Inventory);
                ctx.IsResourceStarved = SuspicionTracker.IsResourceStarved;
            }
            return ctx;
        }

    }
}
