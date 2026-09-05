# Year of Ash Hardening Implementation Log

## Phase 1 — Deterministic timeline state

Status: PASS

Changed:

- Made the Year of Ash timeline monotonic and idempotent for repeated or
  out-of-order day inputs.
- Made restore derive phase and environmental parameters from the authoritative
  clamped day.
- Added replay and inconsistent-save regression coverage.
- Added `docs/world/YEAR_OF_ASH_SEASON_FLOW.md`.

Tests:

- `YearOfAshTests.Timeline_IgnoresRepeatedAndOutOfOrderDays`
- `YearOfAshTests.Timeline_RestoreDerivesPhaseFromAuthoritativeDay`

Result:

- The same day sequence and a save/reload sequence cannot regress the season
  phase or duplicate day-advance notifications.

Divergences:

- Storm-window catalog, Ice Road integration, and shared economy modifiers are
  not claimed complete. They remain a separate authored-data phase.
