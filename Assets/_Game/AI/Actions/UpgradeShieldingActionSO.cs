using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// AI action that upgrades a room's ceiling shielding material
    /// (Prompt #125 — MaterialShieldingSystem). One upgrade per room; the
    /// ceiling attenuation goes from 0 → ~0.6 over a few upgrades.
    /// Scores when the room is at the lowest material and the survivor
    /// has the resources (lead/concrete scrap).
    /// </summary>
    [CreateAssetMenu(fileName = "NewUpgradeShieldingAction", menuName = "ASHFALL/AI Actions/Upgrade Shielding")]
    public class UpgradeShieldingActionSO : SurvivorAction
    {
        public UpgradeShieldingActionSO()
        {
            id = "action_upgrade_shielding";
            displayName = "Upgrade Ceiling Shielding";
            description = "Replace a room's ceiling material with a higher-attenuation option. Requires scrap.";
            basePriority = 0.25f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (!MeetsPrerequisites(context)) return 0f;

            // Higher score when ambient rad is high (the upgrade actually pays off).
            float radUrgency = Mathf.Clamp01(context.AmbientRadRate / 5f);
            return 0.25f + 0.3f * radUrgency;
        }

        private static bool MeetsPrerequisites(AIContext context)
            => CanCraft(context)
               && context.MaterialShieldingSystem != null
               && context.Shelter?.Rooms != null
               && HasUpgradeableRoom(context);

        private static bool HasUpgradeableRoom(AIContext context)
        {
            for (int i = 0; i < context.Shelter.Rooms.Count; i++)
            {
                var room = context.Shelter.Rooms[i];
                if (room == null) continue;
                if (context.MaterialShieldingSystem.GetCeilingAttenuation(room.RoomId) < 0.4f)
                    return true;
            }
            return false;
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || context.MaterialShieldingSystem == null) return;
            if (context.Shelter?.Rooms == null) return;

            string worstRoom = FindWorstShieldedRoom(context);
            if (worstRoom == null) return;

            context.MaterialShieldingSystem.UpgradeCeiling(
                worstRoom,
                MaterialShieldingSystem.WallMaterial.Concrete);
        }

        private static string FindWorstShieldedRoom(AIContext context)
        {
            string worstRoom = null;
            float worstAtten = 1f;
            for (int i = 0; i < context.Shelter.Rooms.Count; i++)
            {
                var room = context.Shelter.Rooms[i];
                if (room == null) continue;
                float att = context.MaterialShieldingSystem.GetCeilingAttenuation(room.RoomId);
                if (att < worstAtten)
                {
                    worstAtten = att;
                    worstRoom = room.RoomId;
                }
            }
            return worstRoom;
        }
    }
}
