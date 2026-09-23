// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 131: Wasteland Information & Rumor Network — Integration Tests
// Verifies hub catalog loading, rumor generation, propagation truthfulness
// decay, intercept mechanics, briefing reports, and save/restore.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.InformationFlow;

namespace Ashfall.Core.Tests.Plan131RumorNetwork
{
    public sealed class Plan131RumorNetworkIntegrationTests
    {
        private static string ResolveDataPath(string filename)
        {
            var candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates) { if (File.Exists(c)) return Path.GetFullPath(c); }
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename));
        }

        [Fact]
        public void LoadCatalog_RegistersHubsFromJson()
        {
            var sys = new RumorSystem();
            string path = ResolveDataPath("rumor_hubs.json");
            Assert.True(File.Exists(path), $"rumor_hubs.json must exist at {path}");

            sys.LoadCatalog(File.ReadAllText(path));

            Assert.True(sys.HubCount >= 1, $"Expected >= 1 hub, got {sys.HubCount}");
            Assert.Contains(sys.Hubs, h => h.HubId == "hub_crossroads_trading_post");
        }

        [Fact]
        public void GenerateRumor_CreatesRumorAndPropagatesDecay()
        {
            var sys = new RumorSystem();
            sys.RegisterHub("hub_market", "Market Square", "loc_market", 0.9f);
            sys.RegisterHub("hub_outpost", "Northern Outpost", "loc_outpost", 0.6f);

            WastelandRumor? generated = null;
            sys.OnRumorGenerated += r => generated = r;

            var rumor = sys.GenerateRumor("loc_market", RumorSubjectType.Faction,
                "faction_wardens", "Wardens mass at eastern gate", "Spotted three platoons heading east.", 0.9f, 1);

            Assert.NotNull(generated);
            Assert.Equal(rumor.RumorId, generated!.RumorId);
            Assert.Equal(RumorSubjectType.Faction, rumor.SubjectType);
            Assert.True(rumor.Truthfulness > 0f);

            // Propagate to second hub — truthfulness should mutate down
            float beforeProp = rumor.Truthfulness;
            bool propagated = sys.PropagateRumorToHub(rumor.RumorId, "hub_outpost");
            Assert.True(propagated);
            Assert.True(rumor.Truthfulness <= beforeProp);
            Assert.Contains("hub_outpost", rumor.ReachedHubIds);
        }

        [Fact]
        public void TickDay_DecaysAndExpiresOldRumors()
        {
            var sys = new RumorSystem();
            sys.RegisterHub("hub_base", "Base Camp", "loc_base", 0.8f);

            var rumor = sys.GenerateRumor("loc_base", RumorSubjectType.Event,
                "evt_flood", "Flood Warning", "Northern river overflowing.", 0.05f, 1);

            // Tick 25 days — decay rate 0.05 × 25 = 1.25 → should expire when truthfulness ≤ 0
            for (int day = 2; day <= 25; day++)
            {
                sys.TickDay(day);
            }

            Assert.Equal(0, sys.TotalRumorCount); // expired
        }

        [Fact]
        public void InterceptRumor_MarksRumorAndFiresEvent()
        {
            var sys = new RumorSystem();
            sys.RegisterHub("hub_intel", "Intel Station", "loc_intel", 0.95f);

            var rumor = sys.GenerateRumor("loc_intel", RumorSubjectType.Economy,
                "trade_north", "Northern convoy spotted", "Heavily laden caravan heading to the depot.", 0.85f, 3);

            WastelandRumor? intercepted = null;
            sys.OnRumorIntercepted += r => intercepted = r;

            bool result = sys.InterceptRumor(rumor.RumorId);

            Assert.True(result);
            Assert.True(rumor.IsIntercepted);
            Assert.NotNull(intercepted);
            Assert.Equal(1, sys.InterceptedRumorCount);
        }

        [Fact]
        public void CreateBriefingReport_AggregatesRumorsAtLocation()
        {
            var sys = new RumorSystem();
            sys.RegisterHub("hub_town", "Town Council Hall", "loc_town", 0.88f);

            sys.GenerateRumor("loc_town", RumorSubjectType.Faction, "faction_red", "Red faction movements", "...", 0.9f, 1);
            sys.GenerateRumor("loc_town", RumorSubjectType.Economy, "trade_grain", "Grain price spike", "...", 0.75f, 1);

            var report = sys.CreateBriefingReport("loc_town");

            Assert.Equal("loc_town", report.LocationId);
            Assert.Equal(2, report.TotalItems);
            Assert.True(report.AverageTruthfulness > 0f);
            Assert.True(report.ThreatCount >= 1); // Faction type = threat
        }

        [Fact]
        public void SaveRestoreState_PreservesRumorsAndHubs()
        {
            var sys = new RumorSystem();
            sys.RegisterHub("hub_a", "Hub Alpha", "loc_a", 0.7f);
            sys.RegisterHub("hub_b", "Hub Beta", "loc_b", 0.6f);
            var r1 = sys.GenerateRumor("loc_a", RumorSubjectType.Location, "loc_ruin", "Ruins spotted north", "...", 0.8f, 2);
            sys.PropagateRumorToHub(r1.RumorId, "hub_b");
            sys.InterceptRumor(r1.RumorId);

            var captured = sys.CaptureState();
            var restored = new RumorSystem();
            restored.RestoreState(captured);

            Assert.Equal(sys.TotalRumorCount, restored.TotalRumorCount);
            Assert.Equal(sys.HubCount, restored.HubCount);
            Assert.Equal(sys.InterceptedRumorCount, restored.InterceptedRumorCount);
            Assert.Contains(restored.Rumors, r => r.RumorId == r1.RumorId && r.IsIntercepted);
        }
    }
}
