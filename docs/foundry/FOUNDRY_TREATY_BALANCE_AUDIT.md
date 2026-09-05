# Foundry Treaty Consequence Balance & Severity Audit

**Catalog Authority:** `Assets/StreamingAssets/Data/foundry_treaty_consequences.json`
**Test Authority:** `Ashfall.Core.Tests/FoundryTreatyConsequenceExpansionTests.cs`

---

## 1. Severity Bands

The 15 consequence policies adhere to strictly bounded mathematical bands:

| Outcome | Standing Delta Range | Average Delta | Market Demand Delta Range | Typical Market Impact |
|---|---|---|---|---|
| `met` | `+2.0` to `+4.0` | `+3.0` | `-0.15` to `-0.40` | Price relief, surplus clearance, stable transit |
| `missed` | `-5.0` to `-6.0` | `-5.67` | `+0.15` to `+0.40` | Minor scarcity, idle fuel burn, backlog |
| `violated` | `-8.0` to `-12.0` | `-10.0` | `+0.25` to `+0.80` | Severe scarcity, road closure, filter fouling |

---

## 2. Policy-by-Policy Economic Audit

| Policy Index | Treaty ID | Outcome | Standing Delta | Good | Demand Delta | Base Good Price | Scaled Price Impact |
|---|---|---|---|---|---|---|---|
| 1 | `brine_pipe` | `met` | `+2.0` | `brine_pipe`<br>`coal` | `-0.40`<br>`-0.15` | 70.0<br>12.0 | $42.0$<br>$10.2$ |
| 2 | `brine_pipe` | `missed` | `-6.0` | `brine_pipe`<br>`coal` | `+0.40`<br>`+0.15` | 70.0<br>12.0 | $98.0$<br>$13.8$ |
| 3 | `cluster_labour` | `met` | `+2.0` | `fuel` | `-0.25` | 20.0 | $15.0$ |
| 4 | `cluster_labour` | `violated` | `-8.0` | `fuel` | `+0.25` | 20.0 | $25.0$ |
| 5 | `road_iron` | `met` | `+3.0` | `coal`<br>`ice_anchor` | `-0.20`<br>`-0.30` | 12.0<br>2.0 | $9.6$<br>$1.4$ |
| 6 | `road_iron` | `missed` | `-6.0` | `coal`<br>`ice_anchor` | `+0.20`<br>`+0.30` | 12.0<br>2.0 | $14.4$<br>$2.6$ |
| 7 | `saline_corridor` | `met` | `+3.0` | `fuel`<br>`clean_water` | `-0.20`<br>`-0.25` | 20.0<br>8.0 | $16.0$<br>$6.0$ |
| 8 | `saline_corridor` | `missed` | `-5.0` | `fuel` | `+0.35` | 20.0 | $27.0$ |
| 9 | `switchback_fuel` | `met` | `+4.0` | `fuel` | `-0.30` | 20.0 | $14.0$ |
| 10 | `switchback_fuel` | `violated` | `-10.0` | `fuel` | `+0.50` | 20.0 | $30.0$ |
| 11 | `deep_coast_aquifer`| `met` | `+3.0` | `clean_water` | `-0.35` | 8.0 | $5.2$ |
| 12 | `deep_coast_aquifer`| `violated` | `-10.0` | `clean_water`<br>`water_filter` | `+0.80`<br>`+0.50` | 8.0<br>35.0 | $14.4$<br>$52.5$ |
| 13 | `grain_tithe` | `met` | `+4.0` | `canned_food` | `-0.30` | 16.0 | $11.2$ |
| 14 | `grain_tithe` | `violated` | `-12.0` | `canned_food`<br>`fuel` | `+0.60`<br>`+0.40` | 16.0<br>20.0 | $25.6$<br>$28.0$ |
| 15 | `fair_trade` | `met` | `+3.0` | `scrap_metal` | `-0.15` | 3.0 | $2.55$ |

All market adjustments remain bounded within the engine limits $[0.5, 3.0]$, preventing game-breaking hyperinflation or zero-cost duplication.
