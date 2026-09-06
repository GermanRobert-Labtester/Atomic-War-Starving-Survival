# Plan B76 — Aeroponics closeout

Status: implemented in the current Godot host.

## Delivered

- `AeroponicsSystem` owns chamber chemistry, misting, light mode, power availability, nozzle wear, root disease, growth and harvest readiness.
- Harvest output enters canonical `Inventory.Inventory`.
- `aeroponics_nutrient_catalog.json`, aeroponic harvest items and nutrient-batch recipe are authoritative JSON.
- `AeroponicsHostSession` is restored through the `aeroponics` campaign envelope section.
- `AeroponicsPanel` exposes chamber creation, planting, watering and harvest actions.
- Host watering consumes clean water before applying the chamber mutation.

## Verification

- `Plans74To77SystemsTests.Aeroponics_GrowsAndReturnsHarvestToCanonicalInventory`
- Core and Godot host builds pass.
- Data-integrity and content-utilization gates pass.

Known limitation: the current host exposes a clean-water cost for panel watering; detailed fertilizer stock accounting remains owned by the existing crafting/inventory path.
