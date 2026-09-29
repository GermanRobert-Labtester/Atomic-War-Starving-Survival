# ASHFALL ChatGPT Item Art Tranche 29

STATUS: APPROVED BY USER

## Outcome

Add fifteen 512×512 inventory illustrations for existing catalog IDs: `item_official_ballot_box`, `item_pemmican`, `item_travel_ration`, `item_vibration_dampening_mount`, `item_iron_pyrite_ore`, `item_industrial_acid_carboy`, `item_neutralizer_lime_bag`, `item_grain_flour`, `item_oxygen_supply`, `item_titanium_breaching_shield`, `item_coated_turbine_blade`, `sandbags`, `item_worn_pet_collar`, `item_pharmacist_ledger`, `item_grandfathers_soldering_iron`.

## Evidence and ownership

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and have no direct or normalized-prefix art in the current `AssetRegistry.GetItem` search roots. `InventoryPanel` uses `AshfallUiHelpers.MakeItemIcon`, so exact `assets/art/{id}.jpg` filenames use the existing host presentation seam. Root owns only these fifteen new JPEGs and matching Godot import sidecars, this plan, and additive report, ownership, and state entries. No Core, host, catalog, UI, or existing art edits.

## Visual specification

One centered object or cohesive small kit per square asset, opaque near-black background, tactile grounded hand-painted realism, restrained cold gray, rust, brass, canvas, wood, food brown, and warm amber palette. Differentiate wax-paper pemmican from cloth-wrapped ration, acid carboy from oxygen unit, flour sack from lime bag, and ledger from ballot box at 26 px. No readable text, real insignia, people, copied art, or copied marks.

## Verification

Check fifteen opaque 512×512 JPEGs; inspect 64 px and 26 px contact sheets; validate `items.json` with `jq empty`; run `godot --headless --path . --import`; confirm fifteen `.jpg.import` sidecars; run scoped `git diff --check`. Art-only additions do not call for gameplay tests.
