using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Maritime;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 23 Task 23C — deterministic tides and the weather → surge producer path.
    /// Tide phase derives purely from the authoritative campaign day (4-day cycle):
    /// no wall clock, no RNG, no serialized phase. Surge state has exactly one
    /// producer — the deep-coast daily tick consuming WeatherSystem weather — and
    /// persists additively (old saves default to no surge).
    /// </summary>
    public class Plan23CoastalDynamicsTests
    {
        private static DiveSiteContainer LoadSites()
            => DiveSiteCatalogLoader.Load(
                DataDir(),
                new FileSystemIO(), new SystemTextJsonSerializer());

        private static string DataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new InvalidOperationException("StreamingAssets/Data directory not found");
        }

        private static DiveSiteContainer LoadCatalogSafe()
            => DiveSiteCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

        // ── Deterministic tide calendar ──────────────────────────────

        [Fact]
        public void Tide_Phase_DerivesFromCampaignDayWithoutRng()
        {
            Assert.Equal(TidePhase.Low, TideCalendar.PhaseFor(0));
            Assert.Equal(TidePhase.Rising, TideCalendar.PhaseFor(1));
            Assert.Equal(TidePhase.High, TideCalendar.PhaseFor(2));
            Assert.Equal(TidePhase.Falling, TideCalendar.PhaseFor(3));
            Assert.Equal(TidePhase.Low, TideCalendar.PhaseFor(4));
            Assert.Equal(TideCalendar.PhaseFor(400), TideCalendar.PhaseFor(1016));
        }

        [Fact]
        public void Tide_Windows_ArePredictableGameTimeDecisions()
        {
            Assert.True(TideCalendar.IsWindowOpen(TideWindow.Any, 0));
            Assert.True(TideCalendar.IsWindowOpen(TideWindow.Slack, 1));  // rising = slack turn
            Assert.True(TideCalendar.IsWindowOpen(TideWindow.Slack, 3));  // falling = slack turn
            Assert.False(TideCalendar.IsWindowOpen(TideWindow.Slack, 0)); // low day
            Assert.True(TideCalendar.IsWindowOpen(TideWindow.LowOnly, 0));
            Assert.False(TideCalendar.IsWindowOpen(TideWindow.LowOnly, 2));
            Assert.True(TideCalendar.IsWindowOpen(TideWindow.HighOnly, 2));
            Assert.True(TideCalendar.IsWindowOpen(TideWindow.FallingOnly, 3));
            Assert.False(TideCalendar.IsWindowOpen(TideWindow.FallingOnly, 1));
            Assert.False(TideCalendar.IsWindowOpen(TideWindow.UnsafeAtPeak, 1));
            Assert.True(TideCalendar.IsWindowOpen(TideWindow.UnsafeAtPeak, 0));

            // Next-window stability: deterministic horizon, no phase drift.
            Assert.Equal(0, TideCalendar.DaysUntilOpen(TideWindow.Slack, 1));
            Assert.Equal(1, TideCalendar.DaysUntilOpen(TideWindow.Slack, 4)); // next rising turn
            Assert.Equal(TideCalendar.DaysUntilOpen(TideWindow.LowOnly, 2),
                         TideCalendar.DaysUntilOpen(TideWindow.LowOnly, 6)); // same phase (High) → same answer
        }

        [Fact]
        public void Tide_OldSaves_StayUngatedWithoutDayAuthority()
        {
            Assert.True(TideCalendar.IsWindowOpen(TideWindow.Slack, -1));
            Assert.True(TideCalendar.IsWindowOpen(TideWindow.LowOnly, -1));
        }

        [Fact]
        public void Tide_SixSites_AuthoredWithVariedWindows()
        {
            var container = LoadCatalogSafe();
            Assert.Equal(14, container.dive_sites.Count);

            var windows = container.dive_sites.Select(s => s.tide_window).ToList();
            Assert.Equal(14, windows.Count(w => !string.IsNullOrEmpty(w)));
            Assert.Contains("slack", windows);
            Assert.Contains("low", windows);
            Assert.Contains("high", windows);
            Assert.Contains("falling", windows);
            Assert.Contains("unsafe_at_peak", windows);
            Assert.Equal(6, windows.Count(w => w != "any"));

            var siphon = container.dive_sites.First(s => s.site_id == "site_exp09_submerged_siphon");
            Assert.Equal("high", siphon.tide_window);
        }

        [Fact]
        public void Tide_LaunchGate_BlocksAndAdmits_ByCampaignDay()
        {
            var dive = new MaritimeDiveSystem(new SeededRng(11));
            dive.LoadCatalog(LoadCatalogSafe());

            // Metro: low-tide-only. Day 4 = Low (open), day 2 = High (closed).
            Assert.True(dive.CanLaunch("site_exp09_flooded_metro", 4, null, out var openBlocker));
            Assert.Equal(string.Empty, openBlocker);

            Assert.False(dive.CanLaunch("site_exp09_flooded_metro", 2, null, out var closedBlocker));
            Assert.StartsWith("tide:", closedBlocker);

            // Gear gate still fires after the tide admits.
            Assert.False(dive.CanLaunch("site_exp23_brine_cistern", 0, Array.Empty<string>(), out var gearBlocker));
            Assert.Equal("item_rebreather_canister", gearBlocker);

            // Old-save behavior: no day authority → ungated.
            Assert.True(dive.CanLaunch("site_exp09_ss_sovereign", -1, null, out _));
        }

        [Fact]
        public void Tide_GateIsDeterministic_AcrossRepeatedCalls()
        {
            Assert.Equal(TideCalendar.IsWindowOpen(TideWindow.Slack, 77), TideCalendar.IsWindowOpen(TideWindow.Slack, 77));
            Assert.Equal(TideCalendar.PhaseFor(77), TideCalendar.PhaseFor(77));
            Assert.Equal(TideCalendar.DaysUntilOpen(TideWindow.LowOnly, 77), TideCalendar.DaysUntilOpen(TideWindow.LowOnly, 77));
        }

        // ── Storm surge: one producer path (weather → deep-coast tick) ──

        [Fact]
        public void Surge_BeginAccumulateRecede_IsAuthoritative()
        {
            var deepCoast = new District8DeepCoastSystem();

            deepCoast.TickDaily(10, WeatherKind.Clear);
            Assert.False(deepCoast.IsSurgeActive);

            deepCoast.TickDaily(11, WeatherKind.FalloutStorm);
            Assert.True(deepCoast.IsSurgeActive);
            Assert.Equal(11, deepCoast.SurgeActiveDay);
            Assert.Contains(District8DeepCoastSystem.JournalSurgeBegan, deepCoast.State.narrativeMarkers);

            // Calm days hold the surge until the recede lag elapses.
            deepCoast.TickDaily(12, WeatherKind.Clear);
            Assert.True(deepCoast.IsSurgeActive);
            deepCoast.TickDaily(13, WeatherKind.Clear);
            Assert.False(deepCoast.IsSurgeActive);
            Assert.Contains(District8DeepCoastSystem.JournalSurgeAftermath, deepCoast.State.narrativeMarkers);
        }

        [Fact]
        public void Surge_Contamination_RisesDuringStorm()
        {
            var deepCoast = new District8DeepCoastSystem();
            deepCoast.TickDaily(5, WeatherKind.Clear);
            float before = deepCoast.ContaminationLevel;
            deepCoast.TickDaily(6, WeatherKind.FalloutStorm);
            Assert.True(deepCoast.ContaminationLevel > before);
        }

        [Fact]
        public void Surge_OldSaves_DefaultToNoSurge_AndRoundTrip()
        {
            var deepCoast = new District8DeepCoastSystem();
            var fresh = deepCoast.CaptureState();
            Assert.Equal(-1, fresh.surgeActiveDay);
            Assert.Equal(-1, fresh.surgeLastStormDay);
            Assert.False(deepCoast.IsSurgeActive);

            deepCoast.TickDaily(30, WeatherKind.FalloutStorm);
            var saved = deepCoast.CaptureState();
            var restored = new District8DeepCoastSystem();
            restored.RestoreState(saved);
            Assert.True(restored.IsSurgeActive);
            Assert.Equal(30, restored.SurgeActiveDay);
        }

        [Fact]
        public void Surge_DockOperation_BlockedDuringSurge_RecoversAfterRecede()
        {
            var deepCoast = new District8DeepCoastSystem();
            deepCoast.SurveyPerimeter(10);
            deepCoast.MakeReopeningDecision(DeepCoastAccessDecision.StabilizeRepair, 11, new SeededRng(3));
            Assert.True(deepCoast.TryClearPerimeter(11, bill => true));
            Assert.True(deepCoast.TryClearServiceChannel(11, bill => true));
            Assert.True(deepCoast.TryRepairDeepBerth(11, bill => true));
            Assert.True(deepCoast.CanStartDockOperation);

            deepCoast.TickDaily(40, WeatherKind.FalloutStorm);
            Assert.False(deepCoast.CanStartDockOperation); // surge blocks the berth

            deepCoast.TickDaily(41, WeatherKind.Clear);
            deepCoast.TickDaily(42, WeatherKind.Clear);
            deepCoast.TickDaily(43, WeatherKind.Clear); // lag satisfied → recede
            Assert.False(deepCoast.IsSurgeActive);
            Assert.True(deepCoast.CanStartDockOperation);
            Assert.Contains(District8DeepCoastSystem.JournalSurgeAftermath, deepCoast.State.narrativeMarkers);
        }

        [Fact]
        public void Surge_TicksAreIdempotent_PerCalendarDay()
        {
            var deepCoast = new District8DeepCoastSystem();
            deepCoast.TickDaily(20, WeatherKind.FalloutStorm);
            float first = deepCoast.ContaminationLevel;
            deepCoast.TickDaily(20, WeatherKind.FalloutStorm); // same day again — guarded
            Assert.Equal(first, deepCoast.ContaminationLevel, 5);
        }

        [Fact]
        public void Tide_Phase_IsPureFunction_NoWallClock()
        {
            // Same day → same phase; no time source involved.
            Assert.Equal(TidePhase.Low, TideCalendar.PhaseFor(4));
            Assert.Equal(TidePhase.Rising, TideCalendar.PhaseFor(5));
            Assert.Equal(TideCalendar.PhaseFor(1000), TideCalendar.PhaseFor(4 + 4 * 248));
        }
    }
}
