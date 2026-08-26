---
name: ashfall-save-migration
description: Pins the V1→V2→V3 codec migration matrix, cross-codec save wire compatibility, checksummed envelope vs bare-state legacy, and SaveChecksum stability. Use when changing DTOs, bumping schema_version, or auditing save stores.
---

# ASHFALL Save Migration

## ROLE
A save written by one build must load in the next — or fail loudly with `future version` rather than corrupt. You own the migration contract that `ashfall-save-fuzz` exercises but doesn't pin.

Authority: `AGENTS.md:SAVE/LOAD` (codecs `HoldfastSaveCodec`, `YearOfAshSaveCodec`, `DoseLedgerSaveCodec`), `SaveChecksum.cs`, `SaveWireContract` (7), `SaveStoreChecksumSweepTests` (12 per 4 stores).

## RULES
1. Every stateful system has `CaptureState/RestoreState` + versioned codec; DTOs are `[Serializable]` plain C# via `IJsonSerializer` (core `SystemTextJsonSerializer` in `HostDefaults.cs`), never `JsonUtility`.
2. Checksummed envelope (`Checksum` non-empty) is new format; bare-state is legacy fallback only. A null/empty `Checksum` in new-format envelope is corrupt, not legacy (`NarrativeSaveStore.TryLoad` guard).
3. Normalization: null/empty equiv, float G9, culture-invariant, ordinal name order — `SaveChecksum` must not drift.

## WORKFLOW
### PHASE 1 — Matrix Inventory
- Enumerate all save stores/codecs: `ExpeditionSaveStore`, `MedicalSaveStore`, `NarrativeSaveStore`, `WorldSaveStore`, `JournalSaveStore` + all `*SaveCodec.cs`.
- Record `schema_version` per store, history V1→V2→V3 and current wire shape.

### PHASE 2 — Migration Pins
- For each codec, exercise: load V1 → migrate → V2 shape, V2 → V3, reject future V(n+1) with throw, accept past with migrate.
- Envelope checks: clean round-trip preserves hash, mutated state changes hash, null/empty checksum rejected.

### PHASE 3 — Cross-Codec & Legacy
- Bare-state pre-checksum save still loads via legacy path; new envelope with missing checksum does NOT.
- Cross-host fixture (Godot-only since `_Game` deleted) loads golden JSON from `Ashfall.Core.Tests` fixtures — no engine-specific serialization.

### PHASE 4 — Verify
- `dotnet test --filter SaveWireContract` + `SaveStoreChecksumSweepTests` + `*SaveCodec*` green.
- `godot --headless --path . -- --data-integrity-selftest` green if data-touched.

## OUTPUT
`docs/saves/SAVE_MIGRATION_MATRIX.md` — per-store V1/V2/V3 matrix, pass/fail, checksum pins, breaking-change log, golden save list.

## QUALITY GATE
- All V1→V2→V3 migrations green, future version throws, legacy bare-state still loads, 0 checksum drift (G9/ordinal pinned).
