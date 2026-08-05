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

        [Header("Progression (Prompt #179)")]
        [Tooltip("snake_case discipline XP awarded on execute. Empty = no progression XP.")]
        public string progressionDiscipline;
        [Tooltip("Hidden XP granted each time this action is executed.")]
        public float progressionXp = 5f;

        /// <summary>
        /// When true, the final utility score is NOT clamped to 0..1.
        /// Use for override actions that must always win (e.g. withdrawal panic).
        /// The raw score should still be reasonable — the evaluator just skips Clamp01.
        /// </summary>
        public bool isOverrideAction;

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

        /// <summary>True when context has a living survivor actor.</summary>
        protected static bool HasLivingSurvivor(AIContext context)
            => context?.Survivor != null && context.Survivor.IsAlive;

        /// <summary>Living survivor who can craft/construct (not child/disabled).</summary>
        protected static bool CanCraft(AIContext context)
            => HasLivingSurvivor(context) && !context.Survivor.CannotCraft;

        /// <summary>Living survivor who can scavenge/haul.</summary>
        protected static bool CanScavenge(AIContext context)
            => HasLivingSurvivor(context) && !context.Survivor.CannotScavenge;
    }
}
