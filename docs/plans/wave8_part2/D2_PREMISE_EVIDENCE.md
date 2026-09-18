# D2 — Execute Recorded Decisions — Premise Evidence

**Task:** Wave 8 Part 2, TASK D2 (`Seal-steps/847219_ASHFALL_WAVE8_IMPLEMENTATION_UNBLOCKER_PLAN_PART2.md`)
**Status:** EXECUTED
**Date:** 2026-09-17
**Base commit:** `033df2b7` + prior D1/C2 work
**Package:** `D2-RECORDED-DECISIONS`

## 1. SurvivorInspection — zero-consumer re-proof (Phase 0)

Whole-tree search (source, tests, docs, generators, JSON, reflection/registration
lists) for `SurvivorInspectionHostSession` / `SurvivorInspectionSnapshot`:

| Match | Classification |
|---|---|
| `Assets/Ashfall.Core/Survivors/SurvivorInspectionHostSession.cs` (2 types) | declaration |
| `Ashfall.Core.Tests/Survivors/SurvivorInspectionHostSessionTests.cs` | test-only fixture |
| `docs/gaps/PARTIAL_PLANS_VERIFIED_AUDIT.md` (P10, D2) | documentation |
| `docs/forensics/survivor_aggregate_FORENSIC_REPORT.md:433` | documentation |
| `KNOWN_DEBT.md` (`DEBT-SURVIVOR-INSPECTION-ORPHAN`, `DEBT-186`) | documentation |
| `Seal-steps/**`, `C-integration-plans/**` | historical plan text |
| `Ashfall.Core.Tests/BalanceFedLoopTelemetryTests.cs:127` | **comment only** (no type reference) |

**No live consumer, construction, registration, serialization identifier, DI
root, router/panel binding, or generator input.** No `.uid` sidecar. The
recorded retirement still applies ⇒ deletion authorized by D2's decision gate.

## 2. Dead-data rows (Phase 3)

The "five primary-wins overridden rows" are the distress-signal expansion rows
overridden by the primary catalog's last-load order:

`freq_distress_55_1`, `freq_distress_401_9`, `freq_distress_217_4`,
`freq_distress_148_2`, `freq_distress_392_7`.

Source: `docs/radio/DISTRESS_SIGNAL_PR2_HINT_TRANCHE.md`; contract: §5 of
`DISTRESS_SIGNAL_STAGE_CONTRACT.md`. The integrity gate emits exactly these five
warnings (verified below). They were previously documented only inside tranche/
contract prose, not as one authoritative register.

## 3. Authority boundaries

- SurvivorInspection: retirement execution only (zero-consumer proof above).
- Live survivor read surface (`SurvivorDetailPanel` → `SurvivorsHostSession`):
  untouched.
- Expansion data authority: the five rows are **not deleted** (no signature);
  they are registered as dead data.
- Debt/audit docs: record the execution to prevent rediscovery.
