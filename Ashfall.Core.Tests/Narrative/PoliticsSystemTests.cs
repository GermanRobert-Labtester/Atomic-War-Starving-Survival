using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    public class PoliticsSystemTests
    {
        private static string LoadPoliticalPoliciesCatalogJson()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data", "political_policies.json");
            if (!File.Exists(path))
            {
                var dir = new DirectoryInfo(AppContext.BaseDirectory);
                while (dir != null)
                {
                    string candidate = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data", "political_policies.json");
                    if (File.Exists(candidate)) return File.ReadAllText(candidate);
                    dir = dir.Parent;
                }
                throw new FileNotFoundException("Could not find political_policies.json");
            }
            return File.ReadAllText(path);
        }

        [Fact]
        public void LoadCatalog_ParsesPoliciesCorrectly()
        {
            var system = new PoliticsSystem();
            system.LoadCatalog(LoadPoliticalPoliciesCatalogJson(), new SystemTextJsonSerializer());

            var rationing = system.GetPolicy("policy_emergency_rationing");
            Assert.NotNull(rationing);
            Assert.Equal("Economic", rationing.category);
            Assert.Contains("quartermaster", rationing.supporter_tags);

            var martial = system.GetPolicy("policy_martial_law");
            Assert.NotNull(martial);
            Assert.True(martial.is_emergency);
            Assert.True(martial.legitimacy_impact < 0f);
        }

        [Fact]
        public void EnactAndRepealPolicy_UpdatesLegitimacyAndRegistry()
        {
            var system = new PoliticsSystem();
            system.LoadCatalog(LoadPoliticalPoliciesCatalogJson(), new SystemTextJsonSerializer());

            float initialLegitimacy = system.Legitimacy;
            bool enacted = system.EnactPolicy("policy_open_gate_refugees", out string failure);

            Assert.True(enacted, failure);
            Assert.Contains("policy_open_gate_refugees", system.ActivePolicies);
            Assert.True(system.Legitimacy > initialLegitimacy);

            bool repealed = system.RepealPolicy("policy_open_gate_refugees");
            Assert.True(repealed);
            Assert.DoesNotContain("policy_open_gate_refugees", system.ActivePolicies);
        }

        [Fact]
        public void EmergencyPolicy_BlockedWithoutMartialLaw()
        {
            var system = new PoliticsSystem();
            system.LoadCatalog(LoadPoliticalPoliciesCatalogJson(), new SystemTextJsonSerializer());

            bool enacted = system.EnactPolicy("policy_martial_law", out string failure);
            Assert.False(enacted);
            Assert.Contains("martial law", failure);

            system.DeclareMartialLaw();
            bool enactedUnderMartial = system.EnactPolicy("policy_martial_law", out string failure2);
            Assert.True(enactedUnderMartial, failure2);
        }

        [Fact]
        public void CalculateVoterScore_ConsidersEnvironmentAndTraits()
        {
            var system = new PoliticsSystem();
            system.LoadCatalog(LoadPoliticalPoliciesCatalogJson(), new SystemTextJsonSerializer());
            system.SetInitialLeader("leader_a");

            var voterTraits = new List<string> { "pragmatist", "artisan" };
            var candATraits = new List<string> { "pragmatist" };
            var candBTraits = new List<string> { "hedonist" };

            // Good conditions favor incumbent
            float scoreA = system.CalculateVoterScore("v1", "leader_a", voterTraits, candATraits, 0.9f, 0.9f);
            float scoreB = system.CalculateVoterScore("v1", "challenger_b", voterTraits, candBTraits, 0.9f, 0.9f);

            Assert.True(scoreA > scoreB, "Incumbent in a prosperous shelter with trait match should outscore challenger");
        }

        [Fact]
        public void HoldElection_ResolvesWinnerDeterministically()
        {
            var system = new PoliticsSystem();
            system.LoadCatalog(LoadPoliticalPoliciesCatalogJson(), new SystemTextJsonSerializer());

            var candidates = new List<string> { "cand_alice", "cand_bob" };
            var voters = new List<string> { "v1", "v2", "v3", "v4", "v5" };

            var traitsMap = new Dictionary<string, List<string>>
            {
                ["cand_alice"] = new List<string> { "humanitarian" },
                ["cand_bob"] = new List<string> { "authoritarian" },
                ["v1"] = new List<string> { "humanitarian" },
                ["v2"] = new List<string> { "humanitarian" },
                ["v3"] = new List<string> { "authoritarian" },
                ["v4"] = new List<string> { "humanitarian" },
                ["v5"] = new List<string> { "neutral" }
            };

            var rng = new SeededRng(42);
            var result = system.HoldElection(
                currentDay: 30,
                candidates,
                voters,
                id => traitsMap.GetValueOrDefault(id, new List<string>()),
                foodSat: 0.7f,
                secSat: 0.7f,
                rng);

            Assert.Equal(5, result.totalTurnout);
            Assert.Equal("cand_alice", result.electedLeaderId);
            Assert.Equal("cand_alice", system.CurrentLeaderId);
            Assert.Equal(30, system.DaysUntilElection); // Reset
            Assert.Equal(1, system.TotalElections);
        }

        [Fact]
        public void ApprovalBreakdown_ReflectsCrueltyAndNutrition()
        {
            var system = new PoliticsSystem();
            system.LoadCatalog(LoadPoliticalPoliciesCatalogJson(), new SystemTextJsonSerializer());

            var good = system.CalculateApprovalBreakdown(0.9f, 0.9f, 0f);
            var terrible = system.CalculateApprovalBreakdown(0.1f, 0.2f, 80f);

            Assert.True(good.totalApproval > 70f);
            Assert.True(terrible.totalApproval < 30f);
            Assert.True(terrible.crueltyPenalty < 0f);
        }

        [Fact]
        public void MartialLawAndCoup_Lifecycle()
        {
            var system = new PoliticsSystem();
            system.LoadCatalog(LoadPoliticalPoliciesCatalogJson(), new SystemTextJsonSerializer());

            system.DeclareMartialLaw();
            Assert.True(system.IsMartialLaw);
            Assert.Equal("MartialLaw", system.GovernanceMode);

            float coupRisk = system.CalculateCoupRisk(0.2f, 50f, 2);
            Assert.True(coupRisk > 0.40f);

            var rng = new SeededRng(123);
            bool loyalistWin = system.ResolveCoup(true, rng);
            Assert.True(loyalistWin);
            Assert.True(system.Legitimacy > 20f);

            system.LiftMartialLaw();
            Assert.False(system.IsMartialLaw);
            Assert.Equal("Democratic", system.GovernanceMode);
        }

        [Fact]
        public void PoliticsState_RoundTripPreservation()
        {
            var system = new PoliticsSystem();
            system.LoadCatalog(LoadPoliticalPoliciesCatalogJson(), new SystemTextJsonSerializer());
            system.SetInitialLeader("pres_rt");
            system.EnactPolicy("policy_militia_conscription", out _);

            var state = system.CaptureState();
            Assert.Equal("pres_rt", state.currentLeaderId);
            Assert.Single(state.activePolicies);

            var restored = new PoliticsSystem();
            restored.RestoreState(state);

            Assert.Equal("pres_rt", restored.CurrentLeaderId);
            Assert.Single(restored.ActivePolicies);
        }
    }
}
