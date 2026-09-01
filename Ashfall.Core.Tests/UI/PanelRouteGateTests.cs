using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    /// <summary>
    /// Gate: every panel id emitted by any UI surface (via Invoke calls in src/UI/*.cs
    /// and src/Main.GameFlow.cs) must have a registered descriptor in PanelRegistry.
    /// Also verifies the two previously-dead routes (codex, caregiving) and typed actions.
    /// </summary>
    public sealed class PanelRouteGateTests : IDisposable
    {
        public PanelRouteGateTests()
        {
            // Always register fresh — tests share the static registry between runs.
            // RegisterAll is idempotent so double-calling is safe.
            Ashfall.Core.UI.PanelRegistryBootstrap.RegisterAll();
        }

        public void Dispose() { /* registry is additive; no teardown needed */ }

        [Fact]
        public void Registry_PopulatesNonEmpty()
        {
            Assert.True(PanelRegistry.Count >= 60,
                $"Expected >= 60 registered panels, got {PanelRegistry.Count}");
        }

        [Fact]
        public void AllRegisteredDescriptors_HaveValidIds()
        {
            var badIds = PanelRegistry.AllIds
                .Where(id => string.IsNullOrWhiteSpace(id) || !Regex.IsMatch(id, @"^[a-z][a-z0-9_]*$"))
                .ToList();

            Assert.Empty(badIds);
        }

        [Fact]
        public void AllRegisteredDescriptors_HaveNonEmptyDisplayName()
        {
            var unnamed = PanelRegistry.AllIds
                .Select(id => PanelRegistry.Get(id)!)
                .Where(d => string.IsNullOrWhiteSpace(d.DisplayName))
                .Select(d => d.Id)
                .ToList();

            Assert.Empty(unnamed);
        }

        [Fact]
        public void KnownDashboardPanels_AreRegistered()
        {
            string[] coreIds =
            {
                "survivors", "inventory", "crafting", "medical", "expeditions",
                "weather", "radio", "map", "shelter", "factions", "quests",
                "journal", "trade", "muster", "expansions", "verdict",
                "duty_roster", "holdfast", "save", "research"
            };

            foreach (var id in coreIds)
            {
                Assert.True(PanelRegistry.IsRegistered(id),
                    $"Panel '{id}' (Dashboard group) is missing from PanelRegistry.");
                var d = PanelRegistry.Get(id)!;
                Assert.Equal(PanelGroup.Dashboard, d.Group);
            }
        }

        [Fact]
        public void CodexRoute_IsFormallyRegistered()
        {
            // Previously a dead route: emitted from MainMenuPanel.OnCodex but no
            // matching case in OpenPlayerPanel's switch.
            Assert.True(PanelRegistry.IsRegistered("codex"),
                "Panel 'codex' must be registered (MainMenu group, resolves to JournalBookUI).");
            var d = PanelRegistry.Get("codex")!;
            Assert.Equal(PanelGroup.MainMenu, d.Group);
            Assert.True(d.AvailableInMenu);
        }

        [Fact]
        public void CaregivingRoute_IsFormallyRegistered()
        {
            // Was handled in OpenExpandedPanel but missing from OpenPlayerPanel's
            // forwarding list — producing a silent no-op at runtime.
            Assert.True(PanelRegistry.IsRegistered("caregiving"),
                "Panel 'caregiving' must be registered (Expanded group).");
            var d = PanelRegistry.Get("caregiving")!;
            Assert.Equal(PanelGroup.Expanded, d.Group);
        }

        [Fact]
        public void AllExpandedPanels_AreRegisteredAsExpanded()
        {
            string[] expandedIds =
            {
                "water_treatment", "airlock_security", "survivor_relations", "regional_treaty",
                "vinyl_morale", "wildlife_trapping", "excavation", "apprenticeship",
                "caregiving", "shelter_thermal", "shelter_schedule", "shelter_decor", "autopsy_report",
                "waystation_network", "chemical_dependency", "sump_flooding", "decontamination",
                "kitchen_nutrition", "equipment_condition", "library_study", "archive_desk",
                "contractor_roster", "mental_health_crisis", "phantom_memory",
                "traveling_caravan", "medical_ward"
            };

            foreach (var id in expandedIds)
            {
                Assert.True(PanelRegistry.IsRegistered(id),
                    $"Expanded panel '{id}' is missing from PanelRegistry.");
                var d = PanelRegistry.Get(id)!;
                Assert.Equal(PanelGroup.Expanded, d.Group);
            }
        }

        [Fact]
        public void SecondaryAndMenuPanels_AreRegistered()
        {
            string[] secondaryIds =
            {
                "settings", "duty_roster_detail", "quest_detail", "faction_detail",
                "map_detail", "combat_detail", "combat_history"
            };

            foreach (var id in secondaryIds)
            {
                Assert.True(PanelRegistry.IsRegistered(id),
                    $"Secondary/menu panel '{id}' is missing from PanelRegistry.");
            }
        }

        [Fact]
        public void Resolve_UnknownId_ReturnsNullAndInvokesCallback()
        {
            string? capturedMsg = null;
            var result = PanelRegistry.Resolve("totally_nonexistent_panel_xyz", msg => capturedMsg = msg);

            Assert.Null(result);
            Assert.NotNull(capturedMsg);
            Assert.Contains("UNKNOWN ROUTE", capturedMsg);
            Assert.Contains("totally_nonexistent_panel_xyz", capturedMsg);
        }

        [Fact]
        public void Resolve_KnownId_ReturnsDescriptorWithoutInvokingCallback()
        {
            bool callbackFired = false;
            var result = PanelRegistry.Resolve("survivors", _ => callbackFired = true);

            Assert.NotNull(result);
            Assert.Equal("survivors", result!.Id);
            Assert.False(callbackFired, "Callback must not fire for a known id.");
        }

        [Fact]
        public void TryOpen_UnknownId_ReturnsFalseAndInvokesDiagnostic()
        {
            string? diag = null;
            bool opened = PanelRegistry.TryOpen("invalid_panel_route_999", msg => diag = msg);

            Assert.False(opened);
            Assert.NotNull(diag);
            Assert.Contains("UNKNOWN ROUTE", diag);
        }

        [Fact]
        public void TryOpen_BlockedInMenu_ReturnsFalseAndInvokesDiagnostic()
        {
            string? diag = null;
            // "inventory" is a gameplay panel, not available in menu
            bool opened = PanelRegistry.TryOpen("inventory", msg => diag = msg, isMenu: true);

            Assert.False(opened);
            Assert.NotNull(diag);
            Assert.Contains("BLOCKED ROUTE", diag);
            Assert.Contains("not available in main menu", diag);
        }

        [Fact]
        public void TryOpen_AllowedInMenu_OpensSuccessfully()
        {
            bool bound = false;
            bool opened = false;

            PanelRegistry.ConfigureActions("codex",
                bindAction: () => bound = true,
                openAction: () => opened = true);

            bool success = PanelRegistry.TryOpen("codex", isMenu: true);

            Assert.True(success);
            Assert.True(bound);
            Assert.True(opened);
        }

        [Fact]
        public void TryOpen_AvailabilityRule_GatesOpening()
        {
            bool canOpen = false;
            PanelRegistry.ConfigureActions("weather",
                availabilityRule: () => canOpen);

            string? diag = null;
            bool failed = PanelRegistry.TryOpen("weather", msg => diag = msg);
            Assert.False(failed);
            Assert.NotNull(diag);
            Assert.Contains("BLOCKED ROUTE", diag);

            canOpen = true;
            bool succeeded = PanelRegistry.TryOpen("weather");
            Assert.True(succeeded);
        }

        [Fact]
        public void TryClose_InvokesCloseAction()
        {
            bool closed = false;
            PanelRegistry.ConfigureActions("status",
                closeAction: () => closed = true);

            bool success = PanelRegistry.TryClose("status");
            Assert.True(success);
            Assert.True(closed);
        }

        [Fact]
        public void AllEmittedPanelIds_HaveRegisteredDescriptor()
        {
            // Source-scan all emitted panel IDs from the host src/ tree and
            // verify every one is registered. This is the structural CI gate.
            string srcRoot = FindSrcRoot();
            var emitted = ScanEmittedIds(srcRoot);

            var missing = emitted
                .Where(id => !PanelRegistry.IsRegistered(id))
                .OrderBy(id => id)
                .ToList();

            if (missing.Count > 0)
            {
                var list = string.Join(", ", missing.Select(id => $"'{id}'"));
                Assert.Fail(
                    $"The following panel IDs are emitted by UI code but have no registered descriptor: {list}. " +
                    $"Add them to PanelRegistryBootstrap.RegisterAll() and handle them in OpenPlayerPanel.");
            }
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        private static string FindSrcRoot()
        {
            string current = Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(current))
            {
                string candidate = Path.Combine(current, "src");
                if (Directory.Exists(candidate))
                    return candidate;
                string parent = Path.GetDirectoryName(current)!;
                if (parent == current) break;
                current = parent;
            }
            throw new DirectoryNotFoundException("Could not find src/ directory relative to test working directory.");
        }

        private static IReadOnlyList<string> ScanEmittedIds(string srcRoot)
        {
            var ids = new HashSet<string>(StringComparer.Ordinal);

            // Pattern: OnOpenPanelRequested?.Invoke("id"), OnOpenExpansionRequested?.Invoke("id"),
            // OpenPlayerPanel("id"), OpenExpandedPanel("id")
            var pattern = new Regex(
                @"(?:OnOpenPanelRequested|OnOpenExpansionRequested|OpenPlayerPanel|OpenExpandedPanel)\s*[\?\.]*\s*(?:Invoke)?\s*\(\s*""([a-z][a-z0-9_]*)""\s*\)",
                RegexOptions.Compiled);

            foreach (string file in Directory.EnumerateFiles(srcRoot, "*.cs", SearchOption.AllDirectories))
            {
                string content = File.ReadAllText(file);
                foreach (Match m in pattern.Matches(content))
                {
                    ids.Add(m.Groups[1].Value);
                }
            }

            return ids.OrderBy(id => id).ToList();
        }
    }
}
