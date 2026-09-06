// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Audio;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Factions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Needs;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public sealed class Plans50_53_SharedIntegrationTests
    {
        private static string LocateDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Could not locate Assets/StreamingAssets/Data from test run");
        }

        private static (VehicleGarageCatalog GarageCat, FactionIntelligenceCatalog EspionageCat, PsychologicalTraumaCatalog MentalCat, ShelterAudioCueCatalog AudioCat) LoadCatalogs()
        {
            string dataDir = LocateDataDir();
            var serializer = new SystemTextJsonSerializer();

            string garageJson = File.ReadAllText(Path.Combine(dataDir, "vehicle_modifications.json"));
            var garageCat = VehicleGarageCatalogLoader.Load(garageJson, serializer);

            string espionageJson = File.ReadAllText(Path.Combine(dataDir, "faction_intelligence.json"));
            var espionageCat = FactionIntelligenceCatalogLoader.Load(espionageJson, serializer);

            string mentalJson = File.ReadAllText(Path.Combine(dataDir, "psychological_trauma.json"));
            var mentalCat = PsychologicalTraumaCatalogLoader.Load(mentalJson, serializer);

            string audioJson = File.ReadAllText(Path.Combine(dataDir, "shelter_audio_cues.json"));
            var audioCat = ShelterAudioCueCatalogLoader.Load(audioJson, serializer);

            return (garageCat, espionageCat, mentalCat, audioCat);
        }

        [Fact]
        public void ThirtyDay_FlagshipIntegrationScenario_SimulatesAllFourPillars()
        {
            var (garageCat, espionageCat, mentalCat, audioCat) = LoadCatalogs();
            var rng = new SeededRng(5053);
            var inv = new Inventory.Inventory();

            var garageSys = new VehicleGarageSystem(garageCat, rng);
            var espionageSys = new ShelterEspionageSystem(espionageCat, rng);
            var mentalSys = new SurvivorMentalHealthSystem(mentalCat, rng);
            var acousticDir = new ShelterAcousticDirector(audioCat, rng);

            // Stockpile workshop materials
            inv.AddById("mechanical_parts", 50);
            inv.AddById("scrap_metal", 100);

            // ── Phase 1: Days 1–10 (Initial Configuration & Sorties) ──
            const string testVehicleId = "veh_recon_buggy";
            bool installFlatbed = garageSys.InstallModification(testVehicleId, VehicleGarageSystem.SlotCargo, "vmod_expanded_flatbed", inv, out string fail1);
            Assert.True(installFlatbed, $"Failed to install flatbed: {fail1}");

            bool installBullbar = garageSys.InstallModification(testVehicleId, VehicleGarageSystem.SlotProtection, "vmod_reinforced_bullbar", inv, out string fail2);
            Assert.True(installBullbar, $"Failed to install bullbar: {fail2}");

            float cargoDelta = garageSys.GetEffectiveCargoCapacityDelta(testVehicleId);
            Assert.True(cargoDelta > 0);

            // Enroll sleeper agent and seed a clandestine dead drop
            const string suspectSurvivor = "surv_infiltrator_kane";
            bool enrolled = espionageSys.EnrollSleeperAgent(suspectSurvivor, "iron_garrison", initialLoyalty: 450);
            Assert.True(enrolled);

            var spawnedDrop = espionageSys.SpawnRandomDeadDrop();
            Assert.NotNull(spawnedDrop);
            string dropId = spawnedDrop.dropId;

            // Intercept dead drop before expiration
            bool intercepted = espionageSys.InterceptDeadDrop(dropId, out int points, out _);
            Assert.True(intercepted);
            Assert.True(points > 0);

            // Survivor suffers acute trauma during surface encounter
            const string traumatizedSurvivor = "surv_recon_val";
            bool inflicted = mentalSys.InflictTrauma(traumatizedSurvivor, "trauma_combat_shock", out _);
            Assert.True(inflicted);
            mentalSys.AddStress(traumatizedSurvivor, 400, "combat");

            var mentalRec = mentalSys.GetOrCreateRecord(traumatizedSurvivor);
            Assert.Contains("trauma_combat_shock", mentalRec.activeTraumaIds);
            Assert.True(mentalRec.stressPermille >= 600);

            // Simulate Days 1–10
            for (int day = 1; day <= 10; day++)
            {
                // Run overland vehicle sorties causing wear
                garageSys.RecordTripWear(testVehicleId, distanceKm: 25f, roadRoughnessMultiplier: 1.5f);

                // Daily espionage operations
                espionageSys.TickDay(day, inv);

                // Daily mental health progression
                mentalSys.TickDay(day);

                // Acoustic snapshot evaluation
                var facts = new AcousticSimulationFacts
                {
                    activeZoneOrRoom = "room_generator",
                    generatorWattage = 3500,
                    generatorMaxWattage = 5000,
                    ventilationLoadPermille = 450,
                    ambientRadiationMillisieverts = 1.2f,
                    structuralStressPermille = 200
                };
                acousticDir.UpdateSimulationFacts(facts);
                var snap = acousticDir.EvaluateSnapshot();
                Assert.NotNull(snap);
                Assert.True(snap.continuousLayerIntensities["generator"] > 500);
            }

            var recAfter10 = garageSys.GetOrCreateRecord(testVehicleId);
            Assert.True(recAfter10.chassisStressPermille > 0);
            Assert.True(recAfter10.engineFoulingPermille > 0);

            // ── Phase 2: Days 11–20 (Mitigation, Counter-Intel & Quiet Decompression) ──
            // Service vehicle chassis, engine, and transmission to remove wear
            bool servicedChassis = garageSys.ServiceChassis(testVehicleId, inv, repairPermille: 1000, out string serviceFail);
            Assert.True(servicedChassis, $"Failed to service chassis: {serviceFail}");
            bool servicedEngine = garageSys.ServiceEngine(testVehicleId, inv, repairPermille: 1000, out string engineFail);
            Assert.True(servicedEngine, $"Failed to service engine: {engineFail}");
            bool servicedTrans = garageSys.ServiceTransmission(testVehicleId, inv, repairPermille: 1000, out string transFail);
            Assert.True(servicedTrans, $"Failed to service transmission: {transFail}");

            var recServiced = garageSys.GetOrCreateRecord(testVehicleId);
            Assert.Equal(0, recServiced.chassisStressPermille);
            Assert.Equal(0, recServiced.engineFoulingPermille);
            Assert.Equal(0, recServiced.transmissionWearPermille);

            // Counter-intelligence investigation
            espionageSys.AddSuspicion(suspectSurvivor, 900);
            bool investigated = espionageSys.InvestigateSuspect(suspectSurvivor, investigatorSkill: 100, out bool unmasked, out _);
            Assert.True(investigated);
            Assert.True(unmasked);

            // Turn unmasked sleeper into a double agent
            bool turned = espionageSys.AttemptTurnDoubleAgent(suspectSurvivor, inv, out _);
            Assert.True(turned);

            // Prescribe quiet room therapy to traumatized survivor
            int stressBeforeDecompression = mentalSys.GetOrCreateRecord(traumatizedSurvivor).stressPermille;
            bool prescribed = mentalSys.PrescribeTherapy(traumatizedSurvivor, "therapy_quiet_room", hasCounselor: true, out _);
            Assert.True(prescribed);

            // Simulate Days 11–20
            for (int day = 11; day <= 20; day++)
            {
                garageSys.RecordTripWear(testVehicleId, distanceKm: 15f, roadRoughnessMultiplier: 1.0f);
                espionageSys.TickDay(day, inv);
                mentalSys.TickDay(day);

                // Check acoustic director in deep excavation zone with high structural stress
                var deepFacts = new AcousticSimulationFacts
                {
                    activeZoneOrRoom = "zone_excavation_deep",
                    structuralStressPermille = 750,
                    ambientRadiationMillisieverts = 4.5f
                };
                acousticDir.UpdateSimulationFacts(deepFacts);
                var deepSnap = acousticDir.EvaluateSnapshot();
                Assert.Equal("acue_prof_deep_excavation", deepSnap.activeMixProfileId);
            }

            int stressAfterDecompression = mentalSys.GetOrCreateRecord(traumatizedSurvivor).stressPermille;
            Assert.True(stressAfterDecompression < stressBeforeDecompression);

            // ── Phase 3: Days 21–30 (Endgame Campaign Stabilization) ──
            for (int day = 21; day <= 30; day++)
            {
                garageSys.RecordTripWear(testVehicleId, distanceKm: 10f, roadRoughnessMultiplier: 1.0f);
                espionageSys.TickDay(day, inv);
                mentalSys.TickDay(day);
            }

            // Final assertions across all pillars
            Assert.True(garageSys.HasVehicleRecord(testVehicleId));
            Assert.False(garageSys.GetOrCreateRecord(testVehicleId).isImmobilized);
            Assert.True(espionageSys.CaptureState().totalIntelPoints > 0);
            Assert.True(mentalSys.HasRecord(traumatizedSurvivor));
        }

        [Fact]
        public void SaveAndLoad_RoundTrip_PreservesAllFourSystemsState()
        {
            var (garageCat, espionageCat, mentalCat, audioCat) = LoadCatalogs();
            var rng = new SeededRng(777);
            var inv = new Inventory.Inventory();
            inv.AddById("mechanical_parts", 20);
            inv.AddById("scrap_metal", 50);

            var garage = new VehicleGarageSystem(garageCat, rng);
            var espionage = new ShelterEspionageSystem(espionageCat, rng);
            var mental = new SurvivorMentalHealthSystem(mentalCat, rng);

            // Setup state
            garage.InstallModification("veh_crawler_01", VehicleGarageSystem.SlotCargo, "vmod_expanded_flatbed", inv, out _);
            garage.RecordTripWear("veh_crawler_01", distanceKm: 80f, roadRoughnessMultiplier: 2.0f);

            espionage.EnrollSleeperAgent("surv_agent_x", "iron_garrison", initialLoyalty: 550);
            var drop = espionage.SpawnRandomDeadDrop();
            Assert.NotNull(drop);
            espionage.AddSuspicion("surv_agent_x", 200);

            mental.InflictTrauma("surv_shellshocked", "trauma_combat_shock", out _);
            mental.AddStress("surv_shellshocked", 500, "combat");
            mental.PrescribeTherapy("surv_shellshocked", "therapy_quiet_room", hasCounselor: false, out _);

            // Capture state
            var garageState = garage.CaptureState();
            var espionageState = espionage.CaptureState();
            var mentalState = mental.CaptureState();

            // Round-trip through JSON serializer
            var serializer = new SystemTextJsonSerializer();
            string garageJson = serializer.Serialize(garageState);
            string espionageJson = serializer.Serialize(espionageState);
            string mentalJson = serializer.Serialize(mentalState);

            var restoredGarageState = serializer.Deserialize<VehicleGarageState>(garageJson);
            var restoredEspionageState = serializer.Deserialize<ShelterEspionageState>(espionageJson);
            var restoredMentalState = serializer.Deserialize<SurvivorMentalHealthState>(mentalJson);

            Assert.NotNull(restoredGarageState);
            Assert.NotNull(restoredEspionageState);
            Assert.NotNull(restoredMentalState);

            // Reinstantiate systems
            var restoredGarage = new VehicleGarageSystem(garageCat, rng);
            restoredGarage.RestoreState(restoredGarageState);

            var restoredEspionage = new ShelterEspionageSystem(espionageCat, rng);
            restoredEspionage.RestoreState(restoredEspionageState);

            var restoredMental = new SurvivorMentalHealthSystem(mentalCat, rng);
            restoredMental.RestoreState(restoredMentalState);

            // Assert exact restoration
            var origRec = garage.GetOrCreateRecord("veh_crawler_01");
            var restoredRec = restoredGarage.GetOrCreateRecord("veh_crawler_01");
            Assert.Equal(origRec.chassisStressPermille, restoredRec.chassisStressPermille);
            Assert.Equal(origRec.engineFoulingPermille, restoredRec.engineFoulingPermille);
            Assert.Equal(origRec.installedSlots[VehicleGarageSystem.SlotCargo], restoredRec.installedSlots[VehicleGarageSystem.SlotCargo]);

            var origAgent = espionage.GetSleeperRecord("surv_agent_x");
            var restoredAgent = restoredEspionage.GetSleeperRecord("surv_agent_x");
            Assert.NotNull(restoredAgent);
            Assert.Equal(origAgent!.loyaltyPermille, restoredAgent.loyaltyPermille);
            Assert.Equal(origAgent.suspicionPermille, restoredAgent.suspicionPermille);

            var origMental = mental.GetOrCreateRecord("surv_shellshocked");
            var restoredMentalRec = restoredMental.GetOrCreateRecord("surv_shellshocked");
            Assert.Equal(origMental.stressPermille, restoredMentalRec.stressPermille);
            Assert.Equal(origMental.therapySessionCount, restoredMentalRec.therapySessionCount);
            Assert.Equal(origMental.activeTraumaIds.Count, restoredMentalRec.activeTraumaIds.Count);
        }

        [Fact]
        public void Determinism_Replay_IdenticalSeedsProduceIdenticalSimulation()
        {
            var (garageCat, espionageCat, mentalCat, audioCat) = LoadCatalogs();

            VehicleGarageState RunSim(int seed)
            {
                var rng = new SeededRng(seed);
                var inv = new Inventory.Inventory();
                inv.AddById("mechanical_parts", 50);
                inv.AddById("scrap_metal", 100);

                var garage = new VehicleGarageSystem(garageCat, rng);
                var espionage = new ShelterEspionageSystem(espionageCat, rng);
                var mental = new SurvivorMentalHealthSystem(mentalCat, rng);

                garage.InstallModification("veh_replay", VehicleGarageSystem.SlotCargo, "vmod_expanded_flatbed", inv, out _);
                espionage.EnrollSleeperAgent("surv_replay", "iron_garrison", initialLoyalty: 500);
                mental.InflictTrauma("surv_replay", "trauma_combat_shock", out _);

                for (int day = 1; day <= 20; day++)
                {
                    garage.RecordTripWear("veh_replay", distanceKm: 20f, roadRoughnessMultiplier: 1.2f);
                    espionage.TickDay(day, inv);
                    mental.TickDay(day);
                }

                return garage.CaptureState();
            }

            var run1 = RunSim(12345);
            var run2 = RunSim(12345);
            var run3 = RunSim(99999);

            var rec1 = run1.vehicleRecords["veh_replay"];
            var rec2 = run2.vehicleRecords["veh_replay"];
            var rec3 = run3.vehicleRecords["veh_replay"];

            // Same seed must produce bitwise identical wear and status
            Assert.Equal(rec1.chassisStressPermille, rec2.chassisStressPermille);
            Assert.Equal(rec1.engineFoulingPermille, rec2.engineFoulingPermille);
            Assert.Equal(rec1.transmissionWearPermille, rec2.transmissionWearPermille);

            // Non-zero wear accrued
            Assert.True(rec1.chassisStressPermille > 0);
        }
    }
}
