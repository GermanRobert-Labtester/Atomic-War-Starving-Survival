# Expansion Reward Matrix & Anti-Exploit Balancing

## 1. Reward Philosophy

Rewards across the four charter expansions avoid inflationary power loops. All rewards adhere to:
1. **Existing Item Authority:** Using canonical IDs in `items.json` or expansion item catalogs.
2. **One-Shot Guardrails:** Every quest completion sets a completion flag and transitions stage to terminal; repeat rewards cannot be triggered.
3. **Non-Farming Policy:** Quests reward access, standing, codex unlocks, and bounded material supplies (e.g., 2–5 ration units, 1–2 filter units) rather than infinite resources.

## 2. Reward Distribution by Expansion

| Expansion | Quest / Event | Primary Material Reward | Secondary State Consequence |
|---|---|---|---|
| **Holdfast** | `quest_holdfast_salt_convoy_haul` | 10x `item_crossing_traded_salt` | Market salt price down 15% |
| | `quest_holdfast_scree_blockage_clear` | 4x `scrap_mechanical` | Sledge transit time normalized |
| | `quest_holdfast_rival_sled_overtake` | 5x `diesel_fuel` | Waystation A municipal rate unlocked |
| | `quest_holdfast_broken_runner_rescue` | 2x `medicine` | Courier survivor enrolled |
| | `quest_holdfast_census_claimant_audit`| 2x `dried_rations` | Census demographic ledger updated |
| | `quest_holdfast_brine_boiler_scum` | 3x `water_filter` | Desalination output +20 L/day |
| **Standing Record** | `quest_record_vault_breach_forensics` | 1x `item_evidence_dossier` | Verdict evidence entry + Codex unlock |
| | `quest_record_metro_derailment_triage` | 2x `copper_wire` | Platform 3 salvage tag cleared |
| | `quest_record_mine_shaft_adit_collapse`| 8x `coal` | 34 names added to Memorial |
| | `quest_record_archive_burn_layer` | 1x `item_prewar_schematic` | Codex historic directive entry |
| | `quest_record_the_unmarked_plaque` | 1x `item_salvaged_iron_rivet` | Span 44 inspect text permanently etched |
| **Crossing** | `quest_crossing_asylum_in_the_truss` | 1x `item_crossing_pledge_slip` | Garrison -5, Scale +3 standing |
| | `quest_crossing_contraband_medical_vial`| 4x `medicine` | Clinic stock replenished |
| | `quest_crossing_vehicle_lien_arbitration`| 10x `scrap_mechanical` | Cutter standing +4 |
| | `quest_crossing_flotilla_docking_rights`| 6x `dried_rations` (salt fish) | Flotilla standing +3 |
| **Verdict** | `quest_verdict_alibi_verification` | None (Procedural) | Morale +2, Tribunal Acquittal entered |
| | `quest_verdict_witness_subpoena` | 1x `item_archive_tape_spool` | Eyewitness tape enrolled in safe |
| | `quest_verdict_charter_authentication`| None (Procedural) | Certified genuine seal entered |
| | `quest_verdict_prior_verdict_appeal` | None (Procedural) | Family standing restored |
