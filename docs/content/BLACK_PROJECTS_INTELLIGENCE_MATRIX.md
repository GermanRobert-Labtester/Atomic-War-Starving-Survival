# BLACK PROJECTS INTELLIGENCE MATRIX — Plan 152 Task A/B

Authority matrix for all 30 records of the BlackProjectsCatalog
(`orbital_kinetic_telemetry.json`, `drone_carrier_blackboxes.json`,
`cobalt_arming_directives.json`, `architect_vault_audits.json` — all
`schema_version: 1`).

**Truth classes** (§5): IT = Instrument Telemetry · VB = Vehicle Blackbox ·
CD = Classified Directive · CA = Compliance/Audit Record · AL = Allegation /
Ambiguous Intelligence.

**Activation** (§19): 16 records activated with real producers (existing
deep-lore sites only); 14 records explicitly DEFERRED with dispositions below.
Producer column: `journal` = activated (site in `deep_lore_locations.json`);
`—` = deferred, no producer.

## Historical identity reconciliation (Task B)

| Archived authority / facility | Nature | Canonical resolution |
|---|---|---|
| `NATIONAL_COMMAND_AUTHORITY_BUNKER_OMEGA` | Pre-war national command authority | **Predecessor institution — no live faction.** Pre-war command structure; extinct as an authority. Plausibly connected to shelter-founding lore but NOT merged into any current faction. |
| `CHEYENNE` (command line) | Pre-war strategic uplink terminus | **Historical facility reference only.** Outside the playable region; no location record exists or may be invented by this plan. |
| `DIRECTORATE_OF_SPECIAL_MUNITIONS` | Pre-war ordnance bureau | **Predecessor institution.** Historical author of metallurgy paperwork; extinct. |
| `AUTONOMIC_RETALIATION_SYSTEM_PERIMETER` | Automated retaliation program | **Historical program, not an organization.** Deliberately NOT linked to `faction_black_ops` (see firewall note). |
| `STRATEGIC_TARGETING_BUREAU` | Pre-war planning bureau | **Predecessor institution.** Its saturation study is a planning document, not an executed order (AL-class ambiguity). |
| `MISSILE_WING_COMMANDER` | Field authority (individual office) | **Historical office.** No live claimant; do not attach to Garrison or any remnant faction. |
| `FACILITY_ENGINEER_CORPS` | Pre-war engineering corps | **Predecessor institution.** Distinct from the Black Ops' Ninth Denial Detachment (denial tasking ≠ engineering corps). |
| `SUPREME_DEFENSE_COUNCIL` | Pre-war supreme command | **Predecessor institution.** Extinct; its final order survives as a document. |
| `OLYMPUS-PLATFORM-01…04` | Orbital weapons platforms | **Historical objects.** The archive records them; no orbital simulation, countdown or impact system exists or may be created. |
| `UAV-CARRIER-VALKYRIE-09` | Autonomous drone carrier | **Historical object.** No drone/carrier entities, items or spawns. |
| `BUNKER-00-ARCHITECT-PRIME` | Deep facility ("the Architect") | **Narrative-only historical facility.** NOT in the location namespace; no vault exists physically in canon; access mapping forbidden (Task G). |
| `AI-MAINTENANCE-SUBROUTINE-7`, `CHIEF_PNEUMATICS_BOT_B`, `BIO_CHEM_SURVEILLOR_4`, `HALON_FIRE_MARSHAL_SYSTEM`, `GEO_STRUCTURAL_PROBE_9`, `RTC_QUARTZ_SYNCHRONIZER`, `MORTUARY_AUTOMATON_01` | Machine auditors | **Dead automation.** In-world authorship flavor; no live AI system is implied. |
| `faction_black_ops` (live) | Black Ops (Ex-Military Rebels) — Ninth Denial Detachment / D/9 | **The only live link:** D/9 held a pre-Exchange continuity tasking. Cobalt records may be *historical context* for the faction's unrescinded order — one-to-many mapping (predecessor authority → surviving detachment). Do not collapse D/9 into NATIONAL_COMMAND_AUTHORITY or the missile wings. |

**Firewall note on the Black Ops link:** the archive NEVER marks records as
"belonging to" `faction_black_ops` and never mutates faction state. The
D/9 relationship is interpretive lore for the player to assemble.

## Orbital Kinetic Telemetry (8) — truth class IT

