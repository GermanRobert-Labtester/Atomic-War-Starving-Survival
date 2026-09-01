# Expansion Flag Provenance & Lifecycle Contract

## 1. Flag Lifecycle Architecture

Every flag in ASHFALL follows a deterministic lifecycle:
```text
Producer (Quest Choice / Dialogue Action)
  ↓
Campaign State Persistence (CampaignConsequenceLedger / SaveStore)
  ↓
Consumer (Subsequent Quest Prereq / NPC Dialogue Gating / Codex Unlock)
  ↓
Terminal / Archival State
```

## 2. Plan 18 Flag Provenance Inventory

| Expansion | Flag Name | Producer Quest / Node | Consumers | Terminal Semantics |
|---|---|---|---|---|
| **Holdfast** | `flag_hf_salt_convoy_saved` | `quest_holdfast_salt_convoy_haul` | Market salt price reduction, Cutter standing +2 | Persistent civic status |
| | `flag_hf_span4_open` | `quest_holdfast_scree_blockage_clear` | Sledge transit time multiplier = 1.0x | Unlocked transit corridor |
| | `flag_hf_fuel_secured_fair` | `quest_holdfast_rival_sled_overtake` | Waystation A fuel stock discount | Economic contract |
| | `flag_hf_harek_admitted` | `quest_holdfast_census_claimant_audit` | Census roll count +1, Room 12 occupied | Demographic record |
| | `flag_hf_forger_arrested` | `quest_holdfast_census_forged_voucher` | Market voucher inflation halted | Law enforcement record |
| | `flag_hf_intake_cleansed` | `quest_holdfast_brine_intake_poisoning` | Water treatment infection chance = 0 | Environmental health |
| | `flag_hf_flue_welded` | `quest_holdfast_boiler_crack_panic` | Sub-zero heating restored | Engineering stability |
| **Standing Record** | `flag_sr_vault_mutiny_proven` | `quest_record_vault_breach_forensics` | Codex entry unlock, Verdict evidence entry | Historical truth |
| | `flag_sr_metro_sabotage` | `quest_record_metro_derailment_triage` | Transit Authority record reconciled | Site reconstruction |
| | `flag_sr_miners_memorialized` | `quest_record_mine_shaft_adit_collapse` | Memorial system count +34 | Civic honor roll |
| | `flag_sr_directive_published` | `quest_record_archive_burn_layer` | Public transparency flag | Narrative consequence |
| | `flag_sr_span44_memorial_complete`| `quest_record_the_unmarked_plaque` | Span 44 inspect text updated | Architectural memory |
| | `flag_sr_summit_beacon_lit` | `quest_record_the_last_watch_beacon` | Radio reception clarity +10% | Navigational beacon |
| **Crossing** | `flag_crossing_kael_asylum_granted`| `quest_crossing_asylum_in_the_truss` | Garrison standing -5, Scale standing +3 | Asylum sanctuary |
| | `flag_crossing_medicine_confiscated`| `quest_crossing_contraband_medical_vial`| Clinic antibiotic stock +30 | Public health stock |
| | `flag_crossing_rig_to_cutters` | `quest_crossing_vehicle_lien_arbitration`| Cutter standing +4 | Asset title transfer |
| | `flag_crossing_family_sponsored` | `quest_crossing_displaced_kin_roll` | Moral choice compassion +1 | Humanitarian record |
| | `flag_crossing_total_lockdown` | `quest_crossing_the_null_charter_vote` | Viaduct transit blocked for 10 days | Emergency lockdown |
| **Verdict** | `flag_verdict_reid_enrolled` | Auto-enrolled on Day 170 | Unlocks Tomas Reid defense dialogue | Active courtroom voice |
| | `flag_verdict_vane_enrolled` | Auto-enrolled on Day 170 | Unlocks Elena Vane cult dialogue | Active courtroom voice |
| | `flag_verdict_holt_enrolled` | Auto-enrolled on Day 170 | Unlocks Kasper Holt archival dialogue | Active courtroom voice |

## 3. Orphan Flag Elimination

All authored flags in Plan 18 are connected to active consumers or persistent narrative registries. No dangling or dead-end flags exist.
