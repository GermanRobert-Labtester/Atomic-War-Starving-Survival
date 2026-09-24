# Prose Expansion Subject Plans — Serial Queue 175

Fifty bounded prose proposals selected from the compiled Ashfall Master Expansion Authority v2.0 and fresh live-data checks. This is a serial queue, not a single execution wave: promote at most one Lane A proposal per wave.

**Scale:** requested 40k–80k (and “closer to 100k”) characters per plan is not an authorized quality target. The v2.0 constitution rejects padding; its factory invariant forbids widening a bounded plan to meet a size target. Counts below are tracking only.

**Premise sweep (2026-09-24):** current factory steps/templates, coordination and test authorities, debt and unclaimed-content census reviewed; A-42 and its measured contract reread; full live `events.json` and `incidents.json`, the catalog registry, and `docs/lore/04_ENCOUNTERS.md` reconciled. Proposals only: no source data, code, flags, routes, or ledgers changed.

## Census and authority corrections

- **VERIFIED local census:** `events.json` has 240 unique IDs/records and 5 exact field-shape variants. `bodyText` is 27–163 words; median 71; 31 entries below 40 words, 64 below 55, and 151 below 80.
- **VERIFIED local census:** `incidents.json` has 25 records. The five A-42 legacy targets are 23, 26, 29, 33, and 35 words.
- **Correction:** current events/incidents share 0 exact IDs. Volume 49’s claimed `fallout_storm` ID mirror is stale: the incident file has `incident_fallout_storm_approach`, with no exact match in the event array. Manual semantic comparison remains necessary.
- **Correction:** current event entries use 5 field shapes, including optional data. The earlier five-field fragment does not describe all records. Each subject freezes its exact live object.

**Routes:** the five incidents are A-42 measured DATA-ONLY prose candidates under the Case D content boundary. The 45 events sit below the freshly measured 71-word median; this ranks them for review but proves no defect. `EventsHostSession` loads/retrieves by ID, while exact selector dispatch remains an open premise in each event plan.

