using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public class CampaignCalendarSourceGateTests
    {
        [Fact]
        public void SourceGate_MainPartialsDoNotDirectlyAssignSimDay()
        {
            string? root = Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(root) && !Directory.Exists(Path.Combine(root, "src")))
            {
                var parent = Directory.GetParent(root);
                root = parent?.FullName;
            }

            if (string.IsNullOrEmpty(root) || !Directory.Exists(Path.Combine(root, "src")))
                return; // Not running in repo tree

            var violations = new List<string>();

            // Pattern checking for direct mutations of _simDay (excluding declaration and lambda/comparison)
            var directAssignRegex = new Regex(@"\b_simDay\s*(\+\+|--|\+=|-=|\*=|/=|=(?!=|>))\s*", RegexOptions.Compiled);

            foreach (var file in Directory.GetFiles(Path.Combine(root, "src"), "Main*.cs", SearchOption.TopDirectoryOnly))
            {
                string[] lines = File.ReadAllLines(file);
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    // Skip read-only property definition: private int _simDay => ...
                    if (line.Contains("int _simDay =>")) continue;
                    // Skip comments
                    if (line.StartsWith("//") || line.StartsWith("/*") || line.StartsWith("*")) continue;

                    if (directAssignRegex.IsMatch(line))
                    {
                        violations.Add($"{Path.GetFileName(file)}:L{i + 1}: {line}");
                    }
                }
            }

            Assert.Empty(violations);
        }

        [Fact]
        public void SourceGate_UiPanelsDoNotDirectlyAdvanceDays()
        {
            string? root = Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(root) && !Directory.Exists(Path.Combine(root, "src", "UI")))
            {
                var parent = Directory.GetParent(root);
                root = parent?.FullName;
            }

            if (string.IsNullOrEmpty(root) || !Directory.Exists(Path.Combine(root, "src", "UI")))
                return;

            var violations = new List<string>();

            foreach (var file in Directory.GetFiles(Path.Combine(root, "src", "UI"), "*.cs", SearchOption.AllDirectories))
            {
                string[] lines = File.ReadAllLines(file);
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (line.StartsWith("//") || line.StartsWith("/*") || line.StartsWith("*")) continue;

                    if (line.Contains("AdvanceDays("))
                    {
                        violations.Add($"{Path.GetFileName(file)}:L{i + 1}: {line}");
                    }
                }
            }

            Assert.Empty(violations);
        }
    }
}
