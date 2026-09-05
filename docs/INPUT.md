# ASHFALL — INPUT ARCHITECTURE, VERBS & REBINDING SPECIFICATION (PLAN 81 / TASK B22)

**Classification:** Core Input & Navigation Authority
**Author:** AI Pair Programmer / Antigravity
**Status:** Implemented & CI-Gated
**Enforcement Test:** `Ashfall.Core.Tests/UI/InputVerbRoutingTests.cs`
**Registry Audit:** `artifacts/input-map-audit.json`

---

## 1. Executive Summary

ASHFALL maps player input to high-level semantic verbs rather than physical keyboard or gamepad buttons. This abstraction guarantees full keyboard accessibility, controller support, persistent user rebinding, conflict prevention, and graceful recovery from corrupted settings files without crashing or blocking the game loop.

---

## 2. Canonical Input Verbs

All UI panels and host systems consume input exclusively through the canonical actions defined in `src/Host/AshfallInputActions.cs` and mirrored in `project.godot`:

| Action Name | Default Key | Controller Binding | Verb Purpose | Rebindable |
|---|---|---|---|---|
| `ashfall_close` / `ui_cancel` | `Escape` | `JoyButton.B` | Dismiss active overlay / pop topmost modal | Fixed (Safety Anchor) |
| `ashfall_confirm` / `ui_accept` | `Enter` / `Space` | `JoyButton.A` | Confirm dialog, commit row, press focused button | Yes |
| `ashfall_next_tab` / `ui_focus_next`| `Tab` | `JoyButton.RightShoulder` | Focus next element or advance multi-tab book | Yes |
| `ashfall_nav_up` | `W` / `Up` | `DpadUp` | Navigate upward in lists and grids | Yes |
| `ashfall_nav_down` | `S` / `Down` | `DpadDown` | Navigate downward in lists and grids | Yes |
| `ashfall_nav_left` | `A` / `Left` | `DpadLeft` | Navigate leftward across columns | Yes |
| `ashfall_nav_right` | `D` / `Right` | `DpadRight` | Navigate rightward across columns | Yes |
| `ashfall_journal` | `J` | `JoyButton.Y` | Open campaign journal and archive | Yes |
| `ashfall_help` | `F1` | `JoyButton.Back` | Open survival guide & onboarding codex | Yes |
| `ashfall_forecast` | `F` | — | Open weather & fallout radar | Yes |
| `ashfall_weather_history` | `H` | — | Open barometric fallout history | Yes |
| `ashfall_events` | `E` | — | Open shelter crisis events log | Yes |
| `ashfall_expeditions` | `X` | — | Open wasteland expedition dispatch | Yes |
| `ashfall_holdfast` | `T` | — | Open holdfast terminal console | Yes |
| `ashfall_holdfast_build` | `B` | — | Open room construction modal | Yes |
| `ashfall_holdfast_status` | `U` | — | Open shelter infrastructure telemetry | Yes |

---

## 3. Conflict Detection & Rebinding Protocol

1. **Safety Anchors**:
   - `ashfall_close` (`Escape`) is non-rebindable to ensure players can never become permanently trapped in a modal or menu without a reliable escape key.
2. **Conflict Prevention**:
   - When rebinding through `SettingsPanel`, `AshfallInputActions.TryRebindAction` inspects all active actions in `InputMap`.
   - If a proposed key is already bound to another action (e.g. attempting to bind `NavDown` to `W`), the action is rejected with an on-screen warning:
     `"Conflict: [W] is already assigned to 'ashfall_nav_up'!"`
3. **Interactive Rebinding Workflow in `SettingsPanel`**:
   - Clicking an action button displays `"[PRESS ANY KEY]"` in amber.
   - The next pressed physical key is tested for conflicts and applied to `InputMap`.
   - Pressing `Escape` while listening cancels the rebinding without altering existing binds.
   - The `"RESET DEFAULT KEYS"` button restores all actions to default assignments.

---

## 4. Persistence & Corruption Recovery

- Key bindings are saved in `user://settings.json` under the `"key_bindings"` map within `UserSettingsData`.
- `UserSettingsStore` performs atomic temp file replacement (`settings.json.tmp` → `settings.json`) to prevent partial writes.
- If `settings.json` is corrupted, missing, or contains unparseable key strings, `UserSettingsCodec.DeserializeWithRecovery` resets `key_bindings` to an empty map and restores canonical defaults without throwing.

---

## 5. Architectural Invariants

1. **No Raw Escape Keys in Panels**:
   - Panels must test `AshfallInputActions.IsCloseOrCancel(@event)` or delegate through `AshfallFocusPolicy.TrapFocus`.
   - Direct comparisons such as `key.Keycode == Key.Escape` are prohibited outside `SettingsPanel` (which possesses an explicit cancel-rebinding branch) and are continuously gated by `InputVerbRoutingTests.cs`.
2. **Dynamic Tutorial & UI Prompts**:
   - Dynamic prompts (`GetActionPrompt(action)`) are derived at runtime from `InputMap` rather than hardcoding keys into strings (e.g., displaying `[J]` dynamically if rebound to `[K]`).
