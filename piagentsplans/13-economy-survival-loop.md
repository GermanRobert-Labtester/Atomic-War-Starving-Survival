# Plan 13 — Economy & Survival Loop: Trade Flow, Trapping & Crisis Weather

## Goal

Connect the now-expanded goods and recipe catalogs to live trade flow, active trapping, and
weather-driven crises. This plan does not add goods or recipes.

## Scope boundary

- Plan 55 owns recipes.json expansion.
- Plan 56 owns economy_goods.json expansion.
- This plan owns the consequence layer that consumes those catalogs: station-to-market flow,
  caravan demand, trapping choices, and weather crises. It must not add catalog rows or a second
  price-tuning authority.

## Task 13A — Crafting-to-market flow and caravan demand

1. Use existing recipe outputs and goods to model station availability, regional demand, and caravan
   requests through MarketSystem and TravelingCaravanSystem.
2. Define visible shortages, surplus opportunities, and fulfilled-contract outcomes without
   changing base prices or adding new items.
3. Test that catalog references resolve and that supply/demand effects use the existing economy
   tuning authority.

## Task 13B — Active trapping, hunting, and butchery loop

1. Expose trap-line placement, bait selection, collection timing, and butchery choices through the
   existing trapping and skill systems.
2. Use existing food safety, radiation, and inventory authorities for yields and contamination.
3. Test deterministic yields, maintenance decisions, and balance against rations/greenhouse food.

## Task 13C — Weather-specific crisis events

1. Give every supported WeatherKind a small number of reachable, data-driven crises.
2. Route outcomes through the existing power, shelter, medical, and economy systems.
3. Validate weather keys, event reachability, save/load, and deterministic resolution.

## Definition of Done

- Plans 55 and 56 remain the sole recipe/goods catalog owners.
- Station output visibly affects market and caravan decisions without a second pricing model.
- Trapping and weather crises create tested daily survival trade-offs.
