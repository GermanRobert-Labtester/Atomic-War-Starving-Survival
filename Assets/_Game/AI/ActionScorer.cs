using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI
{
    /// <summary>
    /// Produces a normalized 0..1 utility score for a candidate action given a
    /// survivor's current needs and context. Pure function; no side effects.
    /// Uses responseCurve, basePriority, and weight from the SurvivorAction SO.
    /// </summary>
    public class ActionScorer
    {
        public float Score(SurvivorAction action, AIContext context)
        {
            if (action == null || context == null) return 0f;

            // #259 Coward: refuses loud labor (build / generator / maintain).
            // #266 God Complex: refuses menial labor (clean / dig / excavate).
            // #275 Pacifist: cannot equip weapons / combat actions.
            // #271 Blind: cannot fire guns / expedition combat.
            // #277 Ex-Con: refuses orders from Cop/General (order-tagged actions).
            if (context.PersonalQuests != null && context.Survivor != null)
            {
                string actionId = action.id ?? string.Empty;
                if (context.PersonalQuests.RefusesLoudLabor(context.Survivor)
                    && context.PersonalQuests.IsLoudLaborAction(actionId))
                    return 0f;
                if (context.PersonalQuests.RefusesMenialLabor(context.Survivor)
                    && context.PersonalQuests.IsMenialLaborAction(actionId))
                    return 0f;
                if (context.PersonalQuests.CannotEquipWeapons(context.Survivor)
                    && IsWeaponOrCombatAction(actionId))
                    return 0f;
                if (!context.PersonalQuests.CanFireGuns(context.Survivor)
                    && IsGunAction(actionId))
                    return 0f;
                // #277 Ex-Con: refuse order-tagged actions when order-giver is Cop/General.
                // Order-giver is approximated as any living Cop/General in the bunker.
                if (IsOrderAction(actionId) && context.GetSurvivors != null)
                {
                    var all = context.GetSurvivors();
                    if (all != null)
                    {
                        for (int oi = 0; oi < all.Count; oi++)
                        {
                            var giver = all[oi];
                            if (giver == null || !giver.IsAlive) continue;
                            if (context.PersonalQuests.RefusesOrdersFrom(context.Survivor, giver))
                                return 0f;
                        }
                    }
                }
                // #279 Politician still *can* do dirty labor (quest needs it) but
                // scores it lower so they prefer delegating when alternatives exist.
            }

            float rawScore = action.EvaluateRaw(context);
            if (rawScore <= 0f) return 0f;

            float curvedScore = action.responseCurve != null && action.responseCurve.length > 0
                ? action.responseCurve.Evaluate(rawScore)
                : rawScore;

            float score = (curvedScore + action.basePriority) * action.weight;

            // #262 Hyper-Empathetic: prioritizes Comfort/Talk over own survival.
            if (context.PersonalQuests != null && context.Survivor != null)
            {
                float comfortBias = context.PersonalQuests.GetComfortTalkUtilityBias(context.Survivor);
                if (comfortBias > 1f && IsComfortOrTalkAction(action.id))
                    score *= comfortBias;
                // #276 Widow: hydroponics over sleep.
                if (context.PersonalQuests.PrioritizesHydroponicsOverSleep(context.Survivor)
                    && context.PersonalQuests.IsHydroponicsAction(action.id))
                    score *= 2f;
                // #277 Ex-Con: physical labor speed/utility bias.
                float laborMult = context.PersonalQuests.GetExConPhysicalLaborMultiplier(context.Survivor);
                if (laborMult > 1f && IsPhysicalLaborAction(action.id))
                    score *= laborMult;
                // #276 Grieving: action efficiency drag.
                float grief = context.PersonalQuests.GetGrievingActionEfficiencyMultiplier(context.Survivor);
                if (grief < 1f)
                    score *= grief;
                // #279 Politician: dirty labor is distasteful (lower utility, still possible).
                if (context.PersonalQuests.TriesToDelegateTasks(context.Survivor)
                    && context.PersonalQuests.IsDirtyLaborAction(action.id))
                    score *= 0.35f;
            }

            // Listless penalty: light-deprived survivors are sluggish about everything.
            // Applied after curve so it can't inflate low-urgency scores, only drag them down.
            if (context.IsListless)
            {
                const float ListlessScorePenalty = 0.08f;
                score -= ListlessScorePenalty;
            }

            // Override actions (e.g. withdrawal SearchForChems) are not clamped;
            // they must reliably win against any 0..1 action.
            if (action.isOverrideAction)
                return Mathf.Max(0f, score);

            return Mathf.Clamp01(score);
        }

        /// <summary>#262 Comfort/Talk action ids that Hyper-Empathetic survivors prefer.</summary>
        public static bool IsComfortOrTalkAction(string actionId)
        {
            if (string.IsNullOrEmpty(actionId)) return false;
            return actionId.IndexOf("comfort", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("talk", System.StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool IsWeaponOrCombatAction(string actionId)
        {
            if (string.IsNullOrEmpty(actionId)) return false;
            return actionId.IndexOf("shoot", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("melee", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("attack", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("suppress", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("fight", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("weapon", System.StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool IsGunAction(string actionId)
        {
            if (string.IsNullOrEmpty(actionId)) return false;
            return actionId.IndexOf("shoot", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("gun", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("rifle", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("suppress", System.StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool IsOrderAction(string actionId)
        {
            if (string.IsNullOrEmpty(actionId)) return false;
            return actionId.IndexOf("order", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("command", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("assign", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("guard", System.StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool IsPhysicalLaborAction(string actionId)
        {
            if (string.IsNullOrEmpty(actionId)) return false;
            return actionId.IndexOf("dig", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("excavat", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("haul", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("build", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("clear", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("tunnel", System.StringComparison.OrdinalIgnoreCase) >= 0;
        }


        /// <summary>Legacy single-survivor scoring signature.</summary>
        public float Score(SurvivorAction action, Survivor survivor)
        {
            var context = new AIContext(survivor);
            return Score(action, context);
        }
    }
}
