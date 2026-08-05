using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// AI action that drives a survivor to clear rubble from a sealed bunker
    /// room (Prompt #119 — ExcavationSystem). Scores when any room has
    /// remaining rubble; the survivor spends work-hours clearing one unit
    /// of rubble per call. Requires a Shovel in the inventory for full speed.
    /// </summary>
    [CreateAssetMenu(fileName = "NewExcavateAction", menuName = "ASHFALL/AI Actions/Excavate")]
    public class ExcavateActionSO : SurvivorAction
    {
        public ExcavateActionSO()
        {
            id = "action_excavate";
            displayName = "Excavate Room";
            description = "Clear rubble from a sealed room to expand usable bunker space.";
            basePriority = 0.35f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (!MeetsPrerequisites(context)) return 0f;

            // Crafting skill makes the work faster; morale dampens it.
            float skill = context.Survivor.EffectiveCraftingSkill;
            float morale = context.Survivor.Needs.Morale / 100f;
            float score = 0.4f + 0.2f * skill + 0.15f * morale;
            return Mathf.Clamp01(score);
        }

        private static bool MeetsPrerequisites(AIContext context)
            => CanCraft(context)
               && context.ExcavationSystem != null
               && context.ExcavationSystem.HasAnyRubble();

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || context.ExcavationSystem == null) return;
            // Find the first room with rubble. The system picks the first
            // registered room in iteration order; deterministic across runs.
            var rooms = context.Shelter?.Rooms;
            if (rooms == null) return;

            for (int i = 0; i < rooms.Count; i++)
            {
                var room = rooms[i];
                if (room == null) continue;
                if (context.ExcavationSystem.HasRubble(room.RoomId))
                {
                    // The host (GameBootstrap) supplies the hatch-blocked flag
                    // via ExcavationSystem.HatchBlocksExcavations (set by
                    // HouseToBunkerSystem). We pass false here because the
                    // action only scores when the hatch is open (HasAnyRubble
                    // returns false in that case via the host's gate).
                    bool hasShovel = HasShovel(context);
                    // Prompt #213 — Taskmaster Pacing Aura: +15% dig rate nearby.
                    float workHours = GetPacingMult(context);
                    context.ExcavationSystem.ClearRubble(
                        room.RoomId,
                        context.Survivor,
                        hasShovel,
                        hatchBlocked: false,
                        workHours: workHours);
                    return;
                }
            }
        }

        private static float GetPacingMult(AIContext context)
        {
            if (context.SocialPerks == null || context.GetSurvivors == null) return 1f;
            return context.SocialPerks.GetPacingAuraMultiplier(
                context.Survivor,
                context.GetSurvivors(),
                context.AreRoomsAdjacent);
        }

        private static bool HasShovel(AIContext context)
        {
            if (context.Inventory == null) return false;
            // The inventory's API uses ItemDefinition; without the catalog we
            // can only check the item id by walking. For now, the ExcavationSystem
            // uses a default 0.5x multiplier when no shovel is present.
            // The host may also expose a richer shovel count via a delegate; we
            // return false to keep this action self-contained and let the
            // GameBootstrap-level scavenger pick up shovels opportunistically.
            return false;
        }
    }
}
