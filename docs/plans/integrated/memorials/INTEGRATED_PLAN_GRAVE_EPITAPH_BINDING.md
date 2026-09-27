# PLAN-GRAVE-EPITAPH-BINDING — Authored Grave Epitaphs → Memorial System Host Binding
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **ARCHIVAL DIRECTIVE (MANDATORY): This plan is FULLY INTEGRATED. Move/keep this file in the integrated plans folder `docs/plans/integrated/<category>/`. It must never remain in `docs/plans/` as open work, and it must not be re-executed or reopened without a new foreman signature.**
> **INTEGRATION STATE: FULLY INTEGRATED — Core authority bound · host session bound · live route/CLI seam bound · focused tests green · no parallel authority created.**

## Integrated evidence

* `SetupGraveEpitaphs()` (called from campaign setup) assigns the authored `wasteland_grave_epitaphs.json` table and a fork of the campaign-seeded RNG to `MemorialSystem.EpitaphCatalog`/`EpitaphRng` — two seams whose consumption rule was already written in `SelectEpitaph` but which nothing ever assigned, so graves were inscribed empty. Probe: `--grave-epitaphs-selftest` 9/9. Focused tests: `PlanGraveEpitaphBindingTests` 6/6 (catalog content remains covered by the pre-existing `WastelandGraveEpitaphsCatalogTests`). Zero new save sections: inscriptions persist inside the existing memorial section, and the allowlisted `MainTriadDriftGateTests` disposition records why no Save twin is required.

## Original plan body (preserved for the record)

> **Package:** `GRAVE-EPITAPH-BINDING`
> **Category:** memorial / narrative
> **Plan type:** assign a designed-but-unassigned Core seam and its campaign RNG.
> **Date of evidence:** 2026-09-26, branch `integration/all-latest-2026-09-24`

---

## 1. Objective

Load the authored `wasteland_grave_epitaphs.json` through the never-called
`GraveEpitaphCatalog.Load` and assign it to `MemorialSystem.EpitaphCatalog` plus
`MemorialSystem.EpitaphRng`, so the epitaph selection the Core **already
implements** becomes reachable and deterministic on the campaign stream.

**Bounded outcome:** a memorial whose death has no bespoke epitaph falls back to
an authored epitaph for its cause instead of an empty inscription.

**Non-goals:** no new memorial authority, no new epitaph text, no new save
section, no new RNG source.

## 2. Current Reality (re-verified 2026-09-26)

| Fact | Evidence |
| --- | --- |
| **Consumer code already exists** | `MemorialSystem.cs:295-299`: `if (string.IsNullOrEmpty(epitaph) && EpitaphCatalog != null) epitaph = EpitaphCatalog.SelectEpitaph(cause, EpitaphRng ?? new SeededRng(seed));` |
| **Seam never assigned** | `MemorialSystem.cs:178` `public GraveEpitaphCatalog? EpitaphCatalog { get; set; }` and `:217` `public ISeededRng? EpitaphRng { get; set; }` — `grep -rg EpitaphCatalog src/` finds only an unrelated constant |
| Orphan loader | `GraveEpitaphCatalog.Load(dataDir, fileIO, json)` + `DefaultFileName = "wasteland_grave_epitaphs.json"` — zero callers |
| Authored data | `Assets/StreamingAssets/Data/wasteland_grave_epitaphs.json` (`cause` + `epitaph` rows) |
| Declared consumer | `ContentUtilizationScanner.cs:500/829/1106` map `wasteland_grave_epitaphs.json → MemorialSystem` |
| Live owner | `MemorialSystem` is hosted: `src/Host/ShelterDecorHostSession.cs`, `HoldfastPresentationHostSession.cs`, `Plan49DepthPassHostSession.cs`, `HostCli.SliceScenario.cs` |
| **Determinism defect** | with `EpitaphRng` unset, line 299 falls back to `new SeededRng(seed)` derived from the memorial rather than the campaign stream |

## 3. Files

### New — Host
- `src/Host/GraveEpitaphHostSession.cs`
- `src/Host/HostCli.GraveEpitaphs.cs`

### Modified — Core
- `Assets/Ashfall.Core/HostCliRegistry.cs` — `--grave-epitaphs-selftest`

### Modified — Host
- `src/Main.Memorial.cs` (or the live memorial setup seam) — assign catalog + campaign-forked RNG
- `src/Host/HostCli.cs`, `src/Main.Application.cs`
- `scripts/ci/generate-architecture-map.py`

### Modified — Tests
- `Ashfall.Core.Tests/Memorial/PlanGraveEpitaphBindingTests.cs` (new)

## 4. Acceptance

1. `--grave-epitaphs-selftest` ≥ 9/9: authored catalog loads; every entry has a
   cause and non-empty epitaph; selection by cause returns an authored line;
   unknown cause still yields an authored fallback rather than empty; the
   memorial system is actually carrying the assigned catalog; the RNG comes from a
   named campaign fork; same seed ⇒ same epitaph; a bespoke epitaph still wins.
2. Focused Core-contract suite green.
3. `MemorialSystem` stays the sole memorial authority.
4. Adjacent gates green (no new save section).

## 5. Deferred with named reasons

- No epitaph authoring: content is the narrative lane's authority.
- No memorial-wall carving binding: `wall_carving_templates.json` has no designed
  slot on the live owner, so binding it would create a new surface rather than
  close a missing wire — recorded for the memorial owner to place.
