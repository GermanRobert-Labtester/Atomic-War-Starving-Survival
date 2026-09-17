# ASHFALL — Quality Roadmap Batch 81

## Theme: Input Rebinding & Control Scheme Management

**Priority:** MEDIUM (accessibility + player preference)
**Risk:** Medium — additive at the data-model/UI layer, but functionally incomplete on its own: it introduces `InputMap` to a codebase that has zero prior `InputMap` usage, and does not migrate the 84 existing `_UnhandledInput` panel handlers to consume it (see Context and Step 4). Shipping Steps 1-7 alone produces a rebinding UI whose changes do not affect actual gameplay input. Treat this batch as "data model + UI scaffold" and track the panel-migration work (Batch 81b) as a required follow-up, not an optional nice-to-have.
**Batch:** 81
**Systems affected:** Main.cs (input handling), all UI panels (keyboard shortcuts), Godot InputMap
**Depends on:** None (standalone feature)
**Unlocks:** Gamepad support, accessibility compliance, Steam Input integration, remappable controls menu (all conditional on the Batch 81b panel migration landing)

---

## Context

The Godot host handles input in `Main.cs` and individual UI panels. Verified against the actual codebase (2024 audit pass):

- Keyboard shortcuts are hardcoded via raw `InputEventKey.Keycode` checks inside `_UnhandledInput` overrides — **not** `Input.IsActionJustPressed("ui_accept")` style calls (that example in the original draft was wrong; no `Input.IsAction*` calls exist in `src/` at all). Confirmed pattern, e.g. `src/Main.cs:6292-6294` (`_UnhandledInput` checking `key.Keycode == Key.Escape`), `src/Host/HoldfastTerminalPanel.cs:329-348` (`Key.Escape`, `Key.B`, `Key.S`), `src/UI/DailyBriefingModal.cs:146-156` (`Key.Enter`, `Key.Space`, `Key.Tab`).
- Grep confirms **84 files** under `src/` implement `_UnhandledInput`/`_Input` with hardcoded `Key.*` checks (out of 91 `*Panel*.cs` files total) — this is a larger surface than "individual UI panels" suggests; the audit step (Step 1) must budget for cataloging closer to 84 files, not a handful.
- **Correction — this is more severe than originally stated:** `InputMap` is used **nowhere** in `src/` (0 matches) and `project.godot` has **no `[input]` section at all** (0 matches for `[input]`). The original claim ("Godot has InputMap... may not use it consistently") implied partial/inconsistent adoption. The reality is zero adoption — every single bindable action in the game is a raw keycode check with no Godot-level indirection. This *simplifies* Step 1 (no InputMap actions to cross-reference, no dead-entry analysis needed) but means Step 4's `GodotInputMapper` is introducing `InputMap` usage into the project for the first time, not migrating an existing partial usage — this adds risk that should be called out (see Risk & Rollback below).
- No rebinding UI exists — confirmed, no `RebindingPanel`/`InputSettingsPanel`/`KeyCaptureOverlay`-equivalent file exists anywhere in `src/`.
- No gamepad support — confirmed, no `JoyButton`/`JoyAxis`/`get_connected_joypads` references found in `src/`.
- No input persistence — confirmed, but note the project already has an established pattern for exactly this kind of user-preference persistence: `src/Audio/AudioSettings.cs` persists versioned, non-save JSON preferences to `user://audio_settings.json` (atomic write, malformed-file recovery, defaults, reset-to-default). Step 4's scheme persistence should follow this existing pattern (same `user://` directory, same versioned-JSON approach) rather than invent a new persistence convention.
- Accessibility requirements: some players cannot use standard WASD/arrow layouts, need single-hand schemes, or require hold-to-confirm instead of tap.

Players need:
- Rebindable keys for all game actions.
- Multiple control schemes (keyboard-only, keyboard+mouse, gamepad).
- Input persistence across sessions (saved to user profile, not game save).
- Conflict detection (two actions bound to same key → warning).
- Reset to defaults (per-scheme and global).

The input system belongs partially in Core (action definitions, scheme data, conflict logic) and partially in the Godot host (actual InputMap wiring, UI for rebinding).

---

## Step 1 — Audit Current Input Handling

**Goal:** Produce a complete map of every input check in the Godot host, identifying which are Godot InputMap actions vs. raw key checks, and what logical action each represents.

