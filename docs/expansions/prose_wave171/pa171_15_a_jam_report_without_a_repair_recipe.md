# Subject Plan PA171-15 — A Jam Report without a Repair Recipe

**Lane A · Cluster C15 · PROPOSAL.** A bounded content subject plan, not an integration plan. The 20 Lane A proposals are a serial queue; the factory permits only one Lane A plan in any future wave.

## Subject

Develop a short, non-procedural report-writing brief for equipment faults: identify the observed failure, retain the recorded classification, and end at the report’s evidence boundary. This is setting text, not an operations guide. Use the existing record family and fields only. This plan approves no copy and changes no live data or behavior.

## Premise evidence

- **VERIFIED:** `hoist_jam_pneumatic_rammer_misfeed` exists in `Assets/StreamingAssets/Data/narrative/ammo_hoist_jam_reports.json`; current `prose` sample: “Pneumatic rammer piston pressure dropped from 150 to 80 PSI during sustained barrage. A 76mm brass casing hung up on the breech guide lip; the advancing rammer crushed the case into an accordion fold, spilling five pounds of cordite granules into the lower turret ring.”.
- **VERIFIED:** v2.0 Part III Lane A / Cluster C15 and Part V’s defense equipment fault report contract are the relevant planning references; neither proves runtime consumption.
- **HIGH CONFIDENCE:** this anchor is absent from prior prose-wave indexes. Coverage and consumer status still need rechecking.

## Priority and boundaries

Part III Lane A C15 permits defense-log prose; Part V maintenance-glitch contract sets the procedural voice. Do not add weapon capability, operational instructions, casualties, a repair outcome, or a combat-system effect. Preserve names, numbers, dates, attribution, and uncertainty as authored. Add no mechanic, route, save state, event, balance rule, or player-facing reachability claim. New wording stays DRAFT. **Epilogue permutations:** none.

## Recommended route (Template R)

**DATA-ONLY, conditional:** existing `narrative/ammo_hoist_jam_reports.json` → current schema/integrity validator → verified loader and existing player surface. No parallel catalog; no save or determinism impact for static prose. No paths claimed; recheck both ledgers and claim exact ownership before any implementation. If the current field has no consumer, stop and return the proposal for owner review.

## Continuity, verification, and open premises

Before drafting, inspect the full catalog and field contract, collision-check any new ID, confirm the consumer, compare adjacent prose, and recheck the unclaimed-content census/utilization evidence. After a separately authorized data edit, run focused data-integrity and utilization checks and inspect the diff; use runtime or xUnit checks only if behavior changes. **UNKNOWN:** whether `prose` is currently presented; whether sibling content outranks unclaimed rows; whether more copy is warranted. Equivalent coverage or an absent route closes this proposal. **Facts used:** source object/path/field and cited v2.0 lane/contract. **New canon:** none. **Risk:** duplication or unsupported attribution.
