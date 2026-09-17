# ASHFALL — Documentation Source-of-Truth & Authority Map

**Date:** 2026-08-26
**Scope:** Canonical navigation map connecting developers and AI agents to active project truth, engine rules, data authority, verification pipelines, UI coverage, and historical archives.

---

## 1. Non-Negotiable Engine Rules & Architecture

| Area | Primary Authority Document | Key Principles |
|---|---|---|
| **Master Agent Rules** | [`AGENTS.md`](../AGENTS.md) | Compact universal rules: Godot authority, engine-free Core, data/save/determinism ownership, focused verification, and secret safety. |
| **AI Foreman Workflow** | [`AI_AGENT_WORKFLOW.md`](../AI_AGENT_WORKFLOW.md)<br>[`INTEGRATION_PLANS.md`](../INTEGRATION_PLANS.md)<br>[`WORKTREE_OWNERSHIP.md`](../WORKTREE_OWNERSHIP.md) | Current batch, exact path ownership, role routing, handoffs, cheap read-only sweeps, and integration acceptance. |
| **Test and Debt Policy** | [`TEST_POLICY.md`](../TEST_POLICY.md)<br>[`KNOWN_DEBT.md`](../KNOWN_DEBT.md) | Targeted test selection, aggregation/quarantine rules, and the current accepted/blocked/retired debt register. |
| **Migration Status** | [`docs/GODOT_MIGRATION_STATUS.md`](GODOT_MIGRATION_STATUS.md) | 100% complete migration record; documents the removal of `Assets/_Game/` and the bridge shim. |
| **Code Index** | [`docs/ASHFALL_CODE_INDEX.md`](ASHFALL_CODE_INDEX.md) | Comprehensive map of systems across Core (`Ashfall.Core.*`) and Godot Host (`AtomicWar.GodotApp.*`). |

---

## 2. Code & Data Authority

| Domain | Authority Location | Description & Contract |
|---|---|---|
| **Simulation Core (Truth)** | [`Assets/Ashfall.Core/`](../Assets/Ashfall.Core) | Pure C# (`netstandard2.1`). Zero references to `Godot`, `UnityEngine`, or `JsonUtility`. Deterministic PRNG (`ISeededRng`). |
| **Godot Host (Presentation)** | [`src/`](../src) | Godot 4.7+ (.NET 8.0). Thin presentation nodes, UI panels, world views, audio manager, and host CLI. |
| **Data Authority** | [`Assets/StreamingAssets/Data/`](../Assets/StreamingAssets/Data) | Canonical snake_case JSON files across 129 catalogs and 4,793 authored IDs. |
| **Visual Asset Registry** | [`src/Host/AssetRegistry.cs`](../src/Host/AssetRegistry.cs)<br>[`docs/visual/FALLBACK_VISUAL_ASSETS.md`](visual/FALLBACK_VISUAL_ASSETS.md) | Centralized texture resolution, canonical fallbacks (`placeholder_survivor.png`, `icon_placeholder.png`), and procedural generation. |

---

## 3. Verification & Quality Gates

