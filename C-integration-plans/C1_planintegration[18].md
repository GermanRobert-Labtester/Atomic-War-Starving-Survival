# C1 — Flagship Integration Plan [18]: Weight & Hygiene — Repository Classification, Asset Budgets & Reproducible Tooling

> **Output:** `C1_planintegration[18].md`
>
> **Source baseline:** Plan 56 — Weight & Hygiene: A Repository That Doesn't Fight Its Own Tools
>
> **Wave:** Continuity Wave 9 — *Weight, Durability & the Shop Window*
>
> **Depends on:** Plan 29A green documentation/rulebook gates; Plan 50A asset manifest; Plan 55B LFS/save-corpus policy; Plan 48B release artifacts; Plan 16A console verdicts; Plan 53A planning register if already landed.
>
> **Mandatory execution order:** 55B step 1 salvage → 56A → 57A → 56B → 58A → 55A → 56C → 57B → 55B → 57C → 55C → 58B → 59 → 58C.
>
> **Primary architectural rule:** the repository must encode file purpose in location. Source, generated output, design references, release evidence, tool configuration, test artifacts, and historical archives are separate categories with separate retention and tracking rules.
>
> **Primary hygiene rule:** every cleanup action begins with a dry run and ends with before/after byte counts. Hygiene is measured, not asserted.
>
> **Primary tooling rule:** clean clone + `setup-repo.sh` + `doctor.sh` must reproduce the same gate set that CI and agents claim to run.
>
> **Guardrails:** archive before delete when historical/design value exists; never weaken LFS or integrity policy to make a gate green; never hide project-level problems in machine-local ignore files; never move an asset before proving no import/export reference breaks; no hand-edited generated gate/status tables; no unsupported agent client silently gaining first-class config; no root-level migration script without lifecycle status.

---

# 0. Mission

ASHFALL's working copy has accumulated substantial non-source mass and filesystem ambiguity.

The source baseline identifies approximately:
- `.godot` ~962 MB;
- `.claude` ~143 MB;
- `.crush` ~96 MB;
- `.mimocode` ~58 MB;
- `Ashfall.Core.Tests` ~28 MB;
- `snapshot-capture` ~2 MB;
- `artifacts` ~1.8 MB;
- many agent/tool configuration directories;
- 13 client rulebook copies plus `AGENTS.md`;
- stray root scripts and XML results;
- 122 unreferenced UI design mockups living under shipped `assets/ui/`;
- split ignore policy between `.gitignore` and `.git/info/exclude`;
- generated QA trees with unclear retention;
- old maintenance scripts with no lifecycle;
- stale README statements about the Unity migration.

None of these issues is catastrophic in isolation.

Together they create a repository that encourages wrong assumptions:

```text
root-level script
→ looks current
→ agent runs stale migration

design mockup under assets/
→ looks shippable
→ export packs dead bytes

tracked test output
→ looks canonical
→ clone grows

machine-local ignore
→ local tree clean
→ teammate clone dirty

manifest says 46 gates
→ workflow runs fewer
→ green CI overstates truth
```

Plan 56 closes these classes of ambiguity.

The target repository lifecycle is:

```text
SOURCE
  ├── src/
  ├── Assets/Ashfall.Core/
  ├── scenes/
  ├── assets/             # runtime assets only
  └── scripts/            # active tooling only

DESIGN / DOCUMENTATION
  ├── docs/design/
  ├── docs/visual/
  ├── docs/hygiene/
  └── docs/archive/

GENERATED / EPHEMERAL
  ├── artifacts/
  ├── snapshot-capture/
  ├── TestResults/
  └── semantic-review/
      # ignored or explicitly retained by policy

GOLDEN / RELEASE EVIDENCE
  ├── snapshots/
  └── archived release artifacts

TOOL CONFIG
  ├── supported generated rulebooks
  └── machine-local client state ignored

MAINTENANCE
  ├── scripts/maintenance/
  └── scripts/archive/
```

The environment then converges on:

```text
git clone
→ setup-repo.sh
→ doctor.sh
→ verify-fast.sh
→ same gate list as CI
→ same rulebook contract
→ same ignore/LFS expectations
→ reproducible green state
```

---

# 1. Source-Evidence Interpretation

## 1.1 Working-copy weight is dominated by generated/tool state

The source baseline shows `.godot` and agent tool directories account for most non-source weight.

The plan therefore distinguishes:
- clone size;
- working-copy size;
- export/PCK size.

They are different budgets.

## 1.2 The root contains historical artifacts that look current

The Unity playmode XML and old patch scripts are particularly dangerous because their location implies relevance.

They must be archived, relocated, or deleted with explicit evidence.

## 1.3 Design mockups are currently packaged like runtime art

The 122 mockups in `assets/ui/Screens` and `assets/ui/HtmlBundles` have no runtime references.

They are design evidence, not game payload.

## 1.4 Ignore behavior differs per clone

Anything in `.git/info/exclude` is local truth.

Project-level generated paths belong in `.gitignore` instead.

## 1.5 Asset weight lacks ratcheted budgets

Real art/audio needs:
- tracked size;
- packed size;
- import cache size;
- import settings;
- LFS classification.

## 1.6 Existing tools already cover the problem

The source explicitly lists:
- repo hygiene;
- LFS gate;
- object inventory;
- asset orphan sweep;
- whitespace gate.

This plan should integrate and harden them rather than duplicate tooling.

## 1.7 CI truth is potentially split

Manifest, workflow, and `verify-fast.sh` can disagree.

56C makes the gate list generated/reconciled.

---

# 2. Non-Negotiable Repository Invariants

## INV-56.1 — Location communicates lifecycle

A file's path must make its category obvious.

## INV-56.2 — Historical evidence is archived, not left current-looking

Old test artifacts and migration evidence leave the root.

## INV-56.3 — Design mockups are not runtime assets

No design-only HTML/PNG ships in game asset paths without a manifest/runtime consumer.

## INV-56.4 — Generated ephemeral output is not tracked

Unless explicitly classified as:
- golden;
- release evidence;
- required fixture.

