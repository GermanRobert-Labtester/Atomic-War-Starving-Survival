# Standing Record Faction Collision & Namespace Reconciliation Audit

## 1. Overview

Before expanding `standing_record_factions.json`, a comprehensive cross-repository survey was executed across all existing faction catalogs:
- `Assets/StreamingAssets/Data/factions.json` (legacy / global registry)
- `Assets/StreamingAssets/Data/holdfast_factions.json` (Holdfast terminal trade factions)
- `Assets/StreamingAssets/Data/crossing_factions.json` (Nobodys Charter / Stallrow market factions)
- `Assets/StreamingAssets/Data/foundry_faction.json` (Silent Foundry production & accord ecology)
- `Assets/StreamingAssets/Data/faction_lore.json` (Canonical lore & ideological definitions)
- `Assets/StreamingAssets/Data/narrative/wasteland_settlement_gazetteer.json` (Settlement authorities)
- `Assets/Ashfall.Core/UI/FactionIconCatalog.cs` (Canonical emblem mappings)

---

## 2. Namespace Collision Matrix

| Proposed ID | Existing Repositories Where Present | Canonical Organization Identity | Reconciliation Decision |
|---|---|---|---|
| `faction_the_overlay` | `standing_record_factions.json` | The Overlay (Cadastral and surveying order) | **Preserved Baseline** verbatim. |
| `faction_the_scale` | `crossing_factions.json`, `FactionIconCatalog.cs`, `WASTELAND_REGION_ATLAS.md` | The Scale (Water-accounting and weighbridge authority) | **Adopted**: Exact semantic and ID match with Crossing / Regional Atlas. |
| `faction_the_compact` | `crossing_factions.json`, `FactionIconCatalog.cs` | The Compact (Arbitration and treaty board) | **Adopted**: Grounded in boundary and deed management. |
| `faction_the_underwrite` | `crossing_factions.json`, `FactionIconCatalog.cs` | The Underwrite (Actuarial and security syndicate) | **Adopted**: Protects fuel depot and convoy risk. |
| `faction_the_cutters` | `holdfast_factions.json`, `FactionIconCatalog.cs` | The Cutters (Road and ice-pass maintenance guild) | **Adopted**: Controls heavy transit chokepoints. |
| `faction_the_fleet` | `holdfast_factions.json`, `FactionIconCatalog.cs` | The Fleet (Coastal and river barge collective) | **Adopted**: Distinct from Black Flotilla (salvage privateers); focuses on working waterway transport. |
| `faction_the_rebuilders` | `faction_lore.json` (`faction_rebuilders`), `WASTELAND_REGION_ATLAS.md` | The Rebuilders (Municipal agricultural continuity) | **Aligned**: Adopted `faction_the_rebuilders` within the unified `faction_the_*` convention. |
| `faction_the_garrison` | `wasteland_settlement_gazetteer.json`, `SettlementCatalogTests.cs` | The Garrison (Fort Karkov rail and checkpoint command) | **Aligned**: Preserves Fort Karkov controlling authority from Settlement gazetteer. |

---

## 3. The Garrison Disambiguation

The term "Garrison" appears in multiple contexts within the ASHFALL setting:
1. `faction_central_garrison` / `iron_garrison`: The pre-war military command under Colonel Voss / Lt. Col. Harven centered at Bunker Sigma-7 and Sector 4 capital.
2. `faction_the_garrison`: The fortified rail-marshalling yard and perimeter checkpoint authority at Fort Karkov (`settlement_01_fort_karkov_rail_garrison`), commanded by Major Volkov.

In Standing Record, `faction_the_garrison` represents the operational checkpoint authority controlling physical highway access, rail bastions, and transit chits across the Ash Flats.

---

## 4. The Fleet Disambiguation

Maritime factions were audited against Plan 23:
1. `faction_black_flotilla`: Armed deep-salvage privateers and coastal raiders operating offshore.
2. `faction_the_fleet`: Coastal river-dock and shallow-draft barge guild operating working wharves (Berth 9), tidal ferries, and dredge lighters along the coastal shelf.
