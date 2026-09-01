# Plan 51 — Environmental Storytelling Document Pack Closeout

## Completion Status

**COMPLETE — fully placed and consumed.**

30 document content units exist. 30 item identities exist. 16 are placed in existing scavenging tables. All item→narrative cross-references resolve. All lore_flags are unique.

## Narrative Schema

Documents use the `documents_batch_3.json` schema:
```json
{
  "schema_version": 1,
  "batch_id": "documents_batch_3",
  "documents": [{
    "doc_id": "string",
    "title": "string",
    "origin": "string",
    "material": "string",
    "physical_description": "string",
    "transcript": "string",
    "lore_flags": ["flag_lore_*_found"],
    "item_id_source": "item_document_*"
  }]
}
```

## Item Schema

Document items use the "Document" type in items.json:
```json
{
  "id": "item_document_*",
  "displayName": "string",
  "description": "string",
  "type": "Document",
  "stackMax": 1,
  "weight": 0.01-0.05,
  "tradeValue": 1-8,
  "moraleEffect": -1 to 2 (optional),
  "empShielded": false
}
```

## 30 Documents

| # | Doc ID | Item ID | Type | Location | Placement |
|---:|---|---|---|---|---|
| 1 | doc_evacuation_roster_001 | item_document_evacuation_list | evacuation list | government | police_station |
| 2 | doc_ration_ledger_001 | item_document_ration_record | ration record | shelter | — |
| 3 | doc_blood_trail_note_001 | item_document_blood_trail_note | blood-trail note | hospital | hospital |
| 4 | doc_barricade_sketch_001 | item_document_barricade_placement | barricade placement | civil defense | police_station |
| 5 | doc_sealed_door_001 | item_document_sealed_door_warning | sealed-door warning | archive | — |
| 6 | doc_family_photo_001 | item_document_family_photograph | family photograph | apartment | apartment_block |
| 7 | doc_military_map_001 | item_document_military_map | military map | depot | military_depot |
| 8 | doc_broadcast_script_001 | item_document_broadcast_transcript | broadcast transcript | radio station | — |
| 9 | doc_propaganda_vandalized_001 | item_document_vandalized_propaganda | vandalized propaganda | checkpoint | — |
| 10 | doc_handwritten_warning_001 | item_document_handwritten_warning | handwritten warning | water point | — |
| 11 | doc_maintenance_log_001 | item_document_maintenance_record | maintenance record | utility room | — |
| 12 | doc_rejection_list_001 | item_document_shelter_rejection_list | shelter rejection list | shelter | — |
| 13 | doc_ration_theft_001 | item_document_ration_theft_ledger | ration theft ledger | storeroom | warehouse |
| 14 | doc_water_notice_001 | item_document_water_notice | contaminated water notice | pump station | — |
| 15 | doc_repair_note_001 | item_document_repair_note | half-finished repair note | workshop | — |
| 16 | doc_casualty_list_001 | item_document_casualty_list | casualty list | hospital | hospital |
| 17 | doc_triage_record_001 | item_document_triage_record | triage record | field clinic | hospital |
| 18 | doc_last_letter_001 | item_document_last_letter | last letter | apartment | apartment_block |
| 19 | doc_supply_requisition_001 | item_document_supply_requisition | supply requisition | garrison | military_depot |
| 20 | doc_evacuation_route_map_001 | item_document_evacuation_route_map | evacuation route map | transit hub | — |
| 21 | doc_quarantine_notice_001 | item_document_quarantine_notice | quarantine notice | shelter | — |
| 22 | doc_civil_defense_poster_001 | item_document_civil_defense_poster | civil defense poster | school | school |
| 23 | doc_field_report_001 | item_document_field_report | field report | outpost | military_depot |
| 24 | doc_journal_fragment_001 | item_document_journal_fragment | journal fragment | apartment | apartment_block |
| 25 | doc_death_certificate_001 | item_document_death_certificate | death certificate | records office | — |
| 26 | doc_supply_inventory_001 | item_document_supply_inventory | supply inventory | warehouse | warehouse |
| 27 | doc_radio_log_001 | item_document_radio_log | radio log | relay station | — |
| 28 | doc_child_drawing_001 | item_document_child_drawing | child drawing | apartment | apartment_block |
| 29 | doc_confession_001 | item_document_confession | confession | shelter | — |
| 30 | doc_will_001 | item_document_will | will | shelter | apartment_block |

## Scavenging Placements

16 documents placed across 6 tables:

| Table | Documents | Count |
|---|---|---|
| table_loot_hospital | triage_record, blood_trail_note, casualty_list | 3 |
| table_loot_school | civil_defense_poster | 1 |
| table_loot_military_depot | military_map, field_report, supply_requisition | 3 |
| table_loot_apartment_block | family_photograph, last_letter, journal_fragment, child_drawing, will | 5 |
| table_loot_police_station | barricade_placement, evacuation_list | 2 |
| table_loot_warehouse | supply_inventory, ration_theft_ledger | 2 |

## Cross-Document Echoes

- `Kovalenko` surname appears in evacuation_roster and will
- `Morozov` surname appears in evacuation_roster, barricade_sketch, and death_certificate
- `Dr. Vel` appears in blood_trail_note, water_notice, and radiology_slide (documents_batch_2)
- `Requisition 4471` appears in supply_requisition and supply_inventory
- `Sector 7` appears in evacuation_roster and casualty_list
- `Bridge Seven` appears in barricade_sketch and evacuation_route_map

## Files Created/Modified

```
Assets/StreamingAssets/Data/items.json (MODIFIED — 30 items added)
Assets/StreamingAssets/Data/narrative/documents_batch_3.json (NEW — 30 documents)
Assets/StreamingAssets/Data/scavenging_tables.json (MODIFIED — 16 placements)
docs/narrative/PLAN51_CLOSEOUT.md (NEW)
```

## Verification

| Check | Result |
|---|---|
| JSON parse (items) | valid |
| JSON parse (narrative) | valid |
| JSON parse (scavenging) | valid |
| Document count | 30/30 |
| Item count | 30/30 |
| Item→narrative refs | all 30 resolve |
| Scavenging placements | 16/15 target |
| Unique lore_flags | all 30 unique |
| Build | 0 errors |
