// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 190 — Item Lore & Provenance: core contract + wiring gates.
// Provenance from committed produce facts; idempotent; ownership chain;
// lore fragments; save/restore replay; registered section; read-only panel.
// ============================================================================
using System;
using System.IO;
using Ashfall.Core.Inventory;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class Plan190ItemLoreIntegrationTests
    {
        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "src", "Main.ItemLore.cs"))) return dir.FullName;
                dir = dir.Parent!;
            }
            throw new DirectoryNotFoundException("repo root");
        }

        private static string Source(string rel) => File.ReadAllText(Path.Combine(RepoRoot(), rel));

        [Fact]
        public void CraftProvenance_IsIdempotent_AndCarriesCrafterLore()
        {
            var sys = new ItemLoreSystem();
            var first = sys.RegisterItem("item_axe", "surv_smith", 4);
            var second = sys.RegisterItem("item_axe", "surv_other", 9); // duplicate delivery
            Assert.Same(first, second);
            Assert.Equal("surv_smith", first.CrafterSurvivorId);       // first fact stands
            Assert.Equal("surv_smith", Assert.Single(first.OwnershipChain));
            Assert.Equal(SignificanceLevel.Mundane, first.Significance);
        }

        [Fact]
        public void DiscoveryProvenance_RecordsLocationContext()
        {
            var sys = new ItemLoreSystem();
            var prov = sys.RegisterItem("item_relic", discoveryLocationId: "ruin_station_kilo",
                discoveryDay: 12, context: "expedition haul");
            Assert.Equal("ruin_station_kilo", prov.DiscoveryLocationId);
            Assert.Equal(12, prov.DiscoveryDay);
        }

        [Fact]
        public void OwnershipTransfer_ExtendsChain_ExactlyOnce()
        {
            var sys = new ItemLoreSystem();
            sys.RegisterItem("item_radio", "surv_a", 2);
            Assert.True(sys.TransferOwnership("item_radio", "surv_b", 6));
            Assert.False(sys.TransferOwnership("item_radio", "surv_b", 6)); // same owner twice refused
            var chain = sys.GetProvenance("item_radio")!.OwnershipChain;
            Assert.Equal(2, chain.Count);
        }

        [Fact]
        public void LoreFragments_AttachToTrackedItems_Only()
        {
            var sys = new ItemLoreSystem();
            sys.RegisterItem("item_axe", "surv_smith", 4);
            var entry = sys.AddLore("item_axe", LoreTriggerType.Gift, "Carried through the ashfall.", 9, "surv_b");
            Assert.NotNull(entry);
            Assert.Equal(2, sys.GetLoreEntries("item_axe").Count); // crafting lore + the gift fragment
        }

        [Fact]
        public void ReplayAfterRestore_KeepsChainWithoutDuplication()
        {
            var sys = new ItemLoreSystem();
            sys.RegisterItem("item_axe", "surv_smith", 4);
            sys.TransferOwnership("item_axe", "surv_b", 6);
            var saved = sys.CaptureState();
            var fresh = new ItemLoreSystem();
            fresh.RestoreState(saved);
            fresh.RegisterItem("item_axe", "surv_smith", 4); // producer replays
            var prov = fresh.GetProvenance("item_axe")!;
            Assert.Equal(2, prov.OwnershipChain.Count);
            Assert.Equal("surv_b", prov.OwnershipChain[1]);
        }

        [Fact]
        public void SaveRegistry_RegistersItemLoreSection()
        {
            Assert.True(SaveSectionRegistry.TryGetSection("item_lore", out var meta));
            Assert.Equal("SaveItemLore", meta!.SaveMethod);
            Assert.Equal("item_lore_save.json", SaveSectionRegistry.FileNameFor("item_lore"));
        }

        [Fact]
        public void HostSource_RecordsCommittedProducersAndReadsPanel()
        {
            string main = Source(Path.Combine("src", "Main.ItemLore.cs"));
            Assert.Contains("OnCraftCompleted", main);   // craft-completion producer
            Assert.Contains("OnItemAdded", main);        // intake producer
            Assert.Contains("SetupItemLore", main);
            Assert.Contains("SaveItemLore", main);
            Assert.DoesNotContain("System.Random", main);

            string panel = Source(Path.Combine("src", "UI", "InventoryDetailPanel.cs"));
            Assert.Contains("BindItemLore", panel);
            Assert.Contains("GetProvenance", panel);
        }
    }
}
