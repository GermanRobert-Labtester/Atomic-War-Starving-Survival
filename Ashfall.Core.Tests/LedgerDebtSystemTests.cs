using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Tests for the LedgerDebtSystem (Nobody's Charter §5.3): read-twice
    /// signing, ink-freeze, term expiry → named forfeit, flat-rate total,
    /// pay (before and after forfeit), renegotiation as replacement-not-
    /// amendment, one-shot tampering, and the port-based save roundtrip.
    /// Debtor ids are master-list ids from characters.json; Wyn's forfeit is
    /// her pledged grain per the bible.
    /// </summary>
    public class LedgerDebtSystemTests
    {
        private const string Wyn = "npc_wyn_sabler";
        private const string Ivo = "npc_ivo_fenn";

        private static LedgerDebtSystem Fixture()
        {
            return new LedgerDebtSystem();
        }

        private static bool ReadTwice(LedgerDebtSystem ledger, string debtor,
            float principal, int termDays, float rate, string forfeit)
        {
            return ledger.PresentContract(debtor, principal, termDays, rate, forfeit)
                && ledger.PresentContract(debtor, principal, termDays, rate, forfeit);
        }

        [Fact]
        public void PresentRejectsInvalidTerms()
        {
            var ledger = Fixture();
            Assert.False(ledger.PresentContract("", 10f, 30, 0.1f, "grain"));
            Assert.False(ledger.PresentContract(Wyn, 0f, 30, 0.1f, "grain"));
            Assert.False(ledger.PresentContract(Wyn, 10f, 0, 0.1f, "grain"));
            Assert.False(ledger.PresentContract(Wyn, 10f, 30, 0.1f, ""));
            Assert.Empty(ledger.Contracts);
        }

        [Fact]
        public void SigningRequiresTwoReadings()
        {
            var ledger = Fixture();
            Assert.True(ledger.PresentContract(Wyn, 12f, 30, 0.2f, "the pledged grain"));
            Assert.False(ledger.SignContract(Wyn, 40), "one reading is not ink");

            Assert.True(ledger.PresentContract(Wyn, 12f, 30, 0.2f, "the pledged grain"));
            Assert.True(ledger.SignContract(Wyn, 40), "the second reading closes it");
            Assert.Equal(2, ledger.GetContract(Wyn).readCount);
        }

        [Fact]
        public void SignedContractFreezesTermsAndBlocksNewDraft()
        {
            var ledger = Fixture();
            Assert.True(ReadTwice(ledger, Wyn, 12f, 30, 0.2f, "the pledged grain"));
            Assert.True(ledger.SignContract(Wyn, 40));

            Assert.False(ledger.PresentContract(Wyn, 999f, 1, 0.9f, "anything"),
                "no new draft over an open debt");
            Assert.Equal(12f, ledger.GetContract(Wyn).principal);
            Assert.Equal(30, ledger.GetContract(Wyn).daysRemaining);
            Assert.Equal(40, ledger.GetContract(Wyn).signedDay);
        }

        [Fact]
        public void TermExpiryTriggersNamedForfeit()
        {
            var ledger = Fixture();
            Assert.True(ReadTwice(ledger, Wyn, 10f, 3, 0.1f, "the pledged grain"));
            Assert.True(ledger.SignContract(Wyn, 10));

            DebtContract forfeited = null;
            ledger.OnForfeitTriggered += c => forfeited = c;

            ledger.TickDaily(11);
            ledger.TickDaily(12);
            Assert.False(ledger.GetContract(Wyn).forfeited);
            ledger.TickDaily(13);
            Assert.True(ledger.GetContract(Wyn).forfeited);
            Assert.NotNull(forfeited);
            Assert.Equal("the pledged grain", forfeited.forfeit);
        }

        [Fact]
        public void PayResolvesBeforeForfeit()
        {
            var ledger = Fixture();
            Assert.True(ReadTwice(ledger, Wyn, 10f, 3, 0.1f, "the pledged grain"));
            Assert.True(ledger.SignContract(Wyn, 10));

            DebtContract paid = null;
            ledger.OnContractPaid += c => paid = c;
            Assert.True(ledger.PayContract(Wyn, 11));
            Assert.True(ledger.GetContract(Wyn).paid);
            Assert.NotNull(paid);

            // Expiry no longer applies to a settled debt.
            for (int d = 0; d < 5; d++) ledger.TickDaily(12 + d);
            Assert.False(ledger.GetContract(Wyn).forfeited);
        }

        [Fact]
        public void PayStillHonoursAfterForfeitDue()
        {
            var ledger = Fixture();
            Assert.True(ReadTwice(ledger, Wyn, 10f, 1, 0.1f, "the pledged grain"));
            Assert.True(ledger.SignContract(Wyn, 10));
            ledger.TickDaily(11);
            Assert.True(ledger.GetContract(Wyn).forfeited);

            Assert.True(ledger.PayContract(Wyn, 12), "paying back the named good is the honoured path");
            Assert.True(ledger.GetContract(Wyn).paid);
            Assert.False(ledger.GetContract(Wyn).forfeited);
        }

        [Fact]
        public void CannotPayUnsignedOrTwice()
        {
            var ledger = Fixture();
            Assert.True(ReadTwice(ledger, Wyn, 10f, 30, 0.1f, "the pledged grain"));
            Assert.False(ledger.PayContract(Wyn, 40), "draft is not a debt yet");

            Assert.True(ledger.SignContract(Wyn, 40));
            Assert.True(ledger.PayContract(Wyn, 41));
            Assert.False(ledger.PayContract(Wyn, 42), "already paid in full");
        }

        [Fact]
        public void RenegotiationReplacesAndRequiresFreshReadings()
        {
            var ledger = Fixture();

            // Draft path: torn up before ink, new terms need two fresh readings.
            Assert.True(ledger.PresentContract(Wyn, 10f, 30, 0.1f, "the pledged grain"));

            DebtContract renegotiated = null;
            ledger.OnContractRenegotiated += c => renegotiated = c;
            Assert.True(ledger.RenegotiateContract(Wyn, 6f, 60, 0.15f, "two weeks at the Lockup"));

            var contract = ledger.GetContract(Wyn);
            Assert.False(contract.signed, "the draft is replaced, not amended");
            Assert.Equal(0, contract.readCount);
            Assert.Equal(6f, contract.principal);
            Assert.Equal("two weeks at the Lockup", contract.forfeit);
            Assert.NotNull(renegotiated);

            Assert.False(ledger.SignContract(Wyn, 41), "new terms still need two readings");
            Assert.True(ledger.PresentContract(Wyn, 6f, 60, 0.15f, "two weeks at the Lockup"));
            Assert.True(ledger.PresentContract(Wyn, 6f, 60, 0.15f, "two weeks at the Lockup"));
            Assert.True(ledger.SignContract(Wyn, 42));

            // Live ink stands: no silent amendment of an open debt.
            Assert.False(ledger.RenegotiateContract(Wyn, 1f, 1, 0f, "anything"),
                "after the second reading there is only the ink");
        }

        [Fact]
        public void RenegotiationRejectsPaidForfeitedAndUnknown()
        {
            var ledger = Fixture();
            Assert.False(ledger.RenegotiateContract(Ivo, 1f, 10, 0f, "anything"),
                "no contract to renegotiate");

            Assert.True(ReadTwice(ledger, Wyn, 10f, 1, 0.1f, "the pledged grain"));
            Assert.True(ledger.SignContract(Wyn, 10));
            ledger.TickDaily(11);
            Assert.False(ledger.RenegotiateContract(Wyn, 1f, 10, 0f, "anything"),
                "forfeit pending — ink stands");

            Assert.True(ledger.PayContract(Wyn, 12));
            Assert.False(ledger.RenegotiateContract(Wyn, 1f, 10, 0f, "anything"),
                "settled debts are closed");
        }

        [Fact]
        public void TamperIsOneShot()
        {
            var ledger = Fixture();
            int tampered = 0;
            ledger.OnLedgerTampered += () => tampered++;

            Assert.True(ledger.TamperLedger());
            Assert.False(ledger.TamperLedger(), "one strike per playthrough");
            Assert.Equal(1, tampered);
            Assert.True(ledger.LedgerTampered);
        }

        [Fact]
        public void TotalOwedIsFlatRate()
        {
            var ledger = Fixture();
            Assert.True(ledger.TotalOwed(Wyn) == 0f, "no debt yet");

            Assert.True(ReadTwice(ledger, Wyn, 100f, 30, 0.25f, "the pledged grain"));
            Assert.True(ledger.SignContract(Wyn, 10));
            Assert.Equal(125.0, (double)ledger.TotalOwed(Wyn), 2);
        }

        [Fact]
        public void TermEndRenegotiationExtendsSignedInk()
        {
            var ledger = Fixture();
            Assert.True(ReadTwice(ledger, Wyn, 12f, 3, 0.2f, "the pledged grain"));
            Assert.True(ledger.SignContract(Wyn, 10));

            // Mid-term: no silent amendment of live ink.
            Assert.False(ledger.RenegotiateContract(Wyn, 10f, 10, 0.1f, "the pledged grain"));

            // Run to the last day of the term.
            ledger.TickDaily(11);
            ledger.TickDaily(12); // daysRemaining == 1
            Assert.Equal(1, ledger.GetContract(Wyn).daysRemaining);

            // Term-end renegotiation extends the ink, stays signed, forfeit named.
            Assert.True(ledger.RenegotiateContract(Wyn, 10f, 10, 0.1f, "the pledged grain"));
            var c = ledger.GetContract(Wyn);
            Assert.True(c.signed);
            Assert.Equal(10, c.termDays);
            Assert.Equal(10, c.daysRemaining);
            Assert.Equal("the pledged grain", c.forfeit);

            // The extended term runs down before the forfeit comes due.
            for (int d = 0; d < 9; d++) ledger.TickDaily(13 + d);
            Assert.False(c.forfeited);
            ledger.TickDaily(22);
            Assert.True(c.forfeited);
        }

        [Fact]
        public void SaveRoundTripPreservesContractsAndTamper()
        {
            var ledger = Fixture();
            Assert.True(ReadTwice(ledger, Wyn, 12f, 30, 0.2f, "the pledged grain"));
            Assert.True(ledger.SignContract(Wyn, 40));
            ledger.TickDaily(41);
            ledger.TamperLedger();

            Assert.True(ReadTwice(ledger, Ivo, 5f, 10, 0.1f, "one day's labour"));

            var json = new SystemTextJsonSerializer();
            var restored = new LedgerDebtSystem();
            restored.RestoreState(json.Deserialize<LedgerDebtSystemState>(json.Serialize(ledger.CaptureState())));

            Assert.Equal(2, restored.Contracts.Count);
            Assert.True(restored.LedgerTampered);
            var wyn = restored.GetContract(Wyn);
            Assert.True(wyn.signed);
            Assert.Equal(29, wyn.daysRemaining);
            Assert.Equal("the pledged grain", wyn.forfeit);
            Assert.False(restored.GetContract(Ivo).signed, "draft stays a draft across saves");
        }

        [Fact]
        public void RestoreStateNullSafeAndIdempotent()
        {
            var ledger = Fixture();
            Assert.True(ReadTwice(ledger, Wyn, 12f, 30, 0.2f, "the pledged grain"));
            var saved = ledger.CaptureState();

            var restored = new LedgerDebtSystem();
            restored.RestoreState(saved);
            restored.RestoreState(saved);
            Assert.Single(restored.Contracts);

            var nullRestored = new LedgerDebtSystem();
            nullRestored.RestoreState(null);
            Assert.Empty(nullRestored.Contracts);
            Assert.True(nullRestored.PresentContract(Wyn, 1f, 10, 0f, "grain"),
                "still usable after null restore");
        }

        [Fact]
        public void StateChangedFiresOnMutations()
        {
            var ledger = Fixture();
            int changed = 0;
            ledger.OnStateChanged += _ => changed++;
            ledger.PresentContract(Wyn, 1f, 10, 0f, "grain");
            ledger.PresentContract(Wyn, 1f, 10, 0f, "grain");
            ledger.SignContract(Wyn, 5);
            ledger.TickDaily(6);
            ledger.TamperLedger();
            Assert.True(changed >= 5);
        }
    }

    public class LedgerDebtHeadlessDemoTests
    {
        [Fact]
        public void HeadlessDemoPasses()
        {
            var report = LedgerDebtHeadlessDemo.Run();
            Assert.True(report.Passed, report.Summary);
            Assert.Equal(0, report.FailedCount);
            Assert.True(report.Checks.Count >= 20);
        }
    }
}
