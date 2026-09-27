// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : SecondGenerationMilestoneHostSession
// Purpose      : PLAN-GENERATIONAL-MILESTONE-TRUTH-160 — evaluate and record
//                second-generation milestones through the ChildDevelopment
//                owner. Milestones live in ChildDevelopmentState; no new save.
// ============================================================================
using Ashfall.Core.Generations;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public sealed class SecondGenerationMilestoneHostSession
    {
        private readonly ChildDevelopmentSystem _children;
        public string LastEvent { get; private set; } = string.Empty;

        public SecondGenerationMilestoneHostSession(ChildDevelopmentSystem children)
        {
            _children = children;
        }

        public MilestoneEvaluationResult EvaluateAndRecord(string childId, int currentDay)
        {
            var profile = _children.GetChild(childId);
            if (profile == null)
            {
                LastEvent = $"No child profile for '{childId}'.";
                return new MilestoneEvaluationResult(false, MilestoneKind.None, DevelopmentStage.Infant, 0f, 0, "No profile.");
            }

            var result = SecondGenerationMilestoneEngine.EvaluateNextMilestone(profile, currentDay);
            if (result.Eligible)
            {
                string key = KeyFor(result.Milestone);
                bool recorded = _children.RecordSecondGenerationMilestone(childId, key, currentDay, result.DossierNote);
                LastEvent = recorded
                    ? $"{profile.Name} achieved {key} (day {currentDay})."
                    : $"{profile.Name} already holds {key}.";
            }
            else
            {
                LastEvent = $"{profile.Name} not yet eligible for {result.Milestone}.";
            }
            return result;
        }

        public int TickAll(int currentDay)
        {
            int recorded = 0;
            var profiles = _children.CaptureState().Profiles;
            for (int i = 0; i < profiles.Count; i++)
            {
                string childId = profiles[i].ChildId;
                var before = _children.GetChild(childId)?.Milestones.Count ?? 0;
                EvaluateAndRecord(childId, currentDay);
                var after = _children.GetChild(childId)?.Milestones.Count ?? 0;
                if (after > before) recorded++;
            }
            return recorded;
        }

        private static string KeyFor(MilestoneKind kind) => kind switch
        {
            MilestoneKind.FirstWords => "first_words",
            MilestoneKind.FoundationalLetters => "foundational_letters",
            MilestoneKind.ToolHandling => "tool_handling",
            MilestoneKind.FieldSurvey => "field_survey",
            MilestoneKind.VocationalApprenticeship => "vocational_apprenticeship",
            MilestoneKind.RiteOfPassage => "rite_of_passage",
            MilestoneKind.SuccessionReadiness => "succession_readiness",
            _ => "none"
        };
    }
}
