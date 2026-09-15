# Plan 118 — Fischer–Tropsch closeout

Delivered:

- `FischerTropschCatalog` and `fischer_tropsch_catalog.json` with schema,
  duplicate, reference and bounds validation.
- `FischerTropschSynthesisEngine` with atomic feed/output inventory actions,
  catalyst state, bounded operating modifiers, deterministic process
  variation, explicit lubricant consumers and capture/restore including seeded
  RNG state.
- 7 focused Core checks and `--synthetic-lubricant-selftest`.

Evidence: standalone selftest 7/7; the combined Plans 118–121 60-day replay
passed production, bounds, same-seed, different-seed and midpoint save/replay
checks. Host UI, live shelter tick registration and a dedicated save-store
entry remain follow-up work because those owners were absent from the authority
map.
