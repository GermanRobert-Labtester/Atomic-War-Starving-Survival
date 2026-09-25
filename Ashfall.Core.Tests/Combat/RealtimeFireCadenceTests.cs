// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;

namespace Ashfall.Core.Tests.Combat
{
    public class RealtimeFireCadenceTests
    {
        public RealtimeFireCadenceTests()
        {
            CombatCatalog.SeedDefaults();
            CombatArenaCatalog.SeedDefaults();
        }

        private static TacticalCombatSystem BeginFight(int seed = 42, int ammo = 90)
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
                    ConditionPct = 0.95f,
                    AmmoId = "ammo_556",
                    AmmoRemaining = ammo,
                    MagazineCapacity = 30
                }
            };
            Assert.True(sys.BeginEncounter(
                "enc_rt_fire", "exp_rt", "loc_rt", "Realtime Yard", 1, seed,
                roster, weapons, enemyCount: 1, enemyHealth: 5000));
            Assert.True(sys.EnableRealtime(CombatArenaCatalog.DefaultArenaId, new SeededRng(seed)));
            return sys;
        }

        [Fact]
        public void FireHeld_ConsumesAmmoOnCadence_Deterministic()
        {
            var a = BeginFight(1001);
            var b = BeginFight(1001);
            var enemyA = a.State.Combatants.Find(c => !c.IsPlayer)!;
            var enemyB = b.State.Combatants.Find(c => !c.IsPlayer)!;
            var input = new CombatInputFrame
            {
                SubjectId = "p0",
                FireHeld = true,
                AimTargetId = enemyA.Id,
                Brace = true
            };
            var inputB = new CombatInputFrame
            {
                SubjectId = "p0",
                FireHeld = true,
                AimTargetId = enemyB.Id,
                Brace = true
            };

            const float dt = TacticalCombatSystem.RealtimeSimDt;
            var rngA = new SeededRng(55);
            var rngB = new SeededRng(55);
            for (int i = 0; i < 40; i++)
            {
                var ra = a.TickRealtime(dt, input, rngA);
                var rb = b.TickRealtime(dt, inputB, rngB);
                Assert.True(ra.Success, "A tick " + i + ": " + ra.Message);
                Assert.True(rb.Success, "B tick " + i + ": " + rb.Message);
                Assert.False(a.State.Resolved);
            }

            var wa = a.State.Weapons[0];
            var wb = b.State.Weapons[0];
            Assert.Equal(wa.AmmoRemaining, wb.AmmoRemaining);
            Assert.True(wa.AmmoRemaining < 90, "Held fire should expend magazine rounds");
            Assert.Equal(a.State.Combatants.Find(c => c.IsPlayer)!.FireCooldown,
                b.State.Combatants.Find(c => c.IsPlayer)!.FireCooldown, 4);
        }

        [Fact]
        public void FireCooldown_GatesShotsBelowWeaponRpm()
        {
            var sys = BeginFight(7, ammo: 90);
            var enemy = sys.State.Combatants.Find(c => !c.IsPlayer)!;
            var def = CombatCatalog.GetWeapon("weapon_assault_rifle");
            Assert.NotNull(def);
            float cooldown = TacticalCombatSystem.FireCooldownSeconds(def);
            Assert.True(cooldown > 0.05f);

            var input = new CombatInputFrame
            {
                SubjectId = "p0",
                FireHeld = true,
                AimTargetId = enemy.Id
            };

            int before = sys.State.Weapons[0].AmmoRemaining;
            // One tick at dt << cooldown: at most one trigger pull.
            Assert.True(sys.TickRealtime(TacticalCombatSystem.RealtimeSimDt, input, new SeededRng(3)).Success);
            int afterOne = sys.State.Weapons[0].AmmoRemaining;
            Assert.True(afterOne < before);

            int mid = afterOne;
            // Another tiny tick while cooldown still hot should not fire again.
            Assert.True(sys.TickRealtime(TacticalCombatSystem.RealtimeSimDt * 0.25f, input, new SeededRng(4)).Success);
            Assert.Equal(mid, sys.State.Weapons[0].AmmoRemaining);
            Assert.True(sys.State.Combatants.Find(c => c.IsPlayer)!.FireCooldown > 0f);
        }

        [Fact]
        public void Climb_BlocksRealtimeFire()
        {
            var sys = BeginFight(9);
            var player = sys.State.Combatants.Find(c => c.IsPlayer)!;
            player.PosX = 10f;
            player.PosY = 0.5f;
            player.Stamina01 = 1f;
            player.MotionMode = (int)CombatMotionMode.Climb;

            var enemy = sys.State.Combatants.Find(c => !c.IsPlayer)!;
            int before = sys.State.Weapons[0].AmmoRemaining;
            var input = new CombatInputFrame
            {
                SubjectId = "p0",
                Climb = true,
                MoveY = 1f,
                FireHeld = true,
                AimTargetId = enemy.Id
            };
            Assert.True(sys.TickRealtime(TacticalCombatSystem.RealtimeSimDt, input, new SeededRng(2)).Success);
            Assert.Equal(before, sys.State.Weapons[0].AmmoRemaining);
        }

        [Fact]
        public void AimTarget_UpdatesAimRadTowardHostile()
        {
            var sys = BeginFight(11);
            var player = sys.State.Combatants.Find(c => c.IsPlayer)!;
            var enemy = sys.State.Combatants.Find(c => !c.IsPlayer)!;
            player.AimRad = 0f;
            var input = new CombatInputFrame
            {
                SubjectId = "p0",
                AimTargetId = enemy.Id,
                Brace = true
            };
            Assert.True(sys.TickRealtime(TacticalCombatSystem.RealtimeSimDt, input, null).Success);
            float expected = (float)System.Math.Atan2(enemy.PosY - player.PosY, enemy.PosX - player.PosX);
            Assert.Equal(expected, player.AimRad, 3);
        }

        [Fact]
        public void EvaluateFire_AllowsActiveRealtimePhase()
        {
            var sys = BeginFight(13);
            var enemy = sys.State.Combatants.Find(c => !c.IsPlayer)!;
            var pf = sys.EvaluateFire(enemy.Id);
            Assert.True(pf.CanExecute, pf.Reason);
        }
    }
}
