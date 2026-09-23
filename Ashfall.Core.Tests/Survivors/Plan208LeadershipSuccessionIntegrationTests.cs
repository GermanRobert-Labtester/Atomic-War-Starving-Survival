// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan208LeadershipSuccessionIntegrationTests
    {
        private static readonly List<string> Roster = new List<string>
        {
            "leader_alpha",
            "deputy_bravo",
            "successor_charlie",
            "challenger_delta",
            "survivor_echo"
        };

        private static LeadershipSystem CreateSystem()
        {
            var sys = new LeadershipSystem();
            sys.GetAliveSurvivorIds = () => Roster;
            return sys;
        }

        [Fact]
        public void LoadCatalog_LoadsAllPolicies_FromValidJson()
        {
            var sys = CreateSystem();
            string path = Path.Combine("..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "leadership_policies.json");
            if (!File.Exists(path))
            {
                path = Path.Combine("Assets", "StreamingAssets", "Data", "leadership_policies.json");
            }
            Assert.True(File.Exists(path), $"Catalog file not found at {path}");

            string json = File.ReadAllText(path);
            sys.LoadCatalog(json);

            Assert.Equal(5, sys.Policies.Count);
            var meritocratic = sys.GetPolicy("policy_meritocratic_appointment");
            Assert.NotNull(meritocratic);
            Assert.Equal("Meritocratic Appointment", meritocratic.Name);
            Assert.Equal("designated_successor", meritocratic.SuccessionMode);
            Assert.True(meritocratic.DeputyPowersDelegated);
            Assert.Equal(10.0f, meritocratic.CrisisMoraleModifier);

            var martial = sys.GetPolicy("policy_martial_challenge");
            Assert.NotNull(martial);
            Assert.Equal("martial_challenge", martial.SuccessionMode);
            Assert.False(martial.DeputyPowersDelegated);
        }

        [Fact]
        public void SetPolicy_ChangesActivePolicy_AndFiresEvent()
        {
            var sys = CreateSystem();
            sys.LoadCatalog(@"{
                ""schema_version"": 1,
                ""policies"": [
                    { ""id"": ""policy_meritocratic_appointment"", ""name"": ""Merit"", ""crisis_morale_modifier"": 10.0 },
                    { ""id"": ""policy_democratic_election"", ""name"": ""Democratic"", ""crisis_morale_modifier"": 8.0 }
                ]
            }");

            string? changedPolicy = null;
            sys.OnPolicyChanged += p => changedPolicy = p;

            bool success = sys.SetPolicy("policy_democratic_election");

            Assert.True(success);
            Assert.Equal("policy_democratic_election", sys.ActivePolicyId);
            Assert.Equal("policy_democratic_election", changedPolicy);
            Assert.Equal("Democratic", sys.ActivePolicy?.Name);

            // Setting invalid policy fails when catalog is populated
            Assert.False(sys.SetPolicy("nonexistent_policy"));
        }

        [Fact]
        public void OnCrisisEvent_UsesActivePolicyMoraleModifier()
        {
            var sys = CreateSystem();
            sys.LoadCatalog(@"{
                ""schema_version"": 1,
                ""policies"": [
                    { ""id"": ""policy_elder_council"", ""name"": ""Elder Council"", ""crisis_morale_modifier"": 15.0 }
                ]
            }");
            sys.SetPolicy("policy_elder_council");
            sys.DesignateLeader("leader_alpha");

            float appliedMorale = 0f;
            sys.ApplyShelterMoraleDelta = delta => appliedMorale = delta;

            sys.OnCrisisEvent();

            Assert.Equal(15.0f, appliedMorale);
        }

        [Fact]
        public void DeputySuccession_WhenSuccessorUnsetOrDead()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("leader_alpha");
            sys.AppointDeputy("deputy_bravo");

            string? deadReported = null;
            string? newReported = null;
            sys.OnSuccessionTriggered += (d, n) =>
            {
                deadReported = d;
                newReported = n;
            };

            // Leader dies; no designated successor was appointed
            sys.OnSurvivorDied("leader_alpha");

            Assert.Equal("leader_alpha", deadReported);
            Assert.Equal("deputy_bravo", newReported);
            Assert.Equal("deputy_bravo", sys.CurrentLeaderId);
            Assert.True(sys.IsDesignatedLeader("deputy_bravo"));
            Assert.True(string.IsNullOrEmpty(sys.DeputyLeaderId)); // Deputy cleared when elevated
        }

        [Fact]
        public void InitiateAndResolveChallenge_SwapsLeadershipAndCancelsConflictingChallenges()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("leader_alpha");

            var ch1 = sys.InitiateChallenge("challenger_delta", "Incompetence during radioactive storm");
            var ch2 = sys.InitiateChallenge("survivor_echo", "Ration distribution dispute");

            Assert.NotNull(ch1);
            Assert.NotNull(ch2);
            Assert.Equal(2, sys.Challenges.Count);

            bool resolved = sys.ResolveChallenge(ch1.challenge_id, challengerWon: true);

            Assert.True(resolved);
            Assert.True(ch1.is_resolved);
            Assert.True(ch1.challenger_won);
            Assert.Equal("challenger_delta", sys.CurrentLeaderId);

            // Conflicting challenge against previous leader is automatically resolved as lost
            Assert.True(ch2.is_resolved);
            Assert.False(ch2.challenger_won);
        }

        [Fact]
        public void ElectLeader_SetsNewLeader_AndClearsDeputyIfElected()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("leader_alpha");
            sys.AppointDeputy("deputy_bravo");

            string? deadOrPrior = null;
            string? elected = null;
            sys.OnSuccessionTriggered += (prior, next) =>
            {
                deadOrPrior = prior;
                elected = next;
            };

            bool success = sys.ElectLeader("deputy_bravo", "Shelter-wide democratic referendum");

            Assert.True(success);
            Assert.Equal("deputy_bravo", sys.CurrentLeaderId);
            Assert.True(sys.IsDesignatedLeader("deputy_bravo"));
            Assert.True(string.IsNullOrEmpty(sys.DeputyLeaderId));
            Assert.Equal("leader_alpha", deadOrPrior);
            Assert.Equal("deputy_bravo", elected);
        }

        [Fact]
        public void CaptureAndRestoreState_RoundTripsAllSuccessionAndPolicyData()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("leader_alpha");
            sys.DesignateSuccessor("successor_charlie");
            sys.AppointDeputy("deputy_bravo");
            sys.SetPolicy("policy_martial_challenge");

            var ch = sys.InitiateChallenge("challenger_delta", "Trial of command");
            Assert.NotNull(ch);

            var save = sys.CaptureState();
            Assert.Equal("leader_alpha", save.current_leader_id);
            Assert.Equal("successor_charlie", save.designated_successor_id);
            Assert.Equal("deputy_bravo", save.deputy_leader_id);
            Assert.Equal("policy_martial_challenge", save.active_policy_id);
            Assert.Single(save.challenges);

            var restored = CreateSystem();
            restored.RestoreState(save);

            Assert.Equal("leader_alpha", restored.CurrentLeaderId);
            Assert.Equal("successor_charlie", restored.DesignatedSuccessorId);
            Assert.Equal("deputy_bravo", restored.DeputyLeaderId);
            Assert.Equal("policy_martial_challenge", restored.ActivePolicyId);
            Assert.Single(restored.Challenges);
            Assert.Equal("challenger_delta", restored.Challenges[0].challenger_id);
        }
    }
}