## INV-56.5 — Project ignore rules are shared

Project concerns live in `.gitignore`.

Machine-local rules are documented exceptions.

## INV-56.6 — Root scripts are active-entrypoint only

One-off migration scripts belong in lifecycle-managed maintenance/archive directories.

## INV-56.7 — Asset families have budgets

Art, sprites, UI, audio, fonts each have:
- tracked-size budget;
- packed-size budget;
- import-cache observation/budget.

## INV-56.8 — Asset growth is ratcheted

Unreviewed size growth beyond tolerance fails.

## INV-56.9 — LFS policy is enforced, never weakened

A failing file is reclassified/migrated properly.

## INV-56.10 — Import settings are contract state

Filter/mipmap/compression drift is testable.

## INV-56.11 — Store-facing and game-facing art are separate

Capsules/screenshots/press kits never ride in the runtime PCK by default.

## INV-56.12 — One generated QA home per artifact type

No duplicate trees for the same output class.

## INV-56.13 — Agent clients are explicitly supported or machine-local

No ambiguous first-class client state.

## INV-56.14 — Gate manifest, CI workflow, and local fast runner agree

Three descriptions of the same gate list are forbidden.

## INV-56.15 — Setup is reproducible

`setup-repo.sh` owns repository bootstrap.

## INV-56.16 — Doctor exposes drift, never hides it

Doctor reports:
- tooling;
- ignored dirs;
- cache;
- red gates;
- dirty-tree weight.

## INV-56.17 — Hygiene claims include bytes

Before/after:
- working copy;
- clone/object store;
- PCK/export.

## INV-56.18 — Cleanup is dry-run first

Moving/deleting many files requires preview and receipt.

---

# 3. Definition of Done

Plan 56 closes only when:

- repository hygiene reports are captured before changes;
- root files are fully classified;
- Unity-era result artifacts are archived;
- one-off root scripts are moved/archive-lifecycle marked or deleted with evidence;
- UI mockups are relocated out of shipped asset directories;
- mockups remain linked to live/shelved console records where useful;
- PCK/export excludes design artifacts;
- PCK size delta is measured;
- README stale Unity claims are corrected;
- mirrored rulebooks are regenerated if necessary;
- project ignore rules move from local exclude to `.gitignore`;
- machine-local ignore exceptions are documented;
- generated output retention rules exist;
- `.trx` and build output are not committed;
- supported agent-client set is documented;
- new hygiene gate rejects stray root result/script files, design files under assets, and tracked ephemeral outputs;
- working-copy/clone/PCK before/after sizes are published;
- asset-family budgets exist;
- LFS policy matches actual tracking;
- LFS health gate includes size delta;
- import presets are asserted;
- mobile/desktop compression choice is documented;
- duplicate-byte assets are identified and deduplicated safely;
- store-facing assets are separated from runtime assets;
- asset gallery is regenerated;
- duplicate generated asset/QA trees are consolidated;
- Godot import-time budget is measured;
- runtime audio format policy exists;
- asset budget gate self-test proves oversize fixtures fail;
- `.gitattributes` conformance is tested;
- CI manifest/workflow/verify-fast gate sets agree;
- red documentation gates are green before 56C completion;
- `setup-repo.sh` is the single bootstrap;
- `doctor.sh` exists;
- generated-output locations have owners/regeneration commands;
- maintenance scripts have lifecycle markers;
- executed plan docs are archived per roadmap policy;
- agents run the same fast tier as humans;
- fast-tier time budget exists;
- doctor smoke test runs in CI;
- gate status table is generated;
- clean clone + setup + doctor + verify-fast passes twice;
- all standard build/selftests/export-smoke gates pass.

---

# 4. Phase P0 — Measure Before Moving Anything

## P0.1 Capture repository identity

Record:

```text
commit SHA
branch
dirty path count
tracked file count
git object size
working-copy size
PCK/export size
LFS object count
LFS tracked size
```

## P0.2 Run existing hygiene reports

Run current commands:

```bash
bash scripts/ci/repo-hygiene-report.sh
bash scripts/ci/git-object-inventory.sh
bash scripts/ci/lfs-health-check.sh
bash scripts/ci/asset-orphan-sweep.sh
```

Store outputs as temporary evidence.

## P0.3 Capture top-level disk profile

Record `du -sh` for:
- `.godot`;
- agent dirs;
- source;
- tests;
- assets;
- snapshots;
- artifacts;
- generated review dirs.

## P0.4 Capture root-file inventory

Generate table:

```text
file
extension
tracked?
referenced?
last modified/commit
suspected category
disposition
```

## P0.5 Capture asset-tree inventory

Per family:

```text
family
file count
tracked bytes
LFS bytes
packed bytes
import-cache bytes
manifest-used
orphaned
```

## P0.6 Capture CI truth inventory

Compare:
- `docs/ci/CI_GATE_MANIFEST.json`;
- `.github/workflows/ci.yml`;
- `scripts/ci/verify-fast.sh`;
- `scripts/ci/run-gates.py`.

List differences.

---

# TASK 56A — Classify the Repository and Act

# 56A.0 Goal

No file should live in a path that misrepresents what it is.

---

## 56A.1 Define filesystem categories

Create:

`docs/tools/TOOLING_CLASSIFICATION_AND_LIFECYCLE.md`

Top categories:

```text
RUNTIME_SOURCE
RUNTIME_ASSET
DESIGN_REFERENCE
GENERATED_EPHEMERAL
GOLDEN_TEST_ARTIFACT
RELEASE_EVIDENCE
TEST_FIXTURE
MAINTENANCE_SCRIPT
ARCHIVED_HISTORY
TOOL_CONFIG_SHARED
TOOL_CONFIG_LOCAL
```

## 56A.2 Map canonical locations

Document canonical roots for each category.

No ambiguity.

## 56A.3 Quarantine Unity-era XML

Move:
- `batch20-playmode-results.xml`;
- `art-wiring-results.xml`;

to:

`docs/archive/unity-era/`

Include README/index explaining:
- historical origin;
- not current CI;
- retained as evidence.

