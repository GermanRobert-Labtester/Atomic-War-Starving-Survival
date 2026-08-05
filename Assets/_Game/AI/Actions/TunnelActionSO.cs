using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// AI action that drives a survivor to dig a tunnel to a neighbor
    /// (Prompt #124 — TunnelingSystem). Requires a pickaxe for full speed.
    /// Scores when the layout allows tunneling AND the neighbor has not
    /// been breached yet.
    /// </summary>
    [CreateAssetMenu(fileName = "NewTunnelAction", menuName = "ASHFALL/AI Actions/Tunnel")]
    public class TunnelActionSO : SurvivorAction
    {
        public TunnelActionSO()
        {
            id = "action_tunnel";
            displayName = "Tunnel to Neighbor";
            description = "Dig a tunnel into an adjacent ruin to loot it. Requires Pickaxe.";
            basePriority = 0.2f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (!MeetsPrerequisites(context)) return 0f;
            if (context.TunnelingSystem.HasHostiles) return 0.85f; // urgent: clear hostiles
            // Mid-priority construction project.
            float skill = context.Survivor.EffectiveCraftingSkill;
            return Mathf.Clamp01(0.2f + 0.2f * skill);
        }

        private static bool MeetsPrerequisites(AIContext context)
            => CanCraft(context)
               && context.TunnelingSystem != null
               && context.TunnelingSystem.CanTunnel
               && !context.TunnelingSystem.NeighborBreached;

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || context.TunnelingSystem == null) return;
            // 4 hours per call. The system applies the pickaxe-multiplier if
            // the inventory has one (caller responsibility).
            // Prompt #213 — Taskmaster Pacing Aura: +15% work rate nearby.
            float hours = 4f * GetPacingMult(context);
            context.TunnelingSystem.Tunnel(hours, context.Survivor, hasPickaxe: false);
        }

        private static float GetPacingMult(AIContext context)
        {
            if (context.SocialPerks == null || context.GetSurvivors == null) return 1f;
            return context.SocialPerks.GetPacingAuraMultiplier(
                context.Survivor,
                context.GetSurvivors(),
                context.AreRoomsAdjacent);
        }
    }
}