**Implementation:**
- Search `src/` for all `_UnhandledInput`/`_Input` overrides and the `InputEventKey`/`key.Keycode ==` checks inside them (confirmed pattern: 84 files, see Context). There are no `Input.IsActionJustPressed`/`Input.IsActionPressed`/`InputMap` call sites to search for today — do not budget audit time for that; the entire surface is raw keycode checks.
- For each of the 84 files, record: file, line, the key checked (e.g., `Key.Escape`, `Key.J`, `Key.F1`), and the logical game action it triggers (e.g., "close overlay panel", "open journal", "toggle developer console").
- Note the dominant repeated pattern first, then catalog exceptions: the overwhelming majority of hits are `if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape) { OnClose?.Invoke(); }` — a single "close this panel" action duplicated per-panel, not 84 distinct logical actions. Deduplicate by logical action, not by file, when building the action list.
- Confirm `project.godot` has no `[input]` section (already verified — 0 matches) so this sub-step is a one-line confirmation, not an investigation.
- Produce `docs/INPUT_AUDIT.md`: table of (logical action, current binding(s), source file(s), count of duplicate implementations).
- Count: expect roughly 15–25 distinct *logical* actions once the repeated "Escape closes panel" pattern is deduplicated across ~84 files — do not report 84 as the action count.

**Verification:**
- `docs/INPUT_AUDIT.md` exists with complete input mapping and explicitly separates "distinct logical actions" from "raw call sites" (avoids double-counting the repeated Escape-to-close pattern as N different actions).
- Every file identified via `grep -rl "_UnhandledInput\|_Input(InputEvent" src/ --include=*.cs` (84 files as of this writing) is accounted for in the audit table.
- `dotnet build Ashfall.csproj` still compiles (audit is read-only, no source changes).
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` passes (no code changes — this step touches no `.cs` files, only adds a doc).

**Done when:** `docs/INPUT_AUDIT.md` exists, all 84 input-handling files are represented in it, and the deduplicated logical-action list (not the raw file count) is finalized and reused as-is in Step 2's registry.

---

## Step 2 — Define Canonical Input Actions

**Goal:** Establish the authoritative list of input actions for the game, organized by category, with default bindings for keyboard and gamepad.

**Implementation:**
- Create `Assets/Ashfall.Core/Input/InputActions.cs`:
  ```csharp
  namespace Ashfall.Core.Input;

  public static class InputActions
  {
      // Navigation
      public const string NavigateUp = "navigate_up";
      public const string NavigateDown = "navigate_down";
      public const string NavigateLeft = "navigate_left";
      public const string NavigateRight = "navigate_right";

      // Core gameplay
      public const string AdvanceDay = "advance_day";
      public const string QuickSave = "quick_save";
      public const string QuickLoad = "quick_load";
      public const string Pause = "pause";

      // UI panels
      public const string OpenInventory = "open_inventory";
      public const string OpenMap = "open_map";
      public const string OpenJournal = "open_journal";
      public const string OpenCrafting = "open_crafting";
      // ... (all panels)

      // Dialog / confirmation
      public const string Confirm = "confirm";
      public const string Cancel = "cancel";
      public const string SkipDialog = "skip_dialog";

      // Accessibility
      public const string HoldToConfirm = "hold_to_confirm";
  }
  ```
- Create `Assets/Ashfall.Core/Input/InputCategory.cs` — enum: `Navigation`, `Gameplay`, `Panels`, `Dialog`, `System`, `Accessibility`.
- Create `Assets/Ashfall.Core/Input/InputActionDefinition.cs`:
  ```csharp
  public sealed class InputActionDefinition
  {
      public string ActionId { get; init; }
      public InputCategory Category { get; init; }
      public string DisplayName { get; init; }
      public bool Rebindable { get; init; } = true;
      public bool AllowHold { get; init; } = false;
  }
  ```
- Create `Assets/Ashfall.Core/Input/InputActionRegistry.cs` — static registry of all `InputActionDefinition` instances, queryable by category.
- Ensure all action IDs use `snake_case` per project convention.
- Document each action with a one-line description of what it does.

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles cleanly.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` passes.
- New test: `InputActionRegistryTests.AllActionsHaveUniqueIds` — no duplicate action IDs.
- New test: `InputActionRegistryTests.AllActionsHaveDisplayNames` — no empty display names.
- All actions from the Step 1 audit are covered by the registry.

