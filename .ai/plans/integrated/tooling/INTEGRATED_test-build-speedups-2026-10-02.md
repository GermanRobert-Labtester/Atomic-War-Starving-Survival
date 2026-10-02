# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Build & test speedups — 5 tasks

> **STATUS: APPROVED BY USER**

User-directed ("find 5 tasks that can help speed up building and testing the game
overall test suite faster usage! ... and implement!"). All five are
behaviour-preserving tooling/CI changes; no gameplay, Core, save, or determinism
change.

## Bounded outcome

Land five evidence-based improvements to the build/test path:

1. **Focused-test selector coverage.** `tools/gotools/pkg/selector/selector.go`
   only maps test files, `Assets/Ashfall.Core/**`, and `tools/**/*.py`; host
   (`src/**`), l10n (`assets/l10n/**`), and gate (`scripts/ci/**`) changes map to
   nothing, so `bin/run-scoped-tests` prints "No affected test targets mapped"
   and the developer either skips tests or runs the whole suite. Add mappings for
   host/data/tooling changes to the closest existing xUnit directory.
2. **Gate manifest reuses the test build.** `ui_panel_contracts_test`,
   `audio_cue_integrity_gate`, `campaign_envelope_fuzz_test`, `save_support_window`,
   and `test_core_suite` each re-run MSBuild. Declare `depends_on:
   ["build_core_tests"]` and run them with `--no-build`, so one build feeds all.
3. **CI NuGet caching.** `.github/workflows/ci.yml` and `build.yml` call
   `actions/setup-dotnet@v4` without `cache:`, so every run re-downloads packages.
   Enable the built-in NuGet cache.
4. **Opt-in fast local compilation.** `UseSharedCompilation=false` +
   `NodeReuse=false` are hard-coded, so every local build starts a fresh Roslyn
   server. Add an env-gated (`ASHFALL_BUILD_FAST=1`) override and export it from
   the local test/gate runners; CI stays isolated.
5. **Parallel light gates.** `scripts/ci/run-gates.py` runs all gates serially.
   Add an opt-in `--jobs N` that delegates non-build/non-Godot gates to the
   existing Go task runner while keeping build/Godot gates on the serial spine
   (default `1` = today's behaviour).

## Exact files

- `tools/gotools/pkg/selector/selector.go` (new mappings)
- `tools/gotools/pkg/selector/selector_test.go` (new focused test)
- `bin/run-scoped-tests`, `bin/ashfall-dev` (rebuild)
- `docs/ci/CI_GATE_MANIFEST.json` (5 gate entries)
- `docs/ci/GATE_INVENTORY.md` (regenerated)
- `docs/INDEX.md` (regenerated)
- `.github/workflows/ci.yml`, `.github/workflows/build.yml`
- `Directory.Build.targets` (new), `scripts/run_test.sh`, `scripts/ci/verify-fast.sh`
- `scripts/ci/run-gates.py`
- `tools/gotools/pkg/runner/runner.go`, `tools/gotools/cmd/ashfall-dev/main.go`
- `packages.lock.json`, `Ashfall.Core/packages.lock.json`,
  `Ashfall.Core.Tests/packages.lock.json`
- `.ai/plans/`, `.ai/state.md`, `WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md`

## Non-goals

- No engine/Core/gameplay/save/determinism change; no full test suite.
- No test parallelization change (`AssemblyInfo.cs` stays serialized).
- No new authoritative gate; no re-baselining of existing gates.

## Acceptance

- `go build -C tools/gotools ./...` and `go vet -C tools/gotools ./...` clean;
  `go test -C tools/gotools ./pkg/selector/ ./pkg/scopedtest/` pass.
- `bin/run-scoped-tests -dry-run` maps a `src/UI/*.cs` change to xUnit targets.
- `run-gates.py --check-only` and `--check-inventory` pass; `GATE_INVENTORY.md`
  regenerated; `--explain test_core_suite` shows the new dependency.
- `run-gates.py --jobs 1` output unchanged; `--jobs 4` on light gates passes.
- `Directory.Build.targets` parses; `dotnet build Ashfall.Core.Tests/...` clean
  with and without `ASHFALL_BUILD_FAST=1`.
