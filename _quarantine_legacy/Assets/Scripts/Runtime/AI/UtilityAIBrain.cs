using System.Collections.Generic;
using AtomicWar.Runtime.Survivors;
using UnityEngine;

namespace AtomicWar.Runtime.AI
{
    /// <summary>
    /// Evaluates utility scores across registered UtilityActionSO actions for a survivor.
    /// Selects and executes the highest-scoring action.
    /// </summary>
    public class UtilityAIBrain
    {
        private readonly List<UtilityActionSO> _availableActions;
        public UtilityActionSO CurrentAction { get; private set; }
        public float BestUtilityScore { get; private set; }

        public UtilityAIBrain(IEnumerable<UtilityActionSO> actions)
        {
            _availableActions = new List<UtilityActionSO>(actions);
        }

        public void AddAction(UtilityActionSO action)
        {
            if (action != null && !_availableActions.Contains(action))
            {
                _availableActions.Add(action);
            }
        }

        public UtilityActionSO EvaluateAndExecute(SurvivorModel survivor, UtilityAIContext context)
        {
            if (survivor == null || !survivor.IsAlive || _availableActions.Count == 0) return null;

            UtilityActionSO bestAction = null;
            float maxScore = float.MinValue;

            foreach (var action in _availableActions)
            {
                float score = action.EvaluateScore(survivor, context);
                if (score > maxScore)
                {
                    maxScore = score;
                    bestAction = action;
                }
            }

            CurrentAction = bestAction;
            BestUtilityScore = maxScore;

            if (bestAction != null)
            {
                bestAction.Execute(survivor, context);
            }

            return bestAction;
        }
    }
}
