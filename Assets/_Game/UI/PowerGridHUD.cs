using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using AtomicWar._Game.Shelter;

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
        public string LastOutcome { get; private set; } = string.Empty;

        /// <summary>One-line budget status: generation / draw / fuel / overload.</summary>
        public string BudgetSummary { get; private set; } = string.Empty;

        /// <summary>Multi-line source list (diesel generator, bicycle, etc.).</summary>
        public string SourcesSummary { get; private set; } = string.Empty;

        /// <summary>Multi-line consumer list with priority and power state.</summary>
        public string ConsumersSummary { get; private set; } = string.Empty;

        public event Action OnPowerGridChanged;
        public event Action<string> OnToggleRoomPowerRequested; // (roomId)

        private PowerNetwork _network;
        private Func<PowerGridSnapshot> _getSnapshot;

        public void Bind(PowerNetwork network)
        {
            if (_network != null)
                _network.OnPowerStateChanged -= Refresh;

            _network = network;
            _getSnapshot = null;

            if (_network != null)
                _network.OnPowerStateChanged += Refresh;

            Refresh();
        }

        public void Bind(Func<PowerGridSnapshot> getSnapshot)
        {
            if (_network != null)
            {
                _network.OnPowerStateChanged -= Refresh;
                _network = null;
            }
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
            var snapshot = CurrentSnapshot();
            int count = snapshot?.rooms?.Count ?? 0;
            if (!IsOpen || count == 0) return false;
            SelectedRoomIndex = (SelectedRoomIndex + 1) % count;
            ReportOutcome("Selected shelter room: " + GetSelectedRoomName(snapshot));
            return true;
        }

        public bool SelectPreviousRoom()
        {
            var snapshot = CurrentSnapshot();
            int count = snapshot?.rooms?.Count ?? 0;
            if (!IsOpen || count == 0) return false;
            SelectedRoomIndex = (SelectedRoomIndex - 1 + count) % count;
            ReportOutcome("Selected shelter room: " + GetSelectedRoomName(snapshot));
            return true;
        }

        public bool RequestToggleRoomPower()
        {
            var snapshot = CurrentSnapshot();
            int count = snapshot?.rooms?.Count ?? 0;
            if (!IsOpen || count == 0)
            {
                ReportOutcome("No room selected for power toggle.");
                return false;
            }

            var room = GetSelectedRoom(snapshot);
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

        /// <summary>
        /// Cycles the priority of a consumer by module id. Returns the new priority.
        /// </summary>
        public int CyclePriority(string consumerId)
        {
            if (_network == null) return 0;
            var consumer = _network.GetConsumer(consumerId);
            if (consumer == null || consumer.PriorityLocked) return 0;

            int next = consumer.Priority >= 5 ? 1 : consumer.Priority + 1;
            _network.SetPriority(consumerId, next);
            Refresh();
            return next;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No power grid action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            RebuildSummaries();
            OnPowerGridChanged?.Invoke();
        }

        /// <summary>
        /// Returns the full panel text for tests or manual display. Calls Refresh internally.
        /// </summary>
        public string BuildPanelText()
        {
            Refresh();
            var sb = new StringBuilder("MAIN ELECTRICAL POWER GRID & ROOM BREAKER  [P] close  ·  [Tab] cycle  ·  [T] toggle room power");
            sb.Append("\n").Append(BudgetSummary);
            if (!string.IsNullOrEmpty(SourcesSummary))
                sb.Append("\n\n").Append(SourcesSummary);
            if (!string.IsNullOrEmpty(ConsumersSummary))
                sb.Append("\n\n").Append(ConsumersSummary);
            if (!string.IsNullOrEmpty(LastOutcome))
                sb.Append("\n\nGRID LOG: ").Append(LastOutcome);
            return sb.ToString();
        }

        private PowerGridSnapshot CurrentSnapshot()
        {
            if (_getSnapshot != null) return _getSnapshot();
            return BuildSnapshotFromNetwork();
        }

        private PowerGridSnapshot BuildSnapshotFromNetwork()
        {
            var snapshot = new PowerGridSnapshot();
            if (_network == null) return snapshot;

            snapshot.totalWattageGenerated = _network.TotalGeneration;
            snapshot.totalWattageConsumed = _network.TotalDraw;
            snapshot.isOverloaded = _network.IsLoadShedding;

            var sources = _network.Sources;
            float totalFuel = 0f;
            float maxFuel = 0f;
            for (int i = 0; i < sources.Count; i++)
            {
                var s = sources[i];
                if (s == null) continue;
                if (s.Definition != null && s.Definition.Kind == PowerSourceKind.Diesel)
                {
                    totalFuel += s.Fuel;
                    maxFuel += 100f; // default tank capacity for UI percentage
                }
            }
            snapshot.generatorFuelLevelPercent = maxFuel > 0f ? (totalFuel / maxFuel) * 100f : 0f;

            var consumers = _network.Consumers;
            for (int i = 0; i < consumers.Count; i++)
            {
                var c = consumers[i];
                if (c == null) continue;
                snapshot.rooms.Add(new PowerGridRoomSnapshot
                {
                    roomId = c.ModuleId,
                    roomName = c.DisplayName ?? c.ModuleId,
                    wattageLoad = c.Watts,
                    isPowered = c.IsPowered,
                    isEssentialCircuit = c.Priority <= 1
                });
            }
            return snapshot;
        }

        private void RebuildSummaries()
        {
            var snapshot = CurrentSnapshot();
            if (snapshot == null)
            {
                BudgetSummary = "Power grid telemetry offline.";
                SourcesSummary = string.Empty;
                ConsumersSummary = string.Empty;
                return;
            }

            // Budget line
            var budget = new StringBuilder("POWER GRID TELEMETRY: Output: ")
                .Append(snapshot.totalWattageGenerated.ToString("0.#")).Append(" W")
                .Append("  ·  Consumed: ").Append(snapshot.totalWattageConsumed.ToString("0.#")).Append(" W")
                .Append("  ·  Diesel Generator Fuel: ").Append(snapshot.generatorFuelLevelPercent.ToString("0")).Append("%");

            if (snapshot.isOverloaded)
                budget.Append("  [CRITICAL OVERLOAD: GRID FUSE FAILURE IMMINENT!]");

            BudgetSummary = budget.ToString();

            // Sources summary
            var sources = new StringBuilder("POWER SOURCES:");
            if (_network != null && _network.Sources.Count > 0)
            {
                for (int i = 0; i < _network.Sources.Count; i++)
                {
                    var s = _network.Sources[i];
                    if (s == null) continue;
                    string name = s.Definition != null ? s.Definition.DisplayName : s.SourceId;
                    sources.Append("\n  ").Append(name)
                        .Append(" — ").Append(s.IsEnabled ? "ON" : "OFF")
                        .Append("  fuel=").Append(s.Fuel.ToString("0.0"));
                }
            }
            else
            {
                sources.Append("\n  No power sources registered.");
            }
            SourcesSummary = sources.ToString();

            // Consumers summary
            var consumers = new StringBuilder("SHELTER ROOM ELECTRICAL SUBCIRCUITS:");
            if (snapshot.rooms == null || snapshot.rooms.Count == 0)
            {
                consumers.Append("\n  No rooms registered on electrical grid.");
            }
            else
            {
                for (int i = 0; i < snapshot.rooms.Count; i++)
                {
                    var room = snapshot.rooms[i];
                    if (room == null) continue;

                    bool selected = (i == SelectedRoomIndex);
                    consumers.Append("\n").Append(selected ? "> " : "  ")
                        .Append(room.roomName ?? room.roomId)
                        .Append(" — Load: ").Append(room.wattageLoad.ToString("0.#")).Append(" W");

                    if (room.isPowered) consumers.Append("  ✔ [ENERGIZED]");
                    else consumers.Append("  ✖ [SHED]");

                    if (room.isEssentialCircuit) consumers.Append("  [ESSENTIAL]");
                }
            }
            ConsumersSummary = consumers.ToString();
        }

        private PowerGridRoomSnapshot GetSelectedRoom(PowerGridSnapshot snapshot)
        {
            if (snapshot != null && snapshot.rooms != null && SelectedRoomIndex >= 0 && SelectedRoomIndex < snapshot.rooms.Count)
                return snapshot.rooms[SelectedRoomIndex];
            return null;
        }

        private string GetSelectedRoomName(PowerGridSnapshot snapshot)
        {
            var r = GetSelectedRoom(snapshot);
            return r != null ? r.roomName : "None";
        }

        private void OnDestroy()
        {
            if (_network != null)
                _network.OnPowerStateChanged -= Refresh;
        }
    }
}
