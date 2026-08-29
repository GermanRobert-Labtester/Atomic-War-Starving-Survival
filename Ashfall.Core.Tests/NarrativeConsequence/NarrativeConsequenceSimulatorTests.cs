using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.NarrativeConsequence;
using Xunit;

namespace Ashfall.Core.Tests.NarrativeConsequence
{
    public class NarrativeConsequenceSimulatorTests
    {
        #region Graph builders

        private static NarrativeConsequenceGraph BuildBasicGraph()
        {
            var quest1 = new QuestNode { Id = "quest_1", Choices = new List<string> { "choice_1" }, FollowUpQuests = new List<string> { "quest_2" } };
            var quest2 = new QuestNode { Id = "quest_2" };
            var choice1 = new ChoiceNode { Id = "choice_1", QuestId = "quest_1", SetFlags = new List<string> { "flag_a" }, UnlockQuests = new List<string> { "quest_3" } };
            var quest3 = new QuestNode { Id = "quest_3", PrerequisiteFlags = new List<string> { "flag_a" } };
            var flagA = new FlagNode { Id = "flag_a", Writers = new List<string> { "choice_1" }, Readers = new List<string> { "quest_3" } };
            var ending1 = new EndingNode { Id = "ending_1", PrerequisiteFlags = new List<string> { "flag_a" } };

            return new NarrativeConsequenceGraph(
                new[] { quest1, quest2, quest3 },
                new[] { choice1 },
                new[] { flagA },
                Array.Empty<CounterNode>(),
                Array.Empty<ItemNode>(),
                Array.Empty<SurvivorNode>(),
                Array.Empty<FactionNode>(),
                Array.Empty<LocationNode>(),
                new[] { ending1 });
        }

        private static NarrativeConsequenceGraph BuildGraphWithDayWindows()
        {
            var quest = new QuestNode { Id = "quest_early", MinDay = 1, MaxDay = 5, Choices = new List<string> { "choice_late" } };
            var choice = new ChoiceNode { Id = "choice_late", QuestId = "quest_early", MaxDay = 4 };
            return new NarrativeConsequenceGraph(
                new[] { quest },
                new[] { choice },
                Array.Empty<FlagNode>(),
                Array.Empty<CounterNode>(),
                Array.Empty<ItemNode>(),
                Array.Empty<SurvivorNode>(),
                Array.Empty<FactionNode>(),
                Array.Empty<LocationNode>(),
                Array.Empty<EndingNode>());
        }

        private static NarrativeConsequenceGraph BuildGraphWithExclusiveEndings()
        {
            var choice = new ChoiceNode { Id = "choice_faction", UnlockEndings = new List<string> { "ending_a", "ending_b" } };
            var endingA = new EndingNode { Id = "ending_a", IsExclusive = true, ExclusiveWith = new List<string> { "ending_b" } };
            var endingB = new EndingNode { Id = "ending_b", IsExclusive = true, ExclusiveWith = new List<string> { "ending_a" } };
            return new NarrativeConsequenceGraph(
                Array.Empty<QuestNode>(),
                new[] { choice },
                Array.Empty<FlagNode>(),
                Array.Empty<CounterNode>(),
                Array.Empty<ItemNode>(),
                Array.Empty<SurvivorNode>(),
                Array.Empty<FactionNode>(),
                Array.Empty<LocationNode>(),
                new[] { endingA, endingB });
        }

        #endregion

        #region Simulator reachability tests

        [Fact]
        public void Simulate_ReachableQuest_IsIncluded()
        {
            var graph = BuildBasicGraph();
            var simulator = new NarrativeSimulator();
            var start = new SimulationStartState { InitialQuests = new HashSet<string> { "quest_1" } };
            var result = simulator.Simulate(graph, start);

            Assert.Contains("quest_1", result.ReachableQuests);
            Assert.Contains("quest_2", result.ReachableQuests);
            Assert.Contains("quest_3", result.ReachableQuests);
        }

        [Fact]
        public void Simulate_ReachableChoice_IsIncluded()
        {
            var graph = BuildBasicGraph();
            var simulator = new NarrativeSimulator();
            var start = new SimulationStartState { InitialQuests = new HashSet<string> { "quest_1" } };
            var result = simulator.Simulate(graph, start);

            Assert.Contains("choice_1", result.ReachableChoices);
        }

