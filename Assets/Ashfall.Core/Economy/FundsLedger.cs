// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Economy
{
    /// <summary>
    /// Failure codes for FundsLedger operations (UNBLOCK-02 F13-A / section 5.1).
    /// </summary>
    public enum FundsFailure
    {
        None = 0,
        InsufficientFunds = 1,
        InvalidAmount = 2,
        UnknownReasonKey = 3,
    }

    /// <summary>
    /// Represents the result of a credit or debit operation on the FundsLedger.
    /// </summary>
    public sealed class FundsResult
    {
        public bool Success { get; }
        public int PreviousBalance { get; }
        public int NewBalance { get; }
        public int Delta { get; }
        public string FailureReason { get; }

        private FundsResult(bool success, int previousBalance, int newBalance, int delta, string failureReason = "")
        {
            Success = success;
            PreviousBalance = previousBalance;
            NewBalance = newBalance;
            Delta = delta;
            FailureReason = failureReason;
        }

        public static FundsResult Succeeded(int previousBalance, int newBalance, int delta)
            => new FundsResult(true, previousBalance, newBalance, delta);

        public static FundsResult Failed(int currentBalance, string reason)
            => new FundsResult(false, currentBalance, currentBalance, 0, reason);
    }

    /// <summary>
    /// An immutable entry in the bounded funds movement log.
    /// </summary>
    [Serializable]
    public sealed class FundsMovementRecord
    {
        public int Day { get; set; }
        public int Delta { get; set; }
        public int ResultingBalance { get; set; }
        public string ReasonKey { get; set; } = string.Empty;
        public string SourceId { get; set; } = string.Empty;

        public FundsMovementRecord() { }

        public FundsMovementRecord(int day, int delta, int resultingBalance, string reasonKey, string sourceId)
        {
            Day = day;
            Delta = delta;
            ResultingBalance = resultingBalance;
            ReasonKey = reasonKey ?? string.Empty;
            SourceId = sourceId ?? string.Empty;
        }
    }

    /// <summary>
    /// Persisted state envelope for FundsLedger.
    /// </summary>
    [Serializable]
    public sealed class FundsLedgerSaveState
    {
        public int schema_version { get; set; } = 1;
        public int balance { get; set; }
        public List<FundsMovementRecord> movements { get; set; } = new List<FundsMovementRecord>();
    }

    /// <summary>
    /// Canonical player-side funds authority (XP-04 / UNBLOCK-02).
    /// Pure domain authority managing integer chits with bounded movement logging
    /// and strict non-negative balance guarantees.
    /// </summary>
    public sealed class FundsLedger
    {
        public const int MaxMovementLogCapacity = 128;

        public const string ReasonBmBuy = "bm_buy";
        public const string ReasonBmSell = "bm_sell";
        public const string ReasonBmFence = "bm_fence";
        public const string ReasonBmContractEscrow = "bm_contract_escrow";
        public const string ReasonBmContractPayout = "bm_contract_payout";
        public const string ReasonBmContractRefund = "bm_contract_refund";
        public const string ReasonHoldfastBuy = "holdfast_buy";
        public const string ReasonHoldfastSell = "holdfast_sell";
        public const string ReasonRouteTariff = "route_tariff";
        public const string ReasonRouteProceeds = "route_proceeds";
        public const string ReasonQuestReward = "quest_reward";
        public const string ReasonDebtSettlement = "debt_settlement";

        private readonly List<FundsMovementRecord> _movements = new List<FundsMovementRecord>();

        public int Balance { get; private set; }

        public IReadOnlyList<FundsMovementRecord> Movements => _movements;

        public event Action<int, int>? OnBalanceChanged;

        public FundsLedger(int initialBalance = 0)
        {
            Balance = Math.Max(0, initialBalance);
        }

        /// <summary>
        /// Attempts to debit a positive amount of chits from the player balance.
        /// Fails if amount is non-positive or if balance is insufficient.
        /// </summary>
        public FundsResult TryDebit(int amount, string reasonKey, string sourceId, int day = 0)
        {
            if (amount <= 0)
            {
                return FundsResult.Failed(Balance, "Amount must be greater than zero.");
            }

            if (Balance < amount)
            {
                return FundsResult.Failed(Balance, "InsufficientFunds");
            }

            int prev = Balance;
            Balance -= amount;

            RecordMovement(day, -amount, Balance, reasonKey, sourceId);
            OnBalanceChanged?.Invoke(prev, Balance);

            return FundsResult.Succeeded(prev, Balance, -amount);
        }

        /// <summary>
        /// Attempts to credit a positive amount of chits to the player balance.
        /// Fails if amount is non-positive.
        /// </summary>
        public FundsResult TryCredit(int amount, string reasonKey, string sourceId, int day = 0)
        {
            if (amount <= 0)
            {
                return FundsResult.Failed(Balance, "Amount must be greater than zero.");
            }

            // Guard against integer overflow
            if ((long)Balance + amount > int.MaxValue)
            {
                return FundsResult.Failed(Balance, "BalanceOverflow");
            }

            int prev = Balance;
            Balance += amount;

            RecordMovement(day, amount, Balance, reasonKey, sourceId);
            OnBalanceChanged?.Invoke(prev, Balance);

            return FundsResult.Succeeded(prev, Balance, amount);
        }

        private void RecordMovement(int day, int delta, int resultingBalance, string reasonKey, string sourceId)
        {
            if (_movements.Count >= MaxMovementLogCapacity)
            {
                _movements.RemoveAt(0); // Oldest-first eviction
            }

            _movements.Add(new FundsMovementRecord(day, delta, resultingBalance, reasonKey, sourceId));
        }

        /// <summary>
        /// Captures the current ledger state for persistence.
        /// </summary>
        public FundsLedgerSaveState CaptureState()
        {
            var state = new FundsLedgerSaveState
            {
                schema_version = 1,
                balance = Balance,
                movements = new List<FundsMovementRecord>(_movements.Count)
            };

            for (int i = 0; i < _movements.Count; i++)
            {
                var m = _movements[i];
                state.movements.Add(new FundsMovementRecord(m.Day, m.Delta, m.ResultingBalance, m.ReasonKey, m.SourceId));
            }

            return state;
        }

        /// <summary>
        /// Restores ledger state from persistence.
        /// </summary>
        public void RestoreState(FundsLedgerSaveState? state)
        {
            _movements.Clear();

            if (state == null)
            {
                Balance = 0;
                return;
            }

            Balance = Math.Max(0, state.balance);

            if (state.movements != null)
            {
                int start = Math.Max(0, state.movements.Count - MaxMovementLogCapacity);
                for (int i = start; i < state.movements.Count; i++)
                {
                    var m = state.movements[i];
                    if (m != null)
                    {
                        _movements.Add(new FundsMovementRecord(m.Day, m.Delta, m.ResultingBalance, m.ReasonKey, m.SourceId));
                    }
                }
            }
        }
    }
}
