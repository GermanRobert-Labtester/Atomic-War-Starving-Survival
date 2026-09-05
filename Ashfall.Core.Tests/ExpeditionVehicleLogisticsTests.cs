using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Crafting;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Task #101 — expedition vehicle logistics + weapon-condition bridging.
    /// Pins: vehicle speed/capacity change expedition outcomes, deterministic
    /// mid-route breakdowns revert to foot, the estimate math mirrors the tick
    /// loop, and the bridge converts units without owning durability state.
    /// </summary>
    public class ExpeditionVehicleLogisticsTests
    {
        private static ExpeditionDefinition Def(int ticks = 8, float encounter = 0.12f) => new ExpeditionDefinition
        {
            id = "loc_test_range",
            displayName = "Test Range",
            distanceTicks = ticks,
            dangerLevel = 1,
            encounterChancePerTick = encounter,
        };

        private static ExpeditionVehicleProfile Vehicle(float speed = 1.6f, float cargo = 120f, float breakdown = 0f, float fuelPerTick = 0.5f) => new ExpeditionVehicleProfile
        {
            vehicleId = "vehicle_cargo_truck",
            speedMultiplier = speed,
            cargoCapacityKg = cargo,
            breakdownChancePerTick = breakdown,
            fuelPerTravelTick = fuelPerTick,
        };

        /// <summary>Tick until the expedition leaves the given phase (bounded).</summary>
        private static int TickUntilPhase(ExpeditionSystem sys, string survivor, ExpeditionPhase from, ExpeditionPhase to, int maxTicks = 64)
        {
            var rng = new SeededRng(1234);
            int ticks = 0;
            while (ticks < maxTicks && sys.Active[survivor].phase == (int)from)
            {
                sys.TickHours(1f, rng);
                ticks++;
            }
            Assert.Equal((int)to, sys.Active[survivor].phase);
            return ticks;
        }

        [Fact]
        public void VehicleSpeed_ShortensOutboundTravel()
        {
            // Foot: 8 ticks at 1.0/tick. Vehicle 2.0x: 4 ticks at 2.0/tick.
            var foot = new ExpeditionSystem();
            foot.Start(Def(), "s_foot", 1);
            int footTicks = TickUntilPhase(foot, "s_foot", ExpeditionPhase.Outbound, ExpeditionPhase.Looting);

            var driven = new ExpeditionSystem();
            driven.Start(Def(), "s_drv", 1, vehicle: Vehicle(speed: 2.0f));
            Assert.Equal(120f, driven.Active["s_drv"].maxLootCapacityKg);
            int drivenTicks = TickUntilPhase(driven, "s_drv", ExpeditionPhase.Outbound, ExpeditionPhase.Looting);

            Assert.Equal(8, footTicks);
            Assert.Equal(4, drivenTicks);
        }

        [Fact]
        public void VehicleBreakdown_IsDeterministic_AndRevertsToFoot()
        {
            // Breakdown guaranteed on the first travel tick; the vehicle's 2x
            // speed then stops applying for the remainder of the sortie.
            var sysA = new ExpeditionSystem();
            var sysB = new ExpeditionSystem();
            foreach (var sys in new[] { sysA, sysB })
                sys.Start(Def(10), "s", 1, vehicle: Vehicle(speed: 2.0f, breakdown: 1f));

            var rngA = new SeededRng(77);
            var rngB = new SeededRng(77);
            sysA.TickHours(1f, rngA);
            sysB.TickHours(1f, rngB);

            Assert.True(sysA.Active["s"].vehicleBrokenDown);
            Assert.True(sysB.Active["s"].vehicleBrokenDown);
            Assert.Contains("on foot", sysA.Active["s"].outcomeText, StringComparison.Ordinal);
            // Capacity reverts to the foot cap after the breakdown.
            Assert.Equal(40f, sysA.Active["s"].maxLootCapacityKg);

            // Same remaining rollout for both engines: determinism after the flip.
            for (int i = 0; i < 12; i++)
            {
                sysA.TickHours(1f, rngA);
                sysB.TickHours(1f, rngB);
            }
            Assert.Equal(sysB.Active["s"].travelTicksCompleted, sysA.Active["s"].travelTicksCompleted);
            Assert.Equal(sysB.Active["s"].phase, sysA.Active["s"].phase);
        }

        [Fact]
        public void VehicleBreakdown_FiresEventOnce()
        {
            var sys = new ExpeditionSystem();
            sys.Start(Def(10), "s", 1, vehicle: Vehicle(speed: 2f, breakdown: 1f));
            int fired = 0;
            sys.OnVehicleBreakdown += _ => fired++;
            var rng = new SeededRng(5);
            sys.TickHours(1f, rng);
            sys.TickHours(1f, rng);
            sys.TickHours(1f, rng);
            Assert.Equal(1, fired);
        }

        [Fact]
        public void NeutralVehicle_NeverConsumesBreakdownRolls()
        {
            // A vehicle with speed 1.0, no capacity override and zero
            // breakdown chance must produce the EXACT legacy foot trajectory
            // (and therefore the identical RNG stream): the breakdown roll is
            // guarded on chance > 0, so foot determinism is untouched.
            var foot = new ExpeditionSystem();
            var neutral = new ExpeditionSystem();
            foot.Start(Def(4), "s", 1);
            neutral.Start(Def(4), "s", 1,
                vehicle: Vehicle(speed: 1f, cargo: 0f, breakdown: 0f, fuelPerTick: 0f));

            var rngFoot = new SeededRng(99);
            var rngNeutral = new SeededRng(99);
            for (int i = 0; i < 10; i++)
            {
                foot.TickHours(1f, rngFoot);
                neutral.TickHours(1f, rngNeutral);
            }
            var a = foot.Active["s"];
            var b = neutral.Active["s"];
            Assert.Equal(a.phase, b.phase);
            Assert.Equal(a.travelTicksCompleted, b.travelTicksCompleted);
            Assert.Equal(a.lootingTicksCompleted, b.lootingTicksCompleted);
            Assert.Equal(a.currentWeightKg, b.currentWeightKg, 5);
            // And the streams stayed in lockstep.
            Assert.Equal(rngFoot.NextDouble(), rngNeutral.NextDouble(), 10);
        }

        [Fact]
        public void Estimate_MirrorsTickMath_ForFootAndVehicle()
        {
            var foot = ExpeditionSystem.Estimate(Def(8), ExpeditionStance.Speed);
            Assert.False(foot.usingVehicle);
            Assert.Equal(6, foot.outboundTicks);            // ceil(8/1.5)
            Assert.Equal(6, foot.inboundTicks);
            Assert.Equal(0f, foot.fuelRequired);
            Assert.Equal(40f, foot.cargoCapacityKg);
            Assert.Equal(0f, foot.breakdownRiskTotal);

            var driven = ExpeditionSystem.Estimate(
                Def(8), ExpeditionStance.Stealth, vehicle: Vehicle(speed: 2f, cargo: 120f, breakdown: 0.25f, fuelPerTick: 0.5f));
            Assert.True(driven.usingVehicle);
            Assert.Equal(4, driven.outboundTicks);          // ceil(8/2.0)
            Assert.Equal(4, driven.inboundTicks);
            Assert.Equal(4f, driven.fuelRequired);          // 0.5 * 8 travel ticks
            Assert.Equal(120f, driven.cargoCapacityKg);
            Assert.Equal(0.25f, driven.breakdownRiskPerTick);
            Assert.Equal(1f - (float)Math.Pow(0.75, 8), driven.breakdownRiskTotal, 5);

            // Stealth halves encounter risk; poor weapon readiness raises it.
            // (This foot estimate is Speed stance: no stealth discount.)
            Assert.Equal(0.12f, foot.encounterRiskPerTick, 5);
            var stealthFoot = ExpeditionSystem.Estimate(Def(8), ExpeditionStance.Stealth);
            Assert.Equal(0.06f, stealthFoot.encounterRiskPerTick, 5);
            var degraded = ExpeditionSystem.Estimate(Def(8), ExpeditionStance.Stealth, weaponReadiness: 0f);
            Assert.Equal(0.06f * 1.5f, degraded.encounterRiskPerTick, 5);
        }

        // ── Weapon/equipment bridge ─────────────────────────────────────

        private static EquipmentConditionSystem Equipment(out Inventory.Inventory inv)
        {
            inv = new Inventory.Inventory();
            return new EquipmentConditionSystem(new SeededRng(42), inv, new CraftingSystem(inv));
        }

        [Fact]
        public void Bridge_ProjectsAuthorityConditionIntoCombatToken()
        {
            var eq = Equipment(out _);
            eq.RegisterItem("eq_w1", "weapon_bolt_rifle", "survivor_a", EquipmentFamily.Weapon);
            eq.UseItem("eq_w1", 30f); // 70/100

            var token = WeaponEquipmentBridge.ToCombatInstance(eq, "weapon_bolt_rifle", "survivor_a");
            Assert.Equal("eq_w1", token.InstanceId);
            Assert.Equal(0.7f, token.ConditionPct, 3);

            // No tracked instance → pristine, unbound token (combat fallback path).
            var unbound = WeaponEquipmentBridge.ToCombatInstance(eq, "weapon_pipe_rifle", "survivor_a");
            Assert.Equal(string.Empty, unbound.InstanceId);
            Assert.Equal(1f, unbound.ConditionPct);
        }

        [Fact]
        public void Bridge_PrefersOwnerAndBestCondition()
        {
            var eq = Equipment(out _);
            eq.RegisterItem("eq_a", "weapon_bolt_rifle", "survivor_a", EquipmentFamily.Weapon);
            eq.RegisterItem("eq_b", "weapon_bolt_rifle", "survivor_a", EquipmentFamily.Weapon);
            eq.RegisterItem("eq_c", "weapon_bolt_rifle", "survivor_b", EquipmentFamily.Weapon);
            eq.UseItem("eq_b", 10f); // eq_a (100) beats eq_b (90); eq_c is another owner's

            Assert.Equal("eq_a", WeaponEquipmentBridge.FindWeaponFor(eq, "weapon_bolt_rifle", "survivor_a")!.instanceId);
            Assert.Equal("eq_c", WeaponEquipmentBridge.FindWeaponFor(eq, "weapon_bolt_rifle", "survivor_b")!.instanceId);
        }

        [Fact]
        public void Bridge_WriteBack_ConvertsUnits_ThroughAuthority()
        {
            var eq = Equipment(out _);
            eq.RegisterItem("eq_w1", "weapon_bolt_rifle", "survivor_a", EquipmentFamily.Weapon);

            WeaponEquipmentBridge.ApplyWear(eq, "eq_w1", 0.05f);
            Assert.Equal(95f, eq.State.items[0].condition);

            // SyncAfterCombat only writes the delta, and only for bound tokens.
            var token = WeaponEquipmentBridge.ToCombatInstance(eq, "weapon_bolt_rifle", "survivor_a");
            float before = token.ConditionPct;
            token.ConditionPct = 0.60f;
            WeaponEquipmentBridge.SyncAfterCombat(eq, token, before);
            Assert.Equal(60f, eq.State.items[0].condition, 3);

            float untouched = eq.State.items[0].condition;
            var unbound = new WeaponInstanceState { InstanceId = "", WeaponId = "x", ConditionPct = 0.1f };
            WeaponEquipmentBridge.SyncAfterCombat(eq, unbound, 1f);
            Assert.Equal(untouched, eq.State.items[0].condition);
        }

        [Fact]
        public void Bridge_Readiness_FollowsAuthorityRisks()
        {
            var eq = Equipment(out _);
            eq.RegisterItem("eq_w1", "weapon_bolt_rifle", "survivor_a", EquipmentFamily.Weapon);

            Assert.Equal(1f, WeaponEquipmentBridge.Readiness(eq, "eq_w1"));   // pristine
            Assert.Equal(1f, WeaponEquipmentBridge.Readiness(null, "eq_w1")); // no authority
            Assert.Equal(1f, WeaponEquipmentBridge.Readiness(eq, null));      // no selection

            eq.UseItem("eq_w1", 85f); // condition 15 → jam (20-15)/20, slip (30-15)/30
            float readiness = WeaponEquipmentBridge.Readiness(eq, "eq_w1");
            Assert.Equal(1f - (0.25f + 0.5f) / 2f, readiness, 3);
            Assert.Equal(0.25f, WeaponEquipmentBridge.JamRisk(eq, "eq_w1"), 3);

            eq.UseItem("eq_w1", 15f); // condition 0 → unusable
            Assert.Equal(0f, WeaponEquipmentBridge.Readiness(eq, "eq_w1"));
        }

        // ── Aggregate persistence (expeditions + vehicle garage) ────────

        private static ExpeditionAggregateState SampleAggregate() => new ExpeditionAggregateState
        {
            expeditions = new List<ExpeditionState>
            {
                new ExpeditionState
                {
                    expeditionId = "survivor_a:loc_x",
                    survivorId = "survivor_a",
                    locationId = "loc_x",
                    vehicleId = "vehicle_utility_quad",
                    vehicleSpeedMultiplier = 1.3f,
                    vehicleBreakdownChancePerTick = 0.1f,
                },
            },
            vehicles = new ExpeditionVehicleState
            {
                ownedVehicles = new Dictionary<string, VehicleInstance>
                {
                    { "vehicle_utility_quad", new VehicleInstance
                        { vehicleId = "vehicle_utility_quad", condition = 80f, fuel = 12f, maxFuel = 40f } },
                },
                activeExpeditionVehicleId = "vehicle_utility_quad",
            },
        };

        [Fact]
        public void AggregateCodec_RoundTripsExpeditionsAndGarage()
        {
            var json = new SystemTextJsonSerializer();
            var aggregate = SampleAggregate();
            aggregate.completedCount = 7;
            aggregate.knownLocationIds = new List<string> { "loc_x", "loc_y" };

            string encoded = ExpeditionAggregateCodec.Encode(aggregate, json);
            ExpeditionAggregateState? decoded = ExpeditionAggregateCodec.Decode(encoded, json);

            Assert.NotNull(decoded);
            Assert.Single(decoded!.expeditions);
            Assert.Equal("vehicle_utility_quad", decoded.expeditions[0].vehicleId);
            Assert.NotNull(decoded.vehicles);
            Assert.True(decoded.vehicles.ownedVehicles.ContainsKey("vehicle_utility_quad"));
            Assert.Equal(80f, decoded.vehicles.ownedVehicles["vehicle_utility_quad"].condition);
            Assert.Equal(7, decoded.completedCount);
            Assert.Equal(2, decoded.knownLocationIds!.Count);
            Assert.True(encoded.Contains("Checksum", StringComparison.Ordinal), "payload keeps the checksummed envelope");
        }

        [Fact]
        public void RestoreCompletedCount_SurvivesAggregateRestore()
        {
            var sys = new ExpeditionSystem();
            sys.RestoreCompletedCount(4);
            Assert.Equal(4, sys.CompletedCount);

            var aggregate = new ExpeditionAggregateState
            {
                expeditions = sys.CaptureState(),
                vehicles = new ExpeditionVehicleState(),
                knownLocationIds = new List<string> { "loc_known" },
                completedCount = sys.CompletedCount,
            };

            var restored = new ExpeditionSystem();
            restored.RestoreState(aggregate.expeditions);
            restored.RestoreCompletedCount(aggregate.completedCount);
            restored.RestoreKnownLocations(aggregate.knownLocationIds);
            Assert.Equal(4, restored.CompletedCount);
            Assert.True(restored.IsLocationKnown("loc_known"));
        }

        [Fact]
        public void AggregateCodec_MigratesLegacyEnvelopeAndBareList()
        {
            var json = new SystemTextJsonSerializer();
            var legacyList = new List<ExpeditionState>
            {
                new ExpeditionState { expeditionId = "s1:loc_y", survivorId = "s1", locationId = "loc_y" },
            };

            // Legacy shape 1: the pre-aggregate { State, Checksum } envelope.
            var legacyEnvelope = new SaveEnvelope<List<ExpeditionState>> { State = legacyList };
            legacyEnvelope.Checksum = SaveChecksum.Compute(legacyEnvelope);
            string envelopeJson = json.Serialize(legacyEnvelope);
            ExpeditionAggregateState? fromEnvelope = ExpeditionAggregateCodec.Decode(envelopeJson, json);
            Assert.NotNull(fromEnvelope);
            Assert.Single(fromEnvelope!.expeditions);
            Assert.NotNull(fromEnvelope.vehicles); // fresh empty garage

            // Legacy shape 2: the bare list (pre-checksum stores).
            string bareJson = json.Serialize(legacyList);
            ExpeditionAggregateState? fromBare = ExpeditionAggregateCodec.Decode(bareJson, json);
            Assert.NotNull(fromBare);
            Assert.Single(fromBare!.expeditions);
            Assert.Equal("s1:loc_y", fromBare.expeditions[0].expeditionId);

            // Malformed JSON propagates — the store service's logging catch
            // rejects the save, exactly as the legacy bare-list store did.
            Assert.ThrowsAny<Exception>(() => ExpeditionAggregateCodec.Decode("{\"nope\"", json));
        }

        [Fact]
        public void AggregateState_RestoreContinuesInFlightVehicleExpedition()
        {
            var sys = new ExpeditionSystem();
            sys.Start(Def(6), "s", 1, vehicle: Vehicle(speed: 2f, breakdown: 1f));
            var rng = new SeededRng(31);
            sys.TickHours(1f, rng); // guaranteed breakdown on the first travel tick
            Assert.True(sys.Active["s"].vehicleBrokenDown);

            var aggregate = new ExpeditionAggregateState
            {
                expeditions = sys.CaptureState(),
                vehicles = new ExpeditionVehicleState(),
            };

            var restored = new ExpeditionSystem();
            restored.RestoreState(aggregate.expeditions);
            var e = restored.Active["s"];
            Assert.True(e.vehicleBrokenDown, "mid-route breakdown must survive save/restore");
            Assert.Equal("vehicle_cargo_truck", e.vehicleId);
            // And the restored sortie keeps walking on foot speed.
            var rng2 = new SeededRng(32);
            int before = e.travelTicksCompleted;
            restored.TickHours(1f, rng2);
            Assert.True(restored.Active["s"].travelTicksCompleted - before <= 1);
        }

        [Fact]
        public void VehicleCatalogLoader_ReadsDataAuthority_AndToleratesAbsence()
        {
            var json = new SystemTextJsonSerializer();
            var files = new FileSystemIO();

            // Repo data dir: walk up like the host does.
            string? dir = AppContext.BaseDirectory;
            string? dataDir = null;
            while (dir != null)
            {
                string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(probe)) { dataDir = probe; break; }
                dir = Path.GetDirectoryName(dir);
            }
            Assert.NotNull(dataDir);

            VehicleCatalog catalog = VehicleCatalogLoader.Load(dataDir!, files, json);
            Assert.True(catalog.vehicles.Count >= 3, "vehicles.json must define the starter fleet");
            Assert.Contains(catalog.track_gear, g => g.gear_id == "vehicle_track_gear_standard");
            foreach (var v in catalog.vehicles)
                Assert.False(string.IsNullOrEmpty(v.vehicle_id));

            // Missing dir → empty catalog (foot expeditions stay valid).
            VehicleCatalog empty = VehicleCatalogLoader.Load("/nonexistent_dir_xyz", files, json);
            Assert.Empty(empty.vehicles);
        }

        [Fact]
        public void TrackGear_InstallsRepairsAndPersistsNormalizedEffects()
        {
            var vehicles = new ExpeditionVehicleSystem(new SeededRng(7));
            vehicles.LoadCatalog(new VehicleCatalog
            {
                vehicles = new List<VehicleDefinition>
                {
                    new VehicleDefinition
                    {
                        vehicle_id = "vehicle_test",
                        display_name = "Test Vehicle",
                        cargo_capacity = 80f,
                        speed_multiplier = 1.2f,
                        fuel_consumption_per_km = 0.4f
                    }
                }
            });
            Assert.True(vehicles.AcquireVehicle("vehicle_test").IsSuccess);

            var installed = vehicles.InstallTrackGear("vehicle_test", "vehicle_track_gear_test", 1.4f, 0.5f, 40f);
            Assert.True(installed.IsSuccess);
            var live = vehicles.GetVehicle("vehicle_test")!;
            Assert.Equal(1.16f, live.trackGear.EffectiveTractionMultiplier(), 3);
            Assert.Equal(0.8f, live.trackGear.EffectiveBreakdownRiskMultiplier(), 3);

            Assert.True(vehicles.RepairTrackGear("vehicle_test", 60f).IsSuccess);
            var saved = vehicles.CaptureState();
            var restored = new ExpeditionVehicleSystem(new SeededRng(8));
            restored.RestoreState(saved);
            var gear = restored.GetVehicle("vehicle_test")!.trackGear;
            Assert.Equal("vehicle_track_gear_test", gear.gearId);
            Assert.Equal(100f, gear.condition);
            Assert.Equal(1.4f, gear.EffectiveTractionMultiplier(), 3);
            Assert.Equal(0.5f, gear.EffectiveBreakdownRiskMultiplier(), 3);
        }

        [Fact]
        public void TrackGear_ProfileProjectionKeepsTravelMathInCore()
        {
            var vehicles = new ExpeditionVehicleSystem(new SeededRng(9));
            vehicles.LoadCatalog(new VehicleCatalog
            {
                vehicles = new List<VehicleDefinition>
                {
                    new VehicleDefinition
                    {
                        vehicle_id = "vehicle_test",
                        speed_multiplier = 1.2f,
                        fuel_consumption_per_km = 0.4f
                    }
                }
            });
            Assert.True(vehicles.AcquireVehicle("vehicle_test").IsSuccess);
            Assert.True(vehicles.InstallTrackGear("vehicle_test", "vehicle_track_gear_test", 1.5f, 0.4f).IsSuccess);

            ExpeditionVehicleProfile profile = vehicles.CreateExpeditionProfile("vehicle_test", 2.5f)!;
            Assert.Equal(1.8f, profile.speedMultiplier, 3);
            Assert.Equal(1f, profile.fuelPerTravelTick, 3);
            Assert.Equal(0f, profile.breakdownChancePerTick, 3);
        }

        [Fact]
        public void TrackGear_RejectsInvalidFactsWithoutMutation()
        {
            var vehicles = new ExpeditionVehicleSystem(new SeededRng(10));
            vehicles.LoadCatalog(new VehicleCatalog
            {
                vehicles = new List<VehicleDefinition>
                {
                    new VehicleDefinition { vehicle_id = "vehicle_test" }
                }
            });
            Assert.True(vehicles.AcquireVehicle("vehicle_test").IsSuccess);
            var before = vehicles.GetVehicle("vehicle_test")!.trackGear.gearId;

            Assert.False(vehicles.InstallTrackGear("vehicle_test", "bad", 2.1f, 0.5f).IsSuccess);
            Assert.Equal(before, vehicles.GetVehicle("vehicle_test")!.trackGear.gearId);
        }
    }
}
