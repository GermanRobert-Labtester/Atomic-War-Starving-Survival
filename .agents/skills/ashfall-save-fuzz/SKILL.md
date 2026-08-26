---
name: ashfall-save-fuzz
description: Stress-tests every ASHFALL save store and codec with round-trips, checksum mutation, legacy-envelope fallbacks, and cross-codec migrations using dotnet tests and godot --headless only. Finds save corruption before players do.
---

# ASHFALL Save System Fuzzer

## ROLE

Cross-host save compatibility is a live battleground in ASHFALL: 5 Godot stores recently gained checksum envelopes (Expedition, Medical, Narrative, World, Journal), codecs migrate V1→V2→V3, the `SaveWireContract` pins Godot/Unity JSON parity, and the legacy `JsonUtility` path must stay loadable until migrated. You attack all of it.

## TARGETS
- Stores: `ExpeditionSaveStore`, `MedicalSaveStore`, `NarrativeSaveStore`, `WorldSaveStore`, `JournalSaveStore`.
- Codecs: `HoldfastSaveCodec`, `YearOfAshSaveCodec`, `DoseLedgerSaveCodec`.
- Contracts: `Assets/Ashfall.Core/SaveWireContract.cs`, `SaveChecksum.cs`.
- Existing contract tests: `Ashfall.Core.Tests/SaveStoreChecksumSweepTests.cs`, `SaveWireContractTests.cs`.

## WORKFLOW

### PHASE 1 — Map Persistence Surface
- Enumerate every `CaptureState/RestoreState` implementer and every save store. Note which have checksum envelopes and which are still bare-state.
- Flag known silent-loss types: `LocationEvolutionSaveable`, `WildlifeSaveable`, `LandmarkSaveable` (empty capture/restore).

### PHASE 2 — Round-Trip Battery
For each store/codec write or extend xUnit tests (never Unity):
1. Clean round-trip: capture → serialize → deserialize → restore → compare state.
2. Checksum mutation: flip one field in serialized state; load must reject with the exact documented error.
3. Null/empty checksum on new-format envelope: must reject (`checksum field missing (corrupt save)`), not fall back silently.
4. Bare-state legacy payload: must still load via the legacy fallback path.
5. Version migrations: V1→V2→V3 chains, plus throw-on-future behavior.

### PHASE 3 — Determinism & Wire Parity
- Same state serialized twice must byte-match (ordering/culture stability).
- Re-run the `SaveWireContract` assertions: Godot-shape and Unity-shape serializers produce identical JSON trees and identical `SaveChecksum` hashes.

### PHASE 4 — Host-Level Smoke
- `godot --headless --path . -- --data-integrity-selftest` and any save-related `--*selftest` verb; confirm 0 errors.

## RULES
- `dotnet` + `godot --headless` only. No Unity.
- New tests live in `Ashfall.Core.Tests/`, flat namespace, xUnit.
- Never weaken a guard to make a test pass; fix the store or report the defect.

## OUTPUT
`docs/saves/SAVE_FUZZ_REPORT.md` — store/codec matrix (round-trip, checksum-reject, legacy-fallback, migration), failures with repro tests, determinism findings.

## QUALITY GATE
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` all green.
- Every store in scope has at least the five battery cases.
