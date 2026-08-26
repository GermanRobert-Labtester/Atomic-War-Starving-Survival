---
name: ashfall-determinism-guard
description: Runs paired seeded replays for new systems, flags System.Random/Guid.NewGuid usage, and pins SaveChecksum culture-invariant formatting. For when the AI already knows the determinism rules.
---

# ASHFALL Determinism Guard

## ROLE

You eliminate the repetitive determinism verification overhead. The AI already knows the rules (`ISeededRng`, no `System.Random`, culture-invariant formatting) — you just enforce them.

## SCOPE

- **Input**: System name (e.g., `MedicalSystem`), seed value
- **Output**: Paired replay report, determinism findings, pinned tests
- **Constraints**: `dotnet` + `godot --headless` only; never Unity

## WORKFLOW

### PHASE 1 — Offender Sweep
- Grep the system for `System.Random`, `Guid.NewGuid()`, culture-sensitive formatting
- Cross-check against `SaveChecksum` formatting rules

### PHASE 2 — Paired Replay
- Run the same seeded simulation twice (fresh start vs save→restore→continue)
- Capture terminal state via `CaptureState()`; hash with `SaveChecksum`
- Identical seeds must produce identical hashes

### PHASE 3 — Test Anchoring
- Convert any divergence into a failing xUnit test pinned to the seed
- Never fix Core logic — only anchor the test

### PHASE 4 — Verify
- `dotnet test Ashfall.Core.Tests` (all green)
- `godot --headless --path . -- --data-integrity-selftest` (0 errors)

## CONSTRAINTS
- Never use wall-clock-derived seeds
- Always use invariant culture and ordinal comparison
- Never modify Core logic — only report findings

## OUTPUT
`docs/determinism/DETERMINISM_REPORT_<system>.md` — offender sweep, replay pairs with hashes, divergence traces, pinned tests

## QUALITY GATE
- Every replay pair shows matching hashes or an opened failing test
- No `System.Random`/`Guid.NewGuid()` in new systems
- `SaveChecksum` formatting culture-invariant
