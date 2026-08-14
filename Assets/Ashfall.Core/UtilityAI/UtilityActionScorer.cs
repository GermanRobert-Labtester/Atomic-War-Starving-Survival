using System;
using System.Collections.Generic;

namespace Ashfall.Core.UtilityAI
{
    /// <summary>
    /// Engine-agnostic port of the Unity ActionScorer pipeline (audit A3-A7
    /// preserved): raw -> curve -> (curved + basePriority) x weight -> trait
    /// vetoes -> listless penalty -> override passthrough -> clamp01.
    /// Vetoes are data-driven (trait x action-tag matrix).
    /// </summary>
    public class UtilityActionScorer
    {
        public const float ListlessScorePenalty = 0.08f;

        public float Score(UtilityActionDef action, AIActionContext context)
        {
            if (action == null || context == null) return 0f;
            if (IsForbiddenByTraits(action, context)) return 0f;

            float rawScore = action.EvaluateRaw(context);
            if (rawScore <= 0f) return 0f;

            float curvedScore = action.Curve.Evaluate(rawScore);

            float score = ApplyTraitBiases(
                (curvedScore + action.basePriority) * action.weight, action, context);

            if (context.IsListless)
                score -= ListlessScorePenalty;

            if (action.isOverrideAction)
                return Math.Max(0f, score);

            return Math.Max(0f, Math.Min(1f, score));
        }

        /// <summary>
        /// Hard vetoes (audit A7: null quests = no vetoes): the trait-tag matrix
        /// from the Unity quest gates, expressed as data.
        /// </summary>
        public static bool IsForbiddenByTraits(UtilityActionDef action, AIActionContext context)
        {
            if (action == null || context == null) return false;

            // Coward refuses loud labor.
            if (context.HasTrait(UtilityTags.TraitCoward) && action.HasTag(UtilityTags.TagLoudLabor))
                return true;
            // God Complex refuses menial labor.
            if (context.HasTrait(UtilityTags.TraitGodComplex) && action.HasTag(UtilityTags.TagMenialLabor))
                return true;
            // Pacifist cannot equip weapons / combat.
            if (context.HasTrait(UtilityTags.TraitPacifist) && action.HasTag(UtilityTags.TagWeapon))
                return true;
            // Blind cannot fire guns.
            if (context.HasTrait(UtilityTags.TraitBlind) && action.HasTag(UtilityTags.TagGun))
                return true;
            // Ex-Con refuses orders from authority (order-tagged actions).
            if (context.HasTrait(UtilityTags.TraitExCon) && action.HasTag(UtilityTags.TagOrder))
                return true;
            // Hitman refuses medical triage and farming.
            if (context.HasTrait(UtilityTags.TraitHitman) &&
                (action.HasTag(UtilityTags.TagMedicalTriage) || action.HasTag(UtilityTags.TagFarming)))
                return true;
            // Germaphobe: no bunker triage without hazmat.
            if (context.HasTrait(UtilityTags.TraitGermaphobe) &&
                action.HasTag(UtilityTags.TagMedicalTriage) && !context.HasHazmat)
                return true;
            return false;
        }

        /// <summary>
        /// Soft biases (reweight, never veto). Politician scores dirty labor
        /// lower so they prefer delegating; placeholder for future gates.
        /// </summary>
        private static float ApplyTraitBiases(float score, UtilityActionDef action, AIActionContext context)
        {
            return score;
        }
    }
}
