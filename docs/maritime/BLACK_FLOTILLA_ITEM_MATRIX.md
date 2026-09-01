# Black Flotilla Item Matrix (Plan 23 / Task 23A)

Authority: `Assets/StreamingAssets/Data/black_flotilla_items.json` (merged into the
global item catalog by `ItemCatalogLoader`; schema_version 1). Baseline 24 items →
**36 items** after Plan 23. All new ids are `item_`-prefixed snake_case; no duplicates
of `items.json`, `holdfast_items.json`, or any other catalog (gated by
`CatalogIntegrityValidator` Tier-1/UNIQUENESS + `Items_NoDuplicateIds_InFlotillaCatalog`).

## Existing 24 items — role classification (audit, unchanged)

| Role | Items |
|---|---|
| Junk/scavenge material | paper_scrap, cardboard_box, item_car_keys, crayon, acoustic_foam_panel, sawdust_block |
| Salvage tools | pipe_wrench, item_ice_pick, bone_saw, item_suitcase_locked, item_ice_pick |
| Industrial/chemical | industrial_bleach, ammonia_tank, halon_tank, brass_fittings, acoustic_foam_panel |
| Food/medical (incl. spoiled) | fat_rendered, blood_bag, spoiled_blood_bag, spoiled_canned_food, spoiled_meat |
| Comfort/symbolic | item_teddy_bear, cigarette_pack_sealed |
| Weapons/defense | ammo_9x19, item_ash_ghillie |
| Quest/identity | item_anchor_notes, item_suitcase_locked, item_anchor_notes |
| Code objects | item_anchor_notes (Anchor notes — Flotilla lore) |

## 12 new Plan 23 items — matrix

| id | Role / fleet | Type | Trade value | Acquisition path | Live consumer(s) |
|---|---|---|---|---|---|
| `item_descent_line` | Dive gear (Deep Fleet) | Tool | 22 | Flotilla trade; wreck salvage | Deep-site gear gate (Plan 23B `required_item_id`); `buys_at_premium`; Flotilla radio copy |
| `item_sealed_dive_lamp` | Dive gear (Deep Fleet) | Device | 34 | Flotilla trade; claim rewards | Deep/cave-site gear gate; `buys_at_premium`; Lotte Verrill want |
| `item_salvage_cutting_tool` | Salvage tool (Salvage Fleet) | Tool | 16 | Quartermaster trade; wreck scavenge | Refinery/structure gear gate; trade |
| `item_rebreather_canister` | Dive gear (Deep Fleet) | Filter | 22 | Trade; deep-site loot | Deep-site gear gate; `buys_at_premium`; radio (air shorthand) |
| `item_escort_challenge_ribbon` | Code/identity (Escort Fleet) | Quest | 0 | Escort standing/quest reward | Radio challenge-response context; escort encounter semantics (lore/conditions) |
| `item_deep_service_ribbon` | Code/identity (Deep Fleet) | Quest | 0 | Dive-Chief standing quest | Radio deep-report code; NPC relationship (Jorin Hael) |
| `item_claim_tag_stamped` | Code/identity + trade | Trade | 12 | Salvage quartermaster | Buys-at-premium at Flotilla; claim-dispute content; wreck-rights quests |
| `item_sea_ration` | Trade/survival | Food (`hungerRestore: 14`) | 4 | Flotilla trade; wreck scavenge | Needs system (hunger), trade stock |
| `item_brine_protein_tin` | Trade/survival | Food (`hungerRestore: 14`) | 5 | Flotilla exchange | Hunger consumer; inland trade loop |
| `item_marine_sealant_kit` | Trade/survival | Material | 15 | Cape Beacon / Flotilla trade | Trade premium + repair flavor; Flotilla `wants` |
| `item_ships_bell_picket` | Relic (war grave) | Relic (`moraleEffect: 2`) | 18 | Picket-craft wreck (Plan 23B loot) | Memorial/war-grave narrative; morale |
| `item_fleet_log_cylinder` | Quest/relic | Quest | 0 | Deep-site loot; Lotte Verrill thread | Uma Tarran (codekeeper) want; deep-site variable loot; radio coded reports |

## Consumer reachability rule

Every Plan 23 item has at least one **live** consumer surface:
item catalog (trade/pricing), `FactionTradePreference` buys-at-premium, dive-site gear
gates (23B), loot tables (23B), settlement/trade stock, or NPC wants/offers. No item is
decorative-only; `tradeValue: 0` items are quest/code objects with narrative consumers
(codekeeper wants, radio meaning, lore).

## Duplicate-risk notes

- No Plan 10 gear duplication: Plan 10 authored no marine gear items; `item_rebreather_scrubber`
  (items.json, Plan 09) already covers scrubber packs — Plan 23 adds the *canister* as its
  Flotilla-packed counterpart, distinct id, no overwrite.
- Ribbons are Quest-typed (zero trade value) so they cannot become a currency.
- Existing 24 items untouched; ids remain stable save/content contracts.
