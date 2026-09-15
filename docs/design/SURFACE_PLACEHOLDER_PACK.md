# Wasteland Surface Placeholder Art Pack

**Status:** PLACEHOLDER — NOT FINAL ART · **Created:** 2026-09-12
**Generator:** `scripts/tools/generate-surface-placeholders.py`
**Manifest:** `assets/sprites/Surface/PLACEHOLDER_MANIFEST.json`
Companion: [Holdfast Shelter Placeholder Art Pack](SHELTER_PLACEHOLDER_PACK.md)

## Scope

First-week (days 1–7) **intact** wasteland surface. No impact sites, craters,
mushroom clouds, or breached structures — the world outside the shelter is
still standing.

## Contents

| File | Size | Wired into |
|---|---|---|
| `wasteland_sky_<phase>.png` | 1920×1080 | `MapPanel` backdrop (`SetLightingPhase`) |
| `surface_hatch_approach_<phase>.png` | 1280×720 | `MapDetailPanel` backdrop |
| `expedition_departure_<phase>.png` | 1280×720 | `ExpeditionPanel` backdrop (`SetLightingPhase`) |
| `PLACEHOLDER_MANIFEST.json` | — | labels every generated file |

Phases: `day1_7`, `dawn`, `dusk`, `night` (12 files). `MapPanel` and
`ExpeditionPanel` swap to the campaign-hour phase alongside the shelter view.

## Labelling

Every generated PNG contains **PLACEHOLDER** in its pixels (plus a boxed
"PLACEHOLDER ASSET" stamp and the scope line "DAY 1–7 … NO IMPACT SITES"),
and the manifest marks each file `"placeholder": true`. Existing assets are
not touched.

## Wiring helper

`src/UI/BackdropArt.cs` loads a cover-fit `TextureRect` + a dim overlay so
foreground UI stays readable. It is null-safe: if a texture is missing the
panel keeps a flat dim background, so headless runs without an import cache
stay clean.

## Regenerate

```bash
python3 scripts/tools/generate-surface-placeholders.py
bash scripts/ci/run-godot-bounded.sh --path . --import
```

Regenerate **all** packs (shelter + surface + characters) and import in one step:

```bash
./launch.sh --generate-placeholders
```

## Verification

- `bash scripts/ci/asset-orphan-sweep.sh` — 0 orphans
- `python3 scripts/ci/generate-asset-registry.py --check` — PASS
- `python3 scripts/ci/scene-lint.py` — 30 scenes, 0 errors
- `godot --headless -- . -- --expedition-panel-uitest` — PASS
- `godot --headless -- . -- --scene-binding-selftest` — 25/25 (MapDetailPanel)
- `godot --headless -- . -- --dashboard-uitest` — PASS
- `dotnet build Ashfall.csproj` — 0 warnings / 0 errors
