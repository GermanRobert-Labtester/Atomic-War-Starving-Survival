using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class PyreCorpseSnapshot
    {
        public string corpseId;
        public string deceasedName;
        public float woodFuelRequiredKg;
        public float ashYieldKg;
        public float smokeFalloutMultiplier;
        public bool isCremated;
    }

    public class PyreCremationSnapshot
    {
        public int totalBodiesCremated;
        public float totalAshHarvestedKg;
        public List<PyreCorpseSnapshot> corpses = new List<PyreCorpseSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Pyre Cremation & Ash Harvesting HUD view-model.
    /// Monitors outdoor funeral pyres, corpse cremation, wood fuel consumption,
    /// ash harvesting (for lye soap & crop fertilizer), and black smoke fallout emissions.
    /// </summary>
    public class PyreCremationHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedCorpseIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnPyreCremationChanged;
        public event Action<string> OnIgnitePyreRequested; // (corpseId)

        private Func<PyreCremationSnapshot> _getSnapshot;
        private PyreCremationSnapshot _snapshot;

        public void Bind(Func<PyreCremationSnapshot> getSnapshot)
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

        public bool SelectNextCorpse()
        {
            if (!IsOpen || _snapshot == null || _snapshot.corpses == null || _snapshot.corpses.Count == 0)
                return false;
            SelectedCorpseIndex = (SelectedCorpseIndex + 1) % _snapshot.corpses.Count;
            ReportOutcome("Selected pyre corpse: " + GetSelectedCorpseName());
            return true;
        }

        public bool SelectPreviousCorpse()
        {
            if (!IsOpen || _snapshot == null || _snapshot.corpses == null || _snapshot.corpses.Count == 0)
                return false;
            SelectedCorpseIndex = (SelectedCorpseIndex - 1 + _snapshot.corpses.Count) % _snapshot.corpses.Count;
            ReportOutcome("Selected pyre corpse: " + GetSelectedCorpseName());
            return true;
        }

        public bool RequestIgnitePyre()
        {
            if (!IsOpen || _snapshot == null || _snapshot.corpses == null || _snapshot.corpses.Count == 0)
            {
                ReportOutcome("No corpse selected for pyre cremation.");
                return false;
            }

            var corpse = GetSelectedCorpse();
            if (corpse == null) return false;

            if (corpse.isCremated)
            {
                ReportOutcome(corpse.deceasedName + "'s pyre is already burned to ash.");
                return false;
            }

            if (OnIgnitePyreRequested == null)
            {
                ReportOutcome("Pyre igniter link offline.");
                return false;
            }

            OnIgnitePyreRequested.Invoke(corpse.corpseId);
            ReportOutcome("Igniting Funeral Pyre for " + corpse.deceasedName + " (Fuel: " + corpse.woodFuelRequiredKg.ToString("0.#") + " kg)...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No cremation action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnPyreCremationChanged?.Invoke();
        }

        private PyreCorpseSnapshot GetSelectedCorpse()
        {
            if (_snapshot != null && _snapshot.corpses != null && SelectedCorpseIndex >= 0 && SelectedCorpseIndex < _snapshot.corpses.Count)
            {
                return _snapshot.corpses[SelectedCorpseIndex];
            }
            return null;
        }

        private string GetSelectedCorpseName()
        {
            var c = GetSelectedCorpse();
            return c != null ? (c.deceasedName ?? c.corpseId) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("FUNERAL PYRE & CREMATION ASH HARVEST  [C] close  ·  [Tab] cycle  ·  [I] ignite pyre");

            if (_snapshot == null)
            {
                sb.Append("\nPyre cremation telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nCREMATION STATS: Bodies Cremated: ").Append(_snapshot.totalBodiesCremated)
              .Append("  ·  Total Ash Harvested: ").Append(_snapshot.totalAshHarvestedKg.ToString("0.#")).Append(" kg");

            sb.Append("\n\nOUTDOOR FUNERAL PYRES:");
            if (_snapshot.corpses == null || _snapshot.corpses.Count == 0)
            {
                sb.Append("\n  No bodies queued for cremation.");
            }
            else
            {
                for (int i = 0; i < _snapshot.corpses.Count; i++)
                {
                    var corpse = _snapshot.corpses[i];
                    if (corpse == null) continue;

                    bool selected = (i == SelectedCorpseIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(corpse.deceasedName ?? corpse.corpseId)
                      .Append(" — Wood Fuel Needed: ").Append(corpse.woodFuelRequiredKg.ToString("0.#")).Append(" kg")
                      .Append(" | Expected Ash: ").Append(corpse.ashYieldKg.ToString("0.#")).Append(" kg");

                    if (corpse.isCremated) sb.Append("  ✔ [CREMATED — ASH READY]");
                    else sb.Append("  ★ [QUEUED FOR PYRE IGNITION]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nPYRE LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
