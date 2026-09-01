# Forensic Evidence Chain & Non-Natural Death Cases

This document details the three authored forensic autopsy cases where dissection produces evidence records for the Verdict and Cold-Case systems.

---

## Forensic Case 1: The Kitchen Poisoning (`case_forensic_poisoning`)
1. **Scenario Producer:** An unannounced sudden collapse of a shelter guard or quartermaster after eating morning stew.
2. **Initial Death Record:** Recorded by default as "sudden heart failure / natural collapse".
3. **Procedure Executed:** `procedure_poison_biochemical_assay` with `field_surgical_kit` and `protective_rubber_gloves`.
4. **Resolved Finding:** `finding_organophosphate_toxin` (unambiguous pesticide concentration in liver/stomach tissue).
5. **Evidence Produced:** `evidence_kitchen_pesticide_traces` -> Filed into Evidence Ledger.
6. **Downstream Consumer:** Verdict interrogation questline; unlocks motive inquiry without accusing innocent random NPCs.

---

## Forensic Case 2: The Staged Cave-In (`case_forensic_staged_accident`)
1. **Scenario Producer:** Scavenger found crushed in a collapsed mining tunnel.
2. **Initial Death Record:** "Crushed by falling reinforced concrete beam".
3. **Procedure Executed:** `procedure_blunt_trauma` followed by `procedure_ballistic_forensics`.
4. **Resolved Finding:** Skull fracture occurred *prior* to dust inhalation (no ash in bronchi), accompanied by `finding_crush_fracture` inconsistency and defensive forearm trauma.
5. **Evidence Produced:** `evidence_pre_collapse_bludgeoning` -> Filed into Evidence Ledger.
6. **Downstream Consumer:** Shelter investigation quest exposing saboteur or claim jumper.

---

## Forensic Case 3: Concealed Asphyxiation (`case_forensic_asphyxiation`)
1. **Scenario Producer:** Sentry found dead in their bunk during night watch.
2. **Initial Death Record:** "Hypothermia / sleep cessation".
3. **Procedure Executed:** `procedure_respiratory_contamination` and trauma inspection.
4. **Resolved Finding:** Petechial hemorrhages in conjunctiva and airway bruising inconsistent with gradual freezing.
5. **Evidence Produced:** `evidence_smothering_airway_trauma` -> Filed into Evidence Ledger.
6. **Downstream Consumer:** Infiltrator / spy questline.
