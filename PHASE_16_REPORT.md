# ASHFALL — Phase 16 Production Execution Status

**Date:** this phase.
**Goal:** Move from `478 ACTIVE ART REPLACEMENT TARGETS` to **VERIFIED PRODUCTION ART** through the full chain: Authoritative content → Generation spec → Staging → QA → Runtime-size QA → Promotion → Registry verification → Gallery verification → Runtime-context verification → Accepted production asset.

---

## PHASE 16 — PRODUCTION EXECUTION STATUS

### PRE-FLIGHT

| Item | Value |
|---|---|
| Starting actionable queue | **478** |
| Quarantine plan entries | **21** (all QUARANTINE_OK) |
| Deprecated ammo total | 80 (19 KEEP_COMPAT, 9 MERGE_CANDIDATE, 52 SAFE_TO_RETIRE_LATER) |
| Active generic ammo placeholders | 3 (preserved: `item_ammo_ap.jpg`, `item_ammo_hp.jpg`, `item_ammo_standard.jpg`) |
| Generation provider | `arkcli` (BytePlus ModelArk) |
| Generation available | **NO** — `API key status is not active` (`BLOCKED_EXTERNAL_AUTH`) |
| Godot runtime available | **YES** — Godot 4.7.1.stable.mono.official.a13da4feb |

### Material changes since Phase 14/15

- Quarantine plan grew from 20 to **21** entries (the script re-ran this phase and confirmed the additional row `ammo_deprecated_cal_545x39_v2.jpg` which is byte-identical to `dirty_water_flask.jpg`).
- AssetRegistry self-test reports the same `48/48 PASS` floor as Phase 14 (no coverage gained or lost).
- Core test suite grew from 1973 to **1985** PASS (+12 tests in intervening work).
- The prompt composer re-ran this phase and produced 478 fresh prompt JSON files (`docs/visual/generated_prompts/`).
- The gallery re-rendered this phase (5 pages, 124 tiles, all `fallback_status: OK`).
- The orphan recompute re-ran this phase (1741 total orphans — same count as Phase 14, classifications unchanged).
- The runtime-context trace re-ran this phase (`Economy: 2`, `Host: 4`, `Main.cs: 1`, `UI: 1`).

---

### DEPRECATED AMMO

| Item | Value |
|---|---|
| Plan entries | 21 |
| All-decision | 21 × QUARANTINE_OK |
| All-SHA256 match src | 21 / 21 ✅ |
| All-SHA256 match partner | 21 / 21 ✅ |
| All-import-companion present | 21 / 21 ✅ |
| All-live-runtime ref count | 0 / 21 ✅ |
| All-source filesystem presence | 21 / 21 ✅ |
| **Dry-run PASS** | ✅ |
| **Applied** | **21** (moved to `assets/_quarantine_legacy/`) |
| Deferred | 0 |
| Unexpected references | 0 |
| Post-quarantine regressions | 0 |

The 21 entries consist of 20 files byte-identical to `ammo_12ga_ap.jpg` and 1 file (`ammo_deprecated_cal_545x39_v2.jpg`) byte-identical to `dirty_water_flask.jpg`. All 21 are genuinely unhooked from runtime production code.

The remaining 59 deprecated ammo files (80 total - 21 quarantined) are retained:
- 19 KEEP_COMPAT — referenced by `Assets/StreamingAssets/Data/items.json` with `ammo_deprecated_cal_*` ids; these need catalog refactor before retirement.
- 9 MERGE_CANDIDATE — byte-identical but their canonical partner is itself a generic placeholder stem (`ammo_`, `ammo_box`, etc.) — not safe to retire until per-caliber resolution exists.
- 52 SAFE_TO_RETIRE_LATER — no consumer, candidates for a later cleanup phase but not in this evidence-gated batch.

---

### GENERATION PRIORITIES

| Class | Count | Meaning |
|---|---|---|
| P0 | 0 | None synthetically produced. AssetRegistry self-test confirms zero fallback activations on production runtime. |
| **P1** | 163 | Required immediately for active gameplay. |
| P2 | 199 | High-value content completion. |
| P3 | 110 | Secondary / expansion. |
| P4 | 6   | Future / low-frequency. |

The 478 actionable rows split as:

| Family | Count |
|---|---|
| Inventory-Item | 233 |
| Location-Art | 200 |
| NPC-Portrait | 36 |
| Faction-Art | 7 |
| Survivor-Portrait | 2 |

Inventory-Item subfamily distribution: Other 174, Food-Water 26, Special-Resource 13, Crafting-Material 9, Medical 7, Equipment 3, Ammunition 1.

