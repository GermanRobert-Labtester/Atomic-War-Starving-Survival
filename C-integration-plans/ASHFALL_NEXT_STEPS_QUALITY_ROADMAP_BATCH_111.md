# ASHFALL — Quality Roadmap Batch 111

## Theme: Build Optimization & Incremental Compilation — Faster Developer Iteration

**Priority:** MEDIUM (developer velocity — build times affect every contributor)
**Risk:** Low — build system configuration, no game code changes
**Batch:** 111
**Depends on:** None (standalone infrastructure improvement)
**Branch pattern:** `quality/batch-111-build-optimization`

---

## Context

The ASHFALL project compiles three distinct targets on every verification cycle:

| Target | Framework | Purpose |
|--------|-----------|---------|
| `Ashfall.Core/Ashfall.Core.csproj` (sources at `Assets/Ashfall.Core/**/*.cs`, `Nullable` already `enable`) | `net8.0` | Engine-agnostic domain logic |
| `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` (`ProjectReference` to `Ashfall.Core.csproj`, `Nullable` already `enable`) | `net9.0` | xUnit test suite |
| `Ashfall.csproj` (Godot host, `Godot.NET.Sdk/4.7.1`, includes `src/**/*.cs`, `scripts/**/*.cs`, and `Assets/Ashfall.Core/**/*.cs` directly by glob — no `ProjectReference` to `Ashfall.Core.csproj`) | `net8.0` | Godot presentation layer |

**Corrected vs. originally drafted:** all three projects are already `net8.0`/`net9.0` with `<Nullable>enable</Nullable>` set — there is no `netstandard2.1` target anywhere in this solution today. Do not "add" Nullable or retarget frameworks as part of this batch; that work is already done. See `## Review Notes (Corrected)` at the end of this file.

**Current pain points:**
- No build caching configured — every `dotnet build` recompiles from scratch even if only one file changed.
- No parallelism flags — projects build sequentially by default without `/m` or `--parallel`.
- Test project is monolithic — running all tests even when only one domain changed.
- Godot editor recompiles the entire solution on every `.cs` save (no conditional compilation, no assembly filtering).
- The full 5-step verification checklist (`dotnet build` × 2, `dotnet test`, `godot --headless` × 2) takes significant wall-clock time with no optimization.
- **No `Directory.Build.props` or `global.json` exist at the repo root today** — confirmed by direct inspection. Steps below must create these files, not "enhance" them.
- `Ashfall.csproj` (Godot host) pulls `Assets/Ashfall.Core/**/*.cs` in directly by glob (no `ProjectReference`), and there is currently no `ProduceReferenceAssembly`/reference-assembly boundary between "Core" and "host" for the Godot build — the reference-assembly speedup in Step 2 only helps the Tests→Core edge, not the Godot host, because the host compiles Core's source directly rather than referencing its output DLL.

