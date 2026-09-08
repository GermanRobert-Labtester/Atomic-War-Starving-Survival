# Moral Choice Echo Branch Coverage Matrix (60 Echo Quests)

**Document ID:** `docs/moral/MORAL_ECHO_BRANCH_MATRIX.md`
**Total Echo Quests:** 60 (32 Baseline + 28 Expansion)

---

## 1. Branch Summary

| Branch | Baseline | Plan 109 Additions | Total Echoes |
|---|:---:|:---:|:---:|
| **The Mercy Road** (`branch_mercy_road`) | 3 | +8 | **11** |
| **The Iron Way** (`branch_iron_way`) | 3 | +8 | **11** |
| **The Listener's Thread** (`branch_listener_thread`) | 3 | +7 | **10** |
| **The Broken Compact** (`branch_broken_compact`) | 3 | +5 | **8** |
| **Branch-Agnostic / Base** (`branch: null`) | 20 | 0 | **20** |
| **Total** | **32** | **+28** | **60** |

---

## 2. The Mercy Road (`branch_mercy_road`) — 11 Echoes

| Quest ID | Source Quest ID | Source Choice | Delay | Earliest Day | Thematic Consequence |
|---|---|:---:|:---:|:---:|---|
| `quest_moral_echo_mercy_recognized` *(baseline)* | `quest_moral_chain_mercy_05` | 0 | 20 | 75 | Reputation for sanctuary acknowledged by travelers. |
| `quest_moral_echo_mercy_tested` *(baseline)* | `quest_moral_chain_mercy_10` | 0 | 15 | 120 | Past promise to dying raider put to the test. |
| `quest_moral_echo_mercy_final` *(baseline)* | `quest_moral_chain_mercy_20` | 0 | 10 | 225 | Mediation precedent resolves valley crisis. |
| `quest_moral_echo_raider_repaid_warning` | `quest_moral_chain_mercy_16` | 0 | 25 | 190 | Prodigal raider spared earlier warns of an impending ambush. |
| `quest_moral_echo_betrayers_child_grown` | `quest_moral_chain_mercy_07` | 0 | 50 | 125 | Child taken into shelter thrives and rejects father's malice. |
| `quest_moral_echo_medicine_shared_recovered` | `quest_moral_chain_mercy_14` | 0 | 35 | 175 | Child given medicine recovers; scout bears quiet limp. |
| `quest_moral_echo_convoy_haven_opened` | `quest_moral_chain_mercy_06` | 0 | 40 | 105 | Quarantined convoy opens a valley refuge station. |
| `quest_moral_echo_plague_secret_infection` | `quest_moral_chain_mercy_12` | 0 | 30 | 150 | Concealed infection leaks to lathe hands; cost of compassion. |
| `quest_moral_echo_well_gratitude_refused` | `quest_moral_chain_mercy_08` | 0 | 30 | 115 | Beneficiary repays water debt, refusing subservience. |
| `quest_moral_echo_patrol_reputation_spread` | `quest_moral_chain_mercy_04` | 0 | 35 | 80 | Extended patrols make northern roads safe for foragers. |
| `quest_moral_echo_shelter_vote_strained_rations` | `quest_moral_chain_mercy_11` | 0 | 30 | 145 | Welcoming outsiders causes flour barrel shortages in freeze. |

---

## 3. The Iron Way (`branch_iron_way`) — 11 Echoes

| Quest ID | Source Quest ID | Source Choice | Delay | Earliest Day | Thematic Consequence |
|---|---|:---:|:---:|:---:|---|
| `quest_moral_echo_iron_feared` *(baseline)* | `quest_moral_chain_iron_05` | 0 | 20 | 30 | Ambushed supply route creates fearful compliance. |
| `quest_moral_echo_iron_challenged` *(baseline)* | `quest_moral_chain_iron_10` | 0 | 15 | 35 | Dissenter challenges iron leadership protocol. |
| `quest_moral_echo_iron_final` *(baseline)* | `quest_moral_chain_iron_20` | 0 | 10 | 54 | Hoarded grain secures valley order through winter. |
| `quest_moral_echo_aldric_blockade_retaliation` | `quest_moral_chain_iron_06` | 0 | 45 | 57 | Strangled Aldric settlement rebuilds alternate trade routes. |
| `quest_moral_echo_old_friend_farewell_note` | `quest_moral_chain_iron_13` | 2 | 30 | 57 | Turned-away old friend leaves a canteen and final note. |
| `quest_moral_echo_expulsion_deterrence_held` | `quest_moral_chain_iron_07` | 0 | 35 | 49 | Public exile of Callum prevents all operational leaks. |
| `quest_moral_echo_strike_broken_fear_quota` | `quest_moral_chain_iron_11` | 2 | 30 | 52 | Broken strike enforces boiler quota amidst sullen silence. |
| `quest_moral_echo_informant_applies_leverage` | `quest_moral_chain_iron_04` | 1 | 40 | 48 | Coerced informant adopts your methods against other camps. |
| `quest_moral_echo_lowfield_harvest_dividend` | `quest_moral_chain_iron_08` | 0 | 50 | 66 | 40% grain cut from Lowfield shields shelter from famine. |
| `quest_moral_echo_varek_blood_debt_claim` | `quest_moral_chain_iron_15` | 0 | 35 | 67 | Kin of eliminated rival demand compensation at the border. |
| `quest_moral_echo_calla_camp_empty_ruin` | `quest_moral_chain_iron_18` | 1 | 45 | 85 | Rival settlement left to freeze is found silent and empty. |

