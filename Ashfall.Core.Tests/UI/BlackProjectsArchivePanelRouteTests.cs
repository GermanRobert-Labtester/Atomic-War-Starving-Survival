// SPDX-License-Identifier: MIT
// ASHFALL UI Gate: Black Projects Archive Panel Route & Contract Tests (Plan 152 Follow-up)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public sealed class BlackProjectsArchivePanelRouteTests
    {
        public BlackProjectsArchivePanelRouteTests()
        {
            PanelRegistryBootstrap.RegisterAll();
        }

        [Fact]
        public void BlackProjectsArchiveRoute_IsRegisteredInPanelRegistry()
        {
            Assert.True(PanelRegistry.IsRegistered("black_projects_archive"),
                "Panel 'black_projects_archive' must be registered in PanelRegistry.");

            var desc = PanelRegistry.Get("black_projects_archive");
            Assert.NotNull(desc);
            Assert.Equal("black_projects_archive", desc.Id);
            Assert.Equal("Black Projects Intelligence Archive", desc.DisplayName);
            Assert.Equal(PanelGroup.Expanded, desc.Group);
            Assert.Equal(PanelMaturity.Live, desc.Maturity);
            Assert.True(desc.IsPlayerNavigable, "black_projects_archive must be player navigable.");
            Assert.Contains("black_projects_archive", desc.SetupDependencies);
        }

        [Fact]
        public void BlackProjectsArchiveRoute_IsIncludedInPlayerSurfaceManifest()
        {
            var manifest = PlayerSurfaceManifest.Generate();
            Assert.NotNull(manifest);

            var contract = manifest.Contracts.FirstOrDefault(c => c.PanelId == "black_projects_archive");
            Assert.NotNull(contract);
            Assert.Equal(SurfaceRouteKind.ExpandedShelter, contract.RouteKind);
            // Non-negotiable rule: ARCHIVAL presentation only, zero executable weapon/breach fields
            Assert.Equal(SurfaceActionCoverage.ReadOnlyObservational, contract.ActionCoverage);
            Assert.Equal(SurfaceRenderCoverage.ProductionRendered, contract.RenderCoverage);
            Assert.Contains("OpenExpandedPanel(\"black_projects_archive\")", contract.ReachableRoutes);
        }

        [Fact]
        public void BlackProjectsArchive_CatalogAndProjection_SupportsAllFourBatches()
        {
            string? dataDir = FindDataDir();
            Assert.NotNull(dataDir);

            var catalog = BlackProjectsCatalog.LoadFromDirectory(dataDir);
            Assert.Equal(8, catalog.OrbitalEntries.Count);
            Assert.Equal(8, catalog.DroneEntries.Count);
            Assert.Equal(7, catalog.CobaltEntries.Count);
            Assert.Equal(7, catalog.VaultEntries.Count);

            var archive = new BlackProjectsArchiveSystem(catalog);
            foreach (var (recordId, producer) in BlackProjectsArchiveSystem.DefaultProducerMap())
            {
                Assert.True(archive.TryRegisterProducer(recordId, producer));
            }

            // Verify truth class mapping across all 4 batches
            Assert.Equal(BlackProjectsTruthClass.InstrumentTelemetry, archive.TruthClassFor(catalog.OrbitalEntries[0].Id));
            Assert.Equal(BlackProjectsTruthClass.VehicleBlackbox, archive.TruthClassFor(catalog.DroneEntries[0].Id));
            Assert.Equal(BlackProjectsTruthClass.ClassifiedDirective, archive.TruthClassFor(catalog.CobaltEntries[0].Id));
            Assert.Equal(BlackProjectsTruthClass.ComplianceAudit, archive.TruthClassFor(catalog.VaultEntries[0].Id));
        }

        [Fact]
        public void BlackProjectsArchive_RelatedRecordGraph_SurfacesCrossReferencesWithoutSpoilers()
        {
            string? dataDir = FindDataDir();
            Assert.NotNull(dataDir);

            var catalog = BlackProjectsCatalog.LoadFromDirectory(dataDir);
            var archive = new BlackProjectsArchiveSystem(catalog);
            foreach (var (recordId, producer) in BlackProjectsArchiveSystem.DefaultProducerMap())
            {
                archive.TryRegisterProducer(recordId, producer);
            }

            // Before discovery: zero relations returned (privacy firewall)
            Assert.Empty(archive.GetRelated("blackbox_valkyrie_radar_interrogation_loop"));

            // Discover only the first record: still zero relations because partner is undiscovered
            archive.DiscoverRecord("blackbox_valkyrie_radar_interrogation_loop");
            Assert.Empty(archive.GetRelated("blackbox_valkyrie_radar_interrogation_loop"));

            // Discover the partner record: now relation is surfaced bidirectionally
            archive.DiscoverRecord("blackbox_valkyrie_target_misidentification");
            var relationsA = archive.GetRelated("blackbox_valkyrie_radar_interrogation_loop");
            var relationsB = archive.GetRelated("blackbox_valkyrie_target_misidentification");

            Assert.Equal(2, relationsA.Count);
            Assert.Contains(relationsA, r => r.RecordId == "blackbox_valkyrie_target_misidentification" && r.RelationKind == "corroborates");
            Assert.Contains(relationsA, r => r.RecordId == "blackbox_valkyrie_target_misidentification" && r.RelationKind == "related_by_carrier");

            Assert.Equal(2, relationsB.Count);
            Assert.Contains(relationsB, r => r.RecordId == "blackbox_valkyrie_radar_interrogation_loop" && r.RelationKind == "corroborates");
            Assert.Contains(relationsB, r => r.RecordId == "blackbox_valkyrie_radar_interrogation_loop" && r.RelationKind == "related_by_carrier");
        }

        [Fact]
        public void BlackProjectsArchive_AuthorityFirewall_ContainsZeroWeaponOrBreachCommands()
        {
            // Verify that BlackProjectsArchiveSystem contains zero executable weapon/breach/spawn methods
            var systemType = typeof(BlackProjectsArchiveSystem);
            var methods = systemType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            var forbiddenPrefixes = new[] { "Launch", "Fire", "Breach", "Spawn", "Arm", "Detonate", "ExecuteAttack", "CommandDrone" };

            foreach (var method in methods)
            {
                foreach (var prefix in forbiddenPrefixes)
                {
                    Assert.False(method.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase),
                        $"BlackProjectsArchiveSystem must not expose executable command method: {method.Name}");
                }
            }
        }

        private static string? FindDataDir()
        {
            string? dir = Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(dir))
            {
                string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(probe))
                    return probe;
                var parent = Directory.GetParent(dir);
                dir = parent?.FullName;
            }
            return null;
        }
    }
}