**Target outcome:** 50% reduction in incremental build time for the test project; full verify cycle time reduction, target renegotiated below (see Review Notes — the original "<30s" and "1941+ tests" figures were not independently verified and should not be treated as committed numbers until Step 1's baseline is actually measured).

---

## Step 1 — Measure Current Build Times (Baseline)

### Goal
Establish reproducible baseline measurements for all build/test/verify paths so improvements can be quantified.

### Implementation

Create a benchmark script at `scripts/ci/build-benchmark.sh`:

```bash
#!/usr/bin/env bash
set -euo pipefail

RESULTS_FILE="build-benchmark-results.txt"
echo "=== ASHFALL Build Benchmark ===" > "$RESULTS_FILE"
echo "Date: $(date -Iseconds)" >> "$RESULTS_FILE"
echo "Machine: $(uname -a)" >> "$RESULTS_FILE"
echo "" >> "$RESULTS_FILE"

# Clean state
dotnet clean Ashfall.Core.Tests/Ashfall.Core.Tests.csproj -v q 2>/dev/null || true
dotnet clean Ashfall.csproj -v q 2>/dev/null || true

# 1. Cold build — Core tests (includes Core itself)
echo "--- Cold build: Ashfall.Core.Tests ---" >> "$RESULTS_FILE"
time_start=$(date +%s%3N)
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj -c Release -v q
time_end=$(date +%s%3N)
echo "Duration: $((time_end - time_start))ms" >> "$RESULTS_FILE"

# 2. Incremental build — no changes
echo "--- Incremental build (no changes): Ashfall.Core.Tests ---" >> "$RESULTS_FILE"
time_start=$(date +%s%3N)
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj -c Release -v q
time_end=$(date +%s%3N)
echo "Duration: $((time_end - time_start))ms" >> "$RESULTS_FILE"

# 3. Incremental build — touch one Core file
echo "--- Incremental build (one Core file touched) ---" >> "$RESULTS_FILE"
touch Assets/Ashfall.Core/SaveChecksum.cs
time_start=$(date +%s%3N)
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj -c Release -v q
time_end=$(date +%s%3N)
echo "Duration: $((time_end - time_start))ms" >> "$RESULTS_FILE"

# 4. Test run only (already compiled)
echo "--- Test execution only (no build) ---" >> "$RESULTS_FILE"
time_start=$(date +%s%3N)
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-build -c Release -v q
time_end=$(date +%s%3N)
echo "Duration: $((time_end - time_start))ms" >> "$RESULTS_FILE"

# 5. Godot host cold build
dotnet clean Ashfall.csproj -v q 2>/dev/null || true
echo "--- Cold build: Ashfall.csproj (Godot host) ---" >> "$RESULTS_FILE"
time_start=$(date +%s%3N)
dotnet build Ashfall.csproj -v q
time_end=$(date +%s%3N)
echo "Duration: $((time_end - time_start))ms" >> "$RESULTS_FILE"

# 6. Full verify cycle
echo "--- Full 5-step verify cycle ---" >> "$RESULTS_FILE"
time_start=$(date +%s%3N)
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj && \
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj && \
dotnet build Ashfall.csproj && \
godot --headless --path . -- --data-integrity-selftest && \
godot --headless --path . -- --bridge-selftest
time_end=$(date +%s%3N)
echo "Duration: $((time_end - time_start))ms" >> "$RESULTS_FILE"

echo ""
echo "Results written to $RESULTS_FILE"
cat "$RESULTS_FILE"
```

Add to `.gitignore`:
```
build-benchmark-results.txt
```

### Risk & Rollback
- Risk: none to game/build logic — this step only adds a script and a `.gitignore` line. The one real risk is a false baseline: if a clean build currently fails for unrelated reasons (see caveat below), the script will abort at step 1 (`set -euo pipefail`) and record no numbers, which must not be misread as "0ms = fast."
- Rollback: delete `scripts/ci/build-benchmark.sh` and revert the `.gitignore` line. No other files touched.

### Known caveat (verified during this review)
A clean rebuild was attempted directly (`dotnet clean` then `dotnet build` on both `Ashfall.Core.Tests.csproj` and `Ashfall.csproj`) and **both failed** with:
```
Assets/Ashfall.Core/ActionResult.cs(59,30): error CS0523: Struct member 'ActionResult.InnerResult' of type 'ActionResult?' causes a cycle in the struct layout
```
`ActionResult.cs` is untracked in git (`git status` shows `??`) and unrelated to this batch's scope — a struct cannot contain a nullable field of its own type in C#. This is a pre-existing, unrelated repo defect, not something introduced by build-optimization work. **Before running Step 1's benchmark, either fix or remove that file**, otherwise the "cold build" and "Godot host cold build" measurements in the script will fail outright and the benchmark will report nothing rather than a slow number. Flag this to the user rather than silently patching `ActionResult.cs` as part of a build-optimization batch — fixing it is a one-line, unrelated change (make `InnerResult` a reference type or wrap the nested result, e.g. via a class or `IReadOnlyList<ActionResult>` of depth 1) and should be a separate, reviewed commit.

### Verification
```bash
chmod +x scripts/ci/build-benchmark.sh
./scripts/ci/build-benchmark.sh
# Confirm results file has all 6 measurements with non-zero durations
# If the script aborts early, check for the ActionResult.cs CS0523 caveat above first —
# do not assume the benchmark itself is broken.
```

### Done when
- [ ] `scripts/ci/build-benchmark.sh` exists, is executable, and completes all 6 measurements without aborting (confirm exit code 0, not just "ran")
- [ ] Baseline measurements recorded for: cold build, incremental (no change), incremental (one file), test-only, Godot host, full verify — each as a concrete millisecond number written to `build-benchmark-results.txt`, not just "recorded" as a checkbox
- [ ] Results file excluded from git tracking (confirm via `git check-ignore build-benchmark-results.txt`)
- [ ] The pre-existing `ActionResult.cs` clean-build failure is resolved or explicitly called out to the user before baseline numbers are treated as valid

---

## Step 2 — Optimize Project References & Dependency Graph

### Goal
Ensure that changing a test file does NOT trigger recompilation of `Ashfall.Core`. Note upfront: this step's `ProduceReferenceAssembly` optimization only affects the **Tests → Core** edge (a real `ProjectReference`). It does **not** help the Godot host, because `Ashfall.csproj` has no `ProjectReference` to `Ashfall.Core.csproj` — it includes `Assets/Ashfall.Core/**/*.cs` directly via `<Compile Include>` glob (confirmed by reading `Ashfall.csproj`). Changing any Core source file will always force a full recompile of the Godot host under the current project structure; only a real conversion of the host to depend on `Ashfall.Core.dll` via `ProjectReference` would change that, and that is an architectural change out of scope for this batch — do not attempt it here.

### Implementation

**`Directory.Build.props` does not exist yet** (confirmed — no such file at repo root). This step creates it for the first time; it is not an "audit" of existing content.

**Audit current project references:**

`Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` already references Core as a `<ProjectReference>` (confirmed by reading the file):

```xml
<ProjectReference Include="..\Ashfall.Core\Ashfall.Core.csproj" />
```

No change needed here — the path in the original draft (`..\Ashfall.Core.csproj`, without the `Ashfall.Core\` subdirectory) was wrong; the real project lives at `Ashfall.Core/Ashfall.Core.csproj` relative to repo root.

**Create `Directory.Build.props`** at repo root:

```xml
<Project>
  <PropertyGroup>
    <!-- Prevent transitive project reference rebuilds -->
    <DisableTransitiveProjectReferences>true</DisableTransitiveProjectReferences>
  </PropertyGroup>
</Project>
```

**Verify output paths are isolated** — each project must output to its own `bin/` and `obj/` directories (default behavior; do NOT add an explicit override unless a conflict is actually observed — an unconditional `BaseIntermediateOutputPath`/`BaseOutputPath` override in `Directory.Build.props` risks breaking the Godot editor's own expectations about where `Ashfall.csproj`'s output lives, since Godot's `.godot/` import cache and C# hot-reload assume default `bin/`/`obj/` locations relative to each `.csproj`). Skip this unless Step 1's baseline shows an actual output-path collision.

**Set `ProduceReferenceAssembly`** for Core — this generates a metadata-only reference assembly so the **Tests** project only rebuilds when Core's public API surface changes, not when method bodies change. This has no effect on the Godot host build (see Goal above):

```xml
<!-- In Ashfall.Core/Ashfall.Core.csproj -->
<PropertyGroup>
  <ProduceReferenceAssembly>true</ProduceReferenceAssembly>
</PropertyGroup>
```

**Files touched:**
- `Directory.Build.props` (root) — new file
- `Ashfall.Core/Ashfall.Core.csproj` (add `ProduceReferenceAssembly`)

### Risk & Rollback
- Risk: Low. `DisableTransitiveProjectReferences` can change build order/output for solutions with deep reference chains; this repo only has one real `ProjectReference` edge (Tests → Core), so the blast radius is small. `ProduceReferenceAssembly` can occasionally surface `InternalsVisibleTo`-related build ordering issues if internals are exposed across the reference boundary — confirmed via search that no `InternalsVisibleTo` attribute exists anywhere in the codebase today, so this specific risk does not currently apply, but re-check if one is added later.
- Rollback: delete `Directory.Build.props`; remove `<ProduceReferenceAssembly>true</ProduceReferenceAssembly>` from `Ashfall.Core.csproj`. Both are single-property, easily reverted changes.

### Verification
```bash
# Build everything clean
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj -c Release

# Touch only a test file — Core should NOT rebuild
touch Ashfall.Core.Tests/SaveWireContractTests.cs
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj -c Release -v detailed 2>&1 | grep -c "Ashfall.Core.*Compiling"
# Expected: 0 (Core not recompiled)

# Touch a Core method body (not public API) — with reference assemblies, tests should NOT rebuild
# (only works if ProduceReferenceAssembly is active and the ref assembly didn't change)

# Confirm this does NOT help the Godot host (documenting the limitation, not a bug):
touch Assets/Ashfall.Core/SaveChecksum.cs
dotnet build Ashfall.csproj -v detailed 2>&1 | grep -c "SaveChecksum"
# Expected: >0 — the Godot host recompiles Core sources on every Core-file touch, by design of the glob include.
```

### Done when
- [ ] `Directory.Build.props` created at repo root with `DisableTransitiveProjectReferences`
- [ ] `ProduceReferenceAssembly` enabled for `Ashfall.Core/Ashfall.Core.csproj`
- [ ] Touching a test file does not trigger Core recompilation (verified via `-v detailed` grep, not assumed)
- [ ] Touching a Core method body (private/internal change) does not trigger the **test** project's recompilation when public API is unchanged
- [ ] Explicitly documented (not silently accepted) that the Godot host still recompiles all of Core on any Core-file change — this step does not and cannot fix that without a `ProjectReference` restructure, which is out of scope

---

## Step 3 — Add Build Parallelism

### Goal
Enable MSBuild parallel compilation so independent projects and source files compile concurrently.

### Implementation

**Add parallel build properties** to `Directory.Build.props`:

```xml
<PropertyGroup>
  <!-- Enable parallel compilation within a project -->
  <BuildInParallel>true</BuildInParallel>
  <!-- Deterministic builds (required for caching) -->
  <Deterministic>true</Deterministic>
</PropertyGroup>
```

**Update verification commands** to use `--parallel` (shorthand `-m`):

The verification checklist in `AGENTS.md` uses sequential builds. Create an optimized verify script at `scripts/ci/verify-parallel.sh`:

```bash
#!/usr/bin/env bash
set -euo pipefail

echo "[1/5] Building test project (includes Core)..."
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj -m -v q

echo "[2/5] Running tests..."
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-build -v q

echo "[3/5] Building Godot host..."
dotnet build Ashfall.csproj -m -v q

echo "[4/5] Data integrity selftest..."
godot --headless --path . -- --data-integrity-selftest

echo "[5/5] Bridge selftest..."
godot --headless --path . -- --bridge-selftest

echo "=== ALL 5 CHECKS PASSED ==="
```

Key optimizations:
- `-m` flag enables multi-process MSBuild (one process per project in the dependency graph).
- `--no-build` on the test step skips redundant recompilation (already built in step 1).
- Steps 4 and 5 could run in parallel (`&` + `wait`) but Godot headless may not support concurrent instances on the same project — measure first.

**Add `MaxCpuCount`** to `Directory.Build.rsp` (MSBuild response file):

```
-maxCpuCount
-verbosity:minimal
```

**Files created/modified:**
- `Directory.Build.props` — add `BuildInParallel`, `Deterministic`
- `Directory.Build.rsp` — create with parallel defaults
- `scripts/ci/verify-parallel.sh` — optimized verification script

### Risk & Rollback
- Risk: Low. `Directory.Build.rsp` is genuinely auto-discovered by MSBuild 15.6+ by walking up from the project directory (verified against Microsoft's MSBuild response-files documentation), so this will silently affect every `dotnet build`/`dotnet msbuild` invocation in the repo, including ones run outside this batch's scripts. `-maxCpuCount` with no explicit number uses all logical cores, which can starve other processes on a shared CI runner or a low-core dev machine — consider pinning a number (e.g. `-maxCpuCount:4`) rather than the unbounded default if this repo's CI runner has limited cores.
- Rollback: delete `Directory.Build.rsp` and revert the `BuildInParallel`/`Deterministic` additions in `Directory.Build.props`. Both are inert once removed — no state is persisted elsewhere.
- Note: the claim "Godot headless may not support concurrent instances on the same project — measure first" in the draft is appropriately hedged; keep steps 4 and 5 sequential unless this is actually measured, since a wrong guess here produces flaky, hard-to-debug CI failures rather than a build error.

### Verification
```bash
chmod +x scripts/ci/verify-parallel.sh
./scripts/ci/verify-parallel.sh
# All 5 steps pass
# Compare wall-clock time against baseline from Step 1 — do not claim a speedup without the Step 1 baseline numbers in hand
```

### Done when
- [ ] `BuildInParallel` and `Deterministic` set in `Directory.Build.props`
- [ ] `Directory.Build.rsp` created with `-maxCpuCount` (pinned to a specific count if running on shared/limited-core CI, unbounded otherwise — document which was chosen and why)
- [ ] `scripts/ci/verify-parallel.sh` passes all 5 verification steps end to end (actual run, not just "should pass")
- [ ] Measured improvement: parallel build faster than sequential, with the actual before/after millisecond numbers recorded, not just a checkbox

---

## Step 4 — Split Test Project by Domain

### Goal
Enable running fast unit tests independently from slower integration/selftest tests, reducing iteration time when working on a specific domain.

**Unverified numbers caveat:** the test counts below (~1600 unit / ~341 integration, 1941+ total) were not independently confirmed for this review — the plan's own AGENTS.md references "1941+ tests" as a live figure that changes as work lands. Before starting this step, run `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --list-tests -v q | grep -c "^\s*Ashfall"` (or equivalent) to get the actual current total, and do not hardcode the specific split numbers below into any committed doc — use them only as an illustrative starting point for categorization, and record the real numbers when this step is actually executed.

### Implementation

**Split strategy (illustrative — confirm real counts before committing to a doc):**

| New project | Content | Expected run time |
|-------------|---------|-------------------|
| `Ashfall.Core.Tests.Unit/` | Pure logic tests (determinism, systems, combat, economy, medical, etc.) | Faster — no I/O |
| `Ashfall.Core.Tests.Integration/` | Save round-trips, catalog validation, wire contract, cross-system scenarios | Slower — file I/O, full catalog loads |

**Create `Ashfall.Core.Tests.Unit/Ashfall.Core.Tests.Unit.csproj`** (match the real Tests project's settings — `net9.0`, `Nullable enable`, matching `NoWarn` list — do not invent a different configuration):

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <RootNamespace>Ashfall.Core.Tests</RootNamespace>
    <IsPackable>false</IsPackable>
    <NoWarn>$(NoWarn);xUnit2013;xUnit2020;CS8618;CS8603;CS8600;CS8601;CS8602;CS8604;CS8625</NoWarn>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="xunit" Version="2.9.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.1" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\Ashfall.Core\Ashfall.Core.csproj" />
  </ItemGroup>
</Project>
```
(Package versions above are pinned to match the exact versions already in `Ashfall.Core.Tests.csproj` — verified by reading that file — rather than the floating `2.9.*`/`17.*` ranges in the original draft. Floating versions on a new project risk pulling in a different xUnit/SDK minor version than the existing suite, which can silently change test discovery or assertion behavior between the two split projects.)

**Create `Ashfall.Core.Tests.Integration/Ashfall.Core.Tests.Integration.csproj`:**

Same structure, same references. The split is by file placement, not by conditional compilation.

**Migration criteria — which tests go where:**

- **Unit** (fast, no I/O, no file system, no large data): `*SystemTests.cs`, `*BehaviorTests.cs`, `DeterminismTests.cs`, `CombatTraumaTests.cs`, `EconomyTests.cs`, etc.
- **Integration** (file I/O, full catalog loads, save round-trips, multi-system): `SaveWireContractTests.cs`, `SaveStoreChecksumSweepTests.cs`, `DataRuleComplianceTests.cs`, `CatalogIntegrity*Tests.cs`, etc.

**Add a solution-level test runner script** (`scripts/ci/test-unit.sh`):

```bash
#!/usr/bin/env bash
set -euo pipefail
dotnet test Ashfall.Core.Tests.Unit/Ashfall.Core.Tests.Unit.csproj --no-build -v q
echo "=== UNIT TESTS PASSED ==="
```

**Backward compatibility:** Keep `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` as a meta-project that includes both sub-projects via `<ProjectReference>`, OR update the verification checklist to run both. The canonical verification command must still pass.

**Alternative (lower-risk):** Instead of physically splitting, use xUnit `[Trait("Category", "Unit")]` and `[Trait("Category", "Integration")]` with `--filter`:

```bash
# Fast unit tests only
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "Category=Unit"

# Full suite
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

This avoids project restructuring but still enables selective test runs. Recommended as Phase 1; physical split as Phase 2 if trait filtering proves insufficient.

### Risk & Rollback
- Risk: Low-Medium. This is the highest-risk step in this batch: splitting a 1-project test suite into 2+ projects (or introducing trait filtering) risks silently dropping tests if a file is missed during the move, or double-counting if a file ends up compiled into both projects (easy to do by accident with glob-based `<Compile Include>` if both new `.csproj` files aren't scoped carefully to disjoint file sets). The "no tests lost" check in Done-when must compare an actual before/after count, not a visual scan.
- Rollback: if using the trait-filtering approach (recommended, see below), rollback is trivial — remove the `[Trait]` attributes and the `--filter` usage in scripts; the single test project is untouched. If using the physical project-split approach, rollback means deleting the two new `.csproj` files and moving their `.cs` files back into `Ashfall.Core.Tests/`, then re-verifying the original test count matches pre-split. Prefer trait filtering specifically because its rollback is a no-op on the source tree.

**Recommendation for this batch:** implement trait-based filtering only (Phase 1). Do not attempt the physical project split (Phase 2) in this batch — it's a larger, separate, reviewable change with its own regression risk (test discovery, CI matrix changes, IDE test-explorer grouping), and nothing in this batch's stated goal (faster iteration) strictly requires physically separate assemblies when `--filter` already achieves the fast/slow split.

### Verification
```bash
# Get the real baseline count before any changes:
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --list-tests -v q 2>&1 | tail -5

# If using trait-based filtering:
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "Category=Unit"
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "Category=Integration"
# Sum of both filtered counts must equal the unfiltered baseline count recorded above — verify this
# arithmetically, do not eyeball it.
```

### Done when
- [ ] Test categorization strategy chosen — **trait-based filtering**, not physical project split, unless a documented reason forces the split later
- [ ] Fast-path command (`--filter "Category=Unit"`) demonstrably reduces wall-clock time vs. the full suite (measured, not assumed)
- [ ] Full test suite still passes via canonical verification command, unchanged
- [ ] Baseline test count captured before the change and confirmed equal to (Unit count + Integration count + any untagged remainder) after — arithmetic check, not a visual scan
- [ ] Developer documentation updated with the fast-test command

---

## Step 5 — Add Build Caching via Directory.Build.props

### Goal
Enable MSBuild's incremental compilation and output caching so unchanged projects skip compilation entirely on subsequent builds.

### Implementation

**Enhance `Directory.Build.props`** with caching directives:

```xml
<Project>
  <PropertyGroup>
    <!-- Already added in earlier steps -->
    <Deterministic>true</Deterministic>
    <ProduceReferenceAssembly>true</ProduceReferenceAssembly>
    <BuildInParallel>true</BuildInParallel>
    <DisableTransitiveProjectReferences>true</DisableTransitiveProjectReferences>

    <!-- Skip unnecessary targets on incremental -->
    <GenerateDocumentationFile>false</GenerateDocumentationFile>
  </PropertyGroup>

  <!-- Shared compilation server (Roslyn VBCSCompiler) keeps compiler warm -->
  <PropertyGroup Condition="'$(ContinuousIntegrationBuild)' != 'true'">
    <UseSharedCompilation>true</UseSharedCompilation>
  </PropertyGroup>
</Project>
```

**Correction from the original draft:** `TieredCompilation` and `ReportAnalyzer` were removed from this list. `TieredCompilation` is a **runtime JIT** setting (it controls how the CLR tiers up JIT'd methods at execution time) — it has no effect on `dotnet build` compile time and does not belong in a build-optimization `Directory.Build.props`; including it would just be a no-op that misleads future readers about what's actually speeding up the build. `ReportAnalyzer` defaulting to `false` is already the default value, so setting it explicitly adds noise without changing behavior — only add it if a specific analyzer-timing investigation needs the report.

**Enable Roslyn compiler server** — the `VBCSCompiler` process stays resident and caches syntax trees between builds. This is the single largest speedup for incremental builds:

```bash
# Verify compiler server is running after first build
dotnet build-server status
```

**Add `global.json`** to pin SDK (if not already present — confirmed via direct inspection that no `global.json` exists at repo root today). **Do not use the SDK version from the original draft (`9.0.100`)** — the only SDK installed in this environment is `10.0.302` (confirmed via `dotnet --list-sdks`), and pinning to an SDK version that isn't installed will make every `dotnet` command in the repo fail outright with an SDK-resolution error, which is a much worse outcome than the problem this step is trying to solve. Pin to the SDK version actually present on the build machines that will run this repo — verify with `dotnet --list-sdks` on each target machine (dev boxes and CI) before choosing a value, since they may differ from this review environment:

```json
{
  "sdk": {
    "version": "10.0.302",
    "rollForward": "latestFeature"
  }
}
```

If dev machines and CI are known to have different SDK minor versions installed, use `"rollForward": "latestMajor"` instead so `global.json` doesn't become a hard blocker on machines with a slightly different patch version — the tradeoff is losing exact build reproducibility across machines, which is likely acceptable here since this project has no `global.json` today and hasn't needed one.

**Consider `Microsoft.Build.Artifacts`** for output caching (optional, larger teams) — flagged as speculative/optional in the original draft and kept that way here; do not implement without first confirming CI would actually benefit, per the draft's own caveat.

**Files modified:**
- `Directory.Build.props` — consolidated caching properties
- `global.json` — SDK pinning (new file; confirmed no prior version to preserve)

### Risk & Rollback
- Risk: Medium for `global.json` specifically — an incorrect or overly strict SDK pin can break builds on any machine (dev or CI) whose installed SDK doesn't satisfy the `rollForward` policy. This is the one change in this batch with a plausible "breaks everyone's build" failure mode, so verify the actual installed SDK on every machine that builds this repo (not just this review sandbox) before merging.
- Rollback: delete `global.json` — the repo builds today without one, so removing it returns to the current (working) behavior. `Directory.Build.props` additions in this step are additive properties; delete the added lines to roll back independently of `global.json`.

### Verification
```bash
# Cold build
dotnet clean Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj -v minimal
# Note time

# Immediate rebuild (no changes) — should show "Build succeeded. 0 Warning(s) 0 Error(s)" almost instantly
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj -v minimal
# Should complete in <2 seconds (skipped compilation)

# Verify compiler server is active
dotnet build-server status | grep -i "alive\|running"
```

### Done when
- [ ] `UseSharedCompilation` enabled (Roslyn compiler server active)
- [ ] `Deterministic` build confirmed (required for caching correctness)
- [ ] No-change rebuild completes in under 2 seconds
- [ ] `global.json` pins SDK version
- [ ] All 5 verification steps still pass

---

## Step 6 — Optimize Godot Editor Compilation

### Goal
Reduce the compilation overhead when working in the Godot editor by excluding test assemblies and enabling conditional compilation for editor-only code.

### Implementation

**Problem:** Godot 4.x (.NET) recompiles the entire solution whenever any `.cs` file changes. This includes test files that the editor never needs.

**Solution 1 — Exclude test project from Godot's build:**

In `Ashfall.csproj` (the Godot host project), ensure there is NO `<ProjectReference>` to the test project. The test project should only be built by `dotnet build/test` directly. Verify:

```bash
grep -r "Ashfall.Core.Tests" Ashfall.csproj
# Expected: no results
```

**Solution 2 — Godot editor compilation scope:**

`project.godot` already sets `[dotnet] project/assembly_name="Ashfall"` (confirmed by reading the file), pointing the editor at `Ashfall.csproj` specifically — the original draft's suggestion to add this line is unnecessary, it's already there.

**Correction from the original draft:** the repo does not use a `.sln` file — it uses `Ashfall.slnx` (the newer XML-based solution format), and it **does** group all three projects together, including the test project:

```xml
<Solution>
  <Project Path="Ashfall.csproj" />
  <Project Path="Ashfall.Core/Ashfall.Core.csproj" />
  <Project Path="Ashfall.Core.Tests/Ashfall.Core.Tests.csproj" />
</Solution>
```

This is relevant because whether the Godot editor's build step reads `Ashfall.slnx` or just `Ashfall.csproj` directly determines whether tests get pulled in during editor compiles. Godot 4.x (.NET) editor builds by invoking `dotnet build` against the single project named by `project/assembly_name`, not the `.slnx` — so tests are not compiled by the editor today. Verify this assumption for the exact Godot 4.7 version in use before relying on it, since editor build behavior has changed across Godot .NET releases; do not just grep the `.csproj` for a reference that doesn't exist and declare success, since the `.slnx` grouping is the more likely accidental inclusion point if editor behavior differs from expectation.

**Files modified:**
- `Ashfall.csproj` — add conditional defines only (no change needed for test exclusion; already excluded)
- Do NOT modify `Ashfall.slnx` as part of removing tests from editor scope unless Step 6 verification actually shows the editor building test code — modifying the solution file blind, without confirming the problem exists, risks breaking IDE-level "build solution" workflows (e.g. Rider/VS Code C# extension) that intentionally want all three projects listed for navigation and IntelliSense even if the Godot *editor* itself only compiles one.

### Risk & Rollback
- Risk: Low for the conditional-compilation-symbols change (additive, opt-in via `#if`). Medium-low for any change to `Ashfall.slnx` — removing the test project from the solution file would break "build all" / test-discovery workflows in IDEs that use the `.slnx` for project enumeration, for a benefit (faster editor compiles) that isn't confirmed to exist yet per the verification note above.
- Rollback: revert `Ashfall.csproj`'s `DefineConstants` addition; if `Ashfall.slnx` was touched, restore the three-project listing shown above.

**Solution 3 — Conditional compilation symbols for debug-only code:**

Add to `Ashfall.csproj`:

```xml
<PropertyGroup Condition="'$(Configuration)' == 'Debug'">
  <DefineConstants>$(DefineConstants);ASHFALL_DEBUG;GODOT_EDITOR</DefineConstants>
</PropertyGroup>
```

Use in Godot host code:

```csharp
#if ASHFALL_DEBUG
    GD.Print($"[DEBUG] System tick: {systemName} took {elapsed}ms");
#endif
```

**Solution 4 — Separate editor-time assemblies:**

If specific src/ code is only needed at edit-time (tool scripts, editor plugins), move it to a separate `.csproj` that Godot loads only in editor mode:

```
src/
├── Main.cs                    (runtime)
├── Host/                      (runtime)
└── EditorTools/               (editor-only, separate assembly)
    └── Ashfall.EditorTools.csproj
```

This is optional — only implement if editor recompilation is measurably slow due to editor-only code.

**Files modified:**
- `Ashfall.csproj` — verify no test reference, add conditional defines
- Potentially `.godot/` configuration
- `Ashfall.slnx` — only if Step 6 verification confirms the editor actually reads it for compilation (see Solution 2 correction above); do not touch pre-emptively

### Verification
```bash
# In Godot editor: save a .cs file, observe build output
# Should only compile Ashfall.csproj (and Core dependency), not tests

# From command line: verify Godot build excludes tests
dotnet build Ashfall.csproj -v detailed 2>&1 | grep -c "Ashfall.Core.Tests"
# Expected: 0

# Verify editor still runs
godot --headless --path . -- --bridge-selftest
```

### Done when
- [ ] Confirmed (not assumed) which file the Godot 4.7 editor actually reads to determine its build scope — `Ashfall.csproj` directly, or `Ashfall.slnx` — before claiming test exclusion is "already the case" or "fixed"
- [ ] If tests are confirmed included in editor compiles, a concrete fix is applied and re-verified; if not, this is documented as a non-issue rather than silently closed
- [ ] Conditional compilation symbols (`ASHFALL_DEBUG`) defined for debug builds and confirmed to compile in both Debug and Release configurations
- [ ] Editor startup/recompile time measurably reduced (actual before/after timing, not an assumption)
- [ ] `godot --headless` selftests still pass
- [ ] No functional regression in Godot host

---

## Step 7 — Measure Improved Build Times & Document

### Goal
Re-run the benchmark from Step 1, compare against baseline, document the improvements, and establish ongoing monitoring.

### Implementation

**Re-run benchmark:**

```bash
./scripts/ci/build-benchmark.sh
# Compare new results against Step 1 baseline
```

**Create performance tracking document** at `docs/build-performance.md`:

```markdown
# Build Performance Tracking

## Baseline (Batch 111, Step 1)
| Measurement | Duration |
|-------------|----------|
| Cold build (tests + Core) | Xms |
| Incremental (no changes) | Xms |
| Incremental (one file) | Xms |
| Test execution only | Xms |
| Godot host cold build | Xms |
| Full 5-step verify | Xms |

## After Optimization (Batch 111, Step 7)
| Measurement | Duration | Improvement |
|-------------|----------|-------------|
| Cold build (tests + Core) | Xms | -Y% |
| Incremental (no changes) | Xms | -Y% |
| Incremental (one file) | Xms | -Y% |
| Test execution only | Xms | -Y% |
| Godot host cold build | Xms | -Y% |
| Full 5-step verify | Xms | -Y% |

## Target (revise after Step 1's real baseline is measured — these are placeholder aspirations, not commitments)
- Incremental build (one file change): meaningfully faster than baseline; a specific "<5s" number is only meaningful once Step 1 produces a real baseline to compare against on the actual dev/CI hardware
- Full verify cycle: meaningfully faster than baseline; "<30s" is aspirational and depends heavily on `godot --headless` startup overhead, which none of the steps in this batch actually optimize (Steps 1-7 only touch `dotnet build`/`dotnet test` paths)
- No-change rebuild: <2 seconds is a reasonable target for Roslyn shared compilation and is the one number in this list that's realistic to commit to in advance
```

**Add build time check to CI** (optional, for regression detection):

```bash
# In scripts/ci/verify-parallel.sh, add timing wrapper:
VERIFY_START=$(date +%s)
# ... existing 5 steps ...
VERIFY_END=$(date +%s)
VERIFY_DURATION=$((VERIFY_END - VERIFY_START))
if [ $VERIFY_DURATION -gt 60 ]; then
  echo "WARNING: Verify cycle took ${VERIFY_DURATION}s (target: <30s)"
fi
```

**Files created/modified:**
- `docs/build-performance.md` — performance tracking
- `scripts/ci/build-benchmark.sh` — updated with comparison logic
- `scripts/ci/verify-parallel.sh` — timing guard added

### Risk & Rollback
- Risk: None — this step only measures and documents; no build configuration changes happen here.
- Rollback: delete `docs/build-performance.md`; revert the timing-guard addition to `verify-parallel.sh`.

### Verification
```bash
# Run full verification to confirm nothing broke
./scripts/ci/verify-parallel.sh

# Run benchmark and compare
./scripts/ci/build-benchmark.sh

# Confirm target met — record the actual measured numbers in docs/build-performance.md,
# do not just check a box without the underlying data
```

### Done when
- [ ] Benchmark re-run with all 6 measurements, real numbers recorded (not placeholders)
- [ ] Improvement percentage calculated from actual before/after numbers — do not assert "50%" unless the arithmetic on real measurements supports it; if the real improvement is smaller, report the real number
- [ ] Full verify cycle time reported honestly, whether or not it beats 30 seconds — if it doesn't, document why (e.g. `godot --headless` startup cost is unaffected by anything in this batch)
- [ ] `docs/build-performance.md` created with before/after comparison using real data
- [ ] CI timing guard in place to catch regressions
- [ ] All 5 canonical verification steps pass, confirmed by an actual run in this session, not assumed from prior steps

---

## Summary Table

| Step | Title | Key Deliverable | Risk | Estimated Effort |
|------|-------|----------------|------|-----------------|
| 1 | Measure Current Build Times | `scripts/ci/build-benchmark.sh` + baseline data | None (but blocked by pre-existing `ActionResult.cs` build failure — see Review Notes) | 30 min |
| 2 | Optimize Project References | `ProduceReferenceAssembly`, `DisableTransitiveProjectReferences` | Low | 1 hour |
| 3 | Add Build Parallelism | `-m` flag, `Directory.Build.rsp`, `verify-parallel.sh` | Low | 1 hour |
| 4 | Split Test Project by Domain | Trait-based filtering (recommended) rather than physical project split | Low-Medium | 2-3 hours |
| 5 | Add Build Caching | Roslyn compiler server, `UseSharedCompilation`, `global.json` | **Medium** (SDK pin can break builds on machines with a different installed SDK — see Step 5 risk note) | 1 hour |
| 6 | Optimize Godot Editor Compilation | Confirm/fix editor build scope, conditional symbols | Low | 1-2 hours |
| 7 | Measure Improved Build Times | `docs/build-performance.md`, CI timing guard | None | 30 min |

**Total estimated effort:** 7-9 hours (add ~30 min if the pre-existing `ActionResult.cs` build failure must be resolved first — see Review Notes; that fix itself is out of scope for this batch but blocks Step 1's baseline measurement)
**Expected outcome:** Reduced incremental build time (specific percentage TBD from real baseline), verify cycle faster than today's baseline, developer iteration improved. Treat "50%" and "<30s" as aspirational until Step 1/Step 7 produce real numbers.

---

## Verification Checklist (Batch 111 Complete)

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # currently FAILS from a clean state (CS0523 in untracked Assets/Ashfall.Core/ActionResult.cs) — resolve before running this batch
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj     # PASS expected once build succeeds — confirm actual test count via --list-tests rather than citing "1941+" from memory
dotnet build Ashfall.csproj                                  # currently FAILS from a clean state, same root cause as above
godot --headless --path . -- --data-integrity-selftest       # PASS (0 errors) — not re-verified in this review; run before relying on it
godot --headless --path . -- --bridge-selftest               # PASS (exit 0) — not re-verified in this review; run before relying on it
```

All build optimizations are infrastructure-only. No game logic changed. No test behavior altered. No save format modified.

---

## Review Notes (Corrected)

This batch was adversarially reviewed against the real repository state on the date of this edit. Corrections made:

1. **Framework/Nullable claims were wrong and have been corrected.** The original draft claimed `Ashfall.Core` targets `netstandard2.1`. It does not — `Ashfall.Core/Ashfall.Core.csproj` targets **`net8.0`** with `<Nullable>enable</Nullable>` already set, confirmed by reading the file directly. `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` is **`net9.0`** with `Nullable enable` and an explicit `NoWarn` list (`xUnit2013;xUnit2020;CS8618;CS8603;CS8600;CS8601;CS8602;CS8604;CS8625`), also already set. `Ashfall.csproj` (Godot host, `Godot.NET.Sdk/4.7.1`) is **`net8.0`** with `Nullable enable` already set. None of these needed retargeting or Nullable enablement as part of this batch — that work was already done before this batch was drafted, and the plan's "Context" table describing a `netstandard2.1`/`net9.0`/`net8.0` split with Core on netstandard was simply incorrect.

2. **A real, currently-failing clean build was discovered and is now documented.** `dotnet clean` followed by `dotnet build` on both `Ashfall.Core.Tests.csproj` and `Ashfall.csproj` fails today with `CS0523: Struct member 'ActionResult.InnerResult' of type 'ActionResult?' causes a cycle in the struct layout` in `Assets/Ashfall.Core/ActionResult.cs`. That file is untracked in git (confirmed via `git status`) and is unrelated to this batch's actual scope, but it directly blocks Step 1's baseline measurement and Step 7's re-measurement, since both require a clean build to succeed. This is flagged rather than silently fixed, per the instruction to keep changes small and reviewable and one system per task — fixing an unrelated struct-layout bug is not build-optimization work and deserves its own reviewed change.

3. **`Directory.Build.props` and `global.json` do not exist yet.** The original draft's Step 2 spoke of "enhancing" `Directory.Build.props`, implying it already existed with some content; it does not exist at all (confirmed via direct file listing). Step 5's `global.json` example pinned SDK `9.0.100`, but only `10.0.302` is installed in this environment (confirmed via `dotnet --list-sdks`) — pinning to an SDK that isn't installed would break every build on this machine. The plan's SDK version has been changed to reflect environment reality, with an explicit instruction to re-verify against each real dev/CI machine before merging, since this review's environment may not match the project's actual build machines.

4. **The Godot host's build graph was mischaracterized.** `Ashfall.csproj` does not have a `ProjectReference` to `Ashfall.Core.csproj` — it compiles `Assets/Ashfall.Core/**/*.cs` directly via a `<Compile Include>` glob (confirmed by reading `Ashfall.csproj`). This means Step 2's `ProduceReferenceAssembly` optimization only speeds up the Tests→Core edge; it does nothing for Godot host rebuilds, since there is no reference boundary there to produce a reference assembly for. The original draft implied this optimization would help "downstream projects" generally, which overstated its effect.

5. **The repo uses `Ashfall.slnx`, not a `.sln` file**, and it groups all three projects (Core, Tests, and the Godot host) together. The original draft hedged with "if there is a `.sln` file" without checking; there is a solution file, just not in the extension the draft assumed, and Step 6 now accounts for it without assuming the Godot editor necessarily reads it for build scoping.

6. **Save-store/test-count figures were unverified and are now flagged as such** rather than asserted as fact (e.g., "1941+ tests", the Unit/Integration test split of "~1600"/"~341"). These numbers were not independently confirmed during this review; Step 1 and Step 4 now instruct the implementer to measure the real numbers rather than trust figures carried over from a prior draft.

7. **Every step now has an explicit Risk & Rollback subsection.** The original draft had a single repo-wide "Risk: Low" line in the header and nothing per-step; for a batch that creates a root-level `Directory.Build.props`, a root-level `Directory.Build.rsp` (both auto-discovered by MSBuild and therefore affecting every build in the repo, not just the ones invoked by this batch's own scripts), and a `global.json` SDK pin (the one change here that can plausibly break other people's builds), per-step rollback guidance was missing and has been added.

8. **Vague Done-when criteria were tightened.** Several criteria such as "Baseline measurements recorded" or "No tests lost in the split" were checkbox-shaped assertions with no defined pass/fail mechanism. These now specify what to actually compare (e.g., arithmetic count checks, `-v detailed` grep output, actual millisecond numbers) rather than accepting a subjective "looks done."
