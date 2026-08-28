using System;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Crafting;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
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
    }
}
