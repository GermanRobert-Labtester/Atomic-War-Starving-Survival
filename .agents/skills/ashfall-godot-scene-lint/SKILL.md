---
name: ashfall-godot-scene-lint
description: Structurally lints Godot .tscn/.tres — missing ext_resource, UID drift, orphan nodes, theme overrides, and signal connection typos. Use when touching scenes, themes, or UI panels; complements snapshot-diff pixel checks.
---

# ASHFALL Godot Scene Lint

## ROLE
`ashfall-snapshot-diff` catches pixel regressions; you catch structural breakage before it renders. With 207 UI files at fixed 1920×1080, a typoed `uid://` or dangling `ext_resource` silently breaks a panel only at runtime.

## RULES
1. Read-only lint — never rewrite `.tscn` UIDs without explicit approval (UID is content-addressed).
2. Scope is `*.tscn`, `*.tres`, `assets/**/*`, `src/**/*.cs` signal connections; not Unity `.unity`/`.prefab`.
3. `dotnet` + `godot --headless` only.

## WORKFLOW
### PHASE 1 — Parse Inventory
- Enumerate `**/*.tscn` with `[ext_resource path="res://..." id="..."]` and `[sub_resource]`. For each, check `uid://` exists in `assets/` and Godot cache.
- List `[connection signal="..." from="..." to="..." method="..."]` and `[Signal]` + `Connect` in C#.

### PHASE 2 — Structural Checks
- Missing `ext_resource` file / `uid` mismatch after move/rename.
- Orphan nodes (no parent), duplicate `unique_name_in_owner`, unnamed `Control` without layout.
- Signal typo: `signal` name vs `method` name arity mismatch (`Node` signature vs handler params).
- Theme override drift vs `assets/ui/theme.tres` — `theme_override_*` that duplicates theme without reason.
- Fixed viewport: `Control` anchors off 1920×1080 safe area, `Expand` without `SIZING`.

### PHASE 3 — Reference Graph
- Build `scene → resource → script` graph; flag resources referenced by no scene (dead asset) and code `GD.Load("res://...")` paths with no file.

### PHASE 4 — Verify
- `godot --headless --path . --check-only` style parse or `--quit-after 2` — 0 scene-load errors.
- `dotnet build Ashfall.csproj` 0 warnings (signal delegate generation).

## OUTPUT
`docs/scenes/SCENE_LINT_REPORT.md` — table: scene | resource | signal | error class | line | fix; orphan asset list.

## QUALITY GATE
- 0 missing `ext_resource`, 0 signal method typos, 0 UID drift, no orphan `unique_name` collisions.
