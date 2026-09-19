// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class PersonalBelongingsSystemTests
    {
        [Fact]
        public void RegisterBelonging_InitializesCorrectly()
        {
            var system = new PersonalBelongingsSystem();
            var item = system.RegisterBelonging(
                ownerSurvivorId: "dweller_1",
                itemId: "keepsake_locket",
                itemName: "Silver Locket",
                category: BelongingCategory.Jewelry,
                sentimentalValue: 80f,
                condition: 95f,
                acquiredDay: 3,
                acquiredFrom: "pre_war",
                description: "Contains a photo of lost family."
            );

            Assert.NotNull(item);
            Assert.Equal("dweller_1", item.OwnerSurvivorId);
            Assert.Equal("Silver Locket", item.ItemName);
            Assert.Equal(BelongingCategory.Jewelry, item.Category);
            Assert.Equal(80f, item.SentimentalValue);
            Assert.Equal(95f, item.Condition);
            Assert.False(item.IsFavorite);
            Assert.Equal(1, system.TotalBelongingsCount);
        }

        [Fact]
        public void SetFavorite_EnforcesSingleFavorite()
        {
            var system = new PersonalBelongingsSystem();
            var item1 = system.RegisterBelonging("dweller_2", "pen", "Fountain Pen", BelongingCategory.Keepsake);
            var item2 = system.RegisterBelonging("dweller_2", "watch", "Pocket Watch", BelongingCategory.Keepsake);
            Assert.NotNull(item1);
            Assert.NotNull(item2);

            system.SetFavorite("dweller_2", item1.BelongingId, true);
            Assert.True(item1.IsFavorite);

            // Setting item2 as favorite unsets item1
            system.SetFavorite("dweller_2", item2.BelongingId, true);
            Assert.False(item1.IsFavorite);
            Assert.True(item2.IsFavorite);
        }

        [Fact]
        public void GiftBelonging_TransfersOwnership_AndRecordsTransfer()
        {
            var system = new PersonalBelongingsSystem();
            var item = system.RegisterBelonging("giver_1", "book", "Old Novel", BelongingCategory.Document, sentimentalValue: 60f);
            Assert.NotNull(item);

            BelongingTransfer? transferEvent = null;
            system.OnBelongingGifted += t => transferEvent = t;

            var transfer = system.GiftBelonging("giver_1", "receiver_1", item.BelongingId, currentDay: 12, reason: "Birthday gift");

            Assert.NotNull(transfer);
            Assert.Equal(transferEvent, transfer);
            Assert.Equal("receiver_1", item.OwnerSurvivorId);
            Assert.Equal(BelongingTransferType.Gift, transfer.TransferType);
            Assert.Equal(6.0f, transfer.SentimentalEffect);

            var receiverItems = system.GetBelongingsForSurvivor("receiver_1");
            Assert.Contains(item, receiverItems);
            var giverItems = system.GetBelongingsForSurvivor("giver_1");
            Assert.DoesNotContain(item, giverItems);
        }

        [Fact]
        public void DistributeInheritanceOnDeath_TransfersAllItems_WithBoostedSentiment()
        {
            var system = new PersonalBelongingsSystem();
            var item1 = system.RegisterBelonging("deceased_1", "knife", "Carved Knife", BelongingCategory.Tool, sentimentalValue: 70f);
            var item2 = system.RegisterBelonging("deceased_1", "ring", "Copper Band", BelongingCategory.Jewelry, sentimentalValue: 50f);
            Assert.NotNull(item1);
            Assert.NotNull(item2);

            var inherited = system.DistributeInheritanceOnDeath("deceased_1", "heir_1", currentDay: 25);

            Assert.Equal(2, inherited.Count);
            Assert.All(inherited, i =>
            {
                Assert.Equal("heir_1", i.OwnerSurvivorId);
                Assert.True(i.IsInherited);
            });

            Assert.Equal(80f, item1.SentimentalValue); // 70 + 10
            Assert.Equal(60f, item2.SentimentalValue); // 50 + 10
        }

        [Fact]
        public void ReportTheftOrLoss_RemovesItem_AndAppliesMoralePenalty()
        {
            var system = new PersonalBelongingsSystem();
            var item = system.RegisterBelonging("dweller_3", "badge", "Sheriff Badge", BelongingCategory.Memento, sentimentalValue: 80f);
            Assert.NotNull(item);
            system.SetFavorite("dweller_3", item.BelongingId, true);

            BelongingEvent? lostEvent = null;
            system.OnBelongingLost += ev => lostEvent = ev;

            bool success = system.ReportTheftOrLoss("dweller_3", item.BelongingId, currentDay: 5, isStolen: true);

            Assert.True(success);
            Assert.NotNull(lostEvent);
            Assert.Equal("item_stolen", lostEvent.EventType);
            Assert.True(lostEvent.MoraleEffect < 0);
            Assert.Empty(system.GetBelongingsForSurvivor("dweller_3"));
        }

        [Fact]
        public void CalculateMoraleBuffer_ScalesWithSentimentConditionAndFavorite()
        {
            var system = new PersonalBelongingsSystem();
            var item = system.RegisterBelonging("dweller_4", "scarf", "Wool Scarf", BelongingCategory.Clothing, sentimentalValue: 80f, condition: 100f);
            Assert.NotNull(item);

            float baseBuffer = system.CalculateMoraleBuffer("dweller_4");
            Assert.True(baseBuffer > 0f);

            // Double effect when favorite
            system.SetFavorite("dweller_4", item.BelongingId, true);
            float favoriteBuffer = system.CalculateMoraleBuffer("dweller_4");
            Assert.Equal(baseBuffer * 2, favoriteBuffer);
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsAccurately()
        {
            var system1 = new PersonalBelongingsSystem();
            var item = system1.RegisterBelonging("survivor_x", "flute", "Tin Whistle", BelongingCategory.Keepsake, 75f, 90f);
            Assert.NotNull(item);
            system1.SetFavorite("survivor_x", item.BelongingId, true);
            system1.GiftBelonging("survivor_x", "survivor_y", item.BelongingId, 4, "Kindness");

            var state = system1.CaptureState();
            Assert.Single(state.Belongings);
            Assert.Single(state.Transfers);

            var system2 = new PersonalBelongingsSystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.TotalBelongingsCount);
            var restored = system2.GetBelonging(item.BelongingId);
            Assert.NotNull(restored);
            Assert.Equal("survivor_y", restored.OwnerSurvivorId);
            Assert.Equal("Tin Whistle", restored.ItemName);
            Assert.Equal(75f, restored.SentimentalValue);
        }

        [Fact]
        public void HasClaimForItem_PreventsDuplicatePhysicalDefinitionClaims()
        {
            var system = new PersonalBelongingsSystem();
            Assert.False(system.HasClaimForItem("locket"));
            var belonging = system.RegisterBelonging("survivor_a", "locket", "Locket", BelongingCategory.Jewelry);
            Assert.NotNull(belonging);
            Assert.True(system.HasClaimForItem("LOCKET"));
            Assert.Single(system.Belongings);
        }
    }
}
