# C2 — Flagship Integration Plan [22]: Asset Truth, Explicit Mapping, Orphan Reclamation, and Screen-Level Visual Proof

> **Deliverable:** `C2_planintegration[22].md`
> **Source scope:** Plan 50 — *Asset Truth: What Actually Renders*
> **Wave:** Continuity Wave 8 — *The Presented Game*
> **Primary objective:** replace convention-based asset guessing with one explicit id→asset authority, publish real coverage by family, separate fallback from success, reclaim or wire the orphaned asset tree, and prove live player-facing screens render intended assets through populated snapshot coverage.
> **Required execution order:** **45A → 50A → 50B → 50C**
> **Hard gate:** do not commission new art until 50A has mapped what already exists and 50B has classified the orphan pool.
> **Dependencies:** 45A content acceptance ladder, 26A resolver, 47B pack/overlay loading, 39A gate tiering/release reporting, 15C/16A live-panel set, 27A fixture fidelity, 29A/29B generated-doc truth discipline.
> **Scope discipline:** no new commissioned art, no fallback counted as strict success, no bulk deletion without manifest evidence and a receipt, no import-setting drift without snapshot re-verification, no LFS-policy drift, and no hand-edited generated gallery.

---

# 0. Executive Intent

ASHFALL already carries a large presented-game asset corpus:

- art,
- UI textures,
- icons,
- screens,
- portraits,
- sprites,
- audio,
- AI-generated outputs,
- imported assets,
- placeholder assets,
- pipeline scripts.

The continuity defect is not “the game has no assets.”

It is:

```text
the game cannot prove which authored IDs resolve to which real files
```

and therefore cannot reliably answer:

```text
Which asset rendered?
Was it a real asset or a fallback?
How much of the authored game has real visual coverage?
Which files are actually unused?
Which portrait belongs to which survivor?
Which assets were AI-generated?
Which variant is canonical?
Which import settings apply?
Which screens show placeholders right now?
```

Current name-based resolution can accidentally work while remaining unverifiable.
A fallback can be treated as success.
The tree contains a large orphan population.
The snapshot system proves layout more readily than it proves asset correctness.

This plan converts visual asset handling into a first-class data/CI contract.

The intended architecture is:

```text
authored game id
      │
      ▼
asset_registry.json
      │
      ├─ kind
      ├─ id
      ├─ path
      ├─ provenance
      ├─ import preset
      └─ scratch/final state
      │
      ▼
AssetManifest loader
      │
      ▼
AssetRegistry
      │
      ├─ manifest lookup
      ├─ legacy convention fallback during migration
      └─ family-specific placeholder
      │
      ▼
coverage scanner
      │
      ├─ real asset
      ├─ fallback
      ├─ unresolved id
      └─ orphan file
      │
      ▼
generated gallery + CI gates
      │
      ▼
populated live-panel snapshots
```

The flagship outcome is:

> **Every rendered asset can be traced to one declared mapping, every fallback is counted honestly, every orphan file has a disposition, and every live screen can be validated against fixture-populated visual baselines.**

---

# 1. Source Diagnosis

The source establishes:

- the asset corpus is much larger than the current gate coverage,
- current gate coverage is only a tiny fraction of authored IDs,
- fallback textures count as valid,
- two canonical placeholders absorb many missing-resolution cases,
- portrait naming depends on guessed stem permutations,
- most art/icon files are not name-referenced anywhere,
- no canonical id→asset mapping exists,
- one older `asset_manifest.json` exists but is not the resolver authority,
- asset generation/import scripts are disconnected from a documented contract,
- LFS policy is already established and must remain intact,
- the current docs contain stale asset-debt claims.

The architectural reading is:

```text
asset resolution is currently heuristic
asset truth must become declarative
```

---

# 2. Program-Level Success Criteria

C2[22] closes only when:

1. One canonical asset manifest exists.
2. Every mapped asset row references a valid authored ID.
3. Resolver checks the manifest before convention fallback.
4. Strict gate distinguishes real load from fallback.
5. Coverage is reported per asset family.
6. Portrait mapping no longer depends on display-name luck.
7. Existing manifest authority is reconciled/retired.
8. Provenance/origin is recorded per asset where required.
9. New mapped assets respect LFS/import policy.
10. The orphan list is exported from the same manifest/coverage scanner.
11. Every orphan is classified.
12. Wireable assets are wired before any art is commissioned.
13. Duplicate/superseded variants have explicit canonical status.
14. Deleted assets have a receipt.
15. New unmanifested assets fail a gate.
16. Root-tree asset junk is removed/re-homed.
17. Every live player-routed panel has a populated snapshot.
18. Covered screenshots fail on undeclared visible fallback.
19. Snapshot variants cover the supported scaling envelope.
20. Release report includes snapshot-set hash and placeholder count.

---

# 3. Architectural Invariants

## 3.1 One mapping authority

