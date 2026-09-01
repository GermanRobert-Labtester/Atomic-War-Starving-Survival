# Plan 17 — Regression Matrix

Maps every Plan 17 deliverable to its test coverage, verification gate, and regression risk.

## Test Coverage Map

### Task 17A — Environmental Text Coverage Audit

| Deliverable | Test Class | Test Count | Status |
|-------------|-----------|:----------:|--------|
| Baseline audit | `PLAN17_BASELINE.md` | — | ✅ Documented |
| Location coverage matrix | `LOCATION_ATMOSPHERE_COVERAGE.md` | — | ✅ Documented |
| Atmosphere text loader | `Plan17BAtmosphereTests` | 10 | ✅ PASS |
| Atmosphere text consumer | `Plan17BAtmosphereTests` | 10 | ✅ PASS |

### Task 17B — Per-Location Atmosphere Expansion

| Deliverable | Test Class | Test Count | Status |
|-------------|-----------|:----------:|--------|
| 152 existing texts wired | `Plan17BAtmosphereTests` | 10 | ✅ PASS |
| 30+ new texts | — | — | ❌ NOT DONE (content authoring) |

### Task 17C — Visit-State & Location-Memory Variants

| Deliverable | Test Class | Test Count | Status |
|-------------|-----------|:----------:|--------|
| State matrix documented | `ENVIRONMENTAL_STATE_MATRIX.md` | — | ✅ Documented |
| State-aware selection | `Plan17BAtmosphereTests` | 3 | ✅ PASS |

### Task 17F — Archive Ink Expansion

| Deliverable | Test Class | Test Count | Status |
|-------------|-----------|:----------:|--------|
| Current 3 inks craftable | `Plan17DArchiveTests` | 10 | ✅ PASS |
| Ink balance analysis | `ARCHIVE_INK_BALANCE.md` | — | ✅ Documented |
| Expansion 3→12 | — | — | ❌ NOT DONE (content authoring) |

### Task 17G — Discoverable Document Expansion

| Deliverable | Test Class | Test Count | Status |
|-------------|-----------|:----------:|--------|
| Discovery matrix | `DOCUMENT_DISCOVERY_MATRIX.md` | — | ✅ Documented |
| 15 new documents | — | — | ❌ NOT DONE (content authoring) |

### Task 17K — World History Chronology

| Deliverable | Test Class | Test Count | Status |
|-------------|-----------|:----------:|--------|
| Chronology documented | `WORLD_HISTORY_CHRONOLOGY.md` | — | ✅ Documented |
| 79 entries sorted | `WORLD_HISTORY_CHRONOLOGY.md` | — | ✅ Documented |

### Task 17L — Codex Conversion

| Deliverable | Test Class | Test Count | Status |
|-------------|-----------|:----------:|--------|
| Conversion matrix | `CODEX_CONVERSION_MATRIX.md` | — | ✅ Documented |
| 40+ entries authored | — | — | ❌ NOT DONE (content authoring) |

### Task 17O — Spoiler Gating

| Deliverable | Test Class | Test Count | Status |
|-------------|-----------|:----------:|--------|
| Spoiler boundaries | `SPOILER_AND_DISCOVERY_BOUNDARIES.md` | — | ✅ Documented |
| 5-tier classification | `SPOILER_AND_DISCOVERY_BOUNDARIES.md` | — | ✅ Documented |

### Task 17P — Provenance & Contradiction

| Deliverable | Test Class | Test Count | Status |
|-------------|-----------|:----------:|--------|
| Provenance taxonomy | `RUNTIME_LORE_PROVENANCE.md` | — | ✅ Documented |
| Contradiction audit | `CANON_CONTRADICTION_AUDIT.md` | — | ✅ Documented |

### Task 17T — Dead Corpus Recovery

| Deliverable | Test Class | Test Count | Status |
|-------------|-----------|:----------:|--------|
| Utilization audit | `LORE_CONTENT_UTILIZATION.md` | — | ✅ Documented |
| Atmosphere texts wired | `AtmosphereTextSystem.cs` | 10 | ✅ PASS |
| Orphan items fixed | `items.json` | — | ✅ 11 items added |

### Cross-Plan Tests

