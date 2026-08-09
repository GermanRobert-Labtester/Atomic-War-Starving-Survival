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
                _hud.MapScreenUI.OnExpeditionRequested += (survivor, nodeId, pathReq) =>
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
            }

            // Wire radiation updates
            _onRadiationDoseChanged = (sv, dose) =>
            {
                if (sv == Survivors?[0]) // primary survivor
                {
                    _hud.OnRadiationUpdated(sv.LifetimeRadiationExposure, sv.RadiationDose);
                }
                // Anti-rad / scripted dose changes outside Tick still drive
                // trust-inversion raid cascades (healthy-ceiling cross).
                EconomySystem?.NotifyPartyRadiationChanged();
            };
            RadiationSystem.OnDoseChanged += _onRadiationDoseChanged;

            // Wire needs updates
            _onNeedChanged = (sv, kind, value) =>
            {
                if (sv == Survivors?[0])
                {
                    _hud.Bind(sv);
                }
            };
            NeedsSystem.OnNeedChanged += _onNeedChanged;

            // Vitals shows the day and hour, and HUD deliberately holds no
            // TimeSystem reference -- every value it displays is pushed in.
            _onHourTickHud = (day, hour) => _hud.SetClock(day, hour);
            TimeSystem.OnHourTick += _onHourTickHud;
            _hud.SetClock(TimeSystem.CurrentDay, TimeSystem.CurrentHour);

            // Wire shelter
            _hud.OnShelterUpdated(Shelter);

            // Initial fog-of-war push
            RefreshMapKnowledgeHUD();
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
