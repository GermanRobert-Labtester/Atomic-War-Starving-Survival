# RETIRED — Ecology Runtime Island (Plan 28 reconciliation record)

> **Status: RETIRED 2026-09-01.** This file preserves the authored content of the
> duplicate ecology implementation (`Assets/Ashfall.Core/Ecology/` +
> `wildlife_migration.json` + `ecological_infestations.json` + their tests) so the
> Phase 4 infestation task can rebuild it **through the owning systems** without
> re-inventing the content.
>
> Why retired: the coordinator had zero host consumers (runtime island),
> `wildlife_migration.json` duplicated the `world_evolution_seeds.json` +
> `WildlifeSeasonalCalendar` authority (Plan 28 §1.1/§15, Invariant 6), and
> `EcologyCatalogLoader` bypassed the `IJsonSerializer`/`CatalogDiagnostics` loader
> convention (H4 pattern). Reconciliation decision: authoritative-runtime path wins;
> see `PLAN28_SESSION_REPORT_LIVE_RUNTIME.md` §6.

## What was retired and why

| Piece | Verdict | Reason |
|---|---|---|
| `wildlife_migration.json` (12 patterns) | **duplicate authority** | species/corridors live in `world_evolution_seeds.json` (13 packs, 11 sectors); seasonal windows/abundance are computed by `WildlifeSeasonalCalendar` from Plan 19 `weather_seasons.json` — a stored pattern layer forked both |
| `EcologyCoordinator.cs` | runtime island | zero consumers in `src/`; consequences (market multipliers, predator pressure) computed but never applied; effects must route through `MarketSystem`/`VentilationSystem`/food authority when wired (§1.9) |
| `EcologyModels.cs` / `EcologyCatalogLoader.cs` | with coordinator | loader bypassed `IJsonSerializer` port |
| `Ashfall.Core.Tests/Ecology/Plan28LivingEcologyTests.cs` (6) | with runtime | tested the island; retired with it |

## Preserved design — 10 authored infestations (Phase 4 seed)

Content contract per SHELTER_INFESTATION_CONTRACT.md applies (owner-system effects only).

### Location infestations (28R — 6)

| id | name | target | clear options | leave/harvest |
|---|---|---|---|---|
| infestation_subway_molerat_nest | Subway Ballast Molerat Burrow Complex | loc_flooded_subway_depot | smoke out; trap line | canned_food ×2 |
| infestation_quarry_hornet_hive | Limestone Quarry Slag Hornet Swarm | location_quarry_overlook | chemical spray; controlled burn | medkit ×1 |
| infestation_cellar_mold_bloom | Presshouse Cellar Black Rot Bloom | loc_cider_press | fungicide scrub | item_mycelium_bricks ×1 |
| infestation_bunker_roach_colony | Ordnance Depot Armored Roach Cluster | loc_ordnance_shoulder | mesh isolation | canned_food ×1 |
| infestation_canal_fungal_carpet | Seven Span Bridge Slime Rot | loc_bridge_seven | lime wash | item_bio_plastic ×1 |
| infestation_mill_rat_king | Printworks Paper-Nest Rat King | loc_printworks | poison sweep | fuel_canister ×1 |

### Shelter infestations (28S — must be re-targeted through owning systems)

| id | target | clear | notes |
|---|---|---|---|
| infestation_shelter_vent_mold | room_filtration | HEPA filter replace; antiseptic steam wash | vent mold → VentilationSystem (filter cost already exists) |
| infestation_shelter_pantry_weevils | room_bunks ⚠️ | tin repack | target should be the commissary/pantry room, not bunks; effect = bounded food loss via spoilage authority |
| infestation_shelter_wall_nest | room_bunks | smoke pellet | wall/maintenance authority |
| infestation_shelter_vermin_incursion | room_filtration ⚠️ | manual pick | cutworm outbreak targets hydroponic trays — mis-filed under room_filtration |

⚠️ = authoring fixes needed when this content is wired (target ids, per-infestation
trigger/hazard summaries were empty in the retired catalog).

### Coordinator behaviors worth rebuilding in Phase 4 (evidence-backed patterns)

- 5-day recurrence cooldown between infestation occurrences (bounded, tested).
- Clear = item cost (consumed via caller-owned inventory callback — no hidden writes) +
  seeded success roll (not guaranteed; failure costs the materials).
- `TolerateAndHarvest`: one-time state transition to ToleratedHarvesting; grants bounded
  resource; daily hazard roll while tolerated (`LeaveDailyHazardRisk` → disease-risk callback).
- Bounded consequence clamps: predator pressure [1.0, 1.6], meat [1.0, 1.5], fish run ×0.75,
  swarm grain ×1.35 — all reset to neutral when the driving condition clears (cascade budget
  compliant).
- Food-loss and disease-risk effects surfaced as **callbacks** — the Phase 4 wiring must
  route them through the food-spoilage authority and the Plan 09 `IDiseaseOutbreakSource`
  port, never direct inventory/health writes.

## Salvaged & kept (already live through existing authorities)

- 8 `event_eco_*` events in `events.json` (existing event runtime schema: weight/minDay/maxDay/choices).
  Note: state-gating against live migration state is a follow-up (they currently fire on
  day-window + weight, not ecology state — Task 28J's "reflect live state" refinement).
- `loc_dead_zone` in `locations.json` (Task 28AL cautionary location).
- `CatalogIntegrityRules` prefix additions needed by kept content (`field_fauna_`, `creature_`,
  `infestation_`, `species_`, `migration_`, `eco_chain_` reference keys).

## Migration patterns (12) — superseded, do not re-author as a catalog

Their corridors/windows/abundance are authoritative in: `world_evolution_seeds.json`
(13 packs, water-flagged river⇄estuary pair) + `WildlifeSeasonalCalendar` (7 archetypes ×
6 Plan 19 windows). The 12 retired patterns (`migration_steppe_herds_winter_rut`,
`migration_thaw_grazer_dispersal`, `migration_coastal_carp_spawning`,
`migration_gray_heron_coastal_prowl`, `migration_ash_boar_mast_run`,
`migration_iron_crow_autumn_flyway`, …) used the same 12 seeded species and the same
sector graph — their *ideas* live on as the archetype × window matrix in
SEASONAL_ABUNDANCE_CALENDAR.md. `rad_taint_risk` / `predator_follow` hooks are preserved
as designs in RAD_TAINT_FOOD_SAFETY_MATRIX.md and PREDATOR_PREY_CONSEQUENCE_MATRIX.md.
