# ASHFALL — Expansion Wave 3 Index (Expansions 22–26)

**Status:** Design plans (pre-integration). Not claims, not authorizations.
**Date:** 2026-09-20
**Purpose:** Index and evidence summary for the five Wave 3 expansion bibles.

These documents investigate the live JSON data authority and Core systems, find
domains where content is thin relative to a working system, and propose expansions
that attach to existing owners. They are design bibles: they do not claim paths,
change code, or authorize implementation. Any implementation must later pass through
`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

---

## Why these five

Wave 1 (12–16) covered generational survival, belief, aviation/orbital,
agriculture/soil, and bionics/robotics. Wave 2 (17–21) covered leisure/culture,
subterranean geology, chemical/biological hazards, espionage/intelligence, and power
grids. Wave 3 covers five further domains with live systems and thin authored content,
chosen to avoid overlap with Waves 1–2, expansions 01–11, and the XP-04/EN-03 economy
legs.

| Expansion | System (live) | Current content | Gap |
|---|---|---|---|
| 22 · The Clean Flow | `WaterTreatmentSystem`, `BrineWaterSystem`, `SanitationSystem`, `WaterborneExposureRules`, `SumpFloodingSystem`, `ChlorAlkaliSynthesisEngine` | 1–4 KB catalogs | No source world, quality profiles, hygiene practices, waste routing, drainage network, public health, water commons |
| 23 · The Alarm | `ShelterFireHazardSystem`, `CascadeCoordinator`, `CascadeRuleCatalog`, `DiseaseTriage`, rescue dispatch | 1.7–5.8 KB catalogs | No evacuation, rescue, command, drills, suppression logistics, refuges, mutual aid |
| 24 · The Long Goodbye | `FinalWishSystem`, `CaregivingSystem`, `PsychologicalSanatoriumSystem`, `MentalHealthCrisisSystem`, `PsychologicalArcSystem`, `MemorialSystem` | 4 mental arcs, thin therapy | No aging, palliative care, therapy modalities, crisis protocols, grief stage, legacy tokens |
| 25 · The Iron Road | `RailwaySystem`, `RailwayInterlockEngine`, `RailLogisticsCatalog`, `RailGrindingEngine`, draisine/flail engines | 2.6–4.2 KB catalogs | No bridges, locomotives, track maintenance, schedules, rail towns, interdiction, commons |
| 26 · The Common Table | `KitchenNutritionSystem`, `NutritionDiversitySystem`, `FoodPreservationSystem`, `ResourceRationingSystem`, `RationConflictSystem`, `DesperationSystem` | 3.2–5.2 KB catalogs | No menus, deficiencies, preservation methods, ration policies, hunger phases, food culture |

Each plan opens with a source-verified authority split. The strongest examples: the
water chain enforces mass balance and never creates water; the cascade coordinator
owns none of the facts it reads and every rule has an off-ramp; the sanatorium only
writes treatment progress and never condition state; `RailwaySystem` and the interlock
are the only owners that may move a train; and nutrition consequences route only
through `NeedsSystem.Modify`.

---

## The five plans

1. `expansion_22_the_clean_flow_plan.md` — 70,078 chars.
   Water sources, quality testing, hygiene practices, waste routing, drainage,
   public health, water commons. Extends `WaterTreatmentSystem`,
   `SanitationSystem`, `SumpFloodingSystem`.
2. `expansion_23_the_alarm_plan.md` — 70,261 chars.
   Readiness, evacuation, incident command, rescue, suppression, drills, mutual aid,
   and a full cascade rule web. Extends `ShelterFireHazardSystem` and
   `CascadeCoordinator`.
3. `expansion_24_the_long_goodbye_plan.md` — 72,008 chars.
   Aging, frailty, palliative care, therapy modalities, crisis ward ethics, grief
   stages, and legacy tokens. Extends the live care, therapy, crisis, and memorial
   systems.
4. `expansion_25_the_iron_road_plan.md` — 70,139 chars.
   Track maintenance, bridges, locomotives, schedules, rail towns, interdiction, and
   a regional rail commons. Extends `RailwaySystem` and the interlock.
5. `expansion_26_the_common_table_plan.md` — 71,067 chars.
   Menus, cook skills, deficiencies, preservation methods, ration policies, hunger
   phases, and food culture. Extends the live kitchen, nutrition, preservation, and
   ration systems.

---

## Shared design constraints (all five)

- **Godot authoritative; Core engine-free.** No Godot or Unity reference in Core logic.
- **JSON data authoritative.** New content lives in snake_case catalogs with integer
  `schema_version`, validated by `CatalogIntegrityValidator` and registered with
  `ContentUtilizationScanner`.
- **One authority per concern.** Every plan contains a non-duplication statement and
  an integration-seam table. No second water, fire, cascade, therapy, rail, kitchen,
  nutrition, or save model is introduced.
- **Deterministic.** All rolls use the host-forked `ISeededRng` where the live system
  already does; the water exposure path stays RNG-free; paired-replay hashes must
  match across continuous and interrupted runs.
- **Persistence.** State is captured in existing save envelopes
  (`water_treatment`, `sanitation`, `sump`, `shelter_fire`, `caregiving`,
  `mental_health`, `sanatorium`, `railway`, `kitchen_nutrition`, `food_preservation`,
  `rationing`, etc.) as additive sub-objects. Legacy saves load neutral; the Triad
  drift gate must pass.
- **Tone.** Restrained, human, fictional. No exploitation of illness, aging, dying,
  hunger, or disaster.
- **Verification.** Focused tests per `TEST_POLICY.md`, plus
  `--data-integrity-selftest` and `--content-utilization-selftest`. A compile-green
  result is not acceptance.

---

## Ethical and content contracts specific to Wave 3

| Plan | Hard contract |
|---|---|
| 22 Clean Flow | No water from nothing; exposure only via `DiseaseSystem.TryExpose`; hygiene derived, never stored; dignified sanitation prose |
| 23 Alarm | Every cascade has a warning and an off-ramp; no scripted death without a route; rescue has real risk and pull-back |
| 24 Long Goodbye | No death rewards; no cure for permanent conditions; aging changes roles not worth; restraint logged and costly |
| 25 Iron Road | One topology and one interlock authority; bridges warn before failure; interdiction prefers negotiation; towns are people |
| 26 Common Table | No hidden nutrition bar; all consequences via `NeedsSystem.Modify`; preservation has real cost; deprivation depicted with dignity |

---

## Cross-wave hooks (summary)

- **Wave 1 × Wave 3:** clean water for fields and bodies; alarm drills for foundry
  and shelter; palliative care for the old and the irradiated; rail for seeds and
  ore; rations for children, the faithful, pilots, and the rebuilt.
- **Wave 2 × Wave 3:** water for decon and quarantine; alarm for grid and chemical
  incidents; therapy for moles and interrogators; rail for microgrid and vault cargo;
  the table for the evening and the quiet hand.
- **Wave 3 × Wave 3:** clean water and the alarm share hygiene and firefighting water;
  the alarm and the long goodbye share triage and crisis wards; the iron road and the
  common table share grain freight and market schedules; the long goodbye and the
  common table share last meals and comfort food.

Each plan is self-contained; none requires another to ship.

---

## How to promote a Wave 3 plan

1. Pick exactly one plan and one phase. Do not start two.
2. Re-audit the premise against current source and data; a plan is not proof an API
   or catalog still exists (`AGENTS.md` Rule 7).
3. Claim exact paths in `WORKTREE_OWNERSHIP.md`; confirm no overlap.
4. Add the package row to `INTEGRATION_PLANS.md` with owner, acceptance, and focused
   verification.
5. Implement data first, then pure Core, then persistence, then host/UI, then content.
6. Verify with focused tests and the data/content selftests; record limitations.
7. Update the live ledger only if you are the foreman or named integrator.

Until a foreman signature exists, these plans remain proposals. The safe pre-signature
work is Phase 1 (schemas and validators), which is additive and reversible.

---

## Open decisions common to the wave

- Content volume budgets (each plan lists an authoring estimate; total Wave 3 is
  roughly 320,000–380,000 words of new prose if all five are fully authored).
- Save-section placement (additive sub-objects vs. sibling sections) for each domain.
- Whether any Wave 3 expansion introduces a new headless selftest verb or extends an
  existing one. Each plan recommends extending existing verbs.
- Priority order. Recommended: 22 (clean flow) and 26 (common table) first, because
  water and food are the tightest survival loops; 23 (alarm) second, because it
  hardens everything; 24 (long goodbye) third, because it is the largest emotional
  build; 25 (iron road) last, because it is the largest content build and depends on
  a stable economy.

---

## Evidence anchors (file references used across the plans)

- `Assets/StreamingAssets/Data/` — 622 JSON files, canonical content.
- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` — data integrity gate.
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` — dead-data gate.
- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` — save section registry.
- `docs/ASHFALL_EXPANSION_CONTEXT_ATLAS.md` — low-connectivity island analysis.
- `docs/expansions/EXPANSIONS_MASTER_CATALOG.md` — expansions 01–11 catalog.
- `docs/expansions/wave1/WAVE1_INDEX.md` — Wave 1 index.
- `docs/expansions/wave2/WAVE2_INDEX.md` — Wave 2 index.
- `docs/CURRENT_AUTHORITY.md` — authority map.
- `AGENTS.md`, `TEST_POLICY.md`, `WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md`.