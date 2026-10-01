// SPDX-License-Identifier: MIT
// W4 — cross-encounter war ledger: every resolved encounter appends one
// entry, the ledger survives the per-encounter StartCombat reset, and it
// round-trips through the existing CombatState save seam (legacy saves
// restore to an empty ledger).
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;

namespace Ashfall.Core.Tests.Combat
{
    public class CombatEncounterLedgerTests
    {
        public CombatEncounterLedgerTests()
        {
            CombatCatalog.SeedDefaults();
            CombatArenaCatalog.SeedDefaults();
        }

        private static TacticalCombatSystem BeginFight(string encounterId, int seed = 42, float enemyHealth = 1f)
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
                    AmmoId = "ammo_556", AmmoRemaining = 200
                }
            };
            Assert.True(sys.BeginEncounter(
                encounterId, "exp_ledger", "loc_ledger", "Ledger Test Yard", 3, seed,
                roster, weapons, enemyCount: 1, enemyHealth: enemyHealth));
            return sys;
        }

        private static void FightToResolution(TacticalCombatSystem sys, int seed = 42)
        {
            var rng = new SeededRng(seed);
            string targetId = sys.State.Combatants.First(c => !c.IsPlayer).Id;
            for (int i = 0; i < 60 && !sys.State.Resolved; i++)
            {
                sys.PlayerFire(targetId, rng);
                if (sys.State.Resolved) break;
                sys.EndTurn(rng);
                targetId = sys.State.Combatants.FirstOrDefault(c => !c.IsPlayer && !c.IsDowned)?.Id
                    ?? sys.State.Combatants.First(c => !c.IsPlayer).Id;
            }
        }

        [Fact]
        public void ResolvedEncounter_AppendsOneLedgerEntry()
        {
            var sys = BeginFight("enc_ledger_1");
            Assert.Empty(sys.State.EncounterHistory);
            FightToResolution(sys);

            Assert.True(sys.State.Resolved, "test fight must resolve for the ledger to record it");
            var ledger = sys.State.EncounterHistory;
            Assert.Single(ledger);
            Assert.Equal(3, ledger[0].Day);
            Assert.Equal("Ledger Test Yard", ledger[0].LocationName);
            Assert.Equal("Hostiles neutralized.", ledger[0].OutcomeText);
            Assert.True(ledger[0].RoundNumber >= 1);
        }

        [Fact]
        public void Ledger_SurvivesNewEncounterReset()
        {
            var sys = BeginFight("enc_ledger_2");
            FightToResolution(sys);
            Assert.Single(sys.State.EncounterHistory);

            // Next fight on the same engine: fresh events, ledger carried.
            Assert.True(sys.BeginEncounter(
                "enc_ledger_2b", "exp_ledger", "loc_ledger", "Second Yard", 5, 7,
                new List<CombatantState>
                {
                    new CombatantState
                    {
                        Id = "p0", Name = "Scout", SurvivorId = "survivor_0",
                        IsPlayer = true, Health = 100, MaxHealth = 100, Lane = 1
                    }
                },
                new List<WeaponInstanceState>
                {
                    new WeaponInstanceState
                    {
                        InstanceId = "w0", WeaponId = "weapon_assault_rifle",
                        OwnerSurvivorId = "survivor_0", ConditionPct = 0.9f,
                        AmmoId = "ammo_556", AmmoRemaining = 200
                    }
                },
                enemyCount: 1, enemyHealth: 1f));

            // Per-encounter events reset with the new state — only the fresh
            // encounter_start line is present; the ledger is what carries over.
            var startEvent = Assert.Single(sys.State.Events);
            Assert.Equal("encounter_start", startEvent.Kind);
            Assert.Equal("enc_ledger_2b", startEvent.SubjectId);
            Assert.Single(sys.State.EncounterHistory);
            Assert.Equal("Ledger Test Yard", sys.State.EncounterHistory[0].LocationName);

            FightToResolution(sys, seed: 7);
            Assert.Equal(2, sys.State.EncounterHistory.Count);
            Assert.Equal("Second Yard", sys.State.EncounterHistory[1].LocationName);
        }

        [Fact]
        public void Ledger_RoundTripsThroughSaveSeam_AndLegacyRestoresEmpty()
        {
            var sys = BeginFight("enc_ledger_3");
            FightToResolution(sys);
            Assert.Single(sys.State.EncounterHistory);

            var saved = sys.CaptureState();
            Assert.Single(saved.EncounterHistory);

            var restored = new TacticalCombatSystem();
            restored.RestoreState(saved);
            Assert.Single(restored.State.EncounterHistory);
            Assert.Equal("Hostiles neutralized.", restored.State.EncounterHistory[0].OutcomeText);

            // A legacy save predating the ledger field deserializes without it
            // and must restore to an empty (never null) ledger.
            var legacy = sys.CaptureState();
            legacy.EncounterHistory = null!;
            var legacyRestored = new TacticalCombatSystem();
            legacyRestored.RestoreState(legacy);
            Assert.NotNull(legacyRestored.State.EncounterHistory);
            Assert.Empty(legacyRestored.State.EncounterHistory);
        }

        [Fact]
        public void Snapshot_ExposesFormattedWarHistory()
        {
            var sys = BeginFight("enc_ledger_4");
            FightToResolution(sys);
            var snap = sys.BuildSnapshot();
            Assert.Single(snap.History);
            Assert.Contains("Ledger Test Yard", snap.History[0]);
            Assert.Contains("Hostiles neutralized.", snap.History[0]);
        }
    }
}
