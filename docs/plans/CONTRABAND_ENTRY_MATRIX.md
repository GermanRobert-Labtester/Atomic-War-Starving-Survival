# CONTRABAND ENTRY MATRIX — Plan 147 Task A.1/A.2

Complete inventory of all 20 records in
`Assets/StreamingAssets/Data/narrative/bunker_contraband_barter.json`
(schema v1), with every non-default mechanics field each entry actually
authored. "Non-default" = every key present in the entry's `mechanics` object
(absent keys are zero/false defaults of the typed class, or untyped).

## Cross-references (`contraband_*` ID sweep, Task A.2)

Grep of every `contraband_` id across items, recipes, quests, trade scenarios,
narrative, factions and tests (2026-09-06):

- **No other data file references any `contraband_*` id.** No item, recipe,
  quest, trade scenario, caravan stock, encounter or faction record links to
  these 20 entries. The only `contraband_` strings outside the catalog are
  prefix-allowlist entries in `CatalogIntegrityValidator`/`CatalogIntegrityRules`
  and the test files.
- `GoodsCatalog` (Plan 56) has a `contraband` **trade category** with one good —
  unrelated to these record ids; no linkage, no conflict.
- `quest_crossing_contraband_medical_vial` is a Crossing quest id containing the
  word, not a reference to this catalog.
- Conclusion: **zero dangling inbound references; zero duplicated item
  definitions.** The catalog is an isolated definition island, which is what
  made a clean identity layer possible without touching any other system.

## Entry matrix

| ID | Category | Tier | Scrip price | Stash location | Authored mechanics fields |
|---|---|---|---|---|---|
| `contraband_copper_condenser_coil` | distillation | 2 | 45 | exhaust_air_plenum_baffle_7 | morale_delta=6, trade_value_bonus=15, tribunal_suspicion_rate=0.12, requires_clean_water_liters=2 |
| `contraband_illicit_triode_tube` | telecom | 3 | 90 | battery_room_dead_lead_cell_core | morale_delta=4, radio_range_boost_km=18, tribunal_suspicion_rate=0.25, emp_shielded=true |
| `contraband_mimeographed_heresy_pamphlet` | literature | 2 | 20 | dormitory_bunk_slat_groove_b12 | morale_delta=8, faction_influence_rebuilder=-5, faction_influence_tempest=10, tribunal_suspicion_rate=0.18 |
| `contraband_lead_counterfeit_slugs` | currency | 2 | 30 | latrine_cistern_overhead_float_tank | scrip_purchasing_falsification=30, vending_machine_jam_chance=0.08, tribunal_suspicion_rate=0.15 |
| `contraband_paraffin_candle_hoard` | lighting | 1 | 18 | air_filter_housing_discard_bin | morale_delta=5, blackout_illumination_hours=36, tribunal_suspicion_rate=0.04 |
| `contraband_unrationed_sugar_brick` | luxury_rations | 2 | 60 | hydroponics_under_deck_sump_pipe | morale_delta=12, calorie_surplus_kcal=3200, fermentation_accelerator=true, tribunal_suspicion_rate=0.09 |
| `contraband_prison_tattoo_needle_rig` | body_modification | 1 | 25 | electrical_junction_box_44_false_rear | dweller_loyalty_boost=8, infection_risk_percentage=0.05, tribunal_suspicion_rate=0.06 |
| `contraband_siphon_hose_and_bulb` | theft_utility | 3 | 75 | generator_room_sump_drain_trap | diesel_theft_liters_per_use=5, fuel_sabotage_potential=0.2, tribunal_suspicion_rate=0.3 |
| `contraband_unregistered_geiger_crystal` | detection | 3 | 110 | vent_shaft_intake_grille_strut | rad_detection_precision_multiplier=2.5, early_storm_warning_minutes=45, tribunal_suspicion_rate=0.22 |
| `contraband_bootleg_morphine_ampoules` | narcotics | 3 | 140 | morgue_cold_drawer_dead_space_6 | instant_pain_relief_hp=40, trauma_suppression_duration_days=3, chemical_dependency_risk=0.35, tribunal_suspicion_rate=0.28 |
| `contraband_modified_filter_cartridge` | safety_tampering | 2 | 35 | decon_lock_overcoat_peg_interior | rad_resistance_delta=-0.15, air_flow_ease_factor=1.4, tribunal_suspicion_rate=0.08 |
| `contraband_card_deck_pinned_kings` | gambling | 1 | 15 | recreation_room_accordion_bellows | scrip_gambling_win_rate_boost=0.25, barter_brawl_chance=0.1, morale_delta=4 |
| `contraband_smuggled_coffee_grounds` | luxury_rations | 2 | 85 | vent_duct_inspection_elbow_14 | morale_delta=15, fatigue_reduction_hours=12, trade_multiplier=1.35, tribunal_suspicion_rate=0.1 |
| `contraband_subverted_keycard_flasher` | security_bypass | 3 | 160 | server_room_cable_tray_blind_spot | blast_door_override_success_rate=0.85, alarm_trigger_chance=0.15, tribunal_suspicion_rate=0.4 |
| `contraband_uninspected_lard_tin` | luxury_rations | 2 | 50 | cold_water_riser_lagging_hollow | hunger_restore_value=45, calorie_surplus_kcal=18000, waterproof_boot_grease_uses=6, tribunal_suspicion_rate=0.07 |
| `contraband_hand_wound_dynamo_spool` | power_harvesting | 1 | 40 | gymnasium_rowing_machine_flywheel_box | battery_recharge_slots=2, noise_generation_db=15, tribunal_suspicion_rate=0.05 |
| `contraband_distillery_hydrometer_glass` | distillation | 2 | 55 | water_lab_broken_centrifuge_housing | alcohol_proof_precision=0.99, distillation_yield_bonus=0.2, tribunal_suspicion_rate=0.11 |
| `contraband_forged_muster_stamp` | forgery | 3 | 130 | commissary_scale_counterweight_recess | ration_chit_forgery_success_rate=0.9, shift_absence_concealment=true, tribunal_suspicion_rate=0.35 |
| `contraband_stolen_nickel_cadmium_cell` | power_harvesting | 2 | 95 | elevator_shaft_counterweight_guide_bracket | emergency_power_storage_kwh=1.2, recharge_cycle_durability=2000, acid_spill_hazard=0.03, tribunal_suspicion_rate=0.16 |
| `contraband_century_seed_grain_vial` | contraband_heirloom | 3 | 200 | crematory_urn_niche_7B | agricultural_yield_multiplier=2.0, genetic_integrity_score=1.0, century_tree_viability=true, tribunal_suspicion_rate=0.45 |

Tier distribution: tier 1 ×4, tier 2 ×9, tier 3 ×7.

## Structural validation (Task A.15)

All 20 rows pass `ContrabandCatalogValidator.ValidateJson` (0 errors):
required strings populated, ids `contraband_`-prefixed and unique, tiers in
[1,3], prices positive integers, mechanics keys on the frozen 45-key
allowlist, all numerics finite and in field-class ranges (probabilities
[0,1], multipliers > 0, quantities ≥ 0, signed keys only where meaningful).
Pinned by `ContrabandPlan147Tests.Validator_AuthoredCatalog_IsValidWith20Entries`
plus 11 tamper-rejection tests.

Identity classification per record: `CONTRABAND_ITEM_IDENTITY_MATRIX.md`.
Field ownership dispositions: `CONTRABAND_MECHANICS_AUTHORITY_MATRIX.md`.
