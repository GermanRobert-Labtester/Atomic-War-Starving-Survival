using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.NarrativeConsequence
{
    /// <summary>
    /// Lightweight graph model for narrative cross-domain consequence tracing.
    /// Indexes quests, choices, flags, counters, items, survivors, factions, locations, and endings.
    /// </summary>
    public sealed class NarrativeConsequenceGraph
    {
        public Dictionary<string, QuestNode> Quests { get; } = new(StringComparer.Ordinal);
        public Dictionary<string, ChoiceNode> Choices { get; } = new(StringComparer.Ordinal);
        public Dictionary<string, FlagNode> Flags { get; } = new(StringComparer.Ordinal);
        public Dictionary<string, CounterNode> Counters { get; } = new(StringComparer.Ordinal);
        public Dictionary<string, ItemNode> Items { get; } = new(StringComparer.Ordinal);
        public Dictionary<string, SurvivorNode> Survivors { get; } = new(StringComparer.Ordinal);
        public Dictionary<string, FactionNode> Factions { get; } = new(StringComparer.Ordinal);
        public Dictionary<string, LocationNode> Locations { get; } = new(StringComparer.Ordinal);
        public Dictionary<string, EndingNode> Endings { get; } = new(StringComparer.Ordinal);

        public HashSet<string> AllNodeIds => new(
            Quests.Keys.Concat(Choices.Keys).Concat(Flags.Keys).Concat(Counters.Keys)
                .Concat(Items.Keys).Concat(Survivors.Keys).Concat(Factions.Keys)
                .Concat(Locations.Keys).Concat(Endings.Keys),
            StringComparer.Ordinal);

        public NarrativeConsequenceGraph() { }

        public NarrativeConsequenceGraph(
            IEnumerable<QuestNode> quests,
            IEnumerable<ChoiceNode> choices,
            IEnumerable<FlagNode> flags,
            IEnumerable<CounterNode> counters,
            IEnumerable<ItemNode> items,
            IEnumerable<SurvivorNode> survivors,
            IEnumerable<FactionNode> factions,
            IEnumerable<LocationNode> locations,
            IEnumerable<EndingNode> endings)
        {
            foreach (var q in quests ?? Array.Empty<QuestNode>()) Quests[q.Id] = q;
            foreach (var c in choices ?? Array.Empty<ChoiceNode>()) Choices[c.Id] = c;
            foreach (var f in flags ?? Array.Empty<FlagNode>()) Flags[f.Id] = f;
            foreach (var c in counters ?? Array.Empty<CounterNode>()) Counters[c.Id] = c;
            foreach (var i in items ?? Array.Empty<ItemNode>()) Items[i.Id] = i;
            foreach (var s in survivors ?? Array.Empty<SurvivorNode>()) Survivors[s.Id] = s;
            foreach (var f in factions ?? Array.Empty<FactionNode>()) Factions[f.Id] = f;
            foreach (var l in locations ?? Array.Empty<LocationNode>()) Locations[l.Id] = l;
            foreach (var e in endings ?? Array.Empty<EndingNode>()) Endings[e.Id] = e;
        }
    }

    public sealed class QuestNode
    {
        public string Id { get; set; } = string.Empty;
        public List<string> PrerequisiteFlags { get; set; } = new();
        public List<string> PrerequisiteCounters { get; set; } = new();
        public List<string> PrerequisiteItems { get; set; } = new();
        public List<string> Choices { get; set; } = new();
        public List<string> FollowUpQuests { get; set; } = new();
        public int? MinDay { get; set; }
        public int? MaxDay { get; set; }
        public string? LocationId { get; set; }
        public List<string> AffectedSurvivors { get; set; } = new();
    }

    public sealed class ChoiceNode
    {
        public string Id { get; set; } = string.Empty;
        public string QuestId { get; set; } = string.Empty;
        public List<string> Prerequisites { get; set; } = new();
        public List<string> SetFlags { get; set; } = new();
        public List<string> SetCounters { get; set; } = new();
        public List<string> ConsumeItems { get; set; } = new();
        public List<string> GrantItems { get; set; } = new();
        public List<string> UnlockQuests { get; set; } = new();
        public List<string> UnlockEndings { get; set; } = new();
        public int? MaxDay { get; set; }
    }

    public sealed class FlagNode
    {
        public string Id { get; set; } = string.Empty;
        public List<string> Writers { get; set; } = new();
        public List<string> Readers { get; set; } = new();
        public List<string> Clearers { get; set; } = new();
    }

    public sealed class CounterNode
    {
        public string Id { get; set; } = string.Empty;
        public List<string> Writers { get; set; } = new();
        public List<string> Readers { get; set; } = new();
        public int? Threshold { get; set; }
    }

    public sealed class ItemNode
    {
        public string Id { get; set; } = string.Empty;
        public List<string> Sources { get; set; } = new();
        public List<string> Sinks { get; set; } = new();
        public bool IsUnique { get; set; }
    }

    public sealed class SurvivorNode
    {
        public string Id { get; set; } = string.Empty;
        public List<string> RelatedQuests { get; set; } = new();
        public List<string> RelatedFlags { get; set; } = new();
    }

    public sealed class FactionNode
    {
        public string Id { get; set; } = string.Empty;
        public List<string> ExclusiveEndings { get; set; } = new();
        public List<string> AlliedFactions { get; set; } = new();
        public List<string> OpposedFactions { get; set; } = new();
    }

    public sealed class LocationNode
    {
        public string Id { get; set; } = string.Empty;
        public List<string> RequiredFlags { get; set; } = new();
        public List<string> UnlockedByQuests { get; set; } = new();
    }

    public sealed class EndingNode
    {
        public string Id { get; set; } = string.Empty;
        public List<string> PrerequisiteFlags { get; set; } = new();
        public List<string> PrerequisiteCounters { get; set; } = new();
        public List<string> PrerequisiteFactions { get; set; } = new();
        public bool IsExclusive { get; set; }
        public List<string> ExclusiveWith { get; set; } = new();
    }
}
