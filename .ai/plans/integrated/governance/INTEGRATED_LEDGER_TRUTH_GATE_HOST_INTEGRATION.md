# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Ledger Truth Integrity Gate Host Integration

**STATUS: APPROVED BY USER** (user authorized EN-08, 2026-09-26)
**Package:** `EN-08-LEDGER-TRUTH`
**Gate authority:** `Ashfall.Core.Orchestration.LedgerTruthIntegrityGate` (previously 0 `src/` references)

## Bounded outcome

Expose the EN-08 ledger-truth gate through the host CLI: decision-register terminal/deferred
invariants plus the D21 zero-quarantine truth. Read-only; no save, no duplicate authority.

## Delivered

- New `src/Host/HostCli.LedgerTruth.cs` — 8-check probe `LedgerTruthGateSelfTest`.
- `Assets/Ashfall.Core/HostCliRegistry.cs` — enum `LedgerTruthGateSelfTest` + descriptor
  `--ledger-truth-selftest`.
- `src/Host/HostCli.cs` — host enum + parse.
- `src/Main.Application.cs` — dispatch.

## Verification

`godot --headless -- --ledger-truth-selftest` → 8/8 (terminal valid; named deferral valid;
unnamed deferral flagged; open verdict flagged; missing id flagged; quarantine flagged;
empty quarantine passes; mixed counts exact). Build 0 errors; parity gate 4/4.

## Non-goals

No change to the gate logic; no register parsing; no quarantine mutation (truth only).
