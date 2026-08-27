using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Maritime;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class MaritimeDiveSystemTests
    {
        [Fact]
        public void RegisterSite_CreatesSite()
        {
            var md = Create();
            int initialCount = md.State.sites.Count;
            var r = md.RegisterSite("site_reef", "Sunken Reef", 30f, 0.2f);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Equal(initialCount + 1, md.State.sites.Count);
        }

        [Fact]
        public void RegisterSite_Duplicate_Blocks()
        {
            var md = Create();
            md.RegisterSite("site_reef", "Sunken Reef", 30f, 0.2f);
            var r = md.RegisterSite("site_reef", "Sunken Reef", 30f, 0.2f);
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact]
        public void ConductDive_ReturnsOutcome()
        {
            var md = Create();
            md.RegisterSite("site_reef", "Sunken Reef", 30f, 0.1f);
            var r = md.ConductDive("site_reef", "diver_1", 0.9f);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(md.State.outcomes);
        }

        [Fact]
        public void ConductDive_UnknownSite_Fails()
        {
            var md = Create();
            var r = md.ConductDive("nonexistent", "diver_1", 0.9f);
            Assert.Equal(ActionResult.StatusKind.Failed, r.Status);
        }

        [Fact]
        public void ConductDive_MarksSiteExplored()
        {
            var md = Create();
            md.RegisterSite("site_reef", "Sunken Reef", 30f, 0.1f);
            md.ConductDive("site_reef", "diver_1", 0.9f);
            var site = md.State.sites.Find(s => s.siteId == "site_reef");
            Assert.NotNull(site);
            Assert.True(site!.isExplored);
        }

        [Fact]
        public void ConductDive_TracksRadiationDose()
        {
            var md = Create();
            md.RegisterSite("site_hot", "Hot Zone Wreck", 50f, 0.8f);
            md.ConductDive("site_hot", "diver_1", 0.5f);
            var outcome = md.State.outcomes[0];
            Assert.True(outcome.radiationDose > 0);
        }

        [Fact]
        public void CaptureRestoreState_PreservesSites()
        {
            var md = Create();
            md.RegisterSite("site_reef", "Sunken Reef", 30f, 0.2f);
            md.ConductDive("site_reef", "diver_1", 0.9f);
            var state = md.CaptureState();
            Assert.NotEmpty(state.sites);
            Assert.Single(state.outcomes);

            var md2 = Create();
            md2.RestoreState(state);
            Assert.Equal(state.sites.Count, md2.State.sites.Count);
            Assert.Single(md2.State.outcomes);
        }

        // ── Substep 6: Oxygen, Decompression, Abort, Diver-Loss, and Reload Tests ──

        [Fact]
        public void Dive_Oxygen_DepletesAndCompressorReplenishes()
        {
            var md = Create();
            md.StartDive("diver_sarah", "operator_chen", initialAir: 100f);
            Assert.True(md.IsActive);
            Assert.Equal(100f, md.AirSupplySeconds);

            // Tick 30 seconds
            md.Tick(30f);
            Assert.True(md.AirSupplySeconds <= 70f);

            // Crank compressor (+30s)
            md.CrankCompressor();
            Assert.True(md.AirSupplySeconds > 90f);
            Assert.True(md.AirSupplySeconds <= 100f);
        }

        [Fact]
        public void Dive_Oxygen_WarningFiresAtLowAir()
        {
            var md = Create();
            md.StartDive("diver_sarah", "operator_chen", initialAir: 60f);

            float reportedAir = -1f;
            int warningFired = 0;
            md.OnAirWarning += air => { warningFired++; reportedAir = air; };

            md.Tick(35f); // Air = ~25s (<= 30)
            Assert.Equal(1, warningFired);
            Assert.True(reportedAir <= 30f);

            // Ticking again does not refire warning
            md.Tick(5f);
            Assert.Equal(1, warningFired);
        }

        [Fact]
        public void Dive_Decompression_BuildsInDeepChambers()
        {
            var md = Create();
            md.StartDive("diver_sarah", "operator_chen", initialAir: 120f);

            // Room 0: Deckhouse (no decompression)
            Assert.Equal(0f, md.DecompressionRequiredSeconds);

            // Advance to Room 1: Companionway
            md.AdvanceToNextRoom(10);
            Assert.Equal(1, md.CurrentRoomIndex);
            Assert.Equal(0f, md.DecompressionRequiredSeconds);

            // Advance to Room 2: Hold Approach (requires 20s decompression)
            md.AdvanceToNextRoom(15);
            Assert.Equal(2, md.CurrentRoomIndex);
            Assert.Equal(20f, md.DecompressionRequiredSeconds);

            // Advance to Room 3: Deep Hold (requires 40s decompression)
            md.AdvanceToNextRoom(20);
            Assert.Equal(3, md.CurrentRoomIndex);
            Assert.Equal(40f, md.DecompressionRequiredSeconds);
        }

        [Fact]
        public void Dive_Decompression_CanCompleteSafely()
        {
            var md = Create();
            md.StartDive("diver_sarah", "operator_chen", initialAir: 120f);
            md.AdvanceToNextRoom(10);
            md.AdvanceToNextRoom(10); // Room 2: 20s decompression required

            bool decompStarted = false;
            bool decompCompleted = false;
            md.OnDecompressionStarted += _ => decompStarted = true;
            md.OnDecompressionCompleted += () => decompCompleted = true;

            md.StartDecompression();
            Assert.True(decompStarted);
            Assert.True(md.IsDecompressing);

            // Tick 25 seconds through decompression
            md.Tick(25f);
            Assert.True(decompCompleted);
            Assert.False(md.IsDecompressing);
            Assert.Equal(0f, md.DecompressionRequiredSeconds);
            Assert.False(md.HasDecompressionSickness);
        }

        [Fact]
        public void Dive_EmergencyAbort_CausesDecompressionSicknessWhenSkipped()
        {
            var md = Create();
            md.StartDive("diver_sarah", "operator_chen", initialAir: 120f);
            md.AdvanceToNextRoom(10);
            md.AdvanceToNextRoom(10);
            md.AdvanceToNextRoom(10); // Deep hold: 40s decomp required

            md.AbortDive(emergency: true);

            Assert.False(md.IsActive);
            Assert.True(md.HasDecompressionSickness);
            Assert.True(md.AccumulatedRadiationDose >= 25f);
        }

        [Fact]
        public void Dive_DiverLoss_OccursOnDeepAsphyxiation()
        {
            var md = Create();
            md.StartDive("diver_sarah", "operator_chen", initialAir: 50f);
            md.AdvanceToNextRoom(10);
            md.AdvanceToNextRoom(10); // Deep chamber (index 2)

            string lostDiver = string.Empty;
            md.OnDiverLost += id => lostDiver = id;

            // Deplete all air
            md.Tick(60f);

            Assert.False(md.IsActive);
            Assert.True(md.DiverLost);
            Assert.Equal("diver_sarah", lostDiver);
            Assert.Equal(DiveResult.CrewLost, md.Outcomes[md.Outcomes.Count - 1].result);
        }

        [Fact]
        public void Dive_DiverLoss_OccursOnCatastrophicDeepBreach()
        {
            var md = Create();
            md.StartDive("diver_sarah", "operator_chen", initialAir: 120f);
            md.AdvanceToNextRoom(30); // Room 1: Noise 30
            md.AdvanceToNextRoom(30); // Room 2: Noise 60
            md.AdvanceToNextRoom(45); // Room 3: Noise 105 (catastrophic breach in deep hold)

            Assert.False(md.IsActive);
            Assert.True(md.DiverLost);
            Assert.True(md.IsCompromised);
            Assert.Equal(DiveResult.CrewLost, md.Outcomes[md.Outcomes.Count - 1].result);
        }

        [Fact]
        public void Dive_FullState_ReloadPreservesAllFields()
        {
            var md = Create();
            md.StartDive("diver_sarah", "operator_chen", initialAir: 100f, siteId: "site_exp09_naval_patrol");
            md.AdvanceToNextRoom(25);
            md.Tick(20f);

            var save = md.CaptureState();
            Assert.True(save.isActive);
            Assert.Equal("diver_sarah", save.diverDwellerId);
            Assert.Equal("operator_chen", save.compressorOperatorDwellerId);
            Assert.Equal("site_exp09_naval_patrol", save.siteId);
            Assert.Equal(1, save.currentRoomIndex);
            Assert.Equal(25, save.noiseLevel);
            Assert.True(save.airSupplySeconds < 100f);

            var restored = Create();
            restored.RestoreState(save);

            Assert.True(restored.IsActive);
            Assert.Equal("diver_sarah", restored.DiverDwellerId);
            Assert.Equal("operator_chen", restored.CompressorOperatorDwellerId);
            Assert.Equal("site_exp09_naval_patrol", restored.CurrentSiteId);
            Assert.Equal(1, restored.CurrentRoomIndex);
            Assert.Equal(25, restored.NoiseLevel);
            Assert.Equal(save.airSupplySeconds, restored.AirSupplySeconds);
        }

        private static MaritimeDiveSystem Create() => new MaritimeDiveSystem(new SeededRng(42));
    }
}
