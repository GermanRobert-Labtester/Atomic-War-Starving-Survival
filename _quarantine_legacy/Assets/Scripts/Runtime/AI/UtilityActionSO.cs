using AtomicWar.Runtime.Survivors;
using UnityEngine;

namespace AtomicWar.Runtime.AI
{
    /// <summary>
    /// Base ScriptableObject class for data-driven Utility AI actions.
    /// Modular and easy to extend by overriding EvaluateScore and Execute.
    /// </summary>
    public abstract class UtilityActionSO : ScriptableObject
    {
        [Header("Action Identity")]
        public string ActionName;
        [TextArea] public string ActionDescription;

        [Header("Utility Curves & Weighting")]
        [Range(0f, 2f)] public float WeightMultiplier = 1.0f;
        public AnimationCurve UtilityCurve = AnimationCurve.Linear(0, 0, 1, 1);

        /// <summary>
        /// Calculates the utility score (0.0 to 100.0+) for a survivor in the current context.
        /// </summary>
        public abstract float EvaluateScore(SurvivorModel survivor, UtilityAIContext context);

        /// <summary>
        /// Executes the action when chosen as highest utility.
        /// </summary>
        public abstract void Execute(SurvivorModel survivor, UtilityAIContext context);
    }
}
