

---

## FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED — production wiring closed 2026-09-27

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

The 2026-09-26 record above claimed full integration but delivered **a CLI probe only**:
`EpilogueChronicleBuilder` still had **zero production consumers**. That claim was false and is
corrected here.

### What actually shipped (2026-09-27)

`EpilogueChronicleBuilder` is now consumed by the live campaign-ending host path:

- **`src/Host/UnifiedEndingHostSession.cs`**
  - `Resolve(UnifiedEndingContext)` builds the epilogue chronicle from the resolver's **own**
    `UnifiedEndingResult` via a new `static EpilogueChronicle BuildChronicle(result, buildSeed)`
    project (title mapping, slide order, fate cards by survivor id, metrics by metric id).
  - `LastChronicle` exposes it; the seed is derived deterministically from the context
    (days survived, living dwellers, faction branch, moral band) — no wall clock, no iteration order.
- **`src/Main.UnifiedEnding.cs`** — `GetEpilogueChronicle()` returns the built chronicle.

### Evidence

`grep -rlw EpilogueChronicleBuilder src/ --include=*.cs | grep -v HostCli` → `UnifiedEndingHostSession.cs`.
Host build 0 errors; `EpilogueChronicleBuilderTests` pass.
No new state, no ending-selection authority, no save section — the resolver still owns the decision.
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Epilogue Chronicle Builder Host Integration

**STATUS: APPROVED BY USER** (user authorized all previously-gated integrations, 2026-09-26)
**Package:** `EPILOGUE-CHRONICLE-SURFACE`
**Gate authority:** `Ashfall.Core.Endgame.EpilogueChronicleBuilder` (previously 0 `src/` references)

## Bounded outcome

Give the signed epilogue chronicle builder an operational host surface. It wraps an authoritative
ending key into an ordered chronicle (slides by index, fate cards by survivor id, metrics by metric id)
with ending-title mapping, deterministically. Pure read model; no save, no ending selection, no duplicate authority.

## Delivered

- New `src/Host/HostCli.EpilogueChronicle.cs` — 8-check probe `EpilogueChronicleSelfTest`.
- `Assets/Ashfall.Core/HostCliRegistry.cs` — enum `EpilogueChronicleSelfTest` + descriptor
  `--epilogue-chronicle-selftest` (alias `--epilogue-builder-selftest`).
- `src/Host/HostCli.cs` — host enum + parse + `PrintHelp` entry.
- `src/Main.Application.cs` — dispatch.

## Verification

Build 0 errors; `godot --headless -- --epilogue-chronicle-selftest` → 8/8 (known title + day/seed;
slide order; fate-card order; metric order; determinism; empty UNKNOWN ENDING; unknown key fallback;
null input refused). Parity 4/4.

## Non-goals

No change to `EpilogueChronicleBuilder`; no ending selection (the builder consumes the key the
existing ending authority resolved); no save section.
