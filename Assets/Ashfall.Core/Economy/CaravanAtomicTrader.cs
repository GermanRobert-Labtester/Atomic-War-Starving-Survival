using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Economy
{
    /// <summary>
    /// ASHFALL Caravan Trading atomic quote/commit (item 12).
    ///
    /// Wraps an existing trade session with an atomic quote/commit flow
    /// that prevents duplicate transaction rewards. The host builds a
    /// quote, validates it, and commits only after the player confirms.
    /// A committed quote is locked: re-committing the same quote id
    /// fails with <see cref="QuoteAlreadyCommitted"/>.
    /// </summary>
    public sealed class CaravanAtomicTrader
    {
        private readonly CaravanTradeState _state;

        public event Action<CaravanCommittedTrade>? OnCommitted;

        public CaravanAtomicTrader(CaravanTradeState state)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
        }

        public IReadOnlyList<CaravanCommittedTrade> Committed => _state.Committed;

        public bool IsCommitted(string quoteId)
        {
            if (string.IsNullOrEmpty(quoteId)) return false;
            for (int i = 0; i < _state.Committed.Count; i++)
                if (_state.Committed[i].QuoteId == quoteId) return true;
            return false;
        }

        public CaravanTradeCommitResult Commit(CaravanTradeQuote quote)
        {
            if (quote == null) throw new ArgumentNullException(nameof(quote));
            if (string.IsNullOrEmpty(quote.QuoteId))
                return CaravanTradeCommitResult.Fail("missing_quote_id");
            if (IsCommitted(quote.QuoteId))
                return CaravanTradeCommitResult.Fail("already_committed");
            if (quote.OfferedUnits <= 0 || quote.RequestedUnits <= 0)
                return CaravanTradeCommitResult.Fail("invalid_units");
            if (quote.PriceMultiplier <= 0)
                return CaravanTradeCommitResult.Fail("invalid_price");

            var trade = new CaravanCommittedTrade
            {
                QuoteId = quote.QuoteId,
                OfferedItemId = quote.OfferedItemId,
                OfferedUnits = quote.OfferedUnits,
                RequestedItemId = quote.RequestedItemId,
                RequestedUnits = quote.RequestedUnits,
                Day = quote.Day,
                RegionId = quote.RegionId,
                StanceId = quote.StanceId
            };
            _state.Committed.Add(trade);
            OnCommitted?.Invoke(trade);
            return CaravanTradeCommitResult.Ok(trade);
        }

        public CaravanTradeState CaptureState() => _state.Capture();

        public void RestoreState(CaravanTradeState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            _state.RestoreInto(state);
        }
    }

    [Serializable]
    public sealed class CaravanTradeQuote
    {
        public string QuoteId;
        public string OfferedItemId;
        public int OfferedUnits;
        public string RequestedItemId;
        public int RequestedUnits;
        public float PriceMultiplier;
        public string RegionId;
        public string StanceId;
        public int Day;
    }

    [Serializable]
    public sealed class CaravanCommittedTrade
    {
        public string QuoteId;
        public string OfferedItemId;
        public int OfferedUnits;
        public string RequestedItemId;
        public int RequestedUnits;
        public string RegionId;
        public string StanceId;
        public int Day;
    }

    [Serializable]
    public sealed class CaravanTradeState
    {
        public List<CaravanCommittedTrade> Committed = new List<CaravanCommittedTrade>();

        private static CaravanCommittedTrade CloneTrade(CaravanCommittedTrade src)
        {
            if (src == null) return null!;
            return new CaravanCommittedTrade
            {
                QuoteId = src.QuoteId,
                OfferedItemId = src.OfferedItemId,
                OfferedUnits = src.OfferedUnits,
                RequestedItemId = src.RequestedItemId,
                RequestedUnits = src.RequestedUnits,
                RegionId = src.RegionId,
                StanceId = src.StanceId,
                Day = src.Day
            };
        }

        public CaravanTradeState Capture()
        {
            var clone = new CaravanTradeState();
            if (Committed != null)
            {
                clone.Committed = new List<CaravanCommittedTrade>(Committed.Count);
                for (int i = 0; i < Committed.Count; i++)
                    clone.Committed.Add(CloneTrade(Committed[i]));
            }
            return clone;
        }

        public void RestoreInto(CaravanTradeState state)
        {
            if (state == null || state.Committed == null)
            {
                Committed = new List<CaravanCommittedTrade>();
                return;
            }
            Committed = new List<CaravanCommittedTrade>(state.Committed.Count);
            for (int i = 0; i < state.Committed.Count; i++)
                Committed.Add(CloneTrade(state.Committed[i]));
        }
    }

    [Serializable]
    public sealed class CaravanTradeCommitResult
    {
        public bool Succeeded;
        public string ReasonCode;
        public CaravanCommittedTrade Trade;

        public static CaravanTradeCommitResult Ok(CaravanCommittedTrade t)
            => new CaravanTradeCommitResult { Succeeded = true, ReasonCode = "ok", Trade = t };

        public static CaravanTradeCommitResult Fail(string reason)
            => new CaravanTradeCommitResult { Succeeded = false, ReasonCode = reason ?? "fail", Trade = null! };
    }
}