## 56A.4 Verify no live reference before move

Search:
- docs;
- scripts;
- CI;
- root tooling.

Update links.

## 56A.5 Root Python script inventory

Classify:
- `export_code.py`;
- `fix_queuefree.py`;
- `fix_syntax.py`;
- `fix_using.py`;
- `safe_fix.py`;
- `generate_master_doc.py`;
- `test_parse.py`;
- any current extras.

## 56A.6 Maintenance lifecycle metadata

For retained maintenance scripts, add header or sidecar:

```text
status: ACTIVE / ONE_SHOT / RETIRED
purpose
created_for
safe_to_rerun
preconditions
last_verified_commit
archive_after
```

## 56A.7 Active scripts move to `scripts/maintenance/`

Only if still useful.

## 56A.8 Retired scripts move to `scripts/archive/`

Retain:
- reason;
- associated plan/commit.

## 56A.9 Delete truly worthless scripts

Only when:
- no historical value;
- no references;
- no unique migration knowledge.

Record deletion receipt.

## 56A.10 Root asset-like file classification

Audit:
- `UI_StyleReference_01.jpg`;
- `icon.svg`;
- any loose images.

Move to:
- runtime asset path if real;
- design docs if reference;
- project branding root only if required by engine/tool.

## 56A.11 Mockup inventory

Recount:
- `assets/ui/Screens/`;
- `assets/ui/HtmlBundles/`.

Map each filename to:
- live panel;
- shelved console;
- unknown/dead design.

## 56A.12 Relocate mockups

Preferred:

```text
docs/design/mockups/
  screens/
  html/
```

or archive equivalents.

## 56A.13 Preserve mockup linkage

For each mockup:
- link from panel/consoles registry;
- status;
- approval/reference role.

## 56A.14 No silent mockup deletion

Shelved console designs remain historical/spec evidence.

## 56A.15 Runtime asset assertion

After move:

```text
assets/ui/Screens/ absent or runtime-only
assets/ui/HtmlBundles/ absent
```

unless a manifest/runtime consumer proves otherwise.

## 56A.16 Export/PCK exclusion verification

Rebuild export.

Compare:
- packed file list;
- bytes.

## 56A.17 PCK size delta

Record:

```text
before
after
absolute delta
percentage delta
```

## 56A.18 README migration truth

Update stale claim about Unity tree.

README must describe current actual state.

## 56A.19 Legacy migration section

Point to:
- tooling lifecycle doc;
- archive;
- current authority docs.

## 56A.20 Rulebook regeneration

If README text mirrored:
- regenerate synchronized rulebooks via Plan 29A tooling.

Never hand-edit copies.

## 56A.21 Ignore-policy audit

Collect:
- `.gitignore`;
- `.git/info/exclude`;
- client-specific ignores;
- `.gdignore`.

## 56A.22 Move project concerns to `.gitignore`

Examples:
- `.claude/worktrees/`;
- generated local outputs;
- build output;
- temp capture.

## 56A.23 Machine-local exception policy

Allow local-only excludes only for:
- machine path;
- developer personal file;
- temporary experimentation.

Document reason.

## 56A.24 Ignore conformance test

A scripted check asserts shared project patterns do not depend on `.git/info/exclude`.

## 56A.25 Generated-output retention table

Create:

`docs/hygiene/GENERATED_OUTPUT_POLICY.md`

Rows:
- `artifacts/`;
- `snapshots/`;
- `snapshot-capture/`;
- `semantic-review/`;
- `TestResults/`;
- test build output;
- export output;
- generated reports.

Fields:

```text
category
tracked?
owner
regenerate command
retention
release archive?
```

## 56A.26 Golden vs capture distinction

`snaphots/`:
- golden tracked contract.

`snapshot-capture/`:
- ephemeral working output.

## 56A.27 TestResults policy

`.trx`/XML:
- ephemeral unless explicitly archived as release evidence.

## 56A.28 Test build output

Ensure:
- `bin/`;
- `obj/`;
- test run artifacts
ignored/untracked.

## 56A.29 Test fixture audit

Binary fixtures:
- retain if needed;
- apply LFS per 55B if required.

## 56A.30 Tool-client inventory

Classify dot dirs:
- supported shared;
- supported generated;
- machine-local;
- obsolete.

## 56A.31 Supported client list

Create one authoritative list.

Examples may include:
- Claude;
- Codex;
- Qwen;
- Vibe;
- Antigravity;
- Gemini;
- etc.

Recompute current actual set.

## 56A.32 Rulebook contract

`AGENTS.md` remains source.

Client rulebooks:
- generated;
- synchronized;
- no divergence.

## 56A.33 Client config policy

Tool runtime/cache/session dirs:
- local;
- ignored;
- never committed unless config is deliberately shared.

## 56A.34 Tool-config weight audit

For large `.claude`, `.crush`, `.mimocode` directories:
- determine tracked vs untracked;
- cache vs configuration.

## 56A.35 No cache in clone

Shared repository must not contain large machine cache.

## 56A.36 Hygiene gate

Create:

`scripts/ci/repo-hygiene-gate.sh`

Tier 2 or fast subset as appropriate.

## 56A.37 Gate rule — stray root files

Fail on new root:
- `*.xml`;
- one-off `fix_*.py`;
- `safe_fix.py`;
- test results;
unless allowlisted as active entrypoint.

## 56A.38 Gate rule — design under assets

Fail if:
- design artifact under runtime `assets/`;
- no asset manifest row/runtime consumer;
- known HTML mockup in runtime tree.

## 56A.39 Gate rule — tracked ephemeral output

Fail tracked:
- `.trx`;
- captures;
- temp reports;
- build output.

## 56A.40 Gate rule — maintenance lifecycle

Scripts in `scripts/maintenance/` require lifecycle marker.

## 56A.41 Dry-run mode

Gate/tool can report moves without mutating.

## 56A.42 Quarantine mode

Repo hygiene skill may move suspicious files to temp quarantine for review.

No destructive default.

## 56A.43 Before/after working-copy size

Measure:
- total;
- excluding `.godot`;
- excluding machine caches.