| Gate / Suite | Documentation | Verification Command |
|---|---|---|
| **Fast-Tier Local Runner** | [`scripts/ci/verify-fast.sh`](../scripts/ci/verify-fast.sh)<br>[`docs/CI.md`](CI.md) | `bash scripts/ci/verify-fast.sh` (mirrors all 14 CI gates in order, fails fast) |
| **Focused Unit Tests** | [`Ashfall.Core.Tests/`](../Ashfall.Core.Tests)<br>[`scripts/run_test.sh`](../scripts/run_test.sh) | `bash scripts/run_test.sh <file-or-focused-directory>`; no default full-suite run. |
| **Host Build** | [`Ashfall.csproj`](../Ashfall.csproj) | `dotnet build Ashfall.csproj` |
| **Data Integrity Gate** | [`Assets/Ashfall.Core/CatalogIntegrityValidator.cs`](../Assets/Ashfall.Core/CatalogIntegrityValidator.cs) | `godot --headless --path . -- --data-integrity-selftest` (129 catalogs, 0 errors) |
| **Triad Drift Gate** | [`scripts/ci/triad-drift-gate.sh`](../scripts/ci/triad-drift-gate.sh)<br>[`docs/architecture/TRIAD_GATE_AND_SAVE_OWNERSHIP.md`](architecture/TRIAD_GATE_AND_SAVE_OWNERSHIP.md) | `bash scripts/ci/triad-drift-gate.sh` (enforces Setup/Save/Flush parity) |
| **Integration Batch Manifest** | [`INTEGRATION_PLANS.md`](../INTEGRATION_PLANS.md) | Sole live batch ledger: premise, dependency order, owner, acceptance, and focused verification. |
| **CI & Gating Map** | [`docs/CI.md`](CI.md)<br>[`docs/ci/GATING_VS_DIAGNOSTIC_CHECKS.md`](ci/GATING_VS_DIAGNOSTIC_CHECKS.md) | Defines Tier 1 blocking gates vs. Tier 2 quality gates vs. Tier 3 diagnostic tools. |
| **Host CLI Catalog** | [`docs/cli/HOST_CLI_COMMAND_CATALOG.md`](cli/HOST_CLI_COMMAND_CATALOG.md) | Generated reference for every CLI verb and flag (regenerated from `--host-help` by `scripts/ci/generate-cli-catalog.sh`; never hand-edited). |
| **Save-Store Contract Matrix** | [`docs/saves/SAVE_STORE_CONTRACT_MATRIX.md`](saves/SAVE_STORE_CONTRACT_MATRIX.md) | Generated completeness authority for all 62 save store classes, checksum envelopes, and slot-root isolation (`scripts/ci/generate-save-store-matrix.sh --check`). |
| **Manual QA Playthrough** | [`docs/qa/MANUAL_PLAYTHROUGH_CHECKLIST.md`](qa/MANUAL_PLAYTHROUGH_CHECKLIST.md)<br>[`docs/qa/AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md`](qa/AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md) | Step-by-step Day 1 → Day 2 playthrough, onboarding checklist, and audio/settings recovery matrix. |

---

## 4. UI Surfaces, Visual Assets & Coverage

| Document | Purpose & Scope |
|---|---|
| [`docs/ui/SNAPSHOT_COVERAGE.md`](ui/SNAPSHOT_COVERAGE.md) | Status of all 29 automated UI snapshot golden targets rendered at 1280×800. |
| [`docs/ui/TIER3_UI_READINESS.md`](ui/TIER3_UI_READINESS.md) | Readiness audit confirming completion of Tier-3 modal atlas panels (Map, Maritime, Muster, Quests, Standing Record, Research). |
| [`docs/ui/SURFACE_GAP_REPORT.md`](ui/SURFACE_GAP_REPORT.md) | Inventory of UI surfaces and integration seams across the Godot host. |
| [`docs/visual/FALLBACK_VISUAL_ASSETS.md`](visual/FALLBACK_VISUAL_ASSETS.md) | Policy for canonical static fallback textures, procedural generators, and screen consumption. |
| [`docs/visual/DIRECT_GODOT_ASSET_LOADS_AUDIT.md`](visual/DIRECT_GODOT_ASSET_LOADS_AUDIT.md) | Audit of direct `GD.Load` and `ResourceLoader.Load` calls in `src/`. |
| [`docs/visual/ASSET_GALLERY.md`](visual/ASSET_GALLERY.md) | Visual asset index, on-disk families, and resolution status. |
| [`docs/ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md`](ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md) | Contributor guide for UI node count telemetry, frame lifecycle, and leak triage. |

---

## 5. Historical Documents (Archival / Do Not Use for Implementation)

The following documents describe superseded Unity architectures, completed port plans, or resolved migration states. They are preserved for history only:

