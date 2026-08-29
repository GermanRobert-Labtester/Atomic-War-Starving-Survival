using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public class ConsequenceLedgerSourceGateTests
    {
        [Fact]
        public void SourceGate_NoProductionPrivateInMemoryFlagLedgerConstruction()
        {
            string? root = Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(root) && !Directory.Exists(Path.Combine(root, "src")))
            {
                var parent = Directory.GetParent(root);
                root = parent?.FullName;
            }

            if (string.IsNullOrEmpty(root) || !Directory.Exists(Path.Combine(root, "src")))
                return; // Not running inside repo tree

            var violations = new List<string>();
            var forbiddenPattern = new Regex(@"new\s+(?:Ashfall\.Core\.Flags\.)?InMemoryFlagLedger\s*\(", RegexOptions.Compiled);

            foreach (var file in Directory.GetFiles(Path.Combine(root, "src"), "*.cs", SearchOption.AllDirectories))
            {
                string relative = Path.GetRelativePath(root, file);
                string[] lines = File.ReadAllLines(file);
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (line.StartsWith("//") || line.StartsWith("/*") || line.StartsWith("*"))
                        continue;

                    if (forbiddenPattern.IsMatch(line))
                    {
                        violations.Add($"{relative}:L{i + 1}: {line}");
                    }
                }
            }

            Assert.Empty(violations);
        }
    }
}
