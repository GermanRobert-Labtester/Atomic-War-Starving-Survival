# ASHFALL Save System Fuzz — Phase 2 (Round-Trip Battery)

**Skill:** ashfall-save-fuzz · **Mode:** round-trip battery
**Date:** 2026-08-22

---

## 1. ExpeditionSaveStore

| Test | Command | PASS/FAIL | Notes |
|---|---|---|---|
| Clean round-trip | dotnet test --filter "FullyQualifiedName~ExpeditionSaveStoreTests" | ✅ | 3/3 tests passed |
| Checksum mutation | dotnet test --filter "FullyQualifiedName~SaveStoreChecksumSweepTests" | ✅ | 12/12 tests passed (checksum-reject) |
| Null checksum on new-format envelope | grep "checksum field missing" | ✅ | Guard exists: "checksum field missing (corrupt save)" |
| Legacy fallback | grep "Legacy bare-state" | ✅ | Path exists: "Legacy bare-state saves (pre-checksum) still load" |
| Version migrations | grep "V1.*V2.*V3" | ❌ | No version migration logic found (checksum-only) |

---

## 2. Summary

ExpeditionSaveStore: 4/5 test types covered (clean, checksum-reject, null-checksum-reject, legacy-fallback). Version migration logic not found — may be checksum-only.

**Next:** Run the same battery for MedicalSaveStore, NarrativeSaveStore, WorldSaveStore, JournalSaveStore.
