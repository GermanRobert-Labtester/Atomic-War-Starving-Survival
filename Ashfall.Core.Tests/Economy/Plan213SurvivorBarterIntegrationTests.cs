// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public sealed class Plan213SurvivorBarterIntegrationTests
    {
        [Fact]
        public void LoadCatalog_LoadsAllBarterRules_FromValidJson()
        {
            var system = new SurvivorBarterSystem();
            string path = Path.Combine("..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "barter_rules.json");
            if (!File.Exists(path))
            {
                path = Path.Combine("Assets", "StreamingAssets", "Data", "barter_rules.json");
            }
            Assert.True(File.Exists(path), $"Catalog not found at {path}");

            string json = File.ReadAllText(path);
            system.LoadCatalog(json);

            Assert.Equal(3, system.Rules.Count);
            var standard = system.GetRule("standard_community_barter");
            Assert.NotNull(standard);
            Assert.Equal("Standard Community Barter", standard.Name);
            Assert.Equal(5.0f, standard.BaseTrustGainPerTrade);
            Assert.Equal(20.0f, standard.DisputeTrustPenalty);
            Assert.Equal(5, standard.MaxActiveOffersPerSurvivor);
            Assert.Equal(14, standard.FavorDeadlineDays);

            var strict = system.GetRule("strict_rationing_barter");
            Assert.NotNull(strict);
            Assert.Equal(35.0f, strict.DisputeTrustPenalty);
        }

        [Fact]
        public void CreateOffer_ValidatesPossession_AndRegistersOffer()
        {
            var system = new SurvivorBarterSystem();

            // Setup inventory mock: survivor_a owns "pocket_knife", but not "silver_watch"
            var inventory = new HashSet<(string, string)>
            {
                ("survivor_a", "pocket_knife"),
                ("survivor_b", "canned_peaches")
            };
            system.HasPersonalItem = (survivor, item) => inventory.Contains((survivor, item));

            // Fails because survivor_a doesn't own silver_watch
            var failedOffer = system.CreateOffer(
                offererId: "survivor_a",
                targetId: "survivor_b",
                offeredItems: new[] { "silver_watch" },
                requestedItems: new[] { "canned_peaches" });
            Assert.Null(failedOffer);

            // Succeeds when offered items are in possession
            var validOffer = system.CreateOffer(
                offererId: "survivor_a",
                targetId: "survivor_b",
                offeredItems: new[] { "pocket_knife" },
                requestedItems: new[] { "canned_peaches" },
                currentDay: 2);

            Assert.NotNull(validOffer);
            Assert.Equal("survivor_a", validOffer.OffererId);
            Assert.Equal("survivor_b", validOffer.TargetSurvivorId);
            Assert.Single(validOffer.OfferedItemIds);
            Assert.Equal("pocket_knife", validOffer.OfferedItemIds[0]);
            Assert.Equal(BarterOfferStatus.Pending, validOffer.Status);
            Assert.Equal(2, validOffer.CreatedDay);
            Assert.Equal(7, validOffer.ExpiresDay); // 2 + 5
        }

        [Fact]
        public void AcceptOffer_ExecutesItemTransfer_AndIncreasesReputation()
        {
            var system = new SurvivorBarterSystem();
            var transfers = new List<(string from, string to, string item)>();
            system.TransferPersonalItem = (from, to, item) => transfers.Add((from, to, item));

            var offer = system.CreateOffer(
                offererId: "dweller_1",
                targetId: "dweller_2",
                offeredItems: new[] { "cloth_scrap" },
                requestedItems: new[] { "clean_water" },
                currentDay: 5);
            Assert.NotNull(offer);

            CompletedBarterTrade? tradeRecorded = null;
            system.OnOfferAccepted += (o, t) => tradeRecorded = t;

            var trade = system.AcceptOffer(offer.OfferId, currentDay: 5);

            Assert.NotNull(trade);
            Assert.Equal(tradeRecorded, trade);
            Assert.Equal(BarterOfferStatus.Accepted, offer.Status);
            Assert.Equal(TradeType.ItemExchange, trade.TradeType);

            // Transfers verified
            Assert.Equal(2, transfers.Count);
            Assert.Contains(("dweller_1", "dweller_2", "cloth_scrap"), transfers);
            Assert.Contains(("dweller_2", "dweller_1", "clean_water"), transfers);

            // Pairwise reputation increased: default 50 + 5 = 55
            var rep = system.GetReputation("dweller_1", "dweller_2");
            Assert.Equal(55f, rep.TrustLevel);
            Assert.Equal(1, rep.TradesCompleted);
        }

        [Fact]
        public void AcceptOffer_WithFavors_CreatesFavorObligations()
        {
            var system = new SurvivorBarterSystem();

            // Dweller A offers a repair favor in exchange for Dweller B's medical assistance
            var offer = system.CreateOffer(
                offererId: "dweller_mason",
                targetId: "dweller_medic",
                offeredItems: null,
                requestedItems: null,
                offeredFavor: FavorType.CraftingAssistance,
                requestedFavor: FavorType.MedicalCare,
                currentDay: 10);
            Assert.NotNull(offer);

            var trade = system.AcceptOffer(offer.OfferId, currentDay: 10);

            Assert.NotNull(trade);
            Assert.Equal(TradeType.FavorExchange, trade.TradeType);

            var masonFavors = system.GetPendingFavors("dweller_mason");
            Assert.Equal(2, masonFavors.Count);

            var masonDebt = masonFavors.FirstOrDefault(f => f.DebtorId == "dweller_mason");
            Assert.NotNull(masonDebt);
            Assert.Equal("dweller_medic", masonDebt.CreditorId);
            Assert.Equal(FavorType.CraftingAssistance, masonDebt.Favor);
            Assert.Equal(24, masonDebt.DueDay); // 10 + 14
            Assert.False(masonDebt.IsFulfilled);

            var medicDebt = masonFavors.FirstOrDefault(f => f.DebtorId == "dweller_medic");
            Assert.NotNull(medicDebt);
            Assert.Equal("dweller_mason", medicDebt.CreditorId);
            Assert.Equal(FavorType.MedicalCare, medicDebt.Favor);
        }

        [Fact]
        public void FulfillFavor_UpdatesLedger_AndIncreasesTrust()
        {
            var system = new SurvivorBarterSystem();

            var offer = system.CreateOffer(
                offererId: "dweller_cook",
                targetId: "dweller_scout",
                offeredFavor: FavorType.ChoreShift,
                currentDay: 1);
            Assert.NotNull(offer);
            system.AcceptOffer(offer.OfferId, currentDay: 1);

            var pending = system.GetPendingFavors("dweller_cook");
            Assert.Single(pending);
            string favorId = pending[0].FavorId;

            FavorObligation? fulfilledEvent = null;
            system.OnFavorFulfilled += f => fulfilledEvent = f;

            bool success = system.FulfillFavor(favorId, currentDay: 4);

            Assert.True(success);
            Assert.NotNull(fulfilledEvent);
            Assert.True(fulfilledEvent.IsFulfilled);
            Assert.Equal(4, fulfilledEvent.FulfilledDay);

            // Trust gained for fulfilling favor: 50 (start) + 5 (trade) + 4 (favor fulfilled) = 59
            var rep = system.GetReputation("dweller_cook", "dweller_scout");
            Assert.Equal(59f, rep.TrustLevel);
        }

        [Fact]
        public void RaiseDispute_AppliesTrustPenalty_ToBothTraders()
        {
            var system = new SurvivorBarterSystem();

            var offer = system.CreateOffer("trader_x", "trader_y", offeredItems: new[] { "radio_part" });
            Assert.NotNull(offer);
            var trade = system.AcceptOffer(offer.OfferId, currentDay: 3);
            Assert.NotNull(trade);

            var repBefore = system.GetReputation("trader_x", "trader_y");
            Assert.Equal(55f, repBefore.TrustLevel); // 50 + 5

            string? disputedTradeId = null;
            string? complainant = null;
            system.OnTradeDisputed += (t, c) =>
            {
                disputedTradeId = t;
                complainant = c;
            };

            bool success = system.RaiseDispute(trade.TradeId, "trader_y");

            Assert.True(success);
            Assert.Equal(trade.TradeId, disputedTradeId);
            Assert.Equal("trader_y", complainant);

            // Trust penalty: 55 - 20 = 35
            var repAfter = system.GetReputation("trader_x", "trader_y");
            Assert.Equal(35f, repAfter.TrustLevel);
            Assert.Equal(1, repAfter.DisputesCount);
        }

        [Fact]
        public void CaptureAndRestoreState_RoundTripsAllBarterData()
        {
            var system = new SurvivorBarterSystem();
            system.LoadCatalog(@"{
                ""schema_version"": 1,
                ""rules"": [
                    { ""id"": ""strict_rationing_barter"", ""name"": ""Strict"", ""base_trust_gain_per_trade"": 3.0 }
                ]
            }");
            system.SetRule("strict_rationing_barter");
            var offer = system.CreateOffer("surv_1", "surv_2", offeredItems: new[] { "meds" }, currentDay: 1);
            Assert.NotNull(offer);
            system.AcceptOffer(offer.OfferId, currentDay: 1);

            var save = system.CaptureState();
            Assert.Equal("strict_rationing_barter", save.ActiveRuleId);
            Assert.Single(save.Offers);
            Assert.Single(save.CompletedTrades);
            Assert.Single(save.Reputations);

            var restored = new SurvivorBarterSystem();
            restored.RestoreState(save);

            Assert.Equal("strict_rationing_barter", restored.ActiveRuleId);
            Assert.Single(restored.Offers);
            Assert.Equal(BarterOfferStatus.Accepted, restored.Offers[0].Status);
            Assert.Single(restored.CompletedTrades);
            Assert.Single(restored.Reputations);
            Assert.Equal(53f, restored.GetReputation("surv_1", "surv_2").TrustLevel); // 50 + 3
        }
    }
}
