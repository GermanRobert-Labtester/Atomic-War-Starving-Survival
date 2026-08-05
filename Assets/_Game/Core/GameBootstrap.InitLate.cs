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
            ExpeditionSystem.SetBicycleSystem(BicycleSystem);
            ExpeditionSystem.SetFloodedNodeSystem(FloodedNodeSystem);
            ExpeditionSystem.SetHasItem(itemId =>
                Inventory != null && !string.IsNullOrEmpty(itemId)
                && Inventory.CountById(itemId) > 0);
            // Prompts #182–#188 — combat milestone tracking on encounters / flee
            ExpeditionSystem.BindCombatPerks(
                CombatPerks,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0,
                affinity: MentalBreakSystem != null ? MentalBreakSystem.Affinity : null,
                getAllSurvivors: () => Survivors);
            SaveSystem.SetExpeditionSystem(ExpeditionSystem);
            SaveSystem.SetFloodedNodeSystem(FloodedNodeSystem);

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
            SaveSystem.SetSkillProgressionSystem(SkillProgression);
            SaveSystem.SetCombatPerkSystem(CombatPerks);
            SaveSystem.SetSurvivalPerkSystem(SurvivalPerks);
            SaveSystem.SetShelterPerkSystem(ShelterPerks);
            SaveSystem.SetMedicalPerkSystem(MedicalPerks);
            SaveSystem.SetExpeditionPerkSystem(ExpeditionPerks);
            SaveSystem.SetSocialPerkSystem(SocialPerks);
            SaveSystem.SetPersonalQuestSystem(PersonalQuests);
            WireCombatPerkBindings();
            WireSurvivalPerkBindings();
            WireShelterPerkBindings();
            WireMedicalPerkBindings();
            WireExpeditionPerkBindings();
            WireSocialPerkBindings();
            WirePersonalQuestBindings();
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
            // Prompt #188 — Desensitized: no corpse morale drain
            CorpseSystem.BindCombatPerks(CombatPerks);
            // Prompt #192 — The Butcher yields / process time
            CorpseSystem.BindSurvivalPerks(
                SurvivalPerks,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0);
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

        /// <summary>
        /// Prompts #182–#188 — bind combat perk milestones into hatch defense,
        /// perimeter traps, weapon jam hooks, and expedition encounter tracking.
        /// </summary>
        private void WireCombatPerkBindings()
        {
            if (CombatPerks == null) return;

            HatchDefenseSystem?.BindCombatPerks(CombatPerks);
            HatchDefenseSystem?.BindPerimeterTraps(PerimeterTrapSystem);

            // Prompt #174 / #182 — jam during hatch defense uses WeaponMaintenance clear ticks.
            if (HatchDefenseSystem != null && WeaponMaintenanceSystem != null)
            {
                HatchDefenseSystem.TryJamWeapon = (weaponId, clearTicks) =>
                    WeaponMaintenanceSystem.TryJam(weaponId, clearTicks: clearTicks);
            }

            PerimeterTrapSystem?.BindCombatPerks(
                CombatPerks,
                getSurvivor: id =>
                {
                    if (Survivors == null || string.IsNullOrEmpty(id)) return null;
                    for (int i = 0; i < Survivors.Count; i++)
                    {
                        if (Survivors[i] != null && Survivors[i].Id == id)
                            return Survivors[i];
                    }
                    return null;
                });

            ExpeditionSystem?.BindCombatPerks(
                CombatPerks,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0,
                affinity: MentalBreakSystem != null ? MentalBreakSystem.Affinity : null,
                getAllSurvivors: () => Survivors);
        }

        /// <summary>
        /// Prompts #189–#194 — bind survival perk milestones into cooking, medical
        /// cures, crafting, corpse processing, and AI context.
        /// </summary>
        private void WireSurvivalPerkBindings()
        {
            if (SurvivalPerks == null) return;

            CookingSystem = new CookingSystem(Inventory, WaterStorage, new System.Random(_worldSeed + 189));
            CookingSystem.BindSurvivalPerks(
                SurvivalPerks,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0);
            CookingSystem.SetMealDefinition(CookingSystem.CreateCookedMealDefinition());

            CraftingSystem?.BindSurvivalPerks(
                SurvivalPerks,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0);

            WorkbenchSystem?.BindSurvivalPerks(SurvivalPerks, NeedsSystem);
            WorkbenchSystem?.SetMoonshineItems(
                WorkbenchSystem.CreateMoonshineDefinition(),
                WorkbenchSystem.CreateMutatedFungiDefinition());

            // Prompt #190 — gastric illness recoveries grant Iron Stomach
            if (MedicalSystem != null)
            {
                MedicalSystem.OnAfflictionCured += (sv, active) =>
                {
                    if (active == null) return;
                    int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
                    SurvivalPerks.RecordIllnessRecovery(sv, active.AfflictionId, day);
                };
            }
        }

        /// <summary>
        /// Prompts #195–#200 — bind shelter-engineering perks into jury-rig,
        /// workbench scrap, struts, excavation, tunneling, and atmosphere.
        /// </summary>
        private void WireShelterPerkBindings()
        {
            if (ShelterPerks == null) return;

            Func<int> getDay = () => TimeSystem != null ? TimeSystem.CurrentDay : 0;

            JuryRigSystem?.BindShelterPerks(ShelterPerks, getDay);
            WorkbenchSystem?.BindShelterPerks(ShelterPerks, new System.Random(_worldSeed + 198));
            WorkbenchSystem?.SetRareComponentItems(
                WorkbenchSystem.CreateBatteryDefinition(),
                WorkbenchSystem.CreateSpringDefinition());

            StructuralIntegrity?.BindShelterPerks(
                ShelterPerks, getDay, CeilingCollapseSystem);
            ExcavationSystem?.BindShelterPerks(ShelterPerks, getDay);
            TunnelingSystem?.BindShelterPerks(ShelterPerks, new System.Random(_worldSeed + 199));
        }

        /// <summary>
        /// Prompts #201–#205 — bind medical milestone perks into surgery, amputation,
        /// Death's Door, and raid-window bandaging.
        /// </summary>
        private void WireMedicalPerkBindings()
        {
            if (MedicalPerks == null) return;

            Func<int> getDay = () => TimeSystem != null ? TimeSystem.CurrentDay : 0;
            Func<string, Survivor> findSv = id =>
            {
                if (Survivors == null || string.IsNullOrEmpty(id)) return null;
                for (int i = 0; i < Survivors.Count; i++)
                {
                    if (Survivors[i] != null && Survivors[i].Id == id)
                        return Survivors[i];
                }
                return null;
            };

            MedicalPerks.SetSurvivorProvider(() => Survivors);

            MedicalSystem?.BindMedicalPerks(
                MedicalPerks,
                findSurvivor: findSv,
                surgeryRng: new System.Random(_worldSeed + 201));
            if (MedicalSystem != null)
            {
                MedicalSystem.IsRaidWindowActive = () =>
                    HatchDefenseSystem != null && HatchDefenseSystem.IsRaidWindowActive;
            }

            AmputationSystem?.BindMedicalPerks(MedicalPerks, getDay);

            // Prompt #205 — Death's Door when colony has a Paramedic.
            if (NeedsSystem != null)
            {
                NeedsSystem.TryDeferDeath = sv => MedicalPerks.TryEnterDeathsDoor(sv);
            }
        }

        /// <summary>
        /// Prompts #206–#210 — bind expedition milestone perks into carry weight,
        /// stealth, city travel, night combat, darkness morale, and foraging.
        /// </summary>
        private void WireExpeditionPerkBindings()
        {
            if (ExpeditionPerks == null) return;

            Func<int> getDay = () => TimeSystem != null ? TimeSystem.CurrentDay : 0;

            ExpeditionSystem?.BindExpeditionPerks(
                ExpeditionPerks,
                getDay: getDay,
                noiseSystem: NoiseSystem,
                isStormActive: () =>
                    WeatherSystem != null
                    && (WeatherSystem.Current == WeatherKind.FalloutStorm
                        || WeatherSystem.Current == WeatherKind.Blizzard
                        || WeatherSystem.Current == WeatherKind.BlackRain));

            PerimeterTrapSystem?.BindExpeditionPerks(ExpeditionPerks, getDay);

            ScavengingSystem?.BindExpeditionPerks(
                ExpeditionPerks,
                getNodeTags: id =>
                {
                    var n = GeneratedMap?.GetNode(id);
                    return n?.Tags;
                },
                getNodeRingName: id =>
                {
                    var n = GeneratedMap?.GetNode(id);
                    return n != null ? n.Ring.ToString() : null;
                });

            // Prompt #209 — Night Terror: zero darkness morale penalty.
            if (NeedsSystem != null)
            {
                NeedsSystem.IgnoresDarknessMorale = sv =>
                    ExpeditionPerks != null && ExpeditionPerks.IgnoresDarknessMorale(sv);
            }
        }

        /// <summary>
        /// Prompts #211–#213 — bind social perks into pantry spoil rate and
        /// (optionally) weapon rust when a Quartermaster shares the room.
        /// </summary>
        private void WireSocialPerkBindings()
        {
            if (SocialPerks == null) return;

            // Prompt #212 — food spoil 50% slower in Quartermaster's room.
            PantrySystem?.BindDegradationMultiplier(roomId =>
                SocialPerks.GetItemDegradationMultiplier(roomId, Survivors));
        }

        /// <summary>
        /// Prompts #214–#219 — bind personal quests into medical, social, crafting,
        /// corpse, and (when present) pet systems. UI evolution toast is logged.
        /// </summary>
        private void WirePersonalQuestBindings()
        {
            if (PersonalQuests == null) return;

            MedicalPerks?.BindPersonalQuests(PersonalQuests);
            SocialPerks?.BindPersonalQuests(PersonalQuests);
            CraftingSystem?.BindPersonalQuests(PersonalQuests, new System.Random(_worldSeed + 214));
            CorpseSystem?.BindPersonalQuests(PersonalQuests);
            CombatPerks?.BindPersonalQuests(PersonalQuests);
            MedicalSystem?.BindPersonalQuests(PersonalQuests);
            HatchDefenseSystem?.BindPersonalQuests(PersonalQuests);
            ExpeditionSystem?.BindPersonalQuests(PersonalQuests);
            EventRunner?.BindPersonalQuests(PersonalQuests);
            PantrySystem?.BindPersonalQuests(PersonalQuests);

            PersonalQuests.OnCharacterEvolution += (sv, traitId, display) =>
            {
                string name = sv != null ? sv.DisplayName : "?";
                GameLog.Log(
                    "CharacterEvolution",
                    $"{name} unlocked latent expert trait: {display} ({traitId})");
            };
            PersonalQuests.OnMapNodeSpawnRequested += (nodeId, ownerId) =>
            {
                GameLog.Log(
                    "PersonalQuest",
                    $"Map node requested: {nodeId} for survivor {ownerId}");
            };
            PersonalQuests.OnBunkerEventRequested += (eventId, ownerId) =>
            {
                GameLog.Log(
                    "PersonalQuest",
                    $"Bunker event requested: {eventId} for survivor {ownerId}");
            };
        }

    }
}
