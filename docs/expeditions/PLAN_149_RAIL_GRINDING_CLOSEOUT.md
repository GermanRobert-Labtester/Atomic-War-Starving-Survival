# Plan 149 Closeout: Rail Grinding & Track Corridor Rehabilitation

**Plan ID:** AF-149
**System:** Rail Grinding & Reprofiling Module
**Namespace:** `Ashfall.Core.Expeditions`, `AtomicWar.GodotApp.UI`
**Status:** Completed & Verified

---

## 1. Implemented Components

1. **Core Simulation:**
   - `Assets/Ashfall.Core/Expeditions/RailGrindingEngine.cs`
   - Mechanical reprofiling of weathered, corrugated steel tracks along railway corridors.
   - Reduction of roughness index and upgrade of safe operating speeds (e.g. from 25 km/h up to 60+ km/h).
   - Abrasive corundum grinding stone diameter wear (250mm down to 120mm replacement threshold).
   - Water misting spark suppression to prevent roadside dry fallout fires.
   - Mutates `RouteInfrastructureSystem` (`RailSegmentCondition`, `RoughnessIndex`, `SafeSpeedLimitKph`).

2. **Data Authority:**
   - `Assets/StreamingAssets/Data/rail_grinding_catalog.json` (schema_version: 1)
   - Catalog entries: `grinding_head_corundum_m4`, `grinding_head_diamond_m2`.
   - Items added to `items.json`: `item_corundum_grinding_stone`, `item_grinder_drive_belt`, `item_misting_water_tank`, `item_rail_profile_gauge`, `item_scrap_metal_swarf`.

3. **Host Session & Persistence:**
   - `src/Host/RailGrindingHostSession.cs`
   - `src/Host/RailGrindingSaveStore.cs` (registered in `SaveSectionRegistry`)

4. **Godot UI Panel:**
   - `src/UI/RailGrindingPanel.cs`
   - Dashboard shell layout, status rail with stone diameter, downforce, and corridor speed limits, 4-column data grid, and grinding pass detail frame.

5. **Verification & Tests:**
   - `Ashfall.Core.Tests/Expeditions/RailGrindingEngineTests.cs`
   - CLI flags: `--rail-grinding-uitest`, `--rail-grinding-selftest`.
