# Bone, Horn & Antler Item Identity Crosswalk

Only exact canonical IDs may participate in item lookup. The current source labels do not resolve to exact inventory definitions, so Plan 160 adds no item inspection or crafting mutation bridge.

| Authored value | Meaning in source | Crosswalk result | Runtime rule |
|---|---|---|---|
| st_hacksaw_blade_01 | historical saw label | unresolved item ID | display in the record only |
| st_hacksaw_blade_02 | historical saw label | unresolved item ID | display in the record only |
| bb_rabbit_tibia_01 | historical bone blank label | unresolved item ID | never creates or finds inventory |
| bb_deer_antler_rod_01 | historical bone blank label | unresolved item ID | never creates or finds inventory |
| bb_dog_femur_01 | historical bone blank label | unresolved item ID | never targets a companion |
| bb_cat_femur_01 | historical bone blank label | unresolved item ID | display-only |
| bb_goat_horn_tip_01 | historical horn blank label | unresolved item ID | display-only |
| bb_bird_bone_rod_01 | historical bone blank label | unresolved item ID | display-only |
| bb_dog_lye_treated_01 | historical bone blank label | unresolved item ID | display-only |
| sandstone_block | abrasive/process label | no exact canonical item match | no abrasive is granted or consumed |
| folded_abrasive_cloth | abrasive/process label | no exact canonical item match | no material is granted or consumed |
| flint_scraper_then_sandstone | compound process label | not an item ID | no parser or recipe inference |
| needle / awl / hook / pin | generic finished-tool classes | no exact matching item link in this corpus | no fishing, sewing or repair modifier |

Nearby canonical items were deliberately not aliased: surgical_saw is a medical saw, item_surgical_bone_chisel is a surgical instrument, phonograph_needle is an audio part, surgical_suture is a medical consumable, and wood_block/leather_strap are different material definitions. Similar words do not establish identity.

Consequently, Plan 55 crafting remains the only recipe authority, inventory remains the only item-existence authority, and item condition/combat/fishing/trade values remain unchanged. A future exact-ID crosswalk may enrich inspection panels after a separate content decision; it must not infer identity from labels.
