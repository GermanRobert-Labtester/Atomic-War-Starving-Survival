using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;

namespace Ashfall.Core.Tests
{
    public class CombatSaveRoundTripTests
    {
        public CombatSaveRoundTripTests() => CombatCatalog.SeedDefaults();

        private static TacticalCombatSystem Engine()
        {
            var sys = new TacticalCombatSystem(null, new CombatHostPorts { ConsumeAmmo = (id, n) => 5000 });
            var players = new List<CombatantState>
            {
                new CombatantState { Id = "p1", Name = "Yuki", SurvivorId = "sv1", IsPlayer = true, Health = 100, MaxHealth = 100 }
            };
            var weapons = new List<WeaponInstanceState>
            {
                new WeaponInstanceState { InstanceId = "w1", WeaponId = "weapon_assault_rifle", OwnerSurvivorId = "sv1", ConditionPct = 0.9f, AmmoId = "ammo_556", AmmoRemaining = 60 }
            };
            sys.BeginEncounter("enc_s", "exp", "loc", "Loc", 7, 1234, players, weapons, 2, 40);
            return sys;
        }

        [Fact]
        public void CaptureRestore_PreservesFullState()
        {
            var sys = Engine();
            sys.SetStance(TacticalStance.Advance);
            sys.PlayerFire("enemy_enc_s_0", new SeededRng(10));
            sys.EndTurn(new SeededRng(10));

            var save = sys.CaptureState();
            Assert.Equal(3, save.Combatants.Count);

            var restored = new TacticalCombatSystem();
            restored.RestoreState(save);

            Assert.Equal(sys.State.EncounterId, restored.State.EncounterId);
            Assert.Equal(sys.State.Turn, restored.State.Turn);
            Assert.Equal(sys.State.PlayerStance, restored.State.PlayerStance);
            Assert.Equal(sys.State.Events.Count, restored.State.Events.Count);
            Assert.Equal(sys.State.Weapons[0].ConditionPct, restored.State.Weapons[0].ConditionPct, 3);
            Assert.Equal(sys.State.Combatants[0].Health, restored.State.Combatants[0].Health, 3);
        }

        [Fact]
        public void JsonRoundTrip_ThroughPort_PreservesState()
        {
            var sys = Engine();
            sys.PlayerFire("enemy_enc_s_0", new SeededRng(10));

            var json = new SystemTextJsonSerializer();
            var blob = json.Serialize(sys.CaptureState());
            var loaded = json.Deserialize<CombatState>(blob);

            var restored = new TacticalCombatSystem();
            restored.RestoreState(loaded);

            Assert.Equal(sys.State.Turn, restored.State.Turn);
            Assert.Equal(sys.State.Events.Count, restored.State.Events.Count);
            Assert.Equal(sys.State.Weapons[0].AmmoRemaining, restored.State.Weapons[0].AmmoRemaining);
        }

        [Fact]
        public void Migrate_ClampsAndDefaultsForeignSaves()
        {
            var legacy = new CombatState
            {
                SaveVersion = 1,
                Phase = 99,                 // out-of-range
                PlayerStance = null,
                Events = null,
                Combatants = new List<CombatantState>
                {
                    new CombatantState { Id = "l1", Name = "Legacy", IsPlayer = true, Lane = 7 }
                }
            };
            var migrated = TacticalCombatSystem.Migrate(legacy);
            Assert.Equal(CombatState.CurrentSaveVersion, migrated.SaveVersion);
            Assert.True(migrated.Phase >= (int)CombatPhase.Setup && migrated.Phase <= (int)CombatPhase.Retreated,
                "out-of-range phase clamped");
            Assert.Equal(TacticalCombatSystem.StanceId(TacticalStance.HoldPosition), migrated.PlayerStance);
            Assert.True(migrated.Combatants[0].Lane <= 2, "out-of-range lane clamped");
            Assert.NotNull(migrated.Events);
            Assert.Equal(1, migrated.Combatants.Count);
        }

        [Fact]
        public void DeterministicReplay_FromSameStateAndSeed()
        {
            var a = Engine(); DoReplay(a);
            var b = Engine(); DoReplay(b);

            var ea = EventDetails(a);
            var eb = EventDetails(b);
            Assert.Equal(ea.Count, eb.Count);
            for (int i = 0; i < ea.Count; i++)
                Assert.Equal(ea[i], eb[i]);
        }

        private static void DoReplay(TacticalCombatSystem sys)
        {
            var rng = new SeededRng(999);
            int guard = 0;
            while (!sys.State.Resolved && guard++ < 50)
            {
                sys.PlayerFire("enemy_enc_s_0", rng);
                if (!sys.State.Resolved) sys.EndTurn(new SeededRng(999));
            }
        }

        private static List<string> EventDetails(TacticalCombatSystem sys)
        {
            var list = new List<string>();
            foreach (var e in sys.State.Events) list.Add(e.Detail);
            return list;
        }
    }
}