**Done when:** Canonical action list is defined in Core, registry is testable, and all audit actions are mapped.

---

## Step 3 — Create InputScheme Data Model in Core

**Goal:** Define the serializable data structure for a complete input scheme (action-to-binding mapping), supporting keyboard and gamepad bindings with scheme metadata.

**Implementation:**
- Create `Assets/Ashfall.Core/Input/InputBinding.cs`:
  ```csharp
  namespace Ashfall.Core.Input;

  public sealed class InputBinding
  {
      public string ActionId { get; set; }
      public string KeyboardPrimary { get; set; }   // e.g., "Key_E", "Key_Space"
      public string KeyboardSecondary { get; set; } // optional alternate
      public string GamepadButton { get; set; }     // e.g., "JoyButton_A", "JoyAxis_LeftX+"
      public string MouseButton { get; set; }       // e.g., "Mouse_Left", "Mouse_ScrollUp"
  }
  ```
- Create `Assets/Ashfall.Core/Input/InputScheme.cs`:
  ```csharp
  public sealed class InputScheme
  {
      public string SchemeId { get; set; }          // e.g., "default_keyboard", "one_hand_left"
      public string DisplayName { get; set; }
      public string Description { get; set; }
      public bool IsBuiltIn { get; set; }          // true = cannot delete, can reset
      public List<InputBinding> Bindings { get; set; } = new();
  }
  ```
- Create `Assets/Ashfall.Core/Input/InputSchemeValidator.cs`:
  - `ValidateScheme(InputScheme scheme)` → list of issues (missing required actions, duplicate bindings).
  - `FindConflicts(InputScheme scheme)` → list of `(actionA, actionB, conflictingKey)` tuples.
  - `IsComplete(InputScheme scheme)` → true if all required (non-optional) actions have at least one binding.
- Create default schemes:
  - `default_keyboard` — standard WASD + common shortcuts.
  - `default_gamepad` — Xbox-style layout.
  - `one_hand_left` — all controls reachable with left hand only (accessibility).
- Schemes serialize to JSON in `Assets/StreamingAssets/Data/input_schemes/` (data authority).

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles cleanly.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` passes.
- New tests:
  - `InputSchemeValidator_DetectsConflict` — two actions on same key flagged.
  - `InputSchemeValidator_DetectsMissingAction` — scheme missing required action flagged.
  - `InputScheme_SerializationRoundTrip` — JSON save/load preserves all fields.
  - `DefaultSchemes_AreComplete` — all built-in schemes pass `IsComplete`.
- Default scheme JSON files exist in `Assets/StreamingAssets/Data/input_schemes/`.

**Done when:** InputScheme model is defined, validator detects conflicts/gaps, default schemes are complete, and round-trip serialization works.

---

## Step 4 — Implement InputSchemeManager

**Goal:** Create the runtime manager that loads/saves schemes, tracks the active scheme, applies bindings to Godot's InputMap, and handles scheme switching.

**Implementation:**
- Create `Assets/Ashfall.Core/Input/IInputSchemeManager.cs` (port interface):
  ```csharp
  public interface IInputSchemeManager
  {
      InputScheme ActiveScheme { get; }
      IReadOnlyList<InputScheme> AvailableSchemes { get; }
      void SetActiveScheme(string schemeId);
      void UpdateBinding(string actionId, InputBinding newBinding);
      void ResetToDefaults(string schemeId);
      void SaveSchemes();
      void LoadSchemes();
      InputScheme CreateCustomScheme(string displayName, string baseSchemeId);
      void DeleteCustomScheme(string schemeId); // fails for built-in
      IReadOnlyList<BindingConflict> GetConflicts();
  }
  ```
- Create `Assets/Ashfall.Core/Input/InputSchemeManager.cs` — Core implementation handling data logic (load from JSON, validate, detect conflicts, persist).
- Create `src/Input/GodotInputMapper.cs` — Godot host adapter that:
  - Translates `InputBinding` key strings to Godot `InputEvent` objects.
  - Calls `InputMap.ActionEraseEvents(action)` then `InputMap.ActionAddEvent(action, event)` for each binding.
  - Handles scheme switching by clearing and re-applying all actions.
- **Critical integration note (new — not in original draft):** This step introduces `InputMap` to the project for the first time (confirmed 0 prior usages). The 84 existing `_UnhandledInput` panel handlers do **not** read from `InputMap` — they check `key.Keycode` directly. Populating `InputMap` alone does nothing until each of the 84 handlers is migrated to check `Input.IsActionJustPressed(actionId)` instead of a raw keycode. That migration is *not scoped in this batch* — Steps 1-7 only build the scheme data model, manager, and UI; they do not rewire the 84 existing panels to consume it. Without that follow-up work, the rebinding UI will change `InputMap` bindings that have no effect on actual gameplay input, which is a misleading player-facing feature. Add an explicit note to the panel and to the batch summary that a **Batch 81b — "Migrate panel input to InputMap actions"** follow-up is required before this feature is functionally complete, and that Step 5's manual test plan must verify end-to-end (rebind a key → confirm the actual panel now responds to the new key, not just that `InputMap` was updated).
- Persist active scheme selection to user preferences using the same pattern as `src/Audio/AudioSettings.cs` (`user://audio_settings.json` — versioned JSON, atomic write, malformed-file recovery, defaults, reset-to-default). Store schemes at `user://input_schemes.json` or equivalent, separate from game save files (this is a player preference, not game state) — do not invent a new persistence convention when `AudioSettings.cs` already establishes one.
- On startup: load schemes → apply active scheme to InputMap → ready.
- On scheme change: validate → apply → persist selection.

