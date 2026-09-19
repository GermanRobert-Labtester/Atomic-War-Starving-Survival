// SPDX-License-Identifier: MIT
// Plan 37 — Input map contract tests (P1 gate mirror).
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    public sealed class InputMapContractTests
    {
        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (File.Exists(Path.Combine(dir, "project.godot"))) return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        private static string Read(string relative) =>
            File.ReadAllText(Path.Combine(RepoRoot(), relative.Replace('/', Path.DirectorySeparatorChar)));

        private static HashSet<string> GetProjectGodotActions()
        {
            string content = Read("project.godot");
            var actions = new HashSet<string>(StringComparer.Ordinal);
            var match = Regex.Match(content, @"\[input\](?<inputSection>.*?)(\n\[|\z)", RegexOptions.Singleline);
            if (match.Success)
            {
                var matches = Regex.Matches(match.Groups["inputSection"].Value, @"(?m)^(ashfall_[a-z0-9_]+)=");
                foreach (Match m in matches)
                {
                    actions.Add(m.Groups[1].Value);
                }
            }
            return actions;
        }

        private sealed record ContractEntry(string Action, string Scope, string? RouteId, bool Rebindable);

        private static List<ContractEntry> ParseContractEntries()
        {
            string content = Read("src/Host/AshfallInputActions.cs");
            var entries = new List<ContractEntry>();

            // Find the Contract array initialization
            var contractBlockMatch = Regex.Match(content, @"public static readonly IReadOnlyList<InputActionContract> Contract\s*=\s*new\[\]\s*\{(?<entries>.*?)\};", RegexOptions.Singleline);
            Assert.True(contractBlockMatch.Success, "Contract array must exist in AshfallInputActions.cs");

            var rowMatches = Regex.Matches(
                contractBlockMatch.Groups["entries"].Value,
                @"new\s+InputActionContract\s*\(\s*(?<action>[A-Za-z0-9_]+)\s*,\s*InputScope\.(?<scope>[A-Za-z0-9_]+)\s*,\s*(?<route>""[^""]*""|null)\s*,\s*(?<rebind>true|false)\s*\)");

            // Resolve constants
            var constants = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (Match cm in Regex.Matches(content, @"public const string\s+(?<name>[A-Za-z0-9_]+)\s*=\s*""(?<val>[^""]+)"";"))
            {
                constants[cm.Groups["name"].Value] = cm.Groups["val"].Value;
            }

            foreach (Match m in rowMatches)
            {
                string actionIdentifier = m.Groups["action"].Value;
                string actionName = constants.TryGetValue(actionIdentifier, out var val) ? val : actionIdentifier.Trim('"');
                string scope = m.Groups["scope"].Value;
                string rawRoute = m.Groups["route"].Value;
                string? routeId = rawRoute == "null" ? null : rawRoute.Trim('"');
                bool rebindable = bool.Parse(m.Groups["rebind"].Value);

                entries.Add(new ContractEntry(actionName, scope, routeId, rebindable));
            }

            return entries;
        }

        [Fact]
        public void EveryProjectGodotAction_HasContractRow()
        {
            var godotActions = GetProjectGodotActions();
            var contractEntries = ParseContractEntries();
            var contractActionNames = new HashSet<string>(contractEntries.Select(e => e.Action), StringComparer.Ordinal);

            foreach (var action in godotActions)
            {
                Assert.True(
                    contractActionNames.Contains(action),
                    $"Action '{action}' is declared in project.godot [input] but is missing a row in AshfallInputActions.Contract.");
            }
        }

        [Fact]
        public void EveryContractRow_ExistsInProjectGodot()
        {
            var godotActions = GetProjectGodotActions();
            var contractEntries = ParseContractEntries();

            foreach (var entry in contractEntries)
            {
                Assert.True(
                    godotActions.Contains(entry.Action),
                    $"Contract row '{entry.Action}' is not declared in project.godot [input].");
            }
        }

        [Fact]
        public void EveryPredicate_HasCallSiteInSource()
        {
            string actionsCode = Read("src/Host/AshfallInputActions.cs");
            var predicateMatches = Regex.Matches(actionsCode, @"public static bool (Is[A-Za-z0-9_]+)\s*\(");
            var predicateNames = predicateMatches.Cast<Match>().Select(m => m.Groups[1].Value).Distinct().ToList();

            Assert.NotEmpty(predicateNames);

            // Collect all .cs files under src/ except AshfallInputActions.cs
            string repoRoot = RepoRoot();
            string srcDir = Path.Combine(repoRoot, "src");
            var csFiles = Directory.GetFiles(srcDir, "*.cs", SearchOption.AllDirectories)
                .Where(f => !Path.GetFileName(f).Equals("AshfallInputActions.cs", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var combinedSrc = string.Join("\n", csFiles.Select(File.ReadAllText));

            foreach (string pred in predicateNames)
            {
                Assert.True(
                    Regex.IsMatch(combinedSrc, $@"\b{pred}\b"),
                    $"Predicate '{pred}' in AshfallInputActions.cs has 0 call sites across src/ files.");
            }
        }

        [Fact]
        public void EveryContractRoute_ResolvesInPanelRegistry()
        {
            PanelRegistryBootstrap.RegisterAll();
            var contractEntries = ParseContractEntries();

            foreach (var entry in contractEntries)
            {
                if (entry.RouteId != null)
                {
                    Assert.True(
                        PanelRegistry.IsRegistered(entry.RouteId),
                        $"Action '{entry.Action}' specifies RouteId '{entry.RouteId}', but it is not registered in PanelRegistry.");
                }
            }
        }

        [Fact]
        public void CanonicalDefaults_HasNoWithinMapCollision()
        {
            string content = Read("src/Host/AshfallInputActions.cs");
            var defaultsBlockMatch = Regex.Match(content, @"public static readonly IReadOnlyDictionary<string, Key> CanonicalDefaults\s*=\s*new Dictionary<string, Key>\s*\{(?<pairs>.*?)\};", RegexOptions.Singleline);
            Assert.True(defaultsBlockMatch.Success, "CanonicalDefaults must exist in AshfallInputActions.cs");

            var matches = Regex.Matches(defaultsBlockMatch.Groups["pairs"].Value, @"\{\s*(?<action>[A-Za-z0-9_]+)\s*,\s*Key\.(?<key>[A-Za-z0-9_]+)\s*\}");
            var seenKeys = new Dictionary<string, string>(StringComparer.Ordinal);
            var collisions = new List<string>();

            foreach (Match m in matches)
            {
                string action = m.Groups["action"].Value;
                string key = m.Groups["key"].Value;

                if (seenKeys.TryGetValue(key, out var existingAction))
                {
                    collisions.Add($"Key '{key}' is assigned to both '{existingAction}' and '{action}'");
                }
                else
                {
                    seenKeys[key] = action;
                }
            }

            Assert.Empty(collisions);
        }

        [Fact]
        public void IsConfirmOrAccept_IsDeleted()
        {
            string content = Read("src/Host/AshfallInputActions.cs");
            Assert.DoesNotContain("IsConfirmOrAccept", content);
        }
    }
}
