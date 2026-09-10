// SPDX-License-Identifier: MIT
// Plan 50 / Task 16 — narrative continuity engine tests.
// Unit tests call the SAME engine the CLI selftest uses (parity by construction).
using System;
using System.IO;
using Ashfall.Core.Narrative.Continuity;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class NarrativeContinuityTests : IDisposable
    {
        private readonly string _dir;

        public NarrativeContinuityTests()
        {
            _dir = Path.Combine(Path.GetTempPath(), "ashfall-narrative-continuity-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_dir);
        }

        public void Dispose()
        {
            if (Directory.Exists(_dir))
                Directory.Delete(_dir, recursive: true);
        }

        private NarrativeContinuityReport Analyze() => new NarrativeContinuityEngine(_dir).Analyze();

        private void Write(string file, string json) => File.WriteAllText(Path.Combine(_dir, file), json);

        // 1 — valid graph passes
        [Fact]
        public void ValidGraph_Passes()
        {
            Write("verdict_questlines.json", """
                {
                  "schema_version": 1,
                  "quests": [
                    {
                      "questlineId": "quest_test_line",
                      "firstStageId": "stage_a",
                      "stages": [
                        { "stageId": "stage_a", "choices": [ { "choiceId": "c1", "nextStageId": "stage_b" } ] },
                        { "stageId": "stage_b", "isTerminal": true, "choices": [] }
                      ]
                    }
                  ]
                }
                """);
            var report = Analyze();
            Assert.True(report.Passed, string.Join(";", report.Findings.ConvertAll(f => f.Details)));
            Assert.Equal(1, report.Graphs.Count);
            Assert.Equal(2, report.NodesCount);
            Assert.Equal(1, report.EdgesCount);
        }

        // 2 — dangling next fails
        [Fact]
        public void DanglingNext_Fails()
        {
            Write("verdict_questlines.json", """
                {
                  "schema_version": 1,
                  "quests": [
                    {
                      "questlineId": "quest_test_line",
                      "firstStageId": "stage_a",
                      "stages": [
                        { "stageId": "stage_a", "choices": [ { "choiceId": "c1", "nextStageId": "stage_missing" } ] }
                      ]
                    }
                  ]
                }
                """);
            var report = Analyze();
            Assert.Contains(report.Findings, f => f.Rule == "DANGLING_NEXT" && f.Severity == "HARD");
            Assert.False(report.Passed);
        }

        // 3 — dangling choice target fails (leadsToStageId form)
        [Fact]
        public void DanglingLeadsToStage_Fails()
        {
            Write("faction_war_events.json", """
                {
                  "schema_version": 1,
                  "chains": [
                    {
                      "chainId": "evt_test_chain",
                      "stages": [
                        { "stageId": "s1", "choices": [ { "choiceId": "c1", "leadsToStageId": "s_nowhere" } ] }
                      ]
                    }
                  ]
                }
                """);
            var report = Analyze();
            Assert.Contains(report.Findings, f => f.Rule == "DANGLING_NEXT" && f.Severity == "HARD");
        }

        // 4 — duplicate node fails
        [Fact]
        public void DuplicateNode_Fails()
        {
            Write("events.json", """
                {
                  "schema_version": 1,
                  "events": [
                    { "id": "event_dup", "title": "A" },
                    { "id": "event_dup", "title": "B" }
                  ]
                }
                """);
            var report = Analyze();
            Assert.Contains(report.Findings, f => f.Rule == "DUPLICATE_NODE" && f.Severity == "HARD");
        }

        // 5 — unreachable required node reports
        [Fact]
        public void UnreachableRequiredNode_Reports()
        {
            Write("verdict_questlines.json", """
                {
                  "schema_version": 1,
                  "quests": [
                    {
                      "questlineId": "quest_test_line",
                      "firstStageId": "stage_a",
                      "stages": [
                        { "stageId": "stage_a", "isTerminal": true, "choices": [] },
                        { "stageId": "stage_orphan", "choices": [] }
                      ]
                    }
                  ]
                }
                """);
            var report = Analyze();
            Assert.Contains(report.Findings, f => f.Rule == "UNREACHABLE_NODE" && f.Severity == "HARD" && f.Details.Contains("stage_orphan"));
        }

        // 6 — reserved (terminal) unreachable node warns
        [Fact]
        public void UnreachableTerminalNode_Warns()
        {
            Write("verdict_questlines.json", """
                {
                  "schema_version": 1,
                  "quests": [
                    {
                      "questlineId": "quest_test_line",
                      "firstStageId": "stage_a",
                      "stages": [
                        { "stageId": "stage_a", "isTerminal": true, "choices": [] },
                        { "stageId": "stage_reserved", "isTerminal": true, "choices": [] }
                      ]
                    }
                  ]
                }
                """);
            var report = Analyze();
            var f = Assert.Single(report.Findings, x => x.Rule == "UNREACHABLE_NODE");
            Assert.Equal("WARN", f.Severity);
            Assert.True(report.Passed, "warn-tier unreachable must not hard-fail");
        }

        // 7 — set-never-read reports
        [Fact]
        public void SetNeverRead_Reports()
        {
            Write("events.json", """
                {
                  "schema_version": 1,
                  "events": [
                    { "id": "event_writer", "title": "W",
                      "choices": [ { "choiceId": "c1", "effects": [ { "setWorldFlag": "flag_nobody_reads" } ] } ] }
                  ]
                }
                """);
            var report = Analyze();
            Assert.Contains(report.Findings, f => f.Rule == "SET_NEVER_READ" && f.Severity == "WARN");
            Assert.Equal(1, report.FlagsSet.Count);
        }

        // 8 — read-never-set reports
        [Fact]
        public void ReadNeverSet_Reports()
        {
            Write("echoes.json", """
                {
                  "schema_version": 1,
                  "echoes": [
                    { "id": "echo_reader", "title": "R",
                      "conditions": { "RequiredFlagId": "flag_ghost" } }
                  ]
                }
                """);
            var report = Analyze();
            Assert.Contains(report.Findings, f => f.Rule == "READ_NEVER_SET" && f.Severity == "WARN" && f.Details.Contains("flag_ghost"));
        }

        // 9 — legacy/system-provided allowlist passes
        [Fact]
        public void SystemProvidedFlag_IsAllowlisted()
        {
            Write("echoes.json", """
                {
                  "schema_version": 1,
                  "echoes": [
                    { "id": "echo_reader", "title": "R",
                      "conditions": { "RequiredFlagId": "micro_contamination_exposure" } }
                  ]
                }
                """);
            var report = Analyze();
            Assert.DoesNotContain(report.Findings, f => f.Rule == "READ_NEVER_SET");
            Assert.Contains(report.AppliedAllowlistEntries, a => a.Contains("micro_contamination_exposure"));
        }

        // 10 — case mismatch fails
        [Fact]
        public void CaseMismatch_Fails()
        {
            Write("events.json", """
                {
                  "schema_version": 1,
                  "events": [
                    { "id": "event_writer", "title": "W",
                      "choices": [ { "choiceId": "c1", "effects": [ { "setWorldFlag": "stranger_inside" } ] } ] },
                    { "id": "event_reader", "title": "R",
                      "conditions": { "RequiredFlagId": "Stranger_Inside" } }
                  ]
                }
                """);
            var report = Analyze();
            Assert.Contains(report.Findings, f => f.Rule == "CASE_MISMATCH" && f.Severity == "HARD");
            Assert.False(report.Passed);
        }

        // 11 — CLI/test parity: same engine twice → identical structural digest
        [Fact]
        public void CliTestParity_SameEngineSameDigest()
        {
            Write("verdict_questlines.json", """
                {
                  "schema_version": 1,
                  "quests": [
                    { "questlineId": "quest_p", "firstStageId": "s1",
                      "stages": [ { "stageId": "s1", "isTerminal": true, "choices": [] } ] }
                  ]
                }
                """);
            // The CLI selftest (src/Host/NarrativeContinuitySelfTest.cs) calls
            // exactly this constructor + Analyze; two runs must agree.
            var digestA = new NarrativeContinuityEngine(_dir).Analyze().StructuralDigest();
            var digestB = new NarrativeContinuityEngine(_dir).Analyze().StructuralDigest();
            Assert.Equal(digestA, digestB);
        }

        // 12 — deterministic artifacts (Stabilize → identical output order)
        [Fact]
        public void Output_IsDeterministic()
        {
            Write("events.json", """
                {
                  "schema_version": 1,
                  "events": [
                    { "id": "event_b", "title": "B" },
                    { "id": "event_a", "title": "A" }
                  ]
                }
                """);
            var r1 = Analyze();
            var r2 = Analyze();
            Assert.Equal(r1.StructuralDigest(), r2.StructuralDigest());
            Assert.Equal(
                string.Join(",", r1.Graphs[0].Nodes.ConvertAll(n => n.LocalId)),
                string.Join(",", r2.Graphs[0].Nodes.ConvertAll(n => n.LocalId)));
        }

        // 13 — bounded duration on fixture corpus
        [Fact]
        public void Duration_IsBounded()
        {
            Write("events.json", """
                { "schema_version": 1, "events": [ { "id": "e1", "title": "x" } ] }
                """);
            var sw = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < 50; i++)
                Analyze();
            sw.Stop();
            Assert.True(sw.ElapsedMilliseconds < 5000, $"50 analyses took {sw.ElapsedMilliseconds}ms");
        }

        // Schedule edge resolution (silent_knock style chains)
        [Fact]
        public void ScheduledEvent_Resolves()
        {
            Write("events.json", """
                {
                  "schema_version": 1,
                  "events": [
                    { "id": "part1", "title": "P1",
                      "choices": [ { "choiceId": "c1", "effects": [ { "scheduleEventId": "part2" } ] } ] },
                    { "id": "part2", "title": "P2" }
                  ]
                }
                """);
            var report = Analyze();
            Assert.DoesNotContain(report.Findings, f => f.Rule == "DANGLING_SCHEDULE");
        }

        // Malformed terminal: terminal stage with outgoing choices
        [Fact]
        public void TerminalWithOutgoing_Fails()
        {
            Write("verdict_questlines.json", """
                {
                  "schema_version": 1,
                  "quests": [
                    { "questlineId": "quest_t", "firstStageId": "s1",
                      "stages": [
                        { "stageId": "s1", "isTerminal": true,
                          "choices": [ { "choiceId": "c1", "nextStageId": "s2" } ] },
                        { "stageId": "s2", "isTerminal": true, "choices": [] }
                      ] }
                  ]
                }
                """);
            var report = Analyze();
            Assert.Contains(report.Findings, f => f.Rule == "MALFORMED_TERMINAL" && f.Severity == "HARD");
        }
    }
}
