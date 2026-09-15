# Holdfast Shelter Placeholder Art Pack

**Status:** PLACEHOLDER — NOT FINAL ART · **Created:** 2026-09-12
**Generator:** `scripts/tools/generate-shelter-placeholders.py`
**Manifest:** `assets/sprites/Shelter/PLACEHOLDER_MANIFEST.json`

## Scope

First-week (days 1–7) **intact** Holdfast shelter only. This pack never
depicts a bombed, breached, flooded, or irradiated interior — the early
campaign shelter is sealed and operational.

## Contents

| Set | Count | Size | Notes |
|---|---|---|---|
| `shelter_interior_<phase>.png` | 4 | 760×420 | Lighting variants: `day1_7`, `dawn`, `dusk`, `night`; swapped by `HoldfastInteriorView.SetLightingPhase` |
| `room_<id>.png` | 23 | 128×128 | One per authored room; shown by `RoomHotspotView` |
| `prop_<name>.png` | 5 | 128×128 | `supply_crate`, `water_barrel`, `ceiling_lamp`, `pipe_bundle`, `hatch_door` |
| `tile_*.png` | 3 | 128×128 | `tile_wall_concrete`, `tile_floor_concrete`, `tile_floor_grate` — reusable stage tiles |
| `PLACEHOLDER_MANIFEST.json` | 1 | — | Labels every generated file `"placeholder": true` |

The interior lighting phase is driven from the campaign hour in
`Main.ExpandedShelterSystems.TickAllExpandedShelterSystems`
(`LightingPhaseForHour`), so the variants are live, not static.

## Labelling contract

Every generated PNG contains the word **PLACEHOLDER** in its pixels, and the
manifest marks each file `"placeholder": true` with
`"status": "PLACEHOLDER — NOT FINAL ART"`. Existing assets are **not** touched
or relabelled. A final art pass replaces this whole directory and removes the
manifest.

## Wiring (assets are consumed, not orphaned)

- `src/World/HoldfastInteriorView.cs` — loads the cutaway background and the
  fixed props via `TryLoadShelterTexture` (null-safe).
- `src/World/RoomHotspotView.cs` — loads `room_<id>.png` on `RoomId` set.
- `src/UI/ShelterPanel.cs` — hosts the interior view in a 760×420 SubViewport.

Missing art is non-fatal: the view falls back to the flat dark background so
headless runs without an import cache stay clean and println-free.

## Regenerate

```bash
python3 scripts/tools/generate-shelter-placeholders.py
bash scripts/ci/run-godot-bounded.sh --path . --import
```

Regenerate **all** packs (shelter + surface + characters) and import in one step:

```bash
./launch.sh --generate-placeholders
```

Room entries are read from `Assets/StreamingAssets/Data/shelter_rooms.json`,
so the pack stays in sync with the data authority.

## Verification

- `bash scripts/ci/asset-orphan-sweep.sh` — 0 orphans
- `python3 scripts/ci/generate-asset-registry.py --check` — PASS, 0 missing
- `python3 scripts/ci/scene-lint.py` — 30 scenes, 0 errors
- `godot --headless --path . -- --player-panels-uitest` — shelter=True, PASS
- `godot --headless --path . -- --shelter-operations-selftest` / `--shelter-decor-selftest` — PASS
- `dotnet build Ashfall.csproj` — 0 warnings / 0 errors
