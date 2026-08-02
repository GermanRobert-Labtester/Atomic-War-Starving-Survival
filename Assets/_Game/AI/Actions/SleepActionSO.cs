using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "Action_Sleep", menuName = "ASHFALL/AI/Sleep Action")]
    public class SleepActionSO : SurvivorAction
    {
        public SleepActionSO()
        {
            id = "action_sleep";
            displayName = "Sleep";
            description = "Rest in shelter to reduce fatigue.";
            basePriority = 0.15f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;
            return Mathf.Clamp01(context.Survivor.Needs.Fatigue / 100f);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor != null)
            {
                context.Survivor.Needs.Fatigue = Mathf.Max(0f, context.Survivor.Needs.Fatigue - 60f);
            }
        }
    }
}
