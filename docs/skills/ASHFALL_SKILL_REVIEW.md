# ASHFALL Skill Review

**Date:** 2026-08-22
**Scope:** Ten skills proposed during the skill-gap brainstorm

## Decision

All ten capabilities were retained, but hardened into narrow project skills.
They are implemented under `.agents/skills/`. None adds production code,
changes JSON authority, or introduces a utility script with unverified
assumptions.

## Quality Decisions

| Skill | Decision | Hardening applied |
|---|---|---|
| `ashfall-save-roundtrip` | Implemented as a focused verifier | Does not duplicate repository-wide fuzzing or generate reflection-based tests |
| `ashfall-determinism-scan` | Implemented as static preflight | Does not claim static cleanliness proves runtime parity or edit code silently |
| `ashfall-dependency-map` | Implemented as evidence map | Distinguishes imports from runtime edges and confirmed orphans from candidates |
| `ashfall-expansion-phase` | Implemented as status audit | Requires current source evidence instead of trusting plans/comments |
| `ashfall-test-fixture` | Implemented as constrained fixture workflow | Uses real constructors and canonical IDs; refuses speculative mocks |
| `ashfall-catalog-audit` | Implemented as deep companion audit | Extends, rather than replaces, `CatalogIntegrityValidator` and the data self-test |
| `ashfall-decompose-godot` | Implemented as read-only decomposition plan | Removes the unsafe promise of an automatic file splitter |
| `ashfall-narrative-check` | Implemented as narrative acceptance check | Narrows overlap with `ashfall-narrative-continuity` to reachability and mechanics |
| `ashfall-equipment-balance` | Implemented as specialized balance workflow | Uses deterministic scenarios and proposes, but does not silently edit, tuning |
| `ashfall-headless-demo` | Implemented as coverage-aware scaffolder | Reuses existing verbs and requires approval before adding CLI surface |

## Existing Skill Relationships

- `ashfall-save-fuzz` remains the broad save-store/codec battery.
- `ashfall-determinism-guard` and `ashfall-seed-replay` remain runtime evidence lanes.
- `ashfall-test-gap` remains the coverage census and backlog owner.
- `ashfall-data-schema` and `CatalogIntegrityValidator` remain data-shape authority.
- `ashfall-narrative-continuity` remains the broad narrative graph auditor.
- `ashfall-balance-sim` remains the general coupled-system simulation lane.
- `ashfall-implement` owns approved production refactors and wiring changes.

## Verification

These are Markdown-only additions. Project build/test gates should still be run
by the invoking agent before claiming a task complete; this review itself does
not alter runtime behavior.
