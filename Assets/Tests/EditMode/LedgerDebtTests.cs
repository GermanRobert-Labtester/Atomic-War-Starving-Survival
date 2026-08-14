using NUnit.Framework;
using Ashfall.Core;
using AtomicWar._Game.Factions;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — Phase 4. The single engine-agnostic
    /// LedgerDebtSystem (§5.3, Ashfall.Core — the Unity host consumes it
    /// directly, no host twin) plus the Perrin, Ivo and Wyn NPCs.
    /// Proves: read-twice signing, ink-freeze, term-end renegotiation,
    /// named forfeit, pay (before and after due), one-shot tamper, save
    /// round-trip, and the NPC accumulators. Pure C#.
    /// </summary>
    [TestFixture]
    public class LedgerDebtTests
    {
        private const string Wyn = "npc_wyn_sabler";

        private static bool ReadTwice(LedgerDebtSystem ledger, string debtor,
            float principal, int termDays, float rate, string forfeit)
            => ledger.PresentContract(debtor, principal, termDays, rate, forfeit)
                && ledger.PresentContract(debtor, principal, termDays, rate, forfeit);

        // ── LedgerDebtSystem (Ashfall.Core, §5.3) ───────────────────────

        [Test]
        public void FreshSystem_HasNoContracts()
        {
            var ledger = new LedgerDebtSystem();
            Assert.That(ledger.Contracts.Count, Is.Zero);
        }

        [Test]
        public void SigningRequiresTwoReadings()
        {
            var ledger = new LedgerDebtSystem();
            Assert.That(ledger.PresentContract(Wyn, 12f, 30, 0.2f, "the pledged grain"), Is.True);
            Assert.That(ledger.SignContract(Wyn, 40), Is.False, "one reading is not ink");
            Assert.That(ledger.PresentContract(Wyn, 12f, 30, 0.2f, "the pledged grain"), Is.True);
            Assert.That(ledger.SignContract(Wyn, 40), Is.True, "the second reading closes it");
        }

        [Test]
        public void SigningRaisesEvent_AndFreezesTerms()
        {
            var ledger = new LedgerDebtSystem();
            DebtContract signed = null;
            ledger.OnContractSigned += c => signed = c;

            Assert.That(ReadTwice(ledger, Wyn, 12f, 30, 0.2f, "the pledged grain"), Is.True);
            Assert.That(ledger.SignContract(Wyn, 40), Is.True);
            Assert.That(signed, Is.Not.Null);
            Assert.That(signed.debtorId, Is.EqualTo(Wyn));
            Assert.That(ledger.GetContract(Wyn).daysRemaining, Is.EqualTo(30));

            // Ink is ink — no new draft over an open debt.
            Assert.That(ledger.PresentContract(Wyn, 999f, 1, 0.9f, "anything"), Is.False);
            Assert.That(ledger.GetContract(Wyn).principal, Is.EqualTo(12f));
        }

        [Test]
        public void TermExpiryTriggersNamedForfeit()
        {
            var ledger = new LedgerDebtSystem();
            Assert.That(ReadTwice(ledger, Wyn, 10f, 3, 0.1f, "the pledged grain"), Is.True);
            Assert.That(ledger.SignContract(Wyn, 10), Is.True);

            DebtContract forfeited = null;
            ledger.OnForfeitTriggered += c => forfeited = c;

            ledger.TickDaily(11);
            ledger.TickDaily(12);
            Assert.That(ledger.GetContract(Wyn).forfeited, Is.False);
            ledger.TickDaily(13);
            Assert.That(ledger.GetContract(Wyn).forfeited, Is.True);
            Assert.That(forfeited, Is.Not.Null);
            Assert.That(forfeited.forfeit, Is.EqualTo("the pledged grain"));
        }

        [Test]
        public void PayContract_MarksPaid_AndHonoursAfterDue()
        {
            var ledger = new LedgerDebtSystem();
            Assert.That(ReadTwice(ledger, Wyn, 10f, 2, 0.1f, "the pledged grain"), Is.True);
            Assert.That(ledger.SignContract(Wyn, 10), Is.True);
            ledger.TickDaily(11);
            ledger.TickDaily(12); // forfeit due
            Assert.That(ledger.GetContract(Wyn).forfeited, Is.True);

            DebtContract paid = null;
            ledger.OnContractPaid += c => paid = c;
            Assert.That(ledger.PayContract(Wyn, 13), Is.True,
                "paying the named good back is the honoured path");
            Assert.That(paid, Is.Not.Null);
            Assert.That(ledger.GetContract(Wyn).paid, Is.True);
            Assert.That(ledger.GetContract(Wyn).forfeited, Is.False);
        }

        [Test]
        public void TermEndRenegotiationExtendsSignedInk()
        {
            var ledger = new LedgerDebtSystem();
            Assert.That(ReadTwice(ledger, Wyn, 12f, 3, 0.2f, "the pledged grain"), Is.True);
            Assert.That(ledger.SignContract(Wyn, 10), Is.True);

            Assert.That(ledger.RenegotiateContract(Wyn, 10f, 10, 0.1f, "the pledged grain"), Is.False,
                "no silent amendment mid-term");

            ledger.TickDaily(11);
            ledger.TickDaily(12); // last day of the term
            Assert.That(ledger.RenegotiateContract(Wyn, 10f, 10, 0.1f, "the pledged grain"), Is.True);

            var c = ledger.GetContract(Wyn);
            Assert.That(c.signed, Is.True);
            Assert.That(c.termDays, Is.EqualTo(10));
            Assert.That(c.daysRemaining, Is.EqualTo(10));
            Assert.That(c.forfeit, Is.EqualTo("the pledged grain"), "forfeit stays named up front");
        }

        [Test]
        public void TamperIsOneShot()
        {
            var ledger = new LedgerDebtSystem();
            int tampered = 0;
            ledger.OnLedgerTampered += () => tampered++;

            Assert.That(ledger.TamperLedger(), Is.True);
            Assert.That(ledger.TamperLedger(), Is.False, "one strike per playthrough");
            Assert.That(tampered, Is.EqualTo(1));
            Assert.That(ledger.LedgerTampered, Is.True);
        }

        [Test]
        public void TotalOwedIsFlatRate()
        {
            var ledger = new LedgerDebtSystem();
            Assert.That(ledger.TotalOwed(Wyn), Is.EqualTo(0f));

            Assert.That(ReadTwice(ledger, Wyn, 100f, 30, 0.25f, "the pledged grain"), Is.True);
            Assert.That(ledger.SignContract(Wyn, 10), Is.True);
            Assert.That(ledger.TotalOwed(Wyn), Is.EqualTo(125f).Within(0.001f));
        }

        [Test]
        public void SaveRoundTrip_PreservesContractsAndTamper()
        {
            var ledger = new LedgerDebtSystem();
            Assert.That(ReadTwice(ledger, Wyn, 12f, 30, 0.2f, "the pledged grain"), Is.True);
            Assert.That(ledger.SignContract(Wyn, 40), Is.True);
            ledger.TickDaily(41);
            ledger.TamperLedger();

            var captured = ledger.CaptureState();
            var restored = new LedgerDebtSystem();
            restored.RestoreState(captured);

            Assert.That(restored.Contracts.Count, Is.EqualTo(1));
            Assert.That(restored.LedgerTampered, Is.True);
            Assert.That(restored.GetContract(Wyn).signed, Is.True);
            Assert.That(restored.GetContract(Wyn).daysRemaining, Is.EqualTo(29));
            Assert.That(restored.GetContract(Wyn).forfeit, Is.EqualTo("the pledged grain"));
        }

        [Test]
        public void RestoreState_NullSafe()
        {
            var ledger = new LedgerDebtSystem();
            ledger.RestoreState(null);
            Assert.That(ledger.Contracts.Count, Is.Zero);
        }

        // ── NPC_PerrinAshby ────────────────────────────────────────────

        [Test]
        public void Perrin_WriteDraft_Accumulates()
        {
            var perrin = new NPC_PerrinAshby();
            perrin.Initialise("Perrin Ashby");
            int last = 0;
            perrin.OnDraftWritten += (_, count) => last = count;

            Assert.That(perrin.WriteDraft(), Is.EqualTo(1));
            Assert.That(perrin.WriteDraft(), Is.EqualTo(2));
            Assert.That(last, Is.EqualTo(2));
        }

        [Test]
        public void Perrin_CollectSignature_Accumulates()
        {
            var perrin = new NPC_PerrinAshby();
            perrin.Initialise("Perrin Ashby");
            Assert.That(perrin.CollectSignature(), Is.EqualTo(1));
            Assert.That(perrin.State.signaturesCollected, Is.EqualTo(1));
        }

        [Test]
        public void Perrin_MarkUnfairClause_IsPermanent()
        {
            var perrin = new NPC_PerrinAshby();
            perrin.Initialise("Perrin Ashby");
            Assert.That(perrin.State.passedUnfairClause, Is.False);
            perrin.MarkUnfairClause();
            Assert.That(perrin.State.passedUnfairClause, Is.True);
        }

        [Test]
        public void Perrin_SaveRoundTrip()
        {
            var perrin = new NPC_PerrinAshby();
            perrin.Initialise("Perrin Ashby");
            perrin.WriteDraft();
            perrin.CollectSignature();

            var perrin2 = new NPC_PerrinAshby();
            perrin2.RestoreState((NPC_PerrinAshbyState)perrin.CaptureState());
            Assert.That(perrin2.State.draftsWritten, Is.EqualTo(1));
            Assert.That(perrin2.State.signaturesCollected, Is.EqualTo(1));
        }

        // ── NPC_IvoFenn ────────────────────────────────────────────────

        [Test]
        public void Ivo_FileRecord_Accumulates()
        {
            var ivo = new NPC_IvoFenn();
            ivo.Initialise("Ivo Fenn");
            int last = 0;
            ivo.OnRecordFiled += (_, count) => last = count;

            Assert.That(ivo.FileRecord(), Is.EqualTo(1));
            Assert.That(ivo.FileRecord(), Is.EqualTo(2));
            Assert.That(last, Is.EqualTo(2));
        }

        [Test]
        public void Ivo_RefuseCharterSummary_OnlyOnce()
        {
            var ivo = new NPC_IvoFenn();
            ivo.Initialise("Ivo Fenn");
            Assert.That(ivo.RefuseCharterSummary(), Is.True);
            Assert.That(ivo.RefuseCharterSummary(), Is.False);
            Assert.That(ivo.State.refusedCharterSummary, Is.True);
        }

        [Test]
        public void Ivo_ProduceCharter_OnlyOnce()
        {
            var ivo = new NPC_IvoFenn();
            ivo.Initialise("Ivo Fenn");
            bool produced = false;
            ivo.OnCharterProduced += _ => produced = true;

            Assert.That(ivo.ProduceCharter(), Is.True);
            Assert.That(produced, Is.True);
            Assert.That(ivo.ProduceCharter(), Is.False);
            Assert.That(ivo.State.charterProduced, Is.True);
        }

        [Test]
        public void Ivo_SaveRoundTrip()
        {
            var ivo = new NPC_IvoFenn();
            ivo.Initialise("Ivo Fenn");
            ivo.FileRecord();
            ivo.RefuseCharterSummary();
            ivo.ProduceCharter();

            var ivo2 = new NPC_IvoFenn();
            ivo2.RestoreState((NPC_IvoFennState)ivo.CaptureState());
            Assert.That(ivo2.State.recordsFiled, Is.EqualTo(1));
            Assert.That(ivo2.State.refusedCharterSummary, Is.True);
            Assert.That(ivo2.State.charterProduced, Is.True);
        }

        // ── NPC_WynSabler ──────────────────────────────────────────────

        [Test]
        public void Wyn_ReciteTerms_Accumulates()
        {
            var wyn = new NPC_WynSabler();
            wyn.Initialise("Wyn Sabler");
            int last = 0;
            wyn.OnTermsRecited += (_, count) => last = count;

            Assert.That(wyn.ReciteTerms(), Is.EqualTo(1));
            Assert.That(wyn.ReciteTerms(), Is.EqualTo(2));
            Assert.That(last, Is.EqualTo(2));
        }

        [Test]
        public void Wyn_FleeWithGrain_OnlyOnce()
        {
            var wyn = new NPC_WynSabler();
            wyn.Initialise("Wyn Sabler");
            bool fled = false;
            wyn.OnFledWithGrain += _ => fled = true;

            Assert.That(wyn.FleeWithGrain(), Is.True);
            Assert.That(fled, Is.True);
            Assert.That(wyn.FleeWithGrain(), Is.False);
            Assert.That(wyn.State.fledWithGrain, Is.True);
        }

        [Test]
        public void Wyn_HonourDebt_OnlyOnce()
        {
            var wyn = new NPC_WynSabler();
            wyn.Initialise("Wyn Sabler");
            bool honoured = false;
            wyn.OnDebtHonoured += _ => honoured = true;

            Assert.That(wyn.HonourDebt(), Is.True);
            Assert.That(honoured, Is.True);
            Assert.That(wyn.HonourDebt(), Is.False);
            Assert.That(wyn.State.debtHonoured, Is.True);
        }

        [Test]
        public void Wyn_SaveRoundTrip()
        {
            var wyn = new NPC_WynSabler();
            wyn.Initialise("Wyn Sabler");
            wyn.ReciteTerms();
            wyn.HonourDebt();

            var wyn2 = new NPC_WynSabler();
            wyn2.RestoreState((NPC_WynSablerState)wyn.CaptureState());
            Assert.That(wyn2.State.termsRecited, Is.EqualTo(1));
            Assert.That(wyn2.State.debtHonoured, Is.True);
            Assert.That(wyn2.State.fledWithGrain, Is.False);
        }
    }
}

