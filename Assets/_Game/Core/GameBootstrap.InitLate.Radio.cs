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
        private void InitRadioAndEndgame()
        {
            InitRadioBroadcastAndTuner();
            InitDebtGhostAndEndgameSystems();
        }

        private void InitRadioBroadcastAndTuner()
        {
            RadioSystem = new RadioBroadcastSystem();
            RadioSystem.SetCatalog(_radioCatalog);

            RadioTunerSystem = new RadioTunerSystem(
                CreateSaltedRng(_worldSeed, "radio_tuner"),
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0);
            InitializeRadioFrequencies();

            var radioModule = Shelter.GetModule("radio");
            if (radioModule != null && radioModule.IsOperational)
            {
                RadioTunerSystem.State.AvailableFuel = radioModule.Fuel;
                RadioTunerSystem.State.PowerConsumptionPerHour = 0.5f;
            }

            // Persist tuner power/tuning + extracted intel (CaptureState already existed).
            if (SaveSystem != null)
                SaveSystem.SetRadioTunerSystem(RadioTunerSystem);
        }

        private void InitDebtGhostAndEndgameSystems()
        {
            InitDebtCollectorSystem();
            InitGhostStationSystem();
            InitLifeboatTransmissionSystem();
            WireRadioIntelExtraction();
            WireRadioTriggeredGameEvents();
            WireRadioDayTickAndCaches();
            InitFlashpointChoreographerAndSave();
        }

        private void InitDebtCollectorSystem()
        {
            // Prompt #18 — Debt Collector (day+20 after faction dig-out).
            DebtCollectorSystem = new DebtCollectorSystem();
            DebtCollectorSystem.Bind(
                EconomySystem,
                FactionRadioIntercepts,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0,
                shelter: Shelter,
                water: WaterStorage,
                inventory: Inventory,
                radioState: RadioTunerSystem?.State);
            if (HatchEntrapmentSystem != null)
                HatchEntrapmentSystem.OnFactionRescueApplied += HandleFactionRescueApplied_ScheduleDebt;
            DebtCollectorSystem.OnCollectorArrived += HandleDebtCollectorArrived;
            SaveSystem.SetDebtCollectorSystem(DebtCollectorSystem);
        }

        private void InitGhostStationSystem()
        {
            // Prompt #19 — Ghost Stations (unlock after EMP; never live/extraction intel).
            GhostStationSystem = new GhostStationSystem();
            GhostStationSystem.SetNeedsSystem(NeedsSystem);
            GhostStationSystem.Bind(
                RadioTunerSystem,
                JournalSystem,
                getSurvivors: () => Survivors,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0);
            EventBus.Subscribe<FlashpointEmptiedDevices>(OnFlashpointEmp_UnlockGhosts);
            SaveSystem.SetGhostStationSystem(GhostStationSystem);
        }

        private void InitLifeboatTransmissionSystem()
        {
            // Prompt #20 — Lifeboat Transmission (late-game single-seat extraction).
            LifeboatTransmissionSystem = new LifeboatTransmissionSystem();
            LifeboatTransmissionSystem.Bind(
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0,
                getSurvivors: () => Survivors,
                isCampaignTerminal: () =>
                    (VictoryProject != null && VictoryProject.IsTerminal)
                    || (EndgameEngine != null && EndgameEngine.Result.IsTerminal)
                    || IsGameOver,
                endgame: EndgameEngine,
                victory: VictoryProject);
            LifeboatTransmissionSystem.OnContactOffered += HandleLifeboatContactOffered;
            SaveSystem.SetLifeboatTransmissionSystem(LifeboatTransmissionSystem);
        }

        private void WireRadioIntelExtraction()
        {
            RadioTunerSystem.OnIntelExtracted += HandleIntelExtracted;
        }

        private void HandleIntelExtracted(IntelNode intel)
        {
            Debug.Log($"[Radio] Extracted intel: {intel.Type} - {intel.Text}");
            // Ghost loops intentionally skip VictoryProject / plume map paths.
            if (intel != null && intel.Type == IntelType.GhostLoop) return;
            VictoryProject?.NotifyIntel(intel);
            if (intel.Type != IntelType.PlumeReport) return;
            RadioTunerSystem.ApplyPlumeReportToMap(intel, KnowledgeMap, GeneratedMap);
            RefreshMapKnowledgeHUD();
            _hud?.MapScreenUI?.Refresh();
        }

        private void WireRadioTriggeredGameEvents()
        {
            // Prompt #46 — Radio-triggered GameEvents when a survivor is at the radio.
            RadioSystem.OnBroadcastStarted += HandleRadioBroadcastTrigger;
            EventRunner.OnChoiceApplied += HandleSafeHavenChoiceApplied;
            EventRunner.OnChoiceApplied += HandleBloodForWaterChoiceApplied;
            EventRunner.OnChoiceApplied += HandleHatchEntrapmentChoiceApplied;
            EventRunner.OnChoiceApplied += HandleChildFoundChoiceApplied;
            EventRunner.OnChoiceApplied += HandleRaidPlanChoiceApplied;
            EventRunner.OnChoiceApplied += HandleDebtCollectorChoiceApplied;
            EventRunner.OnChoiceApplied += HandleLifeboatChoiceApplied;
        }

        private void WireRadioDayTickAndCaches()
        {
            TimeSystem.OnDayTick += OnRadioAndWorldDayTick;
            WarmDayTickCaches();
        }

        private void OnRadioAndWorldDayTick(int day)
        {
            RadioSystem.CheckForBroadcast(day);
            Inventory?.DriftAllDevices(1f);
            KnowledgeMap?.TickDay(day);
            ShiftingHotspotSystem?.TickDay(day);
            FactionRaidPlanSystem?.TickDay(day);
            DebtCollectorSystem?.TickDay(day);
            LifeboatTransmissionSystem?.TickDay(day, Survivors);
            RefreshMapKnowledgeHUD();
            VictoryProject?.TickDay(day, Survivors);
            if (EventRunner == null) return;
            EventRunner.TickDay(day, BuildEventContext(day));
        }

        private void InitFlashpointChoreographerAndSave()
        {
            // Day-30 Flashpoint Choreographer — created last so EMP step has live refs.
            FlashpointChoreographer = new FlashpointChoreographer(
                sequence: _flashpointSequence,
                accessibilitySafeMode: () => GameState != null && GameState.AccessibilitySafeMode,
                systems: new FlashpointChoreographerSystems
                {
                    Inventory = Inventory,
                    Shelter = Shelter,
                    RadioState = RadioTunerSystem?.State,
                    WeatherSystem = WeatherSystem,
                    RadiationSystem = RadiationSystem,
                    EconomySystem = EconomySystem,
                    Survivors = Survivors,
                    ExchangeMoraleHit = WorldPhaseSystem.ExchangeMoraleHit,
                    ExpeditionSystem = ExpeditionSystem
                },
                hasFlashpointTriggered: () => WorldPhaseSystem != null && WorldPhaseSystem.HasTriggeredExchange);
            TimeSystem.OnDayTick += FlashpointChoreographer.OnDayTick;

            if (SaveSystem == null) return;
            SaveSystem.SetFlashpointChoreographer(
                FlashpointChoreographer.CaptureState,
                FlashpointChoreographer.RestoreState);
            SaveSystem.SetMentalBreakSystem(MentalBreakSystem);
            SaveSystem.SetPhantomIntruderSystem(PhantomIntruders);
        }
    }
}
