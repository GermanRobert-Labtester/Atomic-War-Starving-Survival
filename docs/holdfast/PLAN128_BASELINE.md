# Plan 128 Baseline Documentation

## Overview
- **Plan:** Plan 128 — Holdfast Flavor Factions Expansion (3 → 8 factions)
- **Target File:** `Assets/StreamingAssets/Data/holdfast_flavor.json`
- **Mirror Target:** `builds/linux/Assets/StreamingAssets/Data/holdfast_flavor.json`
- **Consumer:** `src/Host/HoldfastFlavorCatalog.cs` & `src/Host/HoldfastDispatchLog.cs`
- **UI Terminal:** `src/Host/HoldfastTerminalPanel.cs`

## Baseline State
- **Root Object:** `{ "schema_version": 1, "factions": { ... }, "items": { ... } }`
- **Baseline Faction Count:** 3 (`faction_the_office`, `faction_the_cutters`, `faction_the_fleet`)
- **Baseline Item Marginalia Count:** 40 entries (`item_map_sheet_ice_road` through `item_electrolyte_salts`)
- **Baseline Test Suite:** 10,031 tests passing in `Ashfall.Core.Tests`
- **Data Integrity Baseline:** 298 catalogs verified, 0 errors, 0 warnings
- **Holdfast Demo Baseline:** 25/25 checks passing (`--holdfast-selftest`)
- **Scene Binding Baseline:** 25/25 scenes passing
- **Scene Lint Baseline:** 30 scenes, 0 errors, 0 warnings
- **Build Status:** 0 errors, 0 warnings in `Ashfall.csproj`
