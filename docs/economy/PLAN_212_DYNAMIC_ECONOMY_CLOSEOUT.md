# Plan 212 — Dynamic Local Economy & Scarcity: Closeout

**Status:** COMPLETE · **Date:** 2026-09-13 · **Batch:** `PLANS-210-213-FLAGSHIP-ECONOMY`

## Delivered

| Layer | Artifact |
|---|---|
| Core | `Assets/Ashfall.Core/Economy/MarketSystem.cs` (extended in place, v1→v2 additive — D5), `CommodityBaselineCatalog.cs`, `EconomyWeatherShockRules.cs` |
| Data | `Assets/StreamingAssets/Data/commodity_baselines.json` (12 categories — every known goods category; zero item-price duplication) |
| Host | `EconomyHostSession` commodity binding + shock event lines; weather bridge in `Main.Economy.cs` |
| Save | Additive `MarketState` v2 fields on the existing `economy` section (no new section) |
| UI | Commodity-trend strip on `EconomyMarketPanel` (arrows + words + numbers — never color-only) |
| Tests | Catalog 8/8 · engine 18/18 · wiring 6/6 · scenario C/D inside 6/6 |

## Contract guarantees

- **One price pipeline.** `MarketSystem` IS the dynamic economy; `GetPrice/ExplainPrice` gained typed Category/Shock factors between Demand and the floor/ceiling clamps. Catalog unbound → v1 path byte-identical (tested).
- **Category behavior is authored.** `commodity_baselines.json` authors elasticity class (low/medium/high), scarcity bounds, and base multipliers per category — item base prices stay in `economy_goods.json` (never duplicated).
- **Shocks are bounded and idempotent.** Shortage/crash, severity 500–3000bp, duration 1–60d, refresh-safe per (category, kind, source); deterministic expiry with exactly-once events; weather→shock mapping is Core policy (`EconomyWeatherShockRules`).
- **Trade pressure is market flow, not inventory (R7).** Buys raise the index, sells lower it; 0.75/day decay; ±3500bp cap. Owning goods elsewhere never distorts this market.
- **Inflation smoothing is bounded.** `index += 0.25 × (target − index)` clamped to authored bounds — no teleporting, no compounding (arbitrage test: 30 buy/sell cycles self-limit, ratio < 1.6, decays to baseline).
- **Save/replay safe.** v2 fields sort/clamp/dedupe on capture; v1 save → neutral; newer → throws. Scenario F: continuous == mid-reload replay.
