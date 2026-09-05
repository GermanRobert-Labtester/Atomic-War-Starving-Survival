# Plan 146 Closeout: EB-PVD Thermal Barrier Coatings

**Plan ID:** AF-146
**System:** Electron Beam Physical Vapor Deposition (EB-PVD) Coater
**Namespace:** `Ashfall.Core.Shelter`, `AtomicWar.GodotApp.UI`
**Status:** Completed & Verified

---

## 1. Implemented Components

1. **Core Simulation:**
   - `Assets/Ashfall.Core/Shelter/EbPvdCoatingEngine.cs`
   - Dimensional beam power calculations: $P = V \times I$ (kV × A = kW).
   - Deposition kinetics based on vacuum chamber pressure, substrate rotation, and raster frequency.
   - Columnar ceramic microstructures (7YSZ, Gadolinium Zirconate, Alumina) with spallation risk modeling.
   - Brownout tolerance: automatic pause on power disruption with clean operator resumption.

2. **Data Authority:**
   - `Assets/StreamingAssets/Data/ebpvd_coating_catalog.json` (schema_version: 1)
   - Catalog entries: `ebpvd_tbc_yttria_stabilized_zirconia`, `ebpvd_tbc_gadolinium_zirconate`, `ebpvd_tbc_alumina_barrier`.
   - Items added to `items.json`: `item_ebpvd_ceramic_target_ingot`, `item_mcraly_bond_coat_powder`, `item_ebpvd_filament_tungsten`, `item_diffusion_pump_oil`, `item_coated_turbine_blade`.

3. **Host Session & Persistence:**
   - `src/Host/EbPvdCoatingHostSession.cs`
   - `src/Host/EbPvdCoatingSaveStore.cs` (registered in `SaveSectionRegistry`)

4. **Godot UI Panel:**
   - `src/UI/EbPvdCoatingPanel.cs`
   - Dashboard shell layout, status rail with vacuum and beam power telemetry, 4-column data grid, and process control detail frame.

5. **Verification & Tests:**
   - `Ashfall.Core.Tests/Shelter/EbPvdCoatingEngineTests.cs`
   - CLI flags: `--ebpvd-coating-uitest`, `--ebpvd-coating-selftest`.