## 56A.44 Clone/object-store size

Fresh clone or object inventory after commit.

## 56A.45 Export size

Record PCK/export.

## 56A.46 Wave ledger receipt

Write metrics to generated/authoritative Wave 9 ledger.

### 56A DoD

Every file category is knowable from location, design material no longer masquerades as runtime assets, project ignores are shared, and the repository is measurably lighter.

---

# TASK 56B — Asset, LFS, Import & Build-Weight Policy

# 56B.0 Goal

Make runtime asset weight a deliberate, budgeted property.

---

## 56B.1 Recompute asset families

Minimum:
- `art/`;
- `sprites/`;
- `ui/`;
- `audio/`;
- `fonts/`.

Add others only if real.

## 56B.2 Asset budget file

Create:

`docs/visual/ASSET_BUDGETS.md`

Per family:

```text
tracked_bytes
packed_bytes
import_cache_bytes
file_count
LFS_bytes
growth_tolerance
owner
review requirement
```

## 56B.3 Baseline budget artifact

Machine-readable:

`artifacts/asset-budget-baseline.json`

## 56B.4 Ratchet semantics

Growth beyond tolerance:
- fail;
- require reviewed baseline update.

## 56B.5 Budget tolerance

Use:
- absolute + percentage threshold;
- family-specific where needed.

Avoid one global arbitrary limit.

## 56B.6 Asset budget gate

Create:

`scripts/ci/asset-budget-gate.sh`

## 56B.7 Budget gate report

Output:

```text
family
baseline
current
delta
tolerance
status
top contributors
```

## 56B.8 Oversize fixture self-test

Add test fixture.

Assert gate fails.

## 56B.9 LFS policy audit

Inspect `.gitattributes`.

Confirm intended policy:
- large images/fonts via LFS;
- audio plain binary or current stated rule.

## 56B.10 LFS intent doc

Document why each extension is:
- LFS;
- normal Git.

## 56B.11 LFS health extension

Report:
- tracked files;
- expected LFS but not LFS;
- LFS but unnecessary;
- size delta.

## 56B.12 Never weaken LFS to pass

Any mismatch:
- migrate attributes/history if needed;
- do not remove rule.

## 56B.13 Import contract per family

Create table:

```text
family
filter
mipmaps
compression
lossy/lossless
repeat
color space
runtime target
```

## 56B.14 UI import policy

UI-only textures:
- usually no mipmaps;
- filtering according to art style;
- compression chosen deliberately.

Use actual rendering requirements.

## 56B.15 World art import policy

May require:
- mipmaps;
- filtering;
- larger texture handling.

## 56B.16 Font policy

Fonts:
- LFS/tracking;
- import;
- fallback;
- no duplicate embedded font copies.

## 56B.17 Audio format inventory

Recount:
- WAV;
- MP3;
- OGG.

## 56B.18 Canonical runtime audio policy

Decide:
- one or two accepted runtime formats;
- source/master format if different;
- conversion pipeline.

## 56B.19 Audio conversion script

If needed:
- deterministic command;
- documented quality settings;
- no repeated hand conversion.

## 56B.20 No fourth audio format

Gate unknown runtime audio extension.

## 56B.21 ETC2/ASTC setting review

Inspect `import_etc2_astc=false`.

Document:
- platform target;
- why setting retained or changed.

## 56B.22 No speculative platform optimization

Do not change compression solely because toggle looks odd.

Measure/export target.

## 56B.23 Duplicate-byte scan

Hash assets across:
- `assets/art`;
- `assets/ui/Icons`;
- sprites;
- duplicated generated trees.

## 56B.24 Duplicate report

Output:
- hash;
- paths;
- bytes wasted;
- manifest consumers.

## 56B.25 Safe dedupe

Before deleting duplicate:
- consolidate manifest references;
- update resource paths;
- snapshot test.

## 56B.26 Store-facing asset root

Create/define:

```text
store/
press/
marketing/
```

or equivalent outside runtime `assets/`.

## 56B.27 Runtime-export exclusion

Export preset excludes store/press trees.

## 56B.28 Asset gallery regeneration

Generate `docs/visual/ASSET_GALLERY.md`.

Include:
- asset ID;
- path;
- size;
- provenance;
- usage;
- LFS state;
- import class.

## 56B.29 Gallery generated marker

Never hand-edit.

## 56B.30 Duplicate generated tree audit

Inspect:
- `snapshot-capture/`;
- `snapshots/`;
- `AI_Generated/` variants;
- other art staging trees.

## 56B.31 One home per class

Examples:

```text
snapshots/ = tracked golden
snapshot-capture/ = ignored temp
docs/design/generated-references/ = design
assets/ = approved runtime
```

## 56B.32 Asset approval pipeline

Plan 50A/import script moves approved source into runtime.

No direct dump from generator into `assets/`.

## 56B.33 Import-time measurement

Measure:

```text
godot --headless --import
```

On:
- clean cache;
- warm cache.

## 56B.34 Import-time budget

Record median/p95 if feasible.

## 56B.35 Cached-import strategy

Only if import time exceeds budget.

Document CI cache key:
- Godot version;
- asset hashes;
- import settings.

## 56B.36 PCK family breakdown

Report packed bytes per asset family.

## 56B.37 Design removal receipt

Quantify how much 56A mockup relocation reduced PCK.

## 56B.38 Asset orphan sweep integration

Orphan results include:
- manifest-mapped but unused;
- unmanifested runtime asset;
- design artifact in assets.

## 56B.39 Import preset conformance tests

Pick representative:
- UI PNG;
- world art;
- icon;
- font;
- audio.

Assert import config.

## 56B.40 Snapshot safety

If compression/import changes visual:
- snapshot diff review required.

## 56B.41 Audio playback regression

If converting audio:
- ensure playback duration/loop behavior unchanged.

## 56B.42 Build/export smoke

Run real export after policy changes.

## 56B.43 Asset-budget release artifact

Plan 48B can publish:
- asset size;
- PCK size;
- delta.

## 56B.44 Docs pointer