| ID | Callsign | Entry type | Alt km | Decay m/d | Payload status | Activated / Producer | Disposition & continuity |
|---|---|---|---|---|---|---|---|
| `telemetry_olympus_perigee_decay` | OLYMPUS-PLATFORM-04 | orbital_decay_log | 342.6 | 145.2 | 5_OF_8_TUNGSTEN_HARPOONS_ARMED | ✅ `location_radar_site` | Recorded decay; no countdown. Present status unknown. |
| `telemetry_olympus_guidance_star_loss` | OLYMPUS-PLATFORM-04 | optical_tracker_error | 338.1 | 162.8 | STAR_TRACKER_BLINDED_BY_SOOT | DEFERRED | Nuclear-winter soot claim; keep for a radio-intercept producer pass (Plan 73/24) before activation. |
| `telemetry_olympus_harpoon_07_discharge` | OLYMPUS-PLATFORM-02 | automated_discharge_record | 410.0 | 85.0 | HARPOON_07_RELEASED_AUTONOMOUSLY | DEFERRED | CRITICAL sensitivity: an autonomous discharge record must never imply a live launch path or target reveal. Deferred pending an explicit evidence consumer; impact claim stays uncorroborated (AL-flavored). |
| `telemetry_olympus_inertial_gyro_drift` | OLYMPUS-PLATFORM-01 | gyroscope_maintenance_dump | 315.4 | 240.5 | BEARING_SEIZURE_WARNING | ✅ `location_ammunition_depot` | Maintenance dump; mechanical-wear continuity is internally consistent. |
| `telemetry_olympus_ground_station_silence` | OLYMPUS-PLATFORM-04 | uplink_carrier_timeout | 340.0 | 150.0 | AWAITING_CHEYENNE_COMMAND_LINE | ✅ `location_radar_site` | Corroborates the dead command web (curated: corroborates the IFF loop above). |
| `telemetry_olympus_sub_armor_impact_calc` | OLYMPUS-PLATFORM-03 | ballistic_impact_projection | 290.2 | 310.0 | 3_HARPOONS_REMAINING | DEFERRED | A *projection*, not a strike. Activating it risks reading it as an event; defer until a confirmed-strike narrative anchor exists. |
| `telemetry_olympus_solar_array_spallation` | OLYMPUS-PLATFORM-04 | power_bus_degradation_log | 335.5 | 170.0 | BUS_VOLTAGE_21V_UNDER_NOMINAL | ✅ `location_weather_station` | Power-degradation telemetry; consistent with nuclear-winter flux. |
| `telemetry_olympus_terminal_deorbit_burn` | OLYMPUS-PLATFORM-04 | terminal_flight_profile | 185.0 | 1200.0 | TERMINAL_DESCENT_ARMED | DEFERRED | Endgame-adjacent (kinetic_death/endgame tags). 1200 m/day at 185 km ≈ terminal timeline — deliberately left ambiguous; forbidden: any countdown. Revisit only with an explicit endgame event owner (Plan 89 boundary). |

Drone Carrier Blackboxes (8) — truth class VB

| ID | Carrier | Record type | Alt ft | Kts | System health | Activated / Producer | Disposition & continuity |
|---|---|---|---|---|---|---|---|
| `blackbox_valkyrie_takeoff_sortie_01` | UAV-CARRIER-VALKYRIE-09 | catapult_launch_telemetry | 1200 | 240 | 100_PERCENT_AUTOMATED_ONLINE | ✅ `location_ammunition_depot` | Pre-war launch record; carrier-based catapult claim is self-consistent. |
| `blackbox_valkyrie_waypoint_radiation_burn` | UAV-CARRIER-VALKYRIE-09 | sensor_damage_event | 28000 | 410 | OPTICAL_CHANNEL_DEGRADED_GAMMA | ✅ `location_irradiated_forest` | Producer = the burned flight path (thematic, no coordinate reveal). |
| `blackbox_valkyrie_fuel_conservation_orbit` | UAV-CARRIER-VALKYRIE-09 | loiter_profile_log | 36000 | 195 | STARBOARD_ENGINE_IDLE_CONSERVATION | DEFERRED | Loiter-duration claim implies a still-airborne timeline; defer until continuity pass on the carrier's fate. |
| `blackbox_valkyrie_radar_interrogation_loop` | UAV-CARRIER-VALKYRIE-09 | iff_transponder_timeout | 32000 | 210 | NO_IFF_CORRELATION | ✅ `location_radar_site` | Curated: corroborates the misidentification strike log. |
| `blackbox_valkyrie_target_misidentification` | UAV-CARRIER-VALKYRIE-09 | strike_authorization_log | 14000 | 320 | WEAPONS_BAY_DOORS_LATCHED | ✅ `location_ammunition_depot` | Tragedy record (AL ambiguity inside VB); never grants weapons anything. |
| `blackbox_valkyrie_ammo_exhaustion_strike` | UAV-CARRIER-VALKYRIE-09 | ordnance_release_report | 8500 | 360 | MAGAZINE_EMPTY | DEFERRED | Depleted-uranium strafing log; defer with the carrier-fate continuity pass. |
| `blackbox_valkyrie_engine_flameout_crash` | UAV-CARRIER-VALKYRIE-09 | crash_impact_log | 0 | 0 | AIRFRAME_DESTROYED_IMPACT | DEFERRED | Implies a wreck site; no canonical crash location exists — defer rather than invent (Plan 116 boundary). |
| `blackbox_valkyrie_recovery_beacon_pulse` | UAV-CARRIER-VALKYRIE-09 | locator_beacon_status | 0 | 0 | BEACON_ACTIVE_BATTERY_LOW | DEFERRED | Active beacon implies a discoverable site; requires an authored salvage anchor first. |

