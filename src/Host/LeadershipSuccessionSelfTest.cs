// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class LeadershipSuccessionSelfTest
    {
        public static int Run(string dataDir)
        {
            Console.WriteLine("=== [HostCli] Leadership Succession & Policy Self-Test (Plan 208) ===");
            int passed = 0;

            void Check(bool condition, string name)
            {
                if (condition)
                {
                    Console.WriteLine($"[PASS] Check {++passed}: {name}");
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check {passed + 1}: {name}");
                    throw new InvalidOperationException($"LeadershipSuccession self-test assertion failed: {name}");
                }
            }

            try
            {
                var roster = new List<string> { "survivor_alpha", "survivor_bravo", "survivor_charlie", "survivor_delta" };
                var sys = new LeadershipSystem();
                sys.GetAliveSurvivorIds = () => roster;

                // Check 1: Catalog loading from leadership_policies.json
                string catalogPath = Path.Combine(dataDir, "leadership_policies.json");
                if (File.Exists(catalogPath))
                {
                    sys.LoadCatalog(File.ReadAllText(catalogPath));
                }
                Check(sys.Policies.Count == 5,
                    $"Authoritative policies loaded from leadership_policies.json ({sys.Policies.Count}/5 policies).");

                // Check 2: Initial state baseline
                var census0 = sys.GetCensus();
                Check(string.IsNullOrEmpty(census0.CurrentLeaderId) && census0.ActivePolicyId == "policy_meritocratic_appointment",
                    "Initial state baseline clean (no leader, meritocratic appointment active).");

                // Check 3: Designate leader
                string leaderDesignated = string.Empty;
                sys.OnLeaderDesignated += id => leaderDesignated = id;
                bool leaderAssigned = sys.DesignateLeader("survivor_alpha");
                Check(leaderAssigned && sys.CurrentLeaderId == "survivor_alpha" && leaderDesignated == "survivor_alpha",
                    $"survivor_alpha successfully designated as shelter leader.");

                // Check 4: Crisis morale aura applies active policy bonus
                float appliedMorale = 0f;
                sys.ApplyShelterMoraleDelta = delta => appliedMorale = delta;
                sys.OnCrisisEvent();
                Check(appliedMorale >= 10.0f,
                    $"Leader crisis event generated +{appliedMorale:F1} shelter morale modifier.");

                // Check 5: Stress accumulation on casualty
                sys.OnSurvivorDied("survivor_delta");
                float stressAfterDeath = sys.GetLeaderStress("survivor_alpha");
                Check(stressAfterDeath >= LeadershipSystem.LeaderStressPerDeath,
                    $"Leader stress accumulated on survivor death (stress={stressAfterDeath:F1}).");

                // Check 6: Daily stress decay
                sys.Tick(24f);
                float stressAfterDecay = sys.GetLeaderStress("survivor_alpha");
                Check(stressAfterDecay < stressAfterDeath && stressAfterDecay == stressAfterDeath - LeadershipSystem.LeaderStressDecayPerDay,
                    $"Leader stress decayed by daily rate (stress={stressAfterDecay:F1}).");

                // Check 7: Designate successor
                bool successorSet = sys.DesignateSuccessor("survivor_bravo");
                Check(successorSet && sys.DesignatedSuccessorId == "survivor_bravo",
                    $"survivor_bravo designated as official successor.");

                // Check 8: Appoint deputy
                bool deputySet = sys.AppointDeputy("survivor_charlie");
                Check(deputySet && sys.DeputyLeaderId == "survivor_charlie",
                    $"survivor_charlie appointed as deputy leader.");

                // Check 9: Initiate leadership challenge
                var challenge = sys.InitiateChallenge("survivor_charlie", "Dispute over emergency power rationing.");
                Check(challenge != null && challenge.challenge_id != null && !challenge.is_resolved,
                    $"Leadership challenge initiated by deputy (id={challenge?.challenge_id}).");

                // Check 10: Resolve challenge with challenger victory
                bool challengeResolved = sys.ResolveChallenge(challenge!.challenge_id, challengerWon: true);
                Check(challengeResolved && challenge.is_resolved && challenge.challenger_won && sys.CurrentLeaderId == "survivor_charlie",
                    $"Challenge victory transitioned leadership cleanly to challenger ({sys.CurrentLeaderId}).");

                // Check 11: Switch policy and verify census
                bool policySwitched = sys.SetPolicy("policy_democratic_election");
                var census = sys.GetCensus();
                Check(policySwitched && sys.ActivePolicyId == "policy_democratic_election" &&
                      !string.IsNullOrEmpty(census.CurrentLeaderId) && census.CurrentLeaderId == "survivor_charlie" &&
                      census.ActivePolicyId == "policy_democratic_election" && census.TotalChallenges == 1,
                    $"Policy transitioned to democratic_election and census reflects state (Challenges={census.TotalChallenges}).");

                // Check 12: Save and restore state fidelity
                var state = sys.CaptureState();
                var restoredSys = new LeadershipSystem();
                restoredSys.GetAliveSurvivorIds = () => roster;
                if (File.Exists(catalogPath))
                {
                    restoredSys.LoadCatalog(File.ReadAllText(catalogPath));
                }
                restoredSys.RestoreState(state);
                var restoredCensus = restoredSys.GetCensus();
                Check(restoredSys.CurrentLeaderId == "survivor_charlie" &&
                      restoredSys.ActivePolicyId == "policy_democratic_election" &&
                      restoredCensus.TotalChallenges == 1 &&
                      !string.IsNullOrEmpty(restoredCensus.CurrentLeaderId),
                    "Leadership save and restore state verified with full round-trip fidelity.");

                Console.WriteLine($"=== [HostCli] Leadership Succession Self-Test PASSED ({passed}/12 checks) ===");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] LeadershipSuccession self-test threw exception: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return 1;
            }
        }
    }
}
