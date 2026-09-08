# Foundry Treaty Consequence Content Utilization

**Authority:** `Assets/StreamingAssets/Data/foundry_treaty_consequences.json`

## Final catalog

- 15 policy rows load through `SilentFoundryConsequencePolicyCatalog`.
- 15 unique `(treaty_id, outcome)` keys.
- 15 treaty IDs resolve in `foundry_accords.json`.
- 15 faction IDs are verified signatories; all are
  `faction_silent_foundry`, the current affected-faction authority.
- All market modifier good IDs resolve in `economy_goods.json`.
- Eight of ten Foundry accords have at least one policy row.
- Apprentice Exchange and Cluster Charter are explicitly staged because no
  typed outcome trigger exists for them.

The nine new rows are not dead catalog entries: they are indexed by the live
policy lookup and validated by the headless smoke test. Their assessment
triggers remain a documented downstream seam rather than an invented runtime.

## Gate evidence

The Plan 103-specific expansion suite is active and covers exact count,
references, outcomes, supported goods, uniqueness, coverage, representative
new rows, idempotency, save compatibility, and balance bounds. The full
content-utilization self-test remains the repository-wide authority for
orphaned catalogs; this document does not claim that an authored policy can
be triggered when its owning outcome system is not yet present.