Cobalt Arming Directives (7) — truth class CD (all historical document fields; salvo sizes are archive data)

| ID | Directive code | Classification | Issuing authority | Salvo | Activated / Producer | Disposition & continuity |
|---|---|---|---|---|---|---|
| `directive_cobalt_dual_key_authorization` | EO-99-Z-COBALT-RELEASE | TOP_SECRET_ORCHID_EYES_ONLY | NATIONAL_COMMAND_AUTHORITY_BUNKER_OMEGA | 24 | ✅ `location_ammunition_depot` | Release-order document; two-man rule continuity is self-consistent. |
| `directive_cobalt_salted_casing_inspection` | TECH-ORD-441-METALLURGY | SECRET_RESTRICTED_ORDNANCE | DIRECTORATE_OF_SPECIAL_MUNITIONS | 12 | ✅ `location_chemical_plant` | Cobalt-59 salting physics; technical claims left qualitative. |
| `directive_cobalt_dead_hand_perimetr_link` | DIRECTIVE-88-AUTOMATIC-TRIGGER | TOP_SECRET_AUTOMATED_WARFARE | AUTONOMIC_RETALIATION_SYSTEM_PERIMETER | 12 | DEFERRED | Most executable-looking record ("automatic trigger"). Deferred as firewall hardening; revisit only with an explicit verdict/quest evidence consumer. |
| `directive_cobalt_atmospheric_saturation_quota` | PLAN-AERO-201-SATURATION | TOP_SECRET_STRATEGIC_STUDY | STRATEGIC_TARGETING_BUREAU | 60 | ✅ `location_chemical_plant` | A study (AL-flavored: planning ≠ execution); extinction-scale claims are propaganda-ambiguous. |
| `directive_cobalt_abort_window_revocation` | ORD-REVOKE-ABORT-00 | SECRET_COMBAT_IMMEDIATE | MISSILE_WING_COMMANDER | 18 | ✅ `location_ammunition_depot` | Abort-revocation order; irreversibility theme, no live mechanic. |
| `directive_cobalt_sub_surface_silo_flood` | BURIAL-PROTOCOL-VOID-7 | SECRET_FACILITY_DENIAL | FACILITY_ENGINEER_CORPS | 0 | DEFERRED | Self-denial protocol (salvo 0); defer until any silo/facility lore anchor exists. |
| `directive_cobalt_crematory_silence_pact` | FINAL-ORDER-SILENCE-300 | EYES_ONLY_LAST_OFFICER | SUPREME_DEFENSE_COUNCIL | 0 | DEFERRED | Final-officer suicide pact (tragedy record). Deferred: strongest narrative beat; keep unactivated until a deliberate quest context exists. |

Architect Vault Audits (7) — truth class CA (BUNKER-00-ARCHITECT-PRIME = narrative-only facility; no access mapping; compliance_status never grants anything)

