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
        private const int HardcodedUiLiteralBaseline = 488;

        /// <summary>Raw <c>Make*(...)</c> chrome literals across src/UI. The
        /// <c>Text=</c> ratchet above does not see chrome passed straight into
        /// <c>MakeButton</c>/<c>MakeTitle</c>/<c>MakeDataRow</c>/…, which is where
        /// the largest remaining localization surface hides. This surfaces that
        /// class and prevents growth while panels are localized. Ratchet down,
        /// never up.</summary>
        private const int RawChromeMakeLiteralBaseline = 1411;

        private static readonly Regex LiteralPattern = new(
            "(Text|Title|Label)\\s*=\\s*\"[A-Z][^\"]{6,}\"",
            RegexOptions.Compiled);

        private static readonly Regex MakeChromePattern = new(
            "(MakeMetadata|MakeDimLine|MakeSmall|MakeBody|MakeSectionHeader|MakeSubsectionHeader|MakeTitle|MakeButton|MakeDataRow|AddNavButton|MakeActionButton)\\(\"(?!ASHFALL)[A-Za-z]",
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

        /// <summary>
        /// Raw <c>Make*(...)</c> chrome literals across src/UI must not grow.
        /// Registered panels are already held to zero by
        /// <c>RegisteredPanels_HaveNoRawChromeLiteral</c>; this covers the
        /// unregistered panels whose chrome the <c>Text=</c> ratchet cannot see.
        /// </summary>
        [Fact]
        public void Raw_Make_Chrome_Literals_Do_Not_Grow()
        {
            string? uiDir = FindUiDirectory();
            Assert.NotNull(uiDir);

            int count = 0;
            foreach (string file in Directory.EnumerateFiles(uiDir!, "*.cs", SearchOption.TopDirectoryOnly))
                count += MakeChromePattern.Matches(File.ReadAllText(file)).Count;

            Assert.True(count <= RawChromeMakeLiteralBaseline,
                $"Raw Make*(...) chrome literals grew from {RawChromeMakeLiteralBaseline} to {count}. " +
                "Route new player-facing chrome through AshfallUiText/AshfallLocalization, or lower the " +
                "baseline in this test when you localize existing ones.");
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
