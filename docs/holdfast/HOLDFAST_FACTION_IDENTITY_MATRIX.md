# Holdfast Faction Identity Matrix

## 1. Candidate Evaluation & Classification

| Planning Candidate | Classification | Disposition | Technical Justification |
|---|---|---|---|
| **The Lamplighters** | Class B (Sub-unit in Holdfast / Class A in Crossing) | Substituted | In Holdfast, Ivy Corrigan is a Cutter (`npc_ivy_corrigan`: `faction_id: "faction_the_cutters"`, role: Lamplighter) maintaining Sector 4 / Kilometre 19 markers. In Crossing, `faction_the_lamplighters` is a distinct municipal lighting guild. Replaced by `faction_hydro_barons` in Holdfast dispatch trade. |
| **Estuary Camp** | Class C (Location) | Substituted | `loc_estuary_camp` / Brine-Pan Hollow is a physical settlement / evaporation basin in Sector 8, not a faction. Substituted by `faction_supply_corps` (the canonical District 8 allocation bureau). |
| **Kittiwake** | Class D (Vessel) | Substituted | `Survey Launch Kittiwake` is a flood-grounded launch with sonar rig and logbook, not a faction. Substituted by `faction_black_flotilla` (the active maritime salvage faction on the coastal shelf). |
| **Ice Road Guild** | Class E (Planning duplicate) | Substituted | The ice road is operated and piloted by The Cutters (`faction_the_cutters`). Creating a duplicate guild would violate Invariant 3.5. Substituted by `faction_railway_guild` (the southern rail transport authority). |
| **Quarantine Post** | Class C/E (Checkpoint / State) | Substituted | In Crossing, `faction_the_quarantine_post` is a gate clinic. In Holdfast, quarantine is a procedure on Block C and health orders under The Office. Substituted by `faction_ordnance_foundry` (the heavy industrial tooling and munitions forge). |

## 2. Final Eight Factions Roster

| Faction ID | Display Name | Source Catalog | Home Region | Register | Dispatch Call Site |
|---|---|---|---|---|---|
| `faction_the_office` | The Office | `holdfast_factions.json` | `the_cluster` | `bureaucratic` | `HoldfastTerminalPanel` |
| `faction_the_cutters` | The Cutters | `holdfast_factions.json` | `the_cut` | `salvage` | `HoldfastTerminalPanel` |
| `faction_the_fleet` | The Fleet | `holdfast_factions.json` | `the_shelf` | `maritime` | `HoldfastTerminalPanel` |
| `faction_black_flotilla` | The Black Flotilla | `holdfast_factions.json` | `coastal_shelf` | `privateer` | `HoldfastTerminalPanel` |
| `faction_supply_corps` | The Supply Corps | `holdfast_factions.json` | `district_8` | `allocation` | `HoldfastTerminalPanel` |
| `faction_railway_guild` | The Railway Guild | `holdfast_factions.json` | `the_rail_south` | `logistics` | `HoldfastTerminalPanel` |
| `faction_hydro_barons` | The Hydro Barons | `holdfast_factions.json` | `the_aquifer` | `monopoly` | `HoldfastTerminalPanel` |
| `faction_ordnance_foundry` | The Ordnance Foundry | `holdfast_factions.json` | `the_foundry_yard` | `foundry` | `HoldfastTerminalPanel` |

### Compatibility roster entry

`faction_scavengers` is also present in `holdfast_factions.json` because older
trade, travel, and verdict content already selects it. It is not a ninth
authored Holdfast identity and has no specialized `holdfast_flavor.json`
profile; dispatch resolves it through the documented neutral fallback.
Removing it would break existing cross-expansion references, while promoting
it into the eight-profile identity matrix would change the Plan 128 contract.
