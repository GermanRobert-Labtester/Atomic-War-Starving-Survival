# Plan 121 — GPR cartography closeout

Delivered:

- `GroundPenetratingRadarCatalog` and `gpr_exploration_catalog.json` with
  schema, duplicate-ID, bounds and meaningful mode-tradeoff validation.
- `GroundPenetratingRadarEngine` with atomic survey power, terrain/mode
  attenuation, seeded noise, bounded confidence/depth, repeat-scan evidence,
  idempotent leads and capture/restore.
- 5 focused Core checks and `--gpr-cartography-selftest`.

Evidence: standalone selftest 5/5 and combined 60-day replay PASS. Live map,
excavation and expedition adapters remain deferred to their existing owners;
the engine does not identify exact ordnance, grant loot or eliminate residual
excavation risk.
