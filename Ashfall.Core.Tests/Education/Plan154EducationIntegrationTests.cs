// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core Tests : Plan 154 — Survivor Education & Knowledge Transfer
// Subsystem          : EducationSystem / Generational Knowledge Integration Tests
// Authority          : Next-steps-plans/Plan_154_Survivor_Education_Knowledge_Transfer.md
//                      UNBLOCK-PROGRAM-WAVE32-BATCH5-PLANS (DEC-151)
// ============================================================================
using System;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Education;

namespace Ashfall.Core.Tests.Plan154Education
{
    public sealed class Plan154EducationIntegrationTests
    {
        private static string ResolveDataPath(string filename)
        {
            string[] candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates)
            {
                if (File.Exists(c)) return Path.GetFullPath(c);
            }
            return Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename);
        }

        [Fact]
        public void Catalog_Loads_AllStagesAndSubjectsSuccessfully()
        {
            var system = new SurvivorEducationSystem();
            string path = ResolveDataPath("education_curriculum.json");
            Assert.True(File.Exists(path), $"education_curriculum.json missing at {path}");

            string json = File.ReadAllText(path);
            system.LoadCatalog(json);

            var lit = system.GetSubject("literacy");
            Assert.NotNull(lit);
            Assert.Equal("skill_reading_comprehension", lit.UnlockedSkillId);

            var eng = system.GetSubject("advanced_engineering");
            Assert.NotNull(eng);
            Assert.Contains("crafting_fundamentals", eng.Prerequisites);
            Assert.Equal("skill_reactor_maintenance", eng.UnlockedSkillId);
        }

        [Fact]
        public void StageProgression_AdvancesWithSurvivorAge()
        {
            var system = new SurvivorEducationSystem();
            var record = system.RegisterLearner("surv_child_01", 8);

            Assert.Equal(EducationStageType.Childhood, record.CurrentStage);
            Assert.False(record.IsGraduated);

            // Advance age into adolescence
            bool stageChanged = system.UpdateAge("surv_child_01", 13);
            Assert.True(stageChanged);
            record = system.GetRecord("surv_child_01")!;
            Assert.Equal(EducationStageType.Adolescence, record.CurrentStage);

            // Advance age into young adult
            system.UpdateAge("surv_child_01", 17);
            record = system.GetRecord("surv_child_01")!;
            Assert.Equal(EducationStageType.YoungAdult, record.CurrentStage);

            // Advance age to 18 (adulthood graduation)
            system.UpdateAge("surv_child_01", 18);
            record = system.GetRecord("surv_child_01")!;
            Assert.Equal(EducationStageType.AdultGraduate, record.CurrentStage);
            Assert.True(record.IsGraduated);
        }

        [Fact]
        public void DailySession_AdvancesProficiencyAndUnlocksSkills()
        {
            var system = new SurvivorEducationSystem();
            system.RegisterLearner("surv_student_02", 10);
            system.AssignTeacherAndSubject("surv_student_02", "teacher_elena", "literacy", isParentChild: false);

            var rng = new SeededRng(12345);
            EducationSessionResult lastResult = default;

            // Conduct sessions until literacy proficiency reaches requirement (75)
            for (int i = 0; i < 15; i++)
            {
                lastResult = system.ConductDailySession("surv_student_02", rng, hasSchoolroom: true);
                if (lastResult.SubjectCompleted)
                    break;
            }

            Assert.True(lastResult.SubjectCompleted);
            Assert.Equal("skill_reading_comprehension", lastResult.UnlockedSkillId);

            var record = system.GetRecord("surv_student_02")!;
            Assert.Contains("skill_reading_comprehension", record.UnlockedSkillIds);
            Assert.True(record.SubjectsStudied["literacy"] >= 75);
        }

        [Fact]
        public void TeachingBonus_ParentAndSchoolroomAccelerateProgress()
        {
            var system = new SurvivorEducationSystem();
            system.RegisterLearner("student_standard", 10);
            system.RegisterLearner("student_boosted", 10);

            system.AssignTeacherAndSubject("student_standard", "mentor_stranger", "numeracy", isParentChild: false);
            system.AssignTeacherAndSubject("student_boosted", "parent_mentor", "numeracy", isParentChild: true);

            var rngStandard = new SeededRng(42);
            var rngBoosted = new SeededRng(42);

            var resStandard = system.ConductDailySession("student_standard", rngStandard, hasSchoolroom: false);
            var resBoosted = system.ConductDailySession("student_boosted", rngBoosted, hasSchoolroom: true);

            // Boosted learner with parent teacher (+20%) and schoolroom (+10%) gains noticeably more proficiency
            Assert.True(resBoosted.ProficiencyGained > resStandard.ProficiencyGained);
        }

        [Fact]
        public void Graduation_AwardsSkillsAndBumpsShelterKnowledge()
        {
            var system = new SurvivorEducationSystem();
            int initialKnowledge = system.ShelterKnowledgeLevel;

            system.RegisterLearner("surv_grad_candidate", 17);
            system.AssignTeacherAndSubject("surv_grad_candidate", "mentor_doc", "literacy");

            // Evaluate graduation at day 45
            bool graduated = system.EvaluateGraduation("surv_grad_candidate", 45);
            Assert.True(graduated);

            var record = system.GetRecord("surv_grad_candidate")!;
            Assert.True(record.IsGraduated);
            Assert.Equal(45, record.GraduationDay);
            Assert.Equal(1, system.TotalGraduates);
            Assert.True(system.ShelterKnowledgeLevel > initialKnowledge);
        }

        [Fact]
        public void SaveRestore_PreservesAllRecordsAndKnowledgeLevel()
        {
            var system = new SurvivorEducationSystem();
            system.RegisterLearner("surv_save_01", 11);
            system.AssignTeacherAndSubject("surv_save_01", "teacher_a", "literacy", isParentChild: true);
            var rng = new SeededRng(777);
            system.ConductDailySession("surv_save_01", rng, hasSchoolroom: true);
            system.AdjustShelterKnowledge(14);

            var state = system.CaptureState();
            Assert.Equal(1, state.SchemaVersion);

            var restoredSystem = new SurvivorEducationSystem();
            restoredSystem.RestoreState(state);

            Assert.Equal(system.ShelterKnowledgeLevel, restoredSystem.ShelterKnowledgeLevel);
            Assert.Equal(system.TotalSessionsConducted, restoredSystem.TotalSessionsConducted);

            var restoredRec = restoredSystem.GetRecord("surv_save_01");
            Assert.NotNull(restoredRec);
            Assert.Equal("teacher_a", restoredRec.AssignedTeacherId);
            Assert.True(restoredRec.IsParentTeacher);
            Assert.True(restoredRec.SubjectsStudied.ContainsKey("literacy"));
        }
    }
}