---

### BATCH 1

| Metric | Value |
|---|---|
| Requested | 34 (planned, see `PRODUCTION_ART_PRIORITY.md`) |
| Generated | 0 (BLOCKED_EXTERNAL_AUTH) |
| Accepted | 0 |
| Regenerated | 0 |
| Rejected | 0 |
| Skipped existing | 0 |
| Promoted | 0 |

The batch plan is **family-cohesive** by design: 10 NPC portraits, 10 Food-Water inventory, 8 Salt/Iodine locations, 2 Survivor family, 4 Faction. Total 34.

---

### BY FAMILY

| Family | Requested | Generated | Promoted | Status |
|---|---|---|---|---|
| Inventory-Item | 0 | 0 | 0 | BLOCKED_EXTERNAL_AUTH |
| Medical | 0 | 0 | 0 | BLOCKED_EXTERNAL_AUTH |
| Crafting | 0 | 0 | 0 | BLOCKED_EXTERNAL_AUTH |
| Weapons/ammo | 0 | 0 | 0 | BLOCKED_EXTERNAL_AUTH (replaceable: 1 P1) |
| Survivors | 0 | 0 | 0 | BLOCKED_EXTERNAL_AUTH (2 P1 planned) |
| NPCs | 0 | 0 | 0 | BLOCKED_EXTERNAL_AUTH (10 P1 planned) |
| Locations | 0 | 0 | 0 | BLOCKED_EXTERNAL_AUTH (8 P1 planned) |
| Faction-Art | 0 | 0 | 0 | BLOCKED_EXTERNAL_AUTH (4 P2 planned) |
| Other | 0 | 0 | 0 | BLOCKED_EXTERNAL_AUTH |

---

### QUALITY

| Bucket | Count |
|---|---|
| Technical PASS | 0 (no candidates) |
| Semantic PASS | 0 |
| Style PASS | 0 |
| Duplicate PASS | 0 |
| Runtime-size PASS | 0 |

The QA harness is exercised and reports 0 / 0 / 0 / 0 / 0 / 0 / 0 across the seven buckets (corrupt, bad-dim, near-solid, exact-staged-dup, exact-prod-dup, perceptual-dup, production-overlap).

---

### WIRING

| Metric | Value |
|---|---|
| Visual rows (deduped across catalogs) | 1208 |
| Resolved (direct + alias) | 591 |
| Missing (no asset at any resolved path) | 617 |
| Actionable in queue | 478 |
| Reference-only skipped | 136 |
| Resolved before quarantine | 591 |
| Resolved after quarantine | 591 |
| Newly resolved | 0 |
| Still actionable | 478 |
| Wrong mappings | 0 |
| Ambiguous | 0 |
| Fallbacks | 0 (production runtime has zero fallback activations) |

The quarantine moves 21 files to `_quarantine_legacy/`. These files were not in the actionable queue (they were identical to existing canonical assets), so the queue count is unchanged.

---

### GALLERY

| Metric | Value |
|---|---|
| Pages before | 5 |
| Pages after | 5 |
| Assets inspected | 124 |
| Style outliers | 0 |
| Wrong mappings | 0 |
| Failures | 0 |

Gallery renders:
- `snapshots/gallery_inventory_item_p01.png` (36 tiles)
- `snapshots/gallery_location_art_p02.png` (36 tiles)
- `snapshots/gallery_survivor_portrait_p03.png` (36 tiles)
- `snapshots/gallery_npc_portrait_p04.png` (12 tiles)
- `snapshots/gallery_faction_art_p05.png` (4 tiles)

All `fallback_status: OK`. Index: `snapshots/gallery_index.json`.

---

### RUNTIME CONTEXT

| Metric | Value |
|---|---|
| Assets checked | 0 (no candidates promoted) |
| Correct | 0 |
| Incorrect | 0 |
| Blocked | 0 |

The runtime-context trace shows the panels that DO reach AssetRegistry at runtime:

| Panel dir | Methods | Calls |
|---|---|---|
| `Economy` | GetItem | 2 |
| `Host` | GetFaction, GetItem, GetLocation, GetPortrait | 4 |
| `Main.cs` | GetItem | 1 |
| `UI` | GetItem | 1 |

`--ui-snapshot-uitest` is **BLOCKED_ENVIRONMENT** — Godot's dummy renderer cannot service `Texture2D.GetImage()` in headless mode. This is a pre-existing limitation, not introduced by Phase 16.

---

### PLACEHOLDERS

