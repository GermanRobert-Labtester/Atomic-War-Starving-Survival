using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.NarrativeConsequence
{
    /// <summary>
    /// Validates a narrative consequence graph for structural failures A-I.
    /// </summary>
    public sealed class NarrativeValidator
    {
        private readonly NarrativeConsequenceGraph _graph;

        public NarrativeValidator(NarrativeConsequenceGraph graph)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
        }

        public NarrativeValidationReport Validate()
        {
            var report = new NarrativeValidationReport();

            // A: Unreachable choices/endings
            foreach (var choice in _graph.Choices.Values)
            {
                if (!IsChoiceReachable(choice))
                    report.UnreachableChoices.Add(choice.Id);
            }

            // B: Contradictory prerequisites
            report.ContradictoryPrerequisites.AddRange(DetectContradictoryPrerequisites());

            // C: Expired day windows (structural check - quest with maxDay before minDay of dependencies)
            report.ExpiredDayWindows.AddRange(DetectStructuralDayWindowIssues());

            // D: Dead writes
            foreach (var choice in _graph.Choices.Values)
            {
                foreach (var flag in choice.SetFlags ?? new List<string>())
                {
                    if (!string.IsNullOrEmpty(flag) && !IsFlagConsumed(flag))
                        report.DeadWrites.Add($"flag:{flag} set by choice:{choice.Id} has no readers");
                }
                foreach (var counter in choice.SetCounters ?? new List<string>())
                {
                    if (!string.IsNullOrEmpty(counter) && !IsCounterConsumed(counter))
                        report.DeadWrites.Add($"counter:{counter} set by choice:{choice.Id} has no readers");
                }
            }

            // E: Reader-without-writer issues
            foreach (var flag in _graph.Flags.Values)
            {
                if (flag.Readers.Count > 0 && flag.Writers.Count == 0)
                    report.ReaderWithoutWriter.Add($"flag:{flag.Id} has {flag.Readers.Count} readers but no writers");
            }
            foreach (var counter in _graph.Counters.Values)
            {
                if (counter.Readers.Count > 0 && counter.Writers.Count == 0)
                    report.ReaderWithoutWriter.Add($"counter:{counter.Id} has {counter.Readers.Count} readers but no writers");
            }

            // F: Faction/ending exclusivity violations
            foreach (var ending in _graph.Endings.Values)
            {
                if (ending.IsExclusive)
                {
                    foreach (var exclusiveWith in ending.ExclusiveWith ?? new List<string>())
                    {
                        if (_graph.Endings.ContainsKey(exclusiveWith))
                            report.FactionEndingExclusivityViolations.Add($"ending:{ending.Id} is exclusive with ending:{exclusiveWith}");
                    }
                }
            }

            // G: Unreachable endings
            foreach (var ending in _graph.Endings.Values)
            {
                if (!IsEndingReachable(ending))
                    report.UnreachableEndings.Add(ending.Id);
            }

            // H: Missing consequence propagation
            foreach (var quest in _graph.Quests.Values)
            {
                if ((quest.Choices == null || quest.Choices.Count == 0) && (quest.FollowUpQuests == null || quest.FollowUpQuests.Count == 0))
                    report.MissingConsequencePropagation.Add($"quest:{quest.Id} has no downstream consequences");
            }

            // I: Circular dependency deadlocks
            report.CircularDependencies.AddRange(DetectCircularDependencies());

            return report;
        }

        private bool IsChoiceReachable(ChoiceNode choice)
        {
            if (!string.IsNullOrEmpty(choice.QuestId) && _graph.Quests.ContainsKey(choice.QuestId))
                return true;

            foreach (var prereq in choice.Prerequisites ?? new List<string>())
            {
                if (!string.IsNullOrEmpty(prereq) && _graph.Flags.ContainsKey(prereq))
                    return true;
            }
            return false;
        }

        private bool IsFlagConsumed(string flagId)
        {
            if (string.IsNullOrEmpty(flagId)) return false;
            foreach (var quest in _graph.Quests.Values)
            {
                if ((quest.PrerequisiteFlags ?? new List<string>()).Contains(flagId, StringComparer.Ordinal))
                    return true;
            }
            foreach (var choice in _graph.Choices.Values)
            {
                if ((choice.Prerequisites ?? new List<string>()).Contains(flagId, StringComparer.Ordinal))
                    return true;
            }
            foreach (var ending in _graph.Endings.Values)
            {
                if ((ending.PrerequisiteFlags ?? new List<string>()).Contains(flagId, StringComparer.Ordinal))
                    return true;
            }
            return false;
        }

        private bool IsCounterConsumed(string counterId)
        {
            if (string.IsNullOrEmpty(counterId)) return false;
            foreach (var quest in _graph.Quests.Values)
            {
                if ((quest.PrerequisiteCounters ?? new List<string>()).Contains(counterId, StringComparer.Ordinal))
                    return true;
            }
            foreach (var ending in _graph.Endings.Values)
            {
                if ((ending.PrerequisiteCounters ?? new List<string>()).Contains(counterId, StringComparer.Ordinal))
                    return true;
            }
            return false;
        }

        private bool IsEndingReachable(EndingNode ending)
        {
            foreach (var flag in ending.PrerequisiteFlags ?? new List<string>())
            {
                if (!string.IsNullOrEmpty(flag) && _graph.Flags.ContainsKey(flag))
                    return true;
            }
            foreach (var counter in ending.PrerequisiteCounters ?? new List<string>())
            {
                if (!string.IsNullOrEmpty(counter) && _graph.Counters.ContainsKey(counter))
                    return true;
            }
            return false;
        }

        private IEnumerable<string> DetectContradictoryPrerequisites()
        {
            var contradictions = new List<string>();
            foreach (var quest in _graph.Quests.Values)
            {
                var prereqFlags = quest.PrerequisiteFlags ?? new List<string>();
                if (prereqFlags.Count > 1)
                {
                    foreach (var flag in prereqFlags)
                    {
                        foreach (var other in prereqFlags)
                        {
                            if (!string.Equals(flag, other, StringComparison.Ordinal))
                            {
                                // Check if these flags are set by mutually exclusive choices
                                var flagWriters = _graph.Flags.TryGetValue(flag, out var f1) ? f1.Writers : new List<string>();
                                var otherWriters = _graph.Flags.TryGetValue(other, out var f2) ? f2.Writers : new List<string>();
                                var overlap = flagWriters.Intersect(otherWriters, StringComparer.Ordinal).ToList();
                                if (overlap.Count == 0)
                                {
                                    contradictions.Add($"quest:{quest.Id} requires mutually exclusive flags {flag} and {other}");
                                }
                            }
                        }
                    }
                }
            }
            return contradictions;
        }

        private IEnumerable<string> DetectStructuralDayWindowIssues()
        {
            var issues = new List<string>();
            foreach (var quest in _graph.Quests.Values)
            {
                if (quest.MinDay.HasValue && quest.MaxDay.HasValue && quest.MinDay.Value > quest.MaxDay.Value)
                {
                    issues.Add($"quest:{quest.Id} has minDay {quest.MinDay} > maxDay {quest.MaxDay}");
                }
            }
            return issues;
        }

        private IEnumerable<string> DetectCircularDependencies()
        {
            var cycles = new List<string>();
            var visited = new HashSet<string>(StringComparer.Ordinal);
            var recursionStack = new HashSet<string>(StringComparer.Ordinal);

            foreach (var questId in _graph.Quests.Keys)
            {
                if (!visited.Contains(questId))
                    DetectCycleDFS(questId, visited, recursionStack, new HashSet<string>(StringComparer.Ordinal), cycles);
            }
            return cycles;
        }

        private void DetectCycleDFS(string nodeId, HashSet<string> visited, HashSet<string> recursionStack, HashSet<string> path, List<string> cycles)
        {
            visited.Add(nodeId);
            recursionStack.Add(nodeId);
            path.Add(nodeId);

            if (_graph.Quests.TryGetValue(nodeId, out var quest))
            {
                foreach (var followUp in quest.FollowUpQuests ?? new List<string>())
                {
                    if (string.IsNullOrEmpty(followUp)) continue;
                    if (!visited.Contains(followUp))
                    {
                        DetectCycleDFS(followUp, visited, recursionStack, path, cycles);
                    }
                    else if (recursionStack.Contains(followUp))
                    {
                        var cyclePath = string.Join(" -> ", path.SkipWhile(p => !string.Equals(p, followUp, StringComparison.Ordinal)).Concat(new[] { followUp }));
                        cycles.Add(cyclePath);
                    }
                }
            }

            recursionStack.Remove(nodeId);
            path.Remove(nodeId);
        }
    }

    public sealed class NarrativeValidationReport
    {
        public HashSet<string> UnreachableChoices { get; } = new HashSet<string>(StringComparer.Ordinal);
        public HashSet<string> UnreachableEndings { get; } = new HashSet<string>(StringComparer.Ordinal);
        public List<string> ContradictoryPrerequisites { get; } = new List<string>();
        public List<string> ExpiredDayWindows { get; } = new List<string>();
        public List<string> DeadWrites { get; } = new List<string>();
        public List<string> ReaderWithoutWriter { get; } = new List<string>();
        public List<string> FactionEndingExclusivityViolations { get; } = new List<string>();
        public List<string> MissingConsequencePropagation { get; } = new List<string>();
        public List<string> CircularDependencies { get; } = new List<string>();

        public bool HasErrors => UnreachableChoices.Count > 0 || UnreachableEndings.Count > 0 ||
            ContradictoryPrerequisites.Count > 0 || ExpiredDayWindows.Count > 0 || DeadWrites.Count > 0 ||
            ReaderWithoutWriter.Count > 0 || FactionEndingExclusivityViolations.Count > 0 ||
            MissingConsequencePropagation.Count > 0 || CircularDependencies.Count > 0;

        public override string ToString()
        {
            var parts = new List<string>();
            if (UnreachableChoices.Count > 0) parts.Add($"UnreachableChoices: {UnreachableChoices.Count}");
            if (UnreachableEndings.Count > 0) parts.Add($"UnreachableEndings: {UnreachableEndings.Count}");
            if (ContradictoryPrerequisites.Count > 0) parts.Add($"ContradictoryPrerequisites: {ContradictoryPrerequisites.Count}");
            if (ExpiredDayWindows.Count > 0) parts.Add($"ExpiredDayWindows: {ExpiredDayWindows.Count}");
            if (DeadWrites.Count > 0) parts.Add($"DeadWrites: {DeadWrites.Count}");
            if (ReaderWithoutWriter.Count > 0) parts.Add($"ReaderWithoutWriter: {ReaderWithoutWriter.Count}");
            if (FactionEndingExclusivityViolations.Count > 0) parts.Add($"FactionEndingExclusivityViolations: {FactionEndingExclusivityViolations.Count}");
            if (MissingConsequencePropagation.Count > 0) parts.Add($"MissingConsequencePropagation: {MissingConsequencePropagation.Count}");
            if (CircularDependencies.Count > 0) parts.Add($"CircularDependencies: {CircularDependencies.Count}");
            return string.Join(", ", parts);
        }
    }
}
