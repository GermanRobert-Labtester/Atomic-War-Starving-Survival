using UnityEngine;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// AI action that has a survivor who witnessed a crime tell one more person
    /// about it (Prompt #839 — System_Gossip.SpreadRumor). A low-priority, petty
    /// social behavior layered on top of the automatic daily gossip tick; the
    /// actual spread is delegated to System_Gossip via AIContext.OnRequestSpreadGossip,
    /// since the AI assembly cannot reference Core types.
    /// </summary>
    [CreateAssetMenu(fileName = "Action_SpreadGossip", menuName = "ASHFALL/AI Actions/Spread Gossip Action")]
    public class SpreadGossipActionSO : SurvivorAction
    {
        public SpreadGossipActionSO()
        {
            id = "action_spread_gossip";
            displayName = "Spread Gossip";
            description = "Corner someone in the hall and tell them what you saw.";
            basePriority = 0.15f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null || !context.Survivor.IsAlive) return 0f;
            if (context.CanSpreadGossip == null || !context.CanSpreadGossip(context.Survivor)) return 0f;
            return 0.2f;
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null) return;
            context.OnRequestSpreadGossip?.Invoke(context.Survivor);
        }
    }
}
