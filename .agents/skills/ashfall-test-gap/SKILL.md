---
name: ashfall-test-gap
description: Maps ASHFALL systems to their test coverage, finds missing save/round-trip/determinism tests (e.g. H10 NeedsSystem round-trips, H11 JournalSystem), and scaffolds skeletons in Ashfall.Core.Tests. Coverage cartography before coverage writing.
---

# ASHFALL Test Coverage Cartographer

## ROLE

`Ashfall.Core.Tests/` holds 226 test files, but coverage is uneven — documented holes include Needs/Radiation save round-trips (H10) and untested `JournalSystem` behavior (H11). You map what's tested versus what exists, rank the gaps by risk, and scaffold the missing tests.

## WORKFLOW

### PHASE 1 — System Census
- Enumerate stateful Core systems (every `CaptureState/RestoreState` implementer), pure-logic systems, and host sessions in `src/`.
- Enumerate test files; extract which types each exercises (grep class references, constructor calls).

### PHASE 2 — Coverage Matrix
- Per system: `UNIT` (behavior tested), `SAVE_RT` (save round-trip), `DETERMINISM` (seeded parity), `INTEGRATION` (selftest/host), `NONE`.
- Cross-check against AGENTS.md known issues (H10, H11, empty CaptureState offenders: `LocationEvolutionSaveable`, `WildlifeSaveable`, `LandmarkSaveable`) for current status.

### PHASE 3 — Risk Ranking
- Rank `NONE`/partial cells by blast radius: save-corrupting > determinism-breaking > balance-affecting > cosmetic.
- Systems with live player impact and zero round-trip tests are top priority.

### PHASE 4 — Scaffold
- For the top-ranked gaps write test skeletons in `Ashfall.Core.Tests` (flat namespace, xUnit): arrange from realistic state, act via capture/serialize/restore, assert field parity.
- Failing-by-design skeletons (Assert.Fail with TODO) are acceptable only when flagged; prefer working minimal round-trips.

### PHASE 5 — Verify
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` clean; `dotnet test` green.
- Zero new analyzer warnings (H9 standard: 0 errors, at most the known 3 xUnit warnings — ideally none added).

## RULES
- xUnit + dotnet only. No Unity Test Framework, ever.
- Tests must be deterministic: fixed seeds, invariant culture, no wall-clock.
- Never modify production code to satisfy tests; gaps in production are findings for repair skills.

## OUTPUT
`docs/tests/COVERAGE_MATRIX.md` — system×test-type matrix, risk ranking, scaffolds added, remaining backlog.

## QUALITY GATE
- Matrix complete for all stateful systems.
- All scaffolds compile and run; suite green.
