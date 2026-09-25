// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Xunit;

namespace Ashfall.Core.Tests.Integration
{
    public sealed class WaterSourcesSurfaceWiringTests
    {
        private static string RepoRoot()
        {
            string[] candidates = { Directory.GetCurrentDirectory(), AppContext.BaseDirectory };
            foreach (string start in candidates)
            {
                var directory = new DirectoryInfo(Path.GetFullPath(start));
                while (directory != null)
                {
                    if (File.Exists(Path.Combine(directory.FullName, "src", "Main.WaterSources.cs")))
                        return directory.FullName;
                    directory = directory.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate the ASHFALL repository root.");
        }

        private static string Source(string relativePath) =>
            File.ReadAllText(Path.Combine(RepoRoot(), relativePath));

        [Fact]
        public void WaterTreatmentRouteBindsAndOpensWaterSourcesSurface()
        {
            string route = Source("src/Main.ExpandedShelterSystems.cs");
            string panel = Source("src/UI/WaterTreatmentPanel.cs");

            Assert.Contains("case \"water_treatment\":", route);
            Assert.Contains("BindWaterSourcesPanel();", route);
            Assert.Contains("BindWaterSources(EnsureWaterSourcesSession())", Source("src/Main.WaterSources.cs"));
            Assert.Contains("BuildWaterSourcesSection();", panel);
            Assert.Contains("TryBuildDeepWell()", panel);
            Assert.Contains("TryBuildCondenser()", panel);
            Assert.Contains("TryConstructPiezometer()", panel);
        }

        [Fact]
        public void ExistingMainRoutesDelegateAndUseAtomicInventoryBills()
        {
            string well = Source("src/Main.DeepWell.cs");
            string condenser = Source("src/Main.WaterCondenser.cs");
            string session = Source("src/Host/WaterSourcesHostSession.cs");
            string piezometer = Source("src/Host/PiezometerHostSession.cs");

            Assert.Contains("EnsureWaterSourcesSession().TryBuildDeepWell()", well);
            Assert.Contains("EnsureWaterSourcesSession().TryServiceDeepWell()", well);
            Assert.Contains("EnsureWaterSourcesSession().TryBuildCondenser()", condenser);
            Assert.Contains("EnsureWaterSourcesSession().TryReplaceCondenserMembrane()", condenser);
            Assert.Contains("return !wasConstructed && Piezometer.System.IsConstructed;", session);
            Assert.DoesNotContain("TryConsumeById", well);
            Assert.DoesNotContain("TryConsumeById", condenser);
            Assert.Contains("_inventory.TryConsumeBill(bill, command)", session);
            Assert.Contains("inventory.TryConsumeBill(costs", piezometer);
        }

        [Fact]
        public void AggregateSnapshotsUseOnlyExistingWaterSaveStores()
        {
            string session = Source("src/Host/WaterSourcesHostSession.cs");

            Assert.Contains("DeepWellSaveStore.TryCapturePersisted", session);
            Assert.Contains("WaterCondenserSaveStore.TryCapturePersisted", session);
            Assert.Contains("PiezometerSaveStore.TryCapturePersisted", session);
            Assert.DoesNotContain("water_sources_save", session);
            Assert.DoesNotContain("\"water_sources\"", Source("Assets/Ashfall.Core/Save/SaveSectionRegistry.cs"));
        }

        [Fact]
        public void SanitationPanelHasARealExpandedRoute()
        {
            string route = Source("src/Main.ExpandedShelterSystems.cs");
            string sanitation = Source("src/Main.Sanitation.cs");
            string graph = Source("scripts/ci/generate-architecture-map.py");

            Assert.Contains("case \"sanitation\":", route);
            Assert.Contains("OpenSanitationPanel();", route);
            Assert.Contains("EnsureSanitationPanel();", sanitation);
            Assert.Contains("\"ui\": [\"SanitationPanel\"]", graph);
            Assert.Contains("\"routes\": [\"sanitation\"]", graph);
        }
    }
}
