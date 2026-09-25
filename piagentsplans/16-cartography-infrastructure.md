# Plan 16 — Cartography and Infrastructure: Current-Seam Integration Architecture

> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-7`
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round7-2026-09-25`
> **Document class:** evidence-backed implementation plan; planning-only artifact
> **Domain:** map graph, waystations, caravans, and treaties
> **Read-only design authority:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
> **Authority SHA-256:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
> **Target depth:** 150k–170k intermediate quality checkpoint, then 250,000+ characters as a quality target, not a ceiling; no padding or unsupported completion claims
> **Scope:** Core/data/host/UI/save/determinism architecture and verification planning only

## Executive summary

Treat the wasteland as one graph with explicit ownership at each layer: canonical map topology, route projection, station state, caravan movement, treaty state, and host presentation.

This document supersedes stale generated or historical claims in the selected plan path. It distinguishes current evidence, required delta, safe extension seam, ownership, persistence, determinism, presentation, failure behavior, focused verification, rollback, and the remaining implementation handoff. It is not an implementation report.

## Selection and premise record

- Original Git `HEAD` baseline: `5748` characters.
- Current worktree copy: `409601` characters before this rebuild.
- Selection rule: next-lowest original `HEAD` character count after excluding every path completed in Rounds 1–6 and any active claim.
- Current worktree generated text was not used as proof of implementation because it may contain stale counts, repetitive expansion, or unsupported pass language.
- User-supplied authority path contained a spacing variation; the canonical repository path used here is `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`.

## Current source and data dossier

