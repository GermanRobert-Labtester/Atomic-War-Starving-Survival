using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class MinefieldSectorSnapshot
    {
        public string sectorId;
        public int activeMinesCount;
        public float tripwireIntegrityPercent;
        public bool isBreachTriggered;
        public int raidersDetonatedCount;
    }

    public class PerimeterMinesSnapshot
    {
        public int totalMinesArmed;
        public int totalDetonationsRecorded;
        public List<MinefieldSectorSnapshot> sectors = new List<MinefieldSectorSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Perimeter Claymore Minefield & Tripwire HUD view-model.
    /// Monitors armed claymore minefields, tripwire tension integrity, perimeter breach detonations,
    /// mine disarming/re-arming operations, and raider casualty counts.
    /// </summary>
    public class PerimeterMinesHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedSectorIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnPerimeterMinesChanged;
        public event Action<string> OnArmMineSectorRequested; // (sectorId)
        public event Action<string> OnRepairTripwireRequested; // (sectorId)

        private Func<PerimeterMinesSnapshot> _getSnapshot;
        private PerimeterMinesSnapshot _snapshot;

        public void Bind(Func<PerimeterMinesSnapshot> getSnapshot)
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
            ReportOutcome("Selected minefield sector: " + GetSelectedSectorName());
            return true;
        }

        public bool SelectPreviousSector()
        {
            if (!IsOpen || _snapshot == null || _snapshot.sectors == null || _snapshot.sectors.Count == 0)
                return false;
            SelectedSectorIndex = (SelectedSectorIndex - 1 + _snapshot.sectors.Count) % _snapshot.sectors.Count;
            ReportOutcome("Selected minefield sector: " + GetSelectedSectorName());
            return true;
        }

        public bool RequestArmSector()
        {
            if (!IsOpen || _snapshot == null || _snapshot.sectors == null || _snapshot.sectors.Count == 0)
            {
                ReportOutcome("No sector selected for mine arming.");
                return false;
            }

            var sector = GetSelectedSector();
            if (sector == null) return false;

            if (OnArmMineSectorRequested == null)
            {
                ReportOutcome("Minefield detonator link offline.");
                return false;
            }

            OnArmMineSectorRequested.Invoke(sector.sectorId);
            ReportOutcome("Arming Claymore Mine in Sector " + sector.sectorId + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No minefield action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnPerimeterMinesChanged?.Invoke();
        }

        private MinefieldSectorSnapshot GetSelectedSector()
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
            var sb = new StringBuilder("PERIMETER MINEFIELD & TRIPWIRE DETECTOR  [M] close  ·  [Tab] cycle  ·  [A] arm claymore mine");

            if (_snapshot == null)
            {
                sb.Append("\nMinefield telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nMINEFIELD STATS: Total Armed Mines: ").Append(_snapshot.totalMinesArmed)
              .Append("  ·  Total Detonations: ").Append(_snapshot.totalDetonationsRecorded);

            sb.Append("\n\nPERIMETER DEFENSE SECTORS:");
            if (_snapshot.sectors == null || _snapshot.sectors.Count == 0)
            {
                sb.Append("\n  No minefields deployed in perimeter.");
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
                      .Append(" — Active Mines: ").Append(sec.activeMinesCount)
                      .Append(" | Tripwire Integrity: ").Append(sec.tripwireIntegrityPercent.ToString("0")).Append("%")
                      .Append(" | Raider Kills: ").Append(sec.raidersDetonatedCount);

                    if (sec.isBreachTriggered) sb.Append("  ★ [MINE DETONATED — PERIMETER BREACH!]");
                    else if (sec.activeMinesCount > 0) sb.Append("  ✔ [ARMED & READY]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nMINEFIELD LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
