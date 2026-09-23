// SPDX-License-Identifier: MIT
using System.IO;
using System.Linq;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan210PersonalBelongingsIntegrationTests
    {
        [Fact]
        public void LoadCatalog_LoadsAllTemplates_FromValidJson()
        {
            var system = new PersonalBelongingsSystem();
            string path = Path.Combine("..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "personal_belongings.json");
            if (!File.Exists(path))
            {
                path = Path.Combine("Assets", "StreamingAssets", "Data", "personal_belongings.json");
            }
            Assert.True(File.Exists(path), $"Catalog file not found at {path}");

            string json = File.ReadAllText(path);
            system.LoadCatalog(json);

            Assert.Equal(8, system.Templates.Count);
            var locket = system.GetTemplate("keepsake_silver_locket");
            Assert.NotNull(locket);
            Assert.Equal("Silver Locket", locket.Name);
            Assert.Equal(BelongingCategory.Jewelry, locket.ParseCategory());
            Assert.Equal(85f, locket.BaseSentimentalValue);
            Assert.Equal(90f, locket.BaseCondition);
            Assert.Equal("rare", locket.Rarity);

            var knife = system.GetTemplate("keepsake_custom_hunting_knife");
            Assert.NotNull(knife);
            Assert.Equal(BelongingCategory.Tool, knife.ParseCategory());
        }

        [Fact]
        public void RegisterFromTemplate_CreatesPersonalBelonging_MatchingTemplateAttributes()
        {
            var system = new PersonalBelongingsSystem();
            system.LoadCatalog(@"{
                ""schema_version"": 1,
                ""templates"": [
                    {
                        ""id"": ""keepsake_faded_photograph"",
                        ""name"": ""Faded Photograph"",
                        ""category"": ""keepsake"",
                        ""base_sentimental_value"": 75.0,
                        ""base_condition"": 65.0,
                        ""description"": ""A creased picture of home."",
                        ""rarity"": ""uncommon""
                    }
                ]
            }");

            var item = system.RegisterFromTemplate(
                ownerSurvivorId: "dweller_gamma",
                templateId: "keepsake_faded_photograph",
                acquiredDay: 5,
                acquiredFrom: "ruined_library");

            Assert.NotNull(item);
            Assert.Equal("dweller_gamma", item.OwnerSurvivorId);
            Assert.Equal("keepsake_faded_photograph", item.ItemId);
            Assert.Equal("Faded Photograph", item.ItemName);
            Assert.Equal(BelongingCategory.Keepsake, item.Category);
            Assert.Equal(75.0f, item.SentimentalValue);
            Assert.Equal(65.0f, item.Condition);
            Assert.Equal(5, item.AcquiredDay);
            Assert.Equal("ruined_library", item.AcquiredFrom);
            Assert.False(item.IsFavorite);
            Assert.False(item.IsInherited);
        }

        [Fact]
        public void SetFavorite_EnforcesSingleFavoriteAndBoostsMoraleBuffer()
        {
            var system = new PersonalBelongingsSystem();
            var item1 = system.RegisterBelonging("dweller_delta", "item_a", "Item A", BelongingCategory.Keepsake, sentimentalValue: 50f, condition: 100f);
            var item2 = system.RegisterBelonging("dweller_delta", "item_b", "Item B", BelongingCategory.Tool, sentimentalValue: 50f, condition: 100f);
            Assert.NotNull(item1);
            Assert.NotNull(item2);

            // Buffer without favorites: 50*0.05*1 + 50*0.05*1 = 2.5 + 2.5 = 5.0
            float unfavBuffer = system.CalculateMoraleBuffer("dweller_delta");
            Assert.Equal(5.0f, unfavBuffer);

            // Set item1 as favorite: 50*0.05*2 + 50*0.05*1 = 5.0 + 2.5 = 7.5
            system.SetFavorite("dweller_delta", item1.BelongingId, true);
            Assert.True(item1.IsFavorite);
            float fav1Buffer = system.CalculateMoraleBuffer("dweller_delta");
            Assert.Equal(7.5f, fav1Buffer);

            // Switching favorite to item2 unsets item1
            system.SetFavorite("dweller_delta", item2.BelongingId, true);
            Assert.False(item1.IsFavorite);
            Assert.True(item2.IsFavorite);
            float fav2Buffer = system.CalculateMoraleBuffer("dweller_delta");
            Assert.Equal(7.5f, fav2Buffer);
        }

        [Fact]
        public void GiftBelonging_TransfersOwnership_AndInvokesEvent()
        {
            var system = new PersonalBelongingsSystem();
            var item = system.RegisterBelonging("dweller_alice", "book_1", "Survival Guide", BelongingCategory.Document, sentimentalValue: 80f);
            Assert.NotNull(item);

            BelongingTransfer? transferRecorded = null;
            system.OnBelongingGifted += t => transferRecorded = t;

            var transfer = system.GiftBelonging("dweller_alice", "dweller_bob", item.BelongingId, currentDay: 10, reason: "Befriending gesture");

            Assert.NotNull(transfer);
            Assert.Equal(transferRecorded, transfer);
            Assert.Equal("dweller_alice", transfer.FromSurvivorId);
            Assert.Equal("dweller_bob", transfer.ToSurvivorId);
            Assert.Equal(8.0f, transfer.SentimentalEffect);
            Assert.Equal("dweller_bob", item.OwnerSurvivorId);

            var aliceItems = system.GetBelongingsForSurvivor("dweller_alice");
            var bobItems = system.GetBelongingsForSurvivor("dweller_bob");
            Assert.Empty(aliceItems);
            Assert.Single(bobItems);
            Assert.Equal(item.BelongingId, bobItems[0].BelongingId);
        }

        [Fact]
        public void DistributeInheritanceOnDeath_TransfersToPrimaryHeir_WithSentimentalBonus()
        {
            var system = new PersonalBelongingsSystem();
            var item1 = system.RegisterBelonging("dweller_fallen", "relic_1", "Heirloom Pocket Knife", BelongingCategory.Tool, sentimentalValue: 40f);
            var item2 = system.RegisterBelonging("dweller_fallen", "relic_2", "Grandfather's Watch", BelongingCategory.Jewelry, sentimentalValue: 70f);
            Assert.NotNull(item1);
            Assert.NotNull(item2);

            var inherited = system.DistributeInheritanceOnDeath("dweller_fallen", "dweller_survivor", currentDay: 42);

            Assert.Equal(2, inherited.Count);
            Assert.Equal("dweller_survivor", item1.OwnerSurvivorId);
            Assert.Equal("dweller_survivor", item2.OwnerSurvivorId);
            Assert.True(item1.IsInherited);
            Assert.True(item2.IsInherited);
            Assert.Equal(50f, item1.SentimentalValue); // 40 + 10
            Assert.Equal(80f, item2.SentimentalValue); // 70 + 10
        }

        [Fact]
        public void ReportTheftOrLoss_RemovesBelonging_AndAppliesMoralePenalty()
        {
            var system = new PersonalBelongingsSystem();
            var item = system.RegisterBelonging("dweller_victim", "ring_1", "Gold Band", BelongingCategory.Jewelry, sentimentalValue: 60f);
            Assert.NotNull(item);
            system.SetFavorite("dweller_victim", item.BelongingId, true);

            BelongingEvent? lostEvent = null;
            system.OnBelongingLost += e => lostEvent = e;

            bool removed = system.ReportTheftOrLoss("dweller_victim", item.BelongingId, currentDay: 14, isStolen: true);

            Assert.True(removed);
            Assert.NotNull(lostEvent);
            Assert.Equal("item_stolen", lostEvent.EventType);
            Assert.Equal("dweller_victim", lostEvent.SurvivorId);
            // Favorite penalty = -(60 * 0.25) = -15
            Assert.Equal(-15.0f, lostEvent.MoraleEffect);
            Assert.Null(system.GetBelonging(item.BelongingId));
        }

        [Fact]
        public void CaptureAndRestoreState_RoundTripsBelongingsAndEvents()
        {
            var system = new PersonalBelongingsSystem();
            var item = system.RegisterBelonging("dweller_1", "trinket", "Lucky Coin", BelongingCategory.Memento, sentimentalValue: 90f);
            Assert.NotNull(item);
            system.GiftBelonging("dweller_1", "dweller_2", item.BelongingId, currentDay: 3, reason: "Luck gift");

            var save = system.CaptureState();
            Assert.Single(save.Belongings);
            Assert.Single(save.Transfers);
            Assert.Equal(2, save.Events.Count);

            var restored = new PersonalBelongingsSystem();
            restored.RestoreState(save);

            Assert.Equal(1, restored.TotalBelongingsCount);
            var restoredItem = restored.GetBelonging(item.BelongingId);
            Assert.NotNull(restoredItem);
            Assert.Equal("dweller_2", restoredItem.OwnerSurvivorId);
            Assert.Equal(90f, restoredItem.SentimentalValue);
        }
    }
}
