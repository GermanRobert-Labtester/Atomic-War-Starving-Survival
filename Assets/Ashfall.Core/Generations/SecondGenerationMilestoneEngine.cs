// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Generations
{
    public enum MilestoneKind
    {
        None = 0,
        FirstWords = 1,
        FoundationalLetters = 2,
        ToolHandling = 3,
        FieldSurvey = 4,
        VocationalApprenticeship = 5,
        RiteOfPassage = 6,
        SuccessionReadiness = 7,
    }

    public readonly struct MilestoneEvaluationResult
    {
        public bool Eligible { get; }
        public MilestoneKind Milestone { get; }
        public DevelopmentStage RequiredStage { get; }
        public float RequiredEducation { get; }
        public int AptitudeBonusPermille { get; }
        public string DossierNote { get; }

        public MilestoneEvaluationResult(
            bool eligible,
            MilestoneKind milestone,
            DevelopmentStage requiredStage,
            float requiredEducation,
            int aptitudeBonusPermille,
            string dossierNote)
        {
            Eligible = eligible;
            Milestone = milestone;
            RequiredStage = requiredStage;
            RequiredEducation = requiredEducation;
            AptitudeBonusPermille = aptitudeBonusPermille;
            DossierNote = dossierNote ?? string.Empty;
        }
    }

    /// <summary>
    /// Expansion 12: The Second Generation — Succession & Kinship Milestone Engine.
    /// Pure domain engine evaluating developmental milestones, curriculum progress,
    /// aptitude bonuses, and succession readiness for second-generation shelter children.
    /// Engine-free, deterministic integer permille calculations.
    /// </summary>
    public static class SecondGenerationMilestoneEngine
    {
        public static MilestoneEvaluationResult EvaluateNextMilestone(ChildProfile profile, int currentDay)
        {
            if (profile == null)
            {
                return new MilestoneEvaluationResult(false, MilestoneKind.None, DevelopmentStage.Infant, 0f, 0, "No profile.");
            }

            var milestones = new HashSet<string>(profile.Milestones ?? new List<string>(), StringComparer.OrdinalIgnoreCase);

            if (!milestones.Contains("first_words"))
            {
                bool eligible = profile.Stage >= DevelopmentStage.Toddler;
                return new MilestoneEvaluationResult(
                    eligible,
                    MilestoneKind.FirstWords,
                    DevelopmentStage.Toddler,
                    0.0f,
                    50,
                    "Acquired early language and vocalization in the shelter.");
            }

            if (!milestones.Contains("foundational_letters"))
            {
                bool eligible = profile.Stage >= DevelopmentStage.Child && profile.EducationScore >= 10f;
                return new MilestoneEvaluationResult(
                    eligible,
                    MilestoneKind.FoundationalLetters,
                    DevelopmentStage.Child,
                    10.0f,
                    100,
                    "Learned literacy and basic survival cipher reading.");
            }

            if (!milestones.Contains("tool_handling"))
            {
                bool eligible = profile.Stage >= DevelopmentStage.Child && profile.EducationScore >= 25f;
                return new MilestoneEvaluationResult(
                    eligible,
                    MilestoneKind.ToolHandling,
                    DevelopmentStage.Child,
                    25.0f,
                    150,
                    "Mastered basic tool maintenance and safety protocols.");
            }

            if (!milestones.Contains("field_survey"))
            {
                bool eligible = profile.Stage >= DevelopmentStage.Adolescent && profile.EducationScore >= 45f;
                return new MilestoneEvaluationResult(
                    eligible,
                    MilestoneKind.FieldSurvey,
                    DevelopmentStage.Adolescent,
                    45.0f,
                    200,
                    "Participated in perimeter observation and weather tracking.");
            }

            if (!milestones.Contains("vocational_apprenticeship"))
            {
                bool eligible = profile.Stage >= DevelopmentStage.Adolescent && profile.EducationScore >= 60f;
                return new MilestoneEvaluationResult(
                    eligible,
                    MilestoneKind.VocationalApprenticeship,
                    DevelopmentStage.Adolescent,
                    60.0f,
                    250,
                    "Assigned to a shelter craft mentor for specialized labor.");
            }

            if (!milestones.Contains("rite_of_passage"))
            {
                bool eligible = profile.Stage >= DevelopmentStage.YoungAdult && profile.EducationScore >= 80f;
                return new MilestoneEvaluationResult(
                    eligible,
                    MilestoneKind.RiteOfPassage,
                    DevelopmentStage.YoungAdult,
                    80.0f,
                    300,
                    "Completed the formal coming-of-age ceremony; recognized as full dweller.");
            }

            if (!milestones.Contains("succession_readiness"))
            {
                bool eligible = profile.Stage >= DevelopmentStage.YoungAdult && profile.EducationScore >= 95f;
                return new MilestoneEvaluationResult(
                    eligible,
                    MilestoneKind.SuccessionReadiness,
                    DevelopmentStage.YoungAdult,
                    95.0f,
                    500,
                    "Dossier approved for shelter leadership succession.");
            }

            return new MilestoneEvaluationResult(
                true,
                MilestoneKind.SuccessionReadiness,
                DevelopmentStage.YoungAdult,
                100.0f,
                500,
                "All second-generation milestones achieved.");
        }

        public static int CalculateSuccessionReadinessPermille(ChildProfile profile, bool parentDeceased, int parentKinshipPermille = 500)
        {
            if (profile == null) return 0;

            int score = 0;

            // Base by stage
            switch (profile.Stage)
            {
                case DevelopmentStage.Infant: score = 0; break;
                case DevelopmentStage.Toddler: score = 50; break;
                case DevelopmentStage.Child: score = 200; break;
                case DevelopmentStage.Adolescent: score = 500; break;
                case DevelopmentStage.YoungAdult: score = 800; break;
            }

            // Education contribution up to 150 permille
            int educationBonus = (int)Math.Min(150, profile.EducationScore * 1.5f);
            score += educationBonus;

            // Milestones achieved bonus (20 permille each, max 100)
            int milestoneBonus = Math.Min(100, (profile.Milestones?.Count ?? 0) * 20);
            score += milestoneBonus;

            // Urgency if parent is deceased (+50 permille)
            if (parentDeceased)
            {
                score += 50;
            }

            // Kinship scaling
            score = (score * Math.Max(200, Math.Min(1200, parentKinshipPermille))) / 1000;

            return Math.Max(0, Math.Min(1000, score));
        }

        public static Dictionary<string, int> CalculateAptitudeModifiers(ChildProfile profile)
        {
            var aptitudes = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                ["craft"] = 500,
                ["medicine"] = 500,
                ["scavenge"] = 500,
                ["agriculture"] = 500
            };

            if (profile == null) return aptitudes;

            var milestones = new HashSet<string>(profile.Milestones ?? new List<string>(), StringComparer.OrdinalIgnoreCase);

            if (milestones.Contains("tool_handling"))
                aptitudes["craft"] += 150;

            if (milestones.Contains("field_survey"))
                aptitudes["scavenge"] += 150;

            if (milestones.Contains("vocational_apprenticeship"))
            {
                aptitudes["craft"] += 100;
                aptitudes["medicine"] += 100;
                aptitudes["agriculture"] += 100;
            }

            if (milestones.Contains("rite_of_passage"))
            {
                aptitudes["scavenge"] += 100;
                aptitudes["agriculture"] += 100;
            }

            return aptitudes;
        }
    }
}
