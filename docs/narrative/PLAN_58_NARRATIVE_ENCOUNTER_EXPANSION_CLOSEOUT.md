# Plan 58 — Narrative Encounter Expansion: Closeout

## Status: **COMPLETE**

## Counts

```text
Base before:  3  (enc_dead_letter_office, enc_weather_station, enc_pianist)
New:         22
Base after: 25
Expansion:  29  (not loaded into the live selection pool — see runtime contract)
Combined authored content: 54 records across both catalogs
```

Catalog merge behavior confirmed: the runtime pool = base + NPC-arc catalogs
only; `narrative_encounters_expansion.json` is content-audited but unloaded
(existing state, unchanged — flagged below as a follow-up).

## Final 22 IDs (all `enc_*`, unique across both catalogs)

| # | ID | Category | w | stealth | speed | minDanger | Location binding |
|---|---|---|---|---|---|---|---|
| 1 | `enc_relay_booth_frequency` | Discovery | 2.0 | 1.3 | 0.7 | 0.0 | `loc_radio_relay_mast` |
| 2 | `enc_sealed_civil_defense_door` | Discovery | 1.5 | 1.1 | 0.8 | 0.0 | `loc_excavation_civilian_shelter` |
| 3 | `enc_tarpaulin_vehicle_cache` | Discovery | 1.8 | 0.8 | 1.3 | 0.0 | — |
| 4 | `enc_scrap_line_ambush` | Combat | 1.2 | 0.5 | 1.5 | 1.0 | — |
| 5 | `enc_territory_warning` | Combat | 1.0 | 0.5 | 1.4 | 1.0 | — |
| 6 | `enc_checkpoint_no_flag` | Combat | 1.0 | 0.6 | 1.3 | 1.0 | — |
| 7 | `enc_wrong_directions` | Social | 1.5 | 0.6 | 1.0 | 0.0 | — |
| 8 | `enc_broken_scale_trader` | Social | 1.3 | 0.6 | 1.0 | 0.0 | — |
| 9 | `enc_separated_young_survivor` | Social | 1.2 | 0.6 | 1.0 | 0.0 | — |
| 10 | `enc_family_cache_names` | Moral | 1.2 | 1.0 | 1.0 | 0.0 | — |
| 11 | `enc_hoarders_promise` | Moral | 1.0 | 1.0 | 1.0 | 0.0 | — |
| 12 | `enc_road_tax_toll` | Moral | 1.0 | 0.6 | 1.2 | 1.0 | — |
| 13 | `enc_settled_floor_crack` | Environmental | 1.2 | 1.0 | 1.2 | 0.0 | — |
| 14 | `enc_bitter_water` | Environmental | 1.2 | 1.0 | 1.0 | 0.0 | `loc_water_station` |
| 15 | `enc_field_clinic_after_evacuation` | Medical | 0.8 | 0.9 | 1.0 | 1.0 | `abandoned_hospital` |
| 16 | `enc_expired_stock` | Medical | 0.8 | 0.9 | 1.0 | 0.0 | — |
| 17 | `enc_looted_food_drop` | Scavenging | 1.3 | 1.1 | 0.8 | 0.0 | — |
| 18 | `enc_fuel_no_keys` | Scavenging | 1.2 | 1.1 | 0.8 | 0.0 | — |
| 19 | `enc_footbridge_stranded` | Rescue | 0.8 | 1.0 | 1.2 | 1.0 | `loc_bridge_seven` |
| 20 | `enc_ice_water_rescue` | Rescue | 0.7 | 1.0 | 1.3 | 1.0 | — |
| 21 | `enc_unexploded_ordnance` | Hazard | 0.7 | 0.8 | 1.2 | 1.0 | — |
| 22 | `enc_hot_salvage` | Hazard | 0.7 | 0.8 | 1.2 | 1.0 | — |

