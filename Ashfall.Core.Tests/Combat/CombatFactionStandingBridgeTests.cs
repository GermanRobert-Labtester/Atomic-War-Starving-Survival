// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Combat;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests.Combat
{
    public sealed class CombatFactionStandingBridgeTests
    {
        [Fact]
        public void EvaluateConsequences_FactionlessCombatants_ProducesNoConsequences()
        {
            var state = State(
                new CombatantState { Id = "mutant", FactionId = "faction_unaligned", Health = 0f },
                new CombatantState { Id = "bandit", FactionId = string.Empty, Health = 0f });

            Assert.Empty(CombatFactionStandingBridge.EvaluateConsequences(state));
        }

        [Fact]
        public void EvaluateConsequences_EnemyCasualties_AggregatesBoundedPenalty()
        {
            var state = State(
                new CombatantState { Id = "g1", FactionId = "iron_garrison", Health = 0f },
                new CombatantState { Id = "g2", FactionId = "iron_garrison", Health = 10f, IsDowned = true });

            var consequence = Assert.Single(CombatFactionStandingBridge.EvaluateConsequences(state));

            Assert.Equal(1, consequence.Kills);
            Assert.Equal(1, consequence.Downed);
            Assert.Equal(-7.5f, consequence.StandingDelta);
            Assert.Equal("combat_aggression", consequence.Reason);
        }

        [Fact]
        public void EvaluateConsequences_SelfDefense_HalvesPenalty()
        {
            var state = State(new CombatantState
            {
                Id = "attacker",
                FactionId = "faction_rebel",
                Health = 0f
            });

            var consequence = Assert.Single(
                CombatFactionStandingBridge.EvaluateConsequences(state, isSelfDefense: true));

            Assert.True(consequence.IsSelfDefense);
            Assert.Equal(-2.5f, consequence.StandingDelta);
            Assert.Equal("combat_self_defense", consequence.Reason);
        }

        [Fact]
        public void EvaluateConsequences_ExcessiveKills_RespectsPenaltyCap()
        {
            var combatants = Enumerable.Range(0, 10)
                .Select(i => new CombatantState
                {
                    Id = "enemy_" + i,
                    FactionId = "iron_garrison",
                    Health = 0f
                })
                .ToArray();

            var consequence = Assert.Single(
                CombatFactionStandingBridge.EvaluateConsequences(State(combatants)));

            Assert.Equal(CombatFactionStandingBridge.MaxCasualtyPenalty, consequence.StandingDelta);
        }

        [Fact]
        public void EvaluateConsequences_VictoryWithAlliedFaction_GrantsAssistance()
        {
            var state = State(
                new CombatantState { Id = "ally", IsPlayer = true, FactionId = "faction_prpf", Health = 100f },
                new CombatantState { Id = "enemy", FactionId = "iron_garrison", Health = 0f });
            state.Phase = (int)CombatPhase.Won;
            state.OutcomeText = "Victory";

            var consequences = CombatFactionStandingBridge.EvaluateConsequences(state);

            Assert.Equal(2, consequences.Count);
            Assert.Equal(CombatFactionStandingBridge.AssistanceBonus,
                consequences.Single(c => c.FactionId == "faction_prpf").StandingDelta);
        }

        [Fact]
        public void EvaluateConsequences_IsDeterministicallyOrdered()
        {
            var state = State(
                new CombatantState { Id = "z", FactionId = "faction_z", Health = 0f },
                new CombatantState { Id = "a", FactionId = "faction_a", Health = 0f });

            var ids = CombatFactionStandingBridge.EvaluateConsequences(state)
                .Select(c => c.IncidentId)
                .ToArray();

            Assert.Equal(ids.OrderBy(id => id, System.StringComparer.Ordinal), ids);
        }

        [Fact]
        public void ApplyConsequences_UsesOneFactionAuthorityExactlyOnce()
        {
            var bridge = new CombatFactionStandingBridge();
            var factionWar = new FactionWarSystem();
            var markers = new List<string>();
            var consequences = CombatFactionStandingBridge.EvaluateConsequences(State(
                new CombatantState { Id = "g1", FactionId = "iron_garrison", Health = 0f },
                new CombatantState { Id = "g2", FactionId = "iron_garrison", Health = 10f, IsDowned = true }));

            int first = bridge.ApplyConsequences(consequences, factionWar.ModifyStanding, markers);
            int second = bridge.ApplyConsequences(consequences, factionWar.ModifyStanding, markers);

            Assert.Equal(1, first);
            Assert.Equal(0, second);
            Assert.Equal(-8, factionWar.GetStanding("iron_garrison"));
            Assert.Equal(-8f, consequences[0].StandingDelta);
            Assert.Single(markers);
        }

        [Fact]
        public void ApplyConsequences_MissingSink_FailsClosedWithoutMarkingIncident()
        {
            var markers = new List<string>();
            var consequences = CombatFactionStandingBridge.EvaluateConsequences(State(
                new CombatantState { Id = "g1", FactionId = "iron_garrison", Health = 0f }));

            int count = new CombatFactionStandingBridge().ApplyConsequences(consequences, null, markers);

            Assert.Equal(0, count);
            Assert.Empty(markers);
            Assert.False(consequences[0].Applied);
        }

        [Fact]
        public void CombatStateMigration_PreservesAppliedMarkersAndIncidentHistory()
        {
            var state = State(new CombatantState { Id = "g1", FactionId = "iron_garrison", Health = 0f });
            state.AppliedFactionConsequenceIds.Add("ccon_enc_test_iron_garrison_casualty");
            state.FactionConsequences.Add(new CombatFactionConsequence
            {
                IncidentId = "ccon_enc_test_iron_garrison_casualty",
                EncounterId = "enc_test",
                FactionId = "iron_garrison",
                StandingDelta = -5f,
                Applied = true
            });

            var migrated = TacticalCombatSystem.Migrate(state);

            Assert.Equal(CombatState.CurrentSaveVersion, migrated.SaveVersion);
            Assert.Single(migrated.AppliedFactionConsequenceIds);
            Assert.Single(migrated.FactionConsequences);
            Assert.True(migrated.FactionConsequences[0].Applied);
        }

        private static CombatState State(params CombatantState[] combatants)
        {
            return new CombatState
            {
                EncounterId = "enc_test",
                Day = 12,
                Resolved = true,
                Combatants = new List<CombatantState>(combatants)
            };
        }
    }
}
