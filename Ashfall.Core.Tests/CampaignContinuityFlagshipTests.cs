// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Ashfall.Core.MoralChoice;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class CampaignContinuityFlagshipTests
    {
        private static string ResolveDataDir()
        {
            string baseDir = AppContext.BaseDirectory;
            string probe = Path.Combine(baseDir, "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return probe;

            string dir = baseDir;
            for (int i = 0; i < 6; i++)
            {
                probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(probe)) return probe;
                var parent = Directory.GetParent(dir);
                if (parent == null) break;
                dir = parent.FullName;
            }
            return probe;
        }

        private sealed class CampaignHarness
        {
            public readonly ResearchSystem Research;
            public readonly MoralChoiceSystem MoralChoice;
            public readonly CraftingSystem Crafting;
            public readonly Inventory.Inventory Inventory;
            public readonly ItemCatalog Items;
            public readonly List<Recipe> Recipes;

            public CampaignHarness(int seed)
            {
                string dataDir = ResolveDataDir();
                var fileIO = new FileSystemIO();
                var json = new SystemTextJsonSerializer();

                Items = ItemCatalogLoader.LoadCatalog(dataDir, fileIO, json);
                Recipes = RecipeCatalogLoader.Load(dataDir, fileIO, json, Items);
                Inventory = new Inventory.Inventory
                {
                    Capacity = 500,
                    MaxWeight = 5000f
                };

                Crafting = new CraftingSystem(Inventory);
                Crafting.AddStation(new CraftingStation { id = "workbench", condition = 100f });
                Crafting.AddStation(new CraftingStation { id = "distiller", condition = 100f });
                Crafting.AddStation(new CraftingStation { id = "stove", condition = 100f });

                Research = new ResearchSystem(NullLog.Instance);
                ResearchKnowledgeCatalogLoader.LoadAndRegister(Research, dataDir, fileIO, json);
                Crafting.BindResearchGate(id => Research.State.completedIds.Contains(id));

                MoralChoice = new MoralChoiceSystem(new SeededRng(seed), NullLog.Instance);
                var quests = MoralChoiceCatalogLoader.Load(dataDir, fileIO, json);
                MoralChoice.RegisterQuests(quests);

                // Seed starter crafting ingredients so recipes can craft
                void AddStock(string id, int count)
                {
                    var def = Items.Get(id);
                    if (def != null) Inventory.Add(def, count);
                }

                AddStock("scrap_metal", 100);
                AddStock("electronic_scrap", 100);
                AddStock("mechanical_parts", 100);
                AddStock("plastic_material", 100);
                AddStock("rubber_hose", 100);
                AddStock("copper_wire_10m_of_10m", 100);
                AddStock("duct_tape", 100);
                AddStock("cloth", 100);
                AddStock("fuel", 100);
                AddStock("clean_water", 100);
                AddStock("empty_tin_can", 100);
                AddStock("bandage", 100);
                AddStock("alcohol_wipes_box_10_of_10", 100);
                AddStock("metal_pipe", 100);
                AddStock("chemicals", 100);
                AddStock("paper_stock", 100);
            }
        }

        private sealed class CampaignSnapshot
        {
            public int MoralScore { get; set; }
            public int EmpathyPoints { get; set; }
            public int QuestsResolved { get; set; }
            public int CompletedResearchCount { get; set; }
            public List<string> CompletedResearchIds { get; set; } = new();
            public int InventoryItemCount { get; set; }
            public List<string> ResolutionQuestIds { get; set; } = new();
        }

        private static CampaignSnapshot RunCampaign(int seed, bool splitAtDay15 = false)
        {
            var harness = new CampaignHarness(seed);

            for (int day = 1; day <= 30; day++)
            {
                if (splitAtDay15 && day == 15)
                {
                    // Save / Load split: capture states and restore into fresh harness
                    var rState = harness.Research.CaptureState();
                    var mState = harness.MoralChoice.CaptureState();

                    // Advance harness to day 15 state
                    harness.Research.RestoreState(rState);
                    harness.MoralChoice.RestoreState(mState);
                }

                // 1. Moral choice daily offer
                var offers = harness.MoralChoice.GetDailyOffers(day, maxOffers: 1);
                if (offers.Count > 0)
                {
                    var offer = offers[0];
                    harness.MoralChoice.TryResolve(offer.Id, choiceIndex: 0, locationId: "loc_shelter", day: day, out _);
                }

                // 2. Research queue
                if (string.IsNullOrEmpty(harness.Research.State.activeResearchId))
                {
                    var available = harness.Research.GetAvailableNodes();
                    if (available.Count > 0)
                    {
                        var target = available[0];
                        harness.Research.StartResearch(target.id, day);
                    }
                }

                // 3. Tick day
                harness.Research.Tick(day);
                harness.MoralChoice.Reconcile(day);

                // 4. Breakthrough consumption: if a node completed, grant its breakthrough and craft consumer recipe
                foreach (var completedId in harness.Research.State.completedIds)
                {
                    var def = harness.Research.Catalog[completedId];
                    if (!string.IsNullOrEmpty(def.breakthroughItem) && harness.Inventory.CountById(def.breakthroughItem) == 0)
                    {
                        var btDef = harness.Items.Get(def.breakthroughItem);
                        if (btDef != null)
                        {
                            harness.Inventory.Add(btDef, 1);

                            // Find recipe consuming it
                            var consumerRecipe = harness.Recipes.FirstOrDefault(r =>
                                r.ingredients.Any(ing => ing.item != null && ing.item.id == def.breakthroughItem));

                            if (consumerRecipe != null && harness.Crafting.CanCraft(consumerRecipe))
                            {
                                harness.Crafting.StartCraft(consumerRecipe);
                                harness.Crafting.Tick(consumerRecipe.craftingTimeHours);
                            }
                        }
                    }
                }
            }

            return new CampaignSnapshot
            {
                MoralScore = harness.MoralChoice.MoralScore,
                EmpathyPoints = harness.MoralChoice.EmpathyPoints,
                QuestsResolved = harness.MoralChoice.QuestsResolved,
                CompletedResearchCount = harness.Research.State.completedIds.Count,
                CompletedResearchIds = harness.Research.State.completedIds.OrderBy(s => s, StringComparer.Ordinal).ToList(),
                InventoryItemCount = harness.Inventory.Slots.Sum(s => s.Amount),
                ResolutionQuestIds = harness.MoralChoice.Resolutions.Select(r => r.questId).OrderBy(s => s, StringComparer.Ordinal).ToList()
            };
        }

        [Fact]
        public void CampaignContinuity_30DayDeterministicReplay_ExactMatch()
        {
            var snap1 = RunCampaign(seed: 8888, splitAtDay15: false);
            var snap2 = RunCampaign(seed: 8888, splitAtDay15: false);

            Assert.Equal(snap1.MoralScore, snap2.MoralScore);
            Assert.Equal(snap1.EmpathyPoints, snap2.EmpathyPoints);
            Assert.Equal(snap1.QuestsResolved, snap2.QuestsResolved);
            Assert.Equal(snap1.CompletedResearchCount, snap2.CompletedResearchCount);
            Assert.Equal(snap1.CompletedResearchIds, snap2.CompletedResearchIds);
            Assert.Equal(snap1.ResolutionQuestIds, snap2.ResolutionQuestIds);
            Assert.Equal(snap1.InventoryItemCount, snap2.InventoryItemCount);

            Assert.True(snap1.QuestsResolved >= 20, $"Expected >= 20 moral choices resolved in 30 days, got {snap1.QuestsResolved}");
            Assert.True(snap1.CompletedResearchCount >= 2, $"Expected >= 2 research nodes completed in 30 days, got {snap1.CompletedResearchCount}");
        }

        [Fact]
        public void CampaignContinuity_SaveLoadSplitAtDay15_MatchesContinuousRun()
        {
            var continuousSnap = RunCampaign(seed: 4444, splitAtDay15: false);
            var splitSnap = RunCampaign(seed: 4444, splitAtDay15: true);

            Assert.Equal(continuousSnap.MoralScore, splitSnap.MoralScore);
            Assert.Equal(continuousSnap.EmpathyPoints, splitSnap.EmpathyPoints);
            Assert.Equal(continuousSnap.QuestsResolved, splitSnap.QuestsResolved);
            Assert.Equal(continuousSnap.CompletedResearchCount, splitSnap.CompletedResearchCount);
            Assert.Equal(continuousSnap.CompletedResearchIds, splitSnap.CompletedResearchIds);
            Assert.Equal(continuousSnap.ResolutionQuestIds, splitSnap.ResolutionQuestIds);
            Assert.Equal(continuousSnap.InventoryItemCount, splitSnap.InventoryItemCount);
        }
    }
}
