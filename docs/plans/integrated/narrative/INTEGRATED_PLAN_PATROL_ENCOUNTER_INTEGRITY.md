# PLAN-PATROL-ENCOUNTER-INTEGRITY — Travel Encounter Data Integrity Host Integration
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **ARCHIVAL DIRECTIVE (MANDATORY): This plan is FULLY INTEGRATED. Move/keep this file in the integrated plans folder `docs/plans/integrated/<category>/`. It must never remain in `docs/plans/` as open work, and it must not be re-executed or reopened without a new foreman signature.**
> **INTEGRATION STATE: FULLY INTEGRATED — Core authority bound · host session bound · live route/CLI seam bound · focused tests green · no parallel authority created.**

## Integrated evidence

* `PatrolEncounterValidator` gained a real consumer: `ValidatePatrolEncounters()` runs the authored patrol rules over exactly the catalog the live travel owner resolves from (`ExpeditionHostSession.BoundTravelCatalog`), against the real faction and item sets, and reports honestly that 21 of 57 rows are patrol rows (the validator is patrol-specialised by design; the rest are out of scope, not silently passed). Read-only: no row is dropped, edited, or invented. Probe: `--patrol-encounter-integrity-selftest` 11/11. Test coverage stays with the pre-existing `PatrolEncounterValidationTests`; a duplicate suite was removed rather than added.

## Original plan body (preserved for the record)

> **Package:** `PATROL-ENCOUNTER-INTEGRITY`
> **Category:** narrative / data integrity
> **Plan type:** wire an unhosted validator into the existing integrity pipeline.
> **Date of evidence:** 2026-09-26, branch `integration/all-latest-2026-09-24`

---

## 1. Objective

Bind `Assets/Ashfall.Core/Narrative/PatrolEncounterValidator.cs` — a 336-line
authored rule set with **zero consumers** — to the live travel-encounter catalog,
so patrol encounter rows are checked for duplicate ids, dangling faction and item
references, malformed weights, and broken choice wiring at load.

**Bounded outcome:** one validation pass over the same catalog the live
`ExpeditionHostSession.TravelEngine` consumes, using the **real** faction and item
sets, reported through the host and gated by a probe.

**Non-goals:** no new encounter authority, no catalog edits, no invented
references, no repair-by-deletion.

## 2. Current Reality (re-verified 2026-09-26)

| Fact | Evidence |
| --- | --- |
| Orphan validator | `grep -rl PatrolEncounterValidator src/` → no matches; no Core runtime consumer |
| Validator contract | `Validate(IEnumerable<TravelEncounterDefinition>, ISet<string>? validFactionIds, ISet<string>? validItemIds)` → `List<string>`; plus `ValidateJson(...)` |
| Live catalog under test | `TravelEncounterCatalog.LoadFromDirectory(dataDir, fileIO)` → `Encounters` / `Count`, consumed by `ExpeditionHostSession.cs:527-529` |
| **Pre-flight: data is clean today** | measured run: 57 authored encounters → **0 errors** with no reference sets, so wiring the validator cannot turn a green gate red |
| Real reference sets available | factions from the live catalog, items from `_inventory.Catalog` (`ItemCatalog.Get`) |
| Integrity rule to honour | AGENTS.md: "Validate catalog IDs, references, ranges, and consumers through the current integrity pipeline" |

## 3. Files

### New — Host
- `src/Host/PatrolEncounterIntegrityHostSession.cs`
- `src/Host/HostCli.PatrolEncounterIntegrity.cs`

### Modified — Core
- `Assets/Ashfall.Core/HostCliRegistry.cs` — `--patrol-encounter-integrity-selftest`

### Modified — Host
- `src/Host/ExpeditionHostSession.cs` — validate the bound travel catalog
- `src/Host/HostCli.cs`, `src/Main.Application.cs`, `src/Main.Expeditions.cs`
- `scripts/ci/generate-architecture-map.py`

### Modified — Tests
- `Ashfall.Core.Tests/Narrative/PlanPatrolEncounterIntegrityTests.cs` (new)

## 4. Acceptance

1. `--patrol-encounter-integrity-selftest` ≥ 9/9: authored catalog reports **zero**
   integrity errors; each injected defect (duplicate id, unknown faction, unknown
   item, empty choices, out-of-range weight) produces its authored message; a
   clean row produces none; validation never mutates the catalog; repeat
   validation is deterministic.
2. Focused Core-contract suite green.
3. The validator is read-only — no row is dropped, edited, or invented.
4. Adjacent gates green (no new save section).

## 5. Deferred with named reasons

- No CI data-integrity gate registration: that generator table is integrator-owned.
- No auto-repair of bad rows: silent deletion would hide authoring mistakes.
