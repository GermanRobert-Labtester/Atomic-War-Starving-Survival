// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 28 — The Lesson
// Subsystem    : Apprenticeship Curriculum & Literacy Progression Engine
// Authority    : docs/expansions/wave4/expansion_28_the_lesson_plan.md
//                UNBLOCK-05_EXPANSION_WAVES_C3_EN_GATE.md §3.1, §5.3
// ============================================================================
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Education
{
    /// <summary>
    /// Literacy attainment tier for a survivor learner.
    /// </summary>
    public enum LiteracyLevel
    {
        Illiterate  = 0,  // cannot read; cannot transcribe
        Basic       = 1,  // reads simple signage; slow learner
        Functional  = 2,  // reads manuals; normal learner throughput
        Scholarly   = 3   // reads advanced texts; can teach others
    }

    /// <summary>
    /// Mutable record tracking a learner's curriculum progress.
    /// </summary>
    public sealed class LearnerRecord
    {
        public string SurvivorId             { get; set; } = string.Empty;
        public LiteracyLevel LiteracyLevel   { get; set; } = LiteracyLevel.Illiterate;
        /// <summary>Accumulated comprehension points toward next literacy tier (0..1000 permille).</summary>
        public int ComprehensionPermille     { get; set; } = 0;
        /// <summary>Trade-specific skill progress keyed by trade ID (0..1000 permille each).</summary>
        public Dictionary<string, int> TradeSkillPermille { get; set; } = new();
        /// <summary>Session count without adequate sleep rest (fatigue accumulates over sessions).</summary>
        public int FatigueSessionCount       { get; set; } = 0;

        public LearnerRecord Clone()
        {
            var clone = new LearnerRecord
            {
                SurvivorId         = SurvivorId,
                LiteracyLevel      = LiteracyLevel,
                ComprehensionPermille = ComprehensionPermille,
                FatigueSessionCount = FatigueSessionCount
            };
            foreach (var kv in TradeSkillPermille)
                clone.TradeSkillPermille[kv.Key] = kv.Value;
            return clone;
        }
    }

    /// <summary>
    /// Immutable result of a literacy teaching session.
    /// </summary>
    public readonly struct CurriculumSessionResult
    {
        /// <summary>Comprehension gained this session (0..1000).</summary>
        public int ComprehensionGained       { get; }
        /// <summary>True if the learner advanced to the next literacy tier this session.</summary>
        public bool AdvancedLiteracyTier     { get; }
        /// <summary>New literacy level (same as before if no tier advance).</summary>
        public LiteracyLevel NewLiteracyLevel { get; }
        /// <summary>Fatigue penalty applied this session due to consecutive learning overload.</summary>
        public int FatiguePenaltyPermille    { get; }

        public CurriculumSessionResult(
            int comprehensionGained,
            bool advancedLiteracyTier,
            LiteracyLevel newLiteracyLevel,
            int fatiguePenaltyPermille)
        {
            ComprehensionGained   = Math.Max(0, comprehensionGained);
            AdvancedLiteracyTier  = advancedLiteracyTier;
            NewLiteracyLevel      = newLiteracyLevel;
            FatiguePenaltyPermille = Math.Clamp(fatiguePenaltyPermille, 0, 500);
        }
    }

    /// <summary>
    /// Pure domain engine governing literacy session progression, vocational
    /// certification readiness, and manual transcription yield.
    /// Extends ApprenticeshipSystem (root Core) without duplicating
    /// mentorship pair tracking or save section ownership.
    /// Zero engine references, integer permille determinism.
    /// </summary>
    public static class ApprenticeshipCurriculumEngine
    {
        /// <summary>Comprehension permille required to advance each tier boundary.</summary>
        public const int TierAdvanceThreshold = 1000;

        /// <summary>Vocational certification readiness threshold (900 permille = job-ready).</summary>
        public const int CertificationReadyThreshold = 900;

        /// <summary>Consecutive fatigue sessions before a penalty kicks in.</summary>
        public const int FatigueOnsetSessions = 3;

        /// <summary>
        /// Conducts one literacy teaching session.
        /// Mutates the learner record with comprehension gain and potential tier advance.
        /// </summary>
        /// <param name="learner">Learner record to mutate.</param>
        /// <param name="teacherSkillPermille">
        ///     Teacher's teaching ability (0..1000); Scholarly teacher = 1000.
        /// </param>
        /// <param name="manualAvailabilityPermille">
        ///     Quality and availability of reading manuals (0 = none, 1000 = full library).
        /// </param>
        /// <param name="sessionSeed">Deterministic seed for session variance (use HashCode.Combine).</param>
        /// <returns>Session result; caller applies ComprehensionGained to learner.ComprehensionPermille.</returns>
        public static CurriculumSessionResult AdvanceLiteracySession(
            LearnerRecord learner,
            int teacherSkillPermille,
            int manualAvailabilityPermille,
            int sessionSeed)
        {
            if (learner == null) throw new ArgumentNullException(nameof(learner));

            teacherSkillPermille      = Math.Clamp(teacherSkillPermille, 0, 1000);
            manualAvailabilityPermille = Math.Clamp(manualAvailabilityPermille, 0, 1000);

            // Scholarly learners can still advance if not yet maxed
            if (learner.LiteracyLevel == LiteracyLevel.Scholarly &&
                learner.ComprehensionPermille >= TierAdvanceThreshold)
            {
                // Already at max tier
                return new CurriculumSessionResult(0, false, LiteracyLevel.Scholarly, 0);
            }

            // Base comprehension yield from teacher quality and manual access
            int baseYield = (teacherSkillPermille * 80 + manualAvailabilityPermille * 40) / 1000;

            // Learner literacy multiplier (higher literacy = faster absorption)
            int literacyMultiplier = learner.LiteracyLevel switch
            {
                LiteracyLevel.Illiterate => 600,
                LiteracyLevel.Basic      => 850,
                LiteracyLevel.Functional => 1000,
                LiteracyLevel.Scholarly  => 1200,
                _                         => 800
            };
            baseYield = (baseYield * literacyMultiplier) / 1000;

            // Deterministic session variance ±15% using seeded hash
            int hash = HashCode.Combine(sessionSeed, learner.SurvivorId, learner.ComprehensionPermille);
            int variance = ((hash & 0x7FFFFFFF) % 31) - 15; // -15..+15
            baseYield = Math.Max(1, baseYield + (baseYield * variance) / 100);

            // Fatigue penalty
            int fatiguePenalty = 0;
            if (learner.FatigueSessionCount >= FatigueOnsetSessions)
            {
                int fatigueSeverity = Math.Min(500, (learner.FatigueSessionCount - FatigueOnsetSessions) * 60);
                baseYield     = Math.Max(0, baseYield - (baseYield * fatigueSeverity) / 1000);
                fatiguePenalty = fatigueSeverity;
            }

            // Apply gain to record and check tier advance
            learner.ComprehensionPermille += baseYield;
            learner.FatigueSessionCount++;

            bool advanced = false;
            LiteracyLevel newLevel = learner.LiteracyLevel;

            if (learner.ComprehensionPermille >= TierAdvanceThreshold &&
                learner.LiteracyLevel < LiteracyLevel.Scholarly)
            {
                learner.LiteracyLevel       = (LiteracyLevel)((int)learner.LiteracyLevel + 1);
                learner.ComprehensionPermille = learner.ComprehensionPermille - TierAdvanceThreshold;
                learner.FatigueSessionCount   = 0; // tier advance resets fatigue
                advanced  = true;
                newLevel  = learner.LiteracyLevel;
            }

            return new CurriculumSessionResult(baseYield, advanced, newLevel, fatiguePenalty);
        }

        /// <summary>
        /// Evaluates a learner's readiness for vocational trade certification.
        /// </summary>
        /// <param name="learner">Learner record.</param>
        /// <param name="tradeId">Trade identifier (must match a key in TradeSkillPermille).</param>
        /// <returns>
        ///     True if the learner's trade skill meets <see cref="CertificationReadyThreshold"/>
        ///     and they are at least Functional literacy.
        /// </returns>
        public static bool EvaluateVocationalCertification(LearnerRecord learner, string tradeId)
        {
            if (learner == null) throw new ArgumentNullException(nameof(learner));
            if (string.IsNullOrEmpty(tradeId)) return false;
            if (learner.LiteracyLevel < LiteracyLevel.Functional) return false;

            return learner.TradeSkillPermille.TryGetValue(tradeId, out int skill)
                   && skill >= CertificationReadyThreshold;
        }

        /// <summary>
        /// Calculates the number of manual pages a Scholarly survivor can transcribe in a labor block.
        /// </summary>
        /// <param name="masterLiteracyLevel">Transcriber's literacy level (Scholarly required for meaningful output).</param>
        /// <param name="laborHours">Hours dedicated to transcription (1..24).</param>
        /// <returns>Pages transcribed (0 if not Scholarly).</returns>
        public static int CalculateManualTranscriptionYield(LiteracyLevel masterLiteracyLevel, int laborHours)
        {
            laborHours = Math.Clamp(laborHours, 0, 24);

            if (masterLiteracyLevel < LiteracyLevel.Scholarly)
                return 0;

            // A Scholarly survivor transcribes ~4 pages/hour at full concentration
            int pagesPerHour = 4;
            // Fatigue kicks in after 6 hours: diminishing returns
            int sustainedHours = Math.Min(laborHours, 6);
            int overtimeHours  = Math.Max(0, laborHours - 6);
            return (sustainedHours * pagesPerHour) + (overtimeHours * pagesPerHour / 2);
        }
    }
}
