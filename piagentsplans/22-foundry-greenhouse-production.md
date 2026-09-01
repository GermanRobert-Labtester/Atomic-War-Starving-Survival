# Plan 22 — Production Decisions: Foundry, Food Preservation & Labor

## Goal

Turn existing Foundry and greenhouse content into difficult production decisions: choose
commissions, preserve food under pressure, and govern labor around the furnace. This plan does
not expand product or crop catalogs.

## Scope boundary

- Plan 129 owns foundry_production.json entries.
- Plan 91 owns greenhouse_items.json entries.
- Plan 55 owns new cooking/preservation recipes.
- This plan owns capacity, spoilage, and labor consequences that consume those catalogs. It must
  not add product, crop, or recipe rows.

## Task 22A — Foundry commissions and capacity trade-offs

1. Consume Plan 129 products as competing orders with material, heat, labor, and treaty
   requirements already owned by the Foundry systems.
2. Let the player prioritize shelter repair, expedition supply, trade, or defense, with visible
   opportunity cost and no invented currency.
3. Test order deferral, labor gates, output attribution, and save/load.

## Task 22B — Preservation, reserves, and blight response

1. Consume Plan 91 cultivars and Plan 55 preservation recipes to manage perishability, seasonal
   reserves, and blight recovery.
2. Make food-security outcomes visible through existing needs, storage, weather, and medical
   authorities; do not create a second crop-growth model.
3. Test a normal harvest, a blight, and a recovery reserve across save/load.

## Task 22C — Foundry faction politics and labor

1. Use existing foundry divisions and treaty-labor state to author disputes, work stoppages,
   resolution choices, and faction consequences.
2. Tie political outcomes to commission capacity rather than duplicating production definitions.
3. Test dispute triggers, strike/resolution behavior, and deterministic standing effects.

## Definition of Done

- Plans 91, 129, and 55 remain the only crop/product/recipe catalog owners.
- Production decisions use one set of Foundry, food, and labor authorities.
- Labor and food-security consequences are visible, persistent, and tested.
