# Plan 25 Political QA Matrix (25H.1)

> Verification status per content row. Automated coverage: `FactionActionBoardTests` (16), `WitnessSelectionTests` (13), `MusterPathEvaluatorTests` (13), `FactionWarFlagExtensionTests` (9), `FactionEcologySelftestTests` (1 → runs 27-check demo), `RegionalTreatyFeedTests` (3), plus the host verbs `--faction-ecology-selftest`, `--muster-selftest`, `--muster-uitest`, `--data-integrity-selftest`.

## Faction actions (12) — board: day/flag/once/cooldown/band gating, idempotence, save

| Row | Band variants | Standing-sensitive | Grievance produced | Save idempotent | Covered by |
|---|---|---|---|---|---|
| act_salvage_rights_offer | 5 | trust bands | yes | once | BoardTests + demo + uitest surface |
| act_claim_arbitration | 3 (hostile/neutral/allied) | yes | yes | once | demo path (board) |
| act_apprentice_rule_dispute | 1 (neutral, band-agnostic dispute) | n/a (single variant) | yes | once | BoardTests patterns |
| act_purification_toll | 3 | yes | yes | once | BoardTests |
| act_hydro_emergency_appeal | 2 (neutral/allied) | yes | yes | once | BoardTests |
| act_intake_dispute | 1 | single | yes | once | BoardTests |
| act_raider_parley | 3 | aggression band | yes | once | BoardTests (raider band/agg delta) |
| act_raider_passage_levy | 2 | yes | yes | cooldown 20d | BoardTests cooldown |
| act_raider_code_dispute | 1 | single | yes | once | BoardTests |
| act_coalition_mediation_request | 3 | camp band | yes | cooldown 25d | BoardTests camp band |
| act_coalition_supply_appeal | 2 | yes | yes | cooldown 25d | BoardTests |
| act_camp_rules_dispute | 1 | single | yes | once | BoardTests |

## Escalation / war / weariness chains (16)

| Chain | Gate | Impossible-state guard | Test |
|---|---|---|---|
| evt_p25_marked_ruin | grievance flag + minDay 200 | no flag → never surfaces | demo (`E-P1 never surfaces without the grievance`) + FlagExtensionTests |
| evt_p25_stopped_convoy | grievance flag | same class | FlagExtensionTests pattern |
| evt_p25_bitter_water | grievance flag | same | 〃 |
| evt_p25_empty_chair | grievance flag | same | 〃 |
| evt_p25_cistern_toll_blockade | grievance flag | same | 〃 |
| evt_p25_prisoner_at_the_gate | grievance flag | same | 〃 |
| evt_p25_refugees..retaliation (6) | ChainResolved on real 06C stages | battle not run → no fire | ChainRunnerTests trigger totality + minDay |
| evt_p25_no_more_volunteers | ChainResolved evt_d565 | 〃 | 〃 |
| evt_p25_bread_before_bullets | flag_war_refugees_arrived | refugees never arrived → no fire | FlagTrigger tests |
| evt_p25_quiet_faction | ChainResolved evt_d578 | 〃 | 〃 |
| evt_p25_refusal_at_dawn | flag_peace_faction_forms | quiet faction never formed → no fire | FlagTrigger tests |

## Witnesses (15) and scenes (4)

| Concern | Coverage |
|---|---|
| Day gating | WitnessSelectionTests + panel day gate |
| Alive/dead (subject census) | selector test (`DeadSubjectNeverTestifies`); census binding deferred → PortAll fallback documented |
| Faction presence | selector test; institutional exception documented |
| Variant first-match determinism | selector tests + demo (failed/absent variants from real flags) |
| Priority/ordinal ordering, cap+diversity | selector tests |
| Results ledger idempotence + save | demo (`witness delivery idempotent`, `survive save/restore`) |
| Path-sensitive scenes (negotiated/victors/unsettled) | demo (arrivals all three paths + pre-260 dark); same director path covers scenes 2–4 |
| Seen-scene progression | host StageCampScene (SessionGuarded); demo covers director selection |

## Muster paths

| Path | Rule | Test |
|---|---|---|
| victors | dominance + (hostile≥2 or tension≥60) | MusterPathEvaluatorTests |
| negotiated | ≥2 majors + camp + (treaty or peace flag) | 〃 |
| unsettled | fallback | 〃 |
| persistence | MusterState.musterPath additive, old saves empty | 〃 + demo |

## Data & continuity

| Concern | Gate |
|---|---|
| Data integrity (all ids resolve) | `--data-integrity-selftest` PASS 0 findings / 161 catalogs |
| Flag producer→consumer | whitelists/plan25_flags.json + integrity gate (no orphans) |
| Trigger-table totality for new stages | FactionWarChainRunnerTests |
| Schema policy (snake_case + schema_version) | commit hook JSON gate |
| Witness count resilience | MusterContentCatalogTests (v2-aware, ≥3 + founding ids) |
| Host UI | --muster-uitest PASS (roster, witnesses ≥3, epilogues, modal, escalate, camp, strategy, approach, matrix, bias) |
| Manual journey (25H.18) | scripted via --faction-ecology-selftest (action→grievance→escalation→witness→path→scene→save/load); full 15-step hand journey deferred to a playtest session (telemetry skill) |
