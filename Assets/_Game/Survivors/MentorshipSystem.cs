using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    public class MentorshipSystem
    {
        private PersonalQuestSystem _personalQuests;

        /// <summary>Prompt #235 — Polymath mentors any skill.</summary>
        public void BindPersonalQuests(PersonalQuestSystem personalQuests) =>
            _personalQuests = personalQuests;

        public const float MentorSkillThreshold = 0.6f;
        public const float StudentSkillThreshold = 0.4f;
        public const float MoraleThreshold = 50f;
        public const float AffinityThreshold = 30f;
        public const float SkillGainRatePerHour = 0.02f;

        public event Action<Survivor, Survivor, string, float> OnMentorshipProgress;

        public bool CanMentor(Survivor mentor, Survivor student, string skillName, InterpersonalAffinity affinityMatrix)
        {
            if (mentor == null || !mentor.IsAlive || student == null || !student.IsAlive) return false;
            if (mentor.Id == student.Id) return false;

            // Must be in Latent or Manifest prognosis stage
            if (mentor.PrognosisStage != PrognosisStage.Latent && mentor.PrognosisStage != PrognosisStage.Manifest)
            {
                return false;
            }

            // High morale requirement
            if (mentor.Needs == null || mentor.Needs.Morale < MoraleThreshold)
            {
                return false;
            }

            // Interpersonal affinity requirement
            float affinity = affinityMatrix != null ? affinityMatrix.Get(mentor.Id, student.Id) : 0f;
            if (affinity < AffinityThreshold)
            {
                return false;
            }

            // Skill thresholds — Polymath (#235) may mentor any skill.
            bool polymath = _personalQuests != null && _personalQuests.UnlocksSkillMentorshipForAllSkills(mentor);
            float mentorSkill = GetSkillValue(mentor, skillName);
            float studentSkill = GetSkillValue(student, skillName);
            if (polymath)
                return studentSkill <= StudentSkillThreshold;
            return mentorSkill >= MentorSkillThreshold && studentSkill <= StudentSkillThreshold;
        }

        public bool TeachSkillSession(Survivor mentor, Survivor student, string skillName, float gameHours, InterpersonalAffinity affinityMatrix)
        {
            if (!CanMentor(mentor, student, skillName, affinityMatrix)) return false;

            float gain = SkillGainRatePerHour * gameHours;
            float mentorSkill = GetSkillValue(mentor, skillName);
            float currentStudentSkill = GetSkillValue(student, skillName);

            float newSkill = Mathf.Min(mentorSkill, currentStudentSkill + gain);
            SetSkillValue(student, skillName, newSkill);

            OnMentorshipProgress?.Invoke(mentor, student, skillName, gain);
            return true;
        }

        public static float GetSkillValue(Survivor sv, string skillName)
        {
            if (sv == null) return 0f;
            switch (skillName?.ToLowerInvariant())
            {
                case "medical": return sv.MedicalSkill;
                case "crafting": return sv.CraftingSkill;
                case "science": return sv.ScienceSkill;
                default: return sv.CraftingSkill;
            }
        }

        public static void SetSkillValue(Survivor sv, string skillName, float val)
        {
            if (sv == null) return;
            float clamped = Mathf.Clamp01(val);
            switch (skillName?.ToLowerInvariant())
            {
                case "medical": sv.MedicalSkill = clamped; break;
                case "crafting": sv.CraftingSkill = clamped; break;
                case "science": sv.ScienceSkill = clamped; break;
                default: sv.CraftingSkill = clamped; break;
            }
        }
    }
}
