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
        /// <summary>Recorded 2026-09-26 at 603 literals; re-recorded 2026-09-30 at the
        /// verified current count (612) after committed growth pushed past the old
        /// baseline; re-recorded 2026-10-01 at 617 after the T18 l10n repair
        /// localized the pilot ResearchPanel atlas tooltip (drift gate green);
        /// re-recorded again the same day at 619 after concurrent atlas/tooltip
        /// work landed before the full commit; re-recorded 2026-10-01 at the
        /// verified current count (625), then ratcheted down to 608 by localizing
        /// the 17 WorkshopPanel strings, then to 557 by localizing VisitorIntegration
        /// (13), ShelterHud (13), DutyRoster (13), and SilentFoundry (12), then to
        /// 546 by localizing SkillMatrix (11), then to 536 by localizing Triangulation (10),
        /// then to 528 by localizing the 5 remaining ExpeditionPanel literals and the
        /// StatusPanel expedition-injury row. Ratchet
        /// down, never up: a future l10n sweep should continue lowering this number.</summary>
        private const int HardcodedUiLiteralBaseline = 528;

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
