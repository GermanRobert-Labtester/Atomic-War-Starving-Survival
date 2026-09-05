# Plan 58 — Encounter Coverage & Dedup Matrix

All 32 pre-existing entries audited (3 base + 29 expansion) before authoring.
Categories below are as-authored (free-form labels — see runtime contract).

## Base catalog (3)

| ID | Category | Core situation | Location | Choices | Stealth | Speed |
|---|---|---|---|---|---|---|
| `enc_dead_letter_office` | Discovery | mummified driver, deliverable letter vs supplies | — (forced) | 4 | 0.5 | 1.5 |
| `enc_weather_station` | Discovery | still weather station, records vs leave | — (forced) | 4 | 0.5 | 1.5 |
| `enc_pianist` | Social | pianist playing in a ruin | — | 4 | 0.5 | 1.5 |

## Expansion catalog (29) — situation summary

| ID | Category | Situation |
|---|---|---|
| `enc_overturned_postal` | Discovery | postal van variant of the dead-letter scene |
| `enc_weather_station` | Discovery | duplicate id of base weather station (unloaded catalog) |
| `enc_pianist` | Observation | duplicate id of base pianist |
| `enc_roadside_trader` / `enc_water_seller` | Trade | merchant trade scenes |
| `enc_false_broadcast` / `enc_two_camps` | Misinformation | contradictory-information scenes |
| `enc_sick_child` | Rescue | medical need, child |
| `enc_injured_scavenger` | Rescue | **pinned scavenger** — injured person needing extraction |
| `enc_roof_access` / `enc_cold_storage` | Structural | traversal/storage decisions |
| `enc_hot_sign` | Radiation | radiation warning discovery |
| `enc_clean_well` | Radiation | found-safe water |
| `enc_whiteout_traveler` / `enc_flood_road` | Weather | weather-blocked travel |
| `enc_dogs_silent` / `enc_following_footsteps` | Fear | dread/atmosphere scenes |
| `enc_border_camp` / `enc_two_factions_trade` | Faction | faction presence/trade |
| `enc_locked_room` | Mystery | **mysterious locked room** |
| `enc_two_graves` | Mystery | graves with disputed identity |
| `enc_beggars` / `enc_thief_child` | Ethical | begging/theft moral scenes |
| `enc_library_cache` | Discovery | knowledge cache |
| `enc_greenhouse_keeper` / `enc_dead_radio_operator` / `enc_ice_fishermen` | Observation | people-at-work observation scenes |
| `enc_quarantine_sign` | Mystery | quarantine warning |
| `enc_last_train` | Discovery | transit discovery |

## Plan-58 briefs: kept (20) vs replaced (2) + 1 cliché-avoided

| Brief | Verdict | Reason |
|---|---|---|
| D1 Relay Booth | **kept** → `enc_relay_booth_frequency` | distinct from `enc_dead_radio_operator` (information vs corpse scene); bound to `loc_radio_relay_mast` |
| D2 Sealed Civil-Defense Door | **kept** → `enc_sealed_civil_defense_door` | distinct from `enc_locked_room` (shelter-door with air movement; structural decision); bound to `loc_excavation_civilian_shelter` |
| D3 Vehicle Cache Under Tarpaulin | **kept** → `enc_tarpaulin_vehicle_cache` | no expansion overlap |
| C1 Scrap-Line Ambush | **kept** → `enc_scrap_line_ambush` | no expansion ambush scene |
| C2 Feral Dogs at the Culvert | **REPLACED** | near-duplicate of `enc_dogs_silent` (Fear) → `enc_territory_warning` (tools-arranged-as-warning standoff) |
| C3 Checkpoint With No Flag | **kept** → `enc_checkpoint_no_flag` | deniable-identity checkpoint; distinct from `enc_border_camp` |
| S1 The Wrong Directions | **kept** → `enc_wrong_directions` | distinct from `enc_whiteout_traveler` (disorientation, not weather) |
| S2 Trader With a Broken Scale | **kept** → `enc_broken_scale_trader` | trust/measurement decision, not a plain trade |
| S3 Separated Young Survivor | **kept** → `enc_separated_young_survivor` | practical waiting/overdue situation — not the sick-child medical scene; tone-disciplined per §58C.10 |
| M1 Family Cache With Names | **kept** → `enc_family_cache_names` | distinct from `enc_library_cache` (survival cache vs knowledge) |
| M2 Too Injured to Move | **REPLACED** | direct duplicate of `enc_injured_scavenger` → `enc_hoarders_promise` (dead hoarder's promised shares — moral, no injured-person overlap) |
| M3 Road Tax | **kept** → `enc_road_tax_toll` | written-tariff authority claim; distinct from `enc_border_camp` |
| E1 Floor With a New Crack | **kept** → `enc_settled_floor_crack` | no expansion structural-crack scene |
| E2 Bitter Water | **kept** → `enc_bitter_water` | inverse polarity of `enc_clean_well` (suspicious vs safe); bound to `loc_water_station` |
| MED1 Field Clinic | **kept** → `enc_field_clinic_after_evacuation` | no expansion clinic scene; bound to `abandoned_hospital` |
| MED2 Expired Stock | **kept** → `enc_expired_stock` | unique (dated-medicine decision) |
| SC1 Locked Storeroom | **REPLACED** | §0 explicitly bans duplicating "a mysterious locked room" (`enc_locked_room`) → `enc_looted_food_drop` (contested remains of a food drop) |
| SC2 Fuel, No Keys | **kept** → `enc_fuel_no_keys` | unique |
| R1 Under the Slab | **REPLACED** | near-duplicate of `enc_injured_scavenger` ("Pinned Scavenger") → `enc_footbridge_stranded` (group stranded at a broken crossing); bound to `loc_bridge_seven` |
| R2 Ice-Water Rescue | **kept** → `enc_ice_water_rescue` | distinct from `enc_ice_fishermen` (emergency vs observation) |
| H1 Unexploded Ordnance | **kept** → `enc_unexploded_ordnance` | unique |
| H2 Hot Salvage | **kept** → `enc_hot_salvage` | risk/reward retrieval, distinct from `enc_hot_sign` (warning discovery) |

**Mechanical-duplicate check:** expansion moral scenes reduce to
`help = -resource +morale / ignore = +guilt`; the new slate varies decision
shapes — risk-vs-information (relay, crack, hot rack), pay-vs-refuse
(tariff, checkpoint), restraint-vs-gain (named cache, hoarder), speed-vs-care
(tarpaulins, food drop), and no-mechanical-change options in 14 of 22
encounters (§58G.2 met: 14 ≥ 4–6).
