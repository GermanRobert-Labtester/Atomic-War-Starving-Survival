using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.IO;
using Ashfall.Core.Maritime;
using Ashfall.Core.Narrative;
using Ashfall.Core.Radio;
using Ashfall.Core.World;

namespace Ashfall.Core.Tests
{
    public class ProductionGameplayApiTests
    {
        [Fact]
        public void ExpeditionCore_ProductionRegistryAndEngine_StartAndTick()
        {
            var engine = new ExpeditionSystem();
            var rng = new SeededRng(1001);

            var def = new ExpeditionDefinition
            {
                id = "loc_production_target",
                displayName = "The Production Target Hub",
                distanceTicks = 6,
                dangerLevel = 3,
                encounterChancePerTick = 0.15f,
                baseStaminaDrainPerHour = 2.5f,
                lootCategories = new List<string> { "scrap_metal", "clean_water" }
            };
            ExpeditionDefinitionRegistry.Register(def);

            Assert.Equal("loc_production_target", ExpeditionDefinitionRegistry.Get("loc_production_target")?.id);

            bool started = engine.Start(def, "survivor_sarah", 50, ExpeditionStance.Stealth);
            Assert.True(started);
            Assert.Equal(1, engine.ActiveCount);

            var active = engine.Active["survivor_sarah"];
            Assert.NotNull(active);
            Assert.Equal((int)ExpeditionPhase.Outbound, active.phase);

            // Tick hours
            engine.TickHours(2f, rng);
            Assert.True(active.stamina < 100f);

            // Push luck
            active.phase = (int)ExpeditionPhase.Looting;
            bool pushed = engine.PushLuck("survivor_sarah");
            Assert.True(pushed);
            Assert.True(active.isPushingLuck);

            // Retreat
            bool retreated = engine.Retreat("survivor_sarah");
            Assert.True(retreated);
            Assert.Equal((int)ExpeditionPhase.Inbound, active.phase);

            // Capture and restore
            var saved = engine.CaptureState();
            Assert.Single(saved);

            var newEngine = new ExpeditionSystem();
            newEngine.RestoreState(saved);
            Assert.Equal(1, newEngine.ActiveCount);
            Assert.Equal((int)ExpeditionPhase.Inbound, newEngine.Active["survivor_sarah"].phase);
        }

        [Fact]
        public void ExpeditionCore_CampEngine_FullNightCycle()
        {
            var engine = new ExpeditionSystem();
            var rng = new SeededRng(2002);

            var def = new ExpeditionDefinition
            {
                id = "loc_camp_test",
                displayName = "Camp Test Outpost",
                distanceTicks = 10,
                dangerLevel = 2
            };
            ExpeditionDefinitionRegistry.Register(def);
            engine.Start(def, "survivor_yuki", 60);

            // Enter camp
            bool campEntered = engine.EnterCamp("survivor_yuki", 1, 18f,
                temperatureC: -8f, weatherCondition: "Snow", firewood: 10f, water: 5f, food: 5f,
                hasTent: true, hasBedroll: true, shelterType: "tent", hasSentry: true);
            Assert.True(campEntered);

            var camp = engine.GetCampState("survivor_yuki");
            Assert.NotNull(camp);
            Assert.Equal(4, camp.totalNightSegments);

            // Tick segments through dawn
            bool dawn = false;
            for (int i = 0; i < 10 && !dawn; i++)
            {
                dawn = engine.CampTick("survivor_yuki", rng);
            }

            // Resolve encounter
            bool resOk = engine.ResolveCampEncounter("survivor_yuki", "peaceful", 0f);
            Assert.NotNull(camp);

            // Break camp
            bool breakOk = engine.BreakCamp("survivor_yuki", retreat: false);
            Assert.True(breakOk);
            Assert.Null(engine.GetCampState("survivor_yuki"));
        }

