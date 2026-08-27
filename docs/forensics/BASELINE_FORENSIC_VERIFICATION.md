# Baseline Forensic Verification

**Date:** 2026-08-26
**Purpose:** Re-establish current executable baseline after forensic reconciliation and expanded shelter save fixes
**Mode:** Exact command output, zero modification

---

## 1. Core Unit Tests

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

- **Result:** PASS
- **Tests:** 3,242 passed, 0 failed, 0 skipped
- **Duration:** ~5s
- **Framework:** .NET 9.0

---

## 2. Core Library Build

```bash
dotnet build Ashfall.Core/Ashfall.Core.csproj
```

- **Result:** PASS
- **Errors:** 0
- **Target Framework:** `net8.0` / `netstandard2.1` compatible
- **Invariant 1 Status:** 0 engine references (zero `UnityEngine`, `Godot`, `JsonUtility`)

---

## 3. Godot Host Application Build

```bash
dotnet build Ashfall.csproj
```

- **Result:** PASS
- **Errors:** 0
- **Target Framework:** `net8.0`
- **Engine Version:** Godot 4.7.1 (.NET)

---

## 4. Data Integrity Selftest

```bash
godot --headless --path . -- --data-integrity-selftest
```

- **Result:** PASS
- **Findings:** 0 errors, 0 warnings
- **Catalogs Validated:** 129 catalogs
- **IDs Authored:** 4,793 IDs
- **Reuses Reserved:** 976

---

## 5. Bridge Selftest

```bash
godot --headless --path . -- --bridge-selftest
```

- **Result:** PASS
- **Status:** Stable CI verb confirming `UnityEngine.*` compatibility shim is fully removed (`src/Bridge/` deleted) and host migration to Godot is complete.

---

## 6. Triad Drift Gate

```bash
./scripts/ci/triad-drift-gate.sh
```

- **Result:** GATE PASS
- **Setups:** 66
- **Saves:** 61
- **Sections:** 60 (`AllSaveSections` synchronized with Setup/Save pairs)

---

## 7. Orphan Candidate Reclassification (F0-2)

- **Total Candidates Evaluated:** 15
- **Directly Hosted:** 2 (`CaregivingSystem`, `PhantomMemoryEngine`)
- **Indirectly Hosted:** 1 (`MaritimeDiveSystem`)
- **Core-Internal Collaborators:** 12 (`BallisticsSystem`, `ExpeditionVehicleSystem`, `IdeologicalFrictionSystem`, `LeadershipSystem`, `RationConflictSystem`, `OrbitalHarrowTelemetrySystem`, `PharmaLabSystem`, `SkillAtrophySystem`, `TraumaBondSystem`, `WeaponConditionSystem`, `WeatherStationSystem`, `WorkshopReverseEngineeringSystem`)
- **True Unhosted Orphans:** 0

**Conclusion:** All 15 candidate systems are actively reachable and integrated. No new unneeded `HostSession` classes are required.

---

## 8. Canonical Registry

- **Generated Documents:**
  - `docs/forensics/CANONICAL_SUBSYSTEM_REGISTRY.md`
  - `docs/forensics/CANONICAL_SUBSYSTEM_REGISTRY.csv`
- **Total Unique Entries:** 578 components (gameplay systems, catalogs, host sessions, save stores, codecs, UI panels)

---

## 9. Exit Criteria Verification Status

- [x] One current, reproducible set of baseline results captured
- [x] Actual test count verified: **3,242 / 3,242 passed**
- [x] Data integrity: **0 findings across 129 catalogs**
- [x] Bridge selftest: **PASS**
- [x] Triad drift gate: **PASS**
- [x] Orphan candidate reachability reclassification complete (0 true orphans)
- [x] Canonical registry generated and deduplicated