Add hygiene docs link to asset budgets.

### 56B DoD

Every runtime asset belongs to a family with a tracking policy, import contract, size budget, and export purpose.

---

# TASK 56C — Working Environment, Agents & CI Parity

# 56C.0 Goal

Make a fresh clone reproducibly healthy for humans and agents.

---

## 56C.1 Precondition: Plan 29A doc gates green

Do not start 56C acceptance while:
- rulebook sync;
- docs index;
- skills catalog
are red.

## 56C.2 Gate-set reconciliation

Compare:
- manifest;
- workflow;
- `run-gates.py`;
- `verify-fast.sh`.

Create one authoritative generated representation.

## 56C.3 Preferred authority

Recommended:
- `docs/ci/CI_GATE_MANIFEST.json` as machine-readable authority;
- workflow and status docs generated/validated against it.

Reconcile with current architecture.

## 56C.4 Workflow enforcement

`.github/workflows/ci.yml` must actually invoke:
- fast tier;
- required named gates.

## 56C.5 No wish-list gates

A gate listed as required but not executed:
- CI fails consistency test.

## 56C.6 Verify-fast parity

Local runner uses same fast-tier manifest.

No copied list if script can call `run-gates.py --tier fast`.

## 56C.7 Single bootstrap

`setup-repo.sh` owns:
- Git case setting;
- LFS install;
- shared ignore prerequisites;
- any required submodule/tool bootstrap;
- local folder creation if needed.

## 56C.8 Idempotent setup

Run twice:
- no harmful changes;
- same output/state.

## 56C.9 No destructive setup

Setup does not:
- delete user work;
- rewrite uncommitted files;
- force reset.

## 56C.10 Doctor command

Create:

`scripts/ci/doctor.sh`

## 56C.11 Doctor — tooling

Report:
- dotnet version;
- Godot version;
- Git;
- Git LFS;
- Python;
- required shell tools.

## 56C.12 Doctor — repository settings

Check:
- `core.ignorecase=false`;
- LFS installed;
- clean/dirty tree;
- current branch;
- expected ignore behavior.

## 56C.13 Doctor — cache state

Report:
- `.godot` size;
- stale import cache suspicion;
- generated-tree sizes.

Do not auto-delete by default.

## 56C.14 Doctor — local dirs

Report:
- unignored tool caches;
- unsupported client directories;
- machine-local artifacts.

## 56C.15 Doctor — gates

Run or summarize:
- fast gate status;
- red doc gates;
- generated drift.

## 56C.16 Doctor — disk profile

Show top local contributors.

Useful for agents with constrained disk.

## 56C.17 Doctor exit codes

Define:
- 0 healthy;
- nonzero actionable mismatch.

Warnings can remain 0 if non-blocking.

## 56C.18 Doctor output format

Human-readable plus optional:
- JSON.

## 56C.19 Supported client authority

Generate/document:
- client;
- rulebook filename;
- shared config path if any;
- local/cache path;
- support status.

## 56C.20 Unsupported client policy

Unsupported tool can still be used personally.

Its config/cache remains local and receives no synced rulebook guarantee.

## 56C.21 Rulebook sync

All supported rulebooks generated from `AGENTS.md`.

## 56C.22 Rulebook omission gate

Supported client without generated rulebook:
- fail.

## 56C.23 Extra rulebook gate

Unknown committed client rulebook:
- warn/fail depending policy.

## 56C.24 Generated QA output homes

Standardize:
- artifacts;
- snapshots;
- captures;
- semantic review;
- test results.

## 56C.25 One owner and regeneration command

Every generated tree has:
- owner script;
- `--check` where appropriate.

## 56C.26 Generator idempotence

Run generator twice:
- no diff second time.

## 56C.27 Maintenance script lifecycle

Active:
- `scripts/maintenance/`.

Completed:
- move to `scripts/archive/`.

## 56C.28 Archive note

Each archived script:
- original purpose;
- completion commit;
- not safe/current entrypoint.

## 56C.29 Plan-document hygiene

Use Plan 29C/53A register.

Executed plans:
- archive;
- status at top;
- completion commit.

## 56C.30 Avoid deleting historical plans

Move/archive.

## 56C.31 Agent parity rule

Every automated author must run:

```bash
bash scripts/ci/verify-fast.sh
```

before claiming completion.

## 56C.32 Result receipt

Agent output/PR includes:
- command;
- pass/fail summary;
- commit SHA.

## 56C.33 Human parity

Same command for human contributors.

No agent-only special gate subset.

## 56C.34 Fast-tier budget

Measure:
- total runtime;
- slowest gates.

## 56C.35 Time budget

Set explicit target.

Example:
- under X minutes on CI reference runner.

Use measured current baseline.

## 56C.36 Slow-gate tiering

Move:
- large corpus;
- nightlies;
- heavy soak;
- huge asset scans
to appropriate tier if they threaten fast loop.

Do not remove coverage.

## 56C.37 Gate status table

`docs/CI.md` includes generated table:

```text
gate
tier
owner
last green
command
```

## 56C.38 No manually typed status

Generator owns table.

## 56C.39 Doctor smoke CI job

Run doctor in CI/fresh environment.

## 56C.40 Clean-clone test

Fresh clone:
1. `setup-repo.sh`;
2. `doctor.sh`;
3. `verify-fast.sh`.

Must pass.

## 56C.41 Manifest/workflow/runner consistency test

Automated test compares all three.

## 56C.42 Verify-fast idempotence

Run twice.

Second run:
- no generated diff;
- same pass state.

## 56C.43 Environment receipt

Publish:
- tool versions;
- gate counts;
- fast runtime;
- doctor status.

## 56C.44 Current authority update

Update `docs/CURRENT_AUTHORITY.md` with:
- bootstrap;
- doctor;
- CI source.

Generated links/claims where possible.

## 56C.45 CI docs regeneration

Regenerate:
- docs index;
- skills catalog;
- rulebooks;
- gate table.

## 56C.46 No local-only fix

If clean clone fails but developer machine passes:
- fix repository/config;
- do not add a personal exclude/workaround.

