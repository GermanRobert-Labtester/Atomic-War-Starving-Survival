# ASHFALL — Continuous Integration & Verification Guide

**Authoritative host/engine:** Godot 4.7+ (.NET / C#) (`project.godot`)
**Primary CI Workflow:** `.github/workflows/ci.yml` (Canonical `dotnet` + `godot --headless` gate)
**Target Frameworks:** `netstandard2.1` (Core), `net8.0` (Godot host `Ashfall.csproj` & `Ashfall.Core.csproj`), `net9.0` (Tests `Ashfall.Core.Tests.csproj`)
**Workspace SDK Config:** `global.json` (`version: 8.0.100`, `rollForward: latestMajor`)

---

## Canonical CI Pipeline (Active)

Per `AGENTS.md`, all verification uses **`dotnet` + `godot --headless`**. The canonical GitHub Actions workflow (`.github/workflows/ci.yml`) executes the following stages on every push and PR:

1. **Trailing Whitespace Gate:** `bash scripts/ci/no-whitespace-churn.sh` (fast-fails on trailing whitespace or whitespace errors).
2. **JSON Syntax & Schema Policy Gate:** `bash scripts/ci/json-schema-policy-gate.sh` (fast-fails on invalid JSON, bare array roots, or missing/invalid `schema_version` declarations).
3. **Build Core & Tests:** `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --nologo`
4. **Run Test Suite:** `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --nologo` (all tests passing / 0 failed)
5. **Build Godot Host:** `dotnet build Ashfall.csproj --nologo` (0 errors)
6. **Data Integrity Gate:** `godot --headless --path . -- --data-integrity-selftest` (0 errors across 129 catalogs, 4,794 authored IDs)
7. **Bridge Removal Gate:** `godot --headless --path . -- --bridge-selftest` (verifies shim removal notice & exit 0)
8. **Asset Registry Gate:** `godot --headless --path . -- --asset-registry-selftest` (verifies catalog IDs resolve to real textures under `assets/`)
9. **Player Panels UI Test:** `godot --headless --path . -- --player-panels-uitest` (binds and renders Survivors, Medical, Weather, Radio, Shelter, Status, Tutorial, Afflictions, Radiation panels)
10. **Save Store & Failure-Path Suite:**
    - `godot --headless --path . -- --save-load-ui-failure-selftest` (verifies missing, corrupt, tampered saves show recoverable error messages and preserve live session)
    - `godot --headless --path . -- --holdfast-save-selftest` (Holdfast S1 round-trip and tamper rejection)
    - `godot --headless --path . -- --inventory-save-selftest` (Inventory serialization and checksum verification)
    - `godot --headless --path . -- --journal-save-selftest` (Journal entry ordering, serialization, and persistence)
11. **Deterministic Campaign Smoke:**
    - `godot --headless --path . -- --playable-shell-selftest` (Playable shell, multi-day loop, bunker upgrades, greenhouse planting, save/continue flow)
    - `godot --headless --path . -- --day1-selftest` (Day 1 onboarding, needs decay, triage, bunker fortification, radio protocols)
12. **Expansions Completeness:** `godot --headless --path . -- --expansions-selftest` (all expansions 01–10 + Verdict chain)
13. **Triad Drift Gate:** `bash scripts/ci/triad-drift-gate.sh` (enforces Setup/Save/AllSaveSections parity against declarative `SaveSectionRegistry.cs`)
14. **CLI Catalog Drift Gate:** `bash scripts/ci/generate-cli-catalog.sh --check` (verifies `docs/cli/HOST_CLI_COMMAND_CATALOG.md` matches live `--host-help` output)
15. **Save-Store Contract Matrix Gate:** `bash scripts/ci/generate-save-store-matrix.sh --check` (verifies all 62 save store classes maintain checksum envelopes and slot-root isolation)
16. **Compiler Warning Baseline Gate:** `bash scripts/ci/warning-baseline-gate.sh` (0 unexpected warnings across all targets)
17. **Master Docs Index Drift Gate:** `python3 scripts/ci/generate-docs-index.py --check` (verifies `docs/INDEX.md` stays in sync with repository docs corpus)

For a detailed distinction between blocking CI gates, domain quality gates, and report-only diagnostic tools, see [`docs/ci/GATING_VS_DIAGNOSTIC_CHECKS.md`](ci/GATING_VS_DIAGNOSTIC_CHECKS.md).

---

## Local Verification Runner

To run the exact ordered sequence of all 17 CI gates locally and stop immediately on the first failure:

```bash
bash scripts/ci/verify-fast.sh
```

---

## Individual Local Verification Commands

Run these granular steps manually if debugging a specific stage:

```bash
# 1. Clean cold build
rm -rf .godot/mono/temp Ashfall.Core/bin Ashfall.Core/obj Ashfall.Core.Tests/bin Ashfall.Core.Tests/obj

# 2. Build and run unit tests
dotnet build Ashfall.csproj                               # expect: 0 errors
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj  # expect: all tests passed / 0 failed

# 3. Headless Godot fast-tier self-tests (~15-20s total)
godot --headless --path . -- --data-integrity-selftest    # expect: PASS (129 catalogs, 0 errors)
godot --headless --path . -- --bridge-selftest            # expect: PASS
godot --headless --path . -- --asset-registry-selftest    # expect: PASS (50/50 critical assets)
godot --headless --path . -- --player-panels-uitest       # expect: PASS (player panels rendered)
godot --headless --path . -- --save-load-ui-failure-selftest # expect: PASS (4/4 failure paths verified)
godot --headless --path . -- --holdfast-save-selftest     # expect: PASS (holdfast save round-trip)
godot --headless --path . -- --inventory-save-selftest    # expect: PASS (inventory save round-trip)
godot --headless --path . -- --journal-save-selftest      # expect: PASS (journal save round-trip)
godot --headless --path . -- --playable-shell-selftest    # expect: PASS (playable loop smoke)
godot --headless --path . -- --day1-selftest              # expect: PASS (day 1 onboarding smoke)
godot --headless --path . -- --expansions-selftest        # expect: PASS (all expansions 01-10)

# 4. Triad drift gate
bash scripts/ci/triad-drift-gate.sh

# 5. CLI catalog drift gate (regenerate docs with: bash scripts/ci/generate-cli-catalog.sh)
bash scripts/ci/generate-cli-catalog.sh --check
```

### Current Cold-Build Trust Signal (2026-08-27)

- **`dotnet build`**: 0 errors
- **`dotnet test`**: **all tests passed / 0 failed** (0 skipped)
- **`--data-integrity-selftest`**: 0 errors, 0 warnings across **129 catalogs** (4,794 IDs authored)
- **`--expansions-selftest`**: All expansion suites pass (01–10)
- **All host CLI flags**: Documented in `--host-help`; the command catalog at `docs/cli/HOST_CLI_COMMAND_CATALOG.md` is generated from it (never hand-edited) and kept honest by the CLI catalog drift gate

---

## Fail-Fast Save Restore in CI

`SaveSystem.DefaultFailFastRestoreForEnvironment()` returns **true** in development/test environments.
`GameBootstrap` applies that to `SaveSystem.FailFastRestore` after construction.

| Context | FailFastRestore |
|---------|-----------------|
| Test suite (`dotnet test`) / CI runner | **true** (all-or-nothing save restore) |
| Development player build | **true** |
| Release player build | **false** (best-effort; log and continue) |

---

## Git LFS Policy

`.gitattributes` overrides binary files to plain Git blobs where appropriate. Tracked art and audio are managed according to the LFS gate (`ashfall-lfs-gate`). Verify before adding large binary assets.

---

## Historical Note: Retired Unity CI & Game-CI Secrets

> [!NOTE]
> **RETIRED MIGRATION CONTEXT (Unity 6 / game-ci)**
> The historical Unity game-ci infrastructure that was used prior to the complete migration to Godot has been retired. The Unity host (`Assets/_Game/`), Unity Test Framework (`-runTests -testPlatform EditMode/PlayMode`), and Unity build runners have been fully removed from active CI. No Unity license secrets (`UNITY_LICENSE`, `UNITY_EMAIL`, `UNITY_PASSWORD`) are required for current builds or CI.
