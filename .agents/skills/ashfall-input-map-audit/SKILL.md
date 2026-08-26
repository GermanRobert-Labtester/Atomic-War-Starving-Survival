---
name: ashfall-input-map-audit
description: Audits Godot InputMap, project.godot [input] actions, key/joypad bindings, rebinding paths, and fixed-viewport input handling for conflicts and accessibility gaps. Use when adding input, panels, or controller support.
---

# ASHFALL Input Map Audit

## ROLE
ASHSFALL ships at fixed 1920×1080 (`project.godot:gl_compatibility`, 60 FPS) with keyboard + controller. You ensure every player action has one canonical `InputMap` action, no hardcoded `Key` checks, no duplicate binds, and remapping survives save/load.

## RULES
1. Canonical source is `project.godot` `[input]` / `[input_devices]` — never hardcode `Key.W` in gameplay code; use `Input.IsActionPressed("move_up")`.
2. Read-only audit — never rewrite `project.godot` without explicit user approval.
3. `dotnet` + `godot --headless` only.

## WORKFLOW
### PHASE 1 — Inventory
- Parse `project.godot` `[input]` actions, events, deadzones. Enumerate all `Input.IsAction*`, `InputEvent*`, `_UnhandledInput`, `_Input` usages in `src/` (207-file UI tree).
- Catalog panels that consume input (`src/UI/**/*.cs`, `.tscn` with `InputEvent`).

### PHASE 2 — Conflict & Gap Check
- Duplicate scancode / joy button across actions.
- Missing actions for panel-required inputs (e.g., inventory close, map zoom).
- Hardcoded `OS.GetKeycodeString` or `InputEventKey.Keycode` checks bypassing `InputMap`.
- Unbound actions (defined but never queried) and unmapped queries (queried action not in `project.godot`).
- Viewport scaling: input coordinates vs 1920×1080 fixed viewport; `get_viewport_rect` mismatches.

### PHASE 3 — Accessibility
- All actions reachable without chorded combos where possible; check `ashfall-ui-access` overlap (keyboard nav, focus traversal).
- Joypad deadzone consistency, no required mouse-only actions.

### PHASE 4 — Verify
- `godot --headless --path . --check-only` (if available) or `godot --headless --path . --quit-after 2` boots without InputMap warnings.
- Manual headless action probe: `InputMap.HasAction("xxx")` sweep.

## OUTPUT
`docs/input/INPUT_MAP_AUDIT.md` — table: action | events | queried in | duplicate/conflict | missing/reachability | hardcoded bypass | fix recommendation.

## QUALITY GATE
- 0 queried-but-undefined actions, 0 duplicate scancodes across non-modifier actions, 0 hardcoded `Key.*` in `src/` gameplay paths, all panel close/back actions bound.
