using System;
using System.Collections.Generic;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 25 Seam S4: flag/standing extensions on the faction-war chain runner —
    /// FlagTrigger, stage/choice requires_flag gating, produces_flag, and the
    /// host-owned standing-delta sink. Additive only: the 45 authored pre-Plan-25
    /// stages use none of these fields and their behavior is pinned elsewhere.
    /// </summary>
    public class FactionWarFlagExtensionTests
    {
        private static FactionWarEventChain Chain(string chainId, params FactionWarEventStage[] stages)
        {
            var chain = new FactionWarEventChain { chainId = chainId, band = "cold_war", title = chainId };
            chain.stages.AddRange(stages);
            return chain;
        }

        private static FactionWarEventStage Stage(
            string stageId, int minDay, string requiresFlag = "", string producesFlag = "",
            params FactionWarEventChoice[] choices)
        {
            var stage = new FactionWarEventStage
            {
                stageId = stageId,
                minDay = minDay,
                triggerCondition = "authored prose",
                title = stageId,
                bodyText = "body",
                requiresFlag = requiresFlag,
                producesFlag = producesFlag
            };
            stage.choices.AddRange(choices);
            return stage;
        }

        private static FactionWarEventChoice Choice(
            string choiceId, string leadsTo = "", int morale = 0,
            string requiresFlag = "", string producesFlag = "",
            string standingFaction = "", int standingDelta = 0)
        {
            return new FactionWarEventChoice
            {
                choiceId = choiceId,
                text = choiceId,
                moraleDelta = morale,
                leadsToStageId = leadsTo,
                requiresFlag = requiresFlag,
                producesFlag = producesFlag,
                standingFactionId = standingFaction,
                standingDelta = standingDelta
            };
        }

        private static FactionWarChainRunner RunnerWith(
            FactionWarContentCatalog catalog,
            out FactionWarChainRunner runner,
            List<string>? producedFlags = null)
        {
            var state = new FactionWarChainRunnerState();
            if (producedFlags != null) state.producedFlags = producedFlags;
            runner = new FactionWarChainRunner(catalog, state);
            return runner;
        }

        // ── FlagTrigger ────────────────────────────────────────────────

        [Fact]
        public void FlagTrigger_FiresOnlyWhileTheFlagIsKnown()
        {
            var ctx = new FactionWarTriggerContext();
            var trigger = new FlagTrigger("flag_grievance_test");

            Assert.False(trigger.IsSatisfied(ctx)); // default context: never set

            var catalog = new FactionWarContentCatalog();
            catalog.AddEventChain(Chain("c1", Stage("c1_s1", 100)));
            RunnerWith(catalog, out var runner);
            ctx.IsFlagSet = runner.IsFlagSet;
            Assert.False(trigger.IsSatisfied(ctx));

            runner.ProduceFlagForTest("flag_grievance_test");
            Assert.True(trigger.IsSatisfied(ctx));
        }

        [Fact]
        public void ExternalFlagProbe_IsConsulted()
        {
            var catalog = new FactionWarContentCatalog();
            catalog.AddEventChain(Chain("c1", Stage("c1_s1", 100)));
            RunnerWith(catalog, out var runner);
            runner.ExternalFlagProbe = flag => flag == "flag_external";

            Assert.True(runner.IsFlagSet("flag_external"));
            Assert.False(runner.IsFlagSet("flag_other"));
        }

        // ── stage gating and production ────────────────────────────────

        [Fact]
        public void StageRequiresFlag_HoldsSurfacingUntilProduced()
        {
            var catalog = new FactionWarContentCatalog();
            catalog.AddEventChain(Chain("evt_test_grievance", Stage("evt_test_grievance_s1", 100,
                requiresFlag: "flag_grievance_gate")));
            RunnerWith(catalog, out var runner);

            Assert.Null(runner.GetSurfacedStage("evt_test_grievance", 150));

            runner.ExternalFlagProbe = flag => flag == "flag_grievance_gate";
            Assert.NotNull(runner.GetSurfacedStage("evt_test_grievance", 150));
        }

        [Fact]
        public void StageProducesFlag_LandsInStateAndSurvivesRoundTrip()
        {
            var catalog = new FactionWarContentCatalog();
            catalog.AddEventChain(Chain("evt_test_producer", Stage("evt_test_producer_s1", 100,
                producesFlag: "flag_produced_by_stage",
                choices: Choice("accept"))));
            RunnerWith(catalog, out var runner);

            runner.TickDay(100); // one choice → surfaced, not auto-advanced
            var surfaced = runner.GetSurfacedStage("evt_test_producer", 100);
            Assert.NotNull(surfaced);
            runner.ResolveChoice("evt_test_producer", "evt_test_producer_s1", "accept", 100);

            Assert.True(runner.IsFlagSet("flag_produced_by_stage"));

            var restored = new FactionWarChainRunner(catalog);
            restored.RestoreState(runner.CaptureState());
            Assert.True(restored.IsFlagSet("flag_produced_by_stage"));
        }

        [Fact]
        public void ZeroChoiceStage_AutoAdvancesAndProduces()
        {
            var catalog = new FactionWarContentCatalog();
            catalog.AddEventChain(Chain("evt_test_auto", Stage("evt_test_auto_s1", 100,
                producesFlag: "flag_auto_produced")));
            RunnerWith(catalog, out var runner);

            runner.TickDay(100);
            Assert.True(runner.IsChainResolved("evt_test_auto"));
            Assert.True(runner.IsFlagSet("flag_auto_produced"));
        }

        // ── choice gating and effects ──────────────────────────────────

        [Fact]
        public void ChoiceRequiresFlag_HidesChoiceUntilSetAndRefusesSpeculativeResolve()
        {
            var catalog = new FactionWarContentCatalog();
            catalog.AddEventChain(Chain("evt_test_choicegate",
                Stage("evt_test_choicegate_s1", 100,
                    choices: new[]
                    {
                        Choice("mediator", requiresFlag: "flag_coalition_invited", leadsTo: "evt_test_choicegate_s2"),
                        Choice("refuse")
                    }),
                Stage("evt_test_choicegate_s2", 120)));
            RunnerWith(catalog, out var runner);

            var surfaced = runner.GetSurfacedStage("evt_test_choicegate", 100);
            Assert.NotNull(surfaced);
            var mediator = surfaced!.choices[0];
            var refuse = surfaced.choices[1];

            Assert.False(runner.IsChoiceAvailable(surfaced, mediator, 100));
            Assert.True(runner.IsChoiceAvailable(surfaced, refuse, 100));
            Assert.Throws<InvalidOperationException>(() =>
                runner.ResolveChoice("evt_test_choicegate", "evt_test_choicegate_s1", "mediator", 100));

            runner.ProduceFlagForTest("flag_coalition_invited");
            Assert.True(runner.IsChoiceAvailable(surfaced, mediator, 100));
            runner.ResolveChoice("evt_test_choicegate", "evt_test_choicegate_s1", "mediator", 100);
            Assert.True(runner.IsFlagSet("flag_coalition_invited")); // still set (produced externally)
        }

        [Fact]
        public void ChoiceProducesFlagAndStandingDelta_AreApplied()
        {
            var catalog = new FactionWarContentCatalog();
            catalog.AddEventChain(Chain("evt_test_effects", Stage("evt_test_effects_s1", 100,
                choices: Choice("back_faction", producesFlag: "flag_escalation_backed",
                    standingFaction: "faction_central_garrison", standingDelta: -5))));
            RunnerWith(catalog, out var runner);

            string? standingFaction = null;
            int standingDelta = 0;
            runner.StandingDeltaApplier = (factionId, delta) => { standingFaction = factionId; standingDelta = delta; };

            runner.ResolveChoice("evt_test_effects", "evt_test_effects_s1", "back_faction", 100);
            Assert.True(runner.IsFlagSet("flag_escalation_backed"));
            Assert.Equal("faction_central_garrison", standingFaction);
            Assert.Equal(-5, standingDelta);
        }

        [Fact]
        public void ZeroStandingDeltaOrEmptyFaction_NeverTouchesTheApplier()
        {
            var catalog = new FactionWarContentCatalog();
            catalog.AddEventChain(Chain("evt_test_nofx", Stage("evt_test_nofx_s1", 100,
                choices: Choice("plain", standingFaction: "", standingDelta: 0))));
            RunnerWith(catalog, out var runner);

            int calls = 0;
            runner.StandingDeltaApplier = (_, _) => calls++;
            runner.ResolveChoice("evt_test_nofx", "evt_test_nofx_s1", "plain", 100);
            Assert.Equal(0, calls);
        }

        // ── back-compat ────────────────────────────────────────────────

        [Fact]
        public void OldSaveWithoutProducedFlags_RestoresClean()
        {
            var catalog = new FactionWarContentCatalog();
            catalog.AddEventChain(Chain("evt_old", Stage("evt_old_s1", 100)));
            RunnerWith(catalog, out var runner);

            var legacy = runner.CaptureState();
            legacy.producedFlags = null; // simulate a pre-Plan-25 payload
            var restored = new FactionWarChainRunner(catalog);
            restored.RestoreState(legacy);
            Assert.False(restored.IsFlagSet("anything"));
            Assert.NotNull(restored.State.producedFlags);
        }

        [Fact]
        public void AuthoredPrePlan25Stage_StillSurfacesUnchanged()
        {
            var catalog = new FactionWarContentCatalog();
            catalog.AddEventChain(Chain("evt_d480_grain_tally_dispute",
                Stage("evt_d480_grain_tally_dispute_s1", 480,
                    choices: Choice("side_garrison", morale: -2))));
            RunnerWith(catalog, out var runner);

            // minDay gate only (table entry defaults to Always for unknown ids in tests)
            Assert.NotNull(runner.GetSurfacedStage("evt_d480_grain_tally_dispute", 480));
            Assert.Null(runner.GetSurfacedStage("evt_d480_grain_tally_dispute", 479));
        }
    }

    /// <summary>Test seam: test-visible alias for the runner's private flag producer.</summary>
    internal static class FactionWarRunnerTestExtensions
    {
        public static void ProduceFlagForTest(this FactionWarChainRunner runner, string flagId)
        {
            // Route through the public produce path by resolving is intrusive; the
            // runner exposes flag production only via authored content, so tests
            // inject through the external probe instead: clear the probe and rely
            // on the state list via reflection-free access.
            var state = runner.State;
            if (!state.producedFlags.Contains(flagId))
                state.producedFlags.Add(flagId);
        }
    }
}