        [Fact]
        public void Simulate_ReachableEnding_IsIncluded()
        {
            var graph = BuildBasicGraph();
            var simulator = new NarrativeSimulator();
            var start = new SimulationStartState { InitialQuests = new HashSet<string> { "quest_1" }, InitialFlags = new HashSet<string> { "flag_a" } };
            var result = simulator.Simulate(graph, start);

            Assert.Contains("ending_1", result.ReachableEndings);
        }

        [Fact]
        public void Simulate_UnreachableQuest_IsReported()
        {
            var graph = BuildBasicGraph();
            var quest1 = graph.Quests["quest_1"];
            quest1.FollowUpQuests = new List<string> { "quest_locked" };
            var quest = new QuestNode { Id = "quest_locked", PrerequisiteFlags = new List<string> { "flag_missing" } };
            graph.Quests["quest_locked"] = quest;

            var simulator = new NarrativeSimulator();
            var start = new SimulationStartState { InitialQuests = new HashSet<string> { "quest_1" } };
            var result = simulator.Simulate(graph, start);

            Assert.Contains("quest_locked", result.UnreachableQuests);
        }

        #endregion

        #region Day window tests

        [Fact]
        public void Simulate_ExpiredChoiceDayWindow_IsReported()
        {
            var graph = BuildGraphWithDayWindows();
            var simulator = new NarrativeSimulator();
            var start = new SimulationStartState { InitialQuests = new HashSet<string> { "quest_early" }, Day = 5 };
            var result = simulator.Simulate(graph, start);

            Assert.Contains(result.ExpiredDayWindows, s => s.Contains("choice_late"));
        }

        [Fact]
        public void Simulate_ExpiredQuestDayWindow_IsReported()
        {
            var graph = BuildGraphWithDayWindows();
            var quest = graph.Quests["quest_early"];
            quest.MaxDay = 3;

            var simulator = new NarrativeSimulator();
            var start = new SimulationStartState { InitialQuests = new HashSet<string> { "quest_early" }, Day = 5 };
            var result = simulator.Simulate(graph, start);

            Assert.Contains(result.ExpiredDayWindows, s => s.Contains("quest_early"));
        }

        #endregion

        #region Dead write / reader-without-writer tests

        [Fact]
        public void Validate_DeadWriteFlag_IsDetected()
        {
            var graph = new NarrativeConsequenceGraph(
                new[] { new QuestNode { Id = "quest_1", Choices = new List<string> { "choice_1" } } },
                new[] { new ChoiceNode { Id = "choice_1", SetFlags = new List<string> { "flag_orphan" } } },
                new[] { new FlagNode { Id = "flag_orphan", Writers = new List<string> { "choice_1" }, Readers = new List<string>() } },
                Array.Empty<CounterNode>(),
                Array.Empty<ItemNode>(),
                Array.Empty<SurvivorNode>(),
                Array.Empty<FactionNode>(),
                Array.Empty<LocationNode>(),
                Array.Empty<EndingNode>());

            var validator = new NarrativeValidator(graph);
            var report = validator.Validate();

            Assert.Contains("flag_orphan", report.DeadWrites.FirstOrDefault() ?? string.Empty);
        }

        [Fact]
        public void Validate_ReaderWithoutWriter_IsDetected()
        {
            var graph = new NarrativeConsequenceGraph(
                new[] { new QuestNode { Id = "quest_1", PrerequisiteFlags = new List<string> { "flag_ghost" } } },
                Array.Empty<ChoiceNode>(),
                new[] { new FlagNode { Id = "flag_ghost", Writers = new List<string>(), Readers = new List<string> { "quest_1" } } },
                Array.Empty<CounterNode>(),
                Array.Empty<ItemNode>(),
                Array.Empty<SurvivorNode>(),
                Array.Empty<FactionNode>(),
                Array.Empty<LocationNode>(),
                Array.Empty<EndingNode>());

            var validator = new NarrativeValidator(graph);
            var report = validator.Validate();

            Assert.Contains("flag_ghost", report.ReaderWithoutWriter.FirstOrDefault() ?? string.Empty);
        }

        #endregion

        #region Contradictory prerequisite tests

