using System;
using System.Collections.Generic;

using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class SumpFloodingState
    {
        public string systemId = SumpFloodingSystem.SystemId;
        public List<SumpNode> nodes = new List<SumpNode>();
        public float globalGroundwaterLevel;
        public int lastFloodDay = -1;
        public List<FloodIncident> incidentLog = new List<FloodIncident>();
    }

    [Serializable]
    public sealed class SumpNode
    {
        public string nodeId = string.Empty;
        public string displayName = string.Empty;
        public float waterLevelCm;
        public float maxWaterLevelCm = 200f;
        public bool hasSumpPump;
        public float pumpCondition = 100f;
        public bool pumpPowered;
        public bool hasFloatValve;
        public bool hasSandbagMitigation;
        public bool isFlooded;
        public bool equipmentDisabled;
        public float contaminationLevel;     // 0-1
        public List<string> adjacentNodeIds = new List<string>();
    }

    [Serializable]
    public sealed class FloodIncident
    {
        public int day;
        public string nodeId = string.Empty;
        public FloodIncidentKind kind;
        public string description = string.Empty;
    }

    public enum FloodIncidentKind { PumpFailure, FloodStart, EquipmentDisabled, DrainComplete, Contamination }

    public sealed class SumpFloodingSystem
    {
        public const string SystemId = "sump_flooding";
        private SumpFloodingState _state = new SumpFloodingState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly WeatherSystem _weather;
        private readonly PowerGridSystem _powerGrid;
        private readonly YearOfAshDeepFreezeSystem _deepFreeze;
        private int _currentDay;

        public SumpFloodingState State => _state;
        public event Action<FloodIncident> OnIncident;
        public event Action OnFloodingChanged;

        public SumpFloodingSystem(
            ISeededRng rng,
            WeatherSystem weather,
            PowerGridSystem powerGrid,
            YearOfAshDeepFreezeSystem deepFreeze,
            ILog log = null!)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _weather = weather ?? throw new ArgumentNullException(nameof(weather));
            _powerGrid = powerGrid ?? throw new ArgumentNullException(nameof(powerGrid));
            _deepFreeze = deepFreeze ?? throw new ArgumentNullException(nameof(deepFreeze));
            _log = log ?? NullLog.Instance;
        }

        public ActionResult AddNode(string nodeId, string displayName, float maxWaterLevelCm = 200f)
        {
            if (_state.nodes.Exists(n => n.nodeId == nodeId))
                return ActionResult.Blocked("node_exists", "sump.node_exists");

            _state.nodes.Add(new SumpNode
            {
                nodeId = nodeId, displayName = displayName, maxWaterLevelCm = maxWaterLevelCm
            });
            OnFloodingChanged?.Invoke();
            return ActionResult.Success("sump.node_added");
        }

        public ActionResult InstallPump(string nodeId)
        {
            var node = _state.nodes.Find(n => n.nodeId == nodeId);
            if (node == null) return ActionResult.Failed("unknown_node", "sump.unknown_node");
            if (node.hasSumpPump) return ActionResult.Blocked("pump_exists", "sump.pump_exists");

            node.hasSumpPump = true;
            node.pumpCondition = 100f;
            OnFloodingChanged?.Invoke();
            return ActionResult.Success("sump.pump_installed");
        }

        public ActionResult SetNodePower(string nodeId, bool powered)
        {
            var node = _state.nodes.Find(n => n.nodeId == nodeId);
            if (node == null) return ActionResult.Failed("unknown_node", "sump.unknown_node");
            if (!node.hasSumpPump) return ActionResult.Blocked("no_pump", "sump.no_pump");

            node.pumpPowered = powered;
            OnFloodingChanged?.Invoke();
            return ActionResult.Success("sump.power_set");
        }

        public ActionResult AddMitigation(string nodeId, string mitigationType)
        {
            var node = _state.nodes.Find(n => n.nodeId == nodeId);
            if (node == null) return ActionResult.Failed("unknown_node", "sump.unknown_node");

            switch (mitigationType)
            {
                case "float_valve":
                    node.hasFloatValve = true;
                    break;
                case "sandbag":
                    node.hasSandbagMitigation = true;
                    break;
                default:
                    return ActionResult.Failed("unknown_mitigation", "sump.unknown_mitigation");
            }
            OnFloodingChanged?.Invoke();
            return ActionResult.Success($"sump.{mitigationType}_added");
        }

        public void TickDay(int day)
        {
            _currentDay = day;

            // Weather input: rain and storms increase groundwater
            float weatherInput = _weather.Current switch
            {
                WeatherKind.Rain => 5f,
                WeatherKind.Ashfall => 8f,
                WeatherKind.FalloutStorm => 20f,
                WeatherKind.Overcast => 2f,
                _ => 0f
            };
            if (_deepFreeze.IsIntakeBlocked)
                weatherInput *= 0.3f; // frozen ground reduces infiltration

            _state.globalGroundwaterLevel = Math.Max(0, _state.globalGroundwaterLevel + weatherInput * 0.1f);

            // Process each node
            foreach (var node in _state.nodes)
            {
                if (node.isFlooded && node.equipmentDisabled) continue;

                // Inflow from groundwater
                float inflow = _state.globalGroundwaterLevel * 0.5f;
                if (_deepFreeze.IsIntakeBlocked)
                    inflow *= 0.2f; // frozen ground

                // Mitigation reduces inflow
                if (node.hasFloatValve) inflow *= 0.5f;
                if (node.hasSandbagMitigation) inflow *= 0.3f;

                // Pump drainage
                float drainage = 0f;
                if (node.hasSumpPump && node.pumpPowered && node.pumpCondition > 0)
                {
                    bool hasPower = _powerGrid.IsRoomPowered(node.nodeId);
                    if (hasPower)
                    {
                        drainage = 20f * (node.pumpCondition / 100f);
                        node.pumpCondition = Math.Max(0, node.pumpCondition - 0.1f);
                    }
                    else
                    {
                        // Pump failure due to no power
                        if (_rng.NextDouble() < 0.1f)
                        {
                            node.pumpCondition = Math.Max(0, node.pumpCondition - 5f);
                            if (node.pumpCondition <= 0)
                            {
                                var incident = new FloodIncident
                                {
                                    day = day, nodeId = node.nodeId,
                                    kind = FloodIncidentKind.PumpFailure,
                                    description = $"Sump pump failed in {node.displayName} (no power)"
                                };
                                _state.incidentLog.Add(incident);
                                OnIncident?.Invoke(incident);
                            }
                        }
                    }
                }

                node.waterLevelCm = Math.Max(0, node.waterLevelCm + inflow - drainage);

                // Flood threshold
                if (node.waterLevelCm > node.maxWaterLevelCm * 0.8f && !node.isFlooded)
                {
                    node.isFlooded = true;
                    node.contaminationLevel = Math.Min(1f, node.contaminationLevel + 0.2f);
                    _state.lastFloodDay = day;

                    var incident = new FloodIncident
                    {
                        day = day, nodeId = node.nodeId,
                        kind = FloodIncidentKind.FloodStart,
                        description = $"{node.displayName} flooded ({node.waterLevelCm:F0}cm)"
                    };
                    _state.incidentLog.Add(incident);
                    _log.Warn($"[Sump] {incident.description}");
                    OnIncident?.Invoke(incident);
                }

                // Equipment disablement
                if (node.isFlooded && node.waterLevelCm > node.maxWaterLevelCm * 0.9f)
                {
                    node.equipmentDisabled = true;
                    var incident = new FloodIncident
                    {
                        day = day, nodeId = node.nodeId,
                        kind = FloodIncidentKind.EquipmentDisabled,
                        description = $"Equipment disabled in flooded {node.displayName}"
                    };
                    _state.incidentLog.Add(incident);
                    OnIncident?.Invoke(incident);
                }

                // Slow natural drainage
                if (!node.isFlooded && node.waterLevelCm > 0)
                {
                    node.waterLevelCm = Math.Max(0, node.waterLevelCm - 2f);
                    if (node.waterLevelCm == 0)
                    {
                        // Bug-08: when the node finishes draining, lift the
                        // equipmentDisabled latch. Otherwise the node stays
                        // unusable even when dry, forcing the player to call
                        // DrainNode manually.
                        node.equipmentDisabled = false;

                        var incident = new FloodIncident
                        {
                            day = day, nodeId = node.nodeId,
                            kind = FloodIncidentKind.DrainComplete,
                            description = $"{node.displayName} drained"
                        };
                        _state.incidentLog.Add(incident);
                        OnIncident?.Invoke(incident);
                    }
                }
            }

            OnFloodingChanged?.Invoke();
        }

        public ActionResult DrainNode(string nodeId)
        {
            var node = _state.nodes.Find(n => n.nodeId == nodeId);
            if (node == null) return ActionResult.Failed("unknown_node", "sump.unknown_node");

            node.waterLevelCm = Math.Max(0, node.waterLevelCm - 50f);
            if (node.waterLevelCm < 10f)
            {
                node.isFlooded = false;
                node.equipmentDisabled = false;
            }
            OnFloodingChanged?.Invoke();
            return ActionResult.Success("sump.node_drained",
                new Dictionary<string, double> { { "water_level", node.waterLevelCm } });
        }

        public bool IsNodeAvailable(string nodeId)
        {
            var node = _state.nodes.Find(n => n.nodeId == nodeId);
            if (node == null) return false;
            return !node.isFlooded || !node.equipmentDisabled;
        }

        public SumpFloodingState CaptureState() => _state;
        public void RestoreState(SumpFloodingState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnFloodingChanged?.Invoke();
        }
    }
}
