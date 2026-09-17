# ASHFALL — Quality Roadmap Batch 57

## Theme: UI Accessibility Audit & Keyboard Navigation

**Priority:** MEDIUM-HIGH
**Risk:** Low — additive, no behavioral changes to game logic
**Batch:** 57

---

## Context

The project has 83 files named `*Panel.cs` in `src/UI/` (97 `.cs` files total in that flat directory, including 14 non-panel shells/helpers/modals/overlays: `AshfallDashboardShell`, `AshfallDataGrid`, `AshfallMetricCard`, `AshfallSidebar`, `AshfallStatusRail`, `AshfallUiHelpers`, `MainMenuBuilder`, `CombatHudOverlay`, `GameHudOverlay`, `DailyBriefingModal`, `OpeningProtocolModal`, `SnapshotHarness`, `SnapshotOrchestrator`, `UiBackgroundCarousel`). "~85 panels" is a reasonable rounding of 83 — use that number, not "85+", in later steps. All panel/widget classes use partial class patterns and consume the five named `Ashfall*` widgets (verified to exist at `src/UI/AshfallDashboardShell.cs:25`, `AshfallDataGrid.cs:26`, `AshfallMetricCard.cs:23`, `AshfallSidebar.cs:21`, `AshfallStatusRail.cs:17`, all in namespace `AtomicWar.GodotApp.UI`). Theme colors come from `Ashfall.Core.UI.Theme` (`Assets/Ashfall.Core/UI/Theme.cs:6`) which defines `Ink`, `InkPanel`, `Warm`, `Hot`, `Pale`, `Muted`, `Dim` exactly as named (plus additional tokens not relevant here). Note: `src/UI/*.cs` consumers import this via `using DesignTheme = Ashfall.Core.UI.Theme;`, so grepping for `Ashfall.Core.UI.Theme.Ink` directly in `src/UI/` will find nothing — grep for `DesignTheme\.` instead. Fonts `BarlowCondensed` + `ShareTechMono` are confirmed in use (`src/UI/AshfallUiHelpers.cs:25-57`, `.ttf.import` sidecars under `assets/fonts/`). No `.tscn` scenes exist for individual panels — confirmed only 5 `.tscn` files exist repo-wide (`scenes/Main.tscn`, `scenes/CSharpTest.tscn`, `scenes/HoldfastInterior.tscn`, `scenes/WastelandMap.tscn`, `src/World/MapLocationMarkerView.tscn`), none of which back a `*Panel.cs` class; `scenes/Main.tscn` is a single empty root `Control` node whose script points to `res://src/Main.cs`, and `BuildUserInterface()` (`src/Main.cs:640`) populates the entire UI tree at runtime. Resolution: 1920×1080.

All UI construction happens in code via Godot's Control node API. Focus traversal, screen reader metadata, and keyboard shortcuts are confirmed absent — a search across all of `src/UI/` for `FocusMode`, `FocusNeighbor*`, `FocusNext`, `FocusPrevious` returns zero matches.

---

## Step 1 — Audit Current Accessibility State

### Goal

Establish a baseline of accessibility gaps: missing focus modes, absent focus neighbors, contrast ratio violations, missing screen reader hints (`hint_tooltip`, `accessibility_name`), and tab-order breaks across all panels.

### Implementation

**Note:** `scripts/audit/` does not exist as a directory today (only a flat `scripts/audit_assets.py` exists, unrelated). `--accessibility-audit` is not a recognized CLI verb anywhere in `src/Host/HostCli.cs` — this step builds it from scratch. Follow the existing precedent for how self-test verbs are wired: `--data-integrity-selftest` and `--bridge-selftest` are both parsed in `src/Host/HostCli.cs` (e.g. `HostCli.cs:239-240` for `--data-integrity-selftest`) and dispatched from `src/Main.cs`'s CLI action switch to a real handler (see `src/Host/HostCli.SelfTests.cs:32-44` for `RunDataIntegritySelfTest`). New work should mirror this pattern rather than inventing a separate mechanism.

