# Plan 56 — Economy Goods Expansion: Final Report & Matrices

> `economy_goods.json` expanded from a verified **34-good baseline to 40 goods**.
> The plan's stated baseline of 16 was stale (Case B): foundry goods, chelation
> pellets, traps, and filters had already landed via other plans. All 34 baseline
> goods are preserved byte-for-byte; 6 new canonical goods complete the 40 target.

## Schema contract (Task 56A — from `GoodsCatalogLoader`, repository truth)

| Field | Type | Constraints | Default |
|---|---|---|---|
| `id` | string | snake_case, unique, non-empty | required |
| `displayName` | string | non-empty | required |
| `category` | enum | one of: food, water, medical, fuel, weapons, tools, materials, ammo, documents, luxury, misc, contraband | required |
| `basePrice` | float | > 0, finite | required |
| `volatility` | float | [0, 1] | 0.1 |
| `elasticity` | float | > 0, finite | 1.0 |
| `stackSize` | int | >= 1 | 10 |
| `weightKg` | float | >= 0, finite | 1.0 |
| `barterNote` | string | optional flavor | "" |

Envelope: `{ schema_version: 1, goods: [...] }`; schema_version > 1 rejected.

## Runtime semantics (verified in `MarketSystem.cs`)

- **volatility** = daily uniform noise amplitude: `noise = U(-volatility, +volatility)`
  applied as a delta to the demand multiplier — NOT a price percentage.
- **elasticity** = scales that noise delta (`delta = noise × elasticity`) — in this
  runtime, higher elasticity means the *demand multiplier walks further per day*,
  i.e. prices are more sensitive/labile. Values authored accordingly.
- Price = `basePrice × demandMult`, demandMult clamped [0.25, 4.0] (Unity parity),
  price clamped to `[0.25 × basePrice, 4 × basePrice]` (floor/ceiling).
- All stochasticity flows through the caller's `ISeededRng` → fully deterministic.
- The loader does **not** validate item references: 5 legacy goods (`bandages`,
  `9mm_ammo`, `crowbar`, `diamond`, `coal`) are market-projection ids that predate
  the canonical-item convention and are grandfathered. Plan-56 goods follow the
  modern convention: good id = canonical `items.json` id (like the foundry/trap goods).

## The 6 new goods (Task 56C–56N selection from the 24-concept roster)

| id | category | basePrice | volatility | elasticity | stack | weightKg | Rationale |
|---|---|---|---|---|---|---|---|
| `ammo_556` | ammo | 18 | 0.22 | 1.2 | 20 | 0.2 | Live caliber (2 weapons); scarcer than 9mm (12) — war-sensitive volatility |
| `ammo_12g` | ammo | 10 | 0.20 | 1.2 | 20 | 0.25 | Third ammo family; cheaper than 9mm, distinct stack/weight |
| `diesel_fuel` | fuel | 16 | 0.16 | 1.3 | 4 | 2.5 | Mechanically distinct from generic `fuel` (rail/barge industrial); sits between coal (12) and fuel (20) |
| `item_smoked_meat` | food | 20 | 0.16 | 1.4 | 8 | 0.3 | Preserved protein (Plan-55 production chain); more stable than cooked_meat (0.20) |
| `item_pickled_tubers` | food | 11 | 0.12 | 1.3 | 10 | 0.4 | Preserved vegetables; stable staple, salt-camp economy tie |
| `tobacco_pouch` | luxury | 30 | 0.35 | 1.8 | 10 | 0.2 | The high-volatility/high-elasticity discretionary contrast to diamond (0.05/0.3) — plan's core theme |

Roster reconciliation: antibiotics/iodine/chelation/scrap/coal/9mm/canned_food were
**already covered** by the 34 baseline goods. Concepts dropped: gasoline (not
mechanically distinct from existing fuels), timber/concrete/steel (substituted by
existing mechanical_parts/scrap/brine_pipe coverage), wrench/saw (crowbar + tools
coverage), maps (collectibles with codex unlocks — non-fungible per Task 56AI
guardrail), spirits (distiller station unreachable; tobacco chosen as the luxury).

## Price bands (Task 56F)

Final 40 range: 2.0 (`item_foundry_ice_anchor`) → 150 (`diamond`). New goods slot
into existing bands: ammo mid-low (10–18), diesel mid-fuel (16), preserved food
mid (11–20), tobacco mid-luxury (30, below premium tools/medical, far below diamond).

## Volatility distribution (Task 56AP)

Catalog max was 0.28 (`anti_rad`); tobacco_pouch 0.35 is the single intentional
outlier (pinned by test). Preserved foods at 0.12–0.16 sit below fresh cooked_meat
(0.20). Diesel 0.16 between fuel (0.18) and coal (0.15). No cluster at maximum.

