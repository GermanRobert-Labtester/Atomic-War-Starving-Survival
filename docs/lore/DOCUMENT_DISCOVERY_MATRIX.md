# Plan 17 — Document Discovery Matrix

Maps discoverable documents to their physical discovery sources, ensuring every document has a plausible placement in the world.

## Discovery Source Types

| Source Type | Description | Example |
|-------------|-------------|---------|
| Location loot | Found in containers at specific locations | Maintenance log in industrial site |
| Expedition reward | Obtained through expedition encounters | Field report from deep-lore site |
| Quest completion | Reward for completing a questline | Merchant ledger from trade quest |
| Faction reward | Given by faction after reaching threshold | Faction circular from allied group |
| Archive cache | Pre-placed in archive desk queue | Sealed government document |
| Trade goods | Purchased from traders/caravans | Child's workbook from traveling merchant |
| Survivor drop | Carried by specific survivor NPCs | Unsent soldier letter from deserter |

## Placement Principles

1. **Thematic coherence** — maintenance logs belong in industrial sites, death registers in clinics
2. **Geographic plausibility** — faction circulars in controlled territory, not random wasteland
3. **Temporal consistency** — pre-war documents in pre-war locations, post-exchange in aftermath sites
4. **Accessibility balance** — important documents not locked behind impossible RNG or late-game zones only
5. **Provenance alignment** — document author/source matches discovery location

## Document Placement Categories

### Administrative / Bureaucratic

| Document Type | Plausible Locations | Example |
|---------------|-------------------|---------|
| Ration ledger | Administrative centers, holdfasts | `loc_municipal_archive`, `loc_administration_building` |
| Transport manifest | Transit hubs, depots | `loc_transit_authority_hq`, `loc_freight_depot` |
| Death register | Clinics, administrative sites | `loc_field_hospital`, `loc_morgue` |
| Work order | Industrial sites, maintenance | `loc_thermal_plant`, `loc_water_treatment` |
| Attendance record | Schools, barracks, work sites | `loc_schoolhouse`, `loc_garrison_barracks` |

### Personal / Intimate

| Document Type | Plausible Locations | Example |
|---------------|-------------------|---------|
| Unsent letter | Personal effects, military sites | `loc_soldier_bunker`, `loc_refugee_camp` |
| Diary fragment | Residences, personal spaces | `loc_apartment_block`, `loc_personal_quarters` |
| Child's workbook | Schools, shelters with families | `loc_schoolhouse`, `loc_bunker_nursery` |
| Family photograph | Personal effects, residences | Any residential location |
| Personal keepsake | Survivor inventories, memorial sites | Carried by NPCs, memorial plaques |

### Technical / Industrial

| Document Type | Plausible Locations | Example |
|---------------|-------------------|---------|
| Maintenance log | Industrial infrastructure | `loc_thermal_plant`, `loc_pump_station` |
| Equipment fault report | Engineering sites, workshops | `loc_engineering_bay`, `loc_workshop` |
| Technical manual | Libraries, research sites | `loc_municipal_library`, `loc_research_lab` |
| Blueprint/schematic | Industrial sites, military | `loc_factory`, `loc_military_bunker` |

### Medical / Scientific

| Document Type | Plausible Locations | Example |
|---------------|-------------------|---------|
| Medical note | Clinics, hospitals, aid stations | `loc_field_hospital`, `loc_clinic` |
| Research log | Labs, research facilities | `loc_research_lab`, `loc_biolab` |
| Patient record | Medical facilities | `loc_hospital_ward`, `loc_quarantine_zone` |
| Dosage log | Medical stores, pharmacies | `loc_pharmacy`, `loc_medical_storage` |

### Faction / Political

| Document Type | Plausible Locations | Example |
|---------------|-------------------|---------|
| Faction circular | Faction-controlled territory | `loc_faction_stronghold`, `loc_faction_outpost` |
| Propaganda leaflet | Faction areas, public spaces | Any faction-controlled location |
| Treaty draft | Administrative centers, meeting sites | `loc_council_chamber`, `loc_diplomatic_center` |
| Intelligence report | Military sites, faction HQs | `loc_intelligence_center`, `loc_spy_safehouse` |

