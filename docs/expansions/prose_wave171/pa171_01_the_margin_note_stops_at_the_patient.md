# Subject Plan PA171-01 — The Margin Note Stops at the Patient

**Lane A · Cluster C2 · PROPOSAL.** A bounded content subject plan, not an integration plan. The 20 Lane A proposals are a serial queue; the factory permits only one Lane A plan in any future wave.

## Subject

Expand the clinical-record voice for chemical-injury cases by auditing the existing margin-note field against this case. Any sibling entry must distinguish the physician’s observation from the patient’s report and the intervention actually recorded. Use the existing record family and fields only. This plan approves no copy and changes no live data or behavior.

## Premise evidence

- **VERIFIED:** `med_02_chemical_sulfur_cornea_burn` exists in `Assets/StreamingAssets/Data/narrative/dweller_medical_casebook.json`; current `doctor_margin_note` sample: “A brave girl. She did not cry even when the saline burned her eyelids. She asked if her rifle was still dry.”.
- **VERIFIED:** v2.0 Part III Lane A / Cluster C2 and Part V’s clinical casebook contract are the relevant planning references; neither proves runtime consumption.
- **HIGH CONFIDENCE:** this anchor is absent from prior prose-wave indexes. Coverage and consumer status still need rechecking.

## Priority and boundaries

A-03/A-04: casebook and dose-treatment prose are explicit Lane A C2 openings. Preserve all patient, dose, symptom, intervention, and outcome values. Fictional clinical prose is not medical advice; add no diagnosis, treatment recommendation, or recovery claim. Preserve names, numbers, dates, attribution, and uncertainty as authored. Add no mechanic, route, save state, event, balance rule, or player-facing reachability claim. New wording stays DRAFT. **Epilogue permutations:** none.

## Recommended route (Template R)

**DATA-ONLY, conditional:** existing `narrative/dweller_medical_casebook.json` → current schema/integrity validator → verified loader and existing player surface. No parallel catalog; no save or determinism impact for static prose. No paths claimed; recheck both ledgers and claim exact ownership before any implementation. If the current field has no consumer, stop and return the proposal for owner review.

## Continuity, verification, and open premises

Before drafting, inspect the full catalog and field contract, collision-check any new ID, confirm the consumer, compare adjacent prose, and recheck the unclaimed-content census/utilization evidence. After a separately authorized data edit, run focused data-integrity and utilization checks and inspect the diff; use runtime or xUnit checks only if behavior changes. **UNKNOWN:** whether `doctor_margin_note` is currently presented; whether sibling content outranks unclaimed rows; whether more copy is warranted. Equivalent coverage or an absent route closes this proposal. **Facts used:** source object/path/field and cited v2.0 lane/contract. **New canon:** none. **Risk:** duplication or unsupported attribution.
