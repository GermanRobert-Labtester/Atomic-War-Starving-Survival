using AtomicWar._Game.Shelter;
using UnityEngine;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// Utility-AI bridge for the bunker maintenance terminal. It never repairs an
    /// asset directly: it only allows the specifically assigned survivor to claim
    /// the persisted work order, whose timed completion is owned by Shelter.
    /// </summary>
    [CreateAssetMenu(fileName = "Action_RepairWorkOrder", menuName = "ASHFALL/AI/Repair Work Order")]
    public sealed class RepairWorkOrderActionSO : SurvivorAction
    {
        public RepairWorkOrderActionSO()
        {
            id = "action_repair_work_order";
            displayName = "Repair Work Order";
            description = "Carry out the maintenance terminal's assigned repair task.";
            basePriority = 0f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (!HasLivingSurvivor(context) || context.RepairWorkOrderSystem == null)
                return 0f;
            if (!context.RepairWorkOrderSystem.CanSurvivorWork(context.Survivor))
                return 0f;

            var snapshot = context.RepairWorkOrderSystem.GetSnapshot();
            switch (snapshot.Priority)
            {
                case MaintenanceRepairPriority.Critical: return 0.9f;
                case MaintenanceRepairPriority.Low: return 0.3f;
                default: return 0.6f;
            }
        }

        public override void Execute(AIContext context)
        {
            if (context?.RepairWorkOrderSystem == null || context.Survivor == null) return;
            context.RepairWorkOrderSystem.TryStartWork(context.Survivor, out _);
        }
    }
}
