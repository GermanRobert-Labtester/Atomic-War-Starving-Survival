# Foundry Treaty Resource Handoff

Treaty policies do not mutate shelter inventory. The live supported resource
surface is `market_modifiers[]`, applied through `MarketSystem.AdjustDemand`.
This keeps inventory, capacity, and production authority in their existing
systems.

## New policy market channels

| Treaty | Met relief | Missed / violated pressure |
|---|---|---|
| Saltworks Access | `clean_water -0.20`, brine pipe `-0.15` | `clean_water +0.35`, filter `+0.25` |
| Coal Window | `coal -0.25`, `fuel -0.15` | `coal +0.30`, `fuel +0.15` |
| Membrane Repair | brine pipe `-0.20`, `clean_water -0.15` | brine pipe `+0.35`, filter `+0.35` |
| Crisis Mutual Aid | `clean_water -0.20`, `fuel -0.20` | `clean_water +0.40`, `fuel +0.40` |
| Incident Book | no market modifier | not authored; no breach trigger |

All good IDs resolve in `economy_goods.json`; demand deltas remain bounded and
are subject to the market system's own clamps. No water-allocation, power,
inventory, or tariff field was invented for the consequence catalog.
