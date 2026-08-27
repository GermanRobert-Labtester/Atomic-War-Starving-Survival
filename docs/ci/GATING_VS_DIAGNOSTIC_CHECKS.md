# ASHFALL — Verification Gates vs. Diagnostic-Only Checks

**Date:** 2026-08-26
**Scope:** Clarifies all automated checks, headless verbs, and CLI commands into **Blocking CI Release Gates**, **Domain Self-Test Quality Gates**, and **Diagnostic / Report-Only Tools** to ensure informational reports are never mistaken for blocking release failures.

---

## 1. Summary Comparison

| Classification | Purpose | Failure Consequence | Examples |
|---|---|---|---|
| **Tier 1: Mandatory CI Release Gates** | Automated pipeline checks in `.github/workflows/ci.yml` | **Blocks PR / Merge** (Non-zero exit code stops build) | `dotnet test`, `--data-integrity-selftest`, `triad-drift-gate.sh` |
| **Tier 2: Domain Self-Test Quality Gates** | Comprehensive headless smoke & assertion batteries | **Blocks Subsystem Release** if assertions fail (Exits 1 on error) | `--expeditions-selftest`, `--day1-to-day2-milestone-selftest`, `--ui-snapshot-uitest` |
| **Tier 3: Informational / Diagnostic Reports** | Telemetry, coverage sweeps, briefings, inspection dumps | **Report-Only (Always Exits 0)** — Informational; does NOT block builds or releases | `--asset-coverage-report`, `--holdfast-briefing`, `--host-help` |

---

## 2. Tier 1: Canonical CI Release Gates (`.github/workflows/ci.yml`)

These automated steps are executed on every pull request and commit to `main`. Every check must pass cleanly (0 errors, 0 warnings where enforced). To run the full ordered gate suite locally and fail fast:
```bash
bash scripts/ci/verify-fast.sh
```

| Gate Step | Command / Script | Coverage & Enforcement Contract |
|---|---|---|
| **1. Fast-Fail JSON Syntax** | `python3 scripts/ci/validate_json.py` (inline) | Parses all 129 JSON catalogs under `Assets/StreamingAssets/Data/` to guarantee valid JSON syntax before compiling code. |
| **2. Core Assembly Build** | `dotnet build Ashfall.Core.Tests/` | Validates compilation of engine-agnostic core and tests (net8.0 / net9.0) with zero engine references (`Invariant 1`). |
| **3. Core Unit Test Suite** | `dotnet test Ashfall.Core.Tests/` | Executes the complete Core unit test suite covering survival needs, radiation math, save codecs, catalog integrity, determinism, and CLI contracts. |
| **4. Godot Host Assembly** | `dotnet build Ashfall.csproj` | Compiles the Godot .NET aggregate host with **0 errors and 0 warnings**. |
| **5. Data Integrity Gate** | `godot --headless -- --data-integrity-selftest` | Cross-references 4,794 authored IDs across 129 catalogs (items, recipes, quests, locations, encounters, survivors, factions, ranges, uniqueness). |
| **6. Bridge Removal Gate** | `godot --headless -- --bridge-selftest` | Asserts removal of legacy `UnityEngine.*` bridge shim; prints confirmation and exits 0. |
| **7. Expansion Gate** | `godot --headless -- --expansions-selftest` | End-to-end verification of all 7 core expansion state machines (Holdfast, Duty Roster, Standing Record, Crossing, Arbitration, LedgerDebt, Greenhouse). |
| **8. Triad Drift Gate** | `bash scripts/ci/triad-drift-gate.sh` | Ensures every `SetupXxx` subsystem in `Main.cs` has an exact corresponding `SaveXxx` method and registration in `AllSaveSections`. Documented exceptions and save ownership are detailed in [`docs/architecture/TRIAD_GATE_AND_SAVE_OWNERSHIP.md`](../architecture/TRIAD_GATE_AND_SAVE_OWNERSHIP.md). |
| **9. CLI Catalog Drift Gate** | `bash scripts/ci/generate-cli-catalog.sh --check` | Regenerates the host CLI command catalog from live `--host-help` output and fails if `docs/cli/HOST_CLI_COMMAND_CATALOG.md` is out of date. |

---

## 3. Tier 2: Domain Self-Test Quality Gates (Pre-Merge Verification)

