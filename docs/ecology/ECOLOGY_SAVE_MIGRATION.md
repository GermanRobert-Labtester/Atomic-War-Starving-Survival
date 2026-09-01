# ECOLOGY_SAVE_MIGRATION.md — Plan 28 Task 28AY

**Design rule that made this cheap: the seasonal calendar stores nothing.**

`WildlifeSeasonalCalendar` is pure functions of (day, profile, pack state). Abundance,
hunger pacing, notices, and the water filter are computed at tick/refresh time from
existing persisted fields. Therefore:

- **Save shape unchanged:** `WildlifeSaveState` remains
  `{ schema_version, systemId, lastMigrationDay, packs[] }` (schema_version 1).
- **Old saves load unchanged:** a pre-Plan-28 save restores pack positions verbatim; the
  bound season profile then paces hunger from the current day forward.
- **Round-trip is exact:** `SaveRestore_WithBoundCalendar_RoundTripsExactly` pins
  serialize(deserialize(x)) == x for a 100-day-old state with the calendar bound.
- **No reload may restart migration:** positions live in `packs[].currentSectorId` and are
  restored wholesale by `RestoreState` (clone semantics, pinned since task 122).

## Persistent field inventory (all pre-existing)

| Field | Owner | Round-trip proof |
|---|---|---|
| pack position (`currentSectorId`) | `WildlifePackRecord` | world envelope tests + seasonal round-trip |
| population / seededPopulation | same | birth-ceiling + collapse tests |
| starvationLevel / aggressionScore / isRabid / lastThreatFiredDay | Live tick | `SaveWireContract` + capture/restore parity |
| season profile | **not persisted** — re-bound from `weather_seasons.json` at boot | `WeatherProfileLoader` (existing gate) |
| water flags | seed catalog, re-derived at seed | `SeededCatalog_KeepsWaterFlagsOnTheWaterwayPair` |

## Reload invariants (tested)

1. Active migration resumes at the saved sector — never re-seeded, never re-rolled.
2. Restore is byte-identical (JSON equality on capture/restore states).
3. Re-seeding after restore is a no-op (`Seeder_IsIdempotent_...`, selftest step 5).
4. UI/diagnostic reads (`GetSectorPackPopulation`, `SectorAbundanceFactor`) never mutate
   state (pinned: `SectorAbundance_..._ObservationNeverMutates`).

## Old-save matrix

| Save | Load behavior |
|---|---|
| pre-Plan-28 (11 packs) | loads; two new packs seed into empty ledger only on a *fresh* boot — a restored save keeps exactly its packs (idempotent seeder) |
| active migration mid-route | position + starvation resume; calendar re-paces from current day |
| rabid pack | `isRabid`/`lastThreatFiredDay` round-trip inside the envelope (existing) |
| 2× population peak | seededPopulation preserved; ceiling holds |
