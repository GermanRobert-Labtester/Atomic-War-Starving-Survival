// SPDX-License-Identifier: MIT
// ASHFALL Core: Narrative continuity model (Plan 50 / Task 16).
//
// Normalizes the corpus's *distinct* narrative schemas (questline stage
// chains, faction-war event chains, scheduled events, echoes, quest
// catalogs, moral-choice branches) into one pure graph model, and reports
// structural integrity findings. Engine-agnostic, deterministic (ordinal
// ordering everywhere), offline-only — never on the startup path.

using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative.Continuity
{
    public enum NarrativeNodeKind
    {
        Stage,      // questline / event-chain stage
        Event,      // schedulable world event
        Quest,      // quest definition
        Branch,     // moral-choice branch
    }

    public enum NarrativeEdgeType
    {
        NEXT_STAGE,          // choice.nextStageId / leadsToStageId
        SCHEDULES_EVENT,     // effects.scheduleEventId
        QUEST_PREREQ,        // prereq_quest_id
        BRANCH_ENTRY_QUEST,  // entry_quests
    }

    [Serializable]
    public sealed class NarrativeNode
    {
        public string Id { get; set; } = string.Empty;          // globally qualified: file::nodeId
        public string LocalId { get; set; } = string.Empty;     // authored id
        public NarrativeNodeKind Kind { get; set; }
        public string SourceFile { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public bool IsRoot { get; set; }
        public bool IsTerminal { get; set; }

        /// <summary>Flags this node's content reads (conditions).</summary>
        public List<string> FlagsRead { get; set; } = new();

        /// <summary>Flags this node's content writes (effects/choices).</summary>
        public List<string> FlagsSet { get; set; } = new();
    }

    [Serializable]
    public sealed class NarrativeEdge
    {
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public NarrativeEdgeType Type { get; set; }
        public string Condition { get; set; } = string.Empty;
        /// <summary>Authored target as written (for dangling diagnostics).</summary>
        public string AuthoredTarget { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class NarrativeGraph
    {
        public string GraphId { get; set; } = string.Empty;
        public string SourceFile { get; set; } = string.Empty;
        public string? RootNodeId { get; set; }
        public List<NarrativeNode> Nodes { get; set; } = new();
        public List<NarrativeEdge> Edges { get; set; } = new();
    }

    [Serializable]
    public sealed class NarrativeFinding
    {
        public string Rule { get; set; } = string.Empty;        // e.g. DANGLING_NEXT
        public string Severity { get; set; } = "HARD";          // HARD | WARN
        public string SourceFile { get; set; } = string.Empty;
        public string NodeId { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class NarrativeContinuityReport
    {
        public string SchemaVersion { get; set; } = "1.0.0";

        public int FilesScanned { get; set; }
        public int GraphsScanned { get; set; }
        public List<NarrativeGraph> Graphs { get; set; } = new();

        public int NodesCount => ComputeTotal(n => n.Nodes.Count);
        public int EdgesCount => ComputeTotal(g => g.Edges.Count);

        public List<NarrativeFinding> Findings { get; set; } = new();

        /// <summary>Every authored flag write, with the writing node.</summary>
        public Dictionary<string, List<string>> FlagsSet { get; set; } = new();

        /// <summary>Every authored flag read, with the reading node.</summary>
        public Dictionary<string, List<string>> FlagsRead { get; set; } = new();

        /// <summary>Explicitly justified exceptions (never a dumping ground).</summary>
        public List<string> AppliedAllowlistEntries { get; set; } = new();

        public int HardErrors => Count("HARD");
        public int Warnings => Count("WARN");
        public bool Passed => HardErrors == 0;

        private int Count(string severity)
        {
            int n = 0;
            foreach (var f in Findings)
                if (f.Severity == severity) n++;
            return n;
        }

        private int ComputeTotal(Func<NarrativeGraph, int> selector)
        {
            int n = 0;
            foreach (var g in Graphs) n += selector(g);
            return n;
        }

        /// <summary>Deterministic ordinal stabilization for artifacts and parity.</summary>
        public void Stabilize()
        {
            Graphs.Sort((a, b) => string.CompareOrdinal(a.GraphId, b.GraphId));
            foreach (var g in Graphs)
            {
                g.Nodes.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
                g.Edges.Sort((a, b) =>
                {
                    int c = string.CompareOrdinal(a.From, b.From);
                    if (c != 0) return c;
                    c = string.CompareOrdinal(a.To, b.To);
                    if (c != 0) return c;
                    return string.CompareOrdinal(a.Type.ToString(), b.Type.ToString());
                });
            }
            Findings.Sort((a, b) =>
            {
                int c = string.CompareOrdinal(a.Severity, b.Severity);
                if (c != 0) return c;
                c = string.CompareOrdinal(a.SourceFile, b.SourceFile);
                if (c != 0) return c;
                c = string.CompareOrdinal(a.Rule, b.Rule);
                if (c != 0) return c;
                return string.CompareOrdinal(a.Details, b.Details);
            });
            FlagsSet = SortFlagMap(FlagsSet);
            FlagsRead = SortFlagMap(FlagsRead);
            AppliedAllowlistEntries.Sort(StringComparer.Ordinal);
        }

        private static Dictionary<string, List<string>> SortFlagMap(Dictionary<string, List<string>> map)
        {
            var result = new Dictionary<string, List<string>>(StringComparer.Ordinal);
            var keys = new List<string>(map.Keys);
            keys.Sort(StringComparer.Ordinal);
            foreach (var k in keys)
            {
                var v = new List<string>(map[k]);
                v.Sort(StringComparer.Ordinal);
                result[k] = v;
            }
            return result;
        }

        /// <summary>Stable digest of the report structure (parity checks).</summary>
        public string StructuralDigest()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("files=").Append(FilesScanned)
              .Append(";graphs=").Append(GraphsScanned)
              .Append(";nodes=").Append(NodesCount)
              .Append(";edges=").Append(EdgesCount)
              .Append(";hard=").Append(HardErrors)
              .Append(";warn=").Append(Warnings)
              .Append(";flagsSet=").Append(FlagsSet.Count)
              .Append(";flagsRead=").Append(FlagsRead.Count);
            foreach (var f in Findings)
                sb.Append('|').Append(f.Severity).Append(':').Append(f.Rule).Append(':').Append(f.SourceFile).Append(':').Append(f.Details);
            return sb.ToString();
        }
    }
}
