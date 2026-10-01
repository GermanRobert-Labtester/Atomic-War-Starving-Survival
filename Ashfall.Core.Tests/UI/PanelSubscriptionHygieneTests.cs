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

        /// <summary>
        /// T11 regression (2026-10-01): MapPanel.Bind runs on every panel open
        /// (PanelRegistry bindAction), so it must drop the previous session
        /// subscriptions before subscribing again, and Unbind must remove every
        /// source Bind added — including the formerly leaked
        /// WastelandMap.OnMarkersChanged.
        /// </summary>
        [Fact]
        public void MapPanelBindIsResubscriptionSafe()
        {
            string srcRoot = FindSrcRoot();
            string mapPanelPath = Path.Combine(srcRoot, "UI", "MapPanel.cs");
            Assert.True(File.Exists(mapPanelPath), $"Could not find MapPanel.cs at {mapPanelPath}");
            string source = File.ReadAllText(mapPanelPath);

            string bindBody = ExtractBalancedBody(source, "public void Bind(");
            int firstSubscribe = bindBody.IndexOf("+=", StringComparison.Ordinal);
            Assert.True(firstSubscribe >= 0, "MapPanel.Bind must subscribe to its session events");
            int unbindCall = bindBody.IndexOf("Unbind()", StringComparison.Ordinal);
            Assert.True(unbindCall >= 0 && unbindCall < firstSubscribe,
                "MapPanel.Bind must call Unbind() before its first subscription: Bind is re-invoked on every " +
                "panel open, so additive subscription accumulates one RefreshView per accumulated re-bind");

            string unbindBody = ExtractBalancedBody(source, "public void Unbind()");
            foreach (string required in new[]
            {
                "_expeditions.StateChanged -= RefreshView",
                "_world.StateChanged -= RefreshView",
                "WastelandMap.OnMarkersChanged -= RefreshView",
                "_deepCoast.StateChanged -= RefreshView",
                "Warlord.OnStateChanged -= RefreshView",
            })
            {
                Assert.True(unbindBody.Contains(required, StringComparison.Ordinal),
                    $"MapPanel.Unbind must remove the '{required}' subscription that Bind adds");
            }
        }

        /// <summary>
        /// Extracts the balanced-brace body of the first method whose source
        /// contains <paramref name="signatureMarker"/>. Adequate for the
        /// marker methods asserted above, which contain no braces inside
        /// string literals.
        /// </summary>
        private static string ExtractBalancedBody(string source, string signatureMarker)
        {
            int sigIndex = source.IndexOf(signatureMarker, StringComparison.Ordinal);
            Assert.True(sigIndex >= 0, $"Method '{signatureMarker}' not found");
            int openIndex = source.IndexOf('{', sigIndex);
            Assert.True(openIndex >= 0, $"No open brace found after '{signatureMarker}'");
            int depth = 0;
            for (int i = openIndex; i < source.Length; i++)
            {
                if (source[i] == '{') depth++;
                else if (source[i] == '}')
                {
                    depth--;
                    if (depth == 0)
                        return source.Substring(openIndex, i - openIndex + 1);
                }
            }
            throw new InvalidOperationException($"Unbalanced braces in method '{signatureMarker}'");
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