**Verification:**
- `dotnet build Ashfall.csproj` compiles cleanly.
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles cleanly.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` passes.
- New tests:
  - `InputSchemeManager_LoadsSavedSchemes` — persisted schemes restore correctly.
  - `InputSchemeManager_SwitchScheme_UpdatesActive` — switching changes `ActiveScheme`.
  - `InputSchemeManager_CustomScheme_CanCreateAndDelete` — CRUD for custom schemes.
  - `InputSchemeManager_BuiltIn_CannotDelete` — delete on built-in throws/returns error.
- `godot --headless --path . -- --data-integrity-selftest` passes (new JSON files are valid).

**Done when:** Manager loads/saves schemes, applies to Godot `InputMap`, handles switching, all tests pass, and the summary explicitly states that `InputMap` now holds the bindings but no existing panel yet reads from it (that migration is out of scope for this batch — see Batch 81b note above).

---

## Step 5 — Create Input Rebinding UI Panel

**Goal:** Build a player-facing UI panel where users can view current bindings, rebind keys, switch schemes, and see conflict warnings.

**Implementation:**
- Create `src/UI/InputSettingsPanel.cs` (Godot `Control` node):
  - **Scheme selector** — dropdown listing available schemes (built-in + custom).
  - **Action list** — scrollable list grouped by `InputCategory`, showing action display name + current binding.
  - **Rebind flow** — click a binding → "Press a key..." overlay → capture next `InputEvent` → assign → highlight conflicts.
  - **Conflict indicator** — orange warning icon next to conflicting bindings, tooltip shows which other action conflicts.
  - **Reset button** — per-scheme reset to defaults (with confirmation dialog).
  - **Create custom scheme** — button to clone current scheme into editable custom scheme.
  - **Delete custom scheme** — available only for user-created schemes.
- Create `src/UI/KeyCaptureOverlay.cs` — modal overlay that captures the next key/button press and returns it to the rebind flow.
- Follow existing UI patterns in `src/UI/` (programmatic construction, project fonts BarlowCondensed/ShareTechMono, dark theme).
- Ensure keyboard-navigable (Tab between fields, Enter to activate rebind, Escape to cancel).
- Support mouse click capture (for mouse button bindings).
- Gamepad button capture (for gamepad bindings).

**Verification:**
- `dotnet build Ashfall.csproj` compiles cleanly.
- `godot --headless --path . -- --data-integrity-selftest` passes.
- Manual testing (documented test plan):
  - Open panel → all actions visible with current bindings.
  - Click binding → overlay appears → press key → binding updates.
  - Create conflict → warning appears on both conflicting actions.
  - Switch scheme → all bindings update.
  - Reset → confirms → bindings return to defaults.
  - Escape from capture overlay → no change applied.
  - **Explicitly confirm the known gap:** rebinding e.g. "close panel" away from Escape does **not** change the behavior of any existing panel (all 84 panels still hard-check `Key.Escape` directly) — this is expected per the Step 4 integration note, not a bug, but the test plan must record it so it isn't mistaken for a regression later.
- Panel is keyboard-navigable without mouse.

**Done when:** Rebinding panel is functional against the `InputScheme`/`InputSchemeManager` data model, conflicts are visible, schemes are switchable, the UI follows project conventions, and the manual test plan explicitly documents that changes do not yet propagate to actual panel behavior (pending Batch 81b).

---

## Step 6 — Add Gamepad Mapping Defaults

**Goal:** Define comprehensive gamepad bindings for all input actions, supporting Xbox and PlayStation-style controllers, with proper axis handling for navigation.

**Implementation:**
- Define `default_gamepad` scheme in `Assets/StreamingAssets/Data/input_schemes/default_gamepad.json`:
  - D-pad / Left stick → navigation.
  - A/Cross → Confirm.
  - B/Circle → Cancel.
  - X/Square → context action (crafting, interact).
  - Y/Triangle → open quick menu.
  - Bumpers → cycle panels (LB = previous, RB = next).
  - Triggers → scroll lists (analog scroll speed).
  - Start → Pause / advance day (context-dependent).
  - Select/Back → open map.
  - Stick click → quick save (hold for 1s to prevent accidental).
- Create `src/Input/GamepadGlyphMapper.cs`:
  - Maps internal button names to display glyphs based on detected controller type.
  - Xbox: A/B/X/Y glyphs.
  - PlayStation: Cross/Circle/Square/Triangle glyphs.
  - Generic: Button 0/1/2/3 text.
- Update `InputSettingsPanel` to show correct glyphs for gamepad bindings.
- Handle analog stick deadzone (configurable, default 0.2).
- Handle trigger threshold (configurable, default 0.5 for digital-style press).
- Add `InputScheme.ControllerType` field: `Keyboard`, `Gamepad_Xbox`, `Gamepad_PlayStation`, `Gamepad_Generic`.
- Auto-detect connected controller type on startup (Godot C# API: `Input.GetConnectedJoypads()`, not the GDScript-style `Input.get_connected_joypads()` — the original draft used GDScript naming convention in a C#-only project; every other Godot API call in `src/` uses PascalCase, e.g. `Input.IsActionJustPressed`, `GetViewport().SetInputAsHandled()`).

**Verification:**
- `dotnet build Ashfall.csproj` compiles cleanly.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` passes.
- `godot --headless --path . -- --data-integrity-selftest` passes (new JSON validates).
- New tests:
  - `GamepadScheme_IsComplete` — all required actions have gamepad bindings.
  - `GamepadGlyphMapper_XboxLayout` — correct glyphs for Xbox buttons.
  - `GamepadGlyphMapper_PlayStationLayout` — correct glyphs for PS buttons.
  - `GamepadScheme_DeadzoneRespected` — axis values below deadzone don't trigger.
