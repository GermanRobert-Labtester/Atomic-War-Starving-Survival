// SPDX-License-Identifier: MIT
// A11Y-ACCENT-TOKENS-2026-09-29 — static drift gate for the remaining
// hardcoded-accent sweep (plan .ai/plans/ui-a11y-accent-tokens-2026-09-29.md,
// findings docs/ui/ACCESSIBILITY_REPORT_2026-09-29.md §2b/§2c).
// Pattern: MainTriadDriftGateTests / UiA11yP1InputGateTests.
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class UiA11yAccentTokenGateTests
    {
        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (Directory.Exists(Path.Combine(dir, "src")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        private static string ReadSrc(params string[] parts)
            => File.ReadAllText(Path.Combine(new[] { RepoRoot() }.Concat(parts).ToArray()));

        // Every accent literal consolidated in the 2026-09-29 package must not
        // return. If a new panel needs one of these hues, use
        // AshfallUiHelpers.ToColor(DesignTheme.X) instead.
        [Theory]
        [InlineData("EmergencyResponseHud")]
        [InlineData("SaveLoadPanel")]
        [InlineData("SurvivorDeathLegacyPanel")]
        [InlineData("TimeCapsulePanel")]
        [InlineData("RelationshipDecayPanel")]
        [InlineData("PersonalQuestPanel")]
        [InlineData("ShelterPanel")]
        public void AccentedPanels_UseCanonicalTokens(string fileName)
        {
            string src = ReadSrc("src", "UI", $"{fileName}.cs");

            var regex = new Regex(
                @"new Color\((0\.6f, 0\.6f, 0\.6f|0\.7f, 0\.7f, 0\.7f|0\.8f, 0\.8f, 0\.8f|0\.85f, 0\.85f, 0\.85f|0\.9f, 0\.9f, 0\.9f|1f, 0\.3f, 0\.3f|1f, 0\.4f, 0\.4f|1f, 0\.45f, 0\.4f|1f, 0\.5f, 0\.5f|1f, 0\.6f, 0\.2f|1f, 0\.85f, 0\.3f|0\.95f, 0\.5f, 0\.5f|0\.95f, 0\.5f, 0\.4f|0\.95f, 0\.85f, 0\.4f|0\.9f, 0\.7f, 0\.4f|0\.53f, 1f, 0\.67f|0\.4f, 0\.9f, 0\.5f|0\.4f, 0\.85f, 0\.95f|0\.5f, 0\.8f, 0\.9f|0\.83f, 0\.67f, 0\.38f|0\.43f, 0\.64f, 0\.66f|0\.58f, 0\.56f, 0\.52f)\)",
                RegexOptions.Compiled);
            Assert.False(regex.IsMatch(src),
                $"{fileName}.cs reintroduced a hand-rolled accent literal; use " +
                "AshfallUiHelpers.ToColor(DesignTheme.X) instead.");

            Assert.Contains("using DesignTheme = Ashfall.Core.UI.Theme;", src);
        }
    }
}
