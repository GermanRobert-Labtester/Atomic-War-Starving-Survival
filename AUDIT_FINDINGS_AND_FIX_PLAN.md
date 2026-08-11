# ASHFALL Deep Code Audit — Findings & Fix Plan

**Audit Date:** 2026-08-10
**Sweeps:** 7 parallel read-only audits across Architecture, Null Safety, Event Bus/Lifecycle, Save/Load, UI System, Test Coverage, Performance.

---

## Executive Summary

The codebase is **well-engineered** in several key areas: atomic save writes with checksum verification, event-driven UI with fingerprint-based change detection, disciplined dictionary access via `TryGetValue`, no `FindObjectOfType`/`Camera.main` in hot paths, zero coroutines (eliminating an entire class of leak), and a strong test suite (~2,036 methods). However, several critical issues were found that should be addressed before shipping.

**Total findings: 52** — 4 Critical, 15 High, 20 Medium, 13 Low.

---

## CRITICAL Findings (Fix Before Shipping)

### C-1: ~60+ Anonymous Lambda Event Subscriptions Never Unsubscribed
**Sweep:** Event Bus & Lifecycle  
**Files:** `GameBootstrap.InitLate.cs`, `InitLate.Radio.cs`, `InitFoundation.cs`, `BunkerSocial.cs`, `InitWorld.Narrative.cs`, `InitWorld.Diary.cs`, `UiActions.Hud.cs`, `UiActions.Radio.cs`

~60+ anonymous lambdas are subscribed to long-lived system events (`+=`) but never unsubscribed (`-=`). On scene reload or "new game," the old `GameBootstrap` cannot be garbage-collected because lambda closures capture `this`. Handler duplication causes double journal entries, double narrative raises, etc. Memory grows unbounded across play sessions in the editor.

**Fix:** Introduce a `SubscriptionBag` helper class that records `(object source, Delegate handler)` pairs and provides a single `DisposeAll()` call. Replace all anonymous lambdas in the 8 affected partial files with bag-tracked subscriptions. Call `bag.DisposeAll()` from `OnDestroy`.

**Estimated scope:** ~60 subscription sites across 8 files + 1 new helper class.

---

### C-2: Iron Man Save Deletion Reports Success Even When Deletion Fails
**Sweep:** Null Safety  
**File:** `Assets/_Game/Core/Mode_IronMan.cs` (lines 105, 118)

Bare `catch { }` blocks around memorial write and save deletion. If save deletion fails (file locked, permissions), `_state.save_deleted = true` is still set and `OnSaveDeleted` fires — telling the UI the save was deleted when it wasn't. This defeats the entire Iron Man mode.

**Fix:** Move `_state.save_deleted = true` and `OnSaveDeleted?.Invoke()` inside the try block (after successful deletion). In the catch, set `_state.save_deleted = false` and log the error. Add a test verifying that a failed deletion is reported as a failure.

**Estimated scope:** 1 file, ~10 lines changed + 1 test.

---

### C-3: GameBootstrap Is a 13,757-Line God Class with 602 Public Properties
**Sweep:** Architecture  
**Files:** 58 partial class files under `Assets/_Game/Core/`

Every system in the game is exposed as a public property on a single MonoBehaviour. Adding any new system requires modifying GameBootstrap in 4+ places (constructor, property, registry registration, save wiring, HUD wiring, tick registration, OnDestroy cleanup). This is the root cause of most coupling issues.

**Fix:** This is a long-term architectural effort (see Fix Plan Phase 4 below). Immediate mitigation: stop adding new properties; use `SystemRegistry.Get<T>()` for new systems.

---

### C-4: EMP Event Has Zero Test Coverage
**Sweep:** Test Coverage  
**Files:** Production: `EMPEvent.cs`, `Weather_EMPStorm.cs`. Tests: none.

The spec explicitly lists "EMP/electronics failure" as a core hazard. No test verifies that EMP disables shelter modules, power grid, or instruments.

