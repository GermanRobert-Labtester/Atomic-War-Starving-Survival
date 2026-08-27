// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: Architecture Evidence Graph & Completeness Gate.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Ashfall.Core;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ArchitectureTestMapGateTests
    {
        private static string RepoRoot()
        {
            string[] candidates =
            {
                Directory.GetCurrentDirectory(),
                AppContext.BaseDirectory
            };
            foreach (string start in candidates)
            {
                var dir = new DirectoryInfo(Path.GetFullPath(start));
                while (dir != null)
                {
                    string probeMap = Path.Combine(dir.FullName, "docs", "architecture", "ARCHITECTURE_TEST_MAP.md");
                    if (File.Exists(probeMap))
                        return dir.FullName;
                    dir = dir.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate docs/architecture/ARCHITECTURE_TEST_MAP.md from test execution context.");
        }

        [Fact]
        public void ArchitectureTestMap_ExistsAndCoversAllRegisteredSections()
        {
            string root = RepoRoot();
            string docPath = Path.Combine(root, "docs", "architecture", "ARCHITECTURE_TEST_MAP.md");
            Assert.True(File.Exists(docPath), "ARCHITECTURE_TEST_MAP.md must exist in docs/architecture/.");

            string text = File.ReadAllText(docPath);
            Assert.Contains("## 1. Six-Tier Architectural Layering Flow", text);
            Assert.Contains("## 2. Complete Architecture Subsystem & Evidence-Derived Graph Matrix", text);
            Assert.Contains("## 3. Subsystem Deep Evidence Graph & Source Paths", text);
            Assert.Contains("## 4. Lifecycle Status & Reachability Proof Matrix", text);

            var registeredSections = SaveSectionRegistry.All.Select(s => s.SectionKey).ToList();
            Assert.Equal(61, registeredSections.Count);

            var missingSections = new List<string>();
            foreach (string sectionKey in registeredSections)
            {
                if (!text.Contains($"`{sectionKey}`"))
                {
                    missingSections.Add(sectionKey);
                }
            }

            Assert.True(missingSections.Count == 0,
                $"Architecture Test Map is missing {missingSections.Count} registered save sections:\n  " +
                string.Join("\n  ", missingSections));
        }

        [Fact]
        public void ArchitectureTestMap_SixVerticalLayers_AreFullyPopulated()
        {
            string root = RepoRoot();
            string docPath = Path.Combine(root, "docs", "architecture", "ARCHITECTURE_TEST_MAP.md");
            string text = File.ReadAllText(docPath);

            var lines = text.Split('\n')
                .Where(l => l.StartsWith("|") && !l.Contains("---|") && !l.Contains("Section Key") && !l.Contains("Save Store Class") && !l.Contains("Implemented"))
                .ToList();

            Assert.True(lines.Count >= 60,
                $"Expected at least 60 subsystem rows in ARCHITECTURE_TEST_MAP.md matrix, found {lines.Count}.");

            foreach (var line in lines)
            {
                var cols = line.Split('|').Select(c => c.Trim()).Where(c => !string.IsNullOrEmpty(c)).ToList();
                // Format: # | Section Key | Domain | Core System | Data Catalog | Host Session | Save Store | UI Panel | CLI Self-Test / Unit Tests | Status
                if (cols.Count >= 10)
                {
                    string secKey = cols[1];
                    string domain = cols[2];
                    string core = cols[3];
                    string catalog = cols[4];
                    string host = cols[5];
                    string store = cols[6];
                    string ui = cols[7];
                    string test = cols[8];
                    string status = cols[9];

                    Assert.False(string.IsNullOrWhiteSpace(secKey), $"Row {line} missing section key");
                    Assert.False(string.IsNullOrWhiteSpace(domain), $"Section {secKey} missing domain");
                    Assert.False(string.IsNullOrWhiteSpace(core), $"Section {secKey} missing Core system");
                    Assert.False(string.IsNullOrWhiteSpace(catalog), $"Section {secKey} missing Data catalog");
                    Assert.False(string.IsNullOrWhiteSpace(host), $"Section {secKey} missing Host session");
                    Assert.False(string.IsNullOrWhiteSpace(store), $"Section {secKey} missing Save store");
                    Assert.False(string.IsNullOrWhiteSpace(ui), $"Section {secKey} missing UI panel");
                    Assert.False(string.IsNullOrWhiteSpace(test), $"Section {secKey} missing CLI / Unit tests");
                    Assert.False(string.IsNullOrWhiteSpace(status), $"Section {secKey} missing Status badge");
                }
            }
        }

        [Fact]
        public void ArchitectureTestMap_AllReferencedCliFlags_ExistInHostCliRegistry()
        {
            string root = RepoRoot();
            string docPath = Path.Combine(root, "docs", "architecture", "ARCHITECTURE_TEST_MAP.md");
            string text = File.ReadAllText(docPath);

            var matches = Regex.Matches(text, @"`(--[a-z0-9-]+)`");
            var citedFlags = matches.Select(m => m.Groups[1].Value).Distinct().ToList();

            Assert.NotEmpty(citedFlags);

            var missingFlags = new List<string>();
            foreach (string flag in citedFlags)
            {
                if (!HostCliRegistry.FlagMap.ContainsKey(flag))
                {
                    missingFlags.Add(flag);
                }
            }

            Assert.True(missingFlags.Count == 0,
                $"Architecture Test Map references {missingFlags.Count} CLI flags not registered in HostCliRegistry:\n  " +
                string.Join("\n  ", missingFlags));
        }

        [Fact]
        public void ArchitectureTestMap_AllSixStatusesTracked()
        {
            string root = RepoRoot();
            string docPath = Path.Combine(root, "docs", "architecture", "ARCHITECTURE_TEST_MAP.md");
            string text = File.ReadAllText(docPath);

            Assert.Contains("## 4. Lifecycle Status & Reachability Proof Matrix", text);
            Assert.Contains("| Section Key | Implemented | Constructed | Ticked / Cadence | Persisted | Player-Routed | Tested | E2E Status |", text);
            Assert.Contains("**PASS (6/6)**", text);
            Assert.Contains("(100.0%)", text);
        }

        [Fact]
        public void ArchitectureTestMap_SubsystemDeepEvidenceGraph_CitesValidFiles()
        {
            string root = RepoRoot();
            string docPath = Path.Combine(root, "docs", "architecture", "ARCHITECTURE_TEST_MAP.md");
            string text = File.ReadAllText(docPath);

            var fileMatches = Regex.Matches(text, @"\[`([^`]+\.cs)`\]");
            var citedFiles = fileMatches.Select(m => m.Groups[1].Value).Distinct().ToList();

            Assert.True(citedFiles.Count >= 100, $"Expected at least 100 cited C# source files, found {citedFiles.Count}.");

            var missingFiles = new List<string>();
            foreach (string relFile in citedFiles)
            {
                string fullPath = Path.Combine(root, relFile);
                if (!File.Exists(fullPath))
                {
                    missingFiles.Add(relFile);
                }
            }

            Assert.True(missingFiles.Count == 0,
                $"Architecture Test Map references {missingFiles.Count} C# files that do not exist:\n  " +
                string.Join("\n  ", missingFiles));
        }
    }
}
