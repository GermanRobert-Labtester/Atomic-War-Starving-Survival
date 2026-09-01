# Plan 11 Continuity Matrix & Cross-Plan Integration

> **Document Class:** Cross-System Continuity Specification
> **Integrates:** Plan 04 (Relics), Plan 06 (Narrative & Radio), Plan 09 (Disease & Pathology), Plan 10 (Warlords & Combat), Plan 14 (UI & Accessibility)

---

## 1. Cross-Plan Dependency Map

| Content / System ID | Source Domain | Dependent System | Integration Seam | Degradation Fallback |
|---|---|---|---|---|
| `item_comm_codebook_alpha` | Plan 11 (Task 11A/B) | Plan 04 (Reverse Eng.) | Unlocks radio decryption schematic | Acts as high-value trade relic |
| `item_relic_military_core` | Plan 11 (Task 11A) | Plan 04 (Tech Research) | Feeds military electronics reverse-eng. | Acts as salvage component |
| `cipher_station_relay_count`| Plan 11 (Task 11B) | Plan 06 (Radio Broadcasts) | Expands shortwave audio rundowns | Plays as standard number station |
| `hazard_spore_mold` | Plan 11 (Task 11A/C) | Plan 09 (Disease Pathology) | Triggers spore inhalation affliction | Applies standard rad/toxin debuff |
| `event_evolution_warlord_expansion` | Plan 11 (Task 11C) | Plan 10 (Warlord Doctrines) | Syncs with `warlords_sector_4` territory | Applies local travel danger penalty |
| `MapLocationMarkerView` | Plan 11 (Task 11F) | Plan 14 (UI / Accessibility)| Accessible icon + high-contrast text | Standard fallback icons |

---

## 2. Narrative Flag Lifecycle Matrix

| Flag ID | Producer | Consumer | Lifecycle Role |
|---|---|---|---|
| `flag_sig_relay_heard` | Radio Broadcast 104.5 MHz | Questline / Journal | Marks broadcast intercepted |
| `flag_sig_relay_key_found` | Collapsed Command Vault | Questline / Inventory | Marks codebook retrieved |
| `flag_sig_relay_decoded` | Cipher Hunt Engine | Questline / Radio | Satisfies decryption logic |
| `flag_sig_relay_location_revealed` | `WastelandMapSystem` | Map Atlas Panel | Unlocks `loc_hidden_relay_bunker` |
| `flag_sig_relay_resolved` | Expedition Arrival | Campaign Journal | Terminal resolution of chain 1 |
| `flag_sig_winter_heard` | Radio Broadcast 94.2 MHz | Questline / Journal | Marks broadcast intercepted |
| `flag_sig_winter_key_found` | Metro Interchange / Cache | Questline / Inventory | Marks ledger sheet retrieved |
| `flag_sig_winter_decoded` | Cipher Hunt Engine | Questline / Radio | Satisfies decryption logic |
| `flag_sig_winter_location_revealed`| `WastelandMapSystem` | Map Atlas Panel | Unlocks `loc_logistics_reserve_cache`|
| `flag_sig_winter_resolved` | Expedition Arrival | Campaign Journal | Terminal resolution of chain 2 |
| `flag_sig_rotation_heard` | Radio Broadcast 107.8 MHz | Questline / Journal | Marks broadcast intercepted |
| `flag_sig_rotation_key_found`| Pre-War Archive Bunker | Questline / Inventory | Marks index cylinder retrieved |
| `flag_sig_rotation_decoded` | Cipher Hunt Engine | Questline / Radio | Satisfies decryption logic |
| `flag_sig_rotation_location_revealed`| `WastelandMapSystem`| Map Atlas Panel | Unlocks `loc_deaddrop_command_shelter`|
| `flag_sig_rotation_resolved`| Expedition Arrival | Campaign Journal | Terminal resolution of chain 3 |

---

## 3. Data Integrity & Validation Pass
All IDs strictly follow snake_case naming conventions, match registered prefixes (`item_`, `loc_`, `quest_`, `event_`, `flag_`, `cipher_station_`, `radio_`), and pass `CatalogIntegrityValidator`.
