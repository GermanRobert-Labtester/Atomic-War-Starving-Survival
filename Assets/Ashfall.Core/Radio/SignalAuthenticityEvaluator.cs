// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Persisted authenticity verdict categories (Task 2).
    /// Unknown = analysis not yet performed or inconclusive.
    /// </summary>
    public enum SignalAuthenticityCategory
    {
        Unknown = 0,
        Genuine = 1,
        Trap = 2,
        FalseFlag = 3,
        Stale = 4,
        Uncertain = 5
    }

    /// <summary>
    /// Structured result of one authenticity analysis (plan §6.2).
    /// </summary>
    [Serializable]
    public sealed class SignalAuthenticityCheckResult
    {
        public bool CheckPerformed { get; set; }
        public bool ThreatDetected { get; set; }
        public bool StalenessDetected { get; set; }
        public SignalAuthenticityCategory Assessment { get; set; } = SignalAuthenticityCategory.Unknown;
        public string SkillId { get; set; } = string.Empty;
        public int SkillBonus { get; set; }
        public int Difficulty { get; set; }
        public int DetectionChance { get; set; }
        /// <summary>Deterministic roll that produced the verdict; -1 when the
        /// result was returned from persistence (no reroll performed).</summary>
        public int Roll { get; set; }
    }

    /// <summary>
    /// Pure, deterministic authenticity evaluator for distress signals
    /// (Task 2). Engine-free; no shared-state RNG consumption — the caller
    /// supplies a dedicated ISeededRng sub-stream, so an authenticity check
    /// never perturbs expedition/encounter/scavenging randomness.
    ///
    /// Hard invariants:
    /// - genuine signals are NEVER classified as Trap or FalseFlag;
    /// - stale signals are NEVER classified as traps; staleness is a
    ///   deterministic read of trace progress (authenticity == "stale" AND
    ///   days traced &gt;= days_to_trace), consuming no randomness;
    /// - deception detection probability rises monotonically with skill.
    /// </summary>
    public static class SignalAuthenticityEvaluator
    {
        /// <summary>Base deception-check difficulty on a 0–100 scale
        /// (plan §6.4 mapping; detection chance = 100 − difficulty + skill bonus).</summary>
        public const int DeceptionDifficulty = 65;

        public const string SkillSignalEar = "skill_signal_ear";
        public const string SkillWatchful = "skill_watchful";
        public const string SkillColdAnalysis = "skill_cold_analysis";

        public const int BonusSignalEar = 15;
        public const int BonusWatchful = 10;
        public const int BonusColdAnalysis = 8;

        /// <summary>
        /// Evaluates one authenticity analysis. <paramref name="rng"/> must be a
        /// dedicated deterministic sub-stream (derived per signal+survivor).
        /// </summary>
        /// <param name="def">Signal definition (ground-truth authenticity).</param>
        /// <param name="daysTraced">Days elapsed since first heard.</param>
        /// <param name="hasSkill">Skill lookup in the canonical skill ids; null = no skills.</param>
        /// <param name="rng">Dedicated deterministic RNG sub-stream.</param>
        public static SignalAuthenticityCheckResult Evaluate(
            DistressSignalDefinition def,
            int daysTraced,
            string survivorId,
            Func<string, string, bool>? hasSkill,
            ISeededRng rng)
        {
            var result = new SignalAuthenticityCheckResult();
            if (def == null)
            {
                result.Assessment = SignalAuthenticityCategory.Unknown;
                return result;
            }

            (string skillId, int bonus) = ResolveSkill(survivorId, hasSkill);
            result.SkillId = skillId;
            result.SkillBonus = bonus;
            result.Difficulty = DeceptionDifficulty;
            result.DetectionChance = (100 - DeceptionDifficulty) + bonus;
            result.CheckPerformed = true;

            if (def.IsTrapOrDeception)
            {
                int roll = rng.Next(0, 100);
                result.Roll = roll;
                result.ThreatDetected = roll < result.DetectionChance;
                result.Assessment = result.ThreatDetected
                    ? (def.Authenticity.Equals("false_flag", StringComparison.OrdinalIgnoreCase)
                        ? SignalAuthenticityCategory.FalseFlag
                        : SignalAuthenticityCategory.Trap)
                    : SignalAuthenticityCategory.Uncertain;
                return result;
            }

            if (def.Authenticity.Equals("stale", StringComparison.OrdinalIgnoreCase))
            {
                // Staleness is never rolled and never a trap classification.
                // Before trace completion the operator can only be uncertain.
                result.StalenessDetected = daysTraced >= def.DaysToTrace;
                result.Assessment = result.StalenessDetected
                    ? SignalAuthenticityCategory.Stale
                    : SignalAuthenticityCategory.Uncertain;
                result.Roll = -1;
                return result;
            }

            if (def.IsGenuineRescue)
            {
                // Genuine: the roll can only confirm or leave uncertain.
                // A weak analysis never fabricates hostility.
                int roll = rng.Next(0, 100);
                result.Roll = roll;
                result.Assessment = roll < result.DetectionChance
                    ? SignalAuthenticityCategory.Genuine
                    : SignalAuthenticityCategory.Unknown;
                return result;
            }

            // Automated/encrypted/unknown categories carry no rescue analysis.
            result.CheckPerformed = false;
            result.Assessment = SignalAuthenticityCategory.Unknown;
            result.Roll = -1;
            return result;
        }

        /// <summary>
        /// Skill preference (plan §6.3): radio/technical signal ear first,
        /// then watchful, then cold analysis. Only the primary skill's bonus
        /// applies — no additive stacking.
        /// </summary>
        private static (string SkillId, int Bonus) ResolveSkill(
            string survivorId,
            Func<string, string, bool>? hasSkill)
        {
            if (!string.IsNullOrEmpty(survivorId) && hasSkill != null)
            {
                if (hasSkill(survivorId, SkillSignalEar)) return (SkillSignalEar, BonusSignalEar);
                if (hasSkill(survivorId, SkillWatchful)) return (SkillWatchful, BonusWatchful);
                if (hasSkill(survivorId, SkillColdAnalysis)) return (SkillColdAnalysis, BonusColdAnalysis);
            }
            return (string.Empty, 0);
        }
    }
}
