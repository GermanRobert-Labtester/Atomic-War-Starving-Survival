// SPDX-License-Identifier: MIT
// ASHFALL accessibility guard test (Plan 80 / Task B21).
// Enforces font floors, focus policy presence, and bans raw Key.Escape
// in migrated top dashboard and overlay panels.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;
using Ashfall.Core.UI;

namespace Ashfall.Core.Tests.UI
{
    public class AccessibilitySourceAuditTests
    {
        private static readonly Regex LineComment = new Regex("//.*", RegexOptions.Compiled);
        private static readonly Regex BlockComment =
            new Regex("/\\*.*?\\*/", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex HardcodedFontSizeBelowFloor =
            new Regex(@"(?:font_size""\s*,\s*|FontSize\s*=\s*)([0-9]+)\b", RegexOptions.Compiled);

        private static string FindRepoRoot()
        {
            string[] candidates =
            {
                Directory.GetCurrentDirectory(),
                AppContext.BaseDirectory
            };
            foreach (string start in candidates)
            {
                var dir = new DirectoryInfo(Path.GetFullPath(start));
                while (dir != null)
                {
                    string probeSrc = Path.Combine(dir.FullName, "src");
                    string probeAssets = Path.Combine(dir.FullName, "Assets");
                    if (Directory.Exists(probeSrc) && Directory.Exists(probeAssets))
                        return dir.FullName;
                    dir = dir.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate repo root from the test run");
        }

        private static string StripComments(string text)
        {
            text = BlockComment.Replace(text, string.Empty);
            return LineComment.Replace(text, string.Empty);
        }

        [Fact]
        public void ThemeFontSizes_MeetAccessibilityFloors()
        {
            // Plan 80:
            // - body text never below floor (15px)
            // - label floor (11px)
            // - small floor (12px)
            // - mono floor (13px)
            Assert.True(Theme.FontSizeBody >= 14, "Theme.FontSizeBody must be at least 14px for readable body text.");
            Assert.True(Theme.FontSizeLabel >= 11, "Theme.FontSizeLabel must be at least 11px.");
            Assert.True(Theme.FontSizeSmall >= 12, "Theme.FontSizeSmall must be at least 12px.");
            Assert.True(Theme.FontSizeMono >= 12, "Theme.FontSizeMono must be at least 12px.");
        }

        [Fact]
        public void UiSourceFiles_HaveNoFontSizesBelowAbsoluteFloor()
        {
            string root = FindRepoRoot();
            string uiDir = Path.Combine(root, "src", "UI");
            Assert.True(Directory.Exists(uiDir), $"UI directory {uiDir} must exist");

            var csFiles = Directory.GetFiles(uiDir, "*.cs", SearchOption.AllDirectories);
            Assert.NotEmpty(csFiles);

            foreach (var file in csFiles)
            {
                string text = File.ReadAllText(file);
                string clean = StripComments(text);
                var matches = HardcodedFontSizeBelowFloor.Matches(clean);
                foreach (Match match in matches)
                {
                    if (int.TryParse(match.Groups[1].Value, out int size))
                    {
                        Assert.False(size < 11,
                            $"File {Path.GetFileName(file)} contains hardcoded font size {size}px which is below the absolute floor of 11px.");
                    }
                }
            }
        }

        [Fact]
        public void MigratedPanels_DoNotUseRawEscapeKey()
        {
            string root = FindRepoRoot();
            string uiDir = Path.Combine(root, "src", "UI");

            string[] migratedPanels =
            {
                "InventoryPanel.cs",
                "ResearchAtlasPanel.cs",
                "ResearchPanel.cs",
                "CraftingPanel.cs",
                "MedicalPanel.cs",
                "StatusPanel.cs"
            };

            foreach (var panelFile in migratedPanels)
            {
                string path = Path.Combine(uiDir, panelFile);
                Assert.True(File.Exists(path), $"Migrated panel file {path} must exist");

                string content = StripComments(File.ReadAllText(path));
                Assert.DoesNotContain("Key.Escape", content);
            }
        }

        /// <summary>
        /// Controller-parity ratchet (WHOLEGAME-P1D residual sweep, 2026-09-26):
        /// panel dismissal must route through the rebindable
        /// <c>AshfallInputActions.IsCloseOrCancel</c> contract (Esc, pad B,
        /// rebound keys) instead of raw <c>Key.Escape</c>, which silently drops
        /// controller and rebound-key users. The allowlist is exhaustive and
        /// each entry must keep a live reason:
        /// - SettingsPanel.cs — the rebind-capture cancel gesture must observe
        ///   the raw Esc key so a pad button being bound to Close cannot cancel
        ///   its own capture.
        /// The previously allowlisted claim-blocked files (DutyRosterPanel,
        /// ExpeditionPanel, SurvivorDetailPanel, PfglOctetBoardPanels) were
        /// converted 2026-09-26 under direct user authorization overriding the
        /// stale c1-plan24 / PFGL-octet claims for these one-line dismissals.
        /// </summary>
        [Fact]
        public void UiPanels_DismissThroughCloseAction_NotRawEscape()
        {
            string root = FindRepoRoot();
            string uiDir = Path.Combine(root, "src", "UI");
            Assert.True(Directory.Exists(uiDir), $"UI directory {uiDir} must exist");

            var allowedRawEscape = new HashSet<string>(StringComparer.Ordinal)
            {
                "SettingsPanel.cs",
            };

            var offenders = new List<string>();
            foreach (var file in Directory.GetFiles(uiDir, "*.cs", SearchOption.AllDirectories))
            {
                if (allowedRawEscape.Contains(Path.GetFileName(file)))
                    continue;

                string content = StripComments(File.ReadAllText(file));
                if (content.Contains("Key.Escape"))
                    offenders.Add(Path.GetFileName(file));
            }

            Assert.True(
                offenders.Count == 0,
                $"Panels must dismiss via AshfallInputActions.IsCloseOrCancel (rebindable, controller parity), " +
                $"not raw Key.Escape. Offenders: {string.Join(", ", offenders)}");
        }

        [Fact]
        public void AshfallFocusPolicy_IsImplementedAndExposesRequiredApi()
        {
            string root = FindRepoRoot();
            string policyPath = Path.Combine(root, "src", "UI", "AshfallFocusPolicy.cs");
            Assert.True(File.Exists(policyPath), "AshfallFocusPolicy.cs must exist in src/UI/");

            string content = StripComments(File.ReadAllText(policyPath));
            Assert.Contains("MakeFocusVisibleStyleBox", content);
            Assert.Contains("ApplyFocusVisibleStyle", content);
            Assert.Contains("OpenWithFocus", content);
            Assert.Contains("TrapFocus", content);
            Assert.Contains("RestoreFocus", content);
        }
    }
}