## 56C.47 Cross-platform considerations

Where supported:
- path casing;
- line endings;
- shell availability;
- Git settings.

At minimum Linux reference environment must be deterministic.

## 56C.48 Agent-client smoke

For one or more supported clients:
- verify rulebook found;
- no committed cache requirement.

## 56C.49 Failure diagnostics

Doctor/gates must say:
- what is wrong;
- how to fix;
- which command to run.

## 56C.50 Docs

Update:
- `docs/CI.md`;
- tooling lifecycle;
- setup instructions;
- current authority.

### 56C DoD

A clean clone can be bootstrapped, diagnosed, and verified by the same contract used by CI and every supported agent.

---

# 5. Cross-Task Dependency Graph

```text
29A green docs
      │
      ▼
56A classify/move
      │
      ▼
56B asset budgets/LFS/import
      │
      ▼
56C setup/doctor/CI parity
```

Supporting:

```text
50A asset manifest ──────► mockup/runtime classification
55B LFS policy ──────────► binary tracking
48B release evidence ────► retained reports
16A console verdicts ────► mockup linkage
53A plan register ───────► plan-document archive
```

---

# 6. Filesystem Classification Contract

Every root/path belongs to one category.

Example:

```text
assets/                 RUNTIME_ASSET
docs/design/            DESIGN_REFERENCE
docs/archive/           ARCHIVED_HISTORY
artifacts/               GENERATED_EPHEMERAL or RELEASE_EVIDENCE by subpolicy
snapshots/               GOLDEN_TEST_ARTIFACT
snapshot-capture/        GENERATED_EPHEMERAL
scripts/maintenance/     MAINTENANCE_SCRIPT
scripts/archive/         ARCHIVED_HISTORY
```

---

# 7. Root Cleanliness Contract

Allowed root files should be intentional:
- solution/project;
- README;
- license;
- engine config;
- canonical entry scripts.

Everything else requires:
- allowlist;
- documented purpose.

---

# 8. Design vs Runtime Asset Contract

A file may live under runtime `assets/` only when:

```text
manifest row exists
AND intended runtime consumer exists
AND export policy includes it
```

Design reference:
- docs/design or archive.

---

# 9. Generated Output Contract

Generated output must answer:

```text
tracked?
why?
regenerate how?
retained how long?
release evidence?
```

No undefined tree.

---

# 10. Ignore Policy Contract

Project-level ignore:
- `.gitignore`.

Machine-local:
- local excludes allowed only when not project semantics.

Doctor verifies.

---

# 11. Asset Budget Contract

Per family:

```text
baseline tracked
baseline packed
tolerance
current
delta
top growth
```

Review updates baseline.

---

# 12. LFS Contract

`.gitattributes` is authoritative.

Gate verifies:
- extension policy;
- actual pointer state;
- unexpected large binaries.

---

# 13. Import Contract

Every family has a preset.

New asset outside preset:
- fail.

---

# 14. Store-Facing Asset Contract

Marketing/store assets:
- outside runtime assets;
- excluded from PCK;
- may have separate LFS policy.

---

# 15. Audio Format Contract

Define:
- source/master format;
- runtime format;
- conversion command;
- looping/streaming defaults.

---

# 16. Maintenance Script Contract

Each active script has:

```text
purpose
safe_to_rerun
dry_run
owner
last_verified
retire_condition
```

---

# 17. Supported Client Contract

One table:

```text
client
supported?
rulebook
shared config
local dirs
sync source
```

No ambiguity.

---

# 18. CI Truth Contract

Preferred:

```text
CI_GATE_MANIFEST
→ run-gates.py
→ verify-fast.sh
→ ci.yml
→ docs/CI.md generated table
```

One manifest, multiple consumers.

---

# 19. Bootstrap Contract

`setup-repo.sh`:
- installs/configures repo prerequisites;
- idempotent;
- non-destructive;
- documented.

---

# 20. Doctor Contract

Doctor does not repair by default.

It reports.

Optional fix flags may exist only for safe actions.

---

# 21. Before/After Receipt

Publish:

```text
working copy total
working copy excluding cache
git objects
LFS objects
runtime assets
design assets
PCK/export
import time
fast CI time
```

Before and after.

---

# 22. Failure Injection Matrix

## N56.1 Add root `random-results.xml`
Expected: hygiene gate fails.

## N56.2 Add `fix_temp.py` at root
Expected: hygiene gate fails.

## N56.3 Put HTML design mockup under runtime assets
Expected: hygiene/asset gate fails.

## N56.4 Track `.trx`
Expected: ephemeral-output gate fails.

## N56.5 Put project ignore only in `.git/info/exclude`
Expected: doctor/ignore conformance fails.

## N56.6 Add oversized image beyond family tolerance
Expected: asset-budget gate fails.

## N56.7 Add PNG expected in LFS but committed raw
Expected: LFS gate fails.

## N56.8 Change import settings off preset
Expected: import conformance fails.

## N56.9 Add unsupported audio format
Expected: asset/audio gate fails.

## N56.10 CI manifest lists gate not executed by workflow
Expected: parity test fails.

## N56.11 Supported client missing rulebook
Expected: sync gate fails.

## N56.12 Clean clone passes locally only because user exclude hides file
Expected: doctor/fresh-clone test fails.

---

# 23. Test Pyramid

## Tier 1 — Static filesystem
- root files;
- ignore rules;
- maintenance markers.

## Tier 2 — Asset policy
- budgets;
- LFS;
- import presets;
- duplicate hashes.

## Tier 3 — Generated truth
- rulebooks;
- docs;
- gate manifests.

## Tier 4 — Clean clone
- setup;
- doctor;
- verify-fast.

## Tier 5 — Export
- PCK size;
- runtime smoke.

---

# 24. CI / Gate Set

Add/strengthen:

```text
repo_hygiene
asset_budget
lfs_health
import_preset
ci_gate_parity
doctor_smoke
rulebook_sync
generated_output_policy
```

Reuse existing gates where possible.

---

# 25. Verification Commands

