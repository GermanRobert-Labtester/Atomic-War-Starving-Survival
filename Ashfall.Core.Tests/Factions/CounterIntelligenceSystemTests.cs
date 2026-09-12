// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Random;
using Xunit;

namespace Ashfall.Core.Tests.Factions
{
    public sealed class CounterIntelligenceSystemTests
    {
        private static string GetDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var path))
                return path;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data directory not found.");
        }

        private static CounterIntelligenceSystem CreateSystem(CounterIntelligenceState? state = null)
        {
            return new CounterIntelligenceSystem(state, new SeededRng(42), new TestLog());
        }

        private static VettingCandidateState CreateCandidate(string id, float scrutiny = 10f)
        {
            return new VettingCandidateState
            {
                candidateId = id,
                claimedBackground = "laborer",
                scrutinyRating = scrutiny,
                suspicionFlags = new System.Collections.Generic.List<string>(),
                quarantineClearance = "none",
                status = "awaiting_vetting",
                interviewCount = 0
            };
        }

        private static UndercoverAgentState CreateAgent(string survivorId, string profileId, bool exposed = false)
        {
            return new UndercoverAgentState
            {
                survivorId = survivorId,
                profileId = profileId,
                sourceFactionId = "faction_the_cutters",
                isExposed = exposed,
                exposureDay = -1,
                isInactive = false,
                inactivationDay = -1
            };
        }

        // ── Catalog ──────────────────────────────────────────────────

        [Fact]
        public void Catalog_LoadsSuccessfullyFromDataDir()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var catalog = InfiltratorCatalogLoader.Load(GetDataDir(), files, json);

            Assert.NotNull(catalog);
            Assert.NotEmpty(catalog!.Profiles);
            Assert.Contains(catalog.Profiles, p => p.ProfileId == "agent_cutter_saboteur");
            Assert.Equal(10, catalog.Profiles.Count);
        }

        [Fact]
        public void LoadCatalog_RegistersAllProfiles()
        {
            var system = CreateSystem();
            var catalog = new InfiltratorCatalog
            {
                Profiles = new System.Collections.Generic.List<InfiltratorProfileDef>
                {
                    new InfiltratorProfileDef { ProfileId = "agent_test", ConfessionThreshold = 0.5f, SabotageTargets = new System.Collections.Generic.List<string>{ "power" } }
                }
            };

            system.LoadCatalog(catalog);
            Assert.Single(system.Profiles);
            Assert.True(system.Profiles.ContainsKey("agent_test"));
        }

        // ── VetCandidate ─────────────────────────────────────────────

        [Fact]
        public void VetCandidate_UnknownCandidate_ReturnsFailed()
        {
            var system = CreateSystem();
            var result = system.VetCandidate("unknown_id", "officer_1");
            Assert.False(result.IsSuccess);
            Assert.Equal("unknown_candidate", result.FailureCode);
        }

        [Fact]
        public void VetCandidate_AlreadyVetted_ReturnsFailed()
        {
            var system = CreateSystem();
            var candidate = CreateCandidate("cand_1");
            candidate.status = "cleared";
            system.State.candidates.Add(candidate);

            var result = system.VetCandidate("cand_1", "officer_1");
            Assert.False(result.IsSuccess);
            Assert.Equal("already_vetted", result.FailureCode);
        }

        [Fact]
        public void VetCandidate_HighSuspicion_FlagsAndQuarantines()
        {
            var system = CreateSystem();
            var candidate = CreateCandidate("cand_1", scrutiny: 65f);
            candidate.suspicionFlags.Add("suspicious_package");
            system.State.candidates.Add(candidate);

            string? flaggedId = null;
            string? flagReason = null;
            system.OnCandidateFlagged += (id, reason) => { flaggedId = id; flagReason = reason; };

            var result = system.VetCandidate("cand_1", "officer_1");
            Assert.True(result.IsSuccess);
            Assert.Equal(75f, result.Suspicion);
            Assert.Equal("quarantined", result.Status);
            Assert.Equal("restricted", candidate.quarantineClearance);
            Assert.Equal("cand_1", flaggedId);
            Assert.Equal("high_suspicion", flagReason);
        }

        [Fact]
        public void VetCandidate_LowSuspicion_ClearsCandidate()
        {
            var system = CreateSystem();
            var candidate = CreateCandidate("cand_1", scrutiny: 10f);
            system.State.candidates.Add(candidate);

            var result = system.VetCandidate("cand_1", "officer_1");
            Assert.True(result.IsSuccess);
            Assert.Equal(10f, result.Suspicion);
            Assert.Equal("cleared", result.Status);
            Assert.Equal("full", candidate.quarantineClearance);
        }

        // ── Interrogate ──────────────────────────────────────────────

        [Fact]
        public void Interrogate_NotDetained_ReturnsFailed()
        {
            var system = CreateSystem();
            var result = system.Interrogate("not_detained", "officer_1", 1.0f);
            Assert.False(result.IsSuccess);
            Assert.Equal("not_detained", result.FailureCode);
        }

        [Fact]
        public void Interrogate_MaxInterrogations_ReturnsFailed()
        {
            var system = CreateSystem();
            system.State.detainees.Add(new DetaineeState { suspectId = "suspect_1", interrogationCount = 3 });
            var result = system.Interrogate("suspect_1", "officer_1", 1.0f);
            Assert.False(result.IsSuccess);
            Assert.Equal("max_interrogations", result.FailureCode);
        }

        [Fact]
        public void Interrogate_AgentExposedOnHighRoll()
        {
            var system = CreateSystem();
            var profile = new InfiltratorProfileDef
            {
                ProfileId = "agent_test",
                ConfessionThreshold = 0.5f,
                SabotageTargets = new System.Collections.Generic.List<string> { "power" }
            };
            system.RegisterProfile(profile);

            var agent = CreateAgent("survivor_1", "agent_test");
            system.State.undercoverAgents.Add(agent);
            system.State.detainees.Add(new DetaineeState { suspectId = "survivor_1", interrogationCount = 0 });

            // Use a deterministic mock RNG that returns 0.4 (< 0.5 threshold => full confession)
            var mockRng = new MockRng(0.4);
            // Recreate system with mock RNG
            var testSystem = new CounterIntelligenceSystem(system.State, mockRng, new TestLog());
            testSystem.RegisterProfile(profile);
            testSystem.OnCandidateFlagged += (id, reason) => { };
            testSystem.OnAgentExposed += id => { };
            testSystem.OnInterrogationCompleted += (id, outcome) => { };

            string? exposedId = null;
            testSystem.OnAgentExposed += id => exposedId = id;

            var result = testSystem.Interrogate("survivor_1", "officer_1", 1.0f);
            Assert.True(result.IsSuccess);
            Assert.Equal("full_confession", result.Outcome);
            Assert.True(agent.isExposed);
            Assert.Equal("survivor_1", exposedId);
            Assert.Equal(1, testSystem.State.detainees[0].interrogationCount);
        }

        // ── DetainSuspect ────────────────────────────────────────────

        [Fact]
        public void DetainSuspect_Success_AddsDetainee()
        {
            var system = CreateSystem();
            var result = system.DetainSuspect("suspect_1");
            Assert.True(result.IsSuccess);
            Assert.Single(system.State.detainees);
            Assert.Equal("suspect_1", system.State.detainees[0].suspectId);
        }

        [Fact]
        public void DetainSuspect_AlreadyDetained_ReturnsBlocked()
        {
            var system = CreateSystem();
            system.State.detainees.Add(new DetaineeState { suspectId = "suspect_1" });
            var result = system.DetainSuspect("suspect_1");
            Assert.False(result.IsSuccess);
            Assert.Equal("already_detained", result.FailureCode);
        }

        // ── AcceptDefector ───────────────────────────────────────────

        [Fact]
        public void AcceptDefector_UnknownCandidate_ReturnsFailed()
        {
            var system = CreateSystem();
            var result = system.AcceptDefector("unknown");
            Assert.False(result.IsSuccess);
            Assert.Equal("unknown_candidate", result.FailureCode);
        }

        [Fact]
        public void AcceptDefector_Success_RecordsAsylumState()
        {
            var system = CreateSystem();
            var candidate = CreateCandidate("cand_1");
            system.State.candidates.Add(candidate);

            string? acceptedId = null;
            system.OnDefectorAccepted += id => acceptedId = id;

            var result = system.AcceptDefector("cand_1");
            Assert.True(result.IsSuccess);
            Assert.Equal("defector_accepted", candidate.status);
            Assert.Single(system.State.defectorAsylum);
            Assert.Equal("cand_1", system.State.defectorAsylum[0].candidateId);
            Assert.Equal("accepted", system.State.defectorAsylum[0].status);
            Assert.Equal("cand_1", acceptedId);
        }

        // ── ResolveSabotage ──────────────────────────────────────────

        [Fact]
        public void ResolveSabotage_UnknownAgent_ReturnsFailed()
        {
            var system = CreateSystem();
            var result = system.ResolveSabotage("unknown");
            Assert.False(result.IsSuccess);
            Assert.Equal("unknown_agent", result.FailureCode);
        }

        [Fact]
        public void ResolveSabotage_ExposedAgent_ReturnsFailed()
        {
            var system = CreateSystem();
            var agent = CreateAgent("agent_1", "agent_test", exposed: true);
            system.State.undercoverAgents.Add(agent);
            var result = system.ResolveSabotage("agent_1");
            Assert.False(result.IsSuccess);
            Assert.Equal("agent_inactive", result.FailureCode);
        }

        [Fact]
        public void ResolveSabotage_SelectsTargetFromProfile()
        {
            var system = CreateSystem();
            var profile = new InfiltratorProfileDef
            {
                ProfileId = "agent_test",
                SabotageTargets = new System.Collections.Generic.List<string> { "water", "power" }
            };
            system.RegisterProfile(profile);
            var agent = CreateAgent("agent_1", "agent_test");
            system.State.undercoverAgents.Add(agent);

            string? discoveredTarget = null;
            system.OnSabotageDiscovered += (target, agentId, day) => discoveredTarget = target;

            var result = system.ResolveSabotage("agent_1");
            Assert.True(result.IsSuccess);
            Assert.Contains(result.Target, new[] { "water", "power" });
            Assert.NotNull(discoveredTarget);
            Assert.Equal(result.Target, discoveredTarget);
        }

        // ── TickDay ──────────────────────────────────────────────────

        [Fact]
        public void TickDay_AdvancesOncePerDay()
        {
            var system = CreateSystem();
            system.TickDay(1);
            system.TickDay(1); // idempotent
            Assert.Equal(1, system.State.lastProcessedDay);
        }

        // ── Save Round-Trip ──────────────────────────────────────────

        [Fact]
        public void SaveRoundTrip_PreservesCandidatesAndAgents()
        {
            var system = CreateSystem();
            var candidate = CreateCandidate("cand_1", scrutiny: 30f);
            candidate.suspicionFlags.Add("flag_a");
            system.State.candidates.Add(candidate);

            var agent = CreateAgent("surv_1", "agent_test");
            system.State.undercoverAgents.Add(agent);
            system.State.detainees.Add(new DetaineeState { suspectId = "surv_1", interrogationCount = 1 });
            system.TickDay(5);

            var captured = system.CaptureState();
            Assert.Single(captured.candidates);
            Assert.Single(captured.undercoverAgents);
            Assert.Single(captured.detainees);
            Assert.Equal(5, captured.lastProcessedDay);

            var restored = CreateSystem(captured);
            Assert.Single(restored.State.candidates);
            Assert.Equal("cand_1", restored.State.candidates[0].candidateId);
            Assert.Equal(30f, restored.State.candidates[0].scrutinyRating);
            Assert.Single(restored.State.candidates[0].suspicionFlags);
            Assert.Equal("flag_a", restored.State.candidates[0].suspicionFlags[0]);
            Assert.Single(restored.State.undercoverAgents);
            Assert.Equal("surv_1", restored.State.undercoverAgents[0].survivorId);
            Assert.Equal(5, restored.State.lastProcessedDay);
        }

        // ── Determinism ──────────────────────────────────────────────

        [Fact]
        public void Deterministic_SameSeed_SameInterrogationOutcome()
        {
            // Run 1
            var system1 = CreateSystem();
            var profile = new InfiltratorProfileDef
            {
                ProfileId = "agent_test",
                ConfessionThreshold = 0.5f,
                SabotageTargets = new System.Collections.Generic.List<string> { "power" }
            };
            system1.RegisterProfile(profile);
            var agent1 = CreateAgent("survivor_1", "agent_test");
            system1.State.undercoverAgents.Add(agent1);
            system1.State.detainees.Add(new DetaineeState { suspectId = "survivor_1", interrogationCount = 0 });
            var result1 = system1.Interrogate("survivor_1", "officer_1", 1.0f);

            // Run 2 with same seed
            var system2 = CreateSystem();
            system2.RegisterProfile(profile);
            var agent2 = CreateAgent("survivor_1", "agent_test");
            system2.State.undercoverAgents.Add(agent2);
            system2.State.detainees.Add(new DetaineeState { suspectId = "survivor_1", interrogationCount = 0 });
            var result2 = system2.Interrogate("survivor_1", "officer_1", 1.0f);

            Assert.Equal(result1.IsSuccess, result2.IsSuccess);
            Assert.Equal(result1.Outcome, result2.Outcome);
            Assert.Equal(agent1.isExposed, agent2.isExposed);
        }

        [Fact]
        public void Deterministic_SameSeed_SameSabotageTarget()
        {
            var system1 = CreateSystem();
            var profile = new InfiltratorProfileDef
            {
                ProfileId = "agent_test",
                SabotageTargets = new System.Collections.Generic.List<string> { "water", "power", "radio" }
            };
            system1.RegisterProfile(profile);
            var agent1 = CreateAgent("agent_1", "agent_test");
            system1.State.undercoverAgents.Add(agent1);
            var result1 = system1.ResolveSabotage("agent_1");

            var system2 = CreateSystem();
            system2.RegisterProfile(profile);
            var agent2 = CreateAgent("agent_1", "agent_test");
            system2.State.undercoverAgents.Add(agent2);
            var result2 = system2.ResolveSabotage("agent_1");

            Assert.Equal(result1.Target, result2.Target);
        }
    }

    // ── Deterministic mock RNG ─────────────────────────────────────

    internal sealed class MockRng : ISeededRng
    {
        private readonly double _fixedDouble;

        public int Seed { get; }

        public MockRng(double fixedDouble, int seed = 0)
        {
            _fixedDouble = fixedDouble;
            Seed = seed;
        }

        public int Next(int minInclusive, int maxExclusive)
        {
            return minInclusive;
        }

        public float NextFloat()
        {
            return (float)_fixedDouble;
        }

        public double NextDouble()
        {
            return _fixedDouble;
        }
    }

    // ── Minimal test log ────────────────────────────────────────────

    internal sealed class TestLog : ILog
    {
        public void Info(string message) { }
        public void Warn(string message) { }
        public void Error(string message) { }
        public void Debug(string message) { }
    }
}