| Asset | Status |
|---|---|
| `item_ammo_ap.jpg` | KEEP_ACTIVE_PLACEHOLDER |
| `item_ammo_hp.jpg` | KEEP_ACTIVE_PLACEHOLDER |
| `item_ammo_standard.jpg` | KEEP_ACTIVE_PLACEHOLDER |
| Total active placeholders | 3 |
| Still required | 3 (per-caliber resolution not yet in place) |
| Retire candidates | 0 |

The three ammo placeholders continue to participate in generic ammo-category resolution. They are de-prioritised for now because the per-caliber coverage is still sparse (only `ammo_12ga_ap.jpg` is a real active caliber asset; the rest of the caliber stem is `ammo_`/`ammo_box` — generic placeholders themselves).

---

### DUPLICATES

| Metric | Value |
|---|---|
| Exact groups | 182 (per `production_duplicate_plan.py`) |
| New consolidation candidates | 0 (no new candidates promoted) |
| Deferred | 182 (broad cleanup deferred — Phase 17+) |

The 21 quarantined files were byte-identical to existing canonical assets, so the duplicate landscape is unchanged post-quarantine: the ALREADY-CANONICAL files remain the source of truth, and the deprecated copies are now isolated.

---

### ORPHANS

| Metric | Value |
|---|---|
| Recomputed | 1741 |
| TRUE_ORPHAN | 1244 |
| LEGACY | 99 |
| FUTURE_CONTENT | 19 |
| FIGMA | 317 |
| STITCH | 62 |
| Unknown | 0 |

The 21-file move did not change the orphan count because the deprecated files were already excluded from the orphan classification (they were deprecated references, not legacy/inventory).

---

### REGRESSION

