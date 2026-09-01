using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.NarrativeConsequence
{
    /// <summary>
    /// Deterministic BFS-based narrative consequence simulator.
    /// Traces forward from a canonical starting state across quests, choices, flags, counters, items, survivors, factions, locations, and endings.
    /// </summary>
    public sealed class NarrativeSimulator
    {
        private const int DefaultMaxSteps = 256;
        private const int DefaultMaxBreadth = 64;

        public NarrativeSimulator() { }

        public NarrativeSimulator(int maxSteps, int maxBreadth)
        {
            MaxSteps = maxSteps > 0 ? maxSteps : DefaultMaxSteps;
            MaxBreadth = maxBreadth > 0 ? maxBreadth : DefaultMaxBreadth;
        }

        public int MaxSteps { get; set; } = DefaultMaxSteps;
        public int MaxBreadth { get; set; } = DefaultMaxBreadth;

        /// <summary>
        /// Runs the simulation and returns a summary of reachable nodes and detected anomalies.
        /// </summary>
        public NarrativeSimulationResult Simulate(NarrativeConsequenceGraph graph, SimulationStartState startState)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));
            startState ??= new SimulationStartState();

            var result = new NarrativeSimulationResult();
            var visited = new HashSet<string>(StringComparer.Ordinal);
            var queue = new Queue<SimulationState>();
            var start = new SimulationState
            {
                Day = startState.Day,
                Flags = new HashSet<string>((startState.InitialFlags ?? new HashSet<string>(StringComparer.Ordinal)), StringComparer.Ordinal),
                Counters = new Dictionary<string, int>(startState.InitialCounters ?? new Dictionary<string, int>(StringComparer.Ordinal)),
                Items = new HashSet<string>((startState.InitialItems ?? new HashSet<string>(StringComparer.Ordinal)), StringComparer.Ordinal),
                VisitedQuests = new HashSet<string>((startState.InitialQuests ?? new HashSet<string>(StringComparer.Ordinal)), StringComparer.Ordinal),
                VisitedChoices = new HashSet<string>(StringComparer.Ordinal),
                Steps = 0
            };

            queue.Enqueue(start);
            visited.Add(CanonicalKey(start));

            while (queue.Count > 0 && result.TotalSteps < MaxSteps)
            {
                var current = queue.Dequeue();
                result.TotalSteps = current.Steps;

                // Track visited nodes
                foreach (var q in current.VisitedQuests) result.ReachableQuests.Add(q);
                foreach (var c in current.VisitedChoices) result.ReachableChoices.Add(c);

                // Check day windows for current quests/choices
                foreach (var questId in current.VisitedQuests)
                {
                    if (graph.Quests.TryGetValue(questId, out var quest))
                    {
                        if (quest.MaxDay.HasValue && current.Day > quest.MaxDay.Value)
                            result.ExpiredDayWindows.Add($"quest:{questId} expired at day {current.Day} > max {quest.MaxDay}");
                        if (quest.MinDay.HasValue && current.Day < quest.MinDay.Value)
                            result.BlockedByDayWindow.Add($"quest:{questId} blocked until day {quest.MinDay}");
                    }
                }

                foreach (var choiceId in current.VisitedChoices)
                {
                    if (graph.Choices.TryGetValue(choiceId, out var choice))
                    {
                        if (choice.MaxDay.HasValue && current.Day > choice.MaxDay.Value)
                            result.ExpiredDayWindows.Add($"choice:{choiceId} expired at day {current.Day} > max {choice.MaxDay}");
                    }
                }

                // Explore from visited quests
                foreach (var questId in current.VisitedQuests)
                {
                    if (!graph.Quests.TryGetValue(questId, out var quest)) continue;

                    // Follow-up quests
                    foreach (var followUp in quest.FollowUpQuests ?? new List<string>())
                    {
                        if (string.IsNullOrEmpty(followUp)) continue;
                        if (CanEnterQuest(graph, followUp, current))
                        {
                            var next = current.Clone();
                            next.VisitedQuests.Add(followUp);
                            next.Steps++;
                            TryEnqueue(queue, visited, next, result);
                        }
                        else
                        {
                            result.UnreachableQuests.Add(followUp);
                        }
                    }

                    // Choices within quest
                    foreach (var choiceId in quest.Choices ?? new List<string>())
                    {
                        if (string.IsNullOrEmpty(choiceId)) continue;
                        if (!graph.Choices.TryGetValue(choiceId, out var choice)) continue;

                        if (CanTakeChoice(choice, current))
                        {
                            var next = current.Clone();
                            next.VisitedChoices.Add(choiceId);
                            ApplyChoiceEffects(choice, next);
                            next.Steps++;
                            TryEnqueue(queue, visited, next, result);
                        }
                        else
                        {
                            result.UnreachableChoices.Add(choiceId);
                        }
                    }
                }

                // Explore from visited choices
                foreach (var choiceId in current.VisitedChoices)
                {
                    if (!graph.Choices.TryGetValue(choiceId, out var choice)) continue;

                    foreach (var unlockQuest in choice.UnlockQuests ?? new List<string>())
                    {
                        if (string.IsNullOrEmpty(unlockQuest)) continue;
                        if (CanEnterQuest(graph, unlockQuest, current))
                        {
                            var next = current.Clone();
                            next.VisitedQuests.Add(unlockQuest);
                            next.Steps++;
                            TryEnqueue(queue, visited, next, result);
                        }
                        else
                        {
                            result.UnreachableQuests.Add(unlockQuest);
                        }
                    }

                    foreach (var endingId in choice.UnlockEndings ?? new List<string>())
                    {
                        result.ReachableEndings.Add(endingId);
                    }
                }

                // Check endings reachable from current state
                foreach (var ending in graph.Endings.Values)
                {
                    if (CanReachEnding(ending, current))
                    {
                        result.ReachableEndings.Add(ending.Id);
                    }
                    else
                    {
                        result.UnreachableEndings.Add(ending.Id);
                    }
                }

                // Detect dead writes
                foreach (var choiceId in current.VisitedChoices)
                {
                    if (!graph.Choices.TryGetValue(choiceId, out var choice)) continue;
                    foreach (var flag in choice.SetFlags ?? new List<string>())
                    {
                        if (!string.IsNullOrEmpty(flag) && !current.Flags.Contains(flag))
                            result.DeadWrites.Add($"flag:{flag} set by choice:{choiceId} but never read in reachable path");
                    }
                    foreach (var counter in choice.SetCounters ?? new List<string>())
                    {
                        if (!string.IsNullOrEmpty(counter) && !current.Counters.ContainsKey(counter))
                            result.DeadWrites.Add($"counter:{counter} set by choice:{choiceId} but never read in reachable path");
                    }
                }
            }

            // Reader-without-writer analysis
            foreach (var flagNode in graph.Flags.Values)
            {
                if (flagNode.Readers.Count > 0 && flagNode.Writers.Count == 0)
                    result.ReaderWithoutWriter.Add($"flag:{flagNode.Id} has readers but no writers");
            }
            foreach (var counterNode in graph.Counters.Values)
            {
                if (counterNode.Readers.Count > 0 && counterNode.Writers.Count == 0)
                    result.ReaderWithoutWriter.Add($"counter:{counterNode.Id} has readers but no writers");
            }

            // Faction/ending exclusivity
            foreach (var ending in graph.Endings.Values)
            {
                if (ending.IsExclusive)
                {
                    foreach (var exclusiveWith in ending.ExclusiveWith ?? new List<string>())
                    {
                        if (result.ReachableEndings.Contains(exclusiveWith))
                            result.FactionEndingExclusivityViolations.Add($"ending:{ending.Id} conflicts with reachable ending:{exclusiveWith}");
                    }
                }
            }

            return result;
        }

        private static bool TryEnqueue(Queue<SimulationState> queue, HashSet<string> visited, SimulationState state, NarrativeSimulationResult result)
        {
            var key = CanonicalKey(state);
            if (!visited.Add(key)) return false;
            if (queue.Count >= result.MaxBreadth) return false;
            queue.Enqueue(state);
            return true;
        }

        private static string CanonicalKey(SimulationState state)
        {
            var flags = string.Join(",", state.Flags.OrderBy(f => f, StringComparer.Ordinal));
            var counters = string.Join(";", state.Counters.OrderBy(kv => kv.Key, StringComparer.Ordinal).Select(kv => $"{kv.Key}={kv.Value}"));
            var items = string.Join(",", state.Items.OrderBy(i => i, StringComparer.Ordinal));
            var quests = string.Join(",", state.VisitedQuests.OrderBy(q => q, StringComparer.Ordinal));
            var choices = string.Join(",", state.VisitedChoices.OrderBy(c => c, StringComparer.Ordinal));
            return $"{state.Day}|{flags}|{counters}|{items}|{quests}|{choices}";
        }

        private static bool CanEnterQuest(NarrativeConsequenceGraph graph, string questId, SimulationState state)
        {
            if (string.IsNullOrEmpty(questId)) return false;
            if (state.VisitedQuests.Contains(questId)) return false;
            if (!graph.Quests.TryGetValue(questId, out var quest)) return false;

            foreach (var flag in quest.PrerequisiteFlags ?? new List<string>())
            {
                if (!string.IsNullOrEmpty(flag) && !state.Flags.Contains(flag)) return false;
            }
            foreach (var counter in quest.PrerequisiteCounters ?? new List<string>())
            {
                if (!string.IsNullOrEmpty(counter) && (!state.Counters.TryGetValue(counter, out var val) || val <= 0)) return false;
            }
            foreach (var item in quest.PrerequisiteItems ?? new List<string>())
            {
                if (!string.IsNullOrEmpty(item) && !state.Items.Contains(item)) return false;
            }
            return true;
        }

        private static bool CanTakeChoice(ChoiceNode choice, SimulationState state)
        {
            foreach (var prereq in choice.Prerequisites ?? new List<string>())
            {
                if (!string.IsNullOrEmpty(prereq) && !state.Flags.Contains(prereq)) return false;
            }
            return true;
        }

        private static void ApplyChoiceEffects(ChoiceNode choice, SimulationState state)
        {
            foreach (var flag in choice.SetFlags ?? new List<string>())
            {
                if (!string.IsNullOrEmpty(flag)) state.Flags.Add(flag);
            }
            foreach (var counter in choice.SetCounters ?? new List<string>())
            {
                if (!string.IsNullOrEmpty(counter))
                {
                    state.Counters[counter] = state.Counters.GetValueOrDefault(counter) + 1;
                }
            }
            foreach (var item in choice.ConsumeItems ?? new List<string>())
            {
                if (!string.IsNullOrEmpty(item)) state.Items.Remove(item);
            }
            foreach (var item in choice.GrantItems ?? new List<string>())
            {
                if (!string.IsNullOrEmpty(item)) state.Items.Add(item);
            }
        }

        private static bool CanReachEnding(EndingNode ending, SimulationState state)
        {
            foreach (var flag in ending.PrerequisiteFlags ?? new List<string>())
            {
                if (!string.IsNullOrEmpty(flag) && !state.Flags.Contains(flag)) return false;
            }
            foreach (var counter in ending.PrerequisiteCounters ?? new List<string>())
            {
                if (!string.IsNullOrEmpty(counter) && (!state.Counters.TryGetValue(counter, out var val) || val <= 0)) return false;
            }
            return true;
        }
    }

    public sealed class SimulationStartState
    {
        public int Day { get; set; } = 0;
        public HashSet<string>? InitialFlags { get; set; }
        public Dictionary<string, int>? InitialCounters { get; set; }
        public HashSet<string>? InitialItems { get; set; }
        public HashSet<string>? InitialQuests { get; set; }
    }

    public sealed class SimulationState
    {
        public int Day { get; set; }
        public HashSet<string> Flags { get; set; } = new HashSet<string>(StringComparer.Ordinal);
        public Dictionary<string, int> Counters { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
        public HashSet<string> Items { get; set; } = new HashSet<string>(StringComparer.Ordinal);
        public HashSet<string> VisitedQuests { get; set; } = new HashSet<string>(StringComparer.Ordinal);
        public HashSet<string> VisitedChoices { get; set; } = new HashSet<string>(StringComparer.Ordinal);
        public int Steps { get; set; }

        public SimulationState Clone()
        {
            return new SimulationState
            {
                Day = this.Day,
                Flags = new HashSet<string>(this.Flags, StringComparer.Ordinal),
                Counters = new Dictionary<string, int>(this.Counters, StringComparer.Ordinal),
                Items = new HashSet<string>(this.Items, StringComparer.Ordinal),
                VisitedQuests = new HashSet<string>(this.VisitedQuests, StringComparer.Ordinal),
                VisitedChoices = new HashSet<string>(this.VisitedChoices, StringComparer.Ordinal),
                Steps = this.Steps
            };
        }
    }

    public sealed class NarrativeSimulationResult
    {
        public int TotalSteps { get; set; }
        public int MaxBreadth { get; set; } = 64;
        public HashSet<string> ReachableQuests { get; } = new HashSet<string>(StringComparer.Ordinal);
        public HashSet<string> ReachableChoices { get; } = new HashSet<string>(StringComparer.Ordinal);
        public HashSet<string> ReachableEndings { get; } = new HashSet<string>(StringComparer.Ordinal);
        public HashSet<string> UnreachableQuests { get; } = new HashSet<string>(StringComparer.Ordinal);
        public HashSet<string> UnreachableChoices { get; } = new HashSet<string>(StringComparer.Ordinal);
        public HashSet<string> UnreachableEndings { get; } = new HashSet<string>(StringComparer.Ordinal);
        public HashSet<string> ExpiredDayWindows { get; } = new HashSet<string>(StringComparer.Ordinal);
        public HashSet<string> BlockedByDayWindow { get; } = new HashSet<string>(StringComparer.Ordinal);
        public HashSet<string> DeadWrites { get; } = new HashSet<string>(StringComparer.Ordinal);
        public HashSet<string> ReaderWithoutWriter { get; } = new HashSet<string>(StringComparer.Ordinal);
        public HashSet<string> FactionEndingExclusivityViolations { get; } = new HashSet<string>(StringComparer.Ordinal);
        public List<string> ContradictoryPrerequisites { get; } = new List<string>();
        public List<string> CircularDependencies { get; } = new List<string>();
    }
}
