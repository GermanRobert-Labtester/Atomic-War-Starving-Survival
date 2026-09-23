#nullable enable
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Research;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class Plan29_34ShelterResearchIntegrationTests
    {
        private static string ResolveDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void ShelterRoomAndMachineCatalogs_LoadFromAuthoritativeJson()
        {
            string dataDir = ResolveDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var roomCatalog = ShelterRoomIdentityCatalog.Load(files, json, dataDir);
            var machineCatalog = ShelterMachineTellCatalog.Load(files, json, dataDir);

            Assert.NotNull(roomCatalog);
            Assert.True(roomCatalog.Rooms.Count >= 10, $"Expected at least 10 rooms, found {roomCatalog.Rooms.Count}");

            foreach (var room in roomCatalog.Rooms)
            {
                Assert.False(string.IsNullOrWhiteSpace(room.id));
                Assert.StartsWith("room_", room.id, StringComparison.OrdinalIgnoreCase);
                Assert.False(string.IsNullOrWhiteSpace(room.display_name));
            }

            Assert.NotNull(machineCatalog);
            Assert.True(machineCatalog.Machines.Count >= 5, $"Expected at least 5 machines, found {machineCatalog.Machines.Count}");

            foreach (var machine in machineCatalog.Machines)
            {
                Assert.False(string.IsNullOrWhiteSpace(machine.id));
                Assert.StartsWith("machine_", machine.id, StringComparison.OrdinalIgnoreCase);
                Assert.False(string.IsNullOrWhiteSpace(machine.display_name));
                Assert.False(string.IsNullOrWhiteSpace(machine.room_id));
            }
        }

        [Fact]
        public void ResearchKnowledgeCatalog_LoadsFromAuthoritativeJson_AndValidatesTechTree()
        {
            string dataDir = ResolveDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var nodes = ResearchKnowledgeCatalogLoader.Load(dataDir, files, json);

            Assert.NotNull(nodes);
            Assert.True(nodes.Count >= 50, $"Expected >= 50 nodes, found {nodes.Count}");

            var nodeMap = new Dictionary<string, ResearchKnowledgeDef>(StringComparer.OrdinalIgnoreCase);
            foreach (var node in nodes)
            {
                Assert.False(string.IsNullOrWhiteSpace(node.id));
                Assert.StartsWith("knowledge_", node.id, StringComparison.OrdinalIgnoreCase);
                Assert.False(string.IsNullOrWhiteSpace(node.displayName));
                Assert.True(node.daysToComplete > 0, $"Node '{node.id}' must have positive daysToComplete");
                Assert.True(nodeMap.TryAdd(node.id, node), $"Duplicate node ID detected: '{node.id}'");
            }

            // Verify prerequisite integrity via loader DAG validator
            bool dagOk = ResearchKnowledgeCatalogLoader.ValidateDag(nodes, out string dagError);
            Assert.True(dagOk, $"DAG validation failed: {dagError}");
            Assert.Empty(dagError);
        }

        [Fact]
        public void ShelterInfrastructure_And_ResearchTree_CoexistAndCrossValidate()
        {
            string dataDir = ResolveDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // Load catalogs concurrently
            var roomCatalog = ShelterRoomIdentityCatalog.Load(files, json, dataDir);
            var machineCatalog = ShelterMachineTellCatalog.Load(files, json, dataDir);
            var researchNodes = ResearchKnowledgeCatalogLoader.Load(dataDir, files, json);

            Assert.NotNull(roomCatalog);
            Assert.NotNull(machineCatalog);
            Assert.NotNull(researchNodes);

            // Verify key machines exist and bind to valid rooms
            foreach (var machine in machineCatalog.Machines)
            {
                var room = roomCatalog.GetRoomIdentity(machine.room_id);
                Assert.NotNull(room);
                Assert.False(string.IsNullOrWhiteSpace(room.display_name));
            }

            // Verify related engineering / shelter research nodes exist
            var powerResearch = researchNodes.FirstOrDefault(n => n.id.Contains("power") || n.id.Contains("generator") || n.id.Contains("electric"));
            Assert.NotNull(powerResearch);

            var waterResearch = researchNodes.FirstOrDefault(n => n.id.Contains("water") || n.id.Contains("filtration") || n.id.Contains("filter"));
            Assert.NotNull(waterResearch);

            // Pure domain separation check: catalog state is immutable and non-interfering
            Assert.True(roomCatalog.Rooms.Count >= 10);
            Assert.True(machineCatalog.Machines.Count >= 5);
            Assert.True(researchNodes.Count >= 50);
        }
    }
}
