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
            IsOpen = false;
            OnWorkbenchUiChanged?.Invoke();
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

        public string BuildPanelText()
        {
            Refresh();
            return PanelSummary;
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder();
            sb.AppendLine("WORKBENCH");
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

            if (_lines.Count == 0)
            {
                sb.Append("No breakdown or repair actions available.");
                PanelSummary = sb.ToString();
                return;
            }

            for (int i = 0; i < _lines.Count; i++)
            {
                var line = _lines[i];
                string flag = line.CanExecute ? "[OK]" : "[--]";
                sb.Append(i).Append(". ").Append(flag).Append(' ')
                    .Append(line.Label).Append("  (").Append(line.CostSummary).Append(')')
                    .AppendLine();
            }
            PanelSummary = sb.ToString().TrimEnd();
        }
    }
}
