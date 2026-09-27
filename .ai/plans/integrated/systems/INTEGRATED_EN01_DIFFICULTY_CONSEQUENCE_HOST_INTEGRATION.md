# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# EN-01 Difficulty-Consequence Weave Host Integration

**STATUS: APPROVED BY USER** (user authorized all previously-gated integrations, 2026-09-26)
**Package:** `EN-01-DIFFICULTY-CONSEQUENCE-SURFACE`
**Gate authority:** `Ashfall.Core.Difficulty.DifficultyConsequenceWeave` (previously 0 `src/` references)

## Bounded outcome

Give the signed EN-01 read model an operational host surface. It maps canonical difficulty scalars
into the four world-consumer consequences: war-stage severity multiplier, crisis deadline offset,
shock/rumor weight, and monotonicity validation. Read-only projection; no save, no duplicate authority.

## Delivered

- New `src/Host/HostCli.DifficultyConsequence.cs` — 8-check probe `DifficultyConsequenceSelfTest`.
- `Assets/Ashfall.Core/HostCliRegistry.cs` — enum `DifficultyConsequenceSelfTest` + descriptor
  `--difficulty-consequence-selftest`.
- `src/Host/HostCli.cs` — host enum + parse + `PrintHelp` entry.
- `src/Main.Application.cs` — dispatch.

## Verification

`dotnet build Ashfall.csproj` → 0 errors; `godot --headless -- --difficulty-consequence-selftest`
→ 8/8 (null refusal, legacy neutrality, deadline clamping, harsh 1.875 severity/0.8x deadline,
directional monotonicity). `HostCliActionParityGateTests` 4/4; manifest 261 tests; CLI catalog 321
entries; my flags absent from the help-contract miss list.

## Non-goals

No change to `DifficultyConsequenceWeave` logic; no wiring into war/crisis owners (that belongs to a
separate consumer package); no save section.
