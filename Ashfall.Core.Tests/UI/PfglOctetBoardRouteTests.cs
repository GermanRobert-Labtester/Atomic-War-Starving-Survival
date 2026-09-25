using System.Linq;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public sealed class PfglOctetBoardRouteTests
    {
        [Fact]
        public void CodexLuna6Boards_AreRegisteredAsLiveDashboardRoutes()
        {
            PanelRegistryBootstrap.RegisterAll();

            var romance = PanelRegistry.Get("romance_family_board");
            var colony = PanelRegistry.Get("colony_operations");
            var mediation = PanelRegistry.Get("ideological_mediation_desk");

            Assert.NotNull(romance);
            Assert.NotNull(colony);
            Assert.NotNull(mediation);
            Assert.True(romance!.IsPlayerNavigable);
            Assert.True(colony!.IsPlayerNavigable);
            Assert.True(mediation!.IsPlayerNavigable);
            Assert.Equal(PanelGroup.Dashboard, romance.Group);
            Assert.Equal(PanelGroup.Dashboard, colony.Group);
            Assert.Equal(PanelGroup.Dashboard, mediation.Group);
            Assert.Contains("survivors", romance.SetupDependencies);
            Assert.Contains("expeditions", colony.SetupDependencies);
            Assert.Contains("survivor_relations", mediation.SetupDependencies);
        }

        [Fact]
        public void CodexLuna6Boards_ManifestMarksOnlyCommandBoardsInteractive()
        {
            var manifest = PlayerSurfaceManifest.Generate();
            var romance = manifest.Contracts.Single(c => c.PanelId == "romance_family_board");
            var colony = manifest.Contracts.Single(c => c.PanelId == "colony_operations");
            var mediation = manifest.Contracts.Single(c => c.PanelId == "ideological_mediation_desk");

            Assert.Equal(SurfaceActionCoverage.ReadOnlyObservational, romance.ActionCoverage);
            Assert.Equal(SurfaceActionCoverage.InteractiveCommands, colony.ActionCoverage);
            Assert.Equal(SurfaceActionCoverage.InteractiveCommands, mediation.ActionCoverage);
            Assert.Equal(SurfaceRouteKind.Dashboard, colony.RouteKind);
            Assert.Equal(SurfaceRouteKind.Dashboard, mediation.RouteKind);
        }
    }
}
