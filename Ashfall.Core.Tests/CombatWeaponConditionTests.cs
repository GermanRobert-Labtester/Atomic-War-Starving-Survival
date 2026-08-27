using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;

namespace Ashfall.Core.Tests
{
    public class CombatWeaponConditionTests
    {
        public CombatWeaponConditionTests() => CombatCatalog.SeedDefaults();

        private static WeaponInstanceState Rifle(string ammo = "ammo_556") => new WeaponInstanceState
        {
            InstanceId = "w1",
            WeaponId = "weapon_assault_rifle",
            OwnerSurvivorId = "sv1",
            ConditionPct = 1f,
            AmmoId = ammo,
            AmmoRemaining = 60
        };

        private static WeaponInstanceState Pipe(string ammo = "ammo_357") => new WeaponInstanceState
        {
            InstanceId = "w2",
            WeaponId = "weapon_pipe_rifle",
            OwnerSurvivorId = "sv1",
            ConditionPct = 1f,
            AmmoId = ammo,
            AmmoRemaining = 40
        };

        [Fact]
        public void JamChance_RisesAsConditionDrops()
        {
            var w = Rifle();
            float pristine = WeaponConditionSystem.ComputeJamChance(w);
            WeaponConditionSystem.Degrade(w, 0.8f); // condition 0.2 < threshold 0.25
            float degraded = WeaponConditionSystem.ComputeJamChance(w);
            Assert.True(degraded > pristine, "degraded condition must raise jam chance");
        }

        [Fact]
        public void JamChance_IsBoundedToOne()
        {
            var w = Pipe();
            WeaponConditionSystem.Degrade(w, 1f);
            float chance = WeaponConditionSystem.ComputeJamChance(w);
            Assert.True(chance <= 1f && chance >= 0f);
        }

        [Fact]
        public void JamRoll_MarksWeaponJammed()
        {
            var w = Pipe();
            bool jammed = WeaponConditionSystem.TryJammed(w, new StubRng(7, 0.0)); // roll < chance => jam
            Assert.True(jammed);
            Assert.True(w.IsJammed);
            Assert.True(w.JamClearTicksRemaining > 0);
        }

        [Fact]
        public void ClearingJam_TakesTicks()
        {
            var w = Pipe();
            WeaponConditionSystem.TryJammed(w, new StubRng(7, 0.0));
            bool done1 = WeaponConditionSystem.TickJamClear(w, 2);
            Assert.False(done1); // not fully cleared yet (ticks > 2)
            WeaponConditionSystem.TickJamClear(w, 100);
            Assert.False(w.IsJammed, "jam cleared after enough ticks");
        }

        [Fact]
        public void FieldRepair_ConsumesScrap_AndRestores()
        {
            var w = Rifle();
            WeaponConditionSystem.Degrade(w, 0.6f);
            int cost = WeaponConditionSystem.GetScrapRepairCost(w);
            Assert.True(cost >= 1, "repair has a scrap cost");

            int requested = 0;
            bool ok = new WeaponConditionSystem().TryFieldRepair(w, CombatHostPorts.NoOp(),
                (id, n) => { requested = n; return true; });
            Assert.True(ok);
            Assert.Equal(cost, requested);
            Assert.Equal(1f, w.ConditionPct, 3);
            Assert.False(w.IsJammed);
        }

        [Fact]
        public void FieldRepair_FailsWithoutScrap()
        {
            var w = Rifle();
            WeaponConditionSystem.Degrade(w, 0.5f);
            bool ok = new WeaponConditionSystem().TryFieldRepair(w, CombatHostPorts.NoOp(),
                (id, n) => false);
            Assert.False(ok);
            Assert.True(w.ConditionPct < 1f, "failed repair must not restore condition");
        }

        [Fact]
        public void AshDunes_JamAndDegradeFirearm()
        {
            var w = Rifle();
            float before = w.ConditionPct;
            WeaponConditionSystem.ExposeToAsh(w, 1f);
            Assert.True(w.IsJammed);
            Assert.True(w.ConditionPct < before);
            Assert.True(WeaponConditionSystem.ComputeJamChance(w) > 0.5f);
        }

        [Fact]
        public void PipeWithMilitaryAmmo_CanBurst()
        {
            // Military ammo in a pipe weapon risks a burst (Hazard_WeaponBurst parity).
            bool sawBurst = false;
            for (int seed = 1; seed < 80 && !sawBurst; seed++)
            {
                var w = Pipe("ammo_556");
                if (WeaponConditionSystem.TryWeaponBurst(w, new SeededRng(seed)))
                {
                    sawBurst = true;
                    Assert.True(w.ConditionPct < 0.1f, "burst wrecks the weapon");
                }
            }
            Assert.True(sawBurst, "expected a burst across seeds");
        }

        [Fact]
        public void MilitaryAmmoInRifle_DoesNotBurst()
        {
            var w = Rifle("ammo_556"); // not jury-rigged
            bool burst = WeaponConditionSystem.TryWeaponBurst(w, new StubRng(1, 0.0));
            Assert.False(burst);
        }

        [Fact]
        public void ScrapCost_ScalesWithDamage()
        {
            var w = Rifle();
            int pristine = WeaponConditionSystem.GetScrapRepairCost(w);
            WeaponConditionSystem.Degrade(w, 0.9f);
            int ruined = WeaponConditionSystem.GetScrapRepairCost(w);
            Assert.True(ruined >= pristine, "worse condition costs more to repair");
        }
    }
}