| Historical Document | Historical Context & Current Replacement |
|---|---|
| Missing legacy review reports | `REPO_REVIEW_REPORT.md` and `COMPREHENSIVE_GAME_AUDIT.md` are not present in this checkout. Do not treat references to them as active task prerequisites; use the current authority map and source evidence. |
| [`docs/plans/sources.md`](plans/sources.md) | Describes early August 2026 Unity/Bridge strangler migration. Current truth is [`AGENTS.md`](../AGENTS.md). |
| [`docs/gaps/ASHFALL_IMPLEMENTATION_GAP_AUDIT.md`](gaps/ASHFALL_IMPLEMENTATION_GAP_AUDIT.md) | Pre-migration gap audit from Unity era; all critical and high items are resolved. |
| [`docs/systems/SKILL_PROGRESSION_CORE_PORT_PLAN.md`](systems/SKILL_PROGRESSION_CORE_PORT_PLAN.md) | Historical port plan for `SkillProgressionSystem` (closed; fully ported in Core Phase 18 and tested). |
| [`docs/systems/RESEARCH_CORE_PORT_PLAN.md`](systems/RESEARCH_CORE_PORT_PLAN.md) | Historical port plan for `ResearchSystem` (closed; fully ported in Core Phase 28 and tested). |

---

## 6. Continuity Wave 1 Metrics & Plan Integration Authority (Plans 15–19)

**Wave Scope:** Closure of Continuity Wave 1 across Plans 15A/15B, 16A/16B/16C, 17A/17B/17C, 18A/18B/18C, and 19A/19B/19C (`docs/plans/C1_planintegration[3].md`).

| Metric | Prior Baseline | Current Measured Truth | Gate / Authority |
|---|---:|---:|---|
| **Hardcoded Epilogue Inputs** | 2 routes (game-over + UI) | **0** | `EpilogueContextFactory.cs` (INV-19.1) |
| **Unreachable Matrix Branches** | >0 (literals prevented reach) | **0** (32/32 reachable) | `Plan19EndingContinuityTests.cs` (INV-19.4) |
| **Dangling `.cs.uid` Sidecars** | 14 dangling | **0** | `scripts/ci/uid-sidecar-gate.sh` (INV-19.6) |
| **Compiler Warnings (net8/net9)** | Baseline warnings | **0** across all 3 targets | `scripts/ci/warning-baseline-gate.sh` |
| **Fast CI Gate Suite** | Baseline | **47/47 PASS** cleanly | `scripts/ci/verify-fast.sh` |
| **Cohort Continuity Links** | Underconnected | **3** (rations, schooling/work, ending) | `Plan19CohortContinuityTests.cs` (INV-19.5) |
| **Session Continuity Journey** | Absent | **PASS** | `Plan19SessionContinuityJourneyTests.cs` |
| **Content Utilization Runtime Events** | Baseline | **1563 events** | `godot --headless -- --content-utilization-selftest` |
| **Exempt Catalogs** | Baseline | **4 (0 stale, 0 invalid)** | `artifacts/content-utilization.json` |
| **Scene-Backed Live Panels** | 22 documented | **22 verified** | `scripts/ci/generate-ui-panel-catalog.py` |

### Plans 15–19 Authority Documents

| Plan | Primary Contract Document | Authority Artefacts & Gates |
|---|---|---|
| **Plan 15 (15A/15B/15C)** | [`docs/plans/C1_planintegration.md`](plans/C1_planintegration.md) | `Assets/Ashfall.Core/Economy/` · `TradeEmbargoSystem.cs` · `RegionalPriceAtlas.cs` |
| **Plan 16 (16A/16B/16C)** | [`docs/plans/C1_planintegration.md`](plans/C1_planintegration.md) | `TravelingCaravanSystem.cs` · `WaystationNetworkSystem.cs` |
| **Plan 17 (17A/17B/17C)** | [`docs/plans/C1_planintegration[2].md`](plans/C1_planintegration[2].md) | Panel Bind/Unbind/Rebind Lifecycle · `PlayerPanelsUiTest` |
| **Plan 18 (18A/18B/18C)** | [`docs/plans/C1_planintegration[2].md`](plans/C1_planintegration[2].md) | Honest Navigation · Purity Gates · Route & Session Persistence |
| **Plan 19 (19A/19B/19C)** | [`docs/plans/C1_planintegration[3].md`](plans/C1_planintegration[3].md) | Ending Continuity · Generational State · Repository Truth · `EpilogueContextFactory.cs` |
| **Plan 22 (22A/22B/22C)** | [`docs/plans/C1_planintegration[4].md`](plans/C1_planintegration[4].md) | One Food Authority · Eating, Meals, Medicine & Pantry Truth · `KitchenNutritionSystem.cs` |
