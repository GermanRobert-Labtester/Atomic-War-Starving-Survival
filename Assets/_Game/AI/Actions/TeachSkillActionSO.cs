using System;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "NewTeachSkillAction", menuName = "ASHFALL/AI Actions/Teach Skill")]
    public class TeachSkillActionSO : SurvivorAction
    {
        public string targetSkillName = "crafting";

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;
            return 0.5f;
        }

        public float ScoreAction(Survivor mentor, Survivor student, MentorshipSystem system, InterpersonalAffinity affinity)
        {
            if (system == null) return 0f;
            if (system.CanMentor(mentor, student, targetSkillName, affinity))
            {
                return 0.90f;
            }
            return 0f;
        }

        public bool PerformTeach(Survivor mentor, Survivor student, MentorshipSystem system, float durationHours, InterpersonalAffinity affinity)
        {
            if (system == null) return false;
            return system.TeachSkillSession(mentor, student, targetSkillName, durationHours, affinity);
        }
    }
}
