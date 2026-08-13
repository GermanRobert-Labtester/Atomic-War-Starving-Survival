#pragma warning disable CS0067 // Public API event surface; subscribers arrive with feature wiring
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class BarricadeSectorSnapshot
    {
        public string sectorId;
        public float barricadeHealth; // 0..100
        public int defendersAssignedCount;
        public bool isBreached;
    }

    public class DesertersStandSnapshot
    {
        public int currentWave;
        public int totalDefendersAlive;
        public List<BarricadeSectorSnapshot> sectors = new List<BarricadeSectorSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Deserter's Last Stand HUD view-model.
    /// Monitors final defensive perimeter barricades, raider assault wave counters,
    /// defender assignment, barricade repair dispatch, and breach warning alerts.
    /// </summary>
    public class DesertersStandHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedSectorIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnDesertersStandChanged;
        public event Action<string> OnRepairBarricadeRequested; // (sectorId)
        public event Action<string> OnReinforceSectorRequested; // (sectorId)

        private Func<DesertersStandSnapshot> _getSnapshot;
        private DesertersStandSnapshot _snapshot;

        public void Bind(Func<DesertersStandSnapshot> getSnapshot)
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

        public bool SelectNextSector()
        {
            if (!IsOpen || _snapshot == null || _snapshot.sectors == null || _snapshot.sectors.Count == 0)
                return false;
            SelectedSectorIndex = (SelectedSectorIndex + 1) % _snapshot.sectors.Count;
            ReportOutcome("Selected defense sector: " + GetSelectedSectorName());
            return true;
        }

        public bool SelectPreviousSector()
        {
            if (!IsOpen || _snapshot == null || _snapshot.sectors == null || _snapshot.sectors.Count == 0)
                return false;
            SelectedSectorIndex = (SelectedSectorIndex - 1 + _snapshot.sectors.Count) % _snapshot.sectors.Count;
            ReportOutcome("Selected defense sector: " + GetSelectedSectorName());
            return true;
        }

        public bool RequestRepairBarricade()
        {
            if (!IsOpen || _snapshot == null || _snapshot.sectors == null || _snapshot.sectors.Count == 0)
            {
                ReportOutcome("No sector selected for barricade repair.");
                return false;
            }

            var sector = GetSelectedSector();
            if (sector == null) return false;

            if (OnRepairBarricadeRequested == null)
            {
                ReportOutcome("Perimeter repair link offline.");
                return false;
            }

            OnRepairBarricadeRequested.Invoke(sector.sectorId);
            ReportOutcome("Repairing barricade at Sector " + sector.sectorId + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No defense action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnDesertersStandChanged?.Invoke();
        }

        private BarricadeSectorSnapshot GetSelectedSector()
        {
            if (_snapshot != null && _snapshot.sectors != null && SelectedSectorIndex >= 0 && SelectedSectorIndex < _snapshot.sectors.Count)
            {
                return _snapshot.sectors[SelectedSectorIndex];
            }
            return null;
        }

        private string GetSelectedSectorName()
        {
            var s = GetSelectedSector();
            return s != null ? s.sectorId : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("DESERTER'S LAST STAND DEFENSE MONITOR  [W] close  ·  [Tab] cycle  ·  [R] repair barricade");

            if (_snapshot == null)
            {
                sb.Append("\nDefense perimeter telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nPERIMETER STATS: Assault Wave: ").Append(_snapshot.currentWave)
              .Append("  ·  Defenders Alive: ").Append(_snapshot.totalDefendersAlive);

            sb.Append("\n\nDEFENSIVE BARRICADE SECTORS:");
            if (_snapshot.sectors == null || _snapshot.sectors.Count == 0)
            {
                sb.Append("\n  No barricade sectors registered.");
            }
            else
            {
                for (int i = 0; i < _snapshot.sectors.Count; i++)
                {
                    var sec = _snapshot.sectors[i];
                    if (sec == null) continue;

                    bool selected = (i == SelectedSectorIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append("Sector ").Append(sec.sectorId)
                      .Append(" — Health: ").Append(sec.barricadeHealth.ToString("0")).Append("%")
                      .Append(" | Defenders: ").Append(sec.defendersAssignedCount);

                    if (sec.isBreached) sb.Append("  ★ [CRITICAL BREACH — ENEMY INSIDE!]");
                    else if (sec.barricadeHealth < 30f) sb.Append("  [WARNING: BARRICADE FAILING]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nDEFENSE LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
