# Plan 28 — Wildlife Ecology: Observation, Blooms & Food-Web Consequences

## Goal

Make ecological change legible through animal signs and forecasts, localized infestations, and
consequences across food, hazards, and trade. This plan does not create wildlife-migration data.

## Scope boundary

- Plan 35 owns wildlife_migration.json and migration-route definitions.
- Plan 36 owns trapping catalog data.
- This plan owns observability and ecological consequences that consume migration/trapping facts.
  It must not add species routes, migration windows, or a second population simulation.

## Task 28A — Ecological forecast and observation

1. Convert Plan 35 migration state into field-guide signs, radio/forecast notices, and map-facing
   observations with known/uncertain confidence.
2. Let a visit confirm or correct observations without altering the migration source of truth.
3. Test season changes, discovery state, and stable presentation after save/load.

## Task 28B — Ecological hazard blooms and infestations

1. Author localized bloom/infestation crises through existing event, greenhouse, ventilation, and
   excavation authorities.
2. Provide clear, costly responses and the occasional ecological trade-off without adding a new
   infestation simulation.
3. Validate disease, item, and location references; test bloom, clearing, and recovery.

## Task 28C — Food-web and market consequences

1. Map a small set of legible chains from existing migration, trapping, crop, and market facts.
2. Trigger authored consequences such as scarcity, predator pressure, or a fish-run opportunity
   through owning systems rather than a parallel ecology ledger.
3. Test bounded cascading behavior and balance so no route becomes a guaranteed food exploit.

## Definition of Done

- Plan 35 remains the sole migration-catalog owner.
- Players can observe ecological change before exploiting or suffering it.
- Blooms and food-web effects are bounded, data-valid, and testable.