### Current evidence: `Assets/Ashfall.Core/World/WastelandMapSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `4edb79c28b4d8baec664fdac3bdcbc2084b4bbd5524f48dec1f6e2e1029bdce3`
- Snapshot size: 50731 characters; 1274 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0021:     /// </summary>
0022:     public sealed class WastelandMapSystem
0023:     {
0024:         private readonly WastelandMapState _state;
...
0038:
0039:         public WastelandMapSystem(WastelandMapState state,
0040:             IEnumerable<MapNode> nodes, IEnumerable<MapRoute> routes,
0041:             IEnumerable<TrapSiteMapLocation>? trapSiteLocations = null,
...
0055:             if (_nodes.Count == 0)
0056:                 throw new InvalidOperationException("WastelandMapSystem: at least one node required.");
0057:
0058:             _routes = new List<MapRoute>();
```

### Current evidence: `Assets/Ashfall.Core/World/WastelandMapCatalogLoader.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `40b46e728b6d84f602291d2f454f91c7003048bedc37106bf198047ae40d215b`
- Snapshot size: 16924 characters; 405 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0145:     /// </summary>
0146:     public static class WastelandMapCatalogLoader
0147:     {
0148:         public const string DefaultFileName = "wasteland_map_v1.json";
...
0321:
0322:         public static WastelandMapSystem CreateSystem(
0323:             string dataDir,
0324:             WastelandMapState? state = null,
...
0343:             var tunnelCatalog = LoadTunnelCatalog(dataDir, fileIO);
0344:             return new WastelandMapSystem(state ?? new WastelandMapState(), nodes, routes, trapSites, tunnelCatalog);
0345:         }
0346:
```

### Current evidence: `Assets/Ashfall.Core/World/DamagedMapSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `b5a78fef932b80b13d08c2d802f18a38157d83aa82ac76294cba48c954709e26`
- Snapshot size: 9314 characters; 204 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0019:     /// reveal at most once. Reveal goes through the authoritative world map —
0020:     /// <see cref="WastelandMapSystem.Discover"/> plus
0021:     /// <see cref="WastelandMapSystem.Unlock"/> of the installation node —
0022:     /// never through a parallel location registry.
...
0026:     /// </summary>
0027:     public sealed class DamagedMapSystem
0028:     {
0029:         private readonly List<DamagedMapZone> _zones;
...
0032:         private readonly Dictionary<string, DamagedMapZone> _zonesByDestination;
0033:         private readonly WastelandMapSystem? _map;
0034:
0035:         /// <summary>Fired once when a zone transitions incomplete → complete (edge-triggered).</summary>
...
0045:         /// tools; reveal is then limited to raising events without map mutation.</summary>
0046:         public DamagedMapSystem(IReadOnlyList<DamagedMapZone> zones, WastelandMapSystem? map)
0047:         {
0048:             _map = map;
```

### Current evidence: `Assets/Ashfall.Core/World/DamagedMapCatalog.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `d06e9a1a8b3606f3f9eaa51f2f12be9f37773ad334727343e4a040f84daca2cf`
- Snapshot size: 15102 characters; 316 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0120:     /// The catalog is static content; campaign progress lives in
0121:     /// WastelandMapState via <see cref="DamagedMapSystem"/>.
0122:     /// </summary>
0123:     public static class DamagedMapCatalogLoader
...
0299:         /// <summary>
0300:         /// Loads the catalog and builds a <see cref="DamagedMapSystem"/> bound
0301:         /// to the live wasteland map (the authoritative reveal target).
0302:         /// Returns null when the catalog is missing or invalid.
...
0305:             string dataDir,
0306:             WastelandMapSystem? wastelandMap,
0307:             IFileIO? fileIO = null,
0308:             IJsonSerializer? json = null)
...
0312:                 return null;
0313:             return new DamagedMapSystem(zones, wastelandMap);
0314:         }
0315:     }
```

### Current evidence: `Assets/Ashfall.Core/Waystation/WaystationNetworkSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `51cf876a0bed96e4cc3c89c657bc4ab383ebdc339abc92b4d8a1fed8bc8b1e6e`
- Snapshot size: 8153 characters; 200 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0024:     {
0025:         public string systemId = WaystationNetworkSystem.SystemId;
0026:         public List<WaystationInstanceState> stations = new List<WaystationInstanceState>();
0027:         public int totalMaintenanceActions = 0;
...
0033:     /// </summary>
0034:     public sealed class WaystationNetworkSystem
0035:     {
0036:         public const string SystemId = "waystation_network_system";
...
0058:
0059:         public WaystationNetworkSystem(List<WaystationDef>? catalog = null, WaystationNetworkState? state = null)
0060:         {
0061:             _catalog = catalog ?? WaystationCatalogLoader.GetDefaultWaystations();
```

### Current evidence: `Assets/Ashfall.Core/Waystation/WaystationCatalogLoader.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `f7899cb6a574c095623779174d9e90621eca9458426971b68987bcadd5f7e140`
- Snapshot size: 7200 characters; 164 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using System.IO;
0005:
0006: namespace Ashfall.Core.Waystation
0007: {
0008:     [Serializable]
0009:     public sealed class WaystationDef
0010:     {
0011:         public string id { get; set; } = string.Empty;
0012:         public string name { get; set; } = string.Empty;
0013:         public string node_id { get; set; } = string.Empty;
0014:         public string region { get; set; } = string.Empty;
0015:         public string keeper_name { get; set; } = string.Empty;
0016:         public string specialty { get; set; } = string.Empty;
0017:         public float condition { get; set; } = 100f;
0018:         public float filter_health { get; set; } = 100f;
0019:         public int defense_rating { get; set; } = 3;
0020:         public List<string> services { get; set; } = new List<string>();
```

### Current evidence: `Assets/Ashfall.Core/TravelingCaravanSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `9c7a92b23c61fab7d3f7b434eb3a9d78773ce6c37d2e5de21cd3d1723ccbc349`
- Snapshot size: 22689 characters; 518 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0051:     /// </summary>
0052:     public class TravelingCaravanSystem
0053:     {
0054:         public const string SystemId = "traveling_caravan_system";
...
0072:         /// <summary>Plan 32B — optional wasteland graph. Unbound keeps authored hops.</summary>
0073:         public WastelandMapSystem? Map { get; set; }
0074:
0075:         public event Action<CaravanEntry, string>? OnCaravanArrivedAtNode;
...
0085:
0086:         public TravelingCaravanSystem(TravelingCaravanState? state = null)
0087:         {
0088:             _state = new TravelingCaravanState();
...
0143:         /// </summary>
0144:         private static void AddRegionalSpecialtyStock(TravelingCaravanSystem system, CaravanEntry caravan, string region)
0145:         {
0146:             if (system.Catalog != null)
```

### Current evidence: `Assets/Ashfall.Core/Economy/CaravanTradeNetworkSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `0674cafba15eb65c0bc6d9e87b5573a0c37ed5363e41d5dabe9c8e752ddac63e`
- Snapshot size: 28307 characters; 669 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0106:         /// <summary>Plan 32B — optional map graph for travel_days overlay.</summary>
0107:         public WastelandMapSystem? Map { get; set; }
0108:
0109:         public IReadOnlyList<CaravanRouteDefinition> Routes => _routes;
...
0528:         /// a trade pact with that faction is ratified. Derived on every read by the
0529:         /// provider (typically RegionalTreatySystem.GetTradeDiscount), so nothing is
0530:         /// granted-and-persisted here and save/restore can never double-apply it.</summary>
0531:         private Func<string, float>? _treatyPriceReliefProvider;
```

### Current evidence: `Assets/Ashfall.Core/RegionalTreatySystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `244cab1cc764fcbe3ebc70b3ec693fc7105e4814cb3110c790773d94029d52fd`
- Snapshot size: 17322 characters; 388 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0010:     {
0011:         public string systemId = RegionalTreatySystem.SystemId;
0012:         public List<TreatyInstance> treaties = new List<TreatyInstance>();
0013:     }
...
0057:
0058:     public sealed class RegionalTreatySystem
0059:     {
0060:         public const string SystemId = "regional_treaty";
...
0074:
0075:         public RegionalTreatySystem(ILog? log = null)
0076:         {
0077:             _log = log ?? NullLog.Instance;
```

### Current evidence: `Assets/Ashfall.Core/RegionalTreatyCatalogLoader.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e1bc7acaa859fc7ae79bebc35c91dc1a095383ed7949b2b139fad8fad6fbdb69`
- Snapshot size: 3883 characters; 95 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0008:     /// <summary>
0009:     /// Mechanical treaty catalog for <see cref="RegionalTreatySystem"/>.
0010:     /// Does not read narrative protocols or foundry accords.
0011:     /// </summary>
```

### Current evidence: `src/Host/WastelandMapSaveStore.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `04f3f787b339a5796641353115df24a3d852c71e7950bf948f744528946c832b`
- Snapshot size: 2733 characters; 59 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: // ============================================================================
0003: // Save Store : WastelandMapSaveStore
0004: // Core State : Ashfall.Core.World.WastelandMapState
0005: // Host Caller: Main.Expeditions / WorldHostSession
0006: // Purpose    : Wasteland travel map exploration, discovered POIs, and route fog-of-war
0007: // ============================================================================
0008: using System;
0009: using Ashfall.Core.Save;
0010: using Ashfall.Core.World;
0011:
0012: namespace AtomicWar.GodotApp
0013: {
0014:     /// <summary>
0015:     /// Wasteland Map save persistence — thin façade over the Core
0016:     /// SaveStore&lt;T&gt; service (via SaveStoreHub). Checksummed envelope and
0017:     /// atomic write live in the service; this section keeps its historical
0018:     /// strictness of NOT adopting pre-envelope bare-state files. The envelope
0019:     /// DTO stays because the deterministic smoke test round-trips it directly.
0020:     /// </summary>
```

### Current evidence: `src/Host/WaystationHostSession.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `30686b36f2b91f466c2b07f13c16736feeecc729c3cf7850a0cca6165887fac0`
- Snapshot size: 3483 characters; 106 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0025:         /// </summary>
0026:         public WaystationNetworkSystem? Network { get; private set; }
0027:         public WaystationHostSession(WaystationSystem system)
0028:         {
...
0090:         public void AttachNetwork(
0091:             WaystationNetworkSystem network,
0092:             Ashfall.Core.Economy.GoodsCatalog catalog,
0093:             Func<bool> isSuppliesShort)
```

### Current evidence: `src/Host/TravelingCaravanHostSession.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `c7d2f84a1650e46942b0fe301a65cd18fe0c3ded6892b3b0e541cb7e036a79e7`
- Snapshot size: 4708 characters; 105 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0017:     : HostSessionBase{
0018:         public TravelingCaravanSystem Engine { get; }
0019:
0020:         public string LastEvent { get; private set; } = string.Empty;
```

### Current evidence: `src/Host/RegionalTreatyHostSession.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `6cc962da22436cda5ee619ad1fd23a728e513be1bfc770891999950875e70686`
- Snapshot size: 1900 characters; 62 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0008:     /// <summary>
0009:     /// Host session for RegionalTreatySystem.
0010:     /// Manages diplomatic accords, scrap ratification costs, compliance checks, and violation penalties.
0011:     /// </summary>
...
0015:         public string LastEvent { get; private set; } = string.Empty;
0016:         public RegionalTreatyHostSession(RegionalTreatySystem system)
0017:         {
0018:             System = system ?? new RegionalTreatySystem(new GodotLog());
```

### Current evidence: `src/Main.Expeditions.cs`
- Role: current source/owner candidate
- Worktree status: `M src/Main.Expeditions.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `0e87d7d7f3605ba923f739c7bc36ab51edecc431781fe0014122a453422b300b`
- Snapshot size: 38629 characters; 831 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using Godot;
0003: using System;
0004: using System.Globalization;
0005: using System.IO;
0006: using System.Linq;
0007: using System.Collections.Generic;
0008: using AtomicWar.Journal;
0009: using Ashfall.Core;
0010: using Ashfall.Core.Combat;
0011: using Ashfall.Core.Campaign;
0012: using Ashfall.Core.Economy;
0013: using Ashfall.Core.Disease;
0014: using Ashfall.Core.Expeditions;
0015: using Ashfall.Core.Foundry;
0016: using Ashfall.Core.Inventory;
0017: using Ashfall.Core.Journal;
0018: using Ashfall.Core.Muster;
0019: using Ashfall.Core.Random;
0020: using Ashfall.Core.YearOfAsh;
```

### Current evidence: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `5384c1c4093451593327ccc1fce9f8708b21923b8c94104a17e94d72808adf2b`
- Snapshot size: 20796 characters; 809 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 2,
0003:   "trapSites": [
0004:     {
0005:       "siteId": "snare_perimeter_north",
0006:       "anchorNodeId": "loc_holdfast",
0007:       "offsetX": -70.0,
0008:       "offsetY": -50.0
0009:     }
0010:   ],
0011:   "nodes": [
0012:     {
0013:       "id": "loc_holdfast",
0014:       "displayName": "Holdfast",
0015:       "danger": "none",
0016:       "faction": "player",
0017:       "lootTable": null,
0018:       "positionX": 500,
0019:       "positionY": 300,
0020:       "discoverable": false,
```

### Current evidence: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `209af93c2b21f195faf3efb99e8f1645100be56b0b6c02c2045e715afc2cc1e0`
- Snapshot size: 15554 characters; 348 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "zones": [
0004:     {
0005:       "zone_id": "industrial_district",
0006:       "zone_name": "Industrial District",
0007:       "total_fragments": 3,
0008:       "hidden_installation_id": "underground_fuel_depot",
0009:       "hidden_installation_name": "Underground Fuel Depot",
0010:       "installation_description": "A pre-war emergency fuel reserve, sealed beneath a collapsed factory. The maps show a maintenance access through the old sewer junction.",
0011:       "revealed_items": [
0012:         "diesel_fuel",
0013:         "mechanical_parts"
0014:       ],
0015:       "fragments": [
0016:         {
0017:           "fragment_id": "damaged_map_industrial_1",
0018:           "label": "Northern Sector",
0019:           "description": "Shows the factory district north of the river. Burn damage obscures the eastern edge."
0020:         },
```

### Current evidence: `Assets/StreamingAssets/Data/caravans.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `0cb331e654f547f6f642e6780f3594d933bffd5437724e456f73d37ed42a1fac`
- Snapshot size: 4154 characters; 153 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "caravans": [
0004:     {
0005:       "caravan_id": "caravan_flotilla_salt_run",
0006:       "name": "Salt & Saline Flotilla Convoy",
0007:       "faction_id": "faction_the_fleet",
0008:       "origin_region": "deep_coast",
0009:       "route_node_ids": [
0010:         "loc_black_flotilla_outpost",
0011:         "loc_cut_radiation_zone_alpha",
0012:         "loc_cut_merchant_caravanserai",
0013:         "loc_cut_abandoned_depot",
0014:         "loc_holdfast"
0015:       ],
0016:       "stay_duration_days": 2,
0017:       "guard_count": 5,
0018:       "specialty_goods": [
0019:         {
0020:           "item_id": "clean_water",
```

### Current evidence: `Assets/StreamingAssets/Data/caravan_trade_routes.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `374993eb7b5b811ca87f437c1eda6e417f08254a77b0ee8a90202336327a8f7a`
- Snapshot size: 6711 characters; 235 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "routes": [
0004:     {
0005:       "route_id": "route_compact_supply_line",
0006:       "display_name": "Compact Northern Supply Line",
0007:       "faction_id": "faction_the_compact",
0008:       "origin_region_id": "settlement",
0009:       "destination_region_id": "iron_basin",
0010:       "travel_days": 6,
0011:       "base_risk_permille": 120,
0012:       "season_start_day": 1,
0013:       "season_end_day": 360,
0014:       "arrival_interval_days": 18,
0015:       "base_guard_strength": 6,
0016:       "weather_risk_multiplier": 1.2,
0017:       "bandit_risk_multiplier": 1.0,
0018:       "import_demands": [
0019:         "anesthetic_ether",
0020:         "sterile_gauze"
```

### Current evidence: `Assets/StreamingAssets/Data/waystations.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e213387763d78088d937e82af1de682d38def9e591ff2157a12c0ec95ed7b602`
- Snapshot size: 10821 characters; 320 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "waystations": [
0004:     {
0005:       "id": "waystation_alpha_cut",
0006:       "name": "Waystation A — The Cut",
0007:       "node_id": "loc_cut_abandoned_depot",
0008:       "region": "industrial_belt",
0009:       "keeper_name": "Warden Kessel",
0010:       "specialty": "Industrial Tools & Fasteners",
0011:       "condition": 85.0,
0012:       "filter_health": 90.0,
0013:       "defense_rating": 4,
0014:       "services": [
0015:         "trade",
0016:         "staging",
0017:         "rest",
0018:         "filter_recharge"
0019:       ],
0020:       "stock_item_ids": [
```

### Current evidence: `Assets/StreamingAssets/Data/diplomatic_treaties.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `3a13ed89328f54911f275e2a4ef6949046d66473bd5b0ab73651bb56aaa23a00`
- Snapshot size: 6842 characters; 152 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "treaties": [
0004:     {
0005:       "treaty_id": "treaty_non_aggression_compact",
0006:       "display_name": "Compact of Non-Aggression",
0007:       "description": "Signatories forswear raids and reprisals against one another for the treaty term. Old grudges are shelved, not settled.",
0008:       "eligible_faction_tags": ["militia", "civic", "holdout", "caravan"],
0009:       "minimum_signatories": 2,
0010:       "required_concessions": [
0011:         {"concession_kind": "goods", "item_id": "clean_water", "amount": 4}
0012:       ],
0013:       "duration_days": 30,
0014:       "stability_rating": 55,
0015:       "violation_tolerance": 1,
0016:       "guarantee_allowed": true,
0017:       "dmz_zone_ids": [],
0018:       "agenda_clauses": ["cease_raid_preparation", "mutual_reprisal_waiver", "quarterly_messenger_exchange"],
0019:       "violation_penalty_standing": -12,
0020:       "tags": ["core", "foundational"]
```

### Current evidence: `Assets/StreamingAssets/Data/treaty_templates.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `ec93396ed45ea9bd08a5bb96ffba60ac2d0a92b502b61372d685b81c3f754cf6`
- Snapshot size: 4331 characters; 113 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "templates": [
0004:     {
0005:       "treaty_type_id": "non_aggression",
0006:       "display_name": "Non-Aggression Pact",
0007:       "category": "peace",
0008:       "base_duration_days": 60,
0009:       "reputation_requirement": 20,
0010:       "relation_requirement": "neutral",
0011:       "terms": [
0012:         {
0013:           "term_id": "term_no_hostilities",
0014:           "term_type": "non_hostility",
0015:           "description": "Neither faction shall initiate raids, ambushes, or combat actions against each other's personnel or territory.",
0016:           "value": 1.0,
0017:           "condition": "mutual"
0018:         }
0019:       ],
0020:       "description": "Formal commitment between shelter and faction to cease all hostile incursions and honor borders for the agreed duration."
```

### Current evidence: `Assets/StreamingAssets/Data/faction_territory.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `ef0940bbe98f3082b75cbbef77df670e7e2c7358b89ddeefc20b8fd119b90915`
- Snapshot size: 19950 characters; 540 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "collection_id": "faction_territory_catalog",
0004:   "territories": [
0005:     {
0006:       "id": "territory_the_office",
0007:       "faction": "faction_the_office",
0008:       "display_name": "Industrial Rail Corridor & Weighbridge Network",
0009:       "classification": "territorial",
0010:       "territory_scale": "major",
0011:       "primary_resource_interest": "rail_freight_and_hardware",
0012:       "controlled_nodes": [
0013:         "loc_cut_arsenal_ruin"
0014:       ],
0015:       "control_points": [
0016:         "loc_settlement_nine_rails",
0017:         "loc_weighbridge"
0018:       ],
0019:       "contested_with": [
0020:         "faction_the_tally",
```

### Current evidence: `src/UI/MapAtlasPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/MapAtlasPanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `59bde9b7fa99cff27b095b688dafbe9f11c7590dafcc949956bfc784437a5d1b`
- Snapshot size: 17675 characters; 447 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0013: ///
0014: /// This panel is presentation-only. It projects the live WastelandMapSystem
0015: /// read model and uses ExpeditionHostSession only for active-sortie count.
0016: /// It never invents sector IDs, radiation rates, route reachability, or
...
0192:         mapColumn.AddChild(AshfallUiHelpers.MakeMetadata(
0193:             "Rows come from WastelandMapSystem.GetNodeIntel. Unknown nodes are intentionally hidden."));
0194:         mapColumn.AddChild(_grid);
0195:         body.AddChild(mapColumn);
```

### Current evidence: `src/UI/WaystationNetworkPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/WaystationNetworkPanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `c26cabb69854fc616eaf89ab0889854c738a0e3b9b570fee82e12a0b97350260`
- Snapshot size: 7184 characters; 185 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0154:                 if (station == null) continue;
0155:                 var lapsed = WaystationNetworkSystem.LapsedImports(def, station);
0156:                 var row = new HBoxContainer();
0157:                 row.AddThemeConstantOverride("separation", 8);
```

### Current evidence: `src/UI/TravelingCaravanPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/TravelingCaravanPanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `1ac34a6c364a547c4c51833e595c3ca5985ac49c0b465134cdb044df5ecce8e0`
- Snapshot size: 22924 characters; 465 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using System.Linq;
0005: using Godot;
0006: using Ashfall.Core;
0007: using Ashfall.Core.Economy;
0008: using Ashfall.Core.UI;
0009: using AtomicWar.GodotApp;
0010: using DesignTheme = Ashfall.Core.UI.Theme;
0011:
0012: namespace AtomicWar.GodotApp.UI
0013: {
0014:     /// <summary>
0015:     /// ASHFALL — Traveling Caravans & Regional Trade Route Panel.
0016:     /// Manages itinerant merchants, wasteland route nodes, daily movements,
0017:     /// and ration-based outpost barter.
0018:     /// </summary>
0019:     public partial class TravelingCaravanPanel : Control, IBindablePanel
0020:     {
```

### Current evidence: `src/UI/RegionalTreatyPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `1bd35d831f0b31aa24aeb4776056e99064472b09bdc9a33e9b78c979825d6689`
- Snapshot size: 6429 characters; 160 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using Godot;
0004: using Ashfall.Core;
0005: using Ashfall.Core.UI;
0006: using AtomicWar.GodotApp;
0007: using DesignTheme = Ashfall.Core.UI.Theme;
0008:
0009: namespace AtomicWar.GodotApp.UI
0010: {
0011:     public partial class RegionalTreatyPanel : Control, IBindablePanel
0012:     {
0013:         public event Action? OnClose;
0014:
0015:         private AshfallDashboardShell _shell = null!;
0016:         private AshfallStatusRail? _statusRail;
0017:         private VBoxContainer _contentStack = null!;
0018:         private Label _detailText = null!;
0019:
0020:         private RegionalTreatyHostSession? _host;
```

### Current evidence: `src/UI/SubterraneanCartographyPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/SubterraneanCartographyPanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `d1cf9376754824e8950393da6153d3b96790370bb43c3efc58fdd7e16aae22d0`
- Snapshot size: 5139 characters; 126 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using Godot;
0004: using Ashfall.Core.UI;
0005: using DesignTheme = Ashfall.Core.UI.Theme;
0006:
0007: namespace AtomicWar.GodotApp.UI
0008: {
0009:     /// <summary>
0010:     /// ASHFALL — Subterranean Cartography & 3D Cavity GIS (MAP-02).
0011:     /// 3D LiDAR point cloud voxel mapping, underground cavern network GIS, and radiation isobars.
0012:     /// </summary>
0013:     public partial class SubterraneanCartographyPanel : Control, IBindablePanel
0014:     {
0015:         public event Action? OnClose;
0016:
0017:         private Label _headerLabel = null!;
0018:         private Label _statusLabel = null!;
0019:         private Label _voxelLabel = null!;
0020:         private Label _cavityLabel = null!;
```

### Current evidence: `Ashfall.Core.Tests/World/Plan16CartographyTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e8b1e7ea938eef913778422359c7882086ce6bf4a0c6681122ca7b32b08a2187`
- Snapshot size: 10327 characters; 249 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0025:             string dataDir = GetDataDir();
0026:             var (nodes, routes) = WastelandMapCatalogLoader.Load(dataDir);
0027:
0028:             Assert.True(nodes.Count >= 6);
...
0059:             string dataDir = GetDataDir();
0060:             var (_, routes) = WastelandMapCatalogLoader.Load(dataDir);
0061:
0062:             foreach (var route in routes)
...
0081:
0082:             var (mapNodes, _) = WastelandMapCatalogLoader.Load(dataDir);
0083:             var mapNodeIds = mapNodes.Select(n => n.Id).ToHashSet();
0084:             string locPath = Path.Combine(dataDir, "locations.json");
...
0112:         [Fact]
0113:         public void WaystationNetworkSystem_FilterDecayAndMaintenance_OperatesCorrectly()
0114:         {
0115:             string dataDir = GetDataDir();
...
0150:
0151:             var (mapNodes, _) = WastelandMapCatalogLoader.Load(dataDir);
0152:             var mapNodeIds = mapNodes.Select(n => n.Id).ToHashSet();
0153:
...
0205:
0206:             var (mapNodes, _) = WastelandMapCatalogLoader.Load(dataDir);
0207:             var mapNodeIds = mapNodes.Select(n => n.Id).ToHashSet();
0208:
...
0220:             string dataDir = GetDataDir();
0221:             var mapSystem = WastelandMapCatalogLoader.CreateSystem(dataDir);
0222:             mapSystem.Discover("loc_cut_abandoned_depot");
0223:             mapSystem.Discover("loc_cut_merchant_caravanserai");
...
0229:
0230:             var newMapSystem = new WastelandMapSystem(mapState, mapSystem.Nodes, mapSystem.Routes);
0231:             Assert.True(newMapSystem.IsDiscovered("loc_cut_abandoned_depot"));
0232:             Assert.True(newMapSystem.IsLocked("loc_black_flotilla_outpost"));
...
0235:             var wsCatalog = WaystationCatalogLoader.Load(dataDir);
0236:             var wsSystem = new WaystationNetworkSystem(wsCatalog);
0237:             wsSystem.RepairFilter("waystation_alpha_cut");
0238:             wsSystem.AssignWatch("waystation_verity", new[] { "survivor_medic_1" });
```

### Current evidence: `Ashfall.Core.Tests/World/WastelandMapSystemTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `855fb0173db437b7cf6e4b4c8d86de6fe71a958fa2ccfc7f500b040ed3b72d8c`
- Snapshot size: 4917 characters; 151 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0008: {
0009:     public class WastelandMapSystemTests
0010:     {
0011:         private static WastelandMapSystem MakeMap()
...
0033:             };
0034:             return new WastelandMapSystem(new WastelandMapState(), nodes, routes);
0035:         }
0036:
```

### Current evidence: `Ashfall.Core.Tests/World/WastelandMapRouteValidationTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e1b98ca978808496266d3ebcb4ecb51f8eec2d907c7ad6c2942e898c8f283db8`
- Snapshot size: 8179 characters; 186 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0028:             string dataDir = GetDataDir();
0029:             var (nodes, routes, errors) = WastelandMapCatalogLoader.LoadWithValidation(dataDir);
0030:
0031:             Assert.NotEmpty(nodes);
...
0045:
0046:             var errors = WastelandMapCatalogLoader.ValidateRoutes(nodes, routes);
0047:
0048:             Assert.Single(errors);
...
0061:
0062:             var errors = WastelandMapCatalogLoader.ValidateRoutes(nodes, routes);
0063:
0064:             Assert.Contains(errors, e => e.Kind == MapRouteErrorKind.DanglingEndpoint && e.ErrorMessage.Contains("'from'"));
...
0075:
0076:             var errors = WastelandMapCatalogLoader.ValidateRoutes(nodes, routes);
0077:
0078:             Assert.Contains(errors, e => e.Kind == MapRouteErrorKind.DanglingEndpoint && e.ErrorMessage.Contains("'to'"));
...
0090:
0091:             var errors = WastelandMapCatalogLoader.ValidateRoutes(nodes, routes);
0092:
0093:             Assert.Equal(2, errors.Count);
...
0105:
0106:             var errors = WastelandMapCatalogLoader.ValidateRoutes(nodes, routes);
0107:
0108:             Assert.Single(errors);
...
0113:         [Fact]
0114:         public void WastelandMapSystem_SanitizesInvalidRoutesOnConstruction()
0115:         {
0116:             var nodes = GetTestNodes();
...
0125:
0126:             var system = new WastelandMapSystem(new WastelandMapState(), nodes, mixedRoutes);
0127:
0128:             // Only the 1 valid route should be present in the active system
```

### Current evidence: `Ashfall.Core.Tests/WastelandMapPersistenceTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `56f8197bd392aac6abd6436789d19d2d15f4e1082e58a1fcd8ccabc63bd694a5`
- Snapshot size: 10797 characters; 261 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0033:             var state = new WastelandMapState();
0034:             var sys = new WastelandMapSystem(state, DummyNodes, DummyRoutes);
0035:             sys.Discover("loc_b");
0036:             var snap = sys.CaptureState();
...
0048:             var state = new WastelandMapState();
0049:             var sys = new WastelandMapSystem(state, DummyNodes, DummyRoutes);
0050:             sys.Discover("loc_b");
0051:             var snap = sys.CaptureState();
...
0059:             var state = new WastelandMapState();
0060:             var sys = new WastelandMapSystem(state, DummyNodes, DummyRoutes);
0061:             sys.Discover("loc_b");
0062:             var snap = sys.CaptureState();
...
0065:             var state2 = new WastelandMapState();
0066:             var sys2 = new WastelandMapSystem(state2, DummyNodes, DummyRoutes);
0067:             sys2.RestoreState(snap);
0068:             Assert.Contains("loc_c", sys2.CaptureState().Discovered);
...
0076:             var state = new WastelandMapState();
0077:             var sys = new WastelandMapSystem(state, DummyNodes, DummyRoutes);
0078:             // Starting node loc_a should be discovered even if not explicitly discovered
0079:             Assert.Contains("loc_a", sys.CaptureState().Discovered);
...
0085:         {
0086:             var sys1 = new WastelandMapSystem(new WastelandMapState(), DummyNodes, DummyRoutes);
0087:             sys1.Discover("loc_b");
0088:             sys1.Discover("loc_c");
...
0091:
0092:             var sys2 = new WastelandMapSystem(new WastelandMapState(), DummyNodes, DummyRoutes);
0093:             sys2.RestoreState(state);
0094:
...
0103:         {
0104:             var sys1 = new WastelandMapSystem(new WastelandMapState(), DummyNodes, DummyRoutes);
0105:             // loc_locked_static is locked by definition
0106:             Assert.True(sys1.IsLocked("loc_locked_static"));
...
0116:
0117:             var sys2 = new WastelandMapSystem(new WastelandMapState(), DummyNodes, DummyRoutes);
0118:             sys2.RestoreState(state);
0119:
...
0127:         {
0128:             var sys1 = new WastelandMapSystem(new WastelandMapState(), DummyNodes, DummyRoutes);
0129:             sys1.Complete("loc_b");
0130:             sys1.Complete("loc_c");
...
0140:
0141:             var sys2 = new WastelandMapSystem(new WastelandMapState(), DummyNodes, DummyRoutes);
0142:             sys2.RestoreState(state);
0143:
...
```

### Current evidence: `Ashfall.Core.Tests/WaystationSystemTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `da18e2893cbd2df0e69943aca046e65f033ec9e9b09220c95a7bedbf3ebe36f0`
- Snapshot size: 5279 characters; 151 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using Xunit;
0003: using Ashfall.Core;
0004:
0005: namespace Ashfall.Core.Tests
0006: {
0007:     /// <summary>
0008:     /// Cross-tool review of the WaystationSystem extraction (Holdfast §5.4).
0009:     /// One forward camp: watch cap, filter burn, stove notches, resupply,
0010:     /// wintering, save roundtrip.
0011:     /// </summary>
0012:     public class WaystationSystemTests
0013:     {
0014:         [Fact]
0015:         public void LockedUntilUnlock()
0016:         {
0017:             var way = new WaystationSystem();
0018:             Assert.False(way.Unlocked);
0019:             Assert.False(way.AssignWatch(new[] { "sv_one" }));
0020:             float before = way.State.filterHealth;
```

### Current evidence: `Ashfall.Core.Tests/TravelingCaravanSystemTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `b29870657e2d1f44edb5e1c52d2507e11a9ac78556e0149731fdd6b0c10f369e`
- Snapshot size: 8518 characters; 226 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0007: {
0008:     public class TravelingCaravanSystemTests
0009:     {
0010:         [Fact]
...
0040:             };
0041:             var system = new TravelingCaravanSystem(input);
0042:
0043:             input.completedTradesCount = 42;
...
0065:             };
0066:             var system = new TravelingCaravanSystem(input);
0067:
0068:             system.DailyTick();
...
0075:         {
0076:             var system = new TravelingCaravanSystem();
0077:             system.SpawnCaravan("c1", "Trader", "f1", new List<string> { "node_a" });
0078:             system.SpawnCaravan("c1", "Duplicate", "f1", new List<string> { "node_b" });
...
0087:         {
0088:             var system = new TravelingCaravanSystem();
0089:             var route = new List<string> { "node_a", "node_b" };
0090:             system.SpawnCaravan("c1", "Caravan 1", "f1", route);
...
0111:         {
0112:             var system = new TravelingCaravanSystem();
0113:             var route = new List<string> { "node_market" };
0114:             system.SpawnCaravan("c1", "Trader", "f1", route);
...
0133:         {
0134:             var system = new TravelingCaravanSystem();
0135:             system.SpawnCaravan("c1", "Trader", "f1", new List<string> { "node_market" });
0136:             var caravan = system.GetCaravanAtNode("node_market")!;
...
0150:         {
0151:             var system = new TravelingCaravanSystem();
0152:             system.SpawnCaravan("c1", "Trader", "f1", new List<string> { "node_market" });
0153:             var caravan = system.GetCaravanAtNode("node_market")!;
...
0169:         {
0170:             var system = new TravelingCaravanSystem();
0171:             var route = new List<string> { "node_x" };
0172:             system.SpawnCaravan("c1", "Trader", "f1", route);
...
0188:         {
0189:             var system = new TravelingCaravanSystem();
0190:             var route = new List<string> { "node_x" };
0191:             system.SpawnCaravan("c1", "Trader", "f1", route);
...
0196:
0197:             var newSystem = new TravelingCaravanSystem();
0198:             newSystem.RestoreState(snapshot);
0199:
...
```

### Current evidence: `Ashfall.Core.Tests/RegionalTreatySystemTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `4d3c22cda0ca506fbbb488af54c5092ec8481854aedc8f11050f0e199800477c`
- Snapshot size: 3469 characters; 101 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0006: {
0007:     public class RegionalTreatySystemTests
0008:     {
0009:         [Fact] public void Propose_CreatesTreaty()
...
0081:
0082:         private static RegionalTreatySystem Create() => new RegionalTreatySystem();
0083:
0084:         private static System.Collections.Generic.List<TreatyDefinition> MakeTreaties()
```

### Current evidence: `Ashfall.Core.Tests/Economy/CaravanTradeNetworkTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `9e9739d2db2d5433a1b8f1350e2f521c8c23e26475005ab57555046a14ddd374`
- Snapshot size: 9685 characters; 236 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using System.IO;
0005: using Ashfall.Core;
0006: using Ashfall.Core.Economy;
0007: using Ashfall.Core.Inventory;
0008: using Xunit;
0009:
0010: namespace Ashfall.Core.Tests.Economy
0011: {
0012:     public sealed class CaravanTradeNetworkTests
0013:     {
0014:         private static string GetDataDir()
0015:         {
0016:             string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
0017:             if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
0018:             candidate = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data");
0019:             if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
0020:             var dir = new DirectoryInfo(AppContext.BaseDirectory);
```

## Current JSON audit

#### `Assets/StreamingAssets/Data/wasteland_map_v1.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, trapSites, nodes, routes`
- `trapSites`: list count=1; sample IDs=[]
- `nodes`: list count=22; sample IDs=['loc_holdfast', 'loc_cut_abandoned_depot', 'loc_cut_radiation_zone_alpha', 'loc_black_flotilla_outpost', 'loc_cut_merchant_caravanserai', 'loc_cut_arsenal_ruin', 'loc_hidden_relay_bunker', 'loc_logistics_reserve_cache']
- `routes`: list count=68; sample IDs=[]
- `schema_version`: `2`
- SHA-256: `5384c1c4093451593327ccc1fce9f8708b21923b8c94104a17e94d72808adf2b`
#### `Assets/StreamingAssets/Data/damaged_map_zones.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, zones`
- `zones`: list count=12; sample IDs=['industrial_district', 'suburban_heights', 'military_corridor', 'crater_ground_zero', 'deep_coast_shelf', 'high_scarp_ridgeline', 'old_medical_quarter', 'court_district']
- `schema_version`: `1`
- SHA-256: `209af93c2b21f195faf3efb99e8f1645100be56b0b6c02c2045e715afc2cc1e0`
#### `Assets/StreamingAssets/Data/caravans.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, caravans`
- `caravans`: list count=4; sample IDs=[]
- `schema_version`: `1`
- SHA-256: `0cb331e654f547f6f642e6780f3594d933bffd5437724e456f73d37ed42a1fac`
#### `Assets/StreamingAssets/Data/caravan_trade_routes.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, routes`
- `routes`: list count=10; sample IDs=['route_compact_supply_line', 'route_compact_iron_corridor', 'route_compact_grain_express', 'route_scale_salt_artery', 'route_scale_mercantile_circuit', 'route_scale_chemical_exchange', 'route_flotilla_coastal_drift', 'route_flotilla_salvage_run']
- `schema_version`: `1`
- SHA-256: `374993eb7b5b811ca87f437c1eda6e417f08254a77b0ee8a90202336327a8f7a`
#### `Assets/StreamingAssets/Data/waystations.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, waystations`
- `waystations`: list count=14; sample IDs=['waystation_alpha_cut', 'waystation_switchback', 'waystation_span44', 'waystation_verity', 'waystation_coast_lock', 'waystation_grain_verge', 'waystation_south_beacon', 'waystation_tinkers_notch']
- `schema_version`: `1`
- SHA-256: `e213387763d78088d937e82af1de682d38def9e591ff2157a12c0ec95ed7b602`
#### `Assets/StreamingAssets/Data/diplomatic_treaties.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, treaties`
- `treaties`: list count=8; sample IDs=[]
- `schema_version`: `1`
- SHA-256: `3a13ed89328f54911f275e2a4ef6949046d66473bd5b0ab73651bb56aaa23a00`
#### `Assets/StreamingAssets/Data/treaty_templates.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, templates`
- `templates`: list count=6; sample IDs=[]
- `schema_version`: `1`
- SHA-256: `ec93396ed45ea9bd08a5bb96ffba60ac2d0a92b502b61372d685b81c3f754cf6`
#### `Assets/StreamingAssets/Data/faction_territory.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, collection_id, territories, contested_zones`
- `territories`: list count=19; sample IDs=['territory_the_office', 'territory_the_cutters', 'territory_black_flotilla', 'territory_the_fleet', 'territory_deserter_coalition', 'territory_cold_count', 'territory_the_tally', 'territory_grain_exchange']
- `contested_zones`: list count=5; sample IDs=['zone_contested_water_rights', 'zone_contested_cut_salvage', 'zone_contested_merchant_crossroads', 'zone_contested_scarp_pass', 'zone_contested_coastal_bluff']
- `schema_version`: `1`
- `collection_id`: `faction_territory_catalog`
- SHA-256: `ef0940bbe98f3082b75cbbef77df670e7e2c7358b89ddeefc20b8fd119b90915`
## Symbol and caller audit

#### `WastelandMapSystem` — HOST_REFERENCE_PRESENT — core/declaration=32, host=39, test=72
- `Assets/Ashfall.Core/TravelingCaravanSystem.cs:73` (core) — public WastelandMapSystem? Map { get; set; }
- `Assets/Ashfall.Core/Economy/CaravanTradeNetworkSystem.cs:107` (core) — public WastelandMapSystem? Map { get; set; }
- `Assets/Ashfall.Core/Expeditions/ReconTelemetrySystem.cs:26` (core) — private readonly WastelandMapSystem? _wastelandMap;
- `Assets/Ashfall.Core/Expeditions/ReconTelemetrySystem.cs:60` (core) — WastelandMapSystem? wastelandMap = null,
- `Assets/Ashfall.Core/Narrative/CipherQuestChainEngine.cs:116` (core) — public void RecordBroadcastHeard(string broadcastOrStationId, WastelandMapSystem? map = null)
- `Assets/Ashfall.Core/Narrative/CipherQuestChainEngine.cs:130` (core) — public void RecordKeyAcquired(string itemId, WastelandMapSystem? map = null)
- `Assets/Ashfall.Core/Narrative/CipherQuestChainEngine.cs:143` (core) — public bool EvaluateDecode(CipherChainDefinition def, WastelandMapSystem? map = null)
- `Assets/Ashfall.Core/Narrative/CipherQuestChainEngine.cs:173` (core) — public void RestoreState(List<CipherQuestState>? savedStates, WastelandMapSystem? map = null)
- `Assets/Ashfall.Core/World/DamagedMapCatalog.cs:306` (core) — WastelandMapSystem? wastelandMap,
- `Assets/Ashfall.Core/World/DamagedMapSystem.cs:20` (core) — /// <see cref="WastelandMapSystem.Discover"/> plus
- `Assets/Ashfall.Core/World/DamagedMapSystem.cs:21` (core) — /// <see cref="WastelandMapSystem.Unlock"/> of the installation node —
- `Assets/Ashfall.Core/World/DamagedMapSystem.cs:33` (core) — private readonly WastelandMapSystem? _map;
- `Assets/Ashfall.Core/World/DamagedMapSystem.cs:46` (core) — public DamagedMapSystem(IReadOnlyList<DamagedMapZone> zones, WastelandMapSystem? map)
- `Assets/Ashfall.Core/World/WastelandMapCatalogLoader.cs:322` (core) — public static WastelandMapSystem CreateSystem(
- `Assets/Ashfall.Core/World/WastelandMapCatalogLoader.cs:344` (core) — return new WastelandMapSystem(state ?? new WastelandMapState(), nodes, routes, trapSites, tunnelCatalog);
- `Assets/Ashfall.Core/World/WorldEvolutionEngine.cs:101` (core) — WastelandMapSystem? map)
- `Assets/Ashfall.Core/World/WorldEvolutionEngine.cs:132` (core) — WastelandMapSystem? map)
- `Assets/Ashfall.Core/World/WorldEvolutionEngine.cs:182` (core) — public void RestoreState(WorldEvolutionState? saved, WastelandMapSystem? map = null)
- `Assets/Ashfall.Core/World/WastelandMapSystem.cs:22` (declaration) — public sealed class WastelandMapSystem
- `Assets/Ashfall.Core/World/WastelandMapSystem.cs:39` (core) — public WastelandMapSystem(WastelandMapState state,
- `Assets/Ashfall.Core/World/WastelandMapSystem.cs:56` (core) — throw new InvalidOperationException("WastelandMapSystem: at least one node required.");
- `Assets/Ashfall.Core/World/LivingMapRouteProjection.cs:10` (core) — /// total hops, and distance from canonical WastelandMapSystem.PlanRoute.
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:502` (core) — ["damaged_map_zones.json"] = new[] { "WastelandMapSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:763` (core) — ["wasteland_map_v1.json"] = "WastelandMapSystem",
- … 119 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `WastelandMapCatalogLoader` — HOST_REFERENCE_PRESENT — core/declaration=4, host=10, test=42
- `Assets/Ashfall.Core/World/WastelandMapCatalogLoader.cs:146` (declaration) — public static class WastelandMapCatalogLoader
- `Assets/Ashfall.Core/Content/CollectibleCatalogIntegrityValidator.cs:171` (core) — var mapFile = WastelandMapCatalogLoader.Load(dataDir, fileIO, json);
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:349` (core) — ["locations.json"] = new[] { "LocationLayoutSystem", "WastelandMapCatalogLoader" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:360` (core) — ["wasteland_map_v1.json"] = new[] { "WastelandMapCatalogLoader" },
- `src/Main.TunnelNetwork.cs:33` (host) — /// composed. The catalog itself is seeded by WastelandMapCatalogLoader
- `src/Host/ContentUtilizationRuntimeCollector.cs:828` (host) — instr.RecordCatalogOpened("wasteland_map_v1.json", "WastelandMapCatalogLoader");
- `src/Host/ContentUtilizationRuntimeCollector.cs:830` (host) — var map = WastelandMapCatalogLoader.Load(dataDir, files, json);
- `src/Host/HostCli.Cartography.cs:41` (host) — var (nodes, routes) = WastelandMapCatalogLoader.Load(dataDirectory, files, json);
- `src/Host/HostCli.Collectibles.cs:384` (host) — var liveMapNodes = WastelandMapCatalogLoader.Load(dataDirectory, fileIO, serializer);
- `src/Host/HostCli.EvolvingWorld.cs:178` (host) — var map = WastelandMapCatalogLoader.CreateSystem(dataDirectory);
- `src/Host/HostCli.WorldExploration.cs:70` (host) — var (nodes, routes) = WastelandMapCatalogLoader.Load(dataDirectory, files, json);
- `src/Host/SevenDayDeterministicSmokeTest.cs:442` (host) — map = WastelandMapCatalogLoader.CreateSystem(dataDirectory);
- `src/Host/WorldHostSession.cs:101` (host) — WastelandMap = wastelandMap ?? WastelandMapCatalogLoader.CreateSystem(string.Empty);
- `src/Host/WorldHostSession.cs:117` (host) — ? WastelandMapCatalogLoader.CreateSystem(dataDir)
- `Ashfall.Core.Tests/EvolvingWorldActivationTests.cs:430` (test) — var map = WastelandMapCatalogLoader.CreateSystem(DataDirectory);
- `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs:41` (test) — var (nodes, routes) = WastelandMapCatalogLoader.Load(_dataDir, _files, _json);
- `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs:117` (test) — var (nodes, routes) = WastelandMapCatalogLoader.Load(_dataDir, _files, _json);
- `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs:202` (test) — var (nodes, routes) = WastelandMapCatalogLoader.Load(_dataDir, _files, _json);
- `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs:244` (test) — var (nodes, routes) = WastelandMapCatalogLoader.Load(_dataDir, _files, _json);
- `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs:288` (test) — var (nodes, routes) = WastelandMapCatalogLoader.Load(_dataDir, _files, _json);
- `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs:313` (test) — var (nodes, routes) = WastelandMapCatalogLoader.Load(_dataDir, _files, _json);
- `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs:392` (test) — var (nodes, routes) = WastelandMapCatalogLoader.Load(_dataDir, _files, _json);
- `Ashfall.Core.Tests/World/Plan11ExplorationTests.cs:110` (test) — var (nodes, routes) = WastelandMapCatalogLoader.Load(dataDir);
- `Ashfall.Core.Tests/World/Plan11ExplorationTests.cs:139` (test) — var (nodes, routes) = WastelandMapCatalogLoader.Load(dataDir);
- … 32 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `DamagedMapSystem` — HOST_REFERENCE_PRESENT — core/declaration=7, host=3, test=24
- `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs:315` (core) — public DamagedMapSystem? DamagedMap { get; set; }
- `Assets/Ashfall.Core/World/DamagedMapCatalog.cs:121` (core) — /// WastelandMapState via <see cref="DamagedMapSystem"/>.
- `Assets/Ashfall.Core/World/DamagedMapCatalog.cs:300` (core) — /// Loads the catalog and builds a <see cref="DamagedMapSystem"/> bound
- `Assets/Ashfall.Core/World/DamagedMapCatalog.cs:304` (core) — public static DamagedMapSystem? CreateSystem(
- `Assets/Ashfall.Core/World/DamagedMapCatalog.cs:313` (core) — return new DamagedMapSystem(zones, wastelandMap);
- `Assets/Ashfall.Core/World/DamagedMapSystem.cs:27` (declaration) — public sealed class DamagedMapSystem
- `Assets/Ashfall.Core/World/DamagedMapSystem.cs:46` (core) — public DamagedMapSystem(IReadOnlyList<DamagedMapZone> zones, WastelandMapSystem? map)
- `src/Host/ExpeditionHostSession.cs:230` (host) — public void AttachDamagedMapFeedback(DamagedMapSystem? damagedMap)
- `src/Host/WorldHostSession.cs:28` (host) — public DamagedMapSystem? DamagedMap { get; private set; }
- `src/Host/WorldHostSession.cs:89` (host) — DamagedMapSystem? damagedMap = null,
- `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs:39` (test) — private (DamagedMapSystem system, WastelandMapSystem map, List<DamagedMapZone> zones) CreateLive()
- `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs:45` (test) — return (new DamagedMapSystem(zones, map), map, zones);
- `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs:122` (test) — string nodeId = DamagedMapSystem.ResolveRevealNodeId(zone.InstallationId)!;
- `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs:137` (test) — string nodeId = DamagedMapSystem.ResolveRevealNodeId(zone.InstallationId)!;
- `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs:195` (test) — string nodeId = DamagedMapSystem.ResolveRevealNodeId(zone.InstallationId)!;
- `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs:204` (test) — var restoredSystem = new DamagedMapSystem(zones, restoredMap);
- `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs:224` (test) — string nodeId = DamagedMapSystem.ResolveRevealNodeId(zone.InstallationId)!;
- `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs:253` (test) — var system = new DamagedMapSystem(zones, map);
- `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs:293` (test) — var forward = new DamagedMapSystem(zones, new WastelandMapSystem(new WastelandMapState(), nodes, routes));
- `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs:294` (test) — var backward = new DamagedMapSystem(shuffled, new WastelandMapSystem(new WastelandMapState(), nodes, routes));
- `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs:304` (test) — forward.IsDestinationLocked(DamagedMapSystem.ResolveRevealNodeId(zone.InstallationId)!),
- `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs:305` (test) — backward.IsDestinationLocked(DamagedMapSystem.ResolveRevealNodeId(zone.InstallationId)!));
- `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs:316` (test) — var damagedMap = new DamagedMapSystem(zones, map);
- `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs:395` (test) — var damagedMap = new DamagedMapSystem(zones, map);
- … 10 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `WaystationNetworkSystem` — HOST_REFERENCE_PRESENT — core/declaration=3, host=5, test=11
- `Assets/Ashfall.Core/Waystation/WaystationNetworkSystem.cs:25` (core) — public string systemId = WaystationNetworkSystem.SystemId;
- `Assets/Ashfall.Core/Waystation/WaystationNetworkSystem.cs:34` (declaration) — public sealed class WaystationNetworkSystem
- `Assets/Ashfall.Core/Waystation/WaystationNetworkSystem.cs:59` (core) — public WaystationNetworkSystem(List<WaystationDef>? catalog = null, WaystationNetworkState? state = null)
- `src/Main.ShelterInfrastructure.cs:579` (host) — var network = new WaystationNetworkSystem();
- `src/Host/HostCli.Cartography.cs:77` (host) — var wsSystem = new WaystationNetworkSystem(waystations);
- `src/Host/WaystationHostSession.cs:26` (host) — public WaystationNetworkSystem? Network { get; private set; }
- `src/Host/WaystationHostSession.cs:91` (host) — WaystationNetworkSystem network,
- `src/UI/WaystationNetworkPanel.cs:155` (host) — var lapsed = WaystationNetworkSystem.LapsedImports(def, station);
- `Ashfall.Core.Tests/Plan56Phase3Tests.cs:186` (test) — private static WaystationNetworkSystem WaystationUnderTest(bool marketShort)
- `Ashfall.Core.Tests/Plan56Phase3Tests.cs:189` (test) — var network = new WaystationNetworkSystem();
- `Ashfall.Core.Tests/Plan56Phase6Tests.cs:46` (test) — private static WaystationNetworkSystem NetworkWithPolicy(bool marketShort)
- `Ashfall.Core.Tests/Plan56Phase6Tests.cs:48` (test) — var network = new WaystationNetworkSystem(TwoStationCatalog());
- `Ashfall.Core.Tests/Plan56Phase6Tests.cs:88` (test) — var lapsed = WaystationNetworkSystem.LapsedImports(def, station);
- `Ashfall.Core.Tests/Plan56Phase6Tests.cs:104` (test) — Assert.Empty(WaystationNetworkSystem.LapsedImports(def, station));
- `Ashfall.Core.Tests/Plan56Phase6Tests.cs:113` (test) — var network = new WaystationNetworkSystem(TwoStationCatalog());
- `Ashfall.Core.Tests/Plan56Phase6Tests.cs:130` (test) — var restored = new WaystationNetworkSystem(TwoStationCatalog());
- `Ashfall.Core.Tests/World/Plan16CartographyTests.cs:117` (test) — var system = new WaystationNetworkSystem(catalog);
- `Ashfall.Core.Tests/World/Plan16CartographyTests.cs:236` (test) — var wsSystem = new WaystationNetworkSystem(wsCatalog);
- `Ashfall.Core.Tests/World/Plan16CartographyTests.cs:241` (test) — var newWsSystem = new WaystationNetworkSystem(wsCatalog, wsState);
#### `TravelingCaravanSystem` — HOST_REFERENCE_PRESENT — core/declaration=5, host=5, test=26
- `Assets/Ashfall.Core/TravelingCaravanHeadlessDemo.cs:29` (core) — var system = new TravelingCaravanSystem();
- `Assets/Ashfall.Core/TravelingCaravanHeadlessDemo.cs:62` (core) — var newSystem = new TravelingCaravanSystem();
- `Assets/Ashfall.Core/TravelingCaravanSystem.cs:52` (declaration) — public class TravelingCaravanSystem
- `Assets/Ashfall.Core/TravelingCaravanSystem.cs:86` (core) — public TravelingCaravanSystem(TravelingCaravanState? state = null)
- `Assets/Ashfall.Core/TravelingCaravanSystem.cs:144` (core) — private static void AddRegionalSpecialtyStock(TravelingCaravanSystem system, CaravanEntry caravan, string region)
- `src/Host/HostCli.Cartography.cs:92` (host) — var caravanSystem = new TravelingCaravanSystem();
- `src/Host/HostCli.Cartography.cs:100` (host) — Check(caravanSystem.CaravanCount >= 4, $"TravelingCaravanSystem initialized with {caravanSystem.CaravanCount} active caravans");
- `src/Host/TravelingCaravanHostSession.cs:18` (host) — public TravelingCaravanSystem Engine { get; }
- `src/Host/TravelingCaravanHostSession.cs:21` (host) — public TravelingCaravanHostSession(TravelingCaravanSystem engine = null!)
- `src/Host/TravelingCaravanHostSession.cs:23` (host) — Engine = engine ?? new TravelingCaravanSystem();
- `Ashfall.Core.Tests/CaravanPatrolIntegrationTests.cs:37` (test) — var caravanSys = new TravelingCaravanSystem
- `Ashfall.Core.Tests/CaravanPatrolIntegrationTests.cs:80` (test) — var caravanSys = new TravelingCaravanSystem
- `Ashfall.Core.Tests/CaravanPatrolIntegrationTests.cs:119` (test) — var caravanSys = new TravelingCaravanSystem
- `Ashfall.Core.Tests/CaravanPatrolIntegrationTests.cs:149` (test) — var caravanSys = new TravelingCaravanSystem
- `Ashfall.Core.Tests/PatrolCampaignCrossSystemSmokeTests.cs:49` (test) — public TravelingCaravanSystem CaravanSystem { get; set; }
- `Ashfall.Core.Tests/PatrolCampaignCrossSystemSmokeTests.cs:66` (test) — CaravanSystem = new TravelingCaravanSystem
- `Ashfall.Core.Tests/PatrolCampaignCrossSystemSmokeTests.cs:267` (test) — var restoredCaravan = new TravelingCaravanSystem
- `Ashfall.Core.Tests/Plan56Phase3Tests.cs:148` (test) — var system = new TravelingCaravanSystem { Catalog = catalog };
- `Ashfall.Core.Tests/Plan56Phase3Tests.cs:176` (test) — var system = new TravelingCaravanSystem();
- `Ashfall.Core.Tests/SaveSnapshotAliasCaravanWastelandTests.cs:137` (test) — var sys = new TravelingCaravanSystem();
- `Ashfall.Core.Tests/TravelingCaravanSystemTests.cs:13` (test) — var system = new TravelingCaravanSystem();
- `Ashfall.Core.Tests/TravelingCaravanSystemTests.cs:41` (test) — var system = new TravelingCaravanSystem(input);
- `Ashfall.Core.Tests/TravelingCaravanSystemTests.cs:66` (test) — var system = new TravelingCaravanSystem(input);
- `Ashfall.Core.Tests/TravelingCaravanSystemTests.cs:76` (test) — var system = new TravelingCaravanSystem();
- … 12 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `RegionalTreatySystem` — HOST_REFERENCE_PRESENT — core/declaration=12, host=7, test=10
- `Assets/Ashfall.Core/RegionalTreatyFeed.cs:13` (core) — /// RegionalTreatySystem finally ships a catalog. Balance mapping is
- `Assets/Ashfall.Core/RegionalTreatySystem.cs:11` (core) — public string systemId = RegionalTreatySystem.SystemId;
- `Assets/Ashfall.Core/RegionalTreatySystem.cs:58` (declaration) — public sealed class RegionalTreatySystem
- `Assets/Ashfall.Core/RegionalTreatySystem.cs:75` (core) — public RegionalTreatySystem(ILog? log = null)
- `Assets/Ashfall.Core/RegionalTreatyCatalogLoader.cs:9` (core) — /// Mechanical treaty catalog for <see cref="RegionalTreatySystem"/>.
- `Assets/Ashfall.Core/Economy/CaravanTradeNetworkSystem.cs:529` (core) — /// provider (typically RegionalTreatySystem.GetTradeDiscount), so nothing is
- `Assets/Ashfall.Core/Muster/MusterPathEvaluator.cs:18` (core) — /// HOST maps live systems (FactionWarSystem, RegionalTreatySystem, flag ledger,
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:365` (core) — ["regional_treaties.json"] = new[] { "RegionalTreatyCatalogLoader", "RegionalTreatySystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:718` (core) — ["regional_treaties.json"] = "RegionalTreatySystem",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:966` (core) — ["regional_treaties.json"] = new[] { "RegionalTreatySystem" },
- `Assets/Ashfall.Core/Treaties/TreatyConsequences.cs:12` (core) — /// <see cref="RegionalTreatySystem"/> each read. Effects are derived from
- `Assets/Ashfall.Core/Treaties/TreatyConsequences.cs:35` (core) — /// <summary>Player-initiated breach (<see cref="RegionalTreatySystem.BreakTreaty"/>).</summary>
- `src/Main.ShelterSocial.cs:78` (host) — var rtSys = new RegionalTreatySystem(new GodotLog());
- `src/Main.ShelterSocial.cs:106` (host) — private void OnTreatyTransitionWorldConsequences(RegionalTreatySystem treatySystem, TreatyTransition transition)
- `src/Host/PanelBindLifecycleSelfTest.cs:414` (host) — var regSession = new RegionalTreatyHostSession(new RegionalTreatySystem(log));
- `src/Host/RegionalTreatyHostSession.cs:9` (host) — /// Host session for RegionalTreatySystem.
- `src/Host/RegionalTreatyHostSession.cs:14` (host) — public RegionalTreatySystem System { get; }
- `src/Host/RegionalTreatyHostSession.cs:16` (host) — public RegionalTreatyHostSession(RegionalTreatySystem system)
- `src/Host/RegionalTreatyHostSession.cs:18` (host) — System = system ?? new RegionalTreatySystem(new GodotLog());
- `Ashfall.Core.Tests/RegionalTreatyFeedTests.cs:11` (test) — /// host finally ships a RegionalTreatySystem catalog instead of an empty one.</summary>
- `Ashfall.Core.Tests/RegionalTreatyFeedTests.cs:56` (test) — var system = new RegionalTreatySystem();
- `Ashfall.Core.Tests/RegionalTreatyIntegrationTests.cs:13` (test) — var sys = new RegionalTreatySystem();
- `Ashfall.Core.Tests/RegionalTreatyIntegrationTests.cs:38` (test) — var sys1 = new RegionalTreatySystem();
- `Ashfall.Core.Tests/RegionalTreatyIntegrationTests.cs:47` (test) — var sys2 = new RegionalTreatySystem();
- … 5 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `AllMapNodes_ExistInLocationsCatalog` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=0, host=0, test=1
- `Ashfall.Core.Tests/World/WastelandMapCatalogLoaderTests.cs:92` (test) — public void AllMapNodes_ExistInLocationsCatalog()
## Read-only authority alignment

Authority file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
Authority SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
Total lines in snapshot: 5510

The following excerpts are read-only orientation anchors. They do not override current source/data evidence or create implementation authority.
#### authority lines 41-44
00041: **DR-02 — The docs tree has substantially more subdirectories than the v1.0 map. VERIFIED.**
00042: Live `docs/` subdirectories observed in the audit include (selection; the listing was long and partially truncated): `adr/`, `agents/`, `architecture/`, `archive/`, `balance/`, `bodymind/`, `campaign/`, `cartography/`, `ci/`, `cli/`, `collectibles/`, `combat/`, `content/`, `contracts/`, `crafting/`, `crossing/`, `culture/`, `decisions/`, `design/`, `discovery/`, `duty_roster/`, `ecology/`, `economy/`, `endgame/`, `expansions/`, `expeditions/`, `faction_war/`, `factions/`, `foreman/`, `forensics/`, `foundry/`, `gaps/`, `governance/`, `greenhouse/`, `health/`, `holdfast/`, `hygiene/`, `i18n/`, `implementation/`, `incidents/`, `integration/`, `journal/`, `lore/`, `maritime/`, `medical/`, `memorials/`, `mods/`, `moral/`, `moral_choice/`, `muster/`, `narrative/`, `onboarding/`, `orbital/`, `perf/`, `phantoms/`, `plans/`, `power/`, `process/`, `production/`, and a `player_surface_manifest.json`. Two of these — `gaps/` and `incidents/` — are first-class *expansion feedstock*: directories whose entire purpose is to record what is missing or broken. The Factory Protocol (Part II, step 2) now treats `docs/gaps/` and `docs/incidents/` as mandatory inputs.
00043:
00044: **DR-03 — New top-level authority documents absent from the v1.0 docs map. VERIFIED.**
#### authority lines 117-120
00117:
00118: C1 Shelter operations (rooms, thermal, schedules, fire, decor, barter, noise, prisoners, sanitation, airlock, decon, atmosphere) · C2 Medical pipeline (disease, dose ledger, ARS, surgery, autopsy, pharma, diagnostics, therapies, dependency, crises) · C3 Water, food, agriculture (treatment, condensers, wells, brine, nutrition, kitchen, preservation, grain, greenhouse, crops, aquaponics, apiculture) · C4 Power and industry (grid, SOFC, solar, kinetic, geothermal, foundry, CVD diamond, coatings, optics, powder metallurgy, pyrolysis, Fischer-Tropsch, chlor-alkali, acids, fermentation, ethanol, air separation, metrology) · C5 Expeditions and travel (destinations, scavenging tables, vehicles, waystations, caravans, routes, travel encounters, micro-locations) · C6 Map and geography (wasteland map, damaged zones, fog, route gates, cartography, survey instruments) · C7 Factions and war (stance, doctrines, war chains, tributes, treaties, embargoes, espionage, psyops, infiltration, musters, labor camps, bounties) · C8 Radio and information (stations, programs, intercepts, distress signals, rumors, sound ranging, direction finding, NVIS, heliograph) · C9 Survivors and interiority (needs, skills, traits, arcs, trauma, guilt, therapies, relations, caregiving, beliefs, rituals, memorials, final wishes, lineage, cohorts, apprenticeships) · C10 Quests and moral choice (questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip, branching, bureaucratic morality, expansion quests) · C11 Economy (market, baselines, regional prices, shocks, rumors, black market, debt ledger, tributes, trade screens, tell lines) · C12 Weather and Year of Ash (weather system, seasons, effects, gates, hardening, storm windows, Year-of-Ash families, epilogue pressure) · C13 Endgame and epilogue (Reckoning, verdict, epilogue matrix, chronicle, muster epilogues, standing records, census) · C14 Ecology and wildlife (migration, trapping, ecosystem, bestiary, flora, infestations, contagion, pathogens, crop genomes) · C15 Defense and security (perimeter, defense grid, sky defense, ordnance, chemical defense, orbital harrow, interlocks, EMP effects) · C16 Progression and meta (skills, research, collectibles, trophies, achievements, difficulty presets, XP wave, codex, field guide, bestiary, L10N, mods, settings, input) · C17 Host surface and UI (panels, shell, focus navigation, snapshots, a11y, briefings, dashboards).
00119:
00120: ### 3.1 Lane A — Narrative and prose (all types and kinds)
#### authority lines 687-690
00687:
00688: **A-11 · C4 · Foundry pour-window log continuation.** Subject: cupola pour records conditioned on accord state and treaty consequences. Evidence: foundry corpus exists (`forge_charcoal_ash_assays`, kiln records); `foundry_accords.json` and `foundry_treaty_consequences.json` verified live. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00689:
00690: **A-12 · C5 · Destination arrival/revisit prose completion.** Subject: `arrival_description` and `revisit_description` completion for any of the 53 dispatchable destinations with sparse prose, following the Part 9 field contracts exactly (80–140 / 60–110 words, one landmark, one sensory anchor, one danger indication). Evidence: destination surface is canon (53 destinations, 263-id dispatch surface). Route: DATA-ONLY. Confidence: HIGH CONFIDENCE that gaps exist; measure per destination in session.
#### authority lines 843-846
00843:
00844: **E-08 · C11 · Caravan barter ledger legibility.** Subject: `CaravanBarterLedger` panel state completeness against the sealed merchant-restock display-order priority (DEC-05). Evidence: DEC-05 sealed (DR-06). Route: HOST-WIRING. Confidence: PROPOSAL.
00845:
00846: **E-09 · C12 · Storm-window forecast legibility.** Subject: weather forecast surface rendering storm-window warnings with adequate lead time for the 180–360 window. Evidence: forecast observation is a canon loop step. Route: HOST-WIRING. Confidence: PROPOSAL.
#### authority lines 940-943
00940:
00941: **DM-5 — Expeditions and travel (C5).** Owners: expedition system, vehicles, dispatch preflight, scavenging tables, waystations, caravans, travel encounters, micro-locations, anomalous encounters. Live catalogs: `expeditions`, `vehicles`, `vehicle_modifications`, `vehicle_armor_grades`, `scavenging_tables`, `waystations`, `caravans`, `merchant_caravans`, `caravan_trade_routes`, `travel_encounters`, `micro_locations`, `anomalous_expedition_encounters`. Hosts: Expedition, ExpeditionVehicle, TravelingCaravan, Waystation, RescueDispatchPreflight. Docs: `EXPEDITION_30_DAY_PLAYTEST_REPORT.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md` (verified live). Openings: A-12, A-13, A-14, B-07, B-08, C-03, C-04, E-07.
00942:
00943: **DM-6 — Map and geography (C6).** Owners: wasteland map system/loader, damaged zones, fog, route gates, survey instruments. Live catalogs: `wasteland_map_v1`, `damaged_map_zones`, `weather_route_gates`, `gpr_exploration_catalog`, `insar_geodesy_catalog`, `geodetic_survey_catalog`, `seismic_fault_catalog`, `piezometer_network_catalog`. Hosts: GeodeticSurvey, InSarMapping, Cartography selftest family. Openings: A-15, B-09 (GATE). Constraint: the orphan gate `AllMapNodes_ExistInLocationsCatalog` governs all map authoring.
#### authority lines 978-981
00978: purpose: diegetic inventory/shipping record
00979: trigger: cargo, expedition loadout, caravan arrival
00980: length: 4-10 line items + header
00981: must_include: quantities, condition notes, receiving marks
#### authority lines 1040-1043
01040:   is short tonight, so here is what was counted: four degrees of frost
01041:   on the east road, a caravan burning bad fuel at the crossing, and
01042:   one frequency that still answers. Keep your filters dry. We will
01043:   read the names again at the top of the hour.
#### authority lines 1126-1129
01126:
01127: - 2026-09-25 — Volume 5: prose specification library part 2 (twenty-one additional worked genre contracts: journal, unsent letter, communiqué, directive, liturgy, cipher, dispatch/debrief, graffiti, folklore, almanac, gazetteer, treaty, permit, glitch report, provenance, eulogy, rumor, environmental clue, item inspection, relationship reaction, quest/outcome texts, codex, map texts, world-state notification; genre list now fully covered) — ~17,700 — cumulative ~143,000
01128: - 2026-09-25 — Volume 6: twenty full subject plans expanded from Lane A seeds (FP-A01…FP-A27 selection, with wave sequencing F6-1 through F6-4) — ~16,600 — cumulative ~160,000
01129: - 2026-09-25 — Volume 7: catalog authoring contract library (field-inventory protocol, six CANON record contracts, item authoring rules, integrity checklist, schema-sheet registry with priority inventory order) — ~8,700 — cumulative ~169,000
#### authority lines 1199-1202
01199: purpose: faction-to-faction or faction-to-public formal statement
01200: trigger: faction war chain, treaty step, embargo notice
01201: length: 60-120 words
01202: must_include: the issuing authority, one actionable demand or declaration, one procedural channel
#### authority lines 1273-1276
01273: model (debrief): "Arrived on the third day with the tally intact and
01274:   the twine one knot short. The knot was traded at the waystation,
01275:   deliberately, for stove time. West post holds. The road does not."
01276: ` ` `
#### authority lines 1321-1324
01321:
01322: ## 5.11 Gazetteer entry (cartographic register)
01323:
01324: ` ` `text
#### authority lines 1340-1343
01340:
01341: ## 5.12 Treaty protocol (diplomatic register)
01342:
01343: ` ` `text
#### authority lines 1438-1441
01438: model:
01439:   They say the flats settlement turned away a whole caravan at
01440:   gunpoint over a salt price. The registrar's bench says the
01441:   caravan never had salt to begin with — but someone at the crossing
#### authority lines 1581-1584
01581:
01582: With Volumes 4 and 5, all genres in the v1.0 Part 8.4 list now carry worked contracts: manifest, audit/assay, titration record, log, journal, diary, letter (sent/unsent), intake interview, therapy note, casebook, court verdict, wiretap transcript, communiqué, directive, liturgy, hymnal (liturgy family), canon (religious), epitaph, eulogy, burial record, provenance dossier, rundown, scriptbook, cipher, dispatch, debrief, field report, waypoint note, planning brief, schedule notice, graffiti, carving, folklore (children's and adult), song (folklore family), almanac entry, gazetteer entry, bestiary entry (natural-history family, A-29 model), genealogy (lineage registers), treaty protocol, permit, load-shed schedule, maintenance glitch report, risk-of-failure wishlist (planning-brief family). Sessions extend these; they do not invent parallels.
01583:
01584:
#### authority lines 1695-1698
01695: Lane A · C4 · Status PROPOSAL.
01696: Subject: cupola pour records conditioned on accord state and treaty consequences.
01697: Premise evidence: VERIFIED `foundry_accords.json`, `foundry_treaty_consequences.json`, `foundry_production.json` live; VERIFIED foundry assay corpus families exist.
01698: Must not change: production math; accords semantics.
#### authority lines 1714-1717
01714:
01715: ## FP-A13 — Waystation Register Prose
01716:
01717: Lane A · C5 · Status PROPOSAL.
#### authority lines 1889-1892
01889:
01890: Sessions append one line per catalog they inventory. Until a line exists for a catalog, plans touching it carry the inventory step as an open premise. Priority order for first inventory passes: `moral_choice_flags`, `quests_massive_expansion_200`, `locations` (+ expansions), `items`, `epilogue_chronicle`, `muster_witnesses`, `metrology_standards_catalog`, `waystations`, `foundry_production`.
01891:
01892:
#### authority lines 2165-2168
02165: Lane B · C11 · Status PROPOSAL (data-first).
02166: Subject: additional trade-screen scenarios and tell lines for merchant identities under-covered relative to the merchant/caravan catalogs, deepening the read-the-trader minigame's content surface.
02167: Premise evidence: VERIFIED `trade_screen_scenarios.json`, `trade_tell_lines.json`, `trade_specialties.json` live; VERIFIED merchant restock display-order priority is sealed (DEC-05) and untouched by scenario authoring.
02168: Why this: pure data against an established schema, with zero sealed-surface contact — the safest Lane B content tranche in the economy cluster.
… 20 additional authority matches omitted; the implementation owner must cite the exact relevant section at execution time.
## Objective and success definition

The objective is to make route authoring, reachability, hazard gates, station placement, caravan schedules, treaty effects, and map UI compose through the existing owners while preserving the canonical map-node/location orphan gate. The success condition is not merely a larger catalog or a more attractive panel. Success requires a current owner, a reachable consumer, a durable state decision, deterministic behavior, truthful UI, explicit failure semantics, and a focused verification handoff.

## Current reality, requested behavior, and minimum delta

**Existing behavior.** WastelandMapSystem and loader, DamagedMapSystem, WaystationNetworkSystem, TravelingCaravanSystem, CaravanTradeNetworkSystem, RegionalTreatySystem, save stores, host sessions, and UI surfaces are live. The old proposal’s map-size numbers are not accepted without recounting the current catalogs.

**Requested behavior.** Make route authoring, reachability, hazard gates, station placement, caravan schedules, treaty effects, and map UI compose through the existing owners while preserving the canonical map-node/location orphan gate.

**Minimum safe delta.** Extend `wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels` only after the current owner and caller audit confirms the missing link. If the old plan’s proposed system already exists, convert the task into a bounded maintenance/reachability package rather than creating a replacement.

## Non-goals and collision exclusions

- No parallel gameplay authority, save store, ledger, selector, event bus, simulation, or UI-owned rule.
- No Unity restoration, Unity dependency, or engine types in Core.
- No edits to authored data or production code in this planning-only pass.
- No broad test suite, full runtime soak, generated index rewrite, or unrelated documentation cleanup.
- No invented API, count, save section, or caller claim. Unknowns remain named unknowns.

## Current reality and required delta

**Current reality.** WastelandMapSystem and loader, DamagedMapSystem, WaystationNetworkSystem, TravelingCaravanSystem, CaravanTradeNetworkSystem, RegionalTreatySystem, save stores, host sessions, and UI surfaces are live. The old proposal’s map-size numbers are not accepted without recounting the current catalogs.

**Required delta.** Make route authoring, reachability, hazard gates, station placement, caravan schedules, treaty effects, and map UI compose through the existing owners while preserving the canonical map-node/location orphan gate.

**Primary seam.** wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels

**Non-goals.** This planning package does not modify production, authored data, saves, tests, UI, generated indexes, assets, or runtime code. It does not approve a new owner, authorize Unity work, or turn a historical plan claim into current evidence. The following terms are design hypotheses to verify during implementation, not facts asserted by this document: `WastelandMapSystem`, `WastelandMapCatalogLoader`, `DamagedMapSystem`, `WaystationNetworkSystem`, `TravelingCaravanSystem`, `RegionalTreatySystem`, `AllMapNodes_ExistInLocationsCatalog`.

## Integration framework

The integration framework is deliberately owner-first:

1. **Read and classify current state.** Start with the named Core owner, current JSON, host session, save store, and focused tests. Record whether the feature is live, partially wired, dormant, stale, or decision-gated.
2. **Choose one authority per concern.** Extend the current owner when it exists. If no owner exists, stop at the architecture decision boundary and name the new authority decision rather than creating a parallel store, selector, ledger, panel, or simulation.
3. **Author data against consumers.** A JSON row is not integrated merely because it parses. Every row must have a current loader, a current consumer, a visible or mechanically observable outcome, and a validation path.
4. **Route effects through existing events/seams.** Core emits facts; host sessions translate them; UI presents truthful state. Do not place gameplay calculations in a panel or Godot callback.
5. **Persist through the owning save path.** Capture and restore must be implemented before a feature is called persistent. Old versions, nulls, empty collections, checksums, and mid-event saves are explicit cases.
6. **Verify narrowly.** Use the smallest existing test file or a new focused test for an uncovered confirmed contract. Keep Core, data, host, UI, save, determinism, and cross-system checks distinguishable.
7. **Roll back by boundary.** A failed expansion should disable its adapter or authored tranche without corrupting the owning state or requiring a destructive reset.

## Code architecture

### Core layer

- Put reusable rules, validation, state transitions, deterministic selection, and read models in `Assets/Ashfall.Core/`.
- Keep Core engine-free. No Godot, Unity, `Texture2D`, `JsonUtility`, wall-clock, or unseeded randomness belongs in a Core contract.
- Extend existing models and public methods when the current API already expresses the concern. A new DTO, interface, event, or catalog loader is justified only when it removes a real ownership or boundary problem.
- Make invalid input observable through the owner’s normal result/diagnostic path. Do not silently coerce malformed content into a successful state.
- Keep deterministic ordering explicit: use ordinal IDs, stable catalog order, bounded collections, and the existing seeded RNG fork for any stochastic choice.

### Data layer

- Author under `Assets/StreamingAssets/Data/` using the existing schema and snake_case IDs.
- Prefer additive fields and existing collections over parallel catalogs.
- Validate IDs, references, ranges, and consumer reachability through the current catalog integrity pipeline.
- Record schema version and old-data behavior in the plan and implementing handoff.
- Data prose may describe a consequence only when the consequence is expressible through a current owner and event.

### Host layer

- Load the catalog in the current host/session owner, not in a panel constructor.
- Subscribe once to owner events, translate facts into existing journal/radio/UI signals, and dispose subscriptions with the session.
- Bind day/hour/event triggers through the existing campaign owner. Do not create a second clock or update loop.
- Rehydrate from the existing save owner and mark dirty only for actual canonical mutations.
- Keep host code free of duplicate gameplay math; it may format, route, and adapt.

### Presentation layer

- Panels expose current commands and truthful current state.
- They display unavailable/blocked reasons, provenance, and the next legitimate action rather than simulating a result.
- Preserve keyboard/controller close/back behavior, focus order, readable contrast, and reduced-motion/accessibility settings.
- Refresh from owner events and lifecycle state; never use a panel cache as authority.
- Snapshot or accessibility fixtures are verification artifacts, not gameplay state.

## Ownership and state matrix

| Concern | Authoritative owner | Adapter responsibility | Persistence rule | Verification gate |
|---|---|---|---|---|
| Domain rules | WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands | Bind inputs and translate facts | Existing owner DTO/save | Core focused tests |
| Authored content | Current JSON catalog and loader | Load/validate once | Catalog version/defaults | Data integrity |
| Runtime lifecycle | Existing host/session owner | Setup, event subscription, disposal | Existing save/session | Host wiring |
| Presentation | Existing Godot surface | Format and command dispatch | No gameplay state | UI/focus/headless |
| Diagnostics | Existing logging/telemetry owner | Correlate ID and phase | Bounded/non-authoritative | Failure test |
| Historical authority | Read-only master document | Cite relevant section only | Never persisted | Plan QA |

## Data, event, and command flow

`authoritative JSON / player command / current owner event` → validation at the owning seam → canonical Core state transition → typed fact/event → host session projection → journal/radio/UI refresh → existing save owner if the transition mutated durable state.

The flow is intentionally one-way for authority. UI may send a command, but it cannot directly mutate the domain. Events may be consumed by several read-only projections, but only the owner writes the state. If a proposed feature needs a second writer, that is an architecture failure, not an invitation to add another event bus.

## State, API, and compatibility contract

The implementing agent must confirm the actual public API and write the final signatures in the implementation handoff. At minimum, expose:

- a read-only query/projection for the current state;
- an explicit command or owner method for each player-visible mutation;
- a typed fact/event for meaningful state changes;
- capture/restore methods on the existing state owner;
- a diagnostic result for invalid or unavailable data;
- a stable key for idempotent commands and replay;
- a bounded, ordinal-stable collection for any retained history.

Old saves must default missing additive fields to the documented neutral value. New required fields need a versioned migration. Null and empty semantics must differ deliberately: null means unavailable/not supplied; empty means validly no records, unless the current owner’s contract says otherwise. Do not infer a new save section from the plan title.

## Determinism and replay

For every proposed random or time-dependent element, name the seed source, stream/fork, draw order, retry behavior, and tie-break rule. Prefer no randomness for validation, lookup, and deterministic UI state. If an existing owner uses `ISeededRng`, reuse its campaign stream/fork rather than constructing a private generator. Wall-clock time, `System.Random`, `Guid.NewGuid`, hash iteration order, filesystem enumeration order, and frame timing are prohibited in deterministic Core behavior.

The replay acceptance test must use two equivalent runs with identical seed, catalog snapshot, state, day/hour, and command sequence. Compare canonical state, event order, resource/ledger deltas, and persistence payload. Presentation-only differences are acceptable only when they are explicitly non-authoritative and do not alter commands or outcomes.

## Save, restore, and migration

Before implementation claims persistence:

1. Identify the current save-section owner and DTO.
2. Add or reuse capture/restore through that owner.
3. Deep-copy mutable collections so restoring does not alias runtime state.
4. Define old-version defaults for every new field.
5. Define behavior for missing catalogs, unknown IDs, partial records, and corrupted checksums.
6. Test capture → serialize → restore → continued mutation, plus a mid-transition reload.
7. Confirm a save/load pair does not duplicate one-shot events or reapply a quest/choice/ledger mutation.

No plan-created “state cache” is allowed. If a new authority is genuinely required, the package must pause for a decision and name its owner, section, migration, and test contract.

## Failure and edge behavior

The implementation must cover null/empty state, empty catalogs, duplicate IDs, missing references, stale old saves, invalid numeric values, extreme but bounded values, unavailable owners, dead or absent participants, repeated commands, simultaneous events, host reload, missing UI, missing audio/journal, new-game reset, teardown, and deterministic replay. The expected result should preserve the last valid state, report a useful diagnostic, and avoid presenting a fabricated success. The detailed failure matrix below expands these cases for 16.

## Test strategy and focused commands

The plan-only pass does not execute tests. The implementing package should reuse existing focused files first, run a new file alone, and stay below the repository’s focused-test policy unless a foreman-approved hypothesis requires more. The current candidate commands are listed in the verification matrix and must be revalidated against the worktree at implementation start. A passing compile is not proof of host wiring, persistence, determinism, or player reachability.

## Phased implementation and rollback

The detailed phase table below is the implementation contract. Each phase has a completion gate and a “must not touch yet” boundary. Rollback is additive and local: disable the adapter or remove the authored tranche, retain the owner’s last valid state, and never reset the shared worktree or shared save registry to hide a failure.

## Dependency-ordered implementation phases
| Phase | Outcome | Work | Boundary | Completion gate |
|---|---|---|---|---|
| 0 | Premise recheck and baseline | Confirm exact owner/API/catalog counts, current claims, dirty paths, and active decision gates. | No edits to production. | A written evidence table and focused baseline commands. |
| 1 | Owner and collision map | Trace current callers, save owner, event seam, and duplicate/legacy candidates. | No new catalog or state. | Single-owner map with zero unresolved authority collisions. |
| 2 | Core contract or bounded extension | Add only the smallest pure contract needed by the confirmed gap, or document that no Core change is needed. | No Godot/UI/data authoring. | Core tests for boundaries, transitions, invalid data, and determinism. |
| 3 | Persistence and migration contract | Implement capture/restore/old-save defaults through the existing owner. | No unrelated save sections. | Round-trip, deep-copy, corrupt/invalid, and mid-event tests are required to pass. |
| 4 | Authored data tranche | Author schema-valid rows only after consumer fields are known; validate references and reachability. | No prose-only orphan rows. | Data integrity and consumer coverage pass for the tranche. |
| 5 | Host/event wiring | Load, subscribe, translate, and dispose in the current host/session owner. | No panel gameplay math. | Host wiring test proves event → projection and setup/teardown. |
| 6 | Presentation and accessibility | Expose truthful state, commands, focus, controller/keyboard behavior, and feedback. | No new authority in UI. | Panel route/focus/headless checks pass; snapshots only through the owning harness. |
| 7 | End-to-end and replay | Run a bounded scenario, save/reload, paired seeded replay, and cross-system consequence check. | No full-suite default. | Named commands/results and limitations recorded. |
| 8 | Balance/content polish | Tune only authored values with current harnesses; remove dead rows and polish truthful text. | No hidden tuning or parallel scalar. | Content review confirms no dominated/unreachable row and no unsupported claim. |
| 9 | Rollback and closeout | Document feature disablement, migration reversal, owner handoff, and residual debt. | No unowned cleanup. | Foreman review accepts or records a blocker. |

### Phase-specific implementation questions
#### Phase 0: Premise recheck and baseline
- What current evidence must be reread? WastelandMapSystem and loader, DamagedMapSystem, WaystationNetworkSystem, TravelingCaravanSystem, CaravanTradeNetworkSystem, RegionalTreatySystem, save stores, host sessions, and UI surfaces are live. The old proposal’s map-size numbers are not accepted without recounting the current catalogs.
- What is the smallest safe change? Make route authoring, reachability, hazard gates, station placement, caravan schedules, treaty effects, and map UI compose through the existing owners while preserving the canonical map-node/location orphan gate.
- Which owner is touched? WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands
- What must remain untouched? No edits to production.
- What proves completion? A written evidence table and focused baseline commands.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 1: Owner and collision map
- What current evidence must be reread? WastelandMapSystem and loader, DamagedMapSystem, WaystationNetworkSystem, TravelingCaravanSystem, CaravanTradeNetworkSystem, RegionalTreatySystem, save stores, host sessions, and UI surfaces are live. The old proposal’s map-size numbers are not accepted without recounting the current catalogs.
- What is the smallest safe change? Make route authoring, reachability, hazard gates, station placement, caravan schedules, treaty effects, and map UI compose through the existing owners while preserving the canonical map-node/location orphan gate.
- Which owner is touched? WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands
- What must remain untouched? No new catalog or state.
- What proves completion? Single-owner map with zero unresolved authority collisions.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 2: Core contract or bounded extension
- What current evidence must be reread? WastelandMapSystem and loader, DamagedMapSystem, WaystationNetworkSystem, TravelingCaravanSystem, CaravanTradeNetworkSystem, RegionalTreatySystem, save stores, host sessions, and UI surfaces are live. The old proposal’s map-size numbers are not accepted without recounting the current catalogs.
- What is the smallest safe change? Make route authoring, reachability, hazard gates, station placement, caravan schedules, treaty effects, and map UI compose through the existing owners while preserving the canonical map-node/location orphan gate.
- Which owner is touched? WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands
- What must remain untouched? No Godot/UI/data authoring.
- What proves completion? Core tests for boundaries, transitions, invalid data, and determinism.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 3: Persistence and migration contract
- What current evidence must be reread? WastelandMapSystem and loader, DamagedMapSystem, WaystationNetworkSystem, TravelingCaravanSystem, CaravanTradeNetworkSystem, RegionalTreatySystem, save stores, host sessions, and UI surfaces are live. The old proposal’s map-size numbers are not accepted without recounting the current catalogs.
- What is the smallest safe change? Make route authoring, reachability, hazard gates, station placement, caravan schedules, treaty effects, and map UI compose through the existing owners while preserving the canonical map-node/location orphan gate.
- Which owner is touched? WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands
- What must remain untouched? No unrelated save sections.
- What proves completion? Round-trip, deep-copy, corrupt/invalid, and mid-event tests are required to pass.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 4: Authored data tranche
- What current evidence must be reread? WastelandMapSystem and loader, DamagedMapSystem, WaystationNetworkSystem, TravelingCaravanSystem, CaravanTradeNetworkSystem, RegionalTreatySystem, save stores, host sessions, and UI surfaces are live. The old proposal’s map-size numbers are not accepted without recounting the current catalogs.
- What is the smallest safe change? Make route authoring, reachability, hazard gates, station placement, caravan schedules, treaty effects, and map UI compose through the existing owners while preserving the canonical map-node/location orphan gate.
- Which owner is touched? WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands
- What must remain untouched? No prose-only orphan rows.
- What proves completion? Data integrity and consumer coverage pass for the tranche.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 5: Host/event wiring
- What current evidence must be reread? WastelandMapSystem and loader, DamagedMapSystem, WaystationNetworkSystem, TravelingCaravanSystem, CaravanTradeNetworkSystem, RegionalTreatySystem, save stores, host sessions, and UI surfaces are live. The old proposal’s map-size numbers are not accepted without recounting the current catalogs.
- What is the smallest safe change? Make route authoring, reachability, hazard gates, station placement, caravan schedules, treaty effects, and map UI compose through the existing owners while preserving the canonical map-node/location orphan gate.
- Which owner is touched? WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands
- What must remain untouched? No panel gameplay math.
- What proves completion? Host wiring test proves event → projection and setup/teardown.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 6: Presentation and accessibility
- What current evidence must be reread? WastelandMapSystem and loader, DamagedMapSystem, WaystationNetworkSystem, TravelingCaravanSystem, CaravanTradeNetworkSystem, RegionalTreatySystem, save stores, host sessions, and UI surfaces are live. The old proposal’s map-size numbers are not accepted without recounting the current catalogs.
- What is the smallest safe change? Make route authoring, reachability, hazard gates, station placement, caravan schedules, treaty effects, and map UI compose through the existing owners while preserving the canonical map-node/location orphan gate.
- Which owner is touched? WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands
- What must remain untouched? No new authority in UI.
- What proves completion? Panel route/focus/headless checks pass; snapshots only through the owning harness.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 7: End-to-end and replay
- What current evidence must be reread? WastelandMapSystem and loader, DamagedMapSystem, WaystationNetworkSystem, TravelingCaravanSystem, CaravanTradeNetworkSystem, RegionalTreatySystem, save stores, host sessions, and UI surfaces are live. The old proposal’s map-size numbers are not accepted without recounting the current catalogs.
- What is the smallest safe change? Make route authoring, reachability, hazard gates, station placement, caravan schedules, treaty effects, and map UI compose through the existing owners while preserving the canonical map-node/location orphan gate.
- Which owner is touched? WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands
- What must remain untouched? No full-suite default.
- What proves completion? Named commands/results and limitations recorded.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 8: Balance/content polish
- What current evidence must be reread? WastelandMapSystem and loader, DamagedMapSystem, WaystationNetworkSystem, TravelingCaravanSystem, CaravanTradeNetworkSystem, RegionalTreatySystem, save stores, host sessions, and UI surfaces are live. The old proposal’s map-size numbers are not accepted without recounting the current catalogs.
- What is the smallest safe change? Make route authoring, reachability, hazard gates, station placement, caravan schedules, treaty effects, and map UI compose through the existing owners while preserving the canonical map-node/location orphan gate.
- Which owner is touched? WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands
- What must remain untouched? No hidden tuning or parallel scalar.
- What proves completion? Content review confirms no dominated/unreachable row and no unsupported claim.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 9: Rollback and closeout
- What current evidence must be reread? WastelandMapSystem and loader, DamagedMapSystem, WaystationNetworkSystem, TravelingCaravanSystem, CaravanTradeNetworkSystem, RegionalTreatySystem, save stores, host sessions, and UI surfaces are live. The old proposal’s map-size numbers are not accepted without recounting the current catalogs.
- What is the smallest safe change? Make route authoring, reachability, hazard gates, station placement, caravan schedules, treaty effects, and map UI compose through the existing owners while preserving the canonical map-node/location orphan gate.
- Which owner is touched? WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands
- What must remain untouched? No unowned cleanup.
- What proves completion? Foreman review accepts or records a blocker.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

## Ownership matrix and file impact map
The following is an impact map for a future implementation package, not a request to edit these paths in this planning-only task.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/World/WastelandMapSystem.cs` — WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/World/WastelandMapCatalogLoader.cs` — WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/World/DamagedMapSystem.cs` — WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/World/DamagedMapCatalog.cs` — WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Waystation/WaystationNetworkSystem.cs` — WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Waystation/WaystationCatalogLoader.cs` — WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/TravelingCaravanSystem.cs` — WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Economy/CaravanTradeNetworkSystem.cs` — WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/RegionalTreatySystem.cs` — WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/RegionalTreatyCatalogLoader.cs` — WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Host/WastelandMapSaveStore.cs` — WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Host/WaystationHostSession.cs` — WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Host/TravelingCaravanHostSession.cs` — WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Host/RegionalTreatyHostSession.cs` — WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Main.Expeditions.cs` — WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands; change only the confirmed owner seam.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/wasteland_map_v1.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/damaged_map_zones.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/caravans.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/caravan_trade_routes.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/waystations.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/diplomatic_treaties.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/treaty_templates.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/faction_territory.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after presentation claim`: `src/UI/MapAtlasPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/WaystationNetworkPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/TravelingCaravanPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/RegionalTreatyPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/SubterraneanCartographyPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/World/Plan16CartographyTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/World/WastelandMapSystemTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/World/WastelandMapRouteValidationTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/WastelandMapPersistenceTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/WaystationSystemTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/TravelingCaravanSystemTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/RegionalTreatySystemTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Economy/CaravanTradeNetworkTests.cs` — focused verification; no test created to mask a stale premise.

## Out of scope
- No unrelated refactor.
- No Unity restoration or dependency.
- No generated index or unrelated documentation regeneration.
- No broad test suite or runtime soak by default.
- No new save owner, registry, selector, or simulation unless a signed architecture decision names it.

## Definition of done
- The current owner and public API are cited from the implementation snapshot.
- wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels is wired end to end or the plan explicitly closes as already integrated.
- Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics.
- Data references, schema version, old-save behavior, focused tests, and rollback are recorded.
- No stale “sealed”, “approved”, or pass-count language is used without current evidence.

## Numbered implementation contract

# 1. Objective

Deliver only the bounded delta described as: Make route authoring, reachability, hazard gates, station placement, caravan schedules, treaty effects, and map UI compose through the existing owners while preserving the canonical map-node/location orphan gate. The objective is measured by current-owner reachability, truthful state, deterministic replay, and a safe implementation handoff rather than by document length.

# 2. Current Reality

WastelandMapSystem and loader, DamagedMapSystem, WaystationNetworkSystem, TravelingCaravanSystem, CaravanTradeNetworkSystem, RegionalTreatySystem, save stores, host sessions, and UI surfaces are live. The old proposal’s map-size numbers are not accepted without recounting the current catalogs.

# 3. Required Delta

Make route authoring, reachability, hazard gates, station placement, caravan schedules, treaty effects, and map UI compose through the existing owners while preserving the canonical map-node/location orphan gate.

# 4. Evidence

Use the current source/data dossier, JSON audit, symbol/caller audit, and read-only authority excerpts in this document. The canonical authority is docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md with SHA-256 911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c; current source/data remain the implementation truth when the authority is descriptive or historical.

# 5. Existing Extension Seams

Primary seam: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels. Before creating anything, verify the current public method, event, host session, save store, and consumer named in the dossier. A new abstraction is justified only when this seam cannot express the confirmed delta.

# 6. Proposed Architecture

Use the owner-first Core → data → host → presentation architecture described above. The proposed architecture is a bounded extension of WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands; it does not create a parallel gameplay system.

# 7. Ownership Matrix

Canonical ownership: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands. JSON owns authored content, Core owns pure rules/state transitions, the host owns lifecycle/adapters, and Godot panels own presentation only. Every proposed write must be assigned to exactly one row of that matrix.

# 8. Data Flow

INPUT (catalog, command, current state) → VALIDATION (owner/schema/reference checks) → CORE STATE → DOMAIN FACT → HOST PROJECTION → UI FEEDBACK → SAVE OWNER. The reverse UI path is a command request, never a direct state mutation.

# 9. State Model

State rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. The implementing agent must document fields, defaults, lifecycle, mutation, reset, persistence, and migration against the actual current DTO before editing.

# 10. API/Contracts

Expose only the current owner’s read query, command/mutation, typed fact/event, capture/restore, diagnostic result, idempotency key, and stable ordering needed for wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels. Confirm signatures from source; never copy historical API names from the old plan.

# 11. Data Changes

Data changes must extend current catalogs under Assets/StreamingAssets/Data/. For each row, validate schema_version, snake_case ID, references, ranges, default behavior, loader, consumer, and observable outcome. The record review ledger applies this rule to Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.

# 12. Save/Load

Persistence must use the current owner identified by the dossier. Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Require capture/restore, deep-copy isolation, old-version defaults, partial/corrupt input behavior, checksum handling, and mid-event reload before claiming persistence.

# 13. Determinism

Determinism contract: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery. Reuse existing seeded RNG forks, ordinal ordering, bounded state, and invariant culture formatting. A compile-green result is not replay evidence.

# 14. System/Event Wiring

Wire the confirmed event or command through wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels. Define event order, daily/hourly triggers, idempotency, subscriber lifetime, missing-owner behavior, and the exact host projection. Do not add a second event authority.

# 15. Godot Integration

Godot integration is limited to the current host/session and named presentation surfaces. UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs'] Preserve focus, controller/keyboard close/back, contrast, reduced motion, refresh, and disposal behavior.

# 16. Narrative/Content Integration

Content must describe only effects expressible by the current owner. Record-level action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable. Validate narrative references, continuity, voice, and player-visible consequence without making prose a hidden gameplay authority.

# 17. Failure Modes

Failure behavior: Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics. The failure matrix covers null/empty state, missing/duplicate references, old/corrupt saves, unavailable owners, dead or hostile participants, repeated commands, simultaneous events, reload, missing UI/audio/journal, and deterministic replay.

# 18. Test Strategy

Use the smallest existing focused test first; run a new test alone; keep save/load, determinism, lifecycle, mutation, fuzzing, state-transition, and cross-system workflows independently reported. The plan-only package does not claim these commands were run.

# 19. Dependency-Ordered Phases

Follow phases 0–9: premise recheck, owner/collision map, Core contract, persistence/migration, data tranche, host/event wiring, presentation/accessibility, end-to-end/replay, balance/polish, and rollback/closeout. Each phase has a completion gate and a must-not-touch boundary above.

# 20. File Impact Map

The future implementation package may modify only the confirmed owner/data/host/UI/test paths listed in the dossier and only after claiming them. This Round 7 planning package intentionally modifies none of those production paths.

# 21. Risks

Primary risk: Route flood topology and per-strike war emission are decision-gated in the authority; this plan must not silently author those contracts. Additional risks are dirty-worktree drift, stale catalog counts, missing host callers, shared save seams, decision-gated authority, accessibility regressions, and false completion claims. Each risk has a stop/escalate rule in the handoff.

# 22. Out of Scope

No unrelated refactor, Unity restoration, new parallel authority, broad test suite, generated-index rewrite, asset production, or opportunistic gameplay tuning is included.

# 23. Rollback Strategy

Rollback is local: disable the adapter, remove only the bounded authored tranche, preserve the owner’s last valid state, and keep the save schema readable. Never reset the shared worktree or hide a failure with a destructive migration.

# 24. Definition of Done

The implementing package is done only when wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels is wired or proven already integrated, current data validates and reaches a consumer, save/determinism/failure/UI contracts pass focused verification, accessibility is truthful, and the handoff records limitations. This document itself remains planning-only.

# 25. Implementation Handoff

MUST PRESERVE the current owner, Godot/Core boundary, JSON authority, save/determinism contracts, and accessibility. MUST ADD only the smallest confirmed extension and focused evidence. MUST NOT invent APIs or claim unrun tests. FIRST SAFE STEP: reread the first current owner/catalog/host/test path and write a live-versus-stale premise table before any implementation edit.

## Detailed record-by-record integration ledger

### Record review 001: `loc_holdfast`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 002: `loc_cut_abandoned_depot`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 003: `loc_cut_radiation_zone_alpha`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 004: `loc_black_flotilla_outpost`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 005: `loc_cut_merchant_caravanserai`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 006: `loc_cut_arsenal_ruin`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 007: `loc_hidden_relay_bunker`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 008: `loc_logistics_reserve_cache`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 009: `loc_broadcast_bunker_echo`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 010: `loc_diesel_tank_farm`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 011: `loc_recovery_yard`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 012: `loc_underground_fuel_depot`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 013: `loc_municipal_seed_vault`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 014: `loc_blacksite_armory_7`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 015: `loc_excavation_command_vault`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 016: `loc_deaddrop_command_shelter`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 017: `loc_sealed_triage_annex`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 018: `loc_evidence_sub_basement`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 019: `loc_quarantine_barn`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 020: `loc_forestry_emergency_store`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 021: `loc_materials_research_sublevel`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 022: `loc_electrical_maintenance_exchange`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 023: `industrial_district`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 024: `suburban_heights`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 025: `military_corridor`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 026: `crater_ground_zero`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 027: `deep_coast_shelf`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 028: `high_scarp_ridgeline`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 029: `old_medical_quarter`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 030: `court_district`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 031: `pasture_valley`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 032: `north_woods`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 033: `university_quarter`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 034: `metro_service_ring`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 035: `faction_the_fleet`
- Source: `Assets/StreamingAssets/Data/caravans.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 036: `faction_rebuilders`
- Source: `Assets/StreamingAssets/Data/caravans.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 037: `faction_silent_foundry`
- Source: `Assets/StreamingAssets/Data/caravans.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 038: `faction_the_scale`
- Source: `Assets/StreamingAssets/Data/caravans.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 039: `route_compact_supply_line`
- Source: `Assets/StreamingAssets/Data/caravan_trade_routes.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 040: `route_compact_iron_corridor`
- Source: `Assets/StreamingAssets/Data/caravan_trade_routes.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 041: `route_compact_grain_express`
- Source: `Assets/StreamingAssets/Data/caravan_trade_routes.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 042: `route_scale_salt_artery`
- Source: `Assets/StreamingAssets/Data/caravan_trade_routes.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 043: `route_scale_mercantile_circuit`
- Source: `Assets/StreamingAssets/Data/caravan_trade_routes.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 044: `route_scale_chemical_exchange`
- Source: `Assets/StreamingAssets/Data/caravan_trade_routes.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 045: `route_flotilla_coastal_drift`
- Source: `Assets/StreamingAssets/Data/caravan_trade_routes.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 046: `route_flotilla_salvage_run`
- Source: `Assets/StreamingAssets/Data/caravan_trade_routes.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 047: `route_flotilla_smugglers_deep`
- Source: `Assets/StreamingAssets/Data/caravan_trade_routes.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 048: `route_compact_frontier_convoys`
- Source: `Assets/StreamingAssets/Data/caravan_trade_routes.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 049: `waystation_alpha_cut`
- Source: `Assets/StreamingAssets/Data/waystations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 050: `waystation_switchback`
- Source: `Assets/StreamingAssets/Data/waystations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 051: `waystation_span44`
- Source: `Assets/StreamingAssets/Data/waystations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 052: `waystation_verity`
- Source: `Assets/StreamingAssets/Data/waystations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 053: `waystation_coast_lock`
- Source: `Assets/StreamingAssets/Data/waystations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 054: `waystation_grain_verge`
- Source: `Assets/StreamingAssets/Data/waystations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 055: `waystation_south_beacon`
- Source: `Assets/StreamingAssets/Data/waystations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 056: `waystation_tinkers_notch`
- Source: `Assets/StreamingAssets/Data/waystations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 057: `waystation_brine_pans`
- Source: `Assets/StreamingAssets/Data/waystations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 058: `waystation_slate_hollow`
- Source: `Assets/StreamingAssets/Data/waystations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 059: `waystation_iron_siding`
- Source: `Assets/StreamingAssets/Data/waystations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 060: `waystation_weighbridge`
- Source: `Assets/StreamingAssets/Data/waystations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 061: `waystation_pilgrim_switchbacks`
- Source: `Assets/StreamingAssets/Data/waystations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 062: `waystation_st_brigids`
- Source: `Assets/StreamingAssets/Data/waystations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 063: `clean_water`
- Source: `Assets/StreamingAssets/Data/diplomatic_treaties.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 064: `mechanical_parts`
- Source: `Assets/StreamingAssets/Data/diplomatic_treaties.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 065: `fuel`
- Source: `Assets/StreamingAssets/Data/diplomatic_treaties.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 066: `bandage`
- Source: `Assets/StreamingAssets/Data/diplomatic_treaties.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 067: `scrap_metal`
- Source: `Assets/StreamingAssets/Data/diplomatic_treaties.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 068: `battery`
- Source: `Assets/StreamingAssets/Data/diplomatic_treaties.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 069: `item_preservation_salt`
- Source: `Assets/StreamingAssets/Data/diplomatic_treaties.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 070: `scrap_electronic`
- Source: `Assets/StreamingAssets/Data/diplomatic_treaties.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 071: `copper_wire_10m_of_10m`
- Source: `Assets/StreamingAssets/Data/diplomatic_treaties.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 072: `territory_the_office`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 073: `territory_the_cutters`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 074: `territory_black_flotilla`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 075: `territory_the_fleet`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 076: `territory_deserter_coalition`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 077: `territory_cold_count`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 078: `territory_the_tally`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 079: `territory_grain_exchange`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 080: `territory_quiet_house`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 081: `territory_scavenger_guild`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 082: `territory_long_walk`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 083: `territory_undertow`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 084: `territory_hydro_barons`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 085: `territory_iron_raiders`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 086: `territory_the_provisioned`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 087: `territory_archivists`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 088: `territory_lamplighters`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 089: `territory_sun_seekers`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 090: `territory_osteophages`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 091: `zone_contested_water_rights`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 092: `zone_contested_cut_salvage`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 093: `zone_contested_merchant_crossroads`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 094: `zone_contested_scarp_pass`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 095: `zone_contested_coastal_bluff`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate every map, route, station, caravan, and treaty record against the canonical location/item/faction references and identify the single consumer that can make its promised effect observable.
- Primary owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State/save rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

## Precision scenario matrix
Each row is a future implementation checkpoint, not a claim that the current repository already passes it.
### Scenario 01: fresh campaign before the owner is initialized
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 02: old save restored at day zero
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 03: old save restored after a partial event
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 04: catalog unavailable at startup
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 05: catalog contains an empty collection
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 06: duplicate canonical ID
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 07: reference points to a missing item
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 08: reference points to a missing location
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 09: reference points to a missing faction
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 10: unavailable optional owner
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 11: owner disabled by difficulty or policy
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 12: unpowered infrastructure
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 13: zero resources
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 14: negative or malformed numeric input
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 15: large but bounded collection
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 16: dead survivor or unavailable participant
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 17: hostile faction state
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 18: repeated player command
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 19: simultaneous day events
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 20: mid-transition save
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 21: reload after event dispatch
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 22: missing UI surface
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 23: stale presentation cache
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 24: missing audio cue
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 25: missing journal owner
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 26: missing save owner
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 27: corrupt save payload
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 28: checksum mismatch
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 29: RNG fork unavailable
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 30: unordered dictionary iteration
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 31: clock boundary at midnight
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 32: seasonal boundary
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 33: weather gate closure
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 34: route closure
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 35: trade or treaty conflict
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 36: choice already resolved
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 37: ending owner unavailable
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 38: optional content absent
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 39: mod or compatibility row absent
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 40: concurrent package changes source
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 41: headless session without UI
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 42: snapshot fixture unavailable
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 43: controller/keyboard focus path
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 44: screen reader/high contrast path
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 45: asset/resource fallback
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 46: final archive projection
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 47: new-game reset
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 48: legacy content migration
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 49: consumer not wired
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 50: host setup order reversed
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 51: teardown/disposal
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 52: replay after reload
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 53: telemetry/diagnostic emission
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 54: authority conflict discovered
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 55: future implementation acceptance boundary
- Domain lens: map graph, waystations, caravans, and treaties.
- Seam under test: wasteland_map_v1.json -> WastelandMapCatalogLoader/WastelandMapSystem -> route/weather gates and expedition consumers -> waystation/caravan/treaty projections -> map/caravan/treaty panels.
- Expected authority: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority. Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

## Failure and rejection matrix
The failure contract is intentionally strict: an unavailable feature is preferable to a convincing but unauthoritative simulation.
### Failure 01: null state
- Detection: accept an explicit empty/default state only where the owner contract permits it.
- Owner response: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Player-facing truth: Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics.
- Persistence response: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism response: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 02: empty catalog
- Detection: report a data-integrity gap and keep the previous safe projection.
- Owner response: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Player-facing truth: Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics.
- Persistence response: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism response: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 03: duplicate ID
- Detection: reject the row with a stable diagnostic rather than last-write-wins.
- Owner response: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Player-facing truth: Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics.
- Persistence response: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism response: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 04: missing reference
- Detection: do not create a phantom entity; expose the unresolved dependency.
- Owner response: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Player-facing truth: Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics.
- Persistence response: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism response: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 05: stale save
- Detection: migrate or default only through the owner’s versioned restore path.
- Owner response: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Player-facing truth: Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics.
- Persistence response: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism response: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 06: corrupt checksum
- Detection: refuse the corrupted section and preserve unrelated valid sections.
- Owner response: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Player-facing truth: Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics.
- Persistence response: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism response: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 07: host reload
- Detection: rehydrate through the same owner and event registration path.
- Owner response: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Player-facing truth: Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics.
- Persistence response: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism response: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 08: new game
- Detection: clear transient host state and initialize owner defaults exactly once.
- Owner response: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Player-facing truth: Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics.
- Persistence response: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism response: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 09: deterministic replay
- Detection: same seed, day, catalog, and state must produce the same fact/order.
- Owner response: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Player-facing truth: Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics.
- Persistence response: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism response: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 10: UI unavailable
- Detection: retain canonical state and defer presentation without re-running mutation.
- Owner response: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Player-facing truth: Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics.
- Persistence response: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism response: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 11: disposal
- Detection: unregister listeners and release host resources deterministically.
- Owner response: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Player-facing truth: Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics.
- Persistence response: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism response: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 12: concurrent claim
- Detection: stop and hand off rather than editing a shared seam.
- Owner response: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Player-facing truth: Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics.
- Persistence response: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism response: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 13: authority conflict
- Detection: name the conflict and defer the architectural decision.
- Owner response: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Player-facing truth: Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics.
- Persistence response: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism response: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 14: missing test
- Detection: do not claim integration; add the focused contract test in the implementing package.
- Owner response: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Player-facing truth: Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics.
- Persistence response: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism response: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 15: unsupported API
- Detection: use the current public API or mark the premise stale.
- Owner response: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Player-facing truth: Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics.
- Persistence response: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism response: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 16: legacy Unity reference
- Detection: do not restore it; port only through the Godot/Core boundary.
- Owner response: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Player-facing truth: Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics.
- Persistence response: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism response: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 17: engine reference in Core
- Detection: reject the change and move only presentation adaptation to src/.
- Owner response: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Player-facing truth: Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics.
- Persistence response: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism response: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 18: unowned save state
- Detection: reject the change until the owner and migration path are explicit.
- Owner response: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Player-facing truth: Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics.
- Persistence response: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism response: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 19: UI-owned gameplay
- Detection: reject the panel mutation and route the command to Core.
- Owner response: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Player-facing truth: Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics.
- Persistence response: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism response: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 20: false completion claim
- Detection: downgrade the handoff to planning/static evidence only.
- Owner response: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- Player-facing truth: Orphan map nodes, missing location IDs, disconnected graphs, stale route gates, missing station references, treaty conflicts, and absent caravan owners fail closed with actionable diagnostics.
- Persistence response: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism response: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

## Layered focused verification matrix
Commands are exact paths only where the current test file exists. They are future implementation gates for this plan-only package.
### Verification layer 01: Core unit
- Coverage: valid input, boundary, missing optional field, duplicate/unknown reference, state invariant.
- Domain contract: WastelandMapSystem.
- Owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 02: Core transition
- Coverage: one legal transition, one illegal transition, repeated transition, cancellation/rollback.
- Domain contract: WastelandMapCatalogLoader.
- Owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 03: save round-trip
- Coverage: capture, serialize, restore, deep-copy isolation, old version/defaults.
- Domain contract: DamagedMapSystem.
- Owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 04: data integrity
- Coverage: schema_version, snake_case IDs, duplicate IDs, references, ranges, collection shape.
- Domain contract: WaystationNetworkSystem.
- Owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 05: host wiring
- Coverage: setup, command dispatch, event subscription, refresh, disposal, missing owner.
- Domain contract: TravelingCaravanSystem.
- Owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 06: UI projection
- Coverage: truthful current state, disabled action, focus order, controller/keyboard close/back.
- Domain contract: RegionalTreatySystem.
- Owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 07: headless/selftest
- Coverage: bounded deterministic scenario and diagnostics without a renderer.
- Domain contract: AllMapNodes_ExistInLocationsCatalog.
- Owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 08: cross-system
- Coverage: owner event to consumer, ordering, idempotency, no parallel state.
- Domain contract: WastelandMapSystem.
- Owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 09: replay
- Coverage: same seed/day/input produces same state hash and fact order.
- Domain contract: WastelandMapCatalogLoader.
- Owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 10: content utilization
- Coverage: authored record has a current loader, consumer, and observable outcome.
- Domain contract: DamagedMapSystem.
- Owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 11: migration
- Coverage: old save and current catalog remain readable or fail with a named reason.
- Domain contract: WaystationNetworkSystem.
- Owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 12: rollback
- Coverage: feature flag/owner boundary permits disabling the delta without data loss.
- Domain contract: TravelingCaravanSystem.
- Owner: WastelandMapSystem owns graph state; waystation, caravan, treaty, weather, and expedition systems retain their own state; host coordinates reads and commands.
- State rule: Discovered/unlocked map state uses WastelandMapState; waystation/caravan/treaty state uses their existing save stores. No map-panel cache becomes authority.
- Determinism rule: Graph traversal, route choice, caravan timetables, and treaty effects use canonical ordering and existing seeded RNG forks; repeated route scans are bounded and do not mutate discovery.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/WaystationNetworkPanel.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/RegionalTreatyPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

#### Current focused command 01
- Test: `Ashfall.Core.Tests/World/Plan16CartographyTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan16CartographyTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 02
- Test: `Ashfall.Core.Tests/World/WastelandMapSystemTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/World/WastelandMapSystemTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 03
- Test: `Ashfall.Core.Tests/World/WastelandMapRouteValidationTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/World/WastelandMapRouteValidationTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 04
- Test: `Ashfall.Core.Tests/WastelandMapPersistenceTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/WastelandMapPersistenceTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 05
- Test: `Ashfall.Core.Tests/WaystationSystemTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/WaystationSystemTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 06
- Test: `Ashfall.Core.Tests/TravelingCaravanSystemTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/TravelingCaravanSystemTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 07
- Test: `Ashfall.Core.Tests/RegionalTreatySystemTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/RegionalTreatySystemTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 08
- Test: `Ashfall.Core.Tests/Economy/CaravanTradeNetworkTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/CaravanTradeNetworkTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

> **Structured body length before post-250K polishing:** 399,327 characters.
# Post-250K deep polishing pass

The architecture body above reached 399,405 characters before this pass. This is a second deep polishing phase, not a license to pad. Re-read the plan as an implementer and tighten it against the current source/data snapshot.

## Deep polish A — authority and collision recheck

- Re-run the owner/caller audit for: `WastelandMapSystem`, `WastelandMapCatalogLoader`, `DamagedMapSystem`, `WaystationNetworkSystem`, `TravelingCaravanSystem`, `RegionalTreatySystem`, `AllMapNodes_ExistInLocationsCatalog`.
- Confirm each proposed mutation has exactly one writer. A host adapter may translate a fact; a panel may display it; neither becomes authority.
- Check for legacy or parallel names before proposing any new class, DTO, event, catalog, save section, or RNG stream.
- Treat the master authority as a read-only design lens. Current source/data wins when they disagree, and the disagreement is recorded rather than hidden.
- Recheck the live worktree status recorded in each evidence dossier. A dirty source path is a coordination warning, not a stable acceptance result.

## Deep polish B — data and consumer precision

- For every candidate row, name the loader, consumer, reference validator, and observable outcome.
- Replace counts copied from the old plan with current JSON counts or an explicit census task.
- Remove any row whose only consumer is a test, a prose generator, or a panel.
- Preserve additive schema compatibility and explain old-data defaults.
- Check that narrative text does not promise an effect that the current owner cannot produce.

## Deep polish C — state, save, and replay precision

- Confirm the existing save owner and DTO before naming a field.
- Test capture/restore, deep-copy isolation, old-version defaults, partial/corrupt input, and mid-event reload.
- Confirm repeated commands and replay cannot duplicate one-shot effects.
- Use the existing seeded stream/fork; document every random draw and tie-break.
- Treat a UI refresh as a projection, never as persistence or mutation.

## Deep polish D — failure and player truth

- Walk null, empty, missing, duplicate, stale, hostile, dead, unavailable, and repeated-event cases.
- Ensure blocked/unavailable states are legible and do not masquerade as success.
- Keep accessibility behavior attached to the same command/state surface as the visual behavior.
- Record which failures are diagnostic-only, which defer presentation, and which halt the transition.

## Deep polish E — implementation readiness

- Replace generic file lists with owner-specific action verbs and completion gates.
- Keep the phase order dependency-safe: premise, owner, Core, persistence, data, host, presentation, end-to-end, polish, closeout.
- Give the next implementer exact focused commands, test selection rationale, and stop conditions.
- Keep a final residual-risk list for decision-gated or concurrent work rather than hiding it in prose.

# Post-250K deep polishing pass — second pass

The first post-250K pass above checked structure and owner collisions. This second pass is a separate adversarial review after the plan has reached its depth checkpoint; it is not a duplicate paragraph exercise.

## Second-pass adversarial questions

- What would a reviewer incorrectly assume after reading only the executive summary?
- Which sentence describes historical intent but could be mistaken for current behavior?
- Which current owner, save path, host session, or event seam is missing from the impact map?
- Which data row has a valid ID but no reachable consumer?
- Which UI label could claim a consequence before the Core transition succeeds?
- Which failure currently falls through as a default success, duplicate event, or stale cache?
- Which random choice lacks a named seed/fork/tie-break?
- Which old-save field lacks a default, migration, or deep-copy test?
- Which concurrent package or decision gate could invalidate the proposed path?
- What is the smallest rollback that leaves the previous owner state readable?

## Second-pass correction protocol

For every answer, classify the issue as `CURRENT_EVIDENCE`, `PROPOSED_EXTENSION`, `DECISION_GATE`, `CONCURRENT_CLAIM`, or `OUT_OF_SCOPE`. Update the relevant numbered section and record the exact path/command that would close the issue. If no current evidence supports a claim, remove the claim rather than softening it with adjectives. If a proposed change needs a new architecture decision, stop at the gate and name the decision owner. This pass must leave the plan more precise, not merely longer.

## Second-pass acceptance

- [ ] Every “current” statement has a path or is marked as an open premise.
- [ ] Every “implemented” statement names a current caller or is downgraded to catalog-only.
- [ ] Every proposed state field names its save owner and migration behavior.
- [ ] Every proposed random/temporal behavior names determinism treatment.
- [ ] Every UI consequence has a command/state source.
- [ ] Every failure has a safe expected outcome.
- [ ] Every focused command resolves or is labeled future implementation work.
- [ ] Every decision-gated or concurrently claimed seam is explicit.

# Final precision and reaccuracy pass

1. Re-read every current path cited in the dossier and mark missing paths as open premises.
2. Re-run the hash verifier and data JSON parse check at the final snapshot.
3. Re-run the symbol occurrence audit and distinguish declarations, host consumers, and test-only references.
4. Check all current focused test commands resolve to existing files.
5. Remove unsupported historical counts, “sealed” claims, invented APIs, and unproven caller assertions.
6. Reconcile the plan title and requested delta with the actual current owner; if the old plan is already implemented or stale, state maintenance or blocked scope plainly.
7. Confirm Core remains engine-free, JSON remains authoritative, and no parallel authority is proposed.
8. Confirm UI, failure, save, determinism, and accessibility contracts are concrete.
9. Confirm rollback is local and does not require destructive state reset.
10. Record limitations honestly: this package is planning-only and does not run implementation tests.

# Full repolishing phase

The final repolishing pass is a quality audit over the complete document, not an append-only slogan. Read the plan from executive summary through handoff as one artifact.

- **Coherence:** every section uses the same owner names, state terms, and delta.
- **Evidence:** every important claim points to a current path, current data record, read-only authority anchor, or explicit unknown.
- **Architecture:** no duplicate system, parallel save, second RNG, engine dependency, or UI authority is smuggled into a phase.
- **Mechanics:** effects are expressed through existing commands/events and have a visible or auditable consequence.
- **Continuity:** references, days, locations, factions, and narrative facts use canonical IDs and current catalogs.
- **Failure:** invalid and unavailable states are defined, bounded, and truthful.
- **Persistence:** capture/restore and migration are named for every durable delta; no new section is casually proposed.
- **Determinism:** random sources, ordering, and replay comparisons are explicit.
- **UI:** panels are thin, accessible, refreshable, and non-authoritative.
- **Testing:** each layer has a focused command or a clear reason it must be authored later.
- **Rollback:** every phase has a local reversal and preservation rule.
- **Handoff:** the next implementer can start with the first safe step without interpreting the plan around stale prose.

## Repolish acceptance checklist

- [ ] Current owner/API confirmed from source.
- [ ] Current data schema and references confirmed.
- [ ] Existing save owner and migration path confirmed.
- [ ] Existing host/event seam confirmed.
- [ ] Existing UI surface and accessibility path confirmed.
- [ ] Focused test paths resolve or are labeled future work.
- [ ] Deterministic replay and idempotency contract stated.
- [ ] Failure and old-save behavior stated.
- [ ] No unsupported completion claim remains.
- [ ] Rollback and owner handoff are actionable.
- [ ] Plan remains implementation-ready without production edits in this package.

# Implementation handoff

## MUST PRESERVE

- The current owner named in this plan and the one-authority rule.
- Godot as the presentation/host authority and Core as engine-free domain logic.
- Authoritative JSON under `Assets/StreamingAssets/Data/`.
- Existing save ownership, migration semantics, seeded RNG contracts, and event ordering.
- Existing accessibility, focus, controller/keyboard close/back, and truthful UI behavior.

## MUST ADD

- Only the smallest confirmed Core/data/host/UI extension needed by the current delta.
- Exact catalog validation, consumer reachability, save/restore, failure, determinism, and focused tests.
- A bounded content tranche whose records are consumed and observable.
- A local rollback/disablement path and a concise implementation handoff.

## MUST NOT DO

- Do not create a second ledger, save store, selector, event authority, simulation, or panel-owned rule.
- Do not edit production, data, tests, UI, assets, or generated indexes during this planning package.
- Do not restore Unity or add engine references to Core.
- Do not claim tests, host wiring, save integration, or player reachability from static intent alone.
- Do not use unseeded randomness, wall-clock ordering, or hash iteration order for deterministic behavior.

## VERIFY WITH

- The exact focused `scripts/run_test.sh` commands listed in this plan after the implementation claim is opened.
- Current catalog integrity and consumer/utilization checks.
- A bounded Core test, save round-trip/migration test, host wiring test, and deterministic replay comparison.
- Godot headless/UI checks only when the confirmed implementation touches the host/UI path.

## FIRST SAFE IMPLEMENTATION STEP

Re-read the current owner, the first current catalog, the first current host/session, and the first focused test listed in the dossier. Write a one-page premise table that classifies each as live, partial, stale, or blocked. Do not author content or code until the table has one owner and one non-overlapping implementation seam.