`asset_registry.json` or equivalent is the canonical ID→asset mapping.

Do not keep two live manifests.

## 3.2 Manifest lookup precedes convention guessing

Migration-friendly resolution order:

```text
manifest
→ legacy convention
→ family fallback
```

Later waves may remove convention fallback once coverage is complete.

## 3.3 Fallback is not success

Fast tier may warn during migration.
Strict/nightly/release tier fails.

## 3.4 Placeholder state is explicit

Scratch placeholders are categorized per family.

They are not visually indistinguishable from production art.

## 3.5 Asset identity uses authored IDs

Display names are presentation.
File stems are storage.
The authored ID is the stable identity.

## 3.6 Provenance is metadata

AI-generated/human-authored/imported status belongs in manifest/provenance data.

## 3.7 LFS policy remains authoritative

Do not move/bypass large-image tracking policy casually.

## 3.8 Orphan deletion follows mapping evidence

No mass delete from substring grep alone.

## 3.9 Snapshots prove both binding and rendering

Fixtures must populate real data.

## 3.10 Generated docs are not hand-edited

Gallery/coverage tables regenerate from the manifest.

---

# 4. Dependency Graph

```text
45A content acceptance ladder
       │
       ▼
50A asset map + strict coverage
       │
       ▼
50B orphan reclamation
       │
       ▼
50C screen-level visual proof

26A/47B ─────────► resolver/pack loading
15C/16A ─────────► live panel set
27A ─────────────► realistic fixtures
29A/29B ─────────► generated docs truth
39A/26B ─────────► release/report/artifact gates
51/52 ───────────► render on top of 50's asset truth
```

Required order:

```text
45A → 50A → 50B → 50C
```

---

# 5. Baseline Capture

Before behavior changes, generate an honest baseline.

## 5.1 Asset-family inventory

At minimum:

```text
items
icons
portraits
characters
locations
weather
terminals
screens
ui_textures
audio
```

For each record:

```text
authored IDs
real file resolution
fallback resolution
unresolved IDs
files on disk
orphan files
```

## 5.2 Current resolver behavior

Document:

- search path order,
- stem candidate generation,
- placeholder paths,
- fallback semantics,
- case sensitivity assumptions,
- pack/overlay behavior.

## 5.3 Current import/LFS state

Record:

- `.gitattributes`,
- LFS tracked patterns,
- image/audio handling,
- import presets,
- `.godot/imported` impact.

## 5.4 Snapshot baseline

Record:

- live player-routed panels,
- panels with snapshots,
- panels with fixture-populated snapshots,
- visible placeholders,
- snapshot sizes/scales.

