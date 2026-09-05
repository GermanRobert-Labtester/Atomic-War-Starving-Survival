# Hardcore Save Contract

## 1. Stateless Authority & Campaign Envelopes

`hardcore_economy_tuning.json` is a **static tuning data catalog**, not a mutable player state file:

- **No Save Drift:** Scarcity tiers, faction preference definitions, and price shock rules are loaded into memory on game boot via `HardcoreEconomyTuningLoader.Load`.
- **Runtime Active State:**
  - Active price shock timers are maintained transiently in memory or tracked via world event flags in `CampaignState`.
  - Upgrading the tuning data catalog does not mutate or invalidate existing player save files.
- **Backward Compatibility:**
  - Old saves created prior to Plan 99 will immediately benefit from all 8 tiers, 8 faction preferences, and 6 price shocks upon loading.
  - No database migration or save envelope version bump is required.