- Create `src/Host/HostCli.AccessibilityAudit.cs` (partial class, following the existing `HostCli.SelfTests.cs` convention) containing the audit logic, and add a `--accessibility-audit` case to the existing argument parser in `HostCli.cs`.
- The audit tool must run after `BuildUserInterface()` completes (`src/Main.cs:640`) since panels are built entirely in code with no `.tscn` backing — there is no way to inspect the tree before this call executes.
- Walks the full scene tree after `BuildUserInterface()` completes.
- For every `Control` node, records: `FocusMode`, `FocusNeighborTop/Bottom/Left/Right`, `FocusNext/FocusPrevious`, `TooltipText`, node type, and whether it is an interactive control (`Button`, `LineEdit`, `Slider`, `CheckBox`, etc.).
- Flags interactive controls with `FocusMode == None`.
- Flags any control missing `FocusNext`/`FocusPrevious` when it has siblings.
- Computes contrast ratio between foreground font color and background `StyleBox` color for each labeled control; flags ratios below WCAG AA (4.5:1 for normal text, 3:1 for large text).
- Output: JSON report at `Builds/accessibility-audit-report.json` with per-panel summary and aggregated violation counts.
- Run via: `godot --headless --path . -- --accessibility-audit`

### Risk / Rollback

Low risk — this step only adds a new CLI verb and a read-only tree walk; it does not mutate any `Control` state. If the new verb destabilizes the CLI argument parser, revert the `HostCli.cs` diff and delete the new partial file; no other system depends on it yet (nothing in Steps 2+ can run until this exists, so a revert here simply blocks the rest of the batch, it does not break existing functionality).

### Verification

```
godot --headless --path . -- --accessibility-audit
# Exits 0, produces Builds/accessibility-audit-report.json
test -f Builds/accessibility-audit-report.json && python3 -m json.tool Builds/accessibility-audit-report.json > /dev/null
# ^ confirms the report is valid, parseable JSON (do not just eyeball it)
```

### Done-when

- `--accessibility-audit` is a recognized verb in `HostCli.cs` and exits 0.
- `Builds/accessibility-audit-report.json` exists, is valid JSON, and contains a per-panel entry for at least all 83 `*Panel.cs`-backed screens plus the 5 `Ashfall*` shared widgets.
- The report's aggregated violation count for "interactive controls with FocusMode==None" is a concrete non-negative integer (baseline is expected to be high — every interactive control today, since no `src/UI/` file sets `FocusMode` — but the number itself, not just "high", must be recorded in this step's output for Step 2 to measure against).

---

## Step 2 — Add Focus Navigation Infrastructure

### Goal

Every interactive control in the 85+ panels must participate in a logical focus chain. Players can navigate the entire UI with Tab/Shift-Tab and directional keys without the mouse.

### Implementation

- Add `src/UI/Accessibility/FocusChainBuilder.cs`:
  - Static utility that, given a `Control` root, recursively finds all interactive descendants and assigns `FocusMode = FocusModeEnum.All`.
  - Assigns `FocusNext`/`FocusPrevious` in document order (depth-first left-to-right).
  - Assigns `FocusNeighborTop/Bottom/Left/Right` using spatial proximity (nearest-neighbor by center point in the relevant axis).
  - Exposes `BuildChain(Control root)` and `RebuildChain(Control root)` (for dynamic panel add/remove).
- Integrate into each panel's construction epilogue: after all widgets are added, call `FocusChainBuilder.BuildChain(panelRoot)`.
- Add `src/UI/Accessibility/FocusRing.cs`:
  - Visual feedback: draws a 2px `Hot` color border around the currently focused control using `_Draw()` override or a `StyleBoxFlat` swap on focus enter/exit.
  - Subscribes to `FocusEntered`/`FocusExited` signals on the focused control.
- Wire into `Main.cs` `BuildUserInterface()` (single definition at line 640, body runs to line 1283) — after all panels are constructed, invoke `FocusChainBuilder.BuildChain` on the root UI container, as the last statement inside that one method body. Do **not** duplicate the call at each of the method's 16 call sites (see Risk/Rollback below) — placing it inside the method itself means every caller (startup, reset-rebuild, and the 13 headless `--*-ui-test-and-quit` self-tests) gets a consistently-built chain automatically.

### Verification

```
dotnet build Ashfall.csproj   # 0 errors
godot --headless --path . -- --accessibility-audit
# Re-run audit: interactive controls with FocusMode==None should drop to 0
```

### Risk / Rollback