        [Fact]
        public void TacticalCombatCore_ProductionBeginEncounterAndActions()
        {
            var ports = CombatHostPorts.NoOp();
            var combat = new TacticalCombatSystem(null!, ports);

            var players = new List<CombatantState>
            {
                new CombatantState { Id = "p1", Name = "Sarah", SurvivorId = "survivor_sarah", IsPlayer = true, Health = 100, MaxHealth = 100, ArmorRating = 0.5f, CoverRating = 0.4f },
                new CombatantState { Id = "p2", Name = "Marcus", SurvivorId = "survivor_marcus", IsPlayer = true, Health = 90, MaxHealth = 90, ArmorRating = 0.3f, CoverRating = 0.2f }
            };
            var weapons = new List<WeaponInstanceState>
            {
                new WeaponInstanceState { InstanceId = "w1", WeaponId = "weapon_rifle", OwnerSurvivorId = "survivor_sarah", ConditionPct = 1.0f, AmmoId = "ammo_556", AmmoRemaining = 30 },
                new WeaponInstanceState { InstanceId = "w2", WeaponId = "weapon_pistol", OwnerSurvivorId = "survivor_marcus", ConditionPct = 0.9f, AmmoId = "ammo_9mm", AmmoRemaining = 20 }
            };

            bool started = combat.BeginEncounter("enc_prod_01", "exp_prod_01", "loc_denial_cut", "The Denial Cut", 1, 4242, players, weapons, enemyCount: 2, enemyHealth: 30);
            Assert.True(started);
            Assert.False(combat.State.Resolved);
            Assert.Equal(4, combat.State.Combatants.Count);

            var rng = new SeededRng(777);
            var stanceRes = combat.SetStance(TacticalStance.Advance);
            Assert.True(stanceRes.Success);

            var enemy = combat.State.Combatants.Find(c => !c.IsPlayer);
            Assert.NotNull(enemy);

            var fireRes = combat.PlayerFire(enemy.Id, rng);
            Assert.NotNull(fireRes.Message);

            var supRes = combat.PlayerSuppress(rng);
            Assert.NotNull(supRes.Message);

            var jamRes = combat.PlayerClearJam("survivor_sarah", rng);
            Assert.NotNull(jamRes.Message);

            var repRes = combat.PlayerFieldRepair("survivor_sarah", rng);
            Assert.NotNull(repRes.Message);

            var trapRes = combat.PlayerDeployTrap(rng);
            Assert.NotNull(trapRes.Message);

            var deconRes = combat.PlayerDecontaminate(rng);
            Assert.NotNull(deconRes.Message);

            var laneRes = combat.PlayerMoveLane("p1", CombatLane.Right, rng);
            Assert.NotNull(laneRes.Message);

            // Capture and restore
            var saved = combat.CaptureState();
            Assert.NotNull(saved);

            var newCombat = new TacticalCombatSystem(null!, ports);
            newCombat.RestoreState(saved);
            Assert.Equal(combat.State.EncounterId, newCombat.State.EncounterId);
            Assert.Equal(combat.State.Combatants.Count, newCombat.State.Combatants.Count);
        }

        [Fact]
        public void Maritime_SafeCracking_ProductionActions()
        {
            var safeSystem = new SafeCrackingSystem(1234);
            var safeDef = new SafeDefinition
            {
                id = "safe_prod_test",
                displayName = "Production Test Safe",
                roomId = "room_vault",
                difficulty = 3,
                maxAttempts = 5,
                noisePerAttempt = 0.1f,
                alarmThreshold = 0.9f,
                loot = new List<SafeLootEntry>
                {
                    new SafeLootEntry { itemId = "scrap_metal", minQuantity = 3, maxQuantity = 5 }
                }
            };

            bool registered = safeSystem.RegisterSafe(safeDef, "loc_vault");
            Assert.True(registered);

            var safe = safeSystem.InspectSafe("safe_prod_test");
            Assert.NotNull(safe);
            Assert.Equal(3, safe.difficulty);
            Assert.False(safe.isOpened);

            // Attempt safe cracking
            var rng = new SeededRng(42);
            var feedback = safeSystem.Attempt("safe_prod_test", new int[] { 0, 0, 0 }, 1.0f, rng);
            Assert.NotNull(feedback.Message);
            Assert.True(feedback.NoiseLevel > 0f);

            // Accessible attempt
            var accFeedback = safeSystem.AttemptAccessible("safe_prod_test", confidence: 0.8f, toolCondition: 1.0f, skillLevel: 0.9f, rng: rng);
            Assert.NotNull(accFeedback.Message);

            // Round trip state
            var state = safeSystem.CaptureState();
            Assert.NotNull(state);

            var newSafeSystem = new SafeCrackingSystem(1234);
            newSafeSystem.RestoreState(state);
            var reloadedSafe = newSafeSystem.InspectSafe("safe_prod_test");
            Assert.NotNull(reloadedSafe);
            Assert.Equal(safe.attemptsUsed, reloadedSafe.attemptsUsed);
        }

