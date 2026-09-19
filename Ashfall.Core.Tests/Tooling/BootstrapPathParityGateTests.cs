// SPDX-License-Identifier: MIT
// CF-P28 — fresh/restore manifest-bootstrap parity and reset coverage gate.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class BootstrapPathParityGateTests
    {
        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (Directory.Exists(Path.Combine(dir, "src"))) return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        private static string Read(string relative) =>
            File.ReadAllText(Path.Combine(RepoRoot(), relative.Replace('/', Path.DirectorySeparatorChar)));

        [Fact]
        public void RestorePath_InvokesManifestBootstrap_ExactlyOnce()
        {
            string body = ExtractMethodBody(Read("src/Main.SaveOrchestrator.cs"), "RestoreAllSubsystemsFromDisk");
            Assert.Equal(1, CountCalls(body, "ExecuteSubsystemManifestBootstrap"));
        }

        [Fact]
        public void FreshPath_InvokesManifestBootstrap_ExactlyOnce()
        {
            string body = ExtractMethodBody(Read("src/Main.CampaignServices.cs"), "ComposeCampaign");
            Assert.Equal(1, CountCalls(body, "ExecuteSubsystemManifestBootstrap"));
        }

        [Fact]
        public void ManifestRegistration_CoversEveryDedicatedDescriptor_ExactlyOnce()
        {
            string manifest = Read("Assets/Ashfall.Core/Orchestration/SubsystemManifest.cs");
            string lifecycle = Read("src/Main.Lifecycle.cs");
            var dedicated = new HashSet<string>(StringComparer.Ordinal);
            foreach (Match match in Regex.Matches(
                manifest,
                @"(?ms)^\s*new\(\s*""(?<id>[^""]+)""(?<body>.*?)^\s*\)"))
            {
                if (Regex.IsMatch(match.Groups["body"].Value, @"(?m)^\s*true,\s*$"))
                    dedicated.Add(match.Groups["id"].Value);
            }

            var registrations = Regex.Matches(
                    lifecycle,
                    @"RegisterSetupAction\(\s*""(?<id>[^""]+)""")
                .Cast<Match>()
                .GroupBy(match => match.Groups["id"].Value, StringComparer.Ordinal)
                .ToDictionary(group => group.Key, group => group.Count(), StringComparer.Ordinal);

            Assert.Equal(dedicated.OrderBy(id => id), registrations.Keys.OrderBy(id => id));
            Assert.All(registrations, entry => Assert.Equal(1, entry.Value));
        }

        [Fact]
        public void RegisteredTargets_Exist()
        {
            string lifecycle = Read("src/Main.Lifecycle.cs");
            string source = string.Join("\n", Directory.GetFiles(Path.Combine(RepoRoot(), "src"), "Main*.cs")
                .Select(File.ReadAllText));
            string body = ExtractMethodBody(lifecycle, "RegisterManifestSetupActions");
            var targets = Regex.Matches(body, @"\b(?<name>(?:Setup|Ensure)[A-Z][A-Za-z0-9_]*)\s*\(")
                .Cast<Match>()
                .Select(match => match.Groups["name"].Value)
                .Distinct(StringComparer.Ordinal)
                .ToList();

            Assert.NotEmpty(targets);
            foreach (string target in targets)
            {
                Assert.Matches(
                    new Regex(@"(?m)^\s*(?:private|public|internal|protected)\s+[^\r\n\{;]+\b"
                              + Regex.Escape(target) + @"\s*\("),
                    source);
            }
        }

        [Fact]
        public void ManifestFields_HaveResetCoverage()
        {
            string source = string.Join("\n", Directory.GetFiles(Path.Combine(RepoRoot(), "src"), "Main*.cs")
                .Select(File.ReadAllText));
            string[] fields =
            {
                "_journal", "_survivors", "_inventory", "_world", "_doseLedger", "_radio",
                "_expeditions", "_dutyRoster", "_crafting", "_sharedResearch", "_medical",
                "_medicalWard", "_factionBranch", "_economy", "_greenhouse", "_skyDefense",
                "_vehicleGarage", "_blackMarket", "_memorial"
            };

            foreach (string field in fields)
            {
                Assert.Matches(new Regex(Regex.Escape(field) + @"\s*=\s*null"), source);
            }
        }

        [Fact]
        public void BootstrapCallSites_AreExactlyTwo()
        {
            var matches = new List<string>();
            foreach (string path in Directory.GetFiles(Path.Combine(RepoRoot(), "src"), "Main*.cs"))
            {
                foreach (string line in File.ReadLines(path))
                {
                    if (line.Contains("ExecuteSubsystemManifestBootstrap();", StringComparison.Ordinal))
                        matches.Add(Path.GetFileName(path));
                }
            }

            Assert.Equal(2, matches.Count);
            Assert.Contains("Main.CampaignServices.cs", matches);
            Assert.Contains("Main.SaveOrchestrator.cs", matches);
        }

        private static int CountCalls(string body, string method) =>
            Regex.Matches(body, @"\b" + Regex.Escape(method) + @"\s*\(").Count;

        private static string ExtractMethodBody(string source, string method)
        {
            Match match = Regex.Match(
                source,
                @"(?m)^\s*(?:private|public|internal|protected)\s+[^\r\n\{;]+\b"
                + Regex.Escape(method) + @"\s*\([^;{}]*\)\s*\{");
            Assert.True(match.Success, $"method declaration not found: {method}");
            int openBrace = source.IndexOf('{', match.Index + match.Length - 1);
            return ExtractBalancedBody(source, openBrace);
        }

        private static string ExtractBalancedBody(string source, int openBrace)
        {
            int depth = 0;
            bool lineComment = false;
            bool blockComment = false;
            bool stringLiteral = false;
            bool charLiteral = false;
            bool escaped = false;

            for (int i = openBrace; i < source.Length; i++)
            {
                char c = source[i];
                char next = i + 1 < source.Length ? source[i + 1] : '\0';
                if (lineComment) { if (c == '\n') lineComment = false; continue; }
                if (blockComment) { if (c == '*' && next == '/') { blockComment = false; i++; } continue; }
                if (stringLiteral)
                {
                    if (escaped) { escaped = false; continue; }
                    if (c == '\\') { escaped = true; continue; }
                    if (c == '"') stringLiteral = false;
                    continue;
                }
                if (charLiteral)
                {
                    if (escaped) { escaped = false; continue; }
                    if (c == '\\') { escaped = true; continue; }
                    if (c == '\'') charLiteral = false;
                    continue;
                }
                if (c == '/' && next == '/') { lineComment = true; i++; continue; }
                if (c == '/' && next == '*') { blockComment = true; i++; continue; }
                if (c == '"') { stringLiteral = true; continue; }
                if (c == '\'') { charLiteral = true; continue; }
                if (c == '{') depth++;
                else if (c == '}' && --depth == 0)
                    return source.Substring(openBrace, i - openBrace + 1);
            }

            throw new InvalidOperationException("unbalanced method body: " + openBrace);
        }
    }
}
