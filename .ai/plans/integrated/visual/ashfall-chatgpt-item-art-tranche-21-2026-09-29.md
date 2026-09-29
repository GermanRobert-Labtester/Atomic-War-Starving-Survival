# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# ASHFALL ChatGPT Item Art Tranche 21

STATUS: APPROVED BY USER

## Outcome

Add fifteen 512×512 inventory illustrations for the existing crop-waste and biofuel chain (`item_crop_waste`, `item_biofuel_low_grade`, `item_biofuel_generator_grade`, `item_biofuel_high_grade`, `item_separation_media_cartridge`), workshop machining parts (`item_machined_blank_small`, `item_machined_blank_medium`, `item_internal_spline_hub`, `item_keyed_actuator_collar`, `item_cutting_fluid_canister`), and run-flat wheel supplies (`item_runflat_insert_utility`, `item_runflat_insert_reinforced`, `item_rim_bead_kit`, `item_armored_beadlock_set`, `item_runflat_balancing_kit`).

## Evidence and ownership

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and lack direct or normalized-prefix art under the current `AssetRegistry.GetItem` search paths. `InventoryPanel` calls `AshfallUiHelpers.MakeItemIcon`, so exact `assets/art/{id}.jpg` filenames use the existing presentation seam. Root owns only these fifteen JPEGs, their Godot import sidecars, this plan, and additive visual report, ownership, and state entries. Existing catalogs, code, UI, and other art stay untouched.

## Visual specification

One distinct centered object or coherent kit per square asset; opaque near-black background; tactile grounded hand-painted realism; restrained cold gray, olive, rust, bone, and amber palette. The three biofuel grades and both insert grades must read as different at small sizes. No readable text, real insignia, people, or copied marks.

## Verification

Check fifteen JPEGs are 512×512, inspect contact sheets at 64 px and inventory's 26 px size, run `jq empty Assets/StreamingAssets/Data/items.json`, run `godot --headless --path . --import`, confirm `.jpg.import` sidecars, and run scoped `git diff --check`. No gameplay tests are needed for art-only additions.
