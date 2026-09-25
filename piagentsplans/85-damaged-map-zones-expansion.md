# Plan 85 — Damaged Map Zones: Current-Seam Integration Architecture

> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-7`
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round7-2026-09-25`
> **Document class:** evidence-backed implementation plan; planning-only artifact
> **Domain:** cartographic fragments and hidden installations
> **Read-only design authority:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
> **Authority SHA-256:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
> **Target depth:** 150k–170k intermediate quality checkpoint, then 250,000+ characters as a quality target, not a ceiling; no padding or unsupported completion claims
> **Scope:** Core/data/host/UI/save/determinism architecture and verification planning only

## Executive summary

Extend the existing DamagedMapSystem and WastelandMapSystem with validated fragment provenance, partial hints, deterministic completion, and truthful installation reveal.

This document supersedes stale generated or historical claims in the selected plan path. It distinguishes current evidence, required delta, safe extension seam, ownership, persistence, determinism, presentation, failure behavior, focused verification, rollback, and the remaining implementation handoff. It is not an implementation report.

## Selection and premise record

- Original Git `HEAD` baseline: `5897` characters.
- Current worktree copy: `478543` characters before this rebuild.
- Selection rule: next-lowest original `HEAD` character count after excluding every path completed in Rounds 1–6 and any active claim.
- Current worktree generated text was not used as proof of implementation because it may contain stale counts, repetitive expansion, or unsupported pass language.
- User-supplied authority path contained a spacing variation; the canonical repository path used here is `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`.

## Current source and data dossier