| Test | Result |
|---|---|
| `dotnet build Ashfall.csproj` | 0 errors / 0 warnings |
| `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | 0 errors / 0 warnings |
| `dotnet test Ashfall.Core.Tests` | 1985 / 1985 PASS |
| `--asset-registry-selftest` | 48 / 48 PASS (0 missing, 0 load-failed) |
| `--data-integrity-selftest` | 0 errors / 0 warnings across 94 catalogs |
| `--bridge-selftest` | 41 / 41 PASS |
| `--ui-snapshot-uitest` | BLOCKED_ENVIRONMENT (headless dummy renderer) |
| Gallery | 5 pages / 124 tiles / 0 failures |

The 1985 figure is **+12 over the Phase 14 1973** floor. The AssetRegistry selftest runs end-to-end this phase (was previously observed to be partial) — 48/48 PASS.

---

### FILES CREATED

- `docs/visual/PRODUCTION_ART_PRIORITY.md` (Phase 16 — updated)
- `docs/visual/ART_FAMILY_REFERENCE_GUIDE.md` (new)
- `docs/visual/PRODUCTION_ART_GENERATION_LEDGER.md` (Phase 16 — updated)
- `docs/visual/PHASE_16_REPORT.md` (this file)
- `VISUAL_QA_REPORT.md` (Phase 16 — replaced)

### FILES MODIFIED

- `scripts/ci/quarantine_deprecated_ammo.sh` (fixed space-in-path quoting — see DO NOT DO caveat: the script is a tool, not a production file)
- `docs/visual/PRODUCTION_ART_GENERATION_MANIFEST.json` (re-emitted by `production_manifest.py`)
- `docs/visual/PRODUCTION_ART_GENERATION_LEDGER.json` (re-emitted)
- `docs/visual/ASSET_REPLACEMENT_QUEUE.md` (re-emitted)
- `docs/visual/RUNTIME_CONTEXT_TRACE.md` (re-emitted)
- `docs/visual/_qa/*` (re-emitted)
- `docs/visual/ORPHAN_VISUAL_ASSETS.md` (re-emitted)
- `docs/visual/DUPLICATE_CONSOLIDATION_PLAN.md` (re-emitted)
- `docs/visual/_phase13_missing_classification.json` (re-emitted)
- `snapshots/gallery_*.png` (re-rendered, 5 pages)
- `snapshots/gallery_index.json` (re-emitted)
- `docs/visual/generated_prompts/*.json` (478 prompt files re-emitted)

### FILES MOVED (quarantine)

- 21 `.jpg` files: `assets/art/ammo_deprecated_*.jpg` → `assets/_quarantine_legacy/ammo_deprecated_*.jpg`
- 21 `.import` files: `assets/art/ammo_deprecated_*.jpg.import` → `assets/_quarantine_legacy/ammo_deprecated_*.jpg.import`

(All 42 moves are tracked as `R` rename status in `git status`.)

---

### QUEUE

| Metric | Value |
|---|---|
| Starting | 478 |
| Completed | 0 |
| Removed stale | 0 |
| Blocked | 478 (BLOCKED_EXTERNAL_AUTH) |

The 478 actionable rows are reconciled against current repository state: every row satisfies `art genuinely absent` (target_filename missing on disk), `not reference-only` (not in the 136 SKIP_REFERENCE_ONLY set), `not deprecated` (not in the 80 deprecated ammo files), `not already satisfied by newly discovered art` (canonical filename absent), `not a duplicate mapping` (no alias covering it), `not intentionally visual-less` (canonical filename and target directory derivable).

---

### NEXT MAJOR PHASE

**Phase 17 — First Real Generation Batch** (when auth is renewed)

1. **Auth probe** — confirm `arkcli +gen` returns 200 with a 1-image test.
2. **Batch 1** — 24–36 assets family-cohesive (per `PRODUCTION_ART_PRIORITY.md`).
3. **QA gate** — `production_qa.py` filters down to QA-PASS only.
4. **Promotion** — `production_promote.py --id <content_id>` writes canonical filename.
5. **Wiring re-trace** — `visual_wiring_postfix.py` confirms zero MISSING for the batch.
6. **Gallery** — `production_gallery_render.py` regenerates contact sheets.
7. **Runtime-context** — promote and verify each promoted asset in the actual panel that references it.
8. **Ledger** — `production_ledger.py` records each attempt.
9. **Halt condition** — pause if >20% semantic failures in any single family.

**Phase 18 — Coherent Family Completion**

1. Pick 3–5 families with anchor assets present (e.g. Medical, Food-Water, Salt/Iodine Locations).
2. Generate complete sets per family (10–20 each).
3. Each set is a single visual style-pass.
4. Verify the family reads coherent at the gallery level.

**Phase 19 — Bulk Generation**

1. Scale to 100+ assets per quarter.
2. Introduce style-anchor auto-rebuild for families with weak anchors.
3. Begin promoting per-caliber ammo art so the generic placeholders can retire.

**Phase 20 — Deprecated Asset Audit Pass**

1. Address the 19 KEEP_COMPAT deprecated caliber files (catalog refactor).
2. Address the 9 MERGE_CANDIDATE files whose canonical partner is itself a generic placeholder.
3. Re-classify 52 SAFE_TO_RETIRE_LATER files.

---

## DO NOT DO reminders preserved

- Refrain from restarting AssetRegistry normalization work.
- Refrain from resurrecting the disproven prefix-strip theory.
- Refrain from deleting all 80 deprecated ammo files (only 21 evidence-strong).
- Refrain from deleting the three active generic ammo placeholders.
- Refrain from mass-generating all 478 assets in one pass.
- Refrain from generating reference-only content.
- Refrain from generating art directly into production directories.
- Refrain from promoting QA failures.
- Refrain from accepting style-inconsistent art.
- Refrain from treating gallery rendering as runtime proof.
- Refrain from creating hundreds of new manual AssetRegistry aliases.
- Refrain from renaming canonical content IDs.
- Refrain from changing game balance or simulation.
- Refrain from removing legacy/reference files merely to reduce orphan metrics.
- Refrain from hiding blocked generation behind fabricated assets.
- Refrain from reporting a generated candidate as production-complete before wiring verification.

---

## Status

Phase 16 evidence-gated deliverables are complete:

1. ✅ Evidence-strong deprecated-ammo bucket safely reverified.
2. ✅ Applied cleanup is quarantine-only and regression-checked.
3. ✅ Active generic ammo placeholders remain protected.
4. ✅ Current 478-row queue is reconciled.
5. ❌ Real generation batch — BLOCKED_EXTERNAL_AUTH.
6. ⚠️ Generation uses staging/provenance — pipeline is in place, no candidates executed.
7. ⚠️ Promoted QA passes — no candidates to promote.
8. ⚠️ Runtime-size readability — no candidates to verify.
9. ⚠️ Promoted assets resolve through canonical registry — no candidates to verify.
10. ✅ Gallery contact sheets regenerated and inspected.
11. ❌ Representative assets checked in actual runtime context — no candidates promoted.
12. ✅ Queue counts and audit documents updated from current truth.
13. ✅ Expanded AssetRegistry runtime selftest finally executed (48/48 PASS).
14. ✅ No simulation/game-design behaviour altered.
15. ✅ No destructive mass cleanup occurred.
16. ✅ All executable regression gates green (builds + tests + bridge + data + AssetRegistry).

**Phase 16 has reached the floor of "all-green structural pipeline" pending an authenticated image-generation tool.** The seed for the first batch is in place; it will fire automatically when the API key is renewed.
