---
name: ashfall-determinism-scan
description: Performs a read-only static determinism preflight over ASHFALL Core or changed C# files, identifying unstable randomness, time, identity, hashing, culture, and collection-order patterns. Use before seeded replay or when reviewing deterministic gameplay changes.
---

# ASHFALL Determinism Static Scan

## Role

Find deterministic-risk patterns before runtime replay. This is a static
companion to `ashfall-determinism-guard` and `ashfall-seed-replay`; it does not
claim that a clean text scan proves behavioral determinism.

## Scan set

Start with changed files, then expand to `Assets/Ashfall.Core/` when the user
asks for a full audit. Inspect context for:

- `System.Random`, `new Random`, `Guid.NewGuid`, and wall-clock/environment
  values such as `DateTime.Now`, `DateTime.UtcNow`, and `Environment.TickCount`.
- Unstable `GetHashCode()` use, unordered enumeration used for simulation or
  serialized output, and culture-sensitive formatting/parsing.
- Seed derivation, RNG ownership, event ordering, and any host-specific branch
  that can alter simulation state.

## Workflow

1. Capture repository status and define the exact scan boundary.
2. Search source and inspect each match in context; distinguish comments,
   tests, UI-only code, and simulation-affecting code.
3. Classify each result as `CONFIRMED`, `CONTEXT_REQUIRED`, or `CLEAR`. Include
   file, line, call path, and why it can affect reproducibility.
4. For every confirmed issue, name the existing project replacement, normally
   `ISeededRng`, stable ordinal ordering, or invariant culture. Do not invent a
   new RNG or silently rewrite code.
5. Recommend the smallest paired replay or regression test. Hand implementation
   to `ashfall-repair` or `ashfall-implement` after approval.

## Rules

- Read-only by default. Do not edit Core, tests, or data during the scan.
- `System.Random` in a test fixture is not automatically a production defect;
  explain whether it can influence an assertion or persisted state.
- A static scan cannot establish cross-host parity. Require runtime evidence
  from `ashfall-determinism-guard` or `ashfall-seed-replay` for that claim.
- Use only `dotnet` and `godot --headless` for verification.

## Output

Return a compact findings table, scan boundary, false-positive decisions,
replacement recommendations, and the next test needed. If a report is
requested, use `docs/determinism/STATIC_SCAN_<scope>.md` without overwriting
another report.

## Quality gate

- Every match has context and a severity.
- No “clean” conclusion is made without stating the scanned boundary.
- Existing determinism skills are linked as follow-up evidence, not duplicated.
