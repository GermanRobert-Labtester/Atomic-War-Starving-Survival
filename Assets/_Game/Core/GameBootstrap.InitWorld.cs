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
            // Prompt #67 — Cartography Table
            // ───────────────────────────────────────────────────────────
            CartographySystem = new CartographySystem();
            CartographySystem.Bind(
                () => Shelter,
                itemId => Inventory?.CountById(itemId) ?? 0,
                (itemId, amount) => { if (Inventory != null) Inventory.RemoveByType(AtomicWar._Game.Inventory.ItemType.Material, amount); });

            // Prompt #68 — Bicycle Logistics
            // ───────────────────────────────────────────────────────────
            BicycleSystem = new BicycleSystem();

            // Prompt #69 — Flooded Ruins
            // ───────────────────────────────────────────────────────────
            FloodedNodeSystem = new FloodedNodeSystem();
            // Mark some nodes as flooded from proc-gen seed.
            if (GeneratedMap != null)
            {
                var rng = new System.Random(_worldSeed + 69);
                for (int i = 0; i < GeneratedMap.Nodes.Count; i++)
                {
                    var node = GeneratedMap.Nodes[i];
                    if (node == null || node.IsShelter) continue;
                    if (rng.NextDouble() < 0.2f) // 20% of nodes flooded
                        FloodedNodeSystem.SetFlooded(node.NodeId, true);
                }
            }

            // Prompt #71 — Tracker System
            // ───────────────────────────────────────────────────────────
            TrackerSystem = new TrackerSystem(new System.Random(_worldSeed + 71));

            // Prompt #72 — Dead Drops
            // ───────────────────────────────────────────────────────────
            DeadDropSystem = new DeadDropSystem(new System.Random(_worldSeed + 72));
            // Mark some nodes as dead-drop sites.
            if (GeneratedMap != null)
            {
                var rng = new System.Random(_worldSeed + 72);
                for (int i = 0; i < GeneratedMap.Nodes.Count; i++)
                {
                    var node = GeneratedMap.Nodes[i];
                    if (node == null || node.IsShelter) continue;
                    if (rng.NextDouble() < 0.15f) // 15% of nodes are dead-drops
                        DeadDropSystem.SetDeadDropNode(node.NodeId, true);
                }
            }

            // Prompt #73 — Hostage Situations
            // ───────────────────────────────────────────────────────────
            HostageSystem = new HostageSystem();

            // Prompt #74 — Propaganda Broadcasting
            // ───────────────────────────────────────────────────────────
            PropagandaSystem = new PropagandaSystem();

            // Prompt #75 — Deserter/Spy System
            // ───────────────────────────────────────────────────────────
            DeserterSystem = new DeserterSystem(new System.Random(_worldSeed + 75));

            // Prompt #76 — Weather Scapegoating
            // ───────────────────────────────────────────────────────────
            ScapegoatSystem = new WeatherScapegoatSystem(new System.Random(_worldSeed + 76));

            // Prompt #77 — Slave Labor Camps
            // ───────────────────────────────────────────────────────────
            LaborCampSystem = new LaborCampSystem();
            if (GeneratedMap != null)
            {
                var rng = new System.Random(_worldSeed + 77);
                for (int i = 0; i < GeneratedMap.Nodes.Count; i++)
                {
                    var node = GeneratedMap.Nodes[i];
                    if (node == null || node.IsShelter) continue;
                    if (rng.NextDouble() < 0.1f) // 10% of nodes are labor camps
                        LaborCampSystem.SetLaborCamp(node.NodeId, true);
                }
            }

            // Prompt #78 — Cult Moral Disgust
            // ───────────────────────────────────────────────────────────
            CultMoralSystem = new CultMoralDisgustSystem();

            // Prompt #79 — Mutated Ecosystem (Flora & Fauna)
            // ───────────────────────────────────────────────────────────
            EcosystemSystem = new MutatedEcosystemSystem(new System.Random(_worldSeed + 79));

            // Prompt #79–#84 — House-to-Bunker Transition & Layout Selection
            // ───────────────────────────────────────────────────────────
            var layouts = Data.ShelterLayoutFactory.CreateAll();
            var layoutRng = new System.Random(_worldSeed);
            ShelterLayout = layouts[layoutRng.Next(layouts.Count)];
            Debug.Log($"[GameBootstrap] Selected shelter layout: {ShelterLayout.layoutName}");

            HouseToBunkerSystem = new HouseToBunkerSystem(new System.Random(_worldSeed + 79));
            HouseToBunkerSystem.InitializeFromLayout(ShelterLayout);

            // Apply layout-specific shelter modifications.
            ApplyShelterLayout(ShelterLayout);

            // Prompts #85–#94 — Location Quests
            // ───────────────────────────────────────────────────────────
            LocationQuestSystem = new LocationQuestSystem();
            InjectQuestNodesIntoMap();

        }

        private void InitShelterTacticalSystems()
        {
            // Prompts #119–#128 — Shelter tactical systems
            ExcavationSystem = new ExcavationSystem(new System.Random(_worldSeed + 119));
            FloodingSystem = new RoomFloodingSystem();
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

            // ───────────────────────────────────────────────────────────
        }

        private void InitNarrativeDependentSystems()
        {
            // Prompt #6 — Phantom Intruders System
            // ───────────────────────────────────────────────────────────
            PhantomIntruders = new PhantomIntruderSystem();
            PhantomIntruders.ConsumeAmmoHandler = amount =>
            {
                if (Inventory == null || _itemCatalog == null) return false;
                // Try common ammo types
                var ammoTypes = new[] { "ammo_9mm", "ammo_shotgun", "ammo_rifle" };
                foreach (var ammoId in ammoTypes)
                {
                    var def = _itemCatalog.GetById(ammoId);
                    if (def != null && Inventory.Remove(def, amount)) return true;
                }
                return false;
            };
            PhantomIntruders.OnWeaponFiredHandler = () =>
            {
                Debug.Log("[Phantom Intruder] Weapon fired at the hatch door!");
            };
            PhantomIntruders.OnPhantomIntruderTriggered += paranoid =>
            {
                Debug.Log($"[Phantom Intruder] {paranoid.DisplayName} sees a Hatch Breach that isn't there!");
            };
            PhantomIntruders.OnPhantomIntruderResolved += paranoid =>
            {
                Debug.Log($"[Phantom Intruder] {paranoid.DisplayName} realizes nothing was out there.");
            };

            // ───────────────────────────────────────────────────────────
            // Prompt #9 — The Child Dependent System
            // ───────────────────────────────────────────────────────────
            ChildSystem = new ChildDependentSystem();
            ChildSystem.ConsumeChildRationsHandler = (food, water) =>
            {
                if (Inventory == null || _itemCatalog == null) return;
                var foodItem = _itemCatalog.GetById("canned_food");
                if (foodItem != null) Inventory.Remove(foodItem, Mathf.CeilToInt(food / 20f));
                var waterItem = _itemCatalog.GetById("clean_water");
                if (waterItem != null) Inventory.Remove(waterItem, Mathf.CeilToInt(water / 20f));
            };
            ChildSystem.OnChildFound += child =>
            {
                if (Survivors != null)
                {
                    Survivors.Add(child);
                    NeedsSystem.Register(child);
                }
                Debug.Log("[Child] The child has been found and brought into the bunker. Hope rises.");
            };
            ChildSystem.OnChildDied += _ =>
            {
                Debug.Log("[Child] The child has died. The bunker's hope shatters.");
                if (SaveSystem != null)
                    SaveSystem.SetWorldFlag(ChildDependentSystem.ChildDiedFlag, true);
            };

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

            // Prompt #51 — Vermin Infestations
            // ───────────────────────────────────────────────────────────
            VerminSystem = new VerminSystem(new System.Random(_worldSeed + 51));
            VerminSystem.Bind(
                () => Shelter,
                () => null, // PetSystem wired after construction
                () => WasteSystem != null ? WasteSystem.Hygiene : 100f);

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

        private void InitDiaryAndHatchSystems()
        {
            // Prompt #5 — Diary Fragment Catalog (Previous Tenants)
            // ───────────────────────────────────────────────────────────
            DiaryCatalog = new List<DiaryFragmentSO>();
            // Load diary fragments from Resources or StreamingAssets
            var loadedDiaries = Resources.LoadAll<DiaryFragmentSO>("Diaries");
            if (loadedDiaries != null && loadedDiaries.Length > 0)
            {
                DiaryCatalog.AddRange(loadedDiaries);
            }
            // If no authored diaries exist, create default ones inline so the
            // rubble-clearing system has content to reveal.
            if (DiaryCatalog.Count == 0)
            {
                DiaryCatalog.Add(CreateDefaultDiary(new DiarySeed
                {
                    Id = "diary_filter_is_a_lie",
                    Title = "Torn Notebook Page",
                    Text = "The filter is a lie. I watched them install it. It doesn't purify anything — " +
                           "it just pushes the radon deeper into the vents. The reading at the intake looks " +
                           "clean because it bypasses the sensor. We've been breathing poison for three weeks. " +
                           "I don't know how to tell the others. — M.",
                    Author = "M.",
                    RoomId = "deep_vault",
                    WarnsSystem = "air_filtration",
                    Page = 0,
                    Total = 3
                }));

                DiaryCatalog.Add(CreateDefaultDiary(new DiarySeed
                {
                    Id = "diary_water_truth",
                    Title = "Water-Stained Journal",
                    Text = "The catchment on the roof is cracked. Has been since the first mortar. " +
                           "Every time it rains, we cheer — but the water tastes like metal and the " +
                           "geiger clicks faster every time we boil it. I tried to patch it last week " +
                           "but the suit tore and I couldn't stay out there. The crack is getting wider. " +
                           "— Unknown",
                    Author = "Unknown",
                    RoomId = "deep_vault",
                    WarnsSystem = "water_purifier",
                    Page = 1,
                    Total = 3
                }));

                DiaryCatalog.Add(CreateDefaultDiary(new DiarySeed
                {
                    Id = "diary_shielding_rot",
                    Title = "Last Entry of the Engineer",
                    Text = "The shielding in the deep vault was never finished. They poured half the " +
                           "concrete and ran out of aggregate. The plans say six inches. There's maybe " +
                           "two. I've been sleeping against the wrong wall for a month. The skin on " +
                           "my back is peeling and I don't think it's just dry air anymore. " +
                           "If you're reading this — check the east wall. Check it with a dosimeter, " +
                           "not the panel. The panel lies. — Engineer Kostya",
                    Author = "Engineer Kostya",
                    RoomId = "deep_vault",
                    WarnsSystem = "radiation_shielding",
                    Page = 2,
                    Total = 3
                }));
            }
            // Wire diary reveal into JournalSystem (simplified — logs via debug; full
            // JournalSystem integration can use AddEntryFactory when needed)
            var clearRubbleAction = Actions.Find(a => a is ClearRubbleActionSO) as ClearRubbleActionSO;
            if (clearRubbleAction != null)
            {
                clearRubbleAction.OnDiaryRevealed = (roomId, fragmentIndex) =>
                {
                    if (DiaryCatalog != null)
                    {
                        foreach (var diary in DiaryCatalog)
                        {
                            if (diary != null && diary.foundInRoomId == roomId && diary.pageOrder == fragmentIndex && !diary.IsFound)
                            {
                                diary.IsFound = true;
                                Debug.Log($"[Diary] Found in {roomId}: \"{diary.title}\" — {diary.text}");
                                return diary.text;
                            }
                        }
                    }
                    return null;
                };
            }

            // Hatch-dilemma prompt: tracks the active "knock at the
            // hatch" decision and provides a timeout so the survivor
            // doesn't sit in AtHatchDilemma forever. The UI flow is
            // wired in OnHatchDilemmaReady_Handle (EventRunner.Run shows
            // the modal; the prompt's Tick advances the timeout).
            HatchDilemmaPromptField = new HatchDilemmaPrompt();

            // Hatch defense (Prompt #33): security vs raids, guard duty, loot theft
            HatchDefenseSystem = new HatchDefenseSystem(
                getShelter: () => Shelter,
                getInventory: () => Inventory,
                getSurvivors: () => Survivors,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0,
                inflictTrauma: (sv, affId) => MedicalSystem?.Inflict(sv, affId),
                rng: new System.Random(_worldSeed + 33));
            // Starting hatch plate: reinforced locks at level 1
            Shelter.AddModule(new ShelterModuleInstance(
                HatchDefenseModuleSO.ReinforcedLocksId, 1)
            {
                SecurityContribution = 10f,
                FilterHealth = 100f
            });
            // Workbench lists hatch install / upgrade lines (scrap sink)
            WorkbenchSystem?.SetHatchDefense(HatchDefenseSystem);

            // Dynamic phase economy + faction trust matrix
            EconomySystem = new DynamicEconomySystem(
                getPhase: () => WorldPhaseSystem.CurrentPhase,
                shelter: Shelter,
                rng: new System.Random(_worldSeed + 91));
            foreach (var fac in DynamicEconomySystem.CreateDefaultFactions())
                EconomySystem.RegisterFaction(fac);
            EconomySystem.SetHatchDefense(HatchDefenseSystem);
            EconomySystem.SetDayProvider(() => TimeSystem != null ? TimeSystem.CurrentDay : 0);
            // Cult of the Glow (trustInversion): disposition tracks party radiation dose.
            EconomySystem.SetPartyRadiationProvider(GetPartyAverageRadiationDose);
            // #16 polish: ARS reverence + intact-hazmat contempt providers.
            EconomySystem.SetPartyHasArsProvider(PartyHasAcuteRadiationSyndrome);
            EconomySystem.SetPartyIntactHazmatProvider(PartyWearsIntactHazmat);
            EconomySystem.BindEventRunner(EventRunner);

            // Post-repel parley modal + faction radio intercept log
            ParleyOfferPromptField = new ParleyOfferPrompt();
            FactionRadioIntercepts = new FactionRadioInterceptSystem();
            FactionRadioIntercepts.Bind(
                EconomySystem,
                () => TimeSystem != null ? TimeSystem.CurrentDay : 0);
            EconomySystem.OnRaidResolved += OnFactionRaidResolved_Handle;
            FactionRadioIntercepts.OnIntercept += entry =>
            {
                if (entry == null || string.IsNullOrEmpty(entry.Message)) return;
                Debug.Log($"[Radio intercept] {entry.Message}");
                PushRadioInterceptToHud(entry);
            };

        }
    }
}
