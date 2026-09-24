// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class ChildDevelopmentSelfTest
    {
        public static int Run(string dataDir)
        {
            Console.WriteLine("=== [HostCli] Child Development Stages System Self-Test (Plan 183) ===");
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
                    throw new InvalidOperationException($"ChildDevelopment self-test assertion failed: {name}");
                }
            }

            try
            {
                // Check 1: Catalog loads traits from development_traits.json
                var session = ChildDevelopmentHostSession.Create(dataDir);
                Check(session.System.GetAllTraits().Count >= 6,
                    "Authoritative traits loaded from development_traits.json (>= 6 traits).");

                // Check 2: Initial state baseline
                var census0 = session.GetCensus();
                Check(census0.TotalChildren == 0 && census0.TotalMilestones == 0,
                    "Initial state baseline clean (0 children, 0 milestones).");

                // Check 3: Canonical age-to-stage thresholds
                bool stageThresholdsValid =
                    ChildDevelopmentSystem.ResolveStage(0) == DevelopmentStage.Infant &&
                    ChildDevelopmentSystem.ResolveStage(59) == DevelopmentStage.Infant &&
                    ChildDevelopmentSystem.ResolveStage(60) == DevelopmentStage.Toddler &&
                    ChildDevelopmentSystem.ResolveStage(179) == DevelopmentStage.Toddler &&
                    ChildDevelopmentSystem.ResolveStage(180) == DevelopmentStage.Child &&
                    ChildDevelopmentSystem.ResolveStage(499) == DevelopmentStage.Child &&
                    ChildDevelopmentSystem.ResolveStage(500) == DevelopmentStage.Adolescent &&
                    ChildDevelopmentSystem.ResolveStage(719) == DevelopmentStage.Adolescent &&
                    ChildDevelopmentSystem.ResolveStage(720) == DevelopmentStage.YoungAdult;
                Check(stageThresholdsValid, "Canonical age thresholds resolve all 5 stages correctly.");

                // Check 4: Register child profile at Infant stage
                var child1 = session.RegisterChild("child_leo", "Leo", birthDay: 1, new[] { "parent_alice", "parent_bob" });
                Check(child1 != null && child1.Stage == DevelopmentStage.Infant && child1.ParentIds.Count == 2,
                    $"Child profile registered at Infant stage (id={child1?.ChildId}, parents={child1?.ParentIds.Count}).");

                // Check 5: Canonical projection from Generational child
                var genChild = new ChildDevelopment
                {
                    survivorId = "child_canonical",
                    birthDay = 1,
                    developmentPhase = DevelopmentPhase.YoungChild,
                    educationXp = 45f,
                    assignedGuardianId = "guardian_marcus"
                };
                var projected = ChildDevelopmentSystem.ProjectCanonicalChild(genChild, currentDay: 100);
                Check(projected != null && projected.Stage == DevelopmentStage.Toddler && projected.AssignedCaregiverId == "guardian_marcus" && Math.Abs(projected.EducationScore - 45f) < 0.01f,
                    "Canonical ChildDevelopment cleanly projected into detached ChildProfile view.");

                // Check 6: Day tick advances stage
                DevelopmentStage observedStage = DevelopmentStage.Infant;
                session.System.OnStageChanged += (profile, stage) => observedStage = stage;
                session.TickDay(currentDay: 65); // age 64 -> Toddler
                Check(child1!.Stage == DevelopmentStage.Toddler && observedStage == DevelopmentStage.Toddler,
                    $"Day progression transitioned child to Toddler stage (stage={child1.Stage}).");

                // Check 7: Milestone event created and recorded
                Check(child1.Milestones.Contains("Milestone_Toddler") && session.GetCensus().TotalMilestones >= 1,
                    $"Milestone event recorded in history (milestones count={session.GetCensus().TotalMilestones}).");

                // Check 8: Caregiver assignment
                bool caregiverAssigned = session.AssignCaregiver("child_leo", "caregiver_sarah");
                Check(caregiverAssigned && child1.AssignedCaregiverId == "caregiver_sarah",
                    "Caregiver caregiver_sarah assigned to child profile.");

                // Check 9: Education recording
                bool educated = session.RecordEducation("child_leo", 25f);
                Check(educated && child1.EducationScore >= 25f,
                    $"Education recorded successfully (score={child1.EducationScore:F1}).");

                // Check 10: Chore work capacity scaling
                float infantCapacity = session.GetChoreWorkCapacity("child_leo"); // Currently Toddler -> 0.0
                session.TickDay(currentDay: 200); // age 199 -> Child stage (capacity > 0.35)
                float childCapacity = session.GetChoreWorkCapacity("child_leo");
                Check(infantCapacity == 0.0f && childCapacity >= 0.35f,
                    $"Chore work capacity scales with development stage (toddler=0, child={childCapacity:F2}).");

                // Check 11: Census verification
                var census = session.GetCensus();
                Check(census.TotalChildren == 1 && census.ChildCount == 1 && census.TotalMilestones >= 2,
                    $"Census matches state (Total={census.TotalChildren}, ChildStage={census.ChildCount}, Milestones={census.TotalMilestones}).");

                // Check 12: Save and restore state fidelity
                var state = session.System.CaptureState();
                var restoredSession = ChildDevelopmentHostSession.Create(dataDir, state);
                var restoredCensus = restoredSession.GetCensus();
                var restoredChild = restoredSession.GetChild("child_leo");
                Check(restoredCensus.TotalChildren == 1 &&
                      restoredChild != null &&
                      restoredChild.Stage == DevelopmentStage.Child &&
                      restoredChild.AssignedCaregiverId == "caregiver_sarah" &&
                      restoredChild.EducationScore == child1.EducationScore,
                    "Save and restore state verified with full round-trip fidelity.");

                Console.WriteLine($"=== [HostCli] Child Development Self-Test PASSED ({passed}/12 checks) ===");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] ChildDevelopment self-test threw exception: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return 1;
            }
        }
    }
}
