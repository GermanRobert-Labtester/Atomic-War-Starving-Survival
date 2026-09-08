# Final Wish NPC Arc Handoff Integration

**Document:** `docs/survivors/FINAL_WISH_NPC_HANDOFF.md`

---

## 1. Durable NPC Arc Connections (6 Wired Wishes)

Six wishes connect dying survivors with canonical NPCs from `npc_arcs.json`, creating permanent narrative legacy and relationship flags:

| Wish # | Archetype | Target NPC | Relationship Dynamic | Durable Impact |
|---|---|---|---|---|
| **13** | `the_reporter` | `npc_mara_veln` | Verification of convoy casualties | Mara gains verified transit casualty log; unlocks trusted trade rates |
| **17** | `the_prisoner` | `npc_marek_voln` | Discharge of old pre-war grievance | Discharges friction debt; clears prison-block grudge flag |
| **18** | `the_defector` | `npc_ilze_kaar` | Palliative confession of Day 14 omission | Relieves mutual guilt; clears defection suspicion in clinic |
| **26** | `the_fierce_mother` | `npc_lina` | Guardianship handover for orphan | Assigns Lina to permanent warm quarters; establishes protector status |
| **27** | `the_martyr` | `npc_niko` | Protection of vulnerable survivor | Enforces daily ration and rest-shift monitoring for Niko |
| **30** | `the_quartermaster` | `npc_oskar_ruut` | Logistics key & ledger handover | Hands over keys to Oskar; succession recorded in shelter administration |

---

## 2. Target Absence Guardrail

If a target NPC is absent or deceased prior to wish trigger, host session handlers degrade the interaction to a bedside remembrance or memorial record without hanging the state machine.
