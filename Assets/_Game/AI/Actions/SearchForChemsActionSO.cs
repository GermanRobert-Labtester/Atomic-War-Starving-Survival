using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// Panic chem-search action (Prompt #7 — Addiction/Withdrawal). When a
    /// survivor is in active withdrawal, their Utility AI overrides to this
    /// action: they tear through inventory/shelter looking for the addictive
    /// chem, destroying other items in their panic. This action scores
    /// extremely high during withdrawal, overriding all other actions.
    ///
    /// The actual item-destruction side effect is handled by AddictionSystem's
    /// PanicDestroyHandler; this action is the AI's decision to "search"
    /// (which is always the highest-priority action during withdrawal).
    /// </summary>
    [CreateAssetMenu(fileName = "NewSearchForChemsAction", menuName = "ASHFALL/AI Actions/Search For Chems")]
    public class SearchForChemsActionSO : SurvivorAction
    {
        [Header("Withdrawal Override")]
        [Tooltip("Base utility score when the survivor is in active withdrawal. " +
                 "Set high enough to override all other actions (e.g. 10+).")]
        [Range(1f, 20f)]
        public float withdrawalScore = 10f;

        public SearchForChemsActionSO()
        {
            id = "action_search_for_chems";
            displayName = "Search For Chems";
            description = "Frantically search the bunker for addictive chems during withdrawal.";
            basePriority = 1.0f;
            weight = 1.0f;
            isOverrideAction = true;
        }

        // -----------------------------------------------------------------
        // Scoring
        // -----------------------------------------------------------------

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;
            if (!context.Survivor.IsAlive) return 0f;

            // Only scores during active withdrawal
            if (!context.Survivor.IsInWithdrawal) return 0f;

            float score = withdrawalScore;

            // Scale with withdrawal severity: the longer without a dose, the more desperate
            float hoursInWithdrawal = context.Survivor.HoursSinceLastDose - AtomicWar._Game.Medical.AddictionSystem.WithdrawalThresholdHours;
            if (hoursInWithdrawal > 0f)
            {
                float desperation = Mathf.Clamp01(hoursInWithdrawal / 48f); // ramp over 48h
                score += desperation * 3f;
            }

            // Belief system multiplier: anxious survivors are more driven to find chems
            if (context.BeliefSystem != null)
            {
                float anxietyFactor = 1f + context.Survivor.RadiationAnxiety * 0.5f;
                score *= anxietyFactor;
            }

            return score;
        }

        // -----------------------------------------------------------------
        // Execution
        // -----------------------------------------------------------------

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null) return;

            // The actual item destruction is handled by AddictionSystem.PanicDestroyHandler
            // during its own Tick. This action just resolves the AI choice — the survivor
            // spends the tick searching frantically, which is inherently unproductive.
            // We apply a fatigue cost to represent the panic exertion.
            context.Survivor.Needs.Fatigue = Mathf.Clamp(
                context.Survivor.Needs.Fatigue + 2f, 0f, 100f);

            // Small morale hit: the search is a desperate, humiliating act
            context.Survivor.Needs.Morale = Mathf.Clamp(
                context.Survivor.Needs.Morale - 1f, 0f, 100f);
        }
    }
}