## 5.5 Baseline commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --asset-registry-selftest
bash scripts/ci/godot-asset-gate.sh
bash scripts/ci/lfs-health-check.sh
bash scripts/ci/verify-fast.sh
```

Capture exact current output, including fallback counts.

---

# 6. Workstream 50A — Measure and Map

## Goal

Replace name guessing with declared mapping and make “green asset gate” mean real coverage.

---

# 7. 50A Phase A — Publish the Four-Column Baseline

For each asset family publish:

| Family | Authored IDs | Real asset | Fallback | On-disk orphan |
|---|---:|---:|---:|---:|

Also track unresolved IDs separately if useful.

This table is generated from actual scans.

---

# 8. 50A Phase B — Fix Success Semantics First

Refactor `AssetResolution` semantics.

Conceptual:

```text
LoadedReal
FallbackUsed
Missing
LoadFailed
```

`IsValid` should not collapse:

```text
LoadedReal
FallbackUsed
```

into one strict result.

---

# 9. 50A Phase C — Gate Tiers

## Fast tier

- real loads pass,
- fallback warns/reports,
- missing/load failure fails.

## Strict/nightly tier

- fallback fails unless explicitly marked scratch/awaiting-art by manifest policy,
- missing/load failure fails.

## Release

- same as strict,
- placeholder count included in report.

---

# 10. 50A Phase D — Define Asset Manifest Schema

Create:

```text
Assets/StreamingAssets/Data/asset_registry.json
```

Recommended row:

```json
{
  "kind": "portrait",
  "id": "survivor_elena_vasquez",
  "path": "res://assets/sprites/Portraits/elena_vasquez.png",
  "source": "human_authored",
  "license": "project_owned",
  "ai_generated": false,
  "import_preset": "portrait_ui",
  "status": "final"
}
```

Schema should be compact and stable.

---

# 11. 50A Phase E — Core Manifest Types

Create engine-free Core types:

```text
AssetManifest
AssetManifestEntry
AssetKind
AssetSourceKind
AssetStatus
```

No Godot `Texture2D` in Core.

---

# 12. 50A Phase F — Manifest Loader

Create:

```text
Assets/Ashfall.Core/Assets/AssetManifestLoader.cs
```

Responsibilities:

- parse,
- validate schema,
- normalize paths,
- validate duplicate keys,
- expose diagnostics.

---

# 13. 50A Phase G — ID Validation

Each manifest ID resolves against the authoritative catalog family.

Examples:

- survivor portrait → survivor ID,
- item icon → item ID,
- location art → location ID,
- weather art → weather kind/ID.

Unknown invented ID fails CI.

---

# 14. 50A Phase H — Asset Kind Vocabulary

Keep the family vocabulary deliberate.

Possible:

```text
item_icon
survivor_portrait
character_sprite
location_art
weather_art
terminal_art
screen
ui_texture
audio_cue
```

Avoid ambiguous generic `image`.

---

# 15. 50A Phase I — Path Validation

Validate:

- path exists,
- expected extension,
- expected family root,
- no path traversal,
- case-correct path on Linux,
- pack/overlay compatibility.

---

# 16. 50A Phase J — Resolver Precedence

Update `AssetRegistry`:

```text
1. manifest lookup
2. legacy convention fallback
3. family fallback
```

Emit structured result with resolution source.

---

# 17. 50A Phase K — Resolution Provenance

For each lookup record in diagnostics:

```text
id
kind
result
path
resolution_source
```

Resolution sources:

```text
manifest
legacy_convention
family_fallback
```

This enables migration progress.

---

# 18. 50A Phase L — Portrait Mapping

For portraits choose one policy:

### Policy 1 — rename files to authored ID stems

or

### Policy 2 — keep display-name filenames but map explicitly.

Do not retain both as equally implicit authority.

Recommendation for minimal churn:

```text
explicit manifest mapping
```

with a future optional rename cleanup.

---

# 19. 50A Phase M — Existing Manifest Reconciliation

Inspect:

```text
assets/sprites/asset_manifest.json
```

For every field:

```text
merge into canonical manifest
or
retire as generated/non-authoritative artifact
```

Do not keep duplicate live truth.

---

# 20. 50A Phase N — Family-Specific Placeholders

Replace single broad placeholders with obvious scratch placeholders:

```text
portrait_missing
item_icon_missing
location_art_missing
ui_texture_missing
```

Distinct visuals/names.

Count each family separately.

---

# 21. 50A Phase O — Awaiting-Art Status

If an ID intentionally has no final art yet, manifest may declare:

```text
status = awaiting_art
```

This does not equal strict success unless release policy explicitly allows that family.

Do not hide it.

---

# 22. 50A Phase P — Provenance Fields

Record as needed:

```text
source
license
ai_generated
generator/tool
human_revision
source_prompt_ref? (only if repository policy wants it)
```

Keep privacy/secret material out.

---

# 23. 50A Phase Q — AI Disclosure Alignment

Generate/store-ready summary from manifest:

- AI-generated asset count,
- human-authored asset count,
- mixed/revised count.

Coordinate:

```text
docs/AI_DISCLOSURE.md
docs/HUMAN_AUTHORSHIP.md
```

No folder-name assumptions.

---

# 24. 50A Phase R — Import Presets

Define stable import-preset IDs.

Examples:

```text
portrait_ui
pixel_icon
background_art
screen_ui
audio_sfx
```

Each maps to expected Godot import settings.

---

# 25. 50A Phase S — Import Preset Validation

Scan `.import`/Godot metadata as appropriate.

Flag:

- wrong filtering,
- wrong mipmaps,
- unexpected compression,
- wrong color-space assumptions.

Do not rewrite all imports blindly.

---

# 26. 50A Phase T — Manifest Generator

Create:

```text
scripts/ci/generate-asset-manifest.py
```

Potential responsibilities:

- validate manifest,
- compute coverage,
- compute orphans,
- generate gallery metadata,
- `--check` mode.

Avoid auto-inventing mappings from stems as authoritative output.

It may propose candidates separately.

---

# 27. 50A Phase U — Candidate Mapping Report

Generator may output:

```text
artifacts/asset-mapping-candidates.json
```

with:

- probable ID,
- probable file,
- confidence,
- reason.

Human reviews before committing manifest rows.

---

# 28. 50A Phase V — Generated Asset Gallery

Create:

```text
docs/visual/ASSET_GALLERY.md
```

Generated rows/cards include:

```text
kind
id
path
status
coverage
provenance
```

If Markdown image embedding is practical, use relative preview links.
Otherwise include structured path/status tables.

---

# 29. 50A Phase W — Fallback Visual Assets Doc

Create:

```text
docs/visual/FALLBACK_VISUAL_ASSETS.md
```

Document:

- family placeholders,
- intended use,
- whether allowed in release,
- how strict gate treats them.

---

# 30. 50A Phase X — LFS Gate Integration

Any newly mapped file must pass:

```text
lfs-health-check.sh
ashfall-lfs-gate
```

Mapping a file must not silently move its storage policy.

---

# 31. 50A Phase Y — Manifest Tests

Tests:

- schema,
- duplicate `(kind,id)`,
- unknown ID,
- missing path,
- invalid family root,
- precedence manifest > legacy > fallback,
- strict fallback failure,
- provenance enum validation,
- import preset validation,
- Linux path case correctness.

---

# 32. 50A Phase Z — Coverage Report Contract

Selftest output:

```text
ASSET_IDS_TOTAL
ASSET_REAL_RESOLVED
ASSET_LEGACY_RESOLVED
ASSET_FALLBACK
ASSET_MISSING
ASSET_ORPHAN_FILES
```

Per-family lines too.

---

# 33. 50A Definition of Done

- [ ] honest family baseline,
- [ ] fallback separated from success,
- [ ] strict tier,
- [ ] canonical asset registry,
- [ ] engine-free manifest types,
- [ ] loader,
- [ ] ID/path validation,
- [ ] resolver precedence,
- [ ] resolution-source diagnostics,
- [ ] portrait policy,
- [ ] old manifest reconciled,
- [ ] family placeholders,
- [ ] awaiting-art state,
- [ ] provenance,
- [ ] AI disclosure projection,
- [ ] import presets,
- [ ] manifest generator,
- [ ] mapping-candidate report,
- [ ] generated gallery,
- [ ] fallback doc,
- [ ] LFS checks,
- [ ] strict fallback test.

---

# 34. Workstream 50B — Reclaim or Remove the Orphan Tree

## Goal

Every asset file receives an explicit reason to exist.

---

# 35. 50B Phase A — Export Canonical Orphan List

Use 50A scanner.

Do not rely on ad-hoc substring grep as deletion authority.

Output:

```text
artifacts/asset-orphans.json
```

with:

- path,
- hash,
- size,
- family,
- stem,
- possible mapping candidates.

---

# 36. 50B Phase B — Five-Bucket Classification

Each orphan:

```text
A — wireable to existing ID
B — useful for family with missing art
C — superseded variant
D — concept/reference/archive
E — genuine orphan
```

Every row gets disposition.

---

# 37. 50B Phase C — Wire Bucket A First

These are the highest-value fixes.

For each:

- verify intended ID,
- add manifest row,
- run strict asset check,
- snapshot any affected live panel.

No new art required.

---

# 38. 50B Phase D — Bucket B Holding Policy

Assets useful but not yet wired to a real gameplay ID must not masquerade as active coverage.

Options:

- keep in clearly named staging/concept area,
- map once a canonical ID exists,
- archive.

Do not invent gameplay IDs just to keep art.

---

# 39. 50B Phase E — Variant Families

For filename variants:

```text
base
_hq
_10_of_10
_alt
_final
_final2
```

determine:

- intended use,
- canonical asset,
- alternate state,
- obsolete duplicate.

Manifest stores purpose explicitly.

---

# 40. 50B Phase F — Exact Duplicate Hashing

Hash asset tree.

For byte-identical duplicates:

- map one canonical file,
- remove duplicate working-tree copies,
- preserve history naturally via Git/LFS.

Report reclaimed bytes.

---

# 41. 50B Phase G — Near-Duplicate Review

Optional perceptual/manual review for visually duplicated variants.

Do not auto-delete based only on image similarity.

---

# 42. 50B Phase H — Archive Concept/Reference Art

Move appropriate D bucket to:

```text
docs/archive/assets/
```

or a size-conscious external/branch strategy if necessary.

Include manifest/index note.

Do not keep concept art in runtime asset roots.

---

# 43. 50B Phase I — Delete Genuine Orphans With Receipt

For E bucket generate:

```text
docs/archive/assets/REMOVAL_RECEIPT_<date>.md
```

or JSON/Markdown combined.

Receipt contains:

- paths,
- hashes,
- total bytes,
- reason,
- commit SHA after merge.

---

# 44. 50B Phase J — Working-Tree/LFS Verification

After deletions:

```text
git ls-files
git lfs ls-files
```

Confirm removed files no longer appear in current tree.

Do not claim Git history size shrank unless history rewrite actually occurred.

---

# 45. 50B Phase K — Clone/Import Cost Measurement

Measure before/after:

```text
git checkout/clone working size
assets/ size
LFS pull size
.godot import cache size
import duration if measurable
```

Report facts, not estimates.

---

# 46. 50B Phase L — Import Setting Review

For surviving mapped assets:

- filter,
- mipmap,
- compression,
- import mode.

Use declared family preset from 50A.

Fix mismatches with snapshot re-verification.

---

# 47. 50B Phase M — Root Hygiene

Re-home:

- `art-wiring-results.xml`,
- stray import sidecars,
- `fix_*.py`,
- style references,
- pipeline outputs.

Destinations:

```text
scripts/maintenance/
docs/archive/assets/
artifacts/
```

according to type.

---

# 48. 50B Phase N — Asset Pipeline Contract

Document how scripts:

```text
generate_item_icons.py
generate_assets.py
import_approved_assets.py
```

feed:

```text
generated candidate
→ review/approval
→ canonical manifest row
→ runtime asset
```

No script output becomes “live” merely by existing on disk.

---

# 49. 50B Phase O — New-Asset Gate

Fail any newly added runtime asset without:

- manifest row,
- provenance,
- import preset,
- valid ID mapping where applicable.

Concept/reference directories can have separate policy.

---

# 50. 50B Phase P — Orphan Ratchet

During migration, record baseline orphan count.

Allowed:

```text
count decreases
```

New runtime orphan:

```text
fail
```

This makes cleanup durable.

---

# 51. 50B Phase Q — Update Stale Asset Docs

Correct AGENTS claim that Unity-era asset migration remains pending.

Regenerate all rulebook copies.

Add current facts:

- runtime asset roots,
- manifest authority,
- new-asset rule,
- fallback rule.

---

# 52. 50B Phase R — Orphan Sweep Script

Create:

```bash
scripts/ci/asset-orphan-sweep.sh
```

or equivalent.

Output:

- new orphan count,
- total orphan count,
- orphan bytes,
- missing manifest rows.

Nightly strict mode can enforce stronger baseline.

---

# 53. 50B Tests

- new asset without manifest fails,
- deleted manifest row leaves orphan and fails,
- duplicate hash report deterministic,
- import preset conformance,
- root hygiene denylist,
- orphan baseline ratchet,
- archived concept paths excluded from runtime package.

---

# 54. 50B Definition of Done

- [ ] canonical orphan export,
- [ ] all orphans classified,
- [ ] wireable files wired,
- [ ] bucket B policy,
- [ ] variants resolved,
- [ ] identical bytes deduped,
- [ ] concept/reference art archived,
- [ ] genuine orphans deleted with receipt,
- [ ] LFS/current-tree verification,
- [ ] clone/assets/import payoff measured,
- [ ] import settings reviewed,
- [ ] root junk re-homed,
- [ ] pipeline contract documented,
- [ ] new-asset manifest gate,
- [ ] orphan ratchet,
- [ ] AGENTS asset docs corrected,
- [ ] rulebooks regenerated,
- [ ] asset-orphan-sweep gate active.

---

# 55. Workstream 50C — Screen-Level Visual QA

## Goal

Prove what the player sees, not merely what the resolver can load.

---

# 56. 50C Phase A — Canonical Live Panel Set

Generate post-16A live-routed panel list.

Do not snapshot shelved/prototype surfaces as release coverage.

Source:

- PanelRegistry,
- liveness/maturity metadata,
- generated panel catalog.

---

# 57. 50C Phase B — Snapshot Coverage Matrix

Generate:

| Panel | Live | Snapshot | Populated fixture | Placeholder-free | Scale variants | Approved |
|---|---:|---:|---:|---:|---:|---:|

This is release coverage truth.

---

# 58. 50C Phase C — Fixture-Populated State

Every covered panel uses realistic populated fixture.

Avoid:

```text
empty shell snapshot
```

where asset/data binding is not exercised.

Examples:

- inventory with real items/icons,
- survivors with real portraits,
- map with locations,
- medical with treatments,
- expedition with party/route.

---

# 59. 50C Phase D — Asset-Presence Assertion

Snapshot fixture should expose asset resolution metadata.

For a covered target:

```text
visible asset
→ real mapped asset
```

unless manifest explicitly declares awaiting-art allowance.

No silent placeholder.

---

# 60. 50C Phase E — Placeholder Count

Per snapshot:

```text
visible_placeholders
visible_fallbacks
```

Release target:

```text
0
```

for covered/final panels unless approved exceptions exist.

---

# 61. 50C Phase F — Snapshot Scales

At minimum:

```text
1920×1080
1280×800
small windowed
text scale min/standard/max
```

Use Plan 37C scale contract.

---

# 62. 50C Phase G — Focus/Keyboard State

For selected snapshots include focus state.

This validates:

- focus visibility,
- layout with highlighted controls,
- keyboard legibility.

---

# 63. 50C Phase H — Contrast/Legibility

Use `ashfall-ui-access` or equivalent.

Measure:

- text/background contrast,
- warning contrast,
- disabled/selected/focused states.

Evaluate actual screenshot colors, not just theme tokens.

---

# 64. 50C Phase I — Nightly Snapshot Diff

Wire:

```text
ashfall-snapshot-diff
```

into nightly tier.

Do not necessarily run full visual suite on every push if expensive.

---

# 65. 50C Phase J — Diff Threshold

Use simple bounded perceptual/mean-difference threshold.

Avoid heavy ML vision infrastructure.

Need:

```text
machine threshold
+ human approval for intended change
```

---

# 66. 50C Phase K — Deliberate Diff Failure Proof

Modify/break a fixture image in a test scenario.

Assert snapshot diff fails.

Do not trust an unproven visual gate.

---

# 67. 50C Phase L — Baseline Approval Workflow

Require:

- named reviewer,
- reason,
- related plan/issue,
- timestamp/date,
- affected snapshots.

Do not “accept all” to clear CI.

---

# 68. 50C Phase M — Snapshot Manifest Consistency

Generated snapshot manifest must agree with:

- live panel catalog,
- fixture catalog,
- files on disk.

Fail stale/dead baselines.

---

# 69. 50C Phase N — Motion/Transition Frames

Once Plan 51 adds animation:

- capture mid-transition states,
- ensure alpha/fade states remain readable.

Do not block Plan 50 completion on future motion work; establish extension seam.

---

# 70. 50C Phase O — Release Report Integration

Release report includes:

```text
snapshot_set_hash
live_panels
snapshot_covered
fixture_populated
visible_placeholder_count
unapproved_diff_count
```

---

# 71. 50C Phase P — Asset-to-Screen Traceability

For sampled/critical panels, diagnostics should answer:

```text
panel
control
asset id
manifest path
resolution result
```

Useful for visual regressions.

---

# 72. 50C Phase Q — High-Risk Panel Priority

Prioritize:

- dashboard,
- inventory,
- survivors,
- expedition,
- map,
- medical,
- crafting,
- trade,
- briefing,
- settings.

Then complete all live routes.

---

# 73. 50C Phase R — Audio Presence on Screens

Where UI relies on audio cues:

- audio asset resolves through same manifest/registry concept,
- text/visual path remains complete for accessibility.

Do not use snapshot tool to validate audio acoustics; validate presence IDs separately.

---

# 74. 50C Phase S — Snapshot Coverage Doc

Generate:

```text
docs/ui/SNAPSHOT_COVERAGE.md
```

Never hand-maintain the coverage counts.

---

# 75. 50C Tests

- live-panel→snapshot manifest consistency,
- populated fixture requirement,
- no undeclared visible placeholder,
- supported scale coverage,
- diff failure proof,
- snapshot-set hash stability,
- stale snapshot detection,
- asset trace metadata.

---

# 76. 50C Definition of Done

- [ ] canonical live-panel list,
- [ ] generated coverage matrix,
- [ ] populated fixtures,
- [ ] visible asset-presence assertions,
- [ ] placeholder count,
- [ ] scale variants,
- [ ] focus/keyboard state coverage,
- [ ] contrast/accessibility pass,
- [ ] nightly snapshot diff,
- [ ] bounded threshold,
- [ ] deliberate failure proof,
- [ ] approval workflow,
- [ ] manifest consistency,
- [ ] motion extension seam,
- [ ] release report metrics,
- [ ] asset-to-screen traceability,
- [ ] high-risk panels covered,
- [ ] generated SNAPSHOT_COVERAGE.md.

---

# 77. Integrated Asset Truth Pipeline

```text
authored ID
   │
   ▼
