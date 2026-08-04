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

            float rawScore = action.EvaluateRaw(context);
            if (rawScore <= 0f) return 0f;

            float curvedScore = action.responseCurve != null && action.responseCurve.length > 0
                ? action.responseCurve.Evaluate(rawScore)
                : rawScore;

            float score = (curvedScore + action.basePriority) * action.weight;

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


        /// <summary>Legacy single-survivor scoring signature.</summary>
        public float Score(SurvivorAction action, Survivor survivor)
        {
            var context = new AIContext(survivor);
            return Score(action, context);
        }
    }
}
