// SPDX-License-Identifier: MIT
// ASHFALL C1 Plan 19B — Cohort, Memorials, Generational State & Continuity Tests (INV-19.5).

using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Endgame;
using Ashfall.Core.Memorial;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Endgame
{
    public sealed class Plan19CohortContinuityTests
    {
        // ── 19B.2 / 19B.3 Child Ration Calculations ───────────────────────────

        [Fact]
        public void ChildRations_ZeroChildren_RequiresZeroRations()
        {
            var cohort = new CohortSystem();
            Assert.Equal(0, cohort.CalculateChildFoodUnits(StartingLevel.RationPolicy.Standard));
            Assert.Equal(0, cohort.CalculateChildFoodUnits(StartingLevel.RationPolicy.Half));
        }

        [Fact]
        public void ChildRations_SingleChild_StandardPolicy_CalculatesDeterministicCeiling()
        {
            var cohort = new CohortSystem();
            cohort.BookChild("child_1", new List<string>(), "medium", 1);

            // 1 child * 3 base * 0.5 fraction = 1.5 -> ceil = 2 units
            int units = cohort.CalculateChildFoodUnits(StartingLevel.RationPolicy.Standard);
            Assert.Equal(2, units);
        }

        [Fact]
        public void ChildRations_SingleChild_HalfPolicy_CalculatesDeterministicCeiling()
        {
            var cohort = new CohortSystem();
            cohort.BookChild("child_1", new List<string>(), "medium", 1);

            // 1 child * 2 base * 0.5 fraction = 1.0 -> ceil = 1 unit
            int units = cohort.CalculateChildFoodUnits(StartingLevel.RationPolicy.Half);
            Assert.Equal(1, units);
        }

        [Fact]
        public void ChildRations_NChildren_RoundsCorrectly()
        {
            var cohort = new CohortSystem();
            cohort.BookChild("c1", new List<string>(), "low", 1);
            cohort.BookChild("c2", new List<string>(), "medium", 1);
            cohort.BookChild("c3", new List<string>(), "high", 1);

            // 3 children * 3 base * 0.5 fraction = 4.5 -> ceil = 5 units
            Assert.Equal(5, cohort.CalculateChildFoodUnits(StartingLevel.RationPolicy.Standard));

            // 3 children * 2 base * 0.5 fraction = 3.0 -> ceil = 3 units
            Assert.Equal(3, cohort.CalculateChildFoodUnits(StartingLevel.RationPolicy.Half));
        }

        [Fact]
        public void ChildRations_CustomTuningFraction_AppliesCorrectly()
        {
            var cohort = new CohortSystem();
            cohort.Tuning = new CohortTuning
            {
                child_ration_fraction = 0.25f,
                schooling_age_days = 30,
                maturation_age_days = 180,
                max_active_apprentices = 3
            };
            cohort.BookChild("c1", new List<string>(), "medium", 1);
            cohort.BookChild("c2", new List<string>(), "medium", 1);

            // 2 children * 3 base * 0.25 fraction = 1.5 -> ceil = 2 units
            Assert.Equal(2, cohort.CalculateChildFoodUnits(StartingLevel.RationPolicy.Standard));
        }

        // ── 19B.4 Schooling / Apprenticeship Eligibility & Capacity ───────────

        [Fact]
        public void SchoolingEligibility_UnderAgeChild_NotEligible()
        {
            var cohort = new CohortSystem();
            cohort.BookChild("infant_1", new List<string>(), "low", birthDay: 10);

            // Current day 25 -> Age 15 days < schooling_age_days (30)
            Assert.False(cohort.IsSchoolEligible("infant_1", currentDay: 25));
        }

        [Fact]
        public void SchoolingEligibility_SchoolAgeChild_Eligible()
        {
            var cohort = new CohortSystem();
            cohort.BookChild("youth_1", new List<string>(), "medium", birthDay: 10);

            // Current day 45 -> Age 35 days >= schooling_age_days (30)
            Assert.True(cohort.IsSchoolEligible("youth_1", currentDay: 45));
        }

        [Fact]
        public void SchoolingEligibility_MaturedChild_NoLongerEligible()
        {
            var cohort = new CohortSystem();
            cohort.BookChild("adult_youth", new List<string>(), "high", birthDay: 1);

            Assert.True(cohort.IsSchoolEligible("adult_youth", currentDay: 100));
            Assert.True(cohort.TryMaturation("adult_youth", day: 180));

            // Matured dwellers graduate from schooling eligibility
            Assert.False(cohort.IsSchoolEligible("adult_youth", currentDay: 181));
        }

        [Fact]
        public void SchoolingEligibility_DeceasedChild_NotEligible()
        {
            var cohort = new CohortSystem();
            cohort.BookChild("lost_child", new List<string>(), "medium", birthDay: 1);
            cohort.MarkChildLost("lost_child", day: 50, cause: "fever");

            Assert.False(cohort.IsSchoolEligible("lost_child", currentDay: 60));
        }

        [Fact]
        public void Apprenticeship_RejectsIneligibleChild_ViaDelegate()
        {
            var skills = new SkillProgressionSystem();
            skills.RecordAction(new SimpleSkillActor("mentor_1"), "skill_medicine", 60f, 1);
            var roster = new DutyRosterSystem();
            var relations = new SurvivorRelationsSystem(new SeededRng(42));
            var apprenticeSys = new ApprenticeshipSystem(new SeededRng(42), skills, roster, relations);

            var cohort = new CohortSystem();
            cohort.BookChild("infant_1", new List<string>(), "low", birthDay: 20);

            // Wire eligibility delegate to CohortSystem on day 30 (infant age = 10 < 30)
            apprenticeSys.IsApprenticeEligible = id => cohort.IsSchoolEligible(id, currentDay: 30);

            var result = apprenticeSys.StartPair("mentor_1", "infant_1", "skill_medicine");
            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.Equal("apprentice_ineligible", result.FailureCode);
        }

        [Fact]
        public void Apprenticeship_AcceptsEligibleSchoolAgeChild()
        {
            var skills = new SkillProgressionSystem();
            skills.RecordAction(new SimpleSkillActor("mentor_1"), "skill_medicine", 60f, 1);
            var roster = new DutyRosterSystem();
            var relations = new SurvivorRelationsSystem(new SeededRng(42));
            var apprenticeSys = new ApprenticeshipSystem(new SeededRng(42), skills, roster, relations);

            var cohort = new CohortSystem();
            cohort.BookChild("school_child", new List<string>(), "medium", birthDay: 1);

            // Wire eligibility delegate on day 40 (child age = 39 >= 30)
            apprenticeSys.IsApprenticeEligible = id => cohort.IsSchoolEligible(id, currentDay: 40);

            var result = apprenticeSys.StartPair("mentor_1", "school_child", "skill_medicine");
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Single(apprenticeSys.State.activePairs);
        }

        [Fact]
        public void Apprenticeship_CapacityCap_RejectsFourthPair()
        {
            var skills = new SkillProgressionSystem();
            skills.RecordAction(new SimpleSkillActor("m1"), "skill_medicine", 60f, 1);
            skills.RecordAction(new SimpleSkillActor("m2"), "skill_medicine", 60f, 1);
            skills.RecordAction(new SimpleSkillActor("m3"), "skill_medicine", 60f, 1);
            skills.RecordAction(new SimpleSkillActor("m4"), "skill_medicine", 60f, 1);

            var roster = new DutyRosterSystem();
            var relations = new SurvivorRelationsSystem(new SeededRng(42));
            var apprenticeSys = new ApprenticeshipSystem(new SeededRng(42), skills, roster, relations);
            Assert.Equal(3, apprenticeSys.MaxConcurrentPairs);

            Assert.Equal(ActionResult.StatusKind.Success, apprenticeSys.StartPair("m1", "a1", "skill_medicine").Status);
            Assert.Equal(ActionResult.StatusKind.Success, apprenticeSys.StartPair("m2", "a2", "skill_medicine").Status);
            Assert.Equal(ActionResult.StatusKind.Success, apprenticeSys.StartPair("m3", "a3", "skill_medicine").Status);

            // 4th pair exceeds MaxConcurrentPairs = 3
            var r4 = apprenticeSys.StartPair("m4", "a4", "skill_medicine");
            Assert.Equal(ActionResult.StatusKind.Blocked, r4.Status);
            Assert.Equal("capacity_full", r4.FailureCode);
        }

        // ── 19B.5 Duty Roster Maturity Gate ───────────────────────────────────

        [Fact]
        public void DutyRoster_UnderAgeUnmaturedChild_RejectedFromAssignment()
        {
            var roster = new DutyRosterSystem();
            roster.Unlock(0);

            var cohort = new CohortSystem();
            cohort.BookChild("child_unmatured", new List<string>(), "low", birthDay: 1);

            // Wire eligibility filter to CohortSystem
            roster.IsCandidateEligible = survivorId => cohort.IsWorkEligible(survivorId);

            // Attempting to write into chart and assign should be blocked
            bool writeOk = roster.WriteName("child_unmatured", "Child Unmatured", "labourer", DutyRosterIds.ScriptPencil, 1, true);
            Assert.False(writeOk);

            bool assignOk = roster.Assign(DutyRosterIds.AssignmentRoles[0], "child_unmatured");
            Assert.False(assignOk);
        }

        [Fact]
        public void DutyRoster_MaturedChild_AcceptedIntoAssignment()
        {
            var roster = new DutyRosterSystem();
            roster.Unlock(0);

            var cohort = new CohortSystem();
            cohort.BookChild("child_matured", new List<string>(), "high", birthDay: 1);
            Assert.True(cohort.TryMaturation("child_matured", day: 180));

            // Wire eligibility filter
            roster.IsCandidateEligible = survivorId => cohort.IsWorkEligible(survivorId);

            // Matured cohort youth is accepted
            bool writeOk = roster.WriteName("child_matured", "Youth Graduate", "labourer", DutyRosterIds.ScriptPencil, 181, true);
            Assert.True(writeOk);

            bool assignOk = roster.Assign(DutyRosterIds.AssignmentRoles[0], "child_matured");
            Assert.True(assignOk);
        }

        [Fact]
        public void DutyRoster_NonCohortAdult_AcceptedIntoAssignment()
        {
            var roster = new DutyRosterSystem();
            roster.Unlock(0);

            var cohort = new CohortSystem();
            // Not registered in cohort -> IsWorkEligible is true (standard adult)
            roster.IsCandidateEligible = survivorId => cohort.IsWorkEligible(survivorId);

            bool writeOk = roster.WriteName("adult_dweller", "Adult Dweller", "medic", DutyRosterIds.ScriptPencil, 1, true);
            Assert.True(writeOk);

            bool assignOk = roster.Assign(DutyRosterIds.AssignmentRoles[0], "adult_dweller");
            Assert.True(assignOk);
        }

        // ── 19B.7 / 19B.8 Child Loss & Memorial Circumstance Variance ──────────

        [Fact]
        public void ChildLoss_MarkChildLost_UpdatesState_AndFiresEvent()
        {
            var cohort = new CohortSystem();
            cohort.BookChild("c1", new List<string>(), "medium", 1);

            int eventFired = 0;
            string firedCause = string.Empty;
            cohort.OnChildLost += (id, day, cause) =>
            {
                eventFired++;
                firedCause = cause;
            };

            Assert.True(cohort.MarkChildLost("c1", day: 42, cause: "radiation_sickness"));
            Assert.Equal(1, eventFired);
            Assert.Equal("radiation_sickness", firedCause);

            var child = cohort.GetChild("c1");
            Assert.NotNull(child);
            Assert.True(child.isDeceased);
            Assert.Equal(42, child.deathDay);
            Assert.Equal("radiation_sickness", child.deathCause);

            // Redundant call refused
            Assert.False(cohort.MarkChildLost("c1", day: 43, cause: "radiation_sickness"));
        }

        [Fact]
        public void ChildLoss_DeceasedChildren_NotCountedInSurvivingChildren()
        {
            var cohort = new CohortSystem();
            cohort.BookChild("c1", new List<string>(), "medium", 1);
            cohort.BookChild("c2", new List<string>(), "high", 5);

            Assert.Equal(2, cohort.SurvivingChildrenCount);
            Assert.True(cohort.AnyChildrenSurvived);

            cohort.MarkChildLost("c1", day: 20, cause: "exposure");
            Assert.Equal(1, cohort.SurvivingChildrenCount);
            Assert.True(cohort.AnyChildrenSurvived);

            cohort.MarkChildLost("c2", day: 30, cause: "starvation");
            Assert.Equal(0, cohort.SurvivingChildrenCount);
            Assert.False(cohort.AnyChildrenSurvived);
        }

        [Fact]
        public void Memorial_CircumstanceVariance_PreservesDistinctDeathAttributes()
        {
            var memorial = new MemorialSystem(new MemorialState());
            var sink = new CapturingGriefSink();
            memorial.GriefSink = sink;

            // Circumstance 1: Disease (Peaceful, Burial)
            var entry1 = memorial.Memorialize(new MemorialInput
            {
                SurvivorId = "victim_disease",
                Cause = "lung_blight",
                Day = 60,
                BirthDay = 10,
                DeathQuality = DeathQuality.Peaceful,
                Outcome = MemorialOutcome.Burial,
                MoraleDelta = 10f,
                SurvivingRelationshipIds = new[] { "rel_a" }
            });

            // Circumstance 2: Combat (Rushed, AshScatter)
            var entry2 = memorial.Memorialize(new MemorialInput
            {
                SurvivorId = "victim_combat",
                Cause = "raider_ambush",
                Day = 85,
                BirthDay = 1,
                DeathQuality = DeathQuality.Rushed,
                Outcome = MemorialOutcome.AshScatter,
                MoraleDelta = 20f,
                SurvivingRelationshipIds = new[] { "rel_b" }
            });

            // Circumstance 3: Starvation (Unattended, WallEntry)
            var entry3 = memorial.Memorialize(new MemorialInput
            {
                SurvivorId = "victim_starvation",
                Cause = "winter_famine",
                Day = 120,
                BirthDay = 15,
                DeathQuality = DeathQuality.Unattended,
                Outcome = MemorialOutcome.WallEntry,
                MoraleDelta = 25f,
                SurvivingRelationshipIds = new[] { "rel_c" }
            });

            // Assert entries preserve distinct circumstance metadata
            Assert.Equal("lung_blight", entry1.Cause);
            Assert.Equal(DeathQuality.Peaceful, entry1.DeathQuality);
            Assert.Equal(MemorialOutcome.Burial, entry1.Outcome);
            Assert.Equal(50, entry1.SurvivedDays);

            Assert.Equal("raider_ambush", entry2.Cause);
            Assert.Equal(DeathQuality.Rushed, entry2.DeathQuality);
            Assert.Equal(MemorialOutcome.AshScatter, entry2.Outcome);
            Assert.Equal(84, entry2.SurvivedDays);

            Assert.Equal("winter_famine", entry3.Cause);
            Assert.Equal(DeathQuality.Unattended, entry3.DeathQuality);
            Assert.Equal(MemorialOutcome.WallEntry, entry3.Outcome);
            Assert.Equal(105, entry3.SurvivedDays);

            // Assert grief scale varied according to death quality
            Assert.Equal(3, sink.Records.Count);
            Assert.Equal(0.5f, sink.Records[0].QualityScale);
            Assert.Equal(1.0f, sink.Records[1].QualityScale);
            Assert.Equal(1.25f, sink.Records[2].QualityScale);
        }

        // ── 19B.11 / 19B.12 Children Survived Ending Fact ─────────────────────

        [Fact]
        public void CampaignOutcome_ChildrenSurvived_FalseWhenNoChildren()
        {
            var input = new CampaignOutcomeEvaluationInput
            {
                LivingDwellerCount = 10,
                CohortChildren = new List<CohortChild>()
            };

            var snapshot = CampaignOutcomeEvaluator.Evaluate(input);
            Assert.False(snapshot.ChildrenSurvived);
        }

        [Fact]
        public void CampaignOutcome_ChildrenSurvived_FalseWhenAllChildrenDeceased()
        {
            var input = new CampaignOutcomeEvaluationInput
            {
                LivingDwellerCount = 10,
                CohortChildren = new List<CohortChild>
                {
                    new CohortChild { survivorId = "c1", isDeceased = true, deathDay = 20, deathCause = "cold" },
                    new CohortChild { survivorId = "c2", isDeceased = true, deathDay = 30, deathCause = "famine" }
                }
            };

            var snapshot = CampaignOutcomeEvaluator.Evaluate(input);
            Assert.False(snapshot.ChildrenSurvived);
        }

        [Fact]
        public void CampaignOutcome_ChildrenSurvived_TrueWhenAtLeastOneSurvives()
        {
            var input = new CampaignOutcomeEvaluationInput
            {
                LivingDwellerCount = 10,
                CohortChildren = new List<CohortChild>
                {
                    new CohortChild { survivorId = "c1", isDeceased = true, deathDay = 20 },
                    new CohortChild { survivorId = "c2", isDeceased = false }
                }
            };

            var snapshot = CampaignOutcomeEvaluator.Evaluate(input);
            Assert.True(snapshot.ChildrenSurvived);
        }

        [Fact]
        public void CohortSystem_SaveLoad_RoundTripsMortalityAndMaturation()
        {
            var cohort = new CohortSystem();
            cohort.BookChild("c1", new List<string> { "p1" }, "low", birthDay: 10, moralityMemory: "born in dark");
            cohort.BookChild("c2", new List<string> { "p2" }, "high", birthDay: 20, moralityMemory: "born in dawn");

            cohort.TryMaturation("c1", day: 190);
            cohort.MarkChildLost("c2", day: 75, cause: "water_toxin");

            var state = cohort.CaptureState();
            var restored = new CohortSystem();
            restored.RestoreState(state);

            var c1 = restored.GetChild("c1");
            Assert.NotNull(c1);
            Assert.True(c1.isMatured);
            Assert.Equal(190, c1.maturationDay);
            Assert.False(c1.isDeceased);

            var c2 = restored.GetChild("c2");
            Assert.NotNull(c2);
            Assert.True(c2.isDeceased);
            Assert.Equal(75, c2.deathDay);
            Assert.Equal("water_toxin", c2.deathCause);

            Assert.Equal(1, restored.SurvivingChildrenCount);
            Assert.True(restored.AnyChildrenSurvived);
        }

        // ── 19B.13 Three-Year Deterministic Balance Simulation ────────────────

        [Fact]
        public void ThreeYearBalanceSimulation_DeterministicScaling()
        {
            // Simulate 3 years (1095 days)
            var (survivors1, foodUnits1, matured1) = RunThreeYearSimulation(seed: 12345);
            var (survivors2, foodUnits2, matured2) = RunThreeYearSimulation(seed: 12345);

            // Assert exact determinism across paired seeded runs
            Assert.Equal(survivors1, survivors2);
            Assert.Equal(foodUnits1, foodUnits2);
            Assert.Equal(matured1, matured2);

            // Assert meaningful simulation milestones occurred over 3 years
            Assert.True(survivors1 > 0, "At least some children survived the 3 years.");
            Assert.True(foodUnits1 > 0, "Children imposed ongoing food requirements.");
            Assert.True(matured1 > 0, "Children graduated to working age over 3 years.");
        }

        private static (int survivingCount, int totalFoodConsumed, int maturedCount) RunThreeYearSimulation(int seed)
        {
            var rng = new SeededRng(seed);
            var cohort = new CohortSystem();
            int totalFood = 0;
            int matured = 0;

            for (int day = 1; day <= 1095; day++)
            {
                // Births occur periodically (e.g. every 120 days)
                if (day % 120 == 0 && cohort.Children.Count < 10)
                {
                    string id = $"cohort_gen_{day}";
                    cohort.BookChild(id, new List<string> { "adult_1" }, "medium", birthDay: day);
                }

                // Daily food demand
                int dailyFood = cohort.CalculateChildFoodUnits(StartingLevel.RationPolicy.Standard);
                totalFood += dailyFood;

                // Aging & maturation check
                foreach (var child in cohort.Children)
                {
                    if (child.isDeceased) continue;

                    int age = day - child.birthDay;
                    if (age >= cohort.Tuning.maturation_age_days && !child.isMatured)
                    {
                        if (cohort.TryMaturation(child.survivorId, day))
                        {
                            matured++;
                        }
                    }

                    // Low probability hazard (e.g. winter days 300..360, 665..725)
                    bool isWinter = (day % 365) >= 300;
                    if (isWinter && !child.isMatured && rng.Next(0, 1000) < 5) // 0.5% risk in winter
                    {
                        cohort.MarkChildLost(child.survivorId, day, cause: "winter_chill");
                    }
                }
            }

            return (cohort.SurvivingChildrenCount, totalFood, matured);
        }
    }
}
