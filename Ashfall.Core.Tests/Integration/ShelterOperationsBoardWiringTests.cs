// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests.Integration
{
    public sealed class ShelterOperationsBoardWiringTests
    {
        private static string FindRepoRoot()
        {
            var current = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (current != null)
            {
                if (File.Exists(Path.Combine(current.FullName, "INTEGRATION_PLANS.md")))
                    return current.FullName;
                current = current.Parent;
            }
            throw new InvalidOperationException("Repository root was not found.");
        }

        [Fact]
        public void OperationsRoute_IsRegisteredAndExpanded()
        {
            PanelRegistryBootstrap.RegisterAll();
            var route = PanelRegistry.Get("shelter_operations");

            Assert.NotNull(route);
            Assert.Equal(PanelGroup.Expanded, route!.Group);
            Assert.True(route.IsPlayerNavigable);
            Assert.Contains("inventory", route.SetupDependencies);
            Assert.Contains("shelter_assignment", route.SetupDependencies);
            Assert.Contains("shelter_expansion", route.SetupDependencies);
            Assert.Contains("outpost_settlement", route.SetupDependencies);
            Assert.Contains("seasonal_celebration", route.SetupDependencies);
            var surface = PlayerSurfaceManifest.Generate().Contracts.Single(x => x.PanelId == "shelter_operations");
            Assert.Equal(SurfaceActionCoverage.InteractiveCommands, surface.ActionCoverage);
        }

        [Fact]
        public void OperationsRoute_HasPlayerForwardingAndPanelCommandWiring()
        {
            string root = FindRepoRoot();
            string gameFlow = File.ReadAllText(Path.Combine(root, "src", "Main.GameFlow.cs"));
            string expanded = File.ReadAllText(Path.Combine(root, "src", "Main.ExpandedShelterSystems.cs"));
            string surfaces = File.ReadAllText(Path.Combine(root, "src", "Main.PlayerSurfaces.cs"));
            string host = File.ReadAllText(Path.Combine(root, "src", "Host", "ShelterOperationsHostSession.cs"));
            string panel = File.ReadAllText(Path.Combine(root, "src", "UI", "ShelterOperationsPanel.cs"));

            Assert.Contains("case \"shelter_operations\":", gameFlow);
            Assert.Contains("case \"shelter_operations\":", expanded);
            Assert.Contains("\"shelter_operations\"", surfaces);
            Assert.Contains("TryStartRoomConstruction", host);
            Assert.Contains("TryEstablish", host);
            Assert.Contains("TrySupply", host);
            Assert.Contains("TryHoldCelebration", host);
            Assert.Contains("Start build", panel, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void OperationsBoard_ReusesExistingSaveAuthorities()
        {
            string root = FindRepoRoot();
            string host = File.ReadAllText(Path.Combine(root, "src", "Host", "ShelterOperationsHostSession.cs"));
            string main = File.ReadAllText(Path.Combine(root, "src", "Main.ShelterOperations.cs"));

            Assert.Contains("ShelterExpansionHostSession", host);
            Assert.Contains("OutpostSettlementHostSession", host);
            Assert.Contains("SeasonalCelebrationHostSession", host);
            Assert.Contains("IPlayerInventoryPort", host);
            Assert.DoesNotContain("SaveStore<", host);
            Assert.DoesNotContain("ShelterOperationsSaveStore", main);
        }
    }
}
