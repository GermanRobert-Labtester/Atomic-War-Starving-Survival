// SPDX-License-Identifier: MIT
// Localization readiness ratchet (forward-leap A5).
//
// The UI carries more hardcoded player-facing literals than the l10n table
// covers. This gate cannot make that better on its own — it makes it
// impossible to get worse: the count must never exceed the recorded baseline,
// so every new panel string either lands in assets/l10n or pays for its
// removal somewhere else.

using System;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    public sealed class LocalizationRatchetTests
    {
        /// <summary>Recorded 2026-09-26 (603 literals as counted by this test). Ratchet down, never up.</summary>
        private const int HardcodedUiLiteralBaseline = 603;

        private static readonly Regex LiteralPattern = new(
            "(Text|Title|Label)\\s*=\\s*\"[A-Z][^\"]{6,}\"",
            RegexOptions.Compiled);

        [Fact]
        public void Hardcoded_Ui_Literals_Do_Not_Grow()
        {
            string? uiDir = FindUiDirectory();
            Assert.NotNull(uiDir);

            int count = 0;
            foreach (string file in Directory.EnumerateFiles(uiDir!, "*.cs", SearchOption.TopDirectoryOnly))
            {
                string text = File.ReadAllText(file);
                count += LiteralPattern.Matches(text).Count;
            }

            Assert.True(count <= HardcodedUiLiteralBaseline,
                $"Hardcoded UI literals grew from {HardcodedUiLiteralBaseline} to {count}. " +
                "Route new player-facing strings through assets/l10n/strings.csv, or " +
                "lower the baseline in this test when you localize existing ones.");
        }

        [Fact]
        public void Ratchet_Baseline_Is_Plausible()
        {
            Assert.InRange(HardcodedUiLiteralBaseline, 100, 5000);
        }

        private static string? FindUiDirectory()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string candidate = Path.Combine(dir.FullName, "src", "UI");
                if (Directory.Exists(candidate) && File.Exists(Path.Combine(dir.FullName, "project.godot")))
                    return candidate;
                dir = dir.Parent;
            }
            return null;
        }
    }
}
