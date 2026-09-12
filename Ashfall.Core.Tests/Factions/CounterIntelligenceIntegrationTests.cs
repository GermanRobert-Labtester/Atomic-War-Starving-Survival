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
    /// <summary>
    /// Integration tests for Task 6: Faction Infiltration, Counter-Intelligence & Defector Vetting.
    /// Simulates end-to-end scenarios: vetting pipeline, interrogation exposure, defector acceptance, sabotage discovery.
    /// </summary>
    public sealed class CounterIntelligenceIntegrationTests
    {
        private static string GetDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var path))
                return path;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data directory not found.");
        }

        [Fact]
        public void FullVettingPipeline_NewArrival_ClearedOrQuarantined()
        {
            var system = new CounterIntelligenceSystem(null, new SeededRng(2001), new TestLog());
            system.LoadCatalog(InfiltratorCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer()));

            // Add a low-scrutiny candidate
            var lowCandidate = new VettingCandidateState
            {
                candidateId = "arrival_low",
                claimedBackground = "laborer",
                scrutinyRating = 5f,
                suspicionFlags = new System.Collections.Generic.List<string>(),
                status = "awaiting_vetting"
            };
            system.State.candidates.Add(lowCandidate);

            var lowResult = system.VetCandidate("arrival_low", "vetter_chief");
            Assert.True(lowResult.IsSuccess);
            Assert.Equal("cleared", lowResult.Status);

            // Add a high-scrutiny candidate with flags
            var highCandidate = new VettingCandidateState
            {
                candidateId = "arrival_high",
                claimedBackground = "medic_aide",
                scrutinyRating = 60f,
                suspicionFlags = new System.Collections.Generic.List<string> { "forged_docs", "suspicious_package" },
                status = "awaiting_vetting"
            };
            system.State.candidates.Add(highCandidate);

            bool flagged = false;
            system.OnCandidateFlagged += (id, reason) => flagged = true;

            var highResult = system.VetCandidate("arrival_high", "vetter_chief");
            Assert.True(highResult.IsSuccess);
            Assert.Equal("quarantined", highResult.Status);
            Assert.True(flagged);
        }

        [Fact]
        public void InterrogationPipeline_AgentExposure_ThenSabotageDiscovery()
        {
            var system = new CounterIntelligenceSystem(null, new SeededRng(2001), new TestLog());
            var profile = new InfiltratorProfileDef
            {
                ProfileId = "agent_cutter",
                ConfessionThreshold = 0.5f,
                SabotageTargets = new System.Collections.Generic.List<string> { "water", "power" }
            };
            system.RegisterProfile(profile);

            // Plant an undercover agent
            var agent = new UndercoverAgentState
            {
                survivorId = "survivor_agent",
                profileId = "agent_cutter",
                sourceFactionId = "faction_the_cutters",
                isExposed = false
            };
            system.State.undercoverAgents.Add(agent);

            // Detain and interrogate
            system.DetainSuspect("survivor_agent");
            var interrogation = system.Interrogate("survivor_agent", "officer_1", 1.0f);
            Assert.True(interrogation.IsSuccess);
            Assert.Equal("full_confession", interrogation.Outcome);
            Assert.True(agent.isExposed);

            // Resolve sabotage (should be blocked now because agent is exposed)
            var sabotage = system.ResolveSabotage("survivor_agent");
            Assert.False(sabotage.IsSuccess);
            Assert.Equal("agent_inactive", sabotage.FailureCode);
        }

        [Fact]
        public void DefectorVetting_Acceptance_AndAsylumRecording()
        {
            var system = new CounterIntelligenceSystem(null, new SeededRng(2001), new TestLog());

            var candidate = new VettingCandidateState
            {
                candidateId = "defector_candidate",
                claimedBackground = "faction_military",
                scrutinyRating = 20f,
                suspicionFlags = new System.Collections.Generic.List<string>(),
                status = "awaiting_vetting"
            };
            system.State.candidates.Add(candidate);

            // Vet and accept
            var vetResult = system.VetCandidate("defector_candidate", "vetter_chief");
            Assert.True(vetResult.IsSuccess);
            Assert.Equal("cleared", vetResult.Status);

            string? acceptedId = null;
            system.OnDefectorAccepted += id => acceptedId = id;

            var acceptResult = system.AcceptDefector("defector_candidate");
            Assert.True(acceptResult.IsSuccess);
            Assert.Equal("defector_accepted", candidate.status);
            Assert.Single(system.State.defectorAsylum);
            Assert.Equal("defector_candidate", acceptedId);
            Assert.Equal("accepted", system.State.defectorAsylum[0].status);
            Assert.Equal("faction_military", system.State.defectorAsylum[0].claimedFactionId);
        }

        [Fact]
        public void MultiDayTick_StatePreserved_NoDoubleTick()
        {
            var system = new CounterIntelligenceSystem(null, new SeededRng(2001), new TestLog());
            system.State.candidates.Add(new VettingCandidateState { candidateId = "cand_1", status = "awaiting_vetting" });

            system.TickDay(1);
            system.TickDay(2);
            system.TickDay(2); // idempotent

            Assert.Equal(2, system.State.lastProcessedDay);
            Assert.Single(system.State.candidates);
        }

        [Fact]
        public void SaveRoundTrip_FullState_PreservedAcrossRestore()
        {
            var system = new CounterIntelligenceSystem(null, new SeededRng(2001), new TestLog());
            system.LoadCatalog(InfiltratorCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer()));

            // Build state
            var candidate = new VettingCandidateState
            {
                candidateId = "cand_1",
                claimedBackground = "laborer",
                scrutinyRating = 25f,
                suspicionFlags = new System.Collections.Generic.List<string> { "flag_x" },
                status = "cleared",
                interviewCount = 2
            };
            system.State.candidates.Add(candidate);

            var agent = new UndercoverAgentState
            {
                survivorId = "surv_1",
                profileId = "agent_cutter_saboteur",
                sourceFactionId = "faction_the_cutters",
                isExposed = true,
                exposureDay = 10
            };
            system.State.undercoverAgents.Add(agent);

            system.State.detainees.Add(new DetaineeState
            {
                suspectId = "surv_1",
                detentionDay = 8,
                interrogationCount = 1,
                confessionOutcome = "full"
            });

            system.State.defectorAsylum.Add(new DefectorAsylumState
            {
                candidateId = "defector_1",
                claimedFactionId = "faction_the_compact",
                status = "accepted",
                decisionDay = 12
            });

            system.TickDay(15);

            // Capture and restore
            var captured = system.CaptureState();
            var restored = new CounterIntelligenceSystem(captured, new SeededRng(2001), new TestLog());
            restored.LoadCatalog(InfiltratorCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer()));

            Assert.Single(restored.State.candidates);
            Assert.Equal("cand_1", restored.State.candidates[0].candidateId);
            Assert.Equal(25f, restored.State.candidates[0].scrutinyRating);
            Assert.Single(restored.State.candidates[0].suspicionFlags);
            Assert.Equal("cleared", restored.State.candidates[0].status);
            Assert.Equal(2, restored.State.candidates[0].interviewCount);

            Assert.Single(restored.State.undercoverAgents);
            Assert.Equal("surv_1", restored.State.undercoverAgents[0].survivorId);
            Assert.True(restored.State.undercoverAgents[0].isExposed);
            Assert.Equal(10, restored.State.undercoverAgents[0].exposureDay);

            Assert.Single(restored.State.detainees);
            Assert.Equal("full", restored.State.detainees[0].confessionOutcome);

            Assert.Single(restored.State.defectorAsylum);
            Assert.Equal("defector_1", restored.State.defectorAsylum[0].candidateId);
            Assert.Equal("accepted", restored.State.defectorAsylum[0].status);
            Assert.Equal(15, restored.State.lastProcessedDay);
        }

        [Fact]
        public void SabotageResolution_DifferentProfiles_DifferentTargets()
        {
            // Profile A targets water/power
            var systemA = new CounterIntelligenceSystem(null, new SeededRng(42), new TestLog());
            var profileA = new InfiltratorProfileDef
            {
                ProfileId = "agent_a",
                SabotageTargets = new System.Collections.Generic.List<string> { "water", "power" }
            };
            systemA.RegisterProfile(profileA);
            var agentA = new UndercoverAgentState { survivorId = "agent_a", profileId = "agent_a" };
            systemA.State.undercoverAgents.Add(agentA);
            var resultA = systemA.ResolveSabotage("agent_a");

            // Profile B targets radio/armory
            var systemB = new CounterIntelligenceSystem(null, new SeededRng(42), new TestLog());
            var profileB = new InfiltratorProfileDef
            {
                ProfileId = "agent_b",
                SabotageTargets = new System.Collections.Generic.List<string> { "radio", "armory" }
            };
            systemB.RegisterProfile(profileB);
            var agentB = new UndercoverAgentState { survivorId = "agent_b", profileId = "agent_b" };
            systemB.State.undercoverAgents.Add(agentB);
            var resultB = systemB.ResolveSabotage("agent_b");

            Assert.True(resultA.IsSuccess);
            Assert.True(resultB.IsSuccess);
            Assert.NotEqual(resultA.Target, resultB.Target);
            Assert.Contains(resultA.Target, new[] { "water", "power" });
            Assert.Contains(resultB.Target, new[] { "radio", "armory" });
        }
    }
}
