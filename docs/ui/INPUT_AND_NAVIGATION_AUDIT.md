# ASHFALL — Input Map, Navigation & Interaction Audit

**Audit Reference:** Plan 14 Task 14E / `ashfall-input-map-audit`
**Authority:** `src/Host/AshfallInputActions.cs`, `project.godot`, `Main.Application.cs`.

---

## 1. Input Map Action Registry

ASHFALL enforces canonical `InputMap` action names defined centrally in `AshfallInputActions.cs`. Every action maps to primary keyboard, secondary keyboard, and Joypad gamepad inputs:

| Action Identifier | Primary Key | Secondary Key | Gamepad (Joypad) | Scope / Context |
|---|---|---|---|---|
| `ashfall_close` / `ui_cancel` | `Escape` | — | `JoyButton.B` | Global: Close panel / cancel dialog / back |
| `ashfall_confirm` / `ui_accept` | `Enter` | `Space` | `JoyButton.A` | Global: Confirm action / activate button |
| `ashfall_next_tab` | `Tab` | — | `JoyButton.RightShoulder` | Panels: Next tab in multi-tab panels |
| `ashfall_nav_up` | `Up Arrow` | — | `JoyButton.DpadUp` | Global: Directional menu & focus navigation |
| `ashfall_nav_down` | `Down Arrow` | — | `JoyButton.DpadDown` | Global: Directional menu & focus navigation |
| `ashfall_nav_left` | `Left Arrow` | — | `JoyButton.DpadLeft` | Global: Directional menu & focus navigation |
| `ashfall_nav_right` | `Right Arrow` | — | `JoyButton.DpadRight` | Global: Directional menu & focus navigation |
| `ashfall_journal` | `J` | — | `JoyButton.Y` | PlayingOnly: Open Journal / Codex |
| `ashfall_help` | `F1` | — | `JoyButton.Back` | PlayingOnly: Open Tutorial / Help |
| `ashfall_guidance` | `F2` | — | — | PlayingOnly: Toggle Guidance Overlay |
| `ashfall_forecast` | `F` | — | — | PlayingOnly: Open Weather Forecast |
| `ashfall_weather_history` | `H` | — | — | PlayingOnly: Open Weather History |
| `ashfall_events` | `E` | — | — | PlayingOnly: Open Events Log |
| `ashfall_expeditions` | `X` | — | — | PlayingOnly: Open Expeditions |
| `ashfall_holdfast` | `T` | — | — | PlayingOnly: Open Holdfast Terminal |
| `ashfall_journal_tab_1..5` | `1`–`5` | — | — | JournalBook: Direct tab selection |
| `ashfall_holdfast_build` | `B` | — | — | HoldfastTerminal: Build tab |
| `ashfall_holdfast_status` | `S` | — | — | HoldfastTerminal: Status tab |

---

## 2. Collision & Reachability Analysis

1. **0 Hardcoded Keycode Collisions:**
   - No direct `Key.W`, `Key.S`, `Key.Escape` comparisons exist in production UI paths without falling back to canonical `AshfallInputActions`.
2. **Context-Sensitive Keys:**
   - Number keys `1`–`5` switch Journal tabs only when the Journal is open.
   - `B` (Buy) and `S` (Sell) in Holdfast terminal are bound only within the terminal modal.
3. **Dynamic Prompt Display:**
   - Tutorial copy, button text, and tooltip help query `AshfallInputActions.GetActionPrompt()` so rebound keys or gamepad mode automatically update UI prompts (e.g., "[J]" vs "[Y]" on gamepad).
4. **Modal Stack Escape Unification:**
   - Esc / Back uniformly dismisses the topmost modal/overlay without pausing or resetting underlying session state.
