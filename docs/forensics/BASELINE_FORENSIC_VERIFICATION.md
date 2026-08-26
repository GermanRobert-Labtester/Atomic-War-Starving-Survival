# Baseline Forensic Verification

**Date:** 2026-08-23  
**Purpose:** Capture current executable baseline after P0-3 catalog tests  
**Mode:** Exact command output, no modification

---

## 1. Core Tests

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

**Result:** PASS  
**Tests:** 2554 passed, 0 failed, 0 skipped  
**Duration:** ~4s  

---

## 2. Core Build

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

**Result:** PASS  
**Errors:** 0  
**Warnings:** 67 pre-existing nullable warnings  

---

## 3. Godot Host Build

```bash
dotnet build Ashfall.csproj
```

**Result:** PASS  
**Errors:** 0  
**Warnings:** 88 pre-existing nullable warnings  

---

## 4. Data Integrity Selftest

```bash
godot --headless --path . -- --data-integrity-selftest
```

**Result:** PASS  
**Findings:** 0  
**Catalogs:** 102  
**IDs authored:** 3637  
**Reuses reserved:** 716  

---

## 5. Bridge Selftest

```bash
godot --headless --path . -- --bridge-selftest
```

**Result:** PASS  
**Note:** UnityEngine.* shim removed — src/Bridge/ is empty. Migration to Godot is complete.  

---

## 6. P0-3 Catalog Tests Verification

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~GhostTransmissionCatalogTests|FullyQualifiedName~OralLoreCatalogTests|FullyQualifiedName~RadioScriptbookCatalogTests"
```

**Result:** PASS  
**Tests:** 9 passed, 0 failed  
**Files:** 
- `GhostTransmissionCatalogTests.cs` (3 tests)
- `OralLoreCatalogTests.cs` (3 tests)
- `RadioScriptbookCatalogTests.cs` (3 tests)

---

## 7. Orphan Reclassification (F0-2)

**Candidates evaluated:** 15  
**CORE_INTERNAL:** 14  
**TRUE_ORPHAN:** 1 (`PhantomMemorySystem`)  

**Key finding:** Previous reports overstated orphan count. 14 of 15 "orphan" systems are Core-internal collaborators with test coverage. Only `PhantomMemorySystem` is a true orphan.

---

## 8. Canonical Registry

**Generated:** `docs/forensics/CANONICAL_SUBSYSTEM_REGISTRY.md`  
**Generated:** `docs/forensics/CANONICAL_SUBSYSTEM_REGISTRY.csv`  
**Total entries:** 391  
**Kinds:** gameplay system (112), other (101), catalog (92), host session (48), demo/selftest (26), save store (12)

---

## 9. Pre-existing Warnings (Not Blocking)

| Warning | Count | Status |
|---------|-------|--------|
| CS8600 Possible null reference | 15 | Pre-existing |
| CS8603 Possible null reference return | 10 | Pre-existing |
| CS8604 Possible null reference argument | 1 | Pre-existing |
| **Total** | **67** | **Non-blocking** |

---

## 10. Exit Criteria Status

- [x] One current, reproducible set of results captured
- [x] Actual test count verified: 2554/2554
- [x] Data integrity: 0 errors
- [x] Bridge selftest: pass
- [x] P0-3 catalog tests: 9/9 pass
- [x] Orphan reclassification complete
- [x] Canonical registry generated
