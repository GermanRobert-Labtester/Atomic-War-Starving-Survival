// SPDX-License-Identifier: MIT
// UI-BRIEFING-DEEPLINK-2026-09-29 — static drift gate for a11y pkg 14
// (plan .ai/plans/ui-briefing-deeplinks-2026-09-29.md). DailyBriefingModal
// RTL [url] links are mouse-only by engine limitation (no per-link focus);
// the keyboard/controller path is the HFlowContainer button row rebuilt per
// report. If that seam disappears, deep links become mouse-only again.
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class UiBriefingDeepLinkGateTests
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

        [Fact]
        public void BriefingModalHasKeyboardDeepLinkRow()
        {
            string src = File.ReadAllText(Path.Combine(RepoRoot(), "src", "UI", "DailyBriefingModal.cs"));

            // The link row exists, starts hidden, and sits in the briefing layout.
            Assert.Contains("private HFlowContainer _linkRow", src);
            Assert.Contains("_linkRow.Visible = false;", src);
            Assert.Contains("_linkRow.Visible = added > 0;", src);

            // Links are rebuilt per report from unique DeepLinkRoute values.
            Assert.Contains("private void RebuildDeepLinkRow(DailyBriefingReport report)", src);
            Assert.Contains("new HashSet<string>(StringComparer.Ordinal)", src);
            Assert.Contains("entry?.DeepLinkRoute ?? string.Empty", src);

            // Buttons ride the same dispatch seam as RTL MetaClicked.
            Assert.Contains("() => OnDeepLinkRequested?.Invoke(captured)", src);
            Assert.Contains("_bodyLabel.MetaClicked += OnMetaClicked;", src);

            // Overflow cap is a real constant, not a magic number.
            Assert.Contains("private const int MaxDeepLinkButtons = 8;", src);
            Assert.Contains("added >= MaxDeepLinkButtons", src);
        }
    }
}
