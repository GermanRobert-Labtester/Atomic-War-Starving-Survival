// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;

namespace Ashfall.Core.Tests.Combat
{
    public class RealtimeFleeInterruptTests
    {
        public RealtimeFleeInterruptTests()
        {
            CombatCatalog.SeedDefaults();
            CombatArenaCatalog.SeedDefaults();
        }

        private static TacticalCombatSystem BeginFight(int seed = 42, float enemyHealth = 500f, float playerHealth = 200f)
        {
            var sys = new TacticalCombatSystem();
            var roster = new List<CombatantState>
            {
                new CombatantState
                {
                    Id = "p0",
                    Name = "Scout",
                    SurvivorId = "survivor_0",
                    IsPlayer = true,
                    Health = playerHealth,
                    MaxHealth = playerHealth,
                    Lane = 1
                }
            };
            var weapons = new List<WeaponInstanceState>
            {
                new WeaponInstanceState
                {
                    InstanceId = "w0",
                    WeaponId = "weapon_assault_rifle",
                    OwnerSurvivorId = "survivor_0",
                    ConditionPct = 0.95f,
                    AmmoId = "ammo_556",
                    AmmoRemaining = 30,
                    MagazineCapacity = 30
                }
            };
            Assert.True(sys.BeginEncounter(
                "enc_rt_flee", "exp_rt", "loc_rt", "Realtime Yard", 1, seed,
                roster, weapons, enemyCount: 1, enemyHealth: enemyHealth));
            var enemy = sys.State.Combatants.Find(c => !c.IsPlayer)!;
            enemy.AiSpecialMove = "None";
            enemy.AiAccuracyMod = 1.4f;
            Assert.True(sys.EnableRealtime(CombatArenaCatalog.DefaultArenaId, new SeededRng(seed)));
            return sys;
        }

        [Fact]
        public void RequestFlee_SetsFleeMode_AndBlocksLastStand()
        {
            var sys = BeginFight(11);
            var ok = sys.RequestFlee();
            Assert.True(ok.Success, ok.Message);
            Assert.Equal((int)CombatMotionMode.Flee, sys.State.Combatants.Find(c => c.IsPlayer)!.MotionMode);
            Assert.Contains(sys.State.Events, e => e.Kind == "flee_start");

            var last = BeginFight(12);
            last.PlayerLastStand("p0", new SeededRng(1));
            var blocked = last.RequestFlee();
            Assert.False(blocked.Success);
        }

        [Fact]
        public void Flee_SteersTowardExtract_AndCompletesHold()
        {
            var sys = BeginFight(21, enemyHealth: 5000f);
            var player = sys.State.Combatants.Find(c => c.IsPlayer)!;
            // Start away from extract (extract is near x=0.5..2.5).
            player.PosX = 10f;
            player.PosY = 3f;
            Assert.True(sys.RequestFlee().Success);

            var rng = new SeededRng(21);
            var idle = new CombatInputFrame { SubjectId = "p0" };
            bool extracted = false;
            for (int i = 0; i < 200; i++)
            {
                var tick = sys.TickRealtime(TacticalCombatSystem.RealtimeSimDt, idle, rng);
                if (sys.State.Resolved)
                {
                    extracted = sys.State.Phase == (int)CombatPhase.Retreated;
                    break;
                }
                Assert.True(tick.Success, "tick " + i + ": " + tick.Message);
            }

            Assert.True(extracted, "squad should complete extract hold");
            Assert.True(player.PosX < 4f, "player should have moved toward extract");
            Assert.Contains(sys.State.Events, e => e.Kind == "retreat");
        }

        [Fact]
        public void Flee_CanTakeFleeHit_WhileRunning()
        {
            var sys = BeginFight(33, enemyHealth: 5000f, playerHealth: 200f);
            var player = sys.State.Combatants.Find(c => c.IsPlayer)!;
            var enemy = sys.State.Combatants.Find(c => !c.IsPlayer)!;
            player.PosX = 8f;
            player.PosY = 3f;
            enemy.PosX = 9f;
            enemy.PosY = 3f;
            enemy.Lane = player.Lane;
            enemy.AiAccuracyMod = 5f; // aimbot for the test
            float startHp = player.Health;
            Assert.True(sys.RequestFlee().Success);

            var rng = new SeededRng(33);
            var idle = new CombatInputFrame { SubjectId = "p0" };
            for (int i = 0; i < 40; i++)
                sys.TickRealtime(TacticalCombatSystem.RealtimeSimDt, idle, rng);

            Assert.True(
                player.Health < startHp
                || sys.State.Events.Any(e => e.Kind == "flee_hit"),
                "running away must remain targetable");
        }

        [Fact]
        public void FleeInput_OnTick_StartsExtract()
        {
            var sys = BeginFight(44, enemyHealth: 5000f);
            var input = new CombatInputFrame { SubjectId = "p0", Flee = true };
            Assert.True(sys.TickRealtime(TacticalCombatSystem.RealtimeSimDt, input, new SeededRng(44)).Success);
            Assert.Equal((int)CombatMotionMode.Flee, sys.State.Combatants.Find(c => c.IsPlayer)!.MotionMode);
        }

        [Fact]
        public void LegacyPlayerRetreat_StillWorks_WhenRealtimeOff()
        {
            var sys = new TacticalCombatSystem();
            var roster = new List<CombatantState>
            {
                new CombatantState
                {
                    Id = "p0", Name = "Scout", SurvivorId = "survivor_0",
                    IsPlayer = true, Health = 100, MaxHealth = 100, Lane = 1
                }
            };
            var weapons = new List<WeaponInstanceState>
            {
                new WeaponInstanceState
                {
                    InstanceId = "w0", WeaponId = "weapon_assault_rifle",
                    OwnerSurvivorId = "survivor_0", ConditionPct = 0.9f,
                    AmmoId = "ammo_556", AmmoRemaining = 30
                }
            };
            Assert.True(sys.BeginEncounter(
                "enc_legacy_flee", "exp", "loc", "Yard", 1, 55,
                roster, weapons, enemyCount: 1, enemyHealth: 40));
            Assert.False(sys.State.RealtimeActive);
            // Mobility of HoldPosition is high enough that many seeds succeed;
            // just prove the API still returns a result without requiring realtime.
            var r = sys.PlayerRetreat(new SeededRng(55));
            Assert.False(string.IsNullOrEmpty(r.Message));
        }
    }
}