asset manifest
   │
   ├─ path
   ├─ source/provenance
   ├─ status
   └─ import preset
   │
   ▼
resolver
   │
   ├─ manifest
   ├─ legacy convention
   └─ family fallback
   │
   ▼
resolution result
   │
   ├─ real
   ├─ fallback
   ├─ missing
   └─ failed
   │
   ▼
coverage/orphan report
   │
   ├─ wire
   ├─ archive
   └─ delete
   │
   ▼
live screen snapshot
   │
   ▼
release visual evidence
```

---

# 78. Asset Identity Contract

Stable key:

```text
(kind, authored_id)
```

File path is mutable storage.
Display name is presentation.

Do not use filename stem as identity authority.

---

# 79. Manifest Authority Contract

Every runtime asset lookup should be explainable by one manifest row or an explicitly reported migration fallback.

---

# 80. Resolver Result Contract

Resolution result must distinguish:

```text
ManifestLoaded
LegacyConventionLoaded
FallbackUsed
Missing
LoadFailed
```

Strict coverage considers only real mapped/convention-loaded assets as non-fallback success during migration, with a future target of manifest-only coverage.

---

# 81. Placeholder Contract

Every placeholder declares:

```text
family
status
allowed tiers
visible scratch styling
```

A placeholder is not production art.

---

# 82. Provenance Contract

Every release-relevant asset should have sufficient provenance to answer:

```text
human?
AI-assisted?
AI-generated?
third-party?
license?
source/import path?
```

Do not use directory name as the only provenance source.

---

# 83. Import Contract

Every asset family maps to one expected import preset.

Changing preset requires:

```text
manifest/import diff
→ snapshot verification
```

---

# 84. LFS Contract

Asset mapping/cleanup must preserve repository LFS rules.

New runtime image that violates expected LFS handling fails.

---

# 85. Orphan Contract

Orphan means:

```text
runtime asset file
with no active manifest reference
and no declared archive/concept role
```

Not merely “filename never appears in source.”

---

# 86. Deletion Contract

Delete only after:

- orphan status confirmed,
- candidate mapping reviewed,
- duplicate role reviewed,
- archive status reviewed,
- receipt generated.

---

# 87. Snapshot Contract

A release-covered panel snapshot proves:

- live route,
- realistic fixture,
- real asset binding,
- no undeclared placeholder,
- supported scale,
- approved visual baseline.

---

# 88. Generated Documentation Contract

Generated:

```text
ASSET_GALLERY.md
SNAPSHOT_COVERAGE.md
coverage/orphan reports
```

must be reproducible with `--check`.

---

# 89. Release Contract

Release report includes:

```text
ASSET_REAL_RESOLVED
ASSET_FALLBACK
ASSET_MISSING
ASSET_ORPHAN_FILES
SNAPSHOT_SET_HASH
LIVE_PANEL_SNAPSHOT_COVERAGE
VISIBLE_PLACEHOLDER_COUNT
```

---

# 90. Migration Strategy

Phase 1:

```text
manifest + legacy convention + fallback
```

Phase 2:

```text
manifest + fallback
```

Phase 3 target:

```text
manifest-only for final runtime assets
```

Do not remove convention fallback until manifest coverage proves safety.

---

# 91. Failure Modes

## Asset gate says green because fallback counted as valid

Fix result semantics and strict tier.

## Portrait works only by display-name guess

Add explicit manifest mapping.

## Two manifests both claim authority

Reconcile/retire one.

## Orphan list deletes valid dynamically loaded asset

Use manifest/runtime evidence before deletion.

## New art lands with no manifest row

Gate fails.

## Import settings change and screens blur

Snapshot/import preset gate catches it.

## AI-generated asset has no provenance

Manifest validation fails required provenance.

## Snapshot passes with empty fixture

Fixture coverage gate fails.

## Snapshot baseline updated just to clear CI

Approval record required.

## LFS policy drifts

LFS gate fails.

---

# 92. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| manifest migration misses dynamic asset consumer | Medium | High | legacy fallback + runtime trace |
| orphan deletion removes hidden dependency | Medium | High | candidate/runtime evidence |
| strict gate too red initially | High | Medium | fast warn + nightly strict |
| provenance work becomes over-broad | Medium | Medium | only release-relevant metadata |
| gallery grows huge | Medium | Low | generated summary/index |
| LFS churn | Medium | High | no storage-policy rewrite |
| import preset fixes alter visuals | Medium | Medium | snapshot re-check |
| snapshot suite too slow | Medium | Medium | nightly tier |
| placeholder exemptions become permanent | Medium | High | explicit status/ratchet |
| duplicate variant choice subjective | High | Low–Med | manifest receipt + reviewer |

---

# 93. Commit Strategy

## C2[22].1 — baseline asset coverage + fallback semantics

## C2[22].2 — manifest schema/Core loader

## C2[22].3 — resolver manifest precedence

## C2[22].4 — portrait mapping + old manifest reconciliation

## C2[22].5 — family placeholders + status/provenance

## C2[22].6 — import presets + LFS validation

## C2[22].7 — generator/gallery/coverage reports

## C2[22].8 — strict gate + failure proofs

### Gate: 50A complete

## C2[22].9 — orphan export + classification

## C2[22].10 — wire bucket A

## C2[22].11 — variants + duplicate-byte cleanup

## C2[22].12 — archive concept/reference assets

## C2[22].13 — delete genuine orphans + receipts

## C2[22].14 — import/root hygiene cleanup

## C2[22].15 — pipeline contract + new-asset gate

## C2[22].16 — orphan ratchet + docs/rulebook refresh

### Gate: 50B complete

## C2[22].17 — live-panel snapshot catalog

## C2[22].18 — populated fixture coverage

## C2[22].19 — placeholder/fallback snapshot assertions

## C2[22].20 — scale/accessibility variants

## C2[22].21 — nightly diff + approval workflow

## C2[22].22 — release-report integration

### Gate: 50C complete

## C2[22].23 — Wave‑8 asset-truth closure

---

# 94. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --asset-registry-selftest
bash scripts/ci/godot-asset-gate.sh
bash scripts/ci/lfs-health-check.sh
bash scripts/ci/asset-orphan-sweep.sh
python3 scripts/ci/generate-asset-manifest.py --check
bash scripts/ci/verify-fast.sh
```

