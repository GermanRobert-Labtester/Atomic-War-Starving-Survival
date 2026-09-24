// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Plan 176 (Aging & Elderly Survivor System).

using System;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class HostCliAging
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Aging & Elderly Survivor Self-Test (Plan 176) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Catalog loading
                var loadResult = LifeStagesCatalogLoader.Load(dataDir);
                if (loadResult.Success && loadResult.Catalog != null &&
                    loadResult.Catalog.life_stages.Count >= 5 && loadResult.Catalog.milestones.Count >= 6)
                {
                    GD.Print($"[PASS] Check 1: Catalog loaded successfully ({loadResult.Catalog.life_stages.Count} stages, {loadResult.Catalog.milestones.Count} milestones).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 1: Catalog load failed: {string.Join("; ", loadResult.Errors)}");
                }

                // Check 2: Host session instantiation & configuration
                var host = AgingHostSession.Create(dataDir);
                if (host.LifeStages.Count >= 5 && host.DaysPerYear == 30 && host.MinRetirementAgeYears == 65)
                {
                    GD.Print($"[PASS] Check 2: Host session initialized with {host.LifeStages.Count} stages, 30 days/yr, min retirement age 65.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 2: Host session configuration mismatch: stages={host.LifeStages.Count}, days/yr={host.DaysPerYear}, minRetire={host.MinRetirementAgeYears}");
                }

                // Check 3: Canonical life stages verification
                bool hasChild = host.LifeStages.Any(s => s.stage_enum == "Child" && s.min_age == 0 && s.max_age == 17);
                bool hasYoungAdult = host.LifeStages.Any(s => s.stage_enum == "YoungAdult" && s.min_age == 18 && s.max_age == 30);
                bool hasPrime = host.LifeStages.Any(s => s.stage_enum == "Prime" && s.min_age == 31 && s.max_age == 50);
                bool hasMiddleAge = host.LifeStages.Any(s => s.stage_enum == "MiddleAge" && s.min_age == 51 && s.max_age == 65);
                bool hasElderly = host.LifeStages.Any(s => s.stage_enum == "Elderly" && s.min_age == 66 && s.can_retire);

                if (hasChild && hasYoungAdult && hasPrime && hasMiddleAge && hasElderly)
                {
                    GD.Print("[PASS] Check 3: All 5 canonical life stages verified with valid age spans and retirement eligibility.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 3: Life stages configuration verification failed.");
                }

                // Check 4: Survivor registration
                var reg = host.RegisterSurvivor("survivor_young", 25, 1);
                if (reg != null && host.TrackedSurvivorCount == 1 && reg.SurvivorId == "survivor_young" && reg.BaseAgeYears == 25)
                {
                    GD.Print("[PASS] Check 4: Survivor 'survivor_young' registered at age 25 on day 1.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 4: Registration failed. Tracked: {host.TrackedSurvivorCount}");
                }

                // Check 5: Profile evaluation at day 1
                var profileDay1 = host.EvaluateSurvivor("survivor_young", 1);
                if (profileDay1.EffectiveAgeYears == 25 && profileDay1.Stage == SurvivorLifeStage.YoungAdult &&
                    profileDay1.PhysicalLaborMultiplier > 1.0f && !profileDay1.IsRetirementEligible && !profileDay1.IsRetired)
                {
                    GD.Print($"[PASS] Check 5: Profile evaluated at day 1: Age {profileDay1.EffectiveAgeYears} ({profileDay1.Stage}), Labor {profileDay1.PhysicalLaborMultiplier}x.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 5: Profile evaluation mismatch: Age={profileDay1.EffectiveAgeYears}, Stage={profileDay1.Stage}");
                }

                // Check 6: Chronological aging progression (advance 180 days = 6 years at 30 days/year)
                host.AdvanceDay(181); // Day 181 = 180 days tenure / 30 = 6 years added -> 25 + 6 = 31
                var profileDay181 = host.EvaluateSurvivor("survivor_young", 181);
                if (profileDay181.EffectiveAgeYears == 31 && profileDay181.Stage == SurvivorLifeStage.Prime)
                {
                    GD.Print($"[PASS] Check 6: Chronological progression verified: Day 181 -> Age {profileDay181.EffectiveAgeYears}, Stage transitioned to {profileDay181.Stage}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 6: Progression mismatch: Age={profileDay181.EffectiveAgeYears}, Stage={profileDay181.Stage}");
                }

                // Check 7: Birthday / milestone celebration
                var youngRecord = host.System.RegisterSurvivor("survivor_young", 25, 1);
                bool celebratedAge30 = youngRecord.CelebratedMilestoneAges.Contains(30);
                if (celebratedAge30)
                {
                    GD.Print("[PASS] Check 7: Milestone at age 30 ('Thirtieth Year') celebrated and recorded.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 7: Milestone age 30 was not recorded as celebrated.");
                }

                // Check 8: Register elder candidate and evaluate retirement eligibility
                host.RegisterSurvivor("survivor_veteran", 64, 1);
                host.AdvanceDay(181); // 64 + 6 = 70 years old
                var profileVet = host.EvaluateSurvivor("survivor_veteran", 181);
                if (profileVet.EffectiveAgeYears == 70 && profileVet.Stage == SurvivorLifeStage.Elderly && profileVet.IsRetirementEligible && !profileVet.IsRetired)
                {
                    GD.Print($"[PASS] Check 8: Elder survivor evaluated: Age {profileVet.EffectiveAgeYears}, Stage {profileVet.Stage}, Retirement Eligible: {profileVet.IsRetirementEligible}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 8: Elder profile mismatch: Age={profileVet.EffectiveAgeYears}, Stage={profileVet.Stage}, Eligible={profileVet.IsRetirementEligible}");
                }

                // Check 9: Dignified retirement execution
                bool retired = host.RetireSurvivor("survivor_veteran", 181);
                bool isRet = host.IsRetired("survivor_veteran");
                if (retired && isRet && host.RetiredSurvivorCount == 1)
                {
                    GD.Print("[PASS] Check 9: Dignified retirement executed successfully for elder survivor.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 9: Retirement execution failed: retired={retired}, isRetired={isRet}");
                }

                // Check 10: Retired elder mechanics and modifiers
                var retiredProfile = host.EvaluateSurvivor("survivor_veteran", 181);
                if (retiredProfile.IsRetired && retiredProfile.PhysicalLaborMultiplier == SurvivorAgingProgressionEngine.RetiredLightDutyMultiplier &&
                    retiredProfile.MentorshipXpBonus > 0f)
                {
                    GD.Print($"[PASS] Check 10: Retired elder status verified: Light labor multiplier {retiredProfile.PhysicalLaborMultiplier:F2}x, Mentorship bonus +{retiredProfile.MentorshipXpBonus:P0}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 10: Retired elder modifiers mismatch: labor={retiredProfile.PhysicalLaborMultiplier}, mentor={retiredProfile.MentorshipXpBonus}");
                }

                // Check 11: Elder mentorship presence in shelter
                var rosterIds = new[] { "survivor_young", "survivor_veteran" };
                bool hasElder = host.HasLivingElderMentor(181, rosterIds);
                float apprenticeMult = SurvivorAgingProgressionEngine.CalculateApprenticeLearningMultiplier(hasElder);

                if (hasElder && apprenticeMult > 1.0f)
                {
                    GD.Print($"[PASS] Check 11: Elder mentorship active in shelter; apprentice learning multiplier = {apprenticeMult:F2}x.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 11: Elder mentorship detection failed: hasElder={hasElder}, mult={apprenticeMult}");
                }

                // Check 12: Save/restore roundtrip via AgingSaveStore
                var captured = host.CaptureState();
                bool saveOk = AgingSaveStore.TrySave(captured);
                var loaded = AgingSaveStore.TryLoad();

                var restoredHost = AgingHostSession.Create(dataDir);
                restoredHost.RestoreState(loaded);
                var restoredCensus = restoredHost.Census;

                if (saveOk && loaded != null && restoredCensus.TotalTrackedSurvivors == 2 &&
                    restoredCensus.RetiredSurvivors == 1 && restoredCensus.ElderlySurvivors == 1)
                {
                    GD.Print("[PASS] Check 12: AgingSaveStore save/load/restore roundtrip verified.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 12: Save/restore roundtrip failed. Tracked={restoredCensus.TotalTrackedSurvivors}, Retired={restoredCensus.RetiredSurvivors}");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[EXCEPTION] Exception during aging self-test: {ex}");
            }

            GD.Print($"=== Aging & Elderly Survivor Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
