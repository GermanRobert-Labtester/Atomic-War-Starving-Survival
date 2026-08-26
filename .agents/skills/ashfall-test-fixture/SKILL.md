---
name: ashfall-test-fixture
description: Designs and, when requested, implements a minimal deterministic xUnit fixture for one ASHFALL Core system from its real constructor and data contracts. Use when a focused test needs reliable setup; complements ashfall-test-gap without generating speculative boilerplate.
---

# ASHFALL Deterministic Test Fixture

## Role

Create the smallest honest fixture for one named Core system. A fixture must
exercise real contracts and expose meaningful state; it must not hide missing
dependencies behind broad mocks or fake production behavior.

`ashfall-save-roundtrip` owns persistence assertions. This skill owns fixture
construction only and should be invoked by it when setup is the missing piece.

## Workflow

1. Inspect the target constructor, required ports, state DTO, catalog shape,
   existing test helpers, and nearby tests.
2. List dependencies as `required`, `optional`, or `not used`. Reuse existing
   helpers before introducing another factory.
3. Choose a fixed seed and construct the project’s seeded RNG. Use in-memory
   file/JSON/log/clock adapters already present in Core where appropriate.
4. Build one baseline fixture plus only the variants needed for boundaries:
   empty collections, threshold values, missing references, and save state.
5. By default, return the fixture design without editing. Add the fixture and
   focused tests only when the user explicitly authorizes test edits. Name any
   assumptions about data authority and ordering.
6. Run the focused tests, inspect the diff, then run the full Core test suite
   when edits were authorized.

## Rules

- Never use `System.Random`, wall-clock values, or random GUIDs.
- Do not fabricate catalog IDs; resolve them from authoritative JSON or use
  an existing canonical test ID.
- Do not add a fixture abstraction for one test unless it reduces duplication
  without obscuring setup.
- If construction is impossible because production wiring is incomplete,
  report that seam rather than bypassing it.

## Output

Return the fixture dependency map with constructor file/line evidence, seed,
tests added if authorized, assumptions, and exact verification results. Put
tests in `Ashfall.Core.Tests/`; do not add production fixtures to Core.

## Quality gate

- Fixture setup uses real signatures and compiles.
- Tests fail for the intended defect and pass after the correct repair.
- No new analyzer warnings or hidden mutable global state are introduced.