Run per task:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/repo-hygiene-report.sh
bash scripts/ci/git-object-inventory.sh
bash scripts/ci/lfs-health-check.sh
bash scripts/ci/asset-orphan-sweep.sh
bash scripts/ci/asset-budget-gate.sh
bash scripts/ci/doctor.sh
bash scripts/ci/godot-asset-gate.sh
bash scripts/ci/export-smoke-boot.sh
bash scripts/ci/verify-fast.sh
```

Also:
- shader/material import lint where applicable;
- generated docs/rulebook `--check`;
- fresh-clone smoke;
- `verify-fast.sh` twice.

---

# 26. Recommended Commit Breakdown

```text
56A-1 baseline reports + classification taxonomy
56A-2 archive Unity-era artifacts
56A-3 root maintenance script lifecycle
56A-4 design mockup relocation/linkage
56A-5 README + ignore-policy correction
56A-6 generated-output retention
56A-7 supported-client/tool config classification
56A-8 hygiene gate + before/after receipt

56B-1 asset-family baseline/budgets
56B-2 LFS policy/conformance
56B-3 import preset contracts
56B-4 duplicate-hash dedupe
56B-5 store/runtime art split
56B-6 generated-tree consolidation
56B-7 import/audio format policy
56B-8 asset-budget gate/export metrics/docs

