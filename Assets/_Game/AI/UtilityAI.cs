using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI
{
    /// <summary>
    /// Utility-AI engine (NOT an LLM at runtime). Each evaluation it scores the
    /// available SurvivorActions for a survivor via the ActionScorer and selects
    /// the highest-scoring one to execute. Deterministic when provided with a seeded RNG.
    /// </summary>
    public class UtilityAI
    {
        private readonly ActionScorer _scorer = new ActionScorer();
        private float _evaluationTimer;
        public float EvaluationInterval = 1f;

        public event System.Action<Survivor, SurvivorAction, float> OnActionSelected;

        public ActionScorer Scorer => _scorer;

        public void Tick(float deltaTimeSeconds)
        {
            if (deltaTimeSeconds <= 0f) return;
            _evaluationTimer += deltaTimeSeconds;
        }

        public bool ShouldEvaluate()
        {
            if (_evaluationTimer >= EvaluationInterval)
            {
                _evaluationTimer = 0f;
                return true;
            }
            return false;
        }

        public SurvivorAction SelectAction(AIContext context, IReadOnlyList<SurvivorAction> candidates)
        {
            if (context == null || candidates == null || candidates.Count == 0) return null;

            SurvivorAction bestAction = null;
            float bestScore = -1f;

            for (int i = 0; i < candidates.Count; i++)
            {
                var candidate = candidates[i];
                if (candidate == null) continue;

                float score = _scorer.Score(candidate, context);

                // Add small deterministic noise if random generator is present
                if (score > 0f && context.Random != null)
                {
                    float noise = (float)(context.Random.NextDouble() * 0.0001);
                    score += noise;
                }

                if (score > bestScore)
                {
                    bestScore = score;
                    bestAction = candidate;
                }
            }

            if (bestAction != null && context.Survivor != null)
            {
                OnActionSelected?.Invoke(context.Survivor, bestAction, Mathf.Max(0f, bestScore));
            }

            return bestAction;
        }

        /// <summary>Legacy selection signature.</summary>
        public SurvivorAction SelectAction(Survivor survivor, IReadOnlyList<SurvivorAction> candidates)
        {
            var context = new AIContext(survivor);
            return SelectAction(context, candidates);
        }
    }
}
