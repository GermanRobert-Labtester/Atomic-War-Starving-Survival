// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Shelter
{
    [Serializable]
    public sealed class SubgridNodeRuntimeState
    {
        public string node_id { get; set; } = string.Empty;
        public string target_room_id { get; set; } = string.Empty;
        public bool is_breaker_closed { get; set; } = true;
        public bool is_fuse_blown { get; set; }
        public float current_load_watts { get; set; }
        public float temperature_celsius { get; set; } = 25.0f;
        public float transformer_oil_condition { get; set; } = 100.0f; // 0 - 100%
        public int days_since_last_service { get; set; }
    }

    [Serializable]
    public sealed class PowerDistributionSubgridSave
    {
        public string systemId { get; set; } = "power_subgrids";
        public int schema_version { get; set; } = 1;
        public int last_tick_day { get; set; } = 1;
        public float capacitor_bank_charge_watts { get; set; } = 2000.0f;
        public float max_capacitor_buffer_watts { get; set; } = 2000.0f;
        public List<SubgridNodeRuntimeState> nodes { get; set; } = new List<SubgridNodeRuntimeState>();
    }

    public sealed class PowerDistributionSubgridSystem
    {
        public const string SystemId = "power_subgrids";
        public const float AmbientTemperatureCelsius = 20.0f;
        public const float MaxCapacitorBurstWatts = 2000.0f;
        public const float OverloadThresholdFraction = 0.90f; // 90% load raises thermal stress

        private readonly List<PowerSubgridNodeDefinition> _nodeDefs = new List<PowerSubgridNodeDefinition>();
        private readonly Dictionary<string, PowerSubgridNodeDefinition> _defsById = new Dictionary<string, PowerSubgridNodeDefinition>(StringComparer.Ordinal);
        private readonly Inventory.Inventory _inventory;
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        private PowerDistributionSubgridSave _state = new PowerDistributionSubgridSave();

        public event Action<string, float>? OnNodeThermalWarning; // nodeId, temp
        public event Action<string>? OnNodeFuseBlown;             // nodeId
        public event Action<string, string>? OnSubgridArcFlash;   // nodeId, roomId
        public event Action<string>? OnTransformerMaintained;     // nodeId

        public IReadOnlyList<PowerSubgridNodeDefinition> NodeDefinitions => _nodeDefs;
        public IReadOnlyList<SubgridNodeRuntimeState> Nodes => _state.nodes;
        public float CapacitorBankChargeWatts => _state.capacitor_bank_charge_watts;

        public PowerDistributionSubgridSystem(
            IEnumerable<PowerSubgridNodeDefinition> nodeDefs,
            Inventory.Inventory inventory,
            ISeededRng rng,
            ILog? log = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;

            if (nodeDefs != null)
            {
                foreach (var def in nodeDefs)
                {
                    if (def == null || string.IsNullOrEmpty(def.node_id)) continue;
                    _nodeDefs.Add(def);
                    _defsById[def.node_id] = def;

                    _state.nodes.Add(new SubgridNodeRuntimeState
                    {
                        node_id = def.node_id,
                        target_room_id = def.target_room_id,
                        is_breaker_closed = true,
                        is_fuse_blown = false,
                        current_load_watts = 0f,
                        temperature_celsius = AmbientTemperatureCelsius,
                        transformer_oil_condition = def.transformer_oil_condition
                    });
                }
            }
        }

        public SubgridNodeRuntimeState? FindNode(string nodeId)
        {
            for (int i = 0; i < _state.nodes.Count; i++)
            {
                if (_state.nodes[i].node_id == nodeId) return _state.nodes[i];
            }
            return null;
        }

        public SubgridNodeRuntimeState? FindNodeForRoom(string roomId)
        {
            for (int i = 0; i < _state.nodes.Count; i++)
            {
                if (_state.nodes[i].target_room_id == roomId) return _state.nodes[i];
            }
            return null;
        }

        public bool IsNodeDeliveringPower(string nodeId)
        {
            var node = FindNode(nodeId);
            return node != null && node.is_breaker_closed && !node.is_fuse_blown;
        }

        public bool IsRoomPowered(string roomId)
        {
            var node = FindNodeForRoom(roomId);
            return node != null && node.is_breaker_closed && !node.is_fuse_blown;
        }

        public void SetBreaker(string nodeId, bool closed)
        {
            var node = FindNode(nodeId);
            if (node != null)
            {
                node.is_breaker_closed = closed;
                _log.Info($"[Subgrid] Node {nodeId} breaker set to {(closed ? "CLOSED" : "OPEN")}.");
            }
        }

        public void ApplyRoomLoad(string roomId, float loadWatts)
        {
            loadWatts = Math.Max(0f, loadWatts);
            var node = FindNodeForRoom(roomId);
            if (node == null || !node.is_breaker_closed || node.is_fuse_blown) return;

            if (!_defsById.TryGetValue(node.node_id, out var def)) return;

            node.current_load_watts = loadWatts;

            // Check momentary surge absorption via capacitor bank
            if (loadWatts > def.max_capacity_watts)
            {
                float surge = loadWatts - def.max_capacity_watts;
                if (surge <= def.surge_limit_watts - def.max_capacity_watts && _state.capacitor_bank_charge_watts >= surge)
                {
                    // Buffer momentary surge
                    _state.capacitor_bank_charge_watts -= surge * 0.1f;
                    _log.Info($"[Subgrid] Node {node.node_id} surge buffered by industrial capacitor bank.");
                }
                else if (loadWatts > def.surge_limit_watts)
                {
                    // Catastrophic surge blows fuse!
                    node.is_fuse_blown = true;
                    OnNodeFuseBlown?.Invoke(node.node_id);
                    _log.Warn($"[Subgrid] Node {node.node_id} exceeded surge limit ({loadWatts}W > {def.surge_limit_watts}W) — fuse blown!");

                    // Arc flash check
                    TriggerArcFlashRisk(node, def);
                    return;
                }
            }

            // Thermal heating if load > 90%
            float loadFraction = loadWatts / Math.Max(100f, def.max_capacity_watts);
            if (loadFraction > OverloadThresholdFraction)
            {
                float deltaTemp = (loadFraction - OverloadThresholdFraction) * 40f * (1.1f - def.cooling_efficiency);
                node.temperature_celsius = Math.Min(150f, node.temperature_celsius + deltaTemp);

                // Oil degradation under heat
                if (node.temperature_celsius > 60f)
                {
                    float oilWear = (node.temperature_celsius - 60f) * 0.05f;
                    node.transformer_oil_condition = Math.Max(0f, node.transformer_oil_condition - oilWear);
                }

                if (node.temperature_celsius > 85f)
                {
                    OnNodeThermalWarning?.Invoke(node.node_id, node.temperature_celsius);
                }

                if (node.transformer_oil_condition < 20f || node.temperature_celsius > 110f)
                {
                    TriggerArcFlashRisk(node, def);
                }
            }
        }

        private void TriggerArcFlashRisk(SubgridNodeRuntimeState node, PowerSubgridNodeDefinition def)
        {
            double roll = _rng.NextDouble();
            float arcRisk = (100f - node.transformer_oil_condition) * 0.005f;
            if (node.temperature_celsius > 100f) arcRisk += 0.25f;

            if (roll < arcRisk)
            {
                node.is_fuse_blown = true;
                OnSubgridArcFlash?.Invoke(node.node_id, node.target_room_id);
                _log.Warn($"[Subgrid] ARC-FLASH BLOWOUT at node {node.node_id} in {node.target_room_id}!");
            }
        }

        public ActionResult ReplaceFuse(string nodeId, bool hasRepairSkill = true)
        {
            var node = FindNode(nodeId);
            if (node == null)
                return ActionResult.Failed("node_not_found", "subgrid.node_not_found");

            if (!node.is_fuse_blown)
                return ActionResult.Blocked("fuse_intact", "subgrid.fuse_intact");

            if (!hasRepairSkill)
                return ActionResult.Blocked("lacks_repair_skill", "subgrid.lacks_repair_skill");

            if (!_inventory.HasSufficient("copper_fuse", 1))
                return ActionResult.Blocked("missing_fuse", "subgrid.missing_copper_fuse");

            if (!_inventory.TryConsume("copper_fuse", 1))
                return ActionResult.Failed("consume_fuse_failed", "subgrid.consume_fuse_failed");

            node.is_fuse_blown = false;
            node.is_breaker_closed = true;
            _log.Info($"[Subgrid] Replaced fuse on {nodeId} with new copper fuse. Subgrid restored.");
            return ActionResult.Success("subgrid.fuse_replaced");
        }

        public ActionResult PerformTransformerMaintenance(string nodeId)
        {
            var node = FindNode(nodeId);
            if (node == null)
                return ActionResult.Failed("node_not_found", "subgrid.node_not_found");

            if (!_inventory.HasSufficient("machine_oil", 1))
                return ActionResult.Blocked("missing_machine_oil", "subgrid.missing_machine_oil");

            if (!_inventory.HasSufficient("electrical_wire", 2))
                return ActionResult.Blocked("missing_electrical_wire", "subgrid.missing_electrical_wire");

            if (!_inventory.TryConsume("machine_oil", 1) || !_inventory.TryConsume("electrical_wire", 2))
                return ActionResult.Failed("consume_reagents_failed", "subgrid.consume_reagents_failed");

            node.transformer_oil_condition = 100.0f;
            node.temperature_celsius = AmbientTemperatureCelsius;
            node.days_since_last_service = 0;
            OnTransformerMaintained?.Invoke(nodeId);
            _log.Info($"[Subgrid] Transformer maintenance completed on {nodeId}. Oil flushed and condition 100%.");
            return ActionResult.Success("subgrid.transformer_maintained");
        }

        public void TickDay(int day)
        {
            _state.last_tick_day = day;

            // Re-charge capacitor bank toward 2000W buffer
            _state.capacitor_bank_charge_watts = Math.Min(
                MaxCapacitorBurstWatts,
                _state.capacitor_bank_charge_watts + 500f);

            for (int i = 0; i < _state.nodes.Count; i++)
            {
                var n = _state.nodes[i];
                n.days_since_last_service++;

                // Thermal dissipation towards ambient
                if (n.temperature_celsius > AmbientTemperatureCelsius)
                {
                    n.temperature_celsius = Math.Max(
                        AmbientTemperatureCelsius,
                        n.temperature_celsius - 15.0f);
                }

                // Damp sector moisture corrosion (pumps and filtration)
                if (n.target_room_id == "room_water_pump" || n.target_room_id == "room_filtration")
                {
                    n.transformer_oil_condition = Math.Max(0f, n.transformer_oil_condition - 0.8f);
                }
                else
                {
                    n.transformer_oil_condition = Math.Max(0f, n.transformer_oil_condition - 0.3f);
                }
            }
        }

        public PowerDistributionSubgridSave CaptureState()
        {
            var save = new PowerDistributionSubgridSave
            {
                systemId = SystemId,
                schema_version = 1,
                last_tick_day = _state.last_tick_day,
                capacitor_bank_charge_watts = _state.capacitor_bank_charge_watts,
                max_capacitor_buffer_watts = _state.max_capacitor_buffer_watts
            };

            foreach (var n in _state.nodes)
            {
                save.nodes.Add(new SubgridNodeRuntimeState
                {
                    node_id = n.node_id,
                    target_room_id = n.target_room_id,
                    is_breaker_closed = n.is_breaker_closed,
                    is_fuse_blown = n.is_fuse_blown,
                    current_load_watts = n.current_load_watts,
                    temperature_celsius = n.temperature_celsius,
                    transformer_oil_condition = n.transformer_oil_condition,
                    days_since_last_service = n.days_since_last_service
                });
            }

            return save;
        }

        public void RestoreState(PowerDistributionSubgridSave? save)
        {
            if (save == null) return;
            _state.last_tick_day = save.last_tick_day;
            _state.capacitor_bank_charge_watts = save.capacitor_bank_charge_watts;
            _state.max_capacitor_buffer_watts = save.max_capacitor_buffer_watts;

            _state.nodes.Clear();
            if (save.nodes != null)
            {
                foreach (var n in save.nodes)
                {
                    _state.nodes.Add(new SubgridNodeRuntimeState
                    {
                        node_id = n.node_id,
                        target_room_id = n.target_room_id,
                        is_breaker_closed = n.is_breaker_closed,
                        is_fuse_blown = n.is_fuse_blown,
                        current_load_watts = n.current_load_watts,
                        temperature_celsius = n.temperature_celsius,
                        transformer_oil_condition = n.transformer_oil_condition,
                        days_since_last_service = n.days_since_last_service
                    });
                }
            }
        }
    }
}
