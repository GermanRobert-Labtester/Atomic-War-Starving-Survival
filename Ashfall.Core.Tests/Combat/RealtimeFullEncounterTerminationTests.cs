// SPDX-License-Identifier: MIT
// T12 realtime combat playability pass — a full encounter must not dead-end.
// The production host pumps TickRealtime through CombatHostSession.PumpRealtime;
// these tests prove the realtime loop itself always reaches a terminal state
// (unattended loss), is seed-reproducible, and that a squad that cannot shoot
// can always break contact via the realtime flee extract.
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;

namespace Ashfall.Core.Tests.Combat
{
    public class RealtimeFullEncounterTerminationTests
    {
        public RealtimeFullEncounterTerminationTests()
        {
            CombatCatalog.SeedDefaults();
            CombatArenaCatalog.SeedDefaults();
        }

        private static TacticalCombatSystem BeginFight(int seed = 42, float enemyHealth = 40f, int playerAmmo = 30)
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
                    AmmoRemaining = playerAmmo
                }
            };
            Assert.True(sys.BeginEncounter(
                "enc_rt", "exp_rt", "loc_rt", "Realtime Yard", 1, seed,
                roster, weapons, enemyCount: 1, enemyHealth: enemyHealth));
            Assert.True(sys.EnableRealtime(CombatArenaCatalog.DefaultArenaId, new SeededRng(seed)));
            return sys;
        }

        private static readonly CombatInputFrame Idle = new CombatInputFrame { SubjectId = "p0" };

        private static int PumpUntilResolved(TacticalCombatSystem sys, SeededRng rng, int maxTicks)
        {
            const float dt = TacticalCombatSystem.RealtimeSimDt;
            for (int i = 0; i < maxTicks; i++)
            {
                if (sys.State.Resolved) return i;
                var tick = sys.TickRealtime(dt, Idle, rng);
                Assert.True(tick.Success, "tick " + i + ": " + tick.Message);
            }
            return -1;
        }

        [Fact]
        public void UnattendedRealtimeEncounter_AlwaysReachesTerminalState()
        {
            var sys = BeginFight(42);
            int endedCount = 0;
            sys.OnEncounterEnded += _ => endedCount++;

            int resolvedAt = PumpUntilResolved(sys, new SeededRng(42), 20000);

            Assert.True(resolvedAt >= 0,
                "unattended realtime encounter must resolve within the tick cap (dead-end)");
            Assert.Contains(sys.State.Phase, new[]
                { (int)CombatPhase.Won, (int)CombatPhase.Lost, (int)CombatPhase.Retreated });
            Assert.Equal(1, endedCount);
            Assert.False(string.IsNullOrEmpty(sys.State.OutcomeText));
        }

        [Fact]
        public void UnattendedRealtimeEncounter_IsDeterministic_SameSeedSameResolution()
        {
            var a = BeginFight(77);
            var b = BeginFight(77);
            int resolvedA = PumpUntilResolved(a, new SeededRng(77), 20000);
            int resolvedB = PumpUntilResolved(b, new SeededRng(77), 20000);

            Assert.Equal(resolvedA, resolvedB);
            Assert.Equal(a.State.Phase, b.State.Phase);
            Assert.Equal(a.State.OutcomeText, b.State.OutcomeText);
            var pa = a.State.Combatants.Find(c => c.IsPlayer)!;
            var pb = b.State.Combatants.Find(c => c.IsPlayer)!;
            Assert.Equal(pa.Health, pb.Health);
        }

        [Fact]
        public void AmmoStarvedSquad_CanAlwaysBreakContact_ViaRealtimeExtract()
        {
            // The starved-shooter contract: with no ammunition the squad cannot
            // win by fire, so the realtime flee extract must remain a working
            // exit (retreat resolves instead of freezing the encounter active).
            var sys = BeginFight(21, enemyHealth: 5000f, playerAmmo: 0);
            Assert.True(sys.RequestFlee().Success);

            int resolvedAt = PumpUntilResolved(sys, new SeededRng(21), 20000);

            Assert.True(resolvedAt >= 0, "ammo-starved squad must resolve via extract");
            Assert.Equal((int)CombatPhase.Retreated, sys.State.Phase);
            Assert.Contains(sys.State.Events, e => e.Kind == "retreat");
        }

        [Fact]
        public void DownedLastEnemy_BleedsOut_AndResolvesWon()
        {
            // T23 regression: a downed last hostile must bleed out on the
            // realtime clock and resolve Won — previously it blocked the
            // encounter forever (the auto-fire sweep stalled).
            var sys = BeginFight(42);
            int endedCount = 0;
            sys.OnEncounterEnded += _ => endedCount++;
            var enemy = sys.State.Combatants.Find(c => !c.IsPlayer)!;
            enemy.Health = 0f;
            enemy.IsDowned = true;
            enemy.BleedTurnsRemaining = 2;

            var rng = new SeededRng(42);
            const float dt = TacticalCombatSystem.RealtimeSimDt;
            for (int i = 0; i < 200 && !sys.State.Resolved; i++)
                Assert.True(sys.TickRealtime(dt, Idle, rng).Success);

            Assert.True(sys.State.Resolved, "downed last enemy must bleed out and resolve");
            Assert.Equal((int)CombatPhase.Won, sys.State.Phase);
            Assert.Equal(1, endedCount);
            Assert.Contains(sys.State.Events, e => e.Kind == "bleed");
            Assert.Contains(sys.State.Events, e => e.Kind == "death");
            Assert.Contains(sys.State.Events, e => e.Kind == "victory");
        }
    }
}
