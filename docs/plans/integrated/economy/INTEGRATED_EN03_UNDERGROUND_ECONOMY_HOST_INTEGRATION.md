# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# EN-03 Underground Economy Pressure Host Integration

**STATUS: APPROVED BY USER** (user authorized all previously-gated integrations, 2026-09-26)
**Package:** `EN-03-UNDERGROUND-ECONOMY-SURFACE`
**Gate authority:** `Ashfall.Core.Economy.UndergroundEconomyPressure` (previously 0 `src/` references)

## Bounded outcome

Give the signed EN-03 read model an operational host surface. It maps black-market heat, trust, and
relocation state to the Calm/Raised/Hot/Relocated temperature band, price pressure, and attention
risk for callers and panels. Pure view; no duplicate ledger or mutable store.

## Delivered

- New `src/Host/HostCli.UndergroundEconomy.cs` — 8-check probe `UndergroundEconomyPressureSelfTest`.
- `Assets/Ashfall.Core/HostCliRegistry.cs` — enum `UndergroundEconomyPressureSelfTest` + descriptor
  `--underground-economy-selftest` (alias `--market-temperature-selftest`).
- `src/Host/HostCli.cs` — host enum + parse + `PrintHelp` entry.
- `src/Main.Application.cs` — dispatch.

## Verification

`godot --headless -- --underground-economy-selftest` → 8/8 (Calm/Raised/Hot/Relocated thresholds and
multipliers, relocation override, heat/threshold clamping, explicit-constructor normalization, band
ordering, default threshold/trust). Build 0 errors; parity 4/4; manifest + CLI catalog regenerated.

## Non-goals

No change to `UndergroundEconomyPressure` logic; no new heat/trust authority; no save section.
