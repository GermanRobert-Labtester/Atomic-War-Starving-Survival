# Shelter Room Pictogram + Prop + Tile Bake — 2026-09-26

Provenance and verification record for the final 28 sprites of the
`assets/sprites/Shelter/` collection, continuing the 2026-09-25 stage bake
(`SHELTER_STAGE_2D_BAKE_2026-09-25.md`) to the placeholders it left behind.
No runtime code changed; filenames, dimensions, and load paths are identical.

## Verified gap (pre-work evidence)

- `PLACEHOLDER_MANIFEST.json` listed exactly 28 `placeholder: true` files:
  23 `room_*.png` pictograms, `prop_ceiling_lamp.png`, `prop_pipe_bundle.png`,
  and 3 `tile_*.png`.
- Runtime truth for the pictograms: `RoomHotspotView.UpdateIcon` loads
  `ShelterArtDir + roomId + ".png"` and draws it at 0.5 scale above each room
  badge. `shelter_rooms.json` (the catalog that drives hotspots) contains
  exactly those 23 room ids — no orphan art, no missing icon. The additional
  `room_{foundry,greenhouse,main,water_pump}` / `room_fixture_*` /
  `room_history_*` ids exist only in `shelter_room_identities.json`
  (identity/fixture/vignette data) and never render hotspots.
- The 2 props and 3 tiles have **no runtime consumer today** (`grep` across
  `src/` and Core); they are baked for manifest truth and future wiring, and
  that wiring is explicitly out of scope.

## Assets produced (28 files, same paths as the placeholders they replace)

| Family | Files | Size | Deterministic source |
|---|---|---|---|
| Room pictograms | `room_<id>.png` (23) | 128×128 RGBA | `bake-shelter-rooms.py` (Blender 5.2.2 headless, Cycles seed 20260925) → `post-shelter-rooms-bake.py` crop/fit |
| Props | `prop_ceiling_lamp.png`, `prop_pipe_bundle.png` | 128×128 RGBA | same pipeline; wall/ceiling-mounted, no contact-shadow floor |
| Tiles | `tile_wall_concrete.png`, `tile_floor_concrete.png`, `tile_floor_grate.png` | 128×128 RGB | `post-shelter-rooms-bake.py`, seed 20260926 |

## Method

- Each room is a purpose-built miniature vignette (3–10 primitives: bunks,
  kitchen counter + stockpot, generator on skid + exhaust, filter bank,
  anvil-and-forge, isolation tent, beacon mast, …) authored in the stage
  palette (charcoal `#24272E` family, steel `#454B54`, rust `#6E3F28`,
  cloth olive, clinical pale, `#C7DCD0` lamp glow, restrained amber
  emissions). No real-world symbols or text.
- Same neutral day-authoritative two-light rig and front orthographic
  camera as the three shipped props; transparent film; contact shadows
  baked on the shared floor patch for grounded vignettes. All 25 vignettes
  live in one scene; each render isolates its entry via `hide_render`
  (objects named `<room_id>|<part>`), 256×256 raws downscaled/cropped to
  the final 128×128 by the post script — identical flow to the shipped
  props (`116px` fit box, centered).
- Two Blender-5.2 gotchas recorded for re-runs: this build's broken OCIO
  fallback means raws are display-referred (no extra LUT applied — same
  stance as the stage bake), and the orthographic camera MUST carry the
  stage rotation `(90°, 0, 0)`; moving only its location silently renders
  empty frames.
- Tiles are seamless by construction: concrete uses wrapped value noise
  (period = full 128px) with wrapped stain blobs; the grate is periodic
  geometry with in-cell-offset jitter only, so every 32px cell is
  byte-identical (provable, and proven, by `--check`).

## Iteration record (vision QA)

- Pass 1 (montage of all 23): verdict USABLE, 18/23 clean; six composition
  fixes applied — mess hall gained an overhead pot rack (was a low smudge),
  ward clinical gained a tall screen + IV stand, radio tuner's chassis
  enlarged to hero size with a big dial, storage secure's cage closed with
  a mid rail + tighter bars, workshop heavy re-centered on the anvil,
  quarantine tent re-clothed dark (palette outlier); plus thickness bumps
  for thin elements (clinic IV pole, beacon mast/crossbars).
- Pass 2 (the 8 revised sprites): 8/8 PASS, overall USABLE.
- In-context composite (pictograms at 64px on the day backdrop above badge
  mockups): grounding and scale confirmed.

## Verification performed

- `python3 scripts/tools/post-shelter-rooms-bake.py --check`: **28/28 OK**
  (sizes, RGBA + real transparency, 754–2268 colors each; tile seam deltas
  dv/dh < 0.5 for concrete, exact 32px periodicity for the grate).
- Prior-session finals intact before work: `post-shelter-bake.py --check`
  7/7 OK.
- Godot 4.7.1 headless: `--import` clean; `--quit-after 2` interactive boot
  clean ("Exiting cleanly"); `--player-panels-uitest` **Errors: 0**, all
  panel lifecycle gates PASS.
- `PLACEHOLDER_MANIFEST.json` now reports 0 placeholders in the collection;
  regeneration recipes and the overwrite warning live in its notes.

## Remaining gaps (not addressed here)

- The 1,079-file `assets/art/placeholders-512/` pack and ~614 unresolved
  catalog references (items/portraits/locations) are untouched; the foreign
  `loc_*.jpg` wave in `assets/art/` belongs to another stream.
- Wiring `prop_ceiling_lamp` / `prop_pipe_bundle` / tiles into runtime
  anchors is a separate decision (host seams owned by active claims).
