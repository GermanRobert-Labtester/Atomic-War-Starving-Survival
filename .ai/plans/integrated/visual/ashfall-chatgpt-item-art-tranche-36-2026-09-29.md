# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# ASHFALL ChatGPT Item Art Tranche 36

STATUS: APPROVED BY USER

## Outcome

Add fifteen 512×512 inventory illustrations for existing catalog IDs: `item_collectible_family_portrait`, `item_collectible_unit_photograph`, `item_collectible_civil_defense_poster`, `item_collectible_propaganda_poster`, `item_collectible_concert_poster`, `item_collectible_pre_war_novel`, `item_collectible_science_magazine`, `item_collectible_water_treatment_handbook`, `item_collectible_air_filter_manual`, `item_collectible_dosimeter_guide`, `item_collectible_unit_log_fragment`, `item_collectible_deployment_order`, `item_collectible_casualty_list`, `item_collectible_mothers_letter`, `item_collectible_soldiers_letter`.

## Evidence and ownership

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and have no direct or normalized-prefix art in the current `AssetRegistry.GetItem` search roots. `InventoryPanel` uses `AshfallUiHelpers.MakeItemIcon`, so exact `assets/art/{id}.jpg` filenames use the existing host presentation seam. Root owns fifteen new JPEGs and matching Godot import sidecars, fifteen editable SVG sources under `docs/visual/sources/tranche36/`, this plan, and additive report, ownership, and state entries. No Core, host, catalog, UI, or existing art edits.

## Visual specification

Use local vector illustration for archival objects: two photographs, three posters, five books or guides, three official papers, and two letters. One centered object or cohesive small kit per square asset, opaque charcoal background, textured paper, worn edges, restrained fictional post-disaster colors, clear silhouette at 26 px. No readable text, real insignia, recognizable people, copied art, or copied marks. Art is non-authoritative presentation only.

## Verification

Check fifteen opaque 512×512 JPEGs; inspect 64 px and 26 px contact sheets; validate `items.json` with `jq empty`; run `godot --headless --path . --import`; confirm fifteen `.jpg.import` sidecars; check fifteen SVG sources and scoped `git diff --check`. Art-only additions do not call for gameplay tests.
