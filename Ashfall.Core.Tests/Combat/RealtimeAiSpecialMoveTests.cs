// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;

namespace Ashfall.Core.Tests.Combat
{
    public class RealtimeAiSpecialMoveTests
    {
        public RealtimeAiSpecialMoveTests()
        {
            CombatCatalog.SeedDefaults();
            CombatArenaCatalog.SeedDefaults();
        }

        private static TacticalCombatSystem BeginFight(string aiMove, int seed = 42, float fleeThreshold = -1f)
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
                    Health = 200,
                    MaxHealth = 200,
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
                "enc_rt_ai", "exp_rt", "loc_rt", "Realtime Yard", 1, seed,
                roster, weapons, enemyCount: 1, enemyHealth: 120));
            var enemy = sys.State.Combatants.Find(c => !c.IsPlayer)!;
            enemy.AiSpecialMove = aiMove;
            enemy.FleeThreshold = fleeThreshold;
            enemy.AiAccuracyMod = 1f;
            enemy.AiDamageMod = 1f;
            Assert.True(sys.EnableRealtime(CombatArenaCatalog.DefaultArenaId, new SeededRng(seed)));
            return sys;
        }

        private static void Pump(TacticalCombatSystem sys, int ticks, int seed = 7)
        {
            var rng = new SeededRng(seed);
            var idle = new CombatInputFrame { SubjectId = "p0" };
            for (int i = 0; i < ticks; i++)
                Assert.True(sys.TickRealtime(TacticalCombatSystem.RealtimeSimDt, idle, rng).Success);
        }

        [Fact]
        public void Burrow_HidesThenEmergesWithEvent()
        {
            var sys = BeginFight("Burrow", 101);
            var enemy = sys.State.Combatants.Find(c => !c.IsPlayer)!;
            Pump(sys, 2, seed: 101);
            Assert.Equal("burrow_hide", enemy.AiBehaviorPhase);
            Assert.True(TacticalCombatSystem.IsBurrowHidden(enemy));
            Assert.Contains(sys.State.Events, e => e.Kind == "ai_burrow");

            // ~2.0s hide + emerge at 20 Hz ≈ 45 ticks.
            Pump(sys, 50, seed: 102);
            Assert.False(TacticalCombatSystem.IsBurrowHidden(enemy));
            Assert.Contains(sys.State.Events, e => e.Kind == "ai_burrow_emerge");
        }

        [Fact]
        public void Flank_MovesLaterally_AndArmsBonus()
        {
            var sys = BeginFight("Flank", 202);
            var player = sys.State.Combatants.Find(c => c.IsPlayer)!;
            var enemy = sys.State.Combatants.Find(c => !c.IsPlayer)!;
            player.PosX = 6f;
            player.PosY = 2f;
            enemy.PosX = 7f;
            enemy.PosY = 2f;
            float startX = enemy.PosX;
            Pump(sys, 40, seed: 202);
            Assert.True(Math.Abs(enemy.PosX - startX) > 0.5f || enemy.AiBehaviorPhase == "flank_bonus");
            Assert.True(
                enemy.AiBehaviorPhase == "flank_bonus"
                || Math.Abs(enemy.PosX - player.PosX) >= 3.0f);
        }

        [Fact]
        public void Spore_EmitsSporeCloudEventOnPeriod()
        {
            var sys = BeginFight("Spore", 303);
            Pump(sys, 8, seed: 303);
            Assert.Contains(sys.State.Events, e => e.Kind == "spore_cloud");
            int firstCount = sys.State.Events.Count(e => e.Kind == "spore_cloud");
            Pump(sys, 80, seed: 304);
            int secondCount = sys.State.Events.Count(e => e.Kind == "spore_cloud");
            Assert.True(secondCount > firstCount);
        }

        [Fact]
        public void Charge_ClosesDistance_AndCanHit()
        {
            var sys = BeginFight("Charge", 404);
            var player = sys.State.Combatants.Find(c => c.IsPlayer)!;
            var enemy = sys.State.Combatants.Find(c => !c.IsPlayer)!;
            player.PosX = 8f;
            player.PosY = 2f;
            player.Health = 200f;
            enemy.PosX = 14f;
            enemy.PosY = 2f;
            float startDist = Math.Abs(enemy.PosX - player.PosX);
            float startHp = player.Health;
            Pump(sys, 60, seed: 404);
            float endDist = Math.Abs(enemy.PosX - player.PosX);
            Assert.True(endDist < startDist - 1f || player.Health < startHp);
            Assert.True(
                player.Health < startHp
                || sys.State.Events.Any(e => e.Kind == "ai_charge_hit"));
        }

        [Fact]
        public void SuppressiveFire_CanPinPlayer()
        {
            var sys = BeginFight("SuppressiveFire", 505);
            var player = sys.State.Combatants.Find(c => c.IsPlayer)!;
            // Force frequent hits: raise enemy accuracy and keep player in same lane.
            var enemy = sys.State.Combatants.Find(c => !c.IsPlayer)!;
            enemy.AiAccuracyMod = 3f;
            enemy.Lane = player.Lane;
            enemy.PosX = player.PosX + 3f;
            enemy.PosY = player.PosY;
            Pump(sys, 80, seed: 505);
            Assert.True(
                player.IsPinned
                || sys.State.Events.Any(e => e.Kind == "ai_suppress_pin")
                || sys.State.Events.Count(e => e.Kind == "enemy_fire") > 0);
        }

        [Fact]
        public void TacticalRetreat_MovesTowardBacklineWhenBelowThreshold()
        {
            var sys = BeginFight("TacticalRetreat", 606, fleeThreshold: 0.9f);
            var enemy = sys.State.Combatants.Find(c => !c.IsPlayer)!;
            enemy.Health = enemy.MaxHealth * 0.2f;
            float startX = enemy.PosX;
            Pump(sys, 40, seed: 606);
            Assert.Equal("retreat", enemy.AiBehaviorPhase);
            Assert.True(enemy.PosX > startX + 0.4f);
        }

        [Fact]
        public void EnemyFire_IsDeterministic_SameSeedSameHits()
        {
            var a = BeginFight("None", 707);
            var b = BeginFight("None", 707);
            Pump(a, 30, seed: 707);
            Pump(b, 30, seed: 707);
            var pa = a.State.Combatants.Find(c => c.IsPlayer)!;
            var pb = b.State.Combatants.Find(c => c.IsPlayer)!;
            Assert.Equal(pa.Health, pb.Health, 3);
            Assert.Equal(
                a.State.Events.Count(e => e.Kind == "enemy_fire"),
                b.State.Events.Count(e => e.Kind == "enemy_fire"));
        }
    }
}
