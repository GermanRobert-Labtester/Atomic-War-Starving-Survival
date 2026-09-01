# Plan 28 — Completion Report (reconciled)

> **Superseded claim.** An earlier version of this report declared Plan 28 "100% complete"
> based on a parallel implementation (`EcologyCoordinator` + `wildlife_migration.json` +
> `ecological_infestations.json`) that was a runtime island: zero host consumers, a second
> migration authority beside `world_evolution_seeds.json`, and a loader bypassing the
> `IJsonSerializer` port convention. That layer was retired on 2026-09-01 and its authored
> content preserved in `RETIRED_ECOLOGY_ISLAND.md`.

## Reconciled state of Plan 28

**Live and authoritative (verified by build, 5,757-test suite, and selftests):**

- Migration/population authority: `WildlifeMigrationSystem` over `world_evolution_seeds.json`
  (13 packs, 11 canonical sectors, water-flagged river⇄estuary pair).
- Seasonal rhythm: `WildlifeSeasonalCalendar` — 7 archetypes × 6 Plan 19 season windows,
  pure functions, legacy-neutral when unbound, deterministic under per-day RNG forks.
- Consumers wired: trapping density (existing hook), expedition danger (existing), market
  scarcity deltas (existing), archetype-flavored radio intercepts (capped 3/day).
- Save/determinism: round-trip and same-seed trajectory tests green; old saves safe.

**Salvaged from the retired implementation (content lives in existing authorities):**

- 8 `event_eco_*` events in `events.json` (existing event-runtime schema).
- `loc_dead_zone` cautionary location (existing location authority; integrity-gated).
- 10 authored infestation definitions → preserved as the Phase 4 seed in
  `RETIRED_ECOLOGY_ISLAND.md` (targets/items/options inventoried; two target-id bugs noted).
- `CatalogIntegrityRules` prefix additions required by kept content.

**Retired:** `Assets/Ashfall.Core/Ecology/` (coordinator/models/loader),
`wildlife_migration.json`, `ecological_infestations.json`, and their 6 tests — the
duplicate-authority and runtime-island risks (Plan 28 §0.8, §15 DoD, Invariant 6) outweighed
keeping unwired code. Nothing authored was lost: see `RETIRED_ECOLOGY_ISLAND.md`.

**Plan 28 true status:** Phase 1–2 complete, Phase 3 partial; Phases 4–7 open. See
`PLAN28_BASELINE.md` §4–5 for the task ledger and SEASONAL_ABUNDANCE_CALENDAR.md /
MIGRATION_CORRIDOR_MATRIX.md / ECOLOGICAL_WEB.md for the living design.
