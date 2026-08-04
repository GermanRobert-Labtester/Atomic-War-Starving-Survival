using NUnit.Framework;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.AI.Actions;
using UnityEngine;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class MentorshipTests
    {
        private MentorshipSystem _mentorshipSystem;
        private TeachSkillActionSO _teachAction;
        private Survivor _dyingEngineer;
        private Survivor _youngApprentice;
        private InterpersonalAffinity _affinity;

        [SetUp]
        public void SetUp()
        {
            _mentorshipSystem = new MentorshipSystem();
            _teachAction = ScriptableObject.CreateInstance<TeachSkillActionSO>();
            _teachAction.targetSkillName = "crafting";

            _dyingEngineer = new Survivor
            {
                Id = "mentor_engineer",
                DisplayName = "Old Engineer",
                PrognosisStage = PrognosisStage.Manifest,
                CraftingSkill = 0.85f
            };
            _dyingEngineer.Needs.Morale = 75f;

            _youngApprentice = new Survivor
            {
                Id = "student_kid",
                DisplayName = "Apprentice",
                CraftingSkill = 0.20f
            };
            _youngApprentice.Needs.Morale = 60f;

            _affinity = new InterpersonalAffinity();
            _affinity.Set("mentor_engineer", "student_kid", 50f);
        }

        [Test]
        public void CanMentor_ValidDyingMentorHighSkillAndMorale_ReturnsTrue()
        {
            bool canTeach = _mentorshipSystem.CanMentor(_dyingEngineer, _youngApprentice, "crafting", _affinity);
            Assert.IsTrue(canTeach);
        }

        [Test]
        public void CanMentor_HealthyMentor_ReturnsFalse()
        {
            _dyingEngineer.PrognosisStage = PrognosisStage.Healthy;
            bool canTeach = _mentorshipSystem.CanMentor(_dyingEngineer, _youngApprentice, "crafting", _affinity);
            Assert.IsFalse(canTeach);
        }

        [Test]
        public void TeachSkillSession_TransfersSkillToStudent()
        {
            float initialSkill = _youngApprentice.CraftingSkill;

            bool success = _mentorshipSystem.TeachSkillSession(_dyingEngineer, _youngApprentice, "crafting", 10f, _affinity);

            Assert.IsTrue(success);
            Assert.Greater(_youngApprentice.CraftingSkill, initialSkill);
        }
    }
}
