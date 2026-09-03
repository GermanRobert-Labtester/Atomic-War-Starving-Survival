using System;
using System.Collections.Generic;
#pragma warning disable CS8618

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
        public string stratumId = string.Empty;   // bound drainage stratum (sump_drainage_catalog)
        public float suspendedSolidsKg;           // silt in suspension; settles daily into settledSludgeKg
        public float settledSludgeKg;             // settled sludge mass awaiting treatment/dredging
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

        // ── Stratum/sludge model constants (sump_drainage_catalog) ──────
        /// <summary>Basin cross-section convention: 1 cm of node level = 10 L of water.</summary>
        public const float BasinLitersPerCmLevel = 10f;
        /// <summary>Fraction of suspended solids that settles into sludge each day.</summary>
        public const float SettledFractionPerDay = 0.15f;
        /// <summary>Solids mass (suspended + settled) at which the wear multiplier reaches 2×.</summary>
        public const float SolidsWearReferenceKg = 10f;
        /// <summary>Cap on the solids contribution to the pump wear multiplier.</summary>
        public const float MaxSolidsWearFactor = 4f;
        /// <summary>Throughput loss factor per kg of suspended solids (viscosity/throttling).</summary>
        public const float ViscosityThroughputPenaltyPerKg = 0.02f;
        /// <summary>Settled sludge above this mass clogs the strainer and halves throughput.</summary>
        public const float StrainerBlockageThresholdKg = 25f;

        private SumpFloodingState _state = new SumpFloodingState();
        private readonly Dictionary<string, SumpStratumDef> _strata = new Dictionary<string, SumpStratumDef>(StringComparer.Ordinal);
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
ILog? log = null)
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

        /// <summary>Registers drainage strata from sump_drainage_catalog.json. Returns count applied.</summary>
        public int ApplyStratumCatalog(IEnumerable<SumpStratumDef> defs)
        {
            if (defs == null) return 0;
            int applied = 0;
            foreach (var def in defs)
            {
                if (def == null || string.IsNullOrEmpty(def.stratum_id)) continue;
                _strata[def.stratum_id] = def;
                applied++;
            }
            return applied;
        }

        /// <summary>Binds a node to a catalog stratum. Nodes without a stratum keep the legacy inflow model.</summary>
        public ActionResult AssignStratum(string nodeId, string stratumId)
        {
            var node = _state.nodes.Find(n => n.nodeId == nodeId);
            if (node == null) return ActionResult.Failed("unknown_node", "sump.unknown_node");
            if (string.IsNullOrEmpty(stratumId) || !_strata.ContainsKey(stratumId))
                return ActionResult.Failed("unknown_stratum", "sump.unknown_stratum");

            node.stratumId = stratumId;
            OnFloodingChanged?.Invoke();
            return ActionResult.Success("sump.stratum_assigned");
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

                // Inflow from groundwater. Nodes bound to a drainage stratum use
                // catalog-driven ingress scaled by live groundwater pressure;
                // unbound nodes keep the legacy flat model.
                _strata.TryGetValue(node.stratumId, out var stratum);
                float inflow;
                float siltFraction = 0f;
                float pumpLoadModifier = 1f;
                int toxicityTier = 0;
                if (stratum != null)
                {
                    float pressure = 0.5f + _state.globalGroundwaterLevel / 10f;
                    inflow = stratum.base_ingress_cm_per_day * pressure * stratum.water_table_pressure;
                    siltFraction = stratum.silt_fraction;
                    pumpLoadModifier = stratum.pump_load_modifier;
                    toxicityTier = stratum.toxicity_tier;
                }
                else
                {
                    inflow = _state.globalGroundwaterLevel * 0.5f;
                }
                if (_deepFreeze.IsIntakeBlocked)
                    inflow *= 0.2f; // frozen ground

                // Mitigation reduces inflow
                if (node.hasFloatValve) inflow *= 0.5f;
                if (node.hasSandbagMitigation) inflow *= 0.3f;

                // Silt carried by the ingress (mass-conserving; settles below).
                node.suspendedSolidsKg += inflow * BasinLitersPerCmLevel * siltFraction;

                // Pump drainage
                float drainage = 0f;
                if (node.hasSumpPump && node.pumpPowered && node.pumpCondition > 0)
                {
                    bool hasPower = _powerGrid.IsRoomPowered(node.nodeId);
                    if (hasPower)
                    {
                        // Solids load: suspended silt throttles throughput (viscosity)
                        // and settled sludge above the strainer threshold blocks the
                        // inlet. Wear scales with total solids and stratum load.
                        float solidsFactor = Math.Min(
                            MaxSolidsWearFactor,
                            (node.suspendedSolidsKg + node.settledSludgeKg) / SolidsWearReferenceKg);
                        float viscosity = 1f / (1f + node.suspendedSolidsKg * ViscosityThroughputPenaltyPerKg);
                        float blockage = node.settledSludgeKg > StrainerBlockageThresholdKg ? 0.5f : 1f;
                        drainage = 20f * (node.pumpCondition / 100f) * viscosity * blockage;
                        node.pumpCondition = Math.Max(0, node.pumpCondition
                            - 0.1f * (1f + pumpLoadModifier * solidsFactor));
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

                // Daily settling: a fixed fraction of suspended solids settles into
                // the sludge layer. Mass is conserved (moved, never deleted).
                float settledMass = node.suspendedSolidsKg * SettledFractionPerDay;
                node.suspendedSolidsKg = Math.Max(0f, node.suspendedSolidsKg - settledMass);
                node.settledSludgeKg += settledMass;

                // Flood threshold
                if (node.waterLevelCm > node.maxWaterLevelCm * 0.8f && !node.isFlooded)
                {
                    node.isFlooded = true;
                    float contaminationGain = stratum != null
                        ? 0.05f * (toxicityTier + 1)
                        : 0.2f;
                    node.contaminationLevel = Math.Min(1f, node.contaminationLevel + contaminationGain);
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

        public SumpFloodingState CaptureState() => CloneState(_state);

        public void RestoreState(SumpFloodingState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static SumpFloodingState CloneState(SumpFloodingState src)
        {
            if (src == null) return new SumpFloodingState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<SumpFloodingState>(json) ?? new SumpFloodingState();
        }
    }
}
