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

                // #295 Hitman Professional: refuses medical triage and farming.
                if (context.PersonalQuests.RefusesMedicalAndFarming(context.Survivor)
                    && (context.PersonalQuests.IsMedicalTriageAction(actionId)
                        || context.PersonalQuests.IsFarmingAction(actionId)))
                    return 0f;

                // #289 Germaphobe: no bunker triage without hazmat (host sets HazmatEquipped).
                if (context.PersonalQuests.RequiresHazmatForTriage(context.Survivor)
                    && context.PersonalQuests.IsMedicalTriageAction(actionId)
                    && !context.PersonalQuests.CanPerformTriage(
                        context.Survivor,
                        hazmatEquipped: context.HazmatEquipped,
                        inBunker: !context.Survivor.IsOnExpedition))
                    return 0f;
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

                // #287 Custodian: clean waste/mold before thirst or hunger.
                if (context.PersonalQuests.PrioritizesCleaningOverNeeds(context.Survivor)
                    && context.PersonalQuests.IsCleaningAction(action.id))
                    score = Mathf.Max(score, 0.95f);

                // #288 Lumberjack low-morale salvage wood instead of repair.
                if (context.PersonalQuests.ShouldSalvageBrokenWoodWhenMoraleLow(
                        context.Survivor, context.Survivor.Needs.Morale)
                    && context.PersonalQuests.IsSalvageWoodAction(action.id))
                    score *= 1.8f;

                // #293 Musician: periodically prefer Play Instrument.
                if (context.PersonalQuests.CanPlayInstrument(context.Survivor)
                    && context.PersonalQuests.IsPlayInstrumentAction(action.id))
                    score = Mathf.Max(score, 0.55f);

                // #290 Astronomer: climb to surface hatch at night.
                if (context.IsNight
                    && context.PersonalQuests.SeeksSurfaceSkyAtNight(context.Survivor, isNight: true)
                    && IsSurfaceSkyAction(action.id))
                    score = Mathf.Max(score, 0.7f);

                // #290 Night Owl: action speed bias via utility (day slow / night fast).
                float nightOwl = context.PersonalQuests.GetNightOwlActionSpeedMultiplier(
                    context.Survivor, context.IsNight);
                if (!Mathf.Approximately(nightOwl, 1f))
                    score *= nightOwl;

                // #302 Gamer: works nights, sleeps days (coop task speed bias).
                float gamerCoop = context.PersonalQuests.GetGamerCoopTaskSpeedMultiplier(
                    context.Survivor, context.IsNight);
                if (!Mathf.Approximately(gamerCoop, 1f))
                    score *= gamerCoop;
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

        public static bool IsSurfaceSkyAction(string actionId)
        {
            if (string.IsNullOrEmpty(actionId)) return false;
            return actionId.IndexOf("surface", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("hatch", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("sky", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("stargaz", System.StringComparison.OrdinalIgnoreCase) >= 0;
        }


        /// <summary>Legacy single-survivor scoring signature.</summary>
        public float Score(SurvivorAction action, Survivor survivor)
        {
            var context = new AIContext(survivor);
            return Score(action, context);
        }
    }
}
