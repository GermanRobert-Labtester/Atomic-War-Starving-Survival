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
        /// <summary>
        /// NeedsSystem's isNearHeatSource predicate: true when it is warm enough where
        /// this survivor is standing for Needs.Warmth to recover. Indoors that means a
        /// fuelled, operational heater has lifted the bunker above comfort; out on an
        /// expedition there is no shelter bonus, so the weather-adjusted ambient decides.
        ///
        /// This was hardcoded to `sv => true`, which counted every survivor as standing
        /// next to a fire at all times: warmth only ever climbed, and nuclear-winter cold
        /// — one of the game's core hazards — never fired at all.
        /// </summary>
        private bool IsSurvivorNearHeat(Survivor survivor)
        {
            if (survivor == null || TemperatureSystem == null) return false;
            var shelter = IsSurvivorOnExpedition(survivor) ? null : Shelter;
            return TemperatureSystem.IsWarmEnoughForRecovery(survivor, shelter);
        }

        private void InitFoundation()
        {
            // H-5: System registry for centralized tick dispatch and diagnostics.
            _registry = new SystemRegistry();

            // Core
            GameState = new GameState();
            TimeSystem = new TimeSystem { SecondsPerGameHour = _secondsPerGameHour };

            // GameState.Day is a mirror of the clock, not a second source of truth.
            // Nothing advanced it, so it sat at 1 for the whole campaign: the save
            // file recorded day 1 and VictoryProjectManager.DaysSurvived — which reads
            // GameState.Day off the save — always reported a one-day run.
            GameState.Day = TimeSystem.CurrentDay;
            _onDayTick_SetGameStateDay = day => GameState.Day = day;
            TimeSystem.OnDayTick += _onDayTick_SetGameStateDay;

            // Environment
            WeatherSystem = new WeatherSystem(_seasonProfile, _worldSeed);
            TemperatureSystem = new TemperatureSystem(_seasonProfile, WeatherSystem);
            PhotoperiodSystem = new PhotoperiodSystem(_seasonProfile, WeatherSystem);

            // Shelter
            Shelter = new Shelter.Shelter();
            Shelter.AddModule(new ShelterModuleInstance("air_filtration", 2) { FilterHealth = 100f });
            Shelter.AddModule(new ShelterModuleInstance("radiation_shielding", 2));
            Shelter.AddModule(new ShelterModuleInstance("heater", 1) { Fuel = 50f });
            Shelter.AddModule(new ShelterModuleInstance("workbench", 1));
            // Grow-light starts installed but dry (no fuel). Player must scavenge fuel to light it.
            Shelter.AddModule(new ShelterModuleInstance("grow_light", 1) { Fuel = 0f });
            // Roof catchment starts open (player can close it to stop collecting during a storm).
            Shelter.AddModule(new ShelterModuleInstance("catchment_surface", 1) { IsEnabled = true });
            Shelter.AddModule(new ShelterModuleInstance("water_purifier", 1) { FilterHealth = 100f });
            // Sleep quarters: bed in "quarters"; diesel lives in "plant" next door (Prompt #32).
            Shelter.AddModule(new ShelterModuleInstance("bed", 1)
            {
                RoomId = SleepQualitySystem.DefaultSleepRoomId,
                ComfortLevel = 1f,
                Capacity = 2
            });
            // Comfort station: a quiet corner of the quarters where the AI
            // is willing to spend comfort items on a broken survivor. Always
            // on (no fuel cost in the current tuning); the
            // MentalBreakComfortActionSO reads it via MedicalSystem.ComfortStationModuleId.
            Shelter.AddModule(new ShelterModuleInstance("comfort_station", 1)
            {
                RoomId = SleepQualitySystem.DefaultSleepRoomId,
                IsEnabled = true
            });
            Shelter.SetRoomsAdjacent(
                SleepQualitySystem.DefaultSleepRoomId,
                SleepQualitySystem.DefaultGeneratorRoomId);

            // Prompt #5 — Previous Tenants: a sealed deep vault with diaries.
            // The player must clear rubble to access it. Contains diary warnings
            // about the filter, water, and shielding — diegetic system intel.
            var deepVault = new ShelterRoom("deep_vault", null)
            {
                UnlockState = RoomUnlockState.Sealed,
                RubbleClearHoursRemaining = 16f,
                RubbleClearHoursTotal = 16f,
                DiaryFragmentIds = new System.Collections.Generic.List<string>
                {
                    "diary_filter_is_a_lie",
                    "diary_water_truth",
                    "diary_shielding_rot"
                }
            };
            Shelter.RegisterRoom(deepVault);

            // Shelter power grid: finite watts, load-shedding, diesel + bicycle generators.
            // Fully-qualified type: property name PowerNetwork shadows the class.
            PowerNetwork = AtomicWar._Game.Shelter.PowerNetwork.CreateDefault(dieselFuel: 40f);
            // Prompt #799 — IF/THEN grid automation over power consumers/sources.
            LogicGates = new System_LogicGates();
            WireLogicGates();
            // Prompt #864 — community mods (JSON overrides) before other data consumers run.
            BootModLoader();
            // Prompt #865 — Twitch chat polls (offline stub; host hooks re-bound after HatchDefense exists).
            BootTwitchAPI();
            // Batch: disease / scapegoat / iron man / android / sheriff / scenario / speedrun / true ending / sieges.
            BootBatchSystems();
            // Victory paths (14 endings; TrueEnding already constructed in BootBatchSystems).
            BootVictoryPaths();
            // Map hazards — expedition node hazards (acid geyser / ashlanche / traps / …).
            BootMapHazards();
            // Map anomalies — expedition node anomalies (cherenkov / mirage / quiet zone / …).
            BootMapAnomalies();
            // Expedition biomes — ash swamp / glass desert / salt flats / … .
            BootBiomes();
            // Special weather events — acid snow / EMP storm / solar flare / … .
            BootWeather();
            // Expedition encounters — amalgamation / burrowers / maze / tank / … .
            BootEncounters();
            // Shelter modules with CaptureState (acid trap / autodoc / lathe / …).
            BootShelterModules();
            // Narrative Event_* with CaptureState (brawl / schism / witch hunt / …).
            BootEvents();
            // CoreFamilies bulk boots (auto)
            BootActionFamily();
            BootAfflictionFamily();
            BootAudioEventFamily();
            BootCombatFamily();
            BootCombatStanceFamily();
            BootCrisisFamily();
            BootDurabilityFamily();
            BootEndgameFamily();
            BootHazardFamily();
            BootHiddenStatFamily();
            BootItemFamily();
            BootLocationFamily();
            BootMapFamily();
            BootMiscFamily();
            BootNPCFamily();
            BootNodeFamily();
            BootPetFamily();
            BootProjectFamily();
            BootShelterEventFamily();
            BootSkirmishFamily();
            BootTraderFamily();
            BootTraitFamily();
            BootUIEventFamily();
            BootVehicleFamily();
            BootVisitorFamily();
            BootWeaponFamily();
            BootWorldEventFamily();
            BootRemainingComplexFamily();

            var diesel = PowerNetwork.GetSource("diesel_generator");
            if (diesel != null)
            {
                diesel.RoomId = SleepQualitySystem.DefaultGeneratorRoomId;
            }
            // Heater/filter are installed and requested; grow light stays optional until fuel/power allow.
            PowerNetwork.SetRequested("grow_light", false);
            PowerNetwork.SetRequested("radio", false);
            PowerNetwork.SetRequested("water_purifier", true);
            PowerNetwork.ApplyToShelter(Shelter);

            // Bunker water economy: roof catchment + 3-tier purifier (Prompt #28).
            WaterStorage = new WaterStorage();
            WaterEconomySystem = new WaterEconomySystem();
            // Prompt #11 — Black Rain (constructed after WeatherSystem exists; see late bind).
            // WeatherSystem is created earlier in Awake — safe to construct here.
            if (WeatherSystem != null)
                BlackRainHazardSystem = new BlackRainHazardSystem(WeatherSystem);

            // Needs + Radiation
            // Harden: missing inspector assignment must not NRE the whole bootstrap
            // (tests / partial scenes). Use an ephemeral default profile instead.
            var needsProfile = _needsProfile != null
                ? _needsProfile
                : ScriptableObject.CreateInstance<NeedsProfile>();
            NeedsSystem = new NeedsSystem(needsProfile, IsSurvivorNearHeat);

            // Wire photoperiod into NeedsSystem (null-safe: skipped if LightProfile not assigned)
            if (_lightProfile != null)
            {
                NeedsSystem.SetPhotoPeriodSystem(
                    () => PhotoperiodSystem.EffectiveDaylightHours,
                    _lightProfile,
                    () => Shelter.IsGrowLightActive);
            }

            RadiationSystem = new RadiationSystem(NeedsSystem, BuildExposureContext);

            // Wire NeedsSystem into all systems that modify survivor needs so they
            // route through Modify (trait caps, perk effects, OnNeedChanged events).
            BlackRainHazardSystem?.SetNeedsSystem(NeedsSystem);

            BeliefSystem = new BeliefSystem(rng: CreateSaltedRng(_worldSeed, "belief"));
            BeliefSystem.SetNeedsSystem(NeedsSystem);
            _onRadiationStatusGained = (sv, status) =>
            {
                if (status == SurvivorStatus.AcuteRadiationSyndrome)
                {
                    BeliefSystem.ShockRecoverNumbness(sv);
                }
            };
            RadiationSystem.OnStatusGained += _onRadiationStatusGained;

            // World Phase (Civil War -> Flashpoint -> Nuclear Winter). Phase 1 defaults:
            // no radiation, no post-war weather hazards, until the exchange fires.
            WorldPhaseSystem = new WorldPhaseSystem(_worldPhaseConfig);
            RadiationSystem.IsPaused = true;
            WeatherSystem.RestrictToNonHazardWeather = true;
            WorldPhaseSystem.OnNuclearExchange += HandleNuclearExchange;
            TimeSystem.OnDayTick += WorldPhaseSystem.OnDayTick;

            // Inventory + Crafting + Workbench scrap economy
            Inventory = new Inventory.Inventory { Capacity = 50, MaxWeight = 200f };
            // CRAFT-003 overflow stash: a separate unlimited-capacity inventory for
            // craft results that don't fit in the main bag. Modeled as a "post
            // office box" — the player can retrieve from it on a future tick.
            // Capacity=0 + MaxWeight=0 means infinite (per Inventory.CanAdd
            // short-circuits). Items here are persistent until retrieved.
            CraftingOverflowStash = new Inventory.Inventory { Capacity = 0, MaxWeight = 0f };
            CraftingSystem = new CraftingSystem(Inventory);
            CraftingSystem.OverflowStash = CraftingOverflowStash;
            CraftingSystem.AddStation(new CraftingStation
            {
                id = WorkbenchSystem.StationId,
                displayName = "Workbench",
                Condition = 100f
            });
            WorkbenchSystem = new WorkbenchSystem(
                Inventory,
                id => _itemCatalog?.GetById(id),
                CraftingSystem,
                () => Shelter,
                () => TimeSystem != null ? TimeSystem.CurrentDay : 0);

            // Seed inventory
            SeedStartingInventory();

            // Survivors
            Survivors = new List<Survivor>();
            CreateSurvivor("sv_elena", "Elena Vasquez");
            CreateSurvivor("sv_marcus", "Marcus Olejnik");
            CreateSurvivor("sv_suki", "Suki Tanaka");

        }

        private void InitUtilityAI()
        {
            // AI
            UtilityAI = new UtilityAI();
            Actions = new List<SurvivorAction>
            {
                CreateAction<EatActionSO>(),
                CreateAction<DrinkActionSO>(),
                CreateAction<DrinkContaminatedWaterActionSO>(),
                CreateAction<SleepActionSO>(),
                CreateAction<RestActionSO>(),
                CreateAction<WarmUpActionSO>(),
                CreateAction<TakeIodineActionSO>(),
                CreateAction<UseAntiRadActionSO>(),
                CreateAction<ScavengeActionSO>(),
                CreateAction<SurveyActionSO>(),
                CreateAction<TreatPatientActionSO>(),
                CreateAction<CaregiveActionSO>(),
                CreateAction<MentalBreakComfortActionSO>(),
                CreateAction<CraftActionSO>(),
                CreateAction<GuardActionSO>(),
                CreateAction<PedalGeneratorActionSO>(),
                CreateAction<SearchForChemsActionSO>(),
                CreateAction<ClearRubbleActionSO>(),
                CreateAction<HuntRatsActionSO>(),
                CreateAction<MercyKillActionSO>(),
                CreateAction<ChartMapActionSO>(),
                // Audit C-3: AI actions for the systems wired in C-1. Each
                // action scores when the system has work to do; the action's
                // Execute method calls the system's primary method. The new
                // AIContext fields (ExcavationSystem, HaulingSystem, etc.) are
                // bound in WarmDayTickCaches so these actions can run on the
                // survivor decision pass.
                CreateAction<ExcavateActionSO>(),
                CreateAction<CompostWasteActionSO>(),
                CreateAction<BoilToolsActionSO>(),
                CreateAction<BeginChelationActionSO>(),
                CreateAction<BuildWindTurbineActionSO>(),
                CreateAction<HaulLootActionSO>(),
                CreateAction<DeconAndEnterActionSO>(),
                CreateAction<ExcavateEscapeHatchActionSO>(),
                CreateAction<UpgradeShieldingActionSO>(),
                CreateAction<TunnelActionSO>(),
                // Prompt #184 — Suppressing Fire (unlocked via perk_suppressing_fire)
                CreateAction<SuppressingFireActionSO>()
            };

        }

        private void InitMedicalSystems()
        {
            // Medical triage (afflictions drain health; treatments halt/cure)
            MedicalSystem = new MedicalSystem(NeedsSystem, Inventory, Shelter);
            foreach (var aff in AtomicWar._Game.Medical.MedicalSystem.CreateDefaultAfflictions())
                MedicalSystem.RegisterAffliction(aff);
            // Register common treatment recipes if items exist
            var bandage = _itemCatalog?.GetById("bandage");
            var tweezers = _itemCatalog?.GetById("tweezers");
            if (bandage != null)
            {
                MedicalSystem.RegisterTreatment(
                    AtomicWar._Game.Medical.MedicalSystem.CreateGunshotBandageHaltRecipe(bandage));
                if (tweezers != null)
                {
                    MedicalSystem.RegisterTreatment(
                        AtomicWar._Game.Medical.MedicalSystem.CreateGunshotFullRecipe(bandage, tweezers));
                }
            }

        }

        private void InitEventsAndSurvivors()
        {
            InitEventRunnerCore();
            InitMentalBreakAndTraits();
            InitAddictionAndMedicalPrompts();
            InitWorldSideSystems();
            InitShelterTacticalSystems();
            InitNarrativeDependentSystems();
            InitAtmosphereHygieneSystems();
            InitDiaryAndHatchSystems();
            InitFactionMapSystems();
        }

        private void InitEventRunnerCore()
        {
            // Events
            EventRunner = new EventRunner();

            // H-6: Unified event pool construction. All event sources
            // (catalog, 6 Ensure* chains, encounter factory) are merged
            // into a single call with built-in duplicate-id detection.
            var eventPool = EventPoolBuilder.Build(_eventCatalog);
            EventRunner.SetPool(eventPool);

            SuspicionTracker = new SuspicionTracker();
            SuspicionTracker.SetNeedsSystem(NeedsSystem);
            SuspicionTracker.Bind(EventRunner);

            // Diegetic journal — survivors write discoveries (no tutorial popups).
            // Entries run through a pool: evicted/cleared entries are recycled,
            // never collected, so 100-day fast-forward runs stay GC-flat.
            JournalSystem = new JournalSystem();
            JournalSystem.SetNeedsSystem(NeedsSystem);
            _journalEntryPool = new GenericObjectPool<JournalEntry>(
                () => new JournalEntry(),
                e =>
                {
                    e.Id = null;
                    e.Text = null;
                    e.Timestamp = null;
                    e.AuthorName = null;
                    e.AuthorId = null;
                    e.KnowledgeKey = null;
                    e.Day = 0;
                    e.Hour = 0f;
                },
                // +1: at a full list the new entry is acquired before the
                // evicted one is released, so steady state needs cap+1 stock.
                initialCapacity: JournalSystem.MaxEntries + 1);
            JournalSystem.SetEntryFactory(_journalEntryPool.Acquire, _journalEntryPool.Release);
            JournalSystem.OnEntryAdded += entry =>
            {
                if (entry == null || string.IsNullOrEmpty(entry.Text)) return;
                GameLog.Log($"[Journal] {entry.Timestamp} — {entry.AuthorName}: {entry.Text}");
                PushJournalEntryToHud(entry);
            };

            // Campaign win/loss — radio extraction + vehicle escape projects
            VictoryProject = new VictoryProjectManager();
            EndgameEngine = new EndgameEngine(GameModeKind.Story, _campaignLengthDays);
            VictoryProject.OnExtractionUnlocked += () =>
            {
                GameLog.Log("[Endgame] Extraction coordinates unlocked (10 military intel). Survive to Day 100.");
            };
            VictoryProject.OnEndgameTriggered += summary =>
            {
                if (summary == null) return;
                ApplyEndgame(summary);
            };

            // DEEP3-WIN-001 — EndgameEngine.Evaluate raises OnCampaignEnded and routes
            // a CampaignEndedEvent through the bus, but nothing freezes the run on that
            // path: only VictoryProject.OnEndgameTriggered was wired to ApplyEndgame. A
            // natural all-dead / bunker-collapse / extraction / 100-day ending now sets
            // IsGameOver and pauses the clock instead of silently continuing.
            EndgameEngine.OnCampaignEnded += _ => ApplyEndgameFromEndgameEngine();

        }

        private void InitMentalBreakAndTraits()
        {
            // Mental Break System (Prompt #29). Designers populate the catalog
            // with MentalBreakSO assets (BingeEater, ViolentParanoia, etc.).
            // If the catalog is null, the system is still constructed — just
            // empty — so the rest of the game continues to work; the Survivor
            // just never rolls for a break.
            MentalBreakSystem = new MentalBreakSystem();
            MentalBreakSystem.SetNeedsSystem(NeedsSystem);
            if (_mentalBreakCatalog != null)
            {
                foreach (var br in _mentalBreakCatalog.breaks)
                {
                    if (br != null) MentalBreakSystem.RegisterBreak(br);
                }
            }
            // Host-side binge/sabotage so Survivors assembly stays free of
            // Inventory/Shelter refs (avoids asmdef cycles).
            MentalBreakSystem.BingeEatHandler = (sv, br) => ForceMentalBreakBingeEat(sv, br);
            MentalBreakSystem.SabotageHandler = (sv, br, rng) => ForceMentalBreakSabotage(rng);
            // Host-side comfort-cure so the system can consume a Comfort
            // item from the Inventory without referencing the Inventory
            // assembly directly. Same asmdef-boundary pattern.
            MentalBreakSystem.ComfortCureHandler = (sv, br) => ForceMentalBreakComfortCure(sv, br);

            // ───────────────────────────────────────────────────────────
            // Prompt #10 — Skill Atrophy System
            // ───────────────────────────────────────────────────────────
            SkillAtrophy = new SkillAtrophySystem();

            // ───────────────────────────────────────────────────────────
            // Prompts #179–#181 — Action-driven progression
            // ───────────────────────────────────────────────────────────
            SkillProgression = new SkillProgressionSystem();
            SkillProgression.SetNeedsSystem(NeedsSystem);
            SkillProgression.RegisterDefaultPerks();
            // Prompts #182–#188 — combat milestone perks (jam/stealth/ammo/CQ/traps/flee/kills)
            CombatPerks = new CombatPerkSystem();
            CombatPerks.SetNeedsSystem(NeedsSystem);
            CombatPerks.Bind(SkillProgression);
            // Prompts #189–#194 — survival / wasteland-digestion milestone perks
            SurvivalPerks = new SurvivalPerkSystem();
            SurvivalPerks.Bind(SkillProgression);
            // Prompts #195–#200 — shelter-engineering milestone perks
            ShelterPerks = new ShelterPerkSystem();
            ShelterPerks.Bind(SkillProgression);
            // Prompts #201–#205 — medical milestone perks (Steady Hands … Paramedic)
            MedicalPerks = new MedicalPerkSystem();
            MedicalPerks.Bind(SkillProgression, () => Survivors);
            // Prompts #206–#210 — expedition / wasteland milestone perks
            ExpeditionPerks = new ExpeditionPerkSystem();
            ExpeditionPerks.Bind(SkillProgression);
            // Prompts #211–#213 — social / leadership milestone perks
            SocialPerks = new SocialPerkSystem();
            SocialPerks.Bind(SkillProgression);
            // Prompts #214–#219 — personal quest engine + latent expert traits
            PersonalQuests = new PersonalQuestSystem();
            PersonalQuests.Bind(SkillProgression);
            PersonalQuests.SetNeedsSystem(NeedsSystem);
            // MISC-007 — quest rad spikes / archetype lifetime seeds go through
            // RadiationSystem only (no direct survivor.RadiationDose writes on the
            // host path). Positive spikes use Expose so lifetime + events fire;
            // negative deltas (if any) cleanse current dose only.
            PersonalQuests.BindRadiationDose((sv, delta) =>
            {
                if (RadiationSystem == null || sv == null) return;
                if (delta > 0f)
                    RadiationSystem.Expose(sv, delta, 1f);
                else
                    RadiationSystem.AdjustDose(sv, delta);
            });
            PersonalQuests.BindLifetimeRadiation((sv, lifetime) =>
            {
                RadiationSystem?.SeedLifetimeExposure(sv, lifetime);
            });
            AssignActionProgressionDisciplines();

            // ───────────────────────────────────────────────────────────
            // Prompt #8 — Empath & Sociopath System
            // ───────────────────────────────────────────────────────────
            EmpathSystem = new EmpathSystem();
            EmpathSystem.SetNeedsSystem(NeedsSystem);
            SurvivorDiaries = new SurvivorDiariesSystem();
            InternalLockSystem = new InternalLockSystem();
            SpatialPsychology = new SpatialPsychologySystem();
            SpatialPsychology.SetNeedsSystem(NeedsSystem);
            GriefKeepsakes = new GriefKeepsakeSystem();
            GriefKeepsakes.SetNeedsSystem(NeedsSystem);
            HallucinationSystem = new AI.HallucinationSystem();
            MentorshipSystem = new MentorshipSystem();

            // Prompts #469-#478 — interpersonal &amp; leadership director.
            InitBunkerSocial();

            // Wire death hook into NeedsSystem.OnDied
            _onNeedsDied = deceased =>
            {
                // #250 Pillar of Atlas: permanent repair debuff if he dies with the trait.
                PersonalQuests?.NotifySurvivorDied(deceased);
                EmpathSystem.OnSurvivorDied(deceased, Survivors);
                ChildSystem?.CheckChildDeath(Survivors);
                GriefKeepsakes?.OnSurvivorDied(deceased, Survivors, MentalBreakSystem?.Affinity, "item_keepsake_pendant");
                // #469 lover-grief mental break + cleanup.
                BunkerSocial?.NotifySurvivorDied(deceased, null);
                // Prompt #862 — Iron Man save deletion when last survivor dies.
                NotifyIronManSurvivorDeath(deceased);
            };
            NeedsSystem.OnDied += _onNeedsDied;

            // DEATH-001 wire: Tribunal.Execution and ResolveExecute go through
            // SurvivorNeedWrite.SetHealth which bypasses NeedsSystem.OnDied. The
            // OnKilled / OnLeaderKilled delegates below run the same death chain
            // so a Tribunal execution or a successful mutiny kills the survivor
            // the same way a natural death does.
            if (BunkerSocial != null)
            {
                BunkerSocial.OnKilled = _onNeedsDied;
                if (BunkerSocial.Mutiny != null)
                    BunkerSocial.Mutiny.OnLeaderKilled = _onNeedsDied;
            }

            // ───────────────────────────────────────────────────────────
        }

        /// <summary>
        /// Prompt #799 — re-evaluate logic gates when the power grid rebalances.
        /// </summary>
        private void WireLogicGates()
        {
            if (LogicGates == null || PowerNetwork == null) return;

            PowerNetwork.OnPowerStateChanged += () => TickLogicGates();
            LogicGates.OnRuleTriggered += ruleId =>
                GameLog.Log($"[GameBootstrap] LOGIC: rule '{ruleId}' triggered.");
        }

        /// <summary>
        /// Prompt #799 — sample power module states, evaluate IF/THEN rules, apply actions.
        /// </summary>
        private void TickLogicGates()
        {
            if (_logicGatesTicking) return;
            if (LogicGates == null || PowerNetwork == null) return;
            if (LogicGates.GetRuleCount() <= 0) return;

            _logicGatesTicking = true;
            try
            {
                var states = CollectLogicModuleStates();
                var triggered = LogicGates.EvaluateRules(states);
                if (triggered == null || triggered.Count == 0) return;

                for (int i = 0; i < triggered.Count; i++)
                {
                    var rule = LogicGates.FindRule(triggered[i]);
                    if (rule != null)
                        ApplyLogicRuleAction(rule);
                }
            }
            finally
            {
                _logicGatesTicking = false;
            }
        }

        /// <summary>Build float samples for consumers, sources, and grid aggregates.</summary>
        private Dictionary<string, float> CollectLogicModuleStates()
        {
            var states = new Dictionary<string, float>(32);
            if (PowerNetwork == null) return states;

            states["generation"] = PowerNetwork.TotalGeneration;
            states["total_generation"] = PowerNetwork.TotalGeneration;
            states["draw"] = PowerNetwork.TotalDraw;
            states["total_draw"] = PowerNetwork.TotalDraw;
            states["requested_draw"] = PowerNetwork.RequestedDraw;
            states["blackout"] = PowerNetwork.IsBlackout ? 1f : 0f;
            states["load_shedding"] = PowerNetwork.IsLoadShedding ? 1f : 0f;
            states["co_ppm"] = PowerNetwork.CarbonMonoxidePpm;
            states["diesel_fuel"] = PowerNetwork.GetDieselFuelTotal();

            var consumers = PowerNetwork.Consumers;
            if (consumers != null)
            {
                for (int i = 0; i < consumers.Count; i++)
                {
                    var c = consumers[i];
                    if (c == null || string.IsNullOrEmpty(c.ModuleId)) continue;
                    // 1 = powered and drawing, 0 = unpowered / shed / unrequested.
                    states[c.ModuleId] = c.IsPowered ? 1f : 0f;
                    states[c.ModuleId + "_watts"] = c.EffectiveDraw;
                    states[c.ModuleId + "_requested"] = c.IsRequested ? 1f : 0f;
                }
            }

            var sources = PowerNetwork.Sources;
            if (sources != null)
            {
                for (int i = 0; i < sources.Count; i++)
                {
                    var s = sources[i];
                    if (s == null || string.IsNullOrEmpty(s.SourceId)) continue;
                    states[s.SourceId] = s.IsEnabled ? 1f : 0f;
                    states[s.SourceId + "_fuel"] = s.Fuel;
                }
            }

            return states;
        }

        /// <summary>Execute a rule's actionCommand against the power grid / sources.</summary>
        private void ApplyLogicRuleAction(LogicRule rule)
        {
            if (rule == null || PowerNetwork == null) return;
            if (string.IsNullOrEmpty(rule.actionModule) || string.IsNullOrEmpty(rule.actionCommand))
                return;

            string cmd = rule.actionCommand.Trim().ToLowerInvariant();
            string target = rule.actionModule;

            switch (cmd)
            {
                case System_LogicGates.CmdEnable:
                case System_LogicGates.CmdOn:
                case System_LogicGates.CmdRequest:
                    PowerNetwork.SetRequested(target, true);
                    break;
                case System_LogicGates.CmdDisable:
                case System_LogicGates.CmdOff:
                case System_LogicGates.CmdUnrequest:
                    PowerNetwork.SetRequested(target, false);
                    break;
                case System_LogicGates.CmdSourceOn:
                    PowerNetwork.SetSourceEnabled(target, true);
                    break;
                case System_LogicGates.CmdSourceOff:
                    PowerNetwork.SetSourceEnabled(target, false);
                    break;
                default:
                    Debug.LogWarning($"[GameBootstrap] LOGIC: unknown actionCommand '{rule.actionCommand}' on rule '{rule.ruleId}'.");
                    break;
            }
        }

        /// <summary>Prompt #799 — UI/test hook: add or replace an automation rule.</summary>
        public void AddLogicRule(string ruleId, LogicRule rule)
        {
            LogicGates?.AddRule(ruleId, rule);
        }

        /// <summary>Prompt #799 — UI/test hook: remove an automation rule.</summary>
        public void RemoveLogicRule(string ruleId)
        {
            LogicGates?.RemoveRule(ruleId);
        }

    }
}
