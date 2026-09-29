// SPDX-License-Identifier: MIT
// A11Y-P1-INPUT-2026-09-29 — static drift gate for the UI accessibility P1
// input-correctness fixes (plan .ai/plans/ui-a11y-p1-input-correctness-2026-09-29.md,
// findings docs/ui/ACCESSIBILITY_REPORT_2026-09-29.md §9.1/§9.2/§9.6).
// The host layer is not unit-testable from Ashfall.Core.Tests; these gates pin
// the intentional source shape so silent regression fails CI. Pattern: MainTriadDriftGateTests.
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class UiA11yP1InputGateTests
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

        private static string Slice(string src, string startMarker, string endMarker)
        {
            int start = src.IndexOf(startMarker, StringComparison.Ordinal);
            Assert.True(start >= 0, $"marker not found: {startMarker}");
            int end = src.IndexOf(endMarker, start + 1, StringComparison.Ordinal);
            if (end < 0) end = src.Length;
            return src.Substring(start, end - start);
        }

        [Fact]
        public void MoralChoiceModal_UsesExclusiveOpenSeam()
        {
            // Raw keys 1–5 are shared with CombatPanel: opening the moral-choice
            // modal must close open overlay panels first (exclusive-open model).
            string body = Slice(
                ReadSrc("src", "Main.UiHandlers.cs"),
                "public void OpenMoralChoiceModal",
                "public void OpenFireIncidentPanel");
            int close = body.IndexOf("CloseAllOverlayPanels()", StringComparison.Ordinal);
            int open = body.IndexOf("_moralChoiceModal.Open()", StringComparison.Ordinal);
            Assert.True(close >= 0, "OpenMoralChoiceModal no longer calls CloseAllOverlayPanels — 1–5 key collision with CombatPanel returns.");
            Assert.True(open >= 0, "OpenMoralChoiceModal body changed shape; re-audit this gate.");
            Assert.True(close < open, "CloseAllOverlayPanels must run before _moralChoiceModal.Open().");
        }

        [Fact]
        public void JournalHotkey_IsPlayingGated()
        {
            // J must be inert outside GameState.Playing, like every sibling hotkey.
            string body = Slice(
                ReadSrc("src", "Main.Application.cs"),
                "AshfallInputActions.IsJournal(@event)",
                "AshfallInputActions.IsHelp(@event)");
            Assert.Contains("_state == GameState.Playing", body);
        }

        [Fact]
        public void CrisisHud_EscUsesUnhandledPhase()
        {
            // `_Input` preempted stacked modals: Esc must be handled in the
            // unhandled phase so tree order gives later siblings first claim.
            string src = ReadSrc("src", "UI", "EmergencyResponseHud.cs");
            Assert.Contains("public override void _UnhandledKeyInput(InputEvent @event)", src);
            Assert.DoesNotContain("public override void _Input(", src);
        }

        [Fact]
        public void CrisisHud_IsReraisedAbovePanels_AndNotCatalogued()
        {
            // The HUD must stay out of OverlayPanelCatalog (closing it on panel
            // switch would lose an active crisis) and be re-raised instead.
            string body = Slice(
                ReadSrc("src", "Main.PanelLifecycle.cs"),
                "private void CloseAllOverlayPanels",
                "private void CloseSettingsPanel");
            Assert.Contains("_crisisHud.MoveToFront()", body);

            string catalog = Slice(
                ReadSrc("src", "Main.PanelLifecycle.cs"),
                "OverlayPanelCatalog()",
                "private void CloseAllOverlayPanels");
            Assert.DoesNotContain("_crisisHud", catalog);
        }
    }
}
