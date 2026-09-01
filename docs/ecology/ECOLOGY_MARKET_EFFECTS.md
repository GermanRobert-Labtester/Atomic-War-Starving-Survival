# ECOLOGY_MARKET_EFFECTS.md — Plan 28 Tasks 28AE/28AF

**Status: the scarcity direction is LIVE; the abundance direction is live via the same clamp.**

## Live chain (verified by `--evolving-world-selftest`)

```
WildlifeMigrationSystem.GetGlobalPopulationRatio()
  ↓ EvolvingWorldDayOwner.TickDay (daily)
ratio < 0.6  → demand +0.02/day on scarcity_goods (canned_food)
ratio < 0.85 → +0.005
ratio > 1.2  → −0.005 (abundance eases preserved-food demand)
  ↓ MarketSystem.AdjustDemand — the market clamps demand internally (bounded authority)
```

| Ecological event | Market effect | Bound | Reset |
|---|---|---|---|
| Herd/fish collapse (ratio < 0.6) | preserved-protein demand rises | +0.02/day while collapsed | reverses at ratio > 0.85 (demand decays via −0.005 and market clamp) |
| Fish run / herd boom (ratio > 1.2) | −0.005/day | tiny, demand-floor clamped | auto |
| Blight/grain failure | **deferred** — grain price coupling waits on Plan 22 crop-harvest → market goods mapping | — | — |
| Hive/honey loss | **deferred** — no honey/wax goods chain yet (content pass) | — | — |

## Anti-arbitrage rules (28BD/28AF)

- Modifiers are **daily deltas with demand clamps**, not price writes — the market keeps
  one pricing authority; ecology only nudges demand.
- Deltas are asymmetric and small; repeated events cannot stack past the clamp.
- Abundance windows are temporary by construction (seasonal factors); storage/spoilage
  remains the realistic brake on banking a run's yield.
- No player action reveals exact regional prices pre-arrival; trade opportunity stays
  informational (radio/field-guide), not a guaranteed loop.
