# Research Parity Matrix

> **Document Status:** Authoritative Parity Verification
> **Project:** ASHFALL (Godot 4.7+ / .NET 8 / C# Core)
> **Date:** September 2026

---

## 1. 15-Node Base Parity

This table verifies 1:1 parity between the legacy hardcoded nodes and the migrated JSON authority entries:

| Knowledge ID | Category | Days | Legacy Breakthrough | Migrated JSON Breakthrough | Status |
|---|---|---|---|---|---|
| `knowledge_water_basics` | survival | 5 | None | None | EXACT MATCH |
| `knowledge_water_advanced` | survival | 12 | `item_water_filter_advanced` | `item_water_filter_advanced` | EXACT MATCH |
| `knowledge_radiation_basics` | medical | 5 | None | None | EXACT MATCH |
| `knowledge_radiation_shielding` | engineering | 15 | `item_radiation_shielding_panel` | `item_radiation_shielding_panel` | EXACT MATCH |
| `knowledge_gas_mask_improved` | engineering | 10 | `item_gas_mask_improved` | `item_gas_mask_improved` | EXACT MATCH |
| `knowledge_hydroponics` | survival | 8 | None | None | EXACT MATCH |
| `knowledge_solar_basics` | engineering | 7 | None | None | EXACT MATCH |
| `knowledge_solar_advanced` | engineering | 14 | `item_solar_inverter` | `item_solar_inverter` | EXACT MATCH |
| `knowledge_food_preservation` | survival | 10 | None | None | EXACT MATCH |
| `knowledge_radio_basics` | science | 6 | None | None | EXACT MATCH |
| `knowledge_radio_advanced` | science | 12 | `item_radio_cipher_rotor` | `item_radio_cipher_rotor` | EXACT MATCH |
| `knowledge_shelter_insulation` | engineering | 8 | None | None | EXACT MATCH |
| `knowledge_air_filtration` | engineering | 10 | `item_air_filter_hepa` | `item_air_filter_hepa` | EXACT MATCH |
| `knowledge_scavenge_efficiency` | scavenging | 7 | None | None | EXACT MATCH |
| `knowledge_combat_training` | combat | 8 | None | None | EXACT MATCH |

---

## 2. 16 Relic Reverse-Engineering Blueprints Parity

All 16 relic blueprint knowledge nodes match the non-empty `research_unlock_id` entries in `relic_recipes.json`:

| Knowledge ID | Category | Days | Breakthrough Item | Status |
|---|---|---|---|---|
| `knowledge_micro_dosimeter_blueprint` | medical | 6 | `item_dosimeter_calibrated` | EXACT MATCH |
| `knowledge_water_condenser_blueprint` | engineering | 8 | `item_desal_membrane` | EXACT MATCH |
| `knowledge_signal_amplifier_blueprint` | science | 6 | `item_radio_vacuum_tube` | EXACT MATCH |
| `knowledge_battery_reconditioner_blueprint` | engineering | 8 | `item_battery_reconditioned` | EXACT MATCH |
| `knowledge_hydroponic_doser_blueprint` | survival | 7 | `item_hydroponic_nutrients` | EXACT MATCH |
| `knowledge_uv_sterilizer_blueprint` | medical | 7 | `item_surgical_kit` | EXACT MATCH |
| `knowledge_hand_centrifuge_blueprint` | medical | 5 | `item_reagent_clean` | EXACT MATCH |
| `knowledge_seismic_geophone_blueprint` | scavenging | 6 | `item_seismic_detector` | EXACT MATCH |
| `knowledge_turret_controller_blueprint` | combat | 10 | `item_sentry_targeting_chip` | EXACT MATCH |
| `knowledge_encrypted_radio_blueprint` | science | 10 | `item_military_radio_module` | EXACT MATCH |
| `knowledge_radar_scope_blueprint` | scavenging | 9 | `item_radar_display_tube` | EXACT MATCH |
| `knowledge_power_armor_servo_blueprint` | engineering | 12 | `item_hydraulic_actuator` | EXACT MATCH |
| `knowledge_vault_breach_blueprint` | scavenging | 8 | `item_thermal_lance` | EXACT MATCH |
| `knowledge_iff_transponder_blueprint` | combat | 8 | `item_iff_beacon` | EXACT MATCH |
| `knowledge_cbrn_filter_blueprint` | survival | 9 | `item_cbrn_cartridge` | EXACT MATCH |
| `knowledge_surgical_robot_blueprint` | medical | 12 | `item_surgical_arm_servo` | EXACT MATCH |
