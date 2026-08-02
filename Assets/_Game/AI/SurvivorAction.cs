using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI
{
    /// <summary>
    /// Base ScriptableObject describing a candidate Utility AI action (id, priority, weight, curve).
    /// Data-driven so action tuning and parameters can be adjusted without changing code.
    /// </summary>
    public abstract class SurvivorAction : ScriptableObject
    {
        public string id;
        public string displayName;
        [TextArea(2, 4)] public string description;
        public float basePriority = 0.1f;
        public float weight = 1.0f;
        public AnimationCurve responseCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        /// <summary>Calculates raw uncurved desirability score (0..1) given AIContext.</summary>
        public abstract float EvaluateRaw(AIContext context);

        /// <summary>Executes the action for the survivor.</summary>
        public virtual void Execute(AIContext context)
        {
            if (context?.Survivor != null)
            {
                Execute(context.Survivor);
            }
        }

        /// <summary>Legacy single-survivor execution overload.</summary>
        public virtual void Execute(Survivor survivor) { }
    }
}
