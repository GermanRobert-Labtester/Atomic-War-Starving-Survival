// SPDX-License-Identifier: MIT
// ASHFALL CI Source Gate: Production UI Purity & No Fabricated Fallbacks (Task 107).
// Enforces that opening or interacting with production UI panels in src/UI/ can never
// silently create gameplay content, domain definitions, or synthetic entity IDs.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public sealed class ProductionUiNoFabricatedFallbackGateTests
    {
        private static readonly Regex LineComment = new Regex(@"//.*$", RegexOptions.Multiline | RegexOptions.Compiled);
        private static readonly Regex BlockComment = new Regex(@"/\*.*?\*/", RegexOptions.Singleline | RegexOptions.Compiled);

        private static string FindUiRoot()
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
                    string probe = Path.Combine(dir.FullName, "src", "UI");
                    if (Directory.Exists(probe))
                        return probe;
                    dir = dir.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate src/UI from the test run");
        }

        private static string StripComments(string code)
        {
            code = BlockComment.Replace(code, string.Empty);
            return LineComment.Replace(code, string.Empty);
        }

        private static List<(string FilePath, string RelativePath, string StrippedCode)> LoadUiSourceFiles()
        {
            string uiRoot = FindUiRoot();
            var files = Directory.EnumerateFiles(uiRoot, "*.cs", SearchOption.AllDirectories)
                .Where(f => !f.Replace('\\', '/').Contains("/obj/") && !f.Replace('\\', '/').Contains("/bin/"))
                .OrderBy(f => f)
                .ToList();

            var list = new List<(string FilePath, string RelativePath, string StrippedCode)>(files.Count);
            foreach (var f in files)
            {
                string rel = Path.GetRelativePath(uiRoot, f).Replace('\\', '/');
                string text = File.ReadAllText(f);
                string stripped = StripComments(text);
                list.Add((f, rel, stripped));
            }
            return list;
        }

        [Fact]
        public void ProductionUi_ContainsNoDomainDefinitionInstantiations()
        {
            var files = LoadUiSourceFiles();
            var pattern = new Regex(@"new\s+([A-Za-z0-9_]*Definition)\s*[\(\{]", RegexOptions.Compiled);
            var violations = new List<string>();

            foreach (var (filePath, rel, stripped) in files)
            {
                var lines = stripped.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    var match = pattern.Match(lines[i]);
                    if (match.Success)
                    {
                        violations.Add($"{rel}:{i + 1} -> {match.Value.Trim()}");
                    }
                }
            }

            Assert.True(violations.Count == 0,
                $"Production UI files must not instantiate domain *Definition objects (definitions belong in JSON data authority):\n" +
                string.Join("\n", violations));
        }

        private static readonly Regex StringLiteral = new Regex(@"""(\\""|[^""])*""", RegexOptions.Compiled);

        [Fact]
        public void ProductionUi_ContainsNoDemoMethodInvocations()
        {
            var files = LoadUiSourceFiles();
            var memberDemoPattern = new Regex(@"\.(?:[A-Za-z0-9_]*Demo|Demo[A-Za-z0-9_]*)\b", RegexOptions.Compiled);
            var varDemoPattern = new Regex(@"\b(?:[A-Za-z0-9_]*Demo|demo[A-Za-z0-9_]*)\b", RegexOptions.Compiled);
            var violations = new List<string>();

            string[] allowedWords = { "democracy", "democratic", "demographic", "demolish", "demonstrate", "demands" };

            foreach (var (filePath, rel, stripped) in files)
            {
                var lines = stripped.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    // Strip string literals to avoid false positives on user-facing prose
                    string codeOnly = StringLiteral.Replace(line, "\"\"");
                    if (allowedWords.Any(w => codeOnly.IndexOf(w, StringComparison.OrdinalIgnoreCase) >= 0))
                        continue;

                    var m1 = memberDemoPattern.Match(codeOnly);
                    var m2 = varDemoPattern.Match(codeOnly);
                    if (m1.Success)
                    {
                        violations.Add($"{rel}:{i + 1} [member] -> {m1.Value.Trim()} in: {line.Trim()}");
                    }
                    else if (m2.Success)
                    {
                        violations.Add($"{rel}:{i + 1} [var] -> {m2.Value.Trim()} in: {line.Trim()}");
                    }
                }
            }

            Assert.True(violations.Count == 0,
                $"Production UI files must not invoke or declare *Demo methods or variables:\n" +
                string.Join("\n", violations));
        }

        [Fact]
        public void ProductionUi_ContainsNoDirectHostStateGenerators()
        {
            var files = LoadUiSourceFiles();
            var generatorPattern = new Regex(@"_host\.(?:GenerateOffer|RegisterItem|AddNode|AddSite|TriggerCrisis|QueueCase|SpawnCaravan|LoadCatalog|LoadInkCatalog)\s*\(", RegexOptions.Compiled);
            var violations = new List<string>();

            foreach (var (filePath, rel, stripped) in files)
            {
                var lines = stripped.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    var match = generatorPattern.Match(lines[i]);
                    if (match.Success)
                    {
                        violations.Add($"{rel}:{i + 1} -> {match.Value.Trim()}");
                    }
                }
            }

            Assert.True(violations.Count == 0,
                $"Production UI files must not call domain state generation methods on host sessions:\n" +
                string.Join("\n", violations));
        }

        [Fact]
        public void ProductionUi_ContainsNoFabricatedEntityIds()
        {
            var files = LoadUiSourceFiles();
            var fakeIdsPattern = new Regex(@"\""(?:survivor_dweller_|caregiver_a|patient_b|specimen_survivor_01|Master_Blacksmith|Teen_Dweller_01)", RegexOptions.Compiled);
            var violations = new List<string>();

            foreach (var (filePath, rel, stripped) in files)
            {
                var lines = stripped.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    var match = fakeIdsPattern.Match(lines[i]);
                    if (match.Success)
                    {
                        violations.Add($"{rel}:{i + 1} -> {match.Value.Trim()} in: {lines[i].Trim()}");
                    }
                }
            }

            Assert.True(violations.Count == 0,
                $"Production UI files must not contain hardcoded fabricated entity IDs:\n" +
                string.Join("\n", violations));
        }
    }
}
