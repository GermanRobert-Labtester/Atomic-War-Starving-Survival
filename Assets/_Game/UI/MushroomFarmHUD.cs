#pragma warning disable CS0067 // Public API event surface; subscribers arrive with feature wiring
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class MushroomBedSnapshot
    {
        public string bedId;
        public string mushroomSpecies; // e.g. "dark_spore_caps", "glow_gill"
        public float sporeGrowthPercent; // 0..100
        public float compostMoisturePercent;
        public float yieldKg;
        public bool isReadyToHarvest;
    }

    public class MushroomFarmSnapshot
    {
        public float totalHarvestedKg;
        public List<MushroomBedSnapshot> beds = new List<MushroomBedSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Dark Spore Mushroom Farm HUD view-model.
    /// Monitors subterranean mushroom cultivation (Dark Spore, Glow Gill), compost moisture levels,
    /// dark room humidity, spore harvest yields, and toxic mold outbreaks.
    /// </summary>
    public class MushroomFarmHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedBedIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnMushroomFarmChanged;
        public event Action<string> OnMoistenCompostRequested; // (bedId)
        public event Action<string> OnHarvestMushroomsRequested; // (bedId)

        private Func<MushroomFarmSnapshot> _getSnapshot;
        private MushroomFarmSnapshot _snapshot;

        public void Bind(Func<MushroomFarmSnapshot> getSnapshot)
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

        public bool SelectNextBed()
        {
            if (!IsOpen || _snapshot == null || _snapshot.beds == null || _snapshot.beds.Count == 0)
                return false;
            SelectedBedIndex = (SelectedBedIndex + 1) % _snapshot.beds.Count;
            ReportOutcome("Selected mushroom bed: " + GetSelectedBedName());
            return true;
        }

        public bool SelectPreviousBed()
        {
            if (!IsOpen || _snapshot == null || _snapshot.beds == null || _snapshot.beds.Count == 0)
                return false;
            SelectedBedIndex = (SelectedBedIndex - 1 + _snapshot.beds.Count) % _snapshot.beds.Count;
            ReportOutcome("Selected mushroom bed: " + GetSelectedBedName());
            return true;
        }

        public bool RequestHarvestMushrooms()
        {
            if (!IsOpen || _snapshot == null || _snapshot.beds == null || _snapshot.beds.Count == 0)
            {
                ReportOutcome("No mushroom bed selected for harvest.");
                return false;
            }

            var bed = GetSelectedBed();
            if (bed == null) return false;

            if (!bed.isReadyToHarvest)
            {
                ReportOutcome("Mushroom Bed " + bed.bedId + " spores are not ready (" + bed.sporeGrowthPercent.ToString("0") + "%).");
                return false;
            }

            if (OnHarvestMushroomsRequested == null)
            {
                ReportOutcome("Mushroom farm harvest link offline.");
                return false;
            }

            OnHarvestMushroomsRequested.Invoke(bed.bedId);
            ReportOutcome("Harvesting " + bed.yieldKg.ToString("0.#") + " kg of " + bed.mushroomSpecies + " from Bed " + bed.bedId + "!");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No mushroom farm action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnMushroomFarmChanged?.Invoke();
        }

        private MushroomBedSnapshot GetSelectedBed()
        {
            if (_snapshot != null && _snapshot.beds != null && SelectedBedIndex >= 0 && SelectedBedIndex < _snapshot.beds.Count)
            {
                return _snapshot.beds[SelectedBedIndex];
            }
            return null;
        }

        private string GetSelectedBedName()
        {
            var b = GetSelectedBed();
            return b != null ? (b.bedId + " — " + b.mushroomSpecies) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("SUBTERRANEAN MUSHROOM CULTIVATION  [M] close  ·  [Tab] cycle  ·  [H] harvest mushrooms");

            if (_snapshot == null)
            {
                sb.Append("\nMushroom farm telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nFARM STATS: Total Harvested Mushrooms: ").Append(_snapshot.totalHarvestedKg.ToString("0.#")).Append(" kg");

            sb.Append("\n\nDARK ROOM COMPOST BEDS:");
            if (_snapshot.beds == null || _snapshot.beds.Count == 0)
            {
                sb.Append("\n  No mushroom beds active in dark room.");
            }
            else
            {
                for (int i = 0; i < _snapshot.beds.Count; i++)
                {
                    var bed = _snapshot.beds[i];
                    if (bed == null) continue;

                    bool selected = (i == SelectedBedIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append("Bed ").Append(bed.bedId)
                      .Append(" — Species: ").Append(bed.mushroomSpecies ?? "Dark Spore Caps")
                      .Append(" — Growth: ").Append(bed.sporeGrowthPercent.ToString("0")).Append("%")
                      .Append(" | Moisture: ").Append(bed.compostMoisturePercent.ToString("0")).Append("%");

                    if (bed.isReadyToHarvest) sb.Append("  ✔ [READY TO HARVEST: +").Append(bed.yieldKg.ToString("0.#")).Append(" kg]");
                    else sb.Append("  [SPORE GERMINATION IN PROGRESS]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nFARM LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
