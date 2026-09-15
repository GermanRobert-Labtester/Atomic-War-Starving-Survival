# Economy price-factor matrix

The Core `MarketSystem` remains the pricing authority. `PriceExplanation` is a
read-only decomposition of the same path used by `GetPrice`; it does not mutate
demand, ledger state, or RNG state.

| Order | Factor | Kind | Source | Operation |
|---:|---|---|---|---|
| 1 | Demand | `PriceFactorKind.Demand` | market demand entry for the good | `base × demand multiplier` |
| 2 | Floor | `PriceFactorKind.FloorClamp` | `PriceFloorFraction` (`0.25`) | lower bound when required |
| 3 | Ceiling | `PriceFactorKind.CeilingClamp` | `PriceCeilingFraction` (`4.0`) | upper bound when required |

The explanation records base price, demand multiplier, unclamped price,
intermediate values, final price, stable factor order, and transaction side.
The host exposes it through `EconomyHostSession.ExplainPrice`; the economy
detail panel translates the largest two typed deltas into concise player copy.

Stance, debt, caravan manifests, and holdfast offers are owned by their
respective trade/session authorities rather than by plain `MarketSystem`.
They must remain typed projections and must not be reimplemented in this
explanation path. No `trade_texts.json` edit is part of this stream.
