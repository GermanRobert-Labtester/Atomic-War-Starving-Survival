# Plan 25 Late-Game Continuity Matrix (25G.13)

> Generated from the shipped data; the machine-readable producer→consumer flag map lives in `Assets/StreamingAssets/Data/whitelists/plan25_flags.json` (regenerate: `python3 tools/plan25/generate_flag_whitelist.py`). Dependency classes per 25G.14: **[H]** hard dependency, **[O]** optional integration, **[F]** fallback, **[X]** future hook.

## Peacetime faction actions (`muster_faction_actions.json`, 12)

| Content id | Faction | Consumes | Produces | Witness hook | Muster path | Epilogue hook |
|---|---|---|---|---|---|---|
| act_salvage_rights_offer | scavenger_guild | — | favor_scavenger_claim_recognized, grievance_scavenger_claim_disputed | W3 | via E-P1 chain | testimony ledger |
| act_claim_arbitration | scavenger_guild | — | favor_scavenger_arbitration_fair, grievance_scavenger_arbitration_refused | W3 | — | testimony ledger |
| act_apprentice_rule_dispute | scavenger_guild | — | favor_scavenger_apprentice_backed, grievance_scavenger_registrar_defied | W3, W4 | — | testimony ledger |
| act_purification_toll | hydro_barons | — | favor_hydro_toll_paid, grievance_hydro_toll_defaulted | W5 | via E-P2 | testimony ledger |
| act_hydro_emergency_appeal | hydro_barons | — | favor_hydro_water_accord_honored, grievance_hydro_appeal_refused | W5 | via E-P3 | testimony ledger |
| act_intake_dispute | hydro_barons | — | favor_hydro_intake_audited, grievance_hydro_intake_disputed | W2 | via E-P5 | testimony ledger |
| act_raider_parley | iron_raiders | — | favor_raider_parley_honored, grievance_raider_parley_broken | W7 | via E-P4 | testimony ledger |
| act_raider_passage_levy | iron_raiders | — | grievance_raider_passage_evaded, grievance_raider_passage_fought | W6 | via E-P6 | testimony ledger |
| act_raider_code_dispute | iron_raiders | — | favor_raider_parley_honored, grievance_raider_code_widened | W7 | — | testimony ledger |
| act_coalition_mediation_request | deserter_coalition | camp formed | favor_coalition_mediation_served, grievance_coalition_mediation_refused | W10 | — | testimony ledger |
| act_coalition_supply_appeal | deserter_coalition | camp formed | favor_coalition_supply_shared, grievance_coalition_supply_refused | W10 | strengthens attendance | testimony ledger |
| act_camp_rules_dispute | deserter_coalition | camp formed | favor_coalition_rules_first, grievance_coalition_security_backed | W12 | — | testimony ledger |

## Escalation → war → weariness chains (`faction_war_events.json`, 16 Plan 25 chains)

| Chain | Band | Trigger (closed grammar) | Produces | Consumed by |
|---|---|---|---|---|
| evt_p25_marked_ruin | escalation | FlagTrigger grievance_scavenger_claim_disputed | escalation_marked_ruin (+mediated choice) | testimony, scenes |
| evt_p25_stopped_convoy | escalation | FlagTrigger grievance_hydro_toll_defaulted | escalation_stopped_convoy | W9, confrontation |
| evt_p25_bitter_water | escalation | FlagTrigger grievance_hydro_appeal_refused | escalation_bitter_water(+investigated) | W9, confrontation |
| evt_p25_empty_chair | escalation | FlagTrigger grievance_raider_parley_broken | escalation_empty_chair | W9 |
| evt_p25_cistern_toll_blockade | escalation | FlagTrigger grievance_hydro_intake_disputed | escalation_cistern_blockade(+published) | W9, confrontation |
| evt_p25_prisoner_at_the_gate | escalation | FlagTrigger grievance_raider_passage_fought | escalation_prisoner_gate(+truth_told) | W9, old enemies |
| evt_p25_refugees_from_the_line | war_context | ChainResolved evt_d509_border_clash_span44 **[H 06C]** | war_refugees_arrived | E-R2, shared meal |
| evt_p25_requisition | war_context | ChainResolved evt_d503_conscription_lists **[H 06C]** | war_requisition_demand/met/refused | W8, W11, confrontation |
| evt_p25_broken_route | war_context | ChainResolved evt_d522_switchback_toll **[H 06C]** | — | — |
| evt_p25_field_hospital_overflow | war_context | ChainResolved evt_d533_garrison_offensive_grain_silo **[H 06C]** | war_shelter_took_wounded | W8 |
| evt_p25_deserter_column | war_context | ChainResolved evt_d545_ration_plaza_strike **[H 06C]** | — | — |
| evt_p25_retaliation | war_context | ChainResolved evt_d552_rebuilders_fracture **[H 06C]** | war_sheltered_retaliation_families | confrontation |
| evt_p25_no_more_volunteers | weariness | ChainResolved evt_d565_hydro_leverage_break **[H 06C]** | peace_volunteers_dry | W13 (queue singer) |
| evt_p25_bread_before_bullets | weariness | FlagTrigger war_refugees_arrived | peace_bread_before_bullets | W13, shared meal |
| evt_p25_quiet_faction | weariness | ChainResolved evt_d578_shrine_strike_anomaly **[H 06C]** | peace_faction_forms | **MusterPath PeacePressure [O]**, W11, E-R4 |
| evt_p25_refusal_at_dawn | weariness | FlagTrigger peace_faction_forms | peace_refusal_at_dawn | shared meal |

War terminator stays 06C canon: `evt_d588_ceasefire_by_exhaustion` **[H]** — Plan 25 never ends the war.

## Witnesses (15 total, `muster_witnesses.json`) and scenes (4, `muster_camp_scenes.json`)

Variant flag bindings per witness/scene are in the JSON itself (`requires_*_flags`) and in the flag whitelist. Scene selection reads `MusterState.musterPath` **[H Plan 25 internal]** + board flags. Witness results persist in `MusterState.witnessResults` **[H]** → Plan 15A/15B consume **[X]**.

## Cross-plan dependency classification (25G.14)

- **[H]** 06C war spine (consume only), MusterSystem/CoalitionCamp canon, flag ledger, four faction systems' own scalars.
- **[O]** 16C treaties (mechanical feed shipped — `RegionalTreatyFeed`), 22C foundry state, 20B npc ids (subject_id binding deferred), 09 palliative (census binding deferred), 12A lineage (deferred).
- **[F]** v1 witness files load forever; empty catalogs → empty offers; scenes fall back to ungated variants; MusterPath `unsettled` fallback.
- **[X]** Plan 15A/15B epilogue/Verdict consumption of `MusterState.musterPath` + `witnessResults`; witness suppression (Plan 21); prevented-war ending flag.