Medium risk in practice, not "Low" as the batch header implies for this specific step — `FocusChainBuilder.BuildChain` runs across all 83 panels + 5 shared widgets and mutates `FocusMode`/`FocusNeighbor*`/`FocusNext`/`FocusPrevious` on every interactive `Control` it finds. `Main.cs` is a single ~7,014-line partial class (AGENTS.md H7 cites ~6.5k; actual is 7,014 per `wc -l`) and `BuildUserInterface()` is invoked from **16 call sites**, not 4 (verified: `grep -n "BuildUserInterface();" src/Main.cs` → lines 507, 4099, 4196, 4446, 4479, 4667, 4945, 4978, 5020, 5079, 5132, 5169, 5222, 5255, 5303, 5327). Only the first (line 507, from `_Ready()`) is the real startup path and the next two (4099, 4196) are in-place UI rebuilds after a save-file reset. The remaining **12 call sites (4446 onward) are headless CLI self-test entry points** — `RunUtilityAiUiTestAndQuit`, `RunEconomyUiTestAndQuit`, `RunHoldfastRuntimeUiTestAndQuit`, `RunDoseUiTestAndQuit`, `RunVerdictUiTestAndQuit`, `RunInventoryUiTestAndQuit`, `RunExpeditionPanelUiTestAndQuit`, `RunSurvivorsUiTestAndQuit`, `RunPhase0UiTestAndQuit`, `RunMusterUiTestAndQuit`, `RunJournalUiTestAndQuit`, `RunDashboardUiTestAndQuit`, `RunPlayerPanelsUiTestAndQuit` — each of which calls `BuildUserInterface()` once per process invocation and then exits, so they are lower-risk for chain-ordering bugs (single build, no stale-state carryover) but they multiply the number of places `BuildChain` must behave correctly, and any of the existing `--*-ui-test-and-quit` headless smoke tests could start silently asserting on focus-chain side effects if `BuildChain` throws or mutates state unexpectedly. Calling `BuildChain` from the wrong call site, or before all panels finish constructing, will silently produce an incomplete or wrong-order chain — there is no compile error for this, only a behavioral regression discoverable via the audit report. Mitigation: call `BuildChain` only once per `BuildUserInterface()` invocation, at the very end of that single method body (not duplicated at each of the 16 call sites) so every path — startup, reset-rebuild, and all 13 headless self-tests — gets a consistent chain for free. Rollback: `FocusChainBuilder`/`FocusRing` are new additive files; if the wiring misbehaves, revert the single call inside `BuildUserInterface()` — the game is fully playable via mouse with that one line removed, since no other system depends on the focus chain yet.

### Done-when

- Every interactive control in all 83 `*Panel.cs` screens (plus the 5 `Ashfall*` shared widgets, where they host interactive children) has `FocusMode.All`.
- Tab cycles through all controls in each panel in logical reading order (verified manually for at least 3 representative panels: one simple list panel, one `AshfallDataGrid`-heavy panel, one modal).
- Focus ring is visible on the active control (manual screenshot or automated pixel-diff check).
- Re-running `--accessibility-audit` shows the "missing focus mode" violation count drop from the Step 1 baseline to exactly 0 for interactive controls.

---

## Step 3 — Add Keyboard Shortcuts for Core Gameplay Actions

### Goal

Players can perform all critical gameplay actions without a mouse. Shortcuts are discoverable and rebindable.

### Implementation

- Add `src/UI/Accessibility/KeyboardShortcutRegistry.cs`:
  - Dictionary mapping `StringName` action → `Key` + modifiers.
  - Default bindings:
    - `Space` / `Enter` — advance day (when no modal is open).
    - `Tab` — cycle to next panel.
    - `Shift+Tab` — cycle to previous panel.
    - `Escape` — close current modal / open pause menu.
    - `1`–`9` — switch to panel by index (Survivors, Inventory, Map, Medical, Economy, Shelter, Radio, Journal, Settings).
    - `Ctrl+S` — manual save.
    - `?` or `F1` — open help/shortcut reference overlay.
  - Persisted to `user://accessibility_keybinds.json` via `IFileIO`.
  - Loaded on startup; defaults used if file missing.