56C-1 gate-set parity audit
56C-2 setup-repo consolidation
56C-3 doctor command
56C-4 supported-client/rulebook policy
56C-5 generated-output owners/generators
56C-6 maintenance/plan archive lifecycle
56C-7 fast-tier budget + parity tests
56C-8 clean-clone smoke/docs/status table
```

---

# 27. Risk Register

## R56.1 Moving mockups breaks references/export

Mitigation:
- search refs;
- dry run;
- export smoke;
- PCK diff.

## R56.2 Deleting old scripts loses migration knowledge

Mitigation:
- archive lifecycle;
- delete only if redundant.

## R56.3 Ignore consolidation causes tracked surprises

Mitigation:
- `git status --ignored`;
- fresh clone test.

## R56.4 LFS migration bloats history

Mitigation:
- avoid history rewrite unless necessary;
- apply forward policy first;
- coordinate 55B.

## R56.5 Import preset changes alter visuals

Mitigation:
- snapshot diff;
- targeted review.

## R56.6 Asset budget becomes bureaucratic

Mitigation:
- tolerance bands;
- generated delta report;
- review only on meaningful growth.

## R56.7 Doctor becomes destructive

Mitigation:
- report-only default.

## R56.8 CI parity changes slow fast tier

Mitigation:
- measure;
- tier heavy gates;
- preserve coverage.

---

# 28. Acceptance Checklist

## P0

- [ ] commit/branch captured
- [ ] working-copy size captured
- [ ] git object size captured
- [ ] PCK/export size captured
- [ ] LFS inventory captured
- [ ] hygiene report captured
- [ ] asset orphan report captured
- [ ] root file inventory
- [ ] asset family inventory
- [ ] CI truth comparison

## 56A — Classification

- [ ] lifecycle taxonomy written
- [ ] canonical locations documented
- [ ] Unity XML archived
- [ ] archive index updated
- [ ] root scripts classified
- [ ] maintenance markers
- [ ] active scripts moved
- [ ] retired scripts archived
- [ ] deletions receipted
- [ ] loose root images classified
- [ ] mockups recounted
- [ ] mockups mapped to panel/console verdict
- [ ] design mockups relocated
- [ ] no silent mockup deletion
- [ ] runtime asset dirs clean
- [ ] export filter verified
- [ ] PCK size delta
- [ ] README corrected
- [ ] rulebooks regenerated if mirrored
- [ ] ignore policies inventoried
- [ ] project concerns in `.gitignore`
- [ ] machine-local exceptions documented
- [ ] ignore conformance test
- [ ] generated-output policy
- [ ] golden vs capture explicit
- [ ] TestResults policy
- [ ] bin/obj ignored
- [ ] fixtures retained correctly
- [ ] tool clients classified
- [ ] supported client list
- [ ] rulebook source contract
- [ ] client config/cache policy
- [ ] large tool dirs classified
- [ ] no machine cache tracked
- [ ] hygiene gate
- [ ] root-file rule
- [ ] design-under-assets rule
- [ ] tracked-ephemeral rule
- [ ] maintenance-lifecycle rule
- [ ] dry-run mode
- [ ] quarantine mode
- [ ] before/after working copy
- [ ] clone/object size
- [ ] export size
- [ ] wave ledger receipt

## 56B — Assets

- [ ] asset families recounted
- [ ] ASSET_BUDGETS.md
- [ ] machine baseline
- [ ] ratchet semantics
- [ ] tolerances
- [ ] asset-budget gate
- [ ] family delta report
- [ ] oversize fixture fails
- [ ] LFS policy audited
- [ ] LFS intent documented
- [ ] LFS size delta
- [ ] no weakened policy
- [ ] import contract
- [ ] UI preset
- [ ] world-art preset
- [ ] font policy
- [ ] audio formats recounted
- [ ] runtime audio policy
- [ ] conversion pipeline if needed
- [ ] unknown audio format gate
- [ ] ETC2/ASTC reviewed
- [ ] no speculative toggle change
- [ ] duplicate-byte scan
- [ ] duplicate report
- [ ] safe dedupe
- [ ] store-facing root
- [ ] export exclusion
- [ ] asset gallery regenerated
- [ ] gallery generated marker
- [ ] duplicate QA/art trees audited
- [ ] one home per output kind
- [ ] approval pipeline
- [ ] clean import time measured
- [ ] warm import time measured
- [ ] import-time budget
- [ ] cache strategy only if needed
- [ ] PCK family breakdown
- [ ] mockup removal delta
- [ ] orphan sweep integrated
- [ ] preset conformance tests
- [ ] snapshot review for import changes
- [ ] audio playback regression
- [ ] export smoke
- [ ] release artifact metrics
- [ ] docs pointer

## 56C — Tooling/CI parity

- [ ] doc gates green precondition
- [ ] gate-set reconciliation
- [ ] machine-readable authority chosen
- [ ] workflow executes required gates
- [ ] no wish-list gate
- [ ] verify-fast uses same list
- [ ] setup-repo single bootstrap
- [ ] setup idempotent
- [ ] setup non-destructive
- [ ] doctor exists
- [ ] doctor checks tool versions
- [ ] doctor checks repo settings
- [ ] doctor reports cache state
- [ ] doctor reports local dirs
- [ ] doctor reports gates
- [ ] doctor reports disk profile
- [ ] doctor exit codes
- [ ] human + JSON output if useful
- [ ] supported-client authority
- [ ] unsupported-client policy
- [ ] rulebooks synced
- [ ] missing supported rulebook fails
- [ ] extra rulebook policy
- [ ] generated QA roots standardized
- [ ] each generated root has owner/command
- [ ] generator idempotence
- [ ] maintenance lifecycle
- [ ] archived script notes
- [ ] plan docs archived via register
- [ ] historical plans preserved
- [ ] agent verify-fast parity
- [ ] command receipt
- [ ] human parity
- [ ] fast-tier runtime measured
- [ ] fast-tier target
- [ ] heavy gates tiered
- [ ] generated gate status table
- [ ] no hand-typed status
- [ ] doctor smoke CI job
- [ ] clean-clone test
- [ ] manifest/workflow/runner parity test
- [ ] verify-fast twice
- [ ] environment receipt
- [ ] CURRENT_AUTHORITY updated
- [ ] docs regenerated
- [ ] no local-only workaround
- [ ] platform/path notes
- [ ] supported-client smoke
- [ ] actionable failure messages
- [ ] docs complete

---

# 29. Ship / No-Ship Gate

**SHIP** only if:

```text
unclassified_root_files == 0
AND unity_era_current_looking_artifacts == 0
AND active_one_off_scripts_at_root == 0
AND design_mockups_in_runtime_assets == 0
AND exported_design_mockups == 0
AND tracked_ephemeral_outputs == 0
AND project_ignore_rules_local_only == 0
AND runtime_asset_families_without_budget == 0
AND lfs_policy_violations == 0
AND import_preset_violations == 0
AND unsupported_runtime_audio_formats == 0
AND duplicate_asset_bytes_above_review_threshold == 0
AND store_assets_in_runtime_pck == 0
AND generated_output_classes_without_owner == 0
AND supported_clients_without_rulebook_contract == 0
AND ci_manifest_workflow_fast_runner_drift == 0
AND doctor_clean_clone == pass
AND setup_repo_idempotence == pass
AND verify_fast_first_run == pass
AND verify_fast_second_run == pass
AND asset_budget_gate == pass
AND lfs_health_check == pass
AND godot_asset_gate == pass
AND export_smoke_boot == pass
AND documentation_gates == pass
AND before_after_byte_receipt_published == true
```

Otherwise: **NO SHIP**.

---

# 30. Implementer Handoff

1. Run the existing hygiene/object/LFS/orphan reports first.
2. Record bytes before moving anything.
3. Define lifecycle categories and canonical paths.
4. Archive Unity-era artifacts before cleaning the root.
5. Move active one-off scripts into maintenance and retire old ones into archive.
6. Move unreferenced UI mockups out of runtime asset paths, preserving links to live/shelved panel records.
7. Rebuild/export immediately after asset relocation and record the PCK delta.
8. Correct stale README migration claims and regenerate mirrored rulebooks.
9. Move project ignore rules out of local excludes.
10. Define generated-output retention before deleting artifacts.
11. Separate tracked goldens from ephemeral captures.
12. Publish the supported agent-client list and cache/config policy.
13. Land the hygiene gate before proceeding to asset budgets.
14. Publish asset-family size/LFS/import baselines.
15. Give LFS and import policy enforcement teeth; never weaken them to pass.
16. Deduplicate by hash only after reference/manifest analysis.
17. Separate marketing/store art from game assets.
18. Consolidate duplicate generated trees.
19. Define the runtime audio format policy.
20. Measure clean/warm Godot import times.
21. Reconcile CI manifest, workflow, gate runner and `verify-fast.sh`.
22. Make `setup-repo.sh` the sole bootstrap.
23. Add a report-first `doctor.sh`.
24. Require all supported agents and humans to run the same fast tier.
25. Time-box the fast tier by moving heavy gates to the correct tier—not by deleting them.
26. Verify everything from a fresh clone.
27. Run `verify-fast.sh` twice to prove idempotence.
28. Publish the before/after working-copy, clone/object and PCK size receipt.

---

# 31. Final Outcome

When this plan is complete, the ASHFALL repository stops lying through its filesystem.

The root contains active entrypoints, not six-week-old patch scripts and ancient Unity test results. Historical artifacts still exist, but in an archive that clearly says what they are. Design mockups remain available as visual references for live or shelved panels, but they no longer masquerade as shippable game assets or inflate every export.

Runtime assets become governed. Art, UI, sprites, fonts and audio each have a size baseline, LFS policy, import preset, packed-size expectation and growth tolerance. Duplicate bytes are visible. Store-facing art lives outside the PCK. The asset gallery reflects actual provenance and use.

Generated output becomes predictable. Golden snapshots are tracked; captures are ephemeral. Test results and semantic-review output have named owners and regeneration commands. Maintenance scripts have lifecycles instead of living forever.

The tooling environment converges too. `AGENTS.md` remains the source for supported client rulebooks. Machine-local caches stay local. `setup-repo.sh` creates the expected repository state. `doctor.sh` explains what is wrong instead of relying on tribal knowledge. CI, the gate manifest and `verify-fast.sh` describe the same required checks.

Most importantly, the plan ends with numbers:
- how much lighter the working copy became;
- how much smaller the clone/object store became;
- how much smaller the exported PCK became;
- how long import and fast CI take;
- which asset families grew or shrank.

The result is not cosmetic cleanup.

It is a repository that stops wasting disk, shipping dead design weight, hiding configuration drift, and encouraging agents or contributors to run obsolete tooling by accident.
