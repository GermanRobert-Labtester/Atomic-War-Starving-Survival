using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class GuerrillaTrapSnapshot
    {
        public string trapId;
        public string locationNodeId;
        public string trapType;
        public float durabilityPercent; // 0..100
        public bool isArmed;
        public int enemiesKilledCount;
    }

    public class GuerrillaWarfareSnapshot
    {
        public int totalTrapsArmed;
        public int totalAmbushVictims;
        public List<GuerrillaTrapSnapshot> traps = new List<GuerrillaTrapSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Guerrilla Warfare & Improvised Traps HUD view-model.
    /// Monitors wasteland defensive traps (punji stakes, tripwire IEDs, acoustic decoys),
    /// trap arming state, sector ambush efficiency, and kill counts.
    /// </summary>
    public class GuerrillaWarfareHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedTrapIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnGuerrillaWarfareChanged;
        public event Action<string> OnArmTrapRequested; // (trapId)

        private Func<GuerrillaWarfareSnapshot> _getSnapshot;
        private GuerrillaWarfareSnapshot _snapshot;

        public void Bind(Func<GuerrillaWarfareSnapshot> getSnapshot)
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

        public bool SelectNextTrap()
        {
            if (!IsOpen || _snapshot == null || _snapshot.traps == null || _snapshot.traps.Count == 0)
                return false;
            SelectedTrapIndex = (SelectedTrapIndex + 1) % _snapshot.traps.Count;
            ReportOutcome("Selected guerrilla trap: " + GetSelectedTrapName());
            return true;
        }

        public bool SelectPreviousTrap()
        {
            if (!IsOpen || _snapshot == null || _snapshot.traps == null || _snapshot.traps.Count == 0)
                return false;
            SelectedTrapIndex = (SelectedTrapIndex - 1 + _snapshot.traps.Count) % _snapshot.traps.Count;
            ReportOutcome("Selected guerrilla trap: " + GetSelectedTrapName());
            return true;
        }

        public bool RequestArmTrap()
        {
            if (!IsOpen || _snapshot == null || _snapshot.traps == null || _snapshot.traps.Count == 0)
            {
                ReportOutcome("No trap selected for arming.");
                return false;
            }

            var trap = GetSelectedTrap();
            if (trap == null) return false;

            if (OnArmTrapRequested == null)
            {
                ReportOutcome("Trap arming mechanism link offline.");
                return false;
            }

            OnArmTrapRequested.Invoke(trap.trapId);
            ReportOutcome("Arming " + trap.trapType + " at sector " + trap.locationNodeId + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No guerrilla action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnGuerrillaWarfareChanged?.Invoke();
        }

        private GuerrillaTrapSnapshot GetSelectedTrap()
        {
            if (_snapshot != null && _snapshot.traps != null && SelectedTrapIndex >= 0 && SelectedTrapIndex < _snapshot.traps.Count)
            {
                return _snapshot.traps[SelectedTrapIndex];
            }
            return null;
        }

        private string GetSelectedTrapName()
        {
            var t = GetSelectedTrap();
            return t != null ? (t.trapType + " @ " + t.locationNodeId) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("GUERRILLA WARFARE & IMPROVISED TRAPS  [T] close  ·  [Tab] cycle  ·  [A] arm trap");

            if (_snapshot == null)
            {
                sb.Append("\nGuerrilla trap telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nWARFARE STATS: Traps Armed: ").Append(_snapshot.totalTrapsArmed)
              .Append("  ·  Ambush Victims Neutralized: ").Append(_snapshot.totalAmbushVictims);

            sb.Append("\n\nIMPROVISED DEFENSIVE TRAPS:");
            if (_snapshot.traps == null || _snapshot.traps.Count == 0)
            {
                sb.Append("\n  No guerrilla traps deployed in perimeter sectors.");
            }
            else
            {
                for (int i = 0; i < _snapshot.traps.Count; i++)
                {
                    var trap = _snapshot.traps[i];
                    if (trap == null) continue;

                    bool selected = (i == SelectedTrapIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(trap.trapType ?? "Trap")
                      .Append(" @ ").Append(trap.locationNodeId)
                      .Append(" — Health: ").Append(trap.durabilityPercent.ToString("0")).Append("%")
                      .Append(" | Neutralizations: ").Append(trap.enemiesKilledCount);

                    if (trap.isArmed) sb.Append("  ★ [ARMED & LETHAL]");
                    else sb.Append("  [DISARMED]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nWARFARE LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
