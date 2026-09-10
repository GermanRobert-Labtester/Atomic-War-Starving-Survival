# Plan 146 Completion Report — Bunker Court Records Runtime Activation & Institutional Consequence Projection

**Document ID:** ARCH-PLAN146-COMPLETION-REPORT
**Status:** Complete & Verified
**Project:** ASHFALL (Godot 4.7+ .NET 8 Host / C# Core)
**Date:** 2026-09-09

---

## 1. Executive Summary

Plan 146 activated the 24-record bunker tribunal corpus (`Assets/StreamingAssets/Data/narrative/bunker_court_verdicts_codex.json`) as discoverable, queryable institutional history within ASHFALL. The system provides complete, progression-gated access to a decade of communal justice, emergency decrees, minor infractions, and constitutional development spanning Day 84 to Day 3650.

All implementation goals were achieved with zero runtime side effects:
- **Zero Gameplay Side-Effects:** Viewing historical court dockets never mutates live inventory, morale, survivor health, detention states, or faction standings.
- **Engine-Agnostic Core:** Hardened `BunkerCourtCatalog` with full idempotency, duplicate protection, deterministic sort ordering, directory loading, and rich query helpers.
- **Full Discovery Integration:** All 24 cases registered in `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` under `channel: library_terminal` and `producer_id: government_bunker` with calibrated `min_day` pacing.
- **Host Wiring:** Exposed `GetBunkerCourtCatalog()` in `src/Main.ShelterInfrastructure.cs` following the established lazy-singleton pattern.
- **Content Utilization Scanner:** Fully wired in `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` across loader, registry, consumer, and UI stages.

---

## 2. Forensic Changes Summary

### 2.1 Core Loader Hardening (`Assets/Ashfall.Core/Narrative/BunkerCourtCatalog.cs`)
- Added `GetDocketDay()` on `BunkerCourtCaseEntry` to parse historical trial day directly from docket format (`TRIB-{day}-{KEYWORD}`).
- Made `BunkerCourtCatalog.Load()` strictly idempotent: guarded by `_byId.ContainsKey(c.case_id)` to eliminate enumeration duplication on repeat loading.
- Implemented `Clear()` for clean state resets.
- Added deterministic sorting: ordered by historical day ascending, then `case_id` ordinal.
- Added `LoadFromDirectory(string dataDir, IFileIO fileIo, IJsonSerializer serializer)` with `CatalogDiagnostics.Warn` exception logging.
- Added query methods: `GetById`, `GetByDocket`, `GetByDefendant`, `GetByMagistrate`, `GetByVerdict`, `GetByTag`, `GetUnlockedByDay`, and `GetBySearch`.

### 2.2 Discovery Manifest (`Assets/StreamingAssets/Data/narrative_discovery_manifest.json`)
- Added discovery entries for cases 5 through 24 (`disc_court_ladle_favoritism` through `disc_court_century_constitution_ratification`), completing registration of all 24 cases.
- Configured channel `library_terminal`, producer `government_bunker`, and graded `min_day` values (Day 3 to Day 60).

### 2.3 Content Utilization Scanner (`Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`)
- Added `IsPlan146CourtFile` matching `narrative/bunker_court_verdicts_codex.json`.
- Registered `BunkerCourtCatalog` as loader and registry node.
- Registered consumers: `NarrativeDiscoveryCatalog`, `JournalPanel`.
- Registered UI surface: `JournalPanel`.

### 2.4 Host Accessor (`src/Main.ShelterInfrastructure.cs`)
- Added `GetBunkerCourtCatalog()` lazy singleton accessor on `Main`.

### 2.5 Test Suites
- Expanded `Ashfall.Core.Tests/BunkerCourtCatalogTests.cs` to 11 comprehensive regression tests covering idempotency, state resets, sorting, docket parsing, query helpers, malformed input rejection, directory loading, manifest integration, and zero-gameplay-mutation guarantees.
- Updated `Ashfall.Core.Tests/NarrativeDiscoverySystemTests.cs` to validate 80 total records across 18 catalogs in the manifest (including 30 government bunker library terminal entries).

---

## 3. Mandatory CI Verification Results (Rule 5)

| Gate Command | Result | Details |
|---|---|---|
| `dotnet test Ashfall.Core.Tests` | **PASS (0 errors)** | **10,273 passed, 0 failed, 0 skipped** (44s duration) |
| `godot --headless --path . -- --content-utilization-selftest` | **PASS** | Discovered 582 catalogs, **0 orphaned**, CI gate PASS |
| `godot --headless --path . -- --data-integrity-selftest` | **PASS** | **0 errors, 0 warnings across 299 catalogs** (12,528 ids authored) |
| `godot --headless --path . -- --scene-binding-selftest` | **PASS** | **25 passed, 0 failed (of 25)** |
| `python3 scripts/ci/scene-lint.py` | **PASS** | 30 production scenes checked; **0 errors; 0 warnings** |

---

## 4. Documentation Deliverables

1. `docs/architecture/PLAN146_BASELINE.md`
2. `docs/architecture/BUNKER_COURT_AUTHORITY_MAP.md`
3. `docs/architecture/BUNKER_COURT_CASE_MATRIX.md`
4. `docs/architecture/BUNKER_COURT_DISCOVERY_MATRIX.md`
5. `docs/architecture/BUNKER_COURT_IDENTITY_AND_PROVENANCE.md`
6. `docs/architecture/BUNKER_COURT_SAVE_COMPATIBILITY.md`
7. `docs/architecture/PLAN146_REGRESSION_MATRIX.md`
8. `docs/architecture/PLAN146_COMPLETION_REPORT.md`