Also run repository-canonical equivalents of:

```text
ashfall-lfs-gate
ashfall-snapshot-diff over live-panel set
ashfall-ui-access
asset duplicate hash scan
snapshot deliberate-diff regression
```

---

# 95. Flagship Definition of Done

## 50A — Mapping

- [ ] family coverage baseline,
- [ ] fallback separate from success,
- [ ] strict tier,
- [ ] one asset manifest,
- [ ] ID validation,
- [ ] path validation,
- [ ] resolver manifest-first,
- [ ] resolution-source diagnostics,
- [ ] portrait mapping explicit,
- [ ] old manifest reconciled,
- [ ] family placeholders,
- [ ] awaiting-art status,
- [ ] provenance,
- [ ] import presets,
- [ ] LFS checks,
- [ ] generated gallery,
- [ ] strict fallback failure proof.

## 50B — Reclamation

- [ ] orphan list from canonical scanner,
- [ ] every orphan classified,
- [ ] wireable files wired,
- [ ] variant canon declared,
- [ ] duplicates reviewed,
- [ ] concept/reference art archived,
- [ ] genuine orphans removed with receipt,
- [ ] size payoff measured,
- [ ] import settings conformed,
- [ ] root junk cleaned,
- [ ] pipeline contract,
- [ ] new runtime asset requires manifest,
- [ ] orphan baseline ratchets downward,
- [ ] stale docs corrected.

