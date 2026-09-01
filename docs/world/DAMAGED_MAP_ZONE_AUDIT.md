# Damaged Map Zone Audit & Fragment Assembly — Plan 11

> **Document Class:** Cartographic & Zone Exploration Audit
> **Authority:** `Assets/StreamingAssets/Data/damaged_map_zones.json`
> **Systems:** `WastelandMapSystem`, `LocationMemorySystem`, `ExpeditionSystem`

---

## 1. Executive Summary

`damaged_map_zones.json` contains pre-war fragments that reveal hidden underground installations once fully assembled by the player during wasteland expeditions.

---

## 2. Damaged Map Zone Inventory

| Zone ID | Zone Name | Total Fragments | Hidden Installation ID | Installation Name | Revealed Key Rewards |
|---|---|---|---|---|---|
| `industrial_district` | Industrial District | 3 | `underground_fuel_depot` | Underground Fuel Depot | `diesel_fuel`, `generator_parts` |
| `suburban_heights` | Suburban Heights | 2 | `municipal_seed_vault` | Municipal Seed Vault | `seed_packets`, `heirloom_seeds`, `growing_manual` |
| `military_corridor` | Military Supply Corridor | 3 | `blacksite_armory_7` | Blacksite Armory 7 | `faraday_pack`, `military_radio`, `night_vision_scope`, `ammo_308` |

---

## 3. Fragment Data & Lore Table

| Fragment ID | Zone | Label | Description |
|---|---|---|---|
| `damaged_map_industrial_1` | Industrial District | Northern Sector | Shows the factory district north of the river. Burn damage obscures the eastern edge. |
| `damaged_map_industrial_2` | Industrial District | Sewer Grid | Pre-war sewer system map. Junction at sector C-7 is circled in red ink. |
| `damaged_map_industrial_3` | Industrial District | Fuel Reserve Key | Key card with access code: "DEPOT — EMERGENCY ONLY". |
| `damaged_map_suburban_1` | Suburban Heights | Library Blueprint | Architectural plans showing basement level missing from public records. |
| `damaged_map_suburban_2` | Suburban Heights | Librarian's Note | "Vault code is the date of the first seed catalog. Ask Margaret. She'll remember." |
| `damaged_map_military_1` | Military Corridor | Radar Station Exterior | Survey map of the old radar station marked "condemned" with active substructure. |
| `damaged_map_military_2` | Military Corridor | Elevator Access | Service elevator schematic with digits 7, 4, 9 legible. |
| `damaged_map_military_3` | Military Corridor | Supply Manifest | Quartermaster manifest: "Armory 7 — Full Stock. Code: CHIMERA-7-4-9." |

---

## 4. Integration with Plan 11
- Zone fragments drop deterministically from expedition scouting and deep-strata excavation chambers.
- Completing a zone's fragments reveals the installation node on `WastelandMapSystem` via `Discover(installationId)`.
- All unlocked installations persist across saves.
