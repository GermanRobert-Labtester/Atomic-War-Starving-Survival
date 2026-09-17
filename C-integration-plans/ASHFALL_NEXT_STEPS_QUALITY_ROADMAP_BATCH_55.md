# ASHFALL Quality Roadmap — Batch 55
## Theme: CI Pipeline Hardening & Automated Quality Gates

**Priority:** MEDIUM-HIGH (infrastructure — prevents regressions)
**Risk:** Low-implementation / **HIGH-decision** — see Step 0 below. The CI files that exist today are not a partial version of the `dotnet`/`godot` pipeline; they are a **different, Unity-based pipeline** that AGENTS.md forbids running. This changes the shape of the whole batch from "extend" to "replace or run side-by-side," which is a project-authority decision, not a CI-scripting detail.
**Prerequisite:** None (can run in parallel with all other batches), but Step 0 must be resolved with the user before any workflow file is edited.

---

## Rationale (corrected)

The project's canonical verification path (AGENTS.md, non-negotiable rule #2) is `dotnet` + `godot --headless`, with **zero** Unity invocation permitted unless explicitly requested. `docs/CI.md` and `docs/ASHFALL_CODE_INDEX.md` already document this pipeline in detail, including self-test flags well beyond the 5 baseline steps (`--expansions-selftest`, `--verdict-selftest`, `--muster-selftest`, `--dose-ledger-selftest`, etc.).

**What actually exists on disk contradicts this.** Both `.github/workflows/ci.yml` and `.github/workflows/build.yml` were read in full for this batch and contain **no `dotnet` and no `godot --headless` anywhere**. They are 100% Unity: `game-ci/unity-builder@v4`, `game-ci/unity-test-runner@v4`, `EditMode`/`PlayMode` test runs, `AtomicWar._Game.Editor.CatalogGenerator.GenerateAll`, `AtomicWar._Game.Editor.GameplaySceneBuilder.BuildGameplayScene`, and multi-platform Unity builds (Linux64/Windows/WebGL), pinned to `UNITY_VERSION: 6000.5.5f1`.

This is not a gap in an otherwise-`dotnet` pipeline — it is a fully independent, Unity-only pipeline that runs on every push/PR today. Editing it (adding steps, changing gates) means modifying live Unity automation, which AGENTS.md's rule #1 says not to invoke "ever — unless the user explicitly asks in that message." Whether that rule is meant to constrain *this agent's local actions* or *also* the *committed CI configuration* is a real ambiguity this batch must resolve first, not paper over.

This batch, corrected, does the following:
1. Documents the actual current state of `ci.yml`/`build.yml` (100% Unity, 0% dotnet/Godot) instead of assuming partial coverage.
2. Forces an explicit decision with the user on how the two pipelines coexist (see Step 0).
3. Adds a **new**, separate `dotnet`/`godot`-only workflow rather than silently rewriting the existing Unity gate, unless the user directs otherwise.
4. Adds code quality gates (invariant enforcement, no engine coupling).
5. Adds a test coverage regression gate.
6. Adds self-test execution in headless Godot as a CI step, contingent on Godot-in-CI feasibility (Step 6).

---

## Step 0 — Resolve Pipeline Authority Conflict (NEW — blocks all other steps)

**Goal:** Get an explicit user decision on how the existing Unity CI (`ci.yml`, `build.yml`) relates to the AGENTS.md-mandated `dotnet`/`godot --headless` pipeline, before touching either workflow file.

**Why this step exists:** The original plan assumed `ci.yml`/`build.yml` were the right place to bolt on `dotnet build`/`godot --headless` steps because they are "the GitHub Actions workflows." Reading them shows they are Unity `EditMode`/`PlayMode`/build workflows with no relation to the Core/Godot pipeline at all. Silently adding `dotnet`/`godot` jobs into a Unity-pinned workflow file conflates two build systems the AGENTS.md explicitly keeps apart, and touching `ci.yml`/`build.yml` at all is itself borderline "invoking a Unity build tool" territory if done carelessly (e.g. changing `unity-builder`/`unity-test-runner` steps).

