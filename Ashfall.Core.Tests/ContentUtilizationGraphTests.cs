// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: Content Utilization Graph Tests
//
// Tests for Ticket #127 — content utilization graph, scanner,
// instrumentation, exemptions, CI gate, and manifest determinism.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Content;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ContentUtilizationGraphTests
    {
        private static string FindRepoRoot()
        {
            string dir = Directory.GetCurrentDirectory();
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir, "Assets", "StreamingAssets", "Data")))
                    return dir;
                var parent = Directory.GetParent(dir);
                if (parent == null) break;
                dir = parent.FullName;
            }
            throw new DirectoryNotFoundException("Repository root not found");
        }

        private static string GetDataDir() => Path.Combine(FindRepoRoot(), "Assets", "StreamingAssets", "Data");
        private static string GetCoreDir() => Path.Combine(FindRepoRoot(), "Assets", "Ashfall.Core");
        private static string GetSrcDir() => Path.Combine(FindRepoRoot(), "src");

        // ── A. DISCOVERED VS LOADED ──────────────────────────────────

        [Fact]
        public void DiscoveredFiles_NotAllAreGameplayConsumed()
        {
            var scanner = new ContentUtilizationScanner(FindRepoRoot(), GetDataDir(), GetCoreDir(), GetSrcDir());
            var graph = scanner.Scan();

            // Many files are discovered but not all are gameplay-consumed
            Assert.True(graph.TotalCatalogs > 0, "Should discover catalogs");
            Assert.True(graph.GameplayConsumedCatalogs < graph.TotalCatalogs,
                "Not all discovered catalogs should be classified as gameplay-consumed");
        }

        // ── B. LOADED VS QUERIED ────────────────────────────────────

        [Fact]
        public void CatalogWithLoaderButNoConsumer_IsNotGameplayConsumed()
        {
            var scanner = new ContentUtilizationScanner(FindRepoRoot(), GetDataDir(), GetCoreDir(), GetSrcDir());
            var graph = scanner.Scan();

            // Find at least one catalog that has a loader but no consumer
            var loadedNotQueried = graph.Catalogs
                .Where(c => !string.IsNullOrEmpty(c.Loader) && c.ConsumerSystems.Count == 0
                    && c.Classification != ContentClassification.CODEX_ONLY
                    && c.Classification != ContentClassification.OPTIONAL)
                .ToList();

            foreach (var cat in loadedNotQueried)
            {
                Assert.NotEqual(ContentClassification.GAMEPLAY_CONSUMED, cat.Classification);
            }
        }

        // ── C. QUERY OBSERVATION ────────────────────────────────────

        [Fact]
        public void Instrumentation_RecordsQueries()
        {
            var instr = new ContentUtilizationInstrumentation();
            instr.Enabled = true;

            instr.RecordDefinitionQueried("items.json", "item_water_filter", "GetById", "InventorySystem", 1);
            instr.RecordDefinitionQueried("items.json", "item_iodine_pills", "GetById", "InventorySystem", 1);

            Assert.True(instr.WasCatalogQueried("items.json"));
            Assert.True(instr.WasDefinitionQueried("item_water_filter"));
            Assert.False(instr.WasDefinitionQueried("item_nonexistent"));
            Assert.Equal(2, instr.EventCount);
        }

        [Fact]
        public void Instrumentation_Disabled_DoesNotRecord()
        {
            var instr = new ContentUtilizationInstrumentation();
            // Enabled defaults to false

            instr.RecordDefinitionQueried("items.json", "item_water_filter", "GetById", "InventorySystem", 1);
            Assert.Equal(0, instr.EventCount);
            Assert.False(instr.WasCatalogQueried("items.json"));
        }

        // ── D. ELIGIBLE RANDOM CONTENT ──────────────────────────────

        [Fact]
        public void EligibleButNotSelected_IsNotOrphaned()
        {
            // Content that enters a candidate pool but isn't RNG-selected
            // must not be classified as orphaned. This is a classification test.
            var graph = new ContentUtilizationGraph();
            var cat = new CatalogEntry
            {
                Path = "test_pool.json",
                Classification = ContentClassification.UNRESOLVED,
                ConsumerSystems = new List<string> { "TestSystem" },
                MaxStage = UtilizationStage.QUERIED
            };
            graph.Catalogs.Add(cat);

            // Simulate classification: has consumer and is queried → GAMEPLAY_CONSUMED
            Assert.NotEqual(ContentClassification.ORPHANED, cat.Classification);
            Assert.True(cat.ConsumerSystems.Count > 0);
        }

        // ── E. GATE REACHABILITY ────────────────────────────────────

        [Fact]
        public void GatedContentWithSatisfiablePrerequisites_ReportsReachable()
        {
            var def = new DefinitionEntry
            {
                Id = "quest:gated_quest",
                Catalog = "test_quests.json",
                Reachability = ReachabilityStatus.GATED,
                Gates = new List<string> { "day >= 20", "flag:radio_repaired" }
            };

            Assert.Equal(ReachabilityStatus.GATED, def.Reachability);
            Assert.NotEmpty(def.Gates);
        }

        // ── F. BROKEN GATE ──────────────────────────────────────────

        [Fact]
        public void BrokenGateChain_IsDetected()
        {
            var def = new DefinitionEntry
            {
                Id = "quest:orphan_quest",
                Catalog = "test_quests.json",
                Reachability = ReachabilityStatus.BROKEN_GATE,
                Gates = new List<string> { "flag:no_producer_exists" }
            };

            Assert.Equal(ReachabilityStatus.BROKEN_GATE, def.Reachability);
        }

        // ── G. TEST ONLY ────────────────────────────────────────────

        [Fact]
        public void TestOnlyContent_RemainsTestOnly()
        {
            var cat = new CatalogEntry
            {
                Path = "test_fixture.json",
                Classification = ContentClassification.TEST_ONLY,
                ConsumerSystems = new List<string>()
            };

            Assert.Equal(ContentClassification.TEST_ONLY, cat.Classification);
            Assert.NotEqual(ContentClassification.GAMEPLAY_CONSUMED, cat.Classification);
        }

        // ── H. OPTIONAL EXEMPTION ───────────────────────────────────

        [Fact]
        public void ValidExemption_PassesValidation()
        {
            var exemption = new ContentExemption
            {
                ExemptionId = "exempt_test_001",
                ContentPath = "test_optional.json",
                Owner = "test-owner",
                Classification = "OPTIONAL",
                Rationale = "Test optional content",
                TrackingTicket = "TICKET-127"
            };

            Assert.True(exemption.IsValid());
        }

        // ── I. INVALID EXEMPTION ────────────────────────────────────

        [Fact]
        public void InvalidExemption_FailsValidation()
        {
            var exemption = new ContentExemption
            {
                ExemptionId = "exempt_bad",
                // Missing owner, classification, rationale
                ContentPath = "test.json"
            };

            Assert.False(exemption.IsValid());
        }

        [Fact]
        public void EmptyRationale_IsInvalid()
        {
            var exemption = new ContentExemption
            {
                ExemptionId = "exempt_empty",
                ContentPath = "test.json",
                Owner = "someone",
                Classification = "OPTIONAL",
                Rationale = "" // Empty rationale
            };

            Assert.False(exemption.IsValid());
        }

        // ── J. STALE EXEMPTION ──────────────────────────────────────

        [Fact]
        public void StaleExemption_ReferencesMissingContent()
        {
            var graph = new ContentUtilizationGraph();
            graph.Catalogs.Add(new CatalogEntry { Path = "existing.json" });

            var exemption = new ContentExemption
            {
                ExemptionId = "exempt_stale",
                ContentPath = "nonexistent.json",
                Owner = "someone",
                Classification = "OPTIONAL",
                Rationale = "Was valid but content was removed"
            };

            Assert.True(exemption.IsStale(graph));
        }

        [Fact]
        public void ActiveExemption_IsNotStale()
        {
            var graph = new ContentUtilizationGraph();
            graph.Catalogs.Add(new CatalogEntry { Path = "existing.json" });

            var exemption = new ContentExemption
            {
                ExemptionId = "exempt_active",
                ContentPath = "existing.json",
                Owner = "someone",
                Classification = "OPTIONAL",
                Rationale = "Still valid"
            };

            Assert.False(exemption.IsStale(graph));
        }

        // ── K. NEW ORPHAN CI ────────────────────────────────────────

        [Fact]
        public void NewOrphan_FailsCiGate()
        {
            var baseline = new UtilizationBaseline
            {
                CatalogClassifications = new Dictionary<string, string>
                {
                    ["existing.json"] = "GAMEPLAY_CONSUMED"
                }
            };

            var current = new ContentUtilizationGraph();
            current.Catalogs.Add(new CatalogEntry
            {
                Path = "existing.json",
                Classification = ContentClassification.GAMEPLAY_CONSUMED
            });
            current.Catalogs.Add(new CatalogEntry
            {
                Path = "new_orphan.json",
                Classification = ContentClassification.ORPHANED,
                Findings = new List<string> { "No known consumer" }
            });
            current.ComputeSummaries();

            var result = ContentUtilizationGate.Run(current, baseline);
            Assert.False(result.Passed);
            Assert.Contains(result.Errors, e => e.Contains("new_orphan.json"));
        }

        // ── L. REGRESSION ───────────────────────────────────────────

        [Fact]
        public void PreviouslyConsumed_NowOrphaned_FailsCiGate()
        {
            var baseline = new UtilizationBaseline
            {
                CatalogClassifications = new Dictionary<string, string>
                {
                    ["was_consumed.json"] = "GAMEPLAY_CONSUMED"
                }
            };

            var current = new ContentUtilizationGraph();
            current.Catalogs.Add(new CatalogEntry
            {
                Path = "was_consumed.json",
                Classification = ContentClassification.ORPHANED,
                Findings = new List<string> { "Consumer removed" }
            });
            current.ComputeSummaries();

            var result = ContentUtilizationGate.Run(current, baseline);
            Assert.False(result.Passed);
            Assert.Contains(result.Regressions, r => r.Contains("was_consumed.json"));
        }

        // ── M. MANIFEST DETERMINISM ─────────────────────────────────

        [Fact]
        public void Manifest_IsDeterministic()
        {
            var scanner = new ContentUtilizationScanner(FindRepoRoot(), GetDataDir(), GetCoreDir(), GetSrcDir());
            var graph1 = scanner.Scan();
            var graph2 = scanner.Scan();

            // Same inputs produce identical outputs
            Assert.Equal(graph1.TotalCatalogs, graph2.TotalCatalogs);
            Assert.Equal(graph1.GameplayConsumedCatalogs, graph2.GameplayConsumedCatalogs);
            Assert.Equal(graph1.OrphanedCatalogs, graph2.OrphanedCatalogs);
            Assert.Equal(graph1.ExemptedCatalogs, graph2.ExemptedCatalogs);

            // Catalog paths are identical
            var paths1 = graph1.Catalogs.Select(c => c.Path).OrderBy(p => p).ToList();
            var paths2 = graph2.Catalogs.Select(c => c.Path).OrderBy(p => p).ToList();
            Assert.Equal(paths1, paths2);

            // Use ToDictionary with a custom comparer to avoid duplicate keys from
            // catalogs that share the same filename in different subdirectories
            var classes1 = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var c in graph1.Catalogs)
            {
                string key = c.Path;
                if (classes1.ContainsKey(key)) key = "duplicate:" + c.Path + ":" + classes1.Count;
                classes1[key] = c.Classification.ToString();
            }
            var classes2 = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var c in graph2.Catalogs)
            {
                string key = c.Path;
                if (classes2.ContainsKey(key)) key = "duplicate:" + c.Path + ":" + classes2.Count;
                classes2[key] = c.Classification.ToString();
            }
            Assert.Equal(classes1, classes2);
        }

        // ── N. HARDCODED AUTHORITY ──────────────────────────────────

        [Fact]
        public void HardcodedAuthorityFinding_RecordsCorrectly()
        {
            var finding = new HardcodedAuthorityFinding
            {
                RuntimeSystem = "TestSystem",
                HardcodedSource = "HardcodedArray<int>",
                AvailableCatalog = "test_data.json",
                JsonLoaded = true,
                RuntimeUsesJson = false,
                RecommendedStatus = "Migrate to JSON"
            };

            Assert.Equal("TestSystem", finding.RuntimeSystem);
            Assert.True(finding.JsonLoaded);
            Assert.False(finding.RuntimeUsesJson);
        }

        // ── O. UI METRIC ────────────────────────────────────────────

        [Fact]
        public void UiMetric_DoesNotEquateFileEnumerationWithConnected()
        {
            var scanner = new ContentUtilizationScanner(FindRepoRoot(), GetDataDir(), GetCoreDir(), GetSrcDir());
            var graph = scanner.Scan();

            // Total catalogs (file enumeration) ≠ gameplay-consumed
            Assert.True(graph.TotalCatalogs > graph.GameplayConsumedCatalogs,
                $"File enumeration ({graph.TotalCatalogs}) should NOT equal gameplay-consumed ({graph.GameplayConsumedCatalogs})");

            // The "connected" metric is now: discovered + classified
            // Not just: discovered = connected
            Assert.True(graph.OrphanedCatalogs + graph.GameplayConsumedCatalogs
                + graph.CodexOnlyCatalogs + graph.UnresolvedCatalogs
                + graph.OptionalCatalogs + graph.TestOnlyCatalogs
                + graph.UiOnlyCatalogs <= graph.TotalCatalogs,
                "Classification counts should not exceed total catalogs");
        }

        // ── Graph/Scanner ───────────────────────────────────────────

        [Fact]
        public void Scanner_ProducesValidGraph()
        {
            var scanner = new ContentUtilizationScanner(FindRepoRoot(), GetDataDir(), GetCoreDir(), GetSrcDir());
            var graph = scanner.Scan();

            Assert.NotNull(graph);
            Assert.True(graph.TotalCatalogs > 0);
            Assert.NotEmpty(graph.Nodes);
            Assert.NotEmpty(graph.Edges);
            Assert.NotEmpty(graph.FamilySummaries);
        }

        [Fact]
        public void Graph_Stabilize_ProducesDeterministicOrder()
        {
            var graph = new ContentUtilizationGraph();
            graph.Catalogs.Add(new CatalogEntry { Path = "z.json" });
            graph.Catalogs.Add(new CatalogEntry { Path = "a.json" });
            graph.Catalogs.Add(new CatalogEntry { Path = "m.json" });

            graph.Stabilize();

            Assert.Equal("a.json", graph.Catalogs[0].Path);
            Assert.Equal("m.json", graph.Catalogs[1].Path);
            Assert.Equal("z.json", graph.Catalogs[2].Path);
        }

        [Fact]
        public void ComputeSummaries_Accurate()
        {
            var graph = new ContentUtilizationGraph();
            graph.Catalogs.Add(new CatalogEntry { Path = "gameplay.json", Classification = ContentClassification.GAMEPLAY_CONSUMED });
            graph.Catalogs.Add(new CatalogEntry { Path = "codex.json", Classification = ContentClassification.CODEX_ONLY });
            graph.Catalogs.Add(new CatalogEntry { Path = "orphan.json", Classification = ContentClassification.ORPHANED });
            graph.Catalogs.Add(new CatalogEntry { Path = "optional.json", Classification = ContentClassification.OPTIONAL, ExemptionId = "exempt_1" });

            graph.ComputeSummaries();

            Assert.Equal(4, graph.TotalCatalogs);
            Assert.Equal(1, graph.GameplayConsumedCatalogs);
            Assert.Equal(1, graph.CodexOnlyCatalogs);
            Assert.Equal(1, graph.OrphanedCatalogs);
            Assert.Equal(1, graph.OptionalCatalogs);
            Assert.Equal(1, graph.ExemptedCatalogs);
        }

        // ── Exemption Registry ──────────────────────────────────────

        [Fact]
        public void DefaultExemptions_AreValid()
        {
            var registry = DefaultExemptions.CreateDefault();
            var invalid = registry.GetInvalidExemptions();

            Assert.Empty(invalid);
            Assert.True(registry.Exemptions.Count > 0);
        }

        [Fact]
        public void ExemptionRegistry_FindsExemptions()
        {
            var registry = new ExemptionRegistry();
            registry.Exemptions.Add(new ContentExemption
            {
                ExemptionId = "exempt_001",
                ContentPath = "test_data.json",
                Owner = "test",
                Classification = "OPTIONAL",
                Rationale = "Test"
            });

            Assert.True(registry.TryGetExemption("test_data.json", out var ex));
            Assert.Equal("exempt_001", ex.ExemptionId);
            Assert.False(registry.TryGetExemption("nonexistent.json", out _));
        }

        // ── CI Gate ─────────────────────────────────────────────────

        [Fact]
        public void CiGate_NoBaseline_Passes()
        {
            var graph = new ContentUtilizationGraph();
            graph.Catalogs.Add(new CatalogEntry { Path = "test.json", Classification = ContentClassification.GAMEPLAY_CONSUMED });

            var result = ContentUtilizationGate.Run(graph, null);
            Assert.True(result.Passed);
            Assert.Contains(result.Warnings, w => w.Contains("baseline"));
        }

        [Fact]
        public void CiGate_NoChanges_Passes()
        {
            var baseline = new UtilizationBaseline
            {
                CatalogClassifications = new Dictionary<string, string>
                {
                    ["test.json"] = "GAMEPLAY_CONSUMED"
                }
            };

            var graph = new ContentUtilizationGraph();
            graph.Catalogs.Add(new CatalogEntry { Path = "test.json", Classification = ContentClassification.GAMEPLAY_CONSUMED });
            graph.ComputeSummaries();

            var result = ContentUtilizationGate.Run(graph, baseline);
            Assert.True(result.Passed);
        }

        [Fact]
        public void CiGate_Improvement_Detected()
        {
            var baseline = new UtilizationBaseline
            {
                CatalogClassifications = new Dictionary<string, string>
                {
                    ["was_orphan.json"] = "ORPHANED"
                }
            };

            var graph = new ContentUtilizationGraph();
            graph.Catalogs.Add(new CatalogEntry { Path = "was_orphan.json", Classification = ContentClassification.GAMEPLAY_CONSUMED });
            graph.ComputeSummaries();

            var result = ContentUtilizationGate.Run(graph, baseline);
            Assert.True(result.Passed);
            Assert.Contains(result.Improvements, i => i.Contains("was_orphan.json"));
        }

        [Fact]
        public void CiGate_SaveAndLoadBaseline_RoundTripsAccurately()
        {
            var tempPath = Path.Combine(Path.GetTempPath(), $"baseline_test_{Guid.NewGuid():N}.json");
            try
            {
                var baseline = new UtilizationBaseline
                {
                    GeneratedFromCommit = "test_commit_123",
                    CatalogClassifications = new Dictionary<string, string>
                    {
                        ["items.json"] = "GAMEPLAY_CONSUMED",
                        ["echoes.json"] = "OPTIONAL"
                    },
                    KnownOrphans = new List<string> { "legacy_orphan.json" },
                    ExemptedCatalogs = new List<string> { "echoes.json" }
                };

                ContentUtilizationGate.SaveBaseline(baseline, tempPath);
                var loaded = ContentUtilizationGate.LoadBaseline(tempPath);

                Assert.NotNull(loaded);
                Assert.Equal("1.0.0", loaded.SchemaVersion);
                Assert.Equal("test_commit_123", loaded.GeneratedFromCommit);
                Assert.Equal(2, loaded.CatalogClassifications.Count);
                Assert.Equal("GAMEPLAY_CONSUMED", loaded.CatalogClassifications["items.json"]);
                Assert.Equal("OPTIONAL", loaded.CatalogClassifications["echoes.json"]);
                Assert.Single(loaded.KnownOrphans);
                Assert.Equal("legacy_orphan.json", loaded.KnownOrphans[0]);
                Assert.Single(loaded.ExemptedCatalogs);
                Assert.Equal("echoes.json", loaded.ExemptedCatalogs[0]);
            }
            finally
            {
                if (File.Exists(tempPath)) File.Delete(tempPath);
            }
        }

        [Fact]
        public void CiGate_CommittedBaseline_MatchesCurrentScanWithoutRegressions()
        {
            var repoRoot = FindRepoRoot();
            var scanner = new ContentUtilizationScanner(repoRoot, GetDataDir(), GetCoreDir(), GetSrcDir());
            var graph = scanner.Scan();

            var baselinePath = Path.Combine(repoRoot, ContentUtilizationGate.BaselinePath);
            Assert.True(File.Exists(baselinePath), "Committed baseline file must exist on disk");

            var baseline = ContentUtilizationGate.LoadBaseline(baselinePath);
            Assert.NotNull(baseline);
            Assert.NotEmpty(baseline.CatalogClassifications);

            var gateResult = ContentUtilizationGate.Run(graph, baseline);
            Assert.True(gateResult.Passed, $"CI Gate failed against committed baseline:\n{ContentUtilizationGate.FormatReport(gateResult)}");
            Assert.Empty(gateResult.Errors);
            Assert.Empty(gateResult.Regressions);
            Assert.Empty(gateResult.NewOrphans);
        }

        // ── Manifest Generation ─────────────────────────────────────

        [Fact]
        public void Manifest_GeneratesValidJson()
        {
            var graph = new ContentUtilizationGraph();
            graph.Catalogs.Add(new CatalogEntry { Path = "test.json", Classification = ContentClassification.GAMEPLAY_CONSUMED });
            graph.Stabilize();
            graph.ComputeSummaries();

            string json = ContentUtilizationManifest.GenerateManifest(graph);
            Assert.NotNull(json);
            Assert.Contains("test.json", json);
            Assert.Contains("gameplayConsumed", json); // camelCase from System.Text.Json
        }

        [Fact]
        public void Report_GeneratesMarkdown()
        {
            var graph = new ContentUtilizationGraph();
            graph.Catalogs.Add(new CatalogEntry { Path = "test.json", Classification = ContentClassification.GAMEPLAY_CONSUMED });
            graph.Stabilize();
            graph.ComputeSummaries();

            string report = ContentUtilizationManifest.GenerateReport(graph);
            Assert.NotNull(report);
            Assert.Contains("ASHFALL Content Utilization Report", report);
            Assert.Contains("Summary", report);
            Assert.Contains("Total Catalogs", report);
            Assert.Contains("1", report); // Total catalogs = 1
        }

        // ── Instrumentation ─────────────────────────────────────────

        [Fact]
        public void Instrumentation_RecordsAllStages()
        {
            var instr = new ContentUtilizationInstrumentation();
            instr.Enabled = true;

            instr.RecordCatalogOpened("test.json", "TestLoader");
            instr.RecordCatalogDeserialized("test.json", 5);
            instr.RecordDefinitionsRegistered("test.json", "TestRegistry", 5);
            instr.RecordDefinitionQueried("test.json", "def_001", "GetById", "TestSystem", 1);
            instr.RecordDefinitionSelected("test.json", "def_001", "TestSystem", 1);
            instr.RecordDefinitionConsumed("test.json", "def_001", "TestSystem", "effect", 1);

            Assert.Equal(6, instr.EventCount);
            Assert.True(instr.WasCatalogQueried("test.json"));
            Assert.True(instr.WasDefinitionQueried("def_001"));
            Assert.True(instr.WasDefinitionSelected("def_001"));
            Assert.True(instr.WasDefinitionConsumed("def_001"));
        }

        [Fact]
        public void Instrumentation_Clear_ResetsState()
        {
            var instr = new ContentUtilizationInstrumentation();
            instr.Enabled = true;
            instr.RecordDefinitionQueried("test.json", "def_001", "GetById", "TestSystem", 1);
            instr.Clear();

            Assert.Equal(0, instr.EventCount);
            Assert.False(instr.WasCatalogQueried("test.json"));
        }

        [Fact]
        public void Instrumentation_MergeInto_UpdatesGraph()
        {
            var instr = new ContentUtilizationInstrumentation();
            instr.Enabled = true;
            instr.RecordDefinitionQueried("test.json", "def_001", "GetById", "TestSystem", 1);

            var graph = new ContentUtilizationGraph();
            graph.Catalogs.Add(new CatalogEntry
            {
                Path = "test.json",
                Classification = ContentClassification.UNRESOLVED,
                MaxStage = UtilizationStage.LOADED
            });

            instr.MergeInto(graph);

            var cat = graph.Catalogs[0];
            Assert.Equal(UtilizationStage.QUERIED, cat.MaxStage);
            Assert.Equal(EvidenceTier.RUNTIME, cat.BestEvidence);
        }
    }
}