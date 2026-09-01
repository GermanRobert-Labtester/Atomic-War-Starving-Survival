# Hardcoded Research Inventory (Plan 34 §34A.1) — baseline 2026-09-01

Source of truth at reconnaissance time: `Assets/Ashfall.Core/Research/ResearchSystem.cs` → `RegisterDefaults()` (lines 59–193).

`RegisterDefaults()` hardcodes **31 nodes**: the original 15 (the Plan 34 save-contract set) plus 16 relic reverse-engineering blueprint nodes added later. All 31 are **value-identical** in `research_knowledge.json` (verified field-by-field: id, display_name, category, description, days_to_complete, prerequisites, breakthrough_item — 0 mismatches). The JSON catalog holds 25 further nodes that exist **only** in JSON.

Registration order below is the C# registration order (== save-relevant insertion order while defaults were live).

| # | knowledge_id | category | days | prerequisites | breakthrough_item |
|---|---|---|---|---|---|
| 1 | `knowledge_water_basics` | survival | 5 | — | — |
| 2 | `knowledge_water_advanced` | survival | 12 | knowledge_water_basics | item_water_filter_advanced |
| 3 | `knowledge_radiation_basics` | medical | 5 | — | — |
| 4 | `knowledge_radiation_shielding` | engineering | 15 | knowledge_radiation_basics | item_radiation_shielding_panel |
| 5 | `knowledge_gas_mask_improved` | engineering | 10 | knowledge_radiation_basics | item_gas_mask_improved |
| 6 | `knowledge_hydroponics` | survival | 8 | — | — |
| 7 | `knowledge_solar_basics` | engineering | 7 | — | — |
| 8 | `knowledge_solar_advanced` | engineering | 14 | knowledge_solar_basics | item_solar_inverter |
| 9 | `knowledge_food_preservation` | survival | 10 | — | — |
| 10 | `knowledge_radio_basics` | science | 6 | — | — |
| 11 | `knowledge_radio_advanced` | science | 12 | knowledge_radio_basics | item_radio_cipher_rotor |
| 12 | `knowledge_shelter_insulation` | engineering | 8 | — | — |
| 13 | `knowledge_air_filtration` | engineering | 10 | knowledge_shelter_insulation | item_air_filter_hepa |
| 14 | `knowledge_scavenge_efficiency` | scavenging | 7 | — | — |
| 15 | `knowledge_combat_training` | combat | 8 | — | — |
| 16 | `knowledge_micro_dosimeter_blueprint` | medical | 6 | — | item_dosimeter_calibrated |
| 17 | `knowledge_water_condenser_blueprint` | engineering | 8 | — | item_desal_membrane |
| 18 | `knowledge_signal_amplifier_blueprint` | science | 6 | — | item_radio_vacuum_tube |
| 19 | `knowledge_battery_reconditioner_blueprint` | engineering | 8 | — | item_battery_reconditioned |
| 20 | `knowledge_hydroponic_doser_blueprint` | survival | 7 | — | item_hydroponic_nutrients |
| 21 | `knowledge_uv_sterilizer_blueprint` | medical | 7 | — | item_surgical_kit |
| 22 | `knowledge_hand_centrifuge_blueprint` | medical | 5 | — | item_reagent_clean |
| 23 | `knowledge_seismic_geophone_blueprint` | scavenging | 6 | — | item_seismic_detector |
| 24 | `knowledge_turret_controller_blueprint` | combat | 10 | — | item_sentry_targeting_chip |
| 25 | `knowledge_encrypted_radio_blueprint` | science | 10 | — | item_military_radio_module |
| 26 | `knowledge_radar_scope_blueprint` | scavenging | 9 | — | item_radar_display_tube |
| 27 | `knowledge_power_armor_servo_blueprint` | engineering | 12 | — | item_hydraulic_actuator |
| 28 | `knowledge_vault_breach_blueprint` | scavenging | 8 | — | item_thermal_lance |
| 29 | `knowledge_iff_transponder_blueprint` | combat | 8 | — | item_iff_beacon |
| 30 | `knowledge_cbrn_filter_blueprint` | survival | 9 | — | item_cbrn_cartridge |
| 31 | `knowledge_surgical_robot_blueprint` | medical | 12 | — | item_surgical_arm_servo |

Original 15 (save-contract IDs, registration order): `knowledge_water_basics`, `knowledge_water_advanced`, `knowledge_radiation_basics`, `knowledge_radiation_shielding`, `knowledge_gas_mask_improved`, `knowledge_hydroponics`, `knowledge_solar_basics`, `knowledge_solar_advanced`, `knowledge_food_preservation`, `knowledge_radio_basics`, `knowledge_radio_advanced`, `knowledge_shelter_insulation`, `knowledge_air_filtration`, `knowledge_scavenge_efficiency`, `knowledge_combat_training`

Blueprint 16 (relic reverse-engineering targets): `knowledge_micro_dosimeter_blueprint`, `knowledge_water_condenser_blueprint`, `knowledge_signal_amplifier_blueprint`, `knowledge_battery_reconditioner_blueprint`, `knowledge_hydroponic_doser_blueprint`, `knowledge_uv_sterilizer_blueprint`, `knowledge_hand_centrifuge_blueprint`, `knowledge_seismic_geophone_blueprint`, `knowledge_turret_controller_blueprint`, `knowledge_encrypted_radio_blueprint`, `knowledge_radar_scope_blueprint`, `knowledge_power_armor_servo_blueprint`, `knowledge_vault_breach_blueprint`, `knowledge_iff_transponder_blueprint`, `knowledge_cbrn_filter_blueprint`, `knowledge_surgical_robot_blueprint`

## RegisterDefaults() call sites at baseline (dual-authority map)

| Site | Role | Fate under Plan 34 reconciliation |
|---|---|---|
| `src/Host/ResearchHostSession.cs:27` (ctor) | live research/atlas host; `LoadCatalog` **never called in production** → game ran on the 31 hardcoded nodes | replaced by catalog load from `dataDir` |
| `src/Host/CraftingHostSession.cs:41` | workshop/pharma crafting session; populates the shared `_sharedResearch` as a side effect | removed; `Create(dataDir,…)` loads the JSON catalog when it owns the instance |
| `src/Main.UiTests.PlayerPanels.cs:137` | UI panel selftest | loads real catalog via `EnsureSharedResearch()`; phantom `res_rad_mapping` ID replaced with a real catalog ID |
| `src/Host/PanelBindLifecycleSelfTest.cs:241` | headless panel-bind selftest (Gate 5) | loads real catalog from data dir |
| `ResearchKnowledgeCatalogLoader.LoadAndRegister` fallback | silent `RegisterDefaults()` when file missing — Plan 34 §1.10 violation | removed; missing catalog is a diagnostic, never silent defaults |
| `Ashfall.Core.Tests/ResearchSystemTests.cs:14` | unit-test fixture | replaced by test-only fixture in the test project |

## Downstream consumers of knowledge IDs (all resolve against the 56-node catalog)

- `relic_recipes.json` — 24 `research_unlock_id` refs, 15 distinct IDs — all in catalog.
- `library_manuals.json` — 12 `knowledge_unlocks` refs — all in catalog.
- `autopsy_procedures.json` — 4 distinct knowledge IDs — all in catalog.
- `research_knowledge.json` — 32 `breakthrough_item` refs — all resolve to authored item IDs.
