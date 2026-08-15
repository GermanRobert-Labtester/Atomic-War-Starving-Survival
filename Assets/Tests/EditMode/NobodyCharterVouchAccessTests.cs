using NUnit.Framework;
using AtomicWar._Game.Core;
using AtomicWar._Game.Factions;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — Phase 1 (Sprint 1). The social gate
    /// (VouchAccessSystem §5.2) plus the two opening companions.
    /// Proves: grant / burn / soften / last-resort, save round-trip,
    /// idempotent restore. Pure C# — no game data or scene required.
    /// </summary>
    [TestFixture]
    public class NobodyCharterVouchAccessTests
    {
        [Test]
        public void FreshSystem_GateClosed()
        {
            var vouch = new VouchAccessSystem();
            Assert.That(vouch.RequiresVouch, Is.True, "un-vouched visitor must face the gate");
            Assert.That(vouch.HasAccess, Is.False);
        }

        [Test]
        public void GrantVouch_OpensGate_AndRaisesEvent()
        {
            var vouch = new VouchAccessSystem();
            string grantedTo = null;
            vouch.OnVouchGranted += id => grantedTo = id;

            Assert.That(vouch.GrantVouch("npc_mattis_cray"), Is.True);
            Assert.That(vouch.HasAccess, Is.True);
            Assert.That(vouch.RequiresVouch, Is.False);
            Assert.That(vouch.VouchedBy, Is.EqualTo("npc_mattis_cray"));
            Assert.That(grantedTo, Is.EqualTo("npc_mattis_cray"), "OnVouchGranted must fire with the vouching NPC id");
        }

        [Test]
        public void GrantVouch_AgainWhenClean_IsIdempotent()
        {
            var vouch = new VouchAccessSystem();
            Assert.That(vouch.GrantVouch("npc_mattis_cray"), Is.True);
            int events = 0;
            vouch.OnVouchGranted += _ => events++;
            Assert.That(vouch.GrantVouch("npc_bram_ostrowski"), Is.False,
                "a second vouch while cleanly vouched is a no-op");
            Assert.That(vouch.VouchedBy, Is.EqualTo("npc_mattis_cray"), "original vouch stands");
            Assert.That(events, Is.Zero);
        }

        [Test]
        public void GrantVouch_NullOrEmpty_IsRejected()
        {
            var vouch = new VouchAccessSystem();
            Assert.That(vouch.GrantVouch(null), Is.False);
            Assert.That(vouch.GrantVouch(""), Is.False);
            Assert.That(vouch.HasAccess, Is.False);
        }

        [Test]
        public void BurnVouch_ReclosesGate_AndRaisesEvent()
        {
            var vouch = new VouchAccessSystem();
            vouch.GrantVouch("npc_mattis_cray");

            bool burned = false;
            vouch.OnVouchBurned += () => burned = true;

            Assert.That(vouch.BurnVouch(), Is.True);
            Assert.That(vouch.HasAccess, Is.False);
            Assert.That(vouch.RequiresVouch, Is.True);
            Assert.That(burned, Is.True, "OnVouchBurned must fire once");
        }

        [Test]
        public void BurnVouch_WhenNeverVouched_IsNoOp()
        {
            var vouch = new VouchAccessSystem();
            int events = 0;
            vouch.OnVouchBurned += () => events++;
            Assert.That(vouch.BurnVouch(), Is.False, "nothing to burn when the gate was never opened");
            Assert.That(vouch.HasAccess, Is.False);
            Assert.That(events, Is.Zero);
        }

        [Test]
        public void AfterBurn_NewVouch_RestoresAccess_AndLastResortFlagged()
        {
            var vouch = new VouchAccessSystem();
            vouch.GrantVouch("npc_osran_kell", isLastResort: false);
            vouch.BurnVouch();

            // The pack must never hard-lock: a fresh vouch re-opens the gate.
            Assert.That(vouch.GrantVouch("npc_mattis_cray", isLastResort: true), Is.True);
            Assert.That(vouch.HasAccess, Is.True);
            Assert.That(vouch.LastResortUsed, Is.True, "the paid-for last resort has been consumed");
        }

        [Test]
        public void SoftenAccess_RequiresAName_AndIsIdempotent()
        {
            var vouch = new VouchAccessSystem();
            Assert.That(vouch.SoftenAccess(), Is.False, "a never-vouched visitor cannot soften the gate");
            Assert.That(vouch.RequiresVouch, Is.True, "gate stays closed without a name");

            vouch.GrantVouch("npc_osran_kell");
            Assert.That(vouch.SoftenAccess(), Is.True);
            Assert.That(vouch.HasAccess, Is.True);
            Assert.That(vouch.RequiresVouch, Is.False);
            Assert.That(vouch.SoftenAccess(), Is.True, "idempotent: already softened");

            // Softened access cannot be burned away.
            Assert.That(vouch.BurnVouch(), Is.False);
            Assert.That(vouch.HasAccess, Is.True);
        }

        [Test]
        public void NeedsLastResort_OnlyAfterABurnedFirstVouch()
        {
            var vouch = new VouchAccessSystem();
            Assert.That(vouch.NeedsLastResort, Is.False, "fresh gate: the first vouch is still available");
            vouch.GrantVouch("npc_osran_kell");
            Assert.That(vouch.NeedsLastResort, Is.False, "gate open");

            vouch.BurnVouch();
            Assert.That(vouch.NeedsLastResort, Is.True, "burned first vouch, last resort uncashed");

            vouch.GrantVouch("npc_mattis_cray", isLastResort: true);
            Assert.That(vouch.NeedsLastResort, Is.False, "last resort cashed");
        }

        [Test]
        public void SaveRoundTrip_PreservesVouchState()
        {
            var vouch = new VouchAccessSystem();
            vouch.GrantVouch("npc_mattis_cray", isLastResort: true);

            var captured = (VouchAccessSystemState)vouch.CaptureState();

            var restored = new VouchAccessSystem();
            restored.RestoreState(captured);
            Assert.That(restored.HasAccess, Is.True);
            Assert.That(restored.VouchedBy, Is.EqualTo("npc_mattis_cray"));
            Assert.That(restored.LastResortUsed, Is.True);
        }

        [Test]
        public void RestoreState_IsIdempotent_AndNullSafe()
        {
            var vouch = new VouchAccessSystem();
            vouch.GrantVouch("npc_osran_kell");
            var captured = (VouchAccessSystemState)vouch.CaptureState();

            var restored = new VouchAccessSystem();
            restored.RestoreState(captured);
            restored.RestoreState(captured); // twice is safe
            Assert.That(restored.HasAccess, Is.True);
            Assert.That(restored.VouchedBy, Is.EqualTo("npc_osran_kell"));

            // Null restore is a no-op (legacy saves without this system).
            var nullRestored = new VouchAccessSystem();
            nullRestored.RestoreState(null);
            Assert.That(nullRestored.HasAccess, Is.False);
        }

        // ── Opening companions ───────────────────────────────────────────

        [Test]
        public void Osran_Weigh_Accumulates_AndRaisesEvent()
        {
            var osran = new NPC_OsranKell();
            osran.Initialise("Osran Kell");
            int last = 0;
            osran.OnWeighPerformed += (_, count) => last = count;

            Assert.That(osran.PerformWeigh(), Is.EqualTo(1));
            Assert.That(osran.PerformWeigh(), Is.EqualTo(2));
            Assert.That(last, Is.EqualTo(2));
            Assert.That(osran.State.weighsPerformed, Is.EqualTo(2));
        }

        [Test]
        public void Osran_RefusesBribe_OnlyFirstTime()
        {
            var osran = new NPC_OsranKell();
            osran.Initialise("Osran Kell");
            Assert.That(osran.AttemptBribe(), Is.True);
            Assert.That(osran.AttemptBribe(), Is.False, "only one on-the-record refusal");
            Assert.That(osran.State.refusedBribe, Is.True);
            Assert.That(osran.State.bribeAttempted, Is.True);
        }

        [Test]
        public void Mattis_WillNotVouchTwice_ForTheBurned()
        {
            var mattis = new NPC_MattisCray();
            mattis.Initialise("Mattis Cray");
            Assert.That(mattis.WillVouch, Is.True);
            Assert.That(mattis.GiveVouch(), Is.True);

            mattis.BurnMattis();
            Assert.That(mattis.WillVouch, Is.False, "he does not offer his name a second time");
            Assert.That(mattis.GiveVouch(), Is.False);
        }

        [Test]
        public void CompanionSaveRoundTrips()
        {
            var osran = new NPC_OsranKell();
            osran.Initialise("Osran Kell");
            osran.PerformWeigh();

            var osran2 = new NPC_OsranKell();
            osran2.RestoreState((NPC_OsranKellState)osran.CaptureState());
            Assert.That(osran2.State.weighsPerformed, Is.EqualTo(1));

            var mattis = new NPC_MattisCray();
            mattis.Initialise("Mattis Cray");
            mattis.GiveVouch();

            var mattis2 = new NPC_MattisCray();
            mattis2.RestoreState((NPC_MattisCrayState)mattis.CaptureState());
            Assert.That(mattis2.State.vouchesGiven, Is.EqualTo(1));
        }
    }
}