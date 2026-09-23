#nullable enable
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Settlements
{
    /// <summary>
    /// Definition of an authored outpost or secondary holdfast position.
    /// Loaded from StreamingAssets/Data/outposts.json.
    /// </summary>
    public sealed class OutpostDef
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string GraphNodeId { get; set; } = string.Empty;
        public int MaxGarrisonBunks { get; set; } = 4;
        public int DailySupplyDemand { get; set; } = 4;
        public int DefenseRating { get; set; } = 50;
        public int RadioRelayRange { get; set; } = 10;
        public Dictionary<string, int> BuildCost { get; set; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Runtime instance state of an outpost.
    /// Preserves single authority: does not duplicate population or food stores.
    /// </summary>
    public sealed class OutpostInstance
    {
        public string OutpostId { get; set; } = string.Empty;
        public string GraphNodeId { get; set; } = string.Empty;
        public bool IsEstablished { get; set; }
        public int ConditionPermille { get; set; } = 1000;
        public List<string> GarrisonSurvivorIds { get; set; } = new List<string>();
        public int DaysSinceSupply { get; set; }
        public bool IsStarving { get; set; }
        public bool IsOverrun { get; set; }
        public int RationReserve { get; set; }
    }

    /// <summary>
    /// Persisted projection of one outpost instance. Definitions are never
    /// duplicated here: they are re-derived from the authored catalog on
    /// restore, so a definition edit can never be frozen into a save.
    /// </summary>
    public sealed class OutpostInstanceState
    {
        public string outpost_id { get; set; } = string.Empty;
        public bool is_established { get; set; }
        public int condition_permille { get; set; } = 1000;
        public List<string> garrison_survivor_ids { get; set; } = new List<string>();
        public int days_since_supply { get; set; }
        public bool is_starving { get; set; }
        public bool is_overrun { get; set; }
        public int ration_reserve { get; set; }
    }

    /// <summary>
    /// Plan 58 capture state for the outpost settlement section.
    /// </summary>
    public sealed class OutpostSettlementState
    {
        public int schema_version { get; set; } = 1;
        public List<OutpostInstanceState> outposts { get; set; } = new List<OutpostInstanceState>();
    }

    /// <summary>
    /// Pure domain engine for Plan 58: The Continuation — Outposts, Waystations, and a Second Holdfast.
    /// Provides lifecycle (establish, staff, supply, risk, overrun, abandon) without duplicating core authorities.
    /// Zero engine references (Godot/UnityEngine free).
    /// </summary>
    public sealed class OutpostSettlementSystem
    {
        public const string SystemId = "outpost_settlement_system";

        private readonly Dictionary<string, OutpostDef> _definitions = new Dictionary<string, OutpostDef>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, OutpostInstance> _instances = new Dictionary<string, OutpostInstance>(StringComparer.OrdinalIgnoreCase);

        // Seam delegates
        public Action<string, string>? OnOutpostEstablishedSeam { get; set; }
        public Action<string, int>? OnOutpostSuppliedSeam { get; set; }
        public Action<string>? OnOutpostOverrunSeam { get; set; }
        public Action<string, string>? OnGarrisonAssignedSeam { get; set; }
        public Action<string, string>? OnGarrisonRelievedSeam { get; set; }
        public Action<string>? OnOutpostStarvingSeam { get; set; }

        public OutpostSettlementSystem(IEnumerable<OutpostDef>? definitions = null)
        {
            if (definitions != null)
            {
                foreach (var def in definitions)
                {
                    if (def != null && !string.IsNullOrEmpty(def.Id))
                    {
                        _definitions[def.Id] = def;
                        _instances[def.Id] = new OutpostInstance
                        {
                            OutpostId = def.Id,
                            GraphNodeId = def.GraphNodeId,
                            IsEstablished = false,
                            ConditionPermille = 1000,
                            DaysSinceSupply = 0,
                            IsStarving = false,
                            IsOverrun = false,
                            RationReserve = 0
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Factory parser for outposts.json catalog.
        /// </summary>
        public static OutpostSettlementSystem FromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("Outpost JSON cannot be null or empty", nameof(json));

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            var list = new List<OutpostDef>();

            if (root.TryGetProperty("outposts", out var outpostsElem) && outpostsElem.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in outpostsElem.EnumerateArray())
                {
                    var def = new OutpostDef
                    {
                        Id = item.TryGetProperty("id", out var idElem) ? idElem.GetString() ?? string.Empty : string.Empty,
                        Name = item.TryGetProperty("name", out var nameElem) ? nameElem.GetString() ?? string.Empty : string.Empty,
                        GraphNodeId = item.TryGetProperty("graph_node_id", out var nodeElem) ? nodeElem.GetString() ?? string.Empty : string.Empty,
                        MaxGarrisonBunks = item.TryGetProperty("max_garrison_bunks", out var bunksElem) ? bunksElem.GetInt32() : 4,
                        DailySupplyDemand = item.TryGetProperty("daily_supply_demand", out var demandElem) ? demandElem.GetInt32() : 4,
                        DefenseRating = item.TryGetProperty("defense_rating", out var defElem) ? defElem.GetInt32() : 50,
                        RadioRelayRange = item.TryGetProperty("radio_relay_range", out var radioElem) ? radioElem.GetInt32() : 10
                    };

                    if (item.TryGetProperty("build_cost", out var costElem) && costElem.ValueKind == JsonValueKind.Object)
                    {
                        foreach (var prop in costElem.EnumerateObject())
                        {
                            def.BuildCost[prop.Name] = prop.Value.GetInt32();
                        }
                    }

                    list.Add(def);
                }
            }

            return new OutpostSettlementSystem(list);
        }

        public OutpostDef? GetDefinition(string outpostId)
        {
            if (string.IsNullOrEmpty(outpostId)) return null;
            _definitions.TryGetValue(outpostId, out var def);
            return def;
        }

        public OutpostInstance? GetInstance(string outpostId)
        {
            if (string.IsNullOrEmpty(outpostId)) return null;
            _instances.TryGetValue(outpostId, out var inst);
            return inst;
        }

        public IReadOnlyList<OutpostDef> GetAllDefinitions() => _definitions.Values.ToList();
        public IReadOnlyList<OutpostInstance> GetAllInstances() => _instances.Values.ToList();

        /// <summary>
        /// Establishes an outpost. Can optionally consume resources through a caller-provided delegate.
        /// </summary>
        public bool EstablishOutpost(string outpostId, Func<string, int, bool>? costConsumer = null)
        {
            if (!_definitions.TryGetValue(outpostId, out var def)) return false;
            if (!_instances.TryGetValue(outpostId, out var inst)) return false;
            if (inst.IsEstablished) return false;

            if (costConsumer != null)
            {
                foreach (var cost in def.BuildCost)
                {
                    if (!costConsumer(cost.Key, cost.Value))
                        return false;
                }
            }

            inst.IsEstablished = true;
            inst.ConditionPermille = 1000;
            inst.DaysSinceSupply = 0;
            inst.IsStarving = false;
            inst.IsOverrun = false;
            inst.RationReserve = 0;

            OnOutpostEstablishedSeam?.Invoke(outpostId, def.GraphNodeId);
            return true;
        }

        /// <summary>
        /// Assigns a survivor from the central roster to garrison an outpost.
        /// Respects bunks capacity and optional fitness gating.
        /// </summary>
        public bool AssignGarrison(string outpostId, string survivorId, Func<string, bool>? fitnessCheck = null)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            if (!_definitions.TryGetValue(outpostId, out var def)) return false;
            if (!_instances.TryGetValue(outpostId, out var inst)) return false;
            if (!inst.IsEstablished || inst.IsOverrun) return false;

            if (inst.GarrisonSurvivorIds.Contains(survivorId)) return false;
            if (inst.GarrisonSurvivorIds.Count >= def.MaxGarrisonBunks) return false;

            if (fitnessCheck != null && !fitnessCheck(survivorId))
                return false;

            inst.GarrisonSurvivorIds.Add(survivorId);
            OnGarrisonAssignedSeam?.Invoke(outpostId, survivorId);
            return true;
        }

        /// <summary>
        /// Relieves a survivor from an outpost garrison, returning them to central holdfast duty.
        /// </summary>
        public bool RelieveGarrison(string outpostId, string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            if (!_instances.TryGetValue(outpostId, out var inst)) return false;

            if (inst.GarrisonSurvivorIds.Remove(survivorId))
            {
                OnGarrisonRelievedSeam?.Invoke(outpostId, survivorId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Delivers rations to an outpost reserve from a caravan or supply route.
        /// </summary>
        public bool SupplyOutpost(string outpostId, int rationsDelivered)
        {
            if (rationsDelivered <= 0) return false;
            if (!_instances.TryGetValue(outpostId, out var inst)) return false;
            if (!inst.IsEstablished) return false;

            inst.RationReserve += rationsDelivered;
            inst.DaysSinceSupply = 0;
            inst.IsStarving = false;

            OnOutpostSuppliedSeam?.Invoke(outpostId, rationsDelivered);
            return true;
        }

        /// <summary>
        /// Advances the daily outpost lifecycle. Consumes rations and checks for starvation.
        /// Central ration supplier can be passed to draw from main supply or route.
        /// </summary>
        public void TickDay(Func<string, int, int>? centralRationSupplyProvider = null)
        {
            foreach (var kvp in _instances)
            {
                var inst = kvp.Value;
                if (!inst.IsEstablished || inst.IsOverrun) continue;

                if (!_definitions.TryGetValue(inst.OutpostId, out var def)) continue;

                int demand = inst.GarrisonSurvivorIds.Count > 0
                    ? inst.GarrisonSurvivorIds.Count
                    : Math.Max(1, def.DailySupplyDemand / 2);

                if (centralRationSupplyProvider != null)
                {
                    int drawn = centralRationSupplyProvider(inst.OutpostId, demand);
                    if (drawn > 0) inst.RationReserve += drawn;
                }

                if (inst.RationReserve >= demand)
                {
                    inst.RationReserve -= demand;
                    inst.DaysSinceSupply = 0;
                    inst.IsStarving = false;
                    inst.ConditionPermille = Math.Min(1000, inst.ConditionPermille + 5);
                }
                else
                {
                    inst.RationReserve = 0;
                    inst.DaysSinceSupply++;
                    inst.IsStarving = true;
                    inst.ConditionPermille = Math.Max(0, inst.ConditionPermille - 50);
                    OnOutpostStarvingSeam?.Invoke(inst.OutpostId);
                }
            }
        }

        /// <summary>
        /// Simulates hostile wasteland pressure against an outpost.
        /// Returns true if the outpost was overrun.
        /// </summary>
        public bool SimulateRisk(string outpostId, int dangerRating, ISeededRng rng)
        {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (!_definitions.TryGetValue(outpostId, out var def)) return false;
            if (!_instances.TryGetValue(outpostId, out var inst)) return false;
            if (!inst.IsEstablished || inst.IsOverrun) return false;

            int effectiveDefense = def.DefenseRating + (inst.GarrisonSurvivorIds.Count * 10) + (inst.ConditionPermille / 20);

            if (dangerRating > effectiveDefense)
            {
                double roll = rng.NextDouble();
                double overrunChance = Math.Min(0.9, (dangerRating - effectiveDefense) / 100.0);
                if (roll < overrunChance)
                {
                    inst.IsOverrun = true;
                    inst.ConditionPermille = Math.Max(0, inst.ConditionPermille - 300);
                    OnOutpostOverrunSeam?.Invoke(outpostId);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Abandons an established outpost, removing its garrison and clearing its active state.
        /// </summary>
        public bool AbandonOutpost(string outpostId)
        {
            if (!_instances.TryGetValue(outpostId, out var inst)) return false;
            if (!inst.IsEstablished) return false;

            inst.IsEstablished = false;
            inst.GarrisonSurvivorIds.Clear();
            inst.RationReserve = 0;
            inst.IsStarving = false;
            inst.DaysSinceSupply = 0;
            return true;
        }

        /// <summary>
        /// Plan 58 persistence — capture the live outpost state.
        /// Only instance state is projected: definitions stay in the authored
        /// <c>outposts.json</c> authority and are never frozen into a save.
        /// Deterministic: no RNG and no wall-clock is read.
        /// </summary>
        public OutpostSettlementState CaptureState()
        {
            var state = new OutpostSettlementState { schema_version = 1 };
            foreach (var def in _definitions.Values)
            {
                // Definitions are emitted in authored (catalog) order so a
                // capture is byte-stable regardless of dictionary ordering.
                state.outposts.Add(new OutpostInstanceState
                {
                    outpost_id = def.Id,
                    is_established = false,
                    condition_permille = 1000,
                    garrison_survivor_ids = new List<string>(),
                    days_since_supply = 0,
                    is_starving = false,
                    is_overrun = false,
                    ration_reserve = 0
                });
            }

            foreach (var inst in _instances.Values)
            {
                if (string.IsNullOrEmpty(inst.OutpostId)) continue;
                var row = state.outposts.Find(o => string.Equals(o.outpost_id, inst.OutpostId, StringComparison.OrdinalIgnoreCase));
                if (row == null) continue;
                row.is_established = inst.IsEstablished;
                row.condition_permille = inst.ConditionPermille;
                row.garrison_survivor_ids = inst.GarrisonSurvivorIds != null
                    ? new List<string>(inst.GarrisonSurvivorIds)
                    : new List<string>();
                row.days_since_supply = inst.DaysSinceSupply;
                row.is_starving = inst.IsStarving;
                row.is_overrun = inst.IsOverrun;
                row.ration_reserve = inst.RationReserve;
            }

            return state;
        }

        /// <summary>
        /// Plan 58 persistence — restore a captured state over the current
        /// instances. The state is authoritative for every authored outpost;
        /// rows naming an unknown outpost or over-full garrison are rejected
        /// deterministically rather than inventing a phantom outpost.
        /// </summary>
        public bool RestoreState(OutpostSettlementState? state)
        {
            if (state == null || state.outposts == null) return false;
            if (state.schema_version != 1) return false;

            // Missing authored rows default to unestablished so a partial save
            // can never resurrect an outpost the catalog no longer defines.
            foreach (var inst in _instances.Values)
            {
                inst.IsEstablished = false;
                inst.IsOverrun = false;
                inst.IsStarving = false;
                inst.ConditionPermille = 1000;
                inst.DaysSinceSupply = 0;
                inst.RationReserve = 0;
                inst.GarrisonSurvivorIds.Clear();
            }

            foreach (var row in state.outposts)
            {
                if (row == null || string.IsNullOrWhiteSpace(row.outpost_id)) continue;
                if (!_instances.TryGetValue(row.outpost_id, out var inst)) continue;  // phantom outpost
                if (!_definitions.TryGetValue(row.outpost_id, out var def)) continue;

                var garrison = row.garrison_survivor_ids ?? new List<string>();
                var cap = Math.Max(1, def.MaxGarrisonBunks);
                var bounded = garrison.Count > cap ? garrison.GetRange(0, cap) : garrison;

                inst.IsEstablished = row.is_established;
                inst.ConditionPermille = Math.Max(0, Math.Min(1000, row.condition_permille));
                inst.GarrisonSurvivorIds = new List<string>(bounded);
                inst.DaysSinceSupply = Math.Max(0, row.days_since_supply);
                inst.IsStarving = row.is_starving;
                inst.IsOverrun = row.is_overrun;
                inst.RationReserve = Math.Max(0, row.ration_reserve);
            }

            return true;
        }
    }
}