**Fix:** Create `EMPEventTests.cs` covering: module disablement, power grid impact, instrument failure, duration, save/load mid-EMP.

**Estimated scope:** 1 new test file, ~15 test methods.

---

## HIGH Findings (Fix Soon)

| ID | Finding | Sweep | File(s) | Fix Summary |
|----|---------|-------|---------|-------------|
| H-1 | USS canonical vs Resources copy divergence — player builds may get different styles than editor | UI | `DiegeticHud.uss` (both copies) | Run `DiegeticHudDeploy.SyncResourcesCopies()` + add CI check that both files match |
| H-2 | `EndgameEngine.CampaignResult` (victory/defeat state) is never saved | Save/Load | `EndgameEngine.cs` | Add `SetEndgameEngine()` to SaveSystem.Wiring, register with ISaveable adapter |
| H-3 | SystemRegistry tick catch logs `ex.Message` only — no stack trace | Null Safety | `SystemRegistry.cs` (lines 189, 208) | Change `ex.Message` to `ex.ToString()` to include stack trace |
| H-4 | SaveSystem double-serializes + SHA256 byte-by-byte string building on main thread | Performance | `SaveSystem.IO.cs` (lines 35-37, 189, 266) | Compute checksum incrementally; consider async write for large saves |
| H-5 | `HUD.EnsureWidgetReferences()` auto-creates 28+ components via `AddComponent` fallback | UI | `HUD.cs` (lines 80-150) | Log a warning when auto-creating; consider requiring scene wiring |
| H-6 | DiegeticHudView.PaintEventModal creates N new `Label` VisualElements per paint | Performance | `DiegeticHudView.cs` (~500-520) | Pool/cache choice row elements like vitals rows |
| H-7 | No service locator / DI — all 200+ systems manually wired in GameBootstrap | Architecture | `GameBootstrap.*.cs` | Introduce `SystemRegistry.Get<T>()` lookup (see Phase 4) |
| H-8 | Duplicate tick paths (registry + fallback) can drift apart | Architecture | `GameBootstrap.TickSystems.cs` | Remove fallback path; tests should use SystemRegistry subset |
| H-9 | SaveSystem has 60+ setter methods (one per system) | Architecture | `SaveSystem.Wiring.cs` | Auto-discover ISaveable systems via SystemRegistry iteration |
| H-10 | 9 anonymous `EventRunner.OnChoiceApplied` lambdas never unsubscribed | Event Bus | `InitLate.Radio.cs` (lines 135-145) | Convert to SubscriptionBag pattern (see C-1) |
| H-11 | MedicalSystem core pipeline (diagnosis → treatment → outcome) has no test | Test Coverage | `MedicalSystem.cs` | Create `MedicalSystemPipelineTests.cs` |
| H-12 | Irradiated food consumption + cooking system untested | Test Coverage | `CookingSystem.cs` | Create `CookingSystemTests.cs` and `IrradiatedFoodTests.cs` |
| H-13 | RadiationMutagenesisSystem has no dedicated tests | Test Coverage | `RadiationMutagenesisSystem.cs` | Create `RadiationMutagenesisTests.cs` |
| H-14 | AddictionSystem (onset, withdrawal, recovery) untested | Test Coverage | `AddictionSystem.cs` | Create `AddictionSystemTests.cs` |
| H-15 | Core/ folder is a 620-file dumping ground | Architecture | `Assets/_Game/Core/` | Move domain files to proper folders (see Phase 3) |

---

## MEDIUM Findings (Fix When Convenient)