        [Fact]
        public void Validate_ContradictoryPrerequisites_AreDetected()
        {
            var graph = new NarrativeConsequenceGraph(
                new[] { new QuestNode { Id = "quest_conflict", PrerequisiteFlags = new List<string> { "flag_x", "flag_y" } } },
                Array.Empty<ChoiceNode>(),
                new[]
                {
                    new FlagNode { Id = "flag_x", Writers = new List<string> { "choice_a" }, Readers = new List<string> { "quest_conflict" } },
                    new FlagNode { Id = "flag_y", Writers = new List<string> { "choice_b" }, Readers = new List<string> { "quest_conflict" } }
                },
                Array.Empty<CounterNode>(),
                Array.Empty<ItemNode>(),
                Array.Empty<SurvivorNode>(),
                Array.Empty<FactionNode>(),
                Array.Empty<LocationNode>(),
                Array.Empty<EndingNode>());

            var validator = new NarrativeValidator(graph);
            var report = validator.Validate();

            Assert.NotEmpty(report.ContradictoryPrerequisites);
            Assert.Contains(report.ContradictoryPrerequisites, s => s.Contains("quest_conflict"));
        }

        #endregion

        #region Faction/ending exclusivity tests

        [Fact]
        public void Validate_FactionEndingExclusivityViolation_IsDetected()
        {
            var graph = BuildGraphWithExclusiveEndings();
            var validator = new NarrativeValidator(graph);
            var report = validator.Validate();

            Assert.Contains(report.FactionEndingExclusivityViolations, s => s.Contains("ending_a") && s.Contains("ending_b"));
        }

        #endregion

        #region Circular dependency tests

        [Fact]
        public void Validate_CircularDependency_IsDetected()
        {
            var graph = new NarrativeConsequenceGraph(
                new[]
                {
                    new QuestNode { Id = "q_a", FollowUpQuests = new List<string> { "q_b" } },
                    new QuestNode { Id = "q_b", FollowUpQuests = new List<string> { "q_a" } }
                },
                Array.Empty<ChoiceNode>(),
                Array.Empty<FlagNode>(),
                Array.Empty<CounterNode>(),
                Array.Empty<ItemNode>(),
                Array.Empty<SurvivorNode>(),
                Array.Empty<FactionNode>(),
                Array.Empty<LocationNode>(),
                Array.Empty<EndingNode>());

            var validator = new NarrativeValidator(graph);
            var report = validator.Validate();

            Assert.NotEmpty(report.CircularDependencies);
            Assert.Contains(report.CircularDependencies, s => s.Contains("q_a") && s.Contains("q_b"));
        }

        #endregion

        #region Missing consequence propagation tests

        [Fact]
        public void Validate_MissingConsequencePropagation_IsDetected()
        {
            var graph = new NarrativeConsequenceGraph(
                new[] { new QuestNode { Id = "quest_dead_end" } },
                Array.Empty<ChoiceNode>(),
                Array.Empty<FlagNode>(),
                Array.Empty<CounterNode>(),
                Array.Empty<ItemNode>(),
                Array.Empty<SurvivorNode>(),
                Array.Empty<FactionNode>(),
                Array.Empty<LocationNode>(),
                Array.Empty<EndingNode>());

            var validator = new NarrativeValidator(graph);
            var report = validator.Validate();

            Assert.Contains("quest_dead_end", report.MissingConsequencePropagation.FirstOrDefault() ?? string.Empty);
        }

        #endregion

        #region Simulation boundary tests

        [Fact]
        public void Simulate_MaxSteps_IsRespected()
        {
            var graph = new NarrativeConsequenceGraph(
                Enumerable.Range(0, 100).Select(i => new QuestNode { Id = $"quest_{i}", FollowUpQuests = new List<string> { i < 99 ? $"quest_{i + 1}" : string.Empty } }),
                Array.Empty<ChoiceNode>(),
                Array.Empty<FlagNode>(),
                Array.Empty<CounterNode>(),
                Array.Empty<ItemNode>(),
                Array.Empty<SurvivorNode>(),
                Array.Empty<FactionNode>(),
                Array.Empty<LocationNode>(),
                Array.Empty<EndingNode>());

            var start = new SimulationStartState { InitialQuests = new HashSet<string> { "quest_0" } };
            var simulator = new NarrativeSimulator(maxSteps: 10, maxBreadth: 64);
            var result = simulator.Simulate(graph, start);

            Assert.True(result.TotalSteps <= 10);
        }

        [Fact]
        public void Simulate_CanonicalState_DeduplicatesIdenticalStates()
        {
            var graph = BuildBasicGraph();
            var start = new SimulationStartState { InitialQuests = new HashSet<string> { "quest_1" } };
            var simulator = new NarrativeSimulator();

            var result1 = simulator.Simulate(graph, start);
            var result2 = simulator.Simulate(graph, start);

            Assert.Equal(result1.ReachableQuests.Count, result2.ReachableQuests.Count);
            Assert.Equal(result1.ReachableChoices.Count, result2.ReachableChoices.Count);
            Assert.Equal(result1.ReachableEndings.Count, result2.ReachableEndings.Count);
        }

