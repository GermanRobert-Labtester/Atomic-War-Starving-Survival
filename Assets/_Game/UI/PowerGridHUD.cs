using System.Text;
using UnityEngine;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Power budget panel: generation vs draw, source list, and priority toggles
    /// for each consumer module. Presentation is data-driven from PowerNetwork.
    /// </summary>
    public class PowerGridHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }

        public float TotalGeneration { get; private set; }
        public float TotalDraw { get; private set; }
        public float RequestedDraw { get; private set; }
        public bool IsBlackout { get; private set; }
        public bool IsLoadShedding { get; private set; }
        public float CarbonMonoxidePpm { get; private set; }

        public string BudgetSummary { get; private set; } = string.Empty;
        public string SourcesSummary { get; private set; } = string.Empty;
        public string ConsumersSummary { get; private set; } = string.Empty;

        private PowerNetwork _network;

        public void Bind(PowerNetwork network)
        {
            if (_network != null)
                _network.OnPowerStateChanged -= Refresh;

            _network = network;
            if (_network != null)
                _network.OnPowerStateChanged += Refresh;

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
        }

        public void Toggle()
        {
            if (IsOpen) Close();
            else Open();
        }

        public void Refresh()
        {
            if (_network == null)
            {
                TotalGeneration = TotalDraw = RequestedDraw = 0f;
                IsBlackout = IsLoadShedding = false;
                CarbonMonoxidePpm = 0f;
                BudgetSummary = "No power network.";
                SourcesSummary = string.Empty;
                ConsumersSummary = string.Empty;
                return;
            }

            TotalGeneration = _network.TotalGeneration;
            TotalDraw = _network.TotalDraw;
            RequestedDraw = _network.RequestedDraw;
            IsBlackout = _network.IsBlackout;
            IsLoadShedding = _network.IsLoadShedding;
            CarbonMonoxidePpm = _network.CarbonMonoxidePpm;

            var budget = new StringBuilder();
            budget.Append($"POWER  {TotalDraw:0}/{TotalGeneration:0} W");
            if (IsBlackout) budget.Append("  [BLACKOUT]");
            else if (IsLoadShedding) budget.Append("  [LOAD SHED]");
            else budget.Append("  [OK]");
            if (CarbonMonoxidePpm > 1f)
                budget.Append($"  CO {CarbonMonoxidePpm:0.0} ppm");
            BudgetSummary = budget.ToString();

            var sources = new StringBuilder();
            sources.AppendLine("Sources:");
            var srcList = _network.Sources;
            for (int i = 0; i < srcList.Count; i++)
            {
                var s = srcList[i];
                if (s == null) continue;
                string name = s.Definition != null ? s.Definition.DisplayName : s.SourceId;
                float watts = s.Definition != null ? s.Definition.OutputWatts : 0f;
                string state = !s.IsEnabled ? "OFF" : "ON";
                if (s.Definition != null && s.Definition.Kind == PowerSourceKind.Diesel)
                    sources.AppendLine($"  {name}: {watts:0}W [{state}] fuel={s.Fuel:0.0}");
                else if (s.Definition != null && s.Definition.Kind == PowerSourceKind.Bicycle)
                {
                    string pedal = s.HasPedaler ? s.PedalingSurvivorId : "idle";
                    sources.AppendLine($"  {name}: {watts:0}W [{state}] pedal={pedal}");
                }
                else
                    sources.AppendLine($"  {name}: {watts:0}W [{state}]");
            }
            SourcesSummary = sources.ToString().TrimEnd();

            var loads = new StringBuilder();
            loads.AppendLine("Loads (priority toggle):");
            var consumers = _network.Consumers;
            for (int i = 0; i < consumers.Count; i++)
            {
                var c = consumers[i];
                if (c == null) continue;
                string flag = !c.IsRequested ? "OFF" : c.IsShed ? "SHED" : "ON";
                loads.AppendLine(
                    $"  P{c.Priority} {c.DisplayName}: {c.Watts:0}W [{flag}]");
            }
            ConsumersSummary = loads.ToString().TrimEnd();
        }

        /// <summary>UI: cycle a module's priority 1→5.</summary>
        public int CyclePriority(string moduleId)
        {
            if (_network == null) return 0;
            int p = _network.CyclePriority(moduleId);
            Refresh();
            return p;
        }

        /// <summary>UI: set absolute priority.</summary>
        public void SetPriority(string moduleId, int priority)
        {
            _network?.SetPriority(moduleId, priority);
            Refresh();
        }

        /// <summary>UI: request/unrequest a module load.</summary>
        public void SetRequested(string moduleId, bool requested)
        {
            _network?.SetRequested(moduleId, requested);
            Refresh();
        }

        /// <summary>Full panel text for debug / OnGUI / tests.</summary>
        public string BuildPanelText()
        {
            Refresh();
            return BudgetSummary + "\n" + SourcesSummary + "\n" + ConsumersSummary;
        }

        private void OnDestroy()
        {
            if (_network != null)
                _network.OnPowerStateChanged -= Refresh;
        }
    }
}