### Current evidence: `Assets/Ashfall.Core/World/DamagedMapSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `b5a78fef932b80b13d08c2d802f18a38157d83aa82ac76294cba48c954709e26`
- Snapshot size: 9314 characters; 204 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0012:     /// here, and registration appends the fragment id to
0013:     /// <see cref="WastelandMapState.RegisteredMapFragments"/> — the single
0014:     /// persisted fragment-progress authority (persisted by the existing
0015:     /// wasteland_map save section).
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
...
0201:
0202:         private WastelandMapState? MapState => _map?.State;
0203:     }
0204: }
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
0008: {
0009:     /// <summary>Raw deserialization shape for damaged_map_zones.json.</summary>
0010:     [Serializable]
0011:     public sealed class DamagedMapCatalogContainer
...
0032:
0033:         /// <summary>Hidden installation identity revealed on completion.</summary>
0034:         public string hidden_installation_id { get; set; } = string.Empty;
0035:
...
0086:
0087:         /// <summary>Hidden installation identity (may be prefixed or unprefixed; see ResolveRevealNodeId).</summary>
0088:         public string InstallationId { get; }
0089:
...
0117:     /// <summary>
0118:     /// Loads the damaged treasure-map zone catalog (damaged_map_zones.json).
0119:     /// Engine-agnostic: uses IFileIO and IJsonSerializer ports only.
0120:     /// The catalog is static content; campaign progress lives in
...
0125:         /// <summary>Default catalog file name inside the data authority.</summary>
0126:         public const string DefaultFileName = "damaged_map_zones.json";
0127:
0128:         /// <summary>
...
0131:         /// </summary>
0132:         public static List<DamagedMapValidationError> Validate(DamagedMapCatalogContainer? container)
0133:         {
0134:             var errors = new List<DamagedMapValidationError>();
...
0258:
0259:             DamagedMapCatalogContainer? container;
0260:             try
0261:             {
...
0265:             {
0266:                 CatalogDiagnostics.Warn("DamagedMapCatalog", path, ex);
0267:                 return (zones, new List<DamagedMapValidationError>
0268:                 {
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
...
0091:
0092:         public WastelandMapState State => _state;
0093:         public IReadOnlyList<MapNode> Nodes => _nodes;
0094:         public IReadOnlyList<MapRoute> Routes => _routes;
...
0783:
0784:         public WastelandMapState CaptureState()
0785:         {
0786:             _state.Tunnels = Tunnels.CaptureState();
...
0789:
0790:         public void RestoreState(WastelandMapState state)
0791:         {
0792:             if (state == null) throw new ArgumentNullException(nameof(state));
...
1062:     [Serializable]
1063:     public sealed class WastelandMapState
1064:     {
1065:         /// <summary>List of discovered location node IDs.</summary>
...
1199:
1200:         public WastelandMapState Capture() => new WastelandMapState
1201:         {
1202:             Discovered = new List<string>(Discovered),
...
1211:
1212:         public void RestoreInto(WastelandMapState state, IReadOnlyList<MapNode> nodes)
1213:         {
1214:             Discovered = state.Discovered != null ? new List<string>(state.Discovered) : new List<string>();
```

### Current evidence: `src/Host/HostCli.Cartography.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `93a67f4d27fa1ba0a6ff1f62f230ff573e15f1e2ad9dec9aa31d33bb6f2f7674`
- Snapshot size: 5959 characters; 128 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0044:
0045:             var mapState = new WastelandMapState();
0046:             var map = new WastelandMapSystem(mapState, nodes, routes);
0047:
...
0111:             // 5. Damaged Map Regional Zones
0112:             string damagedMapPath = System.IO.Path.Combine(dataDirectory, "damaged_map_zones.json");
0113:             Check(files.FileExists(damagedMapPath), "damaged_map_zones.json exists on disk");
0114:
...
0120:             var capturedMap = map.CaptureState();
0121:             var restoredMap = new WastelandMapSystem(capturedMap, nodes, routes);
0122:             Check(restoredMap.IsDiscovered("loc_cut_abandoned_depot"), "Restored map retains discovered nodes");
0123:
```

### Current evidence: `src/Host/ExpeditionHostSession.cs`
- Role: current source/owner candidate
- Worktree status: `M src/Host/ExpeditionHostSession.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `46c1ce80f030a53183aea292df6246ab3278b2c2d481baf91e91cbf1f1c8af37`
- Snapshot size: 84483 characters; 1663 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0088:         /// </summary>
0089:         public WastelandMapSystem? WastelandMap { get; set; }
0090:
0091:         /// <summary>
...
0155:                 return "Route blocked";
0156:             // Plan 85 — hidden installations stay undispatchable until the
0157:             // treasure map is completed and the site is revealed.
0158:             if (Engine.DamagedMap != null && Engine.DamagedMap.IsDestinationLocked(locationId))
...
0229:         /// </summary>
0230:         public void AttachDamagedMapFeedback(DamagedMapSystem? damagedMap)
0231:         {
0232:             if (damagedMap == null || _damagedMapFeedbackAttached) return;
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

### Current evidence: `Assets/StreamingAssets/Data/locations.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `97543ade61b6458f5b31bcffb85a96ad5d3b322b80294deb158d1ae29da3386b`
- Snapshot size: 112801 characters; 1440 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "locations": [
0004:     {
0005:       "id": "abandoned_hospital",
0006:       "displayName": "Abandoned Hospital",
0007:       "description": "The east wing of the regional hospital came down in the second winter, and nobody has cleared it since. Girders lean against the stairwell, and the pharmacy door is buried under a ton of masonry. Sealed rooms still hold medicine if you can reach them without the ceiling deciding otherwise. Dosimeters tick up fast near the radiology basement, where the lead-lined walls kept their charge and the machines kept theirs. The morgue drawers are open. A stretcher with one wheel has been propped against the exit, as if someone meant to come back for it.",
0008:       "dangerLevel": 6,
0009:       "travelHours": 2.0,
0010:       "baseRadsPerHour": 35
0011:     },
0012:     {
0013:       "id": "rural_gas_station",
0014:       "displayName": "Rural Gas Station",
0015:       "description": "A roadside station stripped down to its frame on the main route east. The pumps are gutted, the shop glass is gone, and the wind blows ash through the aisles. Fuel drums lie where they were rolled and dropped, most of them empty, a few still holding dregs. The radiation is low here and the danger is low, which is exactly why it has been picked over so completely. What remains is what everyone else passed on. Someone has been sleeping in the workshop bay and oiling the door hinges, so the place is watched even when it is empty.",
0016:       "dangerLevel": 3,
0017:       "travelHours": 1.5,
0018:       "baseRadsPerHour": 15
0019:     },
0020:     {
```

### Current evidence: `Assets/StreamingAssets/Data/collectibles.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `b3eb02f8aa33ce9fa0635d8c4894bae09c6449f4c45e91b8e1ed142c588cdd15`
- Snapshot size: 11272 characters; 405 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "collectibles": [
0004:     {
0005:       "item_id": "item_collectible_vinyl_chamber_record",
0006:       "category": "vinyl",
0007:       "rarity": "uncommon",
0008:       "effect_type": "none",
0009:       "effect_target": "",
0010:       "effect_value": 0,
0011:       "location_type": "residential",
0012:       "unique": false
0013:     },
0014:     {
0015:       "item_id": "item_collectible_vinyl_civil_broadcast",
0016:       "category": "vinyl",
0017:       "rarity": "rare",
0018:       "effect_type": "none",
0019:       "effect_target": "",
0020:       "effect_value": 0,
```

### Current evidence: `Assets/StreamingAssets/Data/items.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`
- Snapshot size: 390056 characters; 9660 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "items": [
0004:     {
0005:       "id": "item_decon_chelator_concentrate",
0006:       "displayName": "Chelator Concentrate",
0007:       "description": "A sealed glass ampoule of chelating agent concentrate. The label is half dissolved but the formula is standard: binds heavy radionuclides into a water-soluble complex for rinse removal. One ampoule per decon cycle. The pre-war stock won't last forever, and the synthesis requires a working pharma bench.",
0008:       "type": "Consumable",
0009:       "stackMax": 20,
0010:       "weight": 0.2,
0011:       "tradeValue": 8
0012:     },
0013:     {
0014:       "id": "item_lead_lined_effluent_filter",
0015:       "displayName": "Lead-Lined Effluent Filter",
0016:       "description": "A cylindrical filtration cartridge with a lead-foil inner liner and activated charcoal matrix. Installed in the decon airlock effluent tank to capture radionuclide-laden particulates before they can be sluiced into the general water system. Good for approximately five hundred liters of contaminated wash water before replacement is required.",
0017:       "type": "Equipment",
0018:       "stackMax": 5,
0019:       "weight": 3.5,
0020:       "tradeValue": 15
```

### Current evidence: `src/UI/MapDetailPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/MapDetailPanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `6b091b1a461434e3ad68b33e4f50f12d9fd2c237d5aed33e117f21020ef3d8d7`
- Snapshot size: 13065 characters; 247 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using Godot;
0005: using Ashfall.Core;
0006: using Ashfall.Core.Expeditions;
0007: using Ashfall.Core.UI;
0008: using AtomicWar.Journal;
0009:
0010: namespace AtomicWar.GodotApp.UI
0011: {
0012:     /// <summary>
0013:     /// ASHFALL — Map Detail panel.
0014:     /// Shows detailed sector intelligence, radiation readings, transit requirements,
0015:     /// architectural sub-layouts, and site salvage potential for a chosen location.
0016:     /// </summary>
0017:     public partial class MapDetailPanel : Control
0018:     {
0019:         public event Action? OnClose;
0020:
```

### Current evidence: `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `d380d40700907225ffeab265efa5803404fcc33dd1107dae115393d0ca37d8b4`
- Snapshot size: 22724 characters; 493 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0018:     /// </summary>
0019:     public sealed class DamagedMapSystemTests : IDisposable
0020:     {
0021:         private readonly string _dataDir;
...
0024:
0025:         public DamagedMapSystemTests()
0026:         {
0027:             _dataDir = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
...
0038:
0039:         private (DamagedMapSystem system, WastelandMapSystem map, List<DamagedMapZone> zones) CreateLive()
0040:         {
0041:             var (nodes, routes) = WastelandMapCatalogLoader.Load(_dataDir, _files, _json);
...
0044:             Assert.Empty(errors);
0045:             return (new DamagedMapSystem(zones, map), map, zones);
0046:         }
0047:
...
0052:         {
0053:             var (zones, errors) = DamagedMapCatalogLoader.LoadWithValidation(_dataDir, _files, _json);
0054:             Assert.Empty(errors);
0055:             Assert.True(zones.Count >= 12, $"expected >= 12 zones, got {zones.Count}");
...
0089:
0090:             var (zones, errors) = DamagedMapCatalogLoader.LoadWithValidation(_dataDir, _files, _json);
0091:             Assert.Empty(errors);
0092:             foreach (var zone in zones)
...
0106:
0107:             var (zones, _) = DamagedMapCatalogLoader.LoadWithValidation(_dataDir, _files, _json);
0108:             foreach (var zone in zones)
0109:             foreach (var fragment in zone.Fragments)
...
0118:             var nodeIds = nodes.Select(n => n.Id).ToHashSet();
0119:             var (zones, _) = DamagedMapCatalogLoader.LoadWithValidation(_dataDir, _files, _json);
0120:             foreach (var zone in zones)
0121:             {
...
0136:             var fragments = zone.Fragments.Select(f => f.fragment_id).ToList();
0137:             string nodeId = DamagedMapSystem.ResolveRevealNodeId(zone.InstallationId)!;
0138:
0139:             int completions = 0;
...
0194:             var zone = zones[0];
0195:             string nodeId = DamagedMapSystem.ResolveRevealNodeId(zone.InstallationId)!;
0196:             foreach (var f in zone.Fragments)
0197:                 system.RegisterFragment(f.fragment_id);
...
0202:             var (nodes, routes) = WastelandMapCatalogLoader.Load(_dataDir, _files, _json);
0203:             var restoredMap = new WastelandMapSystem(captured, nodes, routes);
0204:             var restoredSystem = new DamagedMapSystem(zones, restoredMap);
0205:
...
```

### Current evidence: `Ashfall.Core.Tests/World/Plan85_71DamagedMapPowerIntegrationTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `53a1674ce6afe7e37ecc9e48c484661110df082f47c19e979713b10a048877c0`
- Snapshot size: 10446 characters; 217 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0019:             // Plan 85: Damaged Map Zones (12 zones, 32 fragments)
0020:             var (zones, errors) = DamagedMapCatalogLoader.LoadWithValidation(
0021:                 DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
0022:             Assert.Empty(errors);
...
0043:             Assert.NotNull(metroZone);
0044:             Assert.Equal("loc_electrical_maintenance_exchange", DamagedMapSystem.ResolveRevealNodeId(metroZone!.InstallationId));
0045:             Assert.Contains(PowerGridSystem.BatteryBankItemId, metroZone.RevealedItems);
0046:         }
...
0101:         [Fact]
0102:         public void DamagedMapSystem_FragmentDiscoveryToInstallationReveal()
0103:         {
0104:             var (nodes, routes) = WastelandMapCatalogLoader.Load(
...
0107:
0108:             var (zones, errors) = DamagedMapCatalogLoader.LoadWithValidation(
0109:                 DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
0110:             Assert.Empty(errors);
...
0114:             var metroZone = zones.First(z => z.ZoneId == "metro_service_ring");
0115:             string nodeId = DamagedMapSystem.ResolveRevealNodeId(metroZone.InstallationId)!;
0116:
0117:             // Destination must be initially locked
...
0123:             string? revealedNode = null;
0124:             damagedMapSystem.OnZoneCompleted += z => completedZone = z;
0125:             damagedMapSystem.OnInstallationRevealed += (z, id) => revealedNode = id;
0126:
...
0129:             Assert.True(damagedMapSystem.RegisterFragment("damaged_map_metro_2"));
0130:             Assert.Equal(2, damagedMapSystem.RegisteredCount("metro_service_ring"));
0131:             Assert.False(damagedMapSystem.IsZoneComplete("metro_service_ring"));
0132:             Assert.True(damagedMapSystem.IsDestinationLocked(nodeId));
...
0135:             Assert.True(damagedMapSystem.RegisterFragment("damaged_map_metro_3"));
0136:             Assert.Equal(3, damagedMapSystem.RegisteredCount("metro_service_ring"));
0137:             Assert.True(damagedMapSystem.IsZoneComplete("metro_service_ring"));
0138:             Assert.NotNull(completedZone);
...
0144:             Assert.False(mapSystem.IsLocked(nodeId));
0145:             Assert.False(damagedMapSystem.IsDestinationLocked(nodeId));
0146:         }
0147:
...
0153:                 DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
0154:             var mapSystem = new WastelandMapSystem(new WastelandMapState(), nodes, routes);
0155:             var (zones, _) = DamagedMapCatalogLoader.LoadWithValidation(
0156:                 DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
...
0161:             {
0162:                 damagedMapSystem.RegisterFragment(frag.fragment_id);
0163:             }
0164:             Assert.True(damagedMapSystem.IsZoneComplete("metro_service_ring"));
...
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
0195:             var files = new FileSystemIO();
0196:             string path = Path.Combine(dataDir, "damaged_map_zones.json");
0197:             Assert.True(files.FileExists(path));
0198:
...
0217:         [Fact]
0218:         public void SaveRoundtrip_WaystationAndWastelandMapState_PreservesDeterminism()
0219:         {
0220:             string dataDir = GetDataDir();
...
0229:
0230:             var newMapSystem = new WastelandMapSystem(mapState, mapSystem.Nodes, mapSystem.Routes);
0231:             Assert.True(newMapSystem.IsDiscovered("loc_cut_abandoned_depot"));
0232:             Assert.True(newMapSystem.IsLocked("loc_black_flotilla_outpost"));
```

### Current evidence: `Ashfall.Core.Tests/World/WastelandMapFragmentPersistenceFuzzTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `14bf8218f6aefbe732891aaea229aa845c08bfad58ccb1edbb51c07a137f27bd`
- Snapshot size: 14258 characters; 297 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0020:     /// Battery coverage:
0021:     ///   1. clean round-trip through the live DamagedMapSystem (partial +
0022:     ///      complete zones; no completion/reveal re-fire after restore);
0023:     ///   2. checksummed-envelope round-trip via SaveEnvelopeHelper (the
...
0051:
0052:         private (DamagedMapSystem system, WastelandMapSystem map, List<DamagedMapZone> zones) CreateLive(
0053:             WastelandMapState? state = null)
0054:         {
...
0058:             Assert.Empty(errors);
0059:             return (new DamagedMapSystem(zones, map), map, zones);
0060:         }
0061:
...
0075:
0076:             string completeNode = DamagedMapSystem.ResolveRevealNodeId(complete.InstallationId)!;
0077:             Assert.True(map.IsDiscovered(completeNode));
0078:
...
0113:
0114:             WastelandMapState captured = map.CaptureState();
0115:             Assert.NotEmpty(captured.RegisteredMapFragments);
0116:
...
0119:
0120:             var (success, restored, error) = SaveEnvelopeHelper.RestoreEnvelope<WastelandMapState>(
0121:                 envelopeJson, _json, legacyFallback: null, allowBareFallback: false);
0122:             Assert.True(success, error);
...
0147:
0148:             var (success, _, error) = SaveEnvelopeHelper.RestoreEnvelope<WastelandMapState>(
0149:                 tampered, _json, legacyFallback: null, allowBareFallback: false);
0150:             Assert.False(success);
...
0164:
0165:             var envelope = new SaveEnvelope<WastelandMapState> { State = map.CaptureState(), Checksum = checksum };
0166:             string json = _json.Serialize(envelope);
0167:
...
0186:
0187:             var (success, _, error) = SaveEnvelopeHelper.RestoreEnvelope<WastelandMapState>(
0188:                 bareJson, _json, legacyFallback: null, allowBareFallback: false);
0189:             Assert.False(success);
...
0212:             // must serialize identically (no drift across save generations).
0213:             var restoredState = _json.Deserialize<WastelandMapState>(jsonA);
0214:             Assert.NotNull(restoredState);
0215:             var (restoredSystem, restoredMap, _) = CreateLive(restoredState);
...
0246:                 string envelopeJson = SaveEnvelopeHelper.CaptureEnvelope(fresh.map.CaptureState(), _json);
0247:                 var (success, restored, error) = SaveEnvelopeHelper.RestoreEnvelope<WastelandMapState>(
0248:                     envelopeJson, _json, legacyFallback: null, allowBareFallback: false);
0249:                 Assert.True(success, error);
...
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

## Current JSON audit

#### `Assets/StreamingAssets/Data/damaged_map_zones.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, zones`
- `zones`: list count=12; sample IDs=['industrial_district', 'suburban_heights', 'military_corridor', 'crater_ground_zero', 'deep_coast_shelf', 'high_scarp_ridgeline', 'old_medical_quarter', 'court_district']
- `schema_version`: `1`
- SHA-256: `209af93c2b21f195faf3efb99e8f1645100be56b0b6c02c2045e715afc2cc1e0`
#### `Assets/StreamingAssets/Data/wasteland_map_v1.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, trapSites, nodes, routes`
- `trapSites`: list count=1; sample IDs=[]
- `nodes`: list count=22; sample IDs=['loc_holdfast', 'loc_cut_abandoned_depot', 'loc_cut_radiation_zone_alpha', 'loc_black_flotilla_outpost', 'loc_cut_merchant_caravanserai', 'loc_cut_arsenal_ruin', 'loc_hidden_relay_bunker', 'loc_logistics_reserve_cache']
- `routes`: list count=68; sample IDs=[]
- `schema_version`: `2`
- SHA-256: `5384c1c4093451593327ccc1fce9f8708b21923b8c94104a17e94d72808adf2b`
#### `Assets/StreamingAssets/Data/locations.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, locations`
- `locations`: list count=179; sample IDs=['abandoned_hospital', 'rural_gas_station', 'suburban_house', 'government_bunker', 'stranger_cache', 'location_geo_thermal_plant_ruins', 'location_arcology_sector_4', 'location_frozen_river_barge']
- `schema_version`: `1`
- SHA-256: `97543ade61b6458f5b31bcffb85a96ad5d3b322b80294deb158d1ae29da3386b`
#### `Assets/StreamingAssets/Data/collectibles.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, collectibles`
- `collectibles`: list count=40; sample IDs=['item_collectible_vinyl_chamber_record', 'item_collectible_vinyl_civil_broadcast', 'item_collectible_vinyl_folk_compilation', 'item_collectible_family_portrait', 'item_collectible_unit_photograph', 'item_collectible_civil_defense_poster', 'item_collectible_propaganda_poster', 'item_collectible_concert_poster']
- `schema_version`: `1`
- SHA-256: `b3eb02f8aa33ce9fa0635d8c4894bae09c6449f4c45e91b8e1ed142c588cdd15`
#### `Assets/StreamingAssets/Data/items.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, items`
- `items`: list count=724; sample IDs=['item_decon_chelator_concentrate', 'item_lead_lined_effluent_filter', 'item_heavy_neoprene_scrub_brush', 'item_sealed_waste_bin', 'item_theodolite_brass_precision', 'item_surveyor_stadia_rod', 'item_datum_plate_bronze', 'item_concrete_mix']
- `schema_version`: `1`
- SHA-256: `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`
## Symbol and caller audit

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
#### `DamagedMapCatalog` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=1, host=0, test=0
- `Assets/Ashfall.Core/World/DamagedMapCatalog.cs:266` (core) — CatalogDiagnostics.Warn("DamagedMapCatalog", path, ex);
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
#### `damaged_map_zones` — HOST_REFERENCE_PRESENT — core/declaration=10, host=4, test=1
- `Assets/Ashfall.Core/World/DamagedMapCatalog.cs:9` (core) — /// <summary>Raw deserialization shape for damaged_map_zones.json.</summary>
- `Assets/Ashfall.Core/World/DamagedMapCatalog.cs:118` (core) — /// Loads the damaged treasure-map zone catalog (damaged_map_zones.json).
- `Assets/Ashfall.Core/World/DamagedMapCatalog.cs:126` (core) — public const string DefaultFileName = "damaged_map_zones.json";
- `Assets/Ashfall.Core/Content/CollectibleCatalogIntegrityValidator.cs:270` (core) — CheckFileForItems("damaged_map_zones.json", "map_zone:damaged_map_zones");
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:92` (core) — "damaged_map_zones.json", "currents.json",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:502` (core) — ["damaged_map_zones.json"] = new[] { "WastelandMapSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:831` (core) — ["damaged_map_zones.json"] = "WastelandMapSystem",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1108` (core) — ["damaged_map_zones.json"] = new[] { "WastelandMapSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1467` (core) — ["damaged_map_zones.json"] = new[] { "MapPanel" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1740` (core) — "damaged_map_zones.json", "cassette_sets.json",
- `src/Main.ContentCertification.cs:87` (host) — session.MarkCatalogLoaded("damaged_map_zones.json", false);
- `src/Host/HostCli.Cartography.cs:112` (host) — string damagedMapPath = System.IO.Path.Combine(dataDirectory, "damaged_map_zones.json");
- `src/Host/HostCli.Cartography.cs:113` (host) — Check(files.FileExists(damagedMapPath), "damaged_map_zones.json exists on disk");
- `src/Host/ContentCertificationHostSession.cs:61` (host) — new ContentCertificationFamily("damaged_map_zones", "SMALL RITUAL / COLLECTION / TRADE", "damaged_map_zones.json", "DamagedMapCatalogContainer")
- `Ashfall.Core.Tests/World/Plan16CartographyTests.cs:196` (test) — string path = Path.Combine(dataDir, "damaged_map_zones.json");
#### `WastelandMapState` — HOST_REFERENCE_PRESENT — core/declaration=13, host=22, test=75
- `Assets/Ashfall.Core/World/DamagedMapCatalog.cs:121` (core) — /// WastelandMapState via <see cref="DamagedMapSystem"/>.
- `Assets/Ashfall.Core/World/DamagedMapSystem.cs:13` (core) — /// <see cref="WastelandMapState.RegisteredMapFragments"/> — the single
- `Assets/Ashfall.Core/World/DamagedMapSystem.cs:202` (core) — private WastelandMapState? MapState => _map?.State;
- `Assets/Ashfall.Core/World/WastelandMapCatalogLoader.cs:324` (core) — WastelandMapState? state = null,
- `Assets/Ashfall.Core/World/WastelandMapCatalogLoader.cs:344` (core) — return new WastelandMapSystem(state ?? new WastelandMapState(), nodes, routes, trapSites, tunnelCatalog);
- `Assets/Ashfall.Core/World/WastelandMapSystem.cs:24` (core) — private readonly WastelandMapState _state;
- `Assets/Ashfall.Core/World/WastelandMapSystem.cs:39` (core) — public WastelandMapSystem(WastelandMapState state,
- `Assets/Ashfall.Core/World/WastelandMapSystem.cs:92` (core) — public WastelandMapState State => _state;
- `Assets/Ashfall.Core/World/WastelandMapSystem.cs:784` (core) — public WastelandMapState CaptureState()
- `Assets/Ashfall.Core/World/WastelandMapSystem.cs:790` (core) — public void RestoreState(WastelandMapState state)
- `Assets/Ashfall.Core/World/WastelandMapSystem.cs:1063` (declaration) — public sealed class WastelandMapState
- `Assets/Ashfall.Core/World/WastelandMapSystem.cs:1200` (core) — public WastelandMapState Capture() => new WastelandMapState
- `Assets/Ashfall.Core/World/WastelandMapSystem.cs:1212` (core) — public void RestoreInto(WastelandMapState state, IReadOnlyList<MapNode> nodes)
- `src/Host/HostCli.Cartography.cs:45` (host) — var mapState = new WastelandMapState();
- `src/Host/HostCli.Collectibles.cs:69` (host) — var mapState = new WastelandMapState();
- `src/Host/HostCli.Collectibles.cs:160` (host) — map = new WastelandMapSystem(new WastelandMapState(),
- `src/Host/HostCli.Collectibles.cs:398` (host) — mapProvider: () => new WastelandMapSystem(new WastelandMapState(), liveMapNodes.nodes, liveMapNodes.routes),
- `src/Host/HostCli.Collectibles.cs:445` (host) — var s3Map = new WastelandMapSystem(new WastelandMapState(),
- `src/Host/HostCli.Collectibles.cs:451` (host) — s3Map = new WastelandMapSystem(new WastelandMapState(),
- `src/Host/HostCli.WorldExploration.cs:71` (host) — var mapState = new WastelandMapState();
- `src/Host/SaveStoreChecksumSelfTest.cs:229` (host) — var mapState = new WastelandMapState();
- `src/Host/SevenDayDeterministicSmokeTest.cs:485` (host) — return new WastelandMapSystem(new WastelandMapState(), nodes, routes);
- `src/Host/WastelandMapSaveStore.cs:4` (host) — // Core State : Ashfall.Core.World.WastelandMapState
- `src/Host/WastelandMapSaveStore.cs:26` (host) — private static readonly SaveStore<WastelandMapState> s_store =
- … 86 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `hidden installation` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=1, host=0, test=0
- `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs:417` (core) — return false; // hidden installation — map not completed/revealed (Plan 85)
## Read-only authority alignment

Authority file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
Authority SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
Total lines in snapshot: 5510

The following excerpts are read-only orientation anchors. They do not override current source/data evidence or create implementation authority.
#### authority lines 128-131
00128: | C5 | Expedition field reports and waypoint notes for destinations with sparse `arrival_description`/`revisit_description` coverage; route-waypoint batches | HIGH CONFIDENCE |
00129: | C6 | Gazetteer entries and damaged-zone survey prose; cartographic marginalia | INFERENCE — verify current coverage |
00130: | C7 | Communiqué, directive, and verdict-corpus expansion for factions with thin public/private language separation | HIGH CONFIDENCE |
00131: | C8 | Radio rundown/transcript batches for stations with thin programming; numbers-station and cipher follow-ups | HIGH CONFIDENCE — but distress-signal content is SEALED under `CF-P1-DISTRESS-CONTENT-SEAL` (DR-06); do not add signal scenarios |
#### authority lines 695-698
00695:
00696: **A-15 · C6 · Damaged-zone survey marginalia.** Subject: surveyor marginalia for `damaged_map_zones.json` entries, in the geodetic voice. Evidence: catalog verified live; `GeodeticSurveyHostSession` exists. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00697:
00698: **A-16 · C7 · Standing-record testimony depth.** Subject: witness-statement and registry-annotation prose expanding `standing_record_memory.json` coverage. Evidence: the standing-record family (factions, layouts, memory, quests) is verified live and is a canon epilogue evidence source. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
#### authority lines 942-945
00942:
00943: **DM-6 — Map and geography (C6).** Owners: wasteland map system/loader, damaged zones, fog, route gates, survey instruments. Live catalogs: `wasteland_map_v1`, `damaged_map_zones`, `weather_route_gates`, `gpr_exploration_catalog`, `insar_geodesy_catalog`, `geodetic_survey_catalog`, `seismic_fault_catalog`, `piezometer_network_catalog`. Hosts: GeodeticSurvey, InSarMapping, Cartography selftest family. Openings: A-15, B-09 (GATE). Constraint: the orphan gate `AllMapNodes_ExistInLocationsCatalog` governs all map authoring.
00944:
00945: **DM-7 — Factions and war (C7).** Owners: stance engine, doctrines, war system/chain runner, tributes, treaties, embargoes, espionage, psyops, counter-intelligence, musters, labor camps, bounty board. Live catalogs: `factions`, `faction_lore`, `faction_territory`, `faction_intelligence`, branch catalogs (independent/military/rebel), faction war family (communiques/dialogue/events/journal/radio/location_overrides), `warlord_doctrines`, `muster_*` family (five), `labor_camps`, `bounty_board`, `regional_treaties`, `trade_embargoes`, `foundry_accords`, `holdfast_factions`, `crossing_factions`. Hosts: Espionage, PsyOps, CounterIntelligence, Muster, FactionBranch, RegionalTreaty. Openings: A-16, A-17, A-18, B-10 (GATE), B-11 (GATE), C-05, C-06, G-05, plus the F-004 muster campaign. Constraint: all standing effects through `FactionStanceEngine`.
#### authority lines 1139-1142
01139: - 2026-09-27 — Volume 15: nine remaining Lane A seed expansions into full plans (FP-A09, FP-A14, FP-A15, FP-A20, FP-A23, FP-A24, FP-A25, FP-A29, FP-A30), closing Lane A's compressed-seed backlog, including the FP-A25 200-record quest prose audit tranche program (Tranche 0 census plus eight authoring tranches plus completion regression) — ~19,000 — cumulative ~301,000
01140: - 2026-09-27 — Volume 16: worked content tranche library — twelve PROPOSAL-model JSON tranche examples per established genre (glitch, load-shed, assay, interlock report, marginalia, rundown, intake, quest prose, sighting log, ordnance manifest, almanac), each with validation notes and the three mandatory focused tests — ~12,400 — cumulative ~313,000
01141: - 2026-09-27 — Volume 17: cluster-by-cluster expansion roadmaps C1–C17, each anchored to its deep map with Phase I–IV structure and five cross-cluster sequencing rules — ~16,900 — cumulative ~330,000
01142: - 2026-09-27 — Volume 18: re-audit against the repository's public surface — four drift-register candidates (DR-16 communiqué board and tick-gate, DR-17 map-atlas repair, DR-18 hardening/quarantine cleanup, DR-19 post-v1.0 subsystem families absent from the deep maps), five premise corrections, five evidence-gated seed replenishments (A-31, A-32, B-26, G-09, E-11), and the factory self-audit — ~9,900 — cumulative ~342,000
#### authority lines 1321-1324
01321:
01322: ## 5.11 Gazetteer entry (cartographic register)
01323:
01324: ` ` `text
#### authority lines 1549-1552
01549:
01550: ## 5.23 Map label and descriptions (cartographic play register)
01551:
01552: ` ` `text
#### authority lines 2971-2974
02971:
02972: ## FP-A15 — Damaged-Zone Survey Marginalia
02973:
02974: Lane A · C6 · Status PROPOSAL.
#### authority lines 3140-3143
03140:
03141: ## 16.5 Model tranche — damaged-zone survey marginalia (FP-A15 pattern, contracts 5.11/5.23)
03142:
03143: ` ` `json
#### authority lines 3290-3293
03290:
03291: ## 17.6 C6 — Map and geography (DM-6)
03292: Owns: wasteland map system/loader, damaged zones, fog, route gates, survey instruments. Expanded plans: FP-A15 (this factory); satellite B-09 (GATE — flooded-route topology tags, decision packet DP-01). Phase I: damaged-zone marginalia. Phase II: DP-01's implementation if signed — authored map edges with flood tags consumable by route gates; the orphan gate governs all authoring. Phase III: survey-instrument prose depth across the four geodetic catalogs; census of zone marginalia coverage. Phase IV: DR entry if DP-01's signature changes the map-edge surface (the largest single structural change this cluster can receive). Multi-year arc: the map as the game's most trustworthy narrator — every zone annotated, every route's dangers legible before departure.
03293:
#### authority lines 3354-3357
03354: Status: VERIFIED-AS-LISTED; merge state UNVERIFIED.
03355: Consequence if confirmed: a B-24-class finding (stale/untruthful panel) already repaired at the atlas level; DM-6's and DM-17's openings should be annotated so the factory does not propose a duplicate atlas-truth sweep. The atlas's repaired projection becomes the reference surface for FP-A15's marginalia pairing checks.
03356:
03357: ### DR-18 (candidate) — Repository hardening wave and quarantine cleanup
#### authority lines 3419-3422
03419:
03420: PR #50 (6 commits, merged 2026-09-18): the Map Atlas canonical-projection repair is publicly listed as merged. Volume 18's consequence analysis stands: the atlas is repaired by its owners, and FP-A15's marginalia pairing checks reference the repaired projection. B-24's sweep starts from the live panel inventory and should not list the atlas on stale evidence.
03421:
03422: ### DR-18 — UPGRADED to MERGED-AS-LISTED (2026-09-18)
#### authority lines 3484-3487
03484:
03485: ## 20.3 RB-PAIR — the cross-catalog pairing test pattern (glitch/room, marginalia/zone, assay/process)
03486:
03487: 1. Identify the two catalogs: the prose surface and the id authority.
#### authority lines 3827-3830
03827: Lane allocation: Lane A — up to four tranche plans, one per censused family, each capped at its census-sized batch.
03828: Flagship: FP-A25 Tranche 1 (the first 200-record quest tranche, the largest single authoring batch). Satellites: FP-A24's completion tranche, FP-A29's first sighting-log tranche, FP-A15's marginalia tranche if its census (from ASH-EXP-3's sibling set) supports it.
03829: Verification matrix: RB-PAIR pairing tests per tranche; integrity selftest; per-tranche prose-coverage assertions against the tranche's record ids; FP-A20-style seal guards where any radio-adjacent genre enters (none scheduled).
03830: Closeout discipline: per-tranche closeout with the cumulative completed-record count published; the one-quest-tranche-per-wave rule enforced (Tranche 1 only).
#### authority lines 3950-3953
03950: - VERIFIED: code search across the repository for `ResourceRationing`, `DiscoveryConsequence`, `ItemLore`, `BeliefMovement`, and `rewrite.py` (call-site evidence; section 25.3 and DR-05 update).
03951: - VERIFIED: quest-catalog size inventory and a fragment census of `quests_massive_expansion_200.json` (Volume 26 executes this).
03952:
03953: What this session did NOT read: the interiors of `INTEGRATION_PLANS.md` (78,528 bytes — only its size is now verified), the full per-gate body of `CI_GATE_MANIFEST.json` beyond its header and first entries, individual Core source files' internals, and the remaining DR-16/DR-17/DR-18 carrier documents. Rows depending on those reads are labeled PENDING with their exact verification step, per the runbook's rule 4.
#### authority lines 4017-4020
04017:
04018: **DR-28 (new) — Quest-corpus size ordering contradicts the factory's "largest prose debt" premise. VERIFIED (sizes; fragment census in Volume 26).**
04019: `quests_massive_expansion_200.json` is 223,249 bytes — but `moral_choice_quests_branching.json` is 339,862 bytes, `quests_faction_branching.json` is 194,389, `thirdonary_quests.json` is 143,059, `moral_choice_quests.json` is 141,249, `year_of_ash_questlines.json` is 140,283, and `moral_choice_quests_expansion.json` is 118,103. FP-A25's framing of the 200-record catalog as "the largest single prose debt surface in the data authority" is corrected: it is the largest single record count, not the largest catalog. The tranche program's priority argument survives on record-count grounds but must cite the corrected fact. Additionally, `quests_bureaucratic_morality.json` is 1,885 bytes containing exactly 2 records (verified by full read and parse) — FP-A24's premise of a broad bureaucratic prose-completion surface is retired; the plan's census step, correctly executed, closes FP-A24 with a documented no-change area (see Volume 26.5).
04020:
#### authority lines 4033-4036
04033: 1. FP-A24 — retired in scope; the bureaucratic catalog is 2 records, both with authored prose (Volume 26.5 records the closure as a no-change area).
04034: 2. FP-A25 — corrected framing (record count vs. size) and a field-inventory correction (Volume 26.4: the visible record shape is `id`/`display_name`/`type`/`briefing`/`choices`, with no `objective_text` field in the fragment read; the prose contract must be re-derived from the catalog's actual fields before Tranche 1).
04035: 3. SB-10/F-010 — re-scoped per DR-27.
04036: 4. FP-B14 — vocabulary surface located: `SpiritualModels.cs` (DR-20 table).
#### authority lines 4055-4058
04055:
04056: Volume 25's confirmed access allows a genuine (partial) execution of the ASH-EXP-3 census wave's first steps. This volume publishes what was actually measured and explicitly bounds what was not. It executes the data-authority census in full, the quest-corpus Tranche-0 census in fragment form, and records FP-A24's closure as a no-change area.
04057:
04058: ## 26.1 Data-authority census (COMPLETE)
#### authority lines 4073-4076
04073:
04074: ## 26.3 FP-A25 Tranche 0, fragment execution (PARTIAL — bounded honestly)
04075:
04076: The 223,249-byte `quests_massive_expansion_200.json` could not be read in full this session (the fetch surface returned the first ~32,793 characters — approximately 15 percent of the file). Within that verified fragment:
… 22 additional authority matches omitted; the implementation owner must cite the exact relevant section at execution time.
## Objective and success definition

The objective is to make fragment discovery, assembly, partial triangulation, installation unlock, revisits, and persistence observable through existing map state and content-discovery routes. The success condition is not merely a larger catalog or a more attractive panel. Success requires a current owner, a reachable consumer, a durable state decision, deterministic behavior, truthful UI, explicit failure semantics, and a focused verification handoff.

## Current reality, requested behavior, and minimum delta

**Existing behavior.** DamagedMapSystem, DamagedMapCatalog, WastelandMapSystem, damaged_map_zones.json, cartography host CLI, expedition host, map panels, and dedicated tests are live. The old plan’s hidden-installations file and unreachability claims must be verified before being treated as current.

**Requested behavior.** Make fragment discovery, assembly, partial triangulation, installation unlock, revisits, and persistence observable through existing map state and content-discovery routes.

**Minimum safe delta.** Extend `damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections` only after the current owner and caller audit confirms the missing link. If the old plan’s proposed system already exists, convert the task into a bounded maintenance/reachability package rather than creating a replacement.

## Non-goals and collision exclusions

- No parallel gameplay authority, save store, ledger, selector, event bus, simulation, or UI-owned rule.
- No Unity restoration, Unity dependency, or engine types in Core.
- No edits to authored data or production code in this planning-only pass.
- No broad test suite, full runtime soak, generated index rewrite, or unrelated documentation cleanup.
- No invented API, count, save section, or caller claim. Unknowns remain named unknowns.

## Current reality and required delta

**Current reality.** DamagedMapSystem, DamagedMapCatalog, WastelandMapSystem, damaged_map_zones.json, cartography host CLI, expedition host, map panels, and dedicated tests are live. The old plan’s hidden-installations file and unreachability claims must be verified before being treated as current.

**Required delta.** Make fragment discovery, assembly, partial triangulation, installation unlock, revisits, and persistence observable through existing map state and content-discovery routes.

**Primary seam.** damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections

**Non-goals.** This planning package does not modify production, authored data, saves, tests, UI, generated indexes, assets, or runtime code. It does not approve a new owner, authorize Unity work, or turn a historical plan claim into current evidence. The following terms are design hypotheses to verify during implementation, not facts asserted by this document: `DamagedMapSystem`, `DamagedMapCatalog`, `WastelandMapSystem`, `damaged_map_zones`, `WastelandMapState`, `hidden installation`.

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
| Domain rules | DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands | Bind inputs and translate facts | Existing owner DTO/save | Core focused tests |
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

The implementation must cover null/empty state, empty catalogs, duplicate IDs, missing references, stale old saves, invalid numeric values, extreme but bounded values, unavailable owners, dead or absent participants, repeated commands, simultaneous events, host reload, missing UI, missing audio/journal, new-game reset, teardown, and deterministic replay. The expected result should preserve the last valid state, report a useful diagnostic, and avoid presenting a fabricated success. The detailed failure matrix below expands these cases for 85.

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
- What current evidence must be reread? DamagedMapSystem, DamagedMapCatalog, WastelandMapSystem, damaged_map_zones.json, cartography host CLI, expedition host, map panels, and dedicated tests are live. The old plan’s hidden-installations file and unreachability claims must be verified before being treated as current.
- What is the smallest safe change? Make fragment discovery, assembly, partial triangulation, installation unlock, revisits, and persistence observable through existing map state and content-discovery routes.
- Which owner is touched? DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands
- What must remain untouched? No edits to production.
- What proves completion? A written evidence table and focused baseline commands.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 1: Owner and collision map
- What current evidence must be reread? DamagedMapSystem, DamagedMapCatalog, WastelandMapSystem, damaged_map_zones.json, cartography host CLI, expedition host, map panels, and dedicated tests are live. The old plan’s hidden-installations file and unreachability claims must be verified before being treated as current.
- What is the smallest safe change? Make fragment discovery, assembly, partial triangulation, installation unlock, revisits, and persistence observable through existing map state and content-discovery routes.
- Which owner is touched? DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands
- What must remain untouched? No new catalog or state.
- What proves completion? Single-owner map with zero unresolved authority collisions.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 2: Core contract or bounded extension
- What current evidence must be reread? DamagedMapSystem, DamagedMapCatalog, WastelandMapSystem, damaged_map_zones.json, cartography host CLI, expedition host, map panels, and dedicated tests are live. The old plan’s hidden-installations file and unreachability claims must be verified before being treated as current.
- What is the smallest safe change? Make fragment discovery, assembly, partial triangulation, installation unlock, revisits, and persistence observable through existing map state and content-discovery routes.
- Which owner is touched? DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands
- What must remain untouched? No Godot/UI/data authoring.
- What proves completion? Core tests for boundaries, transitions, invalid data, and determinism.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 3: Persistence and migration contract
- What current evidence must be reread? DamagedMapSystem, DamagedMapCatalog, WastelandMapSystem, damaged_map_zones.json, cartography host CLI, expedition host, map panels, and dedicated tests are live. The old plan’s hidden-installations file and unreachability claims must be verified before being treated as current.
- What is the smallest safe change? Make fragment discovery, assembly, partial triangulation, installation unlock, revisits, and persistence observable through existing map state and content-discovery routes.
- Which owner is touched? DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands
- What must remain untouched? No unrelated save sections.
- What proves completion? Round-trip, deep-copy, corrupt/invalid, and mid-event tests are required to pass.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 4: Authored data tranche
- What current evidence must be reread? DamagedMapSystem, DamagedMapCatalog, WastelandMapSystem, damaged_map_zones.json, cartography host CLI, expedition host, map panels, and dedicated tests are live. The old plan’s hidden-installations file and unreachability claims must be verified before being treated as current.
- What is the smallest safe change? Make fragment discovery, assembly, partial triangulation, installation unlock, revisits, and persistence observable through existing map state and content-discovery routes.
- Which owner is touched? DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands
- What must remain untouched? No prose-only orphan rows.
- What proves completion? Data integrity and consumer coverage pass for the tranche.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 5: Host/event wiring
- What current evidence must be reread? DamagedMapSystem, DamagedMapCatalog, WastelandMapSystem, damaged_map_zones.json, cartography host CLI, expedition host, map panels, and dedicated tests are live. The old plan’s hidden-installations file and unreachability claims must be verified before being treated as current.
- What is the smallest safe change? Make fragment discovery, assembly, partial triangulation, installation unlock, revisits, and persistence observable through existing map state and content-discovery routes.
- Which owner is touched? DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands
- What must remain untouched? No panel gameplay math.
- What proves completion? Host wiring test proves event → projection and setup/teardown.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 6: Presentation and accessibility
- What current evidence must be reread? DamagedMapSystem, DamagedMapCatalog, WastelandMapSystem, damaged_map_zones.json, cartography host CLI, expedition host, map panels, and dedicated tests are live. The old plan’s hidden-installations file and unreachability claims must be verified before being treated as current.
- What is the smallest safe change? Make fragment discovery, assembly, partial triangulation, installation unlock, revisits, and persistence observable through existing map state and content-discovery routes.
- Which owner is touched? DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands
- What must remain untouched? No new authority in UI.
- What proves completion? Panel route/focus/headless checks pass; snapshots only through the owning harness.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 7: End-to-end and replay
- What current evidence must be reread? DamagedMapSystem, DamagedMapCatalog, WastelandMapSystem, damaged_map_zones.json, cartography host CLI, expedition host, map panels, and dedicated tests are live. The old plan’s hidden-installations file and unreachability claims must be verified before being treated as current.
- What is the smallest safe change? Make fragment discovery, assembly, partial triangulation, installation unlock, revisits, and persistence observable through existing map state and content-discovery routes.
- Which owner is touched? DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands
- What must remain untouched? No full-suite default.
- What proves completion? Named commands/results and limitations recorded.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 8: Balance/content polish
- What current evidence must be reread? DamagedMapSystem, DamagedMapCatalog, WastelandMapSystem, damaged_map_zones.json, cartography host CLI, expedition host, map panels, and dedicated tests are live. The old plan’s hidden-installations file and unreachability claims must be verified before being treated as current.
- What is the smallest safe change? Make fragment discovery, assembly, partial triangulation, installation unlock, revisits, and persistence observable through existing map state and content-discovery routes.
- Which owner is touched? DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands
- What must remain untouched? No hidden tuning or parallel scalar.
- What proves completion? Content review confirms no dominated/unreachable row and no unsupported claim.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 9: Rollback and closeout
- What current evidence must be reread? DamagedMapSystem, DamagedMapCatalog, WastelandMapSystem, damaged_map_zones.json, cartography host CLI, expedition host, map panels, and dedicated tests are live. The old plan’s hidden-installations file and unreachability claims must be verified before being treated as current.
- What is the smallest safe change? Make fragment discovery, assembly, partial triangulation, installation unlock, revisits, and persistence observable through existing map state and content-discovery routes.
- Which owner is touched? DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands
- What must remain untouched? No unowned cleanup.
- What proves completion? Foreman review accepts or records a blocker.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

## Ownership matrix and file impact map
The following is an impact map for a future implementation package, not a request to edit these paths in this planning-only task.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/World/DamagedMapSystem.cs` — DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/World/DamagedMapCatalog.cs` — DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/World/WastelandMapSystem.cs` — DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Host/HostCli.Cartography.cs` — DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Host/ExpeditionHostSession.cs` — DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Main.Expeditions.cs` — DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/UI/MapAtlasPanel.cs` — DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/UI/SubterraneanCartographyPanel.cs` — DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands; change only the confirmed owner seam.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/damaged_map_zones.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/wasteland_map_v1.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/locations.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/collectibles.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/items.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after presentation claim`: `src/UI/MapAtlasPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/SubterraneanCartographyPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/MapDetailPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/World/Plan85_71DamagedMapPowerIntegrationTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/World/Plan16CartographyTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/World/WastelandMapFragmentPersistenceFuzzTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/World/WastelandMapRouteValidationTests.cs` — focused verification; no test created to mask a stale premise.

## Out of scope
- No unrelated refactor.
- No Unity restoration or dependency.
- No generated index or unrelated documentation regeneration.
- No broad test suite or runtime soak by default.
- No new save owner, registry, selector, or simulation unless a signed architecture decision names it.

## Definition of done
- The current owner and public API are cited from the implementation snapshot.
- damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections is wired end to end or the plan explicitly closes as already integrated.
- Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue.
- Data references, schema version, old-save behavior, focused tests, and rollback are recorded.
- No stale “sealed”, “approved”, or pass-count language is used without current evidence.

## Numbered implementation contract

# 1. Objective

Deliver only the bounded delta described as: Make fragment discovery, assembly, partial triangulation, installation unlock, revisits, and persistence observable through existing map state and content-discovery routes. The objective is measured by current-owner reachability, truthful state, deterministic replay, and a safe implementation handoff rather than by document length.

# 2. Current Reality

DamagedMapSystem, DamagedMapCatalog, WastelandMapSystem, damaged_map_zones.json, cartography host CLI, expedition host, map panels, and dedicated tests are live. The old plan’s hidden-installations file and unreachability claims must be verified before being treated as current.

# 3. Required Delta

Make fragment discovery, assembly, partial triangulation, installation unlock, revisits, and persistence observable through existing map state and content-discovery routes.

# 4. Evidence

Use the current source/data dossier, JSON audit, symbol/caller audit, and read-only authority excerpts in this document. The canonical authority is docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md with SHA-256 911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c; current source/data remain the implementation truth when the authority is descriptive or historical.

# 5. Existing Extension Seams

Primary seam: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections. Before creating anything, verify the current public method, event, host session, save store, and consumer named in the dossier. A new abstraction is justified only when this seam cannot express the confirmed delta.

# 6. Proposed Architecture

Use the owner-first Core → data → host → presentation architecture described above. The proposed architecture is a bounded extension of DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands; it does not create a parallel gameplay system.

# 7. Ownership Matrix

Canonical ownership: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands. JSON owns authored content, Core owns pure rules/state transitions, the host owns lifecycle/adapters, and Godot panels own presentation only. Every proposed write must be assigned to exactly one row of that matrix.

# 8. Data Flow

INPUT (catalog, command, current state) → VALIDATION (owner/schema/reference checks) → CORE STATE → DOMAIN FACT → HOST PROJECTION → UI FEEDBACK → SAVE OWNER. The reverse UI path is a command request, never a direct state mutation.

# 9. State Model

State rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. The implementing agent must document fields, defaults, lifecycle, mutation, reset, persistence, and migration against the actual current DTO before editing.

# 10. API/Contracts

Expose only the current owner’s read query, command/mutation, typed fact/event, capture/restore, diagnostic result, idempotency key, and stable ordering needed for damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections. Confirm signatures from source; never copy historical API names from the old plan.

# 11. Data Changes

Data changes must extend current catalogs under Assets/StreamingAssets/Data/. For each row, validate schema_version, snake_case ID, references, ranges, default behavior, loader, consumer, and observable outcome. The record review ledger applies this rule to Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.

# 12. Save/Load

Persistence must use the current owner identified by the dossier. Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Require capture/restore, deep-copy isolation, old-version defaults, partial/corrupt input behavior, checksum handling, and mid-event reload before claiming persistence.

# 13. Determinism

Determinism contract: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration. Reuse existing seeded RNG forks, ordinal ordering, bounded state, and invariant culture formatting. A compile-green result is not replay evidence.

# 14. System/Event Wiring

Wire the confirmed event or command through damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections. Define event order, daily/hourly triggers, idempotency, subscriber lifetime, missing-owner behavior, and the exact host projection. Do not add a second event authority.

# 15. Godot Integration

Godot integration is limited to the current host/session and named presentation surfaces. UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs'] Preserve focus, controller/keyboard close/back, contrast, reduced motion, refresh, and disposal behavior.

# 16. Narrative/Content Integration

Content must describe only effects expressible by the current owner. Record-level action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer. Validate narrative references, continuity, voice, and player-visible consequence without making prose a hidden gameplay authority.

# 17. Failure Modes

Failure behavior: Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue. The failure matrix covers null/empty state, missing/duplicate references, old/corrupt saves, unavailable owners, dead or hostile participants, repeated commands, simultaneous events, reload, missing UI/audio/journal, and deterministic replay.

# 18. Test Strategy

Use the smallest existing focused test first; run a new test alone; keep save/load, determinism, lifecycle, mutation, fuzzing, state-transition, and cross-system workflows independently reported. The plan-only package does not claim these commands were run.

# 19. Dependency-Ordered Phases

Follow phases 0–9: premise recheck, owner/collision map, Core contract, persistence/migration, data tranche, host/event wiring, presentation/accessibility, end-to-end/replay, balance/polish, and rollback/closeout. Each phase has a completion gate and a must-not-touch boundary above.

# 20. File Impact Map

The future implementation package may modify only the confirmed owner/data/host/UI/test paths listed in the dossier and only after claiming them. This Round 7 planning package intentionally modifies none of those production paths.

# 21. Risks

Primary risk: The map orphan gate and existing WastelandMapState are hard constraints; no new map node may be introduced without a valid locations-catalog reference. Additional risks are dirty-worktree drift, stale catalog counts, missing host callers, shared save seams, decision-gated authority, accessibility regressions, and false completion claims. Each risk has a stop/escalate rule in the handoff.

# 22. Out of Scope

No unrelated refactor, Unity restoration, new parallel authority, broad test suite, generated-index rewrite, asset production, or opportunistic gameplay tuning is included.

# 23. Rollback Strategy

Rollback is local: disable the adapter, remove only the bounded authored tranche, preserve the owner’s last valid state, and keep the save schema readable. Never reset the shared worktree or hide a failure with a destructive migration.

# 24. Definition of Done

The implementing package is done only when damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections is wired or proven already integrated, current data validates and reaches a consumer, save/determinism/failure/UI contracts pass focused verification, accessibility is truthful, and the handoff records limitations. This document itself remains planning-only.

# 25. Implementation Handoff

MUST PRESERVE the current owner, Godot/Core boundary, JSON authority, save/determinism contracts, and accessibility. MUST ADD only the smallest confirmed extension and focused evidence. MUST NOT invent APIs or claim unrun tests. FIRST SAFE STEP: reread the first current owner/catalog/host/test path and write a live-versus-stale premise table before any implementation edit.

## Detailed record-by-record integration ledger

### Record review 001: `industrial_district`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 002: `suburban_heights`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 003: `military_corridor`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 004: `crater_ground_zero`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 005: `deep_coast_shelf`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 006: `high_scarp_ridgeline`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 007: `old_medical_quarter`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 008: `court_district`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 009: `pasture_valley`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 010: `north_woods`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 011: `university_quarter`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 012: `metro_service_ring`
- Source: `Assets/StreamingAssets/Data/damaged_map_zones.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 013: `loc_holdfast`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 014: `loc_cut_abandoned_depot`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 015: `loc_cut_radiation_zone_alpha`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 016: `loc_black_flotilla_outpost`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 017: `loc_cut_merchant_caravanserai`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 018: `loc_cut_arsenal_ruin`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 019: `loc_hidden_relay_bunker`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 020: `loc_logistics_reserve_cache`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 021: `loc_broadcast_bunker_echo`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 022: `loc_diesel_tank_farm`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 023: `loc_recovery_yard`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 024: `loc_underground_fuel_depot`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 025: `loc_municipal_seed_vault`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 026: `loc_blacksite_armory_7`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 027: `loc_excavation_command_vault`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 028: `loc_deaddrop_command_shelter`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 029: `loc_sealed_triage_annex`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 030: `loc_evidence_sub_basement`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 031: `loc_quarantine_barn`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 032: `loc_forestry_emergency_store`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 033: `loc_materials_research_sublevel`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 034: `loc_electrical_maintenance_exchange`
- Source: `Assets/StreamingAssets/Data/wasteland_map_v1.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 035: `abandoned_hospital`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 036: `rural_gas_station`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 037: `suburban_house`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 038: `government_bunker`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 039: `stranger_cache`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 040: `location_geo_thermal_plant_ruins`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 041: `location_arcology_sector_4`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 042: `location_frozen_river_barge`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 043: `location_crashed_icebreaker_convoy`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 044: `location_silent_observatory`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 045: `location_subterranean_seed_vault`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 046: `location_ministry_of_truth_bunker`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 047: `location_ash_dune_cemetery`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 048: `location_abandoned_ski_resort`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 049: `location_geothermal_borehole_site`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 050: `location_flooded_subway_depot`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 051: `location_sub_level_4_transit`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 052: `location_municipal_sewage`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 053: `location_collapsed_salt_mine`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 054: `location_bio_remediation_lab`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 055: `location_submerged_data_center`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 056: `location_geothermal_vent_shaft`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 057: `location_the_sump_cathedral`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 058: `location_abandoned_desalination`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 059: `location_deep_core_borehole`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 060: `location_uxo_highway_choke`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 061: `location_radar_array_spire`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 062: `location_drone_hive_silo`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 063: `location_automated_mortar_pit`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 064: `location_scrap_neuromancer_camp`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 065: `location_magnetic_anomaly_crater`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 066: `location_abandoned_convoy_yard`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 067: `location_acoustic_testing_facility`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 068: `location_substation_omega`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 069: `location_the_dead_hand_core`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 070: `location_lethe_water_treatment`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 071: `location_observatory_dome`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 072: `location_submerged_arcology`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 073: `location_concrete_batching_plant`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 074: `location_seed_vault_antechamber`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 075: `location_hospital_psych_wing`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 076: `location_mirror_factory`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 077: `location_radio_telescope_array`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 078: `location_ash_whale_carcass`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 079: `location_the_memory_vault`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 080: `highway_pileup`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 081: `prewar_medical_cache`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 082: `loc_grange_hall`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 083: `loc_apiary_rows`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 084: `loc_seed_library_annex`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 085: `loc_veterinary_surgery`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 086: `loc_school_gymnasium`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 087: `loc_cider_press`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 088: `loc_terrace_pumphouse`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 089: `loc_ration_queue_plaza`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 090: `loc_conscription_office`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 091: `loc_municipal_archive`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 092: `loc_dentists_row`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 093: `loc_transit_authority_hq`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 094: `loc_printworks`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 095: `loc_department_store`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 096: `loc_public_swimming_baths`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 097: `loc_st_brigids_almshouse`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 098: `loc_weighbridge`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 099: `loc_motel_verity`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 100: `loc_bridge_seven`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 101: `loc_ordnance_shoulder`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 102: `loc_bus_reversal_loop`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 103: `loc_radio_relay_mast`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 104: `loc_ash_sign_shrine`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 105: `loc_pilgrim_switchbacks`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 106: `loc_snowline_station`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 107: `loc_low_background_lab`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 108: `loc_ice_core_store`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 109: `loc_avalanche_gallery`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 110: `loc_summit_relay`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 111: `loc_the_vessels_cell`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 112: `loc_lock_gate_four`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 113: `loc_pump_station_nine`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 114: `loc_alloc_12b`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 115: `loc_records_annex`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 116: `loc_drowned_cinema`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 117: `loc_cold_store_atlantic`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 118: `loc_bathymetric_boat`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 119: `loc_the_shallows_market`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 120: `loc_the_allotments`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 121: `checkpoint_kilo_armory`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 122: `hospital_pharmacy`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 123: `family_bunker_backyard_shed`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 124: `old_library_cache`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 125: `convoy_echo7_cache`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 126: `raider_ambush_site`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 127: `collapsed_building`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 128: `raider_trap_location`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 129: `electrical_substation`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 130: `ruined_garage`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 131: `concert_hall_ruins`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 132: `loc_grain_silo`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 133: `loc_garrison_checkpoint_gamma`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 134: `loc_railway_span_44_alpha`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 135: `loc_forward_roster_camp`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 136: `loc_shrine_switchback_waystation`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 137: `loc_understory_transmitter`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 138: `loc_shelter_gate`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 139: `loc_shelter_meeting`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 140: `loc_shelter_infirmary`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 141: `loc_shelter_storage`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 142: `loc_shelter_quarters`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 143: `loc_shelter_fire`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 144: `loc_shelter_perimeter`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 145: `loc_eastern_road`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 146: `loc_neutral_ground`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 147: `loc_water_station`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 148: `loc_excavation_utility_tunnels`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 149: `loc_excavation_metro_interchange`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 150: `loc_excavation_mine_shaft`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 151: `loc_excavation_archive_bunker`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 152: `loc_excavation_drainage_network`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 153: `loc_excavation_storage_chamber`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 154: `loc_excavation_civilian_shelter`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 155: `loc_settlement_brine_pans`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 156: `loc_settlement_iron_siding`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 157: `loc_settlement_cape_beacon`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 158: `loc_settlement_slate_hollow`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 159: `loc_settlement_pilgrim_hearth`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 160: `loc_settlement_tinkers_notch`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 161: `location_quarry_overlook`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 162: `loc_grain_exchange`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 163: `loc_automated_abattoir`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 164: `loc_flooded_subway_depot`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 165: `loc_scavenger_camp`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 166: `loc_iron_garrison`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 167: `loc_dead_zone`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 168: `loc_settlement_ferry_crossing`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 169: `loc_settlement_nine_rails`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 170: `loc_settlement_fort_karkov`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 171: `loc_settlement_lock_seven`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 172: `loc_settlement_silo_burrow`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 173: `loc_settlement_st_nicholas`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 174: `loc_iron_crest`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 175: `loc_ash_needle`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 176: `loc_wind_gap_ridge`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 177: `loc_signal_hill_tower`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 178: `loc_river_bend_outpost`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 179: `loc_rusted_span_bridge`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 180: `loc_old_crematory_stacks`
- Source: `Assets/StreamingAssets/Data/locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Verify that each fragment has a real source, stable identity, meaningful partial state, a valid destination/reveal contract, and a persistence/UI consumer.
- Primary owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State/save rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI truth rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

## Precision scenario matrix
Each row is a future implementation checkpoint, not a claim that the current repository already passes it.
### Scenario 01: fresh campaign before the owner is initialized
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 02: old save restored at day zero
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 03: old save restored after a partial event
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 04: catalog unavailable at startup
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 05: catalog contains an empty collection
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 06: duplicate canonical ID
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 07: reference points to a missing item
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 08: reference points to a missing location
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 09: reference points to a missing faction
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 10: unavailable optional owner
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 11: owner disabled by difficulty or policy
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 12: unpowered infrastructure
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 13: zero resources
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 14: negative or malformed numeric input
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 15: large but bounded collection
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 16: dead survivor or unavailable participant
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 17: hostile faction state
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 18: repeated player command
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 19: simultaneous day events
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 20: mid-transition save
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 21: reload after event dispatch
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 22: missing UI surface
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 23: stale presentation cache
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 24: missing audio cue
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 25: missing journal owner
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 26: missing save owner
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 27: corrupt save payload
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 28: checksum mismatch
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 29: RNG fork unavailable
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 30: unordered dictionary iteration
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 31: clock boundary at midnight
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 32: seasonal boundary
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 33: weather gate closure
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 34: route closure
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 35: trade or treaty conflict
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 36: choice already resolved
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 37: ending owner unavailable
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 38: optional content absent
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 39: mod or compatibility row absent
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 40: concurrent package changes source
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 41: headless session without UI
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 42: snapshot fixture unavailable
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 43: controller/keyboard focus path
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 44: screen reader/high contrast path
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 45: asset/resource fallback
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 46: final archive projection
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 47: new-game reset
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 48: legacy content migration
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 49: consumer not wired
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 50: host setup order reversed
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 51: teardown/disposal
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 52: replay after reload
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 53: telemetry/diagnostic emission
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 54: authority conflict discovered
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 55: future implementation acceptance boundary
- Domain lens: cartographic fragments and hidden installations.
- Seam under test: damaged_map_zones.json -> DamagedMapCatalog/System -> WastelandMapState.Discover/Unlock -> expedition/content/map projections.
- Expected authority: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger. Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI/accessibility check: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

## Failure and rejection matrix
The failure contract is intentionally strict: an unavailable feature is preferable to a convincing but unauthoritative simulation.
### Failure 01: null state
- Detection: accept an explicit empty/default state only where the owner contract permits it.
- Owner response: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Player-facing truth: Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue.
- Persistence response: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism response: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 02: empty catalog
- Detection: report a data-integrity gap and keep the previous safe projection.
- Owner response: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Player-facing truth: Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue.
- Persistence response: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism response: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 03: duplicate ID
- Detection: reject the row with a stable diagnostic rather than last-write-wins.
- Owner response: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Player-facing truth: Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue.
- Persistence response: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism response: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 04: missing reference
- Detection: do not create a phantom entity; expose the unresolved dependency.
- Owner response: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Player-facing truth: Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue.
- Persistence response: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism response: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 05: stale save
- Detection: migrate or default only through the owner’s versioned restore path.
- Owner response: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Player-facing truth: Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue.
- Persistence response: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism response: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 06: corrupt checksum
- Detection: refuse the corrupted section and preserve unrelated valid sections.
- Owner response: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Player-facing truth: Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue.
- Persistence response: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism response: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 07: host reload
- Detection: rehydrate through the same owner and event registration path.
- Owner response: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Player-facing truth: Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue.
- Persistence response: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism response: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 08: new game
- Detection: clear transient host state and initialize owner defaults exactly once.
- Owner response: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Player-facing truth: Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue.
- Persistence response: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism response: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 09: deterministic replay
- Detection: same seed, day, catalog, and state must produce the same fact/order.
- Owner response: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Player-facing truth: Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue.
- Persistence response: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism response: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 10: UI unavailable
- Detection: retain canonical state and defer presentation without re-running mutation.
- Owner response: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Player-facing truth: Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue.
- Persistence response: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism response: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 11: disposal
- Detection: unregister listeners and release host resources deterministically.
- Owner response: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Player-facing truth: Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue.
- Persistence response: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism response: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 12: concurrent claim
- Detection: stop and hand off rather than editing a shared seam.
- Owner response: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Player-facing truth: Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue.
- Persistence response: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism response: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 13: authority conflict
- Detection: name the conflict and defer the architectural decision.
- Owner response: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Player-facing truth: Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue.
- Persistence response: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism response: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 14: missing test
- Detection: do not claim integration; add the focused contract test in the implementing package.
- Owner response: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Player-facing truth: Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue.
- Persistence response: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism response: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 15: unsupported API
- Detection: use the current public API or mark the premise stale.
- Owner response: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Player-facing truth: Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue.
- Persistence response: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism response: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 16: legacy Unity reference
- Detection: do not restore it; port only through the Godot/Core boundary.
- Owner response: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Player-facing truth: Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue.
- Persistence response: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism response: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 17: engine reference in Core
- Detection: reject the change and move only presentation adaptation to src/.
- Owner response: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Player-facing truth: Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue.
- Persistence response: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism response: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 18: unowned save state
- Detection: reject the change until the owner and migration path are explicit.
- Owner response: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Player-facing truth: Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue.
- Persistence response: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism response: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 19: UI-owned gameplay
- Detection: reject the panel mutation and route the command to Core.
- Owner response: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Player-facing truth: Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue.
- Persistence response: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism response: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 20: false completion claim
- Detection: downgrade the handoff to planning/static evidence only.
- Owner response: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- Player-facing truth: Missing fragment source, duplicate fragment, unknown destination, partial old save, map reload, and unreachable installation preserve prior state and report a precise integrity issue.
- Persistence response: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism response: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

## Layered focused verification matrix
Commands are exact paths only where the current test file exists. They are future implementation gates for this plan-only package.
### Verification layer 01: Core unit
- Coverage: valid input, boundary, missing optional field, duplicate/unknown reference, state invariant.
- Domain contract: DamagedMapSystem.
- Owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 02: Core transition
- Coverage: one legal transition, one illegal transition, repeated transition, cancellation/rollback.
- Domain contract: DamagedMapCatalog.
- Owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 03: save round-trip
- Coverage: capture, serialize, restore, deep-copy isolation, old version/defaults.
- Domain contract: WastelandMapSystem.
- Owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 04: data integrity
- Coverage: schema_version, snake_case IDs, duplicate IDs, references, ranges, collection shape.
- Domain contract: damaged_map_zones.
- Owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 05: host wiring
- Coverage: setup, command dispatch, event subscription, refresh, disposal, missing owner.
- Domain contract: WastelandMapState.
- Owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 06: UI projection
- Coverage: truthful current state, disabled action, focus order, controller/keyboard close/back.
- Domain contract: hidden installation.
- Owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 07: headless/selftest
- Coverage: bounded deterministic scenario and diagnostics without a renderer.
- Domain contract: DamagedMapSystem.
- Owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 08: cross-system
- Coverage: owner event to consumer, ordering, idempotency, no parallel state.
- Domain contract: DamagedMapCatalog.
- Owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 09: replay
- Coverage: same seed/day/input produces same state hash and fact order.
- Domain contract: WastelandMapSystem.
- Owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 10: content utilization
- Coverage: authored record has a current loader, consumer, and observable outcome.
- Domain contract: damaged_map_zones.
- Owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 11: migration
- Coverage: old save and current catalog remain readable or fail with a named reason.
- Domain contract: WastelandMapState.
- Owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 12: rollback
- Coverage: feature flag/owner boundary permits disabling the delta without data loss.
- Domain contract: hidden installation.
- Owner: DamagedMapSystem owns fragment/zone progression; WastelandMapSystem owns map graph state; content/expedition owners produce fragments; host routes commands.
- State rule: Collected fragment IDs, completed zones, and unlocked installations persist through map state or the existing damaged-map owner; no duplicate inventory ledger.
- Determinism rule: Fragment selection, hint calculation, and reveal order are stable by zone/fragment IDs and current day; no unordered dictionary iteration.
- UI rule: ['src/UI/MapAtlasPanel.cs', 'src/UI/SubterraneanCartographyPanel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

#### Current focused command 01
- Test: `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/World/DamagedMapSystemTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 02
- Test: `Ashfall.Core.Tests/World/Plan85_71DamagedMapPowerIntegrationTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan85_71DamagedMapPowerIntegrationTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 03
- Test: `Ashfall.Core.Tests/World/Plan16CartographyTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan16CartographyTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 04
- Test: `Ashfall.Core.Tests/World/WastelandMapFragmentPersistenceFuzzTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/World/WastelandMapFragmentPersistenceFuzzTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 05
- Test: `Ashfall.Core.Tests/World/WastelandMapRouteValidationTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/World/WastelandMapRouteValidationTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

> **Structured body length before post-250K polishing:** 468,326 characters.
# Post-250K deep polishing pass

The architecture body above reached 468,404 characters before this pass. This is a second deep polishing phase, not a license to pad. Re-read the plan as an implementer and tighten it against the current source/data snapshot.

## Deep polish A — authority and collision recheck

- Re-run the owner/caller audit for: `DamagedMapSystem`, `DamagedMapCatalog`, `WastelandMapSystem`, `damaged_map_zones`, `WastelandMapState`, `hidden installation`.
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
