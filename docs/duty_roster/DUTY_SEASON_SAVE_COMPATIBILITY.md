# Plan 77 — Save Compatibility

## Active season: derived, never persisted

`DutyRosterSaveCodec` persists sim day and system states (including `ShelterEncounterSystemState.encounterWeightMultiplier` / `secondWinterActiveSince`), **not** a season ID. On load, the active season re-resolves from the restored campaign day via `GetSeasonForDay` — stale cached season state cannot exist because no season identity is cached.

**Pinned by** `DutyRosterSeasonCatalogTests.SaveRoundTrip_RestoresSimDayAndReResolvesActiveSeasonCleanly`: save at day 150 → round-trip JSON → restored `simDay == 150` → re-resolved season is `season_first_siege` with the pinned modifier values (1.75 / 0.03).

## Catalog expansion requires no migration

- Old saves (single-season catalog era) load against the new catalog: their restored day simply resolves to the containing new season. Days that previously had no season now resolve — the intended content expansion.
- `season_second_winter` identity and values are unchanged, so any reference (including `DutyRosterIds` constants and the host's Second Winter activation path) resolves identically.
- No retroactive transition simulation occurs on load: selection is a pure function of the restored day; nothing replays days 0–N.

## Multi-day jumps and rollback

- Large jumps: selection derives from the current day; no per-transition event must fire (verified by `Selection_LargeDayJumpsResolveCorrectly`).
- Rollback/restore: because the season is derived, restoring an older save automatically restores the era-appropriate season — no cached siege state can survive a restore to day 61.

## Post-final-window saves

Days beyond 365 (debug or long campaigns) resolve to `season_long_winter` via the repository's carry-forward convention (pinned by `Selection_Post365OverflowFallback` at 366/400/500). Negative days resolve to null (pinned by `Selection_NegativeDayReturnsNull`).
