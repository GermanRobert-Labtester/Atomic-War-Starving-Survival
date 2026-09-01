using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Ashfall.Core.Random;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public class CampaignRngSourceGateTests
    {
        [Fact]
        public void CanonicalStreamIds_AreNonEmptyUniqueAndLowerSnakeCase()
        {
            var fields = typeof(CampaignStreamIds).GetFields(BindingFlags.Public | BindingFlags.Static);
            var seen = new HashSet<string>(StringComparer.Ordinal);

            foreach (var f in fields)
            {
                string? id = f.GetValue(null) as string;
                Assert.False(string.IsNullOrWhiteSpace(id));
                Assert.DoesNotContain(id!, seen);
                seen.Add(id!);
                Assert.True(Regex.IsMatch(id!, @"^[a-z_]+$"), $"Stream ID '{id}' must be lower snake_case.");
            }
        }

        [Fact]
        public void SourceGate_MainPartialsDoNotConstructHardcodedSeededRng()
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
            var hardcodedRngRegex = new Regex(@"new\s+(?:Ashfall\.Core\.)?SeededRng\s*\(\s*(?:1986|2026|MoralChoiceSeed)\s*\)", RegexOptions.Compiled);

            // Production partials that should use CampaignRngManager streams
            string[] productionPartials = new[]
            {
                "Main.MoralChoice.cs",
                "Main.ShelterBatch3.cs",
                "Main.ShelterSocial.cs",
                "Main.SurvivorSocial.cs"
            };

            foreach (var fileName in productionPartials)
            {
                string path = Path.Combine(root, "src", fileName);
                if (!File.Exists(path)) continue;

                string[] lines = File.ReadAllLines(path);
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (line.StartsWith("//") || line.StartsWith("/*") || line.StartsWith("*")) continue;

                    if (hardcodedRngRegex.IsMatch(line))
                    {
                        violations.Add($"{fileName}:L{i + 1}: {line}");
                    }
                }
            }

            Assert.Empty(violations);
        }
    }
}