| ID | Finding | Sweep | File(s) |
|----|---------|-------|---------|
| M-1 | MainMenuController uses raw string queries (no constants) | UI | `MainMenuController.*.cs` |
| M-2 | DiegeticHudView dual build path (Build/BindExisting) implicit coupling | UI | `DiegeticHudView.cs` |
| M-3 | HUD.Update polls 3 subsystems per frame (EndgameSummaryUI, PowerGridHUD lack change events) | UI | `HUD.cs`, `EndgameSummaryUI.cs`, `PowerGridHUD.cs` |
| M-4 | GameBootstrap.Update pushes HUD data unconditionally every frame | Performance | `GameBootstrap.Lifecycle.cs` (~289-302) |
| M-5 | DiagnosticsOverlay.OnGUI allocates ~10+ strings per frame via interpolation | Performance | `DiagnosticsOverlay.cs` |
| M-6 | UtilityAIDebugHUD.OnGUI allocates strings per action per frame | Performance | `UtilityAIDebugHUD.cs` |
| M-7 | `TimeSystem` dual-write (positional + ISaveable) with potential clock drift | Save/Load | `SaveSystem.Capture.cs`, `SaveSystem.Restore.cs` |
| M-8 | Restore fires live events — systems restored early can trigger side effects in not-yet-restored systems | Save/Load | `SaveSystem.Entities.cs` |
| M-9 | No event-suppression during save capture; async callbacks could mutate mid-snapshot | Save/Load | All systems |
| M-10 | `PolypharmSave.ValuesJagged` (float[][]) silently dropped by JsonUtility | Save/Load | `SimulationSystems.Medical.cs` |
| M-11 | SaveSystem.Set* methods accept null silently (50+ methods) | Null Safety | `SaveSystem.Wiring.cs` |
| M-12 | PlayerInputHandler silently does nothing if bootstrap is on wrong GameObject | Null Safety | `PlayerInputHandler.cs` |
| M-13 | GeigerAudioHook has no fallback for missing AudioSource/clip | Null Safety | `GeigerAudioHook.cs` |
| M-14 | Diagnostics catch is too narrow (TargetInvocationException only) | Null Safety | `GameBootstrap.Diagnostics.cs` |
| M-15 | `GameBootstrap.InitWorld.Diary.cs` `.Find() as T` result may be null in init chain | Null Safety | `GameBootstrap.InitWorld.Diary.cs` |
| M-16 | AudioEventBus.Teardown() not called from OnDestroy | Event Bus | `GameBootstrap.Lifecycle.cs` |
| M-17 | MainMenuController button click handlers never unsubscribed | Event Bus | `MainMenuController.cs`, `MainMenuController.Dialogs.cs` |
| M-18 | Shared mutable state in `SeededRandom` tests without TearDown reset | Test Coverage | `NeedsSystemWiringTests.cs` |
| M-19 | Every partial file imports all 20 using namespaces — zero access restriction | Architecture | All `GameBootstrap.*.cs` |
| M-20 | Business logic in bootstrap (hoarder rolls, pianist creation, scavenge roster) | Architecture | Multiple partials |

---

## LOW Findings (Acceptable / Minor)

| ID | Finding | Sweep |
|----|---------|-------|
| L-1 | Duplicate `border-left-width` in `.diegetic-panel` | UI |
| L-2 | Unused `--font-bold` and `--exclusive-line` CSS variables | UI |
| L-3 | SetVisible dual mechanism (class + inline style) | UI |
| L-4 | UtilityAIDebugHUD uses IMGUI OnGUI (legacy) | UI |
| L-5 | PaintEncounter/PaintEventModal rebuild Labels each call | Performance |
| L-6 | Enum `.ToString()` boxing in debug overlays | Performance |
| L-7 | `ClothingDegradationSystem` and `ScrapWeaponSystem` bypass RegisterSystem | Save/Load |
| L-8 | No per-DTO versioning on sub-DTOs | Save/Load |
| L-9 | MedicalSystem null treatment defaults to magic 10f heal | Null Safety |
| L-10 | SystemRegistry loses stack traces in tick error logging | Null Safety |
| L-11 | No nullable reference type annotations anywhere | Null Safety |
| L-12 | Reflection-based testing of private methods | Test Coverage |
| L-13 | Ceiling collapse probabilistic test (flaky risk) | Test Coverage |

---

## Prioritized Fix Plan

