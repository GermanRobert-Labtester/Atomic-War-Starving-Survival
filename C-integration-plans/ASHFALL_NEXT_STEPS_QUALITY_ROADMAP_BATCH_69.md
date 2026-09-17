# ASHFALL — Quality Roadmap Batch 69

## Theme: Dead Code Elimination & Binary Size Reduction

**Priority:** MEDIUM (maintainability + faster builds)
**Risk:** Medium — removing code that might be referenced but appears unused; requires careful verification at each step
**Estimated effort:** 3–4 sessions (reduced from 4–6: Steps 3 and much of Step 4 as originally scoped are no-ops against the current tree — see Review Notes)
**Prerequisites:** All verification steps pass before starting (baseline green)

---

## Motivation

The project completed the Unity-to-Godot migration: `Assets/_Game/` and `src/Bridge/` are **fully deleted from the working tree** (verified — see Review Notes). What remains as real debt is different from what a "Unity cleanup" pass would assume:

- `ProjectSettings/`, `Packages/`, and `.asset` files under `ProjectSettings/` **still exist on disk** and are still referenced by `.gitattributes` unity-yaml rules — they were never part of the `_Game/` deletion and are a separate, unresolved question (does the Godot project still need a Unity project shell alongside it, or is this legacy debris from before the migration completed? — needs an explicit owner decision, not a mechanical deletion).
- `unity-assets-archive-2026-08-14.tar.gz` (146 MB, confirmed via `ls -la`) is **already untracked and gitignored** (commit `d03dd555`, "fix(cli): wire dead selftest verbs, drop Unity archive, sync AGENTS.md" — `git rm --cached` + `.gitignore` entry `/unity-assets-archive-*.tar.gz`). It is not "in git history requiring filter-branch" as a live problem; it is a stray local file on disk that can simply be deleted if the operator no longer needs the local backup.
- `scripts/` is **not empty** and **not orphaned**, but the reason is not what an earlier draft of this document claimed. `Ashfall.csproj` does have `<Compile Include="scripts/**/*.cs" .../>`, but `scripts/` on disk today (re-verified via `find scripts -iname "*.cs"`) contains **zero** `.cs` files — only Python (`audit_assets.py`, `composio_asset_pipeline.py`, `generate_item_icons.py`, `pipeline/`) and `scripts/ci/` shell scripts. The compile-include glob therefore matches nothing and compiles nothing right now; calling it "actively compiled" overstated the mechanism. The correct reason `scripts/` is not dead: it holds real, in-use Python tooling that other project surfaces depend on (`scripts/ci/godot-asset-gate.sh` is invoked by AGENTS.md's own asset-verification instructions). There is nothing to remove here, but for a build-dependency reason, not a compile-dependency one — if a future contributor sees the empty `.cs` glob and reasonably assumes it's dead weight in the `.csproj`, that assumption would be correct about the glob (it matches nothing) but wrong about the directory (it is not dead).
- `GameBootstrap` (Unity `MonoBehaviour` bootstrap, and the referenced `GameBootstrap.Phase0Expansion.cs` partial) **does not exist anywhere in the active tree**. The only `GameBootstrap.cs` in the repository is under `_quarantine_legacy/Assets/Scripts/Core/GameBootstrap.cs`, which is itself `.gitignore`'d (`/_quarantine_legacy/`) and therefore already excluded from version control — not a cleanup target, just legacy scratch space the operator chose to keep locally.

Given this, Batch 69 is rescoped around what actually remains: unreferenced Core systems/host sessions, stale documentation pointing at deleted paths, and the genuinely unresolved `ProjectSettings/`/`Packages/` question. Steps below are renumbered and rewritten accordingly; audit-only steps (1–2) are unchanged in spirit but corrected in their "known suspects."

---

## Step 1 — Identify Unreachable Code in Core (Systems Never Instantiated by Any Host)

**Goal:** Find Core systems that are defined but never constructed or referenced by any live host (`src/Main.cs`, Godot sessions in `src/Host/`), indicating they are dead code candidates.

**Implementation:**
- Write a discovery script (`tools/dead-code-audit.sh` or a Roslyn-based tool — confirmed: no such script exists yet, `tools/` currently holds only asset/audio generation and audit Python scripts):
  - Parse all `.cs` files in `Assets/Ashfall.Core/` to extract class names.
  - Search all `.cs` files in `src/` (Godot host) for instantiation (`new ClassName(`, `= new ClassName`) or type references.
  - Cross-reference with `src/Main.cs` setup methods — **confirmed count: 37 `private void SetupXxx()` methods** (re-verified via `grep -c "private void Setup[A-Za-z]*(" src/Main.cs` during this review pass — an earlier pass in this same document had miscounted this as 38; the earlier "38" figure was itself an error, not just AGENTS.md's "31" being stale), not 31 as previously assumed. Recount before writing the report; do not hardcode either number into tooling.
  - Cross-reference with `src/Main.cs` flush methods — **confirmed count: 16 `private void FlushXxxIfDirty()` methods** (AGENTS.md's H7 says 17; treat AGENTS.md counts as approximate and re-derive from source, don't copy stale numbers forward).
  - Cross-reference with test files in `Ashfall.Core.Tests/` (a system exercised only by tests is still alive).
  - Output a report: `docs/dead-code-candidates.md` listing:
    - Systems with zero host references and zero test references (HIGH confidence dead).
    - Systems with test references only (MEDIUM confidence — may be future-planned).
    - Systems referenced only by other unreachable systems (transitive dead).
- Known suspects to investigate (corrected):
  - There is **no** "Phase 11" stub list in `GameBootstrap.Phase0Expansion.cs` — that file does not exist in the active tree. If AGENTS.md's H1-adjacent stub note ("six systems constructed/registered/ticked but key effects are stubs") is still accurate, locate the equivalent in the current Godot host (search `src/Main.cs` and `src/Host/ExpansionHostSession.cs` for `// TODO` / "stub" / "wired in Phase" comments) rather than assuming the old file path.
  - Any Core class with **zero** references outside its own file and outside `Ashfall.Core.Tests/` — this is the actually-reliable signal, independent of what got deleted from `_Game/`.
  - `ExpansionMasterSession` sub-systems for expansions 03/04 — verify current wiring in `src/Host/ExpansionHostSession.cs` before assuming these are unwired; do not carry forward an assumption from before the migration completed.
  - `Ashfall.csproj`'s own `<Compile Include="scripts/**/*.cs" .../>` line (confirmed via this review: `scripts/` currently contains zero `.cs` files, so this glob matches nothing) — this is a small, genuinely new, and very low-risk dead-code finding this review pass surfaced that no prior draft of this batch had flagged as an actual audit target: a no-op include line in build configuration itself. It costs nothing to leave (an empty glob does not slow builds meaningfully), so treat it as a LOW-confidence/no-action item in the report rather than a removal candidate — flag it for completeness, don't spend a removal step on it.

**Verification:**
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
# Confirm: this step is audit-only, no code removed yet
# Confirm: docs/dead-code-candidates.md exists with categorized results
```

**Done when:**
- `docs/dead-code-candidates.md` lists all unreachable systems with confidence levels, each with: class name, file path, reason for classification, and the exact grep/search command used to reach that conclusion (so the classification is independently reproducible, not just asserted).
- No code has been modified or deleted in this step.
- The report is reviewed by a human before proceeding to removals in later steps.

---

## Step 2 — Identify Unwired Host Sessions

**Goal:** Find host sessions in `src/Host/` that are defined but never instantiated or called from `src/Main.cs` or any other live entry point.

**Implementation:**
- Enumerate all classes in `src/Host/` that end in `Session`, `HostSession`, or `RuntimeSession`.
- For each, search `src/Main.cs` for:
  - Field declarations of that type.
  - Constructor calls (`new XxxSession(`).
  - Method calls on instances of that type.
  - References in `SetupXxx`, `SaveXxx`, or `FlushXxxIfDirty` methods.
- Cross-reference with Godot scene files (`.tscn`) in case sessions are instantiated via scene tree.
- Produce a table in `docs/dead-code-candidates.md` (append to Step 1 output):

  | Session Class | File | Referenced in Main.cs | Referenced in .tscn | Verdict |
  |---|---|---|---|---|
  | `ExampleHostSession` | `src/Host/ExampleHostSession.cs` | No | No | DEAD |

- **Correction to the previous "known suspect":** `HoldfastRuntimeSession` is **not** orphaned — it is actively constructed and ticked in `src/Main.cs` (`_holdfastRuntime = HoldfastRuntimeSession.Create(_core);` and `_holdfastRuntime.TickDay()` inside `TickSimDay`, plus multiple `new HoldfastRuntimeSession(...)` call sites in self-test methods). AGENTS.md's H1 flags it as duplicating core survival mechanics, which is a **design/architecture concern** (should this logic live in `Ashfall.Core` instead of `src/Host/`?), not a dead-code concern. Do not classify it as DEAD; if anything, file it as a candidate for the *separate* "migrate host logic into Core" work (Invariant 5), not this batch.
  - Actually walk `src/Host/*.cs` for the real unwired candidates instead of assuming — there are dozens of `*HostSession.cs` and `*SaveStore.cs` files (e.g. `CombatHostSession.cs`, `CraftingHostSession.cs`, `DeepCoastHostSession.cs`, `DoseLedgerHostSession.cs`, `DutyRosterHostSession.cs`, `EconomyHostSession.cs`, `ExpansionHostSession.cs`); each needs its own grep against `src/Main.cs`, not a single named suspect.

**Verification:**
```bash
dotnet build Ashfall.csproj
# Confirm: build still succeeds (audit only, no removals)
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
# Confirm: all tests pass
# Review docs/dead-code-candidates.md for session audit section
```

**Done when:**
- All host sessions are cataloged with their wiring status, verified against the current `src/Main.cs`, not against assumptions carried over from AGENTS.md.
- Dead sessions are flagged with rationale and the exact search performed.
- No code removed yet — this is the investigation phase.

---

## Step 3 — Resolve the `ProjectSettings/` / `Packages/` Question (was: "Remove Unity Configuration Artifacts")

**Goal:** Originally this step assumed removing leftover Unity project config after the `_Game/` deletion. That assumption is only half right: `Assets/_Game/` is gone, but the Unity **project-level** files (`ProjectSettings/*.asset`, `Packages/manifest.json`, `Packages/packages-lock.json`) are still present on disk and still referenced as "still relevant" by `.gitattributes`'s own maintenance note. This step must start by determining **whether these are still needed for anything**, not assume they aren't.

**Implementation:**
- Confirm current state (already verified during this review):
  - `ProjectSettings/` exists with 29 top-level entries (re-verified via `ls -la ProjectSettings/` during this review pass): 27 `.asset`/config files (`AudioManager.asset`, `InputManager.asset`, `TagManager.asset`, `TimeManager.asset`, `QualitySettings.asset`, `Physics2DSettings.asset`, `GraphicsSettings.asset`, `EditorSettings.asset`, `EditorBuildSettings.asset`, `URPProjectSettings.asset`, `XRSettings.asset`, etc.), one empty `.gdignore`, and — not previously flagged — a **second, nested `ProjectSettings/Packages/` directory** distinct from the top-level `Packages/` at the repo root. Both `Packages/` locations exist and are both git-tracked; do not conflate them when deciding what Step 3 would remove.
  - `Packages/manifest.json` and `Packages/packages-lock.json` exist.
  - `.gitattributes` contains a 2026-08 maintenance note stating LFS is actively in use and that `assets/` (lowercase, Godot) is distinct from `Assets/` (uppercase, Unity legacy) via `core.ignorecase=false` — this note does **not** say `ProjectSettings/`/`Packages/` are dead, it's about the LFS/case-sensitivity split.
- Ask the actual question before deleting anything: is there still a Unity Editor project at the repo root that some contributor opens (even read-only, e.g. to re-export an asset), or is `ProjectSettings/`/`Packages/` truly vestigial now that `Assets/_Game/` is gone? **This requires a human answer, not a mechanical "it's Unity, migration is done, delete it" pass** — AGENTS.md's own STACK table still lists a Unity host row as "(inactive — migrating out)," implying the Unity project shell may still be intentionally kept around for reference/export during the tail of the migration.
- If and only if the owner confirms the Unity project shell is no longer needed at all:
  - Remove `ProjectSettings/`, `Packages/`.
  - Remove any stray `.asmdef` files outside `Assets/Ashfall.Core/Ashfall.Core.asmdef` (which must be preserved — it's still used by `dotnet build` via the `.csproj`).
  - Update `.gitattributes` to drop the now-truly-obsolete Unity YAML/asset rules (`*.unity`, `*.prefab`, `*.mat`, `*.controller`, `*.anim`, etc.) — but only the ones for extensions that no longer appear anywhere in the tree; re-run the check in Step 6 below before deleting rules.
- Preserve regardless of the above decision:
  - `Assets/Ashfall.Core/` — the Core library (still active).
  - `Assets/StreamingAssets/Data/` — the data authority (still active).
  - `assets/` (lowercase) — the Godot-native asset tree, entirely unrelated to this question.

**Verification:**
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
# Confirm: all 5 verification steps pass
# Confirm: removed files (if any were removed) do not break any import or reference
git status  # review what was removed
```

**Done when:**
- The owner has made an explicit decision on `ProjectSettings/`/`Packages/` (keep or remove), recorded in the PR/commit description.
- If removal was approved: files are removed, `.gitattributes` obsolete rules are dropped, no references to removed files exist in remaining code, all 5 verification steps pass.
- If removal was **not** approved: this step is marked "deferred, not applicable this batch" and Batch 69's exit criteria is amended to drop this line item — do not silently skip it without recording the decision.

**Risk / rollback:** Medium, not Low — unlike the rest of this batch, deleting `ProjectSettings/`/`Packages/` is only safely reversible via `git checkout` if the files are still tracked in git; confirm with `git ls-files ProjectSettings/ Packages/` before deleting, and take a local tar backup first if they are tracked and this is the first time they're being removed.

---

## Step 4 — Clean Up AGENTS.md and REPO_REVIEW_REPORT.md Stale References

**Goal:** Update project documentation to reflect the current state of the codebase — remove references to deleted `Assets/_Game/` paths, resolved issues, and defunct systems. **Confirmed: both files exist** (`AGENTS.md`, 30055 bytes; `REPO_REVIEW_REPORT.md`, 17938 bytes) — this step is real and actionable, unlike Step 3.

**Implementation:**
- In `AGENTS.md`:
  - Review the "Known Issues" tables — several items are already marked `RESOLVED` inline (e.g. C2, C3, C4, C5, H9); confirm these are still accurate and haven't regressed rather than re-verifying from scratch.
  - Grep for `Assets/_Game/` — it appears throughout AGENTS.md, almost entirely in **historical/context sections** describing what was migrated (e.g. Invariant 3, Invariant 5 offender list, C1/C6 known-issues rows, asset migration table). These are legitimate historical/status references, not stale action items pointing at files that should exist — do not delete them wholesale; only flag entries that are phrased as *current, actionable* work against a path that's actually gone (there do not appear to be any such entries as of this review — the ones present already say things like "MISSING," "do not extend," or describe pre-migration state).
  - Update the "STACK" table's Unity host row if its status has changed since last edit (currently: `Assets/_Game/ (read-only legacy)` / "Unity 6 (do not run)" — this remains accurate as a description of the *type* row even though the directory contents are gone, since the row also documents the namespace/target that would apply if it existed).
  - Re-verify the "Invariant 5" offender list against the current tree — it lists `Assets/_Game/Quests/PersonalQuestSystem.cs`, `Assets/_Game/Medical/MedicalSystem.cs`, `Assets/_Game/Survivors/SurvivorWorkShiftSystem.cs`, `Assets/_Game/Economy/DynamicEconomySystem.cs` as "known offenders — do not grow these." **These files no longer exist** (confirmed: `Assets/_Game/` is deleted). This list must be corrected: either removed entirely (if the concern is now moot because the files are gone) or replaced with any *current* Core-layer/host-layer file that duplicates gameplay logic outside `Ashfall.Core` (candidate: `src/Host/HoldfastRuntimeSession.cs`, already listed separately as H1 — check for overlap before duplicating the entry).
  - Recount `SetupXxx`/`SaveXxx`/`FlushXxxIfDirty` in H7 against the real numbers from Step 1 of this batch (confirmed during this review: 37 Setup, 29 Save-prefixed methods including `SaveAll`, 16 Flush — AGENTS.md currently says 31/24/17).
- In `REPO_REVIEW_REPORT.md`:
  - Read it in full and cross-check each entry against the current tree the same way Step 1/2 do for code — do not assume its contents without reading (this file was not read as part of this review pass; a follow-up task should do so before editing it).
  - Remove or archive entries that reference deleted code.
  - Update issue counts to reflect current state.
- Create a commit with message: `docs: correct stale _Game/ file references and recount Main.cs method totals`

**Verification:**
```bash
# Confirm: every literal file path referenced in AGENTS.md's actionable (non-historical) sections resolves
grep -n "Assets/_Game/" AGENTS.md
# Manually classify each hit: historical/context (fine) vs. actionable path claim (must point at a real file)
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```
Note: the original verification command (`grep -c "Assets/_Game/" AGENTS.md`, asserting the count trends toward zero) is the wrong check — most matches are intentionally historical and should stay. Use a manual review pass, not a bare count assertion.

**Done when:**
- The Invariant 5 offender file list no longer cites nonexistent `Assets/_Game/` paths as current, actionable offenders.
- `SetupXxx`/`SaveXxx`/`FlushXxxIfDirty` counts in H7 match the real counts as of the commit.
- `REPO_REVIEW_REPORT.md` has been read and reconciled against the current codebase state (not assumed).
- Documentation is internally consistent — historical narrative is preserved, actionable claims are accurate.

---

## Step 5 — Delete the Stray Local Unity Archive (was: covered under Step 7's "Measure Binary Size")

**Goal:** `unity-assets-archive-2026-08-14.tar.gz` (146 MB, plus its `.sha256` sidecar) sits untracked and gitignored in the working tree root. It is not a git-history problem (already resolved in commit `d03dd555` via `git rm --cached`); it is purely a local disk-space question for whoever is running this batch.

**Implementation:**
- Confirm the file's status before touching it:
  ```bash
  git status --porcelain --ignored=matching -- unity-assets-archive-2026-08-14.tar.gz
  # Expect: "!!" prefix (ignored, untracked) — confirms it is not part of the commit history going forward
  git log --all --oneline -- unity-assets-archive-2026-08-14.tar.gz
  # Expect: shows the historical add + the d03dd555 removal-from-tracking commit; the blob remains in old
  # commits' history regardless of what happens to the working-tree file — that is a separate, explicitly
  # out-of-scope history-rewrite question (BFG/filter-branch), not resolved by deleting the local copy.
  ```
- If the operator confirms the local backup copy is no longer needed (e.g. it has been archived elsewhere, or the migration is confirmed complete and no rollback is anticipated), delete the two files:
  ```bash
  rm unity-assets-archive-2026-08-14.tar.gz unity-assets-archive-2026-08-14.tar.gz.sha256
  ```
- This has **zero effect on repository size** (`git count-objects`, `du -sh .git/`) since the file was never tracked going forward — it only frees local disk space. Do not report this as a "repo size win" in Step 7's report; report it separately as a local disk cleanup.
- The git-history blob (140 MB per the original commit message, though the current working-tree copy measures 146 MB — these are not necessarily the same bytes; do not assume the historical blob size from the current file's `ls -la` output) remains a genuine future consideration for BFG/`git filter-branch`, exactly as the original plan noted. That part of the original plan's guidance was correct and is preserved here.

**Verification:**
```bash
ls -la unity-assets-archive-2026-08-14.tar.gz* 2>&1  # should report "No such file" after deletion
git status --short  # should show no change (file was untracked/ignored, so its removal is invisible to git)
```

**Done when:**
- The operator has explicitly decided whether to keep or delete the local archive copy, and that decision is recorded.
- If deleted, both the `.tar.gz` and `.sha256` sidecar are gone and `git status` shows no diff (proving it was correctly untracked).
- The distinction between "local file deleted" and "git history rewritten" is documented — this step does the former only.

---

## Step 6 — Audit .gitattributes for Obsolete Patterns

**Goal:** Remove `.gitattributes` rules that reference file types or paths no longer present in the repository, and ensure LFS tracking is correct for remaining binary assets. **Correction: this step's premise needs adjusting** — as of this review, `.gitattributes` already contains a maintainer-written note (dated 2026-08) explicitly stating LFS is in active use and clarifying the audio-exception policy. Treat that note as authoritative and current; this step is about the *Unity-specific* rules only, and those are conditional on Step 3's outcome.

**Implementation:**
- Read current `.gitattributes` and categorize each Unity-specific rule:
  - **Still relevant if Step 3 keeps `ProjectSettings/`/`Packages/`:** `*.asset` (unity-yaml) is actively matched by files in `ProjectSettings/` right now (27 `.asset`/config files, plus a nested `ProjectSettings/Packages/` dir — 29 top-level entries total) — do not remove this rule unless Step 3 actually deletes those files first. Removing it prematurely would silently change how `ProjectSettings/*.asset` diffs/merges in git while the files still exist.
  - **Confirmed obsolete regardless of Step 3** (no matching files anywhere in the tree — verified via `find . -iname "*.<ext>"` for each): `*.unity`, `*.prefab`, `*.controller`, `*.mat`, `*.anim`, `*.overrideController`, `*.playable`, `*.spriteatlas`, `*.spriteatlasv2`, `*.terrainlayer`, `*.mask`, `*.brush`, `*.guiskin`, `*.lighting`, `*.giparams`, `*.mixer`, `*.renderTexture`, `*.shadervariants`, `*.signal`, `*.scenetemplate`, `*.physicMaterial`, `*.physicsMaterial2D`, `*.flare`, `*.fontsettings`, `*.preset`.
  - **Questionable — re-verify, do not assume:** `*.asmdef`/`*.asmref` are matched by `Assets/Ashfall.Core/Ashfall.Core.asmdef`, which is explicitly preserved per AGENTS.md — keep this rule.
- Remove only the confirmed-obsolete patterns (the list above), leaving `*.asset`/`.meta`/`.asmdef` rules untouched pending Step 3's resolution.
- Verify LFS is still tracking what it should:
  ```bash
  git lfs ls-files | wc -l   # confirmed baseline as of this review: 5064 files tracked
  git lfs ls-files | head -20
  ```
- Confirm audio files (`.wav`, `.mp3`, `.ogg`, plus `.aif`/`.aiff` per the existing note) remain plain binary per the `.gitattributes` policy already documented inline — this is already correct, this step should only verify it, not change it.

**Verification:**
```bash
git lfs ls-files | wc -l
# Confirm: LFS file count is unchanged for legitimate assets (baseline: 5064)
git check-attr -a -- assets/art/*.png 2>/dev/null | head -5
# Confirm: PNG files still tracked by LFS
git check-attr -a -- assets/audio/*.wav 2>/dev/null | head -5
# Confirm: audio files are NOT LFS (plain binary per policy)
dotnet build Ashfall.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

**Done when:**
- `.gitattributes` contains only rules for file types that exist in the repository, OR rules that are conditionally kept pending Step 3.
- LFS tracking is unchanged: confirmed baseline 5064 files before this step; recheck the same count after.
- No Unity-specific patterns remain for extensions confirmed absent from the entire tree.
- Repository operations (`git status`, `git add`, `git lfs`) behave correctly — spot check with a no-op `git add -A --dry-run` after the change, not just a build.

---

## Step 7 — Measure and Report Repository/Build Size

**Goal:** Quantify the impact of Steps 1–6 on repository size, build output size, and (separately) local disk usage. Establish a baseline for ongoing monitoring. Renamed from "Binary Size Reduction Report" because most of this batch's real changes (Steps 1, 2, 4) don't touch binaries at all — calling the whole batch a "binary size reduction" overstates what it does.

**Implementation:**
- Measure BEFORE (record at start of batch, before any removals):
  - `git count-objects -vH` — confirmed current baseline: `size-pack: 53.78 MiB`, 28688 objects in-pack, 7 packs.
  - `du -sh .git/`.
  - `du -sh Assets/ src/ assets/`.
  - `dotnet build Ashfall.csproj && du -sh bin/ obj/` (if a `bin/`/`obj/` output directory is produced at repo root by this SDK-style build — confirm the actual output path first, Godot.NET.Sdk builds may place output elsewhere; do not assume `bin/`/`obj/` exist at root without checking).
  - `git lfs ls-files | wc -l` — confirmed current baseline: 5064.
  - Local disk usage of the untracked archive: `du -sh unity-assets-archive-2026-08-14.tar.gz` (this is a *local* number, will vary per machine, and must be reported separately from repo-size numbers — see Step 5).
- Measure AFTER (after all removals from Steps 1–6 are committed):
  - Same measurements as above, same machine, same clone if possible (measurements will differ across clones due to local `.git/` pack state).
- Produce `docs/size-reduction-report.md`:
  ```markdown
  # Repository & Build Size Report — Batch 69

  | Metric | Before | After | Delta | % Change |
  |--------|--------|-------|-------|----------|
  | .git/ size | X MB | Y MB | -Z MB | -N% |
  | Assets/ size | X MB | Y MB | -Z MB | -N% |
  | Build output | X MB | Y MB | -Z MB | -N% |
  | LFS objects | 5064 | M | -K | -J% |

  ## Local-only (not repo size, not reproducible across machines)
  | Metric | Before | After |
  |--------|--------|-------|
  | unity-assets-archive-2026-08-14.tar.gz on disk | 146 MB | 0 MB (if deleted per Step 5) |
  ```
- Note: the git-history blob for the Unity archive (added before commit `d03dd555`) is still in `.git`'s pack history and cannot be removed without `git filter-branch` or BFG. This is unchanged from the original plan's guidance and remains correct — document it as a future consideration, do NOT perform history rewriting in this batch (high risk, requires team coordination and a force-push that rewrites shared history).
- Recommend next actions: LFS migration for any remaining large binaries found outside LFS, and a scheduled future maintenance window for the history rewrite.

**Verification:**
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
# Confirm: all 5 verification steps pass after all removals
# Confirm: docs/size-reduction-report.md contains before/after measurements
```

**Done when:**
- `docs/size-reduction-report.md` exists with complete before/after measurements, with repo-size and local-disk numbers kept in clearly separate tables.
- All removals from Steps 1–6 (whichever were actually approved/performed) are committed and verified.
- Recommendations for further reduction are documented.
- All 5 verification steps pass on the final state.

---

## Summary Table

| Step | Title | Risk | Removes Code | Output |
|------|-------|------|--------------|--------|
| 1 | Identify unreachable Core systems | None (audit) | No | `docs/dead-code-candidates.md` |
| 2 | Identify unwired host sessions | None (audit) | No | Appended to dead-code report |
| 3 | Resolve `ProjectSettings/`/`Packages/` question | **Medium** — requires owner decision, files may still be needed | Conditional — only if approved | Decision recorded; files removed only if approved |
| 4 | Clean up stale doc references | None | No code | Corrected `AGENTS.md`, reconciled `REPO_REVIEW_REPORT.md` |
| 5 | Delete stray local Unity archive | Low — local disk only, git-history unaffected | No (not a git object) | 146 MB freed locally, decision recorded |
| 6 | Audit .gitattributes | Low | Patterns only, conditional on Step 3 | Cleaned `.gitattributes` |
| 7 | Measure repo/build size | None (report) | No | `docs/size-reduction-report.md` |

---

## Risk Mitigation

| Risk | Mitigation |
|------|-----------|
| Removing code that is actually referenced | Steps 1–2 are audit-only; removals happen only after review and full verification |
| Deleting `ProjectSettings/`/`Packages/` while still needed | Step 3 requires an explicit owner decision before any deletion; back up with `git ls-files` check + local tar if tracked |
| Breaking Godot import after config removal | Run `godot --headless --path . -- --data-integrity-selftest` after every deletion |
| LFS corruption from .gitattributes changes | Verify with `git lfs ls-files` before and after (baseline: 5064); keep backups |
| History rewrite temptation | Explicitly out of scope — document for future batch, as in the original plan |
| Partial file deletion leaving dangling references | `grep -r` for removed filenames across entire repo before committing |
| Conflating "local disk cleanup" with "repo size reduction" in the final report | Step 5 and Step 7 keep local-only numbers in a separate table from git/repo numbers |
| Documentation edits deleting legitimate historical context | Step 4 explicitly distinguishes historical/narrative mentions of `Assets/_Game/` from actionable path claims; only the latter are corrected |

---

## Exit Criteria (Batch 69 Complete)

- [ ] Dead code audit report complete with confidence-categorized candidates, each with a reproducible search command.
- [ ] Unwired host sessions identified and documented against the *current* `src/Main.cs` (not assumptions).
- [ ] `ProjectSettings/`/`Packages/` question explicitly resolved (keep or remove) and recorded — not silently skipped.
- [ ] AGENTS.md corrected: Invariant 5 offender list no longer cites deleted `Assets/_Game/` paths as current; H7 method counts match the real `src/Main.cs` counts.
- [ ] `REPO_REVIEW_REPORT.md` read in full and reconciled against current codebase state.
- [ ] Stray local `unity-assets-archive-2026-08-14.tar.gz` handled per owner decision (kept or deleted), with the git-history-vs-local-file distinction documented.
- [ ] `.gitattributes` contains only patterns for file types that exist, or are explicitly conditional on Step 3's outcome.
- [ ] Size/report with before/after measurements, repo-size and local-disk numbers kept separate.
- [ ] All 5 verification steps pass on final state.
- [ ] No test regressions.
- [ ] No behavioral changes to the game.

---

## Review Notes (Corrected)

This plan was adversarially reviewed against the live repository at
`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` before editing. Findings:

1. **`Assets/_Game/` is confirmed fully deleted** (`ls Assets/` shows only `Ashfall.Core/`, `StreamingAssets/`, `art/`, `audio/`, `sprites/`, `ui/`, plus `.meta`/`.gdignore` files — no `_Game/` subdirectory, `ls Assets/_Game` errors with "No such file or directory"). The original plan's premise on this point was correct and is preserved.
2. **`ProjectSettings/` and `Packages/` are NOT deleted** — the original Step 3 assumed these needed removing "if still present" but treated it as a mechanical cleanup. They are present (27 `.asset`/config files plus a nested `ProjectSettings/Packages/` directory, 29 top-level entries total, in `ProjectSettings/`; `manifest.json` + `packages-lock.json` in the top-level `Packages/`) and there is no evidence in the repo that they're safe to delete without an owner decision — rewritten as Step 3 with an explicit decision gate and upgraded risk rating. Both are also git-tracked (`git ls-files ProjectSettings/ Packages/` returns 34 tracked paths), confirming the Step 3 risk note that deletion is only cleanly reversible via `git checkout` while they remain tracked.
3. **`GameBootstrap` and `GameBootstrap.Phase0Expansion.cs` do not exist in the active tree.** The only match for `GameBootstrap` outside `Assets/Ashfall.Core/*Session.cs` (unrelated classes with "Bootstrap" as a substring, e.g. `HoldfastSession.cs`/`CrossingSession.cs` — false positive grep hits, not GameBootstrap itself) is `_quarantine_legacy/Assets/Scripts/Core/GameBootstrap.cs`, and `_quarantine_legacy/` is gitignored (`.gitignore:130`). The original plan's "Phase 11 stub" known-suspect in Step 1 pointed at a nonexistent file; corrected to point at the current host files instead.
4. **`scripts/` is not empty, but it is also not "actively compiled" — that phrasing (present in an earlier draft of this same review) overstated the mechanism.** `Ashfall.csproj` does include `<Compile Include="scripts/**/*.cs" .../>`, but `scripts/` contains zero `.cs` files today (confirmed via `find scripts -iname "*.cs"` — no results), so the glob matches nothing and nothing from `scripts/` is actually compiled by that line. The directory holds real Python tooling and a `ci/` subdirectory with shell scripts referenced by AGENTS.md's own asset-gate verification (`scripts/ci/godot-asset-gate.sh`), which is the actual reason it's not orphaned. The original Step 5 ("Remove Empty scripts/ Directory Reference from .csproj") was based on a false premise (that the directory was empty) and has been deleted from this plan; scripts/ requires no action — but note for a future batch that the `<Compile Include="scripts/**/*.cs" .../>` line in `Ashfall.csproj` is itself currently a no-op glob and could legitimately be flagged as its own tiny piece of dead build configuration, separate from the question of whether the `scripts/` directory itself is dead.
5. **`unity-assets-archive-2026-08-14.tar.gz` is 146 MB on disk (not 140 MB as AGENTS.md's older text states) and is already untracked + gitignored**, per commit `d03dd555` ("fix(cli): wire dead selftest verbs, drop Unity archive, sync AGENTS.md"). The original plan's Step 7 treated this purely as a "measure it, note it for later" item under binary-size reporting; corrected into its own Step 5 that clarifies it is a *local disk* concern, not a repo-size or git-history concern — the git-history blob is a separate, still-unresolved, still-explicitly-out-of-scope question.
6. **`HoldfastRuntimeSession` is not an "unwired" suspect** — it is actively constructed and ticked from `src/Main.cs` (confirmed via `grep`: `HoldfastRuntimeSession.Create(_core)` at line 1450, `_holdfastRuntime.TickDay()` inside the day-tick, and multiple self-test constructions). The original Step 2 named it as the sole "known suspect" for dead-session auditing, which would have wasted the audit step confirming something already known to be alive; corrected to remove it as a suspect and instead direct the audit at the full, actual list of `*HostSession.cs` files.
7. **`src/Main.cs` method counts were re-verified and differ from AGENTS.md's H7**: 37 `SetupXxx` methods (AGENTS.md says 31; re-verified via `grep -c "private void Setup[A-Za-z]*(" src/Main.cs` — a prior draft of this same review had this at 38, which was its own counting error, now corrected), 29 `SaveXxx`-prefixed methods including `SaveAll` itself (AGENTS.md says 24 + `SaveAll`; re-verified via `grep -c "private void Save[A-Za-z]*(" src/Main.cs`, `SaveAll` confirmed at line 6227), 16 `FlushXxxIfDirty` methods (AGENTS.md says 17). None of these are new problems introduced by this review; they indicate the counts in AGENTS.md have already drifted since it was last edited and should be recounted as part of Step 4 rather than propagated forward as fact by this batch. **This also means Step 1's and Step 4's numbers must be treated as a snapshot, not a constant** — the exact figures will keep drifting as `src/Main.cs` grows, which is precisely why Step 1's implementation note says not to hardcode either number into tooling.
8. **`.gitattributes` already contains a maintainer note (dated 2026-08)** stating LFS is confirmed active (5064 files via `git lfs ls-files`) and clarifying the audio plain-binary exception. The original Step 6 read as if this had never been investigated; corrected to treat the existing note as authoritative and scope the step to only the genuinely-unused Unity extensions, gated on Step 3 for `*.asset`/`*.asmdef`.
9. **No `docs/dead-code-candidates.md`, `docs/size-reduction-report.md`, or `tools/dead-code-audit.sh` exist yet** — confirmed via `find`. These are legitimately net-new outputs of this batch, not something to "update"; no correction needed here, noted for completeness.
10. **Ordering fix:** the original plan's Step 4 (docs cleanup) came after Step 3 (config removal) and Step 5 (empty scripts/ removal, now deleted). Since Step 3 here is now decision-gated and may not execute in this batch, doc cleanup (Step 4) no longer has a hard dependency on it and can proceed independently — the dependency was illogical in the original ordering because the original Step 3 removal and the doc cleanup in Step 4 don't actually share any file targets.
11. **A second review pass (this one) re-verified every numeric claim in the draft above against
    the live repository and found the "corrected" numbers themselves contained new errors** —
    corrections compound if not re-checked, which is the same lesson this document repeatedly
    applies to AGENTS.md. Specifically:
    - The `SetupXxx` count was stated as **38** in three places (Step 1's implementation note, Step
      4's implementation note, and finding #7 above). Re-running
      `grep -c "private void Setup[A-Za-z]*(" src/Main.cs` during this pass returns **37**, not 38.
      All three locations are corrected.
    - The `SaveXxx`-prefixed count (including `SaveAll`) was stated as **30**; re-running
      `grep -c "private void Save[A-Za-z]*(" src/Main.cs` returns **29** (with `SaveAll` itself
      confirmed at line 6227 as previously claimed). Corrected in Step 4's implementation note and
      finding #7.
    - `ProjectSettings/` was described as having **27 files**; `ls -la ProjectSettings/` shows 27
      `.asset`/config files plus an empty `.gdignore` plus a **nested `ProjectSettings/Packages/`
      directory** (distinct from the top-level `Packages/` at the repo root) — 29 top-level entries
      in total. The nested `Packages/` subdirectory was not mentioned anywhere in the prior draft
      and is relevant to Step 3's decision gate, since it means there are two `Packages/` locations
      to account for, not one. Corrected in Step 3, Step 6, and finding #2.
    - The claim that `scripts/` is "actively compiled" (Step 1's motivation section and finding #4)
      overstated what the `Ashfall.csproj` `<Compile Include="scripts/**/*.cs" .../>` line actually
      does: `scripts/` contains zero `.cs` files today, so the glob matches nothing and compiles
      nothing. The directory is still correctly judged as not dead — it holds real Python tooling
      used elsewhere in the project — but for a build-dependency reason, not a compile-dependency
      one. Corrected in both locations, and the no-op glob itself was added to Step 1's "known
      suspects" list as a minor, no-action-needed finding.
    - The `CatalogIntegrityValidator.cs` line count and `CaptureState` occurrence-count figures
      referenced in Batch 70 (a companion document reviewed in the same pass) were also found to
      have drifted (657 actual vs. 603 cited, 211 actual vs. 203 cited) — noted here only for
      cross-reference; those corrections were applied to Batch 70 directly, not this file.
    None of these corrections change any conclusion, risk rating, or exit criterion in this plan —
    they are all instances of a number drifting between when it was first counted and when it was
    last copied forward. The practical takeaway, already stated throughout this plan, stands
    reinforced: **do not hardcode any of these counts into tooling or documentation; re-derive them
    from source at the time they're needed.**
