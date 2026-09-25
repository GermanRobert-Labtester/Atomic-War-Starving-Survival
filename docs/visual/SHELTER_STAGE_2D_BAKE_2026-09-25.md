# Shelter Stage 2D Bake — 2026-09-25

Provenance and verification record for the baked shelter-interior stage art.
Scope: replace the Plan 139 Pillow line-art placeholders that the live game
actually displays on the holdfast interior stage with Blender-baked 2D art.
No runtime code changed; filenames, dimensions, and load paths are identical.

## Verified gap (pre-work evidence)

- `src/World/HoldfastInteriorView.cs` builds the stage from
  `res://assets/sprites/Shelter/` (Plan 139 placeholder pack, manifest status
  `PLACEHOLDER — NOT FINAL ART`, every tile stamped "PLACEHOLDER" in pixels).
- `ShelterPanel` hosts this view as its visual anchor; `Main.GameFlow.cs`
  instantiates it directly — the pack is live runtime content, not dead art.
- `PLACEHOLDER_MANIFEST.json` explicitly sets `replace_with_final_art: true`.

## Assets produced (7 files, same paths as the placeholders they replace)

| File | Size | Consumer |
|---|---|---|
| `shelter_interior_day1_7.png` | 760x420 RGB | `BackdropArt.ShelterInterior` (default) |
| `shelter_interior_dawn.png` | 760x420 RGB | `BackdropArt.ShelterInteriorFor("dawn")` |
| `shelter_interior_dusk.png` | 760x420 RGB | `BackdropArt.ShelterInteriorFor("dusk")` |
| `shelter_interior_night.png` | 760x420 RGB | `BackdropArt.ShelterInteriorFor("night")` |
| `prop_supply_crate.png` | 128x128 RGBA | `BuildBackground` anchor (64, 344) scale 0.55 |
| `prop_water_barrel.png` | 128x128 RGBA | `BuildBackground` anchor (694, 346) scale 0.55 |
| `prop_hatch_door.png` | 128x128 RGBA | `BuildBackground` anchor (714, 226) scale 0.50 |

## 3D-to-2D conversion method

- Authored in Blender 5.2 (headless, `--background`) as a "theater flat": a
  front-facing orthographic camera at 2x render scale with wall / floor /
  ceiling panels mapped 1:1 onto the 760x420 pixel grid, so every feature
  lands at an exact pixel row (floor line at py 330 = `FloorStandY` band).
- Consistent single lighting/camera/palette system across all outputs; the
  three props are baked from the same scene with the same light rig (day
  authoritative) and transparent film, contact shadows baked in.
- Four phase rigs differ only in world env / lamp / key parameters; the
  runtime `CanvasModulate` phase tint continues to apply on top.
- Deterministic: fixed geometry, fixed materials, Cycles seed 20260925.
- Palette follows DESIGN.md: charcoal surfaces (wall #24272E family), floor
  #30353F, steel #454B54, rust #6E3F28/#8A4A2B, paper #9C9686, lamp glow
  #C7DCD0 (the washed CRT green-white). No real-world symbols or text.
- Composition keeps py 140–265 calm for the dynamic room-hotspot badges and
  leaves the runtime prop anchors (x 64 / 694 / 714) clean of baked clutter.

## Regeneration

```bash
blender --background --python scripts/tools/bake-shelter-stage.py -- \
    --only backdrops,props --phases all
python3 scripts/tools/post-shelter-bake.py        # grades + writes finals
python3 scripts/tools/post-shelter-bake.py --check
```

Note: this Blender build logs an OpenColorIO config error and falls back to
"color management disabled"; the raws are therefore graded in
`post-shelter-bake.py` (gentle per-phase levels + 14% desat, no gamma LUT —
empirically the raws land display-referred). Also note
`generate-shelter-placeholders.py` would overwrite these finals if re-run.

## Repairs included in the same pass

- `assets/sprites/Map/marker_safe.png` was base64 text (truncated stream,
  unrecoverable) — regenerated as a clean 32x32 RGBA marker in palette.
- Two `assets/ui/Screens/*.png` files contained JPEG data under .png names —
  re-encoded to real PNG format (content unchanged).

## Verification performed

- `magick identify` / Pillow checks: exact dimensions, RGBA props with real
  transparency, color counts (no flat placeholder output).
- Vision QA on contact sheets (backdrops + props) and on a runtime-mockup
  composite (backdrop + props at code anchors + hotspot badges + actor
  stand-ins): structure readable, phases differentiated, calm band preserved,
  props grounded; verdicts USABLE/PASS.
- Pixel targets: day wall sRGB ~(54,57,63), floor pools ~(75,87), ceiling
  ~(40,43,46) — DESIGN.md charcoal family; night variant intentionally
  lamp-led (runtime modulate darkens it further).
- Godot 4.7.1 headless: `--import` clean (marker_safe rescanned, no errors);
  `--quit-after 2` boot 36/36 catalogs; `--player-panels-uitest` Errors: 0
  (ShelterPanel path exercised, no art-load warnings).

## Remaining gaps (not addressed here)

- Room pictograms (`room_*.png`, 24 files) and remaining props/tiles
  (`prop_ceiling_lamp`, `prop_pipe_bundle`, `tile_*`) are still Plan 139
  placeholders; same bake pipeline can extend to them.
- The 1,079-file `assets/art/placeholders-512/` pack and ~614 unresolved
  catalog references (items/portraits/locations) are untouched; an untracked
  `loc_*.jpg` wave in `assets/art/` suggests another stream owns that lane.
