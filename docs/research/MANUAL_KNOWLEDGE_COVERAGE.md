# ASHFALL Manual Knowledge Coverage & Acquisition Matrix
**Document ID:** AF-DOC-MANUAL-KNOWLEDGE-COVERAGE
**Authority:** Plans 60–63 Flagship Integration
**Scope:** 24 Canonical Field Manuals across 6 Disciplines
**Status:** Complete & Authoritative

---

## 1. Summary

The ASHFALL Library Study archive provides survivors with educational paths across 6 primary survival disciplines: **Survival**, **Engineering**, **Medical**, **Science**, **Scavenging**, and **Combat**. Each discipline contains 4 canonical manuals (total 24), providing progressive depth from un-gated entry-level manuals to advanced, specialized protocols.

---

## 2. Complete Discipline Matrix

### 2.1 Survival Discipline
| Manual ID | Display Name | Hours | Prereqs | Knowledge Reveals | Acquisition Paths |
|---|---|---|---|---|---|
| `manual_water_filtration` | Field Water Filtration | 10h | None | `knowledge_water_basics` | `table_loot_farm`, `loc_the_allotments`, `caravan_flotilla_salt_run` |
| `manual_bunker_hydroponics` | Subterranean Hydroponics & Soil Nutrients | 12h | `manual_water_filtration` | `knowledge_hydroponics` | `table_loot_greenhouse`, `loc_the_allotments` |
| `manual_vacuum_preservation` | Pressure Canning & Food Preservation | 10h | None | `knowledge_food_preservation` | `table_loot_residential`, `table_loot_general_store` |
| `manual_apiculture_and_pollination` | Enclosed Apiculture & Colony Thermoregulation | 14h | `manual_bunker_hydroponics` | `knowledge_apiculture_ecology` | `table_loot_greenhouse`, `loc_the_allotments` |

### 2.2 Engineering Discipline
| Manual ID | Display Name | Hours | Prereqs | Knowledge Reveals | Acquisition Paths |
|---|---|---|---|---|---|
| `manual_solar_maintenance` | Photovoltaic Maintenance & Inverter Rewiring | 14h | None | `knowledge_solar_basics` | `table_loot_power_substation`, `loc_denial_cut_substation` |
| `manual_relic_reverse_engineering` | Pre-War Solid-State Electronics Repair | 16h | `manual_solar_maintenance` | `knowledge_signal_amplifier_blueprint` | `table_loot_telecom`, `table_loot_military_bunker` |
| `manual_radiation_shielding_fabrication` | Radiation Shielding & Attenuation Matrix | 16h | `manual_solar_maintenance` | `knowledge_radiation_shielding` | `table_loot_industrial`, `loc_denial_cut_substation` |
| `manual_gas_mask_canister_rebuild` | Gas Mask Filter Repacking & Seal Testing | 12h | None | `knowledge_gas_mask_improved` | `table_loot_military_depot`, `table_loot_industrial` |

### 2.3 Medical Discipline
| Manual ID | Display Name | Hours | Prereqs | Knowledge Reveals | Acquisition Paths |
|---|---|---|---|---|---|
| `manual_rad_first_aid` | Radiation First Aid & Dose Mitigation | 12h | None | `knowledge_radiation_basics` | `table_loot_clinic`, `table_loot_hospital`, `loc_the_allotments` |
| `manual_field_trauma_surgery` | Emergency Trauma & Field Surgery Protocols | 18h | `manual_rad_first_aid` | `knowledge_field_trauma_surgery` | `table_loot_hospital`, `loc_the_allotments` |
| `manual_quarantine_epidemiology` | Pathogen Containment & Quarantine Protocols | 16h | `manual_rad_first_aid` | `knowledge_pathogen_containment` | `table_loot_clinic`, `table_loot_hospital` |
| `manual_pharmacology_synthesis` | Post-Collapse Pharmacology & Chemical Syntheses | 20h | `manual_quarantine_epidemiology` | `knowledge_pharmacology_synthesis` | `table_loot_pharmacy`, `table_loot_hospital` |

