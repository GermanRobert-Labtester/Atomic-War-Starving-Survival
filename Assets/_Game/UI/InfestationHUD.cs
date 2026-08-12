using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class InfestationSectorSnapshot
    {
        public string sectorId;
        public string pestType; // e.g. "radiant_roaches", "mutated_rats"
        public float infestationSeverityPercent; // 0..100
        public float foodContaminatedKg;
        public bool isTrapSet;
    }

    public class InfestationSnapshot
    {
        public int totalInfestedSectors;
        public int totalPestsExterminated;
        public List<InfestationSectorSnapshot> sectors = new List<InfestationSectorSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Mutated Pest Infestation & Fumigation HUD view-model.
    /// Monitors radiant roach & mutated rat infestations, food contamination losses,
    /// poison bait trap setting, fumigation dispatch, and pest extermination logs.
    /// </summary>
    public class InfestationHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedSectorIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnInfestationChanged;
        public event Action<string> OnSetPestTrapRequested; // (sectorId)
        public event Action<string> OnFumigateSectorRequested; // (sectorId)

        private Func<InfestationSnapshot> _getSnapshot;
        private InfestationSnapshot _snapshot;

        public void Bind(Func<InfestationSnapshot> getSnapshot)
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
            ReportOutcome("Selected infestation sector: " + GetSelectedSectorName());
            return true;
        }

        public bool SelectPreviousSector()
        {
            if (!IsOpen || _snapshot == null || _snapshot.sectors == null || _snapshot.sectors.Count == 0)
                return false;
            SelectedSectorIndex = (SelectedSectorIndex - 1 + _snapshot.sectors.Count) % _snapshot.sectors.Count;
            ReportOutcome("Selected infestation sector: " + GetSelectedSectorName());
            return true;
        }

        public bool RequestSetTrap()
        {
            if (!IsOpen || _snapshot == null || _snapshot.sectors == null || _snapshot.sectors.Count == 0)
            {
                ReportOutcome("No sector selected for pest trap placement.");
                return false;
            }

            var sector = GetSelectedSector();
            if (sector == null) return false;

            if (OnSetPestTrapRequested == null)
            {
                ReportOutcome("Pest control link offline.");
                return false;
            }

            OnSetPestTrapRequested.Invoke(sector.sectorId);
            ReportOutcome("Setting Poison Bait Trap at Sector " + sector.sectorId + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No infestation action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnInfestationChanged?.Invoke();
        }

        private InfestationSectorSnapshot GetSelectedSector()
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
            var sb = new StringBuilder("PEST INFESTATION & FUMIGATION CONTROL  [I] close  ·  [Tab] cycle  ·  [T] set poison trap");

            if (_snapshot == null)
            {
                sb.Append("\nPest infestation telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nINFESTATION STATS: Infested Sectors: ").Append(_snapshot.totalInfestedSectors)
              .Append("  ·  Pests Exterminated: ").Append(_snapshot.totalPestsExterminated);

            sb.Append("\n\nSHELTER INFESTATION SECTORS:");
            if (_snapshot.sectors == null || _snapshot.sectors.Count == 0)
            {
                sb.Append("\n  No pest infestations detected in shelter.");
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
                      .Append(" — Pest: ").Append(sec.pestType ?? "Radiant Roaches")
                      .Append(" — Severity: ").Append(sec.infestationSeverityPercent.ToString("0")).Append("%")
                      .Append(" | Food Lost: ").Append(sec.foodContaminatedKg.ToString("0.#")).Append(" kg");

                    if (sec.isTrapSet) sb.Append("  ✔ [POISON BAIT TRAP SET]");
                    else sb.Append("  ★ [NO TRAPS SET — UNCONTROLLED SPREAD]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nINFESTATION LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
