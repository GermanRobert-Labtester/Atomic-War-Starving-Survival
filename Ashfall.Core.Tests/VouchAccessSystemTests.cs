using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Cross-tool review of the VouchAccessSystem extraction (QA register:
    /// vouch × backers × debt). Ports the Unity NobodyCharterVouchAccessTests
    /// scenarios plus port-based JSON roundtrip. NPC ids are master-list ids
    /// from characters.json.
    /// </summary>
    public class VouchAccessSystemTests
    {
        [Fact]
        public void FreshSystem_GateClosed()
        {
            var vouch = new VouchAccessSystem();
            Assert.True(vouch.RequiresVouch, "un-vouched visitor must face the gate");
            Assert.False(vouch.HasAccess);
        }

        [Fact]
        public void GrantVouch_OpensGate_AndRaisesEvent()
        {
            var vouch = new VouchAccessSystem();
            string grantedTo = null;
            vouch.OnVouchGranted += id => grantedTo = id;

            Assert.True(vouch.GrantVouch("npc_mattis_cray"));
            Assert.True(vouch.HasAccess);
            Assert.False(vouch.RequiresVouch);
            Assert.Equal("npc_mattis_cray", vouch.VouchedBy);
            Assert.Equal("npc_mattis_cray", grantedTo);
        }

        [Fact]
        public void GrantVouch_AgainWhenClean_IsIdempotent()
        {
            var vouch = new VouchAccessSystem();
            Assert.True(vouch.GrantVouch("npc_mattis_cray"));
            int events = 0;
            vouch.OnVouchGranted += _ => events++;
            Assert.False(vouch.GrantVouch("npc_bram_ostrowski"),
                "a second vouch while cleanly vouched is a no-op");
            Assert.Equal("npc_mattis_cray", vouch.VouchedBy);
            Assert.Equal(0, events);
        }

        [Fact]
        public void GrantVouch_NullOrEmpty_IsRejected()
        {
            var vouch = new VouchAccessSystem();
            Assert.False(vouch.GrantVouch(null));
            Assert.False(vouch.GrantVouch(""));
            Assert.False(vouch.HasAccess);
        }

        [Fact]
        public void BurnVouch_ReclosesGate_AndRaisesEvent()
        {
            var vouch = new VouchAccessSystem();
            vouch.GrantVouch("npc_mattis_cray");

            bool burned = false;
            vouch.OnVouchBurned += () => burned = true;

            Assert.True(vouch.BurnVouch());
            Assert.False(vouch.HasAccess);
            Assert.True(vouch.RequiresVouch);
            Assert.True(burned);
        }

        [Fact]
        public void BurnVouch_WhenNeverVouched_IsNoOp()
        {
            var vouch = new VouchAccessSystem();
            int events = 0;
            vouch.OnVouchBurned += () => events++;
            Assert.False(vouch.BurnVouch());
            Assert.False(vouch.HasAccess);
            Assert.Equal(0, events);
        }

        [Fact]
        public void AfterBurn_NewVouch_RestoresAccess_AndLastResortFlagged()
        {
            var vouch = new VouchAccessSystem();
            vouch.GrantVouch("npc_osran_kell", isLastResort: false);
            vouch.BurnVouch();

            // The pack must never hard-lock: a fresh vouch re-opens the gate.
            Assert.True(vouch.GrantVouch("npc_mattis_cray", isLastResort: true));
            Assert.True(vouch.HasAccess);
            Assert.True(vouch.LastResortUsed, "the paid-for last resort has been consumed");
        }

        [Fact]
        public void SoftenAccess_RequiresAName_AndIsIdempotent()
        {
            var vouch = new VouchAccessSystem();
            Assert.False(vouch.SoftenAccess(), "a never-vouched visitor cannot soften the gate");
            Assert.True(vouch.RequiresVouch, "gate stays closed without a name");

            vouch.GrantVouch("npc_osran_kell");
            Assert.True(vouch.SoftenAccess());
            Assert.True(vouch.HasAccess);
            Assert.False(vouch.RequiresVouch);
            Assert.True(vouch.SoftenAccess(), "idempotent: already softened");

            // Softened access cannot be burned away.
            Assert.False(vouch.BurnVouch());
            Assert.True(vouch.HasAccess);
        }

        [Fact]
        public void SoftenAccess_AfterABurnedVouch_IsAllowed()
        {
            var vouch = new VouchAccessSystem();
            vouch.GrantVouch("npc_osran_kell");
            vouch.BurnVouch();

            // The burned name still counts as a name on the ledger: the
            // player cannot skip the name, but the burned path is not a dead end.
            Assert.True(vouch.SoftenAccess());
            Assert.True(vouch.HasAccess);
        }

        [Fact]
        public void NeedsLastResort_OnlyAfterABurnedFirstVouch()
        {
            var vouch = new VouchAccessSystem();
            Assert.False(vouch.NeedsLastResort, "fresh gate: the first vouch is still available");
            vouch.GrantVouch("npc_osran_kell");
            Assert.False(vouch.NeedsLastResort, "gate open");

            vouch.BurnVouch();
            Assert.True(vouch.NeedsLastResort, "burned first vouch, last resort uncashed");

            vouch.GrantVouch("npc_mattis_cray", isLastResort: true);
            Assert.False(vouch.NeedsLastResort, "last resort cashed");
        }

        [Fact]
        public void SaveRoundTrip_PreservesVouchState()
        {
            var vouch = new VouchAccessSystem();
            vouch.GrantVouch("npc_mattis_cray", isLastResort: true);

            var json = new SystemTextJsonSerializer();
            var restored = new VouchAccessSystem();
            restored.RestoreState(json.Deserialize<VouchAccessSystemState>(json.Serialize(vouch.CaptureState())));
            Assert.True(restored.HasAccess);
            Assert.Equal("npc_mattis_cray", restored.VouchedBy);
            Assert.True(restored.LastResortUsed);
        }

        [Fact]
        public void RestoreState_IsIdempotent_AndNullSafe()
        {
            var vouch = new VouchAccessSystem();
            vouch.GrantVouch("npc_osran_kell");
            var captured = vouch.CaptureState();

            var restored = new VouchAccessSystem();
            restored.RestoreState(captured);
            restored.RestoreState(captured); // twice is safe
            Assert.True(restored.HasAccess);
            Assert.Equal("npc_osran_kell", restored.VouchedBy);

            // Null restore is a no-op (legacy saves without this system).
            var nullRestored = new VouchAccessSystem();
            nullRestored.RestoreState(null);
            Assert.False(nullRestored.HasAccess);
        }

        [Fact]
        public void StateChangedFiresOnGrantBurnSoften()
        {
            var vouch = new VouchAccessSystem();
            int changed = 0;
            vouch.OnStateChanged += _ => changed++;
            vouch.GrantVouch("npc_mattis_cray");
            vouch.BurnVouch();
            vouch.SoftenAccess();
            vouch.SoftenAccess(); // idempotent: no extra event
            Assert.Equal(3, changed);
        }
    }
}
