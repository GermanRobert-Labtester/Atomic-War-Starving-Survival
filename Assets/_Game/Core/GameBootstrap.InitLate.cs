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
        private void InitFactionMapSystems()
        {
            // Prompt #17 — inter-faction raid plans / wiretap choices.
            // Antenna gate uses RadioTunerSystem once it is constructed later in
            // InitializeSystems; the provider reads live state each call.
            // Map is rebound after GeneratedMap is created below.
            FactionRaidPlanSystem = new FactionRaidPlanSystem(new System.Random(_worldSeed + 21));
            FactionRaidPlanSystem.Bind(
                EconomySystem,
                FactionRadioIntercepts,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0,
                isAntennaOperational: IsWiretapAntennaOperational,
                map: null);
            FactionRaidPlanSystem.OnInterceptOffered += HandleRaidPlanInterceptOffered;

            _onWorldPhaseChanged = phase =>
            {
                EconomySystem.NotifyPhaseChanged(phase);
                // Keep weather/rad systems in sync with campaign phase labels
                if (phase == WorldPhase.Flashpoint || phase == WorldPhase.NuclearWinter)
                {
                    // Exchange already unpauses rads in HandleNuclearExchange
                }
            };
            WorldPhaseSystem.OnPhaseChanged += _onWorldPhaseChanged;

            // Proc-gen wasteland map (seed-stable layout for this playthrough)
            GeneratedMap = MapGenerator.Generate(_worldSeed);
            FactionRaidPlanSystem?.SetMap(GeneratedMap);

            // Knowledge map must exist before SaveSystem can capture it
            KnowledgeMap = new RadiationKnowledgeMap();
            SeedKnowledgeMap();

        }

        private void InitSaveAndExpeditions()
        {
            // Save
            SaveSystem = new SaveSystem(new SaveSystem.CoreDeps
            {
                GameState = GameState,
                WeatherSystem = WeatherSystem,
                TemperatureSystem = TemperatureSystem,
                NeedsSystem = NeedsSystem,
                RadiationSystem = RadiationSystem,
                Shelter = Shelter,
                GetSurvivors = () => Survivors,
                ItemLookup = id =>
                {
                    var fromCatalog = _itemCatalog?.GetById(id);
                    if (fromCatalog != null) return fromCatalog;
                    // Prompt #13 — poisoned iodine may only exist as a runtime plant.
                    if (SabotagedCacheSystem != null
                        && string.Equals(id, SabotagedCacheSystem.PoisonedIodineItemId,
                            System.StringComparison.OrdinalIgnoreCase))
                        return SabotagedCacheSystem.PoisonedIodineDefinition;
                    return null;
                },
                ModuleLookup = id => null
            });
            // P1 / AUDIT-004: fail-fast ISaveable restore in Editor + Development
            // (game-ci batchmode) only. Release players keep best-effort restore.
            SaveSystem.FailFastRestore = SaveSystem.DefaultFailFastRestoreForEnvironment();
            SaveSystem.SetPhotoPeriodSystem(PhotoperiodSystem);
            SaveSystem.SetKnowledgeMap(KnowledgeMap);
            SaveSystem.SetGeneratedMap(GeneratedMap);
            SaveSystem.SetInventory(Inventory);
            SaveSystem.SetMedicalSystem(MedicalSystem);
            SaveSystem.SetBloodTransfusionSystem(BloodTransfusion);
            SaveSystem.SetAmputationSystem(AmputationSystem);
            SaveSystem.SetScurvySystem(ScurvySystem);
            SaveSystem.SetMutagenesisSystem(Mutagenesis);
            SaveSystem.SetWorldPhaseSystem(WorldPhaseSystem);
            SaveSystem.SetEconomySystem(EconomySystem);
            SaveSystem.SetPowerNetwork(PowerNetwork);
            SaveSystem.SetHatchDefense(HatchDefenseSystem);
            SaveSystem.SetFactionRadioIntercepts(FactionRadioIntercepts);
            SaveSystem.SetFactionRaidPlanSystem(FactionRaidPlanSystem);
            SaveSystem.SetJournalSystem(JournalSystem);
            SaveSystem.SetVictoryProjectManager(VictoryProject);
            SaveSystem.SetEventRunner(EventRunner);
            SaveSystem.SetSuspicionTracker(SuspicionTracker);
            SaveSystem.SetPreCaptureHook(SnapshotRadioHudToInterceptSystem);
            SaveSystem.SetWaterStorage(WaterStorage);
            // SetFlashpointChoreographer is called later in InitializeSystems
            // after the Choreographer itself is constructed (it depends on
            // RadioTunerSystem and other systems wired after SaveSystem).

            // Subscribe to phase changes for autosave
            _onGameStateChanged = phase =>
            {
                if (phase == GamePhase.Running) SaveSystem.AutoSave();
            };
            GameState.OnPhaseChanged += _onGameStateChanged;

            // Scavenging + survey (shares KnowledgeMap with SaveSystem)
            ScavengingSystem = new LocationScavengingSystem(
                RadiationSystem, Inventory, _itemCatalog, _worldSeed,
                KnowledgeMap, () => TimeSystem.CurrentDay,
                _lootTable, () => WorldPhaseSystem.CurrentPhase);
            ScavengingSystem.OnSurveyCompleted += (mission, success) => RefreshMapKnowledgeHUD();
            ScavengingSystem.OnMissionCompleted += (mission, loot) => RefreshMapKnowledgeHUD();

            // Expedition Engine (node-based events, stances, stamina drain, push-your-luck)
            // Wired with the MedicalSystem so the Day-30 flashpoint intercept
            // can inflict trauma afflictions on survivors caught outside, and
            // with the Shelter + Survivors list so the hatch-dilemma handler
            // can spike bunker contamination and propagate deny-entry morale.
            ExpeditionSystem = new ExpeditionSystem(
                RadiationSystem, Inventory, _itemCatalog,
                new ExpeditionSystem.Config
                {
                    WeatherSystem = WeatherSystem,
                    KnowledgeMap = KnowledgeMap,
                    MedicalSystem = MedicalSystem,
                    Shelter = Shelter,
                    Survivors = Survivors,
                    Seed = _worldSeed
                });
            ExpeditionSystem.SetGeneratedMap(GeneratedMap);
            SaveSystem.SetExpeditionSystem(ExpeditionSystem);

            // Prompt #13 — hostile factions learn scavenging habits and plant
            // poisoned medical crates. High Medical skill / Paranoid spots them.
            SabotagedCacheSystem = new SabotagedCacheSystem(new System.Random(_worldSeed + 19));
            SabotagedCacheSystem.BindEconomy(EconomySystem);
            SabotagedCacheSystem.SetPoisonedIodineDefinition(
                SabotagedCacheSystem.CreatePoisonedIodineDefinition());
            ExpeditionSystem.SetSabotagedCacheSystem(SabotagedCacheSystem);
            SaveSystem.SetSabotagedCacheSystem(SabotagedCacheSystem);
            ExpeditionSystem.OnSabotagedCacheDetected += (exp, msg) =>
            {
                Debug.Log($"[Sabotaged Cache] Detected by {exp?.Survivor?.DisplayName}: {msg}");
            };

            // Prompt #14 — post-Day-30 windstorms move death-zone rad two path-hops.
            ShiftingHotspotSystem = new ShiftingHotspotSystem(new System.Random(_worldSeed + 20));
            ShiftingHotspotSystem.Bind(GeneratedMap, KnowledgeMap);
            SaveSystem.SetShiftingHotspotSystem(ShiftingHotspotSystem);
            ShiftingHotspotSystem.OnHotspotShifted += shift =>
            {
                if (shift == null) return;
                Debug.Log(
                    $"[Shifting Hotspot] Windstorm day {shift.Day}: " +
                    $"{shift.FromNodeId} → {shift.ToNodeId} " +
                    $"(moved {shift.MovedRad:F0} rad/hr)");
                RefreshMapKnowledgeHUD();
            };

            // Prompt #48 — weather buries/freezes the hatch after 72 continuous
            // hours of Blizzard/FalloutStorm; DigOut spikes entry CO2; broken
            // air filter while sealed starts suffocation countdown.
            HatchEntrapmentSystem = new HatchEntrapmentSystem();
            _entryRoom = new ShelterRoom(HatchEntrapmentSystem.EntryRoomId, null);
            HatchEntrapmentSystem.OnHatchStateChanged += (prev, next) =>
            {
                SyncHatchExpeditionLock();
                Debug.Log($"[Hatch Entrapment] HatchState {prev} → {next}");
            };
            HatchEntrapmentSystem.OnBuriedAliveTriggered += () =>
            {
                // Present the Buried Alive event immediately when the seal lands.
                if (EventRunner == null) return;
                var buried = EventRunner.FindInPool(EventRunner.BuriedAliveEventId)
                             ?? EventRunner.CreateBuriedAliveEvent();
                var ctx = BuildEventContext(TimeSystem != null ? TimeSystem.CurrentDay : 1);
                ctx.SetEventFlag(HatchEntrapmentSystem.FlagBuriedAliveOffered, true);
                if (buried != null && buried.CanTrigger(ctx))
                    EventRunner.Run(buried, ctx);
            };
            SaveSystem.SetHatchEntrapment(HatchEntrapmentSystem);
            SaveSystem.SetChildDependentSystem(ChildSystem);
            SaveSystem.SetStructuralIntegritySystem(StructuralIntegrity);
            SaveSystem.SetWasteSystem(WasteSystem);
            SaveSystem.SetVerminSystem(VerminSystem);
            SaveSystem.SetJuryRigSystem(JuryRigSystem);
            SaveSystem.SetFreezePipeSystem(FreezePipeSystem);
            SaveSystem.SetCartographySystem(CartographySystem);
            SaveSystem.SetTrackerSystem(TrackerSystem);
            SaveSystem.SetDeadDropSystem(DeadDropSystem);
            SaveSystem.SetHostageSystem(HostageSystem);
            SaveSystem.SetPropagandaSystem(PropagandaSystem);
            SaveSystem.SetDeserterSystem(DeserterSystem);
            SaveSystem.SetScapegoatSystem(ScapegoatSystem);
            SaveSystem.SetLaborCampSystem(LaborCampSystem);
            SaveSystem.SetCultMoralSystem(CultMoralSystem);
            SaveSystem.SetEcosystemSystem(EcosystemSystem);
            SaveSystem.SetHouseToBunkerSystem(HouseToBunkerSystem);
            SaveSystem.SetLocationQuestSystem(LocationQuestSystem);
            SaveSystem.SetExcavationSystem(ExcavationSystem);
            SaveSystem.SetFloodingSystem(FloodingSystem);
            SaveSystem.SetHiddenStorageSystem(HiddenStorageSystem);
            SaveSystem.SetCeilingCollapseSystem(CeilingCollapseSystem);
            SaveSystem.SetPerimeterTrapSystem(PerimeterTrapSystem);
            SaveSystem.SetTunnelingSystem(TunnelingSystem);
            SaveSystem.SetHatchVisibilitySystem(HatchVisibilitySystem);
            SaveSystem.SetEscapeHatchSystem(EscapeHatchSystem);
            SaveSystem.SetMaterialShieldingSystem(MaterialShieldingSystem);
            SaveSystem.SetAirlockSystem(AirlockSystem);
            SaveSystem.SetNoiseSystem(NoiseSystem);
            SaveSystem.SetResilienceSystem(ResilienceSystem);
            SaveSystem.SetCompostSystem(CompostSystem);
            SaveSystem.SetScrapWeaponSystem(ScrapWeaponSystem);
            SaveSystem.SetSterilizationSystem(SterilizationSystem);
            SaveSystem.SetChelationSystem(ChelationSystem);
            SaveSystem.SetWindTurbineSystem(WindTurbineSystem);
            SaveSystem.SetAntibioticResistSystem(AntibioticResistSystem);
            SaveSystem.SetHaulingSystem(HaulingSystem);
            SaveSystem.SetWeaponMaintenanceSystem(WeaponMaintenanceSystem);
            SaveSystem.SetAestheticsSystem(AestheticsSystem);
            SaveSystem.SetHamRadioSystem(HamRadioSystem);
            SaveSystem.SetTriageSystem(TriageSystem);
            SaveSystem.SetPolypharmacySystem(PolypharmacySystem);
            SyncHatchExpeditionLock();

            // ───────────────────────────────────────────────────────────
            // Internal Horror — atmosphere / corpses / pantry rust
            // ───────────────────────────────────────────────────────────
            _storesRoom = new ShelterRoom("stores", null);
            AtmosphereSystem = new ShelterAtmosphereSystem(new System.Random(_worldSeed + 16));
            AtmosphereSystem.RegisterRoom(_entryRoom);
            AtmosphereSystem.RegisterRoom(_storesRoom);
            AtmosphereSystem.RegisterRoom(new ShelterRoom("quarters", null));
            AtmosphereSystem.RegisterRoom(new ShelterRoom("plant", null));

            CorpseSystem = new CorpseManagementSystem(
                NeedsSystem, Inventory, MedicalSystem, RadiationSystem,
                new System.Random(_worldSeed + 17));
            CorpseSystem.SetItemDefinitions(
                CorpseManagementSystem.CreateCorpseDefinition(),
                CorpseManagementSystem.CreateFertilizerDefinition());
            CorpseSystem.SetStoresRoom(_storesRoom);
            CorpseSystem.SetSurvivorProvider(() => Survivors);
            CorpseSystem.BindDeathHandler();

            PantrySystem = new PantryContaminationSystem(
                Inventory, new System.Random(_worldSeed + 18));
            PantrySystem.SetContaminatedFoodDefinition(
                PantryContaminationSystem.CreateContaminatedFoodDefinition());
            PantrySystem.SetStoresRoom(_storesRoom);

            SaveSystem.SetAtmosphereSystem(AtmosphereSystem);
            SaveSystem.SetCorpseSystem(CorpseSystem);
            SaveSystem.SetPantrySystem(PantrySystem);

            // Hatch dilemma: when a comms-severed expedition arrives at the
            // bunker, run a forced dilemma GameEventSO with three choices
            // (let them in, force decon, deny). The ExpeditionSystem raises
            // HatchDilemmaReadySignal when an expedition enters the
            // AtHatchDilemma phase; we build the event here and run it
            // through the EventRunner, then forward the choice back via
            // HatchDilemmaResolvedSignal (which the ExpeditionSystem listens to).
            ExpeditionSystem.OnHatchDilemmaReady += OnHatchDilemmaReady_Handle;
            if (Inventory != null)
            {
                Inventory.OnInventoryChanged += RefreshMapKnowledgeHUD;
                Inventory.OnInventoryChanged += RefreshInventoryStrip;
            }
            if (KnowledgeMap != null)
            {
                KnowledgeMap.OnKnowledgeChanged += RefreshMapKnowledgeHUD;
            }

        }

        private void InitRadioAndEndgame()
        {
            // Radio (broadcast only; tuner/intel extraction is separate)
            RadioSystem = new RadioBroadcastSystem();
            RadioSystem.SetCatalog(_radioCatalog);
            
            // Radio Tuner System (intel extraction)
            RadioTunerSystem = new RadioTunerSystem(
                new System.Random(_worldSeed + 31),
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0);
            InitializeRadioFrequencies();
            
            // Wire up radio module fuel supply to RadioTunerSystem
            var radioModule = Shelter.GetModule("radio");
            if (radioModule != null && radioModule.IsOperational)
            {
                RadioTunerSystem.State.AvailableFuel = radioModule.Fuel;
                RadioTunerSystem.State.PowerConsumptionPerHour = 0.5f; // Default consumption
            }

            // Prompt #18 — Debt Collector (day+20 after faction dig-out).
            // Constructed after RadioTuner so antenna cut can EMP the live RadioState.
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

            // Prompt #19 — Ghost Stations (unlock after EMP; never live/extraction intel).
            GhostStationSystem = new GhostStationSystem();
            GhostStationSystem.Bind(
                RadioTunerSystem,
                JournalSystem,
                getSurvivors: () => Survivors,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0);
            EventBus.Subscribe<FlashpointEmptiedDevices>(OnFlashpointEmp_UnlockGhosts);
            SaveSystem.SetGhostStationSystem(GhostStationSystem);

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
            
            // Wire up intel extraction events
            RadioTunerSystem.OnIntelExtracted += intel =>
            {
                Debug.Log($"[Radio] Extracted intel: {intel.Type} - {intel.Text}");
                // Ghost loops intentionally skip VictoryProject / plume map paths.
                if (intel != null && intel.Type == IntelType.GhostLoop) return;
                VictoryProject?.NotifyIntel(intel);
                if (intel.Type == IntelType.PlumeReport)
                {
                    // Apply plume reports to knowledge map + proc-gen node reveal
                    RadioTunerSystem.ApplyPlumeReportToMap(intel, KnowledgeMap, GeneratedMap);
                    RefreshMapKnowledgeHUD();
                    _hud?.MapScreenUI?.Refresh();
                }
            };

            // Prompt #46 — Radio-triggered GameEvents. When a broadcast with
            // a non-empty triggerEventId plays AND a survivor is currently
            // at the radio (IsOnRadio on the EventContext), raise the named
            // event through the standard EventRunner path. This is how the
            // Safe Haven broadcast surfaces as a player choice: the radio
            // plays the loop, the player is at the dial, the event fires.
            RadioSystem.OnBroadcastStarted += HandleRadioBroadcastTrigger;
            EventRunner.OnChoiceApplied += HandleSafeHavenChoiceApplied;
            EventRunner.OnChoiceApplied += HandleBloodForWaterChoiceApplied;
            EventRunner.OnChoiceApplied += HandleHatchEntrapmentChoiceApplied;
            EventRunner.OnChoiceApplied += HandleChildFoundChoiceApplied;
            EventRunner.OnChoiceApplied += HandleRaidPlanChoiceApplied;
            EventRunner.OnChoiceApplied += HandleDebtCollectorChoiceApplied;
            EventRunner.OnChoiceApplied += HandleLifeboatChoiceApplied;

            TimeSystem.OnDayTick += day =>
            {
                RadioSystem.CheckForBroadcast(day);
                Inventory?.DriftAllDevices(1f);
                KnowledgeMap?.TickDay(day);
                // Prompt #14 — rare windstorm may move a death-zone after Day 30.
                ShiftingHotspotSystem?.TickDay(day);
                // Prompt #17 — latent inter-faction raid plans + wiretap window.
                FactionRaidPlanSystem?.TickDay(day);
                // Prompt #18 — delayed dig-out debt collectors.
                DebtCollectorSystem?.TickDay(day);
                // Prompt #20 — late-game lifeboat contact (Day ≥ 80).
                LifeboatTransmissionSystem?.TickDay(day, Survivors);
                RefreshMapKnowledgeHUD();
                // Radio win path: extraction coords + survive to Day 100.
                VictoryProject?.TickDay(day, Survivors);
                // Multi-stage narrative chains (Prompt #43): fire day-gated follow-ups.
                if (EventRunner != null)
                {
                    var dayCtx = BuildEventContext(day);
                    EventRunner.TickDay(day, dayCtx);
                }
            };

            // Cache day-tick RNGs / lambdas once so TickSystems allocates no
            // System.Random or closure per hour (see DayTickGcProfileTests).
            WarmDayTickCaches();

            // Day-30 Flashpoint Choreographer (narrative/UX layer over the
            // mechanical EMP/weather cascade). Owns the buildup days 25-29
            // and the second-by-second choreography. The mechanical side
            // effects of the exchange fire from the choreography's 'emp' step
            // so the EMP happens after the white flash, not before.
            //
            // Created at the END of InitializeSystems so the systems bundle
            // (Inventory, Survivors, EconomySystem, RadioTunerSystem) all have
            // real references; the EMP step needs every one of them on day 30.
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

            // Wire the Choreographer into SaveSystem so the buildup-day and
            // choreography-step state persist across save/load. Done here
            // (after the Choreographer is created) rather than near the
            // other SetXSystem calls because the Choreographer depends on
            // systems that are wired after SaveSystem in InitializeSystems.
            if (SaveSystem != null)
            {
                SaveSystem.SetFlashpointChoreographer(
                    FlashpointChoreographer.CaptureState,
                    FlashpointChoreographer.RestoreState);
                SaveSystem.SetMentalBreakSystem(MentalBreakSystem);
                SaveSystem.SetPhantomIntruderSystem(PhantomIntruders);
            }

        }
    }
}
