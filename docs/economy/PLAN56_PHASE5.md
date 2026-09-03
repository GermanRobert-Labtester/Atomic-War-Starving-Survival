# Plan 56 Phase 5 — Provenance-Aware Trade Pricing

> Wires `RegionalSupplyRouter.ShortageDemandScale` into the live scarcity
> path, with seeded before/after price traces and a long-horizon arbitrage
> re-audit.

## 1. The world-level scale

`RegionalSupplyRouter.WorldShortageDemandScale(catalog, goodId, activeOriginRegions)` —
the best relief across every **active caravan-serviced region** (the
`origin_region` values in `caravans.json`):

| Condition | Scale |
|---|---|
| Any serviced region produces the good (supply line exists) | **0.5×** — the convoy line resupplies it |
| `general` supply | **1.0×** — tracks the market |
| No serviced region produces it (structural scarcity) | **1.5×** |
| Unknown/unannotated good | **1.0×** (legacy-neutral) |

With the live caravan set (`deep_coast` / `ash_flats` / `industrial_belt` /
`settlement`): greenhouse- and foundry-pool goods are buffered, traplines-pool
goods escalate, general goods track.

## 2. Call-site wiring

`Main.CampaignOwners` (the wildlife-pressure scarcity owner): the per-good
`AdjustDemand` delta is now
`delta × WorldShortageDemandScale(catalog, good, activeOrigins)` where the
active origin regions are loaded from the caravan data authority
(`CaravanCatalogLoader.Load(_dataDir)`, distinct `origin_region`s).

**Behavior change on live data:** the one authored scarcity good
(`canned_food`, annotated `greenhouse` in the phase-5 data pass) is now
buffered — the ash-flats greenhouse region + the grain convoy resupply
preserved food, so its demand escalates at half rate. Every future
`scarcity_goods` entry gets provenance pricing automatically.

## 3. Seeded before/after traces (verified)

Both runs share an identical seeded walk (same catalog → same per-good draw
order in `TickDay`), so the runs differ **exactly** by the nudge
differential:

```
20 days × +0.02 base delta, canned_food (greenhouse, buffered):
  unscaled run: demand multiplier +0.40 (raw delta)
  scaled run:   demand multiplier +0.20 (0.5× provenance scale)
  beforeDemand − afterDemand = 0.20  (asserted exactly, 3 decimals)
```

Deterministic across replays (prices for canned_food / duct_tape /
water_filter / tobacco_pouch identical to 5 decimals).

## 4. Arbitrage re-audit (120-day horizon)

Seeded run with rotating scarcity pressure across all three scale classes
(greenhouse / foundry / traplines / general):

- Every good within its clamp band `[0.25 × base, 4 × base]` after the full
  horizon; zero NaN/negative prices.
- Pressured goods end at/above their floor (pressure visible), nothing
  collapses.
- Replay: identical final prices per scale class (dictionary equality).
- No permanent dominant good: the clamp band is structural (16:1 max
  spread per good); the provenance scale only shifts *how fast* each class
  responds, not where the bands sit.

## 5. Gates

```
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # 6600 + 7 = all green
dotnet build Ashfall.csproj                                 # 0 errors
--data-integrity-selftest / --bridge-selftest               # PASS
--economy-selftest / --caravan-selftest                     # PASS
```
