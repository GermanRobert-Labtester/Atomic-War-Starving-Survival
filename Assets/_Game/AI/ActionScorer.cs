using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI
{
    /// <summary>
    /// Produces a normalized 0..1 utility score for a candidate action given a
    /// survivor's current needs and context. Pure function; no side effects.
    /// Uses responseCurve, basePriority, and weight from the SurvivorAction SO.
    /// </summary>
    public class ActionScorer
    {
        public float Score(SurvivorAction action, AIContext context)
        {
            if (action == null || context == null) return 0f;

            // #259 Coward: refuses loud labor (build / generator / maintain).
            // #266 God Complex: refuses menial labor (clean / dig / excavate).
            if (context.PersonalQuests != null && context.Survivor != null)
            {
                string actionId = action.id ?? string.Empty;
                if (context.PersonalQuests.RefusesLoudLabor(context.Survivor)
                    && context.PersonalQuests.IsLoudLaborAction(actionId))
                    return 0f;
                if (context.PersonalQuests.RefusesMenialLabor(context.Survivor)
                    && context.PersonalQuests.IsMenialLaborAction(actionId))
                    return 0f;
            }

            float rawScore = action.EvaluateRaw(context);
            if (rawScore <= 0f) return 0f;

            float curvedScore = action.responseCurve != null && action.responseCurve.length > 0
                ? action.responseCurve.Evaluate(rawScore)
                : rawScore;

            float score = (curvedScore + action.basePriority) * action.weight;

            // #262 Hyper-Empathetic: prioritizes Comfort/Talk over own survival.
            if (context.PersonalQuests != null && context.Survivor != null)
            {
                float comfortBias = context.PersonalQuests.GetComfortTalkUtilityBias(context.Survivor);
                if (comfortBias > 1f && IsComfortOrTalkAction(action.id))
                    score *= comfortBias;
            }

            // Listless penalty: light-deprived survivors are sluggish about everything.
            // Applied after curve so it can't inflate low-urgency scores, only drag them down.
            if (context.IsListless)
            {
                const float ListlessScorePenalty = 0.08f;
                score -= ListlessScorePenalty;
            }

            // Override actions (e.g. withdrawal SearchForChems) are not clamped;
            // they must reliably win against any 0..1 action.
            if (action.isOverrideAction)
                return Mathf.Max(0f, score);

            return Mathf.Clamp01(score);
        }

        /// <summary>#262 Comfort/Talk action ids that Hyper-Empathetic survivors prefer.</summary>
        public static bool IsComfortOrTalkAction(string actionId)
        {
            if (string.IsNullOrEmpty(actionId)) return false;
            return actionId.IndexOf("comfort", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("talk", System.StringComparison.OrdinalIgnoreCase) >= 0;
        }


        /// <summary>Legacy single-survivor scoring signature.</summary>
        public float Score(SurvivorAction action, Survivor survivor)
        {
            var context = new AIContext(survivor);
            return Score(action, context);
        }
    }
}
