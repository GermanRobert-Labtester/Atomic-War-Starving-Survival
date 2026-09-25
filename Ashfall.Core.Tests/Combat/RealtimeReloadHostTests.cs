// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;

namespace Ashfall.Core.Tests.Combat
{
    /// <summary>
    /// Core-side coverage for DEC-358 W2 reload: TickRealtime Reload input and
    /// PlayerReload local-magazine fallback (host ActionReload is a thin wrap).
    /// </summary>
    public class RealtimeReloadHostTests
    {
        public RealtimeReloadHostTests()
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
                    ConditionPct = 0.95f,
                    AmmoId = "ammo_556",
                    AmmoRemaining = 4,
                    MagazineCapacity = 30
                }
            };
            Assert.True(sys.BeginEncounter(
                "enc_rt_reload", "exp_rt", "loc_rt", "Realtime Yard", 1, seed,
                roster, weapons, enemyCount: 1, enemyHealth: 40));
            Assert.True(sys.EnableRealtime(CombatArenaCatalog.DefaultArenaId, new SeededRng(seed)));
            return sys;
        }

        [Fact]
        public void PlayerReload_RefillsMagazine_WhenNoInventoryPort()
        {
            var sys = BeginFight(21);
            Assert.Equal(4, sys.State.Weapons[0].AmmoRemaining);
            var res = sys.PlayerReload("p0");
            Assert.True(res.Success, res.Message);
            Assert.Equal(30, sys.State.Weapons[0].AmmoRemaining);
        }

        [Fact]
        public void TickRealtime_ReloadInput_RefillsMagazine()
        {
            var sys = BeginFight(22);
            var input = new CombatInputFrame { SubjectId = "p0", Reload = true };
            Assert.True(sys.TickRealtime(TacticalCombatSystem.RealtimeSimDt, input, null).Success);
            Assert.Equal(30, sys.State.Weapons[0].AmmoRemaining);
        }

        [Fact]
        public void EvaluateReload_BlocksWhenFull()
        {
            var sys = BeginFight(23);
            sys.PlayerReload("p0");
            var pf = sys.EvaluateReload("p0");
            Assert.False(pf.CanExecute);
        }

        [Fact]
        public void EvaluateReload_AllowsWhenPartial_InRealtime()
        {
            var sys = BeginFight(24);
            var pf = sys.EvaluateReload("p0");
            Assert.True(pf.CanExecute, pf.Reason);
        }

        [Fact]
        public void PlayerReload_UsesConsumeAmmoPort_WhenBound()
        {
            var sys = BeginFight(25);
            int requested = 0;
            sys.Ports = new CombatHostPorts(
                consumeAmmo: (ammoId, amount) =>
                {
                    requested = amount;
                    Assert.Equal("ammo_556", ammoId);
                    return 10;
                });
            var res = sys.PlayerReload("p0");
            Assert.True(res.Success, res.Message);
            Assert.Equal(26, requested); // 30 - 4
            Assert.Equal(14, sys.State.Weapons[0].AmmoRemaining); // 4 + 10
        }
    }
}