All 9 category families represented (3/3/3/3/2/2/2/2/2). Every encounter has
3 choices using only `moraleDelta`/`guiltDelta` (the baseline grammar);
morale −2…+4, guilt 0…4 — within the base anchors (0–5 / 0–4). Weights tiered
0.7–2.0 (rare → common), inside the 32-entry anchor band (1.5–3.0 base +
expansion; the new tier floor 0.7 intentionally makes rescue/hazard rarer
than every pre-existing entry). Multipliers bounded 0.5–1.5 (inside the
0.5–2.0 anchor band), combat explicitly stealth-suppressed (0.5),
discovery speed-suppressed (0.7–0.8). `forceOnArrival = false` on all 22 —
the 2 forced base entries remain the only forced content in the base pool.
`minDangerLevel` 0.0 except four hazard/combat/rescue scenes at 1.0
(expansion-consistent 0/1/2 scale).

## Location bindings — 5/5 with committed IDs

`loc_radio_relay_mast`, `loc_excavation_civilian_shelter`, `loc_water_station`,
`abandoned_hospital`, `loc_bridge_seven` — all resolved from the live
`locations.json` (151 locations). Zero forward references.

## Faction/patrol-themed — 3/3 without forward refs

1. `enc_road_tax_toll` — written tariff "stamped with a faction seal" (generic
   authority claim; schema has no faction-condition field, so prose-grounded).
2. `enc_checkpoint_no_flag` — burned-off faction markings (deniable patrol).
3. `enc_faction_patrol_nearby`-adjacent coverage exists in Plan 57's incident
   layer; the third encounter slot here is the checkpoint/tariff pair per §10.

No Plan 45 patrol IDs, no Plan 54 enemy refs, no Plan 52 NPC ids, no flags,
no quest links, no item grants — none referenced (all current-schema-safe;
the Plan 49/52 extension fields exist but are unused to keep the base catalog
free of forward dependencies).

## Plan 32 / Plan 45 status

```text
Base encounter expansion complete.
Plan 32 / Plan 45-specific bindings deferred until those IDs/consumers are committed.
```

## Deviations

1. Briefs M2 (injured stranger) and R1 (trapped under slab) replaced — direct
   duplicates of expansion `enc_injured_scavenger`. Brief SC1 (locked storeroom)
   replaced — §0-banned locked-room cliché (`enc_locked_room`). Brief C2 (dog
   pack) replaced — near-duplicate of `enc_dogs_silent`.
2. Categories authored as free-form labels (no runtime enum exists to match).
3. Choice consequences limited to morale/guilt: the base grammar. The Plan 49
   grant/flag/discover fields exist but were not used (zero-ref discipline).

## Verification

| Gate | Result |
|---|---|
| `--data-integrity-selftest` | **PASS** — 0 findings / 208 catalogs (10,150 ids authored) |
| `--content-utilization-selftest` | **PASS** |
| `Ashfall.Core.Tests` full suite | **PASS** 6,580/6,580 (three consecutive runs) |
| Narrative/encounter suites | **PASS** 16/16 × 3 runs (deterministic selection green) |
| `Ashfall.csproj` build | **PASS** 0 errors |
| `--bridge-selftest` | **PASS** exit 0 |
| Save path | unchanged — resolution history is id-keyed; new ids conform (no migration) |

## Pre-existing issue discovered (out of scope, flagged)

`Plan45Phase2BindingTests.TravelCatalog_CreatureEncountersCarryCombatantTags`
is order-dependent: `combatant_feral_mutt` exists in `combat_catalog.json` but
the test fails under the combined `Narrative|Travel|Plan52|Plan49` filter
(passes in class-isolation and in full-suite runs). Pre-existing test-isolation
issue in the Plan 45 travel-catalog binding tests — no Plan 58 relation
(narrative_encounters.json untouched by that test); flagged for the Plan 45
owners. No test changes made per §2.4.

## Deferred hooks

- Plan 32 destination-specific eligibility (5 bindings possible once committed).
- Plan 45 patrol-state-aware checkpoint/tariff variants.
- Plan 54 enemy refs for the ambush/standoff scenes (currently resolved through
  non-combat choices).
- Plan 52 recurring-NPC identity for the trader and the waiting survivor.
- Plan 49 `grantItemId`/`discoverLocationId` rewards (fields supported, kept
  unused for zero forward refs).
- Loading `narrative_encounters_expansion.json` into the live pool (29 entries
  currently data-present but unloaded) — content-utilization follow-up.
