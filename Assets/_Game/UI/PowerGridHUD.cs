using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class PowerGridRoomSnapshot
    {
        public string roomId;
        public string roomName;
        public float wattageLoad;
        public bool isPowered;
        public bool isEssentialCircuit;
    }

    public class PowerGridSnapshot
    {
        public float totalWattageGenerated;
        public float totalWattageConsumed;
        public float generatorFuelLevelPercent; // 0..100
        public bool isOverloaded;
        public List<PowerGridRoomSnapshot> rooms = new List<PowerGridRoomSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Main Electrical Power Grid Telemetry HUD view-model.
    /// Monitors shelter diesel generator output (kW), per-room wattage consumption,
    /// essential vs non-essential room power toggles, grid overload prevention, and fuel reserves.
    /// </summary>
    public class PowerGridHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedRoomIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnPowerGridChanged;
        public event Action<string> OnToggleRoomPowerRequested; // (roomId)

        private Func<PowerGridSnapshot> _getSnapshot;
        private PowerGridSnapshot _snapshot;

        public void Bind(Func<PowerGridSnapshot> getSnapshot)
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

        public bool SelectNextRoom()
        {
            if (!IsOpen || _snapshot == null || _snapshot.rooms == null || _snapshot.rooms.Count == 0)
                return false;
            SelectedRoomIndex = (SelectedRoomIndex + 1) % _snapshot.rooms.Count;
            ReportOutcome("Selected shelter room: " + GetSelectedRoomName());
            return true;
        }

        public bool SelectPreviousRoom()
        {
            if (!IsOpen || _snapshot == null || _snapshot.rooms == null || _snapshot.rooms.Count == 0)
                return false;
            SelectedRoomIndex = (SelectedRoomIndex - 1 + _snapshot.rooms.Count) % _snapshot.rooms.Count;
            ReportOutcome("Selected shelter room: " + GetSelectedRoomName());
            return true;
        }

        public bool RequestToggleRoomPower()
        {
            if (!IsOpen || _snapshot == null || _snapshot.rooms == null || _snapshot.rooms.Count == 0)
            {
                ReportOutcome("No room selected for power toggle.");
                return false;
            }

            var room = GetSelectedRoom();
            if (room == null) return false;

            if (OnToggleRoomPowerRequested == null)
            {
                ReportOutcome("Power grid breaker panel link offline.");
                return false;
            }

            OnToggleRoomPowerRequested.Invoke(room.roomId);
            ReportOutcome((room.isPowered ? "Cutting" : "Restoring") + " electrical power to " + room.roomName + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No power grid action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnPowerGridChanged?.Invoke();
        }

        private PowerGridRoomSnapshot GetSelectedRoom()
        {
            if (_snapshot != null && _snapshot.rooms != null && SelectedRoomIndex >= 0 && SelectedRoomIndex < _snapshot.rooms.Count)
            {
                return _snapshot.rooms[SelectedRoomIndex];
            }
            return null;
        }

        private string GetSelectedRoomName()
        {
            var r = GetSelectedRoom();
            return r != null ? r.roomName : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("MAIN ELECTRICAL POWER GRID & ROOM BREAKER  [P] close  ·  [Tab] cycle  ·  [T] toggle room power");

            if (_snapshot == null)
            {
                sb.Append("\nPower grid telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nPOWER GRID TELEMETRY: Output: ").Append(_snapshot.totalWattageGenerated.ToString("0.#")).Append(" W")
              .Append("  ·  Consumed: ").Append(_snapshot.totalWattageConsumed.ToString("0.#")).Append(" W")
              .Append("  ·  Diesel Generator Fuel: ").Append(_snapshot.generatorFuelLevelPercent.ToString("0")).Append("%");

            if (_snapshot.isOverloaded)
                sb.Append("  [CRITICAL OVERLOAD: GRID FUSE FAILURE IMMINENT!]");

            sb.Append("\n\nSHELTER ROOM ELECTRICAL SUBCIRCUITS:");
            if (_snapshot.rooms == null || _snapshot.rooms.Count == 0)
            {
                sb.Append("\n  No rooms registered on electrical grid.");
            }
            else
            {
                for (int i = 0; i < _snapshot.rooms.Count; i++)
                {
                    var room = _snapshot.rooms[i];
                    if (room == null) continue;

                    bool selected = (i == SelectedRoomIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(room.roomName ?? room.roomId)
                      .Append(" — Load: ").Append(room.wattageLoad.ToString("0.#")).Append(" W");

                    if (room.isPowered) sb.Append("  ✔ [ENERGIZED]");
                    else sb.Append("  ✖ [UNPOWERED / CUT]");

                    if (room.isEssentialCircuit) sb.Append("  [ESSENTIAL]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nGRID LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
