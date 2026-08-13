using NUnit.Framework;
using AtomicWar._Game.Core;
using AtomicWar._Game.Factions;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — Phase 4. LedgerDebtSystem (§5.3)
    /// plus Perrin Ashby and Ivo Fenn NPCs. Proves: sign/pay/renegotiate/
    /// forfeit/tamper/save-round-trip, NPC accumulators. Pure C#.
    /// </summary>
    [TestFixture]
    public class LedgerDebtTests
    {
        private static DebtContract MakeContract(string id = "contract_1",
            string debtor = "player", int termDays = 30,
            string principal = "5 sacks of grain", string forfeit = "the granary key")
        {
            return new DebtContract
            {
                contractId = id,
                debtorId = debtor,
                creditorId = "npc_dessa_vane",
                principal = principal,
                termDays = termDays,
                rate = 0f,
                forfeit = forfeit
            };
        }

        // ── LedgerDebtSystem ───────────────────────────────────────────

        [Test]
        public void FreshSystem_HasNoContracts()
        {
            var ledger = new LedgerDebtSystem();
            Assert.That(ledger.Contracts.Count, Is.Zero);
            Assert.That(ledger.GetOutstandingContracts().Count, Is.Zero);
        }

        [Test]
        public void SignContract_AddsToLedger_AndRaisesEvent()
        {
            var ledger = new LedgerDebtSystem();
            DebtContract signed = null;
            ledger.OnContractSigned += c => signed = c;

            var contract = MakeContract();
            Assert.That(ledger.SignContract(contract, 70), Is.True);
            Assert.That(ledger.Contracts.Count, Is.EqualTo(1));
            Assert.That(signed, Is.Not.Null);
            Assert.That(signed.contractId, Is.EqualTo("contract_1"));
            Assert.That(signed.daySigned, Is.EqualTo(70));
        }

        [Test]
        public void SignContract_DuplicateId_Rejected()
        {
            var ledger = new LedgerDebtSystem();
            Assert.That(ledger.SignContract(MakeContract(), 70), Is.True);
            Assert.That(ledger.SignContract(MakeContract(), 71), Is.False);
            Assert.That(ledger.Contracts.Count, Is.EqualTo(1));
        }

        [Test]
        public void SignContract_Null_Rejected()
        {
            var ledger = new LedgerDebtSystem();
            Assert.That(ledger.SignContract(null, 70), Is.False);
        }

        [Test]
        public void PayContract_MarksAsPaid_AndRaisesEvent()
        {
            var ledger = new LedgerDebtSystem();
            ledger.SignContract(MakeContract(), 70);

            DebtContract paid = null;
            ledger.OnContractPaid += c => paid = c;

            Assert.That(ledger.PayContract("contract_1"), Is.True);
            Assert.That(paid, Is.Not.Null);
            Assert.That(ledger.GetContract("contract_1").isPaid, Is.True);
        }

        [Test]
        public void PayContract_AlreadyPaid_IsNoOp()
        {
            var ledger = new LedgerDebtSystem();
            ledger.SignContract(MakeContract(), 70);
            Assert.That(ledger.PayContract("contract_1"), Is.True);
            Assert.That(ledger.PayContract("contract_1"), Is.False);
        }

        [Test]
        public void PayContract_Forfeited_IsNoOp()
        {
            var ledger = new LedgerDebtSystem();
            ledger.SignContract(MakeContract(termDays: 5), 70);
            ledger.Tick(76); // trigger forfeit
            Assert.That(ledger.PayContract("contract_1"), Is.False);
        }

        [Test]
        public void Tick_TriggersForfeit_AfterTermExpires()
        {
            var ledger = new LedgerDebtSystem();
            DebtContract forfeited = null;
            ledger.OnForfeitTriggered += c => forfeited = c;

            ledger.SignContract(MakeContract(termDays: 10), 70);
            ledger.Tick(79); // day 9 — not yet
            Assert.That(forfeited, Is.Null);

            ledger.Tick(80); // day 10 — forfeit
            Assert.That(forfeited, Is.Not.Null);
            Assert.That(forfeited.forfeitTriggered, Is.True);
            Assert.That(ledger.State.forfeitsTriggered, Is.EqualTo(1));
        }

        [Test]
        public void Tick_DoesNotDoubleForfeit()
        {
            var ledger = new LedgerDebtSystem();
            int count = 0;
            ledger.OnForfeitTriggered += _ => count++;

            ledger.SignContract(MakeContract(termDays: 5), 70);
            ledger.Tick(80);
            ledger.Tick(90);
            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void RenegotiateContract_UpdatesTerms()
        {
            var ledger = new LedgerDebtSystem();
            DebtContract renegotiated = null;
            ledger.OnContractRenegotiated += c => renegotiated = c;

            ledger.SignContract(MakeContract(termDays: 10), 70);
            Assert.That(ledger.RenegotiateContract("contract_1", 20, "3 sacks of grain"), Is.True);
            Assert.That(renegotiated, Is.Not.Null);

            var c = ledger.GetContract("contract_1");
            Assert.That(c.termDays, Is.EqualTo(20));
            Assert.That(c.principal, Is.EqualTo("3 sacks of grain"));
            Assert.That(c.renegotiated, Is.True);
        }

        [Test]
        public void AttemptTamper_AlwaysFails_AndRaisesEvent()
        {
            var ledger = new LedgerDebtSystem();
            string tamperedId = null;
            ledger.OnLedgerTampered += id => tamperedId = id;

            ledger.SignContract(MakeContract(), 70);
            Assert.That(ledger.AttemptTamper("contract_1"), Is.False, "Ivo's records do not lie");
            Assert.That(tamperedId, Is.EqualTo("contract_1"));
            Assert.That(ledger.State.ledgerTamperAttempts, Is.EqualTo(1));
        }

        [Test]
        public void GetContractsForDebtor_FiltersCorrectly()
        {
            var ledger = new LedgerDebtSystem();
            ledger.SignContract(MakeContract(id: "c1", debtor: "player"), 70);
            ledger.SignContract(MakeContract(id: "c2", debtor: "player"), 71);
            ledger.SignContract(MakeContract(id: "c3", debtor: "wyn_sabler"), 72);

            var playerContracts = ledger.GetContractsForDebtor("player");
            Assert.That(playerContracts.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetOutstandingContracts_ExcludesPaidAndForfeited()
        {
            var ledger = new LedgerDebtSystem();
            ledger.SignContract(MakeContract(id: "c1", termDays: 30), 70);
            ledger.SignContract(MakeContract(id: "c2", termDays: 5), 70);
            ledger.PayContract("c1");
            ledger.Tick(76); // forfeit c2

            Assert.That(ledger.GetOutstandingContracts().Count, Is.Zero);
        }

        [Test]
        public void SaveRoundTrip_PreservesContracts()
        {
            var ledger = new LedgerDebtSystem();
            ledger.SignContract(MakeContract(id: "c1", termDays: 30), 70);
            ledger.SignContract(MakeContract(id: "c2", termDays: 10), 71);
            ledger.PayContract("c1");
            ledger.AttemptTamper("c2");

            var captured = ledger.CaptureState();
            var restored = new LedgerDebtSystem();
            restored.RestoreState(captured);

            Assert.That(restored.Contracts.Count, Is.EqualTo(2));
            Assert.That(restored.GetContract("c1").isPaid, Is.True);
            Assert.That(restored.GetContract("c2").isPaid, Is.False);
            Assert.That(restored.State.ledgerTamperAttempts, Is.EqualTo(1));
            Assert.That(restored.State.contractsSigned, Is.EqualTo(2));
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
    }
}