---

## 4. The Listener's Thread (`branch_listener_thread`) — 10 Echoes

| Quest ID | Source Quest ID | Source Choice | Delay | Earliest Day | Thematic Consequence |
|---|---|:---:|:---:|:---:|---|
| `quest_moral_echo_listener_confided` *(baseline)* | `quest_moral_chain_listen_05` | 0 | 20 | 30 | Listening to broadcast creates trust with settlement survivor. |
| `quest_moral_echo_listener_secret` *(baseline)* | `quest_moral_chain_listen_10` | 0 | 25 | 45 | Impartial listening yields hidden filter schematic. |
| `quest_moral_echo_listener_final` *(baseline)* | `quest_moral_chain_listen_20` | 0 | 10 | 52 | Child's drawing reveals forgotten pre-war bunker entrance. |
| `quest_moral_echo_defector_corroborates_truth` | `quest_moral_chain_listen_11` | 0 | 35 | 57 | Defector returns with valuable raider faction schism data. |
| `quest_moral_echo_cartographer_water_cache_located` | `quest_moral_chain_listen_15` | 0 | 40 | 71 | Preserved water notes reveal pure bedrock spring during drought. |
| `quest_moral_echo_prophet_calendar_discrepancy` | `quest_moral_chain_listen_09` | 0 | 30 | 48 | Storm calendar was flawed, but marked exposed coal deposit. |
| `quest_moral_echo_trader_ledger_censorship_threat` | `quest_moral_chain_listen_07` | 0 | 30 | 44 | Syndicate demands destruction of recorded debt ledger. |
| `quest_moral_echo_soldier_second_confession` | `quest_moral_chain_listen_08` | 0 | 35 | 51 | Veteran reveals true perpetrator of bridge sabotage. |
| `quest_moral_echo_doctors_notes_reinterpreted` | `quest_moral_chain_listen_04` | 0 | 45 | 53 | Epidemic re-diagnosed as radiation damage, validating notes. |
| `quest_moral_echo_librarian_memorial_preserved` | `quest_moral_chain_listen_12` | 0 | 50 | 75 | Preserved library notes train apprentice archivists after death. |

---

## 5. The Broken Compact (`branch_broken_compact`) — 8 Echoes

| Quest ID | Source Quest ID | Source Choice | Delay | Earliest Day | Thematic Consequence |
|---|---|:---:|:---:|:---:|---|
| `quest_moral_echo_betrayer_hunted` *(baseline)* | `quest_moral_chain_betray_05` | 0 | 15 | 57 | Betrayed traders form hunting parties against shelter scouts. |
| `quest_moral_echo_betrayer_cornered` *(baseline)* | `quest_moral_chain_betray_10` | 0 | 10 | 90 | Played factions realize manipulation and demand reckoning. |
| `quest_moral_echo_betrayer_final` *(baseline)* | `quest_moral_chain_betray_20` | 0 | 10 | 210 | Broken loyalty breaks internal command during crisis. |
| `quest_moral_echo_kessler_exile_uncovered` | `quest_moral_chain_betray_04` | 1 | 35 | 70 | Framed machinist survives exile and reveals who planted tools. |
| `quest_moral_echo_poisoned_gift_reputation_drop` | `quest_moral_chain_betray_06` | 1 | 30 | 80 | Compromised vials traced back; couriers demand cash upfront. |
| `quest_moral_echo_crisis_gambit_warlord_respect` | `quest_moral_chain_betray_11` | 1 | 25 | 113 | Warlord recognizes ruthless extortion and offers contract. |
| `quest_moral_echo_voss_blackmail_exposed` | `quest_moral_chain_betray_08` | 2 | 40 | 105 | Voss's blackmail journal stolen, threatening council revolt. |
| `quest_moral_echo_pell_hostage_border_locked` | `quest_moral_chain_betray_16` | 1 | 35 | 185 | Pell clan seals off southern passage permanently. |

---

## 6. Branch-Agnostic Baseline Echoes (`branch: null`) — 20 Echoes

All 20 baseline standalone echoes remain active and unmodified:
- `quest_moral_echo_child_returns`
- `quest_moral_echo_child_steals`
- `quest_moral_echo_family_defends`
- `quest_moral_echo_family_ambush`
- `quest_moral_echo_farmer_harvest`
- `quest_moral_echo_farmer_dead`
- `quest_moral_echo_raider_warning`
- `quest_moral_echo_raider_ambush`
- `quest_moral_echo_peacekeeper_intel`
- `quest_moral_echo_peacekeeper_hunted`
- `quest_moral_echo_widow_gift`
- `quest_moral_echo_prophet_map`
- `quest_moral_echo_prophet_curse`
- `quest_moral_echo_soldier_teaches`
- `quest_moral_echo_soldier_hostile`
- `quest_moral_echo_messenger_packet`
- `quest_moral_echo_messenger_stolen`
- `quest_moral_echo_dead_child_haunt`
- `quest_moral_echo_dead_child_peace`
- `quest_moral_echo_scientist_formula`