**Implementation:**
1. Present the user with the concrete finding: `ci.yml` and `build.yml` contain zero `dotnet`/`godot` steps and are entirely `game-ci/unity-*` actions targeting Unity `6000.5.5f1`.
2. Ask the user to choose one:
   - **(A) Add a new workflow** — e.g. `.github/workflows/core-godot-ci.yml` — that runs only the 5-step `dotnet`/`godot --headless` pipeline, leaving `ci.yml`/`build.yml` untouched. Lowest risk; does not require reasoning about the Unity jobs at all.
   - **(B) Retire the Unity workflows** — mark `ci.yml`/`build.yml` as legacy/disabled (e.g. rename trigger to `workflow_dispatch`-only) and replace their role with the new Core/Godot workflow, since AGENTS.md says Unity is not a target editor.
   - **(C) Run both, unmodified relationship** — keep Unity CI as-is for the legacy `Assets/_Game/` tree (which AGENTS.md still treats as "read-only legacy," not "deleted"), and add Core/Godot CI as a fully separate, additive gate.
3. Record the decision at the top of this file before proceeding — the remaining steps below assume **(A)/(C)** (additive new workflow) since that is the non-destructive default; if the user picks (B), re-scope Steps 2–7 to edit `ci.yml` directly instead of a new file.

**Verification:**
- User has explicitly chosen A, B, or C.
- The decision is written down (this file, or a linked issue/ADR).

**Done when:** A named workflow-file strategy is agreed upon and recorded — not "some steps get added to `ci.yml`" but a specific file-level plan.

---

## Step 1 — Audit Current CI Workflows (corrected)

**Goal:** Understand what `build.yml` and `ci.yml` currently do, and confirm there is no partial `dotnet`/`godot` coverage to build on.

**Implementation:**
1. Read `.github/workflows/build.yml` and `.github/workflows/ci.yml` in full (already done for this corrected plan — see findings below).
2. Compare against the required 5 steps from AGENTS.md:
   - `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
   - `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
   - `dotnet build Ashfall.csproj`
   - `godot --headless --path . -- --data-integrity-selftest`
   - `godot --headless --path . -- --bridge-selftest`
3. **Confirmed findings (not "likely" — verified by reading both files):**
   - `ci.yml` jobs: `validate` (Python JSON-syntax scan of `Assets/StreamingAssets` + `ProjectVersion.txt` pin check), `regenerate` (Unity `CatalogGenerator.GenerateAll` + `GameplaySceneBuilder.BuildGameplayScene` via `game-ci/unity-builder@v4`), `test` (Unity EditMode via `game-ci/unity-test-runner@v4`), `test-playmode` (Unity PlayMode), `build-linux` (Unity `StandaloneLinux64` build).
   - `build.yml` jobs: `build-windows`, `build-webgl` — both Unity release builds, explicitly documented in-file as *not* running tests (that's `ci.yml`'s job), triggered on push-to-`main`/manual dispatch only.
   - **Zero occurrences** of `dotnet`, `godot`, `--headless`, `--data-integrity-selftest`, or `--bridge-selftest` in either file.
   - All 5 required verification steps are missing — this is a 100% gap, not a partial one.
4. Document findings (this section) and proposed changes (Steps 0, 2–7 below).

**Verification:**
- Gap analysis complete: 0/5 steps present, both workflows confirmed Unity-only by direct file read.

**Done when:** Both workflow files have been read in full, and the gap is stated as "0 of 5 dotnet/godot steps exist; both workflows are Unity EditMode/PlayMode/build pipelines" — not a vague "likely Godot headless steps missing."

---

## Step 2 — Add Core Test Job to New Workflow

**Goal:** Ensure `dotnet build` and `dotnet test` for the Core test project run on every PR, in the workflow file chosen in Step 0.

