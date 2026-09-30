# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# ASHFALL ChatGPT Item Art Tranche 37

STATUS: APPROVED BY USER

## Outcome

Add fifteen 512×512 inventory illustrations for existing catalog IDs: `item_collectible_rejection_letter`, `item_collectible_military_patch`, `item_collectible_prayer_book`, `item_collectible_match_program`, `item_collectible_exchange_day_newspaper`, `item_collectible_local_newspaper`, `item_collectible_road_map`, `item_collectible_topo_map`, `item_collectible_survivor_map`, `item_document_evacuation_list`, `item_document_ration_record`, `item_document_blood_trail_note`, `item_document_barricade_placement`, `item_document_sealed_door_warning`, `item_document_family_photograph`.

## Evidence and ownership

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and have no direct or normalized-prefix art in the current `AssetRegistry.GetItem` search roots. `InventoryPanel` uses `AshfallUiHelpers.MakeItemIcon`, so exact `assets/art/{id}.jpg` filenames use the existing host presentation seam. Root owns fifteen new JPEGs and matching Godot import sidecars, fifteen editable SVG sources under `docs/visual/sources/tranche37/`, this plan, and additive report, ownership, and state entries. No Core, host, catalog, UI, or existing art edits.

## Visual specification

Use local vector illustration for a patch, book and match program, two newspapers, three maps, five records or notices, and a family photograph. One centered object or cohesive small kit per square asset, opaque charcoal background, textured paper and fabric, worn edges, restrained fictional colors, clear silhouette at 26 px. No readable text, real insignia, recognizable people, copied art, or copied marks. Art is non-authoritative presentation only.

## Verification

Check fifteen opaque 512×512 JPEGs; inspect 64 px and 26 px contact sheets; validate `items.json` with `jq empty`; run `godot --headless --path . --import`; confirm fifteen `.jpg.import` sidecars; check fifteen SVG sources and scoped `git diff --check`. Art-only additions do not call for gameplay tests.
