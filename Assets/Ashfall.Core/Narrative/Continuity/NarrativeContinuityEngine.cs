// SPDX-License-Identifier: MIT
// ASHFALL Core: Narrative continuity engine (Plan 50 / Task 16).
//
// One lint authority, shared by the CLI selftest (--narrative-continuity-
// selftest) and the unit-test layer. Structural rules are HARD; flag-ledger
// audits are WARN unless a case-discipline collision is proven. The engine
// never edits prose — it reports.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Narrative.Continuity
{
    public sealed class NarrativeContinuityEngine
    {
        private readonly string _dataDir;

        /// <summary>Catalogs that carry graph-shaped narrative (stage chains etc.).</summary>
        public static readonly string[] StageChainFiles =
        {
            "verdict_questlines.json",
            "year_of_ash_questlines.json",
            "faction_war_events.json",
        };

        /// <summary>Catalogs that carry schedulable events / echo content with flags.</summary>
        public static readonly string[] EventLikeFiles =
        {
            "events.json",
            "echoes.json",
            "incidents.json",
        };

        /// <summary>Quest catalogs joined into the cross-file quest reference graph.</summary>
        public static readonly string[] QuestCatalogFiles =
        {
            "quests_npc_arcs.json",
            "quests_faction_branching.json",
            "quests_bureaucratic_morality.json",
            "quests_expansion_05.json",
            "quests_expansion_06.json",
            "quests_massive_expansion_200.json",
            "quests_moral_branching_expansion.json",
            "moral_choice_quests.json",
            "moral_choice_quests_branching.json",
            "moral_choice_quests_distress.json",
            "moral_choice_chains.json",
            "standing_record_quests.json",
            "holdfast_quests.json",
            "duty_roster_quests.json",
            "crossing_quests.json",
        };

        // Wire keys (case-sensitive, as authored).
        private const string SetFlagKey = "set_flag";
        private const string SetWorldFlagKey = "setWorldFlag";
        private static readonly string[] FlagReadKeys =
        {
            "RequiredFlagId", "requiredFlagId", "required_flag", "locked_flag",
        };

        public NarrativeContinuityEngine(string dataDir)
        {
            _dataDir = dataDir ?? throw new ArgumentNullException(nameof(dataDir));
        }

        /// <summary>
        /// Full corpus analysis. Same engine for CLI and tests — parity by construction.
        /// </summary>
        public NarrativeContinuityReport Analyze()
        {
            var report = new NarrativeContinuityReport();
            var flagSetters = new Dictionary<string, List<string>>(StringComparer.Ordinal);
            var flagReaders = new Dictionary<string, List<string>>(StringComparer.Ordinal);
            var allNodeIds = new HashSet<string>(StringComparer.Ordinal);

            foreach (var file in EnumerateCorpusFiles())
            {
                string path = Path.Combine(_dataDir, file);
                if (!File.Exists(path)) continue;
                report.FilesScanned++;

                string json;
                try
                {
                    json = File.ReadAllText(path);
                }
                catch (Exception)
                {
                    report.Findings.Add(new NarrativeFinding
                    {
                        Rule = "FILE_UNREADABLE", Severity = "HARD", SourceFile = file,
                        Details = "catalog file could not be read",
                    });
                    continue;
                }

                JsonDocument doc;
                try
                {
                    doc = JsonDocument.Parse(json);
                }
                catch (JsonException ex)
                {
                    report.Findings.Add(new NarrativeFinding
                    {
                        Rule = "FILE_UNPARSEABLE", Severity = "HARD", SourceFile = file,
                        Details = "JSON parse failed: " + ex.Message,
                    });
                    continue;
                }

                using (doc)
                {
                    if (StageChainFiles.Contains(file))
                        NormalizeStageChain(file, doc, report, flagSetters, flagReaders, allNodeIds);
                    else if (EventLikeFiles.Contains(file))
                        NormalizeEventLike(file, doc, report, flagSetters, flagReaders, allNodeIds);
                    else if (QuestCatalogFiles.Contains(file))
                        NormalizeQuestCatalog(file, doc, report, flagSetters, flagReaders, allNodeIds);
                }
            }

            Lint(report, flagSetters, flagReaders);
            report.FlagsSet = flagSetters;
            report.FlagsRead = flagReaders;
            report.Stabilize();
            return report;
        }

        private IEnumerable<string> EnumerateCorpusFiles()
        {
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var f in StageChainFiles.Concat(EventLikeFiles).Concat(QuestCatalogFiles))
            {
                if (seen.Add(f))
                    yield return f;
            }
        }

        // ── Schema normalizers ──────────────────────────────────────

        /// <summary>
        /// Questline/event-chain stage graphs:
        ///   quests[].firstStageId + stages[].stageId + choices[].nextStageId
        ///   chains[].stages[].choices[].leadsToStageId (first stage is root)
        /// </summary>
        private void NormalizeStageChain(
            string file, JsonDocument doc, NarrativeContinuityReport report,
            Dictionary<string, List<string>> flagSetters, Dictionary<string, List<string>> flagReaders,
            HashSet<string> allNodeIds)
        {
            var graph = new NarrativeGraph { GraphId = file, SourceFile = file };
            var nodesByLocal = new Dictionary<string, NarrativeNode>(StringComparer.Ordinal);

            foreach (var group in EnumerateStageGroups(doc))
            {
                string? firstStage = null;
                bool hasExplicitRoot = group.GroupElement.TryGetProperty("firstStageId", out var fsEl)
                                       && fsEl.ValueKind == JsonValueKind.String;
                if (hasExplicitRoot)
                    firstStage = fsEl.GetString();
                string groupId = group.GroupId;

                if (!group.GroupElement.TryGetProperty("stages", out var stagesEl) || stagesEl.ValueKind != JsonValueKind.Array)
                {
                    if (hasExplicitRoot)
                    {
                        report.Findings.Add(new NarrativeFinding
                        {
                            Rule = "MISSING_ROOT", Severity = "HARD", SourceFile = file,
                            NodeId = groupId, Details = $"stage group '{groupId}' declares firstStageId but has no stages array",
                        });
                    }
                    continue;
                }

                // Schema variant: faction_war_events chains carry no firstStageId;
                // the first authored stage is the implicit root. Questline files
                // (verdict / year_of_ash) declare firstStageId explicitly.
                bool implicitRootResolved = false;

                bool firstStageSeen = false;
                foreach (var stage in stagesEl.EnumerateArray())
                {
                    if (stage.ValueKind != JsonValueKind.Object) continue;
                    if (!stage.TryGetProperty("stageId", out var sidEl) || sidEl.ValueKind != JsonValueKind.String) continue;
                    string sid = sidEl.GetString() ?? string.Empty;
                    string qualified = $"{file}::{groupId}::{sid}";

                    if (!hasExplicitRoot && !implicitRootResolved)
                    {
                        firstStage = sid;
                        implicitRootResolved = true;
                    }

                    var node = new NarrativeNode
                    {
                        LocalId = sid,
                        Id = qualified,
                        Kind = NarrativeNodeKind.Stage,
                        SourceFile = file,
                        Label = stage.TryGetProperty("title", out var tEl) && tEl.ValueKind == JsonValueKind.String
                            ? tEl.GetString() ?? string.Empty : string.Empty,
                        IsRoot = firstStage != null && sid == firstStage && !firstStageSeen,
                        IsTerminal = stage.TryGetProperty("isTerminal", out var termEl) && termEl.ValueKind == JsonValueKind.True,
                    };
                    firstStageSeen |= node.IsRoot;

                    CollectFlags(stage, qualified, flagSetters, flagReaders, node);

                    if (stage.TryGetProperty("choices", out var choicesEl) && choicesEl.ValueKind == JsonValueKind.Array)
                    {
                        bool hasOutgoing = false;
                        foreach (var choice in choicesEl.EnumerateArray())
                        {
                            if (choice.ValueKind != JsonValueKind.Object) continue;
                            string? next = null;
                            if (choice.TryGetProperty("nextStageId", out var nEl) && nEl.ValueKind == JsonValueKind.String)
                                next = nEl.GetString();
                            if (next == null && choice.TryGetProperty("leadsToStageId", out var lEl) && lEl.ValueKind == JsonValueKind.String)
                                next = lEl.GetString();
                            if (!string.IsNullOrEmpty(next))
                            {
                                graph.Edges.Add(new NarrativeEdge
                                {
                                    From = qualified, To = $"{file}::{groupId}::{next}",
                                    Type = NarrativeEdgeType.NEXT_STAGE, AuthoredTarget = next,
                                });
                                hasOutgoing = true;
                            }
                        }
                        if (node.IsTerminal && hasOutgoing)
                        {
                            report.Findings.Add(new NarrativeFinding
                            {
                                Rule = "MALFORMED_TERMINAL", Severity = "HARD", SourceFile = file,
                                NodeId = qualified, Details = $"terminal stage '{sid}' has outgoing choices",
                            });
                        }
                        if (!node.IsTerminal && !hasOutgoing)
                        {
                            report.Findings.Add(new NarrativeFinding
                            {
                                Rule = "DEAD_END_STAGE", Severity = "WARN", SourceFile = file,
                                NodeId = qualified, Details = $"non-terminal stage '{sid}' has no outgoing choices",
                            });
                        }
                    }

                    if (nodesByLocal.ContainsKey(qualified))
                    {
                        report.Findings.Add(new NarrativeFinding
                        {
                            Rule = "DUPLICATE_NODE", Severity = "HARD", SourceFile = file,
                            NodeId = qualified, Details = $"duplicate stage id '{sid}'",
                        });
                    }
                    else
                    {
                        nodesByLocal[qualified] = node;
                        graph.Nodes.Add(node);
                        allNodeIds.Add(qualified);
                    }
                }

                if (firstStage == null)
                {
                    report.Findings.Add(new NarrativeFinding
                    {
                        Rule = "MISSING_ROOT", Severity = "HARD", SourceFile = file,
                        NodeId = groupId, Details = $"stage group '{groupId}' has no root (no firstStageId and no authored stages)",
                    });
                }
            }

            report.Graphs.Add(graph);
        }

        /// <summary>Enumerates stage groups: quests[] entries or chains[] entries.</summary>
        private static IEnumerable<(string GroupId, JsonElement GroupElement)> EnumerateStageGroups(JsonDocument doc)
        {
            foreach (var collKey in new[] { "quests", "chains" })
            {
                if (doc.RootElement.ValueKind == JsonValueKind.Object &&
                    doc.RootElement.TryGetProperty(collKey, out var arr) &&
                    arr.ValueKind == JsonValueKind.Array)
                {
                    foreach (var group in arr.EnumerateArray())
                    {
                        if (group.ValueKind != JsonValueKind.Object) continue;
                        string id = group.TryGetProperty("questlineId", out var qEl) && qEl.ValueKind == JsonValueKind.String
                            ? qEl.GetString() ?? string.Empty
                            : group.TryGetProperty("chainId", out var cEl) && cEl.ValueKind == JsonValueKind.String
                                ? cEl.GetString() ?? string.Empty : string.Empty;
                        yield return (id, group);
                    }
                }
            }
        }

        /// <summary>
        /// Event-like leaf content (events / echoes / incidents): nodes are the
        /// authored ids; edges come from effects.scheduleEventId; flags are read
        /// (conditions) and written (choices effects).
        /// </summary>
        private void NormalizeEventLike(
            string file, JsonDocument doc, NarrativeContinuityReport report,
            Dictionary<string, List<string>> flagSetters, Dictionary<string, List<string>> flagReaders,
            HashSet<string> allNodeIds)
        {
            var graph = new NarrativeGraph { GraphId = file, SourceFile = file };
            var localIds = new HashSet<string>(StringComparer.Ordinal);

            foreach (var (id, el) in EnumerateIdItems(doc))
            {
                string qualified = $"{file}::{id}";
                var node = new NarrativeNode
                {
                    LocalId = id, Id = qualified, Kind = NarrativeNodeKind.Event,
                    SourceFile = file,
                    Label = el.TryGetProperty("title", out var tEl) && tEl.ValueKind == JsonValueKind.String
                        ? tEl.GetString() ?? string.Empty : string.Empty,
                };
                CollectFlags(el, qualified, flagSetters, flagReaders, node);

                if (el.TryGetProperty("choices", out var choicesEl) && choicesEl.ValueKind == JsonValueKind.Array)
                {
                    foreach (var choice in choicesEl.EnumerateArray())
                    {
                        if (choice.ValueKind != JsonValueKind.Object) continue;
                        if (choice.TryGetProperty("effects", out var effEl) && effEl.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var eff in effEl.EnumerateArray())
                            {
                                if (eff.ValueKind == JsonValueKind.Object &&
                                    eff.TryGetProperty("scheduleEventId", out var sEl) &&
                                    sEl.ValueKind == JsonValueKind.String)
                                {
                                    string target = sEl.GetString() ?? string.Empty;
                                    if (!string.IsNullOrEmpty(target))
                                    {
                                        graph.Edges.Add(new NarrativeEdge
                                        {
                                            From = qualified, To = $"{file}::{target}",
                                            Type = NarrativeEdgeType.SCHEDULES_EVENT, AuthoredTarget = target,
                                        });
                                    }
                                }
                            }
                        }
                    }
                }

                if (!localIds.Add(id))
                {
                    report.Findings.Add(new NarrativeFinding
                    {
                        Rule = "DUPLICATE_NODE", Severity = "HARD", SourceFile = file,
                        NodeId = qualified, Details = $"duplicate event id '{id}'",
                    });
                    continue;
                }
                graph.Nodes.Add(node);
                allNodeIds.Add(qualified);
            }

            report.Graphs.Add(graph);
        }

        /// <summary>
        /// Quest catalogs: nodes are quest ids; edges are prereq_quest_id and
        /// moral-choice branch entry_quests (cross-file resolved at lint time).
        /// </summary>
        private void NormalizeQuestCatalog(
            string file, JsonDocument doc, NarrativeContinuityReport report,
            Dictionary<string, List<string>> flagSetters, Dictionary<string, List<string>> flagReaders,
            HashSet<string> allNodeIds)
        {
            var graph = new NarrativeGraph { GraphId = file, SourceFile = file };
            var localIds = new HashSet<string>(StringComparer.Ordinal);

            foreach (var (id, el) in EnumerateIdItems(doc))
            {
                string qualified = $"{file}::{id}";
                var node = new NarrativeNode
                {
                    LocalId = id, Id = qualified,
                    Kind = file.Contains("moral_choice_chains", StringComparison.Ordinal)
                        ? NarrativeNodeKind.Branch : NarrativeNodeKind.Quest,
                    SourceFile = file,
                    Label = el.TryGetProperty("title", out var tEl) && tEl.ValueKind == JsonValueKind.String
                        ? tEl.GetString() ?? string.Empty
                        : el.TryGetProperty("display_name", out var dEl) && dEl.ValueKind == JsonValueKind.String
                            ? dEl.GetString() ?? string.Empty : string.Empty,
                };
                CollectFlags(el, qualified, flagSetters, flagReaders, node);

                if (el.TryGetProperty("prereq_quest_id", out var preEl) && preEl.ValueKind == JsonValueKind.String)
                {
                    string target = preEl.GetString() ?? string.Empty;
                    if (!string.IsNullOrEmpty(target))
                        graph.Edges.Add(new NarrativeEdge
                        {
                            From = qualified, To = target, // cross-file: resolved by authored quest id
                            Type = NarrativeEdgeType.QUEST_PREREQ, AuthoredTarget = target,
                        });
                }

                if (el.TryGetProperty("entry_quests", out var eqEl) && eqEl.ValueKind == JsonValueKind.Array)
                {
                    foreach (var q in eqEl.EnumerateArray())
                    {
                        if (q.ValueKind == JsonValueKind.String)
                            graph.Edges.Add(new NarrativeEdge
                            {
                                From = qualified, To = q.GetString() ?? string.Empty,
                                Type = NarrativeEdgeType.BRANCH_ENTRY_QUEST,
                                AuthoredTarget = q.GetString() ?? string.Empty,
                            });
                    }
                }

                if (!localIds.Add(id))
                {
                    report.Findings.Add(new NarrativeFinding
                    {
                        Rule = "DUPLICATE_NODE", Severity = "HARD", SourceFile = file,
                        NodeId = qualified, Details = $"duplicate quest id '{id}'",
                    });
                    continue;
                }
                graph.Nodes.Add(node);
                allNodeIds.Add(qualified);
            }

            report.Graphs.Add(graph);
        }

        /// <summary>Enumerates (id, element) pairs from wrapped-array catalogs.</summary>
        private static IEnumerable<(string Id, JsonElement Element)> EnumerateIdItems(JsonDocument doc)
        {
            if (doc.RootElement.ValueKind != JsonValueKind.Object) yield break;
            foreach (var prop in doc.RootElement.EnumerateObject())
            {
                if (prop.Name == "schema_version") continue;
                if (prop.Value.ValueKind != JsonValueKind.Array) continue;
                foreach (var item in prop.Value.EnumerateArray())
                {
                    if (item.ValueKind != JsonValueKind.Object) continue;
                    if (item.TryGetProperty("id", out var idEl) && idEl.ValueKind == JsonValueKind.String)
                        yield return (idEl.GetString() ?? string.Empty, item);
                }
            }
        }

        // ── Flag collection ─────────────────────────────────────────

        private void CollectFlags(
            JsonElement el, string nodeId,
            Dictionary<string, List<string>> flagSetters, Dictionary<string, List<string>> flagReaders,
            NarrativeNode node)
        {
            // Depth-first walk; the schema variants nest flags under choices/effects.
            WalkForFlags(el, nodeId, flagSetters, flagReaders, node, depth: 0);
        }

        private static void WalkForFlags(
            JsonElement el, string nodeId,
            Dictionary<string, List<string>> flagSetters, Dictionary<string, List<string>> flagReaders,
            NarrativeNode node, int depth)
        {
            if (depth > 6) return;
            if (el.ValueKind == JsonValueKind.Object)
            {
                foreach (var prop in el.EnumerateObject())
                {
                    switch (prop.Name)
                    {
                        case SetFlagKey:
                        case SetWorldFlagKey:
                            if (prop.Value.ValueKind == JsonValueKind.String)
                                AddFlag(flagSetters, prop.Value.GetString() ?? string.Empty, nodeId, node.FlagsSet);
                            break;
                        default:
                            if (FlagReadKeys.Contains(prop.Name) && prop.Value.ValueKind == JsonValueKind.String)
                                AddFlag(flagReaders, prop.Value.GetString() ?? string.Empty, nodeId, node.FlagsRead);
                            break;
                    }
                    if (prop.Value.ValueKind is JsonValueKind.Object or JsonValueKind.Array)
                        WalkForFlags(prop.Value, nodeId, flagSetters, flagReaders, node, depth + 1);
                }
            }
            else if (el.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in el.EnumerateArray())
                    if (item.ValueKind is JsonValueKind.Object or JsonValueKind.Array)
                        WalkForFlags(item, nodeId, flagSetters, flagReaders, node, depth + 1);
            }
        }

        private static void AddFlag(
            Dictionary<string, List<string>> map, string flag, string nodeId, List<string> nodeFlags)
        {
            if (string.IsNullOrWhiteSpace(flag)) return;
            if (!map.TryGetValue(flag, out var list))
            {
                list = new List<string>();
                map[flag] = list;
            }
            list.Add(nodeId);
            nodeFlags.Add(flag);
        }

        // ── Lint rules ──────────────────────────────────────────────

        private void Lint(
            NarrativeContinuityReport report,
            Dictionary<string, List<string>> flagSetters,
            Dictionary<string, List<string>> flagReaders)
        {
            var allAuthoredIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var g in report.Graphs)
                foreach (var n in g.Nodes)
                    allAuthoredIds.Add(n.LocalId);

            foreach (var g in report.Graphs)
            {
                // Dangling next/choice/schedule targets (in-file graph edges use
                // qualified ids; cross-file edges carry authored ids directly).
                foreach (var e in g.Edges)
                {
                    bool resolved =
                        g.Nodes.Any(n => n.Id == e.To) ||
                        report.Graphs.Any(other => other.Nodes.Any(n => n.Id == e.To)) ||
                        allAuthoredIds.Contains(e.AuthoredTarget);
                    if (!resolved)
                    {
                        // Are there ANY authored nodes with this local id in another file?
                        bool localAnywhere = report.Graphs.Any(other =>
                            other.Nodes.Any(n => n.LocalId == e.AuthoredTarget));
                        report.Findings.Add(new NarrativeFinding
                        {
                            Rule = e.Type == NarrativeEdgeType.SCHEDULES_EVENT ? "DANGLING_SCHEDULE"
                                 : e.Type == NarrativeEdgeType.QUEST_PREREQ ? "DANGLING_PREREQ"
                                 : e.Type == NarrativeEdgeType.BRANCH_ENTRY_QUEST ? "DANGLING_CROSS_FILE_TARGET"
                                 : "DANGLING_NEXT",
                            Severity = "HARD",
                            SourceFile = g.SourceFile,
                            NodeId = e.From,
                            Details = localAnywhere
                                ? $"'{e.AuthoredTarget}' resolves only as a local id in a different file — qualify the reference"
                                : $"'{e.AuthoredTarget}' does not resolve to any authored node",
                        });
                    }
                }

                // Reachability within stage-chain graphs.
                var roots = g.Nodes.Where(n => n.IsRoot).ToList();
                var reachable = new HashSet<string>(StringComparer.Ordinal);
                foreach (var root in roots)
                {
                    var queue = new Queue<string>();
                    queue.Enqueue(root.Id);
                    reachable.Add(root.Id);
                    while (queue.Count > 0)
                    {
                        var cur = queue.Dequeue();
                        foreach (var e in g.Edges.Where(x => x.From == cur))
                        {
                            var target = g.Nodes.FirstOrDefault(n => n.Id == e.To);
                            if (target != null && reachable.Add(target.Id))
                                queue.Enqueue(target.Id);
                        }
                    }
                }

                bool isStageGraph = g.Nodes.Count > 0 && g.Nodes.All(n => n.Kind == NarrativeNodeKind.Stage);
                if (isStageGraph)
                {
                    if (roots.Count == 0 && g.Nodes.Count > 0)
                    {
                        report.Findings.Add(new NarrativeFinding
                        {
                            Rule = "MISSING_ROOT", Severity = "HARD", SourceFile = g.SourceFile,
                            NodeId = g.GraphId, Details = "stage graph has no root (firstStageId)",
                        });
                    }
                    foreach (var n in g.Nodes)
                    {
                        if (reachable.Contains(n.Id) || n.IsRoot) continue;
                        report.Findings.Add(new NarrativeFinding
                        {
                            Rule = "UNREACHABLE_NODE",
                            Severity = n.IsTerminal ? "WARN" : "HARD",
                            SourceFile = g.SourceFile,
                            NodeId = n.Id,
                            Details = n.IsTerminal
                                ? $"terminal stage '{n.LocalId}' is unreachable (reserved/dead branch — allowlist if intentional)"
                                : $"required stage '{n.LocalId}' is unreachable from its root",
                        });
                    }
                }
            }

            // Flag set→read audit.
            foreach (var (flag, setters) in flagSetters)
            {
                if (flagReaders.ContainsKey(flag)) continue;
                report.Findings.Add(new NarrativeFinding
                {
                    Rule = "SET_NEVER_READ", Severity = "WARN", SourceFile = setters[0],
                    NodeId = setters[0], Details = $"flag '{flag}' is set but never read",
                });
            }

            // Flag read→set audit.
            foreach (var (flag, readers) in flagReaders)
            {
                if (flagSetters.ContainsKey(flag)) continue;
                if (NarrativeContinuityAllowlist.IsSystemProvided(flag))
                {
                    report.AppliedAllowlistEntries.Add($"read-never-set:{flag}:system-provided");
                    continue;
                }
                report.Findings.Add(new NarrativeFinding
                {
                    Rule = "READ_NEVER_SET", Severity = "WARN", SourceFile = readers[0],
                    NodeId = readers[0], Details = $"flag '{flag}' is read ({readers.Count} reader(s)) but no authored setter exists",
                });
            }

            // Case discipline: flags that collide case-insensitively but differ in
            // spelling resolve only accidentally — always a HARD error.
            var byCaseFold = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
            foreach (var flag in flagSetters.Keys.Concat(flagReaders.Keys))
            {
                if (!byCaseFold.TryGetValue(flag, out var set))
                {
                    set = new HashSet<string>(StringComparer.Ordinal);
                    byCaseFold[flag] = set;
                }
                set.Add(flag);
            }
            foreach (var (folded, spellings) in byCaseFold)
            {
                if (spellings.Count <= 1) continue;
                var ordered = spellings.OrderBy(s => s, StringComparer.Ordinal).ToList();
                report.Findings.Add(new NarrativeFinding
                {
                    Rule = "CASE_MISMATCH", Severity = "HARD", SourceFile = ordered[0],
                    NodeId = folded, Details = "flag spellings differ only by case: " + string.Join(", ", ordered),
                });
            }

            // Structural exemptions (documented).
            foreach (var ex in NarrativeContinuityAllowlist.StructuralExemptions)
            {
                int removed = report.Findings.RemoveAll(f =>
                    f.Rule == ex.Rule && f.SourceFile == ex.SourceFile &&
                    (string.IsNullOrEmpty(ex.NodeId) || f.NodeId.Contains(ex.NodeId, StringComparison.Ordinal)));
                if (removed > 0)
                    report.AppliedAllowlistEntries.Add($"structural:{ex.Rule}:{ex.SourceFile}:{ex.Reason}");
            }
        }
    }
}
