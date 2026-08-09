using UnityEngine;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// Guard Duty (Prompt #33): temporary ShelterSecurity boost, heavy Fatigue drain.
    /// </summary>
    [CreateAssetMenu(fileName = "Action_Guard", menuName = "ASHFALL/AI/Guard Action")]
    public class GuardActionSO : SurvivorAction
    {
        public GuardActionSO()
        {
            id = "action_guard";
            displayName = "Guard";
            description = "Watch the hatch. Boosts security; burns fatigue fast.";
            basePriority = 0.15f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;

            // Cannot fight / guard (child, incapacitated, etc.)
            if (context.Survivor.CannotFight) return 0f;

            // Too exhausted to stand a post
            if (context.Survivor.Needs.Fatigue >= 90f) return 0f;

            float averageNeed = (context.Survivor.Needs.Hunger + context.Survivor.Needs.Thirst + context.Survivor.Needs.Fatigue) / 3f;
            float readiness = averageNeed > 50f ? 0.05f : Mathf.Clamp01((50f - averageNeed) / 100f + 0.15f);

            // Prefer guard when raids are unlocked / threat is high / security is thin
            float threat = 0f;
            if (context.RaidThreatLevel > 0f)
                threat = Mathf.Clamp01(context.RaidThreatLevel);
            else if (context.CurrentDay >= HatchDefenseSystem.RaidUnlockDay)
                threat = 0.2f;

            if (context.HatchDefense != null)
            {
                float sec = context.HatchDefense.GetShelterSecurity();
                if (sec < 40f) threat = Mathf.Max(threat, 0.35f);
            }

            float score = Mathf.Clamp01(readiness + threat * 0.5f);
            // #252 Hardened Daughter: Utility AI heavily favors Guard.
            if (context.PersonalQuests != null)
                score = Mathf.Clamp01(score * context.PersonalQuests.GetTrainGuardUtilityBias(context.Survivor));
            return score;
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || !context.Survivor.IsAlive) return;

            if (context.HatchDefense != null)
            {
                context.HatchDefense.AssignGuard(context.Survivor);
                return;
            }

            // Fallback without hatch system: still pay the fatigue cost
            if (context.NeedsSystem != null)
                context.NeedsSystem.Modify(context.Survivor, NeedKind.Fatigue, HatchDefenseSystem.GuardFatigueDrain);
            else
                context.Survivor.Needs.Fatigue = Mathf.Clamp(context.Survivor.Needs.Fatigue + HatchDefenseSystem.GuardFatigueDrain, 0f, 100f);
            context.Survivor.State = SurvivorState.Working;
        }
    }
}
