// SPDX-License-Identifier: MIT
// ASHFALL Core: deterministic shelter fluid distribution.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ashfall.Core.IO;

namespace Ashfall.Core.Shelter
{
    public enum FluidNodeType { Source, Reservoir, Pump, Sink }
    public enum FluidQualityBand { Potable, Questionable, Unsafe, Toxic }

    [Serializable]
    public sealed class FluidQuality
    {
        public float pathogen01;
        public float chemical01;
        public float radiological01;
        public float salinity01;
        public float sediment01;

        public FluidQualityBand Band
        {
            get
            {
                float severe = Math.Max(Math.Max(pathogen01, chemical01), Math.Max(radiological01, salinity01));
                if (severe >= 0.75f || sediment01 >= 0.9f) return FluidQualityBand.Toxic;
                if (pathogen01 > 0.1f || chemical01 > 0.05f || radiological01 > 0.05f || salinity01 > 0.15f || sediment01 > 0.2f)
                    return FluidQualityBand.Unsafe;
                if (pathogen01 > 0.02f || chemical01 > 0.01f || radiological01 > 0.01f || salinity01 > 0.05f || sediment01 > 0.05f)
                    return FluidQualityBand.Questionable;
                return FluidQualityBand.Potable;
            }
        }

        public FluidQuality Clone() => new FluidQuality
        {
            pathogen01 = Math.Clamp(pathogen01, 0f, 1f),
            chemical01 = Math.Clamp(chemical01, 0f, 1f),
            radiological01 = Math.Clamp(radiological01, 0f, 1f),
            salinity01 = Math.Clamp(salinity01, 0f, 1f),
            sediment01 = Math.Clamp(sediment01, 0f, 1f)
        };

        public static FluidQuality Mix(FluidQuality first, float firstVolume, FluidQuality second, float secondVolume)
        {
            float total = Math.Max(0f, firstVolume) + Math.Max(0f, secondVolume);
            if (total <= 0f) return new FluidQuality();
            float a = Math.Max(0f, firstVolume) / total;
            float b = Math.Max(0f, secondVolume) / total;
            return new FluidQuality
            {
                pathogen01 = Math.Clamp(first.pathogen01 * a + second.pathogen01 * b, 0f, 1f),
                chemical01 = Math.Clamp(first.chemical01 * a + second.chemical01 * b, 0f, 1f),
                radiological01 = Math.Clamp(first.radiological01 * a + second.radiological01 * b, 0f, 1f),
                salinity01 = Math.Clamp(first.salinity01 * a + second.salinity01 * b, 0f, 1f),
                sediment01 = Math.Clamp(first.sediment01 * a + second.sediment01 * b, 0f, 1f)
            };
        }
    }