**Implementation:**
1. Create (or edit, per Step 0's decision) the target workflow, e.g. `.github/workflows/core-godot-ci.yml`:
   ```yaml
   name: Core & Godot Host CI

   on:
     push:
       branches: [main, master]
     pull_request:
       branches: [main, master]

   jobs:
     core-tests:
       name: Core Tests (dotnet)
       runs-on: ubuntu-latest
       steps:
         - uses: actions/checkout@v4

         - name: Setup .NET
           uses: actions/setup-dotnet@v4
           with:
             dotnet-version: |
               9.0.x
               8.0.x

         - name: Build Core Tests
           run: dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --configuration Release

         - name: Run Core Tests
           run: dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --configuration Release --no-build --verbosity normal
   ```
2. `Ashfall.Core.Tests.csproj` targets `net9.0` (confirmed by reading the file) and references `..\Ashfall.Core\Ashfall.Core.csproj` — the setup-dotnet step must install `9.0.x`. The Godot host (`Ashfall.csproj`) targets `net8.0` (Godot.NET.Sdk/4.7.1) — install both major versions in the same job matrix if Steps 2 and 3 are combined, or split into separate jobs with separate SDK installs.
3. A test failure in this job fails the job by default (`dotnet test` returns non-zero on any failed test) — no extra `fail-fast` flag is needed for a single-job gate; `fail-fast` only matters if this becomes a matrix job.

**Verification:**
- Push a branch, verify the new workflow triggers and `core-tests` job runs.
- Intentionally break a test locally, run `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`, confirm non-zero exit code, then confirm the same failure surfaces as a failed CI check.

**Done when:** `core-tests` job appears as a required (or at minimum, visible) status check on a PR, and a failing test produces a red check — verified by an actual PR run, not just local `dotnet test` success.

---

## Step 3 — Add Godot Host Build Job

**Goal:** Ensure `dotnet build Ashfall.csproj` (the Godot host, `Godot.NET.Sdk/4.7.1`, `net8.0`) compiles on CI.

**Implementation:**
1. Add a job to the workflow from Step 2:
   ```yaml
     godot-host-build:
       name: Build Godot Host (dotnet)
       runs-on: ubuntu-latest
       steps:
         - uses: actions/checkout@v4

         - name: Setup .NET
           uses: actions/setup-dotnet@v4
           with:
             dotnet-version: 8.0.x

         - name: Build Godot Host
           run: dotnet build Ashfall.csproj --configuration Release
   ```
2. `Ashfall.csproj` compiles `src/**/*.cs`, `scripts/**/*.cs`, and `Assets/Ashfall.Core/**/*.cs` directly (confirmed by reading the `.csproj` — there's no separate Core project reference from the Godot host; Core sources are compiled in-place). This means `dotnet build Ashfall.csproj` alone exercises Core + host together; it does **not** need `Ashfall.Core.Tests` restored first, and the two jobs are independent (no `needs:` dependency required between Step 2 and Step 3 jobs).
3. Because the project uses `Godot.NET.Sdk`, confirm the GitHub Actions runner's NuGet feed can resolve `Godot.NET.Sdk/4.7.1` (it's a standard NuGet package, no local Godot install needed for a *build*-only step — only the self-tests in Step 6 need an actual Godot binary).

**Verification:**
- CI job passes for current code — this must actually be run once (not assumed) since this is the first time this project has been built via `dotnet build Ashfall.csproj` in CI.
- Intentionally add a compile error to `src/Main.cs` on a scratch branch, verify CI catches it, then revert.

**Done when:** `godot-host-build` job is green on current `main`, and a deliberately introduced compile error in `src/Main.cs` fails the job (verified once, then reverted).

---

## Step 4 — Add Invariant 1 Enforcement (No Engine Coupling in Core)

**Goal:** Add a CI step that fails if `Assets/Ashfall.Core/` contains any engine references.

**Implementation:**
1. Add a step (can run in the `core-tests` job, no need for its own job):
   ```yaml
   - name: Invariant 1 - No engine coupling in Core
     run: |
       if grep -rn "using UnityEngine\|using UnityEditor\|using Godot\|using GodotSharp\|JsonUtility" Assets/Ashfall.Core/ --include="*.cs"; then
         echo "FAIL: Engine coupling detected in Ashfall.Core"
         exit 1
       fi
       echo "PASS: no engine coupling in Ashfall.Core"
   ```
2. This mechanically enforces Invariant 1 on every commit. Note the grep pattern intentionally matches bare `JsonUtility` (not `using ... JsonUtility`) since `JsonUtility.FromJson<T>(...)` is typically called via the static class without a `using` import of anything Unity-specific beyond the ambient `UnityEngine` namespace — the existing `using UnityEngine` check already covers the common case, but the bare `JsonUtility` term is kept as a belt-and-suspenders match for fully-qualified `UnityEngine.JsonUtility` calls.
3. Verified against the current tree: reading `Assets/Ashfall.Core/Ports.cs` and `HostDefaults.cs` (the two files most likely to reference engine types) shows no `UnityEngine`, `UnityEditor`, `Godot`, or `GodotSharp` references — consistent with AGENTS.md's "Holds today: 0 violations" claim for Invariant 1. This step has not been run against the *entire* `Assets/Ashfall.Core/` tree as part of this batch (that would require iterating all ~200+ files); treat "0 violations" as inherited from AGENTS.md's existing claim, not independently re-verified file-by-file here.

**Verification:**
- Run the grep command locally against `Assets/Ashfall.Core/` before wiring it into CI, and confirm it currently reports zero matches (do this once, locally, as part of implementing this step — do not assume it from AGENTS.md alone).
- Add a `using Godot;` to a scratch Core file, verify CI catches it, then revert.

**Done when:** The grep command has been run locally at least once against the real `Assets/Ashfall.Core/` tree with a confirmed zero-match result, and the CI step is wired in and reproduces that same zero-match pass on the next PR.

---

## Step 5 — Add Invariant 4 Enforcement (No New System.Random / Guid.NewGuid in Core)

**Goal:** Add a CI step that fails if `Assets/Ashfall.Core/` introduces new `System.Random` or `Guid.NewGuid()` usage beyond the documented, already-known offenders.

**Implementation:**
1. AGENTS.md lists these known offenders (as of the version reviewed for this batch):
   - `Assets/Ashfall.Core/FinalWishSystem.cs:66` — `public System.Random Rng;`
   - `Assets/Ashfall.Core/CombatTraumaSystem.cs:53` — `public System.Random Rng;`
   - `Assets/Ashfall.Core/WeatherSystem.cs:144` — `new Random(unchecked(...))`
   - `Assets/Ashfall.Core/ProceduralItemInstance.cs:36` — `Guid.NewGuid()` (note: AGENTS.md's Known Issues table separately marks C3/`Guid.NewGuid()` as **RESOLVED** at `Inventory/ProceduralItemInstance.cs:48`, while the Invariant 4 section still lists `ProceduralItemInstance.cs:36` as an open offender — **this is an internal AGENTS.md inconsistency, not something this CI step can silently paper over.** Before wiring the baseline count, re-check the actual current line count of `System.Random`/`Guid.NewGuid` hits in `Assets/Ashfall.Core/` rather than trusting either AGENTS.md section blindly, since the two sections disagree with each other.)
   - `InMemoryFlagLedger` — `StringComparer.OrdinalIgnoreCase` (this is a case-normalization risk, not a `System.Random`/`Guid.NewGuid` hit, and should **not** be counted in this grep-based gate at all).
2. Add a script step whose baseline is derived from an actual grep run against current `Assets/Ashfall.Core/`, not a hardcoded guess:
   ```yaml
   - name: Invariant 4 - Determinism check
     run: |
       VIOLATIONS=$(grep -rn "new Random(\|System\.Random\|Guid\.NewGuid" Assets/Ashfall.Core/ --include="*.cs" | wc -l)
       KNOWN=<run the same grep locally first and fill in the real count — do not guess>
       if [ "$VIOLATIONS" -gt "$KNOWN" ]; then
         echo "FAIL: New non-deterministic code detected ($VIOLATIONS hits, baseline $KNOWN)"
         grep -rn "new Random(\|System\.Random\|Guid\.NewGuid" Assets/Ashfall.Core/ --include="*.cs"
         exit 1
       fi
       echo "PASS: $VIOLATIONS hits (baseline: $KNOWN)"
   ```
   The pattern was tightened from `"new Random\|..."` to `"new Random(\|..."` to avoid false matches on identifiers merely containing the substring `Random` (e.g. a hypothetical `RandomEncounterTable` class name) — the original plan's bare `new Random` term has the same false-positive risk for anything textually starting with "new Random" in a comment or string, which is an acceptable known limitation of a grep-based gate; do not treat it as airtight static analysis.
3. Known offenders are allowed up to the confirmed baseline; new ones fail CI. As offenders are fixed, decrease `KNOWN`.

**Verification:**
- Run the grep locally against current `Assets/Ashfall.Core/` and record the exact count as `KNOWN` before writing the CI step — this plan does not assert a specific number because AGENTS.md's own sections disagree (3 vs. resolved) and re-deriving it is part of doing this step correctly.
- Add a new `System.Random` call to a scratch file, verify CI catches it, then revert.

**Done when:** The baseline `KNOWN` value in the committed CI step matches an actual, freshly-run grep count against `Assets/Ashfall.Core/` (documented in the PR description), and a deliberately added new offender fails the job.

---

## Step 6 — Add Godot Headless Self-Tests (Requires Godot Binary in CI)

**Goal:** Run `godot --headless` self-tests in CI for data-integrity and bridge-selftest, matching the AGENTS.md-mandated verification steps 4–5.

**Implementation:**
1. Add a Godot installation step. The project pins `Godot 4.7+` (per AGENTS.md STACK table) and `Godot.NET.Sdk/4.7.1` (per `Ashfall.csproj`) — use a matching stable release, e.g.:
   ```yaml
   - name: Install Godot
     run: |
       wget -q https://github.com/godotengine/godot/releases/download/4.7-stable/Godot_v4.7-stable_mono_linux_x86_64.zip
       unzip -q Godot_v4.7-stable_mono_linux_x86_64.zip
       echo "$PWD/Godot_v4.7-stable_mono_linux_x86_64" >> "$GITHUB_PATH"
   ```
   (Corrected from the original plan's `export PATH=...` inside a single step, which does not persist to subsequent steps in GitHub Actions — each `run:` block is a new shell. Use `$GITHUB_PATH` or reference the binary by full path in later steps.)
2. Confirm the actual downloaded binary name matches what's referenced in later steps — Godot's Mono/`.NET` release archives can rename the executable inside the zip between versions; verify the exact filename after unzip (e.g. `godot` vs `Godot_v4.7-stable_mono_linux.x86_64`) rather than assuming it matches the zip's directory name, and add a `chmod +x` if the extracted binary is not already executable.
3. Run self-tests:
   ```yaml
   - name: Data Integrity Self-Test
     run: godot --headless --path . -- --data-integrity-selftest

   - name: Bridge Self-Test
     run: godot --headless --path . -- --bridge-selftest
   ```
4. This CI job needs the one-time Godot asset import step first (referenced by `scripts/ci/godot-asset-gate.sh`, which this plan did not fully audit) — a fresh checkout does not have an imported `.godot/` cache, and self-test flags may not run correctly against an unimported project. Read `scripts/ci/godot-asset-gate.sh` before wiring this step and reuse it rather than reimplementing import logic inline.
5. **If Godot in CI is impractical** (binary size, licensing, runner constraints, missing asset-import prerequisites): document as manual-only step, add a PR template checklist reminder instead. Given this project already documents 40+ self-test flags (per `COMPREHENSIVE_GAME_AUDIT.md`) beyond just `--data-integrity-selftest`/`--bridge-selftest`, decide up front (with the user) whether this CI job should run the full self-test suite or just the two AGENTS.md-mandated ones — running only 2 of 40+ leaves a real coverage gap that should be named, not silently accepted as "done."

**Verification:**
- If running: both self-tests pass in CI, and the job has been observed to pass at least once on a real runner (not just "should work" from the yaml).
- If not: documented as manual + a PR template checklist item that actually exists in `.github/PULL_REQUEST_TEMPLATE.md` (create one if it doesn't exist) — a "reminder" that isn't wired into the PR template is not a real gate.

**Done when:** Decision made and implemented (either automated with a verified-green run, or documented manual with an actual PR template checklist item) — not a paper decision with no enforcement mechanism.

---

## Step 7 — Add Test Count Regression Gate

**Goal:** Prevent test count from decreasing (catches accidental test deletion or skip-attribute additions).

**Implementation:**
1. `dotnet test --list-tests` output format varies by SDK/runner version and includes header/footer lines that are not test names — the original plan's `grep -c "  "` (two spaces) is fragile and will overcount or undercount depending on exact output formatting. Verify the actual output format for this repo's SDK version (`Microsoft.NET.Test.Sdk 17.11.1`, confirmed from `Ashfall.Core.Tests.csproj`) before relying on it, and prefer a more precise filter:
   ```yaml
   - name: Test count regression check
     run: |
       COUNT=$(dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --list-tests --no-build 2>/dev/null | grep -E "^\s+\S+\." | wc -l)
       BASELINE=<fill in from an actual local run of this exact command — do not reuse a number from an unrelated audit doc>
       if [ "$COUNT" -lt "$BASELINE" ]; then
         echo "FAIL: Test count dropped from $BASELINE to $COUNT"
         exit 1
       fi
       echo "PASS: $COUNT tests (baseline: $BASELINE)"
   ```
   **Note on the baseline number:** different project docs report different totals — `COMPREHENSIVE_GAME_AUDIT.md` says "2,016 xUnit tests," `docs/CI.md` says "732 passed / 0 failed," and the original version of this plan asserted 1941. These numbers were not independently re-run as part of this correction (running the full suite is outside the scope of a plan-review pass), so **do not trust any of the three numbers above as the CI baseline** — the person implementing this step must run `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --list-tests --no-build` (after a build) once, count the real output, and use that as `BASELINE`. The conflicting numbers across docs are themselves a sign the project's own documentation is stale in at least two of the three places, which is worth flagging to the user separately from this CI task.
2. Update `BASELINE` when tests are intentionally added (one-way ratchet up).
3. Decrease requires explicit justification in the PR description.

**Verification:**
- Run the exact `--list-tests` command locally first, confirm the grep pattern produces a sane count (spot-check against a known small number of tests in one file), then set `BASELINE` to that confirmed count.
- Deleting a test file locally and re-running the command would produce a lower count, failing the gate.

**Done when:** `BASELINE` is set from a command that was actually executed during implementation (with its output shown in the PR), not copied from a conflicting doc, and the gate is wired into the same workflow as Step 2.

---

## Rollback Plan

Because Step 0 determines whether this batch touches the live Unity `ci.yml`/`build.yml` at all:
- If option (A) or (C) was chosen (new, additive workflow file): rollback is trivial — delete the new workflow file. No existing gate is affected.
- If option (B) was chosen (retiring Unity CI): rollback requires restoring `ci.yml`/`build.yml` triggers from git history (`git revert` the specific commit that changed the `on:` triggers) — keep that change as its own isolated commit, separate from adding the new workflow, specifically so it can be reverted independently.
- Every step in this batch (2–7) is additive (new jobs/steps) and individually revertible via normal `git revert` of that step's commit — per AGENTS.md's "one system per task" rule, each step should land as its own commit.

---

## Summary

| Step | Gate | Automates |
|------|------|-----------|
| 0 | Pipeline authority decision | Blocks all other steps — must be resolved with the user first |
| 1 | Audit (corrected: confirmed 0/5 steps exist, both workflows are Unity-only) | Manual analysis |
| 2 | Core tests (new workflow, dotnet 9.0.x) | Verification steps 1-2 |
| 3 | Godot host build (dotnet 8.0.x) | Verification step 3 |
| 4 | Invariant 1 (no engine in Core) | Architecture rule |
| 5 | Invariant 4 (determinism) — baseline must be re-derived, not assumed | Architecture rule |
| 6 | Godot headless self-tests — needs asset-import prerequisite + Godot binary in CI | Verification steps 4-5 |
| 7 | Test count ratchet — baseline must be re-derived, conflicting docs found | Coverage regression |

**End state:** A new (or, per Step 0(B), replacement) `dotnet`/`godot`-only CI workflow exists, separate from and non-destructive to the existing Unity `ci.yml`/`build.yml` unless the user explicitly chose to retire the latter. Every PR is automatically checked against the 5-step verification checklist (or documented subset), invariant violations are caught before merge, and test coverage cannot silently decrease. Manual discipline is replaced by automation — but only after the Unity-vs-Core/Godot CI authority question is resolved, not by silently editing a Unity pipeline this project's own rules say not to touch.

---

## Review Notes (Corrected)

This plan was adversarially reviewed against the real codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. The following were wrong in the original version and have been fixed above:

1. **Central factual error — CI is not partially covered, it is 0% covered and actively Unity-based.** The original Step 1 assumed `build.yml`/`ci.yml` existed in some state adjacent to the `dotnet`/`godot` pipeline ("likely Godot headless steps" missing). Reading both files in full shows they contain **zero** `dotnet`/`godot` references and are entirely `game-ci/unity-builder@v4` / `game-ci/unity-test-runner@v4` jobs (EditMode, PlayMode, Unity scene/catalog regeneration, multi-platform Unity builds), pinned to Unity `6000.5.5f1`. This directly conflicts with AGENTS.md's non-negotiable rule #1/#2 ("Unity is NOT a target editor... do not invoke... any Unity build tool — ever"). The original plan would have had an agent silently bolt `dotnet build` steps onto a live Unity CI pipeline without ever surfacing that the pipeline itself is the exact thing AGENTS.md forbids running. **Fix:** added Step 0, a mandatory blocking step that forces an explicit user decision (new additive workflow vs. replacing/retiring the Unity workflows vs. running both) before any workflow file is touched, and changed Steps 2–7 to target a new workflow file by default rather than silently editing `ci.yml`.

2. **Step 1's "expected findings" were guesses presented as findings.** Fixed by replacing them with the actual job-by-job contents of both files (job names, actions used, triggers) as confirmed by direct file reads.

3. **Missing .NET SDK version detail.** The original Step 2 mentioned "add .NET SDK setup step if not present" without specifying versions. Confirmed by reading `Ashfall.Core.Tests.csproj` (targets `net9.0`) and `Ashfall.csproj` (targets `net8.0`, `Godot.NET.Sdk/4.7.1`) — both are needed, and the corrected Step 2/3 specify `actions/setup-dotnet` with explicit version pins for each.

4. **Step 5's baseline count is unverifiable as written and contradicts AGENTS.md itself.** The original hardcoded `KNOWN=3` citing `FinalWishSystem:66`, `CombatTraumaSystem:53`, `WeatherSystem:144`. Cross-checking AGENTS.md's own "Known Issues" table shows C3 (`Guid.NewGuid()` at `ProceduralItemInstance.cs`) is separately marked **RESOLVED**, while the Invariant 4 section still lists a `ProceduralItemInstance.cs:36` offender — the source document contradicts itself on this exact count. The plan cannot inherit a number from a self-contradictory source. **Fix:** Step 5 now requires running the grep locally and deriving the real baseline at implementation time, and flags the AGENTS.md inconsistency explicitly rather than propagating it.

5. **Step 6's PATH-export bug.** The original `export PATH=...` inside one YAML `run:` block does not persist to later steps in GitHub Actions (each `run:` is a fresh shell) — this would have caused every self-test step after the install step to fail with "command not found." Fixed to use `$GITHUB_PATH`. Also added the missing prerequisite that a Godot self-test needs the one-time asset-import step (referenced in `scripts/ci/godot-asset-gate.sh`, not audited in the original plan) before self-test flags will run correctly against a fresh checkout.

6. **Step 7's test count baseline (1941) was unsourced and now confirmed to conflict with two other documents.** `COMPREHENSIVE_GAME_AUDIT.md` states 2,016 tests; `docs/CI.md` states 732; the original plan asserted 1941. None of the three were re-verified by actually running the suite as part of this review (out of scope for a plan-correction pass), so the corrected plan explicitly refuses to pick one of the three numbers and instead requires the implementer to run the real command and use that output — while flagging that at least two of the three existing docs are stale.

7. **`grep -c "  "` (two literal spaces) as a test-count proxy is fragile.** `dotnet test --list-tests` output format includes non-test header/footer lines; the original pattern would over/under-count depending on SDK version formatting. Tightened to a pattern anchored on leading whitespace + a qualified test name, with an explicit instruction to spot-check it before trusting it.

8. **Determinism grep pattern had a false-positive risk.** `"new Random\|..."` matches any identifier containing "Random" as a substring in a comment/string. Tightened to `"new Random(\|..."` and flagged as a known limitation of grep-based gates rather than presented as airtight enforcement.

9. **No rollback plan existed.** Added a dedicated Rollback Plan section, since this batch — depending on the Step 0 decision — could involve modifying a live, currently-passing Unity CI pipeline, which is exactly the kind of change that needs an explicit, isolated-commit undo path.

10. **Risk level was understated.** Original header said "Risk: Low — CI-only, no game code changes." Corrected to flag that while no *game code* changes are involved, the *decision* of what to do with an existing, contradictory, Unity-based CI pipeline is a project-authority question, not a low-risk mechanical task — hence Step 0.
