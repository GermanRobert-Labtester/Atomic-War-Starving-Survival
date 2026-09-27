# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# EN-05 Rescued Survivor Arc Projection Host Integration

**STATUS: APPROVED BY USER** (user authorized all previously-gated integrations, 2026-09-26)
**Package:** `EN-05-RESCUED-ARC-SURFACE`
**Gate authority:** `Ashfall.Core.Radio.RescuedArcProjection` (previously 0 `src/` references)

## Bounded outcome

Give the signed EN-05 read model an operational host surface. It projects distress-rescue mission
state into a truthful UI-ready arc: None / EnRoute / Hospitalized / Integrated / Perished / Ambushed,
with recovery countdown and exactly-once journal key. Read-only; no save, no duplicate survivor record.

## Delivered

- New `src/Host/HostCli.RescuedArc.cs` — 8-check probe `RescuedArcSelfTest`.
- `Assets/Ashfall.Core/HostCliRegistry.cs` — enum `RescuedArcSelfTest` + descriptor
  `--rescued-arc-selftest` (alias `--distress-rescue-arc-selftest`).
- `src/Host/HostCli.cs` — host enum + parse + `PrintHelp` entry.
- `src/Main.Application.cs` — dispatch.

## Verification

Build 0 errors; `godot --headless -- --rescued-arc-selftest` → 8/8 (null None; Dispatched EnRoute;
recent rescue Hospitalized 4 days; past-window Integrated; Ambush; Failed Perished; dead-sender
Perished; constructor clamping). Parity 4/4; manifest 265 tests; CLI catalog 325 entries.

## Non-goals

No change to `RescuedArcProjection`; no distress-mission mutation; no journal write (the projection
produces the key; the existing journal owner consumes it).
