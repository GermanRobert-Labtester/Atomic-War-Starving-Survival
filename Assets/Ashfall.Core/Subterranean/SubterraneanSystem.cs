using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Inventory;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Subterranean
{
    /// <summary>Persistent per-node state for one underground zone (Plan 156.3).</summary>
    public sealed class SubterraneanNodeState
    {
        public string nodeId = string.Empty;      // subnode_* (the catalog definition id)
        public int depthTier = 1;
        public bool discovered;
        public bool blocked;
        /// <summary>0..100; damage is permanent until shored.</summary>
        public float structuralIntegrity = 100f;
        public int shoringLevel;                  // 0..3, permanent risk reduction
        /// <summary>0..100 abstract air reserve for unventilated nodes.</summary>
        public float oxygenLevel = 100f;
        public bool ventilationInstalled;
        /// <summary>0..100 flood pressure; high water blocks and erodes.</summary>
        public float waterLevel;
        public int lastHazardDay = -1;
    }

    /// <summary>Serialized network state (checksummed via SubterraneanSaveCodec).</summary>
    public sealed class SubterraneanNetworkState
    {
        public int schemaVersion = 1;
        /// <summary>Campaign/world seed the network was generated from. Restore never regenerates.</summary>
        public int networkSeed;
        public bool generated;
        public List<SubterraneanNodeState> nodes = new List<SubterraneanNodeState>();
    }

    /// <summary>
    /// Flagship XI — Plan 156 subterranean exploration networks. Owns underground
    /// node state (discovery, structural integrity, oxygen, flood, shoring) and
    /// deterministic per-day hazards. It does NOT own survivor expedition state,
    /// surface topology, loot rolls, or health — the canonical authorities do;
    /// the host bridge feeds underground days in and applies consequences.
    /// Deterministic: ordinal iteration, hazard rolls seeded from
    /// (day, nodeId) — no RNG state persisted.
    /// </summary>
    public sealed class SubterraneanSystem
    {
        public const string SystemId = "subterranean_network";

        /// <summary>Trait id consulted for the underground morale modifier (Plan 156.15).</summary>
        public const string ClaustrophobiaTraitId = "trait_claustrophobe";
        public const float ClaustrophobiaMoralePenalty = -3f;

        public const int MaxShoringLevel = 3;
        public const float OxygenDrainPerOccupantPerDay = 2.5f;
        public const float OxygenRecoveryPerDayVentilated = 15f;
        public const float LowOxygenHealthCostPerDay = 4f;
        public const float LowOxygenThreshold = 25f;
        public const float ForcedRetreatOxygenThreshold = 10f;
        public const float FloodedThreshold = 80f;
        public const float CaveInIntegrityDamage = 30f;
        public const float BlockedIntegrityThreshold = 20f;
        public const float ShoringIntegrityRepair = 25f;
        public const float RiskScalePerDay = 0.1f; // ExcavationSystem cave-in precedent

        private readonly SubterraneanZoneCatalogContainer _catalog;
        private readonly InventoryContainer _inventory;
        private readonly Dictionary<string, SubterraneanZoneDef> _zoneById =
            new Dictionary<string, SubterraneanZoneDef>(StringComparer.Ordinal);
        private readonly SubterraneanNetworkState _state = new SubterraneanNetworkState();

        /// <summary>Canonical weather read (flood pressure source). Null = clear.</summary>
        public Func<WeatherKind>? CurrentWeather { get; set; }
        /// <summary>Canonical health mutation (low-oxygen consequence). Null = request logged only.</summary>
        public Action<string, float>? ApplyHealthDelta { get; set; }
        /// <summary>Optional sink for deterministic one-line diagnostics.</summary>
        public event Action<string, int>? OnCaveIn;            // nodeId, day
        public event Action<string, string, int>? OnForcedRetreat; // survivorId, nodeId, day
        public event Action<string, int>? OnNodeFlooded;       // nodeId, day
        public event Action<string>? OnNodeDiscovered;         // nodeId

        public SubterraneanSystem(SubterraneanZoneCatalogContainer catalog, InventoryContainer inventory)
        {
            _catalog = catalog ?? new SubterraneanZoneCatalogContainer();
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            foreach (var zone in _catalog.subterranean_zones)
                if (zone != null && !string.IsNullOrEmpty(zone.id))
                    _zoneById[zone.id] = zone;
        }

        public SubterraneanNetworkState State => _state;
        public IReadOnlyDictionary<string, SubterraneanZoneDef> Zones => _zoneById;

        // ---------------------------------------------------------- generation

        /// <summary>
        /// Deterministic one-time generation of node states from the catalog and
        /// campaign seed (Plan 156.4). No-ops when already generated — restore
        /// never regenerates the network.
        /// </summary>
        public void EnsureNetwork(int campaignSeed)
        {
            if (_state.generated) return;
            _state.networkSeed = campaignSeed;
            _state.nodes.Clear();

            foreach (var zone in _catalog.subterranean_zones.OrderBy(z => z.id, StringComparer.Ordinal))
            {
                if (zone == null || string.IsNullOrEmpty(zone.id)) continue;

                // Seeded per-node variance keeps generation deterministic but not
                // uniform; the catalog stays the topology authority.
                var rng = new SeededRng(NodeSeed(campaignSeed, zone.id));
                float integrityVariance = (float)rng.NextDouble() * 10f;
                float initialWater = zone.flood_susceptibility > 0.6f ? (float)rng.NextDouble() * 15f : 0f;

                _state.nodes.Add(new SubterraneanNodeState
                {
                    nodeId = zone.id,
                    depthTier = Math.Clamp(zone.depth_tier, 1, 3),
                    structuralIntegrity = Math.Clamp(100f - zone.base_structural_risk * 20f - integrityVariance, 10f, 100f),
                    oxygenLevel = zone.oxygen_class == "foul" ? 55f : zone.oxygen_class == "thin" ? 75f : 100f,
                    waterLevel = initialWater,
                    discovered = false
                });
            }
            _state.generated = true;
        }

        // ---------------------------------------------------------- discovery

        /// <summary>Surface-anchored discovery: entering the anchor location reveals its tier-1 nodes.</summary>
        public IReadOnlyList<string> DiscoverFromAnchor(string surfaceLocationId)
        {
            var revealed = new List<string>();
            foreach (var zone in _catalog.subterranean_zones.OrderBy(z => z.id, StringComparer.Ordinal))
            {
                if (zone == null || !string.Equals(zone.surface_anchor_id, surfaceLocationId, StringComparison.Ordinal))
                    continue;
                if (zone.depth_tier != 1) continue;
                var node = Find(zone.id);
                if (node == null || node.discovered) continue;
                node.discovered = true;
                revealed.Add(zone.id);
                OnNodeDiscovered?.Invoke(zone.id);
            }
            return revealed;
        }

        /// <summary>Descending discovery: finding a node reveals the zones connected one step deeper.</summary>
        public IReadOnlyList<string> DiscoverConnectedFrom(string nodeId)
        {
            var revealed = new List<string>();
            var zone = _zoneById.GetValueOrDefault(nodeId);
            if (zone == null) return revealed;

            foreach (var rule in zone.connection_rules.OrderBy(c => c.to, StringComparer.Ordinal))
            {
                if (rule == null || string.IsNullOrEmpty(rule.to)) continue;
                var neighbour = Find(rule.to);
                if (neighbour == null || neighbour.discovered) continue;
                neighbour.discovered = true;
                revealed.Add(rule.to);
                OnNodeDiscovered?.Invoke(rule.to);
            }
            return revealed;
        }

        public bool IsDiscovered(string nodeId) => Find(nodeId)?.discovered ?? false;

        // --------------------------------------------------------- daily tick

        /// <summary>
        /// One underground day (Plan 156.5/156.7/156.14). Environmental hazards
        /// (flood pressure, structural decay) run for every generated node —
        /// the underground keeps its own time whether or not anyone is down
        /// there — while oxygen drains only for actual occupants. Ordinal by
        /// node, then survivor; hazard rolls are seeded per (day, node).
        /// </summary>
        public void ApplyUndergroundDay(int day, IEnumerable<(string survivorId, string nodeId)> occupants)
        {
            if (!_state.generated) return;

            var byNode = new Dictionary<string, List<string>>(StringComparer.Ordinal);
            foreach (var (survivorId, nodeId) in occupants)
            {
                if (string.IsNullOrEmpty(nodeId) || !_zoneById.ContainsKey(nodeId)) continue;
                if (!byNode.TryGetValue(nodeId, out var list))
                {
                    list = new List<string>();
                    byNode[nodeId] = list;
                }
                list.Add(survivorId);
            }

            foreach (var node in _state.nodes.OrderBy(n => n.nodeId, StringComparer.Ordinal).ToList())
            {
                var zone = _zoneById.GetValueOrDefault(node.nodeId);
                if (zone == null) continue;
                byNode.TryGetValue(node.nodeId, out var occupantsHere);

                // Oxygen (156.7): occupants drain, ventilation recovers.
                float drain = (occupantsHere?.Count ?? 0) * OxygenDrainPerOccupantPerDay;
                float oxygen = node.oxygenLevel - drain;
                if (node.ventilationInstalled) oxygen += OxygenRecoveryPerDayVentilated;
                node.oxygenLevel = Math.Clamp(oxygen, 0f, 100f);

                if (occupantsHere != null)
                {
                    foreach (var survivorId in occupantsHere.Distinct().OrderBy(id => id, StringComparer.Ordinal))
                    {
                        if (node.oxygenLevel < ForcedRetreatOxygenThreshold)
                        {
                            OnForcedRetreat?.Invoke(survivorId, node.nodeId, day);
                        }
                        else if (node.oxygenLevel < LowOxygenThreshold)
                        {
                            ApplyHealthDelta?.Invoke(survivorId, -LowOxygenHealthCostPerDay);
                        }
                    }
                }

                // Flood pressure (156.14): canonical weather query, SumpFlooding scale.
                float weatherPressure = CurrentWeather?.Invoke() switch
                {
                    WeatherKind.FalloutStorm => 20f,
                    WeatherKind.BlackRain => 12f,
                    WeatherKind.Ashfall => 8f,
                    WeatherKind.Blizzard => 6f,
                    WeatherKind.Rain => 5f,
                    WeatherKind.Overcast => 2f,
                    _ => 0f
                };
                if (weatherPressure > 0f)
                {
                    node.waterLevel = Math.Clamp(node.waterLevel + weatherPressure * zone.flood_susceptibility, 0f, 100f);
                    if (node.waterLevel >= FloodedThreshold)
                    {
                        // Saturation erodes whether or not the node already gave
                        // way; the event fires only on the transition.
                        node.structuralIntegrity = Math.Max(0f, node.structuralIntegrity - 10f);
                        if (!node.blocked)
                        {
                            node.blocked = true;
                            OnNodeFlooded?.Invoke(node.nodeId, day);
                        }
                    }
                }
                else if (node.waterLevel > 0f)
                {
                    node.waterLevel = Math.Max(0f, node.waterLevel - 3f); // slow seep-out in dry weather
                }

                // Cave-in risk (156.5): abstract factors; shoring damps.
                float risk = zone.base_structural_risk
                             + (100f - node.structuralIntegrity) / 200f
                             + node.waterLevel / 250f;
                risk *= 1f - 0.25f * node.shoringLevel;
                if (risk > 0f)
                {
                    var rng = new SeededRng(NodeSeed(day, node.nodeId));
                    if (rng.NextDouble() < risk * RiskScalePerDay)
                    {
                        node.structuralIntegrity = Math.Max(0f, node.structuralIntegrity - CaveInIntegrityDamage);
                        if (node.structuralIntegrity <= BlockedIntegrityThreshold) node.blocked = true;
                        node.lastHazardDay = day;
                        OnCaveIn?.Invoke(node.nodeId, day);
                    }
                }
            }
        }

        // -------------------------------------------------------------- shoring

        /// <summary>
        /// Shoring (156.10): permanent structural-risk reduction via an atomic
        /// material billing. Returns null on success, otherwise the blocker code.
        /// </summary>
        public string? TryShoreNode(string nodeId)
        {
            var node = Find(nodeId);
            if (node == null) return "unknown_node";
            if (node.shoringLevel >= MaxShoringLevel) return "shoring_maxed";

            var bill = new InventoryBill();
            bill.AddCost("scrap_wood", 4);
            bill.AddCost("steel_rebar", 2);
            if (node.shoringLevel >= 1) bill.AddCost("scrap_metal", 2);

            using var tx = _inventory.BeginTransaction(bill);
            if (!tx.Validation.IsValid)
            {
                tx.Cancel();
                return "missing_materials";
            }
            node.shoringLevel++;
            node.structuralIntegrity = Math.Min(100f, node.structuralIntegrity + ShoringIntegrityRepair);
            tx.TryCommit();
            return null;
        }

        /// <summary>Ventilation (156.8): abstract pump/filter install, atomic billing.</summary>
        public string? TryInstallVentilation(string nodeId)
        {
            var node = Find(nodeId);
            if (node == null) return "unknown_node";
            if (node.ventilationInstalled) return "already_installed";

            var bill = new InventoryBill();
            bill.AddCost("scrap_metal", 3);
            bill.AddCost("battery", 1);

            using var tx = _inventory.BeginTransaction(bill);
            if (!tx.Validation.IsValid)
            {
                tx.Cancel();
                return "missing_materials";
            }
            node.ventilationInstalled = true;
            tx.TryCommit();
            return null;
        }

        /// <summary>Clears a blockage when the structure can bear re-entry.</summary>
        public string? TryClearBlockage(string nodeId)
        {
            var node = Find(nodeId);
            if (node == null) return "unknown_node";
            if (!node.blocked) return "not_blocked";
            if (node.structuralIntegrity <= BlockedIntegrityThreshold) return "structure_unsound";

            var bill = new InventoryBill();
            bill.AddCost("scrap_wood", 2);
            using var tx = _inventory.BeginTransaction(bill);
            if (!tx.Validation.IsValid)
            {
                tx.Cancel();
                return "missing_materials";
            }
            node.blocked = false;
            tx.TryCommit();
            return null;
        }

        // --------------------------------------------------------- morale hook

        /// <summary>
        /// Canonical morale modifier for an underground assignment (156.15):
        /// full weight for claustrophobia bearers, nothing for everyone else —
        /// distress then spreads through morale contagion naturally.
        /// </summary>
        public float UndergroundMoraleDelta(string nodeId, bool hasClaustrophobia)
        {
            if (!_zoneById.ContainsKey(nodeId)) return 0f;
            return hasClaustrophobia ? ClaustrophobiaMoralePenalty : 0f;
        }

        // ------------------------------------------------------------ helpers

        public SubterraneanNodeState? Find(string nodeId)
        {
            foreach (var node in _state.nodes)
                if (node != null && string.Equals(node.nodeId, nodeId, StringComparison.Ordinal))
                    return node;
            return null;
        }

        /// <summary>Stable per-(seed, id) roll seed — FNV-1a over the parts.</summary>
        public static int NodeSeed(int seed, string nodeId)
        {
            unchecked
            {
                uint hash = 2166136261u;
                foreach (char c in nodeId ?? string.Empty) { hash ^= c; hash *= 16777619u; }
                hash ^= 0x5F; hash *= 16777619u;
                for (int i = 0; i < 4; i++)
                {
                    hash ^= (byte)((seed >> (i * 8)) & 0xFF);
                    hash *= 16777619u;
                }
                return (int)hash;
            }
        }

        // -------------------------------------------------------- persistence

        public SubterraneanNetworkState CaptureState()
        {
            var copy = new SubterraneanNetworkState
            {
                networkSeed = _state.networkSeed,
                generated = _state.generated
            };
            foreach (var node in _state.nodes)
                copy.nodes.Add(new SubterraneanNodeState
                {
                    nodeId = node.nodeId,
                    depthTier = node.depthTier,
                    discovered = node.discovered,
                    blocked = node.blocked,
                    structuralIntegrity = node.structuralIntegrity,
                    shoringLevel = node.shoringLevel,
                    oxygenLevel = node.oxygenLevel,
                    ventilationInstalled = node.ventilationInstalled,
                    waterLevel = node.waterLevel,
                    lastHazardDay = node.lastHazardDay
                });
            return copy;
        }

        /// <summary>NON-OPERATIVE restore: reconstructs the persisted network; never regenerates.</summary>
        public void RestoreState(SubterraneanNetworkState state)
        {
            _state.nodes.Clear();
            _state.networkSeed = 0;
            _state.generated = false;
            if (state == null) return;

            _state.networkSeed = state.networkSeed;
            _state.generated = state.generated;
            foreach (var node in state.nodes ?? new List<SubterraneanNodeState>())
                if (node != null && !string.IsNullOrEmpty(node.nodeId))
                    _state.nodes.Add(new SubterraneanNodeState
                    {
                        nodeId = node.nodeId,
                        depthTier = node.depthTier,
                        discovered = node.discovered,
                        blocked = node.blocked,
                        structuralIntegrity = Math.Clamp(node.structuralIntegrity, 0f, 100f),
                        shoringLevel = Math.Clamp(node.shoringLevel, 0, MaxShoringLevel),
                        oxygenLevel = Math.Clamp(node.oxygenLevel, 0f, 100f),
                        ventilationInstalled = node.ventilationInstalled,
                        waterLevel = Math.Clamp(node.waterLevel, 0f, 100f),
                        lastHazardDay = node.lastHazardDay
                    });
        }
    }
}