### Historical / Archival

| Document Type | Plausible Locations | Example |
|---------------|-------------------|---------|
| Pre-war record | Archives, government buildings | `loc_municipal_archive`, `loc_government_office` |
| Evacuation manifest | Transit hubs, emergency centers | `loc_evacuation_point`, `loc_emergency_shelter` |
| Historical account | Libraries, museums, archives | `loc_museum`, `loc_historical_society` |
| Sealed government document | Secure archives, military | `loc_classified_archive`, `loc_military_vault` |

## 15 New Documents (Plan 17G Target)

| # | Document ID | Type | Discovery Location | Provenance | Voice |
|---|-------------|------|-------------------|------------|-------|
| 1 | `doc_merchant_ledger_04` | Merchant ledger | `loc_trade_post` | Exchange trader | Bureaucratic, numbers-focused |
| 2 | `doc_unsent_soldier_letter_07` | Unsent letter | `loc_soldier_bunker` | Conscripted soldier | Personal, fragmented, emotional |
| 3 | `doc_maintenance_log_thermal` | Maintenance log | `loc_thermal_plant` | Maintenance technician | Technical, terse, practical |
| 4 | `doc_death_register_field_hospital` | Death register | `loc_field_hospital` | Medical orderly | Institutional, clinical, exhausted |
| 5 | `doc_child_workbook_exercises` | Child's workbook | `loc_schoolhouse` | Bunker child | Simple, innocent, sometimes unsettling |
| 6 | `doc_ration_ledger_holdfast` | Ration ledger | `loc_holdfast_admin` | Quartermaster | Bureaucratic, precise, defensive |
| 7 | `doc_field_report_expedition` | Field report | Deep-lore site | Expedition leader | Observational, cautious, military |
| 8 | `doc_medical_note_radiation` | Medical note | `loc_clinic` | Physician | Clinical, concerned, understaffed |
| 9 | `doc_transport_manifest_convoy` | Transport manifest | `loc_freight_depot` | Logistics officer | Institutional, numbered, routing-focused |
| 10 | `doc_faction_circular_archivists` | Faction circular | Archivist contact | The Archivists | Formal, cryptic, purposeful |
| 11 | `doc_personal_diary_fragment` | Diary fragment | `loc_apartment_block` | Bunker resident | Intimate, reflective, incomplete |
| 12 | `doc_machine_terminal_printout` | Terminal printout | `loc_research_lab` | Automated system | Machine-readable, timestamped, cold |
| 13 | `doc_equipment_fault_report` | Fault report | `loc_engineering_bay` | Engineer | Technical, frustrated, safety-conscious |
| 14 | `doc_evacuation_roster` | Evacuation roster | `loc_evacuation_point` | Civil defense | Official, stamped, incomplete |
| 15 | `doc_bunker_graffiti_transcription` | Graffiti record | Bunker common area | Bunker resident | Informal, defiant, anonymous |

## Reachability Validation

Every document must satisfy:

1. **Has a valid discovery location** — resolves to a real `loc_*` ID
2. **Location is visitable** — player can physically reach it
3. **Not locked behind impossible RNG** — reasonable chance of acquisition
4. **Not all in late-game zones** — spread across early/mid/late game
5. **Provenance matches location** — makes sense that this document is here

## Integration with Archive System

```
Discover document (loot/quest/reward)
→ Document added to evidence list
→ Player can view but not read fully
→ Queue at Archive Desk
→ Consume ink + time (8-hour work day)
→ Transcription complete
→ Journal entry created
→ Codex entry unlocked
→ Knowledge key discovered
```

## Verification

| Check | Status |
|-------|--------|
| All documents have discovery sources | ❌ NOT DONE |
| All source locations are valid loc_* IDs | Pending |
| All locations are visitable | Pending |
| Thematic placement is coherent | Pending |
| Important lore not locked behind impossible RNG | Pending |
| Documents spread across game progression | Pending |
