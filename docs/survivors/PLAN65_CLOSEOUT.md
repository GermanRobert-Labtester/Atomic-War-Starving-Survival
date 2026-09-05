# Plan 65 — Final Wishes Expansion: Closeout

## Status: **COMPLETE** (count-reconciliation variant)

## Counts

```text
Baseline:    10  (plan stated 8 — stale; the file family held 10 across
                  10 archetype ids: surgeon, soldier, nurse, mother, mechanic,
                  teacher, refugee, electrician, quartermaster, miner)
New:         22  (exactly the plan's requested additions)
Final:       32  — 32 unique archetype ids, 32 unique titles
```

## Runtime contract (verified in `FinalWishSystem.cs`)

- **Selection:** `DeclareTerminalPrognosis(survivorId, archetypeId, isAlive)`
  → wish type from `_archetypeWishes[archetypeId]` (registered via
  `RegisterWish`) → archetype-prefix fallback (surgeon/nurse→teach_lesson,
  soldier/guard→build_memorial, parent/mother→reconcile) → default
  `deliver_letter`. **One wish type per archetype.**
- **Steps:** a counter (`stepsCompleted`) — `AdvanceWishStep` increments and
  fires `OnFinalWishStepCompleted`; hardcoded per-type required steps
  (see_the_sky=1, build_memorial=3, retrieve_heirloom/deliver_letter/
  teach_lesson/reconcile=2, unknown types=2). The JSON's
  `required_items`/`requires_patient` fields are **host-gating hints**, not
  runtime-evaluated objectives.
- **Consequences:** hardcoded — completion = +15 permanent shelter morale
  (`their_memory_lives_on`); prognosis expiry = −10. Per-wish
  `morale_bonus`/`completion_text` are display data.
- **Prognosis clock:** `Tick` decrements `daysRemaining` (3–7 default);
  expiry fails the wish.
- **Save:** `FinalWishSaveState` (per-survivor states + archetype→wish map) —
  id-keyed, additive-safe. **No new save fields; no schema change.**

## Type mapping (requested → authored)

| Requested type | Authored type (runtime) | Steps | Count |
|---|---|---|---|
| teach_lesson | `teach_lesson` | 2 | 3 |
| deliver_letter | `deliver_letter` | 2 | 3 |
| see_a_place | `see_the_sky` (exact semantic match) | 1 | 2 |
| reconcile | `reconcile` | 2 | 2 |
| die_with_dignity | `die_with_dignity` (data precedent; runtime default 2) | 2 | 3 |
| last_meal | `last_meal` (free-string type; runtime default 2) | 2 | 2 |
| confess | `confess` (default 2) | 2 | 2 |
| protect_someone | `protect_someone` (default 2) | 2 | 2 |
| return_a_relic | `retrieve_heirloom` (exact semantic match) | 2 | 2 |
| name_a_successor | `name_a_successor` (default 2) | 2 | 1 |

All types are legal: the runtime accepts free-string types (the switch's
`_ => 2` branch), and the four new type strings follow the same snake_case
convention as the six named constants. No Core change was made for
vocabulary.

## The 22 new wishes

| Archetype | Type | Title | Required items (canonical) |
|---|---|---|---|
| `the_hunter` | teach_lesson | Reading Snow | `trap_improvised_wire` |
| `the_carpenter` | teach_lesson | Where the Weight Sits | `box_of_nails_10` |
| `the_radio_operator` | teach_lesson | Keep the Watch | — |
| `the_courier` | deliver_letter | The Last Run | — |
| `the_preacher` | deliver_letter | To the Flock, Signed | `item_document_field_report` |
| `the_reporter` | deliver_letter | Attribution | `item_document_field_report` |
| `the_hermit` | see_the_sky | The Whole Sky | — |
| `the_farmer` | see_the_sky | Standing Soil | `crop_leafy_green` |
| `the_convict` | reconcile | The Sentence Served | — |
| `the_neighbor` | reconcile | The Fence Line | — |
| `the_undertaker` | die_with_dignity | Professionally, Then Privately | — |
| `the_monk` | die_with_dignity | The Last Office | `item_dried_herb_packets` |
| `the_watchmaker` | die_with_dignity | Wound Once More | — |
| `the_cook` | last_meal | The Kitchen Scale | `crop_tuber`, `crop_leafy_green`, `clean_water` |
| `the_chef` | last_meal | What the Sea Used to Do | `item_preservation_salt`, `crop_leafy_green` |
| `the_arsonist` | confess | Eleven Fires | `item_document_field_report` |
| `the_executive` | confess | The Distribution Order | `item_document_field_report` |
| `the_caregiver` | protect_someone | Her Night Rounds | — |
| `the_guard` | protect_someone | The Watch After | — |
| `the_collector` | retrieve_heirloom | The Wrong Shelf | `item_preservation_salt` |
| `the_archivist` | retrieve_heirloom | The Seized Files | `item_document_field_report` |
| `the_foreman` | name_a_successor | The Order of Work | `box_of_nails_10` |

