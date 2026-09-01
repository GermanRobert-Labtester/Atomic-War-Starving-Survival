using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Waystation
{
    [Serializable]
    public sealed class WaystationInstanceState
    {
        public string stationId { get; set; } = string.Empty;
        public bool isUnlocked { get; set; } = true;
        public float condition { get; set; } = 100f;
        public float filterHealth { get; set; } = 100f;
        public bool stoveLit { get; set; } = true;
        public int daysSinceResupply { get; set; } = 0;
        public List<string> availableStockItemIds { get; set; } = new List<string>();
        public List<string> assignedWatchSurvivorIds { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class WaystationNetworkState
    {
        public string systemId = WaystationNetworkSystem.SystemId;
        public List<WaystationInstanceState> stations = new List<WaystationInstanceState>();
        public int totalMaintenanceActions = 0;
    }

    /// <summary>
    /// Coordinates the multi-node waystation network across all 6 wasteland regions.
    /// Manages filter health decay, stove fuel, stock refresh, and watch staffing.
    /// </summary>
    public sealed class WaystationNetworkSystem
    {
        public const string SystemId = "waystation_network_system";

        private readonly List<WaystationDef> _catalog;
        private WaystationNetworkState _state;

        public event Action<WaystationInstanceState>? OnWaystationStateChanged;

        public WaystationNetworkState State => _state;
        public IReadOnlyList<WaystationDef> Catalog => _catalog;

        public WaystationNetworkSystem(List<WaystationDef>? catalog = null, WaystationNetworkState? state = null)
        {
            _catalog = catalog ?? WaystationCatalogLoader.GetDefaultWaystations();
            _state = state ?? new WaystationNetworkState();

            // Initialize instances for all catalog stations if missing
            foreach (var def in _catalog)
            {
                if (!_state.stations.Any(s => s.stationId == def.id))
                {
                    _state.stations.Add(new WaystationInstanceState
                    {
                        stationId = def.id,
                        isUnlocked = true,
                        condition = def.condition,
                        filterHealth = def.filter_health,
                        stoveLit = true,
                        daysSinceResupply = 0,
                        availableStockItemIds = new List<string>(def.stock_item_ids)
                    });
                }
            }
        }

        public WaystationInstanceState? GetStation(string stationId)
        {
            return _state.stations.FirstOrDefault(s => s.stationId == stationId);
        }

        public WaystationDef? GetDefinition(string stationId)
        {
            return _catalog.FirstOrDefault(d => d.id == stationId);
        }

        public void TickDay()
        {
            foreach (var station in _state.stations)
            {
                station.daysSinceResupply++;

                // Filter degradation (slow natural decay under wasteland ash)
                station.filterHealth = MathF.Max(0f, station.filterHealth - 1.5f);

                // If filter drops to 0, station condition degrades faster
                if (station.filterHealth <= 0f)
                {
                    station.condition = MathF.Max(10f, station.condition - 3.0f);
                }

                // If staffed with watch sentries, condition holds better
                if (station.assignedWatchSurvivorIds.Count > 0)
                {
                    station.condition = MathF.Min(100f, station.condition + 0.5f);
                }

                // Periodic stock refresh every 7 days
                if (station.daysSinceResupply >= 7)
                {
                    var def = GetDefinition(station.stationId);
                    if (def != null)
                    {
                        station.availableStockItemIds = new List<string>(def.stock_item_ids);
                        station.daysSinceResupply = 0;
                    }
                }

                OnWaystationStateChanged?.Invoke(station);
            }
        }

        public bool RepairFilter(string stationId)
        {
            var station = GetStation(stationId);
            if (station == null) return false;

            station.filterHealth = 100f;
            station.condition = MathF.Min(100f, station.condition + 15f);
            _state.totalMaintenanceActions++;
            OnWaystationStateChanged?.Invoke(station);
            return true;
        }

        public bool AssignWatch(string stationId, IEnumerable<string> survivorIds)
        {
            var station = GetStation(stationId);
            if (station == null) return false;

            station.assignedWatchSurvivorIds = survivorIds != null
                ? new List<string>(survivorIds)
                : new List<string>();

            OnWaystationStateChanged?.Invoke(station);
            return true;
        }

        public WaystationNetworkState CaptureState() => _state;

        public void RestoreState(WaystationNetworkState state)
        {
            _state = state ?? new WaystationNetworkState();
        }
    }
}
