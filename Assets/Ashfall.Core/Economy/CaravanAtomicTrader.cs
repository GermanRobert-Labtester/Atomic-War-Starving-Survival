using System;
using System.Collections.Generic;

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

        public CaravanTradeState Capture() => new CaravanTradeState
        {
            Committed = new List<CaravanCommittedTrade>(Committed)
        };

        public void RestoreInto(CaravanTradeState state)
        {
            Committed = state.Committed ?? new List<CaravanCommittedTrade>();
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
