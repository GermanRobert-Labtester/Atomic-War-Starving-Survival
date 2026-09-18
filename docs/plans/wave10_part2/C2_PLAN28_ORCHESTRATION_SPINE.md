# Plan 28 (C2[9]) Reconciliation & Implementation Log: "The Orchestration Spine"

**Package:** Wave 10 Part 2 — Task C2\
**Plan Authority:** `C-integration-plans/C2_planintegration[9].md` (Plan 28)\
**Status:** **DELTA-SEALED / PROMOTED-REMAINDER**\
**Date:** 2026-09-17\
**Integrator:** Antigravity\

---

## 1. Executive Intent & Architecture

Plan 28 ("The Orchestration Spine: Registration You Cannot Forget") targets human-memory orchestration defects where adding a subsystem requires remembering disjoint registration sites across `Setup*`, `Save*`, `Flush*`, day-owner advances, panel routing, and lifecycle hooks.

Per Wave 10 Part 2 §6:
1. **Clauses Sealed Elsewhere:**
   - Wave 2 decomposition and Wave 7 loader/event/clock consolidation decomposed the monolithic `Main.cs` into structured partials (`Main.SaveOrchestrator.cs`, `Main.CampaignOwners.cs`, `Main.GameFlow.cs`, etc.).
   - `SaveSectionRegistry.cs` acts as the declarative authority for save sections (`SaveSectionMetadata`).
   - `MainTriadDriftGateTests.cs` enforces `Setup*` ↔ `Save*` ↔ `SaveAll` triad parity.
   - `CampaignDayCoordinator.cs` enforces deterministic day-owner registration and execution order.
   - `PanelRegistryBootstrap.cs` enforces declarative UI surface registration.
2. **Current Executed Delta (28A):**
   - Implemented `Assets/Ashfall.Core/Orchestration/SubsystemManifest.cs` declaring `SubsystemDescriptor` unifying identity, `LifecyclePhase`, `SaveSectionKey`, `DayOwnerId`, and `PrimaryPanelRoute`.
   - Verified cross-reference integrity with `SaveSectionRegistry` and `PanelRegistry` in `Ashfall.Core.Tests/Orchestration/SubsystemManifestTests.cs` (6/6 PASS).
   - Validated triad drift gate `MainTriadDriftGateTests.cs` (7/7 PASS).
3. **Promoted Remainder (28B/28C Wide Migration):**
   - Mechanically moving dozens of `Setup*` invocations inside `Main.cs` to be dynamically iterated by `SubsystemManifest` is promoted as a separate dedicated refactoring package per §6.10, ensuring zero runtime regressions or session rebind breakage.

---

## 2. Clause Matrix

| Clause | Historical Requirement | Status Before Wave 10 | Executed Delta | Final Status | Evidence |
|---|---|---|---|---|---|
| **28A.1 Subsystem Descriptor** | Unified metadata linking setup, save, day tick, and UI | Split across separate registries | Implemented `SubsystemDescriptor` record in `Assets/Ashfall.Core/Orchestration/SubsystemManifest.cs` | **SEALED** | `SubsystemManifestTests.cs` |
| **28A.2 Cross-Registry Parity** | Subsystem declarations match save sections and panel routes | Enforced by disparate ad-hoc test gates | Cross-referenced against `SaveSectionRegistry` and `PanelRegistry` | **SEALED** | `All_SaveSectionKeys_ExistInSaveSectionRegistry`, `All_PrimaryPanelRoutes_ExistInPanelRegistry` |
| **28B File Decomposition** | Split large host files along subsystem seams | Decomposed in Wave 2/Wave 7 partials | Existing partials maintained; no behavioral churn | **SEALED-ELSEWHERE** | `src/Main.*.cs` partial files |
| **28C Lifecycle Formalization** | Universal constructor iteration via manifest | Disjoint `Setup*` calls in `Main` | Promoted to dedicated wide migration queue package per §6.10 | **PROMOTED-TO-QUEUE** | Promotion queue record below |

---

## 3. Promoted Migration Package: `QUEUE-PLAN28-MAIN-CONSTRUCTOR-MIGRATION`

- **Target Paths:** `src/Main.*.cs` setup methods.
- **Intended Manifest Owner:** `SubsystemManifest` in Core + `Main` bootstrap loop.
- **Migration Strategy:** Staged migration of `Setup*` calls into descriptor `SetupAction` delegates, gated by `MainTriadDriftGateTests`.
- **Parity Gate:** `godot --headless --path . -- --player-panels-uitest` and `--7-day-smoke-selftest`.

Task C2 is **DELTA-SEALED**.