    [Serializable]
    public sealed class FluidPipeDef
    {
        [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
        [JsonPropertyName("diameter_class")] public string DiameterClass { get; set; } = "standard";
        [JsonPropertyName("max_pressure")] public float MaxPressure { get; set; } = 1f;
        [JsonPropertyName("flow_resistance")] public float FlowResistance { get; set; } = 0.1f;
        [JsonPropertyName("freeze_tolerance")] public float FreezeTolerance { get; set; } = -5f;
        [JsonPropertyName("condition_decay")] public float ConditionDecay { get; set; } = 0.01f;
        [JsonPropertyName("repair_item_ids")] public List<string> RepairItemIds { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class FluidPumpDef
    {
        [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
        [JsonPropertyName("max_flow")] public float MaxFlow { get; set; } = 10f;
        [JsonPropertyName("head_pressure")] public float HeadPressure { get; set; } = 1f;
        [JsonPropertyName("power_draw")] public float PowerDraw { get; set; } = 1f;
        [JsonPropertyName("efficiency")] public float Efficiency { get; set; } = 1f;
        [JsonPropertyName("condition_decay")] public float ConditionDecay { get; set; } = 0.01f;
    }

    [Serializable]
    public sealed class FluidReservoirDef
    {
        [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
        [JsonPropertyName("capacity")] public float Capacity { get; set; } = 100f;
        [JsonPropertyName("base_leak_rate")] public float BaseLeakRate { get; set; } = 0.01f;
        [JsonPropertyName("allowed_water_classes")] public List<string> AllowedWaterClasses { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class FluidInfrastructureCatalog
    {
        [JsonPropertyName("schema_version")] public int SchemaVersion { get; set; } = 1;
        [JsonPropertyName("pipes")] public List<FluidPipeDef> Pipes { get; set; } = new List<FluidPipeDef>();
        [JsonPropertyName("pumps")] public List<FluidPumpDef> Pumps { get; set; } = new List<FluidPumpDef>();
        [JsonPropertyName("reservoirs")] public List<FluidReservoirDef> Reservoirs { get; set; } = new List<FluidReservoirDef>();
    }

    public static class FluidInfrastructureCatalogLoader
    {
        public const string FileName = "fluid_infrastructure.json";

        public static FluidInfrastructureCatalog Load(string dataDir, IFileIO files, IJsonSerializer serializer)
        {
            if (files == null || serializer == null || string.IsNullOrWhiteSpace(dataDir)) return new FluidInfrastructureCatalog();
            string path = files.Combine(dataDir, FileName);
            if (!files.FileExists(path)) return new FluidInfrastructureCatalog();
            try
            {
                return JsonSerializer.Deserialize<FluidInfrastructureCatalog>(files.ReadAllText(path), SystemTextJsonSerializer.Options)
                    ?? new FluidInfrastructureCatalog();
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(FileName, "FluidInfrastructureCatalogLoader", ex);
                return new FluidInfrastructureCatalog();
            }
        }

        public static bool Validate(FluidInfrastructureCatalog catalog, out string error)
        {
            error = string.Empty;
            if (catalog == null) { error = "Fluid infrastructure catalog is null."; return false; }
            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var pipe in catalog.Pipes ?? new List<FluidPipeDef>())
            {
                if (pipe == null || string.IsNullOrWhiteSpace(pipe.Id) || !ids.Add(pipe.Id)
                    || pipe.MaxPressure <= 0f || pipe.FlowResistance < 0f || pipe.ConditionDecay < 0f || pipe.ConditionDecay > 1f)
                { error = "Invalid or duplicate fluid pipe definition."; return false; }
            }
            ids.Clear();
            foreach (var pump in catalog.Pumps ?? new List<FluidPumpDef>())
            {
                if (pump == null || string.IsNullOrWhiteSpace(pump.Id) || !ids.Add(pump.Id)
                    || pump.MaxFlow <= 0f || pump.HeadPressure < 0f || pump.PowerDraw < 0f
                    || pump.Efficiency <= 0f || pump.Efficiency > 1f)
                { error = "Invalid or duplicate fluid pump definition."; return false; }
            }
            ids.Clear();
            foreach (var reservoir in catalog.Reservoirs ?? new List<FluidReservoirDef>())
            {
                if (reservoir == null || string.IsNullOrWhiteSpace(reservoir.Id) || !ids.Add(reservoir.Id)
                    || reservoir.Capacity <= 0f || reservoir.BaseLeakRate < 0f || reservoir.BaseLeakRate > 1f)
                { error = "Invalid or duplicate fluid reservoir definition."; return false; }
            }
            return true;
        }
    }

    [Serializable]
    public sealed class FluidNodeState
    {
        public string nodeId = string.Empty;
        public FluidNodeType nodeType;
        public float capacity = 100f;
        public float volume;
        public FluidQuality quality = new FluidQuality();
        public float pressure = 1f;
        public float demand;
        public int priority = 5;
        public bool enabled = true;
        public bool pumpPowered = true;
        public float pumpHeadPressure;
        public float pumpMaxFlow = 100f;
    }

    [Serializable]
    public sealed class FluidEdgeState
    {
        public string edgeId = string.Empty;
        public string fromNodeId = string.Empty;
        public string toNodeId = string.Empty;
        public string pipeDefId = string.Empty;
        public float valveOpen01 = 1f;
        public float condition01 = 1f;
        public float leakRate;
        public bool frozen;
        public bool burst;
        public float maxFlow = 100f;
        public float resistance = 0.1f;
        public float lastPressure;
    }

    [Serializable]
    public sealed class FluidLogisticsState
    {
        public string systemId = FluidLogisticsSystem.SystemId;
        public List<FluidNodeState> nodes = new List<FluidNodeState>();
        public List<FluidEdgeState> edges = new List<FluidEdgeState>();
        public int lastSolvedDay;
        public float totalVolumeLost;
        public float totalVolumeDelivered;
    }

    [Serializable]
    public sealed class FluidDistributionReport
    {
        public int day;
        public float startingVolume;
        public float endingVolume;
        public float deliveredVolume;
        public float leakedVolume;
        public float massBalanceError;
        public Dictionary<string, float> deliveredBySink = new Dictionary<string, float>(StringComparer.Ordinal);
        public Dictionary<string, FluidQuality> qualityBySink = new Dictionary<string, FluidQuality>(StringComparer.Ordinal);

        public float DeliveredTo(string sinkId)
            => deliveredBySink.TryGetValue(sinkId ?? string.Empty, out float amount) ? amount : 0f;
    }

    /// <summary>
    /// Routes allocated water through a shelter graph. Bulk water quantities
    /// remain owned by WaterTreatmentSystem; this state only tracks network
    /// storage, in-network losses, and delivery outcomes.
    /// </summary>
    public sealed class FluidLogisticsSystem
    {
        public const string SystemId = "fluid_logistics";
        public const string DefaultReservoirId = "reservoir_main";
        public const string DefaultGreenhouseSinkId = "sink_greenhouse";
        public const string DefaultDrinkingSinkId = "sink_drinking";
        public const string DefaultPipeGreenhouseId = "pipe_res_gh";
        public const string DefaultPipeDrinkingId = "pipe_res_drink";

        private FluidLogisticsState _state;
        private readonly Dictionary<string, FluidPipeDef> _pipes = new Dictionary<string, FluidPipeDef>(StringComparer.Ordinal);
        private readonly Dictionary<string, FluidPumpDef> _pumps = new Dictionary<string, FluidPumpDef>(StringComparer.Ordinal);
        private readonly Dictionary<string, float> _lastDelivered = new Dictionary<string, float>(StringComparer.Ordinal);
        private readonly Dictionary<string, FluidQuality> _lastQuality = new Dictionary<string, FluidQuality>(StringComparer.Ordinal);
        private readonly ILog _log;
        private ISeededRng _rng;

        public FluidLogisticsState State => _state;
        public FluidDistributionReport LastReport { get; private set; } = new FluidDistributionReport();
        public IReadOnlyDictionary<string, FluidPipeDef> Pipes => _pipes;
        public IReadOnlyDictionary<string, FluidPumpDef> Pumps => _pumps;

        public event Action<string>? OnPipeBurst;
        public event Action<FluidDistributionReport>? OnDistributionSolved;
        public event Action? OnStateChanged;

        public FluidLogisticsSystem(ILog? log = null, FluidLogisticsState? state = null)
        {
            _log = log ?? NullLog.Instance;
            _state = state ?? new FluidLogisticsState();
            _rng = new SeededRng(168);
        }

        public void BindRng(ISeededRng rng) => _rng = rng ?? new SeededRng(168);

        public void LoadCatalog(FluidInfrastructureCatalog catalog)
        {
            _pipes.Clear();
            _pumps.Clear();
            foreach (var pipe in catalog?.Pipes ?? new List<FluidPipeDef>())
                if (pipe != null && !string.IsNullOrWhiteSpace(pipe.Id) && !_pipes.ContainsKey(pipe.Id)) _pipes[pipe.Id] = pipe;
            foreach (var pump in catalog?.Pumps ?? new List<FluidPumpDef>())
                if (pump != null && !string.IsNullOrWhiteSpace(pump.Id) && !_pumps.ContainsKey(pump.Id)) _pumps[pump.Id] = pump;
        }

        public bool AddNode(FluidNodeState node)
        {
            if (node == null || string.IsNullOrWhiteSpace(node.nodeId) || _state.nodes.Any(n => n.nodeId == node.nodeId)) return false;
            node.capacity = Math.Max(0f, node.capacity);
            node.volume = Math.Clamp(node.volume, 0f, node.capacity);
            node.quality ??= new FluidQuality();
            _state.nodes.Add(CloneNode(node));
            return true;
        }

        public bool AddEdge(FluidEdgeState edge)
        {
            if (edge == null || string.IsNullOrWhiteSpace(edge.edgeId) || _state.edges.Any(e => e.edgeId == edge.edgeId)) return false;
            if (FindNode(edge.fromNodeId) == null || FindNode(edge.toNodeId) == null) return false;
            if (!string.IsNullOrWhiteSpace(edge.pipeDefId) && _pipes.TryGetValue(edge.pipeDefId, out var pipe))
            {
                edge.maxFlow = pipe.MaxPressure * 100f;
                edge.resistance = pipe.FlowResistance;
            }
            _state.edges.Add(CloneEdge(edge));
            return true;
        }

        /// <summary>
        /// Plan 168: seed the shelter default graph when empty/partial.
        /// Idempotent — existing matching node/edge ids are left alone.
        /// </summary>
        public bool EnsureDefaultShelterTopology(
            float reservoirCapacity = 200f,
            float greenhouseDemand = 20f,
            float drinkingDemand = 30f)
        {
            bool changed = false;
            if (FindNode(DefaultReservoirId) == null)
            {
                changed |= AddNode(new FluidNodeState
                {
                    nodeId = DefaultReservoirId,
                    nodeType = FluidNodeType.Reservoir,
                    capacity = Math.Max(1f, reservoirCapacity),
                    pressure = 1f,
                    quality = new FluidQuality()
                });
            }

            if (FindNode(DefaultGreenhouseSinkId) == null)
            {
                changed |= AddNode(new FluidNodeState
                {
                    nodeId = DefaultGreenhouseSinkId,
                    nodeType = FluidNodeType.Sink,
                    demand = Math.Max(0f, greenhouseDemand),
                    priority = 2,
                    pressure = 0.5f
                });
            }
            else
            {
                changed |= ConfigureSink(DefaultGreenhouseSinkId, Math.Max(0f, greenhouseDemand), 2);
            }

            if (FindNode(DefaultDrinkingSinkId) == null)
            {
                changed |= AddNode(new FluidNodeState
                {
                    nodeId = DefaultDrinkingSinkId,
                    nodeType = FluidNodeType.Sink,
                    demand = Math.Max(0f, drinkingDemand),
                    priority = 1,
                    pressure = 0.5f
                });
            }
            else
            {
                changed |= ConfigureSink(DefaultDrinkingSinkId, Math.Max(0f, drinkingDemand), 1);
            }

            if (FindEdge(DefaultPipeGreenhouseId) == null)
            {
                changed |= AddEdge(new FluidEdgeState
                {
                    edgeId = DefaultPipeGreenhouseId,
                    fromNodeId = DefaultReservoirId,
                    toNodeId = DefaultGreenhouseSinkId,
                    maxFlow = 100f,
                    resistance = 0.1f
                });
            }

            if (FindEdge(DefaultPipeDrinkingId) == null)
            {
                changed |= AddEdge(new FluidEdgeState
                {
                    edgeId = DefaultPipeDrinkingId,
                    fromNodeId = DefaultReservoirId,
                    toNodeId = DefaultDrinkingSinkId,
                    maxFlow = 100f,
                    resistance = 0.1f
                });
            }

            if (changed) OnStateChanged?.Invoke();
            return FindNode(DefaultReservoirId) != null
                && FindNode(DefaultGreenhouseSinkId) != null
                && FindNode(DefaultDrinkingSinkId) != null
                && FindEdge(DefaultPipeGreenhouseId) != null
                && FindEdge(DefaultPipeDrinkingId) != null;
        }

        public float GetFreeCapacity(string nodeId)
        {
            var node = FindNode(nodeId);
            if (node == null) return 0f;
            return Math.Max(0f, node.capacity - node.volume);
        }

        public bool InjectWater(string nodeId, float amount, FluidQuality quality)
        {
            if (amount <= 0f || float.IsNaN(amount) || float.IsInfinity(amount)) return false;
            var node = FindNode(nodeId);
            if (node == null || (node.nodeType != FluidNodeType.Source && node.nodeType != FluidNodeType.Reservoir)) return false;
            if (!CanAcceptWater(nodeId, amount)) return false;
            var incoming = quality?.Clone() ?? new FluidQuality();
            node.quality = FluidQuality.Mix(node.quality, node.volume, incoming, amount);
            node.volume += amount;
            OnStateChanged?.Invoke();
            return true;
        }

        public bool CanAcceptWater(string nodeId, float amount)
        {
            if (amount <= 0f || float.IsNaN(amount) || float.IsInfinity(amount)) return false;
            var node = FindNode(nodeId);
            return node != null
                && (node.nodeType == FluidNodeType.Source || node.nodeType == FluidNodeType.Reservoir)
                && node.volume + amount <= node.capacity + 0.0001f;
        }

        public bool SetValve(string edgeId, float open01)
        {
            var edge = FindEdge(edgeId);
            if (edge == null) return false;
            edge.valveOpen01 = Math.Clamp(open01, 0f, 1f);
            OnStateChanged?.Invoke();
            return true;
        }

        public bool SetPumpState(string nodeId, bool powered)
        {
            var node = FindNode(nodeId);
            if (node == null || node.nodeType != FluidNodeType.Pump) return false;
            node.pumpPowered = powered;
            OnStateChanged?.Invoke();
            return true;
        }

        public bool ConfigureSink(string nodeId, float demand, int priority)
        {
            var node = FindNode(nodeId);
            if (node == null || node.nodeType != FluidNodeType.Sink || demand < 0f) return false;
            node.demand = demand;
            node.priority = Math.Clamp(priority, 0, 100);
            OnStateChanged?.Invoke();
            return true;
        }

        public bool RepairPipe(string edgeId, float condition01 = 1f)
        {
            var edge = FindEdge(edgeId);
            if (edge == null) return false;
            edge.condition01 = Math.Clamp(condition01, 0f, 1f);
            edge.burst = false;
            edge.frozen = false;
            OnStateChanged?.Invoke();
            return true;
        }

        public FluidDistributionReport Tick(int day, float outdoorTemperatureC, float powerAvailability01 = 1f)
        {
            EvaluatePipeHazards(day, outdoorTemperatureC);
            return Solve(day, powerAvailability01);
        }

        public FluidDistributionReport Solve(int day, float powerAvailability01 = 1f)
        {
            powerAvailability01 = Math.Clamp(powerAvailability01, 0f, 1f);
            var report = new FluidDistributionReport { day = day };
            foreach (var node in _state.nodes.Where(n => n != null && (n.nodeType == FluidNodeType.Source || n.nodeType == FluidNodeType.Reservoir)))
                report.startingVolume += Math.Max(0f, node.volume);

            var sinks = _state.nodes.Where(n => n != null && n.enabled && n.nodeType == FluidNodeType.Sink && n.demand > 0f)
                .OrderBy(n => n.priority).ThenBy(n => n.nodeId, StringComparer.Ordinal).ToList();
            var qualityVolumes = new Dictionary<string, float>(StringComparer.Ordinal);
            foreach (var sink in sinks)
            {
                float remaining = sink.demand;
                var sourceNodes = _state.nodes.Where(n => n != null && n.enabled
                    && (n.nodeType == FluidNodeType.Source || n.nodeType == FluidNodeType.Reservoir) && n.volume > 0f)
                    .OrderBy(n => n.nodeId, StringComparer.Ordinal).ToList();
                foreach (var source in sourceNodes)
                {
                    if (remaining <= 0.00001f) break;
                    var path = FindPath(source.nodeId, sink.nodeId);
                    if (path.Count == 0 || !PathPowered(path, powerAvailability01)) continue;
                    float pressure = PathPressure(source, path);
                    foreach (var edge in path) edge.lastPressure = pressure;
                    if (pressure < sink.pressure) continue;
                    float pathCapacity = path.Min(e => Math.Max(0f, e.maxFlow * e.valveOpen01 * e.condition01));
                    float pumpCapacity = path
                        .SelectMany(e => new[] { FindNode(e.fromNodeId), FindNode(e.toNodeId) })
                        .Where(n => n?.nodeType == FluidNodeType.Pump)
                        .Select(n => Math.Max(0f, n!.pumpMaxFlow) * powerAvailability01)
                        .DefaultIfEmpty(float.PositiveInfinity)
                        .Min();
                    pathCapacity = Math.Min(pathCapacity, pumpCapacity);
                    float available = Math.Min(remaining, Math.Min(source.volume, pathCapacity));
                    if (available <= 0f) continue;
                    float leakRate = Math.Clamp(path.Sum(e => Math.Max(0f, e.leakRate)), 0f, 1f);
                    float leaked = Math.Min(available, available * leakRate);
                    float delivered = available - leaked;
                    source.volume -= available;
                    remaining -= delivered;
                    report.leakedVolume += leaked;
                    report.deliveredVolume += delivered;
                    if (!report.deliveredBySink.ContainsKey(sink.nodeId)) report.deliveredBySink[sink.nodeId] = 0f;
                    report.deliveredBySink[sink.nodeId] += delivered;
                    AddQuality(report.qualityBySink, qualityVolumes, sink.nodeId, source.quality, delivered);
                }
            }

            foreach (var node in _state.nodes.Where(n => n != null && (n.nodeType == FluidNodeType.Source || n.nodeType == FluidNodeType.Reservoir)))
                report.endingVolume += Math.Max(0f, node.volume);
            report.massBalanceError = report.startingVolume - report.endingVolume - report.deliveredVolume - report.leakedVolume;
            _state.lastSolvedDay = day;
            _state.totalVolumeDelivered += report.deliveredVolume;
            _state.totalVolumeLost += report.leakedVolume;
            LastReport = report;
            _lastDelivered.Clear();
            _lastQuality.Clear();
            foreach (var pair in report.deliveredBySink) _lastDelivered[pair.Key] = pair.Value;
            foreach (var pair in report.qualityBySink) _lastQuality[pair.Key] = pair.Value.Clone();
            OnDistributionSolved?.Invoke(report);
            OnStateChanged?.Invoke();
            return report;
        }

        public float GetDeliveredVolume(string sinkId)
            => _lastDelivered.TryGetValue(sinkId ?? string.Empty, out float amount) ? amount : 0f;

        public FluidQuality GetDeliveredQuality(string sinkId)
            => _lastQuality.TryGetValue(sinkId ?? string.Empty, out var quality) ? quality.Clone() : new FluidQuality();

        private void EvaluatePipeHazards(int day, float outdoorTemperatureC)
        {
            foreach (var edge in _state.edges.OrderBy(e => e.edgeId, StringComparer.Ordinal).ToList())
            {
                if (edge == null || edge.burst || edge.frozen) continue;
                FluidPipeDef? definition = null;
                if (!string.IsNullOrWhiteSpace(edge.pipeDefId)) _pipes.TryGetValue(edge.pipeDefId, out definition);
                float freezeTolerance = definition?.FreezeTolerance ?? -5f;
                float conditionRisk = Math.Max(0f, 0.35f - edge.condition01) * 0.8f;
                float freezeRisk = outdoorTemperatureC < freezeTolerance ? Math.Clamp((freezeTolerance - outdoorTemperatureC) * 0.05f, 0f, 0.6f) : 0f;
                float pressureRisk = edge.lastPressure > 0f && edge.lastPressure > (definition?.MaxPressure ?? 1f) ? 0.6f : 0f;
                float risk = Math.Clamp(conditionRisk + freezeRisk + pressureRisk, 0f, 0.9f);
                if (risk <= 0f || _rng.NextDouble() >= risk) continue;

                // Freeze dominates when cold is the primary hazard and pressure
                // is within rating — pipe blocks flow but remains repairable.
                bool freezeDominates = freezeRisk >= conditionRisk && freezeRisk >= pressureRisk && freezeRisk > 0f;
                if (freezeDominates)
                {
                    edge.frozen = true;
                    _log.Warn($"[Fluid] pipe frozen: {edge.edgeId} on day {day}");
                    continue;
                }

                edge.burst = true;
                edge.condition01 = 0f;
                OnPipeBurst?.Invoke(edge.edgeId);
                _log.Warn($"[Fluid] pipe burst: {edge.edgeId} on day {day}");
            }
        }

        private bool PathPowered(List<FluidEdgeState> path, float powerAvailability01)
        {
            foreach (var edge in path)
            {
                var from = FindNode(edge.fromNodeId);
                if (from?.nodeType == FluidNodeType.Pump && (!from.pumpPowered || powerAvailability01 <= 0f)) return false;
                var to = FindNode(edge.toNodeId);
                if (to?.nodeType == FluidNodeType.Pump && (!to.pumpPowered || powerAvailability01 <= 0f)) return false;
            }
            return true;
        }

        private float PathPressure(FluidNodeState source, List<FluidEdgeState> path)
        {
            float pressure = Math.Max(0f, source.pressure);
            foreach (var edge in path)
            {
                pressure -= Math.Max(0f, edge.resistance);
                var node = FindNode(edge.toNodeId);
                if (node?.nodeType == FluidNodeType.Pump && node.pumpPowered)
                    pressure += Math.Max(0f, node.pumpHeadPressure);
            }
            return Math.Max(0f, pressure);
        }

        private List<FluidEdgeState> FindPath(string sourceId, string sinkId)
        {
            var queue = new Queue<string>();
            var previous = new Dictionary<string, FluidEdgeState>(StringComparer.Ordinal);
            var visited = new HashSet<string>(StringComparer.Ordinal) { sourceId };
            queue.Enqueue(sourceId);
            while (queue.Count > 0)
            {
                string current = queue.Dequeue();
                if (current == sinkId) break;
                foreach (var edge in _state.edges.Where(e => e != null && e.fromNodeId == current)
                    .OrderBy(e => e.edgeId, StringComparer.Ordinal))
                {
                    if (edge.burst || edge.frozen || edge.valveOpen01 <= 0f || edge.condition01 <= 0f) continue;
                    if (visited.Add(edge.toNodeId))
                    {
                        previous[edge.toNodeId] = edge;
                        queue.Enqueue(edge.toNodeId);
                    }
                }
            }
            if (!visited.Contains(sinkId)) return new List<FluidEdgeState>();
            var path = new List<FluidEdgeState>();
            string cursor = sinkId;
            while (cursor != sourceId)
            {
                if (!previous.TryGetValue(cursor, out var edge)) return new List<FluidEdgeState>();
                path.Add(edge);
                cursor = edge.fromNodeId;
            }
            path.Reverse();
            return path;
        }

        private FluidNodeState? FindNode(string nodeId)
            => _state.nodes.FirstOrDefault(n => n != null && string.Equals(n.nodeId, nodeId, StringComparison.Ordinal));

        private FluidEdgeState? FindEdge(string edgeId)
            => _state.edges.FirstOrDefault(e => e != null && string.Equals(e.edgeId, edgeId, StringComparison.Ordinal));

        private static void AddQuality(Dictionary<string, FluidQuality> target, Dictionary<string, float> volumes,
            string sinkId, FluidQuality source, float amount)
        {
            if (amount <= 0f) return;
            if (!target.TryGetValue(sinkId, out var current))
            {
                target[sinkId] = source.Clone();
                volumes[sinkId] = amount;
                return;
            }
            float previousVolume = volumes.TryGetValue(sinkId, out float existing) ? existing : 0f;
            target[sinkId] = FluidQuality.Mix(current, previousVolume, source, amount);
            volumes[sinkId] = previousVolume + amount;
        }

        public FluidLogisticsState CaptureState()
            => new FluidLogisticsState
            {
                systemId = _state.systemId,
                nodes = _state.nodes.Where(n => n != null).Select(CloneNode).ToList(),
                edges = _state.edges.Where(e => e != null).Select(CloneEdge).ToList(),
                lastSolvedDay = _state.lastSolvedDay,
                totalVolumeLost = _state.totalVolumeLost,
                totalVolumeDelivered = _state.totalVolumeDelivered
            };

        public void RestoreState(FluidLogisticsState saved)
        {
            if (saved == null) return;
            _state = new FluidLogisticsState
            {
                systemId = string.IsNullOrWhiteSpace(saved.systemId) ? SystemId : saved.systemId,
                nodes = saved.nodes?.Where(n => n != null).Select(CloneNode).ToList() ?? new List<FluidNodeState>(),
                edges = saved.edges?.Where(e => e != null).Select(CloneEdge).ToList() ?? new List<FluidEdgeState>(),
                lastSolvedDay = saved.lastSolvedDay,
                totalVolumeLost = Math.Max(0f, saved.totalVolumeLost),
                totalVolumeDelivered = Math.Max(0f, saved.totalVolumeDelivered)
            };
            _lastDelivered.Clear();
            _lastQuality.Clear();
            LastReport = new FluidDistributionReport { day = _state.lastSolvedDay };
            OnStateChanged?.Invoke();
        }

        private static FluidNodeState CloneNode(FluidNodeState source)
            => new FluidNodeState
            {
                nodeId = source.nodeId,
                nodeType = source.nodeType,
                capacity = Math.Max(0f, source.capacity),
                volume = Math.Clamp(source.volume, 0f, Math.Max(0f, source.capacity)),
                quality = source.quality?.Clone() ?? new FluidQuality(),
                pressure = Math.Max(0f, source.pressure),
                demand = Math.Max(0f, source.demand),
                priority = Math.Clamp(source.priority, 0, 100),
                enabled = source.enabled,
                pumpPowered = source.pumpPowered,
                pumpHeadPressure = Math.Max(0f, source.pumpHeadPressure),
                pumpMaxFlow = Math.Max(0f, source.pumpMaxFlow)
            };

        private static FluidEdgeState CloneEdge(FluidEdgeState source)
            => new FluidEdgeState
            {
                edgeId = source.edgeId,
                fromNodeId = source.fromNodeId,
                toNodeId = source.toNodeId,
                pipeDefId = source.pipeDefId,
                valveOpen01 = Math.Clamp(source.valveOpen01, 0f, 1f),
                condition01 = Math.Clamp(source.condition01, 0f, 1f),
                leakRate = Math.Clamp(source.leakRate, 0f, 1f),
                frozen = source.frozen,
                burst = source.burst,
                maxFlow = Math.Max(0f, source.maxFlow),
                resistance = Math.Max(0f, source.resistance),
                lastPressure = Math.Max(0f, source.lastPressure)
            };
    }
}
