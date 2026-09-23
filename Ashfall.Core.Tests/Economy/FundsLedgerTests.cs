// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Xunit;
using Ashfall.Core.Economy;

namespace Ashfall.Core.Tests
{
    public sealed class FundsLedgerTests
    {
        [Fact]
        public void InitialBalance_DefaultsToZeroOrProvidedValue()
        {
            var defaultLedger = new FundsLedger();
            Assert.Equal(0, defaultLedger.Balance);
            Assert.Empty(defaultLedger.Movements);

            var customLedger = new FundsLedger(500);
            Assert.Equal(500, customLedger.Balance);
            Assert.Empty(customLedger.Movements);

            var negativeClamp = new FundsLedger(-100);
            Assert.Equal(0, negativeClamp.Balance);
        }

        [Fact]
        public void TryCredit_ValidAmount_IncreasesBalanceAndLogsMovement()
        {
            var ledger = new FundsLedger(100);
            bool eventFired = false;
            int oldVal = 0, newVal = 0;

            ledger.OnBalanceChanged += (o, n) =>
            {
                eventFired = true;
                oldVal = o;
                newVal = n;
            };

            var result = ledger.TryCredit(50, "scavenge_bounty", "source_holdfast", day: 5);

            Assert.True(result.Success);
            Assert.Equal(100, result.PreviousBalance);
            Assert.Equal(150, result.NewBalance);
            Assert.Equal(50, result.Delta);
            Assert.Equal(150, ledger.Balance);

            Assert.True(eventFired);
            Assert.Equal(100, oldVal);
            Assert.Equal(150, newVal);

            Assert.Single(ledger.Movements);
            var m = ledger.Movements[0];
            Assert.Equal(5, m.Day);
            Assert.Equal(50, m.Delta);
            Assert.Equal(150, m.ResultingBalance);
            Assert.Equal("scavenge_bounty", m.ReasonKey);
            Assert.Equal("source_holdfast", m.SourceId);
        }

        [Fact]
        public void TryDebit_SufficientFunds_DecreasesBalanceAndLogsMovement()
        {
            var ledger = new FundsLedger(200);

            var result = ledger.TryDebit(75, "buy_rations", "merchant_caravan", day: 12);

            Assert.True(result.Success);
            Assert.Equal(200, result.PreviousBalance);
            Assert.Equal(125, result.NewBalance);
            Assert.Equal(-75, result.Delta);
            Assert.Equal(125, ledger.Balance);

            Assert.Single(ledger.Movements);
            var m = ledger.Movements[0];
            Assert.Equal(12, m.Day);
            Assert.Equal(-75, m.Delta);
            Assert.Equal(125, m.ResultingBalance);
            Assert.Equal("buy_rations", m.ReasonKey);
            Assert.Equal("merchant_caravan", m.SourceId);
        }

        [Fact]
        public void TryDebit_InsufficientFunds_FailsWithoutMutatingBalanceOrLog()
        {
            var ledger = new FundsLedger(50);

            var result = ledger.TryDebit(100, "expensive_mod", "black_market", day: 20);

            Assert.False(result.Success);
            Assert.Equal("InsufficientFunds", result.FailureReason);
            Assert.Equal(50, result.PreviousBalance);
            Assert.Equal(50, result.NewBalance);
            Assert.Equal(50, ledger.Balance);
            Assert.Empty(ledger.Movements);
        }

        [Fact]
        public void NonPositiveAmounts_FailWithValidationMessage()
        {
            var ledger = new FundsLedger(100);

            var debitZero = ledger.TryDebit(0, "test", "src");
            Assert.False(debitZero.Success);
            Assert.Contains("greater than zero", debitZero.FailureReason);

            var debitNeg = ledger.TryDebit(-10, "test", "src");
            Assert.False(debitNeg.Success);
            Assert.Contains("greater than zero", debitNeg.FailureReason);

            var creditZero = ledger.TryCredit(0, "test", "src");
            Assert.False(creditZero.Success);
            Assert.Contains("greater than zero", creditZero.FailureReason);

            var creditNeg = ledger.TryCredit(-10, "test", "src");
            Assert.False(creditNeg.Success);
            Assert.Contains("greater than zero", creditNeg.FailureReason);

            Assert.Equal(100, ledger.Balance);
            Assert.Empty(ledger.Movements);
        }

        [Fact]
        public void MovementLog_EvictsOldestEntriesWhenExceeding128()
        {
            var ledger = new FundsLedger(0);

            for (int i = 1; i <= 140; i++)
            {
                ledger.TryCredit(10, $"credit_{i}", "source", day: i);
            }

            Assert.Equal(1400, ledger.Balance);
            Assert.Equal(FundsLedger.MaxMovementLogCapacity, ledger.Movements.Count);
            Assert.Equal(128, ledger.Movements.Count);

            // First entry in log should be i = 13 (since 140 - 128 = 12 evicted)
            Assert.Equal(13, ledger.Movements[0].Day);
            Assert.Equal("credit_13", ledger.Movements[0].ReasonKey);

            // Last entry in log should be i = 140
            Assert.Equal(140, ledger.Movements[127].Day);
            Assert.Equal("credit_140", ledger.Movements[127].ReasonKey);
        }

        [Fact]
        public void SaveRestoreRoundTrip_PreservesBalanceAndMovements()
        {
            var ledger = new FundsLedger(500);
            ledger.TryDebit(100, "ammo_purchase", "merchant", day: 3);
            ledger.TryCredit(250, "salvage_turnin", "holdfast", day: 7);

            var state = ledger.CaptureState();
            Assert.Equal(1, state.schema_version);
            Assert.Equal(650, state.balance);
            Assert.Equal(2, state.movements.Count);

            var restoredLedger = new FundsLedger();
            restoredLedger.RestoreState(state);

            Assert.Equal(650, restoredLedger.Balance);
            Assert.Equal(2, restoredLedger.Movements.Count);
            Assert.Equal("ammo_purchase", restoredLedger.Movements[0].ReasonKey);
            Assert.Equal("salvage_turnin", restoredLedger.Movements[1].ReasonKey);
        }

        [Fact]
        public void RestoreState_NullOrEmpty_ResetsBalanceSafely()
        {
            var ledger = new FundsLedger(500);
            ledger.TryCredit(50, "reward", "gov", day: 1);

            ledger.RestoreState(null);
            Assert.Equal(0, ledger.Balance);
            Assert.Empty(ledger.Movements);
        }
    }
}
