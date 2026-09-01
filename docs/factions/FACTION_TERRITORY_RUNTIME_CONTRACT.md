# FACTION TERRITORY RUNTIME CONTRACT (Plan 44)

> **Authority:** `Assets/StreamingAssets/Data/faction_territory.json`
> **Core Engine Type:** `Ashfall.Core.World.FactionTerritoryCatalog` (`Assets/Ashfall.Core/World/FactionTerritoryCatalog.cs`)
> **Schema Version:** `1`

---

## 1. System Identity & Support Classification

- **Support Level:** Level 1 (Authoritative Data Catalog & Core Loader). Full gameplay runtime consumption (dynamic territory decay, caravan tariff levying, combat encounter modification, and faction war territory flips) is decoupled and scheduled for Plans 45, 16B, 40, and W43.
- **Catalog Path:** `Assets/StreamingAssets/Data/faction_territory.json`
- **Prefix Namespaces:**
  - `territory_` — Recognized by `CatalogIntegrityValidator` Tier-1 registry.
  - `zone_contested_` — Recognized by `CatalogIntegrityValidator` Tier-1 `zone_` namespace.
- **Root Object Contract:**
  - `schema_version`: `1`
  - `collection_id`: `"faction_territory_catalog"`
  - `territories`: Array of 19 `FactionTerritoryDef` records.
  - `contested_zones`: Array of 5 `ContestedZoneDef` records.

---

## 2. Data Structure Specification

### `FactionTerritoryDef` DTO

| Field | Type | Description |
|---|---|---|
| `id` | `string` | Canonical territory ID (`territory_*`). |
| `faction` | `string` | Controlling faction ID (`faction_*`), resolves to `currents.json` / `holdfast_factions.json`. |
| `display_name` | `string` | Narrative title of the territory / district. |
| `classification` | `string` | Geographic mobility classification: `"territorial"`, `"nomadic"`, `"ideological"`, `"mixed"`. |
| `territory_scale` | `string` | Scale of physical presence: `"major"`, `"medium"`, `"minor"`, `"none"`. |
| `primary_resource_interest` | `string` | Strategic material anchor justifying control. |
| `controlled_nodes` | `List<string>` | Traversable map node IDs from `wasteland_map_v1.json`. |
| `control_points` | `List<string>` | Physical location IDs from `locations.json` anchoring presence. |
| `contested_with` | `List<string>` | Rival faction IDs claiming or disputing this territory. |
| `control_strength` | `int` | Initial baseline authority (0–100). |
| `trade_tax` | `float` | Authoritative caravan transit tariff rate (0.00–0.20). |
| `travel_safety` | `float` | Base transit safety rating (0.00–1.00). |
| `shift_trigger` | `string` | Canonical flag or event causing territorial realignment. |
| `description` | `string` | Diegetic description of the territory and its defensive features. |

### `ContestedZoneDef` DTO

| Field | Type | Description |
|---|---|---|
| `id` | `string` | Canonical zone ID (`zone_contested_*`). |
| `name` | `string` | Narrative name of the contested geographic flashpoint. |
| `strategic_value` | `string` | Core resource or tactical asset at stake. |
| `focal_node_id` | `string` | Map node ID where dispute is centered. |
| `focal_location_id` | `string` | World location ID anchoring the physical dispute. |
| `claimant_factions` | `List<string>` | Array of at least 2 rival faction IDs contesting control. |
| `conflict_driver` | `string` | Narrative and economic driver of the dispute. |
| `hazard_rating` | `int` | Environmental / combat hazard severity (1–5). |
| `dispute_intensity` | `int` | Flashpoint volatility and skirmish frequency (0–100). |

---

## 3. Downstream System Interfaces

```text
faction_territory.json (Data Authority)
  │
  ├──► Plan 45 (Faction Patrol Encounters)
  │      └── Reads: control_strength, control_points, travel_safety
  │
  ├──► Plan 16B (Caravan Economy & Trade Routes)
  │      └── Reads: trade_tax, controlled_nodes, controlling faction
  │
  ├──► Plan 06C (Faction War & Border Conflicts)
  │      └── Reads: contested_zones, claimant_factions, shift_trigger
  │
  ├──► Plan 40 (Debt & Consequence Dispatcher)
  │      └── Reads: territory_id, control_strength degradation on default
  │
  └──► Plan 43 (Settlements & Inhabitants)
         └── Reads: control_points (anchoring settlement allegiance)
```
