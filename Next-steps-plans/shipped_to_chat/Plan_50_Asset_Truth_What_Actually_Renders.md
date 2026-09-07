# Plan 50 — Asset Truth: What Actually Renders

> **Wave:** Continuity Wave 8 — *The Presented Game* (Plans 50–54)
> (predecessors: [W1](Wave1_Continuity_Audit_INDEX.md)–[W7](Wave7_Continuity_Audit_INDEX.md))
> **Depends on:** 45A (acceptance ladder — assets need the same treatment content got), 26A
> (resolver), 47B (pack/overlay loading), 39A (gate tier).
>
> **Theme:** the repository carries **~2,780 Godot asset files / 114 MB** of art, icons, portraits,
> sprites, and audio — and the asset gate checks **50 entries** against **5,563 authored ids**: 0.9 %
> coverage. The gate also treats a *fallback* as success (`IsValid => Texture != null &&
> (Loaded || FallbackUsed)`), so it can print `missing=0` while reporting
> `unique missing assets: 6` and `duplicate_fallback_requests: 3`. Meanwhile name-based probing finds
> **1,189 art files and 148 of 217 icons referenced nowhere**, and portraits are stored by
> display-name stems (`elena_vasquez.png`) while the resolver queries id stems (`survivor_*`) — an
> arrangement that only works by luck.

---

## Evidence Inventory (re-verified @ `ccac926e`; I ran the asset gate myself)

