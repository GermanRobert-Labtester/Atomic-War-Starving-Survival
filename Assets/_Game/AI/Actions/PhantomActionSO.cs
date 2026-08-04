using System;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "NewPhantomAction", menuName = "ASHFALL/AI Actions/Phantom Action")]
    public class PhantomActionSO : SurvivorAction
    {
        public string phantomItemId = "phantom_clean_water";
        public string phantomDisplayName = "Clean Water Bottle";

        public event Action<Survivor, string> OnPhantomActionAttempted;

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;
            return ScoreAction(context.Survivor);
        }

        public float ScoreAction(Survivor sv)
        {
            if (sv == null || !sv.IsAlive) return 0f;
            if (sv.Needs != null && sv.Needs.Health < 30f) return 0.85f;
            if (sv.RadiationAnxiety > 0.7f) return 0.80f;
            return 0f;
        }

        public bool ExecutePhantomAction(Survivor sv, out string resultMessage)
        {
            resultMessage = string.Empty;
            if (sv == null || !sv.IsAlive) return false;

            resultMessage = $"{sv.DisplayName} reached for {phantomDisplayName}, but it dissolved into thin air.";
            OnPhantomActionAttempted?.Invoke(sv, phantomItemId);

            return true;
        }
    }
}
