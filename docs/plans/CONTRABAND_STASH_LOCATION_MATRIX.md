# CONTRABAND STASH LOCATION MATRIX — Plan 147 Task A.7 / Task B.7

## Decision

Every `hidden_stash_location` value is classified **DESCRIPTIVE — no location
resolution**. None of the 20 strings is a world `loc_` id, and none matches an
interior shelter room id (rooms live in `ShelterThermalSystem` as `room_*`:
`room_airlock`, etc.). The plan's rule — "no string-to-location guesswork" —
is satisfied by not guessing: stash placement for the three activated slices is
owned by the activation map's day gate + once-only state, not by the prose
string. The prose remains player-facing flavor describing where the cache was
hidden inside the bunker.

## Why no mapping file was introduced

- A mapping file (`prose-string → loc_/room_id`) would be speculative: the
  bunker interior has no discovery-position semantics for "exhaust air plenum
  baffle 7", and inventing 20 sub-rooms is scope explosion into shelter
  architecture, explicitly out of scope (Plan §10).
- Discovery is modeled as a **campaign event** (day-gated, once-only), which
  matches how the shelter actually exposes content: actions and states, not
  coordinates.

## Per-entry values (all descriptive)

| Record | hidden_stash_location (descriptive prose id) |
|---|---|
| contraband_copper_condenser_coil | exhaust_air_plenum_baffle_7 |
| contraband_illicit_triode_tube | battery_room_dead_lead_cell_core |
| contraband_mimeographed_heresy_pamphlet | dormitory_bunk_slat_groove_b12 |
| contraband_lead_counterfeit_slugs | latrine_cistern_overhead_float_tank |
| contraband_paraffin_candle_hoard | air_filter_housing_discard_bin |
| contraband_unrationed_sugar_brick | hydroponics_under_deck_sump_pipe |
| contraband_prison_tattoo_needle_rig | electrical_junction_box_44_false_rear |
| contraband_siphon_hose_and_bulb | generator_room_sump_drain_trap |
| contraband_unregistered_geiger_crystal | vent_shaft_intake_grille_strut |
| contraband_bootleg_morphine_ampoules | morgue_cold_drawer_dead_space_6 |
| contraband_modified_filter_cartridge | decon_lock_overcoat_peg_interior |
| contraband_card_deck_pinned_kings | recreation_room_accordion_bellows |
| contraband_smuggled_coffee_grounds | vent_duct_inspection_elbow_14 |
| contraband_subverted_keycard_flasher | server_room_cable_tray_blind_spot |
| contraband_uninspected_lard_tin | cold_water_riser_lagging_hollow |
| contraband_hand_wound_dynamo_spool | gymnasium_rowing_machine_flywheel_box |
| contraband_distillery_hydrometer_glass | water_lab_broken_centrifuge_housing |
| contraband_forged_muster_stamp | commissary_scale_counterweight_recess |
| contraband_stolen_nickel_cadmium_cell | elevator_shaft_counterweight_guide_bracket |
| contraband_century_seed_grain_vial | crematory_urn_niche_7B |

Validator note: these strings carry no known snake_case prefix, so
`CatalogIntegrityValidator` TIER-1 does not (and must not) attempt resolution —
consistent with the descriptive decision above.

## Once-only / reroll policy (Plan Task B.8, C.4)

- Stash discovery is **once-only per campaign**: `ContrabandStashSystem` records
  `claimedDayByEntry` and every repeat `TryClaimStash` returns
  `contraband_already_claimed` — including after save/load round-trips and
  location/screen revisits (no screen has any reroll path; discovery is a
  claim action, not a view).
- No RNG exists anywhere in the discovery path: availability is a pure function
  of (registered activation, claimed set, campaign day). Pinned by
  `Stash_Deterministic_IdenticalSystemsProduceIdenticalAvailabilitySequences`
  and `Stash_SaveRoundTrip_PreservesOnceOnlyClaims`.
- A capacity-blocked claim does **not** consume the stash
  (`Stash_CapacityBlocked_ClaimIsNotRecorded_AndRetrySucceeds`).
