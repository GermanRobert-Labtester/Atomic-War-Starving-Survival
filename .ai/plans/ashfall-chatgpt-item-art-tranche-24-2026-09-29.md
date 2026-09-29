# ASHFALL ChatGPT Item Art Tranche 24

STATUS: APPROVED BY USER

## Outcome

Add fifteen 512×512 inventory illustrations for ten existing trap IDs (`trap_improvised_wire`, `trap_box`, `trap_fish`, `trap_body_grip`, `trap_snare`, `trap_deadfall`, `trap_pit`, `trap_net`, `trap_cage`, `trap_bird_snare`) and five related field or food-preparation items (`item_decor_trophy_ash_hound_pelt`, `item_fur_mittens`, `item_boiled_roots`, `item_collectible_hunting_magazine`, `item_vacuum_seal_canner`).

## Evidence and ownership

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and lack direct or normalized-prefix art under the current `AssetRegistry.GetItem` search paths. `InventoryPanel` uses `AshfallUiHelpers.MakeItemIcon`, so exact `assets/art/{id}.jpg` filenames use the existing presentation seam. Root owns only these fifteen JPEGs and Godot import sidecars, this plan, and additive report, ownership, and state entries. Existing catalogs, code, UI, and other art stay untouched.

## Visual specification

One distinct centered trap, object, or small cutaway per square asset; opaque near-black background; tactile grounded hand-painted realism; restrained cold gray, olive, rust, straw, and bone palette. Differentiate the two wire snares, box and cage traps, and net and fish traps at inventory size. Show traps inert and unoccupied; no gore, readable text, people, real insignia, or copied marks.

## Verification

Check fifteen JPEGs are opaque 512×512, inspect 64 px and inventory's 26 px contact sheets, run `jq empty Assets/StreamingAssets/Data/items.json`, run `godot --headless --path . --import`, confirm `.jpg.import` sidecars, and run scoped `git diff --check`. No gameplay tests are needed for art-only additions.