## Elasticity distribution (Task 56AQ)

Necessity anchors unchanged (diamond 0.3, solar_cell 0.5, crowbar 0.6). New goods:
ammo 1.2 (like 9mm), diesel 1.3 (vehicle demand), preserved food 1.3–1.4, tobacco
**1.8 — the most elastic good in the catalog** (discretionary luxury).

## Weight/value & stack (Task 56AR/56AS)

value/kg highlights: tobacco_pouch 150/kg, item_smoked_meat 66.7/kg, ammo_556
90/kg — premium light cargo. diesel_fuel 6.4/kg and item_pickled_tubers 27.5/kg
remain bulk-constrained. Stacks match trade semantics (ammo 20, diesel 4 like fuel,
food 8–10, tobacco 10).

## Settlement wiring (Tasks 56AT–56AV) — 6/6 new goods wired

| Settlement | Region | New export (trade_goods) | New import (trade_needs) |
|---|---|---|---|
| `settlement_fort_karkov` | high_scarp | `ammo_556` (garrison stock) | `item_smoked_meat` (feeding the garrison) |
| `settlement_lock_seven` | the_toll | `ammo_12g` (stronghold defense) | `item_pickled_tubers` (food variety) |
| `settlement_iron_siding` | industrial_belt | `diesel_fuel` (rail diesels) | `tobacco_pouch` (paid rail crews) |
| `settlement_brine_pans` | the_toll | `item_pickled_tubers` (salt-cured produce identity) | — |
| `settlement_ferry_crossing` | the_drown | `item_smoked_meat` (hunting trade post) | `diesel_fuel` (river trade) |
| `settlement_tinkers_notch` | dead_suburbs | `tobacco_pouch` (scrap-market vice trade) | — |

Specialization verified: no settlement exports its own needs (pinned by test);
industrial_belt exports fuel, the_toll exports preserved goods, high_scarp
strongholds export munitions, the_drown exports protein.

## Caravan wiring (Tasks 56AW–56AX) — 5/6 new goods on existing routes

| Caravan | New specialty goods | Fit |
|---|---|---|
| `caravan_flotilla_salt_run` | `diesel_fuel` (pre-existing) | barge fuel |
| `caravan_verge_grain_convoy` | `item_pickled_tubers` | food convoy |
| `caravan_foundry_coal_iron` | `ammo_556` | industrial-belt munitions (alongside ammo_308) |
| `caravan_free_trader_circuit` | `tobacco_pouch`, `item_smoked_meat` | varied trader cargo |

`ammo_12g` is settlement-only (stronghold trade) — intentional scarcity.

## Arbitrage audit (Tasks 56BP–56BU)

- **Cross-settlement:** new goods follow existing margins; tobacco (high volatility)
  creates episodic — not permanent — opportunities. No good is both cheaply bought
  and structurally overpriced at any single settlement.
- **Craft-to-sell:** `item_smoked_meat`/`item_pickled_tubers` are Plan-55 craft
  outputs; their economy basePrices (20/11) are consistent with their item
  tradeValues (18/10) — no new arbitrage beyond the item-level margins already
  audited in Plan 55 (which are labor/fuel-bounded).
- **Debt/cargo/stack:** no new debt templates or cargo semantics; weights are
  per-unit consistent with item mass; diesel at 2.5 kg/unit stays bulk-constrained.

## Save contract (Tasks 56BV–56BW)

`MarketState.demand` is a list of `{itemId, multiplier}` entries keyed by id, not
position — adding goods requires **no migration**. Existing 16→34 baseline demand
entries are untouched; new goods default to demand multiplier 1.0 until first tick
(verified: `GetDemandMultiplier` returns 1f for absent entries).

## Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # PASS 0/0
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # PASS 6,538/6,538 (15 new Plan-56)
dotnet build Ashfall.csproj                                 # PASS 0 errors
godot --headless -- --data-integrity-selftest               # PASS 0 findings / 208 catalogs
godot --headless -- --content-utilization-selftest          # PASS
godot --headless -- --bridge-selftest                       # PASS exit 0
```

## Category distribution — final 40

- **ammo**: 3
- **food**: 5
- **fuel**: 3
- **luxury**: 2
- **materials**: 9
- **medical**: 7
- **tools**: 10
- **water**: 1

## Deferred

- Gasoline as a distinct fuel (needs mechanical differentiation first).
- Information/documents goods (maps are codex-locked collectibles; a fungible
  intel good needs a non-unique document item first).
- Spirits as a second luxury (distiller station reachability — see Plan 55 report).
- Deeper settlement specialization and additional caravan routes (§16).
