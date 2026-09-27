# Wave 0 local inventory: latest branch salvage

Status: inventory only, not a carrier or a merge-ready package. Measured 2026-09-27.

## Refs and scope

- Integration branch: `integration/all-latest-2026-09-24` at `4ab1891e16d135decdb724f336224b7c068363f2`; clean working tree before this inventory.
- Cached `origin/main`: `edf815506bb9e0aed6f9335dd1b3e27390d92267`.
- Local `main`: `b8363cb7deb8e8f8e9410f5fa400c8b9bf171327`, **32 commits behind cached `origin/main`**. Never use local `main` as the carrier base without refreshing/validating it.
- Relative to cached `origin/main`, integration is **64 branch-only commits ahead and 18 main-only commits behind**. The cited PR #70 head `1a3e6fee…` is older than this local HEAD.
- The merge base is `d13e9322db959017312f7f9688eecb04213a127d` (2026-09-19). The 64 branch-only commits are linear (no branch-side merge commits).
- Six newest branch commits are only local, not on its tracked upstream: `6e5ce4f2b`, `d5ca23fff`, `5dc8bb1d7`, `3dfeb4cf5`, `7837f3fae`, `4ab1891e1`. Keep this working copy safe until a separate backup/publish decision is made.
- Remote `main`, PR #70 mergeability, Qlty and workflow status were **not refreshed**. No fetch, checkout, PR mutation, push or history rewrite was performed.
- Separate unstaged changes appeared in `Assets/Ashfall.Core/Inventory/ClothingWarmthSystem.cs` and `src/Host/ClothingWarmthHostSession.cs` during this inventory. They were not modified, staged or reviewed here. Preserve them; the checkout is no longer clean for a branch switch. This status is a snapshot, not a promise that the concurrent lane is finished.

## Tracked-tree size (raw blob sizes, not Git pack transfer sizes)

| Ref/family | Files | Raw size |
|---|---:|---:|
| Current integration HEAD, all | 22,015 | 27.205 GiB |
| Current integration HEAD, Markdown | 5,583 | 27.015 GiB |
| Cached `origin/main`, all | 15,525 | 0.142 GiB |
| `docs/expansions/prose_wave*` | 2,041 | 15.664 GiB |
| `docs/plans/EXPANSION_PROGRAM*` | 526 | 4.564 GiB |
| Other `docs/` (not plans) | 2,550 | 3,528.22 MiB |
| Other `docs/plans/` (not expansion/integrated) | 317 | 3,124.63 MiB |
| `docs/plans/integrated/` | 166 | 200.46 MiB |
| `scripts/tools/expand_oldest_*` | 231 | 47.34 MiB |

3,064 tracked Markdown files exceed **2 MiB** (26.850 GiB total); 1,906 are at least **10 MiB**. These are not the decimal-MB cutoffs used by the historical audit. Largest tracked Markdown is 11,476,013 bytes.

## Preliminary path classification (merge base to local HEAD)

`git diff --name-only --no-renames origin/main...HEAD` reports **10,172 changed paths**:

| Candidate class | Changed paths | Selection status |
|---|---:|---|
| Core runtime | 362 | Candidate only, not individually accepted |
| Godot host/UI | 783 | Candidate only, not individually accepted |
| Focused tests | 436 | Candidate only, not individually accepted |
| Runtime data | 98 | Candidate only, not individually accepted |
| Godot assets | 3,097 | Review source/sidecar pairs and real usage |
| Agent plans/state | 142 | Preserve relevant decision evidence |
| Other docs | 1,522 | Separate compact authority from generated prose |
| Expansion-plan docs | 526 | Candidate external archive, not blanket deletion |
| Generated prose docs | 2,041 | Candidate external archive, not blanket deletion |
| One-off expansion scripts | 231 | Preserve provenance/reproducibility before consolidation |
| Other tooling | 551 | Inspect individually |
| Other/root | 383 | Inspect individually |

Additional check: 361 Core C#, 782 host C#, 433 test C# and 98 runtime JSON paths changed. The `other/root` class includes planning folders such as `piagentsplans` and `Next-steps-plans`; it is not safe to equate this class with runtime code.

## Branch-only commit classification

Every branch-only commit belongs to one **dominant** class below (64 total). These are selection hints, not permissions to transplant a whole commit. `--no-renames` counts 10,172 changed paths above; rename-aware analysis counts 10,170.

| Dominant class | Count | Commits (abbreviated, chronological selection still required) |
|---|---:|---|
| Prose-led | 28 | `b1a8d08d1`, `126319e9c`, `9fa5c9eeb`, `32775d8d3`, `012c4b165`, `a70102802`, `ed931a729`, `bad68e9d3`, `4d8c9b347`, `546459241`, `1678c0749`, `b397f997a`, `8362a5174`, `bb3be024e`, `fd737864a`, `baaa2ab2b`, `c2098db03`, `e555cd161`, `58f3dd8ea`, `db1d277ea`, `9988eea80`, `ddbb06009`, `18f168dfc`, `b48947413`, `4fc95f3d8`, `895961264`, `7bc553ec9`, `d3b3d484e` |
| Mixed mega-checkpoints, **never pick whole** | 4 | `c8c1e453d` (3,811 paths), `2acb62fdf` (3,591), `a3a938868` (2,468), `e7a9ec23f` (841) |
| Feature work bundled with large prose | 7 | `1bcffbd8f`, `3382735e8`, `2549b37d4`, `717cf1cd6`, `02eaeafed`, `00d07d6b6`, `729156094` |
| Runtime/host-heavy work | 18 | `4ab1891e1`, `7837f3fae`, `d5ca23fff`, `6e5ce4f2b`, `40263e98e`, `05e252933`, `12a5bbab4`, `5be1a30a6`, `87b805726`, `1c8fd9857`, `cf70de702`, `41ecf77e7`, `a89f431f0`, `c76652926`, `0878a6c70`, `5dc8bb1d7`, `1a3e6fee1`, `d0a086a5d` |
| Tests only | 2 | `4a34d9e0f`, `bcc3642b1` |
| Host-only | 1 | `6b19afe04` |
| Assets/generated catalog mixed | 3 | `5741ec45e`, `3bc02bb08`, `17ad5aef7` |
| Agent-plan archival | 1 | `3dfeb4cf5` |

