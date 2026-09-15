# Plan 120 — Carbon composites closeout

Delivered:

- `CarbonCompositeCatalog` and `carbon_composite_catalog.json` with material,
  cure and explicit component validation.
- `CarbonCompositeEngine` with atomic material intake/output claim, freshness
  penalty, cold-storage/conformity inputs, seeded defect roll, bounded quality,
  rejection and explicit component projection.
- 5 focused Core checks and `--carbon-composite-selftest`.

Evidence: standalone selftest 5/5 and combined 60-day replay PASS. A live
workshop job owner, vehicle component adapter, composite housing adapters and
dedicated host save registration remain follow-up work; no blanket vehicle
mass/range modifier was introduced.
