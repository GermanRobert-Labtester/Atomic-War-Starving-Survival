// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class ItemLoreSystemTests
    {
        [Fact]
        public void Constructor_CapturedState_DoesNotAliasInput()
        {
            var state = new ItemLoreState
            {
                NextSequence = 3,
                LoreEntries = new List<ItemLoreEntry>
                {
                    new ItemLoreEntry
                    {
                        LoreId = "lore_2",
                        ItemInstanceId = "item_axe_01",
                        Text = "Recovered from a flooded shelter."
                    }
                },
                Provenances = new List<ItemProvenanceChain>
                {
                    new ItemProvenanceChain
                    {
                        ItemInstanceId = "item_axe_01",
                        LoreEntryIds = new List<string> { "lore_2" }
                    }
                }
            };

            var system = new ItemLoreSystem(state);
            state.LoreEntries.Clear();
            state.Provenances.Clear();

            Assert.Equal(1, system.TotalLoreEntriesCount);
            Assert.Equal(1, system.TrackedItemCount);
        }

        [Fact]
        public void Restore_StaleNextSequence_DoesNotCreateDuplicateLoreIds()
        {
            var state = new ItemLoreState
            {
                NextSequence = 1,
                LoreEntries = new List<ItemLoreEntry>
                {
                    new ItemLoreEntry
                    {
                        LoreId = "lore_2",
                        ItemInstanceId = "item_axe_01",
                        Text = "Existing lore."
                    }
                },
                Provenances = new List<ItemProvenanceChain>
                {
                    new ItemProvenanceChain
                    {
                        ItemInstanceId = "item_axe_01",
                        LoreEntryIds = new List<string> { "lore_2" }
                    }
                }
            };
            var system = new ItemLoreSystem(state);

            system.AddLore("item_axe_01", LoreTriggerType.Combat, "First new lore.", 4);
            system.AddLore("item_axe_01", LoreTriggerType.Trade, "Second new lore.", 5);

            Assert.Equal(3, system.TotalLoreEntriesCount);
            Assert.Equal(3, system.GetLoreEntries("item_axe_01").Select(entry => entry.LoreId).Distinct().Count());
        }

        [Fact]
        public void Queries_ReturnReadOnlySnapshots()
        {
            var system = new ItemLoreSystem();
            system.RegisterItem("item_axe_01", crafterId: "surv_smith", craftingDay: 2);

            var provenance = system.GetProvenance("item_axe_01")!;
            provenance.OwnershipChain.Add("tampered_owner");
            provenance.Significance = SignificanceLevel.Legendary;
            var lore = system.GetLoreEntries("item_axe_01");
            lore[0].Text = "tampered lore";

            var liveProvenance = system.GetProvenance("item_axe_01")!;
            Assert.DoesNotContain("tampered_owner", liveProvenance.OwnershipChain);
            Assert.NotEqual(SignificanceLevel.Legendary, liveProvenance.Significance);
            Assert.NotEqual("tampered lore", system.GetLoreEntries("item_axe_01")[0].Text);
        }

        [Fact]
        public void Restore_NullEntries_FailsClosed()
        {
            var state = new ItemLoreState
            {
                LoreEntries = new List<ItemLoreEntry> { null! },
                Provenances = new List<ItemProvenanceChain> { null! }
            };

            var system = new ItemLoreSystem();
            system.RestoreState(state);

            Assert.Equal(0, system.TotalLoreEntriesCount);
            Assert.Equal(0, system.TrackedItemCount);
        }

        [Fact]
        public void TransferOwnership_UsesCanonicalItemIdInEvent()
        {
            var system = new ItemLoreSystem();
            string? eventItemId = null;
            system.OnOwnershipTransferred += (itemId, _) => eventItemId = itemId;

            bool transferred = system.TransferOwnership(" item_axe_01 ", "surv_smith", day: 3);

            Assert.True(transferred);
            Assert.Equal("item_axe_01", eventItemId);
            Assert.NotNull(system.GetProvenance("item_axe_01"));
        }

        [Fact]
        public void RegisterItem_WithCrafter_AddsInitialCraftingLore()
        {
            var system = new ItemLoreSystem();
            var prov = system.RegisterItem("item_axe_01", crafterId: "surv_smith", craftingDay: 5, context: "forged in workshop");

            Assert.NotNull(prov);
            Assert.Equal("item_axe_01", prov.ItemInstanceId);
            Assert.Equal("surv_smith", prov.CrafterSurvivorId);
            Assert.Equal(5, prov.CraftingDay);
            Assert.Contains("surv_smith", prov.OwnershipChain);
            Assert.Single(prov.LoreEntryIds);
            Assert.Equal(1, system.TotalLoreEntriesCount);

            var lore = system.GetLoreEntries("item_axe_01");
            Assert.Single(lore);
            Assert.Equal(LoreTriggerType.Crafting, lore[0].TriggerType);
        }

        [Fact]
        public void RegisterItem_WithDiscovery_AddsDiscoveryLore()
        {
            var system = new ItemLoreSystem();
            var prov = system.RegisterItem("item_rifle_01", discoveryLocationId: "loc_bunker_alpha", discoveryDay: 12, context: "weapons locker");

            Assert.NotNull(prov);
            Assert.Equal("loc_bunker_alpha", prov.DiscoveryLocationId);
            Assert.Equal(12, prov.DiscoveryDay);
            Assert.Single(prov.LoreEntryIds);

            var lore = system.GetLoreEntries("item_rifle_01");
            Assert.Single(lore);
            Assert.Equal(LoreTriggerType.Discovery, lore[0].TriggerType);
            Assert.Equal(SignificanceLevel.Mundane, prov.Significance);
        }

        [Fact]
        public void TransferOwnership_UpdatesOwnershipChain_AndAddsLore()
        {
            var system = new ItemLoreSystem();
            var prov = system.RegisterItem("item_knife_01", crafterId: "surv_alice", craftingDay: 1);

            string? transferredId = null;
            string? newOwner = null;
            system.OnOwnershipTransferred += (id, owner) =>
            {
                transferredId = id;
                newOwner = owner;
            };

            bool ok = system.TransferOwnership("item_knife_01", "surv_bob", day: 4);

            Assert.True(ok);
            Assert.Equal("item_knife_01", transferredId);
            Assert.Equal("surv_bob", newOwner);
            Assert.Equal(2, prov.OwnershipChain.Count);
            Assert.Equal("surv_bob", prov.OwnershipChain.Last());
            Assert.Equal(2, prov.LoreEntryIds.Count);
        }

        [Fact]
        public void AddLore_IncreasesLoreCount_AndUpgradesSignificanceLevel()
        {
            var system = new ItemLoreSystem();
            var prov = system.RegisterItem("item_relic_01", crafterId: "surv_artisan", craftingDay: 1);

            // Lore count: 1 (Crafting) -> Mundane
            Assert.Equal(SignificanceLevel.Mundane, prov.Significance);

            // Add 1 more -> total 2 -> Notable
            system.AddLore("item_relic_01", LoreTriggerType.Combat, "Saved owner in ambush", 3, "surv_artisan");
            Assert.Equal(SignificanceLevel.Notable, prov.Significance);

            // Add 2 more -> total 4 -> Important
            system.AddLore("item_relic_01", LoreTriggerType.Gift, "Passed down to apprentice", 5);
            system.AddLore("item_relic_01", LoreTriggerType.Trade, "Ransomed back from raiders", 7);
            Assert.Equal(SignificanceLevel.Important, prov.Significance);

            // Add 2 more -> total 6 -> Legendary
            system.AddLore("item_relic_01", LoreTriggerType.LossRecovery, "Recovered from toxic swamp", 9);
            system.AddLore("item_relic_01", LoreTriggerType.SignificantMoment, "Used to seal breach in core", 12);
            Assert.Equal(SignificanceLevel.Legendary, prov.Significance);
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsCorrectly()
        {
            var system1 = new ItemLoreSystem();
            system1.RegisterItem("item_armor_01", crafterId: "surv_armorer", craftingDay: 2);
            system1.TransferOwnership("item_armor_01", "surv_scout", day: 5);
            system1.AddLore("item_armor_01", LoreTriggerType.Combat, "Deflected sniper round", day: 6, survivorId: "surv_scout");

            var state = system1.CaptureState();

            var system2 = new ItemLoreSystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.TrackedItemCount);
            Assert.Equal(3, system2.TotalLoreEntriesCount);

            var prov = system2.GetProvenance("item_armor_01");
            Assert.NotNull(prov);
            Assert.Equal("surv_armorer", prov.CrafterSurvivorId);
            Assert.Equal(2, prov.OwnershipChain.Count);
            Assert.Equal("surv_scout", prov.OwnershipChain.Last());
            Assert.Equal(SignificanceLevel.Notable, prov.Significance);

            var lore = system2.GetLoreEntries("item_armor_01");
            Assert.Equal(3, lore.Count);
        }
    }
}
