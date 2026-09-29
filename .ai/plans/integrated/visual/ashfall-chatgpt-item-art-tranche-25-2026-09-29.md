# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# ASHFALL ChatGPT Item Art Tranche 25

STATUS: APPROVED BY USER

## Outcome

Add fifteen 512×512 inventory illustrations for existing collectible IDs: `item_collectible_vinyl_chamber_record`, `item_collectible_vinyl_civil_broadcast`, `item_collectible_vinyl_folk_compilation`, `item_collectible_field_medicine_handbook`, `item_collectible_diesel_service_manual`, `item_collectible_radio_repair_guide`, `item_collectible_civil_defense_badge`, `item_collectible_transit_badge`, `item_collectible_trade_guild_patch`, `item_collectible_childs_doll`, `item_collectible_music_box`, `item_collectible_prayer_beads`, `item_collectible_team_pennant`, `item_collectible_civic_token`, and `item_collectible_folk_craft`.

## Evidence and ownership

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and lack direct or normalized-prefix art under the current `AssetRegistry.GetItem` search paths. `InventoryPanel` uses `AshfallUiHelpers.MakeItemIcon`, so exact `assets/art/{id}.jpg` filenames use the existing presentation seam. Root owns only these fifteen JPEGs and Godot import sidecars, this plan, and additive report, ownership, and state entries. Existing catalogs, code, UI, and other art stay untouched.

## Visual specification

One distinct centered object or cohesive compact pair per square asset; opaque near-black background; tactile grounded hand-painted realism; restrained cold gray, faded blue, rust, brass, paper, and bone palette. Vinyl jackets, manuals, badges, patches, and keepsakes should remain distinct at inventory size. All symbols and places are fictional abstractions. No readable text, real insignia, people, copied art, or copied marks.

## Verification

Check fifteen JPEGs are opaque 512×512, inspect 64 px and inventory's 26 px contact sheets, run `jq empty Assets/StreamingAssets/Data/items.json`, run `godot --headless --path . --import`, confirm `.jpg.import` sidecars, and run scoped `git diff --check`. No gameplay tests are needed for art-only additions.
