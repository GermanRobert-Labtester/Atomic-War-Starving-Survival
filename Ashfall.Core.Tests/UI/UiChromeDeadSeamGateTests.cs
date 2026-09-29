// SPDX-License-Identifier: MIT
// UI-CHROME-DEADSEAM-2026-09-29 — static drift gate for a11y pkg 13
// (plan .ai/plans/ui-chrome-deadseam-2026-09-29.md). Two classes of defect
// are sealed: (1) Godot light-default chrome for tooltips, separators, and
// factory-bypassing raw Labels — the theme entries below must stay inside
// AshfallUiTheme.Build(); (2) regrowth of the deleted parallel modal-open
// authority (ModalManager / ModalStackController) — ShowPanelLifecycle and
// AshfallFocusPolicy are the only sanctioned open/focus path.
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class UiChromeDeadSeamGateTests
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

        [Theory]
        [InlineData("TooltipPanel", "\"panel\", \"TooltipPanel\"")]
        [InlineData("TooltipLabel", "\"font_color\", \"TooltipLabel\"")]
        [InlineData("HSeparator", "\"separator\", \"HSeparator\"")]
        [InlineData("VSeparator", "\"separator\", \"VSeparator\"")]
        [InlineData("Label", "\"font_color\", \"Label\"")]
        public void ThemeCoversChromeTypes(string typeName, string requiredItem)
        {
            string src = ReadSrc("src", "UI", "AshfallUiTheme.cs");

            Assert.Contains(requiredItem, src);

            // Coverage must live inside Build() (installed at boot), not be
            // dead code appended elsewhere in the file.
            int build = src.IndexOf("private static Godot.Theme Build()", StringComparison.Ordinal);
            int item = src.IndexOf(requiredItem, StringComparison.Ordinal);
            Assert.True(item > build, $"{typeName} coverage must be inside Theme Build()");
        }

        [Fact]
        public void BaseLabelGetsBrandFontAndSize()
        {
            string src = ReadSrc("src", "UI", "AshfallUiTheme.cs");
            Assert.Contains("\"font_size\", \"Label\", DesignTheme.FontSizeBody", src);
            Assert.Contains("SetFontIfAvailable(theme, \"Label\", AshfallUiHelpers.FontBarlowRegular)", src);
        }

        private static string[] SourcesUnder(params string[] parts)
            => Directory.EnumerateFiles(Path.Combine(RepoRoot(), Path.Combine(parts)), "*.cs", SearchOption.AllDirectories)
                .ToArray();

        [Theory]
        [InlineData("ModalManager")]
        [InlineData("ModalStackController")]
        public void DeletedModalStackAuthorityDoesNotRegrow(string symbol)
        {
            string[] files = SourcesUnder("src").Concat(SourcesUnder("Assets", "Ashfall.Core")).ToArray();
            var offenders = files
                .Where(f => File.ReadAllText(f).Contains(symbol, StringComparison.Ordinal))
                .Select(f => Path.GetRelativePath(RepoRoot(), f))
                .OrderBy(f => f, StringComparer.Ordinal)
                .ToList();

            Assert.True(offenders.Count == 0,
                $"{symbol} regrew in: {string.Join(", ", offenders)}. " +
                "Panel opens must go through ShowPanelLifecycle; focus through AshfallFocusPolicy.");
        }

        [Fact]
        public void DeadStickRepeatTickRemoved()
        {
            foreach (string file in SourcesUnder("src"))
            {
                string src = File.ReadAllText(file);
                Assert.DoesNotContain("TickStickRepeat", src);
                Assert.DoesNotContain("StickRepeatCadenceSeconds", src);
            }
        }
    }
}