| # | Plan | Source selector | Cluster | Words | Characters |
|---:|---|---|---|---:|---:|
| 1 | [Water Contamination](pa175_01_c1_incident_water_contamination.md) | `incident_water_contamination` · `incidents.json` | A / C1 | 23 | 6,613 |
| 2 | [Radio Interference](pa175_02_c1_incident_radio_interference.md) | `incident_radio_interference` · `incidents.json` | A / C1 | 26 | 6,557 |
| 3 | [Ambush in Sector 4](pa175_03_c1_incident_ambush_in_sector_4.md) | `incident_ambush_sector_4` · `incidents.json` | A / C1 | 29 | 6,552 |
| 4 | [Radiation Spike](pa175_04_c1_incident_radiation_spike.md) | `incident_radiation_spike` · `incidents.json` | A / C1 | 33 | 6,550 |
| 5 | [Bunker Breach Attempt](pa175_05_c1_incident_bunker_breach_attempt.md) | `incident_bunker_breach` · `incidents.json` | A / C1 | 35 | 6,579 |
| 6 | [Nothing Seized Stays Seized](pa175_06_a_event_nothing_seized_stays_seized.md) | `narrative_machinist_mastery` · `events.json` | A / C9 | 27 | 6,704 |
| 7 | [Master Machinist](pa175_07_a_event_master_machinist.md) | `narrative_trade_mastery_machinist` · `events.json` | A / C9 | 28 | 6,679 |
| 8 | [The Second Shift](pa175_08_a_event_the_second_shift.md) | `narrative_nurse_milestone_2` · `events.json` | A / C9 | 32 | 6,684 |
| 9 | [The Clinic Holds](pa175_09_a_event_the_clinic_holds.md) | `narrative_nurse_mastery` · `events.json` | A / C9 | 33 | 6,671 |
| 10 | [The Bench Works](pa175_10_a_event_the_bench_works.md) | `narrative_machinist_milestone_2` · `events.json` | A / C9 | 34 | 6,689 |
| 11 | [Master Electrician](pa175_11_a_event_master_electrician.md) | `narrative_trade_mastery_electrician` · `events.json` | A / C9 | 34 | 6,725 |
| 12 | [First Turn](pa175_12_a_event_first_turn.md) | `narrative_machinist_milestone_1` · `events.json` | A / C9 | 35 | 6,664 |
| 13 | [The Needle Finds Its Mark](pa175_13_a_event_the_needle_finds_its_mark.md) | `narrative_sewing_machine_restored` · `events.json` | A / C9 | 36 | 6,694 |
| 14 | [A Voice in the Static](pa175_14_a_event_a_voice_in_the_static.md) | `narrative_ham_radio_restored` · `events.json` | A / C9 | 36 | 6,676 |
| 15 | [The First Lesson](pa175_15_a_event_the_first_lesson.md) | `narrative_teacher_milestone_1` · `events.json` | A / C9 | 36 | 6,686 |
| 16 | [Master Teacher](pa175_16_a_event_master_teacher.md) | `narrative_trade_mastery_teacher` · `events.json` | A / C9 | 36 | 6,690 |
| 17 | [The Grid Wakes](pa175_17_a_event_the_grid_wakes.md) | `narrative_electrician_milestone_2` · `events.json` | A / C9 | 36 | 6,705 |
| 18 | [Decoder Back Online](pa175_18_a_event_decoder_back_online.md) | `frequency_decoder_reset` · `events.json` | A / C8 | 36 | 6,699 |
| 19 | [Master Nurse](pa175_19_a_event_master_nurse.md) | `narrative_trade_mastery_nurse` · `events.json` | A / C9 | 37 | 6,674 |
| 20 | [The Music Box Turns](pa175_20_a_event_the_music_box_turns.md) | `narrative_music_box_restored` · `events.json` | A / C9 | 37 | 6,670 |
| 21 | [The Record Resumes](pa175_21_a_event_the_record_resumes.md) | `narrative_typewriter_restored` · `events.json` | A / C9 | 37 | 6,680 |
| 22 | [Fresh Boar Wallower Discovered](pa175_22_a_event_fresh_boar_wallower_discovered.md) | `event_eco_boar_rut_wallow_ambush` · `events.json` | A / C14 | 37 | 7,066 |
| 23 | [The Third Note](pa175_23_a_event_the_third_note.md) | `narrative_violin_restored` · `events.json` | A / C9 | 38 | 6,652 |
| 24 | [Bait Stolen](pa175_24_a_event_bait_stolen.md) | `trap_bait_stolen` · `events.json` | A / C15 | 38 | 6,626 |
| 25 | [First Circuit Restored](pa175_25_a_event_first_circuit_restored.md) | `narrative_electrician_milestone_1` · `events.json` | A / C9 | 38 | 6,725 |
| 26 | [Field Dressing](pa175_26_a_event_field_dressing.md) | `narrative_nurse_milestone_1` · `events.json` | A / C9 | 38 | 6,689 |
| 27 | [Saint Maren: Full Tape](pa175_27_a_event_saint_maren_full_tape.md) | `narrative_cassette_saint_maren_complete` · `events.json` | A / C9 | 38 | 6,753 |
| 28 | [The Camera Sees](pa175_28_a_event_the_camera_sees.md) | `narrative_camera_restored` · `events.json` | A / C9 | 39 | 6,658 |
| 29 | [Checkpoint Kilo: Full Tape](pa175_29_a_event_checkpoint_kilo_full_tape.md) | `narrative_cassette_checkpoint_kilo_complete` · `events.json` | A / C9 | 39 | 6,786 |
| 30 | [A Second Class](pa175_30_a_event_a_second_class.md) | `narrative_teacher_milestone_2` · `events.json` | A / C9 | 39 | 6,723 |
| 31 | [The Gramophone Plays](pa175_31_a_event_the_gramophone_plays.md) | `narrative_gramophone_restored` · `events.json` | A / C9 | 40 | 6,706 |
| 32 | [Sprung, Blood Trail](pa175_32_a_event_sprung_blood_trail.md) | `trap_sprung_blood_trail` · `events.json` | A / C15 | 40 | 6,700 |
| 33 | [Moving Pictures Return](pa175_33_a_event_moving_pictures_return.md) | `narrative_projector_restored` · `events.json` | A / C9 | 41 | 6,682 |
| 34 | [Power For The Whole Floor](pa175_34_a_event_power_for_the_whole_floor.md) | `narrative_electrician_mastery` · `events.json` | A / C9 | 41 | 6,761 |
| 35 | [North, Still There](pa175_35_a_event_north_still_there.md) | `narrative_brass_compass_restored` · `events.json` | A / C9 | 41 | 6,708 |
| 36 | [A Second Try at the Numbers](pa175_36_a_event_a_second_try_at_the_numbers.md) | `apprenticeship_completion_signal_ear` · `events.json` | A / C9 | 42 | 7,134 |
| 37 | [Something Climbs](pa175_37_a_event_something_climbs.md) | `narrative_box_kite_restored` · `events.json` | A / C9 | 43 | 6,707 |
| 38 | [The Lens Clears](pa175_38_a_event_the_lens_clears.md) | `narrative_laboratory_microscope_restored` · `events.json` | A / C9 | 44 | 6,706 |
| 39 | [Reconsidering The Walkout](pa175_39_a_event_reconsidering_the_walkout.md) | `post_walkout_reconsider` · `events.json` | A / C9 | 44 | 7,002 |
| 40 | [The School Holds](pa175_40_a_event_the_school_holds.md) | `narrative_teacher_mastery` · `events.json` | A / C9 | 45 | 6,723 |
| 41 | [Free Radio: Full Tape](pa175_41_a_event_free_radio_full_tape.md) | `narrative_cassette_free_radio_complete` · `events.json` | A / C9 | 45 | 6,779 |
| 42 | [The Smell of Morning](pa175_42_a_event_the_smell_of_morning.md) | `narrative_coffee_grinder_restored` · `events.json` | A / C9 | 45 | 6,746 |
| 43 | [The Press Runs](pa175_43_a_event_the_press_runs.md) | `narrative_hand_printing_press_restored` · `events.json` | A / C9 | 48 | 6,708 |
| 44 | [Family Bunker: Full Tape](pa175_44_a_event_family_bunker_full_tape.md) | `narrative_cassette_family_bunker_complete` · `events.json` | A / C9 | 49 | 6,803 |
| 45 | [The Sky Has Detail](pa175_45_a_event_the_sky_has_detail.md) | `narrative_telescope_restored` · `events.json` | A / C9 | 49 | 6,743 |
| 46 | [First Watch, Listed](pa175_46_a_event_first_watch_listed.md) | `post_maturation_watch_readiness` · `events.json` | A / C9 | 51 | 7,019 |
| 47 | [The Clock Keeps Time](pa175_47_a_event_the_clock_keeps_time.md) | `narrative_mantel_clock_restored` · `events.json` | A / C9 | 51 | 6,757 |
| 48 | [The First Solo Repair](pa175_48_a_event_the_first_solo_repair.md) | `apprenticeship_completion_rough_repairs` · `events.json` | A / C9 | 51 | 7,431 |
| 49 | [Grain Refiner](pa175_49_a_event_grain_refiner.md) | `narrative_miller_milestone_2` · `events.json` | A / C9 | 51 | 6,767 |
| 50 | [Cable Splicer](pa175_50_a_event_cable_splicer.md) | `narrative_wireman_milestone_1` · `events.json` | A / C9 | 53 | 6,788 |

## Factory scheduling and disposition

Each proposal is a separate future Lane A wave. The 45 event candidates are shorter than the corpus median, not automatically deficient; a no-op closeout is valid. Do not apply the incident 55–80-word band to events. Exact selectors were checked against previous `docs/expansions/` plans; no overlap was found. No implementation package or file claim is consumed.

## Verification

- 50 distinct live selectors: five measured incident targets and 45 event candidates from the full local census.
- Each plan records exact prose, word count, field shape, non-prose invariants, editorial direction, route, continuity, and open premises.
- Integration requires selector reachability, current ownership, and refreshed census; unresolved routes stay DOCS-ONLY.
- This set is far below the requested per-plan character range. The authority forbids padding. Reviewable polishing briefs were created without game-data changes or broad tests.
