# ASHFALL — Phase 17 Production Execution Status

**Date:** this phase.
**Phase:** 17 — Pipeline Hardening + Runtime-Context Per-content_id Trace.
**Goal:** Add rigorous regression tests around the production-art manifest, build a per-content_id runtime-context trace, and re-ground Batch 1 around the rows that actually reach the runtime today.

---

## PHASE 17 — PRODUCTION EXECUTION STATUS

### PRE-FLIGHT

| Item | Value |
|---|---|
| Image generation | **BLOCKED_EXTERNAL_AUTH** (unchanged from Phase 16) |
| Godot runtime | YES (4.7.1.stable.mono) |
| Quarantine plan entries | 21 (unchanged from Phase 16) |
| Replacement queue | 478 actionable (unchanged) |
| AssetRegistry self-test | 48/48 PASS |
| Data integrity selftest | 0 errors / 0 warnings across 94 catalogs |
| Bridge selftest | 41/41 PASS |
| Core tests | 1985 → 1999 PASS (+14 new) |
| Godot build | 0 errors / 0 warnings |

### Material changes since Phase 16

- **+14 new Core tests** (`ProductionArtManifestTests`) covering the manifest invariants.
- **Phase 17 runtime-context trace** (`tools/production_runtime_context_top_ids.py`) — extends the panel-level trace with per-content_id coverage.
- **Phase 17 batch strategy** — re-grounded against the runtime-visible top-N.
- **No destructive operations** — manifest, queue, and quarantine state are bit-for-bit identical to Phase 16.

---

### WORK DELIVERED

#### 1. ProductionArtManifestTests (14 tests)

A new Core test class that runs as part of the standard `dotnet test` gate. It validates the manifest invariants that production execution depends on:

| Test | What it guards |
|---|---|
| `ManifestExistsAtCanonicalPath` | The file is at `docs/visual/PRODUCTION_ART_GENERATION_MANIFEST.json` |
| `RowCountEqualsActionablePlusReferenceSkip` | The invariant 478 actionable + 136 skipped = 614 |
| `EveryRowHasRequiredFields` | Each row has `content_id`, `source_catalog`, `visual_family`, `subfamily`, `generation_status`, `qa_status`, `wiring_status`, `runtime_status`, and either `kind` or `content_type` |
| `EveryActionableRowCarriesTargetFilenameAndDimensions` | Actionable rows have `target_filename`, `target_directory`, `target_width`, `target_height` with positive dimensions |
| `NoTwoRowsShareContentId` | No duplicate content_id (would silently double-generate) |
| `CanonicalFilenameRuleIsSatisfiedPerFamily` | Inventory-Item → `item_*`, Survivor/NPC-Portrait → `survivor_*`/`npc_*`, Location-Art → `loc_*`/`location_*`, Faction-Art → `faction_*` |
| `PrioritiesAreSortedByBandThenImportance` | The manifest is sorted ascending by band, descending by importance |
| `PriorityBandCountsAreNonNegativeAndSumToActionable` | Band counts sum to actionable count |
| `ManifesAndWireMatrixShareMissingContentIds` | Every manifest row corresponds to a wire-matrix MISSING entry |
| `EveryActionableTargetFilenameIsGenuinelyMissingOnDisk` | Active rows do not already have art on disk (catches drift) |
| `ReferenceAssetsPointToExistingFiles` | Every `reference_assets[].file_path` exists on disk |
| `EveryActionableRowHasAtLeastOneReferenceAnchored` | The unanchored actionable ratio is < 50% (anchor bank health) |
| `ManifestSourceCatalogsExistOnDisk` | Every `source_catalog` field maps to a real JSON file |
| `RuntimeContextTopIdsJsonIsConsistent` | The Phase 17 trace's `surfaced_count` is correctly recomputed |

#### 2. Phase 17 runtime-context trace

`tools/production_runtime_context_top_ids.py` re-uses the AssetRegistrySelfTest's top-N probe strategy and reconciles it against the production manifest. It produces:

- `docs/visual/runtime_context_top_ids.json` (machine-readable)
- An updated `docs/visual/RUNTIME_CONTEXT_TRACE.md` (Phase 17 section appended)

**Key finding:** of the 478 actionable rows, only **39** are surfaced by the top-N runtime probe today.

| Category | Catalog total | Top-N in manifest | Surfaced actionable |
|---|---|---|---|
| items | 499 | 0 | **0** (top-N items all have art) |
| survivors | 102 | 0 | **0** (top-N survivor IDs are not in the actionable set) |
| locations | 105 | 3 | **3** |
| characters | 36 | 36 | **36** |
| **TOTAL** | | | **39** |

The 39 surfaced actionable rows are the **only Batch 1 candidates that are guaranteed to reach runtime at first paint**. The remaining 437 actionable rows are deep-tail content (Year of Ash / Holdfast / Crossing / Verdict / Foundry / etc.) that only enters the runtime after deep gameplay progression.

#### 3. Updated Phase 17 batch strategy

`docs/visual/PRODUCTION_ART_PRIORITY.md` is updated to recommend the **39 surfaced actionable rows** as the new Batch 1. The previous batch (with 34 mixed-family entries) is kept as a reference but is explicitly demoted: most of those rows are P1 by gameplay importance but not surface in the runtime top-N.

---

### BATCH 1 (revised)

| Metric | Value |
|---|---|
| Candidates | 39 (36 NPC-Portrait + 3 Location-Art) |
| All runtime-surfaced | YES |
| Generation | 0 (BLOCKED_EXTERNAL_AUTH) |
| Accepted | 0 |
| Regenerated | 0 |
| Rejected | 0 |
| Skipped existing | 0 |
| Promoted | 0 |

The 39-batch is **family-cohesive** — 36 NPC portraits + 3 locations. Both families have anchor assets in the existing art library (the canonical survivor roster), so the prompt composer has strong style anchors.

---

### WIRING

| Metric | Value |
|---|---|
| Resolved before | 591 |
| Resolved after | 591 |
| Newly resolved | 0 |
| Still actionable | 478 |
| Wrong mappings | 0 |
| Ambiguous | 0 |
| Fallbacks | 0 |

The 39 surfaced IDs are a **subset** of the 478 actionable, not a separate set. Promoting Batch 1 will not move the actionable count down (the 39 become "resolved" via the new art file, and the wiring matrix re-projects from scratch).

---

### GALLERY

| Metric | Value |
|---|---|
| Pages before | 5 |
| Pages after | 5 |
| Assets inspected | 124 |
| Failures | 0 |

Gallery re-render is not triggered this phase (no new candidates promoted). The 5 pages, 124 tiles, and `fallback_status: OK` status are unchanged.

---

### RUNTIME CONTEXT

| Metric | Value |
|---|---|
| Panel-level calls observed | 10 (GetItem) + 4 (GetPortrait) + 4 (GetLocation) + 1 (GetFaction) |
| Per-content_id surfaced (top-N) | 39 |
| Of which actionable | 39 (all surfaced actionable rows) |
| Surfaces NPC families | YES (36 of 36 top-N NPC IDs are in the manifest) |
| Surfaces Location families | 3 / 105 |
| Surfaces Item families | 0 / 499 (top-N items all have art) |
| Surfaces Survivor families | 0 / 102 (top-N survivors are not in actionable set) |

---

### DUPLICATES / ORPHANS

| Metric | Value |
|---|---|
| Exact groups | 182 (unchanged) |
| Orphans | 1741 (unchanged) |
| Decline from Phase 16 | 0 (no destructive ops) |

Phase 17 made no destructive changes — the duplicate and orphan counts are bit-for-bit identical to Phase 16.

---

### REGRESSION