**Selective-port risks:** 29 branch commits touch `src/`, 24 touch Core, 29 touch tests, and 8 touch runtime data. The mixed checkpoints contain both useful runtime code and bulk prose, and `2acb62fdf` also contains the canonical `tools/gotools` toolchain and committed `dist/` zips. `c8c1e453d` contains hundreds of one-off generator files. Cherry-pick only after reviewing each path and its dependencies; do not use a prefix-only filter as an acceptance oracle.

**Main-only reconciliation:** the 18 cached `origin/main` commits comprise 6 merges and 12 substantive commits:

- Governance/catalog/gates: `5abcc6752`, `8b2116ff2`, `5c8dd3c7b`, `561c65efa`, `065c9d7c0`, `4c286683b`.
- Plan 220/205 and save/overlay work: `a2d5a4256`, `c66185520`, `d21de5fc8`, `05fe533e7`.
- UI overlay detection: `3129519fe`.
- Hygiene: `aa0c82cda`.
- Merges: `c7cb3e212`, `7cd897f58`, `c67e17ecb`, `0acb50ac7`, `ff1efce8a`, `edf815506`.

Branch commit `41ecf77e7` and main commit `a2d5a4256` deliver nearly identical Plan 220/205 code. Do not duplicate their code; main also has extra generated-catalog work. Forty-five changed paths overlap between the two sides, including `SaveSectionRegistry`, `HostCliRegistry`, host CLI/dispatch, `Main.GameFlow`, `Main.PanelLifecycle`, save tests and generated manifests. Fifty-one branch commits touch at least one overlap path. Regenerate generator-owned outputs on a carrier rather than hand-merge them.

**Confirmed main-only regression risk:** cached `origin/main` contains `_shelterAtmospherePanel` in both `Main.GameFlow.cs` and `Main.PanelLifecycle.cs`, but the integration HEAD has no reference in either method. Its `Main.ShelterAtmosphere.cs` still opens that panel with raw `Visible = true`. Do not overwrite main's close/Esc registration during salvage.

## Dependency and retention constraints

- `INTEGRATION_PLANS.md` is the live ledger; `KNOWN_DEBT.md` marks historical plan sprawl accepted. The older `AGENTS.md` queue is duplicated/stale.
- No direct path references to `docs/expansions/prose_wave*`, `docs/plans/EXPANSION_PROGRAM*` or `expand_oldest_*` were found in `src/`, `Assets/Ashfall.Core/`, focused tests, `scripts/ci/`, or `.github/` by exact-family search. This is **not** proof that individual plans are dispensable or lack links from other documents.
- `scripts/ci/generate-docs-index.py` enumerates **all** Markdown and reads each document. `docs/INDEX.md` links to them, and `.github/workflows/docs-regen.yml` checks index drift. Excluding corpus files requires a deliberate index/link policy change and a valid archive-location/manifest contract; do not merely delete files or weaken gates.
- No archive destination, archive credentials, or external retention policy has been approved. Before any exclusion, establish a recoverable archive and per-file logical ID, SHA-256, generator/version, authority status, and archive pointer.
- Existing lifecycle work at this HEAD already fixes Muster and Standing Atlas repeated binds (named handler, old-host detach and null-on-unbind); Gate 22 of `PanelBindLifecycleSelfTest` covers 25 repeat binds and host switch. Do not reapply the older audit's suggested patch.
- Plan 146–149 and five `Main.World.cs` direct opens still bypass the shared lifecycle open helper; they are future packages, not Wave 0 carrier-selection work.

## Wave 0 disposition

**Local inventory complete; Wave 0 carrier work not started.** Preserve PR #70 / current integration history as evidence. No carrier branch, selective port, archive, deletion, merge, remote update or status fix has occurred. Current local `main` is not the remote main. The newly dirty worktree must be preserved. A fresh remote read/fetch, reviewed archive/size policy, detailed cross-commit dependency mapping, and conflict-aware selection against current `main` are prerequisites to the carrier.

Reproduce local counts without reading the prose bodies:

```text
git rev-list --left-right --count origin/main...HEAD
git log --reverse --format='%H %ad %s' --date=short origin/main..HEAD
git log --reverse --format='@@ %H %s' --name-status origin/main..HEAD
git ls-tree -r -l HEAD
git diff --name-only --no-renames origin/main...HEAD
git diff --name-status d13e9322db959017312f7f9688eecb04213a127d origin/main
```
