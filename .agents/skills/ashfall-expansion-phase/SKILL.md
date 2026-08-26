---
name: ashfall-expansion-phase
description: Audits the real implementation status of an ASHFALL expansion across system, data, IDs, Godot wiring, saves, and tests. Use when coordinating Holdfast, Duty Roster, Standing Record, Crossing, or standalone expansion work.
---

# ASHFALL Expansion Phase Coordinator

## Phase model

Use the project’s five phases as a checklist, not as proof of completion:

1. Core system and state contract
2. Authority data in `Assets/StreamingAssets/Data/`
3. Canonical IDs and catalog references
4. Godot construction, wiring, ticking, and save paths
5. Behavior, save, determinism, and integration tests

## Workflow

1. Name the expansion and read its current Core, data, `src/`, and test paths.
2. For each phase mark `COMPLETE`, `PARTIAL`, `BLOCKED`, or `NOT_STARTED`.
   `COMPLETE` requires source evidence; a plan, comment, constructor, or save
   registration alone is insufficient.
3. Trace each system through construction, initialization, tick/update, events,
   capture/restore, and any headless self-test. Record missing links.
4. Check canonical snake_case IDs and catalog integrity without inventing IDs.
5. Detect placeholder effects, unreachable callbacks, duplicate host logic,
   and tests that only prove construction rather than behavior.
6. Order the next actions by dependency: Core contract, data, IDs, wiring,
   then tests. Do not modify production code in this audit.

## Rules

- Current source outranks old expansion plans and reports.
- JSON is authoritative; do not create parallel engine data.
- A registered saveable with empty state behavior is not phase-complete.
- Read-only by default: do not edit Core, host code, JSON, scenes, or tests.
- Use `ashfall-implement` for approved changes and `ashfall-test-gap` for a
  repository-wide coverage inventory.

## Output

Return a phase matrix, evidence links, blockers, risk-ranked next actions, and
explicit unresolved assumptions. If a report is requested, use
`docs/expansions/PHASE_STATUS_<expansion>.md` and preserve existing reports.

## Quality gate

- Every phase status has current file/line evidence.
- No phase is marked complete based solely on documentation.
- The result identifies whether the gap is code, data, wiring, save, or test.
