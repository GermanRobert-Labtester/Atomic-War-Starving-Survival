# ASHFALL — Documentation Source-of-Truth & Authority Map

**Date:** 2026-08-26
**Scope:** Canonical navigation map connecting developers and AI agents to active project truth, engine rules, data authority, verification pipelines, UI coverage, and historical archives.

---

## 1. Non-Negotiable Engine Rules & Architecture

| Area | Primary Authority Document | Key Principles |
|---|---|---|
| **Master Agent Rules** | [`AGENTS.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/AGENTS.md) | **Godot is authoritative.** Unity is fully removed. Core has zero engine references. Verification uses `dotnet` + `godot --headless`. |
| **Migration Status** | [`docs/GODOT_MIGRATION_STATUS.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/GODOT_MIGRATION_STATUS.md) | 100% complete migration record; documents the removal of `Assets/_Game/` and the bridge shim. |
| **Code Index** | [`docs/ASHFALL_CODE_INDEX.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/ASHFALL_CODE_INDEX.md) | Comprehensive map of systems across Core (`Ashfall.Core.*`) and Godot Host (`AtomicWar.GodotApp.*`). |

---

## 2. Code & Data Authority

| Domain | Authority Location | Description & Contract |
|---|---|---|
| **Simulation Core (Truth)** | [`Assets/Ashfall.Core/`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/) | Pure C# (`netstandard2.1`). Zero references to `Godot`, `UnityEngine`, or `JsonUtility`. Deterministic PRNG (`ISeededRng`). |
| **Godot Host (Presentation)** | [`src/`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/) | Godot 4.7+ (.NET 8.0). Thin presentation nodes, UI panels, world views, audio manager, and host CLI. |
| **Data Authority** | [`Assets/StreamingAssets/Data/`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/StreamingAssets/Data/) | Canonical snake_case JSON files across 129 catalogs and 4,793 authored IDs. |
| **Visual Asset Registry** | [`src/Host/AssetRegistry.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/Host/AssetRegistry.cs)<br>[`docs/visual/FALLBACK_VISUAL_ASSETS.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/visual/FALLBACK_VISUAL_ASSETS.md) | Centralized texture resolution, canonical fallbacks (`placeholder_survivor.png`, `icon_placeholder.png`), and procedural generation. |

---

## 3. Verification & Quality Gates

| Gate / Suite | Documentation | Verification Command |
|---|---|---|
| **Unit Test Suite** | [`Ashfall.Core.Tests/`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Ashfall.Core.Tests/) | `dotnet test` (3,333 tests, 0 failures) |
| **Host Build** | [`Ashfall.csproj`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Ashfall.csproj) | `dotnet build Ashfall.csproj` |
| **Data Integrity Gate** | [`Assets/Ashfall.Core/CatalogIntegrityValidator.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/CatalogIntegrityValidator.cs) | `godot --headless --path . -- --data-integrity-selftest` (129 catalogs, 0 errors) |
| **Triad Drift Gate** | [`scripts/ci/triad-drift-gate.sh`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/scripts/ci/triad-drift-gate.sh)<br>[`docs/architecture/TRIAD_GATE_AND_SAVE_OWNERSHIP.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/architecture/TRIAD_GATE_AND_SAVE_OWNERSHIP.md) | `bash scripts/ci/triad-drift-gate.sh` (enforces Setup/Save/Flush parity) |
| **Integration Batch Manifest** | [`docs/CURRENT_INTEGRATION_BATCH.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/CURRENT_INTEGRATION_BATCH.md) | Lists uncommitted working-tree systems, changes, and verification commands. |
| **CI & Gating Map** | [`docs/CI.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/CI.md)<br>[`docs/ci/GATING_VS_DIAGNOSTIC_CHECKS.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/ci/GATING_VS_DIAGNOSTIC_CHECKS.md) | Defines Tier 1 blocking gates vs. Tier 2 quality gates vs. Tier 3 diagnostic tools. |
| **Host CLI Catalog** | [`docs/cli/HOST_CLI_COMMAND_CATALOG.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/cli/HOST_CLI_COMMAND_CATALOG.md) | Generated reference for every CLI verb and flag (regenerated from `--host-help` by `scripts/ci/generate-cli-catalog.sh`; never hand-edited). |
| **Manual QA Playthrough** | [`docs/qa/MANUAL_PLAYTHROUGH_CHECKLIST.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/qa/MANUAL_PLAYTHROUGH_CHECKLIST.md)<br>[`docs/qa/AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/qa/AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md) | Step-by-step Day 1 → Day 2 playthrough, onboarding checklist, and audio/settings recovery matrix. |

---

## 4. UI Surfaces, Visual Assets & Coverage

| Document | Purpose & Scope |
|---|---|
| [`docs/ui/SNAPSHOT_COVERAGE.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/ui/SNAPSHOT_COVERAGE.md) | Status of all 29 automated UI snapshot golden targets rendered at 1280×800. |
| [`docs/ui/TIER3_UI_READINESS.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/ui/TIER3_UI_READINESS.md) | Readiness audit confirming completion of Tier-3 modal atlas panels (Map, Maritime, Muster, Quests, Standing Record, Research). |
| [`docs/ui/SURFACE_GAP_REPORT.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/ui/SURFACE_GAP_REPORT.md) | Inventory of UI surfaces and integration seams across the Godot host. |
| [`docs/visual/FALLBACK_VISUAL_ASSETS.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/visual/FALLBACK_VISUAL_ASSETS.md) | Policy for canonical static fallback textures, procedural generators, and screen consumption. |
| [`docs/visual/DIRECT_GODOT_ASSET_LOADS_AUDIT.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/visual/DIRECT_GODOT_ASSET_LOADS_AUDIT.md) | Audit of direct `GD.Load` and `ResourceLoader.Load` calls in `src/`. |
| [`docs/visual/ASSET_GALLERY.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/visual/ASSET_GALLERY.md) | Visual asset index, on-disk families, and resolution status. |
| [`docs/ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md) | Contributor guide for UI node count telemetry, frame lifecycle, and leak triage. |

---

## 5. Historical Documents (Archival / Do Not Use for Implementation)

The following documents describe superseded Unity architectures, completed port plans, or resolved migration states. They are preserved for history only:

| Historical Document | Historical Context & Current Replacement |
|---|---|
| [`docs/plans/sources.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/plans/sources.md) | Describes early August 2026 Unity/Bridge strangler migration. Current truth is [`AGENTS.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/AGENTS.md). |
| [`docs/gaps/ASHFALL_IMPLEMENTATION_GAP_AUDIT.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/gaps/ASHFALL_IMPLEMENTATION_GAP_AUDIT.md) | Pre-migration gap audit from Unity era; all critical and high items are resolved. |
| [`docs/systems/SKILL_PROGRESSION_CORE_PORT_PLAN.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/systems/SKILL_PROGRESSION_CORE_PORT_PLAN.md) | Historical port plan for `SkillProgressionSystem` (closed; fully ported in Core Phase 18 and tested). |
| [`docs/systems/RESEARCH_CORE_PORT_PLAN.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/systems/RESEARCH_CORE_PORT_PLAN.md) | Historical port plan for `ResearchSystem` (closed; fully ported in Core Phase 28 and tested). |
