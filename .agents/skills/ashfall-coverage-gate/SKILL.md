---
name: ashfall-coverage-gate
description: Gates xUnit coverage via coverlet for Ashfall.Core, enforcing save round-trip and determinism coverage for H10/H11 gaps. Use when adding systems, before PR, or to close test-gap findings.
---

# ASHFALL Coverage Gate

## ROLE
`Ashfall.Core.Tests` must prove behavior, not just compilation. You enforce that new stateful systems ship `CaptureState/RestoreState` round-trip and determinism tests, and that Core stays above threshold.

Complements `ashfall-test-gap` (maps) and `csharp-testing` (generic xUnit) with an ASHFALL-specific gate.

## RULES
1. Coverage via `dotnet test /p:CollectCoverage=true` + `coverlet.collector` only — never Unity Test Framework.
2. Cover `Assets/Ashfall.Core/` only; `src/` (Godot host) is thin-node glue excluded from gate.
3. Every stateful system must have: behavior test, save round-trip test, determinism pin (if RNG-touched).

## WORKFLOW
### PHASE 1 — Baseline
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=lcov --collect:"XPlat Code Coverage"` → parse `coverage.info` / `lcov.info`.
- Record line/branch % overall and per-system (`NeedsSystem`, `RadiationSystem`, `JournalSystem`, etc.). Highlight H10 (58 behavior tests but 0 save round-trip) and H11 (JournalSystem untested).

### PHASE 2 — Gap Map
- Cross-ref `ashfall-test-gap` output: systems missing `SaveStoreChecksumSweepTests`-style round-trip (clean + mutated-hash + null-checksum-rejected).
- Flag DTOs not exercised (`LocationEvolutionSaveable`, `WildlifeSaveable` empty CaptureState).

### PHASE 3 — Gate
Default thresholds (tightenable per task):
- Overall Core line ≥ 65%, branch ≥ 55%
- Any new/changed stateful system: 100% `CaptureState/RestoreState` exercised, ≥1 determinism seed pin
- Fail PR if threshold missed or H10/H11 regressions.

### PHASE 4 — Verify
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` 0 errors, 3 allowed analyzer warns (`xUnit2013`).
- `dotnet test` all green, coverage artifacts emitted.

## OUTPUT
`docs/coverage/COVERAGE_GATE_REPORT.md` — overall %, per-system table, uncovered stateful systems, save-round-trip matrix, determinism pin list, pass/fail vs threshold.

## QUALITY GATE
- Coverage thresholds met, no uncovered `CaptureState/RestoreState`, H10/H11 tracked to closure or explicit deferral.
