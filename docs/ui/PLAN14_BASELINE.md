# Plan 14 — UX, Onboarding & Accessibility: Baseline Evidence & State

**Execution Date:** 2026-08-31
**Target:** ASHFALL: Atomic War - Starving Survival (Godot 4.7+ .NET Mono Host / .NET 8 / .NET 9 Tests)
**Corpus State:** 164 C# UI files in `src/UI/`, 22 `.tscn` UI scenes in `assets/ui/`, 138 data catalogs in `Assets/StreamingAssets/Data/`, 30 snapshot targets in `SnapshotHarness.cs`.

---

## 1. Initial Verification Matrix Status

All baseline verification gates pass with zero errors prior to modification:

| Command / Gate | Status | Output Evidence |
|---|:---:|---|
| `dotnet test Ashfall.Core.Tests` | **PASS** | 5,303 passed, 0 failed, 0 skipped (Duration: 26s) |
| `godot --headless --path . -- --content-utilization-selftest` | **PASS** | 413 catalogs scanned, 0 orphaned, CI gate PASS |
| `godot --headless --path . -- --data-integrity-selftest` | **PASS** | 0 findings, 5,563 authored IDs, 0 errors |
| `godot --headless --path . -- --scene-binding-selftest` | **PASS** | 22/22 scenes passed contract binding |
| `python3 scripts/ci/scene-lint.py` | **PASS** | 26 production scenes checked; 0 errors, 0 warnings |
| `godot --headless --path . -- --ui-accessibility-selftest` | **PASS** | 5/5 gates passed (Focus, Labels, Close, Modals, Static Lint) |
| `godot --headless --path . -- --onboarding-journey-selftest` | **PASS** | 20/20 assertions passed; save/load resume verified |

---

## 2. Structural & Architectural Inventory

### 2.1 UI System Composition
- **UI Code Tree:** `src/UI/` containing 164 `.cs` presentation classes.
- **Scene Files:** `assets/ui/` containing 22 `.tscn` (19 panels + 3 modals).
- **Design Tokens:** `Assets/Ashfall.Core/UI/Theme.cs` (pure C# token authority) and `src/UI/AshfallUiHelpers.cs` (Godot factory helpers).
- **Settings Store:** `UserSettingsData.cs` in Core + `UserSettingsStore.cs` in Godot host + `SettingsPanel.cs` UI.
- **Onboarding Machine:** `OnboardingJourney.cs` in Core + `OnboardingHintPanel.cs` / `TutorialPanel.cs` in Godot host + `Main.Onboarding.cs` orchestration.
- **Input System:** `AshfallInputActions.cs` canonical action definitions and runtime mapping.

### 2.2 Golden Snapshot Baseline
- **Harness:** `SnapshotHarness.cs` & `SnapshotOrchestrator.cs`.
- **Snapshots Catalog:** 30 player-facing runtime targets defined in `SnapshotHarness.Targets` and documented in `docs/ui/snapshot_manifest.json` and `docs/ui/SNAPSHOT_COVERAGE.md`.
- **All 29 tracked runtime player-facing surfaces are covered** (27 COVERED, 1 PARTIAL, 1 REGRESSION_ONLY, 0 MISSING).

---

## 3. Plan 14 Investigation Focus Areas

1. **Onboarding & Tutorial Causality (Task 14A):**
   - Address the 3 critical teach-before-demand gaps: Radiation/Dosimeter interpretation, Ration policy, and Power/Water triage.
   - Expand contextual hints and consequence feedback so that novice players understand what killed them or caused a failure *before* it occurs.
   - Maintain persistent onboarding completion and settings controls.

2. **Accessibility & Readability (Task 14B):**
   - Eliminate color-only status channels (add icons/shapes/text labels).
   - Implement font-size floors (no critical text below 12px, labels at 11px).
   - Verify UI scaling at 100%, 125%, 150% and test layout overflow resilience.
   - Ensure focus visibility, keyboard navigation, and zero focus traps.

3. **Localization Readiness (Task 14C):**
   - Inventory user-facing strings across UI and structured JSON.
   - Scaffold `TranslationServer` integration, stable key convention, and source English catalog.
   - Build development pseudo-locale (+30-40% expansion) and automated CI string linter.

4. **Information Hierarchy & Severity Vocabulary (Task 14D):**
   - Standardize severity levels: Normal, Attention, Dangerous, Critical, Blocked.
   - Glance → Inspect → Act navigation flows on HUD and core panels.
   - Provide actionable explanations on disabled controls.

5. **Input, Navigation & Rebinding (Task 14E):**
   - Audit action map for conflicts and missing bindings.
   - Dynamic prompt glyph resolution instead of hardcoded key strings.
   - Predictable modal escape, tab traversal, and focus restoration.

6. **Expert Efficiency & Field Manual (Tasks 14F & 14G):**
   - Friction reduction for high-frequency workflows (survivor triage, ration adjustment, inventory check).
   - Contextual deep links from tutorial hints to the Field Manual in Journal/Codex.
   - Status icon glossary and dynamic key displays.
