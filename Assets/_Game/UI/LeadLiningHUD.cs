using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class LeadLiningSectorSnapshot
    {
        public string sectorId;
        public string sectorName;
        public float leadThicknessMm;
        public float radiationAttenuationPercent; // 0..100
        public int leadPlatesInstalledCount;
    }

    public class LeadLiningSnapshot
    {
        public int totalLeadPlatesInInventory;
        public float averageShieldingPercent;
        public List<LeadLiningSectorSnapshot> sectors = new List<LeadLiningSectorSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Lead Lining Wall Radiation Shielding HUD view-model.
    /// Monitors bunker room lead plate lining, radiation attenuation percentages (% rad block),
    /// lead thickness (mm), scrap lead smelting, and structural radiation protection.
    /// </summary>
    public class LeadLiningHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedSectorIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnLeadLiningChanged;
        public event Action<string> OnInstallLeadPlateRequested; // (sectorId)

        private Func<LeadLiningSnapshot> _getSnapshot;
        private LeadLiningSnapshot _snapshot;

        public void Bind(Func<LeadLiningSnapshot> getSnapshot)
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
            ReportOutcome("Selected shelter sector: " + GetSelectedSectorName());
            return true;
        }

        public bool SelectPreviousSector()
        {
            if (!IsOpen || _snapshot == null || _snapshot.sectors == null || _snapshot.sectors.Count == 0)
                return false;
            SelectedSectorIndex = (SelectedSectorIndex - 1 + _snapshot.sectors.Count) % _snapshot.sectors.Count;
            ReportOutcome("Selected shelter sector: " + GetSelectedSectorName());
            return true;
        }

        public bool RequestInstallLeadPlate()
        {
            if (!IsOpen || _snapshot == null || _snapshot.sectors == null || _snapshot.sectors.Count == 0)
            {
                ReportOutcome("No sector selected for lead plate installation.");
                return false;
            }

            var sector = GetSelectedSector();
            if (sector == null) return false;

            if (_snapshot != null && _snapshot.totalLeadPlatesInInventory <= 0)
            {
                ReportOutcome("CANNOT INSTALL: No Lead Plates in inventory!");
                return false;
            }

            if (OnInstallLeadPlateRequested == null)
            {
                ReportOutcome("Shielding installation crew link offline.");
                return false;
            }

            OnInstallLeadPlateRequested.Invoke(sector.sectorId);
            ReportOutcome("Installing Lead Plate onto walls of Sector " + sector.sectorName + " (+Attenuation)...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No lead lining action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnLeadLiningChanged?.Invoke();
        }

        private LeadLiningSectorSnapshot GetSelectedSector()
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
            return s != null ? s.sectorName : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("LEAD LINING & RADIATION WALL SHIELDING  [L] close  ·  [Tab] cycle  ·  [I] install lead plate");

            if (_snapshot == null)
            {
                sb.Append("\nShielding telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nSHIELDING STATS: Lead Plates Available: ").Append(_snapshot.totalLeadPlatesInInventory)
              .Append("  ·  Avg Shelter Attenuation: ").Append(_snapshot.averageShieldingPercent.ToString("0")).Append("%");

            sb.Append("\n\nSHELTER ROOM SHIELDING SECTORS:");
            if (_snapshot.sectors == null || _snapshot.sectors.Count == 0)
            {
                sb.Append("\n  No rooms registered for lead lining.");
            }
            else
            {
                for (int i = 0; i < _snapshot.sectors.Count; i++)
                {
                    var sector = _snapshot.sectors[i];
                    if (sector == null) continue;

                    bool selected = (i == SelectedSectorIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(sector.sectorName ?? sector.sectorId)
                      .Append(" — Lead Thickness: ").Append(sector.leadThicknessMm.ToString("0.#")).Append(" mm")
                      .Append(" | Rad Attenuation: ").Append(sector.radiationAttenuationPercent.ToString("0")).Append("%")
                      .Append(" | Plates Installed: ").Append(sector.leadPlatesInstalledCount);
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nSHIELDING LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
