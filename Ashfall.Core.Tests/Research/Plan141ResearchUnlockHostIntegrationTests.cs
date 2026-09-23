// SPDX-License-Identifier: MIT
// Host and domain integration tests for Plan 141: Research -> Downstream Unlocks Bridge.

using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Research;
using Xunit;

namespace Ashfall.Core.Tests.Research
{
    public sealed class Plan141ResearchUnlockHostIntegrationTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent!;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        [Fact]
        public void Bridge_GetCensus_ReportsValidCounts()
        {
            string dataDir = GetDataDir();
            var bridge = ResearchUnlockBridge.LoadFromDirectory(dataDir, new FileSystemIO());
            var census = bridge.GetCensus();

            Assert.True(census.TotalCatalogUnlocks >= 30, $"Expected >= 30 unlocks, got {census.TotalCatalogUnlocks}");
            Assert.Equal(0, census.GrantedUnlocksCount);
            Assert.Equal(0, census.GrantedItemsCount);
            Assert.Equal(0, census.UnlockedRecipesCount);
            Assert.Equal(0, census.UnlockedCapabilitiesCount);
        }

        [Fact]
        public void Bridge_ProcessResearchCompletion_GrantsItemsAndRecipesIdempotently()
        {
            string dataDir = GetDataDir();
            var bridge = ResearchUnlockBridge.LoadFromDirectory(dataDir, new FileSystemIO());
            var inventory = new Ashfall.Core.Inventory.Inventory();
            var sink = new InventoryResearchUnlockSink(inventory);

            int grantedFirst = bridge.ProcessResearchCompletion("knowledge_water_advanced", sink);
            Assert.True(grantedFirst >= 2, $"Expected at least 2 unlocks granted, got {grantedFirst}");

            int countBefore = inventory.CountById("item_water_filter_advanced");
            Assert.True(countBefore >= 1);

            // Re-processing the same node must be completely idempotent
            int grantedSecond = bridge.ProcessResearchCompletion("knowledge_water_advanced", sink);
            Assert.Equal(0, grantedSecond);
            Assert.Equal(countBefore, inventory.CountById("item_water_filter_advanced"));
        }

        [Fact]
        public void Bridge_Queries_ReflectGrantedState()
        {
            string dataDir = GetDataDir();
            var bridge = ResearchUnlockBridge.LoadFromDirectory(dataDir, new FileSystemIO());
            var inventory = new Ashfall.Core.Inventory.Inventory();
            var sink = new InventoryResearchUnlockSink(inventory);

            bridge.ProcessResearchCompletion("knowledge_water_advanced", sink);

            Assert.True(bridge.HasItem("item_water_filter_advanced"));
            Assert.True(bridge.HasRecipe("purify_water"));
            Assert.True(bridge.HasUnlock("unlock_water_advanced_filter"));
            Assert.True(bridge.HasUnlock("unlock_water_purify_recipe"));

            var census = bridge.GetCensus();
            Assert.True(census.GrantedItemsCount >= 1);
            Assert.True(census.UnlockedRecipesCount >= 1);
            Assert.True(census.GrantedUnlocksCount >= 2);
        }

        [Fact]
        public void Bridge_SynchronizeCompletedResearch_HandlesPreExistingNodes()
        {
            string dataDir = GetDataDir();
            var bridge = ResearchUnlockBridge.LoadFromDirectory(dataDir, new FileSystemIO());
            var inventory = new Ashfall.Core.Inventory.Inventory();
            var sink = new InventoryResearchUnlockSink(inventory);

            var completedNodes = new[] { "knowledge_water_advanced", "knowledge_hydroponics" };
            bridge.SynchronizeCompletedResearch(completedNodes, sink);

            Assert.True(bridge.HasItem("item_water_filter_advanced"));
            Assert.True(bridge.HasRecipe("purify_water"));
            Assert.True(bridge.HasCapability("shelter:shelter_upgrade_hydro_bays"));

            // Repeat sync should be idempotent
            bridge.SynchronizeCompletedResearch(completedNodes, sink);
            Assert.Equal(1, inventory.CountById("item_water_filter_advanced"));
        }

        [Fact]
        public void Bridge_StateRoundTrip_PreservesAllUnlocks()
        {
            string dataDir = GetDataDir();
            var bridge = ResearchUnlockBridge.LoadFromDirectory(dataDir, new FileSystemIO());
            var inventory = new Ashfall.Core.Inventory.Inventory();
            var sink = new InventoryResearchUnlockSink(inventory);

            bridge.ProcessResearchCompletion("knowledge_water_advanced", sink);
            var state = bridge.CaptureState();

            Assert.Equal(1, state.schema_version);
            Assert.NotEmpty(state.grantedUnlockIds);
            Assert.NotEmpty(state.grantedItems);
            Assert.NotEmpty(state.unlockedRecipeIds);

            // Fresh bridge restore
            var freshBridge = ResearchUnlockBridge.LoadFromDirectory(dataDir, new FileSystemIO());
            freshBridge.RestoreState(state);

            Assert.True(freshBridge.HasItem("item_water_filter_advanced"));
            Assert.True(freshBridge.HasRecipe("purify_water"));
            Assert.True(freshBridge.HasUnlock("unlock_water_advanced_filter"));

            var census = freshBridge.GetCensus();
            Assert.Equal(state.grantedUnlockIds.Count, census.GrantedUnlocksCount);
            Assert.Equal(state.grantedItems.Count, census.GrantedItemsCount);
            Assert.Equal(state.unlockedRecipeIds.Count, census.UnlockedRecipesCount);
        }
    }
}
