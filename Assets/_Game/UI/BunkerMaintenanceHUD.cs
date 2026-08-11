using System;
using System.Text;
using AtomicWar._Game.Shelter;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Presentation-only bunker maintenance terminal. Core receives the selected
    /// target, survivor, urgency, and repair intents; this widget never mutates
    /// shelter condition or inventory on its own.
    /// </summary>
    public class BunkerMaintenanceHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedTargetIndex { get; private set; }
        public int SelectedSurvivorIndex { get; private set; }
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnBunkerMaintenanceChanged;
        public event Action<MaintenanceTargetType, string> OnRepairRequested;
        public event Action OnRepairCancellationRequested;
        public event Action<string> OnSurvivorAssignmentRequested;
        public event Action<int> OnPriorityAdjustmentRequested;

        private Func<BunkerMaintenanceSnapshot> _getSnapshot;
        private Func<RepairWorkOrderSnapshot> _getWorkOrderSnapshot;
        private Func<System.Collections.Generic.IReadOnlyList<AtomicWar._Game.Survivors.Survivor>> _getSurvivors;
        private BunkerMaintenanceSnapshot _snapshot;

        public void Bind(
            Func<BunkerMaintenanceSnapshot> getSnapshot,
            Func<System.Collections.Generic.IReadOnlyList<AtomicWar._Game.Survivors.Survivor>> getSurvivors,
            Func<RepairWorkOrderSnapshot> getWorkOrderSnapshot = null)
        {
            _getSnapshot = getSnapshot;
            _getSurvivors = getSurvivors;
            _getWorkOrderSnapshot = getWorkOrderSnapshot;
            Refresh();
        }

        public void Open()
        {
            if (IsOpen) return;
            IsOpen = true;
            Refresh();
        }

        public void Close()
        {
            if (!IsOpen) return;
            IsOpen = false;
            Refresh();
        }

        public void Toggle()
        {
            if (IsOpen) Close();
            else Open();
        }

        public bool SelectNextTarget() => CycleTarget(1);
        public bool SelectPreviousTarget() => CycleTarget(-1);

        public bool SelectNextSurvivor() => CycleSurvivor(1);
        public bool SelectPreviousSurvivor() => CycleSurvivor(-1);

        public bool IncreasePriority() => RequestPriority(1);
        public bool DecreasePriority() => RequestPriority(-1);

        public bool RepairSelected()
        {
            if (!IsOpen || _snapshot == null || _snapshot.Targets == null || _snapshot.Targets.Count == 0)
                return false;
            var target = _snapshot.Targets[Mathf.Clamp(SelectedTargetIndex, 0, _snapshot.Targets.Count - 1)];
            if (target == null || OnRepairRequested == null)
            {
                ReportOutcome("Maintenance link offline.");
                return false;
            }
            OnRepairRequested.Invoke(target.TargetType, target.TargetId);
            return true;
        }

        public bool CancelRepairOrder()
        {
            if (!IsOpen) return false;
            if (OnRepairCancellationRequested == null)
            {
                ReportOutcome("Maintenance link offline.");
                return false;
            }
            OnRepairCancellationRequested.Invoke();
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No maintenance change recorded." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            ClampSelection();
            RebuildPanel();
            OnBunkerMaintenanceChanged?.Invoke();
        }

        private bool CycleTarget(int direction)
        {
            if (!IsOpen || _snapshot == null || _snapshot.Targets == null || _snapshot.Targets.Count == 0)
                return false;
            SelectedTargetIndex = Wrap(SelectedTargetIndex + direction, _snapshot.Targets.Count);
            LastOutcome = "Selected " + SelectedTarget().DisplayName + ".";
            Refresh();
            return true;
        }

        private bool CycleSurvivor(int direction)
        {
            if (!IsOpen) return false;
            var survivors = _getSurvivors != null ? _getSurvivors() : null;
            if (survivors == null || survivors.Count == 0 || OnSurvivorAssignmentRequested == null)
            {
                ReportOutcome("No living repair worker is available.");
                return false;
            }

            int attempts = survivors.Count;
            while (attempts-- > 0)
            {
                SelectedSurvivorIndex = Wrap(SelectedSurvivorIndex + direction, survivors.Count);
                var survivor = survivors[SelectedSurvivorIndex];
                if (survivor == null || !survivor.IsAlive || string.IsNullOrEmpty(survivor.Id)) continue;
                OnSurvivorAssignmentRequested.Invoke(survivor.Id);
                return true;
            }

            ReportOutcome("No living repair worker is available.");
            return false;
        }

        private bool RequestPriority(int direction)
        {
            if (!IsOpen || direction == 0) return false;
            if (OnPriorityAdjustmentRequested == null)
            {
                ReportOutcome("Maintenance link offline.");
                return false;
            }
            OnPriorityAdjustmentRequested.Invoke(direction);
            return true;
        }

        private void ClampSelection()
        {
            int targetCount = _snapshot != null && _snapshot.Targets != null ? _snapshot.Targets.Count : 0;
            SelectedTargetIndex = targetCount > 0 ? Mathf.Clamp(SelectedTargetIndex, 0, targetCount - 1) : 0;

            var survivors = _getSurvivors != null ? _getSurvivors() : null;
            int survivorCount = survivors != null ? survivors.Count : 0;
            SelectedSurvivorIndex = survivorCount > 0 ? Mathf.Clamp(SelectedSurvivorIndex, 0, survivorCount - 1) : 0;
        }

        private BunkerMaintenanceTargetSnapshot SelectedTarget()
        {
            return _snapshot != null && _snapshot.Targets != null && _snapshot.Targets.Count > 0
                ? _snapshot.Targets[Mathf.Clamp(SelectedTargetIndex, 0, _snapshot.Targets.Count - 1)]
                : null;
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("BUNKER MAINTENANCE  [N] close  ·  [Tab] target  ·  [Shift+Tab] worker  ·  [,/.] priority  ·  [Enter] queue  ·  [Backspace] cancel");
            if (_snapshot == null)
            {
                sb.Append("\nMaintenance telemetry is unavailable.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nSTORES: mechanical parts ").Append(_snapshot.MechanicalPartsOnHand)
                .Append("  ·  electronic scrap ").Append(_snapshot.ElectronicScrapOnHand);
            if (_snapshot.RepairsBlockedByPowerFailure)
                sb.Append("\nGRID INTERLOCK: ACTIVE POWER FAILURE — repairs locked.");
            else
                sb.Append("\nGRID INTERLOCK: clear.");

            sb.Append("\nORDER: ").Append(_snapshot.HasAssignedLivingSurvivor
                ? _snapshot.AssignedSurvivorName
                : "UNASSIGNED")
                .Append("  ·  ").Append(PriorityLabel(_snapshot.RepairPriority));

            AppendWorkOrder(sb, _getWorkOrderSnapshot != null ? _getWorkOrderSnapshot() : null);

            if (_snapshot.Targets == null || _snapshot.Targets.Count == 0)
            {
                sb.Append("\nNo installed assets report to this terminal.");
            }
            else
            {
                for (int i = 0; i < _snapshot.Targets.Count; i++)
                    AppendTarget(sb, _snapshot.Targets[i], i == SelectedTargetIndex);
            }

            sb.Append("\nA full blackout cancels repair work before materials are spent.");
            if (!string.IsNullOrEmpty(LastOutcome)) sb.Append("\nREPORT: ").Append(LastOutcome);
            PanelSummary = sb.ToString();
        }

        private static void AppendTarget(StringBuilder sb, BunkerMaintenanceTargetSnapshot target, bool selected)
        {
            if (target == null) return;
            sb.Append("\n").Append(selected ? "> " : "  ").Append(target.DisplayName.ToUpperInvariant())
                .Append(": ").Append(target.Condition.ToString("0")).Append("% ");
            if (target.IsDestroyed) sb.Append("DESTROYED");
            else if (target.Condition >= 100f) sb.Append("SERVICED");
            else sb.Append(target.CanRepair ? "WORN" : "OFFLINE");
            sb.Append("  ·  ").Append(FormatMaterials(target.Materials));
            if (target.CanRepair && !target.HasRequiredMaterials) sb.Append(" [MISSING]");
        }

        private static void AppendWorkOrder(StringBuilder sb, RepairWorkOrderSnapshot order)
        {
            if (order == null)
            {
                sb.Append("\nWORK ORDER: link unavailable.");
                return;
            }
            if (!order.HasActiveOrder)
            {
                sb.Append("\nWORK ORDER: none.");
                if (!string.IsNullOrEmpty(order.LastReport))
                    sb.Append("  ·  ").Append(order.LastReport);
                return;
            }

            sb.Append("\nWORK ORDER: ").Append(order.Status == RepairWorkOrderStatus.Working ? "WORKING" : "QUEUED")
                .Append("  ·  ").Append(order.TargetDisplayName)
                .Append("  ·  ").Append(order.AssignedSurvivorName)
                .Append("  ·  ").Append(order.ProgressHours.ToString("0.0"))
                .Append("/").Append(order.RequiredWorkHours.ToString("0.0")).Append("h")
                .Append("  ·  ").Append(PriorityLabel(order.Priority));
            if (!string.IsNullOrEmpty(order.LastReport))
                sb.Append("\nWORK REPORT: ").Append(order.LastReport);
        }

        private static string FormatMaterials(System.Collections.Generic.List<BunkerMaintenanceMaterialRequirement> materials)
        {
            if (materials == null || materials.Count == 0) return "no material cost";
            var sb = new StringBuilder("needs ");
            for (int i = 0; i < materials.Count; i++)
            {
                if (i > 0) sb.Append(" + ");
                var material = materials[i];
                if (material == null) continue;
                sb.Append(material.Amount).Append(" ").Append(MaterialLabel(material.ItemId));
            }
            return sb.ToString();
        }

        private static string PriorityLabel(MaintenanceRepairPriority priority)
        {
            switch (priority)
            {
                case MaintenanceRepairPriority.Critical: return "CRITICAL";
                case MaintenanceRepairPriority.Low: return "LOW";
                default: return "STANDARD";
            }
        }

        private static string MaterialLabel(string itemId)
        {
            if (itemId == AtomicWar._Game.Inventory.ScrapMaterialIds.MechanicalParts) return "mechanical parts";
            if (itemId == AtomicWar._Game.Inventory.ScrapMaterialIds.ElectronicScrap) return "electronic scrap";
            return itemId;
        }

        private static int Wrap(int value, int count)
        {
            if (count <= 0) return 0;
            value %= count;
            return value < 0 ? value + count : value;
        }
    }
}
