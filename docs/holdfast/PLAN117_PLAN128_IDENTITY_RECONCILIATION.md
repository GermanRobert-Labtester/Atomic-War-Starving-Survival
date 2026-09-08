# Plan 117 & Plan 128 Identity Reconciliation

## 1. Context & Architecture Alignment
Plan 117 introduces Holdfast quests, detailing missions across the ice road, Sector 4, the estuary shelf, and District 8. Quests in `holdfast_quests.json` describe operations involving the coastal economy.

Plan 128 expands the transactional voice profiles in `holdfast_flavor.json`.

## 2. Shared Lore & Institutional Reconciliation

| Entity / Character in Plan 117 | Canonical Faction ID | Plan 128 Flavor Binding | Narrative Relationship |
|---|---|---|---|
| **Ivy Corrigan** (Ice Pilot, Kilometre 19) | `faction_the_cutters` | `faction_the_cutters` | Ivy Corrigan lights the lamp at KM 19 under Cutter authority (`role: "Lamplighter"` in `holdfast_npcs.json`). The Cutters handle road maintenance and ice pilotage. |
| **Edor Vale** (Census Clerk Grade III) | `faction_the_office` | `faction_the_office` | Edor collects census returns and inspects triplicate sheets at the weighbridge under Office scheduling. |
| **Bram Ostrowski** (Waxed map vendor) | `faction_the_cutters` / `the_cut` | `faction_the_cutters` | Operates around the Cut and sells charts showing summer water turned to ice. |
| **Sparks Halden Mire** (Radio operator) | `faction_the_fleet` | `faction_the_fleet` | Maintains Fleet pad copies, communications, and berth schedules on the shelf. |
| **Shore Party / Divers** | `faction_black_flotilla` | `faction_black_flotilla` | Runs marine salvage operations off coastal shelf intake caissons and tidal wrecks. |
| **District 8 Storekeeper** | `faction_supply_corps` | `faction_supply_corps` | Issues rationing chits, fuel advances, and medical tabs in the northern shelter sectors. |
| **Southern Line Engineer** | `faction_railway_guild` | `faction_railway_guild` | Manages locomotive fuel, rail spurs, and carriage over the southern freight tracks. |

## 3. Schema Non-Invasion Guarantee
- No `faction_id` property was added to `HoldfastQuestEntry` or `holdfast_quests.json` (respecting Invariant 5 and Plan 128 Section 13.1).
- Quests route player actions to locations where counterparty trade operates under canonical faction identities.
