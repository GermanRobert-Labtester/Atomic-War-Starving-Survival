# Placeholder-Art Replacement: Shelter Room Pictograms, Remaining Props, Tiles

STATUS: APPROVED BY USER
Date: 2026-09-26
Claim: `claim-placeholder-art-shelter-rooms-2026-09-26`
User directive: "Placeholder-art replacement lane — shelter room pictograms/tiles
are still procedural placeholders (the stage backdrops were replaced by the
Blender bake on 09-25); continue that bake pipeline for the remaining sprites."

## Bounded outcome

Replace the remaining 28 Plan 139 Pillow placeholders in
`assets/sprites/Shelter/` with deterministic final 2D art via the proven
Blender-bake pipeline (09-25 stage bake, seed 20260925 conventions):

- 23 room pictograms `room_*.png` (128×128 RGBA) — runtime-consumed by
  `RoomHotspotView.UpdateIcon` at 0.5 scale above each room badge.
- 2 props `prop_ceiling_lamp.png`, `prop_pipe_bundle.png` (128×128 RGBA) —
  manifest-tracked, not yet wired to runtime anchors (kept honest).
- 3 tiles `tile_{wall_concrete,floor_concrete,floor_grate}.png` (128×128 RGB) —
  manifest-tracked only, no runtime consumer today; generated seamless by
  construction (periodic value noise / periodic geometry) so any future
  tiling use is safe.

Same filenames, same dimensions, zero runtime code change.

## Evidence (verified 2026-09-26)

- `PLACEHOLDER_MANIFEST.json`: exactly these 28 entries have
  `placeholder: true`; the 7 stage finals are intact
  (`post-shelter-bake.py --check` 7/7 OK).
- `shelter_rooms.json` (hotspot driver) contains exactly the 23 room IDs of
  the pictogram files — no orphan art, no missing icon. The extra
  `room_{foundry,greenhouse,main,water_pump}` and `room_fixture_*` /
  `room_history_*` IDs live only in `shelter_room_identities.json`
  (identity/fixture/vignette data) and never render hotspots.
- `tile_*` and `prop_ceiling_lamp|pipe_bundle` have no `src/` consumers —
  baked for manifest truth and future wiring, not claimed as runtime-visible.

## Files

- New: `scripts/tools/bake-shelter-rooms.py` (Blender headless; 25 vignettes:
  23 rooms + 2 props; same helpers/palette/neutral rig/256-raw pattern as
  `bake-shelter-stage.py`; Cycles seed 20260925).
- New: `scripts/tools/post-shelter-rooms-bake.py` (Pillow: crop/fit rooms and
  props to 128 RGBA like the prop flow; deterministic seamless tile
  generation, seed 20260926; `--check` for all 28 finals).
- Replaced: the 28 sprite files above.
- Updated: `assets/sprites/Shelter/PLACEHOLDER_MANIFEST.json` (28 entries →
  `placeholder: false` with `replaced_by` provenance).
- New: `docs/visual/SHELTER_ROOM_PICTOGRAM_BAKE_2026-09-26.md` (provenance +
  verification record).
- Raw renders under `artifacts/shelter-bake-rooms/` (pruned before commit;
  QA contact sheets kept out of the repo).

## Verification

1. `post-shelter-rooms-bake.py --check` — dimensions/mode/transparency for
   all 28, seamlessness deltas for tiles.
2. Vision QA on contact sheets (montage) via image analysis: each pictogram
   structurally readable at 64×64, palette-consistent, no clipped silhouettes.
3. Godot 4.7.1 headless: `--import` clean, `--quit-after 2` boot,
   `--player-panels-uitest` Errors: 0 (exercises ShelterPanel hotspots).

## Non-goals

- No runtime code change (`RoomHotspotView`/`HoldfastInteriorView` read-only).
- No wiring of the 2 unwired props or 3 tiles into scene anchors (separate
  decision; requires touching PFGL-claimed host seams).
- No work on `assets/art/placeholders-512/`, catalog-referenced items/portraits,
  or the foreign `loc_*.jpg` lane.
- `generate-shelter-placeholders.py` untouched; manifest note keeps the
  overwrite warning current.

## Approval

User selected this lane explicitly in the 2026-09-26 session (option 2 of the
proposed task list) with full knowledge of the pipeline and claim requirement.
