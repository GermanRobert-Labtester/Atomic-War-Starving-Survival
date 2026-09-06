using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class Plan167EspionageTests
    {
        private sealed class FixedRng : ISeededRng
        {
            private readonly Queue<double> _values;
            public FixedRng(params double[] values) => _values = new Queue<double>(values);
            public int Seed => 167;
            public int Next(int minInclusive, int maxExclusive) => minInclusive;
            public float NextFloat() => (float)NextDouble();
            public double NextDouble() => _values.Count > 0 ? _values.Dequeue() : 0.99;
        }

        private static EspionageSystem Create(ISeededRng rng, bool researchComplete = true, Action<string, bool>? setAway = null)
        {
            var system = new EspionageSystem();
            system.BindRng(rng);
            system.BindAgentAvailability(_ => true, _ => true, setAway);
            system.BindAgentCapability(_ => 1f);
            system.BindResearchGate(_ => researchComplete);
            system.LoadMissionCatalog(new[]
            {
                new EspionageMissionDef
                {
                    Id = "espionage_test_surveillance",
                    MissionType = EspionageMissionType.Surveillance,
                    BaseDurationDays = 3,
                    BaseDetectionChance = 0.05f,
                    IntelGain = 2,
                    NetworkExposureGain = 0.05f,
                    IntelFactIds = new List<string> { "intel_fact_test_route" }
                },
                new EspionageMissionDef
                {
                    Id = "espionage_test_signal",
                    MissionType = EspionageMissionType.CommunicationsIntelligence,
                    BaseDurationDays = 1,
                    BaseDetectionChance = 0.05f,
                    IntelGain = 1,
                    RequiredResearchId = "knowledge_signal_triangulation"
                }
            });
            return system;
        }

        [Fact]
        public void DeployUsesCanonicalFactionAndReservesSurvivor()
        {
            bool away = false;
            var system = Create(new FixedRng(0.99), setAway: (_, value) => away = value);

            var result = system.DeployAgent("survivor_1", "iron_garrison", "espionage_test_surveillance", 10);

            Assert.True(result.IsSuccess);
            Assert.True(away);
            Assert.True(system.IsAgentCommitted("survivor_1"));
            Assert.Equal("faction_central_garrison", system.State.networks.Single().targetFactionId);
        }

        [Fact]
        public void IntelProgressionIsDeterministicAndFactsEmitOnce()
        {
            var firstFacts = new List<EspionageIntelFactState>();
            var first = Create(new FixedRng(0.99, 0.99, 0.99, 0.99));
            first.OnIntelDiscovered += fact => firstFacts.Add(fact);
            Assert.True(first.DeployAgent("survivor_1", "iron_garrison", "espionage_test_surveillance", 1).IsSuccess);
            first.Tick(4);

            var second = Create(new FixedRng(0.99, 0.99, 0.99, 0.99));
            Assert.True(second.DeployAgent("survivor_1", "iron_garrison", "espionage_test_surveillance", 1).IsSuccess);
            second.Tick(4);

            Assert.Single(firstFacts);
            Assert.Single(first.State.knownIntel);
            Assert.Equal(first.State.knownIntel[0].factId, second.State.knownIntel[0].factId);
            Assert.Equal(first.State.networks[0].intelLevel, second.State.networks[0].intelLevel);
            Assert.Empty(first.State.activeMissions.Select(m =>
                $"{m.missionInstanceId}:{m.status}:{m.progressDays}:{m.lastTickDay}"));
        }

        [Fact]
        public void DetectionCreatesOneCapturedAgentAndRansomReleasesIt()
        {
            bool away = false;
            var system = Create(new FixedRng(0.0), setAway: (_, value) => away = value);
            Assert.True(system.DeployAgent("survivor_1", "faction_upland_militia", "espionage_test_surveillance", 7).IsSuccess);
            system.Tick(8);

            Assert.Single(system.State.capturedAgents);
            Assert.Equal(EspionageMissionStatus.Captured, system.State.networks[0].status);
            Assert.True(system.PayRansom("survivor_1", 8).IsSuccess);
            Assert.Empty(system.State.capturedAgents);
            Assert.False(away);
        }

        [Fact]
        public void ResearchGateBlocksLockedMissionWithoutMutation()
        {
            var system = Create(new FixedRng(0.99), researchComplete: false);

            var result = system.DeployAgent("survivor_1", "iron_garrison", "espionage_test_signal", 1);

            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.Equal("research_locked", result.FailureCode);
            Assert.Empty(system.State.networks);
        }

        [Fact]
        public void RestorePreservesNetworkAndDoesNotReplayIntelEvent()
        {
            var system = Create(new FixedRng(0.99, 0.99, 0.99));
            Assert.True(system.DeployAgent("survivor_1", "iron_garrison", "espionage_test_surveillance", 1).IsSuccess);
            system.Tick(2);
            var saved = system.CaptureState();
            int replayed = 0;
            var restored = Create(new FixedRng(0.99));
            restored.OnIntelDiscovered += _ => replayed++;
            restored.RestoreState(saved);

            Assert.Equal(0, replayed);
            Assert.Equal(saved.networks[0].networkId, restored.State.networks[0].networkId);
            Assert.Equal(saved.knownIntel.Count, restored.State.knownIntel.Count);
        }

        [Fact]
        public void MissionCatalogLoadsAndValidatesAuthoritativeData()
        {
            string root = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
            string dataDir = System.IO.Path.Combine(root, "Assets", "StreamingAssets", "Data");
            var missions = EspionageMissionCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.Equal(5, missions.Count);
            Assert.True(EspionageMissionCatalogLoader.Validate(missions, out var error), error);
        }
    }
}
