# Plan 74 — Narrative Progression Chapters Closeout

## Completion mode

**COMPLETE — narrative spine only.**

> Fifteen-chapter narrative spine authored and validated.
> Cross-system transition effects remain owned by existing/future incident, territory,
> season, economy, guilt, warlord, rebuilding, and epilogue integrations.

## Runtime trigger model

Model E — hardcoded progression / display-only metadata (verified end-to-end, see
[NARRATIVE_PROGRESSION_RUNTIME_CONTRACT.md](NARRATIVE_PROGRESSION_RUNTIME_CONTRACT.md)):

- Sole consumer: `EventsHostSession` (loads catalog) → `EventsLogPanel` / `EventDetailPanel` (display all entries sorted by `Order`).
- Schema supports exactly two fields per entry: `description` (string) + `order` (int).
- No trigger evaluator, no current-chapter state, no transition events, no save state, no localization keys.
- No `trigger_day`, `phase`, or `world_state_changes` fields were added — they would be silently ignored dead data.

## Original five chapters (preserved unchanged)

| Order | Title | Status prefix |
|---:|---|---|
| 1 | The Exchange | Complete |
| 2 | Ashfall | Complete |
| 3 | The Bunker | Active |
| 4 | First Contact | Pending |
| 5 | The Long Winter | Pending |

All five are byte-identical to the pre-Plan-74 catalog.

## Final fifteen

| Order | Title | New? | Campaign role |
|---:|---|---|---|
| 1 | The Exchange | existing | immediate catastrophe |
| 2 | Ashfall | existing | fallout and disorientation |
| 3 | The Bunker | existing | sheltering and first routines |
| 4 | First Contact | existing | encountering other survivors |
| 5 | The Long Winter | existing | winter onset (Deep Freeze, day 60) |
| 6 | The Consolidation | **new** | factions become durable institutions |
| 7 | The Long Dark | **new** | deep-winter attrition (morale, sleep, people) |
| 8 | The Thaw | **new** | mobility and contact return (day 120) |
| 9 | The Schism | **new** | alliances and internal politics fracture |
| 10 | The Black Market | **new** | unofficial economy matures |
| 11 | The Reckoning | **new** | debts and history return |
| 12 | The Rebuilding | **new** | durable infrastructure projects |
| 13 | The Second Winter | **new** | renewed deep cold against a harder world (High Cold, day 240) |
| 14 | The Muster | **new** | the surviving gather to decide who speaks for them |
| 15 | The Inheritance | **new** | legacy and endgame handoff (The Turning, day 300+) |

New chapters follow the existing `Chapter N Pending: Title — text` description convention (statuses are static authored text, matching chapters 4–5).

## Changes to existing chapters

**None.** No existing entry was renamed, renumbered, reworded, or re-triggered.

## Duplicate-resolution decisions

- Draft "First Winter" was **dropped** — existing Chapter 5 "The Long Winter" already covers winter onset.
- Draft "Long Dark" was **retained**, repositioned as deep-winter *interior* attrition (distinct function from onset).
- Slot 14 uses **The Muster** (replacement pool) instead of a second pressure chapter — the late arc needed a political/opportunity beat between The Second Winter and The Inheritance.

## Season mapping (authority: weather_seasons.json)

| Chapter | Season anchor |
|---|---|
| 5 The Long Winter | `window_deep_freeze` (startDay 60) |
| 7 The Long Dark | interior of Deep Freeze (60–120) |
| 8 The Thaw | `window_thaw` (startDay 120) |
| 11 The Reckoning | `window_black_bloom` (startDay 180) |
| 13 The Second Winter | `window_high_cold` (startDay 240) |
| 15 The Inheritance | `window_the_turning` (startDay 300) |

Season system (`WeatherSystem.GetSeasonForDay`) is untouched and remains the sole weather authority.

## Incident links (Plan 57)

**0 authored.** No transition event exists to attach incidents to and no `incident_id` field exists. Five target chapters are recorded as deferred integration points in the integration matrix. No future/hypothetical IDs entered production JSON.

## Territory links (Plan 44)

**0 authored.** No territory hook or field exists. Consolidation / Schism / Reckoning are narrative framing only.

## Season integrations (19C)

**3 of 3 aligned thematically** (see table above). Trigger-side binding is not possible in the current schema and was not forced.

## Cross-plan refs

All referenced plans (57, 44, 19C, 48, 63, 66, 61, 71, 15A, 30C) appear only in documentation matrices as deferred integration expectations. **Zero unmerged plan IDs in production JSON.**

## Campaign-duration findings

`CampaignEpilogueEngine.FinalDay` is taken from the live runtime day (`Main.GameFlow.cs`); no fixed campaign-length cap exists. The season calendar defines windows through day 300+, so The Second Winter (240) and The Inheritance (300+) are structurally reachable in a surviving campaign. No unreachable late chapter was authored.

## Save behavior

Progression has **no save state** (by design, per `EventsHostSession`'s own contract). Save/load cannot replay, lose, or migrate chapter state. Old-save compatibility is trivially preserved: the loader re-reads the catalog fresh; entries are pure display rows; orders 1–5 unchanged. No save-schema change, no migration.

## Transition idempotence / determinism

Not applicable at the progression layer (no transitions exist). Display order is deterministic (`OrderBy(n => n.Order)` in `EventsLogPanel`); orders are unique and contiguous 1–15.

## Verification results

| Gate | Result |
|---|---|
| JSON validation (parse, fields, schema_version) | PASS — 15 entries, `order`+`description` only |
| Count | 15/15 |
| Orders unique + contiguous 1..15 | PASS |
| `--data-integrity-selftest` | PASS — 0 errors, 0 warnings, 298 catalogs |
| `--content-utilization-selftest` | PASS — CI gate PASS (0 orphaned; pre-existing warnings in unrelated catalogs) |
| Narrative/Progression/Season/Weather tests | PASS — 609/609 |
| Full `Ashfall.Core.Tests` | PASS — 9461/9461 |
| `dotnet build Ashfall.csproj` | PASS — 0 errors, 0 warnings |
| Manual chapter journey | N/A — chapters are static display rows; no runtime journey exists to exercise (Model E) |

## Deferred integrations

| Integration | Owner plan | Status |
|---|---|---|
| 5 chapter-transition incidents | 57 | deferred — requires progression transition events |
| 3 territory-linked chapter hooks | 44 | deferred — requires territory hook |
| Season-triggered chapters | 19C | deferred — thematic alignment only today |
| Phase metadata (early/mid/late) | — | deferred — no schema field |
| Runtime current-chapter state + UI banner | — | deferred — would require Core/host changes |

## Artifacts

- `Assets/StreamingAssets/Data/narrative_progression.json` — 15 chapters
- `docs/narrative/NARRATIVE_PROGRESSION_RUNTIME_CONTRACT.md`
- `docs/narrative/PLAN_74_CHAPTER_COVERAGE_MATRIX.md`
- `docs/narrative/PLAN_74_CHAPTER_PACING_MATRIX.md`
- `docs/narrative/PLAN_74_CHAPTER_INTEGRATION_MATRIX.md`
- `docs/narrative/PLAN_74_NARRATIVE_PROGRESSION_CHAPTERS_CLOSEOUT.md` (this file)