All 22 `archetype_id`s follow the established `the_*` convention and map to
the survivor roster's profession vocabulary (hunter, carpenter, radio
operator, courier, preacher, reporter, hermit, farmer, convict, neighbor,
undertaker, monk, watchmaker, cook, chef, arsonist, executive, caregiver,
guard, collector, archivist, foreman). All required items resolve in the
merged item catalog. Dignity content is privacy/ritual/witness-based — no
procedural content (§2.10 honored).

## Cross-system handoffs (as executed within the runtime contract)

- **NPC/relationship (5+, via narrative + the host's `GetWishNarrativeText`/
  `RegisterWish` surface):** the convict's reconciliation, the neighbor's
  fence line, the caregiver's night rounds, the guard's watch handover, the
  foreman's signed order of work — durable relationship/legacy consequences
  expressed through the runtime's completion events
  (`OnFinalWishCompleted` → shelter morale + narrative event
  `narrative_final_wish_completed`). Hard NPC-id bindings remain Plan 52
  scope (archetypes are the unit of assignment, not named individuals).
- **Expedition/travel:** the hunter's line walk, the hermit's open ground,
  the farmer's field rows, the neighbor's fence walk — shelter-adjacent and
  expedition-adjacent travel expressed narratively. Hard `loc_*` step fields
  are not part of the runtime objective grammar (steps are counted, not
  located) — documented rather than faked (§2.5 discipline).
- **Recipes/meals:** the cook's and chef's `required_items` reference real
  food-chain items (crops, water, salt) — the cooking authority owns any
  actual crafting.
- **Confession:** both confessions author `item_document_field_report`
  records — filed/factual, no lurid detail.
- **Mourning/ritual:** the monk's office and the undertaker's linen ground
  the dignity wishes in ritual objects; the mourning authority remains
  external.
- **Guilt/relic:** completion effects remain the runtime's +15 buff — per-wish
  guilt mechanics do not exist in the grammar and were not invented.

## Balance profile

- Consequences stay the runtime's fixed ±15/−10 — no reward inflation
  possible or authored.
- Required items are common-to-moderate (no rare medicine/fuel demands).
- Travel wishes are shelter-adjacent, not deep-expedition demands.
- One wish per archetype (runtime-keyed) — no farming loop is expressible.

## Verification

| Gate | Result |
|---|---|
| `--data-integrity-selftest` | **PASS** 0 findings / 208 catalogs (10,360 ids) |
| `dotnet test Ashfall.Core.Tests` | **PASS** 6,616/6,616 |
| `dotnet build Ashfall.csproj` | **PASS** 0 errors |
| `--content-utilization-selftest` | **PASS** |
| `--bridge-selftest` | **PASS** exit 0 |

## Deferred

1. Runtime-consumed per-wish `morale_bonus`/`buff_id` variation (hardcoded
   +15/−10 today).
2. Objective checking for `required_items` (host-gating hints today).
3. Per-archetype multiple wish templates with seeded selection (the
   archetype→wish map is 1:1 by design).
4. Plan 52 named-NPC bindings (archetype-keyed assignment model).
5. Hard `loc_*` expedition step objectives (needs objective-grammar
   extension first).
6. Plan 15 Chronicle/epilogue projection for high-value wish outcomes.
