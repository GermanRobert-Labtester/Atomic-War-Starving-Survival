using System;
using System.Text;
using AtomicWar._Game.Survivors;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Terminal presentation for daily bunker rations. It only stores focus and
    /// emits adjustment intent; Core owns policy, stores, and daily consequences.
    /// </summary>
    public class BunkerRationingHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public RationResource SelectedResource { get; private set; } = RationResource.Food;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnBunkerRationingChanged;
        public event Action<RationResource, int> OnLevelAdjustmentRequested;

        private Func<BunkerRationingSnapshot> _getSnapshot;
        private BunkerRationingSnapshot _snapshot;

        public void Bind(Func<BunkerRationingSnapshot> getSnapshot)
        {
            _getSnapshot = getSnapshot;
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

        public bool ToggleSelectedResource()
        {
            SelectedResource = SelectedResource == RationResource.Food
                ? RationResource.Water
                : RationResource.Food;
            Refresh();
            return true;
        }

        public bool IncreaseSelected() => RequestAdjustment(1);
        public bool DecreaseSelected() => RequestAdjustment(-1);

        public void ReportAdjustment(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "Policy unchanged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnBunkerRationingChanged?.Invoke();
        }

        private bool RequestAdjustment(int direction)
        {
            if (!IsOpen || direction == 0) return false;
            if (OnLevelAdjustmentRequested == null)
            {
                LastOutcome = "Policy link offline.";
                Refresh();
                return false;
            }
            OnLevelAdjustmentRequested.Invoke(SelectedResource, direction);
            return true;
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("BUNKER RATION BOARD  [T] close  ·  [TAB] select  ·  [,/.] lower/raise");
            if (_snapshot == null)
            {
                sb.Append("\nPolicy data is unavailable.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nCREW: ").Append(_snapshot.SurvivorCount).Append(" living.");
            AppendResource(sb, RationResource.Food, _snapshot.FoodLevel, _snapshot.FoodOnHand,
                _snapshot.FoodRequired, _snapshot.ProjectedFoodCoverage, _snapshot.ProjectedHungerReduction);
            AppendResource(sb, RationResource.Water, _snapshot.WaterLevel, _snapshot.WaterOnHand,
                _snapshot.WaterRequired, _snapshot.ProjectedWaterCoverage, _snapshot.ProjectedThirstReduction);
            if (_snapshot.CleanCisternWaterOnHand > 0)
                sb.Append("\n  Water source: ").Append(_snapshot.InventoryWaterOnHand)
                    .Append(" carried + ").Append(_snapshot.CleanCisternWaterOnHand)
                    .Append(" purified cistern.");
            sb.Append("\n--- NEXT ISSUE ---");
            sb.Append("\nProjected relief: hunger -").Append(_snapshot.ProjectedHungerReduction.ToString("0.#"))
                .Append(" · thirst -").Append(_snapshot.ProjectedThirstReduction.ToString("0.#"));
            sb.Append("\nProjected morale: ").Append(FormatSigned(_snapshot.ProjectedMoraleDelta)).Append(" per survivor.");
            if (_snapshot.ProjectedFoodCoverage < 1f || _snapshot.ProjectedWaterCoverage < 1f)
                sb.Append("\nCAUTION: stores cannot cover this policy. The shortage is shared.");

            AppendLastIssue(sb, _snapshot.LastReport);
            if (!string.IsNullOrEmpty(LastOutcome)) sb.Append("\nREPORT: ").Append(LastOutcome);
            PanelSummary = sb.ToString();
        }

        private void AppendResource(StringBuilder sb, RationResource resource, RationLevel level, int onHand,
            int required, float coverage, float projectedRelief)
        {
            bool selected = resource == SelectedResource;
            sb.Append("\n").Append(selected ? "> " : "  ").Append(ResourceLabel(resource).ToUpperInvariant())
                .Append(": ").Append(LevelLabel(level)).Append("  ·  ")
                .Append(onHand).Append(" on hand / ").Append(required).Append(" required")
                .Append("  ·  ").Append((coverage * 100f).ToString("0")).Append("% covered")
                .Append("  ·  ").Append(resource == RationResource.Food ? "hunger -" : "thirst -")
                .Append(projectedRelief.ToString("0.#"));
        }

        private static void AppendLastIssue(StringBuilder sb, BunkerRationingReport report)
        {
            if (report == null) return;
            sb.Append("\n--- LAST ISSUE: DAY ").Append(report.Day).Append(" ---");
            sb.Append("\nFood ").Append(report.FoodIssued).Append("/").Append(report.FoodRequested)
                .Append(" · water ").Append(report.WaterIssued).Append("/").Append(report.WaterRequested)
                .Append(" · morale ").Append(FormatSigned(report.MoraleDeltaPerSurvivor)).Append(" each.");
        }

        private static string ResourceLabel(RationResource resource)
        {
            return resource == RationResource.Food ? "food" : "water";
        }

        private static string LevelLabel(RationLevel level)
        {
            switch (level)
            {
                case RationLevel.Restricted: return "RESTRICTED";
                case RationLevel.Full: return "FULL";
                default: return "STANDARD";
            }
        }

        private static string FormatSigned(float value)
        {
            return value >= 0f ? "+" + value.ToString("0.#") : value.ToString("0.#");
        }
    }
}
