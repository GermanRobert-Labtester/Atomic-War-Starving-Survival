using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Unified workbench screen: dynamically lists disassemble / repair / recalibrate
    /// lines from inventory ItemDefinitions (ScrapValue + RepairRecipe). No hardcoded
    /// item tables — costs come from <see cref="WorkbenchSystem.BuildLines"/>.
    /// </summary>
    public class WorkbenchUI : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public string PanelSummary { get; private set; } = string.Empty;
        public IReadOnlyList<WorkbenchLine> Lines => _lines;

        public event Action OnWorkbenchUiChanged;

        private WorkbenchSystem _workbench;
        private readonly List<WorkbenchLine> _lines = new List<WorkbenchLine>();

        public void Bind(WorkbenchSystem workbench)
        {
            if (_workbench != null)
                _workbench.OnWorkbenchChanged -= Refresh;

            _workbench = workbench;
            if (_workbench != null)
                _workbench.OnWorkbenchChanged += Refresh;

            Refresh();
        }

        public void Open()
        {
            IsOpen = true;
            Refresh();
        }

        public void Close()
        {
            if (!IsOpen) return; // idempotent: skip the rebuild and event on no-op closes
            IsOpen = false;
            // Refresh through the normal path so the panel's PanelSummary and
            // line list are reset to a known state before the event fires.
            // Skipping this leaves stale text on the model for the next open.
            Refresh();
        }

        public void Toggle()
        {
            if (IsOpen) Close();
            else Open();
        }

        public void Refresh()
        {
            _lines.Clear();
            if (_workbench != null)
            {
                var built = _workbench.BuildLines();
                for (int i = 0; i < built.Count; i++)
                    _lines.Add(built[i]);
            }
            RebuildPanel();
            OnWorkbenchUiChanged?.Invoke();
        }

        /// <summary>Execute a line by index (disassemble / repair / recalibrate).</summary>
        public bool Execute(int lineIndex)
        {
            if (_workbench == null || lineIndex < 0 || lineIndex >= _lines.Count) return false;
            bool ok = _workbench.ExecuteLine(_lines[lineIndex]);
            Refresh();
            return ok;
        }

        public bool Disassemble(ItemDefinition item)
        {
            if (_workbench == null) return false;
            bool ok = _workbench.Disassemble(item);
            Refresh();
            return ok;
        }

        public bool RepairFirst(string itemId)
        {
            if (_workbench == null) return false;
            bool ok = _workbench.RepairFirst(itemId);
            Refresh();
            return ok;
        }

        public bool RecalibrateGeiger()
        {
            if (_workbench == null) return false;
            bool ok = _workbench.RecalibrateGeiger();
            Refresh();
            return ok;
        }

        /// <summary>Install or level a hatch upgrade by module id (from hatch install lines).</summary>
        public bool InstallHatchUpgrade(string moduleId)
        {
            if (_workbench == null) return false;
            bool ok = _workbench.InstallHatchUpgrade(moduleId);
            Refresh();
            return ok;
        }

        /// <summary>Count of hatch install/upgrade lines currently listed.</summary>
        public int HatchInstallLineCount
        {
            get
            {
                int n = 0;
                for (int i = 0; i < _lines.Count; i++)
                    if (_lines[i].Kind == WorkbenchActionKind.InstallHatch) n++;
                return n;
            }
        }

        public string BuildPanelText()
        {
            Refresh();
            return PanelSummary;
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder();
            sb.AppendLine("WORKBENCH  [B] toggle  ·  [1-9] execute");
            if (_workbench == null)
            {
                sb.Append("No workbench bound.");
                PanelSummary = sb.ToString();
                return;
            }

            if (!_workbench.HasOperationalWorkbench())
                sb.AppendLine("[Station offline]");

            if (_workbench.NeedsElectronicScrapForCriticalRepair())
                sb.AppendLine("! Critical repair needs ElectronicScrap — scavenge junk.");

            if (_workbench.HatchDefense != null)
                sb.AppendLine("Hatch installs available below (scrap + mechanical parts).");

            if (_lines.Count == 0)
            {
                sb.Append("No breakdown, repair, or hatch install actions available.");
                PanelSummary = sb.ToString();
                return;
            }

            bool wroteHatchHeader = false;
            for (int i = 0; i < _lines.Count; i++)
            {
                var line = _lines[i];
                if (line.Kind == WorkbenchActionKind.InstallHatch && !wroteHatchHeader)
                {
                    sb.AppendLine("--- HATCH DEFENSE ---");
                    wroteHatchHeader = true;
                }
                string flag = line.CanExecute ? "[OK]" : "[--]";
                // 1-based index for keybind UX (1-9)
                sb.Append(i + 1).Append(". ").Append(flag).Append(' ')
                    .Append(line.Label).Append("  (").Append(line.CostSummary).Append(')')
                    .AppendLine();
            }
            PanelSummary = sb.ToString().TrimEnd();
        }
    }
}
