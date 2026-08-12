using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class ForensicCorpseSnapshot
    {
        public string corpseId;
        public string survivorName;
        public int clothAvailable;
        public float fatRenderableKg;
        public bool hasGoldFilling;
        public bool hasPhoto;
        public float infectionRiskNoMask;
    }

    public class ForensicScavengingSnapshot
    {
        public bool corpseThiefShameActive;
        public int totalClothStripped;
        public float totalFatRenderedKg;
        public int totalGoldFillingsExtracted;
        public int totalAshHarvested;
        public List<ForensicCorpseSnapshot> corpses = new List<ForensicCorpseSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Forensic Scavenging & Corpse Economy HUD view-model.
    /// Controls forensic extraction from fallen corpses: stripping wet cloth,
    /// rendering tallow/fat for soap and lamp fuel, extracting dental gold fillings,
    /// crematorium ash harvesting, and managing corpse thief psychological shame.
    /// </summary>
    public class ForensicScavengingHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedCorpseIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnForensicScavengingChanged;
        public event Action<string, string, bool> OnStripCorpseRequested; // (corpseId, survivorId, wearingMask)
        public event Action<string, string> OnRenderFatRequested;        // (corpseId, survivorId)
        public event Action<string, string> OnExtractGoldRequested;       // (corpseId, survivorId)
        public event Action<string, string> OnCremateCorpseRequested;     // (corpseId, survivorId)

        private Func<ForensicScavengingSnapshot> _getSnapshot;
        private ForensicScavengingSnapshot _snapshot;

        public void Bind(Func<ForensicScavengingSnapshot> getSnapshot)
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
            ReportOutcome("Selected corpse record: " + GetSelectedCorpseName());
            return true;
        }

        public bool SelectPreviousCorpse()
        {
            if (!IsOpen || _snapshot == null || _snapshot.corpses == null || _snapshot.corpses.Count == 0)
                return false;
            SelectedCorpseIndex = (SelectedCorpseIndex - 1 + _snapshot.corpses.Count) % _snapshot.corpses.Count;
            ReportOutcome("Selected corpse record: " + GetSelectedCorpseName());
            return true;
        }

        public bool RequestStripCorpse(string survivorId, bool wearingMask)
        {
            if (!IsOpen || _snapshot == null || _snapshot.corpses == null || _snapshot.corpses.Count == 0)
            {
                ReportOutcome("No corpse selected for stripping.");
                return false;
            }

            var corpse = GetSelectedCorpse();
            if (corpse == null) return false;

            if (OnStripCorpseRequested == null)
            {
                ReportOutcome("Forensic station link offline.");
                return false;
            }

            OnStripCorpseRequested.Invoke(corpse.corpseId, survivorId ?? "unassigned", wearingMask);
            ReportOutcome("Stripping clothing from " + corpse.survivorName + (wearingMask ? " (Gas Mask On)" : " (No Mask — Infection Risk!)"));
            return true;
        }

        public bool RequestRenderFat(string survivorId)
        {
            if (!IsOpen || _snapshot == null || _snapshot.corpses == null || _snapshot.corpses.Count == 0)
            {
                ReportOutcome("No corpse selected for tallow rendering.");
                return false;
            }

            var corpse = GetSelectedCorpse();
            if (corpse == null) return false;

            if (OnRenderFatRequested == null)
            {
                ReportOutcome("Forensic rendering vat link offline.");
                return false;
            }

            OnRenderFatRequested.Invoke(corpse.corpseId, survivorId ?? "unassigned");
            ReportOutcome("Rendering fat from " + corpse.survivorName + "...");
            return true;
        }

        public bool RequestExtractGold(string survivorId)
        {
            if (!IsOpen || _snapshot == null || _snapshot.corpses == null || _snapshot.corpses.Count == 0)
            {
                ReportOutcome("No corpse selected for dental extraction.");
                return false;
            }

            var corpse = GetSelectedCorpse();
            if (corpse == null) return false;

            if (!corpse.hasGoldFilling)
            {
                ReportOutcome("Corpse has no gold fillings to extract.");
                return false;
            }

            if (OnExtractGoldRequested == null)
            {
                ReportOutcome("Forensic tool kit link offline.");
                return false;
            }

            OnExtractGoldRequested.Invoke(corpse.corpseId, survivorId ?? "unassigned");
            ReportOutcome("Extracting gold filling from " + corpse.survivorName + "...");
            return true;
        }

        public bool RequestCremateCorpse(string survivorId)
        {
            if (!IsOpen || _snapshot == null || _snapshot.corpses == null || _snapshot.corpses.Count == 0)
            {
                ReportOutcome("No corpse selected for cremation.");
                return false;
            }

            var corpse = GetSelectedCorpse();
            if (corpse == null) return false;

            if (OnCremateCorpseRequested == null)
            {
                ReportOutcome("Crematorium control link offline.");
                return false;
            }

            OnCremateCorpseRequested.Invoke(corpse.corpseId, survivorId ?? "unassigned");
            ReportOutcome("Incinerating " + corpse.survivorName + " in Tessarat Crematorium...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No forensic action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnForensicScavengingChanged?.Invoke();
        }

        private ForensicCorpseSnapshot GetSelectedCorpse()
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
            return c != null ? c.survivorName ?? c.corpseId : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("FORENSIC SCAVENGING & CORPSE ECONOMY  [K] close  ·  [Tab] cycle  ·  [S] strip  ·  [F] render fat  ·  [G] gold  ·  [C] cremate");

            if (_snapshot == null)
            {
                sb.Append("\nForensic morgue telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nMORGUE STATS: Cloth Stripped: ").Append(_snapshot.totalClothStripped)
              .Append("  ·  Fat Rendered: ").Append(_snapshot.totalFatRenderedKg.ToString("0.#")).Append(" kg")
              .Append("  ·  Gold Extracted: ").Append(_snapshot.totalGoldFillingsExtracted)
              .Append("  ·  Crematorium Ash: ").Append(_snapshot.totalAshHarvested);

            if (_snapshot.corpseThiefShameActive)
                sb.Append("\nPSYCHOLOGICAL TOLL: [WARNING: CORPSE THIEF SHAME ACTIVE — WASH CLOTHES TO CURE]");

            sb.Append("\n\nUNPROCESSED CORPSES IN MORGUE:");
            if (_snapshot.corpses == null || _snapshot.corpses.Count == 0)
            {
                sb.Append("\n  No unburied corpses awaiting processing.");
            }
            else
            {
                for (int i = 0; i < _snapshot.corpses.Count; i++)
                {
                    var corpse = _snapshot.corpses[i];
                    if (corpse == null) continue;

                    bool selected = (i == SelectedCorpseIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(corpse.survivorName ?? corpse.corpseId)
                      .Append(" — Cloth: ").Append(corpse.clothAvailable)
                      .Append(" | Fat: ").Append(corpse.fatRenderableKg.ToString("0.#")).Append(" kg");

                    if (corpse.hasGoldFilling) sb.Append(" ★ [GOLD FILLING]");
                    if (corpse.hasPhoto) sb.Append(" [PHOTO MEMENTO]");
                    sb.Append(" | Infection Risk: ").Append((corpse.infectionRiskNoMask * 100f).ToString("0")).Append("%");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nMORGUE LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
