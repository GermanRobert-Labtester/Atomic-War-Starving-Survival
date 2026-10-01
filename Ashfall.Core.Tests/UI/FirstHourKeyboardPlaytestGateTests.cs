// SPDX-License-Identifier: MIT
// First-hour keyboard/triage regression gate (T01, 2026-10-01).
//
// Two live defects found while playing the first-hour journey in its strongest
// automated keyboard-only form (docs/alpha/FIRST_HOUR_PLAYTEST_KIT.md):
//
//  1. The `inventory` route's player-facing overlay never subscribed
//     OnItemSelected, so every row's SELECT button was a dead affordance and
//     the only consume path (food.ration_consumed) was unreachable.
//  2. DutyRosterPanel raised OnAssignmentChanged from row *selection*, while
//     the real assignment mutations raised it never — so the Duty onboarding
//     stage completed from selecting a row (Enter) and a genuine assignment
//     produced no signal.
//
// These are source contracts, not runtime assertions: the host panels are
// Godot Controls, so the invariants are pinned at the wiring seam exactly like
// UiLifecycleSourceRegressionTests does for the shared overlay catalog.
using System;
using System.IO;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public sealed class FirstHourKeyboardPlaytestGateTests
    {
        [Fact]
        public void InventoryOverlay_ItemSelection_IsWiredToTheHostDetailRoute()
        {
            string root = RepoRoot();
            string uiPanels = File.ReadAllText(Path.Combine(root, "src", "Main.UiPanels.cs"));

            // The player-facing overlay is the panel the Food stage's
            // "Show me where" route opens; its SELECT must reach the same
            // detail/consume seam the legacy right-column widget used.
            Assert.Contains("_inventoryOverlay.OnItemSelected += OnInventoryItemSelected;",
                uiPanels, StringComparison.Ordinal);
        }

        [Fact]
        public void DutyRoster_AssignmentChanged_FiresFromMutationNotRowSelection()
        {
            string root = RepoRoot();
            string panel = File.ReadAllText(Path.Combine(root, "src", "UI", "DutyRosterPanel.cs"));

            string rowSelected = ExtractMethodBlock(panel, "private void HandleRowSelected(int idx)");
            Assert.DoesNotContain("OnAssignmentChanged", rowSelected, StringComparison.Ordinal);

            foreach (string signature in new[]
                     {
                         "private void TryAssign(string roleId, string survivorId)",
                         "private void ConfirmPendingAssignment(string roleId, string survivorId)",
                     })
            {
                string mutation = ExtractMethodBlock(panel, signature);
                Assert.Contains("OnAssignmentChanged?.Invoke();", mutation, StringComparison.Ordinal);
            }

            string assignSection = ExtractMethodBlock(panel,
                "private void RenderAssignSection(string roleId, string currentSurvivorId)");
            Assert.Contains("if (result.IsSuccess) OnAssignmentChanged?.Invoke();",
                assignSection, StringComparison.Ordinal);
        }

        private static string ExtractMethodBlock(string source, string signature)
        {
            int signatureStart = source.IndexOf(signature, StringComparison.Ordinal);
            if (signatureStart < 0)
                throw new InvalidOperationException($"Could not find method signature '{signature}'.");

            int bodyStart = source.IndexOf('{', signatureStart);
            if (bodyStart < 0)
                throw new InvalidOperationException($"Could not find method body for '{signature}'.");

            int depth = 0;
            for (int i = bodyStart; i < source.Length; i++)
            {
                if (source[i] == '{')
                    depth++;
                else if (source[i] == '}' && --depth == 0)
                    return source.Substring(bodyStart, i - bodyStart + 1);
            }

            throw new InvalidOperationException($"Could not find the end of method body for '{signature}'.");
        }

        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "Ashfall.slnx")) ||
                    Directory.Exists(Path.Combine(dir.FullName, "Assets", "Ashfall.Core")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            return Directory.GetCurrentDirectory();
        }
    }
}