## 50C — Visual QA

- [ ] every live panel cataloged,
- [ ] every live panel snapshot-covered,
- [ ] populated fixtures,
- [ ] no undeclared visible placeholder,
- [ ] supported scale variants,
- [ ] focus state visible,
- [ ] contrast/accessibility pass,
- [ ] nightly snapshot diff,
- [ ] deliberate diff failure proof,
- [ ] approval workflow,
- [ ] stale baselines detected,
- [ ] release report includes hash/counts.

## Global

- [ ] no new commissioned art,
- [ ] no fallback counted as strict pass,
- [ ] no unreceipted bulk deletion,
- [ ] no second manifest authority,
- [ ] no LFS drift,
- [ ] no hand-edited generated visual docs,
- [ ] full verification green.

---

# 96. Closure Report Template

```markdown
## C2[22] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Authored asset IDs:
- Real resolved:
- Fallback:
- Missing:
- On-disk runtime files:
- Orphans:
- Runtime asset bytes:
- Snapshot-covered live panels:

### 50A — Manifest
- Manifest schema:
- Mapped rows:
- Portrait mappings:
- Legacy convention resolutions:
- Fallback resolutions:
- Strict failures:
- Provenance coverage:
- Import preset failures:
- LFS failures:
- Gallery:
- Result:

### 50B — Orphans
- Orphans before:
- Wireable:
- Wired:
- Variant/superseded:
- Archived:
- Deleted:
- Duplicate bytes removed:
- Bytes reclaimed:
- Root junk removed:
- Orphans after:
- New-orphan gate:
- Result:

### 50C — Visual QA
- Live panels:
- Snapshots:
- Populated fixtures:
- Placeholder-free:
- 1920x1080:
- 1280x800:
- Small window:
- Text-scale variants:
- Accessibility failures:
- Snapshot diff failures:
- Approved baseline changes:
- Snapshot set hash:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Bridge:
- Asset registry:
- Godot asset gate:
- LFS:
- Orphan sweep:
- Manifest check:
- Snapshot diff:
- Verify fast:

### Final Metrics
- ASSET_IDS_TOTAL:
- ASSET_REAL_RESOLVED:
- ASSET_LEGACY_RESOLVED:
- ASSET_FALLBACK:
- ASSET_MISSING:
- ASSET_ORPHAN_FILES:
- ASSET_ORPHAN_BYTES:
- LIVE_PANELS:
- LIVE_PANELS_SNAPSHOT_COVERED:
- VISIBLE_PLACEHOLDERS:
- SNAPSHOT_SET_HASH:

### Remaining Debt
- Mapping:
- Orphans:
- Import settings:
- Provenance:
- Snapshots:
- Accessibility:
```

