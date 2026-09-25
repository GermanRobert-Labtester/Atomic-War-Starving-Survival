// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;

namespace Ashfall.Core.Tests.Combat
{
    public class RealtimeLocomotionTests
    {
        public RealtimeLocomotionTests()
        {
            CombatCatalog.SeedDefaults();
            CombatArenaCatalog.SeedDefaults();
        }

        private static TacticalCombatSystem BeginFight(int seed = 42)
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
                    Health = 100,
                    MaxHealth = 100,
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
                    ConditionPct = 0.9f,
                    AmmoId = "ammo_556",
                    AmmoRemaining = 30
                }
            };
            Assert.True(sys.BeginEncounter(
                "enc_rt", "exp_rt", "loc_rt", "Realtime Yard", 1, seed,
                roster, weapons, enemyCount: 1, enemyHealth: 40));
            Assert.True(sys.EnableRealtime(CombatArenaCatalog.DefaultArenaId, new SeededRng(seed)));
            return sys;
        }

        [Fact]
        public void EnableRealtime_SetsActivePhaseAndSeedsPose()
        {
            var sys = BeginFight();
            Assert.True(sys.State.RealtimeActive);
            Assert.Equal((int)CombatPhase.ActiveRealtime, sys.State.Phase);
            Assert.Equal(CombatArenaCatalog.DefaultArenaId, sys.State.ArenaId);
            var player = sys.State.Combatants.Find(c => c.IsPlayer);
            Assert.NotNull(player);
            Assert.True(player!.PoseSeeded);
            Assert.True(player.PosX > 0f);
        }

        [Fact]
        public void Walk_MovesDeterministically_SameSeedSamePath()
        {
            var a = BeginFight(1001);
            var b = BeginFight(1001);
            var input = new CombatInputFrame { SubjectId = "p0", MoveX = 1f, MoveY = 0f, Sprint = false };
            const float dt = TacticalCombatSystem.RealtimeSimDt;
            for (int i = 0; i < 20; i++)
            {
                Assert.True(a.TickRealtime(dt, input, null).Success);
                Assert.True(b.TickRealtime(dt, input, null).Success);
            }

            var pa = a.State.Combatants.Find(c => c.IsPlayer)!;
            var pb = b.State.Combatants.Find(c => c.IsPlayer)!;
            Assert.Equal(pa.PosX, pb.PosX, 4);
            Assert.Equal(pa.PosY, pb.PosY, 4);
            Assert.Equal((int)CombatMotionMode.Walk, pa.MotionMode);
            Assert.True(pa.PosX > 6f); // spawned near x=6, walked +X
        }

        [Fact]
        public void Run_IsFasterThanWalk_ForSameTicks()
        {
            var walk = BeginFight(77);
            var run = BeginFight(77);
            var walkInput = new CombatInputFrame { SubjectId = "p0", MoveX = 1f, Sprint = false };
            var runInput = new CombatInputFrame { SubjectId = "p0", MoveX = 1f, Sprint = true };
            const float dt = TacticalCombatSystem.RealtimeSimDt;
            for (int i = 0; i < 10; i++)
            {
                walk.TickRealtime(dt, walkInput, null);
                run.TickRealtime(dt, runInput, null);
            }

            float walkX = walk.State.Combatants.Find(c => c.IsPlayer)!.PosX;
            float runX = run.State.Combatants.Find(c => c.IsPlayer)!.PosX;
            Assert.True(runX > walkX + 0.5f);
            Assert.Equal((int)CombatMotionMode.Run, run.State.Combatants.Find(c => c.IsPlayer)!.MotionMode);
        }

        [Fact]
        public void Climb_RequiresSegment_AndAdvancesAlongIt()
        {
            var sys = BeginFight(9);
            var player = sys.State.Combatants.Find(c => c.IsPlayer)!;
            // Place on the default climb segment at x=10.
            player.PosX = 10f;
            player.PosY = 0.5f;
            player.Stamina01 = 1f;

            var far = BeginFight(9);
            var farPlayer = far.State.Combatants.Find(c => c.IsPlayer)!;
            farPlayer.PosX = 2f;
            farPlayer.PosY = 1f;

            float startY = player.PosY;
            var climb = new CombatInputFrame { SubjectId = "p0", Climb = true, MoveY = 1f };
            for (int i = 0; i < 15; i++)
            {
                Assert.True(sys.TickRealtime(TacticalCombatSystem.RealtimeSimDt, climb, null).Success);
                far.TickRealtime(TacticalCombatSystem.RealtimeSimDt, climb, null);
            }

            player = sys.State.Combatants.Find(c => c.IsPlayer)!;
            farPlayer = far.State.Combatants.Find(c => c.IsPlayer)!;
            Assert.True(player.PosY > startY + 0.4f);
            Assert.Equal((int)CombatMotionMode.Climb, player.MotionMode);
            Assert.Equal(1f, farPlayer.PosY, 3); // climb without segment is a no-op
        }

        [Fact]
        public void CaptureRestore_PreservesRealtimePose()
        {
            var sys = BeginFight(55);
            var input = new CombatInputFrame { SubjectId = "p0", MoveX = 1f, Sprint = true };
            for (int i = 0; i < 8; i++)
                sys.TickRealtime(TacticalCombatSystem.RealtimeSimDt, input, null);

            var captured = sys.CaptureState();
            var restored = new TacticalCombatSystem();
            restored.RestoreState(captured);

            Assert.True(restored.State.RealtimeActive);
            Assert.Equal(sys.State.SimTick, restored.State.SimTick);
            Assert.Equal(
                sys.State.Combatants.Find(c => c.IsPlayer)!.PosX,
                restored.State.Combatants.Find(c => c.IsPlayer)!.PosX,
                4);
        }

        [Fact]
        public void DefaultArenaCatalog_HasClimbAndExtract()
        {
            var arena = CombatArenaCatalog.GetOrDefault(null);
            Assert.Equal(CombatArenaCatalog.DefaultArenaId, arena.id);
            Assert.NotEmpty(arena.climb_segments);
            Assert.True(arena.extract_volume.w > 0f);
            Assert.True(CombatArenaCatalog.NearClimbSegment(arena, 10f, 0.2f, 1f, out var seg));
            Assert.NotNull(seg);
        }
    }
}