These CLI commands execute deep domain and UI simulation passes. They use assertions and return exit code 1 if any condition fails:

| Command Flag | Owning Subsystem | Verification Scope |
|---|---|---|
| `--day1-to-day2-milestone-selftest` | Gameplay Loop | Validates Day 1 onboarding, needs decay, shelter fortification, triage decisions, and overnight transition to Day 2. |
| `--player-panels-uitest` | UI Presentation | Validates that all 15 player-reachable HUD panels construct and bind to live host sessions. |
| `--ui-snapshot-uitest` | Visual Regression | Evaluates 29 golden UI snapshot panel renders against approved hash targets. |
| `--save-store-checksum-selftest` | Persistence | Verifies checksummed envelope serialization and legacy bare-state migration across all save stores. |
| `--asset-registry-selftest` | Asset Resolution | Asserts that referenced items, portraits, and locations resolve to real disk assets or canonical fallback textures; also gates missing-asset warning de-duplication by category × id (same id re-queried logs once per category; same id in a different category logs separately). |
| `--combat-selftest` | Combat Domain | Tactical combat ballistics, weapon wear, determinism, and battle state persistence. |
| `--economy-selftest` | Economy Domain | Pricing models, demand clamps, whole-unit barter math, and market save roundtrips. |
| `--expedition-selftest` | Expeditions | Sortie deployment, stamina collapse, loot generation, and encounter choices. |
| `--radio-selftest` | Radio Domain | Tuner frequencies, broadcast intercept queues, and played-transmission deduplication. |
| `--weather-save-selftest` | Weather Domain | Microclimate forecasts, ash storm spikes, radon accumulation, and save integrity. |

---

## 4. Tier 3: Diagnostic & Report-Only Tools (Non-Blocking)

> [!IMPORTANT]
> The following tools are **diagnostic and informational only**. They are designed to assist developers and artists with asset tracking and inspection. **They do NOT fail builds or block release pipelines.**

| Tool Flag | Purpose & Output | Why It Is Non-Blocking |
|---|---|---|
| **`--asset-coverage-report`** | Performs a full sweep comparing all authored catalog IDs against texture files on disk. Summarizes total art coverage percentage and lists missing assets by category. | Many optional lore items and future expansion goods intentionally use centralized procedural generators or fallback textures (`placeholder_survivor.png`, `icon_placeholder.png`) until production art is delivered. |
| **`--holdfast-briefing`** | Prints location inventories and text briefings for all active Holdfast questlines to standard output. | Informational narrative inspection tool; contains no assertions. |
| **`--ice-road-tick-demo`** | Runs a 30-day simulation of ice road supply lines and prints progress metrics. | Demonstration / sandbox tool for reviewing pacing and supply curves. |
| **`--host-help` / `--help`** | Displays documentation and usage instructions for all available CLI flags and verbs. | Standard command-line documentation interface. |
| **`--ui-snapshot-regenerate`** | Captures and writes fresh PNG snapshots to disk for developer review. | Generation tool; does not perform pass/fail diff assertions. |
| **`ASHFALL_UI_NODE_DIAGNOSTICS=1`** (env var, not a CLI flag) | Reports live UI node counts (tree nodes, Control nodes, live Godot objects) before and after each panel test block in `--player-panels-uitest`; flags any node-count growth across a panel's open→close cycle as a suspected node leak. Composable with any uitest run; opt-in only. | Purely informational per-panel leak triage; never changes a uitest verdict or exit code. Full contributor triage guide: [`docs/ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md`](../ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md). |

---

## 5. Summary Policy for Contributors

1. **Never block release on Tier 3 reports:** An incomplete asset in `--asset-coverage-report` is expected during active content authoring and is handled gracefully by runtime fallbacks.
2. **Never ignore Tier 1 or Tier 2 failures:** Any failure in `dotnet test`, `--data-integrity-selftest`, or domain self-tests indicates a real logic defect, save corruption risk, or broken reference that must be resolved prior to merge.
3. **UI Leak Diagnostics vs Functional Pass:** Passing UI tests (`player_panels_uitest PASS`) verify behavioral correctness. Deferred deletion (`QueueFree`), single-frame test loops, and first-open caching can emit `NODE LEAK SUSPECT` telemetry; follow [`docs/ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md`](../ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md) for leak debt triage.
