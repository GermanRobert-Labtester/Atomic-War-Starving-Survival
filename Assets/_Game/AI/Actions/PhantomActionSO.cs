using System;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// AI action that has a survivor reach for one of their HallucinationSystem-tracked
    /// PhantomItems (Prompt #65). Name/id in the result message and event come from the
    /// actual consumed PhantomItem, not a fixed asset field — the spawned item is random.
    /// </summary>
    [CreateAssetMenu(fileName = "NewPhantomAction", menuName = "ASHFALL/AI Actions/Phantom Action")]
    public class PhantomActionSO : SurvivorAction
    {
        public event Action<Survivor, string> OnPhantomActionAttempted;

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;
            return ScoreAction(context.Survivor, context.HallucinationSystem);
        }

        public float ScoreAction(Survivor sv, HallucinationSystem hallucinations)
        {
            if (sv == null || !sv.IsAlive) return 0f;
            if (hallucinations == null || !hallucinations.HasActivePhantom(sv.Id)) return 0f;
            if (sv.Needs != null && sv.Needs.Health < 30f) return 0.85f;
            if (sv.RadiationAnxiety > 0.7f) return 0.80f;
            return 0f;
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null) return;
            ExecutePhantomAction(context.Survivor, context.HallucinationSystem, out _);
        }

        /// <summary>Consumes one of the survivor's active phantoms for zero real benefit.</summary>
        public bool ExecutePhantomAction(Survivor sv, HallucinationSystem hallucinations, out string resultMessage)
        {
            resultMessage = string.Empty;
            if (sv == null || !sv.IsAlive || hallucinations == null) return false;

            var phantoms = hallucinations.GetPhantomsForSurvivor(sv.Id);
            if (phantoms.Count == 0) return false;
            var phantom = phantoms[0];
            if (!hallucinations.ConsumePhantomItem(sv, phantom.Id)) return false;

            resultMessage = $"{sv.DisplayName} reached for {phantom.DisplayName}, but it dissolved into thin air.";
            OnPhantomActionAttempted?.Invoke(sv, phantom.Id);

            return true;
        }
    }
}
