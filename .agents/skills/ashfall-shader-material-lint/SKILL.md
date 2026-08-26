---
name: ashfall-shader-material-lint
description: Lints Godot CanvasItemMaterial/ShaderMaterial, .tres materials, and .import settings (filter, mipmaps, compression) for drift from Unity→Godot port. Use when porting art, touching shaders, or before visual PRs.
---

# ASHFALL Shader & Material Lint

## ROLE
Every Unity `.mat`/`PhysicsMaterial2D` became a Godot `Material` (`.tres`) or `CanvasItemMaterial`/`ShaderMaterial` inside `.tscn`; every PNG regained a `.import` preset. You prevent filter/mipmap/compression drift and expensive fullscreen shader regressions.

Complements `ashfall-foundry` (creates) with a lint gate.

## RULES
1. `assets/` is authority after migration — never extend `Assets/art/` tree (`AGENTS.md:ASSET MIGRATION`).
2. Import settings must be ported into the Godot `.import` file, not left as Unity `.meta`.
3. Read-only lint; never mass-rewrite `.tres` without approval.

## WORKFLOW
### PHASE 1 — Inventory
- Enumerate `assets/**/*.tres`, `*.material`, `*.shader`, `*.gdshader`, and `**/*.import` plus `.tscn` with embedded `CanvasItemMaterial`.
- For each, record: filter (Nearest/Linear), mipmaps, `lossy/lossless` compress, `vramp`/`repeat`, shader `render_mode`.

### PHASE 2 — Drift Checks
- Texture import: sprite pixel-art must be `filter=false` + `mipmap=false`; large backgrounds `filter=true` + mipmaps as ported spec.
- Duplicate `.tres` differing only by defaults — consolidate to shared material.
- Shader cost: `texture_samples`, branches, loops, fullscreen `SCREEN_TEXTURE` usage, animated noise per-pixel cost (tie to `ashfall-tune` Pass 18).

### PHASE 3 — Reference Integrity
- `.tres` `ext_resource` paths valid, no missing `uid://` remaps.
- No Unity leftover `Assets/art/*.mat` still referenced by code.

### PHASE 4 — Verify
- `dotnet build Ashfall.csproj` 0 warnings
- `godot --headless --path . --quit-after 2` — no shader compile errors

## OUTPUT
`docs/art/SHADER_MATERIAL_LINT.md` — table: asset | type | import preset | drift | shader cost | dup cluster | fix.

## QUALITY GATE
- 0 import-preset drift for sprites/backgrounds, 0 missing `uid` refs, no duplicate shared-material candidates, no fullscreen shader >4 samples without justification.
