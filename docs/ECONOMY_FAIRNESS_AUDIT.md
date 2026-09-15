# Economy fairness audit

The economy pass is explainability-first. It does not rewrite the dynamic
pricing model or take ownership of the volatile trade-text stream.

## Evidence

- `MarketSystem.GetPrice(item)` and `MarketSystem.ExplainPrice(item)` share one
  authoritative quote path;
- explanation final price equals the player quote;
- factor ordering is ordinal/stable and the explanation is side-effect-free;
- floor/ceiling constraints are represented explicitly rather than hidden;
- existing `TradeScreenScenarioCatalogTests` cover cross-scenario price
  consistency and anti-arbitrage spread bands;
- existing `CaravanTradeNetworkTests` cover caravan buy/sell quote behavior;
- existing trade presenter tests cover stance/fairness gating and preview
  projection;
- `--economy-selftest` remains green at `11/11`.

The current market quote itself has demand plus clamps. Debt and counterpart
stance are not silently fabricated in the plain market panel; their owning
trade sessions continue to expose those restrictions. A future combined
caravan/holdfast preview change should reuse those authority surfaces and add
typed stale/insufficient-value results rather than UI-side arithmetic.

No fee/spread data was tuned in this pass. A guaranteed-profit finding must be
reproduced over the existing fixed-seed trade matrix before any data-only tune.
