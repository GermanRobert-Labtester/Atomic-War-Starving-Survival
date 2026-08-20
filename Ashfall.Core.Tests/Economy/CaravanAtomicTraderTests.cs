using System;
using System.Collections.Generic;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public class CaravanAtomicTraderTests
    {
        private static CaravanTradeQuote MakeQuote(string id = "q1")
        {
            return new CaravanTradeQuote
            {
                QuoteId = id,
                OfferedItemId = "canned_food",
                OfferedUnits = 5,
                RequestedItemId = "clean_water",
                RequestedUnits = 3,
                PriceMultiplier = 1f,
                RegionId = "loc_cut_merchant_caravanserai",
                StanceId = "neutral",
                Day = 4
            };
        }

        [Fact]
        public void Commit_AddsTrade()
        {
            var t = new CaravanAtomicTrader(new CaravanTradeState());
            var r = t.Commit(MakeQuote());
            Assert.True(r.Succeeded);
            Assert.Single(t.Committed);
        }

        [Fact]
        public void Commit_IsIdempotent_NoDuplicateTrade()
        {
            var t = new CaravanAtomicTrader(new CaravanTradeState());
            t.Commit(MakeQuote());
            var second = t.Commit(MakeQuote());
            Assert.False(second.Succeeded);
            Assert.Equal("already_committed", second.ReasonCode);
            Assert.Single(t.Committed);
        }

        [Fact]
        public void Commit_ValidatesInputs()
        {
            var t = new CaravanAtomicTrader(new CaravanTradeState());
            Assert.False(t.Commit(new CaravanTradeQuote { QuoteId = "" }).Succeeded);
            Assert.False(t.Commit(new CaravanTradeQuote { QuoteId = "q2", OfferedUnits = 0 }).Succeeded);
            Assert.False(t.Commit(new CaravanTradeQuote
            {
                QuoteId = "q2", OfferedUnits = 1, RequestedUnits = 0
            }).Succeeded);
            Assert.False(t.Commit(new CaravanTradeQuote
            {
                QuoteId = "q2", OfferedUnits = 1, RequestedUnits = 1, PriceMultiplier = 0
            }).Succeeded);
        }

        [Fact]
        public void IsCommitted_TrueAfterCommit()
        {
            var t = new CaravanAtomicTrader(new CaravanTradeState());
            Assert.False(t.IsCommitted("q1"));
            t.Commit(MakeQuote());
            Assert.True(t.IsCommitted("q1"));
        }

        [Fact]
        public void DifferentQuotes_BothCommit()
        {
            var t = new CaravanAtomicTrader(new CaravanTradeState());
            t.Commit(MakeQuote("q1"));
            t.Commit(MakeQuote("q2"));
            Assert.Equal(2, t.Committed.Count);
        }

        [Fact]
        public void CaptureRestore_RoundTrip()
        {
            var t = new CaravanAtomicTrader(new CaravanTradeState());
            t.Commit(MakeQuote("q1"));
            var save = t.CaptureState();
            var fresh = new CaravanAtomicTrader(new CaravanTradeState());
            fresh.RestoreState(save);
            Assert.True(fresh.IsCommitted("q1"));
        }

        [Fact]
        public void Events_FireOnCommit()
        {
            var t = new CaravanAtomicTrader(new CaravanTradeState());
            CaravanCommittedTrade? captured = null;
            t.OnCommitted += tr => captured = tr;
            t.Commit(MakeQuote("q1"));
            Assert.NotNull(captured);
            Assert.Equal("q1", captured.QuoteId);
        }

        [Fact]
        public void DuplicateCommit_DoesNotFireEvent()
        {
            var t = new CaravanAtomicTrader(new CaravanTradeState());
            int fired = 0;
            t.OnCommitted += _ => fired++;
            t.Commit(MakeQuote("q1"));
            t.Commit(MakeQuote("q1"));
            Assert.Equal(1, fired);
        }
    }
}
