# Subject Plan PA171-02 — The Record of a Difficult Repair

**Lane A · Cluster C2 · PROPOSAL.** A bounded content subject plan, not an integration plan. The 20 Lane A proposals are a serial queue; the factory permits only one Lane A plan in any future wave.

## Subject

Review this existing case’s outcome prose for clear separation of presentation, intervention, and recorded result. A future short entry should remain an in-world case note rather than a how-to sequence. Use the existing record family and fields only. This plan approves no copy and changes no live data or behavior.

## Premise evidence

- **VERIFIED:** `case_b2_003_compound_fracture_mechanic` exists in `Assets/StreamingAssets/Data/narrative/surgeons_casebook_batch_2.json`; current `outcome` sample: “RECOVERED with residual stiffness. Bone healed in 6 weeks (confirmed by palpation and functional testing — no X-ray available). Full range of motion not restored — approximately 70% of normal. The Mechanic will not return to lathe work. He has transitioned to bench assembly and quality inspection. The Watchmaker's external fixator is now the standard of care for fractures in the bunker. The Surgeon notes: 'The Watchm”.
- **VERIFIED:** v2.0 Part III Lane A / Cluster C2 and Part V’s surgical casebook contract are the relevant planning references; neither proves runtime consumption.
- **HIGH CONFIDENCE:** this anchor is absent from prior prose-wave indexes. Coverage and consumer status still need rechecking.

## Priority and boundaries

Part V casebook/clinical contracts and the Lane A C2 coverage opening; live record shape supplies current fields. Retain the case’s dates, patient identity, and treatment exactly as authored. Do not add procedural instructions, prognosis, complications, or a new medical mechanic. Preserve names, numbers, dates, attribution, and uncertainty as authored. Add no mechanic, route, save state, event, balance rule, or player-facing reachability claim. New wording stays DRAFT. **Epilogue permutations:** none.

## Recommended route (Template R)

**DATA-ONLY, conditional:** existing `narrative/surgeons_casebook_batch_2.json` → current schema/integrity validator → verified loader and existing player surface. No parallel catalog; no save or determinism impact for static prose. No paths claimed; recheck both ledgers and claim exact ownership before any implementation. If the current field has no consumer, stop and return the proposal for owner review.

## Continuity, verification, and open premises

Before drafting, inspect the full catalog and field contract, collision-check any new ID, confirm the consumer, compare adjacent prose, and recheck the unclaimed-content census/utilization evidence. After a separately authorized data edit, run focused data-integrity and utilization checks and inspect the diff; use runtime or xUnit checks only if behavior changes. **UNKNOWN:** whether `outcome` is currently presented; whether sibling content outranks unclaimed rows; whether more copy is warranted. Equivalent coverage or an absent route closes this proposal. **Facts used:** source object/path/field and cited v2.0 lane/contract. **New canon:** none. **Risk:** duplication or unsupported attribution.
