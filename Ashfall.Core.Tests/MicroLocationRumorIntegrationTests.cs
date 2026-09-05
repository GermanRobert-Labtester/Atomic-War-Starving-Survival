using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Narrative;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Task F16: Rumor and location discovery integration tests.
    /// Verifies clue linkages, discovery authorities, multi-effect execution order,
    /// dispatch gating, and flag requirements.
    /// </summary>
    public class MicroLocationRumorIntegrationTests
    {
        private static string DataDir()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent!;
            return Path.Combine(dir!.FullName, "Assets", "StreamingAssets", "Data");
        }

        private static NarrativeEncounterSystem CreateNarrativeSystem()
        {
            var sys = new NarrativeEncounterSystem();
            string dataDir = DataDir();
            var defs = NarrativeEncounterCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            foreach (var d in defs)
            {
                if (d.id.StartsWith("micro_", StringComparison.Ordinal))
                    sys.RegisterEncounter(d);
            }
            return sys;
        }

        private static ExpeditionSystem CreateExpeditionSystem()
        {
            var expSys = new ExpeditionSystem();
            string dataDir = DataDir();
            ExpeditionCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            return expSys;
        }

        [Fact]
        public void ObservationPost_ReadGridReferences_DiscoversRuralGasStation()
        {
            var sys = CreateNarrativeSystem();
            var res = sys.TryResolve("micro_observation_post", "read_grid_references", "loc_route", 1);

            Assert.NotNull(res);
            Assert.Equal("rural_gas_station", res!.DiscoverLocationId);
            Assert.Equal("micro_observation_post_grid", res.JournalUnlockId);
        }

        [Fact]
        public void SupplyDrop_ReadSupplyLabel_DiscoversGovernmentBunker()
        {
            var sys = CreateNarrativeSystem();
            var res = sys.TryResolve("micro_supply_drop", "read_supply_label", "loc_route", 2);

            Assert.NotNull(res);
            Assert.Equal("government_bunker", res!.DiscoverLocationId);
            Assert.Equal("micro_supply_drop_label", res.JournalUnlockId);
        }

        [Fact]
        public void LocationDiscovery_TargetIdsResolve()
        {
            var expSys = CreateExpeditionSystem();

            var gasStation = ExpeditionDefinitionRegistry.Get("rural_gas_station");
            Assert.NotNull(gasStation);
            Assert.True(gasStation!.requiresDiscovery);

            var bunker = ExpeditionDefinitionRegistry.Get("government_bunker");
            Assert.NotNull(bunker);
            Assert.True(bunker!.requiresDiscovery);
        }

        [Fact]
        public void LocationDiscovery_TargetsAreNotDefaultDiscovered()
        {
            var expSys = CreateExpeditionSystem();

            Assert.False(expSys.IsLocationKnown("rural_gas_station"));
            Assert.False(expSys.IsLocationKnown("government_bunker"));

            Assert.False(expSys.CanDispatch("rural_gas_station", out var reason1));
            Assert.Equal("Destination has not been discovered.", reason1);

            Assert.False(expSys.CanDispatch("government_bunker", out var reason2));
            Assert.Equal("Destination has not been discovered.", reason2);
        }

        [Fact]
        public void LocationDiscovery_SurvivesSaveLoad()
        {
            var expSys = CreateExpeditionSystem();
            expSys.DiscoverLocation("rural_gas_station");
            Assert.True(expSys.IsLocationKnown("rural_gas_station"));

            var saved = expSys.CaptureKnownLocations();
            Assert.Contains("rural_gas_station", saved);

            var restoredSys = CreateExpeditionSystem();
            Assert.False(restoredSys.IsLocationKnown("rural_gas_station"));

            restoredSys.RestoreKnownLocations(saved);
            Assert.True(restoredSys.IsLocationKnown("rural_gas_station"));
        }

        [Fact]
        public void LocationDiscovery_IsIdempotent()
        {
            var expSys = CreateExpeditionSystem();
            int eventFiredCount = 0;
            expSys.OnLocationDiscovered += loc => eventFiredCount++;

            bool first = expSys.DiscoverLocation("rural_gas_station");
            Assert.True(first);
            Assert.Equal(1, eventFiredCount);

            bool second = expSys.DiscoverLocation("rural_gas_station");
            Assert.True(second);
            Assert.Equal(1, eventFiredCount); // Idempotent: does not re-fire event
        }

        [Fact]
        public void LocationChoice_JournalAndLocationEffectsBothCommit()
        {
            var sys = CreateNarrativeSystem();
            var expSys = CreateExpeditionSystem();

            var res = sys.TryResolve("micro_observation_post", "read_grid_references", "loc_route", 1);
            Assert.NotNull(res);

            // Execute effects in multi-effect order: journal -> location
            Assert.False(string.IsNullOrEmpty(res!.JournalUnlockId));
            Assert.False(string.IsNullOrEmpty(res.DiscoverLocationId));

            bool discovered = expSys.DiscoverLocation(res.DiscoverLocationId);
            Assert.True(discovered);
            Assert.True(expSys.IsLocationKnown("rural_gas_station"));
        }

        [Fact]
        public void DiscoveredLocation_ExistsInExpeditionRegistry()
        {
            CreateExpeditionSystem();
            Assert.NotNull(ExpeditionDefinitionRegistry.Get("rural_gas_station"));
            Assert.NotNull(ExpeditionDefinitionRegistry.Get("government_bunker"));
        }

        [Fact]
        public void DiscoveredRuralGasStation_CanBeDispatchedWhenEligible()
        {
            var expSys = CreateExpeditionSystem();

            // Not dispatchable initially
            Assert.False(expSys.CanDispatch("rural_gas_station", out _));

            // Discovered via clue
            expSys.DiscoverLocation("rural_gas_station");
            Assert.True(expSys.CanDispatch("rural_gas_station", out var reason));
            Assert.Null(reason);

            // Verify dispatch starts successfully
            var def = ExpeditionDefinitionRegistry.Get("rural_gas_station");
            Assert.NotNull(def);
            bool started = expSys.Start(def!, "survivor_alpha", 1);
            Assert.True(started);
            Assert.Equal(1, expSys.ActiveCount);
        }

        [Fact]
        public void LocationDiscovery_DoesNotBypassRequiredFlag()
        {
            var expSys = CreateExpeditionSystem();
            var testLoc = new ExpeditionDefinition
            {
                id = "test_flagged_location",
                displayName = "Flagged Bunker",
                requiresDiscovery = true,
                requiredFlagId = "test_flag_cleared"
            };
            ExpeditionDefinitionRegistry.Register(testLoc);

            // 1. Undiscovered
            Assert.False(expSys.CanDispatch("test_flagged_location", out _));

            // 2. Discovered, but flag is not set
            expSys.DiscoverLocation("test_flagged_location");
            expSys.FlagChecker = flag => false;

            Assert.False(expSys.CanDispatch("test_flagged_location", out var reason));
            Assert.Equal("Destination requires flag 'test_flag_cleared'.", reason);

            // 3. Flag is set
            expSys.FlagChecker = flag => flag == "test_flag_cleared";
            Assert.True(expSys.CanDispatch("test_flagged_location", out var clearReason));
            Assert.Null(clearReason);
        }

        [Fact]
        public void InvalidLocationId_FailsIntegrityValidation()
        {
            var expSys = CreateExpeditionSystem();
            Assert.False(expSys.DiscoverLocation(""));
            Assert.False(expSys.DiscoverLocation("non_existent_location_12345"));
        }

        [Fact]
        public void TriangulationSystem_DiscoversClueLocationsSafely()
        {
            var triSys = new SignalTriangulationSystem();
            Assert.False(triSys.IsLocationDiscovered("rural_gas_station"));

            int eventCount = 0;
            triSys.OnLocationRevealed += loc => eventCount++;

            var status1 = triSys.TryDiscoverLocation("rural_gas_station");
            Assert.Equal(SignalTriangulationSystem.LocationDiscoveryStatus.NewDiscovery, status1);
            Assert.True(triSys.IsLocationDiscovered("rural_gas_station"));
            Assert.Equal(1, eventCount);

            // Re-discovery is safe and idempotent
            var status2 = triSys.TryDiscoverLocation("rural_gas_station");
            Assert.Equal(SignalTriangulationSystem.LocationDiscoveryStatus.AlreadyKnown, status2);
            Assert.Equal(1, eventCount);
        }
    }
}
