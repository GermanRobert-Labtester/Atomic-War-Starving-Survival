// SPDX-License-Identifier: MIT
// ============================================================================
// CORE-MECH W8 — Moral choice → rumor seed (pure, deterministic).
//
// Plan: docs/plans/CORE_GAME_MECHANICS_GAP_SEAL_MASTER_INTEGRATION_PLAN.md
//       (W8-GOSSIP-PROPAGATION, appendices AA.8 / AW.8 / BD.8).
//
// The information-flow owner already models propagation (hubs, propagation
// speed, decay, interception). What never happened: a moral choice seeding one.
// This file is the missing translation and nothing else — pure data in, a seed
// description out. The caller (Main) hands the result to the canonical RumorSystem,
// which stays the only rumor authority; no second rumor store, no parallel
// propagation model, no new save section.
// ============================================================================

using System;

namespace Ashfall.Core.MoralChoice
{
    /// <summary>What a resolved choice would say, if it became a rumor.</summary>
    public sealed class MoralGossipSeed
    {
        public string SubjectId { get; }
        public string OriginLocationId { get; }
        public int OriginDay { get; }
        public string Headline { get; }
        public string Description { get; }

        /// <summary>How likely the traveling version is to be true (0..1).</summary>
        public float Truthfulness { get; }

        /// <summary>Authored decay/speed profile the rumor owner may adopt.</summary>
        public float DecayRate { get; }
        public int PropagationSpeed { get; }

        public MoralGossipSeed(
            string subjectId, string originLocationId, int originDay,
            string headline, string description,
            float truthfulness, float decayRate, int propagationSpeed)
        {
            SubjectId = subjectId ?? string.Empty;
            OriginLocationId = originLocationId ?? string.Empty;
            OriginDay = Math.Max(1, originDay);
            Headline = headline ?? string.Empty;
            Description = description ?? string.Empty;
            Truthfulness = Clamp01(truthfulness);
            DecayRate = Clamp(decayRate, 0f, 1f);
            PropagationSpeed = Math.Clamp(propagationSpeed, 1, 6);
        }

        private static float Clamp01(float v)
            => float.IsNaN(v) ? 0f : Math.Clamp(v, 0f, 1f);

        private static float Clamp(float v, float lo, float hi)
            => float.IsNaN(v) ? lo : Math.Clamp(v, lo, hi);
    }

    /// <summary>
    /// Deterministic translation from a resolved moral choice to a rumor seed.
    /// Restrained by design: choices with visible public impact travel, intimate
    /// ones stay quiet, and suppression is an explicit input rather than a hidden
    /// rule. Pure — same inputs, same seed, every time.
    /// </summary>
    public static class MoralChoiceGossipSeed
    {
        /// <summary>Public choices spread; private ones do not become rumor fuel.</summary>
        public const float PublicImpactThreshold = 0.5f;

        /// <summary>Choices quieter than this never become rumors.</summary>
        public const float MinimumImpactToTravel = 0.25f;

        /// <summary>
        /// Build a seed, or return null when the choice is too quiet, too private,
        /// or explicitly suppressed. Suppression is a caller decision (a courier
        /// blackout, a jammed channel) so the rule stays inspectable.
        /// </summary>
        public static MoralGossipSeed? Build(
            string questId,
            string originLocationId,
            int resolvedDay,
            string epitaph,
            float publicImpact01,
            bool isPrivate,
            bool suppressed = false,
            float authoredDecayRate = 0.05f,
            int authoredPropagationSpeed = 1)
        {
            if (string.IsNullOrWhiteSpace(questId)) return null;
            if (isPrivate || suppressed) return null;

            float impact = float.IsNaN(publicImpact01) ? 0f : Math.Clamp(publicImpact01, 0f, 1f);
            if (impact < MinimumImpactToTravel) return null;

            // A decisive choice travels accurately; a marginal one gets garbled.
            float truthfulness = 0.45f + 0.45f * Math.Clamp(
                (impact - PublicImpactThreshold) / (1f - PublicImpactThreshold), 0f, 1f);
            float decay = authoredDecayRate * (2f - truthfulness); // untruth travels, fades
            int speed = authoredPropagationSpeed + (impact >= 0.8f ? 1 : 0);

            string headline = $"Word out of {originLocationId}: {questId}";
            string description = epitaph ?? string.Empty;

            return new MoralGossipSeed(
                questId, originLocationId, resolvedDay,
                headline, description, truthfulness, decay, speed);
        }
    }
}