### Phase 1: Critical Bug Fixes (Tomorrow — Day 1)
**Goal:** Eliminate data-loss and correctness bugs.

| Task | Finding | Files to Touch | Effort |
|------|---------|---------------|--------|
| 1.1 Fix Iron Man false-success on failed save deletion | C-2 | `Mode_IronMan.cs` + new test | Small |
| 1.2 Fix SystemRegistry missing stack traces | H-3 | `SystemRegistry.cs` | Trivial |
| 1.3 Save EndgameEngine.CampaignResult | H-2 | `EndgameEngine.cs`, `SaveSystem.Wiring.cs`, `SaveSystem.cs`, `GameBootstrap.InitLate.cs` | Medium |
| 1.4 Sync USS Resources copy with canonical | H-1 | `DiegeticHud.uss` (Resources copy) | Trivial (run deploy tool) |

---

### Phase 2: Memory Leak Fix (Tomorrow — Day 2)
**Goal:** Eliminate the ~60+ event subscription leaks that prevent GC and cause handler duplication.

| Task | Finding | Files to Touch | Effort |
|------|---------|---------------|--------|
| 2.1 Create `SubscriptionBag` helper | C-1 (infra) | New: `Assets/_Game/Utilities/SubscriptionBag.cs` | Small |
| 2.2 Migrate `InitLate.cs` (~16 lambdas) to bag | C-1 | `GameBootstrap.InitLate.cs`, `GameBootstrap.Lifecycle.cs` | Medium |
| 2.3 Migrate `InitLate.Radio.cs` (~14 lambdas) to bag | C-1, H-10 | `GameBootstrap.InitLate.Radio.cs` | Medium |
| 2.4 Migrate `InitFoundation.cs` (~8 lambdas) to bag | C-1 | `GameBootstrap.InitFoundation.cs` | Medium |
| 2.5 Migrate `BunkerSocial.cs` (~17 lambdas) to bag | C-1 | `GameBootstrap.BunkerSocial.cs` | Medium |
| 2.6 Migrate `UiActions.Hud.cs` + `UiActions.Radio.cs` (~8 lambdas) to bag | C-1 | `GameBootstrap.UiActions.Hud.cs`, `GameBootstrap.UiActions.Radio.cs` | Small |
| 2.7 Migrate `InitWorld.Narrative.cs` + `InitWorld.Diary.cs` to bag | C-1 | `GameBootstrap.InitWorld.Narrative.cs`, `GameBootstrap.InitWorld.Diary.cs` | Small |
| 2.8 Dispose all bags in `OnDestroy` | C-1 | `GameBootstrap.Lifecycle.cs` | Small |
| 2.9 Add test: verify no event leaks across scene reload | C-1 | New test file | Medium |

---

### Phase 3: Test Coverage Gaps (Day 3-4)
**Goal:** Cover critical untested systems from the spec.

| Task | Finding | New Test File | Effort |
|------|---------|--------------|--------|
| 3.1 EMP event effects test | C-4 | `EMPEventTests.cs` | Medium |
| 3.2 MedicalSystem pipeline test | H-11 | `MedicalSystemPipelineTests.cs` | Medium |
| 3.3 Cooking + irradiated food test | H-12 | `CookingSystemTests.cs` | Medium |
| 3.4 Radiation mutagenesis test | H-13 | `RadiationMutagenesisTests.cs` | Medium |
| 3.5 Addiction system test | H-14 | `AddictionSystemTests.cs` | Medium |
| 3.6 Fix SeededRandom test teardown | M-18 | `NeedsSystemWiringTests.cs` | Trivial |
| 3.7 Convert "AcceptsNeedsSystem" existence tests to behavior tests | M-18 | `NeedsSystemWiringTests.cs` | Small |

---

### Phase 4: Performance Optimizations (Day 5-6)
**Goal:** Eliminate frame hitches and per-frame allocations.

