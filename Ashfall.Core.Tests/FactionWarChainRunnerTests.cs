using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class FactionWarChainRunnerTests : CatalogTestBase
    {
        private static FactionWarContentCatalog LoadCatalog()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new FactionWarContentCatalogLoader(files, json);
            return loader.Load(DataDirectory);
        }

        // ── Trigger table coverage (correctness gate for the whole design) ──

        [Fact]
        public void TriggerTable_HasAnExplicitEntryForEveryStageInTheCatalog()
        {
            // Every one of the 45 authored stages must have a real, explicit
            // trigger — none should silently fall back to AlwaysTrigger,
            // which would mean a stage fires the instant minDay is reached
            // regardless of what its authored prose actually says.
            var catalog = LoadCatalog();
            var missing = new List<string>();
            foreach (var chain in catalog.EventChains)
            {
                foreach (var stage in chain.stages)
                {
                    if (!FactionWarTriggerTable.ByStageId.ContainsKey(stage.stageId))
                        missing.Add($"{chain.chainId}/{stage.stageId}");
                }
            }
            Assert.True(missing.Count == 0, "stages missing an explicit trigger: " + string.Join(", ", missing));
        }

        [Fact]
        public void TriggerTable_HasNoEntriesForStageIdsThatDontExist()
        {
            // The reverse check: catch typos in the hand-authored table that
            // would otherwise silently reference a stage that doesn't exist.
            var catalog = LoadCatalog();
            var realStageIds = new HashSet<string>();
            foreach (var chain in catalog.EventChains)
                foreach (var stage in chain.stages)
                    realStageIds.Add(stage.stageId);

            var orphaned = FactionWarTriggerTable.ByStageId.Keys.Where(k => !realStageIds.Contains(k)).ToList();
            Assert.True(orphaned.Count == 0, "trigger table entries for nonexistent stages: " + string.Join(", ", orphaned));
        }

        // ── PlayerVisitedTrigger ──────────────────────────────────────────

        [Fact]
        public void ChainOpeningStage_DoesNotSurface_UntilLocationVisited()
        {
            var catalog = LoadCatalog();
            var runner = new FactionWarChainRunner(catalog);

            Assert.Null(runner.GetSurfacedStage("evt_d480_grain_tally_dispute", 480));

            runner.RecordLocationVisited("loc_grain_silo");
            var stage = runner.GetSurfacedStage("evt_d480_grain_tally_dispute", 480);
            Assert.NotNull(stage);
            Assert.Equal("evt_d480_grain_tally_dispute_s1", stage!.stageId);
        }

        [Fact]
        public void ChainOpeningStage_DoesNotSurface_BeforeMinDay_EvenIfVisited()
        {
            var catalog = LoadCatalog();
            var runner = new FactionWarChainRunner(catalog);
            runner.RecordLocationVisited("loc_grain_silo");

            Assert.Null(runner.GetSurfacedStage("evt_d480_grain_tally_dispute", 479));
            Assert.NotNull(runner.GetSurfacedStage("evt_d480_grain_tally_dispute", 480));
        }

        // ── DayOffsetTrigger (linear progression) ──────────────────────────

        [Fact]
        public void ResolveChoice_AdvancesToNextStage_AndAppliesMoraleDelta()
        {
            var catalog = LoadCatalog();
            var runner = new FactionWarChainRunner(catalog);
            runner.RecordLocationVisited("loc_grain_silo");

            runner.ResolveChoice("evt_d480_grain_tally_dispute", "evt_d480_grain_tally_dispute_s1",
                "evt_d480_grain_tally_dispute_s1_c1", 480);

            Assert.Equal(2, runner.CumulativeMoraleDelta);
            Assert.False(runner.IsChainResolved("evt_d480_grain_tally_dispute"));

            // s2 requires 3 days after s1 (which resolved on day 480) -> day 483.
            Assert.Null(runner.GetSurfacedStage("evt_d480_grain_tally_dispute", 482));
            var s2 = runner.GetSurfacedStage("evt_d480_grain_tally_dispute", 483);
            Assert.NotNull(s2);
            Assert.Equal("evt_d480_grain_tally_dispute_s2", s2!.stageId);
        }

        [Fact]
        public void ResolveChoice_TerminalStage_MarksChainResolved()
        {
            var catalog = LoadCatalog();
            var runner = new FactionWarChainRunner(catalog);
            runner.RecordLocationVisited("loc_grain_silo");

            runner.ResolveChoice("evt_d480_grain_tally_dispute", "evt_d480_grain_tally_dispute_s1",
                "evt_d480_grain_tally_dispute_s1_c1", 480);
            Assert.False(runner.IsChainResolved("evt_d480_grain_tally_dispute"));

            runner.ResolveChoice("evt_d480_grain_tally_dispute", "evt_d480_grain_tally_dispute_s2",
                "evt_d480_grain_tally_dispute_s2_c1", 483);
            Assert.True(runner.IsChainResolved("evt_d480_grain_tally_dispute"));
        }

        [Fact]
        public void ResolveChoice_WrongStage_Throws()
        {
            var catalog = LoadCatalog();
            var runner = new FactionWarChainRunner(catalog);
            runner.RecordLocationVisited("loc_grain_silo");

            // s2 hasn't been reached yet — resolving it directly must throw.
            Assert.Throws<System.InvalidOperationException>(() =>
                runner.ResolveChoice("evt_d480_grain_tally_dispute", "evt_d480_grain_tally_dispute_s2",
                    "evt_d480_grain_tally_dispute_s2_c1", 480));
        }

        // ── TickDay auto-advance for zero-choice stages ────────────────────

        [Fact]
        public void TickDay_AutoAdvancesZeroChoiceTerminalStages()
        {
            var catalog = LoadCatalog();
            var runner = new FactionWarChainRunner(catalog);
            runner.RecordLocationVisited("loc_forward_roster_camp");

            // evt_d570_forward_roster_first_action_s2 is a genuine zero-choice
            // terminal stage in the authored data — TickDay must auto-resolve
            // it without any ResolveChoice call, which resolves the chain.
            runner.ResolveChoice("evt_d552_rebuilders_fracture", "evt_d552_rebuilders_fracture_s1",
                "evt_d552_rebuilders_fracture_s1_c1", 552);
            runner.ResolveChoice("evt_d552_rebuilders_fracture", "evt_d552_rebuilders_fracture_s2",
                "evt_d552_rebuilders_fracture_s2_c1", 555);
            Assert.True(runner.IsChainResolved("evt_d552_rebuilders_fracture"));

            runner.ResolveChoice("evt_d570_forward_roster_first_action", "evt_d570_forward_roster_first_action_s1",
                "evt_d570_forward_roster_first_action_s1_c1", 570);
            Assert.False(runner.IsChainResolved("evt_d570_forward_roster_first_action"));

            runner.TickDay(572);
            Assert.True(runner.IsChainResolved("evt_d570_forward_roster_first_action"));
        }

        // ── ChainResolvedTrigger (cross-chain dependency) ──────────────────

        [Fact]
        public void CrossChainDependency_DoesNotSurface_UntilPriorChainResolved()
        {
            var catalog = LoadCatalog();
            var runner = new FactionWarChainRunner(catalog);
            runner.RecordLocationVisited("loc_railway_span_44_alpha");

            // evt_d509_border_clash_span44 depends on evt_d495_the_clean_strike resolving.
            Assert.Null(runner.GetSurfacedStage("evt_d509_border_clash_span44", 600));

            // Drive evt_d495_the_clean_strike to resolution.
            runner.ResolveChoice("evt_d495_the_clean_strike", "evt_d495_the_clean_strike_s1",
                "evt_d495_the_clean_strike_s1_c1", 495);
            runner.ResolveChoice("evt_d495_the_clean_strike", "evt_d495_the_clean_strike_s2",
                "evt_d495_the_clean_strike_s2_c1", 498);
            Assert.True(runner.IsChainResolved("evt_d495_the_clean_strike"));

            var next = runner.GetSurfacedStage("evt_d509_border_clash_span44", 600);
            Assert.NotNull(next);
            Assert.Equal("evt_d509_border_clash_span44_s1", next!.stageId);
        }

        // ── AndTrigger (chain-resolved AND player-visited) ─────────────────

        [Fact]
        public void AndTrigger_RequiresBothConditions_ForwardRosterFirstAction()
        {
            var catalog = LoadCatalog();
            var runner = new FactionWarChainRunner(catalog);

            // Neither condition met yet.
            Assert.Null(runner.GetSurfacedStage("evt_d570_forward_roster_first_action", 700));

            // Meet only the chain-resolved condition.
            runner.ResolveChoice("evt_d552_rebuilders_fracture", "evt_d552_rebuilders_fracture_s1",
                "evt_d552_rebuilders_fracture_s1_c1", 552);
            runner.ResolveChoice("evt_d552_rebuilders_fracture", "evt_d552_rebuilders_fracture_s2",
                "evt_d552_rebuilders_fracture_s2_c1", 555);
            Assert.True(runner.IsChainResolved("evt_d552_rebuilders_fracture"));
            Assert.Null(runner.GetSurfacedStage("evt_d570_forward_roster_first_action", 700));

            // Meet the second condition too.
            runner.RecordLocationVisited("loc_forward_roster_camp");
            var stage = runner.GetSurfacedStage("evt_d570_forward_roster_first_action", 700);
            Assert.NotNull(stage);
        }

        // ── DayOffsetTrigger OR-of-multiple-sources (the d541/d545 fan-out) ──

        [Fact]
        public void FanOutChain_AnyVariantPath_TriggersTheSameFollowUpChain()
        {
            // evt_d545_ration_plaza_strike_s1 fires 2 days after ANY of the
            // three evt_d541 s2 variants resolves, regardless of which path.
            var catalog = LoadCatalog();

            foreach (var choiceId in new[]
            {
                "evt_d541_evacuation_window_plaza_s1_c1", // warn
                "evt_d541_evacuation_window_plaza_s1_c2", // loot
                "evt_d541_evacuation_window_plaza_s1_c3", // stay silent
            })
            {
                var runner = new FactionWarChainRunner(catalog);
                runner.ResolveChoice("evt_d533_garrison_offensive_grain_silo", "evt_d533_garrison_offensive_grain_silo_s1",
                    "evt_d533_garrison_offensive_grain_silo_s1_c1", 533);
                runner.ResolveChoice("evt_d533_garrison_offensive_grain_silo", "evt_d533_garrison_offensive_grain_silo_s2",
                    "evt_d533_garrison_offensive_grain_silo_s2_c1", 536);
                Assert.True(runner.IsChainResolved("evt_d533_garrison_offensive_grain_silo"));

                var s1 = runner.GetSurfacedStage("evt_d541_evacuation_window_plaza", 541);
                Assert.NotNull(s1);
                runner.ResolveChoice("evt_d541_evacuation_window_plaza", "evt_d541_evacuation_window_plaza_s1", choiceId, 541);

                // The fan-out s2 variant is a zero-choice terminal stage —
                // TickDay auto-resolves it, which resolves the whole chain.
                runner.TickDay(543);
                Assert.True(runner.IsChainResolved("evt_d541_evacuation_window_plaza"),
                    $"chain should resolve regardless of path ({choiceId})");

                // Two days after whichever s2 variant resolved (day 543), the
                // ration-plaza strike itself must surface on day 545.
                var strikeStage = runner.GetSurfacedStage("evt_d545_ration_plaza_strike", 545);
                Assert.NotNull(strikeStage);
                Assert.Equal("evt_d545_ration_plaza_strike_s1", strikeStage!.stageId);
            }
        }

        // ── Save round-trip ────────────────────────────────────────────────

        [Fact]
        public void SaveRoundTrip_PreservesProgressVisitsAndMorale()
        {
            var catalog = LoadCatalog();
            var runnerA = new FactionWarChainRunner(catalog);
            runnerA.RecordLocationVisited("loc_grain_silo");
            runnerA.ResolveChoice("evt_d480_grain_tally_dispute", "evt_d480_grain_tally_dispute_s1",
                "evt_d480_grain_tally_dispute_s1_c1", 480);

            var state = runnerA.CaptureState();

            var runnerB = new FactionWarChainRunner(catalog);
            runnerB.RestoreState(state);

            Assert.True(runnerB.HasVisited("loc_grain_silo"));
            Assert.Equal(2, runnerB.CumulativeMoraleDelta);
            Assert.False(runnerB.IsChainResolved("evt_d480_grain_tally_dispute"));
            var s2 = runnerB.GetSurfacedStage("evt_d480_grain_tally_dispute", 483);
            Assert.NotNull(s2);
            Assert.Equal("evt_d480_grain_tally_dispute_s2", s2!.stageId);
        }

        [Fact]
        public void RestoreState_WrongSystemId_Throws()
        {
            var catalog = LoadCatalog();
            var runner = new FactionWarChainRunner(catalog);
            var badState = new FactionWarChainRunnerState { systemId = "not_the_right_system" };
            Assert.Throws<System.ArgumentException>(() => runner.RestoreState(badState));
        }
    }
}
