# ECOLOGY_MAP_VISIBILITY.md — Plan 28 Task 28L

**Status: DEFERRED to the map UI pass (Plan 16 owns marker architecture). Design recorded
so the UI pass implements one contract, not three.**

## Contract

1. **Granularity:** region presence only — "wildlife reported in sector_4_hills", never
   exact pack coordinates or counts (the system exposes `GetSectorPackPopulation`, but the
   map shows a coarse band: absent / passing / holding).
2. **Cadence:** refreshed on the daily tick (the same day-owner cadence), never per frame.
3. **Discovery gate:** a sector shows migration presence only after the player has scouted
   it or heard a radio intercept naming it — no omniscient radar.
4. **Encoding:** icon + text label; color never the sole channel (Plan 14 accessibility).
   Status vocabulary: `migrating / abundant / scarce / tainted* / infested*`
   (*deferred couplings).
5. **Update cadence:** once per campaign day tick (not per frame) — reads the same
   sector-map diff the radio projection uses, so map and radio can never disagree.
6. Data source: `WildlifeMigrationSystem.State.packs` + `WildlifeSeasonalCalendar`
   abundance factors; no new state, no new poll loop.

## Acceptance (for the implementing pass)

- Snapshot diff clean; keyboard-navigable; screen-reader text present.
- Population counts never rendered unless a future system explicitly exposes them.
- Peak-day snapshot of a herd corridor + fish-run marker stored under snapshots/.
