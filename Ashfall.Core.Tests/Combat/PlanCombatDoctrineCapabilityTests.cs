// SPDX-License-Identifier: MIT
// Focused Core contract tests — Package H / Combat Doctrine Capability.
//
// Pinned behaviour: TacticalCombatSystem already ADDS DoctrineCapability
// .AccuracyBonus to every shot and .TacticalMobilityBonus to every mobility
// check, and ResearchSystem is the only knowledge authority. These tests prove
// the projection between them is real, composable, revocable and idempotent.
//
// Run: scripts/run_test.sh PlanCombatDoctrineCapability

using System;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Research;
using Xunit;
using Xunit.Abstractions;

namespace Ashfall.Core.Tests.Combat
{
    public class PlanCombatDoctrineCapabilityTests
    {
        private const string CombatTraining = "knowledge_combat_training";
        private const string FortifiedChokepoints = "knowledge_fortified_chokepoints";

        private readonly ITestOutputHelper _out;
        public PlanCombatDoctrineCapabilityTests(ITestOutputHelper output) => _out = output;

        private static CombatDoctrineCapability Project(ResearchSystem? research, TacticalCombatSystem engine)
        {
            // The Core factory is a pure projection over a knowledge query delegate.
            // ResearchSystem.HasCapability is the only query the host is allowed to use.
            var capability = CombatDoctrineCapability.FromResearch(
                research == null ? (Func<string, bool>?)null : research.HasCapability);
            engine.DoctrineCapability = capability;
            return capability;
        }

        private static ResearchSystem ResearchWith(params string[] unlocked)
        {
            var research = new ResearchSystem();
            foreach (var id in unlocked) research.State.unlockedIds.Add(id);
            return research;
        }

        [Fact]
        public void Unbound_EngineCarriesInertDefault()
        {
            var engine = new TacticalCombatSystem();

            Assert.NotNull(engine.DoctrineCapability);
            Assert.False(engine.DoctrineCapability.HasCombatTraining);
            Assert.False(engine.DoctrineCapability.HasFortifiedChokepoints);
            Assert.Equal(0f, engine.DoctrineCapability.AccuracyBonus);
            Assert.Equal(0f, engine.DoctrineCapability.TacticalMobilityBonus);
            _out.WriteLine("unbound doctrine contributes exactly zero (the pre-integration state)");
        }

        [Fact]
        public void ResearchIsTheOnlyAuthority_ProjectionFollowsHasCapability()
        {
            var research = ResearchWith(CombatTraining);
            var engine = new TacticalCombatSystem();

            Assert.True(research.HasCapability(CombatTraining));
            Project(research, engine);
            Assert.True(engine.DoctrineCapability.HasCombatTraining);

            // Revoking the knowledge must revoke the combat bonus on the next
            // projection — combat keeps no local copy of its own.
            research.State.unlockedIds.Clear();
            Assert.False(research.HasCapability(CombatTraining));
            Project(research, engine);
            Assert.False(engine.DoctrineCapability.HasCombatTraining);
            Assert.Equal(0f, engine.DoctrineCapability.AccuracyBonus, 4);
        }

        [Fact]
        public void Projection_IsIdempotentNeverAdditive()
        {
            var research = ResearchWith(CombatTraining, FortifiedChokepoints);
            var engine = new TacticalCombatSystem();

            var first = Project(research, engine);
            for (int i = 0; i < 8; i++) Project(research, engine);
            var latest = engine.DoctrineCapability;

            Assert.Equal(first.AccuracyBonus, latest.AccuracyBonus, 4);
            Assert.Equal(first.BarrierIntegrityBonus, latest.BarrierIntegrityBonus, 4);
            Assert.Equal(first.RecoilMitigation, latest.RecoilMitigation, 4);
        }

        [Fact]
        public void HostProjection_UsesResearchCapabilityQuery_NotACopyOfIds()
        {
            // The seam under test is the host query delegate: the engine must end up
            // holding exactly what ResearchSystem.HasCapability reports right now.
            var research = ResearchWith(CombatTraining);
            var engine = new TacticalCombatSystem();

            Project(research, engine);
            Assert.Equal(research.HasCapability(CombatTraining), engine.DoctrineCapability.HasCombatTraining);
            Assert.True(engine.DoctrineCapability.AccuracyBonus > 0f,
                "Actions.cs shot resolution adds DoctrineCapability.AccuracyBonus.");
            Assert.True(engine.DoctrineCapability.TacticalMobilityBonus > 0f,
                "Actions.cs mobility resolution adds DoctrineCapability.TacticalMobilityBonus.");
        }

        [Fact]
        public void UnknownKnowledgeIds_DoNotSilentlyGrantDoctrine()
        {
            var research = ResearchWith("knowledge_not_in_any_catalog");
            var engine = new TacticalCombatSystem();

            var capability = Project(research, engine);
            Assert.False(capability.HasCombatTraining);
            Assert.False(capability.HasFortifiedChokepoints);
            Assert.Equal(0f, capability.AccuracyBonus, 4);
        }
    }
}