        [Fact]
        public void Maritime_DiveAndScavenge_ProductionActions()
        {
            var diveSystem = new MaritimeDiveSystem(new SeededRng(42));
            diveSystem.StartDive("diver_01", "op_01", 120f, "site_exp09_ss_sovereign");

            Assert.True(diveSystem.IsActive);
            Assert.Equal(120f, diveSystem.AirSupplySeconds);

            // Advance room and crank
            diveSystem.AdvanceToNextRoom(15);
            Assert.Equal(1, diveSystem.CurrentRoomIndex);
            Assert.Equal(15, diveSystem.NoiseLevel);

            diveSystem.Tick(30f);
            Assert.True(diveSystem.AirSupplySeconds < 120f);

            diveSystem.CrankCompressor();
            Assert.True(diveSystem.AirSupplySeconds > 60f);

            // Controlled abort
            diveSystem.AbortDive(emergency: false);
            Assert.False(diveSystem.IsActive);

            // Round trip state
            var state = diveSystem.CaptureState();
            var newDive = new MaritimeDiveSystem(new SeededRng(42));
            newDive.RestoreState(state);
            Assert.False(newDive.IsActive);
            Assert.Equal(1, newDive.CurrentRoomIndex);
        }

        [Fact]
        public void Weather_Sonde_ProductionActions()
        {
            var weather = new WeatherSystem();
            var sondeSystem = new WeatherSondeSystem(weather);

            bool launched = sondeSystem.Launch("sonde_day_1", 1, 12f, hydrogenAvailable: 1f, batteryAvailable: 1f);
            Assert.True(launched);
            Assert.True(sondeSystem.IsLaunched);

            var rng = new SeededRng(101);
            for (int i = 0; i < 5; i++)
            {
                sondeSystem.Tick(rng);
            }

            Assert.True(sondeSystem.State.ticksElapsed > 0);
            Assert.NotEmpty(sondeSystem.State.samples);

            // Round trip state
            var state = sondeSystem.CaptureState();
            var newSonde = new WeatherSondeSystem(weather);
            newSonde.RestoreState(state);
            Assert.True(newSonde.IsLaunched);
            Assert.Equal(sondeSystem.State.ticksElapsed, newSonde.State.ticksElapsed);
        }

        [Fact]
        public void Radio_Triangulation_ProductionActions()
        {
            var triangulation = new SignalTriangulationSystem();
            var rng = new SeededRng(555);

            // Record observations
            triangulation.RecordObservation(new RadioObservation
            {
                signalId = "sig_distress_beacon",
                stationId = "station_alpha",
                day = 1,
                bearingDegrees = 45f,
                signalStrength = 0.8f,
                noiseLevel = 0.1f
            });
            triangulation.RecordObservation(new RadioObservation
            {
                signalId = "sig_distress_beacon",
                stationId = "station_beta",
                day = 1,
                bearingDegrees = 135f,
                signalStrength = 0.7f,
                noiseLevel = 0.15f
            });
            triangulation.RecordObservation(new RadioObservation
            {
                signalId = "sig_distress_beacon",
                stationId = "station_gamma",
                day = 1,
                bearingDegrees = 90f,
                signalStrength = 0.9f,
                noiseLevel = 0.05f
            });

            Assert.Equal(3, triangulation.GetObservationCount("sig_distress_beacon"));

            var candidate = triangulation.Triangulate("sig_distress_beacon", rng);
            Assert.NotNull(candidate);
            Assert.NotEmpty(candidate.locationId);

            // Round trip state
            var state = triangulation.CaptureState();
            var newTri = new SignalTriangulationSystem();
            newTri.RestoreState(state);
            Assert.Equal(3, newTri.GetObservationCount("sig_distress_beacon"));
        }

        [Fact]
        public void Foundry_SaltMine_ProductionActions()
        {
            var mine = new SaltMineExtractionSystem();
            var vein = new SaltMineVeinState
            {
                veinId = "vein_salt_main",
                displayName = "Deep Salt Core",
                isUnlocked = false,
                remainingOre = 4000f,
                extractionRate = 12f,
                maxWorkers = 4,
                assignedWorkers = 0,
                drillCondition = 1.0f,
                pumpPressure = 1.0f
            };

            mine.RegisterVein(vein);
            mine.UnlockVein("vein_salt_main");
            mine.AssignWorkers("vein_salt_main", 3);

            Assert.True(mine.GetVein("vein_salt_main")?.isUnlocked == true);

            // Tick daily extraction
            mine.TickDaily(1, new SeededRng(100));
            Assert.True(mine.State.saltStorage > 0f || mine.State.brineStorage > 0f);

            // Round trip state
            var state = mine.CaptureState();
            var newMine = new SaltMineExtractionSystem();
            newMine.RestoreState(state);
            Assert.True(newMine.GetVein("vein_salt_main")?.isUnlocked == true);
            Assert.Equal(mine.State.saltStorage, newMine.State.saltStorage);
        }
    }
}
