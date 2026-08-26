# ASHFALL — Unified Master Plan Index & Living Architecture Matrix

**Canonical Index Path:** `docs/plans/ASHFALL_UNIFIED_MASTER_PLAN_INDEX.md`  
**Target:** `GermanRobert-Labtester/Atomic-War-Starving-Survival` (ASHFALL)  
**Engine Authority:** Godot 4.7+ (.NET/C#)  
**Simulation Authority:** `Assets/Ashfall.Core/` (netstandard2.1/net8.0, 0 engine references)  
**Data Authority:** `Assets/StreamingAssets/Data/` (129 catalogs, 4,793 authored IDs)  
**Status:** ALL ACTIVE PLANS INDEXED, INITIATED, RECONCILED & RE-VERIFIED

---

## 1. Master Plan Hierarchy & Registry

The following authoritative plans govern the architecture, milestones, asset pipelines, and expansion arcs of ASHFALL:

| Plan ID / Canonical Document | Primary Purpose | Scope & Horizon | Active Status |
|---|---|---|---|
| [`docs/plans/ASHFALL_DAY1_TO_DAY2_MAJOR_PLAN.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/plans/ASHFALL_DAY1_TO_DAY2_MAJOR_PLAN.md) | First-session playable vertical slice (Launch → Day 1 → Advance → Day 2 → Continue) | Milestone (WP-01 to WP-09) | **PASS / VERIFIED** (`DAY1_TO_DAY2_SELFTEST PASS`) |
| [`docs/audit/ACTIONABLE_AUDIT_EXECUTION_PLAN.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/audit/ACTIONABLE_AUDIT_EXECUTION_PLAN.md) | Actionable remediation of the 42-section forensic audit and Top 50 Action Items | Forensic / Host Wiring (Phase A–F) | **INITIATED & ACTIVE** (Phases F0-P3, Phase B 2D shelter integrated) |
| [`docs/audit/ASHFALL_COMPREHENSIVE_GAME_AUDIT.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/audit/ASHFALL_COMPREHENSIVE_GAME_AUDIT.md) | Comprehensive 42-section game audit covering all domains, rendering, and persistence | Complete Architectural Audit | **CANONICAL REFERENCE** |
| [`docs/plans/ASHFALL_MAJOR_WORLD_EXPANSION_AND_INTEGRATION_PLAN.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/plans/ASHFALL_MAJOR_WORLD_EXPANSION_AND_INTEGRATION_PLAN.md) | World Consequence Spine, Living Holdfast, Faction War, Combat AI Utility, World Population | Living World Expansion Arc | **INITIATED** |
| [`docs/plans/ASHFALL_FURTHER_NEXT_STEPS_AND_LIVING_WORLD_MILESTONE.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/plans/ASHFALL_FURTHER_NEXT_STEPS_AND_LIVING_WORLD_MILESTONE.md) | Multi-day campaign headless runner, Campaign Director, delayed consequence loops | Campaign Simulation & Balancing | **INITIATED** |
| [`docs/plans/ASHFALL_REALISTIC_NEXT_STEPS_REAUTHORED.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/plans/ASHFALL_REALISTIC_NEXT_STEPS_REAUTHORED.md) | 200-step dependency-ordered roadmap derived from the 10,000-step generated horizon | Core Architecture to Alpha (NS-001–200) | **ACTIVE ROADMAP** |
| [`docs/plans/ASHFALL_GITHUB_INVESTIGATION_50_STEP_ROADMAP.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/plans/ASHFALL_GITHUB_INVESTIGATION_50_STEP_ROADMAP.md) | 50-step repository hardening, CI/CD migration, LFS tracking, and test gates | Repository & CI Engineering | **ACTIVE** |
| [`docs/plans/ASHFALL_Visual_Asset_Pipeline_Plan.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/plans/ASHFALL_Visual_Asset_Pipeline_Plan.md) | Asset production decision ladder, 2D art staging, Godot import settings, Krita/Pixelorama pipeline | Visual & Audio Production | **ACTIVE PIPELINE** |
| [`docs/plans/GAME_CREATION_APPLICATIONS.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/plans/GAME_CREATION_APPLICATIONS.md) | Tool routing registry (Godot, .NET SDK, Krita, Pixelorama, Tiled, ImageMagick, FFmpeg) | Workstation Tool Authority | **CANONICAL ROUTING** |
| [`docs/forensics/CANONICAL_SUBSYSTEM_REGISTRY.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/forensics/CANONICAL_SUBSYSTEM_REGISTRY.md) | 578-component deduplicated inventory across Core (314) and Godot Hosts (264) | Subsystem Inventory | **CANONICAL INVENTORY** |

---

## 2. Global Architectural Invariants

Every agent and developer must maintain these 6 inviolable rules:

1. **Zero Engine Coupling in Core:** `Assets/Ashfall.Core/` contains **0** references to `UnityEngine`, `Godot`, `GodotSharp`, or `JsonUtility`.
2. **Ports and Adapters:** All host interactions are abstracted through `Assets/Ashfall.Core/Ports.cs` (`IJsonSerializer`, `IFileIO`, `ILog`, `IClock`, `ISeededRng`).
3. **Save Integrity & Cross-Host Verification:** Save stores use checksummed envelope schemas (`SaveChecksum`), explicit migration codecs (V1→V2→V3), and fail-safe serialization.
4. **Strict Determinism:** All procedural logic uses `ISeededRng` (`SeededRng`). Never `System.Random`, `Guid.NewGuid()`, or unseeded wall-clock values.
5. **Thin Presentation Nodes:** Nodes in `src/` only handle input, UI rendering, audio playback, and node lifecycle. All game simulation resides in `Ashfall.Core`.
6. **Data Authority in JSON:** `Assets/StreamingAssets/Data/` is the single authority (129 catalogs, snake_case IDs, validated via `CatalogIntegrityValidator`).

---

## 3. Active Verification Gates

| Gate Name | CLI Invocation | Current Status |
|---|---|---|
| **Core Test Suite** | `dotnet test Ashfall.Core.Tests/` | **PASS (3,242 / 3,242 passed)** |
| **Godot Host C# Build** | `dotnet build Ashfall.csproj` | **PASS (0 errors)** |
| **Day 1 → Day 2 Milestone** | `godot --headless -- --day1-to-day2-selftest` | **PASS (`DAY1_TO_DAY2_SELFTEST PASS`)** |
| **Expansions 01–10 Gate** | `godot --headless -- --expansions-selftest` | **PASS (`ALL EXPANSIONS GREEN 01–10`)** |
| **Shelter Operations Gate** | `godot --headless -- --shelter-operations-selftest` | **PASS** |
| **Data Integrity Gate** | `godot --headless -- --data-integrity-selftest` | **PASS (0 findings across 129 catalogs)** |
| **Triad Drift Gate** | `./scripts/ci/triad-drift-gate.sh` | **GATE PASS (66 Setups, 61 Saves, 60 Sections)** |
| **UI Layout Matrix** | `godot --headless -- --ui-layout-selftest` | **PASS (8/8 resolutions passed)** |
| **Audio Pipeline** | `godot --headless -- --audio-selftest` | **PASS (141/141 passed)** |
