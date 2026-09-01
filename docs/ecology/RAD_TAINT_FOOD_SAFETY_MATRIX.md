# RAD_TAINT_FOOD_SAFETY_MATRIX.md — Plan 28 Task 28I

**Verdict: DEFERRED — with an exact integration path. No second contamination model was built.**

## Why deferred

Rule 1.8 / decision rule #14: taint may only ride an existing contamination authority.
Repository reality:

- Food-item contamination lives with inventory/food-safety state (per-item, host-owned).
- `LocationEvolutionRecord.contaminationLevel` exists **per location**, not per sector;
  the wildlife runtime moves between **sectors**. No sector↔contamination authority exists.
- Wildlife harvest (trapping) already carries a per-catch toxin roll (`TrapSite.isToxic`,
  `RemoveToxin`, bait `toxicReduction`) — the food-safety consumer is live.

Attaching taint to packs today would require inventing a sector-contamination model —
precisely the "hidden arbitrary taint" §1.8 forbids.

## The traceable design (filed for the follow-up task)

1. **Producer:** extend `LocationSeedRecord.contamination` semantics with a sector mapping
   (each sector names its representative locations), OR add optional `sector_contamination`
   to the seeds catalog — one authority, validated like other seeds.
2. **Accumulation rule (deterministic):** while a pack holds a sector whose representative
   contamination > threshold, accumulate `taintLevel += exposure * days` in
   `WildlifePackRecord` (new optional field, default 0 → legacy saves safe).
3. **Consumer:** trapping catch rolls taint from the **caught pack's** taintLevel into the
   existing `isToxic` path — no new poisoning system; medical/food-safety unchanged.
4. **Inspection:** `RemoveToxin` already exists as the processing hook.

## Matrix (contract for the deferred implementation)

| Route exposure | Harvest state | Player-facing trace | Consumer |
|---|---|---|---|
| Clean sector (rep. contamination < 0.1) | clean catch | geiger on the carcass reads low | existing food safety |
| Moderate (0.1–0.4) | taint rolls with existing `toxicChance` | field-guide note; RemoveToxin works | existing |
| Heavy (> 0.4, e.g. diesel tank farm country) | high taint; catch flagged toxic more often | scout notes + bestiary | existing |

**Acceptance for un-deferring:** taint derivable from world exposure, consumed by the
existing toxin/medical path, one new save field maximum, tests for clean vs. contaminated
routes. Until then: **no taint coupling ships.**
