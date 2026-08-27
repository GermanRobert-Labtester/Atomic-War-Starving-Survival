using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;

namespace Ashfall.Core.Tests
{
    public class TacticalCombatSystemTests
    {
        public TacticalCombatSystemTests() => CombatCatalog.SeedDefaults();

        private static List<CombatantState> PlayerRoster(int n = 1)
        {
            var list = new List<CombatantState>();
            for (int i = 0; i < n; i++)
                list.Add(new CombatantState
                {
                    Id = "p" + i,
                    Name = "Survivor " + i,
                    SurvivorId = "sv" + i,
                    IsPlayer = true,
                    Health = 100,
                    MaxHealth = 100
                });
            return list;
        }

        private static List<WeaponInstanceState> RifleWeapons(int n = 1)
        {
            var list = new List<WeaponInstanceState>();
            for (int i = 0; i < n; i++)
                list.Add(new WeaponInstanceState
                {
                    InstanceId = "w" + i,
                    WeaponId = "weapon_assault_rifle",
                    OwnerSurvivorId = "sv" + i,
                    ConditionPct = 0.9f,
                    AmmoId = "ammo_556",
                    AmmoRemaining = 50
                });
            return list;
        }

        private static TacticalCombatSystem Engine(int enemyCount, float enemyHealth, CombatHostPorts ports = null)
        {
            var sys = new TacticalCombatSystem(null, ports ?? CombatHostPorts.NoOp());
            sys.BeginEncounter("enc_t", "exp", "loc", "Loc", 1, 99, PlayerRoster(), RifleWeapons(), enemyCount, enemyHealth);
            return sys;
        }

        [Fact]
        public void BeginEncounter_StartsInPlayerTurn()
        {
            var sys = Engine(2, 40);
            Assert.Equal(CombatPhase.PlayerTurn, (CombatPhase)sys.State.Phase);
            Assert.Equal(3, sys.State.Combatants.Count); // 1 player + 2 enemies
        }

        [Fact]
        public void SetStance_IsSerialized()
        {
            var sys = Engine(1, 40);
            sys.SetStance(TacticalStance.Advance);
            Assert.Equal(TacticalCombatSystem.StanceId(TacticalStance.Advance), sys.State.PlayerStance);
            Assert.True(TacticalCombatSystem.TryParseStance(sys.State.PlayerStance, out var s));
            Assert.Equal(TacticalStance.Advance, s);
        }

        [Fact]
        public void StanceMods_ProduceDistinctTradeoffs()
        {
            var hold = TacticalCombatSystem.GetStanceMods(TacticalStance.HoldPosition);
            var last = TacticalCombatSystem.GetStanceMods(TacticalStance.LastStand);
            Assert.True(last.Accuracy > hold.Accuracy, "last stand grants accuracy");
            Assert.True(last.Damage > hold.Damage, "last stand grants damage");
            Assert.False(last.CanFlee, "last stand cannot flee");
            Assert.True(hold.CanFlee, "hold can flee");
            Assert.True(last.DeathIsInstant, "last stand death is instant");
            // Retreat/advance trade defense against damage.
            Assert.True(TacticalCombatSystem.GetStanceMods(TacticalStance.HoldPosition).Defense
                        > TacticalCombatSystem.GetStanceMods(TacticalStance.Advance).Defense);
        }

        [Fact]
        public void FiringConsumesAmmoAndDegradesWeapon()
        {
            var ports = new CombatHostPorts(null, null, null, consumeAmmo: (id, n) => 1000 - n);
            var sys = Engine(1, 40, ports);
            var shooter = sys.State.Combatants[0];
            var weapon = sys.State.Weapons[0];
            float condBefore = weapon.ConditionPct;
            var res = sys.PlayerFire(FindEnemyId(sys), new SeededRng(5));
            Assert.True(res.Success);
            Assert.True(weapon.ConditionPct <= condBefore, "firing degrades the weapon");
            Assert.True(weapon.ShotsFired > 0, "shots fired recorded");
        }

        [Fact]
        public void Suppression_PinsEnemiesAndStopsTheirFire()
        {
            var sys = Engine(2, 40);
            var res = sys.PlayerSuppress(new SeededRng(6));
            Assert.True(res.Success, res.Message);
            foreach (var c in sys.State.Combatants)
            {
                if (!c.IsPlayer)
                    Assert.True(c.IsPinned, "enemy pinned by suppression");
            }
            // Pinned enemies lose the turn: player health unchanged after EndTurn.
            int hpBefore = (int)sys.State.Combatants[0].Health;
            sys.EndTurn(new SeededRng(6));
            Assert.Equal(hpBefore, (int)sys.State.Combatants[0].Health);
        }

        [Fact]
        public void MoveLane_ChangesPosition()
        {
            var sys = Engine(1, 40);
            var res = sys.PlayerMoveLane("p0", CombatLane.Right, new SeededRng(4));
            Assert.True(res.Success);
            Assert.Equal((int)CombatLane.Right, sys.State.Combatants[0].Lane);
        }

