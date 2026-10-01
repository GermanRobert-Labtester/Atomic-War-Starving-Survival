// SPDX-License-Identifier: MIT
// ASHFALL UI Gate: Difficulty Settings Panel route/wiring contract.
// Closes DEBT-PLAN181-DIFFICULTY-RUNTIME-UI: the host commands and the
// 12-check probe already existed, but no player-navigable surface rendered
// them. This gate pins the route, the surface manifest contract, and the host
// binding so the panel cannot silently disappear from the router.
using System.IO;
using System.Linq;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public sealed class DifficultySettingsPanelRouteTests
    {
        public DifficultySettingsPanelRouteTests()
        {
            PanelRegistryBootstrap.RegisterAll();
        }

        [Fact]
        public void DifficultySettingsRoute_IsRegisteredInPanelRegistry()
        {
            Assert.True(PanelRegistry.IsRegistered("difficulty_settings"),
                "Panel 'difficulty_settings' must be registered in PanelRegistry.");

            var desc = PanelRegistry.Get("difficulty_settings");
            Assert.NotNull(desc);
            Assert.Equal("difficulty_settings", desc!.Id);
            Assert.Equal(PanelGroup.Expanded, desc.Group);
            Assert.Equal(PanelMaturity.Live, desc.Maturity);
            Assert.True(desc.IsPlayerNavigable, "difficulty_settings must be player navigable.");
            Assert.Contains("difficulty_settings", desc.SetupDependencies);
        }

        [Fact]
        public void DifficultySettingsRoute_IsIncludedInPlayerSurfaceManifest()
        {
            var manifest = PlayerSurfaceManifest.Generate();
            Assert.NotNull(manifest);

            var contract = manifest.Contracts.FirstOrDefault(c => c.PanelId == "difficulty_settings");
            Assert.NotNull(contract);
            Assert.Equal(SurfaceRouteKind.ExpandedShelter, contract!.RouteKind);
            Assert.Equal(SurfaceRenderCoverage.ProductionRendered, contract.RenderCoverage);
            Assert.Contains("difficulty_settings", contract.SetupDependencies);
        }

        [Fact]
        public void DifficultySettingsRoute_HostBindingAndPanelSourceAreWired()
        {
            string? root = Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(root) && !Directory.Exists(Path.Combine(root, "src", "UI")))
            {
                var parent = Directory.GetParent(root);
                root = parent?.FullName;
            }
            if (string.IsNullOrEmpty(root) || !Directory.Exists(Path.Combine(root, "src", "UI")))
                return; // Not running in repo tree

            string surfaces = File.ReadAllText(Path.Combine(root, "src", "Main.PlayerSurfaces.cs"));
            Assert.Contains("\"difficulty_settings\"", surfaces);

            string expanded = File.ReadAllText(Path.Combine(root, "src", "Main.ExpandedShelterSystems.cs"));
            Assert.Contains("case \"difficulty_settings\"", expanded);
            Assert.Contains("SetupDifficultySettingsPanel()", expanded);

            string host = File.ReadAllText(Path.Combine(root, "src", "Main.DifficultySettings.cs"));
            Assert.Contains("SetupDifficultySettingsPanel", host);
            Assert.Contains("SelectDifficultyPreset", host);
            Assert.Contains("SetDifficultyCustomScalar", host);
            Assert.Contains("LockDifficultySettings", host);
            Assert.Contains("isDirty: () => _difficultySettingsDirty", host);
            Assert.Contains("save: SaveDifficultySettings", host);

            string panel = File.ReadAllText(Path.Combine(root, "src", "UI", "DifficultySettingsPanel.cs"));
            Assert.Contains("DifficultySettingsHostSession.ScalarNames", panel);
            Assert.Contains("LOCK DIFFICULTY (IRONMAN)", panel);
            Assert.Contains("CONFIRM LOCK — CANNOT BE UNDONE", panel);
            Assert.Contains("_lockConfirmPending", panel);
            Assert.Contains("UNSAVED CHANGES", panel);
            Assert.Contains("SAVE SETTINGS", panel);
            Assert.Contains("IBindablePanel", panel);
        }
    }
}
