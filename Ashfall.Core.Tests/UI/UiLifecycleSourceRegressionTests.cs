// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public sealed class UiLifecycleSourceRegressionTests
    {
        [Fact]
        public void OverlayDetectionAndDismissal_UseTheSharedPanelCatalog()
        {
            string root = RepoRoot();
            string lifecycle = File.ReadAllText(Path.Combine(root, "src", "Main.PanelLifecycle.cs"));
            string gameFlow = File.ReadAllText(Path.Combine(root, "src", "Main.GameFlow.cs"));
            string closeAll = ExtractMethodBlock(lifecycle, "private void CloseAllOverlayPanels()");
            string anyOpen = ExtractMethodBlock(gameFlow, "private bool AnyOverlayPanelOpen()");

            Assert.Contains("private Control[] OverlayPanelCatalog()", lifecycle, StringComparison.Ordinal);
            Assert.Contains("foreach (Control panel in OverlayPanelCatalog())", closeAll, StringComparison.Ordinal);
            Assert.Contains("foreach (Control panel in OverlayPanelCatalog())", anyOpen, StringComparison.Ordinal);
        }

        [Fact]
        public void DutyRosterCallbacks_UseNamedIdempotentSubscriptions()
        {
            string root = RepoRoot();
            string uiPanels = File.ReadAllText(Path.Combine(root, "src", "Main.UiPanels.cs"));
            string buildUi = ExtractMethodBlock(uiPanels, "private void BuildUserInterface()");
            string bindCallbacks = ExtractMethodBlock(uiPanels, "private void BindDutyRosterPanelCallbacks()");

            Assert.Contains("BindDutyRosterPanelCallbacks();", buildUi, StringComparison.Ordinal);
            Assert.Contains("_dutyRosterPanel.OnAssignmentChanged -= HandleDutyRosterAssignmentChanged;", bindCallbacks, StringComparison.Ordinal);
            Assert.Contains("_dutyRosterPanel.OnAssignmentChanged += HandleDutyRosterAssignmentChanged;", bindCallbacks, StringComparison.Ordinal);
            Assert.Contains("_dutyRosterPanel.OnDetailsRequested -= HandleDutyRosterDetailsRequested;", bindCallbacks, StringComparison.Ordinal);
            Assert.Contains("_dutyRosterPanel.OnDetailsRequested += HandleDutyRosterDetailsRequested;", bindCallbacks, StringComparison.Ordinal);
            Assert.DoesNotContain("=>", bindCallbacks, StringComparison.Ordinal);
            Assert.DoesNotContain("_dutyRosterPanel.OnAssignmentChanged += () =>", uiPanels, StringComparison.Ordinal);
            Assert.DoesNotContain("_dutyRosterPanel.OnDetailsRequested += () =>", uiPanels, StringComparison.Ordinal);
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
