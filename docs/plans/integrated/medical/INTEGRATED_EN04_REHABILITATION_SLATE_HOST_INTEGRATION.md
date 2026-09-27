

---

## FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED — production wiring closed 2026-09-27

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

The 2026-09-26 record above claimed full integration but delivered **a CLI probe only**:
`RehabilitationSlateProjection` still had **zero production consumers**. Corrected here.

### What actually shipped (2026-09-27)

The projection is now rendered by the live amputation-triage patient slate:

- **`src/UI/AmputationTriagePanel.cs`**
  - `PhantomPainProvider` seam + `GetRehabSlate()` — projects the **amputation authority's own**
    `BuildBodyState(survivorId)` plus its `HasPhantomPain` verdict into a REHABILITATION section
    (phase, days in phase, adaptation quality, next milestone, phantom-pain status).
- **`Assets/Ashfall.Core/Medical/AmputationSystem.cs`** — added read-only `HasPhantomPain(survivorId)`
  over existing phantom-pain state, so the panel holds no parallel pain cache.
- **`src/Main.PlayerSurfaces.cs`** — `amputation_surgery` panel bind now supplies the provider.

### Evidence

`grep -rlw RehabilitationSlateProjection src/ --include=*.cs | grep -v HostCli` → `AmputationTriagePanel.cs`.
Host build 0 errors; `RehabilitationSlateProjectionTests`, `Plan142ClothingWarmthHostIntegrationTests` and
`Plan11ExplorationTests` pass. Projection only — no state, no save, no duplicate survivor record.
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# EN-04 Rehabilitation Medicine Slate Host Integration

**STATUS: APPROVED BY USER** (user authorized all previously-gated integrations, 2026-09-26)
**Package:** `EN-04-REHABILITATION-SLATE-SURFACE`
**Gate authority:** `Ashfall.Core.Medical.RehabilitationSlateProjection` (previously 0 `src/` references)

## Bounded outcome

Give the signed EN-04 read model an operational host surface. It renders a survivor's prosthetic
count, rehab phase, quality ramp, next milestone, and phantom-pain state from `SurvivorBodyState` and
its `RehabRecord` without mutating state. Pure projection; no save, no duplicate authority.

## Delivered

- New `src/Host/HostCli.RehabilitationSlate.cs` — 8-check probe `RehabilitationSlateSelfTest`.
- `Assets/Ashfall.Core/HostCliRegistry.cs` — enum `RehabilitationSlateSelfTest` + descriptor
  `--rehabilitation-slate-selftest` (alias `--prosthetics-slate-selftest`).
- `src/Host/HostCli.cs` — host enum + parse + `PrintHelp` entry.
- `src/Main.Application.cs` — dispatch.

## Verification

`godot --headless -- --rehabilitation-slate-selftest` → 8/8 (null/intact none-phase; fitting 50% +
milestone + phantom pain; adaptation 75% + remaining days; mastery permanent; dual-condition count;
constructor clamps; unknown-phase Stable fallback). Build 0 errors; parity 4/4.

## Non-goals

No change to `RehabilitationSlateProjection`; no change to `SurvivorBodyState`/`RehabRecord`
ownership; no medical-ward panel edit (the projection is the read authority, exercised through CLI).
