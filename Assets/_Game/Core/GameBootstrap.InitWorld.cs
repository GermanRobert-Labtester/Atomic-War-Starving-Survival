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
        private void InitWorldSideSystems()
        {
            InitCartographyAndTravel();
            InitFactionSideSystems();
            InitMapTaggedNodes();
            InitEcosystemAndHouseLayout();

            LocationQuestSystem = new LocationQuestSystem();
            InjectQuestNodesIntoMap();
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
            FloodedNodeSystem = new FloodedNodeSystem();
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
            PropagandaSystem = new PropagandaSystem();
            DeserterSystem = new DeserterSystem(new System.Random(_worldSeed + 75));
            ScapegoatSystem = new WeatherScapegoatSystem(new System.Random(_worldSeed + 76));
            LaborCampSystem = new LaborCampSystem();
            SeedMapNodes(0.1f, (nodeId) => LaborCampSystem.SetLaborCamp(nodeId, true), seedOffset: 77);
            CultMoralSystem = new CultMoralDisgustSystem();
        }

        private void InitMapTaggedNodes()
        {
            // Map tagging is done inside InitCartographyAndTravel / InitFactionSideSystems
            // via SeedMapNodes — kept as a named phase for audit readability.
        }

        private void InitEcosystemAndHouseLayout()
        {
            EcosystemSystem = new MutatedEcosystemSystem(CreateSaltedRng(_worldSeed, "ecosystem"));

            var layouts = Data.ShelterLayoutFactory.CreateAll();
            var layoutRng = CreateSaltedRng(_worldSeed, "shelter_layout");
            ShelterLayout = layouts[layoutRng.Next(layouts.Count)];
            Debug.Log($"[GameBootstrap] Selected shelter layout: {ShelterLayout.layoutName}");

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

        private void InitShelterTacticalSystems()
        {
            // Prompts #119–#128 — Shelter tactical systems
            ExcavationSystem = new ExcavationSystem(new System.Random(_worldSeed + 119));
            FloodingSystem = new RoomFloodingSystem();
            FloodingSystem.SetRng(new System.Random(_worldSeed + 120));
            HiddenStorageSystem = new HiddenStorageSystem();
            CeilingCollapseSystem = new CeilingCollapseSystem();
            PerimeterTrapSystem = new PerimeterTrapSystem();
            TunnelingSystem = new TunnelingSystem();
            TunnelingSystem.SeedNeighbor(new System.Random(_worldSeed + 124));
            HatchVisibilitySystem = new HatchVisibilitySystem();
            EscapeHatchSystem = new EscapeHatchSystem();
            MaterialShieldingSystem = new MaterialShieldingSystem();
            AirlockSystem = new AirlockSystem();

            // Prompts #164–#178 — simulation systems
            NoiseSystem = new NoiseSystem();
            ClothingSystem = new ClothingDegradationSystem();
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

    }
}
