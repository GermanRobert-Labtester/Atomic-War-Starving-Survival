# Duty Season Schema Contract

## Schema (repository-native — no added fields)

Wrapped catalog: `{"schema_version": 1, "items": [...]}`, loaded via `CatalogLocator.LoadWrappedList<DutyRosterSeasonEntry>`.

| Field | Type | Valid range | Semantics |
|---|---|---|---|
| `id` | string | `season_*` prefix, unique, snake_case | Stable identity |
| `windowMinDays` | int | ≥ 0 | First included day (inclusive) |
| `windowMaxDays` | int | ≥ `windowMinDays` | Final included day (inclusive) |
| `encounterWeight` | float | [0.5, 2.5] | Shelter-encounter pressure multiplier (replacement semantics via `ShelterEncounterSystem.SetSecondWinter`) |
| `steamTripChanceBoost` | float | [0.0, 0.15] | Authored modifier; range-validated; live consumer deferred |

## Selector contract (`DutyRosterCatalog.GetSeasonForDay`)

- Pure derivation from the authoritative campaign day — no state, no second clock.
- Matching rule (repository open-ended-window convention): **last season whose `windowMinDays <= day`**; ties keep the first-listed entry (deterministic regardless of file order).
- Inclusive bounds: day `d` matches season `s` iff `s.windowMinDays <= d <= s.windowMaxDays`.
- Day before the first window (incl. negative days) → `null`.
- Day after the final window → final season (carry-forward).

## Coverage contract (authored catalog)

- Chronological, **gap-free, overlap-free**: `next.windowMinDays == prev.windowMaxDays + 1`.
- First window starts at day 0; final window ends at day 365.
- Every day 0–365 matches **exactly one** season; days ≥ 366 match the final season; days < 0 match none.

## Non-fields (deliberately absent)

No display names, descriptions, weather IDs, chapter IDs, schedule IDs, flags, or tags — the DTO has no such fields and no consumers. Narrative phase prose lives in the coverage matrix docs.
