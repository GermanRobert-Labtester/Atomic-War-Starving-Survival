// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Governance;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Governance
{
    public class Plan43GoverningTogetherTests
    {
        private readonly string _testPoliciesJson = @"{
          ""schema_version"": 1,
          ""policies"": [
            {
              ""id"": ""policy_ration_triage"",
              ""scope"": ""rations"",
              ""proposer_rules"": ""leader_only"",
              ""decision_method"": ""executive"",
              ""default_option_id"": ""ration_standard"",
              ""options"": [
                { ""id"": ""ration_standard"", ""label"": ""Standard"", ""attention_cost"": 1, ""reversal_cost"": 1, ""effect_type"": ""ration_policy"", ""effect_payload"": ""Standard"" },
                { ""id"": ""ration_half"", ""label"": ""Half"", ""attention_cost"": 1, ""reversal_cost"": 1, ""effect_type"": ""ration_policy"", ""effect_payload"": ""Half"" }
              ]
            },
            {
              ""id"": ""policy_curfew"",
              ""scope"": ""curfew"",
              ""proposer_rules"": ""open_council"",
              ""decision_method"": ""consensus"",
              ""default_option_id"": ""curfew_disabled"",
              ""options"": [
                { ""id"": ""curfew_disabled"", ""label"": ""Disabled"", ""attention_cost"": 1, ""reversal_cost"": 1, ""effect_type"": ""schedule_flag"", ""effect_payload"": ""curfew_disabled"" },
                { ""id"": ""curfew_enabled"", ""label"": ""Enabled"", ""attention_cost"": 1, ""reversal_cost"": 1, ""effect_type"": ""schedule_flag"", ""effect_payload"": ""curfew_enabled"" }
              ]
            }
          ]
        }";

        [Fact]
        public void PolicySystem_CatalogAndDefaults_Initialized()
        {
            var catalog = new PolicyCatalog();
            var serializer = new SystemTextJsonSerializer();
            catalog.Load(_testPoliciesJson, serializer);

            var policySys = new PolicySystem(catalog);
            Assert.Equal("ration_standard", policySys.GetActiveOption("rations"));
            Assert.Equal("curfew_disabled", policySys.GetActiveOption("curfew"));
        }

        [Fact]
        public void PolicySystem_SetPolicy_UpdatesStateAndHistory()
        {
            var catalog = new PolicyCatalog();
            var serializer = new SystemTextJsonSerializer();
            catalog.Load(_testPoliciesJson, serializer);

            var policySys = new PolicySystem(catalog);
            string? appliedScope = null;
            string? appliedPayload = null;
            policySys.EffectApplier = (s, opt, effType, payload) =>
            {
                appliedScope = s;
                appliedPayload = payload;
            };

            var result = policySys.SetPolicy("rations", "ration_half", "dweller_leader", 15, "famine crisis");
            Assert.True(result.IsSuccess);
            Assert.Equal("ration_half", policySys.GetActiveOption("rations"));
            Assert.Equal("rations", appliedScope);
            Assert.Equal("Half", appliedPayload);
            Assert.Single(policySys.History);
            Assert.Equal("dweller_leader", policySys.History[0].proposer_id);
            Assert.Equal(15, policySys.History[0].day);

            // Re-enacting same policy is blocked
            var repeatResult = policySys.SetPolicy("rations", "ration_half", "dweller_leader", 16);
            Assert.False(repeatResult.IsSuccess);
            Assert.Equal("already_active", repeatResult.FailureCode);
        }

        [Fact]
        public void PolicySystem_LeaderOnlyPolicy_EnforcedByLeadershipSystem()
        {
            var catalog = new PolicyCatalog();
            var serializer = new SystemTextJsonSerializer();
            catalog.Load(_testPoliciesJson, serializer);

            var leadership = new LeadershipSystem
            {
                GetAliveSurvivorIds = () => new List<string> { "leader_marcus", "worker_elena" }
            };
            leadership.DesignateLeader("leader_marcus");

            var policySys = new PolicySystem(catalog)
            {
                Leadership = leadership
            };

            // Non-leader attempting leader_only policy is blocked
            var failResult = policySys.SetPolicy("rations", "ration_half", "worker_elena", 10);
            Assert.False(failResult.IsSuccess);
            Assert.Equal("leader_only_policy", failResult.FailureCode);

            // Leader can enact it
            var successResult = policySys.SetPolicy("rations", "ration_half", "leader_marcus", 10);
            Assert.True(successResult.IsSuccess);

            // Open council policy can be proposed by worker_elena
            var councilResult = policySys.SetPolicy("curfew", "curfew_enabled", "worker_elena", 10);
            Assert.True(councilResult.IsSuccess);
        }

        [Fact]
        public void PolicySystem_CaptureAndRestore_PreservesActivePoliciesAndHistory()
        {
            var catalog = new PolicyCatalog();
            var serializer = new SystemTextJsonSerializer();
            catalog.Load(_testPoliciesJson, serializer);

            var policySys = new PolicySystem(catalog);
            policySys.SetPolicy("curfew", "curfew_enabled", "proposer_1", 20, "security threat");

            var state = policySys.CaptureState();
            var restored = new PolicySystem(catalog);
            restored.RestoreState(state);

            Assert.Equal("curfew_enabled", restored.GetActiveOption("curfew"));
            Assert.Single(restored.History);
            Assert.Equal("proposer_1", restored.History[0].proposer_id);
        }

        [Fact]
        public void DutyRoster_CrewConsent_RefusalBlocksAssignment()
        {
            var roster = new DutyRosterSystem();
            roster.Unlock(60);
            roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 60);
            roster.WriteName("survivor_viktor", "Viktor", "machinist", DutyRosterIds.ScriptPencil, 60, true);

            // Attach crew consent evaluator: survivor_viktor refuses NightWatch role
            roster.EvaluateCrewConsent = (survivorId, role) =>
            {
                if (survivorId == "survivor_viktor" && role == DutyRosterIds.RoleNightWatch)
                    return CrewConsentVerdict.Refused("severe_fatigue");
                return CrewConsentVerdict.Accepted();
            };

            // Preview consent
            var preview = roster.PreviewCrewConsent("survivor_viktor", DutyRosterIds.RoleNightWatch);
            Assert.NotNull(preview);
            Assert.False(preview.Consented);
            Assert.Equal("severe_fatigue", preview.RefusalReason);

            // Try assign NightWatch: blocked with refusal reason
            var result = roster.AssignWithResult(DutyRosterIds.RoleNightWatch, "survivor_viktor");
            Assert.False(result.IsSuccess);
            Assert.Equal("severe_fatigue", result.FailureCode);
            Assert.Null(roster.GetAssignment(DutyRosterIds.RoleNightWatch));

            // Assign Mess: accepted
            var messResult = roster.AssignWithResult(DutyRosterIds.RoleMess, "survivor_viktor");
            Assert.True(messResult.IsSuccess);
            Assert.Equal("survivor_viktor", roster.GetAssignment(DutyRosterIds.RoleMess));
        }

        [Fact]
        public void DutyRoster_AutoAssign_ExcludesRefusingSurvivors()
        {
            var roster = new DutyRosterSystem();
            roster.Unlock(60);
            roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 60);
            roster.WriteName("survivor_refuser", "Refuser", "miner", DutyRosterIds.ScriptPencil, 60, true);

            roster.EvaluateCrewConsent = (survivorId, role) =>
            {
                if (survivorId == "survivor_refuser")
                    return CrewConsentVerdict.Refused("strike");
                return CrewConsentVerdict.Accepted();
            };

            // AutoAssign runs with eligible pool containing only refuser
            int assigned = roster.AutoAssignDefaults(61);
            Assert.Equal(0, assigned);
            Assert.Null(roster.GetAssignment(DutyRosterIds.RoleNightWatch));
            Assert.Null(roster.GetAssignment(DutyRosterIds.RoleMess));
        }
    }
}
