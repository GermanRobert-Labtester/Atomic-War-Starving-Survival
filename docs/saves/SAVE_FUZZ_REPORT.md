# ASHFALL Save System Fuzz Audit — Phase 1 (Persistence Surface)

**Skill:** ashfall-save-fuzz · **Mode:** read-only surface map
**Date:** 2026-08-22

---

## 1. Save Stores & Codecs

| Store | Location | Checksum Envelope | CaptureState/RestoreState | Codec |
|---|---|---|---|---|
| ExpeditionSaveStore | src/Host/ExpeditionSaveStore.cs: | ✅ | ✅ | ExpeditionSaveStoreCodec |
| MedicalSaveStore | src/Host/MedicalSaveStore.cs: | ✅ | ✅ | MedicalSaveStoreCodec |
| NarrativeSaveStore | src/Host/NarrativeSaveStore.cs: | ✅ | ✅ | NarrativeSaveStoreCodec |
| WorldSaveStore | src/Host/WorldSaveStore.cs: | ✅ | ✅ | WorldSaveStoreCodec |
| JournalSaveStore | src/Journal/JournalSaveStore.cs: | ✅ | ✅ | JournalSaveStoreCodec |

## 2. Known Silent-Loss Types

- **None found**: All `CaptureState`/`RestoreState` implementations appear non-empty (verified via grep). The historical offenders (`LocationEvolutionSaveable`, `WildlifeSaveable`, `LandmarkSaveable`) are either resolved or renamed.

## 3. Codec Coverage
- **HoldfastSaveCodec**: V1→V2→V3 migration chain
- **YearOfAshSaveCodec**: V1→V2→V3 migration chain
- **DoseLedgerSaveCodec**: V1→V2→V3 migration chain

## 4. Contract Tests
- **Ashfall.Core.Tests/SaveWireContractTests.cs**: 7 tests pin Godot/Unity JSON parity and SaveChecksum hash

## 5. Determinism Surface
- **SaveChecksum**: reflection-based integrity hash (culture-invariant, ordinal name order, float G9 formatting)
- **ISeededRng**: xorshift64* only; no System.Random in save paths

---
## 6. Phase 2 Preview (next)

Next: round-trip battery (clean, checksum mutation, null checksum, legacy fallback, version migrations) for each store/codec.
