# Plan 76 — Loot Authority Audit & Repair Log

## 1. Authority decision (§1.8 resolved)

The loot contract is **dual-mode with Plan 46 live**:

1. **`scavenging_table_id`** → `ScavengingTableCatalog.RollLoot` (authoritative,
   weighted, hazard-aware). 20 tables in `Assets/StreamingAssets/Data/scavenging_tables.json`.
2. **`lootCategories`** → used **directly as item ids** by
   `ExpeditionSystem.PickLootCategory` → `AddLoot` →
   `ExpeditionPanel` deposit → `InventoryHost.Add(itemId, qty)`.

They are **not** abstract tags (§1.9 resolved): an unresolved `lootCategories`
value produces a phantom inventory item with no catalog backing. The
pre-existing `docs/expeditions/LOOT_CATEGORY_ALLOWLIST.md` states the same
contract ("every entry in `lootCategories` must be a valid, existing `id` in
`items.json`").

## 2. Findings

Sweep of all 69 distinct `lootCategories` values across the 53 authored
destinations against the merged item catalogs (`items.json`,
`holdfast_items.json`, `year_of_ash_items.json`, `greenhouse_items.json`,
`crossing_items.json`, `verdict_items.json`, `dose_items.json`,
`foundry_items.json`, `chemical_dependency_items.json`,
`black_flotilla_items.json`):

- **66 / 69 resolve.** Note: item ids in this project are prefix-less
  snake_case (`bandage`, `dosimeter`), not `item_*`.
- **3 invalid values** (phantom-item risk on loot deposit):

| Invalid ref | Used by | Correct replacement (exists in `items.json`) |
|---|---|---|
| `bandages` | `loc_the_allotments`, `loc_settlement_pilgrim_hearth` | `bandage` |
| `food_rations` | `loc_the_allotments`, `loc_settlement_brine_pans` | `dried_rations` |
| `copper_wire` | `loc_denial_cut_substation`, `loc_settlement_tinkers_notch` | `copper_wire_10m_of_10m` |

- `bandages` exists only as an **economy goods id** (`economy_goods.json`) —
  a different namespace; not valid as a loot item id.
- `food_rations` exists in no catalog (items or goods).
- `copper_wire` exists only as the quantity-suffixed instance
  `copper_wire_10m_of_10m` ("Copper Wire (10m)").

All three replacements are on (or added to) the thematic allowlist:
`bandage` (Medical), `dried_rations` (Food), `copper_wire_10m_of_10m`
(Electrical — added by Plan 76).

## 3. Repairs applied

| File | Change |
|---|---|
| `Assets/StreamingAssets/Data/expeditions.json` | 5 destinations × 1 ref each (table above). Zero other field changes. |
| `src/Host/ExpeditionHostSession.cs` | Hardcoded no-catalog fallback definitions for the two original destinations mirrored the same invalid refs; repaired to the same values so the fallback cannot re-introduce phantoms. |

Justification for the host-code touch (Plan 76 §62 gate): the constants
duplicate the data authority verbatim; this is reference plumbing repair, not
new expedition gameplay logic.

## 4. Scavenging-table bindings (§29)

- 11 / 53 authored destinations carry `scavenging_table_id`; **all 11 resolve**
  (8 distinct tables used: farm, power_substation ×2, apartment_block,
  industrial_district, school ×2, warehouse, chemical_plant, hospital).
- 42 / 53 fall back to `lootCategories` (functional, deterministic).
- Binding the remaining 42 destinations to Plan 46 tables is a **content
  authoring migration** (new weighted tables per destination) and is recorded
  as deferred follow-up work, not a Plan 76 data defect. No table ids were
  invented to make destinations look richer (§1.10).

## 5. Regression gate added

`Ashfall.Core.Tests/Expeditions/Plan76DestinationLootReferenceTests.cs` (4 tests):

1. authored catalog loads at 53 with original-two parity;
2. every `lootCategories` value resolves against the merged item catalogs;
3. every `scavenging_table_id` resolves against `scavenging_tables.json`
   (and pins the binding count at 11);
4. the three repaired refs can never regress.