        [Fact]
        public void Simulate_EmptyGraph_ReturnsEmptyResult()
        {
            var graph = new NarrativeConsequenceGraph();
            var simulator = new NarrativeSimulator();
            var result = simulator.Simulate(graph, new SimulationStartState());

            Assert.Empty(result.ReachableQuests);
            Assert.Empty(result.ReachableChoices);
            Assert.Empty(result.ReachableEndings);
        }

        #endregion

        #region Structural day window tests

        [Fact]
        public void Validate_StructuralDayWindow_IsDetected()
        {
            var graph = new NarrativeConsequenceGraph(
                new[] { new QuestNode { Id = "quest_bad_window", MinDay = 10, MaxDay = 5 } },
                Array.Empty<ChoiceNode>(),
                Array.Empty<FlagNode>(),
                Array.Empty<CounterNode>(),
                Array.Empty<ItemNode>(),
                Array.Empty<SurvivorNode>(),
                Array.Empty<FactionNode>(),
                Array.Empty<LocationNode>(),
                Array.Empty<EndingNode>());

            var validator = new NarrativeValidator(graph);
            var report = validator.Validate();

            Assert.Contains(report.ExpiredDayWindows, s => s.Contains("quest_bad_window"));
        }

        #endregion

        #region Counter dead write tests

        [Fact]
        public void Validate_DeadWriteCounter_IsDetected()
        {
            var graph = new NarrativeConsequenceGraph(
                new[] { new QuestNode { Id = "quest_1", Choices = new List<string> { "choice_1" } } },
                new[] { new ChoiceNode { Id = "choice_1", SetCounters = new List<string> { "counter_orphan" } } },
                Array.Empty<FlagNode>(),
                new[] { new CounterNode { Id = "counter_orphan", Writers = new List<string> { "choice_1" }, Readers = new List<string>() } },
                Array.Empty<ItemNode>(),
                Array.Empty<SurvivorNode>(),
                Array.Empty<FactionNode>(),
                Array.Empty<LocationNode>(),
                Array.Empty<EndingNode>());

            var validator = new NarrativeValidator(graph);
            var report = validator.Validate();

            Assert.Contains("counter_orphan", report.DeadWrites.FirstOrDefault() ?? string.Empty);
        }

        #endregion

        #region Unreachable ending tests

        [Fact]
        public void Validate_UnreachableEnding_IsDetected()
        {
            var graph = new NarrativeConsequenceGraph(
                Array.Empty<QuestNode>(),
                Array.Empty<ChoiceNode>(),
                Array.Empty<FlagNode>(),
                Array.Empty<CounterNode>(),
                Array.Empty<ItemNode>(),
                Array.Empty<SurvivorNode>(),
                Array.Empty<FactionNode>(),
                Array.Empty<LocationNode>(),
                new[] { new EndingNode { Id = "ending_orphan", PrerequisiteFlags = new List<string> { "flag_nonexistent" } } });

            var validator = new NarrativeValidator(graph);
            var report = validator.Validate();

            Assert.Contains("ending_orphan", report.UnreachableEndings);
        }

        #endregion

        #region Integration / smoke tests

        [Fact]
        public void Validate_Report_HasErrors_ReflectsFindings()
        {
            var graph = BuildBasicGraph();
            var quest = new QuestNode { Id = "quest_locked", PrerequisiteFlags = new List<string> { "flag_missing" } };
            graph.Quests["quest_locked"] = quest;

            var validator = new NarrativeValidator(graph);
            var report = validator.Validate();

            Assert.True(report.HasErrors);
            Assert.NotEmpty(report.ToString());
        }

        [Fact]
        public void Simulator_Deterministic_GivenSameInput()
        {
            var graph = BuildBasicGraph();
            var start = new SimulationStartState { InitialQuests = new HashSet<string> { "quest_1" } };
            var simulator = new NarrativeSimulator();

            var result1 = simulator.Simulate(graph, start);
            var result2 = simulator.Simulate(graph, start);

            Assert.Equal(result1.ReachableQuests.Count, result2.ReachableQuests.Count);
            Assert.Equal(result1.ReachableChoices.Count, result2.ReachableChoices.Count);
            Assert.Equal(result1.ReachableEndings.Count, result2.ReachableEndings.Count);
        }

        #endregion
    }
}
