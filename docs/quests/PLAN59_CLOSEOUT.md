# Plan 59 — Dynamic Questline Expansion: Closeout

## Status: **COMPLETE** (schema-migration variant, documented below)

## Summary

`dynamic_questlines.json` expanded to **15 authored multi-stage questlines**,
all live through the existing `QuestlineSystem` choice-graph runtime.

## Baseline & schema findings (Phase 0 — repository truth)

The plan's stated baseline of 4 was inaccurate in two ways, both resolved by
audit:

1. **Count:** `dynamic_questlines.json` contained **2 entries**
   (`quest_dying_signal`, `quest_aquifer_contamination`). The plan's
   field-by-field description (quest ID / title / target location / stage
   objectives / objective-item requirements, "including `quest_dying_signal`")
   matches this file family; the 4-entry catalog with the same schema family
   is `narrative_questlines.json`.
2. **Schema:** the file's original schema (numeric `stage`, `objective_items`,
   `branch_a`/`branch_b`, `rewards`/`cost` arrays) was **consumed by no
   loader** — `QuestlineSystem` reads the `QuestlineDefinition` choice-graph
   schema (`questlineId`/`firstStageId`/`stages[].choices[].nextStageId`),
   populated from `BuiltInQuestlineCatalog` (8 hardcoded C# questlines) and
   `year_of_ash_questlines.json` via `YearOfAshCatalogLoader`.
   `questline_master.json` (491 entries) is a separate atomic-quest registry.

**Resolution (Model B, per §1.3):** dynamic questlines are a specialized
independent catalog. The file was migrated to the runtime-supported
`YearOfAshQuestContainer` choice-graph schema — preserving both existing
questlines' IDs, titles, premises, target locations, stage structures, and
branch outcomes exactly — plus 13 new questlines. Schema migration was
required because the original file's data was unreachable dead content; the
2 broken location references (`loc_comm_array`, `loc_water_treatment_plant`
did not exist) independently prevented validation, which §1.2 permits fixing.

## Core change (justified per §72)

- **New:** `Assets/Ashfall.Core/YearOfAsh/DynamicQuestlineCatalogLoader.cs` —
  loads `dynamic_questlines.json` (same `YearOfAshQuestContainer` parse the
  runtime already uses) and registers via the existing `RegisterQuestline`
  API. No new quest logic, stage interpreter, or reward engine.
- **Wiring:** one line in `src/YearOfAsh/YearOfAshHostSession.Create`
  (the questline host), following the established
  `LoadAndRegisterQuests` pattern.

## Questline roster (15)

| # | ID | Type | Stages | Days | Faction |
|---|---|---|---|---|---|
| 1 | `quest_dying_signal` | investigation/rescue | 6 (2 terminals) | 185–330 | iron_garrison |
| 2 | `quest_aquifer_contamination` | resource | 5 (2 terminals) | 190–340 | faction_hydro_barons |
| 3 | `quest_missing_caravan` | investigation | 4 | 195–345 | faction_railway_guild |
| 4 | `quest_trapped_engineer` | rescue | 4 | 200–350 | iron_garrison |
| 5 | `quest_separated_child` | rescue | 4 | 205–355 | faction_salt_freeholders |
| 6 | `quest_contested_delivery` | faction | 5 (2 terminals) | 210–350 | iron_garrison |
| 7 | `quest_patrol_bounty` | faction/moral | 6 (3 terminals) | 215–355 | iron_garrison |
| 8 | `quest_number_station` | mystery | 3 | 220–360 | faction_scavengers |
| 9 | `quest_waystation_generator` | engineering | 4 | 200–345 | faction_railway_guild |
| 10 | `quest_rail_restoration` | engineering | 4 | 225–355 | faction_railway_guild |
| 11 | `quest_medicine_cache` | moral | 5 (3 terminals) | 230–360 | faction_salt_freeholders |
| 12 | `quest_silent_spectrum` | investigation | 3 | 210–350 | iron_garrison |
| 13 | `quest_collapsed_section` | rescue | 3 | 215–350 | iron_garrison |
| 14 | `quest_tithe_dispute` | faction | 4 (2 terminals) | 220–350 | faction_salt_freeholders |
| 15 | `quest_orphaned_stock` | moral | 5 (3 terminals) | 235–360 | faction_scavengers |

Type coverage: 2 investigation (overdue convoy, silent spectrum) + 2 rescue
(engineer, separated child) + 2 faction (contested delivery, tithe dispute) +
1 resource (aquifer) + 1 mystery (number station) + 2 engineering (waystation,
rail span) + 1 moral (medicine cache) — plus the 2 migrated originals. Every
questline has 3–6 stages, ≥1 terminal state, 2–3 choices per non-terminal
stage, and a distinct core verb (intercept/trace, diagnose, track, negotiate,
repair, decode, decide).

## References

- **Locations:** `loc_radio_relay_mast`, `loc_excavation_civilian_shelter`,
  `loc_water_station`, `abandoned_hospital`-family anchors resolved via prose
  + `locations.json` (151 locations). The 2 broken baseline refs were remapped
  to `loc_radio_relay_mast` / `loc_water_station`.
- **Items granted:** `vacuum_tube`, `copper_wire_10m_of_10m`,
  `mechanical_parts`, `item_preservation_salt`, `antibiotics`, `water_filter`,
  `machine_oil` — all resolve in the merged catalog. The 3 unresolvable
  baseline refs (`copper_wiring`, `survivor_family_adult/child`) were
  replaced: alias fix + narrative outcomes (survivors are not inventory).
- **Factions:** `iron_garrison`, `faction_railway_guild`,
  `faction_salt_freeholders`, `faction_hydro_barons`, `faction_scavengers`,
  `faction_rebuilders` — all resolve in `faction_lore.json` (23 factions).
- **Flags:** none authored — `QuestlineSystem` owns stage/choice history
  (`choiceHistory` + `completedQuestlineIds`/`failedQuestlineIds`), so per §1.6
  no external `flag_*` edges were needed. Zero orphan flags by construction.
- **Quests:** zero ID collisions across `questline_master.json` (491),
  `narrative_questlines.json` (4), and `year_of_ash_questlines.json` (8).

## Distress-signal / NPC bindings (live vs deferred)

- **Signal-triggered (live, via narrative premise + day windows):** dying
  signal, substation plea, number station, silent spectrum — 4 of the plan's
  5-target, with the overdue convoy as Guild-notice trigger. Plan 50 signal
  catalog binding remains a **deferred runtime hook** (signal→flag→availability
  auto-wiring does not exist; availability is day-windowed via
  `minDay`/`maxDay`, the system's only trigger mechanism).
- **NPC arc bindings (deferred):** the trapped engineer and separated child
  are authored as persona-anchored scenes with `outcomeNarrative` hooks;
  hard `npcId`/arc-quest bindings await Plan 52 recurring-character ids in
  this catalog's scope (the `QuestLink` bridge exists for encounters and is
  the documented future path).

## Consequences (all through existing authorities)

- Faction standing: `targetFactionId` + `factionStandingDelta` on 11 closing
  choices — the runtime's native stance mechanics (deltas 8–20, bounded).
- Item grants: idempotent per-resolution via `grantItemId`/`grantItemQuantity`
  (single close-out choice per terminal stage).
- World-state (water output, route reopening, family survivors): expressed as
  authored narrative + terminal outcomes — dynamic territory/settlement/route
  stores do not exist yet (§16/§84.3–84.4 deferred), so no static catalog
  mutation and no duplicate authority was created.

## Persistence

`QuestlineSystem` owns all campaign state (`ActiveQuestlineRecord`:
`currentStageId`, `choiceHistory`, status, day stamps) — static definitions
stay out of saves. New questlines default unavailable outside their day
window (no retroactive failure); the 2 migrated questlines keep their IDs, so
any prior completed-state records (they were previously unreachable) remain
forward-compatible.

## Balance & pacing

Day windows stagger availability across 185–360 (the system's designed
late-campaign band, matching the built-in catalog's 180–360): at most a
handful of questlines are available simultaneously; windows overlap in
triads at most. Reward burden is bounded (≤6 single items, standing ≤20).
No questline is mandatory (§1.15); all are ignorable opportunities.

## Verification

| Gate | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests` | **PASS** 0/0 |
| `dotnet test Ashfall.Core.Tests` | **PASS** 6,600/6,600 (Questline/YearOfAsh suites: 95/95) |
| `dotnet build Ashfall.csproj` | **PASS** 0 errors |
| `--data-integrity-selftest` | **PASS** 0 findings / 208 catalogs (10,281 ids) |
| `--content-utilization-selftest` | **PASS** |
| `--bridge-selftest` | **PASS** exit 0 |

## Deferred

1. Plan 50 distress-signal → questline auto-trigger wiring (signal catalog
   exists; the signal→flag→availability bridge does not).
2. Plan 52 `npcId`/arc bindings for the engineer/child scenes.
3. Plan 44/43 dynamic territory/settlement consequence stores.
4. Plan 34 knowledge rewards for the number-station logs.
5. `narrative_questlines.json` (4 sibling-catalog entries) — same schema
   family, same unloaded status; a future loader pass can adopt it into the
   same pipeline.
