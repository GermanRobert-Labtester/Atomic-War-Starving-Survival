// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 133: Expedition Discovery → Persistent World Consequences — Integration Tests
// Verifies consequence catalog loading, discovery registration with auto-outcomes,
// exploit/conceal/escalate lifecycle, caravan safety accumulation, and save/restore.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Expeditions;

namespace Ashfall.Core.Tests.Plan133DiscoveryConsequence
{
    public sealed class Plan133DiscoveryConsequenceIntegrationTests
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
        public void LoadCatalog_LoadsAllFiveConsequenceTypes()
        {
            var sys = new DiscoveryConsequenceSystem();
            string path = ResolveDataPath("discovery_consequences.json");
            Assert.True(File.Exists(path), $"discovery_consequences.json must exist at {path}");

            sys.LoadCatalog(File.ReadAllText(path));

            var types = sys.GetAllConsequenceTypes();
            Assert.Equal(5, types.Count);
            Assert.Contains(types, t => t.type_id == "csq_threat_cleared");
            Assert.Contains(types, t => t.type_id == "csq_resource_deposit");
            Assert.Contains(types, t => t.type_id == "csq_faction_contact");
        }

        [Fact]
        public void RegisterDiscovery_ThreatCleared_AutoGeneratesCaravanSafetyOutcome()
        {
            var sys = new DiscoveryConsequenceSystem();

            ConsequenceOutcome? fired = null;
            sys.OnConsequenceTriggered += c => fired = c;

            var record = sys.RegisterDiscovery(
                "loc_bridge_sector", DiscoveryType.ThreatCleared,
                "Bridge garrison neutralised", "Hostile holdouts cleared from crossing.",
                day: 5, tags: new[] { "combat", "safe_route" });

            Assert.NotNull(fired);
            Assert.Equal(ConsequenceStatus.Discovered, record.Status);
            Assert.True(fired!.CaravanSafetyBonus > 0f, "ThreatCleared should produce a positive caravan safety bonus");
            Assert.Equal(record.DiscoveryId, fired.DiscoveryId);
        }

        [Fact]
        public void ExploitDiscovery_ResourceDeposit_UpdatesStatusAndYieldsStandingBonus()
        {
            var sys = new DiscoveryConsequenceSystem();

            var record = sys.RegisterDiscovery(
                "loc_copper_vein", DiscoveryType.ResourceDeposit,
                "Copper vein found", "Substantial deposit beneath the old factory.",
                day: 8, resourceYield: "copper_ore", associatedFactionId: "faction_miners");

            Assert.Equal(ConsequenceStatus.Discovered, record.Status);

            var outcome = sys.ExploitDiscovery(record.DiscoveryId, day: 10);

            Assert.NotNull(outcome);
            Assert.Equal(ConsequenceStatus.Exploited, record.Status);
            Assert.True(outcome!.FactionStandingDelta > 0f, "Exploiting resource with faction should yield standing");
        }

        [Fact]
        public void ConcealAndEscalate_AffectStatusCorrectly()
        {
            var sys = new DiscoveryConsequenceSystem();

            var record = sys.RegisterDiscovery(
                "loc_bunker_7", DiscoveryType.RuinsUncovered,
                "Old command bunker", "Pre-war military facility, partially intact.",
                day: 3);

            bool concealed = sys.ConcealDiscovery(record.DiscoveryId);
            Assert.True(concealed);
            Assert.Equal(ConsequenceStatus.Concealed, record.Status);

            // Re-register different discovery and escalate
            var record2 = sys.RegisterDiscovery(
                "loc_cache_north", DiscoveryType.FactionContact,
                "Northern alliance envoy", "Diplomatic contact made.",
                day: 6, associatedFactionId: "faction_northern_alliance");

            var escalated = sys.EscalateDiscovery(record2.DiscoveryId, day: 9);
            Assert.NotNull(escalated);
            Assert.Equal(ConsequenceStatus.Escalated, record2.Status);
        }

        [Fact]
        public void GetConsequenceType_ReturnsCorrectDefFromCatalog()
        {
            var sys = new DiscoveryConsequenceSystem();
            sys.LoadCatalog(File.ReadAllText(ResolveDataPath("discovery_consequences.json")));

            var def = sys.GetConsequenceType(DiscoveryType.ThreatCleared);
            Assert.NotNull(def);
            Assert.Equal("csq_threat_cleared", def!.type_id);
            Assert.True(def.caravan_safety_bonus > 0f);
        }

        [Fact]
        public void SaveRestoreState_PreservesDiscoveriesAndConsequences()
        {
            var sys = new DiscoveryConsequenceSystem();

            var d1 = sys.RegisterDiscovery("loc_alpha", DiscoveryType.StrategicLocation,
                "Alpha Crossroads", "Key junction point.", day: 2);
            var d2 = sys.RegisterDiscovery("loc_beta", DiscoveryType.ThreatCleared,
                "Beta Safe Zone", "Cleared perimeter.", day: 4);
            sys.ExploitDiscovery(d1.DiscoveryId, day: 5);

            int discCount = sys.DiscoveryCount;
            int csqCount = sys.ConsequenceCount;
            float safetyBonus = sys.GetTotalCaravanSafetyBonus();

            var captured = sys.CaptureState();
            var restored = new DiscoveryConsequenceSystem();
            restored.RestoreState(captured);

            Assert.Equal(discCount, restored.DiscoveryCount);
            Assert.Equal(csqCount, restored.ConsequenceCount);
            Assert.Equal(safetyBonus, restored.GetTotalCaravanSafetyBonus(), 4);
        }
    }
}
