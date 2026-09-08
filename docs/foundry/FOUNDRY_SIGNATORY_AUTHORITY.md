# Foundry Signatory Authority & Faction Identity Mapping

**Accords File:** `Assets/StreamingAssets/Data/foundry_accords.json`
**Faction Catalogs:** `Assets/StreamingAssets/Data/foundry_faction.json`, `Assets/StreamingAssets/Data/holdfast_factions.json`, `Assets/StreamingAssets/Data/currents.json`, `Assets/StreamingAssets/Data/crossing_factions.json`, and the existing regional accord roster.

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
| The Scale | `faction_the_scale` | The Scale | `crossing_factions.json` | Commercial arbitration guild operating certified balances, scrap sorting, and the Caravanserai. |
| The Archivists | `faction_archivists` | The Archivists of the Before | `currents.json` / `foundry_faction.json` | Custodian of witnessed copies and named incident records. |
| The Grain Exchange | `faction_grain_exchange` | The Grain Exchange | `currents.json` / `foundry_faction.json` | Bulk food market able to settle bounded emergency relief costs. |
| The Hydro Barons | `faction_hydro_barons` | The Hydro Barons / Coastal Hydro-Barons | `foundry_faction.json` / `currents.json` | Water-queue authority; a rival with a reason to sign measured intake terms. |
| The Scavenger Guild | `faction_scavenger_guild` | The Scavenger Guild | `currents.json` / `foundry_faction.json` | Claim registry and apprenticeship counterparty for supervised salvage training. |

---

## 2. Invariants

- Zero inferred or guessed IDs.
- Zero `"all_factions"` wildcards.
- Every signatory is bound by real economic necessity, geographic adjacency, and institutional capacity.

## 3. Plan 102 Resolution Rules

- The Foundry is an explicit signatory because the live baseline accords include
  `faction_silent_foundry`; `Cluster` is a region/institutional constituency,
  not a guessed faction ID.
- The six new records use only IDs resolved in the canonical faction files or
  already present in the authority's regional accord roster.
- The Crisis Mutual-Aid Protocol enumerates eight eligible signatories. It does
  not use an `all_factions` wildcard and intentionally excludes the Iron Raiders
  from a bounded cooperative pact.
- `SilentFoundryCatalogLoader` and `RegionalTreatyCatalog` do not perform this
  cross-file resolution at runtime; the authority mapping is enforced by the
  Plan 102 xUnit matrix and the headless roster check.
