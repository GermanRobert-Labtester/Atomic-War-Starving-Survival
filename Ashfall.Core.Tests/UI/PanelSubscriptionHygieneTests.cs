// SPDX-License-Identifier: MIT
// ASHFALL CI Gate: Panel Event Subscription Lifecycle Hygiene (REM-005 / R09).
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public sealed class PanelSubscriptionHygieneTests
    {
        private static readonly Regex LambdaUnsubscribeRegex = new(
            @"-=\s*(?:\w+|\([^)]*\))\s*=>",
            RegexOptions.Compiled);

        [Fact]
        public void NoLambdaUnsubscriptionsInUiPanels()
        {
            string srcRoot = FindSrcRoot();
            string uiDir = Path.Combine(srcRoot, "UI");
            Assert.True(Directory.Exists(uiDir), $"Could not find UI directory at {uiDir}");

            var csFiles = Directory.GetFiles(uiDir, "*.cs", SearchOption.AllDirectories);
            Assert.NotEmpty(csFiles);

            var violations = new System.Collections.Generic.List<string>();

            foreach (var file in csFiles)
            {
                string[] lines = File.ReadAllLines(file);
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (line.StartsWith("//") || line.StartsWith("/*")) continue;

                    if (LambdaUnsubscribeRegex.IsMatch(line))
                    {
                        violations.Add($"{Path.GetFileName(file)}: line {i + 1}: {line}");
                    }
                }
            }

            Assert.True(violations.Count == 0,
                $"Found {violations.Count} prohibited lambda unsubscription(s) in src/UI. Unsubscribing a newly created lambda is a no-op that causes event handler leaks. Store the delegate in a field.\n" +
                string.Join("\n", violations));
        }

        private static string FindSrcRoot()
        {
            string current = Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(current))
            {
                string candidate = Path.Combine(current, "src");
                if (Directory.Exists(candidate))
                    return candidate;
                string parent = Path.GetDirectoryName(current)!;
                if (parent == current) break;
                current = parent;
            }
            throw new DirectoryNotFoundException("Could not locate src/ directory from " + Directory.GetCurrentDirectory());
        }
    }
}
