using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class StoredCorpseSnapshot
    {
        public string corpseId;
        public string deceasedName;
        public float hoursInStorage;
        public float rotLevel;  // 0..100
        public float moldLevel; // 0..100
        public float diseaseRisk; // 0..1
    }

    public class CorpseManagementSnapshot
    {
        public float moraleDrainPerHour;
        public float moldIncreasePerHour;
        public float bacterialDiseaseChancePerHour;
        public int totalBuriedOutside;
        public int totalRecycledFertilizer;
        public List<StoredCorpseSnapshot> corpses = new List<StoredCorpseSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Corpse Management & Rot Storage HUD view-model.
    /// Tracks bunker storage corpse rot, mold growth, bacterial disease infection risk,
    /// outdoor trench burial dispatch (4h daylight + ambient rad risk), and greenhouse
    /// fertilizer recycling options (severe morale hit).
    /// </summary>
    public class CorpseManagementHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedCorpseIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnCorpseManagementChanged;
        public event Action<string, string> OnBuryOutsideRequested;        // (corpseId, survivorId)
        public event Action<string, string> OnRecycleFertilizerRequested;  // (corpseId, survivorId)

        private Func<CorpseManagementSnapshot> _getSnapshot;
        private CorpseManagementSnapshot _snapshot;

        public void Bind(Func<CorpseManagementSnapshot> getSnapshot)
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
            ReportOutcome("Selected deceased body: " + GetSelectedCorpseName());
            return true;
        }

        public bool SelectPreviousCorpse()
        {
            if (!IsOpen || _snapshot == null || _snapshot.corpses == null || _snapshot.corpses.Count == 0)
                return false;
            SelectedCorpseIndex = (SelectedCorpseIndex - 1 + _snapshot.corpses.Count) % _snapshot.corpses.Count;
            ReportOutcome("Selected deceased body: " + GetSelectedCorpseName());
            return true;
        }

        public bool RequestBuryOutside(string survivorId)
        {
            if (!IsOpen || _snapshot == null || _snapshot.corpses == null || _snapshot.corpses.Count == 0)
            {
                ReportOutcome("No deceased body selected for burial.");
                return false;
            }

            var corpse = GetSelectedCorpse();
            if (corpse == null) return false;

            if (OnBuryOutsideRequested == null)
            {
                ReportOutcome("Outside burial dispatch link offline.");
                return false;
            }

            OnBuryOutsideRequested.Invoke(corpse.corpseId, survivorId ?? "unassigned");
            ReportOutcome("Dispatching " + (survivorId ?? "gravedigger") + " to bury " + corpse.deceasedName + " outside (4h daylight + 8 rads)...");
            return true;
        }

        public bool RequestRecycleFertilizer(string survivorId)
        {
            if (!IsOpen || _snapshot == null || _snapshot.corpses == null || _snapshot.corpses.Count == 0)
            {
                ReportOutcome("No deceased body selected for greenhouse fertilizer recycling.");
                return false;
            }

            var corpse = GetSelectedCorpse();
            if (corpse == null) return false;

            if (OnRecycleFertilizerRequested == null)
            {
                ReportOutcome("Greenhouse recycling link offline.");
                return false;
            }

            OnRecycleFertilizerRequested.Invoke(corpse.corpseId, survivorId ?? "unassigned");
            ReportOutcome("Processing " + corpse.deceasedName + " for greenhouse fertilizer (Group Morale Hit: -25)...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No corpse management action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnCorpseManagementChanged?.Invoke();
        }

        private StoredCorpseSnapshot GetSelectedCorpse()
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
            return c != null ? c.deceasedName ?? c.corpseId : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("CORPSE STORAGE & ROT DISPOSAL  [C] close  ·  [Tab] cycle  ·  [B] bury outside  ·  [F] greenhouse fertilizer");

            if (_snapshot == null)
            {
                sb.Append("\nCorpse storage telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nSTORAGE HAZARDS: Morale Drain: -").Append(_snapshot.moraleDrainPerHour.ToString("0.#")).Append(" morale/hr")
              .Append("  ·  Mold Rate: +").Append(_snapshot.moldIncreasePerHour.ToString("0.00")).Append("/hr")
              .Append("  ·  Infection Risk: ").Append((_snapshot.bacterialDiseaseChancePerHour * 100f).ToString("0")).Append("%/hr")
              .Append("  ·  Buried: ").Append(_snapshot.totalBuriedOutside)
              .Append("  ·  Recycled: ").Append(_snapshot.totalRecycledFertilizer);

            sb.Append("\n\nUNBURIED BODIES IN BUNKER STORAGE:");
            if (_snapshot.corpses == null || _snapshot.corpses.Count == 0)
            {
                sb.Append("\n  No deceased bodies in shelter storage.");
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
                      .Append(" — Storage: ").Append(corpse.hoursInStorage.ToString("0.#")).Append(" hrs")
                      .Append(" | Rot: ").Append(corpse.rotLevel.ToString("0")).Append("%")
                      .Append(" | Mold: ").Append(corpse.moldLevel.ToString("0")).Append("%");

                    if (corpse.rotLevel > 60f) sb.Append("  [HIGH ROT HAZARD]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nSTORAGE LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