### 2.4 Science Discipline
| Manual ID | Display Name | Hours | Prereqs | Knowledge Reveals | Acquisition Paths |
|---|---|---|---|---|---|
| `manual_radio_signal_direction` | Radio Direction Finding & Morse Signal Analysis | 12h | None | `knowledge_radio_basics` | `table_loot_telecom`, `loc_denial_cut_substation` |
| `manual_cloud_seeding_meteorology` | Tropospheric Condensation & Silver Iodide Seeding | 18h | `manual_radio_signal_direction` | `knowledge_atmospheric_cloud_seeding` | `table_loot_weather_station`, `loc_denial_cut_substation` |
| `manual_ionospheric_propagation` | High-Frequency Ionospheric Skip & Grayline Propagation | 15h | `manual_radio_signal_direction` | `knowledge_ionospheric_propagation` | `table_loot_telecom`, `table_loot_military_bunker` |
| `manual_subterranean_geophone` | Seismic Geophone Arrays & Fault Drift Detection | 16h | `manual_radio_signal_direction` | `knowledge_seismic_geophone_blueprint` | `table_loot_industrial`, `loc_denial_cut_substation` |

### 2.5 Scavenging Discipline
| Manual ID | Display Name | Hours | Prereqs | Knowledge Reveals | Acquisition Paths |
|---|---|---|---|---|---|
| `manual_subterranean_cartography` | Subterranean Fault & Vault Cartography | 14h | None | `knowledge_ruin_structural_survey` | `table_loot_industrial`, `loc_denial_cut_substation` |
| `manual_salvage_mechanics` | High-Yield Scrap Extraction & Rigging | 12h | None | `knowledge_scavenge_efficiency` | `table_loot_industrial`, `loc_the_allotments` |
| `manual_hazmat_breaching_drills` | Hazmat Vault Breaching & Hot-Zone Infiltration | 18h | `manual_subterranean_cartography` | `knowledge_hazmat_breaching_technique` | `table_loot_military_bunker`, `loc_denial_cut_substation` |
| `manual_wasteland_taxonomy` | Wasteland Botanical & Mineral Field Guide | 14h | None | `knowledge_field_guide_taxonomy` | `table_loot_residential`, `loc_the_allotments` |

### 2.6 Combat Discipline
| Manual ID | Display Name | Hours | Prereqs | Knowledge Reveals | Acquisition Paths |
|---|---|---|---|---|---|
| `manual_improvised_weapons` | Improvised Weapons Fabrication | 14h | None | `knowledge_combat_training` | `table_loot_military_depot`, `loc_the_allotments` |
| `manual_ballistic_handloading` | Precision Match Handloaded Ammunition | 15h | `manual_improvised_weapons` | `knowledge_precision_ballistics` | `table_loot_military_depot`, `loc_denial_cut_substation` |
| `manual_fortified_chokepoints` | Corridor Defense & Fortified Chokepoints | 16h | `manual_improvised_weapons` | `knowledge_fortified_chokepoints` | `table_loot_military_bunker`, `loc_the_allotments` |
| `manual_defensive_tripwire_doctrine` | Perimeter Tripwire & Area Denial Arrays | 14h | `manual_improvised_weapons` | `knowledge_defensive_tripwire_arrays` | `table_loot_military_depot`, `loc_denial_cut_substation` |

---

## 3. Acquisition Verification

1. **Loot Tables & Caravans:** Every manual declares viable salvage pools (`loot_table_ids`), expedition ruins (`expedition_reward_ids`), and trader caravans (`trader_pool_ids`), connecting reading material directly to the wasteland exploration loop.
2. **Technical Tier Distribution:**
   - Tier 1 (Fundamental Basics): 6 manuals
   - Tier 2 (Applied Operational): 8 manuals
   - Tier 3 (Advanced Systems): 7 manuals
   - Tier 4 (Specialized Protocol): 3 manuals
3. **Power Dependency:**
   - Manuals requiring active electrical testing or electronics repair mandate power (`requires_power: true`).
   - Fundamental theory and field hygiene manuals can be read under emergency candle or bioluminescent lighting (`requires_power: false`).
