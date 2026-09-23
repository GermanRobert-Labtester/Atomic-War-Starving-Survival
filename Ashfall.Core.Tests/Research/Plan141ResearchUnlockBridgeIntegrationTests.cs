// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Research;
using Xunit;

namespace Ashfall.Core.Tests.Research
{
    public sealed class Plan141ResearchUnlockBridgeIntegrationTests
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
        public void ResearchUnlockCatalog_LoadsAuthoredUnlocks_Cleanly()
        {
            string dataDir = GetDataDir();
            var bridge = ResearchUnlockBridge.LoadFromDirectory(dataDir, new FileSystemIO());

            Assert.NotNull(bridge.Catalog);
            Assert.True(bridge.Catalog.Count >= 30, $"Expected >= 30 unlocks, found {bridge.Catalog.Count}");

            // Verify item, recipe, shelter, expedition, combat, and medical unlocks
            Assert.Contains(bridge.Catalog, u => u.unlock_type == "item" && u.unlock_target_id == "item_water_filter_advanced");
            Assert.Contains(bridge.Catalog, u => u.unlock_type == "recipe" && u.unlock_target_id == "purify_water");
            Assert.Contains(bridge.Catalog, u => u.unlock_type == "shelter" && u.unlock_target_id == "shelter_upgrade_lead_lining");
            Assert.Contains(bridge.Catalog, u => u.unlock_type == "expedition" && u.unlock_target_id == "expedition_encrypted_caches");
            Assert.Contains(bridge.Catalog, u => u.unlock_type == "combat" && u.unlock_target_id == "combat_doctrine_precision_rifling");
            Assert.Contains(bridge.Catalog, u => u.unlock_type == "medical" && u.unlock_target_id == "medical_procedure_field_trauma");
        }

        [Fact]
        public void ResearchCompletion_AwardsBreakthroughItem_AndUnlocksRecipe()
        {
            string dataDir = GetDataDir();
            var bridge = ResearchUnlockBridge.LoadFromDirectory(dataDir, new FileSystemIO());
            var inventory = new Ashfall.Core.Inventory.Inventory();
            var sink = new InventoryResearchUnlockSink(inventory);

            // Process completion of advanced water filtration
            int granted = bridge.ProcessResearchCompletion("knowledge_water_advanced", sink);

            Assert.True(granted >= 2);
            // Breakthrough item deposited into inventory
            Assert.True(inventory.CountById("item_water_filter_advanced") >= 1);
            // Recipe unlocked
            Assert.Contains("purify_water", sink.UnlockedRecipes);
            // Recorded in bridge state
            Assert.Contains("item_water_filter_advanced", bridge.State.grantedItems);
            Assert.Contains("purify_water", bridge.State.unlockedRecipeIds);
        }

        [Fact]
        public void ResearchSystem_BoundEvent_AutomaticallyTriggersDownstreamUnlocks()
        {
            string dataDir = GetDataDir();
            var bridge = ResearchUnlockBridge.LoadFromDirectory(dataDir, new FileSystemIO());
            var researchSystem = new ResearchSystem();
            var inventory = new Ashfall.Core.Inventory.Inventory();
            var sink = new InventoryResearchUnlockSink(inventory);

            bridge.BindResearchSystem(researchSystem, sink);

            // Register radiation shielding node
            var nodeDef = new ResearchKnowledgeDef(
                "knowledge_radiation_shielding",
                "Radiation Shielding Materials",
                "engineering",
                "Lead cloth and shielding panels",
                10,
                null,
                "item_radiation_shielding_panel");

            researchSystem.Register(nodeDef);

            // Simulate node completion in research system
            researchSystem.StartResearch(nodeDef.id, 1);
            researchSystem.CompleteResearch(nodeDef.id);

            // Verify breakthrough item and shelter capability were granted automatically
            Assert.True(inventory.CountById("item_radiation_shielding_panel") >= 1);
            Assert.Contains("shelter:shelter_upgrade_lead_lining", sink.EnabledCapabilities);
        }

        [Fact]
        public void RetroactiveSynchronization_GrantsMissingUnlocks_ForOldSaves()
        {
            string dataDir = GetDataDir();
            var bridge = ResearchUnlockBridge.LoadFromDirectory(dataDir, new FileSystemIO());
            var inventory = new Ashfall.Core.Inventory.Inventory();
            var sink = new InventoryResearchUnlockSink(inventory);

            // Simulate save with previously completed nodes
            var completedNodes = new List<string>
            {
                "knowledge_air_filtration",
                "knowledge_solar_advanced"
            };

            int granted = bridge.SynchronizeCompletedResearch(completedNodes, sink);

            Assert.True(granted >= 4);
            Assert.True(inventory.CountById("item_air_filter_hepa") >= 1);
            Assert.True(inventory.CountById("item_solar_inverter") >= 1);
            Assert.Contains("shelter:shelter_upgrade_vent_scrubber", sink.EnabledCapabilities);
            Assert.Contains("shelter:shelter_upgrade_solar_roof", sink.EnabledCapabilities);
        }

        [Fact]
        public void ResearchUnlockBridge_CaptureRestore_PreservesAllGrantedState()
        {
            var bridge = new ResearchUnlockBridge();
            bridge.RegisterUnlock(new ResearchUnlockDef
            {
                id = "unlock_test_1",
                research_node_id = "test_node",
                unlock_type = "item",
                unlock_target_id = "test_item"
            });

            bridge.ProcessResearchCompletion("test_node");
            Assert.Single(bridge.State.grantedUnlockIds);

            var state = bridge.CaptureState();
            Assert.Equal(1, state.schema_version);
            Assert.Single(state.grantedUnlockIds);
            Assert.Single(state.grantedItems);

            var restored = new ResearchUnlockBridge();
            restored.RestoreState(state);

            Assert.Single(restored.State.grantedUnlockIds);
            Assert.Equal("unlock_test_1", restored.State.grantedUnlockIds[0]);
            Assert.Equal("test_item", restored.State.grantedItems[0]);
        }
    }
}
