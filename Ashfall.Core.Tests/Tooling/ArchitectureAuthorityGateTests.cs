// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Source-level ownership gates for the two consolidation results in this
    /// wave. These are intentionally small and cheap: they prevent a second
    /// gameplay authority from quietly returning during later feature work.
    /// </summary>
    public sealed class ArchitectureAuthorityGateTests
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

        [Fact]
        public void UtilityAi_CoreIsTheOnlyGameplayAuthority()
        {
            string root = RepoRoot();
            string coreDir = Path.Combine(root, "Assets", "Ashfall.Core", "UtilityAI");
            string hostDir = Path.Combine(root, "src", "UtilityAI");
            string core = ReadSources(coreDir);
            string host = ReadSources(hostDir);

            Assert.True(Directory.Exists(coreDir), "Core Utility-AI authority directory is missing");
            foreach (string type in new[] { "UtilityActionDef", "UtilityActionScorer", "UtilityAiSystem" })
            {
                int definitions = Regex.Matches(core, $@"\b(class|struct|record)\s+{type}\b").Count;
                Assert.Equal(1, definitions);
                Assert.DoesNotContain($"class {type}", host, StringComparison.Ordinal);
                Assert.DoesNotContain($"struct {type}", host, StringComparison.Ordinal);
            }

            Assert.DoesNotContain("Godot.", core, StringComparison.Ordinal);
            Assert.DoesNotContain("AtomicWar.", core, StringComparison.Ordinal);
            Assert.Contains("Ashfall.Core.UtilityAI", host, StringComparison.Ordinal);
            Assert.Contains("UtilityActionScorer", host, StringComparison.Ordinal);
            Assert.Contains("UtilityAiSystem", host, StringComparison.Ordinal);
            Assert.Contains("stateless", File.ReadAllText(Path.Combine(root, "docs", "utility_ai", "UTILITY_ACTION_SAVE_CONTRACT.md")), StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void WornGear_HasOneDefinition_AndNoInventoryRadiationBridge()
        {
            string root = RepoRoot();
            string coreRoot = Path.Combine(root, "Assets", "Ashfall.Core");
            string core = ReadSources(coreRoot);
            int definitions = Regex.Matches(core, @"\b(class|struct|record)\s+WornGear\b").Count;

            Assert.Equal(1, definitions);
            Assert.DoesNotContain("FromInventory", core, StringComparison.Ordinal);

            string radiation = File.ReadAllText(Path.Combine(coreRoot, "Radiation", "RadiationSystem.cs"));
            string inventory = File.ReadAllText(Path.Combine(coreRoot, "Inventory", "Inventory.cs"));
            string host = File.ReadAllText(Path.Combine(root, "src", "Host", "SurvivorsHostSession.cs"));
            Assert.Contains("using InventoryWornGear = Ashfall.Core.Inventory.WornGear", radiation, StringComparison.Ordinal);
            Assert.Contains("FillWornGear", inventory, StringComparison.Ordinal);
            Assert.Contains("FillWornGear", host, StringComparison.Ordinal);
        }

        private static string ReadSources(string directory)
        {
            if (!Directory.Exists(directory)) return string.Empty;
            return string.Join("\n", Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories)
                .OrderBy(path => path, StringComparer.Ordinal)
                .Select(File.ReadAllText));
        }
    }
}