---

# 97. Final Execution Directive

Execute Plan 50 as an **asset-truth and visual-evidence** repair.

The critical sequence is:

```text
measure the current asset reality
→ separate fallback from success
→ introduce one explicit ID→asset manifest
→ migrate resolver to manifest-first
→ publish family coverage
→ classify every orphan
→ wire existing usable art before commissioning anything
→ archive/delete the rest with receipts
→ snapshot every live player screen with populated fixtures
→ fail on undeclared placeholders and unintended visual diffs
```

Do not commission new art before proving existing coverage.

Do not let a placeholder count as a strict pass.

Do not delete a large orphan pool based on filename grep alone.

Do not preserve two manifest authorities.

Do not treat a texture load as proof that the screen is correct.

The strongest asset rule is:

> **Every rendered asset traces to one declared mapping keyed by the game’s authored ID, not by filename luck.**

The strongest cleanup rule is:

> **Every runtime asset file must have a reason to exist: wired, staged with intent, archived, or deleted with a receipt.**

The strongest visual rule is:

> **A release-covered screen must prove real data binding, real asset resolution, supported scaling, and zero undeclared visible fallback.**

The flagship acceptance scenario is:

> **Choose a populated survivor/inventory/expedition screen, resolve every visible portrait/icon/art element through the manifest, deliberately break one mapping and verify the strict asset gate and snapshot suite both fail, restore the mapping, and prove the release report returns zero missing/undeclared placeholders for that covered screen.**