        [Fact]
        public void Retreat_Succeeds_EndsEncounter()
        {
            var sys = Engine(2, 40);
            var res = sys.PlayerRetreat(new StubRng(1, 0.1)); // below mobility 0.75 → clean retreat
            Assert.True(res.Success);
            Assert.True(sys.State.Resolved);
            Assert.Equal(CombatPhase.Retreated, (CombatPhase)sys.State.Phase);
        }

        [Fact]
        public void Retreat_Failure_Injures()
        {
            var sys = Engine(2, 40);
            int hpBefore = (int)sys.State.Combatants[0].Health;
            var res = sys.PlayerRetreat(new StubRng(1, 0.99)); // >= mobility → fail & injury
            Assert.True(res.Success);
            Assert.True(sys.State.Combatants[0].Health < hpBefore, "failed retreat injures the squad");
        }

        [Fact]
        public void LastStand_SetsFlagAndTerminalStance()
        {
            var sys = Engine(2, 40);
            var res = sys.PlayerLastStand("p0", new SeededRng(3));
            Assert.True(res.Success);
            Assert.True(sys.State.Combatants[0].IsLastStand);
            Assert.Equal(TacticalCombatSystem.StanceId(TacticalStance.LastStand), sys.State.PlayerStance);
        }

        [Fact]
        public void DownedPlayer_BleedsOutAndDies()
        {
            var sys = new TacticalCombatSystem(null, CombatHostPorts.NoOp());
            // Two players: one takes the lethal hit, the other keeps the encounter alive.
            sys.BeginEncounter("enc_t", "exp", "loc", "Loc", 1, 99, PlayerRoster(2), RifleWeapons(2), 1, 40);
            sys.State.Combatants[0].Health = 5f; // Yuki low
            sys.EndTurn(new StubRng(1, 0.1)); // enemy hits players[0] at 0.425 acc
            var yuki = sys.State.Combatants[0];
            Assert.True(yuki.IsDowned, "player downed by lethal hit");
            Assert.True(yuki.BleedTurnsRemaining > 0, "downed player begins bleeding out");
            Assert.False(sys.State.Resolved, "teammate keeps the encounter alive");

            sys.EndTurn(new StubRng(1, 0.1)); // bleed-out ticks down on the next round
            Assert.True(yuki.BleedTurnsRemaining < TacticalCombatSystem.DefaultBleedTurns, "bleed-out ticks down");
        }

        [Fact]
        public void Victory_GrantsLootAndMoraleAndSurvivorSurvival()
        {
            int loot = 0, morale = 0, survived = 0, trauma = 0;
            var ports = new CombatHostPorts(
                damageSurvivor: null,
                healSurvivor: null,
                applyMoraleDelta: (id, d) => morale++,
                consumeAmmo: (id, n) => 10000,
                raiseTrauma: (id, k, s) => trauma++,
                grantLoot: l => loot++,
                markCombatSurvived: id => survived++);
            var sys = Engine(1, 10, ports); // fragile enemy
            sys.ResolveToEnd(new SeededRng(42), 60);
            Assert.True(sys.State.Resolved, "encounter resolves");
            Assert.Equal(CombatPhase.Won, (CombatPhase)sys.State.Phase);
            Assert.True(loot > 0, "victory grants loot");
            Assert.True(morale > 0, "victory raises morale");
            Assert.True(survived > 0, "survivor recorded as survived combat");
        }

        [Fact]
        public void EnvironmentalAsh_JamsEquippedWeapons()
        {
            var sys = Engine(1, 40);
            var res = sys.TickEnvironmental(1f, new SeededRng(2));
            Assert.True(res.Success);
            Assert.True(sys.State.Weapons[0].IsJammed, "ash jammed the equipped rifle");
        }

        [Fact]
        public void ClearJam_WorksAfterJam()
        {
            var sys = Engine(1, 40);
            sys.TickEnvironmental(1f, new SeededRng(2));
            Assert.True(sys.State.Weapons[0].IsJammed);
            var res = sys.PlayerClearJam("p0", new SeededRng(2));
            Assert.True(res.Success, res.Message);
            Assert.False(sys.State.Weapons[0].IsJammed, "jam cleared");
        }

        [Fact]
        public void BuildSnapshot_ReflectsState()
        {
            var sys = Engine(2, 40);
            var snap = sys.BuildSnapshot();
            Assert.Equal("enc_t", snap.EncounterId);
            Assert.True(snap.IsActive);
            Assert.True(snap.Combatants.Count >= 3);
            Assert.True(snap.Weapons.Count == 1);
            Assert.Equal("PlayerTurn", snap.Phase);
        }

        private static string FindEnemyId(TacticalCombatSystem sys)
        {
            foreach (var c in sys.State.Combatants)
                if (!c.IsPlayer) return c.Id;
            return null;
        }
    }
}
