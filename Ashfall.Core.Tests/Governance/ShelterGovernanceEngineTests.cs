// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Governance;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Governance
{
    public sealed class ShelterGovernanceEngineTests
    {
        private static (PolicySystem policySys, LeadershipSystem leaderSys, ShelterGovernanceEngine govEngine) CreateTestSetup()
        {
            var policyCatalog = new PolicyCatalog();
            var serializer = new SystemTextJsonSerializer();

            string catalogJson = @"{
                ""schema_version"": 1,
                ""policies"": [
                    {
                        ""id"": ""policy_curfew"",
                        ""scope"": ""curfew"",
                        ""proposer_rules"": ""open_council"",
                        ""default_option_id"": ""curfew_standard"",
                        ""options"": [
                            { ""id"": ""curfew_none"", ""label"": ""No Curfew"" },
                            { ""id"": ""curfew_standard"", ""label"": ""Standard Curfew"" },
                            { ""id"": ""curfew_lockdown"", ""label"": ""Strict Lockdown"" }
                        ]
                    },
                    {
                        ""id"": ""policy_rations"",
                        ""scope"": ""equal_rations"",
                        ""proposer_rules"": ""open_council"",
                        ""default_option_id"": ""rations_equal"",
                        ""options"": [
                            { ""id"": ""rations_equal"", ""label"": ""Equal Rations"" },
                            { ""id"": ""rations_merit"", ""label"": ""Merit Allotment"" }
                        ]
                    }
                ]
            }";
            policyCatalog.Load(catalogJson, serializer);

            var policySys = new PolicySystem(policyCatalog);
            var leaderSys = new LeadershipSystem();
            leaderSys.GetAliveSurvivorIds = () => new List<string> { "surv_leader", "surv_alice", "surv_bob", "surv_charlie" };

            var govEngine = new ShelterGovernanceEngine(policySys, leaderSys);
            return (policySys, leaderSys, govEngine);
        }

        [Fact]
        public void Defaults_RegisterFourCoreIdeologicalBlocs()
        {
            var (_, _, gov) = CreateTestSetup();

            Assert.Equal(4, gov.Definitions.Count);
            Assert.True(gov.Definitions.ContainsKey("bloc_security_first"));
            Assert.True(gov.Definitions.ContainsKey("bloc_egalitarian_commons"));
            Assert.True(gov.Definitions.ContainsKey("bloc_free_pioneers"));
            Assert.True(gov.Definitions.ContainsKey("bloc_heritage_archive"));

            var secBloc = gov.Definitions["bloc_security_first"];
            Assert.Contains("curfew", secBloc.supported_policy_scopes);
            Assert.Equal("Authoritarian", secBloc.core_ideology);
        }

        [Fact]
        public void EvaluatePolicyConsent_ComputesNetConsentScoreAndSupporters()
        {
            var (_, _, gov) = CreateTestSetup();

            // Scope "curfew" is supported by bloc_security_first and opposed by bloc_free_pioneers
            var eval = gov.EvaluatePolicyConsent("curfew", "curfew_lockdown");

            Assert.Contains("bloc_security_first", eval.supporting_bloc_ids);
            Assert.Contains("bloc_free_pioneers", eval.opposing_bloc_ids);
            // Default baseline: sec = 25, free = 20 -> net score > 0
            Assert.True(eval.has_majority_consent);
            Assert.True(eval.net_consent_score_bp > 0);
        }

        [Fact]
        public void PolicyChange_AutomaticallyAdjustsBlocGrievances()
        {
            var (policySys, _, gov) = CreateTestSetup();

            int initialOpposingGrievance = gov.Blocs["bloc_free_pioneers"].grievance_bp;
            Assert.Equal(0, initialOpposingGrievance);

            // Enact strict curfew
            var result = policySys.SetPolicy("curfew", "curfew_lockdown", "surv_alice", day: 1);
            Assert.True(result.IsSuccess);

            // Opposing bloc accumulated grievance
            int newOpposingGrievance = gov.Blocs["bloc_free_pioneers"].grievance_bp;
            Assert.Equal(1500, newOpposingGrievance);
        }

        [Fact]
        public void SurvivorAffiliation_RecalculatesBlocInfluenceWeights()
        {
            var (_, _, gov) = CreateTestSetup();

            // Assign 3 survivors to free pioneers and 1 to security
            gov.AssignSurvivorToBloc("surv_alice", "bloc_free_pioneers");
            gov.AssignSurvivorToBloc("surv_bob", "bloc_free_pioneers");
            gov.AssignSurvivorToBloc("surv_charlie", "bloc_free_pioneers");
            gov.AssignSurvivorToBloc("surv_leader", "bloc_security_first");

            Assert.Equal("bloc_free_pioneers", gov.GetSurvivorBloc("surv_alice"));
            Assert.Equal("bloc_security_first", gov.GetSurvivorBloc("surv_leader"));

            // Free pioneers have 3/4 members, should gain significantly higher influence weight
            int freeWeight = gov.Blocs["bloc_free_pioneers"].influence_weight_bp;
            int secWeight = gov.Blocs["bloc_security_first"].influence_weight_bp;
            Assert.True(freeWeight > secWeight);
        }

        [Fact]
        public void DisputeLifecycle_TensionAndMediationResolution()
        {
            var (_, _, gov) = CreateTestSetup();

            gov.AssignSurvivorToBloc("surv_alice", "bloc_security_first");
            gov.AssignSurvivorToBloc("surv_bob", "bloc_free_pioneers");

            var dispute = gov.OpenDispute("surv_alice", "surv_bob", GovernanceDisputeType.ResourceTheft, day: 5);
            Assert.False(dispute.is_resolved);
            Assert.Equal("disp_1", dispute.case_id);

            // Inter-bloc dispute created grievance friction for both
            Assert.True(gov.Blocs["bloc_security_first"].grievance_bp > 0);
            Assert.True(gov.Blocs["bloc_free_pioneers"].grievance_bp > 0);

            // Resolve via Mediation
            bool resolved = gov.ResolveDispute(dispute.case_id, GovernanceDisputeResolution.Mediation, day: 6);
            Assert.True(resolved);
            Assert.True(dispute.is_resolved);
            Assert.Equal(GovernanceDisputeResolution.Mediation, dispute.applied_resolution);
            Assert.Equal(6, dispute.resolution_day);

            // Grievances decreased post-mediation
            Assert.Equal(0, gov.Blocs["bloc_security_first"].grievance_bp);
            Assert.Equal(0, gov.Blocs["bloc_free_pioneers"].grievance_bp);
        }

        [Fact]
        public void StabilityRating_ReflectsGrievancesUnresolvedDisputesAndLeadership()
        {
            var (_, leaderSys, gov) = CreateTestSetup();

            // No leader designated yet: -15 penalty
            int baseStability = gov.CalculateStabilityRating();
            Assert.Equal(85, baseStability); // 100 - 15 = 85

            // Designate healthy leader
            leaderSys.DesignateLeader("surv_leader");
            int leaderStability = gov.CalculateStabilityRating();
            Assert.Equal(100, leaderStability); // 100 + 5 capped at 100

            // Open 2 unresolved disputes: -10 penalty from baseline
            gov.OpenDispute("surv_alice", "surv_bob", GovernanceDisputeType.Insult, 1);
            gov.OpenDispute("surv_bob", "surv_charlie", GovernanceDisputeType.WorkRefusal, 1);
            int disputeStability = gov.CalculateStabilityRating();
            Assert.Equal(95, disputeStability); // 100 - 10 (disputes) + 5 (healthy leader) = 95
            Assert.True(disputeStability < leaderStability);

            // Leader under high stress
            leaderSys.OnSurvivorDied("surv_charlie");
            leaderSys.OnSurvivorDied("surv_bob");
            leaderSys.OnSurvivorDied("surv_alice"); // 3 deaths = 75 stress >= 70
            int stressStability = gov.CalculateStabilityRating();
            Assert.True(stressStability < disputeStability);
        }

        [Fact]
        public void Tick_GrievancesDecayOverTime()
        {
            var (_, _, gov) = CreateTestSetup();

            gov.AdjustBlocGrievance("bloc_security_first", 1000);
            Assert.Equal(1000, gov.Blocs["bloc_security_first"].grievance_bp);

            // Advance 24 hours (1 full day): decay_rate is 5% = 500 bp
            gov.Tick(24f);

            int remaining = gov.Blocs["bloc_security_first"].grievance_bp;
            Assert.Equal(500, remaining);

            // Advance another 24 hours
            gov.Tick(24f);
            Assert.Equal(0, gov.Blocs["bloc_security_first"].grievance_bp);
        }

        [Fact]
        public void SaveLoad_RoundTrip_PreservesAllGovernanceState()
        {
            var (policySys, leaderSys, gov1) = CreateTestSetup();

            gov1.AssignSurvivorToBloc("surv_alice", "bloc_security_first");
            gov1.AssignSurvivorToBloc("surv_bob", "bloc_free_pioneers");
            gov1.AdjustBlocGrievance("bloc_heritage_archive", 1200);
            var disp = gov1.OpenDispute("surv_alice", "surv_bob", GovernanceDisputeType.IdeologicalSchism, day: 3);

            var saved = gov1.CaptureState();

            var gov2 = new ShelterGovernanceEngine(policySys, leaderSys);
            gov2.RestoreState(saved);

            Assert.Equal(saved.next_dispute_seq, gov2.CaptureState().next_dispute_seq);
            Assert.Equal("bloc_security_first", gov2.GetSurvivorBloc("surv_alice"));
            Assert.Equal("bloc_free_pioneers", gov2.GetSurvivorBloc("surv_bob"));
            Assert.Equal(1200, gov2.Blocs["bloc_heritage_archive"].grievance_bp);
            Assert.Single(gov2.Disputes);
            Assert.Equal(disp.case_id, gov2.Disputes[0].case_id);
            Assert.Equal(GovernanceDisputeType.IdeologicalSchism, gov2.Disputes[0].dispute_type);
        }
    }
}
