# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# ASHFALL ChatGPT Item Art Tranche 30

STATUS: APPROVED BY USER

## Outcome

Add fifteen 512×512 inventory illustrations for existing catalog IDs: `item_sludge_cake`, `item_tailings_drum`, `item_decor_trophy_beetle_carapace`, `item_decor_trophy_molerat_skull`, `item_decor_trophy_crow_feathers`, `item_decor_trophy_pheasant_plume`, `item_decor_trophy_gulden_wolf`, `item_decor_trophy_kestrel_wings`, `item_archive_index_cylinder`, `concrete_rubble`, `empty_toner_cartridge`, `mineral_chunk`, `organic_residue`, `item_foundry_weather_canister`, `item_brined_legume_mash`.

## Evidence and ownership

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and have no direct or normalized-prefix art in the current `AssetRegistry.GetItem` search roots. `InventoryPanel` uses `AshfallUiHelpers.MakeItemIcon`, so exact `assets/art/{id}.jpg` filenames use the existing host presentation seam. Root owns only these fifteen new JPEGs and matching Godot import sidecars, this plan, and additive report, ownership, and state entries. No Core, host, catalog, UI, or existing art edits.

## Visual specification

One centered object or cohesive small kit per square asset, opaque near-black background, tactile grounded hand-painted realism, restrained charcoal, oxidized metal, silt green, bone, feather black, copper, and rust palette. The six fictional trophies are non-graphic and must differ by silhouette; follow catalog descriptions for `item_decor_trophy_gulden_wolf` (dust lynx) and `item_decor_trophy_kestrel_wings` (iron crow). Distinguish sludge cake, tailings drum, rubble sack, mineral, and residue jar at 26 px. No readable text, real insignia, people, copied art, or copied marks.

## Verification

Check fifteen opaque 512×512 JPEGs; inspect 64 px and 26 px contact sheets; validate `items.json` with `jq empty`; run `godot --headless --path . --import`; confirm fifteen `.jpg.import` sidecars; run scoped `git diff --check`. Art-only additions do not call for gameplay tests.
