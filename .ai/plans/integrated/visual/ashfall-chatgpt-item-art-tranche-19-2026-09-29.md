# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# ASHFALL ChatGPT Item Art Tranche 19

STATUS: APPROVED BY USER

## Outcome

Add ten 512×512 inventory illustrations for five existing seed/crop pairs:
`item_seed_hardy_tuber` / `crop_hardy_tuber`, `item_seed_ash_grain` /
`crop_ash_grain`, `item_seed_biolum_mushroom` / `crop_biolum_mushroom`,
`item_seed_nutrient_algae` / `crop_nutrient_algae`, and
`item_seed_medicinal_herb` / `crop_medicinal_herb`.

## Evidence and ownership

All ten IDs are authored in `Assets/StreamingAssets/Data/items.json` and had
no direct or normalized-prefix art in the current `AssetRegistry` item search
paths. Inventory uses `AssetRegistry.GetItem` through
`AshfallUiHelpers.MakeItemIcon`, so exact `assets/art/{id}.jpg` filenames use
the existing presentation seam. Root claims only these ten JPEGs, Godot
import sidecars, this plan, and additive visual report, ownership, and state
records. Existing catalogs, art, code, and UI stay untouched.

## Visual specification

One distinct centered object or coherent small grouping per square asset;
opaque near-black background; tactile grounded hand-painted realism matching
the existing frost-pea seed/crop pair; restrained cold gray, muted green,
rust, bone, and blue-green glow palette. Seed items must be visually distinct
from harvested crops. No readable text, real insignia, people, or copied
marks. Art remains legible at 64 px and the inventory's 26 px size.

## Verification

Check ten JPEGs are 512×512, inspect 64/26 px contact sheets, run `godot
--headless --path . --import`, confirm `.jpg.import` sidecars, and run scoped
`git diff --check`. No gameplay test is needed for art-only additions.