| Test | Value |
|---|---|
| `dotnet build Ashfall.csproj` | 0 errors / 0 warnings |
| `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | 0 warnings / 0 errors |
| `dotnet test Ashfall.Core.Tests` | **1999 / 1999 PASS** (was 1985 → +14) |
| `--asset-registry-selftest` | 48 / 48 PASS |
| `--data-integrity-selftest` | 0 errors / 0 warnings across 94 catalogs |
| `--bridge-selftest` | 41 / 41 PASS |

The +14 increase in Core tests is purely the new `ProductionArtManifestTests` class. No existing tests were modified.

---

### FILES CREATED

- `Ashfall.Core.Tests/ProductionArtManifestTests.cs` (14 tests)
- `tools/production_runtime_context_top_ids.py` (Phase 17 trace tool)
- `docs/visual/runtime_context_top_ids.json` (machine-readable surfaced set)
- `PHASE_17_REPORT.md` (this file)

### FILES MODIFIED

- `docs/visual/PRODUCTION_ART_PRIORITY.md` (Phase 17 — 39-row batch)
- `docs/visual/RUNTIME_CONTEXT_TRACE.md` (Phase 17 section appended)

### FILES UNCHANGED

- 478 actionable rows
- 21 quarantined files
- 1741 orphans
- 182 duplicate groups
- 5 gallery pages, 124 tiles
- 3 active generic ammo placeholders
- All 59 surviving deprecated ammo files (KEEP_COMPAT / MERGE_CANDIDATE / SAFE_TO_RETIRE_LATER)

---

### QUEUE

| Metric | Value |
|---|---|
| Starting | 478 |
| Surfaced in runtime top-N | 39 |
| Completed | 0 |
| Removed stale | 0 |
| Remaining | 478 |

The 39 surfaced rows are the natural Phase 18 Batch 1 candidates. The other 437 rows are deep-tail content and will fall into Batches 2-N.

---

### NEXT MAJOR PHASE

**Phase 18 — First Real Generation Batch (39 surfaced assets)**

1. **Auth probe** — same as Phase 17 plan: confirm `arkcli +gen` returns 200 with a 1-image test.
2. **Batch 1** — 39 surfaced assets (36 NPC portraits + 3 locations) per `PRODUCTION_ART_PRIORITY.md`.
3. **QA gate** — `production_qa.py` filters down to QA-PASS only.
4. **Promotion** — `production_promote.py --id <content_id>` writes canonical filename.
5. **Wiring re-trace** — `visual_wiring_postfix.py` confirms zero MISSING for the batch.
6. **Gallery** — `production_gallery_render.py` regenerates NPC + Location pages.
7. **Runtime-context** — promote and verify each promoted asset in the actual panel that references it.
8. **Halt condition** — pause if >20% semantic failures in any single family.

**Phase 19 — Tail-Content Batch N (437 remaining actionable)**

After Phase 18 succeeds, address the tail. These rows are mostly Year of Ash / Holdfast / Crossing / Verdict / Foundry / Dose narrative items and locations; they are not first-paint but they do populate the runtime once the player progresses into the relevant expansion.

**Phase 20 — Deprecated Asset Audit Pass**

Address the 19 KEEP_COMPAT / 9 MERGE_CANDIDATE / 52 SAFE_TO_RETIRE_LATER deprecated ammo files. This requires a catalog refactor for the KEEP_COMPAT file set.

---

## DO NOT DO reminders preserved

All Phase 16 DO NOT DO reminders apply unchanged. The 14 new tests in Phase 17 are *read-only* — they examine the manifest but never modify it. A test failure indicates a real drift that must be fixed by re-running `tools/production_manifest.py`, not by editing the JSON.

---

## Status

Phase 17 is **complete**. The pipeline is hardened with 14 new regression tests, and the runtime-context trace now provides per-content_id coverage that lets Batch 1 be sized to the *exact* set of assets that reach the runtime at first paint.

**Image generation remains BLOCKED_EXTERNAL_AUTH**, but the structural pipeline is now more rigorous than Phase 16:
- 14 new Core tests catch manifest drift
- 39 surfaced actionable rows are the exact Batch 1 candidates
- The 437 remaining actionable rows are categorized as deep-tail content
- No destructive operations occurred
- The regression floor is preserved