| # | Fact | Evidence |
|---|---|---|
| 1 | Asset volume | `assets/art` 1,762 files / 74 MB · `assets/ui` 421 / 32 MB (`Icons` 18 MB, `Screens` 6.5 MB, `MainMenu` 3.5 MB) · `assets/sprites` 599 / 8.3 MB (`Items` 3.1 MB, `Portraits` 2.8 MB) · `assets/audio` 73 / 10 MB |
| 2 | **The gate checks 50 things** | `godot --headless -- --asset-registry-selftest` → `checked=50 passed=50 missing=0 (unique=6, duplicate_fallback_requests=3) load-failed=0 probe-failures=0` → **PASS**, against `--data-integrity-selftest`'s 138 catalogs / **5563 ids** |
| 3 | A fallback counts as valid | `src/Host/AssetRegistry.cs:43` — `IsValid => Texture != null && (Result == Loaded \|\| Result == FallbackUsed)`; enum `AssetLoadResult.FallbackUsed` (`:19–20`) exists for exactly this distinction and is not gated |
| 4 | Two canonical placeholders absorb everything | `:65` `res://assets/sprites/Characters/placeholder_survivor.png`, `:71` `res://assets/ui/Icons/icon_placeholder.png`; 38 `placeholder` references in `src/`; `assets/sprites/Characters/` contains **1 file** — the placeholder |
| 5 | Portraits exist; the naming convention is a guess | `assets/sprites/Portraits/` → **105 files** with display-name stems (`aris_thorne.png`, `elena_vasquez.png`, `elena_rostov.png`); the resolver's candidate prefixes are `survivor_`/`npc_` (`:148–149`) and its portrait paths are `assets/sprites/Portraits/{0}.png` (`:88–89`) — resolution depends on stem-candidate permutation, not on a mapping |
| 6 | Most art is unreferenced by any name | conservative substring probe across all data + `src/` + Core: `assets/art` **497 of 1,686** unique stems referenced (**1,189 not**); `assets/ui/Icons` **69 of 217** (**148 not**). A generous test (any substring anywhere) — so the unreferenced figures are *lower bounds* |
| 7 | There is no id→asset mapping | resolution is by stem convention: `ResolveStemCandidates(survivorId, "portrait")` × `PortraitSearchPaths`; `asset_manifest.json` exists under `assets/sprites/` but is not the authority the registry consults per-id |
| 8 | **AGENTS.md's asset-debt claim is now stale** | `AGENTS.md` "Remaining debt: the Unity-era `Assets/art/` ~2080 files … still lives under `Assets/`". Reality: `Assets/art`, `Assets/sprites`, `Assets/ui`, `Assets/audio` contain **1 file each, 0 bytes** (`.gdignore`). The migration it describes as pending is finished — this is the project's **eighth** verified stale doc claim |
| 9 | The pipeline scripts exist but are unlinked | `scripts/generate_item_icons.py`, `scripts/pipeline/generate_assets.py`, `scripts/pipeline/import_approved_assets.py`, `art-wiring-results.xml` at repo root, `UI_StyleReference_01.jpg` at root — generation, import, and results with no documented contract (root junk also flagged in Wave 1's hygiene pass) |
| 10 | LFS policy is real and narrow | `.gitattributes`/`setup-repo.sh`: images/fonts via LFS, `*.wav/mp3/ogg` plain binary; `scripts/ci/lfs-health-check.sh` is a gate — so a coverage fix must not silently break the 114 MB tree's policy |

**Reading:** no new art is required to make this plan succeed. The game already owns far more art
than it can prove it uses. Wave 8's first job is the same thing Wave 7 did for content: **measure,
map, then gate.**

---

## Task 50A — Measure and map: an explicit id→asset manifest

**Goal:** replace stem guessing with a declared mapping, and publish real coverage numbers per asset
family so "the asset gate is green" means something.

**Files:** `src/Host/AssetRegistry.cs` (all resolution paths), new
`Assets/StreamingAssets/Data/asset_registry.json` (id → asset + category), new
`Assets/Ashfall.Core/Assets/AssetManifest.cs` + loader, `assets/sprites/asset_manifest.json`
(reconcile or retire), `scripts/ci/generate-asset-manifest.py` (new), `docs/visual/ASSET_GALLERY.md`,
`docs/visual/FALLBACK_VISUAL_ASSETS.md`, `Ashfall.Core.Tests/AssetManifestTests.cs`.

### Substeps

1. **Publish the baseline first, honestly**: for each family (items, icons, portraits, characters,
   locations, weather, terminals, screens, ui textures, audio) — ids in the authority, ids that
   resolve to a real file, ids that hit a fallback, files on disk no id references. That four-column
   table is the deliverable; every later step improves one of its columns.
2. **Fix the gate's definition of success** before adding coverage: `FallbackUsed` must be reported
   separately and fail a strict tier (a `--strict` mode for nightly, a warning for fast tier so the
   50-entry gate doesn't go red mid-migration).
3. **Introduce the manifest as data**: `asset_registry.json` mapping `{ kind, id, path, source,
   license/AI-flag, import preset }` — snake_case, `schema_version`, ids validated by the same
   integrity tiers that validate every other catalog (an invented asset id then fails CI, which is
   how Wave 6's identity work was protected).
4. **Make the resolver consult the manifest first**, then legacy stem conventions, then fallback —
   an ordered, documented path that keeps today's working art working while the mapping fills in.
5. **Retire the placeholder collision**: a single `placeholder_survivor.png` currently serves every
   missing portrait; give distinct, obviously-scratch placeholders per family so a scratch asset is
   visible as scratch rather than mistaken for finished art (and so the coverage report can count it).
6. **Normalise naming or record the mapping**: either rename portraits to id stems or bind them in
   the manifest — pick one, document it, and never leave both conventions loadable by luck.
7. **Reconcile the existing `asset_manifest.json`** with the new authority so there aren't two
   manifests (a Wave 6 lesson: one authority per fact).
8. **Produce a generated gallery**: `docs/visual/ASSET_GALLERY.md` rendered from the manifest with
   coverage annotations (used / unused / missing), so an art contributor can see the gap list without
   reading code — generated, never hand-edited (Wave 3's 29A rule).
9. **Attribute provenance**: `AI_Generated` folders and `scripts/generate_item_icons.py` output need
   a declared origin field per asset (there is already `docs/AI_DISCLOSURE.md` and
   `docs/HUMAN_AUTHORSHIP.md`) so store/legal questions have an answer instead of a folder name.
10. **Verify LFS/import policy** for anything newly mapped (`lfs-health-check.sh`, `ashfall-lfs-gate`):
    114 MB in the wrong place is a clone-time catastrophe.
11. **Tests**: manifest schema and id resolution, resolver precedence (manifest > convention >
    fallback), a strict-tier test proving a fallback fails it, and a duplicate-mapping test.
12. **Run the checklist** + `--asset-registry-selftest` (both modes) + `bash
    scripts/ci/godot-asset-gate.sh`.

**DoD:** every rendered pixel traces to a declared mapping, and coverage is a published number.

---

## Task 50B — Reclaim or remove 1,189 unreferenced files

**Goal:** decide the fate of art nobody references — archive, wire, or delete — so the asset tree
describes the game that exists, and the clone stays cheap.

**Files:** `assets/art/`, `assets/ui/Icons/`, `assets/sprites/AI_Generated/`,
`assets/ui/HtmlBundles/`, `assets/art/*.png` naming variants (`*_hq`, `*_10_of_10`), root
`UI_StyleReference_01.jpg`, `art-wiring-results.xml`, `.gitattributes`,
`docs/archive/assets/`, `scripts/maintenance/`, `ashfall-lfs-gate`, `ashfall-asset-counter`,
`ashfall-repo-hygiene` skills.

### Substeps

1. **Export the orphan list from 50A step 1** and bucket it: (a) wireable to an existing id,
   (b) needed by a family with no art yet, (c) superseded variant, (d) concept art / reference,
   (e) genuinely orphaned.
2. **Wire (a) first** — the cheapest coverage win in the whole project: existing files, missing
   manifest rows, no new art.
3. **Resolve duplicate variants**: `improvised_cooking_stove.png` vs `_hq` vs `sewing_kit_10_of_10`
   — declare which variant is canonical per use (icon vs splash vs state) in the manifest, not by
   filename convention.
4. **Archive (c)/(d) deliberately** — `docs/archive/assets/` plus a note, or a LFS-tracked archive
   branch if size matters; the repo already carries a 140 MB archive-tarball warning in `AGENTS.md`,
   so the size lesson is learned, not theoretical.
5. **Delete (e) with a receipt** — no silent removals: the manifest diff, the size reclaimed, and the
   commit that removed them (and `git ls-files`/`git lfs ls-files` verification that the objects
   leave the working tree, if not history).
6. **Measure the payoff**: clone size, `du` of `assets/`, and the `.godot/` import cache cost —
   reported in the wave ledger (Wave 3's 29C), not asserted.
7. **Check the import settings on survivors**: filter/mipmap/compression presets per family
   (`ashfall-shader-material-lint`, `docs/visual/DIRECT_GODOT_ASSET_LOADS_AUDIT.md`) — the Unity→Godot
   port explicitly required porting import settings, so this closes a documented obligation.
8. **Kill root-tree junk** (re-flagged from Wave 1's hygiene pass): `art-wiring-results.xml`,
   stray `codex_alt_0*.png.import` sidecars at repo root, `fix_*.py` scripts — into
   `scripts/maintenance/` or archived; root files are how an agent learns the wrong structure.
9. **Guard against reintroduction**: a gate fails any *new* asset file with no manifest row — the
   asset twin of 45A, and the reason this task doesn't have to be repeated in a year.
10. **Dedup identical bytes** (hash the tree) before deciding anything is art — duplicates in a
    74 MB folder are likely.
11. **Update `AGENTS.md`'s asset-debt paragraph** with the real state (claim #8: the Unity tree is
    already emptied; 4 dirs contain only `.gdignore`), then regenerate the 12 rulebook copies (29A).
12. **Tests**: no-orphan-file assertion (nightly), manifest-row assertion per added asset, import
    preset conformance.
13. **Run the checklist** + `lfs-health-check.sh` + `asset-orphan-sweep.sh`.

**DoD:** the asset tree contains the game's art, each file has a reason to exist, and the clone got
smaller.

---

## Task 50C — Screen-level visual QA: prove what the player sees

**Goal:** move visual verification from "the load returned a texture" to "this screen looks right",
using the snapshot harness that already exists and the panels Wave 1's 16A left live.

**Files:** `src/UI/SnapshotHarness.cs`, `snapshots/`,
`docs/ui/SNAPSHOT_COVERAGE.md`, `docs/ui/SNAPSHOT_FIXTURE_POLICY.md`,
`scripts/ci/generate-ui-panel-catalog.py`, `docs/ui/SNAPSHOT_MANIFEST_CONSISTENCY_AUDIT_2026-08-26.md`,
`docs/ui/SNAPSHOT_REGEN_APPROVAL_2026-08-26.md`, Wave 1's 15C liveness gate, `ashfall-snapshot-diff`.

### Substeps

1. **Snapshot every live-routed panel** (post-16A set, not 135) with **fixture-populated state** so
   an image proves data binding and asset resolution together, not layout alone.
2. **Add an asset-presence assertion** to each snapshot: no visible fallback/placeholder in a
   COVERED target unless the family is explicitly declared "awaiting art" in the manifest.
3. **Review with real eyes on a schedule**: an approved-baseline process (the repo already has an
   approval document) with a named reviewer per batch — cross-tool QA rule applies where two tools
   are in play.
4. **Test at the scaling envelope**: 1920×1080, 1280×800 (the snapshot size), windowed small, plus
   Wave 5's 37C text-scale variants, since `stretch=canvas_items/keep_height` hides overflow only up
   to a point.
5. **Diff, don't eyeball, regressions**: wire `ashfall-snapshot-diff` into the nightly tier so an art
   change fails loudly on unintended screens.
6. **Perceptual smoke, bounded**: a cheap mean-difference threshold with a human-approve escape
   hatch; avoid inventing a bespoke ML pipeline for a 2D UI game.
7. **Contrast/legibility pass**: measured contrast against the graphite/brass/amber palette, per
   `docs/ui/DESIGN_SYSTEM_RULES.md` + `ashfall-ui-access`, on the snapshot set rather than ad hoc.
8. **Motion check**: after Plan 51 adds transitions, snapshot the mid-animation frame set so a
   half-faded panel can't merge unnoticed.
9. **Coverage report**: which live panels have snapshots, which have populated fixtures, which show
   placeholders — one table in `docs/ui/SNAPSHOT_COVERAGE.md`, generated.
10. **Keep the baselines honest**: regenerate only with a recorded reason; never "fix CI by accepting
    the change" (Wave 3's 29 guardrail).
11. **Tie to release**: a release candidate's gate report (Wave 5's 39A step 10) includes the
    snapshot set hash and the placeholder count.
12. **Tests**: manifest/coverage consistency, no-undeclared-placeholder assertion, diff tooling
    self-proof (a deliberately broken image fails).
13. **Run the checklist** + `bash scripts/ci/verify-fast.sh`.

**DoD:** "it looks right" becomes a gated claim with a number attached.

---

## Cross-Task Dependencies

```
45A (content ladder) ──► 50A (same contract, applied to assets)
26A/47B (resolver, overlays) ──► 50A step 4      15C/16A (live panels) ──► 50C step 1
27A (fixture fidelity) ──► 50C step 1            29A/29B (doc truth) ──► 50B step 11
39A/26B (gates, artifacts) ──► 50A step 2, 50C step 11
        50A (map) ──► 50B (reclaim) ──► 50C (prove)
        Plans 51–52 render on top of 50's mapping — art that can't be found can't be shown.
```

**Execution order:** 45A → 50A → 50B → 50C, then Plans 51/52. Do not commission new art before
50A: 114 MB already exists and 1,189 files of it are provably orphans.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors (+ asset manifest ids)
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- --asset-registry-selftest        # coverage table, strict tier
7. bash scripts/ci/godot-asset-gate.sh                           # full asset+expansion gate chain
8. bash scripts/ci/lfs-health-check.sh && bash scripts/ci/asset-orphan-sweep.sh
9. bash scripts/ci/generate-asset-manifest.py --check
10. ashfall-snapshot-diff over the live-panel set (50C)
11. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Code | Data/Assets | Tooling | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|
| 50A | 2 | 1 new catalog | 1 generator | 8–12 | Medium | LOW (additive, convention fallback retained) |
| 50B | 0 | **1,189+ candidates** | 1 gate | 4–6 | Medium (judgement-heavy) | LOW–MED (never delete what an unknown consumer loads — step 9 protects) |
| 50C | 0 | 0 | snapshot wiring | 6–10 + images | Low–Med | LOW (CI-side) |

**Guardrails:** no new commissioned art in this plan (it measures and maps what exists); never let a
fallback count as a pass; no bulk deletion without a manifest diff and a receipt; no import-setting
change without re-verifying the snapshots; no LFS policy drift on a 114 MB tree; and no hand-edited
generated gallery.
