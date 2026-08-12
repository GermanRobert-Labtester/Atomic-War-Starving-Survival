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
using AtomicWar._Game.Encounters;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        // Cached delegate guard for the Expansion IV coming-of-age HUD subscription.
        private Action<ComingOfAgeEvent> _onComingOfAgeHud;

        private void InitWorldSideSystems()
        {
            InitCartographyAndTravel();
            InitFactionSideSystems();
            InitMapTaggedNodes();
            InitEcosystemAndHouseLayout();

            LocationQuestSystem = new LocationQuestSystem();
            InjectQuestNodesIntoMap();
        }

        /// <summary>
        /// Wires the Expansion IV overlay controller to the four Chapter 45 systems.
        /// Called from WireHUD() after the HUD and the systems are both live.
        /// </summary>
        private void WireExpansion4HudController()
        {
            // Inspector reference wins; the controller lives beside
            // DiegeticHudController ([RequireComponent]) on the HUD, so scope
            // the fallback search there instead of a scene-wide FindObjectOfType.
            var controller = _expansion4HudController != null
                ? _expansion4HudController
                : (_hud != null ? _hud.GetComponentInChildren<Expansion4HudController>() : null);
            if (controller == null)
            {
                Debug.LogWarning("[InitWorld] Expansion4HudController not assigned and not found under the HUD. " +
                                 "Attach it to the HUD GameObject to enable Expansion IV overlays.");
                return;
            }

            controller.Bind(
                entropySystem: StructuralEntropySystem,
                letheSystem:   LetheProtocolSystem,
                ozoneSystem:   OzoneScourgeSystem,
                genSystem:     GenerationalPsychologySystem,
                getSurvivors:  () => Survivors);

            if (_onComingOfAgeHud != null)
                GenerationalPsychologySystem.OnComingOfAgeEventBus -= _onComingOfAgeHud;

            _onComingOfAgeHud = evt =>
            {
                if (_hud != null)
                    _hud.PushEventText(
                        $"{evt.DisplayName} has come of age. " +
                        "They have never known sunlight.");
            };
            GenerationalPsychologySystem.OnComingOfAgeEventBus += _onComingOfAgeHud;
        }

        private void InitCartographyAndTravel()
        {
            CartographySystem = new CartographySystem();
            CartographySystem.Bind(
                () => Shelter,
                itemId => Inventory?.CountById(itemId) ?? 0,
                (itemId, amount) =>
                {
                    if (Inventory != null)
                        Inventory.RemoveByType(AtomicWar._Game.Inventory.ItemType.Material, amount);
                });
            BicycleSystem = new BicycleSystem();
            BicycleSystem.SetNeedsSystem(NeedsSystem);
            FloodedNodeSystem = new FloodedNodeSystem();
            FloodedNodeSystem.SetNeedsSystem(NeedsSystem);
            SeedMapNodes(0.2f, (nodeId) => FloodedNodeSystem.SetFlooded(nodeId, true), seedOffset: 69);
            // River nodes generated after GeneratedMap exists (InitLate).
            RiverNodeSystem = new RiverNodeSystem();
        }

        private void InitFactionSideSystems()
        {
            TrackerSystem = new TrackerSystem(CreateSaltedRng(_worldSeed, "tracker"));
            DeadDropSystem = new DeadDropSystem(new System.Random(_worldSeed + 72));
            SeedMapNodes(0.15f, (nodeId) => DeadDropSystem.SetDeadDropNode(nodeId, true), seedOffset: 72);
            HostageSystem = new HostageSystem();
            HostageSystem.SetNeedsSystem(NeedsSystem);
            PropagandaSystem = new PropagandaSystem();
            DeserterSystem = new DeserterSystem(new System.Random(_worldSeed + 75));
            ScapegoatSystem = new WeatherScapegoatSystem(new System.Random(_worldSeed + 76));
            LaborCampSystem = new LaborCampSystem();
            SeedMapNodes(0.1f, (nodeId) => LaborCampSystem.SetLaborCamp(nodeId, true), seedOffset: 77);
            CultMoralSystem = new CultMoralDisgustSystem();
            CultMoralSystem.SetNeedsSystem(NeedsSystem);
        }

        private void InitMapTaggedNodes()
        {
            // Map tagging is done inside InitCartographyAndTravel / InitFactionSideSystems
            // via SeedMapNodes — kept as a named phase for audit readability.
        }

        private void InitEcosystemAndHouseLayout()
        {
            EcosystemSystem = new MutatedEcosystemSystem(CreateSaltedRng(_worldSeed, "ecosystem"));
            EcosystemSystem.SetNeedsSystem(NeedsSystem);
            EcosystemSystem.BindRadiation(RadiationSystem);

            var layouts = Data.ShelterLayoutFactory.CreateAll();
            var layoutRng = CreateSaltedRng(_worldSeed, "shelter_layout");
            ShelterLayout = layouts[layoutRng.Next(layouts.Count)];
            GameLog.Log($"[GameBootstrap] Selected shelter layout: {ShelterLayout.layoutName}");

            HouseToBunkerSystem = new HouseToBunkerSystem(CreateSaltedRng(_worldSeed, "house_to_bunker"));
            HouseToBunkerSystem.InitializeFromLayout(ShelterLayout);
            ApplyShelterLayout(ShelterLayout);
        }

        private void SeedMapNodes(float chance, Action<string> apply, int seedOffset)
        {
            if (GeneratedMap == null || apply == null) return;
            var rng = new System.Random(_worldSeed + seedOffset);
            for (int i = 0; i < GeneratedMap.Nodes.Count; i++)
            {
                var node = GeneratedMap.Nodes[i];
                if (node == null || node.IsShelter) continue;
                if (rng.NextDouble() < chance)
                    apply(node.NodeId);
            }
        }

        /// <summary>
        /// Section VII new-content batch — sleep deprivation, grief, shelter
        /// degradation, ash accumulation, disease mutation, noise discipline,
        /// calorie accounting. Construction + host callbacks only; tick
        /// registration lives in GameBootstrap.Registry.cs and save wiring in
        /// GameBootstrap.InitLate.cs.
        /// </summary>
        private void InitSection7Systems()
        {
            // Noise discipline has no autonomous tick — hosts register/toggle
            // sources (generator, hammering, radio) and it recomputes on change.
            NoiseDiscipline = new NoiseDisciplineSystem();

            SleepDeprivation = new SleepDeprivationSystem
            {
                GetShelterNoiseLevel = () => NoiseDiscipline != null ? NoiseDiscipline.CurrentLevel : 0f,
                // System expects a 0..1 comfort scalar centred on 0.5; reuse the
                // Prompt #32 temperature→quality curve over live indoor °C.
                GetRoomTemperatureForSurvivor = _ => SleepQualitySystem.TemperatureMultiplier(
                    TemperatureSystem != null ? TemperatureSystem.GetIndoorTemperature(Shelter) : 15f),
                ApplyMoraleDelta = (sv, delta) => NeedsSystem?.Modify(sv, NeedKind.Morale, delta)
            };

            Grief = new GriefSystem
            {
                GetDay = () => TimeSystem != null ? TimeSystem.CurrentDay : 0,
                // InterpersonalAffinity.Get is [-100,+100]; grief thresholds want 0..1.
                GetAffinity = (a, b) => Mathf.InverseLerp(-100f, 100f,
                    MentalBreakSystem != null ? MentalBreakSystem.Affinity.Get(a?.Id, b?.Id) : 0f),
                ApplyMoraleDelta = (sv, delta) => NeedsSystem?.Modify(sv, NeedKind.Morale, delta),
                AddAffliction = (sv, afflictionId) => MedicalSystem?.Inflict(sv, afflictionId),
                Rng = CreateSaltedRng(_worldSeed, "grief")
            };

            ShelterDegradation = new ShelterDegradationSystem
            {
                GetDay = () => TimeSystem != null ? TimeSystem.CurrentDay : 0,
                GetShelterNoiseLevel = () => NoiseDiscipline != null ? NoiseDiscipline.CurrentLevel : 0f,
                RequestConsumeItem = (itemId, count) =>
                {
                    if (Inventory != null && !string.IsNullOrEmpty(itemId))
                        Inventory.RemoveById(itemId, Mathf.Max(1, Mathf.CeilToInt(count)));
                },
                AddWaterLossPerDay = (sourceId, litersPerDay) =>
                    GameLog.Log($"[ShelterDegradation] Water loss {litersPerDay:0.#} L/day registered: {sourceId}."),
                QueueFireRisk = (sourceId, chance, scale) =>
                    GameLog.LogWarning($"[ShelterDegradation] Fire risk queued from {sourceId}: {chance:P0} (x{scale:0.##})."),
                Rng = CreateSaltedRng(_worldSeed, "shelter_degradation")
            };

            AshAccumulation = new AshAccumulationSystem
            {
                GetDay = () => TimeSystem != null ? TimeSystem.CurrentDay : 0,
                IsAshStormActive = () => WeatherSystem != null
                    && (WeatherSystem.Current == WeatherKind.Ashfall
                        || WeatherSystem.Current == WeatherKind.FalloutStorm),
                ApplyFilterClogMultiplier = (sourceId, baseRate, multiplier) =>
                    GameLog.Log($"[AshAccumulation] Filter clog x{multiplier:0.#} from {sourceId}."),
                BurySurfaceNode = nodeId =>
                    GameLog.Log($"[AshAccumulation] Surface node buried in ash: {nodeId}."),
                ReduceSolarOutput = fraction =>
                    GameLog.Log($"[AshAccumulation] Solar output reduced by {fraction:P0}."),
                GetRosterSize = () =>
                {
                    int alive = 0;
                    if (Survivors != null)
                        for (int i = 0; i < Survivors.Count; i++)
                            if (Survivors[i] != null && Survivors[i].IsAlive) alive++;
                    return Mathf.Max(1, alive);
                }
            };

            DiseaseMutation = new DiseaseMutationSystem
            {
                RequestConsumeItem = (itemId, count) =>
                {
                    if (Inventory != null && !string.IsNullOrEmpty(itemId))
                        Inventory.RemoveById(itemId, Mathf.Max(1, Mathf.CeilToInt(count)));
                },
                SpawnAffliction = afflictionId =>
                    GameLog.LogWarning($"[DiseaseMutation] Infection escalated: {afflictionId}."),
                GetItemId = itemId =>
                    _itemCatalog != null && _itemCatalog.GetById(itemId) != null ? itemId : null,
                Rng = CreateSaltedRng(_worldSeed, "disease_mutation")
            };

            CalorieAccounting = new CalorieAccountingSystem();

            GameLog.Log("[Section7] Sleep/grief/degradation/ash/mutation/noise/calorie systems initialized.");
        }

        private void InitShelterTacticalSystems()
        {
            // Prompts #119–#128 — Shelter tactical systems
            ExcavationSystem = new ExcavationSystem(new System.Random(_worldSeed + 119));
            ExcavationSystem.SetNeedsSystem(NeedsSystem);
            FloodingSystem = new RoomFloodingSystem();
            FloodingSystem.SetNeedsSystem(NeedsSystem);
            FloodingSystem.SetRng(new System.Random(_worldSeed + 120));
            // Prompt #806 — bilge pumps convert floodwater into purified cistern water.
            BilgePumps = new System_BilgePumps();
            WireBilgePumps();
            HiddenStorageSystem = new HiddenStorageSystem();
            CeilingCollapseSystem = new CeilingCollapseSystem();
            PerimeterTrapSystem = new PerimeterTrapSystem();
            TunnelingSystem = new TunnelingSystem();
            TunnelingSystem.SetNeedsSystem(NeedsSystem);
            TunnelingSystem.SeedNeighbor(new System.Random(_worldSeed + 124));
            HatchVisibilitySystem = new HatchVisibilitySystem();
            // Prompt #658 — outdoor carrion attracts vultures (wired to CorpseSystem in InitLate).
            CarrionBirds = new System_CarrionBirds();
            EscapeHatchSystem = new EscapeHatchSystem();
            MaterialShieldingSystem = new MaterialShieldingSystem();
            // Ceiling material is one more shielding layer in the interior-rad formula;
            // without this hook every ceiling upgrade the player buys does nothing.
            if (Shelter != null)
                Shelter.CeilingAttenuationProvider =
                    () => MaterialShieldingSystem != null
                        ? MaterialShieldingSystem.GetWeakestCeilingAttenuation()
                        : 0f;
            AirlockSystem = new AirlockSystem();
            AirlockSystem.SetNeedsSystem(NeedsSystem);

            // Prompts #164–#178 — simulation systems
            NoiseSystem = new NoiseSystem();
            ClothingSystem = new ClothingDegradationSystem();
            ClothingSystem.SetNeedsSystem(NeedsSystem);
            // Audit C-1: the wiring object is the single tick orchestrator for
            // the systems added in Prompts #119–#178. Created here so it can
            // hold a day-counter across substeps; TickSystems calls it once per
            // game-day (idempotent).
            _systemWiring = new SystemWiring();
            ResilienceSystem = new ResilienceSystem();
            CompostSystem = new CompostSystem();
            ScrapWeaponSystem = new ScrapWeaponSystem();
            SterilizationSystem = new SterilizationSystem();
            ChelationSystem = new ChelationSystem();
            WindTurbineSystem = new WindTurbineSystem();
            AntibioticResistSystem = new AntibioticResistanceSystem();
            HaulingSystem = new InternalHaulingSystem();
            WeaponMaintenanceSystem = new WeaponMaintenanceSystem();
            AestheticsSystem = new RoomAestheticsSystem();
            HamRadioSystem = new HamRadioSystem();
            TriageSystem = new TriageBoardSystem();
            PolypharmacySystem = new PolypharmacySystem();
            // ChemUse is built during medical init (before this); re-bind so polypharmacy is live.
            ChemUse?.Bind(
                Addiction,
                BloodToxicity,
                PolypharmacySystem,
                ChemTolerance,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 1,
                getGameHours: () => TimeSystem != null ? TimeSystem.TotalElapsedHours : 0f);

            // ───────────────────────────────────────────────────────────
        }


        private void InitAtmosphereHygieneSystems()
        {
            // Prompt #49 — Structural Integrity & Cave-ins
            // ───────────────────────────────────────────────────────────
            StructuralIntegrity = new StructuralIntegritySystem(
                new System.Random(_worldSeed + 49));
            StructuralIntegrity.Bind(
                () => Shelter,
                () => Survivors,
                (sv, traumaId) =>
                {
                    if (sv != null && !sv.HasTrauma(traumaId))
                        sv.Traumas.Add(traumaId);
                });

            // Prompt #50 — Waste Management & Hygiene
            // ───────────────────────────────────────────────────────────
            WasteSystem = new WasteSystem(new System.Random(_worldSeed + 50));
            WasteSystem.SetNeedsSystem(NeedsSystem);
            WasteSystem.Bind(() => Shelter, () => Survivors);

            // Prompt #51 — Vermin Infestations (+ PetSystem cats suppress growth)
            // ───────────────────────────────────────────────────────────
            PetSystem = new PetSystem(
                NeedsSystem,
                depositRoomContamination: (roomId, amount) =>
                {
                    if (string.IsNullOrEmpty(roomId) || amount <= 0f || Shelter == null)
                        return;
                    var room = Shelter.GetRoom(roomId);
                    if (room == null) return;
                    // AmbientContamination is 0..1; pet fur deposit is small absolute dust.
                    room.AmbientContamination = Mathf.Clamp01(
                        room.AmbientContamination + amount * 0.01f);
                },
                addBunkerContamination: amount =>
                {
                    if (amount <= 0f || Shelter?.Rooms == null) return;
                    float perRoom = (amount * 0.01f) / Mathf.Max(1, Shelter.Rooms.Count);
                    for (int i = 0; i < Shelter.Rooms.Count; i++)
                    {
                        var room = Shelter.Rooms[i];
                        if (room == null) continue;
                        room.AmbientContamination = Mathf.Clamp01(
                            room.AmbientContamination + perRoom);
                    }
                });
            // PersonalQuests already exists (InitFoundation runs before InitEventsAndSurvivors).
            PetSystem.BindPersonalQuests(PersonalQuests, () => Survivors);

            VerminSystem = new VerminSystem(new System.Random(_worldSeed + 51));
            VerminSystem.SetNeedsSystem(NeedsSystem);
            VerminSystem.Bind(
                () => Shelter,
                () => PetSystem,
                () => WasteSystem != null ? WasteSystem.Hygiene : 100f);

            // Prompt #380 — fuel varnish degradation (daily → diesel burn mult).
            FuelDecaySystem = new FuelDecaySystem();

            // Prompt #52 — Module Jury-Rigging
            // ───────────────────────────────────────────────────────────
            JuryRigSystem = new JuryRigSystem(new System.Random(_worldSeed + 52));
            JuryRigSystem.Bind(() => Shelter);
            JuryRigSystem.StartFireInRoom = (roomId, intensity) =>
            {
                if (AtmosphereSystem != null && Shelter != null)
                {
                    var room = AtmosphereSystem.GetRoom(roomId);
                    if (room == null)
                    {
                        room = new ShelterRoom(roomId, null);
                        AtmosphereSystem.RegisterRoom(room);
                    }
                    if (!room.IsOnFire)
                        AtmosphereSystem.StartFire(room, intensity);
                }
            };

            // Prompt #53 — Freezing Pipes & Water Loss
            // ───────────────────────────────────────────────────────────
            FreezePipeSystem = new FreezePipeSystem();
            FreezePipeSystem.Bind(
                () => TemperatureSystem != null && Shelter != null
                    ? TemperatureSystem.GetIndoorTemperature(Shelter)
                    : 20f,
                () => WaterStorage);

            // ───────────────────────────────────────────────────────────
        }

        /// <summary>
        /// Prompt #806 — on room flood, harvest floodwater through bilge pumps into clean cistern.
        /// </summary>
        private void WireBilgePumps()
        {
            if (BilgePumps == null || FloodingSystem == null) return;

            FloodingSystem.OnRoomFlooded += roomId =>
            {
                if (BilgePumps == null || !BilgePumps.IsActive()) return;
                // One room just flooded — route a single room's worth of floodwater.
                float purified = BilgePumps.ProcessFloodedRooms(1);
                if (purified > 0f && WaterStorage != null)
                    WaterStorage.AddClean(purified);
            };

            BilgePumps.OnWaterRouted += liters =>
            {
                if (liters > 0f)
                    GameLog.Log($"[GameBootstrap] BILGE: routed {liters:0.#} L purified from floodwater.");
            };
        }

        /// <summary>
        /// Prompt #806 — daily pass: while any room is flooded and pumps are active,
        /// convert remaining floodwater into clean storage.
        /// </summary>
        private void TickBilgePumpsDaily()
        {
            if (BilgePumps == null || !BilgePumps.IsActive()) return;
            if (FloodingSystem == null) return;

            int flooded = FloodingSystem.FloodedRooms != null ? FloodingSystem.FloodedRooms.Count : 0;
            if (flooded <= 0) return;

            float purified = BilgePumps.ProcessFloodedRooms(flooded);
            if (purified > 0f && WaterStorage != null)
                WaterStorage.AddClean(purified);
        }

        /// <summary>Prompt #806 — UI/craft hook: power the bilge pumps on.</summary>
        public void ActivateBilgePumps()
        {
            BilgePumps?.Activate();
        }

        /// <summary>Prompt #806 — UI hook: shut bilge pumps down.</summary>
        public void DeactivateBilgePumps()
        {
            BilgePumps?.Deactivate();
        }

        /// <summary>
        /// Prompt #658 — outdoor corpse disposal attracts carrion birds; birds mark
        /// hatch visibility, shelter map danger, and daily morale pressure.
        /// </summary>
        private void WireCarrionBirds()
        {
            if (CarrionBirds == null) return;

            if (CorpseSystem != null)
            {
                // Burying outside leaves remains beyond the hatch — vultures notice.
                CorpseSystem.OnCorpseBuried += _ => CarrionBirds?.AddCorpse();
            }

            if (WasteSystem != null)
            {
                // Outdoor waste dumps also draw scavengers when outdoor corpses exist
                // (reinforces presence without adding corpses).
                WasteSystem.OnOutsideDisposal += _ =>
                {
                    if (CarrionBirds == null || CarrionBirds.CorpseCount <= 0) return;
                    // Nudge hatch visibility immediately if flock already circling.
                    if (CarrionBirds.VulturesPresent)
                        ApplyCarrionHatchVisibility();
                };
            }

            CarrionBirds.OnVulturesArrived += _ =>
            {
                ApplyCarrionHatchVisibility();
                ApplyCarrionMapDanger(true);
                GameLog.Log("[GameBootstrap] CARRION: vultures circling the hatch.");
            };

            CarrionBirds.OnVulturesDeparted += _ =>
            {
                ApplyCarrionMapDanger(false);
                GameLog.Log("[GameBootstrap] CARRION: flock dispersed.");
            };
        }

        /// <summary>
        /// Prompt #658 — daily: arrive/depart flock, re-assert hatch mark, morale pressure.
        /// </summary>
        private void TickCarrionBirdsDaily()
        {
            if (CarrionBirds == null) return;

            CarrionBirds.TickDay();

            if (!CarrionBirds.VulturesPresent) return;

            ApplyCarrionHatchVisibility();
            ApplyCarrionMapDanger(true);
            ApplyCarrionMoralePressure();
        }

        private void ApplyCarrionHatchVisibility()
        {
            if (HatchVisibilitySystem == null || CarrionBirds == null) return;
            float target = CarrionBirds.GetHatchVisibility();
            if (target <= 0f) return;
            float current = HatchVisibilitySystem.Visibility;
            if (current < target)
                HatchVisibilitySystem.AddVisibility(target - current);
        }

        private void ApplyCarrionMapDanger(bool present)
        {
            var node = GeneratedMap?.ShelterNode;
            if (node == null) return;

            if (present && !_carrionMapDangerApplied)
            {
                node.DangerLevel += System_CarrionBirds.MapDangerBoost;
                _carrionMapDangerApplied = true;
                GeneratedMap.NotifyMapChanged();
            }
            else if (!present && _carrionMapDangerApplied)
            {
                node.DangerLevel = Mathf.Max(0f, node.DangerLevel - System_CarrionBirds.MapDangerBoost);
                _carrionMapDangerApplied = false;
                GeneratedMap.NotifyMapChanged();
            }
        }

        private void ApplyCarrionMoralePressure()
        {
            if (NeedsSystem == null || Survivors == null) return;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                NeedsSystem.Modify(sv, NeedKind.Morale, -System_CarrionBirds.MoralePressurePerDay);
            }
        }

        /// <summary>Prompt #658 — UI/test hook: clear outdoor corpses so the flock can leave.</summary>
        public void ClearOutdoorCarrion()
        {
            CarrionBirds?.RemoveCorpses();
        }

        /// <summary>
        /// Protocol Zero — Permafrost expansion systems.
        /// Thermal grid, Ash-Tide scheduler, atmosphere toxicity, and convoy logistics.
        /// </summary>
        private void InitProtocolZeroSystems()
        {
            // ── Thermal Grid System ────────────────────────────────────
            ThermalGrid = new ThermalGridSystem();
            ThermalGrid.SetOutdoorTemperature(-15f);

            // Register all shelter rooms in the thermal grid.
            if (Shelter?.Rooms != null)
            {
                for (int i = 0; i < Shelter.Rooms.Count; i++)
                {
                    var room = Shelter.Rooms[i];
                    if (room == null || string.IsNullOrEmpty(room.RoomId)) continue;
                    float rValue = room.RoomId == "entry" ? 1.5f : 2.5f;
                    float startTemp = room.RoomId == "plant" ? 20f : 15f;
                    ThermalGrid.RegisterRoom(room, rValue, startTemp);
                }

                // Connect adjacent rooms for heat transfer.
                // Entry ↔ Quarters, Quarters ↔ Plant, Plant ↔ Stores.
                ThermalGrid.ConnectRooms("entry", "quarters");
                ThermalGrid.ConnectRooms("quarters", "plant");
                ThermalGrid.ConnectRooms("plant", "stores");
            }

            // Wire pipe burst → RoomFloodingSystem.
            Action<ThermalPipeBurstEvent> onPipeBurst = burst =>
            {
                GameLog.Log($"[ProtocolZero] Pipe burst in room '{burst.RoomId}' at {burst.FloodTemperature:F0}C!");
                FloodingSystem?.ForceFlood(burst.RoomId);
            };
            ThermalGrid.OnPipeBurst += onPipeBurst;
            _subscriptions.Track(() => ThermalGrid.OnPipeBurst -= onPipeBurst);

            // ── Ash-Tide Scheduler ─────────────────────────────────────
            AshTideScheduler = new AshTideScheduler();

            Action<AshTideArrivedEvent> onTideArrived = tide =>
            {
                GameLog.Log($"[ProtocolZero] ASH-TIDE ARRIVED on Day {tide.Day}. " +
                    $"Duration: {tide.DurationHours}h. Surface travel locked.");
                SaveSystem?.SetWorldFlag("ash_tide_active", true);
            };
            AshTideScheduler.OnTideArrived += onTideArrived;
            _subscriptions.Track(() => AshTideScheduler.OnTideArrived -= onTideArrived);

            Action<AshTideRecededEvent> onTideReceded = result =>
            {
                GameLog.Log($"[ProtocolZero] Ash-Tide receded. Total endured: {result.TotalTidesEndured}.");
                SaveSystem?.SetWorldFlag("ash_tide_active", false);
            };
            AshTideScheduler.OnTideReceded += onTideReceded;
            _subscriptions.Track(() => AshTideScheduler.OnTideReceded -= onTideReceded);

            // ── Atmosphere Toxicity System ────────────────────────────
            AtmosphereToxicity = new AtmosphereToxicitySystem();

            Action<ToxicityThresholdEvent> onThresholdCrossed = evt =>
            {
                if (evt.IsAsphyxiation)
                    GameLog.LogError($"[ProtocolZero] ASPHYXIATION DANGER: CO2 at {evt.CurrentLevel:F0} ppm!");
                else if (evt.IsHypoxia)
                    GameLog.LogWarning($"[ProtocolZero] Hypoxia warning: CO2 at {evt.CurrentLevel:F0} ppm.");
                else if (evt.IsCOPoisoning)
                    GameLog.LogWarning($"[ProtocolZero] CO poisoning: {evt.CurrentLevel:F0} ppm.");
            };
            AtmosphereToxicity.OnThresholdCrossed += onThresholdCrossed;
            _subscriptions.Track(() => AtmosphereToxicity.OnThresholdCrossed -= onThresholdCrossed);

            Action onScrubberExpired = () =>
            {
                GameLog.LogWarning("[ProtocolZero] CO2 scrubber cartridge expired. Replace immediately.");
            };
            AtmosphereToxicity.OnScrubberExpired += onScrubberExpired;
            _subscriptions.Track(() => AtmosphereToxicity.OnScrubberExpired -= onScrubberExpired);

            // ── Convoy Logistics System ───────────────────────────────
            ConvoyLogistics = new ConvoyLogisticsSystem();

            // Register known destinations from the new location data.
            RegisterConvoyDestinations();

            Action<ConvoyBlizzardEvent> onBlizzardCaught = blizzard =>
            {
                GameLog.Log($"[ProtocolZero] Convoy mission '{blizzard.MissionId}' caught in blizzard! " +
                    (blizzard.HasSled ? "Sled dilemma triggered." : "Snow-Crawler is shielded."));
            };
            ConvoyLogistics.OnBlizzardCaught += onBlizzardCaught;
            _subscriptions.Track(() => ConvoyLogistics.OnBlizzardCaught -= onBlizzardCaught);

            Action<string> onCrawlerDisabled = (reason) =>
            {
                GameLog.LogError($"[ProtocolZero] Snow-Crawler disabled ({reason})! Repair oil/tracks to restore.");
            };
            ConvoyLogistics.OnCrawlerDisabled += onCrawlerDisabled;
            _subscriptions.Track(() => ConvoyLogistics.OnCrawlerDisabled -= onCrawlerDisabled);

            // ── Save wiring ───────────────────────────────────────────
            if (SaveSystem != null)
            {
                SaveSystem.SetThermalGridSystem(ThermalGrid);
                SaveSystem.SetAshTideScheduler(AshTideScheduler);
                SaveSystem.SetAtmosphereToxicitySystem(AtmosphereToxicity);
                SaveSystem.SetConvoyLogisticsSystem(ConvoyLogistics);
            }

            GameLog.Log("[ProtocolZero] All permafrost systems initialized and save-wired.");
        }

        // ── Expansion II Addendum: Black Aquifer & Myco-Necrosis ─────
        private void InitBlackAquiferSystems()
        {
            // Hydrostatic Pressure
            HydrostaticPressure = new HydrostaticPressureSystem();

            Action<HydrostaticThresholdEvent> onHydroThreshold = evt =>
            {
                if (evt.IsSludgeBreakthrough)
                    GameLog.Log($"[BlackAquifer] Sludge breakthrough — toxicity {evt.ToxicityIndex:P0}. Water is poisoned.");
                if (evt.IsFilterDamage)
                    GameLog.Log($"[BlackAquifer] Filter damage active — purifier degrading {HydrostaticPressureSystem.FilterDamageMultiplier}x faster.");
            };
            HydrostaticPressure.OnThresholdCrossed += onHydroThreshold;
            _subscriptions.Track(() => HydrostaticPressure.OnThresholdCrossed -= onHydroThreshold);

            // Tunneling & Structural Stress
            TunnelingStress = new TunnelingAndStructuralStress();

            Action<TunnelingCaveInEvent> onCaveIn = evt =>
            {
                GameLog.Log($"[BlackAquifer] CAVE-IN at {evt.RoomId} (level {evt.Level}). Stress {evt.OverburdenStress:F0}/{evt.MaterialThreshold:F0}. Rebuild: {TunnelingAndStructuralStress.CaveInRebuildHours:F0}h.");
                EventRunner?.ScheduleEvent("crisis_structural_failure", TimeSystem?.CurrentDay ?? 1, "cave_in");
            };
            TunnelingStress.OnCaveIn += onCaveIn;
            _subscriptions.Track(() => TunnelingStress.OnCaveIn -= onCaveIn);

            Action<TunnelingGasPocketEvent> onGasPocket = evt =>
            {
                GameLog.Log($"[BlackAquifer] Gas pocket hit in {evt.RoomId}: {evt.GasType} at {evt.Concentration:F0} ppm.");
            };
            TunnelingStress.OnGasPocketHit += onGasPocket;
            _subscriptions.Track(() => TunnelingStress.OnGasPocketHit -= onGasPocket);

            // Mycelium Network
            MyceliumNetwork = new MyceliumNetworkSystem();

            // Register all shelter rooms for spore tracking
            if (Shelter?.Rooms != null)
            {
                for (int i = 0; i < Shelter.Rooms.Count; i++)
                {
                    if (Shelter.Rooms[i] != null)
                        MyceliumNetwork.RegisterRoom(Shelter.Rooms[i].RoomId);
                }
            }

            // Wire corpse integration
            if (CorpseSystem != null)
            {
                CorpseSystem.OnCorpseCreated += (sv, def) => MyceliumNetwork.OnCorpseSpawned(sv);
                CorpseSystem.OnCorpseBuried += id => MyceliumNetwork.OnCorpseResolved(id);
                CorpseSystem.OnCorpseProcessedForFertilizer += id => MyceliumNetwork.OnCorpseResolved(id);
            }

            // Wire host callbacks
            MyceliumNetwork.InflictAffliction = (svId, affId) =>
            {
                if (MedicalSystem == null || Survivors == null) return;
                var sv = Survivors.Find(s => s.Id == svId);
                if (sv != null) MedicalSystem.Inflict(sv, affId);
            };
            MyceliumNetwork.GetSurvivorsInRoom = roomId =>
            {
                if (Survivors == null) return null;
                var list = new List<Survivor>();
                for (int i = 0; i < Survivors.Count; i++)
                    if (Survivors[i]?.CurrentRoomId == roomId) list.Add(Survivors[i]);
                return list;
            };

            Action<SporeBloomEvent> onBloom = evt =>
            {
                GameLog.Log($"[BlackAquifer] SPORE BLOOM in {evt.RoomId} — density {evt.SporeDensity:F0}%. Corpse: {evt.SourceCorpseId}.");
                EventRunner?.ScheduleEvent("event_spore_bloom", TimeSystem?.CurrentDay ?? 1, "mycelium");
            };
            MyceliumNetwork.OnSporeBloom += onBloom;
            _subscriptions.Track(() => MyceliumNetwork.OnSporeBloom -= onBloom);

            // Save wiring
            if (SaveSystem != null)
            {
                SaveSystem.SetHydrostaticPressureSystem(HydrostaticPressure);
                SaveSystem.SetTunnelingStressSystem(TunnelingStress);
                SaveSystem.SetMyceliumNetworkSystem(MyceliumNetwork);
            }

            GameLog.Log("[BlackAquifer] Hydrostatic pressure, tunneling stress, and mycelium network initialized.");
        }

        // ── Expansion III: The Dead Hand & The Oxide Wastes ──────────
        private void InitDeadHandSystems()
        {
            // UXO Field System
            UXOField = new UXOFieldSystem();

            // Wire host callbacks
            UXOField.SurvivorHasTrait = traitId =>
            {
                if (Survivors == null) return false;
                for (int i = 0; i < Survivors.Count; i++)
                    if (Survivors[i] != null && Survivors[i].HasTrait(traitId)) return true;
                return false;
            };
            UXOField.InflictAffliction = (svId, affId) =>
            {
                if (MedicalSystem == null || Survivors == null) return;
                var sv = Survivors.Find(s => s.Id == svId);
                if (sv != null) MedicalSystem.Inflict(sv, affId);
            };
            UXOField.ApplyHealthDamage = (svId, damage) =>
            {
                if (NeedsSystem == null || Survivors == null) return;
                var sv = Survivors.Find(s => s.Id == svId);
                if (sv != null) NeedsSystem.Modify(sv, NeedKind.Health, -damage);
            };

            Action<UXOProbeResult> onProbe = result =>
            {
                if (result.Detonated)
                    GameLog.Log($"[DeadHand] UXO probe detonated at {result.NodeId}. Survivor {result.SurvivorId} hit. Skill check: {result.SkillCheckRoll:F2}.");
                else if (result.MineFound)
                    GameLog.Log($"[DeadHand] Mine found and disarmed at {result.NodeId} by {result.SurvivorId}.");
            };
            UXOField.OnProbeResult += onProbe;
            _subscriptions.Track(() => UXOField.OnProbeResult -= onProbe);

            Action<LoiteringMunitionEvent> onMunition = evt =>
            {
                GameLog.Log($"[DeadHand] Loitering munition attracted — acoustic signature {evt.AcousticSignature:F0} exceeds threshold {evt.Threshold:F0}.");
            };
            UXOField.OnLoiteringMunitionAttracted += onMunition;
            _subscriptions.Track(() => UXOField.OnLoiteringMunitionAttracted -= onMunition);

            // Automated Threat System
            AutomatedThreats = new Encounters.AutomatedThreatSystem();

            // Register sentries at key Dead Hand locations
            AutomatedThreats.RegisterSentry("sentry_uxo_highway", "location_uxo_highway_choke", 30f);
            AutomatedThreats.RegisterSentry("sentry_convoy_yard", "location_abandoned_convoy_yard", 25f);
            AutomatedThreats.RegisterSentry("sentry_mortar_pit", "location_automated_mortar_pit", 35f);

            AutomatedThreats.ApplyHealthDamage = (svId, damage) =>
            {
                if (NeedsSystem == null || Survivors == null) return;
                var sv = Survivors.Find(s => s.Id == svId);
                if (sv != null) NeedsSystem.Modify(sv, NeedKind.Health, -damage);
            };
            AutomatedThreats.InflictAffliction = (svId, affId) =>
            {
                if (MedicalSystem == null || Survivors == null) return;
                var sv = Survivors.Find(s => s.Id == svId);
                if (sv != null) MedicalSystem.Inflict(sv, affId);
            };

            Action<SentryBurnoutEvent> onBurnout = evt =>
            {
                GameLog.Log($"[DeadHand] Sentry {evt.SentryId} burned out at {evt.LocationNodeId}. {evt.TotalRoundsFired:F0} rounds fired. Barrel melted: {evt.BarrelMelted}.");
            };
            AutomatedThreats.OnSentryBurnedOut += onBurnout;
            _subscriptions.Track(() => AutomatedThreats.OnSentryBurnedOut -= onBurnout);

            // Electromagnetic Decay System
            ElectromagneticDecay = new Shelter.ElectromagneticDecaySystem();

            // Register shelter rooms for EM tracking
            if (Shelter?.Rooms != null)
            {
                for (int i = 0; i < Shelter.Rooms.Count; i++)
                {
                    if (Shelter.Rooms[i] != null)
                        ElectromagneticDecay.RegisterRoom(Shelter.Rooms[i].RoomId);
                }
            }

            // Register critical devices — ensure rooms exist first
            ElectromagneticDecay.RegisterRoom("radio");
            ElectromagneticDecay.RegisterRoom("medical");
            ElectromagneticDecay.RegisterRoom("plant");
            ElectromagneticDecay.RegisterDevice("radio", "radio");
            ElectromagneticDecay.RegisterDevice("medical", "autodoc");
            ElectromagneticDecay.RegisterDevice("plant", "water_purifier");

            ElectromagneticDecay.ConsumeItem = itemId =>
            {
                if (Inventory == null) return false;
                var def = Inventory.FindSlot(itemId)?.Item;
                return def != null && Inventory.Remove(def, 1);
            };

            Action<DeviceCorruptionEvent> onCorrupt = evt =>
            {
                if (evt.IsLogicGateFailure)
                    GameLog.Log($"[DeadHand] LOGIC GATE FAILURE — {evt.ModuleId} in {evt.RoomId}. Corruption: {evt.CorruptionLevel:F0}%.");
                else
                    GameLog.Log($"[DeadHand] Device corruption — {evt.ModuleId} in {evt.RoomId}. Corruption: {evt.CorruptionLevel:F0}%.");
            };
            ElectromagneticDecay.OnDeviceCorrupted += onCorrupt;
            _subscriptions.Track(() => ElectromagneticDecay.OnDeviceCorrupted -= onCorrupt);

            // Save wiring
            if (SaveSystem != null)
            {
                SaveSystem.SetUXOFieldSystem(UXOField);
                SaveSystem.SetAutomatedThreatSystem(AutomatedThreats);
                SaveSystem.SetElectromagneticDecaySystem(ElectromagneticDecay);
            }

            GameLog.Log("[DeadHand] UXO field, automated threats, and electromagnetic decay initialized.");
        }

        /// <summary>
        /// Register the 10 new Protocol Zero locations as convoy destinations.
        /// </summary>
        private void RegisterConvoyDestinations()
        {
            if (ConvoyLogistics == null) return;

            // These match the new locations added in locations.json.
            ConvoyLogistics.RegisterDestination("location_geo_thermal_plant_ruins", "Geo-Thermal Plant Ruins", 45f, 6f);
            ConvoyLogistics.RegisterDestination("location_arcology_sector_4", "Arcology Sector 4", 60f, 8f);
            ConvoyLogistics.RegisterDestination("location_frozen_river_barge", "Frozen River Barge", 30f, 4.5f);
            ConvoyLogistics.RegisterDestination("location_crashed_icebreaker_convoy", "Crashed Icebreaker Convoy", 35f, 5f);
            ConvoyLogistics.RegisterDestination("location_silent_observatory", "The Silent Observatory", 55f, 7f);
            ConvoyLogistics.RegisterDestination("location_subterranean_seed_vault", "Subterranean Seed Vault", 40f, 5.5f);
            ConvoyLogistics.RegisterDestination("location_ministry_of_truth_bunker", "Ministry of Truth Bunker", 40f, 6f);
            ConvoyLogistics.RegisterDestination("location_ash_dune_cemetery", "Ash Dune Cemetery", 20f, 3f);
            ConvoyLogistics.RegisterDestination("location_abandoned_ski_resort", "Abandoned Ski Resort", 25f, 4f);
            ConvoyLogistics.RegisterDestination("location_geothermal_borehole_site", "Geothermal Borehole Site", 35f, 5f);
        }
    }
}
