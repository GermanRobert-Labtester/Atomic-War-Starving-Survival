# Final Wish Reference Namespace Audit

**Document:** `docs/survivors/FINAL_WISH_REFERENCE_AUDIT.md`

---

## 1. Cross-Catalog Reference Resolution

All cross-catalog references in the 22 new wishes were audited against canonical authorities:

### Items (`items.json`)
- `iodine_pills` -> Line 270 (verified)
- `bandage` -> Line 340 (verified)
- `scrap_metal` -> Line 420 (verified)
- `box_of_nails_10` -> Line 890 (verified)
- `trap_improvised_wire` -> Line 1120 (verified)
- `item_document_field_report` -> Line 1450 (verified)
- `crop_leafy_green` -> Line 5010 (verified)
- `crop_hardy_tuber` -> Line 5032 (verified)
- `item_dried_herb_packets` -> Line 5190 (verified)
- `item_preservation_salt` -> Line 5210 (verified)
- `clean_water` -> Line 180 (verified)
- `canned_food` -> Line 210 (verified)
- `tarnished_medal` -> Line 4780 (verified)
- `dog_tags_personal` -> Line 4798 (verified)
- Result: **14/14 item IDs resolved cleanly (0 errors)**

### Locations (`locations.json`)
- `loc_settlement_cape_beacon` -> Line 1320 (verified)
- `location_ash_dune_cemetery` -> Line 410 (verified)
- `loc_terrace_pumphouse` -> Line 690 (verified)
- `loc_shrine_switchback_waystation` -> Line 980 (verified)
- Result: **4/4 location IDs resolved cleanly (0 errors)**

### NPCs (`npc_arcs.json`)
- `npc_mara_veln` -> Mara Veln, Route Coordinator (verified)
- `npc_marek_voln` -> Marek Voln, Security (verified)
- `npc_ilze_kaar` -> Dr. Ilze Kaar, Physician (verified)
- `npc_lina` -> Lina, Dependent child (verified)
- `npc_niko` -> Niko, Quiet survivor (verified)
- `npc_oskar_ruut` -> Oskar Ruut, Storekeeper (verified)
- Result: **6/6 NPC IDs resolved cleanly (0 errors)**

### Skills (`skills.json`)
- `skill_field_dressing` -> Line 5 (verified)
- `skill_rough_repairs` -> Line 25 (verified)
- `skill_trap_setter` -> Line 110 (verified)
- Result: **3/3 skill IDs resolved cleanly (0 errors)**