- Add `src/UI/Accessibility/ShortcutOverlay.cs`:
  - Modal panel listing all shortcuts, grouped by category.
  - Triggered by `?`/`F1`.
- Wire into `Main._UnhandledInput(InputEvent)`:
  - Consult `KeyboardShortcutRegistry` before panel-specific handlers.
  - Day-advance shortcut calls the same path as the existing day-advance button.
  - Panel-switch shortcut shows/hides panels via existing visibility logic.

### Verification

```
dotnet build Ashfall.csproj   # 0 errors
# Manual QA: launch game, press Tab/1-9/Space/Escape — confirm expected behavior
# Automated: add Ashfall.Core.Tests/KeyboardShortcutRegistryTests.cs
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

### Risk / Rollback

Medium risk — wiring into `Main._UnhandledInput(InputEvent)` intercepts input before panel-specific handlers, so a bug here can make existing panel input (e.g. a `LineEdit` needing literal `Escape`/digit keys) silently stop working. Verify no existing panel already consumes any of these keys before wiring (grep `src/UI/*.cs` for existing `InputEvent`/`_UnhandledInput`/`_Input` overrides that might conflict with `1`-`9`, `Ctrl+S`, or `Escape`). Rollback: the shortcut registry is additive and gated by a single dispatch call in `Main._UnhandledInput`; removing that one call site fully restores prior input behavior.

### Done-when

- All 13 listed shortcuts (Space/Enter, Tab, Shift+Tab, Escape, digits 1-9, Ctrl+S, ?/F1) perform their documented action, verified manually.
- Shortcut overlay displays accurate bindings matching `KeyboardShortcutRegistry`'s actual default dictionary (not a hardcoded duplicate list in the overlay UI — assert this at test time, since a hardcoded copy silently drifts).
- Rebinding persists across sessions (round-trip: rebind a key, quit, relaunch, confirm the rebind survived by reading `user://accessibility_keybinds.json`).
- No shortcut conflicts with an existing panel's own input handling (verified by the grep above, not just "no conflict with Godot engine defaults").

---

## Step 4 — Add Tooltip System for Icon-Only Buttons and Status Indicators

### Goal

Every icon-only button and status indicator has a descriptive tooltip that appears on hover and on focus (for keyboard users). Screen readers can access the same text.

### Implementation

- Add `src/UI/Accessibility/AccessibleTooltip.cs`:
  - Wrapper that sets both `TooltipText` (Godot's built-in hover tooltip) and a custom `AccessibilityDescription` metadata key on the control.
  - For keyboard focus: on `FocusEntered`, display tooltip near the control (same position logic as hover tooltip) using a dedicated `TooltipPopup` singleton.
  - Auto-dismiss on `FocusExited` or after 5 seconds.
- Audit all icon-only buttons in `src/UI/`:
  - `AshfallStatusRail` indicators (radiation, hunger, thirst, morale, etc.).
  - Toolbar buttons (save, settings, help, day advance).
  - Grid action icons (trade, craft, assign, dismiss).
- Add tooltip text for each — concise, action-oriented (e.g., "Advance to next day (Space)", "Open inventory (3)").
- Store tooltip strings in `Assets/StreamingAssets/Data/ui_tooltips.json` for future localization. Per project data-authority convention (AGENTS.md Invariant 6, "only 35 of ~280 JSON files have `schema_version`"), this NEW file must include a `schema_version` field from day one rather than adding to the backlog of un-versioned catalogs.

### Risk / Rollback

Low — additive only. If `AccessibleTooltip` misbehaves (e.g. popup positioning off-screen at extreme resolutions), it degrades gracefully: `TooltipText` still works via Godot's native hover tooltip since that assignment happens independently of the custom `TooltipPopup` singleton. Rollback: remove the `FocusEntered`/`FocusExited` subscription in `AccessibleTooltip` to fall back to hover-only tooltips.

### Verification

```
dotnet build Ashfall.csproj   # 0 errors
godot --headless --path . -- --accessibility-audit
# Audit: 0 interactive controls with empty TooltipText
godot --headless --path . -- --data-integrity-selftest
# Confirm ui_tooltips.json passes integrity check
```

### Done-when

- Every icon-only button and status indicator has non-empty `TooltipText`.
- Tooltips appear on both hover and keyboard focus.
- Tooltip content is externalized in JSON for localization readiness.
- Audit report shows 0 "missing tooltip" violations.

---

## Step 5 — Add High-Contrast Theme Variant

### Goal

Players with low vision can switch to a high-contrast theme that guarantees WCAG AAA (7:1) contrast ratios on all text and interactive elements.

### Implementation

- Add `Ashfall.Core/UI/ThemeVariant.cs`:
  - Enum: `Standard`, `HighContrast`.
  - `HighContrastPalette` static class with adjusted colors:
    - Background: pure black (`#000000`).
    - Primary text: pure white (`#FFFFFF`).
    - Secondary text: bright yellow (`#FFFF00`).
    - Interactive borders: cyan (`#00FFFF`).
    - Alert/Hot: bright red (`#FF4444`).
    - Focus ring: thick (3px) bright yellow.
  - All existing `Theme.*` color references route through `ThemeVariant.Current` — returns standard palette or high-contrast palette.
- Add `src/UI/Accessibility/ThemeSwitcher.cs`:
  - Godot host node that listens for a toggle (Settings panel checkbox or `Ctrl+Shift+H` shortcut).
  - On switch: walks all `Control` nodes, re-applies colors from the active variant.
  - Persists choice to `user://accessibility_settings.json`.
- Update `AshfallDashboardShell`, `AshfallDataGrid`, `AshfallMetricCard`, `AshfallSidebar`, `AshfallStatusRail` — replace hardcoded color references with `ThemeVariant.Resolve(ThemeColor.Ink)` pattern. Note these 5 classes currently reference `Ashfall.Core.UI.Theme` via a `using DesignTheme = Ashfall.Core.UI.Theme;` alias (confirmed in `AshfallDashboardShell.cs:4`, `AshfallDataGrid.cs:5`, `AshfallMetricCard.cs:4`, etc.) — a plain grep for `Ashfall.Core.UI.Theme.Ink` across `src/UI/` will miss these call sites; search for `DesignTheme\.` instead when locating every color reference to migrate.

### Risk / Rollback

Medium — this step rewires color resolution in the 5 most widely-consumed widget classes (used by dozens of the 83 panels, per Step 1's file audit). A mistake here has broad blast radius across the entire UI, not just accessibility-specific screens. Land this as its own reviewable commit separate from the `ThemeVariant`/palette definitions, and manually screenshot-diff a representative sample of panels (at minimum: one `AshfallDataGrid`-heavy panel, one `AshfallMetricCard`-heavy panel) in Standard mode before/after to confirm zero visual regression. Rollback: `ThemeVariant.Current` defaulting to `Standard` and `ThemeVariant.Resolve` returning the exact same values as the current hardcoded `Theme.*` constants makes this a behavior-preserving refactor when correct — if a diff appears in Standard mode, revert the 5 widget files only; `ThemeVariant.cs` itself can stay since it isn't referenced elsewhere yet.

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # Core compiles
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj     # All pass
dotnet build Ashfall.csproj                                  # Godot host compiles
godot --headless --path . -- --accessibility-audit
# Audit with HighContrast active: 0 contrast violations at AAA threshold
```

### Done-when

- High-contrast mode is toggleable at runtime.
- All text meets 7:1 contrast ratio in high-contrast mode.
- Standard theme is unchanged (no visual regression).
- Preference persists across sessions.

---

## Step 6 — Add Font Scaling System

### Goal

Players can increase or decrease UI font size (75%–200% of default) without layout breakage. Supports accessibility needs for larger text.

### Implementation

- Add `src/UI/Accessibility/FontScaler.cs`:
  - Manages a global scale factor (`float`, 0.75–2.0, default 1.0).
  - On scale change: walks all `Control` nodes with `LabelSettings` or `Theme` font size overrides, multiplies base size by scale factor.
  - Base sizes stored in a `Dictionary<string, int>` snapshot taken at startup.
  - Exposes `SetScale(float)` and `GetScale()`.
- Add UI in Settings panel:
  - Slider (0.75–2.0, step 0.25) labeled "Text Size".
  - Preview text that updates live.
  - Keyboard shortcuts: `Ctrl+=` increase, `Ctrl+-` decrease, `Ctrl+0` reset.
- Layout resilience:
  - Panels using `AshfallDataGrid` must have `ClipContents = true` and scroll overflow.
  - `AshfallMetricCard` labels use `AutowrapMode.WordSmart` at scale ≥1.5.
  - `AshfallSidebar` items wrap or scroll rather than clip.
- Persist scale to `user://accessibility_settings.json` (same file as theme variant).
- **Note:** the `--font-scale=` CLI argument used in Verification below does not exist yet and is not part of `--accessibility-audit`'s spec from Step 1 — it must be added as a new optional argument to the `--accessibility-audit` handler in `src/Host/HostCli.AccessibilityAudit.cs` as part of this step (e.g. parsed the same way other `--flag=value` args are read in `HostCli.cs`), otherwise the verification command below will fail with an unrecognized-argument error rather than running the intended check.

### Risk / Rollback

Medium — `FontScaler` walks every `Control` node with a font-size override across all 83 panels + 5 shared widgets and mutates layout-affecting properties at runtime; a scale-application bug can clip or overlap text across the entire UI simultaneously, and `AshfallDataGrid`/`AshfallMetricCard`/`AshfallSidebar` are explicitly called out above as needing dedicated overflow handling, meaning this step touches the same widely-shared widget classes as Step 5. Verify the base-size snapshot is captured once at startup before any scaling is applied — re-snapshotting after a scale is already active will compound the multiplier on repeated adjustments (a common bug class for this kind of feature). Rollback: `FontScaler` is additive and gated by the Settings-panel slider and the three keyboard shortcuts; if scaling misbehaves, force `GetScale()` to always return `1.0` (or revert the Settings panel wiring) to fully restore the unscaled layout — no other system depends on the scaled values.

### Verification

```
dotnet build Ashfall.csproj   # 0 errors
# Manual QA: set scale to 2.0, verify no text clips or overlaps
# Automated: accessibility audit at scale 2.0 — no "text overflow" flags
# --font-scale is a NEW argument added by this step's Implementation, not pre-existing —
# confirm it was actually wired before relying on this command.
godot --headless --path . -- --accessibility-audit --font-scale=2.0
```

### Done-when

- Font scale adjustable from 75% to 200%.
- No layout breakage at any supported scale.
- Scale persists across sessions (round-trip: set scale, quit, relaunch, confirm value read back from `user://accessibility_settings.json`).
- Keyboard shortcuts for quick adjustment work, verified manually (`Ctrl+=`, `Ctrl+-`, `Ctrl+0`).
- `--accessibility-audit --font-scale=2.0` is a real, wired CLI argument (added by this step) that exits 0 and reports 0 "text overflow" flags at that scale.

---

## Step 7 — Write Accessibility Compliance Test Suite

### Goal

Automated tests that validate the focus chain, tooltip coverage, contrast ratios, and keyboard shortcut registration — run in CI to prevent accessibility regressions.

### Implementation

- Add `Ashfall.Core.Tests/AccessibilityComplianceTests.cs`:
  - `FocusChainBuilder` unit tests:
    - Given a mock control tree, verify all interactive nodes get `FocusMode.All`.
    - Verify `FocusNext`/`FocusPrevious` form a complete cycle (no dangling ends).
    - Verify spatial neighbors are assigned (no null neighbors for interior nodes).
  - `KeyboardShortcutRegistry` tests:
    - All default bindings are registered.
    - No two actions share the same key combination.
    - Serialization round-trip preserves all bindings.
  - `ThemeVariant` tests:
    - High-contrast palette: every color pair (text on background) meets 7:1 ratio.
    - Standard palette: every color pair meets 4.5:1 (AA).
  - `FontScaler` tests:
    - Scale clamped to [0.75, 2.0].
    - Base sizes correctly multiplied.
- Add `godot --headless --path . -- --accessibility-selftest`:
  - Integration test that boots UI, runs `FocusChainBuilder`, and asserts:
    - 0 interactive controls with `FocusMode.None`.
    - 0 interactive controls with empty `TooltipText`.
    - Focus chain forms a cycle (last.FocusNext == first).
  - Exits 0 on pass, non-zero with diagnostic output on fail.

### Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # All new tests pass
dotnet build Ashfall.csproj
godot --headless --path . -- --accessibility-selftest        # Exits 0
```

### Done-when

- All accessibility tests pass in CI.
- Regression in focus chain, tooltips, contrast, or shortcuts causes test failure.
- `--accessibility-selftest` is documented in the verification checklist.

---

## Summary

| Step | Title | Files Touched/Created | Risk | Depends On |
|------|-------|----------------------|------|------------|
| 1 | Audit current accessibility state | `src/Host/HostCli.AccessibilityAudit.cs`, `src/Host/HostCli.cs` (new verb) | None | — |
| 2 | Focus navigation infrastructure | `src/UI/Accessibility/FocusChainBuilder.cs`, `src/UI/Accessibility/FocusRing.cs`, `src/Main.cs` | Medium (broad blast radius across 83 panels; see Step 2 rollback) | Step 1 |
| 3 | Keyboard shortcuts | `src/UI/Accessibility/KeyboardShortcutRegistry.cs`, `src/UI/Accessibility/ShortcutOverlay.cs`, `src/Main.cs` | Medium (input-handler conflicts; see Step 3 rollback) | Step 2 |
| 4 | Tooltip system | `src/UI/Accessibility/AccessibleTooltip.cs`, `Assets/StreamingAssets/Data/ui_tooltips.json` | Low | Step 2 |
| 5 | High-contrast theme variant | `Ashfall.Core/UI/ThemeVariant.cs`, `src/UI/Accessibility/ThemeSwitcher.cs`, 5 `Ashfall*` widget files | Medium (rewires color resolution in widely-shared widgets; see Step 5 rollback) | Step 1 |
| 6 | Font scaling system | `src/UI/Accessibility/FontScaler.cs`, Settings panel update, `src/Host/HostCli.AccessibilityAudit.cs` (new `--font-scale=` arg) | Medium (walks all `Control` font overrides across 83 panels + 5 widgets; see Step 6 rollback) | Step 2 |
| 7 | Accessibility compliance test suite | `Ashfall.Core.Tests/AccessibilityComplianceTests.cs`, selftest verb | None | Steps 2–6 |

**Total estimated effort:** 5–7 focused sessions
**Prerequisite:** None — fully additive to existing codebase
**Exit criteria:** `--accessibility-selftest` exits 0; audit report shows 0 violations at AA level (standard) and AAA level (high-contrast variant)

---

## Review Notes (Corrected)

This plan was adversarially reviewed against the real codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. The following errors were found and fixed:

1. **Panel count was imprecise.** "~85 UI panels" conflated two different real numbers: 83 files literally named `*Panel.cs`, and 97 total `.cs` files in `src/UI/` (the extra ~14 are shared widgets, modals, HUD overlays, and non-panel infrastructure — `AshfallUiHelpers`, `MainMenuBuilder`, `CombatHudOverlay`, `GameHudOverlay`, `DailyBriefingModal`, `OpeningProtocolModal`, `SnapshotHarness`, `SnapshotOrchestrator`, `UiBackgroundCarousel`, plus the 5 `Ashfall*` shells). Fixed: Context section now states both real numbers explicitly and Done-when criteria in Step 2 reference "83" directly instead of a vague "~85".
2. **The five `Ashfall*` widget classes, `Ashfall.Core.UI.Theme`, its 7 named color members, and the `BarlowCondensed`/`ShareTechMono` font usage were all VERIFIED CORRECT** — no changes needed there, but the Context section now cites exact file:line evidence instead of asserting these claims without support. Also added a note that `src/UI/` code accesses the theme through a `DesignTheme` alias, not the fully-qualified `Ashfall.Core.UI.Theme` name — anyone grepping for the fully-qualified name in `src/UI/` (as Step 5's implementation would require) will find nothing and wrongly conclude the theme isn't used there.
3. **`scripts/audit/accessibility-audit.cs` and the `--accessibility-audit` CLI verb do not exist anywhere in the codebase — they were fabricated/aspirational, not existing infrastructure to build on.** Confirmed via exhaustive grep: zero matches for `accessibility-audit`, `accessibility_audit`, or `AccessibilityAudit` in any file including the CLI parser (`src/Host/HostCli.cs`). Fixed: Step 1's Implementation section now explicitly states this is new work, redirects the file location to `src/Host/HostCli.AccessibilityAudit.cs` (matching the existing `HostCli.SelfTests.cs` partial-class convention used by the two real, working selftest verbs `--data-integrity-selftest` and `--bridge-selftest`), and cites the exact real precedent (`HostCli.cs:239-240`, `HostCli.SelfTests.cs:32-44`) to follow instead of inventing a parallel mechanism.
4. **Vague Done-when criteria tightened to concrete, testable assertions.** E.g. Step 1's "Baseline numbers documented" now requires the report to literally contain a per-panel entry for all 83 panels + 5 widgets and a concrete non-negative integer violation count (not just "high"). Step 3's "no shortcut conflicts with Godot engine defaults" now also requires checking for conflicts with existing panel input handlers, which is the actually-relevant risk in this codebase (85+ files with their own `_Input`/`_UnhandledInput` overrides are far more likely to collide than Godot's own reserved defaults).
5. **Missing risk/rollback notes added for the four genuinely risky steps.** The batch header claims blanket "Risk: Low — additive, no behavioral changes to game logic" for the whole batch, but Steps 2, 3, 5, and 6 mutate shared, widely-consumed infrastructure (`Main._UnhandledInput`, the 5 `Ashfall*` widgets used across dozens of panels, `BuildUserInterface()`'s **16** call sites in the 7,014-line `Main.cs`, and — for Step 6 — every `Control` node's font-size override across all 83 panels). Added explicit Risk/Rollback subsections to Steps 2, 3, 5, and 6, and corrected their risk rating in the Summary table from blanket "Low" to "Medium" with a rationale pointer.
6. **Unrunnable/unverifiable verification commands fixed.** Step 1's original verification said "Manually review report for baseline numbers" with no pass/fail criterion — replaced with a concrete JSON-validity check (`python3 -m json.tool`) so the step has an actual automatable gate, not just a manual eyeball.
7. **No incorrect file/class references were found for the five `Ashfall*` widgets, `Theme`, or font usage — these were the plan's most solid claims and are left substantively unchanged** beyond adding citations.

### Second adversarial pass (this review) — errors that survived the first pass

The prior "Review Notes" above were themselves re-verified against the live codebase rather than trusted at face value. Two concrete errors were found in claims the first pass had marked as settled:

8. **`BuildUserInterface()` call-site count was wrong even after the first review pass.** The existing Step 2 Risk/Rollback text asserted "4 separate sites (`src/Main.cs:507, 4099, 4196, 4446`)". Direct `grep -n "BuildUserInterface();" src/Main.cs` returns **16** call sites: 507, 4099, 4196, 4446, 4479, 4667, 4945, 4978, 5020, 5079, 5132, 5169, 5222, 5255, 5303, 5327. Reading the surrounding context at each site shows only the first 3 are "real" gameplay paths (initial startup, then two post-reset rebuilds); the other 13 are headless `--*-ui-test-and-quit` CLI self-test entry points, each invoking `BuildUserInterface()` exactly once before exiting. This matters for Step 2's mitigation strategy: calling `FocusChainBuilder.BuildChain` from an external call site (rather than as the last line inside `BuildUserInterface()` itself) would require touching 16 locations correctly, not 4. Fixed by moving the call inside the method body (see Implementation) and correcting the Risk/Rollback section with the full site list and classification.
9. **Step 6's `--font-scale=2.0` CLI argument was asserted as if it already existed, with no step ever specifying it needed to be built.** Unlike `--accessibility-audit` (whose Step 1 Implementation explicitly says "this step builds it from scratch"), the `--font-scale=` flag used in Step 6's Verification block was never declared as new work anywhere in the plan, and `grep -n "font-scale" src/Host/HostCli.cs` returns zero matches — confirming it does not exist. As written, the verification command would fail on an unrecognized argument. Fixed: Step 6's Implementation now explicitly calls out that this argument must be added to the `--accessibility-audit` handler as part of this step, and Done-when now requires confirming it is a real, wired argument rather than assuming Step 1's audit tool already supports it.
10. **Step 6 (font scaling) had no Risk/Rollback section at all**, despite touching the same widely-shared `Ashfall*` widget classes flagged as Medium risk in Step 5, plus every `Control` with a font override across all 83 panels. Added, along with a specific warning about the double-scaling bug class (re-snapshotting base sizes after a scale is already applied).