| Task | Finding | Files to Touch | Effort |
|------|---------|---------------|--------|
| 4.1 Make save checksum incremental (eliminate double-serialization) | H-4 | `SaveSystem.IO.cs` | Medium |
| 4.2 Pool DiegeticHudView event choice Labels | H-6 | `DiegeticHudView.cs` | Medium |
| 4.3 Add dirty-flag guards to GameBootstrap.Update HUD pushes | M-4 | `GameBootstrap.Lifecycle.cs` | Small |
| 4.4 Cache DiagnosticsOverlay string interpolations | M-5 | `DiagnosticsOverlay.cs` | Small |
| 4.5 Cache UtilityAIDebugHUD action list text | M-6 | `UtilityAIDebugHUD.cs` | Small |
| 4.6 Add change events to EndgameSummaryUI and PowerGridHUD | M-3 | `EndgameSummaryUI.cs`, `PowerGridHUD.cs`, `HUD.cs` | Medium |

---

### Phase 5: Architecture Improvement (Day 7-10, ongoing)
**Goal:** Reduce god-class coupling and improve folder organization.

| Task | Finding | Approach | Effort |
|------|---------|----------|--------|
| 5.1 Add `SystemRegistry.Get<T>()` lookup | H-7 | New method on existing SystemRegistry; new systems use it instead of GameBootstrap properties | Small |
| 5.2 Stop adding new properties to GameBootstrap | C-3 | Policy + code review enforcement | Ongoing |
| 5.3 Remove duplicate tick fallback path | H-8 | `GameBootstrap.TickSystems.cs` — delete fallback, update test hosts | Medium |
| 5.4 Auto-discover ISaveable via SystemRegistry | H-9 | `SaveSystem.cs` — iterate registry instead of 60+ setters | Medium |
| 5.5 Move domain files from Core/ to proper folders | H-15 | Move ~400 files: Actions→AI, Items→Inventory, ShelterModules→Shelter, etc. | Large (pure moves + namespace updates) |
| 5.6 Introduce system group factories (VictoryPathFactory, ShelterModuleFactory) | C-3 | New factory classes | Medium each |
| 5.7 Add null guards to SaveSystem.Set* methods | M-11 | `SaveSystem.Wiring.cs` — add `Debug.Assert(s != null)` | Small |
| 5.8 Add suppress-events-during-restore flag | M-8 | `SaveSystem.Restore.cs` | Medium |

---

### Phase 6: UI Polish (As needed)
**Goal:** Clean up UI coupling issues.

| Task | Finding | Files to Touch | Effort |
|------|---------|---------------|--------|
| 6.1 Extract MainMenuController string queries to constants | M-1 | `MainMenuController.*.cs` | Small |
| 6.2 Add warning log when HUD auto-creates components | H-5 | `HUD.cs` | Trivial |
| 6.3 Remove dead USS declarations | L-1, L-2 | `DiegeticHud.uss` | Trivial |

---

## Positive Findings (No Action Needed)

These areas are well-engineered and worth preserving:

- **Atomic save writes** via `File.Replace` with `.bak` rotation
- **SHA-256 checksum** with empty-placeholder verification
- **`.bak` fallback recovery** on corrupt main save
- **`LastLoadSucceeded` / `SuppressAutoSave`** guard against overwrite-after-failed-load
- **Fail-fast two-phase restore** in dev builds
- **`SeededRandom.ResetStreams()`** for deterministic RNG after load
- **Fingerprint-based change detection** in `DiegeticHudView.PaintVitals`
- **Pooled StringBuilder** in `InventoryStripUI`
- **GenericObjectPool** used for highest-churn UI objects
- **Zero coroutines** — eliminates entire class of leak
- **Zero `FindObjectOfType` / `Camera.main`** in runtime code
- **249 `TryGetValue`** call sites — disciplined dictionary access
- **Consistent Arrange-Act-Assert** pattern in tests
- **Save/load round-trip tests** with deliberately "nasty" mid-game states
- **`UntickedSystemsBaseline` ratchet** prevents regression
