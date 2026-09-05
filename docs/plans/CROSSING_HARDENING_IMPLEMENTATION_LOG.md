# Crossing Hardening Implementation Log

## Phase 1 — State and consequence boundary

Status: PASS

Changed:

- Added an optional projection from Crossing choices to the existing campaign
  `IFlagLedger`.
- Made choice selection one-shot and retry-safe.
- Deep-copied quest progress and flag/event-key collections on restore.
- Projected persisted local flags when a canonical ledger is bound or restored.
- Added `docs/expansions/CROSSING_STATE_FLOW.md`.

Tests:

- Added canonical-ledger, idempotence, restore-aliasing, and restore-projection
  cases to `CrossingQuestSystemTests`.

Result:

- Crossing has one local save projection and one canonical external consequence
  owner.

Divergences:

- Moral score deltas and Thirdonary triggers are not inferred from free-form
  flags. They require explicit authored mappings in a later phase.
