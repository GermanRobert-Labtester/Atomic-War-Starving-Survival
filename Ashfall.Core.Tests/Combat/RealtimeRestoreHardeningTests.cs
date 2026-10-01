// SPDX-License-Identifier: MIT
// T17 — mid-encounter realtime save/restore hardening: a restored mid-fight
// encounter must keep its captured poses, a legacy/foreign realtime save with
// zero poses must be seeded before combat resumes, and resolved restores must
// stay untouched.
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;

namespace Ashfall.Core.Tests.Combat
{
    public class RealtimeRestoreHardeningTests
    {
        public RealtimeRestoreHardeningTests()
        {
            CombatCatalog.SeedDefaults();
            CombatArenaCatalog.SeedDefaults();
        }

        private static TacticalCombatSystem BeginFight(int seed = 42, float enemyHealth = 40f)
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

        private static readonly CombatInputFrame Idle = new CombatInputFrame { SubjectId = "p0" };

        [Fact]
        public void RestoreState_MidRealtimeFight_PreservesPosesAndContinues()
        {
            var sys = BeginFight(42);
            var input = new CombatInputFrame { SubjectId = "p0", MoveX = 1f };
            const float dt = TacticalCombatSystem.RealtimeSimDt;
            for (int i = 0; i < 40; i++)
                Assert.True(sys.TickRealtime(dt, input, new SeededRng(42)).Success);

            var player = sys.State.Combatants.Find(c => c.IsPlayer)!;
            float capturedX = player.PosX;
            float capturedY = player.PosY;
            int capturedTick = sys.State.SimTick;

            var restored = new TacticalCombatSystem();
            restored.RestoreState(sys.CaptureState());

            var rp = restored.State.Combatants.Find(c => c.IsPlayer)!;
            Assert.True(rp.PoseSeeded);
            Assert.Equal(capturedX, rp.PosX, 4);
            Assert.Equal(capturedY, rp.PosY, 4);
            Assert.Equal(capturedTick, restored.State.SimTick);

            // The restored fight keeps ticking from where it left off.
            Assert.True(restored.TickRealtime(dt, Idle, new SeededRng(42)).Success);
            Assert.Equal(capturedTick + 1, restored.State.SimTick);
        }

        [Fact]
        public void RestoreState_RealtimeActiveLegacyUnseededPoses_SeedsAndTickable()
        {
            var sys = BeginFight(77);
            // Simulate a legacy/foreign save: realtime armed, poses never set.
            foreach (var c in sys.State.Combatants)
            {
                c.PosX = 0f;
                c.PosY = 0f;
                c.PoseSeeded = false;
            }
            var saved = sys.CaptureState();

            var restored = new TacticalCombatSystem();
            restored.RestoreState(saved);

            Assert.True(restored.State.RealtimeActive);
            foreach (var c in restored.State.Combatants)
            {
                Assert.True(c.PoseSeeded, c.Id + " must be pose-seeded after restore");
                if (!c.IsDowned)
                    Assert.NotEqual(0f, c.PosX);
            }
            Assert.True(restored.TickRealtime(
                TacticalCombatSystem.RealtimeSimDt, Idle, new SeededRng(77)).Success);
        }

        [Fact]
        public void RestoreState_ResolvedEncounter_StaysUntouched()
        {
            var sys = BeginFight(21, enemyHealth: 5000f);
            Assert.True(sys.RequestFlee().Success);
            var rng = new SeededRng(21);
            const float dt = TacticalCombatSystem.RealtimeSimDt;
            for (int i = 0; i < 400 && !sys.State.Resolved; i++)
                sys.TickRealtime(dt, Idle, rng);
            Assert.True(sys.State.Resolved);
            string outcome = sys.State.OutcomeText;

            var restored = new TacticalCombatSystem();
            restored.RestoreState(sys.CaptureState());

            Assert.True(restored.State.Resolved);
            Assert.Equal(outcome, restored.State.OutcomeText);
            // EnsureRealtimePosesSeeded must not run for a resolved encounter.
            Assert.DoesNotContain(restored.State.Events, e => e.Kind == "realtime_poses_seeded");
        }
    }
}
