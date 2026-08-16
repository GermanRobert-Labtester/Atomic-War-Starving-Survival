using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Muster;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Tests the six Section V current state machines + Hydro-Barons (Expansion 06).
    /// Following the existing coalition/camp test shape: construct, mutate,
    /// CaptureState()/RestoreState() round-trip, assert.
    /// </summary>
    public class MusterCurrentSystemsTests
    {
        // ── 1. Cold Count (Section V.1) — SupplyPower/DeliverShielding/Provenance ──

        [Fact]
        public void ColdCount_ProvenanceRequiresPowerAndShielding()
        {
            var sys = new ColdCountSystem();
            Assert.False(sys.CanCompleteProvenanceRun());
            Assert.False(sys.CompleteProvenanceRun());

            sys.SupplyPower(30);
            Assert.False(sys.ProvenanceDataComplete);     // power alone is not enough
            sys.DeliverShielding(4);
            Assert.True(sys.ProvenanceDataComplete);     // both thresholds met
        }

        [Fact]
        public void ColdCount_PartialSupplyCannotBroadcastFullCredibility()
        {
            var sys = new ColdCountSystem();
            sys.SupplyPower(12);                 // below require (30)
            Assert.False(sys.ProvenanceDataComplete);
            sys.DeliverShielding(4);
            Assert.False(sys.ProvenanceDataComplete); // power still short

            sys.TransmitFindings(250);
            Assert.True(sys.BroadcastSent);
            Assert.True(sys.BroadcastIsCaveated); // partial set → caveated broadcast
        }

        [Fact]
        public void ColdCount_CompleteRunBroadcastsUncaveated()
        {
            var sys = new ColdCountSystem();
            sys.SupplyPower(30);
            sys.DeliverShielding(4);
            Assert.True(sys.ProvenanceDataComplete);

            sys.TransmitFindings(300);
            Assert.False(sys.BroadcastIsCaveated);
            Assert.False(sys.TransmitFindings(301)); // fires once
        }

        [Fact]
        public void ColdCount_SaveLoadRoundTrips()
        {
            var sys = new ColdCountSystem();
            sys.SupplyPower(30);
            sys.DeliverShielding(4);
            sys.TransmitFindings(290);

            var restored = new ColdCountSystem();
            restored.RestoreState(sys.CaptureState());
            Assert.Equal(30, restored.PowerSuppliedDays);
            Assert.Equal(4, restored.ShieldingDelivered);
            Assert.True(restored.ProvenanceDataComplete);
            Assert.True(restored.BroadcastSent);
            Assert.Equal(290, restored.State.broadcastDay);
            Assert.Equal(SaveChecksum.Compute(sys.CaptureState()),
                SaveChecksum.Compute(restored.CaptureState()));
        }

        // ── 2. Provisioned (Section V.3) — RecordUnprompted-only, no fork ──

        [Fact]
        public void Provisioned_ContactOnlyViaUnpromptedHelp()
        {
            var sys = new ProvisionedSystem();
            sys.RecordUnprompted(13);
            Assert.True(sys.HaveMadeContact);
            Assert.True(sys.RespectScore >= ProvisionedState.ContactThreshold);
        }

        [Fact]
        public void Provisioned_NoTradeOfferedUntilContact()
        {
            var sys = new ProvisionedSystem();
            Assert.False(sys.UnlockCache("item_prewar_diagnostic_scanner"));
            sys.RecordUnprompted(4);
            Assert.False(sys.HaveMadeContact);
            Assert.False(sys.UnlockCache("item_prewar_diagnostic_scanner"));
            sys.RecordUnprompted(20);
            Assert.True(sys.HaveMadeContact);
            Assert.True(sys.UnlockCache("item_prewar_diagnostic_scanner"));
            Assert.False(sys.UnlockCache("item_prewar_diagnostic_scanner")); // once only
        }

        [Fact]
        public void Provisioned_SaveLoadRoundTrips()
        {
            var sys = new ProvisionedSystem();
            sys.RecordUnprompted(14);
            sys.UnlockCache("item_prewar_diagnostic_scanner");

            var restored = new ProvisionedSystem();
            restored.RestoreState(sys.CaptureState());
            Assert.True(restored.HaveMadeContact);
            Assert.Equal(14, restored.RespectScore);
            Assert.True(restored.HasTrade("item_prewar_diagnostic_scanner"));
            Assert.Equal(SaveChecksum.Compute(sys.CaptureState()),
                SaveChecksum.Compute(restored.CaptureState()));
        }

        // ── 3. Long Walk (Section V.4) — circuit advance + Approach A/B ──

        [Fact]
        public void LongWalk_CircuitAdvancesOnDeparture()
        {
            var sys = new LongWalkSystem();
            string start = sys.CurrentRegion;
            sys.DailyTick(0);                            // first tick (departure cue) already in flight
            Assert.Equal(1, sys.CrossingsCompleted);
            Assert.NotEqual(start, sys.CurrentRegion);
            Assert.Equal(LongWalkState.RegionCycleDays, sys.DaysUntilDeparture);
        }

        [Fact]
        public void LongWalk_EscortImprovesFledgingIntelligence()
        {
            var sys = new LongWalkSystem();
            sys.RecordEscort();
            sys.DailyTick(0); // one crossing, escorted leg
            var report = sys.RequestSituationReport();
            Assert.True(report.ContainsKey("faction_long_walk"));
            Assert.True(report["faction_long_walk"] > 30f); // escorted intel is fresher
        }

        [Fact]
        public void LongWalk_SaveLoadRoundTrips()
        {
            var sys = new LongWalkSystem();
            sys.RecordEscort();
            sys.RecordResupply();
            sys.DailyTick(0);                            // one crossing

            var restored = new LongWalkSystem();
            restored.RestoreState(sys.CaptureState());
            Assert.Equal(1, restored.CrossingsCompleted);
            Assert.Equal(1, restored.State.escortCount);
            Assert.Equal(1, restored.State.resupplyCount);
            Assert.Equal(sys.CurrentRegion, restored.CurrentRegion);
            Assert.Equal(SaveChecksum.Compute(sys.CaptureState()),
                SaveChecksum.Compute(restored.CaptureState()));
        }

        // ── 4. Scavenger Guild (Section V.5) — blacklist is permanent ──

        [Fact]
        public void ScavengerGuild_OverstripBlacklistsPermanently()
        {
            var sys = new ScavengerGuildSystem();
            sys.ClaimSite("loc_muster_treeline_camp");
            Assert.True(sys.RecordOverStrip("shelter_player", "loc_muster_treeline_camp"));
            Assert.True(sys.IsBlacklisted("shelter_player"));
            // no removal method — the ledger never crosses a name out
            Assert.False(sys.ClaimSite("loc_muster_treeline_camp")); // already claimed
        }

        [Fact]
        public void ScavengerGuild_UnclaimedSiteCannotBeOverStripped()
        {
            var sys = new ScavengerGuildSystem();
            Assert.False(sys.RecordOverStrip("shelter_player", "loc_iron_raiders_den"));
            Assert.False(sys.IsBlacklisted("shelter_player"));
        }

        [Fact]
        public void ScavengerGuild_ClaimTracksSite()
        {
            var sys = new ScavengerGuildSystem();
            Assert.True(sys.ClaimSite("loc_scavenger_guildhall"));
            Assert.True(sys.IsClaimed("loc_scavenger_guildhall"));
            Assert.False(sys.ClaimSite("loc_scavenger_guildhall")); // once only
        }

        [Fact]
        public void ScavengerGuild_SaveLoadRoundTrips()
        {
            var sys = new ScavengerGuildSystem();
            sys.ClaimSite("loc_muster_treeline_camp");
            sys.RecordOverStrip("shelter_player", "loc_muster_treeline_camp");

            var restored = new ScavengerGuildSystem();
            restored.RestoreState(sys.CaptureState());
            Assert.True(restored.IsClaimed("loc_muster_treeline_camp"));
            Assert.True(restored.IsBlacklisted("shelter_player"));
            Assert.Equal(SaveChecksum.Compute(sys.CaptureState()),
                SaveChecksum.Compute(restored.CaptureState()));
        }

        // ── 5. Iron Raiders (Section V.6) — raid chance reads aggression+visibility ──

        [Fact]
        public void IronRaiders_FortifyLowersRaidWindow()
        {
            var sys = new IronRaidersSystem();
            sys.SetAggressionLevel(0.8f);
            float before = sys.EvaluateRaidChance();
            sys.FortifyApproachRoutes(60f);
            float after = sys.EvaluateRaidChance();
            Assert.True(after < before);
            Assert.True(before > 0f && before <= 1f);
            Assert.True(after >= 0f && after <= 1f);
        }

        [Fact]
        public void IronRaiders_RaidIsCombatOnly()
        {
            var sys = new IronRaidersSystem();
            sys.ExecuteRaid();
            Assert.Equal(1, sys.RaidsThisSeason);
            sys.ProvokeRaid();
            Assert.Equal(2, sys.RaidsThisSeason);
        }

        [Fact]
        public void IronRaiders_SaveLoadRoundTrips()
        {
            var sys = new IronRaidersSystem();
            sys.SetAggressionLevel(0.75f);
            sys.FortifyApproachRoutes(40f);
            sys.ExecuteRaid();

            var restored = new IronRaidersSystem();
            restored.RestoreState(sys.CaptureState());
            Assert.Equal(1, restored.RaidsThisSeason);
            Assert.True(restored.State.shelterVisibility < 1f);
            Assert.True(System.Math.Abs(restored.AggressionLevel - 0.75f) < 0.001f);
            Assert.Equal(SaveChecksum.Compute(sys.CaptureState()),
                SaveChecksum.Compute(restored.CaptureState()));
        }

        // ── 6. Hydro-Barons (Section II) — four Approach outcomes ──

        [Fact]
        public void HydroBarons_ApproachCSeizesPlant()
        {
            var sys = new HydroBaronsSystem();
            sys.AdvanceQueue(12);
            sys.ResolveApproach(QuestApproach.C);
            Assert.True(sys.PlantSeized);
            Assert.True(sys.QueueChitIsLiveCurrency);
            Assert.Equal(0, sys.QueuePosition);       // queue destroyed
            Assert.Equal("C", sys.ChosenApproach);
        }

        [Fact]
        public void HydroBarons_ApproachAOrDFixesCard()
        {
            var sys = new HydroBaronsSystem();
            sys.ResolveApproach(QuestApproach.A);
            Assert.True(sys.RateCardRevised);
            Assert.False(sys.PlantSeized);
            Assert.False(sys.QueueChitIsLiveCurrency); // becomes a relic
        }

        [Fact]
        public void HydroBarons_ApproachBImposesReform()
        {
            var sys = new HydroBaronsSystem();
            sys.ResolveApproach(QuestApproach.B);
            Assert.True(sys.AdminReform);
            Assert.True(sys.RateCardRevised);
        }

        [Fact]
        public void HydroBarons_ApproachDiscardableOnceOnly()
        {
            var sys = new HydroBaronsSystem();
            Assert.True(sys.ResolveApproach(QuestApproach.D));
            Assert.False(sys.ResolveApproach(QuestApproach.C));
            Assert.Equal("D", sys.ChosenApproach);
        }

        [Fact]
        public void HydroBarons_QueueAdvancesBeforeResolution()
        {
            var sys = new HydroBaronsSystem();
            Assert.True(sys.AdvanceQueue(20));
            Assert.Equal(20, sys.QueuePosition);
            sys.ResolveApproach(QuestApproach.C);
            Assert.False(sys.AdvanceQueue(5)); // resolved → queue frozen
        }

        [Fact]
        public void HydroBarons_SaveLoadRoundTrips()
        {
            var sys = new HydroBaronsSystem();
            sys.AdvanceQueue(25);
            sys.ResolveApproach(QuestApproach.B);

            var restored = new HydroBaronsSystem();
            restored.RestoreState(sys.CaptureState());
            Assert.True(restored.AdminReform);
            Assert.True(restored.RateCardRevised);
            Assert.Equal(25, restored.QueuePosition);
            Assert.Equal("B", restored.ChosenApproach);
            Assert.Equal(SaveChecksum.Compute(sys.CaptureState()),
                SaveChecksum.Compute(restored.CaptureState()));
        }
    }
}
