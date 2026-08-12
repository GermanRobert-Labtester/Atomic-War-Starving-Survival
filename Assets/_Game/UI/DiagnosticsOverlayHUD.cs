using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class SystemTelemetrySnapshot
    {
        public string subsystemName;
        public float frameTimeMs;
        public float memoryAllocatedMb;
        public bool isHealthy;
        public string lastErrorLog;
    }

    public class DiagnosticsOverlaySnapshot
    {
        public float fps;
        public float totalMemoryMb;
        public int activeSubsystemsCount;
        public List<SystemTelemetrySnapshot> subsystems = new List<SystemTelemetrySnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Diagnostics Overlay & Developer System Telemetry HUD view-model.
    /// Monitors real-time engine telemetry, system FPS, memory heap allocations, C# event bus state,
    /// and core domain subsystem health checks.
    /// </summary>
    public class DiagnosticsOverlayHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedSubsystemIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnDiagnosticsOverlayChanged;
        public event Action<string> OnRebootSubsystemRequested; // (subsystemName)

        private Func<DiagnosticsOverlaySnapshot> _getSnapshot;
        private DiagnosticsOverlaySnapshot _snapshot;

        public void Bind(Func<DiagnosticsOverlaySnapshot> getSnapshot)
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

        public bool SelectNextSubsystem()
        {
            if (!IsOpen || _snapshot == null || _snapshot.subsystems == null || _snapshot.subsystems.Count == 0)
                return false;
            SelectedSubsystemIndex = (SelectedSubsystemIndex + 1) % _snapshot.subsystems.Count;
            ReportOutcome("Selected subsystem: " + GetSelectedSubsystemName());
            return true;
        }

        public bool SelectPreviousSubsystem()
        {
            if (!IsOpen || _snapshot == null || _snapshot.subsystems == null || _snapshot.subsystems.Count == 0)
                return false;
            SelectedSubsystemIndex = (SelectedSubsystemIndex - 1 + _snapshot.subsystems.Count) % _snapshot.subsystems.Count;
            ReportOutcome("Selected subsystem: " + GetSelectedSubsystemName());
            return true;
        }

        public bool RequestRebootSubsystem()
        {
            if (!IsOpen || _snapshot == null || _snapshot.subsystems == null || _snapshot.subsystems.Count == 0)
            {
                ReportOutcome("No subsystem selected for reboot.");
                return false;
            }

            var sub = GetSelectedSubsystem();
            if (sub == null) return false;

            if (OnRebootSubsystemRequested == null)
            {
                ReportOutcome("Diagnostics reboot controller offline.");
                return false;
            }

            OnRebootSubsystemRequested.Invoke(sub.subsystemName);
            ReportOutcome("Rebooting C# subsystem [" + sub.subsystemName + "]...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No diagnostics action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnDiagnosticsOverlayChanged?.Invoke();
        }

        private SystemTelemetrySnapshot GetSelectedSubsystem()
        {
            if (_snapshot != null && _snapshot.subsystems != null && SelectedSubsystemIndex >= 0 && SelectedSubsystemIndex < _snapshot.subsystems.Count)
            {
                return _snapshot.subsystems[SelectedSubsystemIndex];
            }
            return null;
        }

        private string GetSelectedSubsystemName()
        {
            var s = GetSelectedSubsystem();
            return s != null ? s.subsystemName : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("DIAGNOSTICS & SYSTEM TELEMETRY OVERLAY  [F3] close  ·  [Tab] cycle  ·  [R] reboot subsystem");

            if (_snapshot == null)
            {
                sb.Append("\nEngine diagnostics telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nENGINE TELEMETRY: FPS: ").Append(_snapshot.fps.ToString("0.#"))
              .Append("  ·  Memory Allocated: ").Append(_snapshot.totalMemoryMb.ToString("0.#")).Append(" MB")
              .Append("  ·  Active C# Systems: ").Append(_snapshot.activeSubsystemsCount);

            sb.Append("\n\nREGISTERED C# SUBSYSTEM HEALTH:");
            if (_snapshot.subsystems == null || _snapshot.subsystems.Count == 0)
            {
                sb.Append("\n  No C# subsystems registered in diagnostics manager.");
            }
            else
            {
                for (int i = 0; i < _snapshot.subsystems.Count; i++)
                {
                    var sub = _snapshot.subsystems[i];
                    if (sub == null) continue;

                    bool selected = (i == SelectedSubsystemIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(sub.subsystemName)
                      .Append(" — ").Append(sub.isHealthy ? "✔ [OK]" : "✖ [FAULTED]")
                      .Append(" | Frame: ").Append(sub.frameTimeMs.ToString("0.00")).Append(" ms")
                      .Append(" | Heap: ").Append(sub.memoryAllocatedMb.ToString("0.#")).Append(" MB");

                    if (!string.IsNullOrEmpty(sub.lastErrorLog))
                        sb.Append("\n    [ERROR LOG: ").Append(sub.lastErrorLog).Append("]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nDIAGNOSTICS LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