| ID | Sub-level | Auditor | Audit type | Activated / Producer | Disposition & continuity |
|---|---|---|---|---|---|
| `audit_architect_vault_biometric_decay` | LEVEL_01_COMMAND_SANCTUARY | AI-MAINTENANCE-SUBROUTINE-7 | biometric_access_audit | ✅ `location_metro_station` | Evacuation-era civic copy; biometric-decay finding is archival. |
| `audit_architect_vault_air_filtration_overhaul` | LEVEL_04_ATMOSPHERE_SCRUBBING | CHIEF_PNEUMATICS_BOT_B | life_support_maintenance_log | ✅ `location_metro_station` | Scrubber overhaul record; shelter-relevant but informational only. |
| `audit_architect_vault_nutrient_broth_rancidity` | LEVEL_06_BIO_SYNTHESIS_VATS | BIO_CHEM_SURVEILLOR_4 | hydroponics_chemical_audit | ✅ `location_agricultural_research` | Spoilage finding; cross-links thematically with Plan 151 abyssal/scientific overlap (no record duplication). |
| `audit_architect_vault_automated_purge_test` | LEVEL_05_CENTRAL_ARCHIVE_CORE | HALON_FIRE_MARSHAL_SYSTEM | security_fire_purge_drill | DEFERRED | Lethal-automated-security record; defer to avoid reading it as a hazard/unlock on any real site. |
| `audit_architect_vault_lead_shielding_subsidence` | LEVEL_03_LEAD_BISMUTH_CURTAIN | GEO_STRUCTURAL_PROBE_9 | structural_shielding_inspection | ✅ `location_steelworks` | Metallurgy/structural record; thematically at home at the steelworks. |
| `audit_architect_vault_governance_ai_clock_drift` | LEVEL_02_MAINFRAME_VAULT | RTC_QUARTZ_SYNCHRONIZER | chronometer_parity_check | DEFERRED | Curated `contradicts` pair with the final census; activate together in a dedicated mystery pass. |
| `audit_architect_vault_last_survivor_cremation` | LEVEL_01_MEDICAL_SUITE | MORTUARY_AUTOMATON_01 | demographic_census_final | DEFERRED | Extinction/tragedy record (the ghost-bunker beat); deliberately ambiguous — defer. |

## Allowed consumers & forbidden interpretations (global)

| Record family | Allowed consumer | Forbidden interpretation |
|---|---|---|
| Orbital telemetry | Journal intel; radio/telemetry intercept flavor (Plan 73/24) if routed | Countdowns, impact scheduling, orbital simulation, payload control |
| Drone blackboxes | Journal intel; codex page; expedition flavor | Drone/carrier spawns, items, avionics grants, combat triggers |
| Cobalt directives | Journal intel; codex page; journal interpretation; evidence ONLY if an existing system accepts a stable evidence ID | Firing/arming, faction aggression, hazard zones, clearance, "directive interpreters" |
| Vault audits | Journal intel; codex page; Plan 151 cross-links | Vault access, doors, loot reveal, new locations, hazard unlocks |

## Presentation (Task I)

Every record renders with: classification/family banner, source callsign or
facility, relative timestamp (`timestamp_relative` strings — pre-war day
semantics, NOT the campaign clock), technical values as recorded, prose, and
the standing label `ARCHIVAL — PRESENT STATUS UNKNOWN`. Redaction is
presentation metadata (the classification strings above); no clearance
progression exists or is created by Plan 152.

## Continuity / plausibility audit (Task J) — findings

1. **Orbital decay consistency:** PLATFORM-04 records are internally
   consistent (decay 145→162→170→150 m/day as altitude 342.6→335.5→338.1→340).
   The 1200 m/day terminal record at 185 km is a deliberate discontinuity
   (terminal profile) — kept, and marked deferred/ambiguous.
2. **Flight profile plausibility:** VALKYRIE altitudes (1200 ft launch →
   28–36 kft cruise → 14 kft strike → 8.5 kft strafe → 0 crash) and speeds
   (240 → 410/195/210/320/360 kts) are plausible for a turboprop-era carrier
   UAV; no contradictions found.
3. **Directive chronology:** dual-key release (salvo 24) → metallurgy
   inspections (12) → saturation study (60, study only) → abort revocation
   (18) reads consistently; the two salvo-0 records are correctly non-strike
   protocols.
4. **Salvo-vs-canon:** salvo sizes (12–60) are consistent with a regional
   strategic scale; no contradictions with the shelter-scale world canon.
5. **Ambiguity preserved:** the automated discharge, saturation study and
   final census are encoded as ambiguous/projection/study records — the
   archive presents them as recorded, not as verified events.
