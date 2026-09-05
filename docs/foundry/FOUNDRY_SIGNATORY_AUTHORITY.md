# Foundry Signatory Authority & Faction Identity Mapping

**Accords File:** `Assets/StreamingAssets/Data/foundry_accords.json`
**Faction Catalogs:** `Assets/StreamingAssets/Data/foundry_faction.json`, `Assets/StreamingAssets/Data/holdfast_factions.json`, `Assets/StreamingAssets/Data/faction_territory.json`

---

## 1. Signatory Authority Directory

Every faction appearing in `signatory_factions` across `foundry_accords.json` is audited below against canonical game data.

| Conceptual Label | Canonical Faction ID | Display Name | Canonical File | Diplomatic Role in Accords |
|---|---|---|---|---|
| The Foundry | `faction_silent_foundry` | The Silent Foundry | `foundry_faction.json` | Industrial metalcasting works of District 8; primary caster of pipe, anchors, and structural iron. |
| The Office | `faction_the_office` | The Office | `holdfast_factions.json` | Civil administration of the Cluster; manages iodine allocation, health boil orders, and district census. |
| The Cutters | `faction_the_cutters` | The Cutters | `holdfast_factions.json` | Ice-road maintainers, winch operators, and salvage sorters controlling passage across the Cut. |
| The Fleet | `faction_the_fleet` | The Fleet / Black Flotilla | `holdfast_factions.json` | Maritime transport, lock gate operation, and deep salvage operators along the coastal shelf. |
| Central Garrison | `faction_central_garrison` | The Central Garrison | `faction_lore.json` | Military continuity force guarding Checkpoint Gamma and the Eastern Arterial Road. |
| The Rebuilders | `faction_rebuilders` | The Rebuilders | `faction_directives_and_notices.json` | Civilian agrarian collective cultivating the Verge and maintaining pump gaskets. |
| Ash Sign | `faction_ash_sign` | The Ash Sign | `faction_lore.json` | Mountain monastic order guarding high cairns, switchback steps, and the Summit Relay. |
| Forward Roster | `faction_forward_roster` | The Forward Roster | `faction_territory.json` | Armed border defense militia observing the 5km Neutral Ground buffer opposite Garrison. |
| The Scale | `faction_the_scale` | The Scale | `faction_territory.json` | Commercial arbitration guild operating certified balances, scrap sorting, and the Caravanserai. |

---

## 2. Invariants

- Zero inferred or guessed IDs.
- Zero `"all_factions"` wildcards.
- Every signatory is bound by real economic necessity, geographic adjacency, and institutional capacity.