- Default gamepad scheme JSON exists and passes `InputSchemeValidator`.

**Done when:** Gamepad scheme is defined, glyph mapping works for Xbox/PS, deadzone is configurable, and all tests pass.

---

## Step 7 — Write Input Scheme Persistence & Integration Tests

**Goal:** Verify end-to-end input scheme lifecycle: creation, modification, persistence across sessions, conflict resolution, and reset behavior.

**Implementation:**
- Create `Ashfall.Core.Tests/Input/InputSchemeIntegrationTests.cs`:
  - **Test: SchemeRoundTrip_PersistsAcrossSessions** — Create manager, modify bindings, save, create new manager instance, load → bindings match.
  - **Test: ConflictDetection_TwoActionsOnSameKey** — Bind two actions to same key → `GetConflicts()` returns the pair.
  - **Test: ConflictResolution_RebindRemovesConflict** — After rebinding one conflicting action → conflicts list is empty.
  - **Test: ResetToDefaults_RestoresOriginalBindings** — Modify scheme, reset → matches built-in default exactly.
  - **Test: CustomScheme_ClonesCorrectly** — Clone built-in → custom has same bindings, different ID, `IsBuiltIn = false`.
  - **Test: DeleteBuiltIn_Fails** — Attempt to delete built-in scheme → operation rejected.
  - **Test: ActiveSchemeSelection_PersistsIndependently** — Active scheme choice persists even if game save is deleted (it's user preference, not game state).
  - **Test: MissingSchemeFile_FallsBackToDefault** — If custom scheme JSON is deleted externally → manager gracefully falls back to `default_keyboard`.
  - **Test: AllDefaultSchemes_HaveNoConflicts** — Built-in schemes ship conflict-free.
  - **Test: SchemeValidation_RejectsEmptyActionId** — Binding with empty `ActionId` is rejected.
- Create `Ashfall.Core.Tests/Input/InputActionRegistryTests.cs`:
  - **Test: AllActions_UseSnakeCase** — No action ID contains uppercase or spaces.
  - **Test: AllCategories_HaveAtLeastOneAction** — No empty category.
  - **Test: ActionIds_MatchConstantValues** — Registry entries match `InputActions` constants.
- Ensure tests use in-memory `IFileIO` (no disk dependency in Core tests).
- Run full verification suite after all tests are green.

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles cleanly.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all new + existing tests pass.
- `dotnet build Ashfall.csproj` compiles cleanly.
- `godot --headless --path . -- --data-integrity-selftest` passes.
- `godot --headless --path . -- --bridge-selftest` exits 0.
- No flaky tests across 5 consecutive runs.

**Done when:** All 13 integration tests pass, full verification suite is green, input system is feature-complete and tested.

---

## Summary

| Step | Deliverable | Risk | New Tests | Key Files |
|------|-------------|------|-----------|-----------|
| 1 | `docs/INPUT_AUDIT.md` — current input mapping | None (read-only) | 0 | All `src/` input handlers |
| 2 | `InputActions.cs`, `InputActionRegistry.cs` — canonical action list | Low | 2 | `Assets/Ashfall.Core/Input/` |
| 3 | `InputScheme.cs`, `InputSchemeValidator.cs` + default schemes | Low | 4 | `Assets/Ashfall.Core/Input/`, `Assets/StreamingAssets/Data/input_schemes/` |
| 4 | `InputSchemeManager` + `GodotInputMapper` | Low | 4 | `Assets/Ashfall.Core/Input/`, `src/Input/` |
| 5 | `InputSettingsPanel` + `KeyCaptureOverlay` UI | Low | 0 (manual test plan) | `src/UI/` |
| 6 | Gamepad defaults + glyph mapper | Low | 4 | `Assets/StreamingAssets/Data/input_schemes/`, `src/Input/` |
| 7 | Integration test suite — full lifecycle coverage | None | 13 | `Ashfall.Core.Tests/Input/` |

**Total new tests:** 27
**Total new files:** ~12 (Core input model + Godot adapters + UI + test files + JSON schemes)
**Estimated effort:** 4–6 days for Steps 1-7 as scoped. This does **not** include migrating the 84 existing `_UnhandledInput` panel handlers to read from `InputMap`/the new scheme system — that is a separate follow-up batch (see Step 4). Underestimating this split was the single biggest scope risk found in review; budget the follow-up batch separately rather than assuming it's covered here.
**Exit criteria:** All existing tests pass (2,120 `[Fact]`/`[Theory]` tests as of this review — verify the current count with `dotnet test` immediately before starting, do not rely on a stale figure from a prior batch), 27 new input tests pass, rebinding UI is functional against the data model, gamepad defaults work, schemes persist across sessions via the `AudioSettings.cs`-pattern `user://` file, full verification suite green. "No flaky tests across 5 consecutive runs" (Step 7) means: run `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` five times in a row with no other changes and confirm identical pass/fail results each time — call this out explicitly as a manual CI step, since no test runner flag in this repo automates repeat-run flake detection.

## Risk & Rollback

- **Risk:** Introducing `InputMap` populated at runtime by `GodotInputMapper` could collide with any *future* code that also touches `InputMap` (e.g. if a later batch starts using Godot's built-in `ui_accept`/`ui_cancel` actions, which currently go unused per this audit). Mitigate by documenting in `docs/INPUT_AUDIT.md` that all `InputMap` population is now owned exclusively by `GodotInputMapper`.
- **Risk:** JSON scheme files under `Assets/StreamingAssets/Data/input_schemes/` are new data-authority files; a malformed hand-edit could fail `--data-integrity-selftest` for the whole project, not just input. Mitigate with `InputSchemeValidator` tests before any file ships, per Step 3.
- **Rollback:** Because no existing panel code is touched until Batch 81b, rolling back this entire batch is a pure deletion of new files (`Assets/Ashfall.Core/Input/`, `src/Input/`, `src/UI/InputSettingsPanel.cs`, `src/UI/KeyCaptureOverlay.cs`, `Assets/StreamingAssets/Data/input_schemes/`, `Ashfall.Core.Tests/Input/`) plus removing their wiring from `Main.cs`/settings menu. No existing panel behavior is modified, so rollback carries no risk of regressing current input handling.
- **Rollback for Batch 81b (future):** Once panels are migrated to read from `InputMap`, rollback becomes higher-risk (touches 84 files). That migration should land in small reviewable increments (a handful of panels per commit) specifically so a bad migration can be reverted per-panel rather than as one large revert.


## Review Notes (Corrected)

Adversarial review performed against the real codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` (Godot host). Findings and fixes applied in place above:

1. **Factual error — input handling mechanism.** Original draft implied a partial/inconsistent mix of `Input.IsActionJustPressed(...)`-style calls and raw key checks, with `InputMap` "not used consistently." Verified via grep: `InputMap` has **zero** usages anywhere in `src/`, `project.godot` has **no `[input]` section**, and there is not a single `Input.IsAction*` call in the codebase. 100% of input handling is raw `InputEventKey.Keycode` comparisons inside `_UnhandledInput`/`_Input` overrides, confirmed across 84 files. Corrected the Context and Step 1 implementation accordingly, and removed the fabricated `if (Input.IsActionJustPressed("ui_accept"))` example that does not exist in this project.

2. **Missing consequence — rebinding UI would be non-functional without a follow-up.** The original 7 steps build a complete `InputScheme`/`InputSchemeManager`/`GodotInputMapper` stack and a rebinding UI, but never touch the 84 existing panel files that hardcode `key.Keycode ==` checks. As drafted, a player could "rebind" an action in the new UI and see zero effect in actual gameplay, because no panel reads `InputMap` today. This is a critical scope gap that would have shipped a misleading feature. Added an explicit Step 4 integration note, updated Step 5's manual test plan to catch this, flagged it in the header Risk rating (changed Low → Medium), and named it as a required follow-up ("Batch 81b").

3. **Scale underestimate in Step 1.** The audit step as drafted didn't establish a file count, risking an open-ended "search src/" task. Pinned it to the actual number (84 files with `_UnhandledInput`/`_Input`) and clarified that the dominant pattern (Escape-to-close, duplicated per panel) should collapse to ~15-25 logical actions, not be miscounted as 84 distinct actions.

4. **Unverifiable/incorrect API reference.** Step 6 used GDScript naming (`Input.get_connected_joypads()`) in a project that is C#-only throughout `src/`. Corrected to the actual C# Godot API name, `Input.GetConnectedJoypads()`.

5. **No established persistence pattern referenced.** The plan invented scheme persistence from scratch. The project already has a working, tested precedent for exactly this (`src/Audio/AudioSettings.cs`, versioned JSON at `user://audio_settings.json`, atomic write + malformed-file recovery). Step 4 now points at this pattern instead of leaving the persistence approach unspecified.

6. **Stale/unverified numbers.** "1941+ existing tests" could not be verified against this repo snapshot; the actual count via `grep -rc "\[Fact\]\|\[Theory\]"` is 2,120. Replaced the hardcoded figure with an instruction to verify the live count before starting, so the plan doesn't silently pass a wrong exit-criterion check.

7. **Unrunnable verification criterion.** "No flaky tests across 5 consecutive runs" had no defined mechanism. Clarified it as a manual 5x-repeat of `dotnet test`, since the repo has no automated flake-detection tooling.

8. **Missing Risk & Rollback section.** The original plan had no dedicated rollback guidance beyond the one-line header risk rating. Added a full Risk & Rollback section distinguishing the low-risk, easily-reversible Steps 1-7 (additive only, no existing panel touched) from the higher-risk future panel-migration work.

All corrections above are integrated into the step-by-step content above, not just listed here.
