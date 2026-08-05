using System;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "NewTeachSkillAction", menuName = "ASHFALL/AI Actions/Teach Skill")]
    public class TeachSkillActionSO : SurvivorAction
    {
        public string targetSkillName = "crafting";

        [Tooltip("Game-hours of mentorship applied per AI execute.")]
        public float sessionHours = 1f;

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null || !context.Survivor.IsAlive) return 0f;
            if (context.MentorshipSystem == null || context.GetSurvivors == null) return 0f;

            var student = FindBestStudent(context);
            if (student == null) return 0f;
            return ScoreAction(
                context.Survivor, student, context.MentorshipSystem, context.Affinity,
                context.PersonalQuests);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || context.MentorshipSystem == null) return;
            var student = FindBestStudent(context);
            if (student == null) return;
            PerformTeach(context.Survivor, student, context.MentorshipSystem,
                sessionHours, context.Affinity);
        }

        public float ScoreAction(Survivor mentor, Survivor student, MentorshipSystem system, InterpersonalAffinity affinity)
        {
            if (system == null) return 0f;
            if (system.CanMentor(mentor, student, targetSkillName, affinity))
            {
                // #252 Hardened Daughter: Utility AI heavily favors Train.
                // Bias applied by caller via context when available; base train urge here.
                return 0.90f;
            }
            return 0f;
        }

        public float ScoreAction(
            Survivor mentor,
            Survivor student,
            MentorshipSystem system,
            InterpersonalAffinity affinity,
            PersonalQuestSystem personalQuests)
        {
            float baseScore = ScoreAction(mentor, student, system, affinity);
            if (baseScore <= 0f || personalQuests == null) return baseScore;
            return Mathf.Clamp01(baseScore * personalQuests.GetTrainGuardUtilityBias(mentor));
        }

        public bool PerformTeach(Survivor mentor, Survivor student, MentorshipSystem system, float durationHours, InterpersonalAffinity affinity)
        {
            if (system == null) return false;
            return system.TeachSkillSession(mentor, student, targetSkillName, durationHours, affinity);
        }

        private Survivor FindBestStudent(AIContext context)
        {
            var list = context.GetSurvivors?.Invoke();
            if (list == null || context.MentorshipSystem == null) return null;

            Survivor best = null;
            float bestScore = 0f;
            for (int i = 0; i < list.Count; i++)
            {
                var candidate = list[i];
                if (candidate == null || !candidate.IsAlive) continue;
                if (candidate.Id == context.Survivor.Id) continue;
                float score = ScoreAction(context.Survivor, candidate,
                    context.MentorshipSystem, context.Affinity);
                if (score > bestScore)
                {
                    bestScore = score;
                    best = candidate;
                }
            }
            return best;
        }
    }
}
