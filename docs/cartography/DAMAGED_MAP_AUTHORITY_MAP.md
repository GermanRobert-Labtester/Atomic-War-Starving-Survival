# Damaged-Map Authority Map (Plan 85)

One authority per concern. Plan 85 added no parallel registries.

| Concern | Authoritative owner | Plan 85 role |
|---|---|---|
| Zone/fragment/installation static definitions | `Assets/StreamingAssets/Data/damaged_map_zones.json` + `Ashfall.Core/World/DamagedMapCatalog.cs` (loader + structural validation) | expanded data; added loader |
| Physical item identity | `items.json` / item catalog | referenced only (zero new items) |
| Inventory possession | `InventorySystem` | untouched (fragments are not items) |
| Fragment discovery/registration | `Ashfall.Core/World/DamagedMapSystem.RegisterFragment` via `ScavengingRollResult.MapFragmentId` from `ScavengingTableCatalog.RollLoot`, forwarded by `ExpeditionSystem.PerformLootRoll` | new (the missing live seam) |
| Fragment progress persistence | `WastelandMapState.RegisteredMapFragments` inside the existing `wasteland_map` save section (`WastelandMapSaveStore` → Core `SaveStore<T>`) | extended existing state DTO |
| Map completion | derived: every zone fragment registered (`DamagedMapSystem.IsZoneComplete`) | computed, never stored |
| Installation discovery (world map) | `WastelandMapSystem.Discover/Unlock` + `OnNodeDiscovered`/`OnNodeLockChanged` | invoked on completion edge |
| Expedition availability | `expeditions.json` destinations + `ExpeditionSystem.Start` gate (`DamagedMap.IsDestinationLocked`) + `ExpeditionHostSession.GetBlockReason` (UI reason: "Map incomplete — location unidentified") | data + core gate |
| World-map marker | `WastelandMapView` via `WastelandMapSystem.ResolveNodeStatus` (Locked → Discovered after reveal) | consumed, unchanged |
| Scavenging production | `scavenging_tables.json` / `ScavengingTableCatalog` (Plan 46 authority) | added weighted fragment entries |
| Site loot | destination `scavenging_table_id` + `lootCategories` through the existing `PerformLootRoll` → `AddLoot` → inventory path | referenced (no new loot engine) |
| Unique-loot claimed state | n/a in v1 — no unique one-time rewards authored | documented in loot provenance |
| Collectible/codex text | `collectibles.json` (Plan 47) — no map collectibles exist | untouched |
| Environmental storytelling | `installation_description` fields (data authority) | authored per §8 quality bar |
| Campaign persistence | existing save stores (`wasteland_map` section) | extended, never shadowed |
| Deterministic RNG | `ISeededRng` / `SeededRng` (existing expedition roll path) | consumed, unchanged |

## Composition / wiring (production)

```
ComposeCampaign
  ├─ SetupWorld        → WorldHostSession.Create(dataDir)
  │    └─ WastelandMapCatalogLoader.CreateSystem → WastelandMapSystem
  │    └─ restore WastelandMapSaveStore → in-place state restore
  │    └─ DamagedMapCatalogLoader.CreateSystem   → DamagedMapSystem (bound to WastelandMap)
  ├─ SetupExpeditions  → ExpeditionHostSession.Create(dataDir)
  │    └─ Engine.ScavengingCatalog = ScavengingTableCatalog (Plan 46, now live)
  │    └─ AttachDamagedMapIfReady(): Engine.DamagedMap = _world.DamagedMap
  └─ SetupMaritime     → weather gates on ExtraGateBlock (composed, not clobbered)

Scavenging roll (during sortie Looting phase):
  ExpeditionSystem.PerformLootRoll
    → ScavengingCatalog.RollLoot(tableId, rng)          [seeded ISeededRng]
    → rollResult.MapFragmentId non-empty
    → DamagedMap.RegisterFragment(id)                   [idempotent]
    → zone complete edge → OnZoneCompleted
       → WastelandMap.Discover(loc_<installation>) + Unlock
       → expedition destination passes IsDestinationLocked
    → fragment-only entry (empty item_id) yields no physical loot line
```

Namespaces: `underground_fuel_depot`-style (3 original zones) and `loc_*`-style (newer zones) installation ids both map onto the `loc_*` map-node namespace via `DamagedMapSystem.ResolveRevealNodeId` (exact match first, then `loc_` prefix). No stable id was renamed.
