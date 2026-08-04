using UnityEngine;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// Hunt Rats — Vermin suppression (Prompt #51). The survivor spends an hour
    /// hunting vermin, reducing pest level at a fatigue cost. Scores when
    /// infestation is active and food stores are threatened.
    /// </summary>
    [CreateAssetMenu(fileName = "NewHuntRatsAction", menuName = "ASHFALL/AI Actions/Hunt Rats")]
    public class HuntRatsActionSO : SurvivorAction
    {
        [Header("Hunt Rats")]
        [Tooltip("Base utility score when vermin are active.")]
        [Range(0f, 1f)]
        public float baseScore = 0.55f;

        [Tooltip("Score bonus per 10% of food stores that pests eat daily.")]
        [Range(0f, 0.5f)]
        public float foodThreatBonus = 0.2f;

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null || !context.Survivor.IsAlive) return 0f;
            if (context.Survivor.CannotFight) return 0f; // Child cannot hunt rats

            var vermin = context.VerminSystem;
            if (vermin == null || !vermin.IsInfested) return 0f;

            float score = baseScore;

            // Higher pest level = more urgent.
            score += (vermin.PestLevel / 100f) * 0.3f;

            // Food threat: more valuable when food stores are low.
            float foodFraction = vermin.DailyFoodTheftFraction;
            score += foodFraction * foodThreatBonus * 10f;

            // Survivalist and Reckless types are more willing to hunt vermin.
            if (context.Survivor.HasTrait("Survivalist"))
                score += 0.15f;

            // Low morale reduces willingness to do pest control.
            float moraleFactor = Mathf.Lerp(0.4f, 1f, context.Survivor.Needs.Morale / 100f);
            score *= moraleFactor;

            return Mathf.Clamp01(score);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null) return;
            var vermin = context.VerminSystem;
            if (vermin == null) return;

            float reduced = vermin.HuntRats(context.Survivor);

            if (reduced > 0f)
            {
                Debug.Log($"[HuntRats] {context.Survivor.DisplayName} hunted rats " +
                          $"(pest level reduced by {reduced:F0}).");
            }
        }
    }
}