| Deliverable | Test Class | Test Count | Status |
|-------------|-----------|:----------:|--------|
| Lore baseline integrity | `Plan17ALoreBaselineTests` | 10 | ✅ PASS |
| Codex/journal system | `Plan17CCodexTests` | 10 | ✅ PASS |
| Archive desk pipeline | `Plan17DArchiveTests` | 10 | ✅ PASS |
| **Total Plan 17 tests** | **4 test files** | **42** | **✅ ALL PASS** |

## Verification Gates

| Gate | Command | Expected | Status |
|------|---------|----------|--------|
| Build | `dotnet build Ashfall.Core.Tests` | 0 errors, 0 warnings | ✅ PASS |
| Plan 17 tests | `dotnet test --filter Plan17` | 42/42 pass | ✅ 42/42 PASS |
| Full test suite | `dotnet test Ashfall.Core.Tests` | All pass | ✅ 5408/5408 PASS |
| Data integrity | `godot --headless --path . -- --data-integrity-selftest` | 0 errors | Pending |
| Bridge selftest | `godot --headless --path . -- --bridge-selftest` | Exit 0 | Pending |

## Regression Risks

| Risk | Impact | Mitigation | Test Coverage |
|------|--------|------------|---------------|
| AtmosphereTextSystem adds new Core dependency | Build coupling | Pure Core, no engine refs | Plan17B tests |
| New items in items.json break existing refs | Data integrity | CatalogIntegrityValidator | Baseline tests |
| Orphan item fix changes trade balance | Economy shift | Items use conservative values | Manual review |
| Documentation drifts from implementation | Misleading docs | Cross-reference tests | Regression matrix |
| Dev-lore conversion leaks spoilers | Player experience | Spoiler tier gating | SPOILER doc |
| Mixed loc_/location_ prefix causes validation errors | Data integrity | Standardize to loc_* | Canon audit |

## Documentation Deliverables

| # | Document | Status |
|---|----------|--------|
| 1 | `PLAN17_BASELINE.md` | ✅ Created |
| 2 | `RUNTIME_LORE_PROVENANCE.md` | ✅ Created |
| 3 | `LOCATION_ATMOSPHERE_COVERAGE.md` | ✅ Created |
| 4 | `ENVIRONMENTAL_STATE_MATRIX.md` | ✅ Created |
| 5 | `ARCHIVE_CATALOG_AUDIT.md` | ✅ Created |
| 6 | `ARCHIVE_INK_BALANCE.md` | ✅ Created |
| 7 | `DOCUMENT_DISCOVERY_MATRIX.md` | ✅ Created |
| 8 | `WORLD_HISTORY_CHRONOLOGY.md` | ✅ Created |
| 9 | `CODEX_CONVERSION_MATRIX.md` | ✅ Created |
| 10 | `SPOILER_AND_DISCOVERY_BOUNDARIES.md` | ✅ Created |
| 11 | `CANON_CONTRADICTION_AUDIT.md` | ✅ Created |
| 12 | `LORE_CONTENT_UTILIZATION.md` | ✅ Created |
| 13 | `PLAN17_REGRESSION_MATRIX.md` | ✅ This file |
| 14 | `PLAN17_COMPLETION_REPORT.md` | ✅ Created |

**14/14 documentation deliverables created.**

## Code Deliverables

| # | File | Lines | Status |
|---|------|-------|--------|
| 1 | `AtmosphereCatalogLoader.cs` | 103 | ✅ Created |
| 2 | `AtmosphereTextSystem.cs` | 254 | ✅ Created |
| 3 | `Plan17ALoreBaselineTests.cs` | ~120 | ✅ Created |
| 4 | `Plan17BAtmosphereTests.cs` | ~150 | ✅ Created |
| 5 | `Plan17CCodexTests.cs` | ~130 | ✅ Created |
| 6 | `Plan17DArchiveTests.cs` | ~140 | ✅ Created |
| 7 | `items.json` (11 new items) | — | ✅ Modified |

## Deferred Follow-Ups

- 30+ new environmental texts for high-exposure locations (content authoring)
- 9 new archive inks (3→12 expansion)
- 15+ discoverable documents with placement
- 40+ codex entries from dev-lore conversion
- environmental_texts_expansion_05.json loader
- OralLoreCatalog host wiring
- DailySurvivalCatalog IFileIO fix + host wiring
- Weather-reactive atmosphere variants
- Dynamic graffiti/notices
- Deep-lore location entries
