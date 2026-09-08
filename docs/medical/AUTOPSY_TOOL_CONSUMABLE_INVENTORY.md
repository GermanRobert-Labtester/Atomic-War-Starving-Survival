# Autopsy Tool & Consumable Inventory

## Critical pre-existing defect found and fixed

Baseline audit found that **`medical_scissors`, `protective_rubber_gloves`, and `sterilised_bandage` existed in no item catalog** — only as references inside `autopsy_procedures.json`. Every one of the 9 baseline procedures required at least one phantom, so `QueueAutopsy` always returned `Blocked("missing_tool")` and **the autopsy system was unplayable**.

Per the plan's §12 exception (clear data bug), the fix is pure data: the three items were added to `items.json` as ordinary, schema-valid entries (plan §9 "good candidate" class — ordinary medical tools/consumables, no new mechanics):

| Item | Type | Schema notes |
|---|---|---|
| `medical_scissors` | Component | stainless bandage shears; stack 3, weight 0.15, trade 9 |
| `protective_rubber_gloves` | Medical | long-cuff exam gloves; stack 6, weight 0.1, trade 5 |
| `sterilised_bandage` | Medical | sterile pressure dressing; stack 10, healthEffect 30, trade 12 (parallel to existing `bandage`) |

No autopsy-table fiction (§79C.3 — no workstation modeled as a portable item). No new tools beyond the three missing IDs.

## Item vocabulary (final)

**Tools** (3, all now real): `medical_scissors`, `field_surgical_kit`, `surgical_mask`, plus `scalpel` (forensic only) and `protective_rubber_gloves` (PPE) — 5 core IDs reused across all 12 procedures; no one-off tools.

**Consumables** (4): `bandage`, `sterilised_bandage`, `clean_water`, `antibiotics`.

## Antibiotics note (§2.6)

`antibiotics` is consumed by `procedure_containment_autopsy` (pre-existing) and the new `procedure_forensic_unknown` (justified: broad-opening a corpse of unknown cause is the one scenario with justified contamination-control use beyond the containment protocol). All low-risk procedures use only dressings/water.

## Final tool/consumable profile matrix

| Procedure | Tools | Consumables | Cost tier |
|---|---|---|---|
| hypothermia | scissors, gloves | water | light |
| **deprivation (new)** | scissors, gloves, kit | water, sterile dressing | light |
| blunt | scissors, kit | bandage, water | light |
| toxicology | scissors, gloves | bandage, water | light |
| ballistic | scissors, gloves, kit | bandage, water | light-mid |
| **blast (new)** | scissors, gloves, kit | bandage, sterile dressing | mid |
| rad pathology | scissors, gloves, kit | sterile dressing, water | mid |
| respiratory | scissors, gloves, mask | water, sterile dressing | mid |
| poison assay | gloves, kit | water, sterile dressing | mid |
| **forensic (new)** | scissors, gloves, kit, scalpel | sterile dressing, water, antibiotics | heavy |
| containment | scissors, gloves, kit, mask | sterile dressing, water, antibiotics | heavy |
| spore isolation | scissors, gloves, kit, mask | sterile dressing, water, antibiotics | heavy |

Preparation requirements differentiate procedures (mask/kit/scalpel gates, consumable weight) without one-off item tax.
