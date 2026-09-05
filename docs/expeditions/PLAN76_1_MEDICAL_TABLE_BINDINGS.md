# Plan 76.1 — Medical Family Scavenging-Table Bindings

First family of the 42-destination `lootCategories`→Plan 46 table migration
(from `PLAN76_LOOT_AUTHORITY_AUDIT.md` §4). Scope: the five medical-signature
destinations. Follow-up families: mechanical/fuel, electrical, household,
administrative, military, settlement, deep/endgame.

## Reuse-first decisions

| Destination | Binding | Rationale |
|---|---|---|
| `abandoned_hospital` | `table_loot_hospital` (existing) | Exact match — collapsed hospital wing, sealed medicine rooms. |
| `hospital_pharmacy` | `table_loot_hospital` (existing) | Same building as the hospital; the table already carries pharmacy-side entries (chelation pellets, HEPA, triage documents). |
| `loc_veterinary_surgery` | **`table_loot_veterinary_surgery`** (new) | Large-animal theatre identity (hoist, cut-open drug cabinet, livestock stocks) — no existing table covers it. |
| `loc_dentists_row` | **`table_loot_dentists_row`** (new) | Dental-practice identity (chairs, drills, methodical stripping) — no existing table covers it. |
| `loc_st_brigids_almshouse` | **`table_loot_hospice_ward`** (new) | Hospice/dormitory identity (made beds, lockers, charts filled to one date) — distinct from the outpatient `table_loot_clinic`. |

No new item ids were invented (Plan 76 §1.10). Every `item_id` resolves against
the merged item catalogs; the only `codex_unlock_id` values used are existing
ones (`codex_surgical_log`, `codex_unsent_letter`).

## New table signatures (Plan 76 §30)

| Table | Entries | Total weight | Common share | Base hazard | Signature |
|---|---:|---:|---:|---|---|
| `table_loot_veterinary_surgery` | 14 | 226 | 81% | disease 0.10 | sutures, splints, sedative draught, forceps, surgical steel |
| `table_loot_dentists_row` | 11 | 158 | 88% | chemical 0.05 | small-yield relief: painkillers, forceps/tweezers, masks, sterilizer chemicals |
| `table_loot_hospice_ward` | 14 | 222 | 83% | disease 0.15 | palliative stock: painkillers, sedative draught, wool blankets, last letters |

Balance notes (§53): all three are `finite` per Plan 46 convention for ruins;
common-heavy distributions match their danger tiers (4/5/7); no table makes
medicine trivially abundant — the rare surgical/kit entries stay rare, and the
almshouse's high base hazard prices its good stock.

## Binding count progression

11 → **16** of 53 authored destinations bound. Remaining unbound: **37**
(pin updated in `Plan76DestinationLootReferenceTests`).

## Verification (all PASS)

- `dotnet build Ashfall.csproj` — 0 errors, 0 warnings
- Scoped tests (Plan 76 + Plan 32 + expedition system/vehicles + scavenging):
  65 / 65 PASS
- `godot --headless --path . -- --data-integrity-selftest` — 0 findings
- `godot --headless --path . -- --expedition-selftest` — 19/19
- `godot --headless --path . -- --content-utilization-selftest` — CI gate PASS
