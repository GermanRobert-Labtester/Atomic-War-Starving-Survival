# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# ASHFALL ChatGPT Item Art Tranche 27

STATUS: APPROVED BY USER

## Outcome

Add fifteen 512×512 inventory illustrations for existing shelter daily-life IDs: `item_heavy_wool_coat`, `item_thermal_parka`, `item_insulated_boots`, `item_improvised_burn_barrel`, `item_portable_kerosene_heater`, `item_acoustic_guitar`, `item_harmonica`, `item_playing_cards`, `item_carved_figurine`, `item_wasteland_sketch`, `slate_and_chalk`, `school_primer`, `item_flatbread`, `item_vegetable_soup`, and `item_dried_herb_packets`.

## Evidence and ownership

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and lack direct or normalized-prefix art under the current `AssetRegistry.GetItem` search paths. `InventoryPanel` uses `AshfallUiHelpers.MakeItemIcon`, so exact `assets/art/{id}.jpg` filenames use the existing presentation seam. Root owns only these fifteen JPEGs and Godot import sidecars, this plan, and additive report, ownership, and state entries. Existing catalogs, code, UI, and other art stay untouched.

## Visual specification

One distinct centered object or cohesive small kit per square asset; opaque near-black background; tactile grounded hand-painted realism; restrained cold gray, olive, rust, wool brown, paper, and warm amber palette. Differentiate the two outer garments and the two heating devices at inventory size. No readable text, real insignia, people, copied art, or copied marks.

## Verification

Check fifteen JPEGs are opaque 512×512, inspect 64 px and inventory's 26 px contact sheets, run `jq empty Assets/StreamingAssets/Data/items.json`, run `godot --headless --path . --import`, confirm `.jpg.import` sidecars, and run scoped `git diff --check`. No gameplay tests are needed for art-only additions.
