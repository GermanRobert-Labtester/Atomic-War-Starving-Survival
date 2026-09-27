// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    /// <summary>
    /// Host-integration tests for the ration conflict authority. The rationing
    /// owner allocates; NeedsSystem owns morale; SurvivorRelationsSystem owns pair
    /// affinity. This suite proves the boundaries and the deterministic escalation.
    /// </summary>
    public sealed class PlanRationConflictHostIntegrationTests
    {
        private static RationConflictSystem BuildEngine(int seed = 7, params string[] survivors)
        {
            var engine = new RationConflictSystem(new SeededRng(seed));
            foreach (string id in survivors) engine.RegisterSurvivor(id);
            return engine;
        }

        [Fact]
        public void EqualAllocations_ReadAsPerfectlyFair()
        {
            var engine = BuildEngine(7, "a", "b");
            engine.SetAllocation("a", 0.5f);
            engine.SetAllocation("b", 0.5f);

            engine.Tick("a", 24f);

            var state = engine.GetState("a");
            Assert.NotNull(state);
            Assert.True(state!.perceivedFairness > 0.99f);
            Assert.Equal(0f, state.resentmentLevel);
        }

        [Fact]
        public void ADeficitBeyondTheThreshold_BuildsResentmentAndNamesATarget()
        {
            var engine = BuildEngine(7, "a", "b", "c");
            engine.SetAllocation("a", 0.2f);
            engine.SetAllocation("b", 0.9f);
            engine.SetAllocation("c", 0.9f);
            string? target = null;
            engine.OnResentmentBuilt += (_, t, _) => target = t;

            engine.Tick("a", 24f);

            var state = engine.GetState("a");
            Assert.NotNull(target);
            Assert.True(state!.resentmentLevel > 0f);
            Assert.Equal(target, state.resentmentTargetId);
        }

        [Fact]
        public void Resentment_DecaysWhileAllocationsReadAsFair()
        {
            var engine = BuildEngine(7, "a", "b", "c");
            engine.SetAllocation("a", 0.2f);
            engine.SetAllocation("b", 0.9f);
            engine.SetAllocation("c", 0.9f);
            engine.Tick("a", 24f);
            float built = engine.GetState("a")!.resentmentLevel;

            engine.SetAllocation("a", 0.95f);
            engine.SetAllocation("b", 0.9f);
            engine.SetAllocation("c", 0.9f);
            for (int i = 0; i < 10; i++) engine.Tick("a", 24f);

            Assert.True(engine.GetState("a")!.resentmentLevel < built);
        }

        [Fact]
        public void Escalation_AppliesMoraleThroughTheCanonicalNeedsOwner()
        {
            var engine = BuildEngine(7, "a", "b", "c");
            var needs = new NeedsSystem();
            var a = new SurvivorNeedsState { Id = "a" };
            var b = new SurvivorNeedsState { Id = "b" };
            needs.Register(a);
            needs.Register(b);

            int moraleEvents = 0;
            engine.OnMoraleDelta += (id, delta, source) =>
            {
                moraleEvents++;
                needs.Modify(id, NeedKind.Morale, delta);
                Assert.False(string.IsNullOrEmpty(source));
            };

            engine.SetAllocation("a", 0.05f);
            engine.SetAllocation("b", 1.0f);
            engine.SetAllocation("c", 1.0f);
            float before = a.Morale;
            bool confrontation = false;
            engine.OnRationConfrontation += (_, _) => confrontation = true;
            for (int i = 0; i < 30 && !confrontation; i++) engine.Tick("a", 24f);

            Assert.True(confrontation);
            Assert.True(moraleEvents >= 1);
            Assert.True(a.Morale < before, "morale must drop exactly through NeedsSystem");
        }

        [Fact]
        public void TheResentmentTarget_IsRecordedThroughTheSingleAffinityOwner()
        {
            var engine = BuildEngine(7, "a", "b", "c");
            var relations = new SurvivorRelationsSystem(new SeededRng(1986));
            engine.OnRationConfrontation += (resenterId, targetId) =>
                relations.ModifyAffinity(resenterId, targetId, -4f);

            engine.SetAllocation("a", 0.05f);
            engine.SetAllocation("b", 1.0f);
            engine.SetAllocation("c", 1.0f);
            bool confrontation = false;
            engine.OnRationConfrontation += (_, _) => confrontation = true;
            for (int i = 0; i < 30 && !confrontation; i++) engine.Tick("a", 24f);

            Assert.True(confrontation);
            Assert.True(relations.TryGetRelationship("a", "b", out var entry));
            Assert.NotNull(entry);
            Assert.True(entry!.affinity < 0f);
        }

        [Fact]
        public void TheTheftRoll_IsDeterministicForAFixedSeed()
        {
            bool FirstRunStole()
            {
                var engine = new RationConflictSystem(new SeededRng(4242));
                engine.RegisterSurvivor("t");
                engine.RegisterSurvivor("v");
                engine.SetAllocation("t", 0.0f);
                engine.SetAllocation("v", 1.0f);
                bool stole = false;
                engine.OnRationsStolen += (_, _) => stole = true;
                for (int i = 0; i < 40 && !stole; i++) engine.Tick("t", 24f);
                return stole;
            }

            bool first = FirstRunStole();
            for (int run = 0; run < 3; run++)
                Assert.Equal(first, FirstRunStole());
        }

        [Fact]
        public void CaptureAndRestore_RoundTripsResentmentState()
        {
            var engine = BuildEngine(7, "a", "b", "c");
            engine.SetAllocation("a", 0.2f);
            engine.SetAllocation("b", 0.9f);
            engine.SetAllocation("c", 0.9f);
            engine.Tick("a", 24f);

            var captured = engine.CaptureState();
            var reloaded = BuildEngine(1, "a", "b", "c");
            reloaded.RestoreState(captured);

            Assert.Equal(engine.GetState("a")!.resentmentLevel, reloaded.GetState("a")!.resentmentLevel);
            Assert.Equal(engine.GetState("a")!.resentmentTargetId, reloaded.GetState("a")!.resentmentTargetId);
        }

        [Fact]
        public void UnregisteredSurvivors_AreIgnored()
        {
            var engine = BuildEngine(7, "a");

            engine.Tick("not_registered", 24f);

            Assert.Null(engine.GetState("not_registered"));
        }

        [Fact]
        public void TheAuthoredThresholds_StayEngineOwned()
        {
            Assert.Equal(0.20f, RationConflictSystem.FairnessDeviationThreshold);
            Assert.Equal(0.10f, RationConflictSystem.ResentmentGainPerDay);
            Assert.Equal(0.03f, RationConflictSystem.ResentmentDecayPerDay);
            Assert.Equal(0.70f, RationConflictSystem.ConfrontationThreshold);
            Assert.Equal(0.85f, RationConflictSystem.TheftThreshold);
            Assert.Equal(-10f, RationConflictSystem.ConfrontationMoraleHit);
            Assert.Equal(-15f, RationConflictSystem.TheftMoraleHit);
        }
    }
}
